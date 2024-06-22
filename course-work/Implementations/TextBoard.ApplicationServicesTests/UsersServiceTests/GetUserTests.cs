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
    public async void TestGetUserById_ValidUser_Succeeds()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByIdAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.User);

        Assert.Equal(user.Id, response.User.Id);
        Assert.Equal(user.Name, response.User.Name);
        Assert.Equal(user.IsAdmin, response.User.IsAdmin);
        Assert.Equal(user.CreateTime, response.User.CreateTime);
        Assert.Equal(user.LastLogin, response.User.LastLogin);
    }

    [Fact]
    public async void TestGetUserById_UserNotExists_Fails()
    {
        int unused_id = 10;

        var response = await _service.GetUserByIdAsync(new(unused_id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.User);
    }

    [Fact]
    public async void TestGetUserById_UserDeleted_Fails()
    {
        var user = GetValidUser();
        user.Deleted = true;
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByIdAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.User);
    }


    [Fact]
    public async void TestGetUserByName_ValidUser_Succeeds()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByNameAsync(new(user.Name));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.NotNull(response.User);

        Assert.Equal(user.Id, response.User.Id);
        Assert.Equal(user.Name, response.User.Name);
        Assert.Equal(user.IsAdmin, response.User.IsAdmin);
        Assert.Equal(user.CreateTime, response.User.CreateTime);
        Assert.Equal(user.LastLogin, response.User.LastLogin);
    }

    [Fact]
    public async void TestGetUserByName_UserNotExists_Fails()
    {
        string unused_name = "Rambley";

        var response = await _service.GetUserByNameAsync(new(unused_name));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.User);
    }

    [Fact]
    public async void TestGetUserByName_UserDeleted_Fails()
    {
        var user = GetValidUser();
        user.Deleted = true;
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.GetUserByNameAsync(new(user.Name));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.User);
    }
}
