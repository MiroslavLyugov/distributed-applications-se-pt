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
    private TextBoardDbContextMock _context;
    private IUsersService _service;

    public UsersServiceTests()
    {
        _context = new();
        _service = new UsersService(_context);
    }

    public static User GetValidUser()
    {
        return new()
        {
            Name = "User",
            Password = "Password"
        };
    }
}
