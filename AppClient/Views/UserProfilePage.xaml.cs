using AppClient.ViewModels;

namespace AppClient.Views;

public partial class UserProfilePage : ContentPage
{
	public UserProfilePage(UserProfilePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}