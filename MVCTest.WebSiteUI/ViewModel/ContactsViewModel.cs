using System.Collections.Generic;
using MVCTest.WebSiteUI.Models;

namespace MVCTest.WebSiteUI.ViewModel
{
    public class ContactsViewModel
    {
        public ContactsViewModel()
        {
            Informacion = new ErrorModel()
            { TipoMensaje = ErrorModel.TipoMensajeEnum.Exito };
        }

        public List<ContactModel> Contacts { get; set; }

        public ErrorModel Informacion { get; set; }
    }
}