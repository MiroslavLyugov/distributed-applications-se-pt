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
    public async void TestArchiveBoard_BoardExists_Succeeds()
    {
        var board = GetValidBoard();
        _context.Add(board);
        _context.SaveChanges();

        // Test board deletion
        var response = await _service.ArchiveBoardAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm board marked as deleted
        var entry = await _context.Boards.SingleOrDefaultAsync(x => x.Name == board.Name);
        Assert.NotNull(entry);
        Assert.True(entry.Archived);
    }

    [Fact]
    public async void TestArchiveBoard_BoardNotExists_Fails()
    {
        var unused_id = 10;

        // Test board deletion
        var response = await _service.ArchiveBoardAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }


    [Fact]
    public async void TestArchiveBoard_AlreadyArchived_Fails()
    {
        var board = GetValidBoard();
        board.Archived = true;
        _context.Add(board);
        _context.SaveChanges();

        // Test board deletion
        var response = await _service.ArchiveBoardAsync(new(board.Id));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);
    }
}
