using ClassLibrary1;
using ExtensionMethodsExample;

Product product = new()
{
    ProductCost = 1000,
    DiscountPercentage = 10
};

Console.WriteLine(product.GetDiscount());
Console.ReadKey();