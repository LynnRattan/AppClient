using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppClient.ViewModels
{
    public class AddDessertPageViewModel: ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private LMBWebApi proxy;
        public AddDessertPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            AddDessertCommand = new Command(OnAddDessert);
            CancelCommand = new Command(OnCancel);
            UploadPhotoCommand = new Command(OnUploadPhoto);
            PhotoURL = proxy.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            DessertNameError = "Dessert Name is required";

        }


        private bool isCakeChecked;
        private bool isCupcakeChecked;
        private bool isCookieChecked;
        private bool isPastryChecked;
        private Image DessertImage;

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
            this.ShowDessertNameError = string.IsNullOrEmpty(DessertName);
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
        private double price;

        public double Price
        {
            get => price;
            set
            {
                price = value;

                OnPropertyChanged(nameof(Price));
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



        public async void OnAddDessert()
        {

            if (!ShowDessertNameError)
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
                    Price = this.Price,
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
                        Dessert? updatedDessert = await proxy.UploadDessertImage(LocalPhotoPath);
                        if (updatedDessert == null)
                        {
                            InServerCall = false;
                        }
                        //}
                        InServerCall = false;
                    }

                    string successMsg = "Adding a dessert succeeded!";
                    await Application.Current.MainPage.DisplayAlert("Adding a dessert", successMsg, "ok");
                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Adding a dessert failed. Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Adding a dessert", errorMsg, "ok");
                }
            }
        }

        public void OnCancel()
        {
            // Navigate to the Baker profile View page
            ((App)Application.Current).MainPage.Navigation.PopAsync();

        }

    }
}
