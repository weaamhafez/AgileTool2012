function openUpdateDialog($project) {
    var sprint = JSON.parse(decodeURI($($project).data("sprint")));
    $("#sprint-number").val(sprint["number"]);
    $("#topic").val(sprint["topic"]);
    $("#sprint-id").val(sprint["Id"]);
    $("#state").val(sprint["state"]);
    var sprintJSON = { sprint: sprint["Id"] };
    $.ajax({
        type: "POST",
        url: "List.aspx/GetSprintStories",
        data: JSON.stringify(sprintJSON),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (stories) {
            if (stories.d.length > 0) {
                for (var i = 0; i < stories.d.length; i++) {
                    $("#UserStories option[value=\"" + (stories.d)[i].Id + "\"]").attr("selected", "selected");
                    $("#UserStories option[value=\"" + (stories.d)[i].Id + "\"]").prop("selected", "selected");
                }


                $("#UserStories").selectpicker('refresh')
            }
            $("#projectModal").modal("show");
            return false;
        },
        error: function (message) {
            errorAlert(message.responseJSON.Message);
            return false;
        }
    });
};
var formsTable;
function removeObject($project, action) {
    var params = { "sprint": { Id: $($project).data("id") } };
    var url = deleteURL;
    if (action === 'finish')
        url = closeURL;
    else if (action === 'open')
        url = openURL;
    bootbox.confirm("Are you sure?", function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                url: url,
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
            {"data": "number"},{"data": "topic"},{"data": "state"},
            {
                "data": "name",
                "render": function (data, type, full, meta) {
                    return ' <div class="btn-group"><a href="#" class="btn btn-info" >Action</a><a href="#" class="btn btn-info dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></a>' +
                        '<ul class="dropdown-menu"><li><a href="javascript:void(0)" onclick="openUpdateDialog(this)" data-sprint="' + encodeURI(JSON.stringify(full)) + '">Update</a></li><li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="removeObject(this)">Delete</a></li>' +
                        (full.state == 'CLOSED' ? '<li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="removeObject(this,\'open\')">Re-Open Sprint</a></li>' : '<li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="removeObject(this,\'finish\')">Close Sprint</a></li>') + '</ul></div>';
                }
            }
        ]
    });

    $('#saveBtn').click(function (e) {

        if (!$('#engineerForm').valid())
            return false;
        e.preventDefault();
        var data = {};
        $("#project_div :input").serializeArray().map(function (x) {
            if (x.name.indexOf("UserStories") > -1)
                x.name = "UserStories";
            if (data[x.name] != null)
                data[x.name] += "," + x.value;
            else
                data[x.name] = x.value;
        });
        var params = { "sprint": data };
        var $btn = $(this).button('loading');
        $.ajax({
            type: "POST",
            url: saveOrUpdateURL,
            data: JSON.stringify(params),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                successAlert("Success", "Request Success");
                formsTable.ajax.reload();
                $btn.button('reset');
                $('#projectModal').modal('hide');
                return false;
            },
            error: function (message) {
                errorAlert("Error",message.responseJSON.Message);
                $btn.button('reset');
                return false;
            }
        });
        return false;
    });


    $('#projectModal').on('hidden.bs.modal', function () {
        $('#project_div').find("input[type=text],input[type=hidden],select,textarea").val("");
        $('#project_div').find(".selectpicker").selectpicker('refresh');
    });
});