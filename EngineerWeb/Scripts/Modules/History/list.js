var diagramJSON;
function openHistoryDialog($project) {
    var diagramId = $($project).data("id");
    var userStoryId = $($project).data("storyid");
    const $diagramHistoryTable = $('#diagramHistoryTable');
    var diagramHistory = $diagramHistoryTable.DataTable({
        "processing": true,
        "serverSide": false,
        "ajax": {
            "type": "POST",
            "url": loadHistoryURL,
            "data": function(d) {
                d.diagram = { attachId: diagramId, userStoryId: userStoryId };
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
                "data": "AspNetUser.UserName",
            },
            {
                "data": "Date",
            },
            {
                "data": "attachId",
                "render": function (data, type, full, meta) {
                    return '<button class="btn btn-primary" type="button" onclick="openView(this)" data-id="' + full.Id + '"><span class="glyphicon glyphicon-eye-open" aria-hidden="true" ></span>Show</button>';
                }
            }
        ]
    });
    $("#showHistoryModal").modal("show");
};
function openView($diagram)
{
    window.location.href = viewURL + "?id=" + $($diagram).data("id");
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
                "data": "UserStory.name"
            },
            {
                "data": "Attachment.name",
                "render": function (data, type, full, meta) {
                    return '<button class="btn btn-primary" type="button" onclick="openHistoryDialog(this)" data-id="' + full.attachId + '" data-storyid="' + full.userStoryId + '"><span class="glyphicon glyphicon-eye-open" aria-hidden="true" ></span>View</button>';
                }
            }
        ]
    });
});