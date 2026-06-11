/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region -----Global Variables-----
var storeID = 0;
///#endregion

///#region ------Configuration Section-----
var StoreMaster = {

    //Urls
    StoreAutoCompleteURL: "StoreManagement.do?Action=GetSearchValue",
    GetStoreTypeURL: "StoreManagement.do?Action=GetStoreType",
    StoreSaveURL: "StoreManagement.do?Action=SaveStore",
    StoreBindGridURL: "StoreManagement.do?Action=GetStoreList&Status=",
    //constants
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    TextZero: "0",
    //Fields
    StoreID: "Trsp",
    StoreName: "TrsNem",
    StoreTypeID: "Ersp",
    //Messages
    ConfirmationMessage: "Translate(Conformation)",
    MessageBoxTitle: "Translate(Information)",
    StoreSaveMessage: "Translate(StoreDetailsSavedSuccesfully)",
    StoreNameExistsMessage: "Translate(StorenameAlreadyExist)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    StoreDeleteMessage: "Translate(StoreDetailsDeletedSuccesfully)",
    StoreUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    StoreDeleteURL: "StoreManagement.do?Action=DeleteStore&StoreID=",
    //validation messages
    StoreNameValidation: "Translate(PleaseProvideStoreName)",
    StoreTypeValidation: "Translate(PleaseProvideStoreName)"

}
///#endregion

///#region------ Initialization Section ----------------

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    //Page Initial condtions
    PageInit();

});

//initial page condition
function PageInit() {
    ///<summary>initial page condition</summary>

    //Reseting all input controls in the page
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    //Filling Store dropdown initially.      
    FillStore();
    //Reseting all input controls in the page.
    ResetPage();
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    //calling function for binding grid.
    BindGrid();
    return false;
}


///#endregion

///#region---- Core Section Section----

///#region---- Data Management Section----
//Failed reason should specify store name exist or store cose exist

function SavePage() {
    ///<summary>saving materials</summary>

    AddValidations();
    if ($(document.forms[0]).valid()) {

        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(StoreMaster.StoreSaveURL, jSonString, function (data) {
            ///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(StoreMaster.StoreNameExistsMessage, StoreMaster.MessageBoxTitle);
            }
            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(StoreMaster.StoreSaveMessage, StoreMaster.MessageBoxTitle, StoreMaster.SaveCommand);
                PageInit();
            }
            else {
                GrandScriptUtils.ShowModal(StoreMaster.ActionFailedMessage);
                ResetPage();
            }
        });
    }
    return false;
}



function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete storeDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    var msgtxt;
    $.get(StoreMaster.StoreDeleteURL + storeID, function (data) {

        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = StoreMaster.StoreDeleteMessage;
        else if (parseInt(data) == 0)
            msgtxt = StoreMaster.StoreUsed;
        else
            msgtxt = StoreMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, StoreMaster.MessageBoxTitle, StoreMaster.DeleteCommand);

    });
    return false;
}

function BindGrid() {

    ///<summary>To handle bind grid corr. to the search type and search value</summary>

    var ajaxUrl = StoreMaster.StoreBindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();
    $("#grdStore").removeAttr("ajaxurl")
    $("#grdStore").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdStore");
    GrandGrid.MakeGrid($("#grdStore"));
    return false;
}

function AfterSelect() {
    ///<summary>filling gridview after entering search value in search textbox</summary>
    BindGrid();

}
function AfterGridBind() {
    ///<summary>Setting width of the template after binding grid</summary>
    $("#grdStore th:last").width("5%");
}
///#endregion

///#region---- Set Or Reset Form----


function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>

    $('[id$=btnSave]').show();
    $('[id$=btnAdd]').hide();
    $('[id$=divData]').show();
    $('[id$=divListing]').hide();
    return false;
}



function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        if (idval.search("StoreMasterID") != -1)
            $(this).val("0");
        //Avoid UserPk to get the value of log in user
        else if (idval.search("UserPk") == -1)
            $(this).val("");
    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == StoreMaster.TextZero) {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
        BindGrid();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
    }
}


function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", StoreMaster.StoreAutoCompleteURL, "SearchType");
}
///#endregion

///#region----Grid Handlers And Model Popup Ok Click----

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
        case StoreMaster.DeleteCommand:
            storeID = GrandGrid.Utilities.GetColumnValue(tr, StoreMaster.StoreID, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(StoreMaster.DeleteConfirmationMessage, StoreMaster.ConfirmationMessage, StoreMaster.DeleteMessageCommand, true);
            break;

        // To Edit Details              
        case StoreMaster.EditCommand:
            FillDetails(tr);
            break;
        // Default Handler    
        default:
            alert(StoreMaster.DefaultAction);
            break;
    }
    return false;

}


function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    /// <param name="command"  type="Object">
    ///     Specific Delete command
    /// </param>
    switch (command) {
        //comment req   
        case StoreMaster.DeleteCommand:
            BindGrid();
            break;
        //Commend When calling   
        case StoreMaster.DeleteMessageCommand:
            DeleteDetails();
            break;
    }
    return false;
}

///#endregion

///#region---- Fetch Data To Populate In Controls

function FillDetails(tr) {
    ///<summary>Fill store  Details for edit</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>


    $("input[id$=StoreMasterID]").val(GrandGrid.Utilities.GetColumnValue(tr, StoreMaster.StoreID, $(tr).parent().attr("id")));
    $("input[id$=StoreName]").val(GrandGrid.Utilities.GetColumnValue(tr, StoreMaster.StoreName, $(tr).parent().attr("id")));
    $("select[id$=StoreTypeID]").val(GrandGrid.Utilities.GetColumnValue(tr, StoreMaster.StoreTypeID, $(tr).parent().attr("id")));

    //changing mode to store lising
    AddNew();

}


function FillStore() {
    ///<summary>function To Fill store type dropdown Details </summary>

    // Get id of the storetype DropDown
    var drpID = $("select[id$=StoreTypeID]").attr("id");
    //Fill store Details to the store DropDown, Name as Text, PK as Value
    $.get(StoreMaster.GetStoreTypeURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

}
///#endregion


///#endregion

///#region------ Validations ----------------
function AddValidations() {
    ///<summary>function To Validations </summary>
    $("input[id$=StoreName]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: StoreMaster.StoreNameValidation }
    });
    $("select[id$=StoreTypeID]").rules("add", {
        selectNone: true,
        messages: { selectNone: StoreMaster.StoreTypeValidation }
    });



}
///#endregion