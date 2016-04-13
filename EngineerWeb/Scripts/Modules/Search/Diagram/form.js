function openView($diagram) {
    window.location.href = viewURL + "?id=" + $($diagram).data("id") + "&storyId=" + $($diagram).data("storyid") + "&historyId=" + $($diagram).data("historyid");
}
$(document).ready(function () {
    var formsTable;
    $('#search').click(function (e) {
        if (!$('#engineerForm').valid())
            return false;
        e.preventDefault();
        var data = {};
        $("#searchDiv :input").serializeArray().map(function (x) {
            if (x.name.indexOf("Users") > -1)
                x.name = "Users";
            else if (x.name.indexOf("Stories") > -1)
                x.name = "Stories";
            else if (x.name.indexOf("DiagramName") > -1)
                x.name = "DiagramName";
            else if (x.name.indexOf("Sprint") > -1)
                x.name = "Sprint";

            if (data[x.name] != null)
                data[x.name] += "," + x.value;
            else
                data[x.name] = x.value;
        });
        var params = { "usersAndStory": data };
        var $btn = $(this).button('loading');
        const $diagramsTable = $('#diagramsTable');

        formsTable = $diagramsTable.DataTable({
            "processing": true,
            "serverSide": false,
            "ajax": {
                "url": searchURL,
                "data": function (d) {
                    d.usersAndStory = new Object();
                    d.usersAndStory.Users = data.Users;
                    d.usersAndStory.Stories = data.Stories;
                    d.usersAndStory.DiagramName = data.DiagramName;
                    d.usersAndStory.Sprint = data.Sprint;
                    dataJSON = JSON.stringify(d);
                    return dataJSON;
                },
                "type": "POST",
                "contentType": "application/json; charset=utf-8",
                "dataType": 'json',
                "dataSrc": "d"
            },
            destroy:true,
            "paging": true,
            "columns": [
                {
                    "data": "DiagramName",
                    "render": function (data, type, full, meta) {
                        return '<a href="javascript:void(0)" data-id="' + full.AttachmentId + '" data-storyid="' + full.UserStoryId + '" data-historyid="'+ full.HistoryId + '" onclick="openView(this)">' + full.DiagramName  + '</a>';
                    }
                },
                { "data": "UserStoryName" },
                { "data": "Users" },
                {"data":"CreatedBy"},
                { "data": "Date" },
                { "data": "Version" },
                {"data": "SprintNumber"}
            ]
        });
        $btn.button('reset');
    });
});