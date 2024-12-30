using AppClient.ViewModels;

namespace AppClient.Views;

public partial class PendingConfectioneriesPage : ContentPage
{
	public PendingConfectioneriesPage(PendingConfectioneriesPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}