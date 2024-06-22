using Microsoft.EntityFrameworkCore;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Implementation;
using TextBoard.ApplicationServices.Messaging;
using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;
using TextBoard.Data.Models;
using TextBoard.DataMock.Contexts;

namespace TextBoard.ApplicationServicesTests;

public partial class MessagesServiceTests
{
    [Fact]
    public async void TestGetMessageById_ValidMessage_Succeeds()
    {
        var message = PopulateValidMessage();

        var response = await _service.GetMessageByIdAsync(new(message.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.Message);

        Assert.Equal(message.Id, response.Message.Id);
        Assert.Equal(message.User.Id, response.Message.UserId);
        Assert.Equal(message.Board.Id, response.Message.BoardId);
        Assert.Equal(message.Content, response.Message.Content);
        Assert.Equal(message.CreateTime, response.Message.CreateTime);
        Assert.Equal(message.DeleteEvent, response.Message.DeleteEvent);
        if(message.OverrideMessage != null)
            Assert.Equal(message.OverrideMessage.Id, response.Message.OverrideMessageId);
    }

    [Fact]
    public async void TestGetMessageById_MessageNotExists_Fails()
    {
        int unused_id = 10;

        var response = await _service.GetMessageByIdAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Message);
    }
}
