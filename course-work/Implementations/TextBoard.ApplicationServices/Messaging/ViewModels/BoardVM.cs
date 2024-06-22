namespace TextBoard.ApplicationServices.Messaging.ViewModels;

#nullable disable


public class BoardVM
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Hidden { get; set; }
    public bool Archived { get; set; }

    public DateTime CreateTime { get; set; }
}
