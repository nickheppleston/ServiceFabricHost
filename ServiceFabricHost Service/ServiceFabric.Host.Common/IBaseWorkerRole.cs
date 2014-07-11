using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceFabric.Host.Common.Configuration;

namespace ServiceFabric.Host.Common
{
    public interface IBaseWorkerRole
    {
        void SetConfigurationPropertyBag(ConfigurationPropertyBag configurationPropertyBag);

        Task OnStartAsync();

        Task RunAsync(CancellationToken cancellationToken);

        Task OnStopAsync();
    }
}
