namespace TextBoard.ApplicationServices.Messaging;

public abstract class ResponseBase
{
    public BusinessStatusCodeEnum StatusCode { get; set; } = BusinessStatusCodeEnum.None;
    public string? MessageText { get; set; }

    public ResponseBase(BusinessStatusCodeEnum statusCode)
    {
        StatusCode = statusCode;
    }

    public ResponseBase(BusinessStatusCodeEnum statusCode, string? messageText)
    {
        StatusCode = statusCode;
        MessageText = messageText;
    }
}
