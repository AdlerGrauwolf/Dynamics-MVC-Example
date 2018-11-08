using System;
using System.Text;

namespace MVCTest.OperationalManagement.Extensions
{
    public static class ExceptionExtenions
    {
        public static string Build(this Exception target)
        {
            var message = new StringBuilder();
            while (target != null)
            {
                message.AppendLine(target.Message);
                target = target.InnerException;
            }
            return message.ToString();
        }
    }
}
