namespace TextBoard.ApplicationServices.Messaging.Responses;

public class UpdateUserResponse : ResponseBase
{
    public int UserId { get; set; }

    public UpdateUserResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {
        
    }

    public UpdateUserResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "User updated")
    {
        UserId = id;
    }
}
