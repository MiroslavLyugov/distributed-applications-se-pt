namespace TextBoard.ApplicationServices.Messaging.ViewModels;

#nullable disable


public class UserVM
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool IsAdmin { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime LastLogin { get; set; }

}
