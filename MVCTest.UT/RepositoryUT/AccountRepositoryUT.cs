using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MVCTest.DataInterfaces;
using MVCTest.DataLayer;
using MVCTest.BusinessTypes;

namespace MVCTest.UT.RepositoryUT
{
    [TestClass]
    public class AccountRepositoryUT
    {
        IAccountRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new AccountRepository();
        }

        [TestMethod]       
        public void GetAccount()
        {
            // Arrange
            Guid accountId = new Guid("010273F3-AF13-E711-8105-5065F38B4131");
            Account objAccount = new Account();

            // Act
            objAccount = _repository.GetAccount(accountId);

            // Assert
            Assert.IsNotNull(objAccount);
            Assert.IsTrue(objAccount.Id != Guid.Empty);
        }

        [TestMethod]
        public void GetAccountList()
        {
            // Arrange
            List<Account> listAccounts = default(List<Account>);
            string accountName = "alpine";

            // Act
            listAccounts = _repository.GetAccounts(accountName);

            // Assert
            Assert.IsNotNull(listAccounts);
            Assert.IsTrue(listAccounts.Any());
            Assert.IsTrue(listAccounts[0].AccountName.Contains(accountName));
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

            Guid newAccountId = Guid.Empty;

            // Act
            newAccountId = _repository.CreateAccount(objAccount);

            // Assert
            Assert.AreNotEqual(Guid.Empty, newAccountId);
        }

        [TestMethod]
        public void UpdateAccount()
        {
            // Arrange 
            Account objAccount = new Account()
            {
                Id = new Guid("1D47BCF4-DF25-E711-810D-5065F38A7BC1"),
                AccountName = "Unit Testing Updated",
                NumeroCuenta = "9876543210",
                CorreoElectronico = "unit_testing_updated@rhino.com",
                Telefono = "7654321",
                Fax = "78654234",
                SitioWeb = "www.unittesting_updated.com",
                CuentaPrimaria = new LookUp()
                {
                    Id = new Guid("F30173F3-AF13-E711-8105-5065F38B4131"),
                    Name = "Alpine Ski Shop"
                },
                ContactoPrincipal = new LookUp()
                {
                    Id = new Guid("9428F91D-AF13-E711-8105-5065F38B4131"),
                    Name = "Kellie Leblanc"
                }
            };

            var response = false;

            // Act
            response = _repository.UpdateAccount(objAccount);

            // Assert
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void DeleteAccount()
        {
            // Arrange 
            Guid accountId = new Guid("1D47BCF4-DF25-E711-810D-5065F38A7BC1");
            bool response = false;

            // Act
            response = _repository.DeleteAccount(accountId);

            // Assert
            Assert.IsTrue(response);
        }
    }
}
