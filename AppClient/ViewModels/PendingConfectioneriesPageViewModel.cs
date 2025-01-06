using AppClient.Models;
using AppClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
    public class PendingConfectioneriesPageViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<Baker> pendingConfectioneries;
        private List<Baker> pendingConfectioneriesKeeper;
        public ObservableCollection<Baker> PendingConfectioneries { get => pendingConfectioneries; set { pendingConfectioneries = value; OnPropertyChanged(); } }
        private Baker selectedPendingConfectionery;
        public Baker SelectedPendingConfectionery { get => selectedPendingConfectionery; set { selectedPendingConfectionery = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }

        public ICommand DeclineConfectioneryCommand { get; private set; }
        public ICommand ApproveConfectioneryCommand { get; private set; }

        public PendingConfectioneriesPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            pendingConfectioneriesKeeper = new();
            PendingConfectioneries = new();
            GetBakers();
            FillPendingConfectioneries();
            DeclineConfectioneryCommand = new Command(OnDecline);
            ApproveConfectioneryCommand = new Command(OnApprove);

        }
        private async void GetBakers()
        {
            pendingConfectioneriesKeeper = await proxy.GetBakers();
        }
        private void FillPendingConfectioneries()
        {
            foreach (Baker b in pendingConfectioneriesKeeper)
            {
                if (b.StatusCode == 1)
                    PendingConfectioneries.Add(b);
            }
        }

        public async void OnDecline(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Question", "Would you like to decline the confectionery?", "Yes", "Cancel"))
            {
                PendingConfectioneries.Remove(((Baker)obj));
                proxy.DeclineCon(((Baker)obj));
            }
        }
        public async void OnApprove(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Question", "Would you like to approve the confectionery?", "Yes", "Cancel"))
            {
                PendingConfectioneries.Remove(((Baker)obj));
                proxy.ApproveCon(((Baker)obj));
            }
        }


    }
}