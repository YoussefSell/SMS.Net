// init the list of clients
var clients = [];

// send an ajax request to load the list of clients
$.ajax({
    url: '/ravenSMS/messages/add/?handler=Clients',
})
.done(function (result) {
    // get the client select
    var $clientsSelect = $("#Input_Client");

    // populate the select
    $.each(result, function () {
        $clientsSelect.append($("<option />").val(this.id).text(this.name));
    });

    // save the clients list result
    clients = result;
});

function onEnableSheduling(enabled) {
    if (enabled) {
        $("#delivery-date-container").show();
    }
    else {
        console.log('on hide');
        $("#delivery-date-container").hide();
        $('#Input_DeliveryDate').val(undefined);
    }
}

$(function () {
    // get the form settings
    var validatorSettings = $.data($('#add-message-form')[0], 'validator').settings;

    // override the settings
    validatorSettings.onkeyup = false;
    validatorSettings.errorElement = "div";
    validatorSettings.errorClass = "is-invalid";
});