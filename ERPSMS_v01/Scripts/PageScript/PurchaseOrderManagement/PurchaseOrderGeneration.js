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
var PRtdset = "";
var Terms = new Array();
var VendorTerms = new Array();

///#endregion

//#region -------Configuration Section-------
var POCreation = {
//URLS
    GetVendorsURL: "VendorRegistration.do?Action=GetVendors&SBUPk=",
    GetVendorDetailsURL: "VendorRegistration.do?Action=GetVendorDtls&VendorId=",
    GetDepartmentURL: "SubDepartment.do?Action=GetInvDepartment",
    GetVendorMaterials: "VendorRegistration.do?Action=GetVendorMaterials&VendorId=",
    GetVendorMaterialsCode:"VendorRegistration.do?Action=GetVendorMaterialsCode&VendorId=",
    GetMaterialDetails: "VendorRegistration.do?Action=GetVendorMaterialDetails&VendorId=",
    GetMaterialUOMDetails: "VendorRegistration.do?Action=GetMaterialUOM&VendorId=",
    GetGeneralTerms: "GeneralTemplateMaster.do?Action=GetGeneralTerms",
    GetVenderTerm: "VendorTermsManagement.do?Action=GetVenderTerm&VendorId=",
    GetMaterialUOMConversion: "MaterialManagement.do?Action=GetMaterialUOMConversion&MaterialId=",
    GetActiveTax: "TaxSettings.do?Action=GetActiveTax",
    CreatePurchaseOrder: "POGeneration.do?Action=CreatePurchaseOrder",
    GetDepartmentDetails: "SubDepartmentManagement.do?Action=GetUserDepartments",
    GetPRDetails :"POGeneration.do?Action=GetPRDetails&POID=",
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
    AdditionalTax: "Additional Tax",
    //Message
    POSaveMessage: "Translate(SaveMsgPO)",
    Information: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MaterialRequired: "Please select a material detail",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    RecordExist: "Translate(AlreadyExists)",
    GeneralTermsDuplicationMsg:"Translate(GeneralTermsDuplicationMsg)",
    VendorTermsDuplicationMsg : "Traslate(VendorTermsDuplicationMsg)"

    


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
    ///<summary>
    ///function invoked after all the controls benn rendered
    ///</summary>
   //Function used to initialize the page details Filling the details corresponding to po
   PageInit();
   $("#dummydiv").hide();

});

function PrintPage() {
    ///<summary>
    ///function Used to print thr po details 
    ///</summary>
    window.location = "PurchaseOrderReport.aspx?POID=" + $("input[id$=POH_PK]").val();
    return false;
}

function PageInit() {
    ///<summary>
    ///function used to initialize page details
    ///</summary>
    //will hide the side navigation bar
    WindowExpand(true);
    //Used to chage the Drop down mode as disabled
    ChangeMode(1);
    //initialize PO Object And Store in divPOData
    POJSON = $.parseJSON($("[id$=MaterialDetails]").val());
    $("#divPOData").data("POData", POJSON);
    //Make File upload
    GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST","PO");
    if (POJSON.POH_PK == undefined || POJSON.POH_PK == 0) {
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdPODetails"), 0, dummyObj);
        GrandGrid.MakeGrid($("#grdPODetails"), 0, dummyObj);
        $("[id$=imbPrint]").hide(); 
    }
    else {
        $("[id$=imbPrint]").show();

    }
    //Fill the general terms to drop down
    FillGeneralTerms();
    //Initializing the array if one element convert it to array object
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

    //Check its initail if initial fill all the details
    if (POJSON.POH_PK == undefined || POJSON.POH_PK == 0) {
       
        FillVendors();
        FillDepartment(false, POCreation.Shipping);
        FillDepartment(false, POCreation.Billing);
        FillDepartments();

    }
    //else it will fill the details corresponding to Purchase order
    else {

        FillVendors(POJSON.POH_VENDOR);
        FillPODetails(POJSON);


    }

    //Create Date Picker
    GrandScriptUtils.DatePicker("POH_DATE", false, true);

    //Function used to assign tax to the text box as attribute
    AssignTax();
    //Function used to Clear material details
    ClearMaterialDetails();

    //Get Query string in a variable
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        //If Query string not empty
        var qstrings = queryStr.split("&")
        //Splitting the query sting with the help of &
        for (var i = 0; i < qstrings.length; i++) {
        //loop through Query string
            var pK = qstrings[i].split("=");
            if (pK[1] != "" && pK[0] == "Status") {
                //Check the query string Name status if status as 1 thn its in view mode
                $("[id$=imbDraft]").hide();
                $("[id$=imbSave]").hide();
                $("#divAction").hide();
            }
        }

    }
    //Close modal window
    $("#divPRDetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        //Removing validations in the modal window controls befor Closing the modal 
        beforeClose: function (event, ui) {
            ClearMaterialDetails();
            $("input[id$=AdditionalQty]").rules("remove");
            $("input[id$=TotalQty]").rules("remove");
            $("input[id$=MaterialPrice]").rules("remove");
            $("input[id$=MaterialDiscount]").rules("remove");
            $("input[id$=MaterialTax]").rules("remove");
            $("input[id$=MaterailTotal]").rules("remove");
            $("[id$=MaterialRemarks]").rules("remove");
        }
    })
    //Function used to bind the workflow comments 
    BindWorkFlowComment();
    
    return false;
}


function BindWorkFlowComment() {
    ///<summary>To handle bind grid </summary>

    GrandScriptUtils.BindWorkFlowCommand("grdWrkfComment");
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
    $("[id$=LblPOH_VENDOR_TERMS]").html(POJSON.VENDOR_TERMS);
    $("[id$=LblPOH_TERMS]").html(POJSON.TERMS);
    if (POJSON.POH_VENDOR_TERMS!=null)
        VendorTerms = POJSON.POH_VENDOR_TERMS.split(',');
    if (POJSON.POH_TERMS != null)
        Terms = POJSON.POH_TERMS.split(',');
    $("[id$=POH_VENDOR_TERMS]").val(POJSON.POH_VENDOR_TERMS);
    $("[id$=POH_TERMS]").val(POJSON.POH_TERMS);
    $("[id$=POH_COMMENTS]").html(POJSON.POH_COMMENTS);
    POJSON.TERMS = "";
    POJSON.VENDOR_TERMS = "";
    if (parseInt(POJSON.POH_STATUS) > 0) {
        $("[id$=imbDraft]").hide();
    }

    $("[id$=MaterialDetails]").val("");

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
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}

function FillVendors(vendorID) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    /// <param name="vendorID"  type="Object">
    /// vendorID  used to select the corresponding vendor in Dropdown
    /// </param>
    var drpID = $("select[id$=POH_VENDOR]").attr("id");
    $.get(POCreation.GetVendorsURL + $("[id$=BizUnitPk]").val(), function (data) {
        if(vendorID)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, vendorID);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);

    });

}

function FillVendorDetails(vendorID, status) {
    ///<summary>function used to fill vendor Details corresponding to Vendor ID </summary>
    /// <param name="vendorID"  type="Object">
    /// vendorID  used to select the corresponding vendor in Dropdown
    /// </param>
    if (vendorID != "0") {
    $.get(POCreation.GetVendorDetailsURL + vendorID, function (data) {
            if (data.length > 0) {
                $("[id$=VendorName]").html(data[0].VEN_NAME);
                $("[id$=ContactName]").html(data[0].VEN_CONT_NAME);
                $("[id$=TinNo]").html(data[0].VEN_TIN);
                $("[id$=VendorAddressDtls]").html(data[0].ADDRESS);
            }
            //Fill Materials mapped to the corresponding vendor
            FillVendorMaterials(vendorID);
            //Fill Terms added to the corresponding vendor
            FillVendorTerms(vendorID);
        });

    }
    //if vendor Drop Down Changes to select need to clear all the vendor related details
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

        POJSON = $("#divPOData").data("POData");
        POJSON.MaterialDetails = new Array();
        GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
        if (POJSON.MaterialDetails.length <= 0) {
            $("#poDetails").hide();

        }
        $("#divPOData").data("POData", POJSON);
    }
    //Idenfies the control event
    if (status) {
        POJSON = $("#divPOData").data("POData");
        POJSON.MaterialDetails = new Array();
        GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
        if (POJSON.MaterialDetails.length <= 0) {
            $("#poDetails").hide();

        }
        $("#divPOData").data("POData", POJSON);
    }
    

}

function FillDepartments(depPK) {
    ///<summary>function used to fill department details  </summary>
    /// <param name="depPK"  type="Object">
    /// vendorID  used to select the corresponding vendor in Dropdown
    /// </param>
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
     var drpCodeID = $("select[id$=ITM_CODE]").attr("id");
     
     if (vendorID != 0) {
         $.get(POCreation.GetVendorMaterials + vendorID, function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        });

        $.get(POCreation.GetVendorMaterialsCode + vendorID, function (data) {
            GrandScriptUtils.FillDropDown(drpCodeID, data, true, true);
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

function FillMaterialDetails(Control, status) {
    materialID = $(Control).val();
//Status will avoid the price adding while editing
    var vendorID = $("select[id$=POH_VENDOR]").val();
    $("input[id$=POD_CONV_FACT]").val('1');
    if (materialID != 0) {
        $("[id$=imbSelect]").show();
        if ($(Control).attr("id").search("ITM_CODE") != -1) {
            $("select[id$=POD_ITEM]").val(materialID)
        }
        else {
            $("select[id$=ITM_CODE]").val(materialID)
        }

        $.get(POCreation.GetMaterialDetails + vendorID + "&MaterialId=" + materialID, function (data) {
            if (data.length > 0) {
               //$("[id$=ITM_CODE]").html(data[0].ITM_CODE);
                $("[id$=TAX_PERC]").html(data[0].TAX_PERC);
                //For PR Details Filling
                $("[id$=TaxPerPiece]").val(data[0].TAX_PERC);
                $("[id$=MaterialPrice]").val(data[0].ITM_PRICE);
                $("[id$=PerPiecePrice]").val(data[0].ITM_PRICE);
                $("input[id$=PRMaterial]").val(data[0].ITM_PK);
                $("input[id$=PRMaterialName]").val(data[0].ITM_NAME);
                $("input[id$=PRMaterialCode]").val(data[0].ITM_CODE);
                $("[id$=MaterialUOMText]").text(data[0].UOM_CODE);
                $("input[id$=MaterialUOM]").val(data[0].ITM_UOM);

                //End PR Details Filling
                if (!status) {
                    $("[id$=POD_RATE]").val(data[0].ITM_PRICE);
                }
                FillMaterialUOM(materialID, data[0].ITM_UOM);

            }
        });
    }
    else {
        $("[id$=imbSelect]").hide();
        //$("select[id$=UOM_PK]").find("option").remove();
        $("select[id$=POD_UOM]").find("option").remove();
        $("[id$=ITM_CODE]").val("0");
        $("[id$=POD_ITEM]").val("0");
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
    
    var temp = $("[id$=LblPOH_TERMS]").html();
    if (!termsID) {
        
        var drpID = $("select[id$=GENERAL_TERMS]").attr("id");
        $.get(POCreation.GetGeneralTerms, function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        });
        
    }
    else {
        if (termsID != "0") {
            var flag = true;
            for (var i in Terms) {
                if (Terms[i] == termsID)
                    flag = false;
            }
            if (flag) {
                Terms.push(termsID)
                $.get(POCreation.GetGeneralTerms + "&TermsID=" + termsID, function (data) {
                    if (data.length > 0) {
                        //$("[id$=POH_TERMS]").val(temp + data[0].TMDDESCRIPTION + "<br/>");
                        $("[id$=LblPOH_TERMS]").html(temp + data[0].TMDDESCRIPTION + "<br/>");
                    }
                });
            }
            else {
                GrandScriptUtils.ShowModal(POCreation.GeneralTermsDuplicationMsg, POCreation.Information, false, false);
            }
        }
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
    if (termsID != "0") {
        var flag = true;
        for (var i in VendorTerms) {
            if (VendorTerms[i] == termsID)
                flag = false;
        }
        if (flag) {
            VendorTerms.push(termsID)
            var temp = $("[id$=LblPOH_VENDOR_TERMS]").html();
            var vendorID = $("select[id$=POH_VENDOR]").val();
            $.get(POCreation.GetVenderTerm + vendorID + "&TermsId=" + termsID, function (data) {
                if (data.length > 0) {
                    //$("[id$=POH_VENDOR_TERMS]").val(temp + data[0].VTD_VAL + "<br/>");
                    $("[id$=LblPOH_VENDOR_TERMS]").html(temp + data[0].VTD_VAL + "<br/>");

                }

            });
        }
        else {
            GrandScriptUtils.ShowModal(POCreation.VendorTermsDuplicationMsg, POCreation.Information, false, false);
        }
    }

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
    var Price = 0.000;
    if (UOMID != "0") {
        $.get(POCreation.GetMaterialUOMConversion + materialID + "&UOMId=" + UOMID, function (data) {
            if (data.length > 0) {
                $("input[id$=POD_CONV_FACT]").val(data[0].UMC_CONV_FACT);
                price = (parseInt($("input[id$=PerPiecePrice]").val()) * (1/parseInt(data[0].UMC_CONV_FACT)));
                $("[id$=MaterialPrice]").val(price);
            }
//            else {
//                $("input[id$=POD_CONV_FACT]").val('1');
//                CalculateMaterialAmount();
//            }
        });
    }
    else {

        $("input[id$=POD_CONV_FACT]").val('1');
        $.get(POCreation.GetMaterialDetails + vendorID + "&MaterialId=" + materialID, function (data) {
            if (data.length > 0) {
                price = (parseInt(data[0].ITM_PRICE) * parseInt(data[0].UMC_CONV_FACT));
                $("[id$=MaterialPrice]").val(price);
            }
        });
    }


}

function AddPRDetails(material) {

    var objPRDetails = new Object();
    var poID = $("input[id$=POH_PK]").val();
     var materialID=0;
     if (material)
         materialID = material;
     else

         materialID = $("input[id$=PRMaterial]").val();

     $("[id$=MaterialUOMText]").text($("select[id$=POD_UOM] option:selected").text());
     $("input[id$=MaterialUOM]").val($("select[id$=POD_UOM]").val());
    //objPRDetails = $.parseJSON($("[id$=PRDetails]").val());

     $.getJSON(POCreation.GetPRDetails + poID + "&ITEMID=" + materialID+"&UOM="+$("input[id$=MaterialUOM]").val(), function (data) {
         objPRDetails = data;
         $("#PRDetails").data("PRData", objPRDetails);
         if (objPRDetails.length > 0) {
             GrandGrid.MakeGrid($("#grdPRDetails"), 0, objPRDetails);
         }
         else {

             GrandGrid.Utilities.ResetGrid(true, "grdPRDetails");
             GrandGrid.MakeGrid($("#grdPRDetails"), 1, new Object());
             $("#divPRData").hide();
             $("#divDummyPR").show();

             if (PRtdset != "") {//tdset contains controls for add details.
                 //                PRtdset = $("#dummyPR").find("tr:eq(1)");
                 //$("#dummyPR").append(PRtdset);
                 // $("#dummyPR").html(PRtdset[1].outerHTML);
                 // $(PRtdset).insertBefore($("#dummyPR").find("tr:eq(1)"));

                 $(".tDummy").appendTo($("#dummyPR tbody"));
                 $("#grdPRDetails tbody").html("");

             }

         }
         

     });


   
//    $("input[id$=MaterialPrice]").val("23");
//    $("input[id$=TaxPerPiece]").val("4");
//    $("input[id$=PRMaterial]").val("5")
//    $("input[id$=PRMaterialName]").val("Latex")
//    $("input[id$=PRMaterialCode]").val("Latex")
    $("#divPRDetails").dialog({ width: 850, height: 450, resizable: false, modal: true });
    $("#divPRDetails").dialog("open");
    $("#divPRDetails").css({ "min-width": "600", "margin-top": "25px" });
    return false;
}

function CalculatePRTotal(element) {
    var total = 0.000;
    var type = "";
    var qty = 0.000;
    if (element) {
        var tr = $(element).parents("tr:first");
        //Checking the balance quantity less than the Po Order quantity
        if (parseFloat($(element).val()) > parseFloat(tr.find("td:eq(6)").html())) {
            //if greater then it will assign the balance quantity as order quantity
            $(element).val(tr.find("td:eq(6)").html());
        }
    }
    ///<summary>Function Used to calculate the Total based on the change in the Order Qty</summary>
    if ($("#grdPRDetails").find("tr:has(td)").length > 0) {
        $("#grdPRDetails").find("tr:has(td)").each(function () {
            type = $(this).find("td:last input").attr("typed")
            if (type == "Orderqty" || type == "Additionalqty") {
                qty = $(this).find("td:last input").val() == "" ? 0.000 : $(this).find("td:last input").val()
                total += parseFloat(qty);
            }
        });
    }
    else {
        qty =$("input[id$=AdditionalQty]").val()== "" ? 0.000 : $("input[id$=AdditionalQty]").val();
        total = parseFloat(qty);
    }
    $("input[id$=TotalQty]").val(total.toFixed(3));
    CalculatePRMaterialTotal();

}

function CalculatePRMaterialTotal(control) {
    var total =0.000;
    var SubTotal = 0.000;
    var discount = 0.000;
    total = $("input[id$=TotalQty]").val();

    var price = $("input[id$=MaterialPrice]").val() == "" ? 0.000 : $("input[id$=MaterialPrice]").val();
    var qty = $("input[id$=TotalQty]").val() == "" ? 0.000 : $("input[id$=TotalQty]").val();
    if (control == "Discount") {
        var priceQty = (parseFloat(price) * parseFloat(qty));
        discount = $("input[id$=MaterialDiscount]").val() == "" ? 0.000 : $("input[id$=MaterialDiscount]").val();
        if (discount > priceQty) {
            discount = priceQty;
            $("input[id$=MaterialDiscount]").val(discount);
        }
    }
    else {
       discount = $("input[id$=MaterialDiscount]").val() == "" ? 0.000 : $("input[id$=MaterialDiscount]").val();
    }
    
    var tax = $("[id$=TaxPerPiece]").val();
    var result;
    var taxper;
    
    result = (parseFloat(price) * parseFloat(qty)) - (parseFloat(discount));
    taxper = parseFloat(result) * (tax / 100)
    result = result + taxper;
    if (isNaN(result)) {
        result = 0.000;
    }
    $("input[id$=MaterialTax]").val(taxper.toFixed(3));
    $("input[id$=MaterailTotal]").val(result.toFixed(3));



}

function MakeNumeric(event) {
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}

function AddtoPoList() {
    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        POJSON = $("#divPOData").data("POData");
        var obj = new Object();
        var editMaterial = $("input[id$=EditMaterial]").val();

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
                if (POJSON.MaterialDetails[i].POD_ITEM == $("input[id$=PRMaterial]").val() && parseInt(editMaterial) != POJSON.MaterialDetails[i].POD_ITEM) {
                    flag = false;
                    break;
                }
                if (parseInt(editMaterial) == POJSON.MaterialDetails[i].POD_ITEM)
                    obj = POJSON.MaterialDetails[i];
            }
        }

        if (flag) {
            var guid = GrandScriptUtils.GenerateGuid();
            obj.POD_ITEM = $("input[id$=PRMaterial]").val();
            obj.ITV_NAME = $("input[id$=PRMaterialName]").val();
            obj.ITM_CODE = $("input[id$=PRMaterialCode]").val();
            obj.POD_RATE = $("input[id$=MaterialPrice]").val();
            obj.POD_QTY_REQUESTED = $("[id$=TotalQty]").val();
            obj.POD_UOM = $("input[id$=MaterialUOM]").val();
            obj.POD_UOM_TEXT = $("[id$=MaterialUOMText]").text();
            obj.POD_DISC_AMT = $("input[id$=MaterialDiscount]").val() == "" ? 0.000 : $("input[id$=MaterialDiscount]").val();
            obj.POD_REMARKS = $("textarea[id$=MaterialRemarks]").val();
            obj.POD_AMT_VALUE = $("input[id$=MaterailTotal]").val();
            obj.POD_CONV_FACT = 1;
            obj.TAX_PERC = $("input[id$=MaterialTax]").val();


            //    obj.POMaterialID = guid;
            //    obj.POD_PK = "0";
            //    POJSON.MaterialDetails.push(obj);

            if (editMaterial == 0) {
                obj.POMaterialID = guid;
                obj.POD_PK = "0";
                POJSON.MaterialDetails.push(obj);
            }
            UpdatePRDetails();
            $("#divPRDetails").dialog("close");
            $("#divPOData").data("POData", POJSON);
            GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
            CalculateTaxDetails();
            ClearMaterialDetails();
        }
        else {
            GrandScriptUtils.ShowModal(POCreation.RecordExist, POCreation.Information);
            $("#divPRDetails").dialog("close");
        }
    }
    else {
        $("input[id$=TotalQty]").attr("disabled", true);
        $("input[id$=MaterialTax]").attr("disabled", true);
        $("input[id$=MaterailTotal]").attr("disabled", true);
    }
}

function UpdatePRDetails() {
    ////<summary>function used Update PR Details Corresponding to the material  </summary>
    POJSON = $("#divPOData").data("POData");
    var obj;
    var objArray = new Array();
    for (var i in POJSON.MaterialDetails) {
        if ($("input[id$=PRMaterial]").val() == POJSON.MaterialDetails[i].POD_ITEM)
            POJSON.MaterialDetails[i].PRDetails = new Array();
    }

    var tableID = $("#grdPRDetails").attr("id");
    $("#grdPRDetails").find("tr:has(td)").each(function () {
        var type = $(this).find("td:last input").attr("typed")
        if (type == "Orderqty") {
            obj = new Object();
            obj.PRH_PK = GrandGrid.Utilities.GetColumnValue($(this), "PRH_PK", tableID);
            obj.PRH_DATE = GrandGrid.Utilities.GetColumnValue($(this), "PRH_DATE", tableID);
            obj.PRH_NO = GrandGrid.Utilities.GetColumnValue($(this), "PRH_NO", tableID);
            obj.PRD_QTY_APPROVED = GrandGrid.Utilities.GetColumnValue($(this), "PRD_QTY_APPROVED", tableID);
            obj.QTY_BALANCE = GrandGrid.Utilities.GetColumnValue($(this), "QTY_BALANCE", tableID);
            obj.QTY_ORDER = $(this).find("td:last input").val();
            obj.PRD_UOM = GrandGrid.Utilities.GetColumnValue($(this), "PRD_UOM", tableID);
            obj.UOM_NAME = GrandGrid.Utilities.GetColumnValue($(this), "UOM_NAME", tableID);
            obj.PRD_ITEM = GrandGrid.Utilities.GetColumnValue($(this), "PRD_ITEM", tableID);
            objArray.push(obj);
        }

    });

    for (var i in POJSON.MaterialDetails) {
        if ($("input[id$=PRMaterial]").val() == POJSON.MaterialDetails[i].POD_ITEM) {
            POJSON.MaterialDetails[i].PRDetails = objArray;
            POJSON.MaterialDetails[i].TaxPerPiece = $("input[id$=TaxPerPiece]").val();
            POJSON.MaterialDetails[i].AdditionalQty = $("input[id$=AdditionalQty]").val() == "" ? "0.000" : $("input[id$=AdditionalQty]").val();
        }
    }
      
    $("#divPOData").data("POData", POJSON);
}

function ClearTerms(type) {
    ////<summary>function used Clear Terms Details </summary>
    /// <param name="type"  type="Object">
    /// Determins the terms need to clear(Vendor,General)
    /// </param>
   

    if (type == "Vendor") {
        VendorTerms = new Array();
        $("[id$=VENDOR_TERMS]").val("0");
        $("[id$=POH_VENDOR_TERMS]").val("");
        $("[id$=LblPOH_VENDOR_TERMS]").html("");

    }
    else if (type == "General") {

        Terms = new Array();
        $("[id$=GENERAL_TERMS]").val("0");
        $("[id$=POH_TERMS]").val("");
        $("[id$=LblPOH_TERMS]").html("");
    }
    return false;
}

// #endregion

 ///#region ---Core Section-----

 function GridHandler(tr, command) {
     ////<summary>function used handle Grid Events </summary>
     /// <param name="tr"  type="Object">
     /// Determins which session to validate if 1 po details 3 material details 
     /// </param>
     /// <param name="command"  type="string">
     /// Determins which command to handle
     /// </param>
     switch (command.toString().toUpperCase()) {
         case POCreation.Edit:
             FillDetails(tr);
             return false;
             break;
         case POCreation.Delete:
             materialID = GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, $(tr).parents("table:first").attr("id"));
             GrandScriptUtils.ShowModal(POCreation.DeleteConfirmationMessage,POCreation.ConfirmationMessage, POCreation.Delete,true);
             //DeleteDetails(tr);
             return false;
             break;
     }

 }

 function FillDetails(tr) {
     ////<summary>function used Fill the po Details Corresponding to a material selected</summary>
     
     var tableID = $(tr).parents("table:first").attr("id");
     $("input[id$=EditMaterial]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID));
     //For PR Details
     $("[id$=TaxPerPiece]").val(GrandGrid.Utilities.GetColumnValue(tr, "TaxPerPiece", tableID));
     $("[id$=MaterialPrice]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialRate, tableID));
     $("input[id$=PRMaterial]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID));
     $("input[id$=PRMaterialName]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITV_NAME", tableID));
     $("input[id$=PRMaterialCode]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITM_CODE", tableID));
     $("input[id$=MaterialDiscount]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialDiscount, tableID));
     $("textarea[id$=MaterialRemarks]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.Remarks, tableID));
     $("input[id$=MaterailTotal]").val(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialQty, tableID));
    


     POJSON = $("#divPOData").data("POData");
//     //Means Its A Draft need to populate the Details of the Material
//     if (POJSON.POH_STATUS == 0) {
//         AddPRDetails(GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID));

//     }
//     else {
         var obj = new Object();
         for (var i in POJSON.MaterialDetails) {
             if (POJSON.MaterialDetails[i].POD_ITEM == GrandGrid.Utilities.GetColumnValue(tr, POCreation.MaterialID, tableID)) {
                 if (!($.isArray(POJSON.MaterialDetails[i].PRDetails))) {

                     if (POJSON.MaterialDetails[i].PRDetails != undefined) {
                         objArray = POJSON.MaterialDetails[i].PRDetails;
                         POJSON.MaterialDetails[i].PRDetails = new Array();
                         POJSON.MaterialDetails[i].PRDetails.push(objArray);
                     }
                     else {
                         objArray = POJSON.MaterialDetails[i].PRDetails;
                         POJSON.MaterialDetails[i].PRDetails = new Array();

                     }
                 }

                 obj = POJSON.MaterialDetails[i].PRDetails;
                 $("input[id$=AdditionalQty]").val(POJSON.MaterialDetails[i].AdditionalQty);
                 $("[id$=TaxPerPiece]").val(POJSON.MaterialDetails[i].TaxPerPiece);
                 $("[id$=MaterialUOMText]").text(POJSON.MaterialDetails[i].POD_UOM_TEXT);
                 $("input[id$=MaterialUOM]").val(POJSON.MaterialDetails[i].POD_UOM);
                 GrandGrid.MakeGrid($("#grdPRDetails"), 0, obj);
                 if (obj.length <= 0) {
                     if (PRtdset != "") {
                         $("#divPRData").hide();
                         $("#divDummyPR").show();
                         $(".tDummy").appendTo($("#dummyPR tbody"));
                         $("#grdPRDetails tbody").html("");

                     }
                     CalculatePRTotal();
                 }
                 //CalculateTaxDetails();
             }
         }
    // }

     $("#divPRDetails").dialog({ width: 850, height: 450, resizable: false, modal: true });
     $("#divPRDetails").dialog("open");
     $("#divPRDetails").css({ "min-width": "600", "margin-top": "25px" });

 }

 function DeleteDetails(tr) {
     ///<summary>Used to delete Materials From Po</summary>
     /// <param name="tr"  type="Object">
     ///  Specific Container and its controls       
     /// </param>  
     var POJSON = $("#divPOData").data("POData");
     //Loop through material details for Deleting the details of the selected material from the po object
     for (var i in POJSON.MaterialDetails) {
         if (POJSON.MaterialDetails[i].POD_ITEM == materialID) {
             //Will delete the Material Details
             POJSON.MaterialDetails.splice(i, 1);
             break;
         }
     }
    $("#divPOData").data("POData", POJSON);
    GrandGrid.MakeGrid($("#grdPODetails"), 0, POJSON.MaterialDetails);
    if (POJSON.MaterialDetails.length <= 0) {
         $("#poDetails").hide();
         
     }
     ClearMaterialDetails();
     CalculateTaxDetails();
     return false;
 }

 function ClearMaterialDetails() {
     ////<summary>function used Reset Material Details Control</summary>
     $("input[id$=EditMaterial]").val("");
     $("select[id$=POD_ITEM]").val("0");
     $("select[id$=POD_UOM]").html("");
     $("[id$=ITM_CODE]").val("0");
     $("input[id$=POD_CONV_FACT]").val("1");
     $("[id$=TAX_PERC]").html("");
     $("select[id$=POD_ITEM]").attr("disabled", false);
     //PRClearing
     $("input[id$=TaxPerPiece]").val("0.000");
     $("input[id$=AdditionalQty]").val("0.000");
     $("input[id$=MaterialDiscount]").val("0.000");
     $("input[id$=TotalQty]").val("0.000");
     $("input[id$=MaterialPrice]").val("0.000");
     $("input[id$=PerPiecePrice]").val("0.000");
     $("input[id$=MaterialTax]").val("0.000");
     $("input[id$=PRMaterial]").val("");
     $("input[id$=MaterailTotal]").val("0.000");
     //Disable ph Details
     $("input[id$=TotalQty]").attr("disabled", true);
     $("input[id$=MaterialTax]").attr("disabled", true);
     $("input[id$=MaterailTotal]").attr("disabled", true);
     $("[id$=imbSelect]").hide();

     $("input[id$=PRMaterialName]").val("");
     $("input[id$=PRMaterialCode]").val("");
     $("[id$=MaterialUOMText]").text("");
     $("input[id$=MaterialUOM]").val("");
     $("textarea[id$=MaterialRemarks]").val("");


     RemoveValidation();
 }

 function CalculateMaterialAmount() {
     ////<summary>function used calculate the Amout</summary>
     //Enable Controls Before Getting Value
     ChangeMode(2);
     var price = $("input[id$=POD_RATE]").val()=="" ? 0.000 : $("input[id$=POD_RATE]").val();
     var qty = $("input[id$=POD_QTY_REQUESTED]").val()=="" ? 0.000 : $("input[id$=POD_QTY_REQUESTED]").val();
     var discount = $("input[id$=POD_DISC_AMT]").val()=="" ? 0.000 : $("input[id$=POD_DISC_AMT]").val();
     var Conversion = $("input[id$=POD_CONV_FACT]").val()=="" ? 1.000: $("input[id$=POD_CONV_FACT]").val();
     var tax = $("[id$=TAX_PERC]").html();
     var result;
     var taxper;
     result = ((parseFloat(price) * ((1 / parseFloat(Conversion)) * parseFloat(qty))) - (parseFloat(discount)));
     taxper = parseFloat(result) * (tax / 100)
     result = result - taxper;
     if (isNaN(result)) {
        result = 0.000;
     }
    $("input[id$=POD_AMT_VALUE]").val(result.toFixed(3));
    //Enable input Controls after Getting Value
     ChangeMode(1);
 }

 function AfterGridBind(gridID) {
//<summary>function Call Afer binding Grid</summary>
     if (gridID == $("#grdPODetails").attr("id")) {
        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
         ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "POD_REMARKS", $("#grdPODetails").attr("id"));
         $("#grdPODetails").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
             if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                 $(this).find("td:eq(" + ColIndexremarks + ")").html("");
             }
         });

     //If GridID is Po Details
         if (tdset.length <= 1) {//tdset contains controls for add details.
         //Assigning the Td to a td set variable and adding it to Grid PO Details
             $("#dummytable").find("tr:has(td)").each(function () {
                 tdset.push(this);
                 $(this).insertAfter($("#grdPODetails").find("tr:last"));

             });
         }
         else {
         //Assigning the td set to grid PODetails
             for (var i in tdset) {
                 $(tdset[i]).insertAfter($("#grdPODetails").find("tr:last"));
             }

         }
         $("#poDetails").show();
         $("#dummydiv").hide();
         CalculateTax();
     }
     //If Grid ID is PRDetails
     if (gridID == $("#grdPRDetails").attr("id")) {
         //Assigning the td in the dummy table to Rtdset variable
         if (PRtdset =="") {
             Rtdset = $("#dummyPR").find("tr:has(td)");
         }
         //Inserting the td set from variable Rtdset to grid grdPRDetails
         if ($(Rtdset).length > 0) {
             $(Rtdset).insertAfter($("#grdPRDetails").find("tr:last"));
         }
         //Set the visibility of PR Details
         $("#divPRData").show();
         $("#divDummyPR").hide();

         if (PRtdset == "") {
             PRtdset = $("#dummyPR").find("tr:eq(1)");
         }
         $(PRtdset).insertBefore($("#grdPRDetails").find("tr:eq(1)"));
         
         //Assigning the value ordered to the input in the template field OrderQty
         var count = 1;
         $("#grdPRDetails").find("tr:has(td)").each(function () {
             if (GrandGrid.Utilities.GetColumnValue($(this), "UOM_CONV_EXIST", gridID) == "0") {
                 type = $(this).find("td:last input").attr("typed")
                 if (type == "Orderqty") {
                     $(this).find("td:last input").attr("disabled", true);
                 }
             }
             else {
                 type = $(this).find("td:last input").attr("typed")
                 if (type == "Orderqty") {
                     $(this).find("td:last input").attr("id", "txtQty" + count);
                     $(this).find("td:last input").val($(this).find("td:eq(2)").html());
                     //$(this).find("td:last input").attr("onkeydown", "MakeNumeric(event);");
                     count++;
                 }
             }
         });
         //calculate the PR Total
         CalculatePRTotal();
         //Assigning the text Order Qty to Template 
         $("#grdPRDetails").find("th:last span").text("Translate(OrderQty)");
     }

 }

 function AssignTax() {
     ////<summary>function used Assign Different Taxes to the Text Boxby assigning it to an attribute Tax </summary>
     
     $.get(POCreation.GetActiveTax, function (data) {
         if (data.length > 0) {
             for (var i in data) {
                if(data[i].TAX_HEAD == POCreation.ShippingCost) {
                //Assigning shipping cost 
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
    ////<summary>function used validate each sections </summary>
    var Result = $("input[id$=POH_SUB_TOTAL]").val();
    var Discount = $("input[id$=POH_DISC_AMT]").val() == "" ? 0.000 : $("input[id$=POH_DISC_AMT]").val();
    var NetAmount = (Result - Discount); 
    if(isNaN(NetAmount))
        NetAmount = 0.000;
    $("input[id$=POH_NET_TOTAL]").val(NetAmount.toFixed(3));
    //Geting the tax Formula which is assigned as an attribute using the function assign
//    var shippingtax = $("input[id$=POH_SHIP_CHARGE]").attr("TAX");
//    var salesTaxrate = $("input[id$=POH_SALES_TAX_AMT]").attr("TAX");
//    var additionalTax = $("input[id$=POH_ADD_TAX_AMT]").attr("TAX");
//    shippingtax = shippingtax.replace("#SUBTOTAL#", NetAmount)
//    salesTaxrate = salesTaxrate.replace("#SUBTOTAL#", NetAmount)
//    additionalTax = additionalTax.replace("#SUBTOTAL#", NetAmount)
//    //Evaluating the formula with the total and assigning back to controls
//    $("input[id$=POH_SHIP_CHARGE]").val(eval(shippingtax).toFixed(3));
//    $("input[id$=POH_SALES_TAX_AMT]").val(eval(salesTaxrate).toFixed(3));
//    $("input[id$=POH_ADD_TAX_AMT]").val(eval(additionalTax).toFixed(3));
    //calculate total 
    CalculateTotal();
}

function CalculateTotal() {
    ////<summary>function used calculate the net total and total for the po </summary>
    
    var Discount = $("input[id$=POH_DISC_AMT]").val() == "" ? 0.000 : $("input[id$=POH_DISC_AMT]").val();
    var SubTotal = $("input[id$=POH_SUB_TOTAL]").val() == "" ? 0.000 : $("input[id$=POH_SUB_TOTAL]").val();
    var NetAmount = $("input[id$=POH_NET_TOTAL]").val() == "" ? 0.000 : $("input[id$=POH_NET_TOTAL]").val();
    NetAmount = (SubTotal - Discount);
    
    NetAmount = isNaN(NetAmount) ? "0.000" : NetAmount;
    $("input[id$=POH_NET_TOTAL]").val(parseFloat(NetAmount).toFixed(3));
    //var shippingtax = $("input[id$=POH_SHIP_CHARGE]").val() == "" ? 0.000 : $("input[id$=POH_SHIP_CHARGE]").val();
    //var salesTaxrate = $("input[id$=POH_SALES_TAX_AMT]").val() == "" ? 0.000 : $("input[id$=POH_SALES_TAX_AMT]").val();
    //var additionalTax = $("input[id$=POH_ADD_TAX_AMT]").val() == "" ? 0.000 : $("input[id$=POH_ADD_TAX_AMT]").val();
    var shippingtax = $("input[id$=POH_SHIP_CHARGE]").attr("TAX");
    var salesTaxrate = $("input[id$=POH_SALES_TAX_AMT]").attr("TAX");
    var additionalTax = $("input[id$=POH_ADD_TAX_AMT]").attr("TAX");
    shippingtax = shippingtax.replace("#SUBTOTAL#", NetAmount)
    salesTaxrate = salesTaxrate.replace("#SUBTOTAL#", NetAmount)
    additionalTax = additionalTax.replace("#SUBTOTAL#", NetAmount)
    
    $("input[id$=POH_SHIP_CHARGE]").val(eval(shippingtax).toFixed(3));
    $("input[id$=POH_SALES_TAX_AMT]").val(eval(salesTaxrate).toFixed(3));
    $("input[id$=POH_ADD_TAX_AMT]").val(eval(additionalTax).toFixed(3));

    shippingtax = $("input[id$=POH_SHIP_CHARGE]").val() == "" ? 0.000 : $("input[id$=POH_SHIP_CHARGE]").val();
    salesTaxrate = $("input[id$=POH_SALES_TAX_AMT]").val() == "" ? 0.000 : $("input[id$=POH_SALES_TAX_AMT]").val();
    additionalTax = $("input[id$=POH_ADD_TAX_AMT]").val() == "" ? 0.000 : $("input[id$=POH_ADD_TAX_AMT]").val();

    var Adjustment = $("input[id$=POH_PRICE_ADJUST]").val() == "" ? 0.000 : $("input[id$=POH_PRICE_ADJUST]").val();
    var POHTotal = (parseFloat(NetAmount) + parseFloat(shippingtax) + parseFloat(salesTaxrate) + parseFloat(additionalTax) + parseFloat(Adjustment)).toFixed(3);
    POHTotal = isNaN(POHTotal) ? "0.000" : POHTotal
    $("input[id$=POH_TOTAL_VALUE]").val(parseFloat(POHTotal).toFixed(3));

}

function ChangeMode(mode) {
    ////<summary>function used to change th mode of text box </summary>
    /// <param name="mode"  type="Object">
    /// if 1 the enable if 2 then disable need to enable before saving 
    /// </param>
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
    ////<summary>function used validate each sections </summary>
    /// <param name="command"  type="Object">
    /// Determins which type is it draft or need to go to workflow
    /// </param>

 AddValidations(1);
 if ($(document.forms[0]).valid()) {
     ChangeMode(2);
     POJSON = $("#divPOData").data("POData");
     POJSON.POH_VENDOR_TERMS = "";
     POJSON.POH_TERMS = "";
     //Handling Terms And Vendor Terms 
     for(var i in Terms) {
         if (POJSON.POH_TERMS != "") {
             POJSON.POH_TERMS += ","+ Terms[i];
         }
         else
             POJSON.POH_TERMS += Terms[i] ;
     }
     for (var i in VendorTerms) {
         if (POJSON.POH_VENDOR_TERMS != "") {
             POJSON.POH_VENDOR_TERMS += "," + VendorTerms[i];
         }
         else
             POJSON.POH_VENDOR_TERMS += VendorTerms[i] ;
     }

     $("[id$=POH_TERMS]").val(POJSON.POH_TERMS);
     $("[id$=POH_VENDOR_TERMS]").val(POJSON.POH_VENDOR_TERMS);

     if (POJSON.MaterialDetails.length > 0){
     $("[id$=MaterialDetails]").val(JSON.stringify(POJSON.MaterialDetails));

     // For File Upload---------------------------------------------------------------------
     var ObjFile = $("#divFileData").data("FileData");
     $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));

     if (command != "Draft") {
         $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
         $("[id$=WrkfComment]").val($("[id$=PRDComments]").val());

     }
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
         }
         else {
             GrandScriptUtils.ShowModal(POCreation.ActionFailedMessage);

         }


     });
    }
    else {
        GrandScriptUtils.ShowModal(POCreation.MaterialRequired, POCreation.Information);
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
    ////<summary>function used Reset the page by redircting to listing page</summary>
    window.location = POCreation.PoListing;
    return false;
}

///#endregion

///#region------ Validations ----------------
function AddValidations(mode) {
    ////<summary>function used validate each sections </summary>
    /// <param name="mode"  type="Object">
    /// Determins which session to validate if 1 po details 3 material details 
    /// </param>
   
   RemoveValidation()
   if (mode == 1) {
       $("input[id$=POH_DATE]").rules("add", {
           required: true,
           maxlength: 11,
           messages: { required: "Translate(ReqRequredDate)" },
           messages: { Date: "Translate(Entervaliddate)" }
       });

       $("select[id$=POH_VENDOR]").rules("add", {
           selectNone: true,
           messages: { selectNone: "Translate(ReqVendorDetail)" }
       });

       $("select[id$=POH_SHIPPING]").rules("add", {
           selectNone: true,
           messages: { selectNone: "Translate(ReqShippingDetail)" }
       });

       $("select[id$=POH_BILLING]").rules("add", {
           selectNone: true,
           messages: { selectNone: "Translate(ReqBillingDetail)" }
       });

       $("select[id$=POH_DEPT]").rules("add", {
           selectNone: true,
           messages: { selectNone: "Translate(SelectDepartment)" }
       });


       $("input[id$=POH_DISC_AMT]").rules("add", {
           ZeroDecimal: true

       });

       $("input[id$=POH_PRICE_ADJUST]").rules("add", {
           ZeroDecimal: true
       });


   }

  //validate Individual MaterialDetails in popup
   else if (mode == 3) {
       $("input[id$=TotalQty]").attr("disabled", false);
       $("input[id$=MaterialTax]").attr("disabled", false);
       $("input[id$=MaterailTotal]").attr("disabled", false);

       $("input[id$=AdditionalQty]").rules("add", {
           ZeroDecimal: true
       });

       $("input[id$=TotalQty]").rules("add", {
           ThreeDecimal: true
       });

       $("input[id$=MaterialPrice]").rules("add", {
           ThreeDecimal: true
       });

       $("input[id$=MaterialDiscount]").rules("add", {
           ZeroDecimal: true
       });
       $("input[id$=MaterialTax]").rules("add", {
           ZeroDecimal: true
       });
       $("input[id$=MaterailTotal]").rules("add", {
           ZeroDecimal: true
       });
       $("[id$=MaterialRemarks]").rules("add", {
           maxlength: 200
       });

   }




}

function RemoveAllValidations() {
    ////<summary>function used Remove validation before Upload the File </summary>
    RemoveValidation();
}

function RemoveValidation() {
    ////<summary>function used Remove validation </summary>
    $("input[id$=POH_DATE]").rules("remove");
    $("select[id$=POH_VENDOR]").rules("remove");
    $("select[id$=POH_SHIPPING]").rules("remove");
    $("select[id$=POH_BILLING]").rules("remove");
    $("select[id$=POH_DEPT]").rules("remove");
    $("select[id$=POD_ITEM]").rules("remove");
    $("input[id$=POH_DISC_AMT]").rules("remove");
    $("input[id$=POH_PRICE_ADJUST]").rules("remove");
    
    
}

///#endregion