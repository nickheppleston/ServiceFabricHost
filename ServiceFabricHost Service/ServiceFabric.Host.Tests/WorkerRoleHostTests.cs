using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceFabric;
using System.Threading;

namespace ServiceFabric.Host.Tests
{
    [TestClass]
    public class WorkerRoleHostTests
    {
        private ServiceFabricHost serviceFabricHost;

        [TestInitialize]
        public void TestSetup()
        {
            serviceFabricHost = new ServiceFabricHost();
        }

        [TestMethod]
        public void OnStartOnStop()
        {
            serviceFabricHost.OnStart();

            Thread.Sleep(new TimeSpan(0, 0, 10));

            serviceFabricHost.OnStop();
        }
    }
}
