using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class Baker
    {
        public int BakerId { get; set; }

        public string? ConfectioneryName { get; set; }
        public double? HighestPrice { get; set; }

        public int? ConfectioneryTypeId { get; set; }

        public string? ConfectioneryTypeName 
        { 
            get
            {
                List<ConfectioneryType> l = ((App)Application.Current).ConfectioneryTypes;
                ConfectioneryType? cType = l.Where(t => t.ConfectioneryTypeId == ConfectioneryTypeId).FirstOrDefault();
                if (cType == null)
                    return "Unknown";
                return cType.ConfectioneryTypeName;
            }
        }

        public int StatusCode { get; set; }

        public double? Profits { get; set; }



    }
}
