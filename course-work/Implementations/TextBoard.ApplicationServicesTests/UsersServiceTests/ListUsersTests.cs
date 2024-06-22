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
    public async void ListUsers_NoUsers_EmptyResponse()
    {
        var response = await _service.ListUsersAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Users);
    }

    [Fact]
    public async void ListUsers_OneUser_ReturnsUserModelList()
    {
        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.ListUsersAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Single(response.Users);
        Assert.Equal(user.Id, response.Users.FirstOrDefault()?.Id);
    }

    [Fact]
    public async void ListUsers_MultipleUsers_ReturnsUserModelList()
    {
        var user = GetValidUser();
        var user2 = GetValidUser();
        user2.Name += "2";
        _context.Users.Add(user);
        _context.Users.Add(user2);
        _context.SaveChanges();

        var response = await _service.ListUsersAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(2, response.Users.Count);
    }

    [Fact]
    public async void ListUsers_PagedOutOfBounds_ReturnsEmptyList()
    {
        var page = 4;
        var size = 1;

        var user = GetValidUser();
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.ListUsersAsync(new(page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Users);
    }

    [Fact]
    public async void ListUsers_Paged_ReturnsListOfSize()
    {
        var page = 4;
        var size = 2;

        for(int i = 0; i < 20; i++)
        {
            var user = GetValidUser();
            user.Name += $"{i}";
            _context.Users.Add(user);
        }
        _context.SaveChanges();

        var response = await _service.ListUsersAsync(new(page, size));
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Equal(size, response.Users.Count);
    }

    [Fact]
    public async void ListUsers_Paged_SequenceTest()
    {
        int count = 20;
        for(int i = 0; i < 20; i++)
        {
            var user = GetValidUser();
            user.Name += $"{i}";
            _context.Users.Add(user);
        }
        _context.SaveChanges();

        var psize = 2;
        int id_counter = 1;
        for(int page = 0; page < count / psize; page++)
        {
            var response = await _service.ListUsersAsync(new(page, psize));
            Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
            Assert.Equal(psize, response.Users.Count);
            foreach(var user in response.Users)
            {
                Assert.Equal(id_counter++, user.Id);
            }
        }
        Assert.Equal(count, id_counter - 1);
    }

    [Fact]
    public async void ListUsers_DeletedUser_EmptyResponse()
    {
        var user = GetValidUser();
        user.Deleted = true;
        _context.Users.Add(user);
        _context.SaveChanges();

        var response = await _service.ListUsersAsync(new());
        Assert.Equal(BusinessStatusCodeEnum.Success, response.StatusCode);
        Assert.Empty(response.Users);
    }
}
