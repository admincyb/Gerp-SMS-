/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../jquery/json2.js" />
/// <reference path="../../GrandScriptUtils.js" />

//Global variable Declaration
var orderJson = new Object();
var tdset = "";

///<summary>On Ready function</summary>
///<summary>For Adding rule to Select</summary>

///#region------ Initialization Section ----------------

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false,
        errorElement: "div"


    });
    GrandScriptUtils.AddDateRange("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, true);
    GrandScriptUtils.DatePicker("TestDate", false, true);
    //For Adding rule to Select
    $.validator.addMethod('selectNone', function (value, element) {
        return ($(element).val() != "0");
    }, 'Translate(Pleaseselectanoption)');
    //Initailizing Order ProductGrid
    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdOrderDetails"), 0, dummyObj);

    // Create customer add dialog
    $("#divCustomer").dialog({ autoOpen: false });
    // Create Tabs
    $("[id$=tabs]").tabs();
    // Intialize Date to the Controls
    DateInit();
    //Fill Products Details
    FillProduct();
    // Fill Customer Details
    FillCustomer(0);
    //initialize Order Object
    orderJson = $.parseJSON($("[id$=OrderDetailsList]").val());
    $("#divData").data("OrderData", orderJson);
    //Get Order Id From the Url and Fill Details - For Edit 

    GrandScriptUtils.MakeFileUploader("fupUploader");
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = queryStr.split("=");
            if (pK[1] != "" && pK[0] == "OrderID") {
                FillOrderDetails(orderJson);
            }
        }

    }
});

///<summary>Used to fill Order Details for editing</summary>
function FillOrderDetails(orderJson) {
    var drpID = $("select[id$=Product]").attr("id");
    $("input[id$=OrderCode]").val(orderJson.OrderCode);
    $("input[id$=OrderDate]").val(orderJson.OrderDate);
    $("input[id$=FinalDate]").val(orderJson.FinalDate);
    $("input[id$=ProductionEndDate]").val(orderJson.ProductionEndDate);
    $("select[id$=OrderStatus]").val(orderJson.OrderStatus);
    $("select[id$=CustomerID]").val(orderJson.CustomerID);
    $("textarea[id$=Description]").val(orderJson.Description);
    $("input[id$=OrderID]").val(orderJson.OrderID);
    FillCustomer(orderJson.CustomerID);
    // Check orderJson.OrderDetailsList is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign OrderList Details to that Array, and then push Array to orderJson.OrderDetailsList
    if (!($.isArray(orderJson.OrderDetailsList))) {
        var objArray = orderJson.OrderDetailsList;
        orderJson.OrderDetailsList = new Array();
        orderJson.OrderDetailsList.push(objArray);
    }
    FillCustomer(orderJson.CustomerID);
    GrandGrid.MakeGrid($("#grdOrderDetails"), 0, orderJson.OrderDetailsList);
}

///<summary>to fill item combo</summary>
function FillProduct() {
    // Get id of the Product DropDown
    var drpID = $("select[id$=Product]").attr("id");
    $.get("OrderManagement.do?Action=GetProducts", function (data) {
        GrandScriptUtils.FillDropDown(drpID,data,true,true);
    });
}
///<summary>Set Date Format And Set Today Date as Default</summary>
function DateInit() {
    $("[id$=OrderDate]").datepicker({
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("[id$=FinalDate]").datepicker("option", "minDate", new Date($("[id$=hdnFrmDate]").val()));
            $("[id$=ProductionEndDate]").datepicker("option", "minDate", new Date($("[id$=hdnFrmDate]").val()));
        },
        altField: $("[id$=hdnFrmDate]"),
        altFormat: "mm/dd/yy",
        defaultDate: GrandScriptUtils.FillDate("OrderDate", "hdnFrmDate")
    });
    $("[id$=FinalDate]").datepicker({
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("[id$=ProductionEndDate]").datepicker("option", "maxDate", new Date($("[id$=hdnToDate]").val()));
        },
        altField: $("[id$=hdnToDate]"),
        altFormat: "mm/dd/yy"
    });
    $("[id$=ProductionEndDate]").datepicker({
        dateFormat: 'dd-M-yy',
        changeMonth: true,
        changeYear: true,
        altField: $("[id$=hdnEndDate]"),
        altFormat: "mm/dd/yy"
    });
    $("[id$=FinalDate]").datepicker("option", "minDate", new Date());
    $("[id$=ProductionEndDate]").datepicker("option", "minDate", new Date());
}

///<summary>Function To Redirect Order Listing  page </summary>
function ShowAllOrderDtls() {
    window.location = "../OrderManagement/OrderListing.aspx";
    return false;
}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {
    switch (command) {
        case "save":
            window.location = "../OrderManagement/OrderListing.aspx";
            break;
        case "customer":
            SaveCustomer();
            break;

        case "savecustomer":
            //If Customer Name Not Saved , Show the Reason, in MessageBox
            AddCustomerName();
            $('input[id$=CustomerName]').focus();
            break;
          }    
    return false;
}
///#endregion

//#region-----------ValidationSection----------------
//<summary>function used to assign validation</summary>
function AddValidations(mode) {
    RemoveValidations();
    //Mode = 1 represents the validation for Order(Master) Header Details
    if (mode == "1") {
        $('input[id$=OrderCode]').rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: 'Translate(PleaseProvideOrderCode)' }
        });
        $('input[id$=OrderDate]').rules("add", {
            required: true,
            maxlength: 50,
            messages: { required: 'Translate(PleaseProvideOrderDate)' }
        });
        $('input[id$=FinalDate]').rules("add", {
            required: true,
            messages: { required: 'Translate(PleaseProvideDesiredFinalDate)' }
        });
        $('input[id$=ProductionEndDate]').rules("add", {
            required: true,
            messages: { required: 'Translate(PleaseProvideProductionEndDate)' }
        });
        $('select[id$=OrderStatus]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectOrderStatus)' }
        });
        $('select[id$=CustomerID]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaCustomer)' }
        });
    }
    //Mode =  2 represents the validation for Order Details
    else if (mode == "2") {
        $('select[id$=Product]').rules("add", {
            selectNone: true,
             
            messages: { selectNone: 'Translate(PleaseselectaProduct)' }
        });
        $('input[id$=Quantity]').rules("add", {
            required: true,
            digits: true,
            messages: { required: 'Translate(PleaseProvideQuantity)' }
        });
        $('input[id$=AvgWgtPerPiece]').rules("add", {
            required: true,
            number: true,
            messages: { required: 'Translate(PleaseProvideAverageWeight)' }
        });
        $('input[id$=MinLength]').rules("add", {
            number: true
        });

        $('input[id$=CuffThickness]').rules("add", {
            number: true
        });
        $('input[id$=PalmThickness]').rules("add", {
            number: true
        });
        $('input[id$=FingerThickness]').rules("add", {

            number: true
        });
    }
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $('select[id$=Product]').rules("remove");
    $('input[id$=Quantity]').rules("remove");
    $('input[id$=AvgWgtPerPiece]').rules("remove");
    $('input[id$=MinLength]').rules("remove");
    $('input[id$=CuffThickness]').rules("remove");
    $('input[id$=PalmThickness]').rules("remove");
    $('input[id$=FingerThickness]').rules("remove");
    $('input[id$=OrderCode]').rules("remove");
    $('input[id$=OrderDate]').rules("remove");
    $('input[id$=FinalDate]').rules("remove");
    $('input[id$=ProductionEndDate]').rules("remove");
    $('select[id$=OrderStatus]').rules("remove");
    $('select[id$=CustomerID]').rules("remove");
}

//#endregion


///#region------ Customer Section ----------------

//<summary>function To Fill Customer Details </summary>
function FillCustomer(customerID) {
   // Get id of the Customer DropDown
    var drpID = $("select[id$=CustomerID]").attr("id");
    //Fill Customer Details to the Customer DropDown, Name as Text, PK as Value
    $.get("OrderManagement.do?Action=GetCustomer", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data,true, true, customerID);
    });
}

//<summary>function To Save Customer Details </summary>
function SaveCustomer() {

    var custName = $('input[id$=CustomerName]').val();
    // check customer name is Empty or not- For Validations
    if (custName == "") {
        $("[id$=lblstar]").show();
        AddCustomerName();
        return false;
    }
    else {
        var msgTxt;
        var custName = $('input[id$=CustomerName]').val();
        // TO Pass Customer Name to handler
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.ajax({
            type: "post",
            url: "OrderManagement.do?Action=SaveCustomer&CustName=" + custName,
            data: jSonString,
            contentType: "application/json",
            dataType: "text",
            success: function (data) {
                // Check Customer Name Saved Successfully or Not - >0 Success ,0- Name Already Exists, <0 - Fail(Exception)
                if (parseInt(data) > 0)
                    msgTxt = 'Translate(CustomerNameAddedSuccesfully)';
                else if (parseInt(data) == 0)
                    msgTxt = 'Translate(CustomerNameAlreadyExists)';
                else if (parseInt(data) < 0)
                    msgTxt = 'Translate(ActionFailedPleaseTryAgain)';
                // Check The Customer Name Saved Succesfully or Not
                if (parseInt(data) > 0) {
                    // Hide * , if Customer Name Saved Succesfully
                    $("[id$=lblstar]").hide();
                    $(this).dialog("close");
                    // After Save Customer Details , Fill All Customer Details  to Customer DropDown
                    FillCustomer(0);
                    // Clear CustomerName Entry TextBox
                    $('input[id$=CustomerName]').val('');
                   return false;
                }
                else {
                    
                    GrandScriptUtils.ShowModal(msgTxt, 'Translate(Information)', "savecustomer");                    
                }
            }
        });
        return false;
    }
}

//<summary>function Show Customer details adding div</summary>

function AddCustomerName() {
    RemoveValidations();
    GrandScriptUtils.ShowModalID("divCustomer", "Customer", "customer");
    return false;
}

///#endregion

///#region------ Product Section ----------------

///<summary>Grid Handler Catch all the grid events in this function </summary>
function GridHandler(tr, command) {
    RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            DeleteDetails(tr);
            return false;
            break;
        case "edit":
            FillDetails(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}
///<summary>For delete the item in the grid - Order Details</summary>
function DeleteDetails(tr) {
    var productID = GrandGrid.Utilities.GetColumnValue(tr, "ProductID", $(tr).parent().attr("id"));
    var ObjOrder = $("#divData").data("OrderData");
    for (var i in ObjOrder.OrderDetailsList) {
        if (ObjOrder.OrderDetailsList[i].ProductID == productID) {
            //Will delete the Product details
            ObjOrder.OrderDetailsList.splice(i, 1);
            break;
        }
    }
    $("#divData").data("OrderData", ObjOrder);
    GrandGrid.MakeGrid($("#grdOrderDetails"), 0, ObjOrder.OrderDetailsList);
    //Used to Show the Product Order details when the products in order is 0
    if (ObjOrder.OrderDetailsList.length == 0) {
        //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
    }
}
///<summary>Used fill Details of order Product for editing</summary>
function FillDetails(tr) {

    $("select[id$=Product]").val(GrandGrid.Utilities.GetColumnValue(tr, "ProductID", $(tr).parent().attr("id")));
    $("input[id$=ProductQuantity]").val(GrandGrid.Utilities.GetColumnValue(tr, "ProductQuantity", $(tr).parent().attr("id")));
    $("input[id$=MinLength]").val(GrandGrid.Utilities.GetColumnValue(tr, "MinLength", $(tr).parent().attr("id")));
    $("input[id$=CuffThickness]").val(GrandGrid.Utilities.GetColumnValue(tr, "CuffThickness", $(tr).parent().attr("id")));
    $("input[id$=PalmThickness]").val(GrandGrid.Utilities.GetColumnValue(tr, "PalmThickness", $(tr).parent().attr("id")));
    $("input[id$=FingerThickness]").val(GrandGrid.Utilities.GetColumnValue(tr, "FingerThickness", $(tr).parent().attr("id")));
    $("input[id$=AvgWgtPerPiece]").val(GrandGrid.Utilities.GetColumnValue(tr, "AvgWgtPerPiece", $(tr).parent().attr("id")));
    $("input[id$=OrderDetailID]").val(GrandGrid.Utilities.GetColumnValue(tr, "OrderDetailID", $(tr).parent().attr("id")));
    $("input[id$=EditProduct]").val(GrandGrid.Utilities.GetColumnValue(tr, "ProductID", $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("select[id$=Product]").focus();

}
//<summary>function used to add products details to order</summary>
function AddOrderProducts() {
    //Add Validation for Product Details by setting mode as 2
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        var ObjOrder = $("#divData").data("OrderData");
        var editProduct = $("input[id$=EditProduct]").val();
        var obj = new Object();
        var flag = true;
        //OrderDetailID ProductID Product ProductQuantity MinLength CuffThickness PalmThickness  FingerThickness  AvgWgtPerPiece
        //Loop used to check the product already added in the order List
        if (parseInt(editProduct) == 0) {
            for (var i in ObjOrder.OrderDetailsList) {
                if (ObjOrder.OrderDetailsList[i].ProductID == $("select[id$=Product]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjOrder.OrderDetailsList) {
                if (ObjOrder.OrderDetailsList[i].ProductID == $("select[id$=Product]").val() && parseInt(editProduct) != ObjOrder.OrderDetailsList[i].ProductID) {
                    flag = false;
                    break;
                }
                if (parseInt(editProduct) == ObjOrder.OrderDetailsList[i].ProductID)
                    obj = ObjOrder.OrderDetailsList[i];
            }
        }
        if (flag) {
            obj.ProductID = parseInt($("select[id$=Product]").val());
            obj.Product = $("select[id$=Product] option:selected").text();
            obj.ProductQuantity = parseInt($("input[id$=ProductQuantity]").val());
            obj.MinLength = $("input[id$=MinLength]").val().length > 0 ? parseFloat($("input[id$=MinLength]").val()) : null;
            obj.CuffThickness = $("input[id$=CuffThickness]").val().length > 0 ? parseFloat($("input[id$=CuffThickness]").val()) : null;
            obj.PalmThickness = $("input[id$=PalmThickness]").val().length > 0 ? parseFloat($("input[id$=PalmThickness]").val()) : null;
            obj.FingerThickness = $("input[id$=FingerThickness]").val().length > 0 ? parseFloat($("input[id$=FingerThickness]").val()) : null;
            obj.AvgWgtPerPiece = parseFloat($("input[id$=AvgWgtPerPiece]").val());
            if (parseInt(editProduct) == 0) {
                obj.OrderDetailID = 0;
                ObjOrder.OrderDetailsList.push(obj);
            }
            $("#divData").data("OrderData", ObjOrder);
            GrandGrid.MakeGrid($("#grdOrderDetails"), 0, ObjOrder.OrderDetailsList);
            ClearProductDetails();
        }
        else {
            GrandScriptUtils.ShowModal('Translate(ProductAlreadyAdded)', 'Translate(Information)');
        }
        return false;
    }
}
//<summary>function used to Clear Product Details</summary>
function ClearProductDetails() {
    $("select[id$=Product]").val('0');
    $("input[id$=EditProduct]").val('0');
    $("input[id$=OrderDetailID]").val('0');
    $("input[id$=ProductQuantity]").val('');
    $("input[id$=MinLength]").val('');
    $("input[id$=CuffThickness]").val('');
    $("input[id$=PalmThickness]").val('');
    $("input[id$=FingerThickness]").val('');
    $("input[id$=AvgWgtPerPiece]").val('')
    $("select[id$=Product]").focus();
}
//<summary>function used to Save Order details</summary>
function SavePage() {
    //Add Validation for Order Details by setting mode as 1
    AddValidations(1);
    var ObjOrder = $("#divData").data("OrderData");
    // Check Have The Order List have More than or equal to one Product Details
    if (ObjOrder.OrderDetailsList.length > 0) {
        if ($(document.forms[0]).valid()) {
            var ObjOrder = $("#divData").data("OrderData");
            //Assginging the Product details to a hidden field by converting the object to string using Json Stringify Methord
            $("[id$=OrderDetailsList]").val(JSON.stringify(ObjOrder.OrderDetailsList));
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            $.ajax({
                type: "post",
                url: "OrderManagement.do?Action=SavePage",
                data: jSonString,
                contentType: "application/json",
                dataType: "text",
                success: function (data) {

                    if (parseInt(data) > 0) {
                        GrandScriptUtils.ShowModal('Translate(OrderDetailssavedsuccessfully)', 'Translate(Information)', "save");
                    }
                    else {
                        var msgtxt;
                        if (parseInt(data) == 0)
                            msgtxt = 'Translate(OrderCodealreadyexists)';
                        else if (parseInt(data) < 0)
                            msgtxt = 'Translate(ActionFailed)';
                        GrandScriptUtils.ShowModal(msgtxt, 'Translate(Status)', "failed");
                    }
                }
            });

        }

    }
    else {
        GrandScriptUtils.ShowModal('Translate(PleaseSelectProductDetails)', 'Translate(Information)');
    }
    return false;
}
//<summary>function Call Afer binding Grid</summary>
function AfterGridBind() {
    if (tdset == "") {
        tdset = $("#ProductInsert").find("tr:eq(1)");
    }
    $("#ProductInsert").hide();
    $(tdset).insertBefore($("#grdOrderDetails").find("tr:eq(1)"));
}

//<summary>function Used to Reset Page</summary>
function ResetPage() {
    //Reseting all input controls in the page
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        //Avoid Order ID And Set the value as 0
        if (idval.search("OrderID") != -1) {
            $(this).val('0');
        }
        if (idval.search("EditProduct") != -1) {
            $(this).val('0');
        }
        //Avoid UserPk to get the value of log in user
        else if (idval.search("UserPk") == -1) {
            $(this).val("");
        }

    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    //Reseting all text area controls in the page
    $(document.forms[0]).find("textarea").each(function () {
        $(this).val('');
    });  
    //Set Focus to first Control
    $('input[id$=OrderCode]').focus();
    DateInit();
    return false;
}

///#endregion
    
