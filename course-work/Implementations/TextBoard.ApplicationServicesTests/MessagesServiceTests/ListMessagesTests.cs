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
    public async void ListBoardMessages_BoardNotExists_Fails()
    {
        var unused_id = 10;

        var response = await _service.ListBoardMessagesAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void ListBoardMessagess_BoardDeleted_Fails()
    {
        var board = BoardsServiceTests.GetValidBoard();
        board.Deleted = true;
        _context.Add(board);
        _context.SaveChanges();

        var response = await _service.ListBoardMessagesAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void ListBoardMessages_NoMessages_EmptyResponse()
    {
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(board);
        _context.SaveChanges();

        var response = await _service.ListBoardMessagesAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Messages);
    }

    [Fact]
    public async void ListBoardMessages_OneMessage_ReturnsMessageModelList()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        var response = await _service.ListBoardMessagesAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Single(response.Messages);
        Assert.Equal(message.Id, response.Messages.FirstOrDefault()?.Id);
    }

    [Fact]
    public async void ListBoardMessages_MultipleMessage_ReturnsMessageModelList()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        var message2 = GetValidMessage(user, board);
        _context.Add(message);
        _context.Add(message2);
        _context.SaveChanges();

        var response = await _service.ListBoardMessagesAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(2, response.Messages.Count);
    }

    [Fact]
    public async void ListBoardMessagesPaged_OutOfRange_ReturnsEmptyList()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        var page = 4;
        var size = 1;

        var response = await _service.ListBoardMessagesAsync(new(board.Id, page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Messages);
    }
}
