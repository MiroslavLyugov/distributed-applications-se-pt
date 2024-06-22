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
    private TextBoardDbContextMock _context;
    private IMessagesService _service;

    public MessagesServiceTests()
    {
        _context = new();
        _service = new MessagesService(_context);
    }

    public Message PopulateValidMessage()
    {
        User user = UsersServiceTests.GetValidUser();
        Board board = BoardsServiceTests.GetValidBoard();
        _context.Add(user);
        _context.Add(board);
        var message = GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();
        return message;
    }

    public static Message GetValidMessage(User user, Board board)
    {
        return new()
        {
            User = user,
            Board = board,
            Content = "This is an example of a text message"
        };
    }
}
