using AppClient.Models;
using AppClient.Services;
//using Microsoft.UI.Xaml.Input;
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

        #region New Quantity
        private string newQuantity;
        public string NewQuantity { get => newQuantity; set { newQuantity=value; ValidateNewQuantity(); OnPropertyChanged(); } }

        private bool showNewQuantityError;

        public bool ShowNewQuantityError
        {
            get => showNewQuantityError;
            set
            {
                showNewQuantityError = value;
                OnPropertyChanged("ShowNewQuantityError");
            }
        }

        private void ValidateNewQuantity()
        {
            int d = 0;
            if (IsChanging&&(string.IsNullOrEmpty(NewQuantity) || !int.TryParse(this.newQuantity, out d)))
            {
                this.ShowNewQuantityError = true;
            }
            else
                this.ShowNewQuantityError = false;

        }

        private string newQuantityError;

        public string NewQuantityError
        {
            get => newQuantityError;
            set
            {
                newQuantityError = value;
                OnPropertyChanged("NewQuantityError");
            }
        }
        #endregion

        #region Adress
        private string adress;
        public string Adress { get => adress; set { adress = value; ValidateAdress(); OnPropertyChanged("Adress"); } }

        private bool showAdressError;

        public bool ShowAdressError
        {
            get => showAdressError;
            set
            {
                showAdressError = value;
                OnPropertyChanged("ShowAdressError");
            }
        }


        private string adressError;

        public string AdressError
        {
            get => adressError;
            set
            {
                adressError = value;
                OnPropertyChanged("AdressError");
            }
        }

        private void ValidateAdress()
        {
            this.ShowAdressError = string.IsNullOrEmpty(Adress);
        }
        #endregion

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
            TotalPrice = 0;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            orderedDessertsKeeper = new();
            UserOrderedDesserts = new();
            IsEmpty = true;
            FillUserDesserts();            
            DeleteDessertCommand = new Command(OnDeleteDessert);
            ChangeQuantityCommand = new Command(OnChangeQuantity);
            UpdateQuantityCommand = new Command(OnUpdateQuantity);
            DeleteAllCommand = new Command(OnDeleteAll);
            OrderCommand = new Command(OnOrder);
            CancelCommand = new Command(OnCancel);
            LoadUserDessertsCommand = new Command(LoadUserDesserts);
            NewQuantityError = "New quantity must be a number.";
            AdressError = "Adress Is Required.";

        }

        private void OnCancel()
        {
            IsChanging = false;
            ShowNewQuantityError = false;
        }

        private async void GetOrderedDesserts()
        {
            orderedDessertsKeeper = await proxy.GetOrderedDesserts();
        }
        private async void FillUserDesserts()
        {
            this.TotalPrice = 0;
            UserOrderedDesserts.Clear();
            orderedDessertsKeeper.Clear();
            this.totalPrice = 0;
            orderedDessertsKeeper = await proxy.GetOrderedDesserts();

            foreach (OrderedDessert d in orderedDessertsKeeper)
            {
                if (d.UserId == LoggedInUser.UserId&&d.OrderId==null)
                {
                    UserOrderedDesserts.Add(d);
                    this.TotalPrice += d.Price;
                }

            }
            if (UserOrderedDesserts != null&&UserOrderedDesserts.Count>0)
            {
                IsEmpty = false;
            }
            else IsEmpty = true;
            IsRefreshing = false;
        }

        public async void OnDeleteDessert(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to delete the dessert from your cart?", "Yes", "Cancel"))
            {
                OrderedDessert d = (OrderedDessert)obj;
                bool isDeleted = await proxy.DeleteOD(d.OrderedDessertId);
                if (isDeleted)
                {
                    UserOrderedDesserts.Remove(((OrderedDessert)obj));
                }
                else
                {
                    AppShell.Current.DisplayAlert("Dessert", "Something went wrong.\nPlease try again later", "Ok");
                }
                if (userOrderedDesserts == null || userOrderedDesserts.Count==0)
                {
                    IsEmpty = true;
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
            o=await proxy.UpdateQuantity(d, int.Parse(NewQuantity));
            if (o != null)
            {
                string successMsg = "Quantity was successfully changed!";
                await Application.Current.MainPage.DisplayAlert("Changing Quantity", successMsg, "ok");
                IsRefreshing = true;
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
                IsEmpty = true;
            }
        }

        public async void OnOrder()
        {
            ValidateNewQuantity();
            ValidateAdress();
            if (!ShowNewQuantityError && !ShowAdressError&& UserOrderedDesserts != null && UserOrderedDesserts.Count>0)
            {
                Baker b = UserOrderedDesserts[0].TheBaker;
                //Create a new AppUser object with the data from the registration form
                var newOrder = new Order
                {
                    UserId = LoggedInUser.UserId,
                    BakerId = b.BakerId,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    ArrivalDate = null,
                    Adress = Adress,
                    TotalPrice = this.TotalPrice,
                    StatusCode = 1,
                    TheBaker=b,
                    TheUser=LoggedInUser
                };

                

                //Call the Register method on the proxy to register the new user
                InServerCall = true;
                newOrder = await proxy.AddOrder(newOrder);
                List<OrderedDessert> temp = UserOrderedDesserts.ToList();
                foreach (OrderedDessert d in temp)
                {
                    proxy.PutInOrder(d.OrderedDessertId, newOrder.Id);
                    UserOrderedDesserts.Remove(d);
                }

                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (newOrder != null)
                {
                    InServerCall = false;
                    
                    string successMsg = "Your order has been sent!";
                    await Application.Current.MainPage.DisplayAlert("Order", successMsg, "ok");
                }
                else
                {
                    string errorMsg1 = "Order has failed. Please try again.";
                    //If the registration failed, display an error message
                    if (UserOrderedDesserts ==null || userOrderedDesserts.Count == 0)
                    {
                        errorMsg1 = "Your cart is empty.";
                    }
                    await Application.Current.MainPage.DisplayAlert("Error", errorMsg1, "ok");
                }
            }
            string errorMsg = "Order has failed. Please try again.";
            //If the registration failed, display an error message
            if (UserOrderedDesserts == null || userOrderedDesserts.Count == 0)
            {
                errorMsg = "Your cart is empty.";
            }
            await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
        }
        private async void LoadUserDesserts()
        {
            IsChanging = false;
            Adress = null;
            FillUserDesserts();
            

        }

    }
}
