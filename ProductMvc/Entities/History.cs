using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductMvc.Entities;

public class History
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? UserId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? Changed { get; set; }
    public DateTime DateTime { get; set; }
    public long ProductId { get; set; }
}