var emptyGuid = "00000000-0000-0000-0000-000000000000";

$(function () {
    InitMethods();
});

function InitMethods() {
    SearchContact();
    addNewContact();
    disableForm("formContact");
    submitContactForm();
    btnCancelar();
    btnRefresh();
}

function btnRefresh() {
    $("#btnRefresh").click(function () {
        reloadContactGrid();
    });
}

function btnCancelar() {
    $("#btnCancelar").click(function () {
        $("#contactModal").modal("hide");
    })
}

function SearchContact() {
    $("#btnSearchContact").click(function () {
        var valorBusqueda = $("#searchContact").val();
        if (valorBusqueda != "" && valorBusqueda != null) {
            if (valorBusqueda.length >= 3) {
                var parameters = { contactName: valorBusqueda };
                StandardAjaxCall(_actionURLSearchContacts, parameters, SuccessSearchContact);
            } else {
                customNotice("El parámetro de búsqueda debe de ser mayor o igual a 3 caracteres.", "Atención: ", "error", 3350);
                $("#searchContact").focus();
            }
        } else {
            customNotice("No se ha ingresado un parámetro de búsqueda.", "Atención: ", "error", 3350);
            $("#searchContact").focus();
        }
    });
}

function SuccessSearchContact(data) {
    $("#listaContactos").html(data);
}

function addNewContact() {
    $("#btnNewContact").click(function () {
        $("#contactModelHeader").html("Nuevo contacto");
        StandardAjaxCall(_actionURLNewContactForm, "", openContactForm);
    });
}

function openContactForm(data) {
    $("#modalContainer").html(data);
    $('#contactModal').modal('show');
}

function btnShowAccounts() {
    $("#contactModal").modal("hide");
    $("#subModalAccount").modal("show");
}

function searchAccounts() {
    var valorBusqueda = $("#accountName").val();
    if (valorBusqueda != "" && valorBusqueda != null) {
        var parameters = { accountName: valorBusqueda, partialViewName: "_accountSelect" };
        StandardAjaxCall(_actionURLSearchAccount, parameters, successSearchAccount);
    } else {
        customNotice("No se ha ingresado un parámetro de búsqueda.", "Atención: ", "error", 3350);
        $("#searchContact").focus();
    }
}

function successSearchAccount(data) {
    $("#subModalAccountContainer").html(data);
}

function SelectAccount() {
    var accountName = $("input[name='checkboxAccounts[]']:checked").val();
    var accountId = $("input[name='checkboxAccounts[]']:checked").attr("id");

    $("#selectedAccountId").val(accountId);
    $("#selectedAccountName").val(accountName);

    $("#contactModal").modal("show");
    $("#subModalAccount").modal("hide");
}

function btnClearAccount() {
    $("#selectedAccountId").val("");
    $("#selectedAccountName").val("");
}

function submitContactForm() {
    $("#btnAceptar").click(function () {
        var form = $("#formContact");
        $.validator.unobtrusive.parse(form);
        form.validate();
        if (form.valid()) {
            var _actionURL = actionMethodURL();
            var objContact = buildContactObj();
            StringifyAjaxCall(_actionURL, objContact, successCUContact)
        } else {
            $.each(form.validate().errorList, function (key, value) {
                $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                $errorSpan.show();
            });
        }
    });
}

function actionMethodURL() {
    var url;
    var contactId = $("#contactId").val();
    var formAction = $("#formAction").val();

    if (formAction == "Update" && contactId != emptyGuid) {
        url = _actionURLUpdateContact;
    } else {
        url = _actionURLCreateContact;
    }

    return url;
}

function successCUContact(data) {
    if (data.Response) {
        reloadContactGrid();
        customNotice(data.Message, "Éxito:", "success", 3350);
        $("#contactModal").modal("hide");
    } else {
        customNotice(data.Message, "Error:", "error", 3350);
    }
}

function editContact(contactId) {
    if (contactId != null && contactId != "" && contactId != emptyGuid) {
        var parameters = { contactId: contactId };

        $("#contactModelHeader").html("Editar contacto");
        StandardAjaxCall(_actionURLEditContactForm, parameters, openContactForm);

    } else {
        customNotice("No se encontró el id del elemento seleccionado", "Error:", "error", 3350);
    }
}

function buildContactObj() {
    var objContact = {};
    var objAccount = {
        Id: getValue("selectedAccountId"),
        Name: getValue("selectedAccountName")
    };

    if (getValue("formAction") == "Update")
        objContact["Id"] = getValue("contactId");

    objContact["Accion"] = getValue("formAction");
    objContact["FirstName"] = getValue("FirstName");
    objContact["LastName"] = getValue("LastName");
    objContact["Puesto"] = getValue("Puesto");
    objContact["CorreoElectronico"] = getValue("CorreoElectronico");
    objContact["TelefonoTrabajo"] = getValue("TelefonoTrabajo");
    objContact["TelefonoMovil"] = getValue("TelefonoMovil");
    objContact["CuentaPrincipal"] = objAccount;
    objContact["MetodoContacto"] = getValue("contactMethod");

    return objContact;
}

function reloadContactGrid() {
    // Reutiliza la función del servidor para buscar un contacto
    // Se pasa el parametro de búsqueda vacio para que traiga todos los contactos   
    StandardAjaxCall(_actionURLSearchContacts, "", SuccessSearchContact);
}

function DeleteContactMessage(contactId, nombreContacto) {
    var alertType = "error";
    var title = "Advertencia";
    var message = "Está apunto de eliminar el contacto <em class='text-danger'>" + nombreContacto + "</em>, esta acción no se puede deshacer. ¿Desea continuar?";

    bootBoxCustomConfirm(title, message, alertType, deleteContact, contactId);
}

function deleteContact(contactId) {
    if (contactId != null && contactId != "" && contactId != emptyGuid) {
        var parameters = { contactId: contactId };
        StandardAjaxCall(_actionURLDeleteContact, parameters, SuccessDeleteContact);
    } else {
        customNotice("Error, id invalido.", "Error:", "error", 3350)
    }

}

function SuccessDeleteContact(data) {
    if (data.Response) {
        reloadContactGrid();
        customNotice(data.Message, "Éxito:", "success", 3350);
    } else {
        customNotice(data.Message, "Error:", "error", 3350);
    }
}