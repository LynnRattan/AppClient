using AppClient.ViewModels;

namespace AppClient.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}