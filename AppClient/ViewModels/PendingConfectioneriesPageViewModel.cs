﻿using AppClient.Models;
using AppClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using static Android.App.ActivityManager;

namespace AppClient.ViewModels
{
    public class PendingConfectioneriesPageViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private List<Baker> pendingConfectioneriesKeeper;
        private ObservableCollection<Baker> pendingConfectioneries;
       
        public ObservableCollection<Baker> PendingConfectioneries { get => pendingConfectioneries; set { pendingConfectioneries = value; OnPropertyChanged(); } }
        private Baker selectedPendingConfectionery;
        public Baker SelectedPendingConfectionery { get => selectedPendingConfectionery; set { selectedPendingConfectionery = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }

        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        public ICommand DeclineConfectioneryCommand { get; private set; }
        public ICommand ApproveConfectioneryCommand { get; private set; }
        public ICommand LoadPendingConfectioneriesCommand { get; private set; }

        public PendingConfectioneriesPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            pendingConfectioneriesKeeper = new();
            PendingConfectioneries = new();
            isEmpty = true;
            //GetBakers();
            InItData();
            DeclineConfectioneryCommand = new Command(OnDecline);
            ApproveConfectioneryCommand = new Command(OnApprove);
            LoadPendingConfectioneriesCommand = new Command(async () => await LoadPendingConfectioneries());
        }


        private async void InItData()
        {
            await LoadPendingConfectioneries();
        }

        private async void GetBakers()
        {
            pendingConfectioneriesKeeper = await proxy.GetBakers();
        }
        private async Task FillPendingConfectioneries()
        {
            pendingConfectioneriesKeeper.Clear();
            pendingConfectioneriesKeeper = await proxy.GetBakers();
            PendingConfectioneries.Clear();
            foreach (Baker b in pendingConfectioneriesKeeper)
            {
                if (b.StatusCode == 1)
                {
                    PendingConfectioneries.Add(b);
                }
            }

            if(PendingConfectioneries!=null &&PendingConfectioneries.Count>0)
                IsEmpty = false;
            else IsEmpty = true;



        }

        public async void OnDecline(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Confectionery", "Would you like to decline the confectionery?", "Yes", "Cancel"))
            {
                Baker baker = (Baker)obj;
                PendingConfectioneries.Remove(((Baker)obj));
                proxy.DeclineCon(baker.BakerId);
                if (PendingConfectioneries != null && PendingConfectioneries.Count > 0)
                    IsEmpty = false;
                else IsEmpty = true;
            }
        }
        public async void OnApprove(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Confectionery", "Would you like to approve the confectionery?", "Yes", "Cancel"))
            {
                Baker baker = (Baker)obj;
                PendingConfectioneries.Remove(((Baker)obj));
                proxy.ApproveCon(baker.BakerId);
                if (PendingConfectioneries != null && PendingConfectioneries.Count > 0)
                    IsEmpty = false;
                else IsEmpty = true;
            }
        }

        public async Task LoadPendingConfectioneries()
        {
            IsRefreshing = true;
            
            FillPendingConfectioneries();
             IsRefreshing = false;
        }



    }
}
