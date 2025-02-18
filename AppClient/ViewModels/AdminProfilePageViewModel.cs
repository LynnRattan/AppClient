using AppClient.Models;
using AppClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.ViewModels
{
    public class AdminProfilePageViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;

        public User? LoggedInUser { get; set; }

        public AdminProfilePageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
        }
    }
}
