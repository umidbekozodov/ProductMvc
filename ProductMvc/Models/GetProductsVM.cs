﻿namespace ProductMvc.Models;

public class GetProductsVM
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Quantly { get; set; }
    public double Price { get; set; }
    public double TotalPriceWithVat { get; set; }
}
