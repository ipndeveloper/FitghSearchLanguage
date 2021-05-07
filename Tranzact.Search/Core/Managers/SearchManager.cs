using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tranzact.Search.Managers;

namespace Tranzact.Search
{

    public class SearchManager<TSearh> : ISearchManager<TSearh> where TSearh: SearchResponse
    {
        protected internal IConfigurationRoot Configuration { get; set; }
        protected internal IEnumerable<ISearch<TSearh>> Search { get; set; }
        protected internal IList<Task<TSearh>> SearchResponse { get; } = new List<Task<TSearh>>();

        public SearchManager(IEnumerable<ISearch<TSearh>> searchs,
            IConfigurationRoot configuration)
        {
            Search = searchs;
            Configuration = configuration;
        }

        public virtual async Task<List<TSearh>> GetSearchResultsAsync(string[] searchQuery)
        {
            string[] words = Utility.FixSearchQuery(searchQuery);
            IList<Task<TSearh>> allSearchTask = this.GetAllResponseSearch(words);
            var response = await Task.WhenAll(allSearchTask);
            return response.ToList();
        }

        private IList<Task<TSearh>> GetAllResponseSearch(string[] searchQuery)
        {
            foreach (var word in searchQuery)
            {
                foreach (var search in Search)
                {
                    SearchResponse.Add(search.GetSearchInfoAsync(word));
                }
            }

            return SearchResponse;
        }

        public void Dispose()
        {

        }

    }
    //public class SearchManager : ISearchManager<SearchResponse>
    //{

    //    protected internal IConfigurationRoot  Configuration { get; set; }
    //    protected internal IEnumerable<ISearch<SearchResponse>> Search { get; set; }
    //    protected  internal IList<Task<SearchResponse>> SearchResponse { get; } = new List<Task<SearchResponse>>();
    //    public SearchManager(IEnumerable<ISearch<SearchResponse>> searchs ,
    //        IConfigurationRoot configuration)
    //    {
    //        Search = searchs;
    //        Configuration = configuration;
    //    }
    //    public virtual async Task<List<SearchResponse>> GetSearchResultsAsync(string[] searchQuery)
    //    {
    //        string[] words = Utility.FixSearchQuery(searchQuery);
    //        IList<Task<SearchResponse>> allSearchTask = this.GetAllResponseSearch(words);
    //        var response =await Task.WhenAll(allSearchTask);
    //        return response.ToList();
    //    }
    //    private IList<Task<SearchResponse>> GetAllResponseSearch(string[] searchQuery)
    //    {
    //        foreach (var word in searchQuery)
    //        {
    //            foreach (var search  in Search)
    //            {
    //                SearchResponse.Add(search.GetSearchInfoAsync(word));
    //            }
    //        }
    //        return SearchResponse;
    //    }

    //    public void Dispose()
    //    {

    //    }
    //}


}
