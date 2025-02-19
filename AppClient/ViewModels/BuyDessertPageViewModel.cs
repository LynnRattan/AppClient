using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.ViewModels
{
    [QueryProperty(nameof(SelectedDessert), "SelectedDessert")]
    public class BuyDessertPageViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;
        public BuyDessertPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            IsOneCon = true;
            LoggedInUser = ((App)Application.Current).LoggedInUser;
            AddToCartCommand = new Command(OnAddToCart);
            CancelCommand = new Command(OnCancel);
            QuantityError = "Quantity must be a number.";

        }

        private Dessert selectedDessert;
        public Dessert SelectedDessert
        {
            get => selectedDessert;
            set
            {
                selectedDessert = value;
                OnPropertyChanged();
            }
        }
        public User? LoggedInUser { get; set; }

        private bool isOneCon;
        public bool IsOneCon
        {
            get => isOneCon;
            set
            {
                isOneCon = value;
                OnPropertyChanged();
            }
        }

        public Command AddToCartCommand { get; }
        public Command CancelCommand { get; }


        #region check if the dessert is from the same confectionery
        private async Task<bool> CheckIfSameCon()
        {
            int bakerID = 0;
            List<OrderedDessert> UserDesserts = new List<OrderedDessert>();
            List<OrderedDessert> temp = await proxy.GetOrderedDesserts();
            foreach (OrderedDessert d in temp)
            {
                if (d.UserId == LoggedInUser.UserId && d.OrderId == null)
                {
                    UserDesserts.Add(d);
                }
            }

            if (UserDesserts.Count > 0 && SelectedDessert.BakerId != UserDesserts[0].BakerId)
            {
                return false;
            }
            return true;

        }
        #endregion

        #region check if the dessert has been ordered already
        private async Task<bool> CheckIfExists()
        {
            int dessertID = 0;
            List<OrderedDessert> UserDesserts = new List<OrderedDessert>();
            List<OrderedDessert> temp = await proxy.GetOrderedDesserts();
            foreach (OrderedDessert d in temp)
            {
                if (d.UserId == LoggedInUser.UserId && d.OrderId == null)
                {
                    UserDesserts.Add(d);
                }
            }

            if (UserDesserts.Count > 0)
            {
                foreach (OrderedDessert d in UserDesserts)
                    if (SelectedDessert.DessertId == d.DessertId)
                        return false;
            }
            return true;

        }
        #endregion


        #region DessertQuantity
        private string quantity;

        public string Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                ValidateQuantity();
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private bool showQuantityError;

        public bool ShowQuantityError
        {
            get => showQuantityError;
            set
            {
                showQuantityError = value;
                OnPropertyChanged("ShowQuantityError");
            }
        }

        private void ValidateQuantity()
        {
            int d = 0;
            if ((string.IsNullOrEmpty(Quantity) || !int.TryParse(this.quantity, out d)))
            {
                this.ShowQuantityError = true;
            }
            else
                this.ShowQuantityError = false;

        }

        private string quantityError;

        public string QuantityError
        {
            get => quantityError;
            set
            {
                quantityError = value;
                OnPropertyChanged("QuantityError");
            }
        }

        #endregion





        public async void OnAddToCart()
        {
            bool b1 = await CheckIfSameCon();
            bool b2 = await CheckIfExists();
            ValidateQuantity();
            if (!ShowQuantityError && b1 && b2)
            {
                Baker b = await proxy.GetBaker(SelectedDessert.BakerId);

                //Create a new AppUser object with the data from the registration form
                var newOrderedDessert = new OrderedDessert
                {
                    DessertId = SelectedDessert.DessertId,
                    OrderId = null,
                    Quantity = int.Parse(this.Quantity),
                    StatusCode = 1,
                    UserId = ((App)Application.Current).LoggedInUser.UserId,
                    BakerId = SelectedDessert.BakerId,
                    OrderedDessertImage = SelectedDessert.DessertImage,
                    Price = SelectedDessert.Price * int.Parse(Quantity),
                    TheBaker = b,
                    TheDessert = SelectedDessert
                };


                //Call the Register method on the proxy to register the new user
                InServerCall = true;

                newOrderedDessert = await proxy.AddOrderedDessert(newOrderedDessert);
                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (newOrderedDessert != null)
                {
                    InServerCall = false;

                    string successMsg = "Dessert has been added to cart!";
                    await Application.Current.MainPage.DisplayAlert(successMsg, "You are able to see the dessert in your cart.", "ok");
                }
                else
                {
                    string errorMsg = "Adding the dessert to cart has failed.Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
                }
            }
            else
            {

                if (!b1)
                {
                    string errorMsg = "You cannot order from different confectioneries.";
                    await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
                }
                else if (!b2)
                {
                    string errorMsg = "You cannot order the same dessert twice.";
                    await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
                }
            

            }
             ((App)Application.Current).MainPage.Navigation.PopAsync();

        }


                public void OnCancel()
                {
                    // Navigate to the Baker profile View page
                    ((App)Application.Current).MainPage.Navigation.PopAsync();

                }
    }
}
    

