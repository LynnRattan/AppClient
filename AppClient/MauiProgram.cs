using AppClient.Services;
using AppClient.ViewModels;
using AppClient.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace AppClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterDataServices()
                .RegisterPages()
                .RegisterViewModels();
            
            
            
            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<UserProfilePage>();
            builder.Services.AddTransient<ConProfilePage>();
            builder.Services.AddTransient<AdminProfilePage>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<PendingConfectioneriesPage>();
            builder.Services.AddTransient<PendingDessertsPage>();
            builder.Services.AddTransient<PendingConfectioneriesPage>();
            builder.Services.AddTransient<PreviousOrdersPage>();
            builder.Services.AddTransient<CartPage>();
            builder.Services.AddTransient<PreviousPurchasesPage>();
            builder.Services.AddTransient<NewOrdersPage>();
            builder.Services.AddTransient<AddDessertPage>();
            builder.Services.AddTransient<SearchConsPage>();

            return builder;
        }

        public static MauiAppBuilder RegisterDataServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<LMBWebApi>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<SignUpPageViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<UserProfilePageViewModel>();
            builder.Services.AddTransient<ConProfilePageViewModel>();
            builder.Services.AddTransient<AdminProfilePageViewModel>();
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<PendingConfectioneriesPageViewModel>();
            builder.Services.AddTransient<PendingDessertsPageViewModel>();
            builder.Services.AddTransient<PendingConfectioneriesPageViewModel>();
            builder.Services.AddTransient<PreviousOrdersPageViewModel>();
            builder.Services.AddTransient<CartPageViewModel>();
            builder.Services.AddTransient<PreviousPurchasesPageViewModel>();
            builder.Services.AddTransient<NewOrdersPageViewModel>();
            builder.Services.AddTransient<AddDessertPageViewModel>();
            builder.Services.AddTransient<SearchConsPageViewModel>();




            return builder;
        }
    }

}
