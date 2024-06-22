using TextBoard.ApplicationServices.Messaging.ViewModels;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class ListBoardMessagesResponse : ResponseBase
{
    public int BoardId { get; set; }
    public ICollection<MessageVM> Messages { get; set; }
    int Page { get; set;} = 0;
    int PageSize { get; set; } = -1;

    public ListBoardMessagesResponse(int boardId, ICollection<MessageVM> messages)
        : base(BusinessStatusCodeEnum.Success, "Message list fetched.")
    {
        BoardId = boardId;
        Messages = messages;
    }

    public ListBoardMessagesResponse(int boardId, ICollection<MessageVM> messages, int page, int size)
        : base(BusinessStatusCodeEnum.Success, "Message list fetched.")
    {
        BoardId = boardId;
        Messages = messages;
        Page = page;
        PageSize = size;
    }

    public ListBoardMessagesResponse(int boardId, BusinessStatusCodeEnum status, string message)
        : base(status, message)
    {
        BoardId = boardId;
        Messages = new List<MessageVM>();
    }

    public ListBoardMessagesResponse(BusinessStatusCodeEnum status, string message)
        : base(status, message)
    {
        Messages = new List<MessageVM>();
    }
}
