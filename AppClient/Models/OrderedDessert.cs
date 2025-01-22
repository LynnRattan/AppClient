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

        public string? StatusName
        {
            get
            {
                List<Status> l = ((App)Application.Current).Statuses;
                Status? sType = l.Where(t => t.StatusCode == StatusCode).FirstOrDefault();
                if (sType == null)
                    return "Unknown";
                return sType.StatusName;
            }
        }

        public double Price { get; set; }
    }
}
