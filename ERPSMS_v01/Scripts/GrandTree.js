
/// <reference path="jquery-1.4.1.js" />

///<summary>To call the event </summary>
function MakeTree(id, header) {
    var url = $(id).attr("ajaxURL");
    $.get(url, function (data) {
        if (data) {
            for (var i in data.TreeDataTable) {
                if (data.TreeDataTable[i] == null) {
                    data.TreeDataTable.splice(i, 1);
                }
            }
            BindJsonTree(data, id, header);
        }
    });
}

///<summary>Render Json Tree </summary>
function BindJsonTree(data, id, header) {
    var checkedArr = new Array();
    var jHtml = "<ul style='width:100%' class='treeview'><img id='imgHeader' src='Images/ui/tree-minus.png' style='margin-right:4px;' onclick='javascript:CollapseAllChild(this);' /><span>" + header + "</span>";
    for (var i in data.TreeDataTable) {
        if (data.TreeDataTable[i] != null) {
            var parentTree = data.TreeDataTable[i];
            if (i == data.TreeDataTable.length - 1)
                jHtml += "<li class='last' id='li_" + parentTree.attributes.id + "'><img src='Images/ui/tree-minus.png' onclick='javascript:CollapseChild(this);' /><input style='margin-left:2px' type='checkbox' id='chkParent_" + parentTree.attributes.id + "' onclick='javascript:CheckAllChild(this);' /><span>" + parentTree.data + "</span>";
            else
                jHtml += "<li class='expandable' id='li_" + parentTree.attributes.id + "'><img src='Images/ui/tree-minus.png' onclick='javascript:CollapseChild(this);' /><input style='margin-left:2px' type='checkbox' id='chkParent_" + parentTree.attributes.id + "' onclick='javascript:CheckAllChild(this);' /><span>" + parentTree.data + "</span>";
            if (parentTree.children != null)
                jHtml += "<ul>";
            for (var k in parentTree.children) {
                var clildTree = parentTree.children[k];
                jHtml += "<li id='li_" + clildTree.attributes.id + "' " + (k == (parentTree.children.length - 1) ? "class = 'last'" : '') + "><input style='margin-left:5px' onclick='javascript:CheckParent(this);' type='checkbox' " + (clildTree.attributes.selected == "1" ? "checked='checked'" : "") + "  id='chkChlid_" + clildTree.attributes.id + "' /> <span>" + clildTree.data + "</span></li>";
                if (clildTree.attributes.selected == "1")
                    checkedArr.push({ ID: "chkChlid_" + clildTree.attributes.id });
            }
            if (parentTree.children != null) jHtml += "</ul>";
            jHtml += " </li>";
        }
    }
    jHtml += "</ul>";
    $(id).html("");
    $(id).append(jHtml);
    for (var j in checkedArr) {
        CheckParent($("#" + checkedArr[j].ID));
    }
}

///<summary>if the all child node checked / unchecked the parent node will be checked/unchecked  </summary>
function CollapseChild(img) {
    $(img).parent().children('ul').find('li').slideToggle(20);
    var cssClass = $(img).parent("li").attr("class");
    if (cssClass != "last") {
        if (cssClass == "expandable")
            $(img).parent("li").attr("class", "collapsable");
        else
            $(img).parent("li").attr("class", "expandable");
    }
    var src = $(img).attr("src");
    src = src.substr(src.lastIndexOf("/") + 1, src.length);
    if (src == "tree-minus.png")
        $(img).attr("src", "Images/ui/tree-plus.png");
    else
        $(img).attr("src", "Images/ui/tree-minus.png");
}

///<summary>Collapse All its child </summary>
function CollapseAllChild(img) {
    $(img).parent().find('li').slideToggle(20);
    var src = $(img).attr("src");
    src = src.substr(src.lastIndexOf("/") + 1, src.length);
    if (src == "tree-minus.png")
        $(img).attr("src", "Images/ui/tree-plus.png");
    else
        $(img).attr("src", "Images/ui/tree-minus.png");
}
