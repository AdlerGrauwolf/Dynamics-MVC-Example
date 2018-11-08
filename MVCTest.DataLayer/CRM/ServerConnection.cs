using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace MVCTest.DataLayer.CRM
{
    public class ServerConnection : IDisposable
    {
        #region Propiedades

        public CrmServiceClient CnxCrm { get; set; }
        public IOrganizationService Service { get; set; }
        public OrganizationServiceContext Context { get; set; }

        private bool _disposed = false;

        #endregion

        #region Constructores
        public ServerConnection()
        {
            if (!_disposed)
            {
                string connectionString = GetServiceConfiguration();
                CnxCrm = new CrmServiceClient(connectionString);
                Service = (IOrganizationService)CnxCrm.OrganizationWebProxyClient != null ?
                    (IOrganizationService)CnxCrm.OrganizationWebProxyClient :
                    (IOrganizationService)CnxCrm.OrganizationServiceProxy;
                Context = new OrganizationServiceContext(Service);
            }
        }
        #endregion

        #region Dispose Objetos

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static String GetServiceConfiguration()
        {
            // Get available connection strings from app.config.
            int count = ConfigurationManager.ConnectionStrings.Count;

            // Create a filter list of connection strings so that we have a list of valid
            // connection strings for Microsoft Dynamics CRM only.
            List<KeyValuePair<String, String>> filteredConnectionStrings =
                new List<KeyValuePair<String, String>>();

            for (int a = 0; a < count; a++)
            {
                if (isValidConnectionString(ConfigurationManager.ConnectionStrings[a].ConnectionString))
                    filteredConnectionStrings.Add
                        (new KeyValuePair<string, string>
                            (ConfigurationManager.ConnectionStrings[a].Name,
                            ConfigurationManager.ConnectionStrings[a].ConnectionString));
            }
           
            // If one valid connection string is found, use that.
            if (filteredConnectionStrings.Any())
            {
                return filteredConnectionStrings[0].Value;
            }


            return null;
        }

        private static Boolean isValidConnectionString(String connectionString)
        {
            // At a minimum, a connection string must contain one of these arguments.
            if (connectionString.Contains("Url=") ||
                connectionString.Contains("Server=") ||
                connectionString.Contains("ServiceUri="))
                return true;

            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Context.Dispose();
            }
        }

        #endregion  
    }
}
