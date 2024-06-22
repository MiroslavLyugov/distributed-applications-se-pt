namespace TextBoard.ApplicationServices.Messaging.Responses;

public class CreateBoardResponse : ResponseBase
{
    public int Id { get; set; } = -1;

    public CreateBoardResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public CreateBoardResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "Board created")
    {
        Id = id;
    }
}
