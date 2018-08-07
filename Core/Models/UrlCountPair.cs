

namespace WikiGraph.Core.Models
{
    public class UrlCountPair
    {
        public string Url { get; set; }
        public int Count { get; set; }

        public UrlCountPair(string url, int count)
        {
            Url = url;
            Count = count;
        }
    }
}