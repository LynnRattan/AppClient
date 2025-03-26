using AppClient.ViewModels;

namespace AppClient.Views;

public partial class BakerPreviousOrders : ContentPage
{
	public BakerPreviousOrders(BakerPreviousOrdersViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BakerPreviousOrdersViewModel vm = (BakerPreviousOrdersViewModel)this.BindingContext;
        LoadBakerOrders(vm);
    }
    private async void LoadBakerOrders(BakerPreviousOrdersViewModel vm)
    {
        await vm.LoadBakerOrders();
    }
}