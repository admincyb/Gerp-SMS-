///#region ------- Global Variable -----
var ginPK = 0;
//var procID = 5;
var pk = 0;
///#endregion

//#region ------- Configuration Section -------
var GoodsInspection = {
    // URL
    GOODSINSPECTIONBINDGRIDURL: "GoodsInspectionNote.do?Action=GetGINList&Status=",
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    GOODSINSPECTIONDELETEURL: "GoodsInspectionNote.do?Action=DeleteGIN&PK=",
    GOODSINSPECTIONAUTOCOMPLETEURL: "GoodsInspectionNote.do?Action=GetSearchValue&AUTOSEARCH=1&SBU=",
    GOODSINSPECTIONENTRYURL: "GINCreate.aspx",
    GOODSINSPECTIONREPORTURL: "PurchaseRequestReport.aspx",
    GINREPORTURL: "../Reports/GINReport.aspx",
    PerformAction: "PERFORMACTION",
    PAGEURL: "/StoreManagement/GINCreate.aspx",
    REPORTURL: "../Reports/GenerateReport.aspx",
    // Constant
    SAVECMD: "Save",
    LOGOUT: "LOGOUT",
    DELETECOMMAND: "DELETE",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    MODIFY: "MODIFY",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    GINPK: "GIH_PK",
    USERSTATUS: "USER_STATUS",
    View: "VIEW",
    PRINT: "PRINT",
    RefID: "REF_ID",
    GIHNO: "GIH_NO",
    GRNPK: "GIH_GRN_HDR",
    GIH_STATUS: "GIH_STATUS",
    CANCEL: "CANCEL",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Translate(GINDetailsdeletedsuccessfully)",
    InspSaveMsg1: "Translate(GoodsInspectionNoteSaveMsg1)",
    InspSaveMsg2: "Translate(GINSaveSuccessmsg2)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    CANCELSUCESS: "Translate(GINCanceledSuccessfully)",
    UNABLETOCANCEL: "Translate(CannotCancelGIN)",
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
    ShowHideAdvancedSearch();
});

function PageInit() {
    ///<summary>Initial page condition</summary>
    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) == 1) {
        $("[id$=divPlantCode]").show();
        FillCompanyDisplayNames();
    }
    else {
        $("[id$=divPlantCode]").hide();
    }
    $("select[id$=SearchType]").val(GoodsInspection.TEXTZERO);
    $("[id$=SearchValue]").val(GoodsInspection.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    return false;
}

///#endregion

///#region ------- Core Section -------


///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    $.get(GoodsInspection.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GoodsInspection.SessionExpired, GoodsInspection.Confirmation, GoodsInspection.LOGOUT, true);
            result = false;
        }
        else {
            window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?TYPE=" + $("[id$=hdfType]").val();
        }
    });

    return false;
}

//<summary>function Used to Reset Page</summary>
function ResetPage() {
    ClearSearchDetails();
    PageInit();
    return false;
}

///#endregion

function SearchAutoInit() {
    ClearSearchDetails();
    BindGrid();
    return false;
}

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(GoodsInspection.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    if (strname == GoodsInspection.TEXTZERO) {
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
        GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        //        $("[id$=imbSearch]").show();
    }

    ClearSearchDetails();
}


function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=FromDate]").val(GoodsInspection.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=ToDate]").val(GoodsInspection.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(GoodsInspection.TEXTEMPTY);

    $("[id$=txtFromDate]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=hdfFromDate]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=txtToDate]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=hdfToDateNew]").val(GoodsInspection.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    $("[id$=ddlStatus]").val(-1);
    $("[id$=txtPONo]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfPONumber]").val(0);
    $("[id$=txtVendor]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfVendor]").val(0);
    $("[id$=txtGRNNumber]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfGRNNumber]").val(0);
    $("[id$=txtGINNo]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfGINNo]").val(0);
    $("[id$=txtDepartment]").val("Translate(AutoDefaultValue)");
    $("[id$=ddlPlantCode]").val("-1");

}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    //    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val()+ "&PageUrl=" + GoodsInspection.PAGEURL+ "?TYPE=" + $("[id$=hdfType]").val(), "SearchType");
    GrandScriptUtils.MakeAutoComplete("txtVendor", GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GIH_VENDOR_TEXT" + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(), "hdfVendor", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtPONo", GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GIH_PO_NO" + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(), "hdfPONumber", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtGRNNumber", GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GIH_GRH_NO" + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(), "hdfGRNNumber", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtGINNo", GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=GIH_NO" + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(), "hdfGINNo", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtDepartment", GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=DPT_NAME" + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(), "hdfDepPk", true, false, false, true);
}

///#endregion

function FillDetails(tr) {
    ///<summary>Function To Fill Purchase Request Details  </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    pk = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GINPK, $(tr).parent().attr("id"));
    var status = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.USERSTATUS, $(tr).parent().attr("id"))
    window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?PK=" + pk + "&Status=" + status;
    return false;
}


function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>
    var msgtxt;
    $.get(GoodsInspection.GOODSINSPECTIONDELETEURL + ginPK, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = GoodsInspection.DELETESUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Assigned";
        else
            msgtxt = GoodsInspection.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, GoodsInspection.INFORMATIONTITLE, GoodsInspection.SAVECMD);

    });
    return false;
}

function BindGrid() {
    ///<summary>Bind Designaion Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>
    //var ajaxUrl = GoodsInspection.GOODSINSPECTIONBINDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&FilterStatus=" + $("select[id$=FilterStatus]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(); 
    var poNumber = "", grnNo = "", ginNo = "", deptName = "";
    poNumber = $("[id$=txtPONo]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtPONo]").val();
    grnNo = $("[id$=txtGRNNumber]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtGRNNumber]").val();
    ginNo = $("[id$=txtGINNo]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtGINNo]").val();
    deptName = $("[id$=txtDepartment]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtDepartment]").val();
    var ajaxUrl = GoodsInspection.GOODSINSPECTIONBINDGRIDURL + $("[id$=ddlStatus]").val() + "&GINNo=" + ginNo + "&GRNNo=" + grnNo + "&Vendor=" + $("[id$=hdfVendor]").val() + "&PONo=" + poNumber + "&Dept=" + deptName + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=txtFromDate]").val() + "&ToDate=" + $("[id$=txtToDate]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val() + "&CMP_PK=" + $("[id$=ddlPlantCode]").val();
    $("#grdGINList").removeAttr("ajaxurl")
    $("#grdGINList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdGINList");
    GrandGrid.MakeGrid($("#grdGINList"));
    ShowHideAdvancedSearch();
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    $.get(GoodsInspection.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GoodsInspection.SessionExpired, GoodsInspection.Confirmation, GoodsInspection.LOGOUT, true);
            result = false;
        }
        else {
            pk = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GINPK, $(tr).parent().attr("id"));
            switch (command.toString()) {
                // To Delete Details       
                case GoodsInspection.PerformAction:
                    //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.USERSTATUS, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.RefID, $(tr).parent().attr("id"));
                        window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?RefID=" + refID + "&TYPE=" + $("[id$=hdfType]").val();
                    }
                    else if (UserStatus == 2) {
                        window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?PK=" + pk + "&TYPE=" + $("[id$=hdfType]").val();
                    }
                    return false;
                    break;
                case GoodsInspection.DELETECOMMAND:
                    ginPK = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GINPK, $(tr).parent().attr("id"));
                    GrandScriptUtils.ShowModal(GoodsInspection.DELETECONFIRMMSG, GoodsInspection.CONFIRMMSG, GoodsInspection.DELETE, true);
                    break;
                // To Edit Details                   
                case GoodsInspection.EDITCOMMAND:
                    FillDetails(tr);
                    break;
                case GoodsInspection.MODIFY:
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.RefID, $(tr).parent().attr("id"));
                    window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?RefID=" + refID + "&Status=1&TYPE=" + $("[id$=hdfType]").val() + "&IsModify=1";
                    return false;
                    break;
                case GoodsInspection.CANCEL:
                    ginPK = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GINPK, $(tr).parent().attr("id"));
                    GrandScriptUtils.ShowModal(GoodsInspection.CancelMessage, GoodsInspection.CONFIRMMSG, GoodsInspection.CANCEL, true);
                    return false;
                    break;
                case GoodsInspection.View:
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.USERSTATUS, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.RefID, $(tr).parent().attr("id"));
                        window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?RefID=" + refID + "&Status=1&TYPE=" + $("[id$=hdfType]").val();
                    }
                    else if (UserStatus == 2) {
                        window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?PK=" + pk + "&Status=1&TYPE=" + $("[id$=hdfType]").val();
                    }
                    else if (UserStatus == 0) {

                        //window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?PK=" + pk + "&Status=2";
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.RefID, $(tr).parent().attr("id"));
                        if (refID == 0) {
                            window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?PK=" + pk + "&Status=1";
                        }
                        else {
                            window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?RefID=" + refID + "&Status=0&TYPE=" + $("[id$=hdfType]").val();
                        }
                    }
                    return false;
                    break;
                case GoodsInspection.PRINT:
                    var prID = 0;
                    var url = "";
                    if ($("[id$=hdfClient]").val() == "EKK") {
                        prID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GRNPK, $(tr).parent().attr("id"));
                        url = GoodsInspection.REPORTURL + "?ID=" + prID + "&APPTYPE=GRN" + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    }
                    else {
                        prID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GINPK, $(tr).parent().attr("id"));
                        url = GoodsInspection.REPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    }
                    //            //IGCL
                    //            var prID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GINPK, $(tr).parent().attr("id"));
                    //            var url = GoodsInspection.REPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    //            //EKK
                    //var prID = GrandGrid.Utilities.GetColumnValue(tr, GoodsInspection.GRNPK, $(tr).parent().attr("id"));
                    //var url = GoodsInspection.REPORTURL + "?ID=" + prID + "&APPTYPE=GRN" + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    OpenPDF(url);

                    break;
                // Default Handler     
                default:
                    GrandScriptUtils.ShowModal(GoodsInspection.DEFAULTACTION, GoodsInspection.INFORMATIONTITLE);
                    break;
            }
        }
    });
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///    
    /// </param>
    switch (command) {

        case GoodsInspection.SAVECMD:
            PageInit();
            break;
        case GoodsInspection.DELETE:
            DeleteDetails();
            break;
        case GoodsInspection.CANCEL:
            CancelGINDetails();
            break;
        case GoodsInspection.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    //var status = 0;
    var isModifyGIN = $("[id$=hdnModifyGIN]").val();
    var isCancelGIN = $("[id$=hdnCancelGIN]").val();
    $("#grdGINList tr:has(td)").each(function () {

        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), GoodsInspection.USERSTATUS, tableID);
        var GIH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), GoodsInspection.GIH_STATUS, tableID);
        //  var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        //Action To perform for the logged in user
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) { //0
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        //Draft will have this status
        else if (UserStatus == 2) {   //2
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
        }
        if (GIH_STATUS == 4) { //Cancelled
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbView]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
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

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GoodsInspection.GIHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), GoodsInspection.GIHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), GoodsInspection.GIHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), GoodsInspection.GIHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(GoodsInspection.DocGenerationNewValue);
        }
        if (isModifyGIN == "1" && UserStatus == 0 && GIH_STATUS != 0 && GIH_STATUS != 3 && GIH_STATUS != 4 && GIH_STATUS != 5) { //GIH_STATUS=> 4 (Cancelled),5(Closed),3(Rejected)
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }
        if (isCancelGIN == "1" && GIH_STATUS != 0 && GIH_STATUS != 3 && GIH_STATUS != 4 && GIH_STATUS != 5) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }
        //NEW
        var value = $(this).find('td:eq(0)').text();
        $(this).find("a[id=lnkPrintNew]").attr("href", GoodsInspection.REPORTURL + "?ID=" + value + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val());

    });

}

function CancelGINDetails() {
    var msgtxt;
    $.get(GoodsInspection.GOODSINSPECTIONDELETEURL + ginPK, function (data) {
        if (parseInt(data) == 1) {
            msgtxt = GoodsInspection.CANCELSUCESS;
        }
        else if (parseInt(data) == 0) {
            msgtxt = "Assigned";
        }
        else if (parseInt(data) == -10) {
            msgtxt = GoodsInspection.UNABLETOCANCEL;
        }
        else {
            msgtxt = GoodsInspection.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, GoodsInspection.INFORMATIONTITLE, GoodsInspection.SAVECMD);
    });
    return false;

}

///#endregion

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
    $.get(GoodsInspection.GOODSINSPECTIONAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&SearchType=CMP_DISPLAY_CODE" + "&PageUrl=" + GoodsInspection.PAGEURL + "?TYPE=" + $("[id$=hdfType]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, false, false, false, false, true);
    });
}
