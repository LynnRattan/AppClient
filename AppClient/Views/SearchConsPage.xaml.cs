using AppClient.ViewModels;

namespace AppClient.Views;

public partial class SearchConsPage : ContentPage
{
	public SearchConsPage(SearchConsPageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
	}
}