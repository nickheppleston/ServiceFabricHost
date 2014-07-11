using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.Net;
using NLog;
using ServiceFabric.Host.Common;
using ServiceFabric.Host.Common.Configuration;

namespace ServiceFabric.Host.Tests.SampleWorkerRole
{
    [Serializable]
    public class SampleWorkerRoleImpl : IBaseWorkerRole
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); 
        
        private string _id;
        private ConfigurationPropertyBag _configurationPropertyBag;

        public SampleWorkerRoleImpl()
        {
            _id = Guid.NewGuid().ToString();

        }

        public void SetConfigurationPropertyBag(ConfigurationPropertyBag configurationPropertyBag)
        {
            _configurationPropertyBag = configurationPropertyBag;
        }

        public async Task OnStartAsync()
        {
            logger.Debug(String.Format("SampleWorkerRole - OnStart({0})", _id));
            Trace.WriteLine(String.Format("SampleWorkerRole - OnStart({0})", _id));
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            logger.Debug(String.Format("SampleWorkerRole - Run({0})", _id));
            System.Diagnostics.Trace.WriteLine(String.Format("SampleWorkerRole - Run({0})", _id));

            while (!cancellationToken.IsCancellationRequested)
            {
                logger.Debug(String.Format(String.Format("SampleWorkerRole - Run({0}) - In WHILE loop", _id)));
                System.Diagnostics.Trace.WriteLine(String.Format("SampleWorkerRole - Run({0}) - In WHILE loop", _id));
                await Task.Delay(new TimeSpan(0, 0, 1));
            }
        }

        public async Task OnStopAsync()
        {
            logger.Debug(String.Format("SampleWorkerRole - OnStop({0})", _id));
            Trace.WriteLine(String.Format("SampleWorkerRole - OnStop({0})", _id));
        }
    }
}
