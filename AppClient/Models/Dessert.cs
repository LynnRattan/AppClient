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

        public int? DessertTypeId { get; set; }
        public string? DessertTypeName
        {
            get
            {
                List<DessertType> l = ((App)Application.Current).DessertTypes;
                DessertType? dType = l.Where(t => t.DessertTypeId == DessertTypeId).FirstOrDefault();
                if (dType == null)
                    return "Unknown";
                return dType.DessertTypeName;
            }
        }

    public double Price { get; set; }

        public string? DessertImagePath { get; set; }

        public int StatusCode { get; set; }

        public int BakerId { get; set; }
    }
}
