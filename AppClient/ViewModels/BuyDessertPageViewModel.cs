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
    public class BuyDessertPageViewModel:ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;
        public BuyDessertPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
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

        public Command AddToCartCommand { get; }
        public Command CancelCommand { get; }

       

        

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
            ValidateQuantity();
            if (!ShowQuantityError)
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
                    await Application.Current.MainPage.DisplayAlert(successMsg,"You are able to see the dessert in your cart.", "ok");
                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Adding a dessert to cart failed. Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
                }
                // Navigate to the Baker profile View page for User
                //ViewConfectioneryPage vcp = serviceProvider.GetService<ViewConfectioneryPage>();
                ((App)Application.Current).MainPage.Navigation.PopAsync();
            }
        }




        public void OnCancel()
        {
            // Navigate to the Baker profile View page
            ((App)Application.Current).MainPage.Navigation.PopAsync();

        }
    }
}
    

