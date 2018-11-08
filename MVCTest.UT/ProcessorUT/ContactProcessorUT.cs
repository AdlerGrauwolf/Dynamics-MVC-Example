using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MVCTest.BusinessInterfaces;
using MVCTest.BusinessLayer;
using MVCTest.BusinessTypes;
using MVCTest.DataInterfaces;
using MVCTest.DataLayer;
using MVCTest.BusinessTypes.Exceptions;

using Ninject;

namespace MVCTest.UT.ProcessorUT
{
    [TestClass]
    public class ContactProcessorUT
    {
        ContactProcessor _processor = default(ContactProcessor);

        #region Initializer

        [TestInitialize]
        public void Initialize()
        {
            IKernel kernel = new StandardKernel();

            // Processors
            kernel.Bind<IContactProcessor>().To<ContactProcessor>();

            // Repository
            kernel.Bind<IContactRepository>().To<ContactRepository>();
            kernel.Bind<ICommonRepository>().To<CommonRepository>();

            _processor = kernel.Get<ContactProcessor>();
        }

        #endregion

        #region Pruebas Unitarias

        [TestMethod]
        public void CreateContact()
        {
            // Arrange 
            Contact newContact = new Contact()
            {
                FirstName = "Unit",
                LastName = "Testing",
                Puesto = "Becario",
                CorreoElectronico = "unit_testing@rhino.com",
                TelefonoTrabajo = "1234567890",
                TelefonoMovil = "0987654321",
                MetodoContacto = 3,
                CuentaPrincipal = new LookUp()
                {
                    Id = new Guid("23E8B4FC-B413-E711-8109-5065F38AC921"),
                    Name = "Fabrikam SA (ejemplo)"
                }
            };

            Guid newContactId = Guid.Empty;

            // Act
            newContactId = _processor.CreateContact(newContact);

            // Assert
            Assert.AreNotEqual(Guid.Empty, newContactId);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLayerValidationException))]
        public void CreateContact_Falied()
        {
            // Arrange 
            Contact newContact = new Contact()
            {
                Puesto = "Becario",
                CorreoElectronico = "unit_testing@rhino.com",
                TelefonoTrabajo = "1234567890",
                TelefonoMovil = "0987654321",
                MetodoContacto = 3,
                CuentaPrincipal = new LookUp()
                {
                    Id = new Guid("23E8B4FC-B413-E711-8109-5065F38AC921"),
                    Name = "Fabrikam SA (ejemplo)"
                }
            };

            Guid newContactId = Guid.Empty;

            try
            {
                // Act
                newContactId = _processor.CreateContact(newContact);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual(ex.GetType(), typeof(BusinessLayerValidationException));
                var validationException = (BusinessLayerValidationException)ex;

                Assert.IsTrue(ex.Message.Contains("Error en lógica de negocios"));
                Assert.IsTrue(validationException.Errors.Any());
                Assert.IsTrue(validationException.Errors.ToList()[0].Contains("obligatorio"));
                throw ex;
            }
        }

        [TestMethod]
        public void UpdateContact()
        {
            // Arrange
            Contact objContact = new Contact()
            {
                Id = new Guid("8754F160-1C26-E711-810E-5065F38A4BA1"), // Eliminado
                FirstName = "Unit",
                LastName = "Testing Updated",
                Puesto = "Tester",
                CorreoElectronico = "unit_testing_updated@rhino.com",
                TelefonoTrabajo = "8126387127",
                TelefonoMovil = "861927868",
                MetodoContacto = 1,
                CuentaPrincipal = new LookUp()
                {
                    Id = new Guid("F10173F3-AF13-E711-8105-5065F38B4131"),
                    Name = "Active Transport Inc."
                }
            };

            bool response = false;

            // Act
            response = _processor.UpdateContact(objContact);

            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateContact_Failed()
        {
            // Arrange
            Contact objContact = default(Contact);

            bool response = false;

            try
            {
                // Act
                response = _processor.UpdateContact(objContact);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("El contacto contiene información nula"));
                // Assert
                throw ex;
            }            
        }

        [TestMethod]
        public void DeleteContact()
        {
            // Arrange
            Guid contactId = new Guid("8754F160-1C26-E711-810E-5065F38A4BA1");
            bool response = false;

            //Act
            response = _processor.DeleteContact(contactId);

            //Assert
            Assert.IsTrue(response);

        }

        [TestMethod]
        public void GetContact()
        {
            // Assert 
            Guid contactId = new Guid("AAFC7154-1725-E711-810E-5065F38A4BA1");
            Contact objContact = default(Contact);

            // Act
            objContact = _processor.GetContact(contactId);

            // Arrange
            Assert.IsNotNull(objContact);
            Assert.AreNotEqual(Guid.Empty, objContact.Id);
        }

        [TestMethod]
        public void GetShortContacts()
        {
            // Arrange 
            List<Contact> contacts = default(List<Contact>);
            string nombreContact = "faith";

            // Act
            contacts = _processor.GetShortContacts(nombreContact);

            // Assert
            Assert.IsNotNull(contacts);
            Assert.IsTrue(contacts.Any());
        }

        [TestMethod]
        public void GetContactMethodSelector()
        {
            // Arrange
            string optionSetName = "preferredcontactmethodcode";
            string entityName = "contact";
            List<OptionsetField> contactMethodSelector = default(List<OptionsetField>);

            // Act
            contactMethodSelector = _processor.GetContactMethodSelector(optionSetName, entityName);

            // Assert
            Assert.IsNotNull(contactMethodSelector);
            Assert.IsTrue(contactMethodSelector.Any());

        }           
        #endregion
    }
}
