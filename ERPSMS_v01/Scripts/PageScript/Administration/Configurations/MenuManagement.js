/// <reference path="../../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../../../GrandScriptUtils.js" />
/// <reference path="../../../GrandTreeMulti.js" />

//#region -------Configuration Section-------

var MenuManagement = {
    RootName: "Root",
    MenuListURL: "MenuManagement.do?Action=GetMenuListDtls&MenuParentID=",
    MenuSaveURL: "MenuManagement.do?Action=SaveMenuDetails",
    MenuEditURL: "MenuManagement.do?Action=GetMenuDetails&MenuID=",
    MenuDeleteURL: "MenuManagement.do?Action=DeleteMenuDetails&MenuID=",
    MessageBoxTitle: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MenuSaveMessage: "Menu saved successfully.",
    MenuNameExistsMessage: "Menu Name already exists",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    MenuDeleteMessage: "Menu deleted successfully.",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    DeleteMessageCommand: "DELETEMSG"
}
///#endregion

///#region-------Initialization Section ----------------

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

    ResetPage();
    SetTreeHeaderStructure("trvCategory", MenuManagement.MenuListURL, MenuManagement.RootName, false, true, "0"); // set the tree view parameters
    //SetTreeHeaderStructure("trvCategory", MenuManagement.MenuListURL, false, true, "0");
    MakeMultiTree(); // call the function to bind tree view
    $("[id$=MenuName]").focus();
}

///#endregion

///#region------ Core Section ----------------

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command" optional="true" type="String">
    /// Click OK which which methode perform based on this command
    /// </param>

    switch (command) {
        case MenuManagement.SaveCommand:
            MakeMultiTree(); // call the function to bind tree view
            $("[id$=MenuName]").focus();
            break;
        case MenuManagement.DeleteCommand:
            MakeMultiTree(); // call the function to bind tree view
            $("[id$=MenuName]").focus();
            break;
        case MenuManagement.DeleteMessageCommand:
            DeleteMenu();
            break;
    }
}

function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd" optional="true" type="String">
    /// The Selected Element ID
    /// </param>

    var menuID = $(liAdd).attr("id"); // get the selected tree id
    menuID = menuID.substr(menuID.lastIndexOf("_") + 1, menuID.length); // fetch the exact id of category
    var cagName = $(liAdd).parent("li").find("span").html(); // get the name of category
    ResetPage();
    $("[id$=Parent]").html(cagName);
    $("[id$=MenuParentPK]").val(menuID);
    $("[id$=MenuPosition]").val($(liAdd).parent("li").children("ul").children("li").length + 1);
}

function EditSelectedTree(liEdit) {
    ///<summary>Function used Edit the tree Data </summary>
    /// <param name="liAdd" optional="true" type="String">
    /// The Selected Element ID
    /// </param>

    RemoveValidations();
    var menuID = $(liEdit).attr("id"); // get the selected tree id
    menuID = menuID.substr(menuID.lastIndexOf("_") + 1, menuID.length);  // fetch the exact id of category
    $.get(MenuManagement.MenuEditURL + menuID, function (data) {
        if (data) {
            $("[id$=MenuPK]").val(data.MenuPK);
            $("[id$=MenuName]").val(data.MenuName);
            $("[id$=MenuUrl]").val(data.MenuUrl);
            $("[id$=MenuPosition]").val(data.MenuPosition);
            $("[id$=MenuParentPK]").val(data.MenuParentPK);
            $("[id$=Parent]").html(data.MenuParentName);
            if ((data.HasChild == "true")) 
                $("[id$=imbDelete]").hide();
            else
                $("[id$=imbDelete]").show();
            $("[id$=MenuName]").focus();
        }
    });
}

function SavePage() {
    ///<summary>Function used to Save material category details</summary>

    AddValidations();
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    if ($(document.forms[0]).valid()) {
        $.post(MenuManagement.MenuSaveURL, jSonString, function (data) {
            if (parseInt(data) > 0) { // menu saved successfully.
                GrandScriptUtils.ShowModal(MenuManagement.MenuSaveMessage, MenuManagement.MessageBoxTitle, MenuManagement.SaveCommand);
                ResetPage();
            }
            else if (parseInt(data) == 0) { // menu name already exists.
                GrandScriptUtils.ShowModal(MenuManagement.MenuNameExistsMessage, MenuManagement.MessageBoxTitle);
            }
            else { // error occured
                GrandScriptUtils.ShowModal(MenuManagement.ActionFailedMessage, MenuManagement.MessageBoxTitle);
                ResetPage();
            }
        });
    }
    return false;
}

function DeletePage() {
    ///<summary>Function Used to confirmation for delete, if ok then delete action takes place </summary>

    GrandScriptUtils.ShowModal(MenuManagement.DeleteConfirmationMessage, MenuManagement.MessageBoxTitle, MenuManagement.DeleteMessageCommand, true);
    return false;
}

function DeleteMenu() {
    //<summary>Function Used to Delete  menu by menu id</summary>

    var menuID = $("[id$=MenuPK]").val();
    $.get(MenuManagement.MenuDeleteURL + menuID, function (data) {
        if (parseInt(data) > 0) // deleted successfully.
            GrandScriptUtils.ShowModal(MenuManagement.MenuDeleteMessage, MenuManagement.MessageBoxTitle, MenuManagement.DeleteCommand);
        else  // error occured
            GrandScriptUtils.ShowModal(MaterialCategory.ActionFailedMessage, MaterialCategory.MessageBoxTitle);
        ResetPage();
    });
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    $(document.forms[0]).find("input").each(function () { //reseting all input controls in the page
        var idval = $(this).attr("id");
        if (idval.search("MenuPK") != -1)
            $(this).val("0");
        else if (idval.search("MenuParentPK") != -1)
            $(this).val("0");
        else if (idval.search("UserPk") == -1) //avoid UserPk to get the value of log in user
            $(this).val("");
    });
    $(document.forms[0]).find("select").each(function () { //selecting the first value in all drop downs
        $(this).val($(this).find("option:eq(0)").val());
    });
    RemoveValidations(); // remove all validation
    $("[id$=Parent]").html(MenuManagement.RootName);
    $("[id$=imbDelete]").hide();
    return false;
}

///#endregion

///#region------ Validations ----------------

function AddValidations() {
    //<summary>Function used to assign validation</summary>

    $("[id$=MenuName]").rules("add", {
        required: true,
        maxlength: 40,
        messages: { required: "Enter Menu Name" }
    });
    $("[id$=MenuUrl]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Enter Menu URL" }
    });
    $("[id$=MenuPosition]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Enter Menu URL" }
    });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
}

///#endregion
