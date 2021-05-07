using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tranzact.Search.Managers
{
    public interface ISearchManager<TResponse> : IDisposable where TResponse : class
    {
        Task<List<TResponse>> GetSearchResultsAsync(string[] searchQuery);
    }
}
