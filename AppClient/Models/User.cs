using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Mail { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public int UserTypeId { get; set; }

        public double? HighestPrice { get; set; }

        public int? ConfectioneryTypeId { get; set; }
    }
}
