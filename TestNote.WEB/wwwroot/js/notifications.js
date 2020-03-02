function Notify(icon, title, message, type) {
    $.notify({
        // options
        icon: icon,
        title: `<strong>${title}</strong>: `,
        message: message
    }, {
            type: type,
            z_index: 1051,
            animate: {
                enter: 'animated bounceIn',
                exit: 'animated bounceOut'
            },
            offset: {
                x: 5,
                y: 90
            }
        });
}

function NotifyError(data) {
    if (Array.isArray(data.errorMessage)) {
        if (data.errorMessage.length >= 1) {
            data.errorMessage.forEach(function (item) {
                $.notify({
                    // options
                    icon: 'fa fa-warning',
                    title: '<strong>Warning</strong>: ',
                    message: item
                }, {
                        type: 'warning',
                        z_index: 1051,
                        animate: {
                            enter: 'animated bounceIn',
                            exit: 'animated bounceOut'
                        },
                        offset: {
                            x: 5,
                            y: 90
                        }
                    });
            });
        }
    }
    else {
        $.notify({
            // options
            icon: 'fa fa-warning',
            title: '<strong>Warning</strong>: ',
            message: data.errorMessage
        }, {
                type: 'warning',
                z_index: 1051,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                },
                offset: {
                    x: 5,
                    y: 90
                }
            });
    }
}

