using AppClient.ViewModels;

namespace AppClient.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}