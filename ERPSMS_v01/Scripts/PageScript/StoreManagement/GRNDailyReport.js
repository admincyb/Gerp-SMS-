

$(document).ready(function () {
    ///<summary>
    ///Document Ready function Aftre page initialization
    ///</summary>
                        //    $(document.forms[0]).validate({
                        //        onclick: false,
                        //        onkeyup: false,
                        //        focusInvalid: false
                        //    });
    //Page Initial condtions
    PageInit();


});

function PageInit() {
    ///<summary>
    ///Used For PageInit
    ///</summary>
    //WindowExpand(true);
    $("#divCatgDtls").dialog({ autoOpen: false });
    //FillMaterialCategory
    //FillMaterialCategory();
    DateTimeInit();
    //FillStore();
    return false;
}


function AddItemCategoryDetails() {
    ///<summary>Function used call the tree Data For filling the Material Category </summary>
    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCatgDtls", "Category", false, "700", false, false);
    return false;
}

function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    //    SetTreeHeaderStructure("trvCategory", "MaterialCategory.do?Action=GetMaterialCategoryDtls" +"&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MatCagID=", "Root", false, false, "0", false);  // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", "MaterialCategory.do?Action=GetMaterialCategoryDtls" + "&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MatCagID=", "Root", false, false, "0", false);  // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view

}


function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    $("select[id$=ddlMaterialCatg]").val(cagID);
    //closing modalbox after selected from treeview.
    $("#divCatgDtls").dialog("destroy");
    $("#divCatgDtls").dialog({ autoOpen: false });
}


//function FillMaterialCategory() {
//    //<summary>function To Fill Category Details </summary>
//    // Get id of the Category DropDown
//    //<Params>materialID</Params>
//    var drpID = $("select[id$=Category]").attr("id");
//    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
//    $.get("MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=" + $("[id$=BizUnitPk]").val(), function (data) {
//        GrandScriptUtils.FillDropDown(drpID, data, true, false);
//    });
//}

function DateTimeInit() {
    //<summary>Function used to Init Date and Time Extender to the control</summary>
    GrandScriptUtils.DatePicker("GRNDate", false, false);
    if ($("[id$=hdfDateGRN]").val() != "") {
        $("input[id$=GRNDate]").val($("[id$=hdfDateGRN]").val());
    }
}

//function FillStore() {
//    ///<summary>to fill store combo</summary>
//    var drpID = $("select[id$=Store]").attr("id");
//    $.get("StoreRequisitionSlip.do?Action=GetStores&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Flag=1", function (data) {
//        GrandScriptUtils.FillDropDown(drpID, data, true, false,0, true);
//    });
//}