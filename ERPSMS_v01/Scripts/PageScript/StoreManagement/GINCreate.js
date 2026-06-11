/// <reference path="../../GrandScriptUtils.js" />
//#region----------- Global Variable----------------
var sl_NO = 0;
var ginPK = 0;

var DamageType = 0;
var grnPk = 0;
var itmPk = 0;
var pohDefaultCompany = 0;

var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "GIN";
//#endregion

//#region----------- Configuration Section----------------
var GINCreate = {
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    GRNBindGridURL: "GoodsIssueNote.do?Action=GetPendignGRN&grnPk=",
    FillStoreDropdownURL: "StoreRequisitionSlip.do?Action=GetStoresByType&SBUPk=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    SaveGINUrl: "GoodsInspectionNote.do?Action=SaveGoodsInspectionNote",
    GetPendingGRNItems: "GoodsReceiptNote.do?Action=GetGINDetailsView&GINPK=",
    BindPendingGrnItemUrl: "GoodsInspectionNote.do?Action=GetGRNPending",
    GINListUrl: "GINList.aspx",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    INBOX: "INBOX",
    BizUnitPk: 0,
    GINList: new Array(),
    GINObj: new Object(),
    IsRendered: false,
    IsModifyGIN: false,
    TextEmpty: "",
    ValueZero: "0",
    InformationTtile: "Translate(Information)",
    SelectGRNToAdd: "Translate(SelectGRNtoadd)",
    Delete: "delete",
    GINView: "ginview",
    GINViewDtls: "ginviewdtls",
    RejectDtls: "rejectdtls",
    Save: "save",
    POD_RATE: "POD_RATE",
    LOGOUT: "LOGOUT",

    SelectStore: "Translate(PleaseselectaStore)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    ConfirmMsg: "Translate(Conformation)",
    InspSaveMsg1: "Translate(GoodsInspectionNoteSaveMsg1)",
    InspSaveMsg2: "Translate(GINSaveSuccessmsg2)",
    SubmitMessage: "Translate(SubmittedMsg)",
    GINSavedMessage: "Translate(GoodsInspectionNoteSavedMessage)",
    DoEMsg: "Translate(DoEMsg)",

    ActionFailedmsg: "Translate(ActionFailedPleaseTryAgain)",
    EditMsg: "Translate(EditbyAnotherUser)",
    ItemAlreadyAddedMsg: "Translate(Itemsalreadyaddedbyanotheruser)",
    ItemAlreadyDeletedMsg: "Translate(Itemsalreadydeletedbyanotheruser)",
    AddDetailsMsg: "Translate(Addgoodsinspectiondetails)",
    SomeItemHaveZeroQty: "Translate(SomeItemsWithinspectedquantityiszero)",
    CannotModifyGIN: "Translate(CannotModifyGIN)",
    CannotRemoveGIN: "Translate(CannotRemoveGIN)",
    GRNDetails: "Translate(GRNDetails)",
    RejectedDetails: "Translate(RejectedDetails)",
    DamageDtlsErrorMsg: "Translate(DamageQtyExceedRejetcedQty)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    InspectionReqMsg: "Translate(Msg_IsspectionRequired)",
    MsgSameItemDiffRate: "Translate(MsgSameItemDiffRate)",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)"
}
//#endregion

var QtyDec, AmtDec, RateDec;

//#region----------- Initilization Section----------------
$(document).ready(function () {
    ///<summary>Document . Rerady ()</summary>
    //Set Decimal Points For Qty and Amount
    $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimalDigitP2P']").val();
    PageInit();
});


function PageInit() {
    ///<summary>initial page condition</summary>
    DateInit();
    Popup();
    FillDamageType(0);
    GINCreate.BizUnitPk = $("[id$=BizUnitPk]").val();
    if ($("[id$=GINList]").val() == "null") {
        RemoveValidations();
        GrandScriptUtils.ShowModal(GINCreate.ItemAlreadyDeletedMsg, GINCreate.InformationTtile, GINCreate.Save);
    }
    else {
        var GINObj = $.parseJSON($("[id$=GINList]").val());

        GINCreate.GINList = GINObj.GINList;
        $("#divData").data("GINData", GINObj);

        var queryStr = window.location.search.substring(1);
        if (queryStr != "" && queryStr != "TYPE=1" && queryStr != "TYPE=2") {
            var queryStr = queryStr.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] != "" && pK[0] == "Status") || (pK[1] != "" && pK[0] == "Flag")) {
                    //Check the query string Name status if status as 1 thn its in view mode
                    $("[id$=ViewStatus]").val('1');
                    $("[id$=AddToList]").css("display", "none");
                    //$("select[id$=SearchType]").attr("disabled", true);
                    $("#searchwrap").css("display", "none");
                }
                else if (pK[1] == 1 && pK[0] == "IsModify") {
                    $("[id$=GIH_IS_EDIT]").val("1");
                    $("[id$=AddToList]").show();
                    $("#searchwrap").show();
                }
            }
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                    FillDetails(GINObj);
                }
                //23-01-2014
                else if (pK[0] == "PRefID") {
                    if (parseInt($("[id$=hdfTransactionData]").val()) > 0) {
                        FillDetails(GINObj);
                    }
                    else {
                        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GIN", true);
                        GrandGrid.Utilities.ResetGrid(true, "grdGRNItemList");
                        GINCreate.GINList = new Array();
                        $("#divData").data("GINData", GINCreate.GINList);
                        GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINCreate.GINList);
                        FillStore($("[id$=hdfDeptID]").val());
                        //FillStore(0);
                        FillGRNStore($("[id$=hdfGrnStore]").val());
                        FillCompany(0);

                    }
                }
            }
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GIN", true);
            GrandGrid.Utilities.ResetGrid(true, "grdGRNItemList");
            GINCreate.GINList = new Array();
            $("#divData").data("GINData", GINCreate.GINList);
            GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINCreate.GINList);
            FillStore($("[id$=hdfDeptID]").val());
            FillCompany(0);
            FillGRNStore(0);
        }
        SetSearchType();
        FillGRNNOAutoComplete();

        $("[id$=GRH_DEPT_STORE]").focus();

        if (parseInt($("input[id$=GIH_STATUS]").val()) == 0 && parseInt($("input[id$=GIH_STATUS]").val()) == 6 && parseInt($("input[id$=GIH_PK]").val()) == 0) {
            $("[id$=ddlGihDept]").attr("disabled", false);
            $("[id$=GIH_COMPANY]").attr("disabled", false);
            $("[id$=GRH_DEPT_STORE]").attr("disabled", false);
            $("[id$=GIH_DATE]").attr("disabled", false);
            $("#divGINItems").css("display", "none");
        }
        else {
            if (parseInt($("input[id$=GIH_STATUS]").val()) == 0 || parseInt($("[id$=GIH_IS_EDIT]").val())) {
                $("[id$=ddlGihDept]").attr("disabled", false);
                $("[id$=GIH_COMPANY]").attr("disabled", false);
                $("[id$=GRH_DEPT_STORE]").attr("disabled", false);
                $("[id$=GIH_DATE]").attr("disabled", false);
            }
            else {
                $("[id$=ddlGihDept]").attr("disabled", true);
                $("[id$=GIH_COMPANY]").attr("disabled", true);
                $("[id$=GRH_DEPT_STORE]").attr("disabled", true);
                if (parseInt($("input[id$=GIH_STATUS]").val()) != 16 || ($("[id$=ViewStatus]").val() == "1")) {//Can Change the Date of Send Backed Record.0 -->SAVE,1--> Submitted,2---> Approved,16---Requested for more Info
                    $("[id$=GIH_DATE]").attr("disabled", true);
                }
            }
        }
        $("#divPendingGRNList").css("display", "none");
        FillScrapStore(0);
    }
    if (parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
        $("[id$=GIH_DATE]").attr("disabled", false);
    }
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
        $("[id$=GIH_COMPANY]").attr("disabled", "disabled");
    }
}

function DateInit() {
    ///<summary>initial Date condition</summary>
    GrandScriptUtils.DatePicker("GIH_DATE", false, false, false);
}
//#endregion

//#region----------- Core Section----------------
function HidePendingInspections() {
    //<summary>Function Used to Hide  Panel </summary>
    $("#imgPendingInspHide").hide();
    $("#imgPendingInspShow").show();
    $("#divPendingIspections").hide();
}

function ShowPendingInspections() {
    //<summary>Function Used to Show  Panel </summary>
    $("#imgPendingInspHide").show();
    $("#imgPendingInspShow").hide();
    $("#divPendingIspections").show();
}

function HideGINILst() {
    $("#imgGINIHide").hide();
    $("#imgGINIShow").show();
    $("#divGINItems").hide();
}

function ShowGINILst() {
    $("#imgGINIHide").show();
    $("#imgGINIShow").hide();
    $("#divGINItems").show();
}

function HideAttachments() {
    $("#imgAttachmentsHide").hide();
    $("#imgAttachmentsShow").show();
    $("#divAttachments").hide();
}

function ShowAttachments() {
    $("#imgAttachmentsHide").show();
    $("#imgAttachmentsShow").hide();
    $("#divAttachments").show();
}

function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=GIH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = GINCreate.FillCompanyDropdownURL + GINCreate.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val() + "&ApsPK=" + selectVal;
    }
    else {
        getURL = GINCreate.FillCompanyDropdownURL + GINCreate.BizUnitPk + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        if (selectVal == undefined || selectVal == 0) {
            var selCompany = $("[id$=hdfCompany]").val();
            $("#GIH_COMPANY").val(selCompany);
        }
    });
}


function FillDetails(GINObj) {
    ///<summary>Function to fill GIN Details</summary>
    //Set a stamp for cancelled record
    if (GINObj.GIH_STATUS == 4)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");
    //End
    $("[id$=GIH_STATUS]").val(GINObj.GIH_STATUS);
    if (parseInt($("[id$=GIH_STATUS]").val()) == 0 || parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_STATUS]").val()) == 16) {
        if ($("[id$=ViewStatus]").val() != "1") {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GIN", true);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GIN", false);
        }
    }
    else {
        if (parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GRN", true);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GIN", false);
        }
    }
    if (!($.isArray(GINCreate.GINList))) {

        if (GINCreate.GINList != undefined) {
            var objArray = GINCreate.GINList;
            GINCreate.GINList = new Array();
            GINCreate.GINList.push(objArray);
        }
        else {
            var objArray = GINCreate.GINList;
            GINCreate.GINList = new Array();
        }
    }
    if ($.isArray(GINCreate.GINList)) {
        GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINCreate.GINList);
        $("#divData").data("GINData", GINCreate.GINList);
    }
    $("[id$=GIH_PK]").val(GINObj.GIH_PK);
    if (GINObj.GIH_NO == null || GINObj.GIH_NO == "") {
        $("[id$=GIH_NO]").html(GINCreate.DocGenerationNewValue);
    }
    else {
        $("[id$=GIH_NO]").html(GINObj.GIH_NO);
    }

    $("[id$=GIH_DATE]").val(GINObj.GIH_DATE);
    $("[id$=LAST_MOD_DT]").val(GINObj.LAST_MOD_DT);
    if (parseInt(GINObj.GIH_STATUS) == 1 || parseInt($("[id$=GIH_STATUS]").val()) == 7 || parseInt($("[id$=GIH_STATUS]").val()) == 11 || parseInt($("[id$=GIH_STATUS]").val()) == 12) {
        $("[id$=AddToList]").css("display", "none");
    }
    FillCompany(GINObj.GIH_COMPANY);
    FillStore(GINObj.GIH_DEPT);
    if (GINObj.GDD_DEPT_STORE != undefined)
        FillGRNStore(GINObj.GDD_DEPT_STORE);
    else
        FillGRNStore(GINObj.GIH_GRN_DEPT);
    //#region --------------- Fill File Upload Details----------------------------------
    if (!($.isArray(GINObj.FILELIST))) {
        if (GINObj.FILELIST != undefined) {
            objArray = GINObj.FILELIST;
            FileJson.FILELIST = new Array();
            FileJson.FILELIST.push(objArray);
        }
        else {
            objArray = GINObj.FILELIST;
            FileJson.FILELIST = new Array();
        }
    }
    else {
        FileJson.FILELIST = GINObj.FILELIST;
    }
    FillFileDetails();
    //#Endregion
}

function FillFileDetails() {
    ///<summary>function used to fill the file details</summary>
    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + "." + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}

function BindGRNItemDetails(type) {
    ///<summary>Function To FillGRN Item Details Whene select Dept </summary>
    // Type 1 mean Refesh Details By Refersh Button , 0 - if we Change Department, need to clear selected item
    var GINJson = $("#divData").data("GINData");
    if (type == 0) {
        if (parseInt($("input[id$=GIH_STATUS]").val()) == 0) {
            //$("#divGINItems").css("display", "none");
            for (var i in GINJson) {
                GINJson.splice(i, 1);
            }
        }
    }
    $("#divData").data("GINData", GINJson);
    GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINJson);
    $("select[id$=SearchType]").val(0);
    $("#divDate").hide();
    $("input[id$=SearchValue]").val(GINCreate.TextEmpty);
    BindGrid();
}

function SetSearchType(isLoad) {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(GINCreate.TextEmpty);

    if (strname == "ITM_NAME") {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
        $("[id$=imgbtnclear]").show();
        if (isLoad) {
            BindGrid();
        }
    }
    else if (strname == "GRH_NAME") {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
        $("[id$=imgbtnclear]").show();
        if (isLoad) {
            BindGrid();
        }
    }
    else if (strname == "GRH_NO") {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
        $("[id$=imgbtnclear]").show();
        if (isLoad) {
            BindGrid();
        }
    }
}

function ClearGRNSearchDetails() {
    //<summary> Function Used to Clear GRN Item Details After Search </summary>
    BindGRNItemDetails(1);
    return false;
}

function FillGRNNOAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    var gihPkParms = "";
    if (parseInt($("[id$=GIH_STATUS]").val()) == 0 && parseInt($("[id$=GIH_STATUS]").val()) == 6) {
        gihPkParms = "&GINPk=" + $("[id$=GIH_STATUS]").val();
    }
    else {
        gihPkParms = "&GINPk=0";
    }
    var Type = $("[id$=hdfType]").val() == "1" ? "1" : "0";
    gihPkParms += "&IsStockItem=" + Type;
    GrandScriptUtils.MakeAutoComplete("SearchValue", "GoodsInspectionNote.do?Action=GetPendingSearchAuto&AUTOSEARCH=1" + gihPkParms, false, true, false, "SearchType", false, "GRH_DEPT_STORE", false, "ddlSelectDiv3");
}

function FillStore(selectVal) {
    ///<summary>to fill store combo</summary>
    var drpID = $("select[id$=ddlGihDept]").attr("id");
    $.get(GINCreate.FillStoreDropdownURL + GINCreate.BizUnitPk + "&Flag=1" + "&DeptType=4", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        $("[id$=ddlGihDept]").attr("disabled", true);
        $("[id$=GIH_DEPT]").val(selectVal);
    });
}

function FillGRNStore(selectVal) {
    ///<summary>to fill store combo</summary>
    var drpID = $("select[id$=GRH_DEPT_STORE]").attr("id");
    var grnStoreGetUrl = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current company pk
        grnStoreGetUrl = GINCreate.FillStoreDropdownURL + GINCreate.BizUnitPk + "&Flag=1" + "&DeptType=3" + "&DeptCompany=" + $("[id$=hdfCompany]").val();
    }
    else {
        grnStoreGetUrl = GINCreate.FillStoreDropdownURL + GINCreate.BizUnitPk + "&Flag=1" + "&DeptType=3";
    }
    $.get(grnStoreGetUrl, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        BindGrid();
    });
}

function FillScrapStore(selectVal) {
    ///<summary>to fill store combo</summary>
    var drpID = $("select[id$=GDD_DEPT_STORE]").attr("id");
    $.get(GINCreate.FillStoreDropdownURL + GINCreate.BizUnitPk + "&Flag=1" + "&DeptType=1", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
    });
}

function BindGrid() {
    ///<summary>To handle bind grid </summary>
    $("#divPendingGRNList").css("display", "none");
    var ginPkParams = GINCreate.TextEmpty;
    // 6 Submit more info - 0- Draft
    if ((parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_STATUS]").val()) == 0) && parseInt($("[id$=GIH_PK]").val()) > 0) {
        ginPkParams = "&GINPk=" + parseInt($("[id$=GIH_PK]").val());
    }
    else {
        ginPkParams = "&GINPk=0";
    }
    var Type = $("[id$=hdfType]").val() == "1" ? "1" : "0";
    var ajaxUrl = GINCreate.BindPendingGrnItemUrl + "&Store=" + $("[id$=GRH_DEPT_STORE]").val() + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + ginPkParams + "&GrhPK=" + $("[id$=GRH_PK]").val() + "&IsStockItem=" + Type;
    $("#grdGRNList").removeAttr("ajaxurl")
    $("#grdGRNList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdGRNList");
    GrandGrid.MakeGrid($("#grdGRNList"));
    //Set Default company while page load from inbox
    if ($("[id$=IsPrefID]").val() == "1") {
        $.get(ajaxUrl, function (data) {
            if (data != null && data != "") {
                pohDefaultCompany = data.Table1[0].GRH_COMPANY;
                $("[id$=hdfCompany]").val(data.Table1[0].GRH_COMPANY);
                $("[id$=GIH_DATE]").val(data.Table1[0].GRH_DATE);
                //Set DDL
                FillCompany(pohDefaultCompany);
            }
        });
    }
    return false;
}

//function AddButtonVisibility() { 
// $("#grdPendingPOList tr:has(td)").each(function (index) {
//            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.GRD_REMARKS, grdID);
//            remarks = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_REMARKS, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_REMARKS, grdID);
//            recvQtyColIndex = GrandGrid.Utilities.GetColumnIndex($(this), 'GRD_QTY_RECEIVED', grdID);
//            recvQty = GrandGrid.Utilities.GetColumnValue($(this), 'GRD_QTY_RECEIVED', grdID);
//            if (remkColIndex != null && (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6)) {
//                $(this).find("td:eq(" + remkColIndex + ")").html("");
//                $(this).find("td:eq(" + remkColIndex + ")").append("<input type=\"text\" id=\"txtRemarks_" + index + "\" style=\"width:100px\"  value=\"" + remarks + "\" width=\"85%\" > </input>");
//            }
//            else {
//                $(this).find("td:eq(" + remkColIndex + ")").html(remarks == null ? " " : remarks);
//            }
//        });
//}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    $("#divPendingGRNList").css("display", "none")
    if (targetControlID == "SearchValue") {
        BindGrid();
    }
}

function CheckItemExists(grnDtlPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    GINCreate.GINList = $("#divData").data("GINData");
    var flag = true;
    if (GINCreate.GINList.length > 0) {
        for (var i in GINCreate.GINList) {
            if ((GINCreate.GINList[i].GRD_PK == grnDtlPK)) {
                flag = false;
                break;
            }
        }
    }
    return flag;
}

/// <summary>
/// Check the selected PO's have same items with different rate
/// </summary>
function IsSameItemDiffRateExists(itemPK, poRate) {
    GINCreate.GINList = $("#divData").data("GINData");
    var flag = false;
    if (GINCreate.GINList.length > 0) {
        for (var i in GINCreate.GINList) {
            if ((GINCreate.GINList[i].GRD_ITEM == itemPK) && (GINCreate.GINList[i].POD_RATE != poRate)) {
                flag = true;
                break;
            }
        }
    }
    return flag;
}

function GetMonth1(strMonth) {
    var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    for (var i in mmm) {
        if (mmm[i] == strMonth)
            return i;
    }
    return 0;
}
function ItemRequireInspection() {
    var items = "";
    var inspectionRequired = 0;
    var qaLotNo = "";
    $("#grdGRNList tr:has(td)").each(function () {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            inspectionRequired = GrandGrid.Utilities.GetColumnValue($(this), "ITM_NEED_QC_INSP", grdID);
            qaLotNo = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QA_LOT_NO", grdID);
            if (inspectionRequired == "1" && qaLotNo == "") {
                items = items + GrandGrid.Utilities.GetColumnValue($(this), "ITM_NAME", grdID) + ",";
            }
        }
    });
    return items.slice(0, -1);
}

function AddToList() {
    ///<summary>Function used to add the needed po material list</summary>
    var grdID;
    GINCreate.GINList = $("#divData").data("GINData");
    if ($("#grdGRNList tr input[type=checkbox]:checked").length == 0) {
        GrandScriptUtils.ShowModal(GINCreate.SelectGRNToAdd, GINCreate.InformationTtile);
        return false;
    }
    var requiredInspectionList = ItemRequireInspection();
    if (requiredInspectionList != "") {
        requiredInspectionList = requiredInspectionList + " " + GINCreate.InspectionReqMsg;
        GrandScriptUtils.ShowModal(requiredInspectionList, GINCreate.InformationTtile);
        return false;
    }
    var doeFlag = 0;
    $("#grdGRNList tr:has(td)").each(function () {
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            grdID = $(this).parents("table:first").attr("id");
            GINCreate.GINObj = new Object();
            GINCreate.GINObj.GRD_PK = GrandGrid.Utilities.GetColumnValue($(this), "GRD_PK", grdID);
            GINCreate.GINObj.GRD_ITEM = GrandGrid.Utilities.GetColumnValue($(this), "GRD_ITEM", grdID);
            GINCreate.GINObj.POD_RATE = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), "POD_RATE", grdID)).toFixed(RateDec);

            var poNumber = "";
            var itemID = 0;
            var grdDoE = "";
            var curPoNumber = "";
            var curItemID = "";

            if (CheckItemExists(GINCreate.GINObj.GRD_PK)) {

                if (IsSameItemDiffRateExists(GINCreate.GINObj.GRD_ITEM, GINCreate.GINObj.POD_RATE)) {//Check the selected PO's have same items with different rate
                    GrandScriptUtils.ShowModal(GINCreate.MsgSameItemDiffRate, GINCreate.MessageBoxTitle);
                    return false;
                }

                //Check DoE is valid 
                if ($("[id$=IsReqDoeValidation]").val() == "1" && doeFlag == 0)
                    for (var i in GINCreate.GINList) {
                        poNumber = GINCreate.GINList[i].POH_NO;
                        itemID = GINCreate.GINList[i].GID_ITEM;
                        curPoNumber = GrandGrid.Utilities.GetColumnValue($(this), "POH_NO", grdID);
                        curItemID = GrandGrid.Utilities.GetColumnValue($(this), "GRD_ITEM", grdID);
                        if (poNumber == curPoNumber && itemID == curItemID) {
                            //Previous Date
                            grdDoE = GINCreate.GINList[i].GRD_DOE;
                            var dateSplit = grdDoE.split("-");
                            var month = GetMonth1(dateSplit[1]);
                            //                    var date1 = new Date("26-Oct-2013");
                            var strDate = month + "/" + dateSplit[0] + "/" + dateSplit[2];
                            var date1 = new Date(strDate);

                            //Current Date
                            grdDoE = GrandGrid.Utilities.GetColumnValue($(this), "GRD_DOE", grdID);
                            dateSplit = grdDoE.split("-");
                            month = GetMonth1(dateSplit[1]);
                            strDate = month + "/" + dateSplit[0] + "/" + dateSplit[2];
                            var date2 = new Date(strDate);
                            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                            diffDays = (diffDays < 0) ? diffDays * -1 : diffDays;
                            if (parseInt(diffDays) > parseInt($("[id$=DoeDifrDays]").val())) {
                                doeFlag = 1;
                                //                                GrandScriptUtils.ShowModal(GINCreate.DoEMsg, GINCreate.InformationTtile);
                            }
                        }
                    }

                GINCreate.GINObj.GRH_PK = GrandGrid.Utilities.GetColumnValue($(this), "GRH_PK", grdID);
                GINCreate.GINObj.GID_ITEM = GrandGrid.Utilities.GetColumnValue($(this), "GRD_ITEM", grdID);
                GINCreate.GINObj.GID_SL_NO = GINCreate.GINList.length + 1;
                GINCreate.GINObj.GRD_PK = GrandGrid.Utilities.GetColumnValue($(this), "GRD_PK", grdID);
                GINCreate.GINObj.GID_PK = $("input[id$=GID_PK]").val();
                GINCreate.GINObj.GID_GI = $("input[id$=GIH_PK]").val();
                GINCreate.GINObj.GID_NO = $("[id$=GIH_NO]").html();
                GINCreate.GINObj.GID_ITEM_NAME = GrandGrid.Utilities.GetColumnValue($(this), "ITM_NAME", grdID);
                GINCreate.GINObj.GRN_NO = GrandGrid.Utilities.GetColumnValue($(this), "GRH_NO", grdID);
                GINCreate.GINObj.GID_UOM = GrandGrid.Utilities.GetColumnValue($(this), "GRD_UOM", grdID);
                GINCreate.GINObj.UOM_CODE = GrandGrid.Utilities.GetColumnValue($(this), "UOM_CODE", grdID);
                GINCreate.GINObj.GRD_QTY_BALANCE = GrandGrid.Utilities.GetColumnValue($(this), "BALANCE_QTY", grdID).replace(/[^0-9\.]+/g, "");
                GINCreate.GINObj.POH_NO = GrandGrid.Utilities.GetColumnValue($(this), "POH_NO", grdID);
                GINCreate.GINObj.GRD_QTY_APPROVED = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", grdID).replace(/[^0-9\.]+/g, "");
                GINCreate.GINObj.GRD_DOE = GrandGrid.Utilities.GetColumnValue($(this), "GRD_DOE", grdID);
                GINCreate.GINObj.GRD_QTY_ACCEPTED = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_ACCEPTED", grdID).replace(/[^0-9\.]+/g, "");
                GINCreate.GINObj.GRD_QTY_INSPECTED = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_INSPECTED", grdID).replace(/[^0-9\.]+/g, "");
                GINCreate.GINObj.GRD_QTY_REJECTED = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_REJECTED", grdID).replace(/[^0-9\.]+/g, "");
                GINCreate.GINObj.GID_QA_LOT_NO = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QA_LOT_NO", grdID);
                //GINCreate.GINObj.GID_QTY_ACCEPTED = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_ACCEPTED", grdID);
                GINCreate.GINObj.GID_QTY_ACCEPTED = GINCreate.GINObj.GRD_QTY_BALANCE;
                GINCreate.GINObj.GID_QTY_INSPECTED = GINCreate.GINObj.GRD_QTY_BALANCE;
                GINCreate.GINObj.GID_QTY_REJECTED = 0;
                GINCreate.GINObj.GID_QTY_INSPECTED_LAST = 0;
                GINCreate.GINObj.GID_REMARKS = "";
                if ($("[id$=hdfCompany]").val() == $("[id$=hdfSBUCompany]").val()) {
                    $("[id$=hdfCompany]").val(GrandGrid.Utilities.GetColumnValue($(this), "GRH_COMPANY", grdID));
                }
                $("select[id$=GIH_COMPANY]").val($("[id$=hdfCompany]").val());

                if (!($.isArray(GINCreate.GINObj.DamageList))) {
                    if (GINCreate.GINObj.DamageList != undefined) {
                        objArray = GINCreate.GINObj.DamageList;
                        GINCreate.GINObj.DamageList = new Array();
                        GINCreate.GINObj.DamageList.push(objArray);
                    }
                    else {
                        objArray = GINCreate.GINObj.DamageList;
                        GINCreate.GINObj.DamageList = new Array();
                    }
                }
                GINCreate.GINList.push(GINCreate.GINObj);
            }

        }
    });
    $("#divData").data("GINData", GINCreate.GINList);
    GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINCreate.GINList);
    if (doeFlag == 1)
        GrandScriptUtils.ShowModal(GINCreate.DoEMsg, GINCreate.InformationTtile);
    return false;
}

function CheckHaveDamageDetails(grnPK, itmPk, grdPK) {
    ///<summary>Function Check have Damage Details </summary>
    GINCreate.GINList = $("#divData").data("GINData");
    for (var i in GINCreate.GINList) {
        if (GINCreate.GINList[i].GID_ITEM == itmPk && GINCreate.GINList[i].GRH_PK == grnPK && GINCreate.GINList[i].GRD_PK == grdPK) {
            DamageList = GINCreate.GINList[i].DamageList;
        }
    }
    if (!($.isArray(DamageList))) {
        var objArray;
        if (DamageList != undefined) {
            objArray = DamageList;
            DamageList = new Array();
            DamageList.push(objArray);
        }
        else {
            objArray = DamageList;
            DamageList = new Array();
        }
    }
    if (DamageList.length > 0) {
        return true;
    }
    else {
        return false;
    }
}

function AfterGridBind(grdID) {
    ///<summary>Function to Set Grid After Binding details</summary>
    if (grdID == "grdGRNItemList") {
        $("#divGINItems").css({ "display": "block", "visibility": "visible" });
        var remkColIndex = 0;
        var remarks = "";
        var txtBoxRemarks;
        var acptQtyColIndex = 0;
        var acptQty = "0";
        var txtBoxAcptQty;
        var ginColIndex = 0;
        var ginNo = "";
        var ginRejDtlIndex = 0;

        var ginPK = 0;
        var itmPK = 0;
        var grdPK = 0;
        var colIndex = 0;
        var grdDate = "";
        var acceptedQtyIndex = 0;
        var acceptNowQty = 0;
        var qaLotNoIndex = 0;
        var qaLotNo = "";

        $("#grdGRNItemList tr:has(td)").each(function (index) {
            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_REMARKS", grdID);
            remarks = GrandGrid.Utilities.GetColumnValue($(this), "GID_REMARKS", grdID) == "null" ? GINCreate.TextEmpty : GrandGrid.Utilities.GetColumnValue($(this), "GID_REMARKS", grdID);
            acptQtyColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_INSPECTED", grdID);
            acptQty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_INSPECTED", grdID); acceptNowQty
            ginColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRN_NO", grdID);
            ginNo = GrandGrid.Utilities.GetColumnValue($(this), "GRN_NO", grdID);
            rejQtyColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_REJECTED", grdID);
            rejQty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_REJECTED", grdID);
            ginPK = GrandGrid.Utilities.GetColumnValue($(this), "GRH_PK", grdID);
            itmPK = GrandGrid.Utilities.GetColumnValue($(this), "GID_ITEM", grdID);
            grdPK = GrandGrid.Utilities.GetColumnValue($(this), "GRD_PK", grdID);
            ginRejDtlIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_REJ_DTLS", grdID);
            acceptedQtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_ACCEPTED", grdID);
            acceptNowQty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_ACCEPTED", grdID);
            qaLotNoIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QA_LOT_NO", grdID);
            qaLotNo = GrandGrid.Utilities.GetColumnValue($(this), "GID_QA_LOT_NO", grdID);

            var damageTxt = "Add";
            var dtlpK = GrandGrid.Utilities.GetColumnValue($(this), "GID_PK", grdID);
            //            if (parseInt($("[id$=GIH_STATUS]").val()) != 1 && parseInt($("[id$=GIH_STATUS]").val()) != 7 && parseInt($("[id$=GIH_STATUS]").val()) != 11 && parseInt($("[id$=GIH_STATUS]").val()) != 12 && parseInt($("[id$=ViewStatus]").val()) == 0 && parseInt($("[id$=GIH_STATUS]").val()) != 3) {
            if (parseInt($("[id$=GIH_STATUS]").val()) == 0 || parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_STATUS]").val()) == 8 || parseInt($("[id$=GIH_STATUS]").val()) == 9 || parseInt($("[id$=GIH_STATUS]").val()) == 16 || parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
                if (remkColIndex != null) {
                    txtBoxRemarks = document.createElement("textarea");
                    txtBoxRemarks.id = "txtRemarks_" + index;
                    $(txtBoxRemarks).attr("cols", "10");
                    $(txtBoxRemarks).attr("rows", "2");
                    $(txtBoxRemarks).css({ "width": "70px" });
                    $(txtBoxRemarks).css({ "height": "20px" });
                    $(txtBoxRemarks).val(remarks);
                    $(this).find("td:eq(" + remkColIndex + ")").html("");
                    $(this).find("td:eq(" + remkColIndex + ")").append($(txtBoxRemarks));
                }
                if (qaLotNoIndex != null) {
                    if (qaLotNo == 'undefined' || qaLotNo == 'null') {
                        qaLotNo = "";
                    }
                    $(this).find("td:eq(" + qaLotNoIndex + ")").html("");
                    $(this).find("td:eq(" + qaLotNoIndex + ")").append("<input type=\"text\" style=\"width:50px\" id=\"txtLotNo_" + index + "\" value=\"" + qaLotNo + "\"  maxlength=\"15\"  tabindex=\"10\" />");

                }

                if (acptQtyColIndex != null) {
                    $(this).find("td:eq(" + acptQtyColIndex + ")").html("");
                    $(this).find("td:eq(" + acptQtyColIndex + ")").append("<input type=\"text\" style=\"width:50px\" id=\"txtAcceptQty_" + index + "\" onblur=\"javascript:return CalculateInpsQtyWithReject(" + index + "); \" value=\"" + parseFloat(acptQty).toFixed(QtyDec) + "\" class=\"numeric\" maxlength=\"12\" tabindex=\"10\"   onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(),this);\" onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"  />");
                    //                    $(this).find("td:eq(" + acptQtyColIndex + ")").append("<input type=\"text\" style=\"width:50px\" id=\"txtAcceptQty_" + index + "\" onblur=\"javascript:return CalculateInpsQtyWithReject(" + index + "); \" value=\"" + parseFloat(acptQty).toFixed(QtyDec) + "\" class=\"numeric\" maxlength=\"12\"    onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(),this);\" onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"  />");

                }
                if (acceptedQtyIndex != null) {
                    $(this).find("td:eq(" + acceptedQtyIndex + ")").html("");
                    //                    $(this).find("td:eq(" + acceptedQtyIndex + ")").append("<input type=\"text\" style=\"width:50px\" id=\"txtAcceptNow_" + index + "\" onblur=\"javascript:return CalculateInpsQty(" + index + "); \" value=\"" + parseFloat(acceptNowQty).toFixed(QtyDec) + "\" class=\"numeric\" maxlength=\"12\"    onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(),this);\" onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"  />");
                    $(this).find("td:eq(" + acceptedQtyIndex + ")").append("<input type=\"text\" style=\"width:50px\" id=\"txtAcceptNow_" + index + "\" onblur=\"javascript:return CalculateInpsQtyWithReject(" + index + "); \" value=\"" + parseFloat(acceptNowQty).toFixed(QtyDec) + "\" class=\"numeric input-disabled\" Readonly=\"true\" maxlength=\"12\"  tabindex=\"10\"  onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(),this);\" onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"  />");
                }

                if (rejQtyColIndex != null) {
                    $(this).find("td:eq(" + rejQtyColIndex + ")").html("");
                    $(this).find("td:eq(" + rejQtyColIndex + ")").append("<input type=\"text\" id=\"txtRejectQty_" + index + "\" onblur=\"javascript:return CalculateInpsQtyWithReject(" + index + "); \" value=\"" + parseFloat(rejQty).toFixed(QtyDec) + "\" class=\"numeric\" maxlength=\"12\" style=\"width:50px\"  tabindex=\"10\"  onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(),this);\"  onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"  />");
                }
                //
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_APPROVED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", grdID);
                if (colIndex != null) {
                    // if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_INSPECTED", grdID);
                if (colIndex != null) {
                    //  if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_ACCEPTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_ACCEPTED", grdID);
                if (colIndex != null) {
                    // if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_REJECTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_REJECTED", grdID);
                if (colIndex != null) {
                    //  if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_BALANCE", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_BALANCE", grdID);
                if (colIndex != null) {
                    //  if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
            }
            else {

                $(this).find("td:eq(" + remkColIndex + ")").html(remarks);
                //Number Format
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_APPROVED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_INSPECTED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_ACCEPTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_ACCEPTED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_REJECTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_REJECTED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_BALANCE", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_BALANCE", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_INSPECTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_INSPECTED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_ACCEPTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_ACCEPTED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_REJECTED", grdID);
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GID_QTY_REJECTED", grdID);
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
                }
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QA_LOT_NO", grdID);
                QaLotno = GrandGrid.Utilities.GetColumnValue($(this), "GID_QA_LOT_NO", grdID);
                if (colIndex != null) {
                    if (QaLotno == 'undefined' || QaLotno == 'null') {
                        QaLotno = "";
                    }
                    $(this).find("td:eq(" + colIndex + ")").html(QaLotno);
                }
                //End Format

            }

            if (ginColIndex != null) {
                $(this).find("td:eq(" + ginColIndex + ")").html("");
                $(this).find("td:eq(" + ginColIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'GINVIEWDTLS');\">" + ginNo + "</a>");
            }

            // To add Item rejected details
            if (ginRejDtlIndex != null) {
                $(this).find("td:eq(" + ginRejDtlIndex + ")").html("");
                if (parseInt($("[id$=GIH_STATUS]").val()) == 0 || parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
                    if (rejQty == 0) {
                        damageTxt = "None";
                        $(this).find("td:eq(" + ginRejDtlIndex + ")").html(damageTxt);
                    }
                    else {
                        if ($("[id$=ViewStatus]").val() == "1" && parseInt($("[id$=GIH_IS_EDIT]").val()) != 1) {
                            if (CheckHaveDamageDetails(ginPK, itmPK, grdPK)) {
                                damageTxt = "View";
                                $(this).find("td:eq(" + ginRejDtlIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'REJECTDTLS');\">" + damageTxt + "</a>");
                            }
                            else {
                                damageTxt = "None";
                                $(this).find("td:eq(" + ginRejDtlIndex + ")").html(damageTxt);
                            }
                        }
                        else {
                            damageTxt = "Add";
                            $(this).find("td:eq(" + ginRejDtlIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'REJECTDTLS');\">" + damageTxt + "</a>");
                        }
                    }
                }
                else {
                    if (rejQty == 0) {
                        damageTxt = "None";
                        $(this).find("td:eq(" + ginRejDtlIndex + ")").html(damageTxt);
                    }
                    else {
                        damageTxt = "View";
                        $(this).find("td:eq(" + ginRejDtlIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'REJECTDTLS');\">" + damageTxt + "</a>");
                    }
                }
            }
        });

        if ((parseInt($("[id$=GIH_STATUS]").val()) != 0 && parseInt($("[id$=GIH_STATUS]").val()) != 6) || $("[id$=ViewStatus]").val() == "1") {
            if (parseInt($("[id$=GIH_IS_EDIT]").val()) != 1) {
                //Hiding the template field (Action section)
                $("#grdGRNItemList").find("tr").each(function () {
                    $(this).find("td:last,th:last").hide();
                });
            }
        }
    }

    if (grdID == "grdGRNList") {
        $("#divPendingGRNList").css({ "display": "block", "visibility": "visible" });
        var ginColIndex = 0;
        var ginNo = GINCreate.TextEmpty;

        var qtyInspColIndx = 0;
        var qtyInsp = GINCreate.TextEmpty;

        var qty = 0;
        var strVal = "";

        $("#grdGRNList tr:has(td)").each(function (index) {
            ginColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRH_NO", grdID);
            ginNo = GrandGrid.Utilities.GetColumnValue($(this), "GRH_NO", grdID);

            qtyInspColIndx = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED", grdID);
            qtyInsp = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_INSPECTED", grdID);

            if (ginColIndex != null) {
                $(this).find("td:eq(" + ginColIndex + ")").html("");
                $(this).find("td:eq(" + ginColIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'GINVIEW');\">" + ginNo + "</a>");
            }

            if (qtyInspColIndx != null && qtyInsp > 0) {
                $(this).find("td:eq(" + qtyInspColIndx + ")").html("");
                $(this).find("td:eq(" + qtyInspColIndx + ")").html("<a style=\"cursor:pointer;  float: right; text-align : right !important\"  onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'GRNDTLVIEW');\">" + qtyInsp + "</a>");
            }
            //Date for DOM
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_DOM", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                grdDate = GrandGrid.Utilities.GetColumnValue($(this), "GRD_DOM", $(this).parents("table:first").attr("id"));
                if (grdDate == "undefined" || grdDate == "null") grdDate = "";
                $(this).find("td:eq(" + colIndex + ")").html(grdDate);
            }
            //Date for DOE
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_DOE", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                grdDate = GrandGrid.Utilities.GetColumnValue($(this), "GRD_DOE", $(this).parents("table:first").attr("id"));
                if (grdDate == "undefined" || grdDate == "null") grdDate = "";
                $(this).find("td:eq(" + colIndex + ")").html(grdDate);
            }
            //Status
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_GIN_FLAG", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                status = GrandGrid.Utilities.GetColumnValue($(this), "GRD_GIN_FLAG", $(this).parents("table:first").attr("id"));
                if (status == 1)
                    $(this).find("td:eq(" + colIndex + ")").html("<img  src=\"../Images/Classic/Icons/arrived.png\"  alt=\"Translate(Added)\" title=\"Translate(Added)\" />");
                else
                    $(this).find("td:eq(" + colIndex + ")").html("");
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_APPROVED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_INSPECTED", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_ACCEPTED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_ACCEPTED", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_REJECTED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_REJECTED", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE_QTY", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "BALANCE_QTY", $(this).parents("table:first").attr("id"));

                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QA_LOT_NO", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                strVal = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QA_LOT_NO", $(this).parents("table:first").attr("id"));
                if (strVal == "null") {
                    $(this).find("td:eq(" + colIndex + ")").html("");
                }
            }
        });

        if (GINCreate.IsRendered) {
            if (parseInt($("[id$=GIH_STATUS]").val()) == 6) {
                ReloadGoodsInspecionMaterialDetails();
            }
        }

        GINCreate.IsRendered = true;
        // Code for hide checkbox 
        if ($("[id$=ViewStatus]").val() == "1" || parseInt($("[id$=GIH_STATUS]").val()) == 1 || parseInt($("[id$=GIH_STATUS]").val()) == 2 || parseInt($("[id$=GIH_STATUS]").val()) == 3 || parseInt($("[id$=GIH_STATUS]").val()) == 7 || parseInt($("[id$=GIH_STATUS]").val()) == 11 || parseInt($("[id$=GIH_STATUS]").val()) == 12) {
            if (parseInt($("[id$=GIH_IS_EDIT]").val()) != 1) {
                //Hiding the template field
                $("#grdGRNList").find("tr").each(function () {
                    $(this).find("td:first,th:first").hide();
                });
            }
        }
    }

    if (grdID == "grdGRNDetails") {
        var colIndex = 0;
        var qty = 0;
        $("#grdGRNDetails tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_APPROVED", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec));
            }
        });
    }



    if (grdID == $("#grdDamageDtls").attr("id")) {
        //Mode iS view
        if ($("[id$=ViewStatus]").val() == "1" || parseInt($("[id$=GIH_STATUS]").val()) == 1 || parseInt($("[id$=GIH_STATUS]").val()) == 2 || parseInt($("[id$=GIH_STATUS]").val()) == 3 || parseInt($("[id$=GIH_STATUS]").val()) == 7 || parseInt($("[id$=GIH_STATUS]").val()) == 11 || parseInt($("[id$=GIH_STATUS]").val()) == 12) {
            if (parseInt($("[id$=GIH_IS_EDIT]").val()) != 1) {
                //Hiding the template field
                $("#grdDamageDtls").find("tr").each(function () {
                    $(this).find("td:last,th:last").hide();
                });
            }
        }


        var qty = 0;
        var strVal = "";

        $("#grdDamageDtls tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GDD_DMG_QTY", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "GDD_DMG_QTY", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }
        });
    }
}

function CalculateRejectQtyWithDamageDtlQty(indx, rejQty) {
    ///<summary>Function to Check total Damage Qty enter in popup <= Rejected Qry</summary>
    var dmgQty = 0;
    if (GINCreate.GINList[indx].DamageListDamageList != null) {
        if (GINCreate.GINList[indx].DamageList.length > 0) {
            for (var i in GINCreate.GINList[indx].DamageList) {
                dmgQty += parseInt(GINCreate.GINList[indx].DamageList[i].GDD_DMG_QTY);
            }
        }
    }
    if (rejQty == NaN || rejQty == undefined || rejQty == "undefined" || isNaN(rejQty)) {
        rejQty == 0;
    }
    if (rejQty >= dmgQty) {
        return true;
    }
    else {
        return false;
    }
}

function GetTotalDamageQty(DamageList) {
    ///<summary>Function to Get Damage details</summary>
    var dmgQty = 0;
    if (DamageList != null) {
        if (!($.isArray(DamageList))) {
            if (DamageList != undefined) {
                var objArray = DamageList;
                DamageList = new Array();
                DamageList.push(objArray);
            }
            else {
                var objArray = DamageList;
                DamageList = new Array();
            }
        }
        if (DamageList.length >= 0) {
            for (var i in DamageList) {
                dmgQty += parseFloat(DamageList[i].GDD_DMG_QTY);
            }
        }
    }
    return dmgQty;
}

function ClearDamageDetailsList(index) {
    ///<summary>Function to CLear Damage Details</summary> 
    var rejQty = 0;
    var dmgQty = 0;
    GINCreate.GINList = $("#divData").data("GINData");
    var damageList;
    for (var i in GINCreate.GINList) {
        if (parseInt(i) == index) {
            rejQty = parseFloat(GINCreate.GINList[i].GID_QTY_REJECTED);
            damageList = GINCreate.GINList[i].DamageList;
            dmgQty = GetTotalDamageQty(damageList);
            if (dmgQty > rejQty) {
                GINCreate.GINList[i].DamageList = null;
            }
        }
    }

    $("#divData").data("GINData", GINCreate.GINList);
}

function MakeNumeric(event, AllowDot, value, control) {
    ///<summary>Function to Make Numeric Action</summary> 
    var Qty = 0;
    var qtyArray;
    var keyCode = event.keyCode ? event.keyCode : event.which;
    if (keyCode != 39 && keyCode != 37 && keyCode != 8 && keyCode != 9 && keyCode != 46) {
        var regx = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
        if (!(regx.test(value))) {
            qtyArray = String(value).split('.');
            if (qtyArray.length > 1) {
                if (qtyArray[1] != "" && qtyArray[1] != "0") {
                    if (qtyArray[0].length <= 8 && qtyArray[1].length <= parseInt(QtyDec)) {
                    }
                    else {
                        if (qtyArray[0].length > 8) {
                            $("input[id$=" + control.id + "]").val(qtyArray[0].slice(0, qtyArray[0].length - 1) + "." + qtyArray[1]);
                        }
                        else {
                            $("input[id$=" + control.id + "]").val(qtyArray[0] + "." + qtyArray[1].slice(0, qtyArray[1].length - 1));
                        }
                    }
                }
                else {
                    if (qtyArray[0] == "" && qtyArray[1] == "") {
                        $("input[id$=" + control.id + "]").val(".");
                    }
                    else if (qtyArray[0] == "" && qtyArray[1] != "") {
                        $("input[id$=" + control.id + "]").val("." + value);
                    }
                    else {
                        $("input[id$=" + control.id + "]").val(value);
                    }
                }
            }
            else {
                if (qtyArray[0].length > 8) {
                    $("input[id$=" + control.id + "]").val(qtyArray[0].slice(0, qtyArray[0].length - 1));
                }
                else {
                    $("input[id$=" + control.id + "]").val(value);
                }
            }
        }
    }
    else {

    }

}

function CheckValidNumber(indx) {
    ///<summary>Function to Check  Valid number or not</summary> 
    return false;
}

function CalculateInpsQtyWithReject(indx) {
    ///<summary>Function to Calculate Rejected Quantity </summary>
    var aprvQty = 0;
    var rejQty = 0;
    var rmks = "";
    var grnDtlpk = 0;
    var orginalInspQty = 0;
    var qty;
    GINCreate.GINList = $("#divData").data("GINData");
    if (GINCreate.GINList.length > 0) {
        for (var i in GINCreate.GINList) {
            if ((GINCreate.GINList[i].GID_SL_NO == indx + 1)) {
                aprvQty = parseFloat(GetInspectedQty(indx));
                qty = GetRejectedQty(indx);
                if (qty == "") {
                    rejQty = 0;
                }
                else {
                    rejQty = parseFloat(qty);
                }
                if (CalculateRejectQtyWithDamageDtlQty(i, rejQty)) {
                    grnDtlpk = GINCreate.GINList[i].GRD_PK;
                    if (aprvQty >= rejQty) {
                        rmks = GetRemarks(indx);
                        if (parseFloat(GINCreate.GINList[i].GRD_QTY_BALANCE) >= aprvQty) {
                            if (GINCreate.GINList[i].GID_PK > 0) {
                                orginalInspQty = GINCreate.GINList[i].GID_QTY_INSPECTED; //  GINCreate.GINList[i].GID_QTY_INSPECTED_LAST;
                            }
                            else {

                                orginalInspQty = GINCreate.GINList[i].GID_QTY_INSPECTED;
                            }
                            GINCreate.GINList[i].GID_QTY_INSPECTED = aprvQty;
                            GINCreate.GINList[i].GID_QTY_REJECTED = rejQty;
                            GINCreate.GINList[i].GID_REMARKS = rmks;
                            GINCreate.GINList[i].GID_QTY_ACCEPTED = aprvQty - rejQty;
                            if (parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
                                AdjustItemValueAfterSearch(grnDtlpk, GINCreate.GINList[i].GRD_QTY_BALANCE, GINCreate.GINList[i].GID_QTY_INSPECTED, 2);
                            }
                        }
                        else {
                            GINCreate.GINList[i].GID_REMARKS = rmks;
                        }
                    }
                }
                else {
                    GrandScriptUtils.ShowModal(GINCreate.DamageDtlsErrorMsg, GINCreate.InformationTtile);
                }
            }
        }
    }
    $("#divData").data("GINData", GINCreate.GINList);
    GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINCreate.GINList);
    ClearDamageDetailsList(indx);
    return false;
}

function CalculateInpsQty(indx) {
    ///<summary>Function to Calculate Rejected Quantity </summary>
    var aprvQty = 0;
    var rejQty = 0;
    var rmks = "";
    var grnDtlpk = 0;
    var orginalInspQty = 0;
    GINCreate.GINList = $("#divData").data("GINData");
    if (GINCreate.GINList.length > 0) {
        for (var i in GINCreate.GINList) {
            if ((GINCreate.GINList[i].GID_SL_NO == indx + 1)) {
                aprvQty = parseFloat(GetInspectedQty(indx));
                rejQty = parseFloat(GetRejectedQty(indx));
                if (CalculateRejectQtyWithDamageDtlQty(i, rejQty)) {
                    grnDtlpk = GINCreate.GINList[i].GRD_PK;
                    if (aprvQty >= rejQty) {
                        rmks = GetRemarks(indx);
                        if (parseFloat(GINCreate.GINList[i].GRD_QTY_BALANCE) >= aprvQty) {
                            if (GINCreate.GINList[i].GID_PK > 0) {
                                orginalInspQty = GINCreate.GINList[i].GID_QTY_INSPECTED; //  GINCreate.GINList[i].GID_QTY_INSPECTED_LAST;
                            }
                            else {
                                orginalInspQty = GINCreate.GINList[i].GID_QTY_INSPECTED;
                            }
                            GINCreate.GINList[i].GID_QTY_INSPECTED = aprvQty;
                            GINCreate.GINList[i].GID_QTY_REJECTED = rejQty;
                            GINCreate.GINList[i].GID_REMARKS = rmks;
                            if (parseInt($("[id$=GIH_STATUS]").val()) == 6) {
                                AdjustItemValueAfterSearch(grnDtlpk, GINCreate.GINList[i].GRD_QTY_BALANCE, GINCreate.GINList[i].GID_QTY_INSPECTED, 2);
                            }
                        }
                        else {
                            GINCreate.GINList[i].GID_REMARKS = rmks;
                        }
                    }
                }
                else {
                    GrandScriptUtils.ShowModal(GINCreate.DamageDtlsErrorMsg, GINCreate.InformationTtile);
                }
            }
        }
    }
    $("#divData").data("GINData", GINCreate.GINList);
    GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINCreate.GINList);
    return false;
}

function AdjustItemValueAfterSearch(grdPK, balQty, inspQty, type) {
    ///<summary>Function to Adjust item value after Search</summary> 
    var colInspIndex = 0;
    var colInsp = 0;
    var colBalIndex = 0;
    var colBal = 0;
    var inspVal = 0;
    $("#grdGRNList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        grdetailPk = GrandGrid.Utilities.GetColumnValue($(this), "GRD_PK", grdID);
        if (grdetailPk == grdPK) {
            // Deleted
            if (type == 1) {
                colBalIndex = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE_QTY", grdID);
                $(this).find("td:eq(" + colBalIndex + ")").html(balQty);
                aprvValue = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", grdID);
                colInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED", grdID);
                $(this).find("td:eq(" + colInspIndex + ") ").text(Round(parseFloat(aprvValue) - parseFloat(balQty), 3));
                originalInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED_LAST", grdID);
                $(this).find("td:eq(" + originalInspIndex + ")").html(Round(parseFloat(aprvValue) - parseFloat(balQty), 3));
            }
            // Already in List and Adjustment
            if (type == 2) {
                colBalIndex = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE_QTY", grdID);
                var balance = Round(parseFloat(balQty) - parseFloat(inspQty), 3);
                $(this).find("td:eq(" + colBalIndex + ")").html(balance);
                aprvValue = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_APPROVED", grdID);
                colInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED", grdID);
                $(this).find("td:eq(" + colInspIndex + ") ").text(Round(parseFloat(aprvValue) - parseFloat(balance), 3));
                originalInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_INSPECTED_LAST", grdID);
                $(this).find("td:eq(" + originalInspIndex + ")").html(Round(parseFloat(aprvValue) - parseFloat(balance), 3));
            }
        }
    });
}

function Round(x, y) {
    //<summary>Function used to Calculate Round Value</summary>
    return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);
}

function GetInspectedQty(indx) {
    ///<summary>Function to Get inspected quantity from textbox</summary>
    var grdID;
    aprvdIndex = 0;
    var aprvQty = 0;
    $("#grdGRNItemList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            aprvdIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_INSPECTED", grdID);
            aprvQty = $(this).find("td:eq(" + aprvdIndex + ") input[type=text]").val();
            if (aprvQty == null || aprvQty == undefined || aprvQty == GINCreate.TextEmpty) {
                aprvQty = 0;
            }
        }
    });
    return aprvQty;
}

function GetRejectedQty(indx) {
    ///<summary>Function to Get Rejected quantity from textbox</summary>
    var grdID;
    aprvdIndex = 0;
    var aprvQty = 0;
    $("#grdGRNItemList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            rejIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QTY_REJECTED", grdID);
            rejQty = $(this).find("td:eq(" + rejIndex + ") input[type=text]").val();
            if (rejQty == null || rejQty == undefined || rejQty == GINCreate.TextEmpty) {
                aprvQty = 0;
            }
        }
    });
    return rejQty;
}

function GetRemarks(indx) {
    ///<summary>Function to Get Remarks from textbox</summary>
    var grdID;
    var rmksIndex = 0;
    var rmks = GINCreate.TextEmpty;
    $("#grdGRNItemList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            rmksIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_REMARKS", grdID);
            rmks = $(this).find("td:eq(" + rmksIndex + ") textarea").val();
            if (rejQty == null || rejQty == undefined) {
                rmks = GINCreate.TextEmpty;
            }
        }
    });
    return rmks;
}

function GridHandler(tr, command) {
    ///<summary>Function to do Action in Grid Controls</summary>
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case GINCreate.Delete:
            sl_NO = GrandGrid.Utilities.GetColumnValue(tr, "GID_SL_NO", "grdGRNItemList");
            GrandScriptUtils.ShowModal(GINCreate.DeleteConfirmMsg, GINCreate.ConfirmMsg, GINCreate.Delete, true)
            $("[id$=hdfCompany]").val($("[id$=hdfSBUCompany]").val());
            $("select[id$=GIH_COMPANY]").val($("[id$=hdfCompany]").val());
            return false;
            break;
        case GINCreate.GINView:
            ginPK = GrandGrid.Utilities.GetColumnValue(tr, "GRH_PK", "grdGRNList");
            ViewGRNDtls(ginPK);
            return false;
            break;
        case GINCreate.GINViewDtls:
            ginPK = GrandGrid.Utilities.GetColumnValue(tr, "GRH_PK", "grdGRNItemList");
            ViewGRNDtls(ginPK);
            return false;
            break;
        case GINCreate.RejectDtls:
            ginPK = GrandGrid.Utilities.GetColumnValue(tr, "GRH_PK", "grdGRNItemList");
            itmPK = GrandGrid.Utilities.GetColumnValue(tr, "GID_ITEM", "grdGRNItemList");
            itmName = GrandGrid.Utilities.GetColumnValue(tr, "GID_ITEM_NAME", "grdGRNItemList");
            rejIndx = GrandGrid.Utilities.GetColumnIndex(tr, "GID_QTY_REJECTED", "grdGRNItemList");
            grdPK = GrandGrid.Utilities.GetColumnValue(tr, "GRD_PK", "grdGRNItemList");
            if (parseInt($("[id$=GIH_STATUS]").val()) == 0 || parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_STATUS]").val()) == 2 || parseInt($("[id$=GIH_STATUS]").val()) == 16 || parseInt($("[id$=GIH_STATUS]").val()) == 9 || parseInt($("[id$=GIH_STATUS]").val()) == 8) {  //if ($("[id$=ViewStatus]").val() != "1"
                if (parseInt($("[id$=ViewStatus]").val()) == 0 || parseInt($("[id$=GIH_STATUS]").val()) == 16 || parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
                    rejQty = $(tr).find("td:eq(" + rejIndx + ") input").val();
                }
                else {
                    rejQty = GrandGrid.Utilities.GetColumnValue(tr, "GID_QTY_REJECTED", "grdGRNItemList");
                }
            }
            else {
                rejQty = GrandGrid.Utilities.GetColumnValue(tr, "GID_QTY_REJECTED", "grdGRNItemList");
            }

            AddRejectedDtls(ginPK, itmPK, itmName, rejQty, grdPK);
            return false;
            break;
        case "grndtlview":
            grnDtlsPK = GrandGrid.Utilities.GetColumnValue(tr, "GRD_PK", "grdGRNList");
            ViewGINInspectedDtls(grnDtlsPK);
            break;
        default:
            GrandScriptUtils.ShowModal(GINCreate.DefaultAction, GINCreate.InformationTtile);
            return false;
            break;
    }
}

function ModalOk(command) {
    ///<summary>Function to do action after click modal popup ok button</summary>
    switch (command) {
        case GINCreate.Delete:
            DeleteGINItemDetails();
            break;
        case GINCreate.Save:
            if ($("[id$=hdfType]").val() == "2")
                window.location = GINCreate.GINListUrl + "?TYPE=2";
            else
                window.location = GINCreate.GINListUrl;
            break;
        case GINCreate.INBOX:
            window.location = GINCreate.InboxURL;
            break;
        case "DELETEDAMAGEDTL":
            DeleteDamageItemDetails();
            break;
        case GINCreate.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
    return false;
}

function ViewGRNDtls(ginPK) {
    ///<summary>Function to View GRN Details In PopUp</summary>
    BindGRNGrid(ginPK);
    $("#divGRNDetails").dialog({ width: 850, height: 450, resizable: false, modal: true, title: GINCreate.GRNDetails });
    $("#divGRNDetails").dialog("open");
}

function BindGRNGrid(ginPK) {
    ///<summary>To handle bind po grid </summary>
    var ajaxUrl = GINCreate.GetPendingGRNItems + ginPK;
    $("#grdGRNDetails").removeAttr("ajaxurl")
    $("#grdGRNDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdGRNDetails");
    GrandGrid.MakeGrid($("#grdGRNDetails"));
}

function ViewGINInspectedDtls(grnDtlPK) {
    ///<summary>Function to View Already Inspected Details  In PopUp</summary>
    BindGINInspectedGrid(grnDtlPK);
    $("#divGINInspectedDetails").dialog({ width: 850, height: 450, resizable: false, modal: true, title: "Translate(InspectionDetails)" });
    $("#divGINInspectedDetails").dialog("open");
}

function BindGINInspectedGrid(grnDtlPK) {
    ///<summary>Get And Fill Already Inspected Details </summary>
    var ajaxUrl = "GoodsInspectionNote.do?Action=GetGINAlreadyInspectionDtls&GRDPK=" + grnDtlPK + "&SBU=" + $("[id$=BizUnitPk]").val() + "&Status=0";
    $("#grdGINInspDtls").removeAttr("ajaxurl")
    $("#grdGINInspDtls").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdGINInspDtls");
    GrandGrid.MakeGrid($("#grdGINInspDtls"));
}

function Popup() {
    ///<summary>Function to PopUp Window</summary>
    $("#divGRNDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#divGINInspectedDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#DivRejectedDtls").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveRejectedDtlsValidations();
        }
    });
}

function DeleteGINItemDetails() {
    ///<summary>Function to Delete GIN Details</summary>
    $("#divGINItems").css("display", "none");
    var GINJson = $("#divData").data("GINData");
    for (var i in GINJson) {
        if (GINJson[i].GID_SL_NO == sl_NO) {
            if (parseInt($("[id$=GIH_STATUS]").val()) == 6 || parseInt($("[id$=GIH_IS_EDIT]").val()) == 1) {
                AdjustItemValueAfterSearch(GINJson[i].GRD_PK, GINJson[i].GRD_QTY_BALANCE, 0, 1);
            }
            GINJson.splice(i, 1);
            break;
        }
    }
    for (var i in GINJson) {
        GINJson[i].GID_SL_NO = parseInt(i) + 1;
    }

    $("#divData").data("GINData", GINJson);
    GrandGrid.MakeGrid($("#grdGRNItemList"), 0, GINJson);
    return false;
}

function CheckInspectedQuantityGreaterThanZero() {
    ///<summary>Function to check any inspected quantity is zero in a list before save</summary>
    var flag = true;
    GINCreate.GINList = $("#divData").data("GINData");
    for (var i in GINCreate.GINList) {
        if (parseFloat(GINCreate.GINList[i].GID_QTY_INSPECTED) <= 0) {
            flag = false;
            break;
        }
    }
    return flag;
}

function CheckDamageDetailsAllocated() {
    ///<summary>Function to check Damage Details Allocated</summary>
    var status = true;
    GINCreate.GINList = $("#divData").data("GINData");
    for (var i in GINCreate.GINList) {
        var rejQty = parseFloat(GINCreate.GINList[i].GID_QTY_REJECTED).toFixed(QtyDec);
        if (rejQty != 0) {
            if (GINCreate.GINList[i].DamageList != null) {
                if (!($.isArray(GINCreate.GINList[i].DamageList))) {
                    if (GINCreate.GINList[i].DamageList != undefined) {
                        var objArray = GINCreate.GINList[i].DamageList;
                        GINCreate.GINList[i].DamageList = new Array();
                        GINCreate.GINList[i].DamageList.push(objArray);
                    }
                    else {
                        var objArray = GINCreate.GINList[i].DamageList;
                        GINCreate.GINList[i].DamageList = new Array();
                    }
                }

                if (GINCreate.GINList[i].DamageList.length > 0) {
                    var dmgQty = 0;
                    for (var j in GINCreate.GINList[i].DamageList) {
                        dmgQty = parseFloat(parseFloat(dmgQty) + parseFloat(GINCreate.GINList[i].DamageList[j].GDD_DMG_QTY)).toFixed(QtyDec);
                    }
                    if (rejQty != dmgQty) {
                        status = false;
                    }
                }
                else {
                    status = false;
                }
            }
            else {
                status = false;
            }
        }
        else {
            status = true;
        }
    }
    return status;
}

function SavePage(command) {
    ///<summary>Function to Save GIN Details</summary>

    $.get(GINCreate.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GINCreate.SessionExpired, GINCreate.Confirmation, GINCreate.LOGOUT, true);
            result = false;
        }
        else {

            $("[id$=GIH_COMPANY]").attr("disabled", false);
            AddValidations();
            GINCreate.GINList = new Array();
            GINCreate.GINList = GetGINItemDetailsToSave();
            $("[id$=GINList]").val(JSON.stringify(GINCreate.GINList));
            // For File Upload---------------------------------------------------------------------
            var ObjFile = $("#divFileData").data("FileData");
            $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
            // For File Upload---------------------------------------------------------------------
            $("[id$=WKF_FLAG]").val("0");
            if (command != "Draft") {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
                if ($("[id$=ReferenceID]").val() == "0") {
                    $("[id$=WKF_FLAG]").val("1");
                }
            }
            else
                $("[id$=ActionID]").val(GINCreate.ValueZero);  // save only.

            if ($(document.forms[0]).valid()) {
                //Showing validation for Future Date selection
                if ($("[id$=hdfIsContFutureDate]").val() != "1") {
                    var RetVal = CompareDate($("[id$=GIH_DATE]").val(), $("[id$=hdfCurrentDate]").val());
                    if (RetVal == 1) {
                        ShowFutureDate(command);
                        return false;
                    }
                }

                if (CheckInspectedQuantityGreaterThanZero()) {
                    if (GINCreate.GINList.length > 0) {
                        if (parseInt($("input[id$=GIH_STATUS]").val()) != 0) {
                            $("[id$=ddlGihDept]").attr("disabled", false);
                            $("[id$=GRH_DEPT_STORE]").attr("disabled", false);
                            $("[id$=GIH_COMPANY]").attr("disabled", false);
                            $("[id$=GIH_DATE]").attr("disabled", false);
                        }
                        if (CheckDamageDetailsAllocated()) {
                            var jSonString = GrandScriptUtils.FormToJsonString(false);
                            var SaveMessageWithGIN = "";

                            //To Prevent Muliple Click
                            if ($("[id$=SubmitFlag]").val() == "0")
                                $("[id$=SubmitFlag]").val('1')
                            else
                                return false;
                            $.post(GINCreate.SaveGINUrl, jSonString, function (data) {
                                if (parseInt(data[0]) > 0) {
                                    if (command == "Draft") {
                                        SaveMessageWithGIN = GINCreate.GINSavedMessage;
                                        if ($("[id$=AST_DOC_MODE]").val() == "1")
                                            SaveMessageWithGIN = GINCreate.InspSaveMsg1 + " " + data[1] + " " + GINCreate.InspSaveMsg2;

                                        GrandScriptUtils.ShowModal(SaveMessageWithGIN, GINCreate.InformationTtile, GINCreate.Save);
                                    }
                                    // If action - WorkFlow Save
                                    else {
                                        $("[id$=hdfAppID]").val(data[0]);
                                        $("[id$=AppNo]").val(data[1]);
                                        // $("[id$=btnSubmitDelegate]").click();
                                        SaveWorkFlow();
                                    }
                                }
                                else if (parseInt(data[0]) == -1) {
                                    GrandScriptUtils.ShowModal(GINCreate.ActionFailedmsg, GINCreate.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -2) {
                                    GrandScriptUtils.ShowModal(GINCreate.InspSaveMsg1 + " " + data[1] + " " + GINCreate.EditMsg, GINCreate.InformationTtile, GINCreate.Save);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -3) {
                                    GrandScriptUtils.ShowModal(GINCreate.ItemAlreadyAddedMsg, GINCreate.InformationTtile, GINCreate.Save);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -35) {//cannot modify if GIN already transferred or stock admitted
                                    GrandScriptUtils.ShowModal(GINCreate.CannotModifyGIN, GINCreate.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else if (parseInt(data[0]) == -36) {//Removed record was already transferred or stock admitted
                                    GrandScriptUtils.ShowModal(GINCreate.CannotRemoveGIN, GINCreate.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                                else {
                                    GrandScriptUtils.ShowModal(GINCreate.ActionFailedmsg, GINCreate.InformationTtile);
                                    $("[id$=SubmitFlag]").val('0')
                                }
                            });
                        }
                        else {
                            GrandScriptUtils.ShowModal("Translate(DamageQtyEqualToRejectedQty)", GINCreate.InformationTtile);
                            $("[id$=SubmitFlag]").val('0')
                        }
                    }
                    else {
                        GrandScriptUtils.ShowModal(GINCreate.AddDetailsMsg, GINCreate.InformationTtile);
                        $("[id$=SubmitFlag]").val('0')
                    }
                }
                else {
                    GrandScriptUtils.ShowModal(GINCreate.SomeItemHaveZeroQty, GINCreate.InformationTtile);
                }
            }
        }
    });
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    //var msg = GINCreate.InspSaveMsg1 + " " + $("[id$=AppNo]").val() + " " + GINCreate.SubmitMessage;
    //GrandScriptUtils.ShowModal(msg, GINCreate.InformationTtile, GINCreate.Save);
    var msg = "";
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        msg = GINCreate.InspSaveMsg1 + " " + $("[id$=AppNo]").val() + " " + GINCreate.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, GINCreate.InformationTtile, GINCreate.INBOX);
    } else {
        msg = GINCreate.InspSaveMsg1 + " " + $("[id$=AppNo]").val() + " " + GINCreate.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, GINCreate.InformationTtile, GINCreate.Save);
    }
}

function GetRemarksFormGrid(indx) {
    ///<summary>Function to Get Remarks from textbox</summary>
    var grdID;
    rmrkIndex = 0;
    var remark = GINCreate.TextEmpty;
    $("#grdGRNItemList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            rmrkIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_REMARKS", grdID);
            remark = $(this).find("td:eq(" + rmrkIndex + ") textarea").val();
            if (remark == null || remark == undefined || remark == GINCreate.TextEmpty) {
                remark = GINCreate.TextEmpty;
            }
        }
    });
    return remark;
}

function GetGINItemDetailsToSave() {
    ///<summary>Function to Get GIn Details TO Save</summary>
    GINCreate.GINList = $("#divData").data("GINData");
    for (var i in GINCreate.GINList) {
        if (parseInt($("[id$=GIH_STATUS]").val()) != 1 && parseInt($("[id$=GIH_STATUS]").val()) != 7 && parseInt($("[id$=GIH_STATUS]").val()) != 11 && parseInt($("[id$=GIH_STATUS]").val()) != 12) {
            var remarks = GetRemarksFormGrid(i);
            GINCreate.GINList[i].GID_REMARKS = remarks;
            GINCreate.GINList[i].GID_QA_LOT_NO = $("#txtLotNo_" + i).val();
        }
    }
    return GINCreate.GINList;
}

//function GetQALotNoFromgrid(indx) {
//    ///<summary>Function to Get QA Lot Number from textbox</summary>
//    var grdID;
//    var lotNoIndex = 0;
//    var qaLotNo = GINCreate.TextEmpty;
//    $("#grdGRNItemList tr:has(td)").each(function (index) {
//        grdID = $(this).parents("table:first").attr("id");
//        if (indx == index) {
//            lotNoIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GID_QA_LOT_NO", grdID);
//            remark = $(this).find("td:eq(" + rmrkIndex + ") textarea").val();
//            if (remark == null || remark == undefined || remark == GINCreate.TextEmpty) {
//                remark = GINCreate.TextEmpty;
//            }
//        }
//    });
//    return remark;
//}

function ReloadGoodsInspecionMaterialDetails() {
    ///<summary>To handle bind po grid </summary>
    $.getJSON("GoodsInspectionNote.do?Action=GetGINDetails&GIHPK=" + $("[id$=GIH_PK]").val(), function (data) {
        if (data) {
            FillGoodsInpectionDetails(data);
        }
    });

}

function GetInspQty(grdPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var inspQty = 0;
    for (var i in GINCreate.GINList) {
        if (GINCreate.GINList[i].GRD_PK == grdPK) {
            inspQty = GINCreate.GINList[i].GID_QTY_INSPECTED;
        }
    }
    return inspQty;
}

function GetLastInspectedQty(grdPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var inspQty = 0;
    for (var i in GINCreate.GINList) {
        if (GINCreate.GINList[i].GRD_PK == grdPK) {
            inspQty = GINCreate.GINList[i].GID_QTY_INSPECTED_LAST;
        }
    }
    return inspQty;
}

function GetLastInspectedQtyFromOrgData(grdPK, GINObj) {
    ///<summary>Function to Get Last inspected quantity from list</summary>
    var inspQty = 0;
    for (var i in GINObj.GINList) {
        if ((GINObj.GINList[i].GRD_PK == grdPK)) {
            inspQty = GINObj.GINList[i].GID_QTY_INSPECTED_LAST;
        }
    }
    return inspQty;
}

function FillGoodsInpectionDetails(GINObj) {
    ///<summary>To handle bind po grid </summary>
    if (!$.isArray(GINObj.GINList)) {
        var objGIN = GINObj.GINList;
        GINObj.GINList = new Array();
        GINObj.GINList.push(objGIN);
    }
    GINCreate.GINList = $("#divData").data("GINData");
    var grdID = "";
    var pendPO = 0;
    var pendItem = 0;
    var inspQty = 0;
    var currQty = 0;
    var lastInspQty = 0;
    var balQty = 0;
    $("#grdGRNList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        grdpk = GrandGrid.Utilities.GetColumnValue($(this), "GRD_PK", grdID);
        if (CheckGRNItemExists(grdpk)) {
            inspQty = GetInspQty(grdpk);
            balQty = GetBalanceQty(grdpk, GINObj);
            AdjustItemValueAfterSearch(grdpk, balQty, inspQty, 2);
        }
        else {
            if (CheckOrgItemExists(grdpk, GINObj)) {
                balQty = GetBalanceQty(grdpk, GINObj);
                AdjustItemValueAfterSearch(grdpk, balQty, 0, 1);
            }
        }
    });
}

function GetBalanceQty(grdPK, GINObj) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var balncQty = 0;
    for (var i in GINObj.GINList) {
        if (GINObj.GINList[i].GRD_PK == grdPK) {
            balncQty = GINObj.GINList[i].GRD_QTY_BALANCE;
        }
    }
    return balncQty;
}

function CheckGRNItemExists(itemPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var flag = false;
    GINCreate.GINList = $("#divData").data("GINData");
    for (var i in GINCreate.GINList) {
        if ((GINCreate.GINList[i].GRD_PK == itemPK)) {
            flag = true;
            break;
        }
    }
    return flag;
}

function CheckOrgItemExists(itemPK, GINObj) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var flag = false;
    for (var i in GINObj.GINList) {
        if ((GINObj.GINList[i].GRD_PK == itemPK)) {
            flag = true;
            break;
        }
    }
    return flag;
}

function CancelPage() {
    //<summary>Function used to redirect to listing page  </summary>
    $.get(GINCreate.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GINCreate.SessionExpired, GINCreate.Confirmation, GINCreate.LOGOUT, true);
            result = false;
        }
        else {

            RemoveValidations();
            if ($("[id$=hdfType]").val() == "2")
                window.location = GINCreate.GINListUrl + "?TYPE=2";
            else
                window.location = GINCreate.GINListUrl;
            return false;

        }
    });
    return false;
}
//#region Damage Store

function AddRejectedDtls(grnPK, itmPk, itmName, rejQty, grdPK) {
    ///<summary>Function to View Rejected Details In PopUp</summary>
    //BindGRNGrid(ginPK);
    if (rejQty > 0) {
        if (parseInt($("[id$=GIH_IS_EDIT]").val()) != 1) {
            if ($("[id$=ViewStatus]").val() == "1" || (parseInt($("[id$=GIH_STATUS]").val()) != 0 && parseInt($("[id$=GIH_STATUS]").val()) != 6 && parseInt($("[id$=GIH_STATUS]").val()) != 16 && parseInt($("[id$=GIH_STATUS]").val()) != 9 && parseInt($("[id$=GIH_STATUS]").val()) != 8)) {
                $("#addDamageDetails").hide();
                $("[id$=btnContainer]").hide();
            }
        }
        $("input[id$=GRNPK]").val(grnPK);
        $("input[id$=ItemPk]").val(itmPk);
        $("[id$=ItemName]").text(itmName);
        $("[id$=QtyRejected]").text(rejQty);
        $("input[id$=RejQty]").val(rejQty);
        $("input[id$=GDD_GRN_DTL]").val(grdPK);

        GINCreate.GINList = $("#divData").data("GINData");
        for (var i in GINCreate.GINList) {
            if (GINCreate.GINList[i].GID_ITEM == itmPk && GINCreate.GINList[i].GRH_PK == grnPK && GINCreate.GINList[i].GRD_PK == grdPK) {
                DamageList = GINCreate.GINList[i].DamageList;
            }
        }

        if (!($.isArray(DamageList))) {
            var objArray;
            if (DamageList != undefined) {
                objArray = DamageList;
                DamageList = new Array();
                DamageList.push(objArray);
            }
            else {
                objArray = DamageList;
                DamageList = new Array();
            }
        }
        if (DamageList.length > 0) {
            $("#DivRejectedDtls").dialog({ width: 500, height: 450, resizable: false, modal: true, title: GINCreate.RejectedDetails });
            $("#DivRejectedDtls").dialog("open");
            ClearDamageDetails();
            GrandGrid.MakeGrid($("#grdDamageDtls"), 0, DamageList);
        }
        else {
            if (parseInt($("[id$=GIH_IS_EDIT]").val()) != 1) {
                if ($("[id$=ViewStatus]").val() == "1" || (parseInt($("[id$=GIH_STATUS]").val()) != 0 && parseInt($("[id$=GIH_STATUS]").val()) != 6 && parseInt($("[id$=GIH_STATUS]").val()) != 16 && parseInt($("[id$=GIH_STATUS]").val()) != 8)) {
                }
                else {
                    $("#DivRejectedDtls").dialog({ width: 500, height: 450, resizable: false, modal: true, title: GINCreate.RejectedDetails });
                    $("#DivRejectedDtls").dialog("open");
                    ClearDamageDetails();
                }
            }
            else {
                $("#DivRejectedDtls").dialog({ width: 500, height: 450, resizable: false, modal: true, title: GINCreate.RejectedDetails });
                $("#DivRejectedDtls").dialog("open");
                ClearDamageDetails();
            }

            GrandGrid.MakeGrid($("#grdDamageDtls"), 0, new Object());
        }
    }
}

function FillDamageType(selectVal) {
    ///<summary>Function to Fill Damage Type</summary>
    var drpID = $("select[id$=GDD_DMG_TYPE]").attr("id");
    $.get("StoreAuditManagement.do?Action=GetDamageTypes&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        if (selectVal)
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, false);
    });
}

function AddDamageDetails() {
    ///<summary>Function to Add  Damage Details</summary>
    var itemPKD = $("input[id$=ItemPk]").val();
    var grnPKD = $("input[id$=GRNPK]").val();
    var itemNameD = $("[id$=ItemName]").text();
    var totalRejQtyD = $("input[id$=RejQty]").val();
    var damageType = $("[id$=GDD_DMG_TYPE]").val();
    var grdDtlPK = $("input[id$=GDD_GRN_DTL]").val();
    var indx = 0;
    AddRejectedDtlsValidations();
    if ($(document.forms[0]).valid()) {
        var DamageList = new Object();
        var DummyDamageList = new Object();

        GINCreate.GINList = $("#divData").data("GINData");
        for (var i in GINCreate.GINList) {
            if (GINCreate.GINList[i].GID_ITEM == itemPKD && GINCreate.GINList[i].GRH_PK == grnPKD && GINCreate.GINList[i].GRD_PK == grdDtlPK) {
                DamageList = GINCreate.GINList[i].DamageList;
            }
        }
        if (!($.isArray(DamageList))) {
            var objArray;
            if (DamageList != undefined) {
                objArray = DamageList;
                DamageList = new Array();
                DamageList.push(objArray);
            }
            else {
                objArray = DamageList;
                DamageList = new Array();
            }
        }
        //DummyDamageList = DamageList;
        var damageType = $("[id$=GDD_DMG_TYPE]").val();
        var damageStore = $("[id$=GDD_DEPT_STORE]").val();
        var damageDetails = new Object();
        var flag = true;
        var count = 0;
        var isSplice = false;
        //if (DamageType == "0") {
        for (var i in DamageList) {
            if (DamageList[i].GDD_DMG_TYPE == $("[id$=EditReasonType]").val() && DamageList[i].GDD_DEPT_STORE == $("[id$=EditStore]").val()) {
                indx = i;
                isSplice = true;
                DummyDamageList = DamageList[i];
                DamageList.splice(i, 1);
            }
        }
        for (var i in DamageList) {
            if (DamageList[i].GDD_DMG_TYPE == damageType && DamageList[i].GDD_DEPT_STORE == damageStore) {
                flag = false;
                break;
            }
        }
        if (flag) {
            if (checkRejectedQty(DamageList, totalRejQtyD)) {
                damageDetails.GDD_GRN = grnPKD;
                damageDetails.GDD_ITEM = itemPKD;
                damageDetails.GDD_ITEM_NAME = itemNameD;
                damageDetails.GDD_DMG_TYPE = $("[id$=GDD_DMG_TYPE]").val();
                damageDetails.GDD_DMG_TYPE_TEXT = $("[id$=GDD_DMG_TYPE] :selected").text();
                damageDetails.GDD_DMG_QTY = $("[id$=GDD_DMG_QTY]").val();
                damageDetails.GDD_GRN_DTL = grdDtlPK;
                damageDetails.GDD_DEPT_STORE = $("[id$=GDD_DEPT_STORE]").val(); ;
                damageDetails.GDD_DEPT_STORE_NAME = $("[id$=GDD_DEPT_STORE] :selected").text();
                DamageList.push(damageDetails);
                for (var i in GINCreate.GINList) {
                    if (GINCreate.GINList[i].GID_ITEM == itemPKD && GINCreate.GINList[i].GRH_PK == grnPKD && GINCreate.GINList[i].GRD_PK == grdDtlPK) {
                        GINCreate.GINList[i].DamageList = DamageList;
                    }
                }
                if (DamageList.length > 0) {
                    GrandGrid.MakeGrid($("#grdDamageDtls"), 0, DamageList);
                }
                else {
                    GrandGrid.Utilities.ResetGrid(true, "grdDamageDtls");
                }
                ClearDamageDetails();
            }
            else {
                GrandScriptUtils.ShowModal("Translate(RejectedQtyExceeded)", GINCreate.InformationTtile, "EXIST", false);
            }
        }
        else {
            if (!($.isArray(DamageList))) {
                var objArray;
                if (DamageList != undefined) {
                    objArray = DamageList;
                    DamageList = new Array();
                    DamageList.push(objArray);
                }
                else {
                    objArray = DamageList;
                    DamageList = new Array();
                }
            }
            if (isSplice) {
                DamageList.push(DummyDamageList);
                for (var i in GINCreate.GINList) {
                    if (GINCreate.GINList[i].GID_ITEM == itemPKD && GINCreate.GINList[i].GRH_PK == grnPKD && GINCreate.GINList[i].GRD_PK == grdDtlPK) {
                        GINCreate.GINList[i].DamageList = DamageList;
                    }
                }
                if (DamageList.length > 0) {
                    GrandGrid.MakeGrid($("#grdDamageDtls"), 0, DamageList);
                }
            }
            GrandScriptUtils.ShowModal("Translate(DamageTypeAdded)", GINCreate.InformationTtile, "EXIST", false);
        }
    }
    return false;
}

function checkRejectedQty(DamageList, totalRejQtyD) {
    ///<summary>Function to Check  Reject Quantity Details</summary>
    var totQty = 0;
    var qty = parseFloat($("[id$=GDD_DMG_QTY]").val());
    for (var i in DamageList) {
        totQty += parseFloat(DamageList[i].GDD_DMG_QTY);
    }
    if (parseInt($("[id$=EditMode]").val()) == 0) {
        //$("[id$=EditQty]").val(0);
        if ((qty + totQty) <= totalRejQtyD) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        var editRejQty = parseFloat($("[id$=EditQty]").val());
        if ((qty + totQty) <= totalRejQtyD) {
            return true;
        }
        else {
            return false;
        }
    }
}

function ClearDamageDetails() {
    ///<summary>Function to Clear  Damage Details</summary>
    DamageType = 0;
    $("select[id$=GDD_DMG_TYPE]").val("0");
    $("input[id$=GDD_DMG_QTY]").val("");
    $("select[id$=GDD_DEPT_STORE]").val("0");
    grnPk = 0;
    itmPk = 0;
    RemoveRejectedDtlsValidations();
    $("[id$=EditMode]").val(0);
    $("[id$=EditQty]").val(0);
    $("[id$=EditReasonType]").val("0");
    $("[id$=EditStore]").val("0");
    return false;
}

function DeleteDamageItemDetails() {
    ///<summary>Function to Delete  Damage Details</summary>
    var DamageList = new Object();
    var indx = 0;
    GINCreate.GINList = $("#divData").data("GINData");
    var grdDtlPK = $("input[id$=GDD_GRN_DTL]").val();
    for (var i in GINCreate.GINList) {
        if (GINCreate.GINList[i].GID_ITEM == itmPk && GINCreate.GINList[i].GRD_PK == grdDtlPK) {
            indx = i;
            DamageList = GINCreate.GINList[i].DamageList;
        }
    }

    if (!($.isArray(DamageList))) {
        var objArray;
        if (DamageList != undefined) {
            objArray = DamageList;
            DamageList = new Array();
            DamageList.push(objArray);
        }
        else {
            objArray = DamageList;
            DamageList = new Array();
        }
    }
    //AuditJson.ItemList

    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in DamageList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (DamageList[i].GDD_DMG_TYPE == DamageType) {
            // Splice Details From List, Corresponding ToUnit
            DamageList.splice(i, 1);
            GINCreate.GINList[indx].DamageList = DamageList;
            break;
        }
    }
    $("#divData").data("GINData", GINCreate.GINList);

    if (DamageList.length > 0) {
        GrandGrid.MakeGrid($("#grdDamageDtls"), 0, DamageList);
    }

    else {
        GrandGrid.MakeGrid($("#grdDamageDtls"), 0, new Object());
    }

    ClearDamageDetails();
}

function DamageGridHandler(tr, command) {
    ///<summary>Function to Handle Damage Actions</summary>
    DamageType = GrandGrid.Utilities.GetColumnValue(tr, "GDD_DMG_TYPE", "grdDamageDtls");
    switch (command) {
        case "EDIT":
            $("select[id$=GDD_DMG_TYPE]").val(GrandGrid.Utilities.GetColumnValue(tr, "GDD_DMG_TYPE", "grdDamageDtls"));
            $("input[id$=GDD_DMG_QTY]").val(GrandGrid.Utilities.GetColumnValue(tr, "GDD_DMG_QTY", "grdDamageDtls").replace(/[^0-9\.]+/g, ""));
            $("select[id$=GDD_DEPT_STORE]").val(GrandGrid.Utilities.GetColumnValue(tr, "GDD_DEPT_STORE", "grdDamageDtls"));
            $("[id$=EditMode]").val(1);
            $("[id$=EditQty]").val(GrandGrid.Utilities.GetColumnValue(tr, "GDD_DMG_QTY", "grdDamageDtls"));
            $("[id$=EditReasonType]").val(GrandGrid.Utilities.GetColumnValue(tr, "GDD_DMG_TYPE", "grdDamageDtls"));
            $("[id$=EditStore]").val(GrandGrid.Utilities.GetColumnValue(tr, "GDD_DEPT_STORE", "grdDamageDtls"));
            break;
        case "DELETE":
            // Do Confirmation.. Before Delete Details
            itmPk = GrandGrid.Utilities.GetColumnValue(tr, "GDD_ITEM", $(tr).parent().parent().attr("id"));
            grnPk = GrandGrid.Utilities.GetColumnValue(tr, "GDD_GRN", $(tr).parent().parent().attr("id"));
            DamageType = GrandGrid.Utilities.GetColumnValue(tr, "GDD_DMG_TYPE", $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(GINCreate.DeleteConfirmMsg, GINCreate.ConfirmMsg, "DELETEDAMAGEDTL", true);
            return false;
            break;
    }
    return false;
}
//#endregion
//#endregion

//#region----------- Validation Section----------------
function AddValidations() {
    //<summary>Function used to assign validation</summary>
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>
    $(document.forms[0]).validate().resetForm();
}

function RemoveAllValidations() {
    //<summary>Function Remove Validation</summary>
    RemoveValidations();
}

function AddRejectedDtlsValidations() {
    //<summary>Function used to assign validation</summary>
    $("input[id$=GDD_DMG_QTY]").rules("add", {
        required: true,
        maxlength: 12,
        DecimalDigits: QtyDec,
        CustomDecimal: true,
        messages: { required: "Translate(EnterRejectedQty)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
    });
}

function RemoveRejectedDtlsValidations() {
    //<summary>Function Remove Validation</summary>
    $(document.forms[0]).validate().resetForm();
    $("input[id$=GDD_DMG_QTY]").rules("remove");
}
//#endregion
//Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

//For checking selected date is a future date or not
//command=>Draft,SaveandSubmit
function ShowFutureDate(command) {

    var msgTitle;
    var msg;
    msgTitle = GINCreate.MessageBoxTitle;
    msg = GINCreate.ContFutureDateMsg;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfIsContFutureDate]").val(1);
                $(this).dialog("close");
                if (command == "Draft") {
                    $("[id$=btnSave]").click();
                }
                else {
                    $("[id$=btnSaveandSubmit]").click();
                }
            },
            Cancel: function (e) {
                $("[id$=hdfIsContFutureDate]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}