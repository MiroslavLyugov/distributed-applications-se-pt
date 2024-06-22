namespace TextBoard.ApplicationServices.Messaging.Requests;

public class ReplaceMessageRequest
{
    public int OriginalId{ get; set; }
    public string Content { get; set; }

    public ReplaceMessageRequest(int original, string content)
    {
        OriginalId = original;
        Content = content;
    }
}
