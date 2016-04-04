(function () {
    'use strict';

    angular
        .module('app')
        .controller('controller', controller);

    controller.$inject = ['$scope','$compile'];
    function controller($scope, $compile) {
        $scope.openUpdateDialog = function($project) {
            var project = JSON.parse(decodeURI($($project.currentTarget).data("project")));
            $scope.projectName = project["name"];
            $scope.description = project["description"];
            $scope.projectId = project["Id"];
            var projectJSON = { project: project["Id"] };
            $.ajax({
                type: "POST",
                url: "List.aspx/GetProjectUsers",
                data: JSON.stringify(projectJSON),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (users) {
                    if (users.d.length > 0) {
                        for (var i = 0; i < users.d.length; i++) {
                            $("#AspNetUsers option[value=\"" + (users.d)[i].Id + "\"]").attr("selected", "selected");
                            $("#AspNetUsers option[value=\"" + (users.d)[i].Id + "\"]").prop("selected", "selected");
                        }


                        $("#AspNetUsers").selectpicker('refresh')
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
        $scope.removeObject = function($project) {
            var params = { "project": { Id: $($project.currentTarget).data("id") } };
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
        destroy: true,
        "columns": [
            {"data": "name"},{"data": "description"},
            {
                data: "name",
                "render":function(){
                    return "";
                },
                "createdCell": function (td, cellData, rowData, row, col) {
                    var $el = ' <div class="btn-group"><a href="#" class="btn btn-info" >Action</a><a href="#" class="btn btn-info dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></a>' +
                        '<ul class="dropdown-menu"><li><a href="javascript:void(0)" ng-click="openUpdateDialog($event)" data-project="' + encodeURI(JSON.stringify(rowData)) + '">Update</a></li><li><a href="javascript:void(0)" data-id="' + rowData.Id + '" ng-click="removeObject($event)">Delete</a></li></ul></div>';
                    $(td).append($compile($el)($scope));
                }
            }
        ]
    });
    $('#saveBtn').click(function (e) {

        if (!$('#engineerForm').valid())
            return false;

        if ($("#AspNetUsers").val() == "")
        {
            errorAlert("please select users");
            return;
        }

        e.preventDefault();
        var data = {};
        $("#project_div :input").serializeArray().map(function (x) {
            if (x.name.indexOf("AspNetUsers") > -1) x.name = "AspNetUsers";
            if (data[x.name] != null)
                data[x.name] += "," + x.value;
            else
                data[x.name] = x.value;
        });
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

    $('#projectModal').on('hidden.bs.modal', function () {
        $('#project_div').find("input[type=text],input[type=hidden],select,textarea").val("");
        $('#project_div').find(".selectpicker").selectpicker('refresh');
    });
    }
    
})();

