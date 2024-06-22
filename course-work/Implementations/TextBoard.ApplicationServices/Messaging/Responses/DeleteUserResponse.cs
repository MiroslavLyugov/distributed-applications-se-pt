namespace TextBoard.ApplicationServices.Messaging.Responses;

public class DeleteUserResponse : ResponseBase
{
    public DeleteUserResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {
        
    }
}
