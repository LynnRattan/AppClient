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
        private bool isConfectionerChecked;
        private bool isUserChecked;
        private readonly IServiceProvider serviceProvider;
        private string profileName;
        private double highestPrice;
        private bool isBakeryChecked;
        private bool isPatisserieChecked;
        private bool isHomemadeChecked;
        private bool isEverythingChecked;
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

        // מאפיין לבחירת "משתמש קונה"
        public bool IsUserChecked
        {
            get { return isConfectionerChecked; }
            set
            {
                if (isUserChecked != value)
                {
                    isUserChecked = value;
                    if (isUserChecked)
                    {
                        UserType = "1"; // עדכון ל-1 עבור משתמש קונה
                        IsConfectionerChecked = false; // Uncheck the Confectioner radio button
                    }
                    OnPropertyChanged(nameof(IsUserChecked));
                }
            }
        }

        // מאפיין לבחירת "קונדיטור"
        public bool IsConfectionerChecked
        {
            get { return isConfectionerChecked; }
            set
            {
                if (isConfectionerChecked != value)
                {
                    isConfectionerChecked = value;
                    if (isConfectionerChecked)
                    {
                        UserType = "2"; // עדכון ל-2 עבור קונדיטור
                        IsUserChecked = false; // Uncheck the User radio button
                    }
                    OnPropertyChanged(nameof(IsConfectionerChecked));
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

        public bool IsBakeryChecked
        {
            get { return isBakeryChecked; }
            set
            {
                if (isBakeryChecked != value)
                {
                    isBakeryChecked = value;
                    if (isBakeryChecked)
                    {
                        ConfectioneryType = "1"; // עדכון ל-1 עבור מאפייה
                        IsPatisserieChecked = false; // Uncheck the Patisserie radio button
                        IsHomemadeChecked = false; // Uncheck the Homemade radio button
                        IsEverythingChecked = false; // Uncheck the Everything radio button
                    }
                    OnPropertyChanged(nameof(IsBakeryChecked));
                }
            }
        }

        public bool IsPatisserieChecked
        {
            get { return isPatisserieChecked; }
            set
            {
                if (isPatisserieChecked != value)
                {
                    isPatisserieChecked = value;
                    if (isPatisserieChecked)
                    {
                        ConfectioneryType = "2"; // עדכון ל-2 עבור פטיסרי
                        IsBakeryChecked = false; // Uncheck the Bakery radio button
                        IsHomemadeChecked = false; // Uncheck the Homemade radio button
                        IsEverythingChecked = false; // Uncheck the Everything radio button
                    }
                    OnPropertyChanged(nameof(IsPatisserieChecked));
                }
            }
        }

        public bool IsHomemadeChecked
        {
            get { return isHomemadeChecked; }
            set
            {
                if (isHomemadeChecked != value)
                {
                    isHomemadeChecked = value;
                    if (isHomemadeChecked)
                    {
                        ConfectioneryType = "3"; // עדכון ל-3 עבור ביתית
                        IsPatisserieChecked = false; // Uncheck the Patisserie radio button
                        IsBakeryChecked = false; // Uncheck the Bakery radio button
                        IsEverythingChecked = false; // Uncheck the Everything radio button
                    }
                    OnPropertyChanged(nameof(IsHomemadeChecked));
                }
            }
        }

        public bool IsEverythingChecked
        {
            get { return isEverythingChecked; }
            set
            {
                if (isEverythingChecked != value)
                {
                    isEverythingChecked = value;
                    if (isEverythingChecked)
                    {
                        ConfectioneryType = "4"; // עדכון ל-4 עבור הכל
                        IsPatisserieChecked = false; // Uncheck the Patisserie radio button
                        IsHomemadeChecked = false; // Uncheck the Homemade radio button
                        IsBakeryChecked = false; // Uncheck the Bakery radio button
                    }
                    OnPropertyChanged(nameof(IsEverythingChecked));
                }
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
