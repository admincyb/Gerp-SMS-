/// <reference path="../../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../../../GrandScriptUtils.js" />
/// <reference path="../../../GrandTreeMulti.js" />

//#region------ Configuration Section-------
var StoreMaterialMapping = {
    RootName: "Root",
    AutoCompleteURL: "StoreMaterialMapping.do?Action=GetSearchValue&SBUPk=",
    StoreMaterialMappingListURL: "StoreMaterialMapping.do?Action=GetMaterialMappingList&MenuParentID=",
    SaveStoreMaterialMappingURL: "StoreMaterialMapping.do?Action=SaveMaterialMappingDetails",
    GetStoreMaterialMappingTreeURL: "StoreMaterialMapping.do?Action=GetMaterialMappingDetails&SBUPk=",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillMaterialCategoryURL: "MaterialManagement.do?Action=GetMaterialCategoryByLevel&SBUPk=",
    BindGridURL: "StoreMaterialMapping.do?Action=GetMaterialMappingList&Status=",
    FillPackingType: "CommonManagement.do?Action=GetPackingType&BizUnit=",
    GetCategoryValue: "CommonManagement.do?Action=GetCategoryValue&CategoryPK=",

    //SBUDTLSURL: "SBUConfiguration.do?Action=GetAllSBUList",

    SELECTONEMSG: "Translate(Pleaseselectanoption)",
   
    //SELECTSBU: "Translate(SelectSBU)",
    INFORMATIONTITLE: "Translate(Information)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    SAVESUCCESS: "Translate(StoreMappingDetailsSavedSuccessfully)",

    ReferencedItems: "Translate(ReferencedItems)",
    SaveCommand: "SAVE",
   
    EditCommand: "EDIT",
   
    StoreParam: "&StoreID=",
    MapParam: "&MapParentID=",
    Type: "&Type=",
    SELECTONE: "selectNone",
    TEXTZERO: "0",

    //constants

    StoreID: "IDM_DEPT",

    StoreValidation: "Translate(PleaseselectaStore)"
}
//#endregion

///#region------- Initialization Section --------
//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != TankMaster.TextZero);
}, 'Translate(Pleaseselectanoption)');

$(document).ready(function () {
    
    //Page Initial condtions
    PageInit();
  

});

function PageInit() {
    ///<summary>Initial page condition</summary>
    //Reseting all input controls in the page
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("select[id$=SearchType]").val("0");
    $("[id$=SearchValue]").val("");
    FillItemCategory();
    SearchInit();
    SetSearchType();
    FillStore();
    $("[id$=Store]").focus();
    BindGrid();
    FillPackingType(0);
    return false;
}

function FillItemCategory() {
    var drpID = $("[id$=ITM_CATEGORY]").attr("id");
    $.get(StoreMaterialMapping.FillMaterialCategoryURL + $("[id$=BizUnitPk]").val() + "&Level=1", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false);
        FillMenuTreeView(0);
    });
}
///#region---- Set Or Reset Form----

function AddNew() {
    $("[id$=btnSave]").show();
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    return false;
}

function FillPackingType(itemPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=IPD_TYPE]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(StoreMaterialMapping.FillPackingType + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, itemPK);
//        setPackingVisibility();
    });
}


//<summary>function Used to Reset Page</summary>
function ResetPage() {
    //<summary>Function Used toReset Page</summary>
//    $(document.forms[0]).find("input").each(function () {
//        var idval = $(this).attr("id");
//        if (idval.search("UserPk") == -1)
//            $(this).val("");
//    });
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    FillMenuTreeView(0);
    BindGrid();
    SetSearchType();
  //  $("[id$=Store]").focus();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
//    var strname = $("select[id$=SearchType]").val();
//    $("[id$=SearchValue]").val("");
//    if (strname == "0") {
//        $("[id$=SearchValue]").hide()
//        $("[id$=imbSearch]").hide();
//        BindGrid();
//    }
//    else {
//        $("[id$=SearchValue]").show()
//        $("[id$=imbSearch]").show();
//    }
    $("[id$=SearchValue]").show()
    $("[id$=imbSearch]").show();
    BindGrid();
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", StoreMaterialMapping.AutoCompleteURL + $("[id$=BizUnitPk]").val(), "SearchType");
}


///#endregion

///#endregion

///#region------ Core Section ----------------

///#region---- Fetch Data To Populate In Controls
function FillStore(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store
    var drpID = $("select[id$=Store]").attr("id");
    $.get(StoreMaterialMapping.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
    });
}


function FillSBUCombo() {
    //<summary>Function Used to fill all sbu</summary>

    var drpID = $("[id$=SBU]").attr("id"); // Get id of the SBU DropDown
    $.get(MenuGroupSetting.SBUDTLSURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillMenuTreeView(grpPk) {
    //<summary>Function Used to Fill Menu Details to Tree View </summary>
    //var sbuPK = 1;
    var sbuPK = $("[id$=BizUnitPk]").val();
    var storePk = 0;
    var mapParentID = 0;
    var type = 0;
    if ($("select[id$=Store]").val() == null) {
        storePk = 0;
    }
    else {
        storePk = $("select[id$=Store]").val();
    }
    mapParentID = $("select[id$=ITM_CATEGORY]").val();

    $.get(StoreMaterialMapping.GetCategoryValue + mapParentID + "&Active=1", function (data) {
        if (data) {
            if (data[0].ITC_VALUE == 3) {
                $("[id$=divMaterialType]").show();
                type = $("select[id$=IPD_TYPE]").val();
                $("#updateProgress").show();
                SetTreeHeaderStructure("trvMaterialMap", StoreMaterialMapping.GetStoreMaterialMappingTreeURL + sbuPK + StoreMaterialMapping.StoreParam + storePk + StoreMaterialMapping.Type + type + StoreMaterialMapping.MapParam, StoreMaterialMapping.RootName, true, false, mapParentID, true, "&pVal=0");    // set the tree view parameters
                MakeMultiTree(); 
            }
            else {
                $("[id$=divMaterialType]").hide();
                type = 0;
                $("#updateProgress").show();
                SetTreeHeaderStructure("trvMaterialMap", StoreMaterialMapping.GetStoreMaterialMappingTreeURL + sbuPK + StoreMaterialMapping.StoreParam + storePk + StoreMaterialMapping.Type + type + StoreMaterialMapping.MapParam, StoreMaterialMapping.RootName, true, false, mapParentID, true, "&pVal=0");    // set the tree view parameters
                MakeMultiTree(); 
            }
        }

    });

    //SetTreeHeaderStructure("trvMaterialMap", StoreMaterialMapping.GetStoreMaterialMappingTreeURL + sbuPK + StoreMaterialMapping.StoreParam + storePk + StoreMaterialMapping.MapParam, StoreMaterialMapping.RootName, true, false, "0", true);    // set the tree view parameters
 // call the function to bind tree view
}

function FillPackingMaterialTree() {
    var sbuPK = $("[id$=BizUnitPk]").val();
    var storePk = 0;
    var mapParentID = 0;
    var type = 0;
    if ($("select[id$=Store]").val() == null) {
        storePk = 0;
    }
    else {
        storePk = $("select[id$=Store]").val();
    }
    mapParentID = $("select[id$=ITM_CATEGORY]").val();
    type = $("select[id$=IPD_TYPE]").val();
    $("#updateProgress").show();
    SetTreeHeaderStructure("trvMaterialMap", StoreMaterialMapping.GetStoreMaterialMappingTreeURL + sbuPK + StoreMaterialMapping.StoreParam + storePk + StoreMaterialMapping.Type + type + StoreMaterialMapping.MapParam, StoreMaterialMapping.RootName, true, false, mapParentID, true, "&pVal=0");    // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view

}
///#endregion

///#region---- Data Management Section----
function SavePage() {
    //<summary>Function Used to save all dept user group </summary>
    AddValidations();
    if ($(document.forms[0]).valid()) {
         $("[id$=STORE_MATERIAL_MAPPING_LIST]").val(JSON.stringify(GetSelectedMaterial()));
        var jSonString = GrandScriptUtils.FormToJsonString();
        $.post(StoreMaterialMapping.SaveStoreMaterialMappingURL, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(StoreMaterialMapping.SAVESUCCESS, StoreMaterialMapping.INFORMATIONTITLE);
                ResetPage();
                RemoveValidations();
            }
            else if (parseInt(result) == -3) {
                GrandScriptUtils.ShowModal(StoreMaterialMapping.ReferencedItems, StoreMaterialMapping.INFORMATIONTITLE);
                FillMenuTreeView(0);
                RemoveValidations();
            }
            else {
                GrandScriptUtils.ShowModal(StoreMaterialMapping.ACTIONFAILEDMSG, StoreMaterialMapping.INFORMATIONTITLE);
                ResetPage();
                RemoveValidations();
            }
        });
    }
    return false;
}

function GetSelectedMaterial() {
    //<summary>Function Used to get the all checked dept details </summary>
    var MaterialArray = new Array();
    var sbuPK = $("[id$=BizUnitPk]").val();
    var store = $("[id$=Store]").val();
    var userPK = $("[id$=UserPk]").val();
    var categoryID = $("select[id$=ITM_CATEGORY]").val();
    $("[id$=IDM_DEPT]").val(store);
    $("[id$=IDM_MOD_BY]").val(userPK);
    var materialMapping
    var materialPK = 0;
    $("#trvMaterialMap").find("input[type=checkbox]:checked").each(function () {
        materialPK = $(this).attr("id");
        if ($(this).next().next("input[type=hidden]").val() == "true") {
            materialPK = materialPK.substr(materialPK.lastIndexOf("_") + 1, materialPK.length);
            MaterialArray.push({ ITM_CATEGORY: categoryID, IDM_ITEM: materialPK, IDM_DEPT: store, IDM_MOD_BY: userPK, IDM_BIZUNIT: sbuPK });
        }
    });
    return MaterialArray;
}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";


    var ajaxUrl = StoreMaterialMapping.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&SBUPk=" + $("[id$=BizUnitPk]").val();
    $("#grdStoreList").removeAttr("ajaxurl")
    $("#grdStoreList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdStoreList");
    GrandGrid.MakeGrid($("#grdStoreList"));
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
    // RemoveValidations();
    switch (command.toString()) {
       
        
        // To Edit Details              
        case StoreMaterialMapping.EditCommand:
            FillDetails(tr);
            break;
        // Default Handler    
        default:
            alert(TankMaster.DefaultAction);
            break;
    }
    return false;

}
function FillDetails(tr) {

    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param>

    var grdID = $(tr).parents("table:first").attr("id");
//    $("input[id$=StoreMaterialMappingPK]").val(GrandGrid.Utilities.GetColumnValue(tr, StoreMaterialMapping.StoreMaterialMappingID, grdID));
    var storeID = GrandGrid.Utilities.GetColumnValue(tr, StoreMaterialMapping.StoreID, grdID)
    $("select[id$=Store]").val(storeID);

    FillMenuTreeView(storeID);
    //changing mode to  lising
    AddNew();

    //    Filling UOM(CategoryID,SelctVal)
    //    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID), GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialMOU, grdID))
    //    FillVendorUOM(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialCategory, grdID));
    //    FillVendorMappingXmlDetails(GrandGrid.Utilities.GetColumnValue(tr, MaterialMaster.MaterialDetailId, grdID))



}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {

    switch (command) {

//        case TankMaster.DELETE:
//            DeleteDetails();
//            break;

    }
    return false;
}
///#endregion


///#endregion
///#endregion

///#region------ Validations ----------------

function AddValidations() {
    //<summary>Function Used to Set Validation to Controls in page</summary>
    $("[id$=Store]").rules("add", {
        selectNone: true,
        messages: { selectNone: StoreMaterialMapping.StoreValidation }
    });
//    $("[id$=SBU]").rules("add", {
//        selectNone: true,
//        messages: { selectNone: MenuGroupSetting.SELECTSBU }
//    });
}
function RemoveValidations() {
    //<summary>Function Used to Remove Validation to Controls in page</summary>
    $("select[id$=Store]").rules("remove");
//    $("select[id$=SBU]").rules("remove");
}
///#endregion