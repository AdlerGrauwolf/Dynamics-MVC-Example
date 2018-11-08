using System;
using System.Collections.Generic;

using MVCTest.BusinessTypes;

namespace MVCTest.BusinessInterfaces
{
    public interface IContactProcessor
    {
        // Obtener la lista de cuentas existentes
        List<Contact> GetShortContacts(string contactName = "");

        // Obtener un contacto
        Contact GetContact(Guid contactId);

        // Crear un contacto nuevo
        Guid CreateContact(Contact contact);

        // Actualizar un contacto existente
        bool UpdateContact(Contact contact);

        // Eliminar un contacto existente
        bool DeleteContact(Guid contactId);

        // Obtener lista de opciones
        List<OptionsetField> GetContactMethodSelector(string optionSetName, string entityName);

    }
}
