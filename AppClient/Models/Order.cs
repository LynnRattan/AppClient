using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BakerId { get; set; }

        public DateOnly OrderDate { get; set; }
        public DateOnly? ArrivelDate { get; set; }

        public string Adress { get; set; }

        public double TotalPrice { get; set; }
        public int StatusCode { get; set; }
    }
}
