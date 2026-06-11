/// <reference path="../../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../../../GrandScriptUtils.js" />
/// <reference path="../../../GrandGridMulti.js" />
/// <reference path="../../../jquery/json2.js" />


var DepartmentConfig = {
    GetSBUList: "SBUConfiguration.do?Action=GetAllSBUList",
    SaveSbuDeptConfig: "DepartmentConfig.do?Action=SaveSBUDepartmentConfig",
    GetSbuDeptConfig: "DepartmentConfig.do?Action=GetSBUDepartmentConfig&SBUPk=",
    
    // Messages 

    MessageBoxTitle: "Translate(Information)",
    DeptSaveMessage: "Translate(DeptmentConfigSavedSuccesfully)",
    ActionFailedMessage: "Translate(CannotInactive)",
    OneDeptActive: "Translate(OneDeptActive)",

    // Fields

    DeptPK: "DPT_PK",
    Active: "DPT_ACTIVE"
}

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
    PageInit();
});

function PageInit() {
    //<summary>Function to initialize the page</summary>

    FillSBUCombo();
    BindGrid(0);
    $("[id$=SBU]").focus();
}

//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=SBU]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectSBU)" }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
}

//#endregion

//#region----------- Core Section----------------

function FillSBUCombo() {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=SBU]").attr("id"); // Get id of the SBU DropDown
    $.get(DepartmentConfig.GetSBUList, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function BindGrid(sbuPK) {
    //<summary>Function to bind sbu details</summary>

    var ajaxUrl = DepartmentConfig.GetSbuDeptConfig + sbuPK;
    $("#grdDeptDetails").removeAttr("ajaxurl")
    $("#grdDeptDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDeptDetails");
    GrandGrid.MakeGrid($("#grdDeptDetails"));
    return false;
}

function SavePage() {
    //<summary>Function used to save dept config agianst sbu</summary>

    AddValidations();
    if ($(document.forms[0]).valid()) {
        var DepartmentList = SetJsonDepartment();
        $("[id$=DepartmentActive]").val(DepartmentList);
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        if (DepartmentList) {
            $.post(DepartmentConfig.SaveSbuDeptConfig, jSonString, function (result) {
                if (parseInt(result) > 0) {
                    GrandScriptUtils.ShowModal(DepartmentConfig.DeptSaveMessage, DepartmentConfig.MessageBoxTitle);
                    ResetPage();
                }
                else
                    GrandScriptUtils.ShowModal(DepartmentConfig.ActionFailedMessage, DepartmentConfig.MessageBoxTitle);
            });
        }
        else {
            GrandScriptUtils.ShowModal(DepartmentConfig.OneDeptActive, DepartmentConfig.MessageBoxTitle);
        }
    }
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        if (idval.search("UserPk") == -1)
            $(this).val("");
    });
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    BindGrid(0);
    $(document.forms[0]).validate().resetForm();
    return false;
}

//#endregion

//#region----------- Utility Section----------------

function SetJsonDepartment() {
    //<summary>Function used to set the dapartement active/inactive </summary>

    var deptPK = 0;
    var jsonStringify = new Array();
    var isChecked = false;
    $("#grdDeptDetails tr:has(td)").each(function () {
        deptPK = GrandGrid.Utilities.GetColumnValue($(this), DepartmentConfig.DeptPK, $(this).parents("table:first").attr("id"));
        if ($(this).find("td:first input[type=checkbox]").attr("checked")) {
            jsonStringify.push({ Department: deptPK, Active: 1 });
            isChecked = true;
        }
        else
            jsonStringify.push({ Department: deptPK, Active: 0 });
    });
    return isChecked == false ? isChecked : JSON.stringify(jsonStringify);
}

function AfterGridBind() {
    //<summary>Function used to set the dept as checked based on the active </summary>

    var active = 0;
    $("#grdDeptDetails tr:has(td)").each(function () {
        active = GrandGrid.Utilities.GetColumnValue($(this), DepartmentConfig.Active, $(this).parents("table:first").attr("id"));
        if (active == "1") {
            $(this).find("td:first input[type=checkbox]").attr("checked", "checked");
        }
    });
}

//#endregion