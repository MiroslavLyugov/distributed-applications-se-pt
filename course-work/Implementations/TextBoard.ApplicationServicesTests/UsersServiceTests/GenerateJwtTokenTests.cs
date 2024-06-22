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
    public async void TestGenerateJwtToken_Succeeds()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GenerateJwtTokenAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.BearerToken);

        TestJwtToken(user, response.BearerToken);
    }

    [Fact]
    public async void TestGenerateJwtToken_NotExists_Fails()
    {
        var user = GetValidUser();

        var response = await _service.GenerateJwtTokenAsync(new(user.Id));
        Assert.NotEqual(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotEqual(BusinessStatusCodeEnum.NotImplemented, response.StatusCode);
        Assert.Null(response.BearerToken);
    }

    [Fact]
    public async void TestGenerateJwtToken_UserDeleted_Fails()
    {
        var user = GetValidUser();
        user.Deleted = true;
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GenerateJwtTokenAsync(new(user.Id));
        Assert.NotEqual(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotEqual(BusinessStatusCodeEnum.NotImplemented, response.StatusCode);
        Assert.Null(response.BearerToken);
    }
}
