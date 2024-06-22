namespace TextBoard.ApplicationServices.Messaging.Requests;

public class ListBoardsRequest
{
    public int Page { get; set; } = 0; // Count from 0
    public int PageSize { get; set; } = -1; // Print all by default

    public ListBoardsRequest()
    {

    }

    public ListBoardsRequest(int page, int size)
    {
        Page = page;
        PageSize = size;
    }
}
