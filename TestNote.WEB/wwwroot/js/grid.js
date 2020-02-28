var Mmodal = "user-modal";
var Mcontainer = "user-container";
//var Mtable_grid = "#table-user-grid";
//var GridTable = $(Mtable_grid);
var table;

$(document).ready(function () {
    table = $("#table-user-grid").DataTable({
        "serverSide": true,
        "processing": true,
        "ajax": {
            "url": "/User/GetList",
            "datatype": "json"
        },
        "searchDelay": 300,
        "createdRow": function (row, data, dataIndex) {
            var actions = $(row).children().last();
            $(actions).empty();
            if (data.blockDate === null) {
                //$(row).children().first().next().next().css({ "color": "red", "border": "2px solid red" });
                $(actions).append(`<button type="button" class="btn btn-danger btn-md" data-toggle="modal" data-url="/User/Block/` + data.ip + `" id="btn-block-user">
                                    <span class="fa fa-times" aria-hidden="true"></span> Block
                                </button>`);
            }
            else {
                $(actions).append(`<button type="button" class="btn btn-success btn-md" data-toggle="modal" data-url="/User/Unblock/` + data.ip + `" id="btn-unblock-user">
                                    <span class="fa fa-check" aria-hidden="true"></span> Unblock
                                </button>`);
            }
        },
        "columns": [
            {
                "data": "ip",
                "searchable": true
            },
            {
                "data": "userName",
                "searchable": false,
                "sortable": false,
                "orderable": false,
                "render": function (userName) { return userName === null ? "NoName" : userName; }
            },
            {
                "data": "blockDate",
                "searchable": false,
                "sortable": false,
                "orderable": false,
                "render": function (blockDate) {
                    return blockDate === null ? "-" : blockDate;
                }
            },
            {
                "data": "ip",
                "searchable": false,
                "sortable": false,
                "orderable": false,
                "render": function (ip) {
                    return `<div class="btn-group" role="group">
                            </div>`;
                }
            }
        ]
    });

    $("#table-user-grid" + 'td').css('white-space', 'initial');
});


$("#table-user-grid").on("click", "#btn-details-user", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        if (data.data === "null") {
            NotifyError(data);
            return;
        }
        $('#details-' + Mcontainer).html(data);

        $('#details-' + Mmodal).modal('show');
    });

});

$("#table-user-grid").on("click", "#btn-edit-user", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        if (data.data === "null") {
            NotifyError(data);
            return;
        }
        $('#edit-' + Mcontainer).html(data);

        $('#edit-' + Mmodal).modal('show');
        $('.chosen-select').chosen({
            no_results_text: "Oops, nothing found!",
            placeholder_text_multiple: "Please, select some option"
        });
    });
});

$("#table-user-grid").on("click", "#btn-delete-user", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        if (data.data === "null") {
            NotifyError(data);
            return;
        }
        $('#delete-' + Mcontainer).html(data);

        $('#delete-' + Mmodal).modal('show');
    });

});

function DeleteUserSuccess(data) {

    if (data.data != "success") {
        $('#delete-' + Mcontainer).html(data.data);
        NotifyError(data);
        $('#loader').removeClass('loading');
        return;
    }
    Notify('fa fa-check-circle', "Success", "Deleted!", "success");
    $('#delete-' + Mmodal).modal('hide');
    $('#loader').removeClass('loading');
    $('#delete-' + Mcontainer).html("");
    $("#table-user-grid").DataTable().ajax.reload();
}

function UpdateUserSuccess(data) {
    if (data.data != "success") {
        $('#edit-' + Mcontainer).html(data.data);
        NotifyError(data);
        $('#loader').removeClass('loading');
        return;
    }
    Notify('fa fa-check-circle', "Success", "Video was updated!", "success");
    $('#edit-' + Mmodal).modal('hide');
    $('#loader').removeClass('loading');
    $('#edit-' + Mcontainer).html("");
    $("#table-user-grid").DataTable().ajax.reload();
}