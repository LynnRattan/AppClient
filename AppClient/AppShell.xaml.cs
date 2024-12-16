using AppClient.ViewModels;
using AppClient.Views;

namespace AppClient
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {

            this.BindingContext = vm;
            InitializeComponent();
            RegisterRoutes();

        }
        private void RegisterRoutes()
        {
        Routing.RegisterRoute("SignUp", typeof(SignUpPage));
        Routing.RegisterRoute("Login", typeof(LoginPage));
        Routing.RegisterRoute("AddDessert", typeof(AddDessertPage));
        }
        
    }
}
