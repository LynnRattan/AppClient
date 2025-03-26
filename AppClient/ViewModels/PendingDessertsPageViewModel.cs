using AppClient.Models;
using AppClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
    public class PendingDessertsPageViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<Dessert> pendingDesserts;
        private List<Dessert> pendingDessertsKeeper;
        public ObservableCollection<Dessert> PendingDesserts { get => pendingDesserts; set { pendingDesserts = value; OnPropertyChanged(); } }
        private Dessert selectedPendingDessert;
        public Dessert SelectedPendingDessert { get => selectedPendingDessert; set { selectedPendingDessert = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }

        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        public ICommand DeclineDessertCommand { get; private set; }
        public ICommand ApproveDessertCommand { get; private set; }

        public ICommand LoadPendingDessertsCommand { get; private set; }

        public PendingDessertsPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            pendingDessertsKeeper = new();
            PendingDesserts = new();
            isEmpty = true;
            InItData();
            DeclineDessertCommand = new Command(OnDecline);
            ApproveDessertCommand = new Command(OnApprove);
            LoadPendingDessertsCommand = new Command(async () => await LoadPendingDesserts());

        }

        private async void InItData()
        {
            await LoadPendingDesserts();
        }

        private async void GetDesserts()
        {
            pendingDessertsKeeper = await proxy.GetDesserts();
        }
        private async Task FillPendingDesserts()
        {
            pendingDessertsKeeper.Clear();
            pendingDessertsKeeper = await proxy.GetDesserts();
            PendingDesserts.Clear();

            foreach (Dessert d in pendingDessertsKeeper)
            {
                if (d.StatusCode == 1)
                    PendingDesserts.Add(d);
            }
            if(PendingDesserts!=null&&PendingDesserts.Count>0)
            {
                IsEmpty = false;
            }
            else IsEmpty = true;
        }

        public async void OnDecline(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to decline the dessert?", "Yes", "Cancel"))
            {
                Dessert d = (Dessert)obj;
                PendingDesserts.Remove(((Dessert)obj));
                proxy.DeclineDes(d.DessertId);
                if (PendingDesserts != null && PendingDesserts.Count > 0)
                {
                    IsEmpty = false;
                }
                else IsEmpty = true;
            }
        }
        public async void OnApprove(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to approve the dessert?", "Yes", "Cancel"))
            {
                Dessert d = (Dessert)obj;
                PendingDesserts.Remove(((Dessert)obj));
                proxy.ApproveDes(d.DessertId);
                if (PendingDesserts != null && PendingDesserts.Count > 0)
                {
                    IsEmpty = false;
                }
                else IsEmpty = true;
            }
        }

        public async Task LoadPendingDesserts()
        {
            IsRefreshing = true;
            FillPendingDesserts();
            IsRefreshing = false;

        }


    }
}