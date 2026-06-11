///#region ------- Global Variable -----
var stPK = 0;
var lastModDt = 0;
var pk = 0;
///#endregion

//#region ------- Configuration Section -------
var StockTransfer = {
    // URL
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    STOCKTRANSFERENTRYURL: "StockTransfer.aspx",
    STOCKTRANSFERBINDGRIDURL: "StockTransfer.do?Action=GetStockTransferList&Status=",
    STOCKTRANSFERDELETEURL: "StockTransfer.do?Action=DeleteStockTransfer&PK=",
    STOCKTRANSFERAUTOCOMPLETEURL: "StockTransfer.do?Action=GetSearchValue&AUTOSEARCH=1&SBU=",
    STOCKTRANSFERREPORTURL: "StockTransfer.aspx",
    ReportUrl: "../StoreManagement/StockTransferReport.aspx",
    PERFORMACTION: "PERFORMACTION",
    PAGEURL: "/storemanagement/StockTransfer.aspx",
    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    LOGOUT: "LOGOUT",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    MODIFY: "MODIFY",
    CANCEL: "CANCEL",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    STOCKTRANSFERPK: "SFH_PK",
    USERSTATUS: "USER_STATUS",
    SFH_STATUS: "SFH_STATUS",
    View: "VIEW",
    PRINT: "PRINT",
    RefID: "REF_ID",
    SFHNO: "SFH_NO",
    SFHPONO: "SFH_PO_NO",
    SFHVENDORTEXT: "SFH_VENDOR_TEXT",
    ValueEmpty: ' ',
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Stock Transfer details deleted successfully",
    SaveMsg1: "Translate(StockTransferSaveMsg1)",
    SaveMsg2: "Translate(StockTransferSaveMsg2)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    CANCELSUCESS: "Translate(SACanceledSuccessfully)",
    UNABLETOCANCEL: "Translate(UnableToCancelMsg)",
    CancelMessage: "Translate(ConfirmCancel)",
    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)"

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
    $("select[id$=SearchType]").val(StockTransfer.TEXTZERO);
    $("[id$=SearchValue]").val(StockTransfer.TEXTEMPTY);
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
    $.get(StockTransfer.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(StockTransfer.SessionExpired, StockTransfer.Confirmation, StockTransfer.LOGOUT, true);
            result = false;
        }
        else {
            window.location = StockTransfer.STOCKTRANSFERENTRYURL;
        }
    });
    return false;
}

//function AddNew() {
//    ///<summary>Function To Show Data Entry Form </summary>
//    window.location = StockTransfer.STOCKTRANSFERENTRYURL;
//    return false;
//}

//<summary>function Used to Reset Page</summary>
function ResetPage() {
    ClearSearchDetails();
    PageInit();
    return false;
}
function SearchAutoInit() {
    ClearSearchDetails();
    BindGrid();
    return false;
}
///#endregion

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(StockTransfer.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    if (strname == StockTransfer.TEXTZERO) {
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

    ClearSearchDetails();
}
///#endregion

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(StockTransfer.TEXTEMPTY);
    $("[id$=FromDate]").val(StockTransfer.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(StockTransfer.TEXTEMPTY);
    $("[id$=ToDate]").val(StockTransfer.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(StockTransfer.TEXTEMPTY);

    $("[id$=txtFromDate]").val(StockTransfer.TEXTEMPTY);
    $("[id$=hdfFromDate]").val(StockTransfer.TEXTEMPTY);
    $("[id$=txtToDate]").val(StockTransfer.TEXTEMPTY);
    $("[id$=hdfToDateNew]").val(StockTransfer.TEXTEMPTY);
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDateNew", false, false);
    $("[id$=ddlStatus]").val("0");
    $("[id$=txtPONo]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfPONumber]").val(0);
    $("[id$=txtVendor]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfVendor]").val(0);
    $("[id$=txtGRNNumber]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfGRNNumber]").val(0);
    $("[id$=txtGINNo]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfGINNo]").val(0);
    $("[id$=txtDepartment]").val("Translate(AutoDefaultValue)");
    $("[id$=txtSANo]").val("Translate(AutoDefaultValue)");
    $("[id$=hdfSANo]").val(0);
    $("[id$=ddlPlantCode]").val("-1");

}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    // GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val(), "SearchType");
    GrandScriptUtils.MakeAutoComplete("txtVendor", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=SFH_VENDOR_TEXT" + "&PageUrl=" + StockTransfer.PAGEURL, "hdfVendor", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtPONo", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=SFH_PO_NO" + "&PageUrl=" + StockTransfer.PAGEURL, "hdfPONumber", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtGRNNumber", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=SFH_GRN_NO" + "&PageUrl=" + StockTransfer.PAGEURL, "hdfGRNNumber", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtGINNo", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=SFH_GIN_NO" + "&PageUrl=" + StockTransfer.PAGEURL, "hdfGINNo", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtDepartment", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=DPT_NAME" + "&PageUrl=" + StockTransfer.PAGEURL, "hdfDepPk", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtSANo", StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=SFH_NO" + "&PageUrl=" + StockTransfer.PAGEURL, "hdfSANo", true, false, false, true);
}

function FillDetails(tr) {
    ///<summary>Function To Fill Stock Transfer Details  </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    pk = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.STOCKTRANSFERPK, $(tr).parent().attr("id"));
    var status = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.USERSTATUS, $(tr).parent().attr("id"))
    window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?PK=" + pk + "&Status=" + status;
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Stock Transfer Details </summary>
    var msgtxt;
    $.get(StockTransfer.STOCKTRANSFERDELETEURL + stPK + "&LastModDt=" + "", function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = StockTransfer.DELETESUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Assigned";
        else
            msgtxt = StockTransfer.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, StockTransfer.INFORMATIONTITLE, StockTransfer.SAVECMD);

    });
    return false;
}

function BindGrid() {
    ///<summary>Bind Stock Transfer Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>
    //var ajaxUrl = StockTransfer.STOCKTRANSFERBINDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&FilterStatus=" + $("select[id$=FilterStatus]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + StockTransfer.PAGEURL; 
    var poNumber = "", grnNo = "", ginNo = "", deptName = "", saNo = "";
    poNumber = $("[id$=txtPONo]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtPONo]").val();
    grnNo = $("[id$=txtGRNNumber]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtGRNNumber]").val();
    ginNo = $("[id$=txtGINNo]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtGINNo]").val();
    deptName = $("[id$=txtDepartment]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtDepartment]").val();
    saNo = $("[id$=txtSANo]").val() == "Translate(AutoDefaultValue)" ? "" : $("[id$=txtSANo]").val();
    var ajaxUrl = StockTransfer.STOCKTRANSFERBINDGRIDURL + $("[id$=ddlStatus]").val() + "&SANo=" + saNo + "&GINNo=" + ginNo + "&GRNNo=" + grnNo + "&Vendor=" + $("[id$=hdfVendor]").val() + "&PONo=" + poNumber + "&Dept=" + deptName + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=txtFromDate]").val() + "&ToDate=" + $("[id$=txtToDate]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + StockTransfer.PAGEURL + "&CMP_PK=" + $("[id$=ddlPlantCode]").val();
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
    $.get(StockTransfer.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(StockTransfer.SessionExpired, StockTransfer.Confirmation, StockTransfer.LOGOUT, true);
            result = false;
        }
        else {
            pk = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.STOCKTRANSFERPK, $(tr).parent().attr("id"));
            var TransDate = GrandGrid.Utilities.GetColumnValue(tr, "SFH_DATE", $(tr).parent().attr("id"));
            switch (command.toString()) {
                // To Delete Details         
                case StockTransfer.PERFORMACTION:
                    //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.USERSTATUS, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.RefID, $(tr).parent().attr("id"));
                        window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?RefID=" + refID;
                    }
                    else if (UserStatus == 2) {
                        window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?PK=" + pk;
                    }
                    return false;
                    break;
                case StockTransfer.DELETECOMMAND:
                    $.get(StockTransfer.InventoryLockCheckingURL + TransDate + "&Module=2", function (data) {
                        if (data != null && data.length > 0) {
                            if (parseInt(data[0]) == 0) {
                                stPK = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.STOCKTRANSFERPK, $(tr).parent().attr("id"));
                                lastModDt = GrandGrid.Utilities.GetColumnValue(tr, "LAST_MOD_DT", $(tr).parent().attr("id"));
                                GrandScriptUtils.ShowModal(StockTransfer.DELETECONFIRMMSG, StockTransfer.CONFIRMMSG, StockTransfer.DELETE, true);
                            }
                            else {
                                var Err_TranslockedMsg = StockTransfer.ErrTransLockedMsg + StockTransfer.ValueEmpty + data[1];
                                GrandScriptUtils.ShowModal(Err_TranslockedMsg.fontcolor("red"), StockTransfer.INFORMATIONTITLE);
                            }
                        }
                    });
                    break;
                // To Edit Details                     
                case StockTransfer.EDITCOMMAND:
                    FillDetails(tr);
                    break;
                case StockTransfer.MODIFY:
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.RefID, $(tr).parent().attr("id"));
                    window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?RefID=" + refID + "&Status=1&IsModify=1";
                    return false;
                    break;
                case StockTransfer.CANCEL:
                    $.get(StockTransfer.InventoryLockCheckingURL + TransDate + "&Module=2", function (data) {
                        if (data != null && data.length > 0) {
                            if (parseInt(data[0]) == 0) {
                                stPK = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.STOCKTRANSFERPK, $(tr).parent().attr("id"));
                                GrandScriptUtils.ShowModal(StockTransfer.CancelMessage, StockTransfer.CONFIRMMSG, StockTransfer.CANCEL, true);
                            }
                            else {
                                var Err_TranslockedMsg = StockTransfer.ErrTransLockedMsg + StockTransfer.ValueEmpty + data[1];
                                GrandScriptUtils.ShowModal(Err_TranslockedMsg.fontcolor("red"), StockTransfer.INFORMATIONTITLE);
                            }
                        }
                    });
                    return false;
                    break;
                case StockTransfer.View:
                    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.USERSTATUS, $(tr).parent().attr("id"));
                    if (UserStatus == 1) {
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.RefID, $(tr).parent().attr("id"));
                        window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?RefID=" + refID + "&Status=1";
                    }
                    else if (UserStatus == 2) {
                        window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?PK=" + pk + "&Status=1";
                    }
                    else if (UserStatus == 0) {

                        //window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?PK=" + pk + "&Status=2";
                        var refID = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.RefID, $(tr).parent().attr("id"));
                        if (refID == 0) {
                            window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?PK=" + pk + "&Status=1";
                        }
                        else {
                            window.location = StockTransfer.STOCKTRANSFERENTRYURL + "?RefID=" + refID + "&Status=1";
                        }
                    }
                    return false;
                    break;
                //To Print Transfer Details  
                case StockTransfer.PRINT:
                    //window.location = StockTransfer.ReportUrl + "?PK=" + pk;
                    var url = StockTransfer.ReportUrl + "?PK=" + pk;
                    OpenPDF(url);
                    return false;
                    break;
                // Default Handler       
                default:
                    GrandScriptUtils.ShowModal(StockTransfer.DEFAULTACTION, StockTransfer.INFORMATIONTITLE);
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
        case StockTransfer.SAVECMD:
            PageInit();
            break;
        case StockTransfer.DELETE:
            DeleteDetails();
            break;
        case StockTransfer.CANCEL:
            CancelSADetails();
            break;
        case StockTransfer.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    //var status = 0;
    var isModifySA = $("[id$=hdnModifySA]").val();
    var isCancelSA = $("[id$=hdnCancelSA]").val();
    $("#grdGINList tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.USERSTATUS, tableID);
        var SFH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFH_STATUS, tableID);
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

        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE_TEXT", tableID);
        if (ColIndex != null) {
            $(this).find("td:eq(" + ColIndex + ")").css("font-weight", "bold");
        }

        //Line Color
        var ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_LINE_COLOUR", tableID);
        if (ColIndex != null) {
            var lineColor = GrandGrid.Utilities.GetColumnValue($(this), "CMP_LINE_COLOUR", tableID);
            if (lineColor != "null") {
                ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE_TEXT", tableID);
                if (ColIndex != null) {
                    $(this).find("td:eq(" + ColIndex + ")").addClass(lineColor);
                }
            }
        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.SFHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(StockTransfer.DocGenerationNewValue);
        }

        //Limit the values ,in grid to single line using dots
        POColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.SFHPONO, tableID);
        POColValue = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHPONO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHPONO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHPONO, tableID);
        if (POColIndex != null) {
            len = POColValue.toString().length;
            if (len > 35) {
                $(this).find("td:eq(" + POColIndex + ")").attr("title", POColValue);
                $(this).find("td:eq(" + POColIndex + ")").html($(this).find("td:eq(" + POColIndex + ")").html().toString().substr(0, 35) + '...');
            }
        }
        VendorTextColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.SFHVENDORTEXT, tableID);
        VendorTextColValue = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHVENDORTEXT, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHVENDORTEXT, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.SFHVENDORTEXT, tableID);
        if (VendorTextColIndex != null) {
            len = VendorTextColValue.toString().length;
            if (len > 45) {
                $(this).find("td:eq(" + VendorTextColIndex + ")").attr("title", VendorTextColValue);
                $(this).find("td:eq(" + VendorTextColIndex + ")").html($(this).find("td:eq(" + VendorTextColIndex + ")").html().toString().substr(0, 45) + '...');
            }
        }
        if (isModifySA == "1" && UserStatus == 0 && SFH_STATUS != 4 && SFH_STATUS != 5 && SFH_STATUS != 3) {   //SFH_STATUS=> 4 (Cancelled),5(Closed),3(Rejected)
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }
        if (isCancelSA == "1" && SFH_STATUS != 0 && SFH_STATUS != 3 && SFH_STATUS != 4 && SFH_STATUS != 5) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }
    });
}

function CancelSADetails() {
    var msgtxt;
    $.get(StockTransfer.STOCKTRANSFERDELETEURL + stPK, function (data) {
        if (data != null && data.length > 0) {
            if (parseInt(data[0]) == 1) {
                msgtxt = StockTransfer.CANCELSUCESS;
            }
            else if (parseInt(data[0]) == 0) {
                msgtxt = "Assigned";
            }
            else if (parseInt(data[0]) == -10) {
                // msgtxt = "Unable to cancel.It is referenced in '" + data[1] + "'";
                msgtxt = String.format("Translate(ErrCancelStockAdmission)", "'" + data[1] + "'")
            }
            else {
                msgtxt = StockTransfer.ACTIONFAILEDMSG;
            }
        }
        GrandScriptUtils.ShowModal(msgtxt, StockTransfer.INFORMATIONTITLE, StockTransfer.SAVECMD);
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
    $.get(StockTransfer.STOCKTRANSFERAUTOCOMPLETEURL + $("select[id$=SBU]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&SearchType=CMP_DISPLAY_CODE" + "&PageUrl=" + StockTransfer.PAGEURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, false, false, false, false, true);
    });
}

