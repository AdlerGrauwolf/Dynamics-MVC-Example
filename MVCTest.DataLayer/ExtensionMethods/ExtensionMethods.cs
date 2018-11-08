using System;

using Microsoft.Xrm.Sdk;

namespace MVCTest.DataLayer.ExtensionMethods
{
    public static class ExtensionMethods
    {

        public static Guid GetGuidValue(this Entity target, string nombreCampo)
        {
            if (target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return target.GetAttributeValue<Guid>(nombreCampo);

            return Guid.Empty;
        }

        public static string GetStringValue(this Entity target, string nombreCampo)
        {
            if (target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return target.GetAttributeValue<string>(nombreCampo);

            return string.Empty;
        }

        public static int GetOptionSetValue(this Entity target, string nombreCampo)
        {

            if (target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return target.GetAttributeValue<OptionSetValue>(nombreCampo).Value;

            return default(int);
        }

        public static Guid GetEntityReferenceId(this Entity target, string nombreCampo)
        {

            if (target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return target.GetAttributeValue<EntityReference>(nombreCampo).Id;

            return Guid.Empty;
        }

        public static string GetEntityReferenceName(this Entity target, string nombreCampo)
        {
            if (target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return target.GetAttributeValue<EntityReference>(nombreCampo).Name;

            return string.Empty;
        }

        #region AliasedMethods
        public static string GetAliasedStringValue(this Entity target, string nombreCampo)
        {
            if (target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return (string)(target.GetAttributeValue<AliasedValue>(nombreCampo).Value);

            return string.Empty;
        }
        
        public static Guid GetAliasedGuidValue(this Entity target, string nombreCampo)
        {
            if(target != null && target.Contains(nombreCampo) && target[nombreCampo] != null)
                return (Guid)(target.GetAttributeValue<AliasedValue>(nombreCampo).Value);
           
            return Guid.Empty;
        }

        #endregion
    }
}
