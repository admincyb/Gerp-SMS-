/// <reference path="../../JSLINQ/JSLINQ-vsdoc.js" />
/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../GrandScriptUtils.js" />

//#region ---------- Configuration Section-------
var PurchaseOrderConfig = {
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    //    SavePurchaseOrder: "PurchaseOrderGenerate.do?Action=SavePurchaseOrder",
    SavePurchaseOrder: "PurchaseOrderGenerate.do?Action=SavePurchaseOrderWkf",
    WorkflowStatus: "CommonManagement.do?Action=GetWorkflowStatus&RefID=",
    GetPOType: "PurchaseOrderGenerate.do?Action=GetPurchaseOrderTypes&BizUnit=",
    URLGetVendors: "VendorRegistration.do?Action=GetActiveVendors&SBUPk=",
    URLGetVendorsOnly: "VendorRegistration.do?Action=GetActiveVendorsOnly&SBUPk=", //vendors with Role :Dealer,PM Manufacturer
    GetMaterialDtl: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    GetPOCategory: "PurchaseOrderGenerate.do?Action=GetPOCategory&BizUnit=",
    GetPendingPRGridURL: "PurchaseOrderGenerate.do?Action=GetPendingPurchaseRequest",
    GetRevisionHistoryURL: "PurchaseOrderGenerate.do?Action=GetRevisionHistory&PohPK=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    GetPRItemVendor: "PurchaseOrderGenerate.do?Action=GetPurchaseRequestItemVendor",
    GetExchangeRate: "PurchaseOrderGenerate.do?Action=GetExchangeRate",
    GetVendorRates: "PurchaseOrderGenerate.do?Action=GetVendorRates&VendorId=",
    GetGrnQty: "PurchaseOrderGenerate.do?Action=GetGrnQty&podPK=",
    GetStore: "SubDepartment.do?Action=GetGeneralStores&SBUPk=",
    GetGRNDept: "SubDepartmentManagement.do?Action=GetInventoryStores&SBUPk=",
    GetUserDept: "SubDepartmentManagement.do?Action=GetUserDepartments",
    GetCategoryTaxDiscount: "TaxSettings.do?Action=GetActiveCategoryValue&CategoryPK=",
    GetCategoryTaxDiscountDateBase: "TaxSettings.do?Action=GetActiveCategoryDateValue&CategoryPK=",
    GetPRAmendGridURL: "PurchaseOrderGenerate.do?Action=GetPurchaseOrderMapPR&POD_PK=",
    //vendor tax details
    GetVendorCategoryTaxDiscountDateBase: "TaxSettings.do?Action=GetVendorActiveCategoryDateValue",
    PURCHASEREQUESTREPORTURL: "../Reports/GenerateReport.aspx",
    DepartmentsDDL: "CommonManagement.do?Action=DepartmentsDDL&DeptPk=",
    POCategoryDDLGet: "CommonManagement.do?Action=POCategoryDDLGet&DeptPk=",
    GetPortDetailsURL: "CommonManagement.do?Action=GetPortDetails&Type=",
    GetDefaultShippingBillingDeptURL: "StoreLocationMaster.do?Action=GetStoreLocDtlsByID&StoreLocID=",
    PAGEURL: "/VendorManagement/VendorListing.aspx",
    VendorNameAutoCompleteURL: "VendorRegistration.do?Action=GetPOVendorsAuto&AUTOSEARCH=1&SBUPk=",
    GetPurchaseOrderProjectBudget: "PurchaseOrderGenerate.do?Action=GetPurchaseOrderProjectBudget&POH_INVESTOR_CODE=",

    GetInventoryDeptByID: "SubDepartment.do?Action=GetInvDepartment&DeptID=",
    GetVendorDetailsByID: "VendorRegistration.do?Action=GetVendorDtls&VendorId=",
    GetConstantValue: "CommonManagement.do?Action=GetConstantValue&Pk=",
    GetVendorDetailsByIDStatus: "VendorRegistration.do?Action=GetVendorDtlsStatus&VendorId=",
    GetVendorTerms: "VendorTermsManagement.do?Action=GetVenderTerm&VendorId=",
    GetGeneralTerms: "GeneralTemplateMaster.do?Action=GetPOTemplate",
    GetVendorMaterials: "VendorRegistration.do?Action=GetVendorStoreMaterials&VendorId=",
    GetVendorMaterialDetails: "VendorRegistration.do?Action=GetVendorMaterialDetails&VendorId=",
    GetSearchValue: "PurchaseOrderGenerate.do?Action=GetSearchValue&AUTOSEARCH=1&DeptPK=",
    GetTaxFormula: "TaxSettings.do?Action=GetActiveCategoryValue&TaxPK=",
    GetItemRates: "MaterialManagement.do?Action=GetItemRates",
    GetCurrency: "CommonManagement.do?Action=GetCurrencyList&SBU=",
    GetEmployeeCreator: "CommonManagement.do?Action=GetEmployeeCreatorList&SBU=",
    POREPORTURL: "../Reports/GenerateReport.aspx",
    FillStoreDropdownURL: "SubDepartmentManagement.do?Action=GetInventoryStoresBasedOnConfig&SBUPk=",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&SearchCorr=",
    BACKURL: "PurchaseOrderListing.aspx",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    PURCHASERQSTENTRYURL: "../PurchaseRequestManagement/PurchaseRequestCreation.aspx",
    // messages
    SaveMessage1: "Translate(SaveMsgPO1)",
    QtyGRN: "Translate(QtyGrnGreater)",
    QtyInvoice: "Translate(QtyInvoiceGreater)",
    MaterialRateGrZero: "Translate(MaterialRateGrZero)",
    SaveMessage2: "Translate(SaveMsgPO2)",
    NoExchangeRate: "Translate(NoExchangeRate)",
    NoHistory: "Translate(NoHistory)",
    NoSummary: "Translate(NoHistory)",
    SubmitMessage: "Translate(SubmittedMsg)",
    PurchaseOrderSavedMessage: "Translate(PurchaseOrderSavedSuccessfully)",
    Information: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    NotAllowStockAndNonStockItems: "Translate(NotAllowStockAndNonStockItems)",
    NotAllowTotalLessInvoiced: "Translate(NotAllowTotalLessInvoiced)",
    PurchaseBudgetErr: "Translate(PurchaseBudgetErr)",
    GRNBatchUsed: "Translate(GRNBatchUsed)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    ItemAlreadyAddedMsg: "Translate(Itemsalreadyaddedbyanotheruser)",
    MaterialRequired: "Translate(SelectMaterialDtls)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    RecordExist: "Translate(AlreadyExists)",
    GeneralTermsDuplicationMsg: "Translate(GeneralTermsDuplicationMsg)",
    VendorTermsDuplicationMsg: "Translate(VendorTermsDuplicationMsg)",
    OrderQtyNumeric: "Translate(OrderQtyNumeric)",
    OrderAddQtyNumeric: "Translate(OrderAddQtyNumeric)",
    NoItemAdded: "Translate(NoitemAdded)",
    OrderQtyValid: "Translate(OrderQtyValid)",
    EnterRate: "Translate(EnterRate)",
    NoRecordFound: "Translate(NoRecordFound)",
    AdditionalQtyValid: "Translate(AdditionalQtyValid)",
    SelectPurchaseRequest: "Translate(SelectPurchaseRequest)",
    GeneralTermsDuplicationMsg: "Translate(GeneralTermsDuplicationMsg)",
    VendorTermsDuplicationMsg: "Translate(VendorTermsDuplicationMsg)",
    ItemAdded: "Translate(ItemAdded)",
    AddAtleastOneItem: "Translate(AddAtleastOneItem)",
    EnterRequiredBy: "Translate(EnterRequiredBy)",
    NoVendors: "Translate(NoVendors)",
    NoMaterialMapped: "Translate(NoMaterialMapped)",
    NoHistory: "Translate(NoHistory)",
    TypeAlreadyAdded: "Translate(TypeAlreadyAdded)",
    Pleaseenteravalidnumber: "Translate(PleaseenteravalidRate)",
    AmountShouldBeGreaterthanDiscount: "Translate(AmountShouldBeGreaterthanDiscount)",
    WorkflowSubmit: "Translate(WorkflowSubmit)",
    InvalidOrderQtyInvoiced: "Translate(InvalidOrderQtyInvoiced)",
    InvalidOrderQtyGRN: "Translate(InvalidOrderQtyGRN)",
    DifferentRequestedDept: "Translate(DifferentRequestedDept)",
    NoRequestedDeptSelected: "Translate(NoRequestedDeptSelected)",
    TaxDiscOCTypeDuplicate: "Translate(TaxDiscOCTypeDuplicate)",
    VerifyRateChangesRequired: "Translate(VerifyRateChangesRequired)",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    DifferentPRtype: "Translate(DifferentPRtype)",
    DifferentPRgroup: "Translate(DifferentPRgroup)",
    GlovePurchaseError: "Translate(GlovePurchaseError)",
    DifferentInvestor: "Translate(DifferentInvestor)",
    RemainingInvestment: "Translate(RemainingInvestment)",
    RemainingBudget: "Translate(RemainingBudget)",

    Local: "2",
    Import: "1",
    SpecialCond: "&specialCond=PUR", //For Purchase Order
    Edit: "EDIT",
    TaxEdit: "TAXEDIT",
    TaxDelete: "TAXDELETE",
    Delete: "DELETE",
    DeleteItem: "DELETEITEM",
    SaveCommand: "SAVED",
    INBOX: "INBOX",
    HistoryCommand: "HISTORY",
    HistoryCommandPODetails: "HISTORYPODETAILS",
    BizUnitPk: 1,
    Shipping: "Shipping",
    Billing: "Billing",
    VendorPK: 0,
    BaseDept: 1,
    ItemPK: 0,
    DeleteItemPK: 0,
    EditTax: 0,
    EditTaxName: "",
    EditTaxType: 0,
    ItemTaxPK: 0,
    SlNo: 0,
    TaxFormula: "",
    IsViewMode: false,
    CfgType: "PURCHASE TYPE",
    CfgTypeCategory: "PO ITEM TYPE",
    LOGOUT: "LOGOUT",
    HeaderWise: 0,
    ItemWise: 1,
    BothHeaderItem: 2,

    PRD_PK: "PRD_PK",
    PRD_ITEM: "PRD_ITEM",
    PRD_UOM: "PRD_UOM",
    PRD_UOM_TEXT: "PRD_UOM_TEXT",
    PRH_NO: "PRH_NO",
    ITM_CODE: "ITM_CODE", // now its not using may be needed
    ITM_TEXT: "ITM_TEXT",
    ITC_PO_ITEM_TYPE: "ITC_PO_ITEM_TYPE",
    ITV_PRICE: "ITV_PRICE",
    ITV_PRICE_PREV: "ITV_PRICE_PREV",
    POD_REASON: "POD_REASON",
    PRD_REQD_DATE: "PRD_REQD_DATE",
    PRD_QTY_APPROVED: "PRD_QTY_APPROVED",
    PRD_QTY_ORDERED: "PRD_QTY_ORDERED",
    PRD_QTY_BALANCE: "PRD_QTY_BALANCE",
    POR_QTY_ORDERED: "POR_QTY_ORDERED",
    PRD_ITEM_SPEC: "PRD_ITEM_SPEC",
    PRD_PURPOSE: "PRD_PURPOSE",
    IO_NO: "IO_NO",
    POR_QTY_ADDITIONAL: "POR_QTY_ADDITIONAL",
    POD_TAX: "POD_TAX",
    POD_DISC_AMT: "POD_DISC_AMT",
    POD_ITEM: "POD_ITEM",
    POD_UOM: "POD_UOM",
    UOM_CODE: "UOM_CODE",
    POD_QTY_REQUESTED: "POD_QTY_REQUESTED",
    POD_RATE: "POD_RATE",
    POD_SL_NO: "POD_SL_NO",
    POD_AMT_VALUE: "POD_AMT_VALUE",
    POD_AMOUNT: "POD_AMOUNT",
    POD_PK: "POD_PK",
    PRH_DEPT: "PRH_DEPT",
    //    POD_AMT_VALUE: "POD_AMT_VALUE",
    POD_REMARKS: "POD_REMARKS",
    POD_REQD_DATE: "POD_REQD_DATE",
    POT_SL_NO: "POT_SL_NO",
    POT_TAX: "POT_TAX",
    POT_TYPE: "POT_TYPE",
    POT_NAME: "POT_NAME",
    POT_TAX_AMT: "POT_TAX_AMT",
    PRD_DATE: "PRD_DATE",
    ITM_NAME: "ITM_NAME",
    POR_ITEM: "POR_ITEM",
    POR_PK: "POR_PK",
    ITM_IS_PM: "ITM_IS_PM",
    PRH_PK: "PRH_PK",

    VIH_RATING: "VIH_RATING",
    VIH_LAST_QUOT_DATE: "VIH_LAST_QUOT_DATE",
    VIH_LAST_QUOT_RATE: "VIH_LAST_QUOT_RATE",
    VIH_LAST_ORDR_DATE: "VIH_LAST_ORDR_DATE",
    VIH_LAST_ORDR_RATE: "VIH_LAST_ORDR_RATE",
    VIH_LAST_ORDR_QTY: "VIH_LAST_ORDR_QTY",
    VIH_LEAD_TIME: "VIH_LEAD_TIME",
    PRH_COMPANY: "PRH_COMPANY",
    PRH_COMPANY_TEXT: "PRH_COMPANY_TEXT",
    PRH_ISSUE_DEPT: "PRH_ISSUE_DEPT",
    PRH_ISSUE_DEPT_TEXT: "PRH_ISSUE_DEPT_TEXT",
    PRH_TYPE: "PRH_TYPE",
    PRH_GROUP: "PRH_GROUP",
    PRH_IS_GLOVE: "PRH_IS_GLOVE",
    POH_IS_GLOVE: "POH_IS_GLOVE",
    PRH_INVESTOR_CODE: "PRH_INVESTOR_CODE",
    POH_INVESTOR: "POH_INVESTOR",
    // url
    PurchaseListUrl: "PurchaseRequestListing.aspx",

    RateHeading: "Translate(Rate)",
    SubTotalHeading: "Translate(SubTotal)",
    AmountHeading: "Translate(Amount)",
    TaxHeading: "Translate(Tax)",
    DiscountHeading: "Translate(Discount)",
    SelectDeliveryTo: "Translate(SelectDeliveryTo)",
    Domestic: "1",
    Overseas: "2"
}
///#endregion

var QtyDec, AmtDec, RateDec;
var IsMaterialMapped = false;
var IsDirectPO = false;

var LineItemSlno = 0;


///#region----------- Initialization Section ----------------
$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    // RateDec = $("[id$='hdfRateDecimal']").val();
    //   RateDec = $("[id$='hdfRateDecimal']").val();
    RateDec = $("[id$='hdfRateDecimalDigitP2P']").val();
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("NumericExceptZero", function (value, element) {
        return this.optional(element) || /(?!^0*$)\d+$/i.test(value);
    }, "Translate(NumericExceptZero)");
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "Translate(AutoDefaultValue)");
    }, "Translate(Pleaseselectanoption)");
    PageInit();
    $("[id*=TaxRate]").ForceNumericOnly();
    $("[id*=TaxDiscAmount]").ForceNumericOnly();
    $("#divPendPRList").hide();
    $("[id$=btnAddSelectedItems").hide();



});
function AlertShow() {
    $("#updateProgress").show();
    return true;
}


function CancelPO() {
    $.get(PurchaseOrderConfig.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.SessionExpired, PurchaseOrderConfig.ConfirmationMessage, PurchaseOrderConfig.LOGOUT, true);
            result = false;
        }
        else {
            window.location = $("[id$=hdfBackUrl]").val(); //PurchaseOrderConfig.BACKURL;
        }
    });
    return false;
}

function FillPRHType(SelectVal, disable) {
    var drpID = $("select[id$=POH_PO_CATEGORY]").attr("id");
    var drpID2 = $("select[id$=ddlPOhCategory]").attr("id");
    var pk = SelectVal;
    if (SelectVal == undefined)
        pk = '0';
    $("select[id$=POH_PO_CATEGORY]").attr("disabled", false);
    // $.get(PurchaseOrderConfig.GetConstantValue + pk + "&GroupTypeConst=10&GroupConstant=3", function (data) {
    $.get(PurchaseOrderConfig.POCategoryDDLGet + pk + "&ProcessID=" + $("[id$=hdfProcessID]").val() + "&RefID=" + $("[id$=hdfRefID]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
        if (data.length > 1)
            GrandScriptUtils.FillDropDown(drpID2, data, true, true);
        else
            GrandScriptUtils.FillDropDown(drpID2, data, true, false);
        if (disable == true)
            $("select[id$=POH_PO_CATEGORY]").attr("disabled", true);
    });
}

function PageInit() {
    if (parseInt($("[id$=hdfShowTransactionPort]").val()) == 1) {
        $("[id$=divFromPort]").show();
        $("[id$=divToPort]").show();
    }
    else {
        $("[id$=divFromPort]").hide();
        $("[id$=divToPort]").hide();
    }
    if ($("[id$=hdfEnableDirectPO]").val() == "1") {
        $("[id$=btnDirectPO]").show();
    }
    else {
        $("[id$=btnDirectPO]").hide();
    }

    if (parseInt($("[id$=hdfshowbudgetsummary]").val()) == 1) {
        $("[id$=BudgetLink]").show();
    }
    else {
        $("[id$=BudgetLink]").hide();
    }

    if ($("[id$=hdfShowInvestor]").val() == "1") {
        $("[id$=divInvestor]").show();
    }
    else {
        $("[id$=divInvestor]").hide();
    }



    ///<summary>Initial page condition</summary>   
    //WindowExpand(true);
    var queryString = window.location.search.substring(1);
    PurchaseOrderConfig.BizUnitPk = $("[id$=BizUnitPk]").val();
    if (queryString != "") {
        var queryStr = queryString.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                PurchaseOrderConfig.IsViewMode = true;
            }
        }
    }
    if ($("[id$=POH_IS_AMEND]").val() == "1") {
        PurchaseOrderConfig.IsViewMode = false;
        $("#divAmendDate").show();
        $("[id$=POH_AMEND_DATE]").attr("disabled", "disabled");
    }
    if ($("[id$=hdfIsShowAlert]").val() == "1") {
        $("[id$=btnAlert]").show();
    }
    else {
        $("[id$=btnAlert]").hide();
    }
    if ($("[id$=hdfIsReqDeptPostback]").val() != "1") {
        FillStore();
    }


    var purchaseOrderObj = $.parseJSON($("[id$=PurchaseOrderList]").val());
    if (purchaseOrderObj.POH_PK == undefined || purchaseOrderObj.POH_PK == 0)
        $("[id$=PurchaseOrderListPostback]").val($("[id$=PurchaseOrderList]").val());


    $("[id$=PurchaseOrderList]").val("");
    $("[id$=hdfShowAmendmentButton]").val("0");
    $("[id$=PurchaseOrderID]").val(purchaseOrderObj.POH_PK);
    $("[id$=PurchaseOrderStatus]").val(purchaseOrderObj.POH_STATUS);
    //    SetSearchType();
    SearchInit();
    DateInit();

    if ($("[id$=hdfPRtypeEnabled]").val() == "1") {
        $("[id$=POH_PO_CATEGORY]").show();
        $("[id$=POH_TYPE]").hide();
    }
    else {
        $("[id$=POH_PO_CATEGORY]").hide();
        $("[id$=POH_TYPE]").show();
    }
    if (purchaseOrderObj.POH_PK == undefined || purchaseOrderObj.POH_PK == 0) {
        GetSetDefaultShippingBillingDept();
    }
    else {
        FillDepartment(purchaseOrderObj.POH_SHIPPING, PurchaseOrderConfig.Shipping);
        FillDepartment(purchaseOrderObj.POH_BILLING, PurchaseOrderConfig.Billing);
    }
    if (purchaseOrderObj.POH_DEPT == undefined)
        FillDepartments($("[id$=hdfDeptID]").val());
    else
        FillDepartments(purchaseOrderObj.POH_DEPT);
    // FillType(purchaseOrderObj.POH_TYPE);
    if ($("[id$=hdfPOCategoryVal]").val() != "0")
        FillPRHType($("[id$=hdfPOCategoryVal]").val(), true);
    else
        FillPRHType(purchaseOrderObj.POH_PO_CATEGORY, false);
    FillCategory(purchaseOrderObj.POH_ITEM_TYPE);
    if ($("[id$=hdfIsPostbackDirectPO]").val() != "1") { //For resolving Bug ID:  24273:Some times while creating direct po currency is not getting loaded
        FillCurrency(purchaseOrderObj.POH_CURRENCY);
    }
    FillGeneralTerms(false);
    FillEmployeeCreator(purchaseOrderObj.POH_EMPLOYEE);
    Popup();
    if ($("[id$=hdfShowPOAmendAfterInvoice]").val() == "0") {//Hide Amend Button after invoiced (need to add validation in amend button, and remove hide logic)
        if (purchaseOrderObj.POH_HAS_INVOICE != undefined && purchaseOrderObj.POH_HAS_INVOICE == 1)
            $("[id$=btnAmend]").hide();
    }

    CheckConfigForShowDescPM(); //Config for checking Description of Packing Material is showing/ not in Comments field by default.
    if (purchaseOrderObj.POH_PK == undefined || purchaseOrderObj.POH_PK == 0) {
        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "PO", true);
        $("[id$=btnSave]").hide();
        $("[id$=btnAlert]").hide();
        $("[id$=btnRevision]").hide();
        $("[id$=btnPrint]").hide();

        if ($("[id$=hdfIsReqDeptPostback]").val() == "1") {
            $("#divData").data("ReqPODetails", new Array());
            GrandGrid.MakeGrid($("#grdPODetails"), 0, new Array());
            $("#divData").data("TaxDetails", new Array());
            $("#divData").data("TempDetails", new Array()); //For Temporary Storage of Tax/Discount      
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, new Array());
            $("#divData").data("ReqPOListAmend", new Array());
            GrandGrid.MakeGrid($("#grdAmendPRDetails"), 0, new Array());
            if ($("[id$=hdfIsPostbackDirectPO]").val() == "1") {
                FillStore();
                ContinueDirectPO();
            }
            else
                RestorePageAfterPostBack($("[id$=hdfStoreBeforePostback]").val());

        }
        else {
            $("#divData").data("ReqPOList", new Array());
            GrandGrid.MakeGrid($("#grdPOList"), 0, new Array());
            $("#divData").data("ReqPODetails", new Array());
            GrandGrid.MakeGrid($("#grdPODetails"), 0, new Array());
            $("#divData").data("TaxDetails", new Array());
            $("#divData").data("TempDetails", new Array()); //For Temporary Storage of Tax/Discount      
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, new Array());
            $("#divData").data("ReqPOListAmend", new Array());
            GrandGrid.MakeGrid($("#grdAmendPRDetails"), 0, new Array());
        }

        ActiveSearch();
        FillCompany(0);
    }
    else {
        FillPODetails(purchaseOrderObj);
    }

    //    if (purchaseOrderObj.POH_STATUS != 2)//  2--Approved
    //        $("[id$=btnAmend]").hide();

    if ($("[id$=hdfShowAmendmentButton]").val() == 0)
        $("[id$=btnAmend]").hide();

    if (purchaseOrderObj.POH_STATUS > 100) { //ie, Amend Process started then no need to show  Amend Button.But We need to fill Amend Workflow process
        if ($("[id$=POH_IS_AMEND]").val() != "1") {//this checking avoids more than one calling
            $("[id$=btnDummyAmend]").click();
        }
    }
    HideVendorDtl();
    var IsWkfSetting = $("[id$=hdfIsWkfSettingPostBackReqd]").val();
    if (parseInt(IsWkfSetting) == 1) {
        $("[id$=divRequestedDept]").show();
        $("[id$=divReqDepVendorPopup]").show();
    }
    else {
        $("[id$=divRequestedDept]").hide();
        $("[id$=divReqDepVendorPopup]").hide();
    }
    if ($("[id$=hdfPOCatWkfChange]").val() == "1") {
        $("[id$=divPoCategory]").show();
    }
    else {
        $("[id$=divPoCategory]").hide();
    }



    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) != 1) {
        var drpID = $("select[id$=SearchType]").attr("id");
        $("#" + drpID + " option[value=CMP_DISPLAY_CODE]").remove(); //No need to show Plant filter Type    
    }

    //Header Discount and Tax button show hide based on Line item wise setting
    if (parseInt($("[id$=isDiscountAdd]").val()) == PurchaseOrderConfig.ItemWise) {
        $("[id$=imbHdrDiscount]").hide();
    }
    else {
        $("[id$=imbHdrDiscount]").show();
    }
    if (parseInt($("[id$=isTaxAdd]").val()) == PurchaseOrderConfig.ItemWise) {
        $("[id$=imbHdrTax]").hide();
    }
    else {
        $("[id$=imbHdrTax]").show();
    }
}

function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=POH_COMPANY]").attr("id");
    var cmpPK = 0;
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        $.get(PurchaseOrderConfig.FillCompanyDropdownURL + PurchaseOrderConfig.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val() + "&ApsPK=" + selectVal, function (data) {
            if (parseInt($("[id$=PurchaseOrderID]").val()) == 0 && parseInt($("[id$=hdfCompany]").val()) > 0) {
                selectVal = $("[id$=hdfCompany]").val();
            }
            GrandScriptUtils.FillDropDown(drpID, data, false, false, selectVal);
        });
    }
    else {
        $.get(PurchaseOrderConfig.FillCompanyDropdownURL + PurchaseOrderConfig.BizUnitPk + "&Active=1", function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, false, false, selectVal);
        });
    }
}


function FillStore(selectVal) {
    ///<summary>to fill store combo</summary>
    var drpID = $("select[id$=Store]").attr("id");
    var storeURL = "";
    storeURL = PurchaseOrderConfig.FillStoreDropdownURL + PurchaseOrderConfig.BizUnitPk + "&UserPK=" + $("[id$=UserPk]").val() + "&ProcID=" + $("[id$=hdfProcessID]").val() + "&FLD_NAME=DPT_PK";
    $.get(storeURL, function (data) {
        if (data != null && data.length == 1) {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal); //If only single store ,no need of All in requesting store ddl (eg.MMT)
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal, true); //If have multiple store,show All in requesting store ddl.(eg.IGCL,EKK)
        }
        //BindPendingPRGrid();       
        SetSearchType();
    });
}

///#endregion

//#region----------- Core Section----------------
function FillDepartment(deptID, type) {
    ///<summary>Function used to Fill shipping/billing department combo</summary>
    var drpID;
    if (type == PurchaseOrderConfig.Shipping) {
        drpID = $("select[id$=POH_SHIPPING]").attr("id");
        $.get(PurchaseOrderConfig.GetGRNDept + PurchaseOrderConfig.BizUnitPk + "&DeptType=3", function (data) {
            if (data != null && data.length == 1) {//If only single store ,no need of 'select' option
                GrandScriptUtils.FillDropDown(drpID, data, true, false, deptID);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, deptID);
            }
        });
    }
    else if (type == PurchaseOrderConfig.Billing) {
        drpID = $("select[id$=POH_BILLING]").attr("id");
        $.get(PurchaseOrderConfig.GetStore + PurchaseOrderConfig.BizUnitPk + "&UserFlag=0&DeptType=2&DeptPk=0", function (data) {
            if (data != null && data.length == 1) {//If only single store ,no need of 'select' option
                GrandScriptUtils.FillDropDown(drpID, data, true, false, deptID);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, deptID);
            }
        });
    }
}

function FillDepartments(depPK) {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    var drpID = $("select[id$=POH_DEPT]").attr("id");
    $.get(PurchaseOrderConfig.GetUserDept + "&BaseDpt=" + PurchaseOrderConfig.BaseDept + "&SBUPk=" + PurchaseOrderConfig.BizUnitPk, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, depPK);
        $("select[id$=POH_DEPT]").attr("disabled", true);
    });
}

function FillType(SelectVal) {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    $("select[id$=POH_TYPE]").attr("disabled", false);
    var drpID = $("select[id$=POH_TYPE]").attr("id");
    $.get(PurchaseOrderConfig.GetPOType + PurchaseOrderConfig.BizUnitPk + "&CfgType=" + PurchaseOrderConfig.CfgType, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
        $("select[id$=POH_TYPE]").attr("disabled", true);
    });

}



function FillCategory(SelectVal) {
    ///<summary>function used to Fill which category po</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&CfgType=" + CfgTypeCategory
    $("select[id$=POH_ITEM_TYPE]").attr("disabled", false);
    var drpID = $("select[id$=POH_ITEM_TYPE]").attr("id");
    $.get(PurchaseOrderConfig.GetPOCategory + PurchaseOrderConfig.BizUnitPk + "&CfgType=" + PurchaseOrderConfig.CfgTypeCategory, function (data) {
        if (parseInt($("[id$=hdfDefaultPOCategory]").val()) > 0) {
            SelectVal = $("[id$=hdfDefaultPOCategory]").val();
        }
        //        if (parseInt($("[id$=hdfItemPoType]").val()) > 0) {
        //            SelectVal = $("[id$=hdfItemPoType]").val();
        //        }
        if ($("[id$=hdfPOCatWkfChange]").val() == "1") {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectVal);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
        }
    });
    if ($("[id$=POH_NO]").val() != "" || $("[id$=hdfDisablePOCategory]").val() == "1") {
        $("select[id$=POH_ITEM_TYPE]").attr("disabled", true);
    }
}

function FillTaxDiscount(category) {
    ///<summary>function To Fill tax Details </summary>
    $("#divTxRatePer").hide();
    $("[id$=TaxName]").attr("disabled", true);
    var drpID;
    var reqString = "";
    drpID = $("[id$=ChooseTax]").attr("id");
    var PohDate = $("[id$=POH_DATE]").val();
    if (category == 3 || category == 2)//3->Discount,2->OtherCharge,1->Tax
        reqString = PurchaseOrderConfig.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDate=" + PohDate + "&TaxDue=" + $("[id$=hdnIsTaxNotDue]").val();
    else
        //reqString = PurchaseOrderConfig.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDate=" + PohDate + PurchaseOrderConfig.SpecialCond;
        reqString = PurchaseOrderConfig.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDate=" + PohDate + "&ISPURCHASE=1" + "&TaxDue=" + $("[id$=hdnIsTaxNotDue]").val();


    $.get(reqString, function (data) {
        var custom = new Object();
        custom.Formula = "";
        custom.Text = "Custom";
        custom.Value = -1;
        data.push(custom);
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

//Fill vendor header taxes in  popup grid
function FillVendorHeaderTax(VendorPk) {
    ///<summary>function To Fill tax Details </summary>  
    var reqString = "";
    var PohDate = $("[id$=POH_DATE]").val();
    reqString = PurchaseOrderConfig.GetVendorCategoryTaxDiscountDateBase + "&VenPk=" + PurchaseOrderConfig.VendorPK + "&Active=1" + "&TaxDate=" + PohDate;


    $.get(reqString, function (data) {
        var TaxDetails = $("#divData").data("TaxDetails");
        var totTaxAmount = 0;
        if (data != null && data.length > 0) {
            for (var i in data) {
                var TaxDetailsObj = new Object();
                TaxDetailsObj.IsHeader = true;
                TaxDetailsObj.POT_PK = 0;
                TaxDetailsObj.POT_SL_NO = 0;
                TaxDetailsObj.POT_PO_DTL = 0;
                TaxDetailsObj.POT_TAX = data[i].IVT_TAX;
                TaxDetailsObj.POT_TAX_TEXT = data[i].IVT_TAX_TEXT;
                //Amount Calculation
                var formula = data[i].TAX_FORMULA;
                var taxAmount = 0;
                //Finding HeaderTotAmount For tax
                var amount = 0;
                if ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem) {

                    amount = SetTaxApplicableAmount();
                }
                else {
                    var hdrAmt = parseFloat($("[id$=POH_SUB_TOTAL]").val());
                    var hdrDisc = parseFloat($("[id$=POH_DISC_AMT]").val());
                    amount = isNaN(hdrAmt) ? 0 : hdrAmt - (isNaN(hdrDisc) ? 0 : hdrDisc);
                }

                //End
                var total = amount.toFixed(AmtDec);
                formula = formula.replace(/#SUBTOTAL#/g, total);
                try {
                    taxAmount = eval(formula);
                } catch (e) {
                    taxAmount = 0;
                }
                //end
                TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
                TaxDetailsObj.POT_TAX_CATEGORY = data[i].TAX_CATEGORY;
                TaxDetailsObj.POT_TAX_FORMULA = data[i].TAX_FORMULA;
                TaxDetailsObj.POT_NAME = data[i].TAX_HEAD;
                TaxDetailsObj.POT_TYPE = "1";

                totTaxAmount = totTaxAmount + taxAmount;
                //Filling TotTaxAmount to HeaderTaxDetails             
                $("[id$=POH_ADD_TAX_AMT]").val(parseFloat(totTaxAmount).toFixed(AmtDec));
                //End
                TaxDetails.push(TaxDetailsObj);
            }
            CalculateTotal();
        }

        $("#divData").data("TaxDetails", TaxDetails);
        //Clone TaxDetails Data to TempDetails             
        $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
        //End
        GrandGrid.MakeGrid($("#grdTaxDetails"), 1, TaxDetails);

    });
}

//End


//Fill Line Item taxes in popup grid
function FillLineItemTax(VendorItemPk, amount) {
    ///<summary>function To Fill line item tax Details </summary>  
    var reqString = "";
    var PohDate = $("[id$=POH_DATE]").val();
    reqString = PurchaseOrderConfig.GetVendorCategoryTaxDiscountDateBase + "&VenPk=" + PurchaseOrderConfig.VendorPK + "&ITMPK=" + VendorItemPk + "&Active=1" + "&TaxDate=" + PohDate;


    //For Getting Current Slno 
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    //check if this item is allready added.then no need to give new slno.
    var IsItemExist = 0;
    var itemName = "";
    for (var item in PurOrderDetails) {
        if (PurOrderDetails[item].POD_ITEM == VendorItemPk) {
            IsItemExist = 1;
            LineItemSlno = PurOrderDetails[item].POD_SL_NO;
        }
    }
    if (IsItemExist == 0) {
        LineItemSlno = PurOrderDetails.length + 1;

        $.get(reqString, function (data) {
            var TaxDetails = $("#divData").data("TaxDetails");
            var totTaxAmount = 0;
            if (data != null && data.length > 0) {
                for (var i in data) {
                    var TaxDetailsObj = new Object();
                    TaxDetailsObj.IsHeader = false;
                    TaxDetailsObj.POT_PK = 0;
                    TaxDetailsObj.POT_SL_NO = LineItemSlno;
                    TaxDetailsObj.POT_PO_DTL = 0;
                    TaxDetailsObj.POT_TAX = data[i].IVT_TAX;
                    TaxDetailsObj.POT_TAX_TEXT = data[i].IVT_TAX_TEXT;

                    //tax Amount Calculation
                    var formula = data[i].TAX_FORMULA;
                    var taxAmount = 0;
                    var total = amount.toFixed(AmtDec);
                    formula = formula.replace(/#SUBTOTAL#/g, total);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }


                    totTaxAmount = totTaxAmount + taxAmount;
                    //Filling TotTaxAmount to HeaderTaxDetails             
                    $("[id$=POD_TAX]").val(parseFloat(totTaxAmount).toFixed(AmtDec));

                    //end

                    TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
                    TaxDetailsObj.POT_TAX_CATEGORY = data[i].TAX_CATEGORY;
                    TaxDetailsObj.POT_TAX_FORMULA = data[i].TAX_FORMULA;
                    TaxDetailsObj.POT_NAME = data[i].TAX_HEAD;
                    TaxDetailsObj.POT_TYPE = "1";

                    TaxDetails.push(TaxDetailsObj);

                }
            }

            $("#divData").data("TaxDetails", TaxDetails);
            GrandGrid.MakeGrid($("#grdTaxDetails"), 1, TaxDetails);

        });

    }
    else {
        //        UpdateLineItemTax;
        $.get(reqString, function (data) {
            var TaxDetails = $("#divData").data("TaxDetails");
            var totTaxAmount = 0;
            var count = 0;
            if (data != null && data.length > 0) {
                for (var i in data) {


                    var TaxDetailsObj;
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_SL_NO == LineItemSlno && tax.POT_NAME == data[i].TAX_HEAD; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        //tax Amount RE Calculation
                        var formula = data[i].TAX_FORMULA;
                        var taxAmount = 0;
                        var total = amount.toFixed(AmtDec);
                        formula = formula.replace(/#SUBTOTAL#/g, total);
                        try {
                            taxAmount = eval(formula);
                        } catch (e) {
                            taxAmount = 0;
                        }

                        totTaxAmount = totTaxAmount + taxAmount;
                        //Filling TotTaxAmount to HeaderTaxDetails             
                        $("[id$=POD_TAX]").val(parseFloat(totTaxAmount).toFixed(AmtDec));

                        //end

                        //end
                        TaxDetailsObj.POT_TAX_AMT = parseFloat(taxAmount).toFixed(AmtDec);

                    }
                }
            }

        });
    }
}

//End



function FillDepartmentDetails(deptID, type) {
    ///<summary>function To fill selected department details </summary>
    if (deptID > 0) {
        $.get(PurchaseOrderConfig.GetInventoryDeptByID + deptID, function (data) {
            if (data != null && data.length > 0) {
                if (type == PurchaseOrderConfig.Shipping) {
                    $("[id$=ShippingAddress1]").html(data[0].DPT_ADDR1);
                    $("[id$=ShippingAddress2]").html(data[0].DPT_ADDR2);
                    $("[id$=ShippingCity]").html(data[0].DPT_CITY);
                    $("[id$=ShippingCountry]").html(data[0].DPT_CNTRY_TEXT);
                }
                else if (type == PurchaseOrderConfig.Billing) {
                    $("[id$=BillingAddress1]").html(data[0].DPT_ADDR1);
                    $("[id$=BillingAddress2]").html(data[0].DPT_ADDR2);
                    $("[id$=BillingCity]").html(data[0].DPT_CITY);
                    $("[id$=BillingCountry]").html(data[0].DPT_CNTRY_TEXT);
                }
            }
        });
    }
    else {
        if (type == PurchaseOrderConfig.Shipping) {
            $("[id$=ShippingAddress1]").html("");
            $("[id$=ShippingAddress2]").html("");
            $("[id$=ShippingCity]").html("");
            $("[id$=ShippingCountry]").html("");
        }
        else if (type == PurchaseOrderConfig.Billing) {
            $("[id$=BillingAddress1]").html("");
            $("[id$=BillingAddress2]").html("");
            $("[id$=BillingCity]").html("");
            $("[id$=BillingCountry]").html("");
        }
    }
}

function FillCurrency(currencyID) {
    ///<summary>
    ///Used for FillCurrency
    ///</summary>
    // Get id of the Country DropDown
    var drpID = $("select[id$=POH_CURRENCY]").attr("id");
    $.get(PurchaseOrderConfig.GetCurrency + PurchaseOrderConfig.BizUnitPk, function (data) {
        if (parseInt($("[id$=hdfVendorCurrency]").val()) > 0) {
            currencyID = $("[id$=hdfVendorCurrency]").val();
        }
        GrandScriptUtils.FillDropDown(drpID, data, true, true, currencyID);
    });
}
function FillEmployeeCreator(creatorID) {
    var drpID = $("select[id$=POH_EMPLOYEE]").attr("id");
    $.get(PurchaseOrderConfig.GetEmployeeCreator + PurchaseOrderConfig.BizUnitPk, function (data) {
        if (parseInt($("[id$=hdfCreator]").val()) > 0) {
            currencyID = $("[id$=hdfCreator]").val();
        }
        GrandScriptUtils.FillDropDown(drpID, data, true, true, creatorID);
    });
}

function FillVendorDetails(vendorID, PODetails) {
    ///<summary>function used to fill vendor Details corresponding to Vendor ID </summary>
    if (vendorID != "0") {
        $.get(PurchaseOrderConfig.GetVendorDetailsByIDStatus + vendorID + "&Status=0", function (data) {  //Altered for resolving Bug ID:  2220 
            if (data != null && data.length > 0) {
                //$("[id$=POH_CURRENCY]").val(data[0].VEN_CURRENCY);
                $("[id$=VendorName]").html(data[0].VEN_NAME);
                $("[id$=VendorNameText]").val(data[0].VEN_NAME);

                $("[id$=ContactName]").html(data[0].VEN_CONT_NAME);
                $("[id$=TinNo]").html(data[0].VEN_TIN);
                $("[id$=VendorAddressDtls]").html(data[0].ADDRESS);
                $("[id$=VendorCurrencyCode]").val(data[0].VEN_CURRENCY_TEXT);
                $("[id$=hdfVendorCountry]").val(data[0].VEN_CNTRY);
            }
            GrandGrid.MakeGrid($("#grdPODetails"), 0, PODetails);
        });
    }
}

function FillVendor(poID, status) {
    ///<summary>function To fill vendors in radio button list </summary>
    var isViewMode = false;
    if (PurchaseOrderConfig.IsViewMode) {
        isViewMode = true;
    }
    else if (status == 1 || status == 7) {
        isViewMode = true;
    }

    var rdoTable = "";
    var PurOrderList = new Object();
    PurOrderList.ItemDetails = $("#divData").data("ReqPOList");
    var selectedPRItems = JSON.stringify(PurOrderList);
    //    $.get(PurchaseOrderConfig.GetPRItemVendor + "&POID=" + poID + "&PRItems=" + selectedPRItems, function (data) {
    $.post(PurchaseOrderConfig.GetPRItemVendor + "&POID=" + poID, selectedPRItems, function (data) {
        if (data) {
            for (var i in data) {
                var countryText = data[i].VEN_CNTRY_TEXT == null || data[i].VEN_CNTRY_TEXT == "" ? "" : ", " + data[i].VEN_CNTRY_TEXT;
                var vendorText = (data[i].VEN_NAME.length > 40 ? data[i].VEN_NAME.substr(0, 40) + "..." : data[i].VEN_NAME) + (countryText.length > 25 ? countryText.substr(0, 25) + "..." : countryText);
                var leastPrice = parseFloat(data[0].TOTAL_PRICE); //We need change the color of vendor who have least price.In SP Vendor's are order by ItemPrice.
                if (leastPrice == parseFloat(data[i].TOTAL_PRICE))//if more than one vendor have least price.
                    vendorText = vendorText.fontcolor("green");
                var vendorTitle = data[i].VEN_NAME + countryText;
                if (parseInt($("[id$=PurchaseOrderID]").val()) == 0 && i == 0) {
                    rdoTable += "<div class=\"tdCheckbx\"><input tabIndex=\"8\" id=\"rdoVendors_" + i + "\" value=" + data[i].VEN_PK + " type=\"radio\" name=\"rdoVendors\" checked=\"checked\" " + (((isViewMode) && data[i].PO_VENDOR_FLAG == "false") ? "disabled=\"disabled\"" : "") + " onclick=\"javascript:FilterPR();\" /><label for=\"rdoVendors_" + i + "\" title = \"" + vendorTitle + "\">" + vendorText + "</label> </div>";
                }
                else {
                    rdoTable += "<div class=\"tdCheckbx\"><input tabIndex=\"8\" id=\"rdoVendors_" + i + "\" value=" + data[i].VEN_PK + " type=\"radio\" name=\"rdoVendors\" " + (((isViewMode) && data[i].PO_VENDOR_FLAG == "false") ? "disabled=\"disabled\"" : "") + " onclick=\"javascript:FilterPR();\" " + (data[i].PO_VENDOR_FLAG == "true" ? "checked=\"checked\"" : "") + " /><label  for=\"rdoVendors_" + i + "\" title = \"" + vendorTitle + "\">" + vendorText + "</label> </div>";
                }
                if ((i != 0) && (i != data.length - 1)) {
                    if ((i % 3) == 0) {
                        rdoTable += "<div class=\"clear\" />";
                    }
                }
            }
            $("#divVendors").html(rdoTable);
        }
        $('input:checkbox').removeAttr('checked'); // remove the 'checked' state from all checkboxes    // BindPendingPRGrid();
        $("#divVendors").find("[name=rdoVendors]:checked").attr("tabIndex", "8");
        $("#divVendors").find("[name=rdoVendors]:checked").focus();

        if ($("[id$=hdfIsReqDeptPostback]").val() == "1") {
            POContinue();
            $("[id$=hdfIsReqDeptPostback]").val("0");
            if ($("[id$=VEN_PK]").val() > 0) {
                $('input:radio').removeAttr('checked');
            }

        }
    });
}

function FillPODetails(purchaseOrderObj) {

    ///<summary>function To fill purchase order details when edit case </summary>   
    //0=>Drafted,6=>Requested for more Info,8=>Requested to review for more Info,16=>Requested to approve for more Info,2=>Verified,108=>Amended:Requested for more info by reviewer,5=>Closed
    //<summary>
    if (parseInt(purchaseOrderObj.POH_STATUS) == 0 || parseInt(purchaseOrderObj.POH_STATUS) == 6 || parseInt(purchaseOrderObj.POH_STATUS) == 8 || parseInt(purchaseOrderObj.POH_STATUS) == 16 || parseInt(purchaseOrderObj.POH_STATUS) == 2 || parseInt(purchaseOrderObj.POH_STATUS) == 108 || parseInt(purchaseOrderObj.POH_STATUS) == 5) {
        if (PurchaseOrderConfig.IsViewMode == false) {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "PO", true);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "PO", false);
        }
    }
    else {
        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "PO", false);
    }
    $("[id$=POH_PK]").val(purchaseOrderObj.POH_PK);
    $("[id$=hdfShowAmendmentButton]").val(purchaseOrderObj.POH_CAN_AMEND);

    if (purchaseOrderObj.POH_NO == null || purchaseOrderObj.POH_NO == "") {
        $("[id$=POH_NO]").val("");
        $("[id$=lblPOH_NO]").html("[NEW]");
    }
    else {
        $("[id$=POH_NO]").val(purchaseOrderObj.POH_NO);
        $("[id$=lblPOH_NO]").html(purchaseOrderObj.POH_NO);
    }
    FillCompany(purchaseOrderObj.POH_COMPANY);
    $("[id$=POH_DATE]").val(purchaseOrderObj.POH_DATE);
    if (purchaseOrderObj.POH_AMEND_DATE != undefined) {
        $("[id$=POH_AMEND_DATE]").val(purchaseOrderObj.POH_AMEND_DATE);
        $("#divAmendDate").show();
        $("[id$=POH_AMEND_DATE]").attr("disabled", "disabled");
    }
    $("[id$=POH_CONTRACT_REF_NO]").val(purchaseOrderObj.POH_CONTRACT_REF_NO);
    $("[id$=CreatedBy]").html(purchaseOrderObj.POH_CRTD_BY.length > 35 ? purchaseOrderObj.POH_CRTD_BY.substr(0, 35) + "..." : purchaseOrderObj.POH_CRTD_BY);
    $("[id$=CreatedBy]").attr('title', purchaseOrderObj.POH_CRTD_BY);
    $("[id$=POH_CRTD_BY]").val(purchaseOrderObj.POH_CRTD_BY);
    $("[id$=POH_SUB_TOTAL]").val(parseFloat(purchaseOrderObj.POH_SUB_TOTAL).toFixed(AmtDec));
    $("[id$=POH_DISC_AMT]").val(parseFloat(purchaseOrderObj.POH_DISC_AMT).toFixed(AmtDec));
    $("[id$=POH_SHIP_CHARGE]").val(parseFloat(purchaseOrderObj.POH_SHIP_CHARGE).toFixed(AmtDec));
    $("[id$=POH_ADD_TAX_AMT]").val(parseFloat(purchaseOrderObj.POH_ADD_TAX_AMT).toFixed(AmtDec));
    $("[id$=POH_PRICE_ADJUST]").val(parseFloat(purchaseOrderObj.POH_PRICE_ADJUST).toFixed(AmtDec));
    $("[id$=POH_TOTAL_VALUE]").val(parseFloat(purchaseOrderObj.POH_TOTAL_VALUE).toFixed(AmtDec));
    $("[id$=POH_REMARKS]").html(purchaseOrderObj.POH_REMARKS);
    $("[id$=POH_GROUP]").val(purchaseOrderObj.POH_GROUP == 0 ? 1 : purchaseOrderObj.POH_GROUP);
    $("[id$=POH_IS_GLOVE]").val(purchaseOrderObj.POH_IS_GLOVE == 0 ? 1 : purchaseOrderObj.POH_IS_GLOVE);
    BindVendor();
    $("[id$=POH_DELIVERY]").val(purchaseOrderObj.POH_DELIVERY);
    $("[id$=txtBillVendor]").val(purchaseOrderObj.POH_DELIVERY_TEXT);
    if (purchaseOrderObj.POH_INVESTOR != "null") {
        $("[id$=POH_INVESTOR_CODE]").val(purchaseOrderObj.POH_INVESTOR);
    }


    //$("[id$=LblPOH_VENDOR_TERMS]").html(purchaseOrderObj.VENDOR_TERMS); 
    //    $("[id$=LblPOH_TERMS]").html(purchaseOrderObj.TERMS);
    var regex = /<br\s*[\/]?>/gi;
    if (purchaseOrderObj.POH_VENDOR_TERMS_TEXT != null)
        $("[id$=txtVendorTermText]").val(purchaseOrderObj.POH_VENDOR_TERMS_TEXT.replace(regex, "\n"));
    if (purchaseOrderObj.POH_TERMS_TEXT != null)
        $("[id$=txtTermText]").val(purchaseOrderObj.POH_TERMS_TEXT.replace(regex, "\n"));

    if (purchaseOrderObj.POH_VENDOR_TERMS != null)
        VendorTerms = purchaseOrderObj.POH_VENDOR_TERMS.split(',');
    if (purchaseOrderObj.POH_TERMS != null)
        Terms = purchaseOrderObj.POH_TERMS.split(',');
    $("[id$=POH_VENDOR_TERMS]").val(purchaseOrderObj.POH_VENDOR_TERMS);
    $("[id$=POH_TERMS]").val(purchaseOrderObj.POH_TERMS);
    $("[id$=POH_COMMENTS]").html(purchaseOrderObj.POH_COMMENTS);
    PurchaseOrderConfig.VendorPK = purchaseOrderObj.POH_VENDOR;
    $("[id$=POH_VENDOR]").val(PurchaseOrderConfig.VendorPK);
    $("[id$=LAST_MOD_DT]").val(purchaseOrderObj.LAST_MOD_DT);
    $("[id$=LastModifiedTime]").val(purchaseOrderObj.LAST_MOD_DT);
    $("[id$=lblRequestedDept]").html(purchaseOrderObj.POH_ISSUE_DEPT_TEXT);
    $("[id$=POH_VERIFIED]").val(purchaseOrderObj.POH_VERIFIED);

    if (PurchaseOrderConfig.IsViewMode) {
        DisableControlls();
    }
    else if (purchaseOrderObj.POH_STATUS == 1 || purchaseOrderObj.POH_STATUS == 7 || purchaseOrderObj.POH_STATUS == 11) {
        DisableControlls();
    }
    if (purchaseOrderObj.POH_STATUS == 4) {
        $("#h1ShortClose").show();
        $("#divShortCloseInfo").show();
        $("[id$=lblShrtDoneBy]").html(purchaseOrderObj.POH_SHORT_CLS_USER);
        $("[id$=lblShrtRemarks]").html(purchaseOrderObj.POH_SHORT_CLS_REASON);
        $("[id$=lblShrtDate]").html(purchaseOrderObj.POH_SHORT_CLS_DATE);
        $("[id$=lblShrtRefNo]").html(purchaseOrderObj.POH_SHORT_CLS_REFNO);
        $("[id$=lblShrtTime]").html(purchaseOrderObj.POH_SHORT_CLS_TIME);
    }
    else {
        $("#h1ShortClose").hide();
        $("#divShortCloseInfo").hide();
    }
    if ($("[id$=hdfRestrictEditOption]").val() == "1") {
        if (purchaseOrderObj.POH_STATUS == 8 || purchaseOrderObj.POH_STATUS == 16 || purchaseOrderObj.POH_STATUS == 22 || purchaseOrderObj.POH_STATUS == 26
            || purchaseOrderObj.POH_STATUS == 101 || purchaseOrderObj.POH_STATUS == 106 || purchaseOrderObj.POH_STATUS == 107 || purchaseOrderObj.POH_STATUS == 109
            || purchaseOrderObj.POH_STATUS == 111 || purchaseOrderObj.POH_STATUS == 117 || purchaseOrderObj.POH_STATUS == 122 || purchaseOrderObj.POH_STATUS == 126) {
            DisableControlls();
        }
    }
    FillDepartmentDetails(purchaseOrderObj.POH_SHIPPING, PurchaseOrderConfig.Shipping);
    FillDepartmentDetails(purchaseOrderObj.POH_BILLING, PurchaseOrderConfig.Billing);
    FillVendorTerms(PurchaseOrderConfig.VendorPK, purchaseOrderObj.POH_VENDOR_TERMS, "true");
    FillVendorMaterials(PurchaseOrderConfig.VendorPK);
    FillType(purchaseOrderObj.POH_TYPE);
    FillPRHType(purchaseOrderObj.POH_PO_CATEGORY, true);
    FillCategory(purchaseOrderObj.POH_ITEM_TYPE);

    FillCurrency(purchaseOrderObj.POH_CURRENCY);
    FillEmployeeCreator(purchaseOrderObj.POH_EMPLOYEE);

    SetInitialPortList(purchaseOrderObj.POH_TYPE);
    var PODetails = new Array();
    if ($.isArray(purchaseOrderObj.PurchaseOrderList.PODetails)) {
        PODetails = purchaseOrderObj.PurchaseOrderList.PODetails;
    }
    else {
        PODetails.push(purchaseOrderObj.PurchaseOrderList.PODetails);
    }
    SetPOPRDetails(PODetails);
    PurchaseOrderRequestSummary();
    $("[id$=btnSave]").show();
    $("[id$=btnSubmit]").show();
    SetPOTaxDetails(purchaseOrderObj, PODetails);
    $("#divData").data("ReqPODetails", PODetails);
    FillVendorDetails(PurchaseOrderConfig.VendorPK, PODetails);
    $("#divData").data("purchaseOrderObj", purchaseOrderObj);
    ActiveCreatePO();
    $("[id$=POH_FROM_PORT_TEXT]").val(purchaseOrderObj.POH_FROM_PORT_TEXT != null ? purchaseOrderObj.POH_FROM_PORT_TEXT : "Select/Type");
    $("[id$=POH_FROM_PORT]").val(purchaseOrderObj.POH_FROM_PORT);
    $("[id$=POH_TO_PORT]").val(purchaseOrderObj.POH_TO_PORT);
    $("[id$=POH_TO_PORT_TEXT]").val(purchaseOrderObj.POH_TO_PORT_TEXT != null ? purchaseOrderObj.POH_TO_PORT_TEXT : "Select/Type");
    if (PurchaseOrderConfig.IsViewMode) {
        DisableAuto($("[id$=POH_FROM_PORT_TEXT]"), $("[id$=POH_FROM_PORT]"));
        DisableAuto($("[id$=POH_TO_PORT_TEXT]"), $("[id$=POH_TO_PORT]"));
    }
    //#region --------------- Fill File Upload Details----------------------------------
    if (!($.isArray(purchaseOrderObj.FILELIST))) {
        if (purchaseOrderObj.FILELIST != undefined) {
            objArray = purchaseOrderObj.FILELIST;
            FileJson.FILELIST = new Array();
            FileJson.FILELIST.push(objArray);
        }
        else {
            objArray = purchaseOrderObj.FILELIST;
            FileJson.FILELIST = new Array();
        }
    }
    else {
        FileJson.FILELIST = purchaseOrderObj.FILELIST;
    }
    FillFileDetails();
    //#Endregion
}

function GetExchangeRate() {
    var BaseCurency = $("[id$=POH_CURRENCY_BC]").val();
    var ToCurrency = $("[id$=POH_CURRENCY]").val();
    var PohDate = $("[id$=POH_DATE]").val();

    $.post(PurchaseOrderConfig.GetExchangeRate + "&fromCurrency=" + BaseCurency + "&toCurrency=" + ToCurrency + "&date=" + PohDate, function (data) {
        if (data) {
            var Rate = data[0].Column1;
            $("[id$=POH_EXCHG_RATE]").val(data[0].Column1);
            $("[id$=POH_TOTAL_VALUE_BC]").val(parseFloat($("[id$=POH_EXCHG_RATE]").val()) * parseFloat($("[id$=POH_TOTAL_VALUE]").val()))

        }
    });
    return true;

}
function BindPendingPRGrid() {
    $("#divPendPRList").show();
    $("[id$=btnAddSelectedItems").show();
    ///<summary>function used to bind pending pr list</summary>
    var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&POID=" + $("[id$=PurchaseOrderID]").val() + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&P_DeptPK=" + $("[id$=Store]").val() + "&SortBy=PRD_SORT_DATE" + "&ApplicationPK=" + $("[id$=hdfApplicationPK]").val() + "&ProcessID=" + $("[id$=hdfProcessID]").val() + "&TYPE=" + $("[id$=POH_MENU_TYPE]").val();
    var ajaxUrl = PurchaseOrderConfig.GetPendingPRGridURL + queryString;
    $("#grdPendingPRList").removeAttr("ajaxurl")
    $("#grdPendingPRList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPendingPRList");
    GrandGrid.MakeGrid($("#grdPendingPRList"));
}

function AddPORequestList() {
    //<summary> function used to select the pending purchase request, and add it into the list </summary>
    var grdID;
    var colIndex = 0;
    var floatRegQty = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
    var floatAddRegQty = new RegExp("^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
    var isValid = true;
    var PurOrderObj;
    var prdPK = 0;
    var PurOrderList = $("#divData").data("ReqPOList");
    var tempList = new Array();
    if ($("#grdPendingPRList tr input[type=checkbox]:checked").length == 0) {
        GrandScriptUtils.ShowModal(PurchaseOrderConfig.SelectPurchaseRequest, PurchaseOrderConfig.Information);
        return false;
    }


    //---------------------
    if ($("[id$=hdfShowInvestor]").val() == "1") {
        if (!IsSameInvestor()) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.DifferentInvestor, PurchaseOrderConfig.Information);
            return false;
        }
    }
    //---------------------
    if ($("[id$=hdfEnableGlovePR]").val() == "1") {
        if (!IsSameGloveGroup()) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.GlovePurchaseError, PurchaseOrderConfig.Information);
            return false;
        }
    }
    //---------------------- 
    if ($("[id$=hdfPRGroupEnabled]").val() == "1") {
        if (!IsSamePRGroup()) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.DifferentPRgroup, PurchaseOrderConfig.Information);
            return false;
        }
    }
    //----------------------
    if ($("[id$=hdfPRtypeEnabled]").val() == "1") {
        if (!IsSamePRtype()) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.DifferentPRtype, PurchaseOrderConfig.Information);
            return false;
        }
    }
    //----------------------
    if ($("[id$=hdfIsWkfSettingPostBackReqd]").val() == "1") {
        if (!IsSameRequestedDept()) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.DifferentRequestedDept, PurchaseOrderConfig.Information);
            return false;
        }
    }
    //---------------------
    $("#grdPendingPRList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            if (GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT, grdID) != "null") {
                $("[id$=POH_ISSUE_DEPT]").val(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT, grdID));  //Setting Issue dep for furthure use;
            }
            $("[id$=hdfIssueDeptText]").val(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT_TEXT, grdID));
            $("[id$=hdfInvestorCode]").val(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_INVESTOR_CODE, grdID));

            if ($("[id$=hdfShowInvestor]").val() == "1" && $("[id$=hdfInvestorCode]").val() != "null") {
                $("[id$=POH_INVESTOR_CODE]").val($("[id$=hdfInvestorCode]").val());
            }

            prdPK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_PK, grdID);
            var PurOrderObj = JSLINQ(PurOrderList)
                .Where(function (item) { return item.POR_PR_DTL == prdPK; })
                .FirstOrDefault(null);
            if (PurOrderObj == null) {
                PurOrderObj = new Object();
                PurOrderObj.POR_PK = 0;
                PurOrderObj.POR_PO_DTL = 0;
                PurOrderObj.POR_SL_NO = 0;
                PurOrderObj.PRD_PK = prdPK;
                PurOrderObj.POR_PR_DTL = prdPK;
                PurOrderObj.PRH_PK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_PK, grdID);
                PurOrderObj.PRH_DEPT = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_DEPT, grdID);
                PurOrderObj.POR_ITEM = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_ITEM, grdID);
                PurOrderObj.ITC_PO_ITEM_TYPE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITC_PO_ITEM_TYPE, grdID);
                PurOrderObj.POR_UOM = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_UOM, grdID);
                PurOrderObj.PRD_UOM_TEXT = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_UOM_TEXT, grdID);
                PurOrderObj.PRH_NO = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_NO, grdID);
                PurOrderObj.ITM_TEXT = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_TEXT, grdID);
                PurOrderObj.ITV_PRICE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITV_PRICE, grdID);
                PurOrderObj.PRD_QTY_APPROVED = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_APPROVED, grdID));
                PurOrderObj.PRD_REQD_DATE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_REQD_DATE, grdID);
                PurOrderObj.PRD_QTY_ORDERED = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_ORDERED, grdID));
                PurOrderObj.PRD_QTY_BALANCE = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, grdID).replace(/[^0-9\.]+/g, ""));
                var ioNo = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.IO_NO, grdID);
                PurOrderObj.IO_NO = ioNo == "null" ? "" : ioNo;
                $("[id$=hdfCompany]").val(GrandGrid.Utilities.GetColumnValue($(this), "PRH_COMPANY", grdID));

                //new Fields
                PurOrderObj.PRD_DATE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_DATE, grdID);
                PurOrderObj.ITM_CODE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_CODE, grdID);
                PurOrderObj.ITM_NAME = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_NAME, grdID);
                PurOrderObj.PRD_ITEM_SPEC = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_ITEM_SPEC, grdID);
                PurOrderObj.PRD_PURPOSE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_PURPOSE, grdID);
                PurOrderObj.PRD_QTY_BALANCE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, grdID).replace(/[^0-9\.]+/g, "");
                PurOrderObj.PRD_REQD_DATE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_REQD_DATE, grdID);

                PurOrderObj.PRH_COMPANY = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_COMPANY, grdID);
                PurOrderObj.PRH_COMPANY_TEXT = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_COMPANY_TEXT, grdID);
                PurOrderObj.PRH_ISSUE_DEPT = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT, grdID);
                PurOrderObj.PRH_ISSUE_DEPT_TEXT = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT_TEXT, grdID);
                PurOrderObj.PRH_TYPE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_TYPE, grdID);
                PurOrderObj.PRH_PO_CATEGORY = GrandGrid.Utilities.GetColumnValue($(this), "PRH_PO_CATEGORY", grdID);
                PurOrderObj.PRH_GROUP = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_GROUP, grdID);
                PurOrderObj.PRH_IS_GLOVE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_IS_GLOVE, grdID); //juno

                PurOrderObj.PRH_GROUP_TEXT = GrandGrid.Utilities.GetColumnValue($(this), "PRH_GROUP_TEXT", grdID);
                PurOrderObj.PRH_INVESTOR_CODE = GrandGrid.Utilities.GetColumnValue($(this), "PRH_INVESTOR_CODE", grdID);


                if ($("[id$=hdfEnbleCostCenter]").val() == "1") {
                    PurOrderObj.POR_COST_CENTER = GrandGrid.Utilities.GetColumnValue($(this), "PRH_COST_CENTER", grdID);
                    PurOrderObj.POR_COST_CENTER_TEXT = GrandGrid.Utilities.GetColumnValue($(this), "PRH_COST_CENTER_TEXT", grdID);
                }
                //PurOrderList.push(PurOrderObj);
                tempList.push(PurOrderObj);
            }
            else {
                PurOrderObj.PRD_PK = prdPK;
                PurOrderObj.POR_PR_DTL = prdPK;
                PurOrderObj.PRD_REQD_DATE = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_REQD_DATE, grdID);
                PurOrderObj.PRD_QTY_ORDERED = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_ORDERED, grdID).replace(/[^0-9\.]+/g, ""));
                PurOrderObj.PRD_QTY_BALANCE = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, grdID).replace(/[^0-9\.]+/g, ""));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ORDERED, grdID);
            if (colIndex) {
                PurOrderObj.POR_QTY_ORDERED = parseFloat($(this).find("td:eq(" + colIndex + "): input[type=text]").val());
                if (!floatRegQty.test(PurOrderObj.POR_QTY_ORDERED)) {
                    isValid = false;
                    GrandScriptUtils.ShowModal(String.format(PurchaseOrderConfig.OrderQtyNumeric.fontcolor("red"), QtyDec), PurchaseOrderConfig.Information);
                    return false;
                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ADDITIONAL, grdID);
            if (colIndex) {
                PurOrderObj.POR_QTY_ADDITIONAL = parseFloat($(this).find("td:eq(" + colIndex + "): input[type=text]").val());
                if (!floatAddRegQty.test(PurOrderObj.POR_QTY_ADDITIONAL)) {
                    isValid = false;
                    GrandScriptUtils.ShowModal(String.format(PurchaseOrderConfig.OrderAddQtyNumeric.fontcolor("red"), QtyDec), PurchaseOrderConfig.Information);
                    return false;
                }
            }
            if (PurOrderObj.POR_QTY_ORDERED > PurOrderObj.PRD_QTY_BALANCE) {
                isValid = false;
                GrandScriptUtils.ShowModal(PurchaseOrderConfig.OrderQtyValid, PurchaseOrderConfig.Information);
                return false;
            }
            else if ((PurOrderObj.POR_QTY_ORDERED < PurOrderObj.PRD_QTY_BALANCE) && (PurOrderObj.POR_QTY_ADDITIONAL > 0)) {
                isValid = false;
                GrandScriptUtils.ShowModal(PurchaseOrderConfig.AdditionalQtyValid, PurchaseOrderConfig.Information);
                return false;
            }
        }
        else {
            //            prdPK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_PK, grdID);
            //            for (var i in PurOrderList) {
            //                if (PurOrderList[i].POR_PR_DTL == prdPK) {
            //                    PurOrderList.splice(i, 1);
            //                }
            //            }
        }
    });
    if (isValid) {
        for (var arrCount = 0; arrCount < tempList.length; arrCount++) {
            PurOrderList.push(tempList[arrCount]);
        }

        $("#divData").data("ReqPOList", PurOrderList);
        PurchaseOrderRequestSummary();
    }
    $("#divPoListing").show();
    BindVendor();
    if (PurOrderList != null && PurOrderList.length > 0) {
        $("#VendorSelection").show();
        $("#divContinue").show();
        if ($("[id$='hdfShowPoOtherVendor']").val() == "1") {
            $("#divOtherVendor").show();
            HideOtherVendor();
        }
        $("[id$=hdfItemPoType]").val(PurOrderList[0].ITC_PO_ITEM_TYPE); //For Rule
        $("[id$=hdfPOCategoryVal]").val(PurOrderList[0].PRH_PO_CATEGORY); // For Workflow
    }
    else {
        $("#VendorSelection").hide();
        $("#divContinue").hide();
        $("#divOtherVendor").hide();
    }
    return false;
}

function ShowTraceability() {
    $("#divRelatedWidget").dialog("open");
    $("#divRelatedWidget").dialog(
        {
            width: 420,
            title: "Traceability"
        });
    return false;

}

function PurchaseOrderRequestSummary() {
    //<summary>function used to list the selected pending purchase request summary list</summary>
    var PurOrderList = $("#divData").data("ReqPOList");
    var summaryPOList = new Array();
    var summaryPOObj;
    var approvedQty = 0;
    for (var i in PurOrderList) {
        var summaryPOObj = JSLINQ(summaryPOList)
            .Where(function (item) { return item.POR_ITEM == PurOrderList[i].POR_ITEM; })
            .FirstOrDefault(null);
        if (summaryPOObj == null) {
            summaryPOObj = new Object();
            summaryPOObj.SLNO = summaryPOList.length + 1;
            summaryPOObj.POR_ITEM = PurOrderList[i].POR_ITEM;
            summaryPOObj.POR_UOM = PurOrderList[i].POR_UOM;
            summaryPOObj.PRD_UOM_TEXT = PurOrderList[i].PRD_UOM_TEXT;
            summaryPOObj.IO_NO = PurOrderList[i].IO_NO;

            summaryPOObj.PRH_COMPANY = PurOrderList[i].PRH_COMPANY;
            summaryPOObj.PRH_COMPANY_TEXT = PurOrderList[i].PRH_COMPANY_TEXT;
            summaryPOObj.PRH_ISSUE_DEPT = PurOrderList[i].PRH_ISSUE_DEPT;
            summaryPOObj.PRH_ISSUE_DEPT_TEXT = PurOrderList[i].PRH_ISSUE_DEPT_TEXT;
            summaryPOObj.PRH_GROUP = PurOrderList[i].PRH_GROUP;
            summaryPOObj.PRH_GROUP_TEXT = PurOrderList[i].PRH_GROUP_TEXT;

            //new Fields
            summaryPOObj.PRH_PK = PurOrderList[i].PRH_PK;
            summaryPOObj.PRH_DEPT = PurOrderList[i].PRH_DEPT;
            summaryPOObj.PRH_NO = PurOrderList[i].PRH_NO;
            summaryPOObj.PRD_DATE = PurOrderList[i].PRD_DATE;
            summaryPOObj.ITM_CODE = PurOrderList[i].ITM_CODE;
            summaryPOObj.ITM_NAME = PurOrderList[i].ITM_NAME;
            summaryPOObj.PRD_ITEM_SPEC = PurOrderList[i].PRD_ITEM_SPEC;
            summaryPOObj.PRD_PURPOSE = PurOrderList[i].PRD_PURPOSE;
            summaryPOObj.PRD_QTY_BALANCE = PurOrderList[i].PRD_QTY_BALANCE;
            summaryPOObj.PRD_REQD_DATE = PurOrderList[i].PRD_REQD_DATE;

            summaryPOObj.ITM_TEXT = PurOrderList[i].ITM_TEXT;
            summaryPOObj.ITV_PRICE = PurOrderList[i].ITV_PRICE;
            summaryPOObj.PRD_QTY_APPROVED = parseFloat(PurOrderList[i].PRD_QTY_APPROVED);
            summaryPOObj.POR_QTY_ORDERED = parseFloat(parseFloat(PurOrderList[i].POR_QTY_ORDERED) +
                parseFloat(PurOrderList[i].POR_QTY_ADDITIONAL)).toFixed(QtyDec);
            if ($("[id$=hdfEnbleCostCenter]").val() == "1") {
                summaryPOObj.POR_COST_CENTER = PurOrderList[i].POR_COST_CENTER;
                summaryPOObj.POR_COST_CENTER_TEXT = PurOrderList[i].POR_COST_CENTER_TEXT;
            }
            summaryPOList.push(summaryPOObj);
        }
        else {
            summaryPOObj.PRD_QTY_APPROVED = parseFloat(parseFloat(summaryPOObj.PRD_QTY_APPROVED) +
                parseFloat(PurOrderList[i].PRD_QTY_APPROVED)).toFixed(QtyDec);
            approvedQty = parseFloat(parseFloat(summaryPOObj.POR_QTY_ORDERED) +
                parseFloat(PurOrderList[i].POR_QTY_ORDERED) +
                parseFloat(PurOrderList[i].POR_QTY_ADDITIONAL)).toFixed(QtyDec);
            summaryPOObj.POR_QTY_ORDERED = approvedQty;

        }
    }
    $("#divData").data("ReqPOSummaryList", summaryPOList);
    GrandGrid.MakeGrid($("#grdPOList"), 0, summaryPOList);
    if (summaryPOList.length > 0) {
        var purchaseOrderPK = parseInt($("[id$=PurchaseOrderID]").val());
        var purchaseOrderStatus = parseInt($("[id$=PurchaseOrderStatus]").val());
        FillVendor(purchaseOrderPK, purchaseOrderStatus);
    }
}

function FilterPR() {
    //<summary>function used to rebind when the vendor select</summary>
    //    ClearAllSelectedItem();
    //    BindPendingPRGrid();
}

function HidePoDetails() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imbHidePoDetails").hide();
    $("#imbShowPoDetails").show();
    $("#divPoDetails").hide();
}

function ShowPoDetails() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imbHidePoDetails").show();
    $("#imbShowPoDetails").hide();
    $("#divPoDetails").show();
}

function HideTerms() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imbHideTerm").hide();
    $("#imbShowTerms").show();
    $("#divTerms").hide();
}

function ShowTerms() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imbHideTerm").show();
    $("#imbShowTerms").hide();
    $("#divTerms").show();
}

function HidePoItems() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgHidePOItems").hide();
    $("#imgShowPOItems").show();
    $("#divPOItems").hide();
}

function ShowPoItems() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imgHidePOItems").show();
    $("#imgShowPOItems").hide();
    $("#divPOItems").show();
}

function HideAttachment() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgHideAttachment").hide();
    $("#imgShowAttachment").show();
    $("#divAttachment").hide();
}

function ShowAttachment() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imgHideAttachment").show();
    $("#imgShowAttachment").hide();
    $("#divAttachment").show();
}

function ShowVendor() {
    //<summary>Function Used to Show Vendor Panel </summary>
    $("#imgVendorHide").show();
    $("#imgVendorShow").hide();
    $("#divVendors").show();
}

function HideVendor() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgVendorHide").hide();
    $("#imgVendorShow").show();
    $("#divVendors").hide();

}

function ShowOtherVendor() {
    //<summary>Function Used to Show Vendor Panel </summary>
    $("#imgOtherVendorHide").show();
    $("#imgOtherVendorShow").hide();
    $("#divSelectOtherVendor").show();
}

function HideOtherVendor() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgOtherVendorHide").hide();
    $("#imgOtherVendorShow").show();
    $("#divSelectOtherVendor").hide();

}


function ShowShortClose() {
    //<summary>Function Used to Show Vendor Panel </summary>
    $("#imgShrtCloseHide").show();
    $("#imgShrtCloseShow").hide();
    $("#divShortCloseInfo").show();
}

function HideShortClose() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgShrtCloseHide").hide();
    $("#imgShrtCloseShow").show();
    $("#divShortCloseInfo").hide();

}

function ShowVendorDtl() {
    //<summary>Function Used to Show Vendor Detail </summary>
    $("#imgVendorDtlHide").show();
    $("#imgVendorDtlShow").hide();
    // $("#divVendorDtl").show();
    $("#divBillingDtls").show();
    $("#divShippingDtl").show();
    $("#divVndDtls").show();
}

function HideVendorDtl() {
    //<summary>Function Used to Hide Vendor Detail </summary>
    $("#imgVendorDtlHide").hide();
    $("#imgVendorDtlShow").show();
    // $("#divVendorDtl").hide();
    $("#divBillingDtls").hide();
    $("#divShippingDtl").hide();
    $("#divVndDtls").hide();
}


function ShowPRShow() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imgPRHide").show();
    $("#imgPRShow").hide();
    $("#divPendingPR").show();
}

function HidePRShow() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgPRHide").hide();
    $("#imgPRShow").show();
    $("#divPendingPR").hide();
}

function ClearAllSelectedItem() {
    //<summary>function used to clear all grids</summary>
    $("#divData").data("ReqPOSummaryList", new Array());
    $("#divData").data("ReqPOList", new Array());
    GrandGrid.MakeGrid($("#grdPOList"), 0, new Array());
    $("#divData").data("ReqPODetails", new Array());
    GrandGrid.MakeGrid($("#grdPODetails"), 0, new Array());
    $("#divData").data("TaxDetails", new Array());
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, new Array());
    $("#divPoListing").hide();
    $("#VendorSelection").hide();
    $("#divOtherVendor").hide();
    $("#divContinue").hide();
    var SBUCompany = $("[id$=hdfSBUcompany]").val();
    $("[id$=hdfCompany]").val(SBUCompany);
    $("[id$=VEN_PK]").val('0');
    BindVendor();
    IsDirectPO = false;
    return false;
}

function FillVendorTerms(vendorID, termsID, IsEdit) {
    //<summary>function used to bind the vendor terms corr. to the vendor</summary>
    var drpID = $("select[id$=VENDOR_TERMS]").attr("id");
    $.get(PurchaseOrderConfig.GetVendorTerms + vendorID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, termsID);

        if (termsID != 0 && termsID != undefined) {
            termsIDArray = termsID.split(',');
            for (var a in termsIDArray) {
                FillAllVendorTermsDetails(termsIDArray[a]);
            }
        }
        else {
            if (IsEdit != "true")
                for (var a in data) {
                    FillAllVendorTermsDetails(data[a].Value);
                }
        }


    });
}


//During the page load bind all vendor terms details
function FillAllVendorTermsDetails(termsID) {
    if (termsID != "0") {
        var flag = true;
        for (var i in VendorTerms) {
            if (VendorTerms[i] == termsID)
                flag = false;
        }
        if (flag) {
            VendorTerms.push(termsID);
            $.get(PurchaseOrderConfig.GetVendorTerms + PurchaseOrderConfig.VendorPK + "&TermsId=" + termsID, function (data) {
                if (data.length > 0) {
                    //                    var temp = $("[id$=LblPOH_VENDOR_TERMS]").html();
                    //$("[id$=LblPOH_VENDOR_TERMS]").html(temp + data[0].VTD_VAL + "<br/>");
                    var temp = $("[id$=txtVendorTermText]").val();
                    $("[id$=txtVendorTermText]").val(temp + data[0].VTD_VAL + "\n");
                }
            });
        }
        else {
            // GrandScriptUtils.ShowModal(PurchaseOrderConfig.VendorTermsDuplicationMsg, PurchaseOrderConfig.Information);
        }
    }
}


function FillVendorTermsDetails(termsID) {
    //<summary>function used to fill selected vendor terms details</summary>
    var temp = $("[id$=txtVendorTermText]").val();
    if (temp == "")
        ClearTerms('Vendor');
    if (termsID != "0") {
        var flag = true;
        for (var i in VendorTerms) {
            if (VendorTerms[i] == termsID)
                flag = false;
        }
        if (flag) {
            VendorTerms.push(termsID);
            //            var temp = $("[id$=LblPOH_VENDOR_TERMS]").html();

            $.get(PurchaseOrderConfig.GetVendorTerms + PurchaseOrderConfig.VendorPK + "&TermsId=" + termsID, function (data) {
                if (data.length > 0) {
                    //                    $("[id$=LblPOH_VENDOR_TERMS]").html(temp + data[0].VTD_VAL + "<br/>");
                    $("[id$=txtVendorTermText]").val(temp + data[0].VTD_VAL + "\n");
                }
            });
        }
        else {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.VendorTermsDuplicationMsg, PurchaseOrderConfig.Information);
        }
    }
}
function ClearEmptyTerms(type) {
    var temp = $("[id$=txtTermText]").val();
    if (temp == '')
        ClearTerms(type);
    return false;
}

function FillGeneralTerms(termsID) {
    //<summary>function used to bind the general terms and also fill the selected terms in list</summary>
    //    var temp = $("[id$=LblPOH_TERMS]").html();
    var temp = $("[id$=txtTermText]").val();
    if (temp == '')
        ClearTerms('General');
    if (!termsID) {
        var drpID = $("[id$=GENERAL_TERMS]").attr("id");
        $.get(PurchaseOrderConfig.GetGeneralTerms, function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        });
    }
    else {
        if (termsID != "0") {
            var flag = true;
            for (var i in Terms) {
                if (Terms[i] == termsID)
                    flag = false;
            }
            if (flag) {
                Terms.push(termsID);
                $.get(PurchaseOrderConfig.GetGeneralTerms + "&TermsID=" + termsID, function (data) {
                    if (data.length > 0) {
                        //                        $("[id$=LblPOH_TERMS]").html(temp + data[0].TMDDESCRIPTION + "<br/>");
                        $("[id$=txtTermText]").val(temp + data[0].TMDDESCRIPTION + '\n');

                    }
                });
            }
            else {
                GrandScriptUtils.ShowModal(PurchaseOrderConfig.GeneralTermsDuplicationMsg, PurchaseOrderConfig.Information);
            }
        }
    }
}

function POContinue() {
    //<summary>function used to continue with the selected purchase request</summary>
    if ($("#divVendors").find("[name=rdoVendors]:checked").length > 0 || $("[id$=VEN_PK]").val() > 0) {
        if ($("[id$=hdfIsReqDeptPostback]").val() == "1") {
            PurchaseOrderConfig.VendorPK = $("[id$=hdfSelectedVendorPK]").val();
        } else {
            PurchaseOrderConfig.VendorPK = $("#divVendors").find("[name=rdoVendors]:checked").val();
        }
        if ($("[id$=VEN_PK]").val() > 0)
            PurchaseOrderConfig.VendorPK = $("[id$=VEN_PK]").val();
        GetVendorRates(PurchaseOrderConfig.VendorPK);
        SetDefaultPOcategory();
        $("select[id$=POH_COMPANY]").val($("[id$=hdfCompany]").val());
        var PurOrderList = $("#divData").data("ReqPOList");
        if (PurOrderList != null && PurOrderList.length > 0) {
            FillPRHType(PurOrderList[0].PRH_PO_CATEGORY, true);

        }
        else {
            FillPRHType(0, false);
        }
    }
    else
        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoVendors, PurchaseOrderConfig.Information);
    if ($("[id$=hdfEnableGlovePR]").val() == "1" && $("[id$=POH_IS_GLOVE]").val() == "1") {
        $("[id$=imbAddItem]").hide();
    }
    return false;
}

function SetDefaultPOcategory() {
    var PurOrderList = $("#divData").data("ReqPOList");
    if (PurOrderList.length > 0) {
        $("[id$=POH_ITEM_TYPE]").val(PurOrderList[0].ITC_PO_ITEM_TYPE);
        $("[id$=hdfDefaultPOCategory]").val(PurOrderList[0].ITC_PO_ITEM_TYPE);
        $("[id$=POH_GROUP]").val(PurOrderList[0].PRH_GROUP);
        $("[id$=POH_IS_GLOVE]").val(PurOrderList[0].PRH_IS_GLOVE);
    }
    return false;

}
function GetVendorRates(vendorID) {
    if (vendorID != "0") {
        var purOrderList = $("#divData").data("ReqPOList");
        var summaryPOList = $("#divData").data("ReqPOSummaryList");
        var VendorRates = new Object();
        VendorRates.ItemDetails = new Array();
        var comments = "";
        var prevComment = "";

        for (var i in summaryPOList) {
            {
                if (summaryPOList[i].IO_NO != null && $.trim(summaryPOList[i].IO_NO) != "") {
                    comments += comments == "" ? ($("[id$=hdfPoCommentPrefix]").val() + summaryPOList[i].IO_NO) : (", " + summaryPOList[i].IO_NO);
                }
                //Delete Duplicate IO Number
                for (var j = parseInt(i) + 1; j < summaryPOList.length; j++)
                    if (summaryPOList[i].IO_NO == summaryPOList[j].IO_NO) {
                        summaryPOList[j].IO_NO = "";
                    }
            }
            VendorRates.ItemDetails.push(new Object({ ITM_PK: summaryPOList[i].POR_ITEM }));
        }
        //PR Purpose field datas should come under PO comment box
        if (comments != "")
            comments += "\n";
        prevComment = "";
        for (var k in summaryPOList) {
            if (summaryPOList[k].PRD_PURPOSE != null && $.trim(summaryPOList[k].PRD_PURPOSE) != "" && prevComment != summaryPOList[k].PRD_PURPOSE) {
                comments += summaryPOList[k].PRD_PURPOSE + "\n";
            }
            prevComment = summaryPOList[k].PRD_PURPOSE;
        }
        var selectedItems = JSON.stringify(VendorRates);
        $.post(PurchaseOrderConfig.GetVendorRates + vendorID, selectedItems, function (data) {
            if (data != null && data.Rates.length > 0) {
                for (var dataItem in data.Rates) {
                    var purOrderObj = JSLINQ(summaryPOList)
                        .Where(function (item) { return item.POR_ITEM == data.Rates[dataItem].ITV_ITEM; })
                        .FirstOrDefault(null);
                    if (purOrderObj != null) {
                        purOrderObj.ITV_PRICE = parseFloat(data.Rates[dataItem].ITV_PRICE).toFixed(RateDec);
                        purOrderObj.ITV_PRICE_PREV = parseFloat(data.Rates[dataItem].ITV_PRICE_PREV).toFixed(RateDec);
                    }
                    //FillType(data.Rates[0].VEN_PO_TYPE);
                }
                $("#divData").data("ReqPOSummaryList", summaryPOList);
                if (data.Taxes != null && data.Taxes.PODetails != null) {
                    if (!$.isArray(data.Taxes.PODetails)) {
                        var itemTaxObj = data.Taxes.PODetails;
                        data.Taxes.PODetails = new Array();
                        data.Taxes.PODetails.push(itemTaxObj);
                    }
                    $("#divData").data("ReqPOTaxDetails", data.Taxes.PODetails);
                }
                FillVendorDetailsContinueAction(PurchaseOrderConfig.VendorPK);
                $("[id$=POH_VENDOR]").val(PurchaseOrderConfig.VendorPK);
                FillVendorTerms(PurchaseOrderConfig.VendorPK, 0);
                FillVendorMaterials(PurchaseOrderConfig.VendorPK);
                $("[id$=POH_COMMENTS]").val(comments);
                $("[id$=btnSave]").show();
                $("[id$=btnSubmit]").show();
            }
            else//Juno
            {
                //                for (var dataItem in data.Rates) {
                //                    var purOrderObj = JSLINQ(summaryPOList)
                //                          .Where(function (item) { return item.POR_ITEM == data.Rates[dataItem].ITV_ITEM; })
                //                          .FirstOrDefault(null);
                //                    if (purOrderObj != null) {
                //                        purOrderObj.ITV_PRICE = parseFloat(data.Rates[dataItem].ITV_PRICE).toFixed(RateDec);
                //                        purOrderObj.ITV_PRICE_PREV = parseFloat(data.Rates[dataItem].ITV_PRICE_PREV).toFixed(RateDec);
                //                    }
                //                    //FillType(data.Rates[0].VEN_PO_TYPE);
                //                }
                $("#divData").data("ReqPOSummaryList", summaryPOList);
                //                if (data.Taxes != null && data.Taxes.PODetails != null) {
                //                    if (!$.isArray(data.Taxes.PODetails)) {
                //                        var itemTaxObj = data.Taxes.PODetails;
                //                        data.Taxes.PODetails = new Array();
                //                        data.Taxes.PODetails.push(itemTaxObj);
                //                    }
                //                    $("#divData").data("ReqPOTaxDetails", data.Taxes.PODetails);
                //                }
                FillVendorDetailsContinueAction(PurchaseOrderConfig.VendorPK);
                $("[id$=POH_VENDOR]").val(PurchaseOrderConfig.VendorPK);
                FillVendorTerms(PurchaseOrderConfig.VendorPK, 0);
                FillVendorMaterials(PurchaseOrderConfig.VendorPK);
                $("[id$=POH_COMMENTS]").val(comments);
                $("[id$=btnSave]").show();
                $("[id$=btnSubmit]").show();
            }
        });
    }
}

function FillVendorDetailsContinueAction(vendorID) {
    //<summary>function used to fill vendor details</summary>
    if (vendorID != "0") {
        $.get(PurchaseOrderConfig.GetVendorDetailsByID + vendorID, function (data) {
            if (data.length > 0) {
                $("[id$=hdfVendorCurrency]").val(data[0].VEN_CURRENCY);
                $("[id$=hdfCreator]").val(data[0].POH_EMPLOYEE);
                if ($("[id$=hdfIsPostbackDirectPO]").val() != "1") { //For resolving Bug ID:  24273:Some times while creating direct po currency is not getting loaded
                    $("[id$=POH_CURRENCY]").val(data[0].VEN_CURRENCY);
                }
                else {
                    FillCurrency(data[0].VEN_CURRENCY);
                    FillEmployeeCreator(data[0].POH_EMPLOYEE);
                }
                $("[id$=VendorName]").html(data[0].VEN_NAME);
                $("[id$=VendorNameText]").val(data[0].VEN_NAME);

                $("[id$=ContactName]").html(data[0].VEN_CONT_NAME);
                $("[id$=TinNo]").html(data[0].VEN_TIN);
                $("[id$=VendorAddressDtls]").html(data[0].ADDRESS);
                $("[id$=VendorCurrencyCode]").val(data[0].VEN_CURRENCY_TEXT);
                $("[id$=hdfVendorCountry]").val(data[0].VEN_CNTRY);
                FillType(data[0].VEN_PO_TYPE);
            }
            BindPODetails();
            ActiveCreatePO();
            //Load popup tax grid with respect  Vendor
            FillVendorHeaderTax(PurchaseOrderConfig.VendorPK);
            //end
            $("[id$=hdfIsPostbackDirectPO]").val("0");
        });
    }
}

function FillVendorMaterials(vendorID) {
    //<summary>function used to material corr to the vendors</summary>
    //  var drpID = $("select[id$=ITM_CODE]").attr("id");
    if (vendorID != 0) {

        //        $.get(PurchaseOrderConfig.GetVendorMaterials + vendorID + "&StoreID=" + $("[id$=Store]").val() + "&Amend=" + $("[id$=POH_IS_AMEND]").val(), function (data) {
        //            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        //            if (IsDirectPO) {
        //                if (data != null)
        //                    IsMaterialMapped = data.length > 0 ? true : false;
        //                if (IsMaterialMapped == true) {
        //                    FillVendorDetailsContinueAction(PurchaseOrderConfig.VendorPK);
        //                    $("[id$=POH_VENDOR]").val(PurchaseOrderConfig.VendorPK);
        //                    FillVendorTerms(PurchaseOrderConfig.VendorPK, 0);

        //                    $("[id$=POH_COMMENTS]").val("");
        //                    $("[id$=btnSave]").show();
        //                    $("[id$=btnSubmit]").show();  
        //                    $("#divVendor").dialog("close");
        //                }
        //                else {
        //                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoMaterialMapped, PurchaseOrderConfig.Information);
        //                    PurchaseOrderConfig.VendorPK = 0;
        //                }
        //            }
        //        });

        $.get(PurchaseOrderConfig.GetVendorMaterials + vendorID + "&StoreID=" + $("[id$=Store]").val() + "&Amend=" + $("[id$=POH_IS_AMEND]").val(), function (data) {
            if (IsDirectPO) {
                if (data != null)
                    IsMaterialMapped = data.length > 0 ? true : false;
                if (IsMaterialMapped == true) {

                }
                else {
                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoMaterialMapped, PurchaseOrderConfig.Information);
                    PurchaseOrderConfig.VendorPK = 0;
                }
            }
            GrandScriptUtils.MakeAutoCompleteLimitLen("txtItemCode", PurchaseOrderConfig.GetVendorMaterials + vendorID + "&StoreID=" + $("[id$=Store]").val() + "&Amend=" + $("[id$=POH_IS_AMEND]").val() + "&AUTOSEARCH=1",
                "ITM_CODE", true, false, "ITM_CODE", true, "", "", "", $("[id$=AutoStartValue]").val(), afterAutoComplete()); //For Avoiding postback effect(Overlay) append "&AUTOSEARCH=1" with QueryString
        });

        if ($("[id$=hdfEnableGlovePR]").val() == "1" && $("[id$=POH_IS_GLOVE]").val() == "1") {
            $("[id$=imbAddItem]").hide();
        }
    }
}

function SetInitialPortList(type) {
    var poType = 0;
    poType = type == null ? $("[id$=POH_TYPE]").val() : type;
    //PO Type 1-> Import  2-> Local
    poType = poType == "1" ? PurchaseOrderConfig.Overseas : PurchaseOrderConfig.Domestic;

    GrandScriptUtils.MakeAutoComplete("POH_FROM_PORT_TEXT", PurchaseOrderConfig.GetPortDetailsURL + poType + "&PurFromPort=1", "POH_FROM_PORT", true, false, "BizUnitPk", true, null, null, null, true);
    GrandScriptUtils.MakeAutoComplete("POH_TO_PORT_TEXT", PurchaseOrderConfig.GetPortDetailsURL + poType + "&PurToPort=1", "POH_TO_PORT", true, false, "BizUnitPk", true, null, null, null, true);
    return false;
}

function afterAutoComplete() {
    if (IsDirectPO) {
        if (IsMaterialMapped == true) {
            FillVendorDetailsContinueAction(PurchaseOrderConfig.VendorPK);
            $("[id$=POH_VENDOR]").val(PurchaseOrderConfig.VendorPK);
            FillVendorTerms(PurchaseOrderConfig.VendorPK, 0);

            $("[id$=POH_COMMENTS]").val("");
            $("[id$=btnSave]").show();
            $("[id$=btnSubmit]").show();
            $("#divVendor").dialog("close");
        }
        else {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoMaterialMapped, PurchaseOrderConfig.Information);
            PurchaseOrderConfig.VendorPK = 0;
        }
    }
}

// if any more function want to done after the result is selected from auto complete
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "txtItemCode") {
        FillMaterialDetails($("[id$=ITM_CODE]").val());
    }
    if (targetControlID == "txtVendorName") {
        $('input:radio').removeAttr('checked');
    }
}

function FillMaterialDetails(materialID) {
    //<summary>function used to fill the selected material details</summary>
    $.get(PurchaseOrderConfig.GetVendorMaterialDetails + PurchaseOrderConfig.VendorPK + "&MaterialId=" + materialID, function (data) {
        if (data.length > 0) {
            $("[id$=POD_TAX]").val("0");
            $("[id$=POD_DISCOUNT]").val("0");
            $("[id$=POD_QTY]").val("0");
            $("[id$=POD_RATE]").val(data[0].ITM_PRICE);
            $("[id$=POD_RATE_PREV]").val(data[0].ITM_PRICE);
            $("[id$=PerPiecePrice]").val(data[0].ITM_PRICE);
            $("[id$=POD_AMOUNT]").val("0");
            $("[id$=POD_SUBTOTAL]").val("0");
            $("[id$=POD_UOM]").val(data[0].UOM_CODE);
            $("[id$=UOM_PK]").val(data[0].ITM_UOM);
            if (data[0].ITM_IS_PM != 1 || $("[id$=hdfShowDescPM]").val() == 1)//In the case of Packing Material no need to show comments(Based on Config). ITM_IS_PM==1  means packing material
            {
                $("textarea[id$=POD_REMARKS]").val(data[0].ITM_DESC);
            }
        }
        else {
            ClearPRDetails();
        }
    });
}

function ClearPRDetails() {
    //<summary>function used to clear pr add details</summary>
    $("[id$=POD_TAX]").val("");
    $("[id$=POD_DISCOUNT]").val("");
    $("[id$=POD_QTY]").val("");
    $("[id$=POD_RATE]").val("");
    $("[id$=PerPiecePrice]").val("");
    $("[id$=POD_AMOUNT]").val("");
    $("[id$=POD_SUBTOTAL]").val("");
    $("[id$=POD_UOM]").val("");
    $("[id$=POD_REMARKS]").val("");
}

function BindPODetails() {
    //<summary>function used to fill the selected purchase request summary to orginal po list </summary>

    var summaryPOList = $("#divData").data("ReqPOSummaryList");
    var PurOrderObj;
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    var PRItemTaxDetails = $("#divData").data("ReqPOTaxDetails");
    var TaxDetails = $("#divData").data("TaxDetails");
    if (TaxDetails == null)
        TaxDetails = new Array();
    for (var i in summaryPOList) {
        var PurOrderObj = JSLINQ(PurOrderDetails)
            .Where(function (item) { return item.POD_ITEM == summaryPOList[i].POR_ITEM; })
            .FirstOrDefault(null);
        if (PurOrderObj == null) {
            PurOrderObj = new Object();
            PurOrderObj.POD_PK = 0;
            PurOrderObj.POD_PO = 0;
            //PurOrderObj.POD_SL_NO = PurOrderDetails.length + 1;
            if (PurOrderObj.POD_SL_NO == null || PurOrderObj.POD_SL_NO == 0) {
                var maxSlNo = JSLINQ(PurOrderDetails)
                    .Max(function (poitem) { return parseInt(poitem.POD_SL_NO); });
                PurOrderObj.POD_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
            }
            PurOrderObj.POD_ITEM = summaryPOList[i].POR_ITEM;
            PurOrderObj.POD_QTY_REQUESTED = summaryPOList[i].POR_QTY_ORDERED;
            PurOrderObj.POD_UOM = summaryPOList[i].POR_UOM;
            PurOrderObj.UOM_CODE = summaryPOList[i].PRD_UOM_TEXT;
            PurOrderObj.ITM_TEXT = summaryPOList[i].ITM_TEXT;
            PurOrderObj.POD_CONV_FACT = 1;
            PurOrderObj.POD_RATE = parseFloat(summaryPOList[i].ITV_PRICE).toFixed(RateDec);
            if (!isNaN(summaryPOList[i].ITV_PRICE_PREV))
                PurOrderObj.POD_RATE_PREV = parseFloat(summaryPOList[i].ITV_PRICE_PREV).toFixed(RateDec);
            PurOrderObj.POD_AMOUNT = parseFloat(summaryPOList[i].ITV_PRICE * summaryPOList[i].POR_QTY_ORDERED).toFixed(AmtDec);
            PurOrderObj.POD_TAX_PERC = 1;
            PurOrderObj.POD_TAX = 0;
            PurOrderObj.POD_DISC_PERC = 1;
            PurOrderObj.POD_DISC_AMT = 0;
            PurOrderObj.POD_AMT_VALUE = parseFloat(summaryPOList[i].ITV_PRICE * summaryPOList[i].POR_QTY_ORDERED).toFixed(AmtDec);
            PurOrderObj.POD_REMARKS = summaryPOList[i].PRD_ITEM_SPEC;
            PurOrderObj.POD_REQD_DATE = summaryPOList[i].PRD_REQD_DATE;
            PurOrderObj.POD_DEPT = 0;
            PurOrderObj.PRDetails = new Array();
            PurOrderObj.TaxDetails = new Array();
            if (PRItemTaxDetails != null) {
                var prItemTaxObj = JSLINQ(PRItemTaxDetails)
                    .Where(function (item) { return item.POD_ITEM == PurOrderObj.POD_ITEM; }).FirstOrDefault(null);
                if (prItemTaxObj != null) {
                    if (!$.isArray(prItemTaxObj.TaxDetails) && prItemTaxObj.TaxDetails != null) {
                        var itemTaxObj = prItemTaxObj.TaxDetails;
                        prItemTaxObj.TaxDetails = new Array();
                        prItemTaxObj.TaxDetails.push(itemTaxObj);
                    }
                    var prItemTaxDtl = prItemTaxObj.TaxDetails;
                    if (prItemTaxDtl != null && prItemTaxDtl.length > 0) {
                        for (var itm in prItemTaxDtl) {
                            prItemTaxDtl[itm].POT_SL_NO = PurOrderObj.POD_SL_NO;
                            prItemTaxDtl[itm].IsHeader = false;
                            TaxDetails.push(prItemTaxDtl[itm]);
                            PurOrderObj.TaxDetails.push(prItemTaxDtl[itm]);
                        }
                    }
                }
            }
            PurOrderDetails.push(PurOrderObj);
        }
        else {
            PurOrderObj.POD_QTY_REQUESTED = summaryPOList[i].POR_QTY_ORDERED;
            PurOrderObj.POD_CONV_FACT = 1;
            PurOrderObj.POD_RATE = parseFloat(summaryPOList[i].ITV_PRICE).toFixed(RateDec);
            if (!isNaN(summaryPOList[i].ITV_PRICE_PREV))
                PurOrderObj.POD_RATE_PREV = parseFloat(summaryPOList[i].ITV_PRICE_PREV).toFixed(RateDec);
            PurOrderObj.POD_AMOUNT = summaryPOList[i].ITV_PRICE * summaryPOList[i].POR_QTY_ORDERED;
            PurOrderObj.POD_AMT_VALUE = summaryPOList[i].ITV_PRICE * summaryPOList[i].POR_QTY_ORDERED;
        }
    }
    if (summaryPOList != undefined && summaryPOList.length == 0) {
        ClearTotalDetails();
    }
    $("#divData").data("ReqPODetails", PurOrderDetails);
    $("#divData").data("TaxDetails", TaxDetails);
    GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
    //Take Tome
    for (var item in PurOrderDetails) {
        UpdateLineItemTax(PurOrderDetails[item].POD_SL_NO, true);
    }
}

function AddPODetails() {
    //<summary>function used to add the additional po material</summary>

    var PurOrderDetails = $("#divData").data("ReqPODetails");
    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        var PurOrderObj = JSLINQ(PurOrderDetails)
            .Where(function (item) { return item.POD_ITEM == $("[id$=ITM_CODE]").val(); })
            .FirstOrDefault(null);
        if (PurOrderObj == null) {
            PurOrderObj = new Object();
            PurOrderObj.POD_PK = 0;
            PurOrderObj.POD_PO = 0;
            //PurOrderObj.POD_SL_NO = PurOrderDetails.length + 1;
            if (PurOrderObj.POD_SL_NO == null || PurOrderObj.POD_SL_NO == 0) {
                var maxSlNo = JSLINQ(PurOrderDetails)
                    .Max(function (poitem) { return parseInt(poitem.POD_SL_NO); });
                PurOrderObj.POD_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
            }
            PurOrderObj.POD_ITEM = $("[id$=ITM_CODE]").val();
            PurOrderObj.POD_QTY_REQUESTED = $("[id$=POD_QTY]").val();
            PurOrderObj.POD_UOM = $("[id$=UOM_PK]").val();
            PurOrderObj.UOM_CODE = $("[id$=POD_UOM]").val();
            //            PurOrderObj.ITM_TEXT = $("[id$=ITM_CODE] :selected").text();
            PurOrderObj.ITM_TEXT = $("[id$=txtItemCode]").val();

            PurOrderObj.POD_CONV_FACT = 1;
            PurOrderObj.POD_RATE = parseFloat($("[id$=POD_RATE]").val()).toFixed(RateDec);
            if (!isNaN($("[id$=POD_RATE_PREV]").val()))
                PurOrderObj.POD_RATE_PREV = parseFloat($("[id$=POD_RATE_PREV]").val()).toFixed(RateDec);
            PurOrderObj.POD_AMOUNT = parseFloat(PurOrderObj.POD_QTY_REQUESTED * PurOrderObj.POD_RATE).toFixed(RateDec);
            PurOrderObj.POD_TAX_PERC = 1;
            PurOrderObj.POD_TAX = $("[id$=POD_TAX]").val();
            PurOrderObj.POD_DISC_PERC = 1;
            PurOrderObj.POD_DISC_AMT = $("[id$=POD_DISCOUNT]").val();
            PurOrderObj.POD_AMT_VALUE = $("[id$=POD_SUBTOTAL]").val();
            PurOrderObj.POD_REMARKS = $("[id$=POD_REMARKS]").val();
            PurOrderObj.POD_REQD_DATE = $("[id$=POD_REQD_DATE]").val();
            PurOrderObj.POD_DEPT = 0;
            PurOrderObj.PRDetails = new Array();
            PurOrderObj.TaxDetails = new Array();
            PurOrderDetails.push(PurOrderObj);
            $("#divData").data("ReqPODetails", PurOrderDetails);
            GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
            ClearPODetails();
            if ($("[id$=POH_IS_AMEND]").val() == "1") {
                $("[id$=hdfIsNew]").val('1');
            }
            SaveItemTaxDiscountApply();

        }
        else if (PurchaseOrderConfig.SlNo > 0) {
            var currSlNo = PurchaseOrderConfig.SlNo;
            //  Amend Only
            if ($("[id$=POH_IS_AMEND]").val() == "1") {
                $.get(PurchaseOrderConfig.GetGrnQty + PurOrderObj.POD_PK, function (data) {
                    if (data.length > 0) {
                        var qtyReveived = data[0].GRD_QTY_RECEIVED;
                        var qtyInvoiced = data[0].GRD_QTY_INVOICED;
                        //Grn Quantity
                        if (parseFloat($("[id$=POD_QTY]").val()) < parseFloat(qtyReveived)) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.QtyGRN, PurchaseOrderConfig.Information);
                            return false;
                        }
                        //Invoiced Quantity
                        if (parseFloat($("[id$=POD_QTY]").val()) < parseFloat(qtyInvoiced)) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.QtyInvoice, PurchaseOrderConfig.Information);
                            return false;
                        }

                        PurOrderObj.POD_ITEM = $("[id$=ITM_CODE]").val();
                        PurOrderObj.POD_QTY_REQUESTED = $("[id$=POD_QTY]").val();
                        PurOrderObj.POD_RATE = parseFloat($("[id$=POD_RATE]").val()).toFixed(RateDec);
                        PurOrderObj.POD_RATE_PREV = parseFloat($("[id$=POD_RATE]").val()).toFixed(RateDec);
                        PurOrderObj.POD_AMOUNT = $("[id$=POD_AMOUNT]").val();
                        PurOrderObj.POD_AMT_VALUE = $("[id$=POD_SUBTOTAL]").val();
                        PurOrderObj.POD_REMARKS = $("[id$=POD_REMARKS]").val();
                        PurOrderObj.POD_REQD_DATE = $("[id$=POD_REQD_DATE]").val();
                        $("#divData").data("ReqPODetails", PurOrderDetails);
                        UpdateLineItemTax(currSlNo, "", PurOrderObj.POD_RATE);
                        GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
                        ClearPODetails();
                        //UpdateLineItemTax(slNo, amount);                        
                        SaveItemTaxDiscountApply();

                    }
                });
            }
            else {
                PurOrderObj.POD_ITEM = $("[id$=ITM_CODE]").val();
                PurOrderObj.POD_QTY_REQUESTED = $("[id$=POD_QTY]").val();
                PurOrderObj.POD_RATE = parseFloat($("[id$=POD_RATE]").val()).toFixed(RateDec);
                PurOrderObj.POD_RATE_PREV = parseFloat($("[id$=POD_RATE]").val()).toFixed(RateDec);
                PurOrderObj.POD_AMOUNT = $("[id$=POD_AMOUNT]").val();
                PurOrderObj.POD_AMT_VALUE = $("[id$=POD_SUBTOTAL]").val();
                PurOrderObj.POD_REMARKS = $("[id$=POD_REMARKS]").val();
                PurOrderObj.POD_REQD_DATE = $("[id$=POD_REQD_DATE]").val();
                $("#divData").data("ReqPODetails", PurOrderDetails);
                UpdateLineItemTax(currSlNo, "", PurOrderObj.POD_RATE);
                GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
                ClearPODetails();
                //UpdateLineItemTax(slNo, amount);
                SaveItemTaxDiscountApply();
            }


        }
        else {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.ItemAdded, PurchaseOrderConfig.Information);
        }
    }
    return false;
}

function ClearPODetails() {
    //<summary>function used to clear material details </summary>

    PurchaseOrderConfig.SlNo = 0;
    $("[id$=ITM_CODE]").val("0");
    $("[id$=POD_QTY]").val("");
    $("[id$=UOM_PK]").val("0");
    $("[id$=POD_UOM]").val("");
    $("[id$=POD_RATE]").val("");
    $("[id$=POD_AMOUNT]").val("");
    $("[id$=POD_TAX]").val("");
    $("[id$=POD_DISCOUNT]").val("");
    $("[id$=POD_SUBTOTAL]").val("");
    $("[id$=POD_REMARKS]").val("");
    $("[id$=ITM_CODE]").removeAttr("disabled");
}

//**********PO Qty Amend  Start Region ***************************************************************************
function BindPRGridPopupAmend(POD_PK) {

    GrandGrid.MakeGrid($("#grdAmendPRDetails"), 0, new Array());
    var PurOrderListAll = $("#divData").data("ReqPOList");
    var PurOrderListArray = new Array();
    if ($.isArray(PurOrderListAll)) {
        PurOrderListArray = PurOrderListAll;
    }
    else {
        PurOrderListArray.push(PurOrderListAll);
    }
    if (PurOrderListArray.length > 0) {
        //***Here we get PR Details.ie,Rate is PR Rate. PO Rate against this item will vary. So Update the PR Rate with PO Rate(For Resolving Bug ID:  2971)
        for (var i in PurOrderListArray) {
            var PurOrderDetails = $("#divData").data("ReqPODetails");
            var PurOrderObj = JSLINQ(PurOrderDetails)
                .Where(function (item) { return item.POD_PK == PurOrderListArray[i].POR_PO_DTL; })
                .FirstOrDefault(null);
            if (PurOrderObj == null) {
            }
            else {
                PurOrderListArray[i].ITV_PRICE = parseFloat(PurOrderObj.POD_RATE).toFixed(RateDec);
            }
        }
        //***End******
        var PurOrderListArray = JSLINQ(PurOrderListArray).Where(function (item) { return item.POR_PO_DTL == POD_PK; }).ToArray();
        GrandGrid.MakeGrid($("#grdAmendPRDetails"), 0, PurOrderListArray);
        $("#divAmendPRDetails").dialog("open");
        $("#divAmendPRDetails").dialog(
            {
                width: 800
            });
    }
    else {
        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoRecordFound, PurchaseOrderConfig.Information);
    }
}

function UpdateAmendPOPRDetails() {
    var grdID;
    var colIndex = 0;
    var floatRegQty = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
    var floatAddRegQty = new RegExp("^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
    var isValid = true;
    var PurOrderObj;
    var prdPK = 0;
    var PurOrderList = $("#divData").data("ReqPOList");
    var tempList = new Array();

    $("#grdAmendPRDetails tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        PorPK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POR_PK, grdID);
        var PurOrderObj = JSLINQ(PurOrderList)
            .Where(function (item) { return item.POR_PK == PorPK; })
            .FirstOrDefault(null);
        if (PurOrderObj == null) {
        }
        else {
            PurOrderObj.POR_PK = PorPK;
        }
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ORDERED, grdID);
        if (colIndex) {
            PurOrderObj.POR_QTY_ORDERED = parseFloat($(this).find("td:eq(" + colIndex + "): input[type=text]").val());
            if (!floatAddRegQty.test(PurOrderObj.POR_QTY_ORDERED)) {
                isValid = false;
                GrandScriptUtils.ShowModal(String.format(PurchaseOrderConfig.OrderQtyNumeric.fontcolor("red"), QtyDec), PurchaseOrderConfig.Information);
                return false;
            }
        }
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ADDITIONAL, grdID);
        if (colIndex) {
            PurOrderObj.POR_QTY_ADDITIONAL = parseFloat($(this).find("td:eq(" + colIndex + "): input[type=text]").val());
            if (!floatAddRegQty.test(PurOrderObj.POR_QTY_ADDITIONAL)) {
                isValid = false;
                GrandScriptUtils.ShowModal(String.format(PurchaseOrderConfig.OrderAddQtyNumeric.fontcolor("red"), QtyDec), PurchaseOrderConfig.Information);
                return false;
            }
        }
        if (PurOrderObj.POR_QTY_ORDERED > PurOrderObj.PRD_QTY_APPROVED) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.OrderQtyValid, PurchaseOrderConfig.Information);
            return false;
        }
        else if ((PurOrderObj.POR_QTY_ORDERED < PurOrderObj.PRD_QTY_APPROVED) && (PurOrderObj.POR_QTY_ADDITIONAL > 0)) {
            isValid = false;
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.AdditionalQtyValid, PurchaseOrderConfig.Information);
            return false;
        }

    });
    if (isValid) {
        $("#divData").data("ReqPOList", PurOrderList);
        PurchaseOrderRequestSummaryAmend();
        $("#divAmendPRDetails").dialog("close");
        BindPODetails();
    }

}

function PurchaseOrderRequestSummaryAmend() {
    //<summary>function used to list the selected  purchase request summary list</summary>
    var PurOrderList = $("#divData").data("ReqPOList");
    var summaryPOList = new Array();
    var summaryPOObj;
    var approvedQty = 0;
    for (var i in PurOrderList) {
        var summaryPOObj = JSLINQ(summaryPOList)
            .Where(function (item) { return item.POR_ITEM == PurOrderList[i].POR_ITEM; })
            .FirstOrDefault(null);
        if (summaryPOObj == null) {
            summaryPOObj = new Object();
            summaryPOObj.SLNO = summaryPOList.length + 1;
            summaryPOObj.POR_ITEM = PurOrderList[i].POR_ITEM;
            summaryPOObj.POR_UOM = PurOrderList[i].POR_UOM;
            summaryPOObj.PRD_UOM_TEXT = PurOrderList[i].PRD_UOM_TEXT;
            summaryPOObj.IO_NO = PurOrderList[i].IO_NO;

            //new Fields
            summaryPOObj.PRH_NO = PurOrderList[i].PRH_NO;
            summaryPOObj.PRD_DATE = PurOrderList[i].PRD_DATE;
            summaryPOObj.ITM_CODE = PurOrderList[i].ITM_CODE;
            summaryPOObj.ITM_NAME = PurOrderList[i].ITM_NAME;
            summaryPOObj.PRD_ITEM_SPEC = PurOrderList[i].PRD_ITEM_SPEC;
            summaryPOObj.PRD_QTY_BALANCE = PurOrderList[i].PRD_QTY_BALANCE;
            summaryPOObj.PRD_REQD_DATE = PurOrderList[i].PRD_REQD_DATE;

            summaryPOObj.ITM_TEXT = PurOrderList[i].ITM_TEXT;
            summaryPOObj.ITV_PRICE = PurOrderList[i].ITV_PRICE;
            summaryPOObj.PRD_QTY_APPROVED = parseFloat(PurOrderList[i].PRD_QTY_APPROVED);
            summaryPOObj.POR_QTY_ORDERED = parseFloat(parseFloat(PurOrderList[i].POR_QTY_ORDERED) +
                parseFloat(PurOrderList[i].POR_QTY_ADDITIONAL)).toFixed(QtyDec);
            summaryPOList.push(summaryPOObj);
        }
        else {
            summaryPOObj.PRD_QTY_APPROVED = parseFloat(parseFloat(summaryPOObj.PRD_QTY_APPROVED) +
                parseFloat(PurOrderList[i].PRD_QTY_APPROVED)).toFixed(QtyDec);
            approvedQty = parseFloat(parseFloat(summaryPOObj.POR_QTY_ORDERED) +
                parseFloat(PurOrderList[i].POR_QTY_ORDERED) +
                parseFloat(PurOrderList[i].POR_QTY_ADDITIONAL)).toFixed(QtyDec);
            summaryPOObj.POR_QTY_ORDERED = approvedQty;
        }
    }
    $("#divData").data("ReqPOSummaryList", summaryPOList);
    GrandGrid.MakeGrid($("#grdPOList"), 0, summaryPOList);
}

function IsValidPOAmendQuantity() {
    if (POObject.PODetails.length > 0) {
        var PurOrderList = POObject.PODetails;
        for (var i in PurOrderList) {
            if (parseFloat(PurOrderList[i].POD_QTY_REQUESTED) < parseFloat(PurOrderList[i].POD_QTY_INVOICED)) {
                GrandScriptUtils.ShowModal(PurchaseOrderConfig.InvalidOrderQtyInvoiced, PurchaseOrderConfig.Information);
                return false;
            }
            if (parseFloat(PurOrderList[i].POD_QTY_REQUESTED) < parseFloat(PurOrderList[i].POD_QTY_GRN_RECEIVED)) {
                GrandScriptUtils.ShowModal(PurchaseOrderConfig.InvalidOrderQtyGRN, PurchaseOrderConfig.Information);
                return false;
            }
        }
    }
    return true;
}

//*****************************End PO Qty Amend ****************************************************************************************


function AddLineItemTax(slNo, amount) {
    //<summary>function used to add the item tax details</summary>
    if ($("[id$=isEditMode]").val() == "1") {
        $("[id$=hdnSlNo]").val(slNo);
        var itemAmt = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
        var itemDisc = parseFloat($("#POD_DISC_AMT_" + slNo).val());
        amount = itemAmt - (isNaN(itemDisc) ? 0 : itemDisc);
    }
    RemoveAllValidations();
    FillTaxDiscount(1);

    $("#divTaxApplicableAmount").hide(); //Hide/Show Tax Applicable Amount div

    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(LineItemTaxDetails)"
        });
    PurchaseOrderConfig.ItemTaxPK = slNo;
    $("[id$=ItemAmount]").val(amount);
    $("[id$=IsLine]").val(1);
    $("[id$=IsLineDiscount]").val(1);
    ClearPopUp(slNo, 1, true);
    ClearTaxDetails();
    //Clone TaxDetails Data to TempDetails   
    var TaxDetails = $("#divData").data("TaxDetails");     //
    $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
    //End
}

function AddLineItemRate(slNo, qty, itempk, isRateUpdate, rateChangeReason) {
    //<summary>function used to add the item rate details</summary>
    var rate = parseFloat($("#POD_RATE_" + slNo).val());
    $.get(PurchaseOrderConfig.GetMaterialDtl + $("[id$=BizUnitPk]").val() + "&MaterialID=" + itempk, function (data) {
        if (data.length > 0) {
            $("[id$=ItemName]").val(data[0].ITM_TEXT);
        }
    });
    // As per  Manoj sir,Update in vendor rate checkbox should be checked by default.If Same rate,no need to insert into  item-vendor history table.     
    //    if (isRateUpdate != undefined && isRateUpdate == 1) 
    //        $("[id$=chkIsUpdateRate]").attr("checked", true);
    //    else
    //        $("[id$=chkIsUpdateRate]").attr("checked", false);
    $("[id$=chkIsUpdateRate]").attr("checked", true);

    $("[id$=hdnSlNo]").val(slNo);
    RemoveAllValidations();
    $("#divItemRate").dialog("open");
    $("#divItemRate").dialog(
        {
            width: 505,
            title: "Translate(LineItemRateDetails)"
        });
    PurchaseOrderConfig.ItemTaxPK = slNo;
    var vname = $("[id$=VendorName]").html();
    //    $("[id$=VendorNameText]").val($("[id$=VendorName]").html());
    $("[id$=ItemQty]").val(parseFloat(qty).toFixed(QtyDec));
    // $("[id$=TaxRate]").val(rate);
    $("[id$=TaxRate]").val(parseFloat(rate).toFixed(RateDec));
    $("[id$=txtRateChangeReason]").val(rateChangeReason);
    $("input[id$=TaxRate]").rules("add", {
        required: true,
        ZeroDecimal: true
        //DecimalDigits: RateDec,
        //CustomDecimal: true,
        //messages: { required: "Translate(ReqRate)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", RateDec) }
    });
}

function AddLineItemDiscAmount(slNo, amount) {
    //<summary>function used to add the item discount details</summary>

    var discount = parseFloat($("#POD_DISC_AMT_" + slNo).val());
    amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());

    $("[id$=hdnSlNo]").val(slNo);
    RemoveAllValidations();
    $("#divItemDiscount").dialog("open");
    $("#divItemDiscount").dialog(
        {
            width: 435,
            title: "Translate(LineItemDiscountDetails)"
        });
    PurchaseOrderConfig.ItemTaxPK = slNo;
    $("[id$=ItemDiscAmount]").val(amount);
    $("[id$=TaxDiscAmount]").val(discount);


    $("input[id$=TaxDiscAmount]").rules("add", {

        required: true,
        messages: { required: "Translate(ReqDiscount)" }
    });


}

function AddLineItemDiscount(slNo, amount) {
    //<summary>function used to add the item discount details</summary>

    RemoveAllValidations();
    FillTaxDiscount(3);
    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(LineItemDiscountDetails)"
        });
    PurchaseOrderConfig.ItemTaxPK = slNo;
    $("[id$=ItemAmount]").val(amount);
    $("[id$=IsLine]").val(1);
    $("[id$=IsLineDiscount]").val(3);
    ClearPopUp(slNo, 3, true);
    ClearTaxDetails();
}

function AddItemHeaderDiscount() {
    //<summary>function used to add the po discount details</summary>
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    if (PurOrderDetails.length > 0) {
        RemoveAllValidations();
        FillTaxDiscount(3);

        $("#divTaxApplicableAmount").hide(); //Hide/Show Tax Applicable Amount div

        $("#divItemTax").dialog("open");
        $("#divItemTax").dialog(
            {
                width: 540,
                title: "Translate(DiscountDetails)"
            });
        PurchaseOrderConfig.ItemTaxPK = $("[id$=ITM_CODE]").val();
        $("[id$=ItemAmount]").val($("[id$=POH_SUB_TOTAL]").val());
        $("[id$=IsLine]").val(2);
        $("[id$=IsLineDiscount]").val(3);
        ClearPopUp($("[id$=ITM_CODE]").val(), 3, false);
        ClearTaxDetails();
        //Clone TaxDetails Data to TempDetails   
        var TaxDetails = $("#divData").data("TaxDetails");
        $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object        
    }
    else {

        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoItemAdded, PurchaseOrderConfig.Information);
    }

}

function AddItemHeaderShipping() {
    //<summary>function used to add the po shipping details</summary>
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    if (PurOrderDetails.length > 0) {
        RemoveAllValidations();
        FillTaxDiscount(2);

        $("#divTaxApplicableAmount").hide(); //Hide/Show Tax Applicable Amount div

        $("#divItemTax").dialog("open");
        $("#divItemTax").dialog(
            {
                width: 540,
                title: "Other Charges"
            });
        PurchaseOrderConfig.ItemTaxPK = $("[id$=ITM_CODE]").val();
        $("[id$=ItemAmount]").val($("[id$=POH_SUB_TOTAL]").val());
        $("[id$=IsLine]").val(2);
        $("[id$=IsLineDiscount]").val(2);
        ClearPopUp($("[id$=ITM_CODE]").val(), 2, false);
        ClearTaxDetails();
        //Clone TaxDetails Data to TempDetails   
        var TaxDetails = $("#divData").data("TaxDetails");     //
        $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
        //End
    }
    else {

        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoItemAdded, PurchaseOrderConfig.Information);
    }

}

function AddItemHeaderTax() {
    //<summary>function used to add the po tax details</summary>
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    $("#divTxRatePer").hide();
    //     if (PurOrderDetails.length > 0) {
    RemoveAllValidations();
    FillTaxDiscount(1);

    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(TaxDetails)"
        });
    PurchaseOrderConfig.ItemTaxPK = $("[id$=ITM_CODE]").val();

    if ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem) {
        ResetTaxApplicableCheckbox();
        SetTaxApplicableAmount();
        GetFormula(parseInt($("[id$=ChooseTax]").val()));
        $("#divTaxApplicableAmount").show(); //Hide/Show Tax Applicable Amount div
    }
    else {
        var hdrAmt = parseFloat($("[id$=POH_SUB_TOTAL]").val());
        var hdrDisc = parseFloat($("[id$=POH_DISC_AMT]").val());
        //Other Charge Tax Calculation Based On Configuration
        var hdrOtherCharge = parseFloat($("[id$=POH_SHIP_CHARGE]").val());
        var amount;
        var amountTemp;
        if ($("[id$=IsTaxForOtherCharge]").val() == "1") {
            amountTemp = (isNaN(hdrAmt) ? 0 : hdrAmt) - (isNaN(hdrDisc) ? 0 : hdrDisc);
            amount = amountTemp + (isNaN(hdrOtherCharge) ? 0 : hdrOtherCharge);
        }
        else {
            amount = (isNaN(hdrAmt) ? 0 : hdrAmt) - (isNaN(hdrDisc) ? 0 : hdrDisc);
        }
        // End----
        $("[id$=ItemAmount]").val(amount.toFixed(AmtDec));
        //$("[id$=ItemAmount]").val($("[id$=POH_SUB_TOTAL]").val());
    }
    $("[id$=IsLine]").val(2);
    $("[id$=IsLineDiscount]").val(1);
    ClearPopUp($("[id$=ITM_CODE]").val(), 1, false);
    ClearTaxDetails();
    //Clone TaxDetails Data to TempDetails   
    var TaxDetails = $("#divData").data("TaxDetails");     //
    $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
    //End
    //     }

    //     else {

    //         GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoItemAdded, PurchaseOrderConfig.Information);
    //     }


}

function ClearPopUp(slNo, type, isLine) {
    //<summary>function used to bind the item tax/ discount details</summary>

    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, type, isLine));
}

function SaveTaxDiscount() {
    //<summary>function to save tax/discount details</summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    var slNo = parseInt(PurchaseOrderConfig.ItemTaxPK);
    var type = parseInt($("[id$=ChooseTax]").val());
    var taxName = $("[id$=TaxName]").val();
    AddValidations(2);
    var TaxDetailsObj = new Object();
    if ($(document.forms[0]).valid()) {
        if (parseInt($("[id$=IsLine]").val()) == 1) {
            if (slNo > 0) {
                if (PurchaseOrderConfig.EditTax == 0) {
                    if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                        if (!ValidateDiscountTotal(slNo)) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount, PurchaseOrderConfig.Information);
                            return false;
                        }
                    }
                    if (type == 1) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == type && tax.POT_TYPE == "1"; })
                            .FirstOrDefault(null);
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_NAME == taxName && (tax.POT_TYPE == "1" || tax.POT_TYPE == "2"); })
                            .FirstOrDefault(null);
                        //                        TaxDetailsObj = JSLINQ(TaxDetails)
                        //                                    .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_NAME == taxName && tax.POT_TYPE == "2"; })
                        //                                    .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IsHeader = false;
                        TaxDetailsObj.POT_SL_NO = slNo;
                        TaxDetailsObj.POT_PK = 0;
                        TaxDetailsObj.POT_PO_DTL = 0;
                        TaxDetailsObj.POT_TAX = parseInt(type) > 0 ? type : 0;
                        TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);
                        TaxDetailsObj.POT_NAME = $("[id$=TaxName]").val();
                        TaxDetailsObj.POT_TYPE = parseInt(type) > 0 ? "1" : "2";

                        TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                        TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                    }
                }
                else {
                    if (PurchaseOrderConfig.EditTax == type) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == PurchaseOrderConfig.EditTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.POT_TAX = type;
                            TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                            TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);
                            TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                            TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == type; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                        }
                    }
                }
                $("#divData").data("TaxDetails", TaxDetails);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, $("[id$=IsLineDiscount]").val(), true));
            }
        }
        else if (parseInt($("[id$=IsLine]").val()) == 2) {
            if (PurchaseOrderConfig.EditTax == 0) {
                //                TaxDetailsObj = JSLINQ(TaxDetails)
                //                          .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true; })
                //                          .FirstOrDefault(null);
                if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                    if (!ValidateDiscountTotal()) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount, PurchaseOrderConfig.Information);
                        return false;
                    }
                }
                if (type == 1) {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true && tax.POT_TYPE == "1"; })
                        .FirstOrDefault(null);
                }
                else {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_NAME == taxName && tax.IsHeader == true && (tax.POT_TYPE == "1" || tax.POT_TYPE == "2"); })
                        .FirstOrDefault(null);

                }

                if (TaxDetailsObj == null) {
                    TaxDetailsObj = new Object();
                    TaxDetailsObj.IsHeader = true;
                    TaxDetailsObj.POT_PK = 0;
                    TaxDetailsObj.POT_SL_NO = 0;
                    TaxDetailsObj.POT_PO_DTL = 0;
                    TaxDetailsObj.POT_TAX = parseInt(type) > 0 ? type : 0;
                    TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                    TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);;
                    TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                    TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    TaxDetailsObj.POT_NAME = $("[id$=TaxName]").val();
                    TaxDetailsObj.POT_TYPE = parseInt(type) > 0 ? "1" : "2";
                    TaxDetails.push(TaxDetailsObj);
                }
                else {
                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                }
            }
            else {
                if (PurchaseOrderConfig.EditTax == type) {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_TAX == PurchaseOrderConfig.EditTax && tax.IsHeader == true; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        TaxDetailsObj.POT_TAX = type;
                        TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val());
                        TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                        TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    }
                }
                else {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                    }
                }
            }
            $("#divData").data("TaxDetails", TaxDetails);
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, $("[id$=IsLineDiscount]").val(), false));
        }
        ClearTaxDetails();
    }
    return false;
}

function AddTaxDiscount() {
    //<summary>function to Add tax/discount details into temporary object</summary>

    var TempDetails = $("#divData").data("TempDetails");
    var TaxDetails = $("#divData").data("TaxDetails");
    var slNo = parseInt(PurchaseOrderConfig.ItemTaxPK);
    var type = parseInt($("[id$=ChooseTax]").val());
    var taxName = $("[id$=TaxName]").val();
    // Move Already Applied tax/Discount from Taxdetails to Temporary object
    var IsAlreadyHave = new Object();
    for (var i in TaxDetails) {
        if (TaxDetails[i].POT_TAX_CATEGORY == $("[id$=IsLineDiscount]").val()) {
            //Checking If the current item is present in temp object
            IsAlreadyHave = JSLINQ(TempDetails)
                .Where(function (tax) { return tax.POT_NAME == TaxDetails[i].POT_NAME && (tax.POT_TYPE == "1" || tax.POT_TYPE == "2"); })
                .FirstOrDefault(null);
            if (IsAlreadyHave == null) {
                TempDetails.push(TaxDetails[i]);
            }
        }
    }
    AddValidations(2);
    var TaxDetailsObj = new Object();
    if ($(document.forms[0]).valid()) {
        if (parseInt($("[id$=IsLine]").val()) == 1) {
            if (slNo > 0) {
                if (PurchaseOrderConfig.EditTax == 0) {
                    if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                        if (!ValidateDiscountTotal(slNo)) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount, PurchaseOrderConfig.Information);
                            return false;
                        }
                    }
                    if (type == 1) {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == type && tax.POT_TYPE == "1"; })
                            .FirstOrDefault(null);
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_NAME == taxName && (tax.POT_TYPE == "1" || tax.POT_TYPE == "2"); })
                            .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IsHeader = false;
                        TaxDetailsObj.POT_SL_NO = slNo;
                        TaxDetailsObj.POT_PK = 0;
                        TaxDetailsObj.POT_PO_DTL = 0;
                        TaxDetailsObj.POT_TAX = parseInt(type) > 0 ? type : 0;
                        TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);
                        TaxDetailsObj.POT_NAME = $("[id$=TaxName]").val();
                        TaxDetailsObj.POT_TYPE = parseInt(type) > 0 ? "1" : "2";
                        TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                        TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                        TempDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                    }
                }
                else {
                    if (PurchaseOrderConfig.EditTax == type) {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == PurchaseOrderConfig.EditTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.POT_TAX = type;
                            TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                            TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);
                            TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                            TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == type; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                        }
                    }
                }
                $("#divData").data("TempDetails", TempDetails);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(slNo, $("[id$=IsLineDiscount]").val(), true));
            }
        }
        else if (parseInt($("[id$=IsLine]").val()) == 2) {
            if (PurchaseOrderConfig.EditTax == 0) {
                //                TaxDetailsObj = JSLINQ(TempDetails)
                //                          .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true; })
                //                          .FirstOrDefault(null);
                if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                    if (!ValidateDiscountTotal()) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount, PurchaseOrderConfig.Information);
                        return false;
                    }
                }
                if (type == 1) {
                    TaxDetailsObj = JSLINQ(TempDetails)
                        .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true && tax.POT_TYPE == "1"; })
                        .FirstOrDefault(null);
                }
                else {
                    TaxDetailsObj = JSLINQ(TempDetails)
                        .Where(function (tax) { return tax.POT_NAME == taxName && tax.IsHeader == true && (tax.POT_TYPE == "1" || tax.POT_TYPE == "2"); })
                        .FirstOrDefault(null);

                }

                if (TaxDetailsObj == null) {
                    TaxDetailsObj = new Object();
                    TaxDetailsObj.IsHeader = true;
                    TaxDetailsObj.POT_PK = 0;
                    TaxDetailsObj.POT_SL_NO = 0;
                    TaxDetailsObj.POT_PO_DTL = 0;
                    TaxDetailsObj.POT_TAX = parseInt(type) > 0 ? type : 0;
                    TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                    TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);;
                    TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                    TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    TaxDetailsObj.POT_NAME = $("[id$=TaxName]").val();
                    TaxDetailsObj.POT_TYPE = parseInt(type) > 0 ? "1" : "2";

                    if ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem && TaxDetailsObj.POT_TAX_CATEGORY == 1) {
                        TaxDetailsObj.POT_HAS_SUB_TOTAL = ($("[id$=chkSubTotal]").is(":checked")) ? "1" : "0";
                        TaxDetailsObj.POT_HAS_DISCOUNT = ($("[id$=chkDiscount]").is(":checked")) ? "1" : "0";
                        TaxDetailsObj.POT_HAS_OTHER_CHARGE = ($("[id$=chkOtherCharges]").is(":checked")) ? "1" : "0";
                    }
                    else {
                        TaxDetailsObj.POT_HAS_SUB_TOTAL = 0;
                        TaxDetailsObj.POT_HAS_DISCOUNT = 0;
                        TaxDetailsObj.POT_HAS_OTHER_CHARGE = 0;
                    }

                    TempDetails.push(TaxDetailsObj);
                }
                else {
                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                }
            }
            else {
                if (PurchaseOrderConfig.EditTax == type) {
                    TaxDetailsObj = JSLINQ(TempDetails)
                        .Where(function (tax) { return tax.POT_TAX == PurchaseOrderConfig.EditTax && tax.IsHeader == true; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        TaxDetailsObj.POT_TAX = type;
                        TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val());
                        TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                        TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    }
                }
                else {
                    TaxDetailsObj = JSLINQ(TempDetails)
                        .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                    }
                }
            }
            $("#divData").data("TempDetails", TempDetails);
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(slNo, $("[id$=IsLineDiscount]").val(), false));
        }
        ClearTaxDetails();
    }
    return false;
}


function GetTempDetails(slNo, type, isLine) {
    //<summary>function used to get the tax / discount details </summary>
    var TaxDetails = $("#divData").data("TempDetails");
    var TaxArray = new Array();
    for (var i in TaxDetails) {
        if (isLine) {
            if (TaxDetails[i].POT_SL_NO == slNo && TaxDetails[i].POT_TAX_CATEGORY == type) {
                TaxArray.push(TaxDetails[i]);
            }
        }
        else {
            if (TaxDetails[i].POT_TAX_CATEGORY == type && TaxDetails[i].IsHeader) {
                TaxArray.push(TaxDetails[i]);
            }
        }
    }
    return TaxArray;
}


function DeleteTempDetails() {
    //<summary>function used to delete the tax details from temporary object</summary>

    var TaxDetails = $("#divData").data("TempDetails");
    if (parseInt($("[id$=IsLine]").val()) == 1) {
        for (var i in TaxDetails) {
            if (PurchaseOrderConfig.EditTax > 0) {
                if (TaxDetails[i].POT_SL_NO == PurchaseOrderConfig.ItemTaxPK && TaxDetails[i].POT_TAX == PurchaseOrderConfig.EditTax
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
            else {
                if (TaxDetails[i].POT_SL_NO == PurchaseOrderConfig.ItemTaxPK && TaxDetails[i].POT_NAME.trim() == PurchaseOrderConfig.EditTaxName.trim()
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
        }
        $("#divData").data("TempDetails", TaxDetails);
        GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(PurchaseOrderConfig.ItemTaxPK, $("[id$=IsLineDiscount]").val(), true));
    }
    else if (parseInt($("[id$=IsLine]").val()) == 2) {
        for (var i in TaxDetails) {
            if (PurchaseOrderConfig.EditTax > 0) {
                if (TaxDetails[i].POT_TAX == PurchaseOrderConfig.EditTax && TaxDetails[i].IsHeader == true
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
            else {
                if (TaxDetails[i].IsHeader == true && TaxDetails[i].POT_NAME.trim() == PurchaseOrderConfig.EditTaxName.trim()
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
        }
        $("#divData").data("TempDetails", TaxDetails);
        GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(PurchaseOrderConfig.ItemTaxPK, $("[id$=IsLineDiscount]").val(), false));
    }
    PurchaseOrderConfig.EditTax = 0;
    PurchaseOrderConfig.EditTaxName = "";
    PurchaseOrderConfig.EditTaxType = 0;
}

function ClearPopUpTemp(slNo, type, isLine) {
    //<summary>function used to bind the item tax/ discount details</summary>
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(slNo, type, isLine));
}


function SaveDiscount(type) {
    //<summary>function to save tax/discount details</summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    var slNo = parseInt(PurchaseOrderConfig.ItemTaxPK);
    //var type = parseInt($("[id$=ChooseTax]").val());
    var type = type;
    AddValidations(2);
    var TaxDetailsObj = new Object();
    if ($(document.forms[0]).valid()) {
        if (parseInt($("[id$=IsLine]").val()) == 1) {
            if (slNo > 0) {
                if (PurchaseOrderConfig.EditTax == 0) {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == type; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IsHeader = false;
                        TaxDetailsObj.POT_SL_NO = slNo;
                        TaxDetailsObj.POT_PK = 0;
                        TaxDetailsObj.POT_PO_DTL = 0;
                        TaxDetailsObj.POT_TAX = type;
                        TaxDetailsObj.POT_TAX_TEXT = "Discount";
                        //TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(3);
                        //Code to edit PO tax,rate,discount
                        if ($("[id$=isEditMode]").val() == "1") {
                            CalculateSubTotalValue();
                        }
                        TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                        TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                    }
                }
                else {
                    if (PurchaseOrderConfig.EditTax == type) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == PurchaseOrderConfig.EditTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.POT_TAX = type;
                            TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                            TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val()).toFixed(AmtDec);
                            TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                            TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == type; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                        }
                    }
                }
                $("#divData").data("TaxDetails", TaxDetails);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, $("[id$=IsLineDiscount]").val(), true));
            }
        }
        else if (parseInt($("[id$=IsLine]").val()) == 2) {
            if (PurchaseOrderConfig.EditTax == 0) {
                TaxDetailsObj = JSLINQ(TaxDetails)
                    .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true; })
                    .FirstOrDefault(null);
                if (TaxDetailsObj == null) {
                    TaxDetailsObj = new Object();
                    TaxDetailsObj.IsHeader = true;
                    TaxDetailsObj.POT_PK = 0;
                    TaxDetailsObj.POT_SL_NO = 0;
                    TaxDetailsObj.POT_PO_DTL = 0;
                    TaxDetailsObj.POT_TAX = type;
                    TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                    TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val());
                    TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                    TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    TaxDetails.push(TaxDetailsObj);
                }
                else {
                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                }
            }
            else {
                if (PurchaseOrderConfig.EditTax == type) {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_TAX == PurchaseOrderConfig.EditTax && tax.IsHeader == true; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        TaxDetailsObj.POT_TAX = type;
                        TaxDetailsObj.POT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.POT_TAX_AMT = parseFloat($("[id$=TaxAmount]").val());
                        TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                        TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    }
                }
                else {
                    TaxDetailsObj = JSLINQ(TaxDetails)
                        .Where(function (tax) { return tax.POT_TAX == type && tax.IsHeader == true; })
                        .FirstOrDefault(null);
                    if (TaxDetailsObj != null) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TypeAlreadyAdded, PurchaseOrderConfig.Information);
                    }
                }
            }
            $("#divData").data("TaxDetails", TaxDetails);
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, $("[id$=IsLineDiscount]").val(), false));
        }
        ClearTaxDetails();
    }
    return false;
}

function SaveItemTaxDiscountApply() {
    //<summary>function to apply tax/discount the added details</summary>

    var taxAmount = 0;
    var slNo = 0;
    var columnIndex = 0;
    var amount = 0;


    //If the Entry is not new return false.
    if ($("[id$=hdfIsNew]").val() == '0')
        return;
    var currSlNo = $("[id$=hdnSlNo]").val();

    UpdateLineItemTax(currSlNo, true);
    //    if (parseInt(slNo) > 0) {
    //        UpdateLineItemTax(currSlNo, true);
    //    }
    //    else {
    TaxDetails = $("#divData").data("TaxDetails");
    if (parseInt($("[id$=IsLine]").val()) == 1) {
        $("#grdPODetails tr:has(td)").each(function (index) {
            taxAmount = 0;
            if (parseInt($("[id$=IsLineDiscount]").val()) == 1) {
                columnIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_TAX, "grdPODetails");
            }
            else if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                columnIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_DISC_AMT, "grdPODetails");
            }
            slNo = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_SL_NO, "grdPODetails");
            amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_AMOUNT, "grdPODetails");
            if (slNo != null && parseInt(slNo) == parseInt(PurchaseOrderConfig.ItemTaxPK)) {
                $(this).find("td:eq(" + columnIndex + ")").html("");
                if (parseInt($("[id$=IsLineDiscount]").val()) == 1) {
                    taxAmount = GetItemTaxAmount(slNo, 1);
                    //Code to edit PO tax,rate,discount
                    if ($("[id$=isEditMode]").val() == "1") {
                        var slNo = $("[id$=hdnSlNo]").val();
                        $("#POD_H_TAX_" + slNo).val(taxAmount);
                    }
                    SetPOItemTaxDiscount(true, slNo, taxAmount);
                    taxAmount = parseFloat(taxAmount).toFixed(AmtDec);
                    if (($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.ItemWise) || ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                        $(this).find("td:eq(" + columnIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />" + taxAmount + "");
                    }
                    else {
                        $(this).find("td:eq(" + columnIndex + ")").html(taxAmount);
                    }
                    //  $(this).find("td:eq(" + columnIndex + ")").html("<img id=\"IMG_TAX_" + slNo + "\" onclick=\"javascript:AddLineItemTax('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/tax.png\" alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />" + "<input type=\"text\" class=\"small\"  id=\"POD_H_TAX_" + slNo + "\" value=\"" + taxAmount + "\" maxLength =\"30\" tabIndex=\"20\" readonly   class=\"input-notheme\"  ></input>");
                }
                else if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                    taxAmount = GetItemTaxAmount(slNo, 3);
                    SetPOItemTaxDiscount(false, slNo, taxAmount);
                    $(this).find("td:eq(" + columnIndex + ")").html("<img id=\"IMG_DISC_" + slNo + "\" onclick=\"javascript:AddLineItemDiscountDtl('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/discount.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + "<input type=\"text\" id=\"POD_DISC_AMT_" + slNo + "\" value=\"" + parseFloat(taxAmount).toFixed(AmtDec) + "\" maxLength =\"30\" tabIndex=\"20\" readonly   class=\"numeric input-w70\"  ></input>");
                    //$(this).find("td:eq(" + columnIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscount('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + taxAmount + "");
                    // $(this).find("td:eq(" + columnIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscount('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/ERP-Blue/Buttons/deducttax.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + taxAmount + "");
                }
                CalculateLineSubTotal($(this));
                ReCalculateSubTotal();
                CalculateTotal();
            }
        });
    }
    else if (parseInt($("[id$=IsLine]").val()) == 2) {
        if (parseInt($("[id$=IsLineDiscount]").val()) == 1) {
            taxAmount = GetTaxHdrAmount(1);
            $("[id$=POH_ADD_TAX_AMT]").val(taxAmount.toFixed(AmtDec));
        }
        else if (parseInt($("[id$=IsLineDiscount]").val()) == 2) {
            taxAmount = GetTaxHdrAmount(2);
            $("[id$=POH_SHIP_CHARGE]").val(taxAmount.toFixed(AmtDec));
        }
        else if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
            taxAmount = GetTaxHdrAmount(3);
            $("[id$=POH_DISC_AMT]").val(taxAmount.toFixed(AmtDec));
        }
        CalculateTotal();
    }
    //    }
}

function SaveApply() {
    //<summary>function used to clear the tax/ discount details</summary>

    //CalculateSubTotalValue();

    var TempDetails = $("#divData").data("TempDetails");
    var TaxDetails = $("#divData").data("TaxDetails");

    //Remove all items under current tax category from Orginal Taxdetails 
    for (var j = 0; j < TaxDetails.length; j++) {
        //        if (TaxDetails[j].POT_TAX_CATEGORY == $("[id$=IsLineDiscount]").val() && TaxDetails[j].IsHeader) { //11_12_2014
        if (TaxDetails[j].POT_TAX_CATEGORY == $("[id$=IsLineDiscount]").val()) {
            TaxDetails.splice(j, 1);
            --j;
        }
    }
    // Add temporary details to original taxdetails 
    for (var i in TempDetails) {
        //        if (TempDetails[i].POT_TAX_CATEGORY == $("[id$=IsLineDiscount]").val() && TempDetails[i].IsHeader) { //11_12_2014
        if (TempDetails[i].POT_TAX_CATEGORY == $("[id$=IsLineDiscount]").val()) {
            TaxDetails.push(TempDetails[i]);
        }
    }

    $("#divData").data("TaxDetails", TaxDetails);
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, $("[id$=IsLineDiscount]").val(), true));
    //end 
    var slNo = $("[id$=hdnSlNo]").val();
    //    if (parseInt(slNo) > 0) {
    //        UpdateLineItemTax(slNo, true);
    //    }
    //    else {
    //        CalculateSubTotalValue();
    //    }
    AssingnRemarks();

    UpdateLineItemTax(slNo, true);
    $("#divItemTax").dialog("close");
    return false;
}

function SaveRateCancel() {
    //<summary>function used to close rate details</summary>

    $("#divItemRate").dialog("close");
    return false;
}

function SaveRateApply() {
    //<summary>function used to calculate valid rate details</summary>
    AssingnRemarks();
    if ($(document.forms[0]).valid()) {
        if (isNaN(parseFloat($("[id$=TaxRate]").val())) || parseInt(parseFloat($("[id$=TaxRate]").val())) < 0) {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.Pleaseenteravalidnumber);
            var slNo = $("[id$=hdnSlNo]").val();

            var PurOrderDetails = $("#divData").data("ReqPODetails");
            for (var i in PurOrderDetails) {
                if (PurOrderDetails[i].POD_SL_NO == slNo) {
                    $("[id$=TaxRate]").val(PurOrderDetails[i].POD_RATE);
                    PurOrderDetails[i].POD_RATE_UPDATE = $("[id$=chkIsUpdateRate]").is(":checked") == true ? "1" : "0";
                    break;
                }
            }
        }
        else {
            //Set Rate to Json
            var slNo = $("[id$=hdnSlNo]").val();
            rate = parseFloat($("[id$=TaxRate]").val());

            var PurOrderDetails = $("#divData").data("ReqPODetails");

            for (var i in PurOrderDetails) {
                if (PurOrderDetails[i].POD_SL_NO == slNo) {
                    PurOrderDetails[i].POD_RATE = rate; // parseFloat(rate).toFixed(AmtDec);
                    PurOrderDetails[i].POD_AMT_VALUE = GetItemSubTotalAmount(PurOrderDetails[i].POD_SL_NO).toFixed(AmtDec);
                    PurOrderDetails[i].POD_AMTOUNT = (parseFloat(rate) * parseFloat(PurOrderDetails[i].POD_QTY_REQUESTED).toFixed(AmtDec));
                    PurOrderDetails[i].POD_AMOUNT = (parseFloat(rate) * parseFloat(PurOrderDetails[i].POD_QTY_REQUESTED).toFixed(AmtDec));
                    PurOrderDetails[i].POD_RATE_UPDATE = $("[id$=chkIsUpdateRate]").is(":checked") == true ? "1" : "0";
                    PurOrderDetails[i].POD_REASON = $("[id$=txtRateChangeReason]").val();
                }
            }
            //New 04-11-2013
            $("#divData").data("ReqPODetails", PurOrderDetails);
            CalculateSubTotalValue();
            $("#divItemRate").dialog("close");
            SaveApply();
            $("[id$=TaxRate]").val("");
            //            UpdateLineItemTax(slNo);            
        }


    }
    return false;
}

function SaveDiscCancel() {
    //<summary>function used to close Discount details</summary>

    $("#divItemDiscount").dialog("close");
    return false;
}

function SaveDiscApply() {
    //<summary>function used to calculate valid  discount details</summary>

    if ($(document.forms[0]).valid()) {
        if (isNaN(parseFloat($("[id$=TaxDiscAmount]").val())) || parseInt(parseFloat($("[id$=TaxDiscAmount]").val())) < 0) {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.Pleaseenteravalidnumber);
        }
        else {
            var PurOrderDetails = $("#divData").data("ReqPODetails");
            var currSlNo = $("[id$=hdnSlNo]").val();
            var amount = parseFloat($("[id$=TaxDiscAmount]").val());
            for (var i in PurOrderDetails) {
                if (PurOrderDetails[i].POD_SL_NO == currSlNo) {
                    PurOrderDetails[i].POD_DISC_AMT = amount.toFixed(AmtDec);
                }
            }

            CalculateSubTotalValue();
            $("#divItemDiscount").dialog("close");
            $("[id$=TaxDiscAmount]").val("");
        }
    }
    return false;
}

function ClearTaxDetails() {
    //<summary>function used to clear the tax/ discount details</summary>

    PurchaseOrderConfig.EditTax = 0;
    PurchaseOrderConfig.EditTaxName = "";
    PurchaseOrderConfig.EditTaxType = 0;
    $("[id$=ChooseTax]").val("0");
    $("[id$=TaxName]").val("");
    $("[id$=txtPercentage]").val("");
    $("[id$=TaxAmount]").val("");
    $("[id$=TaxAmount]").attr("disabled", true);
    //$("[id$=TaxName]").removeAttr("disabled");
    //$("[id$=FinalAmount]").val("");
}

function IsAnyZeero() {
    var flag = false;
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    if ($("[id$=hdfZeroRateConfirm").val() == "1")
        return false;
    else
        for (var i in PurOrderDetails) {
            if (PurOrderDetails[i].POD_RATE == 0) {
                {
                    flag = true;
                }
                break;
            }
        }
    return flag;
}
function ConfirmCurrencyTypeChange(command) {

    var msgTitle;
    var msg;
    msgTitle = "Translate(Information)";
    msg = "Translate(ConfirmCurrencyType)";
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfCurrncyChangeConfirm]").val(1);
                $(this).dialog("close");
                SavePage(command);
            },
            Cancel: function (e) {
                $("[id$=hdfCurrncyChangeConfirm]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;

}

function ConfirmBudgetValidation(command) {

    var msgTitle;
    var msg;
    msgTitle = "Translate(Information)";
    msg = "Translate(PurchaseBudgetErrConfirm)";
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=POH_REQ_BUDG_VALID]").val(0);
                $("[id$=SubmitFlag]").val('0')
                $(this).dialog("close");
                SavePage(command);
            },
            Cancel: function (e) {
                $("[id$=POH_REQ_BUDG_VALID]").val(1);
                $("[id$=SubmitFlag]").val('0')
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;

}
function ConfirmZeroRateValidation(command) {

var msgTitle;
    var msg;
    msgTitle = "Translate(Information)";
    msg = "Translate(ZeroRateConfirm)";
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfZeroRateConfirm]").val(1);
                $("[id$=SubmitFlag]").val('0')
                $(this).dialog("close");
                SavePage(command);
            },
            Cancel: function (e) {
                $("[id$=hdfZeroRateConfirm]").val(0);
                $("[id$=SubmitFlag]").val('0')
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;

}
function POWkfSubmit() {
    if ($("[id$=POH_REQ_BUDG_VALID]").val() == 1) {
        var budget = null;
        var Investor = $("[id$=POH_INVESTOR_CODE]").val();
        var Currency = $("[id$=POH_CURRENCY]").val();
        var PODate = $("[id$=POH_DATE]").val();
        //$.get(PurchaseOrderConfig.GetPurchaseOrderProjectBudget + Investor + "&POH_CURRENCY=" + Currency + "&POH_DATE=" + PODate, function (data) {
        //    if (data) {
        //        budget = data;
        //    }
        //});
        $.ajax({
            url: PurchaseOrderConfig.GetPurchaseOrderProjectBudget + Investor + "&POH_CURRENCY=" + Currency + "&POH_DATE=" + PODate,
            dataType: 'json',
            async: false,
            //data: myData,
            success: function (data) {
                budget = data[0].BUDGET_BAL;
            }
        });
        var lbl = '<%= GetLocalResourceObject("RemaininBudget").ToString() %>';
        $("[id$=ucrWrkf_divPageComments]").show();
        $("[id$=lblPageComment]").html(PurchaseOrderConfig.RemainingInvestment + " " + budget);
    }
    return WkfSubmit();
}
function SavePage(command) {
    ////<summary>function used save PO </summary>
    $.get(PurchaseOrderConfig.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.SessionExpired.fontcolor("red"), PurchaseOrderConfig.ConfirmationMessage, PurchaseOrderConfig.LOGOUT, true);
            result = false;
        }
        else {
            AddValidations(1);

            $("[id$=PurchaseOrderListPostback]").val("");

            var ToCurency = $("[id$=POH_CURRENCY_BC]").val();
            var FromCurrency = $("[id$=POH_CURRENCY]").val();
            var PohDate = $("[id$=POH_DATE]").val();

            //Currency Change Validation
            if ($("[id$=hdfIsValidateCurrencyPoType").val() == "1") {//- 

                if ($("[id$=hdfEnablePOCurrencyValidation").val() == "true") {//1
                    //If vendor currency is base currency, and change PO type to Import, then need to confirm.
                    if (parseInt($("select[id$=POH_CURRENCY]").val()) == parseInt($("[id$=POH_CURRENCY_BC]").val())) {
                        if ($("select[id$=POH_TYPE]").val() != PurchaseOrderConfig.Local && $("[id$=hdfCurrncyChangeConfirm]").val() == "0") {
                            ConfirmCurrencyTypeChange(command);
                            return false;
                        }
                    }

                    if (parseInt($("select[id$=POH_CURRENCY]").val()) != parseInt($("[id$=POH_CURRENCY_BC]").val())) {
                        if ($("select[id$=POH_TYPE]").val() != PurchaseOrderConfig.Import && $("[id$=hdfCurrncyChangeConfirm]").val() == "0") {
                            ConfirmCurrencyTypeChange(command);
                            return false;
                        }
                    }
                } //1
                if ($("[id$=hdfEnablePOCountryValidation").val() == "true") {//2
                    if ($("[id$=hdfVendorCountry]").val() == $("[id$=hdfSBUCountry]").val()) {
                        if ($("select[id$=POH_TYPE]").val() != PurchaseOrderConfig.Local && $("[id$=hdfCurrncyChangeConfirm]").val() == "0") {
                            ConfirmCurrencyTypeChange(command);
                            return false;
                        }
                    }
                    if ($("[id$=hdfVendorCountry]").val() != $("[id$=hdfSBUCountry]").val()) {
                        if ($("select[id$=POH_TYPE]").val() != PurchaseOrderConfig.Import && $("[id$=hdfCurrncyChangeConfirm]").val() == "0") {
                            ConfirmCurrencyTypeChange(command);
                            return false;
                        }
                    }

                } //2
            } //-

            //End

            //Verify rate change Validation
            if ($("[id$=hdnVerificationRequired]").val() == "1") {
                var isChecked = $("input[id$=chkVerifyChanges]").is(':checked');
                if (isChecked == false) {
                    if ($("select[id$=WRKFACT_ID]").val() == $("[id$=hdfDefaultActionPk]").val()) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.VerifyRateChangesRequired.fontcolor("red"), PurchaseOrderConfig.Information);
                        return false;
                    }
                }
                else {
                    $("[id$=POH_VERIFIED]").val("1");
                }
            }

            if (!IsAnyZeero()) {
                $.post(PurchaseOrderConfig.GetExchangeRate + "&fromCurrency=" + FromCurrency + "&toCurrency=" + ToCurency + "&date=" + PohDate, function (data) {
                    if (data) {
                        var Rate = data[0].Column1;
                        $("[id$=POH_EXCHG_RATE]").val(data[0].Column1);
                        $("[id$=POH_TOTAL_VALUE_BC]").val(parseFloat($("[id$=POH_EXCHG_RATE]").val()) * parseFloat($("[id$=POH_TOTAL_VALUE]").val()))
                        if (Rate < 0) {
                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoExchangeRate, PurchaseOrderConfig.Information);
                            return false;
                        }

                        //Save Action start
                        if ($(document.forms[0]).valid()) {
                            if (!IsValidData()) {
                                GrandScriptUtils.ShowModal(PurchaseOrderConfig.EnterRequiredBy, PurchaseOrderConfig.Information);
                                return false;
                            }

                            //To Prevent Muliple Click
                            if ($("[id$=SubmitFlag]").val() == "0")
                                $("[id$=SubmitFlag]").val('1')
                            else
                                return false;

                            $('#updateProgress').show();
                            // AssignTerms();
                            AssingPODetails();

                            //PO Amend Validation Start
                            if ($("[id$=POH_IS_AMEND]").val() == "1") {
                                if (!IsValidPOAmendQuantity()) {
                                    $("[id$=SubmitFlag]").val('0');
                                    return false;
                                }
                                $("[id$=POH_AMEND_DATE]").removeAttr("disabled");
                                $("[id$=POH_AMEND_DATE]").val($("[id$=hdfCurrentDate]").val()); //For Saving Current Date as Amendment date
                            }
                            else {
                                $("[id$=POH_AMEND_DATE]").val("");
                            }
                            //End PO Amend Validation
                            $("select[id$=POH_ITEM_TYPE]").attr("disabled", false);
                            var ObjFile = $("#divFileData").data("FileData");
                            $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
                            $("[id$=WKF_FLAG]").val("0");
                            $("[id$=WKF_TRX_FLAG]").val("0");
                            if ($("[id$=POH_IS_AMEND]").val() == "1") {
                                $("[id$=ReferenceID]").val($("[id$=RefID]").val());
                            }
                            if (command != "Draft") {
                                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
                                if ($("[id$=ReferenceID]").val() == "0") {
                                    $("[id$=WKF_FLAG]").val("1");
                                }
                                $("[id$=WKF_TRX_FLAG]").val("1");
                            }
                            else {
                                $("[id$=ActionID]").val('0');
                                $("[id$=WKF_TRX_FLAG]").val("0");
                                $("[id$=WKF_FLAG]").val("0");
                            }


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


                            if (POObject.PODetails.length > 0) {
                                $("[id$=PurchaseOrderList]").val("$" + JSON.stringify(POObject));
                                EnableFields();
                                $("[id$=POH_VENDOR_TERMS_TEXT]").val($("[id$=txtVendorTermText]").val());
                                $("[id$=POH_TERMS_TEXT]").val($("[id$=txtTermText]").val());
                                $("[id$=POH_INVESTOR]").val($("[id$=POH_INVESTOR_CODE]").val());

                                var jSonString = GrandScriptUtils.FormToJsonString(false);
                                $.post(PurchaseOrderConfig.SavePurchaseOrder, jSonString, function (data) {
                                    if (parseInt(data[0]) > 0) {
                                        if (command == "Draft") {
                                            var msg = PurchaseOrderConfig.PurchaseOrderSavedMessage;
                                            if ($("[id$=AST_DOC_MODE]").val() == "1")
                                                msg = PurchaseOrderConfig.SaveMessage1 + " " + data[1] + " " + PurchaseOrderConfig.SaveMessage2;
                                            GrandScriptUtils.ShowModal(msg, PurchaseOrderConfig.Information, PurchaseOrderConfig.SaveCommand);
                                        }
                                        else {
                                            $("[id$=hdfAppID]").val(parseInt(data[0]));
                                            $("[id$=AppNo]").val(data[1]);
                                            ShowWorkflowSaveMsg();
                                        }
                                    }
                                    else if (parseInt(data[0]) == -1) {
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.ActionFailedMessage);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -2) {
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.SaveMessage1 + " " + $("[id$=POH_NO]").html() + " " + PurchaseOrderConfig.EditUsedByAnotherUser, PurchaseOrderConfig.Information, PurchaseOrderConfig.SaveCommand);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -3) {
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.ItemAlreadyAddedMsg, PurchaseOrderConfig.Information, PurchaseOrderConfig.SaveCommand);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -14) { //Duplicate  Type Exist in Tax/Discount/OC Details
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.TaxDiscOCTypeDuplicate, PurchaseOrderConfig.Information);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -39) {
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NotAllowStockAndNonStockItems, PurchaseOrderConfig.Information);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -40) {//
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NotAllowTotalLessInvoiced, PurchaseOrderConfig.Information);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -41) {//  

                                        var POH_REQ_BUDG_VALID = $("[id$=POH_REQ_BUDG_VALID").val();
                                        var budgetValidationRequired = $("[id$=hdnBudgetValidationReq").val();

                                        if (budgetValidationRequired === "1") {

                                            if (POH_REQ_BUDG_VALID === "1") {
                                                ConfirmBudgetValidation(command);
                                            }
                                        }
                                        else {
                                            GrandScriptUtils.ShowModal(PurchaseOrderConfig.PurchaseBudgetErr, PurchaseOrderConfig.Information);
                                            $("[id$=SubmitFlag]").val('0')
                                        }
                                        //GrandScriptUtils.ShowModal(PurchaseOrderConfig.PurchaseBudgetErr, PurchaseOrderConfig.Information);
                                        //$("[id$=SubmitFlag]").val('0')
                                    }
                                    else if (parseInt(data[0]) == -42) {//
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.GRNBatchUsed, PurchaseOrderConfig.Information);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                    else {
                                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.ActionFailedMessage, PurchaseOrderConfig.Information);
                                        $("[id$=SubmitFlag]").val('0')
                                    }
                                });
                            }
                            else {
                                GrandScriptUtils.ShowModal(PurchaseOrderConfig.AddAtleastOneItem, PurchaseOrderConfig.Information);
                                $("[id$=SubmitFlag]").val('0')
                            }
                        }
                        //Save Action End
                    }
                });
            }
            else {
                //GrandScriptUtils.ShowModal(PurchaseOrderConfig.MaterialRateGrZero);
                if ($("[id$=hdfZeroRateConfirm").val() == "0")
                    ConfirmZeroRateValidation(command);
            }
        }
    });
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var msg = "";
    var IsInbox = "False";
    if ($("[id$=isAlert]").val() == "1") {
        if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
            $("[id$=hdfIsGoInbox]").val("1");
            msg = PurchaseOrderConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseOrderConfig.SubmitMessage;
            $("[id$=saveMsg]").val(msg);
            $("#updateProgress").show();
            $("[id$=btnAlertSave]").click();
        }
        else {
            msg = PurchaseOrderConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseOrderConfig.SubmitMessage;
            $("[id$=saveMsg]").val(msg);
            $("#updateProgress").show();
            $("[id$=btnAlertSave]").click();
        }
    }
    else {
        if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
            msg = PurchaseOrderConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseOrderConfig.SubmitMessage;
            GrandScriptUtils.ShowModal(msg, PurchaseOrderConfig.Information, PurchaseOrderConfig.INBOX);
        }
        else {
            msg = PurchaseOrderConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseOrderConfig.SubmitMessage;
            GrandScriptUtils.ShowModal(msg, PurchaseOrderConfig.Information, PurchaseOrderConfig.SaveCommand);
        }
    }

}

function AssignTerms() {
    //    //<summary>function used to assign the po genaral and vendor terms </summary>

    //    var terms = "";
    //    var vendorTerms = "";
    //    for (var i in Terms) {
    //        terms += (terms != "") ? ("," + Terms[i]) : Terms[i];
    //    }
    //    for (var i in VendorTerms) {
    //        vendorTerms += (vendorTerms != "") ? ("," + VendorTerms[i]) : VendorTerms[i];
    //    }
    //    $("[id$=POH_TERMS]").val(terms);
    //    $("[id$=POH_VENDOR_TERMS]").val(vendorTerms);
}
//Validate month Range
function LeastDate(date1, date2) {
    var flag = false;
    var date1Split = date1.split('-');
    var date2Split = date2.split('-');
    if (parseInt(date1Split[2]) < parseInt(date2Split[2]))
        flag = true;
    else if (LeastMonth(date1Split[1], date2Split[1]) == true)
        flag = true;
    else if (parseInt(date1Split[0]) < parseInt(date2Split[0]))
        flag = true;
    if (flag) return date1;
    else return date2;
}
//Check From Month Is Greater than To Month
function LeastMonth(fromMonth, toMonth) {
    var Months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var index1, index2;
    index1 = 0;
    index2 = 0;
    for (var i = 0; i < 12; i++) {
        if (Months[i] == fromMonth)
            index1 = i;
        if (Months[i] == toMonth)
            index2 = i;

    }
    return index1 < index2 ? true : false;
}

function AssingnRemarks() {
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        PurOrderDetails[i].POD_REMARKS = $("#POD_REMARKS_" + PurOrderDetails[i].POD_SL_NO).val();
    }
    $("#divData").data("ReqPODetails", PurOrderDetails);

}
function AssingPODetails() {
    //<summary>function used to assign po material details </summary>

    var PurOrderDetails = $("#divData").data("ReqPODetails");
    if (PurOrderDetails.length > 0)
        $("[id$=leastDate]").val(PurOrderDetails[0].POD_REQD_DATE);
    for (var i in PurOrderDetails) {
        $("[id$=leastDate]").val(LeastDate($("[id$=leastDate]").val(), PurOrderDetails[i].POD_REQD_DATE));
        PurOrderDetails[i].POD_AMT_VALUE = GetItemSubTotalAmount(PurOrderDetails[i].POD_SL_NO);
        PurOrderDetails[i].POD_REMARKS = $("#POD_REMARKS_" + PurOrderDetails[i].POD_SL_NO).val();
        PurOrderDetails[i].POD_REQD_DATE = $("#PODtlReqDate_" + PurOrderDetails[i].POD_SL_NO).val();

        //Code to edit PO tax,rate,discount
        if ($("[id$=isEditMode]").val() == "1") {
            //PurOrderDetails[i].POD_TAX = $("#POD_H_TAX_" + PurOrderDetails[i].POD_SL_NO).val();
            PurOrderDetails[i].POD_RATE = parseFloat($("#POD_RATE_" + PurOrderDetails[i].POD_SL_NO).val()).toFixed(RateDec);
            if (!isNaN(PurOrderDetails[i].POD_RATE_PREV))
                PurOrderDetails[i].POD_RATE_PREV = parseFloat(PurOrderDetails[i].POD_RATE_PREV).toFixed(RateDec);
            var discount = $("#POD_DISC_AMT_" + PurOrderDetails[i].POD_SL_NO).val();
            if (discount != undefined) {
                PurOrderDetails[i].POD_DISC_AMT = $("#POD_DISC_AMT_" + PurOrderDetails[i].POD_SL_NO).val();
            }
        }

        PurOrderDetails[i].POD_DEPT = $("[id$=POH_DEPT]").val();
        PurOrderDetails[i].PRDetails = GetItemPRDetails(PurOrderDetails[i].POD_ITEM, PurOrderDetails[i].POD_SL_NO);
        PurOrderDetails[i].TaxDetails = GetItemTaxDetails(PurOrderDetails[i].POD_SL_NO, PurOrderDetails[i].POD_PK);
    }
    POObject.PODetails = PurOrderDetails;
    POObject.TaxHeader = GetTaxHdrDetails();
}

function DeleteDetails() {
    //<summary>Function Used to delete po material</summary>

    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        if (PurOrderDetails[i].POD_ITEM == PurchaseOrderConfig.ItemPK) {
            slNo = PurOrderDetails[i].POD_SL_NO;
            var TaxDetails = $("#divData").data("TaxDetails");
            for (var j = 0; j < TaxDetails.length; j++) {
                if (TaxDetails[j].POT_SL_NO == slNo) {
                    TaxDetails.splice(j, 1);
                    --j;
                }
            }
            PurOrderDetails.splice(i, 1);
            $("#divData").data("TaxDetails", TaxDetails);
            break;
        }
    }
    if (!$.isArray(PurOrderDetails)) {
        var PurchaseOrderObj = PurOrderDetails;
        PurOrderDetails = new Array();
        PurOrderDetails.push(PurchaseOrderObj);
    }
    var PurOrderList = $("#divData").data("ReqPOList");
    for (var i = 0; i < PurOrderList.length; i++) {
        if (PurOrderList[i].POR_ITEM == PurchaseOrderConfig.ItemPK) {
            PurOrderList.splice(i, 1);
            i = i - 1;
        }
    }
    if (!$.isArray(PurOrderList)) {
        var PurchaseDtlsObj = PurOrderList;
        PurOrderDetails = new Array();
        PurOrderList.push(PurchaseDtlsObj);
    }
    var TaxDetails = $("#divData").data("TaxDetails");
    for (var i = 0; i < TaxDetails.length; i++) {
        if (TaxDetails[i].POT_SL_NO == PurchaseOrderConfig.SlNo && TaxDetails[i].IsHeader == false) {
            TaxDetails.splice(i, 1);
            i = i - 1;
        }
    }
    if (PurOrderDetails.length == 0) {
        for (var i = 0; i < TaxDetails.length; i++) {
            //            if (TaxDetails[i].IsHeader == true) {   //These 3 Lines are Commented For Resolving BugId 242
            //                TaxDetails.splice(i, 1);
            //                i = i - 1;
            //            }
        }
        ClearTotalDetails();
    }
    PurchaseOrderConfig.SlNo = 0;
    PurchaseOrderConfig.ItemPK = 0;
    $("#divData").data("TaxDetails", TaxDetails);
    $("#divData").data("ReqPODetails", PurOrderDetails);
    GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
    if (PurOrderDetails == null || PurOrderDetails.length == 0) {
        $("#grdPODetails tr:has(td)").remove();
    }
    $("#divData").data("ReqPOList", PurOrderList);
    GrandGrid.MakeGrid($("#grdPOList"), 0, PurOrderList);
    if ($("[id$=POH_IS_AMEND]").val() == "1") {
        $("[id$=hdfIsNew]").val('1');
    }
    SaveItemTaxDiscountApply();
}

function DeleteItemDetails() {
    var PurOrderList = $("#divData").data("ReqPOList");
    for (var i in PurOrderList) {
        if (PurOrderList[i].POR_ITEM == PurchaseOrderConfig.DeleteItemPK) {
            PurOrderList.splice(i, 1);
        }
    }
    $("#divData").data("ReqPOList", PurOrderList);
    PurchaseOrderRequestSummary();
    if (PurOrderList.length == 0) {
        $("#divData").data("ReqPOSummaryList", new Array());
        $("#divData").data("ReqPOList", new Array());
        GrandGrid.MakeGrid($("#grdPOList"), 0, new Array());
        $("#divData").data("ReqPODetails", new Array());
        GrandGrid.MakeGrid($("#grdPODetails"), 0, new Array());
        $("#divData").data("TaxDetails", new Array());
        GrandGrid.MakeGrid($("#grdTaxDetails"), 0, new Array());
        $("#divPoListing").hide();
    }
}

function ClearTotalDetails() {
    //<summary>Function Used to clear all calculate details</summary>

    $("[id$=POH_SUB_TOTAL]").val("");
    $("[id$=POH_DISC_AMT]").val("");
    $("[id$=POH_ADD_TAX_AMT]").val("");
    $("[id$=POH_SHIP_CHARGE]").val("");
    $("[id$=POH_PRICE_ADJUST]").val("");
    $("[id$=POH_TOTAL_VALUE]").val("");
}

function DeleteTaxDetails() {
    //<summary>function used to delete the tax details</summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    if (parseInt($("[id$=IsLine]").val()) == 1) {
        for (var i in TaxDetails) {
            if (PurchaseOrderConfig.EditTax > 0) {
                if (TaxDetails[i].POT_SL_NO == PurchaseOrderConfig.ItemTaxPK && TaxDetails[i].POT_TAX == PurchaseOrderConfig.EditTax
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
            else {
                if (TaxDetails[i].POT_SL_NO == PurchaseOrderConfig.ItemTaxPK && TaxDetails[i].POT_NAME == PurchaseOrderConfig.EditTaxName
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
        }
        $("#divData").data("TaxDetails", TaxDetails);
        GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(PurchaseOrderConfig.ItemTaxPK, $("[id$=IsLineDiscount]").val(), true));
    }
    else if (parseInt($("[id$=IsLine]").val()) == 2) {
        for (var i in TaxDetails) {
            if (PurchaseOrderConfig.EditTax > 0) {
                if (TaxDetails[i].POT_TAX == PurchaseOrderConfig.EditTax && TaxDetails[i].IsHeader == true
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
            else {
                if (TaxDetails[i].IsHeader == true && TaxDetails[i].POT_NAME == PurchaseOrderConfig.EditTaxName
                    && TaxDetails[i].POT_TYPE == PurchaseOrderConfig.EditTaxType) {
                    TaxDetails.splice(i, 1);
                }
            }
        }
        $("#divData").data("TaxDetails", TaxDetails);
        GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(PurchaseOrderConfig.ItemTaxPK, $("[id$=IsLineDiscount]").val(), false));
    }
    PurchaseOrderConfig.EditTax = 0;
    PurchaseOrderConfig.EditTaxName = "";
    PurchaseOrderConfig.EditTaxType = 0;
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    window.location = $("[id$=hdfBackUrl]").val(); //"PurchaseOrderListing.aspx";
    return false;
}

function PrintPage() {
    ///<summary>function Used to print thr po details///</summary>

    //     window.location = "PurchaseOrderReport.aspx?POID=" + $("input[id$=POH_PK]").val();
    //window.location = PurchaseOrderConfig.POREPORTURL + "?ID=" + $("input[id$=POH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    var url = PurchaseOrderConfig.POREPORTURL + "?ID=" + $("input[id$=POH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();

    OpenPDF(url);
    return false;
}

//#endregion

//#region----------- Utility----------------

function GridHandler(tr, command) {
    ////<summary>function used handle Grid Events </summary>

    switch (command.toString().toUpperCase()) {

        case PurchaseOrderConfig.Edit:
            FillPurchaseOrder(tr);
            break;

        case PurchaseOrderConfig.DeleteItem:
            PurchaseOrderConfig.DeleteItemPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POR_ITEM, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.DeleteConfirmationMessage, PurchaseOrderConfig.ConfirmationMessage, PurchaseOrderConfig.DeleteItem, true);
            break;

        case PurchaseOrderConfig.Delete:
            PurchaseOrderConfig.ItemPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_ITEM, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.SlNo = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_SL_NO, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.DeleteConfirmationMessage, PurchaseOrderConfig.ConfirmationMessage, PurchaseOrderConfig.Delete, true);
            break;

        case PurchaseOrderConfig.TaxDelete:
            PurchaseOrderConfig.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_SL_NO, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.EditTax = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_TAX, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.EditTaxName = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_NAME, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.EditTaxType = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_TYPE, $(tr).parents("table:first").attr("id"));
            if ($("[id$=isEditMode]").val() == "1") {
                var slNo = $("[id$=hdnSlNo]").val();
                var subTotal = 0;
                var discount = 0;
                var rate = 0;
                var tax = 0;

                var qty = parseFloat($("#POD_H_QTY_REQUESTED_" + slNo).val());

                rate = parseFloat($("[id$=TaxRate]").val());
                if (isNaN(rate)) {
                    rate = parseFloat($("#POD_RATE_" + slNo).val());
                }

                $("#POD_RATE_" + slNo).val(parseFloat(rate).toFixed(RateDec));


                var amount = parseFloat(parseFloat(rate) * parseFloat(qty));
                amount = isNaN(amount) ? 0 : amount;
                $("#POD_H_AMOUNT_" + slNo).val(amount.toFixed(AmtDec));

                var amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());


                var PurOrderDetails = $("#divData").data("ReqPODetails");
                for (var i in PurOrderDetails) {
                    if (PurOrderDetails[i].POD_SL_NO == slNo) {
                        tax = parseFloat(PurOrderDetails[i].POD_TAX);
                    }
                }

                discount = parseFloat($("[id$=TaxDiscAmount]").val());
                if (isNaN(discount)) {
                    discount = parseFloat($("#POD_DISC_AMT_" + slNo).val());
                }

                subTotal = (((isNaN(amount) ? 0 : amount)
                    + (isNaN(tax) ? 0 : tax))
                    - (isNaN(discount) ? 0 : discount));
                var subt = subTotal - tax;
                if (isNaN(parseFloat(subTotal)) || subt < 0) {
                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount);
                }
                else {
                    GrandScriptUtils.ShowModal(PurchaseOrderConfig.DeleteConfirmationMessage, PurchaseOrderConfig.ConfirmationMessage, PurchaseOrderConfig.TaxDelete, true);
                }
            }
            else {
                GrandScriptUtils.ShowModal(PurchaseOrderConfig.DeleteConfirmationMessage, PurchaseOrderConfig.ConfirmationMessage, PurchaseOrderConfig.TaxDelete, true);
            }
            break;

        case PurchaseOrderConfig.TaxEdit:
            PurchaseOrderConfig.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_SL_NO, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.EditTax = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_TAX, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.EditTaxName = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_NAME, $(tr).parents("table:first").attr("id"));
            PurchaseOrderConfig.EditTaxType = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_TYPE, $(tr).parents("table:first").attr("id"));
            $("[id$=ChooseTax]").val(PurchaseOrderConfig.EditTax);
            $("[id$=TaxAmount]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POT_TAX_AMT, $(tr).parents("table:first").attr("id")));
            break;
    }
    return false;
}

function FillPurchaseOrder(tr) {
    //<summary>function used to Purchase Order for edit</summary>

    var grdID = $(tr).parents("table:first").attr("id");
    PurchaseOrderConfig.SlNo = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_SL_NO, $(tr).parents("table:first").attr("id"));
    $("[id$=ITM_CODE]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_ITEM, $(tr).parents("table:first").attr("id")));

    $("[id$=txtItemCode]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.ITM_TEXT, grdID));

    $("[id$=POD_QTY]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_QTY_REQUESTED, $(tr).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, ""));
    $("[id$=UOM_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_UOM, $(tr).parents("table:first").attr("id")));
    $("[id$=POD_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.UOM_CODE, $(tr).parents("table:first").attr("id")));
    //  $("#POD_RATE_" + PurchaseOrderConfig[i].POD_SL_NO).val();
    if ($("[id$=isEditMode]").val() == "1") {
        $("[id$=POD_RATE]").val(parseFloat($("#POD_RATE_" + PurchaseOrderConfig.SlNo).val()).toFixed(RateDec));
        $("[id$=POD_AMOUNT]").val($("#POD_H_AMOUNT_" + PurchaseOrderConfig.SlNo).val());
        $("[id$=POD_DISCOUNT]").val($("#POD_DISC_AMT_" + PurchaseOrderConfig.SlNo).val());
        $("[id$=POD_SUBTOTAL]").val($("#POD_SUBTOTAL_" + PurchaseOrderConfig.SlNo).val());
    }
    else {
        $("[id$=POD_RATE]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_RATE, $(tr).parents("table:first").attr("id"))).toFixed(RateDec));
        $("[id$=POD_AMOUNT]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_AMOUNT, $(tr).parents("table:first").attr("id")));
        $("[id$=POD_DISCOUNT]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_DISC_AMT, $(tr).parents("table:first").attr("id")));
        $("[id$=POD_SUBTOTAL]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_AMT_VALUE, $(tr).parents("table:first").attr("id")));
    }

    $("[id$=POD_TAX]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_TAX, $(tr).parents("table:first").attr("id")));
    $("[id$=POD_REMARKS]").val($("#POD_REMARKS_" + PurchaseOrderConfig.SlNo).val());
    $("[id$=POD_REQD_DATE]").val($("#PODtlReqDate_" + PurchaseOrderConfig.SlNo).val());
    $("[id$=ITM_CODE]").attr("disabled", "disabled");
}

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    if ($("[id$=hdfIsReqDeptPostback]").val() != "1")
        ClearSearchDetails();
    var strname = $("select[id$=SearchType]").val();
    if (strname == "0") {
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        //$("[id$=imbSearch]").hide();             
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
        //$("[id$=imbSearch]").show();
    }
    if ($("[id$=hdfIsPRFromInbox]").val() == "1") {
        BindPendingPRGrid();
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

function SearchInit() {
    ///<summary>To handle auto complete</summary>

    $("[id$=SearchValue]").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: PurchaseOrderConfig.GetSearchValue + $("[id$=Store]").val() + "&ProcessID=" + $("[id$=hdfProcessID]").val(),
                data: {
                    SearchValue: request.term,
                    SearchType: $("[id$=SearchType]").val()
                    //Vendor: $("#divVendors").find("[name=rdoVendors]:checked").val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Text, // format the the data as text 
                            id: item.Value
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

}
function ActiveSearch() {
    ///<summary>To Fill File Details And dispaly as Listing With Delete Option</summary>

    $("#tab1Content").show();
    $("#tab2Content").hide();
    $("[id$=btnSave]").hide();
    $("[id$=btnSubmit]").hide();
    $("#aSearch").attr("class", "tab-active");
    $("#aCreatePO").attr("class", "tab-inactive");
    var PurOrderList = $("#divData").data("ReqPOList");
    if (PurOrderList != null && PurOrderList.length > 0) {
        $("#divContinue").show();
        if ($("[id$='hdfShowPoOtherVendor']").val() == "1") {
            $("#divOtherVendor").show();
            BindVendor();
            HideOtherVendor();
        }
    }
}

function ActiveCreatePO() {
    ///<summary>To Fill File Details And dispaly as Listing With Delete Option</summary>

    $("#tab2Content").show();
    $("#tab1Content").hide();
    $("#aCreatePO").attr("class", "tab-active");
    $("#aSearch").attr("class", "tab-inactive");
}


function IsValidData() {
    //<summary>Function Used to check the all data required date entered correctly </summary>

    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        PurOrderDetails[i].POD_REQD_DATE = $("#PODtlReqDate_" + PurOrderDetails[i].POD_SL_NO).val();
        if (PurOrderDetails[i].POD_REQD_DATE == "") {
            return false;
        }
        //        else if (!CheckValidDate(PurOrderDetails[i].POD_REQD_DATE)) {
        //            return false;
        //        }
    }
    return true;
}

function CheckValidDate(value) {
    //<summary>Function Used to check the all entered date is correct </summary>

    var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var date = value.split("-");
    var dd = date[0];
    var mm = "";
    for (var i in mmm) {
        if (mmm[i] == date[1]) {
            if ($.browser.msie) {
                mm = i + 1;
            }
            else {
                mm = mmm[i];
            }
        }
    }
    value = date[0] + "/" + mm + "/" + date[2];
    return !/Invalid|NaN/.test(new Date(value));
}

function ClearSrchDtls() {
    ///<summary>function used to clear search details</summary>

    $("[id$=SearchValue]").val("");
}

function FillFileDetails() {
    ///<summary>function used to fill the file details</summary>

    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + "." + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}

function DateInit() {
    //<summary>function used to make datepicker</summary>

    GrandScriptUtils.DatePicker("POH_DATE", false, false);
    GrandScriptUtils.DatePicker("POD_REQD_DATE", false, false);
    GrandScriptUtils.DatePicker("POH_AMEND_DATE", false, false);
}

function Popup() {
    ///<summary>Function used for popup</summary>

    $("#divItemTax").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function () {
            AssingnRemarks();
            //SaveItemTaxDiscountApply();          
        }
    });
    $("#divPurchaseRequest").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

    $("#divVendor").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#divRevisionHistory").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

    $("#divRelatedWidget").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

    $("#divItemRate").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function () {
            if (isNaN(parseFloat($("[id$=TaxRate]").val())) || parseInt(parseFloat($("[id$=TaxRate]").val())) <= 0) {
                AssingnRemarks();
                RestRate();
            }
        }
    });

    $("#divItemDiscount").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

}

function RestRate() {
    if (isNaN(parseFloat($("[id$=TaxRate]").val())) || parseInt(parseFloat($("[id$=TaxRate]").val())) <= 0) {
        var slNo = $("[id$=hdnSlNo]").val();
        var PurOrderDetails = $("#divData").data("ReqPODetails");
        for (var i in PurOrderDetails) {
            if (PurOrderDetails[i].POD_SL_NO == slNo) {
                //$("[id$=TaxRate]").val(PurOrderDetails[i].POD_RATE);
                $("[id$=TaxRate]").val(parseFloat(PurOrderDetails[i].POD_RATE).toFixed(RateDec));

                break;
            }
        }
    }
}

function ClearTerms(type) {
    ///<summary>function used Clear Terms Details </summary>
    if (type == "Vendor") {
        VendorTerms = new Array();
        $("[id$=VENDOR_TERMS]").val("0");
        $("[id$=POH_VENDOR_TERMS]").val("");
        //        $("[id$=LblPOH_VENDOR_TERMS]").html("");
        $("[id$=txtVendorTermText]").val("");
    }
    else if (type == "General") {
        Terms = new Array();
        $("[id$=GENERAL_TERMS]").val("0");
        $("[id$=POH_TERMS]").val("");
        $("[id$=txtTermText]").val("");
    }
    return false;
}

/// Used to disable Autocomplete
function DisableAuto(extender, hfield) {
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
    $(extender).autocomplete("option", "disabled", true);
    $(extender).attr("disabled", true);
}

/// Used to disable Autocomplete
function EnableAuto(extender, hfield) {
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
    $(extender).autocomplete("option", "disabled", false);
    $(extender).attr("disabled", false);
}

function DisableControlls() {
    ///<summary>function used to enable po controls</summary>
    //For Calculation need to check is the po is new or draft. Value 1 for new and draft
    $("[id$=hdfIsNew]").val('0');
    $("[id$=imbAddItem]").hide();

    $("[id$=btnClearAllSelectedItem]").hide();
    $("[id$=imbTaxDiscountSave]").hide();
    $("[id$=btnApply]").hide();
    $("[id$=btnAddSelectedItems]").hide();
    $("[id$=btnClearAllSelectedItem]").hide();
    $("[id$=POH_DATE]").attr("disabled", "disabled");
    $("[id$=POH_PRICE_ADJUST]").attr("disabled", "disabled");
    $("select:not([id$=ddlLocation],[id$=ddlCostCenter])").attr("disabled", "disabled");
    $("[id$=WRKFACT_ID]").removeAttr("disabled");
    //to disable 
    $("[id$=POH_DISC_AMT]").attr("disabled", "disabled");
    $("[id$=POH_SHIP_CHARGE]").attr("disabled", "disabled");
    $("[id$=POH_ADD_TAX_AMT]").attr("disabled", "disabled");
    $("[id$=POH_CONTRACT_REF_NO]").attr("disabled", "disabled");
    DisableAuto($("[id$=POH_FROM_PORT_TEXT]"), $("[id$=POH_FROM_PORT]"));
    DisableAuto($("[id$=POH_TO_PORT_TEXT]"), $("[id$=POH_TO_PORT]"));
    DisableAuto($("[id$=txtBillVendor]"), $("[id$=POH_DELIVERY]"));
    $("[id$=btnPopupCommentSave]").hide();
    $("[id$=POH_COMMENTS]").attr("disabled", "disabled");
    $("[id$=imbVendorTerms]").hide();
    $("[id$=imgbtnclear]").hide();
}

function EnableFields() {
    ///<summary>function used to enable po controls</summary>
    $("[id$=hdfIsNew]").val('1');
    $("[id$=POH_SUB_TOTAL]").removeAttr("disabled");
    $("[id$=POH_DISC_AMT]").removeAttr("disabled");
    $("[id$=POH_SHIP_CHARGE]").removeAttr("disabled");
    $("[id$=POH_ADD_TAX_AMT]").removeAttr("disabled");
    $("[id$=POH_TOTAL_VALUE]").removeAttr("disabled");
    $("[id$=POH_DATE]").removeAttr("disabled");
    $("[id$=POH_PRICE_ADJUST]").removeAttr("disabled");
    $("select").removeAttr("disabled");
    $("[id$=POH_CONTRACT_REF_NO]").removeAttr("disabled");
    EnableAuto($("[id$=POH_FROM_PORT_TEXT]"), $("[id$=POH_FROM_PORT]"));
    EnableAuto($("[id$=POH_TO_PORT_TEXT]"), $("[id$=POH_TO_PORT]"));
    EnableAuto($("[id$=txtBillVendor]"), $("[id$=POH_DELIVERY]"));
    $("[id$=btnPopupCommentSave]").show();
    $("[id$=POH_COMMENTS]").removeAttr("disabled");
    $("[id$=imbVendorTerms]").show();
    $("[id$=imgbtnclear]").show();
}



function SetPOPRDetails(PODetails) {
    //<summary>function used to set the po pr details in divdata</summary>

    var PurOrderList = new Array();
    var PRObj = new Object();
    for (var i in PODetails) {
        if (!$.isArray(PODetails[i].PRDetails)) {
            PRObj = PODetails[i].PRDetails;
            PODetails[i].PRDetails = new Array();
            PODetails[i].PRDetails.push(PRObj);
        }
        for (var k in PODetails[i].PRDetails) {
            if (PODetails[i].PRDetails[k] != undefined) {
                PurOrderList.push(PODetails[i].PRDetails[k]);
            }
        }
    }
    $("#divData").data("ReqPOList", PurOrderList);
}

function SetPOTaxDetails(purchaseOrderObj, PODetails) {
    //<summary>function used to set the po item tax/ discount and tax/ discount / shippent details in divdata</summary>

    var TaxDetails = new Array();
    var TaxObj = new Object();
    for (var i in PODetails) {
        if (!$.isArray(PODetails[i].TaxDetails)) {
            TaxObj = PODetails[i].TaxDetails;
            PODetails[i].TaxDetails = new Array();
            PODetails[i].TaxDetails.push(TaxObj);
        }
        for (var k in PODetails[i].TaxDetails) {
            if (PODetails[i].TaxDetails[k] != undefined) {
                PODetails[i].TaxDetails[k].IsHeader = false;
                TaxDetails.push(PODetails[i].TaxDetails[k]);
            }
        }
    }
    if ($.isArray(purchaseOrderObj.PurchaseOrderList.TaxHeader)) {
        for (var i in purchaseOrderObj.PurchaseOrderList.TaxHeader) {
            if (purchaseOrderObj.PurchaseOrderList.TaxHeader[i] != undefined) {
                purchaseOrderObj.PurchaseOrderList.TaxHeader[i].IsHeader = true;
                if (purchaseOrderObj.PurchaseOrderList.TaxHeader[i].POT_TAX == undefined)
                    purchaseOrderObj.PurchaseOrderList.TaxHeader[i].POT_TAX = 0;
                TaxDetails.push(purchaseOrderObj.PurchaseOrderList.TaxHeader[i]);
            }
        }
    }
    else {
        if (purchaseOrderObj.PurchaseOrderList.TaxHeader != undefined) {
            purchaseOrderObj.PurchaseOrderList.TaxHeader.IsHeader = true;
            if (purchaseOrderObj.PurchaseOrderList.TaxHeader.POT_TAX == undefined)
                purchaseOrderObj.PurchaseOrderList.TaxHeader.POT_TAX = 0;
            TaxDetails.push(purchaseOrderObj.PurchaseOrderList.TaxHeader);
        }
    }
    $("#divData").data("TaxDetails", TaxDetails);
    $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object

}

function SetPOItemTaxDiscount(isTax, currSlNo, amount) {
    //<summary>function used to set the item tax / discount amount</summary>

    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        if (PurOrderDetails[i].POD_SL_NO == currSlNo) {
            if (isTax) {
                PurOrderDetails[i].POD_TAX = parseFloat(amount).toFixed(AmtDec);
            }
            else {
                PurOrderDetails[i].POD_DISC_AMT = parseFloat(amount).toFixed(AmtDec);
            }
        }
    }
}

function CalculateAmount() {
    //<summary>function used to calculate the sub total amount</summary>

    var qty = $("[id$=POD_QTY]").val();
    var rate = $("[id$=POD_RATE]").val();
    var amount = parseFloat(parseFloat(rate) * parseFloat(qty));
    amount = isNaN(amount) ? 0 : amount;
    $("[id$=POD_AMOUNT]").val(amount.toFixed(AmtDec));

    //Fill line item tax popup
    if (($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.ItemWise) || ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
        var materialID = $("select[id$=ITM_CODE]").val();
        FillLineItemTax(materialID, amount);
    }
    //End
    var tax = parseFloat($("[id$=POD_TAX]").val());
    var discount = parseFloat($("[id$=POD_DISCOUNT]").val());
    var subTotal = ((amount + tax) - discount);
    subTotal = isNaN(subTotal) ? 0 : subTotal;
    $("[id$=POD_SUBTOTAL]").val(subTotal.toFixed(AmtDec));
}

function CalculateSubTotalValue() {

    var slNo = $("[id$=hdnSlNo]").val();
    var subTotal = 0;
    var discount = 0;
    var rate = 0;
    var tax = 0;
    var amount = 0;



    var qty = parseFloat($("#POD_H_QTY_REQUESTED_" + slNo).val());
    rate = parseFloat($("[id$=TaxRate]").val());
    if (isNaN(rate)) {
        rate = parseFloat($("#POD_RATE_" + slNo).val());
        amount = parseFloat(parseFloat(rate) * parseFloat(qty));
        amount = isNaN(amount) ? 0 : amount;

    }
    else {

        var type = 1;
        var formula = "";
        amount = parseFloat(parseFloat(rate) * parseFloat(qty));
        amount = isNaN(amount) ? 0 : amount;
        var total = amount;
        var taxAmount = 0;
        var finalTotal = 0;
        //26-12-2013
        //  GetFormulaVal(amount, slNo, type, rate);

    }

    if (tax == 0) {

        var PurOrderDetails = $("#divData").data("ReqPODetails");
        for (var i in PurOrderDetails) {
            if (PurOrderDetails[i].POD_SL_NO == slNo) {
                tax = parseFloat(PurOrderDetails[i].POD_TAX);
            }
        }

        discount = parseFloat($("[id$=TaxDiscAmount]").val());
        if (isNaN(discount)) {
            discount = parseFloat($("#POD_DISC_AMT_" + slNo).val());
        }
        //Line item Tax Based On Configuration 
        if (($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.ItemWise) || ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
            subTotal = (((isNaN(amount) ? 0 : amount)
                + (isNaN(tax) ? 0 : tax))
                - (isNaN(discount) ? 0 : discount));
        }
        else {
            subTotal = isNaN(amount) ? 0 : amount;
        }

        var subt = subTotal;
        if (isNaN(parseFloat(subTotal)) || subt < 0) {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount);
        }
        else {

            $("#POD_RATE_" + slNo).val(parseFloat(rate).toFixed(RateDec));
            $("#POD_H_AMOUNT_" + slNo).val(amount.toFixed(AmtDec));
            amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
            $("#POD_DISC_AMT_" + slNo).val(discount.toFixed(AmtDec));
            $("#POD_SUBTOTAL_" + slNo).val(subTotal.toFixed(AmtDec));

            ReCalculateSubTotalVal();
            CalculateTotal();
        }
    }
}

function GetTaxFormula(pk, total, slNo, TaxDetails, taxText, taxName) {
    var taxAmount = 0;
    var formula = "";
    //   var amount = 0;

    $.get(PurchaseOrderConfig.GetTaxFormula + pk + "&Active=2", function (data) {
        if (data != null && data.length > 0) {
            if (pk > 0) {
                formula = data[0].Formula;

                PurchaseOrderConfig.TaxFormula = formula;
                formula = formula.replace(/#SUBTOTAL#/g, total);
                try {
                    taxAmount = eval(formula);
                } catch (e) {
                    taxAmount = 0;
                }

                var TaxDetailsObj;
                TaxDetailsObj = JSLINQ(TaxDetails)
                    .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == pk && tax.POT_NAME == taxName; })
                    .FirstOrDefault(null);
                if (TaxDetailsObj == null) {
                    TaxDetailsObj = new Object();
                    TaxDetailsObj.IsHeader = false;
                    TaxDetailsObj.POT_SL_NO = slNo;
                    TaxDetailsObj.POT_PK = 0;
                    TaxDetailsObj.POT_PO_DTL = 0;
                    TaxDetailsObj.POT_TAX = pk > 0 ? pk : 0;
                    TaxDetailsObj.POT_TAX_TEXT = taxText;
                    TaxDetailsObj.POT_TAX_AMT = parseFloat(taxAmount).toFixed(AmtDec);
                    TaxDetailsObj.POT_TAX_CATEGORY = $.trim($("[id$=IsLineDiscount]").val());
                    TaxDetailsObj.POT_TAX_FORMULA = PurchaseOrderConfig.TaxFormula;
                    TaxDetailsObj.POT_TYPE = parseInt(pk) > 0 ? "1" : "2";
                    TaxDetailsObj.POT_NAME = taxName;
                    TaxDetails.push(TaxDetailsObj);
                }
                else {
                    TaxDetailsObj.POT_TAX_AMT = parseFloat(taxAmount).toFixed(AmtDec);
                }

                taxAmount = parseFloat(GetItemTaxAmount(slNo, 1));
                //$("[id$=TaxAmount]").val(taxAmount.toFixed(3));
                //tax = parseFloat($("[id$=TaxAmount]").val());

                if (taxAmount != 0) {
                    discount = parseFloat($("[id$=TaxDiscAmount]").val());
                    if (isNaN(discount)) {
                        discount = parseFloat($("#POD_DISC_AMT_" + slNo).val());
                    }
                    var taxval = 0;
                    taxval = taxAmount;

                    subTotal = ((parseFloat(isNaN(amount) ? 0 : amount)
                        + parseFloat(isNaN(taxval) ? 0 : taxval))
                        - parseFloat(isNaN(discount) ? 0 : discount));

                    var subt = subTotal;
                    if (isNaN(parseFloat(subTotal)) || subt < 0) {
                        GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount);
                    }
                    else {
                        $("#POD_DISC_AMT_" + slNo).val(discount);
                        $("#POD_SUBTOTAL_" + slNo).val(subTotal.toFixed(AmtDec));
                        $("#POD_H_AMOUNT_" + slNo).val(amount.toFixed(AmtDec));
                        amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
                        $("#POD_RATE_" + slNo).val(parseFloat(rate).toFixed(RateDec));

                        ReCalculateSubTotalVal();
                        CalculateTotal();

                        $("#grdPODetails tr:has(td)").each(function (index) {

                            columnIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_TAX, "grdPODetails");
                            SetPOItemTaxDiscount(true, slNo, taxAmount);
                            $(this).find("td:eq(" + columnIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + amount + "');\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />" + taxAmount + "");

                        });
                    }
                }
            }
        }
    });
    return taxAmount;
}
function GetFormulaVal(amount, slNo, type, rate) {
    //<summary>function used to get the tax formula and calculate the tax amount</summary>

    var formula = "";
    var total = parseFloat(amount);
    var taxAmount = 0;
    var finalTotal = 0;
    //var tax = 0;
    var subTotal = 0;
    var discount = 0;
    var columnIndex = 0;
    var isLine = true;

    var TaxDetails = $("#divData").data("TaxDetails");
    for (var i in TaxDetails) {
        if (isLine) {
            if (TaxDetails[i].POT_SL_NO == slNo && TaxDetails[i].POT_TAX_CATEGORY == type) {
                pk = TaxDetails[i].POT_TAX;
                taxText = TaxDetails[i].POT_TAX_TEXT;

                //                        $.get(PurchaseOrderConfig.GetTaxFormula + TaxDetails[i].POT_TAX + "&Active=2", function (data) {
                //                            if (data != null && data.length > 0) {
                //                                if (pk > 0) {
                //                                    formula = data[0].Formula;
                //                                    PurchaseOrderConfig.TaxFormula = formula;
                //                                    formula = formula.replace(/#SUBTOTAL#/g, total);
                //                                    try {
                //                                        taxAmount = eval(formula);
                //                                    } catch (e) {
                //                                        taxAmount = 0;
                //                                    }
                //                                }

                taxAmount = GetTaxFormula(TaxDetails[i].POT_TAX, total, slNo, TaxDetails, taxText, TaxDetails[i].POT_NAME);




                //                            }

                //                      });
            }
        }
    }
}

function CalculateSubTotal() {
    //<summary>function used to calculate the sub total amount</summary>

    var amount = parseFloat($("[id$=POD_AMOUNT]").val());
    var tax = parseFloat($("[id$=POD_TAX]").val());
    var discount = parseFloat($("[id$=POD_DISCOUNT]").val());
    var subTotal = (((isNaN(amount) ? 0 : amount)
        + (isNaN(tax) ? 0 : tax))
        - (isNaN(discount) ? 0 : discount));
    $("[id$=POD_SUBTOTAL]").val(subTotal.toFixed(AmtDec));
}

function CalculateLineSubTotal(tr) {
    //<summary>function used to calculate item sub total amount</summary>
    var slNo = 0;
    var amount = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_AMOUNT, $(tr).parents("table:first").attr("id")));
    var tax = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_TAX, $(tr).parents("table:first").attr("id")));
    var discount = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_DISC_AMT, $(tr).parents("table:first").attr("id")));
    //Code to edit PO tax,rate,discount
    if ($("[id$=isEditMode]").val() == "1") {
        slNo = $("[id$=hdnSlNo]").val();
        amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
        discount = parseFloat($("#POD_DISC_AMT_" + slNo).val());

    }

    var subTotal = (((isNaN(amount) ? 0 : amount)
        + (isNaN(tax) ? 0 : tax))
        - (isNaN(discount) ? 0 : discount));
    if (isNaN(parseFloat(subTotal)) || subTotal < 0) {
        GrandScriptUtils.ShowModal(PurchaseOrderConfig.AmountShouldBeGreaterthanDiscount);
    }
    else {
        columnIndex = GrandGrid.Utilities.GetColumnIndex(tr, PurchaseOrderConfig.POD_AMT_VALUE, $(tr).parents("table:first").attr("id"));
        if (columnIndex != null) {
            if ($("[id$=isEditMode]").val() == "1") {
                slNo = $("[id$=hdnSlNo]").val();
                $("#POD_SUBTOTAL_" + slNo).val(subTotal.toFixed(AmtDec));
                //$(this).find("td:eq(" + columnIndex + ")").html("<input type=\"text\" style=\"width:90%\"  id=\"POD_SUBTOTAL_" + slNo + "\" value=\"" + subTotal + "\" maxLength =\"30\" tabIndex=\"20\" readonly   class=\"input-notheme\"   ></input>");
            }
            else {
                tr.find("td:eq(" + columnIndex + ")").html(subTotal.toFixed(AmtDec));
            }
        }
    }
}

function CalculateTotal() {
    //<summary>function used to calculate the total amount</summary>

    var amount = parseFloat($("[id$=POH_SUB_TOTAL]").val());

    //Code to edit PO tax,rate,discount
    if ($("[id$=isMoreEditMode]").val() == "0") {

        $("[id$=POH_ADD_TAX_AMT]").val((GetTotalPOCharges(1, amount)).toFixed(AmtDec));
        $("[id$=POH_DISC_AMT]").val((GetTotalPOCharges(3, amount)).toFixed(AmtDec));
        $("[id$=POH_SHIP_CHARGE]").val((GetTotalPOCharges(2, amount)).toFixed(AmtDec));
    }

    var tax = parseFloat($("[id$=POH_ADD_TAX_AMT]").val());
    var discount = parseFloat($("[id$=POH_DISC_AMT]").val());
    var shipCharge = parseFloat($("[id$=POH_SHIP_CHARGE]").val());


    var priceAdj = parseFloat($("[id$=POH_PRICE_ADJUST]").val());
    var subTot = (((isNaN(amount) ? 0 : amount)
        + (isNaN(tax) ? 0 : tax)
        + (isNaN(shipCharge) ? 0 : shipCharge))
        - (isNaN(discount) ? 0 : discount));
    priceAdj = (isNaN(priceAdj) ? 0 : priceAdj);
    if ((subTot + priceAdj) <= 0) {
        priceAdj = 0;
        $("[id$=POH_PRICE_ADJUST]").val(priceAdj);
    }
    var total = subTot + priceAdj;
    $("[id$=POH_TOTAL_VALUE]").val(total.toFixed(AmtDec));
}

function GetTotalPOCharges(type, total) {
    //<summary>function used to get tet tax formula </summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    var formula = "";
    var charges = 0.000;
    for (var i in TaxDetails) {
        if (TaxDetails[i].POT_TAX_CATEGORY == type && TaxDetails[i].IsHeader) {
            if (TaxDetails[i].POT_TYPE == "1") {
                formula = TaxDetails[i].POT_TAX_FORMULA;
                if (formula == "0") {
                    charges += parseFloat(TaxDetails[i].POT_TAX_AMT);
                }
                else {
                    formula = formula.replace(/#SUBTOTAL#/g, total);
                    try {
                        TaxDetails[i].POT_TAX_AMT = parseFloat(eval(formula)).toFixed(AmtDec);
                        charges += parseFloat(TaxDetails[i].POT_TAX_AMT);
                    }
                    catch (e) { }
                }
            }
            else {
                charges += parseFloat(TaxDetails[i].POT_TAX_AMT);
            }
        }
    }
    return parseFloat(charges).toFixed(AmtDec)
}
function ReCalculateSubTotalVal() {
    var slNo = 0;
    var hdrSubTotal = 0;
    var amount = 0;
    $("#grdPODetails tr:has(td)").each(function (index) {
        slNo = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_SL_NO, "grdPODetails");

        amount = parseFloat($("#POD_SUBTOTAL_" + slNo).val());
        $("#POD_SUBTOTAL_" + slNo).val(parseFloat(amount).toFixed(AmtDec));
        hdrSubTotal += parseFloat(amount);
    });
    $("[id$=POH_SUB_TOTAL]").val(hdrSubTotal.toFixed(AmtDec));
}
function ReCalculateSubTotal() {
    //<summary>function used to re-calculate the sub total amount when any change occured in tax  / discount</summary>

    var amount = 0;
    var hdrSubTotal = 0;
    $("#grdPODetails tr:has(td)").each(function (index) {
        amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_AMT_VALUE, $(this).parents("table:first").attr("id"));
        hdrSubTotal += parseFloat(amount);
    });
    $("[id$=POH_SUB_TOTAL]").val(hdrSubTotal.toFixed(AmtDec));
    //Code to edit PO tax,rate,discount
    if ($("[id$=isEditMode]").val() == "1") {
        ReCalculateSubTotalVal();
    }

}

function GetFormula(pk) {
    //<summary>function used to get the tax formula and calculate the tax amount</summary>

    var formula = "";
    var total = parseFloat($("[id$=ItemAmount]").val());
    var taxAmount = 0;
    var finalTotal = 0;
    PurchaseOrderConfig.TaxFormula = formula;

    if (pk != "-1") {
        $("#divTxRatePer").hide();
        $("[id$=TaxAmount]").attr("disabled", true);
        $("[id$=TaxName]").attr("disabled", true);
        $.get(PurchaseOrderConfig.GetTaxFormula + pk + "&Active=2", function (data) {
            if (data != null && data.length > 0) {
                if (pk > 0) {
                    formula = data[0].Formula;
                    PurchaseOrderConfig.TaxFormula = formula;
                    if (formula == "0") {
                        ShowOthercharges();
                    }
                    else {
                        formula = formula.replace(/#SUBTOTAL#/g, total);
                        try {
                            taxAmount = eval(formula);
                        } catch (e) {
                            taxAmount = 0;
                        }
                        if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                            finalTotal = ((isNaN(total) ? 0 : total)
                                - (isNaN(taxAmount) ? 0 : taxAmount));
                        }
                        else {
                            finalTotal = ((isNaN(total) ? 0 : total)
                                + (isNaN(taxAmount) ? 0 : taxAmount));
                        }
                    }
                    $("[id$=TaxAmount]").val(taxAmount.toFixed(AmtDec));
                    $("[id$=TaxName]").val($("[id$=ChooseTax] option:selected").text());
                }
            }
        });
    }
    else {
        $("#divTxRatePer").show();
        $("[id$=TaxAmount]").val("");
        $("[id$=TaxName]").val("");
        $("[id$=TaxAmount]").removeAttr("disabled");
        $("[id$=TaxName]").removeAttr("disabled");
        $("[id*=TaxDiscAmount]").ForceNumericOnly();
    }
}

function ShowOthercharges() {
    $("#divTxRatePer").show();
    $("[id$=TaxAmount]").val("");
    $("[id$=TaxName]").val("");
    $("[id$=TaxAmount]").removeAttr("disabled");
    $("[id$=TaxName]").removeAttr("disabled");
    $("[id*=TaxDiscAmount]").ForceNumericOnly();
}
//14-Mar-19
function CalcPercentage() {
    var Amount = $("[id$=ItemAmount]").val();
    var TaxPer = $("[id$=txtPercentage]").val();
    var DecimalCount = $("[id$=hdfDecimalVal]").val();
    var Total = 0;
    if (TaxPer != '' && TaxPer > 0)
        Total = parseFloat((parseFloat(Amount) * parseFloat(TaxPer) / 100).toFixed(8));
    $("[id$=TaxAmount]").val(Total.toFixed(AmtDec));
}
//For Vendor Tax  8_10

function GetVendorTaxAmount(pk) {
    //<summary>function used to get the tax formula and calculate the tax amount</summary>

    var formula = "";
    var total = parseFloat($("[id$=ItemAmount]").val());
    var taxAmount = 0;
    var finalTotal = 0;
    if (pk != "-1") {
        $("[id$=TaxAmount]").attr("disabled", true);
        $("[id$=TaxName]").attr("disabled", true);
        $.get(PurchaseOrderConfig.GetTaxFormula + pk + "&Active=2", function (data) {
            if (data != null && data.length > 0) {
                if (pk > 0) {
                    formula = data[0].Formula;

                    PurchaseOrderConfig.TaxFormula = formula;
                    formula = formula.replace(/#SUBTOTAL#/g, total);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }
                    if (parseInt($("[id$=IsLineDiscount]").val()) == 3) {
                        finalTotal = ((isNaN(total) ? 0 : total)
                            - (isNaN(taxAmount) ? 0 : taxAmount));
                    }
                    else {
                        finalTotal = ((isNaN(total) ? 0 : total)
                            + (isNaN(taxAmount) ? 0 : taxAmount));
                    }
                }
                return taxAmount.toFixed(AmtDec);
                //$("[id$=TaxName]").val($("[id$=ChooseTax] option:selected").text());
            }
        });
    }
    else {
        $("[id$=TaxAmount]").val("");
        $("[id$=TaxName]").val("");
        $("[id$=TaxAmount]").removeAttr("disabled");
        $("[id$=TaxName]").removeAttr("disabled");
        $("[id*=TaxDiscAmount]").ForceNumericOnly();
    }
}


//End 




function GetTaxDiscountDetails(slNo, type, isLine) {
    //<summary>function used to get the tax / discount details </summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    var TaxArray = new Array();
    for (var i in TaxDetails) {
        if (isLine) {
            if (TaxDetails[i].POT_SL_NO == slNo && TaxDetails[i].POT_TAX_CATEGORY == type) {
                TaxArray.push(TaxDetails[i]);
            }
        }
        else {
            if (TaxDetails[i].POT_TAX_CATEGORY == type && TaxDetails[i].IsHeader) {
                TaxArray.push(TaxDetails[i]);
            }
        }
    }
    return TaxArray;
}

function GetTaxHdrAmount(type) {
    //<summary>function used to get the header tax amount</summary>

    var taxAmount = 0;
    for (var i in TaxDetails) {
        if (TaxDetails[i].IsHeader && TaxDetails[i].POT_TAX_CATEGORY == type) {
            taxAmount += parseFloat(TaxDetails[i].POT_TAX_AMT);
        }
    }
    return taxAmount;
}

function GetItemTaxAmount(slNo, type) {
    //<summary>function used to get the item tax amount</summary>

    var taxAmount = 0;
    for (var i in TaxDetails) {
        if (TaxDetails[i].POT_SL_NO == slNo && TaxDetails[i].POT_TAX_CATEGORY == type) {
            taxAmount += parseFloat(TaxDetails[i].POT_TAX_AMT);
        }
    }
    return taxAmount;
}

function GetItemPRDetails(itemPK, slNo) {
    //<summary>function used to get the item pr details</summary>

    var PRDetails = $("#divData").data("ReqPOList");
    var PRArray = new Array();
    for (var i in PRDetails) {
        if (PRDetails[i].POR_ITEM == itemPK) {
            PRDetails[i].POR_SL_NO = slNo;
            PRArray.push(PRDetails[i]);
        }
    }
    return PRArray;
}

function GetItemTaxDetails(slNo, poDtlsPK) {
    //<summary>function used to get the item tax details</summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    var TaxArray = new Array();
    for (var i in TaxDetails) {
        if (TaxDetails[i].POT_SL_NO == slNo) {
            TaxDetails[i].POT_PO_DTL = poDtlsPK;
            TaxArray.push(TaxDetails[i]);
        }
    }
    return TaxArray;
}

function GetTaxHdrDetails() {
    //<summary>function used to get the header tax/ discount / shippment details</summary>

    var TaxDetails = $("#divData").data("TaxDetails");
    var TaxHdrObj;
    var TaxHdrList = new Array();
    for (var i in TaxDetails) {
        if (TaxDetails[i].IsHeader) {
            TaxHdrObj = new Object();
            TaxHdrObj.PTH_PK = 0;
            TaxHdrObj.PTH_TAX = TaxDetails[i].POT_TAX;
            TaxHdrObj.PTH_TAX_AMT = TaxDetails[i].POT_TAX_AMT;
            TaxHdrObj.PTH_NAME = TaxDetails[i].POT_NAME;
            TaxHdrObj.PTH_TYPE = TaxDetails[i].POT_TYPE;
            TaxHdrObj.PTH_TAX_CATEGORY = TaxDetails[i].POT_TAX_CATEGORY;
            TaxHdrObj.PTH_HAS_SUB_TOTAL = TaxDetails[i].POT_HAS_SUB_TOTAL == undefined ? 0 : TaxDetails[i].POT_HAS_SUB_TOTAL;
            TaxHdrObj.PTH_HAS_DISCOUNT = TaxDetails[i].POT_HAS_DISCOUNT == undefined ? 0 : TaxDetails[i].POT_HAS_DISCOUNT;
            TaxHdrObj.PTH_HAS_OTHER_CHARGE = TaxDetails[i].POT_HAS_OTHER_CHARGE == undefined ? 0 : TaxDetails[i].POT_HAS_OTHER_CHARGE;
            TaxHdrList.push(TaxHdrObj);
        }
    }
    return TaxHdrList;
}

function GetItemSubTotalAmount(currSlNo) {
    //<summary>function used to get the subtotal amount</summary>

    var slNo = 0;
    var colIndex = 0;
    var amount = 0;
    $("#grdPODetails tr:has(td)").each(function (index) {
        slNo = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_SL_NO, $(this).parents("table:first").attr("id"));
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_AMT_VALUE, $(this).parents("table:first").attr("id"));
        if (colIndex != null && currSlNo == slNo) {
            amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_AMT_VALUE, $(this).parents("table:first").attr("id"));
        }
        if (colIndex != null && currSlNo == slNo && $("[id$=isPOEditable]").val() == "1")
            amount = parseFloat($("#POD_SUBTOTAL_" + slNo).val());
    });
    return amount;
}


function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {

        case PurchaseOrderConfig.SaveCommand:
            window.location = $("[id$=hdfBackUrl]").val(); //"PurchaseOrderListing.aspx";
            break;
        case PurchaseOrderConfig.INBOX:
            window.location = PurchaseOrderConfig.InboxURL;
            break;
        case PurchaseOrderConfig.TaxDelete:
            //            DeleteTaxDetails();
            DeleteTempDetails();
            break;

        case PurchaseOrderConfig.Delete:
            DeleteDetails();
            break;

        case PurchaseOrderConfig.DeleteItem:
            DeleteItemDetails();
            break;
        case PurchaseOrderConfig.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}

function IsSelectedPR(itemPK) {
    ///<summary>function used to check item is added through purchase request</summary>

    var PurOrderList = $("#divData").data("ReqPOList");
    for (var i in PurOrderList) {
        if (PurOrderList[i].POR_ITEM == itemPK) {
            return true;
        }
    }
    return false;
}

function ViewSelectedPR(itemPK) {
    ///<summary>function used to get item's purchase request</summary>

    var PurOrderList = $("#divData").data("ReqPOList");

    var SelectedPR = new Array();
    for (var i in PurOrderList) {
        if (PurOrderList[i].POR_ITEM == itemPK) {
            SelectedPR.push(PurOrderList[i]);
        }
    }

    GrandGrid.MakeGrid($("#grdSelectedPurchaseRequest"), 0, SelectedPR);
    $("#divPurchaseRequest").dialog("open");
    $("#divPurchaseRequest").dialog({ "width": 700 });
}

function ShowVendor() {
    //<summary>Function Used to Show Vendor Panel </summary>

    $("#imgVendorHide").show();
    $("#imgVendorShow").hide();
    $("#divVendors").show();

}
function BindVendor() {
    GrandScriptUtils.MakeAutoComplete("txtVendorName", PurchaseOrderConfig.VendorNameAutoCompleteURL + $("[id$=BizUnitPk]").val(), "VEN_PK", true, false, false, true);
    GrandScriptUtils.MakeAutoComplete("txtBillVendor", PurchaseOrderConfig.VendorNameAutoCompleteURL + $("[id$=BizUnitPk]").val(), "POH_DELIVERY", true, false, false, true);
    if ($("[id$=hdfEnableGlovePR]").val() == "1") {// For temperory
        DisableAuto($("[id$=txtBillVendor]"), $("[id$=POH_DELIVERY]"));
    }

    //GrandScriptUtils.MakeAutoComplete("txtVendorName", PurchaseOrderConfig.VendorNameAutoCompleteURL + $("[id$=BizUnitPk]").val() + "&SearchType=VEN_NAME" + "&PageURL=" + PurchaseOrderConfig.PAGEURL, "VEN_PK", true, false, false, true);
}

function HideVendor() {
    //<summary>Function Used to Hide Vendor Panel </summary>

    $("#imgVendorHide").hide();
    $("#imgVendorShow").show();
    $("#divVendors").hide();
}

function ShowPRShow() {
    //<summary>Function Used to Show Purchase Request Panel </summary>

    $("#imgPRHide").show();
    $("#imgPRShow").hide();
    $("#divPendingPR").show();
}

function HidePRShow() {
    //<summary>Function Used to Hide Vendor Panel </summary>

    $("#imgPRHide").hide();
    $("#imgPRShow").show();
    $("#divPendingPR").hide();
}

function SetCommentInList(slNo) {
    //<summary>Function Used to Hide Vendor Panel </summary>

    var PODetails = $("#divData").data("ReqPODetails");
    for (var i in PODetails) {
        if (PODetails[i].POD_SL_NO == slNo) {
            PODetails[i].POD_REMARKS = $("#POD_REMARKS_" + slNo).val();
            break;
        }
    }
}

//function HideColumn() {

//    $("#grdPendingPRList tr:has(th)").each(function (index) {
//      var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_TAX, $(this).parents("table:first").attr("id"));
// });

//}
function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>
    var isEditableMode = $("[id$=isPOEditable]").val();
    $("[id$=isEditMode]").val("0");
    $("[id$=isMoreEditMode]").val("0");
    var purchaseOrderObj = $("#divData").data("purchaseOrderObj");
    var isViewMode = false;
    var isEditMode = false;

    if (PurchaseOrderConfig.IsViewMode) {
        // $("#grdPODetails th:last").hide();
        //        $("#grdTaxDetails th:last").hide();
        isViewMode = true;
        if (isEditableMode == 1) {
            isEditMode = true;
            $("[id$=isEditMode]").val("1");
            $("[id$=TaxDiscAmount]").attr("disabled", "disabled");
            $("[id$=btnDiscApply]").attr("disabled", "disabled");
            $("[id$=TaxRate]").attr("disabled", "disabled");
            $("[id$=btnRateApply]").attr("disabled", "disabled");
        }
        else {
            isEditMode = false;
        }
    }
    else if (purchaseOrderObj != undefined) {
        if (purchaseOrderObj.POH_STATUS == 1 || purchaseOrderObj.POH_STATUS == 7 || purchaseOrderObj.POH_STATUS == 11) {
            // $("#grdPODetails th:last").hide();
            $("#grdTaxDetails th:last").hide();
            isViewMode = true;

            if (isEditableMode == 1) {
                isEditMode = true;
                $("[id$=isEditMode]").val("1");
                $("[id$=TaxDiscAmount]").attr("disabled", "disabled");
                $("[id$=btnDiscApply]").attr("disabled", "disabled");
                $("[id$=TaxRate]").attr("disabled", "disabled");
                $("[id$=btnRateApply]").attr("disabled", "disabled");
            }
        }
    }
    //Changes related with Bug ID:  27532
    if (purchaseOrderObj != undefined) {
        if ($("[id$=hdfRestrictEditOption]").val() == "1") {
            if (purchaseOrderObj.POH_STATUS == 8 || purchaseOrderObj.POH_STATUS == 16 || purchaseOrderObj.POH_STATUS == 22 || purchaseOrderObj.POH_STATUS == 26
                || purchaseOrderObj.POH_STATUS == 101 || purchaseOrderObj.POH_STATUS == 106 || purchaseOrderObj.POH_STATUS == 107 || purchaseOrderObj.POH_STATUS == 109
                || purchaseOrderObj.POH_STATUS == 111 || purchaseOrderObj.POH_STATUS == 117 || purchaseOrderObj.POH_STATUS == 122 || purchaseOrderObj.POH_STATUS == 126) {
                $("#grdTaxDetails th:last").hide();
                isViewMode = true;
                if (isEditableMode == 1) {
                    isEditMode = true;
                    $("[id$=isEditMode]").val("1");
                    $("[id$=TaxDiscAmount]").attr("disabled", "disabled");
                    $("[id$=btnDiscApply]").attr("disabled", "disabled");
                    $("[id$=TaxRate]").attr("disabled", "disabled");
                    $("[id$=btnRateApply]").attr("disabled", "disabled");
                }
            }
        }
    }
    //code to editable tax,rate,discount
    if (isViewMode == false) {
        if (isEditableMode == 1) {
            isEditMode = true;
            $("[id$=isEditMode]").val("1");
            $("[id$=isMoreEditMode]").val("1");
            //$("[id$=POH_DISC_AMT]").removeAttr("disabled");
            $("[id$=POH_SHIP_CHARGE]").removeAttr("disabled");
            //$("[id$=POH_ADD_TAX_AMT]").removeAttr("disabled");
        }
        else {

        }
    }
    else {
        $("[id$=POH_DISC_AMT]").attr("disabled", "disabled");
        $("[id$=POH_SHIP_CHARGE]").attr("disabled", "disabled");
        $("[id$=POH_ADD_TAX_AMT]").attr("disabled", "disabled");
        if (isEditableMode == 1) {
            $("[id$=isMoreEditMode]").val("1");
        }
    }

    if (grdID == "grdPendingPRList") {
        var colIndex = 0;
        var specColIndex = 0;
        var qtyOrdered = "";
        var qtyAddQty = "";
        var reqSpec = "";
        var reqQty = "";
        var tax = 0;
        $("#grdPendingPRList tr:has(td)").each(function (index) {

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.PRD_ITEM_SPEC, $(this).parents("table:first").attr("id"));
            reqQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_ITEM_SPEC, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                if (reqQty == 'undefined') {
                    $(this).find("td:eq(" + colIndex + ")").html('');
                }
            }


            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, $(this).parents("table:first").attr("id"));
            reqQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, $(this).parents("table:first").attr("id"));
            reqQty = parseFloat(reqQty == null || reqQty == "null" || reqQty == "" ? 0 : reqQty);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(reqQty.toFixed(QtyDec)));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ORDERED, $(this).parents("table:first").attr("id"));
            qtyOrdered = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POR_QTY_ORDERED, $(this).parents("table:first").attr("id"));
            qtyOrdered = parseFloat(qtyOrdered == null || qtyOrdered == "null" || qtyOrdered == "" ? 0 : qtyOrdered);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" value=" + qtyOrdered.toFixed(QtyDec) + " class=\"numeric input-w50\"  maxlength=\"11\" tabIndex=\"5\" />");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ADDITIONAL, $(this).parents("table:first").attr("id"));
            qtyAddQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POR_QTY_ADDITIONAL, $(this).parents("table:first").attr("id"));
            qtyAddQty = parseFloat(qtyAddQty == null || qtyAddQty == "null" || qtyAddQty == "" ? 0 : qtyAddQty);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" value=" + qtyAddQty.toFixed(QtyDec) + "  class=\"numeric input-w50\" maxlength=\"11\" tabIndex=\"5\" />");
            }

            specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.PRD_ITEM_SPEC, grdID);
            reqSpec = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_ITEM_SPEC, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_ITEM_SPEC, grdID);
            if (specColIndex != null && reqSpec != null) {
                $(this).find("td:eq(" + specColIndex + ")").html(reqSpec);
            }
            else {
                $(this).find("td:eq(" + specColIndex + ")").html("");
            }
            // For Resolving Bug ID:  2231 (In Html Double white spaces shows as single whitespace.
            specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.ITM_NAME, grdID);
            itmName = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_NAME, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_NAME, grdID);
            if (specColIndex != null && itmName != null) {
                var spaceReplace = itmName.replace(/\s/g, '&nbsp;'); //Replace all whitespace characters with &nbsp;
                $(this).find("td:eq(" + specColIndex + ")").html(spaceReplace);
            }
            //For avoiding Requested Dept. text showing as null
            specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT_TEXT, grdID);
            issuedeptText = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT_TEXT, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT_TEXT, grdID);
            if (specColIndex != null && issuedeptText != null) {
                $(this).find("td:eq(" + specColIndex + ")").html(issuedeptText);
            }
            else {
                $(this).find("td:eq(" + specColIndex + ")").html("");
            }
            $("[id$=hdfCompany]").val(GrandGrid.Utilities.GetColumnValue($(this), "PRH_COMPANY", grdID));

            //Providing hyperlink to print PR
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRH_NO", grdID);
            if (colIndex != null) {
                PRNo = GrandGrid.Utilities.GetColumnValue($(this), "PRH_NO", grdID);
                PRH_PK = GrandGrid.Utilities.GetColumnValue($(this), "PRH_PK", grdID);
                PRH_DEPT = (GrandGrid.Utilities.GetColumnValue($(this), "PRH_DEPT", grdID) != null && GrandGrid.Utilities.GetColumnValue($(this), "PRH_DEPT", grdID) != "null") ? GrandGrid.Utilities.GetColumnValue($(this), "PRH_DEPT", grdID) : 0;
                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"OpenPRNewWindow(" + PRH_PK + "," + PRH_DEPT + ");\" > " + PRNo + " </a>  ");
            }

        });
    }
    if (grdID == "grdPODetails") {
        var colIndex = 0;
        var tax = 0;
        var discount = 0;
        var slNo = 0;
        var amount = 0;
        var subTotal = 0;
        var hdrSubTotal = 0;
        var remarks = "";
        var reqDate = "";
        var itemCode = 0;
        var itemPK = 0;
        var qty = 0;
        var item = "";
        var CurTax = 0;
        var txtBoxRemarks;
        var rateChangeReason = "";



        $("#grdPODetails tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_TAX, $(this).parents("table:first").attr("id"));
            slNo = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_SL_NO, $(this).parents("table:first").attr("id"));
            amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_AMOUNT, $(this).parents("table:first").attr("id"));
            qty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));

            if (colIndex != null && ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.ItemWise || $("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                tax = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_TAX, $(this).parents("table:first").attr("id"));
                //$(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + amount + "');\" src=\"../Images/ERP-Blue/Buttons/addtax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />" + tax + "");
                //$(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" style=\"width:90%\"  id=\"POD_TAX_" + slNo + "\" value=\"" + tax + "\" maxLength =\"30\" tabIndex=\"24\" disabled=\"disabled\" ></input>");
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").hide();
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_DISC_AMT, $(this).parents("table:first").attr("id"));
            if (colIndex != null && ($("[id$='isDiscountAdd']").val() == PurchaseOrderConfig.ItemWise || $("[id$='isDiscountAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                discount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_DISC_AMT, $(this).parents("table:first").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscount('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/ERP-Blue/Buttons/deducttax.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + discount + "");
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").hide();
                //   $(this).find("td:nth-child(" + colIndex + ")").hide();
                // $('tr td:nth-child(1)').hide();
            }

            //            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_TAX, $(this).parents("table:first").attr("id"));
            //            if (colIndex != null) {
            //                CurTax = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_H_TAX, $(this).parents("table:first").attr("id"));
            //                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" style=\"width:90%\"  id=\"POD_H_TAX_" + slNo + "\" value=\"" + CurTax + "\" maxLength =\"30\" tabIndex=\"19\" disabled=\"disabled\" ></input>");

            //            } 

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_AMT_VALUE, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                subTotal = (parseFloat(amount) + parseFloat(tax)) - parseFloat(discount);
                $(this).find("td:eq(" + colIndex + ")").html(subTotal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_REMARKS, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                remarks = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_REMARKS, $(this).parents("table:first").attr("id"));
                remarks = (remarks == "null") ? "" : remarks;
                if (isViewMode) {
                    txtBoxRemarks = document.createElement("textarea");
                    txtBoxRemarks.id = "POD_REMARKS_" + slNo;
                    $(txtBoxRemarks).attr("cols", "10");
                    $(txtBoxRemarks).attr("disabled", true);
                    $(txtBoxRemarks).attr("rows", "2");
                    $(txtBoxRemarks).css({ "width": "90%" });
                    $(txtBoxRemarks).css({ "height": "30px" });
                    $(txtBoxRemarks).val(remarks);
                    $(this).find("td:eq(" + colIndex + ")").html("");
                    $(txtBoxRemarks).css({ "display": "none" });
                    $(this).find("td:eq(" + colIndex + ")").append($(txtBoxRemarks));
                    var remarkText = getShortString(remarks, 18);
                    if (remarks == "") {
                        remarkText = "<span style='color:#0635FF; width:25px!important; cursor:pointer; text-decoration: underline !important;'>Add</span>";
                    }
                    var t = "<a style=\'cursor:pointer\' onclick=\'javascript:ViewSelectedComment(" + slNo + ");\' title='" + remarks + "'>" + remarkText + "</a>";
                    $(this).find("td:eq(" + colIndex + ")").append(t);
                    // $(this).find("td:eq(" + colIndex + ")").html("<input type=\"textarea\" cols=\"20\" rows=\"2\" style=\"width:90%\"  id=\"POD_REMARKS_" + slNo + "\" value=\"" + remarks + "\" maxLength =\"30\" tabIndex=\"24\" disabled=\"disabled\" ></input>");
                }
                else {
                    txtBoxRemarks = document.createElement("textarea");
                    txtBoxRemarks.id = "POD_REMARKS_" + slNo;
                    $(txtBoxRemarks).attr("cols", "10");
                    $(txtBoxRemarks).attr("maxLength", "500");
                    $(txtBoxRemarks).attr("rows", "2");
                    $(txtBoxRemarks).css({ "width": "90%" });
                    $(txtBoxRemarks).css({ "height": "30px" });
                    $(txtBoxRemarks).val(remarks);
                    $(this).find("td:eq(" + colIndex + ")").html("");
                    $(txtBoxRemarks).css({ "display": "none" });
                    $(this).find("td:eq(" + colIndex + ")").append($(txtBoxRemarks));
                    var remarkText = getShortString(remarks, 18);
                    if (remarks == "") {
                        remarkText = "<span style='color:#0635FF; width:25px!important; cursor:pointer; text-decoration: underline !important;'>Add</span>";
                    }
                    var t = "<a style=\'cursor:pointer\' onclick=\'javascript:ViewSelectedComment(" + slNo + ");\' title='" + remarks + "'>" + remarkText + "</a>";
                    $(this).find("td:eq(" + colIndex + ")").append(t);
                    //$(this).find("td:eq(" + colIndex + ")").html("<input type=\"textarea\" cols=\"20\" rows=\"2\" style=\"width:90% height:20px\"  id=\"POD_REMARKS_" + slNo + "\" value=\"" + remarks + "\" maxLength =\"30\" tabIndex=\"24\" onchange=\"javascript:SetCommentInList('" + slNo + "');\" ></input>");
                }
            }
            //Code to change PO editable (Tax,Rate,Discount,Shipping Cost)
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_TAX, $(this).parents("table:first").attr("id"));
            if (colIndex != null && ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.ItemWise || $("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                tax = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_TAX, $(this).parents("table:first").attr("id"));
                tax = (tax == "null") ? "" : parseFloat(tax).toFixed(AmtDec);

                if (isEditMode) {
                    $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + amount + "');\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />" + parseFloat(tax).toFixed(AmtDec) + "<input type=\"hidden\"  class=\"numeric input-w70\"  id=\"POD_H_TAX_" + slNo + "\" value=\"" + tax + "\"  ></input>");
                }
                else {
                    $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + amount + "');\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />" + parseFloat(tax).toFixed(AmtDec) + "");
                }
                $(this).find("td:eq(" + colIndex + ")").addClass("colicon-left");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_AMOUNT, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_AMOUNT, $(this).parents("table:first").attr("id"));
                amount = (amount == "null") ? "" : parseFloat(amount).toFixed(AmtDec);
                if (isEditMode) {
                    $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" id=\"POD_H_AMOUNT_" + slNo + "\" value=\"" + amount + "\" maxLength =\"30\" readonly   class=\"numeric input-w70 input-disabled\" ></input>");
                }
                else {

                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));
                qty = (qty == "null") ? "" : qty;
                if (isEditMode) {
                    $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec) + "<input type=\"hidden\"  class=\"numeric input-w50\" id=\"POD_H_QTY_REQUESTED_" + slNo + "\" value=\"" + qty + "\"  ></input>");
                    // $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" id=\"POD_H_QTY_REQUESTED_" + slNo + "\" value=\"" + parseFloat(qty).toFixed(QtyDec) + "\" maxLength =\"30\"  readonly   class=\"numeric input-w80 input-disabled\"  ></input>");
                }
                else {

                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_AMT_VALUE, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                subTotal = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_AMT_VALUE, $(this).parents("table:first").attr("id"));
                subTotal = (subTotal == "null") ? "" : parseFloat(subTotal).toFixed(AmtDec);
                if (isEditMode) {
                    $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" id=\"POD_SUBTOTAL_" + slNo + "\" value=\"" + subTotal + "\" maxLength =\"30\"  readonly   class=\"numeric input-w70 input-disabled\"  ></input>");
                }
                else {

                }
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_RATE, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                var isRateUpdate = GrandGrid.Utilities.GetColumnValue($(this), "POD_RATE_UPDATE", $(this).parents("table:first").attr("id"));

                rate = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_RATE, $(this).parents("table:first").attr("id"));
                itemCode = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_TEXT, $(this).parents("table:first").attr("id"));
                itemPK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_ITEM, $(this).parents("table:first").attr("id"));
                rateChangeReason = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_REASON, $(this).parents("table:first").attr("id"));
                rateChangeReason = (rateChangeReason == "null" || rateChangeReason == "undefined") ? "" : rateChangeReason;
                rate = (rate == "null") ? "" : rate;
                if (isEditMode) {
                    //                    $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_RATE_" + slNo + "\" onclick=\"javascript:AddLineItemRate('" + slNo + "','" + qty + "','" + itemPK + "','" + isRateUpdate + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/rate.png\" alt=\"Translate(Rate)\" title=\"Translate(Rate)\" style=\"cursor:pointer\" />" + "<input type=\"text\" id=\"POD_RATE_" + slNo + "\" value=\"" + parseFloat(rate) + "\" maxLength =\"30\" readonly   class=\"numeric input-w58 input-disabled\"  ></input>");
                    $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_RATE_" + slNo + "\" onclick=\"javascript:AddLineItemRate('" + slNo + "','" + qty + "','" + itemPK + "','" + isRateUpdate + "','" + rateChangeReason + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/rate.png\" alt=\"Translate(Rate)\" title=\"Translate(Rate)\" style=\"cursor:pointer\" />" + "<input type=\"text\" id=\"POD_RATE_" + slNo + "\" value=\"" + parseFloat(rate) + "\" maxLength =\"30\" readonly   class=\"numeric input-w58 input-disabled\"  ></input>");

                }
                else {

                }
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_DISC_AMT, $(this).parents("table:first").attr("id"));
            if (colIndex != null && ($("[id$='isDiscountAdd']").val() == PurchaseOrderConfig.ItemWise || $("[id$='isDiscountAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                discount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_DISC_AMT, $(this).parents("table:first").attr("id"));
                discount = (discount == "null") ? "" : discount;
                if (isEditMode) {
                    // $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscAmount('" + slNo + "','" + amount + "');\" src=\"../Images/ERP-Blue/Buttons/deducttax.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + "<input type=\"text\" style=\"width:70%\"  id=\"POD_DISC_AMT_" + slNo + "\" value=\"" + discount + "\" maxLength =\"30\" tabIndex=\"20\"  readonly   class=\"input-notheme\" ></input>");
                    //                    $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_DISC_" + slNo + "\" onclick=\"javascript:AddLineItemDiscAmount('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/discount.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + "<input type=\"text\" id=\"POD_DISC_AMT_" + slNo + "\" value=\"" + parseFloat(discount).toFixed(3) + "\" maxLength =\"30\" tabIndex=\"20\" readonly   class=\"numeric input-w70\"  ></input>");
                    $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_DISC_" + slNo + "\" onclick=\"javascript:AddLineItemDiscountDtl('" + slNo + "','" + amount + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/discount.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />" + "<input type=\"text\" id=\"POD_DISC_AMT_" + slNo + "\" value=\"" + parseFloat(discount).toFixed(AmtDec) + "\" maxLength =\"30\" readonly   class=\"numeric input-w58 input-disabled\"  ></input>");

                }
                else {

                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.ITM_TEXT, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                itemCode = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.ITM_TEXT, $(this).parents("table:first").attr("id"));
                itemPK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_ITEM, $(this).parents("table:first").attr("id"));
                if (IsSelectedPR(itemPK)) {
                    // if ($("[id$=POH_IS_AMEND]").val() == "0") {
                    $(this).find("td:last: [id$=imbEdit]").hide();
                    //  }
                    $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ViewSelectedPR('" + itemPK + "');\" > " + itemCode + " </a>  ");
                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_REQD_DATE, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                reqDate = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_REQD_DATE, $(this).parents("table:first").attr("id"));
                if (isViewMode) {
                    $(this).find("td:eq(" + colIndex + ")").html("<input tabIndex=\"25\" onkeydown=\"return CheckKey(event)\" onpaste=\"return false;\" type=\"text\" class=\"date-picker maxw-85per\" id=\"PODtlReqDate_" + slNo + "\" value=\"" + reqDate + "\" disabled=\"disabled\" ></input>");
                }
                else {
                    $(this).find("td:eq(" + colIndex + ")").html("<input tabIndex=\"25\" onkeydown=\"return CheckKey(event)\" onpaste=\"return false;\" type=\"text\" class=\"date-picker maxw-85per\"  id=\"PODtlReqDate_" + slNo + "\" value=\"" + reqDate + "\" ></input>");
                    if (reqDate == "") {
                        GrandScriptUtils.DatePicker("PODtlReqDate_" + slNo, false, false, true, "dd-M-yy");
                    }
                    else {
                        GrandScriptUtils.DatePicker("PODtlReqDate_" + slNo, false, false, false, "dd-M-yy");
                    }
                }
            }

            //PO Quantity Amend Area Strat ************************************  
            if (isViewMode) {
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));
                if (colIndex != null) {
                    QtyRequested = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(QtyRequested) + "<input type=\"hidden\"  class=\"numeric input-w50\" id=\"POD_H_QTY_REQUESTED_" + slNo + "\" value=\"" + QtyRequested + "\"  ></input>");
                }
            }
            else {
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));
                if (colIndex != null) {
                    POD_PK = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_PK, $(this).parents("table:first").attr("id"));
                    QtyForAmend = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POD_QTY_REQUESTED, $(this).parents("table:first").attr("id"));
                    if (($("[id$=POH_IS_AMEND]").val() == "1") || ($("[id$=PurchaseOrderStatus]").val() == 8) || ($("[id$=PurchaseOrderStatus]").val() == 6)) { //QtyEditbtn visibility Setting.6=>Request For MoreDetails(after submitting Authorize & Approve) & 8 =>Request For MoreDetails (After Submit For Approval).                      
                        if (IsSelectedPR(itemPK)) {
                            $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_QtyAmend_" + slNo + "\" onclick=\"javascript:BindPRGridPopupAmend('" + POD_PK + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/edit-top-menu.png\" alt=\"Translate(AmendQty)\" title=\"Translate(AmendQty)\" style=\"cursor:pointer\" />" + "<input type=\"text\" id=\"POD_QTY_REQUESTED_" + slNo + "\" value=\"" + parseFloat(QtyForAmend) + "\" maxLength =\"30\" readonly   class=\"numeric input-w58 input-disabled\"  ></input>"
                                + "<input type=\"hidden\"  class=\"numeric input-w50\" id=\"POD_H_QTY_REQUESTED_" + slNo + "\" value=\"" + QtyForAmend + "\"  ></input>");
                        }
                        else {//Item not come from PR
                            $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(QtyForAmend) + "<input type=\"hidden\"  class=\"numeric input-w50\" id=\"POD_H_QTY_REQUESTED_" + slNo + "\" value=\"" + QtyForAmend + "\"  ></input>");
                        }
                    }
                    else {
                        $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(QtyForAmend) + "<input type=\"hidden\"  class=\"numeric input-w50\" id=\"POD_H_QTY_REQUESTED_" + slNo + "\" value=\"" + QtyForAmend + "\"  ></input>");
                    }
                }
            }
            //End PO Quantity ***********************************************************

            if (isViewMode) {
                //$(this).find("td:last").hide();
                //$(this).find("td:last").hide();
                $(this).find("td:last input[id$=imbEdit]").hide();
                $(this).find("td:last input[id$=imbDelete]").hide();
            }
            hdrSubTotal += parseFloat(subTotal);
        });


        $("#grdPODetails tr:has(th)").each(function (index) {
            var vendorCurrency = '(' + $("[id$=VendorCurrencyCode]").val() + ')';

            //            var rateIndex = 0;
            //            rateIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_RATE", $(this).parents("table:first").attr("id"));
            //            $(this).find("th:eq(" + rateIndex + ")").html(PurchaseOrderConfig.RateHeading + vendorCurrency);

            //            var amountIndex = 0;
            //            amountIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_AMOUNT", $(this).parents("table:first").attr("id"));
            //            $(this).find("th:eq(" + amountIndex + ")").html(PurchaseOrderConfig.AmountHeading + vendorCurrency);

            //            var taxIndex = 0;
            //            taxIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_TAX", $(this).parents("table:first").attr("id"));
            //            $(this).find("th:eq(" + taxIndex + ")").html(PurchaseOrderConfig.TaxHeading + vendorCurrency);

            //            var discountIndex = 0;
            //            discountIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_DISC_AMT", $(this).parents("table:first").attr("id"));
            //            $(this).find("th:eq(" + discountIndex + ")").html(PurchaseOrderConfig.DiscountHeading + vendorCurrency);

            var subTotalIndex = 0;
            subTotalIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_AMT_VALUE", $(this).parents("table:first").attr("id"));
            $(this).find("th:eq(" + subTotalIndex + ")").html(PurchaseOrderConfig.SubTotalHeading);


            var ColHeadTaxIndex = 0;
            ColHeadTaxIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_TAX", $(this).parents("table:first").attr("id"));
            if (ColHeadTaxIndex != null && ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.ItemWise || $("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                $(this).find("th:eq(" + ColHeadTaxIndex + ")").show();
            }
            else {
                $(this).find("th:eq(" + ColHeadTaxIndex + ")").hide();
            }


            var ColHeadDiscIndex = 0;
            ColHeadDiscIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_DISC_AMT", $(this).parents("table:first").attr("id"));
            if (ColHeadDiscIndex != null && ($("[id$='isDiscountAdd']").val() == PurchaseOrderConfig.ItemWise || $("[id$='isDiscountAdd']").val() == PurchaseOrderConfig.BothHeaderItem)) {
                $(this).find("th:eq(" + ColHeadDiscIndex + ")").show();
            }
            else {
                $(this).find("th:eq(" + ColHeadDiscIndex + ")").hide();
            }
        });
        $("[id$=POH_SUB_TOTAL]").val(hdrSubTotal.toFixed(AmtDec));
        CalculateTotal();
    }


    if (grdID == "grdSelectedPurchaseRequest") {
        var colIndex = 0;
        var qty = 0;
        var colValue = '';
        $("#grdSelectedPurchaseRequest tr:has(th)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POR_COST_CENTER_TEXT", $(this).parents("table:first").attr("id"));
            if ($("[id$=hdfEnbleCostCenter]").val() == "0") {
                if (colIndex != null) {
                    $(this).find("th:eq(" + colIndex + ")").hide();
                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POR_QTY_ORDERED", $(this).parents("table:first").attr("id"));
            if ($("[id$=hdfEnbleCostCenter]").val() == "0") {
                if (colIndex != null) {
                    $(this).find("th:eq(" + colIndex + ")").hide();
                }
            }
        });
        $("#grdSelectedPurchaseRequest tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRD_QTY_APPROVED", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "PRD_QTY_APPROVED", grdID);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POR_COST_CENTER_TEXT", grdID);
            colValue = GrandGrid.Utilities.GetColumnValue($(this), "POR_COST_CENTER_TEXT", grdID);
            if ($("[id$=hdfEnbleCostCenter]").val() == "0") {
                $(this).find("td:eq(" + colIndex + ")").hide();
            }
            else {
                if (colIndex != null) {
                    if (colValue == 'undefined' || colValue == '')
                        $(this).find("td:eq(" + colIndex + ")").html('');
                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POR_QTY_ORDERED", grdID);
            if ($("[id$=hdfEnbleCostCenter]").val() == "1") {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "POR_QTY_ORDERED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec));
                }
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").hide();
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRH_NO", grdID);
            if (colIndex != null) {
                PRNo = GrandGrid.Utilities.GetColumnValue($(this), "PRH_NO", grdID);
                PRH_PK = GrandGrid.Utilities.GetColumnValue($(this), "PRH_PK", grdID);
                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"Print_PR(" + PRH_PK + ");\" > " + PRNo + " </a>  ");
            }
        });
    }

    if (grdID == "grdRevisionHistory") {
        var poNumber = "";
        var colIndex = 0;
        var version = 0;
        $("#grdRevisionHistory tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POH_NO", $(this).parents("table:first").attr("id"));
            version = GrandGrid.Utilities.GetColumnValue($(this), "POH_VERSION", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                poNumber = GrandGrid.Utilities.GetColumnValue($(this), "POH_NO", $(this).parents("table:first").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ViewReport('" + version + "');\" > " + poNumber + " </a>  ");
            }
        });

        //        $("#grdRevisionHistory tr:has(td)").each(function (index) {
        //            var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POH_PK", $(this).parents("table:first").attr("id"));
        //            var itemPK = GrandGrid.Utilities.GetColumnValue($(this), "POH_PK", $(this).parents("table:first").attr("id"));
        //            if (colIndex != null) {
        //                poNumber = GrandGrid.Utilities.GetColumnValue($(this), "POH_NO", $(this).parents("table:first").attr("id"));
        //                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ViewReport('" + itemPK + "');\" > " + "Hi.." + " </a>  ");
        //            }
        //        });
    }
    if (grdID == "grdTaxDetails") {
        var colIndex = 0;
        var colValue = 0;
        $("#grdTaxDetails tr:has(td)").each(function (index) {
            if (isViewMode) {
                $(this).find("td:last").hide();
                $(this).find("td:last").hide();

            }
            //Comma Separation for Amount 
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POT_TAX_AMT", $(this).parents("table:first").attr("id"));
            colValue = GrandGrid.Utilities.GetColumnValue($(this), "POT_TAX_AMT", $(this).parents("table:first").attr("id"));
            colValue = colValue == null || colValue == "null" ? 0 : colValue;
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(colValue));
            }

        });
    }
    if (grdID == "grdPOList") {
        var colIndex = 0;
        var qtyOrdered = "";
        var qtyAddQty = "";
        var reqQty = "";

        $("#grdPOList tr:has(th)").each(function (index) {
            $(this).find("th:last-child").html("");
        });

        $("#grdPOList tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, $(this).parents("table:first").attr("id"));
            reqQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_BALANCE, $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            reqQty = parseFloat(reqQty == null || reqQty == "null" || reqQty == "" ? 0 : reqQty);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(reqQty.toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.PRD_QTY_APPROVED, $(this).parents("table:first").attr("id"));
            qtyOrdered = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRD_QTY_APPROVED, $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            qtyOrdered = parseFloat(qtyOrdered == null || qtyOrdered == "null" || qtyOrdered == "" ? 0 : qtyOrdered);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(qtyOrdered.toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ORDERED, $(this).parents("table:first").attr("id"));
            qtyAddQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POR_QTY_ORDERED, $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            qtyAddQty = parseFloat(qtyAddQty == null || qtyAddQty == "null" || qtyAddQty == "" ? 0 : qtyAddQty);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(qtyAddQty.toFixed(QtyDec)));
            }
            //Providing hyperlink to print PR
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRH_NO", grdID);
            if (colIndex != null) {
                PRNo = GrandGrid.Utilities.GetColumnValue($(this), "PRH_NO", grdID);
                PRH_PK = GrandGrid.Utilities.GetColumnValue($(this), "PRH_PK", grdID);
                PRH_DEPT = (GrandGrid.Utilities.GetColumnValue($(this), "PRH_DEPT", grdID) != null && GrandGrid.Utilities.GetColumnValue($(this), "PRH_DEPT", grdID) != "null") ? GrandGrid.Utilities.GetColumnValue($(this), "PRH_DEPT", grdID) : 0;
                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"OpenPRNewWindow(" + PRH_PK + "," + PRH_DEPT + ");\" > " + PRNo + " </a>  ");
            }
        });
    }
    if (grdID == "grdRateDetails") {
        var colRating = 0;
        var colLastQtDate = 0;
        var colLastQtRate = 0;
        var colLastOrdDate = 0;
        var colLastOrdRate = 0;
        var colLastOrdQty = 0;
        var colLead = 0;

        var rating = "";
        var lastQtDate = null;
        var lastQtRate = null;
        var lastOrdDate = null;
        var lastOrdRate = null;
        var lastOrdQty = null;
        var lead = null;
        $("#grdRateDetails tr:has(td)").each(function (index) {
            colRating = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_RATING, $(this).parents("table:first").attr("id"));
            colLastQtDate = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_LAST_QUOT_DATE, $(this).parents("table:first").attr("id"));
            colLastQtRate = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_LAST_QUOT_RATE, $(this).parents("table:first").attr("id"));
            colLastOrdDate = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_LAST_ORDR_DATE, $(this).parents("table:first").attr("id"));
            colLastOrdRate = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_LAST_ORDR_RATE, $(this).parents("table:first").attr("id"));
            colLastOrdQty = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_LAST_ORDR_QTY, $(this).parents("table:first").attr("id"));
            colLead = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.VIH_LEAD_TIME, $(this).parents("table:first").attr("id"));

            rating = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_RATING, grdID);
            rating = rating == null || rating == "null" ? "" : rating;
            lastQtRate = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_QUOT_RATE, grdID) == "null" ||
                GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_QUOT_RATE, grdID) == "" ? null :
                parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_QUOT_RATE, grdID));
            lastOrdRate = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_ORDR_RATE, grdID) == "null" ||
                GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_ORDR_RATE, grdID) == "" ? null :
                parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_ORDR_RATE, grdID));

            lastOrdQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_ORDR_QTY, grdID) == "null" ||
                GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_ORDR_QTY, grdID) == "" ? null :
                parseFloat(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LAST_ORDR_QTY, grdID));

            lead = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.VIH_LEAD_TIME, grdID);
            lead = lead == null || lead == "null" ? "" : lead;
            if (colLastQtRate != null && lastQtRate != null) {
                $(this).find("td:eq(" + colLastQtRate + ")").html(lastQtRate.toFixed(RateDec));
            }
            else {
                $(this).find("td:eq(" + colLastQtRate + ")").html("");
            }
            if (colLastOrdRate != null && lastOrdRate != null) {
                $(this).find("td:eq(" + colLastOrdRate + ")").html(lastOrdRate.toFixed(RateDec));
            }
            else {
                $(this).find("td:eq(" + colLastOrdRate + ")").html("");
            }

            if (colLastOrdQty != null && lastOrdQty != null) {
                $(this).find("td:eq(" + colLastOrdQty + ")").html(lastOrdQty.toFixed(QtyDec));
            }
            else {
                $(this).find("td:eq(" + colLastOrdQty + ")").html("");
            }

            if (colLead != null && lead != null) {
                $(this).find("td:eq(" + colLead + ")").html(lead);
            }
            else {
                $(this).find("td:eq(" + colLead + ")").html("");
            }

        });
    }

    //*************grdAmendPRDetails  START *************************************************************
    if (grdID == "grdAmendPRDetails") {
        var poNumber = "";
        var colIndex = 0;
        var version = 0;
        $("#grdAmendPRDetails tr:has(td)").each(function (index) {

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ORDERED, $(this).parents("table:first").attr("id"));
            qtyOrdered = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POR_QTY_ORDERED, $(this).parents("table:first").attr("id"));
            qtyOrdered = parseFloat(qtyOrdered == null || qtyOrdered == "null" || qtyOrdered == "" ? 0 : qtyOrdered);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" value=" + qtyOrdered.toFixed(QtyDec) + " class=\"numeric input-w70\"  maxlength=\"11\" tabIndex=\"5\" />");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseOrderConfig.POR_QTY_ADDITIONAL, $(this).parents("table:first").attr("id"));
            qtyAddQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.POR_QTY_ADDITIONAL, $(this).parents("table:first").attr("id"));
            qtyAddQty = parseFloat(qtyAddQty == null || qtyAddQty == "null" || qtyAddQty == "" ? 0 : qtyAddQty);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" value=" + qtyAddQty.toFixed(QtyDec) + "  class=\"numeric input-w70\" maxlength=\"11\" tabIndex=\"5\" />");
            }

        });

    }
    //************END grdAmendPRDetails ********************************************************************
}

function ViewReport(version) {
    var url = PurchaseOrderConfig.POREPORTURL + "?ID=" + $("input[id$=POH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val() + "&RevID=" + version;
    OpenPDF(url);
}
//#endregion

///#region----------- Validations ----------------

function AddValidations(mode) {
    //<summary>function used validate each sections </summary>
    RemoveAllValidations();
    if (mode == 1) {
        $("input[id$=POH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(ReqRequredDate)" }
        });
        $("select[id$=POH_SHIPPING]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ReqShippingDetail)" }
        });
        //Config check

        if ($("[hdfPohEmployee]").val() == "1") {
            $("select[id$=POH_EMPLOYEE]").rules("add", {
                selectNone: true,
                messages: { selectNone: "Translate(ReqPoCreator)" }
            });
        }

        $("select[id$=POH_CURRENCY]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ReqCurrency)" }
        });
        $("select[id$=POH_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ReqType)" }
        });
        $("select[id$=POH_ITEM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ReqPOCategory)" }
        });
        if ($("[id$=hdfEnableGlovePR]").val() == "1") {
            //            $("[id$=txtBillVendor]").rules("add", {
            //                selectAuto: true,
            //                messages: { selectAuto: "Translate(SelectDeliveryTo)" }
            //            });
        }
        else {

            $("select[id$=POH_BILLING]").rules("add", {
                selectNone: true,
                messages: { selectNone: "Translate(ReqBillingDetail)" }
            });
        }

        $("select[id$=POH_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectDepartment)" }
        });
        $("input[id$=POH_PRICE_ADJUST]").rules("add", {
            ZeroDecimal: true
        });
        //        $("textarea[id$=POH_COMMENTS]").rules("add", {
        //            maxlength: 200
        //        });
        $("input[id$=POH_SUB_TOTAL]").rules("add", {
            ThreeDecimal: true
        });
        $("input[id$=POH_TOTAL_VALUE]").rules("add", {
            ThreeDecimal: true
        });
    }
    else if (mode == 2) {
        $("[id$=ItemAmount]").rules("add", {
            ZeroDecimal: true
        });
        $("[id$=ChooseTax]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectType)" }
        });
        $("[id$=TaxName]").rules("add", {
            required: true,
            messages: { required: "Translate(ReqRequredTaxName)" }
        });
        $("[id$=TaxAmount]").rules("add", {
            required: true,
            ThreeDecimal: true,
            messages: { required: "Translate(EnterAmount)" }
        });
    }
    else if (mode == 3) {

        $("[id$=ITM_CODE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectItem)" }
        });
        //        $("[id$=POD_RATE]").rules("add", {
        //            required: true,
        //            ThreeDecimal: true,
        //            messages: { required: "Translate(EnterRate)" }
        //        });
        $("input[id$=POD_RATE]").rules("add", {
            required: true,
            //DecimalDigits: RateDec,
            //CustomDecimal: true,
            maxlength: 15,
            ZeroDecimal: true
            //messages: { required: "Translate(EnterRate)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", RateDec) }
        });

        $("[id$=POD_QTY]").rules("add", {
            required: true,
            //            ThreeDecimal: true,
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            messages: { required: "Translate(EnterQty)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });
        $("[id$=POD_REQD_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(ReqRequredDate)" }
        });
    }
    else if (mode == 4) {
        $("[id$=ITM_CODE]").rules("add", {
            NumericExceptZero: true,
            messages: { selectNone: "Translate(SelectItem)" }
        });
    }
    else if (mode == 5) {
        $("select[id$=ddlPOhCategory]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectPurType)" }
        });
    }
}

function RemoveAllValidations() {
    //<summary>function used Remove validation </summary>

    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

///#endregion

//#region---- Convertion Factor, Now It is not Using It May be helpfull when it needed 

function GetUomConversion(UOMID) {

    var materialID = $("select[id$=ITM_CODE]").val();
    var Price = 0.000;
    if (UOMID != "0") {
        $.get(PurchaseOrderConfig.GetMaterialUOMConversion + materialID + "&UOMId=" + UOMID, function (data) {
            if (data.length > 0) {
                $("input[id$=POD_CONV_FACT]").val(data[0].UMC_CONV_FACT);
                price = (parseFloat($("input[id$=PerPiecePrice]").val()) * (1 / parseFloat(data[0].UMC_CONV_FACT)));
                $("[id$=MaterialPrice]").val(price);
            }
        });
    }
    else {
        $("input[id$=POD_CONV_FACT]").val("1");
        $.get(PurchaseOrderConfig.GetMaterialDetails + vendorID + "&MaterialId=" + materialID, function (data) {
            if (data.length > 0) {
                price = (parseFloat(data[0].ITM_PRICE) * parseFloat(data[0].UMC_CONV_FACT));
                $("[id$=MaterialPrice]").val(price);
            }
        });
    }
}

//#endregion

var POObject = {
    TaxHeader: new Array(),
    PODetails: new Array()
}

var Terms = new Array();
var VendorTerms = new Array();
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Purchase";

function GridMaterialAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr" type="Object">
    /// Specific Container and its controls
    /// </param>
    /// <param name="command" type="Object">
    /// Specific Edit/Delete
    /// </param>

    switch (command.toString().toUpperCase()) {
        case PurchaseOrderConfig.HistoryCommand:
            var PurOrderList = new Object();
            PurOrderList.ItemDetails = $("#divData").data("ReqPOList");
            materialID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POR_ITEM, $(tr).parents("table:first").attr("id"));
            ShowRateHistory(materialID);
            break;
        case PurchaseOrderConfig.HistoryCommandPODetails:
            materialID = GrandGrid.Utilities.GetColumnValue(tr, PurchaseOrderConfig.POD_ITEM, $(tr).parents("table:first").attr("id"));
            ShowRateHistory(materialID);
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            break;
    }
    return false;
}

function ShowRateHistory(itemPK) {
    //<summary>function used to show rate history</summary>
    ///////////////
    ajaxurl = PurchaseOrderConfig.GetItemRates + "&ItemPK=" + itemPK;
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(ajaxurl, function (data) {
        if (data.length > 0) {
            $("[id$=lblItemCode]").html(data[0].VIH_ITEM_CODE.length > 30 ? data[0].VIH_ITEM_CODE.substr(0, 30) + "..." : data[0].VIH_ITEM_CODE);
            $("[id$=lblItemCode]").attr('title', data[0].VIH_ITEM_CODE);
            $("[id$=lblItemName]").html(data[0].VIH_ITEM_NAME.length > 30 ? data[0].VIH_ITEM_NAME.substr(0, 30) + "..." : data[0].VIH_ITEM_NAME);
            $("[id$=lblItemName]").attr('title', data[0].VIH_ITEM_NAME);
            GrandGrid.MakeGrid($("#grdRateDetails"), 0, data);
            $("#divRateHistory").dialog("open");
            $("#divRateHistory").dialog(
                {
                    width: 910,
                    height: 500,
                    title: "Translate(RateHistory)"
                });
        }
        else {
            //Message
            $("[id$=lblItemCode]").val("");
            $("[id$=lblItemCode]").attr('title', '');
            $("[id$=lblItemName]").val("");
            $("[id$=lblItemName]").attr('title', '');
            GrandGrid.MakeGrid($("#grdRateDetails"), 0, new Array());
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoHistory, PurchaseOrderConfig.Information);
            return false;
        }
    });

}

function UpdateLineItemTax(slNo, isGrid, newRate) {
    var formula = "";
    var taxAmount = 0;
    var TaxDetailsObj = null;
    var amount = 0;
    var hdrAmt = 0;
    var hdrDisc = 0;
    var rate = 0;
    var slNoText = $("[id$=hdnSlNo]").val();
    $("[id$=hdnSlNo]").val(slNo);

    var itemAmt = 0;
    var itemDisc = 0;

    //---For Item Disc---
    if (isGrid) {
        amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
    }
    else {
        amount = parseFloat($("[id$=POD_AMOUNT]").val());
    }
    if (newRate == undefined)
        rate = parseFloat($("#POD_RATE_" + slNo).val());
    else
        rate = newRate;

    var TaxDetails = $("#divData").data("TaxDetails");
    var curTax = GetTaxDiscountDetails(slNo, 3, true);
    var totDisc = 0.0;
    for (var selectedTax in curTax) {
        TaxDetailsObj = JSLINQ(TaxDetails)
            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == curTax[selectedTax].POT_TAX && tax.POT_NAME == curTax[selectedTax].POT_NAME; })
            .FirstOrDefault(null);
        if (TaxDetailsObj != null) {
            if (TaxDetailsObj.POT_TYPE == "1") {
                formula = TaxDetailsObj.POT_TAX_FORMULA;
                formula = formula.replace(/#SUBTOTAL#/g, amount);
                try {
                    taxAmount = eval(formula);
                } catch (e) {
                    taxAmount = 0;
                }
            }
            else {
                taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
            }
            taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
            TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
            totDisc = totDisc + taxAmount;
        }
    }
    //---End For Item Disc---

    //    if (isGrid) {
    //        itemAmt = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
    //        itemDisc = parseFloat($("#POD_DISC_AMT_" + slNo).val());
    //    }
    //    else {
    //        itemAmt = parseFloat($("[id$=POD_AMOUNT]").val());
    //        itemDisc = parseFloat($("[id$=POD_DISCOUNT]").val());
    //    }
    //    amount = itemAmt - (isNaN(itemDisc) ? 0 : itemDisc);
    amount = amount - (isNaN(totDisc) ? 0 : totDisc);
    var TaxDetails = $("#divData").data("TaxDetails");
    var curTax = GetTaxDiscountDetails(slNo, 1, true);
    var totTax = 0.0;

    for (var selectedTax in curTax) {
        TaxDetailsObj = JSLINQ(TaxDetails)
            .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == curTax[selectedTax].POT_TAX && tax.POT_NAME == curTax[selectedTax].POT_NAME; })
            .FirstOrDefault(null);
        if (TaxDetailsObj != null) {
            if (TaxDetailsObj.POT_TYPE == "1") {

                formula = TaxDetailsObj.POT_TAX_FORMULA;
                formula = formula.replace(/#SUBTOTAL#/g, amount);
                try {
                    taxAmount = eval(formula);
                } catch (e) {
                    taxAmount = 0;
                }
            }
            else {
                taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
            }
            taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
            TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
            totTax = totTax + taxAmount;
        }
    }
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        if (PurOrderDetails[i].POD_SL_NO == slNo) {
            PurOrderDetails[i].POD_DISC_AMT = parseFloat(totDisc).toFixed(AmtDec);
            PurOrderDetails[i].POD_TAX = parseFloat(totTax).toFixed(AmtDec);

            //  PurOrderDetails[i].POD_AMT_VALUE = (parseFloat(PurOrderDetails[i].POD_AMOUNT) + parseFloat(PurOrderDetails[i].POD_TAX) - parseFloat(PurOrderDetails[i].POD_DISC_AMT)).toFixed(AmtDec);
            PurOrderDetails[i].POD_AMT_VALUE = GetItemSubTotalAmount(PurOrderDetails[i].POD_SL_NO).toFixed(AmtDec);
            PurOrderDetails[i].POD_AMTOUNT = (parseFloat(rate) * parseFloat(PurOrderDetails[i].POD_QTY_REQUESTED).toFixed(AmtDec));
            PurOrderDetails[i].POD_AMOUNT = (parseFloat(rate) * parseFloat(PurOrderDetails[i].POD_QTY_REQUESTED).toFixed(AmtDec));
            //22-05-2104
            // PurOrderDetails[i].POD_RATE = parseFloat(rate).toFixed(AmtDec);
            PurOrderDetails[i].POD_RATE = parseFloat(rate);


        }
    }
    //New 04-11-2013
    $("#divData").data("ReqPODetails", PurOrderDetails);
    GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
    CalculateSubTotalValue();
    $("[id$=hdnSlNo]").val(slNoText);

    //Hdr Discount
    amount = parseFloat($("[id$=POH_SUB_TOTAL]").val());
    var hdrDisc = GetTaxDiscountDetails(0, 3, false);
    for (var selectedHDisc in hdrDisc) {
        TaxDetailsObj = JSLINQ(TaxDetails)
            .Where(function (tax) { return tax.POT_SL_NO == hdrDisc[selectedHDisc].POT_SL_NO && tax.POT_TAX == hdrDisc[selectedHDisc].POT_TAX && tax.POT_NAME == hdrDisc[selectedHDisc].POT_NAME; })
            .FirstOrDefault(null);
        if (TaxDetailsObj != null) {
            if (TaxDetailsObj.POT_TYPE == "1") {
                formula = TaxDetailsObj.POT_TAX_FORMULA;
                formula = formula.replace(/#SUBTOTAL#/g, amount);
                try {
                    taxAmount = eval(formula);
                } catch (e) {
                    taxAmount = 0;
                }
            }
            else {
                taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
            }
            taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
            TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
        }
    }
    $("[id$=POH_DISC_AMT]").val(GetTotalPOCharges(3, amount));
    //Hdr Shipping
    amount = parseFloat($("[id$=POH_SUB_TOTAL]").val());
    var hdrShipping = GetTaxDiscountDetails(0, 2, false);

    for (var selectedHShip in hdrShipping) {
        TaxDetailsObj = JSLINQ(TaxDetails)
            .Where(function (tax) { return tax.POT_SL_NO == hdrShipping[selectedHShip].POT_SL_NO && tax.POT_TAX == hdrShipping[selectedHShip].POT_TAX && tax.POT_NAME == hdrShipping[selectedHShip].POT_NAME; })
            .FirstOrDefault(null);
        if (TaxDetailsObj != null) {
            if (TaxDetailsObj.POT_TYPE == "1") {
                formula = TaxDetailsObj.POT_TAX_FORMULA;
                if (formula == "0") {
                    taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
                }
                else {
                    formula = formula.replace(/#SUBTOTAL#/g, amount);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }
                }
            }
            else {
                taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
            }
            taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
            TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
        }
    }
    $("[id$=POH_SHIP_CHARGE]").val(GetTotalPOCharges(2, amount));
    //Hdr Tax
    if ($("[id$='isTaxAdd']").val() == PurchaseOrderConfig.BothHeaderItem) {
        var hdrTax = GetTaxDiscountDetails(0, 1, false);
        var charges = 0.000;
        for (var selectedHTax in hdrTax) {
            TaxDetailsObj = JSLINQ(TaxDetails)
                .Where(function (tax) { return tax.POT_SL_NO == hdrTax[selectedHTax].POT_SL_NO && tax.POT_TAX == hdrTax[selectedHTax].POT_TAX && tax.POT_NAME == hdrTax[selectedHTax].POT_NAME; })
                .FirstOrDefault(null);
            if (TaxDetailsObj != null) {
                if (TaxDetailsObj.POT_TYPE == "1") {
                    formula = TaxDetailsObj.POT_TAX_FORMULA;

                    //Start
                    var hdrSubTotal = 0;
                    var hdrDisc = 0;
                    var hdrOtherCharge = 0;
                    amount = 0;

                    if (parseInt(TaxDetailsObj.POT_HAS_SUB_TOTAL) == 1) {
                        hdrSubTotal = parseFloat($("[id$=POH_SUB_TOTAL]").val());
                        hdrSubTotal = isNaN(hdrSubTotal) ? 0 : hdrSubTotal;
                    }
                    if (parseInt(TaxDetailsObj.POT_HAS_DISCOUNT) == 1) {
                        hdrDisc = parseFloat($("[id$=POH_DISC_AMT]").val());
                        hdrDisc = isNaN(hdrDisc) ? 0 : hdrDisc;
                    }
                    if (parseInt(TaxDetailsObj.POT_HAS_OTHER_CHARGE) == 1) {
                        hdrOtherCharge = parseFloat($("[id$=POH_SHIP_CHARGE]").val());
                        hdrOtherCharge = isNaN(hdrOtherCharge) ? 0 : hdrOtherCharge;
                    }
                    if (hdrSubTotal == 0)
                        amount = hdrDisc + hdrOtherCharge;
                    else
                        amount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;
                    //End

                    formula = formula.replace(/#SUBTOTAL#/g, amount);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }
                }
                else {
                    taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
                }
                taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
                TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);

                charges += parseFloat(TaxDetailsObj.POT_TAX_AMT);
            }
        }
        $("[id$=POH_ADD_TAX_AMT]").val(parseFloat(charges).toFixed(AmtDec));
    }
    else {
        hdrAmt = parseFloat($("[id$=POH_SUB_TOTAL]").val());
        hdrDisc = parseFloat($("[id$=POH_DISC_AMT]").val());
        // amount = hdrAmt - (isNaN(hdrDisc) ? 0 : hdrDisc);
        //Other Charge Tax Calculation Based On Configuration
        var hdrOtherCharge = parseFloat($("[id$=POH_SHIP_CHARGE]").val());
        var amountTemp;
        if ($("[id$=IsTaxForOtherCharge]").val() == "1") {
            amountTemp = (isNaN(hdrAmt) ? 0 : hdrAmt) - (isNaN(hdrDisc) ? 0 : hdrDisc);
            amount = amountTemp + (isNaN(hdrOtherCharge) ? 0 : hdrOtherCharge);
        }
        else {
            amount = hdrAmt - (isNaN(hdrDisc) ? 0 : hdrDisc);
        }

        // End----
        var hdrTax = GetTaxDiscountDetails(0, 1, false);
        for (var selectedHTax in hdrTax) {
            TaxDetailsObj = JSLINQ(TaxDetails)
                .Where(function (tax) { return tax.POT_SL_NO == hdrTax[selectedHTax].POT_SL_NO && tax.POT_TAX == hdrTax[selectedHTax].POT_TAX && tax.POT_NAME == hdrTax[selectedHTax].POT_NAME; })
                .FirstOrDefault(null);
            if (TaxDetailsObj != null) {
                if (TaxDetailsObj.POT_TYPE == "1") {
                    formula = TaxDetailsObj.POT_TAX_FORMULA;
                    formula = formula.replace(/#SUBTOTAL#/g, amount);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }
                }
                else {
                    taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
                }
                taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
                TaxDetailsObj.POT_TAX_AMT = taxAmount.toFixed(AmtDec);
            }
        }
        $("[id$=POH_ADD_TAX_AMT]").val(GetTotalPOCharges(1, amount));
    }
    CalculateTotal();
    //SaveItemTaxDiscountApply();
}

function AddLineItemDiscountDtl(slNo, amount) {
    //<summary>function used to add the item tax details</summary>
    if ($("[id$=isEditMode]").val() == "1") {
        $("[id$=hdnSlNo]").val(slNo);
        amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
    }
    RemoveAllValidations();
    FillTaxDiscount(3);

    $("#divTaxApplicableAmount").hide(); //Hide/Show Tax Applicable Amount div

    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(DiscountDetails)"
        });
    PurchaseOrderConfig.ItemTaxPK = slNo;
    $("[id$=ItemAmount]").val(amount);
    $("[id$=IsLine]").val(1);
    $("[id$=IsLineDiscount]").val(3);
    ClearPopUp(slNo, 3, true);
    ClearTaxDetails();
    //Clone TaxDetails Data to TempDetails   
    var TaxDetails = $("#divData").data("TaxDetails");     //
    $("#divData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
    //End
}
//Direct PO
function DirectPO() {
    FillVendors(0);
    FillIssuingDepartment(0);
    $("#divVendor").dialog("open");
    $("#divVendor").dialog(
        {
            width: 490,
            title: "Translate(VendorList)"
        });
    return false;
}

function RevisionHistory() {
    FillRevisionHistory();

    return false;
}
function FillRevisionHistory() {
    GrandGrid.MakeGrid($("#grdRevisionHistory"), 0, new Array());
    $.post(PurchaseOrderConfig.GetRevisionHistoryURL + $("[id$=PurchaseOrderID]").val(), function (data) {
        if (data != null && data.length > 0) {
            GrandGrid.MakeGrid($("#grdRevisionHistory"), 0, data);
            $("#divRevisionHistory").dialog("open");
            $("#divRevisionHistory").dialog(
                {
                    width: 420,
                    title: "Translate(RevisionHistory)"
                });
        }
        else {
            //GrandGrid.MakeGrid($("#grdRevisionHistory"), 0, new Array());
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoHistory, PurchaseOrderConfig.Information);
        }

    });
}

function FillVendors(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=ddlVendorList]").attr("id");
    $.get(PurchaseOrderConfig.URLGetVendorsOnly + PurchaseOrderConfig.BizUnitPk, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, selectVal);
    });
}
function ContinueDirectPO() {
    //<summary>function used to continue with the selected vendors </summary>
    $("#updateProgress").show();
    SetInitialPortList();
    ClearAllSelectedItem();
    $("select[id$=POH_COMPANY]").val($("[id$=hdfCompany]").val());
    var vendorID = 0;
    if ($("[id$=hdfIsPostbackDirectPO]").val() == "1") {
        vendorID = $("[id$=hdfSelectedVendorPK]").val();
        $("[id$=lblRequestedDept]").html($("[id$=hdfIssueDeptText]").val());
    }
    else {
        vendorID = $("[id$=ddlVendorList]").val();
    }
    if (vendorID > 0) {
        PurchaseOrderConfig.VendorPK = vendorID;
        IsDirectPO = true;
        FillVendorMaterials(PurchaseOrderConfig.VendorPK);
        FillPRHType($("[id$=hdfPOCategoryVal]").val(), true);
    }
    else
        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoVendors, PurchaseOrderConfig.Information);
    return false;

}


function ValidateDiscountTotal(slNo) {
    var isValid = true;
    var TaxDetails = $("#divData").data("TaxDetails");
    var totDisc = 0.0;
    var amount = 0.0;
    if (slNo > 0) {
        var curTax = GetTaxDiscountDetails(slNo, 3, true);
        totDisc = 0.0;
        amount = parseFloat($("#POD_H_AMOUNT_" + slNo).val());
        for (var selectedTax in curTax) {
            TaxDetailsObj = JSLINQ(TaxDetails)
                .Where(function (tax) { return tax.POT_SL_NO == slNo && tax.POT_TAX == curTax[selectedTax].POT_TAX && tax.POT_NAME == curTax[selectedTax].POT_NAME; })
                .FirstOrDefault(null);
            if (TaxDetailsObj != null) {
                if (TaxDetailsObj.POT_TYPE == "1") {
                    formula = TaxDetailsObj.POT_TAX_FORMULA;
                    formula = formula.replace(/#SUBTOTAL#/g, amount);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }
                }
                else {
                    taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
                }
                taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
                totDisc = parseFloat(totDisc) + parseFloat(taxAmount);
            }
        }

        var currType = parseInt($("[id$=ChooseTax]").val());
        if (currType == 1) {
            formula = PurchaseOrderConfig.TaxFormula;
            formula = formula.replace(/#SUBTOTAL#/g, amount);
            try {
                taxAmount = eval(formula);
            } catch (e) {
                taxAmount = 0;
            }
        }
        else {
            taxAmount = parseFloat($("[id$=TaxAmount]").val());
        }
        taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
        totDisc = parseFloat(totDisc) + parseFloat(taxAmount);

        if (amount < parseFloat(totDisc)) {
            isValid = false;
        }
    }
    else {
        amount = parseFloat($("[id$=POH_SUB_TOTAL]").val());
        var hdrDisc = GetTaxDiscountDetails(0, 3, false);
        for (var selectedHDisc in hdrDisc) {
            TaxDetailsObj = JSLINQ(TaxDetails)
                .Where(function (tax) { return tax.POT_SL_NO == hdrDisc[selectedHDisc].POT_SL_NO && tax.POT_TAX == hdrDisc[selectedHDisc].POT_TAX && tax.POT_NAME == hdrDisc[selectedHDisc].POT_NAME; })
                .FirstOrDefault(null);
            if (TaxDetailsObj != null) {
                if (TaxDetailsObj.POT_TYPE == "1") {
                    formula = TaxDetailsObj.POT_TAX_FORMULA;
                    formula = formula.replace(/#SUBTOTAL#/g, amount);
                    try {
                        taxAmount = eval(formula);
                    } catch (e) {
                        taxAmount = 0;
                    }
                }
                else {
                    taxAmount = parseFloat(TaxDetailsObj.POT_TAX_AMT);
                }
                taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
                totDisc = parseFloat(totDisc) + parseFloat(taxAmount);
            }
        }
        var currType = parseInt($("[id$=ChooseTax]").val());
        if (currType == 1) {
            formula = PurchaseOrderConfig.TaxFormula;
            formula = formula.replace(/#SUBTOTAL#/g, amount);
            try {
                taxAmount = eval(formula);
            } catch (e) {
                taxAmount = 0;
            }
        }
        else {
            taxAmount = parseFloat($("[id$=TaxAmount]").val());
        }
        taxAmount = isNaN(taxAmount) ? 0 : taxAmount;
        totDisc = parseFloat(totDisc) + parseFloat(taxAmount);

        if (amount < parseFloat(totDisc)) {
            isValid = false;
        }
    }








    return isValid;
}

////Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//     return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
// }

function ViewSelectedComment(slNo) {
    ///<summary>function used to get item's purchase request</summary>

    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        if (PurOrderDetails[i].POD_SL_NO == slNo) {
            $("[id$=txtPopupComment]").val(PurOrderDetails[i].POD_REMARKS);
            $("[id$=hdfPopupCommentSlNo]").val(slNo);
            break;
        }
    }

    $("#divCommentPopup").dialog("open");
    $("#divCommentPopup").dialog({ "width": 328 });
}

function getShortString(text, length) {
    var result = '';
    if (text.length > length) {
        result = text.substring(0, length) + '..';
    }
    else {
        result = text;
    }
    return result;
}

function savePopupComment() {
    var comment = $("[id$=txtPopupComment]").val();
    var slNo = $("[id$=hdfPopupCommentSlNo]").val();
    var PurOrderDetails = $("#divData").data("ReqPODetails");
    for (var i in PurOrderDetails) {
        if (PurOrderDetails[i].POD_SL_NO == slNo) {
            PurOrderDetails[i].POD_REMARKS = comment;
            break;
        }
    }
    $("#divData").data("ReqPODetails", PurOrderDetails);
    GrandGrid.MakeGrid($("#grdPODetails"), 0, PurOrderDetails);
    $("#divCommentPopup").dialog("close");
    return false;
}

function Print_PR(prID) {
    var url = PurchaseOrderConfig.PURCHASEREQUESTREPORTURL + "?ID=" + prID + "&APPTYPE=PR";
    OpenPDF(url);
}
function OpenPRNewWindow(prID, prhDept) {
    window.open(PurchaseOrderConfig.PURCHASERQSTENTRYURL + "?PK=" + prID + "&PrhDept=" + prhDept + "&Status=1&IsFromPO=1&KeepCurDep=1", '_blank');
}

function ChangeWorkFlow() {

    if ($("#divVendors").find("[name=rdoVendors]:checked").length > 0 || $("[id$=VEN_PK]").val() > 0) {
        SetInitialPortList();
        if ($("[id$=hdfIsWkfSettingPostBackReqd]").val() == "1" || $("[id$=hdfPOCatWkfChange]").val() == "1") {
            $("[id$=hdfIsReqDeptPostback]").val("1");
            $("[id$=hdfStoreBeforePostback]").val($("[id$=Store]").val());
            $("[id$=PurchaseOrderList]").val($("[id$=PurchaseOrderListPostback]").val());
            var PurOrderlistBeforePostback = $("#divData").data("ReqPOList");
            $("[id$=hdfReqPOList]").val(JSON.stringify(PurOrderlistBeforePostback));
            if ($("#divVendors").find("[name=rdoVendors]:checked").length > 0) {
                $("[id$=hdfSelectedVendorPK]").val($("#divVendors").find("[name=rdoVendors]:checked").val());
            }

            $("[id$=btnChangeWorkFlow]").click();
        }
        else {
            POContinue();
        }

        return false;
    }
    else {
        GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoVendors, PurchaseOrderConfig.Information);
        return false;
    }


}

function RestorePageAfterPostBack(selectVal) {

    if ($("[id$=hdfIsReqDeptPostback]").val() == "1") {
        var drpID = $("select[id$=Store]").attr("id");
        var storeURL = "";
        storeURL = PurchaseOrderConfig.FillStoreDropdownURL + PurchaseOrderConfig.BizUnitPk + "&UserPK=" + $("[id$=UserPk]").val() + "&ProcID=" + $("[id$=hdfProcessID]").val() + "&FLD_NAME=DPT_PK";
        $.get(storeURL, function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal, true);
            SetSearchType();

            //BindPendingPRGrid();
            var PurOrderList = $.parseJSON($("[id$=hdfReqPOList]").val());
            $("#divData").data("ReqPOList", PurOrderList);
            PurchaseOrderRequestSummary();
            $("#divPoListing").show();
            if (PurOrderList != null && PurOrderList.length > 0) {
                $("#VendorSelection").show();
            }
            else {
                $("#VendorSelection").hide();
            }
            ActiveSearch();
            $("[id$=lblRequestedDept]").html($("[id$=hdfIssueDeptText]").val());
            if ($("[id$=hdfInvestorCode]").val() != "null") {
                $("[id$=POH_INVESTOR_CODE]").val($("[id$=hdfInvestorCode]").val());
            }
            else {
                $("[id$=POH_INVESTOR_CODE]").val("");
            }
        });
    }
}

function IsSameRequestedDept() {
    var flag = true;
    var issueDeptAdded = 0;
    var issueDeptAddedText = "";
    $("[id$=POH_ISSUE_DEPT]").val("0");
    var PurOrderList = $("#divData").data("ReqPOList");
    for (var i in PurOrderList) {
        issueDeptAdded = PurOrderList[i].PRH_ISSUE_DEPT;
        issueDeptAddedText = PurOrderList[i].PRH_ISSUE_DEPT_TEXT;
        break;
    }
    $("#grdPendingPRList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            if (issueDeptAdded == 0) {
                issueDeptAdded = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT, grdID);
                issueDeptAddedText = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT_TEXT, grdID);
            }
            else {
                if (issueDeptAdded != GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_ISSUE_DEPT, grdID)) {
                    flag = false;
                    return flag;
                }
            }
        }
    });

    return flag;
}
// #region Direct PO
function FillIssuingDepartment(PK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=ddlIssuingDept]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(PurchaseOrderConfig.DepartmentsDDL + PK + "&ProcessID=" + $("[id$=hdfProcessID]").val() + "&RefID=" + $("[id$=hdfRefID]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, PK);
    });
}

function ChangeWorkFlowDirectPO() {
    if ($("[id$=hdfPOCatWkfChange]").val() == "1") {
        AddValidations(5);
        if ($(document.forms[0]).valid()) {
            $("[id$=hdfIsReqDeptPostback]").val("1");
            $("[id$=hdfIsPostbackDirectPO]").val("1");
            $("[id$=PurchaseOrderList]").val($("[id$=PurchaseOrderListPostback]").val());
            $("[id$=hdfSelectedVendorPK]").val($("[id$=ddlVendorList]").val());
            $("[id$=POH_ISSUE_DEPT]").val($("[id$=ddlIssuingDept").val());
            $("[id$=hdfPOCategoryVal]").val($("[id$=ddlPOhCategory").val());
            $("[id$=POH_PO_CATEGORY]").val($("[id$=hdfPOCategoryVal").val());
            $("[id$=hdfIssueDeptText]").val($("[id$=ddlIssuingDept] :selected").text());
            $("[id$=btnWorkFlowDirectPO]").click();
            return false;
        }
        return false;
    }

    if ($("[id$=hdfIsWkfSettingPostBackReqd]").val() == "1") {
        if ($("[id$=ddlIssuingDept").val() > 0) {

            $("[id$=hdfIsReqDeptPostback]").val("1");
            $("[id$=hdfIsPostbackDirectPO]").val("1");
            $("[id$=PurchaseOrderList]").val($("[id$=PurchaseOrderListPostback]").val());
            $("[id$=hdfSelectedVendorPK]").val($("[id$=ddlVendorList]").val());
            $("[id$=POH_ISSUE_DEPT]").val($("[id$=ddlIssuingDept").val());

            $("[id$=hdfPOCategoryVal]").val($("[id$=POH_PO_CATEGORY").val());

            $("[id$=hdfIssueDeptText]").val($("[id$=ddlIssuingDept] :selected").text());
            $("[id$=btnWorkFlowDirectPO]").click();

        }
        else {
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoRequestedDeptSelected, PurchaseOrderConfig.Information);
        }
    }
    else {
        ContinueDirectPO();
    }
    return false;
}
//#endregion DirectPO

//<summary>function to set default  shipping and billing Dept (In case of MULTIPLE PLANT) </summary>
function GetSetDefaultShippingBillingDept() {
    $.get(PurchaseOrderConfig.GetDefaultShippingBillingDeptURL + $("[id$=hdfDeptID]").val(), function (data) {
        if (data.Table.length > 0) {
            FillDepartment(data.Table[0].SHIP_STORE_IS_DEFAULT, PurchaseOrderConfig.Shipping);
            FillDepartment(data.Table[0].BILL_STORE_IS_DEFAULT, PurchaseOrderConfig.Billing);
        }
    });
}


function IsSameInvestor() {
    var flag = true;
    var PRInvestorCode = "";
    var PurOrderList = $("#divData").data("ReqPOList");
    if (PurOrderList != null && PurOrderList.length > 0) {
        PRInvestorCode = PurOrderList[0].PRH_INVESTOR_CODE;
    }

    $("#grdPendingPRList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            if (PRInvestorCode == "") {
                PRInvestorCode = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_INVESTOR_CODE, grdID);
            }
            else {
                if (PRInvestorCode != GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_INVESTOR_CODE, grdID)) {
                    flag = false;
                    return flag;
                }
            }
        }
    });
    return flag;
}




function IsSamePRtype() {
    var flag = true;
    var PRTypeAdded = -1;
    var PurOrderList = $("#divData").data("ReqPOList");
    for (var i in PurOrderList) {
        PRTypeAdded = PurOrderList[i].PRH_TYPE;
        break;
    }
    $("#grdPendingPRList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            if (PRTypeAdded == -1) {
                PRTypeAdded = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_TYPE, grdID);
            }
            else {
                if (PRTypeAdded != GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_TYPE, grdID)) {
                    flag = false;
                    return flag;
                }
            }
        }
    });

    return flag;
}

function IsSamePRGroup() {
    var flag = true;
    var PRGroupAdded = -1;
    var PurOrderList = $("#divData").data("ReqPOList");
    if ($("[id$=POH_PK]").val() == '0') {
        for (var i in PurOrderList) {
            PRGroupAdded = PurOrderList[i].PRH_GROUP;
            break;
        }
    }
    else {
        PRGroupAdded = $("[id$=POH_GROUP]").val();
    }
    $("#grdPendingPRList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            if (PRGroupAdded == -1) {
                PRGroupAdded = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_GROUP, grdID);
            }
            else {
                if (PRGroupAdded != GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_GROUP, grdID)) {
                    flag = false;
                    return flag;
                }
            }
        }
    });

    return flag;
}

function IsSameGloveGroup() {
    var flag = true;
    var IsGloveGroup = -1;
    var PurOrderList = $("#divData").data("ReqPOList");
    if ($("[id$=POH_PK]").val() == '0') {
        for (var i in PurOrderList) {
            IsGloveGroup = PurOrderList[i].PRH_IS_GLOVE;
            $("[id$=POH_IS_GLOVE]").val(PurOrderList[i].PRH_IS_GLOVE);
            break;
        }
    }
    else {
        IsGloveGroup = $("[id$=POH_IS_GLOVE]").val();
    }
    $("#grdPendingPRList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            if (GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_IS_GLOVE, grdID) == "1") {
                $("[id$=POH_IS_GLOVE]").val(GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_IS_GLOVE, grdID));
            }
            if (IsGloveGroup == -1) {
                IsGloveGroup = GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_IS_GLOVE, grdID);
            }
            else {
                if (IsGloveGroup != GrandGrid.Utilities.GetColumnValue($(this), PurchaseOrderConfig.PRH_IS_GLOVE, grdID)) {
                    flag = false;
                    return flag;
                }
            }
        }
    });

    return flag;
}


//<summary>function to set ItemAmount Textbox of TaxDetails Popup </summary>
function SetTaxApplicableAmount() {

    var hdrSubTotal = 0;
    var hdrDisc = 0;
    var hdrOtherCharge = 0;
    var amount = 0;

    if ($("[id$=chkSubTotal]").is(":checked")) {
        hdrSubTotal = parseFloat($("[id$=POH_SUB_TOTAL]").val());
        hdrSubTotal = isNaN(hdrSubTotal) ? 0 : hdrSubTotal;
    }
    if ($("[id$=chkDiscount]").is(":checked")) {
        hdrDisc = parseFloat($("[id$=POH_DISC_AMT]").val());
        hdrDisc = isNaN(hdrDisc) ? 0 : hdrDisc;
    }
    if ($("[id$=chkOtherCharges]").is(":checked")) {
        hdrOtherCharge = parseFloat($("[id$=POH_SHIP_CHARGE]").val());
        hdrOtherCharge = isNaN(hdrOtherCharge) ? 0 : hdrOtherCharge;
    }
    if (hdrSubTotal == 0)
        amount = hdrDisc + hdrOtherCharge;
    else
        amount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;

    $("[id$=ItemAmount]").val(amount.toFixed(AmtDec));
    GetFormula(parseInt($("[id$=ChooseTax]").val()));
    return amount;
}
function ResetTaxApplicableCheckbox() {
    $("[id$=chkSubTotal]").attr('checked', false);
    $("[id$=chkDiscount]").attr('checked', false);
    $("[id$=chkOtherCharges]").attr('checked', true);
}

function AvoidSpecialChar(e) {
    ///<summary>
    ///Not allowing special charecters in textbox
    ///</summary>
    var keyCode = e.keyCode ? e.keyCode : e.which;
    //96 for number 0  97 - 122(a - z), // 65 - 90(A - Z), // 48 - 57(0 - 9) 
    if (!((keyCode >= 65) && (keyCode <= 90) || (keyCode >= 96 && keyCode <= 122) || (keyCode >= 48) && (keyCode <= 57))) {
        if (!(keyCode == 46 || keyCode == 8 || keyCode == 9 || keyCode == 32 || keyCode == 35 || keyCode == 36 || keyCode == 37 || keyCode == 39 || keyCode == 173 || keyCode == 191)) //46=>Delete;8=>Backspace;9=>Tab;36=>Home;35=>End;37=>Left;39=>Right;32=>SpaceBar;191=>'/',173=>'-'
            return false;
    }
}

//<summary>
//Disable special characters (<,>) from paste in a textbox
//</summary>
$("[id*=SearchValue]").live("paste", function () {
    setTimeout(function () {
        //get the value of the input text
        var data = $("[id*=SearchValue]").val();
        //replace the special characters to '' 
        //         var dataFull = data.replace(/[^\w\s\d]/gi, '');
        var dataFull;
        dataFull = data.replace('<', '');
        dataFull = dataFull.replace('>', '');
        //set the new value of the input text without special characters
        $("[id*=SearchValue]").val(dataFull);
    });

});

function ShowBudgetSummary() {
    ajaxurl = PurchaseOrderConfig.GetItemRates + "&ItemPK=112";
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(ajaxurl, function (data) {
        if (data!=null && data.length > 0) {
            GrandGrid.MakeGrid($("#grdBudgetDetails"), 0, data);
            $("#divBudgetHistory").dialog("open");
            $("#divBudgetHistory").dialog(
                {
                    width: 700,
                    height: 400,
                    title: "Translate(BudgetSummary)"
                });
        }
        else {
            GrandGrid.MakeGrid($("#grdBudgetDetails"), 0, new Array());
            GrandScriptUtils.ShowModal(PurchaseOrderConfig.NoHistory, PurchaseOrderConfig.Information);
            return false;
        }
    });
}

