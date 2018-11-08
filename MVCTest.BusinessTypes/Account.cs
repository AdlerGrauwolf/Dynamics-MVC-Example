using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCTest.BusinessTypes
{
    public class Account : BusinessTypesBase
    {
        [DisplayName("Nombre de la cuenta")]
        [Required(ErrorMessage = "Favor de indicar el nombre de la cuenta")]
        public string AccountName { get; set; }

        [DisplayName("Número de la cuenta")]
        public string NumeroCuenta { get; set; }

        [DisplayName("Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [DisplayName("Fax")]
        public string Fax { get; set; }

        [DisplayName("Página web")]
        public string SitioWeb { get; set; }

        [DisplayName("Cuenta principal")]
        public LookUp CuentaPrimaria { get; set; }

        [DisplayName("Contacto principal")]
        public LookUp ContactoPrincipal { get; set; }
    }
}
