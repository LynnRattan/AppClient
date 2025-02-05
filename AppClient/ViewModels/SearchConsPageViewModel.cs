using AppClient.Models;
using AppClient.Services;
using AppClient.Views;

//using Javax.Security.Auth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppClient.ViewModels
{
    public class SearchConsPageViewModel:ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<Baker> foundConfectioneries;
        private List<Baker> foundConfectioneriesKeeper;
        public ObservableCollection<Baker> FoundConfectioneries { get => foundConfectioneries; set { foundConfectioneries = value; OnPropertyChanged(); } }
        private Baker selectedFoundConfectionery;
        public Baker SelectedFoundConfectionery { get => selectedFoundConfectionery; set { selectedFoundConfectionery = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }
        private string filterEntry;
        public string FilterEntry { get => filterEntry; set { filterEntry = value; OnPropertyChanged(); } }
        public List<ConfectioneryType> ConfectioneryTypes { get; private set; }
        private ConfectioneryType selectedConfectioneryType;
        public ConfectioneryType SelectedConfectioneryType { get => selectedConfectioneryType; set { selectedConfectioneryType = value; OnPropertyChanged(); } }

        public List<DessertType> DessertTypes { get; private set; }
        private DessertType selectedDessertType;
        public DessertType SelectedDessertType { get => selectedDessertType; set { selectedDessertType = value; OnPropertyChanged(); } }

        private string highestPrice;
        public string HighestPrice { get => highestPrice; set { highestPrice = value; OnPropertyChanged(); } }

        private string confectioneryName;
        public string ConfectioneryName { get => confectioneryName; set { confectioneryName = value; OnPropertyChanged(); } }
        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }


        public ICommand ViewConfectioneryCommand { get; private set; }
        public ICommand FilterCommand { get; private set; }
        public ICommand LoadConfectioneriesCommand { get; private set; }
        public SearchConsPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            ConfectioneryTypes = ((App)Application.Current).ConfectioneryTypes;
            DessertTypes = ((App)Application.Current).DessertTypes;
            foundConfectioneriesKeeper = new();
            FoundConfectioneries = new();
            isEmpty = true;
            FillFoundConfectioneriesKeeper();
            ViewConfectioneryCommand = new Command(OnView);
            FilterCommand = new Command(Filter);
            LoadConfectioneriesCommand = new Command(LoadFoundConfectioneries);
        }


        private async void FillFoundConfectioneriesKeeper()
        {
            foundConfectioneriesKeeper = await proxy.GetBakers();
        }
        private async void Filter()
        {
            foundConfectioneries.Clear();
            foreach (Baker b in foundConfectioneriesKeeper)
            {
                foundConfectioneries.Add(b);
            }
            if (ConfectioneryName != null)
            {
                foreach (Baker b in foundConfectioneries)
                {
                    if (b.ConfectioneryName != ConfectioneryName)
                        foundConfectioneries.Remove(b);
                }
            }
            if (SelectedConfectioneryType != null)
            {
                foreach (Baker b in foundConfectioneries)
                {
                    if (b.ConfectioneryTypeId != SelectedConfectioneryType.ConfectioneryTypeId)
                        foundConfectioneries.Remove(b);
                }
            }
            if (SelectedDessertType != null)
            {
                foreach (Baker b in foundConfectioneries)
                {
                    List <Dessert> l = await proxy.GetBakerDesserts(b.BakerId);
                    foreach (Dessert d in l)
                    {
                        if (d.DessertTypeId != SelectedDessertType.DessertTypeId)
                            foundConfectioneries.Remove(b);
                    }
                }
            }
            if (HighestPrice != null)
            {
                foreach (Baker b in foundConfectioneries)
                {  
                        if (b.HighestPrice > double.Parse(HighestPrice))
                            foundConfectioneries.Remove(b);  
                }
            }
        }

        private void OnView()
        {
            // Navigate to the ViewConFectionery View page
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<ViewConfectioneryPage>());

        }

        private async void LoadFoundConfectioneries()
        {
            IsRefreshing = true;
            FoundConfectioneries.Clear();
            FillFoundConfectioneriesKeeper();
            ConfectioneryName = null;
            HighestPrice = null;
            SelectedConfectioneryType = null;
            SelectedDessertType = null;
            Filter();
            if (foundConfectioneries.Count > 0)
                isEmpty = false;
            else isEmpty = true;
            OnPropertyChanged("IsEmpty");
            //OnPropertyChanged("LoggedInUser");
            IsRefreshing = false;
        }
    }
}

