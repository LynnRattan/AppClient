//using Android.Service.Autofill;
using AppClient.Services;
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

        public User? UserNavigation { get; set; }

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

        public String FullImageURL
        {
            get
            {
                if (UserNavigation != null)
                    return UserNavigation.FullImageURL;
                return null;
            }
        }
        private async void GetUsers(List<User>? l, LMBWebApi service)
        {
            l = await service.GetUsers();
        }

        public double? Profits { get; set; }



    }
}
