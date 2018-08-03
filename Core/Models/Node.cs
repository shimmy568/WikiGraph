
using System;

namespace WikiGraph.Core.Models
{
    public class Node
    {
        
        public int NodeID { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }
        public DateTime TimeRetrieved { get; set; }
    }
}