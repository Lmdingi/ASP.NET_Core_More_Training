using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithSP.Database
{
    [Table("OrderDetails", Schema ="shopping")]
    internal class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

        public override string ToString()
        {
            return $"OrderDetailId: {OrderDetailId}, OrderId: {OrderId}, Name: {Name}, Color: {Color}, Size:{Size}";
        }
    }
}
