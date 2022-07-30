using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BookmarkParser
{
    public class Engine
    {
        public Engine(String path)
        {
            JSONParser JP = new JSONParser();

            var files = Directory.EnumerateFiles(path, "*.json");

            foreach (String file in files)
            {
                Console.WriteLine("Examining {0}", file);
                Console.WriteLine("Links - {0}", JP.Parse(file));
                Console.WriteLine("Links - {0}", JP.CountE());
                //  Console.ReadKey();
            }

            JP.DumpFilteredLinks();

            JP.DumpLinks();
            JP.Write();
            Console.ReadKey();
        }
    }
}
