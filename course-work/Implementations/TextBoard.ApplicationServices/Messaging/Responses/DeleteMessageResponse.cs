namespace TextBoard.ApplicationServices.Messaging.Responses;

public class DeleteMessageResponse : ResponseBase
{
    public int DeleteEventId { get; set; }

    public DeleteMessageResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public DeleteMessageResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "Message delete event published")
    {
        DeleteEventId = id;
    }
}
