using System;
using System.Runtime.Serialization;

namespace MVCTest.BusinessTypes.Exceptions
{
    [Serializable]
    public class CrmDataException : Exception
    {
        private static string CrmMessage = "Ocurrió un error de acceso a datos en CRM.";
        public string Errors { get; set; }
        public CrmDataException(string error) : base(CrmMessage) { Errors = error; }
        public CrmDataException(Exception inner) : base(CrmMessage, inner) { }
        public CrmDataException(string message, Exception inner) : base(CrmMessage, inner) { }
        protected CrmDataException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
