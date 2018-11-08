using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCTest.BusinessInterfaces;
using MVCTest.OperationalManagement;
using MVCTest.BusinessTypes.Exceptions;
using MVCTest.BusinessTypes;
using MVCTest.WebSiteUI.Models;
using MVCTest.WebSiteUI.ViewModel;
using static MVCTest.WebSiteUI.Models.ErrorModel;

namespace MVCTest.WebSiteUI.Controllers
{
    public class AccountController : Controller
    {
        #region Inicializar variables

        private const string _messageException = "Ocurrió un error en la aplicación";
        private readonly IAccountProcessor _accountProcessor;
        private readonly ILogger _logger;

        AccountsViewModel _accountsViewModel = default(AccountsViewModel);

        public AccountController(IAccountProcessor accountProcessor, ILogger logger)
        {
            _accountProcessor = accountProcessor;
            _logger = logger;
        }

        #endregion 

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewAccounts()
        {
            _accountsViewModel = new AccountsViewModel();
            try
            {
                List<Account> accounts = _accountProcessor.GetAccounts();
                List<AccountModel> accountsModel = new List<AccountModel>();

                if (accounts != null && accounts.Any())
                    accountsModel = BuildToAccountModel(accounts);

                _accountsViewModel.Accounts = accountsModel;
                return View("ViewAccounts", _accountsViewModel);
            }
            catch (CrmDataException ex)
            {
                _logger.Error(ex);
                throw new CrmDataException(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ViewMensajeModel(_messageException, TipoMensajeEnum.Error);
            }
        }

        public PartialViewResult SearchAccount(string accountName = "", string partialViewName = "_accountList")
        {
            try
            {
                List<Account> accounts = _accountProcessor.GetAccounts(accountName);
                List<AccountModel> accountsModel = new List<AccountModel>();

                if (accounts != null && accounts.Any())
                    accountsModel = BuildToAccountModel(accounts);

                return PartialView(partialViewName, accountsModel);
            }
            catch (CrmDataException ex)
            {
                _logger.Error(ex);
                throw new CrmDataException(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetPartialViewErrorModel(_messageException);
            }
        }

        public PartialViewResult NewAccountForm()
        {
            try
            {
                AccountCUViewModel accountCUViewModel = new AccountCUViewModel();
                accountCUViewModel.Accion = "Create";
                return PartialView("_accountForm", accountCUViewModel);
            }
            catch (CrmDataException ex)
            {
                _logger.Error(ex);
                throw new CrmDataException(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetPartialViewErrorModel(_messageException);
            }
        }

        public PartialViewResult EditAccountForm(Guid accountId)
        {
            try
            {
                AccountCUViewModel accountCUViewModel = new AccountCUViewModel();
                Account account = _accountProcessor.GetAccount(accountId);
                accountCUViewModel = BuildToAccountCUViewModel(account);
                accountCUViewModel.Accion = "Update";

                return PartialView("_accountForm", accountCUViewModel);

            }
            catch (CrmDataException ex)
            {
                _logger.Error(ex);
                throw new CrmDataException(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetPartialViewErrorModel(_messageException);
            }
        }

        public JsonResult CreateAccount(AccountCUViewModel accountCUViewModel)
        {
            if (accountCUViewModel.Accion == "Create")
            {
                Account account = BuildToAccount(accountCUViewModel);
                Guid newAccountId = _accountProcessor.CreateAccount(account);

                return GetJsonResultGenericModel(String.Format("Se ha creado el nuevo contacto con el id {0}.", newAccountId), true);
            }

            return GetJsonResultGenericModel("Error en la acción del formulario.");
        }

        public JsonResult UpdateAccount(AccountCUViewModel accountCUViewModel)
        {
            if (accountCUViewModel.Accion == "Update")
            {
                Account account = BuildToAccount(accountCUViewModel);
                bool response = _accountProcessor.UpdateAccount(account);
                return GetJsonResultGenericModel("Cuenta actualizada", response);
            }
            return GetJsonResultGenericModel("Error en la acción del formulario.");
        }

        public JsonResult DeleteAccount(Guid accountId)
        {
            if (accountId != Guid.Empty && accountId != null)
            {
                var response = _accountProcessor.DeleteAccount(accountId);
                return GetJsonResultGenericModel("Cuenta eliminada", response);
            }

            return GetJsonResultGenericModel("Id invalido", false);
        }

        #region private methods

        private PartialViewResult GetPartialViewErrorModel(string message)
        {
            return PartialView("_Error", new ErrorModel()
            {
                Mensaje = _messageException,
                TipoMensaje = TipoMensajeEnum.Error
            });
        }

        private ViewResult ViewMensajeModel(string message, TipoMensajeEnum typeMessage, string urlView = null)
        {
            _accountsViewModel.Informacion.UrlView = urlView;
            _accountsViewModel.Informacion.Mensaje = message;
            _accountsViewModel.Informacion.TipoMensaje = typeMessage;
            return View(_accountsViewModel);
        }

        private List<AccountModel> BuildToAccountModel(List<Account> accounts)
        {
            List<AccountModel> accountModelList = new List<AccountModel>();
            accounts.ForEach(account => accountModelList.Add(new AccountModel()
            {
                Id = account.Id,
                AccountName = account.AccountName
            }));

            return accountModelList;
        }

        private AccountCUViewModel BuildToAccountCUViewModel(Account account)
        {
            return new AccountCUViewModel()
            {
                Id = account.Id,
                AccountName = account.AccountName,
                NumeroCuenta = account.NumeroCuenta,
                CorreoElectronico = account.CorreoElectronico,
                Telefono = account.Telefono,
                Fax = account.Fax,
                SitioWeb = account.SitioWeb,
                CuentaPrimaria = new LookUp()
                {
                    Id = account.CuentaPrimaria.Id,
                    Name = account.CuentaPrimaria.Name
                },
                ContactoPrincipal = new LookUp()
                {
                    Id = account.ContactoPrincipal.Id,
                    Name = account.ContactoPrincipal.Name
                }
            };
        }

        private Account BuildToAccount(AccountCUViewModel accountCUViewModel)
        {
            return new Account()
            {
                Id = accountCUViewModel.Id,
                AccountName = accountCUViewModel.AccountName,
                NumeroCuenta = accountCUViewModel.NumeroCuenta,
                CorreoElectronico = accountCUViewModel.CorreoElectronico,
                Telefono = accountCUViewModel.Telefono,
                Fax = accountCUViewModel.Fax,
                SitioWeb = accountCUViewModel.SitioWeb,
                CuentaPrimaria = new LookUp()
                {
                    Id = accountCUViewModel.CuentaPrimaria.Id,
                    Name = accountCUViewModel.CuentaPrimaria.Name
                },
                ContactoPrincipal = new LookUp()
                {
                    Id = accountCUViewModel.ContactoPrincipal.Id,
                    Name = accountCUViewModel.ContactoPrincipal.Name
                }
            };
        }

        private JsonResult GetJsonResultGenericModel(string message, bool response = false)
        {
            var error = new { Response = response, Message = message };
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}