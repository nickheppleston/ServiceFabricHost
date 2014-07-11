using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Host.Exceptions
{
    public class FailedToGetWorkerRoleInstanceException : Exception
    {
        public FailedToGetWorkerRoleInstanceException(string message)
            : base(message)
        {
        }
    }
}