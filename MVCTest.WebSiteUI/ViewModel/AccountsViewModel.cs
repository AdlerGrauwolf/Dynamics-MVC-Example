using System.Collections.Generic;

using MVCTest.WebSiteUI.Models;

namespace MVCTest.WebSiteUI.ViewModel
{
    public class AccountsViewModel
    {
        public AccountsViewModel()
        {
            Informacion = new ErrorModel()
            { TipoMensaje = ErrorModel.TipoMensajeEnum.Exito };
        }

        public List<AccountModel> Accounts { get; set; }

        public ErrorModel Informacion { get; set; }
    }
}