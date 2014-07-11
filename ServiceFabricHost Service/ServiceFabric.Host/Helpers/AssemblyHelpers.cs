using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ServiceFabric.Host.Common;
using ServiceFabric.Host.Exceptions;
using NLog;
using System.Reflection;

namespace ServiceFabric.Host.Helpers
{
    public static class AssemblyHelpers
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static IBaseWorkerRole GetWorkerRoleInstance(string assemblyPath, string typeName)
        {
            logger.Debug("GetWorkerRoleInstance() - Entered method (Assembly Path: '{0}' / Type Name: '{1}'", assemblyPath, typeName);

            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
                var assembly = AppDomain.CurrentDomain.Load(assemblyName);
                var workerRoleInstance = assembly.CreateInstance(typeName) as IBaseWorkerRole;

                if (workerRoleInstance == null)
                    throw new FailedToGetWorkerRoleInstanceException(String.Format("Failed to instanciate Worker Role Instance '{0}' from the Assembly '{1}'. Following instanciation, the instance was NULL.", typeName, assemblyPath));

                return (workerRoleInstance);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                throw new FailedToGetWorkerRoleInstanceException(String.Format("Failed to instanciate Worker Role Instance '{0}' from the Assembly '{1}'. The Assembly could not be found. Error: {2}", typeName, assemblyPath, fileNotFoundException.Message));
            }
            catch (TypeLoadException typeLoadException)
            {
                throw new FailedToGetWorkerRoleInstanceException(String.Format("Failed to instanciate Worker Role Instance '{0}' from the Assembly '{1}'. Error: {2}", typeName, assemblyPath, typeLoadException.Message));
            }
            catch (MissingMethodException missingMethodException)
            {
                throw new FailedToGetWorkerRoleInstanceException(String.Format("Failed to instanciate Worker Role Instance '{0}' from the Assembly '{1}'. Error: {2}", typeName, assemblyPath, missingMethodException.Message));
            }
            catch (Exception exception)
            {
                throw new FailedToGetWorkerRoleInstanceException(String.Format("Failed to instanciate Worker Role Instance '{0}' from the Assembly '{1}'. Error: {2}", typeName, assemblyPath, exception.Message));
            }
        }
    }
}
