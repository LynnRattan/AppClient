using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class OrderedDessert
    {
        public int DessertId { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public int StatusCode { get; set; }

        public double Price { get; set; }
    }
}
