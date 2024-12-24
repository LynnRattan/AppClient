using AppClient.Models;
using AppClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
        public class AppShellViewModel : ViewModelBase
        {

            private IServiceProvider serviceProvider;
            private bool isUser;
            private bool isConfectioner;
            private bool isAdmin;
           
            public AppShellViewModel(IServiceProvider serviceProvider)
            {
                this.LogoutCommand = new Command(OnLogout);
                this.serviceProvider = serviceProvider;
            }
           
            //on pressing logout in the shell bar on the left
            public ICommand LogoutCommand { get; set; }
           
            public bool IsUser
            {
                get
                {
                    if ((App)Application.Current == null)
                        return true;
                    else if (((App)Application.Current).LoggedInUser.UserTypeId==1)
                        return true;
                    else
                        return false;
                }
            }
           
            public bool IsConfectioner
            {
                get
                {
                    if ((App)Application.Current == null)
                        return true;
                    else if (((App)Application.Current).LoggedInUser.UserTypeId == 2)
                        return true;
                    else
                        return false;
                }
            }
           
            public bool IsAdmin
            {
                get
                {
                    if ((App)Application.Current == null)
                        return true;
                    else if (((App)Application.Current).LoggedInUser.UserTypeId == 3)
                        return true;
                    else
                        return false;
                }
            }
           
           
            
           
            public void OnLogout()
            {
                ((App)Application.Current).LoggedInUser = null;
           
                ((App)Application.Current).MainPage = new NavigationPage(serviceProvider.GetService<LoginPage>());
            }
           
           
         
        }

}



//private User? currentUser;
//private IServiceProvider serviceProvider;
//public AppShellViewModel(IServiceProvider serviceProvider)
//{
//    this.serviceProvider = serviceProvider;
//    this.currentUser = ((App)Application.Current).LoggedInUser;
//}


////this command will be used for logout menu item
//public Command LogoutCommand
//{
//    get
//    {
//        return new Command(OnLogout);
//    }
//}
////this method will be trigger upon Logout button click
//public void OnLogout()
//{
//    ((App)Application.Current).LoggedInUser = null;

//    ((App)Application.Current).MainPage = new NavigationPage(serviceProvider.GetService<LoginPage>());
//}