/// <reference path="../../JSLINQ/JSLINQ-vsdoc.js" />
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
var QtyDec, AmtDec;
var selectVal = -1;
var selectText = "";
var TRX_TYPE = 1; //1-EMI,3-MaterialReturn;
var IsFromDRCR = 0;
var typeText = "";
var typeTextGRN = "";
var issueAgainist_PK = 0;
///#endregion

///#region -----Configuration-----
var RequisitionSlip = {
    //Url
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    AutoCompleteURL: "StoreRequisitionSlip.do?Action=GetSearchValue",
    //  FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=",
    //    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryAutoList&AUTOSEARCH=1&SBUPk=",
    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryStkAutoList&AUTOSEARCH=1&SBUPk=",
    //FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListExceptFG&SBUPk=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetailsForStore&SBUPk=",
    ////commented when new changes for batch no integration. cross check
    //FillBatchNoDropdownURL: "MaterialManagement.do?Action=GetBatchNoList&SBUPk=",
    //Change start for batch integration
    FillBatchNoDropDownURL: "MaterialManagement.do?Action=GetBatchNoConsumption&SBUPk=",
    FillBatchDetailGetURL: "MaterialManagement.do?Action=GetBatchDetails&SBUPk=",
    GetCurrentStock: "MaterialManagement.do?Action=GetCurrentStockForStore&SBUPk=",
    FillUOMDropdownURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial&MaterialPK=",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    //FillStoreDropdownURL: "SubDepartmentManagement.do?Action=GetInventoryStores&SBUPk=",
    FillIssuingType: "ExternalMaterialIssue.do?Action=GetIssuingType&SBUPk=",
    FillIssuingToList: "ExternalMaterialIssue.do?Action=GetIssuingToList&AUTOSEARCH=1&SBUPk=",
    FillIssuingToDamage: "ExternalMaterialIssue.do?Action=FillIssuingToDamage&SBUPk=",
    GetAssetFormer: "ExternalMaterialIssue.do?Action=GetAssetFormer&SBUPk=",
    //    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&AUTOSEARCH=1",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStoreStk&AUTOSEARCH=1",
    FillDepartementDropdownURL: "StoreRequisitionSlip.do?Action=GetDepartmentDtls&SBUPk=",
    GETItemNameURL: "MaterialManagement.do?Action=GetItemName&MaterialID=",
    RequisitionSaveURL: "ExternalMaterialIssue.do?Action=SaveExternalMaterialIssue",
    EMIDamageSaveWkf: "ExternalMaterialIssue.do?Action=SaveEMIDamageWkf",
    REDIRECTURLAFTERSAVE: "../StoreManagement/FormerDamageIssueList.aspx",
    REDIRECTURLAFTERSAVEFROMINBOX: "../AccountManagement/WorkflowInbox.aspx",
    //GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategoryAndStore&SBUPk=",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategoryAndStoreAuto&AUTOSEARCH=1&SBUPk=",
    GETSRSNoURL: "StoreRequisitionSlip.do?Action=GetSRSNo",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    GetMaterialCategoryExceptFGTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryNewWithoutFG&SBUPk=",
    PRINTURL: "../StoreManagement/ExternalMaterialIssueReport.aspx",
    MaterialSaveURL: "MaterialManagement.do?Action=SavePage&DepartPK=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",
    FillGRNAutoCompleteURL: "ExternalMaterialIssue.do?Action=GetGRNAutoComplteList&SBUPk=",
    GetGRNDetailsListURL: "ExternalMaterialIssue.do?Action=GetGRNDetailsList&GRNPK=",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    RequisitionSaveMessage1: "Translate(ExternalMaterialIssue1)",
    FormerDamageIssueSaveMsg: "Translate(FormerDamageIssue)",
    VoucherNo: "Translate(VoucherNo)",
    MRTSaveMessage1: "Translate(MaterialReturn1)",
    RequisitionSaveMessage2: "Translate(ExternalMaterialIssue2)",
    MRTSaveMessage2: "Translate(MaterialReturn2)",
    SubmitMessage: "Translate(SubmittedMsg)",
    RequisitionUpdateMessage2: "Translate(RequisitionDetailsUpdated2)",
    RequisitionCodeExistsMessage: "Translate(AlreadyExists)",
    EMISavedMessage: "Translate(ExternalMaterialIssueSavedMessage)",
    FDISavedMessage: "Translate(FormerDamageIssueSaved)",
    MRTSavedMessage: "Translate(MaterialReturnSavedMessage)",
    ItemAlreadyDeletedMsg: "Translate(Itemsalreadydeletedbyanotheruser)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(ConsumptionDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    MaterialCategoryValidation: "Translate(SelectItemCategory)",
    RequisitionCodeAlreadyAdded: "Translate(AlreadyExists)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    StoreDoesnthaveReqStock: "Translate(StoreDoesnthaveReqStock)",
    NotEnoughStock: "Translate(NotEnoughStock)",
    RequestQuantityLessCurrentStock: "Translate(ConsumptionQuantityLessCurrentStock)",
    RequestQuantityLessCurrentStockMRT: "Translate(ConsumptionQuantityLessCurrentStockMRT)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    StockAdjustmentIsAlreadyDone: "Translate(StockAdjustmentIsAlreadyDone)",
    StockTransferAlreadyDone: "Translate(StockTransferAlreadyDone)",
    StoreCantSame: "Translate(StoreCantSame)",
    StoreCantSameMRT: "Translate(StoreCantSameMRT)",
    Err_FutureDateTransactionNotAllowed: "Translate(Err_FutureDateTransactionNotAllowed)",
    StockBatchFIFOMsg: "Translate(StockBatchFIFOMsg)",
    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)",
    WrongBatchSelectionMsg: "Translate(WrongBatchSelectionMsg)",
    AccMappingNotExist: "Translate(AccMappingNotExist)",
    DepreciationNotExist: "Translate(DepreciationNotExist)",
    PostingNotEnabled: "Translate(PostingNotEnabled)",
    AssetNotExist: "Translate(AssetNotExist)",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    //Constants
    TextZero: "0",
    DefaultStoreValue: "6",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    LOGOUT: "LOGOUT",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    Param: "&MatCagID=",
    IsModifyEMI: false,
    ValueEmpty: ' ',
    CommentMandatoryCategoryName: "TYRE",
    //Validation messages
    MaterialCodeValidation: "Translate(SelectItemCode)",
    MaterialUOMValidation: "Translate(PleaseSelectUOM)",
    MaterialBatchValidation: "Translate(SelectBatch)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    RequisitionQuantityValidation: "Translate(PleaseProvideQtyIssued)",
    RequisitionCommentsValidation: "Translate(PleaseProvideComments)",
    RequisitionStoreValidation: "Translate(SelectConsumptionStore)",
    QuantityIssuedValidation: "Translate(QtyZeroValidation)",
    DuplicateItemValidation: "Translate(DuplicateItemValidation)",
    GRNValidation: "Translate(SelectGRN)",
    NoStockValidation: "Translate(NoStockValidation)",

    SelectIssuingType: "Translate(SelectIssuingType)",
    SelectIssuingTo: "Translate(SelectIssueTo)",
    SelectIssuingStore: "Translate(SelectIssuingStore)",
    SelectMaterialType: "Translate(SelectMaterialType)",

    RequisitionDepartementValidation: "Translate(PleaseselectaDepartement)",
    SelectRequestDetails: "Translate(AddMaterialIssueDetails)",
    SelectReturnDetails: "Translate(AddMaterialReturnDetails)",
    MaterialNameValidation: "Translate(PleaseProvideMaterialName)",
    EnterDate: "Translate(EnterDate)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    CommentRequired: "Translate(CommentRequired)",
    //Fields
    MaterialCode: "ICD_ITEM_TEXT",
    MaterialDate: "ICH_DATE",
    CurrentStock: "ICD_CURRENT_STK",
    MaterialQtyRequest: "ICD_QTY_CONSUMED",
    MaterialUOMID: "ICD_UOM",
    MaterialComments: "ICD_REMARKS",
    MaterialID: "ICD_ITEM",
    MaterialText: "ICD_ITEM_TEXT",
    ICD_CRDR_NOTE_DTL: "ICD_CRDR_NOTE_DTL",
    ICH_PK: "ICH_PK",
    MaterialTypePk: "ICD_ITEM_CATEGORY",
    MaterialTypeText: "ICD_ITEM_CATEGORY_TEXT",
    IsReturnable: "ICD_ISRETURNTEXT",
    BizUnitPk: 0
}
///#endregion



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

    if ($("[id$=AutoStartValue]").val() != 0) {
        typeText = "Type min. " + $("[id$=AutoStartValue]").val() + " characters";
    }
    else {
        typeText = "Translate(AutoDefaultValue)";
    }
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {
        // return ($(element).val() != "Translate(AutoDefaultValue)");
        return ($(element).val() != "Select/Type");
    }, "Translate(Pleaseselectanoption)");


    if ($("[id$=hdfGRNLimitTextLength]").val() != 0) {
        typeTextGRN = "Type min. " + $("[id$=hdfGRNLimitTextLength]").val() + " characters";
    }
    else {
        typeTextGRN = "Translate(AutoDefaultValue)";
    }

    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
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
        //alert("1" + $("[id$=ConsumptionDtl]").val());
        //Create Date Picker
        if ($("[id$='hdfEmptyEMIDate']").val() == "1")
            GrandScriptUtils.DatePickerClear("ICH_DATE", false, false, true);
        else
            GrandScriptUtils.DatePickerClear("ICH_DATE", false, false, false);
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
                else if (TypePK[1] == 1 && TypePK[0] == "IsModify") {
                    RequisitionSlip.IsModifyEMI = true;
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

            var MenuType = getQueryStringValue("TYPE");
            var RefID = getQueryStringValue("RefID");
            var prefID = getQueryStringValue("PRefID");
            if (MenuType == "7" && RefID != "") {
                EditMode = 1;
                FillRequisitionDetails(requisitionJson);
            }
            if (MenuType == "99" && prefID != "") {
                EditMode = 1;
                FillRequisitionDetails(requisitionJson);
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
        $("select[id$=ICH_ISS_RCV_TYPE]").focus();


    }
    $("[id$=ICH_DATE]").click(function () {
        $("select[id$=MaterialCategory]").val('0');
        FillCategoryDetails('0');
        //        if ($("[id$=hdfEnableBatch]").val() == "0") {
        //            $("[id$=BatchNo]").attr("disabled", true);
        //        }
        //        else {
        //            $("[id$=BatchNo]").attr("disabled", false);
        //        }
    });
});

function getQueryStringValue(key) {
    return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}

function FillRequisitionDetails(requisitionJson) {

    ///<summary>Used to fill requisition Details for editing</summary>
    // var drpID = $("select[id$=Product]").attr("id");
    //Set a stamp for cancelled record
    if (requisitionJson.ICH_STATUS == 4)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");
    //Header Details.  
    requisitionJson.ICH_CRDR_NOTE_HDR == undefined ? "" : $("[id$=ICH_CRDR_NOTE_HDR]").val(requisitionJson.ICH_CRDR_NOTE_HDR);
    $("[id$=ICH_CRDR_FLAG]").val(requisitionJson.ICH_STATUS); //1=>Record should be in View Mode.Using for details comes from CreitDebit
    issueAgainist_PK = requisitionJson.ICH_ISS_RCV_TYPE_PK;
    FillStore(requisitionJson.ICH_DEPT);
    FillDepartement(requisitionJson.ICH_DEPT);
    FillIsuingType(requisitionJson.ICH_ISS_RCV_TYPE);
    FillIssuingToList(requisitionJson.ICH_ISS_RCV_PK);
    FillGRNAutoComplete();
    selectText = requisitionJson.ICH_ISS_RCV_NAME;
    selectVal = requisitionJson.ICH_ISS_RCV_PK;
    FillCompany(requisitionJson.ICH_COMPANY);
    $("[id$=ICH_DATE]").val(requisitionJson.ICH_DATE)
    FillTypes(requisitionJson.ICH_ITEM_TYPE);
    $("input[id$=ICH_PK]").val(requisitionJson.ICH_PK)
    //FillMaterialCategoryEdit(requisitionJson.ICH_ITEM_TYPE, requisitionJson.ICH_DEPT);
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
    $("[id$=ICH_STATUS]").val(requisitionJson.ICH_STATUS);
    $("[id$=hdfTranStatus]").val(requisitionJson.ICH_STATUS);
    // Check requisitionJson.ConsumptionDtl is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign ConsumptionDtl Details to that Array, and then push Array to requisitionJson.ConsumptionDtl
    if (!($.isArray(requisitionJson.ConsumptionDtl))) {
        var objArray = requisitionJson.ConsumptionDtl;
        requisitionJson.ConsumptionDtl = new Array();
        requisitionJson.ConsumptionDtl.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, requisitionJson.ConsumptionDtl);
    FillCategoryMaterials(0);
    // $("select[id$=ICH_DEPT]").focus();
}
function afterAutoComplete() {
    $("[id$=ICH_ISS_RCV_PK]").val(selectVal);
    if (selectText != "")
        $("[id$=txtIssueTo]").val(selectText);
}

function PageInit() {
    ///<summary>initial page condition</summary>
    //Reseting all input controls in the page.
    // ResetPage();

    $.validator.addMethod("selectAutotypeText", function (value, element) {
        return ($(element).val() != typeText);
    }, "Translate(Pleaseselectanoption)");

    $.validator.addMethod("selectGRNAutotypeText", function (value, element) {
        return ($(element).val() != typeTextGRN);
    }, "Translate(SelectGRN)");

    $("[id$=ConfirmStockValueChange]").val('0');
    if (EditMode == 0) {
        //Filling Store dropdown initially.      
        FillStore($("[id$=hdfDeptID]").val());
        FillDepartement(0);
        FillTypes(0);
        FillIsuingType(RequisitionSlip.DefaultStoreValue);
        FillIssueToCategories();
        $("[id$=imbPrint]").hide();
        //        FillMaterialCategoryAutoComplete();
        FillCategoryMaterials(0);
        FillCompany(0);
        FillGRNAutoComplete();
    }
    if ($("[id$=hdfEnableBatch]").val() == "0") {
        $("[id$=BatchNo]").attr("disabled", true);
    }
    else {
        $("[id$=BatchNo]").attr("disabled", false);
    }
    if (TRX_TYPE == 3) {
        $("input[id$=ICD_ISRETURN]").attr("checked", true);
        $("input[id$=ICD_ISRETURN]").attr("disabled", true);
        $("[id$=ICH_TRX_TYPE]").val(3); //For Identifying Material Return Entry
    }
    if ($("[id$='hdfLoadFromGRN']").val() == "0") {
        $("[id$=divLoadFromGRN]").hide();
    }
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=ICH_COMPANY]").attr("disabled", "disabled");
    }
    GrandScriptUtils.DatePickerCommon("ICH_DATE", false, false, $("[id$=hdfCurrentDate]").val());

}
function ReInitAutoComplete(controlID) {
    if (controlID == "MaterialCategory")
        FillMaterialCategoryAutoComplete();
}
function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("MaterialCategory", RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&ItemType=" + $("select[id$=ICH_ITEM_TYPE]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
    if (TRX_TYPE == 7) {
        $.get(RequisitionSlip.GetAssetFormer + $("[id$=BizUnitPk]").val(), function (data) {
            if (data.length > 0) {
                $("[id$=MaterialCategoryPK]").val(data[0].Value);
                $("[id$=MaterialCategory]").val(data[0].Text);
            }
        });
        DisableAuto($("[id$=MaterialCategory]"), $("[id$=MaterialCategoryPK]"));
    }
}

//<summary> Function Used to make Batch No field as auto complete </summary>
function FillBatchNoAutoComplete() {
    var drpID = $("[id$=BatchNo]").attr("id");
    var date = $("[id$=ICH_DATE]").val();
    var transDate = $("[id$=ICH_DATE]").val();
    if ($("[id$=hdfEnableBatch]").val() == "0" || $("[id$=hdnItmNeedBatchStk]").val() == "0") {
        $("[id$=BatchNo]").val("");
        $("[id$=BatchNoPK]").val("0");
        $("[id$=BatchNo]").next("a").remove();
        $("[id$=BatchNo]").attr("disabled", true);

    }
    else {
        $("[id$=CurrentStock]").html("");
        $("[id$=BatchNo]").attr("disabled", false);
        GrandScriptUtils.MakeAutoComplete("BatchNo", RequisitionSlip.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("[id$=MaterialPK]").val() + "&DepartmentID=" + $("select[id$=ICH_DEPT]").val() + "&Date=" + date + "&transDate=" + transDate + "&CDHPk=" + $("[id$=ICH_CRDR_NOTE_HDR]").val(), "BatchNoPK", true, false, "BizUnitPk", true, afterAutoComplete);
    }
}
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "MaterialCategory") {
        FillCategoryMaterials($("[id$=MaterialCategoryPK]").val());
    }
    if (targetControlID == "Material") {
        RemoveValidations();
        AddValidations(6);
        if ($(document.forms[0]).valid()) {
            $("[id$=BatchNoPK]").val("0");
            FillMaterialDetails($("[id$=MaterialPK]").val()); //No need to fill the current stock here. It should based on the batch no.
        }
        //FillBatchNoAutoComplete();
        //        if ($("[id$=hdfEnableBatch]").val() == "0") {
        //            $("[id$=BatchNo]").attr("disabled", true);
        //        }
        //        else {
        //            $("[id$=BatchNo]").attr("disabled", false);
        //        }
    }
    if (targetControlID == "BatchNo") {
        var batchID = $("[id$=BatchNoPK]").val();
        $.getJSON(RequisitionSlip.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
            if (data != null) {
                //            var qtyStock = data[0].SBD_QTY_IN_STOCK;
                if (batchID != 0) {
                    //alert(QtyDec);
                    $("[id$=CurrentStock]").html(parseFloat(data[0].SBD_QTY_IN_STOCK).toFixed(QtyDec));
                    FillUOM($("[id$=MaterialPK]").val(), data[0].SBD_UOM);
                    $("select[id$=ICD_UOM]").removeClass();
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
}
//To excecute after auto complete change
function AfterInvalidSelect(targetControlID) {
    if (targetControlID == "Material") {
        $("[id$=MaterialPK]").val(0);
        $("[id$=BatchNo]").val("");
        $("[id$=BatchNoPK]").val("0");
        $("[id$=BatchNo]").next("a").remove();
        $("[id$=BatchNo]").attr("disabled", true);
        $("[id$=CurrentStock]").html("");
        $("select[id$=ICD_UOM]").removeClass();
        FillUOM(materialID, false);

    }
}
function FillCategoryMaterials(categoryID) {
    ///<summary>Function Used to Fill material based on the category  </summary>
    $("[id$=Material]").val("");
    $("[id$=MaterialPK]").val(0);
    if ($("select[id$=MaterialType]").val() != 0) {
        typePK = $("select[id$=ICH_ITEM_TYPE]").val();
        storePK = $("select[id$=ICH_DEPT]").val();
    }
    categoryID = categoryID == undefined ? 0 : categoryID;
    //GrandScriptUtils.MakeAutoCompleteLimitLen("Material", RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Type=" + typePK + "&Store=" + storePK, "MaterialPK", true, false, "MaterialCategoryPK", true, "Store", "", "", $("[id$=AutoStartValue]").val());
    GrandScriptUtils.MakeAutoCompleteLimitLen("Material", RequisitionSlip.MaterialURL, "MaterialPK", true, false, "MaterialCategoryPK", true, "ICH_DEPT", "", "", $("[id$=AutoStartValue]").val(), "", typeText, false);
}
///#endregion

function FillIsuingType(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store

    var drpID = $("select[id$=ICH_ISS_RCV_TYPE]").attr("id");
    $.get(RequisitionSlip.FillIssuingType + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0&CFG_PK=" + issueAgainist_PK, function (data) {
        if (drpID != null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
            FillIssuingToList();
        }

    });
    $("[id$=hdn_ICH_ISS_RCV_TYPE]").val(SelectedValue);
    if (RefIdMode == 0) {
        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", true);
    }
    if (TRX_TYPE == 7) {
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
    //    $.get("SubDepartment.do?Action=GetAllStores&SBUPk=" + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0&DeptCatg=0", function (data) {
    $.get(RequisitionSlip.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=1&DeptType=-1&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
        $("select[id$=ICH_DEPT]").attr("disabled", true);
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
        GrandScriptUtils.FillDropDown(drpID, data, true, false, typeID);
        FillMaterialCategoryAutoComplete();
        // FillCategoryMaterials(0);
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
            //            if ($("select[id$=ICH_ITEM_TYPE]").val() != 0 && $("select[id$=ICH_ITEM_TYPE]").val() != null) {
            //                FillMaterialCategoryEdit($("select[id$=ICH_ITEM_TYPE]").val(), $("select[id$=ICH_DEPT]").val());
            //            }
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
    FillMaterialCategoryAutoComplete();
    FillCategoryMaterials();
}

function FillIssueToCategories() {
    //    var ObjMaterialIssue = new Object();
    //    ObjMaterialIssue.ConsumptionDtl = new Array();
    //    $("#divRequisitionData").data("RequisitionData", ObjMaterialIssue);
    //        AfterSave();
    //        ClearGridControlDetails();
    FillIssuingToList(0);
}

function ClearGridControlDetails() {
    var drpUUOMID = $("select[id$=ICD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    $("[id$=CurrentStock]").html("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");
    $("[id$=ICD_PK]").val("0");
}

function FillIssuingToList(issueTypeID, issueTypeText) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>    
    var issuingType;
    if ($("select[id$=ICH_ISS_RCV_TYPE]").val() == null)
        issuingType = $("[id$=hdn_ICH_ISS_RCV_TYPE]").val();
    else
        issuingType = $("select[id$=ICH_ISS_RCV_TYPE]").val();
    var DeptType = 0;
    if (TRX_TYPE == 3) {
        DeptType = 2; //For Showing Stock Department only
        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", true);
    }
    GrandScriptUtils.MakeAutoCompleteLimitLen("txtIssueTo", RequisitionSlip.FillIssuingToList + $("[id$=BizUnitPk]").val() + "&issuingType=" + issuingType + "&Despatch=1" + "&DPT_TYPE=" + DeptType, "ICH_ISS_RCV_PK", true, false, "txtIssueTo", true, "Select", "", "", 0, afterAutoComplete, "Translate(AutoDefaultValue)", false);

    if (TRX_TYPE == 7) {
        GrandScriptUtils.MakeAutoCompleteLimitLen("txtIssueTo", RequisitionSlip.FillIssuingToDamage + $("[id$=BizUnitPk]").val());
        $.get(RequisitionSlip.FillIssuingToDamage + $("[id$=BizUnitPk]").val(), function (data) {
            $("[id$=ICH_ISS_RCV_PK]").val(data[0].Value);
            $("[id$=txtIssueTo]").val(data[0].Text);
        });
    }
    //alert($("[id$=transactionType]").val());
}

function FillGRNAutoComplete() {
    //<summary>function To Fill GRN Autocomplete </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>  
    GrandScriptUtils.MakeAutoCompleteLimitLen("txtGRNNo", RequisitionSlip.FillGRNAutoCompleteURL + $("[id$=BizUnitPk]").val(), "hdfGRNPK", true, false, "BizUnitPk", true, "", "", "", $("[id$=hdfGRNLimitTextLength]").val(), "", typeTextGRN, false);
}

function FillMaterialCategoryOld(materialID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value 
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&ItemType=" + $("select[id$=ICH_ITEM_TYPE]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val(), function (data) {
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
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&ItemType=" + itemType + "&Store=" + store, function (data) {
        if (materialID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function ClearGridControlDetailsExeptMaterialType() {
    var drpUItemID = $("select[id$=ITV_ITEM]").attr("id");
    var drpUUOMID = $("select[id$=ICD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpUItemID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    $("[id$=BatchNoPK]").val("0");
    $("[id$=BatchNo]").val("Translate(Select)");
    $("[id$=CurrentStock]").html("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");
}

function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
    ClearGridControlDetailsExeptMaterialType();
    FillCategoryMaterials(categoryID);
}

function FillCategoryMaterialsOld(categoryID, materialID, typePK) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="categoryID"  type="object">
    ///     Specific categoryid to fill corresponding Material
    /// </param>
    /// <param name="materialID"  type="object">
    ///     Specific materialID to select the dropdown item after filling drop down
    /// </param>
    if ($("select[id$=MaterialType]").val() != 0) {
        typePK = $("select[id$=ICH_ITEM_TYPE]").val();
        storePK = $("select[id$=ICH_DEPT]").val();
        var drpID = $("select[id$=ITV_ITEM]").attr("id");
        $.getJSON(RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Type=" + typePK + "&Store=" + storePK, function (data) {
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

function FillMaterialDetails(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    GetCurrentStock(materialID);

    //    var drpID = $("select[id$=ICD_UOM]").attr("id");
    //    $.get(RequisitionSlip.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
    //        if (data) {
    //            if (materialID != 0) {
    //                $("[id$=MaterialName]").html(data[0].ITM_NAME);
    //                //$("select[id$=ICD_UOM]").val(data[0].ITM_UOM);
    //                FillUOM(materialID, data[0].ITM_UOM);
    //            }
    //            else {
    //                $("[id$=MaterialName]").html("");
    //                // $("select[id$=ICD_UOM]").val("0");
    //                FillUOM(materialID, false);
    //            }
    //        }
    //    });
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
        $("[id$=CurrentStock]").html("0");
        if (data) {
            if (materialID != 0) {
                if ($("[id$=hdfEnableBatch]").val() == "0" || data[0].ITM_NEED_BATCH_STK == "0") {
                    $("[id$=CurrentStock]").html(parseFloat(data[0].STD_QTY_IN_STOCK).toFixed(QtyDec));
                }
                $("[id$=hdnItmNeedBatchStk]").val(data[0].ITM_NEED_BATCH_STK);
                FillMaterialCategoryAutoComplete();
                $("[id$=MaterialCategoryPK]").val(data[0].STD_ITEM_CATEGORY);
                $("[id$=MaterialCategory]").val(data[0].STD_ITEM_CATEGORY_TEXT);
                FillUOM(materialID, data[0].STD_UOM);
                $("select[id$=ICD_UOM]").removeClass();
            }
            else {
                $("[id$=CurrentStock]").html("");
                // $("select[id$=ICD_UOM]").val("0");
                $("select[id$=ICD_UOM]").removeClass();
                FillUOM(materialID, false);
            }
            FillBatchNoAutoComplete();
        }
    });
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
    SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryExceptFGTreeURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("select[id$=ICH_ITEM_TYPE]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val() + RequisitionSlip.Param, "Root", false, false, "0", false); // set the tree view parameters
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

function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    $("select[id$=MaterialType]").val(cagID);
    //setting selected value for uom after selecting category from treeview.
    FillCategoryDetails(cagID);
    //closing modalbox after selected from treeview.
    $("#divCategory").dialog("destroy");
    $("#divCategory").dialog({ autoOpen: false });
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>
    var colIndex = 0;
    var colVal = 0;
    if (grdID == "grdRequisitionSlip") {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_REMARKS", $("#grdRequisitionSlip").attr("id"));
        $("#grdRequisitionSlip").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("");
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_CURRENT_STK", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_CURRENT_STK", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "null") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(colVal).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_QTY_CONSUMED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_QTY_CONSUMED", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "null") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(colVal).toFixed(QtyDec)));
            }

            //Batch
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_STK_BATCH_NO", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "ICD_STK_BATCH_NO", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "undefined") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(colVal);
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

            //Stock

        });
        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#ProductInsert").find("tr:eq(1)");
        }
        $("#ProductInsert").hide();
        $(tdset).insertBefore($("#grdRequisitionSlip").find("tr:eq(1)"));
        //for hiding action fields of detail section 23-11-11
        var queryStr = window.location.search.substring(1);
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");

        var isViewMode = false;
        var queryStr = window.location.search.substring(1);
        if (queryStr != "") {
            var queryStr = queryStr.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] == 1 && pK[0] == "Status" && RequisitionSlip.IsModifyEMI == false) || (pK[1] == 1 && pK[0] == "Flag") || ($("[id$=ICH_CRDR_FLAG]").val() == 1 && RequisitionSlip.IsModifyEMI == false)) {               // if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                    $("#grdRequisitionSlip th:last").hide();
                    $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                        $(this).find("td:last").hide();
                        $(this).find("td:last").hide();
                        $("select[id$=ICH_ISS_RCV_TYPE]").attr("disabled", true);
                        //                        $("select[id$=ICH_ISS_RCV_PK]").attr("disabled", true);
                        $("[id$=txtIssueTo]").attr("disabled", true);

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
        if (RequisitionSlip.IsModifyEMI == true) { $("[id$=btnSave]").hide(); }

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
        $("[id$=ICH_DATE]").attr("disabled", false);
        if (ObjRequisition != undefined) {
            if (ObjRequisition.ConsumptionDtl.length > 0) {
                $("[id$=ICH_DATE]").attr("disabled", true);
            }
        }
    }
    $("[id$=CurrentStock]").html("");
    var status = getQueryStringValue("status");

    if (status == "2") {
        $("[id$=btnSave]").hide();
    }
    else if (status == "1") {
        $("[id$=btnSave]").show();
    }
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    //window.location = RequisitionSlip.REDIRECTURLAFTERSAVE + "?TYPE=" + TRX_TYPE;
    window.location = RequisitionSlip.REDIRECTURLAFTERSAVE + "?TYPE=7" ;
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
//End


function SavePage(command) {
 
    ///<summary>Function used to saving   </summary>
    $.get(RequisitionSlip.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(RequisitionSlip.SessionExpired, RequisitionSlip.Confirmation, RequisitionSlip.LOGOUT, true);
            result = false;
        }
        else {
            $("[id$=ICH_COMPANY]").attr("disabled", false);
            RemoveValidations();
            AddValidations(5);
            if ($(document.forms[0]).valid()) {
                $.get(RequisitionSlip.InventoryLockCheckingURL + $("[id$=ICH_DATE]").val() + "&Module=2", function (data) {
                    if (data != null && data.length > 0) {
                        if (parseInt(data[0]) == 0) {
                            $("#updateProgress").show();
                            SaveEmiEmrMr(command); //For developer convenience,all codes are just placed under a new function .
                        }
                        else {
                            var Err_TranslockedMsg = RequisitionSlip.ErrTransLockedMsg + RequisitionSlip.ValueEmpty + data[1];
                            GrandScriptUtils.ShowModal(Err_TranslockedMsg, RequisitionSlip.MessageBoxTitle);
                        }
                    }
                });
            }
        }
    });
    return false;
}

function ShowWkfSubmit() {
    ShowContainerDivWkf('#divWkfSubmit', 'Translate(Submit)', '700');
    return false;
}

function SaveEmiEmrMr(command) {
 
    RemoveValidations();
    AddValidations(2);
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    // Check Have The ConsumptionDtl have More than or equal to one Requisition Details
    if ($(document.forms[0]).valid()) {
        if (ObjRequisition.ConsumptionDtl != null) {
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
                
                //Check Batch is selected(Requird for DebitCredit)
                if ($("[id$=hdfEnableBatch]").val() == "1") {
                    for (var i = 0; i < ObjRequisition.ConsumptionDtl.length; i++) {
                        if (ObjRequisition.ConsumptionDtl[i].ICD_STK_BATCH == null || ObjRequisition.ConsumptionDtl[i].ICD_STK_BATCH == undefined || ObjRequisition.ConsumptionDtl[i].ICD_STK_BATCH == 0) {
                            if (ObjRequisition.ConsumptionDtl[i].ITM_NEED_BATCH_STK != "0") {
                                GrandScriptUtils.ShowModal(RequisitionSlip.MaterialBatchValidation.fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                                return false;
                            }
                        }
                    }
                }
                //End

                if ($("[id$=hdfLoadFromGRN]").val() == "1") {
                    //Check whether the quantity should be greater than zero
                    for (var i = 0; i < ObjRequisition.ConsumptionDtl.length; i++) {
                        if (ObjRequisition.ConsumptionDtl[i].ICD_QTY_CONSUMED <= 0 || ObjRequisition.ConsumptionDtl[i].ICD_QTY_CONSUMED == undefined) {
                            GrandScriptUtils.ShowModal(RequisitionSlip.QuantityIssuedValidation.fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                            return false;
                        }
                    }

                    //Check duplicate item exist
                    for (var i = 0; i < ObjRequisition.ConsumptionDtl.length; i++) {
                        var count = 0;
                        for (var j = 0; j < ObjRequisition.ConsumptionDtl.length; j++) {
                            if (ObjRequisition.ConsumptionDtl[j].ICD_ITEM == ObjRequisition.ConsumptionDtl[i].ICD_ITEM) {
                                count++;
                                if (count > 1)
                                    break;
                            }
                        }
                        if (count > 1) {
                            GrandScriptUtils.ShowModal(RequisitionSlip.DuplicateItemValidation.fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                            return false;
                        }
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

                //Returning/Issuing Store and Return/Issue To store can't be same.
                if ($("select[id$=ICH_ISS_RCV_TYPE]").val() == "6") { //if Issue Against==Department (ADM_CONFIG_MST->EXTERNAL ISS RCV TYPE)
                    if ($("select[id$=ICH_DEPT]").val() == $("[id$=ICH_ISS_RCV_PK]").val()) {
                        var msg = "";
                        if (TRX_TYPE == 3) { msg = "Translate(StoreCantSameMRT)"; }
                        else { msg = "Translate(StoreCantSame)"; }
                        GrandScriptUtils.ShowModal(msg.fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                        return false;
                    }
                }

                ObjRequisition = $("#divRequisitionData").data("RequisitionData");
                //Assigning the Requisition details to a hidden field by converting the object to string using Json Stringify Methord
                $("[id$=ConsumptionDtl]").val(JSON.stringify(ObjRequisition.ConsumptionDtl));
                $("[id$=WKF_FLAG]").val("0");
                //making json string 
          
                //checking command value is draft if it is draft then action id is zero means it is not calling workflow.
                if (command != "Draft") {
                    $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
                    $("[id$=ICH_STATUS]").val('1');
                    $("[id$=WKF_FLAG]").val("1");
                }
                else {
                    $("[id$=ActionID]").val('0');
                    $("[id$=ICH_STATUS]").val('0');
                }
                $("[id$=ICH_ISS_RCV_NAME]").val($("[id$=txtIssueTo]").val());
                //  var jSonString = GrandScriptUtils.FormToJsonString("divXml");
                var jSonString = GrandScriptUtils.FormToJsonString(false);

                //For EMI Damage
                 
                if ($("[id$=transactionType]").val() == "7" || $("[id$=transactionType]").val() == "99") {
                    $("[id$=ICH_TRX_TYPE]").val(7); //For Identifying Former damage issue
                    $("[id$=AST_VALUE]").val("4");
                    $("[id$=WKF_TRX_FLAG]").val("0");
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
                        $("[id$=AST_DOC_MODE]").val("0");
                    }

                    //1 - Submitted, 7 - Re-Submit
                    //Set WKF_TRX_FLAG to 2 for approval (To run workflow only)

                    if ($("[id$=hdfTranStatus]").val() == "1" || $("[id$=hdfTranStatus]").val() == "7") {
                        $("[id$=WKF_TRX_FLAG]").val("2");
                    }

                    //New Workflow Parameters
                    $("[id$=WKF_REFERENCE]").val($("[id$=ReferenceID]").val());
                    $("[id$=WKF_TASK_ACTION]").val($("select[id$=WRKFACT_ID]").val());
                    $("[id$=WKF_APPLICATION]").val($("[id$=hdfAppID]").val());
                    $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
                    $("[id$=WKF_TASK]").val($("[id$=TaskPK]").val());
                    $("[id$=WKF_COMMENTS]").val($("[id$=WrkfComments]").val());
                    $("[id$=USER_PK]").val($("[id$=UserPk]").val());
                    //End NewWorkflow Parameters
                    var jSonString_damage = GrandScriptUtils.FormToJsonString(false);
                    $.post(RequisitionSlip.EMIDamageSaveWkf, jSonString_damage, function (data) {
                        if (parseInt(data[0]) > 0) {
                            $("#updateProgress").hide();
                            if (command == "Draft") {
                                var SaveMessageWithSRSNo = RequisitionSlip.FDISavedMessage;
                                if ($("[id$=AST_DOC_MODE]").val() == "1")
                                    SaveMessageWithSRSNo = RequisitionSlip.FormerDamageIssueSaveMsg + " " + data[1] + " " + RequisitionSlip.RequisitionSaveMessage2
                                GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                            }
                            else {
                                $("[id$=hdfAppID]").val(parseInt(data[0]));
                                $("[id$=AppNo]").val(data[1]);
                                ShowWorkflowSaveMsg(data[3]);
                            }
                        }
                        else if (parseInt(data[0]) == -2) {
                            GrandScriptUtils.ShowModal(data[1] + " " + RequisitionSlip.EditUsedByAnotherUser, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -3) {
                            if (TRX_TYPE == 3) { //Material Return
                                GrandScriptUtils.ShowModal(RequisitionSlip.MRTSaveMessage1 + " " + RequisitionSlip.StoreDoesnthaveReqStock, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            }
                            else {
                                GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionSaveMessage1 + " " + RequisitionSlip.StoreDoesnthaveReqStock, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            }
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -10) {
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
                        else if (parseInt(data[0]) == -41) {//Batch should be in FIFO order of Batch
                            var s = String.format(RequisitionSlip.StockBatchFIFOMsg.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -42) {//WRONG BATCH was selected.ie, Item is not present in  selected batch 
                            var s = String.format(RequisitionSlip.WrongBatchSelectionMsg.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -110) {//Account mapping not exist
                            var s = String.format(RequisitionSlip.AccMappingNotExist.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -9) {//Depreciation Not Exist
                            var s = String.format(RequisitionSlip.DepreciationNotExist.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -111) {//Voucher posting not enabled
                            var s = String.format(RequisitionSlip.PostingNotEnabled.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -33) {//Voucher posting not enabled
                            var s = String.format(RequisitionSlip.AssetNotExist.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else {
                            GrandScriptUtils.ShowModal(RequisitionSlip.ActionFailedMessage);
                            ResetPage();
                        }
                    });
                }
                else {
                    $.post(RequisitionSlip.RequisitionSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
                        if (parseInt(data[0]) == 0) {
                            GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionCodeAlreadyAdded, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                        }
                        else if (parseInt(data[0]) > 0) {
                            $("#updateProgress").hide();
                            // If Action is Draft Save
                            if (command == "Draft") {
                                if (TRX_TYPE == 3) { //Material Return
                                    var SaveMessageWithSRSNo = RequisitionSlip.MRTSavedMessage;
                                    if ($("[id$=AST_DOC_MODE]").val() == "1")
                                        SaveMessageWithSRSNo = RequisitionSlip.MRTSaveMessage1 + " " + data[1] + " " + RequisitionSlip.MRTSaveMessage2;
                                    GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                                }
                                else {
                                    var SaveMessageWithSRSNo = RequisitionSlip.EMISavedMessage;
                                    if ($("[id$=AST_DOC_MODE]").val() == "1")
                                        SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + data[1] + " " + RequisitionSlip.RequisitionSaveMessage2
                                    GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                                }
                                PageInit();
                                AfterSave();
                            }
                            // If action - WorkFlow Save
                            else {
                                $("[id$=hdfAppID]").val(data[0]);
                                $("[id$=AppNo]").val(data[1]);
                                var SaveMessageWithSRSNo = "";
                                if (TRX_TYPE == 3) { //Material Return
                                    SaveMessageWithSRSNo = RequisitionSlip.MRTSaveMessage1 + " " + data[1] + " " + RequisitionSlip.SubmitMessage
                                }
                                else {
                                    SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + data[1] + " " + RequisitionSlip.SubmitMessage
                                }
                                GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                                PageInit();
                                AfterSave();
                            }
                        }
                        else if (parseInt(data[0]) == -2) {
                            GrandScriptUtils.ShowModal(data[1] + " " + RequisitionSlip.EditUsedByAnotherUser, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -3) {
                            if (TRX_TYPE == 3) { //Material Return
                                GrandScriptUtils.ShowModal(RequisitionSlip.MRTSaveMessage1 + " " + RequisitionSlip.StoreDoesnthaveReqStock, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            }
                            else {
                                GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionSaveMessage1 + " " + RequisitionSlip.StoreDoesnthaveReqStock, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            }
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -10) {
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
                        else if (parseInt(data[0]) == -41) {//Batch should be in FIFO order of Batch
                            var s = String.format(RequisitionSlip.StockBatchFIFOMsg.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -42) {//WRONG BATCH was selected.ie, Item is not present in  selected batch 
                            var s = String.format(RequisitionSlip.WrongBatchSelectionMsg.fontcolor("red"));
                            GrandScriptUtils.ShowModal(s, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else {
                            GrandScriptUtils.ShowModal(RequisitionSlip.ActionFailedMessage);
                            ResetPage();
                        }
                    });
                }
            }
        }
        else {
            GrandScriptUtils.ShowModal(RequisitionSlip.SelectReturnDetails, RequisitionSlip.MessageBoxTitle);
            RemoveValidations();
        }
    }
    return false;
}

function ShowWorkflowSaveMsg(voucherNo) {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var SaveMessageWithSRSNo = "";
    if (TRX_TYPE == 3) {
        SaveMessageWithSRSNo = RequisitionSlip.MRTSaveMessage1 + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.SubmitMessage
    }
    else if (TRX_TYPE == 7) {
        if (voucherNo != "")
            SaveMessageWithSRSNo = RequisitionSlip.FormerDamageIssueSaveMsg + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.SubmitMessage + RequisitionSlip.VoucherNo + " " + voucherNo
        else
            SaveMessageWithSRSNo = RequisitionSlip.FormerDamageIssueSaveMsg + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.SubmitMessage
    }
    else {
        SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.SubmitMessage
    }
    GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
}

function AddRequisitionDetails() {
    //<summary>function used to add Evaluation details to Evaluation</summary>
    //Add Validation for Evaluation Details by setting mode as 2
    RemoveValidations();
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        $("[id$=ICH_DATE]").attr("disabled", true); // For disable Calendar
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
        var editRequisition = $("input[id$=EditRequisition]").val();
        var obj = new Object();
        var flag = true;
        var isItemExists = 0;
        //Loop used to check the Evaluation already added in the order List
        if (parseInt(editRequisition) == 0) {
            for (var i in ObjRequisition.ConsumptionDtl) {
                //                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == $("[id$=MaterialPK]").val()) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == $("[id$=MaterialPK]").val() && ObjRequisition.ConsumptionDtl[i].ICD_STK_BATCH == $("[id$=BatchNoPK]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjRequisition.ConsumptionDtl) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == $("[id$=MaterialPK]").val() && ObjRequisition.ConsumptionDtl[i].ICD_STK_BATCH == $("[id$=BatchNoPK]").val() && ObjRequisition.ConsumptionDtl[i].ICD_SL_NO != $("[id$=hdfSlNo]").val()) {
                    flag = false;
                    break;
                }
            }
            for (var k in ObjRequisition.ConsumptionDtl) {
                if (parseInt(editRequisition) == ObjRequisition.ConsumptionDtl[k].ICD_ITEM && ObjRequisition.ConsumptionDtl[k].ICD_SL_NO == $("[id$=hdfSlNo]").val())
                    obj = ObjRequisition.ConsumptionDtl[k];
            }

            if (obj.ICD_ITEM == undefined) {
                editRequisition = 0;
            }
        }

        if (flag) {
            if (ObjRequisition.ConsumptionDtl == undefined) {
                var ObjMaterialIssue = new Object();
                ObjMaterialIssue.ConsumptionDtl = new Array();
                ObjRequisition = ObjMaterialIssue;
            }
            obj.ICD_SL_NO = $("[id$=hdfSlNo]").val() == 0 ? ObjRequisition.ConsumptionDtl.length + 1 : $("[id$=hdfSlNo]").val();
            obj.ICD_ITEM = $("[id$=MaterialPK]").val();
            obj.ICD_ITEM_TEXT = $("[id$=Material]").val();
            obj.ICD_PK = $("[id$=ICD_PK]").val();
            if ($("[id$=BatchNoPK]").val() != 0 && $("[id$=BatchNoPK]").val() != 'undefined') {
                obj.ICD_STK_BATCH = $("[id$=BatchNoPK]").val();
                obj.ICD_STK_BATCH_NO = $("[id$=BatchNo]").val();
            }
            else {
                obj.ICD_STK_BATCH = "0";
                obj.ICD_STK_BATCH_NO = "";
            }

            obj.ICD_ITEM_CATEGORY_TEXT = $("[id$=MaterialCategory]").val();
            obj.ICD_ITEM_CATEGORY = $("[id$=MaterialCategoryPK]").val()

            obj.ICD_CURRENT_STK = $("[id$=CurrentStock]").html().replace(/[^0-9\.]+/g, "");
            obj.ICD_QTY_CONSUMED = parseFloat($("input[id$=ICD_QTY_CONSUMED]").val()).toFixed(QtyDec);
            obj.ICD_UOM_TEXT = $("select[id$=ICD_UOM] option:selected").text();
            obj.ICD_UOM = parseInt($("select[id$=ICD_UOM]").val());
            obj.ICD_ITEM_TYPE = $("input[id$=ICD_ISRETURN]").is(':checked') == true ? 1 : 0;
            obj.ICD_ISRETURNTEXT = obj.ICD_ITEM_TYPE == 1 ? "Yes" : "No";
            obj.ICD_REMARKS = $("input[id$=ICD_REMARKS]").val() == "" ? " " : $("input[id$=ICD_REMARKS]").val();
            obj.ICD_CRDR_NOTE_DTL = $("[id$=hdnICD_CRDR_NOTE_DTL]").val() == undefined ? "" : $("[id$=hdnICD_CRDR_NOTE_DTL]").val();
            obj.ITM_NEED_BATCH_STK = $("[id$=hdnItmNeedBatchStk]").val();
            if (parseInt(editRequisition) == 0) {
                ObjRequisition.ConsumptionDtl.push(obj);
            }
            $("#divRequisitionData").data("RequisitionData", ObjRequisition);

            ClearGridControlDetails();
            // FillMaterialCategory(0);
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

    $("[id$=MaterialCategoryPK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")));
    $("[id$=MaterialCategory]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypeText, $(tr).parent().attr("id")));
    FillCategoryMaterials($("[id$=MaterialCategoryPK]").val());
    $("[id$=MaterialPK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")))
    $("[id$=Material]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialText, $(tr).parent().attr("id")))

    $("input[id$=MaterialCode]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialCode, $(tr).parent().attr("id")));
    //$("input[id$=ICH_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialDate, $(tr).parent().attr("id")));    
    $("input[id$=ICD_QTY_CONSUMED]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialQtyRequest, $(tr).parent().attr("id")).replace(/[^0-9\.]+/g, "")).toFixed(QtyDec));

    $("input[id$=ICD_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialComments, $(tr).parent().attr("id")));
    $("[id$=ICD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, "ICD_PK", $(tr).parent().attr("id")));
    $("input[id$=EditRequisition]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("input[id$=MaterialCode]").focus();
    $("input[id$=ICD_ISRETURN]").attr("checked", GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.IsReturnable, $(tr).parent().parent().attr("id")) == "No" ? false : true);
    //$("[id$=hdnSlNo]").val(GrandGrid.Utilities.GetColumnValue(tr, "ICD_SL_NO", $(tr).parent().attr("id")));
    $("[id$=hdnItmNeedBatchStk]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITM_NEED_BATCH_STK", $(tr).parent().attr("id")));
    $("[id$=hdfSlNo]").val(GrandGrid.Utilities.GetColumnValue(tr, "ICD_SL_NO", $(tr).parent().attr("id")));
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialUOMID, $(tr).parent().attr("id")));
    FillBatchNoAutoComplete();
    $("[id$=CurrentStock]").html(addCommas(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.CurrentStock, $(tr).parent().attr("id")).replace(/[^0-9\.]+/g, "")).toFixed(QtyDec)));
    $.get(RequisitionSlip.FillIssuingType + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0", function (data) {
        $("[id$=BatchNoPK]").val(GrandGrid.Utilities.GetColumnValue(tr, "ICD_STK_BATCH", $(tr).parent().attr("id")));
        $("[id$=BatchNo]").val(GrandGrid.Utilities.GetColumnValue(tr, "ICD_STK_BATCH_NO", $(tr).parent().attr("id")));
    });
    var CrDrDtl = 0;
    CrDrDtl = GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.ICD_CRDR_NOTE_DTL, $(tr).parent().attr("id"));
    if (CrDrDtl != undefined && CrDrDtl != "undefined") {
        $("[id$=hdnICD_CRDR_NOTE_DTL]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.ICD_CRDR_NOTE_DTL, $(tr).parent().attr("id")))
    }
    //FillCategoryMaterials(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), $("select[id$=ICH_DEPT]").val());
}

function DeleteDetails(tr) {
    ///<summary>Used fill Details of requisition for Delete</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    for (var i in ObjRequisition.ConsumptionDtl) {
        if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == materialID && ObjRequisition.ConsumptionDtl[i].ICD_SL_NO == $("[id$=hdfSlNo]").val()) {
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
    FillMaterialCategoryAutoComplete();
    FillCategoryMaterials(0);
}

function ClearProductDetails() {
    //<summary>function used to Clear Requisition Product Details</summary>
    //    $("select[id$=MaterialType]").val("0");
    //    $("select[id$=ITV_ITEM]").val("");

    $("[id$=MaterialPK]").val("0")
    $("[id$=BatchNoPK]").val("0")
    $("[id$=Material]").val("Translate(Select)")
    $("[id$=MaterialCategoryPK]").val("0");
    $("[id$=MaterialCategory]").val("Translate(Select)");
    $("input[id$=ICD_ITEM]").val("0");

    $("input[id$=EditRequisition]").val("0");
    $("input[id$=ICD_QTY_CONSUMED]").val("");
    // $("input[id$=MRH_PK]").val("0");
    $("select[id$=ICD_UOM]").val("0");
    $("input[id$=ICD_REMARKS]").val("");
    //  $("select[id$=ITV_ITEM]").val("0")
    $("input[id$=MaterialCode]").focus();
    FillMaterialCategoryAutoComplete();
    FillCategoryMaterials(0);
    $("[id$=BatchNoPK]").val("0");
    if ($("[id$=hdfEnableBatch]").val() == "1") {
        $("[id$=BatchNo]").val("Translate(Select)");
    }
    $("[id$=hdfSlNo]").val("0");
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
            $("[id$=hdfSlNo]").val(GrandGrid.Utilities.GetColumnValue(tr, "ICD_SL_NO", $(tr).parent().attr("id")));
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
            //window.location = RequisitionSlip.REDIRECTURLAFTERSAVE + "?TYPE=" + TRX_TYPE;
            window.location = RequisitionSlip.REDIRECTURLAFTERSAVE + "?TYPE=7";
            break;
        //comment req                 
        case RequisitionSlip.DeleteCommand:
            DeleteDetails();
            break;
        //Commend When calling                 
        case RequisitionSlip.DeleteMessageCommand:
            GrandScriptUtils.ShowModal(RequisitionSlip.DeleteConfirmationMessage, RequisitionSlip.ConfirmationMessage);
            break;
        case RequisitionSlip.LOGOUT:
            $("[id$=imbLogout]").click();
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
        $("[id$=MaterialCategory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.MaterialCategoryValidation }
        });
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });
        $("[id$=Material]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.MaterialCodeValidation }
        });
        $("[id$=Material]").rules("add", {
            selectAutotypeText: true,
            messages: { selectAutotypeText: RequisitionSlip.MaterialCodeValidation }
        });
        if ($("[id$=hdfEnableBatch]").val() == "1") {
            $("[id$=BatchNo]").rules("add", {
                selectAuto: true,
                messages: { selectAuto: RequisitionSlip.MaterialBatchValidation }
            });
        }

        if ($("[id$=hdfAllowNegativeStock]").val() == "1") {
            $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
                maxlength: 12,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                messages: { CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
            });
            $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
                required: true,
                //max: $("[id$=CurrentStock]").html(),
                messages: { required: RequisitionSlip.RequisitionQuantityValidation }
            });
        }
        else {
            $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
                maxlength: 12,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                messages: { CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
            });
            if (TRX_TYPE == 3) {
                $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
                    required: true,
                    max: $("[id$=CurrentStock]").html().replace(/[^0-9\.]+/g, ""),
                    messages: { required: RequisitionSlip.RequisitionQuantityValidation, max: RequisitionSlip.RequestQuantityLessCurrentStockMRT }
                });
            }
            else {
                $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
                    required: true,
                    max: $("[id$=CurrentStock]").html().replace(/[^0-9\.]+/g, ""),
                    messages: { required: RequisitionSlip.RequisitionQuantityValidation, max: RequisitionSlip.RequestQuantityLessCurrentStock }
                });
            }
        }
        $("select[id$=ICD_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialUOMValidation }
        });

        $("input[id$=ICD_REMARKS]").rules("add", {
            maxlength: 250
        });
        if ($("[id$=hdfAddCommentMandValidation]").val() == "1") { // While issuing "Tyres", comment should be mandatory to fill the serial number(s)
            if ($("[id$=MaterialCategory]").val() == RequisitionSlip.CommentMandatoryCategoryName) {
                $("input[id$=ICD_REMARKS]").rules("add", {
                    required: true,
                    messages: { required: RequisitionSlip.CommentRequired }
                });
            }
        }
    }
    //Mode =  2 represents the validation for request Details Store,Departement
    else if (mode == "2") {
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });

        $("[id$=txtIssueTo]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.SelectIssuingTo }
        });

        $("[id$=txtIssueTo]").rules("add", {
            required: true,
            messages: { required: RequisitionSlip.SelectIssuingTo }
        });
        $("[id$=txtIssueTo]").rules("add", {
            selectAutotypeText: true,
            messages: { selectAutotypeText: RequisitionSlip.SelectIssuingTo }
        });
        //New validation fields
        $("select[id$=ICH_ISS_RCV_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.SelectIssuingType }
        });

        $("[id$=ICH_ISS_RCV_PK]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.SelectIssuingTo }
        });

        $("select[id$=ICH_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.SelectIssuingStore }
        });
        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.SelectMaterialType }
        });
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
        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });
    }
    else if (mode == "5") {
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });
    }
    else if (mode == "6") {
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });
    }
    //Mode =  7 represents the Load GRN validation
    else if (mode == "7") {
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });

        $("[id$=txtGRNNo]").rules("add", {
            selectGRNAutotypeText: true,
            messages: { selectGRNAutotypeText: RequisitionSlip.GRNValidation }
        });
    }

}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    //    $("select[id$=MaterialType]").rules("remove");
    //    $("select[id$=ITV_ITEM]").rules("remove");
    $("input[id$=MaterialCategory]").rules("remove");
    $("input[id$=Material]").rules("remove");
    $("input[id$=BatchNo]").rules("remove");
    $("input[id$=ICH_ISS_RCV_PK]").rules("remove");

    $("select[id$=ICD_UOM]").rules("remove");
    $("input[id$=ICD_QTY_CONSUMED]").rules("remove");
    $("select[id$=ICH_ISS_RCV_TYPE]").rules("remove");
    $("input[id$=ICD_REMARKS]").rules("remove");
    $("select[id$=ICH_DEPT]").rules("remove");
    //    $("select[id$=DeptPk]").rules("remove");
    $("select[id$=ICH_ITEM_TYPE]").rules("remove");
    if ($("[id$='hdfLoadFromGRN']").val() == "1") {
        $("input[id$=txtGRNNo]").rules("remove");
    }
}
//<summary>function Remove Validation</summary>
function RemovePopupValidations() {
    $("input[id$=ITM_NAME]").rules("remove");
    $("select[id$=ITC_PK]").rules("remove");
    $("select[id$=UOM_PK]").rules("remove");
    //  $("input[id$=ITM_DESC]").rules("remove");
}
///#endregion


function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=ICH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val();
    }
    else {
        getURL = RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1";
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

function LoadGRNDetails(command) {
    ///<summary>Function used to Load GRN Items   </summary>
    RemoveValidations();
    AddValidations(7);

    if ($(document.forms[0]).valid()) {
        $.getJSON(RequisitionSlip.GetGRNDetailsListURL + $("[id$=hdfGRNPK]").val() + "&IssuingStore=" + $("select[id$=ICH_DEPT]").val(), function (data) {
            if (data != null) {
                var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
                if (ObjRequisition.ConsumptionDtl == undefined) {
                    ObjRequisition.ConsumptionDtl = new Array();
                }
                var maxSlNo = JSLINQ(ObjRequisition.ConsumptionDtl)
                    .Max(function (grnitem) { return grnitem.ICD_SL_NO; });

                if (data.ConsumptionDtl.length != undefined && data.ConsumptionDtl.length > 0)//Array Of Object
                {
                    for (var i = 0; i < data.ConsumptionDtl.length; i++) {
                        data.ConsumptionDtl[i].ICD_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
                        ObjRequisition.ConsumptionDtl.push(data.ConsumptionDtl[i]);
                        maxSlNo++;
                    }
                }
                else {
                    var objArray = data.ConsumptionDtl;
                    objArray.ICD_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
                    ObjRequisition.ConsumptionDtl.push(objArray);
                }

                $("#divRequisitionData").data("RequisitionData", ObjRequisition);
                GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.ConsumptionDtl);
            }
            else {
                GrandScriptUtils.ShowModal(RequisitionSlip.NoStockValidation.fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                return false;
            }
        });
    }

    return false;
}

