namespace TextBoard.ApplicationServices.Messaging.Responses;

public class UpdateBoardResponse : ResponseBase
{
    public int BoardId { get; set; }

    public UpdateBoardResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }

    public UpdateBoardResponse(int id)
        : base(BusinessStatusCodeEnum.Success, "Board updated.")
    {
        BoardId = id;
    }
}
