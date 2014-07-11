using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Host.Common.Configuration
{
    public static class ConfigurationPropertyBagProvider
    {
        public static ConfigurationPropertyBag GetPropertyBag()
        {
            return (new ConfigurationPropertyBag());
        }
    }
}
