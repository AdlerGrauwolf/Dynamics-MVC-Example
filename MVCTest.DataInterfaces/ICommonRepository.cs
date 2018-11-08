using System.Collections.Generic;

using MVCTest.BusinessTypes;

namespace MVCTest.DataInterfaces
{
    public interface ICommonRepository
    {
        List<OptionsetField> GetLocalOptionSet(string optionSetName, string entityName);

        List<OptionsetField> GetGlobalOptionSet(string optionSetName);
    }
}
