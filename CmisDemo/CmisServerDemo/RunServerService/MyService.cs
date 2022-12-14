using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace RunServer
{
    class Service : ServiceBase
    {
        public Service()
        {
            ServiceName = Module1.ServiceName;
        }

        protected override void OnStart(string[] args)
        {
            Module1.Start(args);
        }

        protected override void OnStop()
        {
            Module1.Stop();
        }
    }
}
