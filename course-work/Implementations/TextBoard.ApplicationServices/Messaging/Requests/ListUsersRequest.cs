namespace TextBoard.ApplicationServices.Messaging.Requests;

public class ListUsersRequest
{
    public int Page { get; set; } = 0; // Count from 0
    public int PageSize { get; set; } = -1; // Print all by default

    public ListUsersRequest()
    {
        
    }

    public ListUsersRequest(int page, int size)
    {
        Page = page;
        PageSize = size;
    }
}
