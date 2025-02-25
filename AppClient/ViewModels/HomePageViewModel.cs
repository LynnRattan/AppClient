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
    public class HomePageViewModel:ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;

       
        public ICommand GoToLoginCommand { get; private set; }
        public ICommand GoToSignUpCommand { get; private set; }

        

        public HomePageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;

            GoToLoginCommand = new Command(OnLogin);
            GoToSignUpCommand = new Command(OnSignUp);
           
        }

       
        private void OnLogin()
        {
            // Navigate to the AddDessert View page
            //AppShell.Current.GoToAsync("AddDessert");
            ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<LoginPage>());

        }

        private void OnSignUp()
        {
            // Navigate to the AddDessert View page
            //AppShell.Current.GoToAsync("AddDessert");
            ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<SignUpPage>());

        }
    }
}

