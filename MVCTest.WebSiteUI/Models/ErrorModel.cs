namespace MVCTest.WebSiteUI.Models
{
    public class ErrorModel
    {
        #region Enumearaciones
        public enum TipoMensajeEnum
        {
            Informacion,
            Exito,
            Advertencia,
            Error
        }
        #endregion
        public string UrlView { get; set; }

        public string Mensaje { get; set; }

        public TipoMensajeEnum TipoMensaje { get; set; }
    }
}