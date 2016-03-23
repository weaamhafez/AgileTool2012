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
                    "data": "name",
                },
                {
                    "data": "@readonly",
                    "render": function (data, type, full, meta) {
                        return full.readonly ? "Yes" : "No";
                    }
                }
            ]
        });
        $btn.button('reset');
    });
});