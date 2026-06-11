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
var Type = 0;
///#endregion




///#region -----Configuration-----
var RequisitionSlip = {
    //Url
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    AutoCompleteURL: "StoreRequisitionSlip.do?Action=GetSearchValue",
    //  FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=",
    //FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListExceptFG&SBUPk=",
    //    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryAutoList&SBUPk=",
    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryStkAutoList&SBUPk=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetailsForStore&SBUPk=",
    GetCurrentStock: "MaterialManagement.do?Action=GetCurrentStockForStore&SBUPk=",
    FillUOMDropdownURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial&MaterialPK=",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillDepartementDropdownURL: "StoreRequisitionSlip.do?Action=GetDepartmentDtls&SBUPk=",
    FillOtherSBUDropDownURL: "StoreRequisitionSlip.do?Action=GetOtherSBUS&Active=1",
    GETItemNameURL: "MaterialManagement.do?Action=GetItemName&MaterialID=",
    RequisitionSaveURL: "StoreRequisitionSlip.do?Action=SaveRequisition",
    REDIRECTURLAFTERSAVE: "../StoreManagement/StoreRequisitionSlipList.aspx",
    REDIRECTURLAFTERSAVEFROMINBOX: "../AccountManagement/WorkflowInbox.aspx",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategoryAndStore&SBUPk=",
    GETSRSNoURL: "StoreRequisitionSlip.do?Action=GetSRSNo",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    GetMaterialCategoryExceptFGTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryNewWithoutFG&SBUPk=",
    PRINTURL: "../StoreManagement/StoreRequisitionReport.aspx",
    MaterialSaveURL: "MaterialManagement.do?Action=SavePage&DepartPK=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    ListUrl: "StoreRequisitionSlipList.aspx",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    INBOX: "INBOX",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    RequisitionSaveMessage1: "Translate(StoreRequest)",
    RequisitionSaveMessage2: "Translate(SavedSuccessfully)",
    RequisitionUpdateMessage2: "Translate(RequisitionDetailsUpdated2)",
    SRSSavedMessage: "Translate(StoreRequisitionSlipSavedMsg)",
    RequisitionCodeExistsMessage: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(RequisitionDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    MaterialCategoryValidation: "Translate(PleaseSelectCategory)",
    RequisitionCodeAlreadyAdded: "Translate(AlreadyExists)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    RequestQuantityLessCurrentStock: "Translate(RequestQuantityLessCurrentStock)",
    //    EnterTHREEDecimal: "Translate(EnterThreeDigitDecimal)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    LOGOUT: "LOGOUT",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    Param: "&MatCagID=",
    IsModifyMR: false,
    //Validation messages
    MaterialCodeValidation: "Translate(PleaseSelectMaterialCode)",
    MaterialUOMValidation: "Translate(PleaseSelectUOM)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    RequisitionQuantityValidation: "Translate(PleaseProvideQtyRequest)",
    RequisitionCommentsValidation: "Translate(PleaseProvideComments)",
    RequisitionStoreValidation: "Translate(PleaseselectaStore)",
    RequisitionDepartementValidation: "Translate(PleaseselectaDepartement)",
    SelectRequestDetails: "Translate(PleaseSelectRequestDetails)",
    MaterialNameValidation: "Translate(PleaseProvideMaterialName)",
    EnterDate: "Translate(EnterDate)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    MRQtyLessThanMIQty: "Translate(MRQtyLessThanMIQty)",

    //Fields
    MaterialCode: "MaterialCode",
    MaterialDate: "MRH_SUBMITTED_DATE",
    CurrentStock: "CurrentStock",
    MaterialQtyRequest: "MRD_QTY_REQUESTED",
    MaterialUOMID: "MRD_UOM",
    MaterialComments: "MRD_REMARKS",
    MaterialID: "MRD_ITEM",
    MRD_PK: "MRD_PK",
    MRH_TO_BIZUNIT: "MRH_TO_BIZUNIT",
    MaterialTypePk: "MaterialTypePk",
    MaterialTypeText: "MaterialType",
    MRD_IS_RETURNABLE: "MRD_IS_RETURNABLE",
    MRH_IS_RETURNABLE:"MRH_IS_RETURNABLE",
    MRH_IS_BZU_TRN: "MRH_IS_BZU_TRN" //bit for identify sbu store request , 1 for sbu transation 0 for other
}
///#endregion

var QtyDec, AmtDec;

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
        return ($(element).val() != "Translate(Select)");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "Select/Type");
    }, "Translate(Pleaseselectanoption)");

    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    //Initailizing Requisition ProductGrid
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, dummyObj);
    //initialize Requisition Object
    requisitionJson = $.parseJSON($("[id$=RequisitionDetailsList]").val());
    $("#divRequisitionData").data("RequisitionData", requisitionJson);

    //Create Date Picker
    GrandScriptUtils.DatePicker("MRH_SUBMITTED_DATE", false, false);
    $("[id$=MRH_DEPT_STR]").focus();

    //Get RequisitionID Id From the Url and Fill Details - For Edit 
    var queryStr = window.location.search.substring(1);
    //from inbox
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("="); //var pK = queryStr.split("=");
            if ((pK[1] != "" && pK[0] == "RequisitionID")) {
                EditMode = 1;
                // FillRequisitionDetails(requisitionJson);
            }
            else if ((pK[1] != "" && pK[0] == "RefID")) {
                EditMode = 1;
                if ((requisitionJson.MRH_STATUS == "1") || (requisitionJson.MRH_STATUS == "7")) {//1- submitted,7-submit more info
                    RefIdMode = 1; //checking refid has or not and setting flag to 1.if flag=1,it means it has refid and have to change dropdow attribute.
                }
                // FillRequisitionDetails(requisitionJson);
            }
            if (pK[1] == 1 && pK[0] == "IsModify") {
                requisitionJson.IsModifyMR = true;
                $("[id$=MRH_IS_EDIT]").val("1");
            }
        }
        FillRequisitionDetails(requisitionJson);
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

});

function CancelPage() {
    //<summary>Function used to redirect to listing page  </summary>
    RemoveValidations();

    if (Type == 2) { //for SBU Store request
        window.location = RequisitionSlip.ListUrl + "?Type=" + $("[id$=hdfType]").val(); //for SBU Store request
    }
    else {
        window.location = RequisitionSlip.ListUrl;
    }
    return false;
}
function FillRequisitionDetails(requisitionJson) {
    ///<summary>Used to fill requisition Details for editing</summary>
    // var drpID = $("select[id$=Product]").attr("id");
    //Header Details.

    if (requisitionJson.MRH_IS_BZU_TRN == 1) {
        $("[id$=MRH_TO_BIZUNIT_VAL]").val(requisitionJson.MRH_TO_BIZUNIT);
        FillOtherSBUS(requisitionJson.MRH_TO_BIZUNIT);
        $("[id$=MRH_IS_RETURNABLE]").val(requisitionJson.MRH_IS_RETURNABLE);
        $("[id$=MRH_IS_BZU_TRN]").val(requisitionJson.MRH_IS_BZU_TRN);
        $("[id$=MRD_IS_RETURNABLE]").val(requisitionJson.MRD_IS_RETURNABLE);
       
    }

    FillStore(requisitionJson.MRH_DEPT_STR);
    FillDepartement(requisitionJson.DeptPk);
    FillCompany(requisitionJson.MRH_COMPANY);
    //$("[id$=MRH_SUBMITTED_DATE]").val(requisitionJson.MRH_SUBMITTED_DATE == undefined ? requisitionJson.MRH_CRTD_DT : requisitionJson.MRH_SUBMITTED_DATE)
    $("[id$=MRH_SUBMITTED_DATE]").val(requisitionJson.MRH_DATE)

    FillTypes(requisitionJson.ITM_TYPE);
    $("input[id$=MRH_PK]").val(requisitionJson.MRH_PK)
    //  FillMaterialCategoryEdit(requisitionJson.ITM_TYPE, requisitionJson.MRH_DEPT_STR);

    if (requisitionJson.MRH_NO == null || requisitionJson.MRH_NO == "") {
        $("input[id$=MRH_NO]").val("");
        $("[id$=lblSRS]").html(RequisitionSlip.DocGenerationNewValue);
    }
    else {
        $("input[id$=MRH_NO]").val(requisitionJson.MRH_NO);
        $("[id$=lblSRS]").html(requisitionJson.MRH_NO);
    }

    $("[id$=LAST_MOD_DT]").val(requisitionJson.LAST_MOD_DT);
    // Check requisitionJson.RequisitionDetailsList is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign RequisitionDetailsList Details to that Array, and then push Array to requisitionJson.RequisitionDetailsList
    if (!($.isArray(requisitionJson.RequisitionDetailsList))) {
        var objArray = requisitionJson.RequisitionDetailsList;
        requisitionJson.RequisitionDetailsList = new Array();
        requisitionJson.RequisitionDetailsList.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, requisitionJson.RequisitionDetailsList);
    // $("select[id$=MRH_DEPT_STR]").focus();

}

function PageInit() {
    ///<summary>initial page condition</summary>
    //Reseting all input controls in the page.
    // ResetPage();
    Type = parseNumber($("[id$=hdfType]").val()); //for sbu store request
    if (Type == 2) {
        $("[id$=DivSbu]").show();
        GrandScriptUtils.DatePicker("MRH_SUBMITTED_DATE", false, false);
    }
    else {
        $("[id$=DivSbu]").hide();
    }

    if (EditMode == 0) {

        //Filling Store dropdown initially. 
        if (Type == 2) {
            FillOtherSBUS(0);
        }
        if (Type != 2) {
            FillStore(0);
        }
        FillDepartement($("[id$=hdfDeptID]").val());
        FillTypes(0);
        FillMaterialCategoryAutoComplete();
        FillCategoryMaterials(0);
        FillCompany(0);

        $("[id$=btnPrint]").hide();
    }
    else {
        $("[id$=btnPrint]").show();
    }
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=MRH_COMPANY]").attr("disabled", "disabled");
    }
}

function ClearSearchDetails() {


    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=FromDate]").val(GoodsInspection.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(GoodsInspection.TEXTEMPTY);
    $("[id$=ToDate]").val(GoodsInspection.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(GoodsInspection.TEXTEMPTY);
}
///#endregion

///#region---- Core Section Section----
///#region---- Fetch Data To Populate In Controls
function FillOtherSBUS(SelectedValue) {
    var drpID = $("select[id$=MRH_TO_BIZUNIT]").attr("id");
    $.get(RequisitionSlip.FillOtherSBUDropDownURL, function (data) {

        if (data.length > 1) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        }
        $("[id$=MRH_TO_BIZUNIT_VAL]").val($("select[id$=MRH_TO_BIZUNIT]").val());
        if (requisitionJson.MRH_IS_BZU_TRN != 1) {
            FillStore(0);
       }
    });
}

function FillStore(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store

    var drpID = $("select[id$=MRH_DEPT_STR]").attr("id");
    var Bizunit = $("[id$=BizUnitPk]").val();

    //for sbu store request
    if ($("[id$=hdfType]").val() == 2) {
        // Bizunit = $("select[id$=MRH_TO_BIZUNIT]").val();  
        Bizunit = $("[id$=MRH_TO_BIZUNIT_VAL]").val();
    }
   
    $.get(RequisitionSlip.FillStoreDropdownURL + Bizunit + "&UserFlag=0&DeptType=-1&DeptPk=0", function (data) {
        if (SelectedValue == 0 && $("[id$=hdfDeptID]").val() == $("[id$=hdfCompoundStore]").val()) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, $("[id$=hdfInvStore]").val());
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
        }

        //removing user department      
        $("#" + drpID + " option[value=" + $("[id$=hdfDeptID]").val() + "]").remove();

    });
    if (RefIdMode == 0) {
        $("select[id$=MRH_DEPT_STR]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        $("select[id$=MRH_DEPT_STR]").attr("disabled", true);
    }
}

function FillTypes(typeID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=ITM_TYPE_TEXT]").attr("id");
    $.get(RequisitionSlip.FillMaterialTypeDropdownURL + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + "Item Type", function (data) {
        if (typeID > 0) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, typeID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, $("[id$=hdfItemTypes]").val());
        }
        FillMaterialCategoryAutoComplete();
        FillCategoryMaterials(0);
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

    SelectedValue = $("[id$=hdfDeptID]").val();
    $("select[id$=DeptPk]").attr("disabled", true);

    $.get(RequisitionSlip.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=" + userFlagText + "&DeptType=-1&DeptPk=0", function (data) {
        drpID = $("select[id$=DeptPk]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        if ($("select[id$=MRH_DEPT_STR]").val() != 0) {
            $("#" + drpID + " option[value=" + $("select[id$=MRH_DEPT_STR]").val() + "]").remove();
            FillMaterialCategoryAutoComplete();
            FillCategoryMaterials(0);
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
    var ObjPurchase = new Object();
    ObjPurchase.RequisitionDetailsList = new Array();
    $("#divRequisitionData").data("RequisitionData", ObjPurchase);
    AfterSave();
    ClearGridControlDetails();
    //  FillMaterialCategory(0);
    FillMaterialCategoryAutoComplete();
}

function ClearGridControlDetails() {
    var drpUUOMID = $("select[id$=MRD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    $("[id$=CurrentStock]").html("");
    $("[id$=MRD_QTY_REQUESTED]").val("");
    $("[id$=MRD_REMARKS]").val("");
    $("[id$=MRD_IS_RETURNABLE]").val("");
}

function FillMaterialCategory(materialID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>

    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&ItemType=" + $("select[id$=ITM_TYPE_TEXT]").val() + "&Store=" + $("select[id$=MRH_DEPT_STR]").val(), function (data) {
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

function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    var BizUnit = $("[id$=BizUnitPk]").val();
    if (Type == 2) {
        BizUnit = $("select[id$=MRH_TO_BIZUNIT]").val(); 
    }
    GrandScriptUtils.MakeAutoComplete("MaterialCategory", RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + BizUnit + "&ItemType=" + $("select[id$=ITM_TYPE_TEXT]").val() + "&Store=" + $("select[id$=MRH_DEPT_STR]").val() + "&AUTOSEARCH=1" + "&Stock=" + $("[id$=hdfMaterialWithStock]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "MaterialCategory") {
        FillCategoryMaterials($("[id$=MaterialCategoryPK]").val());
        FillMaterialDetails(materialID);

    }
    if (targetControlID == "Material") {
        FillMaterialDetails($("[id$=MaterialPK]").val());
    }
}

function FillCategoryMaterials(categoryID) {
    ///<summary>Function Used to Fill material based on the category  </summary>
    $("[id$=Material]").val("");
    $("[id$=MaterialPK]").val(0);
    if ($("select[id$=MaterialType]").val() != 0) {
        typePK = $("select[id$=ITM_TYPE_TEXT]").val();
    }
    storePK = $("select[id$=MRH_DEPT_STR]").val();
    if (storePK !== null && storePK > 0) {
        GrandScriptUtils.MakeAutoCompleteLimitLen("Material", RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Type=" + typePK + "&Store=" + storePK + "&Stock=" + $("[id$=hdfMaterialWithStock]").val() + "&IsActive=" + 1 + "&AUTOSEARCH=1" + "&StoreRequest=1", "MaterialPK", true, false, "MaterialCategoryPK", true, "Store", "", "", $("[id$=AutoStartValue]").val());
    }
}

function ClearGridControlDetailsExeptMaterialType() {
    var drpUItemID = $("select[id$=ITV_ITEM]").attr("id");
    var drpUUOMID = $("select[id$=MRD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpUItemID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    $("[id$=CurrentStock]").html("");
    $("[id$=MRD_QTY_REQUESTED]").val("");
    $("[id$=MRD_REMARKS]").val("");
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
        typePK = $("select[id$=ITM_TYPE_TEXT]").val();
        storePK = $("select[id$=MRH_DEPT_STR]").val();
        var drpID = $("select[id$=ITV_ITEM]").attr("id");
        $.getJSON(RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Type=" + typePK + "&Store=" + storePK + "&Stock=" + $("[id$=hdfMaterialWithStock]").val() + "&IsActive=" + 1, function (data) {
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
        $("select[id$=MRD_UOM]").removeData();
        // Get id of the UOM DropDown
        var drpID = $("select[id$=MRD_UOM]").attr("id");
        //Fill UOM Details to the UOM DropDown, Name as Text, PK as Value

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
        $("select[id$=MRD_UOM]").find("option").remove();
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
}

function GetCurrentStock(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var drpID = $("select[id$=MRD_UOM]").attr("id");
    var store = $("select[id$=MRH_DEPT_STR]").val();
    var date = $("[id$=MRH_SUBMITTED_DATE]").val();
    $.get(RequisitionSlip.GetCurrentStock + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&Store=" + store + "&Date=" + date, function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=CurrentStock]").html(numberWithCommas(parseFloat(data[0].STD_QTY_IN_STOCK).toFixed(QtyDec)));
                $("[id$=MaterialCategoryPK]").val(data[0].STD_ITEM_CATEGORY);
                $("[id$=MaterialCategory]").val(data[0].STD_ITEM_CATEGORY_TEXT);
                //$("select[id$=MRD_UOM]").val(data[0].ITM_UOM);
                FillUOM(materialID, data[0].STD_UOM);
            }
            else {
                $("[id$=CurrentStock]").html("");
                // $("select[id$=MRD_UOM]").val("0");
                FillUOM(materialID, false);
            }
        }
    });
}

function GetSRSNo() {
    ///<summary>to fill SRSNo span</summary>
    $.get(RequisitionSlip.GETSRSNoURL, function (data) {
        $("[id$=MRH_NO]").val("");
        $("[id$=MRH_NO]").val(data);
        $("[id$=lblSRS]").html(data);
    });
}

function ViewRequisition(requistID) {
    window.location = vendorListing.ViewUrl + requistID;
}

function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    //SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryTreeURL + $("[id$=SBU]").val() + MaterialMaster.Param, "Root", false, false, "0", false); // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryExceptFGTreeURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("select[id$=ITM_TYPE_TEXT]").val() + "&Store=" + $("select[id$=MRH_DEPT_STR]").val() + RequisitionSlip.Param, "Root", false, false, "0", false); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}
///#endregion

///#region---- Data Management Section----
function ShowCategory() {
    ///<summary>Function used call the tree Data For filling the Material Category </summary>
    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Choose Category", false, "700", "380", false);
    return false;
}

function RemoveDept() {
    var drpID = $("select[id$=DeptPk]").attr("id");
    if ($("select[id$=MRH_DEPT_STR]").val() != 0) {
        $("#" + drpID + " option[value=" + $("select[id$=MRH_DEPT_STR]").val() + "]").remove();
    }
    else {
        FillDepartement(0);
    }
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
    if (grdID == "grdRequisitionSlip") {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_REMARKS", $("#grdRequisitionSlip").attr("id"));
        $("#grdRequisitionSlip").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("");
            }
            var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CurrentStock", grdID);
            var qty = GrandGrid.Utilities.GetColumnValue($(this), "CurrentStock", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
            var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MRD_QTY_REQUESTED", grdID);
            var qty = GrandGrid.Utilities.GetColumnValue($(this), "MRD_QTY_REQUESTED", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }

        });
        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#ProductInsert").find("tr:eq(1)");
        }
        $("#ProductInsert").hide();
        //$("#ProductInsert").css({ "display": "none", "visibility": "hidden" });
        $(tdset).insertBefore($("#grdRequisitionSlip").find("tr:eq(1)"));
        //for hiding action fields of detail section 23-11-11
        var queryStr = window.location.search.substring(1);
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");

        var isViewMode = false;
        if (parseInt($("[id$=MRH_IS_EDIT]").val()) != 1) {
            var queryStr = window.location.search.substring(1);
            if (queryStr != "") {
                var queryStr = queryStr.split("&")
                for (var i = 0; i < queryStr.length; i++) {
                    var pK = queryStr[i].split("=");

                    if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                        $("#grdRequisitionSlip th:last").hide();
                        $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                            $(this).find("td:last").hide();
                            $(this).find("td:last").hide();
                            $("select[id$=DeptPk]").attr("disabled", true);
                            $("select[id$=MRH_DEPT_STR]").attr("disabled", true);
                            $("[id$=MRH_SUBMITTED_DATE]").attr("disabled", true);
                            $("select[id$=ITM_TYPE_TEXT]").attr("disabled", true);
                            $("select[id$=MRH_COMPANY]").attr("disabled", true);
                            $("select[id$=MRH_TO_BIZUNIT]").attr("disabled", true);
                        });
                    }
                }
            }
        }
        if (ObjRequisition != undefined) {
            if (parseInt($("[id$=MRH_IS_EDIT]").val()) != 1) {
                if (ObjRequisition.MRH_STATUS == 1 || ObjRequisition.MRH_STATUS == 2 || ObjRequisition.MRH_STATUS == 7) {
                    $("#grdRequisitionSlip th:last").hide();
                    $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                        $(this).find("td:last").hide();
                        $(this).find("td:last").hide();
                        $("select[id$=DeptPk]").attr("disabled", true);
                        $("select[id$=MRH_DEPT_STR]").attr("disabled", true);
                        $("[id$=MRH_SUBMITTED_DATE]").attr("disabled", true);
                        $("select[id$=ITM_TYPE_TEXT]").attr("disabled", true);
                        $("select[id$=MRH_COMPANY]").attr("disabled", true);
                        $("select[id$=MRH_TO_BIZUNIT]").attr("disabled", true);
                    });
                }
            }
        }
    }
    $("[id$=CurrentStock]").html("");
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button])").each(function () {
    //        var idval = $(this).attr("id");
    ////        if (!Checkstatus(idval)) {
    ////            $(this).val("");
    ////        }
    //    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();

    window.location = RequisitionSlip.REDIRECTURLAFTERSAVE;

    //    ClearSearchDetails();
    //    PageInit();
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
    window.location = RequisitionSlip.PRINTURL + "?RequisitionID=" + $("input[id$=MRH_PK]").val();
    return false;
}

function SavePage(command) {
    ///<summary>Function used to saving   </summary>
    $.get(RequisitionSlip.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(RequisitionSlip.SessionExpired, RequisitionSlip.Confirmation, RequisitionSlip.LOGOUT, true);
            result = false;
        }
        else {
            $("[id$=MRH_COMPANY]").attr("disabled", false);
            RemoveValidations();
            // RemovePopupValidations();
            AddValidations(2);
            var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
            // Check Have The RequisitionDetailsList have More than or equal to one Requisition Details
            if ($(document.forms[0]).valid()) {
                if (ObjRequisition.RequisitionDetailsList.length > 0) {
                    //Showing validation for Future Date selection
                    if ($("[id$=hdfIsContFutureDate]").val() != "1") {
                        var RetVal = CompareDate($("[id$=MRH_SUBMITTED_DATE]").val(), $("[id$=hdfCurrentDate]").val());
                        if (RetVal == 1) {
                            ShowFutureDate(command);
                            return false;
                        }
                    }
                    //To Prevent Muliple Click
                    if ($("[id$=SubmitFlag]").val() == "0")
                        $("[id$=SubmitFlag]").val('1')
                    else
                        return false;

                    $("select[id$=DeptPk]").attr("disabled", false);
                    $("select[id$=MRH_COMPANY]").attr("disabled", false);
                    $("select[id$=MRH_DEPT_STR]").attr("disabled", false);
                    $("[id$=MRH_SUBMITTED_DATE]").attr("disabled", false);

                    ObjRequisition = $("#divRequisitionData").data("RequisitionData");
                    //Assigning the Requisition details to a hidden field by converting the object to string using Json Stringify Methord
                    $("[id$=RequisitionDetailsList]").val(JSON.stringify(ObjRequisition.RequisitionDetailsList));
                    $("[id$=WKF_FLAG]").val("0");
                    //making json string 
                    //checking command value is draft if it is draft then action id is zero means it is not calling workflow.
                    if (command != "Draft") {
                        $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
                        if ($("[id$=ReferenceID]").val() == "0") {
                            $("[id$=WKF_FLAG]").val("1");
                        }
                    }
                    else
                        $("[id$=ActionID]").val('0');

                    if (Type == 2) {
                        $("[id$=MRH_IS_RETURNABLE]").val('1')
                        $("[id$=MRH_IS_BZU_TRN]").val('1') //bit for identify sbu store request , 1 for sbu transation 0 for other
                    }
                    else {
                        $("[id$=MRH_IS_RETURNABLE]").val('0')
                        $("[id$=MRH_IS_BZU_TRN]").val('0')
                    }

                    $("[id$=hdfType]").val(Type);

                    //  var jSonString = GrandScriptUtils.FormToJsonString("divXml");
                    $("#updateProgress").show();
                    var jSonString = GrandScriptUtils.FormToJsonString(false);
                    $.post(RequisitionSlip.RequisitionSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
                        if (parseInt(data[0]) == 0) {
                            GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionCodeAlreadyAdded, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                            $("[id$=SubmitFlag]").val('0')
                        }                        
                        else if (parseInt(data[0]) > 0) {
                            //##### Start Change Code Here #####//
                            // If Action is Draft Save
                            if (command == "Draft") {
                                var SaveMessageWithSRSNo = RequisitionSlip.SRSSavedMessage;
                                if ($("[id$=AST_DOC_MODE]").val() == "1")
                                    SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + data[1] + " " + RequisitionSlip.RequisitionSaveMessage2
                                GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                                PageInit();
                                AfterSave();
                            }
                            // If action - WorkFlow Save
                            else {
                                $("[id$=hdfAppID]").val(data[0]);
                                $("[id$=AppNo]").val(data[1]);
                                SaveWorkFlow();
                            }
                            //##### END Change Code Here #####//
                        }
                        else if (parseInt(data[0]) == -2) {
                            GrandScriptUtils.ShowModal(RequisitionSlip.SaveMessage1 + " " + data[1] + " " + RequisitionSlip.EditUsedByAnotherUser, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else if (parseInt(data[0]) == -35) {//MR Modify Qty less than MI Qty
                            GrandScriptUtils.ShowModal(RequisitionSlip.MRQtyLessThanMIQty, RequisitionSlip.MessageBoxTitle);
                            $("[id$=SubmitFlag]").val('0')
                        }
                        else {
                            GrandScriptUtils.ShowModal(RequisitionSlip.ActionFailedMessage);
                            $("[id$=SubmitFlag]").val('0')
                            ResetPage();
                        }
                    });
                }
                else {

                    GrandScriptUtils.ShowModal((RequisitionSlip.SelectRequestDetails).fontcolor("red"), RequisitionSlip.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                    RemoveValidations();
                }
            }
        }
    });
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var SaveMessageWithSRSNo = "";
    SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.RequisitionSaveMessage2
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.INBOX);
    } else {
        GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
    }

    // GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
}

function AddRequisitionDetails() {
    //<summary>function used to add Evaluation details to Evaluation</summary>
    //Add Validation for Evaluation Details by setting mode as 2
    RemoveValidations();
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
        var editRequisition = $("input[id$=EditRequisition]").val();
        var obj = new Object();
        var flag = true;

        //Loop used to check the Evaluation already added in the order List 
        if (parseInt(editRequisition) == 0) {
            for (var i in ObjRequisition.RequisitionDetailsList) {
                if (ObjRequisition.RequisitionDetailsList[i].MRD_ITEM == $("[id$=MaterialPK]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjRequisition.RequisitionDetailsList) {
                if (ObjRequisition.RequisitionDetailsList[i].MRD_ITEM == $("[id$=MaterialPK]").val() && parseInt(editRequisition) != ObjRequisition.RequisitionDetailsList[i].MRD_ITEM)
                    flag = false;
                break;
            }
            for (var k in ObjRequisition.RequisitionDetailsList) {
                if (parseInt(editRequisition) == ObjRequisition.RequisitionDetailsList[k].MRD_ITEM)
                    obj = ObjRequisition.RequisitionDetailsList[k];
            }
            //            if (parseInt(editRequisition) == ObjRequisition.RequisitionDetailsList[i].MRD_ITEM)
            //                obj = ObjRequisition.RequisitionDetailsList[i];
        }

        if (flag) {
            obj.MRD_PK = $("input[id$=MRD_PK]").val();
            obj.MRD_ITEM = $("[id$=MaterialPK]").val();
            obj.MaterialCode = $("[id$=Material]").val();
            obj.MaterialType = $("[id$=MaterialCategory]").val();
            obj.MaterialTypePk = $("[id$=MaterialCategoryPK]").val()
            obj.CurrentStock = $("[id$=CurrentStock]").html().replace(/[^0-9\.]+/g, "");
            obj.MRD_QTY_REQUESTED = parseFloat($("input[id$=MRD_QTY_REQUESTED]").val().replace(/[^0-9\.]+/g, "")).toFixed(QtyDec);
            obj.UOM = $("select[id$=MRD_UOM] option:selected").text();
            obj.MRD_UOM = parseInt($("select[id$=MRD_UOM]").val());          
            //            obj.UOMID = $("select[id$=UOMID] option:selected").val();
            obj.MRD_REMARKS = $("input[id$=MRD_REMARKS]").val() == "" ? " " : $("input[id$=MRD_REMARKS]").val();
            if (Type == 2) {
                obj.MRD_IS_RETURNABLE = 1; 
            }
            else {
                obj.MRD_IS_RETURNABLE = 0;
            }
            if (parseInt(editRequisition) == 0) {
                ObjRequisition.RequisitionDetailsList.push(obj);
            }
            $("#divRequisitionData").data("RequisitionData", ObjRequisition);

            ClearGridControlDetails();
            // FillMaterialCategory(0);
            GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.RequisitionDetailsList);
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
    // $("select[id$=ITV_ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    // $("input[id$=MaterialCode]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialCode, $(tr).parent().attr("id")));
    $("[id$=MaterialCategoryPK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")));
    $("[id$=MaterialCategory]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypeText, $(tr).parent().attr("id")));
    FillCategoryMaterials($("[id$=MaterialCategoryPK]").val());
    $("[id$=MaterialPK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")))
    $("[id$=Material]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialCode, $(tr).parent().attr("id")))

    // $("input[id$=MRH_SUBMITTED_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialDate, $(tr).parent().attr("id")));
    $("[id$=CurrentStock]").html(numberWithCommas(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.CurrentStock, $(tr).parent().attr("id")).replace(/[^0-9\.]+/g, "")).toFixed(QtyDec)));
    $("input[id$=MRD_QTY_REQUESTED]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialQtyRequest, $(tr).parent().attr("id")).replace(/[^0-9\.]+/g, "")).toFixed(QtyDec));
    $("select[id$=MaterialType]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")));
    $("input[id$=MRD_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialComments, $(tr).parent().attr("id")));
    $("input[id$=MRD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MRD_PK, $(tr).parent().attr("id")));
    $("input[id$=EditRequisition]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("input[id$=MaterialCode]").focus();
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialUOMID, $(tr).parent().attr("id")));
    // FillCategoryMaterials(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), $("select[id$=MRH_DEPT_STR]").val());
}

function DeleteDetails(tr) {
    ///<summary>Used fill Details of requisition for Delete</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    for (var i in ObjRequisition.RequisitionDetailsList) {
        if (ObjRequisition.RequisitionDetailsList[i].MRD_ITEM == materialID) {
            //Will delete the Evaluation details
            ObjRequisition.RequisitionDetailsList.splice(i, 1);
            break;
        }
    }
    $("#divRequisitionData").data("RequisitionData", ObjRequisition);
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.RequisitionDetailsList);
    //Used to Show the  Evaluation details when the requisition in requisition details is 0
    if (ObjRequisition.RequisitionDetailsList.length == 0) {
        //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
        //  $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
    }
}

function ClearProductDetails() {
    //<summary>function used to Clear Requisition Product Details</summary>
    $("[id$=MaterialPK]").val("0")
    $("[id$=Material]").val("Translate(Select)")
    $("[id$=MaterialCategoryPK]").val("0");
    $("[id$=MaterialCategory]").val("Translate(Select)");
    $("input[id$=MRD_ITEM]").val("0");
    $("input[id$=MRD_PK]").val("0");
    $("input[id$=EditRequisition]").val("0");
    $("input[id$=MRD_QTY_REQUESTED]").val("");
    // $("input[id$=MRH_PK]").val("0");
    $("select[id$=MRD_UOM]").val("0");
    $("input[id$=MRD_REMARKS]").val("");
    //  $("select[id$=ITV_ITEM]").val("0")
    $("input[id$=MaterialCode]").focus();
    FillMaterialCategoryAutoComplete();
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

            if (Type == 2) { //for SBU Store request
                window.location = RequisitionSlip.REDIRECTURLAFTERSAVE + "?Type=" + $("[id$=hdfType]").val(); ////for SBU Store request   
            }
            else {
                window.location = RequisitionSlip.REDIRECTURLAFTERSAVE;
            }
            break;
        //comment req        
        case RequisitionSlip.DeleteCommand:
            DeleteDetails();
            break;
        //Commend When calling        
        case RequisitionSlip.DeleteMessageCommand:
            GrandScriptUtils.ShowModal(RequisitionSlip.DeleteConfirmationMessage, RequisitionSlip.ConfirmationMessage);
            break;
        case RequisitionSlip.INBOX:
            window.location = RequisitionSlip.InboxURL;
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

        $("[id$=Material]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: RequisitionSlip.MaterialCodeValidation }
        });
        if ($("[id$=hdnIsNeededStockValidation]").val() == "1") {
            $("input[id$=MRD_QTY_REQUESTED]").rules("add", {
                required: true,
                maxlength: 12,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                max: $("[id$=CurrentStock]").html().replace(/[^0-9\.]+/g, ""),
                messages: { required: RequisitionSlip.RequisitionQuantityValidation, max: RequisitionSlip.RequestQuantityLessCurrentStock, CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
            });
        }
        else {
            $("input[id$=MRD_QTY_REQUESTED]").rules("add", {
                required: true,
                maxlength: 12,
                DecimalDigits: QtyDec,
                CustomDecimal: true,
                messages: { required: RequisitionSlip.RequisitionQuantityValidation, CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
            });
        }
        $("select[id$=MRD_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialUOMValidation }
        });
        $("input[id$=MRD_REMARKS]").rules("add", {
            maxlength: 250
        });
    }
    //Mode =  2 represents the validation for request Details Store,Departement
    else if (mode == "2") {
        $("select[id$=MRH_DEPT_STR]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.RequisitionStoreValidation }
        });
        $("input[id$=MRH_SUBMITTED_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });
        $("select[id$=DeptPk]").rules("add", {
            selectNone: true,

            messages: { selectNone: RequisitionSlip.RequisitionDepartementValidation }
        });
        $("select[id$=ITM_TYPE_TEXT]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });
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
        $("select[id$=ITM_TYPE_TEXT]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });
    }
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    //    $("select[id$=MaterialType]").rules("remove");
    //    $("select[id$=ITV_ITEM]").rules("remove");
    $("input[id$=MaterialCategory]").rules("remove");
    $("input[id$=Material]").rules("remove");

    $("select[id$=MRD_UOM]").rules("remove");
    $("input[id$=MRD_QTY_REQUESTED]").rules("remove");
    $("input[id$=MRD_REMARKS]").rules("remove");
    $("select[id$=MRH_DEPT_STR]").rules("remove");
    $("select[id$=DeptPk]").rules("remove");
    $("select[id$=ITM_TYPE_TEXT]").rules("remove");
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
    var drpID = $("select[id$=MRH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val();
    }
    else {
        getURL = RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);

    });

}


//function FillCompany(selectVal) {
//    if (selectVal == undefined || selectVal == 0) {
//        var drpID = $("select[id$=MRH_COMPANY]").attr("id");
//        $.get(RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
//            var selCompany = $("[id$=hdfSelCompany]").val();
//            GrandScriptUtils.FillDropDown(drpID, data, true, false, selCompany);
//        });
//    }
//    else {
//        var drpID = $("select[id$=MRH_COMPANY]").attr("id");
//        $.get(RequisitionSlip.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
//            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
//        });
//    }
//}
//Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

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