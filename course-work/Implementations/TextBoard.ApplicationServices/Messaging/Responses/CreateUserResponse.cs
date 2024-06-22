namespace TextBoard.ApplicationServices.Messaging.Responses;

public class CreateUserResponse : ResponseBase
{
    public int Id { get; set; } = -1;

    public CreateUserResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public CreateUserResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "User created")
    {
        Id = id;
    }
}
