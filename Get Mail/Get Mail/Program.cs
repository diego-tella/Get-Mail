using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Get_Mail
{
    class Program
    {
        public static string StrFileorUrl;
        public static bool prox = false;
        static void Main(string[] args)
        {
            GetMails();
        }
        public static void banner()
        {
            Console.WriteLine(@"  ________        __       _____         .__.__   ");
            Console.WriteLine(@" /  _____/  _____/  |_    /     \ _____  |__|  |  ");
            Console.WriteLine(@"/   \  ____/ __ \   __\  /  \ /  \\__  \ |  |  |  ");
            Console.WriteLine(@"\    \_\  \  ___/|  |   /    Y    \/ __ \|  |  |__");
            Console.WriteLine(@" \______  /\___  >__|   \____|__  (____  /__|____/");
            Console.WriteLine(@"        \/     \/               \/     \/         ");
            Console.WriteLine("------https://github.com/diego-tella/Get-Mail------\n");
        }
        public static bool FileOrURL()
        {
            volt1:
            banner();
            Console.WriteLine("Pass a site or a list (.txt file) with different sites to start the scan");
            StrFileorUrl = Console.ReadLine();
            if (!StrFileorUrl.Contains("http") && !StrFileorUrl.Contains(".txt"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You need to pass a valid URL or a text file");
                Thread.Sleep(3000);
                Console.ResetColor();
                Console.Clear();
                goto volt1;
            }
            else
            {
                if (StrFileorUrl.Contains("http"))
                    return false; //return false case it is a site
                else
                    return true;
            }
        }
        public static void GetMails()
        {
            if(FileOrURL()) 
            {
                StreamReader str = new StreamReader(StrFileorUrl);
                StreamWriter esc = new StreamWriter("found.txt");
                esc.WriteLine("List of all emails found");
                string str2;
                while((str2 = str.ReadLine()) != null)
                {
                    Regex Reg = new Regex(@"[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?", RegexOptions.IgnoreCase);
                    string data;
                    using (WebClient web = new WebClient())
                    {
                        try
                        {
                            data = web.DownloadString(str2);
                        }
                        catch(Exception ex)
                        {
                            data = "";
                        }
                    }

                    if (Reg.IsMatch(data))
                    {
                        MatchCollection matches = Reg.Matches(data);
                        string[] strvalor = new string[matches.Count]; 
                        int j = 0;
                        foreach (Match math in matches) 
                        {
                            strvalor[j] = math.ToString(); 
                            j++;
                        }
                        string[] q = strvalor.Distinct().ToArray();
                        Console.WriteLine("\n-----------------------------------------------------------------------\n" + "Emails found in: " + str2);
                        foreach (var math in q) 
                        {
                            Console.WriteLine(math); 
                            esc.WriteLine(math);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n-----------------------------------------------------------------------\n" + "Emails found in: " + str2);
                        Console.WriteLine("No email found :/ ");
;                    }
                }
                esc.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nA list with all emails found was created in " + Environment.CurrentDirectory + @"\found.txt");
                Console.ResetColor();
            }
            else //url
            {
                Regex Reg = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase); //[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?
                string data;
                try
                {
                    using (WebClient web = new WebClient())
                        data = web.DownloadString(StrFileorUrl);
                }
                catch(Exception e)
                {
                    data = "";
                    Console.WriteLine("Error! " + e.ToString());
                    Environment.Exit(0);
                }

                if (Reg.IsMatch(data))
                {
                    MatchCollection matches = Reg.Matches(data); 
                    string[] strvalor = new string[matches.Count]; 
                    int j = 0;
                    foreach (Match math in matches) 
                    {
                        strvalor[j] = math.ToString(); 
                        j++;
                    }
                    string[] q = strvalor.Distinct().ToArray(); 
                    Console.WriteLine("\n"+ q.Length + " emails found!");
                    foreach (var math in q) 
                        Console.WriteLine(math);
                }
                else
                    Console.WriteLine("No email found :/");
            }
            Console.WriteLine("\n----------------------------------------------------------------------\nEnd of scan. press any key to exit");
            Console.ReadKey();
        }
    }
}
