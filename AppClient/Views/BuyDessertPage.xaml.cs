using AppClient.ViewModels;

namespace AppClient.Views;

public partial class BuyDessertPage : ContentPage
{
	public BuyDessertPage(BuyDessertPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}