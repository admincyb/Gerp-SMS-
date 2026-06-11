/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region -----Global Variables-----
var requisitionHeaderId = 0;
var MenuType = 0;

///#endregion

///#region ------Configuration Section-----
var RequisitionList = {


    //Url
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    AutoCompleteURL: "StoreRequisitionSlip.do?Action=GetRequisitionSearchValue&SBUPk=",
    BindGridURL: "StoreRequisitionSlip.do?Action=GetRequisitionList&Status=",
    ApplicationStatus: "CommonManagement.do?Action=GetAppStatus&AUTOSEARCH=1&Type=SRS",
    DeleteRowURL: "StoreRequisitionSlip.do?Action=DeleteRequisition&RequisitionID=",
    REDIRECTURLEDIT: "../StoreManagement/StoreRequisitionSlipCreation.aspx?RequisitionID=",
    ADDNEWURL: "../StoreManagement/StoreRequisitionSlipCreation.aspx",
    PRINTURL: "../StoreManagement/StoreRequisitionReport.aspx",
    PAGEURL: "/storemanagement/storerequisitionslipcreation.aspx",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",

    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(RequisitionDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    RequisitionCodeAlreadyAdded: "Translate(Materialcodelreadyexists)",
    PerformAction: "PERFORMACTION",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    CancelMessage: "Translate(ConfirmCancel)",
    RequisitionCancelMessage: "Translate(RequisitionDetailsCancelSuccesfully)",
    UnableToCancelMR_MI_Exist: "Translate(UnableToCancelMR_MI_Exist)",

    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    LOGOUT: "LOGOUT",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    UserStatus: "USER_STATUS",
    View: "VIEW",
    Print: "PRINT",
    RefID: "REF_ID",
    //Fields
    MRHNO: "MRH_NO",
    RequisitionHeaderId: "MRH_PK",
    MRH_STATUS: "MRH_STATUS",
    CancelCommand: "CANCEL",
    MODIFY: "MODIFY",

    RequisitionSaveMessage1: "Translate(RequisitionDetailsSaved1)",
    RequisitionSaveMessage2: "Translate(RequisitionDetailsSaved2)"

}
///#endregion

///#region------ Initialization Section ----------------

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    //Page Initial condtions
    PageInit();
    ShowWorkflowSaveMsg();
});


function PageInit() {
    ///<summary>initial page condition</summary>

    //Reseting all input controls in the page
    $("[id$=imbSave]").hide();
    $("[id$=imbAdd]").show();

    $("[id$=divListing]").show();

    MenuType = $("[id$=hdfType]").val();

    //Reseting all input controls in the page.
    //ResetPage();
    //setting search type.
    SetSearchType();
    //setting status type.
    FillStatus();
    //initializing search.
    SearchInit();
    //calling function for binding grid.
    //BindGrid();
    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) != 1) {
        var drpID = $("select[id$=SearchType]").attr("id");
        $("#" + drpID + " option[value=CMP_DISPLAY_CODE]").remove(); //No need to show Plant filter Type    
    }
    return false;
}

///#endregion

///#region---- Core Section Section----

///#region---- Data Management Section----

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if (pK[0] == "No") {
                GrandScriptUtils.ShowModal(RequisitionList.RequisitionSaveMessage1 + " " + pK[1] + " " + RequisitionList.RequisitionSaveMessage2, "Information");
            }

        }
    }
}

function FillStatus() {
    var drpID = $("select[id$=ddltrxstatus]").attr("id");
    $.get(RequisitionList.ApplicationStatus, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, false, false, false, true);
        $("[id$=ddltrxstatus]").val("-1");
        BindGrid();
    });
}

function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete storeDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    var msgtxt;

    $.get(RequisitionList.DeleteRowURL + requisitionHeaderId, function (data) {


        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)

            msgtxt = RequisitionList.RequisitionDeleteMessage;
        else if (parseInt(data) == 0)

            msgtxt = RequisitionList.RequisitionUsed;
        else
            msgtxt = RequisitionList.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, RequisitionList.MessageBoxTitle, RequisitionList.DeleteCommand);

    });
    return false;
}

function CancelDetails(tr) {
    ///<summary>Function To Get delete and Delete storeDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    var msgtxt;

    $.get(RequisitionList.DeleteRowURL + requisitionHeaderId, function (data) {


        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = RequisitionList.RequisitionCancelMessage;
        else if (parseInt(data) == 0)
            msgtxt = RequisitionList.RequisitionUsed;
        else if (parseInt(data) == -6)
            msgtxt = RequisitionList.UnableToCancelMR_MI_Exist;
        else
            msgtxt = RequisitionList.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, RequisitionList.MessageBoxTitle, RequisitionList.DeleteCommand);

    });
    return false;
}
///<summary>Function To Get requisitionHeaderId and Pass this requisitionHeaderId as a QueryString </summary>
function FillDetails(tr) {
    //Get OrderID From tr - For Pass this as QueryString
    requisitionHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RequisitionHeaderId, $(tr).parent().attr("id"));
    var URL = "";
    URL = RequisitionList.REDIRECTURLEDIT;
    if (MenuType == 2) {
        URL = RequisitionList.REDIRECTURLEDIT + "?Type=" + $("[id$=hdfType]").val();
    }
    window.location = URL + requisitionHeaderId;
}
function BindGrid() {
    ///<summary>To handle bind grid corr. to the search type and search value</summary
    var URL = "";
    URL = RequisitionList.PAGEURL;
    if (MenuType == 2) {
        URL = RequisitionList.PAGEURL + "?Type=" + $("[id$=hdfType]").val();
    }

    var ajaxUrl = RequisitionList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + URL + "&DeptPK=" + $("[id$=hdfDeptID]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&FilterStatus=" + $("select[id$=ddltrxstatus]").val();
    $("#grdRequsitionList").removeAttr("ajaxurl")
    $("#grdRequsitionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdRequsitionList");
    GrandGrid.MakeGrid($("#grdRequsitionList"));
    return false;
}

function AfterSelect() {
    ///<summary>filling gridview after entering search value in search textbox</summary>
    BindGrid();

}
function AfterGridBind() {
    ///<summary>Setting width of the template after binding grid</summary>
    //$("#grdStore th:last").width("5%");
    var isModifyMR = $("[id$=hdnModifyMR]").val();
    var isCancelMR = $("[id$=hdnCancelMR]").val();
    $("#grdRequsitionList").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, RequisitionList.UserStatus, tableID);
        var MRH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.MRH_STATUS, tableID);       
        //  var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        //Action To perform for the logged in user
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbView]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        //Draft will have this status
        else if (UserStatus == 2) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MRH_ITEM_TEXT", tableID);
        var ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "MRH_ITEM_TEXT", tableID);
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
        if (isModifyMR == "1" && UserStatus == 0 && MRH_STATUS != 4 && MRH_STATUS != 5 && MRH_STATUS != 3) {   //MRH_STATUS=> 4 (Cancelled),5(Closed),3(Rejected),0(Drafted)
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }
        if (isCancelMR == "1" && MRH_STATUS != 0 && MRH_STATUS != 3 && MRH_STATUS != 4 && MRH_STATUS != 5) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }
        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE", tableID);
        if (ColIndex != null) {
            $(this).find("td:eq(" + ColIndex + ")").css("font-weight", "bold");
        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), RequisitionList.MRHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.MRHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.MRHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.MRHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(RequisitionList.DocGenerationNewValue);
        }
    });
}
///#endregion

///#region---- Set Or Reset Form----


function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    //Set Redirecting Url for Enter New Details

    $.get(RequisitionList.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(RequisitionList.SessionExpired, RequisitionList.Confirmation, RequisitionList.LOGOUT, true);
            result = false;
        }
        else {
            if ($("[id$=hdfType]").val() == "2") { //for SBU Store request
                window.location = RequisitionList.ADDNEWURL + "?Type=" + $("[id$=hdfType]").val(); //for SBU Store request
            }
            else {
                window.location = RequisitionList.ADDNEWURL;
            }
        }
    });
    return false;
}
function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input").each(function () {
    //        var idval = $(this).attr("id");
    //        if (idval.search("StoreMasterID") != -1)
    //            $(this).val("0");
    //        //Avoid UserPk to get the value of log in user
    //        else if (idval.search("UserPk") == -1)
    //                    $(this).val("");
    //        var idval = $(this).attr("id");
    //        if (!Checkstatus(idval)) {
    //            $(this).val("");
    //        }
    //    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    PageInit();
    return false;
}
function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page

    if (controlID.search("UserPk") != -1) {
        return true;
    }
    if (controlID.search("BizUnitPk") != -1) {
        return true;
    }
    if (controlID.search("StoreMasterID") != -1) {
        return true;
    }
    if (controlID.search("hdfProcId") != -1) {
        return true;
    }
    return false;
}
///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == RequisitionList.TextZero) {
        ClearSearchDetails();
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        $("#divSearchStatus").hide();
        BindGrid();
    }
    else if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        $("[id$=imbSearch]").show();
        $("#divSearchStatus").hide();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else if (strname == "MRH_STATUS") {
        $("#divSearchStatus").show();
        $("#divDate").hide();
        $("#divSearchDtls").hide();
    }

    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
        $("#divSearchStatus").hide();
    }
    ClearSearchDetails();
}

function ClearSearchDetails() {
    
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(RequisitionList.TEXTEMPTY);
    $("[id$=FromDate]").val(RequisitionList.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(RequisitionList.TEXTEMPTY);
    $("[id$=ToDate]").val(RequisitionList.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(RequisitionList.TEXTEMPTY);
    $("select[id$=ddltrxstatus]").val('-1');
  
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", RequisitionList.AutoCompleteURL + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val(), "SearchType");
}
///#endregion

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    $.get(RequisitionList.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(RequisitionList.SessionExpired, RequisitionList.Confirmation, RequisitionList.LOGOUT, true);
            result = false;
        }
        else {
            requisitionHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RequisitionHeaderId, $(tr).parent().attr("id"));
            switch (command.toString()) {
                case RequisitionList.PerformAction:
                    var URL = "";
                    var NavURL = "";
                    URL = RequisitionList.ADDNEWURL;
                    //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.UserStatus, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                        NavURL = URL + "?RefID=" + refID;
                        //window.location = RequisitionList.ADDNEWURL + "?RefID=" + refID;
                    }
                    else if (UserStatus == 2) {
                        NavURL = URL + "?RequisitionID=" + requisitionHeaderId;
                       // window.location = RequisitionList.ADDNEWURL + "?RequisitionID=" + requisitionHeaderId;
                    }

                    if (MenuType == 2) {
                        NavURL = NavURL + "&Type=" + MenuType;
                    }
                    window.location = NavURL;
                    return false;
                    break;

                // To Delete Details         
                case RequisitionList.DeleteCommand:

                    // Do Confirmation.. Before Delete Details
                    GrandScriptUtils.ShowModal(RequisitionList.DeleteConfirmationMessage, RequisitionList.ConfirmationMessage, RequisitionList.DeleteMessageCommand, true);
                    break;
                case RequisitionList.CancelCommand:
                    GrandScriptUtils.ShowModal(RequisitionList.CancelMessage, RequisitionList.ConfirmationMessage, RequisitionList.CancelCommand, true);
                    break;

                // To Edit Details                 
                case RequisitionList.EditCommand:

                    FillDetails(tr);
                    break;
                case RequisitionList.MODIFY:
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                    var URL = "";
                    var NavURL = "";
                    URL = RequisitionList.ADDNEWURL;
                    NavURL = URL + "?RefID=" + refID + "&Status=1&IsModify=1";
                    if (MenuType == 2) {
                        NavURL = NavURL + "&Type=" + MenuType;
                    }
                    window.location = NavURL;
                   // window.location = RequisitionList.ADDNEWURL + "?RefID=" + refID + "&Status=1&IsModify=1";                                  
                    break;
                case RequisitionList.View:
                    //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
                    var URL = "";
                    var NavURL = "";                  
                    URL = RequisitionList.ADDNEWURL;
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.UserStatus, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                        NavURL = URL + "?RefID=" + refID + "&Status=1";
                        //window.location = URL + "?RefID=" + refID + "&Status=1";
                    }
                    else if (UserStatus == 2) {
                        NavURL = URL + "?RequisitionID=" + requisitionHeaderId + "&Status=1";
                        //window.location = URL + "?RequisitionID=" + requisitionHeaderId + "&Status=1";
                    }
                    else if (UserStatus == 0) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                        if (refID == 0) {
                            NavURL = URL + "?RequisitionID=" + requisitionHeaderId + "&Status=1";
                           // window.location = URL + "?RequisitionID=" + requisitionHeaderId + "&Status=1";
                        }
                        else {
                            NavURL = URL + "?RefID=" + refID + "&Status=1";
                           // window.location = URL + "?RefID=" + refID + "&Status=1";
                        }
                    }
                    if (MenuType == 2) {
                        NavURL = NavURL + "&Type=" + MenuType;
                    }
                    window.location = NavURL;

                    return false;
                    break;
                case RequisitionList.Print:

                    //window.location = RequisitionList.PRINTURL + "?RequisitionID=" + requisitionHeaderId;
                    OpenPDF(RequisitionList.PRINTURL + "?RequisitionID=" + requisitionHeaderId);
                    return false;
                    break;
                // Default Handler  Print      
                default:
                    alert(RequisitionList.DefaultAction);
                    break;
            }
        }
    });
    return false;

}


function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    /// <param name="command"  type="Object">
    ///     Specific Delete command
    /// </param>
    switch (command) {
        //comment req     
        case RequisitionList.DeleteCommand:
            BindGrid();
            break;
        //Commend When calling     
        case RequisitionList.DeleteMessageCommand:
            DeleteDetails();
            break;
        case RequisitionList.CancelCommand:
            CancelDetails();
            break;
        case RequisitionList.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}

///#endregion

///#endregion


