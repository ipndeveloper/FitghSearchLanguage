using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tranzact.Search.ExampleCustom.ModelCustom;
using Tranzact.Search.Managers;
using Tranzact.Search.Models;

namespace Tranzact.Search.ExampleCustom
{
    public class CustomFigthManager : FigthManager<CustomFigthResponse>
    {
        public CustomFigthManager(ISearchManager<SearchResponse> search, ILogger<CustomFigthManager> logger) : base(search, logger)
        {
        }

        public override async  Task<CustomFigthResponse> FigthResultAsync(string[] words)
        {
            var response = await base.FigthResultAsync(words);
            // custom code
            return response;
        }
    }
}
