using System.Collections.Generic;

using MVCTest.WebSiteUI.Models;
using MVCTest.BusinessTypes;

namespace MVCTest.WebSiteUI.ViewModel
{
    public class ContactCUViewModel : ContactModel
    {
        public ContactCUViewModel()
        {
            Informacion = new ErrorModel()
            { TipoMensaje = ErrorModel.TipoMensajeEnum.Exito };
        }

        public ErrorModel Informacion { get; set; }

        public List<OptionsetField> contactMethodSelector { get; set; }

        public string Accion { get; set; }
    }
}