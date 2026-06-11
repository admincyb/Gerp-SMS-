/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="GrandTreeMulti.js" />

///#region -----Global Variables-----
var ITC_PK = 0;
var Plant = 0;
var AccountPK = 0;
var AccType = 0;
var ObjMapping = new Array();
///#endregion
//#region----------- Configuration Section ----------------

var MaterialCategory = {
    GetSBUList: "SBUConfiguration.do?Action=GetAllSBUList",
    CategortListURL: "MaterialCategory.do?Action=GetMaterialCategoryDtls&SBUPk=",
    CategortRootName: "Root",
    UOMTypeURL: "UOMManagement.do?Action=GetUOMType&SBUPk=",
    FillCategoryTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    GetAccounts: "CommonManagement.do?Action=GetAccountTypeXML&SubType=",
    GetAccountsAuto: "CommonManagement.do?Action=GetAccountTypeAuto&SubType=",
    GetSBU: "CommonManagement.do?Action=GetSBU",
    GetAccountsType: "CommonManagement.do?Action=GetAccountType&SubType=",
    SaveCommand: "SAVE",
    GetPOCategory: "PurchaseOrderGenerate.do?Action=GetPOCategory&BizUnit=",
    GetItemReportCategory: "MaterialCategory.do?Action=GetConstantMaster&BizUnit=",
    GetGrnTypes: "MaterialCategory.do?Action=GetGrnTypes&BizUnit=",
    FillCompany: "CommonManagement.do?Action=GetCompanyAutoComplete&BizUnit=",
    FillAccountType: "CommonManagement.do?Action=GetAccountTypesforMapping&BizUnit=",
    BindAccountsGridURL: "MaterialCategory.do?Action=GetMaterialCategoryAccounts&MatCagID=",
    DeleteCommand: "DELETE",
    DeleteMessageCommand: "DELETEMSG",
    MessageBoxTitle: "Translate(Information)",
    CategorySaveURL: "MaterialCategory.do?Action=SaveMaterialCategory",
    CategoryEditURL: "MaterialCategory.do?Action=GetMaterialCategory&MatCagID=",
    CategoryDeleteURL: "MaterialCategory.do?Action=DeleteMaterialCategory&MatCagID=",
    CategorySaveMessage: "Translate(MaterialCategorysavedsuccessfully)",
    CategoryNameExistsMessage: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    CategoryDeleteMessage: "Translate(MaterialCategorydeletedsuccessfully)",
    CategoryUsed: "Translate(MaterialCategoryused)",
    Information: "Translate(Information)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    Param: "&MatCagID=",
    gCagID: "",
    gSBU: "",
    ValueEmpty: ' ',
    Purchase: "3",
    Expence: "22",
    Inventory: "19",
    Sale: "4",
    Consumption: "33",
    BizUnitPk: 1,
    UOMTypeNA: 7,
    CfgTypeCategory: "PO ITEM TYPE",
    CGTValue: 42,
    CNGValue: 1,
    //validation message
    ValidationSUBMessage: "Translate(PleaseselectaSBU)",
    ValidationCategoryCodeMessage: "Translate(PleaseProvideCategoryCode)",
    ValidationCategoryNameMessage: "Translate(PleaseProvideCategoryName)",
    ValidationUOMMessage: "Translate(PleaseselectaUOMType)",
    ValidationPOCategoryMessage: "Translate(PleaseselectPOCategory)",
    ValidationPlantMessage: "Translate(ProvidePlant)",
    ValidationAccountTypeMessage: "Translate(SelectAccountType)",
    ValidationAccountMessage: "Translate(SelectAccount)",
    ValidationSameAccountType: "Same Account type for the selected Plant already exists",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)"

}

//#endregion

//#region----------- Initialization Section----------------

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
    MaterialCategory.BizUnitPk=$("[id$=BizUnitPk]").val();
    PageInit();
    $("input[id$=InactivePeriod]").ForceNumericOnly();

});

function PageInit() {
    //<summary>Function to initialize the page</summary>
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var TypePK = qstrings[i].split("=");
            if ((TypePK[0] == "Type" && TypePK[1] != "")) {
                ITC_PK = TypePK[1];
            }
        }
    }
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    Popup();
    ResetPage();
    MaterialCategory.gSBU = $("[id$=BizUnitPk]").val();
    BindSBUCategory(MaterialCategory.gSBU);
    FillUOMType(MaterialCategory.gSBU, 0);
    if (ITC_PK != 0) {
        FillCategoryTypes(ITC_PK);
    }
    else {
        FillCategoryTypes();
    }
    EnableDisableAccountMapping();
    $("[id$=CategoryCode]").focus();
    FillPurchaseAccount(0);
    FillInventoryAccount(0);
    FillSaleAccount(0);
    FillConsumptionAccount(0);
    FillCategory(0);
    FillItemReportCategory(0);
    FillGrnType(0);
    FillPlantDDL();
    //Show/Hide SaleItem Checkbox  w.r.to GlobalConfiguration
    if ($("[id$=hdfShowSaleItem]").val() == 1) {
        $("[id$=divSaleItem]").show();
    }
    else {
        $("[id$=divSaleItem]").hide();
    }

   
    

}

//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=CategoryCode]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: MaterialCategory.ValidationCategoryCodeMessage }
    });
    $("[id$=CategoryName]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: MaterialCategory.ValidationCategoryNameMessage }
    });
    $("[id$=UOMType]").rules("add", {
        selectNone: true,
        messages: { selectNone: MaterialCategory.ValidationUOMMessage }
    });
    $("[id$=PoCategory]").rules("add", {
        selectNone: true,
        messages: { selectNone: MaterialCategory.ValidationPOCategoryMessage }
    });
}
function AddPopUpValidations() {
    RemoveValidations();
    $("[id$=ddlPlantPopup]").rules("add", {
        selectNone: true,
        messages: { selectNone: MaterialCategory.ValidationPlantMessage }
    });

    $("[id$=AccountType]").rules("add", {
        selectNone: true,
        messages: { selectNone: MaterialCategory.ValidationAccountTypeMessage }
    });
        
    $("[id$=txtAccount]").rules("add", {
        selectAuto: true,
        messages: { selectAuto: MaterialCategory.ValidationAccountMessage }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    //    $(document.forms[0]).validate().resetForm();
    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

function FillPlantDDL() {
    var drpID = $("select[id$=ddlPlantPopup]").attr("id");
    //spl condition set to blank to show consolidated plant
    $.get(MaterialCategory.FillCompany + MaterialCategory.BizUnitPk + "&SplCond=&active=1", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, 0);

    });
}

function FillCategory(SelectVal) {
    ///<summary>function used to Fill which category po</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&CfgType=" + CfgTypeCategory
    //    $("select[id$=POH_ITEM_TYPE]").attr("disabled", false);
    var drpID = $("select[id$=PoCategory]").attr("id");
    $.get(MaterialCategory.GetPOCategory + MaterialCategory.BizUnitPk + "&CfgType=" + MaterialCategory.CfgTypeCategory, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectVal);
    });
}
function FillGrnType(SelectVal) {
    ///<summary>function used to Fill Grn Type category </summary>
    var drpID = $("select[id$=GrnType]").attr("id");
    $.get(MaterialCategory.GetGrnTypes + MaterialCategory.BizUnitPk, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
    });
}
function FillItemReportCategory(SelectVal) {
    ///<summary>function used to Fill Item Report Category</summary>   
    var drpID = $("select[id$=ReportCategory]").attr("id");
    $.get(MaterialCategory.GetItemReportCategory + MaterialCategory.BizUnitPk + "&CGTVALUE=" + MaterialCategory.CGTValue + "&CNGVALUE=" + MaterialCategory.CNGValue, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectVal);
    });
}



//#endregion

//#region----------- Core Section----------------

function FillUOMType(sbuPK, sVal) {
    ///<summary>Function used Fill UOM Type </summary>

    var drpID = $("[id$=UOMType]").attr("id"); // Get id of the UOM Type DropDown
    $.get(MaterialCategory.UOMTypeURL + sbuPK, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
        $("select[id$=UOMType]").val(MaterialCategory.UOMTypeNA);
    });
}

function FillCategoryTypes(typeID) {
    //<summary>Function Used to fill all Category Types</summary>
    var drpID = $("[id$=CategoryType]").attr("id");
    $.get(MaterialCategory.FillCategoryTypeDropdownURL + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + "ITEM CATEGORY VALUE", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, typeID);
    });
    if (typeID != 0) {
        $("[id$=CategoryType]").attr("disabled", "disabled");
    }
    else {
        $("[id$=CategoryType]").attr("disabled", "");
    }
}

function FillPurchaseAccount(sVal) {
    ///<summary>Function used Fill Purchase Account </summary>
    var reqObj = new Object();
    reqObj.COA_SUB_TYPE = new Array();
    reqObj.COA_PK = 0;
    reqObj.ACTIVE = 1;
    reqObj.BIZUNIT_PK = $("[id$=BizUnitPk]").val();
    reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Purchase });
    reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Expence });
    var selectedItems = JSON.stringify(reqObj);
    var drpID = $("[id$=PurAccount]").attr("id"); // Get id of the UOM Type DropDown
    $.post(MaterialCategory.GetAccounts, selectedItems , function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}
function BindSBU() {
    var drpID = $("[id$=ddlSBUPopup]").attr("id"); // Get id of the UOM Type DropDown
    $.post(MaterialCategory.GetSBU, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, $("[id$=BizUnitPk]").val());
    });

}
function FillInventoryAccount(sVal) {
    ///<summary>Function used Fill Inventory Account </summary>
    var reqObj = new Object();
    reqObj.COA_SUB_TYPE = new Array();
    reqObj.COA_PK = 0;
    reqObj.ACTIVE = 1;
    reqObj.BIZUNIT_PK = $("[id$=BizUnitPk]").val();
    reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Inventory });
    var selectedItems = JSON.stringify(reqObj);
    var drpID = $("[id$=InvAccount]").attr("id"); // Get id of the UOM Type DropDown
    // $.get(MaterialCategory.GetAccountsType + MaterialCategory.Inventory, function (data) {
    $.post(MaterialCategory.GetAccounts, selectedItems, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}

function FillSaleAccount(sVal) {
    ///<summary>Function used Fill Inventory Account </summary>
    var reqObj = new Object();
    reqObj.COA_SUB_TYPE = new Array();
    reqObj.COA_PK = 0;
    reqObj.ACTIVE = 1;
    reqObj.BIZUNIT_PK = $("[id$=BizUnitPk]").val();
    reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Sale });
    var selectedItems = JSON.stringify(reqObj);
    var drpID = $("[id$=SaleAccount]").attr("id"); // Get id of the UOM Type DropDown
    // $.get(MaterialCategory.GetAccountsType + MaterialCategory.Inventory, function (data) {
    $.post(MaterialCategory.GetAccounts, selectedItems, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}

function FillConsumptionAccount(sVal) {
    ///<summary>Function used Fill Inventory Account </summary>
    var reqObj = new Object();
    reqObj.COA_SUB_TYPE = new Array();
    reqObj.COA_PK = 0;
    reqObj.ACTIVE = 1;
    reqObj.BIZUNIT_PK = $("[id$=BizUnitPk]").val();
    reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Consumption });
    var selectedItems = JSON.stringify(reqObj);
    var drpID = $("[id$=ConsumptionAccount]").attr("id"); // Get id of the UOM Type DropDown
    // $.get(MaterialCategory.GetAccountsType + MaterialCategory.Inventory, function (data) {
    $.post(MaterialCategory.GetAccounts, selectedItems, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}


function BindSBUCategory(sbuPK) {
    //<summary>Function Used to fill all catogory based on the sbu </summary>

    ResetFields();
    //    SetTreeHeaderStructure("trvCategory", MaterialCategory.CategortListURL + sbuPK + MaterialCategory.Param, MaterialCategory.CategortRootName, false, true, "0"); // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", MaterialCategory.CategortListURL + sbuPK + "&ITCPK=" + ITC_PK + MaterialCategory.Param, MaterialCategory.CategortRootName, false, true, "0"); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command" optional="true" type="String">
    /// Click OK which which methode perform based on this command
    /// </param>

    switch (command) {
        case MaterialCategory.SaveCommand:
            MakeMultiTree(); // call the function to bind tree view
            $("[id$=CategoryCode]").focus();
            break;
        case MaterialCategory.DeleteCommand:
            MakeMultiTree(); // call the function to bind tree view
            $("[id$=CategoryCode]").focus();
            break;
        case MaterialCategory.DeleteMessageCommand:
            DeleteCategory();
            break;
        case "DELETEACC":
            DeleteDetails();
            break;
    }
}

function DisableFields() {
    ///<summary>Function used Disable some fields </summary>

    $("[id$=UOMType]").attr("disabled", "disabled");
}

function EnableFields() {
    ///<summary>Function used Enable some fields </summary>

    $("[id$=UOMType]").removeAttr("disabled");
}

function DisableDefualtCode() {
    ///<summary>Function used Disable some fields </summary>

    $("[id$=CategoryCode]").attr("disabled", "disabled");
}

function EnableDefualtCode() {
    ///<summary>Function used Enable some fields </summary>

    $("[id$=CategoryCode]").removeAttr("disabled");
}


function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd" optional="true" type="String">
    /// The Selected Element ID
    /// </param>

    MaterialCategory.gCagID = $(liAdd).attr("id"); // get the selected tree id
    MaterialCategory.gCagID = MaterialCategory.gCagID.substr(MaterialCategory.gCagID.lastIndexOf("_") + 1, MaterialCategory.gCagID.length); // fetch the exact id of category
    var cagName = $(liAdd).parent("li").find("span").html(); // get the name of category
    ResetPage();
    $("[id$=Parent]").html(cagName);
    $("[id$=MaterialCategoryParentPK]").val(MaterialCategory.gCagID);
    //Setting Category Type
    $.get(MaterialCategory.CategoryEditURL + MaterialCategory.gCagID, function (data) {
        if (data) {
            $("select[id$=CategoryType]").val(data.CategoryType);
            $("[id$=CategoryType]").attr("disabled", "disabled");
        }
    });
}

function EditSelectedTree(liEdit) {
    ///<summary>Function used Edit the tree Data </summary>
    /// <param name="liAdd" optional="true" type="String">
    /// The Selected Element ID
    /// </param>

    RemoveValidations();
    //EnableFields();
    EnableDefualtCode();
    MaterialCategory.gCagID = $(liEdit).attr("id"); // get the selected tree id
    MaterialCategory.gCagID = MaterialCategory.gCagID.substr(MaterialCategory.gCagID.lastIndexOf("_") + 1, MaterialCategory.gCagID.length);  // fetch the exact id of category
    $.get(MaterialCategory.CategoryEditURL + MaterialCategory.gCagID, function (data) {
        if (data) {
            $("[id$=MaterialCategoryPK]").val(data.MaterialCategoryPK);
            $("[id$=CategoryCode]").val(data.CategoryCode);
            if (data.IsDefault.toLowerCase() == "true") {
                DisableDefualtCode();
            }
            $("[id$=CategoryName]").val(data.CategoryName);
            FillUOMType(MaterialCategory.gSBU, data.UOMType);
            FillCategoryTypes(data.CategoryType);
            FillPurchaseAccount(data.PurAccount);
            FillCategory(data.PoCategory);
            FillItemReportCategory(data.ReportCategory);
            FillGrnType(data.GrnType);
            FillInventoryAccount(data.InvAccount);
            FillSaleAccount(data.SaleAccount);
            FillConsumptionAccount(data.ConsumptionAccount);
            FillAccountsGrid(data.MaterialCategoryPK);
            $("[id$=CategoryDesc]").val(data.CategoryDesc);
            $("[id$=MaterialCategoryParentPK]").val(data.MaterialCategoryParentPK);

            $("[id$=InactivePeriod]").val(data.InactivePeriod);

            // data.RequireInspection == "1" ? $("input[id$=RequireInspection]").attr("checked", true) : $("input[id$=RequireInspection]").attr("checked", false);
            data.Stock == "1" ? $("input[id$=Stock]").attr("checked", true) : $("input[id$=Stock]").attr("checked", false);
            $("[id$=Parent]").html(data.MaterialCategoryParentName);
            if (((data.HasChild.toLowerCase() != "true") && (data.IsUsing.toLowerCase() != "true")) && data.IsDefault.toLowerCase() == "true") {
                //EnableFields();
                $("[id$=btnDelete]").hide();
            }
            else if (((data.HasChild.toLowerCase() == "true") || (data.IsUsing.toLowerCase() == "true")) || data.IsDefault.toLowerCase() == "true") {
                //DisableFields();
                $("[id$=btnDelete]").hide();
            }
            else
                $("[id$=btnDelete]").show();
            $("[id$=CategoryCode]").focus();
            if ($("[id$=MaterialCategoryParentPK]").val() == "0" && data.HasChild == "false") {//Selected Category not have both parent and child
                $("[id$=CategoryType]").attr("disabled", "");
            }
            else {
                $("[id$=CategoryType]").attr("disabled", "disabled");
            }
            if (ITC_PK != 0) {
                $("select[id$=CategoryType]").val(ITC_PK);
                $("[id$=CategoryType]").attr("disabled", "disabled");
            }
            data.ITC_IS_SALE == "1" ? $("input[id$=ITC_IS_SALE]").attr("checked", true) : $("input[id$=ITC_IS_SALE]").attr("checked", false);
            data.ITC_IS_VCH_POST == "1" ? $("input[id$=ITC_IS_VCH_POST]").attr("checked", true) : $("input[id$=ITC_IS_VCH_POST]").attr("checked", false);
        }
    });

}

function SavePage() {
    ///<summary>Function used to Save material category details</summary>

    //EnableFields();
    EnableDefualtCode();
    AddValidations();
    $("[id$=CategoryType]").attr("disabled", "");
    if ($("[id$=InactivePeriod]").val() == "") {
        $("[id$=InactivePeriod]").val("0");
    }
    ObjMapping = $("#divSaveData").data("SaveData");
    $("[id$=MaterialAccountlst]").val(JSON.stringify(ObjMapping));
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    if ($(document.forms[0]).valid()) {
        $.post(MaterialCategory.CategorySaveURL, jSonString, function (data) {
            if (parseInt(data) > 0) { // category saved successfully.
                GrandScriptUtils.ShowModal(MaterialCategory.CategorySaveMessage, MaterialCategory.MessageBoxTitle, MaterialCategory.SaveCommand);
                ResetPage();
            }
            else if (parseInt(data) == 0) { // category name already exists.
                GrandScriptUtils.ShowModal(MaterialCategory.CategoryNameExistsMessage, MaterialCategory.MessageBoxTitle);
            }
            else { // error occured
                GrandScriptUtils.ShowModal(MaterialCategory.ActionFailedMessage, MaterialCategory.MessageBoxTitle);
                ResetPage();
            }
        });
    }
    return false;
}

function DeletePage() {
    ///<summary>Function Used to confirmation for delete, if ok then delete action takes place </summary>
    GrandScriptUtils.ShowModal(MaterialCategory.DeleteConfirmationMessage, MaterialCategory.MessageBoxTitle, MaterialCategory.DeleteMessageCommand, true);
    return false;
}

function DeleteCategory() {
    //<summary>Function Used to Delete  material category details</summary>
    var ctgID = $("[id$=MaterialCategoryPK]").val();
    $.get(MaterialCategory.CategoryDeleteURL + ctgID, function (data) {
        if (parseInt(data) > 0) // deleted successfully.
            GrandScriptUtils.ShowModal(MaterialCategory.CategoryDeleteMessage, MaterialCategory.MessageBoxTitle, MaterialCategory.DeleteCommand);
        else if (parseInt(data) == 0) // category assigned for another page
            GrandScriptUtils.ShowModal(MaterialCategory.CategoryUsed, MaterialCategory.MessageBoxTitle);
        else  // error occured
            GrandScriptUtils.ShowModal(MaterialCategory.ActionFailedMessage, MaterialCategory.MessageBoxTitle);
        ResetPage();
    });
}

function ResetFields() {
    //<summary>Function Used to Reset Page</summary>

    //    $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () { //reseting all input controls in the page
    //        var idval = $(this).attr("id");
    //        if (idval.search("MaterialCategoryPK") != -1)
    //            $(this).val("0");
    //        else if (idval.search("MaterialCategoryParentPK") != -1)
    //            $(this).val("0");

    //        else if (idval.search("UserPk") == -1) //avoid UserPk to get the value of log in user
    //            $(this).val("");
    //    });

    $("[id$=MaterialCategoryPK]").val("0");
    $("[id$=CategoryCode]").val('');
    $("[id$=CategoryName]").val('');
    $("[id$=CategoryDesc]").val('');
    $("[id$=InactivePeriod]").val('0');
    $("select[id$=UOMType]").val(MaterialCategory.UOMTypeNA);
    //$("input[id$=RequireInspection]").attr("checked", false);


    RemoveValidations(); // remove all validation
    //EnableFields(); // enable fields
    EnableDefualtCode(); // enable default code 
    $("[id$=Parent]").html(MaterialCategory.CategortRootName);
    $("[id$=btnDelete]").hide();
    return false;
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>
    var idval = "";
    //    $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () { //reseting all input controls in the page
    //        idval = $(this).attr("id");
    //        if (idval.search("MaterialCategoryPK") != -1)
    //            $(this).val("0");
    //        else if (idval.search("MaterialCategoryParentPK") != -1)
    //            $(this).val("0");

    //        else if (idval.search("UserPk") == -1) //avoid UserPk to get the value of log in user
    //            $(this).val("");
    //    });
    //    $(document.forms[0]).find("select").each(function () { //selecting the first value in all drop downs
    //        idval = $(this).attr("id");
    //        if (idval.search("SBU") == -1)
    //            $(this).val($(this).find("option:eq(0)").val());
    //    });
    $("[id$=MaterialCategoryPK]").val("0");
    $("[id$=MaterialCategoryParentPK]").val("0");
    $("[id$=CategoryCode]").val('');
    $("[id$=CategoryName]").val('');
    $("[id$=CategoryDesc]").val('');
    $("[id$=InactivePeriod]").val('0');
    $("select[id$=UOMType]").val(MaterialCategory.UOMTypeNA);
    if (ITC_PK != 0) {
        $("select[id$=CategoryType]").val(ITC_PK);
        $("[id$=CategoryType]").attr("disabled", "disabled");
    }
    else {
        $("select[id$=CategoryType]").val(0);
        $("[id$=CategoryType]").attr("disabled", "");
    }
    $("select[id$=PurAccount]").val(0);
    $("select[id$=InvAccount]").val(0);
    $("select[id$=SaleAccount]").val(0);
    $("select[id$=ConsumptionAccount]").val(0);
    // $("input[id$=RequireInspection]").attr("checked", false);
    $("input[id$=Stock]").attr("checked", true);
    RemoveValidations(); // remove all validation
    //EnableFields(); // enable fields
    EnableDefualtCode(); // enable default code 
    $("[id$=Parent]").html(MaterialCategory.CategortRootName);
    $("[id$=btnDelete]").hide();
    FillCategory(0);
    FillItemReportCategory(0);
    FillGrnType(0);
    $("input[id$=ITC_IS_SALE]").attr("checked", false);
    $("input[id$=ITC_IS_VCH_POST]").attr("checked", false);
    ResetAccountPopUp();
    ObjMapping = new Array();
    $("#divSaveData").data("SaveData", ObjMapping);
    return false;
}

//#endregion

function EnableDisableAccountMapping() {
    if ($("[id$=hdfIsMultiplePlant]").val() == "1") {
        $("#divSinglePlantAccounts").hide();
    }
    else {
        $("#divSinglePlantAccounts").show();
    }
}

function ShowMultiplePlantAccountPopUp() {
    if ($("[id$=PoCategory]").val() != '-1' && $("[id$=PoCategory]").val() != '0') {
        ResetAccountPopUp();
        AccountTypePopUp();
        FillAccountPopUp(0);
        ObjMapping = $("#divSaveData").data("SaveData");
        $("#divMappingData").data("MappingData", ObjMapping);
        GrandGrid.Utilities.ResetGrid(true, "grdMappedAcclist");
        GrandGrid.MakeGrid($("#grdMappedAcclist"), 0, ObjMapping);
        $('#divMultiplePlantAccounts').show();
        $("#divMultiplePlantAccounts").dialog("open");
        $("#divMultiplePlantAccounts").dialog({ width: 650, height: 300, resizable: true });
        if ($("[id$=hdfCategoryACBySBU]").val() == 1) {
            $("[id$=divSBUPopup]").show();
            $("[id$=divPlantPopup]").hide();
            
            BindSBU();
        }
        else {
            $("[id$=divSBUPopup]").hide();
            $("[id$=divPlantPopup]").show();
        }
        if ($("[id$=hdfIsMultiplePlant]").val() == 1) {
            $("[id$=divPlantPopup]").show();
        }
    }
    else {
        GrandScriptUtils.ShowModal(MaterialCategory.ValidationPOCategoryMessage, MaterialCategory.Information);
    }
    return false;
}

function ApplyAccountMapping() {
    ObjMapping = $("#divMappingData").data("MappingData");
    $("#divSaveData").data("SaveData", ObjMapping);
    $("#divMultiplePlantAccounts").dialog("close");
    return false;
}
function AddMappedAccountsPopup() {
    AddPopUpValidations();
    if ($(document.forms[0]).valid()) {

        var tempArray = $("#divMappingData").data("MappingData");
        ObjMapping = new Array();
        for (var i in tempArray) {

            ObjMapping.push(tempArray[i]);
        }

        var obj = new Object();
        var flag = true;
        for (var j in ObjMapping) {
            if (ObjMapping[j].ICC_COMPANY == parseInt($("select[id$=ddlPlantPopup]").val()) && ObjMapping[j].ICC_ACCOUNT_TYPE == $("select[id$=AccountType]").val()) {
                flag = false;
                break;
            }
        }
        if (flag) {
            if ($("select[id$=ddlPlantPopup]").val() != '-1' && $("select[id$=ddlPlantPopup]").val() != '0') {
                //if ($("select[id$=AccountPopUp]").val() != '-1' && $("select[id$=AccountPopUp]").val() != '0') {
                if ($("[id$=hdfAccPK]").val() > 0) {
                    obj.ICC_PK = 0;
                    obj.ICC_COMPANY = $("select[id$=ddlPlantPopup]").val();
                    obj.ICC_COMPANY_TEXT = $("select[id$=ddlPlantPopup] option:selected").text();
                    //obj.ICC_ACCOUNT = $("select[id$=AccountPopUp]").val();
                    //obj.ICC_ACCOUNT_TEXT = $("select[id$=AccountPopUp] option:selected").text();
                    obj.ICC_ACCOUNT = ($("[id$=hdfAccPK]")).val();
                    obj.ICC_ACCOUNT_TEXT = ($("[id$=txtAccount]")).val();
                    obj.ICC_ACCOUNT_TYPE = $("select[id$=AccountType]").val();
                    obj.ICC_ACCOUNT_TYPE_TEXT = $("select[id$=AccountType] option:selected").text();
                }
            }
            else {
                if ($("select[id$=ddlSBUPopup").val() != '-1' && $("select[id$=ddlSBUPopup]").val() != '0') {
                    //if ($("select[id$=AccountPopUp]").val() != '-1' && $("select[id$=AccountPopUp]").val() != '0') {
                    if ($("[id$=hdfAccPK]").val() > 0) {
                        alert(0);
                        obj.ICC_PK = 0;
                        obj.ICC_SBU = $("select[id$=ddlSBUPopup]").val();
                        obj.ICC_SBU_TEXT = $("select[id$=ddlSBUPopup] option:selected").text();
                        obj.ICC_ACCOUNT = ($("[id$=hdfAccPK]")).val();
                        obj.ICC_ACCOUNT_TEXT = ($("[id$=txtAccount]")).val();//$("select[id$=AccountPopUp] option:selected").text();
                        obj.ICC_ACCOUNT_TYPE = $("select[id$=AccountType]").val();
                        obj.ICC_ACCOUNT_TYPE_TEXT = $("select[id$=AccountType] option:selected").text();
                        obj.ICC_COMPANY_TEXT = '';
                    }
                }
            }
            
            ObjMapping.push(obj);
            $("#divMappingData").data("MappingData", ObjMapping);
            GrandGrid.Utilities.ResetGrid(true, "grdMappedAcclist");
            GrandGrid.MakeGrid($("#grdMappedAcclist"), 0, ObjMapping);
            ResetAccountPopUp();
        }
        else {
            GrandScriptUtils.ShowModal(MaterialCategory.ValidationSameAccountType, MaterialCategory.Information);
        }
    }
    return false;
}

///#region---- Data Management Section----
function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    ObjMapping = new Array();
    ObjMapping = $("#divMappingData").data("MappingData");
    var TempObj = new Array();
    for (var i in ObjMapping) {
        //if (ObjMapping[i].ICC_COMPANY != Plant || ObjMapping[i].ICC_ACCOUNT_TYPE != AccType) {
        if (ObjMapping[i].ICC_PK != AccountPK ) {
            TempObj.push(jQuery.extend(true, {}, ObjMapping[i]));
        }
    }
    ObjMapping = TempObj;
    $("#divMappingData").data("MappingData", ObjMapping);
    GrandGrid.Utilities.ResetGrid(true, "grdMappedAcclist");
    GrandGrid.MakeGrid($("#grdMappedAcclist"), 0, ObjMapping);
    ResetAccountPopUp();
    return false;
}

function AccountTypePopUp() {
    ///<summary>Function used Fill Purchase Account </summary>  
    var drpID = $("[id$=AccountType]").attr("id"); // Get id of the UOM Type DropDown
    $.post(MaterialCategory.FillAccountType + MaterialCategory.BizUnitPk + "&CfgType=ITC ACC TYPES", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, 0);
    });
}
function AccountTypeChange() {
    FillAccountPopUp($("select[id$=AccountType]").val());
}

function SBUChange() {
    AccountTypePopUp();
    FillAccountPopUp(0);
}

function FillAccountPopUp(Type) {
    ///<summary>Function used Fill Inventory Account </summary>
    var reqObj = new Object();
    reqObj.COA_SUB_TYPE = new Array();
    reqObj.COA_PK = 0;
    reqObj.ACTIVE = 1;
    if ($("[id$=hdfCategoryACBySBU]").val() == 1) {
        reqObj.BIZUNIT_PK = $("select[id$=ddlSBUPopup]").val()
    }

    if (Type == 1) {
        reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Purchase });
        reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Expence });
    }
    else if (Type == 2) {
        reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Sale });
    }
    else if (Type == 3) {
        reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Consumption });
    }
    else if (Type == 4) {
        reqObj.COA_SUB_TYPE.push({ Value: MaterialCategory.Inventory });
    }
    var selectedItems = JSON.stringify(reqObj);
    var drpID = $("[id$=AccountPopUp]").attr("id"); // Get id of the UOM Type DropDown
    // $.get(MaterialCategory.GetAccountsType + MaterialCategory.Inventory, function (data) {
    //$.post(MaterialCategory.GetAccounts, selectedItems, function (data) {
    //    GrandScriptUtils.FillDropDown(drpID, data, true, true, 0);

    //});
    //$("[id$=BizUnitPk]").val()
    
    var SBU_PK = $("[id$=BizUnitPk]").val();
    if ($("select[id$=ddlSBUPopup]").val() != null && $("select[id$=ddlSBUPopup]").val() != 'null')
        SBU_PK = $("select[id$=ddlSBUPopup]").val();

    GrandScriptUtils.MakeAutoComplete("txtAccount", MaterialCategory.GetAccountsAuto + selectedItems + "&P_COA_PK=0" + "&ACTIVE=1" + "&COA_IS_GROUP=0" + "&BIZUNIT_PK=" + SBU_PK, "hdfAccPK", true, false, 0, true);

}
function ResetAccountPopUp() {
    $("select[id$=ddlPlantPopup]").val('-1');
    $("select[id$=AccountType]").val('-1');
    $("[id$=txtAccount]").val("Select/Type");
    $("select[id$=hdfAccPK]").val('0');
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
    RemoveValidations();
    switch (command.toString()) {
        // To Delete Details          
        case "DELETEACC":
            AccountPK = GrandGrid.Utilities.GetColumnValue(tr, "ICC_PK", $(tr).parents("table:first").attr("id"));
            Plant = GrandGrid.Utilities.GetColumnValue(tr, "ICC_COMPANY", $(tr).parents("table:first").attr("id"));
            AccType = GrandGrid.Utilities.GetColumnValue(tr, "ICC_ACCOUNT_TYPE", $(tr).parents("table:first").attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MaterialCategory.DeleteConfirmationMessage, MaterialCategory.ConfirmationMessage, "DELETEACC", true);
            break;
        // Default Handler         
        default:
            alert(MaterialCategory.DefaultAction);
            break;
    }
    return false;
}
///#endregion

function FillAccountsGrid(materialPk) {
    $.get(MaterialCategory.BindAccountsGridURL + materialPk, function (data) {
        ObjMapping = new Array();
        if (data != '') {
            for (var items in data) {
                ObjMapping.push(data[items]);
            }
        }
        $("#divSaveData").data("SaveData", ObjMapping);
    });
}


function Popup() {
    ///<summary>Function used for popup</summary>
    $("#divMultiplePlantAccounts").dialog({
        autoOpen: false,
        width: 600,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}
