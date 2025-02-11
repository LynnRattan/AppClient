using AppClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class OrderedDessert
    {
        public int OrderedDessertId { get; set; }
        public int DessertId { get; set; }
        public int? OrderId { get; set; }
        public int Quantity { get; set; }
        public int StatusCode { get; set; }
        public int UserId { get; set; }
        public int BakerId { get; set; }
        public Dessert TheDessert { get; set; }
        public Baker TheBaker { get; set; }
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

        public string? OrderedDessertImage { get; set; }
        public String FullImageURL
        {
            get
            {
                return TheDessert.FullImageURL;
            }
        }

        
    }
}
