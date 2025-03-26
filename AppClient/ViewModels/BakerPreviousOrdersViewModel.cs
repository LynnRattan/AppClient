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
    public class BakerPreviousOrdersViewModel:ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private List<Order> bakerOrdersKeeper;
        private ObservableCollection<Order> bakerOrderes;

        public ObservableCollection<Order> BakerOrders { get => bakerOrderes; set { bakerOrderes = value; OnPropertyChanged(); } }
        private Order selectedOrder;
        public Order SelectedOrder { get => selectedOrder; set { selectedOrder = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }

        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

       
        
        


        public Baker? LoggedInBaker { get; set; }
        public ICommand ViewOrderCommand { get; private set; }
        public ICommand LoadBakerOrdersCommand { get; private set; }

        public BakerPreviousOrdersViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInBaker = ((App)Application.Current).LoggedInBaker;
            bakerOrdersKeeper = new();
            BakerOrders = new();
            IsEmpty = true;
            InItData();
            ViewOrderCommand = new Command(OnView);
            LoadBakerOrdersCommand = new Command(async () => await LoadBakerOrders());

        }

        private async void InItData()
        {
            await LoadBakerOrders();
        }

        private async Task FillBakerOrders()
        {
            bakerOrdersKeeper.Clear();
            bakerOrdersKeeper = await proxy.GetOrders();
            BakerOrders.Clear();

            foreach (Order o in bakerOrdersKeeper)
            {
                if (o.BakerId == LoggedInBaker.BakerId && o.StatusCode!=1)
                    BakerOrders.Add(o);
            }
            if (BakerOrders != null&&BakerOrders.Count>0)
            {
                IsEmpty = false;
            }
        }

        public async void OnView(Object obj)
        {
            // Navigate to the UserViewOrder View page
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("SelectedOrder", SelectedOrder);
            await Shell.Current.GoToAsync("BakerViewOrder", data);
            SelectedOrder = null;
        }

        public async Task LoadBakerOrders()
        {
            IsRefreshing = true;
            FillBakerOrders();
            IsRefreshing = false;

        }


    }
}