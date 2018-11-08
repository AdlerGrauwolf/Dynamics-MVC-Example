using System;
using System.Collections.Generic;

using MVCTest.BusinessTypes;

namespace MVCTest.DataInterfaces
{
    public interface IAccountRepository
    {
        // Obtener una lista de cuentas existentes
        List<Account> GetAccounts(string accountName = "");

        // Obtener una única cuenta
        Account GetAccount(Guid Id);

        // Crear una nueva cuenta
        Guid CreateAccount(Account account);

        // Actualizar cuenta existente
        bool UpdateAccount(Account account);

        // Eliminar cuenta existente
        bool DeleteAccount(Guid accountId);       
    }
}
