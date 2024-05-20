using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Document.Models;

[Index(nameof(Login), IsUnique = true)]
public class User
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }
    
    [MinLength(4)]
    public string Name { get; set; }
    
    [MinLength(5)]
    public string Login { get; set; }
    
    [MinLength(5)]
    public string Password { get; set; }
}