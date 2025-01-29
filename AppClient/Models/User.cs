using AppClient.Services;
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

        public string ProfileName { get; set; }

        public int UserTypeId { get; set; }

        public string ProfileImagePath { get; set; } = "";
        public String FullImageURL
        {
            get
            {
                return LMBWebApi.ImageBaseAddress + this.ProfileImagePath;
            }
        }

    }
}
