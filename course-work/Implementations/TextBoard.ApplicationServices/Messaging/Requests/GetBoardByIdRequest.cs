namespace TextBoard.ApplicationServices.Messaging.Requests;

public class GetBoardByIdRequest
{
    public int Id { get; set; }

    public GetBoardByIdRequest(int id)
    {
        Id = id;
    }
}
