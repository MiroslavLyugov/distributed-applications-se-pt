namespace TextBoard.ApplicationServices.Messaging.Requests;

public class DeleteBoardRequest
{
    public int Id { get; set; }

    public DeleteBoardRequest(int id)
    {
        Id = id;
    }
}
