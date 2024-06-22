using TextBoard.ApplicationServices.Messaging.ViewModels;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class ListUserBoardsResponse : ResponseBase
{
    public int UserId { get; set; }
    public ICollection<BoardVM> Boards { get; set; }
    int Page { get; set;} = 0;
    int PageSize { get; set; } = -1;

    public ListUserBoardsResponse(int userId, ICollection<BoardVM> boards)
        : base(BusinessStatusCodeEnum.Success, "Board list fetched.")
    {
        UserId = userId;
        Boards = boards;
    }

    public ListUserBoardsResponse(int userId, ICollection<BoardVM> boards, int page, int size)
        : base(BusinessStatusCodeEnum.Success, "Board list fetched.")
    {
        UserId = userId;
        Boards = boards;
        Page = page;
        PageSize = size;
    }

    public ListUserBoardsResponse(int userId, BusinessStatusCodeEnum status, string message)
        : base(status, message)
    {
        UserId = userId;
        Boards = new List<BoardVM>();
    }

    public ListUserBoardsResponse(BusinessStatusCodeEnum status, string message)
        : base(status, message)
    {
        Boards = new List<BoardVM>();
    }
}
