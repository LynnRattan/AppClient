using Android.OS.Strictmode;
using AppClient.ViewModels;

namespace AppClient.Views;

public partial class NewOrdersPage : ContentPage
{
	public NewOrdersPage(NewOrdersPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		NewOrdersPageViewModel vm = (NewOrdersPageViewModel)this.BindingContext;
		LoadBakers(vm);
    }
	private async void LoadBakers(NewOrdersPageViewModel vm)
	{
		await vm.LoadBakerOrders();
	}

}