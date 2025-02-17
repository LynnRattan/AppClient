using AppClient.ViewModels;

namespace AppClient.Views;

public partial class UserViewOrderPage : ContentPage
{
	public UserViewOrderPage(UserViewOrderViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}