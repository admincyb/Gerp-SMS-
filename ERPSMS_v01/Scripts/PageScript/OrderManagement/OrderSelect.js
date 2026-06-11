/// <reference path="../jquery/jquery-1.5.min.js" />

$(document).ready(function () {
    var srchVal = "SucNem";
    var fromDate = "";
    var toDate = "";
    $("#divOrders").attr("ajaxURL", "OrderManagement.do?Action=GetOrders&SearchType=" + srchVal + "&FromDate=" + fromDate + "&ToDate=" + toDate);
    MakeTree($("#divOrders"), "Customer Name");
});

///#region ----- Core Section ------------------

///<summary>in here order array for order if u use it is in another purpose please rename its you should call these 2 function in ur page </summary>
orderArray = new Array();

///<summary>Function used to add the order detiails into array </summary>
function CheckAllChild(chk) {
    $(chk).parent().children('ul').find('li').each(function () {
        var orderID = $(this).find("input").attr("id");
        orderID = orderID.substr(orderID.lastIndexOf("_") + 1, orderID.length);
        var orderCode = $(this).find("span").html();
        if ($(chk).attr("checked")) {
            $(this).find("input").attr("checked", true);
            var isExists = false;
            for (var i = 0; i < orderArray.length; i++) {
                if (orderID == orderArray[i].orderID)
                    isExists = true;
            }
            if (!isExists)
                orderArray.push({ orderID: orderID, orderCode: orderCode });
        }
        else {
            $(this).find("input").attr("checked", false);
            for (var k = 0; k < orderArray.length; k++) {
                if (orderID == orderArray[k].orderID) {
                    orderArray.splice(k, 1);
                }
            }
        }
    });
}
///<summary>Function BindGrid </summary>
function CheckParent(chk) {
    var orderID = $(chk).attr("id");
    orderID = orderID.substr(orderID.lastIndexOf("_") + 1, orderID.length);
    var orderCode = $(chk).parent("li").find("span").html();
    if ($(chk).attr("checked")) {
        var isChecked = true;
        $(chk).parent().parent('ul').find('li').each(function () {
            if (!$(this).find("input").is(":checked")) {
                isChecked = false;
            }
        });
        if (isChecked) {
            $(chk).parent().parent('ul').parent("li").find("input").attr("checked", true);
        }
        var isExists = false;
        for (var i = 0; i < orderArray.length; i++) {
            if (orderID == orderArray[i].orderID)
                isExists = true;
        }
        if (!isExists)
            orderArray.push({ orderID: orderID, orderCode: orderCode });
    }
    else {
        $(chk).parent().parent('ul').parent("li").find("input:first").attr("checked", false);
        for (var k = 0; k < orderArray.length; k++) {
            if (orderID == orderArray[k].orderID) {
                orderArray.splice(k, 1);
            }
        }
    }
}

///#endregion

///#region----- Multi Level Tree ----------------

///#region------ Core Section ----------------

///<summary>Function call a function </summary>
function ShowDiv() {
    $.get("Handler.ashx", function (data) {
        BindTree(data);
    });
    return false;
}

///#endregion

///#region------ Render Section ----------------
///<summary>Function render parent session </summary>
function BindTree(data) {
    var jTree = "<ul class='treeview'>";
    for (var i in data) {
        if (data[i].parentid == "0") {
            var chidTree = BindChildTree(data, data[i].id);
            jTree += "<li class='expandable' id='li" + data[i].id + "'>" + (chidTree != "" ? "<img id='imgHeader' src='Images/ui/tree-minus.png' style='margin-right:4px;' onclick='javascript:ExpandCollapseChild(this);' />" : "<img id='imgHeader' src='Images/ERP-Blue/tree-line.png' style='margin-right:4px;'/>") + "<input type='checkbox' id='checkbox_" + data[i].id + "' onclick='javascript:CheckUnCheckChild(this)' /><span>" + data[i].name + "</span>" + (chidTree) + "</li>";
        }
    }
    jTree += "</ul>";
    $("#divTree").html(jTree);
}
///<summary>Function render child session </summary>
function BindChildTree(data, id) {
    var jTree = "<ul class='treeview'>";
    var exists = false;
    for (var i in data) {
        if (data[i].parentid == id) {
            exists = true;
            var chidTree = BindChildTree(data, data[i].id);
            jTree += "<li class='expandable' id='li" + data[i].id + "'>" + (chidTree != "" ? "<img id='imgHeader' src='Images/ui/tree-minus.png' style='margin-right:4px;' onclick='javascript:ExpandCollapseChild(this);' />" : "<img id='imgHeader' src='Images/ERP-Blue/tree-line.png' style='margin-right:4px;'/>") + "<input type='checkbox' id='checkbox_" + data[i].id + "' onclick='javascript:CheckUnCheckChild(this)' /><span>" + data[i].name + "</span>" + (chidTree) + "</li>";
        }
    }
    jTree += "</ul>";
    return exists == true ? jTree : "";
}

///#endregion

///#region ----- Utility Methods ------------------

///<summary>Function used expand or collapse child tree </summary>
function ExpandCollapseChild(img) {
    $(img).parent().find("li").toggle();
    var src = $(img).attr("src");
    if (src == "tree-minus.png")
        $(img).attr("src", "Images/ui/tree-plus.png");
    else
        $(img).attr("src", "Images/ui/tree-minus.png");
}
///<summary>Function used checko or check all its child </summary>
function CheckUnCheckChild(chk) {
    var isEnter = true;
    $(chk).parent().children('ul').find('li').each(function () {
        var orderID = $(this).find("input").attr("id");
        orderID = orderID.substr(orderID.lastIndexOf("_") + 1, orderID.length);
        var orderCode = $(this).find("span").html();
        if ($(chk).attr("checked")) {
            $(this).find("input").attr("checked", true);
        }
        else {
            $(this).find("input").attr("checked", false);
        }
        isEnter = false;
    });
    CheckUnCheckParent(chk);
}
///<summary>Function used checko or check all its parents </summary>
function CheckUnCheckParent(chk) {
    var orderID = $(chk).attr("id");
    orderID = orderID.substr(orderID.lastIndexOf("_") + 1, orderID.length);
    var orderCode = $(chk).parent("li").find("span").html();
    if ($(chk).attr("checked")) {
        var isChecked = true;
        $(chk).parent().parent('ul').find('li').each(function () {
            if (!$(this).find("input").is(":checked")) {
                isChecked = false;
            }
        });
        if (isChecked)
            $(chk).parent().parent('ul').parent("li").find("input").attr("checked", true);
    }
    else {
        $(chk).parent().parent('ul').parent("li").find("input:first").attr("checked", false);
    }
}

///#endregion

///#endregion
