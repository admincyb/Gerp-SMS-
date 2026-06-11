/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region GlobalVariables
var vendorlID = 0;
var delvendorlID = 0;
var LastModDate = "";
var UserRoles = new Array();
var Role_Clerk = 5;
var Role_Head = 4;
var IsRole_Clerk = false;
var IsFinalApprovar = false;
var IsRole_Head = false;
///#endregion

//#region Configuration Section
var agentListing = {
    //Url
    GetAgentList: "AgentRegistration.do?Action=GetAgentDetails&Status=",
    DeleteAgentDetailsURL: "AgentRegistration.do?Action=DeleteAgentDetails&AgentID=",
    AutoCompleteURL: "AgentRegistration.do?Action=GetSearchValue&AUTOSEARCH=1",
    PAGEURL: "/VendorManagement/agentListing.aspx",
    EvalUrl: "VendorEvaluation.aspx?PRefID=",
    ViewUrl: "VendorEvaluation.aspx?AgentID=",
    CreateAgent: "AgentMaster.aspx?Type=",
    PerformAction: "PERFORMACTION",
    Delete: "DELETE",
    Evaluate: "EVALUATE",
    View: "VIEW",
    AgentID: "VEN_PK",
    LastModDate:"VEN_MOD_DT",
    RefID: "REF_ID",
    TypeID: "TypeID",
    CommissionRate: "COMMISSIONRATE",
    CommSetupURL:"../Finance/commsetup.aspx?AgentId=",
   
    UserStatus: "USER_STATUS",
    VendorStats: "VEN_TYPE",
    //Message
    DeleteMessage: "Do you want to delete?",
    DeleteTitle: "Information",
    DeleteFailed: "Can't delete, as it is referenced in some other forms",
    VendorSaveMessage: "Translate(VendorSavedMessage)",
    AgentConcurrencyMsg: "Translate(EditUsedByAnotherUser)",
    AlreadyDeletedMsg: "Translate(AlreadyDeleted)",
    Information: "Translate(Information)",
    ConcurrencyExist:"CONCURRENCYEXIST"
}
//#endregion

//#region initialization Section
$(document).ready(function () {
    //Page Initial condtions
    PageInit();
    ShowWorkflowSaveMsg();

});

function PageInit() {

    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("=");
            if (pK[1] != "" && pK[0] == "Type") {
                agentListing.TypeID = pK[1];
            }
        }
    }
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    GetRoles();
    BindGrid("VEN_NAME");   
}

function AddNew() {
    ///<summary>Will redirect the listing page to vendor creation screen</summary>
    window.location = agentListing.CreateAgent + agentListing.TypeID;
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
                if (parseInt(pK[1]) > 0) {
                    GrandScriptUtils.ShowModal(agentListing.VendorSaveMessage, "Information");
                }
            }
        }
    }
}

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
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
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", agentListing.AutoCompleteURL + "&ProcId=" + $("[id$=hdfProcId]").val(), "SearchType");
}
///#endregion

//#endregion

//#region CoreSection
function ResetPage() {
    $("[id$=SearchValue]").val("");
    $("select[id$=SearchType]").val("VEN_CODE");
    $("[id$=SearchValue]").show()
    $("[id$=imbSearch]").show();
    BindGrid("VEN_NAME");

    return false;

}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var SearchVal = $("[id$=SearchValue]").val();
    var ajaxUrl = agentListing.GetAgentList + $("[id$=SearchType]").val() + "&SearchValue=" + encodeURIComponent(SearchVal) + "&UserPk=" + $("input[id$=UserPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + agentListing.PAGEURL + "&Type=" + agentListing.TypeID;

    $("#grdVendorDetails").removeAttr("ajaxurl")
    $("#grdVendorDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdVendorDetails");
    GrandGrid.MakeGrid($("#grdVendorDetails"));
    return false;
}

function AfterGridBind() {
    $("#grdVendorDetails").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, agentListing.UserStatus, tableID);
        var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, agentListing.VendorStats, tableID);
        var workflowStatus = GrandGrid.Utilities.GetColumnValue(this, "VEN_STATUS", tableID);

        //for hide edit button for submitted user
        if (UserStatus == 1 && ApproveStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbUpdate]").hide();
           // $(this).find("td:last input[id$=imbPrint]").show();
        }
        //Action To perform for the logged in user
        else if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbUpdate]").hide();
           // $(this).find("td:last input[id$=imbPrint]").show();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) {
            if (ApproveStatus == 1) {
                $(this).find("td:last input[id$=imbUpdate]").show();
                $(this).find("td:last input[id$=imbUpdate]").removeAttr("title");
                $(this).find("td:last input[id$=imbUpdate]").attr("title", "Update Agent");
               // $(this).find("td:last input[id$=imbPrint]").show();
            }
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }

        //Draft will have this status
        else if (UserStatus == 2) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbUpdate]").hide();
        }

        if (ApproveStatus == 1 && IsRole_Clerk) {
            $(this).find("td:last input[id$=imbEvaluate]").show();
            $(this).find("td:last input[id$=imbUpdate]").hide();
        }
        else {
            $(this).find("td:last input[id$=imbEvaluate]").hide();
        }
        if (workflowStatus == 2 && IsFinalApprovar) {
            $(this).find("td:last input[id$=imbUpdate]").show();
        }
        else {
            $(this).find("td:last input[id$=imbUpdate]").hide();
        }


        //Set the visibility of Commission Rate Btn.(If Role is Agent(Type=11) then Show else hide)
        if (agentListing.TypeID == "11") {
        $(this).find("td:last input[id$=imbCommRate]").show();
        }
        else{
            $(this).find("td:last input[id$=imbCommRate]").hide();
        }

    });
    $("#grdVendorDetails tr:has(td)").each(function () {
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VEN_CONT_NAME", $(this).parents("table:first").attr("id"));
        if (colIndex != null) {
            colData = GrandGrid.Utilities.GetColumnValue($(this), "VEN_CONT_NAME", $(this).parents("table:first").attr("id"));
            if (colData == "null")
                colData = "-";
            $(this).find("td:eq(" + colIndex + ")").html(colData);
        }

        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VEN_PHONE", $(this).parents("table:first").attr("id"));
        if (colIndex != null) {
            colData = GrandGrid.Utilities.GetColumnValue($(this), "VEN_PHONE", $(this).parents("table:first").attr("id"));
            if (colData == "null")
                colData = "-";
            $(this).find("td:eq(" + colIndex + ")").html(colData);
        }

    });
}

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
        case agentListing.PerformAction:

            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, agentListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.RefID, $(tr).parent().attr("id"));
                var workflowStatus = GrandGrid.Utilities.GetColumnValue(tr, "VEN_STATUS", $(tr).parent().attr("id"));
                if (workflowStatus != "2") {
                    window.location = agentListing.CreateAgent + agentListing.TypeID + "&RefID=" + refID;
                }
                else {
                    window.location = agentListing.CreateAgent + agentListing.TypeID + "&RefID=" + refID + "&Status=1";
                }
            }
            else if (UserStatus == 2) {
                window.location = agentListing.CreateAgent + agentListing.TypeID + "&AgentID=" + vendorlID;
            }

            return false;
            break;
        case agentListing.Delete:
            delvendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));
            LastModDate = GrandGrid.Utilities.GetColumnValue(tr, agentListing.LastModDate, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(agentListing.DeleteMessage, agentListing.DeleteTitle, agentListing.Delete, true);
            return false;
            break;
        case agentListing.Evaluate:
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));
            var refID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.RefID, $(tr).parent().attr("id"));
            EvaluateVendor(vendorlID, refID);
            return false;
            break;
        case agentListing.View:
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, agentListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.RefID, $(tr).parent().attr("id"));
                window.location = agentListing.CreateAgent + agentListing.TypeID + "&RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = agentListing.CreateAgent + agentListing.TypeID + "&AgentID=" + vendorlID + "&Status=1";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = agentListing.CreateAgent + agentListing.TypeID + "&AgentID=" + vendorlID + "&Status=1";
                }
                else {
                    window.location = agentListing.CreateAgent + agentListing.TypeID + "&AgentID=" + vendorlID + "&Status=1";
                }
            }
            return false;
            break;
        case "UPDATE":
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));
            //Status= 2 Indicates its Updation on Vendor
            window.location = agentListing.CreateAgent + agentListing.TypeID + "&AgentID=" + vendorlID + "&Status=2";
            return false;
            break;

        case "PRINT":
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));           
            window.open("VendorReport.aspx?PK=" + vendorlID, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes");        
            return false;
            break;
        case agentListing.CommissionRate:
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, agentListing.AgentID, $(tr).parent().attr("id"));
            window.location = agentListing.CommSetupURL + vendorlID;
            return false;
            break;

    }
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req 
        case agentListing.Delete:
            DeleteDetails();
            break;

    }
    return false;
}

function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(agentListing.DeleteAgentDetailsURL + delvendorlID + "&LASTMODDT=" + LastModDate, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            BindGrid()
        }
        else if (parseInt(data) == 0) {
            GrandScriptUtils.ShowModal(agentListing.DeleteFailed, agentListing.DeleteTitle, false);
        }
        else if (parseInt(data) == -3) {
            GrandScriptUtils.ShowModal(agentListing.AgentConcurrencyMsg, agentListing.Information, agentListing.ConcurrencyExist);
        }
        else if (parseInt(data) == -5) {//Already Deleted  
            GrandScriptUtils.ShowModal(agentListing.AlreadyDeletedMsg, agentListing.Information);
        }


    });
    return false;
}

function EvaluateVendor(vendorlID, refID) {
    window.location = agentListing.EvalUrl + refID;
}

function ViewVendor(vendorlID) {
    indow.location = agentListing.ViewUrl + vendorlID;
}

///<summmary>Method to get the workflow user role.
function GetRoles() {
//    var Roles = new Array();
//    var FinalApprovar = $("[id$=hdnFinalApprovar]").val();
//    Roles = $("[id$=UserRoles]").val().split(",");
//    for (var i in Roles) {
//        if (Roles[i] == FinalApprovar) {
//            IsFinalApprovar = true;
//        }

//        if (Roles[i] == Role_Clerk) {

//            IsRole_Clerk = true;
//        }
//        if (Roles[i] == Role_Head) {
//            IsRole_Head = true;
//        }

    //    }
    //Single level workflow .So this user is the final Approver(For Editing)
    IsFinalApprovar = true;
}
//#endregion