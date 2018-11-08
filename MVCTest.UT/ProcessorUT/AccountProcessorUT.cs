using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ninject;

using MVCTest.BusinessInterfaces;
using MVCTest.BusinessLayer;
using MVCTest.DataInterfaces;
using MVCTest.DataLayer;
using MVCTest.BusinessTypes;

namespace MVCTest.UT.ProcessorUT
{
    [TestClass]
    public class AccountProcessorUT
    {
        AccountProcessor _processor = default(AccountProcessor);

        [TestInitialize]
        public void Initializer()
        {
            IKernel kernel = new StandardKernel();

            // Processor
            kernel.Bind<IAccountProcessor>().To<AccountProcessor>();

            // Repository
            kernel.Bind<IAccountRepository>().To<AccountRepository>();

            _processor = kernel.Get<AccountProcessor>();
        }

        [TestMethod]
        public void CreateAccount()
        {
            // Arrange
            Account objAccount = new Account()
            {
                AccountName = "Unit Testing",
                NumeroCuenta = "1234567890",
                CorreoElectronico = "unit_testing@rhino.com",
                Telefono = "7908967",
                Fax = "987765434",
                SitioWeb = "www.unittesting.com",
                CuentaPrimaria = new LookUp()
                {
                    Id = new Guid("9343A04D-4C24-E711-8110-5065F38A0A21"),
                    Name = "Cuenta Ejemplo 1"
                },
                ContactoPrincipal = new LookUp()
                {
                    Id = new Guid("5A28F91D-AF13-E711-8105-5065F38B4131"),
                    Name = "Dianna Woodward"
                }
            };

            Guid accountId = Guid.Empty;

            // Act
            accountId = _processor.CreateAccount(objAccount);

            // Assert
            Assert.AreNotEqual(Guid.Empty, accountId);

        }

        [TestMethod]
        public void UpdateAccount()
        {
            // Arrange
            Account objAccount = new Account()
            {
                Id = new Guid("67B81996-B226-E711-810C-5065F38A2B61"), // Eliminado
                AccountName = "Unit Testing Account Test",
                NumeroCuenta = "18293789687",
                CorreoElectronico = "unit_testing_account@rhino.com",
                Telefono = "979868761",
                Fax = "15462467",
                SitioWeb = "www.unittesting2.com",
                CuentaPrimaria = new LookUp()
                {
                    Id = new Guid("2DE8B4FC-B413-E711-8109-5065F38AC921"),
                    Name = "A. Datum Corporation (ejemplo)"
                },
                ContactoPrincipal = new LookUp()
                {
                    Id = new Guid("4A28F91D-AF13-E711-8105-5065F38B4131"),
                    Name = "Lucille Frazier"
                }
            };

            bool response = false;

            // Act
            response = _processor.UpdateAccount(objAccount);

            // Assert
            Assert.IsTrue(response);

        }

        [TestMethod]
        public void DeleteAccount()
        {
            // Arrange
            Guid accountId = new Guid("67B81996-B226-E711-810C-5065F38A2B61");
            bool response = false;

            // Act
            response = _processor.DeleteAccount(accountId);

            // Assert
            Assert.IsTrue(response);

        }

        [TestMethod]
        public void GetAccount()
        {
            // Arrange
            Guid accountId = new Guid("9343A04D-4C24-E711-8110-5065F38A0A21");
            Account objAccount = default(Account);

            // Act
            objAccount = _processor.GetAccount(accountId);

            // Assert
            Assert.IsNotNull(objAccount);
            Assert.AreNotEqual(Guid.Empty, objAccount.Id);
        }

        [TestMethod]
        public void GetAccounts()
        {
            // Arrange
            List<Account> accounts = default(List<Account>);
            string nombreCuenta = "Alpine";

            // Act
            accounts = _processor.GetAccounts(nombreCuenta);

            // Assert
            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Any());
        }
    }
}
