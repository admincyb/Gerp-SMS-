/// <reference path="../../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../../../GrandScriptUtils.js" />

///#region -------------- Global Variable -------
var SBUJson = new Object();
///#endregion


var SBUConfiguration = {
    // url section
    SBUConfigSave: "SBUConfiguration.do?Action=SaveSBUConfig",
    SBUConfigGet: "SBUConfiguration.do?Action=GetSBUConfig",
    SBUConfigInactive: "SBUConfiguration.do?Action=ActiveSBUConfig&SBUPk=",
    GetSBUConfigDetails: "SBUConfiguration.do?Action=GetSBUConfigDetails&SBUID=",
    GetSBUFooterDetails: "SBUConfiguration.do?Action=GetSBUFooterDetails",
    GetCurrency: "CommonManagement.do?Action=GetCurrencyList", 
    GetCountry: "CommonManagement.do?Action=GetCountryList",
    GetState: "CommonManagement.do?Action=GetStateList",
    Inbox: "../../AccountManagement/WorkflowInbox.aspx",

    // validation section
    SBUNameValidation: "Translate(PleaseProvideSBUName)",
    SBUCodeValidation: "Translate(PleaseProvideSBUCode)",
    SBUAddrValidation: "Translate(PleaseProvideSBUPrimaryContactAddress)",
    SBUEmailValidation: "Translate(Pleaseenteravalidemailaddress)",

    // Command Section
    SaveCommand: "Save",
    EditCommand: "Edit",
    ActivceCommand: "Active",

    // Message section
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    SBUSaveMessage: "Translate(SBUSavedSuccesfully)",
    SBUInActiveMessage: "Translate(SBUDetailsInactiveSuccesfully)",
    SBUActiveMessage: "Translate(SBUDetailsActiveSuccesfully)",
    SBUExistsMessage: "Translate(SBUcodelreadyexists)",
    SBUUsedMessage: "Translate(CannotInactive)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    InActiveConfirmationMessage: "Translate(Doyouwanttoinactivethisdetails)",
    ActiveConfirmationMessage: "Translate(DoyouwanttoActivethisdetails)",

    // Field Map section
    SBUPk: "BZU_PK",
    ISINACTIVE: "IS_INACTIVE",
    SBUName: "BZU_NAME",
    SBUCode: "BZU_CODE",
    SBUPrimaryAddr: "BZU_ADDR1",
    SBUActive: "BZU_ACTIVE",
    Currency: "BZU_CURRENCY",
    BisRegNo: "BZU_REG_NO",
    GSTNo: "BZU_GST_NO",
    FinYearStartDate: "BZU_FIN_START_DT",
    FinYearEndDate: "BZU_FIN_END_DT",

    // Global Variable
    gSBUPk: 0,
    Active: 1
}

//#region----------- Initialization Section----------------

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();

    $("select[id$=SBUCountry]").change(function () {
        FillState($(this).val(), 0);
    });
});

function PageInit() {
    //<summary>Function to initialize the page</summary>

    BindGrid();
    $("[id$=SBUCode]").focus();
    FillCurrency(0,0);
    FillCountry(0);
    FillState(0, 0);
    DateInit();
    FillFooterDetails();
}



//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("input[id$=SBUName]").rules("add", {
        required: true,
        maxlength: 200,
        messages: { required: SBUConfiguration.SBUNameValidation }
    });
    $("input[id$=SBUCode]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: SBUConfiguration.SBUCodeValidation }
    });
    $("[id$=SBUAddr1]").rules("add", {
        required: true,
        maxlength: 200,
        messages: { required: SBUConfiguration.SBUAddrValidation }
    });
    $("[id$=BZU_CURRENCY]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(SelectCurrency)" }
    });
    $("[id$=SBUCountry]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(ProvideCountry)" }
    }); 
    $("[id$=SBUEmail]").rules("add", {
        required: false,
        email: true,
        messages: { required: SBUConfiguration.SBUEmailValidation }
    });
    $("[id$=BZU_YEAR_CTRL]").rules("add", {
        date: true,
        required: false,
        messages: { required: "Enter Year Control" }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
}

//#endregion

//#region----------- Core Section----------------

function BindGrid() {
    //<summary>Function to bind sbu details</summary>

    var ajaxUrl = SBUConfiguration.SBUConfigGet;
    $("#grdSBUList").removeAttr("ajaxurl")
    $("#grdSBUList").attr("ajaxurl", ajaxUrl);
    GrandGrid.MakeGrid($("#grdSBUList"));
    return false;
}

function SavePage() {
    ///<summary>Function used to Save SBU Config details</summary> 

    AddValidations();
    if ($("[id$=BZU_FIN_START_DT]").val() != '') {
        $("[id$=BZU_FIN_START_DT]").val($("[id$=BZU_FIN_START_DT]").val() + "-" + new Date().getFullYear());
    }
    if ($("[id$=BZU_FIN_END_DT]").val() != '') {
        $("[id$=BZU_FIN_END_DT]").val($("[id$=BZU_FIN_END_DT]").val() + "-" + new Date().getFullYear());
    }

    $("[id$=BZU_THEME]").val($("select[id$=ddlTheme]").val() == "-1" ? "" : $("select[id$=ddlTheme]").val());

    var jSonString = GrandScriptUtils.FormToJsonString(false);
    if ($(document.forms[0]).valid()) {
        $.post(SBUConfiguration.SBUConfigSave, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(SBUConfiguration.SBUSaveMessage, SBUConfiguration.MessageBoxTitle, SBUConfiguration.SaveCommand);
                BindGrid();
                ResetPage();
            }
            else if (parseInt(result) == 0)
                GrandScriptUtils.ShowModal(SBUConfiguration.SBUExistsMessage, SBUConfiguration.MessageBoxTitle);
            else {
                GrandScriptUtils.ShowModal(SBUConfiguration.ActionFailedMessage, SBUConfiguration.MessageBoxTitle);
                ResetPage();
            }
        });
    }
    return false;
}


function FillFooterDetails() {
    ///<summary> Display SBU Footer Details </summary>

    $.get(SBUConfiguration.GetSBUFooterDetails, function (data) {
        $("#divfooter").data("SBUFooterData", data);
        FillSBUFooterDetails();
    });
}

//Method to display footer details
function FillSBUFooterDetails() {
    SBUFooterJson = $("#divfooter").data("SBUFooterData");
    if (SBUFooterJson != null && SBUFooterJson != "") {
        $("[id$=SYS_NAME]").html(SBUFooterJson[0].SYS_NAME);
        $("[id$=SYS_VERSION]").html(SBUFooterJson[0].SYS_VERSION);
        $("[id$=SYS_GAF_VERSION]").html(SBUFooterJson[0].SYS_GAF_VERSION);
    }
}

function FillDetails(tr) {
    ///<summary> Fill SBU Details for edit</summary>
    /// <param name="tr"  type="object">
    ///  the tr contains all details of the perticular id
    /// </param>

    // var grdID = $(tr).parents("table:first").attr("id");

    var sbuId = GrandGrid.Utilities.GetColumnValue(tr, "BZU_PK", $(tr).parent().parent().attr("id"));
    var objDisp = new Object();
    $.get(SBUConfiguration.GetSBUConfigDetails + sbuId, function (data) {
        $("#divData").data("SBUData", data);
        FillSBUConfigDetails();
    });

}

///Method to fill currency details 
function FillSBUConfigDetails() {
    //var grdID = $(tr).parents("table:first").attr("id");
    SBUJson = $("#divData").data("SBUData");
    $("[id$=SBUPk]").val(SBUJson[0].BZU_PK);
    $("[id$=SBUName]").val(SBUJson[0].BZU_NAME);
    $("[id$=SBUCode]").val(SBUJson[0].BZU_CODE);
    $("[id$=SBUAddr1]").val(SBUJson[0].BZU_ADDR1);
    $("[id$=SBUAddr2]").val(SBUJson[0].BZU_ADDR2);
    $("[id$=SBUCity]").val(SBUJson[0].BZU_CITY);
    $("[id$=SBUCountry]").val(SBUJson[0].BZU_CNTRY);
    FillState(SBUJson[0].BZU_CNTRY, SBUJson[0].BZU_STATE);
    $("[id$=SBUPhone]").val(SBUJson[0].BZU_PHONE);
    $("[id$=SBUMobile]").val(SBUJson[0].BZU_MOBIL);
    $("[id$=SBUFax]").val(SBUJson[0].BZU_FAX);
    $("[id$=SBUEmail]").val(SBUJson[0].BZU_EMAIL);
    $("[id$=SBUTaxNo]").val(SBUJson[0].BZU_TAX_NO);
    FillCurrency(SBUJson[0].BZU_CURRENCY, SBUJson[0].BZU_PK);
   // $("[id$=BZU_CURRENCY]").val(SBUJson[0].BZU_CURRENCY);
    $("[id$=BZU_REG_NO]").val(SBUJson[0].BZU_REG_NO);
    $("[id$=BZU_GST_NO]").val(SBUJson[0].BZU_GST_NO);
    $("[id$=BZU_FIN_START_DT]").val(SBUJson[0].BZU_FIN_START_DT);
    $("[id$=BZU_FIN_END_DT]").val(SBUJson[0].BZU_FIN_END_DT);
    //$("[id$=BZU_YEAR_CTRL]").val(SBUJson[0].BZU_YEAR_CTRL);
    var rawDate = SBUJson[0].BZU_YEAR_CTRL;

    // Convert the raw date string into a Date object
    var dateObj = new Date(rawDate);

    // Format the date to "dd-M-yy"
    var formattedDate = formatDateToDD_M_YY(dateObj);

    // Set the formatted date value into the input field
    $("[id$=BZU_YEAR_CTRL]").val(formattedDate);

    $("[id$=BZU_THEME]").val(SBUJson[0].BZU_THEME);
    $("select[id$=ddlTheme]").val(SBUJson[0].BZU_THEME);  
//    $("select[id$=ddlTheme]").each(function () {
//        $('option', this).each(function () {
//            if ($.trim($(this).text().toLowerCase()) == $.trim(SBUJson[0].BZU_THEME.toLowerCase())) {
//                $(this).attr('selected', 'selected');
//            };
//        });
//    });

    
}


function SBUActivate() {
    ///<summary> Function Used to activate/inactivate the perticular SBU</summary>

    $.get(SBUConfiguration.SBUConfigInactive + SBUConfiguration.gSBUPk + "&Active=" + SBUConfiguration.Active, function (result) {
        if (parseInt(result) > 0) {
            if (SBUConfiguration.Active == 0)
                GrandScriptUtils.ShowModal(SBUConfiguration.SBUInActiveMessage, SBUConfiguration.MessageBoxTitle);
            else
                GrandScriptUtils.ShowModal(SBUConfiguration.SBUActiveMessage, SBUConfiguration.MessageBoxTitle);
            BindGrid();
            ResetPage();
        }
        else if (parseInt(result) == 0)
            GrandScriptUtils.ShowModal(SBUConfiguration.SBUUsedMessage, SBUConfiguration.MessageBoxTitle);
        else {
            GrandScriptUtils.ShowModal(SBUConfiguration.ActionFailedMessage, SBUConfiguration.MessageBoxTitle);
            ResetPage();
        }
    });
}

//#endregion

//#region----------- Utility Section----------------

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command" optional="true" type="String">
    /// Click OK which which methode perform based on this command
    /// </param>

    switch (command) {
        case SBUConfiguration.SaveCommand:
            $("[id$=CategoryName]").focus();
            break;
        case SBUConfiguration.ActivceCommand:
            SBUActivate();
            break;
    }
}

function GridHandler(tr, command, active) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="String">
    ///     Specific Edit/InActive
    /// </param>

    switch (command.toString()) {
        case SBUConfiguration.ActivceCommand:
            SBUConfiguration.gSBUPk = GrandGrid.Utilities.GetColumnValue(tr, SBUConfiguration.SBUPk, $(tr).parents("table:first").attr("id"));
            SBUConfiguration.Active = active;
            if (SBUConfiguration.Active == 0)
                GrandScriptUtils.ShowModal(SBUConfiguration.InActiveConfirmationMessage, SBUConfiguration.ConfirmationMessage, SBUConfiguration.ActivceCommand, true);
            else
                GrandScriptUtils.ShowModal(SBUConfiguration.ActiveConfirmationMessage, SBUConfiguration.ConfirmationMessage, SBUConfiguration.ActivceCommand, true);
            break;
        case SBUConfiguration.EditCommand:
            ResetPage();
            FillDetails(tr);
            break;
    }
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>

    //    $(document.forms[0]).find("input:not(input[type=submit],input[type=button]),textarea").each(function () {
    //        var idval = $(this).attr("id");
    //        if (idval.search("SBUPk") != -1)
    //            $(this).val('0');
    //        else if (idval.search("UserPk") == -1)
    //            $(this).val("");
    //    });
    //    $(document.forms[0]).find("select").each(function () {
    //        $(this).val($(this).find("option:eq(0)").val()); 
    //    });
    ClearForm()
    $(document.forms[0]).validate().resetForm();
    return false;
}

function ClearForm() {
    $("[id$=SBUPk]").val('0');
    $("[id$=SBUName]").val('');
    $("[id$=SBUCode]").val('');
    $("[id$=SBUAddr1]").val('');
    $("select[id$=BZU_CURRENCY]").val(0);
    $("[id$=SBUAddr2]").val('');
    $("[id$=SBUCity]").val('');
    $("select[id$=SBUCountry]").val(0);
    $("select[id$=SBUState]").val(0);
    $("[id$=SBUPhone]").val('');
    $("[id$=SBUMobile]").val('');
    $("[id$=SBUFax]").val('');
    $("[id$=SBUEmail]").val('');
    $("[id$=SBUTaxNo]").val('');
    $("[id$=BZU_REG_NO]").val('');
    $("[id$=BZU_GST_NO]").val('');
    $("[id$=BZU_FIN_START_DT]").val('');
    $("[id$=BZU_FIN_END_DT]").val(''); 
    $("[id$=BZU_YEAR_CTRL]").val('');
    $("select[id$=ddlTheme]").val(-1);
}

function FillCurrency(currencyID, sbuID) {
    ///<summary>
    ///Used for FillCurrency
    ///</summary>
    // Get id of the Country DropDown
    var ajaxURL = SBUConfiguration.GetCurrency;
    if (sbuID > 0)
        ajaxURL = ajaxURL + "&SBU=" + sbuID;
    var drpID = $("select[id$=BZU_CURRENCY]").attr("id");
    $.get(ajaxURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, currencyID);
    });
}

function FillCountry(countryID) {
    ///<summary>
    ///Used for FillCountry
    ///</summary>
    // Get id of the Country DropDown

    var drpID = $("select[id$=SBUCountry]").attr("id");
    $.get(SBUConfiguration.GetCountry, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, countryID);
    });
}

function FillState(CtyID, stateID) {
    ///<summary>
    ///Used for FillSatte
    ///</summary>
    // Get id of the State DropDown
    var drpID = $("select[id$=SBUState]").attr("id");
    var getCur = SBUConfiguration.GetState + "&CountryID=" + CtyID;
    $.get(getCur, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, stateID);
    });
}




function AfterGridBind() {
    //<summary>Function Used Hide/Show Active Inactive Button</summary>
    var active = 0;
    var isInActive = 1;
    $("#grdSBUList tr:has(td)").each(function () {
        active = GrandGrid.Utilities.GetColumnValue($(this), SBUConfiguration.SBUActive, $(this).parents("table:first").attr("id"));
        isInActive = GrandGrid.Utilities.GetColumnValue($(this), SBUConfiguration.ISINACTIVE, $(this).parents("table:first").attr("id"));
        if (active == "1")
            $(this).find("input[type=image]:eq(2)").hide();
        else
            $(this).find("input[type=image]:eq(1)").hide();
        if (isInActive == "0")
            $(this).find("input[type=image]:not(input[id$=imbEdit])").hide();
    });
}

function RedirectToInbox() {
    window.location = SBUConfiguration.Inbox;
    return false;
}

function DateInit() {
    //<summary>function used to make datepicker</summary>
    GrandScriptUtils.DatePickerMonthOnly("BZU_FIN_START_DT", "dd-M");
    GrandScriptUtils.DatePickerMonthOnly("BZU_FIN_END_DT", "dd-M");
    GrandScriptUtils.DatePickerCommon("BZU_YEAR_CTRL", "dd-M-yy");
    //GrandScriptUtils.DatePicker("BZU_YEAR_CTRL", "dd-M-yy", false);
}

function formatDateToDD_M_YY(date) {
    var day = date.getDate().toString().padStart(2, '0');          // Get day and pad to 2 digits
    var month = date.toLocaleString('default', { month: 'short' }); // Get short month name
    var year = date.getFullYear();                                  // Get full year

    return `${day}-${month}-${year}`;
}
//#endregion