$(document).ready(function () {
    var formsTable;
    $('#search').click(function (e) {

        if (!$('#engineerForm').valid())
            return false;
        e.preventDefault();
        var data = {};
        $("#searchDiv :input").serializeArray().map(function (x) {
            if (x.name.indexOf("Diagrams") > -1)
                x.name = "Diagrams";
            if (data[x.name] != null)
                data[x.name] += "," + x.value;
            else
                data[x.name] = x.value;
        });
        var $btn = $(this).button('loading');
        const $diagramsTable = $('#diagramsTable');

        formsTable = $diagramsTable.DataTable({
            "processing": true,
            "serverSide": false,
            "ajax": {
                "url": searchURL,
                "data": function (d) {
                    d.diagram = data.Diagrams;
                    dataJSON = JSON.stringify(d);
                    return dataJSON;
                },
                "type": "POST",
                "contentType": "application/json; charset=utf-8",
                "dataType": 'json',
                "dataSrc": "d"
            },
            destroy: true,
            "paging": true,
            "columns": [
                {
                    "data": "UserName",
                },
                {
                    "data": "Email"
                }
            ]
        });
        $btn.button('reset');
    });
});