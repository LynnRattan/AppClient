using AppClient.Views;

namespace AppClient
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("SignUp", typeof(SignUpPage));
            InitializeComponent();
        }
    }
}
