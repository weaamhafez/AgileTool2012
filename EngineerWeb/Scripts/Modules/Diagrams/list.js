var diagramJSON;
var userStoriesTable;
function openRemoveDialog($project) {
    var diagramId = $($project).data("id");

    const $userStoriesTable = $('#userStoriesTable');
    userStoriesTable = $userStoriesTable.DataTable({
        "processing": true,
        "serverSide": false,
        "ajax": {
            "type": "POST",
            "url": loadUserStoriesURL,
            "data": function(d) {
                d.diagram = { Id: diagramId };
                diagramJSON = JSON.stringify(d);
                return diagramJSON;
            },
            "contentType": "application/json; charset=utf-8",
            "dataType": 'json',
            "dataSrc": "d"
        },
        destroy: true,
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
            }
        ]
    });
    $("#removeModal").modal("show");
};
function openView($diagram)
{
    window.location.href = "View?id=" + $($diagram).data("id");
}
var formsTable;
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
                "data": "Attachment.name",
            },
            {
                "data": "@readonly",
                "render": function (data, type, full, meta) {
                    return full.readonly ? "Yes" : "No";
                }
            },
            {
                "data": "Attachment.name",
                "render": function (data, type, full, meta) {
                    return ' <div class="btn-group"><a href="#" class="btn btn-info" >Action</a><a href="#" class="btn btn-info dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></a><ul class="dropdown-menu">' +
                        (!full.readonly ? '<li><a href="javascript:void(0)" onclick="window.location.href = \'New?id=' + full.attachId + '&userStoryId=' + full.userStoryId + '\'" >Update</a></li><li><a href="javascript:void(0)" data-id="' + full.attachId + '" data-userStoryId="' + full.userStoryId + '" onclick="openRemoveDialog(this)">Delete</a></li>' : ' ') +
                        //(full.readonly ? '<li><a href="javascript:void(0)" data-id="' + full.Id + '" onclick="openView(this)">View as image</a></li>' : ' ') +
                        '</ul></div>';
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
            data: diagramJSON,
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
    $("#addDiagram").on("click", function (e) {
        window.location.href = "New";
    });
    //$('#removeModal').on('hidden.bs.modal', function () {
    //    if (userStoriesTable)
    //        userStoriesTable.destroy();
    //});
});