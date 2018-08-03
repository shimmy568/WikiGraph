
using System.Threading.Tasks;
using WikiGraph.Core.Models;

namespace WikiGraph.Interfaces.Services
{
    public interface IWebService
    {
        Task<ScrapedInfo> GetInfoFromUrl(string url);
    }
}