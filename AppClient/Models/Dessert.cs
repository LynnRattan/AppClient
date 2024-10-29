using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class Dessert
    {
        public string DessertName { get; set; }

        public int DessertTypeId { get; set; }

        public double Price { get; set; }

        public byte[] DessertImage { get; set; }
    }
}
