namespace TextBoard.ApplicationServices.Messaging.Requests;

public class GetBoardByNameRequest
{
    public string Name { get; set; }

    public GetBoardByNameRequest(string name)
    {
        Name = name;
    }
}
