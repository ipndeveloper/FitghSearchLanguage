using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tranzact.Search
{
    public class BingSearch : ISearch<SearchResponse>
    {
        private static IConfigurationRoot Configuration { get; set; }
        private readonly HttpClient _httpClient;
        private const string SeachEngine = "Bing";
        protected internal string _urlBase { get; }
        public BingSearch(HttpClient httpClient, IConfigurationRoot configuration)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configuration["Search:Bing:Ocp-Apim-Subscription-Key"]);
            _urlBase = configuration["Search:Bing:UrlBase"];
        }

        public async Task<SearchResponse> GetSearchInfoAsync(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                throw new ArgumentNullException(nameof(searchQuery));
            }
            
            var response =    await _httpClient.GetAsync($"{_urlBase}?q={searchQuery}");
            string  Content = await response.Content.ReadAsStringAsync();
            dynamic info =    await JsonConvert.DeserializeObjectAsync(Content);
            return new SearchResponse
            {
                Word = searchQuery,
                SearchEngine = SeachEngine,
                ResultCount =  info.webPages.totalEstimatedMatches
            };
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
