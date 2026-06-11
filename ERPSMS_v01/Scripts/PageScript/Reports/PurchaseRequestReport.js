var _dtFrom = null;
var _dtTo = null;
$(document).ready(function () {
    $('[id$=hdfFromDate]').val("");
    $('[id$=hdfToDate]').val("");
    PageInt();
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
});
//<summary>Function used to initiallize Page</summary>
function PageInt() {
    SetDate();
}
//<summary>Function called after each Request</summary> 
function EndRequestHandler(sender, args) {
    if (typeof EndRequestHandlerPage == 'function') {
        EndRequestHandlerPage();
    }
}
function EndRequestHandlerPage() {
    PageInt();
}
//<summary>Function used to call AddDateRange and assign the default dates</summary>
function SetDate() {
    var dtFrom = $('[id$=hdfFromDate]').val();
    var dtTo = $('[id$=hdfToDate]').val();
    var frmDt = null;
    var toDt = null;
    GrandScriptUtils.AddDateRange("DateFrom", "hdfFromDate", "DateTo", "hdfToDate", false, false);
    if (_dtFrom != null && _dtTo != null) {
        $('input[id$=DateFrom]').val(_dtFrom);
        $('input[id$=DateTo]').val(_dtTo);
    }
    else {
        if (dtFrom.trim() != "" && dtTo.trim() != "") {
            frmDt = new Date(dtFrom);
            toDt = new Date(dtTo);
        }
        else {
            frmDt = new Date();
            frmDt.setDate(frmDt.getDate() + -1);
            toDt = new Date();
        }
        $('input[id$=DateFrom]').datepicker("setDate", frmDt);
        $('input[id$=DateTo]').datepicker("setDate", toDt);
    }
}