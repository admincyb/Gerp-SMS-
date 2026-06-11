/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region GlobalVariables
var vendorlID = 0;
var EvalID = 0;
var UserRoles = new Array();
//var Role_Clerk = 6;
//var Role_Head = 5;
var Role_Clerk =5;
var Role_Head =4;
var IsRole_Clerk=false;
var IsRole_Head=false;
///#endregion

//#region Configuration Section
var vendorListing = {
    //Url
    GetVendorList: "VendorEvaluationManagement.do?Action=GetEvaluationList&VendorID=",
    DeleteVendorDetailsURL: "VendorEvaluationManagement.do?Action=DeleteEvaluation&EvaluationID=",
    AutoCompleteURL: "VendorEvaluationManagement.do?Action=GetSearchValue&BizUnitPk=",
    PAGEURL: "/VendorManagement/VendorEvaluation.aspx",
    EvalUrl: "VendorEvaluation.aspx",
    ViewUrl: "VendorEvaluation.aspx?VendorID=",
    CreateVendorEval: "VendorEvaluation.aspx",
    ReportUrl: "../VendorManagement/VendorEvaluationReport.aspx",
    VENREPORTURL: "../Reports/GenerateReport.aspx",
    PerformAction: "PERFORMACTION",
    Delete: "DELETE",
    Evaluate: "EVALUATE",
    View: "VIEW",
    Print: "PRINT",
    VendorID: "VEH_VENDOR",
    RefID: "REF_ID",
    EvalID: "VEH_PK",
    EvalStatus: "VEH_STATUS",
    //
    UserStatus: "USER_STATUS",
    VendorStats: "VEN_TYPE",
    //Message
    DeleteSuccess: "Translate(EvaluationDeleteSuccess)",
    DeleteMessage: "Translate(Doyouwanttodeletethisdetails)",
    DeleteTitle: "Translate(Information)",
    DeleteFailed: "Translate(CannotdeleteAlreadyasigned)",
    Savedsuccessfully: 'Translate(EvaluationDetailsSavedSuccesfully)'

}
//#endregion

//#region initialization Section
$(document).ready(function () {
    var fdate = new Date();


    GrandScriptUtils.AddDateRange("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, false);
    // GrandScriptUtils.AddDateRange("ToDate", "hdfToDate", "ToDate", "hdfToDate", false, false);

    //Setting controls visiblity depends on search value
    $("[id$=SearchType]").change(function () {
        SetSearchType();
    });

    //Binding grid in search click
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });
    //filling parameters and vendors in dropdownss
    //Page Initial condtions
    PageInit();
    ShowWorkflowSaveMsg();

});
function PageInit(){
    
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    GetRoles();
    if (IsRole_Head) {//if Head is logged in hide new button
        $("[id$=imbAdd]").hide();
    }
    BindGrid("VEN_NAME");
}
function AddNew() {
    ///<summary>Will redirect the listing page to vendor creation screen</summary>
    window.location = vendorListing.CreateVendorEval;
    return false;
}

///#region---- Auto Complete Section ----

    function SetSearchType() {
        var strname = $("select[id$=SearchType]").val();

        $("[id$=SearchValue]").val("");
        if (strname == "0") {
            $("[id$=SearchValue]").hide()
            $("[id$=imbSearch]").hide();
            $("#divDate").hide();
            BindGrid();
        }
        else if (strname == "DATE_RANGE") {
            $("[id$=SearchValue]").hide()
            $("#divDate").show();
            $("[id$=imbSearch]").show();
        }

        else {
            $("#divDate").hide();
            $("[id$=SearchValue]").show()
            $("[id$=imbSearch]").show();
        }
    }
function SearchInit() {
    ///<summary>To handle auto complete</summary>
    var BizUninitPk=parseInt( $("[id$=BizUnitPk]").val());
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", vendorListing.AutoCompleteURL + BizUninitPk, "SearchType");
}

///#endregion

//#endregion

//#region CoreSection
function SerachEnterKey(event) {
    if (event.keyCode == 13) {
        BindGrid();
    }
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
                    GrandScriptUtils.ShowModal(vendorListing.Savedsuccessfully, "Information");
                }
            }
        }
    }
}

///<summary>Method to reset the page</summary>
function ResetPage() {
    $("select[id$=SearchType]").val("0");
    //setting search type.
    SetSearchType();
    BindGrid();
    return false;

}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = vendorListing.GetVendorList + $("[id$=VND_PK]").val() + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&UserPk=" + $("input[id$=UserPk]").val() + "&FromDate=" + $("input[id$=FromDate]").val() + "&ToDate=" + $("input[id$=ToDate]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + vendorListing.PAGEURL; 
    $("#grdVendorDetails").removeAttr("ajaxurl")
    $("#grdVendorDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdVendorDetails");
    GrandGrid.MakeGrid($("#grdVendorDetails"));
    return false;;
}

///<summmary>Method to get the workflow user role.
function GetRoles(){
 var Roles = new Array();
    Roles = $("[id$=UserRoles]").val().split(",");
    for (var i in Roles) {
        if (Roles[i] == Role_Clerk){

            IsRole_Clerk = true;
            }
             if (Roles[i] == Role_Head){
            IsRole_Head = true;
            }
     
    }
}
///<summary>Function called after grid bind</summary>
function AfterGridBind() {

    $("#grdVendorDetails").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var EvalStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.EvalStatus, tableID);
        //var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.UserStatus, tableID);
        //Saved as Draft.


        //---Old Section---
//        if (EvalStatus == 0) {
//            if (IsRole_Clerk) {
//                $(this).find("td:last input[id$=imbEdit]").show();
//                $(this).find("td:last input[id$=imbDelete]").show();
//                $(this).find("td:last input[id$=imbView]").hide();
//            }
//            else if (IsRole_Head) {
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").hide();
//            }
//            else {//unauthorized users
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").hide();
//            }
//        }
//        //Action To perform for the logged in user
//        else if (EvalStatus == 1) {

//            if (IsRole_Clerk) {
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").show();
//            }
//            else if (IsRole_Head) {
//                $(this).find("td:last input[id$=imbEdit]").show();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").show();
//            }
//            else {//unauthorized users
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").hide();
//            }
//        }
//        else if (EvalStatus == 2 || EvalStatus == 3) {


//            if (IsRole_Clerk) {
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").show();
//            }
//            else if (IsRole_Head) {
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").show();
//            }
//            else {//unauthorized users
//                $(this).find("td:last input[id$=imbEdit]").hide();
//                $(this).find("td:last input[id$=imbDelete]").hide();
//                $(this).find("td:last input[id$=imbView]").hide();
//            }
//        }
        //---Old Section---


        //---New Section---
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbPrint]").show();
        }
        //No Action to perform but he is a participent in the work flow
        else if (UserStatus == 0) {
            if (EvalStatus == 1) {
                $(this).find("td:last input[id$=imbPrint]").show();
            }
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
        }
        //Draft will have this status
        else if (UserStatus == 2) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
        }
        //---New Section---

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
     var EvalStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.EvalStatus, $(tr).parent().attr("id"));
     EvalID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.EvalID, $(tr).parent().attr("id"));
     var ReferenceID = GrandGrid.Utilities.GetColumnValue(tr, 'REF_ID', $(tr).parent().attr("id"));
     var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.UserStatus, $(tr).parent().attr("id"));
    switch (command.toString()) {
        // To Delete Details
        case vendorListing.PerformAction:
            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));

            if (UserStatus == 1) {//if Evaluated by user
                var refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
                window.location = vendorListing.EvalUrl + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = vendorListing.EvalUrl + "?EvalID=" + EvalID;
            }
            return false;
            break;
        case vendorListing.Delete:
        if(EvalStatus==0)
        {
            GrandScriptUtils.ShowModal(vendorListing.DeleteMessage, vendorListing.DeleteTitle, vendorListing.Delete,true);
            }
            return false;
            break;
        case vendorListing.Evaluate:
            EvaluateVendor(vendorlID);
            return false;
            break;
        case vendorListing.View:
            EvalID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.EvalID, $(tr).parent().attr("id"));
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.UserStatus, $(tr).parent().attr("id"));
            if (EvalStatus == 0) {
                window.location = vendorListing.EvalUrl + "?EvalID=" + EvalID + "&ReadOnly=" + 1;
            }
            else {
                window.location = vendorListing.EvalUrl + "?RefID=" + ReferenceID + "&ReadOnly=" + 1;
            
            }
            //            if (EvalStatus == 1&&IsRole_Head) {//is evaluated by clerk and to be reviewed by Head
            //                var refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
            //                window.location = vendorListing.EvalUrl + "?RefID=" + refID +"&ReadOnly="+1;
            //            }
            //            else if(EvalStatus == 0||EvalStatus == 2||EvalStatus == 3) 
            //             {
            
            //            }
            return false;

        case vendorListing.Print:
            EvalID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.EvalID, $(tr).parent().attr("id"));
         //   window.location = vendorListing.ReportUrl + "?PK=" + EvalID;
            var url = vendorListing.VENREPORTURL + "?ID=" + EvalID + "&APPTYPE=VNDEVAL&APPSUBTYPE=";
            //window.location = url;
            OpenPDF(url);
            return false;
            break;
    }
}
//---New---
//function GridHandler(tr, command) {
//    ///<summary>Grid Handler Catch all the grid events in this function </summary>
//    /// <param name="tr"  type="Object">
//    ///     Specific Container and its controls
//    /// </param>
//    /// <param name="command"  type="Object">
//    ///     Specific Edit/Delete
//    /// </param>

//    var workflowStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.EvalStatus, $(tr).parent().attr("id"));
//    EvalID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.EvalID, $(tr).parent().attr("id"));
//    var UserStatus = null;
//    var refID = null;

//    switch (command.toString()) {
//        case vendorListing.PerformAction:
//            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
//            UserStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.UserStatus, $(tr).parent().attr("id"));
//            if (UserStatus == 1) {
//                refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
//                if (workflowStatus != "2") {
//                    window.location = vendorListing.EvalUrl + "?RefID=" + refID;
//                }
//                else {
//                    window.location = vendorListing.EvalUrl + "?RefID=" + refID + "&Status=1";
//                }
//            }
//            else if (UserStatus == 2) {
//                window.location = vendorListing.EvalUrl + "?EvalID=" + EvalID;
//            }
//            return false;
//            break;
//        case vendorListing.Delete:
//            if (workflowStatus == 0) {
//                GrandScriptUtils.ShowModal(vendorListing.DeleteMessage, vendorListing.DeleteTitle, vendorListing.Delete, true);
//            }
//            return false;
//            break;
//        case vendorListing.Evaluate:
//            vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
//            EvaluateVendor(vendorlID);
//            return false;
//            break;
//        case vendorListing.View:
//            if (workflowStatus == 0) {
//                window.location = vendorListing.EvalUrl + "?EvalID=" + EvalID + "&ReadOnly=" + 1;
//            }
//            else {
//                window.location = vendorListing.EvalUrl + "?RefID=" + ReferenceID + "&ReadOnly=" + 1;

//            }
//            UserStatus = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.UserStatus, $(tr).parent().attr("id"));
//            if (UserStatus == 1) {
//                refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
//                window.location = vendorListing.EvalUrl + "?RefID=" + refID + "&Status=1";
//            }
//            else if (UserStatus == 2) {
//                window.location = vendorListing.EvalUrl + "?EvalID=" + vendorlID + "&Status=1";
//            }
//            else if (UserStatus == 0) {
//                refID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.RefID, $(tr).parent().attr("id"));
//                if (refID == 0) {
//                    window.location = vendorListing.EvalUrl + "?EvalID=" + vendorlID + "&Status=1";
//                }
//                else {
//                    window.location = vendorListing.EvalUrl + "?EvalID=" + vendorlID + "&Status=1";
//                }
//            }
//            return false;

//        case vendorListing.Print:
//            window.location = vendorListing.ReportUrl + "?PK=" + EvalID;
//            return false;
//            break;
//    }
//}
//---New---

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
    $.get(vendorListing.DeleteVendorDetailsURL + EvalID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            GrandScriptUtils.ShowModal(vendorListing.DeleteSuccess, vendorListing.DeleteTitle, false);
            BindGrid();
        }
        else if (parseInt(data) == 0)
            GrandScriptUtils.ShowModal(vendorListing.DeleteFailed, vendorListing.DeleteTitle, false);

    });
    return false;
}
///Method to redirect to vendorevaluation.aspx
function EvaluateVendor(vendorlID) {
    window.location = vendorListing.EvalUrl + vendorlID;
}
///<summary>Method to redirect to vendor listing
function ViewVendor(vendorlID) {
    window.location = vendorListing.ViewUrl + vendorlID;
}
//#endregion