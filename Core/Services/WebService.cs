

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

        private bool IsValidUri(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri);
        }

        public async Task<ScrapedInfo> GetInfoFromUrl(string url)
        {
            var htmlDoc = await WebRepository.GetHtmlDocFromUrl(url);

            if (htmlDoc == null)
            {
                return null;
            }

            // Get urls and filter out wikipeida: pages
            var urls = GetLinksFromHtmlDocument(htmlDoc).Where(x => !x.Contains("Wikipedia:")).Select(x =>
            {
                if (x.StartsWith("/wiki/") || x.StartsWith("/w/"))
                {
                    return "https://en.wikipedia.org" + x;
                }
                else if (x.StartsWith("//"))
                {
                    return "https:" + x;
                }
                return x;
            }).Where(x => IsValidUri(x));
            var html = htmlDoc.DocumentNode.InnerHtml;
            var title = GetTitleFromDoc(htmlDoc);

            return new ScrapedInfo
            {
                Urls = urls.ToList(),
                Html = html,
                Title = title
            };
        }

        private string GetTitleFromDoc(HtmlDocument htmlDoc)
        {
            var node = htmlDoc.DocumentNode.SelectSingleNode("//h1[@id=\"firstHeading\"]");
            if (node == null)
            {
                return "";
            }
            return node.InnerText;
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