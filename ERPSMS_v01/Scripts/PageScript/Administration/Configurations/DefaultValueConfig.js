/// <reference path="../../../GrandGridMulti.js" />
/// <reference path="../../../GrandScriptUtils.js" />


var DefaultValueConfig = {

    SaveDefaultValue: "DefaultValueConfig.do?Action=SaveDefaultValueConfig",
    GetDefaultValue: "DefaultValueConfig.do?Action=GetDefaultValueConfig&SBUPk=",
    DeleteDefaultValue: "DefaultValueConfig.do?Action=DeleteDefaultValueConfig&SBUPk=",
    GetSBUList: "SBUConfiguration.do?Action=GetAllSBUList",
    GetDeptList: "DepartmentConfig.do?Action=GetSBUDeptList&SBUPk=",
    GetGroupAutoURL: "DefaultValueConfig.do?Action=GetAllGroupList",

    // Fields
    DFT_BIZUNIT: "DFT_BIZUNIT",
    DFT_DEPT: "DFT_DEPT",
    DFT_GROUP: "DFT_GROUP",
    DFT_SL: "DFT_SL",
    DFT_PK: "DFT_PK",
    DFT_NAME: "DFT_NAME",
    DFT_TYPE: "DFT_TYPE",
    DFT_VALUE: "DFT_VALUE",
    DFT_IS_DEFAULT: "DFT_IS_DEFAULT",
    DFT_CAN_DEL: "DFT_CAN_DEL",
    DFT_CAN_MDFY: "DFT_CAN_MDFY",

    //Messages
    Confirmation: "Translate(Confirmation)",
    NameAlreadyExists: "Translate(NameAlreadyExists)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    MessageBoxTitle: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    ReferenceMessage: "Translate(CannotDelete)",
    SavedSuccessfully: "Translate(DefaultValueSavedSuccessfully)",
    DeleteAllMassage: "Translate(DeleteAllGroup)",
    CantAddDiffGroup: "Translate(CannotAddDiffGroup)",
    CantAddDiffType: "Translate(CantAddDiffType)",
    IsDefaultExists: "Translate(IsDefaultExists)",


    // Command 
    String: "String",
    Float: "Float",
    DateTime: "DateTime",
    Boolean: "Boolean",
    EDIT: "edit",
    DELETE: "delete",
    DELETEALL: "DELETEALL",

    // Globel Declaration
    DefaultListArray: new Array(),
    DefaultObject: new Object(),
    DefaultListObject: new Object(),
    SLNo: 0,
    tdset: "",
    SBUPK: 0,
    DeptPK: 0,
    Group: "",
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

    GrandGrid.MakeGrid($("#grdDefaultList"), 0, DefaultValueConfig.DefaultListArray);
    DefaultValueConfig.DefaultObject = $.parseJSON($("[id$=DFT_LIST]").val());
    $("#divData").data("DefaultData", DefaultValueConfig.DefaultObject);

    FillSBUCombo(0);
    FillDeptCombo(0, 0);
    MakeGroupAutoComplete();
    $("[id$=SBU]").focus();
    $("[id$=btnSave]").hide();
}

//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=SBU]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectSBU)" }
    });
    $("[id$=Group]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Translate(PleaseEnterGroup)" }
    });
    $("[id$=DefaultName]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Translate(PleaseEnterName)" }
    });
    switch ($("[id$=DefaultType]").val()) {
        case DefaultValueConfig.String:
            $("[id$=DefaultValue]").rules("add", {
                required: true,
                maxlength: 100,
                messages: { required: "Translate(PleaseEnterStringValue)" }
            });
            break;
        case DefaultValueConfig.DateTime:
            $("[id$=DefaultValue]").rules("add", {
                required: true,
                date: true,
                messages: { required: "Translate(PleaseEnterDateTimeValue)" }
            });
            break;
        case DefaultValueConfig.Float:
            $("[id$=DefaultValue]").rules("add", {
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
    $("[id$=Group]").rules("remove");
    $("[id$=DefaultName]").rules("remove");
    $("[id$=DefaultValue]").rules("remove");
}

//#endregion

//#region----------- Core Section----------------

function FillSBUCombo(sVal) {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=SBU]").attr("id"); // Get id of the SBU DropDown
    $.get(DefaultValueConfig.GetSBUList, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, sVal);
    });
}

function FillDeptCombo(val, sVal) {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=Dept]").attr("id"); // Get id of the Dept DropDown
    $.get(DefaultValueConfig.GetDeptList + val, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, sVal, true);
    });
}

function MakeGroupAutoComplete() {
    //<summary> Function Used to make group field as auto complete </summary>

    GrandScriptUtils.MakeAutoComplete("Group", DefaultValueConfig.GetGroupAutoURL, false, true, false, "Dept");
}

function AddDefaultDetails() {
    //<summary>function used to add products details to order</summary>

    RemoveValidations();
    AddValidations();
    if ($(document.forms[0]).valid()) {
        DefaultValueConfig.DefaultObject = new Object();
        DefaultValueConfig.DefaultObject = $("#divData").data("DefaultData");
        DefaultValueConfig.DefaultListArray = new Array();
        DefaultValueConfig.DefaultListArray = DefaultValueConfig.DefaultObject.DFT_LIST;
        DefaultValueConfig.DefaultListObject = new Object();
        if (!CheckDiffGroupExists()) {
            GrandScriptUtils.ShowModal(DefaultValueConfig.CantAddDiffGroup, DefaultValueConfig.MessageBoxTitle);
            return false;
        }
        if (!CheckDiffTypeExists()) {
            GrandScriptUtils.ShowModal(DefaultValueConfig.CantAddDiffType, DefaultValueConfig.MessageBoxTitle);
            return false;
        }
        if (!CheckDefaultNameExists()) {
            GrandScriptUtils.ShowModal(DefaultValueConfig.NameAlreadyExists, DefaultValueConfig.MessageBoxTitle);
            return false;
        }
        if (!CheckIsDefaultExists()) {
            GrandScriptUtils.ShowModal(DefaultValueConfig.IsDefaultExists, DefaultValueConfig.MessageBoxTitle);
            return false;
        }
        DefaultValueConfig.DefaultListObject.DFT_BIZUNIT = $("[id$=SBU]").val();
        DefaultValueConfig.DefaultListObject.DFT_DEPT = $("[id$=Dept]").val();
        DefaultValueConfig.DefaultListObject.DFT_GROUP = $("[id$=Group]").val();
        DefaultValueConfig.DefaultListObject.DFT_PK = $("[id$=DefaultPk]").val();
        DefaultValueConfig.DefaultListObject.DFT_NAME = $("[id$=DefaultName]").val();
        DefaultValueConfig.DefaultListObject.DFT_TYPE = $("[id$=DefaultType]").val();
        DefaultValueConfig.DefaultListObject.DFT_VALUE = $("[id$=DefaultType]").val() == DefaultValueConfig.Boolean ? ($("[id$=DefaultValue]").attr("checked") == true ? "true" : "false") : $("[id$=DefaultValue]").val();
        DefaultValueConfig.DefaultListObject.DFT_DEFAULT = ($("[id$=SetAsDefault]").attr("checked") ? "true" : "false");
        DefaultValueConfig.DefaultListObject.DFT_IS_DEFAULT = ($("[id$=SetAsDefault]").attr("checked") ? 1 : 0);
        DefaultValueConfig.DefaultListObject.DFT_MOD_BY = $("[id$=UserPk]").val();
        if (parseInt(DefaultValueConfig.SLNo) == 0) {
            DefaultValueConfig.DefaultListObject.DFT_SL = DefaultValueConfig.DefaultListArray.length + 1;
            DefaultValueConfig.DefaultListArray.push(DefaultValueConfig.DefaultListObject);
        }
        ClearDefaultValueDetails();
        $("#divData").data("DefaultData", DefaultValueConfig.DefaultObject);
        GrandGrid.MakeGrid($("#grdDefaultList"), 0, DefaultValueConfig.DefaultListArray);
    }
    return false;
}

function SavePage() {
    //<summary>save dispersion to the database</summary>

    DefaultValueConfig.DefaultObject = new Object();
    DefaultValueConfig.DefaultObject = $("#divData").data("DefaultData");
    if (DefaultValueConfig.DefaultObject.DFT_LIST.length > 0) {
        $("[id$=DFT_LIST]").val(JSON.stringify(DefaultValueConfig.DefaultObject.DFT_LIST));
        var jSonString = GrandScriptUtils.FormToJsonString("divResult");
        $.post(DefaultValueConfig.SaveDefaultValue, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(DefaultValueConfig.SavedSuccessfully, DefaultValueConfig.MessageBoxTitle);
                ResetPage();
                RemoveValidations();
            }
            else {
                GrandScriptUtils.ShowModal(DefaultValueConfig.ActionFailedMessage, DefaultValueConfig.MessageBoxTitle);
                ResetPage();
                RemoveValidations();
            }
        });
    }
    else {
        GrandScriptUtils.ShowModal(DefaultValueConfig.DeleteAllMassage, DefaultValueConfig.MessageBoxTitle, DefaultValueConfig.DELETEALL, true);
    }
    return false;
}

function DeleteDefaultValue() {
    ///<summary>For delete the item in the grid - Details</summary>

    DefaultValueConfig.DefaultObject = new Object();
    DefaultValueConfig.DefaultObject = $("#divData").data("DefaultData");
    for (var i in DefaultValueConfig.DefaultObject.DFT_LIST) {
        if (DefaultValueConfig.DefaultObject.DFT_LIST[i].DFT_SL == DefaultValueConfig.DeletePk) {
            DefaultValueConfig.DefaultObject.DFT_LIST.splice(i, 1);
            break;
        }
    }
    DefaultValueConfig.DeletePk = 0;
    $("#divData").data("DefaultData", DefaultValueConfig.DefaultObject);
    GrandGrid.MakeGrid($("#grdDefaultList"), 0, DefaultValueConfig.DefaultObject.DFT_LIST);
    if (DefaultValueConfig.DefaultObject.DFT_LIST.length == 0) {
        $(DefaultValueConfig.tdset).insertAfter($("#DefaultList").find("tr:eq(0)"));
        $("#DefaultList").css({ "visibility": "visible", "display": "inline" });
    }
    ClearDefaultValueDetails();
}

function DeleteAllDefaultValue() {
    ///<summary>Function used for delete all item</summary>

    $.get(DefaultValueConfig.DeleteDefaultValue + DefaultValueConfig.SBUPK + "&DeptPK=" + DefaultValueConfig.DeptPK + "&Group=" + $("[id$=Group]").val(), function (result) {
        if (parseInt(result) > 0) {
            GrandScriptUtils.ShowModal(DefaultValueConfig.SavedSuccessfully, DefaultValueConfig.MessageBoxTitle);
            ResetPage();
        } 
        else
            GrandScriptUtils.ShowModal(DefaultValueConfig.ActionFailedMessage, DefaultValueConfig.MessageBoxTitle);
    });
}

function FillDefaultValueDetails(tr) {
    //<summary>Function used to Default Value for edit</summary>

    var grdID = $(tr).parents("table:first").attr("id");
    DefaultValueConfig.SLNo = GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_SL, grdID);
    FillSBUCombo(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_BIZUNIT, grdID));
    FillDeptCombo(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_BIZUNIT, grdID), GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_DEPT, grdID));
    $("[id$=Group]").val(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_GROUP, grdID));
    $("[id$=DefaultPk]").val(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_PK, grdID));
    $("[id$=DefaultName]").val(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_NAME, grdID));
    $("[id$=DefaultType]").val(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_TYPE, grdID));
    $("[id$=DefaultValue]").val(GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_VALUE, grdID));
    SetValueUtitlity($("[id$=DefaultType]").val(), $("[id$=DefaultValue]").val());
    $("[id$=SetAsDefault]").attr("checked", ((GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_IS_DEFAULT, grdID) == 1) ? true : false));
    $("[id$=DefaultName]").focus();
}

function GetDefaultListValues() {
    //<summary>Function used to get the list Default Value for edit</summary>

    $("[id$=DefaultType]").val(DefaultValueConfig.String);
    SetValueUtitlity(DefaultValueConfig.String);
    DefaultValueConfig.SBUPK = $("[id$=SBU]").val();
    DefaultValueConfig.DeptPK = $("[id$=Dept]").val();
    DefaultValueConfig.Group = $("[id$=Group]").val();
    $.get(DefaultValueConfig.GetDefaultValue + DefaultValueConfig.SBUPK + "&DeptPK=" + DefaultValueConfig.DeptPK + "&Group=" + DefaultValueConfig.Group, function (result) {
        DefaultValueConfig.DefaultObject.DFT_LIST = new Array();
        if (result.DFT_LIST != undefined) {
            $("[id$=btnSave]").show();
            if (!$.isArray(result.DFT_LIST))
                DefaultValueConfig.DefaultObject.DFT_LIST.push(result.DFT_LIST);
            else
                DefaultValueConfig.DefaultObject.DFT_LIST = result.DFT_LIST;
            $("[id$=Group]").val(DefaultValueConfig.DefaultObject.DFT_LIST[0].DFT_GROUP)
        }
        else {
            $(DefaultValueConfig.tdset).insertAfter($("#DefaultList").find("tr:eq(0)"));
            $("#DefaultList").css({ "visibility": "visible", "display": "inline" });
        }
        $("#divData").data("DefaultData", DefaultValueConfig.DefaultObject);
        GrandGrid.MakeGrid($("#grdDefaultList"), 0, DefaultValueConfig.DefaultObject.DFT_LIST);
    });
}

function AfterAutoCompleteSelect() {
    //<summary>Function used to fire the event when select the auto complete and listing based on this group</summary>

    GetDefaultListValues();
}

function ClearDefaultValueDetails() {
    //<summary>Function used to clear defalut list fields</summary>

    DefaultValueConfig.SLNo = 0;
    $("[id$=DefaultPk]").val(0);
    $("[id$=DefaultName]").val("");
    $("[id$=SetAsDefault]").attr("checked", false);
    SetValueUtitlity(DefaultValueConfig.String);
    $("[id$=DefaultType]").val(DefaultValueConfig.String);
    $("[id$=DefaultName]").focus();
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        if (idval.search("DefaultPk") != -1)
            $(this).val("0");
//        else if (idval.search("UserPk") == -1)
//            $(this).val("");
    });
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });

    FillDeptCombo(0, 0);
    AddNew();
    $("[id$=SetAsDefault]").attr("checked", false);
    $("[id$=SBU]").focus();
    $("[id$=btnSave]").hide();
    $("#divHeader").show();
//    $("[id$=btnReset]").text = "Refresh";

    return false;
}

//#endregion

//#region----------- Utility Section----------------

function CheckDiffGroupExists() {
    //<summary>function used to check diff group.</summary>

    var flag = true;
    if (parseInt(DefaultValueConfig.SLNo) == 0) {
        for (var i in DefaultValueConfig.DefaultListArray) {
            if (DefaultValueConfig.DefaultListArray[i].DFT_GROUP != $("[id$=Group]").val()) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in DefaultValueConfig.DefaultListArray) {
            if (DefaultValueConfig.DefaultListArray[i].DFT_GROUP != $("[id$=Group]").val() && parseInt(DefaultValueConfig.SLNo) != DefaultValueConfig.DefaultListArray[i].DFT_SL) {
                flag = false;
                break;
            }
        }
    }
    return flag;
}

function CheckDiffTypeExists() {
    //<summary>function used to check diff type.</summary>

    var flag = true;
    if (parseInt(DefaultValueConfig.SLNo) == 0) {
        for (var i in DefaultValueConfig.DefaultListArray) {
            if (DefaultValueConfig.DefaultListArray[i].DFT_TYPE != $("[id$=DefaultType]").val()) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in DefaultValueConfig.DefaultListArray) {
            if (DefaultValueConfig.DefaultListArray[i].DFT_TYPE != $("[id$=DefaultType]").val() && parseInt(DefaultValueConfig.SLNo) != DefaultValueConfig.DefaultListArray[i].DFT_SL) {
                flag = false;
                break;
            }
        }
    }
    return flag;
}

function CheckDefaultNameExists() {
    //<summary>function used to check whether name already exists.</summary>

    var flag = true;
    if (parseInt(DefaultValueConfig.SLNo) == 0) {
        for (var i in DefaultValueConfig.DefaultListArray) {
            if (DefaultValueConfig.DefaultListArray[i].DFT_NAME == $("[id$=DefaultName]").val()) {
                flag = false;
                break;
            }
        }
    }
    else {
        for (var i in DefaultValueConfig.DefaultListArray) {
            if (DefaultValueConfig.DefaultListArray[i].DFT_NAME == $("[id$=DefaultName]").val() && parseInt(DefaultValueConfig.SLNo) != DefaultValueConfig.DefaultListArray[i].DFT_SL) {
                flag = false;
                break;
            }
            if (parseInt(DefaultValueConfig.SLNo) == DefaultValueConfig.DefaultListArray[i].DFT_SL)
                DefaultValueConfig.DefaultListObject = DefaultValueConfig.DefaultListArray[i];
        }
    }
    return flag;
}

function CheckIsDefaultExists() {
    //<summary>function used to check whether name already exists.</summary>

    var flag = true;
    if ($("[id$=SetAsDefault]").attr("checked")) {
        if (parseInt(DefaultValueConfig.SLNo) == 0) {
            for (var i in DefaultValueConfig.DefaultListArray) {
                if (DefaultValueConfig.DefaultListArray[i].DFT_IS_DEFAULT == "1") {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in DefaultValueConfig.DefaultListArray) {
                if (DefaultValueConfig.DefaultListArray[i].DFT_IS_DEFAULT == "1" && parseInt(DefaultValueConfig.SLNo) != DefaultValueConfig.DefaultListArray[i].DFT_SL) {
                    flag = false;
                    break;
                }
            }
        }
    }
    return flag;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for Catch all the grid events in this function </summary>

    switch (command.toString().toLowerCase()) {
        case DefaultValueConfig.DELETE:
            DefaultValueConfig.DeletePk = GrandGrid.Utilities.GetColumnValue(tr, DefaultValueConfig.DFT_SL, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(DefaultValueConfig.DeleteConfirmMsg, DefaultValueConfig.Confirmation, DefaultValueConfig.DELETE, true);
            return false;
            break;
        case DefaultValueConfig.EDIT:
            FillDefaultValueDetails(tr);
            return false;
            break;
    }
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdDefaultList") {
        if (DefaultValueConfig.tdset == "")
            DefaultValueConfig.tdset = $("#DefaultList").find("tr:eq(1)");
        //        $("#DefaultList").css({ "visibility": "hidden", "display": "none" });
        $("#divHeader").hide();

//        if (PurchaseRequestConfig.tdset == "")
//            PurchaseRequestConfig.tdset = $("#PurchaseRequestInsert").find("tr:eq(1)");
//        $("#divPurchaseRequestInsert").hide();

        $(DefaultValueConfig.tdset).insertBefore($("#grdDefaultList").find("tr:eq(1)"));
        $("#grdDefaultList tr:has(td)").each(function () {
            if (GrandGrid.Utilities.GetColumnValue($(this), DefaultValueConfig.DFT_CAN_DEL, $(this).parents("table:first").attr("id")) == "0") {
              $(this).find("td:last [id$=imbDelete]").hide();
            }
            if (GrandGrid.Utilities.GetColumnValue($(this), DefaultValueConfig.DFT_CAN_MDFY, $(this).parents("table:first").attr("id")) == "0") {
                $(this).find("td:last [id$=imbEdit]").hide();
            }
        });
    }
}

function AddNew() {
    //<summary>function used to add new entry</summary>

    $("[id$=Group]").val("");
    DefaultValueConfig.DefaultObject = new Object();
    DefaultValueConfig.DefaultObject.DFT_LIST = new Array();
    $("#divData").data("DefaultData", DefaultValueConfig.DefaultObject);
    GrandGrid.MakeGrid($("#grdDefaultList"), 0, DefaultValueConfig.DefaultObject.DFT_LIST);
    $(DefaultValueConfig.tdset).insertAfter($("#DefaultList").find("tr:eq(0)"));
    $("#DefaultList").css({ "visibility": "visible", "display": "block" });
    SetValueUtitlity(DefaultValueConfig.String);
}

function SetValueUtitlity(tVal, iVal) {
    switch (tVal) {
        case DefaultValueConfig.String:
            $("[id$=DefaultValue]").replaceWith("<input type=\"input\" tabindex=6 id=\"" + $("[id$=DefaultValue]").attr("id") + "\"  id=\"" + $("[id$=DefaultValue]").attr("id") + "\" " + (iVal == undefined ? "" : "value=" + iVal + "") + " />");
            break;
        case DefaultValueConfig.Float:
            $("[id$=DefaultValue]").replaceWith("<input type=\"input\" tabindex=6 id=\"" + $("[id$=DefaultValue]").attr("id") + "\"  id=\"" + $("[id$=DefaultValue]").attr("id") + "\" " + (iVal == undefined ? "" : "value=" + iVal + "") + " />");
            break;
        case DefaultValueConfig.DateTime:
            $("[id$=DefaultValue]").replaceWith("<input type=\"input\" tabindex=6 id=\"" + $("[id$=DefaultValue]").attr("id") + "\"  id=\"" + $("[id$=DefaultValue]").attr("id") + "\" " + (iVal == undefined ? "" : "value=" + iVal + "") + " />");
            GrandScriptUtils.DatePicker("DefaultValue");
            break;
        case DefaultValueConfig.Boolean:
            $("[id$=DefaultValue]").replaceWith("<input type=\"checkbox\" tabindex=6 id=\"" + $("[id$=DefaultValue]").attr("id") + "\" " + ((iVal == undefined || iVal == "false") ? "" : "checked=\"checked\"") + "  />");
            break;
    }
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case DefaultValueConfig.DELETE:
            DeleteDefaultValue();
            break;
        case DefaultValueConfig.DELETEALL:
            DeleteAllDefaultValue();
            break;
    }
}

//#endregion