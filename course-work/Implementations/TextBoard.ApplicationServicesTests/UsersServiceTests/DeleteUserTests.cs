using Microsoft.EntityFrameworkCore;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Implementation;
using TextBoard.ApplicationServices.Messaging;
using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;
using TextBoard.Data.Models;
using TextBoard.DataMock.Contexts;

namespace TextBoard.ApplicationServicesTests;

public partial class UsersServiceTests
{
    [Fact]
    public async void TestDeleteUser_UserExists_Succeeds()
    {
        var user = GetValidUser();
        _context.Add(user);
        _context.SaveChanges();

        // Test user deletion
        var response = await _service.DeleteUserAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm user marked as deleted
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.NotNull(entry);
        Assert.True(entry.Deleted);
    }

    [Fact]
    public async void TestDeleteUser_UserNotExists_Fails()
    {
        var unused_id = 10;

        // Test user deletion
        var response = await _service.DeleteUserAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }


    [Fact]
    public async void TestDeleteUser_AlreadyDeleted_Fails()
    {
        var user = GetValidUser();
        user.Deleted = true;
        _context.Add(user);
        _context.SaveChanges();

        // Test user deletion
        var response = await _service.DeleteUserAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
    }
}
