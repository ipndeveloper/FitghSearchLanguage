using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tranzact.Search
{
    public class Utility
    {

        public static string[] FixSearchQuery(string[] searchQuery)
        {
            if(searchQuery==null)  throw new ArgumentNullException(nameof(searchQuery));
            if (searchQuery.Length < 2)  throw new IndexOutOfRangeException("se necesitan dos competidores para una batalla.");
            List<String> searchQuerys = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                var word = Regex.Match(searchQuery[i], "\\\"(.*?)\\\"");
                if (word.Success)
                {
                    var newWord = word.Value.Replace("\"", "");
                    if (string.IsNullOrEmpty(newWord))
                        throw new ArgumentNullException("Ingrese un lenguaje de programación valido");
                    searchQuerys.Add(newWord);

                } 
                else if(!string.IsNullOrEmpty(searchQuery[i]) )
                {
                    searchQuerys.Add(searchQuery[i]);
                    
                }
                else
                {
                    throw new ArgumentNullException("Ingrese un lenguaje de programación valido");
                }

            }
            return searchQuerys.ToArray();
        }

        public static string[] FixQuote(string searchQuery)
        {
            return Regex.Matches(searchQuery, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
        }
    }
}
