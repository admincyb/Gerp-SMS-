/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region -----Global Variables-----
var issuingHeaderId = 0;
var TRX_TYPE = 2; //2-EMR,4-Opening Stock;
var selectVal = -1;
var selectText = "";
var typeText = "";

///#endregion

///#region ------Configuration Section-----
var RequisitionList = {


    //Url
    AutoCompleteURL: "ExternalMaterialIssue.do?Action=GetSearchValue&SBUPk=",
    BindGridURL: "ExternalMaterialIssue.do?Action=GetExternalMaterialListWkf&TrnStatus=",
    DeleteRowURL: "ExternalMaterialIssue.do?Action=DeleteExternalMaterialIssue&IssueID=",
    REDIRECTURLEDIT: "../StoreManagement/EMRCreate.aspx?IssueID=",
    ADDNEWURL: "../StoreManagement/EMRCreate.aspx",
    PRINTURL: "../StoreManagement/ExternalMaterialReceiveReport.aspx",
    REPORTURL: "../Reports/GenerateReport.aspx",
    PAGEURL: "../StoreManagement/EMRCreate.aspx",
    PAGEURLLIST: "/StoreManagement/EMRCreate.aspx",

    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillIssuingType: "ExternalMaterialIssue.do?Action=GetIssuingType&SBUPk=",
    FillIssuingToList: "ExternalMaterialIssue.do?Action=GetIssuingToList&AUTOSEARCH=1&SBUPk=",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    MaterialCategroyDeptURL: "MaterialCategory.do?Action=GetMaterialCategoryListAuto",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",

    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(MaterialReceiptDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    RequisitionCodeAlreadyAdded: "Translate(Materialcodelreadyexists)",
    PerformAction: "PERFORMACTION",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    CANCELSUCESS: "Translate(EMRCanceledSuccessfully)",
    UNABLETOCANCEL: "Translate(UnableToCancelEMR)",
    CancelMessage: "Translate(ConfirmCancelEMR)",
    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)",
    OpeningStockDeleteMessage: "Translate(OpeningStockDeleteMessage)",

    //Constants
    TEXTEMPTY: "",
    TextZero: "0",
    ValueEmpty: ' ',
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    CANCELCommand: "CANCEL",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    ICH_Status: "ICH_STATUS",
    USERSTATUS: "USER_STATUS",
    View: "VIEW",
    Print: "PRINT",
    MODIFY: "MODIFY",
    RefID: "REF_ID",
    //Fields
    ICHNO: "ICH_NO",
    ConsumptionHeaderId: "ICH_PK",

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
    ShowHideAdvancedSearch();

});


function PageInit() {
    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) == 1) {
        $("[id$=divPlantCode]").show();
        FillCompanyDisplayNames();
    }
    else {
        $("[id$=divPlantCode]").hide();
    }
    ///<summary>initial page condition</summary>
    var queryString = window.location.search.substring(1);
    if (queryString != "") {
        var queryStr = queryString.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[0] == "TYPE")) {
                TRX_TYPE = pK[1];
            }
        }
    }
    //Reseting all input controls in the page
    $("[id$=imbSave]").hide();
    $("[id$=imbAdd]").show();

    $("[id$=divListing]").show();

    //Reseting all input controls in the page.
    //ResetPage();
    ClearSearchDetails(); //SearchInit();
    GrandScriptUtils.AddDateRangeCommon("SearchFromDate", "hdfSearchFrmDate", "SearchToDate", "hdfSearchToDate", false, false);
    FillStore();
    FillIsuingType();
    FillIssuingToList();
    typeText = "Translate(AutoDefaultValue)";
    FillMaterialCategoryAutoComplete();
    FillCategoryMaterials(0);
    //calling function for binding grid.
    BindGrid();

    return false;
}

///#endregion

///#region---- Core Section Section----

///#region---- Data Management Section----


function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete storeDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    var msgtxt;

    $.get(RequisitionList.DeleteRowURL + issuingHeaderId + "&HasWorkflow=1", function (data) {


        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            if (TRX_TYPE == 4) {
                msgtxt = RequisitionList.OpeningStockDeleteMessage;
            }
            else {
                msgtxt = RequisitionList.RequisitionDeleteMessage;
            }
        }
        else if (parseInt(data) == 0) {
            msgtxt = RequisitionList.RequisitionUsed;
        }
        else {
            msgtxt = RequisitionList.ActionFailedMessage;
        }
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, RequisitionList.MessageBoxTitle, RequisitionList.DeleteCommand);

    });
    return false;
}
///<summary>Function To Get issuingHeaderId and Pass this issuingHeaderId as a QueryString </summary>
function FillDetails(tr) {
    //Get OrderID From tr - For Pass this as QueryString
    issuingHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.ConsumptionHeaderId, $(tr).parent().attr("id"));
    var status = GrandGrid.Utilities.GetColumnValue(tr, GRNList.USERSTATUS, $(tr).parent().attr("id"))
    window.location = RequisitionList.REDIRECTURLEDIT + issuingHeaderId + "&Status=" + status;
}
function BindGrid() {

    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    //    var Value = $("[id$=SearchValue]").val().toString();
    //    Value = Value.replace('&', '^');
    //    var ajaxUrl = RequisitionList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + Value + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val();
    //Type 2 for material receipt    
    var ajaxUrl = RequisitionList.BindGridURL + $("[id$=ddlTrnStatus]").val() + "&IssueNo=" + $("[id$=SearchValue]").val() + "&ISS_TYPE=" + $("select[id$=ddlType]").val() + "&ISS_TO=" + $("[id$=ICH_ISS_RCV_PK]").val() + "&ISS_STORE=" + $("[id$=ddlIssuingStore]").val() + "&LotNo=" + $("[id$=txtLotno]").val() + "&RefNo=" + $("[id$=txtRefNo]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&TYPE=" + TRX_TYPE + "&PageUrl=" + RequisitionList.PAGEURLLIST + "&FromDate=" + $("[id$=SearchFromDate]").val() + "&ToDate=" + $("[id$=SearchToDate]").val() + "&SortColumn=" + $("[id$=hdfSortColumn]").val() + "&SortOrder=" + $("[id$=hdfSortOrder]").val() + "&SortColumn1=" + $("[id$=hdfSortColumn1]").val() + "&SortOrder1=" + $("[id$=hdfSortOrder1]").val() + "&CatPk=" + $("[id$=hdfItemCatPK]").val() + "&ItmPk=" + $("[id$=hdfItemPk]").val() + "&CMP_PK=" + $("[id$=ddlPlantCode]").val();
    $("#grdRequsitionList").removeAttr("ajaxurl")
    $("#grdRequsitionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdRequsitionList");
    GrandGrid.MakeGrid($("#grdRequsitionList"));
    ShowHideAdvancedSearch();
    return false;
}

function AfterSelect() {
    ///<summary>filling gridview after entering search value in search textbox</summary>
    BindGrid();

}
function AfterGridBind() {
    ///<summary>Setting width of the template after binding grid</summary>
    //$("#grdStore th:last").width("5%");
    $("#grdRequsitionList").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var ICH_Status = GrandGrid.Utilities.GetColumnValue(this, RequisitionList.ICH_Status, tableID);
        //  var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        //Action To perform for the logged in user
        if (ICH_Status == 1) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbView]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
        }
        //No Action to perform but he is a participent in the work flow
        else if (ICH_Status == 0) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbView]").hide();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbPrint]").show();
            $(this).find("td:last input[id$=imbCancel]").hide();
        }
        else if (ICH_Status == 4) { //Cancelled
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbView]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
        }
        if ($("[id$=hdfCancelFlag]").val() == 1 && ICH_Status != 0 && ICH_Status != 4) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }
        if ($("[id$=hdnModifyFlag]").val() == 1 && ICH_Status == 1 && ICH_Status != 4) {   //ICH_Status=> 4 Canceled
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }
        //        //Draft will have this status
        //        else if (UserStatus == 2) {
        //            $(this).find("td:last input[id$=imbEdit]").show();
        //            $(this).find("td:last input[id$=imbDelete]").show();
        //            $(this).find("td:last input[id$=imbView]").hide();
        //        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), RequisitionList.ICHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.ICHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.ICHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.ICHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(RequisitionList.DocGenerationNewValue);
        }
        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE_TEXT", tableID);
        if (ColIndex != null) {
            $(this).find("td:eq(" + ColIndex + ")").css("font-weight", "bold");
        }


        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICH_ITEM_CODE", tableID);
        ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "ICH_ITEM_CODE", tableID);
        if (ItemIndex != null) {
            if (ItemDetails.length > 80) {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails.substring(0, 65) + "...</div>");
            }
            else {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails + "</div>");
            }
        }
        if (TRX_TYPE == 4) {
            $('td:nth-child(4),th:nth-child(4)').hide(); // Hide  Column Type
            $('td:nth-child(5),th:nth-child(5)').hide(); // Hide  Column Receive From 
            $('td:nth-child(8),th:nth-child(8)').width("42%"); //For Design correction
            $('td:nth-child(9),th:nth-child(9)').width("10%");
            $('td:nth-child(10),th:nth-child(10)').width("15%");
        }
    });
}
///#endregion

///#region---- Set Or Reset Form----


function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    //Set Redirecting Url for Enter New Details   
    window.location = RequisitionList.ADDNEWURL + "?TYPE=" + TRX_TYPE;
    return false;
}
function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input").each(function () {
    //        //        var idval = $(this).attr("id");
    //        //        if (idval.search("StoreMasterID") != -1)
    //        //            $(this).val("0");
    //        //        //Avoid UserPk to get the value of log in user
    //        //        else if (idval.search("UserPk") == -1)
    //        //            $(this).val("");
    //        var idval = $(this).attr("id");
    //        if (!Checkstatus(idval)) {
    //            $(this).val("");
    //        }
    //    });

    //    //Selecting the first value in all drop downs
    //    $(document.forms[0]).find("select").each(function () {
    //        $(this).val($(this).find("option:eq(0)").val());
    //    });
    $(document.forms[0]).validate().resetForm();
    SearchAutoInit();
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
        BindGrid();
    }
    else if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        $("[id$=imbSearch]").show();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
    }
    ClearSearchDetails();
}

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(RequisitionList.TEXTEMPTY);
    $("[id$=SearchFromDate]").val(RequisitionList.TEXTEMPTY);
    $("input[id$=hdfSearchFrmDate]").val(RequisitionList.TEXTEMPTY);
    $("[id$=SearchToDate]").val(RequisitionList.TEXTEMPTY);
    $("input[id$=hdfSearchToDate]").val(RequisitionList.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("SearchFromDate", "hdfSearchFrmDate", "SearchToDate", "hdfSearchToDate", false, false, false, false, true);
    $("[id$=ddlTrnStatus]").val(-1);
    $("[id$=ddlPlantCode]").val(0);
    $("[id$=ddlIssuingStore]").val(0);
    $("[id$=txtIssueTo]").val("Translate(AutoDefaultValue)");
    $("[id$=ICH_ISS_RCV_PK]").val(0);
    $("[id$=txtItemCategory]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfItemCatPK]").val(0);
    $("[id$=txtItem]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfItemPk]").val(0);
    SearchInit();
    if (TRX_TYPE != 3)
        $("select[id$=ddlType]").val(0);
    $("[id$=ddlPlantCode]").val("-1");

    $("select[id$=ddlType]").val(0);
    $("[id$=txtLotno]").val("");
    $("[id$=txtRefNo]").val("");
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    //    Type 2 for material receipt
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", RequisitionList.AutoCompleteURL + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&TYPE=" + TRX_TYPE, "SearchType");
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
    issuingHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.ConsumptionHeaderId, $(tr).parent().attr("id"));
    var TransDate = GrandGrid.Utilities.GetColumnValue(tr, "ICH_DATE", $(tr).parent().attr("id"));
    switch (command.toString()) {


        case RequisitionList.PerformAction:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                window.location = RequisitionList.ADDNEWURL + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId;
            }
            break;
        // To Delete Details           
        case RequisitionList.DeleteCommand:
            $.get(RequisitionList.InventoryLockCheckingURL + TransDate + "&Module=2", function (data) {
                if (data != null && data.length > 0) {
                    if (parseInt(data[0]) == 0) {
                        // Do Confirmation.. Before Delete Details
                        GrandScriptUtils.ShowModal(RequisitionList.DeleteConfirmationMessage, RequisitionList.ConfirmationMessage, RequisitionList.DeleteMessageCommand, true);
                    }
                    else {
                        var Err_TranslockedMsg = RequisitionList.ErrTransLockedMsg + RequisitionList.ValueEmpty + data[1];
                        GrandScriptUtils.ShowModal(Err_TranslockedMsg.fontcolor("red"), RequisitionList.MessageBoxTitle);
                    }
                }
            });
            break;
        case RequisitionList.CANCELCommand:
            $.get(RequisitionList.InventoryLockCheckingURL + TransDate + "&Module=2", function (data) {
                if (data != null && data.length > 0) {
                    if (parseInt(data[0]) == 0) {
                        issuingHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.ConsumptionHeaderId, $(tr).parent().attr("id"));
                        GrandScriptUtils.ShowModal(RequisitionList.CancelMessage, RequisitionList.ConfirmationMessage, RequisitionList.CANCELCommand, true);
                    }
                    else {
                        var Err_TranslockedMsg = RequisitionList.ErrTransLockedMsg + RequisitionList.ValueEmpty + data[1];
                        GrandScriptUtils.ShowModal(Err_TranslockedMsg.fontcolor("red"), RequisitionList.MessageBoxTitle);
                    }
                }
            });
            return false;
            break;
        // To Edit Details                   
        case RequisitionList.EditCommand:
            FillDetails(tr);
            break;
        case RequisitionList.MODIFY:
            if (TRX_TYPE == 2) { //EMR      
                var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));   
                window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1&IsModify=1&TYPE=2" + "&RefID=" + refID
            }
            break;
        case RequisitionList.View:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1 || UserStatus == 4) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                if (TRX_TYPE == 4) {
                    window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1" + "&TYPE=4" + "&RefID=" + refID;
                }
                else {
                    window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1" + "&RefID=" + refID;
                }
            }

            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    if (TRX_TYPE == 4) {
                        window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1" + "&TYPE=4";
                    }
                    else {
                        window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1";
                    }
                }
                else {
                    if (TRX_TYPE == 4) {
                        window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1" + "&TYPE=4" + "&RefID=" + refID ;
                    }
                    else {
                        window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1" + "&RefID=" + refID ;
                    }
                }
            }
            return false;
            break;
        case RequisitionList.Print:
            var win;
            if (TRX_TYPE == 4) {//Opening Stock
                win = window.open(RequisitionList.REPORTURL + '?ID=' + issuingHeaderId + '&APPTYPE=OS', '_blank', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');
            }
            else {
                win = window.open(RequisitionList.REPORTURL + '?ID=' + issuingHeaderId + '&APPTYPE=EMR', '_blank', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');
            }
            if (!win) {
                var eMsg = "<span><ul><li>" + errorMessage + ' ' + window.location.host + "</li></ul></span>";
                GrandScriptUtils.ShowModal(eMsg, errorTitle);
                $("#MSGBox").addClass("error");
            }
            //var url = RequisitionList.PRINTURL + "?IssueID=" + issuingHeaderId;
            //OpenPDF(url);
            return false;
            break;
        // Default Handler  Print        
        default:
            alert(RequisitionList.DefaultAction);
            break;
    }
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
        case RequisitionList.CANCELCommand:
            CancelEMRDetails();
            break;
    }
    return false;
}

function CancelEMRDetails() {
    var msgtxt;
    $.get(RequisitionList.DeleteRowURL + issuingHeaderId + "&UserPK=" + $("[id$=UserPk]").val() +"&HasWorkflow=1", function (data) {
        if (parseInt(data) == 1) {
            msgtxt = RequisitionList.CANCELSUCESS;
        }
        else if (parseInt(data) == 0) {
            msgtxt = "EMR is already used in another place.";
        }
        else if (parseInt(data) == -10) {
            msgtxt = RequisitionList.UNABLETOCANCEL;
        }
        else {
            msgtxt = RequisitionList.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, RequisitionList.MessageBoxTitle, RequisitionList.DeleteCommand);
    });
    return false;

}

//************Advance Search Functions****************************************************
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
function FillIssuingToList(issueTypeID, issueTypeText) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>    
    var issuingType;
    if ($("select[id$=ddlType]").val() == null)
        issuingType = $("[id$=hdn_ICH_ISS_RCV_TYPE]").val();
    else
        issuingType = $("select[id$=ddlType]").val();
    var DeptType = 0;
    if (TRX_TYPE == 3) {
        DeptType = 2; //For Showing Stock Department only
        $("select[id$=ddlType]").attr("disabled", true);
    }
    GrandScriptUtils.MakeAutoCompleteLimitLen("txtIssueTo", RequisitionList.FillIssuingToList + $("[id$=BizUnitPk]").val() + "&issuingType=" + issuingType + "&Despatch=1" + "&DPT_TYPE=" + DeptType, "ICH_ISS_RCV_PK", true, false, "txtIssueTo", true, "Select", "", "", 0, afterAutoComplete, "Translate(AutoDefaultValue)", false);
}
function FillIsuingType(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    var drpID = $("select[id$=ddlType]").attr("id");
    $.get(RequisitionList.FillIssuingType + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0", function (data) {
        if (drpID != null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
            FillIssuingToList();
        }

    });
}
function FillStore(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    var drpID = $("select[id$=ddlIssuingStore]").attr("id");
    $.get(RequisitionList.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=1&DeptType=2&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
    });
}
function FillTypes(typeID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=ICH_ITEM_TYPE]").attr("id");
    $.get(RequisitionList.FillMaterialTypeDropdownURL + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + "Item Type", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, typeID);
    });
}
function afterAutoComplete() {
    $("[id$=ICH_ISS_RCV_PK]").val(selectVal);
    if (selectText != "")
        $("[id$=txtIssueTo]").val(selectText);
}
function FillIssueToCategories() {
    FillIssuingToList(0);
}
function SearchAutoInit() {
    ClearSearchDetails();
    BindGrid();
    return false;
}
function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("txtItemCategory", RequisitionList.MaterialCategroyDeptURL + "&Type=0", "hdfItemCatPK", true, false, "BizUnitPk", true);
}
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "txtItemCategory") {
        $("[id$=hdfItemPk]").val("0");
        $("[id$=txtItem]").val("<%= Resources.Messages.AutoDefaultValue %>");
        FillCategoryMaterials($("[id$=hdfItemCatPK]").val());
    }
}
function FillCategoryMaterials(categoryID) {
    //<summary>Function Used to Fill material based on the category  </summary>     
    GrandScriptUtils.MakeAutoComplete("txtItem", RequisitionList.MaterialURL + $("[id$=BizUnitPk]").val() + "&Type=-1" + "&FLDNAME=ITM_TEXT", "hdfItemPk", true, false, "hdfItemCatPK", true);
}

function FillCompanyDisplayNames() {
    ///<summary>function used to fill Plant Code </summary>    
    var drpID = $("select[id$=ddlPlantCode]").attr("id");
    $.get(RequisitionList.AutoCompleteURL + $("[id$=BizUnitPk]").val() + "&SearchType=CMP_DISPLAY_CODE" + "&PageURL=" + RequisitionList.PAGEURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, false, false, false, false, true);
    });
}

//********************End Advance search functions **********************************************


///#endregion

///#endregion


