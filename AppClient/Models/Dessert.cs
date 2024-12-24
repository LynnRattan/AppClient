using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class Dessert
    {
        public int DessertId { get; set; }
        public string DessertName { get; set; }

        public int DessertTypeId { get; set; }

        public double Price { get; set; }

        public byte[] DessertImage { get; set; }

        public int StatusCode { get; set; }
    }
}
