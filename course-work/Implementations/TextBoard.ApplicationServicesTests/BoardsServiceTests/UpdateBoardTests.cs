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
    public async void TestUpdateBoard_ValidData_Succeeds()
    {
        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var name = board.Name + "_";
        var hidden = !board.Hidden;
        var archived = !board.Archived;

        // Test board update
        var response = await _service.UpdateBoardAsync(new(board.Id, name, hidden, archived));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm changes
        Assert.Equal(name, board.Name);
        Assert.Equal(hidden , board.Hidden);
        Assert.Equal(archived, board.Archived);
    }

    [Fact]
    public async void TestUpdateBoard_ShortName_Fails()
    {
        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var name = "";

        // Test board update
        var response = await _service.UpdateBoardAsync(new(board.Id, name));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm no changes
        Assert.NotEqual(name, board.Name);
    }

    [Fact]
    public async void TestUpdateBoard_LongName_Fails()
    {
        var board = GetValidBoard();
        _context.Boards.Add(board);
        _context.SaveChanges();

        var name = new String('a', 512);

        // Test board update
        var response = await _service.UpdateBoardAsync(new(board.Id, name));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm no changes
        Assert.NotEqual(name, board.Name);
    }

    [Fact]
    public async void TestUpdateBoard_UnchangedData_DoNotDeleteData()
    {
        var board = GetValidBoard();
        board.Hidden = true;
        board.Archived = true;
        _context.Boards.Add(board);
        _context.SaveChanges();

        // Test board update
        var response = await _service.UpdateBoardAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm no changes
        Assert.NotNull(board.Name);
        Assert.True(board.Hidden);
        Assert.True(board.Archived);
    }

    [Fact]
    public async void TestUpdateBoard_NotExists_Fails()
    {
        var board = GetValidBoard();
        var unused_id = 10;

        // Test board creation
        var response = await _service.UpdateBoardAsync(new(unused_id, board.Name));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        // Assert no database modifications
        Assert.Empty(_context.Boards.ToList());
    }

}
