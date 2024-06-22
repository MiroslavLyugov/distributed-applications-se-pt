namespace TextBoard.ApplicationServices.Messaging.Responses;

public class DeleteBoardResponse : ResponseBase
{
    public DeleteBoardResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }
}
