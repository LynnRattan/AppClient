using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
    public class UserProfilePageViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;



        public User? LoggedInUser { get; set; }


        public ICommand GoToShopCommand { get; set; }

        

        public UserProfilePageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInUser = ((App)Application.Current).LoggedInUser;            
            GoToShopCommand = new Command(OnShop);
           
        }

       
        
        private void OnShop()
        {
            // Navigate to the AddDessert View page
            //AppShell.Current.GoToAsync("AddDessert");
            ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<SearchConsPage>());

        }

        
    }
}


