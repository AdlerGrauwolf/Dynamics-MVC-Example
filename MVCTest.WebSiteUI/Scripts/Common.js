function notificationMessage(message, messageType, container) {

    if (container == null || container == undefined || container == "")
        container = "displayErrorContainer";

    if (messageType == null || messageType == undefined)
        messageType = "";

    var messageClass, messageType = messageType.toLowerCase();;

    switch (messageType) {
        case "":
        case "error":
            messageClass = 'alert-danger';
            break;
        case "success":
            messageClass = 'alert-success';
            break;
        case "info":
            messageClass = 'alert-info';
            break;
        case "warning":
            messageClass = 'alert-warning';
            break;
    }

    // Crear contenedor del error
    var spanError = "<div class='font-standard alert " + messageClass + " alert-dismissable fade in alertPadding' role='alert'>";
    spanError += "<a href='#' class='close' data-dismiss='alert' aria-label='close'><i class='btn btn-sm glyphicon glyphicon-remove'></i></a>";
    spanError += message;
    spanError += "</div>";

    $("#" + container).html(spanError);
}

function disableForm(formName) {
    $("#" + formName).submit(function () { return false; })
}

function getValue(id) {
    return $("#" + id).val();
}

function getHtml(id) {
    return $("#" + id).html();
}

// Si se requiere más de un parametro para  la función que se invoque en el functionCallback 
// pasar functionCallbackParameters como objeto que contenga los parámetros necesarios.
// Ej. var var parameters = {Marca: 'VolksWagen', Modelo: 'Gol', Especifiaciones : {Color: 'Gris Acero', Puertas : '5'}}
function bootBoxCustomConfirm(title, message, type, functionCallback, functionCallbackParameters) {

    var className = customeAlertType(type);

    bootbox.confirm({
        title: title,
        className: className.main,
        message: message,
        buttons: {
            confirm: {
                label: '<i class="glyphicon glyphicon-ok"></i> Aceptar',
                className: 'btn-success btn-sm'
            },
            cancel: {
                label: '<i class="glyphicon glyphicon-remove"></i> Cancelar',
                className: 'btn-danger btn-sm'
            }
        },
        callback: function (result) {
            if (result) {
                functionCallback(functionCallbackParameters);
            }
        }
    });
}

function bootBoxCustomAlert(message, type) {
    var className = customeAlertType(type);
    bootbox.alert({
        className: className.main,
        message: message,
        buttons: {
            ok: {
                label: 'Aceptar',
                className: className.button
            }
        }
    });
}

function customeAlertType(type) {

    var className = {};

    switch (type) {
        case "default":
            className["main"] = 'customAlertDefault';
            className["button"] = 'btn-default btn-sm';
            break;
        case "primary":
            className["main"] = 'customAlertPrimary';
            className["button"] = 'btn-primary btn-sm';
            break;
        case "error":
            className["main"] = 'customAlertDanger';
            className["button"] = 'btn-danger btn-sm';
            break;
        case "success":
            className["main"] = 'customAlertSuccess';
            className["button"] = 'btn-success btn-sm';
            break;
        case "info":
            className["main"] = 'customAlertInfo';
            className["button"] = 'btn-info btn-sm';
            break;
        case "warning":
            className["main"] = 'customAlertWarning';
            className["button"] = 'btn-warning btn-sm';
            break;
        default:
            className["main"] = 'customAlertPrimary';
            className["button"] = 'btn-primary btn-sm';
            break;
    }

    return className;
}


function bootBoxLoading() {

    dialog = bootbox.dialog({
        closeButton: false,
        onEscape: false,
        size: 'small',
        message: '<p><i class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></i> Cargando...</p>'
    });

    dialog.on("shown.bs.modal", function () {
        dialog.attr("id", "bootboxLoading");
    });

}

function closeBootboxLoading() {
    $("#bootboxLoading").modal('hide');
}


function disableOtherCheckbox(element, chkName) {
    if ($(element).is(":checked")) {
        disableEnableCheckbox(true, chkName);
    } else {
        disableEnableCheckbox(false, chkName);
    }
}

function disableEnableCheckbox(bool, chkName) {
    $("input[name='" + chkName + "']").each(function () {
        if (!$(this).is(":checked"))
            $(this).prop('disabled', bool);
    });
}