using AppClient.Models;
using AppClient.Services;
using AppClient.Views;
using Microsoft.Win32;
using System.Windows.Input;

namespace AppClient.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    private LMBWebApi proxy;
    //public AppClientWebApi service;
    private readonly IServiceProvider serviceProvider;
    public LoginPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.proxy = proxy;
        LoginCommand = new Command(OnLogin);
        GoToSignUpCommand = new Command(OnSignUp);
        mail = "";
        password = "";
        InServerCall = false;
        errorMsg = "";
    }

    

    public ICommand LoginCommand { get; set; }
    public ICommand GoToSignUpCommand { get; set; }
    private string mail;
    private string password;

    //public LoginPageViewModel(SweetBoxWebApi service, IServiceProvider serviceProvider)
    //{
    //    this.service = service;
    //    this.serviceProvider = serviceProvider;
    //    this.LoginCommand = new Command(OnLogin);
    //    this.GoToRegisterCommand = new Command(OnRegisterClicked);
    //}

    public string Mail
    {
        get => mail;
        set
        {
            if (mail != value)
            {
                mail = value;
                OnPropertyChanged(nameof(Mail));
                // can add more logic here like email validation etc.
                // can add error message property and set it here
            }
        }
    }
   
    public string Password
    {
        get => password;
        set
        {
            if (password != value)
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                // can add more logic here like email validation etc.
                // can add error message property and set it here
            }
        }
    }

    private string errorMsg;
    public string ErrorMsg
    {
        get => errorMsg;
        set
        {
            if (errorMsg != value)
            {
                errorMsg = value;
                OnPropertyChanged(nameof(ErrorMsg));
            }
        }
    }


    private async void OnLogin()
    {
        //Choose the way you want to blobk the page while indicating a server call
        InServerCall = true;
        ErrorMsg = "";
        //Call the server to login
        LoginInfo loginInfo = new LoginInfo { Mail = Mail, Password = Password };
        User? u = await this.proxy.LoginAsync(loginInfo);

        InServerCall = false;

        //Set the application logged in user to be whatever user returned (null or real user)
        ((App)Application.Current).LoggedInUser = u;
        if (u == null)
        {
            ErrorMsg = "Invalid mail or password";
        }
        else
        {
            ErrorMsg = "";
            //Navigate to the main page
            AppShell shell = serviceProvider.GetService<AppShell>();
            if (u.UserTypeId == 1)
                ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<UserProfilePage>());
            else if (u.UserTypeId == 2)
                ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<ConProfilePage>());
            else if (u.UserTypeId == 3)
                ((App)(Application.Current)).MainPage.Navigation.PushAsync(serviceProvider.GetService<AdminProfilePage>());
            ((App)Application.Current).MainPage = shell;
        }
    }

    private void OnSignUp()
    {
        ErrorMsg = "";
        Mail = "";
        Password = "";
        // Navigate to the Register View page
        ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<SignUpPage>());
    }



}