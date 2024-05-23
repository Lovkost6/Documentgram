using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Document.Models;

public class Message
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }
    
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public string? PicturePath { get; set; }
    

    public long OwnerId { get; set; }
    
    [ForeignKey("OwnerId")]
    public User? Owner { get; set; }
    
}

public class MessageCreate
{
    public string Name { get; set; }
    
    public string? Description { get; set; }

    public IFormFile? PicturePath { get; set; }

    public List<long> RecipientsId { get; set; }
    
    
}

public class MessageStateUpdate
{
    public long Id { get; set; }
    public State State { get; set; }
    
}