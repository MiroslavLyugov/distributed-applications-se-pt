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
    public async void TestReplaceMessage_ValidData_CreatesMessage()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();

        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        // Test message creation
        var response = await _service.ReplaceMessageAsync(new(message.Id, message.Content + "_"));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm message creation
        var entries = await _context.Messages.Where(x => x.User == user && x.Board == board).ToListAsync();
        Assert.NotEmpty(entries);
        Assert.Equal(message.Content, entries[0].Content);
        Assert.Equal(entries[1].OverrideMessage, entries[0]);
        Assert.Equal(message.Content + "_", entries[1].Content);
    }

    [Fact]
    public async void TestReplaceMessage_ShortContent_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();

        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        // Test message creation
        var response = await _service.ReplaceMessageAsync(new(message.Id, ""));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm message not created
        Assert.Single(_context.Messages.ToList());
    }

    [Fact]
    public async void TestReplaceMessage_LongContent_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();

        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();
        var long_content = new String('a', 32768);

        // Test message creation
        var response = await _service.ReplaceMessageAsync(new(message.Id, long_content));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm message not created
        Assert.Single(_context.Messages.ToList());
    }

    [Fact]
    public async void TestReplaceMessage_InvalidMessage_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        var board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        _context.SaveChanges();

        var message = GetValidMessage(user, board);
        var unused_id = 10;

        // Test message creation
        var response = await _service.ReplaceMessageAsync(new(unused_id, message.Content));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        // Confirm message not created
        Assert.Empty(_context.Messages.ToList());
    }
}
