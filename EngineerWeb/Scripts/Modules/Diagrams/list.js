(function () {
    'use strict';

    angular
        .module('app')
        .controller('controller', controller);

    controller.$inject = ['$scope', '$compile'];
    function controller($scope, $compile) {
    }
});
var diagramJSON;
var userStoriesTable;
var injected = false;
(function () {
    'use strict';
    angular
    .module('app')
    .controller('controller', controller)
    // directive for loading the selectpicker
    .directive("userStories", ['$timeout', function ($timeout) {
        return {
            link: function ($scope, element, attrs) {
                $scope.$on('dataloaded', function () {
                    $timeout(function () {
                        $("#userStories").selectpicker('refresh');
                        $('#shareModal').modal('show');
                    }, 0, false);
                })
            }

        }
    }]);


        controller.$inject = ['$scope', '$compile', '$http'];
        function controller($scope, $compile, $http) {
        $scope.retrieveStoriesToShare = function ($diagram) {
            var diagramJSON = { attachId: $($diagram).data("id") };
            $http.post(retreiveStoriesURL, JSON.stringify(diagramJSON))
            .success(function (result) {
                $scope.diagram = {
                    story: null,
                    stories: result.d,
                    id: $($diagram).data("id"),
                    userStoryId: $($diagram).data("userstoryid")
                };
                $scope.$broadcast('dataloaded');
            })
            .error(function (message) {
                errorAlert(message.responseJSON.Message);
            })
        }
        $scope.shareStory = function () {
            var data = {};
            $("#share_div :input").serializeArray().map(function (x) {
                if (data[x.name] != null)
                    data[x.name] += "," + x.value;
                else
                    data[x.name] = x.value;
            });
            $http.post(shareURL, JSON.stringify({"diagram":data}))
            .success(function (result) {
                $('#shareModal').modal('hide');
                formsTable.ajax.reload();
            })
            .error(function (message) {
                errorAlert(message.responseJSON.Message);
            });
        }
    }
   
})();
function openShareDialog($diagram) {
    var scope = angular.element(document.getElementById('controllerDiv')).scope();
    scope.$apply(function () {
        scope.retrieveStoriesToShare($diagram);
    });
};
var diagramId = 0;
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
                d.diagram = { attachId: diagramId, userStoryId: $($project).data("userstoryid") };
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
function diagramAction($diagram,action)
{
    var params = { "diagram": { attachId: $($diagram).data("id"), userStoryId: $($diagram).data("userstoryid") } };
    bootbox.confirm("Are you sure?", function (result) {
        if (result) {
            $.ajax({
                type: "POST",
                url: action == "Open" ? openURL : closeURL,
                data: JSON.stringify(params),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    successAlert("Success", "Request Success");
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
                "data": "UserStory.name"
            },
            {
                "data": "Attachment.name",
                "render": function (data, type, full, meta) {
                    return ' <div class="btn-group"><a href="#" class="btn btn-info" >Action</a><a href="#" class="btn btn-info dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></a><ul class="dropdown-menu">' +
                        (!full.readonly ? '<li><a href="javascript:void(0)" onclick="window.location.href = \'New?id=' + full.attachId + '&userStoryId=' + full.userStoryId + '\'" >Update</a></li><li><a href="javascript:void(0)" data-id="' + full.attachId + '" data-userStoryId="' + full.userStoryId + '" onclick="openRemoveDialog(this)">Delete</a></li>' +
                        '<li><a href="javascript:void(0)" data-id="' + full.attachId + '" data-userStoryId="' + full.userStoryId + '" onclick="openShareDialog(this)">Share</a></li>' : ' ') +
                        (full.state == "CLOSED" ? '<li><a href="javascript:void(0)" onclick="diagramAction(this,\'Open\')" data-id="' + full.attachId + '" data-userStoryId="' + full.userStoryId + '" >Open</a></li>' : '<li><a href="javascript:void(0)" onclick="diagramAction(this,\'Close\')" data-id="' + full.attachId + '" data-userStoryId="' + full.userStoryId + '" >Close</a></li>') +
                        '</ul></div>';
                }
            }
        ]
    });
    $('#deleteBtn').click(function (e) {

        var box = bootbox.confirm("You are going to delete, Proceed?", function (result) {
            if (result) {
                box.modal("hide");
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
            }
            else
                box.modal("hide");
        });
        
    });
    $("#addDiagram").on("click", function (e) {
        window.location.href = "New";
    });
    $('#shareModal').on('hidden.bs.modal', function () {
        $('#share_div').find("input[type=text],input[type=hidden],select,textarea").val("");
        $('#share_div').find(".selectpicker").selectpicker('refresh');
    });
});