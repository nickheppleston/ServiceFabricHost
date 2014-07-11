using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Host.Exceptions
{
    public class FailedToStopHostException : Exception
    {
        public FailedToStopHostException(string message)
            : base(message)
        {
        }
    }
}