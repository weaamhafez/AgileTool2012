var _selItem = null;
var _previewTemplates = [];
var _propTemplates = [];
var _snippetsTemplates = [];
var count = 0;
var order = 0;
var graph = new joint.dia.Graph;
var paper;
var rect;
var actCounter = 1;
var selected;
function initInteractions() {
    loadTemplates();
    $("#preview").sortable({
        revert: false,
        receive: function (event, ui) { //Preview Area can receive elements either from Toolbox or Fieldset
            var isItemFromToolbox = ui.sender.closest("#toolbox").length == 1;
            if (isItemFromToolbox) {
                //var type = ui.item.data("type");
                var currentItem = $("#preview .toolboxItem");
                addToolboxItemToPreviewArea(currentItem);
            }
        } // end receive function
    }); // end sortable

    $(".toolboxItem").draggable({
        connectToSortable: ".sortable",
        revert: false,
        helper: "clone"
    }); // end draggable
    $(".toolboxItem").disableSelection();
}
function loadTemplates()
{
    var templates = JSON.parse($("#" + templatesClientId + "").val());
    _previewTemplates = templates["Preview"];
    _snippetsTemplates = templates["Snippets"];
    $.each(templates['Prop'], function (key, val) {
        var tempFn = doT.template(val, undefined, _snippetsTemplates);
        _propTemplates[key] = tempFn;
    });
    
}
function addToolboxItemToPreviewArea(currentItem) {
    var type = currentItem.data("type");
    currentItem.removeClass("col-xs-6");
    currentItem.removeClass("toolboxItem");
    currentItem.addClass("previewItem");
    currentItem.html(_previewTemplates[type]);

    //var ctrlName = "ctrl" + (count++);
    //var json = {
    //    "ctrlName": ctrlName,
    //    "order": "" + (order++),
    //    "label":"activity" + actCounter++
    //};
    //currentItem.data("prop", json);


    //attachEventToPreviewItem(currentItem);
}
function saveCurrentSelItemProp() {
    if (_selItem != null) {
        var jsonProp = $("#prop :input").serialize();
        _selItem.data("prop", jsonProp);
    }
}

function deleteSelContol() {
    if (selected != null)
        selected.remove();
}

function changeActivityLabel($input)
{
    if (selected != null)
        paper.getModelById(selected.id).prop("attrs/text/text", $($input).val());
}
function save(type) {

    //type = typeof type !== 'undefined' ? type : 'create';

    var result = $('#engineerForm').valid();
    if (result == false) {
        errorAlert('Your Form contains errors, please fix them before saving');
        return;
    }
    if ($("#UserStoriesList").val() == null || $("#UserStoriesList").val() == "")
    {
        errorAlert("Please select at least 1 user story");
        return;
    }

    // Save currently open properties menu
    //saveCurrentSelItemProp();
    var graphStr = JSON.stringify(graph);
    var graphJSON = JSON.parse(graphStr);
    var svgDoc = paper.svg;
    var serializer = new XMLSerializer();
    var svgString = serializer.serializeToString(svgDoc);
    var diagram = {
        diagram:{
            name: $("#diagramName").val(),
            userStories:$("#UserStoriesList").val(),
            graph: graphStr,
            svg: svgString,
            Id: $("#diagramID").val()
        }
        
    };
    
    $.ajax({
        url: saveOrUpdateURL,
        type: 'POST',
        data: JSON.stringify(diagram),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (formId) {
            successAlert('Your Diagram have been successfully Saved');

            if ($("#diagramID").val() == "") {
                window.location.href = 'New?id=' + formId.d;
            }
        },
        error: function (jqXHR, err) {
            var status = jqXHR.status;
            if (status == 403 || status == 401)
                warningAlert("Session Expired please relogin");
            else
                errorAlert(jqXHR.responseText);
        }
    });
}
function openRenameFormDialog() {
    $('.modalDialog').load('dialogs/rename-form.html', function (modal) {
        $('#modal').modal('show');
    });
}
function loadGraph()
{
    var graphStr = $("#diagramGraph").val();
    if (graphStr != "")
    {
        var graphJSON = JSON.parse(graphStr);
        if (paper == null) {
            paper = new joint.dia.Paper({
                el: $('#preview'),
                width: 600,
                height: 1000,
                model: graph,
                gridSize: 1
            });
        }
        graph = graph.fromJSON(graphJSON);
        rect = graph.attributes.cells.models[graph.attributes.cells.models.length - 2];
    }
}
$(document).ready(function () {
    initInteractions();
    loadGraph();
    $("#dialogNameTitle").text($("#diagramName").val());
    $(document).on('click', '#toolbox .btn', function (e) {
        e.preventDefault();
    });
    $("#renameBtn").on("click", function () {
        if($('#engineerForm').valid())
        {
            $("#dialogNameTitle").text($("#diagramName").val());
            $('#renameModal').modal('hide');
        }
    });

}); //end ready