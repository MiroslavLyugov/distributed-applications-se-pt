namespace TextBoard.ApplicationServices.Messaging.Requests;

public class ListBoardMessagesRequest
{
    public int BoardId;
    public int Page { get; set; } = 0; // Count from 0
    public int PageSize { get; set; } = -1; // Print all by default

    public ListBoardMessagesRequest(int bid)
    {
        BoardId = bid;
    }

    public ListBoardMessagesRequest(int bid, int page, int size)
    {
        BoardId = bid;
        Page = page;
        PageSize = size;
    }
}
