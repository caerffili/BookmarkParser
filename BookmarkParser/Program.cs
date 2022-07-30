using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine eng = new Engine(@"Z:\Documents\Bookmarks");
            Console.ReadKey();
        }
    }
}
