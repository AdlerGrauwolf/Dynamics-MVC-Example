using System;
using System.Collections.Generic;

using MVCTest.BusinessTypes;

namespace MVCTest.DataInterfaces
{
    public interface IContactRepository
    {
        // Crear un contacto nuevo
        Guid CreateContact(Contact contact);

        // Actualizar un contacto existente
        bool UpdateContact(Contact contact);

        // Eliminar un contacto existente
        bool DeleteContact(Guid contactId);

        // Obtener lista de contactos
        List<Contact> GetShortContacts(string contactName = "");

        // Obtener un único contacto
        Contact GetContact(Guid contactId);
    }
}
