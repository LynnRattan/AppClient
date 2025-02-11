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
    public class CartPageViewModel:ViewModelBase
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

        private int newQuantity;
        public int NewQuantity { get => newQuantity; set { newQuantity=value; OnPropertyChanged(); } }

        private double totalPrice;
        public double TotalPrice { get => totalPrice; set { totalPrice = value; OnPropertyChanged(); } }

        public ICommand DeleteDessertCommand { get; private set; }
        public ICommand ChangeQuantityCommand { get; private set; }
        public ICommand UpdateQuantityCommand { get; private set; }
        public ICommand DeleteAllCommand { get; private set; }
        public ICommand OrderCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ICommand LoadUserDessertsCommand { get; private set; }
        public User? LoggedInUser { get; set; }
        

        public CartPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            isChanging = false;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            orderedDessertsKeeper = new();
            UserOrderedDesserts = new();
            isEmpty = true;
            FillUserDesserts();            
            DeleteDessertCommand = new Command(OnDeleteDessert);
            ChangeQuantityCommand = new Command(OnChangeQuantity);
            UpdateQuantityCommand = new Command(OnUpdateQuantity);
            DeleteAllCommand = new Command(OnDeleteAll);
            OrderCommand = new Command(OnOrder);
            CancelCommand = new Command(OnCancel);
            LoadUserDessertsCommand = new Command(LoadUserDesserts);

        }

        private void OnCancel()
        {
            IsChanging = false;
        }

        private async void GetOrderedDesserts()
        {
            orderedDessertsKeeper = await proxy.GetOrderedDesserts();
        }
        private async void FillUserDesserts()
        {
            UserOrderedDesserts.Clear();
            this.totalPrice = 0;
            orderedDessertsKeeper = await proxy.GetOrderedDesserts();

            foreach (OrderedDessert d in orderedDessertsKeeper)
            {
                if (d.UserId == LoggedInUser.UserId)
                {
                    userOrderedDesserts.Add(d);
                    this.TotalPrice += d.Price;
                }

            }
            if (userOrderedDesserts != null)
            {
                isEmpty = false;
            }
            else isEmpty = true;
        }

        public async void OnDeleteDessert(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to delete the dessert from your cart?", "Yes", "Cancel"))
            {
                OrderedDessert d = (OrderedDessert)obj;
                UserOrderedDesserts.Remove(((OrderedDessert)obj));
                proxy.DeleteOD(d.OrderedDessertId);
                if (userOrderedDesserts == null)
                {
                    isEmpty = true;
                }
            }
        }
        public async void OnChangeQuantity(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to change the quantity?", "Yes", "Cancel"))
            {
                IsChanging =true;
            }
        }

        public async void OnUpdateQuantity(Object obj)
        {
            OrderedDessert d = (OrderedDessert)obj;
            OrderedDessert o = new OrderedDessert();
            o=await proxy.UpdateQuantity(d, NewQuantity);
            if (o != null)
            {
                string successMsg = "Quantity was successfully changed!";
                await Application.Current.MainPage.DisplayAlert("Changing Quantity", successMsg, "ok");
                LoadUserDesserts();
            }
            else
            {
                string errorMsg = "Changing the quantity has failed. Please try again.";
                await Application.Current.MainPage.DisplayAlert("Changing Quantity", errorMsg, "ok");
            }
            IsChanging = false;
        }

        public async void OnDeleteAll()
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to empty your cart?", "Yes", "Cancel"))
            {
                List<OrderedDessert> temp = this.UserOrderedDesserts.ToList();
                foreach (OrderedDessert d in temp)
                {
                    OrderedDessert od = d;
                    UserOrderedDesserts.Remove(d);
                    proxy.DeleteOD(od.OrderedDessertId);
                    
                }
                isEmpty = true;
            }
        }

        public async void OnOrder()
        {

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
