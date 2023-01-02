using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace RunServer
{
    /// <summary>
/// Programm zum Starten des Demo-CmisService
/// </summary>
/// <remarks>
/// Bitte als Administrator ausführen!
/// </remarks>
    static class Module1
    {

        public static void Main()
        {
            Console.WriteLine(Environment.CommandLine);
            Console.WriteLine();

            string url = string.Format(System.Configuration.ConfigurationManager.AppSettings["url"], System.Configuration.ConfigurationManager.AppSettings["domain"]);

            // Cmis-Service
            // ' ' ' ' ' ' '
            Console.WriteLine("Cmis-Service - Docuware");

            var cmisHost_AtomPub = new CmisObjectModel.ServiceModel.AtomPub.ServiceManager();
            string url_AomPub = url + "/atom";
            cmisHost_AtomPub.Open(new Uri(url_AomPub));
            Console.WriteLine(" - AtomPub: " + url_AomPub);

            // Dim cmisHost_Browser As New CmisObjectModel.ServiceModel.Browser.ServiceManager()
            // Dim url_Browser As String = url & "/browser"
            // cmisHost_Browser.Open(New Uri(url_Browser))
            // Console.WriteLine(" - Browser: " & url_Browser)

            Console.WriteLine();

            Console.WriteLine("Point de terminaison");
            Console.WriteLine(" - AtomPub: " + url_AomPub + "/help");
            // Console.WriteLine(" - Browser: " & url_Browser & "/help")

            Console.WriteLine();

            Console.WriteLine("Autres caractéristiques");
            Console.WriteLine(" - RepositoryId: " + System.Configuration.ConfigurationManager.AppSettings["repoid"]);
            Console.WriteLine(" - Root-Folder: root");
            Console.WriteLine();

            // Web-Service
            // ' ' ' ' ' '
            Console.WriteLine("Web-Service (URL-Templates)");
            string url_Web = url + "extra";
            var webHost = new System.ServiceModel.ServiceHost(typeof(WebServer.WebService), new Uri(url_Web));
            var secureWebHttpBinding = new WebHttpBinding(WebHttpSecurityMode.Transport) { Name = "secureHttpWeb" };
            //webHost.AddServiceEndpoint(typeof(WebServer.IWebService), secureWebHttpBinding, string.Empty);
            webHost.AddServiceEndpoint(typeof(WebServer.IWebService), new System.ServiceModel.WebHttpBinding(), string.Empty);
            webHost.Description.Endpoints.Single().Behaviors.Add(new System.ServiceModel.Description.WebHttpBehavior());
            webHost.Open();
            Console.WriteLine(" - Aperçu: " + url_Web + "/obj?id={0}");
            Console.WriteLine(" - Métadonnées: " + url_Web + "/meta?id={0}");
            Console.WriteLine(" - Download:  " + url_Web + "/file?id={0}");
            Console.WriteLine();

            bool result;

            // Checking the process is running in user
            // interactive mode or not
            // Using the UserInteractive property
            result = Environment.UserInteractive;

            if(result)
                Console.WriteLine("running in interactive mode ...");
            else
                Console.WriteLine("running in non interactive mode...");
            Console.WriteLine("Ctrl+C for exit");
            Console.WriteLine();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    char key = Console.ReadKey(false).KeyChar;
                    if (Conversions.ToString(key) == Constants.vbCr)
                    {
                        Console.WriteLine(Constants.vbLf + new string('-', Console.WindowWidth - 1));
                    }
                }
                System.Threading.Thread.Sleep(100);
                lock (global::CmisServer.CmisServiceImpl.InMemoryLogQueue)
                {
                    while (global::CmisServer.CmisServiceImpl.InMemoryLogQueue.Count > 0)
                    {
                        string text = global::CmisServer.CmisServiceImpl.InMemoryLogQueue.Dequeue();

                        if (text.StartsWith("ERROR"))
                        {
                            WriteLineInColor(ConsoleColor.Red, text);
                        }
                        else if (text.Contains("Check") || text.Contains("Create") || text.Contains("Delete") || text.Contains("Properties") || text.Contains("Content"))
                        {
                            WriteLineInColor(ConsoleColor.Yellow, text);
                        }
                        else
                        {
                            Console.WriteLine(text);
                        }
                    }
                }

            }

        }

        public static void WriteLineInColor(ConsoleColor color, string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }
    }
}