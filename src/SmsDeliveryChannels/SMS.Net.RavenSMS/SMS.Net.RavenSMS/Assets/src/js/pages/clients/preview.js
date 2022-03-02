$(function () {
    // setup qr code
    var qrcode = new QRCode(document.getElementById("qrcode"), {
        text: qr_content,
        width: 300,
        height: 300,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H
    });

    // get the form settings
    var validatorSettings = $.data($('#update-client-form')[0], 'validator').settings;

    // override the settings
    validatorSettings.onkeyup = false;
    validatorSettings.errorElement = "div";
    validatorSettings.errorClass = "is-invalid";
});