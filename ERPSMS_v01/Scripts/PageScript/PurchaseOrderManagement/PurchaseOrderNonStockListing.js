/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region ----------------------------GlobalVariables----------------------------
var POID = 0;
///#endregion

//#region -------------------------Configuration Section--------------------------
var POListing = {
    //Url
    AutoCompleteURL: "POGeneration.do?Action=GetSearchValue&AUTOSEARCH=1",
    PurchaseSearchAutoCompleteURL: "POGeneration.do?Action=GetPurchaseAutoSearchValue&AUTOSEARCH=1",
    ApplicationStatus: "CommonManagement.do?Action=GetAppStatus&AUTOSEARCH=1&Type=PO",
    CreatePO: "PurchaseOrderNonStock.aspx",
    ReportPO: "PurchaseOrderReport.aspx",
    GetPoDetails: "POGeneration.do?Action=GetPurchaseOrderList&Status=",
    DeletePoDetails: "POGeneration.do?Action=DeletePODetails&POID=",
    SaveShortClose: "POGeneration.do?Action=POShortClose",
    SaveMessage1: "Translate(SaveMsgPO1)",
    SaveMessage2: "Translate(SaveMsgPO2)",
    PAGEURL: "/PurchaseOrderManagement/PurchaseOrderNonStock.aspx?TYPE=1",
    POREPORTURL: "../Reports/GenerateReport.aspx", //"PurchaseRequestReport.aspx",


    //Commands
    PerformAction: "PERFORMACTION",
    Delete: "DELETE",
    SAVE: "SAVE",
    SAVESHORTCLOSE: "SAVESHORTCLOSE",
    Evaluate: "EVALUATE",
    View: "VIEW",
    Print: "PRINT",
    SHORTCLOSE: "SHORTCLOSE",
    Successfully:"DELETEDSUCESSFULLY",
    POSaveMessage: "Translate(SaveMsgPO)",
    GrnDraftSaveExists: "Translate(GrnDraftSaveExists)",
    Information: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    GrnNotApproved: "Translate(GrnNotApproved)",

    //Fields
    UserStatus: "USER_STATUS",
    POID: "POH_PK",
    RefID: "REF_ID",
    POSTATUS: "POH_STATUS",
    VendorName: "VEN_NAME",
    ItemText: "POH_ITEM_TEXT",
    POHNO: "POH_NO",

    //Message
    DeleteMessage: "Translate(Doyouwanttodeletethisdetails)",
    DeleteTitle: "Translate(Information)",
    ShortCloseMessage: "Translate(ConfirmclosePO)",
    DeleteFailed: "Translate(CannotDelete)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    SaveShortCloseMessage: "Translate(SaveClosePo)",
    DeleteSucessfully: "Translate(PODeletedSucessfully)",
    DocGenerationNewValue: "Translate(DocGenerationNew)"

}
//#endregion

//#region ------------------------- initialization Section ------------------------.

$(document).ready(function () {

    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();
    ShowHideAdvancedSearch();
});

function PageInit() {

//    SetSearchType();
    //    SearchInit();
    SearchAutoInit();
    FillStatus();
   // BindGrid("POH_NO");
    $("#divShortClose").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveValidations();
        }
    });
}

function AddNew() {
    ///<summary>Will redirect the listing page to vendor creation screen</summary>

    window.location = POListing.CreatePO;
    return false;
}

///#region--------------------------- Auto Complete Section ----------------------------

//function SetSearchType() {
//    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

//    ClearSearchDetails();
//    var strname = $("select[id$=SearchType]").val();
//    $("[id$=SearchValue]").val("");
//    if (strname == "0") {
//        $("#divSearchDtls").hide();
//        $("#divDate").hide();
////        $("[id$=imbSearch]").hide();
//        BindGrid();
//    }
//    else if (strname == "Date") {
//        $("#divSearchDtls").hide();
//        $("#divDate").show();
////        $("[id$=imbSearch]").show();
//        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
//    }
//    else {
//        $("#divSearchDtls").show();
//        $("#divDate").hide();
////        $("[id$=imbSearch]").show();
//    }
//}

function SearchAutoInit() {
    ClearSelections();
    GrandScriptUtils.DatePicker("ToDate", "dd-M-yy", false, true);
    GrandScriptUtils.DatePicker("txtFromDate", "dd-M-yy", false, false);


    GrandScriptUtils.MakeAutoComplete("txtVendor", POListing.PurchaseSearchAutoCompleteURL + "&SearchType=VEN_NAME" + "&PageURL=" + POListing.PAGEURL, "hdfVendor", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtReqFor", POListing.PurchaseSearchAutoCompleteURL + "&SearchType=DPT_NAME" + "&PageURL=" + POListing.PAGEURL, "hdfReqFor", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtPONumber", POListing.PurchaseSearchAutoCompleteURL + "&SearchType=POH_NO" + "&PageURL=" + POListing.PAGEURL, "hdfPONumber", true, false, false, true, false, false, false, true);

    return false;
}
function FillStatus() {
    var drpID = $("select[id$=ddlTrnStatus]").attr("id");
    $.get(POListing.ApplicationStatus, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, false, false, false, true);
        $("[id$=ddlTrnStatus]").val("-1");
        BindGrid();
    });

}

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val("");
    $("[id$=FromDate]").val("");
    $("input[id$=hdfFrmDate]").val("");
    $("[id$=ToDate]").val("");
    $("input[id$=hdfToDate]").val("");
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>

    $("[id$=SearchValue]").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: POListing.AutoCompleteURL,
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
            $("[id$=SearchValue").val(ui.item.label);
            if (typeof AfterSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                AfterSelect();
            }
        }
    });
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>

    BindGrid();
}

///#endregion

//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=Remarks]").rules("add", {
        required: true,
        maxlength: 200,
        messages: { required: "Translate(PleaseProvideRemarks)" }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

//#endregion

//#region -----------------------------Core section---------------------------

function ResetPage() {

//    $("select[id$=SearchType]").val("0");
//    //setting search type.
    //    SetSearchType();
    ClearSelections();
    SearchAutoInit();
    BindGrid();
    return false;

}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>

    // var srchV = "";
    var poNumber = "";
    poNumber = $("[id$=txtPONumber]").val() == "Select/Type" ? "" : $("[id$=txtPONumber]").val();
    //var ajaxUrl = POListing.GetPoDetails + $("[id$=SearchType]").val() + "&SearchValue=" + escape($("[id$=SearchValue]").val()) + "&UserPk=" + $("input[id$=UserPk]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&FilterStatus=" + $("select[id$=FilterStatus]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + POListing.PAGEURL + "&Service=2";
    var ajaxUrl = POListing.GetPoDetails + $("[id$=ddlStatus]").val() + "&UserPk=" + $("input[id$=UserPk]").val() + "&FromDate=" + $("[id$=txtFromDate]").val() + "&ToDate=" + $("[id$=txtToDate]").val() + "&TrnStatus=" + $("select[id$=ddlTrnStatus]").val() + "&vendor=" + $("[id$=hdfVendor]").val() + "&PageUrl=" + POListing.PAGEURL + "&reqStore=" + $("[id$=hdfReqFor]").val() + "&poNo=" + poNumber + "&prNo=" + $("[id$=txtPrNo]").val() + "&ioNo=" + $("[id$=txtIONo]").val() + "&Service=2";
    $("#grdPODetails").removeAttr("ajaxurl")
    $("#grdPODetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPODetails");
    GrandGrid.MakeGrid($("#grdPODetails"));

    return false;
}

function ClearSelections() {
    $("[id$=txtFromDate]").val("");
    $("[id$=txtToDate]").val("");
    $("[id$=hdfVendor]").val("0");
    $("[id$=hdfReqFor]").val("0");
    $("[id$=txtReqFor]").val("");
    $("[id$=txtPrNo]").val("");
    $("[id$=txtIONo]").val("");
    $("[id$=hdfPONumber]").val("0");
    $("[id$=txtPONumber]").val("");
    $("[id$=SearchValue]").val("");
    $("[id$=ddlStatus]").val("-5");
    $("[id$=ddlTrnStatus]").val("-1");
}   
    
function AfterGridBind() {

    var isShortClose = IsShortClosure();
    $("#grdPODetails").find("tr:has(td)").each(function () {
        //        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), POListing.VendorName, $(this).parents("table:first").attr("id"));
        //       
        //        if (colIndex != null) {
        //            $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" style=\"width:90%\" value=" + POListing.VendorName + " ToolTip= " +  maxlength=\"11\" tabIndex=\"5\" />");
        //            
        //        }

        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, POListing.UserStatus, tableID);
        var poStatus = GrandGrid.Utilities.GetColumnValue(this, POListing.POSTATUS, tableID);
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        else if (UserStatus == 0) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        else if (UserStatus == 2) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        if ((poStatus == "2") && isShortClose) {
            $(this).find("td:last input[id$=imbShortClose]").show();
        }
        else {
            $(this).find("td:last input[id$=imbShortClose]").hide();
        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), POListing.POHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), POListing.POHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), POListing.POHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), POListing.POHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(POListing.DocGenerationNewValue);
        }
        //Providing tooltip for the column 	Item Details 
        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POH_ITEM_TEXT", tableID);
        ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "POH_ITEM_TEXT", tableID);
        ItemFullText = GrandGrid.Utilities.GetColumnValue($(this), "POH_ITEM_FULLTEXT", tableID);
        if (ItemIndex != null) {
            var quotReplace = ItemFullText.replace(/"/g, '&quot;');
            $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails + "</div>");
        }

    });
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
   
    POID = GrandGrid.Utilities.GetColumnValue(tr, POListing.POID, $(tr).parent().attr("id"));
    switch (command.toString()) {
        // To Delete Details
        case POListing.PerformAction:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, POListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, POListing.RefID, $(tr).parent().attr("id"));
                window.location = POListing.CreatePO + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = POListing.CreatePO + "?POID=" + POID;
            }
            break;
        case POListing.Delete:
            GrandScriptUtils.ShowModal(POListing.DeleteMessage, POListing.DeleteTitle, POListing.Delete,true);
            break;
        case POListing.View:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, POListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, POListing.RefID, $(tr).parent().attr("id"));
                window.location = POListing.CreatePO + "?RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = POListing.CreatePO + "?POID=" + POID + "&Status=1";
                
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, POListing.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = POListing.CreatePO + "?POID=" + POID + "&Status=1";
                }
                else {
                    window.location = POListing.CreatePO + "?RefID=" + refID + "&Status=1";
                }
            }
            break;
        case POListing.Print: 
           // window.location = POListing.ReportPO + "?POID=" + POID;
            //window.location = POListing.POREPORTURL + "?ID=" + POID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
            var url = POListing.POREPORTURL + "?ID=" + POID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
            OpenPDF(url);
            break;
        case POListing.SHORTCLOSE:

            ResetShortClose();
            $("[id$=POID]").val(POID);
            $("#divShortClose").dialog("open");
            $("#divShortClose").dialog({ width: 500, height: 150, resizable: false });
            break;
    }
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        //comment req  
        case POListing.Delete:
            DeleteDetails();
            break;
        case POListing.SAVE:
            SaveShortClose();
            break;
        case POListing.Successfully:
            BindGrid();
            break;
        case POListing.SAVESHORTCLOSE:
            BindGrid();
            break;
    }
    return false;
}

function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete Po Details, And Finally, Fill Remaining Data</summary>

    var msgtxt;
    $.get(POListing.DeletePoDetails + POID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            //msgtxt = POListing.DeleteSucessfully;
            GrandScriptUtils.ShowModal(POListing.DeleteSucessfully, POListing.DeleteTitle, POListing.Successfully);
            //BindGrid();
        }
        else if (parseInt(data) == 0)
            //msgtxt = POListing.DeleteFailed;
            GrandScriptUtils.ShowModal(POListing.DeleteFailed, POListing.DeleteTitle, POListing.DeleteFailed);

    });
    return false;
}

function ConfirmSaveShortClose() {
    ///<summary>Function To confirm short close</summary>

    RemoveValidations();
    AddValidations();
    if ($(document.forms[0]).valid()) {
        GrandScriptUtils.ShowModal(POListing.ShortCloseMessage, POListing.DeleteTitle, POListing.SAVE, true);
    }
    return false;
}

function IsShortClosure() {
    ///<summary>Function used to check whether user have the permission to short close po</summary>

    var userRole = $("[id$=hdnRoleID]").val();
    var arrUserRole = userRole.split(",");
    var shortClosureUserGroup = $("[id$=hdnShortCloseGroup]").val();
    for (var i in arrUserRole) {
        if (shortClosureUserGroup == arrUserRole[i]) {
            return true;
        }
    }
    return false;
}

function SaveShortClose() {
    ///<summary>Function To save short close</summary>

    var jSonString = GrandScriptUtils.FormToJsonString(false);
    $.post(POListing.SaveShortClose, jSonString, function (data) {
        if (parseInt(data) == 1) {
            $("[id$=POID]").val("0");
            $("#divShortClose").dialog("close");
            GrandScriptUtils.ShowModal(POListing.SaveShortCloseMessage, POListing.DeleteTitle, POListing.SAVESHORTCLOSE);
        }
        else if (parseInt(data) == -27) {
            GrandScriptUtils.ShowModal(POListing.GrnNotApproved, POListing.DeleteTitle);
        }
        else if (parseInt(data) == -28) {
            GrandScriptUtils.ShowModal(POListing.GrnDraftSaveExists, POListing.DeleteTitle);
        }
        else {
            GrandScriptUtils.ShowModal(POListing.ActionFailedMessage, POListing.DeleteTitle);
        }
    });
}

function ResetShortClose() {
    ///<summary>Function To reset the short close details</summary>

    $("[id$=Remarks]").val("");
    $("[id$=RefNo]").val("");
}

function ShowHideAdvancedSearch(flag) {
    //If flag then Show AdvancedSearch
    if (flag) {
        $("[id$=tbladvancedSearch]").show();
        $("[id$=imbShowFilter]").hide();
        $("[id$=imbHideFilter]").show();
    }
    else {
        $("[id$=tbladvancedSearch]").hide();
        $("[id$=imbShowFilter]").show();
        $("[id$=imbHideFilter]").hide();
    }
    return false;
}

//#endregion