using AppClient.Services;
using AppClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
    public class ConProfilePageViewModel: ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;

        public ICommand GoToAddDessertCommand { get; set; }

        public ConProfilePageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            GoToAddDessertCommand = new Command(OnAddDessert);
        }

            private void OnAddDessert()
        {
            // Navigate to the AddDessert View page
            //AppShell.Current.GoToAsync("AddDessert");
            ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<AddDessertPage>());
        }
    }
}
