namespace TextBoard.ApplicationServices.Messaging.Requests;

public class ArchiveBoardRequest
{
    public int Id { get; set; }

    public ArchiveBoardRequest(int id)
    {
        Id = id;
    }
}
