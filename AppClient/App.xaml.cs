using AppClient.ViewModels;
using AppClient.Views;

namespace AppClient
{
    public partial class App : Application
    {
        public App()
            //LoginPageViewModel loginPageViewModel
        {
            InitializeComponent();

            MainPage = new AppShell();


        }
    }
}
