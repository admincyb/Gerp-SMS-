///#region ------- Global Variable -----
var purchaseRqstPK = 0;
//var procID = 5;
var pk = 0;
///#endregion

//Biju
//$.xhrPool = [];

//$(function () {
//    $.ajaxSetup({
//        beforeSend: function (jqXHR, settings) {
//            if ($.xhrPool.length === 0) {
//                // Show overlay 
//                //alert('Request count' + $.xhrPool.length);   
//                $('#updateProgress').show();
//            }
//            $.xhrPool.push(jqXHR);
//        },
//        complete: function (jqXHR) {
//            if ($.xhrPool.length > -1) {
//                $.xhrPool.pop(jqXHR);
//            }
//            if ($.xhrPool.length === 0) {
//                //hide overlay
//                // alert('Request count' + $.xhrPool.length);
//                $('#updateProgress').hide();
//            }
//        }
//    });
//});

//#region ------- Configuration Section -------
var PurchaseRequest = {
    // URL
    PURCHASEREQUESTBINDGRIDURL: "PurchaseRequest.do?Action=GetPurchaseRequestTradingList&Status=",
    PURCHASEREQUESTDELETEURL: "PurchaseRequest.do?Action=DeletePurchaseRequestTradingList&PK=",
    PURCHASEREQUESTAUTOCOMPLETEURL: "PurchaseRequest.do?Action=GetSearchValueTradingList&AUTOSEARCH=1",
    PURCHASERQSTENTRYURL: "PurchaseRequestTrading.aspx",
    PAGEURL: "/PurchaseRequestManagement/PurchaseRequestTrading.aspx",
    PURCHASEREQUESTREPORTURL: "../Reports/GenerateReport.aspx", //"PurchaseRequestReport.aspx",
    PerformAction: "PERFORMACTION",
    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    CLOSE_PR: "CLOSEPR",
    SHORTCLOSE_PR: "SHORTCLOSEPR",
    CANCEL_PR: "CANCELPR",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    PURCHASERQSTPK: "PRH_PK",
    USERSTATUS: "USER_STATUS",
    ISPOEXIST: "POH_PO_FLAG",
    ISPOWKFEXIST: "POH_PO_WKF_FLAG",
    DELSTATUS: "PRH_DEL_STATUS",
    PRH_STATUS: "PRH_STATUS",
    View: "VIEW",
    PRINT: "PRINT",
    RefID: "REF_ID",
    PRHNO: "PRH_NO",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    ShortCloseMessage: "Translate(ConfirmclosePR)",
    CancelMessage: "Translate(ConfirmCancelPR)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    UNABLETODELETE: "Translate(UnableTodelete)",
    UNABLETOCANCEL: "Translate(UnableToCancel)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Translate(PurchaseRequestDeletedSuccessfully)",
    CANCELSUCESS: "Translate(PurchaseRequestCanceledSuccessfully)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",

    SaveMessage1: "Translate(PurchaseDetailsSaved1)",
    SaveMessage2: "Translate(PurchaseDetailsSaved2)",
    DocGenerationNewValue: "Translate(DocGenerationNew)"
}
//#endregion

///#region ------- Initialization Section ----------------

$(document).ready(function () {

    ///<summary>Document . Ready()</summary>
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();
});

function PageInit() {
    ///<summary>Initial page condition</summary>
    $("select[id$=SearchType]").val(PurchaseRequest.TEXTZERO);
    $("[id$=SearchValue]").val(PurchaseRequest.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    BindGrid();
    Popup();
    return false;
}

///#endregion

///#region ------- Core Section -------

///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>

    window.location = PurchaseRequest.PURCHASERQSTENTRYURL;
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>

    ClearSearchDetails();
    PageInit();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    var strname = $("select[id$=SearchType]").val();
    ClearSearchDetails();
    $("[id$=SearchValue]").val(PurchaseRequest.TEXTEMPTY);
    if (strname == PurchaseRequest.TEXTZERO) {
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        //$("[id$=imbSearch]").hide();
        BindGrid();
    }
    else if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        //$("[id$=imbSearch]").show();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        //$("[id$=imbSearch]").show();
    }
}

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val(PurchaseRequest.TEXTEMPTY);
    $("[id$=FromDate]").val(PurchaseRequest.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(PurchaseRequest.TEXTEMPTY);
    $("[id$=ToDate]").val(PurchaseRequest.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(PurchaseRequest.TEXTEMPTY);

}

function SearchInit() {
    ///<summary>To handle auto complete</summary>

    $("[id$=SearchValue]").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: PurchaseRequest.PURCHASEREQUESTAUTOCOMPLETEURL,
                data: {
                    SearchValue: request.term,
                    SearchType: $("[id$=SearchType]").val(),
                    ProcessPK: $("[id$=hdfProcId]").val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Text // format the the data as text 
                        }
                    }));
                }
            });
        },
        cache: false,
        select: function (event, ui) {
            AfterSelect();
        }
    });
}

///#endregion

function FillDetails(tr) {
    ///<summary>Function To Fill Purchase Request Details  </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.PURCHASERQSTPK, $(tr).parent().attr("id"));
    var status = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.USERSTATUS, $(tr).parent().attr("id"))
    window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?PK=" + pk + "&Status=" + status;
    return false;
}

function DeletePRMessage() {
    $("#divDeletePR").dialog("open");
    $("#divDeletePR").dialog(
        {
            width: 490,
            title: "Translate(ClosePR)"
        });
    return false;
}

function CancelPRMessage() {
    $("#divDeletePR").dialog("open");
    $("#divDeletePR").dialog(
        {
            width: 490,
            title: "Translate(ClosePR)"
        });
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>
    var msgtxt;
    $.get(PurchaseRequest.PURCHASEREQUESTDELETEURL + purchaseRqstPK, function (data) {
        if (parseInt(data) == 1) {
            msgtxt = PurchaseRequest.DELETESUCESS;
        }
        else if (parseInt(data) == 0) {
            msgtxt = "Assigned";
        }
        else {
            msgtxt = PurchaseRequest.ACTIONFAILEDMSG;
        }
        GrandScriptUtils.ShowModal(msgtxt, PurchaseRequest.INFORMATIONTITLE, PurchaseRequest.SAVECMD);

    });
    return false;
}

function DeletePRDetails() {
    var msgtxt;
    $.get(PurchaseRequest.PURCHASEREQUESTDELETEURL + purchaseRqstPK + "&Remarks=" + $("[id$=txtPRDeleteComment]").val(), function (data) {
        if (parseInt(data) == 1) {
            msgtxt = PurchaseRequest.DELETESUCESS;
        }
        else if (parseInt(data) == 0) {
            msgtxt = "Assigned";
        }
        else if (parseInt(data) == -10) {
            msgtxt = PurchaseRequest.UNABLETODELETE;
        }
        else {
            msgtxt = PurchaseRequest.ACTIONFAILEDMSG;
        }
        GrandScriptUtils.ShowModal(msgtxt, PurchaseRequest.INFORMATIONTITLE, PurchaseRequest.SAVECMD);
    });
    $("#divDeletePR").dialog("close");
    $("[id$=txtPRDeleteComment]").val("");
    return false;

}

function CancelPRDetails() {
    var msgtxt;
    $.get(PurchaseRequest.PURCHASEREQUESTDELETEURL + purchaseRqstPK + "&Remarks=" + $("[id$=txtPRDeleteComment]").val(), function (data) {
        if (parseInt(data) == 1) {
            msgtxt = PurchaseRequest.CANCELSUCESS;
        }
        else if (parseInt(data) == 0) {
            msgtxt = "Assigned";
        }
        else if (parseInt(data) == -10) {
            msgtxt = PurchaseRequest.UNABLETOCANCEL;
        }
        else {
            msgtxt = PurchaseRequest.ACTIONFAILEDMSG;
        }
        GrandScriptUtils.ShowModal(msgtxt, PurchaseRequest.INFORMATIONTITLE, PurchaseRequest.SAVECMD);
    });
    $("#divDeletePR").dialog("close");
    $("[id$=txtPRDeleteComment]").val("");
    return false;

}
function BindGrid() {
    ///<summary>Bind Designaion Details With Search value </summary>

    var ajaxUrl = PurchaseRequest.PURCHASEREQUESTBINDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&FilterStatus=" + $("select[id$=FilterStatus]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + PurchaseRequest.PAGEURL;
    $("#grdPurchaseRequest").removeAttr("ajaxurl")
    $("#grdPurchaseRequest").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPurchaseRequest");
    GrandGrid.MakeGrid($("#grdPurchaseRequest"));



    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>

    BindGrid();
}
function Popup() {
    ///<summary>Function used for popup</summary>
    $("#divDeletePR").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}


///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.PURCHASERQSTPK, $(tr).parent().attr("id"));
    switch (command.toString()) {

        case PurchaseRequest.PerformAction:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.RefID, $(tr).parent().attr("id"));
                window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?PK=" + pk;
            }
            return false;
            break;

        case PurchaseRequest.DELETECOMMAND:
            purchaseRqstPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.PURCHASERQSTPK, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(PurchaseRequest.DELETECONFIRMMSG, PurchaseRequest.CONFIRMMSG, PurchaseRequest.DELETE, true);
            break;
        case PurchaseRequest.CLOSE_PR:
            purchaseRqstPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.PURCHASERQSTPK, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(PurchaseRequest.ShortCloseMessage, PurchaseRequest.CONFIRMMSG, PurchaseRequest.CLOSE_PR, true);
            break;
        case PurchaseRequest.SHORTCLOSE_PR:
            var refID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.RefID, $(tr).parent().attr("id"));
            window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?RefID=" + refID + "&Status=1&IsModify=1";
            return false;
            break;
        case PurchaseRequest.CANCEL_PR:
            purchaseRqstPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.PURCHASERQSTPK, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(PurchaseRequest.CancelMessage, PurchaseRequest.CONFIRMMSG, PurchaseRequest.CANCEL_PR, true);
            return false;
            break;
        case PurchaseRequest.EDITCOMMAND:
            FillDetails(tr);
            break;

        case PurchaseRequest.View:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.RefID, $(tr).parent().attr("id"));
                window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?PK=" + pk + "&Status=1";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?PK=" + pk + "&Status=1";
                }
                else {
                    window.location = PurchaseRequest.PURCHASERQSTENTRYURL + "?RefID=" + refID + "&Status=1";
                }
            }

            return false;
            break;

        case PurchaseRequest.PRINT:
            var prID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequest.PURCHASERQSTPK, $(tr).parent().attr("id"));
            //window.location = PurchaseRequest.PURCHASEREQUESTREPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
            var url = PurchaseRequest.PURCHASEREQUESTREPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
            OpenPDF(url);
            break;

        default:
            GrandScriptUtils.ShowModal(PurchaseRequest.DEFAULTACTION, PurchaseRequest.INFORMATIONTITLE);
            break;
    }
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {

        case PurchaseRequest.SAVECMD:
            PageInit();
            break;
        case PurchaseRequest.DELETE:
            DeleteDetails();
            break;
        case PurchaseRequest.CLOSE_PR:
            DeletePRMessage();
            break;
        case PurchaseRequest.CANCEL_PR: //CANCELPR
            CancelPRDetails();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    var isModifyPO = $("[id$=hdnModifyPR]").val();
    var isCancelPR = $("[id$=hdnCancelPR]").val();
    $("#grdPurchaseRequest tr:has(td)").each(function () {

        var ItemDetails = "";
        var ColIndex;
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.USERSTATUS, tableID);
        var IsPOExist = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.ISPOEXIST, tableID); //1 => PO exists against this PR
        var ISPOWKFEXIST = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.ISPOWKFEXIST, tableID); //1 => PO exists against this PR and [POH_STATUS] > 0 (Not Draft) 
        var DELSTATUS = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.DELSTATUS, tableID);
        var PRH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.PRH_STATUS, tableID);
        if (UserStatus == 1) {//Action To perform for the logged in user
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        else if (UserStatus == 0) {//No Action to perform but he is a participent in the work flow
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        else if (UserStatus == 2) {//Draft will have this status
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        if (PRH_STATUS == 104) { //Cancelled
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbView]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
        }
        //      if (isClosePO=="1") {
        if (isModifyPO == "1" && UserStatus == 0 && IsPOExist == 1 && ISPOWKFEXIST == 1 && PRH_STATUS != 4 && PRH_STATUS != 5) {   //PRH_STATUS=> 4 (ShortClosure),5(Closed)
            $(this).find("td:last input[id$=imbPRShorClose]").show();
        }
        else {
            $(this).find("td:last input[id$=imbPRShorClose]").hide();
        }
        //Cancel PR
        if (isCancelPR == "1" && UserStatus != 2 && IsPOExist == 0 && DELSTATUS != 1 && PRH_STATUS != 4 && PRH_STATUS != 3) { //UserStatus: 2=>draft
            $(this).find("td:last input[id$=imbPRCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbPRCancel]").hide();
        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequest.PRHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.PRHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.PRHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequest.PRHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(PurchaseRequest.DocGenerationNewValue);
        }

        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SOH_NO", tableID);
        if (ColIndex != null) {
            var ioNumber = GrandGrid.Utilities.GetColumnValue($(this), "SOH_NO", tableID);
            if (ioNumber == "null")
                $(this).find("td:eq(" + ColIndex + ")").html("");
        }
        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRH_ISSUE_DEPT_TEXT", tableID);
        if (ColIndex != null) {
            var ioNumber = GrandGrid.Utilities.GetColumnValue($(this), "PRH_ISSUE_DEPT_TEXT", tableID);
            if (ioNumber == "null")
                $(this).find("td:eq(" + ColIndex + ")").html("");
        }

        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRH_USER", tableID);
        if (ColIndex != null) {
            var ioNumber = GrandGrid.Utilities.GetColumnValue($(this), "PRH_USER", tableID);
            if (ioNumber == "null")
                $(this).find("td:eq(" + ColIndex + ")").html("");
        }

        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRH_ITEM_TEXT", tableID);
        ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "PRH_ITEM_FULL_TEXT", tableID);
        if (ItemIndex != null) {
            if (ItemDetails.length > 80) {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails.substring(0, 80) + "...</div>");
            }
            else {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails + "</div>");

            }
        }

    });
}
///#endregion

///#endregion

