using System;
using System.Runtime.Serialization;

namespace MVCTest.BusinessTypes.Exceptions
{
    [Serializable]
    public class BusinessLayerException : Exception
    {
        private static string BusinessLayerExceptionMessage = "Error en lógica de negocios";
        public string Errors { get; private set; }

        public BusinessLayerException(string error)
            : base(BusinessLayerExceptionMessage)
        { Errors = error; }

        public BusinessLayerException(Exception inner)
            : base(BusinessLayerExceptionMessage, inner) { }

        public BusinessLayerException(string ServiceExceptionMessage, Exception inner)
            : base(ServiceExceptionMessage, inner) { }

        protected BusinessLayerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
