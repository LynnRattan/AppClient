using AppClient.ViewModels;

namespace AppClient.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}