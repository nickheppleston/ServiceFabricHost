using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using ServiceFabric.Host.Helpers;
using ServiceFabric.Host.Common;
using ServiceFabric.Host.Common.Configuration;
using ServiceFabric.Host.Exceptions;

namespace ServiceFabric.Host
{
    public class ServiceFabricHost
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static ConfigurationPropertyBag configurationPropertyBag = ConfigurationPropertyBagProvider.GetPropertyBag();

        private int _workerRoleInstanceCount;
        private List<IBaseWorkerRole> _workerRoleInstanceList;
        private List<Task> _runTaskAsyncList;
        private CancellationTokenSource _cancellationTokenSource;

        public ServiceFabricHost() 
        {
            _workerRoleInstanceList = new List<IBaseWorkerRole>();
            _workerRoleInstanceCount = System.Convert.ToInt32(configurationPropertyBag.GetProperty("WorkerRoleCount"));
            _runTaskAsyncList = new List<Task>();

            // Define a Cancellation Token Source
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task OnStart()
        {
            logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); // TODO: Remove
            logger.Debug("OnStart() - Entered method.");

            for (int i = 0; i < _workerRoleInstanceCount; i++)
            {
                logger.Info(String.Format("Creating instance {0} of Worker Role '{1}'", i, configurationPropertyBag.GetProperty("WorkerRoleTypeName")));

                try
                {
                    // Load an instance of the supplied Assembly into the 'ServiceFabricAppDomain'
                    var workerRoleInstance = AssemblyHelpers.GetWorkerRoleInstance(configurationPropertyBag.GetProperty("WorkerRoleAssemblyPath"), configurationPropertyBag.GetProperty("WorkerRoleTypeName"));

                    if (workerRoleInstance == null)
                        throw new FailedToStartHostException(String.Format("Worker Role Instance of type '{0}' was null.", configurationPropertyBag.GetProperty("WorkerRoleTypeName")));

                    // Pass the Configuration Property Bag to the Worker Role Instance
                    workerRoleInstance.SetConfigurationPropertyBag(configurationPropertyBag);

                    // Add the WorkerRoleInstance to our list of instances for tracking purposes.
                    _workerRoleInstanceList.Add(workerRoleInstance);
                }
                catch (FailedToGetWorkerRoleInstanceException failedToGetWorkerRoleInstanceException)
                {
                    logger.Error("FailedToGetWorkerRoleInstanceException: {0}", failedToGetWorkerRoleInstanceException.Message);
                    throw new FailedToStartHostException(String.Empty);
                }
            }

            logger.Debug("Finished preparing Worker Role Instances");

            try
            {
                // Start the Worker Role instances...
                await StartWorkerRoleInstances();

                // Run the Worker Role instances...
                await RunWorkerRoleInstances();
            }
            catch (Exception exception)
            {
                logger.Error("OnStart() Error: {0}", exception.Message);
            }
        }

        public async Task OnStop()
        {
            logger.Debug("OnStop() - Entered method.");

            // Issue the Cancellation Token
            IssueCancellationToken();

            // Wait for the Worker Role Instance's Run() method to complete
            WaitForWorkerRoleRunToComplete();

            // Call OnStop() on each of the Worker Role instances
            await StopWorkerRoleInstances();

            logger.Info("All Worker Role instances have stopped.");
        }

        private async Task StartWorkerRoleInstances()
        {
            logger.Debug("StartWorkerRoleInstances() - Entered method");

            logger.Info("Invoking the OnStart() method on each of the {0} Worker Role instances...", _workerRoleInstanceList.Count);

            // Call the OnStart() method on each of the Worker Role instances
            var onStartAsyncTaskList = new List<Task>();
            foreach (var workerRoleInstance in _workerRoleInstanceList)
            {
                onStartAsyncTaskList.Add(workerRoleInstance.OnStartAsync());
            }

            logger.Info("Waiting for Worker Role OnStart() to complete...");

            // Wait for the OnStart() method to complete on each of the Worker Role instances
            while (onStartAsyncTaskList.Count > 0)
            {
                var completedTask = await Task.WhenAny(onStartAsyncTaskList);
                onStartAsyncTaskList.Remove(completedTask);
            }

            logger.Info("All Worker Role instances have started.");
        }

        private async Task RunWorkerRoleInstances()
        {
            logger.Debug("RunWorkerRoleInstances() - Entered method");

            logger.Info("Invoking the Run() method on each of the {0} Worker Role instances...", _workerRoleInstanceList.Count);

            foreach (var workerRoleInstance in _workerRoleInstanceList)
            {
                _runTaskAsyncList.Add(workerRoleInstance.RunAsync(_cancellationTokenSource.Token));
            }

            logger.Info("All Worker Role instances running.");
        }

        private async Task StopWorkerRoleInstances()
        {
            logger.Debug("StopWorkerRoleInstances() - Entered method");

            logger.Info("Invoking the OnStop() method on each of the {0} Worker Role instances...", _workerRoleInstanceList.Count);

            // Call the OnStop() method on each of the Worker Role instances
            var onStopAsyncTaskList = new List<Task>(_workerRoleInstanceList.Count);
            foreach (var workerRoleInstance in _workerRoleInstanceList)
            {
                onStopAsyncTaskList.Add(workerRoleInstance.OnStopAsync());
            }

            logger.Info("Waiting for Worker Role OnStop() to complete...");

            // Wait for the OnStop() method to complete on each of the Worker Role instances
            while (onStopAsyncTaskList.Count > 0)
            {
                var completedTask = await Task.WhenAny(onStopAsyncTaskList);
                onStopAsyncTaskList.Remove(completedTask);
            }
        }

        private async void WaitForWorkerRoleRunToComplete()
        {
            logger.Debug("WaitForWorkerRoleRunToComplete() - Entered method");

            while (_runTaskAsyncList.Count > 0)
            {
                var completedTask = await Task.WhenAny(_runTaskAsyncList);
                _runTaskAsyncList.Remove(completedTask);
            }

            logger.Info("All running Worker Role instances completed.");
        }

        private void IssueCancellationToken()
        {
            logger.Debug("IssueCancellationToken() - Entered method");

            _cancellationTokenSource.Cancel();

            logger.Info("Cancellation token issued to running Worker Role instances.");
        }
    }
}
