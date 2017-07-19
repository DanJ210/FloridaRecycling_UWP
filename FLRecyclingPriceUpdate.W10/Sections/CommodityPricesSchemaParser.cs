using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLRecyclingPriceUpdate.Sections
{
    /// <summary>
    /// Implementation of the CommodityPricesSchemaParser class.
    /// </summary>
    public class CommodityPricesSchemaParser : IParser<CommodityPricesSchema>
    {	
        public IEnumerable<CommodityPricesSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var result = new Collection<CommodityPricesSchema>();
            JToken jtokenData = JsonConvert.DeserializeObject<JToken>(data);
            IEnumerable<JToken> elements = jtokenData.SelectToken("")?.Select(s => s);
            if (elements != null)
            {
                foreach (JToken item in elements)
                {
                    var itemResult = new CommodityPricesSchema();
					itemResult._id = item.SelectToken("commodityId")?.ToString();
					itemResult.commodityName = item.SelectToken("commodityName")?.ToString().DecodeHtml();
					itemResult.commodityPrice = item.SelectToken("commodityPrice")?.ToString().DecodeHtml();
					itemResult.commodityImageUrl = item.SelectToken("commodityImageUrl")?.ToString();
					itemResult.commodityDescription = item.SelectToken("commodityDescription")?.ToString().DecodeHtml();
					itemResult.header = item.SelectToken("header")?.ToString().DecodeHtml();
                    result.Add(itemResult);
                }
            }
            return result;
        }
    }
}
