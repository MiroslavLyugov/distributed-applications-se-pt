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
    public async void TestCreateMessage_ValidData_CreatesMessage()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);

        // Test message creation
        var response = await _service.CreateMessageAsync(new(user.Id, board.Id, message.Content));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm message creation
        var entry = await _context.Messages.SingleOrDefaultAsync(x => x.User == user && x.Board == board);
        Assert.NotNull(entry);
        Assert.Equal(message.Content, entry.Content);
    }

    [Fact]
    public async void TestCreateMessage_ShortContent_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        message.Content = "";

        // Test message creation
        var response = await _service.CreateMessageAsync(new(user.Id, board.Id, message.Content));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm message not created
        Assert.Empty(_context.Messages.ToList());
    }

    [Fact]
    public async void TestCreateMessage_LongContent_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);
        message.Content = new String('a', 32768);

        // Test message creation
        var response = await _service.CreateMessageAsync(new(user.Id, board.Id, message.Content));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm message not created
        Assert.Empty(_context.Messages.ToList());
    }

    [Fact]
    public async void TestCreateMessage_InvalidUser_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);

        // Test message creation
        var response = await _service.CreateMessageAsync(new(user.Id + 5, board.Id, message.Content));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        // Confirm message not created
        Assert.Empty(_context.Messages.ToList());
    }

    [Fact]
    public async void TestCreateMessage_InvalidBoard_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();
        var message = GetValidMessage(user, board);

        // Test message creation
        var response = await _service.CreateMessageAsync(new(user.Id, board.Id + 5, message.Content));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        // Confirm message not created
        Assert.Empty(_context.Messages.ToList());
    }
}
