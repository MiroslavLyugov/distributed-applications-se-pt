namespace TextBoard.ApplicationServices.Messaging.Requests;

public class CreateMessageRequest
{
    public int UserId { get; set; }
    public int BoardId { get; set; }
    public string Content { get; set; }

    public CreateMessageRequest(int uid, int bid, string content)
    {
        UserId = uid;
        BoardId = bid;
        Content = content;
    }
}
