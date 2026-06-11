/// <reference path="../../GrandScriptUtils.js" />

//#region----------- Global Variable----------------
//###
var adnlItemSlno = 0;
var pohDefaultCompany = 0;

//#endregion

//#region----------- Configuration Section----------------

var StockTransfer = {
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    StockTransferList: new Array(),
    StockTransferDtlsObj: new Array(),
    // For GIN List
    GINList: new Array(),
    // For PO List
    POList: new Array(),
    // For PR List
    PRList: new Array(),
    // For Additional Item List
    //AdditionalItemList: new Array(),
    // For GIN Pk List
    GINPKList: new Array(),
    GINListTemp: new Array(),
    GINPKListObj: new Object(),
    // For PO Pk List
    POPKList: new Array(),
    POPKListObj: new Object(),
    // For Total Inspected Item Details From GIN List
    TotalItem: new Array(),
    TotalItemObj: new Object(),
    // For Additonal Item Details From PO List
    AdditionalItem: new Array(),
    AdditionalItemObj: new Object(),
    // for Allocation item Details From PO List
    AllocationItem: new Array(),
    AllocationItemObj: new Object(),
    AdditionalAllocationObj: new Object(),
    AllocatedAdditionalList: new Array(),
    AllocationItemPR: new Array(),
    AllocationItemPRObj: new Object(),
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",

    ValueOne: '1',
    ValueZero: '0',
    ValueEmpty: ' ',

    LOGOUT: "LOGOUT",
    StockTrasnferDate: "SFH_DATE",
    GINDetailsPK: "GID_PK",
    GINItem: "GID_ITEM",
    GINQtyApproved: "GID_QTY_APPROVED",
    PODetailsPk: "POD_PK",
    AdditionalDept: "PRD_DEPT",
    AdditionalItem: "POR_ITEM",
    PorItem: "POR_ITEM",
    AdditionalQty: "ALLOCATE_ADDL_PR_QTY",
    AdditionalUOM: "POR_UOM",
    AdditionalUOMName: "POR_UOM_NAME",
    StockTranferDtlsPk: "SFD_PK",
    StockTranferDtlsSLNo: "SFD_SL_NO",
    AllocatePRQty: "ALLOCATE_PR_QTY",
    AllocatePOQty: "ALLOCATE_PO_QTY",
    AllocatePOAddnlQty: "ALLOCATE_PO_ADDL_QTY",
    BalanceAddlQty: "BAL_PO_ADDL_QTY",
    PRHeaderNo: "PRH_NO",
    PRDeptName: "PRD_DEPT_NAME",
    AllocatePRLoc: "SFD_LOCATION",
    BalancePoQty: "BAL_PO_QTY",
    PorQtyBalance: "POR_QTY_BAL",
    STNO: "SFD_NO",
    QALotNo: "SFD_QA_LOT_NO",
    POD_RATE: "POD_RATE",


    Save: "save",
    Draft: "Draft",
    Delete: "delete",
    Edit: "edit",
    Status: "Status",
    Flag: "Flag",
    Pk: "PK",
    RefId: "RefID",
    InventoryLocationGet: "StoreManagement.do?Action=GetInventoryLocation&AUTOSEARCH=1&DeptPK=",

    ItemAlreadyAddedMsg: "Translate(Itemsalreadyaddedbyanotheruser)",
    SaveMsg1: "Translate(StockTransferSaveMsg1)",
    SaveMsg2: "Translate(StockTransferSaveMsg2)",
    SubmitMessage: "Translate(SubmittedMsg)",
    STSavedMessage: "Translate(StockTransferSavedMessage)",
    ActionFailedTryAgainMsg: "Translate(ActionFailedPleaseTryAgain)",
    DoyouWanttoDelete: "Translate(Doyouwanttodeletethisdetails)",
    DefaultActionNeedtoPerform: "Translate(DefaultActionneedstobeperformed)",
    InformationTitle: "Translate(Information)",
    ConfirmationTitle: "Translate(Conformation)",
    SelectDetails: "Translate(SelectDetailstoadd)",
    SelectPODetails: "Translate(SelectPODetailstoadd)",
    SelectGINDetails: "Translate(SelectGINDetailstoadd)",
    SameLotNumbrNotAllowed: "Translate(SameLotNumbrNotAllowed)",
    ValidationMsg_EnterDate: "Translate(EnterDate)",
    ValidationMsg_SelectTrasnferFrom: "Translate(SelectTransferfrom)",
    ValidationMsg_SelectItem: "Translate(SelectItem)",
    ValidationMsg_SelectStore: "Translate(SelectStore)",
    ValidationMsg_EnterAllocQty: "Translate(EnterAllocateQty)",
    InformationTtile: "Translate(Information)",
    CannotReceivePriorDateSendReceive: "Translate(CannotReceivePriorDateSendReceive)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    Err_FutureDateTransactionNotAllowed: "Translate(Err_FutureDateTransactionNotAllowed)",
    MessageBoxTitle: "Translate(Information)",
    ContDuplicateLotnoMsg: "Translate(ContDuplicateLotnoMsg)",
    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)",
    MsgSameItemDiffRate: "Translate(MsgSameItemDiffRate)",
    SpecifiedQuantityNotAvailableInStock: "Translate(SpecifiedQuantityNotAvailableInStock)",

    AllocationErrorMsg: "Translate(PleaseAllocateQuantity)",
    AllocationMisMatchErrorMsg: "Translate(AdditionalQuantityAllocationInvalidPleaseallocateFullAdditionalQuantity)",
    InvalidAllocationInPRMsg: "Translate(QuantityAllocationInvalidInPRDetailsPleasereallocate)",
    PRAllocationWithPOBalanceErrorMsg: "Translate(PRAllocationInvalidAllocationQuantityMustLessThanOrEqualToPOBalanceQuantity)",
    AllocationMappingError: "Translate(AllocationMappingMismatch)",
    AllocationMappingErrorForFullQty: "Translate(QuantityAllocationInvalidInPODetailsMustAllocateFullQuantityInPODetails)",
    ReallocateErrorMsg: "Translate(QuantityAllocationInvalidInPODetailsPleaseReallocate)",
    StockAdjustmentIsAlreadyDone: "Translate(StockAdjustmentIsAlreadyDone)",
    StockTransferAlreadyDone: "Translate(StockTransferAlreadyDone)",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    ErrModifyStockAdmission: "Translate(ErrModifyStockAdmission)",
    SaveSuccessUrl: "StockTransferList.aspx",
    FillGRNStoreUrl: "StoreRequisitionSlip.do?Action=GetStoresByType&SBUPk=",
    GINDetailsURL: "StockTransfer.do?Action=GetGINDtls&Store=",
    FillPoDetailsURL: "StockTransfer.do?Action=GetAllocatedAndTransferDetails&Type=0",
    FillPRDetailsURL: "StockTransfer.do?Action=GetAllocatedAndTransferDetails&Type=1",
    StockTransferListUrl: "../StoreManagement/StockTransferList.aspx",
    SaveStockTransferUrl: "StockTransfer.do?Action=SavePage",
    GRNStoreParams: "&Flag=1" + "&DeptType=4",
    AdditionalItemUrl: "StockTransfer.do?Action=GetAllocatedAndTransferDetails&Type=1&ItemList=1",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",

    //============
    INBOX: "INBOX",
    EMPTYVALUE: "",
    MINUSONEVALUE: "-1",
    ANDVALUE: "&",
    EQUALVALUE: "=",
    POITEM: "POR_ITEM",
    PRADDNLQTY: "ALLOCATE_ADDL_PR_QTY",
    DOTVALUE: ".",
    InventoryLocationStore: "17",
    InventoryDeptType: "2"
}

var QtyDec, AmtDec, RateDec;

//#endregion

//#region----------- Initilization Section----------------
//###
$(document).ready(function () {
    ///<summary>Document . Rerady ()</summary>
    //Set Decimal Points For Qty and Amount
    $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimalDigitP2P']").val();
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    // Initilize Page Values
    PageInit();

});
//###
function PageInit() {

    ///<summary>initial page condition</summary>
    $("[id$=ConfirmStockValueChange]").val('0');
    // Add Date Controls
    DateInit();
    Popup();
    // Get StockTransfer JSON Value from Hiddenfield
    var StockTransferObj = $.parseJSON($("[id$=StockTransferList]").val());
    $("[id$=AddPRDtlsToList]").hide();
    //$("#divSearch").hide();
    $("#searchwrap").hide();
    // Get Querystring from URL
    var queryStr = window.location.search.substring(1);
    if (queryStr != StockTransfer.EMPTYVALUE) {
        var queryStr = queryStr.split(StockTransfer.ANDVALUE)
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if (pK[1] != "" && pK[0] == StockTransfer.Status) {
                //Check the query string Name status if status as 1 thn its in view mode
                $("[id$=ViewStatus]").val(StockTransfer.ValueOne);
                FillDetails(StockTransferObj, 1);
            }
            if (pK[1] != "" && pK[0] == StockTransfer.Flag) {
                //Check the query string Name status if status as 1 thn its in view mode
                $("[id$=ViewStatus]").val(StockTransfer.ValueOne);
                FillDetails(StockTransferObj, 1);
            }
            if (pK[1] == 1 && pK[0] == "IsModify") {
                $("[id$=SFH_IS_EDIT]").val("1");
            }
            //23-01-2014
            if (pK[0] == "PRefID") {
                if (parseInt($("[id$=hdfTransactionPK]").val()) > 0) {
                    // $("[id$=ViewStatus]").val(StockTransfer.ValueOne);
                    FillDetails(StockTransferObj, 1);
                }
                else {
                    StockTransfer.StockTransferList = StockTransferObj;
                    StockTransfer.GINList = StockTransferObj.GINList;
                    StockTransfer.POList = StockTransferObj.POList;
                    StockTransfer.PRList = StockTransferObj.PRList;
                    StockTransfer.AllocatedAdditionalList = StockTransferObj.AllocatedAdditionalList;
                    $("#divData").data("GINList", StockTransfer.GINList);
                    $("#divData").data("POList", StockTransfer.POList);
                    $("#divData").data("PRList", StockTransfer.PRList);
                    $("#divData").data("AllocatedAdditionalList", StockTransfer.AllocatedAdditionalList);
                    // Clear  GIN grid and Bind grid as null
                    GrandGrid.Utilities.ResetGrid(true, "grdGIN");
                    StockTransfer.GINList = new Array();
                    $("#divData").data("GINList", StockTransfer.GINList);
                    GrandGrid.MakeGrid($("#grdGIN"), 0, StockTransfer.GINList);
                    // Clear  grdPO grid and Bind grid as null
                    GrandGrid.Utilities.ResetGrid(true, "grdPO");
                    StockTransfer.POList = new Array();
                    $("#divData").data("GINList", StockTransfer.POList);
                    GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
                    // Clear  grdPR grid and Bind grid as null
                    GrandGrid.Utilities.ResetGrid(true, "grdPR");
                    StockTransfer.PRList = new Array();
                    $("#divData").data("PRList", StockTransfer.PRList);
                    GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
                    // Clear  grdAdditionalItemList grid and Bind grind as null
                    GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
                    StockTransfer.AllocatedAdditionalList = new Array();
                    $("#divData").data("AllocatedQtyList", StockTransfer.AllocatedAdditionalList);
                    GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
                    // Fill Store to Dropdown
                    //FillGRNStore(0, 1);
                    //  FillGRNStore($("[id$=hdfDeptID]").val(), 1);
                    FillGRNStore($("[id$=hdfGinStore]").val(), 1);

                    // Show Hide Entry Section details
                    SetToggle();
                    FillCompany(0);
                    //SetToggle();
                }
            }
        }
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split(StockTransfer.EQUALVALUE);
            if ((pK[1] != StockTransfer.EMPTYVALUE && pK[0] == StockTransfer.Pk) || (pK[1] != StockTransfer.EMPTYVALUE && pK[0] == StockTransfer.RefId)) {
                if ($("[id$=ViewStatus]").val() == StockTransfer.ValueOne && parseInt($("[id$=SFH_IS_EDIT]").val()) != 1) {
                    FillDetails(StockTransferObj, 1);
                }
                else {
                    FillDetails(StockTransferObj, 0);
                }

            }
        }
    }
    else {

        StockTransfer.StockTransferList = StockTransferObj;
        StockTransfer.GINList = StockTransferObj.GINList;
        StockTransfer.POList = StockTransferObj.POList;
        StockTransfer.PRList = StockTransferObj.PRList;
        StockTransfer.AllocatedAdditionalList = StockTransferObj.AllocatedAdditionalList;
        $("#divData").data("GINList", StockTransfer.GINList);
        $("#divData").data("POList", StockTransfer.POList);
        $("#divData").data("PRList", StockTransfer.PRList);
        $("#divData").data("AllocatedAdditionalList", StockTransfer.AllocatedAdditionalList);
        // Clear  GIN grid and Bind grid as null
        GrandGrid.Utilities.ResetGrid(true, "grdGIN");
        StockTransfer.GINList = new Array();
        $("#divData").data("GINList", StockTransfer.GINList);
        GrandGrid.MakeGrid($("#grdGIN"), 0, StockTransfer.GINList);
        // Clear  grdPO grid and Bind grid as null
        GrandGrid.Utilities.ResetGrid(true, "grdPO");
        StockTransfer.POList = new Array();
        $("#divData").data("GINList", StockTransfer.POList);
        GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
        // Clear  grdPR grid and Bind grid as null
        GrandGrid.Utilities.ResetGrid(true, "grdPR");
        StockTransfer.PRList = new Array();
        $("#divData").data("PRList", StockTransfer.PRList);
        GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
        // Clear  grdAdditionalItemList grid and Bind grind as null
        GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
        StockTransfer.AllocatedAdditionalList = new Array();
        $("#divData").data("AllocatedQtyList", StockTransfer.AllocatedAdditionalList);
        GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
        // Fill Store to Dropdown
        //FillGRNStore(0, 1);
        FillGRNStore($("[id$=hdfDeptID]").val(), 1);

        // Show Hide Entry Section details
        SetToggle();
        FillCompany(0);
        //SetToggle();
    }
    // check status is view mode then enable date and dept
    if ($("[id$=ViewStatus]").val() == StockTransfer.ValueOne && parseInt($("[id$=SFH_IS_EDIT]").val()) != 1) {
        //$("#divSearch").hide();
        $("#searchwrap").hide();
        $("[id$=SFH_DEPT]").attr("disabled", true);
        $("[id$=SFH_DATE]").attr("disabled", true);
        $("[id$=SFH_COMPANY]").attr("disabled", true);
    }
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=SFH_COMPANY]").attr("disabled", "disabled");
    }

}
//###
function DateInit() {
    ///<summary>initial Date condition</summary>
    GrandScriptUtils.DatePicker(StockTransfer.StockTrasnferDate, false, false, false);
}

//#endregion 

//#region----------- Core Section----------------

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    window.location = "StockTransfer.aspx";
    return false;
}

function CancelPage() {
    ///<summary>Method to Cancel page and navigate to listing page</summary>
    // Remove validations
    RemoveAllValidations();
    //set Redirect url
    window.location = StockTransfer.StockTransferListUrl;
    return false;
}

function SetToggle() {
    ///<summary>Method to Set Toggle For Each Section</summary>
    $("#divPO").hide();
    $("#divPR").hide();
    $("#divAdnlDtls").hide();
}

function ShowDetails(type) {
    ///<summary>Method to Show Grid by Type 1- Gin, 2- PO, 3-PR,4-Addnl Details</summary>
    // GIn Details
    if (type == 1) {
        $("#divGIN").show(1000);
        $("#imgGINHide").hide();
        $("#imgGINShow").show();
    }
    //PO Details
    else if (type == 2) {
        $("#divPO").show(1000);
        $("#imgPOHide").hide();
        $("#imgPOShow").show();
    }
    // PR Details
    else if (type == 3) {
        $("#divPR").show(1000);
        $("#imgPRHide").hide();
        $("#imgPRShow").show();
    }
    // Additional Section
    else if (type == 4) {
        $("#divAdnlDtls").show();
        $("#imgADNLHide").hide();
        $("#imgADNLShow").show();
    }

}

function HideDetails(type) {
    ///<summary>Method to Hide Grid by Type 1- Gin, 2- PO, 3-PR,4-Addnl Details</summary>
    // GIn Details
    if (type == 1) {
        $("#divGIN").hide(1000);
        $("#imgGINHide").show();
        $("#imgGINShow").hide();
    }
    //PO Details
    else if (type == 2) {
        $("#divPO").hide(1000);
        $("#imgPOHide").show();
        $("#imgPOShow").hide();
    }
    // PR Details
    else if (type == 3) {
        $("#divPR").hide(1000);
        $("#imgPRHide").show();
        $("#imgPRShow").hide();
    }
    // Additional Section
    else if (type == 4) {
        $("#divAdnlDtls").hide(1000);
        $("#imgADNLHide").show();
        $("#imgADNLShow").hide();

    }
}
function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=SFH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = StockTransfer.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val();
    }
    else {
        getURL = StockTransfer.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        if (selectVal == undefined || selectVal == 0) {
            var selCompany = $("[id$=hdfCompany]").val();
            $("#SFH_COMPANY").val(selCompany);
        }
    });
}

function FillGRNStore(selectVal, type) {
    ///<summary>to fill GRN store combo</summary>
    var drpID = $("select[id$=SFH_DEPT]").attr("id");
    $.get(StockTransfer.FillGRNStoreUrl + $("[id$=BizUnitPk]").val() + StockTransfer.GRNStoreParams, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        //        $("[id$=SFH_DEPT]").attr("disabled", true);
        // Fill Inspected item for transfering
        if (type == 1) {
            BindInspecteditemForTransferingGrid();
        }
    });
}

//#region ============================================ Inspected Item for  Transferring ===============================================

function BindInspecteditemForTransferingGrid() {
    ///<summary>Method to GET GIN Details By Selected Store- Fill Details to Grid</summary>
    $.getJSON(StockTransfer.GINDetailsURL + $("[id$=SFH_DEPT]").val() + "&SBU=" + $("[id$=BizUnitPk]").val() + "&Status=2" + "&GinPk=" + $("[id$=GinPk]").val(), null, function (data) {
        if (data == null) {
            // reset gIN Grid and fill with empty data
            GrandGrid.Utilities.ResetGrid(true, "grdGIN");
            // $("#divGIN").css("display", "none");
            $("#divGIN").hide();
            $("[id$=AddToList]").hide();

        }
        else
            if (data != null && data != StockTransfer.EMPTYVALUE && data != StockTransfer.MINUSONEVALUE) {
                // check data is array or not, then is not an array, then create array and push to list
                $("#divGIN").show();
                if (!($.isArray(data))) {
                    if (data != undefined) {
                        var objArray = data;
                        data = new Array();
                        data.push(objArray);
                    }
                    else {
                        var objArray = data;
                        data = new Array();
                    }
                }
                // assign data to GINList
                StockTransfer.GINList = data;
                // and fill GINList to GIn Grid
                FillGINDetails();
                // show AddToList button
                $("[id$=AddToList]").show();
            }
            else {
                // reset gIN Grid and fill with empty data
                GrandGrid.Utilities.ResetGrid(true, "grdGIN");
                // Hide AddToList button
                $("[id$=AddToList]").hide();
            }

    });
}

function FillGINDetails() {
    ///<summary> Method to Fill GIn Details to GRID </summary>
    GrandGrid.Utilities.ResetGrid(true, "grdGIN");
    // check StockTransfer.GINList is array or not, then is not an array, then create array and push to list
    if (!($.isArray(StockTransfer.GINList))) {
        if (StockTransfer.GINList != undefined) {
            var objArray = StockTransfer.GINList;
            StockTransfer.GINList = new Array();
            StockTransfer.GINList.push(objArray);
        }
        else {
            var objArray = StockTransfer.GINList;
            StockTransfer.GINList = new Array();
        }
    }
    // check GINList count >0 , then fill details to GRiid and show AddToList button, else hide button
    if (StockTransfer.GINList.length > 0) {
        GrandGrid.MakeGrid($("#grdGIN"), 0, StockTransfer.GINList);
        $("[id$=AddToList]").show();
        //Set Default company while page load from inbox
        if ($("[id$=IsPrefID]").val() == "1") {
            pohDefaultCompany = StockTransfer.GINList[0].GIH_COMPANY;
            $("[id$=hdfCompany]").val(StockTransfer.GINList[0].GIH_COMPANY);
            $("[id$=SFH_DATE]").val(StockTransfer.GINList[0].GIH_DATE);
            //Set DDL
            FillCompany(pohDefaultCompany);
        }
    }
    else {
        $("[id$=AddToList]").hide();
    }
    //    // add data to divData Store
    //    $("#divData").data("GINList", StockTransfer.GINList);

}

//Juno
function IsSameQALotNo() {
    var retVal = false;
    var grdID;
    var xVal = "";
    var testVal = "";
    $("#grdGIN tr:has(td)").each(function (index) {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QA_LOT_NO", grdID);
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QA_LOT_NO", grdID);
                if (colVal != 'undefined' || colVal != 'null') {
                    if (colVal == xVal && colVal != "") {
                        retVal = true;
                    }
                    if (xVal == "")
                        xVal = colVal;
                }
            }
        }

    });
    return retVal;
}

//#endregion  ============================================ END Item for  Transferring ===============================================

//#region ============================================ Allocate Transfer Qty based on PO's ===============================================
// Method to Get PO Details , by Add to List Action From the GIN Section
// Read Selected GIN Details PK and Set it as a xml format, and Get Value for the PO Details
//###
function AddPOItemToList() {
    RemoveAllValidations(2);
    //Showing validation for Same QALot No
    if ($("[id$=hdfIsDuplicateLotNo]").val() != "1") {
        if (IsSameQALotNo()) {
            ShowDuplicateLotno();
            return false;
        }
    }
    var grdID;
    if ($("#grdGIN tr input[type=checkbox]:checked").length == 0) {
        GrandScriptUtils.ShowModal(StockTransfer.SelectGINDetails, StockTransfer.InformationTitle);
        return false;
    }
    if (IsSameItemDiffRateExists()) {//Check the selected PO's have same items with different rate
        GrandScriptUtils.ShowModal(StockTransfer.MsgSameItemDiffRate, StockTransfer.InformationTitle);
        return false;
    }
    if (!($.isArray(StockTransfer.GINPKList))) {
        if (StockTransfer.GINPKList != undefined) {
            var objArray = StockTransfer.GINPKList;
            StockTransfer.GINPKList = new Array();
            StockTransfer.GINPKList.push(objArray);
        }
        else {
            var objArray = StockTransfer.GINPKList;
            StockTransfer.GINPKList = new Array();
        }
    }
    StockTransfer.GINPKList = new Array();
    $("#grdGIN tr:has(td)").each(function () {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            StockTransfer.GINPKListObj = new Object();
            StockTransfer.GINPKListObj.xmlPK = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINDetailsPK, grdID);
            StockTransfer.GINPKListObj.ginItem = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINItem, grdID);
            StockTransfer.GINPKListObj.qtyApproved = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINQtyApproved, grdID).replace(/[^0-9\.]+/g, "");
            StockTransfer.GINPKList.push(StockTransfer.GINPKListObj);

            if ($("[id$=hdfCompany]").val() == $("[id$=hdfSBUCompany]").val()) {
                $("[id$=hdfCompany]").val(GrandGrid.Utilities.GetColumnValue($(this), "GID_COMPANY", grdID));
            }
            $("select[id$=SFH_COMPANY]").val($("[id$=hdfCompany]").val());
        }
    });
    if ($(document.forms[0]).valid()) {
        $("[id$=GINSelectedItems]").val('');
        $("[id$=GINSelectedItems]").val(JSON.stringify(StockTransfer.GINPKList));
        $("[id$=GINPKList]").val('');
        $("[id$=GINPKList]").val(JSON.stringify(StockTransfer.GINPKList));
        var jSonString = GrandScriptUtils.FormToJsonString("divGINPKList");
        $.post(StockTransfer.FillPoDetailsURL + "&Sbu=" + $("[id$=BizUnitPk]").val() + "&Pk=" + $("[id$=SFH_PK]").val(), jSonString, function (data) {
            if (data.root !== undefined && data.root != null && data.root != "" && data.root != "-1") {
                if (!($.isArray(data.root.POsTransferQty))) {
                    if (data.root.POsTransferQty != undefined) {
                        var objArray = data.root.POsTransferQty;
                        data.root.POsTransferQty = new Array();
                        data.root.POsTransferQty.push(objArray);
                    }
                    else {
                        var objArray = data.root.POsTransferQty;
                        data.root.POsTransferQty = new Array();
                    }
                }
                StockTransfer.POList = data.root.POsTransferQty;
                FillPODetails();
                $("[id$=AddToList]").hide();
                $("[id$=GINEdit]").val("0");
                // Get Selected Items List Details with GID Pk , Item Pk and Quantity
                GetInspectedItemQuantityDtls();
                $("[id$=AddPRDtlsToList]").show();
            }
        });
    }
    return false;
}
// Method to Get selected Inspected Item Details 
//###
function GetInspectedItemQuantityDtls() {
    $("#grdGIN tr:has(td)").each(function () {
        var itemExist = false;
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            var itemPk = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINItem, grdID);
            var inspQty = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINQtyApproved, grdID).replace(/[^0-9\.]+/g, "");
            if (StockTransfer.TotalItem.length == 0) {
                StockTransfer.TotalItemObj = new Object();
                StockTransfer.TotalItemObj.ItemPk = itemPk;
                StockTransfer.TotalItemObj.InspQty = inspQty;
                StockTransfer.TotalItem.push(StockTransfer.TotalItemObj);
            }
            else {

                for (var i in StockTransfer.TotalItem) {
                    if (StockTransfer.TotalItem[i].ItemPk == itemPk) {
                        StockTransfer.TotalItem[i].InspQty = parseFloat(parseFloat(StockTransfer.TotalItem[i].InspQty.replace(/[^0-9\.]+/g, "")) + parseFloat(inspQty)).toFixed(QtyDec);
                        itemExist = true;
                    }
                }
                if (itemExist == false) {
                    StockTransfer.TotalItemObj = new Object();
                    StockTransfer.TotalItemObj.ItemPk = itemPk;
                    StockTransfer.TotalItemObj.InspQty = inspQty;
                    StockTransfer.TotalItem.push(StockTransfer.TotalItemObj);
                }
            }
        }
    });

}
//Method to fill PO details , when select GID Add to List Action
//###
function FillPODetails() {
    GrandGrid.Utilities.ResetGrid(true, "grdPO");
    if (!($.isArray(StockTransfer.POList))) {
        if (StockTransfer.POList != undefined) {
            var objArray = StockTransfer.POList;
            StockTransfer.POList = new Array();
            StockTransfer.POList.push(objArray);
        }
        else {
            var objArray = StockTransfer.POList;
            StockTransfer.POList = new Array();
        }
    }
    GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);

    $("#divData").data("POList", StockTransfer.POList);
    if (StockTransfer.POList.length > 0) {
        //$("#divPO").show();
        ShowDetails(2);
    }
    else {
        //$("#divPO").hide();
        HideDetails(2);
    }

}
//#endregion ============================================ Allocate Transfer Qty based on PO's ===============================================

//#region ============================================ Allocate Transfer Qty based on PR's ===============================================
// Method to Get PR Details , by Add to List Action From the PO Section
// Read Selected PO Details PK and Set it as a xml format, and Get Value for the PR Details
function AddPRItemToList() {
    var grdID;
    var additionalPOColIndex;
    var additionalPO = 0;
    RemoveAllValidations(2);
    if ($("#grdPO tr input[type=checkbox]:checked").length == 0) {
        GrandScriptUtils.ShowModal(StockTransfer.SelectPODetails, StockTransfer.InformationTitle);
        return false;
    }
    // check Additional Quanity or Allocation Quantity Greater than PO Alloc Qty and PO Adnl Qty
    if (CheckAllocationAdnlQuantity()) {
        if (!($.isArray(StockTransfer.POPKList))) {
            if (StockTransfer.POPKList != undefined) {
                var objArray = StockTransfer.GINPKList;
                StockTransfer.POPKList = new Array();
                StockTransfer.POPKList.push(objArray);
            }
            else {
                var objArray = StockTransfer.POPKList;
                StockTransfer.POPKList = new Array();
            }
        }
        StockTransfer.POPKList = new Array();
        StockTransfer.AdditionalItem = new Array();
        StockTransfer.AllocationItem = new Array();
        var poQty;
        $("#grdPO tr:has(td)").each(function () {
            if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
                grdID = $(this).parents("table:first").attr("id");
                poQty = 0;
                StockTransfer.POPKListObj = new Object();
                StockTransfer.POPKListObj.xmlPK = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PODetailsPk, grdID);
                //26-02-2014 
                StockTransfer.POPKListObj.POD_PK = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PODetailsPk, grdID);
                var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOQty, grdID);
                if (colIndex) {
                    poQty = parseFloat($(this).find("td:eq(" + colIndex + "): input[type=text]").val());
                    if (isNaN(poQty))
                        poQty = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOQty, grdID).replace(/[^0-9\.]+/g, ""));
                }
                StockTransfer.POPKListObj.POQty = poQty;
                additionalPOColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOAddnlQty, grdID);
                if (additionalPOColIndex) {
                    additionalPO = parseFloat($(this).find("td:eq(" + additionalPOColIndex + "): input[type=text]").val());
                    if (isNaN(additionalPO))
                        additionalPO = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOAddnlQty, grdID).replace(/[^0-9\.]+/g, ""));
                }

                StockTransfer.POPKListObj.AdditionalPO = additionalPO;
                StockTransfer.POPKListObj.POR_ITEM = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PorItem, grdID);

                StockTransfer.POPKList.push(StockTransfer.POPKListObj);
            }
        });
        if ($(document.forms[0]).valid()) {
            $("[id$=POPKList]").val('');
            $("[id$=POPKList]").val(JSON.stringify(StockTransfer.POPKList));
            var jSonString = GrandScriptUtils.FormToJsonString("divPOPKList");
            //$("#divSearch").show();
            $("#searchwrap").show();
            // Get additional Item List From Po Grid
            GetAdditionalItemDetailsFromPOList();
            // Get Allocation Item List form PO Grid
            GetAllocationItemDetailsFromPOList();
            StockTransfer.POList = UpdatePODetails();
            // Check All item Fully Allocated or Not from GIn Grid
            if (CheckPOGridAssignSuccess()) {
                if (CheckTotalQtyWithPOAdnlAndAllocQty() && CheckPOWiseItemQty()) {
                    // Disable PO Details
                    DisablePOGrid();
                    FillAdditionalItem(0);
                    $.post(StockTransfer.FillPRDetailsURL + "&Sbu=" + $("[id$=BizUnitPk]").val(), jSonString, function (data) {
                        //                        $('#updateProgress').hide();
                        if (data.root !== undefined && data.root != null && data.root != "" && data.root != "-1") {
                            if (!($.isArray(data.root.PRsTransferQty))) {
                                if (data.root.PRsTransferQty != undefined) {
                                    var objArray = data.root.PRsTransferQty;
                                    data.root.PRsTransferQty = new Array();
                                    data.root.PRsTransferQty.push(objArray);
                                }
                                else {
                                    var objArray = data.root.PRsTransferQty;
                                    data.root.PRsTransferQty = new Array();
                                }
                            }
                            StockTransfer.PRList = data.root.PRsTransferQty;
                            for (var i in StockTransfer.PRList) {
                                StockTransfer.PRList[i].SFD_NO = $("[id$=SFH_NO]").html();
                            }
                            FillPRDetails();
                            $("[id$=AddPRDtlsToList]").hide();

                        }
                        else {
                            ShowDetails(4);
                        }
                    });
                }
                else {
                    GrandScriptUtils.ShowModal(StockTransfer.AllocationMappingError, StockTransfer.InformationTitle);
                }
            }
            else {
                GrandScriptUtils.ShowModal(StockTransfer.AllocationMappingErrorForFullQty, StockTransfer.InformationTitle);
            }
        }
    }
    else {
        GrandScriptUtils.ShowModal(StockTransfer.ReallocateErrorMsg, StockTransfer.InformationTitle);
    }
    if (StockTransfer.POList.length > 0) {
        //$("#divPO").show();
        ShowDetails(4);
    }
    else {
        //$("#divPO").hide();
        HideDetails(4);
    }

    return false;
}

//#region Check Selected sum of GinQuanity ==Total Quantity Assigned In PO(Sum of Both Allocational And Additional 
// Check Fully Selected Inspected Quantity From GIN Grid Assigned in PO Grid Or Not

function CheckPOWiseItemQty() {
    var poItemSum = 0;

    for (var i in StockTransfer.AllocationItem) {
        for (var j in StockTransfer.GINList) {
            if (StockTransfer.AllocationItem[i].POPk == StockTransfer.GINList[j].GID_PO_DTL
            && StockTransfer.AllocationItem[i].Item == StockTransfer.GINList[j].GID_ITEM
            && $.inArray(StockTransfer.GINList[j].GID_PK, StockTransfer.GINPKList)) {
                poItemSum = parseFloat(poItemSum) + parseFloat(StockTransfer.GINList[j].GID_QTY_APPROVED);
            }
        }
        if (parseFloat(poItemSum) < parseFloat(StockTransfer.AllocationItem[i].AllocQty)) {
            return false;
        }
        poItemSum = 0;
    }
    return true;
}


function CheckPOGridAssignSuccess() {
    var status = true;
    var totalAssignedQtyFromPOGrid = parseFloat(GetSumOfItemQtyFromPOGrid()).toFixed(QtyDec);
    var totalInspQtyFromGINGrid = parseFloat(GetSumOfItemQtyFromInspGrid()).toFixed(QtyDec);
    if (parseFloat(totalAssignedQtyFromPOGrid) != parseFloat(totalInspQtyFromGINGrid)) {
        status = false;
    }
    return status;

}
/// <summary>
/// Check the selected PO's have same items with different rate
/// </summary>
function IsSameItemDiffRateExists() {
    if (!($.isArray(StockTransfer.GINListTemp))) {
        if (StockTransfer.GINListTemp != undefined) {
            var objArray = StockTransfer.GINListTemp;
            StockTransfer.GINListTemp = new Array();
            StockTransfer.GINListTemp.push(objArray);
        }
        else {
            var objArray = StockTransfer.GINListTemp;
            StockTransfer.GINListTemp = new Array();
        }
    }
    StockTransfer.GINListTemp = new Array();
    var flag = false;
    $("#grdGIN tr:has(td)").each(function () {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            StockTransfer.GINPKListObj = new Object();
            StockTransfer.GINPKListObj.ginItem = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINItem, grdID);
            StockTransfer.GINPKListObj.POD_RATE = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), "POD_RATE", grdID)).toFixed(RateDec);
            StockTransfer.GINListTemp.push(StockTransfer.GINPKListObj);
            flag = CheckItemDiffRate(StockTransfer.GINPKListObj.ginItem, StockTransfer.GINPKListObj.POD_RATE); //Check the selected PO's have same items with different rate            
        }
    });
    return flag;
}
/// <summary>
/// CheckItemDiffRate
/// </summary>
function CheckItemDiffRate(itemPK, poRate) {
    var flag = false;
    if (StockTransfer.GINListTemp.length > 0) {
        for (var i in StockTransfer.GINListTemp) {
            if ((StockTransfer.GINListTemp[i].ginItem == itemPK) && (StockTransfer.GINListTemp[i].POD_RATE != poRate)) {
                flag = true;
                break;
            }
        }
    }
    return flag;
}

// Method to get Total Items Count From PO Grid

function GetSumOfItemQtyFromPOGrid() {
    var total = 0;
    if (StockTransfer.POList.length > 0) {
        for (var i in StockTransfer.POList) {
            total = parseFloat(total) + parseFloat(StockTransfer.POList[i].ALLOCATE_PO_QTY.replace(/[^0-9\.]+/g, "")) + parseFloat(StockTransfer.POList[i].ALLOCATE_PO_ADDL_QTY.replace(/[^0-9\.]+/g, ""));
        }
    }
    return total;
}
// Method to Total Selected Inspected from Grid

function GetSumOfItemQtyFromInspGrid() {
    var total = 0;
    if (StockTransfer.TotalItem.length > 0) {
        for (var i in StockTransfer.TotalItem) {
            total = parseFloat(total) + parseFloat(StockTransfer.TotalItem[i].InspQty.replace(/[^0-9\.]+/g, ""));
        }
    }
    return total;
}
// #endregion ============================================================================


//#region  Get Sum of each Selected PO Line Item Addtional Qty With GIN Inspected Qty 
// Get Sum of each Selected PO Line Item Addtional Qty With GIN Inspected Qty
function CheckTotalQtyWithPOAdnlAndAllocQty() {
    var status = true;
    for (var i in StockTransfer.AllocationItem) {
        var totalAlloAndAdnl = 0;
        for (var j in StockTransfer.AdditionalItem) {
            if (StockTransfer.AllocationItem[i].Item == StockTransfer.AdditionalItem[j].Item) {
                var sumofAllocQty = GetSumofAllocQtyFroSeletedItem(StockTransfer.AllocationItem[i].Item);
                var AdnlQty = StockTransfer.AdditionalItem[j].AdnlQty;
                totalAlloAndAdnl = parseFloat(sumofAllocQty) + parseFloat(AdnlQty); //.toFixed(QtyDec);
                //   totalAlloAndAdnl = parseFloat(parseFloat(GetSumofAllocQtyFroSeletedItem(StockTransfer.AllocationItem[i].Item)).toFixed(QtyDec) + parseFloat(StockTransfer.AdditionalItem[j].AdnlQty)).toFixed(QtyDec);
            }
        }
        //##Updated on 01112011
        if (totalAlloAndAdnl > 0) {
            if (CheckTotalWithItemPk(totalAlloAndAdnl, StockTransfer.AllocationItem[i].Item) == false) {
                status = false;
                //GrandScriptUtils.ShowModal("Allocation mapping error" + StockTransfer.AllocationItem[i].GinDtlPk + "Qty" + totalAlloAndAdnl, "Information"); 
            }
        }

    }
    return status;
}
// Method to Get Sum of Allocated Qty for a selected Item From PO Grid
function GetSumofAllocQtyFroSeletedItem(item) {
    var total = 0;
    for (var i in StockTransfer.AllocationItem) {
        if (StockTransfer.AllocationItem[i].Item == item)
            total = parseFloat(total) + parseFloat(StockTransfer.AllocationItem[i].AllocQty);
    }
    return total;
}
// Check Gin Inspected Detaisl With PO Additional And Alloc Item Details , is satisfied or not
function CheckTotalWithItemPk(totalAlloAndAdnl, itemPk) {
    for (var i in StockTransfer.TotalItem) {
        if (StockTransfer.TotalItem[i].ItemPk == itemPk) {
            if (parseFloat(StockTransfer.TotalItem[i].InspQty.replace(/[^0-9\.]+/g, "")).toFixed(QtyDec) != parseFloat(totalAlloAndAdnl).toFixed(QtyDec)) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
//endregion==================================================================
// Method to Update PO List - Read data from Allocation and Adnl Qty Text Box as pass as Array for each row
function UpdatePODetails() {
    if (StockTransfer.POList.length > 0) {
        for (var i in StockTransfer.POList) {

            var allocDtls = GetPOEditDetailsFormGrid(i);
            if (allocDtls.length > 0) {
                StockTransfer.POList[i].ALLOCATE_PO_QTY = allocDtls[0];
                //                StockTransfer.POList[i].ALLOCATE_PO_ADDL_QTY = allocDtls[1];
                StockTransfer.POList[i].ALLOCATE_PO_ADDL_QTY = allocDtls[1];
            }
        }
        return StockTransfer.POList;
    }
}
// Method to get Additional Iten and Allocation item from grid , when select Add to List Action in PO section
function GetPOEditDetailsFormGrid(indx) {
    var grdID;
    var allocateQtyIndex = 0;
    var adnlQtyIndex = 0;
    var allocDtls = new Array();
    var allocateQty = 0;
    var adnlQty = 0;
    var qtyApproved = 0;
    $("#grdPO tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
                allocateQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOQty, grdID);
                allocateQty = $(this).find("td:eq(" + allocateQtyIndex + ") input").val();
                if (allocateQty == undefined)
                    allocateQty = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOQty, grdID).replace(/[^0-9\.]+/g, "");
                adnlQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ALLOCATE_PO_ADDL_QTY", grdID);
                qtyApproved = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_APPROVED", grdID);
                adnlQty = $(this).find("td:eq(" + adnlQtyIndex + ") input").val();
                if (adnlQty == undefined)
                    adnlQty = GrandGrid.Utilities.GetColumnValue($(this), "ALLOCATE_PO_ADDL_QTY", grdID).replace(/[^0-9\.]+/g, "");

                if (allocateQty == null || allocateQty == undefined || allocateQty == "") {
                    allocateQty = 0;
                }
                if (adnlQty == null || adnlQty == undefined || adnlQty == "") {
                    adnlQty = 0;
                }
            }
            allocDtls.push(allocateQty);
            allocDtls.push(adnlQty);
            allocDtls.push(qtyApproved - allocateQty);

        }
    });
    return allocDtls;
}
// check Additional Quanity or Allocation Quantity Greater than PO Alloc Qty and PO Adnl Qty
function CheckAllocationAdnlQuantity() {
    var status = true;
    var allocationQty = 0.0;
    var additionalQty = 0.0;
    if (StockTransfer.POList.length > 0) {
        for (var i in StockTransfer.POList) {
            var allocDtls = GetPOEditDetailsFormGrid(i);
            if (allocDtls.length > 0) {

                if (parseFloat(StockTransfer.POList[i].BAL_PO_QTY) < parseFloat(allocDtls[0])) {
                    //GrandScriptUtils.ShowModal("Allocation Qty Error : " + allocDtls[0], "Information"); 
                    status = false;
                }
                //if (parseFloat(StockTransfer.POList[i].BAL_PO_ADDL_QTY) < parseFloat(allocDtls[1])) {
                additionalQty = parseFloat(parseFloat(StockTransfer.POList[i].GID_QTY_APPROVED) - parseFloat(StockTransfer.POList[i].BAL_PO_QTY)).toFixed(QtyDec);
                additionalQty = (additionalQty < 0) ? additionalQty * -1 : additionalQty;
                additionalQty = parseFloat(additionalQty);
                if (additionalQty < parseFloat(allocDtls[1])) {
                    //GrandScriptUtils.ShowModal("Additional Qty Error : " + allocDtls[1], "Information"); 
                    status = false;
                }
            }
        }

        return status;


    }

}
// Method to Get AllocationItem Details From PO Grid
function GetAllocationItemDetailsFromPOList() {
    if (!($.isArray(StockTransfer.AllocationItem))) {
        if (StockTransfer.AllocationItem != undefined) {
            var objArray = StockTransfer.AllocationItem;
            StockTransfer.AllocationItem = new Array();
            StockTransfer.AllocationItem.push(objArray);
        }
        else {
            var objArray = StockTransfer.AllocationItem;
            StockTransfer.AllocationItem = new Array();
        }
    }
    var grdID;
    var allocateQtyIndex = 0;
    var itemIndx = 0;
    var allocateQty = 0;
    var item = 0;
    var itemExist = false;
    var ginDtlPk = 0;
    var poPk = 0;
    $("#grdPO tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {

            allocateQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOQty, grdID);
            allocateQty = $(this).find("td:eq(" + allocateQtyIndex + ") input").val();
            itemIndx = GrandGrid.Utilities.GetColumnIndex($(this), "POR_ITEM", grdID);
            item = GrandGrid.Utilities.GetColumnValue($(this), "POR_ITEM", grdID);
            poPk = GrandGrid.Utilities.GetColumnValue($(this), "POD_PK", grdID);
            ginDtlPk = GrandGrid.Utilities.GetColumnValue($(this), "GID_PK", grdID);
            //GrandGrid.Utilities.GetColumnValue($(this), "GRH_PK", grdID);
            if (allocateQty == null || allocateQty == undefined || allocateQty == "") {
                allocateQty = 0;
            }
            if (item == null || item == undefined || item == "") {
                item = 0;
            }
            if (StockTransfer.AllocationItem.length > 0) {
                for (var i in StockTransfer.AllocationItem) {
                    if (StockTransfer.AllocationItem[i].Item == item && StockTransfer.AllocationItem[i].POPk == poPk) {
                        StockTransfer.AllocationItem[i].AllocQty = parseFloat(parseFloat(StockTransfer.AllocationItem[i].AllocQty) + parseFloat(allocateQty)).toFixed(QtyDec);
                        itemExist = true;
                    }

                }
                // if item not avialable in the allocation List
                if (itemExist == false) {
                    StockTransfer.AllocationItemObj = new Object();
                    StockTransfer.AllocationItemObj.Item = item;
                    StockTransfer.AllocationItemObj.POPk = poPk;
                    StockTransfer.AllocationItemObj.AllocQty = parseFloat(allocateQty);
                    StockTransfer.AllocationItem.push(StockTransfer.AllocationItemObj);
                }

            }
            // if no items in Allocation List, add item as 
            else {
                StockTransfer.AllocationItemObj = new Object();
                StockTransfer.AllocationItemObj.Item = item;
                StockTransfer.AllocationItemObj.POPk = poPk;
                StockTransfer.AllocationItemObj.AllocQty = parseFloat(allocateQty);
                StockTransfer.AllocationItem.push(StockTransfer.AllocationItemObj);
            }

        }
    });


}
// Method to Get Additional Item Details From PO Grid
function GetAdditionalItemDetailsFromPOList() {
    if (!($.isArray(StockTransfer.AdditionalItem))) {
        if (StockTransfer.AdditionalItem != undefined) {
            var objArray = StockTransfer.AdditionalItem;
            StockTransfer.AdditionalItem = new Array();
            StockTransfer.AdditionalItem.push(objArray);
        }
        else {
            var objArray = StockTransfer.AdditionalItem;
            StockTransfer.AdditionalItem = new Array();
        }
    }
    var grdID;
    var adnlQtyIndex = 0;
    var itemIndx = 0;
    var adnlQty = 0;
    var item = 0;
    var itemExist = false;
    var ginDtlPk = 0;
    $("#grdPO tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            itemExist = false;
            adnlQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ALLOCATE_PO_ADDL_QTY", grdID);
            adnlQty = $(this).find("td:eq(" + adnlQtyIndex + ") input").val();
            if (typeof (adnlQty) === "undefined") {
                adnlQty = $(this).find("td:eq(" + adnlQtyIndex + ")").html();
            }


            itemIndx = GrandGrid.Utilities.GetColumnIndex($(this), "POR_ITEM", grdID);
            item = GrandGrid.Utilities.GetColumnValue($(this), "POR_ITEM", grdID);
            ginDtlPk = GrandGrid.Utilities.GetColumnValue($(this), "GID_PK", grdID);
            if (adnlQty == null || adnlQty == undefined || adnlQty == "") {
                adnlQty = 0;
            }
            if (item == null || item == undefined || item == "") {
                item = 0;
            }
            if (StockTransfer.AdditionalItem.length > 0) {
                for (var i in StockTransfer.AdditionalItem) {
                    if (StockTransfer.AdditionalItem[i].Item == item) {
                        StockTransfer.AdditionalItem[i].AdnlQty = parseFloat(parseFloat(StockTransfer.AdditionalItem[i].AdnlQty) + parseFloat(adnlQty)).toFixed(QtyDec);
                        itemExist = true;
                    }
                }
                // if item not avialable in the allocation List
                if (itemExist == false) {
                    StockTransfer.AdditionalItemObj = new Object();
                    StockTransfer.AdditionalItemObj.Item = item;
                    StockTransfer.AdditionalItemObj.AdnlQty = parseFloat(adnlQty);
                    StockTransfer.AdditionalItem.push(StockTransfer.AdditionalItemObj);
                }
            }
            // if no items in Allocation List, add item as 
            else {
                StockTransfer.AdditionalItemObj = new Object();
                StockTransfer.AdditionalItemObj.Item = item;
                StockTransfer.AdditionalItemObj.AdnlQty = parseFloat(adnlQty);
                StockTransfer.AdditionalItem.push(StockTransfer.AdditionalItemObj);
            }
        }
    });

}
// Method to Fill PR Details to Grid after PO Add to list action
function FillPRDetails() {
    GrandGrid.Utilities.ResetGrid(true, "grdPR");
    if (!($.isArray(StockTransfer.PRList))) {
        if (StockTransfer.PRList != undefined) {
            var objArray = StockTransfer.PRList;
            StockTransfer.PRList = new Array();
            StockTransfer.PRList.push(objArray);
        }
        else {
            var objArray = StockTransfer.PRList;
            StockTransfer.PRList = new Array();
        }
    }
    GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
    $("#divData").data("PRList", StockTransfer.PRList);

    if (StockTransfer.PRList.length > 0) {
        //$("#divPR").show();
        ShowDetails(3);
        HideDetails(4);

    }
    else {
        HideDetails(3);
        ShowDetails(4);
        //$("#divPR").hide();
    }
}
// Method to Read only mode - PO Details Grid, After select Add to list action 
function DisablePOGrid() {
    // Assign Grid is Readonly mode
    $("[id$=POEdit]").val(StockTransfer.ValueZero);
    // Reset Grid and Fill Details to Grid
    GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
}
// Method to Fill Additional Item, when select Add Item to List - PO Section
function FillAdditionalItem(selectVal) {
    var drpID = $("select[id$=ITEM]").attr("id");
    $("[id$=POPKList]").val(JSON.stringify(StockTransfer.POPKList));
    var jSonString = GrandScriptUtils.FormToJsonString("divPOPKList");
    $.post(StockTransfer.AdditionalItemUrl + "&Sbu=" + $("[id$=BizUnitPk]").val(), jSonString, function (data) {

        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        FillStore(0);
    });
}
//#endregion ============================================ Allocate Transfer Qty based on PO's ===============================================
//juno
function getQaLotNo(intemID) {

    var retVal = "";
    var grdID;
    var colIndex = "";
    var colVal = "";
    var itemIndex = "";
    var itemVal = "";

    $("#grdGIN tr:has(td)").each(function (index) {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            itemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_ITEM", grdID);
            if (itemIndex != null) {
                itemVal = GrandGrid.Utilities.GetColumnValue($(this), "GID_ITEM", grdID);
                if (itemVal != null && itemVal == intemID) {
                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QA_LOT_NO", grdID);
                    if (colIndex != null) {
                        retVal = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QA_LOT_NO", grdID);
                    }
                }
            }
        }
    });
    return retVal;
}
// #region ============================================ Allocate Additional item Section ===============================================
function FillStore(selectVal) {
    var LotNo = getQaLotNo($("[id$=ITEM]").val())
    $("[id$=SFD_QA_LOT_NO]").val(LotNo)
    var drpID = $("select[id$=STORE]").attr("id");
    // Convert Inspected Item Details to JSOn String
    var jSonString = GrandScriptUtils.FormToJsonString("divInspectedItemList");
    $.get("StockTransfer.do?Action=GetStoreByItem&MaterialID=" + $("[id$=ITEM]").val() + "&SBUPk=" + $("[id$=BizUnitPk]").val() + "&DeptPk=" + $("[id$=hdfDeptID]").val(), jSonString, function (data) {
        if (data == '[object XMLDocument]' || data == "") {
            GrandScriptUtils.FillDropDown(drpID, null, true, true, selectVal);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
            //27-02-2014 
            var selectedItemsList = $.parseJSON($("[id$=POPKList]").val());
            var addlPO = 0;
            if (selectedItemsList != null)
                for (var i = 0; i < selectedItemsList.length; i++) {
                    if ($("[id$=ITEM]").val() == selectedItemsList[i].POR_ITEM) {
                        //                                                addlPO = addlPO + selectedItemsList[i].AdditionalPO; StockTransfer.AdditionalItem                        
                        if (selectedItemsList[i].AdditionalPO === null || selectedItemsList[i].AdditionalPO == "null") {
                            for (var j = 0; j < StockTransfer.AdditionalItem.length; j++) {
                                if ($("[id$=ITEM]").val() == StockTransfer.AdditionalItem[i].Item) {
                                    addlPO = addlPO + StockTransfer.AdditionalItem[i].AdnlQty;
                                }
                            }
                        }
                        else {
                            addlPO = addlPO + selectedItemsList[i].AdditionalPO;
                        }
                    }
                }
            $("[id$=QTY_ALLOCATED]").val(addlPO);
        }
        // Method to Fill Material UOM
        FillMaterialUOM();
    });
}
// Method to Fill Material UOM
function FillMaterialUOM() {
    $.post("StockTransfer.do?Action=GetItemUOM&ItemPk=" + $("[id$=ITEM]").val() + "&Status=1", null, function (data) {
        if (data != null && data != "" && data != "-1") {
            //for solving Firefox null checking  
            var jsonData = data[0];
            if (jsonData === undefined) {
                jsonData = $.parseJSON(data);
            }
            if (jsonData != null && jsonData != '') {
                $("input[id$=UOM_TEXT]").val(jsonData.Text);
                $("[id$=UOM]").val(jsonData.Value);
            }
        }
    });
}

// Add additional Item Details to List and Fill to Grid
function AllocateAdditionalItem() {

    RemoveAllValidations();
    AddValidation(2);
    if ($(document.forms[0]).valid()) {
        GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
        StockTransfer.AdditionalAllocationObj = new Object();
        StockTransfer.AdditionalAllocationObj.SFD_PK = $("[id$=SFD_PK]").val();
        StockTransfer.AdditionalAllocationObj.SFD_SL_NO = StockTransfer.AllocatedAdditionalList.length + 1;
        if (CheckAdnlItemAlreadyAdded($("[id$=ITEM]").val(), $("[id$=STORE]").val())) {
            StockTransfer.AdditionalAllocationObj.PRD_DEPT = $("[id$=STORE]").val();
            StockTransfer.AdditionalAllocationObj.PRD_DEPT_NAME = $("[id$=STORE] :selected").text();
            StockTransfer.AdditionalAllocationObj.POR_ITEM = $("[id$=ITEM]").val();
            StockTransfer.AdditionalAllocationObj.POR_ITEM_NAME = $("[id$=ITEM] :selected").text();
            StockTransfer.AdditionalAllocationObj.ALLOCATE_ADDL_PR_QTY = $("[id$=QTY_ALLOCATED]").val();
            StockTransfer.AdditionalAllocationObj.POR_UOM = $("[id$=UOM]").val();
            StockTransfer.AdditionalAllocationObj.POR_UOM_NAME = $("input[id$=UOM_TEXT]").val();
            StockTransfer.AdditionalAllocationObj.SFD_LOCATION = $("input[id$=SFD_LOCATION]").val();
            StockTransfer.AdditionalAllocationObj.SFD_NO = $("[id$=SFH_NO]").html();
            StockTransfer.AdditionalAllocationObj.SFD_QA_LOT_NO = $("input[id$=SFD_QA_LOT_NO]").val();
            StockTransfer.AllocatedAdditionalList.push(StockTransfer.AdditionalAllocationObj);
            if (adnlItemSlno == 0) {
                $("#divData").data("AllocatedQtyList", StockTransfer.AllocatedAdditionalList);
                GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
                ClearAllocationAdditionalDtls();
            }
            else {
                DeleteAdditionalItemDtls();
                ClearAllocationAdditionalDtls();
            }
        }
        else {
            GrandScriptUtils.ShowModal("Translate(AlreadyItemAssignToStore)", StockTransfer.InformationTitle);
        }
    }
    return false;
}

function CheckAdnlItemAlreadyAdded(itemPk, Store) {
    var status = true;
    for (var i in StockTransfer.AllocatedAdditionalList) {
        if (StockTransfer.AllocatedAdditionalList[i].POR_ITEM == $("[id$=ITEM]").val() && StockTransfer.AllocatedAdditionalList[i].PRD_DEPT == $("[id$=STORE]").val() && StockTransfer.AllocatedAdditionalList[i].SFD_SL_NO != adnlItemSlno) {

            status = false;
        }
    }
    return status;

}



//#endregion ============================================ Allocate Additional item Section ===============================================


function FillDetails(StockTransferObj, type) {
    $("[id$=POEdit]").val("0");
    $("[id$=GINEdit]").val("0")

    //Set a stamp for cancelled record
    if (StockTransferObj.root.SFH_STATUS == 4)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");

    //End

    StockTransfer.StockTransferList = StockTransferObj;
    if (parseInt(StockTransferObj.root.GIN_ISSUED_FLAG) == 0) {
        if (StockTransferObj.root.TotalItem != undefined) {
            StockTransfer.TotalItem = StockTransferObj.root.TotalItem.TotalItemDtls;
            if (!($.isArray(StockTransfer.TotalItem))) {
                if (StockTransfer.TotalItem != undefined) {
                    var objArray = StockTransfer.TotalItem;
                    StockTransfer.TotalItem = new Array();
                    StockTransfer.TotalItem.push(objArray);
                }
                else {
                    var objArray = StockTransfer.TotalItem;
                    StockTransfer.TotalItem = new Array();
                }
            }
        }
        else {
            StockTransfer.TotalItem = new Array();
            if (!($.isArray(StockTransfer.TotalItem))) {
                if (StockTransfer.TotalItem != undefined) {
                    var objArray = StockTransfer.TotalItem;
                    StockTransfer.TotalItem = new Array();
                    StockTransfer.TotalItem.push(objArray);
                }
                else {
                    var objArray = StockTransfer.TotalItem;
                    StockTransfer.TotalItem = new Array();
                }
            }
        }
        if (StockTransferObj.root.GINPKList != undefined) {
            StockTransfer.GINPKList = StockTransferObj.root.GINPKList.GinPkDtl;
            if (!($.isArray(StockTransfer.GINPKList))) {
                if (StockTransfer.GINPKList != undefined) {
                    var objArray = StockTransfer.GINPKList;
                    StockTransfer.GINPKList = new Array();
                    StockTransfer.GINPKList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.GINPKList;
                    StockTransfer.GINPKList = new Array();
                }
            }
        }
        else {
            StockTransfer.GINPKList = new Array();
            if (!($.isArray(StockTransfer.GINPKList))) {
                if (StockTransfer.GINPKList != undefined) {
                    var objArray = StockTransfer.GINPKList;
                    StockTransfer.GINPKList = new Array();
                    StockTransfer.GINPKList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.GINPKList;
                    StockTransfer.GINPKList = new Array();
                }
            }
        }
        if (StockTransferObj.root.POPKList != undefined) {
            StockTransfer.POPKList = StockTransferObj.root.POPKList.POPkDtl;
            if (!($.isArray(StockTransfer.POPKList))) {
                if (StockTransfer.POPKList != undefined) {
                    var objArray = StockTransfer.POPKList;
                    StockTransfer.POPKList = new Array();
                    StockTransfer.POPKList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.POPKList;
                    StockTransfer.POPKList = new Array();
                }
            }
        }
        else {
            StockTransfer.POPKList = new Array();
            if (!($.isArray(StockTransfer.POPKList))) {
                if (StockTransfer.POPKList != undefined) {
                    var objArray = StockTransfer.POPKList;
                    StockTransfer.POPKList = new Array();
                    StockTransfer.POPKList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.POPKList;
                    StockTransfer.POPKList = new Array();
                }
            }
        }
        if (StockTransferObj.root.AdditionalItem != undefined) {
            StockTransfer.AdditionalItem = StockTransferObj.root.AdditionalItem.AdditionalItemDtls;
            if (!($.isArray(StockTransfer.AdditionalItem))) {
                if (StockTransfer.AdditionalItem != undefined) {
                    var objArray = StockTransfer.AdditionalItem;
                    StockTransfer.AdditionalItem = new Array();
                    StockTransfer.AdditionalItem.push(objArray);
                }
                else {
                    var objArray = StockTransfer.AdditionalItem;
                    StockTransfer.AdditionalItem = new Array();
                }
            }
        }
        else {
            StockTransfer.AdditionalItem = new Array();
            if (!($.isArray(StockTransfer.AdditionalItem))) {
                if (StockTransfer.AdditionalItem != undefined) {
                    var objArray = StockTransfer.AdditionalItem;
                    StockTransfer.AdditionalItem = new Array();
                    StockTransfer.AdditionalItem.push(objArray);
                }
                else {
                    var objArray = StockTransfer.AdditionalItem;
                    StockTransfer.AdditionalItem = new Array();
                }
            }
        }
        if (StockTransferObj.root.AllocationItem != undefined) {
            StockTransfer.AllocationItem = StockTransferObj.root.AllocationItem.AllocationItemDtls;
            if (!($.isArray(StockTransfer.AllocationItem))) {
                if (StockTransfer.AllocationItem != undefined) {
                    var objArray = StockTransfer.AllocationItem;
                    StockTransfer.AllocationItem = new Array();
                    StockTransfer.AllocationItem.push(objArray);
                }
                else {
                    var objArray = StockTransfer.AllocationItem;
                    StockTransfer.AllocationItem = new Array();
                }
            }
        }
        else {
            StockTransfer.AllocationItem = new Array();
            if (!($.isArray(StockTransfer.AllocationItem))) {
                if (StockTransfer.AllocationItem != undefined) {
                    var objArray = StockTransfer.AllocationItem;
                    StockTransfer.AllocationItem = new Array();
                    StockTransfer.AllocationItem.push(objArray);
                }
                else {
                    var objArray = StockTransfer.AllocationItem;
                    StockTransfer.AllocationItem = new Array();
                }
            }
        }

        if (StockTransferObj.root.GinList != undefined) {
            StockTransfer.GINList = StockTransferObj.root.GinList.GinListDetail;
            if (!($.isArray(StockTransfer.GINList))) {
                if (StockTransfer.GINList != undefined) {
                    var objArray = StockTransfer.GINList;
                    StockTransfer.GINList = new Array();
                    StockTransfer.GINList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.GINList;
                    StockTransfer.GINList = new Array();
                }
            }
        }
        else {
            StockTransfer.GINList = new Array();
            if (!($.isArray(StockTransfer.GINList))) {
                if (StockTransfer.GINList != undefined) {
                    var objArray = StockTransfer.GINList;
                    StockTransfer.GINList = new Array();
                    StockTransfer.GINList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.GINList;
                    StockTransfer.GINList = new Array();
                }
            }
        }

        if (StockTransferObj.root.POList != undefined) {
            StockTransfer.POList = StockTransferObj.root.POList.POsTransferQty;
            if (!($.isArray(StockTransfer.POList))) {
                if (StockTransfer.POList != undefined) {
                    var objArray = StockTransfer.POList;
                    StockTransfer.POList = new Array();
                    StockTransfer.POList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.POList;
                    StockTransfer.POList = new Array();
                }
            }
        }
        else {
            StockTransfer.POList = new Array();
            if (!($.isArray(StockTransfer.POList))) {
                if (StockTransfer.POList != undefined) {
                    var objArray = StockTransfer.POList;
                    StockTransfer.POList = new Array();
                    StockTransfer.POList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.POList;
                    StockTransfer.POList = new Array();
                }
            }
        }
        if (StockTransferObj.root.PRList != undefined) {
            StockTransfer.PRList = StockTransferObj.root.PRList.PRsTransferQty;
            if (!($.isArray(StockTransfer.PRList))) {
                if (StockTransfer.PRList != undefined) {
                    var objArray = StockTransfer.PRList;
                    StockTransfer.PRList = new Array();
                    StockTransfer.PRList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.PRList;
                    StockTransfer.PRList = new Array();
                }
            }
        }
        else {
            StockTransfer.PRList = new Array();
            if (!($.isArray(StockTransfer.PRList))) {
                if (StockTransfer.PRList != undefined) {
                    var objArray = StockTransfer.PRList;
                    StockTransfer.PRList = new Array();
                    StockTransfer.PRList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.PRList;
                    StockTransfer.PRList = new Array();
                }
            }

        }
        if (StockTransferObj.root.AllocatedQtyList != undefined) {
            StockTransfer.AllocatedAdditionalList = StockTransferObj.root.AllocatedQtyList.AddlDetail;
            if (!($.isArray(StockTransfer.AllocatedAdditionalList))) {
                if (StockTransfer.AllocatedAdditionalList != undefined) {
                    var objArray = StockTransfer.AllocatedAdditionalList;
                    StockTransfer.AllocatedAdditionalList = new Array();
                    StockTransfer.AllocatedAdditionalList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.AllocatedAdditionalList;
                    StockTransfer.AllocatedAdditionalList = new Array();
                }
            }
        }
        else {
            StockTransfer.AllocatedAdditionalList = new Array();
            if (!($.isArray(StockTransfer.AllocatedAdditionalList))) {
                if (StockTransfer.AllocatedAdditionalList != undefined) {
                    var objArray = StockTransfer.AllocatedAdditionalList;
                    StockTransfer.AllocatedAdditionalList = new Array();
                    StockTransfer.AllocatedAdditionalList.push(objArray);
                }
                else {
                    var objArray = StockTransfer.AllocatedAdditionalList;
                    StockTransfer.AllocatedAdditionalList = new Array();
                }
            }
        }
        $("#divData").data("GINList", StockTransfer.GINList);
        $("#divData").data("POList", StockTransfer.POList);
        $("#divData").data("PRList", StockTransfer.PRList);
        $("#divData").data("AllocatedAdditionalList", StockTransfer.AllocatedAdditionalList);

        $("input[id$=SFH_STATUS]").val(StockTransferObj.root.SFH_STATUS);
        if (type == 0) {
            if (parseInt($("input[id$=SFH_STATUS]").val()) == 0 || parseInt($("input[id$=SFH_STATUS]").val()) == 6) {
                $("[id$=ViewStatus]").val(0);
            }
            else {
                $("[id$=ViewStatus]").val(1);
            }
        }


        // GrandGrid.Utilities.ResetGrid(true, "grdGIN");
        $("#divData").data("GINList", StockTransfer.GINList);
        GrandGrid.MakeGrid($("#grdGIN"), 0, StockTransfer.GINList);

        // GrandGrid.Utilities.ResetGrid(true, "grdPO");
        $("#divData").data("POList", StockTransfer.POList);
        GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
        // Clear  grdPR grid and Bind grid as null
        //GrandGrid.Utilities.ResetGrid(true, "grdPR");
        $("#divData").data("PRList", StockTransfer.PRList);
        GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
        // Clear  grdAdditionalItemList grid and Bind grind as null
        // GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
        $("#divData").data("AllocatedQtyList", StockTransfer.AllocatedAdditionalList);
        GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);

        FillGRNStore(StockTransferObj.root.SFH_DEPT, 0);
        FillCompany(StockTransferObj.root.SFH_COMPANY);
        $("input[id$=MCM_TO_DT]").val(StockTransferObj.root.SFH_PK);
        if (StockTransferObj.root.SFH_NO == null || StockTransferObj.root.SFH_NO == "") {
            $("[id$=SFH_NO]").html("[NEW]");
        }
        else {
            $("[id$=SFH_NO]").html(StockTransferObj.root.SFH_NO);
        }
        $("[id$=SFH_PK]").val(StockTransferObj.root.SFH_PK);
        $("input[id$=SFH_DATE]").val(StockTransferObj.root.SFH_DATE);

        $("[id$=LastModDate]").val(StockTransferObj.root.SFH_MOD_DT);
        FillAdditionalItem(0);
        $("[id$=AddToList]").hide();
        $("[id$=AddPRDtlsToList]").hide();
        $("#divPO").show();


        //$("#divSearch").show();
        $("#searchwrap").show();
        $("#imgGINHide").hide();
        $("#imgGINShow").show();

        $("#imgPOHide").hide();
        $("#imgPOShow").show();

        if (StockTransfer.PRList.length > 0) {
            $("#imgPRHide").hide();
            $("#imgPRShow").show();
            $("#divPR").show();
        }
        else {
            $("#imgPRHide").show();
            $("#imgPRShow").hide();
            $("#divPR").hide();
        }

        if (StockTransfer.AllocatedAdditionalList.length > 0) {
            $("#imgADNLHide").hide();
            $("#imgADNLShow").show();
            $("#divAdnlDtls").show();
        }
        else {
            $("#imgADNLHide").show();
            $("#imgADNLShow").hide();
            $("#divAdnlDtls").hide();
        }



    }
    else {
        // Show Alert message for GIn Already Assigned, in the draft case
        StockTransferDtlsObj = StockTransferObj;
        GrandGrid.Utilities.ResetGrid(true, "grdGIN");
        if ($("[id$=ViewStatus]").val() == "1" && parseInt($("[id$=SFH_IS_EDIT]").val()) != 1) {
            CheckGinDraftAlreadySubmitToWfForViewStatus();
        }
        else {
            CheckGinDraftAlreadySubmitToWf();
        }
    }
}

function Popup() {
    ///<summary>Function used for popup for GIN Item Already Assigned in some other stock transfer</summary>
    $("#GinItemAlreadyUsedPopUp").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

}

function CheckGinDraftAlreadySubmitToWf() {
    $("#divAdnlDtls").hide();
    $("#divPR").hide();
    $("#divPO").hide();
    $("#divGIN").hide();
    ///<summary>Function used to show popup for GIN Item Already Assigned in some other stock transfer</summary>
    $("#GinItemAlreadyUsedPopUp").dialog("open");
    $("#GinItemAlreadyUsedPopUp").dialog({ width: 500, height: 200, resizable: true, title: StockTransfer.ConfirmationTitle });
    return false;
}

function CheckGinDraftAlreadySubmitToWfForViewStatus() {
    $("#divAdnlDtls").hide();
    $("#divPR").hide();
    $("#divPO").hide();
    $("#divGIN").hide();
    ///<summary>Function used to show popup for GIN Item Already Assigned in some other stock transfer</summary>
    GrandScriptUtils.ShowModal("Translate(GINDetailsAlreadyAssingedToST)", StockTransfer.InformationTtile, StockTransfer.Save);
    return false;
}


function FillGinDetailsForGinAlreadyUsedCase() {
    ///<summary>Function used to show GIN details if Some Gin used fro any other stock transfer</summary>
    if (StockTransferDtlsObj.root.GinList != undefined) {
        StockTransfer.GINList = StockTransferDtlsObj.root.GinList.GinListDetail;
        if (!($.isArray(StockTransfer.GINList))) {
            if (StockTransfer.GINList != undefined) {
                var objArray = StockTransfer.GINList;
                StockTransfer.GINList = new Array();
                StockTransfer.GINList.push(objArray);
            }
            else {
                var objArray = StockTransfer.GINList;
                StockTransfer.GINList = new Array();
            }
        }
    }
    else {
        StockTransfer.GINList = new Array();
        if (!($.isArray(StockTransfer.GINList))) {
            if (StockTransfer.GINList != undefined) {
                var objArray = StockTransfer.GINList;
                StockTransfer.GINList = new Array();
                StockTransfer.GINList.push(objArray);
            }
            else {
                var objArray = StockTransfer.GINList;
                StockTransfer.GINList = new Array();
            }
        }
    }
    GrandGrid.Utilities.ResetGrid(true, "grdGIN");
    $("#divData").data("GINList", StockTransfer.GINList);
    GrandGrid.MakeGrid($("#grdGIN"), 0, StockTransfer.GINList);
    FillGRNStore(StockTransferDtlsObj.root.SFH_DEPT, 0);
    $("input[id$=MCM_TO_DT]").val(StockTransferDtlsObj.root.SFH_PK);
    $("[id$=SFH_NO]").html(StockTransferDtlsObj.root.SFH_NO);
    $("[id$=SFH_PK]").val(StockTransferDtlsObj.root.SFH_PK);
    $("input[id$=SFH_DATE]").val(StockTransferDtlsObj.root.SFH_DATE);
    $("input[id$=SFH_STATUS]").val(StockTransferDtlsObj.root.SFH_STATUS);
    FillAdditionalItem(0);
    $("[id$=AddToList]").show();
    $("[id$=AddPRDtlsToList]").hide();
    $("#divPO").hide();
    $("#divPR").hide();
    $("#divAdnlDtls").hide();
    //$("#divSearch").show();
    $("#searchwrap").show();
    // Clear  grdPO grid and Bind grid as null
    GrandGrid.Utilities.ResetGrid(true, "grdPO");
    StockTransfer.POList = new Array();
    $("#divData").data("GINList", StockTransfer.POList);
    GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
    // Clear  grdPR grid and Bind grid as null
    GrandGrid.Utilities.ResetGrid(true, "grdPR");
    StockTransfer.PRList = new Array();
    $("#divData").data("PRList", StockTransfer.PRList);
    GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
    // Clear  grdAdditionalItemList grid and Bind grind as null
    GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
    StockTransfer.AllocatedAdditionalList = new Array();
    $("#divData").data("AllocatedQtyList", StockTransfer.AllocatedAdditionalList);
    GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
    $("#GinItemAlreadyUsedPopUp").dialog("close");
    $("#divGIN").show();
    $("#imgGINHide").hide();
    $("#imgGINShow").show();
    //    $('#updateProgress').hide();
    return false;
}

//#region  ===========================================Check Alloc Qty And Additional Qty before Save ======================================
function CheckPRAllocateQtyWithPOAllocQty() {
    var status = true;
    if (StockTransfer.AllocationItemPR.length > 0 && StockTransfer.AllocationItem.length > 0) {
        for (var i in StockTransfer.AllocationItemPR) {
            for (var j in StockTransfer.AllocationItem) {
                if (StockTransfer.AllocationItemPR[i].Item == StockTransfer.AllocationItem[j].Item && StockTransfer.AllocationItemPR[i].POPk == StockTransfer.AllocationItem[j].POPk) {
                    if (parseFloat(StockTransfer.AllocationItemPR[i].AllocQty).toFixed(QtyDec) != parseFloat(StockTransfer.AllocationItem[j].AllocQty)) {
                        status = false;
                    }
                }
            }
        }
    }
    else {

    }
    return status;
}

//#region---------------------------------- Updation 25102011 ------------------------------------

// Method to Get AllocationItem Details From PO Grid
function GetAllocationItemDetailsFromPRList() {
    StockTransfer.AllocationItemPR = new Array();
    if (!($.isArray(StockTransfer.AllocationItemPR))) {
        if (StockTransfer.AllocationItemPR != undefined) {
            var objArray = StockTransfer.AllocationItemPR;
            StockTransfer.AllocationItemPR = new Array();
            StockTransfer.AllocationItemPR.push(objArray);
        }
        else {
            var objArray = StockTransfer.AllocationItemPR;
            StockTransfer.AllocationItemPR = new Array();
        }
    }
    var grdID;
    var allocateQtyIndex = 0;
    var itemIndx = 0;
    var allocateQty = 0;
    var item = 0;
    var itemExist = false;
    var ginDtlPk = 0;
    var poPk = 0;
    $("#grdPR tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        allocateQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRQty, grdID);

        if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
            allocateQty = $(this).find("td:eq(" + allocateQtyIndex + ") input").val();
        }
        else {
            allocateQty = GrandGrid.Utilities.GetColumnValue($(this), "ALLOCATE_PR_QTY", grdID);
        }
        itemIndx = GrandGrid.Utilities.GetColumnIndex($(this), "POR_ITEM", grdID);
        item = GrandGrid.Utilities.GetColumnValue($(this), "POR_ITEM", grdID);
        poPk = GrandGrid.Utilities.GetColumnValue($(this), "POD_PK", grdID);
        if (allocateQty == null || allocateQty == undefined || allocateQty == "") {
            allocateQty = 0;
        }
        if (item == null || item == undefined || item == "") {
            item = 0;
        }


        var allocateLocIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRLoc, grdID);
        var allocLoc = "";
        if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
            allocLoc = $(this).find("td:eq(" + allocateLocIndex + ") input").val();
        }
        else {
            allocLoc = GrandGrid.Utilities.GetColumnValue($(this), "SFD_LOCATION", grdID);
        }
        allocLoc = allocLoc == null || allocLoc == "undefined" ? "" : allocLoc;
        if (StockTransfer.AllocationItemPR.length > 0) {
            for (var i in StockTransfer.AllocationItemPR) {
                if (StockTransfer.AllocationItemPR[i].Item == item && StockTransfer.AllocationItemPR[i].POPk == poPk) {
                    StockTransfer.AllocationItemPR[i].AllocQty = parseFloat(StockTransfer.AllocationItemPR[i].AllocQty) + parseFloat(allocateQty);
                    StockTransfer.AllocationItemPR[i].SFD_LOCATION = allocLoc;
                    StockTransfer.STNO = $("[id$=SFH_NO]").html();
                    itemExist = true;
                }

            }
            // if item not avialable in the allocation List
            if (itemExist == false) {
                StockTransfer.AllocationItemPRObj = new Object();
                StockTransfer.AllocationItemPRObj.Item = item;
                StockTransfer.AllocationItemPRObj.POPk = poPk;
                StockTransfer.AllocationItemPRObj.AllocQty = parseFloat(allocateQty);
                StockTransfer.AllocationItemPRObj.SFD_LOCATION = allocLoc;
                StockTransfer.STNO = $("[id$=SFH_NO]").html();
                StockTransfer.AllocationItemPR.push(StockTransfer.AllocationItemPRObj);
            }
        }
        // if no items in Allocation List, add item as 
        else {
            StockTransfer.AllocationItemPRObj = new Object();
            StockTransfer.AllocationItemPRObj.Item = item;
            StockTransfer.AllocationItemPRObj.POPk = poPk;
            StockTransfer.AllocationItemPRObj.SFD_LOCATION = allocLoc;
            StockTransfer.AllocationItemPRObj.AllocQty = parseFloat(allocateQty);
            StockTransfer.STNO = $("[id$=SFH_NO]").html();
            StockTransfer.AllocationItemPR.push(StockTransfer.AllocationItemPRObj);
        }


    });


}

//#endregion




// Method to Chek Sum of Additional Item == Item Assigned in Additional Section
function CheckTotalAllocItem() {
    var status = true;
    var totalAdnItem = GetSumofTotalPOAdnlAlloc();
    var totalAllocAdnlItem = GetSumofTotalAdnlAllocItem();
    if (parseFloat(totalAdnItem).toFixed(QtyDec) != parseFloat(totalAllocAdnlItem).toFixed(QtyDec)) {
        status = false;
    }
    return status;
}

// Method to Get Sum of Additional Item From PO List
function GetSumofTotalPOAdnlAlloc() {
    var total = 0;
    for (var i in StockTransfer.AdditionalItem) {
        total = parseFloat(total) + parseFloat(StockTransfer.AdditionalItem[i].AdnlQty);
    }
    return total;
}
// Method to Get Sum of Alloc Additional Item From Assign Section
function GetSumofTotalAdnlAllocItem() {
    var total = 0;
    for (var i in StockTransfer.AllocatedAdditionalList) {
        total = parseFloat(total) + parseFloat(StockTransfer.AllocatedAdditionalList[i].ALLOCATE_ADDL_PR_QTY);
    }
    return total;
}

// Method to Check Additional Item Allocation Section is Match with Qty allocated as additional
function CheckAdditionalQtyWithPOAdnlQty() {
    var status = true;
    if (StockTransfer.AdditionalItem.length > 0 && StockTransfer.AllocatedAdditionalList.length > 0) {
        // for (var j in StockTransfer.AdditionalItem) {
        for (var i in StockTransfer.AllocatedAdditionalList) {
            var sumAdnlPO = GetSumofAdnlQtyForSelectedItemFromPO(StockTransfer.AllocatedAdditionalList[i].POR_ITEM);
            var sumAdnlQty = GetSumofAdnlQtyForSelectedItemFromAdnlSection(StockTransfer.AllocatedAdditionalList[i].POR_ITEM);
            if (parseFloat(sumAdnlPO) != parseFloat(sumAdnlQty)) {
                status = false;
            }
        }
        // }
    }
    else {
        if (StockTransfer.AdditionalItem.length > 0 && StockTransfer.AllocatedAdditionalList.length == 0) {
            var totalAdditionalItem = GetSumofAdnlQty();
            var additionalItemCount = 0;
            for (var i = 0; i < StockTransfer.AdditionalItem.length; i++) {
                if (StockTransfer.AdditionalItem[i].AdnlQty > 0) ++additionalItemCount;
            }

            if (parseFloat(totalAdditionalItem) > 0) {
                GrandScriptUtils.ShowModal("Translate(AddAllocationDetails)", StockTransfer.InformationTitle);
                status = false;
            }
            else if (additionalItemCount > 0) {
                if (StockTransfer.AllocatedAdditionalList.length != StockTransfer.AdditionalItem.length) {
                    GrandScriptUtils.ShowModal("Translate(AddAllocationDetails)", StockTransfer.InformationTitle);
                    status = false;
                }
            }
            else {

            }
        }
        else {
            GrandScriptUtils.ShowModal("Translate(NotAllocatePOAdditionalQty)", StockTransfer.InformationTitle);
            status = false;
        }
    }
    return status;
}

function GetSumofAdnlQtyForSelectedItemFromAdnlSection(item) {
    var total = 0;
    for (var i in StockTransfer.AllocatedAdditionalList) {
        if (StockTransfer.AllocatedAdditionalList[i].POR_ITEM == item) {
            total = parseFloat(total) + parseFloat(StockTransfer.AllocatedAdditionalList[i].ALLOCATE_ADDL_PR_QTY);
        }
    }
    return total;
}

function GetSumofAdnlQtyForSelectedItemFromPO(item) {
    var total = 0;
    for (var i in StockTransfer.AdditionalItem) {
        if (StockTransfer.AdditionalItem[i].Item == item) {
            total = parseFloat(total) + parseFloat(StockTransfer.AdditionalItem[i].AdnlQty);
        }
    }
    return total;
}
// Method to Get Sum of Additional Qty Total
function GetSumofAdnlQty() {
    var total = 0;
    for (var i in StockTransfer.AdditionalItem) {
        total = parseFloat(total) + parseFloat(StockTransfer.AdditionalItem[i].AdnlQty);
    }
    return total;
}


//#endregion ======================================= Check Alloc Qty And Additional Qty before Save ======================================

function CheckPRAllocation() {
    ///<summary>Function to Check PR Allocation , check PR Allocation value >= Balnce PR Quantity</summary>
    var status = true;
    if (StockTransfer.PRList.length > 0) {
        for (var i in StockTransfer.PRList) {
            // Get PR Allocation Quantity fro selected record
            var allocateQty = GetPREditDetailsFormGrid(i);
            // check Allocation quantity with PR balnce qty , if Allocation Qty greater , set status as false and return status
            if (parseFloat(StockTransfer.PRList[i].POR_QTY_BAL) < parseFloat(allocateQty)) {
                status = false;
            }
        }
    }
    return status;
}

function GetPREditDetailsFormGrid(indx) {
    ///<summary>Method to get Additional Iten and Allocation item from grid , when select Add to List Action in PO section</summary>
    var grdID;
    var allocateQtyIndex = 0;
    var allocDtls = new Array();
    var allocateQty = 0;
    // Read reach row from PR Grid, and get Allocated Quantity texbox value. and Chek balance quantity with CheckPRAllocation() function
    $("#grdPR tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        // if list index== grid index
        if (indx == index) {
            // get allocated qty textbox index and get value from textbox
            allocateQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRQty, grdID);
            if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
                allocateQty = $(this).find("td:eq(" + allocateQtyIndex + ") input").val();
            }
            else {
                allocateQty = GrandGrid.Utilities.GetColumnValue($(this), "ALLOCATE_PR_QTY", grdID);
            }
            //allocateQty = $(this).find("td:eq(" + allocateQtyIndex + ") input").val();
            // check allocated qty =0 or null or empty, then asssigned it as 0
            if (allocateQty == null || allocateQty == undefined || allocateQty == "") {
                allocateQty = 0;
            }
        }
    });
    // retrn allocated PR quantity fro selected index
    return allocateQty;
}

function GetPRItemDetails() {
    ///<summary>Method to Read Allocated Quanity from the PR Grid textBox && Function to Get PR Details TO Save</summary>
    for (var i in StockTransfer.PRList) {
        // get Allocated PR Quantity from PR Grid textBox
        var allocQty = GetAllocQtyFormPRGrid(i);
        // assign PR allocated qty selected PR List 
        StockTransfer.PRList[i].ALLOCATE_PR_QTY = allocQty;
        var allocLoc = "";
        var QALotnoPRColIndex;
        var QALotnoPR = 0;
        $("#grdPR tr:has(td)").each(function (index) {
            var grdID = $(this).parents("table:first").attr("id");
            if (index == i) {
                var allocateLocIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRLoc, grdID);

                if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
                    allocLoc = $(this).find("td:eq(" + allocateLocIndex + ") input").val();
                }
                else {
                    allocLoc = GrandGrid.Utilities.GetColumnValue($(this), "SFD_LOCATION", grdID);
                }
                allocLoc = allocLoc == null || allocLoc == "undefined" ? "" : allocLoc;

                //***********************Adding QA Lot no*************************************************
                QALotnoPRColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.QALotNo, grdID);
                if (QALotnoPRColIndex) {
                    QALotnoPR = $(this).find("td:eq(" + QALotnoPRColIndex + "): input[type=text]").val();
                }
                QALotnoPR = QALotnoPR == null || QALotnoPR == "undefined" ? "" : QALotnoPR;
                //*******************************End******************************************
            }
        });
        StockTransfer.PRList[i].SFD_LOCATION = allocLoc;
        StockTransfer.STNO = $("[id$=SFH_NO]").html();
        StockTransfer.PRList[i].SFD_QA_LOT_NO = QALotnoPR;
    }
    // return PR List
    return StockTransfer.PRList;
}

function GetAllocQtyFormPRGrid(indx) {
    ///<summary>Function to Get Allocation Item from PR Grid textbox</summary>
    var grdID;
    allocIndex = 0;
    var allocQty = 0;
    // Read PR item row one by one
    $("#grdPR tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        // check list index== pr grid index
        if (indx == index) {
            // get selected rows, allocation index, and get allocation qty from thats colums textbox
            allocIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRQty, grdID);
            if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
                allocQty = $(this).find("td:eq(" + allocIndex + ") input").val();
            }
            else {
                allocQty = GrandGrid.Utilities.GetColumnValue($(this), "ALLOCATE_PR_QTY", grdID);
            }
            //allocQty = $(this).find("td:eq(" + allocIndex + ") input").val();
            // check allocated qty =0 or null or empty, then asssigned it as 0
            if (allocQty == null || allocQty == undefined || allocQty == "") {
                allocQty = 0;
            }
        }
    });
    // return allocqty
    return allocQty;
}

//#endregion

//#region======================================   Save Details =================================================== 

function SavePage(command) {
    ///<summary> Method to Save Store Transfer Details</summary>
    $.get(StockTransfer.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(StockTransfer.SessionExpired, StockTransfer.Confirmation, StockTransfer.LOGOUT, true);
            result = false;
        }
        else {
            $("[id$=SFH_COMPANY]").attr("disabled", false);
            $.get(StockTransfer.InventoryLockCheckingURL + $("[id$=SFH_DATE]").val() + "&Module=2", function (data) {
                if (data != null && data.length > 0) {
                    if (parseInt(data[0]) == 0) {
                        SaveStockTransfer(command); //For developer convenience,all codes are just placed under a new function .
                    }
                    else {
                        var Err_TranslockedMsg = StockTransfer.ErrTransLockedMsg + StockTransfer.ValueEmpty + data[1];
                        GrandScriptUtils.ShowModal(Err_TranslockedMsg, StockTransfer.InformationTtile);
                    }
                }
            });
        }
    });
    return false;
}

function SaveStockTransfer(command) {
    RemoveAllValidations();
    AddValidation(1);
    // Get PR Allocation LIst for Check Details With PO
    GetAllocationItemDetailsFromPRList();
    // Check Documentation is valid or not
    if ($(document.forms[0]).valid()) {
        //Showing validation for Future Date selection
        if ($("[id$=hdfIsContFutureDate]").val() != "1") {
            var RetVal = CompareDate($("[id$=SFH_DATE]").val(), $("[id$=hdfCurrentDate]").val());
            if (RetVal == 1) {
                //                ShowFutureDate(command);
                blockFutureDate();
                return false;
            }
        }
        // Check PR Allocation
        if (CheckPRAllocation()) {
            // check PR allocation Qty With PO Allocation Qty
            if (CheckPRAllocateQtyWithPOAllocQty()) {
                // Check Additional Qty Gird details with PO Additional Quantity
                if (CheckAdditionalQtyWithPOAdnlQty()) {
                    // Check Total items Allocated or not
                    if (CheckTotalAllocItem()) {
                        // Create Selected GINpk Dtls List and ssigned to GINPKList Hiddenfiled as JSON
                        var GINItemPkList = new Object();
                        GINItemPkList.GinPkDtl = new Array();
                        GINItemPkList.GinPkDtl = StockTransfer.GINPKList;
                        $("[id$=GINPKList]").val("$" + JSON.stringify(GINItemPkList));
                        // Create Selected POpk Dtls List and ssigned to POPKList Hiddenfiled as JSON
                        var POItemPkList = new Object();
                        POItemPkList.POPkDtl = new Array();
                        POItemPkList.POPkDtl = StockTransfer.POPKList;
                        $("[id$=POPKList]").val("$" + JSON.stringify(POItemPkList));
                        // Create Selected PO List and ssigned to POPKList Hiddenfiled as JSON
                        var POListDtls = new Object();
                        POListDtls.POsTransferQty = new Array();
                        POListDtls.POsTransferQty = StockTransfer.POList;
                        $("[id$=POList]").val("$" + JSON.stringify(POListDtls));
                        // Create Selected Total Item List  and ssigned to TotalItem Hiddenfiled as JSON
                        var TotalItemList = new Object();
                        TotalItemList.TotalItemDtls = new Array();
                        TotalItemList.TotalItemDtls = StockTransfer.TotalItem;
                        $("[id$=TotalItem]").val("$" + JSON.stringify(TotalItemList));
                        // Create Selected Additional Item  List  and ssigned to AdditionalItem Hiddenfiled as JSON
                        var AdditionalItemList = new Object();
                        AdditionalItemList.AdditionalItemDtls = new Array();
                        AdditionalItemList.AdditionalItemDtls = StockTransfer.AdditionalItem;
                        $("[id$=AdditionalItem]").val("$" + JSON.stringify(AdditionalItemList));
                        // Create Selected Allocation item  List  and ssigned to AllocationItem Hiddenfiled as JSON
                        var AllocationItemList = new Object();
                        AllocationItemList.AllocationItemDtls = new Array();
                        AllocationItemList.AllocationItemDtls = StockTransfer.AllocationItem;
                        $("[id$=AllocationItem]").val("$" + JSON.stringify(AllocationItemList));
                        AllocationItem = new Array();
                        //## PR List
                        var PRListDtls = new Object();
                        PRListDtls.PRsTransferQty = new Array();
                        PRListDtls.PRsTransferQty = GetPRItemDetails();
                        $("[id$=PRList]").val("$" + JSON.stringify(PRListDtls));
                        // ## Allocated Additional List
                        var AddlList = new Object();
                        AddlList.AddlDetail = new Array();
                        AddlList.AddlDetail = StockTransfer.AllocatedAdditionalList;
                        $("[id$=AllocatedQtyList]").val("$" + JSON.stringify(AddlList));
                        $("[id$=WKF_FLAG]").val("0");
                        // Check Save as Draft or not, if save as workflow, send action id else actionId as 0
                        if (command != StockTransfer.Draft) {
                            $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
                            if ($("[id$=ReferenceID]").val() == "0") {
                                $("[id$=WKF_FLAG]").val("1");
                            }
                        }
                        else
                            $("[id$=ActionID]").val(StockTransfer.ValueZero);  // save only.
                        // Check Page satisfy aall validations
                        if ($(document.forms[0]).valid()) {
                            $("[id$=SFH_DEPT]").attr("disabled", false);
                            $("[id$=SFH_DATE]").attr("disabled", false);
                            $("[id$=SFH_COMPANY]").attr("disabled", false);
                            var jSonString = GrandScriptUtils.FormToJsonString(false);
                            var SaveMessageWithStockTransfer = StockTransfer.ValueEmpty;

                            if ($("[id$=SubmitFlag]").val() == "0")
                                $("[id$=SubmitFlag]").val('1')
                            else
                                return false;

                            $.post(StockTransfer.SaveStockTransferUrl + "&LastModDate=" + $("[id$=LastModDate]").val(), jSonString, function (data) {
                                // Check details saved successfully or not
                                if (parseInt(data[0]) > 0) {
                                    // Check Save type is draft
                                    if (command == StockTransfer.Draft) {
                                        // show save success message and redirect to listing page
                                        SaveMessageWithStockTransfer = StockTransfer.STSavedMessage;
                                        if ($("[id$=AST_DOC_MODE]").val() == "1")
                                            SaveMessageWithStockTransfer = StockTransfer.SaveMsg1 + StockTransfer.ValueEmpty + data[1] + StockTransfer.ValueEmpty + StockTransfer.SaveMsg2;
                                        GrandScriptUtils.ShowModal(SaveMessageWithStockTransfer, StockTransfer.InformationTtile, StockTransfer.Save);
                                    }
                                    // If action - WorkFlow Save
                                    else {
                                        // assign application id, appNo, and do action for btnSubmitDelegate
                                        $("[id$=hdfAppID]").val(data[0]);
                                        $("[id$=AppNo]").val(data[1]);
                                        //                                        $("[id$=btnSubmitDelegate]").click();
                                        SaveWorkFlow();
                                        //window.location = "StockTransferList.aspx";
                                    }
                                }
                                // if any error occur , show alert message
                                else if (parseInt(data[0]) == -1) {
                                    $("[id$=SFH_DEPT]").attr("disabled", true);
                                    $("[id$=SFH_DATE]").attr("disabled", true);
                                    GrandScriptUtils.ShowModal(StockTransfer.ActionFailedTryAgainMsg.fontcolor("red"), StockTransfer.InformationTitle);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                // if any concurrency occur, show error message
                                else if (parseInt(data[0]) == -2) {
                                    GrandScriptUtils.ShowModal(StockTransfer.SaveMsg1 + StockTransfer.ValueEmpty + data[1] + StockTransfer.ValueEmpty + "Translate(EditUsedByAnotherUser)", StockTransfer.InformationTtile, "save");
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                // if any concurrency occur, show alert message
                                else if (parseInt(data[0]) == -3) {
                                    $("[id$=SFH_DEPT]").attr("disabled", true);
                                    $("[id$=SFH_DATE]").attr("disabled", true);
                                    GrandScriptUtils.ShowModal(StockTransfer.ItemAlreadyAddedMsg);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                //Stock checking
                                else if (parseInt(data[0]) == -4) {
                                    var arr = data[1].split(",");
                                    var s = String.format(MaterialIssue.SpecifiedQuantityNotAvailableInStock, arr[0], arr[1]);
                                    GrandScriptUtils.ShowModal(s, MaterialIssue.MessageBoxTitle);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -10) {
                                    $("[id$=SFH_DEPT]").attr("disabled", true);
                                    // $("[id$=SFH_DATE]").attr("disabled", true);
                                    //                                    GrandScriptUtils.ShowModal(StockTransfer.CannotReceivePriorDateSendReceive, StockTransfer.MessageBoxTitle);
                                    fnConfirmStockValueChange(command);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -31) {
                                    var s = String.format(StockTransfer.StockTransferAlreadyDone, $("[id$=SFH_DATE]").val());
                                    GrandScriptUtils.ShowModal(s, StockTransfer.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -32) {
                                    var s = String.format(StockTransfer.StockAdjustmentIsAlreadyDone, $("[id$=SFH_DATE]").val());
                                    GrandScriptUtils.ShowModal(s, StockTransfer.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -35) {
                                    var msgtxt = String.format(StockTransfer.ErrModifyStockAdmission, "'" + data[1] + "'")
                                    GrandScriptUtils.ShowModal(msgtxt.fontcolor("red"), StockTransfer.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                // if any other type errors occur, show alert message
                                else {
                                    $("[id$=SFH_DEPT]").attr("disabled", true);
                                    $("[id$=SFH_DATE]").attr("disabled", true);
                                    GrandScriptUtils.ShowModal(StockTransfer.ActionFailedTryAgainMsg.fontcolor("red"), StockTransfer.InformationTitle);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                            });
                        }
                    }
                    else {
                        GrandScriptUtils.ShowModal(StockTransfer.AllocationErrorMsg, StockTransfer.InformationTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                }
                else {
                    GrandScriptUtils.ShowModal(StockTransfer.AllocationMisMatchErrorMsg, StockTransfer.InformationTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
            }
            else {
                GrandScriptUtils.ShowModal(StockTransfer.InvalidAllocationInPRMsg, StockTransfer.InformationTitle);
                $("[id$=SubmitFlag]").val('0')
            }
        }
        else {
            GrandScriptUtils.ShowModal(StockTransfer.PRAllocationWithPOBalanceErrorMsg, StockTransfer.InformationTitle);
            $("[id$=SubmitFlag]").val('0')
        }
    }
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    //    var msg = StockTransfer.SaveMsg1 + StockTransfer.ValueEmpty + $("[id$=AppNo]").val() + StockTransfer.ValueEmpty + StockTransfer.SubmitMessage;
    //    GrandScriptUtils.ShowModal(msg, StockTransfer.InformationTtile, StockTransfer.Save);
    var msg = "";
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        msg = StockTransfer.SaveMsg1 + StockTransfer.ValueEmpty + $("[id$=AppNo]").val() + StockTransfer.ValueEmpty + StockTransfer.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, StockTransfer.InformationTtile, StockTransfer.INBOX);
    } else {
        msg = StockTransfer.SaveMsg1 + StockTransfer.ValueEmpty + $("[id$=AppNo]").val() + StockTransfer.ValueEmpty + StockTransfer.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, StockTransfer.InformationTtile, StockTransfer.Save);
    }
}

function BindLocationAuto() {
    var curValue = "";
    $("#grdPR tr:has(td)").each(function (index) {
        if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
            curValue = $("[id$=" + "txtPRAllocLoc_" + index + "]").val();
            GrandScriptUtils.MakeAutoComplete("txtPRAllocLoc_" + index, StockTransfer.InventoryLocationGet + $("[id$=hdfDeptID]").val() + "&Type=" + StockTransfer.InventoryDeptType + "&Category=" + StockTransfer.InventoryLocationStore, "hdfLocationPK", false, false, "BizUnitPk", false);
            $("[id$=" + "txtPRAllocLoc_" + index + "]").val(curValue);
        }

    });
}

//#endregion =================================== Save Details =====================================================

//#region =======================================================Grdi Action =================================================
//###
function AfterGridBind(grdID) {
    ///<summary>Function to Set Grid After Binding details</summary>
    // Check Action From PO Grid
    if (grdID == "grdPO") {
        var allocationPOColIndex = 0;
        var allocationPO = StockTransfer.EMPTYVALUE;
        var additionalPOColIndex = 0;
        var additionalPO = StockTransfer.EMPTYVALUE;
        var poQtyIndx = 0;
        var adnlPOQtyIndx = 0;
        var balPOQtyIndx = 0;
        var balAdnlPOQtyIndx = 0;
        var balanceAddlQty = 0;
        // Check Details In View Mode
        if ($("[id$=ViewStatus]").val() == StockTransfer.ValueOne && parseInt($("[id$=SFH_IS_EDIT]").val()) != 1) {
            //Hiding the template field of PO Grid
            $("#grdPO").find("th:first").find("input[type=checkbox]").attr("disabled", true);
            $("#grdPO").find("tr").each(function () {
                $(this).find("td:first").find("input[type=checkbox]").attr("disabled", true);
            });
        }
        //Selected Quantity
        var GINSelectedItemsList = $.parseJSON($("[id$=GINSelectedItems]").val());
        var index = 0;
        var ItemPk = 0;
        var qtyApproved = 0;
        var qtyAllocated = 0;
        var additionalQty = 0;
        var balancePOQty = 0;

        // Read Each row of PO Gird
        $("#grdPO tr:has(td)").each(function (index) {
            // Check POEdit Value  is 1, then Allocation, and additional PO Columns in Grid as Entry Mode, display with textbox
            if ($("[id$=POEdit]").val() == StockTransfer.ValueOne) {
                additionalPOColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOAddnlQty, grdID);
                additionalPO = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOAddnlQty, grdID);
                allocationPOColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOQty, grdID);
                allocationPO = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOQty, grdID);
                balancePOQty = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.BalancePoQty, grdID);
                ItemPk = GrandGrid.Utilities.GetColumnValue($(this), "POR_ITEM", grdID);
                //                for (var i = 0; i < GINSelectedItemsList.length; i++) {
                //                    if (GINSelectedItemsList[i].ginItem == ItemPk) {
                //                        qtyApproved = GINSelectedItemsList[i].qtyApproved
                //                        break;
                //                    }
                //                }
                //              //  qtyApproved = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_APPROVED", grdID); 
                //                balanceAddlQty = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.BalanceAddlQty, grdID);

                //                qtyAllocated = parseFloat(qtyApproved - balanceAddlQty);
                //                additionalQty = parseFloat(qtyApproved - (qtyApproved - balanceAddlQty));
                //                //additionalQty = parseFloat(balanceAddlQty);
                //                if ((qtyAllocated + additionalQty) > balancePOQty && additionalQty > qtyAllocated) {
                //                    qtyAllocated = balancePOQty;
                //                    additionalQty = qtyApproved - qtyAllocated;
                //                }

                //                if (allocationPO == 0)
                //                    allocationPO = parseFloat(qtyAllocated).toFixed(QtyDec);                
                if (allocationPOColIndex != null) {
                    allocationPO = parseFloat(allocationPO).toFixed(QtyDec);
                    $(this).find("td:eq(" + allocationPOColIndex + ")").html("");
                    $(this).find("td:eq(" + allocationPOColIndex + ")").append("<input type=\"text\" id=\"txtPOAllocation_" + index + "\" class=\"numeric input-w63\" value=\"" + allocationPO + "\" width=\"90%\" maxlength=\"12\" tabindex=\"8\"  onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(), this);\"  onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"  />");
                }

                if (additionalPOColIndex != null) {
                    additionalPO = parseFloat(additionalPO).toFixed(QtyDec);
                    $(this).find("td:eq(" + additionalPOColIndex + ")").html("");
                    $(this).find("td:eq(" + additionalPOColIndex + ")").append("<input type=\"text\" id=\"txtPOAdditional_" + index + "\" class=\"numeric input-w63\"  value=\"" + additionalPO + "\" width=\"90%\" maxlength=\"12\" tabindex=\"8\" onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(), this);\"  onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\" />");
                }
                index++;
                //Number Formating
                var poQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "QtyOrdered", grdID);
                var adnlPOQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "QtyAdditional", grdID);
                var balPOQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "BAL_PO_QTY", grdID);
                var balAdnlPOQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "BAL_PO_ADDL_QTY", grdID);

                var poQty = GrandGrid.Utilities.GetColumnValue($(this), "QtyOrdered", grdID);
                var adnlPOQty = GrandGrid.Utilities.GetColumnValue($(this), "QtyAdditional", grdID);
                var balPOQty = GrandGrid.Utilities.GetColumnValue($(this), "BAL_PO_QTY", grdID);
                var balAdnlPOQty = GrandGrid.Utilities.GetColumnValue($(this), "BAL_PO_ADDL_QTY", grdID);
                if (poQtyIndx != null) {
                    $(this).find("td:eq(" + poQtyIndx + ")").html(numberWithCommas(parseFloat(poQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (adnlPOQtyIndx != null) {
                    $(this).find("td:eq(" + adnlPOQtyIndx + ")").html(numberWithCommas(parseFloat(adnlPOQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (balPOQtyIndx != null) {
                    $(this).find("td:eq(" + balPOQtyIndx + ")").html(numberWithCommas(parseFloat(balPOQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (balAdnlPOQtyIndx != null) {
                    $(this).find("td:eq(" + balAdnlPOQtyIndx + ")").html(numberWithCommas(parseFloat(balAdnlPOQty).toFixed(QtyDec))).css("text-align", "right");
                }
                //end
            }
            else {

                additionalPOColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOAddnlQty, grdID);
                allocationPOColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePOQty, grdID);
                var poQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "QtyOrdered", grdID);
                var adnlPOQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "QtyAdditional", grdID);
                var balPOQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "BAL_PO_QTY", grdID);
                var balAdnlPOQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "BAL_PO_ADDL_QTY", grdID);

                var allocationPO = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOQty, grdID);
                var additionalPO = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePOAddnlQty, grdID);
                var poQty = GrandGrid.Utilities.GetColumnValue($(this), "QtyOrdered", grdID);
                var adnlPOQty = GrandGrid.Utilities.GetColumnValue($(this), "QtyAdditional", grdID);
                var balPOQty = GrandGrid.Utilities.GetColumnValue($(this), "BAL_PO_QTY", grdID);
                var balAdnlPOQty = GrandGrid.Utilities.GetColumnValue($(this), "BAL_PO_ADDL_QTY", grdID);
                balAdnlPOQty = parseFloat(balAdnlPOQty) < 0 ? 0.00 : balAdnlPOQty;

                if (allocationPOColIndex != null) {
                    $(this).find("td:eq(" + allocationPOColIndex + ")").html(numberWithCommas(parseFloat(allocationPO).toFixed(QtyDec))).css("text-align", "right");
                }
                if (additionalPOColIndex != null) {
                    $(this).find("td:eq(" + additionalPOColIndex + ")").html(numberWithCommas(parseFloat(additionalPO).toFixed(QtyDec))).css("text-align", "right");
                }
                if (poQtyIndx != null) {
                    $(this).find("td:eq(" + poQtyIndx + ")").html(numberWithCommas(parseFloat(poQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (adnlPOQtyIndx != null) {
                    $(this).find("td:eq(" + adnlPOQtyIndx + ")").html(numberWithCommas(parseFloat(adnlPOQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (balPOQtyIndx != null) {
                    $(this).find("td:eq(" + balPOQtyIndx + ")").html(numberWithCommas(parseFloat(balPOQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (balAdnlPOQtyIndx != null) {
                    $(this).find("td:eq(" + balAdnlPOQtyIndx + ")").html(numberWithCommas(parseFloat(balAdnlPOQty).toFixed(QtyDec))).css("text-align", "right");
                }
            }
        });
        var poPk = 0;
        var ginPk = 0;
        // Read each row of Po, to set checked as true , after in view mode / edit mode
        $("#grdPO tr:has(td)").each(function (index) {
            // check Selected PO Pk list count>o
            if (StockTransfer.POPKList.length > 0) {
                for (var i in StockTransfer.POPKList) {
                    poPk = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PODetailsPk, grdID);
                    // check grid row popk == selected PO POPK from lisr 
                    if (StockTransfer.POPKList[i].xmlPK == poPk) {
                        // Set checkbox as checked
                        $(this).find("td:first").find("input[type=checkbox]").attr("checked", "checked");
                    }
                }
            }
        });
    }
    // Check Action from PR Gird
    else if (grdID == "grdPR") {
        //QaLotno       
        var QaLotNoColIndex = 0;
        var QaLotNo = StockTransfer.EMPTYVALUE;
        var allocationPRColIndex = 0;
        var allocationPR = "";
        var store = "";
        var prNo = "";
        var storeIndx = 0;
        var prNoIndx = 0;
        var location = "";
        var locationIndex = 0;
        var allocationPRIndex = 0;
        var balIndex = 0;
        var balQty = 0;
        var poQtyIndex = 0;
        var poQty = 0;
        var porQtyBalance = 0;
        var podPK = 0;
        var selectedItemsList = $.parseJSON($("[id$=POPKList]").val());
        // Read each row of PR Grid
        $("#grdPR tr:has(td)").each(function (index) {
            //QALOt NO
            QaLotNoColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.QALotNo, grdID);
            QaLotNo = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.QALotNo, grdID);

            prNoIndx = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.PRHeaderNo, grdID);
            storeIndx = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.PRDeptName, grdID);
            store = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PRDeptName, grdID);
            prNo = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PRHeaderNo, grdID);
            location = GrandGrid.Utilities.GetColumnValue($(this), "SFD_LOCATION", grdID);
            locationIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SFD_LOCATION", grdID);
            // check PR item is Additional item, then store not avialable, then set td as empty
            if (store == "null") {
                $(this).find("td:eq(" + storeIndx + ")").html("");
            }
            if (location == "null") {
                $(this).find("td:eq(" + locationIndex + ")").html("");
            }
            if (QaLotNo == 'undefined' || QaLotNo == 'null') {
                $(this).find("td:eq(" + QaLotNoColIndex + ")").html("");
            }
            // check PR item is Additional item, then PR Number not avialable, then set td as empty
            if (prNo == "null") {
                $(this).find("td:eq(" + prNoIndx + ")").html("");
            }
            // check PREdit value =1 and details not in view mode, then PR Allocation qty as entry format , with textbox
            allocationPRIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRQty, grdID);
            balIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POR_QTY_BAL", grdID);
            balQty = GrandGrid.Utilities.GetColumnValue($(this), "POR_QTY_BAL", grdID);
            poQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POR_QTY_ORDERED", grdID);
            poQty = GrandGrid.Utilities.GetColumnValue($(this), "POR_QTY_ORDERED", grdID);
            if (($("[id$=PREdit]").val() == StockTransfer.ValueOne && $("[id$=ViewStatus]").val() == StockTransfer.ValueZero) || parseInt($("[id$=SFH_IS_EDIT]").val()) == 1) {
                allocationPRColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRQty, grdID);
                allocationPR = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePRQty, grdID);
                if (allocationPRColIndex != null) {
                    //26-02-2014 
                    if (allocationPR == 0) {

                        porQtyBalance = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PorQtyBalance, grdID);
                        podPK = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.PODetailsPk, grdID);
                        if (selectedItemsList != null)
                            for (var i = 0; i < selectedItemsList.length; i++) {
                                if (selectedItemsList[i].POD_PK == podPK) {
                                    if (selectedItemsList[i].POQty >= porQtyBalance) {
                                        allocationPR = parseFloat(porQtyBalance).toFixed(QtyDec);
                                        selectedItemsList[i].POQty = selectedItemsList[i].POQty - porQtyBalance;
                                    }
                                    else {
                                        allocationPR = parseFloat(selectedItemsList[i].POQty).toFixed(QtyDec);
                                        selectedItemsList[i].POQty = 0;
                                    }

                                }
                            }


                    }
                    $(this).find("td:eq(" + allocationPRColIndex + ")").html("");
                    $(this).find("td:eq(" + allocationPRColIndex + ")").append("<input type=\"text\" class=\"numeric input-w63\" id=\"txtPRAllocation_" + index + "\"  value=\"" + parseFloat(allocationPR).toFixed(QtyDec) + "\" width=\"70%\" maxlength=\"12\" tabindex=\"8\" onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(), this);\" onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"   />");
                }
                var allocationPRLocIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRLoc, grdID);
                var allocationPRLoc = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePRLoc, grdID);
                if (allocationPRLocIndex != null) {
                    allocationPRLoc = allocationPRLoc == null || allocationPRLoc == "undefined" ? "" : allocationPRLoc;
                    $(this).find("td:eq(" + allocationPRLocIndex + ")").html("");
                    $(this).find("td:eq(" + allocationPRLocIndex + ")").append("<input type=\"text\" id=\"txtPRAllocLoc_" + index + "\"  value=\"" + allocationPRLoc + "\" width=\"70%\" maxlength=\"100\" tabindex=\"8\" />");


                }
                if (balIndex != null) {
                    $(this).find("td:eq(" + balIndex + ")").html(numberWithCommas(parseFloat(balQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (poQtyIndex != null) {
                    $(this).find("td:eq(" + poQtyIndex + ")").html(numberWithCommas(parseFloat(poQty).toFixed(QtyDec))).css("text-align", "right");
                }
                //*********QA Lot No***************************************************************************************
                if (QaLotNoColIndex != null) {
                    $(this).find("td:eq(" + QaLotNoColIndex + ")").html("");
                    if (QaLotNo == 'undefined' || QaLotNo == 'null') {
                        QaLotNo = "";
                    }
                    $(this).find("td:eq(" + QaLotNoColIndex + ")").append("<input type=\"text\" id=\"txtQALotNo_" + index + "\" class=\"numeric input-w97\" value=\"" + QaLotNo + "\" width=\"100%\" maxlength=\"50\" tabindex=\"8\"   />");
                }

            }
            else {
                if (allocationPRIndex != null) {
                    $(this).find("td:eq(" + allocationPRIndex + ")").css("text-align", "right");
                }
                if (balIndex != null) {
                    $(this).find("td:eq(" + balIndex + ")").html(numberWithCommas(parseFloat(balQty).toFixed(QtyDec))).css("text-align", "right");
                }
                if (poQtyIndex != null) {
                    $(this).find("td:eq(" + poQtyIndex + ")").html(numberWithCommas(parseFloat(poQty).toFixed(QtyDec))).css("text-align", "right");
                }

                allocationPRColIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRQty, grdID);
                allocationPR = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePRQty, grdID);
                if (allocationPRColIndex != null) {
                    $(this).find("td:eq(" + allocationPRColIndex + ")").html(numberWithCommas(parseFloat(allocationPR).toFixed(QtyDec))).css("text-align", "right");
                }
            }

        });
        BindLocationAuto();
    }
    // check Action from GIN grid
    else if (grdID == "grdGIN") {
        var ginPk = 0;
        var inspQtyIndx = 0;
        var colIndex = 0;
        var colVal;
        // Read each row of GIN, to set checked as true , after in view mode / edit mode
        $("#grdGIN tr:has(td)").each(function (index) {
            inspQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_APPROVED", grdID);
            inspQty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_APPROVED", grdID);
            if (inspQtyIndx != null) {
                $(this).find("td:eq(" + inspQtyIndx + ")").html(numberWithCommas(parseFloat(inspQty).toFixed(QtyDec))).css("text-align", "right");
                // $(this).find("td:eq(" + inspQtyIndx + ")").css("text-align", "right");
            }

            if (StockTransfer.GINPKList.length > 0) {
                for (var i in StockTransfer.GINPKList) {
                    ginPk = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.GINDetailsPK, grdID);
                    if (StockTransfer.GINPKList[i].xmlPK == ginPk) {
                        $(this).find("td:first").find("input[type=checkbox]").attr("checked", "checked");
                    }
                }
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QA_LOT_NO", grdID);
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QA_LOT_NO", grdID);
                if (colVal == 'undefined' || colVal == 'null') {
                    $(this).find("td:eq(" + colIndex + ")").html("");
                }
            }

        });
        // Check Details in View Mode
        if ($("[id$=ViewStatus]").val() == StockTransfer.ValueOne && parseInt($("[id$=SFH_IS_EDIT]").val()) != 1) {
            // enable the header checkbox and each row checkbox set as disabled
            $("#grdGIN").find("th:first").find("input[type=checkbox]").attr("disabled", true);
            $("#grdGIN").find("tr").each(function () {
                $(this).find("td:first").find("input[type=checkbox]").attr("disabled", true);
            });
        }
    }
    // check action from Additional Item List
    else if (grdID == "grdAdditionalItemList") {
        // check entry in View Mode
        var adnlItemQtyIndx = 0;
        var adnlItemQty = 0;
        if ($("[id$=ViewStatus]").val() == StockTransfer.ValueOne && parseInt($("[id$=SFH_IS_EDIT]").val()) != 1) {
            //Hiding the template field (Action section)
            $("#grdAdditionalItemList").find("tr").each(function () {
                $(this).find("td:last,th:last").hide();
            });

            $("#grdAdditionalItemList tr:has(td)").each(function (index) {
                adnlItemQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "ALLOCATE_ADDL_PR_QTY", grdID);
                //inspQty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_APPROVED", grdID);
                adnlItemQty = GrandGrid.Utilities.GetColumnValue($(this), "ALLOCATE_ADDL_PR_QTY", grdID);
                if (adnlItemQtyIndx != null) {
                    $(this).find("td:eq(" + adnlItemQtyIndx + ")").html(numberWithCommas(parseFloat(adnlItemQty).toFixed(QtyDec))).css("text-align", "right");
                }
                var allocationLocIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRLoc, grdID);
                var allocationLoc = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePRLoc, grdID);
                if (allocationLocIndex != null) {
                    allocationLoc = allocationLoc == "null" || allocationLoc == "undefined" ? "" : allocationLoc;
                    $(this).find("td:eq(" + allocationLocIndex + ")").html(allocationLoc);
                }
                var qaLotNoindex = GrandGrid.Utilities.GetColumnIndex($(this), "SFD_QA_LOT_NO", grdID);
                var qaLotNo = GrandGrid.Utilities.GetColumnValue($(this), "SFD_QA_LOT_NO", grdID);
                if (qaLotNoindex != null) {
                    qaLotNo = qaLotNo == "null" || qaLotNo == "undefined" ? "" : qaLotNo;
                    $(this).find("td:eq(" + qaLotNoindex + ")").html(qaLotNo);
                }
            });
        }
        else {
            $("#grdAdditionalItemList tr:has(td)").each(function (index) {
                adnlItemQtyIndx = GrandGrid.Utilities.GetColumnIndex($(this), "ALLOCATE_ADDL_PR_QTY", grdID);
                adnlItemQty = GrandGrid.Utilities.GetColumnValue($(this), "ALLOCATE_ADDL_PR_QTY", grdID);
                if (adnlItemQtyIndx != null) {
                    $(this).find("td:eq(" + adnlItemQtyIndx + ")").html(numberWithCommas(parseFloat(adnlItemQty).toFixed(QtyDec))).css("text-align", "right");
                }
                var allocationLocIndex = GrandGrid.Utilities.GetColumnIndex($(this), StockTransfer.AllocatePRLoc, grdID);
                var allocationLoc = GrandGrid.Utilities.GetColumnValue($(this), StockTransfer.AllocatePRLoc, grdID);
                if (allocationLocIndex != null) {
                    allocationLoc = (allocationLoc == "null" || allocationLoc == "undefined") ? "" : allocationLoc;
                    $(this).find("td:eq(" + allocationLocIndex + ")").html(allocationLoc);
                }
                var qaLotNoindex = GrandGrid.Utilities.GetColumnIndex($(this), "SFD_QA_LOT_NO", grdID);
                var qaLotNo = GrandGrid.Utilities.GetColumnValue($(this), "SFD_QA_LOT_NO", grdID);
                if (qaLotNoindex != null) {
                    qaLotNo = qaLotNo == "null" || qaLotNo == "undefined" ? "" : qaLotNo;
                    $(this).find("td:eq(" + qaLotNoindex + ")").html(qaLotNo);
                }
            });
        }

    }


}
//### 
function CheckBoxClickTrigger(gridID) {
    ///<summary>method to Trigger When Slect a CheckBox in a grid</summary>
    // Check action from PO Grid
    RemoveAllValidations(2);
    if (gridID == "grdPO") {
        if ($("[id$=POEdit]").val() == StockTransfer.ValueZero) {
            // Set POEdit value as 1 for , PO grid Allocation and Additional Allocation as entry mode with textbox
            $("[id$=POEdit]").val(1);
            // Fill Po List to PO Grid
            GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
            // show AddPRDtlsToList button
            $("[id$=AddPRDtlsToList]").show();
            // Clear all list except, GINlist, PO list and Selected GIN pk List
            StockTransfer.AdditionalItem = new Array();
            StockTransfer.AllocationItem = new Array();
            StockTransfer.PRList = new Array();
            // Reset PR grid with fill empty data
            GrandGrid.Utilities.ResetGrid(true, "grdPR");
            GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
            //StockTransfer.AdditionalItemList = new Array();
            // Reset Additional Allocation details grid with empty data
            StockTransfer.AllocatedAdditionalList = new Array();
            GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
            GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
            // Clear Items From Allocation section 
            var drpID = $("select[id$=ITEM]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true, 0);
            //Clear Store From Allocation section 
            var drpID = $("select[id$=STORE]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true, 0);
            // Clear Allocation Additonal Details Section
            ClearAllocationAdditionalDtls();
            // hide PR Details
            $("#divPR").hide();
            HideDetails(3);
            // Hide Additional Item Detials
            $("#divAdnlDtls").hide();
            HideDetails(4);
        }
    }
    // Check Action from GIN Grid
    if (gridID == "grdGIN") {
        if ($("[id$=GINEdit]").val() == StockTransfer.ValueZero) {
            //$("[id$=GINEdit]").val(1);
            //GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
            // shoe AddtoList Button
            $("[id$=AddToList]").show();
            $("[id$=AddPRDtlsToList]").hide();
            // clear all list items except GIN
            StockTransfer.TotalItem = new Array();
            StockTransfer.AdditionalItem = new Array();
            StockTransfer.AllocationItem = new Array();
            StockTransfer.POPKList = new Array();
            StockTransfer.GINPKList = new Array();
            StockTransfer.POList = new Array();
            // Reset PO Gid with empty data
            GrandGrid.Utilities.ResetGrid(true, "grdPO");
            GrandGrid.MakeGrid($("#grdPO"), 0, StockTransfer.POList);
            StockTransfer.PRList = new Array();
            //Reset PR Grid with empty Data
            GrandGrid.Utilities.ResetGrid(true, "grdPR");
            GrandGrid.MakeGrid($("#grdPR"), 0, StockTransfer.PRList);
            //StockTransfer.AdditionalItemList = new Array();
            StockTransfer.AllocatedAdditionalList = new Array();
            // Reset Additional Quanity item grid with empty data
            GrandGrid.Utilities.ResetGrid(true, "grdAdditionalItemList");
            GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
            // Clear Items From Allocation section 
            var drpID = $("select[id$=ITEM]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true, 0);
            //Clear Store From Allocation section 
            var drpID = $("select[id$=STORE]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true, 0);
            // Clear Allocation Additonal Details Section
            ClearAllocationAdditionalDtls();

            // hide po Section
            $("#divPO").hide();
            HideDetails(2);
            // Hide PR Section
            $("#divPR").hide();
            HideDetails(3);
            // hide Additional Details
            $("#divAdnlDtls").hide();
            HideDetails(4);
            // Set POEdit value as as 1, to Set PO 
            $("[id$=POEdit]").val(StockTransfer.ValueOne);
        }
    }
}
//#endregion

//#region ==============================================Utitltiy Function (Make Numeric And Modal OK) ===========================================

function MakeNumeric(event, AllowDot, value, control) {
    var Qty = 0;
    var qtyArray;
    var keyCode = event.keyCode ? event.keyCode : event.which;
    if (keyCode != 39 && keyCode != 37 && keyCode != 8 && keyCode != 9 && keyCode != 46) {
        var regx = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
        if (!(regx.test(value))) {
            qtyArray = String(value).split(StockTransfer.DOTVALUE);
            if (qtyArray.length > 1) {
                if (qtyArray[1] != StockTransfer.EMPTYVALUE && qtyArray[1] != "0") {
                    if (qtyArray[0].length <= 8 && qtyArray[1].length <= parseInt(QtyDec)) {
                    }
                    else {
                        if (qtyArray[0].length > 8) {
                            $("input[id$=" + control.id + "]").val(qtyArray[0].slice(0, qtyArray[0].length - 1) + StockTransfer.DOTVALUE + qtyArray[1]);
                        }
                        else {
                            $("input[id$=" + control.id + "]").val(qtyArray[0] + StockTransfer.DOTVALUE + qtyArray[1].slice(0, qtyArray[1].length - 1));
                        }

                    }
                }
                else {
                    if (qtyArray[0] == StockTransfer.EMPTYVALUE && qtyArray[1] == StockTransfer.EMPTYVALUE) {
                        $("input[id$=" + control.id + "]").val(StockTransfer.DOTVALUE);
                    }
                    else if (qtyArray[0] == StockTransfer.EMPTYVALUE && qtyArray[1] != StockTransfer.EMPTYVALUE) {
                        $("input[id$=" + control.id + "]").val(StockTransfer.DOTVALUE + qtyArray[1]);
                    }
                    else {

                        $("input[id$=" + control.id + "]").val(value);
                    }
                }
            }
            else {
                if (qtyArray[0].length > 8) {
                    $("input[id$=" + control.id + "]").val(qtyArray[0].slice(0, qtyArray[0].length - 1));
                }
                else {
                    $("input[id$=" + control.id + "]").val(value);
                }
            }
        }
    }
    else {

    }

}

function ModalOk(command) {
    ///<summary>Function to do action after Modal POPUp Ok Action</summary>
    switch (command) {
        case StockTransfer.Delete:
            DeleteAdditionalItemDtls();
            ClearAllocationAdditionalDtls();
            break;
        case StockTransfer.Save:
            window.location = StockTransfer.SaveSuccessUrl;
            break;
        case StockTransfer.INBOX:
            window.location = StockTransfer.InboxURL;
            break;
        case StockTransfer.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}
//#endregion

//#region ==============================================Validation Section ====================================

function AddValidation(type) {
    ///<summary>   Method to Add Validations</summary>
    // 1 For Header Details
    if (type == 1) {
        $("input[id$=SFH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: StockTransfer.ValidationMsg_EnterDate }
        });
    }
    // 2 For Allocation Item Section
    else if (type == 2) {
        $("select[id$=ITEM]").rules("add", {
            selectNone: true,
            messages: { selectNone: StockTransfer.ValidationMsg_SelectItem }
        });
        $("select[id$=STORE]").rules("add", {
            selectNone: true,
            messages: { selectNone: StockTransfer.ValidationMsg_SelectStore }
        });
        $("input[id$=QTY_ALLOCATED]").rules("add", {
            required: true,
            maxlength: 12,
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            messages: { required: StockTransfer.ValidationMsg_EnterAllocQty, CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });
    }
}

function RemoveValidation() {
    ///<summary>   Method to Remove Validations</summary>
    // 1 For Header Details

    $(document.forms[0]).validate().resetForm();
    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

function RemoveAllValidations() {
    ///<summary>  Method to Remove All validations</summary>
    RemoveValidation();
}

//#endregion

//#region ====================================Grid Edit, Delete Action With grid Handler - Additional Quantity Details=============================

function GridHandler(tr, command) {
    ///<summary> Method to do Action from Additional Item Details Grid</summary>
    switch (command.toString().toLowerCase()) {
        // To Delete Details                                   
        case StockTransfer.Delete:
            adnlItemSlno = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.StockTranferDtlsSLNo, $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(StockTransfer.DoyouWanttoDelete, StockTransfer.ConfirmationTitle, StockTransfer.Delete, true);
            break;
        // To Edit Details                                            
        case StockTransfer.Edit:
            FillAdditionalQtyDtlsEdit(tr);
            break;
        default:
            GrandScriptUtils.ShowModal(StockTransfer.DefaultActionNeedtoPerform, StockTransfer.InformationTitle);
            break;
    }
    return false;
}

function FillAdditionalQtyDtlsEdit(tr) {
    ///<summary> Method to Fill Allocation Additional Details to Controls</summary>
    ClearAllocationAdditionalDtls();
    $("select[id$=ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.POITEM, $(tr).parent().parent().attr("id")));
    FillStore(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.AdditionalDept, $(tr).parent().parent().attr("id")));
    //$("select[id$=STORE]").val();
    $("input[id$=UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.AdditionalUOM, $(tr).parent().parent().attr("id")));
    $("input[id$=UOM_TEXT]").val(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.AdditionalUOMName, $(tr).parent().parent().attr("id")));
    $("input[id$=SFD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.StockTranferDtlsPk, $(tr).parent().parent().attr("id")));
    $("input[id$=SFD_LOCATION]").val(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.AllocatePRLoc, $(tr).parent().parent().attr("id")));
    $("input[id$=SFD_QA_LOT_NO]").val(GrandGrid.Utilities.GetColumnValue(tr, "SFD_QA_LOT_NO", $(tr).parent().parent().attr("id")));  //$("input[id$=SFD_QA_LOT_NO]").val(GrandGrid.Utilities.GetColumnValue(tr, "SFD_QA_LOT_NO", $(tr).parent().parent().attr("id")));
    //for delaying some time, otherwise qty allocated cleared.
    $.post("StockTransfer.do?Action=GetItemUOM&ItemPk=" + $("[id$=ITEM]").val() + "&Status=1", null, function (data) {
        $("input[id$=QTY_ALLOCATED]").val(GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.PRADDNLQTY, $(tr).parent().parent().attr("id")).replace(/[^0-9\.]+/g, ""));
    });
    adnlItemSlno = GrandGrid.Utilities.GetColumnValue(tr, StockTransfer.StockTranferDtlsSLNo, $(tr).parent().parent().attr("id"));
}

function ClearAllocationAdditionalDtls() {
    ///<summary> method to Clear Allocation Details</summary>
    $("select[id$=STORE]").val(StockTransfer.ValueZero);
    $("select[id$=ITEM]").val(StockTransfer.ValueZero);
    $("input[id$=QTY_ALLOCATED]").val(StockTransfer.EMPTYVALUE);
    $("[id$=UOM]").val(StockTransfer.ValueZero);
    $("input[id$=UOM_TEXT]").val(StockTransfer.ValueEmpty);
    $("input[id$=SFD_LOCATION]").val(StockTransfer.ValueEmpty);
    $("input[id$=SFD_QA_LOT_NO]").val(StockTransfer.ValueEmpty);
    $("[id$=SFD_PK]").val(StockTransfer.ValueZero);
    GrandScriptUtils.FillDropDown($("select[id$=STORE]").attr("id"), null, true, true, 0);
    adnlItemSlno = 0;
    return false;
}

function DeleteAdditionalItemDtls() {
    ///<summary>Method to Delete Allocated Item Details  From Grid</summary>
    for (var i in StockTransfer.AllocatedAdditionalList) {
        // Check MaintenanceList[i].MCM_PK Equal to MaintenanceID
        if (StockTransfer.AllocatedAdditionalList[i].SFD_SL_NO == adnlItemSlno) {
            // Splice Details From List, Corresponding MaintenancePK
            StockTransfer.AllocatedAdditionalList.splice(i, 1);
            break;
        }
    }
    // Reassign MaintenanceInfoID From 1 to Last Record , Starting ID=1
    for (var i in StockTransfer.AllocatedAdditionalList) {
        StockTransfer.AllocatedAdditionalList[i].SFD_SL_NO = parseInt(i + 1);
    }
    $("#divData").data("AllocatedQtyList", StockTransfer.AllocatedAdditionalList);
    GrandGrid.MakeGrid($("#grdAdditionalItemList"), 0, StockTransfer.AllocatedAdditionalList);
}

//#endregion==================================================================

//#endregion
//Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

//For checking selected date is a future date or not
//command=>Draft,SaveandSubmit
function ShowFutureDate(command) {
    var msgTitle;
    var msg;
    msgTitle = StockTransfer.MessageBoxTitle;
    msg = StockTransfer.ContFutureDateMsg;
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

function blockFutureDate() {
    GrandScriptUtils.ShowModal(StockTransfer.Err_FutureDateTransactionNotAllowed, StockTransfer.InformationTtile);
    return false;
}
//For showing warning msg for duplicate lot no
function ShowDuplicateLotno(command) {
    var msgTitle;
    var msg;
    msgTitle = StockTransfer.MessageBoxTitle;
    msg = StockTransfer.ContDuplicateLotnoMsg;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfIsDuplicateLotNo]").val(1);
                $(this).dialog("close");
                $("[id$=AddToList]").click();
            },
            Cancel: function (e) {
                $("[id$=hdfIsDuplicateLotNo]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}