using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceFabric.Host.Common.Configuration;
using System.Configuration;

namespace ServiceFabric.Host.Common.Tests
{
    [TestClass]
    public class ConfigurationPropertyBagTests
    {
        [TestMethod]
        public void PopulateConfigurationPropertyBag()
        {
            // Execution
            var configurationPropertyBag = ConfigurationPropertyBagProvider.GetPropertyBag();

            // Assertioms
            Assert.IsNotNull(configurationPropertyBag);
            Assert.AreEqual(ConfigurationManager.AppSettings.Count, configurationPropertyBag.Count);
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                string value = ConfigurationManager.AppSettings[key];
                Assert.AreEqual(value, configurationPropertyBag.GetProperty(key));
            }
        }

        [TestMethod]
        public void GetConfigurationPropertyBagValue()
        {
            // Execution
            var configurationPropertyBag = ConfigurationPropertyBagProvider.GetPropertyBag();

            // Assertioms
            Assert.IsNotNull(configurationPropertyBag);
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                string value = ConfigurationManager.AppSettings[key];
                Assert.AreEqual(value, configurationPropertyBag.GetProperty(key));
            }
        }

        [TestMethod]
        public void GetConfigurationPropertyBagValue_UnknownKeyReturnsNull()
        {
            // Execution
            var configurationPropertyBag = ConfigurationPropertyBagProvider.GetPropertyBag();
            var value = configurationPropertyBag.GetProperty("Unknown"); // Pass an unknown key

            // Assertions
            Assert.IsNotNull(configurationPropertyBag); 
            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetConfigurationPropertyBagValue_EmptyKeyReturnsNull()
        {
            // Execution
            var configurationPropertyBag = ConfigurationPropertyBagProvider.GetPropertyBag();
            var value = configurationPropertyBag.GetProperty(String.Empty); // Pass an empty key

            // Assertions
            Assert.IsNotNull(configurationPropertyBag); 
            Assert.IsNull(value);
        }
    }
}
