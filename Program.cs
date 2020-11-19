using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using SpookVooper.Api;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SVID_Scraper
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Remove names from groups which cannot be converted to SVID because of having space at the end? (Y/N)");
            string input = Console.ReadLine();
            while (input.ToLower() != "n" && input.ToLower() != "no" && input.ToLower() != "y" && input.ToLower() != "yes")
            {
                Console.WriteLine("This string does not equal Y/N or lower case y/n. Try again.");
                input = Console.ReadLine();
            }

            HtmlWeb web = new HtmlWeb();

            char[] az = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();
            List<int> numberList = Enumerable.Range(0, 9).ToList();
            
            List<string> tableeall = new List<string>();

            foreach (var c in az)
            {
                HtmlDocument doc = web.Load("https://spookvooper.com/User/Search/" + c);

                List<string> list = new List<string>();

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    ///This is the table.    
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        ///This is the row.
                        foreach (HtmlNode cell in row.SelectNodes("td"))
                        {
                            ///This the cell.
                            foreach (HtmlNode pain in cell.SelectNodes("a"))
                            {
                                string text = Regex.Replace(pain.InnerText, @"\s+", "");
                                list.Add(text);
                            }
                        }
                    }
                }

                tableeall.AddRange(list);
                await Task.Delay(100);
            }

            foreach (var c in numberList)
            {

                HtmlDocument doc = web.Load("https://spookvooper.com/User/Search/" + c);

                List<string> list = new List<string>();

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    ///This is the table.    
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        ///This is the row.
                        foreach (HtmlNode cell in row.SelectNodes("td"))
                        {
                            ///This the cell.
                            foreach (HtmlNode pain in cell.SelectNodes("a"))
                            {
                                string text = Regex.Replace(pain.InnerText, @"\s+", "");
                                list.Add(text);
                            }
                        }
                    }
                }

                tableeall.AddRange(list);
                await Task.Delay(100);
            }

            List<string> nodupetableall = tableeall.Distinct().ToList();

            nodupetableall.RemoveAt(0);

            TextWriter twname = new StreamWriter("allUserNames.txt");
            TextWriter twsvid = new StreamWriter("allUserSVIDs.txt");

            foreach (String s in nodupetableall)
            {
                twname.WriteLine(s);
                string SVID = await SpookVooperAPI.Users.GetSVIDFromUsername(s);
                twsvid.WriteLine(SVID);
                await Task.Delay(100);
            }
            twname.Close();
            twsvid.Close();
            
            List<string> tableeallgroups = new List<string>();
       
            foreach (var c in az)
            {
                HtmlDocument doc = web.Load("https://spookvooper.com/Group/Search/" + c);

                List<string> list = new List<string>();

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    ///This is the table.    
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        ///This is the row.
                        foreach (HtmlNode cell in row.SelectNodes("td"))
                        {
                            ///This the cell.
                            foreach (HtmlNode pain in cell.SelectNodes("a"))
                            {
                                string text = pain.InnerText.TrimStart().TrimEnd().TrimEnd('\n', '\r');
                                list.Add(text);
                            }
                        }
                    }
                }

                tableeallgroups.AddRange(list);
                await Task.Delay(100);
            }

            foreach (var c in numberList)
            {

                HtmlDocument doc = web.Load("https://spookvooper.com/Group/Search/" + c);

                List<string> list = new List<string>();

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    ///This is the table.    
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        ///This is the row.
                        foreach (HtmlNode cell in row.SelectNodes("td"))
                        {
                            ///This the cell.
                            foreach (HtmlNode pain in cell.SelectNodes("a"))
                            {
                                string text = pain.InnerText.TrimStart().TrimEnd().TrimEnd('\n','\r');
                                list.Add(text);
                            }
                        }
                    }
                }

                tableeallgroups.AddRange(list);
                await Task.Delay(100);
            }

            List<string> nodupetableallgroups = tableeallgroups.Distinct().ToList();

            nodupetableallgroups.RemoveAt(0);

            TextWriter twnamegroups = new StreamWriter("allGroupsNames.txt");
            TextWriter twsvidgroups = new StreamWriter("allGroupsSVIDs.txt");

            foreach (String s in nodupetableallgroups)
            {
                string SVID = await SpookVooperAPI.Groups.GetSVIDFromName(s.TrimEnd());
                if (SVID != null)
                {
                    twsvidgroups.WriteLine(SVID);
                    if (input.ToLower() == "n" || input.ToLower() == "no")
                    {
                        twnamegroups.WriteLine(s);
                    }
                }
                await Task.Delay(100);
            }
            twnamegroups.Close();
            twsvidgroups.Close();
            
            Console.WriteLine(nodupetableall.Count());
        }
    }
}
