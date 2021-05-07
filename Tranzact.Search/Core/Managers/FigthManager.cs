using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tranzact.Search.Models;

namespace Tranzact.Search.Managers
{
    public class FigthManager<TFigth,TSearch>  where TFigth : FigthResponse  where TSearch : SearchResponse
    {
        private readonly ISearchManager<TSearch> _search;
        public virtual ILogger _logger { get; set; }
        public FigthManager(ISearchManager<TSearch> search ,
                            ILogger<FigthManager<TFigth, TSearch>> logger)
        {
            _search = search;
            _logger = logger;
        }
        public virtual async Task<TFigth> FigthResultAsync(string[] words)
        {
            string result;
            bool success = true;
            try
            {
                var response = await _search.GetSearchResultsAsync(words);
                result = this.GetOutputResult(response);
            }
            catch (Exception ex)
            {
                success = false;
                _logger.LogError(default(EventId), ex ,ex.Message);
                result = "Ocurrio un error al procesar la(s) busquedas.";
            }

            var output = new FigthResponse {Mensaje = result , Success  = success };

            return (TFigth)output;
        }

        private string  GetOutputResult(List<TSearch> responses)
        {
            if (responses == null) throw new ArgumentNullException(nameof(responses));
            StringBuilder result = new StringBuilder();

            var query = responses.GroupBy(s => s.Word)
                .Select(s => new {
                    language = s.Key,
                    group = s.Select(x => new { x.ResultCount, x.SearchEngine }).ToList(),
                    TotalByLanguage = s.Sum(m => m.ResultCount)
                }).ToList();

            long maxLanguage = 0;
            string language = "";
            foreach (var item in query)
            {
                long max = 0;
                string engine = "";
                if (item.TotalByLanguage > maxLanguage)
                {
                    maxLanguage = item.TotalByLanguage;
                    language = item.language;
                }
                result.AppendLine(item.language);
                foreach (var b in item.group)
                {
                    if (b.ResultCount > max)
                    {
                        max = b.ResultCount;
                        engine = b.SearchEngine;
                    }
                    result.AppendLine($"   {b.SearchEngine}: {b.ResultCount}");
                }
                result.AppendLine($"ganador : {engine}");

            }
            result.AppendLine($"Total winner : {language}");

            return result.ToString();
        }


        public void Dispose()
        {
            _search?.Dispose();
        }
    }

}
