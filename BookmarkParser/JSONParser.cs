using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkParser
{
    public class JSONParser
    {
        private Root processedRoot;

        private List<String> uniqueURLList;
        private List<String> filteredLinksList;

        public int entriesParsed { get; set; }
        public int filteredLinkQty { get; set; }
        public int validLinkQty { get; set; }
        public int duplicateLinkQty { get; set; }
        public int addedLinkQty { get; set; }
        public int folderQty { get; set; }
        public int totalAddedLinkQty { get; set; }



        public JSONParser()
        {
            processedRoot = new Root();
            uniqueURLList = new List<string>();
            filteredLinksList = new List<string>();

            processedRoot.children = new List<Child>();

            totalAddedLinkQty = 0;
        }

        public int Parse(String file)
        {
            entriesParsed = 0;
            filteredLinkQty = 0;
            validLinkQty = 0;
            duplicateLinkQty = 0;
            addedLinkQty = 0;
            folderQty = 0;

            var res = JsonConvert.DeserializeObject<Root>(File.ReadAllText(file));

            ParseChildren(res.children, processedRoot.children);

            Console.WriteLine("Parsed Qty {0}", entriesParsed);
            Console.WriteLine("Valid Links {0}", validLinkQty);
            Console.WriteLine("Folders {0}", folderQty);

            Console.WriteLine("Filtered Links {0}", filteredLinkQty);
            Console.WriteLine("Duplicate Links {0}", duplicateLinkQty);
            Console.WriteLine("Added Links {0}", addedLinkQty);
            Console.WriteLine("Total Added Links {0}", totalAddedLinkQty);

            Console.WriteLine("");
            Console.WriteLine("ORIGINAL");

            PurgeOrphans(processedRoot.children);

            Console.WriteLine("");
            Console.WriteLine("PURGED");

            return addedLinkQty;
        }

        public void Write()
        {
            JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new JavaScriptDateTimeConverter());

            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(@"Z:\Documents\Bookmarks\json.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, processedRoot);
                // {"ExpiryDate":new Date(1230375600000),"Price":0}
            }
          
        }

        public int DumpLinks()
        {
            return DumpText(0, processedRoot.children);
        }

        public void DumpFilteredLinks()
        {
            Console.WriteLine();
            Console.WriteLine("Filtered Links-");
            foreach (String link in filteredLinksList)
            {
                Console.WriteLine(link);
            }
        }

        public int CountE()
        {
            return CountEntries(processedRoot.children);
        }

        public void ParseChildren(List<Child> children, List<Child> newchildren)
        {
            foreach (Child child in children)
            {
                entriesParsed++;

                bool filteredLink = false;
                bool validLink = false;
                bool duplicateLink = false;
                bool folder = false;

                if (child.uri != null)
                {


                    if (child.uri.ToString().StartsWith("https://www.watfordvalves.com"))
                    {
                        Console.WriteLine("");
                    }

                    if (child.uri.ToString().Length > 5)
                    {
                        if ((child.uri.ToString().StartsWith("http://")) || (child.uri.ToString().StartsWith("https://")))
                        {
                            // Check for filtered links
                            if (child.uri.ToString().ToLower().StartsWith(@"https://support.mozilla.org/en-gb/products/firefox")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"https://support.mozilla.org/en-gb/kb/customize-firefox-controls-buttons-and-toolbars")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"https://www.mozilla.org/en-gb/contribute/")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"https://www.mozilla.org/en-gb/about/")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"https://www.mozilla.org/en-gb/firefox/central/")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"http://en-gb.www.mozilla.com/en-gb/firefox/central/")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"http://www.mozilla.com/en-gb/firefox/central")) filteredLink = true;
                            if (child.uri.ToString().ToLower().StartsWith(@"http://www.mozilla.com/en-us/firefox/central")) filteredLink = true;
                            if (filteredLink)
                            {
                                if (!filteredLinksList.Contains(child.uri.ToString().ToLower())) filteredLinksList.Add(child.uri.ToString().ToLower());
                                filteredLinkQty++;
                            }
                            else
                            {
                                validLink = true;
                                validLinkQty++;
                            }

                            if (uniqueURLList.Contains(child.uri.ToString()))
                            {
                                duplicateLink = true;
                                duplicateLinkQty++;
                            }
                        }
                        else
                        {
                            if (!filteredLinksList.Contains(child.uri.ToString().ToLower())) filteredLinksList.Add(child.uri.ToString().ToLower());
                            filteredLinkQty++;
                        }
                    }
                }
                else
                {
                    folder = true;
                    folderQty++;
                }

                if ((!filteredLink && validLink && !duplicateLink) || folder)
                {
                    Child newchild = null;

                    foreach (Child c in newchildren)
                    {
                        if ((child.uri != null) && (c.uri != null))
                            if (child.uri.ToString().ToLower() == c.uri.ToString().ToLower()) newchild = c;

                        if ((child.uri == null) && (c.uri == null))
                            if (child.title == c.title) newchild = c;
                    }

                    if (newchild == null)
                    {
                        newchild = child;
                      //     newchild = new Child();
                     //   newchild.title = child.title;
                      //  newchild.uri = child.uri;
                        newchildren.Add(newchild);

                        if (child.uri != null)
                        {
                            uniqueURLList.Add(child.uri.ToString());
                            addedLinkQty++;
                            totalAddedLinkQty++;
                        }
                    }

                    if (child.children != null)
                    {
                        if (child.children.Count > 0)
                        {
                            if (newchild.children == null)
                            {
                                newchild.children = new List<Child>();
                            }

                            ParseChildren(child.children, newchild.children);
                        }
                    }
                }
            }
        }

        public bool PurgeOrphans(List<Child> children)
        {
            bool purge = true;

            List<Child> purgelist = new List<Child>();

            foreach (Child child in children)
            {
                if (child.uri != null)
                {
                    purge = false;
                    if (child.uri.ToString() == @"http://www.volvoclub.org.uk/index.php")
                    {
                        Console.WriteLine("RE");
                    }
                }
                else
                {
                    if (child.children != null)
                    {
                        if (child.children.Count > 0)
                        {
                            if (PurgeOrphans(child.children) == false) purge = false;

                            if (purge)
                            {
                                purgelist.Add(child);
                            }
                        }
                        else
                        {
                            purgelist.Add(child);
                        }
                    }
                    else
                    {
                        purgelist.Add(child);
                    }
                }
            }

            foreach (Child purgeitem in purgelist)
            {


                int childQty = 0;
                if (purgeitem.children != null)
                {
                    childQty = purgeitem.children.Count;
                    DumpText(0, purgeitem.children);
                }
                Console.WriteLine("Purging {0} {1}", purgeitem.title, childQty);
                children.Remove(purgeitem);
            }

            return purge;
        }


        public int CountEntries(List<Child> children)
        {
            int linkQty = 0;

            foreach (Child child in children)
            {
                if (child.uri != null)
                {
                    linkQty++;
                }

                if (child.children != null)
                {
                    if (child.children.Count > 0)
                    {
                        linkQty += CountEntries(child.children);
                    }
                }
            }
            return linkQty;
        }

        public int DumpText(int level, List<Child> children)
        {
            int linkQty = 0;

            foreach (Child child in children)
            {
                Console.WriteLine(new String(' ', level * 2) + child.title + " - " + child.uri);

                if (child.uri != null)
                {
                    linkQty++;
                }

                if (child.children != null)
                {
                    if (child.children.Count > 0)
                    {
                        linkQty += DumpText(level + 1, child.children);
                    }
                }
            }
            return linkQty;
        }
    }
}
