/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="GrandTreeMulti.js" />

///#region -----Global Variables-----
var storeID = 0;
//Global variable Declaration
var requisitionJson = new Object();
var tdset = "";
var materialID = 0;
var EditMode = 0;
var RefIdMode = 0; //0-no refid,1-refid mode
var Status = 1;
var IsFromDRCR = 0;
var TRX_TYPE = 2; //2-EMR,4-Opening Stock;
///#endregion

///#region -----Configuration-----
var RequisitionSlip = {
    //Url
    AutoCompleteURL: "StoreRequisitionSlip.do?Action=GetSearchValue",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=",
    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListExceptFG&SBUPk=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetailsForStore&SBUPk=",
    GetCurrentStock: "MaterialManagement.do?Action=GetCurrentStockForStore&SBUPk=",
    FillUOMDropdownURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial&MaterialPK=",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillIssuingType: "ExternalMaterialIssue.do?Action=GetIssuingType&SBUPk=",
    FillIssuingToList: "ExternalMaterialIssue.do?Action=GetIssuingToList&SBUPk=",
    // MaterialCategroyDeptURL: "MaterialCategory.do?Action=GetMaterialCategoryListDeptAuto&AUTOSEARCH=1&DeptPK=",  

    //FillDepartementDropdownURL: "StoreRequisitionSlip.do?Action=GetDepartmentDtls&SBUPk=",
    GETItemNameURL: "MaterialManagement.do?Action=GetItemName&MaterialID=",
    RequisitionSaveURL: "ExternalMaterialIssue.do?Action=SaveExternalMaterialIssueWkf",
    REDIRECTURLAFTERSAVE: "../StoreManagement/EMRList.aspx",
    REDIRECTURLAFTERSAVEFROMINBOX: "../AccountManagement/WorkflowInbox.aspx",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategoryAndStore&SBUPk=",
    GETSRSNoURL: "StoreRequisitionSlip.do?Action=GetSRSNo",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    GetMaterialCategoryExceptFGTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryByType&SBUPk=",
    PRINTURL: "../StoreManagement/ExternalMaterialReceiveReport.aspx",
    MaterialSaveURL: "MaterialManagement.do?Action=SavePage&DepartPK=",
    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",

    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    RequisitionSaveMessage1: "Translate(ExternalMaterialReceipt1)",
    OSSaveMessage1: "Translate(OpeningStock1)",
    RequisitionSaveMessage2: "Translate(ExternalMaterialIssue2)",
    OSSaveMessage2: "Translate(OpeningStock2)",
    ItemAlreadyDeletedMsg: "Translate(Itemsalreadydeletedbyanotheruser)",
    SubmitMessage: "Translate(SubmittedMsg)",
    RequisitionUpdateMessage2: "Translate(RequisitionDetailsUpdated2)",
    EMRSavedMessage: "Translate(ExternalMaterialReceiptSavedMessage)",
    OSSavedMessage: "Translate(OpeningStockSavedMessage)",
    RequisitionCodeExistsMessage: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(ConsumptionDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    MaterialCategoryValidation: "Translate(SelectItemCategory)",
    RequisitionCodeAlreadyAdded: "Translate(AlreadyExists)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    NotEnoughStock: "Translate(NotEnoughStock)",
    RequestQuantityLessCurrentStock: "Translate(ConsumptionQuantityLessCurrentStock)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    CannotReceivePriorDateSendReceive: "Translate(CannotReceivePriorDateSendReceive)",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    StockAdjustmentIsAlreadyDone: "Translate(StockAdjustmentIsAlreadyDone)",
    ReduceRecvdQtyVal: "Translate(ReduceRecvdQtyVal)",
    StockTransferAlreadyDone: "Translate(StockTransferAlreadyDone)",
    Err_FutureDateTransactionNotAllowed: "Translate(Err_FutureDateTransactionNotAllowed)",
    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)",
    //Constants
    DefaultStoreValue: "6",
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    Param: "&MatCagID=",
    ValueEmpty: ' ',
    SAVE: "save",
    //Validation messages
    MaterialCodeValidation: "Translate(SelectItem)",
    MaterialUOMValidation: "Translate(PleaseSelectUOM)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    RequisitionQuantityValidation: "Translate(PleaseProvideQtyReceived)",
    RequisitionValueValidation: "Translate(EnterValue)",
    RequisitionCommentsValidation: "Translate(PleaseProvideComments)",
    RequisitionStoreValidation: "Translate(SelectConsumptionStore)",
    RequisitionRateValidation: "Translate(PleaseProvideRateReceived)",

    SelectReceiptType: "Translate(SelectReceiptType)",
    SelectReceiveFrom: "Translate(SelectReceiveFrom)",
    SelectReceivingStore: "Translate(SelectReceivingStore)",
    SelectMaterialType: "Translate(SelectMaterialType)",

    RequisitionDepartementValidation: "Translate(PleaseselectaDepartement)",
    SelectRequestDetails: "Translate(AddMaterialReceiptDetails)",
    MaterialNameValidation: "Translate(PleaseProvideMaterialName)",
    EnterDate: "Translate(EnterDate)",
    //Fields
    MaterialCode: "ICD_ITEM_TEXT",
    MaterialDate: "ICH_DATE",
    CurrentStock: "ICD_CURRENT_STK",
    MaterialValue: "ICD_VALUE_CONSUMED",
    MaterialQtyRequest: "ICD_QTY_CONSUMED",
    MaterialUOMID: "ICD_UOM",
    MaterialComments: "ICD_REMARKS",
    MaterialExpiryDate: "ICD_EXPIRY_DATE",
    MaterialID: "ICD_ITEM",
    ICD_CRDR_NOTE_DTL: "ICD_CRDR_NOTE_DTL",
    ICH_PK: "ICH_PK",
    MaterialTypePk: "ICD_ITEM_CATEGORY",
    MaterialType: "ICD_ITEM_CATEGORY_TEXT",
    IsReturnable: "ICD_ISRETURNTEXT",
    LotNo: "ICD_LOT_NO",
    ICD_PK: "ICD_PK",
    BizUnitPk: 0,
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    IsModifyEMR: false,
    IsViewMode: false
}
///#endregion

var QtyDec, AmtDec, RateDec;

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
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "Translate(AutoDefaultValue)");
    }, "Translate(Pleaseselectanoption)");

    GrandScriptUtils.DatePicker("ICD_EXPIRY_DATE", false, false);
    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimalDigitP2P']").val();
    if ($("[id$=ConsumptionDtl]").val() == "null") {
        GrandScriptUtils.ShowModal(RequisitionSlip.ItemAlreadyDeletedMsg, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
    }
    else {
        //Initailizing Requisition ProductGrid
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, dummyObj);
        //initialize Requisition Object
        requisitionJson = $.parseJSON($("[id$=ConsumptionDtl]").val());
        $("#divRequisitionData").data("RequisitionData", requisitionJson);
        //Create Date Picker
        GrandScriptUtils.DatePicker("ICH_DATE", false, false);

        $("[id$=ICH_DEPT]").focus();
        //Get Issue Id From the Url and Fill Details - For Edit 
        var queryStr = window.location.search.substring(1);
        //from inbox
        if (queryStr != "") {
            var qstrings = queryStr.split("&")

            for (var i = 0; i < qstrings.length; i++) {
                var TypePK = qstrings[i].split("=");
                if ((TypePK[0] == "TYPE" && TypePK[1] != "")) {
                    TRX_TYPE = TypePK[1];
                }
            }

            for (var i = 0; i < qstrings.length; i++) {
                var TypePK = qstrings[i].split("=");
                if (TypePK[1] == 1 && TypePK[0] == "IsModify") {
                    RequisitionSlip.IsModifyEMR = true;
                    $("[id$=ICH_IS_EDIT]").val("1");
                }
                else if (TypePK[1] != "" && TypePK[0] == "CrDr") {
                    IsFromDRCR = 1;
                }
            }
            for (var i = 0; i < qstrings.length; i++) {
                var pK = queryStr.split("=");
                if ((pK[1] != "" && pK[0] == "IssueID") || IsFromDRCR == 1) {
                    EditMode = 1;
                    FillRequisitionDetails(requisitionJson);
                }
                else if ((pK[1] != "" && pK[0] == "Status")) {
                    EditMode = 1;
                    if ((requisitionJson.MRH_STATUS == "1") || (requisitionJson.MRH_STATUS == "7")) {//1- submitted,7-submit more info
                        RefIdMode = 1; //checking refid has or not and setting flag to 1.if flag=1,it means it has refid and have to change dropdow attribute.
                    }
                    FillRequisitionDetails(requisitionJson);
                }
            }

        }
        //popup section
        $("#divAddMaterial").dialog({
            autoOpen: false,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            },
            beforeClose: function (event, ui) {
                RemovePopupValidations();
            }
        });
        PageInit();
        //Modal popup and tree view default settings
        $("#divCategory").dialog({ autoOpen: false });
        //        $("select[id$=ICH_ISS_RCV_TYPE]").focus();
        $("[id$=ICH_DEPT]").focus();
    }

    $("[id$=ICH_DATE]").click(function () {
        //        $("select[id$=MaterialType]").val('0');
        //        FillCategoryDetails('0');
        FillMaterialCategoryAutoComplete();
    });
    //DateDtlsInit();

});

function FillMaterialCategoryAutoComplete(val) {
    //<summary> Function Used to make material category field as auto complete </summary>

    GrandScriptUtils.MakeAutoComplete("MaterialCategory", RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val() + "&SFG=" + $("[id$=hdnShowSFGCategory]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
    //  GrandScriptUtils.MakeAutoComplete("txtItemCategory", PurchaseRequestConfig.MaterialCategroyDeptURL + $("[id$=hdfDeptID]").val(), "hdfItemCategoryPK", true, false, "BizUnitPk", true); //textbox Below ROL / MSL / Category 
   // ClearGridControlDetailsExeptMaterialType();
    FillCategoryMaterialsAuto(val);

}
function FillCategoryMaterialsAuto(catgID) {
    ///<summary>Function Used to Fill material based on the category  </summary>

    $("[id$=ItemCodeMaterial]").val("");
    $("[id$=MaterialPK]").val(0);
    //$("[id$=MaterialSpec]").val(""); //For Clearing Specification Field
    var storePK = $("select[id$=ICH_DEPT]").val();
    GrandScriptUtils.MakeAutoCompleteLimitLen("ItemCodeMaterial", RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + catgID + "&Store=" + storePK, "MaterialPK", true, false, "MaterialCategoryPK", true, "Store", "", "", $("[id$=AutoStartValue]").val());
}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "MaterialCategory") {
        FillCategoryMaterialsAuto($("[id$=MaterialCategoryPK]").val());

    }
    if (targetControlID == "ItemCodeMaterial") {
        GetCurrentStock($("[id$=MaterialPK]").val());
    }
}

function GetCurrentStock(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var drpID = $("select[id$=ICD_UOM]").attr("id");
    var store = $("select[id$=ICH_DEPT]").val();
    var date = $("[id$=ICH_DATE]").val();
    $.get(RequisitionSlip.GetCurrentStock + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&Store=" + store + "&Date=" + date, function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=CurrentStock]").html(parseFloat(data[0].STD_QTY_IN_STOCK).toFixed(QtyDec));
                //$("select[id$=ICD_UOM]").val(data[0].ITM_UOM);
                FillUOM(materialID, data[0].STD_UOM);
                $("select[id$=ICD_UOM]").removeClass();
                $("[id$=MaterialCategoryPK]").val(data[0].STD_ITEM_CATEGORY);
                $("[id$=MaterialCategory]").val(data[0].STD_ITEM_CATEGORY_TEXT);
            }
            else {
                $("[id$=CurrentStock]").html("");
                // $("select[id$=ICD_UOM]").val("0");
                $("select[id$=ICD_UOM]").removeClass();
                FillUOM(materialID, false);
            }
        }
    });
}

function FillMaterialDetails(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var drpID = $("select[id$=ICD_UOM]").attr("id");
    $.get(RequisitionSlip.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
        if (data) {
            if (materialID != 0) {
               // $("[id$=MaterialName]").html(data[0].ITM_NAME);
                //$("select[id$=ICD_UOM]").val(data[0].ITM_UOM);
                FillUOM(materialID, data[0].ITM_UOM);
              
            }
            else {
              //  $("[id$=MaterialName]").html("");
                // $("select[id$=ICD_UOM]").val("0");
                FillUOM(materialID, false);
            }
        }
    });
}
function DateDtlsInit() {
    //<summary>Function Used to make details datetime picker</summary>
    GrandScriptUtils.DatePicker("ICD_EXPIRY_DATE", false, false);
    $("[id$=ICD_EXPIRY_DATE]").val("");
}

function FillRequisitionDetails(requisitionJson) {
    ///<summary>Used to fill requisition Details for editing</summary>
    // var drpID = $("select[id$=Product]").attr("id");
    if (requisitionJson.ICH_STATUS == 4)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");
    //Header Details.
    $("[id$=ICH_CRDR_NOTE_HDR]").val(requisitionJson.ICH_CRDR_NOTE_HDR);
    $("[id$=ICH_CRDR_FLAG]").val(requisitionJson.ICH_STATUS); //1=>Record should be in View Mode.Using for details comes from CreitDebit
    FillStore(requisitionJson.ICH_DEPT);
    FillDepartement(requisitionJson.ICH_DEPT);
    FillIsuingType(requisitionJson.ICH_ISS_RCV_TYPE);
    FillIssuingToList(requisitionJson.ICH_ISS_RCV_PK);
    FillCompany(requisitionJson.ICH_COMPANY);

    $("[id$=ICH_DATE]").val(requisitionJson.ICH_DATE)
    FillTypes(requisitionJson.ICH_ITEM_TYPE);
    $("input[id$=ICH_PK]").val(requisitionJson.ICH_PK)
    FillMaterialCategoryEdit(requisitionJson.ICH_ITEM_TYPE, requisitionJson.ICH_DEPT);

    if (requisitionJson.ICH_NO == null || requisitionJson.ICH_NO == "") {
        $("input[id$=ICH_NO]").val("");
        $("[id$=lblMaterilaConsumptionNo]").html(RequisitionSlip.DocGenerationNewValue);
    }
    else {
        $("input[id$=ICH_NO]").val(requisitionJson.ICH_NO);
        $("[id$=lblMaterilaConsumptionNo]").html(requisitionJson.ICH_NO);
    }

    $("[id$=LAST_MOD_DT]").val(requisitionJson.LAST_MOD_DT);
    $("[id$=ICH_REF_NO]").val(requisitionJson.ICH_REF_NO);

    // Check requisitionJson.ConsumptionDtl is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign ConsumptionDtl Details to that Array, and then push Array to requisitionJson.ConsumptionDtl
    if (!($.isArray(requisitionJson.ConsumptionDtl))) {
        var objArray = requisitionJson.ConsumptionDtl;
        requisitionJson.ConsumptionDtl = new Array();
        requisitionJson.ConsumptionDtl.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, requisitionJson.ConsumptionDtl);
   
    //DateDtlsInit();
    // $("select[id$=ICH_DEPT]").focus();
}


function PageInit() {
    ///<summary>initial page condition</summary>
    //Reseting all input controls in the page.
    // ResetPage();

    $("[id$=ConfirmStockValueChange]").val('0');
    if (EditMode == 0) {
        //Filling Store dropdown initially.      
        FillStore($("[id$=hdfDeptID]").val());
        FillDepartement(0);
        FillTypes(0);
        FillIsuingType(RequisitionSlip.DefaultStoreValue);
        FillIssueToCategories();
        $("[id$=imbPrint]").hide();
        FillCompany(0);
    }
    if (TRX_TYPE == 4) {
        $("[id$=ICH_TRX_TYPE]").val(4); //For Identifying Opening Stock Entry
        $("[id$=divType]").hide();
        $("[id$=divFrom]").hide();
    }
    //DateDtlsInit();
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=ICH_COMPANY]").attr("disabled", "disabled");
    }

}
///#endregion

function FillIsuingType(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    var drpID = $("select[id$=ICH_ISS_RCV_TYPE]").attr("id");
    $.get(RequisitionSlip.FillIssuingType + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0", function (data) {
        if (drpID != null)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
    });
    $("[id$=hdn_ICH_ISS_RCV_TYPE]").val(SelectedValue);
    if (RefIdMode == 0) {
        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", true);
    }
}

///#region---- Core Section Section----
///#region---- Fetch Data To Populate In Controls
function FillStore(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    var drpID = $("select[id$=ICH_DEPT]").attr("id");
    $.get(RequisitionSlip.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=1&DeptType=2&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
        if (EditMode == 0) { FillNewCategories(); }
    });
    if (RefIdMode == 0) {
        $("select[id$=ICH_DEPT]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        $("select[id$=ICH_DEPT]").attr("disabled", true);
    }
}
function FillTypes(typeID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=ICH_ITEM_TYPE]").attr("id");
    $.get(RequisitionSlip.FillMaterialTypeDropdownURL + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + "Item Type", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, typeID);
    });
}
function FillDepartement(SelectedValue) {
    ///<summary>to fill Departement combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the Departement DropDown Departement
    ClearGridControlDetails();
    var userFlagText = "";
    if (RefIdMode == 0) {
        userFlagText = "1";
        $("select[id$=DeptPk]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        userFlagText = "0";
        $("select[id$=DeptPk]").attr("disabled", true);
    }
    var drpID = "";
    $.get(RequisitionSlip.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=" + userFlagText + "&DeptType=0&DeptPk=0", function (data) {
        drpID = $("select[id$=DeptPk]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        if ($("select[id$=ICH_DEPT]").val() != 0) {
            $("#" + drpID + " option[value=" + $("select[id$=ICH_DEPT]").val() + "]").remove();
            // if ($("select[id$=ICH_ITEM_TYPE]").val() != 0 && $("select[id$=ICH_ITEM_TYPE]").val() != null) {
            FillMaterialCategoryEdit($("select[id$=ICH_ITEM_TYPE]").val(), $("select[id$=ICH_DEPT]").val());
            FillMaterialCategoryAutoComplete(0);
            FillCategoryMaterialsAuto(0);
            // }
        }
        else {
            drpID = $("select[id$=MaterialType]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true);
            drpID = $("select[id$=ITV_ITEM]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true);
            var tempData = new Array();
            AfterSave();
        }
    });
}
function FillNewCategories() {
    var ObjMaterialIssue = new Object();
    ObjMaterialIssue.ConsumptionDtl = new Array();
    $("#divRequisitionData").data("RequisitionData", ObjMaterialIssue);
    AfterSave();
    ClearGridControlDetails();
    //FillMaterialCategory(0);
    FillMaterialCategoryAutoComplete(0);
}

function FillIssueToCategories() {
    //    var ObjMaterialIssue = new Object();
    //    ObjMaterialIssue.ConsumptionDtl = new Array();
    //    $("#divRequisitionData").data("RequisitionData", ObjMaterialIssue);
    //    AfterSave();
    //    ClearGridControlDetails();
    FillIssuingToList(0);
}

function ResetGridControlDetails() {
    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=MaterialPK]").val(0);

    $("[id$=MaterialCategory]").val("Select/Type");
    $("[id$=ItemCodeMaterial]").val("Select/Type");

    $("[id$=ICD_VALUE_CONSUMED]").val("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");
    $("[id$=ICD_EXPIRY_DATE]").val("");
    $("[id$=ICD_LOT_NO]").val("");
    $("input[id$=EditRequisition]").val("0");
    $("input[id$=IsEdit]").val("false");
    FillMaterialCategoryAutoComplete(0);
    FillCategoryMaterialsAuto(0);
    return false;
}

function ClearGridControlDetails() {
    //DateDtlsInit();
    var drpCategoryID = $("select[id$=MaterialType]").attr("id");
   // var drpUItemID = $("select[id$=ITV_ITEM]").attr("id");
    var drpUUOMID = $("select[id$=ICD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpCategoryID, null, true, false);
  //  GrandScriptUtils.FillDropDown(drpUItemID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    //    $("[id$=CurrentStock]").html("");
    $("[id$=ICD_VALUE_CONSUMED]").val("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");
    $("[id$=ICD_EXPIRY_DATE]").val("");
}

function FillIssuingToList(issueTypeID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var issuingType;
    var drpID = $("select[id$=ICH_ISS_RCV_PK]").attr("id");
    if ($("select[id$=ICH_ISS_RCV_TYPE]").val() == null)
        issuingType = $("[id$=hdn_ICH_ISS_RCV_TYPE]").val();
    else
        issuingType = $("select[id$=ICH_ISS_RCV_TYPE]").val();

    //Fill Category Details to the Category DropDown, Name as Text, PK as Value 
    $.get(RequisitionSlip.FillIssuingToList + $("[id$=BizUnitPk]").val() + "&issuingType=" + issuingType, function (data) {

        if (issueTypeID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, issueTypeID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

    if (TRX_TYPE == 4) {
        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", true);
    }
}

function FillMaterialCategory(materialID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value 
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val() + "&SFG=" + $("[id$=hdnShowSFGCategory]").val(), function (data) {
        if (materialID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillMaterialCategoryEdit(itemType, store) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&Store=" + store + "&SFG=" + $("[id$=hdnShowSFGCategory]").val(), function (data) {
        if (materialID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function ClearGridControlDetailsExeptMaterialType() {
 //   var drpUItemID = $("select[id$=ITV_ITEM]").attr("id");
    var drpUUOMID = $("select[id$=ICD_UOM]").attr("id");
  //  GrandScriptUtils.FillDropDown(drpUItemID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    //    $("[id$=CurrentStock]").html("");
    $("[id$=ICD_VALUE_CONSUMED]").val("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");
    $("[id$=ICD_EXPIRY_DATE]").val("");
    //DateDtlsInit();
}

function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
    ClearGridControlDetailsExeptMaterialType();
    //FillCategoryMaterials(categoryID);
    FillCategoryMaterialsAuto(categoryID);
}

function FillCategoryMaterials(categoryID, materialID, typePK) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="categoryID"  type="object">
    ///     Specific categoryid to fill corresponding Material
    /// </param>
    /// <param name="materialID"  type="object">
    ///     Specific materialID to select the dropdown item after filling drop down
    /// </param>
    if ($("select[id$=MaterialType]").val() != 0) {
        //        typePK = $("select[id$=ICH_ITEM_TYPE]").val();
        storePK = $("select[id$=ICH_DEPT]").val();
        var drpID = $("select[id$=ITV_ITEM]").attr("id");
        $.getJSON(RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Store=" + storePK, function (data) {
            if (materialID) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else {
        var drpID = $("select[id$=ITV_ITEM]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, null, true, true);
    }
}

function FillUOM(materialID, selectval) {
    ///<summary>function To Fill Uom Details </summary>
    /// <param name="categoryID"  type="string">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    /// <param name="selectval"  type="string">
    ///     Specific value to be selected.
    /// </param>
    if (materialID != 0) {
        $("select[id$=ICD_UOM]").removeData();
        // Get id of the ICD_UOM_TEXT DropDown
        var drpID = $("select[id$=ICD_UOM]").attr("id");
        //Fill ICD_UOM_TEXT Details to the ICD_UOM_TEXT DropDown, Name as Text, PK as Value

        $.get(RequisitionSlip.FillUOMDropdownURL + materialID, function (data) {
            if (selectval) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else
        $("select[id$=ICD_UOM]").find("option").remove();
}

function BindWorkFlowComment() {
    ///<summary>To handle bind grid </summary>
    GrandScriptUtils.BindWorkFlowCommand("grdWrkfComment");
}





function GetSRSNo() {
    ///<summary>to fill SRSNo span</summary>
    $.get(RequisitionSlip.GETSRSNoURL, function (data) {
        $("[id$=ICH_NO]").val("");
        $("[id$=ICH_NO]").val(data);
        $("[id$=lblSRS]").html(data);
    });
}

function ViewRequisition(requistID) {
    window.location = vendorListing.ViewUrl + requistID;
}

function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    //SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryTreeURL + $("[id$=SBU]").val() + MaterialMaster.Param, "Root", false, false, "0", false); // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryExceptFGTreeURL + $("[id$=BizUnitPk]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val() + "&SFG=" + $("[id$=hdnShowSFGCategory]").val() + RequisitionSlip.Param, "Root", false, false, "0", false); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}
///#endregion

///#region---- Data Management Section----
function ShowCategory() {
    ///<summary>Function used call the tree Data For filling the Material Category </summary>
    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Choose Category", false, "700", false, false);
    return false;
}

//function RemoveDept() {
//    var drpID = $("select[id$=DeptPk]").attr("id");
//    if ($("select[id$=ICH_DEPT]").val() != 0) {
//        $("#" + drpID + " option[value=" + $("select[id$=ICH_DEPT]").val() + "]").remove();
//    }
//    else {
//        FillDepartement(0);
//    }
//}

function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    $("select[id$=MaterialType]").val(cagID);
    //setting selected value for uom after selecting category from treeview.
    //FillCategoryDetails(cagID);
    FillMaterialCategoryAutoComplete(cagID);
    //closing modalbox after selected from treeview.
    $("#divCategory").dialog("destroy");
    $("#divCategory").dialog({ autoOpen: false });
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>
    var colIndex = 0;
    var colVal = 0;
    var retVal = '';
    if (grdID == "grdRequisitionSlip") {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field

        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_REMARKS", $("#grdRequisitionSlip").attr("id"));
        $("#grdRequisitionSlip").find("tr:has(th)").each(function (index) {
            //            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_CURRENT_STK", $(this).parents("table:first").attr("id"));
            //            if (colIndex != null) {
            //                $(this).find("th:eq(" + colIndex + ")").css('text-align', 'right');
            //            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_VALUE_CONSUMED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                $(this).find("th:eq(" + colIndex + ")").css('text-align', 'right');
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_QTY_CONSUMED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                $(this).find("th:eq(" + colIndex + ")").css('text-align', 'right');
            }
        });

        $("#grdRequisitionSlip").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("");
            }
            //            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_CURRENT_STK", $(this).parents("table:first").attr("id"));
            //            if (colIndex != null) {
            //                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_CURRENT_STK", $(this).parents("table:first").attr("id"));
            //                colVal = (colVal == "null") ? "" : colVal;
            //                $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(colVal).toFixed(QtyDec)));
            //                $(this).find("td:eq(" + colIndex + ")").css('text-align', 'right');
            //            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_VALUE_CONSUMED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_VALUE_CONSUMED", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "null") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(colVal).toFixed(RateDec)));
                $(this).find("td:eq(" + colIndex + ")").css('text-align', 'right');
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_QTY_CONSUMED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_QTY_CONSUMED", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "null") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(colVal).toFixed(QtyDec)));
                $(this).find("td:eq(" + colIndex + ")").css('text-align', 'right');
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_EXPIRY_DATE", grdID);
            retVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_EXPIRY_DATE", grdID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), "ICD_EXPIRY_DATE", grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "ICD_EXPIRY_DATE", grdID);
            if (colIndex != null && retVal != null) {
                $(this).find("td:eq(" + colIndex + ")").html(retVal);
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_LOT_NO", grdID);
            retVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_LOT_NO", grdID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), "ICD_LOT_NO", grdID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "ICD_LOT_NO", grdID);
            if (colIndex != null && retVal != null) {
                $(this).find("td:eq(" + colIndex + ")").html(retVal);
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_UOM_TEXT", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_UOM_TEXT", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "undefined") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(colVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_REMARKS", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_REMARKS", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "undefined") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(colVal);
            }

        });
        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#ProductInsert").find("tr:eq(1)");
        }
        $("#ProductInsert").hide();
        $(tdset).insertBefore($("#grdRequisitionSlip").find("tr:eq(1)"));
        var reqDateID = $("#grdRequisitionSlip").find("tr:eq(1)").find("input[id$=ICD_EXPIRY_DATE]").attr("id");
        var reqDateName = $("#grdRequisitionSlip").find("tr:eq(1)").find("input[id$=ICD_EXPIRY_DATE]").attr("name");

        var index = $("#grdRequisitionSlip").find("tr:eq(1)").children().index($("#grdRequisitionSlip").find("tr:eq(1)").find("input[id$=ICD_EXPIRY_DATE]").parent("td:first"));
        $("#grdRequisitionSlip").find("tr:eq(1)").find("input[id$=ICD_EXPIRY_DATE]").remove();
        $("#grdRequisitionSlip").find("tr:eq(1)").find("td:eq(" + index + ")").html("<input id=\"" + reqDateID + "\" name=\"" + reqDateName + "\" type=\"text\" class=\"date-picker\" tabIndex=14 />");
        DateDtlsInit();

        //for hiding action fields of detail section 23-11-11
        var queryStr = window.location.search.substring(1);
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");

        var isViewMode = false;
        var queryStr = window.location.search.substring(1);
        if (queryStr != "") {
            var queryStr = queryStr.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] == 1 && pK[0] == "Status" && RequisitionSlip.IsModifyEMR == false) || (pK[1] == 1 && pK[0] == "Flag") || ($("[id$=ICH_CRDR_FLAG]").val() == 1 && RequisitionSlip.IsModifyEMR == false)) {
                    $("#grdRequisitionSlip th:last").hide();
                    $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                        $(this).find("td:last").hide();
                        $(this).find("td:last").hide();
                        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", true);
                        $("select[id$=ICH_ISS_RCV_PK]").attr("disabled", true);

                        $("select[id$=ICH_DEPT]").attr("disabled", true);
                        $("[id$=ICH_DATE]").attr("disabled", true);
                        $("select[id$=ICH_ITEM_TYPE]").attr("disabled", true);
                        $("[id$=ICH_REF_NO]").attr("disabled", true);
                        $("[id$=btnSave]").hide();
                        $("[id$=btnSaveandSubmit]").hide();
                        $("select[id$=ICH_COMPANY]").attr("disabled", true);
                    });
                }
            }
        }
        //  if (RequisitionSlip.IsModifyEMR == true) { $("[id$=btnSave]").show(); }
        if (ObjRequisition != undefined) {
            if (ObjRequisition.MRH_STATUS == 1 || ObjRequisition.MRH_STATUS == 2 || ObjRequisition.MRH_STATUS == 7) {
                $("#grdRequisitionSlip th:last").hide();
                $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                    $(this).find("td:last").hide();
                    $(this).find("td:last").hide();
                    //                    $("select[id$=DeptPk]").attr("disabled", true);
                    $("select[id$=ICH_DEPT]").attr("disabled", true);
                    $("[id$=ICH_DATE]").attr("disabled", true);
                    $("select[id$=ICH_ITEM_TYPE]").attr("disabled", true);
                    $("select[id$=ICH_COMPANY]").attr("disabled", true);
                });
            }
        }
    }
    //    $("[id$=CurrentStock]").html("");

    $("[id$=ICH_DATE]").attr("disabled", false);
    if (ObjRequisition != undefined) {
        if (ObjRequisition.ConsumptionDtl.length > 0) {
            $("[id$=ICH_DATE]").attr("disabled", true);
        }
    }
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button])").each(function () {
        var idval = $(this).attr("id");
        if (!Checkstatus(idval)) {
            $(this).val("");
        }
    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    window.location = RequisitionSlip.REDIRECTURLAFTERSAVE;
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
    if (controlID.search("MRH_PK") != -1) {
        return true;
    }
    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
    if (controlID.search("EditRequisition") != -1) {
        return true;
    }
    //    if (controlID.search("ITM_PK") != -1) {
    //        return true;
    //    } 
    return false;
}

function PrintPage() {
    ///<summary>Function To Show PRINT Form </summary>
    window.location = RequisitionSlip.PRINTURL + "?IssueID=" + $("input[id$=ICH_PK]").val();
    return false;
}

function SavePage(command) {
    $("[id$=ICH_COMPANY]").attr("disabled", false);
    ///<summary>Function used to saving   </summary>
    $.get(RequisitionSlip.InventoryLockCheckingURL + $("[id$=ICH_DATE]").val() + "&Module=2", function (data) {
        if (data != null && data.length > 0) {
            if (parseInt(data[0]) == 0) {
                SaveEMR(command); //For developer convenience,all codes are just placed under a new function .
            }
            else {
                var Err_TranslockedMsg = RequisitionSlip.ErrTransLockedMsg + RequisitionSlip.ValueEmpty + data[1];
                GrandScriptUtils.ShowModal(Err_TranslockedMsg, RequisitionSlip.MessageBoxTitle);
            }
        }
    });
    return false;
}

function SaveEMR(command) {
    RemoveValidations();
    // RemovePopupValidations();
    AddValidations(2);

    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    // Check Have The ConsumptionDtl have More than or equal to one Requisition Details

    if ($(document.forms[0]).valid()) {
        if (ObjRequisition.ConsumptionDtl.length > 0) {
            //Showing validation for Future Date selection
            if ($("[id$=hdfIsContFutureDate]").val() != "1") {
                var RetVal = CompareDate($("[id$=ICH_DATE]").val(), $("[id$=hdfCurrentDate]").val());
                if (RetVal == 1) {
                    // ShowFutureDate(command);
                    blockFutureDate();
                    return false;
                }
            }
            //Check UOM is selected(Requird for againist DebitCredit only)          
            for (var i = 0; i < ObjRequisition.ConsumptionDtl.length; i++) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_UOM == null || ObjRequisition.ConsumptionDtl[i].ICD_UOM == undefined) {
                    GrandScriptUtils.ShowModal(RequisitionSlip.MaterialUOMValidation.fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                    return false;
                }
            }
            //End          
            //To Prevent Muliple Click
            if ($("[id$=SubmitFlag]").val() == "0")
                $("[id$=SubmitFlag]").val('1')
            else
                return false;

            //            $("select[id$=DeptPk]").attr("disabled", false);
            $("select[id$=ICH_DEPT]").attr("disabled", false);
            $("[id$=ICH_DATE]").attr("disabled", false);
            $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", false);

            ObjRequisition = $("#divRequisitionData").data("RequisitionData");
            //Assigning the Requisition details to a hidden field by converting the object to string using Json Stringify Methord
            $("[id$=ConsumptionDtl]").val(JSON.stringify(ObjRequisition.ConsumptionDtl));
            $("[id$=WKF_FLAG]").val("0");
            $("[id$=WKF_TRX_FLAG]").val("0");
            //making json string 
            //checking command value is draft if it is draft then action id is zero means it is not calling workflow.
            if (command != "Draft") {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
                $("[id$=ICH_STATUS]").val('1');
                $("[id$=WKF_FLAG]").val("1");
                $("[id$=WKF_TRX_FLAG]").val("1");
            }
            else {
                $("[id$=ActionID]").val('0');
                $("[id$=ICH_STATUS]").val('0');
                $("[id$=WKF_TRX_FLAG]").val("0");
                $("[id$=WKF_FLAG]").val("0");
            }

            if (RequisitionSlip.IsModifyEMR == true) {
                $("[id$=ICH_STATUS]").val('1');
            }


            $("[id$=WKF_TASK_ACTION]").val($("[id$=WRKFACT_ID]").val());
            $("[id$=WKF_APPLICATION]").val($("[id$=hdfAppID]").val());
            $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
            $("[id$=WKF_TASK]").val($("[id$=TaskPK]").val());
            $("[id$=WKF_COMMENTS]").val($("[id$=WrkfComments]").val());
            $("[id$=USER_PK]").val($("[id$=UserPk]").val());

            $("[id$=ICH_ISS_RCV_NAME]").val($("select[id$=ICH_ISS_RCV_PK] option:selected").text());
            //  var jSonString = GrandScriptUtils.FormToJsonString("divXml");
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            $.post(RequisitionSlip.RequisitionSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
                if (parseInt(data[0]) == 0) {
                    GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionCodeAlreadyAdded, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                }
                else if (parseInt(data[0]) > 0) {
                    //##### Start Change Code Here #####//
                    // If Action is Draft Save
                    if (command == "Draft") {
                        var SaveMessageWithSRSNo = "";
                        if (TRX_TYPE == 4) {
                            SaveMessageWithSRSNo = RequisitionSlip.OSSaveMessage1 + " " + data[1] + " " + RequisitionSlip.RequisitionSaveMessage2;
                        }
                        else {
                            SaveMessageWithSRSNo = RequisitionSlip.EMRSavedMessage;
                        }
                        GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                        PageInit();
                        AfterSave();
                    }
                    // If action - WorkFlow Save
                    else {
                        $("[id$=hdfAppID]").val(data[0]);
                        $("[id$=AppNo]").val(data[1]);
                        var SaveMessageWithSRSNo = "";
                        if (TRX_TYPE == 4) {
                            SaveMessageWithSRSNo = RequisitionSlip.OSSaveMessage1 + " " + data[1] + " " + RequisitionSlip.SubmitMessage;
                        }
                        else {
                            SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + data[1] + " " + RequisitionSlip.SubmitMessage;
                        }
                        GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                        PageInit();
                        AfterSave();
                    }
                    //##### END Change Code Here #####//
                }
                else if (parseInt(data[0]) == -2) {
                    GrandScriptUtils.ShowModal(data[1] + " " + RequisitionSlip.EditUsedByAnotherUser, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -3) {
                    GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionSaveMessage1 + " " + RequisitionSlip.NotEnoughStock, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -10) {
                    // GrandScriptUtils.ShowModal(RequisitionSlip.CannotReceivePriorDateSendReceive, RequisitionSlip.MessageBoxTitle);
                    fnConfirmStockValueChange(command);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -31) {
                    var s = String.format(RequisitionSlip.StockTransferAlreadyDone, $("[id$=ICH_DATE]").val());
                    GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -32) {
                    var s = String.format(RequisitionSlip.StockAdjustmentIsAlreadyDone, $("[id$=ICH_DATE]").val());
                    GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -35) {
                    GrandScriptUtils.ShowModal(RequisitionSlip.ReduceRecvdQtyVal, RequisitionSlip.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else {
                    GrandScriptUtils.ShowModal(RequisitionSlip.ActionFailedMessage);
                    ResetPage();
                }
            });
        }
        else {
            GrandScriptUtils.ShowModal(RequisitionSlip.SelectRequestDetails, RequisitionSlip.MessageBoxTitle);
            RemoveValidations();
        }
    }

    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var SaveMessageWithSRSNo = "";
    SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.SubmitMessage
    GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
}

function AddRequisitionDetails() {
    //<summary>function used to add Evaluation details to Evaluation</summary>
    //Add Validation for Evaluation Details by setting mode as 2
    RemoveValidations();
    AddValidations(1);
    //DateDtlsInit();
    if ($(document.forms[0]).valid()) {
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
        var editRequisition = $("input[id$=EditRequisition]").val();
        var obj = new Object();
        var flag = true;
        //Loop used to check the Evaluation already added in the order List 
        if (parseInt(editRequisition) == 0) {
            for (var i in ObjRequisition.ConsumptionDtl) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == parseInt($("select[id$=ITV_ITEM]").val())) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjRequisition.ConsumptionDtl) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == parseInt($("select[id$=ITV_ITEM]").val()) && parseInt(editRequisition) != ObjRequisition.ConsumptionDtl[i].ICD_ITEM)
                    flag = false;
                break;
            }
            for (var k in ObjRequisition.ConsumptionDtl) {
                if (parseInt(editRequisition) == ObjRequisition.ConsumptionDtl[k].ICD_ITEM)
                    obj = ObjRequisition.ConsumptionDtl[k];
            }
            if (obj.ICD_ITEM == undefined) {
                editRequisition = 0;
            }
        }

        if (flag) {
            obj.ICD_ITEM = parseInt($("[id$=MaterialPK]").val());
            obj.ICD_PK = $("input[id$=ICD_PK]").val();
            obj.ICD_ITEM_CATEGORY_TEXT = $("[id$=MaterialCategory]").val();
            obj.ICD_ITEM_CATEGORY = parseInt($("[id$=MaterialCategoryPK]").val());
            obj.ICD_ITEM_TEXT = $("[id$=ItemCodeMaterial]").val();
            obj.ICD_LOT_NO = $("input[id$=ICD_LOT_NO]").val() == "" ? " " : $("input[id$=ICD_LOT_NO]").val();

            obj.ICD_QTY_CONSUMED = parseFloat($("input[id$=ICD_QTY_CONSUMED]").val().replace(/[^0-9\.]+/g, "")).toFixed(QtyDec);
            if ($("input[id$=ICD_VALUE_CONSUMED]").val() == '' || $("input[id$=ICD_VALUE_CONSUMED]").val() == NaN) {
                obj.ICD_VALUE_CONSUMED = 0;
            }
            else {
                obj.ICD_VALUE_CONSUMED = parseFloat($("input[id$=ICD_VALUE_CONSUMED]").val().replace(/[^0-9\.]+/g, "")).toFixed(RateDec);
            }
            obj.ICD_UOM_TEXT = $("select[id$=ICD_UOM] option:selected").text();
            obj.ICD_UOM = parseInt($("select[id$=ICD_UOM]").val());
            obj.ICD_REMARKS = $("input[id$=ICD_REMARKS]").val() == "" ? " " : $("input[id$=ICD_REMARKS]").val();
            obj.ICD_CRDR_NOTE_DTL = $("[id$=hdnICD_CRDR_NOTE_DTL]").val() == undefined ? "" : $("[id$=hdnICD_CRDR_NOTE_DTL]").val(); 

            if ($("input[id$=ICD_EXPIRY_DATE]").val() != "") {
                obj.ICD_EXPIRY_DATE = $("input[id$=ICD_EXPIRY_DATE]").val();
            }

            if (parseInt(editRequisition) == 0) {
                ObjRequisition.ConsumptionDtl.push(obj);
            }

            $("#divRequisitionData").data("RequisitionData", ObjRequisition);
          
            
//            FillMaterialCategoryAutoComplete(obj.ICD_ITEM_CATEGORY);
            GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.ConsumptionDtl);
            RemoveValidations();
            ClearProductDetails();
        }
        else {
            GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionCodeAlreadyAdded, RequisitionSlip.MessageBoxTitle);
        }
        return false;
    }
}

function AfterSave() {
    //<summary>function used to clear Evaluation details from grid</summary>
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, dummyObj);
    $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
    $("#ProductInsert").show();
    // $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
}

function FillDetails(tr) {
    ///<summary>Used fill Details of requisition for editing</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    //    DateDtlsInit();
    
    
    $("[id$=MaterialCategoryPK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")));
    $("[id$=MaterialCategory]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialType, $(tr).parent().attr("id")));
    FillCategoryMaterialsAuto(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")));
    $("[id$=MaterialPK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    $("[id$=ItemCodeMaterial]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialCode, $(tr).parent().attr("id")));
   

    $("input[id$=ICD_VALUE_CONSUMED]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialValue, $(tr).parent().attr("id")).replace(/[^0-9\.]+/g, "")).toFixed(RateDec));
    $("input[id$=ICD_QTY_CONSUMED]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialQtyRequest, $(tr).parent().attr("id")).replace(/[^0-9\.]+/g, "")).toFixed(QtyDec));
   
    $("input[id$=ICD_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialComments, $(tr).parent().attr("id")));
    $("input[id$=ICD_EXPIRY_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialExpiryDate, $(tr).parent().attr("id")));
    $("input[id$=ICD_LOT_NO]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.LotNo, $(tr).parent().attr("id")));
    //DateDtlsInit();
    $("input[id$=ICD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.ICD_PK, $(tr).parent().attr("id")));
    $("input[id$=EditRequisition]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("input[id$=MaterialCode]").focus();
    //    $("input[id$=ICD_ISRETURN]").attr("checked", GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.IsReturnable, $(tr).parent().parent().attr("id")) == "No" ? false : true);
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialUOMID, $(tr).parent().attr("id")));
    var CrDrDtl = 0;
    CrDrDtl = GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.ICD_CRDR_NOTE_DTL, $(tr).parent().attr("id"));
    if (CrDrDtl != undefined && CrDrDtl != "undefined") {
        $("[id$=hdnICD_CRDR_NOTE_DTL]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.ICD_CRDR_NOTE_DTL, $(tr).parent().attr("id")))
    }

}

function DeleteDetails(tr) {
    ///<summary>Used fill Details of requisition for Delete</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    for (var i in ObjRequisition.ConsumptionDtl) {
        if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == materialID) {
            //Will delete the Evaluation details
            ObjRequisition.ConsumptionDtl.splice(i, 1);
            break;
        }
    }
    $("#divRequisitionData").data("RequisitionData", ObjRequisition);
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.ConsumptionDtl);
    //Used to Show the  Evaluation details when the requisition in requisition details is 0
    if (ObjRequisition.ConsumptionDtl.length == 0) {
        //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
        //  $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
        $("[id$=ICH_DATE]").attr("disabled", false);
    }
    else {
        $("[id$=ICH_DATE]").attr("disabled", true);
    }
}

function ClearProductDetails() {
    //<summary>function used to Clear Requisition Product Details</summary>
    //DateDtlsInit();
    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=MaterialPK]").val(0);
    $("input[id$=ICD_ITEM]").val("0");

    $("input[id$=EditRequisition]").val("0");
    $("input[id$=ICD_VALUE_CONSUMED]").val("");
    $("input[id$=ICD_QTY_CONSUMED]").val("");
    // $("input[id$=MRH_PK]").val("0");
    $("select[id$=ICD_UOM]").val("0");
    $("input[id$=ICD_REMARKS]").val("");
    $("input[id$=ICD_EXPIRY_DATE]").val("");
 
    $("input[id$=MaterialCode]").focus();

    $("input[id$=ICD_LOT_NO]").val("");
    FillMaterialCategoryAutoComplete(0);
    FillCategoryMaterials(0);
}
///#endregion

///#region----Grid Handlers And Model Popup Ok Click----
function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    /// <param name="command"  type="Object">
    ///  Specific command for action       
    /// </param>
    RemoveValidations();
    switch (command.toString()) {
        case RequisitionSlip.DeleteCommand:
            // Do Confirmation.. Before Delete Details
            materialID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(RequisitionSlip.DeleteConfirmationMessage, RequisitionSlip.ConfirmationMessage, RequisitionSlip.DeleteCommand, true);
            break;
        case RequisitionSlip.EditCommand:
            FillDetails(tr);
            return false;
            break;
        default:
            alert(RequisitionSlip.DefaultAction);
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
        case RequisitionSlip.SaveCommand:
            window.location = RequisitionSlip.REDIRECTURLAFTERSAVE + "?TYPE=" + TRX_TYPE;
            break;
        //comment req            
        case RequisitionSlip.DeleteCommand:
            DeleteDetails();
            break;
        //Commend When calling            
        case RequisitionSlip.DeleteMessageCommand:
            GrandScriptUtils.ShowModal(RequisitionSlip.DeleteConfirmationMessage, RequisitionSlip.ConfirmationMessage);
            break;
    }
    return false;
}
///#endregion
///#endregion

///#region ------ Validation----------
function AddValidations(mode) {
    //<summary>function used to assign validation</summary>
    //Mode = 1 represents the validation for request(Details) Header Details
    if (mode == "1") {
//        $("select[id$=MaterialType]").rules("add", {
//            selectNone: true,
//            messages: { selectNone: RequisitionSlip.MaterialCategoryValidation }
//        });
//        $("select[id$=ITV_ITEM]").rules("add", {
//            selectNone: true,
//            messages: { selectNone: RequisitionSlip.MaterialCodeValidation }
//        });

//        $("select[id$=ITV_ITEM]").rules("add", {
//            selectNone: true,
//            messages: { selectNone: RequisitionSlip.MaterialCodeValidation }
//        });

        $("[id$=MaterialCategory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.MaterialCategoryValidation }
        });
        $("[id$=ItemCodeMaterial]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.MaterialCodeValidation }
        });


        $("input[id$=ICD_VALUE_CONSUMED]").rules("add", {
            required: true,
            messages: { required: RequisitionSlip.RequisitionRateValidation }
        });
        $("input[id$=ICD_VALUE_CONSUMED]").rules("add", {
            maxlength: 14,
            DecimalDigits: RateDec,
            CustomDecimal: true,
            messages: { CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", RateDec) }
        });


        //        if ($("[id$=hdnIsNeededStockValidation]").val() == "1") {

        $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
            maxlength: 12,
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            messages: { CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });
        $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
            required: true,
            messages: { required: RequisitionSlip.RequisitionQuantityValidation }
        });
        //            messages: { required: RequisitionSlip.RequisitionQuantityValidation, max: RequisitionSlip.RequestQuantityLessCurrentStock }
        $("select[id$=ICD_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialUOMValidation }
        });

        $("input[id$=ICD_REMARKS]").rules("add", {
            maxlength: 250
        });
    }
    //Mode =  2 represents the validation for request Details Store,Departement
    else if (mode == "2") {
        //        $("select[id$=ICH_DEPT]").rules("add", {
        //            selectNone: true,

        //            messages: { selectNone: RequisitionSlip.RequisitionStoreValidation }
        //        });
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });

        //New validation fields
        $("select[id$=ICH_ISS_RCV_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.SelectReceiptType }
        });
        if (TRX_TYPE != 4) {
            $("select[id$=ICH_ISS_RCV_PK]").rules("add", {
                selectNone: true,
                messages: { selectNone: RequisitionSlip.SelectReceiveFrom }
            });
        }

        $("select[id$=ICH_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.SelectReceivingStore }
        });
        //        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: RequisitionSlip.SelectMaterialType }
        //        });
        //        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        //        });
    }
    //Mode =  3 represents the popup validation for request Details Store,Departement
    else if (mode == "3") {
        $("input[id$=ITM_NAME]").rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: RequisitionSlip.MaterialNameValidation }
        });
        $("select[id$=ITC_PK]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });
        $("select[id$=UOM_PK]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialUOMValidation }
        });
    }
    //Mode =  3 represents the popup validation for request Details Store,Departement
    else if (mode == "4") {
        //        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        //        });
    }
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $("input[id$=MaterialCategory]").rules("remove");
    $("input[id$=Material]").rules("remove");
    $("select[id$=ICD_UOM]").rules("remove");
    $("input[id$=ICD_VALUE_CONSUMED]").rules("remove");
    $("input[id$=ICD_QTY_CONSUMED]").rules("remove");
    $("select[id$=ICH_ISS_RCV_TYPE]").rules("remove");
    $("select[id$=ICH_ISS_RCV_PK]").rules("remove");
    $("input[id$=ICD_REMARKS]").rules("remove");
    $("select[id$=ICH_DEPT]").rules("remove");
    //    $("select[id$=DeptPk]").rules("remove");
    //    $("select[id$=ICH_ITEM_TYPE]").rules("remove");
}

//<summary>function Remove Validation</summary>
function RemovePopupValidations() {
    $("input[id$=ITM_NAME]").rules("remove");
    $("select[id$=ITC_PK]").rules("remove");
    $("select[id$=UOM_PK]").rules("remove");
    //  $("input[id$=ITM_DESC]").rules("remove");
}
///#endregion

//function FillCompany(selectVal) {
//    if (selectVal == undefined || selectVal == 0) {
//        var drpID = $("select[id$=ICH_COMPANY]").attr("id");
//        $.get(RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
//            var selCompany = $("[id$=hdfSelCompany]").val();
//            GrandScriptUtils.FillDropDown(drpID, data, true, false, selCompany);
//        });
//    }
//    else {
//        var drpID = $("select[id$=ICH_COMPANY]").attr("id");
//        $.get(RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
//            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
//        });
//    }
//}

function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=ICH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = RequisitionSlip.FillCompanyDropdownURL + RequisitionSlip.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val();
    }
    else {
        getURL = RequisitionSlip.FillCompanyDropdownURL + RequisitionSlip.BizUnitPk + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        if (selectVal == undefined || selectVal == 0) {
            var selCompany = $("[id$=hdfCompany]").val();
            $("#ICH_COMPANY").val(selCompany);
        }
    });
}

//Comma Separation for Quantity & Amount 
function addCommas(number) {
    var FormattedNumber = number;
    var curGroup1 = 3;
    var curGroup2 = 3;
    var NumericPart = "", LastNumericPart = "", DecimalPart = "";
    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
        curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
    }
    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
        curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
    }

    DecimalPart = number.split('.')[1];
    (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
    NumericPart = number.split('.')[0];
    if (NumericPart.length > curGroup1) {
        LastNumericPart = NumericPart.substr(NumericPart.length - curGroup1, curGroup1);
        (LastNumericPart) ? LastNumericPart = "," + LastNumericPart : LastNumericPart = "";
    }
    if ((NumericPart.length - curGroup1) > 0) {
        NumericPart = NumericPart.substr(0, NumericPart.length - curGroup1);
        var pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
        var expression = new RegExp(pattern, "g");
        NumericPart = NumericPart.toString().replace(expression, ",");
    }
    FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
    return FormattedNumber;
}

//For checking selected date is a future date or not
//command=>Draft,SaveandSubmit
function ShowFutureDate(command) {

    var msgTitle;
    var msg;
    msgTitle = RequisitionSlip.MessageBoxTitle;
    msg = RequisitionSlip.ContFutureDateMsg;
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
    GrandScriptUtils.ShowModal(RequisitionSlip.Err_FutureDateTransactionNotAllowed, RequisitionSlip.MessageBoxTitle);
    return false;
}