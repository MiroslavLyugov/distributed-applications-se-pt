namespace TextBoard.ApplicationServices.Messaging.Responses;

public class AuthenticateUserResponse : ResponseBase
{
    public string? BearerToken { get; set; } 

    public AuthenticateUserResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {
    }

    public AuthenticateUserResponse(string token, string message)
        : base(BusinessStatusCodeEnum.Success, message)
    {
        BearerToken = token;
    }
}
