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
        public Baker? LoggedInBaker { get; set; }
        public List<ConfectioneryType> ConfectioneryTypes { get; set; } = new List<ConfectioneryType>();
        public List<DessertType> DessertTypes { get; set; } = new List<DessertType>();
        public List<Status> Statuses { get; set; }=new List<Status>();

        private LMBWebApi proxy;
        public bool notInSession;
        public App(IServiceProvider serviceProvider, LMBWebApi proxy)
        {
            notInSession = true;
            this.proxy = proxy;
            InitializeComponent();
            LoggedInUser = null;
            LoggedInBaker = null;
            LoadBasicDataFromServer();
            //Start with the Login View
            MainPage = new NavigationPage(serviceProvider.GetService<HomePage>());
        }


        private async void LoadBasicDataFromServer()
        {
            List<ConfectioneryType>? confectioneryTypes = await this.proxy.GetConfectioneryTypes();
            if (confectioneryTypes != null)
            {
                ConfectioneryTypes.Clear();
                foreach (ConfectioneryType type in confectioneryTypes)
                {
                    ConfectioneryTypes.Add(type);
                }
            }

            List<DessertType>? dessertTypes = await this.proxy.GetDessertTypes();
            if (dessertTypes != null)
            {
                DessertTypes.Clear();
                foreach (DessertType type in dessertTypes)
                {
                    DessertTypes.Add(type);
                }
            }

            List<Status>? statusTypes = await this.proxy.GetStatuses();
            if (statusTypes != null)
            {
                Statuses.Clear();
                foreach (Status s in statusTypes)
                {
                    Statuses.Add(s);
                }
            }
        }
    }
}
