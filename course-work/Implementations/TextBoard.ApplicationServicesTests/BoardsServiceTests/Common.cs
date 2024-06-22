using Microsoft.EntityFrameworkCore;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Implementation;
using TextBoard.ApplicationServices.Messaging;
using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;
using TextBoard.Data.Models;
using TextBoard.DataMock.Contexts;

namespace TextBoard.ApplicationServicesTests;

public partial class BoardsServiceTests
{
    private TextBoardDbContextMock _context;
    private IBoardsService _service;

    public BoardsServiceTests()
    {
        _context = new();
        _service = new BoardsService(_context);
    }

    public static Board GetValidBoard()
    {
        return new()
        {
            Name = "Board"
        };
    }
}
