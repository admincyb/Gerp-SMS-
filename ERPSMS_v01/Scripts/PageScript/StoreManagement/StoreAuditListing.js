


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
    $("select[id$=SearchType]").val("");
    $("[id$=SearchValue]").val("");
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    return false;
}






//<summary>function Used to Reset Page</summary>
function ResetPage() {
    ClearSearchDetails();
    PageInit();
    return false;
}



function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
      
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        //BindGrid();
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
   // GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", PurchaseRequest.PURCHASEREQUESTAUTOCOMPLETEURL + $("select[id$=SBU]").val(), "SearchType");
}

