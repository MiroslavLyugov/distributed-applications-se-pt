namespace TextBoard.ApplicationServices.Messaging.Responses;

public class ReplaceMessageResponse : ResponseBase
{
    public int NewId { get; set; } = -1;

    public ReplaceMessageResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public ReplaceMessageResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "Message replace event created")
    {
        NewId = id;
    }
}
