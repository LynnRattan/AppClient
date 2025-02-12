using AppClient.ViewModels;

namespace AppClient.Views;

public partial class ViewNewOrder : ContentPage
{
	public ViewNewOrder(ViewNewOrderViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}