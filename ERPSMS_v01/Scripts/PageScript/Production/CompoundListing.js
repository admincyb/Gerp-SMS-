/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../MasterPage.js" />


///#region GlobalVariables
var CompoundlID = 0;
var EvalID = 0;
var UserRoles = new Array();
//var Role_Clerk = 6;
//var Role_Head = 5;
var Role_Clerk = 5;
var Role_Head = 4;
var IsRole_Clerk = false;
var IsRole_Head = false;
var CompoundRqstPK = 0;
///#endregion

//#region Configuration Section
var CompoundListing = {
    //URLs
CreateCompoundEval:"CompoundingPreparation.aspx",
GetCompoundList: "CompoundPreparation.do?Action=GetCompoundTrxList",
 // URL
    GetCompoundDetailsListURL: "CompoundPreparation.do?Action=GetCompoundPreparationList&Status=",
    DeleteCompoundPreparationURL: "CompoundPreparation.do?Action=DeleteCompoundPreparation&CompoundID=",
    CompoundPREPAUTOCOMPLETEURL: "CompoundPreparation.do?Action=GetSearchValue",
    CompoundPREPARATIONURL: "CompoundingPreparation.aspx",
    PerformAction: "PERFORMACTION",
    SaveMessage1: "Translate(CompoundTransactionSaved1)",
    SaveMessage2: "Translate(CompoundTransactionSaved2)",
    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    DISPPK: "CTH_PK",
    USERSTATUS: "USER_STATUS",
    View: "VIEW",
    PRINT: "PRINT",
    RefID: "REF_ID",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Translate(CompoundPreparationDeletedSuccessfully)" ,
    REPORTURL: "../Reports/GenerateReport.aspx"

}
///#endregion

//#region initialization Section
$(document).ready(function () {

    //Setting controls visiblity depends on search value
    $("[id$=SearchType]").change(function () {
        SetSearchType();
    });

    //Binding grid in search click
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });
    //filling parameters and Compounds in dropdownss
    //Page Initial condtions
    PageInit();
    // To show Workflow Save Msg
    ShowWorkflowSaveMsg();

});
function PageInit() {

    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
   
    BindGrid();
}
function AddNew() {
    ///<summary>Will redirect the listing page to Compound creation screen</summary>
    window.location = CompoundListing.CreateCompoundEval;
    return false;
}

///#region---- Auto Complete Section ----

///<summary>To handle auto complete</summary>
function SearchInit() {
    var SBU = parseInt($("[id$=SBU]").val());
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", "CompoundPreparation.do?Action=GetSearchValue&SBU=" + SBU + "&ProcID=" + $("[id$=hdfProcId]").val(), "SearchType");
    
}

///<summary>Function To Enable/Disable Selected Option For Search </summary>

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(CompoundListing.TEXTEMPTY);
    if (strname == CompoundListing.TEXTZERO) {
        //$("[id$=SearchValue]").hide()
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
    $("[id$=SearchValue]").val(CompoundListing.TEXTEMPTY);
    $("[id$=FromDate]").val(CompoundListing.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(CompoundListing.TEXTEMPTY);
    $("[id$=ToDate]").val(CompoundListing.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(CompoundListing.TEXTEMPTY);
    return false;
}
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "DISPESION") {
        BindMaterailGrid();
    }
}
///#endregion

//#endregion

//#region CoreSection

///<summary>Method to reset the page</summary>
function ResetPage() {
    ///<summary>Initial page condition</summary>
    $("select[id$=SearchType]").val(CompoundListing.TEXTZERO);
    $("[id$=SearchValue]").val(CompoundListing.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    ClearSearchDetails();
    BindGrid();
    $("[id$=SearchType]").focus();
    return false;

}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = CompoundListing.GetCompoundDetailsListURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&ProcID=" + $("[id$=hdfProcId]").val();
    //var ajaxUrl = CompoundListing.GetCompoundDetailsListURL + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();"&BizUnit=" + $("select[id$=SBU]").val()
    $("#grdCompoundDetails").removeAttr("ajaxurl")
    $("#grdCompoundDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdCompoundDetails");
    GrandGrid.MakeGrid($("#grdCompoundDetails"));
    return false;
}


function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    //var status = 0;
    $("#grdCompoundDetails tr:has(td)").each(function () {
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), CompoundListing.USERSTATUS, $(this).parents("table:first").attr("id"));
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
function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    pk = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.DISPPK, $(tr).parent().attr("id"));
    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.USERSTATUS, $(tr).parent().attr("id"));
    switch (command.toString()) {
        // To Delete Details       
        case CompoundListing.PerformAction:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.RefID, $(tr).parent().attr("id"));
                window.location = CompoundListing.CompoundPREPARATIONURL + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = CompoundListing.CompoundPREPARATIONURL + "?PK=" + pk;
            }
            return false;
            break;
        case CompoundListing.DELETECOMMAND:
            if (UserStatus == 2) {
                CompoundRqstPK = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.DISPPK, $(tr).parent().attr("id"));
                GrandScriptUtils.ShowModal(CompoundListing.DELETECONFIRMMSG, CompoundListing.CONFIRMMSG, CompoundListing.DELETE, true);
            }
            break;
        // To Edit Details                   
        case CompoundListing.EDITCOMMAND:
            FillDetails(tr);
            break;
        case CompoundListing.View:
            // var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.RefID, $(tr).parent().attr("id"));
                window.location = CompoundListing.CompoundPREPARATIONURL + "?RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = CompoundListing.CompoundPREPARATIONURL + "?PK=" + pk + "&Status=1";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, CompoundListing.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = CompoundListing.CompoundPREPARATIONURL + "?PK=" + pk + "&Status=1";
                }
                else {
                    window.location = CompoundListing.CompoundPREPARATIONURL + "?RefID=" + refID + "&Status=1";
                }
            }
            return false;
            break;
        case CompoundListing.PRINT:
            var win = window.open(CompoundListing.REPORTURL + '?ID=' + pk + '&APPTYPE=CMP', '_blank', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');
            if (!win) {
                var eMsg = "<span><ul><li>" + errorMessage + ' ' + window.location.host + "</li></ul></span>";
                GrandScriptUtils.ShowModal(eMsg, errorTitle);
                $("#MSGBox").addClass("error");
            }
            //MasterPage.OpenPDF(url);
            break;
        // Default Handler     
        default:
            GrandScriptUtils.ShowModal(CompoundListing.DEFAULTACTION, CompoundListing.INFORMATIONTITLE);
            break;
    }
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req 
        case CompoundListing.Delete:
            DeleteDetails();
            break;

    }
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>
    var msgtxt;
    $.get(CompoundListing.DeleteCompoundPreparationURL + CompoundRqstPK, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = CompoundListing.DELETESUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Translate(CannotDelete)";
        else
            msgtxt = CompoundListing.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, CompoundListing.INFORMATIONTITLE, CompoundListing.SAVECMD);
    });
    return false;
}

function ViewCompound(CompoundlID) {
    window.location = CompoundListing.ViewUrl + CompoundlID;
}
function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///    
    /// </param>
    switch (command) {

        case CompoundListing.SAVECMD:
            PageInit();
            break;
        case CompoundListing.DELETE:
            DeleteDetails();
            break;
    }
    return false;
}
function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(CompoundListing.TEXTEMPTY);
    $("[id$=FromDate]").val(CompoundListing.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(CompoundListing.TEXTEMPTY);
    $("[id$=ToDate]").val(CompoundListing.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(CompoundListing.TEXTEMPTY);

}


function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if (pK[0] == "No") {
                GrandScriptUtils.ShowModal(CompoundListing.SaveMessage1 + " " + pK[1] + " " + CompoundListing.SaveMessage2, "Information");
            }
        }
    }
}
//#endregion