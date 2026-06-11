/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../GrandScriptUtils.js" />


$(document).ready(function () {
    SearchInit();
    SetSearchType();
    BindGrid();
    $("[id$=SearchType]").change(function () {
        SetSearchType();
    });
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });
});

var orderID = 0;

///#region------ Initialization Section ----------------

///<summary>To handle auto complete</summary>
function SearchInit() {
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", "OrderManagement.do?Action=GetSearchValue", "SearchType");
}

///#endregion

///#region ----- Utility Methods ------------------

///<summary>To handle bind grid corr. to the search type and search value</summary>
function BindGrid() {
    var ajaxUrl = "OrderManagement.do?Action=GetOrderList&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();
    $("#grdOrderDetails").removeAttr("ajaxurl")
    $("#grdOrderDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdOrderDetails");
    GrandGrid.MakeGrid($("#grdOrderDetails"));
}
///<summary>Function To Enable/Disable Selected Option For Search </summary>
function SetSearchType() {
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("#searchcont").hide();
//        $("[id$=SearchValue]").hide()
//        $("[id$=imbSearch]").hide();
    }
    else {
        $("#searchcont").show();
//        $("[id$=SearchValue]").show()
//        $("[id$=imbSearch]").show();
    }
}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {
    switch (command) {
        case "delete":
            BindGrid();
            break;
        case "deletemsg":
            DeleteDetails();
            break;
    }
    return false;
}

///<summary>Grid Handler Catch all the grid events in this function </summary>
function GridHandler(tr, command) {
    switch (command.toString().toLowerCase()) {
        // To Delete Details 
        case "delete":
            orderID = GrandGrid.Utilities.GetColumnValue(tr, "Rmdp", $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal('Translate(Doyouwanttodeletethisdetails)', 'Translate(Conformation)', "deletemsg", true);
            break;
        // To Edit Details          
        case "edit":
            FillDetails(tr);
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            break;
    }
    return false;

}
///<summary>Function To Get OrderPK and Pass this OrderPK as a QueryString </summary>
function FillDetails(tr) {
    //Get OrderID From tr - For Pass this as QueryString
    orderID = GrandGrid.Utilities.GetColumnValue(tr, "Rmdp", $(tr).parent().attr("id"));
    window.location = "../OrderManagement/OrderCreation.aspx?OrderID=" + orderID;
}
///<summary>Function To Redirect Order Details Entry page </summary>
function AddNew() {
    //Set Redirecting Url for Enter New Details
    window.location = "../OrderManagement/OrderCreation.aspx";
    return false;
}
///<summary>Function To Get OrderPK and Delete OrderDetails, And Finally, Fill Remaining Data</summary>
function DeleteDetails(tr) {
    //Get OrderID From tr
    var msgtxt;
    $.ajax
        ({
            type: "post",
            url: "OrderManagement.do?Action=DeleteOrder&OrderID=" + orderID,
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (data) {
                //Check Order Deleted Succesfully or Not - 1-Sucess 0-Fail
                if (parseInt(data) == 1)
                    msgtxt = 'Translate(OrderDeletedSuccessfully)';
                else
                    msgtxt = 'Translate(ActionFailedPleaseTryAgain)';
                // Show MeesageBox For Order Delete Status
                GrandScriptUtils.ShowModal(msgtxt, 'Translate(Information)', "delete");
            }
        });
    return false;
}
//<summary>function Used to Reset Page</summary>
function ResetPage() {
    //Reseting all input controls in the page
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        //Avoid UserPk to get the value of log in user
        if (idval.search("UserPk") == -1) {
            $(this).val("");
        }
    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    SetSearchType();
    BindGrid();
    return false;
}

///#endregion
