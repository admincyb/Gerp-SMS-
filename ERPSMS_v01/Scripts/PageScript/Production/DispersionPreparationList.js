

///#region ------- Global Variable -----
var dispersionRqstPK = 0;
//var procID = 5;
var pk = 0;
var dispersionDecimal = 0;
///#endregion

//#region ------- Configuration Section -------
var DispersionPreparationList = {

    // URL
    GetDispersionDetailsListURL: "DispersionPreparation.do?Action=GetDispersionPreparationList&Status=",
    DeleteDispersionPreparationURL: "DispersionPreparation.do?Action=DeleteDispersionPreparation&DispersionID=",
    DISPERSIONPREPAUTOCOMPLETEURL: "DispersionPreparation.do?Action=GetSearchValue",
    DISPERSIONPREPARATIONURL: "SemiFinishedGoods.aspx",
    PURCHASEREQUESTREPORTURL: "PurchaseRequestReport.aspx",
    QCURL: "/RawMaterialTest/RawMaterialInspectionTest.aspx?TYPE=2",
    PerformAction: "PERFORMACTION",

    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    DSP_NAME: "DSP_NAME",
    DISPPK: "DTH_PK",
    USERSTATUS: "USER_STATUS",
    DSP_TYPE_TEXT: "DSP_TYPE_TEXT",
    DTH_STATUS: "DTH_STATUS",
    View: "VIEW",
    PRINT: "PRINT",
    QC: "QC",
    RefID: "REF_ID",
    CANCELDISPERSION: "CANCELDISPERSION",
    // Messages
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Translate(SFGPreparationDeletedSuccessfully)",
    SaveMessage1: "Translate(SFGPreparationSaved1)",
    SaveMessage2: "Translate(SFGPreparationSaved2)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",
    NotDispersionStockExists: "Translate(NotSFGMaterialStockExists)",
    REPORTURL: "../Reports/GenerateReport.aspx"
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
function ShowHideAdvSearch(flag) {
    //If flag then Show AdvancedSearch
    if (flag) {
        $("[id$=tbladvSearch]").show();
        $("[id$=imbShowFilter]").hide();
        $("[id$=imbHideFilter]").show();
        $("[id$=hdfShowHideFilter]").val("0");
    }
    else {
        $("[id$=tbladvSearch]").hide();
        $("[id$=imbShowFilter]").show();
        $("[id$=imbHideFilter]").hide();
        $("[id$=hdfShowHideFilter]").val("1");
    }
    return false;
}
function PageInit() {
    ///<summary>Initial page condition</summary>
    //$("select[id$=SearchType]").val(DispersionPreparationList.DSP_NAME);
    $("select[id$=SearchType]").val(DispersionPreparationList.TEXTZERO);
    $("[id$=SearchValue]").val("Translate(AutoDefaultValue)");
    SearchInit();
    SetSearchType();
    BindGrid();
    $("[id$=SearchType]").focus();
    dispersionDecimal = $("[id$=hdfCompoundingDecimal]").val();
    ShowHideAdvSearch();
    return false;
}

///#endregion

///#region ------- Core Section -------


///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>

    window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL;
    return false;
}


function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    ShowHideAdvSearch();
    ClearSearchDetails();
    PageInit();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("Translate(AutoDefaultValue)");
    ClearSearchDetails();
    if (strname == DispersionPreparationList.TEXTZERO) {
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

function BindGrid() {
    ///<summary>To handle bind grid </summary>
    var searchVal;
    if ($("[id$=SearchValue]").val() == "Translate(AutoDefaultValue)") {
        searchVal = DispersionPreparationList.TEXTEMPTY;
    }
    else {
        searchVal = $("[id$=SearchValue]").val();
    }
    var ajaxUrl = DispersionPreparationList.GetDispersionDetailsListURL + $("[id$=SearchType]").val() + "&SearchValue=" + encodeURIComponent(searchVal) + "&CancelFlag=" + ($("input[id$=ChkCancelStatus]").attr("checked") == true ? "1" : "0") + "&BizUnit=" + $("[id$=BizUnitPk]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&ProcId=" + $("[id$=hdfProcId]").val() + "&pageURL=" + $("[id$=hdfpageURL]").val();
    $("#grdDispersionList").removeAttr("ajaxurl")
    $("#grdDispersionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDispersionList");
    GrandGrid.MakeGrid($("#grdDispersionList"));
    return false;
}



function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val("Translate(AutoDefaultValue)");
    $("[id$=FromDate]").val(DispersionPreparationList.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(DispersionPreparationList.TEXTEMPTY);
    $("[id$=ToDate]").val(DispersionPreparationList.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(DispersionPreparationList.TEXTEMPTY);
    $("input[id$=ChkCancelStatus]").attr("checked", false);
    //$("input[id$=ChkShowAll]").attr("checked", false);
    return false;
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>

    GrandScriptUtils.MakeAutoCompleteSearchItem("SearchValue", DispersionPreparationList.DISPERSIONPREPAUTOCOMPLETEURL + "&ProcID=" + $("[id$=hdfProcId]").val(), "SearchType", true);
}

///#endregion

function FillDetails(tr) {
    ///<summary>Function To Fill Purchase Request Details  </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.DISPPK, $(tr).parent().attr("id"));
    var status = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.USERSTATUS, $(tr).parent().attr("id"))
    window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?PK=" + pk + "&Status=" + status;
    return false;
}


function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(DispersionPreparationList.DeleteDispersionPreparationURL + dispersionRqstPK, function (data) {
        if (parseInt(data) == 1)//Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
            msgtxt = DispersionPreparationList.DELETESUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Assigned";
        else
            msgtxt = DispersionPreparationList.ACTIONFAILEDMSG;
        GrandScriptUtils.ShowModal(msgtxt, DispersionPreparationList.MessageBoxTitle, DispersionPreparationList.SAVECMD);
    });
    return false;
}



function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>

    BindGrid();
}

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.DISPPK, $(tr).parent().attr("id"));
    switch (command.toString()) {

        case DispersionPreparationList.PerformAction: // To Delete Details   
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.RefID, $(tr).parent().attr("id"));
                window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?RefID=" + refID;
            }
            else if (UserStatus == 2) {
                window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?PK=" + pk;
            }
            return false;
            break;
        case DispersionPreparationList.DELETECOMMAND:
            dispersionRqstPK = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.DISPPK, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(DispersionPreparationList.DELETECONFIRMMSG, DispersionPreparationList.CONFIRMMSG, DispersionPreparationList.DELETE, true);
            break;

        case DispersionPreparationList.EDITCOMMAND: // To Edit Details         
            FillDetails(tr);
            break;

        case DispersionPreparationList.View:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.RefID, $(tr).parent().attr("id"));
                window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?RefID=" + refID + "&Status=1";
            }
            else if (UserStatus == 2) {
                window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?PK=" + pk + "&Status=1";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?PK=" + pk + "&Status=1";
                }
                else {
                    window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?RefID=" + refID + "&Status=1";
                }
            }
            return false;
            break;
        case DispersionPreparationList.PRINT:
            var win = window.open(DispersionPreparationList.REPORTURL + '?ID=' + pk + '&APPTYPE=BOM', '_blank', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');
            if (!win) {
                var eMsg = "<span><ul><li>" + errorMessage + ' ' + window.location.host + "</li></ul></span>";
                GrandScriptUtils.ShowModal(eMsg, errorTitle);
                $("#MSGBox").addClass("error");
            }
            break;
        case DispersionPreparationList.QC:
            window.location = $("[id$=hdfQcPath]").val() + DispersionPreparationList.QCURL + '&Dep=' + $("[id$=hdfQcDept]").val() + '&PK=' + pk + '&QCTYPE=2';
            break;
        case DispersionPreparationList.CANCELDISPERSION:
            var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.USERSTATUS, $(tr).parent().attr("id"));
            if (UserStatus == 1) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.RefID, $(tr).parent().attr("id"));
                window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?RefID=" + refID + "&Status=2"; //Status =2 for cancellation in transaction
            }
            else if (UserStatus == 2) {
                window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?PK=" + pk + "&Status=2";
            }
            else if (UserStatus == 0) {
                var refID = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparationList.RefID, $(tr).parent().attr("id"));
                if (refID == 0) {
                    window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?PK=" + pk + "&Status=2";
                }
                else {
                    window.location = DispersionPreparationList.DISPERSIONPREPARATIONURL + "?RefID=" + refID + "&Status=2";
                }
            }
            return false;
            break;
        default: // Default Handler  
            GrandScriptUtils.ShowModal(DispersionPreparationList.DEFAULTACTION, DispersionPreparationList.MessageBoxTitle);
            break;
    }
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {

        case DispersionPreparationList.SAVECMD:
            PageInit();
            break;
        case DispersionPreparationList.DELETE:
            DeleteDetails();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    $("#grdDispersionList tr:has(td)").each(function () {
        var Index = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparationList.DSP_TYPE_TEXT, $(this).parents("table:first").attr("id"));
        var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparationList.USERSTATUS, $(this).parents("table:first").attr("id"));
        var dispType = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparationList.DSP_TYPE_TEXT, $(this).parents("table:first").attr("id"));
        var dispStatus = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparationList.DTH_STATUS, $(this).parents("table:first").attr("id"));
        var dispQty = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparationList.DTH_QUANTITY, $(this).parents("table:first").attr("id"));

        $(this).find("td:last input[id$=imbQc]").hide();
        if (UserStatus == 1) {//Action To perform for the logged in user
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").hide();
            $(this).find("td:last input[id$=imbView]").hide();
            if ($("[id$=hdfQcStatus]").val() == "1" && dispStatus != "3" && dispStatus != "6" && dispStatus != "7")
                $(this).find("td:last input[id$=imbQc]").show();
        }
        else if (UserStatus == 0) {//No Action to perform but he is a participent in the work flow
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDelete]").hide();
            if ($("[id$=hdfQcStatus]").val() == "1" && dispStatus != "3" && dispStatus != "6" && dispStatus != "7")
                $(this).find("td:last input[id$=imbQc]").show();
        }
        else if (UserStatus == 2) {//Draft will have this status
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDelete]").show();
            $(this).find("td:last input[id$=imbView]").hide();
            $(this).find("td:last input[id$=imbQc]").hide();
        }
        if (dispType == "null") {
            $(this).find("td:eq(" + Index + ")").html("");
        }

        var batchIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTH_BATCH_NO", "grdDispersionList");
        var BatchNo = GrandGrid.Utilities.GetColumnValue($(this), "DTH_BATCH_NO", "grdDispersionList");
        if (BatchNo == '') {
            $(this).find("td:eq(" + batchIndex + ")").html("[NEW]");
        }


        //Current Stock
        qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTH_QUANTITY", "grdDispersionList");
        quantity = GrandGrid.Utilities.GetColumnValue($(this), "DTH_QUANTITY", "grdDispersionList");
        if (!isNaN(quantity)) {
            //quantity = parseFloat(quantity).toFixed(dispersionDecimal);
            quantity = addCommasForWeight(parseFloat(quantity).toFixed(dispersionDecimal));
        }
        $(this).find("td:eq(" + qtyIndex + ")").attr('style', 'text-align: right');
        if (quantity == "undefined" || quantity == "null" || quantity == "") {
            $(this).find("td:eq(" + qtyIndex + ")").html("0");
        }
        else {
            $(this).find("td:eq(" + qtyIndex + ")").html(quantity);
        }
        //Status
        var Index = GrandGrid.Utilities.GetColumnIndex($(this), "DTH_STATUS", "grdDispersionList");
        var status = GrandGrid.Utilities.GetColumnValue($(this), "DTH_STATUS", "grdDispersionList");
        if ((status != 0 && status != 4 && status != 3) && $("[id$=hdfCancelRight]").val() == 1) {
            $(this).find("td:last input[id$=imbCancelDisp]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancelDisp]").hide();
        }
        /*
        //Balance Qty
        balqtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTH_QTY_BALANCE", "grdDispersionList");
        balquantity = GrandGrid.Utilities.GetColumnValue($(this), "DTH_QTY_BALANCE", "grdDispersionList");
        if (!isNaN(balquantity)) {
            //quantity = parseFloat(quantity).toFixed(dispersionDecimal);
            balquantity = addCommasForWeight(parseFloat(balquantity).toFixed(dispersionDecimal));
        }
        $(this).find("td:eq(" + balqtyIndex + ")").attr('style', 'text-align: right');
        if (balquantity == "undefined" || balquantity == "null" || balquantity == "") {
            $(this).find("td:eq(" + balqtyIndex + ")").html("0");
        }
        else {
            $(this).find("td:eq(" + balqtyIndex + ")").html(balquantity);
        }
        */
    });

}
//Comma Separation for Quantity & Amount  
function addCommasForWeight(number) {
    var FormattedNumber = number;
    var curGroup1 = 3;
    var curGroup2 = 3;
    var NumericPart = "", LastNumericPart = "", DecimalPart = "";

    DecimalPart = number.split('.')[1];
    (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
    NumericPart = number.split('.')[0];
    if (NumericPart.length > curGroup1) {
        LastNumericPart = NumericPart.substr(NumericPart.length - curGroup1, curGroup1);
        (LastNumericPart) ? LastNumericPart = "," + LastNumericPart : LastNumericPart = "";
    }
    if ((NumericPart.length - curGroup1) > 0) {
        NumericPart = NumericPart.substr(0, NumericPart.length - curGroup1);
        var pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
        var expression = new RegExp(pattern, "g");
        NumericPart = NumericPart.toString().replace(expression, ",");
    }
    FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
    return FormattedNumber;
}
///#endregion
