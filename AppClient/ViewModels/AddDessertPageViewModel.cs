using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
//using Javax.Security.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.ViewModels
{
    public class AddDessertPageViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;
        public AddDessertPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInBaker = ((App)Application.Current).LoggedInBaker;
            bakerDesserts = new List<Dessert>();
            FillBakerDesserts();
            AddDessertCommand = new Command(OnAddDessert);
            CancelCommand = new Command(OnCancel);
            UploadPhotoCommand = new Command(OnUploadPhoto);
            PhotoURL = proxy.GetDefaultDessertPhotoUrl();
            LocalPhotoPath = "";
            DessertNameError = "";
            PriceError = "Price must be a number.";

        }

        private List<Dessert> bakerDesserts;

        private bool isCakeChecked;
        private bool isCupcakeChecked;
        private bool isCookieChecked;
        private bool isPastryChecked;
        private Image DessertImage;

        public Baker? LoggedInBaker { get; set; }

        public Command AddDessertCommand { get; }
        public Command CancelCommand { get; }

        #region DessertName
        private bool showDessertNameError;

        public bool ShowDessertNameError
        {
            get => showDessertNameError;
            set
            {
                showDessertNameError = value;
                OnPropertyChanged("ShowDessertNameError");
            }
        }

        private string dessertName;

        public string DessertName
        {
            get => dessertName;
            set
            {
                dessertName = value;
                ValidateDessertName();
                OnPropertyChanged("DessertName");
            }
        }

        private string dessertNameError;

        public string DessertNameError
        {
            get => dessertNameError;
            set
            {
                dessertNameError = value;
                OnPropertyChanged("DessertNameError");
            }
        }

        private void ValidateDessertName()
        {
            bool exists=false;            
            foreach (Dessert d in bakerDesserts)
            {
                if (d.DessertName == this.DessertName)
                    exists = true;
            }
            if (exists)
            {
                DessertNameError = "Dessert name already exists.";
                this.ShowDessertNameError = true;
            }
            else if (string.IsNullOrEmpty(DessertName))
            {
                this.ShowDessertNameError = true;
                DessertNameError = "Dessert name required.";
            }

            else
                this.ShowDessertNameError = false;
        }
        #endregion

        #region DessertType
        public bool IsCakeChecked
        {
            get { return isCakeChecked; }
            set
            {
                if (isCakeChecked != value)
                {
                    isCakeChecked = value;
                    OnPropertyChanged(nameof(IsCakeChecked));
                }
            }
        }
        public bool IsCupcakeChecked
        {
            get { return isCupcakeChecked; }
            set
            {
                if (isCupcakeChecked != value)
                {
                    isCupcakeChecked = value;
                    OnPropertyChanged(nameof(IsCupcakeChecked));
                }
            }
        }
        public bool IsCookieChecked
        {
            get { return isCookieChecked; }
            set
            {
                if (isCookieChecked != value)
                {
                    isCookieChecked = value;
                    OnPropertyChanged(nameof(IsCookieChecked));
                }
            }
        }
        public bool IsPastryChecked
        {
            get { return isPastryChecked; }
            set
            {
                if (isPastryChecked != value)
                {
                    isPastryChecked = value;
                    OnPropertyChanged(nameof(IsPastryChecked));
                }
            }
        }
        #endregion

        #region DessertPrice
        private string price;

        public string Price
        {
            get => price;
            set
            {
                price = value;
                ValidatePrice();
                OnPropertyChanged(nameof(Price));
            }
        }

        private bool showPriceError;

        public bool ShowPriceError
        {
            get => showPriceError;
            set
            {
                showPriceError = value;
                OnPropertyChanged("ShowPriceError");
            }
        }

        private void ValidatePrice()
        {
            double d = 0;
            if ((string.IsNullOrEmpty(Price) || !double.TryParse(this.price, out d)))
            {
                this.ShowPriceError = true;
            }
            else
                this.ShowPriceError = false;

        }

        private string priceError;

        public string PriceError
        {
            get => priceError;
            set
            {
                priceError = value;
                OnPropertyChanged("PriceError");
            }
        }

        #endregion

        #region Photo

        private string photoURL;

        public string PhotoURL
        {
            get => photoURL;
            set
            {
                photoURL = value;
                OnPropertyChanged("PhotoURL");
            }
        }

        private string localPhotoPath;

        public string LocalPhotoPath
        {
            get => localPhotoPath;
            set
            {
                localPhotoPath = value;
                OnPropertyChanged("LocalPhotoPath");
            }
        }

        public Command UploadPhotoCommand { get; }
        //This method open the file picker to select a photo
        private async void OnUploadPhoto()
        {
            try
            {
                var result = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Please select a photo",
                });

                if (result != null)
                {
                    // The user picked a file
                    this.LocalPhotoPath = result.FullPath;
                    this.PhotoURL = result.FullPath;
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void UpdatePhotoURL(string virtualPath)
        {
            Random r = new Random();
            PhotoURL = proxy.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
        }

        #endregion



        public async void FillBakerDesserts()
        {
            bakerDesserts = await proxy.GetBakerDesserts(LoggedInBaker.BakerId);
        }

        public async void OnAddDessert()
        {
            ValidateDessertName();
            ValidatePrice();
            if (!ShowDessertNameError && !ShowPriceError)
            {
                int dessertType;

                if (IsCakeChecked)
                    dessertType = 1;
                else if (IsCupcakeChecked)
                    dessertType = 2;
                else if (IsCookieChecked)
                    dessertType = 3;
                else dessertType = 4;



                //Create a new AppUser object with the data from the registration form
                var newDessert = new Dessert
                {
                    DessertName = this.DessertName,
                    DessertTypeId = dessertType,
                    Price = double.Parse(this.Price),
                    StatusCode = 1,
                    BakerId = ((App)Application.Current).LoggedInBaker.UserId
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
                            Dessert? updatedDessert = await proxy.UploadDessertImage(LocalPhotoPath,newDessert.DessertId,LoggedInBaker.BakerId);
                            if (updatedDessert == null)
                            {
                                InServerCall = false;
                                await Application.Current.MainPage.DisplayAlert("Sign Up", "Dessert Data Was Saved BUT dessert image upload failed", "ok");
                            }
                            else
                            {
                                newDessert=updatedDessert;                            
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
