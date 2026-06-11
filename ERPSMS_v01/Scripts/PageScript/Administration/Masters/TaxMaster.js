/// <reference path="../../../jquery/jquery-1.5.min.js" />
/// <reference path="../../../GrandScriptUtils.js" />


///#region------ Global Variable Declaration ---------------
var TaxID;
//#endregion

///#region------ Configuration Settings ---------------
var TaxSettings = {
    //Url
    GetTaxParameters: "TaxSettings.do?Action=GetTaxParameters",
    GetAccountWithCode: "CommonManagement.do?Action=GetAccountWithCode",
    SaveTaxParameters: "TaxSettings.do?Action=SaveTaxDetails&TaxType=",
    DeleteTaxParameters: "TaxSettings.do?Action=DeleteTaxDetails&TaxPK=",
    GetTaxDetails: "TaxSettings.do?Action=GetTaxTypeDetails",
    TaxAutoComplete: "TaxSettings.do?Action=GetSearchValue",
    GetTaxCategory: "TaxSettings.do?Action=GetTaxTypeCategory",
    GetTaxSubType: "TaxSettings.do?Action=GetCfgValue",
    Inbox: "../AccountManagement/WorkflowInbox.aspx",
    //Commands
    Saved: "SAVED",
    Edit: "EDIT",
    Delete: "DELETE",
    InvalidExpression: "INVALIDEXPRESSION",
    DeleteMessageCommand: "DELETE",
    //MESSAGE
    Title: "Information",
    InvalidExpression: "Translate(InvalidExpression)",
    SavedSuccessfully: "Translate(TaxSaved)",
    POChargeSavedSuccess: "Translate(ChargeSaved)",
    DeductionSavedSuccess: "Translate(DeductionSaved)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    UpdatedSuccessfully: "Translate(TaxUpdate)",
    DeletedSuccessfully: "Translate(TaxMasterDeletedSuccessfully)",
    POChargeDeletedSuccess: "Translate(POChargeDeletedSuccess)",
    DeductionDeletedSuccess: "Translate(DeductionDeletedSuccess)",
    AlreadyAssigned: "Translate(TaxMasterAlreadyAssigned)",
    NameAlreadyExist: "Translate(TaxNameAlreadyExists)",

    //Fields
    TaxPK: "TAX_PK",
    TAX_Type: "TAX_Type",
    TAX_DISC_FROM_DT: "TAX_DISC_FROM_DT",
    TAX_EXT_FROM_DT: "TAX_EXT_FROM_DT",
    TAX_EXT_TO_DATE: "TAX_EXT_TO_DATE",
    TAX_HEAD: "TAX_HEAD",
    TAX_CATEGORY: "TAX_CATEGORY",
    TAX_PARM: "TAX_PARM",
    TAX_FORMULA: "TAX_FORMULA",
    TAX_FROM_DT: "TAX_FROM_DT",
    TAX_TO_DT: "TAX_TO_DT",
    TAX_ACCOUNT: "TAX_ACCOUNT",
    COA_NAME: "COA_NAME",
    COA_CODE: "COA_CODE",
    TAX_NOT_DUE: "TAX_NOT_DUE",
    TAX_DESC: "TAX_DESC",
    TAX_DISP_NAME: "TAX_DISP_NAME",

    TAX_RATE: "TAX_RATE",
    TAX_CODE: "TAX_CODE",
    TAX_IS_SALE: "TAX_IS_SALE",
    TAX_IS_PURCHASE: "TAX_IS_PURCHASE",
    TAX_IS_RETURN: "TAX_IS_RETURN",
    TAX_IS_FOB_CAL: "TAX_IS_FOB_CAL",
    TAX_ACTIVE: "TAX_ACTIVE",
    TAX_AUTO_OTHER_ENABLE: "TAX_AUTO_OTHER_ENABLE"

}
//#endregion

///#region------ Initialization Section ---------------

$(document).ready(function () {
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");

    SetAccount();
    PageInit();
});

//$("[id$='TAX_SUB_CATEGORY']").live("change", function () {
//    SetAccount();
//});

$("[id$='TAX_IS_SALE']").live("change", function () {
    SetAccountBasedonApplicability(1);
});

$("[id$='TAX_IS_PURCHASE']").live("change", function () {
    SetAccountBasedonApplicability(0);
});

function SetAccountBasedonApplicability(result, SelectVal) {
    //1. For Sale
    //0. Purchase
    if ($("[id$='hdfAccountPK']").val() == "-1") {
        if (result == 1) {
            $("input[id$=TAX_IS_PURCHASE]").attr("checked", false);
            $("input[id$=TAX_IS_SALE]").attr("checked", true);
            $("[id$='TAX_SUB_CATEGORY']").val(4);
            //FillAccount(SelectVal, 14);
        }
        else if (result == 0) {
            $("input[id$=TAX_IS_SALE]").attr("checked", false);
            $("input[id$=TAX_IS_PURCHASE]").attr("checked", true)
            $("[id$='TAX_SUB_CATEGORY']").val(4);
            //FillAccount(SelectVal, 5);
        }
        else {
            $("[id$='TAX_SUB_CATEGORY']").val(3);
            //sFillAccount(SelectVal, 15);
        }
    }
    $("[id$='hdfAccountPK']").val("-1");
    return false;
}

function FillAccount(SelectVal, accType) {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    var drpID = $("select[id$=TAX_ACCOUNT]").attr("id");
    $("[id$='hdfAccountPK']").val(SelectVal);
    SelectVal == null || SelectVal == "null" || SelectVal == undefined ? SelectVal = 0 : SelectVal = SelectVal;
    var url = TaxSettings.GetAccountWithCode + "&Active=1" + "&SubType=" + accType + "&IsGroup=0&AccountPK=" + SelectVal;
    $.get(url, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
    });
}



function SetAccount(SelectVal) {
    //    $("[id$=COA_NAME]").val("Select/Type");
    //    $("[id$=TAX_ACCOUNT]").val("0");
    //    var pageURL = window.document.URL;
    //    var virtualPath = $("[id$=hdfVirtualPath]").val();
    //    var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    $("input[id$=TAX_IS_SALE]").attr("disabled", false);
    $("input[id$=TAX_IS_PURCHASE]").attr("disabled", false);
    if ($("[id$='hdfAccountPK']").val() == "-1") {
        var accType = 0;
        if ($("[id$='TAX_SUB_CATEGORY']").val() == "3" || $("[id$='TAX_SUB_CATEGORY']").val() == "2" || $("[id$='TAX_SUB_CATEGORY']").val() == "1") {
            if ($("[id$='TAX_SUB_CATEGORY']").val() == "1")
                accType = 5;
            else if ($("[id$='TAX_SUB_CATEGORY']").val() == "2")
                accType = 14;
            else if ($("[id$='TAX_SUB_CATEGORY']").val() == "3") {
                accType = 15;
                $("input[id$=TAX_IS_SALE]").attr("checked", false);
                $("input[id$=TAX_IS_PURCHASE]").attr("checked", false);
                $("input[id$=TAX_IS_SALE]").attr("disabled", true);
                $("input[id$=TAX_IS_PURCHASE]").attr("disabled", true);
            }
            //            else if ($("[id$='TAX_SUB_CATEGORY']").val() == "4")
            //                accType = 5;
            FillAccount(SelectVal, accType);
        }
        else {
            $("input[id$=TAX_IS_SALE]").attr("checked", false);
            $("input[id$=TAX_IS_PURCHASE]").attr("checked", false);
            FillAccount(SelectVal, 0);
        }
    }
    $("[id$='hdfAccountPK']").val("-1");
    return false;
}


function DisableAuto(extender, hfield) {
    ///<summary>
    /// Used to disable Autocomplete
    ///</summary>
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
    $(extender).autocomplete("option", "disabled", true);
    $(extender).attr("disabled", true);
}
function EnableAuto(extender) {
    ///<summary>
    /// Used to enable Autocomplete
    ///</summary>
    $(extender).removeAttr("disabled");
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
    $(extender).autocomplete("option", "disabled", false);
}


function PageInit() {
    ///<summary>Function Initialize page details </summary>
    if ($("[id$=hdfGstEnabled]").val() == "1") {
        $("[id$=divGSTGroup]").show();
     }
    else {
        $("[id$=divGSTGroup]").hide();
    }
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("#DIVTAX_DISC").hide();
    $("#DIVTAX_EXT").hide();
    //Filling tax paremeters
    FillCategory(0);
    FillGstGroup(0);
    FillParmeters();
    FillTaxSubCategory(0);
    GrandScriptUtils.AddDateRange("TAX_FROM_DT", "taxfromdt", "TAX_TO_DT", "taxtodt", false, false, true);
    $("[id$=EXT_FROM]").val("");
    $("[id$=hdfextfrom]").val("");
    $("[id$=EXT_TO]").val("");
    $("[id$=hdfextto]").val("");
    $("[id$=DISC_FROM]").val("");
    $("[id$=BIZUNIT]").val($("[id$=BizUnitPk]").val());
    $("[id$=UserID]").val($("[id$=UserPk]").val());
    $(".checkbx").hide();
    BindGrid();
    //setting search type.
    SetSearchType();
    //initializing search.  
    SearchInit();
    $("[id$=divTaxFob]").hide();
    var taxFormulaLabelText = "Translate(FormulaStar)";
 
    if ($("[id$=Type]").val() == "1") {
        $("[id$=divTaxType]").show();
        $("[id$=divTaxnotDue]").show();
        $("[id$=divTaxcode]").show();
        $("[id$=divTaxApplicability]").show();
        $("[id$=divTaxActive]").show();
        $("[id$=lblTAX_Formula]").text(taxFormulaLabelText);
        $("[id$=divAutoCalculate").hide();
    }
    else if ($("[id$=Type]").val() == "4") {
        $("[id$=divTaxType]").show();
        $("[id$=divTaxnotDue]").hide();
        $("[id$=divTaxcode]").show();
        $("[id$=divTaxApplicability]").hide();
        $("[id$=divTaxActive]").show();
        FillCategory(4);
        taxFormulaLabelText = "Translate(Formula)";
        $("[id$=lblTAX_Formula]").text(taxFormulaLabelText);
        $("[id$=SearchType] option[value='TAX_IS_SALE_TEXT']").remove();
        $("[id$=SearchType] option[value='TAX_IS_PURCHASE_TEXT']").remove();
        $("[id$=divAutoCalculate").show();
    }
    else {
        $("[id$=divTaxType]").hide();
        $("[id$=divTaxnotDue]").hide();
        $("[id$=divTaxcode]").hide();
        $("[id$=divTaxApplicability]").hide();
        $("[id$=divTaxActive]").hide();
        $("[id$=lblTAX_Formula]").text(taxFormulaLabelText);
        $("[id$=SearchType] option[value='TAX_IS_SALE_TEXT']").remove();
        $("[id$=SearchType] option[value='TAX_IS_PURCHASE_TEXT']").remove();
        $("[id$=divAutoCalculate").show();
    }

    return false;
}

function FillCategory(catgID) {

    ///<summary>Function used to fill Paremeter details for tax </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=TAX_CATEGORY]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    var IsTax = 0;
    if ($("[id$=Type]").val() == "1")
        IsTax = 1;
    else if ($("[id$=Type]").val() == "4")
        IsTax = 4;
    $.get(TaxSettings.GetTaxCategory + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&IsTax=" + IsTax, function (data) {
        if ($("[id$=Type]").val() == "1")
            GrandScriptUtils.FillDropDown(drpID, data, true, false, catgID);
        else if ($("[id$=Type]").val() == "4")
            GrandScriptUtils.FillDropDown(drpID, data, true, false, catgID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true, catgID);
    });
}

function FillTaxSubCategory(subTypeID) {
    ///<summary>Function used to fill Paremeter details for tax </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=TAX_SUB_CATEGORY]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    $.get(TaxSettings.GetTaxSubType + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&CfgValue=TAX TYPE", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, subTypeID);
    });
}

function FillGstGroup(groupID) {
    var drpID = $("select[id$=TAX_GST_GROUP]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    $.get(TaxSettings.GetTaxSubType + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&CfgValue=GST GROUP", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, groupID);
    });

}

function FillParmeters(paramid) {
    ///<summary>Function used to fill Paremeter details for tax </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=TAX_PARM]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    $.get(TaxSettings.GetTaxParameters, function (data) {
        if ($("[id$=Type]").val() == "1") {
            GrandScriptUtils.FillDropDown(drpID, data, true, false);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, false);
        }
    });
}

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    if ((strname == "TAX_IS_SALE_TEXT" && $("[id$=SearchValue]").val() == "") || (strname == "TAX_IS_PURCHASE_TEXT" && $("[id$=SearchValue]").val() == "")) {
        $("[id$=SearchValue]").val("yes");
    }

    $("[id$=SearchValue]").show()
    $("[id$=imbSearch]").show();
    BindGrid();
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", TaxSettings.TaxAutoComplete, "SearchType", false, "SBU");
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}
///#endregion

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = TaxSettings.GetTaxDetails + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&IsTax=" + $("[id$=Type]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val();
    if ($("[id$=Type]").val() == "1") {
        $("[id$=divPocharges]").hide();
        $("[id$=divDeductionTaxList]").hide();
        $("#grdTaxDetails").removeAttr("ajaxurl");
        $("#grdTaxDetails").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdTaxDetails");
        GrandGrid.MakeGrid($("#grdTaxDetails"));
    }
    else if ($("[id$=Type]").val() == "4") {
        $("[id$=divTaxList]").hide();
        $("[id$=divPocharges]").hide();
        $("#grdDeductionTax").removeAttr("ajaxurl");
        $("#grdDeductionTax").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdDeductionTax");
        GrandGrid.MakeGrid($("#grdDeductionTax"));
    }
    else {
        $("[id$=divTaxList]").hide();
        $("[id$=divDeductionTaxList]").hide();
        $("#grdPOCharges").removeAttr("ajaxurl");
        $("#grdPOCharges").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdPOCharges");
        GrandGrid.MakeGrid($("#grdPOCharges"));
    }
    return false;
}

///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    $("[id$=COA_NAME]").val("Select/Type");
    $("[id$=btnSave]").show();
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    //    $("input[id$=TAX_HEAD]").focus();
    $("select[id$=TAX_CATEGORY]").focus();
    $(".checkbx").hide();
    $("input[id$=TAX_NOT_DUE]").attr("checked", false);
    $("input[id$=TAX_IS_SALE]").attr("disabled", false);
    $("input[id$=TAX_IS_PURCHASE]").attr("disabled", false);
    $("input[id$=TAX_IS_FOB_CAL]").attr("checked", false);
    $("[id$=divTaxFob]").hide();
    if ($("[id$=Type]").val() != "1") {
        MakeFormula();
    }

    return false;
}

function RedirectToInbox() {
    window.location = TaxSettings.Inbox;
    return false;
}
function ReloadePage() {
    ResetPage();
    //window.location.reload();
    return false;
}


//#endregion

///#region------ Core Section ---------------


function TaxDetails(controlID) {
    ///<summary>Function used to fill Paremeter details for tax </summary>
    /// <param name="controlID"  type="Object">
    ///Control which causes the event to raise
    /// </param>
    var controlID = $(controlID).attr("id");
    if (controlID.indexOf('TAX_DISC') != -1) {
        AddBetweenDateRange("taxfromdt", "hdfchkToDate", "DISC_FROM", "taxdisFrm", false, false);
        // AddAfterDateRange("taxfromdt", "DISC_FROM", "hdfextto", false, false);
        $("#DIVTAX_DISC").show();
        $("#DIVTAX_EXT").hide();
        $("input[id$=TAX_EXT]").attr("checked", false);
        //20-01-2014
        $("input[id$=EXT_TO]").val("");
        $("input[id$=DISC_FROM]").val("");

    }
    else if (controlID.indexOf('TAX_EXT') != -1) {
        $("#DIVTAX_DISC").hide();
        $("#DIVTAX_EXT").show();
        $("input[id$=TAX_DISC]").attr("checked", false);
        $("input[id$=EXT_FROM]").val("");
        //20-01-2014
        $("input[id$=DISC_FROM]").val("");
        $("input[id$=EXT_TO]").val("");
        AddAfterDateRange("hdfchkToDate", "EXT_TO", "hdfextto", false, false);
    }
    return false;
}

function AfterGridBind(grdID) {
    if (grdID == "grdTaxDetails") {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_SUB_CATEGORY_TEXT", $("#grdTaxDetails").attr("id"));
        ColIndexrate = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_RATE", $("#grdTaxDetails").attr("id"));
        ColIndexcode = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_CODE", $("#grdTaxDetails").attr("id"));
        $("#grdTaxDetails").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("-");
            }
            if ($(this).find("td:eq(" + ColIndexrate + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexrate + ")").html("");
            }
            if ($(this).find("td:eq(" + ColIndexcode + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexcode + ")").html("");
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_STATUS", "grdTaxDetails");
            colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_ACTIVE", "grdTaxDetails");
            if (colIndex != null) {
                if (colIndexActive != null) {
                    if ($(this).find("td:eq(" + colIndexActive + ")").html() == 0) {
                        $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                    }
                    else {
                        $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                    }
                }
            }
        });


    }
    else if (grdID == "grdPOCharges") {
        ColIndexcategory = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_CATEGORY_TEXT", $("#grdPOCharges").attr("id"));
        $("#grdPOCharges").find("tr:has(td)").each(function (index) {
            if ($(this).find("td:eq(" + ColIndexcategory + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexcategory + ")").html("");
            }
        });
    }
    else if (grdID == "grdDeductionTax") {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_SUB_CATEGORY_TEXT", $("#grdDeductionTax").attr("id"));
        ColIndexrate = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_RATE", $("#grdDeductionTax").attr("id"));
        ColIndexcode = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_CODE", $("#grdDeductionTax").attr("id"));
        $("#grdDeductionTax").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_STATUS", $("#grdDeductionTax").attr("id"));
            colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "TAX_ACTIVE", $("#grdDeductionTax").attr("id"));
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("-");
            }
            if ($(this).find("td:eq(" + ColIndexrate + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexrate + ")").html("");
            }
            if ($(this).find("td:eq(" + ColIndexcode + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexcode + ")").html("");
            }
            if (colIndex != null) {
                if (colIndexActive != null) {
                    if ($(this).find("td:eq(" + colIndexActive + ")").html() == 0) {
                        $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_ITEM_D_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                    }
                    else {
                        $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_ITEM_D_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                    }
                }
            }
        });
    }
}

function FillDetails(tr) {

    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param> 
    AddNew();
    var tableId = $(tr).parents("table:first").attr("id");
    var Taxtype = GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_Type, tableId);
    $("input[id$=TaxType]").val(Taxtype);
    $(".checkbx").show();
    $("input[id$=TAX_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TaxPK, tableId));
    var SubType = GrandGrid.Utilities.GetColumnValue(tr, "TAX_SUB_CATEGORY", tableId);
    var CategoryType = GrandGrid.Utilities.GetColumnValue(tr, "TAX_CATEGORY", tableId);
    FillTaxSubCategory(GrandGrid.Utilities.GetColumnValue(tr, "TAX_SUB_CATEGORY", tableId));
    FillGstGroup(GrandGrid.Utilities.GetColumnValue(tr, "TAX_GST_GROUP", tableId)); 
    $("input[id$=TAX_HEAD]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_HEAD, tableId));    
    //Adding Display name
    $("input[id$=TAX_DISP_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DISP_NAME, tableId));
    $("[id$=TAX_CATEGORY]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_CATEGORY, tableId));
    $("[id$=TAX_ACTIVE]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACTIVE, tableId));
    $("[id$=TAX_PARM]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_PARM, tableId));
    $("input[id$=TAX_Formula]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FORMULA, tableId));
    $("[id$=TAX_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DESC, tableId) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DESC, tableId));
    //$("input[id$=TAX_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DESC, tableId));
    $("input[id$=TAX_FROM_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId));
    $("input[id$=taxfromdt]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId));
    $("input[id$=taxfromdt]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId)));
    $("input[id$=TAX_TO_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId));
    $("input[id$=taxtodt]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId)));
    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_NOT_DUE, tableId) == "1") {
        $("input[id$=TAX_NOT_DUE]").attr("checked", true);
    }
    else {
        $("input[id$=TAX_NOT_DUE]").attr("checked", false);
    }

    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_AUTO_OTHER_ENABLE, tableId) == 1) {
        $("input[id$=TAX_AUTO_OTHER_ENABLE]").attr("checked", true);
    }
    else {
        $("input[id$=TAX_AUTO_OTHER_ENABLE]").attr("checked", false);
    }

    if ($("[id$=Type]").val() == "1") {
        if (SubType == "3" || SubType == "2" || SubType == "1") {
            if (SubType == "1")
                accType = 5;
            else if (SubType == "2")
                accType = 14;
            else if (SubType == "3") {
                accType = 15;
                $("input[id$=TAX_IS_SALE]").attr("checked", false);
                $("input[id$=TAX_IS_PURCHASE]").attr("checked", false);
                $("input[id$=TAX_IS_SALE]").attr("disabled", true);
                $("input[id$=TAX_IS_PURCHASE]").attr("disabled", true);
            }
            //            else if (SubType == "4")
            //                accType = 5;
            FillAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId), accType);
        }
        else {
          
            FillAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId), 0);
        }
    }
    else if ($("[id$=Type]").val() == "4") {
      
        accType = 0;
        if (SubType == "3")
            accType = 15;
        FillAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId), accType);
    }
    else {
       
        if (CategoryType == "2" || CategoryType == "3") {
            if (CategoryType == "2") {
                accType = 6;
                $("[id$=divTaxFob]").show();
            }
            if (CategoryType == "3") {
                accType = 7;
                $("[id$=divTaxFob]").hide();
            }
            FillAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId), accType);
        }
        else {
            FillAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId), 0);
        }
    }

    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_IS_FOB_CAL, tableId) == "1") {
        $("input[id$=TAX_IS_FOB_CAL]").attr("checked", true);
    }
    else {
        $("input[id$=TAX_IS_FOB_CAL]").attr("checked", false);
    }

    $("[id$=TAX_RATE]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_RATE, tableId) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_RATE, tableId));
    $("input[id$=TAX_CODE]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_CODE, tableId));
    $("[id$='hdfAccountPK']").val("-1");
    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_IS_SALE, tableId) == "1") {
        $("input[id$=TAX_IS_SALE]").attr("checked", true);
        SetAccountBasedonApplicability(1, GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId));
    }
    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_IS_PURCHASE, tableId) == "1") {
        $("input[id$=TAX_IS_PURCHASE]").attr("checked", true);
        SetAccountBasedonApplicability(0, GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId));
    }

    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_IS_RETURN, tableId) == "1") {
        $("input[id$=TAX_IS_RETURN]").attr("checked", true);
    }
    else {
        $("input[id$=TAX_IS_RETURN]").attr("checked", false);
    }

    //    if (GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId) != "null") {
    //       // $("[id$=COA_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.COA_CODE, tableId) + " - " + GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.COA_NAME, tableId));
    //        // $("[id$=TAX_ACCOUNT]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId));TAX_SUB_CATEGORY
    //        SetAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId));
    //        //FillAccount(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_ACCOUNT, tableId), GrandGrid.Utilities.GetColumnValue(tr, "TAX_SUB_CATEGORY", tableId))
    //    }
    //    else {
    ////        $("[id$=COA_NAME]").val("Select/Type");
    //        //        $("[id$=TAX_ACCOUNT]").val("0");
    //        //FillAccount(0, 0);
    //        SetAccount();
    //    }
    if (Taxtype == "0") {
        $("input[id$=TAX_EXT]").attr("checked", false);
        $("input[id$=TAX_DISC]").attr("checked", false);
        $("#DIVTAX_DISC").hide();
        $("#DIVTAX_EXT").hide();
        $("input[id$=TaxDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId)));
        $("input[id$=hdfchkToDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId)));
    }
    else if (Taxtype == "1") {
        $("input[id$=TAX_EXT]").attr("checked", false);
        $("input[id$=TAX_DISC]").attr("checked", true);
        $("input[id$=DISC_FROM]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DISC_FROM_DT, tableId));
        $("input[id$=hdfchkToDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DISC_FROM_DT, tableId)));
        AddBetweenDateRange("taxfromdt", "hdfchkToDate", "DISC_FROM", "taxdisFrm", false, false);
        $("#DIVTAX_DISC").show();
        $("#DIVTAX_EXT").hide();
        $("input[id$=TaxDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId)));

    }
    else if (Taxtype == "2") {
        $("input[id$=TAX_EXT]").attr("checked", true);
        $("input[id$=TAX_DISC]").attr("checked", false);
        //        $("input[id$=EXT_FROM]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_FROM_DT, tableId));
        $("input[id$=EXT_TO]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_TO_DATE, tableId));
        //$("input[id$=hdfextto]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_TO_DATE, tableId)));
        $("input[id$=hdfchkToDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_TO_DATE, tableId)));
        AddAfterDateRange("hdfchkToDate", "EXT_TO", "hdfextto", false, false);
        $("#DIVTAX_DISC").hide();
        $("#DIVTAX_EXT").show();
        $("input[id$=TaxDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_TO_DATE, tableId)));
    }

}

function SetPOChargeAccount(SelectVal) {

    if ($("[id$='hdfAccountPK']").val() == "-1") {
        var accType = 0;
        if ($("[id$='TAX_CATEGORY']").val() == "2" || $("[id$='TAX_CATEGORY']").val() == "3") {
            if ($("[id$='TAX_CATEGORY']").val() == "2") {
                accType = 6;
                $("input[id$=TAX_IS_FOB_CAL]").attr("checked", false);
                $("[id$=divTaxFob]").show();
            }
            if ($("[id$='TAX_CATEGORY']").val() == "3") {
                accType = 7;
                $("input[id$=TAX_IS_FOB_CAL]").attr("checked", false);
                $("[id$=divTaxFob]").hide();
            }
            FillAccount(SelectVal, accType);
        }
        else {
            FillAccount(SelectVal, 0);
        }
    }
    $("[id$='hdfAccountPK']").val("-1");
    return false;
}

function MakeFormula() {
    ///<summary>Function used to Fill Formula corresponding to the paramtere selected </summary>

    var tempstr = $("input[id$=TAX_Formula]").val();
    if ($("select[id$=TAX_PARM]").val() != "0") {
        tempstr = tempstr + $("select[id$=TAX_PARM]").val();
        $("input[id$=TAX_Formula]").val(tempstr);
    }
    return false;
}

function GetPOChargeAccounts(SelectVal) {
    var accType = 0;
    if (SelectVal == "2" || SelectVal == "3") {
        if (SelectVal == "2") {
            accType = 6;
        }
        if (SelectVal == "3") {
            accType = 7;
        }
        FillAccount(SelectVal, accType);
    }
    else {
        FillAccount(SelectVal, 0);
    }
}

function SavePage() {
    var taxType = 0;
    //    if ($("input[id$=COA_NAME]").val().trim() == "" || $("input[id$=COA_NAME]").val().toLowerCase() == "select/type") {
    //        $("input[id$=TAX_ACCOUNT]").val("0");
    //    }



    if ($("input[id$=TAX_EXT]").is(":checked")) {
        taxType = 2;
        $("input[id$=EXT_TO]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(EnterTaxExtendTillDate)" }
        });

    }
    else if ($("input[id$=TAX_DISC]").is(":checked")) {
        taxType = 1;
        $("input[id$=DISC_FROM]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(EnterTaxDiscontinueDate)" }
        });
    }
    else
        taxType = 0;

    Addvalidation();
    if ($(document.forms[0]).valid()) {

        var jSonString = GrandScriptUtils.FormToJsonString("divData");
        //alert('Test = ' + jSonString);
        $.post(TaxSettings.SaveTaxParameters + taxType, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(TaxSettings.NameAlreadyExist, TaxSettings.Title);
            }
            else if (parseInt(data) > 0) {
                if ($("[id$=Type]").val() == "0") {
                    GrandScriptUtils.ShowModal(TaxSettings.POChargeSavedSuccess, TaxSettings.Title, TaxSettings.Saved, false);
                }
                else if ($("[id$=Type]").val() == "4") {
                    GrandScriptUtils.ShowModal(TaxSettings.DeductionSavedSuccess, TaxSettings.Title, TaxSettings.Saved, false);
                }
                else {
                    GrandScriptUtils.ShowModal(TaxSettings.SavedSuccessfully, TaxSettings.Title, TaxSettings.Saved, false);
                }
                ResetPage();
                BindGrid();
            }
            else if (data == -5) {
                GrandScriptUtils.ShowModal(TaxSettings.InvalidExpression, TaxSettings.Title, TaxSettings.InvalidExpression, false);

            }
            else {
                GrandScriptUtils.ShowModal(TaxSettings.ActionFailedMessage);
                ResetPage();
            }

        });
        return false;
    }
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req                             
        case TaxSettings.InvalidExpression:
            $("[id$=TAX_Formula]").focus();
            break;
        //Commend When calling                             
        case TaxSettings.SavedSuccessfully:
            PageInit();
            break;
        case TaxSettings.DeleteMessageCommand:
            DeleteDetails();
            break;
    }
    return false;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString()) {
        // To Delete Details                                 
        case TaxSettings.Delete:
            TaxID = GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TaxPK, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(TaxSettings.DeleteConfirmationMessage, TaxSettings.ConfirmationMessage, TaxSettings.DeleteMessageCommand, true);
            break;

        // To Edit Details                                          
        case TaxSettings.Edit:
            FillDetails(tr);
            break;

        default:
            alert(TaxSettings.DefaultAction);
            break;
    }
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(TaxSettings.DeleteTaxParameters + TaxID, function (data) {
        if (parseInt(data) == 1) {
            BindGrid();
            if ($("[id$=Type]").val() == "0") {
                msgtxt = TaxSettings.POChargeDeletedSuccess;
            }
            else if ($("[id$=Type]").val() == "4") {
                msgtxt = TaxSettings.DeductionDeletedSuccess;
            }
            else {
                msgtxt = TaxSettings.DeletedSuccessfully;
            }
        }
        else if (parseInt(data) == 0) {
            msgtxt = TaxSettings.AlreadyAssigned;
        }
        else {
            msgtxt = TaxSettings.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, TaxSettings.Title);
    });
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    Removevalidation();
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("#DIVTAX_DISC").hide();
    $("#DIVTAX_EXT").hide();
    $("input[id$=TAX_EXT]").attr("checked", false);
    $("input[id$=TAX_DISC]").attr("checked", false);
    //    $(document.forms[0]).find("input:not(input[type=submit],input[type=button])").each(function () {
    //        var idval = $(this).attr("id");
    //        if (!Checkstatus(idval)) {
    //            $(this).val("");
    //        }
    //    });
    $("[id$=TAX_HEAD]").val('');
    $("[id$=TAX_DESC]").val('');
    $("[id$=TAX_Formula]").val('');
    $("[id$=TAX_TO_DT]").val('');
    $("[id$=TAX_FROM_DT]").val('');
    $("[id$=TAX_DISP_NAME]").val('');

    $("select[id$=TAX_PARM]").val("0");
    $("select[id$=TAX_CATEGORY]").val("0");
    $("select[id$=TAX_SUB_CATEGORY]").val("0");
    $("input[id$=TAX_PK]").val("0");
    $("[id$=COA_NAME]").val('');
    $("[id$=TAX_ACCOUNT]").val('0');
    //setting search type.
    $("select[id$=SearchType]").val("0");
    $("[id$=SearchValue]").val("");

    //Default Tax status is Active
    //1 for Active
    //0 for Inactive
    $("select[id$=TAX_ACTIVE]").val("1");

    $("input[id$=TAX_IS_SALE]").attr("checked", false);
    $("input[id$=TAX_IS_PURCHASE]").attr("checked", false);
    $("input[id$=TAX_IS_RETURN]").attr("checked", false);
    $("[id$=TAX_CODE]").val('');
    $("[id$=TAX_RATE]").val('');
    $("[id$='hdfAccountPK']").val("-1");
    SetSearchType();
    //initializing search.
    SearchInit();
    return false;
}

function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page
    if (controlID.search("UserID") != -1) {
        return true;
    }
    if (controlID.search("TAX_PK") != -1) {
        return true;
    }
    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
    if (controlID.search("UserPK") != -1) {
        return true;
    }
    return false;
}

function DisplayFormula() {
    //<summary>function used to Display Formula</summary>
    if ($("[id$=TAX_RATE]").val() != '' && $("[id$=TAX_RATE]").val() != '.') {
        var rate = $("[id$=TAX_RATE]").val();
        $("[id$=TAX_Formula]").val("(#SUBTOTAL#)*(" + rate + "/100)");
    }
    else {
        $("[id$=TAX_Formula]").val('');
    }
}

//#endregion

///#region------ Validations ---------------
function Addvalidation() {

    $("[id$=TAX_CATEGORY]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectCategory)" }
    });
    $("input[id$=TAX_HEAD]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Enter Name" }
    });
    $("input[id$=TAX_CODE]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Enter Code" }
    });    
    if ($("[id$=Type]").val() != "4") {//Expression validation is not needed for Deduction 
        $("input[id$=TAX_Formula]").rules("add", {
            required: true,
            maxlength: 500,
            messages: { required: "Enter Formula" }
        });
    }
    $("input[id$=TAX_TO_DT]").rules("add", {
        required: true,
        //date: true,
        maxlength: 200,
        messages: { required: "Enter To Date" }
        //messages: { date: "Enter a valid Tax To Date" }
    });

    $("input[id$=TAX_FROM_DT]").rules("add", {
        required: true,
        //date:true,
        maxlength: 200,
        messages: { required: "Enter From Date" }
        //messages: { date: "Enter a valid Tax From Date" }
    });

    $("input[id$=TAX_RATE]").rules("add", {
//        DecimalDigits: 2,
//        CustomDecimal: true,
        ZeroDecimal: true,
        messages: { required: "Enter Tax Rate" }
       // maxlength: 8,
       // messages: { CustomDecimal: String.format("Translate(MsgValidDecimalNo)", 2) }
    });
    $("[id$=TAX_SUB_CATEGORY]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectSubCategory)" }
    });
    $("[id$=TAX_PARM]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectLevel)" }
    });
}

function Removevalidation() {
    $(document.forms[0]).validate().resetForm();
    $("[id$=TAX_CATEGORY]").rules("remove");
    $("input[id$=TAX_HEAD]").rules("remove");
    $("input[id$=TAX_CODE]").rules("remove");
    $("input[id$=TAX_Formula]").rules("remove");
    $("input[id$=TAX_TO_DT]").rules("remove");
    $("input[id$=TAX_FROM_DT]").rules("remove");
    $("input[id$=TAX_DISC]").rules("remove");
    $("input[id$=TAX_EXT]").rules("remove");
    $("input[id$=TAX_RATE]").rules("remove");
    $("[id$=TAX_SUB_CATEGORY]").rules("remove");
}

//#endregion


function AddAfterDateRange(hdfTo, fromDate, hdnFrmDate, format, restrictfromDate) {
    ///<summary>
    ///     Used for From Date and To Date datepicker
    ///</summary>
    /// <param name="fromDate" optional="true" type="String">
    ///     The input id of the fromdate datepicker
    /// </param>
    /// <param name="hdnFrmDate" optional="true" type="String">
    ///     The hidden field id used for the set min date of the todate datepicker
    /// </param>
    /// <param name="toDate" optional="true" type="String">
    ///     The input id of the todate datepicker
    /// </param>
    /// <param name="hdnToDate" optional="true" type="String">
    ///      The hidden field id used for the set max date of the fromdate datepicker
    /// </param>
    /// <param name="format" optional="true" type="String">
    ///      Format of the datepicker
    /// </param>
    /// <param name="restrictfromDate" optional="true" type="bool">
    ///      true used for set the it will not allow to select the  the current before date
    /// </param>
    if (!format)
        format = "dd-M-yy";
    $('input[id$=' + fromDate + ']').datepicker({
        dateFormat: format,
        changeMonth: true,
        changeYear: true,
        altField: $("[id$=" + hdnFrmDate + "]"),
        altFormat: "mm/dd/yy"

    });
    $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdfTo + "]").val()));
    if (restrictfromDate)
        $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());

}

function AddBetweenDateRange(hdnFrmDate, hdfToDate, btwDate, hdnBtwDate, format, restrictfromDate) {
    ///<summary>
    ///     Used for From Date and To Date datepicker
    ///</summary>
    /// <param name="hdnFrmDate" optional="true" type="String">
    ///     The hidden field id used for get From Date 
    /// </param>
    /// <param name="hdfToDate" optional="true" type="String">
    ///     The hidden field id used for get To Date 
    /// </param>
    /// <param name="btwDate" optional="true" type="String">
    ///     The input id of the Between datepicker
    /// </param>
    /// <param name="hdnbtwDate" optional="true" type="String">
    ///      The hidden field id used for the set Between Date 
    /// </param>
    /// <param name="format" optional="true" type="String">
    ///      Format of the datepicker
    /// </param>
    /// <param name="restrictfromDate" optional="true" type="bool">
    ///      true used for set the it will not allow to select the  the current before date
    /// </param>
    if (!format)
        format = "dd-M-yy";
    $('input[id$=' + btwDate + ']').datepicker({
        dateFormat: format,
        changeMonth: true,
        changeYear: true,
        altField: $("[id$=" + hdnBtwDate + "]"),
        altFormat: "mm/dd/yy"

    });
    $('input[id$=' + btwDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
    $('input[id$=' + btwDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdfToDate + "]").val()));
    if (restrictfromDate)
        $('input[id$=' + btwDate + ']').datepicker("option", "minDate", new Date());
}
