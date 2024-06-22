using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Implementation;
using TextBoard.ApplicationServices.Messaging;
using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;
using TextBoard.Data.Models;
using TextBoard.DataMock.Contexts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace TextBoard.ApplicationServicesTests;

public partial class UsersServiceTests
{
    private void TestJwtToken(User user, string token)
    {
        var signingKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("fb5e1f5a4d20490fb3351dd5dbfdc4a7")
        );
        
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = "TextBoard",
            ValidateAudience = true,
            ValidAudience = "TextBoard",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        JwtSecurityToken jwt = (JwtSecurityToken) validatedToken;

        string? id = jwt.Claims.SingleOrDefault(u => u.Type == "LoggedUserId")?.Value;
        Assert.NotNull(id);
        Assert.Equal(user.Id, int.Parse(id));
    }
}
