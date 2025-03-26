using AppClient.ViewModels;

namespace AppClient.Views;

public partial class PendingDessertsPage : ContentPage
{
	public PendingDessertsPage(PendingDessertsPageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PendingDessertsPageViewModel vm = (PendingDessertsPageViewModel)this.BindingContext;
        LoadPendingDesserts(vm);
    }
    private async void LoadPendingDesserts(PendingDessertsPageViewModel vm)
    {
        await vm.LoadPendingDesserts();
    }
}