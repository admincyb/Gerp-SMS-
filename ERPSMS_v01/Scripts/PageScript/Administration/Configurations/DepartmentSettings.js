/// <reference path="../../../GrandGridMulti.js" />
/// <reference path="../../../GrandScriptUtils.js" />


var DepartmentSettingsConfig = {

    SaveConfigValue: "DepartmentSettingsConfig.do?Action=SaveConfigValue",
    GetConfigValue: "DepartmentSettingsConfig.do?Action=GetConfigValue&SBUPk=",
    DeleteConfigValue: "DepartmentSettingsConfig.do?Action=DeleteConfigValue&SBUPk=",
    GetSBUList: "SBUConfiguration.do?Action=GetAllSBUList",
    GetDeptList: "DepartmentConfig.do?Action=GetSBUDeptList&SBUPk=",

    // Fields
    CFG_BIZUNIT: "CFG_BIZUNIT",
    CFG_DEPT: "CFG_DEPT",
    CFG_SL: "CFG_SL",
    CFG_PK: "CFG_PK",
    CFG_NAME: "CFG_NAME",
    CFG_DATA_TYPE: "CFG_DATA_TYPE",
    CFG_VALUE: "CFG_VALUE",

    //Messages
    Confirmation: "Translate(Confirmation)",
    NameAlreadyExists: "Translate(NameAlreadyExists)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    DeleteAllMassage: "Translate(DeleteAllDepartments)",
    MessageBoxTitle: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    SavedSuccessfully: "Translate(DeptSettingSavedSuccessfully)",
    AddDeptSettingValue: "Translate(PleaseAddDeptSettingValues)",
    CantAddDiffType: "Translate(CantAddDiffType)",

    // Command 
    String: "String",
    Float: "Float",
    DateTime: "DateTime",
    Boolean: "Boolean",
    EDIT: "edit",
    DELETE: "delete",
    DELETEALL: "DELETEALL",

    // Globel Declaration
    ConfigListArray: new Array(),
    ConfigObject: new Object(),
    ConfigListObject: new Object(),
    SLNo: 0,
    tdset: "",
    DeletePk: 0
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

    GrandGrid.MakeGrid($("#grdConfigList"), 0, DepartmentSettingsConfig.ConfigListArray);
    DepartmentSettingsConfig.ConfigObject = $.parseJSON($("[id$=CFG_LIST]").val());
    $("#divData").data("ConfigData", DepartmentSettingsConfig.ConfigObject);

    FillSBUCombo(0);
    FillDeptCombo(0);
    GetConfigListValues();
    $("[id$=SBU]").focus();
}

//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=SBU]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectSBU)"  }
    });
    $("[id$=Dept]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectDept)" }
    });
    $("[id$=ConfigName]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Translate(PleaseEnterName)" }
    });
    switch ($("[id$=ConfigType]").val()) {
        case DepartmentSettingsConfig.String:
            $("[id$=ConfigValue]").rules("add", {
                required: true,
                maxlength: 100,
                messages: { required: "Translate(PleaseEnterStringValue)" }
            });
            break;
        case DepartmentSettingsConfig.DateTime:
            $("[id$=ConfigValue]").rules("add", {
                required: true,
                date: true,
                messages: { required: "Translate(PleaseEnterDateTimeValue)" }
            });
            break;
        case DepartmentSettingsConfig.Float:
            $("[id$=ConfigValue]").rules("add", {
                ThreeDecimal: true,
                required: true,
                messages: { required: "Translate(EnterNumeric)" }
            });
            break;
    }
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $("[id$=SBU]").rules("remove");
    $("[id$=Dept]").rules("remove");
    $("[id$=ConfigName]").rules("remove");
    $("[id$=ConfigValue]").rules("remove");
}

//#endregion

//#region----------- Core Section----------------

function FillSBUCombo(sVal) {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=SBU]").attr("id"); // Get id of the SBU DropDown
    $.get(DepartmentSettingsConfig.GetSBUList, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}

function FillDeptCombo(val, sVal, isEdit) {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=Dept]").attr("id"); // Get id of the Dept DropDown
    if (!isEdit)
        AddNew();
    $.get(DepartmentSettingsConfig.GetDeptList + val, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}

function AddConfigDetails() {
    //<summary>function used to add products details to order</summary>

    RemoveValidations();
    AddValidations();
    if ($(document.forms[0]).valid()) {
        DepartmentSettingsConfig.ConfigObject = new Object();
        DepartmentSettingsConfig.ConfigObject = $("#divData").data("ConfigData");
        DepartmentSettingsConfig.ConfigListArray = new Array();
        DepartmentSettingsConfig.ConfigListArray = DepartmentSettingsConfig.ConfigObject.CFG_LIST;
        DepartmentSettingsConfig.ConfigListObject = new Object();
        if (!CheckDiffTypeExists()) {
            GrandScriptUtils.ShowModal(DepartmentSettingsConfig.CantAddDiffType, DepartmentSettingsConfig.MessageBoxTitle);
            return false;
        }
        if (!CheckConfigNameExists()) {
            GrandScriptUtils.ShowModal(DepartmentSettingsConfig.NameAlreadyExists, DepartmentSettingsConfig.MessageBoxTitle);
            return false;
        }
        
        DepartmentSettingsConfig.ConfigListObject.CFG_BIZUNIT = $("[id$=SBU]").val();
        DepartmentSettingsConfig.ConfigListObject.CFG_DEPT = $("[id$=Dept]").val();
        DepartmentSettingsConfig.ConfigListObject.CFG_PK = $("[id$=ConfigPk]").val();
        DepartmentSettingsConfig.ConfigListObject.CFG_NAME = $("[id$=ConfigName]").val();
        DepartmentSettingsConfig.ConfigListObject.CFG_DATA_TYPE = $("[id$=ConfigType]").val();
        DepartmentSettingsConfig.ConfigListObject.CFG_VALUE = $("[id$=ConfigType]").val() == DepartmentSettingsConfig.Boolean ? ($("[id$=ConfigValue]").attr("checked") == true ? "true" : "false") : $("[id$=ConfigValue]").val();
        DepartmentSettingsConfig.ConfigListObject.CFG_MOD_BY = $("[id$=UserPk]").val();
        if (parseInt(DepartmentSettingsConfig.SLNo) == 0) {
            DepartmentSettingsConfig.ConfigListObject.CFG_SL = DepartmentSettingsConfig.ConfigListArray.length + 1;
            DepartmentSettingsConfig.ConfigListArray.push(DepartmentSettingsConfig.ConfigListObject);
        }
        ClearConfigValueDetails();
        $("#divData").data("ConfigData", DepartmentSettingsConfig.ConfigObject);
        GrandGrid.MakeGrid($("#grdConfigList"), 0, DepartmentSettingsConfig.ConfigListArray);

    }
    return false;
}

function SavePage() {
    //<summary>save dispersion to the database</summary>

    DepartmentSettingsConfig.ConfigObject = new Object();
    DepartmentSettingsConfig.ConfigObject = $("#divData").data("ConfigData");
    if (DepartmentSettingsConfig.ConfigObject.CFG_LIST.length > 0) {
        $("[id$=CFG_LIST]").val(JSON.stringify(DepartmentSettingsConfig.ConfigObject.CFG_LIST));
        var jSonString = GrandScriptUtils.FormToJsonString("divResult");
        $.post(DepartmentSettingsConfig.SaveConfigValue, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(DepartmentSettingsConfig.SavedSuccessfully, DepartmentSettingsConfig.MessageBoxTitle);
                ResetPage();
            }
            else {
                GrandScriptUtils.ShowModal(DepartmentSettingsConfig.ActionFailedMessage, DepartmentSettingsConfig.MessageBoxTitle);
                ResetPage();
            }
        });
    }
    else {
        GrandScriptUtils.ShowModal(DepartmentSettingsConfig.DeleteAllMassage, DepartmentSettingsConfig.MessageBoxTitle, DepartmentSettingsConfig.DELETEALL, true);
    }
    return false;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for Catch all the grid events in this function </summary>

    switch (command.toString().toLowerCase()) {
        case DepartmentSettingsConfig.DELETE:
            DepartmentSettingsConfig.DeletePk = GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_SL, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(DepartmentSettingsConfig.DeleteConfirmMsg, DepartmentSettingsConfig.Confirmation, DepartmentSettingsConfig.DELETE, true);
            return false;
            break;
        case DepartmentSettingsConfig.EDIT:
            FillConfigValueDetails(tr);
            return false;
            break;
    }
}

function DeleteConfigValue() {
    ///<summary>For delete the item in the grid - Details</summary>

    DepartmentSettingsConfig.ConfigObject = new Object();
    DepartmentSettingsConfig.ConfigObject = $("#divData").data("ConfigData");
    for (var i in DepartmentSettingsConfig.ConfigObject.CFG_LIST) {
        if (DepartmentSettingsConfig.ConfigObject.CFG_LIST[i].CFG_SL == DepartmentSettingsConfig.DeletePk) {
            DepartmentSettingsConfig.ConfigObject.CFG_LIST.splice(i, 1);
            break;
        }
    }
    DepartmentSettingsConfig.DeletePk = 0;
    $("#divData").data("ConfigData", DepartmentSettingsConfig.ConfigObject);
    GrandGrid.MakeGrid($("#grdConfigList"), 0, DepartmentSettingsConfig.ConfigObject.CFG_LIST);
    if (DepartmentSettingsConfig.ConfigObject.CFG_LIST.length == 0) {
        $(DepartmentSettingsConfig.tdset).insertAfter($("#ConfigList").find("tr:eq(0)"));
        $("#ConfigList").css({ "visibility": "visible", "display": "block" });
    }
    ClearConfigValueDetails();
}

function DeleteAllConfigValue() {
    ///<summary>Function used for delete all item</summary>

    $.get(DepartmentSettingsConfig.DeleteConfigValue + $("[id$=SBU]").val() + "&DeptPK=" + $("[id$=Dept]").val(), function (result) {
        if (parseInt(result) > 0) {
            GrandScriptUtils.ShowModal(DepartmentSettingsConfig.SavedSuccessfully, DepartmentSettingsConfig.MessageBoxTitle);
            ResetPage();
        }
        else {
            GrandScriptUtils.ShowModal(DepartmentSettingsConfig.ActionFailedMessage, DepartmentSettingsConfig.MessageBoxTitle);
            ResetPage();
        }
    });
}

function FillConfigValueDetails(tr) {
    //<summary>Function used to Config Value for edit</summary>

    var grdID = $(tr).parents("table:first").attr("id");
    DepartmentSettingsConfig.SLNo = GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_SL, grdID);
    FillSBUCombo(GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_BIZUNIT, grdID));
    FillDeptCombo(GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_BIZUNIT, grdID), GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_DEPT, grdID), true);
    $("[id$=ConfigPk]").val(GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_PK, grdID));
    $("[id$=ConfigName]").val(GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_NAME, grdID));
    $("[id$=ConfigType]").val(GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_DATA_TYPE, grdID));
    $("[id$=ConfigValue]").val(GrandGrid.Utilities.GetColumnValue(tr, DepartmentSettingsConfig.CFG_VALUE, grdID));
    SetValueUtitlity($("[id$=ConfigType]").val(), $("[id$=ConfigValue]").val());
    $("[id$=ConfigName]").focus();
}

function GetConfigListValues() {
    //<summary>Function used to get the list Config Value for edit</summary>

    $("[id$=ConfigType]").val(DepartmentSettingsConfig.String);
    SetValueUtitlity(DepartmentSettingsConfig.String);
    $.get(DepartmentSettingsConfig.GetConfigValue + $("[id$=SBU]").val() + "&DeptPK=" + $("[id$=Dept]").val(), function (result) {
        DepartmentSettingsConfig.ConfigObject.CFG_LIST = new Array();
        if (result.CFG_LIST != undefined) {
            if (!$.isArray(result.CFG_LIST))
                DepartmentSettingsConfig.ConfigObject.CFG_LIST.push(result.CFG_LIST);
            else
                DepartmentSettingsConfig.ConfigObject.CFG_LIST = result.CFG_LIST;
        }
        else {
            $(DepartmentSettingsConfig.tdset).insertAfter($("#ConfigList").find("tr:eq(0)"));
            $("#ConfigList").css({ "visibility": "visible", "display": "block" });
        }
        $("#divData").data("ConfigData", DepartmentSettingsConfig.ConfigObject);
        GrandGrid.MakeGrid($("#grdConfigList"), 0, DepartmentSettingsConfig.ConfigObject.CFG_LIST);
    });
}


function ClearConfigValueDetails() {
    //<summary>Function used to clear defalut list fields</summary>

    DepartmentSettingsConfig.SLNo = 0;
    $("[id$=ConfigPk]").val(0);
    $("[id$=ConfigName]").val("");
    SetValueUtitlity(DepartmentSettingsConfig.String);
    $("[id$=ConfigType]").val(DepartmentSettingsConfig.String);
    $("[id$=ConfigValue]").val("");
    $("[id$=ConfigName]").focus();
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    RemoveValidations();
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        if (idval.search("ConfigPk") != -1)
            $(this).val("0");
        else if (idval.search("UserPk") == -1)
            $(this).val("");
    });
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    FillDeptCombo(0, 0);
    AddNew();
    $("[id$=SBU]").focus();
    return false;
}

//#endregion

//#region----------- Utility Section----------------

function CheckDiffTypeExists() {
    //<summary>function used to check diff type.</summary>

    var flag = true;
    if (parseInt(DepartmentSettingsConfig.SLNo) == 0) {
        for (var i in DepartmentSettingsConfig.ConfigListArray) {
            if (DepartmentSettingsConfig.ConfigListArray[i].CFG_DATA_TYPE != $("[id$=ConfigType]").val()) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in DepartmentSettingsConfig.ConfigListArray) {
            if (DepartmentSettingsConfig.ConfigListArray[i].CFG_DATA_TYPE != $("[id$=ConfigType]").val() && parseInt(DepartmentSettingsConfig.SLNo) != DepartmentSettingsConfig.ConfigListArray[i].CFG_SL) {
                flag = false;
                break;
            }
        }
    }
    return flag;
}

function CheckConfigNameExists() {
    //<summary>function used to check whether name already exists.</summary>

    var flag = true;
    if (parseInt(DepartmentSettingsConfig.SLNo) == 0) {
        for (var i in DepartmentSettingsConfig.ConfigListArray) {
            if (DepartmentSettingsConfig.ConfigListArray[i].CFG_NAME === $("[id$=ConfigName]").val()) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in DepartmentSettingsConfig.ConfigListArray) {
            if (DepartmentSettingsConfig.ConfigListArray[i].CFG_NAME === $("[id$=ConfigName]").val() && parseInt(DepartmentSettingsConfig.SLNo) != DepartmentSettingsConfig.ConfigListArray[i].CFG_SL) {
                flag = false;
                break;
            }
            if (parseInt(DepartmentSettingsConfig.SLNo) == DepartmentSettingsConfig.ConfigListArray[i].CFG_SL)
                DepartmentSettingsConfig.ConfigListObject = DepartmentSettingsConfig.ConfigListArray[i];
        }
    }
    return flag;
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdConfigList") {
        if (DepartmentSettingsConfig.tdset == "")
            DepartmentSettingsConfig.tdset = $("#ConfigList").find("tr:eq(1)");
        $("#ConfigList").css({ "visibility": "hidden", "display": "none" });
        $(DepartmentSettingsConfig.tdset).insertBefore($("#grdConfigList").find("tr:eq(1)"));
    }
}

function SetValueUtitlity(tVal, iVal) {
    switch (tVal) {
        case DepartmentSettingsConfig.String:
            $("[id$=ConfigValue]").replaceWith("<input type=\"input\" tabindex=5 id=\"" + $("[id$=ConfigValue]").attr("id") + "\"  id=\"" + $("[id$=ConfigValue]").attr("id") + "\" " + (iVal == undefined ? "" : "value=" + iVal + "") + " />");
            break;
        case DepartmentSettingsConfig.Float:
            $("[id$=ConfigValue]").replaceWith("<input type=\"input\" tabindex=5 id=\"" + $("[id$=ConfigValue]").attr("id") + "\"  id=\"" + $("[id$=ConfigValue]").attr("id") + "\" " + (iVal == undefined ? "" : "value=" + iVal + "") + " />");
            break;
        case DepartmentSettingsConfig.DateTime:
            $("[id$=ConfigValue]").replaceWith("<input type=\"input\" tabindex=5 id=\"" + $("[id$=ConfigValue]").attr("id") + "\"  id=\"" + $("[id$=ConfigValue]").attr("id") + "\" " + (iVal == undefined ? "" : "value=" + iVal + "") + " />");
            GrandScriptUtils.DatePicker("ConfigValue");
            break;
        case DepartmentSettingsConfig.Boolean:
            $("[id$=ConfigValue]").replaceWith("<input type=\"checkbox\" tabindex=5 id=\"" + $("[id$=ConfigValue]").attr("id") + "\" " + ((iVal == undefined || iVal == "false") ? "" : "checked=\"checked\"") + "  />");
            break;
    }
}

function AddNew() {
    DepartmentSettingsConfig.ConfigObject = new Object();
    DepartmentSettingsConfig.ConfigObject.CFG_LIST = new Array();
    $("#divData").data("ConfigData", DepartmentSettingsConfig.ConfigObject);
    GrandGrid.MakeGrid($("#grdConfigList"), 0, DepartmentSettingsConfig.ConfigObject.CFG_LIST);
    $(DepartmentSettingsConfig.tdset).insertAfter($("#ConfigList").find("tr:eq(0)"));
    $("#ConfigList").css({ "visibility": "visible", "display": "block" });
    SetValueUtitlity(DepartmentSettingsConfig.String);
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case DepartmentSettingsConfig.DELETE:
            DeleteConfigValue();
            break;
        case DepartmentSettingsConfig.DELETEALL:
            DeleteAllConfigValue();
            break;
    }
}


//#endregion