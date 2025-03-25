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
    [QueryProperty(nameof(SelectedOrder), "SelectedOrder")]
    public class ViewNewOrderViewModel : ViewModelBase
    {
        private LMBWebApi proxy;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<OrderedDessert> bakerOrderedDesserts;
        private List<OrderedDessert> orderedDessertsKeeper;
        public ObservableCollection<OrderedDessert> BakerOrderedDesserts { get => bakerOrderedDesserts; set { bakerOrderedDesserts = value; OnPropertyChanged(); } }
        private OrderedDessert selectedOrderedDessert;
        public OrderedDessert SelectedOrderedDessert { get => selectedOrderedDessert; set { selectedOrderedDessert = value; OnPropertyChanged(); } }
        private bool isRefreshing;
        public bool IsRefreshing { get => isRefreshing; set { isRefreshing = value; OnPropertyChanged(); } }


        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; set { isEmpty = value; OnPropertyChanged(); } }

        

        private Order selectedOrder;
        public Order SelectedOrder
        {
            get => selectedOrder; set
            {
                selectedOrder = value; OnPropertyChanged();
            }
        }
        public ICommand LoadBakerDessertsCommand { get; private set; }
        public ICommand DeclineDessertCommand { get; private set; }

        public ICommand DeclineOrderCommand { get; private set; }
        public ICommand ApproveOrderCommand { get; private set; }
        public Baker? LoggedInBaker { get; set; }


        public ViewNewOrderViewModel(LMBWebApi proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            LoggedInBaker = ((App)Application.Current).LoggedInBaker;
            orderedDessertsKeeper = new();
            BakerOrderedDesserts = new();
            isEmpty = true;
            FillBakerDesserts();
            LoadBakerDessertsCommand = new Command(LoadBakerDesserts);
            DeclineDessertCommand = new Command(OnDeclineDessert);
            DeclineOrderCommand = new Command(OnDeclineOrder);
            ApproveOrderCommand = new Command(OnApproveOrder);



        }

        private async void OnDeclineDessert(Object obj)
        {
            if (await AppShell.Current.DisplayAlert("Dessert", "Would you like to decline the dessert?", "Yes", "Cancel"))
            {
                double newPrice = SelectedOrder.TotalPrice;
                OrderedDessert d = (OrderedDessert)obj;
                BakerOrderedDesserts.Remove(((OrderedDessert)obj));
                proxy.DeclineOrderedDes(d.OrderedDessertId);
                newPrice -= d.Price;
                await proxy.UpdateTotalPrice(SelectedOrder, newPrice);
                
            }
            if (BakerOrderedDesserts != null && BakerOrderedDesserts.Count > 0)
            {
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
                await Application.Current.MainPage.DisplayAlert("Order declined", "Every dessert in the order has been declined", "ok");
                Order o = SelectedOrder;
                proxy.DeclineOrder(o.Id);
                ((App)Application.Current).MainPage.Navigation.PopAsync();
            }
        }

        private async void OnDeclineOrder()
        {
            if (BakerOrderedDesserts.Count == 0)
            {
                string errorMsg = "You cannot decline an empty order.";
                await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
            }
            else
            {
                if (await AppShell.Current.DisplayAlert("Order", "Would you like to decline the order?", "Yes", "Cancel"))
                {
                    Order o = SelectedOrder;
                    proxy.DeclineOrder(o.Id);
                    foreach (OrderedDessert d in BakerOrderedDesserts)
                    {
                        proxy.DeclineOrderedDes(d.OrderedDessertId);
                    }
                }
            ((App)Application.Current).MainPage.Navigation.PopAsync();
            }
        }

        private async void OnApproveOrder()
        {
            if (BakerOrderedDesserts.Count==0)
            {
                string errorMsg = "You cannot approve an empty order.";
                await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "ok");
            }
            else
            {
                if (await AppShell.Current.DisplayAlert("Order", "Would you like to approve the order?", "Yes", "Cancel"))
                {
                    Order o = SelectedOrder;
                    DateOnly arrivaldate = DateOnly.FromDateTime(DateTime.Now);
                    proxy.ApproveOrder(o.Id,arrivaldate);
                    foreach (OrderedDessert d in BakerOrderedDesserts.ToList())
                    {
                            BakerOrderedDesserts.Remove(d);
                        if(d.StatusCode!=3)
                            proxy.ApproveOrderedDes(d.OrderedDessertId);
                    }
                    LoggedInBaker.Profits += SelectedOrder.TotalPrice;
                    proxy.UpdateProfits(LoggedInBaker);

                }
                 ((App)Application.Current).MainPage.Navigation.PopAsync();
            }
           
        }


        private async void FillBakerDesserts()
        {
            BakerOrderedDesserts.Clear();
            orderedDessertsKeeper.Clear();

            orderedDessertsKeeper = await proxy.GetOrderedDesserts();

            foreach (OrderedDessert d in orderedDessertsKeeper)
            {
                if (d.OrderId == SelectedOrder.Id && d.StatusCode == 1)
                {
                    BakerOrderedDesserts.Add(d);
                }

            }
            if (BakerOrderedDesserts != null&&BakerOrderedDesserts.Count>0)
            {
                IsEmpty = false;
            }
            else IsEmpty = true;
        }


        private async void LoadBakerDesserts()
        {
            IsRefreshing = true;
            FillBakerDesserts();
            IsRefreshing = false;

        }

    }
}