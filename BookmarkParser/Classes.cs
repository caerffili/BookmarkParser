using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkParser
{
    public class Anno
    {
        public string name { get; set; }
        public int flags { get; set; }
        public int expires { get; set; }
        public object mimeType { get; set; }
        public int type { get; set; }
        public object value { get; set; }
    }

    public class Child
    {
        public string title { get; set; }
        public int id { get; set; }
        public int parent { get; set; }
        public object dateAdded { get; set; }
        public object lastModified { get; set; }
        public string type { get; set; }
        public string root { get; set; }
        public List<Child> children { get; set; }
        public int? index { get; set; }
        public List<Anno> annos { get; set; }
        public string uri { get; set; }
        public string charset { get; set; }
        public int? livemark { get; set; }
    }

    public class Root
    {
        public string title { get; set; }
        public int id { get; set; }
        public long dateAdded { get; set; }
        public long lastModified { get; set; }
        public string type { get; set; }
        public string root { get; set; }
        public List<Child> children { get; set; }
    }



    public class TitlesDef
    {
        //   public Dictionary<string, Example> Titles { set; get; }
    }
}
