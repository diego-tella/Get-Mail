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
        }
        public static bool FileOrURL()
        {
            volt1:
            banner();
            Console.WriteLine("Qual o nome do site ou o arquivo com os sistes que você deseja extrair os emails?");
            StrFileorUrl = Console.ReadLine();
            if (!StrFileorUrl.Contains("http") && !StrFileorUrl.Contains(".txt"))
            {
                Console.WriteLine("Você precisa passar um arquivo ou uma URL.");
                Thread.Sleep(3000);
                Console.Clear();
                goto volt1;
            }
            else
            {
                if (StrFileorUrl.Contains("http"))
                    return false;
                else
                    return true; //file
            }
        }
        public static void GetMails()
        {
            if(FileOrURL()) //arquivo | ele já executa aqui ao invés de voltar uma variável
            {
                StreamReader str = new StreamReader(StrFileorUrl);
                StreamWriter esc = new StreamWriter("Encontrados.txt");
                string str2;
                while((str2 = str.ReadLine()) != null)
                {
                    Regex Reg = new Regex(@"[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?", RegexOptions.IgnoreCase);
                    string data;
                    using (WebClient web = new WebClient())
                    {
                        try
                        {
                            data = web.DownloadString(str2); //pega o código da página
                        }
                        catch(Exception ex)
                        {
                            data = "";
                        }
                    }

                    if (Reg.IsMatch(data))
                    {
                        MatchCollection matches = Reg.Matches(data); //coloca em uma lista "Match Coleção" todos os emails
                        string[] strvalor = new string[matches.Count]; //cria um array do tipo string com o tamanho da lista de Match Coleção
                        int j = 0;
                        foreach (Match math in matches) //Cria um elemento Match na "Match coleção"
                        {
                            strvalor[j] = math.ToString(); //passa o valor do math 1 para o array de strings
                            j++;
                        }
                        string[] q = strvalor.Distinct().ToArray(); //cria um novo array com os valores de strvalor sem os valores repetidos
                        Console.WriteLine("\n-----------------------------------------------------------------------\n" + "Emails encontrados em: " + str2);
                        esc.WriteLine("\n-----------------------------------------------------------------------\n" + "Emails encontrados em: " + str2);
                        foreach (var math in q) //foreach no novo array criado
                        {
                            Console.WriteLine(math); //printa o array
                            esc.WriteLine(math);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n-----------------------------------------------------------------------\n" + "Emails encontrados em: " + str2);
                        esc.WriteLine("\n-----------------------------------------------------------------------\n" + "Emails encontrados em: " + str2);
                        Console.WriteLine("Não foi achado nenhum email :/");
                        esc.WriteLine("Não foi achado nenhum email :/");
;                    }
                }
                esc.Close();
            }
            else //url
            {
                Regex Reg = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase); //[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?
                string data;
                using (WebClient web = new WebClient())
                    data = web.DownloadString(StrFileorUrl); //pega o código da página

                if (Reg.IsMatch(data))
                {
                    MatchCollection matches = Reg.Matches(data); //coloca em uma lista "Match Coleção" todos os emails
                    string[] strvalor = new string[matches.Count]; //cria um array do tipo string com o tamanho da lista de Match Coleção
                    int j = 0;
                    foreach (Match math in matches) //Cria um elemento Match na "Match coleção"
                    {
                        strvalor[j] = math.ToString(); //passa o valor do math 1 para o array de strings
                        j++;
                    }
                    string[] q = strvalor.Distinct().ToArray(); //cria um novo array com os valores de strvalor sem os valores repetidos
                    Console.WriteLine("\n"+ q.Length +" emails encontrados!");
                    foreach (var math in q) //foreach no novo array criado
                        Console.WriteLine(math); //printa o array
                }
                else
                    Console.WriteLine("Não foi achado nenhum email :/");
            }
            Console.WriteLine("\n----------------------------------------------------------------------\nFim do scan. Pressione qualquer tecla pra sair.");
            Console.ReadKey();
        }
    }
}
