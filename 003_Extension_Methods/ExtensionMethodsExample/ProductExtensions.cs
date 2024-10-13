using System;
using ClassLibrary1;

namespace ExtensionMethodsExample;

public static class ProductExtensions
{
    public static double GetDiscount(this Product product)
    {
        return product.ProductCost * product.DiscountPercentage / 100;
    }
}
