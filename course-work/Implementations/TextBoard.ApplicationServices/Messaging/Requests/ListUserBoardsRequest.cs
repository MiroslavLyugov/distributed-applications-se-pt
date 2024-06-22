namespace TextBoard.ApplicationServices.Messaging.Requests;

public class ListUserBoardsRequest
{
    public int UserId;
    public int Page { get; set; } = 0; // Count from 0
    public int PageSize { get; set; } = -1; // Print all by default

    public ListUserBoardsRequest(int uid)
    {
        UserId = uid;
    }

    public ListUserBoardsRequest(int uid, int page, int size)
    {
        UserId = uid;
        Page = page;
        PageSize = size;
    }
}
