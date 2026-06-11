///#region ------- Global Variable -----
var pk = 0;
///#endregion

//#region ------- Configuration Section -------
var TopUp = {
    // URL
    TOPUPGRIDBINDURL: "TopUpManagement.do?Action=GetTOPUPList&Status=",
    TOPUPDELETEURL: "TopUpManagement.do?Action=DeleteTopUp&PK=",
    TOPUPAUTOCOMPLETEURL: "TopUpManagement.do?Action=GetSearchValue&SBU=",
    TOPUPENTRYURL: "TopUpRecord.aspx",
    PerformAction: "PERFORMACTION",
    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "delete",
    DELETE: "delete",
    EDITCOMMAND: "edit",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    TOPUPPK: "TUH_PK",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "TopUp Record Deleted Successfully"


}
//#endregion

///#region ------- Initialization Section ----------------
$(document).ready(function () {
    ///<summary>Document . Ready()</summary>
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();
});

function PageInit() {
    ///<summary>Initial page condition</summary>
    $("select[id$=SearchType]").val(TopUp.TEXTZERO);
    $("[id$=SearchValue]").val(TopUp.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    return false;
}

///#endregion

///#region ------- Core Section -------


///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    window.location = TopUp.TOPUPENTRYURL;
    return false;
}

//<summary>function Used to Reset Page</summary>
function ResetPage() {
    ClearSearchDetails();
    PageInit();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    ClearSearchDetails();
    if (strname == TopUp.TEXTZERO) {
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
   

   
}


function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val(TopUp.TEXTEMPTY);
    $("[id$=FromDate]").val(TopUp.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(TopUp.TEXTEMPTY);
    $("[id$=ToDate]").val(TopUp.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(TopUp.TEXTEMPTY);

}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", TopUp.TOPUPAUTOCOMPLETEURL + $("select[id$=SBU]").val(), "SearchType");
}

///#endregion

function FillDetails(tr) {
    ///<summary>Function To Fill TopUp  Details  </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    pk = GrandGrid.Utilities.GetColumnValue(tr, TopUp.TOPUPPK, $(tr).parent().attr("id"));
    window.location = TopUp.TOPUPENTRYURL + "?PK=" + pk ;
    return false;
}


function DeleteDetails() {
    ///<summary>Delete Topup Details </summary>
    var msgtxt;
    $.get(TopUp.TOPUPDELETEURL + pk, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = TopUp.DELETESUCESS;
//        else if (parseInt(data) == 0)
//            msgtxt = "Assigned";
        else
            msgtxt = TopUp.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, TopUp.INFORMATIONTITLE, TopUp.SAVECMD);

    });
    return false;
}

function BindGrid() {
    ///<summary>Bind TopUp Details Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>
    var ajaxUrl = TopUp.TOPUPGRIDBINDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    $("#grdPurchaseRequest").removeAttr("ajaxurl")
    $("#grdPurchaseRequest").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPurchaseRequest");
    GrandGrid.MakeGrid($("#grdPurchaseRequest"));
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    pk = GrandGrid.Utilities.GetColumnValue(tr, TopUp.TOPUPPK, $(tr).parent().attr("id"));
    switch (command.toString()) {
        // To Delete Details      

        case TopUp.DELETECOMMAND:
            pk = GrandGrid.Utilities.GetColumnValue(tr, TopUp.TOPUPPK, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(TopUp.DELETECONFIRMMSG, TopUp.CONFIRMATIONTITLE, TopUp.DELETE, true);
            break;
        // To Edit Details                  
        case TopUp.EDITCOMMAND:
            FillDetails(tr);
            break;
        default:
            GrandScriptUtils.ShowModal(TopUp.DEFAULTACTION, TopUp.INFORMATIONTITLE);
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

        case TopUp.SAVECMD:
            PageInit();
            break;
        case TopUp.DELETE:
            DeleteDetails();
            break;
    }
    return false;
}


///#endregion


///#endregion





