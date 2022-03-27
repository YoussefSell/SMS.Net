function onResend() {
    // get the messageId
    const messageId = $('#SmsMessage_Id').val();

    // send an ajax request to resend request
    $.get('/ravenSMS/Messages/Preview/' + messageId + '/?handler=Resend')
        .done(function (result) {
            if (result.success) {
                $('#SmsMessage_Status').val('Queued');

                $.toast({
                    heading: 'Success',
                    text: 'the message has been queued successfully.',
                    showHideTransition: 'slide',
                    position: 'top-right',
                    icon: 'success'
                });
                return;
            }

            $.toast({
                heading: 'Failed to resend',
                text: '<p>failed to resend the message, error message:<br/>' + result.message + '</p>',
                showHideTransition: 'fade',
                position: 'top-right',
                icon: 'error'
            });
        });
}