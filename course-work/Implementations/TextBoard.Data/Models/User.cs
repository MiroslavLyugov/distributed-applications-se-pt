using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextBoard.Data.Models;

public class User
{
    [Key]
    public int Id { get; set; }


    [MinLength(3)]
    [MaxLength(24)]
    public string Name { get; set; } = null!;

    [MinLength(6)]
    [MaxLength(24)]
    public string Password { get; set; } = null!;


    public bool IsAdmin { get; set; } = false;


    public bool Deleted { get; set; } = false;


    public DateTime CreateTime { get; set; } = DateTime.Now;
    public DateTime LastLogin { get; set; } = DateTime.Now;
}
