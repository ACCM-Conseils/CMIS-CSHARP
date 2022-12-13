using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleCMIS
{
    public partial class Service1 : ServiceBase
    {
        private System.Threading.Tasks.Task _proccessSmsQueueTask;
        private CancellationTokenSource _cancellationTokenSource;
        private Process p = new Process();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _proccessSmsQueueTask = System.Threading.Tasks.Task.Run(() => DoWorkAsync(_cancellationTokenSource.Token));
        }

        public async System.Threading.Tasks.Task DoWorkAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    ProccessSmsQueue();
                }
                catch (Exception e)
                {
                    // Handle exception
                }
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromMinutes(1), token);
            }
        }

        void ProccessSmsQueue()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Temp\Logs\ConsoleCMIS-" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt", true))
            {
                try
                {
                    bool isRunning = Process.GetProcessesByName("CMIS Server Docuware").FirstOrDefault(p => p.MainModule.FileName.StartsWith(@"C:\CMIS Server Docuware")) != default(Process);

                    writer.WriteLine("Service running : " + isRunning);
                    if (!isRunning)
                    {
                        writer.WriteLine("Lancement process");

                        using (TaskService tasksrvc = new TaskService(@"\\" + "WIN-N5FB7PMCSME", "Administrator", "WORKGROUP", "578DocuWare!"))
                        {
                            tasksrvc.FindTask("CMIS").Run();
                            /*var t = tasksrvc.Execute(@"C:\CMIS Server Docuware\RunServer.exe")
                                .Once()
                                .Starting(DateTime.Now.AddSeconds(5))
                                .AsTask("CMIS");*/

                        }
                    }
                }
                catch (Exception e)
                {
                    writer.WriteLine(e.Message);
                }
            }
        }

        protected override void OnStop()
        {
            p.Kill();
        }
    }
}
