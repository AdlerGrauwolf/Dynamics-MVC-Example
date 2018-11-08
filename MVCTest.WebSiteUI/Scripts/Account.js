var emptyGuid = "00000000-0000-0000-0000-000000000000";

$(function () {
    initMethods();
});

function initMethods() {
    disableForm("formAccount");
    SearchAccount();
    addNewAccount();
    submitAccountForm();
    btnCancelar();
    btnRefresh();
}

function SearchAccount() {
    $("#btnSearchAccount").click(function () {
        var valorBusqueda = $("#searchAccount").val();
        if (valorBusqueda != "" && valorBusqueda != null) {
            if (valorBusqueda.length >= 3) {
                var parameters = { accountName: valorBusqueda };
                StandardAjaxCall(_actionURLSearchAccount, parameters, SuccessSearchAccount);
            } else {
                customNotice("El parámetro de búsqueda debe de ser mayor o igual a 3 caracteres.", "Advertencia:", "error", 3350);
                $("#searchAccount").focus();
            }
        } else {
            customNotice("No se ha ingresado un parámetro de búsqueda.", "Advertencia:", "error", 3350);
            $("#searchAccount").focus();
        }
    });
}

function SuccessSearchAccount(data) {
    $("#listaCuentas").html(data);
}

function reloadAccountGrid() {
    // Reutiliza la función del servidor para buscar una cuenta
    // Se pasa el parametro de búsqueda vacio para que traiga todas las cuentas
    StandardAjaxCall(_actionURLSearchAccount, "", SuccessSearchAccount);
}

function btnRefresh() {
    $("#btnRefresh").click(function () {
        reloadAccountGrid();
    });
}

function addNewAccount() {
    $("#btnNewAccount").click(function () {
        $("#accountModelHeader").html("Nueva cuenta");
        StandardAjaxCall(_actionURLNewAccountForm, "", openAccountForm);
    });
}

function openAccountForm(data) {
    $("#modalContainer").html(data);
    $("#accountModal").modal("show");
}

function btnCancelar() {
    $("#btnCancelar").click(function () {
        $("#accountModal").modal("hide");
    })
}

// Cuentas
function btnClearAccount() {
    $("#selectedAccountName").val("");
    $("#selectedAccountId").val("");
}

function btnShowAccounts() {
    $("#accountModal").modal("hide");
    $("#subModalAccount").modal("show");
}

function searchAccounts() {
    var valorBusqueda = $("#accountName").val();
    if (valorBusqueda != "" && valorBusqueda != null) {
        var parameters = { accountName: valorBusqueda, partialViewName: "_accountSelect" };
        StandardAjaxCall(_actionURLSearchAccount, parameters, successSearchSelectAccount);
    } else {
        customNotice("No se ha ingresado un parámetro de búsqueda.", "Atención: ", "error", 3350);
        $("#accountName").focus();
    }
}

function successSearchSelectAccount(data) {
    $("#subModalAccountContainer").html(data);
}

function SelectAccount() {
    var accountName = $("input[name='checkboxAccounts[]']:checked").val();
    var accountId = $("input[name='checkboxAccounts[]']:checked").attr("id");

    $("#selectedAccountId").val(accountId);
    $("#selectedAccountName").val(accountName);

    $("#accountModal").modal("show");
    $("#subModalAccount").modal("hide");
}


// Conatactos
function btnClearContact() {
    $("#selectedContactName").val("");
    $("#selectedContactId").val("");
}

function btnShowContacts() {
    $("#accountModal").modal("hide");
    $("#subModalContact").modal("show");
}

function searchContacts() {
    var valorBusqueda = $("#contactName").val();
    if (valorBusqueda != "" && valorBusqueda != null) {
        var parameters = { contactName: valorBusqueda, partialViewName: "_contactSelect" };
        StandardAjaxCall(_actionURLSearchContacts, parameters, successSearchContact);
    } else {
        customNotice("No se ha ingresado un parámetro de búsqueda.", "Atención: ", "error", 3350);
        $("#contactName").focus();
    }
}

function successSearchContact(data) {
    $("#subModalContactContainer").html(data);
}

function SelectContact() {
    var accountName = $("input[name='checkboxContacts[]']:checked").val();
    var accountId = $("input[name='checkboxContacts[]']:checked").attr("id");

    $("#selectedContactId").val(accountId);
    $("#selectedContactName").val(accountName);

    $("#accountModal").modal("show");
    $("#subModalContact").modal("hide");
}



function editAccount(accountId) {
    if (accountId != null && accountId != "" && accountId != emptyGuid) {
        var parameters = { accountId: accountId };

        $("#accountModelHeader").html("Editar cuenta");
        StandardAjaxCall(_actionURLUpdateAccountForm, parameters, openAccountForm)
    } else {
        customNotice("No se encontró el id de la cuenta", "Error:", "error", 3350);
    }
}

function submitAccountForm() {
    $("#btnAceptar").click(function () {
        var form = $("#formAccount");
        $.validator.unobtrusive.parse(form);
        form.validate();
        if (form.valid()) {
            var _urlAction = actionMethodURL();
            var objAccount = BuildAccountObj();
            StringifyAjaxCall(_urlAction, objAccount, successCUAccount)
        } else {
            $.each(form.validate().errorList, function (key, value) {
                $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                $errorSpan.show();
            });
        }
    });
}

function successCUAccount(data) {
    if (data.Response) {
        reloadAccountGrid();
        customNotice(data.Message, "Éxito:", "success", 3350);
        $("#accountModal").modal("hide");
    } else {
        customNotice(data.Message, "Error:", "error", 3350);
    }
}

function actionMethodURL() {
    var url;
    var contactId = getValue("accountId");
    var formAction = getValue("formAction");

    if (formAction == "Update" && contactId != emptyGuid) {
        url = _actionURLUpdateAccount;
    } else {
        url = _actionURLCreateAccount;
    }

    return url;
}

function BuildAccountObj() {
    var objAccount = {};
    var objPrimaryAccount = {
        Id: getValue("selectedAccountId"),
        Name: getValue("selectedAccountName")
    };
    var objPrimaryContact = {
        Id: getValue("selectedContactId"),
        Name: getValue("selectedContactName")
    };

    if (getValue("formAction") == "Update")
        objAccount["Id"] = getValue("accountId");

    objAccount["Accion"] = getValue("formAction");
    objAccount["AccountName"] = getValue("AccountName");
    objAccount["NumeroCuenta"] = getValue("NumeroCuenta");
    objAccount["CorreoElectronico"] = getValue("CorreoElectronico");
    objAccount["Telefono"] = getValue("Telefono");
    objAccount["Fax"] = getValue("Fax");
    objAccount["SitioWeb"] = getValue("SitioWeb");
    objAccount["CuentaPrimaria"] = objPrimaryAccount;
    objAccount["ContactoPrincipal"] = objPrimaryContact;

    return objAccount;
}

function DeleteAccountMessage(accountId, nombreCuenta) {
    var alertType = "error";
    var title = "Advertencia";
    var message = "Está apunto de eliminar la cuenta <em class='text-danger'>" + nombreCuenta + "</em>, esta acción no se puede deshacer. ¿Desea continuar?";

    bootBoxCustomConfirm(title, message, alertType, deleteAccount, accountId);
}

function deleteAccount(accountId) {
    if (accountId != null && accountId != "" && accountId != emptyGuid) {
        var parameters = { accountId: accountId };
        StandardAjaxCall(_actionURLDeleteAccount, parameters, SuccessDeleteAccount);
    } else {
        customNotice("Error, id invalido.", "Error:", "error", 3350)
    }

}

function SuccessDeleteAccount(data) {
    if (data.Response) {
        reloadAccountGrid();
        customNotice(data.Message, "Éxito:", "success", 3350);
    } else {
        customNotice(data.Message, "Error:", "error", 3350);
    }
}