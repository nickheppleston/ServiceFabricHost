using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Host.Common.Configuration
{
    public class ConfigurationPropertyBag
    {
        private NameValueCollection _propertyNameValueCollection;

        public ConfigurationPropertyBag()
        {
            _propertyNameValueCollection = new NameValueCollection();

            InitializePropertyBag();
        }

        private void InitializePropertyBag()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                _propertyNameValueCollection.Add(key, ConfigurationManager.AppSettings[key]);
            }
        }

        public int Count
        {
            get
            {
                return (_propertyNameValueCollection.Count);
            }
        }

        public string GetProperty(string key)
        {
            return (_propertyNameValueCollection[key]);
        }
    }
}
