using AppClient.ViewModels;

namespace AppClient.Views;

public partial class AddDessertPage : ContentPage
{
	public AddDessertPage(AddDessertPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}