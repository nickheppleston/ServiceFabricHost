using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using ServiceFabric.Host;
using ServiceFabric.Host.Exceptions;

namespace ServiceFabric.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HostFactory.Run(x =>
                {
                    x.Service<ServiceFabricHost>(sc =>
                    {
                        sc.ConstructUsing(() => new ServiceFabricHost());

                        // Define Start, Stop and Shutdown methods for the service
                        sc.WhenStarted(s => s.OnStart());
                        sc.WhenStopped(s => s.OnStop());
                        sc.WhenShutdown(s => s.OnStop());
                    });

                    x.RunAsLocalSystem();
                    x.SetDescription("Provides a multi-threaded service host for custom Worker Roles.");
                    x.SetDisplayName("Service Fabric Host");
                    x.SetServiceName("ServiceFabricHost");
                });  
            }
            catch (FailedToStartHostException failedToStartHostException)
            {
                throw; // TODO
            }
            catch (FailedToStopHostException failedToStopHostException)
            {
                throw; // TODO
            }
        }
    }
}
