using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class DessertType
    {
        public int DessertTypeId { get; set; }
        public string DessertTypeName { get; set; }

        public override string ToString()
        {
            return DessertTypeName;
        }
    }
}
