using AppClient.Models;
using AppClient.Services;
using AppClient.ViewModels;
using AppClient.Views;

namespace AppClient
{
    public partial class App : Application
    {
        //Application level variables
        public User? LoggedInUser { get; set; }
        private LMBWebApi proxy;
        public bool notInSession;
        public App(IServiceProvider serviceProvider, LMBWebApi proxy)
        {
            notInSession = true;
            this.proxy = proxy;
            InitializeComponent();
            LoggedInUser = null;
            //Start with the Login View
            MainPage = new NavigationPage(serviceProvider.GetService<LoginPage>());
        }
    }
}
