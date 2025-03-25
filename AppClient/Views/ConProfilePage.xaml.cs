namespace AppClient.Views;
using AppClient.ViewModels;


public partial class ConProfilePage : ContentPage
{
	public ConProfilePage(ConProfilePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ConProfilePageViewModel vm = (ConProfilePageViewModel)this.BindingContext;
        LoadBakerDesserts(vm);
    }
    private async void LoadBakerDesserts(ConProfilePageViewModel vm)
    {
        await vm.LoadBakerDesserts();
    }
}