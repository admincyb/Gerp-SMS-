///#region ------- Global Variable -----
var grnTPK = 0;
//var procID = 5;
var pk = 0;
///#endregion

//#region ------- Configuration Section -------
var GRNList = {

    // URL
    GRNLISTURL: "GoodsReceiptNote.do?Action=GetGRNList&Status=",
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    GRNDELETEURL: "GoodsReceiptNote.do?Action=DeleteGRN&PK=",
    PURCHASEREQUESTAUTOCOMPLETEURL: "GoodsReceiptNote.do?Action=GetSearchValue&AUTOSEARCH=1&SBU=",
    PURCHASERQSTENTRYURL: "GRNCreate.aspx",
    PURCHASEREQUESTREPORTURL: "PurchaseRequestReport.aspx",
    GRNREPORTURL: "../Reports/GRNReport.aspx",
    PerformAction: "PERFORMACTION",
    PAGEURL: "/storemanagement/grncreate.aspx",
    REPORTURL: "../Reports/GenerateReport.aspx",
    PurchaseSearchAutoCompleteURL: "POGeneration.do?Action=GetPurchaseAutoSearchValue&AUTOSEARCH=1",
    FillCompanyDisplayNameURL: "CommonManagement.do?Action=GetCompanyDisplayNames&SBUPk=",

    // Constant
    SAVECMD: "Save",
    LOGOUT: "LOGOUT",
    DELETECOMMAND: "DELETE",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    GRNTPK: "GRH_PK",
    USERSTATUS: "USER_STATUS",
    GRH_STATUS: "GRH_STATUS",
    View: "VIEW",
    PRINT: "PRINT",
    RefID: "REF_ID",
    GRHNO: "GRH_NO",
    MODIFY: "MODIFY",
    CANCEL: "CANCEL",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Translate(GoodReceiptDeletedSuccessfully)",
    SaveMessage1: "Translate(GoodsReceiptSaved1)",
    SaveMessage2: "Translate(GoodsReceiptSaved2)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    CANCELSUCESS: "Translate(GRNCanceledSuccessfully)",
    UnableToCancel_GIN_Exist: "Translate(UnableToCancel_GIN_Exist)",
    UnableToCancel_INV_Exist: "Translate(UnableToCancel_INV_Exist)",
    CancelMessage: "Translate(ConfirmCancel)",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)"


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
    ShowWorkflowSaveMsg();
    ShowHideAdvancedSearch();
});

function PageInit() {
    ///<summary>Initial page condition</summary>

    $("select[id$=SearchType]").val(GRNList.TEXTZERO);
    $("[id$=SearchValue]").val(GRNList.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();

    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) == 1) {
        $("[id$=divPlantCode]").show();
        FillCompanyDisplayNames();
    }
    else {
        $("[id$=divPlantCode]").hide();
    }

    return false;
}

///#endregion

///#region ------- Core Section -------

///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    $.get(GRNList.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GRNList.SessionExpired, GRNList.Confirmation, GRNList.LOGOUT, true);
            result = false;
        }
        else {
            window.location = GRNList.PURCHASERQSTENTRYURL;
        }
    });

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

    ClearSearchDetails();
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(GRNList.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    if (strname == GRNList.TEXTZERO) {
        //$("[id$=SearchValue]").hide()
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        //        $("[id$=imbSearch]").hide();
        BindGrid();
    }
    else if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        //        $("[id$=imbSearch]").show();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        //        $("[id$=imbSearch]").show();
    }
}

function SearchAutoInit() {
    ClearSearchDetails();
    BindGrid();
    return false;
}
function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val(GRNList.TEXTEMPTY);
    $("[id$=FromDate]").val(GRNList.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(GRNList.TEXTEMPTY);
    $("[id$=ToDate]").val(GRNList.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(GRNList.TEXTEMPTY);

    $("[id$=txtFromDate]").val(GRNList.TEXTEMPTY);
    $("[id$=hdfFromDate]").val(GRNList.TEXTEMPTY);
    $("[id$=txtToDate]").val(GRNList.TEXTEMPTY);
    $("[id$=hdfToDateNew]").val(GRNList.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    $("[id$=ddlStatus]").val(9); //Not Closed
    $("[id$=txtPONo]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfPONumber]").val(0);
    $("[id$=txtVendor]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfVendor]").val(0);
    $("[id$=txtGRNNumber]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfGRNNumber]").val(0);
    $("[id$=txtReferenceNo]").val(GRNList.TEXTEMPTY);
    $("[id$=ddlPlantCode]").val("-1");

}

function SearchInit() {
    ///<summary>To handle auto complete</summary>    
    GrandScriptUtils.MakeAutoComplete("txtVendor", GRNList.PURCHASEREQUESTAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GRH_VENDOR_TEXT" + "&PageUrl=" + GRNList.PAGEURL, "hdfVendor", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtPONo", GRNList.PURCHASEREQUESTAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GRH_PO_NO" + "&PageUrl=" + GRNList.PAGEURL, "hdfPONumber", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtGRNNumber", GRNList.PURCHASEREQUESTAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GRH_NO" + "&PageUrl=" + GRNList.PAGEURL, "hdfGRNNumber", true, false, false, true);
}

///#endregion

function FillDetails(tr) {
    ///<summary>Function To Fill Purchase Request Details  </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, GRNList.GRNTPK, $(tr).parent().attr("id"));
    var status = GrandGrid.Utilities.GetColumnValue(tr, GRNList.USERSTATUS, $(tr).parent().attr("id"))
    window.location = GRNList.PURCHASERQSTENTRYURL + "?PK=" + pk + "&Status=" + status;
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(GRNList.GRNDELETEURL + grnTPK, function (data) {
        if (parseInt(data) == 1)
            msgtxt = GRNList.DELETESUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Assigned";
        else
            msgtxt = GRNList.ACTIONFAILEDMSG;
        GrandScriptUtils.ShowModal(msgtxt, GRNList.INFORMATIONTITLE, GRNList.SAVECMD);

    });
    return false;
}

function BindGrid() {
    ///<summary>Bind Designaion Details With Search value </summary>    
    var poNumber = "", grnNo = "";
    poNumber = $("[id$=txtPONo]").val() == "Select/Type" ? "" : $("[id$=txtPONo]").val();
    grnNo = $("[id$=txtGRNNumber]").val() == "Select/Type" ? "" : $("[id$=txtGRNNumber]").val();
    var ajaxUrl = GRNList.GRNLISTURL + $("[id$=ddlStatus]").val() + "&GRNNo=" + grnNo + "&Vendor=" + $("[id$=hdfVendor]").val() + "&PONo=" + poNumber + "&RefNo=" + $("[id$=txtReferenceNo]").val() + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=txtFromDate]").val() + "&ToDate=" + $("[id$=txtToDate]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + GRNList.PAGEURL + "&CMP_PK=" + $("[id$=ddlPlantCode]").val();
    $("#grdGRNList").removeAttr("ajaxurl")
    $("#grdGRNList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdGRNList");
    GrandGrid.MakeGrid($("#grdGRNList"));
    ShowHideAdvancedSearch();
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>

    BindGrid();
}


function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if (pK[0] == "No") {
                GrandScriptUtils.ShowModal(GRNList.SaveMessage1 + " " + pK[1] + " " + GRNList.SaveMessage2, GRNList.INFORMATIONTITLE);
            }
        }
    }
}

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>


    ///<summary>Function To Show Data Entry Form </summary>
    $.get(GRNList.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GRNList.SessionExpired, GRNList.Confirmation, GRNList.LOGOUT, true);
            result = false;
        }
        else {
            pk = GrandGrid.Utilities.GetColumnValue(tr, GRNList.GRNTPK, $(tr).parent().attr("id"));
            switch (command.toString()) {
                case GRNList.PerformAction:
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, GRNList.USERSTATUS, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, GRNList.RefID, $(tr).parent().attr("id"));
                        window.location = GRNList.PURCHASERQSTENTRYURL + "?RefID=" + refID;
                    }
                    else if (UserStatus == 2) {
                        window.location = GRNList.PURCHASERQSTENTRYURL + "?PK=" + pk;
                    }
                    return false;
                    break;
                case GRNList.DELETECOMMAND:
                    grnTPK = GrandGrid.Utilities.GetColumnValue(tr, GRNList.GRNTPK, $(tr).parent().attr("id"));
                    GrandScriptUtils.ShowModal(GRNList.DELETECONFIRMMSG, GRNList.CONFIRMMSG, GRNList.DELETE, true);
                    break;
                case GRNList.EDITCOMMAND:
                    FillDetails(tr);
                    break;
                case GRNList.View:
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, GRNList.USERSTATUS, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, GRNList.RefID, $(tr).parent().attr("id"));
                        window.location = GRNList.PURCHASERQSTENTRYURL + "?RefID=" + refID + "&Status=1";
                    }
                    else if (UserStatus == 2) {
                        window.location = GRNList.PURCHASERQSTENTRYURL + "?PK=" + pk + "&Status=1";
                    }
                    else if (UserStatus == 0) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, GRNList.RefID, $(tr).parent().attr("id"));
                        if (refID == 0) {
                            window.location = GRNList.PURCHASERQSTENTRYURL + "?PK=" + pk + "&Status=1";
                        }
                        else {
                            window.location = GRNList.PURCHASERQSTENTRYURL + "?RefID=" + refID + "&Status=1";
                        }
                    }
                    return false;
                    break;
                case GRNList.MODIFY:
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, GRNList.RefID, $(tr).parent().attr("id"));
                    window.location = GRNList.PURCHASERQSTENTRYURL + "?RefID=" + refID + "&Status=1&IsModify=1";
                    return false;
                    break;
                case GRNList.CANCEL:
                    grnTPK = GrandGrid.Utilities.GetColumnValue(tr, GRNList.GRNTPK, $(tr).parent().attr("id"));
                    GrandScriptUtils.ShowModal(GRNList.CancelMessage, GRNList.INFORMATIONTITLE, GRNList.CANCEL, true);
                    return false;
                    break;
                case GRNList.PRINT:
                    var prID = GrandGrid.Utilities.GetColumnValue(tr, GRNList.GRNTPK, $(tr).parent().attr("id"));
                    //            window.location = GRNList.GRNREPORTURL + "?PRID=" + prID;
                    //window.location = GRNList.REPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    var url = GRNList.REPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    OpenPDF(url);
                    break;
                // Default Handler     
                default:
                    GrandScriptUtils.ShowModal(GRNList.DEFAULTACTION, GRNList.INFORMATIONTITLE);
                    break;
            }

        }
    });
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case GRNList.SAVECMD:
            PageInit();
            break;
        case GRNList.DELETE:
            DeleteDetails();
            break;
        case GRNList.CANCEL:
            CancelGRNDetails();
            break;
        case GRNList.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    var isModifyPO = $("[id$=hdnModifyGRN]").val();
    var isCancelGRN = $("[id$=hdnCancelGRN]").val();
    $("#grdGRNList tr:has(td)").each(function () {

        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), GRNList.USERSTATUS, tableID);
        var GRH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), GRNList.GRH_STATUS, tableID);

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
        if (GRH_STATUS == 4) { //Cancelled
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbView]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
        }

        if (isModifyPO == "1" && UserStatus == 0 && GRH_STATUS != 4 && GRH_STATUS != 5 && GRH_STATUS != 3) {   //GRH_STATUS=> 4 (Cancelled),5(Closed),3(Rejected)
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }
        if (isCancelGRN == "1" && GRH_STATUS != 0 && GRH_STATUS != 3 && GRH_STATUS != 4 && GRH_STATUS != 5) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }
        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE", tableID);
        if (ColIndex != null) {
            $(this).find("td:eq(" + ColIndex + ")").css("font-weight", "bold");
        }


        //Line Color
        var ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_LINE_COLOUR", tableID);
        if (ColIndex != null) {
            var lineColor = GrandGrid.Utilities.GetColumnValue($(this), "CMP_LINE_COLOUR", tableID);
            if (lineColor != "null") {
                ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE", tableID);
                if (ColIndex != null) {
                    $(this).find("td:eq(" + ColIndex + ")").addClass(lineColor);
                }
            }
        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNList.GRHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), GRNList.GRHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), GRNList.GRHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), GRNList.GRHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(GRNList.DocGenerationNewValue);
        }
    });

}

function CancelGRNDetails() {
    var msgtxt;
    $.get(GRNList.GRNDELETEURL + grnTPK + "&UserPK=" + $("[id$=UserPk]").val(), function (data) {
        if (parseInt(data) == 1) {
            msgtxt = GRNList.CANCELSUCESS;
        }
        else if (parseInt(data) == 0) {
            msgtxt = "Assigned";
        }
        else if (parseInt(data) == -10) {//GIN Entry Exists
            msgtxt = GRNList.UnableToCancel_GIN_Exist;
        }
        else if (parseInt(data) == -11) {//Invoice Entry Exists
            msgtxt = GRNList.UnableToCancel_INV_Exist;
        }
        else {
            msgtxt = GRNList.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, GRNList.INFORMATIONTITLE, GRNList.SAVECMD);
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

///#endregion

function FillCompanyDisplayNames() {
    ///<summary>function used to fill Plant Code </summary>    
    var drpID = $("select[id$=ddlPlantCode]").attr("id");
    $.get(GRNList.PURCHASEREQUESTAUTOCOMPLETEURL + "&SearchType=CMP_DISPLAY_CODE" + "&PageURL=" + GRNList.PAGEURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, false, false, false, false, true);
    });
}