
///#region -------------- Global Variable -------
var tankMasterID = 0;
var tankTypeID = 0;
var locationID = 0;
var LatModDate;
var TankEditPk = 0;
///#endregion

//#region ------- Configuration Section --------
var TankMaster = {
    AutoCompleteURL: "TankManagement.do?Action=GetSearchValue&SBUPk=",
    FillTankTypeDropdownURL: "TankManagement.do?Action=GetTankType&SBUPk=",
    FillLocationDropdownURL: "MachineryManagement.do?Action=GetLocation&SBUPk=",
    FillUOMDropdownURL: "UOMManagement.do?Action=GetUnit",
    SaveURL: "TankManagement.do?Action=SavePage",
    SaveLocationURL: "MachineryManagement.do?Action=SaveLocation",
    SaveTankTypeURL: "TankManagement.do?Action=SaveTankType",
    BindGridURL: "TankManagement.do?Action=GetTankMasterList&Status=",
    BindGridTankURL: "TankManagement.do?Action=GetTankTypeList&SBUPk=",
    BindGridLocationURL: "MachineryManagement.do?Action=GetLocationList&SBUPk=",
    DeleteURL: "TankManagement.do?Action=DeleteTankMasterDtls&TankMasterID=",
    DeleteLocationURL: "MachineryManagement.do?Action=DeleteLocation&locID=",
    DeleteTankTypeURL: "TankManagement.do?Action=DeleteTankType&TankTypeID=",
    FillPlantDropdownURL: "CommonManagement.do?Action=GetPlant&SBU=",
    // TreeView Fill
    ProcessMappingTreeURL: "CommonManagement.do?Action=GetAppConfigTree&tankPk=",
    FillLineDropdownURL: "TankManagement.do?Action=GetAllLine&SBUPk=",

    //Constants
    RootName: "Process",
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    Param: "&MatCagID=",
    EDITTYPE: "edittype",
    EDITLOCATION: "editlocation",
    DELETETYPE: "deletetype",
    DELETELOCATION: "deletelocation",
    DELETE: "DELETE",

    TankMasterID: "TNK_PK",
    TankName: "TNK_NAME",
    TankMasterTypeID: "TNK_TYPE",
    TankMasterLocationID: "TNK_LOCATION",
    TankMasterUOMID: "TNK_CAPACITY_UOM",
    Capacity: "TNK_CAPACITY",
    UOMPK: "ITM_TYPE",
    TankCode: "TNK_CODE",
    Remarks: "TNK_REMARKS",
    TankHeight: "TNK_HEIGHT",
    CapacityperCm: "TNK_CAPACITY_PER_CM",
    TankSlope: "TNK_SLOPE",
    LastModdate: "LAST_MOD_DT",
    CompoundGenNo: "TNK_GEN_CODE",
    TankMasterLineID: "TNK_LINE",
    TankMasterPlantID: "TNK_PLANT",
    TankActive: "TNK_ACTIVE",
    HasStock: "TNK_HAS_STOCK",
    CurrentStock: "TNK_STOCK",
    Sequence: "TNK_SEQ",

    TankTypeID: "TNT_PK",
    LocationPK: "LOC_PK",
    TankTypeName: "TNT_NAME",
    LocationName: "LOC_NAME",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    TankSaveMessage: "Translate(TankDetailsSavedSuccesfully)",

    CodeExistsMessage: "Translate(AlreadyExists)",
    TankCodeAlreadyExistsInItemMessage: "Translate(TankCodeAlreadyExistsInItem)",
    NameExistMessage: "Translate(NameExist)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    TankDeleteMessage: "Translate(TankDetailsDeletedSuccesfully)",
    TankTypeDeleteMessage: "Translate(TankTypeDetailsDeletedSuccesfully)",
    LocationDeleteMessage: "Translate(LocationDetailsDeletedSuccesfully)",
    Used: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    AlreadyDeleteMessage: "Translate(AlreadyDeletedRecord)",
    EditUsedByAnotherUser: "Translate(AlreadyUpdatedRecord)",

    //validation messages.
    EnterTankName: "Translate(EnterTankName)",
    EnterLocation: "Translate(EnterLocation)",
    EnterCapacity: "Translate(EnterCapacity)",
    selectUOM: "Translate(PleaseSelectUOM)",
    SelectLocation: "Translate(PleaseSelectLocation)",
    EnterTotalHeight: "Translate(EnterTotalHeight)",
    NonZeromsg: "Translate(NonZeroValue)",
    MsgGreaterZero: "Translate(MsgGreaterZero)",

    SelectTankType: "Translate(SelectTankType)",
    EnterTankType: "Translate(EnterTankType)",

    EnterCapacityperCm: "Translate(EnterCapacityperCm)",
    EnterTankSlope: "Translate(EnterTankSlope)",
    DeletedRecord: "Translate(DeletedRecord)"
}
//#endregion

///#region------- Initialization Section --------
//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != TankMaster.TextZero);
}, 'Translate(Pleaseselectanoption)');

$.validator.addMethod('FourDigitsTwoDecimal', function (value, element) {
    return this.optional(element) || /(?!^0*$)(?!^0*\.0*$)^\d{1,4}(\.\d{1,2})?$/.test(value);
}, 'Translate(Max4NumericAND2decimalallowed)');

$.validator.addMethod('TwoDigitsTwoDecimal', function (value, element) {
    return this.optional(element) || /(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$/.test(value);
}, 'Translate(Max2NumericAND2decimalallowed)');


$.validator.addMethod('FourDigitsSixDecimalwithMinus', function (value, element) {
    return this.optional(element) || /^[-+]?\d{1,4}(\.\d{1,6})?$/.test(value);
}, 'Max 4 Numeric 6 decimal & sign symbol allowed'); //Translate(Max4NumericAND2decimalallowedwithMinus)

$.validator.addMethod('FourDigitsFourDecimalwithMinus', function (value, element) {
    return this.optional(element) || /^[-+]?\d{1,4}(\.\d{1,4})?$/.test(value);
}, 'Max 4 Numeric 4 decimal & sign symbol allowed'); //Translate(Max4NumericAND2decimalallowedwithMinus)

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    //Page Initial condtions
    PageInit();
    //    $("textarea[id$=Remarks]").keypress(function (e) {
    //        var txt = $(this).val();
    //        if (txt.length > 500) {
    //            e.preventDefault();
    //        }
    //    });    

});

function PageInit() {
    $("select[id$=ddlStatus]").val('1');
    $("input[id$=TankActive]").attr("checked", true);
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("[id$=imbSave]").hide();
    $("[id$=imbCancel]").hide();
    $("[id$=imdReset]").show();
    $("[id$=imbAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();

    $("#divAddLocation").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            ResetLocation();
            RemoveLocationValidations();
        }
    });
    $("#divTankType").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
            //$("input[id$=TankTypeName]").focus();
        },
        beforeClose: function (event, ui) {
            ResetTankType();
            RemoveTypeValidations();
        }
    })


    FillLocation(0);
    FillTankType(0);
    FillUOM();
    FillLine(0);
    FillPlant(0);
    //FillProcessTree();

    SetSearchType();
    //initializing search.
    SearchInit();
    return false;
}

function FillProcessTree() {
    var tankPk = $("input[id$=TankMasterPk]").val();
    SetTreeHeaderStructure("trvProcessMap", TankMaster.ProcessMappingTreeURL + tankPk, TankMaster.RootName, true, false, "", "&pVal=0");    // set the tree view parameters
    MakeMultiTree();
}

function GetSelectedProcess() {
    //<summary>Function Used to get the all checked dept details </summary>
    var ProcessArray = new Array();
    var processPk = 0;
    $("#trvProcessMap").find("input[type=checkbox]:checked").each(function () {
        processPk = $(this).attr("id");
        processPk = processPk.substr(processPk.lastIndexOf("_") + 1, processPk.length);
        ProcessArray.push({ TNP_PROCESS: processPk });
    });
    return ProcessArray;
}

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    //    if (strname == "0") {
    //        $("[id$=SearchValue]").hide()
    //        $("[id$=imbSearch]").hide();
    BindGrid();
    //    }
    //    else {
    //        $("[id$=SearchValue]").show()
    //        $("[id$=imbSearch]").show();
    //    }
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", TankMaster.AutoCompleteURL + $("[id$=BizUnitPk]").val(), "SearchType");
}
///#endregion
///#endregion

///#region --------- Core Section ---------------

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    // RemoveValidations();
    switch (command.toString()) {
        // To Delete Details 
        case TankMaster.DeleteCommand:
            tankMasterID = GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterID, $(tr).parents("table:first").attr("id"));
            LatModDate = GrandGrid.Utilities.GetColumnValue(tr, TankMaster.LastModdate, $(tr).parents("table:first").attr("id"));

            // No need to change Last modified date - 30/05/2022
            //// Do Confirmation.. Before Delete Details    
            //LatModDate = LatModDate.replace("/", "");
            //LatModDate = LatModDate.replace("Date", "");
            //LatModDate = LatModDate.replace("/", "");
            //LatModDate = LatModDate.replace("(", "");
            //LatModDate = LatModDate.replace(")", "");
            //LatModDate = LatModDate.split('+');
            //var timestamp = parseInt(LatModDate[0]);
            //var date = new Date(timestamp);
            //var formattedDate = (date.getMonth() + 1) + "-" + date.getDate() + "-" + date.getFullYear();
            //var hours = date.getHours(); // minutes part from the timestamp
            //var minutes = date.getMinutes(); // seconds part from the timestamp
            //var seconds = date.getSeconds(); // will display time in 10:30:23 format
            //var formattedTime = hours + ':' + minutes + ':' + seconds;
            //formattedDate = formattedDate + " " + formattedTime;
            //LatModDate = formattedDate;

            GrandScriptUtils.ShowModal(TankMaster.DeleteConfirmationMessage, TankMaster.ConfirmationMessage, TankMaster.DeleteCommand, true);
            break;

        // To Edit Details              
        case TankMaster.EditCommand:
            FillDetails(tr);
            break;
        // Default Handler    
        default:
            alert(TankMaster.DefaultAction);
            break;
    }
    return false;

}
///<summary>Grid Handler Catches all grid events from vendor Type popup </summary>
function GridHandlerType(tr, command) {
    switch (command.toString().toLowerCase()) {

        case TankMaster.EDITTYPE:
            FillTankTypeDetails(tr);
            return false;
            break;

        case TankMaster.DELETETYPE:
            tankTypeID = GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankTypeID, $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(TankMaster.DeleteConfirmationMessage, TankMaster.ConfirmationMessage, TankMaster.DELETETYPE, true);
            return false;
            break;

        default:

            GrandScriptUtils.ShowModal(TankMaster.DefaultAction, TankMaster.MessageBoxTitle);
            return false;
            break;


    }
    return false;

}
///<summary>Grid Handler Catches all grid events from location Type popup </summary>
function GridHandlerLocation(tr, command) {
    switch (command.toString().toLowerCase()) {

        case TankMaster.EDITLOCATION:
            FillLocationDetails(tr);
            return false;
            break;

        case TankMaster.DELETELOCATION:
            locationID = GrandGrid.Utilities.GetColumnValue(tr, TankMaster.LocationPK, $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(TankMaster.DeleteConfirmationMessage, TankMaster.ConfirmationMessage, TankMaster.DELETELOCATION, true);
            return false;
            break;

        default:

            GrandScriptUtils.ShowModal(TankMaster.DefaultAction, TankMaster.MessageBoxTitle);
            return false;
            break;


    }
    return false;

}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {
    switch (command) {


        case TankMaster.DELETETYPE:
            DeleteTankTypeDetails();
            break;
        case TankMaster.DELETELOCATION:
            DeleteLocationDetails();
            break;
        case TankMaster.DELETE:
            DeleteDetails();
            break;
        case TankMaster.SaveCommand:
            PageInit();
            break;


    }
    return false;
}
///#endregion

///#region --------- ADDnew,reset,checkstatus ---------------
///<summary>Function To Show Data Entry Form </summary>
function AddNew(stat) {
    $("[id$=imbSave]").show();
    $("[id$=imdReset]").hide();
    $("[id$=imbCancel]").show();
    $("[id$=imbAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    $("[id$=TankCode]").focus();
    FillProcessTree();
    if (stat)
        FillLine();
    return false;
    if (stat)
        FillPlant();
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page


    //    $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () {
    //        var idval = $(this).attr("id");
    //        //Avoid DSG_PK to get the value
    //        if (idval.search("TankMasterPk") != -1)
    //            $(this).val("0");
    //        //Avoid UserPk to get the value of log in user
    //        else if (idval.search("UserPk") == -1)
    //            $(this).val("");
    //    });
    //    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button],input[type=submit])").each(function () {
    //            var idval = $(this).attr("id");
    //            if (!Checkstatus(idval)) {
    //                $(this).val("");
    //            }
    //        });
    //    //Selecting the first value in all drop downs
    //        $(document.forms[0]).find("select").each(function () {
    //            idval = $(this).attr("id");
    //            if (idval.search("SBU") == -1)
    //                $(this).val($(this).find("option:eq(0)").val());
    //        });
    $("input[id$=TankMasterPk]").val("0");
    //Reset validation
    //RemoveValidation();
    $(document.forms[0]).validate().resetForm();
    // Focus To SearchType When Reset
    $("[id$=SearchType]").focus();
    $("input[id$=TankName]").val("");
    $("input[id$=TankCapacity]").val("");
    $("input[id$=LAST_MOD_DT]").val("");
    $("[id$=SearchType]").val("0");
    ClearTankDetails();
    PageInit();
    $("input[id$=TNK_ACTIVE]").attr("checked", true);
    return false;
}



function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page
    if (controlID.search("UserID") != -1) {
        return true;
    }
    //    if (controlID.search("TankMasterPk") != -1) {
    //        return true;
    //    }
    if (controlID.search("UserPk") != -1) {
        return true;
    }

    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
    if (controlID.search("TankTypePk") != -1) {
        return true;
    }
    if (controlID.search("LocationPk") != -1) {
        return true;
    }
    return false;
}
///#endregion

///#region---- Fetch Data To Populate In Controls



function FillTankType(tankTypePK) {
    //<summary>function To Fill Tank Type Details </summary>
    // Get id of the TankType DropDown
    var drpID = $("select[id$=TankType]").attr("id");
    //Fill Tank Type to the Category DropDown, Name as Text, PK as Value
    $.getJSON(TankMaster.FillTankTypeDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, tankTypePK);
    });

}
function FillLocation(locPK) {
    //<summary>function To Fill Location Details </summary>
    // Get id of the Location DropDown
    var drpID = $("select[id$=Location]").attr("id");
    //Fill Location Details to the Category DropDown, Name as Text, PK as Value
    $.getJSON(TankMaster.FillLocationDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, locPK);
    });

}

function FillLine(LinePk) {
    //<summary>function To Fill Line </summary> 
    // Get id of the Location DropDown
    var drpID = $("select[id$=Line]").attr("id");
    $.getJSON(TankMaster.FillLineDropdownURL + $("[id$=BizUnitPk]").val() + "&LINEPK=" + TankEditPk + "&STATUS=1" + "&IS_VIRTUAL=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, LinePk);
    });

}

function FillPlant(PlantPk) {
    //<summary>function To Fill Plant </summary> 
    // Get id of the Location DropDown
    var drpID = $("select[id$=TNK_PLANT]").attr("id");
    $.getJSON(TankMaster.FillPlantDropdownURL + $("[id$=BizUnitPk]").val() + "&PlantID=" + TankEditPk, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, PlantPk);
    });

}

function FillUOM() {
    //<summary>function To Fill UOM Type Details </summary>
    // Get id of the UOM DropDown
    //    var drpID = $("select[id$=UOMPk]").attr("id");
    //Fill UOM Details to the Category DropDown, Name as Text, PK as Value
    var ajaxxurl = TankMaster.FillUOMDropdownURL + "&SBU=" + $("[id$=BizUnitPk]").val() + "&UOMTypeName=Weight";
    $.get(ajaxxurl, function (data) {
        var SelVal;
        var UOMArray = new Array();
        UOMArray = data;
        for (var i = 0; i < UOMArray.length; i++) {
            if (UOMArray[i].Text.toLowerCase() == "kg") {
                $("input[id$=UOMPk]").val(UOMArray[i].Value);
            }
        }
        //  GrandScriptUtils.FillDropDown(drpID, data, true, true, SelVal);
        // $("select[id$=UOMPk]").attr("disabled", true);

    });

}
function AddTankType() {
    $("#divTankType").dialog("open");
    $("#divTankType").dialog({ width: 500, height: 350, resizable: true });
    $("#divTankType").css({ "min-height": "300", "margin-top": "25px" });

    BindGridTankType();
    $("input[id$=TankTypeName]").val("");
    //$("input[id$=TankTypeName]").focus();
    setTimeout(function () { $("input[id$=TankTypeName]").focus(); }, 10);
    return false;
}
function AddLocation() {
    $("#divAddLocation").dialog("open");
    $("#divAddLocation").dialog({ width: 500, height: 350, resizable: true });
    $("#divAddLocation").css({ "min-height": "300", "margin-top": "25px" });
    // $("[id$=LocationName]").focus();
    BindGridTLocation();
    setTimeout(function () { $("input[id$=LocationName]").focus(); }, 10);
    return false;
}
function FillDetails(tr) {
    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param>
    var grdID = $(tr).parents("table:first").attr("id");
    $("input[id$=TankMasterPk]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterID, grdID));
    $("input[id$=TankName]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankName, grdID));
    $("input[id$=TankCapacity]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.Capacity, grdID));
    //$("select[id$=UOMPk]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterUOMID, grdID)); 
    //$("[id$=UOMPk]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterUOMID, grdID));   
    FillLocation(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterLocationID, grdID));
    FillTankType(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterTypeID, grdID));
    $("input[id$=TankCode]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankCode, grdID));
    //$("input[id$=Remarks]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.Remarks, grdID));
    $("textarea[id$=Remarks]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.Remarks, grdID));
    $("input[id$=TankHeight]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankHeight, grdID));

    $("input[id$=Sequence]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.Sequence, grdID));


    $("input[id$=CapacityperCm]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.CapacityperCm, grdID));
    $("input[id$=TankSlope]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankSlope, grdID));
    if ((GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankActive, grdID) == "1")) {
        $("input[id$=TankActive]").attr("checked", true);
    }
    else {

        $("input[id$=TankActive]").attr("checked", false);
    }
    //alert(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.HasStock, grdID));
    if ((GrandGrid.Utilities.GetColumnValue(tr, TankMaster.HasStock, grdID) == "1")) {
        $("input[id$=HasStock]").attr("checked", true);
    }
    else {

        $("input[id$=HasStock]").attr("checked", false);
    }
    $("input[id$=LAST_MOD_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.LastModdate, grdID));
    if (GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterLineID, grdID).toString() != 'null' && GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterLineID, grdID).toString() != 'undefined')
        TankEditPk = GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterLineID, grdID);
    FillLine(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterLineID, grdID));
    TankEditPk = 0;
    if (GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterPlantID, grdID).toString() != 'null' && GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterPlantID, grdID).toString() != 'undefined')
        TankEditPk = GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankMasterPlantID, grdID);
    FillPlant(TankEditPk);
    if (GrandGrid.Utilities.GetColumnValue(tr, TankMaster.CompoundGenNo, grdID) == 'null' || GrandGrid.Utilities.GetColumnValue(tr, TankMaster.CompoundGenNo, grdID) == 'undefined') {
        $("input[id$=CompoundGenNo]").val('');
    }
    else {
        $("input[id$=CompoundGenNo]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.CompoundGenNo, grdID));
    }
    FillProcessTree();
    //changing mode to  lising
    AddNew(false);
    // Filling UOM(CategoryID,SelctVal)
    //    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOU, grdID))
    //    FillVendorUOM(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID));
    //    FillVendorMappingXmlDetails(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, grdID))



}
function FillTankTypeDetails(tr) {
    var grdID = $(tr).parents("table:first").attr("id");
    $("input[id$=TankTypePk]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankTypeID, grdID));
    $("input[id$=TankTypeName]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.TankTypeName, grdID));
    $("[id$=TankTypeName]").focus();
}
function FillLocationDetails(tr) {
    var grdID = $(tr).parents("table:first").attr("id");
    $("input[id$=LocationPk]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.LocationPK, grdID));
    $("input[id$=LocationName]").val(GrandGrid.Utilities.GetColumnValue(tr, TankMaster.LocationName, grdID));
    $("[id$=LocationName]").focus();
}
///#endregion

///#region---- Data Management Section----


function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(TankMaster.DeleteURL + tankMasterID + "&LAST_MOD_DT=" + LatModDate, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = TankMaster.TankDeleteMessage;
        else if (parseInt(data) == 0)
            msgtxt = TankMaster.Used;
        else if (parseInt(data) == -5)
            msgtxt = TankMaster.DeletedRecord;
        else if (parseInt(data) == -3)
            msgtxt = TankMaster.EditUsedByAnotherUser;
        else
            msgtxt = TankMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, TankMaster.MessageBoxTitle, TankMaster.DeleteMessageCommand);
        BindGrid();
    });
    return false;
}
function DeleteTankTypeDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(TankMaster.DeleteTankTypeURL + tankTypeID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            msgtxt = TankMaster.TankTypeDeleteMessage;
            FillTankType(0);
        }
        else if (parseInt(data) == 0)
            msgtxt = TankMaster.Used;
        else
            msgtxt = TankMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        BindGridTankType();
        GrandScriptUtils.ShowModal(msgtxt, TankMaster.MessageBoxTitle);

    });
    return false;
}
function DeleteLocationDetails(tr) {
    ///<summary>Function To Get delete and Delete Location, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(TankMaster.DeleteLocationURL + locationID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1) {
            msgtxt = TankMaster.LocationDeleteMessage;
            FillLocation(0);
        }
        else if (parseInt(data) == 0) {
            msgtxt = TankMaster.Used;
        }
        else {
            msgtxt = TankMaster.ActionFailedMessage;
        }
        // Show MeesageBox For  Delete Status
        BindGridTLocation();
        GrandScriptUtils.ShowModal(msgtxt, TankMaster.MessageBoxTitle);

    });
    return false;
}


function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    //-----------------------------------------------------------------------
    var ddlStatus = $("select[id$=ddlStatus]").val();

    //    $("[id$=ddlStatus]").val("");
    var ajaxUrl = TankMaster.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&SBUPk=" + $("[id$=BizUnitPk]").val() + "&StatusPK=" + ddlStatus;
    //------------------------------------------------------------------------
    //var ajaxUrl = TankMaster.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&SBUPk=" + $("[id$=BizUnitPk]").val();
    $("#grdTankMaster").removeAttr("ajaxurl")
    $("#grdTankMaster").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdTankMaster");
    GrandGrid.MakeGrid($("#grdTankMaster"));
    return false;
}
function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}
///<summary>To handle bind grid for TankType Type</summary>
function BindGridTankType() {
    var ajaxUrl = TankMaster.BindGridTankURL + $("[id$=BizUnitPk]").val();
    $("#grdTankType").removeAttr("ajaxurl")
    $("#grdTankType").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdTankType");
    GrandGrid.MakeGrid($("#grdTankType"));
}
///<summary>To handle bind grid for Location Type</summary>
function BindGridTLocation() {
    var ajaxUrl = TankMaster.BindGridLocationURL + $("[id$=BizUnitPk]").val();
    $("#grdLocation").removeAttr("ajaxurl")
    $("#grdLocation").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdLocation");
    GrandGrid.MakeGrid($("#grdLocation"));
}
function SaveTankType() {
    ///<summary>Function used to saving materials  </summary>
    RemoveValidations();
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        if ($("input[id$=TNK_ACTIVE]").is(':checked') == true)
            $("[id$=TNK_ACTIVE]").val('1');
        else
            $("[id$=TNK_ACTIVE]").val('0');
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(TankMaster.SaveTankTypeURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully 
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(TankMaster.CodeExistsMessage, TankMaster.MessageBoxTitle);

            }
            else if (parseInt(data) > 0) {

                // GrandScriptUtils.ShowModal(TankMaster.taMaterialSaveMessage, TankMaster.MessageBoxTitle, TankMaster.SaveCommand);
                BindGridTankType();
                FillTankType(data);
                ResetTankType();
                $("[id$=divTankType]").dialog("close");
            }
            else {
                GrandScriptUtils.ShowModal(TankMaster.ActionFailedMessage);
                // ResetPage();
            }

        });
    }
    return false;
}
///<summary>Used to reset Location popup</summary>
function ResetTankType() {
    $("input[id$=TankTypeName]").val("");
    $("input[id$=TankTypePk]").val("0");

}
function SaveLocation() {
    ///<summary>Function used to saving materials  </summary>
    RemoveValidations();
    AddValidations(3);
    if ($(document.forms[0]).valid()) {

        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(TankMaster.SaveLocationURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(TankMaster.CodeExistsMessage, TankMaster.MessageBoxTitle);

            }
            else if (parseInt(data) > 0) {

                // GrandScriptUtils.ShowModal(TankMaster.MaterialSaveMessage, TankMaster.MessageBoxTitle, TankMaster.SaveCommand);
                BindGridTLocation();
                FillLocation(data);
                ResetLocation();
                $("[id$=divAddLocation]").dialog("close");

            }
            else {
                GrandScriptUtils.ShowModal(TankMaster.ActionFailedMessage);
                // ResetPage();
            }

        });
    }
    return false;
}
///<summary>Used to reset Location popup</summary>
function ResetLocation() {
    $("input[id$=LocationName]").val("");
    $("input[id$=LocationPk]").val("0");

}
function SavePage() {
    ///<summary>Function used to saving materials  </summary>
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        if ($("input[id$=TankCapacity]").val() <= 0) {
            GrandScriptUtils.ShowModal(TankMaster.MsgGreaterZero, TankMaster.MessageBoxTitle);
            return false;
        }
        $("[id$=TankDetails]").val(JSON.stringify(GetSelectedProcess()));

        //        TankMaster.Sequence = $("input[id$=Sequence]").val();
        //        alert($("input[id$=Sequence]").val());

        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(TankMaster.SaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully  

            if (parseInt(data) == 0) {
                //                GrandScriptUtils.ShowModal(TankMaster.CodeExistsMessage, TankMaster.MessageBoxTitle);
                GrandScriptUtils.ShowModal(TankMaster.CodeExNameExistMessageistsMessage, TankMaster.MessageBoxTitle);
                $("input[id$=TankName]").focus();
                //                $("input[id$=TankCode]").focus();
            }
            else if (parseInt(data) > 0) {

                GrandScriptUtils.ShowModal(TankMaster.TankSaveMessage, TankMaster.MessageBoxTitle, TankMaster.SaveCommand);
                ResetPage();
            }
            else if (parseInt(data) == -5) {

                GrandScriptUtils.ShowModal(TankMaster.AlreadyDeleteMessage, TankMaster.MessageBoxTitle);
                ResetPage();
            }
            else if (parseInt(data) == -3) {

                GrandScriptUtils.ShowModal(TankMaster.EditUsedByAnotherUser, TankMaster.MessageBoxTitle);
                ResetPage();
            }
            else if (parseInt(data) == -10) {
                GrandScriptUtils.ShowModal(TankMaster.CodeExistsMessage, TankMaster.MessageBoxTitle);
                $("input[id$=TankName]").focus();
            }
            else if (parseInt(data) == -11) {
                GrandScriptUtils.ShowModal(TankMaster.TankCodeAlreadyExistsInItemMessage, TankMaster.MessageBoxTitle);
                $("input[id$=TankCode]").focus();
            }

            else {
                GrandScriptUtils.ShowModal(TankMaster.ActionFailedMessage);

            }

        });
    }
    return false;
}
///#endregion
function ClearTankDetails() {
    //<summary>function used to Clear mapping  Details</summary>

    $("input[id$=TankName]").val("");
    $("input[id$=TankCapacity]").val("");
    $("select[id$=Location]").val("0");
    $("select[id$=TankType]").val("0");
    //    $("select[id$=UOMPk]").val("0");
    //$("input[id$=UOMPk]").val("0");
    $("input[id$=TankCode]").val("");
    $("input[id$=TankHeight]").val("");
    $("input[id$=CapacityperCm]").val("");
    $("input[id$=Remarks]").val('');
    $("input[id$=TankSlope]").val("");
    $('textarea[id$=Remarks]').val("");
    $("input[id$=CompoundGenNo]").val("");
    $("input[id$=Sequence]").val("");
    //    $("input[id$=TankMasterPk]").val("");
    //    $("input[id$=TankTypePk]").val("");
    //    $("input[id$=LocationPk]").val("");

    FillProcessTree();
}
///#endregion

///#region---------- Validations ----------------
function AddValidations(mode) {
    ///<summary>function To Validations </summary>
    var processPk = 0;
    $("#trvProcessMap").find("input[type=checkbox]:checked").each(function () {
        processPk = $(this).attr("id");
        processPk = processPk.substr(processPk.lastIndexOf("_") + 1, processPk.length);

    });

    if (mode == 1) {

        RemoveValidations();
        $("input[id$=TankName]").rules("add", {
            required: true,
            maxlength: 100,
            minlength: 3,
            messages: { required: TankMaster.EnterTankName }
        });
        $("input[id$=TankCapacity]").rules("add", {
            required: true,
            NonZero: true,
            // ThreeDecimal: true,
            messages: { required: TankMaster.EnterCapacity, NonZero: "Translate(NonZeroValue)" }

        });
        //        $("select[id$=UOMPk]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: TankMaster.selectUOM}
        //        });

        $("select[id$=Location]").rules("add", {
            selectNone: true,
            messages: { selectNone: TankMaster.SelectLocation }
        });
        $("select[id$=TankType]").rules("add", {
            selectNone: true,
            messages: { selectNone: TankMaster.SelectTankType }
        });
        if (processPk == 15) {
            $("input[id$=TankHeight]").rules("add", {
                required: true,
                NonZero: true,
                FourDigitsTwoDecimal: true,
                messages: { required: TankMaster.EnterTotalHeight, NonZero: "Translate(NonZeroValue)" }

            });
            $("input[id$=CapacityperCm]").rules("add", {
                required: true,
                NonZero: true,
                maxlength: 10,
                FourDigitsFourDecimalwithMinus: true,
                messages: { required: TankMaster.EnterCapacityperCm, NonZero: "Translate(NonZeroValue)" }

            });
        }
        else {
            $("input[id$=CapacityperCm]").rules("add", {
                required: false,
                NonZero: true,
                maxlength: 10,
                FourDigitsFourDecimalwithMinus: true,
                messages: { required: TankMaster.EnterCapacityperCm, NonZero: "Translate(NonZeroValue)" }

            });
        }
        $("input[id$=TankSlope]").rules("add", {
            required: true,
            maxlength: 10,
            FourDigitsFourDecimalwithMinus: true,
            messages: { required: TankMaster.EnterTankSlope }

        });
    }
    if (mode == 2) {
        RemoveTypeValidations();
        $("input[id$=TankTypeName]").rules("add", {
            required: true,
            maxlength: 100,
            minlength: 3,
            messages: { required: TankMaster.EnterTankType }
        });

    }
    if (mode == 3) {
        RemoveLocationValidations();
        $("input[id$=LocationName]").rules("add", {
            required: true,
            maxlength: 100,
            minlength: 3,
            messages: { required: TankMaster.EnterLocation }
        });
    }


}
//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $("input[id$=TankName]").rules("remove");
    $("input[id$=TankCapacity]").rules("remove");
    // $("input[id$=TankTypeName]").rules("remove");
    // $("input[id$=LocationName]").rules("remove");
    //    $("select[id$=UOMPk]").rules("remove");
    $("select[id$=Location]").rules("remove");
    $("select[id$=TankType]").rules("remove");

    $("input[id$=TankHeight]").rules("remove");
    $("input[id$=CapacityperCm]").rules("remove");
    $("input[id$=TankSlope]").rules("remove");

}
function RemoveLocationValidations() {
    $("input[id$=LocationName]").rules("remove");
}
function RemoveTypeValidations() {
    $("input[id$=TankTypeName]").rules("remove");
}

function CalculateTotalCapacity() {
    var totalHeight = 0;
    var capacity = 0;
    var slope = 0;

    if ($("[id$=TankHeight]").val() != '') {
        totalHeight = parseFloat($("[id$=TankHeight]").val());
    }
    if ($("[id$=CapacityperCm]").val() != '') {
        capacity = parseFloat($("[id$=CapacityperCm]").val());
    }
    if ($("[id$=TankSlope]").val() != '') {
        slope = parseFloat($("[id$=TankSlope]").val());
    }

    var totCapacity = (totalHeight * capacity) + slope;
    totCapacity = Round(totCapacity, 3);
    $("[id$=TankCapacity]").val(totCapacity);
    if ($("[id$=TankCapacity]").val() == "NaN") {
        $("[id$=TankCapacity]").val('0');
    }
}

function Round(x, y) {
    ///<summary>Method to round decimal no. to given no. of positions</summary>
    /// <param name="x" >
    ///     Input decimal value
    /// </param>
    ///<param name="y" >
    ///     No. of decimal points to be restricted
    /// </param>

    return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);
}
///#endregion

function AfterGridBind(grdID) {
    if (grdID == "grdTankMaster") {

        $("#grdTankMaster").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "TNK_ACTIVE_TEXT", "grdTankMaster");

            colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "TNK_ACTIVE", "grdTankMaster");

            if (colIndex != null) {
                if (colIndexActive != null) {
                    if ($(this).find("td:eq(" + colIndexActive + ")").html() == 0) {
                        $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                    }
                    else {
                        $(this).find("td:eq(" + colIndex + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                    }
                }
            }
        });


    }
}
function AllowOnlyNumberswithMinus(event, AllowDot) {
    var keyCode = event.keyCode ? event.keyCode : event.which;
    //alert(keyCode);
    //Backspace, Tab, Enter, End, Home, Left Arrow, Up Arrow, Right Arrow, Down Arrow
    var arrSafeKeys = [8, 9, 13, 39, 37, 190, 46, 45]; // 35,36,40,38,37, // 190 for point
    //if you find safe char, replace keycode with the keycode of 1, it will bypass the numeric check
    keyCode = $.inArray(keyCode, arrSafeKeys) >= 0 ? 49 : keyCode;
    var char = String.fromCharCode(keyCode);

    if (!AllowDot) { // if you are not passing AllowDot, make it's value false
        AllowDot = false;
    }
    var expression = AllowDot == true ? (/[0-9.]/g) : (/[0-9]/g); //numeric  or numeric with '.'

    if (!expression.test(char)) {
        if ($.browser.msie) {
            event.returnValue = false;
        }
        else {

            event.preventDefault();
        }
    }
}