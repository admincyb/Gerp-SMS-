
///#region ---------- Global Variable 
var compundPK = 0;
///#endregion

///#region ---------- Configuration
var CompoundMaster = {
    CreateCompound: "CompoundingMaster.aspx",
    VALUEZERO: "0",
    TEXTEMPTY: "",
    COMPOUNDPK: "COM_PK",
    DOUWANTTODELETE: "Translate(Doyouwanttodeletethisdetails)",
    COMPOUNDGRIDURL: "CompoundMaster.do?Action=GetCompoundList&Status=",
    COMPOUNDDTLSAUTOCOMPLETEURL: "CompoundMaster.do?Action=GetSearchValue&SBU=",
    CONFIRMATIONMSG: "Translate(Conformation)",
    INFORMATIONTITLE: "Translate(Information)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECOMPOUNDSUCCESSMSG: "Translate(CompoundDetailsDeletedSuccessfully)",
    ACTIVATECOMPOUNDSUCCESSMSG: "Translate(CompoundDetailsActivatedSuccessfully)",
    INACTIVATECOMPOUNDSUCCESSMSG: "Translate(CompoundDetailsInActivatedSuccessfully)",
    DELETEMACHINEDTLSURL: "CompoundMaster.do?Action=DeleteCompoundDtls&CompID=",
    ACTIVATEINACTIVATECOMPUNDDTLSURL: "CompoundMaster.do?Action=ActivateInactivateCompoundDtls&CompID=",
  
    DEFAULTACTIONMSG: "Translate(DefaultActionneedstobeperformed)",
    COMPOUNDEDIT: "edit",
    COMPOUNDDELETE: "delete",
    COMPOUNDACTIVATE: "activate",
    COMPOUNDINACTIVATE: "inactivate",
    COMPOUNDVIEW: "view",
    ActiveStatus: "COM_ACTIVE",
    DELETESUCCESSUS: "DeleteSuccess",
    ACTIVESUCCESSUS: "ActiveSuccess",
    COMPOUNDMASTERPAGE: "CompoundingMaster.aspx",
    CANNOTDELETE: "Translate(CannotDelete)",
    CANNOTACTIVATE: "Translate(CannotActivate)",
    CANNOTINACTIVATE: "Translate(CannotInActivate)"

}
///#endregion

///#region ---------- Initialization

$(document).ready(function () {
    //<summary>function used to Document. ready()</summary>
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
   
    PageInit();

});
function AddNew() {
    ///<summary>Will redirect the listing page to Compound creation screen</summary>
    window.location = CompoundMaster.CreateCompound;
    return false;
}


function PageInit() {
    //<summary>function used to Set Control and Fill Details in PageLoad</summary>
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("select[id$=SearchType]").val(CompoundMaster.VALUEZERO);
    $("[id$=SearchValue]").val(CompoundMaster.TEXTEMPTY);
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();

    return false;
}

///#endregion

///#region ---------- Core Section

//<summary>function used to Fill All Compound Details to Grid</summary>
function BindGrid() {
    //var SearchVal = $("[id$=SearchValue]").val().replace(/[&]/g, "ampersand").replace(/'/g, "singlequote;");
    var SearchVal = $("[id$=SearchValue]").val().replace(/[&]/g, "ampersand");
    var ajaxUrl = CompoundMaster.COMPOUNDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + SearchVal + "&SBU=" + $("[id$=BizUnitPk]").val();
    $("#grdCompoundDetails").removeAttr("ajaxurl")
    $("#grdCompoundDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdCompoundDetails");
    GrandGrid.MakeGrid($("#grdCompoundDetails"));
    return false;
}

///<summary>function used to Handle Compound  Details Grid Command</summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function GridHandler(tr, command) {

    switch (command.toString().toLowerCase()) {

        case CompoundMaster.COMPOUNDDELETE:
            compundPK = GrandGrid.Utilities.GetColumnValue(tr, CompoundMaster.COMPOUNDPK, $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(CompoundMaster.DOUWANTTODELETE, CompoundMaster.CONFIRMATIONMSG, CompoundMaster.COMPOUNDDELETE, true);
            break;
        case CompoundMaster.COMPOUNDEDIT:
            compundPK = GrandGrid.Utilities.GetColumnValue(tr, CompoundMaster.COMPOUNDPK, $(tr).parent().parent().attr("id"));
            FillMachineDetails(compundPK);
            break;
        case CompoundMaster.COMPOUNDVIEW:
            compundPK = GrandGrid.Utilities.GetColumnValue(tr, CompoundMaster.COMPOUNDPK, $(tr).parent().parent().attr("id"));
            ViewCompoundDetails(compundPK);
            break;
        case CompoundMaster.COMPOUNDACTIVATE:
            compundPK = GrandGrid.Utilities.GetColumnValue(tr, CompoundMaster.COMPOUNDPK, $(tr).parent().parent().attr("id"));
            ActivateCompoundDetails(compundPK);
            break;
        case CompoundMaster.COMPOUNDINACTIVATE:
            compundPK = GrandGrid.Utilities.GetColumnValue(tr, CompoundMaster.COMPOUNDPK, $(tr).parent().parent().attr("id"));
            InActivateCompoundDetails(compundPK);
            break;
        default:
            alert(CompoundMaster.DEFAULTACTIONMSG);
            break;
    }
    return false;
}
function ActivateCompoundDetails(compundPK) {
    var msgtxt;
    $.get(CompoundMaster.ACTIVATEINACTIVATECOMPUNDDTLSURL + compundPK + "&Status=1", function (data) {
        // Check Machine Details Deleted Successfully or Not
        if (parseInt(data) == 1) {
            msgtxt = CompoundMaster.ACTIVATECOMPOUNDSUCCESSMSG;
        }
        else if (parseInt(data) == 0) {
            msgtxt = CompoundMaster.CANNOTACTIVATE;
        }
        else {

            msgtxt = CompoundMaster.ACTIONFAILEDMSG;
        }

        GrandScriptUtils.ShowModal(msgtxt, CompoundMaster.INFORMATIONTITLE, CompoundMaster.DELETESUCCESSUS);
    });
    return false;

}
function InActivateCompoundDetails(compundPK) {
    var msgtxt;
    $.get(CompoundMaster.ACTIVATEINACTIVATECOMPUNDDTLSURL + compundPK + "&Status=0", function (data) {
        // Check Machine Details Deleted Successfully or Not
        if (parseInt(data) == 1) {
            msgtxt = CompoundMaster.INACTIVATECOMPOUNDSUCCESSMSG;
        }
        else if (parseInt(data) == 0) {
            msgtxt = CompoundMaster.CANNOTINACTIVATE;
        }
        else {

            msgtxt = CompoundMaster.ACTIONFAILEDMSG;
        }

        GrandScriptUtils.ShowModal(msgtxt, CompoundMaster.INFORMATIONTITLE, CompoundMaster.DELETESUCCESSUS);
    });
    return false;
}
///<summary>Used to fill Machinery Details for editing</summary>
///<summary>Grid Handler Catch all the grid events in this function </summary>
/// <param name="compundPK"  type="Object">   
/// </param>
function FillMachineDetails(compundPK) {
    window.location = CompoundMaster.COMPOUNDMASTERPAGE + "?PK=" + compundPK;
}

///<summary>Used to fill and View Machinery Details for editing</summary>
///<summary>Grid Handler Catch all the grid events in this function </summary>
/// <param name="compundPK"  type="Object">   
/// </param>
function ViewCompoundDetails(compundPK) {
    ///<summary>Used to View Compond Details Can't Update Data</summary>
    window.location = CompoundMaster.COMPOUNDMASTERPAGE + "?PK=" + compundPK + "&Status=0";
}

//<summary>function used to Delete Compound  details </summary>
function DeleteMachineDtls() {
    var msgtxt;
    $.get(CompoundMaster.DELETEMACHINEDTLSURL + compundPK, function (data) {
        // Check Machine Details Deleted Successfully or Not  
        if (parseInt(data) == 1) {
            msgtxt = CompoundMaster.DELETECOMPOUNDSUCCESSMSG;
        }
        else if (parseInt(data) == 0) {
            msgtxt = CompoundMaster.CANNOTDELETE;
        }
        else {

            msgtxt = CompoundMaster.ACTIONFAILEDMSG;
        }

        GrandScriptUtils.ShowModal(msgtxt, CompoundMaster.INFORMATIONTITLE, CompoundMaster.DELETESUCCESSUS);
    });
    return false;
}

///<summary>Function invoke after Model popup ok Click</summary>
/// <param name="command" optional="true" type="String">
/// Click OK which which methode perform based on this command
/// </param>
function ModalOk(command) {
    switch (command) {
        case CompoundMaster.COMPOUNDDELETE:
            DeleteMachineDtls();

            break;
        case CompoundMaster.DELETESUCCESSUS:
            PageInit();

            break;

    }
    return false;
}

//<summary>function Used to Reset Page</summary>
function ResetPage() {
    //Reseting all input controls in the page
//    $(document.forms[0]).find("input").each(function () {
//        var idval = $(this).attr("id");
//        if (idval.search("UserPk") == -1) {
//            $(this).val(CompoundMaster.TEXTEMPTY);
//        }
//    });
//    //Selecting the first value in all drop downs
//    $(document.forms[0]).find("select").each(function () {
//        $(this).val($(this).find("option:eq(0)").val());
//    });
    PageInit();
    return false;
}

//<summary> AutoComplete - For Search Compound Detials </summary>
function SearchInit() {
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", CompoundMaster.COMPOUNDDTLSAUTOCOMPLETEURL + $("[id$=BizUnitPk]").val(), "SearchType");
}

//<summary>Function to SearchType Change Function And Search Details </summary>
function SearchSettings() {
    $("[id$=SearchType]").change(function () {
        SetSearchType();
    });
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });
    BindGrid();
    $("[id$=SearchValue]").hide()
    $("[id$=imbSearch]").hide();
}

///<summary>Set Search Type - Show Hide SearchValue textBox And Button </summary>
function SetSearchType() {
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(CompoundMaster.TEXTEMPTY);
    if (strname == CompoundMaster.VALUEZERO) {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
        BindGrid();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
        $("input[id$=SearchValue]").focus();
    }
}

///<summary>filling gridview after entering search value in search textbox</summary>
function AfterSelect() {
    BindGrid();
}
function AfterGridBind() {
    ///<summary>Setting width of the template after binding grid</summary>
    //$("#grdStore th:last").width("5%");
    $("#grdCompoundDetails").find("tr:has(td)").each(function () {
        var tableID = $(this).parents("table:first").attr("id");
        var ActiveStatus = GrandGrid.Utilities.GetColumnValue(this, CompoundMaster.ActiveStatus, tableID);
        var AlreadyExists = GrandGrid.Utilities.GetColumnValue(this, "COM_FLAG", tableID);
        if (AlreadyExists == "true") {
            $(this).find("td:last input[id$=imbEdit]").hide();
            $(this).find("td:last input[id$=imbDeleteCopmound]").hide();
            $(this).find("td:last input[id$=imbView]").show();
        }
        else {
            $(this).find("td:last input[id$=imbEdit]").show();
            $(this).find("td:last input[id$=imbDeleteCopmound]").show();
            $(this).find("td:last input[id$=imbView]").hide();
        }
        //Acive
        if (ActiveStatus == 1) {
            $(this).find("td:last input[id$=imbActive]").show();
            $(this).find("td:last input[id$=imbInActive]").hide();
        }
        //InActive
        else if (ActiveStatus == 0) {
            $(this).find("td:last input[id$=imbActive]").hide();
            $(this).find("td:last input[id$=imbInActive]").show();
        }

    });
}
///#endregion






