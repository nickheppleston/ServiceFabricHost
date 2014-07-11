using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceFabric;
using ServiceFabric.Host.Common;
using ServiceFabric.Host.Exceptions;
using ServiceFabric.Host.Helpers;
using ServiceFabric.Host.Tests.SampleWorkerRole;

namespace ServiceFabric.Host.Tests
{
    [TestClass]
    public class AssemblyHelperTests
    {
        [TestMethod]
        public void GetWorkerRoleInstance()
        {
            var assemblyPath = Assembly.GetAssembly(typeof(SampleWorkerRoleImpl)).Location;
            var typeName = typeof(SampleWorkerRoleImpl).FullName;

            var workerRoleInstance = AssemblyHelpers.GetWorkerRoleInstance(assemblyPath, typeName);

            Assert.IsNotNull(workerRoleInstance);
            Assert.IsInstanceOfType(workerRoleInstance, typeof(IBaseWorkerRole));
        }

        [TestMethod]
        [ExpectedException(typeof(FailedToGetWorkerRoleInstanceException))]
        public void GetWorkerRoleInstance_InvalidAssemblyPathThrowsException()
        {
            var assemblyPath = @"C:\InvalidPath";
            var typeName = typeof(SampleWorkerRoleImpl).FullName;

            var workerRoleInstance = AssemblyHelpers.GetWorkerRoleInstance(assemblyPath, typeName);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedToGetWorkerRoleInstanceException))]
        public void GetWorkerRoleInstance_InvalidTypeNameThrowsException()
        {
            var assemblyPath = Assembly.GetAssembly(typeof(SampleWorkerRoleImpl)).Location;
            var typeName = "InvalidTypeName";

            var workerRoleInstance = AssemblyHelpers.GetWorkerRoleInstance(assemblyPath, typeName);
        }
        
        //[TestMethod]
        //public void StartWorkerRoles()
        //{
        //    var workerRoleHost = new WorkerRoleHost();
        //    workerRoleHost.StartWorkerRoles();

        //    Assert.AreEqual(1, workerRoleHost.WorkerRoleCount);
        //}
    }
}
