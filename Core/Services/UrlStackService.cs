

using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Interfaces.Services;

namespace WikiGraph.Core.Services
{
    public class UrlStackService : IUrlStackService
    {
        // The max amount of urls to store in the in memory stack before sending it off to the database
        readonly int IN_MEMORY_MAX = 200000;

        // The amount it resets to after hitting the max value
        readonly int IN_MEMORY_RESET = 50000;

        Stack<string> urlStack;
        Dictionary<string, bool> inStackLookup;

        [Inject]
        public INodeService NodeService { private get; set; }

        [Inject]
        public IUrlStackRepository UrlStackRepository { private get; set; }

        public UrlStackService()
        {
            urlStack = new Stack<string>();
            inStackLookup = new Dictionary<string, bool>();
        }

        public async Task<string> GetUrlFromStack()
        {

            // If stack is empty grab 5000 rows from database
            if (urlStack.Count == 0)
                (await UrlStackRepository.GetUrlsFromStack()).ForEach(x =>
                {
                    urlStack.Push(x);
                    inStackLookup.Add(x, true);
                });

            return urlStack.Pop();
        }

        public async Task<int> Count()
        {
            return urlStack.Count + await UrlStackRepository.CountRowsInStack();
        }

        private async Task<bool> ShouldAddUrlToStack(string url)
        {
            // Checks if the url has been added to the stack/nodes
            if (inStackLookup.ContainsKey(url))
            {
                return false;
            }
            else if (await UrlStackRepository.IsUrlInDatabaseStack(url))
            {
                return false;
            }
            else if (await NodeService.GetNodeByUrl(url) != null)
            {
                return false;
            }

            return true;
        }

        // Gives an enum that keeps popping urls from stack untill length < IN_MEMORY_RESET
        // Used in database insert
        private IEnumerable<string> GetEnumerableForStack()
        {
            while (urlStack.Count > IN_MEMORY_RESET)
            {
                var val = urlStack.Pop();
                inStackLookup.Remove(val);
                yield return val;
            }
        }

        public async Task PutUrlInStack(string url)
        {

            // If we have seen the url already dont add it to the stack
            if (!(await ShouldAddUrlToStack(url)))
            {
                return;
            }

            urlStack.Push(url);
            inStackLookup.Add(url, true);

            // If stack is too large put shit in database and move on
            if (urlStack.Count > IN_MEMORY_MAX)
            {
                var tasks = new List<Task>();

                foreach (var curUrl in GetEnumerableForStack())
                {
                    tasks.Add(UrlStackRepository.AddUrlToStack(curUrl));
                }
            }
        }

        private IEnumerable<string> AllUrlsInStack()
        {
            while (urlStack.Count > 0)
            {
                yield return urlStack.Pop();
            }
        }

        public async Task CommitAllUrlsToDatabase()
        {
            var tasks = new List<Task>();

            // foreach (var url in AllUrlsInStack())
            // {
            //     tasks.Add(UrlStackRepository.AddUrlToStack(url));
            // }
            // await Task.WhenAll(tasks);

            await UrlStackRepository.AddUrlsToStack(AllUrlsInStack());

            urlStack = new Stack<string>();
            inStackLookup = new Dictionary<string, bool>();
        }

    }
}