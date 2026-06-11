/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="GrandTreeMulti.js" />

///#region -------------- Global Variable -------
var materialID = 0;
var relMaterialID = 0;
var vendorID = 0;
var tdset = "";
var curTR = null;
var mappingJson = new Object();
var uomName = "";
var QCInspVal = 0;
var QtyDec, AmtDec;
///#endregion
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Material";
//#region ------- Configuration Section -------- 
var MaterialMaster = {
    //Url MaterialMaster.GetMaterialCategoryTreeURLdBind
    MaterialAutoCompleteURL: "MaterialManagement.do?Action=GetSearchTypeValues&AUTOSEARCH=1&SBUPk=",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialCodeNameByCategory&AUTOSEARCH=1&SBUPk=",
    GetMaterialDetailByCategory: "MaterialManagement.do?Action=GetMaterial&SBUPk=",
    GetRelatedMaterial: "MaterialManagement.do?Action=GetRelatedMaterial&SBUPk=",
    GetBOMaterial: "MaterialManagement.do?Action=GetBOMaterial&SBUPk=",

    GetStores: "MaterialManagement.do?Action=GetMaterialStores&SBU=",
    PackingMaterialDtlURL: "MaterialManagement.do?Action=GetPakingMaterialDetails&SBUPk=",
    GetMaterialCategoryTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryTypeWithoutSemiAndFinished&SBUPk=",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryTypeListWithoutSemiAndFinished&SBUPk=",
    GetMaterialCategoryDetailsURL: "CommonManagement.do?Action=GetCategoryValue&CategoryPK=",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    FillPackingType: "CommonManagement.do?Action=GetPackingType&BizUnit=",
    FillClassification: "CommonManagement.do?Action=GetClassification&BizUnit=",
    FillPackingDtl: "CommonManagement.do?Action=GetPackingTypeDtl&BizUnit=",
    FillCustomers: "CommonManagement.do?Action=GetCustomers&BizUnit=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    FillVendorUOMDropdownURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial",
    MaterialSaveURL: "MaterialManagement.do?Action=SavePage",
    //    MaterialBindGridURL: "MaterialManagement.do?Action=GetMaterialTypeList&Status=",
    MaterialBindGridURL: "MaterialManagement.do?Action=GetMaterialTypeList",
    MaterialDeleteURL: "MaterialManagement.do?Action=DeleteMaterial&MaterialID=",
    FillVendorDropdownURL: "VendorManagement.do?Action=GetTypeVendorsActive&SBUPk=",
    GetCurrency: "CommonManagement.do?Action=GetCurrencyList&SBU=",
    VendorSaveURL: "MaterialManagement.do?Action=SaveVendorPage",
    VendorGetXmlURL: "MaterialManagement.do?Action=GetVendorDetails&ItemPk=",
    GetCategoryTaxDiscount: "TaxSettings.do?Action=GetActiveCategoryValue&CategoryPK=",
    GetCategoryTaxDiscountDateBase: "TaxSettings.do?Action=GetActiveCategoryDateValue&CategoryPK=",
    BACKURL: "../AccountManagement/WorkflowInbox.aspx",
    GetNextProductCode: "MaterialManagement.do?Action=GetNextProductCode&BizUnit=",
    MaterialCategroyDeptURL: "MaterialCategory.do?Action=GetMaterialCategoryListAuto",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=",
    FillUOMConversionsURL: "UOMManagement.do?Action=GetUOMCONVUOMPK&UOMId=",
    FillHSNDropdownURL: "MaterialManagement.do?Action=GetGSTClassficationList&PK=",
    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    SaveVendorCommand: "SAVEVENDOR",
    DeleteVendorCommand: "DELETEVENDOR",
    DeleteCommand: "DELETE",
    RelDeleteCommand: "DELETERELATED",
    BOMDeleteCommand: "DELETEBOM",
    EditCommand: "EDIT",
    ShowRateCommand: "SHOWRATE",
    DeleteMessageCommand: "DELETEMSG",
    RelDeleteMessageCommand: "RELDELETEMSG",
    BOMDeleteMessageCommand: "BOMDELETEMSG",
    DeleteVendorMessageCommand: "DELETEMSGVENDOR",
    TaxDelete: "TAXDELETE",
    Param: "&MatCagID=",
    Param1: "&MaterialPK=",
    SpecialCond: "&specialCond=PUR", //For Purchase Order

    //Fields
    MaterialDetailId: "ITM_PK",
    MaterialCode: "ITM_CODE",
    Moq: "ITM_MOQ",
    MaxOq: "ITM_MAX_OQ",
    MaterialName: "ITM_NAME",
    MaterialCategory: "ITC_PK",
    MaterialType: "ITM_TYPE",
    MaterialDesc: "ITM_DESC",
    MaterialMinStockLevel: "ITM_MIN_STK",
    MaterialMaxStockevel: "ITM_MAX_STK",
    MaterialReorderLevel: "ITM_ROL_STK",
    RequireQCInspection: "ITM_NEED_QC_INSP",
    IsWorkOrder: "ITM_IS_WORK_ORDER",
    ITC_IS_STOCK:"ITC_IS_STOCK",
    RequireBatchStk: "ITM_NEED_BATCH_STK",
    RequireProductMapping: "ITM_IS_LINKED_ITEM",
    InactivePeriod: "ITM_INACTIVE_PERIOD",
    MaterialActive: "ITM_ACTIVE",
    ConversionRequired: "ITM_IS_CONVERSION_REQD",
    IsAsset:"ITM_IS_ASSET",
    GstCodeVal: "ITM_GST_CLASS",
    PHR: "ITM_PHR",
    TSC: "ITM_TSC",
    BATCHCODE: "ITM_BATCH_CODE",
    MaterialMOU: "UOM_PK",
    MaterialMOUPurchase: "ITM_UOM_PURCHASE",
    MaterialMOUSale: "ITM_UOM_SALE",
    SEMIFINISHEDGOOD: "SEMI FINISHED GOOD",
    FINISHEDGOOD: "FINISHED GOOD",
    //vendor
    VendorID: "ITV_VENDOR",
    VendorMaterialName: "ITV_NAME",
    VendorPrice: "ITV_PRICE",
    VendorCurrencyID: "ITV_CURRENCY",
    VendorMOQ: "ITV_MOQ",
    VendorMOQUOMID: "ITV_MOQ_UOM",
    VendorTAX: "ITV_TAX_PERC",
    //New Start
    VendorDiscount: "ITV_DISC_PERC",
    ITV_LEAD_TIME: "ITV_LEAD_TIME",
    //New End
    IVT_SL_NO: "IVT_SL_NO",
    IVT_TAX: "IVT_TAX",
    ITV_DISC_PERC: "ITV_DISC_PERC",
    ITV_TAX_PERC: "ITV_TAX_PERC",
    ITV_SL_NO: "ITV_SL_NO",
    Classification: "IPD_CLASSIFICATION",

    //Properties
    ItemTaxPK: 0,
    EditTax: 0,
    ServiceVal: 6,
    PouchType: "301",

    //Messages
    RecordExist: "Translate(AlreadyExists)",
    NotAllowSameMaterial: "Translate(NotAllowSameMaterial)",
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    MaterialSaveMessage: "Translate(MaterialDetailsSavedSuccesfully)",
    ServiceSaveMessage: "Translate(ServiceDetailsSavedSuccesfully)",
    AssetSaveMessage: "Translate(AssetDetailsSavedSuccesfully)",
    NewVerionCreated: "Translate(NewVerionCreated)",
    MaterialUpdateMessage: "Translate(MaterialDetailsUpdatedSuccesfully)",
    MaterialCodeExistsMessage: "Translate(CodeAlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    MaterialDeleteMessage: "Translate(MaterialDetailsDeletedSuccesfully)",
    ServiceDeleteMessage: "Translate(ServicelDetailsDeletedSuccesfully)",
    AssetDeleteMessage: "Translate(AssetDetailsDeletedSuccesfully)",
    MaterialUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    VendorExistsMessage: "Translate(AlreadyExists)",
    VendorSaveMessage: "Translate(VendorDetailsSavedSuccesfully)",
    VendorUpdateMessage: "Translate(VendorDetailsUpdatedSuccesfully)",
    UsedMaterialInAnotherPlace: "Translate(UsedMaterialInAnotherPlace)",
    NoRecordFound: "Translate(NoRecordFound)",

    //Validation messages
    MaterialCodeValidation: "Translate(PleaseProvideMaterialCode)",
    MaterialNameValidation: "Translate(PleaseProvideMaterialName)",
    MaterialCategoryValidation: "Translate(PleaseSelectCategory)",
    MaterialValidation: "Translate(PleaseselectaProduct)",
    PcsInKGValidation: "Translate(EnterPcsInKg)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    MaterialUOMValidation: "Translate(PleaseSelectUOM)",
    MaterialUOMPurchaseValidation: "Translate(PleaseSelectPurchaseUOM)",
    MaterialUOMSalesValidation: "Translate(PleaseSelectSalesUOM)",
    MaterialMinStockValidation: "Translate(PleaseProvideMinStockLevel)",
    ValidReqMOQ: "Translate(ValidReqMOQ)",
    ValidReqPHR: "Translate(ValidReqPHR)",
    ValidReqTSC: "Translate(ValidReqTSC)",
    MaterialMaxStockValidation: "Translate(PleaseProvideMaxStockevel)",
    MaterialReValidation: "Translate(PleaseProvideMaterialReorderLevel)",
    MaterialReorderlevelBetweenMinAndMax: "Translate(ReorderlevelBetweenMinAndMax)",
    MinStockLevelLessthanMax: "Translate(MinStockLevelLessthanMax)",
    EnterBatchCode: "Translate(EnterBatchCode)",
    UOMMismatch: "Translate(UOMMismatch)",
    IsAssetMismatch: "Translate(IsAssetMismatch)",
    InsertionEntryFailed: "Translate(InsertionEntryFailed)",
    //    EnterTHREEDecimal: "Translate(EnterThreeDigitDecimal)",
    //    EnterTWODecimal: "Translate(EnterTwoDigitDecimal)",

    //Vendor validation messages
    VendorNameValidation: "Translate(SelectVendor)",
    VendorCurrencyValidation: "Translate(SelectCurrency)",
    VendorMaterialNameValidation: "Translate(EnterVendorMaterialName)",
    VendorMaterialPriceValidation: "Translate(EnterStdPrice)",
    VendorMOQValidation: "Translate(EnterMinimumOrderedQuantity)",
    VendorTAXValidation: "Translate(EnterTAX)",
    //NEw Start
    VendorDiscountValidation: "Translate(EnterDiscount)",
    VendorLeadDaysValidation: "Translate(EnterLeadDays)",
    TypeAlreadyAdded: "Translate(TypeAlreadyAdded)",
    //New End

    StoreMappSaveMessage: "Translate(StoreMappingSavedSuccesfully)"

}
//#endregion

///#region------- Initialization Section --------

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != MaterialMaster.TextZero);
}, 'Translate(Pleaseselectanoption)');

$.validator.addMethod("TwoDecimal", function (value) {
    return /^\d{1,2}(\.\d{1,2})?$/.test(value);
}, "Max 2 Numeric & 2 decimal allowed");

$.validator.addMethod("SetTwoDecimal", function (value) {
    return /^\d{1,8}(\.\d{1,2})?$/.test(value);
}, "Max 8 Numeric & 2 decimal allowed");

//$.validator.addMethod("selectAutotypeText", function (value, element) {
//    return ($(element).val() != typeText);
//}, "Translate(Pleaseselectanoption)");

$.validator.addMethod("selectAuto", function (value, element) {
    return ($(element).val() != "Select/Type");
}, "Translate(Pleaseselectanoption)");

//$.validator.addMethod("twodecimal", function (value) {
//    //    return /^\d{1,3}(\.\d{0,3})?$/.test(value);
//    return /^\d+(\.\d{1,2})?$/.test(value);
//    //    return /^\d{0,3}(\.\d{0,3})?$/.test(value);
//}, MaterialMaster.EnterTWODecimal);

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    $("[id$=UOM_PK]").change(function () {
        uomName = $('option:selected', this).text();
    });

    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();

    //vendor
    //Initailizing Requisition ProductGrid
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, dummyObj);
    //initialize Requisition Object
    mappingJson = $.parseJSON($("[id$=MappingDetailsList]").val());
    $("#divMappingData").data("MappingData", mappingJson);
    // Create Tabs
    //Page Initial condtions
    PageInit();
    if ($("[id$=MaterialReferenceID]").val() != "0") {
        FillMaterailObject();
    }
    //Modal popup and tree view default settings
    $("#divCategory").dialog({ autoOpen: false });
    ShowHideAdvancedSearch(1);
    $("input[id$=InactivePeriod]").ForceNumericOnly();
});

function ShowSaveImage() {
    $("[id$=AddressSave]").hide();
    $("[id$=btnSave]").show();
    if ($("[id$=ITM_SET]").val() == 3)
        $("[id$=divPackingMaterial]").show();
}
function CancelMaterialMaster() {
    window.location = MaterialMaster.BACKURL;
    return false;
}
function HideSaveImage(aVendor) {

    var liIndex = $("#tabs").tabs("option", "disabled");
    if (liIndex == 0) {
        $("[id$=btnSave]").hide();
        $("[id$=AddressSave]").show();
        if ($("[id$=ITM_SET]").val() == 3)
            $("[id$=divPackingMaterial]").hide();
    }
}
function HideStore(aStore) {

    var liIndex = $("#tabs").tabs("option", "disabled");
    $("[id$=divPackingMaterial]").hide();
    if (liIndex == 0) {
        $("[id$=AddressSave]").hide();
        $("[id$=btnSave]").show();
        if ($("[id$=ITM_SET]").val() == 3)
            $("[id$=divPackingMaterial]").hide();
    }
    $("#trvStores span").each(function () {     //For avoiding changing of mouse pointer to hand sign  in store mapping tab                           
        $(this).css('cursor', 'default');
    });
}
function InitRelMaterial() {
    GrandScriptUtils.MakeAutoCompleteLimitLen("RelatedMaterialItem", MaterialMaster.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + $("[id$=ITC_PK]").val(), "R_ITEM", true, false, "MaterialType", true, "", "", "", $("[id$=AutoStartValue]").val());
}
function DeleteRelDetails() {
    MaterialMaster.RelatedMaterialList = $("#divData").data("RelatedMaterialData");
    for (var i in MaterialMaster.RelatedMaterialList) {
        if (MaterialMaster.RelatedMaterialList[i].ITM_PK == relMaterialID) {
            MaterialMaster.RelatedMaterialList.splice(i, 1);
            break;
        }
    }
    $("#divData").data("RelatedMaterialData", MaterialMaster.RelatedMaterialList);
    GrandGrid.MakeGrid($("#grdRelMaterial"), 0, MaterialMaster.RelatedMaterialList);
}
function DeleteBOMDetails() {
    MaterialMaster.BOMaterialList = $("#divData").data("BOMaterialData");
    for (var i in MaterialMaster.BOMaterialList) {
        if (MaterialMaster.BOMaterialList[i].ITM_PK == relMaterialID) {
            MaterialMaster.BOMaterialList.splice(i, 1);
            break;
        }
    }
    $("#divData").data("BOMaterialData", MaterialMaster.BOMaterialList);
    GrandGrid.MakeGrid($("#grdBOMaterial"), 0, MaterialMaster.BOMaterialList);
}
function GetRelatedItemMap() {
    var ObjRelMaterial = new Array();
    var obj = new Object();
    MaterialMaster.RelatedMaterialList = $("#divData").data("RelatedMaterialData");
    for (var i in MaterialMaster.RelatedMaterialList) {
        obj = new Object();
        obj.IAM_ALT_ITEM = MaterialMaster.RelatedMaterialList[i].ITM_PK;
        ObjRelMaterial.push(obj);
    }
    $("#divData").data("RelatedMaterialData", null);
    return ObjRelMaterial;
}

function GetBOMaterialMap() {
    var ObjBOMaterial = new Array();
    var obj = new Object();
    MaterialMaster.BOMaterialList = $("#divData").data("BOMaterialData");
    for (var i in MaterialMaster.BOMaterialList) {
        obj = new Object();
        obj.IBM_ALT_ITEM = MaterialMaster.BOMaterialList[i].ITM_PK;
        obj.IBM_CONV = MaterialMaster.BOMaterialList[i].IBM_CONV;
        ObjBOMaterial.push(obj);
    }
    $("#divData").data("BOMaterialData", null);
    return ObjBOMaterial;
}
function BindRelatedMaterial() {
    MaterialMaster.RelatedMaterialList = new Array();
    GrandGrid.MakeGrid($("#grdRelMaterial"), 0, MaterialMaster.RelatedMaterialList);
    $("#divData").data("RelatedMaterialData", MaterialMaster.RelatedMaterialList);
    $.get(MaterialMaster.GetRelatedMaterial + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("input[id$=MaterialDetailId]").val(), function (data) {
        if (data != null && data.length > 0) {
            for (var i in data) {
                MaterialMaster.RelatedMaterialObj = new Object();
                MaterialMaster.RelatedMaterialObj.ITM_PK = data[i].ITM_PK;
                MaterialMaster.RelatedMaterialObj.ITM_CODE = data[i].ITM_CODE;
                MaterialMaster.RelatedMaterialObj.ITM_TEXT = data[i].ITM_TEXT;
                MaterialMaster.RelatedMaterialObj.ITC_NAME = data[i].ITC_NAME;
                MaterialMaster.RelatedMaterialObj.UOM_NAME = data[i].UOM_NAME;
                MaterialMaster.RelatedMaterialList.push(MaterialMaster.RelatedMaterialObj);
            }
            $("#divData").data("RelatedMaterialData", MaterialMaster.RelatedMaterialList);
            GrandGrid.MakeGrid($("#grdRelMaterial"), 0, MaterialMaster.RelatedMaterialList);

        }
    });
    return false;
}

function BindBOMaterial() {
    MaterialMaster.BOMaterialList = new Array();
    GrandGrid.MakeGrid($("#grdBOMaterial"), 0, MaterialMaster.BOMaterialList);
    $("#divData").data("BOMaterialData", MaterialMaster.BOMaterialList);
    $.get(MaterialMaster.GetBOMaterial + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("input[id$=MaterialDetailId]").val(), function (data) {
        if (data != null && data.length > 0) {
            for (var i in data) {
                MaterialMaster.BOMaterialObj = new Object();
                MaterialMaster.BOMaterialObj.ITM_PK = data[i].ITM_PK;
                MaterialMaster.BOMaterialObj.ITM_CODE = data[i].ITM_CODE;
                MaterialMaster.BOMaterialObj.ITM_TEXT = data[i].ITM_TEXT;
                MaterialMaster.BOMaterialObj.ITC_NAME = data[i].ITC_NAME;
                MaterialMaster.BOMaterialObj.UOM_NAME = data[i].UOM_NAME;
                MaterialMaster.BOMaterialObj.IBM_CONV = data[i].IBM_CONV;
                MaterialMaster.BOMaterialList.push(MaterialMaster.BOMaterialObj);
            }
            $("#divData").data("BOMaterialData", MaterialMaster.BOMaterialList);
            GrandGrid.MakeGrid($("#grdBOMaterial"), 0, MaterialMaster.BOMaterialList);

        }
    });
    return false;
}
function AddBOMDetails() {
    AddValidations(5);
    if ($(document.forms[0]).valid()) {
        if ($("input[id$=MaterialDetailId]").val() == $("[id$=B_ITEM]").val()) {//Base Item not allowed to map as relatted item
            GrandScriptUtils.ShowModal(MaterialMaster.NotAllowSameMaterial, MaterialMaster.MessageBoxTitle);
            return false;
        }
        var categoryID = 0;
        categoryID = $("select[id$=ITC_BOM_CAT]").val() == null ? 0 : $("select[id$=ITC_BOM_CAT]").val();
        $.get(MaterialMaster.GetMaterialDetailByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Active=1" + "&MaterialID=" + $("[id$=B_ITEM]").val(), function (data) {
            if (data != null && data.length > 0) {
                if (CheckItemExists()) {
                    GrandScriptUtils.ShowModal(MaterialMaster.RecordExist, MaterialMaster.MessageBoxTitle);
                    return false;
                }
                else {
                    MaterialMaster.BOMaterialList = $("#divData").data("BOMaterialData");
                    if (MaterialMaster.BOMaterialList == null)
                        MaterialMaster.BOMaterialList = new Array();
                    MaterialMaster.BOMaterialObj = new Object();
                    MaterialMaster.BOMaterialObj.ITM_PK = data[0].ITM_PK;
                    MaterialMaster.BOMaterialObj.ITM_CODE = data[0].ITM_CODE;
                    MaterialMaster.BOMaterialObj.ITM_TEXT = data[0].ITM_TEXT;
                    MaterialMaster.BOMaterialObj.ITC_NAME = data[0].ITC_NAME;
                    MaterialMaster.BOMaterialObj.UOM_NAME = data[0].UOM_NAME;
                    MaterialMaster.BOMaterialObj.IBM_CONV = $("[id$=txtPcsInKG]").val();

                    MaterialMaster.BOMaterialList.push(MaterialMaster.BOMaterialObj);

                    $("#divData").data("BOMaterialData", MaterialMaster.BOMaterialList);
                    GrandGrid.MakeGrid($("#grdBOMaterial"), 0, MaterialMaster.BOMaterialList);
                }
            }
            InitBOMaterial();
        });
       
    }
    return false;
}
function AddMaterialDetails() {
    if ($("input[id$=MaterialDetailId]").val() == $("[id$=R_ITEM]").val()) {//Base Item not allowed to map as relatted item
        GrandScriptUtils.ShowModal(MaterialMaster.NotAllowSameMaterial, MaterialMaster.MessageBoxTitle);
        return false;
    }
    var categoryID = 0;
    categoryID = $("[id$=ITC_PK]").val() == null ? 0 : $("[id$=ITC_PK]").val();
    $.get(MaterialMaster.GetMaterialDetailByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Active=1" + "&MaterialID=" + $("[id$=R_ITEM]").val(), function (data) {
        if (data != null && data.length > 0) {
            if (CheckItemExists()) {
                GrandScriptUtils.ShowModal(MaterialMaster.RecordExist, MaterialMaster.MessageBoxTitle);
                return false;
            }
            else {
                MaterialMaster.RelatedMaterialList = $("#divData").data("RelatedMaterialData");
                if (MaterialMaster.RelatedMaterialList == null)
                    MaterialMaster.RelatedMaterialList = new Array();
                MaterialMaster.RelatedMaterialObj = new Object();
                MaterialMaster.RelatedMaterialObj.ITM_PK = data[0].ITM_PK;
                MaterialMaster.RelatedMaterialObj.ITM_CODE = data[0].ITM_CODE;
                MaterialMaster.RelatedMaterialObj.ITM_TEXT = data[0].ITM_TEXT;
                MaterialMaster.RelatedMaterialObj.ITC_NAME = data[0].ITC_NAME;
                MaterialMaster.RelatedMaterialObj.UOM_NAME = data[0].UOM_NAME;

                MaterialMaster.RelatedMaterialList.push(MaterialMaster.RelatedMaterialObj);

                $("#divData").data("RelatedMaterialData", MaterialMaster.RelatedMaterialList);
                GrandGrid.MakeGrid($("#grdRelMaterial"), 0, MaterialMaster.RelatedMaterialList);
            }
        }
    });
    InitRelMaterial();
    return false;
}
function CheckItemExists() {
    //<summary>function used to check whether item already exists.</summary>
    var flag = false;
    MaterialMaster.RelatedMaterialList = $("#divData").data("RelatedMaterialData");
    for (var i in MaterialMaster.RelatedMaterialList) {
        if (MaterialMaster.RelatedMaterialList[i].ITM_PK == $("[id$=R_ITEM]").val()) {
            flag = true;
            break;
        }
    }

    return flag;
}


function ShowBOMaterial(aBOMaterial) {

    //    GrandGrid.MakeGrid($("#grdRelMaterial"), 0, new Array());
    //    GrandGrid.Utilities.ResetGrid(true, "grdRelMaterial");
    InitRelMaterial();
    BindBOMaterial();
    var liIndex = $("#tabs").tabs("option", "disabled");
    $("[id$=divPackingMaterial]").hide();
    if (liIndex == 0) {
        $("[id$=AddressSave]").hide();
        $("[id$=btnSave]").show();
        if ($("[id$=ITM_SET]").val() == 3)
            $("[id$=divPackingMaterial]").hide();
    }
}
function ShowRelatedMaterial(aRMaterial) {

    //    GrandGrid.MakeGrid($("#grdRelMaterial"), 0, new Array());
    //    GrandGrid.Utilities.ResetGrid(true, "grdRelMaterial");
    InitRelMaterial();
    BindRelatedMaterial();
    //    MaterialMaster.RelatedMaterialList = $("#divData").data("RelatedMaterialData");
    //    if (MaterialMaster.RelatedMaterialList == null)
    //        MaterialMaster.RelatedMaterialList = new Array();
    //    GrandGrid.MakeGrid($("#grdRelMaterial"), 0, MaterialMaster.RelatedMaterialList);

    var liIndex = $("#tabs").tabs("option", "disabled");
    $("[id$=divPackingMaterial]").hide();
    if (liIndex == 0) {
        $("[id$=AddressSave]").hide();
        $("[id$=btnSave]").show();
        if ($("[id$=ITM_SET]").val() == 3)
            $("[id$=divPackingMaterial]").hide();
    }
}
function FillStoreTree(itemPk) {
    //<summary>Function Used to Fill Menu Details to Tree View </summary>
    var sbuPK = $("[id$=BizUnitPk]").val();
    SetTreeHeaderStructure("trvStores", MaterialMaster.GetStores + sbuPK + "&ItemPK=" + itemPk, "Stores", true, false, "", true);
    MakeMultiTree();
    // call the function to bind tree view
}

function FillMaterailObject() {
    var data = $.parseJSON($("[id$=MaterialDetailsObj]").val());
    if (data != null && data.length > 0) {
        $("input[id$=ITM_CODE]").val(data[0].ITM_CODE);
        $("input[id$=ITM_NAME]").val(data[0].ITM_NAME);
        $("select[id$=ITC_PK]").val(data[0].ITM_CATEGORY);
        $("input[id$=MaterialDetailId]").val(data[0].ITM_PK);
        $("[id$=MaterialName]").html(data[0].ITM_NAME);
        $("textarea[id$=ITM_DESC]").html(data[0].ITM_DESC);
        $("select[id$=ITM_TYPE_TEXT]").val(data[0].ITM_TYPE);
        $("input[id$=ITM_MIN_STK]").val(data[0].ITM_MIN_STK);
        $("input[id$=ITM_MAX_STK]").val(data[0].ITM_MAX_STK);
        $("input[id$=ITM_ROL_STK]").val(data[0].ITM_ROL_STK);
        $("input[id$=InactivePeriod]").val(data[0].ITM_INACTIVE_PERIOD);
        //changing mode to material lising
        AddNew();
        FillCategory(data[0].ITM_CATEGORY, data[0].ITM_UOM);
        //FillUOM(data[0].ITM_CATEGORY, data[0].ITM_UOM);

        //#region --------------- Fill File Upload Details----------------------------------
        // GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Material", true);
        if (!($.isArray(data.FILELIST))) {
            if (data.FILELIST != undefined) {
                objArray = data.FILELIST;
                FileJson.FILELIST = new Array();
                FileJson.FILELIST.push(objArray);
            }
            else {
                objArray = data.FILELIST;
                FileJson.FILELIST = new Array();
            }
        }
        else {
            FileJson.FILELIST = data.FILELIST;
        }
        FillFileDetails();
        //#Endregion
    }
}

function FillFileDetails() {
    ///<summary>function used to fill the file details</summary>

    if (FileJson.FILELIST.length > 0 && FileJson.FILELIST[0].DOC_NAME != null) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}
function ClearFillFileDetails() {
    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(""); //FileName
            $(template).find("span:eq(0)").text("");
            $(template).find("a:eq(0)").attr("");
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
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
function PageInit() {
    //Hide Packing Div
    $("[id$=divPackingMaterial]").hide();
    $("#updateProgress").hide();
    //Reseting all input controls in the page
    $("[id$=btnSave]").hide();
    $("[id$=AddressSave]").hide();
    $("[id$=btnCopy]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=btnReset]").hide();
    if ($("[id$=hdfEnableWorkOrderItem]").val() == "1") {
        $("[id$=divIsWorkOrder]").show();
    }
    else {
        $("[id$=divIsWorkOrder]").hide();
    }


    //Filling category dropdown initially.
    if ($("[id$=ReferenceID]").val() == "0") {
        FillCategory(0);
        SetCategoryType();
        //FillTypes(0);
        if (parseInt($("[id$=hdfItemType]").val()) == 1) {
            $("[id$=divClass]").show();
            FillTypes(0);
        }
        else {
            $("[id$=divClass]").hide();
        }
        FillPackingType(0);
        FillClassification(0);
        FillCustomers(0);
        FillGSTClassification(0, 1);

    }
    //Hide PH & TSC & Mapped with Product checkbox for  packing material , service and Asset
    if ($("[id$=ITM_SET]").val() == 3 || $("[id$=ITM_SET]").val() == 4 || $("[id$=ITM_SET]").val() == 8) {
        $("[id$=divPHTSC]").hide();
        $("[id$=lblReqProductMapping]").hide();
        $("[id$=RequireProductMapping]").hide();
    }
    //Hide/Show Multiple UOM's(Purchase UOm & Sale UOM w.r.to GlobalConfiguration
    if ($("[id$=hdfShowMultipleUOM]").val() == 1) {
        $("[id$=divPurchaseSalesUOM]").show();
        if ($("[id$=hdfShowPurchaseUOM]").val() == 0) {
            $("[id$=lblPurchaseUom]").hide();
            $("[id$=ITM_UOM_PURCHASE").hide();
            $("[id$=lblSaleUom").removeClass("middle-lbl-small-b");
        }
    }
    else {
        $("[id$=divPurchaseSalesUOM]").hide();
        $("[id$=lblUOMPK]").html("UOM*");
    }

    //hide InactivePeriod for Service and Asset Master
    if ($("[id$=ITM_SET]").val() == 4 || $("[id$=ITM_SET]").val() == 8) {
        $("[id$=divInactivePeriod]").hide();

        $("[id$=divPurchaseSalesUOM]").hide();
        $("[id$=lblUOMPK]").html("UOM*");
    }

    if ($("[id$=ITM_SET]").val() == 8) {
        $("[id$=lblGCMPK]").hide();
        $("[id$=ITM_GST_CLASS]").hide();
    }
    //Reseting all input controls in the page.
    $("[id$=tabs]").tabs();
    $("[id$=tabs]").tabs("select", 0);
    $("[id$=tabs]").tabs("disable", 1);
    $("[id$=tabs]").tabs("disable", 2);
    $("[id$=tabs]").tabs("disable", 3);
    $("[id$=tabs]").tabs("disable", 4);
    //.tabs('disable', tabId)
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    //vendor
    FillVendor(0);
    FillCurrency();
    FillStoreTree(0);

    $("#divTaxData").data("TaxDetails", new Array());
    $("#divTaxData").data("TempDetails", new Array()); //For Temporary Storage of Tax/Discount    

    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    //FillVendorUOM(0);
    $("[id$=divType]").hide();
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("=");
            if (pK[1] == "3" && pK[0] == "Type") {
                $("[id$=ITM_SET]").val(pK[1]);
                $("[id$=divClass]").hide();
                $("[id$=divType]").show();
                $("[id$=lblMaterialCategoryName]").hide();
                $("[id$=MaterialCategoryName]").show();
                $("[id$=SearchType]").append("<option value='IPD_TYPE_TEXT'>Type</option>");
                $("[id$=SearchType]").append("<option value='IPD_CUSTOMER_TEXT'>Customer</option>");
            }
        }

    }

    return false;
}
///#endregion

///#region --------- Core Section ---------------
///#region---- Set Or Reset Form----
///<summary>Function To Show Data Entry Form </summary>
function AddNew(fillUOM) {
    $("[id$=btnSave]").show();
    $("[id$=AddressSave]").hide();
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    $("[id$=btnCancel]").hide();
    $("[id$=btnReset]").show();

    if ($("[id$=ITM_MIN_STK]").val() == "") {
        $("[id$=ITM_MIN_STK]").val('0');
    }
    if ($("[id$=ITM_ROL_STK]").val() == "") {
        $("[id$=ITM_ROL_STK]").val('0');
    }
    if ($("[id$=ITM_MAX_STK]").val() == "") {
        $("[id$=ITM_MAX_STK]").val('0');
    }
    //    if ($("[id$=ITM_PHR]").val() == "") {
    //        $("[id$=ITM_PHR]").val('0');
    //    }
    //    if ($("[id$=ITM_TSC]").val() == "") {
    //        $("[id$=ITM_TSC]").val('0');
    //    }
    if ($("[id$=ITM_MOQ]").val() == "") {
        $("[id$=ITM_MOQ]").val('0');
    }
    if ($("[id$=ITM_MAX_OQ]").val() == "") {
        $("[id$=ITM_MAX_OQ]").val('0');
    }



    $("select[id$=ITC_PK]").removeAttr("disabled");
    $("[id$=imbViewCag]").removeAttr("disabled");
    $("select[id$=UOM_PK]").removeAttr("disabled");
    var ObjMapping = $("#divMappingData").data("MappingData");
    ObjMapping = new Object();
    ObjMapping.MappingDetailsList = new Array();
    $("#divMappingData").data("MappingData", ObjMapping);
    $("input[id$=ITM_CODE]").focus();
    $("select[id$=ITM_TYPE_TEXT]").removeAttr("disabled");

    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("=");
            if (pK[1] == "3" && pK[0] == "Type") {
                $("[id$=divPackingMaterial]").show();
                $("[id$=ITM_SET]").val(pK[1]);
                FileJson.FILELIST = new Array();
                GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Material", true);
                if (fillUOM == undefined || fillUOM == true)
                    FillUomCategory();
                $("[id$=imbViewCag]").hide();

            }
            else
                if ((pK[1] == "4" || pK[1] == "8") && pK[0] == "Type") {
                    $("[id$=divMaterialMeasures]").hide();
                    $("[id$=ITM_SET]").val(pK[1]);
                    if (fillUOM == undefined || fillUOM == true)
                        FillUomCategory();
                    $("[id$=imbViewCag]").hide();
                    //$("select[id$=ITM_TYPE_TEXT]").attr("disabled", "disabled");
                    if (parseInt($("[id$=hdfItemType]").val()) == 1) {
                        $("select[id$=ITM_TYPE_TEXT]").removeAttr("disabled");
                    }
                    else {
                        $("select[id$=ITM_TYPE_TEXT]").attr("disabled", "disabled");
                    }
                }
        }
    }
    $("[id$=IPD_PK]").val('0');
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    if ($("[id$=ITM_SET]").val() == 3) {
        $("#updateProgress").show();
        window.location = "MaterialMaster.aspx?Type=3";
    }
    $("[id$=divPackingMaterial]").hide();
    $(document.forms[0]).find("textarea").each(function () {
        $(this).val("");
    });
    $("[id$=btnCancel]").show();
    $("[id$=btnReset]").hide();

    $(document.forms[0]).find("select").each(function () {
        idval = $(this).attr("id");
        if (idval.search("SBU") == -1)
            $(this).val($(this).find("option:eq(0)").val());
    });

    HideAdvSearch();
    RemoveValidations();
    $(document.forms[0]).validate().resetForm();

    $("[id$=btnSave]").hide();
    $("[id$=AddressSave]").hide();
    $("[id$=btnCopy]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=tabs]").tabs();
    $("[id$=tabs]").tabs("select", 0);
    $("[id$=tabs]").tabs("disable", 1);
    $("[id$=tabs]").tabs("disable", 2);
    $("[id$=tabs]").tabs("disable", 3);
    $("[id$=tabs]").tabs("disable", 4);
    $("[id$=tabs]").tabs("enable", 0);
    $("[id$=ITM_MIN_STK]").val('0');
    $("[id$=ITM_PHR]").val('');
    $("[id$=ITM_TSC]").val('');
    $("[id$=ITM_BATCH_CODE]").val('');
    $("[id$=ITM_ROL_STK]").val('0');
    $("[id$=ITM_MAX_STK]").val('0');
    $("[id$=SearchValue]").val('');
    //$("[id$=ITM_DESC]").val('');
    $("textarea[id$=ITM_DESC]").html("");
    $("[id$=ITM_CODE]").val('');
    $("[id$=ITM_NAME]").val('');
    $("input[id$=hdfIsFinalTab]").val('0');
    $("input[id$=InactivePeriod]").val("0");
    $("[id$=chkIsAsset]").attr("checked", false);
    AfterSave();
    ClearProductDetails();
    ClearSearch(); //SetSearchType();
    ClearPackingDetails();
    FillTypes();
    $("[id$=ITC_VALUE]").text('');

    $("#trvStores").find("input[type=checkbox]:checked").each(function () {
        $(this).attr('checked', false);
    });
    return false;
}
function ClearPackingDetails() {

    //<summary>function used to Clear mapping  Details</summary>
    $("select[id$=IPD_TYPE]").val("0");
    $("select[id$=IPD_CLASSIFICATION]").val("0");
    $("select[id$=IPD_CUSTOMER]").val("0");
    //$("select[id$=IPD_ACTIVE]").val("1");
    $("input[id$=IPD_INNER_LENGTH]").val("");
    $("input[id$=IPD_INNER_HEIGHT]").val("");

    $("input[id$=IPD_INNER_BREADTH]").val("");
    $("input[id$=IPD_OUTER_LENGTH]").val("");
    $("input[id$=IPD_OUTER_HEIGHT]").val("");
    $("input[id$=IPD_PLY]").val("");
    $("input[id$=IPD_OUTER_BREADTH]").val("");
    $("input[id$=ITM_WEIGHT]").val("");
    $("input[id$=IPD_PAPER_COLOR]").val("");
    $("input[id$=IPD_ART_WORK]").val("");
    $("input[id$=IPD_THICKNESS]").val("");
    $("input[id$=IPD_PAPER_TYPE]").val("");
    //fupUploader

}

function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page
    if (controlID.search("UserID") != -1) {
        return true;
    }
    if (controlID.search("UserPk") != -1) {
        return true;
    }
    if (controlID.search("MaterialDetailId") != -1) {
        $("#" + controlID).val("0");
        return true;
    }
    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
    if (controlID.search("ITM_PK") != -1) {
        $("#" + controlID).val("0");
        return true;
    }
    if (controlID.search("MaterialTaskID") != -1) {
        $("#" + controlID).val("0");
        return true;
    }
    if (controlID.search("MaterialReferenceID") != -1) {
        $("#" + controlID).val("0");
        return true;
    }
    if (controlID.search("MaterialProcessID") != -1) {
        return true;
    }
    if (controlID.search("MaterialApplicationID") != -1) {
        $("#" + controlID).val("0");
        return true;
    }
    if (controlID.search("MaterialActionID") != -1) {
        $("#" + controlID).val("0");
        return true;
    }
    if (controlID.search("STATUS") != -1) {
        return true;
    }
    return false;
}
///#endregion

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    //    var strname = $("select[id$=SearchType]").val();
    //    $("[id$=SearchValue]").val("");
    //    if (strname == "0") {
    //        $("[id$=SearchValue]").hide()
    //        $("[id$=imbSearch]").hide();
    //        BindGrid();
    //    }
    //    else {
    //        $("[id$=SearchValue]").show()
    //        $("[id$=imbSearch]").show();
    //    }
    $("[id$=SearchValue]").show()
    $("[id$=imbSearch]").show();
    FillMaterialCategoryAutoComplete();
    FillMaterialAutoComplete();
    BindGrid();
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", MaterialMaster.MaterialAutoCompleteURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val(), "SearchType");
    GrandScriptUtils.MakeAutoComplete("ITM_NAME", MaterialMaster.MaterialAutoCompleteURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val() + "&SearchType=ITM_NAME", false, false);
}
function ClearSearch() {
    $("[id$=ItemCodeMaterial]").val("");
    $("[id$=MaterialPK]").val(0);
    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=ddlStatus]").val("1");
    SetSearchType();
    BindGrid();
}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "MaterialCategory") {
        FillMaterialAutoComplete();
    }
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
    
    RemoveValidations();
    switch (command.toString()) {
        // To Delete Details            
        case MaterialMaster.DeleteCommand:
            materialID = GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, $(tr).parents("table:first").attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MaterialMaster.DeleteConfirmationMessage, MaterialMaster.ConfirmationMessage, MaterialMaster.DeleteMessageCommand, true);
            break;
        // To Edit Details                     
        case MaterialMaster.EditCommand:
            FillDetails(tr);
            break;
        case MaterialMaster.TaxDelete:
            MaterialMaster.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.IVT_SL_NO, $(tr).parents("table:first").attr("id"));
            MaterialMaster.EditTax = GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.IVT_TAX, $(tr).parents("table:first").attr("id"));
            var slNo = $("[id$=hdnSlNo]").val();
            GrandScriptUtils.ShowModal(MaterialMaster.DeleteConfirmationMessage, MaterialMaster.ConfirmationMessage, MaterialMaster.TaxDelete, true);
            break;
        case MaterialMaster.ShowRateCommand: //For Showing Vendor Rate PopUp
            materialID = GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, $(tr).parents("table:first").attr("id"));
            ShowVendorRate(materialID);
            break;
        case MaterialMaster.RelDeleteCommand:
            relMaterialID = GrandGrid.Utilities.GetColumnValue(tr, "ITM_PK", $(tr).parents("table:first").attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MaterialMaster.DeleteConfirmationMessage, MaterialMaster.ConfirmationMessage, MaterialMaster.RelDeleteMessageCommand, true);
            break;
        case MaterialMaster.BOMDeleteCommand:
            relMaterialID = GrandGrid.Utilities.GetColumnValue(tr, "ITM_PK", $(tr).parents("table:first").attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MaterialMaster.DeleteConfirmationMessage, MaterialMaster.ConfirmationMessage, MaterialMaster.BOMDeleteMessageCommand, true);
            break;
        // Default Handler           
        default:
            alert(MaterialMaster.DefaultAction);
            break;
    }
    return false;
}
///#endregion

///#region---- Fetch Data To Populate In Controls
function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    //    SetTreeHeaderStructure("trvCategory", MaterialMaster.GetMaterialCategoryTreeURL + $("[id$=BizUnitPk]").val() + MaterialMaster.Param, "Root", false, false, "0", false); // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", MaterialMaster.GetMaterialCategoryTreeURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val() + MaterialMaster.Param, "Root", false, false, "0", false); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

function FillUomCategory() {
    var categoryID = 0;
    if ($("[id$=ITC_PK]").val() != MaterialMaster.TextZero) {
        categoryID = $("[id$=ITC_PK]").val() == null ? 0 : $("[id$=ITC_PK]").val();
        // FillUOM(CategoryID,SelectVal) to fill uom names wrp to categoryid.
        FillUOM(categoryID, false);
        var StockUOM = 0;
        if ($("select[id$=UOM_PK] option:selected").val() != "undefined" && $("select[id$=UOM_PK] option:selected").val() != undefined && $("select[id$=UOM_PK] option:selected").val() !== "") {
            StockUOM = $("select[id$=UOM_PK] option:selected").val();
        }
        FillUOMsHaveConversion($("select[id$=ITM_UOM_PURCHASE]").attr("id"), StockUOM);
        FillUOMsHaveConversion($("select[id$=ITM_UOM_SALE]").attr("id"), StockUOM);
        GetQCInspValue(categoryID);
    }
}

function SetCategoryType() {
    var categoryID = 0;
    categoryID = $("[id$=ITC_PK]").val() == null ? 0 : $("[id$=ITC_PK]").val();
    $.get(MaterialMaster.GetMaterialCategoryDetailsURL + categoryID + "&Active=1", function (data) {
        if (data != null && data.length > 0) {
            $("[id$=ITC_VALUE]").text(data[0].ITC_VALUE_TEXT);
        }
    });

}



function FillCategory(catPK, uomPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=ITC_PK]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val(), function (data) {
        if ($("[id$=ITM_SET]").val() == 3 || $("[id$=ITM_SET]").val() == 4 || $("[id$=ITM_SET]").val() == 8)
            GrandScriptUtils.FillDropDown(drpID, data, true, false, catPK);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true, catPK);
        FillUOM(catPK, uomPK);
        var StockUOM = 0;
        if ($("select[id$=UOM_PK] option:selected").val() != "undefined" && $("select[id$=UOM_PK] option:selected").val() != undefined && $("select[id$=UOM_PK] option:selected").val() !== "") {
            StockUOM = $("select[id$=UOM_PK] option:selected").val();
        }
        FillUOMsHaveConversion($("select[id$=ITM_UOM_PURCHASE]").attr("id"), StockUOM);
        FillUOMsHaveConversion($("select[id$=ITM_UOM_SALE]").attr("id"), StockUOM);
        SetCategoryType();
    });
}

function FillBOMCategory(catPK, uomPK) {

    var drpID = $("select[id$=ITC_BOM_CAT]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val(), function (data) {
        if ($("[id$=ITM_SET]").val() == 3 || $("[id$=ITM_SET]").val() == 4 || $("[id$=ITM_SET]").val() == 8)
            GrandScriptUtils.FillDropDown(drpID, data, true, false, catPK);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true, catPK);
        InitBOMaterial();
    });
}
function InitBOMaterial() {
    $("[id$=txtPcsInKG]").val('')
    GrandScriptUtils.MakeAutoCompleteLimitLen("txtBOMaterial", MaterialMaster.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + $("[id$=ITC_BOM_CAT]").val(), "B_ITEM", true, false, "MaterialType", true, "", "", "", $("[id$=AutoStartValue]").val());
}
function SetBOMCategoryItem() {
    InitBOMaterial();
}
function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("MaterialCategory", MaterialMaster.MaterialCategroyDeptURL + "&Type=" + $("[id$=ITM_SET]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
}
function FillMaterialAutoComplete() {
    //<summary> Function Used to make Item field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("ItemCodeMaterial", MaterialMaster.MaterialURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val() + "&FLDNAME=ITM_TEXT", "MaterialPK", true, false, "MaterialCategoryPK", true);
}
function FillPackingType(itemPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=IPD_TYPE]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillPackingType + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, itemPK);
        setPackingVisibility();
    });
}

function FillClassification(itemPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=IPD_CLASSIFICATION]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillClassification + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, itemPK);
        setPackingVisibility();
    });
}

function FillGSTClassification(PK, Active) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=ITM_GST_CLASS]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillHSNDropdownURL + PK + "&Active=" + Active, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, PK);
    });
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
        //        CBM = Number(CBM).toFixed(3);
        CBM = Number(CBM).toFixed(4);
    }

    $("[id$='ITM_CBM']").val(CBM);

}


function setPackingVisibility() {
    var typePK = $("select[id$=IPD_TYPE]").val();
    $.get(MaterialMaster.FillPackingDtl + $("[id$=BizUnitPk]").val() + "&typePK=" + typePK, function (data) {
        $("[id$=trOuterDimension]").hide();
        $("[id$=trOuterDimensionHdr]").hide();
        $("[id$=divHeight]").show();
        if (data != null && data.length > 0) {
            switch (data[0].CON_DATA) {
                case "MC":
                case "SC":
                    $("[id$=trOuterDimension]").show();
                    $("[id$=trOuterDimensionHdr]").show();
                    break;
                case "PC":
                case "ZB":
                    $("[id$=divHeight]").hide();
                    break;
                case "3":
                    break;

            }

        }
    });
    return false;
}
function FillCustomers(customerPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=IPD_CUSTOMER]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillCustomers + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, customerPK, true);
    });
}


function FillTypes(typeID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=ITM_TYPE_TEXT]").attr("id");
    $.get(MaterialMaster.FillMaterialTypeDropdownURL + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + "Item Type", function (data) {
        if ($("[id$=ITM_SET]").val() == 4 || $("[id$=ITM_SET]").val() == 8) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, MaterialMaster.ServiceVal);
        }
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true, typeID);
    });
}
function CopyVerion() {
    //    setNextProductCode($("input[id$=ITM_CODE]").val());
    $("[id$=MaterialDetailId]").val('0');
    $("[id$=IPD_PK]").val('0');
    //Resetting Vendor Mapping Detail PK(For Resolving:while creating a new packaging material by using the new version copy of old material, vendor mapping details doesn’t saving)
    var MappingDataLst = $("#divMappingData").data("MappingData");
    for (var i in MappingDataLst.MappingDetailsList) {
        MappingDataLst.MappingDetailsList[i].ITV_PK = "0";
    }
    $("[id$=MappingDetailsList]").val(JSON.stringify(MappingDataLst.MappingDetailsList));
    $("#divMappingData").data("MappingData", MappingDataLst);

    //newversion
    var newversion;
    newversion = increment_last($("input[id$=ITM_CODE]").val());
    $("input[id$=ITM_CODE]").val(newversion);
    //end newversion
    GrandScriptUtils.ShowModal(MaterialMaster.NewVerionCreated, MaterialMaster.MessageBoxTitle);
    $("[id$=btnCopy]").hide();
    ClearFileList();
    $("#trvStores").find("input[type=checkbox]:checked").each(function () {
        $(this).attr('checked', false);
    });
    return false;
}
function ClearFileList() {
    $("[id$=TEMPFILELIST]").val('');
    $("[id$=FILELIST]").val('');
    $("#divFileData").data("FileData", "");
    $("#fContainer_" + "fupUploader").find("span.fileUploadClass").each(function (indx) {
        // Remove the File Name From the List
        $(this).parents("div:eq(0)").remove();
    });
}

//Increment last no of a string
function increment_last(v) {
    var last2 = v.slice(-2);
    if (!isNaN(last2)) {
        return v.replace(/[0-9]+(?!.*[0-9])/, parseInt(v.match(/[0-9]+(?!.*[0-9])/), 10) + 1);
    }
    else {
        var last1 = v.slice(-1);
        if (!isNaN(last1)) {
            return v.replace(/[0-9]+(?!.*[0-9])/, parseInt(v.match(/[0-9]+(?!.*[0-9])/), 10) + 1);
        }
        else {
            var newItemCode;
            newItemCode = v + "1";
            return newItemCode;
        }
    }
}
//end

function FillDetails(tr) {
    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param>
    if ($("[id$=ITM_SET]").val() == 3) {//Packing Material
        $("[id$=btnCopy]").show();
    }
    $("[id$=btnCancel]").hide();
    $("[id$=btnReset]").show();

    var grdID = $(tr).parents("table:first").attr("id");
    $("input[id$=MaterialDetailId]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, grdID));
    $("input[id$=ITC_IS_STOCK]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.ITC_IS_STOCK, grdID));
    $("input[id$=ITM_CODE]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCode, grdID));
    $("input[id$=ITM_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialName, grdID));
    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDesc, grdID) != "null")
        $("textarea[id$=ITM_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDesc, grdID));
    // $("[id$=ITM_DESC]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDesc, grdID));
    $("select[id$=ITC_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID));
    SetCategoryType();
    GetQCInspValue(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID)));
    $("select[id$=ITM_TYPE_TEXT]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialType, grdID));
    $("input[id$=ITM_MIN_STK]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMinStockLevel, grdID));
    if ($("input[id$=ITM_MIN_STK]").val() == "null") {
        $("input[id$=ITM_MIN_STK]").val("0");
    }

    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.IsWorkOrder, grdID) == "1" ? $("input[id$=chkIsWorkOrder]").attr("checked", true) : $("input[id$=chkIsWorkOrder]").attr("checked", false);
    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.RequireQCInspection, grdID) == "1" ? $("input[id$=RequireInspection]").attr("checked", true) : $("input[id$=RequireInspection]").attr("checked", false);
    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.RequireBatchStk, grdID) == "1" ? $("input[id$=RequireBatch]").attr("checked", true) : $("input[id$=RequireBatch]").attr("checked", false);
    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.RequireProductMapping, grdID) == "1" ? $("input[id$=RequireProductMapping]").attr("checked", true) : $("input[id$=RequireProductMapping]").attr("checked", false);
    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialActive, grdID) == "1" ? $("input[id$=chkItemActive]").attr("checked", true) : $("input[id$=chkItemActive]").attr("checked", false);
    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.ConversionRequired, grdID) == "1" ? $("input[id$=chbConversionRequired]").attr("checked", true) : $("input[id$=chbConversionRequired]").attr("checked", false);
    GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.IsAsset, grdID) == "1" ? $("input[id$=chkIsAsset]").attr("checked", true) : $("input[id$=chkIsAsset]").attr("checked", false);

    $("input[id$=ITM_MAX_STK]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMaxStockevel, grdID));
    if ($("input[id$=ITM_MAX_STK]").val() == "null") {
        $("input[id$=ITM_MAX_STK]").val("0");
    }

   

    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.ITC_IS_STOCK, grdID) != 1) {//if categorystock !=1 disable RequireBatch
        $("[id$=RequireBatch]").attr("disabled", "disabled");
    }
    else {
        $("[id$=RequireBatch]").removeAttr("disabled");
    }
    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.PHR, grdID) != "null")
        $("input[id$=ITM_PHR]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.PHR, grdID));
    else
        $("input[id$=ITM_PHR]").val("");

    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.TSC, grdID) != "null")
        $("input[id$=ITM_TSC]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.TSC, grdID));
    else
        $("input[id$=ITM_TSC]").val("");

    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.BATCHCODE, grdID) != "null")
        $("input[id$=ITM_BATCH_CODE]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.BATCHCODE, grdID));
    else
        $("input[id$=ITM_BATCH_CODE]").val("");

    $("input[id$=ITM_ROL_STK]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialReorderLevel, grdID));
    if ($("input[id$=ITM_ROL_STK]").val() == "null") {
        $("input[id$=ITM_ROL_STK]").val("0");
    }
    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.Moq, grdID) != "null")
        $("input[id$=ITM_MOQ]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.Moq, grdID));
    else
        $("input[id$=ITM_MOQ]").val("0");
    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaxOq, grdID) != "null")
        $("input[id$=ITM_MAX_OQ]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaxOq, grdID));
    else
        $("input[id$=ITM_MAX_OQ]").val("0");
    if (GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.InactivePeriod, grdID) != "null")
        $("input[id$=InactivePeriod]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.InactivePeriod, grdID));
    else
        $("input[id$=InactivePeriod]").val("0");
    //vendor mapping
    $("input[id$=ITM_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, grdID));

    $("[id$=MaterialCode]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCode, grdID));
    $("[id$=MaterialName]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialName, grdID));
    $("[id$=ITV_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialName, grdID));
    $("[id$=MaterialCategoryName]").html($("select[id$=ITC_PK] option:selected").text());

    //Related Material mapping
    $("[id$=lblBaseMaterialCode]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCode, grdID));
    $("[id$=lblBaseMaterialName]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialName, grdID));
    $("[id$=lblBaseMaterialCatName]").html($("select[id$=ITC_PK] option:selected").text());

    $("input[id$=ItemType]").val(GrandGrid.Utilities.GetColumnValue(tr, "IPD_TYPE", grdID));
    uomName = GrandGrid.Utilities.GetColumnValue(tr, "UOM_NAME", grdID);

    FillGSTClassification(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.GstCodeVal, grdID), 1);
    $("select[id$=ITM_GST_CLASS]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.GstCodeVal, grdID));
    //changing mode to material lising
    AddNew(false);
    //    $("select[id$=ITC_PK]").attr("disabled", "disabled");
    //    $("[id$=imbViewCag]").attr("disabled", "disabled");
    $("select[id$=UOM_PK]").attr("disabled", "disabled");
    //    $("select[id$=ITM_TYPE_TEXT]").attr("disabled", "disabled");
    //------------------------Eligibility----------------------------
    if (parseInt($("[id$=hdfItemType]").val()) == 1) {
        $("select[id$=ITM_TYPE_TEXT]").removeAttr("disabled");
    }
    else {
        $("select[id$=ITM_TYPE_TEXT]").attr("disabled", "disabled");
    }
    //---------------------------------------------------------------


    // Filling UOM(CategoryID,SelctVal)
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOU, grdID))

    FillUOMsHaveConversion($("select[id$=ITM_UOM_PURCHASE]").attr("id"), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOU, grdID), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOUPurchase, grdID))
    FillUOMsHaveConversion($("select[id$=ITM_UOM_SALE]").attr("id"), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOU, grdID), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOUSale, grdID))

    FillVendorUOM(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, grdID));
    FillVendorMappingXmlDetails(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, grdID))

    //BOM
    if ($("input[id$=ItemType]").val() == MaterialMaster.PouchType) {
        $("[id$=lblBOMaterialCode]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCode, grdID));
        $("[id$=lblBOMaterialName]").html(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialName, grdID));
        $("[id$=lblUOM]").html(GrandGrid.Utilities.GetColumnValue(tr, "UOM_NAME", grdID));

        //enabling BO material
        $("[id$=tabs]").tabs("enable", 4);
        FillBOMCategory($("[id$=ITC_PK]").val());
      
    }
    else
        $("[id$=tabs]").tabs("disable", 4);
    //enabling related material
    $("[id$=tabs]").tabs("enable", 3);
    //enabling vendor mapping tab in edit mode
    $("[id$=tabs]").tabs("enable", 1);
    //enabling store mapping tab in edit mode
    $("[id$=tabs]").tabs("enable", 2);
    BindBOMaterial();
    BindRelatedMaterial();
    FillStoreTree($("[id$=ITM_PK]").val());
    //Get Packing Details
    $.get(MaterialMaster.PackingMaterialDtlURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("[id$=ITM_PK]").val(), function (data) {   
        if (data != null && data != "") {
            // GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Material", false);
            $("select[id$=IPD_TYPE]").val(data[0].IPD_TYPE);
            $("select[id$=IPD_CLASSIFICATION]").val(data[0].IPD_CLASSIFICATION);
            $("select[id$=IPD_CUSTOMER]").val(data[0].IPD_CUSTOMER);
            //$("select[id$=IPD_ACTIVE]").val(data[0].IPD_ACTIVE);
            $("input[id$=IPD_INNER_LENGTH]").val(data[0].IPD_INNER_LENGTH);
            $("input[id$=IPD_INNER_HEIGHT]").val(data[0].IPD_INNER_HEIGHT);
            $("[id$=IPD_PK]").val(data[0].IPD_PK);

            $("input[id$=IPD_INNER_BREADTH]").val(data[0].IPD_INNER_BREADTH);
            $("input[id$=IPD_OUTER_LENGTH]").val(data[0].IPD_OUTER_LENGTH);
            $("input[id$=IPD_OUTER_HEIGHT]").val(data[0].IPD_OUTER_HEIGHT);
            $("input[id$=IPD_PLY]").val(data[0].IPD_PLY);
            $("input[id$=IPD_OUTER_BREADTH]").val(data[0].IPD_OUTER_BREADTH);
            $("input[id$=ITM_WEIGHT]").val('');
            if (data[0].ITM_WEIGHT != null && data[0].ITM_WEIGHT > 0) {
                $("input[id$=ITM_WEIGHT]").val((data[0].ITM_WEIGHT).toFixed(4));
            }
            //            $("input[id$=ITM_WEIGHT]").val(data[0].ITM_WEIGHT);
            $("input[id$=IPD_PAPER_COLOR]").val(data[0].IPD_PAPER_COLOR);
            $("input[id$=IPD_ART_WORK]").val(data[0].IPD_ART_WORK);
            $("input[id$=IPD_THICKNESS]").val(data[0].IPD_THICKNESS);
            $("input[id$=IPD_PAPER_TYPE]").val(data[0].IPD_PAPER_TYPE);
            setPackingVisibility();
            CalcCBM();
            if (data[0].DOC_PK != null) {
                FileJson.FILELIST = new Array();
                var obj = new Object();
                obj.DOC_PK = data[0].DOC_PK;
                obj.DOC_TITLE = data[0].DOC_TITLE;
                obj.DOC_TYPE = data[0].DOC_TYPE;
                obj.DOC_NAME = data[0].DOC_NAME;
                FileJson.FILELIST.push(obj);
                FillFileDetails();
            }
        }

    });

}
///#endregion

///#region---- Data Management Section----
function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(MaterialMaster.MaterialDeleteURL + materialID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            if ($("[id$=ITM_SET]").val() == 8)
                msgtxt = MaterialMaster.AssetDeleteMessage;
            else
                msgtxt = $("[id$=ITM_SET]").val() == 4 ? MaterialMaster.ServiceDeleteMessage : MaterialMaster.MaterialDeleteMessage;
        }
        else if (parseInt(data) == 0)
            msgtxt = MaterialMaster.MaterialUsed;
        else
            msgtxt = MaterialMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, MaterialMaster.MessageBoxTitle, MaterialMaster.DeleteCommand);
    });
    return false;
}

function AdvanceSearchInvoke(srchVal) {
    ///<summary>function To invoke advance search to bind grid  </summary>
    BindGrid(srchVal);
}

function ShowAdvSearch() {
    ///<summary>function To set show advance search and hide  search  </summary>
    var searchOpt = new Array();
    var advSearchProperty = {
        AutoCompleteURL: MaterialMaster.MaterialAutoCompleteURL,
        SearchOption: searchOpt
    }
    SetAdvanceSearch(advSearchProperty);
    return false;
}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    //var ajaxUrl = MaterialMaster.MaterialBindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val().replace('&', "%26") + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val();
    //$("[id$=SearchValue]").val().replace(/[&]/g, "ampersand");
    var SearchVal = $("[id$=ItemCodeMaterial]").val();
    if (SearchVal == "Select/Type") {
        if ($("[id$=SearchValue]").val() != "") {
            SearchVal = encodeURIComponent($("[id$=SearchValue]").val()); //this is required when searchdata(QueryString) comes from PackingSpec form
        }
        else {
            SearchVal = "";
        }
    }
    else {
        SearchVal = encodeURIComponent($("[id$=ItemCodeMaterial]").val());
    }
    //var ajaxUrl = MaterialMaster.MaterialBindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + SearchVal + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val();
    var ajaxUrl = MaterialMaster.MaterialBindGridURL + "&SearchValue=" + SearchVal + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val() + "&ITMCAT=" + $("[id$=MaterialCategoryPK]").val() + "&Active=" + $("[id$=ddlStatus]").val();
    //Packing Master
   
    if ($("[id$=ITM_SET]").val() == 3) {
       
        $("[id$=divMaterialList]").hide();
        $("[id$=divServiceList]").hide();
        $("#grdPackingList").removeAttr("ajaxurl")
        $("#grdPackingList").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdPackingList");
        GrandGrid.MakeGrid($("#grdPackingList"));
    }
    //Services MaAster
    else
        if ($("[id$=ITM_SET]").val() == 4 || $("[id$=ITM_SET]").val() == 8) {
            $("[id$=divMaterialList]").hide();
            $("[id$=divPackingList]").hide();
            $("#grdServiceList").removeAttr("ajaxurl")
            $("#grdServiceList").attr("ajaxurl", ajaxUrl);
            GrandGrid.Utilities.ResetGrid(true, "grdServiceList");
            GrandGrid.MakeGrid($("#grdServiceList"));
        }
        //Material Master
        else {
            $("[id$=divPackingList]").hide();
            $("[id$=divServiceList]").hide();
            $("#grdCategory").removeAttr("ajaxurl")
            $("#grdCategory").attr("ajaxurl", ajaxUrl);
            GrandGrid.Utilities.ResetGrid(true, "grdCategory");
            GrandGrid.MakeGrid($("#grdCategory"));
        }

    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

function SavePage() {
    debugger;
    ///<summary>Function used to saving materials  </summary>
    var temp = $("[id$=MaterialDetailId]").val();
    $("select[id$=ITC_PK]").removeAttr("disabled");
    $("[id$=imbViewCag]").removeAttr("disabled");
    $("select[id$=UOM_PK]").removeAttr("disabled");
    $("select[id$=ITM_TYPE_TEXT]").removeAttr("disabled");
    if ($("input[id$=ITM_MIN_STK]").val() == '') $("input[id$=ITM_MIN_STK]").val('0');
    if ($("input[id$=ITM_MAX_STK]").val() == '') $("input[id$=ITM_MAX_STK]").val('0');
    if ($("input[id$=ITM_MOQ]").val() == '') $("input[id$=ITM_MOQ]").val('0');
    if ($("input[id$=ITM_MAX_OQ]").val() == '') $("input[id$=ITM_MAX_OQ]").val('0');
    if ($("input[id$=ITM_ROL_STK]").val() == '') $("input[id$=ITM_ROL_STK]").val('0');
    //    if ($("input[id$=IPD_THICKNESS]").val() == '') $("input[id$=IPD_THICKNESS]").val('0');
    //if ($("input[id$=ITM_PHR]").val() == '') $("input[id$=ITM_PHR]").val('0');
    //if ($("input[id$=ITM_TSC]").val() == '') $("input[id$=ITM_TSC]").val('0');
    var IsWorkOrder = $("input[id$=chkIsWorkOrder]").is(':checked');
    var IsReqInspec = $("input[id$=RequireInspection]").is(':checked');
    var IsReqBatch = $("input[id$=RequireBatch]").is(':checked');
    var IsProductMapped = $("input[id$=RequireProductMapping]").is(':checked');
    var IsItemActive = $("input[id$=chkItemActive]").is(':checked');
    var IsConversionRequired = $("input[id$=chbConversionRequired]").is(':checked');
    var IsAsset = $("input[id$=chkIsAsset]").is(':checked');
    $("[id$=ITM_IS_WORK_ORDER]").val(IsWorkOrder == true ? "1" : "0");
    $("[id$=ITM_NEED_QC_INSP]").val(IsReqInspec == true ? "1" : "0");
    $("[id$=ITM_NEED_BATCH_STK]").val(IsReqBatch == true ? "1" : "0");
    $("[id$=ITM_IS_LINKED_ITEM]").val(IsProductMapped == true ? "1" : "0");
    $("[id$=STATUS]").val(IsItemActive == true ? "1" : "0");
    $("[id$=ITM_IS_CONVERSION_REQD]").val(IsConversionRequired == true ? "1" : "0");
    $("[id$=ITM_IS_ASSET]").val(IsAsset == true ? "1" : "0");
    var selectedtems = JSON.stringify(GetSelectedStores());
    $("#[id*=StoreDtl]").val(selectedtems);
    var RelatedItemMap = JSON.stringify(GetRelatedItemMap());
    $("#[id*=RelatedItemMap]").val(RelatedItemMap);
    var bomMap = JSON.stringify(GetBOMaterialMap());
    $("#[id*=BomItemMap]").val(bomMap);

    $("[id$=TEMPFILELIST]").val('');
    var ObjFile = $("#divFileData").data("FileData");
    if (ObjFile != undefined || ObjFile != null && ObjFile != "")
        $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
    else {
        ClearFileList();
        $("[id$=FILELIST]").remove();
    }
    if ($("[id$=FILELIST]").val() == "undefined" || $("[id$=FILELIST]").val() == "")//Edit for Bug15748 
        $("[id$=FILELIST]").remove();

    if ($("[id$=ITM_SET]").val() == 3) {
        AddValidations(4);
        $("[id$=IPD_ACTIVE]").val(IsItemActive == true ? "1" : "0");
    }
    if ($("input[id$=InactivePeriod]").val() == "") {
        $("input[id$=InactivePeriod]").val("0");
    }
    AddValidations(1);
    debugger;
    $("a[href=#Material]").click(); // For to validate Material Tab, even if save from any tab
    if ($(document.forms[0]).valid()) {
        // if (!validateMaterialTab()) return false;
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(MaterialMaster.MaterialSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
           
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(MaterialMaster.MaterialCodeExistsMessage, MaterialMaster.MessageBoxTitle);
                $("input[id$=ITM_CODE]").focus();
            }
            else if (parseInt(data) > 0) {
                if ($("[id$=ITM_SET]").val() == 4) {// Service
                    GrandScriptUtils.ShowModal(MaterialMaster.ServiceSaveMessage, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveCommand);
                    FillMaterialForVendorMapping(data);
                    $("[id$=divPackingMaterial]").hide();
                    $("[id$=btnCopy]").hide();
                }
                else if ($("[id$=ITM_SET]").val() == 8) {// Asset
                    GrandScriptUtils.ShowModal(MaterialMaster.AssetSaveMessage, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveCommand);
                    FillMaterialForVendorMapping(data);
                    $("[id$=divPackingMaterial]").hide();
                    $("[id$=btnCopy]").hide();
                }
                else {
                    $("input[id$=MaterialDetailId]").val(data);
                    if ($("[id$=hdfIsFinalTab]").val() == 1)//THis is the final tab
                    {
                        GrandScriptUtils.ShowModal(MaterialMaster.StoreMappSaveMessage, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveCommand);
                    }
                    else {
                        GrandScriptUtils.ShowModal(MaterialMaster.MaterialSaveMessage, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveCommand);
                        FillMaterialForVendorMapping(data);
                        BindBOMaterial();
                        BindRelatedMaterial();
                        if (parseInt($("input[id$=IPD_PK]").val()) == 0)
                            $("[id$=IPD_PK]").val(data);
                        $("[id$=divPackingMaterial]").hide();
                        $("[id$=btnCopy]").hide();
                        //ClearMaterialDetails(); 
                        ReplaceUploadFolder();  //For Resolving Bug ID:  1477                  
                    }
                }
                $("select[id$=UOM_PK]").attr("disabled", "disabled");
            }
            else if (parseInt(data) == -32) {//UOM mismatch:Uom is different from previous
                GrandScriptUtils.ShowModal(MaterialMaster.UOMMismatch, MaterialMaster.MessageBoxTitle);
            }
            else if (parseInt(data) == -4) {//Asset: If Asset already created ,unable to modify
                GrandScriptUtils.ShowModal(MaterialMaster.IsAssetMismatch, MaterialMaster.MessageBoxTitle);
            }
            else {
                GrandScriptUtils.ShowModal(MaterialMaster.ActionFailedMessage);
            }
        });
    }
    else {
        //$("select[id$=ITM_TYPE_TEXT]").attr("disabled", "disabled");
        if (parseInt($("[id$=hdfItemType]").val()) == 1) {
            $("select[id$=ITM_TYPE_TEXT]").removeAttr("disabled");
        }
        else {
            $("select[id$=ITM_TYPE_TEXT]").attr("disabled", "disabled");
        }
        if (parseInt($("input[id$=MaterialDetailId]").val()) > 0) {
            //            $("select[id$=ITC_PK]").attr("disabled", "disabled");
            $("[id$=imbViewCag]").attr("disabled", "disabled");
            $("select[id$=UOM_PK]").attr("disabled", "disabled");
        }
    }
    return false;
}

function encode_utf8(s) {
    return unescape(encodeURIComponent(s));
}

function decode_utf8(s) {
    return decodeURIComponent(escape(s));
}


function ReplaceUploadFolder() {
    if (FileJson.FILELIST == "undefined" || FileJson.FILELIST == "") {
        if (FileJson.FILELIST.length > 0 && FileJson.FILELIST[0].DOC_NAME != null) {
            var Temptemplate = $("#_FileUploadTemplate").clone();
            $("#fContainer_" + "fupUploader").find("span.fileUploadClass").each(function (indx) {
                $(this).parents("div:eq(0)").remove(); // Remove the File Name From the List
            });
            for (var index in FileJson.FILELIST) {
                $(Temptemplate).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + FileJson.FILELIST[index].DOC_TYPE); //FileName
                $(Temptemplate).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
                $(Temptemplate).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
                $("#fContainer_" + "fupUploader").append($(Temptemplate).html());
            }
        }
    }

}

function ShowCategory() {
    ///<summary>Function used call the tree Data </summary>
    //    
    //  var encStr=encode_utf8($("[id$=ITM_DESC]").val().replace(/[!'()]/g, escape));
    // var decStr=decode_utf8(encStr);
    //  alert($("[id$=ITM_DESC]").val());
    //   alert(encStr);
    // alert(decStr);

    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Choose Category", false, "700", false, false);
    return false;
}

function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    var CagName = cagID.substr(0, cagID.indexOf("_")); //For avoiding to execute below codes after click on Stores in StoreMapping Tab
    if (CagName != "trvStores") {
        cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
        $("select[id$=ITC_PK]").val(cagID);
        SetCategoryType();
        //setting selected value for uom after selecting category from treeview.
        //        FillUOM(cagID, false)
        //closing modalbox after selected from treeview.
        $("#divCategory").dialog("destroy");
        $("#divCategory").dialog({ autoOpen: false });
    }
}

function FillUOM(categoryID, selectval) {
    ///<summary>function To Fill Uom Details </summary>
    /// <param name="categoryID"  type="string">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    /// <param name="selectval"  type="string">
    ///     Specific value to be selected.
    /// </param>
    // if (categoryID != 0) {
    //        $("select[id$=UOM_PK]").removeData();
    // Get id of the UOM DropDown
    var drpID = $("select[id$=UOM_PK]").attr("id");
    //Fill UOM Details to the UOM DropDown, Name as Text, PK as Value
    /////////////
    //UOM Correction, Need to correct after QC
    //        $.get(MaterialMaster.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val() + MaterialMaster.Param + categoryID, function (data) {
    $.get(MaterialMaster.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
        if (selectval) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);

            var unit = $("select[id$=UOM_PK] option:selected").text();
            $("select[id$=ITV_MOQ_UOM]").each(function () {
                $('option', this).each(function () {
                    if ($.trim($(this).text().toLowerCase()) == $.trim(unit.toLowerCase())) {
                        $(this).attr('selected', 'selected');
                    };
                });
            });

        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });
    //}
    // else
    // $("select[id$=UOM_PK]").find("option").remove();
}

//Purchase Sale UOM Filling
function FillUOMsHaveConversion(dropdwnID, stockUomPk, selectval) {
    ///<summary>function To Fill Purchase/Sale UOM Details </summary>    
    /// <param name="selectval"  type="string">
    ///     Specific value to be selected.
    /// </param>
    var drpID;
    // Get id of the UOM DropDown   
    drpID = dropdwnID;
    //Fill UOM Details to the UOM DropDown, Name as Text, PK as Value

    $.get(MaterialMaster.FillUOMConversionsURL + stockUomPk, function (data) {
        if (selectval) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });
}


//End
function GetQCInspValue(categoryID) {
    $.get(MaterialMaster.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val() + MaterialMaster.Param + categoryID, function (data) {
        if (data != null && data != "") {
            QCInspVal = data[0].QcInsp;
        }
    });
}
///#endregion
///#endregion

///#region---------- Validations ----------------
function AddValidations(mode) {
    RemoveValidations();
    ///<summary>function To Validations </summary>
    if (mode == 1) {
        $("input[id$=ITM_CODE]").rules("add", {
            required: true,
            maxlength: 200,
            //minlength: 3,
            messages: { required: MaterialMaster.MaterialCodeValidation }
        });
        $("input[id$=ITM_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: MaterialMaster.MaterialNameValidation }
        });
        $("select[id$=ITC_PK]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialCategoryValidation }
        });
        $("select[id$=ITM_TYPE_TEXT]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialTypeValidation }
        });
        $("select[id$=UOM_PK]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialUOMValidation }
        });
        $("select[id$=ITM_UOM_PURCHASE]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialUOMPurchaseValidation }
        });
        $("select[id$=ITM_UOM_SALE]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialUOMSalesValidation }
        });
        if ($("input[id$=ITM_MAX_STK]").val() != '0' && $("input[id$=ITM_MIN_STK]").val() != '0') {
            $("input[id$=ITM_MIN_STK]").rules("add", {
                required: false,
                maxlength: 12,
                ThreeDecimal: true,
                max: $("input[id$=ITM_MAX_STK]").val(),
                messages: { max: MaterialMaster.MinStockLevelLessthanMax }
            });
        }
        if ($("input[id$=ITM_MOQ]").val() != '0') {
            $("input[id$=ITM_MOQ]").rules("add", {
                required: false,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                messages: { CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) } //digits: MaterialMaster.ValidReqMOQ
            });
        }
        else {
            $("input[id$=ITM_MOQ]").rules("remove");
        }
        if ($("[id$=hdfEnableMaxOrderQty]").val() == "1") {
            if ($("input[id$=ITM_MAX_OQ]").val() != '0') {
                $("input[id$=ITM_MAX_OQ]").rules("add", {
                    required: false,
                    DecimalDigits: QtyDec,
                    CustomDecimal: true,
                    messages: { CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) } //digits: MaterialMaster.ValidReqMOQ
                });
            }
            else {
                $("input[id$=ITM_MAX_OQ]").rules("remove");
            }
        }
        if ($("input[id$=ITM_PHR]").val() != '0') {
            $("input[id$=ITM_PHR]").rules("add", {
                required: false,
                maxlength: 12,
                digits: true,
                max: $("input[id$=ITM_PHR]").val(),
                messages: { digits: MaterialMaster.ValidReqPHR }
            });

        }
        if ($("input[id$=ITM_TSC]").val() != '0') {
            $("input[id$=ITM_TSC]").rules("add", {
                required: false,
                maxlength: 12,
                FiveDecimal: true,
                max: $("input[id$=ITM_TSC]").val(),
                messages: { digits: MaterialMaster.ValidReqTSC }
            });
        }
        //        $("input[id$=ITM_MAX_STK]").rules("add", {
        //            required: true,
        //            maxlength: 12,
        //            ThreeDecimal: true,
        //            messages: { required: MaterialMaster.MaterialMaxStockValidation }
        //        });

        if (QCInspVal == "1") {
            $("input[id$=ITM_BATCH_CODE]").rules("add", {
                required: true,
                maxlength: 20,
                messages: { required: MaterialMaster.EnterBatchCode }
            });
        }
        else {
            $("input[id$=ITM_BATCH_CODE]").rules("add", {
                maxlength: 20
            });
        }
        
        if ($("input[id$=ITM_MIN_STK]").val() != '0' && $("input[id$=ITM_MAX_STK]").val() != '0' && $("input[id$=ITM_ROL_STK]").val() != '0') {
            $("input[id$=ITM_ROL_STK]").rules("add", {
                required: false,
                maxlength: 12,
                ThreeDecimal: true,
                range: [$("input[id$=ITM_MIN_STK]").val(), $("input[id$=ITM_MAX_STK]").val()],
                messages: { range: MaterialMaster.MaterialReorderlevelBetweenMinAndMax }
            });
        }
    }
    else if (mode == 2) {
        $("select[id$=ITV_VENDOR]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.VendorNameValidation }
        });
        $("input[id$=ITV_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: MaterialMaster.VendorMaterialNameValidation }
        });
        $("input[id$=ITV_PRICE]").rules("add", {
            required: false,
            maxlength: 12,
            ThreeDecimal: true,
            messages: { required: MaterialMaster.VendorMaterialPriceValidation }
        });
        $("select[id$=ITV_CURRENCY]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.VendorCurrencyValidation }
        });
        $("input[id$=ITV_MOQ]").rules("add", {
            required: true,
            maxlength: 12,
            //ThreeDecimal: true,
            //TwoDecimal: true,
            SetTwoDecimal: true,
            messages: { required: MaterialMaster.VendorMOQValidation }
        });
        $("select[id$=ITV_MOQ_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialUOMValidation }
        });
        //        $("input[id$=ITV_TAX_PERC]").rules("add", {
        //           
        //            maxlength: 12,
        //            TwoDecimal: true,
        //            messages: { required: MaterialMaster.VendorTAXValidation }
        //        });
        //        $("input[id$=ITV_DISC_PERC]").rules("add", {
        //            maxlength: 12,
        //            TwoDecimal: true,
        //            messages: { required: MaterialMaster.VendorDiscountValidation }
        //        });
        $("input[id$=ITV_LEAD_TIME]").rules("add", {
            required: true,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { required: MaterialMaster.VendorLeadDaysValidation }
        });
    }
    else if (mode == 3) {
        //        $("[id$=ChooseTax]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: "Translate(SelectType)" }
        //        });
    }
    else if (mode == 4) {
        $("input[id$=IPD_OUTER_LENGTH]").rules("add", {
            required: false,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { number: "Translate(MsgLengthOuter)" }
        });
        $("input[id$=IPD_INNER_LENGTH]").rules("add", {
            required: false,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { number: "Translate(MsgLength)" }
        });
        $("input[id$=IPD_INNER_HEIGHT]").rules("add", {
            required: false,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { number: "Translate(MsgHeight)" }
        });
        $("input[id$=IPD_INNER_BREADTH]").rules("add", {
            required: false,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { number: "Translate(MsgWidth)" }
        });
        $("input[id$=IPD_OUTER_HEIGHT]").rules("add", {
            required: false,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { number: "Translate(MsgHeightOuter)" }
        });
        $("input[id$=IPD_OUTER_BREADTH]").rules("add", {
            required: false,
            number: true,
            digits: true,
            range: [0, 999],
            messages: { number: "Translate(MsgWidthOuter)" }
        });
        $("input[id$=ITM_WEIGHT]").rules("add", {
            required: false,
            number: true,
            FiveDecimal: true,
            /*digits: true,*/
            range: [0, 2000],
            messages: { number: "Translate(MsgWeight)" }
        });
        $("input[id$=IPD_PLY]").rules("add", {
            required: false,
            maxlength: 100
            //ThreeDecimal: true
            //            messages: { ThreeDecimal: "Hi" }
        });
        //        $("input[id$=IPD_THICKNESS]").rules("add", {
        //            required: false,
        //            number: true,
        //            //            digits: true,
        //            maxlength: 12,
        //            messages: { CustomDecimal: "Translate(MsgThickness)" }
        //        });
    }
    else if (mode == 5) {
        $("select[id$=TC_BOM_CAT]").rules("add", {
            selectNone: true,
            messages: { selectNone: MaterialMaster.MaterialCategoryValidation }
        });

        $("[id$=txtBOMaterial]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: MaterialMaster.MaterialValidation }
        });
        $("input[id$=txtPcsInKG]").rules("add", {
            required: true,
            DecimalDigits: 4,
            CustomDecimal: true,
            messages: { required: MaterialMaster.PcsInKGValidation }
        });

    }
   
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    //    $("input[id$=ITV_TAX_PERC]").rules("remove");
    //    $("input[id$=ITV_DISC_PERC]").rules("remove");
    $("input[id$=ITV_LEAD_TIME]").rules("remove");
    //New End
    $("select[id$=ITC_BOM_CAT]").rules("remove");
    $("input[id$=txtBOMaterial]").rules("remove");
    $("input[id$=txtPcsInKG]").rules("remove");

    //$("input[id$=]").rules("remove");
    //$("input[id$=txtBOMaterial]").rules("remove");
    //$("input[id$=txtPcsInKG]").rules("remove");
    $("select[id$=ITV_MOQ_UOM]").rules("remove");
    $("select[id$=ITV_CURRENCY]").rules("remove");
    $("select[id$=ITV_VENDOR]").rules("remove");
    $("select[id$=UOM_PK]").rules("remove");
    $("select[id$=ITM_UOM_PURCHASE]").rules("remove");
    $("select[id$=ITM_UOM_SALE]").rules("remove");
    //    $("select[id$=UOM_PK]").removeClass();
    $("select[id$=ITM_TYPE_TEXT]").rules("remove");
    $("select[id$=ITC_PK]").rules("remove");


    $("input[id$=ITM_CODE]").rules("remove");
    $("input[id$=ITM_NAME]").rules("remove");
    $("input[id$=ITM_MIN_STK]").rules("remove");
    //    $("input[id$=ITM_MAX_STK]").rules("remove");
    $("input[id$=ITM_ROL_STK]").rules("remove");
    $("input[id$=ITM_PHR]").rules("remove");
    $("input[id$=ITM_TSC]").rules("remove");
    $("input[id$=ITV_NAME]").rules("remove");
    $("input[id$=ITV_PRICE]").rules("remove");
    $("input[id$=ITV_MOQ]").rules("remove");
    //$("[id$=ChooseTax]").rules("remove");
    //$("input[id$=ITM_CODE]").removeClass();
    //    $("input[id$=ITM_NAME]").removeClass();
    //    $("select[id$=ITC_PK]").removeClass();
    $("input[id$=ITM_BATCH_CODE]").rules("remove");
    //    $("input[id$=ITM_BATCH_CODE]").removeClass();

}
///#endregion

///#region----------- Vendor Mapping ------------
function FillMaterialForVendorMapping(data) {
    //<summary>function To Fill material  Details in the header of vendor mapping tab. </summary>
    $("input[id$=ITM_PK]").val(data);
    $("[id$=MaterialCode]").html($("input[id$=ITM_CODE]").val())
    $("[id$=MaterialName]").html($("input[id$=ITM_NAME]").val())
    $("input[id$=ITV_NAME]").val($("input[id$=ITM_NAME]").val());
    $("[id$=MaterialCategoryName]").html($("select[id$=ITC_PK] option:selected").text())
    FillVendorUOM($("input[id$=ITM_PK]").val());
    $("[id$=AddressSave]").show();
}

function FillVendor(VEN_PK) {
    //<summary>function To Fill vendor Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=ITV_VENDOR]").attr("id");
    $("select[id$=ITV_VENDOR]").removeData();
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.FillVendorDropdownURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val() + "&VenPK=" + VEN_PK, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, VEN_PK);
    });
}

//## Sumesh 07112011
function FillCurrencyByVendor() {
    $.get("VendorRegistration.do?Action=GetVendorDtls&VendorId=" + $("[id$=ITV_VENDOR]").val(), function (data) {
        if (data.length > 0) {
            $("select[id$=ITV_CURRENCY]").val(data[0].VEN_CURRENCY);
            $("select[id$=ITV_CURRENCY]").attr("disabled", true);
        }
    });
}

function FillCurrency() {
    //<summary>function To Fill currency Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=ITV_CURRENCY]").attr("id");
    $("select[id$=ITV_CURRENCY]").removeData();
    //Fill currency Details to the currency DropDown, Name as Text, PK as Value
    $.get(MaterialMaster.GetCurrency + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

}

function FillVendorUOM(materialPK) {
    //<summary>function To Fill Vendor UOM Details </summary>
    // Get id of the VendorUOM DropDown
    var drpID = $("select[id$=ITV_MOQ_UOM]").attr("id");
    $("select[id$=ITV_MOQ_UOM]").removeData();
    //Fill VendorUOM Details to the VendorUOM DropDown, Name as Text, PK as Value    
    $.get(MaterialMaster.FillVendorUOMDropdownURL + MaterialMaster.Param1 + materialPK, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);

        $("select[id$=ITV_MOQ_UOM]").each(function () {
            $('option', this).each(function () {
                if ($.trim($(this).text().toLowerCase()) == $.trim(uomName.toLowerCase())) {
                    $(this).attr('selected', 'selected');
                };
            });
        });

    });

}

function AddMappingDetails() {
    //<summary>function used to add vendor mapping details</summary>
    //Add Validation for Evaluation Details by setting mode as 2
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        var ObjMapping = $("#divMappingData").data("MappingData");
        var editMapping = $("input[id$=EditMapping]").val();
        var obj = new Object();
        var flag = true;

        //Loop used to check the vendor already added in the mapping List 
        if (parseInt(editMapping) == 0) {
            for (var i in ObjMapping.MappingDetailsList) {
                if (ObjMapping.MappingDetailsList[i].ITV_VENDOR == parseInt($("select[id$=ITV_VENDOR]").val())) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var j in ObjMapping.MappingDetailsList) {
                if (ObjMapping.MappingDetailsList[j].ITV_VENDOR == parseInt($("select[id$=ITV_VENDOR]").val()) && parseInt(editMapping) != ObjMapping.MappingDetailsList[j].ITV_VENDOR)
                    flag = false;
                break;
            }
            for (var k in ObjMapping.MappingDetailsList) {
                if (parseInt(editMapping) == ObjMapping.MappingDetailsList[k].ITV_VENDOR)
                    obj = ObjMapping.MappingDetailsList[k];
            }
        }

        if (flag) {
            obj.ITV_VENDOR = parseInt($("select[id$=ITV_VENDOR]").val());
            obj.ITV_VENDORNAME = $("select[id$=ITV_VENDOR] option:selected").text();
            obj.ITV_NAME = $("input[id$=ITV_NAME]").val();
            obj.ITV_PRICE = $("input[id$=ITV_PRICE]").val();
            obj.ITV_CURRENCY = parseInt($("select[id$=ITV_CURRENCY]").val());
            obj.MaterialCurrencyText = $("select[id$=ITV_CURRENCY] option:selected").text();
            obj.ITV_MOQ = $("input[id$=ITV_MOQ]").val();
            obj.ITV_MOQ_UOM = parseInt($("select[id$=ITV_MOQ_UOM]").val());
            obj.ITV_ACTIVE = $("[id$=ddlActive]").val();
            // obj.ITV_TAX_PERC = $("input[id$=ITV_TAX_PERC]").val();
            //            obj.ITV_TAX_PERC = $("input[id$=ITV_TAX_PERC]").val() == "" ? "0.00" : $("input[id$=ITV_TAX_PERC]").val();            
            //            obj.ITV_DISC_PERC = $("input[id$=ITV_DISC_PERC]").val() == "" ? "0.00" : $("input[id$=ITV_DISC_PERC]").val();
            obj.ITV_TAX_PERC = 0;
            obj.ITV_DISC_PERC = 0;
            if (obj.ITV_SL_NO == null || obj.ITV_SL_NO == 0) {
                var maxSlNo = JSLINQ(ObjMapping.MappingDetailsList)
                    .Max(function (itm) { return itm.ITV_SL_NO; });
                obj.ITV_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
            }
            obj.ITV_LEAD_TIME = $("input[id$=ITV_LEAD_TIME]").val() == "" ? "0" : $("input[id$=ITV_LEAD_TIME]").val();
            //New end
            obj.UOMText = $("select[id$=ITV_MOQ_UOM] option:selected").text();
            if (parseInt(editMapping) == 0) {
                obj.ITV_PK = "0";
                ObjMapping.MappingDetailsList.push(obj);
            }
            $("#divMappingData").data("MappingData", ObjMapping);
            ClearProductDetails();
            GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, ObjMapping.MappingDetailsList);
        }
        else {
            GrandScriptUtils.ShowModal(MaterialMaster.VendorExistsMessage, MaterialMaster.MessageBoxTitle);
        }
        return false;
    }
}

function AfterGridBind(gridID) {
    var colIndex = 0;
    var stock = "";
    //<summary>function Call Afer binding Grid</summary>
    if (gridID == "grdVendorMaterialDetails") {
        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#ProductInsert").find("tr:eq(1)");
        }
        $("#ProductInsert").hide();
        // $("#ProductInsert").css({ "display": "none", "visibility": "hidden" });
        $(tdset).insertBefore($("#grdVendorMaterialDetails").find("tr:eq(1)"));
        $("#grdVendorMaterialDetails").find("tr:has(td)").each(function () {
            var slNo = GrandGrid.Utilities.GetColumnValue($(this), MaterialMaster.ITV_SL_NO, $(this).parents("table:first").attr("id"));
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialMaster.ITV_DISC_PERC, $(this).parents("table:first").attr("id"));
            if (colIndex != null && parseInt($("[id$='isDiscountAdd']").val()) > 0) {
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscount('" + slNo + "');\" src=\"../Images/Classic/Icons/discount.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />");
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialMaster.ITV_TAX_PERC, $(this).parents("table:first").attr("id"));
            if (colIndex != null && parseInt($("[id$='isTaxAdd']").val()) > 0) {
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "');\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" />");
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_ACTIVE_TEXT", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                active = GrandGrid.Utilities.GetColumnValue($(this), "ITV_ACTIVE", $(this).parents("table:first").attr("id"));
                if (active == "1")
                    active = "Yes";
                else
                    active = "No";
                $(this).find("td:eq(" + colIndex + ")").html(active);
            }

        });
    }

    if (gridID == "grdPackingList") {
        $("#grdPackingList tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_MIN_STK", gridID);
            stock = GrandGrid.Utilities.GetColumnValue($(this), "ITM_MIN_STK", gridID);
            if (colIndex != 0) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (stock == "null" || stock == "") {
                    stock = "0";
                }
                $(this).find("td:eq(" + colIndex + ")").append(stock);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_MAX_STK", gridID);
            stock = GrandGrid.Utilities.GetColumnValue($(this), "ITM_MAX_STK", gridID);
            if (colIndex != 0) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (stock == "null" || stock == "") {
                    stock = "0";
                }
                $(this).find("td:eq(" + colIndex + ")").append(stock);
            }
            // Replace Active/InActive status  with corresponding image     
            colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_ACTIVE_TEXT", gridID);
            if (colIndexActive != null) {
                var itemActiveText = $(this).find("td:eq(" + colIndexActive + ")").html();
                if (itemActiveText == "Active") {
                    $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                }
                else {
                    $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                }
            }
        });
    }

    if (gridID == "grdCategory") {
        $("#grdCategory tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_MIN_STK", gridID);
            stock = GrandGrid.Utilities.GetColumnValue($(this), "ITM_MIN_STK", gridID);
            if (colIndex != 0) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (stock == "null" || stock == "") {
                    stock = "0";
                }
                $(this).find("td:eq(" + colIndex + ")").append(stock);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_MAX_STK", gridID);
            stock = GrandGrid.Utilities.GetColumnValue($(this), "ITM_MAX_STK", gridID);
            if (colIndex != 0) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (stock == "null" || stock == "") {
                    stock = "0";
                }
                $(this).find("td:eq(" + colIndex + ")").append(stock);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_NAME", gridID);
            stock = GrandGrid.Utilities.GetColumnValue($(this), "ITM_NAME", gridID);
            if (colIndex != 0) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (stock != "null" || stock != "") {
                    stock = stock.toUpperCase();
                }
                $(this).find("td:eq(" + colIndex + ")").append(stock);
            }
            // Replace Active/InActive status  with corresponding image     
            colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_ACTIVE_TEXT", gridID);
            if (colIndexActive != null) {
                var itemActiveText = $(this).find("td:eq(" + colIndexActive + ")").html();
                if (itemActiveText == "Active") {
                    $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                }
                else {
                    $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                }
            }
        });
    }

    if (gridID == "grdVendorRate") {
        $("#grdVendorRate").find("tr:has(td)").each(function () {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_ACTIVE_TEXT", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                active = GrandGrid.Utilities.GetColumnValue($(this), "ITV_ACTIVE", $(this).parents("table:first").attr("id"));
                if (active == "1")
                    active = "Yes";
                else
                    active = "No";
                $(this).find("td:eq(" + colIndex + ")").html(active);
            }

        });
    }
    $("#grdServiceList tr:has(td)").each(function (index) {
        if ($("[id$=ITM_SET]").val() == 8) {//show vendor rate button in asset master.
            $(this).find("td:last input[id$=imgbtnShowvendorRate]").show();
        }
        else {
            $(this).find("td:last input[id$=imgbtnShowvendorRate]").hide();
        }
        // Replace Active/InActive status  with corresponding image     
        colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_ACTIVE_TEXT", "grdServiceList");
        if (colIndexActive != null) {
            var itemActiveText = $(this).find("td:eq(" + colIndexActive + ")").html();
            if (itemActiveText == "Active") {
                $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
            }
            else {
                $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
            }
        }
    });

}

function FillVendorMappingXmlDetails(itemPk) {
    //<summary>function used to fill vendor mapping details to grid </summary>
    /// <param name="itemPk"  type="object">
    ///     Specific itemPk to fill vendor mapping details
    /// </param>
    var ObjMapping = new Object();
    $.getJSON(MaterialMaster.VendorGetXmlURL + itemPk, function (data) {
        if (data) {
            $("[id$=MappingDetailsList]").val(JSON.stringify(data.MappingDetailsList));
            ObjMapping = data;
            if (ObjMapping.MappingDetailsList == undefined) {
                ObjMapping.MappingDetailsList = new Array();
                $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
                $("#ProductInsert").show();
                // $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
            }
            else if (!($.isArray(ObjMapping.MappingDetailsList))) {
                var objArray = ObjMapping.MappingDetailsList;
                ObjMapping.MappingDetailsList = new Array();
                ObjMapping.MappingDetailsList.push(objArray);
            }

            var taxDetails = new Array();
            if (ObjMapping.MappingDetailsList.length > 0) {
                for (var itv in ObjMapping.MappingDetailsList) {
                    if (!($.isArray(ObjMapping.MappingDetailsList[itv].TaxDtl))) {
                        if (ObjMapping.MappingDetailsList[itv].TaxDtl != undefined) {
                            objArray = ObjMapping.MappingDetailsList[itv].TaxDtl;
                            ObjMapping.MappingDetailsList[itv].TaxDtl = new Array();
                            ObjMapping.MappingDetailsList[itv].TaxDtl.push(objArray);
                        }
                        else {
                            ObjMapping.MappingDetailsList[itv].TaxDtl = new Array();
                        }
                    }
                    if (ObjMapping.MappingDetailsList[itv].TaxDtl.length > 0) {
                        for (var txDtl in ObjMapping.MappingDetailsList[itv].TaxDtl)
                            taxDetails.push(ObjMapping.MappingDetailsList[itv].TaxDtl[txDtl]);
                    }
                }
            }
            $("#divTaxData").data("TaxDetails", taxDetails);

            $("#divMappingData").data("MappingData", ObjMapping);
            GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, ObjMapping.MappingDetailsList);
        }
    });
}

function GetSelectedStores() {
    //<summary>Function Used to get the all checked dept details </summary>
    var ObjMaterialStores = new Array();
    var obj = new Object();

    $("#trvStores").find("input[type=checkbox]:checked").each(function () {

        materialPK = $(this).attr("id");
        obj = new Object();
        materialPK = materialPK.substr(materialPK.lastIndexOf("_") + 1, materialPK.length);
        obj.ITM_DEPT = materialPK;
        ObjMaterialStores.push(obj);
    });
    return ObjMaterialStores;
}
///<summary>Function used trim object Properties  </summary>
function TrimObjectProperties(objectToTrim) {
    for (var key in objectToTrim) {
        if (objectToTrim[key].constructor && objectToTrim[key].constructor == Object)
            trimObjectProperties(objectToTrim[key]);
        else if (objectToTrim[key].trim)
            objectToTrim[key] = objectToTrim[key].trim();
    }
}

function SaveVendorPage() {
    ///<summary>Function used to saving vendor mapping detials  </summary>
    RemoveValidations();
    var ObjMapping = $("#divMappingData").data("MappingData");
    // Check Have The MappingDetailsList have More than or equal to one Mapping Details
    // if (ObjMapping.MappingDetailsList.length > 0) {
    if ($(document.forms[0]).valid()) {
        SetItemTaxDetails();
        ObjMapping = $("#divMappingData").data("MappingData");
        // TrimObjectProperties(ObjMapping.MappingDetailsList);
        //Delete Null Array
        for (var i in ObjMapping.MappingDetailsList) {
            if (ObjMapping.MappingDetailsList[i].TaxDtl.length == 0) {
                delete ObjMapping.MappingDetailsList[i].TaxDtl;
            }
        }
        $("[id$=MappingDetailsList]").val(JSON.stringify(ObjMapping.MappingDetailsList));
        //making json string 
        $("[id$=TEMPFILELIST]").val('');
        var ObjFile = $("#divFileData").data("FileData");
        if (ObjFile == undefined || ObjFile == null && ObjFile == "") {
            ClearFileList();
            $("[id$=FILELIST]").remove();
        }
        if ($("[id$=FILELIST]").val() == "undefined" || $("[id$=FILELIST]").val() == "")//Edit for Bug15748 
            $("[id$=FILELIST]").remove();
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(MaterialMaster.VendorSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(MaterialMaster.VendorExistsMessage, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveVendorCommand);
            }
            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(MaterialMaster.VendorSaveMessage, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveVendorCommand);
                $("input[id$=MaterialDetailId]").val(data);
                $("input[id$=hdfIsFinalTab]").val("1"); //For identifying next we go to the final tab 
                //ResetPage();
                $("#trvStores span").each(function () {     //For avoiding changing of mouse pointer to hand sign  in store mapping tab                           
                    $(this).css('cursor', 'default');
                });
            }
            else if (parseInt(data) == -31) {
                GrandScriptUtils.ShowModal(MaterialMaster.UsedMaterialInAnotherPlace, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveVendorCommand);
            }
            else if (parseInt(data) == -61) {// insert entry failed to history table INV_ITEM_VENDOR_HISTORY 
                GrandScriptUtils.ShowModal(MaterialMaster.InsertionEntryFailed, MaterialMaster.MessageBoxTitle, MaterialMaster.SaveVendorCommand);
            }
            else {
                GrandScriptUtils.ShowModal(MaterialMaster.ActionFailedMessage);
                ResetPage();
            }
        });
    }
    // }
    //    else {
    //        AddValidations(2);
    //    }

    return false;
}
function CallBalck() {

}
function ClearMaterialDetails() {
    //<summary>function used to Clear mapping  Details</summary>
    $("select[id$=ITC_PK]").val("0");
    $("select[id$=UOM_PK]").val("0");
    $("input[id$=ITM_CODE]").val("");
    $("input[id$=ITM_NAME]").val("");
    //    $("select[id$=ITM_TYPE_TEXT]").val("0");
    $("select[id$=ITM_TYPE_TEXT]").val("0");
    $("textarea[id$=ITM_DESC]").html("");
    $("input[id$=ITM_MIN_STK]").val("0");
    $("input[id$=ITM_ROL_STK]").val("0");

    $("input[id$=ITM_MAX_STK]").val("0");

    $("input[id$=MaterialTaskID]").val("0");
    $("input[id$=MaterialReferenceID]").val("0");
    $("input[id$=MaterialApplicationID]").val("0");
    $("input[id$=MaterialActionID]").val("0");
    $("select[id$=ITM_GST_CLASS]").val("0");
}

function ClearProductDetails() {
    //<summary>function used to Clear mapping  Details</summary>
    $("select[id$=ITV_VENDOR]").val("0");
    $("select[id$=ITV_CURRENCY]").val("0");
    //    $("input[id$=ITV_NAME]").val("");
    $("input[id$=ITV_PRICE]").val("");
    $("select[id$=ITV_MOQ_UOM]").val("0");
    $("input[id$=EditMapping]").val("0");
    $("input[id$=ITV_MOQ]").val("0.00");
    $("select[id$=ddlActive]").val("1");
    //    $("input[id$=ITV_TAX_PERC]").val("");    
    //    $("input[id$=ITV_DISC_PERC]").val("");
    $("input[id$=ITV_LEAD_TIME]").val("0");
    //New End
}

function ClearMaterialHeader() {
    //<summary>function used to Clear material  Details from vendor tab</summary>
    $("[id$=MaterialDetailId]").val("0");
    $("[id$=MaterialCode]").html("");
    $("[id$=MaterialName]").html("");
    $("[id$=MaterialCategoryName]").html("");

}

function MakeNumeric(event) {
    ///<summary>function used to make text box Numeric only</summary>
    /// <param name= "event"  type="Object">
    /// Object Used to get the Key Pressed
    /// </param>

    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}

///#region----Grid Handlers And Model Popup Ok Click----
function GridVendorHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    /// <param name="command"  type="Object">
    ///  Specific command for action       
    /// </param>
    // RemoveValidations();
    switch (command.toString()) {
        case MaterialMaster.DeleteCommand:
            // Do Confirmation.. Before Delete Details
            vendorID = GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorID, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(MaterialMaster.DeleteConfirmationMessage, MaterialMaster.ConfirmationMessage, MaterialMaster.DeleteVendorCommand, true);
            break;
        case MaterialMaster.EditCommand:
            $(document.forms[0]).validate().resetForm();
            FillVendorDetails(tr);
            return false;
            break;
        default:
            alert(MaterialMaster.DefaultAction);
            return false;
            break;
    }
    return false;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="Object">
    ///  Specific command for action edit/save/delete       
    /// </param>
    switch (command) {

        case MaterialMaster.SaveVendorCommand:
            var isWrkf = false;
            var queryStr = window.location.search.substring(1);
            if (queryStr != "") {
                var queryStr = queryStr.split("&")
                for (var i = 0; i < queryStr.length; i++) {
                    var pK = queryStr[i].split("=");
                    if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                        isWrkf = true;
                    }
                }
            }
            if (!isWrkf) {
                //                AfterSave();
                //                PageInit();
                //ResetPage();
                $("[id$=tabs]").tabs("enable", 2);
                $("[id$=tabs]").tabs("select", 2);
                $("[id$=btnSave]").show();
                $("[id$=AddressSave]").hide();
            }
            else {
                window.location = "MaterialMaster.aspx";
            }

            break;
        case MaterialMaster.SaveCommand:
            // $("[id$=tabs]").tabs("disable", 0);
            if ($("[id$=hdfIsFinalTab]").val() == 1)//THis is the final tab
            {
                $("[id$=MaterialDetailId]").val('0');
                ResetPage();
            }
            else {
                $("[id$=tabs]").tabs("enable", 1);
                $("[id$=tabs]").tabs("select", 1);
                $("[id$=btnSave]").hide();
            }
            break;
        //function for deleting vendordetails         
        case MaterialMaster.DeleteVendorCommand:
            DeleteVendorDetails();
            break;
        //Commend When calling             
        case MaterialMaster.DeleteVendorMessageCommand:
            GrandScriptUtils.ShowModal(MaterialMaster.DeleteConfirmationMessage, MaterialMaster.ConfirmationMessage);
            break;
        case MaterialMaster.DeleteCommand:
            BindGrid();
            break;
        //Commend When calling           
        case MaterialMaster.DeleteMessageCommand:
            DeleteDetails();
            break;
        case MaterialMaster.RelDeleteMessageCommand:
            DeleteRelDetails();
            break;
        case MaterialMaster.BOMDeleteMessageCommand:
            DeleteBOMDetails();
            break;
        case MaterialMaster.TaxDelete:
            //DeleteTaxDetails();
            DeleteTempDetails();
            break;
    }
    return false;
}

function FillVendorDetails(tr) {
    ///<summary>Used fill Details of requisition for editing</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    $("select[id$=ddlActive]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITV_ACTIVE", $(tr).parent().attr("id")));
    FillVendor(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorID, $(tr).parent().attr("id")))
    //$("select[id$=ITV_VENDOR]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorID, $(tr).parent().attr("id")));
    $("select[id$=ITV_CURRENCY]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorCurrencyID, $(tr).parent().attr("id")));
    $("input[id$=ITV_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorMaterialName, $(tr).parent().attr("id")));
    $("input[id$=ITV_PRICE]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorPrice, $(tr).parent().attr("id")));
    $("input[id$=ITV_MOQ]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorMOQ, $(tr).parent().attr("id")));
    $("select[id$=ITV_MOQ_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorMOQUOMID, $(tr).parent().attr("id")));

    //    $("input[id$=ITV_TAX_PERC]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorTAX, $(tr).parent().attr("id")));
    //    $("input[id$=ITV_DISC_PERC]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorDiscount, $(tr).parent().attr("id")));
    //New start
    $("input[id$=ITV_LEAD_TIME]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.ITV_LEAD_TIME, $(tr).parent().attr("id")));
    //New End
    $("input[id$=EditMapping]").val(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.VendorID, $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("input[id$=ITV_VENDOR]").focus();
}

function DeleteVendorDetails(tr) {
    ///<summary>Used fill Details of requisition for Delete</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>  
    var ObjMapping = $("#divMappingData").data("MappingData");
    for (var i in ObjMapping.MappingDetailsList) {
        if (ObjMapping.MappingDetailsList[i].ITV_VENDOR == vendorID) {
            //Will delete the Evaluation details
            slNo = ObjMapping.MappingDetailsList[i].ITV_SL_NO;
            var TaxDetails = $("#divTaxData").data("TaxDetails");
            for (var j = 0; j < TaxDetails.length; j++) {
                if (TaxDetails[j].IVT_SL_NO == slNo) {
                    TaxDetails.splice(j, 1);
                    --j;
                }
            }
            ObjMapping.MappingDetailsList.splice(i, 1);
            $("#divTaxData").data("TaxDetails", TaxDetails);
            break;
        }
    }
    $("#divMappingData").data("MappingData", ObjMapping);
    GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, ObjMapping.MappingDetailsList);
    //Used to Show the  Evaluation details when the requisition in requisition details is 0
    if (ObjMapping.MappingDetailsList.length == 0) {
        //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
        //  $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
    }
}

function AfterSave() {
    //<summary>function used to clear mapping details  from grid</summary>
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, dummyObj);
    $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
    $("#ProductInsert").show();
    //$("#ProductInsert").css({ "display": "block", "visibility": "visible" });
    var TaxArray = new Array();
    $("#divTaxData").data("TaxDetails", TaxArray);
    ClearMaterialHeader();
}
///#endregion
function FillTaxDiscount(category) {
    ///<summary>function To Fill tax Details </summary>
    var drpID, PohDate;
    drpID = $("[id$=ChooseTax]").attr("id");
    PohDate = $("[id$=hdfCurDate]").val();
    var reqString = MaterialMaster.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDate=" + PohDate + (category == 1 ? MaterialMaster.SpecialCond : "");
    //    $.get(MaterialMaster.GetCategoryTaxDiscount + category + "&Active=1", function (data) {
    $.get(reqString, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}
function AddLineItemTax(slNo) {
    //<summary>function used to add the item tax details</summary>    
    RemoveValidations();
    FillTaxDiscount(1);
    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(TaxDetails)"
        });
    MaterialMaster.ItemTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(1);
    ClearPopUp(slNo, 1);
    ClearTaxDetails();
    //Clone TaxDetails Data to TempDetails   
    var TaxDetails = $("#divTaxData").data("TaxDetails");     //
    $("#divTaxData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
    //End
}
function AddLineItemDiscount(slNo) {
    //<summary>function used to add the item discount details</summary>
    RemoveValidations();
    FillTaxDiscount(3);
    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(DiscountDetails)"
        });
    MaterialMaster.ItemTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(3);
    ClearPopUp(slNo, 3);
    ClearTaxDetails();
    //Clone TaxDetails Data to TempDetails   
    var TaxDetails = $("#divTaxData").data("TaxDetails");     //
    $("#divTaxData").data("TempDetails", JSON.parse(JSON.stringify(TaxDetails))); //For Cloning Javascript Object
    //End
}
function ClearPopUp(slNo, type, isLine) {
    //<summary>function used to bind the item tax/ discount details</summary>
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, type));
}
function GetTaxDiscountDetails(slNo, type) {
    //<summary>function used to get the tax / discount details </summary>
    var TaxDetails = $("#divTaxData").data("TaxDetails");
    var TaxArray = new Array();
    for (var i in TaxDetails) {
        if (TaxDetails[i].IVT_SL_NO == slNo && TaxDetails[i].IVT_TAX_CATEGORY == type) {
            TaxArray.push(TaxDetails[i]);
        }
    }
    return TaxArray;
}
function ClearTaxDetails() {
    //<summary>function used to clear the tax/ discount details</summary>
    MaterialMaster.EditTax = 0;
    $("[id$=ChooseTax]").val("0");
}
function SaveTaxDiscount() {
    //<summary>function to save tax/discount details</summary>
    var TaxDetails = $("#divTaxData").data("TaxDetails");
    if (TaxDetails == null)
        TaxDetails = new Array();
    var slNo = parseInt(MaterialMaster.ItemTaxPK);
    var itemTax = parseInt($("[id$=ChooseTax]").val());
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    AddValidations(3);
    var TaxDetailsObj = null;
    if ($(document.forms[0]).valid()) {
        if (itemTax > 0) {
            if (slNo > 0) {
                if (MaterialMaster.EditTax == 0) {
                    if (TaxDetails.length > 0) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax && tax.IVT_TAX_CATEGORY == category; })
                            .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IVT_SL_NO = slNo;
                        TaxDetailsObj.IVT_PK = 0;
                        TaxDetailsObj.IVT_TAX = parseInt(itemTax) > 0 ? itemTax : 0;
                        TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(MaterialMaster.TypeAlreadyAdded, MaterialMaster.MessageBoxTitle);
                    }
                }
                else {
                    if (MaterialMaster.EditTax == itemTax) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == MaterialMaster.EditTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.IVT_TAX = itemTax;
                            TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                            TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                            .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(MaterialMaster.TypeAlreadyAdded, MaterialMaster.MessageBoxTitle);
                        }
                    }
                }
                $("#divTaxData").data("TaxDetails", TaxDetails);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, category));
            }
            ClearTaxDetails();
        }
    }
    return false;
}

//Edited 05_01_2014
function AddTaxDiscount() {
    //<summary>function to save tax/discount details</summary>
    var TempDetails = $("#divTaxData").data("TempDetails");
    var TaxDetails = $("#divTaxData").data("TaxDetails");
    if (TaxDetails == null)
        TaxDetails = new Array();
    var slNo = parseInt(MaterialMaster.ItemTaxPK);
    var itemTax = parseInt($("[id$=ChooseTax]").val());
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    AddValidations(3);
    var TaxDetailsObj = null;
    if ($(document.forms[0]).valid()) {
        if (itemTax > 0) {
            if (slNo > 0) {
                if (MaterialMaster.EditTax == 0) {
                    if (TempDetails.length > 0) {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax && tax.IVT_TAX_CATEGORY == category; })
                            .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IVT_SL_NO = slNo;
                        TaxDetailsObj.IVT_PK = 0;
                        TaxDetailsObj.IVT_TAX = parseInt(itemTax) > 0 ? itemTax : 0;
                        TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        TempDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(MaterialMaster.TypeAlreadyAdded, MaterialMaster.MessageBoxTitle);
                    }
                }
                else {
                    if (MaterialMaster.EditTax == itemTax) {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == MaterialMaster.EditTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.IVT_TAX = itemTax;
                            TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                            TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TempDetails)
                            .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax; })
                            .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(MaterialMaster.TypeAlreadyAdded, MaterialMaster.MessageBoxTitle);
                        }
                    }
                }
                $("#divTaxData").data("TempDetails", TempDetails);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(slNo, category));
            }
            ClearTaxDetails();
        }
    }
    return false;
}


function GetTempDetails(slNo, type) {
    //<summary>function used to get the tax / discount details </summary>
    var TaxDetails = $("#divTaxData").data("TempDetails");
    var TaxArray = new Array();
    for (var i in TaxDetails) {
        if (TaxDetails[i].IVT_SL_NO == slNo && TaxDetails[i].IVT_TAX_CATEGORY == type) {
            TaxArray.push(TaxDetails[i]);
        }
    }
    return TaxArray;
}

function DeleteTempDetails() {
    //<summary>function used to delete the tax details</summary>

    var TaxDetails = $("#divTaxData").data("TempDetails");
    for (var i in TaxDetails) {
        if (MaterialMaster.EditTax > 0) {
            if (TaxDetails[i].IVT_SL_NO == MaterialMaster.ItemTaxPK && TaxDetails[i].IVT_TAX == MaterialMaster.EditTax) {
                TaxDetails.splice(i, 1);
            }
        }
    }
    $("#divTaxData").data("TempDetails", TaxDetails);
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTempDetails(MaterialMaster.ItemTaxPK, $("[id$=hdfTaxCategory]").val()));

    MaterialMaster.EditTax = 0;
}



function SaveApply() {
    //<summary>function used to clear the tax/ discount details</summary>
    $("#divItemTax").dialog("close");

    //*************************05_01_2015  Start**************************
    var TempDetails = $("#divTaxData").data("TempDetails");
    var TaxDetails = $("#divTaxData").data("TaxDetails");

    //Remove all items under current tax category from Orginal Taxdetails 
    for (var j = 0; j < TaxDetails.length; j++) {
        if (TaxDetails[j].IVT_TAX_CATEGORY == $("[id$=hdfTaxCategory]").val()) {
            TaxDetails.splice(j, 1);
            --j;
        }
    }
    // Add temporary details to original taxdetails 
    for (var i in TempDetails) {
        if (TempDetails[i].IVT_TAX_CATEGORY == $("[id$=hdfTaxCategory]").val()) {
            TaxDetails.push(TempDetails[i]);
        }
    }

    var slNo = parseInt(MaterialMaster.ItemTaxPK);
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    $("#divTaxData").data("TaxDetails", TaxDetails);
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(slNo, category));
    //end ***************************
    return false;
}
function DeleteTaxDetails() {
    //<summary>function used to delete the tax details</summary>

    var TaxDetails = $("#divTaxData").data("TaxDetails");
    for (var i in TaxDetails) {
        if (MaterialMaster.EditTax > 0) {
            if (TaxDetails[i].IVT_SL_NO == MaterialMaster.ItemTaxPK && TaxDetails[i].IVT_TAX == MaterialMaster.EditTax) {
                TaxDetails.splice(i, 1);
            }
        }
    }
    $("#divTaxData").data("TaxDetails", TaxDetails);
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(MaterialMaster.ItemTaxPK, $("[id$=hdfTaxCategory]").val(), true));

    MaterialMaster.EditTax = 0;
}

function SetItemTaxDetails() {
    //<summary>function used to get the item tax details</summary>
    var ObjVendor = $("#divMappingData").data("MappingData");
    var TaxDetails = $("#divTaxData").data("TaxDetails");
    for (var itv in ObjVendor.MappingDetailsList) {
        slNo = ObjVendor.MappingDetailsList[itv].ITV_SL_NO;
        var TaxArray = new Array();
        for (var i in TaxDetails) {
            if (TaxDetails[i].IVT_SL_NO == slNo) {
                TaxArray.push(TaxDetails[i]);
            }
        }
        ObjVendor.MappingDetailsList[itv].TaxDtl = TaxArray;
    }
    $("#divMappingData").data("MappingData", ObjVendor);
}
///#endregion

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

function ShowVendorRate(itemPK) {
    //<summary>Function Used to Popup Vendor Rate Details</summary>   
    var ObjMapping = new Object();
    $.getJSON(MaterialMaster.VendorGetXmlURL + itemPK, function (data) {
        if (data) {
            ObjMapping = data;
            if (ObjMapping.MappingDetailsList == undefined) {
                $("#grdVendorRate").css({ "visibility": "hidden", "display": "none" });
                $("#grdVendorRate").find("tbody").html("");
                $("#divNodata").remove();
                $("<div id=\"divNodata\" class=\"nodata\" >Translate(NoDataFound)</div>").insertBefore($("#grdVendorRate"));
            }
            else if (!($.isArray(ObjMapping.MappingDetailsList))) {
                var objArray = ObjMapping.MappingDetailsList;
                ObjMapping.MappingDetailsList = new Array();
                ObjMapping.MappingDetailsList.push(objArray);
            }
            if (ObjMapping.MappingDetailsList != undefined) {
                $("#divNodata").remove();
                GrandGrid.MakeGrid($("#grdVendorRate"), 0, ObjMapping.MappingDetailsList);
            }
        }
    });
    $("#divVendorRateDetails").dialog("open");
    $("#divVendorRateDetails").dialog({ "width": 450, "height": 250 });
    return false;
}
function StockUOMChanged() {
    var StockUOM = 0;
    if ($("select[id$=UOM_PK] option:selected").val() != "undefined" && $("select[id$=UOM_PK] option:selected").val() != undefined && $("select[id$=UOM_PK] option:selected").val() !== "") {
        StockUOM = $("select[id$=UOM_PK] option:selected").val();
    }
    FillUOMsHaveConversion($("select[id$=ITM_UOM_PURCHASE]").attr("id"), StockUOM, StockUOM);
    FillUOMsHaveConversion($("select[id$=ITM_UOM_SALE]").attr("id"), StockUOM, StockUOM);
}

//function setNextProductCode(oldCode) {
//    //  GetNextProductCode:"MaterialManagement.do?Action=GetNextProductCode&BizUnit=",
//    $.get(MaterialMaster.GetNextProductCode + $("[id$=BizUnitPk]").val() + "ITM_CODE=" + oldCode, function (data) {
//            if (data) {
//                $("[id$=MaterialDetailId]").val('0');
//                $("[id$=IPD_PK]").val('0');    
//                $("input[id$=ITM_CODE]").val(selectval);
//                GrandScriptUtils.ShowModal(MaterialMaster.NewVerionCreated, MaterialMaster.MessageBoxTitle);
//                $("[id$=btnCopy]").hide();
//            }
//            else {
//              
//            }
//        });
//}

//function validateMaterialTab() {
//    var flag=true;
//    var message='';
//    if($.trim($("input[id$=ITM_CODE]").val())=='')
//    {
//        flag=false;
//        message = "<ul><li>" + MaterialMaster.MaterialCodeValidation + "</li></ul>";
//    }
//    if($.trim($("input[id$=ITM_NAME]").val())=='')
//    {
//        flag=false;
//        message = "<ul><li>" + MaterialMaster.MaterialNameValidation + "</li></ul>";
//    }
//    var categoryDropdown = $("select[id$=ITC_PK]");
//    if (categoryDropdown.length == 0 || $(categoryDropdown).val() == "0") {
//        flag = false;
//        message = "<ul><li>" + MaterialMaster.MaterialCategoryValidation + "</li></ul>";
//    }
//    var uomDropdown = $("select[id$=UOM_PK]");
//    if (uomDropdown.length == 0 || $(uomDropdown).val() == "0") {
//        flag = false;
//        message = "<ul><li>" + MaterialMaster.MaterialUOMValidation + "</li></ul>";
//    }

//    var minStock =$("input[id$=ITM_MIN_STK]").val();
//    var rol = $("input[id$=ITM_ROL_STK]").val();
//    var maxStock = $("input[id$=ITM_MAX_STK]").val();
//    var moq = $("input[id$=ITM_MOQ]").val();
//    var phr = $("input[id$=ITM_PHR]").val();
//    var tsc = $("input[id$=ITM_TSC]").val();

//    minStock = parseFloat(minStock);
//    rol = parseFloat(rol);
//    maxStock = parseFloat(maxStock);
//    moq = parseFloat(moq);
//    phr = parseFloat(phr);
//    tsc = parseFloat(tsc);
//    if ($("input[id$=ITM_MIN_STK]").val() != '' && isNaN(minStock) == true) {
//        flag = false;
//    }
//    if ($("input[id$=ITM_ROL_STK]").val() != '' && isNaN(rol) == true) {
//        flag = false;
//    }
//    if ($("input[id$=ITM_MAX_STK]").val() != '' && isNaN(maxStock) == true) {
//        flag = false;
//    }
//    if ($("input[id$=ITM_MOQ]").val() != '' && isNaN(moq) == true) {
//        flag = false;
//    }
//    if ($("input[id$=ITM_PHR]").val() != '' && isNaN(phr) == true) {
//        flag = false;
//    }
//    if ($("input[id$=ITM_TSC]").val() != '' && isNaN(tsc) == true) {
//        flag = false;
//    }

//    if (isNaN(minStock) == false && isNaN(maxStock) == false && minStock > maxStock){
//        flag = false;
//    }
//    if (isNaN(minStock) == false && isNaN(rol) == false && minStock > rol) {
//        flag = false;
//    }
//    if (isNaN(maxStock) == false && isNaN(rol) == false && maxStock < rol) {
//        flag = false;
//    }

//    if (!flag) {
//        $("a[href=#Material]").click();
//        //href = "#Material"
//        //ShowErrorMessage(message, 'Information');
//        return false;
//    }
//    return true;
//}