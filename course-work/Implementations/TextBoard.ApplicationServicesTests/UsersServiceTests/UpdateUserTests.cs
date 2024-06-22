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
    public async void TestUpdateUser_ValidData_Succeeds()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var name = user.Name + "_";
        var pwd = user.Password + "_";

        // Test user update
        var response = await _service.UpdateUserAsync(new(user.Id, name, pwd));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm changes
        Assert.Equal(name, user.Name);
        Assert.Equal(pwd, user.Password);
    }

    [Fact]
    public async void TestUpdateUser_ShortName_Fails()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var name = "a";

        // Test user update
        var response = await _service.UpdateUserAsync(new(user.Id, name));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm no changes
        Assert.NotEqual(name, user.Name);
    }

    [Fact]
    public async void TestUpdateUser_LongName_Fails()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var name = new String('a', 512);

        // Test user update
        var response = await _service.UpdateUserAsync(new(user.Id, name));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm no changes
        Assert.NotEqual(name, user.Name);
    }

    [Fact]
    public async void TestUpdateUser_UnchangedData_DoNotDeleteData()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        // Test user update
        var response = await _service.UpdateUserAsync(new(user.Id));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm no changes
        Assert.NotNull(user.Name);
        Assert.NotNull(user.Password);
    }

    [Fact]
    public async void TestUpdateUser_ShortPassword_Fails()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var pwd = "42";

        // Test user update
        var response = await _service.UpdateUserAsync(new(user.Id, null, pwd));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm no changes
        Assert.NotEqual(pwd, user.Password);
    }

    [Fact]
    public async void TestUpdateUser_LongPassword_Fails()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var pwd = new String('a', 512);

        // Test user update
        var response = await _service.UpdateUserAsync(new(user.Id, null, pwd));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm no changes
        Assert.NotEqual(pwd, user.Password);
    }


    [Fact]
    public async void TestUpdateUser_NotExists_Fails()
    {
        var user = GetValidUser();
        var unused_id = 10;

        // Test user creation
        var response = await _service.UpdateUserAsync(new(unused_id, user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.NotFound, response.StatusCode);

        // Assert no database modifications
        Assert.Empty(_context.Users.ToList());
    }

}
