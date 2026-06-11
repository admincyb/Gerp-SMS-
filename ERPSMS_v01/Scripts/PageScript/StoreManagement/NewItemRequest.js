/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="GrandTreeMulti.js" />

///#region -----Global Variables-----
var storeID = 0;
//Global variable Declaration

var materialID = 0;
var EditMode = 0;
var refIdMode = 0;
///#endregion

///#region -----Configuration-----
var NewItemRequest = {
    //Url

    AutoCompleteURL: "NewItemRequest.do?Action=GetSearchValue",
  //  FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=",
    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListWithoutSemiAndFinished&SBUPk=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",

    FillUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    FillStoreDropdownURL: "StoreRequisitionSlip.do?Action=GetStoresFilterByCategory&SBUPk=",
    FillDepartementDropdownURL: "SubDepartmentManagement.do?Action=GetNonStoreDept&SBUPk=",
    FillDepartementSwitchUserDropdownURL: "SubDepartmentManagement.do?Action=GetNonStoreDeptUserSwitch&SBUPk=",
    
    RequisitionSaveURL: "NewItemRequest.do?Action=SaveNewItemRequest",
    REDIRECTURLAFTERSAVE: "../StoreManagement/NewItemRequestList.aspx",
    REDIRECTURLAFTERSAVEFROMINBOX: "../AccountManagement/WorkflowInbox.aspx",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategoryAndStore&SBUPk=",
    GETSRSNoURL: "NewItemRequest.do?Action=GetSRSNo",
    BindPopupGridURL: "NewItemRequest.do?Action=GetNewItemCheckList&SBUPk=",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    GetMaterialCategoryExceptFGTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryNewWithoutFG&SBUPk=",
    PRINTURL: "../StoreManagement/StoreRequisitionReport.aspx",
    MaterialSaveURL: "MaterialManagement.do?Action=SavePage&DepartPK=",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    NewItemRequestIntimation: "Translate(NewItemRequestIntimation)",
    ConfirmationMessage: "Translate(Conformation)",
    SaveMessage1: "Translate(NewItemDetailsSaved1)",
    SaveMessage2: "Translate(NewItemDetailsSaved2)",
    RequisitionUpdateMessage2: "Translate(RequisitionDetailsUpdated2)",
    CodeExistsMessage: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    DeleteMessage: "Translate(NewItemDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    CodeAlreadyAdded: "Translate(AlreadyExists)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    FRD: "Translate(PleaseProvideFRD)",
    ProvideDate: "Translate(ProvideDate)",
    ProvideRequiredByDate: "Translate(ProvideRequiredByDate)",
//    EnterTHREEDecimal: "Translate(EnterThreeDigitDecimal)",
    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    Param: "&MatCagID=",
    //Validation messages
    NewItemRequiredFor: "Translate(PleaseSelectRequiredFor)",
    NewItemRequestTo: "Translate(PleaseSelectRequestTo)",
    NewItemSelectCategory: "Translate(SelectMaterialCategory)",
    NewItemQuantityValidation: "Translate(PleaseProvideQtyRequest)",

    NewItemEnterNameTitleDepartementValidation: "Translate(PleaseProvideNameTitle)",

    NewItemUOMValidation: "Translate(PleaseSelectUOM)",
    
    //Fields

    MaterialCode: "MaterialCode",
    MaterialDate: "MRH_SUBMITTED_DATE",
    CurrentStock: "CurrentStock",
    MaterialQtyRequest: "MRD_QTY_REQUESTED",
    MaterialUOMID: "MRD_UOM",
    MaterialComments: "MRD_REMARKS",
    MaterialID: "MRD_ITEM",
    MRD_PK: "MRD_PK",
    MaterialTypePk:"MaterialTypePk"

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
    //Hiding itemcode and po# entry part initially.
    $("#divIM").hide();
    //initialize Requisition Object
    newItemJson = $.parseJSON($("[id$=NewItemDetailsList]").val());
    $("#divRequisitionData").data("RequisitionData", newItemJson);
    //Create Date Picker
    GrandScriptUtils.DatePicker("NIR_SUBMITTED_DATE", false, true);
    GrandScriptUtils.DatePicker("NIR_REQUIRED_DATE", false, true);
    //   $("[id$=MRH_SUBMITTED_DATE]").focus();
    //Get RequisitionID Id From the Url and Fill Details - For Edit 

    var queryStr = window.location.search.substring(1);
    //from inbox
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = queryStr.split("=");
            if ((pK[1] != "" && pK[0] == "RefID") || (pK[1] != "" && pK[0] == "NewRequestID")) {
                EditMode = 1;
                FillRequisitionDetails(newItemJson);
            }
        }
    }
    ViewMode();//disabling controls when view mode.
    SettingVisibiltyForTextbox();
    PageInit();
    $("#divItemRequestPopUp").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");

        },
        beforeClose: function (event, ui) {
            //  RemovePopupValidations();
        }
    });

    //Modal popup and tree view default settings
    //$("#divCategory").dialog({ autoOpen: false });

});

function FillRequisitionDetails(newItemJson) {
///<summary>Used to fill requisition Details for editing</summary>
    // var drpID = $("select[id$=Product]").attr("id");
    //Header Details.

    if ((newItemJson[0].ITR_STATUS == 1) || (newItemJson[0].ITR_STATUS == 7)) {//1=submit,7=submit more info
        $("#divIM").show();
        refIdMode = 1; //  refIdMode = 1 get non inventory departement without userpk.
    }
    else {
        $("#divIM").hide();
        refIdMode = 0; //  refIdMode = 0 get non inventory departement with userpk.
    }

    $("input[id$=ITR_PK]").val(newItemJson[0].ITR_PK)
    FillStore(newItemJson[0].ITR_DEPT_STORE);
    FillDepartement(newItemJson[0].ITR_DEPT);
    FillMaterialCategory(newItemJson[0].ITR_ITEM_CATEGORY);
    FillUOM(newItemJson[0].ITR_ITEM_CATEGORY, newItemJson[0].ITR_QTY_UOM)
    $("[id$=NIR_SUBMITTED_DATE]").val(newItemJson[0].ITR_DATE)
    $("[id$=NIR_REQUIRED_DATE]").val(newItemJson[0].ITR_REQD_DATE)

    $("[id$=ActionStatus]").val(newItemJson[0].ITR_STATUS)
    $("input[id$=NIR_NO]").val(newItemJson[0].ITR_NO);
    $("[id$=lblNIR]").html(newItemJson[0].ITR_NO);
    $("input[id$=NameTitle]").val(newItemJson[0].ITR_NAME);
    $("[id$=RequestFrequency]").find("input").each(function () {
        if ($(this).val() == newItemJson[0].ITR_FREQUENCY) {
            $(this).attr("checked", "checked");
        }
    });
    $("[id$=FRD]").html(newItemJson[0].ITR_FRQ_RQMT_DTL);
    $("[id$=KnownVendors]").html(newItemJson[0].ITR_KWN_VENDOR);
    $("input[id$=QtyRequired]").val(newItemJson[0].ITR_QTY_REQD);

    $("[id$=Description]").html(newItemJson[0].ITR_DESC);
    $("[id$=Purpose]").html(newItemJson[0].ITR_PURPOSE);
    $("[id$=CommercialDetails]").html(newItemJson[0].ITR_COMM_DTL);
    $("[id$=LAST_MOD_DT]").val(newItemJson[0].LAST_MOD_DT);
    

   
}

function PageInit() {
    ///<summary>initial page condition</summary>

    //Reseting all input controls in the page.
    // ResetPage();
     
    if (EditMode == 0) {
      
        //Filling Store dropdown initially.      
        FillStore(0);
        FillDepartement(0);
        FillMaterialCategory(0);
        FillUOM(0, 0);
      
       
    }
    $("[id$=MRH_DEPT_STR]").focus();
   
}
///#endregion

///#region---- Core Section Section----
///#region---- Fetch Data To Populate In Controls

function FillStore(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    var drpID = $("select[id$=MRH_DEPT_STR]").attr("id");
    $.get(NewItemRequest.FillStoreDropdownURL + $("[id$=BizUnitPk]").val()+"&Category=" + 0, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
    });
}

function FillDepartement(SelectedValue) {
    ///<summary>to fill Departement combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the Departement DropDown Departement 

    var drpID = "";
    var userFlagText = "";
    if (refIdMode == 1)
    {
        NewItemRequest.FillDepartementDropdownURL = NewItemRequest.FillDepartementSwitchUserDropdownURL;
    }
    $.get(NewItemRequest.FillDepartementDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
        drpID = $("select[id$=DeptPk]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        if ($("select[id$=MRH_DEPT_STR]").val() != 0) {
            $("#" + drpID + " option[value=" + $("select[id$=MRH_DEPT_STR]").val() + "]").remove();
  
        }
        
    });
}

function FillMaterialCategory(materialID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(NewItemRequest.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() , function (data) {
        if (materialID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID,false, true);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true,0,false, true);
    });

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
        $("select[id$=UOM]").removeData();
        // Get id of the UOM DropDown
        var drpID = $("select[id$=UOM]").attr("id");
        //Fill UOM Details to the UOM DropDown, Name as Text, PK as Value

        $.get(NewItemRequest.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val() + NewItemRequest.Param + categoryID, function (data) {
            if (selectval) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else {
        $("select[id$=UOM]").find("option").remove();
        var drpID = $("select[id$=UOM]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, null, true, true);
    }
}


///#endregion

///#region---- Data Management Section----

function BindPopupGrid() {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var ajaxUrl = NewItemRequest.BindPopupGridURL + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ItemName=" + $("[id$=NameTitle]").val();
     $("#grdItemRequestPopup").removeAttr("ajaxurl")
     $("#grdItemRequestPopup").attr("ajaxurl", ajaxUrl);
     GrandGrid.Utilities.ResetGrid(true, "grdItemRequestPopup");
     GrandGrid.MakeGrid($("#grdItemRequestPopup"));
    return false;
}
function AddItemRequestPopUp() {

    if ($(document.forms[0]).valid()) {
        BindPopupGrid();


       
            $("#divItemRequestPopUp").dialog("open");
            $("#divItemRequestPopUp").dialog({ width: 500, height: 350, resizable: true });
            $("#divItemRequestPopUp").css({ "min-height": "300", "margin-top": "25px" });
 //        if (noData == 1) {
//            $("#divItemRequestPopUp").dialog("open");
//            $("#divItemRequestPopUp").dialog({ width: 500, height: 350, resizable: true });
//            $("#divItemRequestPopUp").css({ "min-height": "300", "margin-top": "25px" });
//        }
//        else {
//            $("[id$=btnYes]").click();

//        }
       // setTimeout(function () { $("input[id$=ITM_NAME]").focus(); }, 10);
    }
    return false;
}
var noData = 0;
function ViewMode() {
    //<summary>function Call Afer binding Grid</summary>

    //for hiding action fields of detail section 23-11-11
    var queryStr = window.location.search.substring(1);
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                $('input[type=radio], input[type=checkbox], select, input[type=text], textarea').attr('disabled', 'disabled');
                
                $("#divIM").hide();
            }
        }
    }
    if (ObjRequisition != undefined) {
        if (ObjRequisition[0].ITR_STATUS == 1 || ObjRequisition[0].ITR_STATUS == 2 || ObjRequisition[0].ITR_STATUS == 7) {
            $(' input[type=text],select,input[type=radio], textarea').attr('disabled', 'disabled');


            $("[id$=txtItemCode]").removeAttr('disabled');
            $("[id$=txtPR]").removeAttr('disabled');
            $("[id$=WrkfComments]").removeAttr('disabled');
            $("[id$=WRKFACT_ID]").removeAttr('disabled');
            $("[id$=btnSubmitWrkf]").removeAttr('disabled');  

        }
    }
   
}
function AfterGridBindWithNoData(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    
//    if (grdID == "grdItemRequestPopup") {
//        noData = 0;
//    }
    
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button])").each(function () {
        var idval = $(this).attr("id");
        if (!Checkstatus(idval)) {
            $(this).val("");
        }

    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    window.location = NewItemRequest.REDIRECTURLAFTERSAVE;
    return false;
}
function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page
   
    if (controlID.search("UserPk") != -1) {
        return true;
    }
    if (controlID.search("BizUnitPk") != -1) {
        return true;
    }
    if (controlID.search("ITR_PK") != -1) {
        return true;
    }
    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
  
    return false;
}
function PrintPage(){
    ///<summary>Function To Show PRINT Form </summary>
    window.location = NewItemRequest.PRINTURL + "?RequisitionID=" + $("input[id$=MRH_PK]").val();
    return false;
}
function SavePage(command) {
    ///<summary>Function used to saving   </summary>
    RemoveValidations();
     AddValidations(1);
     if ($(document.forms[0]).valid()) {
         $('input[type=submit], input[type=checkbox], input[type=radio], select, input[type=text], textarea').attr('disabled', false);
         SaveOrDraft = command;
         //checking command value is draft if it is draft then action id is zero means it is not calling workflow.
         if (command != "Draft") {
             $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
         }
         else {
             $("[id$=ActionID]").val('0');
         }
         if ($("[id$=ActionStatus]").val() == "0") {
             AddItemRequestPopUp();
         }
         else {
             SavePageAfterPopup();
         }
        }
    return false;
}
String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
}

var SaveOrDraft = "";
function SavePageAfterPopup() {
    $("[id$=divItemRequestPopUp]").dialog("close");

    var finaltext = String.format(NewItemRequest.NewItemRequestIntimation, $("[id$=txtItemCode]").val(), $("[id$=txtPR]").val());
    //var finaltext = String.format("New Item created with code:{0}  and placed Purchase Request for the item with PR# :{1}", $("[id$=txtItemCode]").val(), $("[id$=txtPR]").val());
    $("[id$=Remarks]").val(finaltext);
    var jSonString = GrandScriptUtils.FormToJsonString(false);

    //To Prevent Duplicate Submission
    if ($("[id$=SubmitFlag]").val() == "0")
        $("[id$=SubmitFlag]").val('1')
    else
        return false;


    $.post(NewItemRequest.RequisitionSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
        if (parseInt(data[0]) == 0) {
            GrandScriptUtils.ShowModal(NewItemRequest.CodeAlreadyAdded, NewItemRequest.MessageBoxTitle, NewItemRequest.SaveCommand);
        }
        else if (parseInt(data[0]) > 0) {
            // If Action is Draft Save
            if (SaveOrDraft == "Draft") {
                var SaveMessageWithNIRNo = "";
                SaveMessageWithNIRNo = NewItemRequest.SaveMessage1 + " " + data[1] + " " + NewItemRequest.SaveMessage2
                GrandScriptUtils.ShowModal(SaveMessageWithNIRNo, NewItemRequest.MessageBoxTitle, NewItemRequest.SaveCommand);
                PageInit();

            }
            else {
                $("[id$=hdfAppID]").val(data[0]);
                $("[id$=AppNo]").val(data[1]);
                if ($("[id$=txtItemCode]").val() != "" && $("[id$=txtPR]").val() != "") {
                    var chkItemText = $("[id$=chkItemCode]").next("label").html() + " : " + $("[id$=txtItemCode]").val();
                    var chkPOText = $("[id$=chkPR]").next("label").html() + " : " + $("[id$=txtPR]").val();
                    var addComment = new Array();
                    addComment[0] = chkItemText;
                    addComment[1] = chkPOText;
                }
                SaveWorkFlow(false, false, addComment);
            }
        }
        else if (parseInt(data[0]) == -2) {
            GrandScriptUtils.ShowModal(NewItemRequest.SaveMessage1 + " " + data[1] + " " + NewItemRequest.EditUsedByAnotherUser, NewItemRequest.MessageBoxTitle, NewItemRequest.SAVE);
        }
        else {
            GrandScriptUtils.ShowModal(NewItemRequest.ActionFailedMessage);
            ResetPage();
        }
    });
    return false;
}
function CancelPageAfterPopup() {
    $("[id$=divItemRequestPopUp]").dialog("close");
    return false;
}
function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>

    var SaveMessageWithNIRNo = "";
    SaveMessageWithNIRNo = NewItemRequest.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + NewItemRequest.SaveMessage2
    GrandScriptUtils.ShowModal(SaveMessageWithNIRNo, NewItemRequest.MessageBoxTitle, NewItemRequest.SaveCommand);
}

function FillDetails(tr) {
    ///<summary>Used fill Details of requisition for editing</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    $("select[id$=ITV_ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=MaterialCode]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialCode, $(tr).parent().attr("id")));
    //$("input[id$=MRH_SUBMITTED_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialDate, $(tr).parent().attr("id")));
    $("[id$=CurrentStock]").html(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.CurrentStock, $(tr).parent().attr("id")));
    $("input[id$=MRD_QTY_REQUESTED]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialQtyRequest, $(tr).parent().attr("id")));
    $("select[id$=MaterialType]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialTypePk, $(tr).parent().attr("id")));
    $("input[id$=MRD_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialComments, $(tr).parent().attr("id")));
    $("input[id$=MRD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MRD_PK, $(tr).parent().attr("id")));
    $("input[id$=EditRequisition]").val(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("input[id$=MaterialCode]").focus();
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialID, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialUOMID, $(tr).parent().attr("id")));
    FillCategoryMaterials(GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialTypePk, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialID, $(tr).parent().attr("id")), $("select[id$=MRH_DEPT_STR]").val());
}

function DeleteDetails(tr) {
    ///<summary>Used fill Details of requisition for Delete</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    for (var i in ObjRequisition.RequisitionDetailsList) {
        if (ObjRequisition.RequisitionDetailsList[i].MRD_ITEM == materialID) {
            //Will delete the Evaluation details
            ObjRequisition.RequisitionDetailsList.splice(i, 1);
            break;
        }
    }
    $("#divRequisitionData").data("RequisitionData", ObjRequisition);
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.RequisitionDetailsList);
    //Used to Show the  Evaluation details when the requisition in requisition details is 0
    if (ObjRequisition.RequisitionDetailsList.length == 0) {
        //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
      //  $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
    }



}


///#endregion

/////#region----Grid Handlers And Model Popup Ok Click----
function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    /// <param name="command"  type="Object">
    ///  Specific command for action       
    /// </param>
   RemoveValidations();
    switch (command.toString()) {
        case NewItemRequest.DeleteCommand:
            // Do Confirmation.. Before Delete Details
            materialID = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequest.MaterialID, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(NewItemRequest.DeleteConfirmationMessage, NewItemRequest.ConfirmationMessage, NewItemRequest.DeleteCommand, true);
            break;
        case NewItemRequest.EditCommand:
            FillDetails(tr);
            return false;
            break;
        default:
            alert(NewItemRequest.DefaultAction);
            return false;
            break;
    }
    return false;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="Object">
    ///  Specific command for action edit/save/delete       
    /// </param>
    switch (command) {
        case NewItemRequest.SaveCommand:
            window.location = NewItemRequest.REDIRECTURLAFTERSAVE;
            break;
        //comment req   
        case NewItemRequest.DeleteCommand:
            DeleteDetails();
            break;
        //Commend When calling   
        case NewItemRequest.DeleteMessageCommand:
            GrandScriptUtils.ShowModal(NewItemRequest.DeleteConfirmationMessage, NewItemRequest.ConfirmationMessage);
            break;
    }
    return false;
}
/////#endregion
///#endregion

///#region ------ Validation----------

function AddValidations(mode) {
    //<summary>function used to assign validation</summary>
   
//    //Mode = 1 represents the validation for request(Details) Header Details
    if (mode == "1") {
        $("select[id$=MRH_DEPT_STR]").rules("add", {
            selectNone: true,
            messages: { selectNone: NewItemRequest.NewItemRequestTo }
        });
        $("select[id$=DeptPk]").rules("add", {
            selectNone: true,
            messages: { selectNone: NewItemRequest.NewItemRequiredFor }
        });
        $("select[id$=MaterialType]").rules("add", {
            selectNone: true,
            messages: { selectNone: NewItemRequest.NewItemSelectCategory }
        });
        $("input[id$=NameTitle]").rules("add", {
            required: true,
            maxlength: 200,

            messages: { required: NewItemRequest.NewItemEnterNameTitleDepartementValidation}
        });
        $("input[id$=QtyRequired]").rules("add", {
            required: true,
            maxlength: 12,
            ThreeDecimal:true,

            messages: { required: NewItemRequest.NewItemQuantityValidation }
        });
        if ($("[id$=RequestFrequency]").find("input:last").attr("checked")) {

            $("[id$=FRD]").rules("add", {
                required: true,
                messages: { required: NewItemRequest.FRD }
            });
            $("[id$=FRD]").rules("add", {
                maxlength: 500
            });
        }
        $("select[id$=UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: NewItemRequest.NewItemUOMValidation }
        });
        $("input[id$=NIR_SUBMITTED_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: NewItemRequest.ProvideDate }
        });
        $("input[id$=NIR_REQUIRED_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: NewItemRequest.ProvideRequiredByDate }
        });
        $("[id$=Description]").rules("add", {
            maxlength: 500
        });
        $("[id$=Purpose]").rules("add", {
            maxlength: 500
        });
        $("[id$=CommercialDetails]").rules("add", {
            maxlength: 500
        });
        $("[id$=FRD]").rules("add", {
            maxlength: 500
        });
        $("[id$=KnownVendors]").rules("add", {
            maxlength: 500
        }); 
    }


    
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $("select[id$=MaterialType]").rules("remove");
    $("select[id$=MRH_DEPT_STR]").rules("remove");
    $("select[id$=DeptPk]").rules("remove");
    $("input[id$=NameTitle]").rules("remove");
    $("input[id$=QtyRequired]").rules("remove");
    $("[id$=FRD]").rules("remove");
    $("select[id$=UOM]").rules("remove");
    $("input[id$=NIR_SUBMITTED_DATE]").rules("remove");
    $("input[id$=NIR_REQUIRED_DATE]").rules("remove");
  
}
function SettingVisibiltyForTextbox() {
    if ($("[id$=RequestFrequency]").find("input:first").attr("checked")) {
        $("label[for='FRD']").hide(); 
        $("[id$=FRD]").hide();
    }
    else {
        $("[id$=FRD]").show();
        $("label[for='FRD']").show(); 
    }
}
///#endregion 
