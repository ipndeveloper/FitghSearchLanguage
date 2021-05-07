namespace Tranzact.Search
{
    public class SearchResponse
    {
        public  string Word { get; set; }
        public  string SearchEngine { get; set; }

        public long ResultCount { get; set; }
    }
}