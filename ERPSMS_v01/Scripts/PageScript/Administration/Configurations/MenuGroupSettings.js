/// <reference path="../../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../../../GrandScriptUtils.js" />
/// <reference path="../../../GrandTreeMulti.js" />


//#region------ Configuration Section-------
var MenuGroupSetting = {
    RootName: "Menu",
    MenuListURL: "MenuManagement.do?Action=GetMenuListDtls&MenuParentID=",
    SaveMenuGrpURL: "MenuManagement.do?Action=SaveMenuGrpDetails",
    GetMenuTreeURL: "MenuManagement.do?Action=GetMenuGrpDetails&SBUPk=",
    USERDTLSURL: "CommonManagement.do?Action=GetUserGroup",
    SBUDTLSURL: "SBUConfiguration.do?Action=GetAllSBUList",

    SELECTONEMSG: "Translate(Pleaseselectanoption)",
    SELECTUSERGROUP: "Translate(PleaseSelectUserGroup)",
    SELECTSBU: "Translate(SelectSBU)",
    INFORMATIONTITLE: "Translate(Information)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    SAVESUCCESS: "Translate(MenuGroupDetailsSavedSuccessfully)",

    UsrGrpParam: "&UserGroup=",
    MenuParam: "&MenuParentID=",
    SELECTONE: "selectNone",
    TEXTZERO: "0"
}
//#endregion

///#region------ Initialization Section ----------------
$.validator.addMethod(MenuGroupSetting.SELECTONE, function (value, element) {
    return ($(element).val() != MenuGroupSetting.TEXTZERO);
}, MenuGroupSetting.SELECTONEMSG);

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();
});

function PageInit() {
    //<summary>Function to initialize the page</summary>
    FillUserCombo();
    FillSBUCombo();
    FillMenuTreeView(0);
    $("[id$=UserGroup]").focus();
}
//#endregion

///#region------ Core Section ----------------

function FillUserCombo() {
    //<summary>Function to Fill UserGroup to DropDown</summary>
    var drpID = $("select[id$=UserGroup]").attr("id");
    $.get(MenuGroupSetting.USERDTLSURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillSBUCombo() {
    //<summary>Function Used to fill all sbu</summary>
    var drpID = $("[id$=SBU]").attr("id"); // Get id of the SBU DropDown
    $.get(MenuGroupSetting.SBUDTLSURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillMenuTreeView(grpPk) {
    //<summary>Function Used to Fill Menu Details to Tree View </summary>
    //var sbuPK = 1;
    var sbuPK = $("[id$=SBU]").val();
    var userGroup = 0;
    if ($("select[id$=UserGroup]").val() == null) {
        userGroup = 0;
    }
    else {
        userGroup = $("select[id$=UserGroup]").val();
    }
    SetTreeHeaderStructure("trvMenu", MenuGroupSetting.GetMenuTreeURL + sbuPK + MenuGroupSetting.UsrGrpParam + userGroup + MenuGroupSetting.MenuParam, MenuGroupSetting.RootName, true, false, "0", true, true);    // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}


function SavePage() {
    //<summary>Function Used to save all dept user group </summary>
    AddValidations();
    if ($(document.forms[0]).valid()) {
        $("[id$=USER_GROUP_MENU_LIST]").val(JSON.stringify(GetSelectedMenu()));
        var jSonString = GrandScriptUtils.FormToJsonString();
        $.post(MenuGroupSetting.SaveMenuGrpURL, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(MenuGroupSetting.SAVESUCCESS, MenuGroupSetting.INFORMATIONTITLE);
                ResetPage();
                RemoveValidations();
            }
            else {
                GrandScriptUtils.ShowModal(MenuGroupSetting.ACTIONFAILEDMSG, MenuGroupSetting.INFORMATIONTITLE);
                ResetPage();
                RemoveValidations();
            }
        });
    }
    return false;
}

function ResetPage() {
    //<summary>Function Used toReset Page</summary>
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        if (idval.search("UserPk") == -1)
            $(this).val("");
    });
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    FillMenuTreeView(0);
    $("[id$=UserGroup]").focus();
    return false;
}

function GetSelectedMenu() {
    //<summary>Function Used to get the all checked dept details </summary>
    var MenuArray = new Array();
    var sbuPK = $("[id$=SBU]").val();
    var userGroup = $("[id$=UserGroup]").val();
    var userPK = $("[id$=UserPk]").val();
    var menuPK = 0;
    $("#trvMenu").find("input[type=checkbox]:checked").each(function () {
        menuPK = $(this).attr("id");
        menuPK = menuPK.substr(menuPK.lastIndexOf("_") + 1, menuPK.length);
        MenuArray.push({ UGM_MENU: menuPK, UGM_MOD_BY: userPK, UGM_ACTIVE: 1 });
    });
    return MenuArray;
}

///#endregion

///#region------ Validations ----------------

function AddValidations() {
    //<summary>Function Used to Set Validation to Controls in page</summary>
    $("[id$=UserGroup]").rules("add", {
        selectNone: true,
        messages: { selectNone: MenuGroupSetting.SELECTUSERGROUP }
    });
    $("[id$=SBU]").rules("add", {
        selectNone: true,
        messages: { selectNone: MenuGroupSetting.SELECTSBU }
    });
}
function RemoveValidations() {
    //<summary>Function Used to Remove Validation to Controls in page</summary>
    $("select[id$=UserGroup]").rules("remove");
    $("select[id$=SBU]").rules("remove");
}
///#endregion

