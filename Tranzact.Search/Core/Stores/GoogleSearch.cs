using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tranzact.Search
{
    public class GoogleSearch : ISearch<SearchResponse>
    {
       
        private static IConfigurationRoot Configuration { get; set; }
        protected internal HttpClient HttpClient { get; set; }
        private const string SeachEngine = "Google";
        protected  internal string UrlBase { get; }
        public GoogleSearch(HttpClient httpClient, IConfigurationRoot configuration)
        {
            HttpClient = httpClient;
            Configuration = configuration;
            UrlBase = configuration["Search:Google:UrlBase"];
        }

        public async Task<SearchResponse>  GetSearchInfoAsync(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                throw new ArgumentNullException(nameof(searchQuery));
            }

            var response = await HttpClient.GetAsync($"{UrlBase}&q={searchQuery}");
            string Content = await response.Content.ReadAsStringAsync();
            dynamic info =  JsonConvert.DeserializeObject(Content);
            return new SearchResponse
            { 
                Word = searchQuery,
                SearchEngine = SeachEngine,
                ResultCount = info.searchInformation.totalResults
            };
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
        }
    }
}
