using System.ComponentModel.DataAnnotations;

namespace TextBoard.Data.Models;

public class Message
{
    [Key]
    public int Id { get; set; }

    public Board Board { get; set; } = null!;
    public User User { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(2048)]
    public string Content { get; set; } = null!;

    public Message? OverrideMessage { get; set; }
    public bool DeleteEvent { get; set; } = false; // if a message is a delete event the Content is the reason for deletion

    public DateTime CreateTime { get; set; } = DateTime.Now;
}
