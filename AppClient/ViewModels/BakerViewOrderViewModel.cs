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
    [QueryProperty(nameof(SelectedOrder), "SelectedOrder")]
    public class BakerViewOrderViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<OrderedDessert> bakerOrderedDesserts;
        private List<OrderedDessert> orderedDessertsKeeper;
        public ObservableCollection<OrderedDessert> BakerOrderedDesserts { get => bakerOrderedDesserts; set { bakerOrderedDesserts = value; OnPropertyChanged(); } }
        private OrderedDessert selectedOrderedDessert;
        public OrderedDessert SelectedOrderedDessert { get => selectedOrderedDessert; set { selectedOrderedDessert = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }


        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        

        private Order selectedOrder;
        public Order SelectedOrder
        {
            get => selectedOrder; set
            {
                selectedOrder = value; OnPropertyChanged();
            }
        }
        public ICommand LoadBakerDessertsCommand { get; private set; }
        public ICommand DeclineDessertCommand { get; private set; }
       
        public ICommand DeclineOrderCommand { get; private set; }
        public ICommand ApproveOrderCommand { get; private set; }
        public Baker? LoggedInBaker { get; set; }


        public BakerViewOrderViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInBaker = ((App)Application.Current).LoggedInBaker;
            orderedDessertsKeeper = new();
            BakerOrderedDesserts = new();
            isEmpty = true;
            FillBakerDesserts();
            LoadBakerDessertsCommand = new Command(LoadBakerDesserts);
            DeclineDessertCommand = new Command(OnDeclineDessert);
            DeclineOrderCommand = new Command(OnDeclineOrder);
            ApproveOrderCommand = new Command(OnApproveOrder);
            


        }

        private async void OnDeclineDessert()
        {

        }

        private async void OnDeclineOrder()
        {

        }

        private async void OnApproveOrder()
        {

        }


        private async void FillBakerDesserts()
        {
            BakerOrderedDesserts.Clear();
            orderedDessertsKeeper.Clear();

            orderedDessertsKeeper = await proxy.GetOrderedDesserts();

            foreach (OrderedDessert d in orderedDessertsKeeper)
            {
                if (d.OrderId == SelectedOrder.Id && d.StatusCode==1)
                {
                    BakerOrderedDesserts.Add(d);
                }

            }
            if (BakerOrderedDesserts != null)
            {
                isEmpty = false;
            }
            else isEmpty = true;
        }


        private async void LoadBakerDesserts()
        {
            IsRefreshing = true;
            FillBakerDesserts();
            IsRefreshing = false;

        }

    }
}