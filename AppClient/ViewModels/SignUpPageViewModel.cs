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
//using static Java.Util.Jar.Attributes;

namespace AppClient.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        private string username;
        private int? userId { get; set; }
        private string? user_error;
        private string mail;
        private string password;
        private string? password_error;
        private string user_type;
        private bool isConChecked;
        private readonly IServiceProvider serviceProvider;
        private string profileName;
        private double highestPrice;
        private string confectioneryType;
        private Image profileImage;
        private string? highestPrice_error;
        private bool isImageSelected;


        public ICommand SelectImageCommand { get; set; }

        //// הוספת אובייקט ממחלקת השירותים שיוכל להפעיל את הפונקציות במחלקה
        //private WebApi api_service;


        public SignUpPageViewModel(IServiceProvider serviceProvider)
        {
            //this.api_service = api_service;
            //SignUpCommand = new Command(SignUp);
            this.serviceProvider = serviceProvider;
        }

        public string ProfileName
        {
            get
            {
                return profileName;
            }
            set
            {
                profileName = value;

                OnPropertyChanged(nameof(ProfileName));

            }
        }


        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                User_Error = ""; // איפוס שגיאת שם המשתמש
                OnPropertyChanged(nameof(Username));
                // בדיקת תקינות שם המשתמש

                if (!string.IsNullOrEmpty(Username))

                {
                    if (char.IsDigit(Username[0]))
                    {
                        User_Error = "!!שם המשתמש לא יכול להתחיל בספרה!!";
                        OnPropertyChanged(nameof(Username));
                    }

                }
            }
        }

        public string User_Error
        {
            get
            {
                return user_error;
            }
            set
            {
                user_error = value;
                OnPropertyChanged(nameof(User_Error));
            }
        }

        public string Mail
        {
            get
            {
                return mail;
            }
            set
            {
                mail = value;
                OnPropertyChanged(nameof(Mail));
            }
        }

        public string? Password
        {
            get { return password; }
            set
            {
                password = value;
                Password_Error = "";
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(User_Error));
                if (string.IsNullOrEmpty(password))
                {
                    Password_Error = ""; // איפוס השגיאה אם השדה ריק
                }
                else
                {
                    if (password != null)
                    {
                        bool IsPasswordOk = IsValidPassword(password);
                        if (!IsPasswordOk)
                        {
                            Password_Error = "!!סיסמה חייבת להכיל לפחות אות גדולה אחת ומספר!!";
                        }
                    }
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            bool hasUpperCase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }

                if (hasUpperCase && hasDigit)
                {
                    break; // אם מצאנו כבר גם אות גדולה וגם ספרה, אפשר לעצור את הלולאה
                }
            }
            return hasUpperCase && hasDigit;

        }

        public string Password_Error
        {
            get { return password_error; }
            set
            {
                password_error = value;
                OnPropertyChanged(nameof(Password_Error));
                //OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string User_Type
        {
            get
            {
                return user_type;
            }
            set
            {
                user_type = value;
                OnPropertyChanged(nameof(User_Type));
            }
        }


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
        public string ConfectioneryType
        {
            get
            {
                return confectioneryType;
            }
            set
            {
                confectioneryType = value;
                OnPropertyChanged(nameof(ConfectioneryType));
            }
        }



        public Image ProfileImage
        {
            get { return profileImage; }
            set
            {
                profileImage = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }

        public double HighestPrice
        {
            get { return highestPrice; }
            set
            {
                try
                {
                    highestPrice = value;
                    OnPropertyChanged(nameof(HighestPrice));
                }
                catch (Exception ex)
                {
                    HighestPrice_Error = "!!מחיר חייב להיות מספר!!   ";
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string HighestPrice_Error
        {
            get
            {
                return HighestPrice_Error;
            }
            set
            {
                HighestPrice_Error = value;
                OnPropertyChanged(nameof(HighestPrice_Error));
            }
        }

        public bool IsImageSelected
        {
            get { return isImageSelected; }
            set
            {
                    isImageSelected = value;
                    OnPropertyChanged(nameof(IsImageSelected));
            }
        }

        public ICommand SignUpCommand
        {
            get; private set;
        }

        public async void SignUp()
        {
            Models.User user = new Models.User
            {

                Username = username,
                Mail = mail,
                Password = password,

            };


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

        }
    }
}
