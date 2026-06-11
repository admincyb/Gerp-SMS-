$(document).ready(function () {
   
    var ajaxUrl = "MaterialManagement.do?Action=GetMaterialList&Status=" + 0 + "&SearchValue=" + "";
    //    $("#grdCategory").attr("ajaxurl", "MaterialManagement.do?Action=GetMaterialList");
    $("#grdCategory").attr("ajaxurl", ajaxUrl);
//    GrandGrid.Utilities.ResetGrid(true, "grdCategory");
    GrandGrid.MakeGrid($("#grdCategory"));
    return false;
});
function AfterGridBind() {
    GrandScriptUtils.ChangeMode("grdTable-wrap");
}
