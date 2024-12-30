using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient
{

    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public int bakerId { get; set; }
        public int highestPrice { get; set; }
        public int confectioneryTypeId { get; set; }
        public int statusCode { get; set; }
        public int profits { get; set; }
    }

}
