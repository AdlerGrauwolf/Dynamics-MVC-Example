using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCTest.BusinessTypes
{
    public class Contact : BusinessTypesBase
    {
        [DisplayName("Nombre del contacto")]   
        public string FullName { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        public string FirstName {get; set;}

        [DisplayName("Apellidos")]
        [Required(ErrorMessage = "El campo apellidos es obligatorio")]
        public string LastName { get; set;}

        [DisplayName("Puesto")]
        public string Puesto { get; set; }

        [DisplayName("Correo electrónico")]
        public string CorreoElectronico { get; set; }

        [DisplayName("Teléfono del trabajo")]
        public string TelefonoTrabajo { get; set; }

        [DisplayName("Teléfono celular")]
        public string TelefonoMovil { get; set; }

        [DisplayName("Método de contacto")]
        public int MetodoContacto { get; set; }

        [DisplayName("Cuenta primaria")]
        public LookUp CuentaPrincipal { get; set; }

        [DisplayName("Cuentas asociadas")]
        public List<Account> CuentasAsociadas { get; set; }       
        
    }
}
