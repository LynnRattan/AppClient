using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
    [QueryProperty(nameof(Confectionery), "FoundConfectionery")]
    public class ViewConfectioneryPageViewModel:ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;
        private Baker confectionery;
        public Baker Confectionery
        {
            get => confectionery;
            set
            {
                confectionery = value;
                OnPropertyChanged();
            }
        }
        private List<Dessert> bakerDessertsKeeper;
        private ObservableCollection<Dessert> bakerDesserts;

        public ObservableCollection<Dessert> BakerDesserts { get => bakerDesserts; set { bakerDesserts = value; OnPropertyChanged(); } }
        private Dessert selectedbakerDesssert;
        public Dessert SelectedBakerDessert { get => selectedbakerDesssert; set { selectedbakerDesssert = value; OnPropertyChanged(); } }

        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }
        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        public ICommand DeleteDessertCommand { get; private set; }
        public ICommand PurchaseCommand { get; private set; }

        
        public User? LoggedInUser { get; set; }

        public ICommand LoadBakerDessertsCommand { get; set; }

        public ViewConfectioneryPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            bakerDessertsKeeper = new();
            BakerDesserts = new();
            isEmpty = true;
            FillBakerDesserts();
            DeleteDessertCommand = new Command(OnDelete);
            PurchaseCommand= new Command(OnPurchase);
            LoadBakerDessertsCommand = new Command(LoadBakerDesserts);
        }

        private async void FillBakerDesserts()
        {
            bakerDessertsKeeper = await proxy.GetDesserts();

            foreach (Dessert d in bakerDessertsKeeper)
            {
                if (d.BakerId == Confectionery.BakerId&& d.StatusCode==2)
                {
                    bakerDesserts.Add(d);
                }
            }
            if (bakerDesserts.Count > 0)
                IsEmpty = false;
            else IsEmpty = true;
            OnPropertyChanged("IsEmpty");
        }

        public async void OnDelete(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to take off the dessert from the menu?", "Yes", "Cancel"))
            {
                Dessert dessert = (Dessert)obj;
                BakerDesserts.Remove(((Dessert)obj));
                proxy.DeclineDes(dessert.DessertId);
                BakerDesserts.Add(dessert);
            }
        }

        public async void OnPurchase()
        {
            // Navigate to the BuyDessert View page
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("SelectedDessert", SelectedBakerDessert);
            await Shell.Current.GoToAsync("BuyDes", data);
            SelectedBakerDessert = null;
        }


        private async void LoadBakerDesserts()
        {
            IsRefreshing = true;
            BakerDesserts.Clear();
            FillBakerDesserts();
            if (bakerDesserts.Count > 0)
                IsEmpty = false;
            else IsEmpty = true;
            OnPropertyChanged("IsEmpty");
            OnPropertyChanged("LoggedInBaker");
            IsRefreshing = false;
        }
    }
}
