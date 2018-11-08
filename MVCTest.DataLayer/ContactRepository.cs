using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using MVCTest.DataInterfaces;
using MVCTest.BusinessTypes;
using MVCTest.DataLayer.CRM;
using MVCTest.DataLayer.ExtensionMethods;
using MVCTest.BusinessTypes.Exceptions;

namespace MVCTest.DataLayer
{
    public class ContactRepository : IContactRepository
    {
        public Contact GetContact(Guid contactId)
        {
            ServerConnection cnx = new ServerConnection();
            try
            {
                Contact contact = new Contact();

                QueryExpression query = new QueryExpression("contact")
                {
                    ColumnSet = new ColumnSet("contactid", "statuscode", "fullname", "firstname", "lastname", "jobtitle", "parentcustomerid", "emailaddress1", "telephone1", "mobilephone", "preferredcontactmethodcode"),
                    NoLock = true,
                    Criteria = new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression("statuscode", ConditionOperator.Equal, (int)ContactStatusCode.Activo),
                            new ConditionExpression("contactid", ConditionOperator.Equal, contactId)
                        }
                    },
                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            Columns = new ColumnSet("accountid", "name", "telephone1", "emailaddress1"),
                            LinkFromEntityName = "contact",
                            LinkFromAttributeName = "contactid",
                            LinkToEntityName = "account",
                            LinkToAttributeName = "primarycontactid",
                            EntityAlias = "Account",
                            LinkCriteria = new FilterExpression()
                            {
                                Conditions =
                                {
                                    new ConditionExpression("statuscode", ConditionOperator.Equal, (int)AccountStatusCode.Activo)
                                }
                            },
                            JoinOperator = JoinOperator.LeftOuter
                        }
                    }

                };

                List<Entity> response = cnx.Service.RetrieveMultiple(query).Entities.ToList();

                if (response != null && response.Any())
                    contact = BuildEntityToContact(response);

                cnx.Dispose();

                return contact;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public List<Contact> GetShortContacts(string contactName = "")
        {
            ServerConnection cnx = new ServerConnection();

            try
            {
                List<Contact> contactos = default(List<Contact>);

                QueryExpression query = new QueryExpression("contact")
                {
                    ColumnSet = new ColumnSet("contactid", "fullname"),
                    NoLock = true
                };

                query.Criteria = new FilterExpression();
                query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (int)ContactStatusCode.Activo);

                if (!string.IsNullOrEmpty(contactName))
                    query.Criteria.AddCondition("fullname", ConditionOperator.BeginsWith, contactName);

                query.AddOrder("fullname", OrderType.Ascending);

                // Limitar la cantidad de resultados
                query.PageInfo.PageNumber = 1;
                query.PageInfo.Count = 100;

                var response = cnx.Service.RetrieveMultiple(query).Entities;

                if (response != null && response.ToList().Any())
                    contactos = BuildListEntityShortContact(response.ToList());

                cnx.Dispose();

                return contactos;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public Guid CreateContact(Contact objContact)
        {
            ServerConnection cnx = new ServerConnection();
            Guid newContactId = Guid.Empty;

            try
            {
                newContactId = cnx.Service.Create(BuildContactToEntityCreate(objContact));
                cnx.Dispose();
                return newContactId;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public bool DeleteContact(Guid contactId)
        {
            ServerConnection cnx = new ServerConnection();

            try
            {
                cnx.Service.Delete("contact", contactId);            
                cnx.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        public bool UpdateContact(Contact objContact)
        {
            ServerConnection cnx = new ServerConnection();

            try
            {
                cnx.Service.Update(BuildContactToEntityUpdate(objContact));
                cnx.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                cnx = null;
                throw new CrmDataException(ex);
            }
        }

        #region Metodos privados    

        #region Object Contact Helpers

        private Contact BuildEntityToContact(List<Entity> entities)
        {
            return new Contact()
            {

                Id = entities[0].Id,
                FullName = entities[0].GetStringValue("fullname"),
                FirstName = entities[0].GetStringValue("firstname"),
                LastName = entities[0].GetStringValue("lastname"),
                Puesto = entities[0].GetStringValue("jobtitle"),
                CorreoElectronico = entities[0].GetStringValue("emailaddress1"),
                TelefonoTrabajo = entities[0].GetStringValue("telephone1"),
                TelefonoMovil = entities[0].GetStringValue("mobilephone"),
                MetodoContacto = entities[0].GetOptionSetValue("preferredcontactmethodcode"),
                CuentaPrincipal = new LookUp()
                {
                    Id = entities[0].GetEntityReferenceId("parentcustomerid"),
                    Name = entities[0].GetEntityReferenceName("parentcustomerid")
                },
                CuentasAsociadas = LinkedEntityToAccountList(entities)
            };
        }

        private List<Account> LinkedEntityToAccountList(List<Entity> entities)
        {
            List<Account> listAccounts = new List<Account>();
            entities.ForEach(e => listAccounts.Add(BuildShortAccount(e)));
            return listAccounts;
        }

        private Account BuildShortAccount(Entity entity)
        {
            return new Account()
            {
                Id = entity.GetAliasedGuidValue("Account.accountid"),
                AccountName = entity.GetAliasedStringValue("Account.name"),
                Telefono = entity.GetAliasedStringValue("Account.telephone1"),
                CorreoElectronico = entity.GetAliasedStringValue("Account.emailaddress1")
            };
        }

        #endregion

        #region List Short Contact Helpers

        private List<Contact> BuildListEntityShortContact(List<Entity> entities)
        {
            List<Contact> listContactos = new List<Contact>();
            entities.ForEach(c => listContactos.Add(BuildEntityToShortContact(c)));
            return listContactos;
        }

        private Contact BuildEntityToShortContact(Entity entities)
        {
            return new Contact()
            {
                Id = entities.GetGuidValue("contactid"),
                FullName = entities.GetStringValue("fullname")
            };
        }

        #endregion

        private Entity BuildContactToEntityUpdate(Contact objContact)
        {
            Entity entityContact = new Entity("contact");

            entityContact.Id = objContact.Id;
            entityContact["firstname"] = objContact.FirstName;
            entityContact["lastname"] = objContact.LastName;
            entityContact["jobtitle"] = objContact.Puesto;
            entityContact["emailaddress1"] = objContact.CorreoElectronico;
            entityContact["telephone1"] = objContact.TelefonoTrabajo;
            entityContact["mobilephone"] = objContact.TelefonoMovil;

            // Si el metodo de contacto que es optionset no viene dejar el valor que tenga
            if (objContact.MetodoContacto != default(int))
                entityContact["preferredcontactmethodcode"] = new OptionSetValue(objContact.MetodoContacto);

            // Validar si viene la cuenta principal si no dejarla en null
            if (objContact.CuentaPrincipal != null && (objContact.CuentaPrincipal.Id != null && objContact.CuentaPrincipal.Id != Guid.Empty))
                entityContact["parentcustomerid"] = new EntityReference("account", objContact.CuentaPrincipal.Id);
            else
                entityContact["parentcustomerid"] = null;

            return entityContact;
        }

        private Entity BuildContactToEntityCreate(Contact objContact)
        {

            Entity entityContact = new Entity("contact");

            if (!string.IsNullOrEmpty(objContact.FirstName))
                entityContact["firstname"] = objContact.FirstName;

            if (!string.IsNullOrEmpty(objContact.LastName))
                entityContact["lastname"] = objContact.LastName;

            if (!string.IsNullOrEmpty(objContact.Puesto))
                entityContact["jobtitle"] = objContact.Puesto;

            if (!string.IsNullOrEmpty(objContact.CorreoElectronico))
                entityContact["emailaddress1"] = objContact.CorreoElectronico;

            if (!string.IsNullOrEmpty(objContact.TelefonoTrabajo))
                entityContact["telephone1"] = objContact.TelefonoTrabajo;

            if (!string.IsNullOrEmpty(objContact.TelefonoMovil))
                entityContact["mobilephone"] = objContact.TelefonoMovil;

            if (objContact.MetodoContacto != default(int))
                entityContact["preferredcontactmethodcode"] = new OptionSetValue(objContact.MetodoContacto);

            if (objContact.CuentaPrincipal != null && (objContact.CuentaPrincipal.Id != null && objContact.CuentaPrincipal.Id != Guid.Empty))
                entityContact["parentcustomerid"] = new EntityReference("account", objContact.CuentaPrincipal.Id);

            return entityContact;
        }

        #endregion
    }
}
