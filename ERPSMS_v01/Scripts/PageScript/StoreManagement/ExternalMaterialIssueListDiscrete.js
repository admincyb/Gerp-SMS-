/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region -----Global Variables-----
var issuingHeaderId = 0;

///#endregion

///#region ------Configuration Section-----
var RequisitionList = {


    //Url

    AutoCompleteURL: "ExternalMaterialIssue.do?Action=GetSearchValue&SBUPk=",
    BindGridURL: "ExternalMaterialIssue.do?Action=GetExternalMaterialList&Status=",
    DeleteRowURL: "ExternalMaterialIssue.do?Action=DeleteExternalMaterialIssue&IssueID=",
    REDIRECTURLEDIT: "../StoreManagement/ExternalMaterialIssueDiscrete.aspx?IssueID=",
    ADDNEWURL: "../StoreManagement/ExternalMaterialIssueDiscrete.aspx",
    //PRINTURL: "../StoreManagement/ExternalMaterialIssueReport.aspx",
    PRINTURL: "../Reports/GenerateReport.aspx",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    PAGEURL: "/StoreManagement/ExternalMaterialIssueDiscrete.aspx",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(MaterialIsseDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    RequisitionCodeAlreadyAdded: "Translate(Materialcodelreadyexists)",
    PerformAction: "PERFORMACTION",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    UserStatus: "ICH_STATUS",
    View: "VIEW",
    Print: "PRINT",
    RefID: "REF_ID",
    //Fields
    ICHNO: "ICH_NO",
    ConsumptionHeaderId: "ICH_PK",

    RequisitionSaveMessage1: "Translate(RequisitionDetailsSaved1)",
    RequisitionSaveMessage2: "Translate(RequisitionDetailsSaved2)"

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

///#endregion

///#region---- Core Section Section----

///#region---- Data Management Section----


function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete storeDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    var msgtxt;

    $.get(RequisitionList.DeleteRowURL + issuingHeaderId, function (data) {


        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)

            msgtxt = RequisitionList.RequisitionDeleteMessage;
        else if (parseInt(data) == 0)

            msgtxt = RequisitionList.RequisitionUsed;
        else
            msgtxt = RequisitionList.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, RequisitionList.MessageBoxTitle, RequisitionList.DeleteCommand);

    });
    return false;
}
///<summary>Function To Get issuingHeaderId and Pass this issuingHeaderId as a QueryString </summary>
function FillDetails(tr) {
    //Get OrderID From tr - For Pass this as QueryString
    issuingHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.ConsumptionHeaderId, $(tr).parent().attr("id"));
    window.location = RequisitionList.REDIRECTURLEDIT + issuingHeaderId;
}
function BindGrid() {

    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    //    var Value = $("[id$=SearchValue]").val().toString();
    //    Value = Value.replace('&', '^');
    //    var ajaxUrl = RequisitionList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + Value + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val();
    //Type 1 for material issue
    var ajaxUrl = RequisitionList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&Type=1" + "&PageUrl=" + RequisitionList.PAGEURL;
    $("#grdRequsitionList").removeAttr("ajaxurl")
    $("#grdRequsitionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdRequsitionList");
    GrandGrid.MakeGrid($("#grdRequsitionList"));
    return false;
}

function AfterSelect() {
    ///<summary>filling gridview after entering search value in search textbox</summary>
    BindGrid();

}
function AfterGridBind() {
    ///<summary>Setting width of the template after binding grid</summary>
    //$("#grdStore th:last").width("5%");
    $("#grdRequsitionList").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, RequisitionList.UserStatus, tableID);
        //  var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        //Action To perform for the logged in user
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbView]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbView]").hide();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbPrint]").show();
        }
        //        //Draft will have this status
        //        else if (UserStatus == 2) {
        //            $(this).find("td:last input[id$=imbEdit]").show();
        //            $(this).find("td:last input[id$=imbDelete]").show();
        //            $(this).find("td:last input[id$=imbView]").hide();
        //        }

        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), RequisitionList.ICHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.ICHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.ICHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), RequisitionList.ICHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(RequisitionList.DocGenerationNewValue);
        }


    });
}
///#endregion

///#region---- Set Or Reset Form----


function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    //Set Redirecting Url for Enter New Details
    window.location = RequisitionList.ADDNEWURL;
    return false;
}
function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input").each(function () {
    //        //        var idval = $(this).attr("id");
    //        //        if (idval.search("StoreMasterID") != -1)
    //        //            $(this).val("0");
    //        //        //Avoid UserPk to get the value of log in user
    //        //        else if (idval.search("UserPk") == -1)
    //        //            $(this).val("");
    //        var idval = $(this).attr("id");
    //        if (!Checkstatus(idval)) {
    //            $(this).val("");
    //        }
    //    });

    //Selecting the first value in all drop downs
    //    $(document.forms[0]).find("select").each(function () {
    //        $(this).val($(this).find("option:eq(0)").val());
    //    });
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
    if (strname == RequisitionList.TextZero) {
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
    //Type 1 for material Issue
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", RequisitionList.AutoCompleteURL + $("[id$=BizUnitPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&Type=1", "SearchType");
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
    issuingHeaderId = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.ConsumptionHeaderId, $(tr).parent().attr("id"));
    switch (command.toString()) {


        case RequisitionList.PerformAction:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            //            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.UserStatus, $(tr).parent().attr("id"));
            //            if (UserStatus == 1) {
            //                var refID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.RefID, $(tr).parent().attr("id"));
            //                window.location = RequisitionList.ADDNEWURL + "?RefID=" + refID;
            //            }
            //            if (UserStatus == 2) {
            window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=0";
            //            }
            //            return false;
            break;

        // To Delete Details         
        case RequisitionList.DeleteCommand:

            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(RequisitionList.DeleteConfirmationMessage, RequisitionList.ConfirmationMessage, RequisitionList.DeleteMessageCommand, true);
            break;

        // To Edit Details                 
        case RequisitionList.EditCommand:
            FillDetails(tr);
            break;


        case RequisitionList.View:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, RequisitionList.UserStatus, $(tr).parent().attr("id"));

            if (UserStatus == 1) {

                window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=1";
            }

            else if (UserStatus == 0) {
                window.location = RequisitionList.ADDNEWURL + "?IssueID=" + issuingHeaderId + "&Status=0";
            }
            return false;
            break;
        case RequisitionList.Print:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));

            //window.location = RequisitionList.PRINTURL + "?ID=" + issuingHeaderId + "&APPTYPE=" + "EMI" + "&APPSUBTYPE=" + "";
            var url = RequisitionList.PRINTURL + "?ID=" + issuingHeaderId + "&APPTYPE=" + "EMI" + "&APPSUBTYPE=" + "";
            OpenPDF(url);
            return false;
            break;
        // Default Handler  Print      
        default:
            alert(RequisitionList.DefaultAction);
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
        case RequisitionList.DeleteCommand:
            BindGrid();
            break;
        //Commend When calling      
        case RequisitionList.DeleteMessageCommand:
            DeleteDetails();
            break;
    }
    return false;
}

///#endregion

///#endregion


