using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Document.Models;

public class MessageRecipient
{
    public long MessageId { get; set; }
    [ForeignKey("MessageId")]
    public Message? Message { get; set; }
    
    public long UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
}
