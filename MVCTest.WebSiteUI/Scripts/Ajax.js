function StandardAjaxCall(urlAction, parameters, functionCallbackSuccess) {
    $.ajax({
        url: urlAction,
        type: "GET",
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: parameters,
        beforeSend: function () {
            $("#loading").fadeIn();
        },
        success: function (data) {
            functionCallbackSuccess(data);
            $("#loading").fadeOut();
        },
        error: function (xhr) {
            alert('Error Ajax: ' + xhr.statusText);
            $("#loading").fadeOut();
        }
    });
}


function StringifyAjaxCall(urlAction, parameters, functionCallbackSuccess) {
    $.ajax({
        url: urlAction,
        type: "POST",
        traditional: true,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters), // Debe obtenerse como JSON.stringify
        dataType: "json",
        cache: true, // sólo para Internet Explorer 8
        beforeSend: function () {
            $("#loading").fadeIn();
        },
        success: function (data) {
            functionCallbackSuccess(data);
            $("#loading").fadeOut();
        },
        error: function (xhr) {
            alert('Error Ajax: ' + xhr.statusText);
            $("#loading").fadeOut();
        }
    });
}