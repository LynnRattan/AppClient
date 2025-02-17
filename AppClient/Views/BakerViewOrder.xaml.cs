using AppClient.ViewModels;
using System.Runtime.CompilerServices;

namespace AppClient.Views;

public partial class BakerViewOrder : ContentPage
{
	public BakerViewOrder(BakerViewOrderViewModel vm)
	{ 
		this.BindingContext= vm;
		InitializeComponent();
	}
}