using AppClient.ViewModels;

namespace AppClient.Views;

public partial class PendingConfectioneriesPage : ContentPage
{
	public PendingConfectioneriesPage(PendingConfectioneriesPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PendingConfectioneriesPageViewModel vm = (PendingConfectioneriesPageViewModel)this.BindingContext;
        LoadPendingConfectioneries(vm);
    }
    private async void LoadPendingConfectioneries(PendingConfectioneriesPageViewModel vm)
    {
        await vm.LoadPendingConfectioneries();
    }
}