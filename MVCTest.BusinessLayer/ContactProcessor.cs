using System;
using System.Linq;
using System.Collections.Generic;

using MVCTest.BusinessTypes;
using MVCTest.DataInterfaces;
using MVCTest.BusinessInterfaces;
using MVCTest.BusinessTypes.ExtensionMethods;

namespace MVCTest.BusinessLayer
{
    public class ContactProcessor : IContactProcessor
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICommonRepository _commonRepository;

        public ContactProcessor(IContactRepository contactRepository, ICommonRepository commonRepository)
        {
            _contactRepository = contactRepository;
            _commonRepository = commonRepository;
        }

        public Guid CreateContact(Contact contact)
        {
            if (contact == null)
                throw new Exception("El contacto contiene información nula");

            Guid newContact = Guid.Empty;

            contact.Validate();

            newContact = _contactRepository.CreateContact(contact);

            return newContact;
        }

        public bool UpdateContact(Contact contact)
        {
            if (contact == null)
                throw new Exception("El contacto contiene información nula");

            if (contact.Id == Guid.Empty)
                throw new Exception("El Id del contacto está vacio.");

            bool response = false;

            contact.Validate();

            response = _contactRepository.UpdateContact(contact);

            return response;
        }

        public bool DeleteContact(Guid contactId)
        {
            bool respuesta = false;

            if (contactId != Guid.Empty && contactId != null)
                respuesta = _contactRepository.DeleteContact(contactId);

            return respuesta;
        }

        public Contact GetContact(Guid contactId)
        {
            Contact contact = new Contact();

            if (contactId != Guid.Empty && contactId != null)
                contact = _contactRepository.GetContact(contactId);

            return contact;
        }

        public List<Contact> GetShortContacts(string contactName = "")
        {
            List<Contact> contacts = _contactRepository.GetShortContacts(contactName.RemoveAccents());
            if (contacts != null && contacts.Any())
                return contacts;

            return new List<Contact>();
        }

        public List<OptionsetField> GetContactMethodSelector(string optionSetName, string entityName)
        {
            List<OptionsetField> selector = new List<OptionsetField>();

            if (!string.IsNullOrWhiteSpace(optionSetName) && !string.IsNullOrWhiteSpace(entityName))
                selector = _commonRepository.GetLocalOptionSet(optionSetName, entityName);

            if (selector != null && selector.Any())
                return selector;

            return new List<OptionsetField>();
        }
    }
}
