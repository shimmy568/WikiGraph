
using System.Threading.Tasks;
using HtmlAgilityPack;
using WikiGraph.Interfaces.Repositories;

namespace WikiGraph.Core.Repositories
{
    public class WebRepository : IWebRepository
    {

        HtmlWeb web;

        public WebRepository()
        {
            web = new HtmlWeb();
        }

        public async Task<HtmlDocument> GetHtmlDocFromUrl(string url)
        {
            HtmlDocument htmlDoc;
            try
            {
                htmlDoc = await web.LoadFromWebAsync(url);
            }
            catch
            {
                return null;
            }
            return htmlDoc;
        }
    }
}