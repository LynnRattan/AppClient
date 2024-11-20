using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.Models
{
    public class User
    {
        public int userId { get; set; }

        public string mail { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string profileName { get; set; }

        public int userTypeId { get; set; }

    }
}
