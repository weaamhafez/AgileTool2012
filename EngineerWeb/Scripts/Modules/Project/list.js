function openUpdateDialog($project) {
    var project = JSON.parse(decodeURI($($project).data("project")));
    $("#project-name").val(project["name"]);
    $("#description").val(project["description"]);
    $("#project-id").val(project["Id"]);
    $("#projectModal").modal("show");
};
var formsTable;
function removeObject($project) {
    var params = { "project": { Id: $($project).data("id") } };
    bootbox.confirm("Are you sure?", function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                url: deleteURL,
                data: JSON.stringify(params),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    formsTable.ajax.reload();
                    return false;
                },
                error: function (message) {
                    errorAlert(message.responseJSON.Message);
                    return false;
                }
            });
        }
    });
}

$(document).ready(function () {
    const $diagramsTable = $('#diagramsTable');

    formsTable = $diagramsTable.DataTable({
        "processing": true,
        "serverSide": false,
        "ajax": {
        "url": datatableURL,
        "type": "POST",
        "contentType": "application/json; charset=utf-8",
        "dataSrc":"d"
        },
        "paging": true,
        "columns": [
            {"data": "name"},{"data": "description"},
            {
                "data": "name",
                "render": function (data, type, full, meta) {
                    return ' <div class="btn-group"><a href="#" class="btn btn-info" >Action</a><a href="#" class="btn btn-info dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></a>' +
                        '<ul class="dropdown-menu"><li><a href="javascript:void(0)" onclick="openUpdateDialog(this)" data-project="' + encodeURI(JSON.stringify(full)) + '">Update</a></li><li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="removeObject(this)">Delete</a></li></ul></div>';
                }
            }
        ]
    });
    $('#saveBtn').click(function (e) {

        if (!$('#engineerForm').valid())
            return false;

        e.preventDefault();
        var data = {};
        $("#project_div :input").serializeArray().map(function (x) { data[x.name] = x.value; });
        var params = { "project": data };
        var $btn = $(this).button('loading');
        $.ajax({
            type: "POST",
            url: saveOrUpdateURL,
            data: JSON.stringify(params),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                formsTable.ajax.reload();
                $btn.button('reset');
                $('#projectModal').modal('hide');
                return false;
            },
            error: function (message) {
                errorAlert(message.responseText);
                $btn.button('reset');
                return false;
            }
        });
        return false;
    });

    $('#renameModal').on('shown.bs.modal', function () {
        $('#renameModal').find("input[type=text]").val("");
    });
    $("#dialogNameTitle").text($("#diagram-name").val() == "" ? "Untitled" : $("#diagram-name").val());
});