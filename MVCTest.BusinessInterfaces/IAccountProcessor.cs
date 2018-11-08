using System;
using System.Collections.Generic;

using MVCTest.BusinessTypes;

namespace MVCTest.BusinessInterfaces
{
    public interface IAccountProcessor
    {
        // Obtener una lista de cuentas existentes
        List<Account> GetAccounts(string accountName = "");

        // Obtener una única cuenta
        Account GetAccount(Guid accountId);

        // Crear una nueva cuenta
        Guid CreateAccount(Account account);

        // Actualizar cuenta existente
        bool UpdateAccount(Account account);

        // Eliminar cuenta existente
        bool DeleteAccount(Guid accountId);
    }
}
