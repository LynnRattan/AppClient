using AppClient.ViewModels;

namespace AppClient.Views;

public partial class NewOrdersPage : ContentPage
{
	public NewOrdersPage(NewOrdersPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}