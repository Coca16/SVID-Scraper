using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using SpookVooper.Api.Entities;
using SpookVooper.Api;
using System.Threading.Tasks;
using SpookVooper.Api.Entities.Groups;

namespace SVID_Scraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Include names? (Y/N)");
            string input = Console.ReadLine();
            while (input.ToLower() != "n" && input.ToLower() != "no" && input.ToLower() != "y" && input.ToLower() != "yes")
            {
                Console.WriteLine("This string does not equal Y/N or lower case y/n. Try again.");
                input = Console.ReadLine();
            }

            HtmlWeb web = new HtmlWeb();

            char[] az = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();
            List<int> numberList = Enumerable.Range(0, 9).ToList();
            
            List<string> tableeallsvid = new List<string>();

            foreach (var c in az)
            {
                HtmlDocument doc = web.Load("https://spookvooper.com/User/Search/" + c);

                List<string> listsvid = new List<string>();

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
                                listsvid.Add(pain.GetAttributeValue("href", "").Replace("/User/Info?svid=", ""));
                            }
                        }
                    }
                }

                tableeallsvid.AddRange(listsvid);
                await Task.Delay(100);
            }

            foreach (var c in numberList)
            {

                HtmlDocument doc = web.Load("https://spookvooper.com/User/Search/" + c);

                List<string> listsvid = new List<string>();

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
                                listsvid.Add(pain.GetAttributeValue("href", "").Replace("/User/Info?svid=", ""));
                            }
                        }
                    }
                }

                tableeallsvid.AddRange(listsvid);
                await Task.Delay(100);
            }

            List<string> nodupetableallsvid = tableeallsvid.Distinct().ToList();

            nodupetableallsvid.RemoveAt(0);

            TextWriter twsvid = new StreamWriter("allUserSVIDs.txt");
            TextWriter twname = new StreamWriter("allUserNames.txt");

            foreach (String s in nodupetableallsvid)
            {
                twsvid.WriteLine(s);
                if (input.ToLower() != "y" || input.ToLower() != "yes")
                {
                    User user = new User(s);
                    twname.WriteLine(await user.GetUsernameAsync());
                }
                await Task.Delay(100);
            }
            twname.Close();
            twsvid.Close();
            
            List<string> tableeallgroupssvid = new List<string>();


            foreach (var c in az)
            {
                HtmlDocument doc = web.Load("https://spookvooper.com/Group/Search/" + c);

                List<string> listsvid = new List<string>();

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
                                listsvid.Add(pain.GetAttributeValue("href", "").Replace("/User/Info?svid=", ""));
                            }
                        }
                    }
                }

                tableeallgroupssvid.AddRange(listsvid);
                await Task.Delay(100);
            }

            foreach (var c in numberList)
            {

                HtmlDocument doc = web.Load("https://spookvooper.com/Group/Search/" + c);

                List<string> listsvid = new List<string>();

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
                                listsvid.Add(pain.GetAttributeValue("href", "").Replace("/User/Info?svid=", ""));
                            }
                        }
                    }
                }

                await Task.Delay(100);
            }

            List<string> nodupetableallgroups = tableeallgroupssvid.Distinct().ToList();

            nodupetableallgroups.RemoveAt(0);

            TextWriter twnamegroups = new StreamWriter("allGroupsNames.txt");
            TextWriter twsvidgroups = new StreamWriter("allGroupsSVIDs.txt");

            foreach (String s in nodupetableallgroups)
            {
                twnamegroups.WriteLine(s);
                if (input.ToLower() != "y" || input.ToLower() != "yes")
                {
                    Group group = new Group(s);
                    twname.WriteLine(await group.GetNameAsync());
                }
                await Task.Delay(100);
            }
            twnamegroups.Close();
            twsvidgroups.Close();
        }
    }
}
