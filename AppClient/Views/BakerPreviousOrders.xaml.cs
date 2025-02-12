using AppClient.ViewModels;

namespace AppClient.Views;

public partial class BakerPreviousOrders : ContentPage
{
	public BakerPreviousOrders(BakerPreviousOrdersViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}