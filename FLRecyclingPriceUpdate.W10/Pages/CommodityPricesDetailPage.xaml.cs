//---------------------------------------------------------------------------
//
// <copyright file="CommodityPricesDetailPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>7/8/2017 11:05:53 PM</createdOn>
//
//---------------------------------------------------------------------------

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppStudio.DataProviders.RestApi;
using FLRecyclingPriceUpdate.Sections;
using FLRecyclingPriceUpdate.Navigation;
using FLRecyclingPriceUpdate.ViewModels;
using AppStudio.Uwp;

namespace FLRecyclingPriceUpdate.Pages
{
    public sealed partial class CommodityPricesDetailPage : Page
    {
        private DataTransferManager _dataTransferManager;

        public CommodityPricesDetailPage()
        {
            ViewModel = ViewModelFactory.NewDetail(new CommodityPricesSection());
            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        public DetailViewModel ViewModel { get; set; }        

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadStateAsync(e.Parameter as NavDetailParameter);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }
    }
}
