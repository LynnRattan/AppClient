using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Baker? LoggedInBaker { get; set; }
        public User? LoggedInUser { get; set; }

        public ICommand GoToAddDessertCommand { get; set; }

        public ICommand LoadBakerDessertsCommand { get; set; }

        public ConProfilePageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInBaker = ((App)Application.Current).LoggedInBaker;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            bakerDessertsKeeper = new();
            BakerDesserts = new();
            isEmpty = true;
            FillBakerDesserts();
            DeleteDessertCommand = new Command(DeleteFromMenu);
            GoToAddDessertCommand = new Command(OnAddDessert);
            LoadBakerDessertsCommand = new Command(LoadBakerDesserts);
        }

        private async void FillBakerDesserts()
        {
            bakerDessertsKeeper = await proxy.GetDesserts();

            foreach (Dessert d in bakerDessertsKeeper)
            {
                if (d.BakerId == LoggedInBaker.BakerId)
                {
                    bakerDesserts.Add(d);
                }
            }
            if (bakerDesserts!=null && bakerDesserts.Count>0)
                isEmpty = false;
            else isEmpty = true;
            OnPropertyChanged("IsEmpty");
        }

        //public async void OnDelete(Object obj)
        //{
        //    if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to take off the dessert from the menu?", "Yes", "Cancel"))
        //    {
        //        Dessert dessert = (Dessert)obj;
        //        BakerDesserts.Remove(((Dessert)obj));
        //        proxy.DeclineDes(dessert.DessertId);
        //        BakerDesserts.Add(dessert);
        //    }
        //}

        public async void DeleteFromMenu(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to take off the dessert from the menu?", "Yes", "Cancel"))
            {
                Dessert d = (Dessert)obj;
                bool isDeleted = await proxy.DeleteOD(d.DessertId);
                if (isDeleted)
                {
                    BakerDesserts.Remove(((Dessert)obj));
                }
                else
                {
                    AppShell.Current.DisplayAlert("Dessert", "Something went wrong.\nPlease try again later", "Ok");
                }
                if (BakerDesserts == null || BakerDesserts.Count == 0)
                {
                    IsEmpty = true;
                }
            }
        }
        private void OnAddDessert()
        {
            // Navigate to the AddDessert View page
            //AppShell.Current.GoToAsync("AddDessert");
            ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<AddDessertPage>());
           
        }

        private async void LoadBakerDesserts()
        {
            IsRefreshing = true;
            BakerDesserts.Clear();
            FillBakerDesserts();
            if (bakerDesserts.Count > 0)
                isEmpty = false;
            else isEmpty = true;
            OnPropertyChanged("IsEmpty");
            OnPropertyChanged("LoggedInBaker");
            IsRefreshing = false;
        }
    }
}
