using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Implementation;
using TextBoard.Data.Contexts;
using System.Text.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TextBoardDbContext>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IBoardsService, BoardsService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();

var signingKey = new SymmetricSecurityKey(
    Encoding.ASCII.GetBytes("fb5e1f5a4d20490fb3351dd5dbfdc4a7"));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = "TextBoard",
    ValidAudience = "TextBoard",


    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = tokenValidationParameters);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
