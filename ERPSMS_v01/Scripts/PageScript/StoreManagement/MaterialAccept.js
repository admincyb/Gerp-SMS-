
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../JSLINQ/JSLINQ.js" />


var MaterialAccept = {
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    SaveMaterialAccept: "MaterialAccept.do?Action=SaveMaterialAccept",
    StoreIssueBindGridURL: "MaterialAccept.do?Action=GetStoreIssuePending&MAHPK=",
    PreviousMABindGridURL: "MaterialAccept.do?Action=GetPreviousMaterialAccept&poID=",
    StoreIssueAutoURL: "MaterialAccept.do?Action=GetPendingSearchAuto",
    //FillNonStoreDropdownURL: "SubDepartmentManagement.do?Action=GetNonStoreDept&SBUPk=",
    FillNonStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    //FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillVendorDropdownURL: "VendorRegistration.do?Action=GetVendors&SBUPk=",
    FillPreviousSRSDetailsView: "StoreRequisitionSlip.do?Action=GetSRSDetails&SRSPK=",
    FillPreviousMADetailsView: "MaterialAccept.do?Action=GetPreviousMADetailsView&MIHPK=",
    GetMaterialUOMConversion: "CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    MaterialAcceptListUrl: "MaterialAcceptList.aspx",
    //FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompany&SBUPk=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    ListUrl: "MaterialAcceptList.aspx",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    REPORTURL: "../Reports/GenerateReport.aspx",

    BizUnitPk: 0,
    MIPK: 0,
    ItemPK: 0,
    MRDPK: 0,
    MIHPK: 0,
    LOGOUT: "LOGOUT",
    MaterialList: new Array(),
    MaterialObj: new Object(),
    MaterialAcceptObj: new Object(),
    Confirmation: "Translate(Confirmation)",
    MASavedMessage: "Translate(MaterialAcceptSavedMessage)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    RecordExist: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MAStockAcceptValid: "Translate(StockAcceptedDateLessthanStockTransferDate)",
    MessageBoxTitle: "Translate(Information)",
    AddMaterialAcceptDetails: "Translate(AddMaterialAcceptDetails)",
    SaveMessage1: "Translate(MaterialAcceptSaved1)",
    SaveMessage2: "Translate(MaterialAcceptSaved2)",
    MAQtyValid: "Translate(MaterialAcceptQtyValid)",
    StockExceed: "Translate(MaterialAcceptStockValid)",
    SelectMIFromList: "Translate(SelectMIFromList)",
    CannotReceivePriorDateSendReceive: "Translate(CannotReceivePriorDateSendReceive)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    SaveRefMsg: "Translate(MsgSaveRefError)",


    MRH_PK: "MRH_PK",
    MIH_PK: "MIH_PK",
    MAD_MI: "MAD_MI",
    MAD_ITEM: "MAD_ITEM",
    MID_ITEM: "MID_ITEM",
    MID_UOM: "MID_UOM",
    MRD_REQUEST_NO: "MRD_REQUEST_NO",
    MIH_NO: "MIH_NO",
    ITM_NAME: "ITM_NAME",
    ITM_CODE: "ITM_CODE",
    MRD_QTY_APPROVED: "MRD_QTY_APPROVED",
    MID_QTY_ISSUED: "MID_QTY_ISSUED",
    MID_QTY_RECEIVED: "MID_QTY_RECEIVED",
    BALANCE_QTY: "BALANCE_QTY",
    MAD_REMARKS: "MAD_REMARKS",
    MAD_QTY_ACCEPTED: "MAD_QTY_ACCEPTED",
    MAD_QTY_LOST: "MAD_QTY_LOST",
    UOM_CODE: "UOM_CODE",
    DELETE: "delete",
    MIVIEW: "miview",
    EDIT: "edit",
    SAVE: "save",
    SRSVIEW: "srsview",
    MAVIEW: "maview",
    PREVIOUSGRNVIEW: "previousgrnview",
    IsViewMode: false,
    INBOX: "INBOX",
    MID_PK: "MID_PK",
    MAD_SL_NO: "MAD_SL_NO",
    IsModify: false
}
var QtyDec, AmtDec;
$(document).ready(function () {
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    GrandScriptUtils.DatePicker("MAH_DATE", false, false);
    PageInit();
});

function PrintPage() {
    //     var prID = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
    var url = MaterialAccept.REPORTURL + "?ID=" + $("input[id$=MAH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    OpenPDF(url);
    return false;
}
function CancelPage() {
    //<summary>Function used to redirect to listing page  </summary>
    window.location = MaterialAccept.ListUrl;
    return false;
}
function PageInit() {
    ///<summary>initial page condition</summary>
    $("[id$=ConfirmStockValueChange]").val('0');
    var queryString = window.location.search.substring(1);
    if (queryString != "") {
        var queryStr = queryString.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                $("[id$=MAH_DEPT]").attr("disabled", "disabled");
                $("[id$=MAH_COMPANY]").attr("disabled", "disabled");
                MaterialAccept.IsViewMode = true;
            }
            else if (pK[1] == 1 && pK[0] == "IsModify") {
                MaterialAccept.IsModify = true;
                $("[id$=MAH_IS_EDIT]").val("1");
            }
        }
    }
    $("[id$=AddToList]").hide();
    Popup();
    MaterialAccept.BizUnitPk = $("[id$=BizUnitPk]").val();
    MaterialAccept.MaterialObj = $.parseJSON($("[id$=MaterialList]").val());
    MaterialAccept.MaterialList = MaterialAccept.MaterialObj.MaterialList;
    if (queryString != "") {
        var queryStr = queryString.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                FillDetails(MaterialAccept.MaterialObj);
            }
            else if (pK[1] == 1 && pK[0] == "IsModify") {
                MaterialAccept.IsModify = true;
                $("[id$=MAH_IS_EDIT]").val("1");
            }
        }
    }
    FillCompany(MaterialAccept.MaterialObj.MAH_COMPANY);
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=MAH_COMPANY]").attr("disabled", "disabled");
    }
    SetSearchType();
    if (MaterialAccept.MaterialObj.MAH_DEPT == 0) {
        FillStore($("[id$=hdfAcceptStore]").val());
        //        FillCompany($("[id$=hdfCompany]").val());
        //        if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        //            $("[id$=MAH_COMPANY]").attr("disabled", "disabled");
        //        }
    }
    else {
        FillStore(MaterialAccept.MaterialObj.MAH_DEPT);
        //        FillCompany(MaterialAccept.MaterialObj.MAH_COMPANY);
        //        if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        //            $("[id$=MAH_COMPANY]").attr("disabled", "disabled");
        //        }
    }

    var a = MaterialAccept.MaterialObj.MAH_PK;

    if (parseInt(MaterialAccept.MaterialObj.MAH_PK) > 0) {
        $("[id$=MAH_PK]").val(MaterialAccept.MaterialObj.MAH_PK);
    }

    FillAutoComplete();
    if (!$.isArray(MaterialAccept.MaterialList)) {
        MaterialAccept.MaterialAcceptObj = MaterialAccept.MaterialList;
        MaterialAccept.MaterialList = new Array();
        MaterialAccept.MaterialList.push(MaterialAccept.MaterialAcceptObj);
    }
    $("#divData").data("SIData", MaterialAccept.MaterialList);
    GrandGrid.MakeGrid($("#grdPendingSIList"), 0, MaterialAccept.MaterialList);
}

function Popup() {
    ///<summary>Function used for popup</summary>

    $("#divSRSDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

    $("#divPreviousMA").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}

function FillStore(selectVal) {
    ///<summary>to fill store combo</summary>

    var drpID = $("select[id$=MAH_DEPT]").attr("id");
    $.get(MaterialAccept.FillNonStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=-1&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        BindGrid();
    });
}

function FillUOM(catgID, selectVal) {
    ///<summary>function To Fill UOM Details </summary>

    var drpID = $("select[id$=UOM]").attr("id");
    $("[id$=UOMOrg]").val(selectVal);
    $.get(MaterialAccept.UomURL + catgID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
    });
}

function FillDetails(MaterialObj) {
    $("[id$=MAH_PK]").val(MaterialObj.MAH_PK);
    $("[id$=MAH_NO]").html(MaterialObj.MAH_NO);
    $("[id$=MAH_DATE]").val(MaterialObj.MAH_DATE);
}

function FillAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>

    GrandScriptUtils.MakeAutoComplete("SearchValue", MaterialAccept.StoreIssueAutoURL, false, true, false, "SearchType", false, "MAH_DEPT", "MAH_PK", "ddlSelectDiv3");
}

function BindGrid() {
    ///<summary>To handle bind grid </summary>

    $("[id$=AddToList]").css("display", "none");
    var ajaxUrl = "";
    if (parseInt($("[id$=MAH_PK]").val()) > 0) {
        ajaxUrl = MaterialAccept.StoreIssueBindGridURL + $("[id$=MAH_PK]").val() + "&Store=" + $("select[id$=MAH_DEPT]").val() + "&BizUnit=" + MaterialAccept.BizUnitPk + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    }
    else if (parseInt($("[id$=hdfMIPk]").val()) > 0) {
        ajaxUrl = MaterialAccept.StoreIssueBindGridURL + "0" + "&Store=" + $("select[id$=MAH_DEPT]").val() + "&BizUnit=" + MaterialAccept.BizUnitPk + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&MIPk=" + $("[id$=hdfMIPk]").val();
    }
    else {
        ajaxUrl = MaterialAccept.StoreIssueBindGridURL + "0" + "&Store=" + $("select[id$=MAH_DEPT]").val() + "&BizUnit=" + MaterialAccept.BizUnitPk + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    }
    $("#grdSIList").removeAttr("ajaxurl")
    $("#grdSIList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdSIList");
    GrandGrid.MakeGrid($("#grdSIList"));
    //Accept Date should default to the Issue date (not current date),while page load from inbox
    if ($("[id$=IsPrefID]").val() == "1") {
        if (CheckValidDate(MaterialAccept.MaterialObj.MAH_DATE))
            $("[id$=MAH_DATE]").val(MaterialAccept.MaterialObj.MAH_DATE);
        $.get(ajaxUrl, function (data) {
            if (data != "" && data != null) {
                $.each(data.Table1, function (key, val) {
                    $("[id$=MAH_DATE]").val(val.MIH_DATE);
                });

            }
        });
    }
    return false;
}
function CheckValidDate(value) {
    try {
        jQuery.datepicker.parseDate("dd-M-yy", value); return true;
    }
    catch (e) {
        return false;
    }
}
function ViewSRSDetails() {
    ///<summary>Function used for view the PO Details</summary>

    BindPreviousSRSGrid();
    $("#divSRSDetails").dialog({ width: 850, height: 450, resizable: false, modal: true });
    $("#divSRSDetails").dialog("open");
}

function ViewMADetails() {
    ///<summary>Function used for view the PO Details</summary>

    BindPreviousMAGrid();
    $("#divPreviousMA").dialog({ width: 750, height: 450, resizable: false, modal: true });
    $("#divPreviousMA").dialog("open");
}

function BindPreviousSRSGrid() {
    ///<summary>To handle bind previuos GRN grid </summary>

    var ajaxUrl = MaterialAccept.FillPreviousSRSDetailsView + MaterialAccept.MRDPK;
    $("#grdSRSDetails").removeAttr("ajaxurl")
    $("#grdSRSDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdSRSDetails");
    GrandGrid.MakeGrid($("#grdSRSDetails"));
    MaterialAccept.MRDPK = 0;
}

function BindPreviousMAGrid() {
    ///<summary>To handle bind previuos GRN grid </summary>

    var ajaxUrl = MaterialAccept.FillPreviousMADetailsView + MaterialAccept.MIHPK;
    $("#grdPreviousMA").removeAttr("ajaxurl")
    $("#grdPreviousMA").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPreviousMA");
    GrandGrid.MakeGrid($("#grdPreviousMA"));
    MaterialAccept.MIHPK = 0;
}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "SearchValue") {
        BindGrid();
    }
}

function AddNew() {
    ///<summary>Function used to add new details</summary>

    MaterialAccept.MaterialList = new Array();
    $("#divData").data("SIData", MaterialAccept.MaterialList);
    GrandGrid.MakeGrid($("#grdPendingSIList"), 0, MaterialAccept.MaterialList);
    BindGrid();
}

function AddToList() {
    ///<summary>Function used to add the needed po material list</summary>

    var grdID;
    MaterialAccept.MaterialList = $("#divData").data("SIData");
    if ($("#grdSIList tr input[type=checkbox]:checked").length == 0) {
        GrandScriptUtils.ShowModal(MaterialAccept.SelectMIFromList, MaterialAccept.MessageBoxTitle);
        return false;
    }
    $("#grdSIList tr:has(td)").each(function () {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            MaterialAccept.MaterialObj = new Object();
            var maxSlNo = JSLINQ(MaterialAccept.MaterialList)
                .Max(function (MAitem) { return MAitem.MAD_SL_NO; });
            MaterialAccept.MaterialObj.MAD_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
            MaterialAccept.MaterialObj.MAD_MI = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MIH_PK, grdID);
            MaterialAccept.MaterialObj.MAD_MI_DTL = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_PK, grdID);
            MaterialAccept.MaterialObj.MAD_ITEM = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_ITEM, grdID);
            MaterialAccept.MaterialObj.MRH_PK = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MRH_PK, grdID);
            if (GrandGrid.Utilities.GetColumnValue($(this), "MAD_STK_BATCH", grdID) != "null")
                MaterialAccept.MaterialObj.MAD_STK_BATCH = GrandGrid.Utilities.GetColumnValue($(this), "MAD_STK_BATCH", grdID);
            if (CheckItemExists(MaterialAccept.MaterialObj.MAD_MI, MaterialAccept.MaterialObj.MAD_ITEM, MaterialAccept.MaterialObj.MRH_PK, MaterialAccept.MaterialObj.MAD_STK_BATCH)) {
                MaterialAccept.MaterialObj.MAD_PK = 0;
                MaterialAccept.MaterialObj.MAD_STK_BATCH_NO = GrandGrid.Utilities.GetColumnValue($(this), "MAD_STK_BATCH_NO", grdID);
                MaterialAccept.MaterialObj.MIH_NO = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MIH_NO, grdID);
                MaterialAccept.MaterialObj.ITM_NAME = "(" + GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.ITM_CODE, grdID) + ") " + GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.ITM_NAME, grdID);
                MaterialAccept.MaterialObj.MAD_UOM = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_UOM, grdID);
                MaterialAccept.MaterialObj.MID_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, "");
                MaterialAccept.MaterialObj.MID_QTY_RECEIVED = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_RECEIVED, grdID).replace(/[^0-9\.]+/g, "");
                MaterialAccept.MaterialObj.MAD_QTY_ACCEPTED = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.BALANCE_QTY, grdID).replace(/[^0-9\.]+/g, "");
                MaterialAccept.MaterialObj.UOM_CODE = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.UOM_CODE, grdID);
                MaterialAccept.MaterialObj.MAD_REMARKS = "";
                MaterialAccept.MaterialList.push(MaterialAccept.MaterialObj);
            }
        }
    });
    $("#divData").data("SIData", MaterialAccept.MaterialList);
    GrandGrid.MakeGrid($("#grdPendingSIList"), 0, MaterialAccept.MaterialList);
    return false;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for Catch all the grid events in this function </summary>

    var grdID;
    switch (command.toString().toLowerCase()) {
        case MaterialAccept.DELETE:
            grdID = $(tr).parents("table:first").attr("id");
            MaterialAccept.MIPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAccept.MAD_MI, grdID);
            MaterialAccept.ItemPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAccept.MAD_ITEM, grdID);
            MaterialAccept.MRDPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAccept.MRH_PK, grdID);
            GrandScriptUtils.ShowModal(MaterialAccept.DeleteConfirmMsg, MaterialAccept.Confirmation, MaterialAccept.DELETE, true);
            break;
        case MaterialAccept.SRSVIEW:
            grdID = $(tr).parents("table:first").attr("id");
            MaterialAccept.MRDPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAccept.MRH_PK, grdID);
            ViewSRSDetails();
            break;
        case MaterialAccept.MAVIEW:
            grdID = $(tr).parents("table:first").attr("id");
            MaterialAccept.MIHPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAccept.MIH_PK, grdID);
            ViewMADetails();
            break;
    }
    return false;
}

function CalcLoseinTransit(indx, qtyAccept) {
    //## Sumesh
    ///<summary>Function check any variation occur between actual stock and audit stock</summary>
    var grdID;
    var actualStockIndx = 0;
    var actualStock = 0;
    var ledgerStock = 0;
    var adnlQty = 0;
    var actionIndex = 0;
    var damageIndx = 0;
    var actValIndex = 0;
    var actVal = 0;
    var ledgerVal;
    var ledgerStock;
    var colIndex = 0;
    var loseInTransit = 0;
    var losInTransitIndex = 0;

    var issdQty = 0;
    //    ClearDamageDetailsList(indx);
    $("#grdPendingSIList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            issdQty = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, "");
            losInTransitIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MAD_QTY_LOST", grdID);
            if (parseFloat(qtyAccept) <= parseFloat(issdQty)) {
                var result = (parseFloat(issdQty) - parseFloat(qtyAccept));
                $(this).find("td:eq(" + losInTransitIndex + ") input").val(parseFloat(result).toFixed(QtyDec));
            }
            else {
                $(this).find("td:eq(" + damageIndx + ") a").html("");
            }
        }
    });
    return false;
}

function IsValidQty(txtQty, index) {
    ///<summary>For delete the item,po in the grid - Details</summary>


    $(txtQty).next(".error[htmlFor=" + $(txtQty).attr("id") + "]").remove();
    var floatReg = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
    var grdID = "";
    var issdQty = 0;
    var accptdQty = 0;
    var accpQty = 0;
    var tr = $(txtQty).parents("tr:first");
    grdID = $(tr).parents("table:first").attr("id");
    issdQty = GrandGrid.Utilities.GetColumnValue($(tr), MaterialAccept.MID_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, "");
    accptdQty = GrandGrid.Utilities.GetColumnValue($(tr), MaterialAccept.MID_QTY_RECEIVED, grdID).replace(/[^0-9\.]+/g, "");
    accpQty = parseFloat(issdQty) - parseFloat(accptdQty);
    if (!floatReg.test($(txtQty).val())) {
        $(txtQty).val(accpQty);
        var divError = document.createElement("div");
        divError.className = "error";
        $(divError).attr("htmlFor", $(txtQty).attr("id"));
        $(divError).html("Translate(MinMaxDecimalFloatValue)");
        $(divError).insertAfter($(txtQty));
    }
    if (accpQty < parseFloat($(txtQty).val())) {
        $(txtQty).val(accpQty);
    }
    CalcLoseinTransit(index, $(txtQty).val());
}

function DeletePOMaterial() {
    ///<summary>For delete the item,po in the grid - Details</summary>

    MaterialAccept.MaterialList = $("#divData").data("SIData");
    for (var i in MaterialAccept.MaterialList) {
        if ((MaterialAccept.MaterialList[i].MAD_ITEM == MaterialAccept.ItemPK) && (MaterialAccept.MaterialList[i].MAD_MI == MaterialAccept.MIPK) && (MaterialAccept.MaterialList[i].MRH_PK == MaterialAccept.MRDPK)) {
            MaterialAccept.MaterialList.splice(i, 1);
            break;
        }
    }
    MaterialAccept.ItemPK = 0;
    MaterialAccept.MIPK = 0;
    MaterialAccept.MRDPK = 0;
    $("#divData").data("SIData", MaterialAccept.MaterialList);
    GrandGrid.MakeGrid($("#grdPendingSIList"), 0, MaterialAccept.MaterialList);
}

function CheckItemExists(siPK, itemPK, srsPK, batchId) {
    //<summary> Function Used to check whether this po item already added. </summary>

    MaterialAccept.MaterialList = $("#divData").data("SIData");
    var flag = true;
    for (var i in MaterialAccept.MaterialList) {
        if ((MaterialAccept.MaterialList[i].MAD_ITEM == itemPK) && (MaterialAccept.MaterialList[i].MAD_MI == siPK) && (MaterialAccept.MaterialList[i].MRH_PK == srsPK) && (MaterialAccept.MaterialList[i].MAD_STK_BATCH == batchId)) {
            flag = false;
            break;
        }
    }
    return flag;
}

function SavePage(command) {
    ///<summary>Function used to saving   </summary>
    $.get(MaterialAccept.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(MaterialAccept.SessionExpired, MaterialAccept.Confirmation, MaterialAccept.LOGOUT, true);
            result = false;
        }
        else {
            //<summary> Function Used to save page </summary>
            $("[id$=MAH_COMPANY]").attr("disabled", false);   //Added on Aug-16-2017    --Sruthy H
            MaterialAccept.MaterialList = new Array();
            MaterialAccept.MaterialList = AddToPendingSIList();
            if (MaterialAccept.MaterialList.length == 0) {
                ShowErrorMessage("<ul><li>" + MaterialAccept.AddMaterialAcceptDetails + "</li></ul>", MaterialAccept.MessageBoxTitle);
                return false;
            }
            //Showing validation for Future Date selection
            if ($("[id$=hdfIsContFutureDate]").val() != "1") {
                var RetVal = CompareDate($("[id$=MAH_DATE]").val(), $("[id$=hdfCurrentDate]").val());
                if (RetVal == 1) {
                    ShowFutureDate(command);
                    return false;
                }
            }

            $("[id$=MaterialList]").val(JSON.stringify(MaterialAccept.MaterialList));
            if (command != "Draft")
                $("[id$=ActionID]").val("1"); // save and doworkflow.
            else
                $("[id$=ActionID]").val("0"); // save only.
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            if ($(document.forms[0]).valid()) {

                //To Prevent Duplicate Submission
                if ($("[id$=SubmitFlag]").val() == "0")
                    $("[id$=SubmitFlag]").val('1')
                else
                    return false;

                $("#updateProgress").show();

                $.post(MaterialAccept.SaveMaterialAccept, jSonString, function (data) {
                    if (parseInt(data[0]) > 0) {
                        //  GrandScriptUtils.ShowModal(SaveMessageWithPRNo, MaterialAccept.MessageBoxTitle, MaterialAccept.SAVE);
                        if (command == "Draft") {
                            var SaveMessageWithNo = MaterialAccept.MASavedMessage;
                            if ($("[id$=AST_DOC_MODE]").val() == "1")
                                SaveMessageWithNo = MaterialAccept.SaveMessage1 + " " + data[1] + " " + MaterialAccept.SaveMessage2;
                            GrandScriptUtils.ShowModal(SaveMessageWithNo, MaterialAccept.MessageBoxTitle, MaterialAccept.SAVE);
                        }
                        else {
                            $("[id$=hdfAppID]").val(data[0]);
                            $("[id$=AppNo]").val(data[1]);
                            SaveWorkFlow();
                        }
                    }
                    else if (parseInt(data[0]) == -1) {
                        GrandScriptUtils.ShowModal(MaterialAccept.MAStockAcceptValid, MaterialAccept.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -2) {
                        GrandScriptUtils.ShowModal(MaterialAccept.SaveMessage1 + " " + $("[id$=MAH_NO]").html() + " " + MaterialAccept.EditUsedByAnotherUser, MaterialAccept.MessageBoxTitle, MaterialAccept.SAVE);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -3) {
                        GrandScriptUtils.ShowModal(MaterialAccept.MAQtyValid, MaterialAccept.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -4) {
                        GrandScriptUtils.ShowModal(MaterialAccept.StockExceed, MaterialAccept.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -10) {
                        //                GrandScriptUtils.ShowModal(MaterialAccept.CannotReceivePriorDateSendReceive, MaterialAccept.MessageBoxTitle);
                        fnConfirmStockValueChange(command);
                        $("[id$=SubmitFlag]").val('0');
                    }
                    else if (parseInt(data[0]) == -31) {
                        var errMsg = MaterialAccept.SaveRefMsg + "<br/>" + data[1];
                        GrandScriptUtils.ShowModal(errMsg.fontcolor("red"), MaterialAccept.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0');

                    }
                    else {
                        GrandScriptUtils.ShowModal(MaterialAccept.ActionFailedMessage, MaterialAccept.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                });
            }
        }
    });
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var SaveMessageWithNo = "";
    SaveMessageWithNo = MaterialAccept.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + MaterialAccept.SaveMessage2;
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        GrandScriptUtils.ShowModal(SaveMessageWithNo, MaterialAccept.MessageBoxTitle, MaterialAccept.INBOX);
    }
    else {
        GrandScriptUtils.ShowModal(SaveMessageWithNo, MaterialAccept.MessageBoxTitle, MaterialAccept.SAVE);
    }
}
function AddToPendingSIList() {
    ///<summary>Function used to add the needed po material list</summary>

    MaterialAccept.MaterialList = $("#divData").data("SIData");
    for (var i in MaterialAccept.MaterialList) {
        MaterialAccept.MaterialList[i].MAD_QTY_ACCEPTED = $("#txtAcceptQty_" + i).val();
        MaterialAccept.MaterialList[i].MAD_QTY_LOST = $("#txtLostQty_" + i).val();
        MaterialAccept.MaterialList[i].MAD_REMARKS = $("#txtRemarks_" + i).val();
    }
    return MaterialAccept.MaterialList;
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdPendingSIList") {

        var remarks = "";
        var remkColIndex = 0;
        var qtyAccept = 0;
        var acceptColIndex = 0;
        var qtylost = 0;
        var lostColIndex = 0;
        var itemPK = 0;
        var mahPK = 0;
        var srsColIndex = 0;
        var srsNo = "";
        var colIndex = 0;
        var qty = 0;
        if (MaterialAccept.IsViewMode) {
            $("#grdPendingSIList").find("th:last").hide();
        }
        $("#grdPendingSIList tr:has(td)").each(function (index) {
            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MAD_REMARKS, grdID);
            remarks = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MAD_REMARKS, grdID);
            acceptColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MAD_QTY_ACCEPTED, grdID);
            qtyAccept = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MAD_QTY_ACCEPTED, grdID);
            lostColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MAD_QTY_LOST, grdID);
            qtylost = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MAD_QTY_LOST, grdID);

            qtyIssColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MID_QTY_ISSUED, grdID);
            qtyIss = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_ISSUED, grdID);

            if (remkColIndex != 0) {
                if (remarks == "null") {
                    remarks = "";
                }
                if (!MaterialAccept.IsViewMode) {
                    $(this).find("td:eq(" + remkColIndex + ")").html("");
                    $(this).find("td:eq(" + remkColIndex + ")").append("<input id=\"txtRemarks_" + index + "\" type=\"text\" value=\"" + remarks + "\" width=\"80%\" tabindex=\"8\" />");
                }
                else {
                    $(this).find("td:eq(" + remkColIndex + ")").html(remarks);
                }
            }
            if (acceptColIndex != 0) {
                if (!MaterialAccept.IsViewMode) {
                    $(this).find("td:eq(" + acceptColIndex + ")").html("");
                    $(this).find("td:eq(" + acceptColIndex + ")").append("<input id=\"txtAcceptQty_" + index + "\" type=\"text\" value=\"" + parseFloat(qtyAccept).toFixed(QtyDec) + "\" class=\"numeric input-w50\"  maxlength=\"11\" tabIndex=\"8\" onchange=\"javascript:return IsValidQty($(this)," + index + ");\" />");
                }
                else {
                    $(this).find("td:eq(" + acceptColIndex + ")").html(numberWithCommas(parseFloat(qtyAccept).toFixed(QtyDec)));
                }
            }
            if (lostColIndex != 0) {
                if (!MaterialAccept.IsViewMode) {
                    $(this).find("td:eq(" + lostColIndex + ")").html("");
                    qtylost = qtylost == "undefined" ? 0 : qtylost;
                    $(this).find("td:eq(" + lostColIndex + ")").append("<input id=\"txtLostQty_" + index + "\" type=\"text\" value=\"" + parseFloat(qtylost).toFixed(QtyDec) + "\" readonly  tabIndex=\"8\"  class=\"numeric input-w70 input-disabled\"/>");
                }
                else {
                    $(this).find("td:eq(" + lostColIndex + ")").html(numberWithCommas(parseFloat(qtylost).toFixed(QtyDec)));
                }
            }
            if (qtyIssColIndex != null) {
                $(this).find("td:eq(" + qtyIssColIndex + ")").html(numberWithCommas(parseFloat(qtyIss).toFixed(QtyDec)));
            }
            if (MaterialAccept.IsViewMode) {
                $(this).find("td:last").hide();
            }
            var batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MAD_STK_BATCH_NO", grdID);
            if (batchColIndex != null) {
                var batch = GrandGrid.Utilities.GetColumnValue($(this), "MAD_STK_BATCH_NO", grdID);
                if (batch == "null" || batch == "undefined")
                    $(this).find("td:eq(" + batchColIndex + ")").html("-");

            }
        });
    }
    if (grdID == "grdSIList") {

        $("[id$=AddToList]").show();
        if (MaterialAccept.IsViewMode) {
            $("[id$=AddToList]").hide();
        }
        $("#grdSIList tr:has(td)").each(function (index) {
            srsColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MRD_REQUEST_NO, grdID);
            srsNo = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MRD_REQUEST_NO, grdID);
            acceptColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MID_QTY_RECEIVED, grdID);
            qtyAccept = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_RECEIVED, grdID);
            if (srsColIndex != null) {
                $(this).find("td:eq(" + srsColIndex + ")").html("");
                $(this).find("td:eq(" + srsColIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'SRSVIEW');\">" + srsNo + "</a>");
            }
            if (acceptColIndex != null) {
                if (parseFloat(qtyAccept) > 0) {
                    $(this).find("td:eq(" + acceptColIndex + ")").html("");
                    $(this).find("td:eq(" + acceptColIndex + ")").html("<a style=\"cursor:pointer;float:right\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'MAVIEW');\">" + qtyAccept + "</a>");
                }
            }

            var batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MAD_STK_BATCH_NO", grdID);
            if (batchColIndex != null) {
                var batch = GrandGrid.Utilities.GetColumnValue($(this), "MAD_STK_BATCH_NO", grdID);
                if (batch == "null")
                    $(this).find("td:eq(" + batchColIndex + ")").html("-");

            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MRD_QTY_APPROVED, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MRD_QTY_APPROVED, $(this).parents("table:first").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MID_QTY_ISSUED, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_ISSUED, $(this).parents("table:first").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.MID_QTY_RECEIVED, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.MID_QTY_RECEIVED, $(this).parents("table:first").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialAccept.BALANCE_QTY, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), MaterialAccept.BALANCE_QTY, $(this).parents("table:first").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
        });
    }
    if (grdID == "grdSRSDetails") {
        $("#grdSRSDetails tr:has(td)").each(function (index) {
            qty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_APPROVED", grdID);
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_APPROVED", grdID);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec));
            }
        });
    }
    if (grdID == "grdPreviousMA") {
        $("#grdPreviousMA tr:has(td)").each(function (index) {
            qty = GrandGrid.Utilities.GetColumnValue($(this), "MAD_QTY_ACCEPTED", grdID);
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MAD_QTY_ACCEPTED", grdID);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec));
            }
        });
    }
}

function SetSearchType(isLoad) {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    ClearSearchDetails();
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        if (isLoad) {
            BindGrid();
        }
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

}

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val("");
    $("[id$=FromDate]").val("");
    $("input[id$=hdfFrmDate]").val("");
    $("[id$=ToDate]").val("");
    $("input[id$=hdfToDate]").val("");
}
function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    window.location = MaterialAccept.MaterialAcceptListUrl;
    return false;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case MaterialAccept.SAVE:
            window.location = MaterialAccept.MaterialAcceptListUrl;
            break;
        case MaterialAccept.DELETE:
            DeletePOMaterial();
            break;
        case MaterialAccept.INBOX:
            window.location = MaterialAccept.InboxURL;
            break;
        case MaterialAccept.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
}

function FillCompany(selectVal) {
    var drpID = $("select[id$=MAH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = MaterialAccept.FillCompanyDropdownURL + MaterialAccept.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val();
    }
    else {
        getURL = MaterialAccept.FillCompanyDropdownURL + MaterialAccept.BizUnitPk + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);

    });
    //    if (selectVal == undefined || selectVal == 0) {
    //        var drpID = $("select[id$=MAH_COMPANY]").attr("id");
    //        $.get(MaterialAccept.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
    //            var selCompany = $("[id$=hdfSelCompany]").val();
    //            GrandScriptUtils.FillDropDown(drpID, data, true, false, selCompany);
    //        });
    //    }
    //    else {
    //        var drpID = $("select[id$=MAH_COMPANY]").attr("id");
    //        $.get(MaterialAccept.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
    //            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
    //        });
    //    }
}

////Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    //Seperates the components of the number
//    var n = x.toString().split(".");
//    //Comma-fies the first part
//    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//    //Combines the two sections
//    return n.join(".");
//   // return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

//For checking selected date is a future date or not
//command=>Draft,SaveandSubmit
function ShowFutureDate(command) {

    var msgTitle;
    var msg;
    msgTitle = MaterialAccept.MessageBoxTitle;
    msg = MaterialAccept.ContFutureDateMsg;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfIsContFutureDate]").val(1);
                $(this).dialog("close");
                if (command == "Draft") {
                    $("[id$=btnSave]").click();
                }
                else {
                    $("[id$=btnSaveandSubmit]").click();
                }
            },
            Cancel: function (e) {
                $("[id$=hdfIsContFutureDate]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}