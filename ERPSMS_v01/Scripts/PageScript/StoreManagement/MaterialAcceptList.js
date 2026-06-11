
///#region ------- Global Variable -----

var mahPK = 0;
var pk = 0;

///#endregion

//#region ------- Configuration Section -------

var MaterialAcceptList = {

    // URL
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    GRNLISTURL: "MaterialAccept.do?Action=GetMaterialAcceptList&Status=",
    GRNDELETEURL: "MaterialAccept.do?Action=DeleteMaterialAccept&MAHPK=",
    GETSTAFORCONVERTURL: "MaterialAccept.do?Action=GetSTAforConvert&MAHPK=",
    SAVESTACONVERSION: "MaterialAccept.do?Action=SaveSTAConversion",
    DELETESTACONVERSION: "MaterialAccept.do?Action=DeletSTAConversion&MAHPK=",
    ApplicationStatus: "CommonManagement.do?Action=GetAppStatus&AUTOSEARCH=1&Type=MA",
    MATERIALACCEPTAUTOCOMPLETEURL: "MaterialAccept.do?Action=GetSearchValue&SBU=",
    MATERIALACCEPTENTRYURL: "MaterialAccept.aspx",
    PAGEURL: "/storemanagement/MaterialAccept.aspx",
    ADDNEWURL: "../StoreManagement/MaterialAccept.aspx",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",
    REPORTURL: "../Reports/GenerateReport.aspx",

    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    LOGOUT: "LOGOUT",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    MAHPK: "MAH_PK",
    RefID: "REF_ID",
    MAHSTATUS: "MAH_STATUS",
    VIEWCOMMAND: "VIEW",
    PRINT: "PRINT",
    CONVERT: "CONVERT",
    NOTCONVERT: "NOTCONVERT",
    UserStatus: "USER_STATUS",
    AcceptStatus: "MAH_STATUS",
    MODIFY: "MODIFY",
    CANCEL: "CANCEL",

    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    DELETESUCESS: "Translate(MaterialAcceptDeletedSuccessfully)",
    CANCELCONFIRMMSG: "Translate(ConfirmCancel)",
    CANCELSUCESS: "Translate(MaterialAcceptCancelledSuccessfully)",
    CANCELREFMSG: "Translate(MsgCancelRefError)",
    UnableToCancelSTA: "Translate(UnableToCancelSTACCEPT)", 
    CONVERSIONSUCESS: "Translate(ConvertedSucessfully)", 
    CONVERSIONDELETE: "Translate(DeletedSuccessfully)", 
    INVALIDQTY: "Translate(InvalidStkQty)", 

    MRHConversionObject: new Object(),
    MRHConversion: new Array()
   


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
    $.validator.addMethod("NumericExceptZero", function (value, element) {
        return this.optional(element) || /(?!^0*$)\d+$/i.test(value);
    }, "Translate(NumericExceptZero)");
    PageInit();

});

function PageInit() {
    ///<summary>Initial page condition</summary>

    $("select[id$=SearchType]").val(MaterialAcceptList.TEXTZERO);
    $("[id$=SearchValue]").val(MaterialAcceptList.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    FillStatus();
    $("[id$=SearchType]").focus();
    //BindGrid();
    var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
    if (parseInt(isMultiplePlant) != 1) {
        var drpID = $("select[id$=SearchType]").attr("id");
        $("#" + drpID + " option[value=CMP_DISPLAY_CODE]").remove(); //No need to show Plant filter Type    
    }
    return false;
}

///#endregion

///#region ------- Core Section -------


///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    $.get(MaterialAcceptList.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(MaterialAcceptList.SessionExpired, MaterialAcceptList.Confirmation, MaterialAcceptList.LOGOUT, true);
            result = false;
        }
        else {
            window.location = MaterialAcceptList.MATERIALACCEPTENTRYURL;
        }
    });
    return false;
}

function FillStatus() {
    var drpID = $("select[id$=ddltrxstatus]").attr("id");
    $.get(MaterialAcceptList.ApplicationStatus, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, false, false, false, false, false, true);
        $("[id$=ddltrxstatus]").val("-1");
        BindGrid();
    });
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>

    ClearSearchDetails();
    PageInit();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>

    ClearSearchDetails();
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(MaterialAcceptList.TEXTEMPTY);
    if (strname == MaterialAcceptList.TEXTZERO) {
        //$("[id$=SearchValue]").hide()
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("[id$=imbSearch]").hide();
        $("#divSearchStatus").hide();
        BindGrid();
    }
    else if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        $("[id$=imbSearch]").show();
        $("#divSearchStatus").hide();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else if (strname == "MAH_STATUS") {
        $("#divSearchDtls").hide();
        $("#divDate").hide();
        $("#divSearchStatus").show();
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
        $("#divSearchStatus").hide();
    }
}


function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>

    $("[id$=SearchValue]").val(MaterialAcceptList.TEXTEMPTY);
    $("[id$=FromDate]").val(MaterialAcceptList.TEXTEMPTY);
    $("input[id$=hdfFrmDate]").val(MaterialAcceptList.TEXTEMPTY);
    $("[id$=ToDate]").val(MaterialAcceptList.TEXTEMPTY);
    $("input[id$=hdfToDate]").val(MaterialAcceptList.TEXTEMPTY);
    $("select[id$=ddltrxstatus]").val('-1');
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>

    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", MaterialAcceptList.MATERIALACCEPTAUTOCOMPLETEURL + $("select[id$=SBU]").val(), "SearchType");
}

///#endregion

function FillDetails(tr) {
    ///<summary>Function To Fill Purchase Request Details  </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
    window.location = MaterialAcceptList.MATERIALACCEPTENTRYURL + "?PK=" + pk;
    return false;
}

function ViewDetails(tr) {
    ///<summary>Function To Fill Purchase Request Details  </summary>

    pk = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
    //    window.location = MaterialAcceptList.MATERIALACCEPTENTRYURL + "?PK=" + pk + "&Status=1";
    var UserStatus = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.UserStatus, $(tr).parent().attr("id"));
    if (UserStatus == 1) {
        var refID = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.RefID, $(tr).parent().attr("id"));
        window.location = MaterialAcceptList.MATERIALACCEPTENTRYUR + "?RefID=" + refID + "&Status=1";
    }
    else if (UserStatus == 2) {
        window.location = MaterialAcceptList.MATERIALACCEPTENTRYUR + "?PK=" + pk + "&Status=1";
    }
    else if (UserStatus == 0) {
        var refID = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.RefID, $(tr).parent().attr("id"));
        if (refID == 0) {
            window.location = MaterialAcceptList.ADDNEWURL + "?PK=" + pk + "&Status=1";
        }
        else {
            window.location = MaterialAcceptList.ADDNEWURL + "?RefID=" + refID + "&Status=1";
        }
    }

    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(MaterialAcceptList.GRNDELETEURL + mahPK, function (data) {
        if (parseInt(data[0]) == 1)
            msgtxt = MaterialAcceptList.DELETESUCESS;
        else if (parseInt(data[0]) == 0)
            msgtxt = "Assigned";
        else if (parseInt(data[0]) == -12)
            msgtxt = MaterialAcceptList.UnableToCancelSTA;
        else
            msgtxt = MaterialAcceptList.ACTIONFAILEDMSG;
        GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.SAVECMD);

    });
    return false;
}

function CancelDetails() {
    var msgtxt;
    $.get(MaterialAcceptList.GRNDELETEURL + mahPK, function (data) {
        if (parseInt(data[0]) == 1)
            msgtxt = MaterialAcceptList.CANCELSUCESS;
        else if (parseInt(data[0]) == 0)
            msgtxt = "Assigned";
        else if (parseInt(data[0]) == -11)
            msgtxt = MaterialAcceptList.CANCELREFMSG + "<br/>" + data[1];
        else if (parseInt(data[0]) == -12)
            msgtxt = MaterialAcceptList.CANCELREFMSG + "<br/>" + data[1];
        else
            msgtxt = MaterialAcceptList.ACTIONFAILEDMSG;
        GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.SAVECMD);

    });
    return false;
}

function BindGrid() {
    ///<summary>Bind Designaion Details With Search value </summary>

    var ajaxUrl = MaterialAcceptList.GRNLISTURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=SBU]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&PageUrl=" + MaterialAcceptList.PAGEURL + "&FilterStatus=" + $("select[id$=ddltrxstatus]").val();
    $("#grdMAList").removeAttr("ajaxurl")
    $("#grdMAList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdMAList");
    GrandGrid.MakeGrid($("#grdMAList"));
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>

    BindGrid();
}

function ShowConversion() {
    $("#divConvert").dialog({ width: 1200, height: 400, buttons: {} });
    $("#divConvert").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
    return false;
}

function GetSTAForConvert() {
    $.get(MaterialAcceptList.GETSTAFORCONVERTURL + mahPK, function (data) {
        if (data != null && data.length > 0) {
            $("#divData").data("Received", data);
            GrandGrid.MakeGrid($("#grdReceived"), 0, new Array());
            GrandGrid.MakeGrid($("#grdReceived"), 1, data[0]);

            GrandGrid.MakeGrid($("#grdConvert"), 0, new Array());
            GrandGrid.MakeGrid($("#grdConvert"), 1, data[1]);
        }     
    });
}

function FillConvertDetails() {
    var Sbd_Pk = 0;
    MaterialAcceptList.MRHConversion = new Array();
    MaterialAcceptList.MRHConversionObject = new Object();

    //MaterialAcceptList.MRHConversionObject.USER_PK = $("[id$=hdfUserPk]").val();
    //MaterialAcceptList.MRHConversionObject.BIZUNIT = $("[id$=hdfBizUnit]").val(); 
    //MaterialAcceptList.MRHConversionObject.MAH_PK = mahPK; 
    $("[id$=MAH_PK]").val(mahPK);
    MaterialAcceptList.MRHConversionObject.ReceivedList = new Array();
    $("#grdReceived tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var ItemPk = GrandGrid.Utilities.GetColumnValue($(this), "ITM_PK", tableID);
        var UomPK = GrandGrid.Utilities.GetColumnValue($(this), "ITM_UOM", tableID);
        var StockPK = GrandGrid.Utilities.GetColumnValue($(this), "SBD_PK", tableID);
        var Quantity = GrandGrid.Utilities.GetColumnValue($(this), "MAD_QTY_ACCEPTED", tableID);
        Sbd_Pk = GrandGrid.Utilities.GetColumnValue($(this), "SBD_PK", tableID);

        var RcvdItem = new Object();
        RcvdItem.ITEM_PK = ItemPk;
        RcvdItem.UOM_PK = UomPK;
        RcvdItem.SBD_PK = StockPK;
        RcvdItem.QTY = Quantity;
        MaterialAcceptList.MRHConversionObject.ReceivedList.push(RcvdItem);

    });
    MaterialAcceptList.MRHConversionObject.SBD_PK = Sbd_Pk; 
    MaterialAcceptList.MRHConversionObject.ConvertList = new Array();
    $("#grdConvert tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var ItemPk = GrandGrid.Utilities.GetColumnValue($(this), "ITM_PK", tableID);
        var UomPK = GrandGrid.Utilities.GetColumnValue($(this), "ITM_UOM", tableID);
        var slno = GrandGrid.Utilities.GetColumnValue($(this), "MAD_SL_NO", tableID);
        var ConvertBal = $("#CONVERT_BALANCE_" + slno).val();
        var ConvItem = new Object();

        ConvItem.ITEM_PK = ItemPk;
        ConvItem.UOM_PK = UomPK;
        ConvItem.QTY = ConvertBal;
        MaterialAcceptList.MRHConversionObject.ConvertList.push(ConvItem);

    });

    MaterialAcceptList.MRHConversion.push(MaterialAcceptList.MRHConversionObject);
    $("#divData").data("MRHConversion", MaterialAcceptList.MRHConversion);
}

function SaveSTAConversion() {
    //  AddValidations(1);
  
    var Isvalid = 1;
    $("#grdConvert tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var slno = GrandGrid.Utilities.GetColumnValue($(this), "MAD_SL_NO", tableID);
        var ConvertBal = $("#CONVERT_BALANCE_" + slno).val();
        if (ConvertBal.match(/^\d+$/)) {
            if (parseFloat(ConvertBal) <= 0) {
                Isvalid = 0;
            }
        }
        else {
            Isvalid = 0;
        }
    });

    if (Isvalid == 1) {
        FillConvertDetails();
        MaterialAcceptList.MRHConversion = $("#divData").data("MRHConversion");
        $("[id$=ConvertDetails]").val(JSON.stringify(MaterialAcceptList.MRHConversion));
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(MaterialAcceptList.SAVESTACONVERSION, jSonString, function (data) {
            if (parseInt(data) > 0) {
                BindGrid();
                msgtxt = MaterialAcceptList.CONVERSIONSUCESS;
                GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.SAVECMD);
                $("#divConvert").dialog("close");
            }
            else {
                msgtxt = MaterialAcceptList.ACTIONFAILEDMSG;
                GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.SAVECMD);
            }
        });
    }
    else {
        msgtxt = MaterialAcceptList.INVALIDQTY;
        GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.SAVECMD);
        return false;
    }
    return false;
}

function DeleteSTAConversion() {
    $.get(MaterialAcceptList.DELETESTACONVERSION + mahPK, function (data) {
        if (parseInt(data) > 0) {
            msgtxt = MaterialAcceptList.CONVERSIONDELETE;
            GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.DELETECOMMAND);
            BindGrid();
            GetSTAForConvert();
            ShowConversion();
        }
        else {
            msgtxt = MaterialAcceptList.ACTIONFAILEDMSG;
            GrandScriptUtils.ShowModal(msgtxt, MaterialAcceptList.INFORMATIONTITLE, MaterialAcceptList.DELETECOMMAND);
        }
    });
    return false;
}

function AddValidations(mode) {
    RemoveAllValidations();
    if (mode == 1) {
        $("[id$=CONVERT_BALANCE_]").rules("add", {
            required: true,
            NonZero: true,
            messages: { required: "Translate(EnterAmount)" }
        });
    }
}

function RemoveAllValidations() {
    //<summary>function used Remove validation </summary>
    $(document.forms[0]).validate().resetForm();
    $('[id$=CONVERT_BALANCE_]').rules("remove");
}

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    $.get(MaterialAcceptList.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(MaterialAcceptList.SessionExpired, MaterialAcceptList.Confirmation, MaterialAcceptList.LOGOUT, true);
            result = false;
        }
        else {

            pk = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
            switch (command.toString()) {

                case MaterialAcceptList.DELETECOMMAND:
                    mahPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
                    GrandScriptUtils.ShowModal(MaterialAcceptList.DELETECONFIRMMSG, MaterialAcceptList.CONFIRMMSG, MaterialAcceptList.DELETE, true);

                    break;
                case MaterialAcceptList.EDITCOMMAND:
                    FillDetails(tr);
                    break;
                case MaterialAcceptList.VIEWCOMMAND:
                    ViewDetails(tr);
                    break;
                case MaterialAcceptList.PRINT:
                    //                    var prID = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
                    //                    window.location = MaterialAcceptList.PURCHASEREQUESTREPORTURL + "?PRID=" + prID;

                    //                    OpenPDF(RequisitionList.PRINTURL + "?RequisitionID=" + requisitionHeaderId);
                    //                    return false;

                    //                    break;
                    var prID = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
                    var url = MaterialAcceptList.REPORTURL + "?ID=" + prID + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
                    OpenPDF(url);
                    break;

                case MaterialAcceptList.CONVERT:
                    mahPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
                    GetSTAForConvert();
                    ShowConversion();
                    break;
                case MaterialAcceptList.NOTCONVERT:
                    mahPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
                    GetSTAForConvert();
                    ShowConversion();
                    break;

                case MaterialAcceptList.CANCEL:
                    mahPK = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.MAHPK, $(tr).parent().attr("id"));
                    GrandScriptUtils.ShowModal(MaterialAcceptList.CANCELCONFIRMMSG, MaterialAcceptList.CONFIRMMSG, MaterialAcceptList.CANCEL, true);
                    break;
                case MaterialAcceptList.MODIFY:
                    var refID = GrandGrid.Utilities.GetColumnValue(tr, MaterialAcceptList.RefID, $(tr).parent().attr("id"));
                    window.location = MaterialAcceptList.ADDNEWURL + "?RefID=" + refID + "&IsModify=1";
                    return false;
                    break;
            }
        }
    });
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case MaterialAcceptList.SAVECMD:
            PageInit();
            break;
        case MaterialAcceptList.DELETE:
            DeleteDetails();
            break;
        case MaterialAcceptList.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
        case MaterialAcceptList.CANCEL:
            CancelDetails();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    var isModify = $("[id$=hdnModify]").val();
    var isCancel = $("[id$=hdnCancel]").val();
    var UserStatus = 0;
    var ConversionReq = 0;
    var Converted = 0;
    $("#grdMAList tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        UserStatus = GrandGrid.Utilities.GetColumnValue($(this), MaterialAcceptList.UserStatus, $(this).parent().attr("id"));
        var MAH_STATUS = GrandGrid.Utilities.GetColumnValue($(this), MaterialAcceptList.AcceptStatus, tableID);
        ConversionReq = GrandGrid.Utilities.GetColumnValue($(this), "MAH_IS_CONVERSION_REQD", $(this).parent().attr("id"));
        Converted = GrandGrid.Utilities.GetColumnValue($(this), "MAH_IS_CONVERTED", $(this).parent().attr("id"));



        //  var ApproveStatus = GrandGrid.Utilities.GetColumnValue(this, vendorListing.VendorStats, tableID);
        //Action To perform for the logged in user
        if (ConversionReq == 1 && MAH_STATUS == 2) {
            if (Converted == 1) {
                $(this).find("td:last input[id$=imbConvert]").show();
                $(this).find("td:last input[id$=imbNotConvert]").hide();
                $("[id$=btnConvert]").hide();
            }
            else {
                $(this).find("td:last input[id$=imbNotConvert]").show();
                $(this).find("td:last input[id$=imbConvert]").hide();
                $("[id$=btnConvert]").show();
            }
        }
        else {
            $(this).find("td:last input[id$=imbConvert]").hide();
            $(this).find("td:last input[id$=imbNotConvert]").hide();
        }

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

        if (isModify == "1" && UserStatus == 0 && MAH_STATUS != 4 && MAH_STATUS != 5) {   //MAH_STATUS=> 4 (Cancelled),5(Closed)
            $(this).find("td:last input[id$=imbModify]").show();
        }
        else {
            $(this).find("td:last input[id$=imbModify]").hide();
        }
        if (isCancel == "1" && MAH_STATUS != 0 && MAH_STATUS != 4 && MAH_STATUS != 5) {
            $(this).find("td:last input[id$=imbCancel]").show();
        }
        else {
            $(this).find("td:last input[id$=imbCancel]").hide();
        }

        //Line Color
        var ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_LINE_COLOUR", $(this).parents("table:first").attr("id"));
        if (ColIndex != null) {

            var lineColor = GrandGrid.Utilities.GetColumnValue($(this), "CMP_LINE_COLOUR", $(this).parents("table:first").attr("id"));
            if (lineColor != "null") {
                ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CMP_DISPLAY_CODE", $(this).parents("table:first").attr("id"));
                if (ColIndex != null) {
                    $(this).find("td:eq(" + ColIndex + ")").addClass(lineColor);

                }
            }
        }

        var ItemIndex = 0;
        ItemIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MAH_ITEM_TEXT", $(this).parents("table:first").attr("id"));
        var ItemDetails = GrandGrid.Utilities.GetColumnValue($(this), "MAH_ITEM_TEXT", $(this).parents("table:first").attr("id"));
        if (ItemIndex != null) {
            if (ItemDetails.length > 100) {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails.substring(0, 100) + "...</div>");
            }
            else {
                var quotReplace = ItemDetails.replace(/"/g, '&quot;');
                $(this).find("td:eq(" + ItemIndex + ")").html("<div tooltip=\"" + quotReplace + "\">" + ItemDetails + "</div>");

            }
        }
    });

    $("#grdConvert tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var BalColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE", tableID);
        var Balance = GrandGrid.Utilities.GetColumnValue($(this), "BALANCE", tableID);
        var SlNo = GrandGrid.Utilities.GetColumnValue($(this), "MAD_SL_NO", tableID);

        if (BalColIndex != null) {
            $(this).find("td:eq(" + BalColIndex + ")").html("");
            Balance = parseFloat(Balance == null || Balance == "null" || Balance == "" ? 0 : Balance);
            $(this).find("td:eq(" + BalColIndex + ")").html("<input type=\"text\" id=\"CONVERT_BALANCE_" + SlNo + "\" value=\"" + Balance + "\" maxLength =\"30\"  class=\"numeric input-w150\" ></input>");
        }
    });
}
///#endregion