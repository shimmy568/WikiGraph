

namespace WikiGraph.Core.Models
{
    public class UrlCountPair
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int Count { get; set; }

        public UrlCountPair(string title, string url, int count)
        {
            Title = title;
            Url = url;
            Count = count;
        }
    }
}