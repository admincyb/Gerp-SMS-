
///#region ----------------------------GlobalVariables----------------------------
var BinID = 0;
///#endregion

//#region -------------------------Configuration Section--------------------------
var BinListing = {
    //Url
    AutoCompleteURL: "BinCardGeneration.do?Action=GetSearchValue",
    CreateBin: "BinCardGeneration.aspx",
    GetBinDetails: "BinCardGeneration.do?Action=GetBinDetails&Status=",
    DeleteBinDetails: "BinCardGeneration.do?Action=DeleteBinDetails&BinID=",
    //Fields
    BinID: "BCH_PK",
    //COmmands
    Delete: "DELETE",
    Edit: "EDIT",
    Print: "PRINT",
    View:"VIEW",


    //Message
    DeleteMessage: "Translate(Doyouwanttodeletethisdetails)",
    DeleteTitle: "Translate(Information)",
    ShortCloseMessage: "Translate(ConfirmclosePO)",
    DeleteFailed: "Translate(CannotDelete)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)"
    
}
//#endregion

//#region ------------------------- initialization Section ------------------------
$(document).ready(function () {
    //Page Initial condtions
    PageInit();


});

function PageInit() {

    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    //BindGrid
    BindGrid("BCH_NO");
}

function AddNew() {
    ///<summary>Will redirect the listing page to vendor creation screen</summary>
    window.location = BinListing.CreateBin;
    return false;
}

///#region--------------------------- Auto Complete Section ----------------------------

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(" ");
    if (strname == "0") {
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
    $("[id$=SearchValue]").val("");
    $("[id$=FromDate]").val("");
    $("input[id$=hdfFrmDate]").val("");
    $("[id$=ToDate]").val("");
    $("input[id$=hdfToDate]").val("");

}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", BinListing.AutoCompleteURL, "SearchType");
}
///#endregion

//#endregion

//#region -----------------------------Core section---------------------------
function ResetPage() {
    $("select[id$=SearchType]").val("0");
    //setting search type.
    SetSearchType();
    return false;

}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = BinListing.GetBinDetails + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&UserPk=" + $("input[id$=UserPk]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    $("#grdBinDetails").removeAttr("ajaxurl")
    $("#grdBinDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdBinDetails");
    GrandGrid.MakeGrid($("#grdBinDetails"));
    return false;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    BinID = GrandGrid.Utilities.GetColumnValue(tr, BinListing.BinID, $(tr).parent().attr("id"));
    switch (command.toString()) {
        // To Delete Details
        case BinListing.Edit:
            window.location = BinListing.CreateBin + "?BinID=" + BinID;
            break;
        case BinListing.View:
            window.location = BinListing.CreateBin + "?BinID=" + BinID + "&Status=1";
            break;
//        case BINListing.Print:
//            window.location = BinListing.ReportBiun + "?BinID=" + BinID;
//            break;
        case BinListing.Delete:
            GrandScriptUtils.ShowModal(BinListing.DeleteMessage, BinListing.DeleteTitle, BinListing.Delete, true);
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
        case BinListing.Delete:
            DeleteDetails();
            break;        
    }
    return false;
}


function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

function DeleteDetails() {
    ///<summary>Function To Get delete and Delete Po Details, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>

    var msgtxt;
    $.get(BinListing.DeleteBinDetails + BinID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            BindGrid()
        else if (parseInt(data) == 0)
            GrandScriptUtils.ShowModal(BinListing.DeleteFailed, BinListing.DeleteTitle, false);

    });
    return false;
}



//#endregion