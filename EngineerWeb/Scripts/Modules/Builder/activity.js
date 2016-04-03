
$(document).ready(function () {

    if (paper == null)
    {
        paper = new joint.dia.Paper({
            el: $('#activityHolder'),
            width: 600,
            height: 1000,
            model: graph,
            gridSize: 1
        });
    }
    
    if (rect == null) {
        rect = new joint.shapes.basic.Rect({
            position: { x: 100, y: 30 },
            size: { width: 100, height: 30 },
            attrs: { rect: { fill: 'green' }, text: { text: 'activity' + actCounter++, fill: 'white' } }
        });

        graph.addCells([rect]);
        
        
    }
    else {
        var rect2 = rect.clone();
        rect2.attributes.attrs["text"].text = 'activity' + actCounter++;
        rect2.translate(0,100);

        var link = new joint.dia.Link({
            source: { id: rect.id },
            target: { id: rect2.id }
        });
        graph.addCells([rect2,link]);

        rect = rect2;
        
    }
    attachItemsEvent();

    // on selecting activity
    paper.on('cell:pointerclick', function (cellView, evt, x, y) {
        saveCurrentSelItemProp();
        selected = cellView.model;
        var activityId = cellView.model.id;
        var selItem = $($("#preview").children().find(".viewport").children()).filter(function () { return $(this).attr("model-id") == activityId; });
        if (selItem.length > 0)
        {
            var type = $(selItem).data("type");
            _selItem  = $(selItem);
            var tempFn = _propTemplates[type];
            var result = tempFn(_selItem.data("prop"));
            $("#prop").html(result);
            $("#preview .selectedControl").removeClass("selectedControl");
            $(this).addClass("selectedControl");
        }
            
    });
    /////////////////////////////////////////////////
});
function attachItemsEvent() {
    var previewItem = $("#preview").children().find(".viewport");
    var activities = $(previewItem).children();
    // var allCells = paper.getEmbeddedCells();
    for (var i = 0; i < activities.length ; i++) {
        var selItem = $(activities)[i];
        var modelId = $(selItem).attr("model-id");
        if (paper.getModelById(modelId).attributes.type === "link")
            continue;

        if ($(selItem).length > 0) {
            var ctrlName = "ctrl" + (count++);
            var json = {
                "ctrlName": ctrlName,
                "order": "" + (order++),
                "label": paper.getModelById(modelId).attributes.attrs.text.text
            };
            $(selItem).data("prop", json);
            $(selItem).data("type", "activity");
        }
    }

}
//# sourceURL=activity.js