using AppClient.ViewModels;

namespace AppClient.Views;

public partial class ViewConfectioneryPage : ContentPage
{
	public ViewConfectioneryPage(ViewConfectioneryPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}