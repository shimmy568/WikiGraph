
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WikiGraph.Core.Models;

namespace WikiGraph.Interfaces.Services
{
    public interface IWebService
    {
        Task<ScrapedInfo> GetInfoFromUrl(string url);
        IEnumerable<string> GetLinksFromHtmlDocument(HtmlDocument htmlDoc);
    }
}