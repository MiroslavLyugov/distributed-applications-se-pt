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
    [Fact]
    public async void TestGetBoardById_ValidBoard_Succeeds()
    {
        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.GetBoardByIdAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.Board);

        Assert.Equal(board.Id, response.Board.Id);
        Assert.Equal(board.Name, response.Board.Name);
        Assert.Equal(board.Hidden, response.Board.Hidden);
        Assert.Equal(board.Archived, response.Board.Archived);
        Assert.Equal(board.CreateTime, response.Board.CreateTime);
    }

    [Fact]
    public async void TestGetBoardById_BoardNotExists_Fails()
    {
        int unused_id = 10;

        var response = await _service.GetBoardByIdAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Board);
    }

    [Fact]
    public async void TestGetBoardById_BoardDeleted_Fails()
    {
        var board = GetValidBoard();
        board.Deleted = true;
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.GetBoardByIdAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Board);
    }


    [Fact]
    public async void TestGetBoardByName_ValidBoard_Succeeds()
    {
        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.GetBoardByNameAsync(new(board.Name));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.Board);

        Assert.Equal(board.Id, response.Board.Id);
        Assert.Equal(board.Name, response.Board.Name);
        Assert.Equal(board.Hidden, response.Board.Hidden);
        Assert.Equal(board.Archived, response.Board.Archived);
        Assert.Equal(board.CreateTime, response.Board.CreateTime);
    }

    [Fact]
    public async void TestGetBoardByName_BoardNotExists_Fails()
    {
        string unused_name = "Rambley";

        var response = await _service.GetBoardByNameAsync(new(unused_name));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Board);
    }

    [Fact]
    public async void TestGetBoardByName_BoardDeleted_Fails()
    {
        var board = GetValidBoard();
        board.Deleted = true;
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.GetBoardByNameAsync(new(board.Name));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Board);
    }
}
