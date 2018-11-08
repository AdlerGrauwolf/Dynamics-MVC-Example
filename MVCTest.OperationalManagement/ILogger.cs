using System;

namespace MVCTest.OperationalManagement
{
    public interface ILogger
    {
        void Error(Exception ex);

        void Error(string error);

        void Infomacion(string mensaje);
    }
}
