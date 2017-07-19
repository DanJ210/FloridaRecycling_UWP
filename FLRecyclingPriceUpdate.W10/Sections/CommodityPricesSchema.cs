using System;
using AppStudio.DataProviders;

namespace FLRecyclingPriceUpdate.Sections
{
    /// <summary>
    /// Implementation of the CommodityPricesSchema class.
    /// </summary>
    public class CommodityPricesSchema : SchemaBase
    {

        public string commodityName { get; set; }

        public string commodityPrice { get; set; }

        public string commodityImageUrl { get; set; }

        public string commodityDescription { get; set; }

        public string header { get; set; }
    }
}
