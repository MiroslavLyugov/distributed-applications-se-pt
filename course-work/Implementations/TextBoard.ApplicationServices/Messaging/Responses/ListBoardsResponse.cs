using TextBoard.ApplicationServices.Messaging.ViewModels;
using System.Collections.Generic;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class ListBoardsResponse : ResponseBase
{
    public ICollection<BoardVM> Boards { get; set; }
    int Page { get; set;} = 0;
    int PageSize { get; set; } = -1;

    public ListBoardsResponse(ICollection<BoardVM> boards)
        : base(BusinessStatusCodeEnum.Success, "Board list fetched.")
    {
        Boards = boards;
    }

    public ListBoardsResponse(ICollection<BoardVM> boards, int page, int size)
        : base(BusinessStatusCodeEnum.Success, "Board list fetched.")
    {
        Boards = boards;
        Page = page;
        PageSize = size;
    }

    public ListBoardsResponse(BusinessStatusCodeEnum status, string message)
        : base(status, message)
    {
        Boards = new List<BoardVM>();
    }
}
