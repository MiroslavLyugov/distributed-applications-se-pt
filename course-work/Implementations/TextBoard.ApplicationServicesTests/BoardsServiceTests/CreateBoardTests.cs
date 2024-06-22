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
    public async void TestCreateBoard_ValidData_CreatesPublicBoard()
    {
        var board = GetValidBoard();

        // Test board creation
        var response = await _service.CreateBoardAsync(new(board.Name));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm board creation
        var entry = await _context.Boards.SingleOrDefaultAsync(x => x.Name == board.Name);
        Assert.NotNull(entry);
        Assert.False(entry.Hidden);
    }

    [Fact]
    public async void TestCreateBoard_ValidDataHidden_Succeeds()
    {
        var board = GetValidBoard();

        // Test board creation
        var response = await _service.CreateBoardAsync(new(board.Name, true));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm board creation
        var entry = await _context.Boards.SingleOrDefaultAsync(x => x.Name == board.Name);
        Assert.NotNull(entry);
        Assert.True(entry.Hidden);
    }

    [Fact]
    public async void TestCreateBoard_ShortName_Fails()
    {
        var board = GetValidBoard();
        board.Name = "";

        // Test board creation
        var response = await _service.CreateBoardAsync(new(board.Name));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm board not created
        var entry = await _context.Boards.SingleOrDefaultAsync(x => x.Name == board.Name);
        Assert.Null(entry);
    }

    [Fact]
    public async void TestCreateBoard_LongName_Fails()
    {
        var board = GetValidBoard();
        board.Name = new String('a', 512);

        // Test board creation
        var response = await _service.CreateBoardAsync(new(board.Name));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm board not created
        var entry = await _context.Boards.SingleOrDefaultAsync(x => x.Name == board.Name);
        Assert.Null(entry);
    }
}
