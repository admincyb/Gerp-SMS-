/// <reference path="../../../GrandGridMulti.js" />
/// <reference path="../../../GrandScriptUtils.js" />
/// <reference path="../../../GrandTreeMulti.js" />


//#region----------- Configuration Section ----------------

var UserGroupDepartment = {
    SaveUserGroupDept: "UserGroupDepartment.do?Action=SaveUserGroupDepartment",
    DeleteUserGroupDept: "UserGroupDepartment.do?Action=DeleteUserGroupDepartment&SBUPk=",
    GetSBUList: "SBUConfiguration.do?Action=GetAllSBUList",
    GetUserGroupList: "CommonManagement.do?Action=GetUserGroup",
    GetDepartmentTreeURL: "CommonManagement.do?Action=GetDepartmentDetails&SBUPk=",
    CategortRootName: "Dapartments",
    DetpParam: "&deptParentID=",
    UsrGrpParam: "&UserGroup=",

    //Messages

    SavedSuccessfully: "Translate(SaveUserGroupSept)",
    MessageBoxTitle: "Translate(Information)",
    SelectOneDept: "Translate(PleaseSelectDept)",
    DeleteAllMessage: "Translate(DeleteAllUserGroupDept)",
    ActionFailedMessage: "Translate(CannotDelete)",

    DELETEALL: "DELETEALL"

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
    FillUserGroupCombo();
    $("[id$=UserGroup]").focus();
}

//#endregion

//#region----------- Validation Section----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=SBU]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectSBU)" }
    });
    $("[id$=UserGroup]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(PleaseSelectUserGroup)" }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $("[id$=SBU]").rules("remove");
    $("[id$=UserGroup]").rules("remove");
}

//#endregion

//#region----------- Core Section----------------

function FillSBUCombo() {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=SBU]").attr("id"); // Get id of the SBU DropDown
    $.get(UserGroupDepartment.GetSBUList, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillUserGroupCombo() {
    //<summary>Function Used to fill all user group</summary>

    var drpID = $("[id$=UserGroup]").attr("id"); // Get id of the SBU DropDown
    $.get(UserGroupDepartment.GetUserGroupList, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillDeptCombo() {
    //<summary>Function Used to fill all dept based on the sbu </summary>

    var sbuPK = $("[id$=SBU]").val();
    var userGroup = $("[id$=UserGroup]").val();
    SetTreeHeaderStructure("trvDepartment", UserGroupDepartment.GetDepartmentTreeURL + sbuPK + UserGroupDepartment.UsrGrpParam + userGroup + UserGroupDepartment.DetpParam, UserGroupDepartment.CategortRootName, true, false, "0", true);   // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

function SavePage() {
    //<summary>Function Used to save all dept user group </summary>

    RemoveValidations();
    AddValidations();
    if ($(document.forms[0]).valid()) {
        var UserGroupDeptArray = GetSelectedDept();
        if (UserGroupDeptArray.length != 0) {
            $("[id$=USER_GROUP_DEPT_LIST]").val(JSON.stringify(UserGroupDeptArray));
            var jSonString = GrandScriptUtils.FormToJsonString("divResult");
            $.post(UserGroupDepartment.SaveUserGroupDept, jSonString, function (result) {
                if (parseInt(result) > 0) {
                    GrandScriptUtils.ShowModal(UserGroupDepartment.SavedSuccessfully, UserGroupDepartment.MessageBoxTitle);
                    ResetPage();
                    RemoveValidations();
                }
                else {
                    GrandScriptUtils.ShowModal(UserGroupDepartment.ActionFailedMessage, UserGroupDepartment.MessageBoxTitle);
                    ResetPage();
                    RemoveValidations();
                }
            });
        }
        else {
            GrandScriptUtils.ShowModal(UserGroupDepartment.DeleteAllMessage, UserGroupDepartment.MessageBoxTitle, UserGroupDepartment.DELETEALL, true);
        }
    }
    return false;
}

function DeleteAllUserGroupDept() {
    ///<summary>Function used for delete all item</summary>

    $.get(UserGroupDepartment.DeleteUserGroupDept + $("[id$=SBU]").val() + "&Group=" + $("[id$=Group]").val(), function (result) {
        if (parseInt(result) > 0) {
            GrandScriptUtils.ShowModal(UserGroupDepartment.SavedSuccessfully, UserGroupDepartment.MessageBoxTitle);
            ResetPage();
        }
        else {
            GrandScriptUtils.ShowModal(UserGroupDepartment.ActionFailedMessage, UserGroupDepartment.MessageBoxTitle);
            ResetPage();
        }
    });
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        if (idval.search("UserPk") == -1)
            $(this).val("");
    });
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    FillDeptCombo();
    $("[id$=UserGroup]").focus();
    return false;
}

//#endregion

//#region----------- Utility Section----------------

function GetSelectedDept() {
    //<summary>Function Used to get the all checked dept details </summary>

    var UserGroupDeptArray = new Array();
    var sbuPK = $("[id$=SBU]").val();
    var userGroup = $("[id$=UserGroup]").val();
    var userPK = $("[id$=UserPk]").val();
    var deptPK = 0;
    $("#trvDepartment").find("input[type=checkbox]:checked").each(function () {
        deptPK = $(this).attr("id");
        deptPK = deptPK.substr(deptPK.lastIndexOf("_") + 1, deptPK.length);
        UserGroupDeptArray.push({ UGD_USER_GROUP: userGroup, UGD_DEPT: deptPK, UGD_BIZUNIT: sbuPK, UGD_MOD_BY: userPK });
    });
    return UserGroupDeptArray;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case UserGroupDepartment.DELETEALL:
            DeleteAllUserGroupDept();
            break;
    }
}

//#endregion

