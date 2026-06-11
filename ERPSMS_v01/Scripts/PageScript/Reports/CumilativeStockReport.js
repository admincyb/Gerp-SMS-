$(document).ready(function () {
    //<summary>document.ready Menthod</summary>
    dateInit();
});

function dateInit() {
    //<summary>Function used to Initilize date control to UI Controls</summary>

    if ($("[id$=txtFromDate]").val() != "") {
        GrandScriptUtils.DatePicker("txtFromDate", "dd-M-yy", false, false);
    }
    else {
        GrandScriptUtils.DatePicker("txtFromDate", "dd-M-yy", false, true);
    }
    if ($("[id$=txtToDate]").val() != "") {
        GrandScriptUtils.DatePicker("txtToDate", "dd-M-yy", false, false);
    }
    else {
        GrandScriptUtils.DatePicker("txtToDate", "dd-M-yy", false, true);
    }
}
