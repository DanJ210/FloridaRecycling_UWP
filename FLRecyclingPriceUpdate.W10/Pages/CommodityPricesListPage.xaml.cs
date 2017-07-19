//---------------------------------------------------------------------------
//
// <copyright file="CommodityPricesListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>7/8/2017 11:05:53 PM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.RestApi;
using FLRecyclingPriceUpdate.Sections;
using FLRecyclingPriceUpdate.ViewModels;
using AppStudio.Uwp;

namespace FLRecyclingPriceUpdate.Pages
{
    public sealed partial class CommodityPricesListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public CommodityPricesListPage()
        {
			ViewModel = ViewModelFactory.NewList(new CommodityPricesSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("0209f201-b48f-4124-a0b4-6e02ffd51e3b");
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
			if (e.NavigationMode == NavigationMode.New)
            {			
				await this.ViewModel.LoadDataAsync();
                this.ScrollToTop();
			}			
            base.OnNavigatedTo(e);
        }

    }
}
