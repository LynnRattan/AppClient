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
    public class UserViewOrderViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<OrderedDessert> userOrderedDesserts;
        private List<OrderedDessert> orderedDessertsKeeper;
        public ObservableCollection<OrderedDessert> UserOrderedDesserts { get => userOrderedDesserts; set { userOrderedDesserts = value; OnPropertyChanged(); } }
        private OrderedDessert selectedOrderedDessert;
        public OrderedDessert SelectedOrderedDessert { get => selectedOrderedDessert; set { selectedOrderedDessert = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }


        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        private bool isChanging;
        public bool IsChanging { get => isChanging; set { isChanging = value; OnPropertyChanged(); } }

        private Order selectedOrder;
        public Order SelectedOrder {get => selectedOrder; set { selectedOrder = value; OnPropertyChanged();
    }
}
        public ICommand LoadUserDessertsCommand { get; private set; }
        public User? LoggedInUser { get; set; }


        public UserViewOrderViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            isChanging = false;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            orderedDessertsKeeper = new();
            UserOrderedDesserts = new();
            isEmpty = true;
            FillUserDesserts();
            LoadUserDessertsCommand = new Command(LoadUserDesserts);
            

        }

       
        private async void FillUserDesserts()
        {
            UserOrderedDesserts.Clear();
            orderedDessertsKeeper.Clear();
           
            orderedDessertsKeeper = await proxy.GetOrderedDesserts();

            foreach (OrderedDessert d in orderedDessertsKeeper)
            {
                if (d.OrderId==SelectedOrder.Id)
                {
                    UserOrderedDesserts.Add(d);
                }

            }
            if (UserOrderedDesserts != null&&UserOrderedDesserts.Count>0)
            {
                IsEmpty = false;
            }
            else IsEmpty = true;
        }

       
        private async void LoadUserDesserts()
        {
            IsRefreshing = true;
            IsChanging = false;
            FillUserDesserts();
            IsRefreshing = false;

        }

    }
}