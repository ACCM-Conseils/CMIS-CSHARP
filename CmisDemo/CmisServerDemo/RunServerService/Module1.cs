using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
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
        public const string ServiceName = "RunServerService";
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            if (Environment.UserInteractive)
            {
                launch();
            }
            else
            {
                // running as service
                using (var service = new Service())
                {
                    ServiceBase.Run(service);
                }
            }
        }

        public static void launch()
        {
            Thread.Sleep(10000);
            log.Info(Environment.CommandLine);

            string url = string.Format(System.Configuration.ConfigurationManager.AppSettings["url"], System.Configuration.ConfigurationManager.AppSettings["domain"]);

            // Cmis-Service
            // ' ' ' ' ' ' '
            log.Info("Cmis-Service - Docuware");

            var cmisHost_AtomPub = new CmisObjectModel.ServiceModel.AtomPub.ServiceManager();
            string url_AomPub = url + "/atom";
            cmisHost_AtomPub.Open(new Uri(url_AomPub));
            log.Info(" - AtomPub: " + url_AomPub);

            // Dim cmisHost_Browser As New CmisObjectModel.ServiceModel.Browser.ServiceManager()
            // Dim url_Browser As String = url & "/browser"
            // cmisHost_Browser.Open(New Uri(url_Browser))
            // log.Info(" - Browser: " & url_Browser)

            log.Info("Point de terminaison");
            log.Info(" - AtomPub: " + url_AomPub + "/help");
            // log.Info(" - Browser: " & url_Browser & "/help")

            log.Info("Autres caractéristiques");
            log.Info(" - RepositoryId: " + System.Configuration.ConfigurationManager.AppSettings["repoid"]);
            log.Info(" - Root-Folder: root");

            // Web-Service
            // ' ' ' ' ' '
            log.Info("Web-Service (URL-Templates)");
            string url_Web = url + "extra";
            log.Info(" - Web: " + url_Web);
            var webHost = new System.ServiceModel.ServiceHost(typeof(WebServer.WebService), new Uri(url_Web));
            var secureWebHttpBinding = new WebHttpBinding(WebHttpSecurityMode.Transport) { Name = "secureHttpWeb" };
            //webHost.AddServiceEndpoint(typeof(WebServer.IWebService), secureWebHttpBinding, string.Empty);
            webHost.AddServiceEndpoint(typeof(WebServer.IWebService), new System.ServiceModel.WebHttpBinding(), string.Empty);
            webHost.Description.Endpoints.Single().Behaviors.Add(new System.ServiceModel.Description.WebHttpBehavior());
            webHost.Open();
            log.Info(" - Aperçu: " + url_Web + "/obj?id={0}");
            log.Info(" - Métadonnées: " + url_Web + "/meta?id={0}");
            log.Info(" - Download:  " + url_Web + "/file?id={0}");

            bool result;

            // Checking the process is running in user
            // interactive mode or not
            // Using the UserInteractive property
            result = Environment.UserInteractive;

            if (result)
                log.Info("running in interactive mode ...");
            else
                log.Info("running in non interactive mode...");
            log.Info("Ctrl+C for exit");

            while (true)
            {
                System.Threading.Thread.Sleep(100);
                lock (global::CmisServer.CmisServiceImpl.InMemoryLogQueue)
                {
                    while (global::CmisServer.CmisServiceImpl.InMemoryLogQueue.Count > 0)
                    {
                        string text = global::CmisServer.CmisServiceImpl.InMemoryLogQueue.Dequeue();

                        if (text.StartsWith("ERROR"))
                        {
                            log.Info(text);
                        }
                        else if (text.Contains("Check") || text.Contains("Create") || text.Contains("Delete") || text.Contains("Properties") || text.Contains("Content"))
                        {
                            log.Info(text);
                        }
                        else
                        {
                            log.Info(text);
                        }
                    }
                }

            }
        }

        public static void Start(string[] args)
        {
            System.Threading.Tasks.Task.Run(() => launch());
            File.AppendAllText(@"c:\temp\MyService.txt", String.Format("{0} started{1}", DateTime.Now, Environment.NewLine));
        }

        public static void Stop()
        {
            File.AppendAllText(@"c:\temp\MyService.txt", String.Format("{0} stopped{1}", DateTime.Now, Environment.NewLine));
        }

        public static void WriteLineInColor(ConsoleColor color, string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            log.Info(text);
            Console.ForegroundColor = oldColor;
        }
    }
}