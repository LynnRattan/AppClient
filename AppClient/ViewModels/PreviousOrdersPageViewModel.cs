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
    public class PreviousOrdersPageViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private List<Order> userOrdersKeeper;
        private ObservableCollection<Order> userOrderes;
        
        public ObservableCollection<Order> UserOrders { get => userOrderes; set { userOrderes = value; OnPropertyChanged(); } }
        private Order selectedOrder;
        public Order SelectedOrder { get => selectedOrder; set { selectedOrder = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }

        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }
        

        public User? LoggedInUser { get; set; }
        public ICommand ViewOrderCommand { get; private set; }
        public ICommand LoadUserOrdersCommand { get; private set; }

        public PreviousOrdersPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            userOrdersKeeper = new();
            UserOrders = new();
            isEmpty = true;
            FillUserOrders();
            ViewOrderCommand = new Command(OnView);
            LoadUserOrdersCommand = new Command(LoadUserOrders);

        }


       
        private async void FillUserOrders()
        {
            userOrdersKeeper = await proxy.GetOrders();

            foreach (Order o in userOrdersKeeper)
            {
                if (o.UserId==LoggedInUser.UserId)
                    UserOrders.Add(o);
            }
            if (UserOrders != null && UserOrders.Count>0)
            {
                IsEmpty = false;
            }
            else IsEmpty = true;
        }

        public async void OnView(Object obj)
        {
            // Navigate to the UserViewOrder View page
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("SelectedOrder", SelectedOrder);
            await Shell.Current.GoToAsync("ViewOrder", data);
            SelectedOrder = null;
        }
       
        private async void LoadUserOrders()
        {
            IsRefreshing = true;
            UserOrders.Clear();
            FillUserOrders();
            IsRefreshing = false;

        }


    }
}

