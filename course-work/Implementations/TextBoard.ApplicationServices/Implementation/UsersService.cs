using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Messaging;
using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;
using TextBoard.ApplicationServices.Messaging.ViewModels;
using TextBoard.Data.Contexts;
using TextBoard.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TextBoard.ApplicationServices.Implementation;

public class UsersService : IUsersService
{
    TextBoardDbContext _context;
    IMapper _mapper;

    public UsersService(TextBoardDbContext context)
    {
        _context = context;
        _mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserVM>()).CreateMapper();
    }


    public async Task<ListUsersResponse> ListUsersAsync(ListUsersRequest request)
    {
        var list = await _context.Users.Where(x => !x.Deleted).Select(x => _mapper.Map<UserVM>(x)).ToListAsync();

        if(request.PageSize < 0)
            return new(list);
        return new(list.Skip(request.Page * request.PageSize).Take(request.PageSize).ToList(), request.Page, request.PageSize);
    }


    public async Task<GetUserResponse> GetUserByNameAsync(GetUserByNameRequest request)
    {
        var user = await _context.Users.Where(x => !x.Deleted).SingleOrDefaultAsync(x => x.Name == request.Name);
        return new(_mapper.Map<UserVM>(user));
    }

    public async Task<GetUserResponse> GetUserByIdAsync(GetUserByIdRequest request)
    {
        var user = await _context.Users.Where(x => !x.Deleted).SingleOrDefaultAsync(x => x.Id == request.Id);
        return new(_mapper.Map<UserVM>(user));
    }


    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        if(request.Name.Length < 3)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be at least 3 characters");
        if(request.Name.Length > 24)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be no more than 24 characters");
        if(request.Password.Length < 3)
            return new(BusinessStatusCodeEnum.InvalidInput, "Password must be at least 3 characters");
        if(request.Password.Length > 24)
            return new(BusinessStatusCodeEnum.InvalidInput, "Password must be no more than 24 characters");

        var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == request.Name);

        if(user == null)
        {
            User entry = new () {
                Name = request.Name,
                Password = request.Password
            };
            _context.Add(entry);
            await _context.SaveChangesAsync();
            return new(entry.Id);
        }

        if(user.Deleted)
            return new(BusinessStatusCodeEnum.Reserved, "Name reserved.");

        return new(BusinessStatusCodeEnum.AlreadyExists, "Name already in use.");
    }


    public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        if(request.Name != null && request.Name.Length < 3)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be at least 3 characters");
        if(request.Name != null && request.Name.Length > 24)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be no more than 24 characters");
        if(request.Password != null && request.Password.Length < 3)
            return new(BusinessStatusCodeEnum.InvalidInput, "Password must be at least 3 characters");
        if(request.Password != null && request.Password.Length > 24)
            return new(BusinessStatusCodeEnum.InvalidInput, "Password must be no more than 24 characters");

        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id);

        if(user == null || user.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");

        if(request.Name != null)
            user.Name = request.Name;
        if(request.Password != null)
            user.Password = request.Password;

        await _context.SaveChangesAsync();

        return new(user.Id);
    }


    public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id);
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User not found.");
        if(user.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "User not found");

        user.Deleted = true;
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "User deleted.");
    }



    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == request.Name);
        if(user == null || user.Deleted)
            return new(BusinessStatusCodeEnum.AuthenticationFailed, "User not found!");


        if(user.Password != request.Password)
            return new(BusinessStatusCodeEnum.AuthenticationFailed, "Wrong password!");

        user.LastLogin = DateTime.Now;
        await _context.SaveChangesAsync();

        string token = GenerateJwtTokenInternal(user);

        return new(token, "Authentication successfull.");
    }


    public async Task<GenerateJwtTokenResponse> GenerateJwtTokenAsync(GetUserByIdRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id);
        if(user == null || user.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "User not found!");

        string token = GenerateJwtTokenInternal(user);

        return new(token, "Token generated successfully.");
    }

    private string GenerateJwtTokenInternal(User user)
    {
        var claims = new[]
        {
            new Claim("LoggedUserId", user.Id.ToString())
        };

        var signingKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("fb5e1f5a4d20490fb3351dd5dbfdc4a7")
        );

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "TextBoard", // issuer
            "TextBoard", // audience
            claims,
            expires: DateTime.UtcNow.AddMinutes(90),
            signingCredentials: signingCredentials
        );


        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}

