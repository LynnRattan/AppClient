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
        
        private List<Baker> foundConfectioneriesKeeper;
        private ObservableCollection<Baker> foundConfectioneries;
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
            Filter();
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

            FoundConfectioneries.Clear();
            foreach (Baker b in foundConfectioneriesKeeper)
            {
                if(b.StatusCode==2)
                FoundConfectioneries.Add(b);
            }
            if (!string.IsNullOrEmpty(ConfectioneryName))
            {
                foreach (Baker b in FoundConfectioneries.ToList())
                {
                    if (!(b.ConfectioneryName.Contains(ConfectioneryName)))
                        FoundConfectioneries.Remove(b);
                }
            }
            if (SelectedConfectioneryType != null)
            {
                foreach (Baker b in FoundConfectioneries.ToList())
                {
                    if (b.ConfectioneryTypeId != SelectedConfectioneryType.ConfectioneryTypeId)
                        FoundConfectioneries.Remove(b);
                }
            }
            if (SelectedDessertType != null)
            {
                foreach (Baker b in FoundConfectioneries.ToList())
                {
                    bool exists = false;
                    List <Dessert> l = await proxy.GetBakerDesserts(b.BakerId);
                    foreach (Dessert d in l)
                    {
                        if (d.DessertTypeId == SelectedDessertType.DessertTypeId)
                            exists = true;
                    }
                    if (!exists)
                    {
                        FoundConfectioneries.Remove(b);
                    }
                }
            }
            if (HighestPrice != null)
            {
                foreach (Baker b in FoundConfectioneries.ToList())
                {  
                        if (b.HighestPrice > double.Parse(HighestPrice))
                            FoundConfectioneries.Remove(b);  
                }
            }
            if (FoundConfectioneries.Count > 0)
                isEmpty = false;
            else isEmpty = true;
            OnPropertyChanged("IsEmpty");

        }

        private async void OnView()
        {
            // Navigate to the ViewConFectionery View page
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("FoundConfectionery", SelectedFoundConfectionery);
            await Shell.Current.GoToAsync("ViewCon", data);
            SelectedFoundConfectionery = null;
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

