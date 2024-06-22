namespace TextBoard.ApplicationServices.Messaging.Requests;

public class DeleteMessageRequest
{
    public int Id { get; set; }
    public string Reason { get; set; }

    public DeleteMessageRequest(int id, string reason = "Message Deleted")
    {
        Id = id;
        Reason = reason;
    }
}
