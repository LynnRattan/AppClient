using AppClient.ViewModels;

namespace AppClient.Views;

public partial class PendingDessertsPage : ContentPage
{
	public PendingDessertsPage(PendingDessertsPageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
	}
}