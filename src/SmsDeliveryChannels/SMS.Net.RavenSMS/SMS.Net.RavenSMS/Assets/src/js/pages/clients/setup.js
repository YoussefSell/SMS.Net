var qrcode = new QRCode(document.getElementById("qrcode"), {
    text: qr_content,
    width: 300,
    height: 300,
    colorDark: "#000000",
    colorLight: "#ffffff",
    correctLevel: QRCode.CorrectLevel.H
});