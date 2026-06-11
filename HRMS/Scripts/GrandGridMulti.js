/// <reference path="jquery/jquery-1.5.min.js" />

var _objBindQue = new Object();
_objBindQue.GridID = "";
_objBindQue.Data = "";

var GrandGrid = new Object();
var BindQueue = new Array();

// type: GrandGrid.GridStructure
GrandGrid.GridCollection = new Array();
GrandGrid.Utilities = new Object();

GrandGrid.AjaxInProgress = false;

// -- The Grid Header Object (the thead section)
GrandGrid.GridHeaderStructure = function () {
    this.Columns = new Array();
    this.IsRendered = false; 
}

// --- Constructor for GridHeaderColumn
GrandGrid.GridHeaderColumn = function (HeaderText, Field, Sortable, Alignment, Type, Visible, Template, Width) {
    this.Field = Field;
    this.HeaderText = HeaderText;
    this.Sortable = Sortable;
    this.Alignment = Alignment;
    this.Type = Type;
    this.Visible = Visible;
    this.Template = Template;
    this.Width = Width;
}

// --- The Grid Object
GrandGrid.GridStructure = function () {
    this.GridID = "";
    this.GridHeader = new GrandGrid.GridHeaderStructure();
    this.GridRows = new Array();
    this.AjaxURL = "";
    this.AjaxPageSize = "20";
    this.CurrentPage = 1;
    this.SortColumn = '';
    this.SortOrder = '';
    this.Paging = false;
    this.DisplayColumns = new Array();
    this.Editable = false;
    this.EditTemplate = "";
    this.EditTemplateWidth = "15%";
    this.EnableCheckBox = false;
    this.Data = null;
}


// #region Utility Methods

GrandGrid.Utilities.AddTemplateColumn = function (gGridObj) {
    var Template = gGridObj.EditTemplate;
    return Template;
}
// -- Builds the URL params for the ajax request like PageSize, Page etc.
GrandGrid.Utilities.BuildURLParams = function (gGridObject) {
    var params = '';
    params += "&PageSize=" + gGridObject.AjaxPageSize;
    params += "&Page=" + gGridObject.CurrentPage;
    if (gGridObject.DisplayColumns.length > 0) {
        params += "&Columns=";
        for (i = 0; i < gGridObject.DisplayColumns.length; i++) {
            params += gGridObject.DisplayColumns[i].toString() + ",";
        }
        if (gGridObject.SortColumn != '') {
            params += "&SortColumn=" + gGridObject.SortColumn;
        }
        if (gGridObject.SortOrder != '') {
            params += "&SortOrder=" + gGridObject.SortOrder;
        }
        if (gGridObject.SortColumn != '') {
            params += "&SolrtColumn=" + gGridObject.SortColumn;
        }
    }
    return params;
}
// --- Call to sort a sortable column
GrandGrid.Utilities.GridSort = function (column) {
    var sortOrd = "Desc";
    if ($(column).find('div').attr('class') == "sortableUp") {
        sortOrd = "Asc";
    }
    var grdID = $(column).parents("table:eq(0)").attr("id");
    GrandGrid.BuildGridStructure(document.getElementById(grdID), 1, $(column).attr("FieldMap"), sortOrd);
}
// --- Sets the grid attributes specified in the grid definition
GrandGrid.Utilities.SetGridAttributes = function (grd, gGridObject, page, sort, sortOrder) {
    if (!gGridObject.GridHeader.IsRendered) {
        if ($(grd).attr("ajaxURL")) {
            //gGridObject.AjaxURL = decodeURIComponent($(grd).attr("ajaxURL"))
            gGridObject.AjaxURL = $(grd).attr("ajaxURL");
        }
        if ($(grd).attr("paging")) {
            if ($(grd).attr("paging") == "true") {
                gGridObject.Paging = true;
            }
        }
        if ($(grd).attr("pageSize")) {
            gGridObject.AjaxPageSize = $(grd).attr("pageSize");
        }
        if ($(grd).attr("editable")) {
            if ($(grd).attr("editable").toString().toLocaleLowerCase() == "true") {
                gGridObject.Editable = true;
                if ($(grd).find('th[type=Template]').length > 0) {
                    if (!gGridObject.EditTemplate) {
                        gGridObject.EditTemplate = $(grd).find('th[type=Template]:last').html();
                        gGridObject.EditTemplateWidth = $(grd).find('th[type=Template]:last').attr("width") ? $(grd).find('th[type=Template]:last').attr("width") : gGridObject.EditTemplateWidth;
                    }
                    $(grd).find('th[type=Template]').remove();
                }
            }
        }
        if ($(grd).attr("enablecheckbox")) {
            if ($(grd).attr("enablecheckbox").toString().toLowerCase() == "true") {
                gGridObject.EnableCheckBox = true;
            }
        }
    }
    if (!page)
        gGridObject.CurrentPage = "1";
    else
        gGridObject.CurrentPage = page.toString();
    if (sort)
        gGridObject.SortColumn = sort.toString();
    if (sortOrder)
        gGridObject.SortOrder = sortOrder.toString();
}
// -- toggle the down and up arrow based on sort order
GrandGrid.Utilities.ToggleSortOrder = function (grid, sort, sortOrder) {
    if (sort && sortOrder) {
        var th = $(grid).find('th[FieldMap=' + sort + '] div');
        $(th).attr("class", function () {
            if (sortOrder.toString().toLowerCase() == 'desc') {
                $(th).removeAttr("class");
                $(th).addClass("sortableUp");
            }
            else {
                $(th).removeAttr("class");
                $(th).addClass("sortableDown");
            }

        });
    }
}
GrandGrid.Utilities.gGridCheckAllSelect = function (chkBox) {

    var tbl = $(chkBox).parents('table:eq(0)');

    var isChecked = true;
    if ($(chkBox).is(":checked")) {
        if ($(tbl).find("tbody input[type=checkbox]:not(:checked)").length == 0) { // if any one of the tr checkbox is uncheked then thead checkbox unchecked
            $(tbl).find("thead input[type=checkbox]").attr("checked", "checked");
        }
    }
    else {
        $(tbl).find("thead input[type=checkbox]").removeAttr("checked");
    }
    if (typeof CheckBoxClickTrigger == 'function') {
        CheckBoxClickTrigger($(tbl).attr("id"));
    }
}

GrandGrid.Utilities.gGridSelectAllCheckBoxes = function (chkBox) {

    var tbl = $(chkBox).parents('table:eq(0)');
    if ($(chkBox).is(":checked")) {
        $(tbl).find(":checkbox").attr("checked", "checked");
    }
    else {
        $(tbl).find(":checkbox").removeAttr("checked");
    }
}
// -- Not Yet Used !!
GrandGrid.Utilities.SetGridConfiguration = function (btn, gId) {
    var confDiv = $(btn).parents("div:eq(0)");
    var arrCheckBox = $(confDiv).find(":checkbox");
    for (i = 0; i < arrCheckBox.length; i++) {
        var th = $("#" + gId + " th:contains('" + $(arrCheckBox[i]).attr("value") + "'):eq(0)");
        if ($(arrCheckBox[i]).is(":checked")) {
            $(th).attr("isvisible", "true");
        }
        else {
            $(th).attr("isvisible", "false");
        }
    }
    GrandScriptUtils.HideModalWindow();
    var eTemplate = gGridObject.EditTemplate;
    var eTemplateWidth = gGridObject.EditTemplateWidth;
    resetGridObject(true);
    gGridObject.EditTemplate = eTemplate;
    gGridObject.EditTemplateWidth = eTemplateWidth;
    makeGrandGrid(document.getElementById(gId));
}
// -- Returns the column value for the specified row and field
GrandGrid.Utilities.GetColumnValue = function (Row, Field, GridID) {
    var index;
    var gGridObject = GrandGrid.Utilities.GetGridObject(GridID);
    if (Row && Field && gGridObject) {
        for (var column in gGridObject.GridHeader.Columns) {
            var col = gGridObject.GridHeader.Columns[column];
            if (col.Field.toLowerCase() == Field.toLowerCase()) {
                index = column;
                break;
            }
        }
        if (column != undefined) {
            return $.trim($(Row).find("td:eq(" + column + ")").text());
        }
        else {
            return "";
        }
    }
    else {
        return "";
    }
}
// -- Returns the column index for the specified row and field
GrandGrid.Utilities.GetColumnIndex = function (Row, Field, GridID) {
    var index = null;
    var gGridObject = GrandGrid.Utilities.GetGridObject(GridID);
    if (Row && Field && gGridObject) {
        for (var column in gGridObject.GridHeader.Columns) {
            var col = gGridObject.GridHeader.Columns[column];
            if (col.Field.toLowerCase() == Field.toLowerCase()) {
                index = column;
                break;
            }
        }
    }
    return index;
}
// -- Returns the Grid Object for the specified grid id
GrandGrid.Utilities.GetGridObject = function (GridID) {
    for (var GrdIndex in GrandGrid.GridCollection) {
        if (GrandGrid.GridCollection[GrdIndex].GridID == GridID) {
            return GrandGrid.GridCollection[GrdIndex];
        }
    }

    if (!GridID && GrandGrid.GridCollection.length > 0) {
        return GrandGrid.GridCollection[0];
    }

    return null;
}

// -- Returns the Grid Object Data for the specified grid id
GrandGrid.Utilities.GetGridData = function (GridID) {
    for (var GrdIndex in GrandGrid.GridCollection) {
        if (GrandGrid.GridCollection[GrdIndex].GridID == GridID) {
            return GrandGrid.GridCollection[GrdIndex].Data;
        }
    }

    if (!GridID && GrandGrid.GridCollection.length > 0) {
        return GrandGrid.GridCollection[0].Data;
    }

    return null;
}
// -- Resets the grid
GrandGrid.Utilities.ResetGrid = function (PreserveEditTemplate, GridID) {
    var eTemplate = "";
    var eTemplateWidth = "15%";
    var grdObj = GridID ? GrandGrid.Utilities.GetGridObject(GridID) : GrandGrid.Utilities.GetGridObject();
    if (grdObj) {
        if (PreserveEditTemplate) {
            eTemplate = grdObj.EditTemplate;
            eTemplateWidth = grdObj.EditTemplateWidth ? grdObj.EditTemplateWidth : eTemplateWidth;
        }
    }
    grdObj = new GrandGrid.GridStructure();
    grdObj.GridID = GridID;
    grdObj.EditTemplate = eTemplate;
    grdObj.EditTemplateWidth = eTemplateWidth;
    GrandGrid.Utilities.UpdateGridObject(grdObj);
}
// -- Updates the stored grid object in the grid object array with the new grid object
GrandGrid.Utilities.UpdateGridObject = function (grdObject) {
    for (var grdIndex in GrandGrid.GridCollection) {
        if (GrandGrid.GridCollection[grdIndex].GridID == grdObject.GridID) {
            GrandGrid.GridCollection[grdIndex] = grdObject;
        }
    }
}
// -- Function will check for any pending bindings to happen
GrandGrid.Utilities.CheckPendingBinds = function (GridID) {
    GrandGrid.AjaxInProgress = false;
    if (BindQueue.length > 0) {
        for (var i = 0; i < BindQueue.length; i++) {
            if (BindQueue[i].GridID == GridID) {
                BindQueue.splice(i, 1);
            }
        }
        if (BindQueue.length > 0) {
            GrandGrid.MakeGrid(document.getElementById(BindQueue[0].GridID), 1, BindQueue[0].Data);
        }
    }
}
// -- Shows grid with different row style for the alternative rows
// -- Use the trodd and treven CSS classes for it.
GrandGrid.Utilities.ShowAlternativeRowStyle = function () {
    for (i = 0; i < BindQueue.length;i++ ) {
        $("#" + BindQueue[i].GridID).find("tr:odd").addClass("trodd");
        $("#" + BindQueue[i].GridID).find("tr:even").addClass("treven");
    }
}
// #endregion

// -- The entry point to the grid
// -- Can use "AjaxURL" or direct data for binding
// -- if use direct data, use page as 1
GrandGrid.MakeGrid = function makeGrandGrid(grd, page, data) {

    var tempGridObj = new GrandGrid.GridStructure();
    var flag = false;

    if (typeof (grd) == "string") {
        grd = $("#" + grd);
    }

    tempGridObj.GridID = $(grd).attr("id");

    if (GrandGrid.Utilities.GetGridObject(tempGridObj.GridID) != null) {
        var GridOriginal = GrandGrid.Utilities.GetGridObject(tempGridObj.GridID)
        GridOriginal.Data = data;
        GrandGrid.Utilities.UpdateGridObject(GridOriginal);
    }
    else {
        if (data == undefined) {
            tempGridObj.Data = null;
        }
        else {
            tempGridObj.Data = data;
        }
        GrandGrid.GridCollection.push(tempGridObj);
    }

    for (var i = 0; i < BindQueue.length; i++) {
        if (BindQueue[i].GridID == $(grd).attr("id")) {
            if (BindQueue.length > 0) {
                flag = true;
                GrandGrid.BuildGridStructure(grd, page, 0, 0, data);
            }
        }
    }

    if (!flag) {
        _objBindQue = new Object();
        _objBindQue.Data = data;
        _objBindQue.GridID = $(grd).attr("id");
        BindQueue.push(_objBindQue);
        GrandGrid.BuildGridStructure(grd, page, 0, 0, data);
    }
}

// Main entry point method - Internal
GrandGrid.BuildGridStructure = function (grd, page, sort, sortOrder, data) {

    // #region Objects and Constructors------
    var gGridObject = GrandGrid.Utilities.GetGridObject($(grd).attr("id"));
    var gGridHeaderObject = gGridObject.GridHeader;

    // Set the attributes defined in the HTML declaration
    GrandGrid.Utilities.SetGridAttributes(grd, gGridObject, page, sort, sortOrder);
    // #endregion

    // #region --------- map the headers if already defined map it for the first
    // time else skip -----------
    if (gGridObject.GridHeader.Columns.length == 0 && gGridObject.GridHeader.IsRendered == false) {
        if ($(grd).find("thead")) {
            if (gGridObject.EnableCheckBox) {
                var thCheck = document.createElement("th");
                $(thCheck).attr({ "type": "template",
                    "FieldMap": "",
                    "align": "left",
                    "width": "20px"
                })
                .css("width", "20px")
                .html("<input type=\"checkbox\" />");
                $(thCheck).insertBefore($(grd).find("th:first"));
                $(grd).find("thead").find("input[type=checkbox]").attr("onclick", "GrandGrid.Utilities.gGridCheckAllSelect(this);");
                $(grd).removeAttr("enablecheckbox");
            }
            else { // if all ready renderd need onclick attr to set like this
                $(grd).find("thead").find("input[type=checkbox]").attr("onclick", "GrandGrid.Utilities.gGridCheckAllSelect(this);");
            }
            $(grd).find("th").each(function () {
                var fieldMap = "";
                var sortable = false;
                var align = "center";
                var visible = true;
                var width = "";
                var type = "Normal";
                var template = "";
                var text = "";

                // #region ------ Attributes Mapping -------------------

                // ---- Field--
                if ($(this).attr("FieldMap") != undefined) {
                    if ($(this).attr("FieldMap")) {
                        fieldMap = $(this).attr("FieldMap");
                    }
                }

                // ---- Sortable--
                if ($(this).attr("sortable") != undefined) {
                    if ($(this).attr("sortable").toString().toLowerCase() == "true") {
                        sortable = true;
                    }
                }

                // ---- Visible--
                if ($(this).attr("isvisible") != undefined) {
                    if ($(this).attr("isvisible").toString().toLowerCase() == "false") {
                        visible = false;
                    }
                }

                // ---- Alignment--
                if ($(this).attr("TempAlign") != undefined) {
                    align = $(this).attr("TempAlign");
                }
                else if ($(this).attr("align") != undefined) {
                    if ($(this).attr("align").toString().toLowerCase() != "center") {
                        align = $(this).attr("align");
                    }
                }

                // ---- Width --
                if ($(this).attr("width") != undefined) {
                    if ($(this).attr("width").toString() != "") {
                        width = $(this).attr("width").toLowerCase();
                    }
                }

                // ---- Template--
                text = $(this).text();
                if ($(this).attr("type") != undefined) {
                    if ($(this).attr("type").toString().toLowerCase() == "template") {
                        type = "Template";
                        template = $(this).html();
                        if ($(this).find("span").length == 1) {  // if check that span aleady exists in th template
                            text = $(this).find("span").html();
                        }
                        else {
                            text = $(this).html();
                        }
                    }
                }

                // #endregion

                gGridHeaderObject.Columns.push(new GrandGrid.GridHeaderColumn(text, fieldMap, sortable, align, type, visible, template, width));
                gGridObject.DisplayColumns.push(fieldMap);
            });
            if (gGridObject.Editable) {
                gGridHeaderObject.Columns.push(new GrandGrid.GridHeaderColumn('', "", false, "left", "Template", true, gGridObject.EditTemplate, gGridObject.EditTemplateWidth));
            }
            gGridObject.GridHeader = gGridHeaderObject;
        }
    }
    // #endregion

    // check to see if data or ajax url is provided
    if (gGridObject.Data == null && gGridObject.AjaxURL.length < 1) {
        alert("You have to provide binding data or Ajax URL");
        return;
    }
    else {
        // if Ajax URL is provided
        if (gGridObject.Data == null && gGridObject.AjaxURL && (!GrandGrid.AjaxInProgress)) {
            GrandGrid.AjaxInProgress = true;
            var urlParams = GrandGrid.Utilities.BuildURLParams(gGridObject);
            var returnData = "";
            // For creating the grid format data if the provided data is not in the expected format
            var newData = new Object();
            $.ajaxSetup({ scriptCharset: "utf-8", contentType: "application/json; charset=utf-8" });
            //$.getJSON(gGridObject.AjaxURL + urlParams, 
            $('#updateProgress').show();
            $.getJSON(encodeURI(gGridObject.AjaxURL) + urlParams,
            function (returnData) {
                if (returnData) {
                    $('#updateProgress').hide();
                    // format the data to suit for the grid
                    newData.Rows = returnData.Table1;
                    newData.TotalResult = returnData.Table[0].Column1;
                    data = newData;
                    $(grd).css({ "visibility": "visible", "display": "block" });
                    GrandGrid.BuildRows(grd, gGridObject, sort, sortOrder, data);
                    if (gGridObject.Paging) {
                        GrandGrid.BuildPaging(data.TotalResult, grd, page, gGridObject.AjaxPageSize, sort, sortOrder);
                    }
                    GrandGrid.Utilities.ShowAlternativeRowStyle();
                    $(grd).find("thead").find("input[type=checkbox]").unbind("onclick");
                    $(grd).find("thead").find("input[type=checkbox]").click(function () { GrandGrid.Utilities.gGridSelectAllCheckBoxes(this); });
                    if (typeof AfterGridBind == 'function') {
                        AfterGridBind($(grd).attr("id"));
                    }
                    $("#divNodata").remove();
                    GrandGrid.Utilities.CheckPendingBinds(gGridObject.GridID);
                }
                else {
                    $(grd).css({ "visibility": "hidden", "display": "none" });
                    $(grd).find("tbody").html("");
                    $("#divNodata").remove();
                    $("<div id=\"divNodata\" class=\"nodata\" >Translate(NoDataFound)</div>").insertBefore($(grd));
                    GrandGrid.AjaxInProgress = false;
                    if (typeof AfterGridBindWithNoData == 'function') {
                        AfterGridBindWithNoData($(grd).attr("id"));
                    }
                    GrandGrid.Utilities.CheckPendingBinds(gGridObject.GridID);
                }
            });
        }
        else {
            if (gGridObject.Data) {
                var dataArr = new Object();
                dataArr.Rows = new Array();
                dataArr.Rows = gGridObject.Data;
                if (dataArr.Rows.length && dataArr.Rows.length > 0) {
                    dataArr.TotalResult = dataArr.Rows.length;
                    $(grd).css({ "visibility": "visible", "display": "block" });
                    GrandGrid.BuildRowsWithData(grd, gGridObject, dataArr);
                    GrandGrid.Utilities.ShowAlternativeRowStyle();
                    $(grd).find("thead").find("input[type=checkbox]").click(function () { GrandGrid.Utilities.gGridSelectAllCheckBoxes(this); });
                    //                    
                    // handle any functions specified after grid rendering
                    if (typeof AfterGridBind == 'function') {
                        AfterGridBind($(grd).attr("id"));
                    }
                    GrandGrid.Utilities.CheckPendingBinds(gGridObject.GridID);
                }
                else {
                    $(grd).css({ "visibility": "hidden", "display": "none" });
                    GrandGrid.Utilities.CheckPendingBinds(gGridObject.GridID);
                }
            }
        }
    }


    // ---- In Progress.

}

// #region ----------- Grid Render ----------------

GrandGrid.BuildRows = function (grid, gGridObj, sort, sortOrder, data) {
    var html = '';
    // ---- Render the Header Row

    if (!gGridObj.GridHeader.IsRendered) {
        // Remove it after saving the template
        $(grid).find("thead").parent().html("");
        html += "<thead>";
        for (HeaderCol = 0; HeaderCol < gGridObj.GridHeader.Columns.length; HeaderCol++) {
            var HeaderColumn = gGridObj.GridHeader.Columns[HeaderCol];
            if (!HeaderColumn.Visible) {
                html += "<th " + (HeaderColumn.Alignment == "right" ? "class=\"txtAlign-right\"" : "") + " FieldMap='" + HeaderColumn.Field.toString() + "' TempAlign='" + HeaderColumn.Alignment + "' isvisible='false' style='display:none; visible:hidden' type='" + HeaderColumn.Type.toString() + "' sortable='" + HeaderColumn.Sortable.toString() + "'><span>" + HeaderColumn.HeaderText + "</span></th>";
            }
            else {
                if (HeaderColumn.Type == "Template") {
                    html += "<th " + (HeaderColumn.Alignment == "right" ? "class=\"txtAlign-right\"" : "") + " FieldMap='" + HeaderColumn.Field + "' TempAlign='" + HeaderColumn.Alignment + "' isvisible='" + HeaderColumn.Visible.toString() + "' " + (HeaderColumn.Width != "" ? "style='text-align:" + HeaderColumn.Alignment + ";width:" + HeaderColumn.Width + "' width='" + HeaderColumn.Width + "'" : "") + " type='Template'><span>" + HeaderColumn.HeaderText + "</span></th>";
                }
                else {
                    if (!HeaderColumn.Sortable) {
                        html += "<th " + (HeaderColumn.Alignment == "right" ? "class=\"txtAlign-right\"" : "") + " FieldMap='" + HeaderColumn.Field.toString() + "' TempAlign='" + HeaderColumn.Alignment + "' isvisible='" + HeaderColumn.Visible.toString() + "' type='" + HeaderColumn.Type.toString() + "' " + (HeaderColumn.Width != "" ? "style='width:" + HeaderColumn.Width + "' width='" + HeaderColumn.Width + "'" : "") + "><span>" + HeaderColumn.HeaderText + "</span></th>";
                    }
                    else {
                        if (HeaderColumn.Type == "Normal") {
                            html += "<th "+ (HeaderColumn.Alignment=="right"? "class=\"txtAlign-right\"":"") +" onclick='javascript:GrandGrid.Utilities.GridSort(this);' TempAlign='" + HeaderColumn.Alignment + "'  isvisible='" + HeaderColumn.Visible.toString() + "' FieldMap='" + HeaderColumn.Field.toString() + "' sortable='true'  type='" + HeaderColumn.Type.toString() + "' " + (HeaderColumn.Width != "" ? "style='width:" + HeaderColumn.Width + "' width='" + HeaderColumn.Width + "'" : "") + "><span  > " + HeaderColumn.HeaderText + "</span><div class=\"sortableDown\"></div></th>";
                        }
                    }
                }
            }
        }
        html += "</thead>";

        gGridObj.GridHeader.IsRendered = true;
    }
    else {
        $(grid).find("tbody").replaceWith("");
    }
    html += "<tbody>";
    // -- render the data rows
    $.each(data.Rows, function () {
        html += "<tr>";
        var currentRow = this;
        $.each(gGridObj.GridHeader.Columns, function () {
            var hColumn = this;
            // for template
            if (this.Type == "Template") {
                if (this.Visible) {
                    html += "<td style='text-align:" + hColumn.Alignment + "'>" + this.Template + "</td>";
                }
                else {
                    html += "<td style='visible:hidden; display:none'></td>";
                }
            }
            // handle visibility
            else {
                if (hColumn.Visible) {
                    html += "<td style='text-align:" + hColumn.Alignment + "'>" + eval("currentRow." + hColumn.Field) + "</td>";
                }
                else {
                    html += "<td style='visible:hidden; display:none'>" + eval("currentRow." + hColumn.Field) + "</td>";
                }
            }
        });
        html += "</tr>";
    });
    $(grid).append(html);
    if (sort) {
        GrandGrid.Utilities.ToggleSortOrder(grid, sort, sortOrder);
    }

    GrandGrid.AjaxInProgress = false;

}
//-- Build rows if data is supplied
GrandGrid.BuildRowsWithData = function (grid, gGridObj, data) {
    var html = '';
    // ---- Render the Header Row

    if (!gGridObj.GridHeader.IsRendered) {
        // Remove it after saving the template
        $(grid).find("thead").parent().html("");
        html += "<thead><tr>";
        for (HeaderCol = 0; HeaderCol < gGridObj.GridHeader.Columns.length; HeaderCol++) {
            var HeaderColumn = gGridObj.GridHeader.Columns[HeaderCol];
            if (!HeaderColumn.Visible) {
                html += "<th "+ (HeaderColumn.Alignment=="right"? "class=\"txtAlign-right\"":"") +" FieldMap='" + HeaderColumn.Field.toString() + "' isvisible='false' style='display:none; visible:hidden' type='" + HeaderColumn.Type.toString() + "' sortable='" + HeaderColumn.Sortable.toString() + "'><span>" + HeaderColumn.HeaderText + "</span></th>";
            }
            else {
                if (HeaderColumn.Type == "Template") {
                    html += "<th"+ (HeaderColumn.Alignment=="right"? "class=\"txtAlign-right\"":"") +" FieldMap='" + HeaderColumn.Field + "' isvisible='" + HeaderColumn.Visible.toString() + "' " + (HeaderColumn.Width != "" ? "style='text-align:center;width:" + HeaderColumn.Width + "' width='" + HeaderColumn.Width + "'" : "") + " type='Template'><span>" + HeaderColumn.HeaderText + "</span></th>";
                }
                else {
                    if (!HeaderColumn.Sortable) {
                        html += "<th "+ (HeaderColumn.Alignment=="right"? "class=\"txtAlign-right\"":"") +" FieldMap='" + HeaderColumn.Field.toString() + "' isvisible='" + HeaderColumn.Visible.toString() + "' type='" + HeaderColumn.Type.toString() + "' " + (HeaderColumn.Width != "" ? "style='width:" + HeaderColumn.Width + "' width='" + HeaderColumn.Width + "'" : "") + "><span>" + HeaderColumn.HeaderText + "</span></th>";
                    }
                    else {
                        if (HeaderColumn.Type == "Normal") {
                            html += "<th "+ (HeaderColumn.Alignment=="right"? "class=\"txtAlign-right\"":"") +" isvisible='" + HeaderColumn.Visible.toString() + "' FieldMap='" + HeaderColumn.Field.toString() + "' sortable='true'  type='" + HeaderColumn.Type.toString() + "' " + (HeaderColumn.Width != "" ? "style='width:" + HeaderColumn.Width + "' width='" + HeaderColumn.Width + "'" : "") + "><span>" + HeaderColumn.HeaderText + "</span></th>";
                        }
                    }
                }
            }
        }
        html += "</thead>";
        gGridObj.GridHeader.IsRendered = true;
    }
    else {
        $(grid).find("tbody").replaceWith("");
    }
    html += "<tbody>";
    // -- render the data rows
    $.each(data.Rows, function () {
        html += "<tr>";
        var currentRow = this;
        $.each(gGridObj.GridHeader.Columns, function () {
            var hColumn = this;
            // for template
            if (this.Type == "Template") {
                if (this.Visible) {
                    html += "<td style='text-align:" + hColumn.Alignment + "'>" + this.Template + "</td>";
                }
                else {
                    html += "<td style='visible:hidden; display:none'></td>";
                }
            }
            // handle visibility
            else {
                if (hColumn.Visible) {
                    html += "<td style='text-align:" + hColumn.Alignment + "'>" + eval("currentRow." + hColumn.Field) + "</td>";
                }
                else {
                    html += "<td style='visible:hidden; display:none'>" + eval("currentRow." + hColumn.Field) + "</td>";
                }
            }
        });
        html += "</tr>";
    });
    $(grid).append("</tbody>" + html);
    GrandGrid.AjaxInProgress = false;

}
// #endregion
// Build paging
GrandGrid.BuildPaging = function (totalResult, grd, page, pageSize, sort, sortOrder) {
    // buildGridStructure(grd, page, sort, sortOrder, data)
    var opParams = "";
    var pagerID = $(grd).attr("id") + "_pager";
    var innerContent = "";
    var totPages = 0;
    var pTD = document.createElement("td");
    var pTR = document.createElement("tr");

    if (!page) {
        page = "1";
    }
    if (!pageSize) {
        pageSize = totalResult;
    }
    totPages = parseInt((parseInt(totalResult) / parseInt(pageSize)));
    if (totPages < 1)
        return;

    if ((parseInt(totalResult) % parseInt(pageSize)) > 0) {
        totPages += 1;
    }
    if (sort && sortOrder) {
        opParams += ",'" + sort + "','" + sortOrder + "'";
    }

    if (parseInt(page) != totPages) {
        innerContent += "<div class='prevnext-wrap'><a href='#' class='next' onclick=\"javascript:GrandGrid.BuildGridStructure(document.getElementById('" + $(grd).attr("id") + "'), '" + (parseInt(page) + 1).toString() + "'" + opParams + ");\">Next</a></div>";
    }

    if (totPages > 1) {
        innerContent += "<div class='select-wrap'><select onchange=\"javascript:GrandGrid.BuildGridStructure(document.getElementById('" + $(grd).attr("id") + "'), this.options[this.selectedIndex].text" + opParams + ");\">";
        
        for (i = 1; i <= totPages; i++) {
            if (parseInt(page) == i) {
                innerContent += "<option value='" + i.toString() + "' selected='selected'>" + i.toString() + "</option>";
            }
            else {
                innerContent += "<option value='" + i.toString() + "'>" + i.toString() + "</option>";
            }
        }

        innerContent += "</select></div>";
    }

    if (parseInt(page) > 1) {
        innerContent += "<div class='prevnext-wrap'><a href='#' class='prev' onclick=\"javascript:GrandGrid.BuildGridStructure(document.getElementById('" + $(grd).attr("id") + "'), '" + (parseInt(page) - 1).toString() + "'" + opParams + ");\">Previous</a></div>";
    }


    $(pTD).attr("colspan", $(grd).find("th[isvisible=true]").length);
    $(pTD).append(innerContent);
    $(pTR).append(pTD);
    $(grd).find('tr:last').parent().append(pTR);
}
