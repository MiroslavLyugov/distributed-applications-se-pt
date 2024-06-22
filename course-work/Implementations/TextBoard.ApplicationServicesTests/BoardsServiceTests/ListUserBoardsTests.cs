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
    public async void ListUserBoards_UserNotExists_Fails()
    {
        var unused_id = 10;

        var response = await _service.ListUserBoardsAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void ListUserBoards_UserDeleted_Fails()
    {
        var user = UsersServiceTests.GetValidUser();
        user.Deleted = true;
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }

    [Fact]
    public async void ListUserBoards_NoBoards_EmptyResponse()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }

    [Fact]
    public async void ListUserBoards_OneBoard_ReturnsBoardModelList()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);

        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var message = MessagesServiceTests.GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Single(response.Boards);
        Assert.Equal(board.Id, response.Boards.FirstOrDefault()?.Id);
    }

    [Fact]
    public async void ListUserBoards_MultipleBoards_ReturnsBoardModelList()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);

        var board = GetValidBoard();
        var board2 = GetValidBoard();
        board2.Name += "2";
        _context.Boards.Add(board);
        _context.Boards.Add(board2);
        _context.SaveChanges();

        var message = MessagesServiceTests.GetValidMessage(user, board);
        var message2 = MessagesServiceTests.GetValidMessage(user, board2);
        _context.Add(message);
        _context.Add(message2);
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(2, response.Boards.Count);
    }

    [Fact]
    public async void ListUserBoards_PagedOutOfBounds_ReturnsEmptyList()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);

        var page = 4;
        var size = 1;

        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var message = MessagesServiceTests.GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id, page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }

    [Fact]
    public async void ListUserBoards_Paged_ReturnsListOfSize()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var page = 4;
        var size = 2;

        for(int i = 0; i < 20; i++)
        {
            var board = GetValidBoard();
            board.Name += $"{i}";
            _context.Boards.Add(board);

            var message = MessagesServiceTests.GetValidMessage(user, board);
            _context.Add(message);
            _context.SaveChanges();
        }
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id, page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(size, response.Boards.Count);
    }

    [Fact]
    public async void ListUserBoards_Paged_SequenceTest()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);

        int count = 20;
        for(int i = 0; i < 20; i++)
        {
            var board = GetValidBoard();
            board.Name += $"{i}";
            _context.Boards.Add(board);

            var message = MessagesServiceTests.GetValidMessage(user, board);
            _context.Add(message);
        }
        _context.SaveChanges();

        var psize = 2;
        int id_counter = 1;
        for(int page = 0; page < count / psize; page++)
        {
            var response = await _service.ListUserBoardsAsync(new(user.Id, page, psize));
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
    public async void ListUserBoards_DeletedBoard_EmptyResponse()
    {
        var user = UsersServiceTests.GetValidUser();
        _context.Users.Add(user);

        var board = GetValidBoard();
        board.Deleted = true;
        _context.Boards.Add(board);
        _context.SaveChanges();

        var message = MessagesServiceTests.GetValidMessage(user, board);
        _context.Add(message);
        _context.SaveChanges();

        var response = await _service.ListUserBoardsAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Boards);
    }
}
