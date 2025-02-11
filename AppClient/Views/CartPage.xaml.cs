using AppClient.ViewModels;

namespace AppClient.Views;

public partial class CartPage : ContentPage
{
	public CartPage(CartPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}