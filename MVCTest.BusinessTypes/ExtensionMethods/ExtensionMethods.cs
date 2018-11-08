using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using MVCTest.BusinessTypes.Exceptions;

namespace MVCTest.BusinessTypes.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static string RemoveAccents(this string value)
        {
            if (value == null || value == string.Empty)
                return string.Empty;

            value = value.TrimStart().TrimEnd();

            string cleanValue = value.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            cleanValue = cleanValue.Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");

            return cleanValue;
        }
        
        public static void Validate(this BusinessTypesBase target)
        {
            var context = new ValidationContext(target);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(target, context, results, true) == false)
            {
                var errors = results.Select(r => r.ErrorMessage);
                throw new BusinessLayerValidationException(errors);
            }
        }                      
    }
}