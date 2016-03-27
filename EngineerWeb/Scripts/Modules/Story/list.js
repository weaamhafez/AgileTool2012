function openUpdateDialog($project) {
    var story = JSON.parse(decodeURI($($project).data("story")));
    $("#story-name").val(story["name"]);
    $("#description").val(story["description"]);
    $("#story-id").val(story["Id"]);
    $("#state").val(story["state"]);
    $("#projectModal").modal("show");
};
var userStoriesTable;
var storyJSON;
function openRemoveDialog($story) {
    var storyId = $($story).data("id");

    const $userStoriesTable = $('#userStoriesTable');
    userStoriesTable = $userStoriesTable.DataTable({
        "processing": true,
        "serverSide": false,
        "ajax": {
            "type": "POST",
            "url": loadDiagramsURL,
            "data": function (d) {
                d.story = { Id: storyId };
                storyJSON = JSON.stringify(d);
                return storyJSON;
            },
            "contentType": "application/json; charset=utf-8",
            "dataType": 'json',
            "dataSrc": "d"
        },
        destroy: true,
        "paging": true,
        "columns": [
            {
                "data": "Attachment.name",
            },
            {
                "data": "@readonly",
                "render": function (data, type, full, meta) {
                    return full.readonly ? "Yes" : "No";
                }
            }
        ]
    });
    $("#removeModal").modal("show");
};
var formsTable;
function removeObject($project,action) {
    var params = { "story": { Id: $($project).data("id") } };
    bootbox.confirm("Are you sure?", function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                url: finishURL,
                data: JSON.stringify(params),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    successAlert("Success", "Request Success");
                    formsTable.ajax.reload();
                    $btn.button('reset');
                    return false;
                },
                error: function (message) {
                    errorAlert(message.responseJSON.Message);
                    $btn.button('reset');
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
            {
                "data": "name",
            },
            {
                "data": "description",
            },
            {
                "data": "state",
            },
            {
                "data": "state",
                "render": function (data, type, full, meta) {
                    return ' <div class="btn-group"><a href="#" class="btn btn-info" >Action</a><a href="#" class="btn btn-info dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></a>' +
                        '<ul class="dropdown-menu"><li><a href="javascript:void(0)" onclick="openUpdateDialog(this)" data-story="' + encodeURI(JSON.stringify(full)) + '">Update</a></li><li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="openRemoveDialog(this)">Delete</a></li>' +
                        (full.state != 'DONE' ? '<li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="removeObject(this,\'finish\')">Finish Story</a></li>' : '') + '</ul></div>';
                }
            }
        ]
    });
    $('#deleteBtn').click(function (e) {

        e.preventDefault();
        var $btn = $(this).button('loading');
        $.ajax({
            type: "POST",
            url: deleteURL,
            data: storyJSON,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                formsTable.ajax.reload();
                $btn.button('reset');
                $('#removeModal').modal('hide');
                return false;
            },
            error: function (message) {
                errorAlert(message.responseJSON.Message);
                $btn.button('reset');
                return false;
            }
        });
        return false;
    });
    $('#saveBtn').click(function (e) {

        //TODO validate
        e.preventDefault();
        var data = {};
        $("#project_div :input").serializeArray().map(function (x) {
            if (x.name.indexOf("projectId") > -1) x.name = "projectId";
            else if (x.name.indexOf("AspNetUsers") > -1) x.name = "AspNetUsers";
            if (data[x.name] != null)
                data[x.name] += "," + x.value;
            else
                data[x.name] = x.value;
        });
        var params = { "story": data };
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
                errorAlert(message.responseJSON.Message);
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

    //$("#projectId").select2();
    //$("#AspNetUsers").select2();
});