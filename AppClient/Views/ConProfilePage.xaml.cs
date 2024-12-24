namespace AppClient.Views;
using AppClient.ViewModels;


public partial class ConProfilePage : ContentPage
{
	public ConProfilePage(ConProfilePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}