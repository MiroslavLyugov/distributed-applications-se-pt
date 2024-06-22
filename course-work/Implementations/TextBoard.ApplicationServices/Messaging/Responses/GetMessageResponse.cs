using TextBoard.ApplicationServices.Messaging.ViewModels;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class GetMessageResponse : ResponseBase
{
    public MessageVM? Message { get; set; }

    public GetMessageResponse(MessageVM? message = null)
        : base(message == null ? BusinessStatusCodeEnum.NotFound : BusinessStatusCodeEnum.Success,
               message == null ? "Message not found" : "Message data fetched.")
    {
        Message = message;
    }

    public GetMessageResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }
}
