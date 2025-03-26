using AppClient.ViewModels;

namespace AppClient.Views;

public partial class CartPage : ContentPage
{
	public CartPage(CartPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CartPageViewModel vm = (CartPageViewModel)this.BindingContext;
        LoadUserDesserts(vm);
    }
    private async void LoadUserDesserts(CartPageViewModel vm)
    {
        await vm.LoadUserDesserts();
    }
}