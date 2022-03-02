$(function () {
    // get the form settings
    var validatorSettings = $.data($('#add-client-form')[0], 'validator').settings;

    // override the settings
    validatorSettings.onkeyup = false;
    validatorSettings.errorElement = "div";
    validatorSettings.errorClass = "is-invalid";
});