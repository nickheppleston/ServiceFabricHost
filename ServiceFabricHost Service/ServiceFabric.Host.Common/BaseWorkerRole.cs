using NLog;
using ServiceFabric.Host.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFabric.Host.Common
{
    //[Serializable]
    public abstract class BaseWorkerRole
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public ConfigurationPropertyBag _configurationPropertyBag;

        public void SetConfigurationPropertyBag(ConfigurationPropertyBag configurationPropertyBag)
        {
            _logger.Debug("BaseWorkerRole.SetConfigurationPropertyBag() - Setting Configuration Property Bag...");

            _configurationPropertyBag = configurationPropertyBag;
        }

        public async Task Sleep(TimeSpan delay, CancellationToken cancellationToken)
        {
            _logger.Debug("BaseWorkerRole.Sleep() - Sleeping until {0}", DateTime.Now.Add(delay).ToString("yyyy-MM-dd @ HH:mm:ss"));

            await Task.Delay(delay, cancellationToken);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogException(string initialMessage, Exception exception)
        {
            if (exception.InnerException != null)
                _logger.Error(String.Format("{0} - {1}. Error: {2}", initialMessage, exception.Message, exception.InnerException.Message));
            else
                _logger.Error(String.Format("{0} - {1}.", initialMessage, exception.Message));
        }
    }
}
