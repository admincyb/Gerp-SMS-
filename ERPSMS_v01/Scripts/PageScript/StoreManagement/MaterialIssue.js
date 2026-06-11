
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../JSLINQ/JSLINQ.js" />

///#region ------Configuration Section-----
var MaterialIssue = {
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    // SaveMaterialIssue: "MaterialIssue.do?Action=SaveMaterialIssue",
    SaveMaterialIssue: "MaterialIssue.do?Action=SaveMaterialIssueWkf",
    SRSPendingBindGridURL: "MaterialIssue.do?Action=GetSRSPending&miPK=",
    WOPendingBindGridURL: "MaterialIssue.do?Action=GetWOPending&miPK=",
    PreviousSRSBindGridURL: "MaterialIssue.do?Action=GetPreviousGRN&poID=",
    SRSAutoURL: "MaterialIssue.do?Action=GetPendingSearchAuto&AUTOSEARCH=1",
    WIHAutoURL: "MaterialIssue.do?Action=GetPendingWIHSearchAuto&AUTOSEARCH=1",

    FillOtherSBUDropDownURL: "StoreRequisitionSlip.do?Action=GetOtherSBUS&Active=1",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillDepartementDropdownURL: "SubDepartmentManagement.do?Action=GetDeparmentsBySBU&SBUPk=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    //    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompany&SBUPk=",
    FillBatchNoDropDownURL: "MaterialManagement.do?Action=GetBatchNo&SBUPk=",
    FillBatchDetailGetURL: "MaterialManagement.do?Action=GetBatchDetails&SBUPk=",
    FillToDepartment: "StoreRequisitionSlip.do?Action=GetDepartmentDtls&SBUPk=",
    GetSRSDetails: "StoreRequisitionSlip.do?Action=GetSRSDetails&SRSPK=",
    // FillStoreDropdownURL: "StoreRequisitionSlip.do?Action=GetStores&SBUPk=",
    FillPreviousSRSDetailsView: "MaterialIssue.do?Action=GetPreviousSRSDetailsView&SRSPK=",
    UomURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial&MaterialPK=",
    GetMaterialUOMConversion: "MaterialManagement.do?Action=GetMaterialUOMConversion&MaterialId=",
    MIListUrl: "MaterialIssueList.aspx",
    GetCurrentStock: "MaterialManagement.do?Action=GetCurrentStockForStore&SBUPk=",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    ListUrl: "MaterialIssueList.aspx",
    REPORTURL: "../Reports/GenerateReport.aspx",
    GetCurrentStock: "MaterialManagement.do?Action=GetCurrentStockForStore&SBUPk=",
    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",
    TEXTEMPTY: "",
    ValueEmpty: ' ',
    BizUnitPk: 0,
    SRSPK: 0,
    ItemPK: 0,
    MIList: new Array(),
    GridList: new Array(),
    MRListDistinct: new Array(),
    SaveList: new Array(),
    MIObj: new Object(),
    MaterialIssueRowObj: new Object(),
    Confirmation: "Translate(Confirmation)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    RecordExist: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MaterialBatchValidation: "Translate(SelectBatch)",
    MessageBoxTitle: "Translate(Information)",
    AddMaterialIssueDetails: "Translate(AddSTDetails)",
    SaveMessage1: "Translate(STSaved)",
    SaveMessage2: "Translate(MaterailIssueSaved2)",
    SaveMessage3: "Translate(MIWO)",
    MISavedMessage: "Translate(STSavedMessage)",
    MIWOSavedMessage: "Translate(STWOSavedMessage1)",
    StoreDoesnthaveReqStock: "Translate(StoreDoesnthaveReqStock)",
    SpecifiedQuantityNotAvailableInStock: "Translate(SpecifiedQuantityNotAvailableInStock)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    MIqtyGreaterThanMRqty: "Translate(MIqtyGreaterThanMRqty)",
    MIqtyGreaterThanMRqtyWO: "Translate(MIqtyGreaterThanMRqtyWO)",
    EnterDate: "Translate(EnterDate)",
    MIQty: "Translate(STQty)",
    MIQtyBtch: "Translate(STQtyBtch)",
    CannotReceivePriorDateSendReceive: "Translate(CannotReceivePriorDateSendReceive)",
    StockAdjustmentIsAlreadyDone: "Translate(StockAdjustmentIsAlreadyDone)",
    StockTransferAlreadyDone: "Translate(StockTransferAlreadyDone)",
    MaterialIssueAgainstWOAlreadyDone: "Translate(MaterialIssueAgainstWOAlreadyDone)", 
    IssueQuantityExceeded: "Translate(IssueQuantityExceeded)",
    NotEnoughStock: "Translate(NotEnoughStock)",
    BatchAlreadyAdded: 'Translate(BatchAlreadyAdded)',
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    ExeedTheRequest: "Translate(ExeedTheRequest)",
    Err_FutureDateTransactionNotAllowed: "Translate(Err_FutureDateTransactionNotAllowed)",
    StockBatchFIFOMsg: "Translate(StockBatchFIFOMsg)",
    StockBatchFIFOMsgwithMaterial: "Translate(StockBatchFIFOMsgwithMaterial)",
    VoucherNotGeneratted: "Translate(VoucherNotGeneratted)",
    AllowExcessIssue: "Translate(ConfirmExcessIssue)",
    IssueQtyExceeded: "Translate(IssueQtyExceeded)",

    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)",
    UnableToModify_MA_Exist: "Translate(UnableToModify_MA_Exist)",
    MsgSaveRefErrorWithTransNo: "Translate(MsgSaveRefErrorWithTransNo)",
    UnableToModify_GRNExist: "Translate(UnableToModify_GRNExist)",
    UnableToModify_AllocationOrReturnExist: "Translate(UnableToModify_AllocationOrReturnExist)",
    MIDateLessThanWO: "Translate(MIDateLessThanWO)",
    //Fields
    MRH_PK: "MRH_PK",
    MRD_ITEM: "MRD_ITEM",
    MRH_NO: "MRH_NO",
    MaterialDate: "MIHDATE",
    ITM_NAME: "ITM_NAME",
    MRD_QTY_APPROVED: "MRD_QTY_APPROVED",
    MRD_QTY_ISSUED: "MRD_QTY_ISSUED",
    BALANCE_QTY: "BALANCE_QTY",
    ORG_BALANCE_QTY: "ORG_BALANCE_QTY",
    MID_REMARKS: "MID_REMARKS",
    MID_ITEM: "MID_ITEM",
    MID_PO: "MID_PO",
    MID_PK: "MID_PK",
    MRD_UOM: "MRD_UOM",
    MID_UOM: "MID_UOM",
    UOM_CODE: "UOM_CODE",
    MID_QTY_ISSUED: "MID_QTY_ISSUED",
    DELETE: "delete",
    POVIEW: "poview",
    EDIT: "edit",
    SAVE: "save",
    PREVIOUSSRSVIEW: "previoussrsview",
    MID_SL_NO: "MID_SL_NO",
    MRD_PK: "MRD_PK",
    MID_MR_DTL: "MID_MR_DTL",
    MID_STK_BATCH_NO: "MID_STK_BATCH_NO",
    INBOX: "INBOX",
    QTY_IN_STOCK: "QTY_IN_STOCK",
    MID_MULT_BTCH_GRP: "MID_MULT_BTCH_GRP",
    IsViewMode: false,
    ADDMULTIPLEBATCH: "addmultiplebatch",
    CLEARMULTIPLEBATCH: "clearmultiplebatch",
    IsModifyMI: false,



}
var QtyDec, AmtDec;
var IsEditMode = false;
var IsGridEdit = false;
var IsFillBNo = 0;
var MIHdeptPK = 0;
///#endregion

///#region------ Initialization Section ----------------
$(document).ready(function () {

    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();

    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    GrandScriptUtils.DatePicker("MIHDATE", false, false);
    PageInit();
});


function PageInit() {
    ///<summary>initial page condition</summary>

    if ($("[id$=hdfMenuType]").val() == 2) {
        $("[id$=DivSbu]").show();
    }
    else {
        $("[id$=DivSbu]").hide();
    }

    if ($("[id$=hdfMenuType]").val() != "0")
        MaterialIssue.ListUrl = MaterialIssue.ListUrl + "?Type=" + $("[id$=hdfMenuType]").val();
    $("[id$=ConfirmStockValueChange]").val('0');
    Popup();
    MaterialIssue.BizUnitPk = $("[id$=BizUnitPk]").val();
    var MIObj = $.parseJSON($("[id$=MIList]").val());
    MaterialIssue.MIList = MIObj.MIList;
    var queryStr = window.location.search.substring(1);
    $("#divMaterial").hide();
    $("#hSRSList").hide();

    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "MIPK") || (pK[1] != "" && pK[0] == "RefID")) {
                $("#divMaterial").show();
                //                $("#hSRSList").show();

                FillDetails(MIObj);
            }
            if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                MaterialIssue.IsViewMode = true;
                $("[id$=AddToList]").hide();
            }
            else if (pK[1] == 2 && pK[0] == "Status") {
                MaterialIssue.IsViewMode = true;
            }
            else if (pK[1] == 1 && pK[0] == "IsModify") {
                MaterialIssue.IsModifyMI = true;
                $("[id$=MIH_IS_EDIT]").val("1");
            }
        }
    }
    SetSearchType();

    if ($("[id$=hdfMenuType]").val() == 2) {
        FillOtherSBUS(0);
    }

    MIHdeptPK = MIObj.MIH_DEPT;
    if (MIObj.MIH_DEPT == 0)

        FillStore($("[id$=hdfDeptID]").val());
    else

        FillStore(MIObj.MIH_DEPT);

    if (MIObj.MIH_PK == null || MIObj.MIH_PK == 0)
        FillDepartement($("[id$=hdfSRSStore]").val());
    else {
        $("[id$=MIH_PK]").val(MIObj.MIH_PK);
        FillDepartement(MIObj.MIH_DEPT_TO);
    }

    //FillDepartement($("[id$=hdfSRSStore]").val())

    FillCompany(MIObj.MIH_COMPANY);
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=MIH_COMPANY]").attr("disabled", "disabled");
    }
    FillSRSAutoComplete();
    if (!$.isArray(MaterialIssue.MIList)) {
        MaterialIssue.SRSObj = MaterialIssue.MIList;
        MaterialIssue.MIList = new Array();
        MaterialIssue.MIList.push(MaterialIssue.SRSObj);
    }
    $("#divSaveData").data("SaveData", MaterialIssue.MIList);
    GetDisplayGridData();
    MaterialIssue.MIList = $("#divData").data("MIData");
    $("#divData").data("MIData", MaterialIssue.MIList);
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    AddBatchDropdown();
    FetchingDistinctMRDetails(); //setting MRListDistinct array here
    FillMRNo(MRListDistinct, 0);
    $("select[id$=MIH_DEPT]").focus();
    SetInitialList();
    if (MIObj.MIH_PK != 0)
        $("#divMaterial").show();

    if ($("[id$=hdfEnableBatch]").val() == "0") {
        $("[id$=MID_BATCH]").attr("disabled", true);
    }
    else {
        $("[id$=MID_BATCH]").attr("disabled", false);
    }
    if (MIObj.MIH_PK != 0) {
        IsEditMode = true;
    }
    EnableDisableDate();
    GrandScriptUtils.DatePickerCommon("MIHDATE", false, false, $("[id$=hdfCurrentDate]").val()); 
}
///#endregion

///#region---- Core Section Section----

function PrintPage() {
    //<summary>Function used Print  </summary>
    var url = MaterialIssue.REPORTURL + "?ID=" + $("input[id$=MIH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    OpenPDF(url);
    return false;
}

function CancelPage() {
    //<summary>Function used to redirect to listing page  </summary>
    window.location = MaterialIssue.ListUrl;;
    return false;
}

function Popup() {
    ///<summary>Function used for popup</summary>

    $("#divPreviousSRS").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#divPopupMaterialBatches").dialog({
        autoOpen: false,
        width: 600,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}

function AddBatchDropdown(newItem) {
    var batchDropdown = $("[id$=tblEditMaterial] tr:eq(1) td:eq(2)").clone();
    var batchColIndex = 0;
    MaterialIssue.MIList = $("#divData").data("MIData");
    $("#grdPendingSRSList tr:has(td)").each(function (index) {
        if (index >= 0) {
            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_STK_BATCH_NO", "grdPendingSRSList");
            $(this).find("td:eq(" + batchColIndex + ")").html(batchDropdown.html());
            $(this).find("td:eq(" + batchColIndex + ") [id$=MID_BATCH]").attr("id", (index) + "_MID_BATCH");
            drpBatchID = $(this).find("td:eq(" + batchColIndex + ") [id$=MID_BATCH]").attr("id");
            var batch = 0;
            if (MaterialIssue.MIList.length > 0) {
                batch = MaterialIssue.MIList[index].MID_STK_BATCH;
                BindBatchNo(drpBatchID, MaterialIssue.MIList[index].MID_ITEM, batch, index, true, $(this), newItem);
                $("#" + drpBatchID).show();
               // AddDropdownTooltip(drpBatchID); //adding tooltip to a BatchDropdown     
            }
        }
    });
    $("#divData").data("MIData", MaterialIssue.MIList);
}

function BindBatchNo(drId, matId, batchId, index, notIncludeSelect, rowRef, newItem) {
    ///<summary>Method to fill batch no. when material index changes</summary>
    /// <param name="drId" >
    ///     Dropdown Id to be filled
    /// </param>    
    ///<param name="matId" >
    ///     Material Id of the selected material.
    /// </param>
    ///<param name="batchId" >
    ///     Batch Id to be  selected after filling data
    /// </param>
    ///<param name="index" >
    ///     if Index is specified save the batch list into the corresponding material object
    /// </param>
    ///<param name="notIncludeSelect" type="bool" >
    ///     whether to include --select-- 
    /// Used: if the function is used for dynamically adding batch dropdowns to the grid,do not include 
    /// select
    /// </param>
    var multipleBatchIndex = GrandGrid.Utilities.GetColumnIndex(rowRef, "MID_IS_MULTIPLE_BATCH", "grdPendingSRSList");
    var IsMultiBatch = 0;
    if (multipleBatchIndex > 0) {
        IsMultiBatch = GrandGrid.Utilities.GetColumnValue(rowRef, "MID_IS_MULTIPLE_BATCH", "grdPendingSRSList");
    }
    //    rowRef.closest('tr').removeClass('highlight');
    var batchID = batchId == undefined ? 0 : batchId;
    var date = $("[id$=MIHDATE]").val();
    var currentQty = 0;
    $.getJSON(MaterialIssue.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + matId + "&DepartmentID=" + $("[id$=hdfDeptID]").val() + "&Date=" + date + "&BatchPK=" + batchID + "&transDate=" + $("[id$=MIHDATE]").val() + "&TestResult=1", function (data) {

        //  rowRef.find("td:eq(" + multipleBatchIndex + ")").html(data.length);
        if ($("[id$=hdfEnableBatch]").val() == "1") {
            GrandScriptUtils.FillDropDown(drId, data, true, true, batchID);
            //Multiple batch button visibility
            if (data != null && data.length > 1)
                $("#imgAddBatch_" + (parseInt(index))).show();
            else {
                $("#imgAddBatch_" + (parseInt(index))).hide();
                $("#imgClearBatch_" + (parseInt(index))).hide();
            }
            //Default stock setting
            //                        if (data.length > 0 && newItem==true) {
            //                            var batchID = data[0].Value;
            //                            $.getJSON(MaterialIssue.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (batchData) {
            //                                if (batchData != null) {
            //                                    var qtyStock = batchData[0].SBD_QTY_IN_STOCK;
            //                                    var qtyStockIndx = GrandGrid.Utilities.GetColumnIndex(rowRef, "QTY_IN_STOCK", "grdPendingSRSList");
            //                                    if (qtyStockIndx != null) {
            //                                        var ColQty = GrandGrid.Utilities.GetColumnValue(rowRef, "QTY_IN_STOCK", "grdPendingSRSList");
            //                                        rowRef.find("td:eq(" + qtyStockIndx + ")").html(numberWithCommas(parseFloat(qtyStock).toFixed(QtyDec)));
            //                                    }

            //                                    var IssuedQtyIndex = GrandGrid.Utilities.GetColumnIndex(rowRef, "MID_QTY_ISSUED", "grdPendingSRSList");
            //                                    var IssuedQty = rowRef.find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");

            //                                    var qtyApprovedIndex = GrandGrid.Utilities.GetColumnIndex(rowRef, "MRD_QTY_APPROVED", "grdPendingSRSList");
            //                                    var qtyApproved = GrandGrid.Utilities.GetColumnValue(rowRef, "MRD_QTY_APPROVED", "grdPendingSRSList");

            //                                    if (parseFloat(qtyStock) < parseFloat(qtyApproved.replace(/[^0-9\.]+/g, ""))) {
            //                                        rowRef.find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val(parseFloat(qtyStock).toFixed(QtyDec));
            //                                        rowRef.closest('tr').addClass('highlight');
            //                                        currentQty = qtyStock;
            //                                    }
            //                                    else {
            //                                        rowRef.find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val(parseFloat(qtyApproved).toFixed(QtyDec));
            //                                        currentQty = qtyApproved;
            //                                    }
            //                                    CaptureGridChanges();
            //                                }
            //                            });

            //                        }
        }
        else {
            GrandScriptUtils.FillDropDown(drId, data, true, true, batchID);
            $("#imgAddBatch_" + (parseInt(index))).hide();
            $("#imgClearBatch_" + (parseInt(index))).hide();
        }
        if (parseInt(IsMultiBatch) > 0) {
            $("#imgClearBatch_" + (parseInt(index))).show();
            $("[id$=" + parseInt(index) + "_MID_BATCH]").empty();
            $("[id$=" + parseInt(index) + "_MID_BATCH]").append($("<option> </option>").val("-1").html("Multiple Batches"));
            //  AddDropdownTooltip(drpBatchID); //adding tooltip to a BatchDropdown  
            $("[id$=" + index + "_MID_QTY_ISSUED" + "]").attr("disabled", "true");

            var qtyColIndex = GrandGrid.Utilities.GetColumnIndex(rowRef, "QTY_IN_STOCK", "grdPendingSRSList");
            if (qtyColIndex != null) {
                rowRef.find("td:eq(" + qtyColIndex + ")").html("");
                rowRef.closest('tr').removeClass('highlight');
            }

        }
        else {
            $("#imgClearBatch_" + (parseInt(index))).hide();
            $("[id$=" + "MID_QTY_ISSUED_" + index + "]").attr("enabled", "true");
        }
        if (MaterialIssue.IsViewMode) {
            $("[id$=" + index + "_MID_BATCH" + "]").attr("disabled", "true");
        }
    }

    );
    success:
    if ($("[id$=MIH_STATUS]").val() != 2 && $("[id$=MIH_STATUS]").val() != 5) //Bug ID:23129 After completion of transaction ,no need to highlight record as it may confuse the user.(5=>Closed)
        HighlightRow();
}

function HighlightRow() {
    var qtyStock = 0;
    var qtyStockIndx = 0;
    var ColQty = 0;
    var IssuedQtyIndex = 0;
    var IssuedQty = 0;
    var qtyApprovedIndex = 0;
    var qtyApproved = 0;
    $("#grdPendingSRSList tr:has(td)").each(function (index) {
        if (index >= 0) {
            IssuedQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", "grdPendingSRSList");
            if (MaterialIssue.IsViewMode) {
                IssuedQty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_ISSUED", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
            }
            else
                IssuedQty = $(this).find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");

            qtyApprovedIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_APPROVED", "grdPendingSRSList");
            qtyApproved = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_APPROVED", "grdPendingSRSList");

            $(this).closest('tr').removeClass('highlight');
            var qtyStockIndx = GrandGrid.Utilities.GetColumnIndex($(this), "QTY_IN_STOCK", "grdPendingSRSList");
            if (qtyStockIndx != null) {
                var qtyStock = GrandGrid.Utilities.GetColumnValue($(this), "QTY_IN_STOCK", "grdPendingSRSList").replace(/[^0-9\.]+/g, "")
                if (qtyStock == "") {
                    $(this).closest('tr').removeClass('highlight');
                }
                else {
                    if (parseFloat(qtyStock) < parseFloat(qtyApproved.replace(/[^0-9\.]+/g, ""))) {
                        $(this).closest('tr').addClass('highlight');

                    }
                    else {
                        $(this).closest('tr').removeClass('highlight');
                    }
                }
            }
        }
    });


}
function FillOtherSBUS(SelectedValue) {
    var drpID = $("select[id$=MRH_TO_BIZUNIT]").attr("id");
    $.get(MaterialIssue.FillOtherSBUDropDownURL, function (data) {

        if (data.length > 1) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        }
        $("[id$=MRH_TO_BIZUNIT_VAL]").val($("select[id$=MRH_TO_BIZUNIT]").val());
        //if (requisitionJson.MRH_IS_BZU_TRN != 1) {
        //    FillStore(0);
        //}
    });
}

function FillStore(selectVal) {
    ///<summary>to fill store combo</summary>
    var drpID = $("select[id$=MIH_DEPT]").attr("id");

    $.get(MaterialIssue.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=1&DeptType=-1&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        BindGrid();
        $("select[id$=MIH_DEPT]").attr("disabled", true);
    });

}

function FillBatchNo(itemPK, SelectVal) {
    ///<summary>to fill Batch Number</summary>
    var drpID = $("select[id$=MID_BATCH]").attr("id");
    var date = $("[id$=MIHDATE]").val();
    var ajaxURL = MaterialIssue.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + itemPK + "&DepartmentID=" + $("select[id$=MIH_DEPT]").val() + "&Date=" + date + "&transDate=" + $("[id$=MIHDATE]").val() + "&TestResult=1";
    $.get(ajaxURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectVal);
        BatchChangeEvent();
    });
}
//******#Region: After adding partial qty, cannot add balance qty against other batches***********************
function FillMRNo(data, SelectVal) {
    ///<summary>to fill Batch Number</summary>
    var drpID = $("select[id$=ddlMRNo]").attr("id");
    GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectVal); //MRH_NO,MRH_PK
}
function FillMRItems(SelectVal) {
    MIList = $("#divData").data("MIData");
    var MRItemLst = new Array();
    var MRItemOnlyLst = new Array();
    var SelMRNoPK = $("select[id$=ddlMRNo]").val();
    for (var item in MIList) {
        if (MIList[item].MRH_PK == SelMRNoPK) {
            var MRListObj = new Object();
            if (MRItemOnlyLst.length > 0) {
                if (CheckMRNoExists(MRItemOnlyLst, MIList[item].MID_ITEM)) {
                    MRListObj.Text = MIList[item].ITM_NAME;
                    MRListObj.Value = MIList[item].MID_ITEM;
                    MRItemOnlyLst.push(MRListObj);
                }
            }
            else {
                MRListObj.Text = MIList[item].ITM_NAME;
                MRListObj.Value = MIList[item].MID_ITEM;
                MRItemOnlyLst.push(MRListObj);
            }
        }
    }

    var drpID = $("select[id$=ddlMRItem]").attr("id");
    GrandScriptUtils.FillDropDown(drpID, MRItemOnlyLst, true, true, SelectVal);
    MRItemChange();
}
function MRItemChange() {
    if (IsFillBNo == 0) {
        FillBatchNo($("select[id$=ddlMRItem]").val());
    }
    IsFillBNo = 0;
    //For Fetching   UOM of Material Request w.r.to Item
    var ItemUOMPk = 0;
    MIList = $("#divData").data("MIData");
    var SelMRNoPK = $("select[id$=ddlMRNo]").val();
    var SelMRItemPK = $("select[id$=ddlMRItem]").val();
    for (var item in MIList) {
        if ((MIList[item].MRH_PK == SelMRNoPK) && (MIList[item].MID_ITEM == SelMRItemPK)) {
            ItemUOMPk = MIList[item].MID_UOM;
        }
    }
    FillUOM($("select[id$=ddlMRItem]").val(), ItemUOMPk);
    $("[id$=QTY_IN_STOCK]").text("");
    $("[id$=QtyIssued]").val("");
}
function FetchingDistinctMRDetails() {
    MIList = $("#divData").data("MIData");
    MRListDistinct = new Array();
    for (var item in MIList) {
        var MRListDistinctObj = new Object();
        if (MRListDistinct.length > 0) {
            if (CheckMRNoExists(MRListDistinct, MIList[item].MRH_PK)) {
                MRListDistinctObj.Text = MIList[item].MRH_NO;
                MRListDistinctObj.Value = MIList[item].MRH_PK;
                MRListDistinct.push(MRListDistinctObj);
            }
        }
        else {
            MRListDistinctObj.Text = MIList[item].MRH_NO;
            MRListDistinctObj.Value = MIList[item].MRH_PK;
            MRListDistinct.push(MRListDistinctObj);
        }
    }
}
function CheckMRNoExists(MRListDistinct, MRH_PK) {
    //<summary> Function Used to check whether this MRNo item already added. </summary>  
    var flag = true;
    for (var i in MRListDistinct) {
        if (MRListDistinct[i].Value == MRH_PK) {
            flag = false;
            break;
        }
    }
    return flag;
}
function CheckMRItemExists(data, MID_ITEM) {
    //<summary> Function Used to check whether this MRNo item already added. </summary>  
    var flag = true;
    for (var i in data) {
        if (data[i].Value == MID_ITEM) {
            flag = false;
            break;
        }
    }
    return flag;
}
function CheckItemBatchnoExists(srsPK, itemPK, BatchPk, SlNo) {
    //<summary> Function Used to check whether this po item already added. </summary>
    MaterialIssue.GRNList = $("#divData").data("MIData");
    var flag = true;
    if (SlNo > 0) {
        for (var i in MaterialIssue.MIList) {
            if ((MaterialIssue.MIList[i].MID_ITEM == itemPK) && (MaterialIssue.MIList[i].MID_PO == srsPK) && (MaterialIssue.MIList[i].MID_STK_BATCH == BatchPk) && (MaterialIssue.MIList[i].MID_SL_NO != SlNo)) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in MaterialIssue.MIList) {
            if ((MaterialIssue.MIList[i].MID_ITEM == itemPK) && (MaterialIssue.MIList[i].MID_PO == srsPK) && (MaterialIssue.MIList[i].MID_STK_BATCH == BatchPk)) {
                flag = false;
                break;
            }
        }
    }
    return flag;
}

function CheckIssueQtyExceeds(CurrentIssuingQty) {
    //<summary> Function Used to check whether Issue qty of this item Exceeds than Requested Qty. </summary>
    var flag = true;
    var Qty_Issued = 0;
    var Qty_Requested = 0;
    var Balance_Qty = 0;
    var AlreadyAdded_Qty = 0;
    var QtyIssuedArray;
    if (!$.isArray(QtyIssuedArray)) {
        QtyIssuedArray = new Array();
    }
    QtyIssuedArray = JSLINQ(MaterialIssue.MIList)
        .Where(function (item) { return item.MID_PO == $("select[id$=ddlMRNo]").val() && item.MID_ITEM == $("select[id$=ddlMRItem]").val(); })

    for (var i = 0; i < QtyIssuedArray.items.length; i++) {
        if (i == 0) {
            Qty_Issued = parseInt(QtyIssuedArray.items[i].MRD_QTY_ISSUED); //Qty. Issued
            Qty_Requested = parseInt(QtyIssuedArray.items[i].MRD_QTY_APPROVED); //Req. Qty   
        }
        if (QtyIssuedArray.items[i].MID_SL_NO != $("[id$=hdfSlNo]").val())
            AlreadyAdded_Qty += parseInt(QtyIssuedArray.items[i].MID_QTY_ISSUED);
    }
    Balance_Qty += Qty_Requested - Qty_Issued - AlreadyAdded_Qty;
    if (CurrentIssuingQty > Balance_Qty) {
        flag = false;
    }
    return flag;
}
//******#END Region: After adding partial qty, cannot add balance qty against other batches

function BatchChangeEvent(containerRow) {
    var batchDropdown = $("#grdPendingSRSList tr:eq(1) td:eq(2)").clone();
    var qtyTextbx = $("#grdPendingSRSList tr:eq(1) td:eq(4)").clone();
    var catagory = 0;
    var batchColIndex = 0;
    var qtyIndex = 0;
    var flag = true;
    var stock = 0;
    var quantity = 0;
    var slNO = GrandGrid.Utilities.GetColumnValue(containerRow, "MID_SL_NO", "grdPendingSRSList");
    var reqQuantity = GrandGrid.Utilities.GetColumnValue(containerRow, "MRD_QTY_APPROVED", "grdPendingSRSList");

    var MRH_PK = GrandGrid.Utilities.GetColumnValue(containerRow, "MRH_PK", "grdPendingSRSList");

    var flag = true;
    if (containerRow) {
        // if (containerRow[0].rowIndex >1) {
        if (!isNaN(parseInt(MRH_PK))) {
            $("#grdPendingSRSList tr:has(td)").each(function (index) {
                if (flag) {
                    if (index != containerRow[0].rowIndex - 1) {//index > 0 && 
                        batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_STK_BATCH_NO", "grdPendingSRSList");
                        drpUOMID = $(this).find("td:eq(" + batchColIndex + ") select").attr("id");
                        if (drpUOMID != undefined) {
                            if (containerRow.find("[id$=MID_BATCH]").val() != 0 && containerRow.find("[id$=MID_BATCH]").val() == $("#" + drpUOMID).val()
                                && $("[id$=hdfMenuType]").val() != "1") {
                                containerRow.find("[id$=MID_BATCH]").val(GetOldValue(slNO, containerRow.find("[id$=MID_BATCH] option:first").val()));
                                flag = false;
                                if ((containerRow[0].rowIndex > 1) && flag == false) {
                                    GrandScriptUtils.ShowModal(MaterialIssue.BatchAlreadyAdded, MaterialIssue.MessageBoxTitle);
                                }
                            }
                        }
                    }
                }
            });
        }
    }

    //If the request is from header(tblEditMaterial)
    //if (slNO == "") {
    if (isNaN(parseInt(MRH_PK))) {
        setBalanceReq();
    }

    MaterialIssue.MIList = $("#divData").data("MIData");
    if (containerRow) {
        // if (containerRow[0].rowIndex >1) {
        if (!isNaN(parseInt(MRH_PK))) {
            $("#grdPendingSRSList tr:has(td)").each(function (index) {
                if (flag) {
                    var IssuedQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", "grdPendingSRSList");
                    var IssuedQty = $(this).find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
                    MaterialIssue.MIList[index].MID_QTY_ISSUED = IssuedQty;
                    if (index == containerRow[0].rowIndex - 1) {
                        batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdPendingSRSList");
                        var batchID = containerRow.find("[id$=MID_BATCH]").val();
                        var group = GrandGrid.Utilities.GetColumnValue(containerRow, "MID_MULT_BTCH_GRP", "grdPendingSRSList");
                        var stockIndex = GrandGrid.Utilities.GetColumnIndex(containerRow, "QTY_IN_STOCK", "grdPendingSRSList");
                        var curSlNO = GrandGrid.Utilities.GetColumnValue($(this), "MID_SL_NO", "grdPendingSRSList");
                        $.getJSON(MaterialIssue.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
                            if (data != null) {
                                var qtyStock = data[0].SBD_QTY_IN_STOCK;
                                containerRow.find("td:eq(" + stockIndex + ")").html(qtyStock);
                                MaterialIssue.MIList[index].MID_STK_BATCH = batchID;
                                MaterialIssue.MIList[index].QTY_IN_STOCK = qtyStock;
                                MaterialIssue.MIList[index].ITM_CUR_STK = qtyStock;
                                MaterialIssue.MIList[index].MID_QTY_ISSUED = parseFloat(qtyStock) < parseFloat(MaterialIssue.MIList[index].ORG_BALANCE_QTY.replace(/[^0-9\.]+/g, "")) ? qtyStock : MaterialIssue.MIList[index].ORG_BALANCE_QTY;
                                $("#divData").data("MIData", MaterialIssue.MIList);
                                //Update Save List
                                MaterialIssue.SaveList = $("#divSaveData").data("SaveData")
                                for (var i in MaterialIssue.SaveList) {
                                    if (group == MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP) {
                                        MaterialIssue.SaveList[i].MID_STK_BATCH = batchID;
                                        MaterialIssue.SaveList[i].QTY_IN_STOCK = qtyStock;
                                        MaterialIssue.SaveList[i].ITM_CUR_STK = qtyStock;
                                        MaterialIssue.SaveList[i].MID_QTY_ISSUED = MaterialIssue.MIList[index].MID_QTY_ISSUED;
                                    }
                                }
                                $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
                                GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
                                AddBatchDropdown();
                                EnableDisableDate();
                            }
                            else {
                                //containerRow.find("td:eq(" + stockIndex + ")").html("");
                                var qtyStock = 0;
                                containerRow.find("td:eq(" + stockIndex + ")").html(qtyStock);
                                MaterialIssue.MIList[index].MID_STK_BATCH = 0;
                                MaterialIssue.MIList[index].QTY_IN_STOCK = "";
                                MaterialIssue.MIList[index].ITM_CUR_STK = 0;
                                MaterialIssue.MIList[index].MID_QTY_ISSUED = 0

                                $("#divData").data("MIData", MaterialIssue.MIList);
                                //Update Save List
                                MaterialIssue.SaveList = $("#divSaveData").data("SaveData")
                                for (var i in MaterialIssue.SaveList) {
                                    if (group == MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP) {
                                        MaterialIssue.SaveList[i].MID_STK_BATCH = 0;
                                        MaterialIssue.SaveList[i].QTY_IN_STOCK = "";
                                        MaterialIssue.SaveList[i].ITM_CUR_STK = 0;
                                        MaterialIssue.SaveList[i].MID_QTY_ISSUED = MaterialIssue.MIList[index].MID_QTY_ISSUED;
                                    }
                                }
                                $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
                                GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
                                AddBatchDropdown();
                                EnableDisableDate();
                            }
                        });
                    }
                }
                else {
                    if (index == containerRow[0].rowIndex - 1) {
                        var qtyStock = 0;
                        containerRow.find("td:eq(" + stockIndex + ")").html(qtyStock);
                        MaterialIssue.MIList[index].MID_STK_BATCH = 0;
                        MaterialIssue.MIList[index].QTY_IN_STOCK = "";
                        MaterialIssue.MIList[index].ITM_CUR_STK = 0;
                        MaterialIssue.MIList[index].MID_QTY_ISSUED = 0

                        $("#divData").data("MIData", MaterialIssue.MIList);
                        //Update Save List
                        MaterialIssue.SaveList = $("#divSaveData").data("SaveData")
                        var group = GrandGrid.Utilities.GetColumnValue(containerRow, "MID_MULT_BTCH_GRP", "grdPendingSRSList");
                        for (var i in MaterialIssue.SaveList) {
                            if (group == MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP) {
                                MaterialIssue.SaveList[i].MID_STK_BATCH = 0;
                                MaterialIssue.SaveList[i].QTY_IN_STOCK = "";
                                MaterialIssue.SaveList[i].ITM_CUR_STK = 0;
                                MaterialIssue.SaveList[i].MID_QTY_ISSUED = MaterialIssue.MIList[index].MID_QTY_ISSUED;
                            }
                        }
                        $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
                        GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
                        AddBatchDropdown();
                        EnableDisableDate();
                    }
                }
            });

        }
    }

    //******#Region In the Case of Edit mode we need to showing the saved batches current stock (This region used For Edit Mode only)****************
    if (IsEditMode && !IsGridEdit) {
        $("#grdPendingSRSList tr:has(td)").each(function (index) {
            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdPendingSRSList");
            var batchID = $(this).find("[id$=MID_BATCH]").val();
            var stockIndex = GrandGrid.Utilities.GetColumnIndex($(this), "QTY_IN_STOCK", "grdPendingSRSList");
            $.getJSON(MaterialIssue.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
                if (data != null) {
                    var qtyStock = data[0].SBD_QTY_IN_STOCK;
                    if ($("[id$=MIH_STATUS]").val() != 0) {
                        qtyStock += parseFloat(MaterialIssue.MIList[index].MID_QTY_ISSUED); //For Excluding the Stock Usage of Current transaction
                    }
                    $(this).find("td:eq(" + stockIndex + ")").html(qtyStock);
                    MaterialIssue.MIList[index].MID_STK_BATCH = batchID;
                    MaterialIssue.MIList[index].QTY_IN_STOCK = qtyStock;
                    MaterialIssue.MIList[index].ITM_CUR_STK = qtyStock;
                    $("#divData").data("MIData", MaterialIssue.MIList);
                    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
                    AddBatchDropdown();
                    EnableDisableDate();
                }
                else {
                    $(this).find("td:eq(" + stockIndex + ")").html("");
                }
            });
        });
    }
    //******#END Region****************
}

var OldValue = 0;

function GetOldValue(slNO, oldVal) {
    for (var i in MaterialIssue.MIList) {
        if (MaterialIssue.MIList[i].SL_NO == slNO) {
            if (MaterialIssue.MIList[i].MID_STK_BATCH == undefined) {
                return oldVal;
            }
            else {
                return MaterialIssue.MIList[i].MID_STK_BATCH;
            }
        }
    }
}

function setBalanceReq() {
    var balanceReq = 0;
    var batchID = $("[id$=MID_BATCH]").val();
    var category = $("[id$=MaterialCatagory]").val();
    $.getJSON(MaterialIssue.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
        if (data != null) {
            var qtyStock = data[0].SBD_QTY_IN_STOCK;
            $("[id$=QTY_IN_STOCK]").text(qtyStock.toFixed(QtyDec));
        }
        else {
            $("[id$=QTY_IN_STOCK]").text();
        }
    });
}

function FillDepartement(SelectedValue, StorePK) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    ClearPrevSRSAndHeader();
    $("[id$=AddToList]").hide();
    ClearDetails();
    var drpID = $("select[id$=MIH_DEPT_TO]").attr("id");
    $.get(MaterialIssue.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=-1&DeptPk=0&MenuType=" + $("[id$=hdfMenuType]").val(), function (data) {
        if ($("[id$=hdfMenuType]").val() == "1" && SelectedValue == 0)
            GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
        if (StorePK != null) { FillStore(StorePK); }
        if ($("select[id$=MIH_DEPT]").val() != 0) {
            $("#" + drpID + " option[value=" + $("select[id$=MIH_DEPT]").val() + "]").remove();
            BindGrid();
        }


    });
}

function FillCompany(selectVal) {
    var drpID = $("select[id$=MIH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = MaterialIssue.FillCompanyDropdownURL + MaterialIssue.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val();
    }
    else {
        getURL = MaterialIssue.FillCompanyDropdownURL + MaterialIssue.BizUnitPk + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);

    });

    //    if (selectVal == undefined || selectVal == 0) {
    //        var drpID = $("select[id$=MIH_COMPANY]").attr("id");
    //        $.get(MaterialIssue.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
    //            var selCompany = $("[id$=hdfSelCompany]").val();
    //            GrandScriptUtils.FillDropDown(drpID, data, true, false, selCompany);
    //        });
    //    }
    //    else {
    //        var drpID = $("select[id$=MIH_COMPANY]").attr("id");
    //        $.get(MaterialIssue.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
    //            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
    //        });
    //    }
}

function SetInitialList() {
    if ($("[id$=hdfAppID]").val() != "0") {
        $.get(MaterialIssue.GetSRSDetails + $("[id$=hdfAppID]").val(), function (data) {
            FillDepartement(data.Table1[0].MRH_DEPT_PK, data.Table1[0].MRH_DEPT_STR_PK);
        });
    }
}


function FillUOM(catgID, selectVal) {
    ///<summary>function To Fill UOM Details </summary>

    var drpID = $("select[id$=UOM]").attr("id");
    $.get(MaterialIssue.UomURL + catgID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        GetUomConversion(selectVal);
    });
}

function FillDetails(MIObj) {

    $("[id$=MIH_PK]").val(MIObj.MIH_PK);

    if (MIObj.MIHNO == null || MIObj.MIHNO == "") {
        $("[id$=lblMINo]").html(MaterialIssue.DocGenerationNewValue);
        $("[id$=MIHNO]").val("");
    }
    else {
        $("[id$=lblMINo]").html(MIObj.MIHNO);
        $("[id$=MIHNO]").val(MIObj.MIHNO);
    }


    $("[id$=MIH_STATUS]").val(MIObj.MIH_STATUS);
    
    $("[id$=MIHDATE]").val(MIObj.MIHDATE);
    //Set a stamp for cancelled record
    if (MIObj.MIH_STATUS == 4)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");

    //End
    
    $("[id$=hdfMihDate]").val() == MIObj.MIHDATE;
}

function FillSRSAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    var AutoURL = MaterialIssue.SRSAutoURL;
    var PendingWo = 1;
    if ($("[id$=ChkPendingWO]").is(":checked") == true) {
        PendingWo = 1;
    }
    else {
        PendingWo = 0;
    }
    if ($("[id$=hdfMenuType]").val() == "1")
        AutoURL = MaterialIssue.WIHAutoURL+ "&IsPendingWO=" + PendingWo + "";
    GrandScriptUtils.MakeAutoComplete("SearchValue", AutoURL, false, true, false, "SearchType", false, "MIH_DEPT", "MIH_DEPT_TO");
}

function GetUomConversion(UOMID, isChanged) {
    //<summary> Function Used to get the convertion factor </summary>

    if (UOMID != "0") {
        $.get(MaterialIssue.GetMaterialUOMConversion + $("select[id$=ddlMRItem]").val() + "&UOMId=" + UOMID, function (data) {
            if (data.length > 0) {
                $("[id$=POD_CONV_FACT]").val(data[0].UMC_CONV_FACT);
                if (isChanged)
                    $("[id$=QtyIssued]").val(parseFloat(data[0].UMC_CONV_FACT) * $("[id$=QtyIssuedOrg]").val());
            }
        });
    }
    else {
        $("[id$=POD_CONV_FACT]").val("1");
    }
}

function BindGrid() {
    ///<summary>To handle bind grid </summary>


    var store = 0;
    var dept = 0;
    var PendingWo = 1;

    var storeValue = $("[id$=MIH_DEPT_TO]").val();
    var departValue = $("[id$=MIH_DEPT]").val();
    if ((storeValue == undefined) || (storeValue == "0") || (storeValue == null)) {
        store = 1;
    }
    if ((departValue == undefined) || (departValue == "0") || (departValue == null)) {
        dept = 1;
    }

    if ($("[id$=ChkPendingWO]").is(":checked") == true) {
        PendingWo = 1;
    }
    else {
        PendingWo = 0;
    }

    if (store == 0 && dept == 0) {
        var ajaxUrl = MaterialIssue.SRSPendingBindGridURL + $("[id$=MIH_PK]").val() + "&Store=" + $("[id$=MIH_DEPT]").val() + "&Dept=" + $("[id$=MIH_DEPT_TO]").val() + "&BizUnit=" + MaterialIssue.BizUnitPk + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&mrhPK=" + $("[id$=hdfApplicationID]").val();
        if ($("[id$=hdfMenuType]").val() == "1") {
            var searchField = $("[id$=SearchType]").val();
            if (searchField == "0")
                searchField = "WIH_NO";
            ajaxUrl = MaterialIssue.WOPendingBindGridURL + $("[id$=MIH_PK]").val() + "&Store=" + $("[id$=MIH_DEPT]").val() + "&Dept=" + $("[id$=MIH_DEPT_TO]").val() + "&BizUnit=" + MaterialIssue.BizUnitPk + "&Status=" + searchField + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&mrhPK=" + $("[id$=hdfApplicationID]").val() + "&IsPendingWO=" + PendingWo + "";
        }
        $("#grdSRSList").removeAttr("ajaxurl")
        $("#grdSRSList").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdSRSList");
        GrandGrid.MakeGrid($("#grdSRSList"));
        FillSRSAutoComplete();

    }
    else {
        var ajaxUrl = new Array();
        $("#grdSRSList").removeAttr("ajaxurl")
        $("#grdSRSList").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdSRSList");
        GrandGrid.MakeGrid($("#grdSRSList"), 0, ajaxUrl);
    }
    return false;
}



function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "SearchValue") {
        BindGrid();
    }
    else if (targetControlID == "ChkPendingWO") {
        FillSRSAutoComplete();
    }
    
}

function BindPreviousSRSGrid() {
    ///<summary>To handle bind previuos GRN grid </summary>

    var ajaxUrl = MaterialIssue.FillPreviousSRSDetailsView + MaterialIssue.SRSPK;
    $("#grdPreviousSRS").removeAttr("ajaxurl")
    $("#grdPreviousSRS").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPreviousSRS");
    GrandGrid.MakeGrid($("#grdPreviousSRS"));
    MaterialIssue.SRSPK = 0;
}

function AddNew() {
    ///<summary>Function used to add new details</summary>

    ClearDetails();
    MaterialIssue.MIList = new Array();
    $("#divData").data("MIData", MaterialIssue.MIList);
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    // FillStore();
    ClearPrevSRSAndHeader();
    $("[id$=AddToList]").hide();
    BindGrid();
    EnableDisableDate();
}

function AddToList() {
    ///<summary>Function used to add the needed po material list</summary>
    ///var checkedornot = 0;
    var grdID;
    CaptureGridChanges();
    // MaterialIssue.MIList = $("#divData").data("MIData");
    MaterialIssue.SaveList = $("#divSaveData").data("SaveData");

    $("#grdSRSList tr:has(td)").each(function () {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            $("#divMaterial").show();
            $("#hSRSList").show();

            grdID = $(this).parents("table:first").attr("id");
            //            [MRH_NO],[MRH_PK],[MRD_ITEM],[ITM_NAME],[MRD_UOM],[UOM_CODE],[MRD_QTY_APPROVED],[MRD_QTY_ISSUED],[BALANCE_QTY]',--%>
            MaterialIssue.MIObj = new Object();
            MaterialIssue.MIObj.MID_PO = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRH_PK, grdID);
            MaterialIssue.MIObj.MID_ITEM = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_ITEM, grdID);
            if (CheckItemExists(MaterialIssue.MIObj.MID_PO, MaterialIssue.MIObj.MID_ITEM)) {
                var maxSlNo = JSLINQ(MaterialIssue.SaveList)
                    .Max(function (MIitem) { return MIitem.MID_SL_NO; });
                MaterialIssue.MIObj.MID_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
                var maxGroupNo = JSLINQ(MaterialIssue.SaveList)
                    .Max(function (MIitem) { return MIitem.MID_MULT_BTCH_GRP; });
                MaterialIssue.MIObj.MID_MULT_BTCH_GRP = maxGroupNo == null || maxGroupNo == 0 ? 1 : parseInt(maxGroupNo) + 1;
                MaterialIssue.MIObj.MID_IS_MULTIPLE_BATCH = 0;
                MaterialIssue.MIObj.MID_MR_DTL = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_PK, grdID);
                // checkedornot = 1;
                MaterialIssue.MIObj.MID_PK = 0;
                // MaterialIssue.MIObj.MID_STK_BATCH = 0;
                MaterialIssue.MIObj.MRH_NO = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRH_NO, grdID);
                MaterialIssue.MIObj.ITM_NAME = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.ITM_NAME, grdID);
                MaterialIssue.MIObj.MID_UOM = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_UOM, grdID);
                MaterialIssue.MIObj.MRD_QTY_APPROVED = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_QTY_APPROVED, grdID).replace(/[^0-9\.]+/g, "");
                MaterialIssue.MIObj.MRD_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, "");
                MaterialIssue.MIObj.MID_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.BALANCE_QTY, grdID).replace(/[^0-9\.]+/g, "");

                MaterialIssue.MIObj.ORG_BALANCE_QTY = 0;
                MaterialIssue.MIObj.ORG_BALANCE_QTY = MaterialIssue.MIObj.MID_QTY_ISSUED.replace(/[^0-9\.]+/g, "");
                MaterialIssue.MIObj.UOM_CODE = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.UOM_CODE, grdID);
                MaterialIssue.MIObj.MID_REMARKS = "";
                MaterialIssue.MIObj.MRH_PK = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRH_PK, grdID);

                MaterialIssue.SaveList.push(MaterialIssue.MIObj);
                $(this).find("td:first").find("input[type=checkbox]").attr("checked", 0);
            }
        }
    });

    if (MaterialIssue.SaveList.length == 0) {
        GrandScriptUtils.ShowModal(MaterialIssue.AddMaterialIssueDetails, MaterialIssue.MessageBoxTitle);
        return false;
    }

    $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
    GetDisplayGridData();
    MaterialIssue.MIList = $("#divData").data("MIData");
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    AddBatchDropdown(true);
    FetchingDistinctMRDetails(); //setting MRListDistinct array here
    FillMRNo(MRListDistinct, 0);
    EnableDisableDate();


    //    $("#divData").data("MIData", MaterialIssue.MIList); 
    //    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    //        AddBatchDropdown(); 
    ////    FetchingDistinctMRDetails(); //setting MRListDistinct array here
    ////    FillMRNo(MRListDistinct, 0);
    //    //Copy existing list to save list
    //    //MaterialIssue.SaveList = new Array()
    //    MaterialIssue.SaveList = $("#divSaveData").data("SaveData");
    //    MaterialIssue.SaveList = CopyList(MaterialIssue.SaveList, MaterialIssue.MIList);
    //    $("#divSaveData").data("SaveData", MaterialIssue.SaveList); 
    return false;
}
function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case MaterialIssue.SAVE:
            var RedirectURL = MaterialIssue.MIListUrl;
            if ($("[id$=hdfMenuType]").val() == "1")
                RedirectURL = RedirectURL + "?Type=1";
            window.location = RedirectURL;
            break;
        case MaterialIssue.DELETE:
            DeleteSRSMaterial();
            break;
        case MaterialIssue.INBOX:
            window.location = MaterialIssue.InboxURL;
            break;
    }
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for Catch all the grid events in this function </summary>
    var grdID;
    switch (command.toString().toLowerCase()) {
        case MaterialIssue.DELETE:
            grdID = $(tr).parents("table:first").attr("id");
            MaterialIssue.SRSPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_PO, grdID);
            MaterialIssue.ItemPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_ITEM, grdID);
            MaterialIssue.MID_MULT_BTCH_GRP = GrandGrid.Utilities.GetColumnValue(tr, "MID_MULT_BTCH_GRP", grdID);
            GrandScriptUtils.ShowModal(MaterialIssue.DeleteConfirmMsg, MaterialIssue.Confirmation, MaterialIssue.DELETE, true);
            break;

        case MaterialIssue.PREVIOUSSRSVIEW:
            grdID = $(tr).parents("table:first").attr("id");
            MaterialIssue.SRSPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MRH_PK, grdID);
            ViewPreviousSRSDetails();
            break;
        case MaterialIssue.EDIT:
            FillGRNDetails(tr);
            break;
        case MaterialIssue.ADDMULTIPLEBATCH:
            MultipleBatchPopup(tr);
            return false;
            break;
        case MaterialIssue.CLEARMULTIPLEBATCH:
            ClearMultipleBatch(tr);
            break;
    }
    return false;
}
function ClearMultipleBatch(tr) {

    var TempSaveList = $("#divSaveData").data("SaveData");
    MaterialIssue.SaveList = new Array();
    var Group = GrandGrid.Utilities.GetColumnValue(tr, "MID_MULT_BTCH_GRP", $(tr).parents("table:first").attr("id"));
    var qtyApproved = GrandGrid.Utilities.GetColumnValue(tr, "MRD_QTY_APPROVED", $(tr).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
    var qtyIssued = GrandGrid.Utilities.GetColumnValue(tr, "MRD_QTY_ISSUED", $(tr).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
    var qtyIssuing = parseFloat(qtyApproved).toFixed(QtyDec) - parseFloat(qtyIssued).toFixed(QtyDec);
    //
    //

    var insertFlag = false;
    for (var i in TempSaveList) {
        if (TempSaveList[i].MID_MULT_BTCH_GRP == Group && insertFlag == false) {
            insertFlag = true;
            TempSaveList[i].MID_IS_MULTIPLE_BATCH = 0;
            TempSaveList[i].MID_QTY_ISSUED = qtyIssuing;
            TempSaveList[i].MID_STK_BATCH = 0;
            TempSaveList[i].MID_STK_BATCH_NO = "";
            TempSaveList[i].QTY_IN_STOCK = "";
            TempSaveList[i].MID_PK = 0
            MaterialIssue.SaveList.push(jQuery.extend(true, {}, TempSaveList[i]));
        }
        else if (TempSaveList[i].MID_MULT_BTCH_GRP != Group) {
            MaterialIssue.SaveList.push(jQuery.extend(true, {}, TempSaveList[i]));
        }

    }

    $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
    GetDisplayGridData();
    MaterialIssue.MIList = $("#divData").data("MIData");
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    AddBatchDropdown();
    FetchingDistinctMRDetails(); //setting MRListDistinct array here
    FillMRNo(MRListDistinct, 0);
    ClearPopupData();
    EnableDisableDate();
    return false;
}
function RemovePopupValidation() {
    $(document.forms[0]).validate().resetForm();
    $("input[id$=txtQtyPopUp]").rules("remove");
    $("select[id$=ddlPopUpBatch]").rules("remove");
    return false;
}
function ClearPopupData() {


    $("[id$=lblItemNamePopup]").html("");
    $("[id$=lblQtyRequired]").html("");
    $("[id$=lblQtyAlradyIssued]").html("");
    $("[id$=lblTotalIssuingQty]").html("");
    $("[id$=lblQtyBalance]").html("");
    $("[id$=lblStockPopUp]").html("");
    $("[id$=txtQtyPopUp]").val("");
    MaterialIssue.GridList = new Array();
    MaterialIssue.MaterialIssueRowObj = new Object();
    //    GrandGrid.Utilities.ResetGrid(true, "grdBatchDetails");
    $("#divGridData").data("BatchData", MaterialIssue.GridList);
    GrandGrid.MakeGrid($("#grdBatchDetails"), 0, MaterialIssue.GridList);
}

//Copy One List to Another
function CopyList(targetList, sourceList) {
    for (var i in sourceList) {
        targetList.push(jQuery.extend(true, {}, sourceList[i]));
    }
    return targetList;
}

function ApplyPopUpBatches() {
    MaterialIssue.GridList = $("#divGridData").data("BatchData");
    if (MaterialIssue.GridList == null || MaterialIssue.GridList.length == 0) {
        GrandScriptUtils.ShowModal(MaterialIssue.MaterialBatchValidation, MaterialIssue.MessageBoxTitle);
        return false;
    }
    MaterialIssue.MIList = $("#divData").data("MIData");
    MaterialIssue.SaveList = $("#divSaveData").data("SaveData");
    for (var i in MaterialIssue.SaveList) {
        if (MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP == MaterialIssue.MaterialIssueRowObj.MID_MULT_BTCH_GRP) {
            var groupArray = JSLINQ(MaterialIssue.SaveList).Where(function (item) { return item.MID_MULT_BTCH_GRP == MaterialIssue.MaterialIssueRowObj.MID_MULT_BTCH_GRP; }).ToArray();
            MaterialIssue.SaveList.splice(i, groupArray.length);
        }
    }
    MaterialIssue.SaveList = CopyList(MaterialIssue.SaveList, MaterialIssue.GridList);
    var srl = 1;
    $("#grdPendingSRSList tr:has(td)").each(function (index) {
        var GroupIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
        var Group = GrandGrid.Utilities.GetColumnValue($(this), "MID_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
        for (var i in MaterialIssue.SaveList) {
            if (MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP == Group) {
                MaterialIssue.SaveList[i].MID_SL_NO = srl;
                srl = srl + 1;
            }
        }
    });
    $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
    GetDisplayGridData();
    MaterialIssue.MIList = $("#divData").data("MIData");
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    AddBatchDropdown();
    FetchingDistinctMRDetails(); //setting MRListDistinct array here
    FillMRNo(MRListDistinct, 0);
    ClearPopupData();
    $("#divPopupMaterialBatches").dialog("close");
    EnableDisableDate();
    return false;
}
function MultipleBatchPopup(tr) {
    ClearPopupData();
    MaterialIssue.MaterialIssueRowObj.MID_PK = GrandGrid.Utilities.GetColumnValue(tr, "MID_PK", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_PO = GrandGrid.Utilities.GetColumnValue(tr, "MID_PO", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MRH_PK = GrandGrid.Utilities.GetColumnValue(tr, "MRH_PK", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_SL_NO = GrandGrid.Utilities.GetColumnValue(tr, "MID_SL_NO", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_MR_DTL = GrandGrid.Utilities.GetColumnValue(tr, "MID_MR_DTL", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_STK_BATCH = GrandGrid.Utilities.GetColumnValue(tr, "MID_STK_BATCH", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    MaterialIssue.MaterialIssueRowObj.MID_IS_MULTIPLE_BATCH = GrandGrid.Utilities.GetColumnValue(tr, "MID_IS_MULTIPLE_BATCH", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_MULT_BTCH_GRP = GrandGrid.Utilities.GetColumnValue(tr, "MID_MULT_BTCH_GRP", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_ITEM = GrandGrid.Utilities.GetColumnValue(tr, "MID_ITEM", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_UOM = GrandGrid.Utilities.GetColumnValue(tr, "MID_UOM", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.ORG_BALANCE_QTY = GrandGrid.Utilities.GetColumnValue(tr, "ORG_BALANCE_QTY", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    MaterialIssue.MaterialIssueRowObj.MRH_NO = GrandGrid.Utilities.GetColumnValue(tr, "MRH_NO", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.ITM_NAME = GrandGrid.Utilities.GetColumnValue(tr, "ITM_NAME", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_STK_BATCH_NO = GrandGrid.Utilities.GetColumnValue(tr, "MID_STK_BATCH_NO", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.QTY_IN_STOCK = GrandGrid.Utilities.GetColumnValue(tr, "QTY_IN_STOCK", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    MaterialIssue.MaterialIssueRowObj.MRD_QTY_APPROVED = GrandGrid.Utilities.GetColumnValue(tr, "MRD_QTY_APPROVED", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    MaterialIssue.MaterialIssueRowObj.MRD_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue(tr, "MRD_QTY_ISSUED", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    // MaterialIssue.MaterialIssueRowObj.MID_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue(tr, "MID_QTY_ISSUED", "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.UOM_CODE = GrandGrid.Utilities.GetColumnValue(tr, "UOM_CODE", "grdPendingSRSList");

    var remkColIndex = GrandGrid.Utilities.GetColumnIndex(tr, MaterialIssue.MID_REMARKS, "grdPendingSRSList");
    MaterialIssue.MaterialIssueRowObj.MID_REMARKS = tr.find("td:eq(" + remkColIndex + ") input[type=text]").val();

    var matId = GrandGrid.Utilities.GetColumnValue(tr, "MID_ITEM", "grdPendingSRSList");
    var slNO = GrandGrid.Utilities.GetColumnValue(tr, "MID_SL_NO", "grdPendingSRSList");
    var batchID = $("[id$=" + (parseInt(slNO) - 1).toString() + "_MID_BATCH] option:selected").val();
    var batchNo = $("[id$=" + (parseInt(slNO) - 1).toString() + "_MID_BATCH] option:selected").html();

    var ItemName = GrandGrid.Utilities.GetColumnValue(tr, "ITM_NAME", $(tr).parents("table:first").attr("id"));
    var SelectedQty = GrandGrid.Utilities.GetColumnValue(tr, "QTY_IN_STOCK", $(tr).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
    var ReqQty = GrandGrid.Utilities.GetColumnValue(tr, "MRD_QTY_APPROVED", $(tr).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
    // var IssuedQty = GrandGrid.Utilities.GetColumnValue(tr, "MID_QTY_ISSUED", $(tr).parents("table:first").attr("id"));
    var IssuedQtyIndex = GrandGrid.Utilities.GetColumnIndex(tr, "MID_QTY_ISSUED", "grdPendingSRSList");
    var IssuedQty = 0;
    //    if (MaterialIssue.IsViewMode) {
    //        IssuedQty = GrandGrid.Utilities.GetColumnValue(tr, "MID_QTY_ISSUED", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    //    }
    //    else {
    //        IssuedQty = tr.find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
    //    }
    IssuedQty = GrandGrid.Utilities.GetColumnValue(tr, "MID_QTY_ISSUED", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
    IssuedQty = IssuedQty == "" ? 0 : IssuedQty;
    MaterialIssue.MaterialIssueRowObj.MID_QTY_ISSUED = IssuedQty;
    var alreadyIssuedQty = GrandGrid.Utilities.GetColumnValue(tr, "MRD_QTY_ISSUED", $(tr).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");

    SelectedQty = SelectedQty == "" ? 0 : SelectedQty;
    //ReqQty = ReqQty.replace(',', '');


    MaterialIssue.GridList = new Array();
    if (MaterialIssue.MaterialIssueRowObj.MID_IS_MULTIPLE_BATCH > 0) {
        IssuedQty = 0;
        MaterialIssue.SaveList = $("#divSaveData").data("SaveData")
        var slno = 1;
        for (var i in MaterialIssue.SaveList) {
            if (MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP == MaterialIssue.MaterialIssueRowObj.MID_MULT_BTCH_GRP) {
                MaterialIssue.SaveList[i].SLNO = slno++;
                MaterialIssue.GridList.push(jQuery.extend(true, {}, MaterialIssue.SaveList[i]));
                IssuedQty += parseFloat(MaterialIssue.SaveList[i].MID_QTY_ISSUED);
                SelectedQty = IssuedQty;
            }
        }
    }
    else {
        if (batchID > 0) {
            var currentIssuedQty = 0;
            var TempBatchDtls = new Object();
            if (MaterialIssue.IsViewMode) {
                IssuedQty = GrandGrid.Utilities.GetColumnValue(tr, "MID_QTY_ISSUED", "grdPendingSRSList").replace(/[^0-9\.]+/g, "");
            }
            else {
                IssuedQty = tr.find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
            }
            TempBatchDtls.SLNO = 1;
            TempBatchDtls.QTY_IN_STOCK = SelectedQty;
            TempBatchDtls.MID_QTY_ISSUED = IssuedQty;
            TempBatchDtls.MID_STK_BATCH_NO = batchNo;
            TempBatchDtls.MID_STK_BATCH = batchID;
            TempBatchDtls.MID_PK = MaterialIssue.MaterialIssueRowObj.MID_PK;
            TempBatchDtls.MID_PO = MaterialIssue.MaterialIssueRowObj.MID_PO;
            TempBatchDtls.MRH_PK = MaterialIssue.MaterialIssueRowObj.MRH_PK;
            TempBatchDtls.MID_SL_NO = MaterialIssue.MaterialIssueRowObj.MID_SL_NO;
            TempBatchDtls.MID_MR_DTL = MaterialIssue.MaterialIssueRowObj.MID_MR_DTL;
            TempBatchDtls.MID_IS_MULTIPLE_BATCH = 1;
            TempBatchDtls.MID_MULT_BTCH_GRP = MaterialIssue.MaterialIssueRowObj.MID_MULT_BTCH_GRP;
            TempBatchDtls.MID_ITEM = MaterialIssue.MaterialIssueRowObj.MID_ITEM;
            TempBatchDtls.MID_UOM = MaterialIssue.MaterialIssueRowObj.MID_UOM;
            TempBatchDtls.ORG_BALANCE_QTY = MaterialIssue.MaterialIssueRowObj.ORG_BALANCE_QTY;
            TempBatchDtls.MRH_NO = MaterialIssue.MaterialIssueRowObj.MRH_NO;
            TempBatchDtls.ITM_NAME = MaterialIssue.MaterialIssueRowObj.ITM_NAME;
            TempBatchDtls.MRD_QTY_APPROVED = MaterialIssue.MaterialIssueRowObj.MRD_QTY_APPROVED;
            TempBatchDtls.MRD_QTY_ISSUED = MaterialIssue.MaterialIssueRowObj.MRD_QTY_ISSUED; //Some doubt
            TempBatchDtls.UOM_CODE = MaterialIssue.MaterialIssueRowObj.UOM_CODE;
            TempBatchDtls.MID_REMARKS = MaterialIssue.MaterialIssueRowObj.MID_REMARKS;
            MaterialIssue.GridList.push(TempBatchDtls);
        }
    }

    ReqQty = ReqQty.replace(/[^0-9\.]+/g, "");
    $("[id$=lblPopupUOM]").html(MaterialIssue.MaterialIssueRowObj.UOM_CODE);
    $("[id$=lblItemNamePopup]").html(ItemName);
    $("[id$=lblQtyRequired]").html(parseFloat(ReqQty).toFixed(QtyDec));
    $("[id$=lblQtyAlradyIssued]").html(parseFloat(alreadyIssuedQty).toFixed(QtyDec));
    $("[id$=lblTotalIssuingQty]").html(parseFloat(IssuedQty).toFixed(QtyDec));
    //    if (MaterialIssue.IsViewMode) {
    //        $("[id$=lblQtyBalance]").html(parseFloat(parseFloat(ReqQty) - (parseFloat(alreadyIssuedQty))).toFixed(QtyDec));
    //    }
    //    else {
    $("[id$=lblQtyBalance]").html(parseFloat(parseFloat(ReqQty) - (parseFloat(IssuedQty) + parseFloat(alreadyIssuedQty))).toFixed(QtyDec));
    //}

    $("#divGridData").data("BatchData", MaterialIssue.GridList);
    GrandGrid.MakeGrid($("#grdBatchDetails"), 0, MaterialIssue.GridList);

    var date = $("[id$=MIHDATE]").val();
    var drpID = $("select[id$=ddlPopUpBatch]").attr("id");
    batchID = batchID == undefined ? 0 : batchID;
    $.getJSON(MaterialIssue.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + matId + "&DepartmentID=" + $("[id$=hdfDeptID]").val() + "&Date=" + date + "&BatchPK=" + batchID + "&transDate=" + $("[id$=MIHDATE]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
        // FillPopUpBatchQuantity(0);
    });
    if (MaterialIssue.IsViewMode) {
        $("[id$=btnApplyBatches]").hide();
        $("[id$=imgAddBatchPopup]").hide();

    }
    $("#divPopupMaterialBatches").dialog("open");
    RemovePopupValidation();
}
function AddPopUpBatchDtls() {
    RemoveValidations();
    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        MaterialIssue.GridList = $("#divGridData").data("BatchData");
        if (MaterialIssue.GridList == undefined)
            MaterialIssue.GridList = new Array();
        var batchID = $("[id$=ddlPopUpBatch] option:selected").val();
        var batchNo = $("[id$=ddlPopUpBatch] option:selected").text();
        var SelectedQty = $("[id$=lblStockPopUp]").html().replace(/[^0-9\.]+/g, "");
        var IssueQty = $("[id$=txtQtyPopUp]").val().replace(/[^0-9\.]+/g, "");



        //Duplicate checking
        if (IsSameBatchExist(batchID)) {
            GrandScriptUtils.ShowModal(MaterialIssue.BatchAlreadyAdded, MaterialIssue.MessageBoxTitle);
            return false;
        }
        //Quantity Checking
        if (parseFloat(IssueQty) > parseFloat(SelectedQty)) {
            GrandScriptUtils.ShowModal(MaterialIssue.NotEnoughStock, MaterialIssue.MessageBoxTitle);
            return false;
        }
        else
            if (parseFloat(IssueQty) > parseFloat($("[id$=lblQtyBalance]").html())) {
                GrandScriptUtils.ShowModal(MaterialIssue.ExeedTheRequest, MaterialIssue.MessageBoxTitle);
                return false;
            }
        if (batchID > 0) {
            var TempBatchDtls = new Object();
            TempBatchDtls.SLNO = MaterialIssue.GridList == undefined ? 1 : MaterialIssue.GridList.length;
            TempBatchDtls.QTY_IN_STOCK = SelectedQty;
            TempBatchDtls.MID_QTY_ISSUED = IssueQty;
            TempBatchDtls.MID_STK_BATCH_NO = batchNo;
            TempBatchDtls.MID_STK_BATCH = batchID;
            TempBatchDtls.MID_PK = 0;
            TempBatchDtls.MID_PO = MaterialIssue.MaterialIssueRowObj.MID_PO;
            TempBatchDtls.MRH_PK = MaterialIssue.MaterialIssueRowObj.MRH_PK;
            TempBatchDtls.MID_SL_NO = MaterialIssue.MaterialIssueRowObj.MID_SL_NO;
            TempBatchDtls.MID_MR_DTL = MaterialIssue.MaterialIssueRowObj.MID_MR_DTL;
            TempBatchDtls.MID_IS_MULTIPLE_BATCH = (TempBatchDtls.SLNO == 0 ? 0 : 1);
            TempBatchDtls.MID_MULT_BTCH_GRP = MaterialIssue.MaterialIssueRowObj.MID_MULT_BTCH_GRP;
            TempBatchDtls.MID_ITEM = MaterialIssue.MaterialIssueRowObj.MID_ITEM;
            TempBatchDtls.MID_UOM = MaterialIssue.MaterialIssueRowObj.MID_UOM;
            TempBatchDtls.ORG_BALANCE_QTY = MaterialIssue.MaterialIssueRowObj.ORG_BALANCE_QTY;
            TempBatchDtls.MRH_NO = MaterialIssue.MaterialIssueRowObj.MRH_NO;
            TempBatchDtls.ITM_NAME = MaterialIssue.MaterialIssueRowObj.ITM_NAME;
            TempBatchDtls.MRD_QTY_APPROVED = MaterialIssue.MaterialIssueRowObj.MRD_QTY_APPROVED;
            TempBatchDtls.MRD_QTY_ISSUED = MaterialIssue.MaterialIssueRowObj.MRD_QTY_ISSUED; //Some doubt
            TempBatchDtls.UOM_CODE = MaterialIssue.MaterialIssueRowObj.UOM_CODE;
            TempBatchDtls.MID_REMARKS = MaterialIssue.MaterialIssueRowObj.MID_REMARKS;
            MaterialIssue.GridList.push(TempBatchDtls);
            if (TempBatchDtls.MID_IS_MULTIPLE_BATCH == 1 && MaterialIssue.GridList.length > 0)
                MaterialIssue.GridList[TempBatchDtls.SLNO - 1].MID_IS_MULTIPLE_BATCH = 1;
        }
        $("#divGridData").data("BatchData", MaterialIssue.GridList);
        GrandGrid.MakeGrid($("#grdBatchDetails"), 0, MaterialIssue.GridList);
        $("[id$=lblTotalIssuingQty]").html(parseFloat(parseFloat($("[id$=lblTotalIssuingQty]").html()) + parseFloat(IssueQty)).toFixed(QtyDec));
        $("[id$=lblQtyBalance]").html(parseFloat(parseFloat($("[id$=lblQtyBalance]").html()) - parseFloat(IssueQty)).toFixed(QtyDec));
        ClearPopupFields();
    }
    return false;
}
function ClearPopupFields() {
    $("[id$=ddlPopUpBatch]").val(0);
    FillPopUpBatchQuantity(0);
    return false;
}
function IsSameBatchExist(batchID) {
    var IsExist = false;
    MaterialIssue.GridList = $("#divGridData").data("BatchData");
    for (var i in MaterialIssue.GridList) {
        if ((MaterialIssue.GridList[i].MID_STK_BATCH == batchID)) {
            IsExist = true;
            break;
        }
    }
    return IsExist;
}
function PopUpGridHandler(tr, command) {
    ///<summary>Grid Handler for grdDispersionDetails Catch all the grid events in this function </summary>
    ///<param "tr">Current row jquery object</param>
    ///<param "command">command to be processed</param> 
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            var DelQty = 0;
            slNO = GrandGrid.Utilities.GetColumnValue(tr, "SLNO", "grdBatchDetails");
            MaterialIssue.GridList = $("#divGridData").data("BatchData");
            var TempSaveList = new Array();
            for (var i in MaterialIssue.GridList) {
                if (MaterialIssue.GridList[i].SLNO != slNO) {
                    TempSaveList.push(jQuery.extend(true, {}, MaterialIssue.GridList[i]));
                }
                else
                    DelQty = MaterialIssue.GridList[i].MID_QTY_ISSUED;
            }
            for (var i in TempSaveList) {
                TempSaveList[i].SLNO = parseInt(i);
            }
            $("#divGridData").data("BatchData", TempSaveList);
            GrandGrid.MakeGrid($("#grdBatchDetails"), 0, TempSaveList);

            //            for (var i in MaterialIssue.GridList) {
            //                if ((MaterialIssue.GridList[i].SLNO == slNO)) {
            //                    DelQty = MaterialIssue.GridList[i].MID_QTY_ISSUED;
            //                    MaterialIssue.GridList.splice(i, 1);
            //                    break;
            //                }
            //            }
            //            for (var i in MaterialIssue.GridList) {
            //                MaterialIssue.GridList[i].SLNO = parseInt(i);
            //            }
            //            $("#divGridData").data("BatchData", MaterialIssue.GridList);
            //            GrandGrid.MakeGrid($("#grdBatchDetails"), 0, MaterialIssue.GridList);
            $("[id$=lblTotalIssuingQty]").html(parseFloat(parseFloat($("[id$=lblTotalIssuingQty]").html()) - parseFloat(DelQty)).toFixed(QtyDec));
            $("[id$=lblQtyBalance]").html(parseFloat(parseFloat($("[id$=lblQtyBalance]").html()) + parseFloat(DelQty)).toFixed(QtyDec));
            return false;
            break;
    }
}
function SortSaveList() {
    MaterialIssue.SaveList = $("#divSaveData").data("SaveData")
    for (var i = 0; i < MaterialIssue.SaveList.length; i++) {
        for (var j = parseInt(i + 1); j < parseInt(MaterialIssue.SaveList.length); j++) {
            if (parseInt(MaterialIssue.SaveList[i].MID_SL_NO) > parseInt(MaterialIssue.SaveList[j].MID_SL_NO)) {
                var templist = jQuery.extend(true, {}, MaterialIssue.SaveList[i])
                MaterialIssue.SaveList[i] = jQuery.extend(true, {}, MaterialIssue.SaveList[j])
                MaterialIssue.SaveList[j] = jQuery.extend(true, {}, templist)
            }
        }
    }
    $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
    return MaterialIssue.SaveList;
}

//To get display list from save list
function GetDisplayGridData() {
    SortSaveList();
    MaterialIssue.SaveList = $("#divSaveData").data("SaveData");
    //Linq used for getting Group values from save list
    var Group = JSLINQ(MaterialIssue.SaveList)
        .Distinct(function (items) { return items.MID_MULT_BTCH_GRP; })
        .Select(function (a) { return a; })
        .OrderBy(function (b) { b.MID_MULT_BTCH_GRP; }).ToArray();
    //Add  items to display list based on group
    MaterialIssue.MIList = new Array();
    for (var j in Group) {
        var insertFlag = false;
        var Total = 0.0;
        var IssuedQty = 0.0;
        var mrdQty = 0.000;
        var IsMultiBatch = -1;
        for (var i in MaterialIssue.SaveList) {
            var TempDispersionDtls = new Object();
            if (MaterialIssue.SaveList[i].MID_MULT_BTCH_GRP == Group[j]) {
                IsMultiBatch++;
                Total = parseFloat(Total) + parseFloat(MaterialIssue.SaveList[i].MID_QTY_ISSUED);
                IssuedQty = parseFloat(MaterialIssue.SaveList[i].MRD_QTY_ISSUED);
                if (insertFlag == false) {
                    MaterialIssue.MIList.push(jQuery.extend(true, {}, MaterialIssue.SaveList[i]));
                    insertFlag = true;
                }
                MaterialIssue.MIList[j].MID_QTY_ISSUED = Total;
                MaterialIssue.MIList[j].MID_IS_MULTIPLE_BATCH = IsMultiBatch > 0 ? 1 : 0;
                //                if (IsMultiBatch > 0 && MaterialIssue.MIList.length > 0)
                //                    MaterialIssue.MIList[j - 1].MID_IS_MULTIPLE_BATCH = MaterialIssue.MIList[j].MID_IS_MULTIPLE_BATCH;
            }
        }
    }
    var srlno = 1;
    for (var i in MaterialIssue.MIList) {
        MaterialIssue.MIList[i].MID_SL_NO = srlno;
        srlno = parseInt(srlno) + 1;
    }
    $("#divData").data("MIData", MaterialIssue.MIList);
}


function FillGRNDetails(tr) {
    //<summary> Function Used to fill PO details edit details </summary>
    IsGridEdit = true;
    IsFillBNo = 1;  //Here no need for calling FillBatchno() from MRItemChange().For avoiding 2 times calling.
    RemoveValidations();
    var grdID = $(tr).parents("table:first").attr("id");
    $("[id$=hdfSlNo]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_SL_NO, $(tr).parents("table:first").attr("id")));
    // $("[id$=SRSEdit]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MRH_NO, grdID));
    var MID_PK = 0;
    MID_PK = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_PK, $(tr).parents("table:first").attr("id"));
    FetchingDistinctMRDetails();
    FillMRNo(MRListDistinct, GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_PO, grdID));
    // $("[id$=SRSItemEdit]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.ITM_NAME, grdID));
    FillMRItems(GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_ITEM, grdID));
    FillBatchNo($("select[id$=ddlMRItem]").val(), GrandGrid.Utilities.GetColumnValue(tr, "MID_STK_BATCH", grdID));
    $("[id$=QtyIssued]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, ""));
    $("[id$=QtyIssuedOrg]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.ORG_BALANCE_QTY, grdID).replace(/[^0-9\.]+/g, ""));
    var uom = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_UOM, grdID);
    FillUOM($("select[id$=ddlMRItem]").val(), GrandGrid.Utilities.GetColumnValue(tr, MaterialIssue.MID_UOM, grdID));
    $("[id$=QTY_IN_STOCK]").text("");
}

function AddMIDetails() {
    //<summary> Function Used to edit the GRN Details </summary>

    RemoveValidations();
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        MaterialIssue.MIList = $("#divData").data("MIData");
        var MIListObj = JSLINQ(MaterialIssue.MIList)
            .Where(function (item) { return item.MID_SL_NO == $("[id$=hdfSlNo]").val(); })
            .FirstOrDefault(null);
        if (MIListObj == null) {//Add new item

            if (!CheckItemBatchnoExists($("select[id$=ddlMRNo]").val(), $("select[id$=ddlMRItem]").val(), $("[id$=MID_BATCH]").val())) {
                GrandScriptUtils.ShowModal(MaterialIssue.RecordExist, MaterialIssue.MessageBoxTitle);
                return false;
            }
            if (!CheckIssueQtyExceeds($("[id$=QtyIssued]").val())) {
                GrandScriptUtils.ShowModal(MaterialIssue.IssueQuantityExceeded, MaterialIssue.MessageBoxTitle);
                return false;
            }
            MaterialIssue.MIObj = new Object();
            MaterialIssue.MIObj.MID_PO = $("select[id$=ddlMRNo]").val();
            MaterialIssue.MIObj.MID_ITEM = $("select[id$=ddlMRItem]").val();
            var maxSlNo = JSLINQ(MaterialIssue.MIList)
                .Max(function (MIitem) { return MIitem.MID_SL_NO; });
            MaterialIssue.MIObj.MID_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
            MaterialIssue.MIObj.MID_PK = 0;
            MaterialIssue.MIObj.MRH_NO = $("[id$=ddlMRNo] :selected").text();
            MaterialIssue.MIObj.ITM_NAME = $("[id$=ddlMRItem] :selected").text();
            MaterialIssue.MIObj.MID_UOM = $("[id$=UOM]").val();
            //For Feching Req. Qty and Qty. Issued aganist this item                
            var QtyIssuedObj = JSLINQ(MaterialIssue.MIList)
                .Where(function (item) { return item.MID_PO == $("select[id$=ddlMRNo]").val() && item.MID_ITEM == $("select[id$=ddlMRItem]").val(); })
                .FirstOrDefault(null);
            if (QtyIssuedObj != null) {
                MaterialIssue.MIObj.MRD_QTY_ISSUED = QtyIssuedObj.MRD_QTY_ISSUED; //Qty. Issued
                MaterialIssue.MIObj.MRD_QTY_APPROVED = QtyIssuedObj.MRD_QTY_APPROVED; //Req. Qty
                MaterialIssue.MIObj.MID_MR_DTL = QtyIssuedObj.MID_MR_DTL;
            }
            MaterialIssue.MIObj.MID_QTY_ISSUED = $("[id$=QtyIssued]").val(); //Issue Qty             
            MaterialIssue.MIObj.UOM_CODE = $("[id$=UOM] :selected").text();
            MaterialIssue.MIObj.MID_REMARKS = "";
            if ($("[id$=MID_BATCH]").val() != 0) {
                MaterialIssue.MIObj.MID_STK_BATCH = $("[id$=MID_BATCH]").val();
                MaterialIssue.MIObj.MID_STK_BATCH_NO = $("[id$=MID_BATCH] option:selected").text();
                MaterialIssue.MIObj.QTY_IN_STOCK = $("[id$=QTY_IN_STOCK]").text().replace(/[^0-9\.]+/g, "");
            }
            MaterialIssue.MIList.push(MaterialIssue.MIObj);
        }
        else { //Update 
            for (var i in MaterialIssue.MIList) {
                if (MaterialIssue.MIList[i].MID_SL_NO == $("[id$=hdfSlNo]").val()) {
                    if (!CheckItemBatchnoExists($("select[id$=ddlMRNo]").val(), $("select[id$=ddlMRItem]").val(), $("[id$=MID_BATCH]").val(), $("[id$=hdfSlNo]").val())) {
                        GrandScriptUtils.ShowModal(MaterialIssue.RecordExist, MaterialIssue.MessageBoxTitle);
                    }
                    if (!CheckIssueQtyExceeds($("[id$=QtyIssued]").val())) {
                        GrandScriptUtils.ShowModal(MaterialIssue.IssueQuantityExceeded, MaterialIssue.MessageBoxTitle);
                        return false;
                    }
                    MaterialIssue.MIList[i].MID_PO = $("select[id$=ddlMRNo]").val();
                    MaterialIssue.MIList[i].MID_ITEM = $("select[id$=ddlMRItem]").val();
                    MaterialIssue.MIList[i].MRH_NO = $("[id$=ddlMRNo] :selected").text();
                    MaterialIssue.MIList[i].ITM_NAME = $("[id$=ddlMRItem] :selected").text();
                    MaterialIssue.MIList[i].MID_QTY_ISSUED = $("[id$=QtyIssued]").val();
                    MaterialIssue.MIList[i].MID_UOM = $("[id$=UOM]").val();
                    MaterialIssue.MIList[i].UOM_CODE = $("[id$=UOM] :selected").text();
                    if ($("[id$=MID_BATCH]").val() != 0) {
                        MaterialIssue.MIList[i].MID_STK_BATCH = $("[id$=MID_BATCH]").val();
                        MaterialIssue.MIList[i].MID_STK_BATCH_NO = $("[id$=MID_BATCH] option:selected").text();
                        MaterialIssue.MIList[i].QTY_IN_STOCK = $("[id$=QTY_IN_STOCK]").text().replace(/[^0-9\.]+/g, "");
                    }

                    break;

                }
            }
        }

        MaterialIssue.ItemPK = 0;
        MaterialIssue.SRSPK = 0;
        $("#divData").data("MIData", MaterialIssue.MIList);
        GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
        AddBatchDropdown();
        ClearDetails();
        EnableDisableDate();
    }
    return false;
}
function ClearPrevSRSAndHeader() {
    $("#divMaterial").hide();
    $("#hSRSList").hide();


    var ajaxUrl = new Array();
    $("#grdPendingSRSList").removeAttr("ajaxurl")
    $("#grdPendingSRSList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPendingSRSList");
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, ajaxUrl);
    EnableDisableDate();
}
function ClearDetails() {
    //<summary> Function Used to clear the GRN Details </summary>

    //$("[id$=SRSEdit]").val("");   
    $("select[id$=ddlMRNo]").val("0");
    FillMRItems(0);
    FillBatchNo(0);
    $("[id$=QTY_IN_STOCK]").text("");
    $("select[id$=ddlMRNo]").val("0");
    $("[id$=SRSItemEdit]").val("");
    $("select[id$=ddlMRItem]").val("0");
    $("[id$=QtyIssued]").val("");
    $("[id$=QtyIssuedOrg]").val("0");
    $("[id$=UOM]").html("");
    $("[id$=hdfSlNo]").val("0");
}

function ViewPreviousSRSDetails() {
    ///<summary>Function used for view the PO Details</summary>

    BindPreviousSRSGrid();
    $("#divPreviousSRS").dialog({ width: 850, height: 450, resizable: false, modal: true });
    $("#divPreviousSRS").dialog("open");
}

function DeleteSRSMaterial() {
    ///<summary>For delete the item,po in the grid - Details</summary>
    CaptureGridChanges();
    MaterialIssue.MIList = $("#divData").data("MIData");
    var TempSaveList = $("#divSaveData").data("SaveData");
    MaterialIssue.SaveList = new Array();
    for (var i in TempSaveList) {
        if (TempSaveList[i].MID_MULT_BTCH_GRP != MaterialIssue.MID_MULT_BTCH_GRP) {
            MaterialIssue.SaveList.push(jQuery.extend(true, {}, TempSaveList[i]));
        }
    }
    $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
    //    for (var i in MaterialIssue.MIList) {
    //        if ((MaterialIssue.MIList[i].MID_ITEM == MaterialIssue.ItemPK) && (MaterialIssue.MIList[i].MID_PO == MaterialIssue.SRSPK)) {

    //            MaterialIssue.MIList.splice(i, 1);
    //            break;
    //        }
    //    }
    GetDisplayGridData();

    MaterialIssue.ItemPK = 0;
    MaterialIssue.SRSPK = 0;
    MaterialIssue.MID_MULT_BTCH_GRP = 0;
    $("#divData").data("MIData", MaterialIssue.MIList);
    GrandGrid.MakeGrid($("#grdPendingSRSList"), 0, MaterialIssue.MIList);
    AddBatchDropdown();
    FetchingDistinctMRDetails(); //setting MRListDistinct array here
    FillMRNo(MRListDistinct, 0);
    EnableDisableDate();
    if (MaterialIssue.MIList.length == 0) {
        $("[id$=MIHDATE]").removeAttr("disabled");
        $("#divMaterial").hide();
        $("#hSRSList").hide();

    }
}

function CheckItemExists(srsPK, itemPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    MaterialIssue.GRNList = $("#divData").data("MIData");
    var flag = true;
    for (var i in MaterialIssue.MIList) {
        if ((MaterialIssue.MIList[i].MID_ITEM == itemPK) && (MaterialIssue.MIList[i].MID_PO == srsPK)) {
            flag = false;
            break;
        }
    }
    return flag;
}

function CheckBatchAdded() {
    //<summary> Function Used to check whether this po item already added. </summary>
    MaterialIssue.GRNList = $("#divData").data("MIData");
    var flag = true;
    for (var i in MaterialIssue.MIList) {
        if (MaterialIssue.MIList[i].MID_STK_BATCH == undefined) {
            flag = false;
            break;
        }
    }
    return flag;
}

function CheckBatchSelected() {
    //<summary> Function Used to check whether Batchno is selected in grid. </summary>   
    var flag = true;
    $("#grdPendingSRSList tr:has(td)").each(function () {
        if ($(this).find("[id$=MID_BATCH]").val() == 0) {
            flag = false;
            return flag;
        }
    });
    return flag;
}


function SavePage(command) {
    //<summary> Function Used to save page </summary>
    $.get(MaterialIssue.InventoryLockCheckingURL + $("[id$=MIHDATE]").val() + "&Module=2", function (data) {
        if (data != null && data.length > 0) {
            if (parseInt(data[0]) == 0) {
                $("#updateProgress").show();
                SaveMaterialIssue(command); //For developer convenience,all codes are just placed under a new function .
            }
            else {
                var Err_TranslockedMsg = MaterialIssue.ErrTransLockedMsg + MaterialIssue.ValueEmpty + data[1];
                GrandScriptUtils.ShowModal(Err_TranslockedMsg, MaterialIssue.MessageBoxTitle);
            }
        }
    });
    return false;
}
function SaveMaterialIssue(command) {
    RemoveValidations();
    AddValidations(2);
    $("[id$=MIH_COMPANY]").attr("disabled", false);
    $("[id$=MIHDATE]").attr("disabled", false);
    if ($(document.forms[0]).valid()) {
        //Showing validation for Future Date selection
        if ($("[id$=hdfIsContFutureDate]").val() != "1") {
            var RetVal = CompareDate($("[id$=MIHDATE]").val(), $("[id$=hdfCurrentDate]").val());
            if (RetVal == 1) {
                //                ShowFutureDate(command);
                blockFutureDate();
                return false;
            }
        }

        if ($("[id$=hdfEnableBatch]").val() == "1") {
            if (!CheckBatchSelected()) {
                GrandScriptUtils.ShowModal(MaterialIssue.MaterialBatchValidation, MaterialIssue.MessageBoxTitle);
                return false;
            }
        }
        CaptureGridChanges();
        if (!CheckStockExists()) {
            GrandScriptUtils.ShowModal(MaterialIssue.NotEnoughStock, MaterialIssue.MessageBoxTitle);
            return false;
        }
        MaterialIssue.MIList = new Array();
        MaterialIssue.MIList = AddToPendingSRSList();
        //Assign Remarks to Save List;
        MaterialIssue.SaveList = $("#divSaveData").data("SaveData");
        var RequstedQty = 0;
        var IssuedQty = 0;
        var hasExcessIssue = false;
        for (var i in MaterialIssue.MIList) {
            for (var j in MaterialIssue.SaveList) {
                if (MaterialIssue.MIList[i].MID_MULT_BTCH_GRP == MaterialIssue.SaveList[j].MID_MULT_BTCH_GRP) {
                    MaterialIssue.SaveList[j].MID_REMARKS = MaterialIssue.MIList[i].MID_REMARKS;
                }
                if (MaterialIssue.SaveList[j].MID_IS_MULTIPLE_BATCH == 0 & MaterialIssue.MIList[i].MID_MULT_BTCH_GRP == MaterialIssue.SaveList[j].MID_MULT_BTCH_GRP) {
                    MaterialIssue.SaveList[j].MID_QTY_ISSUED = MaterialIssue.MIList[i].MID_QTY_ISSUED;
                    MaterialIssue.SaveList[j].MID_STK_BATCH = MaterialIssue.MIList[i].MID_STK_BATCH;
                }
            }
            IssuedQty = parseFloat(MaterialIssue.MIList[i].MID_QTY_ISSUED);
            RequstedQty = parseFloat(MaterialIssue.MIList[i].MRD_QTY_APPROVED);
            if (IssuedQty > RequstedQty)
                hasExcessIssue = true;
        }
        if ($("[id$=hdfMenuType]").val() == 1 && $("[id$=hdfExcessIssueValidated]").val() == 0) {
            if (hasExcessIssue) {
                if ($("[id$=hdfAllowExcessIssue]").val() == 1) {
                    return ShowConfirmAllowExcessIssue(command);
                }
                else {
                    GrandScriptUtils.ShowModal(MaterialIssue.IssueQtyExceeded, MaterialIssue.MessageBoxTitle);
                    return false;
                }
            }
        }
        if (MaterialIssue.MIList.length == 0) {
            GrandScriptUtils.ShowModal(MaterialIssue.AddMaterialIssueDetails, MaterialIssue.MessageBoxTitle);
            return false;
        }
        $("[id$=MIList]").val(JSON.stringify(MaterialIssue.SaveList));
        $("[id$=WKF_FLAG]").val("0");
        if (command != "Draft") {
            $("[id$=ActionID]").val("1"); // save and doworkflow.
            $("[id$=WKF_FLAG]").val("1");
            $("[id$=WKF_TRX_FLAG]").val("1");

            //New Workflow Parameters
            $("[id$=WKF_REFERENCE]").val($("[id$=ReferenceID]").val());
            // $("[id$=WKF_TASK_ACTION]").val($("[id$=WRKFACT_ID]").val());
            //$("[id$=WKF_TASK_ACTION]").val(3795); Admin task action ID, for testing 
            $("[id$=WKF_TASK_ACTION]").val($("select[id$=WRKFACT_ID]").val());
            $("[id$=WKF_APPLICATION]").val($("[id$=hdfAppID]").val());
            $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
            $("[id$=WKF_TASK]").val($("[id$=TaskPK]").val());
            $("[id$=WKF_COMMENTS]").val($("[id$=WrkfComments]").val());
            $("[id$=USER_PK]").val($("[id$=UserPk]").val());
            //End NewWorkflow Parameters
        }
        else {
            $("[id$=ActionID]").val('0');  // save only.
            $("[id$=WKF_TRX_FLAG]").val("0");
            $("[id$=WKF_FLAG]").val("0");
        }
        $("select[id$=MIH_DEPT]").removeAttr("disabled");
        if ($("[id$=hdfMenuType]").val() == "1")
            $("[id$=MIH_IS_WORK_ORDER]").val("1");
        else
            $("[id$=MIH_IS_WORK_ORDER]").val("0");

        var jSonString = GrandScriptUtils.FormToJsonString(false);

        //To Prevent Duplicate Submission
        if ($("[id$=SubmitFlag]").val() == "0")
            $("[id$=SubmitFlag]").val('1')
        else
            return false;

        $.post(MaterialIssue.SaveMaterialIssue, jSonString, function (data) {
            if (parseInt(data[0]) > 0) {

                if (command == "Draft") {
                    if ($("[id$=hdfMenuType]").val() == "1")
                        var SaveMessageWithPRNo = MaterialIssue.MIWOSavedMessage;
                    else
                        var SaveMessageWithPRNo = MaterialIssue.MISavedMessage;

                    if ($("[id$=AST_DOC_MODE]").val() == "1") {
                        if ($("[id$=hdfMenuType]").val() == "1") {
                          SaveMessageWithPRNo = MaterialIssue.SaveMessage3 + " " + data[1] + " " + MaterialIssue.SaveMessage2;
                        }
                        else {
                            SaveMessageWithPRNo = MaterialIssue.SaveMessage1 + " " + data[1] + " " + MaterialIssue.SaveMessage2;
                        }
                    }
                    GrandScriptUtils.ShowModal(SaveMessageWithPRNo, MaterialIssue.MessageBoxTitle, MaterialIssue.SAVE);
                }
                else {
                    $("[id$=hdfAppID]").val(data[0]);
                    $("[id$=AppNo]").val(data[1]);
                    //  SaveWorkFlow();
                    ShowWorkflowSaveMsg();
                }
            }
            else if (parseInt(data[0]) == -1) {
                GrandScriptUtils.ShowModal(MaterialIssue.ActionFailedMessage, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -2) {
                if ($("[id$=hdfMenuType]").val() == "1") {
                    GrandScriptUtils.ShowModal(MaterialIssue.SaveMessage3 + " " + data[1] + " " + MaterialIssue.EditUsedByAnotherUser, MaterialIssue.MessageBoxTitle, MaterialIssue.SAVE);

                }
                else {
                    GrandScriptUtils.ShowModal(MaterialIssue.SaveMessage1 + " " + data[1] + " " + MaterialIssue.EditUsedByAnotherUser, MaterialIssue.MessageBoxTitle, MaterialIssue.SAVE);

                }
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -3) {
                if ($("[id$=hdfMenuType]").val() == "1") {
                    GrandScriptUtils.ShowModal(MaterialIssue.MIqtyGreaterThanMRqtyWO, MaterialIssue.MessageBoxTitle);

                }
                else {
                    GrandScriptUtils.ShowModal(MaterialIssue.MIqtyGreaterThanMRqty, MaterialIssue.MessageBoxTitle);

                }
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -4) {
                var arr = data[1].split(",");
                var s = String.format(MaterialIssue.SpecifiedQuantityNotAvailableInStock, arr[0], arr[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -10) {
                fnConfirmStockValueChange(command);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -31) {
                if ($("[id$=hdfMenuType]").val() == "1") {
                    var s = String.format(MaterialIssue.MaterialIssueAgainstWOAlreadyDone, $("[id$=MIHDATE]").val());
                }
                else {

                    var s = String.format(MaterialIssue.StockTransferAlreadyDone, $("[id$=MIHDATE]").val());

                }
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -32) {
                var s = String.format(MaterialIssue.StockAdjustmentIsAlreadyDone, $("[id$=MIHDATE]").val());
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -41) {//Batch should be in FIFO order of Batch
                var s = String.format(MaterialIssue.StockBatchFIFOMsgwithMaterial.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -81) {//Voucher not generatted for Imported Item
                var s = String.format(MaterialIssue.VoucherNotGeneratted.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -45) {//Unable to modify,material accept entry exists
                var s = String.format(MaterialIssue.UnableToModify_MA_Exist.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -46) {//Cannot  modify ,as it is referenced in some other forms             
                var s = String.format(MaterialIssue.MsgSaveRefErrorWithTransNo.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -50) {//Cannot  modify ,GRN Exist
                var s = String.format(MaterialIssue.UnableToModify_GRNExist.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -71) {//Allocation or Matrerial return exist
                var s = String.format(MaterialIssue.UnableToModify_AllocationOrReturnExist.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data[0]) == -72) {//MI Date less than WO Date
                var s = String.format(MaterialIssue.MIDateLessThanWO.fontcolor("red"), data[1]);
                GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
        });
    }
    EnableDisableDate();
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var SaveMessageWithPRNo = "";
    if ($("[id$=hdfMenuType]").val() == "1")
        SaveMessageWithPRNo = MaterialIssue.SaveMessage3 + " " + $("[id$=AppNo]").val() + " " + MaterialIssue.SaveMessage2;
    else
        SaveMessageWithPRNo = MaterialIssue.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + MaterialIssue.SaveMessage2;
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        GrandScriptUtils.ShowModal(SaveMessageWithPRNo, MaterialIssue.MessageBoxTitle, MaterialIssue.INBOX);
    }
    else {
        GrandScriptUtils.ShowModal(SaveMessageWithPRNo, MaterialIssue.MessageBoxTitle, MaterialIssue.SAVE);
    }
}

function AddToPendingSRSList() {
    ///<summary>Function used to add the needed po material list</summary> 

    var grdID;
    var balColIndex = 0;
    var remkColIndex = 0;
    var isValid = true;
    if ($("#grdPendingSRSList").css("visibility") == "visible") {

        $("#grdPendingSRSList tr:has(td)").each(function () {
            grdID = $(this).parents("table:first").attr("id");
            MaterialIssue.MIObj = new Object();
            MaterialIssue.MIObj.MID_SL_NO = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_SL_NO, grdID);
            MaterialIssue.MIObj.MID_MR_DTL = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_MR_DTL, grdID);
            MaterialIssue.MIObj.MID_PK = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_PK, grdID);
            MaterialIssue.MIObj.MID_PO = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_PO, grdID);
            if ($(this).find("[id$=MID_BATCH]").val() != 0) {
                MaterialIssue.MIObj.MID_STK_BATCH = $(this).find("[id$=MID_BATCH]").val();
            }
            MaterialIssue.MIObj.MID_IS_MULTIPLE_BATCH = GrandGrid.Utilities.GetColumnValue($(this), "MID_IS_MULTIPLE_BATCH", grdID);
            MaterialIssue.MIObj.MID_MULT_BTCH_GRP = GrandGrid.Utilities.GetColumnValue($(this), "MID_MULT_BTCH_GRP", grdID);

            MaterialIssue.MIObj.MID_ITEM = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_ITEM, grdID);
            MaterialIssue.MIObj.MID_UOM = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_UOM, grdID);
            MaterialIssue.MIObj.MRH_NO = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRH_NO, grdID);
            MaterialIssue.MIObj.ITM_NAME = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.ITM_NAME, grdID);
            MaterialIssue.MIObj.MRD_QTY_APPROVED = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_QTY_APPROVED, grdID).replace(/[^0-9\.]+/g, "");
            MaterialIssue.MIObj.MRD_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, "");
            MaterialIssue.MIObj.MID_QTY_ISSUED = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_QTY_ISSUED, grdID).replace(/[^0-9\.]+/g, "");
            MaterialIssue.MIObj.ORG_BALANCE_QTY = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.ORG_BALANCE_QTY, grdID).replace(/[^0-9\.]+/g, "");
            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialIssue.MID_REMARKS, grdID);
            MaterialIssue.MIObj.MID_REMARKS = $(this).find("td:eq(" + remkColIndex + ") input[type=text]").val();
            var IssuedQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", grdID);
            var IssuedQty = $(this).find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
            MaterialIssue.MIObj.MID_QTY_ISSUED = IssuedQty
            MaterialIssue.MIList.push(MaterialIssue.MIObj);
        });
    }
    return MaterialIssue.MIList;
}

function SetCommentList(poPK, itemPK, index) {

    MaterialIssue.MIList = $("#divData").data("MIData");
    for (var i in MaterialIssue.MIList) {
        if (MaterialIssue.MIList[i].MID_PO == poPK && MaterialIssue.MIList[i].MID_ITEM == itemPK) {
            MaterialIssue.MIList[i].MID_REMARKS = $("#txtRemarks_" + index).val();
            break;
        }
    }
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdPendingSRSList") {
        var balColIndex = 0;
        var balance = "";
        var txtBoxBalance;
        var remkColIndex = 0;
        var remarks = "";
        var txtBoxRemarks;
        var mhnoColIndex = 0;
        var mhno = "";
        var issuedColIndex = 0;
        var issuedQty = "";
        var poPK = 0;
        var itemPK = 0;
        var ColQty = 0;
        var ColQtyIndx = 0;
        var batchColIndex = 0;
        var batchValue = "";
        var QtyIssued = 0;
        var IssueQty = 0;
        var MID_PK = 0;
        var ImgIndex = 0;
        $("#grdPendingSRSList tr:has(td)").each(function (index) {
            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialIssue.MID_REMARKS, grdID);
            remarks = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_REMARKS, grdID);
            poPK = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_PO, grdID);
            itemPK = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_ITEM, grdID);
            MID_PK = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MID_PK, grdID);
            if (remkColIndex != 0) {
                $(this).find("td:eq(" + remkColIndex + ")").html("");
                if (remarks == "null") {
                    remarks = "";
                }
                $(this).find("td:eq(" + remkColIndex + ")").append("<input id=\"txtRemarks_" + index + "\" type=\"text\" value=\"" + remarks + "\" onblur=\"javascript:SetCommentList('" + poPK + "','" + itemPK + "','" + index + "');\" width=\"80%\" tabindex=\"8\" />");
            }
            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_APPROVED", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_APPROVED", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }

            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "MID_QTY_ISSUED", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
                IssueQty = parseFloat(ColQty).toFixed(QtyDec);
            }

            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_ISSUED", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_ISSUED", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }
            batchValue = GrandGrid.Utilities.GetColumnValue($(this), "MID_STK_BATCH_NO", grdID);
            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_STK_BATCH_NO", grdID);
            if (batchColIndex != null) {
                if (batchValue == "undefined")
                    $(this).find("td:eq(" + batchColIndex + ")").html("");
            }
            //Current stock Qty
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "QTY_IN_STOCK", grdID);
            quantity = GrandGrid.Utilities.GetColumnValue($(this), "QTY_IN_STOCK", grdID);
            if ($("[id$=hdfEnableBatch]").val() == "0") {
                quantity = GetCurrentStock(itemPK);
            }
            if (!isNaN(quantity) && quantity != "") {
                quantity = parseFloat(quantity).toFixed(QtyDec);
            }
            // $(this).find("td:eq(" + qtyIndex + ")").attr('style', 'text-align: right');
            if (quantity == "undefined" || quantity == "null" || quantity == "" || quantity == undefined) {
                $(this).find("td:eq(" + qtyIndex + ")").html("");
            }
            else {
                $(this).find("td:eq(" + qtyIndex + ")").html(numberWithCommas(parseFloat(quantity).toFixed(QtyDec)));
            }

            ImgIndex = GrandGrid.Utilities.GetColumnIndex($(this), "IMG", grdID);
            $(this).find("td:eq(" + ImgIndex + ")").html("")
            if ($("[id$=hdfEnableBatch]").val() == "1") {
                if (MaterialIssue.IsViewMode) {
                    $(this).find("td:eq(" + ImgIndex + ")").append("<input type=\"image\" id=\"imgViewBatch_" + index.toString() + "\" style=\"margin-left:8px;\" src=\"../images/Classic/Icons/viewbtn-grid.png\" title=\"View Batches\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'AddMultipleBatch');\" />");
                    //                    $("#imgViewBatch_" + (parseInt(index))).hide();
                }
                else {
                    $(this).find("td:eq(" + ImgIndex + ")").append("<input type=\"image\" id=\"imgAddBatch_" + index.toString() + "\" style=\"margin-left:8px;\" src=\"../images/Classic/Icons/addbtn-grid.png\" title=\"Add Batches\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'AddMultipleBatch');\" />");
                    $(this).find("td:eq(" + ImgIndex + ")").append("<input type=\"image\" id=\"imgClearBatch_" + index.toString() + "\" style=\"margin-left:8px;\" src=\"../images/Classic/Icons/clearbtn-grid.png\" title=\"Clear Batches\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'ClearMultipleBatch');\" />");
                    //                                        $("#imgAddBatch_" + (parseInt(index))).hide();
                    //                                        $("#imgClearBatch_" + (parseInt(index))).hide();
                }
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                var issueQty = GrandGrid.Utilities.GetColumnValue($(this), "MID_QTY_ISSUED", $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");;
                issueQty = (issueQty == "null") ? "" : parseFloat(issueQty).toFixed(QtyDec);
                if (!MaterialIssue.IsViewMode) {
                    $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" id=\"MID_QTY_ISSUED_" + index.toString() + "\" value=\"" + issueQty + "\" maxLength =\"30\" tabindex=\"8\"  class=\"numeric input-w70 \"  ></input>");
                }

            }

        });


        var queryStr = window.location.search.substring(1);
        if (queryStr != "") {
            var queryStr = queryStr.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                    $("#grdPendingSRSList th:last").hide();
                    $("#grdPendingSRSList tr:has(td)").each(function (index) {
                        $(this).find("td:last").hide();
                        $(this).find("td:last").hide();
                        $("select[id$=MIH_DEPT]").attr("disabled", true);
                        $("[id$=MIHDATE]").attr("disabled", true);
                        $("select[id$=MIH_DEPT_TO]").attr("disabled", true);
                        $("select[id$=SearchType]").attr("disabled", true);

                        $("[id$=AddToList]").hide();
                        $("[id$=imbSelect]").hide();
                    });
                }
            }
        }
    }
    if (grdID == "grdBatchDetails") {

        $("#grdBatchDetails tr:has(td)").each(function (index) {

            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "QTY_IN_STOCK", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "QTY_IN_STOCK", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }
            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "MID_QTY_ISSUED", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }

        });
    }
    //MRH_PK
    if (grdID == "grdSRSList") {
        if (MaterialIssue.IsViewMode == false)
            $("[id$=AddToList]").show();
        $("#grdSRSList tr:has(td)").each(function (index) {
            issuedColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialIssue.MRD_QTY_ISSUED, grdID);
            issuedQty = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssue.MRD_QTY_ISSUED, grdID);
            if (issuedColIndex != null) {
                if (parseFloat(issuedQty) > 0) {
                    $(this).find("td:eq(" + issuedColIndex + ")").html("");
                    $(this).find("td:eq(" + issuedColIndex + ")").html("<a style=\"cursor:pointer;float: right;\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'PREVIOUSSRSVIEW');\">" + issuedQty + "</a>");
                }
            }
            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_APPROVED", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_APPROVED", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }
            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_ISSUED", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_ISSUED", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }
            ColQty = GrandGrid.Utilities.GetColumnValue($(this), "BALANCE_QTY", grdID);
            ColQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE_QTY", grdID);
            if (ColQtyIndx != null) {
                $(this).find("td:eq(" + ColQtyIndx + ")").html(numberWithCommas(parseFloat(ColQty).toFixed(QtyDec)));
            }

        });
    }
}

function SetSearchType(isLoad) {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(" ");
    if (strname == "0") {
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        ClearSearchDetails();
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

    $("[id$=SearchValue]").val(MaterialIssue.TEXTEMPTY);
    $("[id$=FromDate]").val(MaterialIssue.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(MaterialIssue.TEXTEMPTY);
    $("[id$=ToDate]").val(MaterialIssue.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(MaterialIssue.TEXTEMPTY);
}
function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    window.location = MaterialIssue.MIListUrl;
    return false;
}



///#endregion

//#region----------- Validation Section----------------

function AddValidations(mode) {
    //<summary>Function used to assign validation</summary>

    if (mode == 1) {
        $("[id$=UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectUOM)" }
        });
        if ($("[id$=hdfEnableBatch]").val() == "1") {
            $("[id$=MID_BATCH]").rules("add", {
                selectNone: true,
                messages: { selectNone: MaterialIssue.MaterialBatchValidation }
            });
        }
        if ($("[id$=QTY_IN_STOCK]").text() != "")
            $("[id$=QtyIssued]").rules("add", {
                required: true,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                // max: $("[id$=POD_CONV_FACT]").val() * $("[id$=QtyIssuedOrg]").val(),
                max: $("[id$=QTY_IN_STOCK]").text().replace(/[^0-9\.]+/g, ""),
                messages: { max: MaterialIssue.MIQtyBtch, required: "Translate(EnterQty)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
            });
        else
            $("[id$=QtyIssued]").rules("add", {
                required: true,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                messages: { required: "Translate(EnterQty)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
            });
    }
    if (mode == 2) {
        $("[id$=MIH_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseselectaStore)" }
        });
        $("[id$=MIH_DEPT_TO]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseselectaDepartement)" }
        });
        $("[id$=MIHDATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: MaterialIssue.EnterDate }
        });

    }
    if (mode == 3) {
        $("[id$=ddlPopUpBatch]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialIssue.MaterialBatchValidation }
        });

        $("[id$=txtQtyPopUp]").rules("add", {
            required: true,
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            messages: { required: "Translate(EnterQty)" }
        });
    }
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
    $("[id$=UOM]").rules("remove");
    $("[id$=QtyIssued]").rules("remove");
    //    $("input[id$=txtQtyPopUp]").rules("remove");
    //    $("select[id$=ddlPopUpBatch]").rules("remove");
    $("[id$=MIH_DEPT_TO]").rules("remove");
    $("[id$=MID_BATCH]").rules("remove");
}

//#endregion
function AddDropdownTooltip(drpBatchID) {
    jQuery('#' + drpBatchID).hover(function (e) {
        var tipX = e.pageX + 12;
        var tipY = e.pageY + 12;
        jQuery("body").append("<div id='myTooltip' class='dropdownTooltip' style='position: absolute; z-index: 100; display: none;'>" + jQuery("OPTION:selected", this).text() + "</div>");
        if (jQuery.browser.msie) var tipWidth = jQuery("#myTooltip").outerWidth(true)
        else var tipWidth = jQuery("#myTooltip").width()
        jQuery("#myTooltip").width(tipWidth);
        jQuery("#myTooltip").css("left", tipX).css("top", tipY).fadeIn("medium");
    }, function () {
        jQuery("#myTooltip").remove();
    }).mousemove(function (e) {
        var tipX = e.pageX + 12;
        var tipY = e.pageY + 12;
        var tipWidth = jQuery("#myTooltip").outerWidth(true);
        var tipHeight = jQuery("#myTooltip").outerHeight(true);
        if (tipX + tipWidth > jQuery(window).scrollLeft() + jQuery(window).width()) tipX = e.pageX - tipWidth;
        if (jQuery(window).height() + jQuery(window).scrollTop() < tipY + tipHeight) tipY = e.pageY - tipHeight;
        jQuery("#myTooltip").css("left", tipX).css("top", tipY).fadeIn("medium");
    });
}
function CheckStockExists() {
    ///<summary>Function used to check whether each item have enough stock </summary>
    var stockExcists = true;
    var materialList = $("#divData").data("MIData");

    var quantity = 0;
    for (var i in materialList) {
        $("#" + (parseInt(i)).toString() + "_MID_BATCH").removeAttr("style");
        if (parseFloat(materialList[i].MID_QTY_ISSUED) > parseFloat(materialList[i].QTY_IN_STOCK).toFixed(QtyDec) && materialList[i].MID_IS_MULTIPLE_BATCH == 0) {
            $("#" + (parseInt(i)).toString() + "_MID_BATCH").attr('style', 'border:1px solid red !important;');
            stockExcists = false;
        }
    }
    return stockExcists;
}
function CaptureGridChanges() {
    ///<summary>Function used to fetch grid data and update this data to list </summary>
    var colIndex = 0;
    var ColValue = "";
    MaterialIssue.MIList = $("#divData").data("MIData");
    $("#grdPendingSRSList tr:has(td)").each(function (index) {
        if (index >= 0) {
            if (MaterialIssue.MIList.length > 0) {
                var material = MaterialIssue.MIList[index];
                material.MID_STK_BATCH = $(this).find("[id$=MID_BATCH]").val();

                var remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialIssue.MID_REMARKS, "grdPendingSRSList");
                material.MID_REMARKS = $(this).find("td:eq(" + remkColIndex + ") input[type=text]").val();
                var IssuedQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MID_QTY_ISSUED", "grdPendingSRSList");
                var IssuedQty = $(this).find("td:eq(" + IssuedQtyIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
                //   material.MID_IS_MULTIPLE_BATCH = GrandGrid.Utilities.GetColumnValue($(this), "MID_IS_MULTIPLE_BATCH", "grdPendingSRSList");
                material.MID_MULT_BTCH_GRP = GrandGrid.Utilities.GetColumnValue($(this), "MID_MULT_BTCH_GRP", "grdPendingSRSList");
                material.MID_QTY_ISSUED = IssuedQty;
            }
        }
    });
    $("#divData").data("MIData", MaterialIssue.MIList);
    //update changes to save list
    MaterialIssue.SaveList = $("#divSaveData").data("SaveData");
    for (var i in MaterialIssue.MIList) {
        for (var j in MaterialIssue.SaveList) {
            if (MaterialIssue.MIList[i].MID_MULT_BTCH_GRP == MaterialIssue.SaveList[j].MID_MULT_BTCH_GRP) {
                MaterialIssue.SaveList[j].MID_REMARKS = MaterialIssue.MIList[i].MID_REMARKS;
            }
            if (MaterialIssue.SaveList[j].MID_IS_MULTIPLE_BATCH == 0 & MaterialIssue.MIList[i].MID_MULT_BTCH_GRP == MaterialIssue.SaveList[j].MID_MULT_BTCH_GRP) {
                MaterialIssue.SaveList[j].MID_QTY_ISSUED = MaterialIssue.MIList[i].MID_QTY_ISSUED;
                MaterialIssue.SaveList[j].MID_STK_BATCH = MaterialIssue.MIList[i].MID_STK_BATCH;
            }
        }
    }
    $("#divSaveData").data("SaveData", MaterialIssue.SaveList);
}
////Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

function FillPopUpBatchQuantity(batchID) {

    $.getJSON(MaterialIssue.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + (batchID == 0 ? $("[id$=ddlPopUpBatch] option:selected").val() : batchID), function (data) {
        if (data != null) {
            //            $("[id$=txtQtyPopUp]").val(parseFloat(data[0].SBD_QTY_IN_STOCK).toFixed(QtyDec));
            $("[id$=lblStockPopUp]").html(parseFloat(data[0].SBD_QTY_IN_STOCK).toFixed(QtyDec));
            var balance = parseFloat($("[id$=lblQtyBalance]").html().replace(/[^0-9\.]+/g, ""));
            var qtyInStock = parseFloat(data[0].SBD_QTY_IN_STOCK).toFixed(QtyDec);
            $("[id$=txtQtyPopUp]").val(parseFloat(balance) > parseFloat(qtyInStock) ? parseFloat(qtyInStock).toFixed(QtyDec) : parseFloat(balance).toFixed(QtyDec));
        }
        else {
            $("[id$=txtQtyPopUp]").val("");
            $("[id$=lblStockPopUp]").html("");
        }
    });
}

//For checking selected date is a future date or not
//command=>Draft,SaveandSubmit
function ShowFutureDate(command) {

    var msgTitle;
    var msg;
    msgTitle = MaterialIssue.MessageBoxTitle;
    msg = MaterialIssue.ContFutureDateMsg;
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

function ShowConfirmAllowExcessIssue(command) {

    var msgTitle;
    var msg;
    msgTitle = MaterialIssue.MessageBoxTitle;
    msg = MaterialIssue.AllowExcessIssue;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfExcessIssueValidated]").val(1);
                $(this).dialog("close");
                SaveMaterialIssue(command);
            },
            Cancel: function (e) {
                $("[id$=hdfExcessIssueValidated]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

function GetCurrentStock(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the material</summary>
    /// <param name="materialID"  type="Object">
    /// </param>
    var store = $("select[id$=MIH_DEPT]").val();
    if (store == null) { store = MIHdeptPK; }
    var date = $("[id$=MIHDATE]").val();
    jQuery.ajaxSetup({ async: false });
    var qty = 0;
    $.get(MaterialIssue.GetCurrentStock + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&Store=" + store + "&Date=" + date, function (data) {
        if (data) {
            qty = parseFloat(data[0].STD_QTY_IN_STOCK).toFixed(QtyDec);
        }
    });

    return qty;
}

function blockFutureDate() {
    GrandScriptUtils.ShowModal(MaterialIssue.Err_FutureDateTransactionNotAllowed, MaterialIssue.MessageBoxTitle);
    return false;
}
function EnableDisableDate() {
    var rowscount = $("#grdPendingSRSList tbody tr").length;
    if (rowscount > 0)
        $("[id$=MIHDATE]").attr("disabled", "disabled");
    else
        $("[id$=MIHDATE]").removeAttr("disabled");
}