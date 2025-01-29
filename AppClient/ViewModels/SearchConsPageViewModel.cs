using AppClient.Models;
using AppClient.Services;
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
        //private LMBWebApi proxy;
        //private readonly IServiceProvider serviceProvider;
        //private ObservableCollection<Baker> foundConfectioneries;
        //private List<Baker> foundConfectioneriesKeeper;
        //public ObservableCollection<Baker> FoundConfectioneries { get => foundConfectioneries; set { foundConfectioneries = value; OnPropertyChanged(); } }
        //private Baker selectedFoundConfectionery;
        //public Baker SelectedPendingConfectionery { get => selectedFoundConfectionery; set { selectedFoundConfectionery = value; OnPropertyChanged(); } }
        //private bool isRefreshing;
        //public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }
        //private string filterEntry;
        //public string FilterEntry { get => filterEntry; set { filterEntry = value; OnPropertyChanged(); } }
        //public List<ConfectioneryType> confectioneryType { get; private set; }
        //private ConfectioneryType selectedConfectioneryType;
        //public ConfectioneryType SelectedConfectioneryType { get => selectedConfectioneryType; set { selectedConfectioneryType = value; OnPropertyChanged(); } }

        //private ConfectioneryType selectedDessertType;
        //public ConfectioneryType SelectedDessertType { get => selectedConfectioneryType; set { selectedConfectioneryType = value; OnPropertyChanged(); } }
        //private string confectioneryName;
        //public string ConfectioneryName { get => confectioneryName; set { confectioneryName = value; OnPropertyChanged(); } }
        //private bool isEmpty;
        //public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        //public ICommand ViewConfectioneryCommand { get; private set; }
        //public ICommand FilterCommand { get; private set; }
        //public ICommand LoadConfectioneriesCommand { get; private set; }
        //public SearchConsPageViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        //{
        //    this.serviceProvider = serviceProvider;
        //    this.proxy = proxy;
           
        //    foundConfectioneriesKeeper = new();
        //    FoundConfectioneries = new();
        //    isEmpty = true;
        //    FillFoundConfectioneriesKepper();
        //    GoToViewConfectioneryCommand = new Command(OnView);
        //    FilterCommand = new Command(Fliter);
        //    LoadConfectioneriesCommand = new Command(LoadFoundConfectioneries);
        //}

       
        //private async void FillFoundConfectioneriesKepper()
        //{
        //    foundConfectioneriesKeeper = await proxy.GetBakers();
        //}
        //private void FilterSubject()
        //{
        //    FoundConfectioneries.Clear();
        //    foreach (Baker b in foundConfectioneriesKeeper)
        //    {
        //        FoundConfectioneries.Add(b);
        //    }
        //    if(ConfectioneryName != null)
        //    {
        //        foreach(Baker b in foundConfectioneries)
        //        {
        //            if(b.ConfectioneryName!= ConfectioneryName)
        //                FoundConfectioneries.Remove(b);
        //        }
        //    }
        //    if(SelectedConfectioneryType!= null)
        //    {
        //        foreach (Baker b in foundConfectioneries)
        //        {
        //            if (b.ConfectioneryTypeId != SelectedConfectioneryType.ConfectioneryTypeId)
        //                FoundConfectioneries.Remove(b);
        //        }
        //    }
        //    if(SelectedDessertType!=null)
        //    {
        //        foreach (Baker b in foundConfectioneries)
        //        {
        //            if (b.ConfectioneryTypeId != SelectedConfectioneryType.ConfectioneryTypeId)
        //                FoundConfectioneries.Remove(b);
        //        }
        //    }
        //}

        ////private async Task LoadPendingQuestions()
        ////{
        ////    IsRefreshing = true;
        ////    FilterEntry = string.Empty;
        ////    List<Question> filteredQ = service.GetPendingQuestionsBySubjectName(FilterEntry);
        ////    PendingQuestions.Clear();
        ////    foreach (Question q in filteredQ)
        ////    {
        ////        PendingQuestions.Add(q);
        ////    }
        ////    IsRefreshing = false;
        ////}
    }
}

