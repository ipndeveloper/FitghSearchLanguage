using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tranzact.Search.ExampleCustom.ModelCustom;

namespace Tranzact.Search.ExampleCustom
{
    public class CustomSearchManager : SearchManager<CustomSearchResponse>
    {
        public CustomSearchManager(IEnumerable<ISearch<CustomSearchResponse>> searchs, IConfigurationRoot configuration) : base(searchs, configuration)
        {
        }

        public override async Task<List<CustomSearchResponse>> GetSearchResultsAsync(string[] searchQuery)
        {
            var response =await  base.GetSearchResultsAsync(searchQuery);
            return response;
        }
    }
}
