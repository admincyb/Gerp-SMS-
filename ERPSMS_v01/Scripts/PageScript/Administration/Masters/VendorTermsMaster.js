
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region Global Variables
var TermId = 0;
var SBUPK = 1;

var VendorTermsMaster = {
        //urls
    VendorTermsSaveURL: "VendorTermsManagement.do?Action=SavePage",
    VendorTermsListGet: "VendorTermsManagement.do?Action=GetVenderTermsList&Status=",
    DeleteCommandURL: "VendorTermsManagement.do?Action=Delete&TermID=",
    AutoCompleteSearch: "VendorTermsManagement.do?Action=GetSearchValue&BizUnitPk=",

    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",

    //fields
    TermStatus: "VET_ACTIVE",
    TermStatusText: "VET_ACTIVE_TEXT",
    TermsPK: "VET_PK",
    TermsDescr:"VET_DESC",
    TermsTitle:"VET_TITLE",
    TermsType:"VET_TYPE",

    //Messages
    DeleteConfirmation: "Translate(Doyouwanttodeletethisdetails)",

    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    TermsSaveMessage: "Translate(TermSaveSuccess)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MaterialUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    ActionFailed: "Translate(ActionFailedPleaseTryAgain)",
    DeletedMessage: "Translate(VendorTermDeletedSuccessfully)",
    CannotDelete: "Translate(CannotDelete)",
    AlreadyExists: "Translate(AlreadyExists)",
    Information:"Translate(Information)",
   PleaseProvideTermTitle: 'Translate(PleaseProvideTermTitle)',
   PleaseProvideTermDescription:'Translate(PleaseProvideTermDescription)',
   PleaseselectTermType:'Translate(PleaseselectTermType)'
};
///#endregion

///#region initialization Section

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

    $("[id$=SearchType]").change(function () {
        SetSearchType();
        if ($("select[id$=SearchType]").val() == "0") {
            BindGrid();
        }
    });
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });

});
 ///<summary>Initialize the data entry screen</summary>
function AddNew() {
    $("#divData").show();
    $("#divListing").hide();
    $("[id$=btnAdd]").hide();
    $("[id$=btnSave]").show();
    $("[id$=TermTitle]").focus();
    return false;
}
///<summary>Used for initial settings</summary>
function PageInit() {

    $('[id$=btnSave]').hide();
    $('[id$=btnAdd]').show();
    $('[id$=divData]').hide();
    $('[id$=divListing]').show();

    SearchInit();
    SetSearchType();

    ResetPage();
    BindGrid(0);
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());

}

///#endregion

///#region Gridhandler &  Maodal Ok

 ///<summary>Handle the grid events</summary>
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
         case VendorTermsMaster.DeleteCommand:
             TermId = GrandGrid.Utilities.GetColumnValue(tr, VendorTermsMaster.TermsPK, $(tr).parent().attr("id"));
             // Do Confirmation.. Before Delete Details
             GrandScriptUtils.ShowModal(VendorTermsMaster.DeleteConfirmation, VendorTermsMaster.ConfirmationMessage, VendorTermsMaster.DeleteMessageCommand, true);
             break;

         // To Edit Details              
         case VendorTermsMaster.EditCommand:
             FillDetails(tr);
             break;
         // Default Handler    
         default:
             alert(MaterialMaster.DefaultAction);
             break;
     }
     return false;

 }
 ///<summary>Event handler for Model ok click</summary>
 function ModalOk(command) {
     ///<summary>Function invoke after Model popup ok Click</summary>
     /// <param name="command"  type="object">
     ///      delete
     /// </param>
     switch (command) {
         //comment req   
         case VendorTermsMaster.DeleteCommand:
             BindGrid();
             break;
         //Commend When calling   
         case VendorTermsMaster.DeleteMessageCommand:
             DeleteDetails();
             break;
         case "SaveOK":
             PageInit();


     }
     return false;
 }

 ///#endregion

///#region Data Management 

 ///<Summary>Method to save the  page<summary>
 function SavePage() {
     var IsItemActive = $("input[id$=chkItemActive]").is(':checked');
     $("[id$=ACTIVE]").val(IsItemActive == true ? "1" : "0");
     //Add Validation 
     AddValidations(1);
     //validate
     if ($(document.forms[0]).valid()) {
         var jSonString = GrandScriptUtils.FormToJsonString(false);

         $.post(VendorTermsMaster.VendorTermsSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
             if (parseInt(data) > 0) {//if save success
                 GrandScriptUtils.ShowModal(VendorTermsMaster.TermsSaveMessage, VendorTermsMaster.MessageBoxTitle, "SaveOK");
                 // PageInit();
             }
             else if (parseInt(data) == -2) { //if update success
                 GrandScriptUtils.ShowModal(VendorTermsMaster.TermsSaveMessage, VendorTermsMaster.MessageBoxTitle, "SaveOK");
                 //PageInit();
             }
             else if (parseInt(data) == 0) {
                 GrandScriptUtils.ShowModal(VendorTermsMaster.AlreadyExists, VendorTermsMaster.Information, "SaveOK");
             }
             else {//fail
                 GrandScriptUtils.ShowModal(VendorTermsMaster.ActionFailed);
                 // ResetPage();
             }

         });
     }
     return false;
 }
 ///<summary>Bind data to the grid</summary>
 function BindGrid(srchVal) {
     var srchV = "";
     if (srchVal)
         srchV = srchVal;
     else
         srchV = $("[id$=SearchType]").val() == "0" ? "0" : $("[id$=SearchType]").val() + " Like '%" + $("[id$=SearchValue]").val() + "%'";
     srchV = GrandScriptUtils.FixURLEncoding(srchV);
     var ajaxUrl = VendorTermsMaster.VendorTermsListGet + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnitPk=" + $("[id$=BizUnitPk]").val();
     $("#grdTermsList").removeAttr("ajaxurl")
     $("#grdTermsList").attr("ajaxurl", ajaxUrl);
     GrandGrid.Utilities.ResetGrid(true, "grdTermsList");
     GrandGrid.MakeGrid($("#grdTermsList"));
     return false;
 }
 function AfterGridBind(gridID) {
     if (gridID == "grdTermsList") {
         $("#grdTermsList tr:has(td)").each(function (index) {
             colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VET_PK", gridID);
             // Replace Active/InActive status  with corresponding image     
             colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "VET_ACTIVE", gridID);
             if (colIndexActive != null) {
                 var itemActiveText = $(this).find("td:eq(" + colIndexActive + ")").html();
                 if (itemActiveText == 1) {
                     $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                 }
                 else {
                     $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                 }
             }
        });
     }
 }
 ///<summary>Delete Terms Detail </summary>
 function DeleteDetails(tr) {
     ///<summary>Function To Get delete and Delete Terms Detail, And Finally, Fill Remaining Data</summary>
     /// <param name="tr"  type="object">
     ///      deleted row
     /// </param>
     var msgtxt;
     $.get(VendorTermsMaster.DeleteCommandURL + TermId, function (data) {
         //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
         if (parseInt(data) == 1)//Delete success
             msgtxt = VendorTermsMaster.DeletedMessage;
         else if (parseInt(data) == 0)//Unable to delete dueu to dependencey
             msgtxt = VendorTermsMaster.CannotDelete;
         else//Delete failure
             msgtxt = VendorTermsMaster.ActionFailed;
         // Show MeesageBox For  Delete Status
         GrandScriptUtils.ShowModal(msgtxt, VendorTermsMaster.MessageBoxTitle, VendorTermsMaster.DeleteCommand);

     });
     return false;
 }

 function ClearForm() {
     $("input[id$=TermTitle]").val("");
     $("textarea[id$=TermDescr]").val("");
     $("select[id$=TermType]").val(0);
     $("input[id$=TermPK]").val("0");
 }
 
 ///<summary>Fill Values into the controls from the grid</summary>
 function FillDetails(tr) {
     $("input[id$=TermTitle]").val(GrandGrid.Utilities.GetColumnValue(tr, VendorTermsMaster.TermsTitle, $(tr).parent().attr("id")));
     $("textarea[id$=TermDescr]").val(GrandGrid.Utilities.GetColumnValue(tr, VendorTermsMaster.TermsDescr, $(tr).parent().attr("id")));
     $("select[id$=TermType]").val(GrandGrid.Utilities.GetColumnValue(tr, VendorTermsMaster.TermsType, $(tr).parent().attr("id")));
     $("input[id$=TermPK]").val(GrandGrid.Utilities.GetColumnValue(tr, VendorTermsMaster.TermsPK, $(tr).parent().attr("id")));
     if (GrandGrid.Utilities.GetColumnValue(tr, VendorTermsMaster.TermStatusText, $(tr).parent().attr("id")) == "ACTIVE")
         $("input[id$=chkItemActive]").attr("checked", true)
     else
         $("input[id$=chkItemActive]").attr("checked", false);
     //Change visibility
     $('[id$=divData]').show();
     $('[id$=divListing]').hide();
     $('[id$=btnSave]').show();
     $('[id$=btnAdd]').hide();

 }
 ///<summary>Clear all controls and change the visibility</summary>
 function ResetPage() {
     //<summary>function Used to Reset Page</summary>
     //Reseting all input controls in the page

//     var idval = "";
//     $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () {
//         idval = $(this).attr("id");
//         if (idval.search("TermPK") != -1)
//             $(this).val('0');
//         //Avoid UserPk to get the value of log in user
//         else if (idval.search("UserPk") == -1)
//             $(this).val("");
//     });

//     //Selecting the first value in all drop downs
//     $(document.forms[0]).find("select").each(function () {
//         idval = $(this).attr("id");
//         if (idval.search("SBU") == -1)
//             $(this).val($(this).find("option:eq(0)").val());
     //     });
     ClearForm()
     $('textarea[id$=TermDescr]').val('');
     // HideAdvSearch();
     $(document.forms[0]).validate().resetForm();
     $('[id$=btnSave]').hide();
     $('[id$=btnAdd]').show();
     $('[id$=divData]').hide();
     $('[id$=divListing]').show();
     // $("[id$=BizUnitPk]").val(SBUPK); //to be changed.
     $("[id$=SearchValue]").val('');
     BindGrid();
//     
//     $("[id$=imbSearch]").hide();
     return false;
 }

///#endregion

 ///#region---- Auto Complete Section ----

 function SetSearchType() {
     ///<summary>Function To Enable/Disable Selected Option For Search </summary>
//     var strname = $("select[id$=SearchType]").val();
//     $("[id$=SearchValue]").val("");
//     if (strname == "0") {
//         $("[id$=SearchValue]").hide()
//         $("[id$=imbSearch]").hide();
//         BindGrid();
//     }
//     else {
//         $("[id$=SearchValue]").show()
//         $("[id$=imbSearch]").show();
//     }
     $("[id$=SearchValue]").show()
     $("[id$=imbSearch]").show();
     BindGrid();
 }

 function AfterSelect() {
     BindGrid();
     return false;
 }
 ///<summary>Invoke advanced search</summary>
 function AdvanceSearchInvoke(srchVal) {
     BindGrid(srchVal);
 }
  ///<summary>To handle auto complete</summary>
 function SearchInit() {

     GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", VendorTermsMaster.AutoCompleteSearch + $("[id$=BizUnitPk]").val(), "SearchType");
 }
 ///#endregion

 ///#region Validaton
 ///<Summary>Add validations to controls<summary>
 function AddValidations(mode) {
     RemoveValidations();

     $('input[id$=TermTitle]').rules("add", {
         required: true,
         maxlength: 100,
         messages: { required:VendorTermsMaster.PleaseProvideTermTitle }
     });
     $('textarea[id$=TermDescr]').rules("add", {
         required: true,
         maxlength: 200,
         messages: { required: VendorTermsMaster.PleaseProvideTermDescription }
     });

     $('select[id$=TermType]').rules("add", {
         selectNone: true,
         messages: { selectNone: VendorTermsMaster.PleaseselectTermType }
     });

 }
 ///<Summary>Remove validations<summary>
 function RemoveValidations() {

     $('input[id$=TermTitle]').rules("remove");
     $('textarea[id$=TermDescr]').rules("remove");
     $('select[id$=TermType]').rules("remove");


 }

 ///#endregion
