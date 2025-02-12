using AppClient.ViewModels;

namespace AppClient.Views;

public partial class PreviousOrdersPage : ContentPage
{
	public PreviousOrdersPage(PreviousOrdersPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}