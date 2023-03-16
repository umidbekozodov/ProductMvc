using System.ComponentModel.DataAnnotations;

namespace ProductMvc.Dtoes;

public class CreateProductDto
{
    [Required]
    public string? Title { get; set; }
    [Required]
    public int Quantly { get; set; }
    public double Price { get; set; }
}
