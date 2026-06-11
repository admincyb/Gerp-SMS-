/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region -----Global Variables-----
var itemHeaderId = 0;

///#endregion

///#region ------Configuration Section-----
var NewItemRequestList = {
 

    //Url
    AutoCompleteURL: "NewItemRequest.do?Action=GetSearchValue&SBUPk=",
    BindGridURL: "NewItemRequest.do?Action=GetNIRList&Status=",
    DeleteRowURL: "NewItemRequest.do?Action=DeleteNIR&ItemID=",
    REDIRECTURLEDIT: "../StoreManagement/StoreRequisitionSlipCreation.aspx?ItemID=",
    ADDNEWURL: "../StoreManagement/NewItemRequest.aspx",
    PRINTURL: "../StoreManagement/StoreRequisitionReport.aspx",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
   
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    DeleteMessage: "Translate(NewItemDetailsDeletedSuccesfully)",
    Used: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    PerformAction: "PERFORMACTION",
   
    //Constants
    TextZero: "0",
    TEXTEMPTY: "",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    UserStatus: "USER_STATUS",
    View: "VIEW",
    Print: "PRINT",
    RefID: "REF_ID",
    //Fields

    ItemRequestId: "ITR_PK",
    SaveMessage1: "Translate(NewItemDetailsSaved1)",
    SaveMessage2: "Translate(NewItemDetailsSaved2)"

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
    ShowWorkflowSaveMsg();
});


function PageInit() {
    ///<summary>initial page condition</summary>

    //Reseting all input controls in the page
    $("[id$=imbSave]").hide();
    $("[id$=imbAdd]").show();
   
    $("[id$=divListing]").show();
    
    //Reseting all input controls in the page.
    //ResetPage();
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    //calling function for binding grid.
    BindGrid();

    return false;
}
function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if (pK[0] == "No") {
                GrandScriptUtils.ShowModal(NewItemRequestList.SaveMessage1 + " " + pK[1] + " " + NewItemRequestList.SaveMessage2, "Information");
            }
        }
    }
}
///#endregion

///#region---- Core Section Section----

///#region---- Data Management Section----


function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete storeDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    var msgtxt;

    $.get(NewItemRequestList.DeleteRowURL + ItemRequestId, function (data) {


        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)

            msgtxt = NewItemRequestList.DeleteMessage;
        else if (parseInt(data) == 0)

            msgtxt = NewItemRequestList.Used;
        else
            msgtxt = NewItemRequestList.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, NewItemRequestList.MessageBoxTitle, NewItemRequestList.DeleteCommand);

    });
    return false;
}
///<summary>Function To Get ItemRequestId and Pass this ItemRequestId as a QueryString </summary>
function FillDetails(tr) {
    //Get OrderID From tr - For Pass this as QueryString
    ItemRequestId = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.ItemRequestId, $(tr).parent().attr("id"));
    window.location = NewItemRequestList.REDIRECTURLEDIT + ItemRequestId;
}
function BindGrid() {

    ///<summary>To handle bind grid corr. to the search type and search value</summary>

    var ajaxUrl = NewItemRequestList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    $("#grdNewItemList").removeAttr("ajaxurl")
    $("#grdNewItemList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdNewItemList");
    GrandGrid.MakeGrid($("#grdNewItemList"));
    return false;
}

function AfterSelect() {
    ///<summary>filling gridview after entering search value in search textbox</summary>
    BindGrid();

}
function AfterGridBind() {
    ///<summary>Setting width of the template after binding grid</summary>
    //$("#grdStore th:last").width("5%");
    $("#grdNewItemList").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, NewItemRequestList.UserStatus, tableID);
        //  var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        //Action To perform for the logged in user
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        //Draft will have this status
        else if (UserStatus == 2) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbView]").hide();
                }


    });
}
///#endregion

///#region---- Set Or Reset Form----


function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    //Set Redirecting Url for Enter New Details
    window.location = NewItemRequestList.ADDNEWURL;
    return false;
}
function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    $(document.forms[0]).find("input").each(function () {
//        var idval = $(this).attr("id");
//        if (idval.search("StoreMasterID") != -1)
//            $(this).val("0");
//        //Avoid UserPk to get the value of log in user
//        else if (idval.search("UserPk") == -1)
        //            $(this).val("");
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
    PageInit();
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
    if (controlID.search("StoreMasterID") != -1) {
        return true;
    }
    if (controlID.search("hdfProcId") != -1) {
        return true;
    } 
    return false;
}
///#endregion

///#region---- Auto Complete Section ----



function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == NewItemRequestList.TextZero) {
        //$("[id$=SearchValue]").hide()
        ClearSearchDetails();
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        BindGrid();
    }
    else if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        $("[id$=imbSearch]").show();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
    }
    ClearSearchDetails();
}


function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val(NewItemRequestList.TEXTEMPTY);
    $("[id$=FromDate]").val(NewItemRequestList.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(NewItemRequestList.TEXTEMPTY);
    $("[id$=ToDate]").val(NewItemRequestList.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(NewItemRequestList.TEXTEMPTY);

}
function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", NewItemRequestList.AutoCompleteURL+ $("[id$=BizUnitPk]").val()+ "&ProcID=" + $("[id$=hdfProcId]").val() , "SearchType");
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
    ItemRequestId = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.ItemRequestId, $(tr).parent().attr("id"));
    switch (command.toString()) {


        case NewItemRequestList.PerformAction:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.RefID, $(tr).parent().attr("id"));
                window.location = NewItemRequestList.ADDNEWURL + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = NewItemRequestList.ADDNEWURL + "?NewRequestID=" + ItemRequestId;
            }
            return false;
            break;

        // To Delete Details      
        case NewItemRequestList.DeleteCommand:
            
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(NewItemRequestList.DeleteConfirmationMessage, NewItemRequestList.ConfirmationMessage, NewItemRequestList.DeleteMessageCommand, true);
            break;

        // To Edit Details              
        case NewItemRequestList.EditCommand:
            FillDetails(tr);
            break;


        case NewItemRequestList.View:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.RefID, $(tr).parent().attr("id"));
                window.location = NewItemRequestList.ADDNEWURL + "?RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = NewItemRequestList.ADDNEWURL + "?NewRequestID=" + ItemRequestId + "&Status=1";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, NewItemRequestList.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = NewItemRequestList.ADDNEWURL + "?NewRequestID=" + ItemRequestId + "&Status=1";
                }
                else {
                    window.location = NewItemRequestList.ADDNEWURL + "?RefID=" + refID + "&Status=1";
                }
            }
            return false;
            break;
        case NewItemRequestList.Print:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));

            window.location = NewItemRequestList.PRINTURL + "?NewRequestID=" + ItemRequestId;
            return false;
            break;
        // Default Handler  Print   
        default:
            alert(NewItemRequestList.DefaultAction);
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
        case NewItemRequestList.DeleteCommand:
            BindGrid();
            break;
        //Commend When calling   
        case NewItemRequestList.DeleteMessageCommand:
            DeleteDetails();
            break;
    }
    return false;
}

///#endregion

///#endregion

