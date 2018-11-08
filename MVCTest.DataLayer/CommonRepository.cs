using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

using MVCTest.BusinessTypes;
using MVCTest.DataInterfaces;
using MVCTest.DataLayer.CRM;
using MVCTest.BusinessTypes.Exceptions;

namespace MVCTest.DataLayer
{
    public class CommonRepository : ICommonRepository
    {
        public List<OptionsetField> GetGlobalOptionSet(string optionSetName)
        {
            List<OptionsetField> optionSets = new List<OptionsetField>();
            ServerConnection cnx = new ServerConnection();
            try
            {
                RetrieveOptionSetRequest retrieveOptionSetRequest = new RetrieveOptionSetRequest { Name = optionSetName };

                RetrieveOptionSetResponse retrieveOptionSetResponse =
                    (RetrieveOptionSetResponse)cnx.Context.Execute(retrieveOptionSetRequest);

                OptionSetMetadata opcionesMedaData = (OptionSetMetadata)retrieveOptionSetResponse.OptionSetMetadata;

                opcionesMedaData.Options.ToList().ForEach
                  (l =>
                     optionSets.Add(new OptionsetField()
                     {
                         Label = l.Label.UserLocalizedLabel.Label,
                         Value = Convert.ToInt32(l.Value)
                     })
                  );
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }

            return optionSets;
        }

        public List<OptionsetField> GetLocalOptionSet(string optionSetName, string entityName)
        {
            List<OptionsetField> optionSets = new List<OptionsetField>();
            ServerConnection cnx = new ServerConnection();
            try
            {
                RetrieveEntityRequest retrieveDetails = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.All,
                    LogicalName = entityName
                };

                RetrieveEntityResponse retrieveEntityResponseObj = (RetrieveEntityResponse)cnx.Service.Execute(retrieveDetails);
                EntityMetadata metadata = retrieveEntityResponseObj.EntityMetadata;

                PicklistAttributeMetadata picklistMetadata = metadata.Attributes.FirstOrDefault(attribute => String.Equals
                (attribute.LogicalName, optionSetName, StringComparison.OrdinalIgnoreCase)) as PicklistAttributeMetadata;

                picklistMetadata.OptionSet.Options.ToList().ForEach
                    (l =>
                       optionSets.Add(new OptionsetField()
                       {
                           Label = l.Label.UserLocalizedLabel.Label,
                           Value = Convert.ToInt32(l.Value)
                       })
                    );

            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
            return optionSets;
        }
    }
}
