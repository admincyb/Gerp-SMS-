/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="GrandTreeMulti.js" />

///#region -----Global Variables-----
var storeID = 0;
//Global variable Declaration
var requisitionJson = new Object();
var tdset = "";
var materialID = 0;
var EditMode = 0;
var RefIdMode = 0; //0-no refid,1-refid mode
var Status = 1;
///#endregion

///#region -----Configuration-----
var RequisitionSlip = {
    //Url

    AutoCompleteURL: "StoreRequisitionSlip.do?Action=GetSearchValue",
    //  FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=",
    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListExceptFG&SBUPk=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetailsForStore&SBUPk=",
    GetCurrentStock: "MaterialManagement.do?Action=GetCurrentStockForStore&SBUPk=",
    FillUOMDropdownURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial&MaterialPK=",
    FillStoreDropdownURL: "SubDepartment.do?Action=GetStoresByType&SBUPk=",
    FillDepartementDropdownURL: "StoreRequisitionSlip.do?Action=GetDepartmentDtls&SBUPk=",
    GETItemNameURL: "MaterialManagement.do?Action=GetItemName&MaterialID=",
    RequisitionSaveURL: "MaterialConsumption.do?Action=SaveConsumption",
    REDIRECTURLAFTERSAVE: "../StoreManagement/MaterialConsumptionList.aspx",
    REDIRECTURLAFTERSAVEFROMINBOX: "../AccountManagement/WorkflowInbox.aspx",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategoryAndStore&SBUPk=",
    GETSRSNoURL: "StoreRequisitionSlip.do?Action=GetSRSNo",
    FillMaterialTypeDropdownURL: "CommonManagement.do?Action=GetParentDepartmentCategories&BizUnit=",
    GetMaterialCategoryExceptFGTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryNewWithoutFG&SBUPk=",
    PRINTURL: "../StoreManagement/MaterialConsumptionReport.aspx",
    MaterialSaveURL: "MaterialManagement.do?Action=SavePage&DepartPK=",
    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    RequisitionSaveMessage1: "Translate(ConsumptionDetailsSaved1)",
    RequisitionSaveMessage2: "Translate(ConsumptionDetailsSaved2)",
    RequisitionUpdateMessage2: "Translate(RequisitionDetailsUpdated2)",
    RequisitionCodeExistsMessage: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    RequisitionDeleteMessage: "Translate(ConsumptionDetailsDeletedSuccesfully)",
    RequisitionUsed: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    MaterialCategoryValidation: "Translate(SelectItemCategory)",
    RequisitionCodeAlreadyAdded: "Translate(AlreadyExists)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    NotEnoughStock: "Translate(NotEnoughStock)",
    RequestQuantityLessCurrentStock: "Translate(ConsumptionQuantityLessCurrentStock)",
    //    EnterTHREEDecimal: "Translate(EnterThreeDigitDecimal)",
    //Constants
    TextZero: "0",
    SaveCommand: "SAVE",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    DeleteMessageCommand: "DELETEMSG",
    Param: "&MatCagID=",
    //Validation messages
    MaterialCodeValidation: "Translate(SelectItem)",
    MaterialUOMValidation: "Translate(PleaseSelectUOM)",
    MaterialTypeValidation: "Translate(PleaseSelectMaterialType)",
    RequisitionQuantityValidation: "Translate(PleaseProvideQtyConsumed)",
    RequisitionCommentsValidation: "Translate(PleaseProvideComments)",
    RequisitionStoreValidation: "Translate(SelectConsumptionStore)",
    RequisitionDepartementValidation: "Translate(PleaseselectaDepartement)",
    SelectRequestDetails: "Translate(PleaseSelectConsumptionDetails)",
    MaterialNameValidation: "Translate(PleaseProvideMaterialName)",
    EnterDate: "Translate(EnterDate)",

    //Fields

    MaterialCode: "ICD_ITEM_TEXT",
    MaterialDate: "ICH_DATE",
    CurrentStock: "ICD_CURRENT_STK",
    MaterialQtyRequest: "ICD_QTY_CONSUMED",
    MaterialUOMID: "ICD_UOM",
    MaterialComments: "ICD_REMARKS",
    MaterialID: "ICD_ITEM",
    ICH_PK: "ICH_PK",
    MaterialTypePk: "ICD_ITEM_CATEGORY"

}
///#endregion

///#region------ Initialization Section ----------------

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    //Initailizing Requisition ProductGrid
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, dummyObj);
    //initialize Requisition Object
    requisitionJson = $.parseJSON($("[id$=ConsumptionDtl]").val());
    $("#divRequisitionData").data("RequisitionData", requisitionJson);
    //Create Date Picker
    GrandScriptUtils.DatePicker("ICH_DATE", false, false);
    $("[id$=ICH_DEPT]").focus();

    //Get ConsumptionID Id From the Url and Fill Details - For Edit 

    var queryStr = window.location.search.substring(1);
    //from inbox
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = queryStr.split("=");
            if ((pK[1] != "" && pK[0] == "ConsumptionID")) {
                EditMode = 1;
                FillRequisitionDetails(requisitionJson);
            }
            else if ((pK[1] != "" && pK[0] == "Status")) {
                EditMode = 1;
                if ((requisitionJson.MRH_STATUS == "1") || (requisitionJson.MRH_STATUS == "7")) {//1- submitted,7-submit more info
                    RefIdMode = 1; //checking refid has or not and setting flag to 1.if flag=1,it means it has refid and have to change dropdow attribute.
                }
                FillRequisitionDetails(requisitionJson);
            }
        }
    }
    //popup section


    $("#divAddMaterial").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");

        },
        beforeClose: function (event, ui) {
            RemovePopupValidations();
        }
    });



    PageInit();
    //Modal popup and tree view default settings
    $("#divCategory").dialog({ autoOpen: false });

});

function FillRequisitionDetails(requisitionJson) {
    ///<summary>Used to fill requisition Details for editing</summary>
    // var drpID = $("select[id$=Product]").attr("id");
    //Header Details.
    FillStore(requisitionJson.ICH_DEPT);
    FillDepartement(requisitionJson.ICH_DEPT);
    $("[id$=ICH_DATE]").val(requisitionJson.ICH_DATE)
    FillTypes(requisitionJson.ICH_ITEM_TYPE);
    $("input[id$=ICH_PK]").val(requisitionJson.ICH_PK)
    FillMaterialCategoryEdit(requisitionJson.ICH_ITEM_TYPE, requisitionJson.ICH_DEPT);
    $("input[id$=ICH_NO]").val(requisitionJson.ICH_NO);
    $("[id$=lblMaterilaConsumptionNo]").html(requisitionJson.ICH_NO);
    $("[id$=LAST_MOD_DT]").val(requisitionJson.LAST_MOD_DT);
    // Check requisitionJson.ConsumptionDtl is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign ConsumptionDtl Details to that Array, and then push Array to requisitionJson.ConsumptionDtl
    if (!($.isArray(requisitionJson.ConsumptionDtl))) {
        var objArray = requisitionJson.ConsumptionDtl;
        requisitionJson.ConsumptionDtl = new Array();
        requisitionJson.ConsumptionDtl.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, requisitionJson.ConsumptionDtl);
    // $("select[id$=ICH_DEPT]").focus();
}

function PageInit() {
    ///<summary>initial page condition</summary>

    //Reseting all input controls in the page.
    // ResetPage();

    if (EditMode == 0) {

        //Filling Store dropdown initially.      
        FillStore($("[id$=hdfDeptID]").val());
        FillDepartement(0);
        FillTypes(0);

        $("[id$=imbPrint]").hide();

    }
    //    else {
    //        $("[id$=imbPrint]").show();
    //    }



}
///#endregion

///#region---- Core Section Section----
///#region---- Fetch Data To Populate In Controls

function FillStore(SelectedValue) {
    ///<summary>to fill store combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the store DropDown //store


    var drpID = $("select[id$=ICH_DEPT]").attr("id");
    $.get(RequisitionSlip.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectedValue);
    });
    if (RefIdMode == 0) {
        $("select[id$=ICH_DEPT]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        $("select[id$=ICH_DEPT]").attr("disabled", true);
    }
}
function FillTypes(typeID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=ICH_ITEM_TYPE]").attr("id");
    $.get(RequisitionSlip.FillMaterialTypeDropdownURL + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + "Item Type", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, typeID);
    });
}
function FillDepartement(SelectedValue) {
    ///<summary>to fill Departement combo</summary>
    //<Params>SelectedValue</Params>
    // Get id of the Departement DropDown Departement
    ClearGridControlDetails();
    var userFlagText = "";
    if (RefIdMode == 0) {
        userFlagText = "1";
        $("select[id$=DeptPk]").attr("disabled", false);
    }
    else if (RefIdMode == 1) {
        userFlagText = "0";
        $("select[id$=DeptPk]").attr("disabled", true);
    }
    var drpID = "";
    $.get(RequisitionSlip.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserFlag=" + userFlagText + "&DeptType=0&DeptPk=0", function (data) {
        drpID = $("select[id$=DeptPk]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectedValue);
        if ($("select[id$=ICH_DEPT]").val() != 0) {
            $("#" + drpID + " option[value=" + $("select[id$=ICH_DEPT]").val() + "]").remove();

            if ($("select[id$=ICH_ITEM_TYPE]").val() != 0 && $("select[id$=ICH_ITEM_TYPE]").val() != null) {
                FillMaterialCategoryEdit($("select[id$=ICH_ITEM_TYPE]").val(), $("select[id$=ICH_DEPT]").val());
            }

        }
        else {
            drpID = $("select[id$=MaterialType]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true);
            drpID = $("select[id$=ITV_ITEM]").attr("id");
            GrandScriptUtils.FillDropDown(drpID, null, true, true);
            var tempData = new Array();
            AfterSave();
        }
    });
}
function FillNewCategories() {

    var ObjPurchase = new Object();
    ObjPurchase.ConsumptionDtl = new Array();
    $("#divRequisitionData").data("RequisitionData", ObjPurchase);
    AfterSave();
    ClearGridControlDetails();
    FillMaterialCategory(0);
}
function ClearGridControlDetails() {
    var drpCategoryID = $("select[id$=MaterialType]").attr("id");
    var drpUItemID = $("select[id$=ITV_ITEM]").attr("id");
    var drpUUOMID = $("select[id$=ICD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpCategoryID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUItemID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    $("[id$=CurrentStock]").html("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");


}
function FillMaterialCategory(materialID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value 
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&ItemType=" + $("select[id$=ICH_ITEM_TYPE]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val(), function (data) {

        if (materialID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

}
function FillMaterialCategoryEdit(itemType, store) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(RequisitionSlip.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&ItemType=" + itemType + "&Store=" + store, function (data) {
        if (materialID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

}
function ClearGridControlDetailsExeptMaterialType() {

    var drpUItemID = $("select[id$=ITV_ITEM]").attr("id");
    var drpUUOMID = $("select[id$=ICD_UOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpUItemID, null, true, true);
    GrandScriptUtils.FillDropDown(drpUUOMID, null, true, true);
    $("[id$=CurrentStock]").html("");
    $("[id$=ICD_QTY_CONSUMED]").val("");
    $("[id$=ICD_REMARKS]").val("");


}
function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
    ClearGridControlDetailsExeptMaterialType();
    FillCategoryMaterials(categoryID);

}

function FillCategoryMaterials(categoryID, materialID, typePK) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="categoryID"  type="object">
    ///     Specific categoryid to fill corresponding Material
    /// </param>
    /// <param name="materialID"  type="object">
    ///     Specific materialID to select the dropdown item after filling drop down
    /// </param>
    if ($("select[id$=MaterialType]").val() != 0) {

        typePK = $("select[id$=ICH_ITEM_TYPE]").val();
        storePK = $("select[id$=ICH_DEPT]").val();
        var drpID = $("select[id$=ITV_ITEM]").attr("id");
        $.getJSON(RequisitionSlip.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID + "&Type=" + typePK + "&Store=" + storePK, function (data) {
            if (materialID) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else {
        var drpID = $("select[id$=ITV_ITEM]").attr("id");
        GrandScriptUtils.FillDropDown(drpID, null, true, true);
    }
}

function FillUOM(materialID, selectval) {
    ///<summary>function To Fill Uom Details </summary>
    /// <param name="categoryID"  type="string">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    /// <param name="selectval"  type="string">
    ///     Specific value to be selected.
    /// </param>
    if (materialID != 0) {
        $("select[id$=ICD_UOM]").removeData();
        // Get id of the ICD_UOM_TEXT DropDown
        var drpID = $("select[id$=ICD_UOM]").attr("id");
        //Fill ICD_UOM_TEXT Details to the ICD_UOM_TEXT DropDown, Name as Text, PK as Value

        $.get(RequisitionSlip.FillUOMDropdownURL + materialID, function (data) {
            if (selectval) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else
        $("select[id$=ICD_UOM]").find("option").remove();
}
function BindWorkFlowComment() {
    ///<summary>To handle bind grid </summary>

    GrandScriptUtils.BindWorkFlowCommand("grdWrkfComment");
}
function FillMaterialDetails(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>

    var drpID = $("select[id$=ICD_UOM]").attr("id");
    $.get(RequisitionSlip.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=MaterialName]").html(data[0].ITM_NAME);
                //$("select[id$=ICD_UOM]").val(data[0].ITM_UOM);
                FillUOM(materialID, data[0].ITM_UOM);
            }
            else {
                $("[id$=MaterialName]").html("");
                // $("select[id$=ICD_UOM]").val("0");
                FillUOM(materialID, false);
            }

        }

    });
}
function GetCurrentStock(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>

    var drpID = $("select[id$=ICD_UOM]").attr("id");
    var store = $("select[id$=ICH_DEPT]").val();
    $.get(RequisitionSlip.GetCurrentStock + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&Store=" + store, function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=CurrentStock]").html(data[0].STD_QTY_IN_STOCK);
                //$("select[id$=ICD_UOM]").val(data[0].ITM_UOM);
                FillUOM(materialID, data[0].STD_UOM);
                $("select[id$=ICD_UOM]").removeClass();
            }
            else {
                $("[id$=CurrentStock]").html("");
                // $("select[id$=ICD_UOM]").val("0");
                $("select[id$=ICD_UOM]").removeClass();
                FillUOM(materialID, false);
            }

        }

    });
}
function GetSRSNo() {
    ///<summary>to fill SRSNo span</summary>
    $.get(RequisitionSlip.GETSRSNoURL, function (data) {
        $("[id$=ICH_NO]").val("");
        $("[id$=ICH_NO]").val(data);
        $("[id$=lblSRS]").html(data);

    });
}
function ViewRequisition(requistID) {
    window.location = vendorListing.ViewUrl + requistID;
}
function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    //SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryTreeURL + $("[id$=SBU]").val() + MaterialMaster.Param, "Root", false, false, "0", false); // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", RequisitionSlip.GetMaterialCategoryExceptFGTreeURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("select[id$=ICH_ITEM_TYPE]").val() + "&Store=" + $("select[id$=ICH_DEPT]").val() + RequisitionSlip.Param, "Root", false, false, "0", false); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view

}
///#endregion

///#region---- Data Management Section----
function ShowCategory() {
    ///<summary>Function used call the tree Data For filling the Material Category </summary>
    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Choose Category", false, "700", false, false);
    return false;
}
//function RemoveDept() {
//    var drpID = $("select[id$=DeptPk]").attr("id");
//    if ($("select[id$=ICH_DEPT]").val() != 0) {
//        $("#" + drpID + " option[value=" + $("select[id$=ICH_DEPT]").val() + "]").remove();
//    }
//    else {
//        FillDepartement(0);
//    }
//}
function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    $("select[id$=MaterialType]").val(cagID);
    //setting selected value for uom after selecting category from treeview.
    FillCategoryDetails(cagID);
    //closing modalbox after selected from treeview.
    $("#divCategory").dialog("destroy");
    $("#divCategory").dialog({ autoOpen: false });
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdRequisitionSlip") {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "ICD_REMARKS", $("#grdRequisitionSlip").attr("id"));
        $("#grdRequisitionSlip").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("");
            }
        });
        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#ProductInsert").find("tr:eq(1)");
        }
        $("#ProductInsert").hide();
        //$("#ProductInsert").css({ "display": "none", "visibility": "hidden" });
        $(tdset).insertBefore($("#grdRequisitionSlip").find("tr:eq(1)"));
        //for hiding action fields of detail section 23-11-11
        var queryStr = window.location.search.substring(1);
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");

        var isViewMode = false;
        var queryStr = window.location.search.substring(1);
        if (queryStr != "") {
            var queryStr = queryStr.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                //                if ((pK[1] == 0 && pK[0] == "Status")) {
                //                    $("[id$=imbPrint]").hide();
                //                }
                if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                    $("#grdRequisitionSlip th:last").hide();
                    $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                        $(this).find("td:last").hide();
                        $(this).find("td:last").hide();
                        //                        $("select[id$=DeptPk]").attr("disabled", true);
                        $("select[id$=ICH_DEPT]").attr("disabled", true);
                        $("[id$=ICH_DATE]").attr("disabled", true);
                        $("select[id$=ICH_ITEM_TYPE]").attr("disabled", true);
                        $("[id$=btnSave]").hide();
                        $("[id$=btnSaveandSubmit]").hide();

                    });
                }
            }

        }

        if (ObjRequisition != undefined) {
            if (ObjRequisition.MRH_STATUS == 1 || ObjRequisition.MRH_STATUS == 2 || ObjRequisition.MRH_STATUS == 7) {
                $("#grdRequisitionSlip th:last").hide();
                $("#grdRequisitionSlip tr:has(td)").each(function (index) {
                    $(this).find("td:last").hide();
                    $(this).find("td:last").hide();
                    //                    $("select[id$=DeptPk]").attr("disabled", true);
                    $("select[id$=ICH_DEPT]").attr("disabled", true);
                    $("[id$=ICH_DATE]").attr("disabled", true);
                    $("select[id$=ICH_ITEM_TYPE]").attr("disabled", true);

                });
            }
        }

    }
    $("[id$=CurrentStock]").html("");
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button])").each(function () {
        var idval = $(this).attr("id");
        if (!Checkstatus(idval)) {
            $(this).val("");
        }

    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    $(document.forms[0]).validate().resetForm();
    window.location = RequisitionSlip.REDIRECTURLAFTERSAVE;
    return false;
}
function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page

    if (controlID.search("UserPk") != -1) {
        return true;
    }
    if (controlID.search("BizUnitPk") != -1) {
        return true;
    }
    if (controlID.search("MRH_PK") != -1) {
        return true;
    }
    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
    if (controlID.search("EditRequisition") != -1) {
        return true;
    }
    //    if (controlID.search("ITM_PK") != -1) {
    //        return true;
    //    } 
    return false;
}
function PrintPage() {
    ///<summary>Function To Show PRINT Form </summary>
    window.location = RequisitionSlip.PRINTURL + "?ConsumptionID=" + $("input[id$=ICH_PK]").val();
    return false;
}
function SavePage(command) {
    ///<summary>Function used to saving   </summary>
    RemoveValidations();
    // RemovePopupValidations();
    AddValidations(2);

    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    // Check Have The ConsumptionDtl have More than or equal to one Requisition Details

    if ($(document.forms[0]).valid()) {


        if (ObjRequisition.ConsumptionDtl.length > 0) {

            //To Prevent Muliple Click
            if ($("[id$=SubmitFlag]").val() == "0")
                $("[id$=SubmitFlag]").val('1')
            else
                return false;

            //            $("select[id$=DeptPk]").attr("disabled", false);
            $("select[id$=ICH_DEPT]").attr("disabled", false);
            $("[id$=ICH_DATE]").attr("disabled", false);

            ObjRequisition = $("#divRequisitionData").data("RequisitionData");
            //Assigning the Requisition details to a hidden field by converting the object to string using Json Stringify Methord
            $("[id$=ConsumptionDtl]").val(JSON.stringify(ObjRequisition.ConsumptionDtl));
            //making json string 
            //checking command value is draft if it is draft then action id is zero means it is not calling workflow.
            if (command != "Draft") {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
                $("[id$=ICH_STATUS]").val('1');

            }
            else {
                $("[id$=ActionID]").val('0');
                $("[id$=ICH_STATUS]").val('0');
            }

            //  var jSonString = GrandScriptUtils.FormToJsonString("divXml");
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            $.post(RequisitionSlip.RequisitionSaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
                if (parseInt(data[0]) == 0) {
                    GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionCodeAlreadyAdded, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                }
                else if (parseInt(data[0]) > 0) {
                    //##### Start Change Code Here #####//
                    // If Action is Draft Save
                    if (command == "Draft") {
                        var SaveMessageWithSRSNo = "";
                        SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + data[1] + " " + RequisitionSlip.RequisitionSaveMessage2
                        GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                        PageInit();
                        AfterSave();
                    }
                    // If action - WorkFlow Save
                    else {
                        $("[id$=hdfAppID]").val(data[0]);
                        $("[id$=AppNo]").val(data[1]);
                        var SaveMessageWithSRSNo = "";
                        SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + data[1] + " " + RequisitionSlip.RequisitionSaveMessage2
                        GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
                        PageInit();
                        AfterSave();
                    }
                    //##### END Change Code Here #####//
                }
                else if (parseInt(data[0]) == -2) {
                    GrandScriptUtils.ShowModal(data[1] + " " + RequisitionSlip.EditUsedByAnotherUser, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -3) {
                    GrandScriptUtils.ShowModal(RequisitionSlip.NotEnoughStock, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SAVE);
                    $("[id$=SubmitFlag]").val('0')
                }
                else {
                    GrandScriptUtils.ShowModal(RequisitionSlip.ActionFailedMessage);
                    ResetPage();
                }

            });
        }
        else {

            GrandScriptUtils.ShowModal(RequisitionSlip.SelectRequestDetails, RequisitionSlip.MessageBoxTitle);
            RemoveValidations();
        }
    }

    return false;
}
function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>

    var SaveMessageWithSRSNo = "";
    SaveMessageWithSRSNo = RequisitionSlip.RequisitionSaveMessage1 + " " + $("[id$=AppNo]").val() + " " + RequisitionSlip.RequisitionSaveMessage2
    GrandScriptUtils.ShowModal(SaveMessageWithSRSNo, RequisitionSlip.MessageBoxTitle, RequisitionSlip.SaveCommand);
}
function AddRequisitionDetails() {
    //<summary>function used to add Evaluation details to Evaluation</summary>
    //Add Validation for Evaluation Details by setting mode as 2
    RemoveValidations();

    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
        var editRequisition = $("input[id$=EditRequisition]").val();
        var obj = new Object();
        var flag = true;

        //Loop used to check the Evaluation already added in the order List 
        if (parseInt(editRequisition) == 0) {
            for (var i in ObjRequisition.ConsumptionDtl) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == parseInt($("select[id$=ITV_ITEM]").val())) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjRequisition.ConsumptionDtl) {
                if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == parseInt($("select[id$=ITV_ITEM]").val()) && parseInt(editRequisition) != ObjRequisition.ConsumptionDtl[i].ICD_ITEM)
                    flag = false;
                break;
            }
            for (var k in ObjRequisition.ConsumptionDtl) {
                if (parseInt(editRequisition) == ObjRequisition.ConsumptionDtl[k].ICD_ITEM)
                    obj = ObjRequisition.ConsumptionDtl[k];
            }
            //            if (parseInt(editRequisition) == ObjRequisition.ConsumptionDtl[i].ICD_ITEM)
            //                obj = ObjRequisition.ConsumptionDtl[i];
        }

        if (flag) {

            obj.ICD_ITEM = parseInt($("select[id$=ITV_ITEM]").val());
            obj.ICD_PK = $("input[id$=ICD_PK]").val();
            obj.ICD_ITEM_CATEGORY_TEXT = $("select[id$=MaterialType] option:selected").text();
            obj.ICD_ITEM_CATEGORY = parseInt($("select[id$=MaterialType]").val());
            obj.ICD_ITEM_TEXT = $("select[id$=ITV_ITEM] option:selected").text();
            obj.ICD_CURRENT_STK = $("[id$=CurrentStock]").html();
            obj.ICD_QTY_CONSUMED = $("input[id$=ICD_QTY_CONSUMED]").val();
            obj.ICD_UOM_TEXT = $("select[id$=ICD_UOM] option:selected").text();
            obj.ICD_UOM = parseInt($("select[id$=ICD_UOM]").val());

            //            obj.UOMID = $("select[id$=UOMID] option:selected").val();
            obj.ICD_REMARKS = $("input[id$=ICD_REMARKS]").val() == "" ? " " : $("input[id$=ICD_REMARKS]").val();
            if (parseInt(editRequisition) == 0) {
                ObjRequisition.ConsumptionDtl.push(obj);
            }
            $("#divRequisitionData").data("RequisitionData", ObjRequisition);
            ClearProductDetails();
            ClearGridControlDetails();
            FillMaterialCategory(0);
            GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.ConsumptionDtl);
            RemoveValidations();
        }
        else {
            GrandScriptUtils.ShowModal(RequisitionSlip.RequisitionCodeAlreadyAdded, RequisitionSlip.MessageBoxTitle);
        }
        return false;
    }
}

function AfterSave() {
    //<summary>function used to clear Evaluation details from grid</summary>
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, dummyObj);
    $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
    $("#ProductInsert").show();
    // $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
}

function FillDetails(tr) {
    ///<summary>Used fill Details of requisition for editing</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    $("select[id$=ITV_ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=MaterialCode]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialCode, $(tr).parent().attr("id")));
    //$("input[id$=ICH_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialDate, $(tr).parent().attr("id")));
    $("[id$=CurrentStock]").html(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.CurrentStock, $(tr).parent().attr("id")));
    $("input[id$=ICD_QTY_CONSUMED]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialQtyRequest, $(tr).parent().attr("id")));
    $("select[id$=MaterialType]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")));
    $("input[id$=ICD_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialComments, $(tr).parent().attr("id")));
    $("input[id$=ICD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.ICD_PK, $(tr).parent().attr("id")));
    $("input[id$=EditRequisition]").val(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("input[id$=MaterialCode]").focus();
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialUOMID, $(tr).parent().attr("id")));
    FillCategoryMaterials(GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialTypePk, $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id")), $("select[id$=ICH_DEPT]").val());
}

function DeleteDetails(tr) {
    ///<summary>Used fill Details of requisition for Delete</summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var ObjRequisition = $("#divRequisitionData").data("RequisitionData");
    for (var i in ObjRequisition.ConsumptionDtl) {
        if (ObjRequisition.ConsumptionDtl[i].ICD_ITEM == materialID) {
            //Will delete the Evaluation details
            ObjRequisition.ConsumptionDtl.splice(i, 1);
            break;
        }
    }
    $("#divRequisitionData").data("RequisitionData", ObjRequisition);
    GrandGrid.MakeGrid($("#grdRequisitionSlip"), 0, ObjRequisition.ConsumptionDtl);
    //Used to Show the  Evaluation details when the requisition in requisition details is 0
    if (ObjRequisition.ConsumptionDtl.length == 0) {
        //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
        //  $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
    }



}

function ClearProductDetails() {
    //<summary>function used to Clear Requisition Product Details</summary>
    $("select[id$=MaterialType]").val("0");
    $("select[id$=ITV_ITEM]").val("");
    $("input[id$=ICD_ITEM]").val("0");

    $("input[id$=EditRequisition]").val("0");
    $("input[id$=ICD_QTY_CONSUMED]").val("");
    // $("input[id$=MRH_PK]").val("0");
    $("select[id$=ICD_UOM]").val("0");
    $("input[id$=ICD_REMARKS]").val("");
    $("select[id$=ITV_ITEM]").val("0")
    $("input[id$=MaterialCode]").focus();
}
///#endregion

///#region----Grid Handlers And Model Popup Ok Click----
function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    /// <param name="command"  type="Object">
    ///  Specific command for action       
    /// </param>
    RemoveValidations();
    switch (command.toString()) {
        case RequisitionSlip.DeleteCommand:
            // Do Confirmation.. Before Delete Details
            materialID = GrandGrid.Utilities.GetColumnValue(tr, RequisitionSlip.MaterialID, $(tr).parent().attr("id"));
            GrandScriptUtils.ShowModal(RequisitionSlip.DeleteConfirmationMessage, RequisitionSlip.ConfirmationMessage, RequisitionSlip.DeleteCommand, true);
            break;
        case RequisitionSlip.EditCommand:
            FillDetails(tr);
            return false;
            break;
        default:
            alert(RequisitionSlip.DefaultAction);
            return false;
            break;
    }
    return false;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="Object">
    ///  Specific command for action edit/save/delete       
    /// </param>
    switch (command) {
        case RequisitionSlip.SaveCommand:
            window.location = RequisitionSlip.REDIRECTURLAFTERSAVE;
            break;
        //comment req     
        case RequisitionSlip.DeleteCommand:
            DeleteDetails();
            break;
        //Commend When calling     
        case RequisitionSlip.DeleteMessageCommand:
            GrandScriptUtils.ShowModal(RequisitionSlip.DeleteConfirmationMessage, RequisitionSlip.ConfirmationMessage);
            break;
    }
    return false;
}
///#endregion
///#endregion

///#region ------ Validation----------

function AddValidations(mode) {
    //<summary>function used to assign validation</summary>

    //Mode = 1 represents the validation for request(Details) Header Details
    if (mode == "1") {
        $("select[id$=MaterialType]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialCategoryValidation }
        });
        $("select[id$=ITV_ITEM]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialCodeValidation }
        });
        //        if ($("[id$=hdnIsNeededStockValidation]").val() == "1") {
        $("input[id$=ICD_QTY_CONSUMED]").rules("add", {
            required: true,
            maxlength: 12,
            ThreeDecimal: true,
            max: $("[id$=CurrentStock]").html(),
            messages: { required: RequisitionSlip.RequisitionQuantityValidation, max: RequisitionSlip.RequestQuantityLessCurrentStock }
        });
        //        }
        $("select[id$=ICD_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialUOMValidation }
        });

        $("input[id$=ICD_REMARKS]").rules("add", {
            maxlength: 250
        });

    }
    //Mode =  2 represents the validation for request Details Store,Departement
    else if (mode == "2") {
        $("select[id$=ICH_DEPT]").rules("add", {
            selectNone: true,

            messages: { selectNone: RequisitionSlip.RequisitionStoreValidation }
        });
        $("[id$=ICH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: RequisitionSlip.EnterDate }
        });


        //        $("select[id$=DeptPk]").rules("add", {
        //            selectNone: true,

        //            messages: { selectNone: RequisitionSlip.RequisitionDepartementValidation }
        //        });
        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });
    }
    //Mode =  3 represents the popup validation for request Details Store,Departement
    else if (mode == "3") {

        $("input[id$=ITM_NAME]").rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: RequisitionSlip.MaterialNameValidation }
        });
        $("select[id$=ITC_PK]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });
        $("select[id$=UOM_PK]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialUOMValidation }
        });

    }
    //Mode =  3 represents the popup validation for request Details Store,Departement
    else if (mode == "4") {
        $("select[id$=ICH_ITEM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: RequisitionSlip.MaterialTypeValidation }
        });

    }

}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $("select[id$=MaterialType]").rules("remove");
    $("select[id$=ITV_ITEM]").rules("remove");
    $("select[id$=ICD_UOM]").rules("remove");
    $("input[id$=ICD_QTY_CONSUMED]").rules("remove");
    $("input[id$=ICD_REMARKS]").rules("remove");
    $("select[id$=ICH_DEPT]").rules("remove");
    //    $("select[id$=DeptPk]").rules("remove");
    $("select[id$=ICH_ITEM_TYPE]").rules("remove");
}
//<summary>function Remove Validation</summary>
function RemovePopupValidations() {

    $("input[id$=ITM_NAME]").rules("remove");
    $("select[id$=ITC_PK]").rules("remove");
    $("select[id$=UOM_PK]").rules("remove");
    //  $("input[id$=ITM_DESC]").rules("remove");



}
///#endregion 
