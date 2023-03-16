namespace ProductMvc.Test.Unit
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(0.005, 10, 10, 100.49999999999999)]
        [InlineData(0.005, 5, 5, 25.124999999999996)]
        [InlineData(0.005, 10, 25, 251.24999999999997)]
        public void CalculateMethod(double vat, int amount, double price, double result)
        {
            var calc = ProductMvc.Services.CalculateService.TotalPriceWithVat(vat, amount, price);
            Assert.Equal(result, calc);
        }
    }
}