
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WikiGraph.Interfaces.Repositories
{
    public interface IWebRepository
    {
        Task<HtmlDocument> GetHtmlDocFromUrl(string url);
    }
}