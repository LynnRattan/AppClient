using AppClient.ViewModels;

namespace AppClient.Views;

public partial class PreviousOrdersPage : ContentPage
{
	public PreviousOrdersPage(PreviousOrdersPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PreviousOrdersPageViewModel vm = (PreviousOrdersPageViewModel)this.BindingContext;
        LoadUserOrders(vm);
    }
    private async void LoadUserOrders(PreviousOrdersPageViewModel vm)
    {
        await vm.LoadUserOrders();
    }
}