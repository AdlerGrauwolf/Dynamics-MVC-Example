using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCTest.BusinessInterfaces;
using MVCTest.OperationalManagement;
using MVCTest.WebSiteUI.ViewModel;
using MVCTest.BusinessTypes.Exceptions;
using static MVCTest.WebSiteUI.Models.ErrorModel;
using MVCTest.WebSiteUI.Models;
using MVCTest.BusinessTypes;

namespace MVCTest.WebSiteUI.Controllers
{
    public class ContactController : Controller
    {
        #region Declariación de objetos

        private readonly string _messageException = "Ocurrió un error en la aplicación";
        private readonly IContactProcessor _contactProcessor;
        private readonly ILogger _logger;

        ContactsViewModel _contacsViewModel = default(ContactsViewModel);

        public ContactController(IContactProcessor contactProcessor, ILogger logger)
        {
            _contactProcessor = contactProcessor;
            _logger = logger;
        }

        #endregion
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewContacs()
        {
            _contacsViewModel = new ContactsViewModel();
            try
            {
                List<Contact> contacts = _contactProcessor.GetShortContacts();
                List<ContactModel> contactsModel = new List<ContactModel>();

                if (contacts != null && contacts.Any())
                    contactsModel = BuildToShortContacsModel(contacts);

                _contacsViewModel.Contacts = contactsModel;
                return View("ViewContacs", _contacsViewModel);
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

        public PartialViewResult SearchContacts(string contactName = "", string partialViewName = "_contactList")
        {
            try
            {
                List<ContactModel> contactsModel = new List<ContactModel>();
                List<Contact> contacts = _contactProcessor.GetShortContacts(contactName);

                if (contacts != null && contacts.Any())
                    contactsModel = BuildToShortContacsModel(contacts);

                return PartialView(partialViewName, contactsModel);
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

        public ActionResult NewContactForm()
        {
            try
            {
                ContactCUViewModel contactCUModel = new ContactCUViewModel();
                string optionSetName = "preferredcontactmethodcode";
                string entityName = "contact";
                contactCUModel.contactMethodSelector = _contactProcessor.GetContactMethodSelector(optionSetName, entityName);
                contactCUModel.Accion = "Create";
                return PartialView("_contactForm", contactCUModel);
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

        public JsonResult CreateContact(ContactCUViewModel objContactCUViewModel)
        {
            if (objContactCUViewModel.Accion == "Create")
            {
                Contact objContact = new Contact();
                objContact = BuildToContactModel(objContactCUViewModel);

                Guid newContactId = _contactProcessor.CreateContact(objContact);

                return GetJsonResultGenericModel(String.Format("Se ha creado el nuevo contacto con el id {0}.", newContactId), true);
            }

            return GetJsonResultGenericModel("Error en la acción del formulario.");
        }

        public ActionResult UpdateContactForm(Guid contactId)
        {
            try
            {
                ContactCUViewModel contactCUModel = new ContactCUViewModel();
                Contact contact = _contactProcessor.GetContact(contactId);

                contactCUModel = BuildToContactCUViewModel(contact);

                string optionSetName = "preferredcontactmethodcode";
                string entityName = "contact";
                contactCUModel.contactMethodSelector = _contactProcessor.GetContactMethodSelector(optionSetName, entityName);
                contactCUModel.Accion = "Update";

                return PartialView("_contactForm", contactCUModel);
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

        public JsonResult UpdateContact(ContactCUViewModel objContactCUViewModel)
        {
            if (objContactCUViewModel.Accion == "Update")
            {
                Contact objContact = new Contact();
                objContact = BuildToContactModel(objContactCUViewModel);

                bool response = _contactProcessor.UpdateContact(objContact);

                return GetJsonResultGenericModel("Contacto actualizado", response);
            }

            return GetJsonResultGenericModel("Error en la acción del formulario.");
        }

        public JsonResult DeleteContact(Guid contactId)
        {
            if (contactId != Guid.Empty && contactId != null)
            {
                bool response = _contactProcessor.DeleteContact(contactId);
                return GetJsonResultGenericModel("Contacto eliminado", response);
            }

            return GetJsonResultGenericModel("Id invalido", false);
        }

        private PartialViewResult GetPartialViewErrorModel(string message)
        {
            return PartialView("_Error", new ErrorModel()
            {
                Mensaje = _messageException,
                TipoMensaje = TipoMensajeEnum.Error
            });
        }

        #region privateMethods

        private ViewResult ViewMensajeModel(string message, TipoMensajeEnum typeMessage, string urlView = null)
        {
            _contacsViewModel.Informacion.UrlView = urlView;
            _contacsViewModel.Informacion.Mensaje = message;
            _contacsViewModel.Informacion.TipoMensaje = typeMessage;
            return View(_contacsViewModel);
        }

        private List<ContactModel> BuildToShortContacsModel(List<Contact> contacts)
        {
            List<ContactModel> contactModel = new List<ContactModel>();
            contacts.ForEach(contact => contactModel.Add(new ContactModel()
            {
                Id = contact.Id,
                FullName = contact.FullName
            }));
            return contactModel;
        }

        private Contact BuildToContactModel(ContactCUViewModel objContactCUViewModel)
        {
            return new Contact()
            {
                Id = objContactCUViewModel.Id,
                FirstName = objContactCUViewModel.FirstName,
                LastName = objContactCUViewModel.LastName,
                Puesto = objContactCUViewModel.Puesto,
                CorreoElectronico = objContactCUViewModel.CorreoElectronico,
                TelefonoTrabajo = objContactCUViewModel.TelefonoTrabajo,
                TelefonoMovil = objContactCUViewModel.TelefonoMovil,
                MetodoContacto = objContactCUViewModel.MetodoContacto,
                CuentaPrincipal = new LookUp()
                {
                    Id = objContactCUViewModel.CuentaPrincipal.Id,
                    Name = objContactCUViewModel.CuentaPrincipal.Name
                }
            };
        }

        private ContactCUViewModel BuildToContactCUViewModel(Contact objContact)
        {
            return new ContactCUViewModel()
            {
                Id = objContact.Id,
                FirstName = objContact.FirstName,
                LastName = objContact.LastName,
                Puesto = objContact.Puesto,
                CorreoElectronico = objContact.CorreoElectronico,
                TelefonoTrabajo = objContact.TelefonoTrabajo,
                TelefonoMovil = objContact.TelefonoMovil,
                MetodoContacto = objContact.MetodoContacto,
                CuentaPrincipal = new LookUp()
                {
                    Id = objContact.CuentaPrincipal.Id,
                    Name = objContact.CuentaPrincipal.Name
                },
                CuentasAsociadas = objContact.CuentasAsociadas
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