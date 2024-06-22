using TextBoard.ApplicationServices.Messaging.ViewModels;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class GetBoardResponse : ResponseBase
{
    public BoardVM? Board { get; set; }

    public GetBoardResponse(BoardVM? board = null)
        : base(board == null ? BusinessStatusCodeEnum.NotFound : BusinessStatusCodeEnum.Success,
               board == null ? "Board not found" : "Board data fetched.")
    {
        Board = board;
    }

    public GetBoardResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }
}
