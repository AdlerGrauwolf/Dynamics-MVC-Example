using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MVCTest.WebSiteUI.Models;

namespace MVCTest.WebSiteUI.ViewModel
{
    public class AccountCUViewModel : AccountModel
    {
        public AccountCUViewModel()
        {
            Informacion = new ErrorModel()
            { TipoMensaje = ErrorModel.TipoMensajeEnum.Exito };
        }

        public ErrorModel Informacion { get; set; }

        public string Accion { get; set; }
    }
}