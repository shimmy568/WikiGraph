

using Ninject;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Interfaces.Services;
using System;
using System.Threading.Tasks;
using WikiGraph.Core.Models;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;

namespace WikiGraph.Core.Services
{
    public class WebService : IWebService
    {
        [Inject]
        public IWebRepository WebRepository { private get; set; }

        public async Task<ScrapedInfo> GetInfoFromUrl(string url)
        {
            var htmlDoc = await WebRepository.GetHtmlDocFromUrl(url);

            if(htmlDoc == null){
                return null;
            }
            
            var urls = GetLinksFromHtmlDocument(htmlDoc);
            var html = htmlDoc.DocumentNode.InnerHtml;

            return new ScrapedInfo
            {
                Urls = urls.ToList(),
                Html = html
            };
        }

        private IEnumerable<string> GetLinksFromHtmlDocument(HtmlDocument htmlDoc)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"mw-parser-output\"]//p//a");

            if (nodes == null)
            {
                return new List<string>();
            }

            var links = nodes.Select(x => x.GetAttributeValue("href", "###")).Where(x => x[0] != '#');

            return links;
        }

    }
}