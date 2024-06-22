namespace TextBoard.ApplicationServices.Messaging.Responses;

public class CreateMessageResponse : ResponseBase
{
    public int Id { get; set; } = -1;

    public CreateMessageResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public CreateMessageResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "Message created")
    {
        Id = id;
    }
}
