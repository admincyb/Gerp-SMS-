/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />


//#region ---------- Configuration Section-------
var trDetails;
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Material";
var UPLOADFOLDERPR = "Purchase";
//Biju
//$.xhrPool = [];

//$(function () {
//    $.ajaxSetup({
//    //default async is true
//        async:true,
//        beforeSend: function (jqXHR, settings) {
//            if ($.xhrPool.length === 0) {
//                // Show overlay 
//                //alert('Request count' + $.xhrPool.length);   
//                $('#updateProgress').show();
//            }
//            $.xhrPool.push(jqXHR);
//        },
//        complete: function (jqXHR) {
//            if ($.xhrPool.length > -1) {
//                $.xhrPool.pop(jqXHR);
//            }
//            if ($.xhrPool.length === 0) {
//                //hide overlay
//                // alert('Request count' + $.xhrPool.length);
//                $('#updateProgress').hide();
//            }
//        }
//    });
//});
//==========================================================


var PurchaseRequestConfig = {
    PurchaseRequestBindGridURL: "PurchaseRequest.do?Action=GetPurchaseRequestTrading",
    WorkflowStatus: "CommonManagement.do?Action=GetWorkflowStatus&RefID=",
    DepartmentsDDL: "CommonManagement.do?Action=DepartmentsDDL&DeptPk=",
    SubDepartmentsDDL: "CommonManagement.do?Action=SubDepartmentsDDL&DeptPk=",
    PackingMaterialBindGridURL: "PurchaseRequest.do?Action=PackingMaterials",
    PackingMaterialDtlURL: "MaterialManagement.do?Action=GetPakingMaterialDetails&SBUPk=",
    FillPackingType: "CommonManagement.do?Action=GetPackingType&BizUnit=",
    SavePurchaseRequest: "PurchaseRequest.do?Action=SavePurchaseRequestTradingList",
    MaterialCategroyURL: "MaterialCategory.do?Action=GetMaterialCategoryListExcepetFGAuto",
    MaterialCategroyDeptURL: "MaterialCategory.do?Action=GetMaterialCategoryListDeptAuto&AUTOSEARCH=1&DeptPK=",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&AUTOSEARCH=1",
    UomURL: "MaterialManagement.do?Action=GetItemUOMTrading&MaterialPK=",
    FillStoreDropdownURL: "SubDepartmentManagement.do?Action=GetInventoryStores&SBUPk=",
    FillIODropdownURL: "PurchaseRequest.do?Action=GetIONumberAuto&BizUnit=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",

    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    GetMaterialDescription: "MaterialManagement.do?Action=GetMaterialDescription&MaterialID=",
    //Show the breakup of available qty across all stores
    GetBreakupQtyStore: "MaterialManagement.do?Action=GetBreakUpForStoreCurrentStock&MaterialID=",
    GetPendingPODetails: "POGeneration.do?Action=GetPendingPODetails&MaterialID=",

    GetProcessID: "CommonManagement.do?Action=GetProcessID&DepID=",
    PURCHASEREQUESTURL: "/PurchaseRequestManagement/PurchaseRequestTrading.aspx",
    GetConcversionFactors: "CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    PURCHASEREQUESTREPORTURL: "../Reports/GenerateReport.aspx",
    BACKURL: "PurchaseRequestListing.aspx",
    // messages
    Confirmation: "Translate(Confirmation)",
    RecordExist: "Translate(AlreadyExists)",
    NoDataFound: "Translate(NoDataFound)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    SaveMessage1: "Translate(PurchaseDetailsSaved1)",
    SaveMessage2: "Translate(PurchaseDetailsSaved2)",
    SaveMessage3: "Translate(PurchaseDetailsSubmit2)",
    SubmitMessage: "Translate(SubmittedMsg)",
    PurchaseRequestDetails: "Translate(AddPurchaseRequestDetails)",
    ItemAlreadyAddedMsg: "Translate(Itemsalreadyaddedbyanotheruser)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    EnterRequiredDate: "Translate(EnterRequiredDate)",
    EnterDate: "Translate(EnterDate)",
    ChangeReqDatetitle: "Translate(ChangeRequestDate)",
    UOMConversion: "Translate(UOMConversion)",
    WorkflowSubmit: "Translate(WorkflowSubmit)",
    SelectAnotherUOM: "Translate(SelectAnotherMaterial)",
    ConfirmationMsg: "Translate(Confirmation)",
    ItemAlreadyDeletedMsg: "Translate(Itemsalreadydeletedbyanotheruser)",
    MSLNotReached: "Translate(MSLNotReached)",
    ClearREQ: "Translate(ClearREQ)",
    SelectRequestItem: "Translate(SelectRequestItem)",
    RequestQtyGreater: "Translate(RequestQtyGreater)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    SelectValidItem: "Translate(SelectValidItem)",
    SameItemDateMsg: "Translate(ItemalreadyAddedWntContinue)",
    EnterPurpose: "Translate(EnterRequestPorpose)",

    // declaration
    PurchaseRequestList: new Array(),
    PurchaseRequestObj: new Object(),
    ExistingItems: new Array(),
    ExistingItems2: new Array(),
    BizUnitPk: 0,
    PRDITEM: 0,
    IsViewMode: false,
    IsModifyPR: false,
    IsFromPO: false,
    SlNo: 0,

    PRDREQDDATE: "",
    DeletePk: 0,
    DeleteDate: "",
    tdset: "",
    EDIT: "edit",
    DELETE: "delete",
    SAVE: "Save",
    Submit: "Submit",
    ADD: "Add",
    ADDMANY: "AddMany",
    ADDITEM: "AddItem",
    CLEARITEM: "ClearItem",
    INBOX: "Inbox",
    AddRow: null,

    // fields
    PRD_ITEM: "PRD_ITEM",
    PRH_ISSUE_DEPT: "PRH_ISSUE_DEPT",
    PRH_ISSUE_SUB_DEPT: "PRH_ISSUE_SUB_DEPT",
    PRH_USER: "PRH_USER",
    ITM_CODE: "ITM_CODE", // now its not using may be needed
    ITM_TEXT: "ITM_TEXT",
    PRD_UOM: "PRD_UOM",
    ITM_CATEGORY: "ITM_CATEGORY",
    UOM: "UOM",
    ITM_PK: "ITM_PK",
    ITM_UOM: "ITM_UOM",
    PURCHASE_UOM: "PURCHASE_UOM",
    SALES_UOM: "SALES_UOM",
    ITC_NAME: "ITC_NAME",
    UOM_CODE: "UOM_CODE",
    REQUEST_QTY: "REQUEST_QTY",
    REQUEST_SPEC: "REQUEST_SPEC",
    PRD_QTY_REQUESTED: "PRD_QTY_REQUESTED",
    PRD_QTY_ORDERED: "PRD_QTY_ORDERED",
    PRD_REQD_DATE: "PRD_REQD_DATE",
    PRD_ITEM_SPEC: "PRD_ITEM_SPEC",
    PRD_PURPOSE: "PRD_PURPOSE",
    REORDER_LEVEL: "ITM_ROL_STK",
    MINIMUM_STOCK: "ITM_MIN_STK",
    CURRENT_STOCK: "STH_QTY_IN_STOCK_TO_UOM",
    MAX_STOCK: "ITM_MAX_STK",
    PRE_REQUEST_QTY: "PRD_QTY_APPROVED",
    SOD_LOT_NO: "SOD_LOT_NO",
    ITM_DESC: "ITM_DESC",
    ITM_IS_PM: "ITM_IS_PM",
    // url
    PurchaseListUrl: "PurchaseRequestTradingList.aspx",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx"
};
///#endregion

var QtyDec, AmtDec, RateDec;
var storeLoaded = false;
var SelRequiredDate;
var SelPurpose;

///#region----------- Initialization Section ----------------

$(document).ready(function () {
    //ajaxBehav();
    storeLoaded = false;
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimalDigitP2P']").val();
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "Translate(AutoDefaultValue)");
    }, "Translate(Pleaseselectanoption)");
    PageInit();

});



function PageInit() {
    ///<summary>initial page condition</summary>
    $("[id$=btnAddSelectedItems]").hide();
    $("#imgReOrderHideHeader").hide();
    HideReOrder();
    var IsWkfSetting = $("[id$=hdfIsWkfSettingPostBackReqd]").val();
    if (parseInt(IsWkfSetting) == 1) {
        $("[id$=divSubDept]").show();
    }
    else {
        $("[id$=divSubDept]").hide();
    }
    if ($("[id$=PurchaseRequestList]").val() == "null") {
        GrandScriptUtils.ShowModal(PurchaseRequestConfig.ItemAlreadyDeletedMsg, PurchaseRequestConfig.InformationTtile, PurchaseRequestConfig.SAVE);
    }
    else {
        $("[id$=hdfIsPacking]").val("0")
        var queryString = window.location.search.substring(1);
        if (queryString != "") {
            var queryStr = queryString.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                    PurchaseRequestConfig.IsViewMode = true;
                }
                else if (pK[1] == 1 && pK[0] == "IsModify") {
                    PurchaseRequestConfig.IsModifyPR = true;
                    $("[id$=PRH_IS_EDIT]").val("1");
                }
                else if (pK[1] == 1 && pK[0] == "IsFromPO") {
                    PurchaseRequestConfig.IsFromPO = true;
                }
            }
        }
        PurchaseRequestConfig.BizUnitPk = $("[id$=BizUnitPk]").val();
        var purchaseReqObj = $.parseJSON($("[id$=PurchaseRequestList]").val());
        PurchaseRequestConfig.PurchaseRequestList = purchaseReqObj.PurchaseRequestList;
        Popup();
        CheckConfigForShowDescPM(); //Config for checking Description of Packing Material is showing/ not in Spec field by default.
        CheckConfigForShowBothLotnoDescPM(); // On choosing an item from SC.,by default the Specification field shows lotno# of the item .Along with that also show the item description from packing material master
        SetAllConfigurations();
        if ($("[id$=hdfIsDisableIO]").val() == "1") {
            //$("[id$=PRH_SO_HDR]").attr("disabled", "disabled");
            DisableAuto($("[id$=txtSO]"), $("[id$=PRH_SO_HDR]"));
        }
        if ($("[id$=hdfIsPRPMAttachmentShow]").val() == "0") {
            $("[id$=divFileAttachmentPR]").show();
        }
        if ($("[id$=hdfIsIoNumberHide]").val() == "0") {
            $("[id$=divIONumber]").hide();
        }
        DateHeaderInit();
        DateDtlsInit();
        FillPackingType(0);


        if ((purchaseReqObj.PRH_PK != undefined && purchaseReqObj.PRH_PK > 0) || ((purchaseReqObj.PRH_PK != undefined && purchaseReqObj.PRH_PK > 0) || $("[id$=hdfIsReqDeptPostback]").val() == "1")) {
            if (purchaseReqObj.PRH_SO_HDR != undefined && purchaseReqObj.PRH_SO_HDR > 0) {
                FillIONumber(purchaseReqObj.PRH_SO_HDR);
                $("[id$=hdfIsPacking]").val("1");
            }
            else
                FillIONumber(0);
            FillPurchaseRequestDetails(purchaseReqObj);
        }
        else {
            FillCompany(0);
            FillIssuingDepartment(0);
            FillIssuingSubDepartment(0);
            FillIONumber(0);
            GrandScriptUtils.MakeFileUploader("fupUploaderPR", true, "divFileData", "FILELIST", "PR", true);
        }
        if (purchaseReqObj.Store == undefined)
            FillStore($("[id$=hdfDeptID]").val());
        else
            FillStore(purchaseReqObj.Store);
        //        if (purchaseReqObj.PRH_SO_HDR != undefined && purchaseReqObj.PRH_SO_HDR > 0) {
        //            FillIONumber(purchaseReqObj.PRH_SO_HDR);
        //            $("[id$=hdfIsPacking]").val("1");
        //        }
        //        else
        //            FillIONumber(0);

        FillMaterialCategoryAutoComplete();
        FillCategoryMaterials(0);
        if (!$.isArray(PurchaseRequestConfig.PurchaseRequestList)) {
            PurchaseRequestConfig.PurchaseRequestObj = PurchaseRequestConfig.PurchaseRequestList;
            PurchaseRequestConfig.PurchaseRequestList = new Array();
            PurchaseRequestConfig.PurchaseRequestList.push(PurchaseRequestConfig.PurchaseRequestObj);
        }
        $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
        GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
        //$("select[id$=Store]").focus();
        $("[id$=PRH_NO]").focus();
        //   HideItemRequested();
        //    ShowItemRequested();
        $("[id$=PRH_PERCENTAGE_EXTRA]").val($("[id$=hdfThreshold]").val());
        ChangeROLMSCat();
        if (purchaseReqObj.PRH_PK == undefined || purchaseReqObj.PRH_PK == 0) {
            $("[id$=btnPrint]").hide();
        }
        if (PurchaseRequestConfig.IsFromPO == 1) {
            $("[id$=btnSubmit]").hide();
            $("[id$=btnSave").hide();
            $("[id$=btnClose").hide();
        }
    }
}

///#endregion

//#region----------- Validation Section----------------

function AddValidations(mode) {
    //<summary>Function used to assign validation</summary>

    RemoveValidations();
    if (mode == 1) {
        $("[id$=Store]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseselectaStore)" }
        });
        $("[id$=PRH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(EnterDate)" }
        });
    }
    if (mode == 2) {
        $("[id$=MaterialCategory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItemCategory)" }
        });
        $("[id$=ItemCodeMaterial]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItemCode)" }
        });
        $("[id$=MaterialUOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectUOM)" }
        });
        $("[id$=PurchaseQty]").rules("add", {
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            required: true,
            messages: { required: "Translate(EnterQty)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });
        $("[id$=RequiredDate]").rules("add", {
            date: true,
            messages: { date: "Translate(EnterRequiredDate)" }
        });
        if (parseInt($("[id$=hdfValidatePurpose]").val()) == 1) {
            $("[id$=RequestPorpose]").rules("add", {
                required: true,
                messages: { required: "Translate(EnterRequestPorpose)" }
            });
        }
    }
    if (mode == 3) {
        $("input[id$=RequiredDateNew]").rules("add", {
            date: true,
            required: true,
            messages: { required: PurchaseRequestConfig.EnterDate }
        });
    }
    if (mode == 4) {
        $("[id$=ItemQty]").rules("add", {
            //            ThreeDecimal: true,
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            required: true,
            messages: { required: "Translate(EnterQty)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });
        $("[id$=SelectItemUOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectUOM)" }
        });
    }
    if (mode == 5) {
        $("[id$=PRH_PERCENTAGE_EXTRA]").rules("add", {
            //            DecimalDigits: QtyDec,
            //            CustomDecimal: true,
            //            required: false,
            ZeroDecimal: true,
            messages: { required: "Translate(EnterExtraPer)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });

    }
    if (mode == 6) {
        $("[id$=PRH_ISSUE_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseselectaReqDept)" }
        });
        $("[id$=Store]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseselectaStore)" }
        });
        $("[id$=PRH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(EnterDate)" }
        });
        if (parseInt($("[id$=hdfRequestedBy]").val()) == 1)
            $("[id$=PRH_USER]").rules("add", {
                required: true,
                messages: { required: "Translate(PleaseselectaRequestedBy)" }
            });
        if (parseInt($("[id$=hdfRequestbyDept]").val()) == 1)
            $("[id$=PRH_ISSUE_SUB_DEPT]").rules("add", {
                selectNone: true,
                messages: { selectNone: "Translate(PleaseselectaRequestbyDept)" }
            });
    }
}


function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=PRH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = PurchaseRequestConfig.FillCompanyDropdownURL + PurchaseRequestConfig.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val() + "&ApsPK=" + selectVal;
    }
    else {
        getURL = PurchaseRequestConfig.FillCompanyDropdownURL + PurchaseRequestConfig.BizUnitPk + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        if (selectVal == undefined || selectVal == 0) {
            var selCompany = $("[id$=hdfSelCompany]").val();
            $("#PRH_COMPANY").val(selCompany);
        }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

//#endregion

//#region----------- Core Section----------------

function DateHeaderInit() {
    //<summary>Function Used to make header datetime picker</summary>

    GrandScriptUtils.DatePicker("PRH_DATE", false, false);
}

function DateDtlsInit() {
    //<summary>Function Used to make details datetime picker</summary>

    GrandScriptUtils.DatePicker("RequiredDate", false, false);
}

function Popup() {
    ///<summary>Function used for popup</summary>

    $("#divMetarialDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#divPackingMaterialDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

    $("#ItemAlreadyAddedPopUp").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#divItemDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });


    //Breakup of stores
    $("#divBreakupStore").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

}

function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>

    GrandScriptUtils.MakeAutoComplete("MaterialCategory", PurchaseRequestConfig.MaterialCategroyDeptURL + $("[id$=hdfDeptID]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
    GrandScriptUtils.MakeAutoComplete("txtItemCategory", PurchaseRequestConfig.MaterialCategroyDeptURL + $("[id$=hdfDeptID]").val(), "hdfItemCategoryPK", true, false, "BizUnitPk", true); //textbox Below ROL / MSL / Category 

}

function FillCategoryMaterials(catgID) {
    ///<summary>Function Used to Fill material based on the category  </summary>

    $("[id$=ItemCodeMaterial]").val("");
    $("[id$=MaterialPK]").val(0);
    $("[id$=MaterialSpec]").val(""); //For Clearing Specification Field
    GrandScriptUtils.MakeAutoCompleteLimitLen("ItemCodeMaterial", PurchaseRequestConfig.MaterialURL, "MaterialPK", true, false, "MaterialCategoryPK", true, "Store", "", "", $("[id$=AutoStartValue]").val());
}

function FillUOM(catgID, selectVal) {
    ///<summary>function To Fill UOM Details </summary>
    $("[id$=UOMConversion]").val("1");
    var drpID = $("select[id$=MaterialUOM]").attr("id");
    $("[id$=ShowUOM]").hide();
    $.get(PurchaseRequestConfig.UomURL + catgID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        if ($("select[id$=MaterialUOM] option").length > 2)
            $("[id$=ShowUOM]").show();
        else
            $("[id$=ShowUOM]").hide();
    });
}

function FillItemUOM(catgID, selectVal) {
    ///<summary>function To Fill UOM Details </summary>

    var drpID = $("select[id$=SelectItemUOM]").attr("id");
    $.get(PurchaseRequestConfig.UomURL + catgID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        $("[id$=DefaultUOM]").val(selectVal);
        $("[id$=ItemUOMText]").html($("[id$=SelectItemUOM] :selected").text());
    });
}

function FillStore(selectVal) {
    ///<summary>to fill store combo</summary>
    storeLoaded = false;
    var drpID = $("select[id$=Store]").attr("id");
    var storeURL = "";
    var purchaseReqObj = $("#divData").data("purchaseReqObj");
    if (((purchaseReqObj != undefined) && (purchaseReqObj.PRH_STATUS == 1 || purchaseReqObj.PRH_STATUS == 2 || purchaseReqObj.PRH_STATUS == 7))) {
        storeURL = PurchaseRequestConfig.FillStoreDropdownURL + PurchaseRequestConfig.BizUnitPk;
    }
    else {
        storeURL = PurchaseRequestConfig.FillStoreDropdownURL + PurchaseRequestConfig.BizUnitPk + "&UserPK=" + $("[id$=UserPk]").val();
    }
    $.get(storeURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        $("select[id$=Store]").attr("disabled", true);
        storeLoaded = true;
        BindGrid();
    });
}

///<summary>to fill IO Number combo</summary>
function FillIONumber(selectVal) {
    //    var drpID = $("select[id$=PRH_SO_HDR]").attr("id");
    //    var storeURL = "";
    //    storeURL = PurchaseRequestConfig.FillIODropdownURL + $("[id$=BizUnitPk]").val() + "&PrhPK=" + $("[id$=PRH_PK]").val();
    //    $.get(storeURL, function (data) {
    //        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
    //        if (storeLoaded) {
    //            BindGrid();
    //        }
    //    });

    //    $("[id$=txtSO]").val("");
    //    $("[id$=PRH_SO_HDR]").val(0);
    GrandScriptUtils.MakeAutoComplete("txtSO", PurchaseRequestConfig.FillIODropdownURL + $("[id$=BizUnitPk]").val() + "&PrhPK=" + $("[id$=PRH_PK]").val() + "&SohPK=" + selectVal + "&DeptPk=" + $("[id$=hdfDeptID]").val(), "PRH_SO_HDR", true, false, "BizUnitPk", true);
    if (storeLoaded) {
        BindGrid();
    }
}

function FillMaterialDetails(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the id </summary>
    $("[id$=UOMConversion]").val("1");
    var DefaultPRItemUOM = $("input[id$=hdfDefaultPRItemUOM]").val();
    $.get(PurchaseRequestConfig.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
        if (data) {
            if (materialID != 0) {
                switch (DefaultPRItemUOM) {
                    case PurchaseRequestConfig.ITM_UOM:
                        $("[id$=MaterialUOM]").val(data[0].ITM_UOM);
                        FillUOM(materialID, data[0].ITM_UOM);
                        FillItemUOM(materialID, data[0].ITM_UOM);
                        break;
                    case PurchaseRequestConfig.PURCHASE_UOM:
                        $("[id$=MaterialUOM]").val(data[0].PURCHASE_UOM);
                        FillUOM(materialID, data[0].PURCHASE_UOM);
                        FillItemUOM(materialID, data[0].PURCHASE_UOM);
                        break;
                    case PurchaseRequestConfig.SALES_UOM:
                        $("[id$=MaterialUOM]").val(data[0].SALE_UOM);
                        FillUOM(materialID, data[0].SALE_UOM);
                        FillItemUOM(materialID, data[0].SALES_UOM);
                        break;
                    default:
                        break;
                }
                $("[id$=MaterialROL]").val(data[0].ITM_ROL_STK);
                $("[id$=MaterialMSL]").val(data[0].ITM_MIN_STK);
                // FillCategoryMaterials(data[0].);
                if ($("[id$=MaterialCategoryPK]").val() == 0) {
                    $("[id$=MaterialCategoryPK]").val(data[0].ITM_CATEGORY);
                    $("[id$=MaterialCategory]").val(data[0].ITC_NAME);
                }

                //Set Description in Materials to Specification Field.                         
                if (data[0].ITM_IS_PM != 1 || $("[id$=hdfShowDescPM]").val() == 1)//In the case of Packing Material no need to show Description(Based on Config). ITM_IS_PM==1  means packing material
                {
                    $("[id$=MaterialSpec]").val(data[0].ITM_DESC);
                    $("[id$=MaterialSpec]").attr('title', data[0].ITM_DESC);
                }
                else {
                    $("[id$=MaterialSpec]").val("");
                    $("[id$=MaterialSpec]").attr('title', "");
                }

            }
            else {
                $("[id$=MaterialUOM]").val("0");
                $("[id$=MaterialROL]").val("0");
                $("[id$=MaterialMSL]").val("0");
            }
        }
    });
}

function BindGrid() {
    ///<summary>To handle bind grid </summary>
    $("[id$=btnAddSelectedItems]").hide();
    var ajaxUrl = "";
    var type = -1;
    if ($("[id$=MinStockLvl]").is(":checked")) {
        type = 0;
    }
    if ($("[id$=ReOrderLvl]").is(":checked")) {
        type = 1;
    }
    if ($("[id$=rbtnCategory]").is(":checked")) {
        type = 2;
    }
    $("[id$=Store]").attr("disabled", "disabled");
    if (PurchaseRequestConfig.IsViewMode == true) {
        //$("[id$=PRH_SO_HDR]").removeAttr("disabled");
        EnableAuto($("[id$=txtSO]"), $("[id$=PRH_SO_HDR]"));
    }

    if ($("[id$=PRH_SO_HDR]").val() == "0" || $("[id$=PRH_SO_HDR]").val() == null) {
        $("[id$=hdfIsPacking]").val("0");
        ajaxUrl = PurchaseRequestConfig.PurchaseRequestBindGridURL + "&BizUnit=" + PurchaseRequestConfig.BizUnitPk + "&Dept=" + $("[id$=Store]").val() + "&Type=" + type + "&PRPK=" + $("[id$=PRH_PK]").val() + "&RCount=" + $("[id$=hdfPRRowCount]").val() + "&ItmCat=" + $("[id$=hdfItemCategoryPK]").val();
        $("[id$=divROL]").show();
        $("[id$=divType]").hide();
        $("[id$=imbMaterialShow]").hide();
        $("#imgReOrderHideHeader").hide();
    }
    else {
        $("[id$=hdfIsPacking]").val("1");
        $("[id$=divROL]").hide();
        $("[id$=divType]").show();
        $("[id$=imbMaterialShow]").show();
        ajaxUrl = PurchaseRequestConfig.PackingMaterialBindGridURL + "&SohPK=" + $("[id$=PRH_SO_HDR]").val() + "&Dept=" + $("[id$=Store]").val() + "&Type=" + $("[id$=IPD_TYPE]").val() + "&RCount=" + $("[id$=hdfPRRowCount]").val() + "&ItmCat=" + $("[id$=hdfItemCategoryPK]").val();
    }

    $("#grdPuchasePendingList").removeAttr("ajaxurl");
    $("#grdPuchasePendingList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPuchasePendingList");
    GrandGrid.MakeGrid($("#grdPuchasePendingList"));

    //    $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
    //    GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);

    if (PurchaseRequestConfig.IsViewMode == true) {
        //$("[id$=PRH_SO_HDR]").attr("disabled", "disabled");
        DisableAuto($("[id$=txtSO]"), $("[id$=PRH_SO_HDR]"));
    }

    if ($("[id$=PRH_SO_HDR]").val() != "0" || $("[id$=PRH_SO_HDR]").val() != null) {
        $.get(ajaxUrl, function (data) {
            if (data != '[object XMLDocument]' || data != "") {

                //                $("#divData").data("ItemToOrder", data.Table1);
                CalculatePercentage();
            }
        });
    }

}
function FillPackingType(itemPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=IPD_TYPE]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(PurchaseRequestConfig.FillPackingType + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, itemPK, true);
        if (storeLoaded) {
            BindGrid();
        }
    });
}

function FillIssuingDepartment(PK, SubDeptPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=PRH_ISSUE_DEPT]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(PurchaseRequestConfig.DepartmentsDDL + PK + "&ProcessID=" + $("[id$=hdfProcessID]").val() + "&RefID=" + $("[id$=hdfRefID]").val(), function (data) {
        if ($("[id$=hdfIsWkfSettingPostBackReqd]").val() == "1" && $("[id$=PRH_PK]").val() == 0 && data != null && data.length == 1) {
            if ($("[id$=hdfIsSingleReqDeptWkfSetting]").val() == "0") { //If the Requested Dept ddl contain only one value,select it by default(BugID:30642).In case of WKF SETTING configuration,we need postback.Infinite loop occur.For avoiding this we used this hiddenfield --%>
                GrandScriptUtils.FillDropDown(drpID, data, true, false, data[0].Value);
                $("[id$=hdfReqDeptPk]").val(data[0].Value);
                ReqDeptChange();
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, false, data[0].Value);
                $("[id$=hdfReqDeptPk]").val(data[0].Value);
                FillIssuingSubDepartment(SubDeptPK, data[0].Value);
            }
        }
        else {
            if (data != null && data.length == 1) {
                GrandScriptUtils.FillDropDown(drpID, data, true, false, data[0].Value);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, PK);
            }
            $("[id$=hdfReqDeptPk]").val(PK);
            FillIssuingSubDepartment(SubDeptPK, PK);
        }

    });
}

function FillIssuingSubDepartment(PK, ParentPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=PRH_ISSUE_SUB_DEPT]").attr("id");
    if (ParentPK == null || ParentPK == undefined)
        ParentPK = $("[id$=PRH_ISSUE_DEPT]").val();

    if (PK == null || PK == undefined)
        PK = 0;

    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(PurchaseRequestConfig.SubDepartmentsDDL + PK + "&ParentDeptPk=" + ParentPK, function (data) {
        if (data != null && data.length == 1) { //If the Request by Dept. ddl contain only one value,Select it by default
            GrandScriptUtils.FillDropDown(drpID, data, true, false, data[0].Value);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, PK);
        }
    });
}

function ReqDeptChange() {
    $("[id$=hdfIsReqDeptPostback]").val(1);
    var IsWkfSetting = $("[id$=hdfIsWkfSettingPostBackReqd]").val();
    var issuDept = $("[id$=PRH_ISSUE_DEPT]").val();
    $("[id$=hdfReqDeptPk]").val(issuDept);
    FillIssuingSubDepartment(0, $("[id$=PRH_ISSUE_DEPT]").val());
    if (parseInt(IsWkfSetting) == 1) {
        var PurchaseRequestTemp = new Object();
        PurchaseRequestTemp.PRH_PK = $("[id$=PRH_PK]").val();
        PurchaseRequestTemp.PRH_NO = $("[id$=PRH_NO]").text();
        PurchaseRequestTemp.PRH_DATE = $("[id$=PRH_DATE]").val();
        PurchaseRequestTemp.PRH_ISSUE_DEPT = issuDept;
        PurchaseRequestTemp.PRH_ISSUE_SUB_DEPT = $("[id$=PRH_ISSUE_SUB_DEPT]").val();
        PurchaseRequestTemp.Store = $("[id$=Store]").val();
        PurchaseRequestTemp.BizUnitPk = $("[id$=BizUnitPk]").val();
        PurchaseRequestTemp.UserPk = $("[id$=UserPk]").val();
        PurchaseRequestTemp.PRH_MOD_DT = $("[id$=LAST_MOD_DT]").val();
        PurchaseRequestTemp.LAST_MOD_DT = $("[id$=LAST_MOD_DT]").val();
        PurchaseRequestTemp.PRH_COMPANY = $("[id$=PRH_COMPANY]").val();
        PurchaseRequestTemp.PRH_SO_HDR = $("[id$=PRH_SO_HDR]").val();
        PurchaseRequestTemp.PRH_SO_NO = $("[id$=txtSO]").val();
        PurchaseRequestTemp.PRH_STATUS = $("[id$=hdfStatus]").val();
        PurchaseRequestTemp.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
        $("[id$=PurchaseRequestList]").val(JSON.stringify(PurchaseRequestTemp));
        $("[id$=btnReqDept]").click();
    }
}

//Bind Breakup Store Grid Start
function BindBreakupStoreGrid() {
    ///<summary>To handle bind BreakupStoreGrid </summary>  
    var materialID = $("[id$=MaterialPK]").val();
    $.get(PurchaseRequestConfig.GetBreakupQtyStore + materialID + "&DeptType=2&DeptCat=0&PrDate=" + $("[id$=PRH_DATE]").val() + "&ToUOM=" + $("[id$=MaterialUOM]").val(), function (data) {
        //
        if (data) {
            if ($("#divNodata").length != 0) {
                $("#divNodata").remove();
            }
            GrandGrid.Utilities.ResetGrid(true, "grdBreakupStoreList");
            GrandGrid.MakeGrid($("#grdBreakupStoreList"), 1, data);
        }
        else {
            $("#grdBreakupStoreList").css({ "visibility": "hidden", "display": "none" });
            $("#grdBreakupStoreList").find("tbody").html("");
            $("#divNodata").remove();
            $("<div id=\"divNodata\" class=\"nodata\" >Translate(NoDataFound)</div>").insertBefore($("#grdBreakupStoreList"));
        }
    });
}
//End breakup

//Bind PendingPO Grid Start
function BindPendingPOGrid() {
    ///<summary>To handle bind PendingPOGrid </summary>  
    var materialID = $("[id$=MaterialPK]").val();
    var MaterialUOM = $("[id$=MaterialUOM]").val();
    $.get(PurchaseRequestConfig.GetPendingPODetails + materialID + "&ToUOM=" + $("[id$=MaterialUOM]").val(), function (data) {
        if (data) {
            if ($("#divNodata").length != 0) {
                $("#divNodata").remove();
            }
            GrandGrid.Utilities.ResetGrid(true, "grdPendingPOList");
            GrandGrid.MakeGrid($("#grdPendingPOList"), 1, data);
        }
        else {
            $("#grdPendingPOList").css({ "visibility": "hidden", "display": "none" });
            $("#grdPendingPOList").find("tbody").html("");
            $("#divNodata").remove();
            $("<div id=\"divNodata\" class=\"nodata\" >Translate(NoDataFound)</div>").insertBefore($("#grdPendingPOList"));
        }
    });



}
//End breakup


function PrintPage() {
    //<summary> Function Used to print the details </summary>

    //window.location = "PurchaseRequestReport.aspx?PRID=" + $("input[id$=PRH_PK]").val();
    //window.location = PurchaseRequestConfig.PURCHASEREQUESTREPORTURL + "?ID=" + $("input[id$=PRH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    var url = PurchaseRequestConfig.PURCHASEREQUESTREPORTURL + "?ID=" + $("input[id$=PRH_PK]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    OpenPDF(url);

    return false;
}

function FillPurchaseRequestDetails(purchaseReqObj) {
    //<summary> Function Used to fill purchase request edit details </summary>
    $("[id$=hdfStatus]").val(purchaseReqObj.PRH_STATUS);
    var IsWkfSetting = $("[id$=hdfIsWkfSettingPostBackReqd]").val();
    if (parseInt(IsWkfSetting) == 1) {
        if (purchaseReqObj.PRH_STATUS == 0) {
            $("select[id$=PRH_ISSUE_DEPT]").attr("disabled", false);
            //            $("select[id$=PRH_ISSUE_SUB_DEPT]").attr("disabled", false);
        }
        else {
            $("select[id$=PRH_ISSUE_DEPT]").attr("disabled", true);
            //            $("select[id$=PRH_ISSUE_SUB_DEPT]").attr("disabled", true);
        }
    }
    if (purchaseReqObj.PRH_SO_HDR == null || purchaseReqObj.PRH_SO_HDR == "undefined") {
        FillIONumber(0);
    }
    else {
        $("[id$=PRH_SO_HDR]").val(purchaseReqObj.PRH_SO_HDR);
        $("[id$=txtSO]").val(purchaseReqObj.PRH_SO_NO);
    }
    //Set a stamp for cancelled record
    if (purchaseReqObj.PRH_DEL_STATUS == 1)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");

    //End 
    $("[id$=PRH_PK]").val(purchaseReqObj.PRH_PK);

    if (purchaseReqObj.PRH_NO == null || purchaseReqObj.PRH_NO == "")
        $("[id$=PRH_NO]").text(PurchaseRequestConfig.DocGenerationNewValue);
    else
        $("[id$=PRH_NO]").text(purchaseReqObj.PRH_NO);
    $("[id$=PRH_DATE]").val(purchaseReqObj.PRH_DATE);
    $("[id$=LAST_MOD_DT]").val(purchaseReqObj.PRH_MOD_DT);
    //New Changes on 15-12-2015
    FillIssuingDepartment(purchaseReqObj.PRH_ISSUE_DEPT, purchaseReqObj.PRH_ISSUE_SUB_DEPT);
    $("[id$=PRH_USER]").val(purchaseReqObj.PRH_USER);
    if ($("[id$=hdfIsMultiplePlant]").val() == "1") {
        $("[id$=hdfSelCompany]").val(purchaseReqObj.PRH_COMPANY);
    }

    FillCompany(purchaseReqObj.PRH_COMPANY);
    if (PurchaseRequestConfig.IsViewMode) {
        DisableFileds();
    }
    else if (purchaseReqObj.PRH_STATUS == 1 || purchaseReqObj.PRH_STATUS == 7 || purchaseReqObj.PRH_STATUS == 11 || purchaseReqObj.PRH_STATUS == 9) {
        DisableFileds();
    }
    if (purchaseReqObj.PRH_STATUS == 2 && PurchaseRequestConfig.IsModifyPR == true) {
        $("[id$=imbAddNew]").hide();
        $("[id$=btnAddSelectedItems]").hide();
        $("[id$=Store]").attr("disabled", "disabled");
        $("[id$=PRH_USER]").attr("disabled", "disabled");
        $("[id$=PRH_DATE]").attr("disabled", "disabled");
        // $("select[id$=PRH_SO_HDR]").attr("disabled", true);
        DisableAuto($("[id$=txtSO]"), $("[id$=PRH_SO_HDR]"));
        $("select[id$=PRH_ISSUE_DEPT]").attr("disabled", true);
        $("select[id$=PRH_ISSUE_SUB_DEPT]").attr("disabled", true);
        $("select[id$=PRH_COMPANY]").attr("disabled", true);
        //Assigning POQuantity To PR Quantity if POQty Greater Than Zero
        var PRDetails = new Array();
        if ($.isArray(purchaseReqObj.PurchaseRequestList)) {
            PRDetails = purchaseReqObj.PurchaseRequestList;
        }
        else {
            PRDetails.push(purchaseReqObj.PurchaseRequestList);
        }
        for (var i in PRDetails) {
            //            if (PRDetails[i].PRD_QTY_ORDERED > 0) {
            PRDetails[i].PRD_QTY_REQUESTED = PRDetails[i].PRD_QTY_RECEIVED; //PRDetails[i].PRD_QTY_ORDERED;
            //}
        }
    }
    $("#divData").data("purchaseReqObj", purchaseReqObj);

    if ($("[id$=hdfIsPRPMAttachmentShow]").val() == "0") {
        if (PurchaseRequestConfig.IsViewMode || purchaseReqObj.PRH_STATUS == 1 || purchaseReqObj.PRH_STATUS == 7 || purchaseReqObj.PRH_STATUS == 11 || purchaseReqObj.PRH_STATUS == 9) {
            GrandScriptUtils.MakeFileUploader("fupUploaderPR", true, "divFileData", "FILELIST", "PR", false);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploaderPR", true, "divFileData", "FILELIST", "PR", true);
        }
        //#region --------------- Fill File Upload Details----------------------------------
        if (!($.isArray(purchaseReqObj.FILELIST))) {
            if (purchaseReqObj.FILELIST != undefined) {
                objArray = purchaseReqObj.FILELIST;
                FileJson.FILELIST = new Array();
                FileJson.FILELIST.push(objArray);
            }
            else {
                objArray = purchaseReqObj.FILELIST;
                FileJson.FILELIST = new Array();
            }
        }
        else {
            FileJson.FILELIST = purchaseReqObj.FILELIST;
        }
        FillFileDetails(1);
        //#Endregion
    }
    if ($("[id$=hdfIsReqDeptPostback]").val() == "1") {
        setTimeout(
        function () {
            BindGrid();
            ShowReOrder();
        }, 500
                );
    }
    $("[id$=hdfIsReqDeptPostback]").val(0);
}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "MaterialCategory") {
        FillCategoryMaterials($("[id$=MaterialCategoryPK]").val());

    }
    if (targetControlID == "ItemCodeMaterial") {
        FillMaterialDetails($("[id$=MaterialPK]").val());
    }
    else if (targetControlID == "txtSO") {
        ChangeIO(false);
    }
}

function AfterInvalidSelect(targetControlID) {
    if (targetControlID == "txtSO") {
        ChangeIO(false);
    }
}

function IsItemExistInPendingList(itemPK) {
    var flag = false;
    var colIndex = 0;
    var pendingItemPK = 0;
    $("#grdPuchasePendingList tr:has(td)").each(function (index) {
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_PK", "grdPuchasePendingList");
        pendingItemPK = GrandGrid.Utilities.GetColumnValue($(this), "ITM_PK", "grdPuchasePendingList");
        if (colIndex != null) {
            if (itemPK == pendingItemPK) {
                flag = true;
            }
        }

    });
    return flag;
}
function ChangeIO(isOK) {
    $("#imgReOrderHideHeader").show();
    $("[id$=hdfIsIOSelected]").val($("[id$=PRH_SO_HDR]").val());
    PurchaseRequestConfig.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
    if (PurchaseRequestConfig.PurchaseRequestList.length != 0) {
        if (isOK) {
            BindGrid();
            var length = PurchaseRequestConfig.PurchaseRequestList.length;
            PurchaseRequestConfig.PurchaseRequestList.splice(0, length);
            $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
            $(PurchaseRequestConfig.tdset).insertAfter($("#PurchaseRequestInsert").find("tr:eq(0)"));
            $("#divPurchaseRequestInsert").show();
            GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
            $("[id$=hdfCurIONumber]").val($("[id$=PRH_SO_HDR]").val());
        }
        else {
            GrandScriptUtils.ShowModal(PurchaseRequestConfig.ClearREQ, PurchaseRequestConfig.Confirmation, PurchaseRequestConfig.CLEARITEM, true);
        }
    }
    else {
        BindGrid();
        $("[id$=hdfCurIONumber]").val($("[id$=PRH_SO_HDR]").val());
    }
    $("#divPackingMaterialDetails").dialog("close");
    ShowReOrder();
}

function AddPurchaseRequest(isValidated) {
    //<summary> Function Used to add the item to list </summary>

    AddValidations(2);

    if ($(document.forms[0]).valid()) {
        PurchaseRequestConfig.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
        PurchaseRequestConfig.PurchaseRequestObj = new Object();
        if (!isValidated) {
            //            if (!CheckItemExists()) {
            //                GrandScriptUtils.ShowModal(PurchaseRequestConfig.RecordExist, PurchaseRequestConfig.MessageBoxTitle);
            //                return false;
            //            }
            //Showing Confirmation Popup for Same Item with Same Required by Date 
            if ($("[id$=hdfIsContSameItemDate]").val() != "1") {
                if (!CheckItemExists()) {
                    ConfirmSameItemDate();
                    return false;
                }
            }
            $("[id$=hdfIsContSameItemDate]").val("0"); //Reset after adding item

            var hasMSL = IsItemExistInPendingList($("[id$=MaterialPK]").val());
            if (!hasMSL) {
                if ($("[id$=MaterialROL]").val() != "" || $("[id$=MaterialMSL]").val() != "") {
                    if (parseInt($("[id$=MaterialROL]").val()) != 0 || parseInt($("[id$=MaterialMSL]").val()) != 0) {
                        GrandScriptUtils.ShowModal(PurchaseRequestConfig.MSLNotReached, PurchaseRequestConfig.Confirmation, PurchaseRequestConfig.ADDITEM, true);
                        return false;
                    }
                }
            }
        }
        PurchaseRequestConfig.PurchaseRequestObj = new Object();
        PurchaseRequestConfig.PurchaseRequestObj.SLNO = $("[id$=hdfSlNo]").val() == -1 ? PurchaseRequestConfig.PurchaseRequestList.length + 1 : $("[id$=hdfSlNo]").val();
        PurchaseRequestConfig.PurchaseRequestObj.ITM_CATEGORY = $("[id$=MaterialCategoryPK]").val();
        PurchaseRequestConfig.PurchaseRequestObj.PRD_ITEM = $("[id$=MaterialPK]").val();
        PurchaseRequestConfig.PurchaseRequestObj.PRD_UOM = $("[id$=MaterialUOM]").val();
        PurchaseRequestConfig.PurchaseRequestObj.PRD_REQD_DATE = $.trim($("[id$=RequiredDate]").val());
        PurchaseRequestConfig.PurchaseRequestObj.PRD_BIZUNIT = PurchaseRequestConfig.BizUnitPk;
        PurchaseRequestConfig.PurchaseRequestObj.PRD_DEPT = $("[id$=Store]").val();
        PurchaseRequestConfig.PurchaseRequestObj.ITC_NAME = $("[id$=MaterialCategory]").val();
        PurchaseRequestConfig.PurchaseRequestObj.ITM_TEXT = $("[id$=ItemCodeMaterial]").val();
        PurchaseRequestConfig.PurchaseRequestObj.UOM = $("[id$=MaterialUOM] :selected").text();
        PurchaseRequestConfig.PurchaseRequestObj.PRD_QTY_REQUESTED = parseFloat($("[id$=PurchaseQty]").val()).toFixed(QtyDec);

        PurchaseRequestConfig.PurchaseRequestObj.ITC_CRTD_BY = $("[id$=UserPk]").val();
        PurchaseRequestConfig.PurchaseRequestObj.ITC_MOD_BY = $("[id$=UserPk]").val();
        PurchaseRequestConfig.PurchaseRequestObj.PRD_ITEM_SPEC = $("[id$=MaterialSpec]").val() == "undefined" ? "" : $("[id$=MaterialSpec]").val();
        PurchaseRequestConfig.PurchaseRequestObj.PRD_PURPOSE = $("[id$=RequestPorpose]").val();

        if (parseInt(PurchaseRequestConfig.PRDITEM) == 0) {
            PurchaseRequestConfig.PurchaseRequestList.push(PurchaseRequestConfig.PurchaseRequestObj);
        }
        else {
            for (var i in PurchaseRequestConfig.PurchaseRequestList) {
                var isItemExists = 0;

                //isItemExists = (PurchaseRequestConfig.PurchaseRequestList[i].PRD_ITEM == $.trim($("[id$=MaterialPK]").val()) ) ? true : false;
                isItemExists = (PurchaseRequestConfig.PurchaseRequestList[i].SLNO == $.trim($("[id$=hdfSlNo]").val())) ? true : false;
                if (isItemExists) {
                    PurchaseRequestConfig.PurchaseRequestList[i] = PurchaseRequestConfig.PurchaseRequestObj;
                    break;
                }
            }
        }
        if ($("[id$=chkApplyAll]").is(":checked") == true) {
            for (var i in PurchaseRequestConfig.PurchaseRequestList) {
                PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE = $.trim($("[id$=RequiredDate]").val());
            }
        }
        //If chkApplyAllPurpose checkbox is checked,apply entered purposedetails to all items in the list.
        if ($("[id$=chkApplyAllPurpose]").is(":checked") == true) {
            for (var i in PurchaseRequestConfig.PurchaseRequestList) {
                PurchaseRequestConfig.PurchaseRequestList[i].PRD_PURPOSE = $("[id$=RequestPorpose]").val();
            }
        }
        SelRequiredDate = $("[id$=RequiredDate]").val();
        SelPurpose = $("[id$=RequestPorpose]").val();
        $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
        GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
        ShowFields();
        ClearPurchaseRequest();
    }
    return false;
}

function GetCurrentDate() {
    //<summary> Function Used to get tommorrow date </summary>

    var now = new Date();
    now.setDate(now.getDate() + 1)
    var dd = (now.getDate() < 10) ? "0" + now.getDate() : now.getDate();
    var mm = now.getMonth();
    var yyyy = now.getFullYear();
    var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    mm = mmm[mm];
    var toDate = dd + "-" + mm + "-" + yyyy;
    return toDate;
}
function AddPurchaseRequestList(tr, status, isConfirmed) {
    //<summary> Function Used to add the item to list </summary>

    RemoveValidations();
    var grdID = $(tr).parents("table:first").attr("id");
    var colIndex = 0;
    var reqQty = 0;
    var reqDate = '';

    var currentStock = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.CURRENT_STOCK, grdID));
    var minStock = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.MINIMUM_STOCK, grdID));
    if (!isConfirmed && !isNaN(currentStock) && !isNaN(minStock) && currentStock > minStock) {
        GrandScriptUtils.AddRow = $(tr);
        GrandScriptUtils.ShowModal(PurchaseRequestConfig.MSLNotReached, PurchaseRequestConfig.Confirmation, PurchaseRequestConfig.ADD, true);
    }
    else {
        if (status == true) {
            reqDate = GetCurrentDate();
        }
        else {
            reqDate = $("[id$=datenewRqst]").val();
        }

        if (CheckItemAlreadyAddedInSameDate(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID), reqDate)) {
            PurchaseRequestConfig.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
            $("[id$=MaterialPK]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID));
            PurchaseRequestConfig.PurchaseRequestObj = new Object();
            PurchaseRequestConfig.PurchaseRequestObj.ITM_CATEGORY = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_CATEGORY, grdID);
            PurchaseRequestConfig.PurchaseRequestObj.PRD_ITEM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID);
            PurchaseRequestConfig.PurchaseRequestObj.PRD_UOM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_UOM, grdID);
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REQUEST_QTY, grdID);
            PurchaseRequestConfig.PurchaseRequestObj.PRD_REQD_DATE = reqDate;
            PurchaseRequestConfig.PurchaseRequestObj.PRD_BIZUNIT = PurchaseRequestConfig.BizUnitPk;
            PurchaseRequestConfig.PurchaseRequestObj.PRD_DEPT = $("[id$=Store]").val();
            PurchaseRequestConfig.PurchaseRequestObj.ITC_NAME = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITC_NAME, grdID);
            PurchaseRequestConfig.PurchaseRequestObj.ITM_TEXT = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_TEXT, grdID);
            PurchaseRequestConfig.PurchaseRequestObj.UOM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.UOM_CODE, grdID);
            PurchaseRequestConfig.PurchaseRequestObj.PRD_QTY_REQUESTED = $(tr).find("td:eq(" + colIndex + ") input[type=text]").val();
            PurchaseRequestConfig.PurchaseRequestObj.ITC_CRTD_BY = $("[id$=UserPk]").val();
            PurchaseRequestConfig.PurchaseRequestObj.ITC_MOD_BY = $("[id$=UserPk]").val();
            PurchaseRequestConfig.PurchaseRequestObj.PRD_ITEM_SPEC = "";
            PurchaseRequestConfig.PurchaseRequestObj.PRD_PURPOSE = "";
            if (parseInt(PurchaseRequestConfig.PRDITEM) == 0) {
                PurchaseRequestConfig.PurchaseRequestList.push(PurchaseRequestConfig.PurchaseRequestObj);
            }
            $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
            GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
            ShowFields();
            ClearPurchaseRequest();
            if (status == false) {
                $("#ItemAlreadyAddedPopUp").dialog("close");
            }
        }
        else {
            trDetails = tr;
            ViewAddNewRequestDateForItem();
        }
    }
    return false;
}
function ValidQuantity() {
    var flag = true;
    $("#grdPuchasePendingList tr input[type=checkbox]:checked").each(function () {
        tr = $(this).parent().parent();
        if (tr[0].nodeName != "TR")
            tr = $(this).parent().parent().parent();
        if ($(tr[0]).parent().find("th").length == 0) {
            grdID = $(tr).parents("table:first").attr("id");
            var colIndex = GrandGrid.Utilities.GetColumnIndex(tr, PurchaseRequestConfig.REQUEST_QTY, grdID);
            var reqQty = $(tr).find("td:eq(" + colIndex + ") input[type=text]").val();
            if (parseFloat(reqQty) <= 0.00)
                flag = false;

        }
    });
    return flag;
}
//____________
function AddPRList(status, isConfirmed) {
    //<summary> Function Used to add multiple item to list </summary>
    RemoveValidations();
    var tr = null;
    var grdID = "";
    var colIndex = 0;
    var colIndexSpec = 0;
    var reqQty = 0;
    var reqSpec = "";
    var reqDate = '';
    var itemSelected = null;
    ValidQuantity();
    if (status == true) {
        if (!isConfirmed) {
            var hasMSL = true;
            if ($("#grdPuchasePendingList tr input[type=checkbox]:checked").length == 0) {
                GrandScriptUtils.ShowModal(PurchaseRequestConfig.SelectRequestItem, PurchaseRequestConfig.MessageBoxTitle);
                return false;
            }
            if (!ValidQuantity()) {
                GrandScriptUtils.ShowModal(PurchaseRequestConfig.RequestQtyGreater, PurchaseRequestConfig.MessageBoxTitle);
                return false;
            }
            $("#grdPuchasePendingList tr input[type=checkbox]:checked").each(function () {
                if (hasMSL) {
                    tr = $(this).parent().parent();
                    if (tr[0].nodeName != "TR")
                        tr = $(this).parent().parent().parent();
                    if ($(tr[0]).parent().find("th").length == 0) {
                        grdID = $(tr).parents("table:first").attr("id");
                        var currentStock = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.CURRENT_STOCK, grdID));
                        var minStock = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.MINIMUM_STOCK, grdID));
                        var rolStock = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.REORDER_LEVEL, grdID));

                        if (!isConfirmed && !isNaN(currentStock) && !isNaN(minStock) && currentStock > minStock && !isNaN(rolStock) && currentStock > rolStock) {
                            hasMSL = false;
                        }
                    }
                }
            });
            if (!hasMSL) {
                GrandScriptUtils.ShowModal(PurchaseRequestConfig.MSLNotReached, PurchaseRequestConfig.Confirmation, PurchaseRequestConfig.ADDMANY, true);
                return false;
            }
        }
        PurchaseRequestConfig.ExistingItems = new Array();
        $("#grdPuchasePendingList tr input[type=checkbox]:checked").each(function () {
            tr = $(this).parent().parent();
            if (tr[0].nodeName != "TR")
                tr = $(this).parent().parent().parent();
            if ($(tr[0]).parent().find("th").length == 0) {
                grdID = $(tr).parents("table:first").attr("id");
                colIndex = 0;
                reqQty = 0;
                reqDate = '';

                if (status == true) {
                    reqDate = GetCurrentDate();
                }
                else {
                    reqDate = $("[id$=datenewRqst]").val();
                }
                itemSelected = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID);
                if (!CheckItemAlreadyAddedInSameDate(itemSelected, reqDate))
                    PurchaseRequestConfig.ExistingItems.push(itemSelected);
            }
        });
    }
    if (!status || PurchaseRequestConfig.ExistingItems.length == 0) {
        //     if ( PurchaseRequestConfig.ExistingItems.length == 0) {
        $("#grdPuchasePendingList tr input[type=checkbox]:checked").each(function () {
            tr = $(this).parent().parent();
            if (tr[0].nodeName != "TR")
                tr = $(this).parent().parent().parent();
            if ($(tr[0]).parent().find("th").length == 0) {
                grdID = $(tr).parents("table:first").attr("id");
                colIndex = 0;
                reqQty = 0;
                reqDate = '';
                currDate = '';

                if (status == true) {
                    reqDate = GetCurrentDate();
                }
                else {
                    reqDate = $("[id$=datenewRqst]").val();
                }
                currDate = GetCurrentDate();
                reqDate = $("[id$=datenewRqst]").val();

                PurchaseRequestConfig.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
                $("[id$=MaterialPK]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID));
                PurchaseRequestConfig.PurchaseRequestObj = new Object();
                PurchaseRequestConfig.PurchaseRequestObj.ITM_CATEGORY = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_CATEGORY, grdID);
                PurchaseRequestConfig.PurchaseRequestObj.PRD_ITEM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID);
                PurchaseRequestConfig.PurchaseRequestObj.PRD_UOM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_UOM, grdID);
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REQUEST_QTY, grdID);
                if (status == true) {
                    PurchaseRequestConfig.PurchaseRequestObj.PRD_REQD_DATE = currDate;
                }
                else {
                    if ($.inArray(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID), PurchaseRequestConfig.ExistingItems) != -1) {
                        PurchaseRequestConfig.PurchaseRequestObj.PRD_REQD_DATE = reqDate;
                    }
                    else {
                        PurchaseRequestConfig.PurchaseRequestObj.PRD_REQD_DATE = currDate;
                    }
                }
                PurchaseRequestConfig.PurchaseRequestObj.SLNO = PurchaseRequestConfig.PurchaseRequestList.length + 1;
                PurchaseRequestConfig.PurchaseRequestObj.PRD_BIZUNIT = PurchaseRequestConfig.BizUnitPk;
                PurchaseRequestConfig.PurchaseRequestObj.PRD_DEPT = $("[id$=Store]").val();
                PurchaseRequestConfig.PurchaseRequestObj.ITC_NAME = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITC_NAME, grdID);
                PurchaseRequestConfig.PurchaseRequestObj.ITM_TEXT = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_TEXT, grdID);
                PurchaseRequestConfig.PurchaseRequestObj.UOM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.UOM_CODE, grdID);
                PurchaseRequestConfig.PurchaseRequestObj.PRD_QTY_REQUESTED = $(tr).find("td:eq(" + colIndex + ") input[type=text]").val();

                PurchaseRequestConfig.PurchaseRequestObj.ITC_CRTD_BY = $("[id$=UserPk]").val();
                PurchaseRequestConfig.PurchaseRequestObj.ITC_MOD_BY = $("[id$=UserPk]").val();

                PurchaseRequestConfig.PurchaseRequestObj.PRD_PURPOSE = "";

                colIndexSpec = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REQUEST_SPEC, grdID);
                if ($(tr).find("td:eq(" + colIndexSpec + ") input[type=text]").val() != "") {
                    PurchaseRequestConfig.PurchaseRequestObj.PRD_ITEM_SPEC = $(tr).find("td:eq(" + colIndexSpec + ") input[type=text]").val();
                }

                if (parseInt(PurchaseRequestConfig.PRDITEM) == 0) {
                    PurchaseRequestConfig.PurchaseRequestList.push(PurchaseRequestConfig.PurchaseRequestObj);
                }
            }
        });
        $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
        GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
        ShowFields();
        ClearPurchaseRequest();
        if (status == false) {
            $("#ItemAlreadyAddedPopUp").dialog("close");
        }
    }
    else {
        ViewAddNewRequestDateForItem();
    }
    return false;
}

//__________________


function ViewAddNewRequestDateForItem() {
    //<summary> Function Used to show popup with new item request date </summary>

    InitNewReqDate();
    $("#ItemAlreadyAddedPopUp").dialog("open");
    $("#ItemAlreadyAddedPopUp").dialog({ width: 550, height: 150, resizable: true, title: PurchaseRequestConfig.ChangeReqDatetitle });
    return false;
}

function CancelNewDate() {
    //<summary> Function Used to close popup with new item request date </summary>

    RemoveValidations();
    $("#ItemAlreadyAddedPopUp").dialog("close");
    $("input[id$=AssignNewDate]").focus();
    return false;
}

function AddItemWithNewDate() {
    //<summary> Function Used to add new request date with selected item </summary>
    var isExistFlag = true;
    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        $("[id$=datenewRqst]").val($("[id$=RequiredDateNew]").val());
        //New Code Begin
        PurchaseRequestConfig.ExistingItems = new Array();
        $("#grdPuchasePendingList tr input[type=checkbox]:checked").each(function () {
            tr = $(this).parent().parent();
            if (tr[0].nodeName != "TR")
                tr = $(this).parent().parent().parent();
            if ($(tr[0]).parent().find("th").length == 0) {
                grdID = $(tr).parents("table:first").attr("id");
                colIndex = 0;
                reqQty = 0;
                reqDate = '';
                reqDate = $("[id$=datenewRqst]").val();
                itemSelected = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_PK, grdID);
                if (CheckItemAlreadyAddedInSameDate(itemSelected, reqDate))
                    PurchaseRequestConfig.ExistingItems.push(itemSelected);
                else {
                    isExistFlag = false;
                    ViewAddNewRequestDateForItem();
                }
            }
        });

        //end
        if (isExistFlag) {
            if (PurchaseRequestConfig.ExistingItems != null && PurchaseRequestConfig.ExistingItems.length > 0)
                AddPRList(false);
            else
                AddPurchaseRequestList(trDetails, false);
        }
    }
    return false;
}

function InitNewReqDate() {
    //<summary> Function Used to assign date to control </summary>

    GrandScriptUtils.DatePicker("RequiredDateNew", false, false);
}

function CheckItemAlreadyAddedInSameDate(item, date) {
    //<summary> Function Used to check item already added in same date </summary>

    var status = false;
    for (var indx in PurchaseRequestConfig.PurchaseRequestList) {
        if (PurchaseRequestConfig.PurchaseRequestList[indx].PRD_REQD_DATE == date && PurchaseRequestConfig.PurchaseRequestList[indx].PRD_ITEM == item) {
            return false;
        }
    }
    return true;
}

function DisableFileds() {
    //<summary> Function Used to disable some fields </summary>

    $("[id$=imbAddNew]").hide();
    $("[id$=btnAddSelectedItems]").hide();
    $("[id$=Store]").attr("disabled", "disabled");
    $("[id$=PRH_DATE]").attr("disabled", "disabled");
    // $("select[id$=PRH_SO_HDR]").attr("disabled", true);
    DisableAuto($("[id$=txtSO]"), $("[id$=PRH_SO_HDR]"));
    $("select[id$=PRH_COMPANY]").attr("disabled", true);
    $("select[id$=PRH_ISSUE_DEPT]").attr("disabled", true);
    $("select[id$=PRH_ISSUE_SUB_DEPT]").attr("disabled", true);
    $("[id$=PRH_USER]").attr("disabled", "disabled");
}

function EnableFileds() {
    //<summary> Function Used to enable some fields </summary>

    $("[id$=Store]").removeAttr("disabled");
    $("[id$=PRH_DATE]").removeAttr("disabled");
    //$("select[id$=PRH_SO_HDR]").removeAttr("disabled");
    EnableAuto($("[id$=txtSO]"), $("[id$=PRH_SO_HDR]"));
    $("select[id$=PRH_COMPANY]").removeAttr("disabled");
    $("select[id$=PRH_ISSUE_DEPT]").removeAttr("disabled");
    $("select[id$=PRH_ISSUE_SUB_DEPT]").removeAttr("disabled");
    $("[id$=PRH_USER]").removeAttr("disabled");

}


function SavePage(command) {
    //<summary> Function Used to save page </summary>
    //For overlay
    //  $('<div id = "overlay" />').appendTo('body').fadeIn("slow");

    AddValidations(1);
    var IsWkfSetting = $("[id$=hdfIsWkfSettingPostBackReqd]").val();
    if (parseInt(IsWkfSetting) == 1) {
        AddValidations(6);
    }
    if ($(document.forms[0]).valid()) {

        PurchaseRequestConfig.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
        if (!IsValidData()) {
            GrandScriptUtils.ShowModal(PurchaseRequestConfig.EnterRequiredDate.fontcolor("red"), PurchaseRequestConfig.MessageBoxTitle);
            return false;
        }
        //Checking for mandate option of purpose field while adding from 'Items Below ROL / MSL / Category' list
        if (parseInt($("[id$=hdfValidatePurpose]").val()) == 1) {
            if (!IsHavePurpose()) {
                GrandScriptUtils.ShowModal(PurchaseRequestConfig.EnterPurpose.fontcolor("red"), PurchaseRequestConfig.MessageBoxTitle);
                return false;
            }
        }

        if (PurchaseRequestConfig.PurchaseRequestList.length > 0) {
            EnableFileds();
            if ($("[id$=hdfIsPRPMAttachmentShow]").val() == "0") {
                var ObjFile = $("#divFileData").data("FileData");
                $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
            }
            else {
                $("[id$=FILELIST]").val("[]");
            }
            $("[id$=PurchaseRequestList]").val(JSON.stringify(PurchaseRequestConfig.PurchaseRequestList));
            if (command != "Draft") {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
                $("[id$=WKF_FLAG]").val('1');
            }
            else
                $("[id$=ActionID]").val('0');
            //To prevent multiple submission
            if ($("[id$=SubmitFlag]").val() == "0")
                $("[id$=SubmitFlag]").val('1');
            else
                return false;
            if (command == "Submit") {
                $("[id$=WKF_FLAG]").val('1');
            }
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            $.post(PurchaseRequestConfig.SavePurchaseRequest, jSonString, function (data) {

                if (parseInt(data[0]) > 0) {
                    if (command == "Draft") {
                        var msg = PurchaseRequestConfig.SaveMessage1 + " " + data[1] + " " + PurchaseRequestConfig.SaveMessage2;
                        GrandScriptUtils.ShowModal(msg, PurchaseRequestConfig.MessageBoxTitle, PurchaseRequestConfig.SAVE);
                    }
                    else {
                        $("[id$=AppNo]").val(data[1]);
                        $("[id$=hdfAppID]").val(data[0]);
                        SaveWorkFlow(true);
                    }
                }
                else if (parseInt(data[0]) == -1) {
                    //                    $(this).fadeOut("slow").remove();
                    GrandScriptUtils.ShowModal(PurchaseRequestConfig.ActionFailedMessage, PurchaseRequestConfig.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -2) {
                    //                    $(this).fadeOut("slow").remove();
                    GrandScriptUtils.ShowModal(PurchaseRequestConfig.SaveMessage1 + " " + $("[id$=PRH_NO]").html() + " " + PurchaseRequestConfig.EditUsedByAnotherUser, PurchaseRequestConfig.MessageBoxTitle, PurchaseRequestConfig.SAVE);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -3) {
                    //                    $(this).fadeOut("slow").remove();
                    GrandScriptUtils.ShowModal(PurchaseRequestConfig.ItemAlreadyAddedMsg, PurchaseRequestConfig.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else {
                    //                    $(this).fadeOut("slow").remove();
                    GrandScriptUtils.ShowModal(PurchaseRequestConfig.ActionFailedMessage, PurchaseRequestConfig.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
            });
        }
        else {
            GrandScriptUtils.ShowModal(PurchaseRequestConfig.PurchaseRequestDetails, PurchaseRequestConfig.MessageBoxTitle);
            $("[id$=SubmitFlag]").val('0')
        }
    }

    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var msg = "";
    var IsInbox = "False";
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        msg = PurchaseRequestConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseRequestConfig.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, PurchaseRequestConfig.MessageBoxTitle, PurchaseRequestConfig.INBOX);
    }
    else {
        msg = PurchaseRequestConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseRequestConfig.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, PurchaseRequestConfig.MessageBoxTitle, PurchaseRequestConfig.SAVE);
        //        var refID = $("[id$=hdfRefID]").val() == "0" ? $("[id$=ReferenceID]").val() : $("[id$=hdfRefID]").val();
        //        $.get(PurchaseRequestConfig.WorkflowStatus + refID + "&ProcessID=" + $("[id$=hdfProcessID]").val(), function (data) {
        //            if ((data[0] == undefined || data.length == 0) && $("[id$=hdfIsGoToInbox]").val() == "1") {
        //                IsInbox = "True";
        //                msg = PurchaseRequestConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseRequestConfig.SubmitMessage;
        //                GrandScriptUtils.ShowModal(msg, PurchaseRequestConfig.MessageBoxTitle, PurchaseRequestConfig.INBOX);
        //            }
        //            if (IsInbox == "False") {
        //                msg = PurchaseRequestConfig.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + PurchaseRequestConfig.SubmitMessage;
        //                GrandScriptUtils.ShowModal(msg, PurchaseRequestConfig.MessageBoxTitle, PurchaseRequestConfig.SAVE);
        //            }
        //        });
    }
}

/*
 
*/
function ClearPurchaseRequest() {
    ///<summary> Function Used to Clear Fields </summary>

    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=MaterialPK]").val(0);
    $("[id$=MaterialUOM]").val(0);
    $("[id$=ShowUOM]").hide();
    $("[id$=UOMConversion]").val("1");
    $("[id$=MaterialCategory]").val("");
    $("[id$=ItemCodeMaterial]").val("");
    $("[id$=PurchaseQty]").val("");
    $("[id$=MaterialSpec]").val("");
    if (parseInt($("[id$=hdfValidatePurpose]").val()) == 1)
        $("[id$=RequestPorpose]").val(SelPurpose);
    else
        $("[id$=RequestPorpose]").val("");
    $("[id$=hdfSlNo]").val("-1");
    PurchaseRequestConfig.PRDITEM = 0;
    FillMaterialCategoryAutoComplete();
    FillCategoryMaterials(0);
    DateDtlsInit();
    $("[id$=RequiredDate]").val(SelRequiredDate);
    PurchaseRequestConfig.ExistingItems = new Array();
    $("#grdPuchasePendingList tr input[type=checkbox]").removeAttr("checked");
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for Catch all the grid events in this function </summary>

    RemoveValidations();
    switch (command.toString().toLowerCase()) {

        case PurchaseRequestConfig.EDIT:
            FillPurchaseRequest(tr);
            break;

        case PurchaseRequestConfig.DELETE:
            var grdID = $(tr).parents("table:first").attr("id");
            PurchaseRequestConfig.DeletePk = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_ITEM, grdID);
            PurchaseRequestConfig.DeleteDate = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_REQD_DATE, grdID);
            PurchaseRequestConfig.SlNo = GrandGrid.Utilities.GetColumnValue(tr, "SlNo", grdID);
            GrandScriptUtils.ShowModal(PurchaseRequestConfig.DeleteConfirmMsg, PurchaseRequestConfig.Confirmation, PurchaseRequestConfig.DELETE, true);
            break;
    }
    return false;
}

function DeletePurchaseRequest() {
    ///<summary>For delete the item in the grid - Details</summary>

    PurchaseRequestConfig.PurchaseRequestList = $("#divData").data("PurchaseRequestData");
    for (var i in PurchaseRequestConfig.PurchaseRequestList) {
        if ((PurchaseRequestConfig.PurchaseRequestList[i].PRD_ITEM == PurchaseRequestConfig.DeletePk) && (PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE == PurchaseRequestConfig.DeleteDate) && (PurchaseRequestConfig.PurchaseRequestList[i].SLNO == PurchaseRequestConfig.SlNo)) {
            PurchaseRequestConfig.PurchaseRequestList.splice(i, 1);
            break;
        }
    }
    PurchaseRequestConfig.DeletePk = 0;
    PurchaseRequestConfig.DeleteDate = "";
    $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
    GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
    if (PurchaseRequestConfig.PurchaseRequestList.length == 0) {
        $(PurchaseRequestConfig.tdset).insertAfter($("#PurchaseRequestInsert").find("tr:eq(0)"));
        $("#divPurchaseRequestInsert").show();
    }
    ShowFields();
    ClearPurchaseRequest();
}

function FillPurchaseRequest(tr) {
    //<summary>Function used to Purchase Request for edit</summary>

    var grdID = $(tr).parents("table:first").attr("id");
    $("[id$=MaterialCategoryPK]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_CATEGORY, grdID));
    PurchaseRequestConfig.PRDITEM = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_ITEM, grdID);
    $("[id$=MaterialPK]").val(PurchaseRequestConfig.PRDITEM);
    $("[id$=ItemCodeMaterial]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITM_TEXT, grdID));
    FillUOM($("[id$=MaterialPK]").val(), GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_UOM, grdID));
    FillItemUOM($("[id$=MaterialPK]").val(), GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_UOM, grdID));
    $("[id$=MaterialCategory]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.ITC_NAME, grdID));
    PurchaseRequestConfig.PRDREQDDATE = GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_REQD_DATE, grdID);
    $("[id$=RequiredDate]").val(PurchaseRequestConfig.PRDREQDDATE);
    $("[id$=PurchaseQty]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_QTY_REQUESTED, grdID).replace(/[^0-9\.]+/g, ""));
    $("[id$=MaterialSpec]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_ITEM_SPEC, grdID));
    $("[id$=RequestPorpose]").val(GrandGrid.Utilities.GetColumnValue(tr, PurchaseRequestConfig.PRD_PURPOSE, grdID));
    $("[id$=hdfSlNo]").val(GrandGrid.Utilities.GetColumnValue(tr, "SLNO", grdID));

    //HideFields();
    //$("[id$=MaterialCategory]").attr("disabled", "disabled");
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    window.location = $("[id$=hdfBackUrl]").val(); // PurchaseRequestConfig.PurchaseListUrl;
    return false;
}

function ViewMaterialDetails() {
    //<summary>Function Used to Popup Material Details</summary>

    FillMaterialDescription();
    $("#divMetarialDetails").dialog("open");
    return false;
}

//breakup store popup
function ViewBreakupStore() {
    //<summary>Function Used to Popup Breakup of StoreStock</summary>  
    BindBreakupStoreGrid();
    $("#divBreakupStore").dialog("open");
    $("#divBreakupStore").dialog({ width: 350, height: 200, resizable: true });
    return false;
}

//end breakup

//ViewPendingPO popup
function ViewPendingPO() {
    //<summary>Function Used to Popup Pending PO</summary>     
    BindPendingPOGrid();
    var materialName = $("[id$=ItemCodeMaterial]").val();

    $("[id$=lblMaterialNamePendingPO]").attr('title', materialName);
    if (materialName.length > 90) {
        materialName = materialName.substring(0, 90);
        materialName += '...';
    }

    $("[id$=lblMaterialNamePendingPO]").text(materialName);
    $("#divPendingPO").dialog("open");
    $("#divPendingPO").dialog({ width: 950, height: 400, resizable: true });
    return false;
}

//end ViewPendingPO

function ViewPackingMaterialDetails() {
    //<summary>Function Used to Popup Material Details</summary>
    //Packing Material Details
    ajaxUrl = PurchaseRequestConfig.PackingMaterialBindGridURL + "&SohPK=" + $("[id$=PRH_SO_HDR]").val();
    $("#grdMaterials").removeAttr("ajaxurl");
    $("#grdMaterials").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdMaterials");
    GrandGrid.MakeGrid($("#grdMaterials"));

    $("#divPackingMaterialDetails").dialog("open");
    $("#divPackingMaterialDetails").dialog({ "width": 550 });
    return false;
}

function GetConversionFactor(from, to) {
    ////<summary>method to get the conversion factor</summary>
    ////<param "from">From UOM PK </param>
    ////<param "to">To UOM PK </param>
    if (from == null) {
        from = $("[id$=SelectItemUOM]").val();
    }
    if (to == null) {
        to = $("[id$=DefaultUOM]").val();
    }
    if (from == 0 || to == 0) {
        return false;
    }
    $.get(PurchaseRequestConfig.GetConcversionFactors + from + "&UOMTo=" + to, function (data) {
        if (data != "" && data != "-1") {
            $("[id$=UOMConversion]").val(parseFloat(data));
        }
        else {
            // no conversion factor exists cannot add material
            GrandScriptUtils.ShowModal(PurchaseRequestConfig.SelectAnotherUOM, PurchaseRequestConfig.ConfirmationMsg);
            $("[id$=SelectItemUOM]").val("0");
            $("[id$=UOMConversion]").val("1");
            return false;
        }
    });
    return false;
}

function ShowUOMDetails() {
    //$("[id$=ItemQty]").val($("[id$=PurchaseQty]").val());
    $("[id$=ItemQty]").val("");
    $("[id$=CalculatedQty]").val("");
    $("#UOMDetailsPopUp").dialog({ width: 600, height: 150, resizable: true, title: PurchaseRequestConfig.UOMConversion });
    return false;
}
function CalculateUOMQty() {
    RemoveValidations();
    //AddValidations(4);
    if ($(document.forms[0]).valid()) {
        var uomConversion = parseFloat($("[id$=UOMConversion]").val());
        var uomQty = parseFloat($("[id$=ItemQty]").val());
        if (!isNaN(uomConversion) && !isNaN(uomQty)) {
            $("[id$=CalculatedQty]").val((uomConversion * uomQty).toFixed(QtyDec));
        }
        else
            $("[id$=CalculatedQty]").val("");
    }
    return false;
}
function SetUOMQty() {
    RemoveValidations();
    //AddValidations(4);
    if ($(document.forms[0]).valid()) {
        var uomConversion = parseFloat($("[id$=UOMConversion]").val());
        var uomQty = parseFloat($("[id$=ItemQty]").val());
        if (!isNaN(uomConversion) && !isNaN(uomQty)) {
            $("[id$=PurchaseQty]").val((uomConversion * uomQty).toFixed(QtyDec));
            $("[id$=ItemQty]").val("");
            $("[id$=CalculatedQty]").val("");
            RemoveValidations();
        }
        $("#UOMDetailsPopUp").dialog("close");
        $("input[id$=PurchaseQty]").focus();
    }
    return false;
}
function CancelUOMQty() {
    //<summary> Function Used to close popup with UOM </summary>
    RemoveValidations();
    $("[id$=ItemQty]").val("");
    $("[id$=CalculatedQty]").val("");
    $("[id$=SelectItemUOM]").val("0");
    $("[id$=UOMConversion]").val("1");
    $("#UOMDetailsPopUp").dialog("close");
    $("input[id$=PurchaseQty]").focus();
    return false;
}
function FillMaterialDescription() {
    //<summary>Function Used to Fill Material overview by metarial id and date</summary>

    var materialID = $("[id$=MaterialPK]").val();
    $.get(PurchaseRequestConfig.GetMaterialDescription + materialID + "&DepartmentID=" + $("[id$=Store]").val() + "&ToUOM=" + $("[id$=MaterialUOM]").val(), function (data) {
        if (data) {
            var ddlItemUOM = $("[id*=MaterialUOM]");
            var selectedText = ddlItemUOM.find("option:selected").text();
            $("[id$=ThisStoreStock]").html(data[0].STD_QTY_IN_STOCK_TO_UOM + "  (" + selectedText + ")");
            $("[id$=CurrentStock]").html(data[0].STH_QTY_IN_STOCK_TO_UOM + "  (" + selectedText + ")");
            $("[id$=PendingRecievable]").html(data[0].STH_QTY_PN_RECEIPT_TO_UOM + "  (" + selectedText + ")");
            $("[id$=PendingInspection]").html(data[0].STH_QTY_PN_INSPECTION + "  (" + selectedText + ")");
            if (data[0].ITM_ROL_STK != null)
                $("[id$=lblROL]").html(data[0].ITM_ROL_STK + "  (" + selectedText + ")");
            if (data[0].ITM_MIN_STK != null)
                $("[id$=lblMinStockLevel]").html(data[0].ITM_MIN_STK + "  (" + selectedText + ")");
            var lastPORate = 0;
            if (parseInt(data[0].LAST_PO_PRICE) > 0) {
                lastPORate = parseFloat(data[0].LAST_PO_PRICE).toFixed(RateDec) + "(" + data[0].LAST_PO_CURRENCY + ")";
            }
            $("[id$=lblLastPoRate]").html(lastPORate);
        }
        else {
            $("[id$=ThisStoreStock]").html("");
            $("[id$=CurrentStock]").html("");
            $("[id$=PendingRecievable]").html("");
            $("[id$=PendingInspection]").html("");
            $("[id$=lblROL]").html("");
            $("[id$=lblMinStockLevel]").html("");
            $("[id$=lblLastPoRate]").html("");
        }
    });
}

//#endregion

//#region----------- Utility Section----------------

function CheckItemExists() {
    //<summary>function used to check whether item already exists.</summary>

    var flag = true;
    var isExists = false;
    var isNotCurrentRow = false;
    if (parseInt(PurchaseRequestConfig.PRDITEM) == 0) {
        for (var i in PurchaseRequestConfig.PurchaseRequestList) {
            isExists = (PurchaseRequestConfig.PurchaseRequestList[i].PRD_ITEM == $.trim($("[id$=MaterialPK]").val()) && PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE == $.trim($("[id$=RequiredDate]").val())) ? true : false;
            if (isExists) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in PurchaseRequestConfig.PurchaseRequestList) {
            isExists = (PurchaseRequestConfig.PurchaseRequestList[i].PRD_ITEM == $.trim($("[id$=MaterialPK]").val()) && PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE == $.trim($("[id$=RequiredDate]").val())) ? true : false;
            isNotCurrentRow = (parseInt(PurchaseRequestConfig.PRDITEM) == PurchaseRequestConfig.PurchaseRequestList[i].PRD_ITEM && PurchaseRequestConfig.PRDREQDDATE == PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE) ? true : false;
            if ((isExists && !isNotCurrentRow)) {
                flag = false;
                break;
            }
            if (parseInt(PurchaseRequestConfig.PRDITEM) == PurchaseRequestConfig.PurchaseRequestList[i].PRD_ITEM && PurchaseRequestConfig.PRDREQDDATE == PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE)
                PurchaseRequestConfig.PurchaseRequestObj = PurchaseRequestConfig.PurchaseRequestList[i];
        }
    }
    return flag;
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    var purchaseReqObj = $("#divData").data("purchaseReqObj");
    var isViewMode = false;
    if (PurchaseRequestConfig.IsViewMode) {
        $("[id$=imbAddNew]").hide();
        $("#grdPurchaseRequest th:last").hide();
        $("#grdPuchasePendingList th:last").hide();
        isViewMode = true;
    }
    else if (purchaseReqObj != undefined) {
        if (purchaseReqObj.PRH_STATUS == 1 || purchaseReqObj.PRH_STATUS == 7 || purchaseReqObj.PRH_STATUS == 11 || purchaseReqObj.PRH_STATUS == 9) {
            $("[id$=imbAddNew]").hide();
            $("#grdPurchaseRequest th:last").hide();
            $("#grdPuchasePendingList th:last").hide();
            isViewMode = true;
        }
    }
    if (PurchaseRequestConfig.IsModifyPR) {//For Hiding Edit Delete Btn
        isViewMode = true;
    }
    if (grdID == "grdMaterials") {

        $("#grdMaterials tr:has(td)").each(function (index) {
            var moqColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_MOQ", grdID);
            var moq = GrandGrid.Utilities.GetColumnValue($(this), "ITM_MOQ", grdID);
            if (moqColIndex != null && moq != "null") {
                $(this).find("td:eq(" + moqColIndex + ")").html(moq);
            }
            else {
                $(this).find("td:eq(" + moqColIndex + ")").html("0");
            }
        });
    }

    if (grdID == "grdPurchaseRequest") {
        var specColIndex = 0;
        var reqSpec = "";
        var itemColIndex = 0;
        var itemPK = 0;
        var itemCode = "";
        if (PurchaseRequestConfig.tdset == "")
            PurchaseRequestConfig.tdset = $("#PurchaseRequestInsert").find("tr:eq(1)");
        $("#divPurchaseRequestInsert").hide();
        $(PurchaseRequestConfig.tdset).insertBefore($("#grdPurchaseRequest").find("tr:eq(1)"));
        var reqDateID = $("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequiredDate]").attr("id");
        var reqDateName = $("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequiredDate]").attr("name");
        //For Purpose checkbox
        var reqPurposeID = $("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequestPorpose]").attr("id");
        var reqPurposeName = $("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequestPorpose]").attr("name");
        var index = $("#grdPurchaseRequest").find("tr:eq(1)").children().index($("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequiredDate]").parent("td:first"));
        $("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequiredDate]").remove();
        $("#grdPurchaseRequest").find("tr:eq(1)").find("td:eq(" + index + ")").html("<input id=\"" + reqDateID + "\" name=\"" + reqDateName + "\" type=\"text\" class=\"date-picker\" tabIndex=20 />" + " <input type=\"checkbox\" id=chkApplyAll class=\"margntop5\"  title=\"Apply All\" tabIndex=20 />");
        //For Purpose checkbox 
        var index = $("#grdPurchaseRequest").find("tr:eq(1)").children().index($("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequestPorpose]").parent("td:first"));
        $("#grdPurchaseRequest").find("tr:eq(1)").find("input[id$=RequestPorpose]").remove();
        $("#grdPurchaseRequest").find("tr:eq(1)").find("td:eq(" + index + ")").html("<input id=\"" + reqPurposeID + "\" name=\"" + reqPurposeName + "\" type=\"text\" class=\"date-picker\" tabIndex=22 />" + " <input type=\"checkbox\" id=chkApplyAllPurpose class=\"margntop5\"  title=\"Apply All\" tabIndex=22 />");

        DateDtlsInit();
        var qty = 0;
        $("#grdPurchaseRequest tr:has(td)").each(function (index) {
            if (isViewMode) {

                $(this).find("td:last").hide();
                $(this).find("td:last").hide();
            }
            var tableID = $(this).parents("table:first").attr("id");
            if (index != 0) { // If the row is header row, it contains entry fields
                specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.PRD_ITEM_SPEC, grdID);
                reqSpec = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRD_ITEM_SPEC, grdID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRD_ITEM_SPEC, grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRD_ITEM_SPEC, grdID);
                if (specColIndex != null && reqSpec != null) {
                    $(this).find("td:eq(" + specColIndex + ")").html(reqSpec);
                }
                else {
                    $(this).find("td:eq(" + specColIndex + ")").html("");
                }
                if ($("[id$=hdfIsPacking]").val() == "1") {
                    itemColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_TEXT", grdID);
                    if (itemColIndex != null) {
                        itemPK = GrandGrid.Utilities.GetColumnValue($(this), "PRD_ITEM", grdID);
                        itemCode = GrandGrid.Utilities.GetColumnValue($(this), "ITM_TEXT", grdID);
                        $(this).find("td:eq(" + itemColIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ShowItemDetails('" + itemPK + "');\" > " + itemCode + " </a>  ");
                    }
                }
                //Line Color
                ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_LINE_COLOUR", tableID);
                if (ColIndex != null) {
                    var lineColor = GrandGrid.Utilities.GetColumnValue($(this), "CMP_LINE_COLOUR", tableID);
                    if (lineColor != "null") {
                        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE_TEXT", tableID);
                        if (ColIndex != null) {
                            $(this).find("td:eq(" + ColIndex + ")").addClass(lineColor);
                        }
                    }
                }
                specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.PRD_PURPOSE, grdID);
                reqSpec = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRD_PURPOSE, grdID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRD_PURPOSE, grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRD_PURPOSE, grdID);
                if (specColIndex != null && reqSpec != null) {
                    $(this).find("td:eq(" + specColIndex + ")").html(reqSpec);
                }
                else {
                    $(this).find("td:eq(" + specColIndex + ")").html("");
                }
                specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "PRD_QTY_REQUESTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "PRD_QTY_REQUESTED", $(this).parents("table:first").attr("id"));
                if (specColIndex != null && qty != null) {
                    $(this).find("td:eq(" + specColIndex + ")").html(addCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                else {
                    $(this).find("td:eq(" + specColIndex + ")").html("");
                }
            }

        });
    }

    if (grdID == "grdPuchasePendingList") {
        $("[id$=btnAddSelectedItems]").show();
        var colIndex = 0;
        var reqQty = "";
        var amount = 0;
        var strVal = "";
        var itemPK = 0;
        var itemCode = "";
        var spec = "";
        $("#grdPuchasePendingList tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REQUEST_QTY, "grdPuchasePendingList");
            reqQty = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.REQUEST_QTY, "grdPuchasePendingList");
            if (reqQty == "null")
                reqQty = 0;
            reqQty = parseFloat(reqQty) < 0 ? 0 : reqQty;
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" class=\"numeric input-w80\" id=\"txtReqQty_" + index + "\" value=\"" + parseFloat(reqQty).toFixed(QtyDec) + "\" tabIndex=\"11\" onchange=\"javascript:MakeNumric(this," + reqQty + ");\" />");
            }

            //Specification Text box adding
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REQUEST_SPEC, "grdPuchasePendingList");
            spec = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.REQUEST_SPEC, "grdPuchasePendingList");
            if (spec == "null" || spec == "undefined")
                spec = "";
            //Showing Lot no/description of material in PR Specification field based on configuration.
            var IsPackingMaterial = 0;
            IsPackingMaterial = (GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.ITM_IS_PM, grdID));
            if (($("[id$=hdfShowLotno]").val() == "1") && IsPackingMaterial == "1") {
                var LotNo = "", Desc = "";
                LotNo = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.SOD_LOT_NO, grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.SOD_LOT_NO, grdID);
                Desc = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.ITM_DESC, grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.ITM_DESC, grdID);
                if ($("[id$=hdfShowBothLotnoDescPM]").val() == "1") {//Show Lotno along with Description
                    if (LotNo == "") { spec = Desc; }
                    else { spec = Desc != "" ? Desc + "," + LotNo : LotNo; }
                }
                else {
                    spec = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.SOD_LOT_NO, grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.SOD_LOT_NO, grdID);
                }
                if (spec == "null") {
                    spec = "";
                }
            }
            else {
                var Desc = "";
                if (GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.ITM_DESC, grdID) != "null") {
                    Desc = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.ITM_DESC, grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.ITM_DESC, grdID);
                }
                spec = Desc;
            }
            //-----    
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtReqSpec_" + index + "\" value=\"" + spec + "\" tabIndex=\"11\" />");
            }
            //End Specification textbox
            if (isViewMode) {
                $(this).find("td:last").hide();
            }
            //Code for Popup Start
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_TEXT", "grdPuchasePendingList");
            if (colIndex != null) {
                itemPK = GrandGrid.Utilities.GetColumnValue($(this), "ITM_PK", $(this).parents("table:first").attr("id"));
                itemCode = GrandGrid.Utilities.GetColumnValue($(this), "ITM_TEXT", $(this).parents("table:first").attr("id"));
                if ($("[id$=hdfIsPacking]").val() == "1") {
                    if (itemCode.length > 56) {
                        // $(this).find("td:eq(" + colIndex + ")").html(itemCode + "<img id=\"IMG_ITEM_" + index + "\" onclick=\"javascript:ShowItemDetails('" + itemPK + "');\" class=\"icon-imgspace\" src=\"../Images/Classic/Icons/rate.png\" alt=\"Translate(Rate)\" title=\"Translate(Rate)\" style=\"cursor:pointer\" />");
                        $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ShowItemDetails('" + itemPK + "');\" > " + "<div tooltip=\"" + itemCode + "\">" + itemCode.substring(0, 56) + "...</div> </a>  ");
                    }
                    else {
                        $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ShowItemDetails('" + itemPK + "');\" > " + "<div tooltip=\"" + itemCode + "\">" + itemCode + "</div> </a>  ");
                    }
                }
                else {
                    if (itemCode.length > 45) {
                        $(this).find("td:eq(" + colIndex + ")").html("<div tooltip=\"" + itemCode + "\">" + itemCode.substring(0, 45) + "...</div> </a>  ");
                    }
                    else {
                        $(this).find("td:eq(" + colIndex + ")").html("<div tooltip=\"" + itemCode + "\">" + itemCode + "</div> </a>  ");
                    }
                }
            }

            //End
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.CURRENT_STOCK, "grdPuchasePendingList");
            if (colIndex != null) {
                amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.CURRENT_STOCK, $(this).parents("table:first").attr("id"));
                //                strVal = addCommas(parseFloat(amount).toFixed(QtyDec));
                strVal = parseFloat(amount).toFixed(QtyDec);
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.MAX_STOCK, "grdPuchasePendingList");
            if (colIndex != null) {
                amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.MAX_STOCK, $(this).parents("table:first").attr("id"));
                //strVal = addCommas(parseFloat(amount).toFixed(QtyDec));
                strVal = parseFloat(amount).toFixed(QtyDec);
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REORDER_LEVEL, "grdPuchasePendingList");
            if (colIndex != null) {
                amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.REORDER_LEVEL, $(this).parents("table:first").attr("id"));
                //strVal = addCommas(parseFloat(amount).toFixed(QtyDec));
                strVal = parseFloat(amount).toFixed(QtyDec);
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.PRE_REQUEST_QTY, "grdPuchasePendingList");
            if (colIndex != null) {
                amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.PRE_REQUEST_QTY, $(this).parents("table:first").attr("id"));
                //strVal = addCommas(parseFloat(amount).toFixed(QtyDec));
                strVal = parseFloat(amount).toFixed(QtyDec);
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.MINIMUM_STOCK, "grdPuchasePendingList");
            if (colIndex != null) {
                amount = GrandGrid.Utilities.GetColumnValue($(this), PurchaseRequestConfig.MINIMUM_STOCK, $(this).parents("table:first").attr("id"));
                //strVal = addCommas(parseFloat(amount).toFixed(QtyDec));
                strVal = parseFloat(amount).toFixed(QtyDec);
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
        });
        makeFixedHeader("grdPuchasePendingList", 150);
    }

    //**********#region grdPendingPOList********************
    if (grdID == "grdPendingPOList") {
        var colIndex = 0;
        var qty = 0;
        $("#grdPendingPOList tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POH_NO", grdID);
            if (colIndex != null) {
                PONo = GrandGrid.Utilities.GetColumnValue($(this), "POH_NO", grdID);
                POH_PK = GrandGrid.Utilities.GetColumnValue($(this), "POH_PK", grdID);
                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"Print_PO(" + POH_PK + ");\" > " + PONo + " </a>  "); //providing hyper link for PO Number
            }
        });
    }
    //************#endregion grdPendingPOList***********************

}

function CalculatePercentage() {
    AddValidations(5);
    if ($(document.forms[0]).valid()) {
        //        var ItemToOrder = $("#divData").data("ItemToOrder");
        //        GrandGrid.MakeGrid($("#grdPuchasePendingList"), 0, $("#divData").data("ItemToOrder"));
        var colIndex = 0;
        var reqQty = "";
        var percentage = 0;
        var threshold = 0;
        percentage = parseFloat($("[id$=PRH_PERCENTAGE_EXTRA]").val());
        percentage < 0 ? 0 : percentage;
        if ($("[id$=hdfIsIOSelected]").val() == 0) {
            percentage = 0;
        }
        $("#grdPuchasePendingList tr:has(td)").each(function (index) {
            reqQty = parseFloat($("#txtReqQty_" + index).val());
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), PurchaseRequestConfig.REQUEST_QTY, "grdPuchasePendingList");
            if (reqQty == "null")
                reqQty = 0;
            reqQty = parseFloat(reqQty) < 0 ? 0 : reqQty;
            if (percentage > 0.0) {
                reqQty = reqQty + (reqQty * percentage / 100);
            }
            if ($("[id$=hdfIsPacking]").val() == "1") {
                reqQty = Math.round(reqQty);
            }
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" class=\"numeric input-w80\" id=\"txtReqQty_" + index + "\" value=\"" + parseFloat(reqQty).toFixed(QtyDec) + "\" tabIndex=\"11\" onchange=\"javascript:MakeNumric(this," + reqQty + ");\" />");
            }
        });
    }
    return false;
}

function MakeNumric(txtReq, orgVal) {
    //<summary>Function used hide some fields</summary>

    var floatRegQty = /(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,3})?$/;
    if (!floatRegQty.test($(txtReq).val())) {
        $(txtReq).val(orgVal);
    }
}
function CalcCBM() {
    var opODL;
    var opODB;
    var opODH;
    var CBM = 0;
    if ($("[id$='IPD_OUTER_LENGTH']").val() == "" && $("[id$='IPD_OUTER_HEIGHT']").val() == "" && $("[id$='IPD_OUTER_BREADTH']").val() == "")
        CBM = 0;
    else {
        opODL = $("[id$='IPD_OUTER_LENGTH']").val();
        opODB = $("[id$='IPD_OUTER_BREADTH']").val();
        opODH = $("[id$='IPD_OUTER_HEIGHT']").val();
        CBM = (opODL * opODB * opODH) / (1000 * 1000 * 1000);
        CBM = Number(CBM).toFixed(3);
    }

    $("[id$='ITM_CBM']").val(CBM);

}
function ShowItemDetails(itemPK) {
    //Get Packing Details
    ClearMterialDtl();
    $.get(PurchaseRequestConfig.PackingMaterialDtlURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + itemPK, function (data) {
        try {


            if (data != "") {
                //------------if it is other than master carton then it should show only inner Dimensions(Bug ID: 7094)----------------------------------
                $("[id$=trOuterDimension]").hide();
                $("[id$=trOuterDimensionHdr]").hide();
                if (data != null && data.length > 0) {
                    switch (data[0].CON_DATA) {
                        case "MC":
                        case "SC":
                            $("[id$=trOuterDimension]").show();
                            $("[id$=trOuterDimensionHdr]").show();
                            break;
                        case "3":
                            break;
                    }
                }
                //------------END-------------------------------
                //Header
                $("input[id$=ITM_CODE]").val(data[0].ITM_CODE);
                $("input[id$=ITM_NAME]").val(data[0].ITM_NAME);
                if (data[0].ITM_DESC != "null")
                    $("textarea[id$=ITM_DESC]").val(data[0].ITM_DESC);
                $("select[id$=ITM_TYPE_TEXT]").val(data[0].IPD_TYPE_TEXT);
                $("input[id$=ITM_MIN_STK]").val(data[0].ITM_MIN_STK);
                if ($("input[id$=ITM_MIN_STK]").val() == "null") {
                    $("input[id$=ITM_MIN_STK]").val("0");
                }
                $("input[id$=ITM_MAX_STK]").val(data[0].ITM_MAX_STK);
                if ($("input[id$=ITM_MAX_STK]").val() == "null") {
                    $("input[id$=ITM_MAX_STK]").val("0");
                }
                $("input[id$=ITM_ROL_STK]").val(data[0].ITM_ROL_STK);
                if ($("input[id$=ITM_ROL_STK]").val() == "null") {
                    $("input[id$=ITM_ROL_STK]").val("0");
                }
                if (data[0].ITM_MOQ != "null")
                    $("input[id$=ITM_MOQ]").val(data[0].ITM_MOQ);
                else
                    $("input[id$=ITM_MOQ]").val("0");

                $("input[id$=txtITC_CAT]").val(data[0].ITC_NAME);
                if ($("input[id$=txtITC_CAT]").val() == "null") {
                    $("input[id$=txtITC_CAT]").val("0");
                }
                $("input[id$=txtUom]").val(data[0].UOM_NAME);
                if ($("input[id$=txtUom]").val() == "null") {
                    $("input[id$=txtUom]").val("");
                }
                $("input[id$=txt_TYPE_TEXT]").val(data[0].ITM_TYPE_TEXT);
                if ($("input[id$=txt_TYPE_TEXT]").val() == "null") {
                    $("input[id$=txt_TYPE_TEXT]").val("");
                }
                $("input[id$=txtType]").val(data[0].IPD_TYPE_TEXT);
                if ($("input[id$=txtType]").val() == "null") {
                    $("input[id$=txtType]").val("");
                }
                $("input[id$=txtAppliedFor]").val(data[0].IPD_CUSTOMER_TEXT);
                if ($("input[id$=txtAppliedFor]").val() == "null") {
                    $("input[id$=txtAppliedFor]").val("");
                }
                $("input[id$=IPD_INNER_LENGTH]").val(data[0].IPD_INNER_LENGTH);
                $("input[id$=IPD_INNER_HEIGHT]").val(data[0].IPD_INNER_HEIGHT);
                $("[id$=IPD_PK]").val(data[0].IPD_PK);

                $("input[id$=IPD_INNER_BREADTH]").val(data[0].IPD_INNER_BREADTH);
                $("input[id$=IPD_OUTER_LENGTH]").val(data[0].IPD_OUTER_LENGTH);
                $("input[id$=IPD_OUTER_HEIGHT]").val(data[0].IPD_OUTER_HEIGHT);
                $("input[id$=IPD_PLY]").val(data[0].IPD_PLY);
                $("input[id$=IPD_OUTER_BREADTH]").val(data[0].IPD_OUTER_BREADTH);
                $("input[id$=ITM_WEIGHT]").val(data[0].ITM_WEIGHT);
                $("input[id$=IPD_PAPER_COLOR]").val(data[0].IPD_PAPER_COLOR);
                $("input[id$=IPD_ART_WORK]").val(data[0].IPD_ART_WORK);
                CalcCBM();
                $("#fContainer_" + "fupUploader").html("");
                GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Material", false);
                if (data[0].DOC_PK != null) {
                    FileJson.FILELIST = new Array();
                    var obj = $("[id$=TEMPFILELIST]").val(JSON.stringify($("[id$=TEMPFILELIST]").val()));
                    FileJson.FILELIST.push(obj);
                    FileJson.FILELIST[0].DOC_PK = data[0].DOC_PK;
                    FileJson.FILELIST[0].DOC_TITLE = data[0].DOC_TITLE;
                    FileJson.FILELIST[0].DOC_TYPE = data[0].DOC_TYPE;
                    FileJson.FILELIST[0].DOC_NAME = data[0].DOC_NAME;
                    // FileJson.FILELIST[0].DOC_PATH = data[0].DOC_PATH;

                    FillFileDetails();
                }
                $("#divItemDetails").dialog("open");
                $("#divItemDetails").dialog({ "width": 900 });
            }
            else {
                GrandScriptUtils.ShowModal(PurchaseRequestConfig.NoDataFound, PurchaseRequestConfig.MessageBoxTitle);
            }
        } catch (err) {
            GrandScriptUtils.ShowModal(PurchaseRequestConfig.NoDataFound, PurchaseRequestConfig.MessageBoxTitle);
        }
    });


}

function ClearMterialDtl() {
    $("#fContainer_" + "fupUploader").html("");
    $("input[id$=ITM_CODE]").val("");
    $("input[id$=ITM_NAME]").val("");
    $("textarea[id$=ITM_DESC]").val("");
    $("select[id$=ITM_TYPE_TEXT]").val("");
    $("input[id$=ITM_MIN_STK]").val("");
    $("input[id$=ITM_MAX_STK]").val("");
    $("input[id$=ITM_ROL_STK]").val("");
    $("input[id$=ITM_MOQ]").val("");
    $("input[id$=txtITC_CAT]").val("");
    $("input[id$=txtUom]").val("");
    $("input[id$=txt_TYPE_TEXT]").val("");
    $("input[id$=txtType]").val("");
    $("input[id$=txtAppliedFor]").val("");
    $("input[id$=IPD_INNER_LENGTH]").val("");
    $("input[id$=IPD_INNER_HEIGHT]").val("");
    $("[id$=IPD_PK]").val("");
    $("input[id$=IPD_INNER_BREADTH]").val("");
    $("input[id$=IPD_OUTER_LENGTH]").val("");
    $("input[id$=IPD_OUTER_HEIGHT]").val("");
    $("input[id$=IPD_PLY]").val("");
    $("input[id$=IPD_OUTER_BREADTH]").val("");
    $("input[id$=ITM_WEIGHT]").val("");
    $("input[id$=IPD_PAPER_COLOR]").val("");
    $("input[id$=IPD_ART_WORK]").val("");
}

function FillFileDetails(flag) {
    ///<summary>function used to fill the file details</summary>
    var fupUploaderCntrlName = "fupUploader";
    if (flag == 1) {
        UPLOADFOLDER = UPLOADFOLDERPR;
        fupUploaderCntrlName = "fupUploaderPR";
    }
    if (FileJson.FILELIST.length > 0 && FileJson.FILELIST[0].DOC_NAME != null) {
        for (var index in FileJson.FILELIST) {

            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + fupUploaderCntrlName).append($(template).html());
        }
    }
}

function HideFields() {
    //<summary>Function used hide some fields</summary>

    $("[id$=MaterialCategory]").attr("disabled", "disabled");
    $("[id$=ItemCodeMaterial]").attr("disabled", "disabled");
    $("[id$=MaterialCategory]").autocomplete("option", "disabled", true);
    $("[id$=ItemCodeMaterial]").autocomplete("option", "disabled", true);
}

function ShowFields() {
    //<summary>Function used show some fields</summary>

    $("[id$=MaterialCategory]").removeAttr("disabled");
    $("[id$=ItemCodeMaterial]").removeAttr("disabled");
    $("[id$=MaterialCategory]").autocomplete("option", "disabled", false);
    $("[id$=ItemCodeMaterial]").autocomplete("option", "disabled", false);
}

function IsValidData() {
    //<summary>Function Used to check the all data required date entered correctly </summary>

    for (var i in PurchaseRequestConfig.PurchaseRequestList) {
        if (PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE == "") {
            return false;
        }
        else if (!CheckValidDate(PurchaseRequestConfig.PurchaseRequestList[i].PRD_REQD_DATE)) {
            return false;
        }
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

function HideReOrder() {
    //<summary>Function Used to Hide Reorder Panel </summary>

    $("#imgReOrderHide").hide();
    $("#imgReOrderShow").show();
    $("#divReOrderList").hide();
}

function ShowReOrder() {
    //<summary>Function Used to Show Reorder Panel </summary>

    $("#imgReOrderHide").show();
    $("#imgReOrderShow").hide();
    $("#divReOrderList").show();
}

function HideItemRequested() {
    //<summary>Function Used to Hide Item to be Requested Panel </summary>

    $("#imgItemRequestedHide").hide();
    $("#imgItemRequestedShow").show();
    $("#divItemsRequested").hide();
}

function ShowItemRequested() {
    //<summary>Function Used to Show Item to be Requested Panel </summary>

    $("#imgItemRequestedHide").show();
    $("#imgItemRequestedShow").hide();
    $("#divItemsRequested").show();
}

function StoreDataClear() {
    //<summary>Function used to clear the store data only</summary>

    PurchaseRequestConfig.PurchaseRequestList = new Array();
    $("#divData").data("PurchaseRequestData", PurchaseRequestConfig.PurchaseRequestList);
    GrandGrid.MakeGrid($("#grdPurchaseRequest"), 0, PurchaseRequestConfig.PurchaseRequestList);
    $(PurchaseRequestConfig.tdset).insertAfter($("#PurchaseRequestInsert").find("tr:eq(0)"));
    BindGrid();
    $("#divPurchaseRequestInsert").show();
    RemoveValidations();
    //    //New
    //    var dept = $("[id$=Store]").val();
    //    $("[id$=hdfDeptID]").val(dept); 
    //    $("[id$=hdfStorePK]").val(dept); 

    //    $.get(PurchaseRequestConfig.GetProcessID + dept + "&Path=" + PurchaseRequestConfig.PURCHASEREQUESTURL, function (data) {
    //        if (data) {
    //            $("[id$=hdfProcessID]").val(data[0].PROCESS_PK);
    //        }
    //    });
    //   window.location = PurchaseRequestConfig.PURCHASEREQUESTURL + "?StoreID=" + dept;



}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case PurchaseRequestConfig.DELETE:
            DeletePurchaseRequest();
            break;
        case PurchaseRequestConfig.SAVE:
            window.location = $("[id$=hdfBackUrl]").val(); //PurchaseRequestConfig.PurchaseListUrl;
            break;
        case PurchaseRequestConfig.INBOX:
            window.location = PurchaseRequestConfig.InboxURL;
            break;
        case PurchaseRequestConfig.ADD:
            AddPurchaseRequestList(GrandScriptUtils.AddRow, true, true);
            break;
        case PurchaseRequestConfig.ADDMANY:
            AddPRList(true, true);
            break;
        case PurchaseRequestConfig.ADDMANY:
            AddPurchaseRequest(true);
            break;
        case PurchaseRequestConfig.ADDITEM:
            AddPurchaseRequest(true);
            break;
        case PurchaseRequestConfig.CLEARITEM:
            ChangeIO(true);
            break;
    }
}

function ModalCancel(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case PurchaseRequestConfig.CLEARITEM:
            $("[id$=PRH_SO_HDR]").val($("[id$=hdfCurIONumber]").val());
            break;
    }
}

function makeFixedHeader(grdID, Height) {

    //    $("#" + grdID).css("height", Height + "px");
    var clonedOne = $("#" + grdID).clone(true);
    $(clonedOne).attr("id", grdID + "1");
    $(clonedOne).find("table tbody").remove();
    $(clonedOne).css({
        "left": $("#" + grdID).position().left,
        "top": $("#" + grdID).position().top - 2,
        "position": "absolute",
        "z-index": "1000",
        "width": $("#" + grdID + " table").innerWidth(),
        "height": "auto"
    });
    $(clonedOne).find("table:eq(0)").css("width", "100%");
    $(clonedOne).appendTo(".main:eq(0)");

    $("#" + grdID + " table:eq(0) tbody tr:eq(0) td").each(function (e) {
        $(clonedOne).find("table:eq(0) thead th:eq(" + e + ")").width(($.browser.msie ? ($(this).width() + (1 + e)) : $(this).width()));
    });

}
//#endregionf
//Comma Separation for Quantity & Amount Based on Configuration(Table)
function addCommas(x) {
    //Seperates the components of the number
    var n = x.toString().split(".");
    //Comma-fies the first part
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return n.join(".");
    //    var curGroup1 = 3;
    //    var curGroup2 = 3;
    //    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
    //        curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
    //    }
    //    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
    //        curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
    //    }

    //    var s = n.split('.')[1];
    //    (s) ? s = "." + s : s = "";
    //    n = n.split('.')[0];
    //    if (n.length > curGroup1) {
    //        s = "," + n.substr(n.length - curGroup1, curGroup1) + s;
    //        n = n.substr(0, n.length - curGroup1)
    //        while (n.length > curGroup2) {
    //            s = "," + n.substr(n.length - curGroup2, curGroup2) + s;
    //            n = n.substr(0, n.length - curGroup2)
    //        }
    //    }

    //    return n + s
}
function RemoveAllValidations() { //MakeFileUploader function in GrandScriptUtils requires RemoveAllVaidations()
    RemoveValidations();
}
function HideAttachment() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgHideAttachment").hide();
    $("#imgShowAttachment").show();
    $("#divAttachmentPR").hide();
}
function ShowAttachment() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imgHideAttachment").show();
    $("#imgShowAttachment").hide();
    $("#divAttachmentPR").show();
}
//For Showing continue popup(same item with same required by date)
function ConfirmSameItemDate() {

    var msgTitle;
    var msg;
    msgTitle = PurchaseRequestConfig.Confirmation;
    msg = PurchaseRequestConfig.SameItemDateMsg;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfIsContSameItemDate]").val(1);
                $(this).dialog("close");
                $("[id$=imbAddNew]").click();
            },
            Cancel: function (e) {
                $("[id$=hdfIsContSameItemDate]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

function ChangeROLMSCat() {
    if ($("[id$=rbtnCategory]").is(":checked")) {
        $("[id$=spanItemCat]").show();
        $("[id$=txtItemCategory]").attr("disabled", false);
        GrandScriptUtils.MakeAutoComplete("txtItemCategory", PurchaseRequestConfig.MaterialCategroyDeptURL + $("[id$=hdfDeptID]").val(), "hdfItemCategoryPK", true, false, "BizUnitPk", true); //textbox Below ROL / MSL / Category    
    }
    else {
        $("[id$=spanItemCat]").hide();
        $("[id$=txtItemCategory]").val("");
        $("[id$=hdfItemCategoryPK]").val("0");
        $("[id$=txtItemCategory]").next("a").remove();
        $("[id$=txtItemCategory]").attr("disabled", true);
    }
}

function ShowItems() {
    ShowReOrder();
    BindGrid();
}
function HideReOrderHeader() {
    //<summary>Function Used to Hide Reorder Panel </summary>

    $("#imgReOrderHideHeader").hide();
    $("#imgReOrderShowHeader").show();
    $("#divReOrderList").hide();
}

function ShowReOrderHeader() {
    //<summary>Function Used to Show Reorder Panel </summary>

    $("#imgReOrderHideHeader").show();
    $("#imgReOrderShowHeader").hide();
    $("#divReOrderList").show();
}

function WkfPurchaseSubmit() {
    var IsWkfSetting = $("[id$=hdfIsWkfSettingPostBackReqd]").val();
    if (parseInt(IsWkfSetting) == 1) {
        AddValidations(6);
        if ($(document.forms[0]).valid()) {
            WkfSubmit();
        }
    }
    else {
        WkfSubmit();
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

function CancelPR() {
    window.location = $("[id$=hdfBackUrl]").val(); //PurchaseRequestConfig.BACKURL;
    return false;
}
function IsHavePurpose() {
    //<summary>Function Used to check purpose entered in all rows </summary>
    for (var i in PurchaseRequestConfig.PurchaseRequestList) {
        if (PurchaseRequestConfig.PurchaseRequestList[i].PRD_PURPOSE == "") {
            return false;
        }
    }
    return true;
}
function Print_PO(poID) {
    var url = PurchaseRequestConfig.PURCHASEREQUESTREPORTURL + "?ID=" + poID + "&APPTYPE=PO";
    OpenPDF(url);
}