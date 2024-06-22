namespace TextBoard.ApplicationServices.Messaging.Responses;

public class GenerateJwtTokenResponse : ResponseBase
{
    public string? BearerToken { get; set; }

    public GenerateJwtTokenResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public GenerateJwtTokenResponse(string token, string message)
        : base(BusinessStatusCodeEnum.Success, message)
    {
        BearerToken = token;
    }
}
