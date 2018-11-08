using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using MVCTest.BusinessTypes;
using MVCTest.DataInterfaces;
using MVCTest.DataLayer.ExtensionMethods;
using MVCTest.BusinessTypes.Exceptions;
using MVCTest.DataLayer.CRM;

namespace MVCTest.DataLayer
{
    public class AccountRepository : IAccountRepository
    {

        public Account GetAccount(Guid accountId)
        {
            ServerConnection cnx = new ServerConnection();
            try
            {
                Account objCuenta = new Account();
                QueryExpression query = new QueryExpression("account")
                {
                    ColumnSet = new ColumnSet("accountid", "name", "accountnumber", "emailaddress1", "telephone1", "fax", "websiteurl", "parentaccountid", "primarycontactid"),
                    NoLock = true,
                    Criteria = new FilterExpression()
                    {
                        Conditions =
                    {
                        new ConditionExpression("accountid", ConditionOperator.Equal, accountId),
                        new ConditionExpression("statuscode", ConditionOperator.Equal, (int)AccountStatusCode.Activo)
                    }
                    }
                };

                Entity response = cnx.Service.RetrieveMultiple(query).Entities.FirstOrDefault();

                cnx.Dispose();

                if (response != null)
                    objCuenta = BuildEntityToAccount(response);

                return objCuenta;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }

        }

        public List<Account> GetAccounts(string accountName = "")
        {
            ServerConnection cnx = new ServerConnection();
            try
            {
                List<Account> listCuentas = default(List<Account>);
                QueryExpression query = new QueryExpression("account")
                {
                    ColumnSet = new ColumnSet("accountid", "name"),
                    NoLock = true,
                    Criteria = new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression("statuscode", ConditionOperator.Equal, (int)AccountStatusCode.Activo)
                        }
                    }
                };

                if (!string.IsNullOrEmpty(accountName))
                    query.Criteria.AddCondition("name", ConditionOperator.BeginsWith, accountName);

                query.AddOrder("name", OrderType.Ascending);

                //limitar la cantidad de resultados
                query.PageInfo.PageNumber = 1;
                query.PageInfo.Count = 100;

                var response = cnx.Service.RetrieveMultiple(query).Entities;
                cnx.Dispose();

                if (response != null && response.ToList().Any())
                    listCuentas = BuildEntityToAccountList(response.ToList());

                return listCuentas;

            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public Guid CreateAccount(Account account)
        {
            ServerConnection cnx = new ServerConnection();
            Guid newAccountId = Guid.Empty;
            try
            {
                newAccountId = cnx.Service.Create(BuildAccountToEntityCreate(account));
                cnx.Dispose();
                return newAccountId;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public bool DeleteAccount(Guid accountId)
        {
            ServerConnection cnx = new ServerConnection();
            try
            {
                cnx.Service.Delete("account", accountId);
                cnx.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public bool UpdateAccount(Account account)
        {
            ServerConnection cnx = new ServerConnection();
            try
            {
                cnx.Service.Update(BuildAccountToEntityUpdate(account));
                cnx.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        #region private methods
        private List<Account> BuildEntityToAccountList(List<Entity> accountEntities)
        {
            List<Account> listaCuentas = new List<Account>();
            accountEntities.ForEach(a => listaCuentas.Add(BuilEntityToShortAccount(a)));
            return listaCuentas;
        }

        private Account BuilEntityToShortAccount(Entity accountEntity)
        {
            return new Account()
            {
                Id = accountEntity.Id,
                AccountName = accountEntity.GetStringValue("name")
            };
        }

        private Account BuildEntityToAccount(Entity accountEntity)
        {
            return new Account()
            {
                Id = accountEntity.Id,
                AccountName = accountEntity.GetStringValue("name"),
                NumeroCuenta = accountEntity.GetStringValue("accountnumber"),
                CorreoElectronico = accountEntity.GetStringValue("emailaddress1"),
                Telefono = accountEntity.GetStringValue("telephone1"),
                Fax = accountEntity.GetStringValue("fax"),
                SitioWeb = accountEntity.GetStringValue("websiteurl"),
                CuentaPrimaria = new LookUp()
                {
                    Id = accountEntity.GetEntityReferenceId("parentaccountid"),
                    Name = accountEntity.GetEntityReferenceName("parentaccountid")
                },

                ContactoPrincipal = new LookUp()
                {
                    Id = accountEntity.GetEntityReferenceId("primarycontactid"),
                    Name = accountEntity.GetEntityReferenceName("primarycontactid")
                }

            };
        }

        private Entity BuildAccountToEntityCreate(Account objAccount)
        {
            Entity entityAccount = new Entity("account");

            if (!string.IsNullOrEmpty(objAccount.AccountName))
                entityAccount["name"] = objAccount.AccountName;

            if (!string.IsNullOrEmpty(objAccount.NumeroCuenta))
                entityAccount["accountnumber"] = objAccount.NumeroCuenta;

            if (!string.IsNullOrEmpty(objAccount.CorreoElectronico))
                entityAccount["emailaddress1"] = objAccount.CorreoElectronico;

            if (!string.IsNullOrEmpty(objAccount.Telefono))
                entityAccount["telephone1"] = objAccount.Telefono;

            if (!string.IsNullOrEmpty(objAccount.Fax))
                entityAccount["fax"] = objAccount.Fax;

            if (!string.IsNullOrEmpty(objAccount.SitioWeb))
                entityAccount["websiteurl"] = objAccount.SitioWeb;

            if (objAccount.CuentaPrimaria != null && (objAccount.CuentaPrimaria.Id != null && objAccount.CuentaPrimaria.Id != Guid.Empty))
                entityAccount["parentaccountid"] = new EntityReference("account", objAccount.CuentaPrimaria.Id);

            if (objAccount.ContactoPrincipal != null && (objAccount.ContactoPrincipal.Id != null && objAccount.ContactoPrincipal.Id != Guid.Empty))
                entityAccount["primarycontactid"] = new EntityReference("contact", objAccount.ContactoPrincipal.Id);

            return entityAccount;
        }

        private Entity BuildAccountToEntityUpdate(Account objAccount)
        {
            Entity entityAccount = new Entity("account");

            entityAccount.Id = objAccount.Id;
            entityAccount["name"] = objAccount.AccountName;
            entityAccount["accountnumber"] = objAccount.NumeroCuenta;
            entityAccount["emailaddress1"] = objAccount.CorreoElectronico;
            entityAccount["telephone1"] = objAccount.Telefono;
            entityAccount["fax"] = objAccount.Fax;
            entityAccount["websiteurl"] = objAccount.SitioWeb;

            if (objAccount.CuentaPrimaria != null && (objAccount.CuentaPrimaria.Id != null && objAccount.CuentaPrimaria.Id != Guid.Empty))
                entityAccount["parentaccountid"] = new EntityReference("account", objAccount.CuentaPrimaria.Id);
            else
                entityAccount["parentaccountid"] = null;

            if (objAccount.ContactoPrincipal != null && (objAccount.ContactoPrincipal.Id != null && objAccount.ContactoPrincipal.Id != Guid.Empty))
                entityAccount["primarycontactid"] = new EntityReference("contact", objAccount.ContactoPrincipal.Id);
            else
                entityAccount["primarycontactid"] = null;

            return entityAccount;
        }

        #endregion
    }
}
