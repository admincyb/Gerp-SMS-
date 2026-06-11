/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="GrandTreeMulti.js" />
/// <reference path="../MasterPage.js" />

///#region ------Global Variable-----
var POID = 0;
var POJSON;
var tdset = "";
var tdset1 = "";
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Purchase";
var materialID = "";
///#endregion

//#region -------Configuration Section-------
var POCreation = {
//URLS
    GetVendorsURL: "VendorRegistration.do?Action=GetVendors&SBUPk=",
    GetVendorDetailsURL: "VendorRegistration.do?Action=GetVendorDtls&VendorId=",
    GetDepartmentURL: "SubDepartment.do?Action=GetInvDepartment",
    GetVendorMaterials: "VendorRegistration.do?Action=GetVendorMaterials&VendorId=",
    GetMaterialDetails: "VendorRegistration.do?Action=GetVendorMaterialDetails&VendorId=",
    GetMaterialUOMDetails: "VendorRegistration.do?Action=GetMaterialUOM&VendorId=",
    GetGeneralTerms: "GeneralTemplateMaster.do?Action=GetGeneralTerms",
    GetVenderTerm: "VendorTermsManagement.do?Action=GetVenderTerm&VendorId=",
    GetMaterialUOMConversion: "MaterialManagement.do?Action=GetMaterialUOMConversion&MaterialId=",
    GetActiveTax: "TaxSettings.do?Action=GetActiveTax",
    CreatePurchaseOrder: "POGeneration.do?Action=CreatePurchaseOrder",
    GetDepartmentDetails: "SubDepartmentManagement.do?Action=GetUserDepartments",
    //Constants
    Shipping:"Shipping",
    Billing: "Billing",
    PoListing: "PurchaseOrderListing.aspx",
    Purchase :"1",
    //Commands
    Edit:"EDIT",
    Delete: "DELETE",
    SaveCommand:"SAVED",
    //Fields
    POMaterialPK: "POD_PK",
    MaterialID: "POD_ITEM",
    MaterialCode: "ITM_CODE",
    MaterialRate : "POD_RATE",
    MaterialQty: "POD_QTY_REQUESTED", 
    MaterialUOM : "POD_UOM",
    MaterialDiscount: "POD_DISC_AMT",
    Remarks: "POD_REMARKS",
    Amount: "POD_AMT_VALUE",
    BaseConversion: "POD_CONV_FACT",
    POMaterialID: "POMaterialID",
    MaterialTax:"TAX_PERC",
    //Field For Tax Details
    ShippingCost :"Shipping Cost",
    SalesTax:"Sales Tax",
    AdditionalTax: "AdditionalTax",
    //Message
    POSaveMessage: "Purchase Order #PONUMBER# saved Successfully", 
    Information:"Information",
    ActionFailedMessage: "Action Failed Please Try Again",
    MaterialRequired: "Please select a material detail",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    RecordExist: "Record already exist, Try again"
    


    }
///#endregion

///#region-------Initialization Section ----------------


$.validator.addMethod('selectNone', function (value, element) {
    ///<summary>
    ///Add additional validation for select
    ///</summary>
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$(document).ready(function () {
    PageInit();
    //$("#dummytable").css({ "display": "none", "visibility": "hidden" });
    $("#dummydiv").hide();

});

function PageInit() {

    WindowExpand(true);
    ChangeMode(1);
    //initialize PO Object And Store in divPOData
    POJSON = $.parseJSON($("[id$=MaterialDetails]").val());
    $("#divPOData").data("POData", POJSON);
    //Make File upload
    GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST");
    if (POJSON.POH_PK == undefined || POJSON.POH_PK == 0) {
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdPODetails"), 0, dummyObj);
        GrandGrid.MakeGrid($("#grdPODetails"), 0, dummyObj);

    }
    FillGeneralTerms();

    if (!($.isArray(POJSON.MaterialDetails))) {

        if (POJSON.MaterialDetails != undefined) {
            objArray = POJSON.MaterialDetails;
            POJSON.MaterialDetails = new Array();
            POJSON.MaterialDetails.push(objArray);
        }
        else {
            objArray = POJSON.MaterialDetails;
            POJSON.MaterialDetails = new Array();

        }
    }

    if (!($.isArray(tdset))) {
        objArray = tdset;
        tdset = new Array();
        tdset.push(objArray);
    }



    if (POJSON.POH_PK == undefined || POJSON.POH_PK == 0) {
       
        FillVendors();
        FillDepartment(false, POCreation.Shipping);
        FillDepartment(false, POCreation.Billing);
        FillDepartments();
        
    }
    else {
        FillVendors(POJSON.POH_VENDOR);
        FillPODetails(POJSON);


    }

    //Create Date Picker
    GrandScriptUtils.DatePicker("POH_DATE", false, true);
    AssignTax();
    ClearMaterialDetails();

    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("=");
            if (pK[1] != "" && pK[0] == "Status") {
                $("[id$=imbDraft]").hide();
                $("[id$=imbSave]").hide();
            }
        }

    }
    $("[id$=POH_DATE]").focus();
    return false;
}

function FillPODetails(POJSON) {
    ///<summary>function used to fill vendor details corresponding to vendor id </summary>
    /// <param name="vendorObject"  type="Object">
    /// vendor object fetched corresponding to vendorID
    /// </param>
    $("[id$=POH_PK]").val(POJSON.POH_PK);
    //$("[id$=DeptPk]").val(POJSON.DeptPk);
    $("[id$=POH_NO]").val(POJSON.POH_NO);
    $("[id$=lblPOH_NO]").html(POJSON.POH_NO);
    $("[id$=POH_DATE]").val(POJSON.POH_DATE);
    $("[id$=CreatedBy]").html(POJSON.POH_CRTD_BY);
    $("[id$=POH_CRTD_BY]").val(POJSON.POH_CRTD_BY);
    FillVendors(POJSON.POH_VENDOR);
    FillVendorDetails(POJSON.POH_VENDOR);
    FillDepartment(POJSON.POH_SHIPPING, POCreation.Shipping);
    FillDepartment(POJSON.POH_BILLING, POCreation.Billing);
    FillDepartmentDetails(POJSON.POH_SHIPPING, POCreation.Shipping);
    FillDepartmentDetails(POJSON.POH_BILLING, POCreation.Billing);
    $("[id$=POH_SUB_TOTAL]").val(POJSON.POH_SUB_TOTAL);
    $("[id$=POH_DISC_AMT]").val(POJSON.POH_DISC_AMT);
    $("[id$=POH_NET_TOTAL]").val(POJSON.POH_NET_TOTAL);
    $("[id$=POH_SHIP_CHARGE]").val(POJSON.POH_SHIP_CHARGE);
    $("[id$=POH_SALES_TAX_AMT]").val(POJSON.POH_SALES_TAX_AMT);
    $("[id$=POH_ADD_TAX_AMT]").val(POJSON.POH_ADD_TAX_AMT);
    $("[id$=POH_PRICE_ADJUST]").val(POJSON.POH_PRICE_ADJUST);
    $("[id$=POH_TOTAL_VALUE]").val(POJSON.POH_TOTAL_VALUE);
    FillDepartments(POJSON.POH_DEPT);
    $("[id$=POH_REMARKS]").html(POJSON.POH_REMARKS);
    $("[id$=POH_VENDOR_TERMS]").html(POJSON.POH_VENDOR_TERMS);
    $("[id$=LblPOH_TERMS]").html(POJSON.POH_TERMS);
    $("[id$=POH_TERMS]").val(POJSON.POH_TERMS);
    $("[id$=POH_COMMENTS]").html(POJSON.POH_COMMENTS);

    if (!($.isArray(POJSON.MaterialDetails))) {

        if (POJSON.MaterialDetails != undefined) {
            objArray = POJSON.MaterialDetails;
            POJSON.MaterialDetails = new Array();
            POJSON.MaterialDetails.push(objArray);
        }
        else {
            objArray = POJSON.MaterialDetails;
            POJSON.MaterialDetails = new Array();

        }
    }
   
   //#region --------------- Fill File Upload Details----------------------------------

    if (!($.isArray(POJSON.FILELIST))) {
        if (POJSON.FILELIST != undefined) {
            objArray = POJSON.FILELIST;
            FileJson.FILELIST = new Array();
            FileJson.FILELIST.push(objArray);
        }
        else {
            objArray = POJSON.FILELIST;
            FileJson.FILELIST = new Array();

        }

    }
    else {
        FileJson.FILELIST = POJSON.FILELIST;
    }
    FillFileDetails();
    //#Endregion
    GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
    //ClearMaterials();

}


//To Fill File Details To Grid
function FillFileDetails() {
    ///<summary>To Fill File Details And dispaly as Listing With Delete Option</summary>
    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + "." + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../TryOuts/ShowFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}

function FillVendors(vendorID) {

    var drpID = $("select[id$=POH_VENDOR]").attr("id");
    $.get(POCreation.GetVendorsURL + $("[id$=BizUnitPk]").val(), function (data) {
        if(vendorID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, vendorID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);

    });

}

function FillVendorDetails(vendorID) {
    if (vendorID != "0") {
        FillVendorMaterials(vendorID);
        FillVendorTerms(vendorID);
        $.get(POCreation.GetVendorDetailsURL + vendorID, function (data) {
            if (data.length > 0) {
                $("[id$=VendorName]").html(data[0].VEN_NAME);
                $("[id$=ContactName]").html(data[0].VEN_CONT_NAME);
                $("[id$=TinNo]").html(data[0].VEN_TIN);
                $("[id$=VendorAddressDtls]").html(data[0].ADDRESS);
            }

        });
    }
    else {
        $("select[id$=POD_UOM]").find("option").remove();
        $("[id$=VendorName]").html("");
        $("[id$=ContactName]").html("");
        $("[id$=TinNo]").html("");
        $("[id$=VendorAddressDtls]").html("");
        $("select[id$=VENDOR_TERMS]").find("option").remove();
        $("[id$=POH_VENDOR_TERMS]").val("");
        $("select[id$=VENDOR_TERMS]").find("option").remove();
        $("input[id$=POD_CONV_FACT]").val("1");
        $("select[id$=POD_ITEM]").find("option").remove();
    }

}

function FillDepartments(depPK) {
    var drpID = $("select[id$=POH_DEPT]").attr("id");
    var url = POCreation.GetDepartmentDetails + "&BaseDpt=" + POCreation.Purchase + "&SBUPk=" + $("input[id$=BizUnitPk]").val();
     $.get(url, function (data) {
         if (depPK)
             GrandScriptUtils.FillDropDown(drpID, data, true, true, depPK);
         else
             GrandScriptUtils.FillDropDown(drpID, data, true, true);
     });
}


function FillDepartmentDetails(deptID, type) {
    if (deptID != 0) {
        $.get(POCreation.GetDepartmentURL + "&DeptID=" + deptID, function (data) {
            if (data.length > 0) {
                if (type == POCreation.Shipping) {
                    $("[id$=ShippingSiteName]").html(data[0].DPT_NAME);
                    $("[id$=ShippingAddressDtls]").html(data[0].ADDRESS);
                }

                else if (type == POCreation.Billing) {
                    $("[id$=BillingSiteName]").html(data[0].DPT_NAME);
                    $("[id$=BillingAddressDtls]").html(data[0].ADDRESS);
                }
            }

        });
    }
    else {
        if (type == POCreation.Shipping) {
            $("[id$=ShippingSiteName]").html("");
            $("[id$=ShippingAddressDtls]").html("");
        }

        else if (type == POCreation.Billing) {
            $("[id$=BillingSiteName]").html("");
            $("[id$=BillingAddressDtls]").html("");
        }
    }
 }

 function FillDepartment(deptID,type) {
     var drpID;
     
     if(type==POCreation.Shipping)
         drpID = $("select[id$=POH_SHIPPING]").attr("id");
     else if(type==POCreation.Billing)
         drpID = $("select[id$=POH_BILLING]").attr("id");

     $.get(POCreation.GetDepartmentURL, function (data) {
         if (deptID)
         {         
            GrandScriptUtils.FillDropDown(drpID, data, true, true,deptID);
          }
         else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
     });
 }

 function FillVendorMaterials(vendorID) {
     var drpID = $("select[id$=POD_ITEM]").attr("id");   
     if (vendorID != 0) {
         $.get(POCreation.GetVendorMaterials + vendorID, function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
         });
     }
}

function FillMaterialUOM(materialID,uomID) {
    var vendorID = $("select[id$=POH_VENDOR]").val();
    var drpID = $("select[id$=POD_UOM]").attr("id");
    if (uomID != 0) {
        $.get(POCreation.GetMaterialUOMDetails + vendorID + "&MaterialId=" + materialID, function (data) {
        if(uomID)
                GrandScriptUtils.FillDropDown(drpID, data, true, true,uomID);
            else
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
        });
    }

}

function FillMaterialDetails(materialID, status) {
//Status will avoid the price adding while editing
    var vendorID = $("select[id$=POH_VENDOR]").val();
    $("input[id$=POD_CONV_FACT]").val('1');
    if (materialID != 0) {
        $.get(POCreation.GetMaterialDetails + vendorID + "&MaterialId=" + materialID, function (data) {
            if (data.length > 0) {
                $("[id$=ITM_CODE]").html(data[0].ITM_CODE);
                $("[id$=TAX_PERC]").html(data[0].TAX_PERC);
                if (!status) {
                    $("[id$=POD_RATE]").val(data[0].ITM_PRICE);
                    }
                    FillMaterialUOM(materialID, data[0].ITM_UOM);
                
            }
        });
    }
    else {
        //$("select[id$=UOM_PK]").find("option").remove();
        $("select[id$=POD_UOM]").find("option").remove();
        $("[id$=ITM_CODE]").html("");
        $("[id$=TAX_PERC]").html("");
        $("[id$=POD_RATE]").val("0.000");
        $("[id$=POD_DISC_AMT]").val("0.000");
        $("[id$=POD_QTY_REQUESTED]").val("0.000");
        $("[id$=POD_CONV_FACT]").val("1");
        $("textarea[id$=POD_REMARKS]").html("");
        $("[id$=POD_AMT_VALUE]").val("");
    }

}

function FillGeneralTerms(termsID) {
    var temp = $("[id$=POH_TERMS]").html();
    if (!termsID) {
        var drpID = $("select[id$=GENERAL_TERMS]").attr("id");
        $.get(POCreation.GetGeneralTerms, function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        });
    }
    else {
        $.get(POCreation.GetGeneralTerms + "&TermsID=" + termsID, function (data) {
            if (data.length > 0) {
                $("[id$=POH_TERMS]").val(temp + data[0].TMDDESCRIPTION);
                $("[id$=LblPOH_TERMS]").html(temp + data[0].TMDDESCRIPTION);             
            }
        });
    }
}

function FillVendorTerms(vendorID, termsID) {
    var drpID = $("select[id$=VENDOR_TERMS]").attr("id");
    $.get(POCreation.GetVenderTerm + vendorID, function (data) {
        if (termsID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true,termsID);
    });
}

function FillVendorTermsDetails(termsID) {
    var temp = $("[id$=POH_VENDOR_TERMS]").val();
    var vendorID = $("select[id$=POH_VENDOR]").val();
    $.get(POCreation.GetVenderTerm + vendorID + "&TermsId=" + termsID, function (data) {
        if (data.length > 0) {
            $("[id$=POH_VENDOR_TERMS]").val(temp + data[0].VTD_VAL);

        }
        
    });

}

function CalculateTax() {
    POJSON = $("#divPOData").data("POData");
    var subTot = 0.000;
    for (var i in POJSON.MaterialDetails) {
        subTot = subTot + parseFloat(POJSON.MaterialDetails[i].POD_AMT_VALUE)
    }
    $("input[id$=POH_SUB_TOTAL]").val(subTot.toFixed(3));
}

function GetUomConversion(UOMID) {
    var materialID = $("select[id$=POD_ITEM]").val();
    if (UOMID != "0") {
        $.get(POCreation.GetMaterialUOMConversion + materialID + "&UOMId=" + UOMID, function (data) {
            if (data.length > 0) {
                $("input[id$=POD_CONV_FACT]").val(data[0].UMC_CONV_FACT);
                CalculateMaterialAmount();
            }
            else {
                $("input[id$=POD_CONV_FACT]").val('1');
                CalculateMaterialAmount();
            }
        });
    }
    else {

        $("input[id$=POD_CONV_FACT]").val('1');
        CalculateMaterialAmount();
    }
   

}

// #endregion

 ///#region ---Core Section-----
function AddPoMaterials() {
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        POJSON = $("#divPOData").data("POData");
        var editMaterial = $("input[id$=EditMaterial]").val();
        var obj = new Object();
//        if (editMaterial != 0) {
//            for (var i in POJSON.MaterialDetails) {
//                if (editMaterial == POJSON.MaterialDetails[i].POMaterialID)
//                    obj = POJSON.MaterialDetails[i];
//            }
        //        }

         var flag = true;
        //Loop used to check the Material already added in the order List
        if (parseInt(editMaterial) == 0) {
            for (var i in POJSON.MaterialDetails) {
                if (POJSON.MaterialDetails[i].POD_ITEM == $("select[id$=POD_ITEM]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in POJSON.MaterialDetails) {
                if (POJSON.MaterialDetails[i].POD_ITEM == $("select[id$=POD_ITEM]").val() && parseInt(editMaterial) != POJSON.MaterialDetails[i].POD_ITEM) {
                    flag = false;
                    break;
                }
                if (parseInt(editMaterial) == POJSON.MaterialDetails[i].POD_ITEM)
                    obj = POJSON.MaterialDetails[i];
            }
        }
        if (flag) {
        var guid = GrandScriptUtils.GenerateGuid();

        obj.POD_ITEM = $("select[id$=POD_ITEM]").val();
        obj.ITV_NAME = $("select[id$=POD_ITEM] option:selected").text();
        obj.ITM_CODE = $("[id$=ITM_CODE]").html();
        obj.POD_RATE = $("input[id$=POD_RATE]").val();
        obj.POD_QTY_REQUESTED = $("[id$=POD_QTY_REQUESTED]").val();
        obj.POD_UOM = $("select[id$=POD_UOM]").val();
        obj.POD_UOM_TEXT = $("select[id$=POD_UOM] option:selected").text();
        obj.POD_DISC_AMT = $("input[id$=POD_DISC_AMT]").val();
        obj.POD_REMARKS = $("textarea[id$=POD_REMARKS]").val();
        obj.POD_AMT_VALUE = $("input[id$=POD_AMT_VALUE]").val();
        obj.POD_CONV_FACT = $("input[id$=POD_CONV_FACT]").val();
        obj.TAX_PERC = $("[id$=TAX_PERC]").html();
        if (editMaterial == 0) {
            obj.POMaterialID = guid;
            obj.POD_PK = "0";
            POJSON.MaterialDetails.push(obj);
        }
        $("#divPOData").data("POData", POJSON);
        GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
        CalculateTaxDetails();
        ClearMaterialDetails();
    }
    else {
        GrandScriptUtils.ShowModal(POCreation.RecordExist, "Information");
        }
       
       

    }

    $("select[id$=POD_ITEM]").focus();
    return false;
 }

 function GridHandler(tr, command) {
     switch (command.toString().toUpperCase()) {
         case POCreation.Edit:
             FillDetails(tr);
             return false;
             break;
         case POCreation.Delete:
             materialID = GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, $(tr).parents("table:first").attr("id"));
             GrandScriptUtils.ShowModal(POCreation.DeleteConfirmationMessage,POCreation.ConfirmationMessage, POCreation.Delete);
             //DeleteDetails(tr);
             return false;
             break;
     }

 }

 function FillDetails(tr) {
     var tableID = $(tr).parents("table:first").attr("id");
     $("input[id$=EditMaterial]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID));
     $("[id$=POD_TAX_PERC]").html(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialTax, tableID))
     FillMaterialDetails(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID),true);
     $("select[id$=POD_ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID));
     $("[id$=ITM_CODE]").html(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialCode, tableID));
     $("input[id$=POD_RATE]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialRate, tableID));
     $("[id$=POD_QTY_REQUESTED]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialQty, tableID));
     //$("select[id$=POD_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialUOM, tableID));
     //FillMaterialUOM(GrandGrid.Utilities.GetColumnValue(tr, POCreation.POMaterialID, tableID), GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialUOM, tableID));
     $("input[id$=POD_DISC_AMT]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialDiscount, tableID));
     $("textarea[id$=POD_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.Remarks, tableID));
     $("input[id$=POD_AMT_VALUE]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.Amount, tableID));
     $("input[id$=POD_CONV_FACT]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.BaseConversion, tableID))
     $("select[id$=POD_ITEM]").attr("disabled", true);
 }

 function DeleteDetails(tr) {
     ///<summary>Used fill Details of requisition for Delete</summary>
     /// <param name="tr"  type="Object">
     ///  Specific Container and its controls       
     /// </param>  
     var POJSON = $("#divPOData").data("POData");
     for (var i in POJSON.MaterialDetails) {
         if (POJSON.MaterialDetails[i].POD_ITEM == materialID) {
             //Will delete the Material Details
             POJSON.MaterialDetails.splice(i, 1);
             break;
         }
     }
     

     //Getting the table name in variable 
     //var tableID = $(tr).parents("table:first").attr("id");
     //var POMaterialID = GrandGrid.Utilities.GetColumnValue(tr, "POMaterialID", tableID);
//     var POJSON = $("#divPOData").data("POData");
//     for (var i in POJSON.MaterialDetails) {
//         if (POJSON.MaterialDetails[i].POMaterialID == POMaterialID) {
//             //Will delete the Material Details
//             POJSON.MaterialDetails.splice(i, 1);
//             break;
//         }
//     }
     $("#divPOData").data("POData", POJSON);

     GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
     ClearMaterialDetails();
     CalculateTaxDetails();
     if (POJSON.MaterialDetails.length == 0) {
         //Will insert the selection tr  into the  ProductInsert table and show the ProductInsert Table
         $(tdset1).insertAfter($("#MaterialsDetails").find("tr:eq(0)"));
         //$("#ProductInsert").show();
         $("#MaterialsDetails").css({ "display": "block", "visibility": "visible" });
         $("#poDetails").hide();
     }
     else {
         $("#poDetails").show();
     }

     return false;
 }

 function ClearMaterialDetails() {
     $("input[id$=EditMaterial]").val("");
     $("select[id$=POD_ITEM]").val("0");
     $("[id$=ITM_CODE]").html("");
     $("input[id$=POD_RATE]").val("0.000");
     $("[id$=POD_QTY_REQUESTED]").val("0.000");
     $("input[id$=POD_DISC_AMT]").val("0.000");
     $("textarea[id$=POD_REMARKS]").val("");
     $("input[id$=POD_AMT_VALUE]").val("0.000");
     $("select[id$=POD_UOM]").find("option").remove();
    // $("select[id$=POD_UOM]").val("0");
     //reseting the base convrsion
     $("input[id$=POD_CONV_FACT]").val("1");
     $("[id$=TAX_PERC]").html("");
     $("select[id$=POD_ITEM]").attr("disabled", false);
     RemoveValidation();
 }

 function CalculateMaterialAmount() {
     ChangeMode(2);
     var price = $("input[id$=POD_RATE]").val();
     var qty = $("input[id$=POD_QTY_REQUESTED]").val();
     var discount = $("input[id$=POD_DISC_AMT]").val();
     var Conversion = $("input[id$=POD_CONV_FACT]").val();
     var tax = $("[id$=TAX_PERC]").html();
     var result;
     var taxper;

     if (price == "") 
         price = 0.000;
     if (qty == "") 
        qty = 0.000;
     if (discount == "")
         discount = 0.000;
     result = ((parseFloat(price) * ((1 / parseFloat(Conversion)) * parseFloat(qty))) - (parseFloat(discount)));
     taxper = parseFloat(result) * (tax / 100)
     result = result - taxper;
     if (isNaN(result)) {
        result = 0.000;
     }
     $("input[id$=POD_AMT_VALUE]").val(result.toFixed(3));
     ChangeMode(1);
 }

 function AfterGridBind() {
       //<summary>function Call Afer binding Grid</summary>
     if (tdset.length <= 1) {//tdset contains controls for add details.

         $("#dummytable").find("tr:has(td)").each(function () {
             tdset.push(this);
             $(this).insertAfter($("#grdPODetails").find("tr:last"));

         });
     }
     else {
         for (var i in tdset) {
            $(tdset[i]).insertAfter($("#grdPODetails").find("tr:last"));
        }

    }
    $("#poDetails").show();
    $("#dummydiv").hide();
     //$("#dummytable").hide();
     CalculateTax();
     if (tdset1 == "") {//tdset contains controls for add details.
         tdset1 = $("#MaterialsDetails").find("tr:eq(1)");
     }
     $("#MaterialsDetails").css({ "display": "none", "visibility": "hidden" });
     $(tdset1).insertBefore($("#grdPODetails").find("tr:eq(1)"));

   
 }

 function AssignTax() {
     $.get(POCreation.GetActiveTax, function (data) {
         if (data.length > 0) {
             for (var i in data) {
                if(data[i].TAX_HEAD == POCreation.ShippingCost)
                {
                    $("input[id$=POH_SHIP_CHARGE]").add("attr", "TAX");
                    $("input[id$=POH_SHIP_CHARGE]").attr("TAX",data[i].TAX_FORMULA);
                }
                else if(data[i].TAX_HEAD == POCreation.SalesTax)
                {
                    $("input[id$=POH_SALES_TAX_AMT]").add("attr", "TAX");
                    $("input[id$=POH_SALES_TAX_AMT]").attr("TAX", data[i].TAX_FORMULA);
                }
                else if (data[i].TAX_HEAD == POCreation.AdditionalTax) {
                    $("input[id$=POH_ADD_TAX_AMT]").add("attr", "TAX");
                    $("input[id$=POH_ADD_TAX_AMT]").attr("TAX", data[i].TAX_FORMULA);
                }
             }         
          }
         
     });
}

function CalculateTaxDetails() {
    var Result = $("input[id$=POH_SUB_TOTAL]").val();

    var Discount = $("input[id$=POH_DISC_AMT]").val() == "" ? 0.000 : $("input[id$=POH_DISC_AMT]").val();
//    if (Discount == "")
//        Discount = 0.000;
    var NetAmount = (Result - Discount); 
    if(isNaN(NetAmount))
        NetAmount = 0.000;
    $("input[id$=POH_NET_TOTAL]").val(NetAmount.toFixed(3));

    var shippingtax = $("input[id$=POH_SHIP_CHARGE]").attr("TAX");
    var salesTaxrate = $("input[id$=POH_SALES_TAX_AMT]").attr("TAX");
    var additionalTax = $("input[id$=POH_ADD_TAX_AMT]").attr("TAX");
    shippingtax = shippingtax.replace("#SUBTOTAL#", NetAmount)
    salesTaxrate = salesTaxrate.replace("#SUBTOTAL#", NetAmount)
    additionalTax = additionalTax.replace("#SUBTOTAL#", NetAmount)
   
    $("input[id$=POH_SHIP_CHARGE]").val(eval(shippingtax).toFixed(3));
    $("input[id$=POH_SALES_TAX_AMT]").val(eval(salesTaxrate).toFixed(3));
    $("input[id$=POH_ADD_TAX_AMT]").val(eval(additionalTax).toFixed(3))
    CalculateTotal();
}

function CalculateTotal() {

    var Discount = $("input[id$=POH_DISC_AMT]").val() == "" ? 0.000 : $("input[id$=POH_DISC_AMT]").val();
    var SubTotal = $("input[id$=POH_SUB_TOTAL]").val() == "" ? 0.000 : $("input[id$=POH_SUB_TOTAL]").val();
    var NetAmount = $("input[id$=POH_NET_TOTAL]").val() == "" ? 0.000 : $("input[id$=POH_NET_TOTAL]").val();
    NetAmount = (SubTotal - Discount);
    
    NetAmount = isNaN(NetAmount) ? "0.000" : NetAmount;
    $("input[id$=POH_NET_TOTAL]").val(parseFloat(NetAmount).toFixed(3));
    var shippingtax = $("input[id$=POH_SHIP_CHARGE]").val() == "" ? 0.000 : $("input[id$=POH_SHIP_CHARGE]").val();
    var salesTaxrate = $("input[id$=POH_SALES_TAX_AMT]").val() == "" ? 0.000 : $("input[id$=POH_SALES_TAX_AMT]").val();
    var additionalTax = $("input[id$=POH_ADD_TAX_AMT]").val() == "" ? 0.000 : $("input[id$=POH_ADD_TAX_AMT]").val();
    var Adjustment = $("input[id$=POH_PRICE_ADJUST]").val() == "" ? 0.000 : $("input[id$=POH_PRICE_ADJUST]").val();
    var POHTotal = (parseFloat(NetAmount) + parseFloat(shippingtax) + parseFloat(salesTaxrate) + parseFloat(additionalTax) + parseFloat(Adjustment)).toFixed(3);
    POHTotal = isNaN(POHTotal) ? "0.000" : POHTotal
    $("input[id$=POH_TOTAL_VALUE]").val(parseFloat(POHTotal).toFixed(3));

}

function ChangeMode(mode) {
    if (mode == 1) {
        $("input[id$=POD_AMT_VALUE]").attr("disabled", true);
        $("input[id$=POH_SUB_TOTAL]").attr("disabled", true);

        $("input[id$=POH_NET_TOTAL]").attr("disabled", true);

        $("input[id$=POH_SHIP_CHARGE]").attr("disabled", true);
        $("input[id$=POH_SALES_TAX_AMT]").attr("disabled", true);
        $("input[id$=POH_ADD_TAX_AMT]").attr("disabled", true);

        $("input[id$=POH_TOTAL_VALUE]").attr("disabled", true);
    }
    else {
        $("input[id$=POD_AMT_VALUE]").attr("disabled", false);
        $("input[id$=POH_SUB_TOTAL]").attr("disabled", false);
        $("input[id$=POH_NET_TOTAL]").attr("disabled", false);
        $("input[id$=POH_SHIP_CHARGE]").attr("disabled", false);
        $("input[id$=POH_SALES_TAX_AMT]").attr("disabled", false);
        $("input[id$=POH_ADD_TAX_AMT]").attr("disabled", false);
        $("input[id$=POH_TOTAL_VALUE]").attr("disabled", false);
    }
}

function SavePage(command) {
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        ChangeMode(2);
        POJSON = $("#divPOData").data("POData");
        if (POJSON.MaterialDetails.length > 0) {
            $("[id$=MaterialDetails]").val(JSON.stringify(POJSON.MaterialDetails));

            // For File Upload---------------------------------------------------------------------
            var ObjFile = $("#divFileData").data("FileData");
            $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));

            if (command != "Draft")
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
            else
                $("[id$=ActionID]").val('0');

            var jSonString = GrandScriptUtils.FormToJsonString("divData");
            $.post(POCreation.CreatePurchaseOrder, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully

                if (data != "" && data != "-1") {
                    var msg = POCreation.POSaveMessage.replace('#PONUMBER#', data);
                    GrandScriptUtils.ShowModal(msg, POCreation.Information, POCreation.SaveCommand);

                }
                else if (data == "-1") {
                    GrandScriptUtils.ShowModal(POCreation.ActionFailedMessage);
                    $("[id$=SubmitFlag]").val('0')
                }
                else {
                    GrandScriptUtils.ShowModal(POCreation.ActionFailedMessage);
                    $("[id$=SubmitFlag]").val('0')

                }


            });
        }
        else {
            GrandScriptUtils.ShowModal(POCreation.MaterialRequired, POCreation.Information);
            $("[id$=SubmitFlag]").val('0')
        }

    }
    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req
        case POCreation.SaveCommand:
            window.location = POCreation.PoListing;
            break;
        case POCreation.Delete:
            DeleteDetails()
       
    }
    return false;
}

function ResetPage() {
    window.location = POCreation.PoListing;
    return false;
}

///#endregion

///#region------ Validations ----------------
function AddValidations(mode) {
    RemoveValidation()
    if (mode == 1) {
        $("input[id$=POH_DATE]").rules("add", {
            required: true,
            maxlength: 11,
            messages: { required: "Select Requred By" },
            messages: { Date: "Enter valid date" }
        });
       
        $("select[id$=POH_VENDOR]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Vendor Detail" }
        });

        $("select[id$=POH_SHIPPING]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Shipping Detail" }
        });

        $("select[id$=POH_BILLING]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Billing Detail" }
        });

        $("select[id$=POH_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Department" }
        });
        

        $("input[id$=POH_DISC_AMT]").rules("add", {
            ZeroDecimal: true

        });

        $("input[id$=POH_PRICE_ADJUST]").rules("add", {
            ZeroDecimal: true
        });
       

    }
    else if (mode == 2) {
        $("select[id$=POD_ITEM]").rules("add", {
            selectNone: true,
            required: true,
            messages: { required: "Select Material" , selectNone: "Select Material" }
        });
//        $("select[id$=POD_ITEM]").rules("add", {
//            selectNone: true,
//            messages: { selectNone: "Select Material" }
//        });
        $("select[id$=POD_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Select UOM" }
        });

        $("input[id$=POD_RATE]").rules("add", {
            ThreeDecimal: true

        });

        $("input[id$=POD_QTY_REQUESTED]").rules("add", {
            ThreeDecimal: true

        });

        $("input[id$=POD_DISC_AMT]").rules("add", {
            ZeroDecimal: true
        });

       

        

    }


}

function RemoveValidation() {
    $("input[id$=POH_DATE]").rules("remove");
    $("select[id$=POH_VENDOR]").rules("remove");
    $("select[id$=POH_SHIPPING]").rules("remove");
    $("select[id$=POH_BILLING]").rules("remove");
    $("select[id$=POH_DEPT]").rules("remove");
    $("select[id$=POD_ITEM]").rules("remove");
    $("select[id$=POD_UOM]").rules("remove");
    $("input[id$=POD_RATE]").rules("remove");
    $("input[id$=POD_QTY_REQUESTED]").rules("remove");
    $("input[id$=POD_DISC_AMT]").rules("remove");
    $("input[id$=POH_DISC_AMT]").rules("remove");
    $("input[id$=POH_PRICE_ADJUST]").rules("remove");
}

///#endregion