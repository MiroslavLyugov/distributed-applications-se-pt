using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextBoard.Data.Models;

public class Board
{
    [Key]
    public int Id { get; set; }

    [MinLength(1)]
    [MaxLength(128)]
    public string Name { get; set; } = null!;

    public bool Hidden { get; set; } = false; // Hiding a board hides it from global listings
    public bool Archived { get; set; } = false; // Archiving a board disables adding new messages
    public bool Deleted { get; set; } = false; // Mark as deleted instead deleting

    public DateTime CreateTime { get; set; } = DateTime.Now;


    [InverseProperty("Board")]
    public List<Message> Messages { get; set; } = new();
}
