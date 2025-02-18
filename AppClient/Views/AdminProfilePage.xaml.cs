using AppClient.ViewModels;

namespace AppClient.Views;

public partial class AdminProfilePage : ContentPage
{
	public AdminProfilePage(AdminProfilePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}