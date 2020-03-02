$(document).ready(function () {

    $('#datetimepicker1').datetimepicker();
    $('#datetimepicker2').datetimepicker({
        useCurrent: false //Important! See issue #1075
    });

    $("#datetimepicker1").on("change.datetimepicker", function (e) {
        $('#datetimepicker2').datetimepicker('minDate', e.date);
    });

    $("#datetimepicker2").on("change.datetimepicker", function (e) {
        $('#datetimepicker1').datetimepicker('maxDate', e.date);
    });

    $.ajax({
        url: '/Note/Create',
        dataType: 'html',
        success: function (data) {
            $('#myPartialContainer').html(data);

            var $form = $("#myPartialContainer").find("form");

            $form.removeData("validator");
            $form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($form);
        }
    });
});

function LoadNodeSucces(data) {
    var dvItems = $("#noteTable");
    dvItems.empty();
    $.each(data, function (i, item) {

        $('<div class="col-md-3">' +
            '<div class="card mb-3 shadow-sm">' +
            '<div class="card-body">' +
            '<p class="card-text">' + item.content + '</p>' +
            '<small class="text-muted">' + item.userName + ', ' + item.createDate + '</small>' +
            '</div>' +
            '</div>' +
            '</div>').appendTo(dvItems);
    });
}

function CreateNodeSucces(data) {
    var dvItems = $("#noteTable");
    $('<div class="col-md-3">' +
        '<div class="card mb-3 shadow-sm">' +
        '<div class="card-body">' +
        '<p class="card-text">' + data.content + '</p>' +
        '<small class="text-muted">' + data.userName + ', ' + data.createDate + '</small>' +
        '</div>' +
        '</div>' +
        '</div>').appendTo(dvItems);

    var user = data.userName;
    var message = 'messageInput';
    connection.invoke("SendNotification", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

function CreateNodeError(data) {
    NotifyError(data);
}