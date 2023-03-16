namespace ProductMvc.Services;

public static class CalculateService
{
    public static double TotalPriceWithVat(double vatRate, int itemAmount, double itemPrice)
    {
        var totalPriceWithVat = (itemAmount * itemPrice) * (1 + vatRate);

        return totalPriceWithVat;
    }
}
