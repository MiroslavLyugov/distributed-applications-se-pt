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
    public async void ListBoards_NoBoards_EmptyResponse()
    {
        var response = await _service.ListBoardsAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }

    [Fact]
    public async void ListBoards_OneBoard_ReturnsBoardModelList()
    {
        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.ListBoardsAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Single(response.Boards);
        Assert.Equal(board.Id, response.Boards.FirstOrDefault()?.Id);
    }

    [Fact]
    public async void ListBoards_MultipleBoards_ReturnsBoardModelList()
    {
        var board = GetValidBoard();
        var board2 = GetValidBoard();
        board2.Name += "2";
        _context.Boards.Add(board);
        _context.Boards.Add(board2);
        _context.SaveChanges();

        var response = await _service.ListBoardsAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(2, response.Boards.Count);
    }

    [Fact]
    public async void ListBoards_PagedOutOfBounds_ReturnsEmptyList()
    {
        var page = 4;
        var size = 1;

        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.ListBoardsAsync(new(page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }

    [Fact]
    public async void ListBoards_Paged_ReturnsListOfSize()
    {
        var page = 4;
        var size = 2;

        for(int i = 0; i < 20; i++)
        {
            var board = GetValidBoard();
            board.Name += $"{i}";
            _context.Boards.Add(board);
        }
        _context.SaveChanges();

        var response = await _service.ListBoardsAsync(new(page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(size, response.Boards.Count);
    }

    [Fact]
    public async void ListBoards_Paged_SequenceTest()
    {
        int count = 20;
        for(int i = 0; i < 20; i++)
        {
            var board = GetValidBoard();
            board.Name += $"{i}";
            _context.Boards.Add(board);
        }
        _context.SaveChanges();

        var psize = 2;
        int id_counter = 1;
        for(int page = 0; page < count / psize; page++)
        {
            var response = await _service.ListBoardsAsync(new(page, psize));
            Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
            Assert.Equal(psize, response.Boards.Count);
            foreach(var board in response.Boards)
            {
                Assert.Equal(id_counter++, board.Id);
            }
        }
        Assert.Equal(count, id_counter - 1);
    }

    [Fact]
    public async void ListBoards_DeletedBoard_EmptyResponse()
    {
        var board = GetValidBoard();
        board.Deleted = true;
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.ListBoardsAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }

    [Fact]
    public async void ListBoards_HiddenBoard_EmptyResponse()
    {
        var board = GetValidBoard();
        board.Hidden = true;
        _context.Boards.Add(board);
        _context.SaveChanges();

        var response = await _service.ListBoardsAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }
}
