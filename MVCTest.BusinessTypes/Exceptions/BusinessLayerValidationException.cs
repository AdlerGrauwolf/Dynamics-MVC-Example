using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MVCTest.BusinessTypes.Exceptions
{
    [Serializable]
    public class BusinessLayerValidationException : BusinessLayerException
    {
        private static string BusinessLayerValidationExceptionMessage = "Hay varios errores en la validación de datos";

        public new IEnumerable<string> Errors { get; private set; }

        public BusinessLayerValidationException(IEnumerable<string> errors)
            : base(BusinessLayerValidationExceptionMessage)
        {
            Errors = errors;
        }

        public BusinessLayerValidationException(IEnumerable<string> errors, Exception inner)
            : base(BusinessLayerValidationExceptionMessage, inner)
        {
            Errors = errors;
        }

        protected BusinessLayerValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
