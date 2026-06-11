/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region -----Global Variables-----
var materialHeaderId = 0;

///#endregion

///#region ------Configuration Section-----
var MaterialIssueList = {
 

    //Url
    AutoCompleteURL: "MaterialIssue.do?Action=GetSearchValue&AUTOSEARCH=1&SBUPk=",
    BindGridURL: "MaterialIssue.do?Action=GetMRIssueList&Status=",
    DeleteRowURL: "MaterialIssue.do?Action=DeleteMRIssue&MIPK=",
    DeleteP2PRowURL: "MaterialIssue.do?Action=DeleteMRIssuePlantToPlant&MIPK=",
    REDIRECTURLEDIT: "../StoreManagement/MaterialIssue.aspx?MIPK=",
    ADDNEWURL: "../Inventory/MRissue.aspx",
    //    PRINTURL: "../StoreManagement/MaterialIssueReport.aspx",
    PRINTURL: "../Reports/GenerateReport.aspx",
    PAGEURL: "/Inventory/MRissue.aspx",
    InventoryLockCheckingURL: "CommonManagement.do?Action=CheckInventoryLocking&Date=",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
   
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    MIListDeleteMessage: "Translate(MIListDeleteMessage)",
    MIListUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    PerformAction: "PERFORMACTION",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    ErrTransLockedMsg: "Translate(ErrTransLockedMsg)",
    CancelMessage: "Translate(ConfirmCancel)",
    CANCELSUCESS: "Translate(MICanceledSuccessfully)",
    UnableToCancel_MA_Exist: "Translate(UnableToCancel_MA_Exist)",  
    CancelMessage: "Translate(ConfirmCancel)",
    MsgCancelRefError: "Translate(MsgCancelRefError)",  
    VoucherAccLocked: "Translate(VoucherAccLocked)",
    TEXTEMPTY:"",
    //Constants
    TextZero: "0",
    ValueEmpty: ' ',
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    UserStatus: "USER_STATUS",
    View: "VIEW",
    Print: "PRINT",
    RefID: "REF_ID",
    //Fields
    MIHNO: "ICH_NO",
    MaterialHeaderId: "ICH_PK",
    IssueStatus: "ICH_STATUS",
    MODIFY: "MODIFY",
    CANCEL: "CANCEL",

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
    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) != 1) {
        var drpID = $("select[id$=SearchType]").attr("id");
        $("#" + drpID + " option[value=CMP_DISPLAY_CODE]").remove(); //No need to show Plant filter Type    
    }
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
    
    var DeleteUrl = MaterialIssueList.DeleteRowURL;
    if ($("[id$=hdfMenuType]").val() == "9")
        DeleteUrl = MaterialIssueList.DeleteP2PRowURL;

    $.get(DeleteUrl + materialHeaderId, function (data) {


        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)

            msgtxt = MaterialIssueList.MIListDeleteMessage;
        else if (parseInt(data) == 0)

            msgtxt = MaterialIssueList.MIListUsed;
        else
            msgtxt = MaterialIssueList.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, MaterialIssueList.MessageBoxTitle, MaterialIssueList.DeleteCommand);

    });
    return false;
}
///<summary>Function To Get requisitionHeaderId and Pass this requisitionHeaderId as a QueryString </summary>
function FillDetails(tr) {
    //Get OrderID From tr - For Pass this as QueryString
    requisitionHeaderId = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.MaterialHeaderId, $(tr).parent().attr("id"));
    window.location = MaterialIssueList.REDIRECTURLEDIT + materialHeaderId;
}
function BindGrid() {

    ///<summary>To handle bind grid corr. to the search type and search value</summary>
   // var ajaxUrl = MaterialIssueList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("id$=BizUnitPk]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    var pageUrl = MaterialIssueList.PAGEURL;
    var MenuType = 0;
    if (parseInt($("[id$=hdfMenuType]").val()) > 0) {
        pageUrl += "?TYPE=" + $("[id$=hdfMenuType]").val();
        MenuType = $("[id$=hdfMenuType]").val();
    }
    var ajaxUrl = MaterialIssueList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&PageUrl=" + pageUrl + "&MenuTYpe=" + MenuType;
   // var ajaxUrl = MaterialIssueList.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val();
    $("#grdMIList").removeAttr("ajaxurl")
    $("#grdMIList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdMIList");
    GrandGrid.MakeGrid($("#grdMIList"));
    return false;
}

function AfterSelect() {
    ///<summary>filling gridview after entering search value in search textbox</summary>
    BindGrid();

}
function AfterGridBind() {
    var isModify = $("[id$=hdnModify]").val();
    var isCancel = $("[id$=hdnCancel]").val();
    $("#grdMIList").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var UserStatus = GrandGrid.Utilities.GetColumnValue(this, MaterialIssueList.UserStatus, tableID);
        var MIH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssueList.IssueStatus, tableID);
        //Action To perform for the logged in user
        if (UserStatus == 1) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbView]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
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

        if (isModify == "1" && UserStatus == 0 && MIH_STATUS != 4 && MIH_STATUS != 5) {   //MIH_STATUS=> 4 (Cancelled),5(Closed)
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }

        if (isCancel == "1" && MIH_STATUS != 0 && MIH_STATUS != 4 && MIH_STATUS != 5) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }

        ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE", tableID);
        if (ColIndex != null) {
            $(this).find("td:eq(" + ColIndex + ")").css("font-weight", "bold");
        }

        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MIH_ITEM_TEXT", $(this).parents("table:first").attr("id"));
        var ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "MIH_ITEM_TEXT", $(this).parents("table:first").attr("id"));
        if (ItemIndex != null) {
            if (ItemDetails.length > 70) {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails.substring(0, 70) + "...</div>");
            }
            else {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails + "</div>");

            }
        }

        //Line Color
        var ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_LINE_COLOUR", tableID);
        if (ColIndex != null) {
            var lineColor = GrandGrid.Utilities.GetColumnValue($(this), "CMP_LINE_COLOUR", tableID);
            if (lineColor != "null") {
                ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE", tableID);
                if (ColIndex != null) {
                    $(this).find("td:eq(" + ColIndex + ")").addClass(lineColor);
                }
            }
        }
        
        specColIndex = GrandGrid.Utilities.GetColumnIndex($(this), MaterialIssueList.MIHNO, tableID);
        reqSpec = GrandGrid.Utilities.GetColumnValue($(this), MaterialIssueList.MIHNO, tableID) == "null" || GrandGrid.Utilities.GetColumnValue($(this), MaterialIssueList.MIHNO, tableID) == "undefined" ? "" : GrandGrid.Utilities.GetColumnValue($(this), MaterialIssueList.MIHNO, tableID);
        if (specColIndex != null) {
            if (reqSpec == "")
                $(this).find("td:eq(" + specColIndex + ")").html(MaterialIssueList.DocGenerationNewValue);
        }


    });
}
///#endregion

///#region---- Set Or Reset Form----


function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    //Set Redirecting Url for Enter New Details
    var NewUrl = MaterialIssueList.ADDNEWURL;
    if (parseInt($("[id$=hdfMenuType]").val()) > 0)
        NewUrl += "?TYPE=" + $("[id$=hdfMenuType]").val();
    window.location = NewUrl;
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
       return false;
}
///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == MaterialIssueList.TextZero) {
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

    $("[id$=SearchValue]").val(MaterialIssueList.TEXTEMPTY);
    $("[id$=FromDate]").val(MaterialIssueList.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(MaterialIssueList.TEXTEMPTY);
    $("[id$=ToDate]").val(MaterialIssueList.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(MaterialIssueList.TEXTEMPTY);

}


function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", MaterialIssueList.AutoCompleteURL+ $("[id$=BizUnitPk]").val() , "SearchType");
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
    materialHeaderId = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.MaterialHeaderId, $(tr).parent().attr("id"));
    var TransDate = GrandGrid.Utilities.GetColumnValue(tr, "ICH_DATE", $(tr).parent().attr("id")); 
    switch (command.toString()) {


        case MaterialIssueList.PerformAction:
            //vendorlID = GrandGrid.Utilities.GetColumnValue(tr, vendorListing.VendorID, $(tr).parent().attr("id"));
            var performUrl = MaterialIssueList.ADDNEWURL + "?MIPK=" + materialHeaderId;
            if (parseInt($("[id$=hdfMenuType]").val()) > 0)
                performUrl = MaterialIssueList.ADDNEWURL + "?TYPE=" + $("[id$=hdfMenuType]").val() + "&MIPK=" + materialHeaderId;
            window.location = performUrl;
            return false;
            break;

        // To Delete Details      
        case MaterialIssueList.DeleteCommand:
        // Do Confirmation.. Before Delete Details
          // GrandScriptUtils.ShowModal(MaterialIssueList.DeleteConfirmationMessage, MaterialIssueList.ConfirmationMessage, MaterialIssueList.DeleteMessageCommand, true);
            $.get(MaterialIssueList.InventoryLockCheckingURL + TransDate + "&Module=2", function (data) {
                if (data != null && data.length > 0) {
                    if (parseInt(data[0]) == 0) {
                        // Do Confirmation.. Before Delete Details
                        GrandScriptUtils.ShowModal(MaterialIssueList.DeleteConfirmationMessage, MaterialIssueList.ConfirmationMessage, MaterialIssueList.DeleteMessageCommand, true);
                    }
                    else {
                        var Err_TranslockedMsg = MaterialIssueList.ErrTransLockedMsg + MaterialIssueList.ValueEmpty + data[1];
                        GrandScriptUtils.ShowModal(Err_TranslockedMsg.fontcolor("red"), MaterialIssueList.MessageBoxTitle);
                    }
                }
            });             
            break;

        // To Edit Details              
        case MaterialIssueList.EditCommand:
            FillDetails(tr);
            break;


        case MaterialIssueList.View:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.UserStatus, $(tr).parent().attr("id"));
            var redirectUrl;
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.RefID, $(tr).parent().attr("id"));
                redirectUrl = MaterialIssueList.ADDNEWURL + "?RefID=" + refID + "&Status=1";
                if (parseInt($("[id$=hdfMenuType]").val()) > 0)
                    redirectUrl = MaterialIssueList.ADDNEWURL + "?TYPE=" + $("[id$=hdfMenuType]").val() + "&RefID=" + refID + "&Status=1";
                window.location = redirectUrl;
            }
            else if (UserStatus == 2) {
                redirectUrl = MaterialIssueList.ADDNEWURL + "?MIPK=" + materialHeaderId + "&Status=1";
                if (parseInt($("[id$=hdfMenuType]").val()) > 0)
                    redirectUrl = MaterialIssueList.ADDNEWURL + "?TYPE=" + $("[id$=hdfMenuType]").val() + "&MIPK=" + materialHeaderId + "&Status=1";;
                window.location = redirectUrl;
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    redirectUrl = MaterialIssueList.ADDNEWURL + "?MIPK=" + materialHeaderId + "&Status=1";
                    if (parseInt($("[id$=hdfMenuType]").val()) > 0)
                        redirectUrl = MaterialIssueList.ADDNEWURL + "?TYPE=" + $("[id$=hdfMenuType]").val() + "&MIPK=" + materialHeaderId + "&Status=1";
                    window.location = redirectUrl;
                }
                else {
                    redirectUrl = MaterialIssueList.ADDNEWURL + "?RefID=" + refID + "&Status=1";
                    if (parseInt($("[id$=hdfMenuType]").val()) > 0)
                        redirectUrl = MaterialIssueList.ADDNEWURL + "?TYPE=" + $("[id$=hdfMenuType]").val() + "&RefID=" + refID + "&Status=1";
                    window.location = redirectUrl;
                }
            }
            return false;
            break;
        case MaterialIssueList.Print:            
            var url = MaterialIssueList.PRINTURL + "?ID=" + materialHeaderId + "&APPTYPE=" + "MTI" + "&APPSUBTYPE=" + "";
            OpenPDF(url);
            return false;
            break;
        case MaterialIssueList.MODIFY:
            var refID = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.RefID, $(tr).parent().attr("id"));
            var EditUrl = MaterialIssueList.ADDNEWURL + "?RefID=" + refID + "&IsModify=1";
            if (parseInt($("[id$=hdfMenuType]").val()) > 0)
                EditUrl = MaterialIssueList.ADDNEWURL + "?TYPE=" + $("[id$=hdfMenuType]").val() + "&RefID=" + refID + "&IsModify=1";
            window.location = EditUrl;
            return false;
            break;
        case MaterialIssueList.CANCEL:
            materialHeaderId = GrandGrid.Utilities.GetColumnValue(tr, MaterialIssueList.MaterialHeaderId, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(MaterialIssueList.CancelMessage, MaterialIssueList.MessageBoxTitle, MaterialIssueList.CANCEL, true);
            return false;
            break;
        // Default Handler  Print   
        default:
            alert(MaterialIssueList.DefaultAction);
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
        case MaterialIssueList.DeleteCommand:
            BindGrid();
            break;
        //Commend When calling   
        case MaterialIssueList.DeleteMessageCommand:
            DeleteDetails();
            break;
        case MaterialIssueList.CANCEL:
            CancelMIDetails();
            break;
    }
    return false;
}

function CancelMIDetails() {
    var msgtxt;
    var DeleteUrl = MaterialIssueList.DeleteRowURL;
    if ($("[id$=hdfMenuType]").val() == "9")
        DeleteUrl = MaterialIssueList.DeleteP2PRowURL;
    $.get(DeleteUrl + materialHeaderId + "&UserPK=" + $("[id$=UserPk]").val(), function (data) {
        if (parseInt(data) == 1) {
            msgtxt = MaterialIssueList.CANCELSUCESS;
        }
        else if (parseInt(data) == -11) {//Cannot  cancel ,as it is referenced in some other forms           
            msgtxt = MaterialIssueList.MsgCancelRefError.fontcolor("red")+"<br/>" +data[1].fontcolor("red");
        }      
        else if (parseInt(data) == -12) {//MA Entry Exists
            msgtxt = MaterialIssueList.UnableToCancel_MA_Exist.fontcolor("red");
        }  
          else if (parseInt(data[0]) == -51) {//Account lock for the Voucher 
                 msgtxt = String.format(MaterialIssueList.VoucherAccLocked.fontcolor("red"), data[2]);
       }      
        else {
            msgtxt = MaterialIssueList.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, MaterialIssueList.MessageBoxTitle, MaterialIssueList.DeleteCommand);
    });
    return false;

}

///#endregion

///#endregion

