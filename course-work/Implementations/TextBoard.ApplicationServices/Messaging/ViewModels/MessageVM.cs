namespace TextBoard.ApplicationServices.Messaging.ViewModels;

#nullable disable


public class MessageVM
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BoardId { get; set; }

    public string Content { get; set; } // or a reason for deletion

    public int OverrideMessageId { get; set; }
    public bool DeleteEvent { get; set; }

    public DateTime CreateTime { get; set; }
}
