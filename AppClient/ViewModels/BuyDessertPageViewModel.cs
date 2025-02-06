using AppClient.Models;
using AppClient.Services;
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
            get => QuantityError;
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

                //Create a new AppUser object with the data from the registration form
                var newOrderedDessert = new OrderedDessert
                {
                    DessertId = SelectedDessert.DessertId,
                    OrderID = 0,
                    Quantity = int.Parse(this.Quantity),
                    StatusCode = 1,
                    BakerId = ((App)Application.Current).LoggedInUser.UserId
                };


                //Call the Register method on the proxy to register the new user
                InServerCall = true;

                newDessert = await proxy.AddDessert(newDessert);
                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (newDessert != null)
                {
                    //UPload profile imae if needed
                    if (!string.IsNullOrEmpty(LocalPhotoPath))
                    {
                        Dessert? updatedDessert = await proxy.UploadDessertImage(LocalPhotoPath, newDessert.DessertId, LoggedInBaker.BakerId);
                        if (updatedDessert == null)
                        {
                            InServerCall = false;
                            await Application.Current.MainPage.DisplayAlert("Sign Up", "Dessert Data Was Saved BUT dessert image upload failed", "ok");
                        }
                        else
                        {
                            newDessert = updatedDessert;
                        }

                        InServerCall = false;
                    }

                    string successMsg = "Adding a dessert succeeded!";
                    await Application.Current.MainPage.DisplayAlert("Adding a dessert", successMsg, "ok");
                    if (double.Parse(this.Price) > LoggedInBaker.HighestPrice)
                    {
                        if (await AppShell.Current.DisplayAlert("Dessert price is higher than your highest price.", "Would you like to change you highest price?", "Yes", "Cancel"))
                        {
                            LoggedInBaker.HighestPrice = double.Parse(this.price);
                            proxy.UpdateHighestPrice(LoggedInBaker);

                        }
                        else
                            OnCancel();
                    }

                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Adding a dessert failed. Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Adding a dessert", errorMsg, "ok");
                }
                // Navigate to the Baker profile View page
                ConProfilePage cp = serviceProvider.GetService<ConProfilePage>();
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
    

