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
    public async void TestCreateUser_ValidData_Succeeds()
    {
        var user = GetValidUser();

        // Test user creation
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm user creation
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.NotNull(entry);
    }

    [Fact]
    public async void TestCreateUser_ShortName_Fails()
    {
        var user = GetValidUser();
        user.Name = "";

        // Test user creation
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm user not created
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.Null(entry);
    }

    [Fact]
    public async void TestCreateUser_LongName_Fails()
    {
        var user = GetValidUser();
        user.Name = new String('a', 512);

        // Test user creation
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm user not created
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.Null(entry);
    }

    [Fact]
    public async void TestCreateUser_ShortPassword_Fails()
    {
        var user = GetValidUser();
        user.Password = "42";

        // Test user creation
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm user not created
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.Null(entry);
    }

    [Fact]
    public async void TestCreateUser_LongPassword_Fails()
    {
        var user = GetValidUser();
        user.Password = new String('a', 512);

        // Test user creation
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.InvalidInput, response.StatusCode);

        // Confirm user not created
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.Null(entry);
    }


    [Fact]
    public async void TestCreateUser_AlreadyExists_Fails()
    {
        var user = GetValidUser();

        // Test user creation
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);

        // Confirm user creation
        var entry = await _context.Users.SingleOrDefaultAsync(x => x.Name == user.Name);
        Assert.NotNull(entry);


        // Test user creation again
        var response2 = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.AlreadyExists, response2.StatusCode);

        // Confirm user not created
        Assert.Single(_context.Users.ToList());
    }

    [Fact]
    public async void TestCreateUser_AlreadyDeleted_Fails()
    {
        var user = GetValidUser();
        user.Deleted = true;
        _context.Users.Add(user);
        _context.SaveChanges();

        // Test user creation again
        var response = await _service.CreateUserAsync(new(user.Name, user.Password));
        Assert.Equal(BusinessStatusCodeEnum.Reserved, response.StatusCode);

        // Confirm user not created
        Assert.Single(_context.Users.ToList());
    }
}
