namespace TextBoard.ApplicationServices.Messaging.Requests;

public class UpdateBoardRequest
{
    public int Id { get; set; }

    public string? Name { get; set; } = null;
    public bool? Hidden { get; set; } = null;
    public bool? Archived { get; set; } = null;

    public UpdateBoardRequest(int id, string? name = null, bool? hidden = null, bool? archived = null)
    {
        Id = id;
        Name = name;
        Hidden = hidden;
        Archived = archived;
    }
}
