using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkParser
{
    public class HTMLExporter
    {
        public void Export(int level, List<Child> children)
        {
            foreach (Child child in children)
            {
                Console.WriteLine(new String(' ', level * 2) + child.title + " - " + child.uri);

                if (child.children != null)
                {
                    if (child.children.Count > 0)
                    {
                        Export(level + 1, child.children);
                    }
                }
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE NETSCAPE-Bookmark-file-1>");
            sb.AppendLine("<!--This is an automatically generated file.");
            sb.AppendLine("It will be read and overwritten.");
            sb.AppendLine("Do Not Edit!-->");
            sb.AppendLine("<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">");
            sb.AppendLine("<Title > Bookmarks </ Title > ");
            sb.AppendLine("<H1>Bookmarks</H1>");
            sb.AppendLine("<DL><p>");
            sb.AppendLine("<DT><A HREF=\"http://www.daveeddy.com\">Dave Eddy</a>");
            sb.AppendLine("<DT><A HREF=\"http://www.perfume-global.com\">Perfume Global</a>");
            sb.AppendLine("</DL><p>");

        }
    }
}
