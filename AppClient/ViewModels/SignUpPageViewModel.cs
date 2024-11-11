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

namespace AppClient.ViewModels
{
    public class SignUpPageViewModel:ViewModelBase
    {
        private string username;
        private int? userId { get; set; }
        private string? user_error;
        private string mail;
        private string password;
        private string? password_error;
        private string userType;
        private bool isConChecked;
        private readonly IServiceProvider serviceProvider;
        private string profileName;
        private double highestPrice;
        private string confectioneryType;
        private Image profileImage;

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

        public string UserType
        {
            get
            {
                return userType;
            }
            set
            {
                userType = value;
                OnPropertyChanged(nameof(UserType));
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
                highestPrice = value;
                OnPropertyChanged(nameof(HighestPrice));
            }
        }


    }
}
