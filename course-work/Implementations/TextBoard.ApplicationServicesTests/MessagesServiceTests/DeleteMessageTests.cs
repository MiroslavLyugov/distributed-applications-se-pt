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
    public async void TestDeleteMessage_MessageExists_Succeeds()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        // Test board deletion
        var response = await _service.DeleteMessageAsync(new(message.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm message delete event created
        var entry = await _context.Messages.SingleOrDefaultAsync(x => x.OverrideMessage == message);
        Assert.NotNull(entry);
        Assert.True(entry.DeleteEvent);
    }

    [Fact]
    public async void TestDeleteMessage_MessageNotExists_Fails()
    {
        var unused_id = 10;

        // Test message deletion
        var response = await _service.DeleteMessageAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }


    [Fact]
    public async void TestDeleteMessage_AlreadyDeleted_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();
        var delete_event = new Message(){
            Board = message.Board,
            User = message.User,
            Content = "Deleted for testing purposes",
            DeleteEvent = true,
            OverrideMessage = message
        };
        _context.Add(delete_event);
        _context.SaveChanges();

        // Test message deletion
        var response = await _service.DeleteMessageAsync(new(message.Id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        // Assert no new delete events created in the chain
        Assert.Equal(2, _context.Messages.ToList().Count());
    }
}
