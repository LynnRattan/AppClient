using System.Windows.Input;

namespace AppClient.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    //public AppClientWebApi service;
    private readonly IServiceProvider serviceProvider;

    public ICommand LoginCommand { get; set; }
    public ICommand GoToSignUpCommand { get; set; }
    private string mail;
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
    private string password;
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

    public LoginPageViewModel()
	{
		
	}
}