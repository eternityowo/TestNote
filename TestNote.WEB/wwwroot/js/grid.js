var Mmodal = "user-modal";
var Mcontainer = "user-container";

var table;

$(document).ready(function () {
    table = $("#table-user-grid").DataTable({
        "serverSide": true,
        "processing": true,
        "ajax": {
            "url": "/User/GetList",
            "datatype": "json"
        },
        "searchDelay": 350,
        "createdRow": function (row, data, dataIndex) {
            var actions = $(row).children().last();
            $(actions).empty();
            var act = `<button type="button" class="btn btn-` + (data.blockDate == null ? 'danger' : 'success') + ` btn-md" data-toggle="modal" data-url="/User/ChangeUserStatus/" id="btn-change-user">
                                    <span class="fa fa-` + (data.blockDate == null ? 'times' : 'check') + `" aria-hidden="true"></span>` + (data.blockDate == null ? 'Block' : 'Unblock') + `
                                </button>`;
            $(actions).append(act);
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


$("#table-user-grid").on("click", "#btn-change-user", function () {

    var url = $(this).data("url");

    $.get(url, function (data) {
        if (data.data === "null") {
            NotifyError(data);
            return;
        }

        if (data.data !== "success") {
            NotifyError(data);
            return;
        }
        Notify('fa fa-check-circle', "Success", "User status changed", "success");
        $("#table-user-grid").DataTable().ajax.reload();
    });

});