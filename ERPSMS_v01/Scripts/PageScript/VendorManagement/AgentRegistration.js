materialID/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region GlobalVariables
var vendorlID = 0;
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Vendor";
var tdset = "";
var addressID = 0;
var materialID = 0;
var sampleMaterialID = 0;
var tdset = "";
var tdsetSamples = "";
var bankID = 0;

//var FileJson = new Object(); 
///#endregion

///#region Configuration Section
var agentRegistration = {
    GetCountry: "CommonManagement.do?Action=GetCountryList",
    GetAccount: "CommonManagement.do?Action=GetAccount",
    GetVendorPOType: "AgentRegistration.do?Action=GetVendorPOType&BizUnit=",
    GetState: "CommonManagement.do?Action=GetStateList",
    GetTax: "TaxSettings.do?Action=GetTaxCategoryValue",
    GetCurrency: "CommonManagement.do?Action=GetCurrencyList&SBU=",
    GetMaterialCategoryTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryWithoutSemiAndFinished&SBUPk=",
    GetVendorTerms: "VendorTermsManagement.do?Action=GetVenderTermsList",
    SaveAgentDetails: "AgentRegistration.do?Action=SaveAgentDetails",
    SaveTaxDiscount: "AgentRegistration.do?Action=TaxDiscountApply",
    GetVndrTaxDiscountDetails: "AgentRegistration.do?Action=GetTaxDiscountDetails&ItemPk=",
    GetVendorRoles: "AgentRegistration.do?Action=GetRole&SBU=",
    DeleteVenMaterial: "AgentRegistration.do?Action=DeleteVndMaterial&MaterialID=",
    GetVendorMaterials: "AgentRegistration.do?Action=GetVndMaterials&AgentID=",
    SaveMaterial: "AgentRegistration.do?Action=SaveVenMaterial",
    GetCategoryTaxDiscountDateBase: "TaxSettings.do?Action=GetActiveCategoryDateValue&CategoryPK=",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListWithoutSemiAndFinished&SBUPk=",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&AUTOSEARCH=1",
    //    GetMaterialCategoryListExceptFG
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategory&AUTOSEARCH=1&SBUPk=",
    GetMaterialUOM: "MaterialManagement.do?Action=GetMaterialUOM",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    GetCategoryTaxDiscount: "TaxSettings.do?Action=GetActiveCategoryValue&CategoryPK=",
    AgentListing: "Agentlisting.aspx?Type=",
    Category: "&CategoryPK=1",
    SubCategory: "&SubCategoryPK=3",
    //New
    GetAddressTypeList: "VendorRegistration.do?Action=GetAddressTypeList&SBUPk=",
    //New End
    //NewMaterial Start
    GetRateHistory: "MaterialManagement.do?Action=GetRateHistory&SBUPk=",
    //New End
    SaveBank: "AgentRegistration.do?Action=SaveAgentBank",
    GetAccountTypes: "VendorRegistration.do?Action=GetAccountTypesBank",
    GetAgentBanks: "AgentRegistration.do?Action=GetAgentBanks&AgentID=",
    GetBankDetailsById: "AgentRegistration.do?Action=GetBankDetaislByIdAgent&AgentID=",
    DeleteAgentBank: "AgentRegistration.do?Action=DeleteAgentBank&P_VBD_PK=",


    //Constants
    ViewCommand: "VIEW",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    HistoryCommand: "HISTORY",
    SaveCommand: "SAVE",
    CodeExist: "CODEEXIST",
    Param: "&MatCagID=",
    DeleteAddress: "DELETEADDRESS",
    DeleteMaterial: "DELETEMATERIAL",
    SEMIFINISHEDGOOD: "SEMI FINISHED GOOD",
    FINISHEDGOOD: "FINISHED GOOD",
    ConcurrencyExist:"CONCURRENCYEXIST",
    //NewSamples Start
    DeleteSamples: "DELETESAMPLES",
    //New End
    TaxDelete: "TAXDELETE",
    TaxDeleteVendor: "TAXDELETEVENDOR",
    DeleteBank: "DELETEBANK",

    IVT_SL_NO: "IVT_SL_NO",
    IVT_TAX: "IVT_TAX",
    ITV_DISC_PERC: "ITV_DISC_PERC",
    ITV_TAX_PERC: "ITV_TAX_PERC",
    ITV_SL_NO: "ITV_SL_NO",
    CfgType: "PURCHASE TYPE", 

    //Properties
    ItemTaxPK: 0,
    EditTax: 0,
    VendorTaxPK: 0,
    TypeID: "TypeID",

    //Messages
    AgentCodeExist: "Translate(AgentCodeExist)",
    AgentSaveMessage: "Translate(AgentSavedMessage)",
    AgentSubmitMessage: "Translate(AgentSubmitMessage)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    Information: "Translate(Information)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    TypeAlreadyAdded: "Translate(TypeAlreadyAdded)",
    AgentConcurrencyMsg: "Translate(AlreadyUpdatedRecord)",
    AlreadyDeletedMsg: "Translate(AlreadyDeletedRecord)"
}
///#endregion

var QtyDec, AmtDec, RateDec;

//#region initialization Section
$.validator.addMethod('selectNone', function (value, element) {
    ///<summary>
    ///Add additional validation for select
    ///</summary>
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$.validator.addMethod("TwoDecimal", function (value) {
    return /^\d{1,2}(\.\d{1,2})?$/.test(value);
}, "Max 2 numeric and 2 decimals allowed");

$(document).ready(function () {
    ///<summary>
    ///Document Ready function Aftre page initialization
    ///</summary>
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

    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimal']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimal']").val();
    var pageURL = window.document.URL;   
    PageInit(); //Page Initial condtions
});

function PageInit() {
    ///<summary>
    ///Used For PageInit
    ///</summary>
    WindowExpand(true); //Hide Side Menu
    $('#VEN_ADDR3').hide();
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("=");
            //Role Id Setting to Hiddenfield
            if (pK[1] != "" && pK[0] == "Type") {
                $("[id$=VRM_ROLE]").val(pK[1]);
                agentRegistration.TypeID = pK[1];
            }

            if (pK[1] != "" && pK[0] == "Status" || (pK[1] != "" && pK[0] == "RefID")) {
                if (pK[0] == "RefID") {                    
                    $("[id$=divdummySamples]").show();
                  
                }
                if (pK[1] == "1") {
                    $("[id$=btnSave]").hide();
                    $("[id$=imbSave]").hide();
                    $("[id$=ViewStatus]").val("1");
                    $("[id$=AddressSave]").hide();
                    $("[id$=divdummyMaterial]").hide();
                    $("[id$=btnBack]").hide();                  
                    $("[id$=divdummySamples]").hide();
                    $("[id$=imbSamplesAdd]").hide();
                    $("textarea[id='ctl00_MainContent_VEN_ADDR3']").remove();
                    $('#VEN_ADDR3').show();
                }
                if (pK[1] == "2") {
                    $("[id$=btnSave]").show();
                    $("[id$=imbSave]").hide();
                }
            }
        }
    }
   
    $("[id$=tabs]").tabs(); // Create Tabs
    vendorJson = $.parseJSON($("[id$=VendorDetails]").val());
    $("#divVendorData").data("VendorData", vendorJson);
    $("#divCategory").dialog({ autoOpen: false });
    FillAddressType(0);
    DateInit();
    FillTax(0);
    Popup();
    if (vendorJson != null) {
        if (vendorJson.VEN_PK == undefined || vendorJson.VEN_PK == 0) {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", true);
            var dummyObj = new Object();
            FillDropDowns();
            $("[id$=VEN_CURRENCY_TEXT]").html("");
            $("[id$=VEN_CNTRY_TEXT]").html("");
            $("[id$=VEN_STATE_TEXT]").html("");
            $("[id$=VEN_ACCOUNT_TEXT]").html("");
            $("[id$=VNC_TYPE_TEXT]").html("");
            $("[id$=VEN_CURRENCY_TEXT]").hide("");
            $("[id$=VEN_CNTRY_TEXT]").hide("");
            $("[id$=VEN_STATE_TEXT]").hide("");
            $("[id$=VEN_ACCOUNT_TEXT]").hide("");
            $("[id$=VNC_TYPE_TEXT]").hide("");
            $(".ddlSelect").show();
        }
        else {
            FillVendorDetails(vendorJson);
        }
    }
    else {
        GrandScriptUtils.ShowModal(agentRegistration.AgentConcurrencyMsg, agentRegistration.Information, agentRegistration.ConcurrencyExist);
    }
    $("input[id$=UserID]").val($("input[id$=UserPk]").val());
    $("input[id$=VEN_CODE]").focus();
    $("select[id$=VEN_TYPE]").attr("disabled", true);
    if ($("[id$=ViewStatus]").val() == "1") {
        GrandScriptUtils.ChangeMode("FormData");
        DisableControlls();
    }
    $("[id$=imbaddnew]").show();

    bindBankTab();
    BindGridBanks();
    return false;
}
function FillTax(taxID) {
    var drpID = $("select[id$=VEN_WHT_TAX]").attr("id");
    $.get(agentRegistration.GetTax + agentRegistration.Category + agentRegistration.SubCategory, function (data) {
        if (taxID) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, taxID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });

}
function FillCategoryMaterials(catgID) {
    ///<summary>Function Used to Fill material based on the category  </summary>
    $("[id$=MaterialItem]").val("");
    $("[id$=ITV_ITEM]").val(0);
   
    GrandScriptUtils.MakeAutoCompleteLimitLen("MaterialItem", agentRegistration.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + catgID, "ITV_ITEM", true, false, "MaterialType", true, "Store", "", "", $("[id$=AutoStartValue]").val());
}
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "MaterialItem") {
        FillMaterialDetails($("[id$=ITV_ITEM]").val());
    }
}

function FillRoleTreeView(venPk) {
    //<summary>Function Used to Fill Menu Details to Tree View </summary>
    var sbuPK = $("[id$=BizUnitPk]").val();
    SetTreeHeaderStructure("trvRoleMap", agentRegistration.GetVendorRoles + sbuPK + "&venPK=" + venPk + "&MapParentID=0", "Role", true, false, "0", true);    // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

function GetSelectedMaterial() {
    //<summary>Function Used to get the all checked dept details </summary>
    var MaterialArray = new Array();
    var sbuPK = $("[id$=BizUnitPk]").val();
    var store = $("[id$=Store]").val();
    var userPK = $("[id$=UserPk]").val();
    var materialMapping
    var materialPK = 0;
    $("#trvMaterialMap").find("input[type=checkbox]:checked").each(function () {
        materialPK = $(this).attr("id");
        if ($(this).next().next("input[type=hidden]").val() == "true") {
            materialPK = materialPK.substr(materialPK.lastIndexOf("_") + 1, materialPK.length);
            MaterialArray.push({ IDM_ITEM: materialPK, IDM_DEPT: store, IDM_MOD_BY: userPK, IDM_BIZUNIT: sbuPK });
        }
    });
    return MaterialArray;
}

function GetSelectedRoles() {
    //<summary>Function Used to get the all checked dept details </summary>
    var ObjVendorRole = new Array();
    var obj = new Object();
    $("#trvRoleMap").find("input[type=checkbox]:checked").each(function () {
        rolePK = $(this).attr("id");
        obj = new Object();
        rolePK = rolePK.substr(rolePK.lastIndexOf("_") + 1, rolePK.length);
        obj.VRM_ROLE = rolePK;
        ObjVendorRole.push(obj);
    });
    return ObjVendorRole;
}

function GetVendorTerms() {
    var ObjVendor = $("#divVendorData").data("VendorData");
    var obj = new Object();
    var flag = false;
    $("#grdTermsDetails").find("tr:has(td)").each(function () {
        var grdcbx = $(this).find("td:first input");
        var grdinput = $(this).find("td:last input");
        var TermID = GrandGrid.Utilities.GetColumnValue(this, agentRegistration.VendorTermsID, $(this).parent().attr("id"));
        if ($(grdcbx).attr("checked")) {
            obj = new Object();
            flag = false;
            for (var i in ObjVendor.TermsDetails) {
                if (TermID == ObjVendor.TermsDetails[i].VTD_VENDER_TERM) {
                    flag = true;
                    obj = ObjVendor.TermsDetails[i];
                }
            }
            if (flag)
                obj.VTD_VALUE = $(grdinput).val();
            else {
                obj.VTD_VENDER_TERM = TermID;
                obj.VTD_VALUE = $(grdinput).val();
                ObjVendor.TermsDetails.push(obj);
            }
        }
        else {
            for (var i in ObjVendor.TermsDetails) {
                if (ObjVendor.TermsDetails[i].VTD_VENDER_TERM == TermID) {
                    ObjVendor.TermsDetails.splice(i, 1);
                    break;
                }
            }
        }
    });
    $("#divVendorData").data("VendorData", ObjVendor);
    return false;
}


//NewSample Start
function DateInit() {
    //<summary>function used to make datepicker</summary>
    GrandScriptUtils.DatePicker("ISV_RECEIVED_DATE", false, false);
    GrandScriptUtils.DatePicker("VEN_GST_NO_MOD_DT", false, false);
}
//New End

function MakeNumeric(event) {
    ///<summary>function used to make text box Numeric only</summary>
    /// <param name= "event"  type="Object">
    /// Object Used to get the Key Pressed
    /// </param>
    if (!(event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57 || event.keyCode == 190)) {
        event.returnValue = false;
    }
}
//Allow only numbers to be typed in a textbox 
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function FillVendorDetails(vendorObject) {
    ///<summary>function used to fill vendor details corresponding to vendor id </summary>
    /// <param name="vendorObject"  type="Object">
    /// vendor object fetched corresponding to AgentID
    /// </param>
    var objArray;
    if (parseInt(vendorObject.VEN_STATUS) == 0 || parseInt(vendorObject.VEN_STATUS) == 6) {
        if ($("[id$=ViewStatus]").val() != "1") {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", true);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", false);
        }
    }
    else {
        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", false); 0
    }
    $("[id$=VEN_NAME]").val(vendorObject.VEN_NAME);
    $("[id$=VEN_CODE]").val(vendorObject.VEN_CODE);
    $("[id$=VEN_CONT_NAME]").val(vendorObject.VEN_CONT_NAME);
    $("[id$=VEN_TIN]").val(vendorObject.VEN_TIN);
    $("[id$=VEN_PHONE]").val(vendorObject.VEN_PHONE);
    $("[id$=VEN_ADDR1]").val(vendorObject.VEN_ADDR1);
    $("[id$=VEN_PIN]").val(vendorObject.VEN_PIN);
    $("[id$=VEN_ADDR2]").val(vendorObject.VEN_ADDR2);
    $("[id$=VEN_EMAIL]").val(vendorObject.VEN_EMAIL);
    $("[id$=VEN_MOBIL]").val(vendorObject.VEN_MOBIL);
    $("[id$=VEN_CITY]").val(vendorObject.VEN_CITY);
    $("[id$=VEN_FAX]").val(vendorObject.VEN_FAX);
    $("[id$=VEN_NAME2]").val(vendorObject.VEN_NAME2);
    $("[id$=VEN_ADDR3]").val(vendorObject.VEN_ADDR3);
    $('#VEN_ADDR3').html(vendorObject.VEN_ADDR3);
    $("[id$=VEN_REG_NO]").val(vendorObject.VEN_REG_NO);
    $("[id$=VEN_GST_NO_MOD_DT]").val('');
    if (vendorObject.VEN_GST_NO_MOD_DT) {  // != 'undefined')
        $("[id$=VEN_GST_NO_MOD_DT]").val(convertDate(vendorObject.VEN_GST_NO_MOD_DT));
    }
    $("[id$=VEN_WEBSITE]").val(vendorObject.VEN_WEBSITE);

    $("[id$=LAST_MOD_DATE]").val(vendorObject.LAST_MOD_DATE);
    
   
    //To prevent 0 to annual sale text, otherwise generate validation error occure
    var varCreditDays = parseFloat(vendorObject.VEN_CREDIT_DAYS).toFixed(3);
    varCreditDays = parseFloat(varCreditDays);
    if (varCreditDays != 0)
        $("[id$=VEN_CREDIT_DAYS]").val(varCreditDays);

    var varAnnualValue = parseFloat(vendorObject.VEN_ANNUAL_SALES).toFixed(3);
    varAnnualValue = parseFloat(varAnnualValue);
    if (varAnnualValue != 0)
        $("[id$=VEN_ANNUAL_SALES]").val(varAnnualValue);
    $("[id$=VEN_WAREHOUSE_DTL]").val(vendorObject.VEN_WAREHOUSE_DTL);
    $("[id$=VEN_MANAGER]").val(vendorObject.VEN_MANAGER);
    $("[id$=VEN_HAS_ISO]").val(vendorObject.VEN_HAS_ISO);
    if (vendorObject.VEN_HAS_ISO == "True" || vendorObject.VEN_HAS_ISO == "1") {
        $("[id$=VEN_HAS_ISO1]").attr("checked", true);
    } else {
        $("[id$=VEN_HAS_ISO1]").attr("checked", false);
    }

    $("[id$=VEN_PAY_FOR_VENDOR]").val(vendorObject.VEN_PAY_FOR_VENDOR);
    if (vendorObject.VEN_PAY_FOR_VENDOR == "True" || vendorObject.VEN_PAY_FOR_VENDOR == "1") {
        $("[id$=VEN_PAY_FOR_VENDOR1]").attr("checked", true);
        $("[id$=VEN_PAY_FOR_VENDOR_TEXT]").attr("checked", true);
    } else {
        $("[id$=VEN_PAY_FOR_VENDOR1]").attr("checked", false);
        $("[id$=VEN_PAY_FOR_VENDOR_TEXT]").attr("checked", false);
    }

    //Add VEN_ACTIVE
    $("[id$=VEN_ACTIVE]").val(vendorObject.VEN_ACTIVE);
    if (vendorObject.VEN_ACTIVE == "True" || vendorObject.VEN_ACTIVE == "1") {
        $("[id$=VEN_ACTIVE1]").attr("checked", true);
        $("[id$=VEN_ACTIVE]").attr("checked", true);
    } else {
        $("[id$=VEN_ACTIVE1]").attr("checked", false);
        $("[id$=VEN_ACTIVE]").attr("checked", false);
    }

    FillAccount(vendorObject.VEN_ACCOUNT);
    //Fill the country drop down and selecting the Country already added
    FillCountry("VENDOR", vendorObject.VEN_CNTRY);
    FillCountry("VBD_COUNTRY", vendorObject.VEN_CNTRY);
    $("[id$=VEN_TYPE]").val(vendorObject.VEN_TYPE);
    if ($("select[id$=VEN_CNTRY]").val() != "0")
        FillState("VENDOR", vendorObject.VEN_STATE, vendorObject.VEN_CNTRY);
    else
        $("select[id$=VEN_STATE]").find("option").remove();
   // FillType(vendorObject.VEN_PO_TYPE);
    FillCurrency(vendorObject.VEN_CURRENCY);
    $("[id$=VEN_PK]").val(vendorObject.VEN_PK);
    FillRoleTreeView(vendorObject.VEN_PK);
    FillCountry("ADDRESS");
    FillTax(vendorObject.VEN_WHT_TAX);

    FillAddressType(vendorObject.VNC_TYPE);
    $("[id$=VNC_TYPE_NAME]").val(vendorObject.VNC_TYPE_NAME);
 
    if (!($.isArray(vendorObject.TaxHdr))) {

        if (vendorObject.TaxHdr != undefined) {
            objArray = vendorObject.TaxHdr;
            vendorObject.TaxHdr = new Array();
            vendorObject.TaxHdr.push(objArray);

            var ObjVendor = $("#divVendorData").data("VendorData");
            var ObjTax = new Object();
            ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
            ObjTax.USER_PK = ObjVendor.UserPk;
            ObjTax.ITV_PK = materialID;
            var category = parseInt($("[id$=hdfTaxCategory]").val());
            ObjTax.ITV_TAX_CATEGORY = category;
            ObjTax.TaxDetail = vendorObject.TaxHdr;

            $("#divTaxDataVendor").data("TaxObj", ObjTax);
            var v = JSON.stringify(ObjTax.TaxDetail);
            $("[id$=TaxHdr]").val(v);
            // Bind Vendor tax details to textbox 'TaxByVendor' 'IVT_TAX_TEXT'
            var taxText = '';
            for (i = 0; i < ObjTax.TaxDetail.length; i++) {
                taxText += ObjTax.TaxDetail[i].IVT_TAX_TEXT + ", ";
            }
            taxText = taxText.substring(0, taxText.length - 2);
            $("[id$=TaxByVendor]").attr('title', taxText);
            if (taxText.length > 35) {
                taxText = taxText.substring(0, 35);
                taxText += '...';
            }
            $("[id$=TaxByVendor]").text(taxText);
            if (taxText != '') {
                $("[id$=hdfIsVendorTax]").val('1');
            }
        }
        else {
            objArray = vendorObject.TaxHdr;
            vendorObject.TaxHdr = new Array();
            $("[id$=hdfIsVendorTax]").val('0');
        }
    }
    else {
        var ObjVendor = $("#divVendorData").data("VendorData");
        var ObjTax = new Object();
        ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
        ObjTax.USER_PK = ObjVendor.UserPk;
        ObjTax.ITV_PK = materialID;
        var category = parseInt($("[id$=hdfTaxCategory]").val());
        ObjTax.ITV_TAX_CATEGORY = category;
        ObjTax.TaxDetail = vendorObject.TaxHdr;

        $("#divTaxDataVendor").data("TaxObj", ObjTax);
        var v = JSON.stringify(ObjTax.TaxDetail);
        $("[id$=TaxHdr]").val(v);

        // Bind Vendor tax details to textbox 'TaxByVendor' 'IVT_TAX_TEXT'
        var taxText = '';
        for (i = 0; i < ObjTax.TaxDetail.length; i++) {
            taxText += ObjTax.TaxDetail[i].IVT_TAX_TEXT + ", ";
        }
        taxText = taxText.substring(0, taxText.length - 2);
        $("[id$=TaxByVendor]").attr('title', taxText);
        if (taxText.length > 35) {
            taxText = taxText.substring(0, 35);
            taxText += '...';
        }
        $("[id$=TaxByVendor]").text(taxText);
        $("[id$=hdfIsVendorTax]").val('1')
    }
   
    if ($("[id$=ViewStatus]").val() == "1") {
        $("[id$=VEN_STATE]").remove();
       // ShowVendor();
        $("[id$=venCurrText]").show();
        $("[id$=venCurr]").hide();
        $("[id$=divTaxText]").show();
        $("[id$=divTaxddl]").hide();
        $("[id$=divTypeText]").hide();
        $("[id$=divTypeddl]").hide();
        $("[id$=divAccountText]").show();
        $("[id$=divAccountddl]").hide();
        $("[id$=VEN_CNTRY]").remove();
        $("[id$=VEN_CURRENCY_TEXT]").html(vendorObject.VEN_CURRENCY_TEXT);
        $("[id$=VEN_CNTRY_TEXT]").html(vendorObject.VEN_CNTRY_TEXT);
        $("[id$=VEN_STATE_TEXT]").html(vendorObject.VEN_STATE_TEXT);
        $("[id$=VEN_ACCOUNT]").remove();
        $("[id$=VNC_TYPE]").remove();
        $("[id$=VEN_ACCOUNT_TEXT]").html(vendorObject.VEN_ACCOUNT_TEXT);
        $("[id$=VNC_TYPE_TEXT]").html(vendorObject.VNC_TYPE_TEXT);
        $(".ddlSelect").hide();


    }
    else {
        $("[id$=divAccountText]").hide();
        $("[id$=divAccountddl]").show();
        $("[id$=divTypeText]").hide();
       // $("[id$=divTypeddl]").show();
        $("[id$=venCurrText]").hide();
        $("[id$=venCurr]").show();
        $("[id$=divTaxText]").hide();
        $("[id$=divTaxddl]").show();
        $("[id$=VEN_CURRENCY_TEXT]").html("");
        $("[id$=VEN_CNTRY_TEXT]").html("");
        $("[id$=VEN_STATE_TEXT]").html("");
        $("[id$=VEN_ACCOUNT_TEXT]").html("");
        $("[id$=VNC_TYPE_TEXT]").html("");
        $("[id$=VEN_CURRENCY_TEXT]").hide("");
        $("[id$=VEN_CNTRY_TEXT]").hide("");
        $("[id$=VEN_STATE_TEXT]").hide("");
        $("[id$=VEN_ACCOUNT_TEXT]").hide("");
        $("[id$=VNC_TYPE_TEXT]").hide("");
        $(".ddlSelect").show();
    }
    if (vendorObject.COA_WHT_TAX_TEXT != undefined) {
        $("[id$=VEN_WHT_TAX_TEXT]").html(vendorObject.COA_WHT_TAX_TEXT);
    }  

  
    checkHideMaterialTaxPopUpImage();
    checkHideVendorTaxPopUpImage();
}

function BindGrid() {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = agentRegistration.GetVendorMaterials + $("[id$=hdfVendorPK]").val()
    $("#grdVendorMaterialDetails").removeAttr("ajaxurl")
    $("#grdVendorMaterialDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdVendorMaterialDetails");
    GrandGrid.MakeGrid($("#grdVendorMaterialDetails"));
    checkHideMaterialTaxPopUpImage();
    return false;
}

// Method to Change Item Currency as vendor Currency
function ChangeCurrency() {
    $("select[id$=ITV_CURRENCY]").val($("select[id$=VEN_CURRENCY]").val());
    $("select[id$=ITV_CURRENCY]").attr("disabled", true);
   // UpdateMaterialMappingList();
}

/// Method to Update Material Mapping List Currency Based on Vendor Currency
function UpdateMaterialMappingList() {
    var ObjVendor = $("#divVendorData").data("VendorData");
    if (ObjVendor.MaterialDetails.length > 0) {
        for (var index in ObjVendor.MaterialDetails) {
            ObjVendor.MaterialDetails[index].ITV_CURRENCY = $("select[id$=ITV_CURRENCY]").val();
            ObjVendor.MaterialDetails[index].MaterialCurrencyText = $("select[id$=ITV_CURRENCY] option:selected").text();
        }
        $("#divVendorData").data("VendorData", ObjVendor);
        GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, ObjVendor.MaterialDetails);
    }
}

//To Fill File Details To Grid
function FillFileDetails() {
    ///<summary>To Fill File Details And dispaly as Listing With Delete Option</summary>
    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}

function FillDropDowns() {
    ///<summary>
    ///Used for FillDropDowns
    ///</summary>
    FillCountry();
    FillCurrency();
    // FillType();
    FillAccount(0);  

}


function FillAccount(SelectVal) {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    var drpID = $("select[id$=VEN_ACCOUNT]").attr("id");
    var url = agentRegistration.GetAccount + "&Active=1" + "&SubType=1" + "&IsGroup=0";
    $.get(url, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
    });
}

function FillCountry(controlID, countryID) {
    ///<summary>function used to fill vendor details corresponding to vendor id </summary>
    /// <param name="controlID"  type="Object">
    /// Determins which tab Address or vendor
    /// </param>
    /// <param name="countryID"  type="Object">
    /// Country Id to Select after filling Drop down
    /// </param>
    var drpID = $("select[id$=VEN_CNTRY]").attr("id");
    var AddressdrpID = $("select[id$=VNC_CNTRY]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(agentRegistration.GetCountry, function (data) {
        if (controlID == "ADDRESS") {
            GrandScriptUtils.FillDropDown(AddressdrpID, data, true, true, countryID);
        }
        else if (controlID == "VENDOR") {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, countryID);
        }
        else if (controlID == "VBD_COUNTRY") {
            var id = $("select[id$=VBD_COUNTRY]").attr("id");
            GrandScriptUtils.FillDropDown(id, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
            GrandScriptUtils.FillDropDown(AddressdrpID, data, true, true);
            var id = $("select[id$=VBD_COUNTRY]").attr("id");
            GrandScriptUtils.FillDropDown(id, data, true, true);
        }
    });
}

function FillState(controlID, stateID, countryID) {
    ////<summary>function used to fill State </summary>
    /// <param name="controlID"  type="Object">
    /// Determins which tab Address or vendor
    /// </param>
    /// <param name="stateID"  type="Object">
    /// State Id to Select after filling Drop down
    /// </param>
    /// <param name="countryID"  type="Object">
    /// Country Id to Fill State Corresponding to Country
    /// </param>
    var drpID;
    var ajaxurl;
    if (controlID == "VENDOR") {
        drpID = $("select[id$=VEN_STATE]").attr("id");
    }
    else if (controlID == "ADDRESS") {
        drpID = $("select[id$=VNC_STATE]").attr("id");
    }
    if (countryID)
        ajaxurl = agentRegistration.GetState + "&CountryID=" + countryID;
    else
        ajaxurl = agentRegistration.GetState + "&CountryID=" + $("select[id$=VEN_CNTRY]").val();

    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(ajaxurl, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, stateID);
    });
    return false;
}

function FillCurrency(currencyID) {
    ///<summary>
    ///Used for FillCurrency
    ///</summary>
    // Get id of the Country DropDown
    var drpID = $("select[id$=VEN_CURRENCY]").attr("id");
    var drpAddressCurrency = $("select[id$=Currency]").attr("id");
    var drpmaterialCurrency = $("select[id$=ITV_CURRENCY]").attr("id");
    $.get(agentRegistration.GetCurrency + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, currencyID);
        GrandScriptUtils.FillDropDown(drpAddressCurrency, data, true, true);
        GrandScriptUtils.FillDropDown(drpmaterialCurrency, data, true, true);
    });
}



function TabChange(tabname) {
    if (tabname == "material") {
        $("select[id$=ITV_CURRENCY]").val($("select[id$=VEN_CURRENCY]").val());
        $("select[id$=ITV_CURRENCY]").attr("disabled", true);
        $("select[id$=MaterialType]").focus();
        $("[id$='ITV_MOQ']").val("");
        //$("[id$='ITV_MOQ']").val(parseFloat("0").toFixed(QtyDec));
    }
}

function ResetPage() {
    $("[id$=hdfVendorPK]").val('0')
    window.location = agentRegistration.AgentListing + agentRegistration.TypeID;
    return false;
}

function ShowVendor() {
    //<summary>Function Used to Show Vendor Panel </summary>
    $("#imgVendorHide").show();
    $("#imgVendorShow").hide();
    $("#VendorSelection").show();
}

function HideVendor() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgVendorHide").hide();
    $("#imgVendorShow").show();
    $("#VendorSelection").hide();
}
//#endregion

///#region Core Section
//#region *************************************************************AddressBook Management**********************************************************
function SaveAddress() {
    ///<summary>
    ///Used for SaveAddress
    ///</summary>
    AddValidations(2)
    if ($(document.forms[0]).valid()) {
        $("[id$=VNC_DEFAULT]").attr("disabled", false);
        var ObjVendor = $("#divVendorData").data("VendorData");
        var editAddress = $("input[id$=EditAddress]").val();
        var obj = new Object();
       
        var guid = GrandScriptUtils.GenerateGuid();
        obj.VNC_NAME = $("[id$=VNC_NAME]").val();
        obj.VNC_CONT_NAME = $("[id$=VNC_CONT_NAME]").val();
        obj.VNC_ADDR1 = $("[id$=VNC_ADDR1]").val();
        obj.VNC_ADDR2 = $("[id$=VNC_ADDR2]").val();
        obj.VNC_EMAIL = $("[id$=VNC_EMAIL]").val();
        obj.VNC_PHONE = $("[id$=VNC_PHONE]").val();
        obj.VNC_DEFAULT = ($("[id$=VNC_DEFAULT]").attr("checked")) ? "True" : "False";
        obj.VNC_CITY = $("[id$=VNC_CITY]").val();
        obj.VNC_CNTRY = $("[id$=VNC_CNTRY]").val();
        obj.VNC_STATE = $("[id$=VNC_STATE]").val();
        obj.VNC_MOBIL = $("[id$=VNC_MOBIL]").val();
        obj.VNC_FAX = $("[id$=VNC_FAX]").val();
        obj.VNC_TAX_NO = $("[id$=VNC_TAX_NO]").val();
        obj.VNC_TYPE = $("[id$=VNC_TYPE]").val();
        obj.VNC_TYPE_TEXT = $("[id$=VNC_TYPE] option:selected").html() == "--Select--" ? "" : $("[id$=VNC_TYPE] option:selected").html();
        obj.VNC_NAME2 = $("[id$=VNC_NAME2]").val();
        obj.VNC_ADDR3 = $("[id$=VNC_ADDR3]").val();

        //TYPE_ID
        obj.VNC_TYPE_NAME = $("[id$=VNC_TYPE_NAME]").val();

        $("#divVendorData").data("VendorData", ObjVendor);
       
    }
    return false;
}

function GridAddressHandler(tr, command) {
    switch (command.toString().toUpperCase()) {
        case agentRegistration.DeleteCommand:
            addressID = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.AddressID, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(agentRegistration.DeleteConfirmationMessage, agentRegistration.ConfirmationMessage, agentRegistration.DeleteAddress, true);
            //DeleteAddress(tr);
            return false;
            break;
        case agentRegistration.EditCommand:
            FillAddress(tr);
            return false;
            break;
        case agentRegistration.ViewCommand:
            FillAddress(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

//New
function FillAddressType(catgID) {
    //<summary>Function used to fill Paremeter details for tax </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=VNC_TYPE]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    $.get(agentRegistration.GetAddressTypeList + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, catgID);
    });


}
//new End
//#endregion

//#region*****************************************************MaterialManagement**************************************************************
function FillMaterialCategory() {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    $.get(agentRegistration.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
        for (var i in data) {//checking data contains finished good or semifinished good.if exist removing that items from dropdown.
            if ((data[i].Text == agentRegistration.SEMIFINISHEDGOOD) || (data[i].Text == agentRegistration.FINISHEDGOOD)) {
                data.splice(i, 1);
            }
        }
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}



//NewSamples Start
function FillMaterialSamplesCategory() {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var SamplesdrpID = $("select[id$=MaterialTypeSamples]").attr("id");
    $.get(agentRegistration.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
        for (var i in data) {//checking data contains finished good or semifinished good.if exist removing that items from dropdown.
            if ((data[i].Text == agentRegistration.SEMIFINISHEDGOOD) || (data[i].Text == agentRegistration.FINISHEDGOOD)) {
                data.splice(i, 1);
            }
        }
        GrandScriptUtils.FillDropDown(SamplesdrpID, data, true, true);
    });
}
//New End

function ShowCategory() {
    ///<summary>Function used call the tree Data For filling the Material Category </summary>
    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Category", false, "700", false, false);
    return false;
}

function FillUOM(categoryID, selectval) {
    ///<summary>function To Fill Uom Details </summary>
    /// <param name="categoryID"  type="string">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    /// <param name="selectval"  type="string">
    ///     Specific value to be selected.
    /// </param>
    if (categoryID != 0) {
        $("select[id$=ITV_MOQ_UOM]").removeData();
        var drpID = $("select[id$=ITV_MOQ_UOM]").attr("id");
        $.get(agentRegistration.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val() + agentRegistration.Param + categoryID, function (data) {
            if (selectval) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else
        $("select[id$=MaterialUOM]").find("option").remove();
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
    FillUOM(cagID, false)
    //Filling the material Drop down after selecting the material category
    FillCategoryMaterials(cagID);
    //closing modalbox after selected from treeview.
    $("#divCategory").dialog("destroy");
    $("#divCategory").dialog({ autoOpen: false });
}

function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    if (categoryID != "0") {
        FillUOM(categoryID, false);
        FillCategoryMaterials(categoryID);
    }
    else {
        ClearMaterials();
    }
}

//NewSamples Start
function FillCategorySamplesDetails(categoryID) {
    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    if (categoryID != "0") {
        FillCategorySampleMaterials(categoryID);
    }
    else {
        ClearSamples();
    }
}
//New End

function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    SetTreeHeaderStructure("trvCategory", agentRegistration.GetMaterialCategoryTreeURL + $("[id$=BizUnitPk]").val() + "&MatCagID=", "Root", false, false, "0", false);  // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

function GridMaterialAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString().toUpperCase()) {
        case agentRegistration.DeleteCommand: //GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.ITV_ITEM, tableID)
            materialID = GrandGrid.Utilities.GetColumnValue(tr, "ITV_PK", $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(agentRegistration.DeleteConfirmationMessage, agentRegistration.ConfirmationMessage, agentRegistration.DeleteMaterial, true);
            //DeleteMaterial(tr);
            return false;
            break;
        //NewMAterial Start                                  
        case agentRegistration.HistoryCommand:
            materialID = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.ITV_ITEM, $(tr).parents("table:first").attr("id"));
            //$("[id$=VEN_PK]").val(vendorObject.VEN_PK);
            if ($("[id$=VEN_PK]").val() != "0") {
                var vendorPK = "0";
                var vendorPK = $("[id$=VEN_PK]").val(); //GetRateHistory: "MaterialManagement.do?Action=GetRateHistory&SBUPk=",
                ShowRateHistory(materialID, vendorPK);
            }
            return false;
            break;
        //New End                                  
        case agentRegistration.EditCommand:
            materialID = GrandGrid.Utilities.GetColumnValue(tr, "ITV_PK", $(tr).parents("table:first").attr("id"));
            FillMaterial(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

//NewSamples Start
function GridSamplesAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString().toUpperCase()) {
        case agentRegistration.DeleteCommand:
            sampleMaterialID = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.ISV_ITEM, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(agentRegistration.DeleteConfirmationMessage, agentRegistration.ConfirmationMessage, agentRegistration.DeleteSamples, true);
            return false;
            break;
        case agentRegistration.EditCommand:
            FillSamples(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}
//New End

function AfterGridBind(gridID) {
    var colIndex = 0;
    if (gridID == $("#grdTermsDetails").attr("id")) {
        var count = 1;
        var ObjVendor = $("#divVendorData").data("VendorData");
        //$("#grdTermsDetails").find("th:eq(0)").css("width","10px");
        $("#grdTermsDetails").find("th:eq(4) span").text("Details");
        $("#grdTermsDetails").find("tr:has(td)").each(function () {
            var tableID = $(this).parents("table:first").attr("id");
            var TermsType = GrandGrid.Utilities.GetColumnValue(this, agentRegistration.VendorTermsType, tableID)
            var TermsID = GrandGrid.Utilities.GetColumnValue(this, agentRegistration.VendorTermsID, tableID)
            var grdcbx = $(this).find("td:first input");
            var grdinput = $(this).find("td:last input");
            if ($("[id$=ViewStatus]").val() == "1") {
                $(grdinput).attr("disabled", true);
                $(grdcbx).attr("disabled", true);
            }

            grdinput.attr("id", "inputype" + count)
            if (TermsType == "1") {
                //$(grdinput).removeAttr("onkeypress")
                GrandScriptUtils.DatePicker(grdinput.attr("id"), false, false);
                $(grdinput).val('');
                $(grdinput).attr("onkeydown", "javascript:return false;");
            }
            else if (TermsType == "3") {
                $(grdinput).attr("onkeydown", "javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);");
            }

            if (TermsType == "2") {
                $(grdinput).removeAttr("onkeypress");
            }
            //Loop through vendor terms and assigning the terms value while editing

            for (var i in ObjVendor.TermsDetails) {
                if (ObjVendor.TermsDetails.length >= 1) {
                    if (TermsID == ObjVendor.TermsDetails[i].VTD_VENDER_TERM) {
                        $(grdinput).val(ObjVendor.TermsDetails[i].VTD_VALUE);
                        $(grdcbx).attr("checked", true);
                    }
                }
            }
            count++;
        });
    }

    if (gridID == $("#grdVendorMaterialDetails").attr("id")) {
        var amt = 0;
        var qty = 0;
        var strVal = "";
        var itemCount = 0;
        var active = "Yes";

        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#materialInsert").find("tr:eq(1)");
        }
        //Mode iS view
        if ($("[id$=ViewStatus]").val() == "1") {
            $("#divdummyMaterial").hide();
            //Hiding the template field
            $("#grdVendorMaterialDetails").find("tr").each(function () {
                //                $(this).find("td:last,th:last").hide();
                $(this).find("td:last input[id$=imbEditMaterial]").hide();
                $(this).find("td:last input[id$=imbDeleteMaterial]").hide();
                $(this).find("td:last input[id$=imbrateHistory]").show();
            });
        }
        $("#divdummyMaterial").hide();
        $(tdset).insertBefore($("#grdVendorMaterialDetails").find("tr:eq(1)"));
        $("#grdVendorMaterialDetails").find("tr:has(td)").each(function () {
            itemCount = itemCount + 1;
            var ItemPK = GrandGrid.Utilities.GetColumnValue($(this), "ITV_PK", $(this).parents("table:first").attr("id"));
            ItemPK = parseInt(ItemPK) >= 0 ? ItemPK : 0;
            var slNo = GrandGrid.Utilities.GetColumnValue($(this), agentRegistration.ITV_SL_NO, $(this).parents("table:first").attr("id"));
            slNo = parseInt(slNo) >= 0 ? slNo : 0;
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), agentRegistration.ITV_DISC_PERC, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscount('" + slNo + "','" + ItemPK + "');\" src=\"../Images/Classic/Icons/discount.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), agentRegistration.ITV_TAX_PERC, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + ItemPK + "');\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" purpose='MaterialTax' />");
                checkHideMaterialTaxPopUpImage();
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_PRICE", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                amt = GrandGrid.Utilities.GetColumnValue($(this), "ITV_PRICE", $(this).parents("table:first").attr("id"));
                strVal = amt == "" ? "" : parseFloat(amt).toFixed(RateDec);
                // strVal = amt == "" ? "" : amt;

                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_LEAD_TIME", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                var leadDays = GrandGrid.Utilities.GetColumnValue($(this), "ITV_LEAD_TIME", $(this).parents("table:first").attr("id"));
                strVal = leadDays == "" ? "" : leadDays;
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_MOQ", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "ITV_MOQ", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                if (!isNaN(strVal))
                    $(this).find("td:eq(" + colIndex + ")").html(strVal);
                else
                    $(this).find("td:eq(" + colIndex + ")").html("");

            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_ACTIVE", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                active = GrandGrid.Utilities.GetColumnValue($(this), "ITV_ACTIVE", $(this).parents("table:first").attr("id"));
                if (active == "1")
                    active = "Yes";
                else
                    active = "No";
                $(this).find("td:eq(" + colIndex + ")").html(active);
            }
        });
        if (itemCount == 0) {
            $("#divdummyMaterial").show();
        }

    }

    if (gridID == $("#grdRateDetails").attr("id")) {
        var toDate = "";
        $("#grdRateDetails").find("tr").each(function () {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), agentRegistration.ToDate, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                toDate = GrandGrid.Utilities.GetColumnValue($(this), agentRegistration.ToDate, $(this).parents("table:first").attr("id"));
                if (toDate == "null")
                    toDate = "Till Date";
                $(this).find("td:eq(" + colIndex + ")").html(toDate);
            }
        });
    }

    //NewSamples start
    if (gridID == $("#grdMaterialSampleDetails").attr("id")) {
        if (tdsetSamples == "") {//tdset contains controls for add details.
            tdsetSamples = $("#sampleInsert").find("tr:eq(1)");
        }
        //Mode iS view
        if ($("[id$=ViewStatus]").val() == "1") {
            //$("#divdummySamples").hide();
            //Hiding the template field
            // $("#grdPODetails th:last").hide();
            $("#grdMaterialSampleDetails").find("tr").each(function () {
                $(this).find("td:last,th:last").hide();

            });
        }
        else {
            //$("#divdummySamples").hide();
            //$(tdsetSamples).insertBefore($("#grdMaterialSampleDetails").find("tr:eq(1)"));
        }
        GrandScriptUtils.DatePicker("ISV_RECEIVED_DATE", false, false);
    }
    //New End

    if (gridID == $("#grdVendorAddressDetails").attr("id")) {
        $("#grdVendorAddressDetails").find("tr").each(function () {
            if ($("[id$=ViewStatus]").val() == "1") {
                $(this).find("td:last input[id$=imbEdit]").hide();
                $(this).find("td:last input[id$=imbDelete]").hide();
                $(this).find("td:last input[id$=imbView]").show();
            }
            else {
                $(this).find("td:last input[id$=imbEdit]").show();
                $(this).find("td:last input[id$=imbDelete]").show();
                $(this).find("td:last input[id$=imbView]").hide();
            }
        });
    }

    if (gridID == $("#grdVendorAddressDetails").attr("id")) {
        var addressType;
        var addressIndx = 0;
        var phoneNo;
        var phoneNoIndex = 0;
        var AddressBookType;
        var AddressBookTypeIndex = 0;
        var AddressBookContactPerson;
        var AddressBookContactPersonIndex = 0;


        $("#grdVendorAddressDetails").find("tr:has(td)").each(function () {
            addressIndx = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_ADDRESS_TYPE", gridID);
            addressType = GrandGrid.Utilities.GetColumnValue($(this), "VNC_DEFAULT", gridID);

            phoneNoIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_PHONE", gridID);
            phoneNo = GrandGrid.Utilities.GetColumnValue($(this), "VNC_PHONE", gridID);

            AddressBookTypeIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_TYPE_TEXT", gridID);
            AddressBookType = GrandGrid.Utilities.GetColumnValue($(this), "VNC_TYPE_TEXT", gridID);

            AddressBookContactPersonIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_CONT_NAME", gridID);
            AddressBookContactPerson = GrandGrid.Utilities.GetColumnValue($(this), "VNC_CONT_NAME", gridID);


            if (addressType == "1" || addressType == "True") {
                $(this).find("td:eq(" + addressIndx + ")").html("True");
            }
            else {
                $(this).find("td:eq(" + addressIndx + ")").html("False");
            }

            if (phoneNo == "null") {
                $(this).find("td:eq(" + phoneNoIndex + ")").html(" ");
            }

            if (AddressBookType == "undefined") {
                $(this).find("td:eq(" + AddressBookTypeIndex + ")").html(" ");
            }

            if (AddressBookContactPerson == "undefined" || AddressBookContactPerson == "null") {
                $(this).find("td:eq(" + AddressBookContactPersonIndex + ")").html(" ");
            }
        });
    }
    //    // grdVendorBankDetails Grid Bank Details
    if (gridID == $("#grdVendorBankDetails").attr("id")) {
        var ItemPK = GrandGrid.Utilities.GetColumnValue($(this), "VBD_PK", $(this).parents("table:first").attr("id"));
        ItemPK = parseInt(ItemPK) >= 0 ? ItemPK : 0;
        $("[id$=imbViewBankGrid]").each(function () {
            $(this).hide();
        });
        if ($("[id$=hdfViewMode]").val() == '1') {
            $('table[id=grdVendorBankDetails] input[type=image]').each(function () {
                $(this).hide();
            });
            $("[id$=imbViewBankGrid]").each(function () {
                $(this).show();
            });
        }
        $("#grdVendorBankDetails").find("tr:has(td)").each(function () {
            var countryIndx = GrandGrid.Utilities.GetColumnIndex($(this), "VBD_COUNTRY_NAME", gridID);
            var country = GrandGrid.Utilities.GetColumnValue($(this), "VBD_COUNTRY_NAME", gridID);
            if (country == "undefined" || country == "null") {
                $(this).find("td:eq(" + countryIndx + ")").html(" ");
            }
        });
    }

}

//NewMaterial start
function ShowRateHistory(itemPK, vendorPK) {
    //<summary>function used to add the item tax details</summary>
    ///////////////
    ajaxurl = agentRegistration.GetRateHistory + $("[id$=BizUnitPk]").val() + "&ItemPK=" + itemPK + "&VendorPK=" + vendorPK;
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(ajaxurl, function (data) {
        if (data.length > 0) {           
            $("[id$=ItemCode]").html(data[0].VIH_ITEM_CODE.length > 30 ? data[0].VIH_ITEM_CODE.substr(0, 30) + "..." : data[0].VIH_ITEM_CODE);
            $("[id$=ItemCode]").attr('title', data[0].VIH_ITEM_CODE);
            $("[id$=ItemName]").html(data[0].VIH_ITEM_NAME.length > 30 ? data[0].VIH_ITEM_NAME.substr(0, 30) + "..." : data[0].VIH_ITEM_NAME);
            $("[id$=ItemName]").attr('title', data[0].VIH_ITEM_NAME);
            GrandGrid.MakeGrid($("#grdRateDetails"), 0, data);
            $("#divRateHistory").dialog(
        {
            width: 540,
            title: "Translate(RateHistory)"
        });
        }
        else {
          
            $("[id$=ItemCode]").attr('title', "");
            $("[id$=ItemName]").attr('title', "");
            GrandGrid.MakeGrid($("#grdRateDetails"), 0, new Array());
            GrandScriptUtils.ShowModal("Translate(NoRateHistory)");
        }
    });
}

function SavePage(command, Type) {
    $("[id$=imbSave]").hide();   
    AddValidations(1);
    $("select[id$=VEN_TYPE]").attr("disabled", false);
    if ($(document.forms[0]).valid()) {
        SetItemTaxDetails();
   
        var ObjVendor = $("#divVendorData").data("VendorData");
        if ($("[id$=ActionID]").val() == "2") {
            ObjVendor.VEN_TYPE == "1";
        }

        // For File Upload---------------------------------------------------------------------
        var ObjFile = $("#divFileData").data("FileData");
        $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
        // For File Upload---------------------------------------------------------------------

        if (command != "Draft") {
            $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
        }
        else {
            $("[id$=ActionID]").val('0');
        }
        $("[id$=VEN_HAS_ISO]").val($("[id$=VEN_HAS_ISO1]").val() == "on" ? "true" : "false");
        $("[id$=VEN_PAY_FOR_VENDOR]").val($("[id$=VEN_PAY_FOR_VENDOR1]").val() == "on" ? "true" : "false");

        //Add VEN_ACTIVE
        var ActiveInactive;
        ActiveInactive = ($("[id$=VEN_ACTIVE1]").attr("checked")) ? "1" : "0";
        $("[id$=VEN_ACTIVE]").val(ActiveInactive);


        $("[id$=Roles]").val(JSON.stringify(GetSelectedRoles()));
        var jSonString = GrandScriptUtils.FormToJsonString("FormData");
        //To Prevent Duplicate Submission
        if ($("[id$=SubmitFlag]").val() == "0")
            $("[id$=SubmitFlag]").val('1')
        else
            return false;

        $.post(agentRegistration.SaveAgentDetails, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully

            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(agentRegistration.AgentCodeExist, agentRegistration.Information, agentRegistration.CodeExist);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data) == -3) {
                GrandScriptUtils.ShowModal(agentRegistration.AgentConcurrencyMsg, agentRegistration.Information, agentRegistration.ConcurrencyExist);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data) == -5) {//Already deleted
                GrandScriptUtils.ShowModal(agentRegistration.AlreadyDeletedMsg, agentRegistration.Information);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (parseInt(data) > 0) {
                $("[id$=VEN_PK]").val(data);
                if (command == "Draft") {
                    $("[id$=hdfVendorPK]").val(data)
                    if (Type == "AddMaterial") {
                        AddMaterials();
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (Type == "AddBank") {
                        AddBank();
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else
                        GrandScriptUtils.ShowModal(agentRegistration.AgentSaveMessage, agentRegistration.Information, agentRegistration.SaveCommand);
                }
                else {
                    $("[id$=hdfAppID]").val(data);
                    SaveWorkFlow();                   
                }
                $("[id$=hdfVendorPK]").val(data)
            }
            else {
                GrandScriptUtils.ShowModal(agentRegistration.ActionFailedMessage);
                $("[id$=SubmitFlag]").val('0')
            }
        });
    }
    $("select[id$=VEN_TYPE]").attr("disabled", true);
    $("[id$=imbSave]").show();

    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>Function used to show meassage</summary>
    GrandScriptUtils.ShowModal(agentRegistration.AgentSubmitMessage, agentRegistration.Information, agentRegistration.SaveCommand);
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        case agentRegistration.SaveCommand: //comment req
            window.location = agentRegistration.AgentListing + agentRegistration.TypeID;
            break;
        case agentRegistration.CodeExist: //Commend When calling 
            break;
        case agentRegistration.ConcurrencyExist: //Commend When calling 
            window.location = agentRegistration.AgentListing + agentRegistration.TypeID;
            break;
        case agentRegistration.DeleteAddress:
            DeleteAddress();
            break;
        case agentRegistration.DeleteMaterial:
            DeleteMaterial();
            break;
        //NewSamples                                  
        case agentRegistration.DeleteSamples:
            DeleteSamples();
            break;
        //New End                        
        case agentRegistration.TaxDelete:
            DeleteTaxDetails();
            break;
        case agentRegistration.TaxDeleteVendor:
            deleteVendorTaxDetails();
            break;
        case agentRegistration.DeleteBank:
            DeleteBank();
            break;
    }
    return false;
}
///#endregion

///#region validation Section
function AddValidations(mode) {
    ////<summary>function used validate each sections </summary>
    /// <param name="mode"  type="Object">
    /// Determins which session to validate if 1 vendor 2 address book 3 material 
    /// </param>
    RemoveValidation();
    if (mode == 1) {
        $("input[id$=VEN_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(ReqAgentName)" }
        });
        $("input[id$=VEN_CODE]").rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: "Translate(ReqAgentCode)" }
        });

        $("select[id$=VEN_CURRENCY]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectCurrency)" }
        });
      
        $("input[id$=VEN_EMAIL]").rules("add", {
            email: true,
            messages: { email: "Translate(RegEmail)" }
        });        
           
    }   
    else if (mode == 6) { // Bank Add
        $("input[id$=VBD_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(ReqBankTitle)" }
        });
    }
}

function RemoveAllValidations() {
    ////<summary>function used Remove validation before Upload the File </summary>
    RemoveValidation();
}

function RemoveValidation() {
    ////<summary>function remove all added validations </summary>
    /// <param name="mode"  type="Object">
    /// Determins which session to validate if 1 vendor 2 address book 3 material 
    /// </param>
   
    $("input[id$=VEN_NAME]").rules("remove");
    $("input[id$=MaterialItem]").rules("remove");
    $("input[id$=VEN_CONT_NAME]").rules("remove");
    $("input[id$=VEN_CODE]").rules("remove");
    $("input[id$=VEN_PHONE]").rules("remove");
    if ($("select[id$=VEN_CURRENCY]").lenght > 0)
        $("select[id$=VEN_CURRENCY]").rules("remove");
    $("input[id$=VNC_NAME]").rules("remove");
    $("input[id$=VNC_CONT_NAME]").rules("remove");
    $("input[id$=VNC_PHONE]").rules("remove");
    $("select[id$=MaterialType]").rules("remove");
    //    $("select[id$=ITV_ITEM]").rules("remove");
    $("input[id$=ITV_NAME]").rules("remove");
    $("input[id$=ITV_PRICE]").rules("remove");
    $("select[id$=ITV_CURRENCY]").rules("remove");
    $("select[id$=VEN_PO_TYPE]").rules("remove");
    $("input[id$=ITV_MOQ]").rules("remove");
    $("select[id$=ITV_MOQ_UOM]").rules("remove");
    
    $("input[id$=ITV_LEAD_TIME]").rules("remove");
    
    $("input[id$=VEN_ADDR1]").rules("remove");
    $("input[id$=VEN_TIN]").rules("remove");
    $("input[id$=VEN_ANNUAL_SALES]").rules("remove");

    //NewSamples Start
    $("select[id$=MaterialTypeSamples]").rules("remove");
    $("input[id$=txtSampleItem]").rules("remove");
    $("input[id$=ISV_RECEIVED_DATE]").rules("remove");
    $("input[id$=ISV_QC_TEST]").rules("remove");
    $("input[id$=ISV_QC_VALUE]").rules("remove");
    $("input[id$=ISV_REMARKS]").rules("remove");
    $("input[id$=ISV_QUANTITY]").rules("remove");
    //New End   
    $("input[id$=VBD_NAME]").rules("remove");
}

function FillTaxDiscount(category) {
    ///<summary>function To Fill tax Details </summary>
    var drpID;
    drpID = $("[id$=ChooseTax]").attr("id");
    var reqString = agentRegistration.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDue=0&ISPURCHASE=1";
    $.get(reqString, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function AddLineItemTax(slNo, ItemPK) {
    //<summary>function used to add the item tax details</summary>   
    $("[id$='hdfTaxPopUpByVendor']").val('0');
    $("#divTaxDataVendor").data("TaxObj", '');
    RemoveAllValidations();
    FillTaxDiscount(1);
    materialID = ItemPK;
    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(TaxDetails)"
        });
    agentRegistration.ItemTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(1);
    ClearPopUp(slNo, 1, ItemPK);
    ClearTaxDetails();
}

function AddLineItemDiscount(slNo, itemPK) {
    //<summary>function used to add the item discount details</summary>
    $("#divTaxDataVendor").data("TaxObj", '');
    RemoveAllValidations();
    materialID = itemPK;
    FillTaxDiscount(3);

    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
    {
        width: 540,
        title: "Translate(DiscountDetails)"
    });
    agentRegistration.ItemTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(3);
    ClearPopUp(slNo, 3, itemPK);
    ClearTaxDetails();
}

function ClearPopUp(slNo, type, itemPK) {
    //<summary>function used to bind the item tax/ discount details</summary>
    var TaxDetails = new Array();
    var ObjVendor = $("#divVendorData").data("VendorData");
    $.get(agentRegistration.GetVndrTaxDiscountDetails + itemPK + "&Category=" + type, function (data) {
        if (data.TaxDetail) {
            var category = parseInt($("[id$=hdfTaxCategory]").val());
            data.BIZUNIT_PK = ObjVendor.BizUnitPk;
            data.USER_PK = ObjVendor.UserPk;
            data.ITV_PK = materialID;
            data.ITV_TAX_CATEGORY = category;
            $("#divTaxData").data("TaxObj", data);
            if (data.TaxDetail.length > 0) {
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, data.TaxDetail);
            }
            else {
                var TaxObj = new Object();
                TaxObj.IVT_SL_NO = data.TaxDetail.IVT_SL_NO;
                TaxObj.IVT_PK = data.TaxDetail.IVT_PK;
                TaxObj.IVT_TAX = data.TaxDetail.IVT_TAX;
                TaxObj.IVT_TAX_TEXT = data.TaxDetail.IVT_TAX_TEXT;
                TaxObj.IVT_TAX_CATEGORY = data.TaxDetail.IVT_TAX_CATEGORY;
                TaxDetails.push(TaxObj);
                data.TaxDetail = TaxDetails;
                $("#divTaxData").data("TaxObj", data);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, TaxDetails);
            }
        }
        else {
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, new Array());
            $("#divTaxData").data("TaxObj", new Array());
        }
    });
    
}


function GetTaxDiscountDetails(type, itemPK) {
    //<summary>function used to get the tax / discount details </summary>
    var TaxArray = new Array();
    $.get(agentRegistration.GetVndrTaxDiscountDetails + itemPK + "&Category=" + type, function (data) {
        if (data.TaxDetail) {
            for (var i in data.TaxDetail) {
                TaxArray.push(TaxDetail[i]);
            }
        }
    });
    return TaxArray;

    for (var i in TaxDetails) {
        if (TaxDetails[i].IVT_SL_NO == slNo && TaxDetails[i].IVT_TAX_CATEGORY == type) {
            TaxArray.push(TaxDetails[i]);
        }
    }
    return TaxArray;
}



function ClearTaxDetails() {
    //<summary>function used to clear the tax/ discount details</summary>
    agentRegistration.EditTax = 0;
    $("[id$=ChooseTax]").val("0");
}

function SaveTaxDiscount() {
    //<summary>function to save tax/discount details</summary>
    // var TaxDetails = $("#divData").data("TaxDetails");
    var ObjTaxDetails = $("#divTaxData").data("TaxObj");
    var TaxDetails = TaxDetails = new Array();
    if (ObjTaxDetails.TaxDetail) {
        if (ObjTaxDetails.TaxDetail.length > 0)
            for (var i in ObjTaxDetails.TaxDetail) {
                TaxDetails.push(ObjTaxDetails.TaxDetail[i]);
            }
        else {
            var TaxObj = new Object();
            TaxObj.IVT_SL_NO = ObjTaxDetails.TaxDetail.IVT_SL_NO;
            TaxObj.IVT_PK = ObjTaxDetails.TaxDetail.IVT_PK;
            TaxObj.IVT_TAX = ObjTaxDetails.TaxDetail.IVT_TAX;
            TaxObj.IVT_TAX_TEXT = ObjTaxDetails.TaxDetail.IVT_TAX_TEXT;
            TaxObj.IVT_TAX_CATEGORY = ObjTaxDetails.TaxDetail.IVT_TAX_CATEGORY;
            TaxDetails.push(TaxObj);
        }
    }
    //   TaxDetails = ObjTaxDetails.TaxDetail;


    var slNo = parseInt(agentRegistration.ItemTaxPK);
    var itemTax = parseInt($("[id$=ChooseTax]").val());
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    AddValidations(5);
    var TaxDetailsObj = null;
    if ($(document.forms[0]).valid()) {
        if (itemTax > 0) {
            if (slNo > 0) {
                if (agentRegistration.EditTax == 0) {
                    if (TaxDetails.length > 0) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                    .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax && tax.IVT_TAX_CATEGORY == category; })
                                    .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IVT_SL_NO = slNo;
                        TaxDetailsObj.IVT_PK = 0;
                        // TaxDetailsObj.IVT_PK = materialID;
                        TaxDetailsObj.IVT_TAX = parseInt(itemTax) > 0 ? itemTax : 0;
                        TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(agentRegistration.TypeAlreadyAdded, agentRegistration.Information);
                    }
                }
                else {
                    if (agentRegistration.EditTax == itemTax) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                        .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == agentRegistration.EditTax; })
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
                            GrandScriptUtils.ShowModal(agentRegistration.TypeAlreadyAdded, agentRegistration.Information);
                        }
                    }
                }

                var ObjVendor = $("#divVendorData").data("VendorData");

                var TaxArray = new Array();
                for (var i in TaxDetails) {
                    TaxArray.push(TaxDetails[i]);
                }
                var ObjTax = new Object();
                ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
                ObjTax.USER_PK = ObjVendor.UserPk;
                ObjTax.ITV_PK = materialID;
                ObjTax.ITV_TAX_CATEGORY = category;
                ObjTax.TaxDetail = TaxDetails;
                $("#divTaxData").data("TaxObj", ObjTax);
              
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, ObjTax.TaxDetail);
            }
            ClearTaxDetails();
        }
    }
    return false;
}

function SaveApply() {
    //<summary>function used to clear the tax/ discount details</summary>
    var TaxDetails = $("#divData").data("TaxDetails");
    var ObjTax = $("#divTaxData").data("TaxObj");
    var taxList = JSON.stringify(ObjTax);
    $.post(agentRegistration.SaveTaxDiscount, taxList, function (data) {
        if (data) {

        }
    });
    $("#divItemTax").dialog("close");
    $("[id$=hdfIsMaterialTax]").val('1');
    checkHideVendorTaxPopUpImage();
    return false;
}

function GridHandler(tr, command) {
    ////<summary>function used handle Grid Events </summary>
    switch (command.toString().toUpperCase()) {
        case agentRegistration.TaxDelete:
            agentRegistration.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.IVT_SL_NO, $(tr).parents("table:first").attr("id"));
            agentRegistration.EditTax = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.IVT_TAX, $(tr).parents("table:first").attr("id"));
            var slNo = $("[id$=hdnSlNo]").val();
            GrandScriptUtils.ShowModal(agentRegistration.DeleteConfirmationMessage, agentRegistration.ConfirmationMessage, agentRegistration.TaxDelete, true);
            break;
        case agentRegistration.TaxDeleteVendor:
            agentRegistration.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.IVT_SL_NO, $(tr).parents("table:first").attr("id"));
            agentRegistration.EditTax = GrandGrid.Utilities.GetColumnValue(tr, agentRegistration.IVT_TAX, $(tr).parents("table:first").attr("id"));
            var slNo = $("[id$=hdnSlNo]").val();
            GrandScriptUtils.ShowModal(agentRegistration.DeleteConfirmationMessage, agentRegistration.ConfirmationMessage, agentRegistration.TaxDeleteVendor, true);
            break;
    }
    return false;
}

function DeleteTaxDetails() {
    //<summary>function used to delete the tax details</summary>    
    //var TaxDetails = $("#divData").data("TaxDetails");
    var taxObj = $("#divTaxData").data("TaxObj");
    var TaxDetails = taxObj.TaxDetail;
    for (var i in TaxDetails) {
        if (agentRegistration.EditTax > 0) {
            if (TaxDetails[i].IVT_SL_NO == agentRegistration.ItemTaxPK && TaxDetails[i].IVT_TAX == agentRegistration.EditTax) {
                TaxDetails.splice(i, 1);
            }
        }
    }
    //  $("#divData").data("TaxDetails", TaxDetails);
    taxObj.TaxDetail = TaxDetails;
    $("#divTaxData").data("TaxObj", taxObj);
    // GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(agentRegistration.ItemTaxPK, $("[id$=hdfTaxCategory]").val(), true));
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, TaxDetails);

    agentRegistration.EditTax = 0;
}

function SetItemTaxDetails() {
    //<summary>function used to get the item tax details</summary>
    var ObjVendor = $("#divVendorData").data("VendorData");
    var TaxDetails = $("#divData").data("TaxDetails");
    for (var itv in ObjVendor.MaterialDetails) {
        slNo = ObjVendor.MaterialDetails[itv].ITV_SL_NO;
        var TaxArray = new Array();
        for (var i in TaxDetails) {
            if (TaxDetails[i].IVT_SL_NO == slNo) {
                TaxArray.push(TaxDetails[i]);
            }
        }
        ObjVendor.MaterialDetails[itv].TaxDtl = TaxArray;
    }
}
///#endregion

function AddVendorItemTax(slNo, ItemPK) {
    //<summary>function used to add the item tax details</summary>    
    RemoveAllValidations();
    RemoveValidation();
    FillTaxDiscountVendor(1);
    $("[id$='hdfTaxPopUpByVendor']").val('1');
    //    if (!$("#divTaxDataVendor").data("TaxObj")) {
    //        $("#divTaxDataVendor").data("TaxObj", '');
    //    }    

    // materialID = ItemPK;
   

    agentRegistration.VendorTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(1);
    var category = parseInt($("[id$=hdfTaxCategory]").val());

    $("[id$=ChooseTaxVendor]").val("0"); // ClearTaxDetails();
    var t = $("[id$=TaxHdr]").val();
    var TaxArray = jQuery.parseJSON(t);
    if (!TaxArray) {
        TaxArray = new Array();
    }
    var ObjVendor = $("#divVendorData").data("VendorData");
    // var TaxArray = new Array();
    var ObjTax = new Object();
    ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
    ObjTax.USER_PK = ObjVendor.UserPk;
    ObjTax.ITV_PK = materialID;
    ObjTax.ITV_TAX_CATEGORY = category;
    ObjTax.TaxDetail = TaxArray;
    $("#divTaxDataVendor").data("TaxObj", ObjTax);

    ClearPopUpVendorTax(slNo, 1, ItemPK);
    $("#divVendorTax").dialog("open");
    $("#divVendorTax").dialog(
        {
            width: 540,
            title: "Translate(TaxDetails)"
        });

    return false;
}

function FillTaxDiscountVendor(category) {
    ///<summary>function To Fill tax Details </summary>
    var drpID;
    drpID = $("[id$=ChooseTaxVendor]").attr("id");
    var reqString = agentRegistration.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDue=0&ISPURCHASE=1";
    $.get(reqString, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function ClearPopUpVendorTax(slNo, type, itemPK) {
    var ObjTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, ObjTaxDetails.TaxDetail);
}

function AddTaxDiscountVendorToGrid() {
    //<summary>function to save tax/discount details for Vendor</summary>  
    var ObjTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    var TaxDetails = new Array();
    if (ObjTaxDetails.TaxDetail) {
        if (ObjTaxDetails.TaxDetail.length > 0)
            for (var i in ObjTaxDetails.TaxDetail) {
                TaxDetails.push(ObjTaxDetails.TaxDetail[i]);
            }

    }
    //   TaxDetails = ObjTaxDetails.TaxDetail;


    var slNo = parseInt(agentRegistration.VendorTaxPK);
    var itemTax = parseInt($("[id$=ChooseTaxVendor]").val());
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    // AddValidations(5);
    var TaxDetailsObj = null;
    if ($(document.forms[0]).valid()) {
        if (itemTax > 0) {
            if (slNo > 0) {
                if (agentRegistration.EditTax == 0) {
                    if (TaxDetails.length > 0) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                    .Where(function (tax) { return tax.IVT_TAX == itemTax; })
                                    .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IVT_SL_NO = slNo;
                        TaxDetailsObj.IVT_PK = 0;
                        // TaxDetailsObj.IVT_PK = materialID;
                        TaxDetailsObj.IVT_TAX = parseInt(itemTax) > 0 ? itemTax : 0;
                        TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTaxVendor] :selected").text());
                        TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(agentRegistration.TypeAlreadyAdded, agentRegistration.Information);
                    }
                }
                else {
                    if (agentRegistration.EditTax == itemTax) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                        .Where(function (tax) { return tax.IVT_TAX == agentRegistration.EditTax; })
                                        .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.IVT_TAX = itemTax;
                            TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTaxVendor] :selected").text());
                            TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                          .Where(function (tax) { return tax.IVT_TAX == itemTax; })
                          .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(agentRegistration.TypeAlreadyAdded, agentRegistration.Information);
                        }
                    }
                }

                var ObjVendor = $("#divVendorData").data("VendorData");

                var TaxArray = new Array();
                for (var i in TaxDetails) {
                    TaxArray.push(TaxDetails[i]);
                }
                var ObjTax = new Object();
                ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
                ObjTax.USER_PK = ObjVendor.UserPk;
                ObjTax.ITV_PK = materialID;
                ObjTax.ITV_TAX_CATEGORY = category;
                ObjTax.TaxDetail = TaxDetails;
                $("#divTaxDataVendor").data("TaxObj", ObjTax);
                //  var taxList = JSON.stringify(ObjTax);
                //    $("#divData").data("TaxDetails", TaxDetails);
                GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, ObjTax.TaxDetail);
            }

            agentRegistration.EditTax = 0;
            $("[id$=ChooseTaxVendor]").val("0");
        }
    }
    return false;
}

function SaveApplyVendorTax() {
    var ObjTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    var v = JSON.stringify(ObjTaxDetails.TaxDetail);
    $("[id$=TaxHdr]").val(v);
    $("#divVendorTax").dialog("close");
    $("[id$=hdfIsVendorTax]").val('1')
    checkHideMaterialTaxPopUpImage();
    var taxText = '';
    for (i = 0; i < ObjTaxDetails.TaxDetail.length; i++) {
        taxText += ObjTaxDetails.TaxDetail[i].IVT_TAX_TEXT + ", ";
    }
    taxText = taxText.substring(0, taxText.length - 2);
    $("[id$=TaxByVendor]").attr('title', taxText);
    if (taxText.length > 35) {
        taxText = taxText.substring(0, 35);
        taxText += '...';
    }
    $("[id$=TaxByVendor]").text(taxText);
}


function convertDate(date) {
    // 2014-09-21T00:00:00 to 01-Sep-2014
    var months = { '01': 'Jan', '02': 'Feb', '03': 'Mar', '04': 'Apr', '05': 'May', '06': 'Jun', '07': 'Jul', '08': 'Aug', '09': 'Sep', '10': 'Oct', '11': 'Nov', '12': 'Dec' };
    var splitArr = date.split('-'); // split based on '-'
    var yyyy = splitArr[0]; // Year
    var mm = months[splitArr[1]]; // convert month into lower
    var splitArr2 = splitArr[2].split('T');
    var dd = splitArr2[0];
    if (dd < 10) dd = dd; // add '0' if date is less then 10.
    return [dd, mm, yyyy].join('-'); // join value according to format.
}

function checkHideVendorTaxPopUpImage() {
    if ($("[id$=hdfIsMaterialTax]").val() == '1') {
        $("img[purpose='VendorTax']").hide();
    }
}
function checkHideMaterialTaxPopUpImage() {
    if ($("[id$=hdfIsVendorTax]").val() == '1') {
        $("img[purpose='MaterialTax']").hide();
    }

}
function deleteVendorTaxDetails() {
    //<summary>function used to delete the tax details</summary>
    var objTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    for (var i in objTaxDetails.TaxDetail) {
        if (agentRegistration.EditTax > 0) {
            if (objTaxDetails.TaxDetail[i].IVT_TAX == agentRegistration.EditTax) {
                objTaxDetails.TaxDetail.splice(i, 1);
                $("#divTaxDataVendor").data("TaxObj", objTaxDetails);
                var TaxDetails1 = $("#divTaxDataVendor").data("TaxObj");
            }
        }
    }
    $("#divTaxDataVendor").data("TaxObj", objTaxDetails);
    //  GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(agentRegistration.ItemTaxPK, $("[id$=hdfTaxCategory]").val(), true));
    GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, objTaxDetails.TaxDetail);
    agentRegistration.EditTax = 0;
}

// Bank Tab -- Begins

function bindBankTab() {
    // bindBankCountry();
    bindAccountTypes();

    $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").attr('disabled', 'disabled');
    $("select[id$=VBD_ACCOUNT_TYPE]").change(function () {
        $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").val('')
        $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").attr('disabled', 'disabled');

        var value = $("select[id$=VBD_ACCOUNT_TYPE] option:selected").val();
        if (value == '-1') {
            $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").removeAttr('disabled');
        }
    });
}

function bindAccountTypes() {
    $.get(agentRegistration.GetAccountTypes, function (data) {
        var id = $("select[id$=VBD_ACCOUNT_TYPE]").attr("id");
        GrandScriptUtils.FillDropDown(id, data, true, true, false, false, true);
    });
}

function AddBankDetails() {
    ///<summary>
    ///Used for SaveBankDetails
    ///</summary>
    if ($("[id$=hdfVendorPK]").val() == "0") {
        SavePage("Draft", "AddBank"); 
    }
    else
        AddBank();

    return false;
}

function AddBank() {
    // $("input[id$=VBD_NAME]").removeClass("error");
    $("input[id$=VBD_ACCOUNT_NO]").removeClass("error");
    $("input[id$=VBD_ACCOUNT_NO_CNFM]").removeClass("error");
    RemoveValidation();
    AddValidations(6)
    if ($(document.forms[0]).valid()) {
        if ($("input[id$=VBD_ACCOUNT_NO]").val() != $("input[id$=VBD_ACCOUNT_NO_CNFM]").val()) {
            GrandScriptUtils.ShowModal("Translate(ValuesMisMatch)", "Information");
            $("input[id$=VBD_ACCOUNT_NO]").addClass("error");
            $("input[id$=VBD_ACCOUNT_NO_CNFM]").addClass("error");
            return false;
        }
        var obj = new Object();
        obj.VBD_PK = bankID;
        obj.VBD_VENDOR = $("[id$=hdfVendorPK]").val();
        obj.VBD_NAME = $("[id$=VBD_NAME]").val();
        obj.VBD_BRANCH = $("input[id$=VBD_BRANCH]").val();
        obj.VBD_CITY = $("input[id$=VBD_CITY]").val();
        if ($("select[id$=VBD_COUNTRY]").val() == 0) {
            obj.VBD_COUNTRY = '';
        }
        else {
            obj.VBD_COUNTRY = $("select[id$=VBD_COUNTRY]").val();
        }
        obj.VBD_PHONE = $("input[id$=VBD_PHONE]").val();
        obj.VBD_FAX = $("input[id$=VBD_FAX]").val();
        obj.VBD_IFSC_CODE = $("input[id$=VBD_IFSC_CODE]").val();
        obj.VBD_ACCOUNT_NO = $("input[id$=VBD_ACCOUNT_NO]").val();
        if ($("select[id$=VBD_ACCOUNT_TYPE]").val() == 0 || $("select[id$=VBD_ACCOUNT_TYPE]").val() == -1) {
            obj.VBD_ACCOUNT_TYPE = '';
        }
        else {
            obj.VBD_ACCOUNT_TYPE = $("select[id$=VBD_ACCOUNT_TYPE]").val();
        }
        obj.VBD_ACCOUNT_TYPE_OTHER = $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").val();
        obj.VBD_STATE_OTHER = $("input[id$=VBD_STATE_OTHER]").val();
        obj.VBD_ZIP = $("input[id$=VBD_ZIP]").val();
        obj.VBD_EMAIL = $("input[id$=VBD_EMAIL]").val();
        obj.VBD_SWIFT_CODE = $("input[id$=VBD_SWIFT_CODE]").val();
        obj.VBD_ACCOUNT_NO_CNFM = $("input[id$=VBD_ACCOUNT_NO_CNFM]").val();
        obj.VBD_CONTACT = $("input[id$=VBD_CONTACT]").val();
        obj.VBD_ACTIVE = 1;
        obj.VBD_MOD_BY = 0;
        obj.VBD_ADDRESS = $("textarea[id$=VBD_ADDRESS]").val();
        obj.VBD_MOBILE = $("input[id$=VBD_MOBILE]").val();

        var bank = JSON.stringify(obj);
        $.post(agentRegistration.SaveBank, bank, function (data) {
            if (data) {
                if (parseInt(data) > 0) {
                    BindGridBanks();
                }
                else {
                    GrandScriptUtils.ShowModal("Information Not Saved", "Information");
                }
            }
        });
        clearBank();

    }
    // RemoveValidation();
    return false;
}

function BindGridBanks() {
    //<summary>To handle bind grid Bank corr. to the VendorPk value</summary>
    //    var srchV = "";
    //    var ajaxUrl = agentRegistration.GetAgentBanks + $("[id$=hdfVendorPK]").val()
    //    $("#grdVendorBankDetails").removeAttr("ajaxurl")
    //    $("#grdVendorBankDetails").attr("ajaxurl", ajaxUrl);
    //    GrandGrid.Utilities.ResetGrid(true, "grdVendorBankDetails");
    //    GrandGrid.MakeGrid($("#grdVendorBankDetails"));
    if ($("[id$=hdfVendorPK]").val() == undefined || $("[id$=hdfVendorPK]").val() == 0) {
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdVendorBankDetails"), 0, dummyObj);
    }
    else {
        $.get(agentRegistration.GetAgentBanks + $("[id$=hdfVendorPK]").val(), function (data) {
            GrandGrid.Utilities.ResetGrid(true, "grdVendorBankDetails");
            GrandGrid.MakeGrid($("#grdVendorBankDetails"), 1, data);
            //Bug:Shows an concurrency message when save agent after save bank details.For Resolving this we fetch VEN_MOD_DT from bankdetails get sp and set this as Last modified date
            if (data.length > 0) {
                var bankDet = data[0];
                if (bankDet.VEN_MOD_DT) {
                    $("[id$=LAST_MOD_DATE]").val((bankDet.VEN_MOD_DT));
                }
            }
        });
    }
    return false;
}

function GridBankAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString().toUpperCase()) {
        case agentRegistration.DeleteCommand:
            var vbdPk = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            //  materialID = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(agentRegistration.DeleteConfirmationMessage, agentRegistration.ConfirmationMessage, agentRegistration.DeleteBank, true);
            bankID = vbdPk;
            //            DeleteBank(vbdPk);
            return false;
            break;
        case agentRegistration.EditCommand:
            var vbdPk = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            FillBankDetailsFromGrid(vbdPk);
            return false;
            break;
        case agentRegistration.ViewCommand:
            var vbdPk = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            FillBankDetailsFromGrid(vbdPk);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

function FillBankDetailsFromGrid(vbdPk) {
    ///<summary>Function Used Fill the Bank Details corresponding to the vbdPk </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls       
    /// </param>
    var AgentID = $("[id$=VEN_PK]").val();
    bankID = vbdPk;
    $.get(agentRegistration.GetBankDetailsById + AgentID + "&P_VBD_PK=" + vbdPk, function (data) {
        var bankDet = data[0];
        $("[id$=VBD_NAME]").val(bankDet.VBD_NAME);
        $("[id$=VBD_BRANCH]").val(bankDet.VBD_BRANCH);
        $("[id$=VBD_ADDRESS]").val(bankDet.VBD_ADDRESS);
        $("[id$=VBD_CITY]").val(bankDet.VBD_CITY);
        $("[id$=VBD_STATE_OTHER]").val(bankDet.VBD_STATE_OTHER);
        if (bankDet.VBD_COUNTRY == null) {
            $("[id$=VBD_COUNTRY]").val(0);
        }
        else {
            $("[id$=VBD_COUNTRY]").val(bankDet.VBD_COUNTRY);
        }       
        $("[id$=VBD_ZIP]").val(bankDet.VBD_ZIP);
        $("[id$=VBD_PHONE]").val(bankDet.VBD_PHONE);
        $("[id$=VBD_MOBILE]").val(bankDet.VBD_MOBILE);
        $("[id$=VBD_FAX]").val(bankDet.VBD_FAX);
        $("[id$=VBD_EMAIL]").val(bankDet.VBD_EMAIL);
        $("[id$=VBD_IFSC_CODE]").val(bankDet.VBD_IFSC_CODE);
        $("[id$=VBD_SWIFT_CODE]").val(bankDet.VBD_SWIFT_CODE);
        $("[id$=VBD_ACCOUNT_NO]").val(bankDet.VBD_ACCOUNT_NO);
        $("[id$=VBD_ACCOUNT_NO_CNFM]").val(bankDet.VBD_ACCOUNT_NO);
        $("[id$=VBD_ACCOUNT_TYPE]").val(bankDet.VBD_ACCOUNT_TYPE);
        if (bankDet.VBD_ACCOUNT_TYPE == null) {
            $("[id$=VBD_ACCOUNT_TYPE]").val(0);
        }
        else {
            $("[id$=VBD_ACCOUNT_TYPE]").val(bankDet.VBD_ACCOUNT_TYPE);
        }
        $("[id$=VBD_ACCOUNT_TYPE_OTHER]").val(bankDet.VBD_ACCOUNT_TYPE_OTHER);
        if (bankDet.VBD_ACCOUNT_TYPE_OTHER != '') {
            $("[id$=VBD_ACCOUNT_TYPE]").val(-1);
        }
        $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").attr('disabled', 'disabled');
        var value = $("select[id$=VBD_ACCOUNT_TYPE] option:selected").val();
        if (value == '-1') {
            $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").removeAttr('disabled');
        }
        $("[id$=VBD_CONTACT]").val(bankDet.VBD_CONTACT);
    });
    return false;
}

function clearBank() {
    bankID = 0;
    $("[id$=VBD_NAME]").val('');
    $("[id$=VBD_BRANCH]").val('');
    $("[id$=VBD_ADDRESS]").val('');
    $("[id$=VBD_CITY]").val('');
    $("[id$=VBD_STATE_OTHER]").val('');
    $("[id$=VBD_COUNTRY]").val(0);
    $("[id$=VBD_ZIP]").val('');
    $("[id$=VBD_PHONE]").val('');
    $("[id$=VBD_MOBILE]").val('');
    $("[id$=VBD_FAX]").val('');
    $("[id$=VBD_EMAIL]").val('');
    $("[id$=VBD_IFSC_CODE]").val('');
    $("[id$=VBD_SWIFT_CODE]").val('');
    $("[id$=VBD_ACCOUNT_NO]").val('');
    $("[id$=ACCOUNT_NO_CNFM]").val('');
    $("[id$=VBD_ACCOUNT_TYPE]").val(0);
    $("[id$=VBD_ACCOUNT_TYPE_OTHER]").val('');
    $("[id$=VBD_CONTACT]").val('');
    RemoveValidation();
    //$("input[id$=VBD_NAME]").rules("remove");
    return false;
}

function DeleteBank() {
    ///<summary>Function Used to delete the selected Bank from the Bank Grid and Database </summary>
    $.get(agentRegistration.DeleteAgentBank + bankID, function (data) {
        if (parseInt(data) > 0) {
            bankID = 0;
            BindGridBanks();
        }

    });
    clearBank();
}
// Bank Tab -- Ends

function Popup() {
    ///<summary>Function used for popup</summary>

    $("#divVendorTax").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}

function DisableControlls() {
    $("[id$=imbTaxDiscountSaveVendor]").hide();
    $("[id$=btnApplyVendorTax]").hide();
    $("[id$=ChooseTaxVendor]").attr("disabled", "disabled");
    $("[id$=imbTaxDeleteVendor").hide();
}