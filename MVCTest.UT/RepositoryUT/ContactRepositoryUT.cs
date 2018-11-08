using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MVCTest.BusinessTypes;
using MVCTest.DataInterfaces;
using MVCTest.DataLayer;

namespace MVCTest.UT
{
    [TestClass]
    public class ContactRepositoryUT
    {
        IContactRepository _repository;

        [TestInitialize]
        public void Inicializar()
        {
            _repository = new ContactRepository();
        }

        [TestMethod]
        public void GetContact()
        {
            // Arrange
            Guid contactId = new Guid("AAFC7154-1725-E711-810E-5065F38A4BA1");

            // Act
            Contact objContacto = _repository.GetContact(contactId);

            // Assert
            Assert.IsNotNull(objContacto);
            Assert.IsTrue(objContacto.Id != Guid.Empty);
        }

        [TestMethod]
        public void GetShortContactList()
        {
            // Arrange
            List<Contact> listaContactos = default(List<Contact>);
            string contactName = "Faith";

            // Act
            listaContactos = _repository.GetShortContacts(contactName);

            // Assert
            Assert.IsNotNull(listaContactos);
            Assert.IsTrue(listaContactos.Any());
        }

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
            newContactId = _repository.CreateContact(newContact);

            // Assert           
            Assert.IsTrue(newContactId != Guid.Empty);
        }

        [TestMethod]
        public void UpdateContact()
        {
            // Arrange        
            Contact updatedContact = new Contact()
            {
                Id = new Guid("528D3D8E-5225-E711-810D-5065F38A7BC1"),
                FirstName = "Pruebas",
                LastName = "Unitarias",
                Puesto = "Consultor Trainee",
                CorreoElectronico = "pruebas_unitarias@rhino.com",
                TelefonoTrabajo = "7897656",
                TelefonoMovil = "4778765463",
                MetodoContacto = 4,
                CuentaPrincipal = new LookUp()
                {
                    Id = new Guid("475B158C-541C-E511-80D3-3863BB347BA8"),
                    Name = "A. Datum"
                }
            };

            bool respuesta = false;

            // Act
            respuesta = _repository.UpdateContact(updatedContact);

            // Assert
            Assert.IsTrue(respuesta);
        }

        [TestMethod]
        public void DeleteContact()
        {
            // Arrange
            Guid contactId = new Guid("f61f902c-b526-e711-8110-5065f38a4ba1");
            bool respuesta = false;

            // Act
            respuesta = _repository.DeleteContact(contactId);

            // Assert
            Assert.IsTrue(respuesta);

        }

    }
}
