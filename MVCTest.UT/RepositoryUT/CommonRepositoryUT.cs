using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCTest.DataInterfaces;
using MVCTest.DataLayer;
using MVCTest.BusinessTypes;

namespace MVCTest.UT.RepositoryUT
{
    [TestClass]
    public class CommonRepositoryUT
    {
        ICommonRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new CommonRepository();
        }

        [TestMethod]
        public void GetLocalOptionSet()
        {
            // Arrange
            string optionSetName = "preferredcontactmethodcode";
            string entityName = "contact";
            List<OptionsetField> selector = default(List<OptionsetField>);

            // Act
            selector = _repository.GetLocalOptionSet(optionSetName, entityName);

            // Assert
            Assert.IsNotNull(selector);
            Assert.IsTrue(selector.Any());
        }
    }
}
