using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tranzact.Search
{
    public interface ISearch<TResponse> : IDisposable where TResponse : class
    {
        Task<TResponse> GetSearchInfoAsync(string word);
    }
}
