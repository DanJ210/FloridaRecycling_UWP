using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.RestApi;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp;
using System.Linq;

using FLRecyclingPriceUpdate.Navigation;
using FLRecyclingPriceUpdate.ViewModels;

namespace FLRecyclingPriceUpdate.Sections
{	
	public class CommodityPricesSection : Section<CommodityPricesSchema>
    {
		private RestApiDataProvider _dataProvider;

		public CommodityPricesSection()
		{	
			_dataProvider = new RestApiDataProvider();
		}

		public override async Task<IEnumerable<CommodityPricesSchema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new RestApiDataConfig
            { 
				Url = new Uri("http://floridarecyclingmetalapi.azurewebsites.net/api/metal"),
				Headers = GetHeaders(),
				PaginationConfig = new MemoryPagination()
				{
					OrderBy = "commodityName",
					OrderDirection = SortDirection.Ascending
				}
			};

			return await _dataProvider.LoadDataAsync(config, MaxRecords, new CommodityPricesSchemaParser());
        }

        public override async Task<IEnumerable<CommodityPricesSchema>> GetNextPageAsync()
        {
            return await _dataProvider.LoadMoreDataAsync<CommodityPricesSchema>();
        }

        public override bool HasMorePages
        {
            get
            {
                return _dataProvider.HasMoreItems;
            }
        }

        public override ListPageConfig<CommodityPricesSchema> ListPage
        {
            get 
            {
                var userCurrency = Windows.System.UserProfile.GlobalizationPreferences.Currencies[0];
                var userCurrencyFormat = new Windows.Globalization.NumberFormatting.CurrencyFormatter(userCurrency);
                return new ListPageConfig<CommodityPricesSchema>
                {
                    Title = "Commodity Prices",

                    Page = typeof(Pages.CommodityPricesListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
                        var price = userCurrencyFormat.Format(double.Parse(item.commodityPrice));
                        viewModel.Header = item.header.ToSafeString();
                        viewModel.Title = item.commodityName.ToSafeString();
                        //viewModel.Title = price + " Per Pound";
                        viewModel.SubTitle = price + " Per Pound";
                        viewModel.Description = "Testing description";
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.commodityImageUrl.ToSafeString());
						viewModel.Footer = item.commodityDescription.ToSafeString();

						viewModel.GroupBy = item.header.SafeType();

						viewModel.OrderBy = item.commodityName;
                    },
					OrderType = OrderType.Ascending,
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.CommodityPricesDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<CommodityPricesSchema> DetailPage
        {
            get
            {
                var userCurrency = Windows.System.UserProfile.GlobalizationPreferences.Currencies[0];
                var userCurrencyFormat = new Windows.Globalization.NumberFormatting.CurrencyFormatter(userCurrency);
                var bindings = new List<Action<ItemViewModel, CommodityPricesSchema>>();
                bindings.Add((viewModel, item) =>
                {
                    var price = userCurrencyFormat.Format(double.Parse(item.commodityPrice));
                    viewModel.PageTitle = item.commodityName.ToSafeString();
                    //viewModel.Title = String.Format("{0:#$.0}", item.commodityPrice.ToSafeString());
                    viewModel.Title = price + " Per Pound";
                    viewModel.Description = item.commodityDescription.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.commodityImageUrl.ToSafeString());
                    viewModel.Content = null;					
                });

                var actions = new List<ActionConfig<CommodityPricesSchema>>
                {
                };

                return new DetailPageConfig<CommodityPricesSchema>
                {
                    Title = "Commodity Prices",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
		
		private IDictionary<string, string> GetHeaders()
        {
			IDictionary<string, string> result = new Dictionary<string, string>();
			result["User-Agent"] ="Mozilla/5.0 (Windows NT 10.0)";
            return result;
        }
		
    }
}
