using EFCoreWithSP.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("orderDetail =====>");

using (var dbContext = new ShoppingDbContext())
{
    dbContext.Database.EnsureCreated();

    // with params
    SqlParameter orderDetailIdParam = new SqlParameter("@orderDetails", 1);
    var orderDetails = dbContext.OrderDetails.FromSql($"EXEC usp_GetOrderDetails {orderDetailIdParam}");

    //var orderDetails = dbContext.OrderDetails.FromSql($"EXEC usp_GetallOrderDetails");
    //.AsEnumerable().Where(x => x.OrderDetailId == 1);

    foreach (var orderDetail in orderDetails)
    {
        Console.WriteLine(orderDetail);
    }
}