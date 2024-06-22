namespace TextBoard.ApplicationServices.Messaging.Requests;

public class CreateBoardRequest
{
    public string Name { get; set; }
    public bool Hidden { get; set; }

    public CreateBoardRequest(string name, bool hidden = false)
    {
        Name = name;
        Hidden = hidden;
    }
}
