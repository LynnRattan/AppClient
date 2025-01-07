using AppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.ModelsExt
{
    public class UserBaker : User
    {
        public string? ConfectioneryName { get; set; }
        public double? HighestPrice {  get; set; }
        public int? ConfectioneryTypeId { get; set; }
        public int StatusCode { get; set; }

        public double? Profits { get; set; }
    }
}
