/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region GlobalVariables
var vendorlID = 0;
var delvendorlID = 0;
var UserRoles = new Array();
//var Role_Clerk = 6;
//var Role_Head = 5;
var Role_Clerk = 5;
var Role_Head = 4;
var IsRole_Clerk = false;
var IsFinalApprovar = false;
var IsRole_Head = false;
///#endregion

//#region Configuration Section
var vendorListing = {
    //Url
    //    GetVendorList: "VendorRegistration.do?Action=GetVendorDetails&Status=",
    GetVendorList: "VendorRegistration.do?Action=GetVendorDetails",
    GetVendorPOType: "VendorRegistration.do?Action=GetVendorPOType&BizUnit=",
    DeleteVendorDetailsURL: "VendorRegistration.do?Action=DeleteVendorDetails&VendorID=",
    AutoCompleteURL: "VendorRegistration.do?Action=GetSearchValue&AUTOSEARCH=1",
    PAGEURL: "/VendorManagement/VendorListing.aspx",
    EvalUrl: "VendorEvaluation.aspx?PRefID=",
    ViewUrl: "VendorEvaluation.aspx?VendorID=",
    //PurchaseSearchAutoCompleteURL: "POGeneration.do?Action=GetPurchaseAutoSearchValue&AUTOSEARCH=1",
    VendorNameAutoCompleteURL: "VendorRegistration.do?Action=GetVendorAutoSearch&AUTOSEARCH=1&SBUPk=",
    CreateVendor: "VendorMaster.aspx",
    PerformAction: "PERFORMACTION",
    Delete: "DELETE",
    Evaluate: "EVALUATE",
    View: "VIEW",
    VendorID: "VEN_PK",
    RefID: "REF_ID",
    //
    UserStatus: "USER_STATUS",
    VendorStats: "VEN_TYPE",
    CfgType: "PURCHASE TYPE",
    //Message
    DeleteMessage: "Do you want to delete?",
    DeleteTitle: "Information",
    DeleteFailed: "Can't delete, as it is referenced in some other forms",
    VendorSaveMessage: "Translate(VendorSavedMessage)",
    DocGenerationNewValue: "Translate(DocGenerationNew)"
}
//#endregion

//#region initialization Section
$(document).ready(function () {
    //Page Initial condtions
    PageInit();
    ShowWorkflowSaveMsg();
    ShowHideAdvancedSearch();
});

function PageInit(){
    
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    GetRoles();
    BindGrid("VEN_NAME");
}

function AddNew() {
    ///<summary>Will redirect the listing page to vendor creation screen</summary>
    window.location = vendorListing.CreateVendor;
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
                    GrandScriptUtils.ShowModal(vendorListing.VendorSaveMessage, "Information");
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
//    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", vendorListing.AutoCompleteURL + "&ProcId=" + $("[id$=hdfProcId]").val(), "SearchType");
    GrandScriptUtils.MakeAutoComplete("txtVendorName", vendorListing.VendorNameAutoCompleteURL + $("[id$=BizUnitPk]").val() + "&SearchType=VEN_NAME" + "&PageURL=" + vendorListing.PAGEURL, "hdfVendor", true, false, false, true);
    FillVendorType();
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

function ClearSearch() {
    $("[id$=txtVendorCode]").val("");
    $("[id$=hdfVendor]").val(0);
    $("[id$=txtPhone]").val("");  
    $("#chkActive").attr("checked", true);
    SearchInit();
    BindGrid();
}
function FillVendorType() {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    var drpID = $("select[id$=ddlVendorType]").attr("id");
    $.get(vendorListing.GetVendorPOType + $("[id$=BizUnitPk]").val() + "&CfgType=" + vendorListing.CfgType, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);        
    });
}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    //var SearchVal = $("[id$=SearchValue]").val().replace(/[&]/g, "ampersand");
    var vendorCode = $("[id$=txtVendorCode]").val().replace(/[&]/g, "ampersand");
    var vendorName = $("[id$=txtVendorName]").val().replace(/[&]/g, "ampersand");
    var vendorActive = $("#chkActive").is(":checked") ? "1" : "0";
    if (vendorName == "Select/Type")
        vendorName = "";
    //    var ajaxUrl = vendorListing.GetVendorList + $("[id$=SearchType]").val() + "&SearchValue=" + encodeURIComponent(SearchVal) + "&UserPk=" + $("input[id$=UserPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + vendorListing.PAGEURL; 
    var ajaxUrl = vendorListing.GetVendorList + "&venCode=" + vendorCode + "&SearchValue=" + vendorName + "&venType=" + $("select[id$=ddlVendorType]").val() + "&venPhone=" + $("[id$=txtPhone]").val() + "&venActive=" + vendorActive + "&UserPk=" + $("input[id$=UserPk]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + vendorListing.PAGEURL;
     
    $("#grdVendorDetails").removeAttr("ajaxurl")
    $("#grdVendorDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdVendorDetails");
    GrandGrid.MakeGrid($("#grdVendorDetails"));
    return false;
}

function AfterGridBind() {
    $("#grdVendorDetails tr:has(td)").each(function (index) {
        //Button visibility
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.UserStatus, tableID);
        var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        var workflowStatus = GrandGrid.Utilities.GetColumnValue(this, "VEN_STATUS", tableID);

        //for hide edit button for submitted user
        if (UserStatus == 1 && ApproveStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbUpdate]").hide();
            $(this).find("td:last input[id$=imbPrint]").hide();//Hide:As per the instruction from Manoj Sir(29-11-2021)
        }
        //Action To perform for the logged in user
        else if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbUpdate]").hide();
            $(this).find("td:last input[id$=imbPrint]").hide();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) {
            if (ApproveStatus == 1) {
                $(this).find("td:last input[id$=imbUpdate]").show();
                $(this).find("td:last input[id$=imbUpdate]").removeAttr("title");
                $(this).find("td:last input[id$=imbUpdate]").attr("title", "Update Vendor");
                $(this).find("td:last input[id$=imbPrint]").hide();
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
            //  $(this).find("td:last input[id$=imbUpdate]").hide();
        }
        if (workflowStatus == 2 && IsFinalApprovar) {
            $(this).find("td:last input[id$=imbUpdate]").show();
            $(this).find("td:last input[id$=imbUpdate]").removeAttr("title");
            $(this).find("td:last input[id$=imbUpdate]").attr("title", "Update Vendor");
        }
        //else {
        //    $(this).find("td:last input[id$=imbUpdate]").hide();
        //}

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

       // include active/in active status      
        colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "VEN_ACTIVE", $(this).parents("table:first").attr("id"));
        if (colIndexActive != null) {
            if ($(this).find("td:eq(" + colIndexActive + ")").html() == 0) {
                $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\" \" />");
            }
            else {
                $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\" \" />");
            }
        }

        CodeColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VEN_CODE", $(this).parents("table:first").attr("id"));
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), "VEN_CODE", $(this).parents("table:first").attr("id")) == "null" || GrandGrid.Utilities.GetColumnValue($(this), "VEN_CODE", $(this).parents("table:first").attr("id")) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "VEN_CODE", $(this).parents("table:first").attr("id"));
        if (CodeColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + CodeColIndex + ")").html(vendorListing.DocGenerationNewValue);
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
        case vendorListing.PerformAction:

            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
                var workflowStatus = GrandGrid.Utilities.GetColumnValue(tr, "VEN_STATUS", $(tr).parent().attr("id"));
                if (workflowStatus != "2") {
                    window.location = vendorListing.CreateVendor + "?RefID=" + refID;
                }
                else {
                    window.location = vendorListing.CreateVendor + "?RefID=" + refID + "&Status=1";
                }
            }
            else if (UserStatus == 2) {
                window.location = vendorListing.CreateVendor + "?VendorID=" + vendorlID;
            }
            return false;
            break;
        case vendorListing.Delete:
            delvendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(vendorListing.DeleteMessage, vendorListing.DeleteTitle, vendorListing.Delete, true);
            return false;
            break;
        case vendorListing.Evaluate:
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
            EvaluateVendor(vendorlID, refID);
            return false;
            break;
        case vendorListing.View:
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.UserStatus, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
                window.location = vendorListing.CreateVendor + "?RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = vendorListing.CreateVendor + "?VendorID=" + vendorlID + "&Status=1";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = vendorListing.CreateVendor + "?VendorID=" + vendorlID + "&Status=1";
                }
                else {
                    window.location = vendorListing.CreateVendor + "?VendorID=" + vendorlID + "&Status=1";
                }
            }
            return false;
            break;
        case "UPDATE":
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            //Status= 2 Indicates its Updation on Vendor
            window.location = vendorListing.CreateVendor + "?VendorID=" + vendorlID + "&Status=2";
            return false;
            break;

        case "PRINT":
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            //            window.location = "VendorReport.aspx?PK=" + vendorlID;
            window.open("VendorReport.aspx?PK=" + vendorlID, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes");
//  //          var url = "../VendorManagement/VendorReport.aspx?PK=" + vendorlID;
//    //        OpenPDF(url);
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
        case vendorListing.Delete:
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
    $.get(vendorListing.DeleteVendorDetailsURL + delvendorlID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            BindGrid()
        else if (parseInt(data) == 0)
            GrandScriptUtils.ShowModal(vendorListing.DeleteFailed, vendorListing.DeleteTitle,false);

    });
    return false;
}

function EvaluateVendor(vendorlID, refID) {
    window.location = vendorListing.EvalUrl + refID;
}

function ViewVendor(vendorlID) {
    indow.location = vendorListing.ViewUrl + vendorlID;
}

///<summmary>Method to get the workflow user role.
function GetRoles() {
    var Roles = new Array();
    var FinalApprovarList = new Array();
    FinalApprovarList = $("[id$=hdnFinalApprovar]").val().split(",");
    var list_count = FinalApprovarList.length;
    Roles = $("[id$=UserRoles]").val().split(",");
    for (var i in Roles) {

        for (var j in FinalApprovarList) {
            if (Roles[i] == FinalApprovarList[j]) {
                IsFinalApprovar = true;
            }
        }
       
        if (Roles[i] == Role_Clerk) {

            IsRole_Clerk = true;
        }
        if (Roles[i] == Role_Head) {
            IsRole_Head = true;
        }

    }
}
//#endregion

function ShowHideAdvancedSearch(flag) {
    //If flag then Show AdvancedSearch
    if (flag) {
        $("[id$=tbladvancedSearch]").show();
        $("[id$=imbShowFilter]").hide();
        $("[id$=imbHideFilter]").show();
    }
    else {
        $("[id$=tbladvancedSearch]").hide();
        $("[id$=imbShowFilter]").show();
        $("[id$=imbHideFilter]").hide();
    }
    return false;
}
