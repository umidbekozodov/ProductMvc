using System.ComponentModel.DataAnnotations;

namespace ProductMvc.Dtoes;

public class UpdateProductDto
{
    [Required]
    public string? Title { get; set; }
    [Required]
    public int Quantly { get; set; }
    public double Price { get; set; }
}
