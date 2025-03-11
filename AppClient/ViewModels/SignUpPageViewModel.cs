using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppClient.Services;
using AppClient.Models;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AppClient.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using AppClient.ModelsExt;
using Microsoft.Maui.Graphics.Text;
//using static Java.Util.Jar.Attributes;

namespace AppClient.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        public SignUpPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            users = new();
            FillUsers();
            SignUpCommand = new Command(OnSignUp);
            CancelCommand = new Command(OnCancel);
            UploadPhotoCommand = new Command(OnUploadPhoto);
            PhotoURL = proxy.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            IsPassword = true;
            ProfileNameError = "Profile Name is required";
            UsernameError = "Username is required";
            MailError = "";
            PasswordError = "Password must be at least 4 characters long and contain letters and numbers";
            ConfectioneryNameError= "Confectionery Name is required";
            HighestPriceError = "Highest price must be a number.";


        }


        private bool isConChecked;
        private readonly IServiceProvider serviceProvider;

        private List<User> users;

       
        private bool isBakeryChecked;
        private bool isPatisserieChecked;
        private bool isHomemadeChecked;
        private bool isEverythingChecked;
        private Image profileImage;
       



        //Defiine properties for each field in the registration form including error messages and validation logic
        #region Username
        private bool showUsernameError;

        public bool ShowUsernameError
        {
            get => showUsernameError;
            set
            {
                showUsernameError = value;
                OnPropertyChanged("ShowUsernameError");
            }
        }

        private string username;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                ValidateUsername();
                OnPropertyChanged("Username");
            }
        }

        private string usernameError;

        public string UsernameError
        {
            get => usernameError;
            set
            {
                usernameError = value;
                OnPropertyChanged("UsernameError");
            }
        }

        private void ValidateUsername()
        {
            this.ShowUsernameError = string.IsNullOrEmpty(Username);
        }
        #endregion

        #region ProfileName
        private string profileName;

        public string ProfileName
        {
            get => profileName;
            set
            {
                profileName = value;
                ValidateProfileName();
                OnPropertyChanged("ProfileName");
            }
        }

        private bool showProfileNameError;

        public bool ShowProfileNameError
        {
            get => showProfileNameError;
            set
            {
                showProfileNameError = value;
                OnPropertyChanged("ShowProfileNameError");
            }
        }

        private void ValidateProfileName()
        {
            this.ShowProfileNameError = string.IsNullOrEmpty(ProfileName);
        }

        private string profileNameError;

        public string ProfileNameError
        {
            get => profileNameError;
            set
            {
                profileNameError = value;
                OnPropertyChanged("ProfileNameError");
            }
        }


        #endregion

        #region Mail
        private string mail;

        public string Mail
        {
            get => mail;
            set
            {
                mail = value;
                ValidateMail();
                OnPropertyChanged("Mail");
            }
        }

        private string mailError;

        public string MailError
        {
            get => mailError;
            set
            {
                mailError = value;
                OnPropertyChanged("MailError");
            }
        }

        private bool showMailError;

        public bool ShowMailError
        {
            get => showMailError;
            set
            {
                showMailError = value;
                OnPropertyChanged("ShowMailError");
            }
        }

        private void ValidateMail()
        {
            bool exists = false;
            foreach (User u in users)
            {
                if (u.Mail == this.Mail)
                    exists = true;
            }
            if (exists)
            {
               MailError = "Mail already exists.";
                this.ShowMailError = true;
            }
            else if (string.IsNullOrEmpty(Mail))
            {
                this.ShowMailError = true;
                MailError = "Mail is required.";
            }
                //check if Mail is in the correct format using regular expression
            else  if (!System.Text.RegularExpressions.Regex.IsMatch(Mail, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                MailError = "Mail is not valid";
                ShowMailError = true;
            }
            else
            {
                MailError = "";
                ShowMailError = false;
            }
        }
        #endregion

        #region Password
        private bool showPasswordError;

        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string password;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                ValidatePassword();
                OnPropertyChanged("Password");
            }
        }

        private string passwordError;

        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private void ValidatePassword()
        {
            //Password must include characters and numbers and be longer than 4 characters
            if (string.IsNullOrEmpty(password) ||
                password.Length < 4 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsLetter))
            {
                this.ShowPasswordError = true;
            }
            else
                this.ShowPasswordError = false;
        }

        //This property will indicate if the password entry is a password
        private bool isPassword = true;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        //This command will trigger on pressing the password eye icon
        public Command ShowPasswordCommand { get; }
        //This method will be called when the password eye icon is pressed
        public void OnShowPassword()
        {
            //Toggle the password visibility
            IsPassword = !IsPassword;
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

        #region IsConChecked
        // מאפיין לבחירת "קונדיטור"
        public bool IsConChecked
        {
            get { return isConChecked; }
            set
            {
                if (isConChecked != value)
                {
                    isConChecked = value;
                    OnPropertyChanged(nameof(IsConChecked));
                }
            }
        }
        #endregion

        #region ConfectioneryType
        public bool IsBakeryChecked
        {
            get { return isBakeryChecked; }
            set
            {
                if (isBakeryChecked != value)
                {
                    isBakeryChecked = value;
                    OnPropertyChanged(nameof(IsBakeryChecked));
                }
            }
        }public bool IsPatisserieChecked
        {
            get { return isPatisserieChecked; }
            set
            {
                if (isPatisserieChecked != value)
                {
                    isPatisserieChecked = value;
                    OnPropertyChanged(nameof(IsPatisserieChecked));
                }
            }
        }public bool IsHomemadeChecked
        {
            get { return isHomemadeChecked; }
            set
            {
                if (isHomemadeChecked != value)
                {
                    isHomemadeChecked = value;
                    OnPropertyChanged(nameof(IsHomemadeChecked));
                }
            }
        }public bool IsEverythingChecked
        {
            get { return isEverythingChecked; }
            set
            {
                if (isEverythingChecked != value)
                {
                    isEverythingChecked = value;
                    OnPropertyChanged(nameof(IsEverythingChecked));
                }
            }
        }
        #endregion

        #region ConfectioneryName
        private string confectioneryName;

        public string ConfectioneryName
        {
            get => confectioneryName;
            set
            {
                confectioneryName = value;
                ValidateConfectioneryName();
                OnPropertyChanged("ConfectioneryName");
            }
        }

        private bool showConfectioneryNameError;

        public bool ShowConfectioneryNameError
        {
            get => showConfectioneryNameError;
            set
            {
                showConfectioneryNameError = value;
                OnPropertyChanged("ShowConfectioneryNameError");
            }
        }

        private void ValidateConfectioneryName()
        {
            if (IsConChecked)
                this.ShowConfectioneryNameError = string.IsNullOrEmpty(ConfectioneryName);
            else
                this.ShowConfectioneryNameError = false;
        }

        private string confectioneryNameError;

        public string ConfectioneryNameError
        {
            get => confectioneryNameError;
            set
            {
                confectioneryNameError = value;
                OnPropertyChanged("confectioneryNameError");
            }
        }
        #endregion

        #region HighestPrice
        private string highestPrice;

        public string HighestPrice
        {
            get => highestPrice;
            set
            {
                highestPrice = value;
                ValidateHighestPrice();
                OnPropertyChanged(nameof(HighestPrice));
            }
        }

        private bool showHighestPriceError;

        public bool ShowHighestPriceError
        {
            get => showHighestPriceError;
            set
            {
                showHighestPriceError = value;
                OnPropertyChanged("ShowHighestPriceError");
            }
        }

        private void ValidateHighestPrice()
        {
            double d = 0;
            if (IsConChecked)
            {

                if ((string.IsNullOrEmpty(HighestPrice) || !double.TryParse(this.highestPrice, out d)))
                {
                    this.ShowHighestPriceError = true;
                }
                else
                    this.ShowHighestPriceError = false;
            }
            else this.ShowHighestPriceError = false;
            
        }

        private string highestPriceError;

        public string HighestPriceError
        {
            get => highestPriceError;
            set
            {
                highestPriceError = value;
                OnPropertyChanged("HighestPriceError");
            }
        }
        #endregion

        #region Profits
        private double profits;
        public double Profits;
        #endregion


        public async void FillUsers()
        {
            users = await proxy.GetUsers();
        }

        //Define a command for the register button
        public Command SignUpCommand { get; }
        public Command CancelCommand { get; }

        //Define a method that will be called when the register button is clicked
        public async void OnSignUp()
        {
            ValidateUsername();
            ValidateProfileName();
            ValidateMail();
            ValidatePassword();
            ValidateConfectioneryName();
            ValidateHighestPrice();


            if (!ShowUsernameError && !ShowProfileNameError && !ShowMailError && !ShowPasswordError && !ShowConfectioneryNameError && !ShowHighestPriceError)
            {
                int userType;
                int conType;
                if (IsConChecked)
                {
                    userType = 2;
                    if (IsBakeryChecked)
                        conType = 1;
                    else if (IsPatisserieChecked)
                        conType = 2;
                    else if (IsHomemadeChecked)
                        conType = 3;
                    else conType = 4;
                }
                else
                {
                    userType = 1;
                    conType = 0;
                }

                //Create a new AppUser object with the data from the registration form
                var newUserBaker = new UserBaker
                {
                    Username = this.Username,
                    ProfileName = this.ProfileName,
                    Mail = this.Mail,
                    Password = this.Password,
                    UserTypeId = userType,
                    ProfileImagePath=""
                };
               
                //Call the Register method on the proxy to register the new user
                InServerCall = true;
                if (newUserBaker.UserTypeId == 2)
                {
                    newUserBaker.ConfectioneryName = this.ConfectioneryName;
                    newUserBaker.HighestPrice = double.Parse(this.HighestPrice);
                    newUserBaker.ConfectioneryTypeId = conType;
                    newUserBaker.StatusCode = 1;
                    newUserBaker.Profits = 0;
                }
                newUserBaker = await proxy.SignUp(newUserBaker);
                
                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (newUserBaker != null)
                {
                    //UPload profile imae if needed
                    if (!string.IsNullOrEmpty(LocalPhotoPath))
                    {
                        await proxy.LoginAsync(new LoginInfo { Mail = newUserBaker.Mail, Password = newUserBaker.Password });
                        User? updatedUser = await proxy.UploadProfileImage(LocalPhotoPath);
                        if (updatedUser == null)
                        {
                            InServerCall = false;
                            await Application.Current.MainPage.DisplayAlert("Sign Up", "User Data Was Saved BUT Profile image upload failed", "ok");
                        }
                        else
                            newUserBaker.ProfileImagePath = updatedUser.ProfileImagePath;
                    }
                    InServerCall = false;

                    ((App)Application.Current).LoggedInUser = newUserBaker;
                    AppShell shell = serviceProvider.GetService<AppShell>();

                    string successMsg = "Welcome to LET ME BAKE!";
                    await Application.Current.MainPage.DisplayAlert("Sign Up Succeeded!", successMsg, "ok");
                    if (newUserBaker.UserTypeId == 1)
                        ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<UserProfilePage>());
                    else if (newUserBaker.UserTypeId == 2)
                    {
                        ((App)Application.Current).LoggedInBaker = await proxy.GetBaker(newUserBaker.UserId);
                        ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<ConProfilePage>());
                    }
                    ((App)Application.Current).MainPage = shell;
                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Sign Up failed. Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Sign Up", errorMsg, "ok");
                }
            }
        }

        //Define a method that will be called upon pressing the cancel button
        public void OnCancel()
        {
            //Navigate back to the login page
            ((App)(Application.Current)).MainPage.Navigation.PopAsync();
        }

    }
}
















    

    //public ICommand SelectImageCommand { get; set; }

    ////// הוספת אובייקט ממחלקת השירותים שיוכל להפעיל את הפונקציות במחלקה
    ////private WebApi api_service;


    //public SignUpPageViewModel(IServiceProvider serviceProvider)
    //{
    //    //this.api_service = api_service;
    //    //SignUpCommand = new Command(SignUp);
    //    this.serviceProvider = serviceProvider;
    //}

    //public string ProfileName
    //{
    //    get
    //    {
    //        return profileName;
    //    }
    //    set
    //    {
    //        profileName = value;

    //        OnPropertyChanged(nameof(ProfileName));

    //    }
    //}


    //public string Username
    //{
    //    get
    //    {
    //        return username;
    //    }
    //    set
    //    {
    //        username = value;
    //        User_Error = ""; // איפוס שגיאת שם המשתמש
    //        OnPropertyChanged(nameof(Username));
    //        // בדיקת תקינות שם המשתמש

    //        if (!string.IsNullOrEmpty(Username))

    //        {
    //            if (char.IsDigit(Username[0]))
    //            {
    //                User_Error = "!!שם המשתמש לא יכול להתחיל בספרה!!";
    //                OnPropertyChanged(nameof(Username));
    //            }

    //        }
    //    }
    //}

    //public string User_Error
    //{
    //    get
    //    {
    //        return user_error;
    //    }
    //    set
    //    {
    //        user_error = value;
    //        OnPropertyChanged(nameof(User_Error));
    //    }
    //}

    //public string Mail
    //{
    //    get
    //    {
    //        return mail;
    //    }
    //    set
    //    {
    //        mail = value;
    //        OnPropertyChanged(nameof(Mail));
    //    }
    //}

    //public string? Password
    //{
    //    get { return password; }
    //    set
    //    {
    //        password = value;
    //        Password_Error = "";
    //        OnPropertyChanged(nameof(Password));
    //        OnPropertyChanged(nameof(User_Error));
    //        if (string.IsNullOrEmpty(password))
    //        {
    //            Password_Error = ""; // איפוס השגיאה אם השדה ריק
    //        }
    //        else
    //        {
    //            if (password != null)
    //            {
    //                bool IsPasswordOk = IsValidPassword(password);
    //                if (!IsPasswordOk)
    //                {
    //                    Password_Error = "!!סיסמה חייבת להכיל לפחות אות גדולה אחת ומספר!!";
    //                }
    //            }
    //        }
    //    }
    //}

    //private bool IsValidPassword(string password)
    //{
    //    bool hasUpperCase = false;
    //    bool hasDigit = false;

    //    foreach (char c in password)
    //    {
    //        if (char.IsUpper(c))
    //        {
    //            hasUpperCase = true;
    //        }
    //        if (char.IsDigit(c))
    //        {
    //            hasDigit = true;
    //        }

    //        if (hasUpperCase && hasDigit)
    //        {
    //            break; // אם מצאנו כבר גם אות גדולה וגם ספרה, אפשר לעצור את הלולאה
    //        }
    //    }
    //    return hasUpperCase && hasDigit;

    //}

    //public string Password_Error
    //{
    //    get { return password_error; }
    //    set
    //    {
    //        password_error = value;
    //        OnPropertyChanged(nameof(Password_Error));
    //        //OnPropertyChanged(nameof(CanRegister));
    //    }
    //}

    //public string User_Type
    //{
    //    get
    //    {
    //        return user_type;
    //    }
    //    set
    //    {
    //        user_type = value;
    //        OnPropertyChanged(nameof(User_Type));
    //    }
    //}


    //// מאפיין לבחירת "קונדיטור"
    //public bool IsConChecked
    //{
    //    get { return isConChecked; }
    //    set
    //    {
    //        if (isConChecked != value)
    //        {
    //            isConChecked = value;
    //            OnPropertyChanged(nameof(IsConChecked));
    //        }
    //    }
    //}
    //public string ConfectioneryType
    //{
    //    get
    //    {
    //        return confectioneryType;
    //    }
    //    set
    //    {
    //        confectioneryType = value;
    //        OnPropertyChanged(nameof(ConfectioneryType));
    //    }
    //}



    //public Image ProfileImage
    //{
    //    get { return profileImage; }
    //    set
    //    {
    //        profileImage = value;
    //        OnPropertyChanged(nameof(ProfileImage));
    //    }
    //}

    //public double HighestPrice
    //{
    //    get { return highestPrice; }
    //    set
    //    {
    //        try
    //        {
    //            highestPrice = value;
    //            OnPropertyChanged(nameof(HighestPrice));
    //        }
    //        catch (Exception ex)
    //        {
    //            HighestPrice_Error = "!!מחיר חייב להיות מספר!!   ";
    //            OnPropertyChanged(nameof(Username));
    //        }
    //    }
    //}

    //public string HighestPrice_Error
    //{
    //    get
    //    {
    //        return HighestPrice_Error;
    //    }
    //    set
    //    {
    //        HighestPrice_Error = value;
    //        OnPropertyChanged(nameof(HighestPrice_Error));
    //    }
    //}

    //public bool IsImageSelected
    //{
    //    get { return isImageSelected; }
    //    set
    //    {
    //            isImageSelected = value;
    //            OnPropertyChanged(nameof(IsImageSelected));
    //    }
    //}

    //public ICommand SignUpCommand
    //{
    //    get; private set;
    //}

    //public async void SignUp()
    //{
    //    Models.User user = new Models.User
    //    {

    //        Username = username,
    //        Mail = mail,
    //        Password = password,

    //    };


    //// check
    //int? res = await this.api_service.SignUp(user);
    //// אם ההרשמה הצליחה
    //if (res != null)
    //{
    //    // בדיקת סוג המשתמש
    //    if (User_Type == "2") // אם המשתמש הוא קונדיטור
    //    {

    //        // קבלת ה-SellerRegistrationPage וה-ViewModel דרך DI
    //        var ConfectionerSignUpPage = serviceProvider.GetRequiredService<ConfectionerSignUpPage>();
    //        var ConfectionerSignUpPageViewModel = serviceProvider.GetRequiredService<ConfectionerSignUpPageViewModel>();

    //        // אתחול ה-ViewModel עם ה-SellerId שנוצר
    //        ConfectionerSignUpPageViewModel.Initialize((int)res);

    //        // הגדרת ה-ViewModel כ-BindingContext של הדף
    //        ConfectionerSignUpPage.BindingContext = ConfectionerSignUpPageViewModel;
    //        await App.Current.MainPage.Navigation.PushAsync(ConfectionerSignUpPage);

    //    }
    //    else if (User_Type == "1") // אם המשתמש הוא קונה
    //    {
    //        // מעבר לדף BusinessesPage
    //        var CustomerHomePage = serviceProvider.GetRequiredService<CustomerHomePage>();
    //        await App.Current.MainPage.Navigation.PushAsync(CustomerPagePage);
    //    }
    //}
    //else
    //{
    //    // טיפול במקרה שההרשמה נכשלה (הודעת שגיאה למשתמש, למשל)
    //    await Application.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה, נסה שוב.", "אישור");
    //}


    

