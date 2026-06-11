$(document).ready(function () {
    ///<summary>Document . Ready()</summary>

    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();
    BindGrid();
});

function PageInit() {

    ///<summary>Initial page condition</summary>
    $("select[id$=SearchType]").val("0");
    $("[id$=SearchValue]").val("");
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>

    ClearSearchDetails();
    PageInit();
    return false;
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>

    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", AuditList.AutoCompleteURL +"&SBUID=" +$("select[id$=BizUnitPk]").val(), "SearchType");
}

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        ClearSearchDetails();
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
        BindGrid();
    }
    ClearSearchDetails();
}

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val("");
    $("[id$=FromDate]").val("");
    $("input[id$=hdfFrmDate]").val("");
    $("[id$=ToDate]").val("");
    $("input[id$=hdfToDate]").val("");
}

////==========================================================================================================================================================

/////#region ------- Global Variable -----

var purchaseRqstPK = 0;
var pk = 0;

/////#endregion

////#region ------- Configuration Section -------

var AuditList = {
    PerformAction: "PERFORMACTION",
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    SAHPK: "SAH_PK",
    USERSTATUS: "USER_STATUS",
    STOCKVARIANCE: "STK_VARIANCE",
    ADJUSTMENT :"ADJUSTMENT",
    View: "VIEW",
    PRINT: "PRINT",
    RefID: "REF_ID",
    PAGEURL: "/StoreManagement/StoreAuditing.aspx",
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Store Audit deleted successfully",
    AutoCompleteURL: "StoreAuditManagement.do?Action=GetSearchValue&AUTOSEARCH=1",
    CONFIRMATIONTITLE:"Translate(Information)"
}
//#endregion

/////#region ------- Core Section -------

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get("StoreAuditManagement.do?Action=DeleteStoreAudit&PK=" + pk, function (data) {
        if (parseInt(data) == 1)
            msgtxt = AuditList.DELETESUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Translate(CannotDelete)";
        else
            msgtxt = AuditList.ACTIONFAILEDMSG;
        GrandScriptUtils.ShowModal(msgtxt, AuditList.INFORMATIONTITLE, AuditList.SAVECMD);
    });
    return false;
}

function BindGrid() {
    ///<summary>Bind Designaion Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>

    var ajaxUrl = "StoreAuditManagement.do?Action=GetStoreAuditList&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=BizUnitPk]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&ProcID=" + $("[id$=hdfProcId]").val() + "&PageUrl=" + AuditList.PAGEURL; 
    $("#grdStoreAuditList").removeAttr("ajaxurl")
    $("#grdStoreAuditList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdStoreAuditList");
    GrandGrid.MakeGrid($("#grdStoreAuditList"));
    return false;
}

/////#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>

    pk = GrandGrid.Utilities.GetColumnValue(tr, "SAH_PK", $(tr).parent().attr("id"));
    switch (command.toString()) {
        // To Delete Details      
        case AuditList.PerformAction:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, AuditList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, AuditList.RefID, $(tr).parent().attr("id"));
                window.location = "StoreAuditing.aspx" + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = "StoreAuditing.aspx" + "?PK=" + pk;
            }
            return false;
            break;
        case AuditList.DELETECOMMAND:
            GrandScriptUtils.ShowModal(AuditList.DELETECONFIRMMSG, AuditList.CONFIRMMSG, AuditList.DELETE, true);
            break;
        // To Edit Details                  
        case AuditList.EDITCOMMAND:
            FillDetails(tr);
            break;
        case AuditList.View:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, AuditList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus != "" && UserStatus != undefined && UserStatus != "undefined") {
                if (parseInt(UserStatus) == 1) {
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, AuditList.RefID, $(tr).parent().attr("id"));
                    window.location = "StoreAuditing.aspx" + "?RefID=" + refID + "&Status=1";
                }
                else if (parseInt(UserStatus) == 2) {
                    window.location = "StoreAuditing.aspx" + "?PK=" + pk + "&Status=1";
                }
                else if (parseInt(UserStatus) == 0) {
                    //window.location = GoodsInspection.GOODSINSPECTIONENTRYURL + "?PK=" + pk + "&Status=2";
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, AuditList.RefID, $(tr).parent().attr("id"));
                    if (refID == 0) {
                        window.location = "StoreAuditing.aspx" + "?PK=" + pk + "&Status=1";
                    }
                    else {
                        window.location = "StoreAuditing.aspx" + "?RefID=" + refID + "&Status=0";
                    }
                }
            }
            return false;
            break;
        case AuditList.PRINT: 
            var prID = GrandGrid.Utilities.GetColumnValue(tr, AuditList.PURCHASERQSTPK, $(tr).parent().attr("id"));
            //window.location = "StoreAuditingReport.aspx" + "?PK=" + pk;
            var url = "StoreAuditingReport.aspx" + "?PK=" + pk;
            OpenPDF(url);
            break;

        case AuditList.ADJUSTMENT:
            var prID = GrandGrid.Utilities.GetColumnValue(tr, AuditList.PURCHASERQSTPK, $(tr).parent().attr("id"));
            window.location = "StockAdjustment.aspx" + "?PK=" + pk;
            break;
        // Default Handler    
        default:
            GrandScriptUtils.ShowModal(AuditList.DEFAULTACTION, AuditList.INFORMATIONTITLE);
            break;
    }
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///    
    /// </param>

    switch (command) {

        case AuditList.SAVECMD:
            PageInit();
            BindGrid();
            break;
        case AuditList.DELETE:
            DeleteDetails();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    
    //var status = 0;
    $("#grdStoreAuditList tr:has(td)").each(function () {
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), AuditList.USERSTATUS, $(this).parents("table:first").attr("id"));
        var variencestatus = GrandGrid.Utilities.GetColumnValue($(this), AuditList.STOCKVARIANCE, $(this).parents("table:first").attr("id"));
        
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
            $(this).find("td:last input[id$=imbView]").show();
        }
        //Draft will have this status
        else if (UserStatus == 2) {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        
        // Check to Display Adjustment Button
        if (variencestatus == 1) {
            $(this).find("td:last input[id$=ImbAdjustment]").show();
        }
        else {
            $(this).find("td:last input[id$=ImbAdjustment]").hide();
        }
        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAH_ITEM_TEXT", $(this).parents("table:first").attr("id"));
        var ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "SAH_ITEM_TEXT", $(this).parents("table:first").attr("id"));
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
    });

}
/////#endregion


/////#endregion



////==========================================================================================================================================================







