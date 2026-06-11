/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />


//#region ------- Configuration Section -------
var InboxConfig = {
    // Constant
    APPROVECMD: "APPROVE",
    RefID: "RefID",
    APPROVE: "APPROVE",
    GetProcessID: "GetProcessID?GetProcessID",
    // Messages
    INFORMATIONTITLE: "Translate(Confirmation)",
    APPROVECONFIRMMSG: "Translate(ApproveConfirmationmsg)",
    DOYOUWANT: "Translate(Doyouwantto)"
}
//#endregion

// first set the task tab then call set the total count
$(document).ready(function () {

    PageInt();
});

function PageInt() {
    //<summary>Function used to initiallize Page</summary>

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    DateInit();
    DropDownReadable();
 

}


function EndRequestHandler(sender, args) {
    // Validate the input text controls to prevent XSS

    DropDownReadable();
}

function DropDownReadable() {

    if ($.browser.msie) {
        $("select").each(function () {
            var drp = $(this);
            $(drp).attr("id");
            $(drp).find("option").each(function () {
                $(this).attr("title", $(this).text());
            });
        });
    }
}

function DateInit() {
    GrandScriptUtils.AddDateRangeCommon("PeriodFrom", "hdfPrdFrm", "PeriodTo", "hdfPrdTo", false, false, true, false);
    //    var format = "dd-M-yy";
    //    $('input[id$=PeriodFrom]').datepicker({
    //        dateFormat: format,
    //        changeMonth: true,
    //        changeYear: true,
    //        altField: $("[id$=hdfPrdFrm]"),
    //        altFormat: "mm/dd/yy"
    //    });
    //    $('input[id$=PeriodTo]').datepicker({
    //        dateFormat: format,
    //        changeMonth: true,
    //        changeYear: true,
    //        altField: $("[id$=hdfPrdTo]"),
    //        altFormat: "mm/dd/yy"
    //    });
}


 

function ShowApproveConfirm(btn, message) {
    var msgTitle;
    var msg;
    msgTitle = InboxConfig.INFORMATIONTITLE;
    msg = message ? InboxConfig.DOYOUWANT + message + "?" : InboxConfig.APPROVECONFIRMMSG;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 130,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            OK: function (e) {
                $(this).dialog("close");
                __doPostBack(btn.name, '');
            },
            Cancel: function (e) {
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

function ShowConfirmQuickApprove(btn) {
    var msgTitle;
    var msg;
    var message = $("[id$=hdfTitle]").val();
    msgTitle = InboxConfig.INFORMATIONTITLE;
    msg = message ? InboxConfig.DOYOUWANT + message + "?" : InboxConfig.APPROVECONFIRMMSG;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 130,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            OK: function (e) {
                $(this).dialog("close");
                $("[id$=btnApprove]").click();
                //__doPostBack(btn.name, '');
            },
            Cancel: function (e) {
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

