namespace TextBoard.ApplicationServices.Messaging.Requests;

public class GetMessageByIdRequest
{
    public int Id { get; set; }

    public GetMessageByIdRequest(int id)
    {
        Id = id;
    }
}
