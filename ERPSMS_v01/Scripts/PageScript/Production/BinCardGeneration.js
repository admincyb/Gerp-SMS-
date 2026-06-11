/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../MasterPage.js" />

///#region ------Global Variable-----
var BINID = 0;
var BINJSON = "";
var ctrlWeight;
var ctrlAvgWeight;
var ctrlResult;
var ctrlhdnField;
///#endregion

//#region -------Configuration Section-------
var BinCreation = {
//URLS
    GetShiftURL: "CommonManagement.do?Action=GetShift",
    GetPlanURL: "CommonManagement.do?Action=GetPlans",
    GetLinesURL: "CommonManagement.do?Action=GetLines",
    GetProductsURL: "CommonManagement.do?Action=GetProducts",
    GetCompoundBatchURL: "CommonManagement.do?Action=GetCompoundBatch",
    GetEmployeesURL: "CommonManagement.do?Action=GetEmployees",
    SaveBinCardURL:"BinCardGeneration.do?Action=CreateBinCard",
    GetProductDetails: "CommonManagement.do?Action=GetProductDetails&ProductID=",
    BinCardListUrl:"BinCardListing.aspx",
    //Message
    BinSaveMessage: "#BINNUMBER# Bincard Saved Successfully",
    Information: "Information",
    UpdationFailed: "Translate(UpdateFailedBinCardMsg)",
    ActionFailedMessage: "Action failed",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    //Command
    UpdationFail: "UPDATIONFAILE",
    SaveCommand:"SAVED"

}
//#endregion

///#region-----------------------------Initialization Section -------------------------
$(document).ready(function () {
    ///<summary>
    ///function invoked after all the controls been rendered
    ///</summary>
   //Function used to initialize the page details Filling the details corresponding to po
   PageInit();

});

$.validator.addMethod("Three3Decimal", function (value) {
    return /^\d{1,3}(\.\d{1,3})?$/.test(value);
}, "Max 3 numeric and 3 decimals allowed");

$.validator.addMethod('selectNone', function (value, element) {
    ///<summary>
    ///Add additional validation for select
    ///</summary>
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

//#endregion
//#region-------------------------------- Methord -------------------------------------



//#region ------------------------------- Fill Dropdowns--------------------------------
function FillShift() {
    ////<summary>function used to fill shift </summary>
     GrandScriptUtils.MakeAutoComplete("BCHTXT_SHIFT", BinCreation.GetShiftURL, "BCH_SHIFT",true, false,false,true, false);
//    $("[id$=BCHTXT_SHIFT]").val('--Select--');
//    $("[id$=BCH_SHIFT]").val('');

}
function FillPlan(planID) {
    ////<summary>function used to fill plan </summary>
    /// <param name="planID"  type="Object">
    /// Determins which plan need to be selected after filling plan
    /// </param>

    GrandScriptUtils.MakeAutoComplete("BCHTXT_PLAN", BinCreation.GetPlanURL, "BCH_PLAN", true, false,false, true, false);
//    $("[id$=BCHTXT_PLAN]").val('--Select--');
//    $("[id$=BCH_PLAN]").val('');
}
function FillLine(lineID) {
    ////<summary>function used to fill line details </summary>
    /// <param name="lineID"  type="Object">
    /// Determins which line need to be selected after filling line
    /// </param>

    GrandScriptUtils.MakeAutoComplete("BCHTXT_LINE", BinCreation.GetLinesURL, "BCH_LINE", true, false,false,true, false);
//    $("[id$=BCHTXT_LINE]").val('--Select--');
//    $("[id$=BCH_LINE]").val('');
}
function FillProduct(productID) {
    ////<summary>function used to fill product details </summary>
    /// <param name="lineID"  type="Object">
    /// Determins which product need to be selected after filling product
    /// </param>
   
    GrandScriptUtils.MakeAutoComplete("BCHTXT_PRODUCT", BinCreation.GetProductsURL, "BCH_PRODUCT", true, false,false, true, false);
//    $("[id$=BCHTXT_PRODUCT]").val('--Select--');
//    $("[id$=BCH_PRODUCT]").val('');
}
function FillEmployees(empID) {
    ////<summary>function used to fill Eployee details </summary>
    /// <param name="empID"  type="Object">
    /// Determins which employee need to be selected after filling Employee
    /// </param>
    GrandScriptUtils.MakeAutoComplete("BCHTXT_INSP_BY", BinCreation.GetEmployeesURL, "BCH_INSP_BY", true, false,false, true, false);
//    $("[id$=BCHTXT_INSP_BY]").val('--Select--');
//    $("[id$=BCH_INSP_BY]").val('');

}

function FillCompound() {
    ////<summary>function used to fill Eployee details </summary>
    /// <param name="empID"  type="Object">
    /// Determins which employee need to be selected after filling Employee
    /// </param>
    GrandScriptUtils.MakeAutoComplete("BCHTXT_COMP_BATCH", BinCreation.GetCompoundBatchURL, "BCH_COMP_BATCH", true, false,false, true, false);
//    $("[id$=BCHTXT_COMP_BATCH]").val('--Select--');
//    $("[id$=BCH_COMP_BATCH]").val('');
    
}

//#endregion

//#region --------------------- Manipulation Functions ----------------------------------

function MakeNumeric(event){
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}

function SavePage() {
AddValidations(1);
if ($(document.forms[0]).valid()) {
    var jSonString = GrandScriptUtils.FormToJsonString();
    $.post(BinCreation.SaveBinCardURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
        if (data != "" && data != "-1") {
            var msg = BinCreation.BinSaveMessage.replace('#BINNUMBER#', data);
            GrandScriptUtils.ShowModal(msg, BinCreation.Information, BinCreation.SaveCommand);
            
        }
        else if (data == "-1") {
            GrandScriptUtils.ShowModal(BinCreation.ActionFailedMessage);
        }
        else if (data == "-2") {
            GrandScriptUtils.ShowModal(BinCreation.UpdationFailed + " " + $("[id$=BCH_NO]").html() + " " + BinCreation.EditUsedByAnotherUser, BinCreation.Information, BinCreation.UpdationFail);
        }
        else {
            GrandScriptUtils.ShowModal(BinCreation.ActionFailedMessage);

        }
    });
}
return false;

}

function PageInit() {
    //initialize PO Object And Store in divPOData
    BINJSON = $.parseJSON($("[id$=BCHDETAILs]").val());
    $("#divBINData").data("BinData", BINJSON);
    FillShift();
    FillPlan();
    FillLine();
    FillProduct();
    FillEmployees();
    FillCompound();
    $("#divtype").hide();
    $("#divsize").hide();
    GrandScriptUtils.DatePicker("BCH_INSP_DT", false, true);
    $("[id$=BCH_TUMB_ST_TM]").timepicker();

    if (BINJSON.BCH_PK == undefined || BINJSON.BCH_PK == 0) {
//        GrandScriptUtils.MakeAutoComplete("BCHTXT_PLAN", BinCreation.GetShiftURL, "BCH_PLAN", true, false, false, false);
//        GrandScriptUtils.MakeAutoComplete("BCHTXT_COMP_BATCH", BinCreation.GetShiftURL, "BCH_COMP_BATCH", true, false, false, false);
//        GrandScriptUtils.MakeAutoComplete("BCHTXT_PRODUCT", BinCreation.GetShiftURL, "BCH_PRODUCT", true, false, false, false);
//        GrandScriptUtils.MakeAutoComplete("BCHTXT_SHIFT", BinCreation.GetShiftURL, "BCH_SHIFT", true, false, false, false);
//        GrandScriptUtils.MakeAutoComplete("BCHTXT_LINE", BinCreation.GetShiftURL, "BCH_LINE", true, false, false, false);
//        GrandScriptUtils.MakeAutoComplete("BCHTXT_INSP_BY", BinCreation.GetShiftURL, "BCH_INSP_BY", true, false, false, false);
        
    }
    else {
        FillBinDetails(BINJSON);
    }
    $("[id$=BCHTXT_SHIFT]").focus();

}

function FillBinDetails(BINJSON) {
    $("[id$=lblBCH_NO]").text(BINJSON.BCH_NO);
    $("[id$=BCH_NO]").val(BINJSON.BCH_NO);
    $("[id$=BCH_PK]").val(BINJSON.BCH_PK);
    
    $("[id$=BCHTXT_PLAN]").val(BINJSON.BCHTXT_PLAN);
    $("[id$=BCH_PLAN]").val(BINJSON.BCH_PLAN);
   
    $("[id$=BCHTXT_COMP_BATCH]").val(BINJSON.BCHTXT_COMP_BATCH);
    $("[id$=BCH_COMP_BATCH]").val(BINJSON.BCH_COMP_BATCH);
    
    $("[id$=BCHTXT_PRODUCT]").val(BINJSON.BCHTXT_PRODUCT);
    $("[id$=BCH_PRODUCT]").val(BINJSON.BCH_PRODUCT);
    FillProductDetails(BINJSON.BCH_PRODUCT);
   
    $("[id$=BCHTXT_SHIFT]").val(BINJSON.BCHTXT_SHIFT);
    $("[id$=BCH_SHIFT]").val(BINJSON.BCH_SHIFT);
    $("[id$=BCH_PROD_BATCH]").val(BINJSON.BCH_PROD_BATCH);
    
    $("[id$=BCHTXT_LINE]").val(BINJSON.BCHTXT_LINE);
    $("[id$=BCH_LINE]").val(BINJSON.BCH_LINE);
    $("[id$=BCH_TUMB_ST_TM]").val(BINJSON.BCH_TUMB_ST_TM);
    $("[id$=BCH_TOT_WT]").val(BINJSON.BCH_TOT_WT);
    $("[id$=BCH_TOT_AVG_WT]").val(BINJSON.BCH_TOT_AVG_WT);
    $("[id$=TotalPcs]").text(BINJSON.BCH_TOT_PCS);
    $("[id$=BCH_TOT_PCS]").val(BINJSON.BCH_TOT_PCS);

    $("[id$=BCH_WT_A]").val(BINJSON.BCH_WT_A);
    $("[id$=BCH_AVG_WT_A]").val(BINJSON.BCH_AVG_WT_A);
    $("[id$=AGradePcs]").text(BINJSON.BCH_PCS_A);
    $("[id$=BCH_PCS_A]").val(BINJSON.BCH_PCS_A);


    $("[id$=BCH_WT_B]").val(BINJSON.BCH_WT_B);
    $("[id$=BCH_AVG_WT_B]").val(BINJSON.BCH_AVG_WT_B);
    $("[id$=BGradePcs]").text(BINJSON.BCH_PCS_B);
    $("[id$=BCH_PCS_B]").val(BINJSON.BCH_PCS_B);

    $("[id$=BCH_WT_C]").val(BINJSON.BCH_WT_C);
    $("[id$=BCH_AVG_WT_C]").val(BINJSON.BCH_AVG_WT_C);
    $("[id$=CGradePcs]").text(BINJSON.BCH_PCS_C);
    $("[id$=BCH_PCS_C]").val(BINJSON.BCH_PCS_C);
   
    $("[id$=BCHTXT_INSP_BY]").val(BINJSON.BCHTXT_INSP_BY);
    $("[id$=BCH_INSP_BY]").val(BINJSON.BCH_INSP_BY);
    $("[id$=BCH_INSP_DT]").val(BINJSON.BCH_INSP_DT);

    $("[id$=LAST_MOD_DT]").val(BINJSON.LAST_MOD_DT);
}

    function GetTotalPcs(txtWeight, txtAvgWeight, txtResult, hdnField,flag) {
        ////<summary>function used call after a data is picked from auto complete </summary>
        /// <param name="txtWeight"  type="String">
        ///Determins the weight Text Box
        /// </param>
        /// <param name="targetControlID"  type="Object">
        ///Determins the Avg weight Text Box always in gms
        /// </param>
        /// <param name="targetControlID"  type="Object">
        ///Determins the control wich picks a value
        /// </param>
        /// <param name="targetControlID"  type="Object">
        ///Determins the control wich picks a value
        /// </param>
        $("[id$=" + txtWeight + "]").rules("add", {
            Three3Decimal: true
            //messages: { selectNone: "Select plan" }

        });

        $("[id$=" + txtAvgWeight + "]").rules("add", {
            Three3Decimal: true
            //messages: { selectNone: "Select plan" }

        });

        if ($("[id$=" + txtWeight + "]").val() == "")
            $("[id$=" + txtWeight + "]").val("0.00");

        if ($("[id$=" + txtAvgWeight + "]").val() == "")
            $("[id$=" + txtAvgWeight + "]").val("0.00");

        if ($(document.forms[0]).valid())
        {
        var TotalNos = $("[id$=BCH_TOT_PCS]").val();
        var AGrade;
        var BGrade;
        var CGrade;
        var totABCPcs;
        var TotalBinWeight = $("[id$=BCH_TOT_PCS]").val()
        var BinWeight =0.000;
        var AvgWeight =0.000;
        var totPcs =0.000;
        BinWeight = parseFloat($("[id$=" + txtWeight + "]").val());
        AvgWeight = parseFloat($("[id$=" + txtAvgWeight + "]").val());
        //Converting the bin weight into gram
        BinWeight = (BinWeight * 1000);
        totPcs = (BinWeight / AvgWeight);
        if (isNaN(totPcs)||totPcs=="Infinity") {
            totPcs = 0.000;
        }
        if (!flag) {

           
          
                $("[id$=" + txtResult + "]").text(totPcs.toFixed(0))
                if (hdnField)
                    $("[id$=" + hdnField + "]").val(totPcs.toFixed(0))

                AGrade = $("[id$=BCH_PCS_A]").val() == "" ? "0" : $("[id$=BCH_PCS_A]").val();
                BGrade = $("[id$=BCH_PCS_B]").val() == "" ? "0" : $("[id$=BCH_PCS_B]").val();
                CGrade = $("[id$=BCH_PCS_C]").val() == "" ? "0" : $("[id$=BCH_PCS_C]").val();
                totABCPcs = parseInt(AGrade) + parseInt(BGrade) + parseInt(CGrade);
                if (isNaN(totABCPcs)) {
                    totABCPcs = 0.000;
                }

                if (TotalBinWeight >= totABCPcs) {

                }
                else {
                    //ctrlAvgWeight = txtAvgWeight;
                    //ctrlhdnField = hdnField;
                    //ctrlResult = txtResult;
                    //ctrlWeight = txtWeight;
                    $("[id$=" + txtAvgWeight + "]").val('0.00');
                    $("[id$=" + hdnField + "]").val('0.00');
                    $("[id$=" + txtResult + "]").text('0');
                    $("[id$=" + txtWeight + "]").val('0.00');
                    GrandScriptUtils.ShowModal("Total no cannot be greater than bin nos", "Information", "EXCESS", false);


            }
        }
        else {
            $("[id$=" + txtResult + "]").text(totPcs.toFixed(0))
            if (hdnField)
                $("[id$=" + hdnField + "]").val(totPcs.toFixed(0))


            AGrade = $("[id$=BCH_PCS_A]").val() == "" ? "0" : $("[id$=BCH_PCS_A]").val();
            BGrade = $("[id$=BCH_PCS_B]").val() == "" ? "0" : $("[id$=BCH_PCS_B]").val();
            CGrade = $("[id$=BCH_PCS_C]").val() == "" ? "0" : $("[id$=BCH_PCS_C]").val();
            totABCPcs = parseInt(AGrade) + parseInt(BGrade) + parseInt(CGrade);

            if (TotalBinWeight > totABCPcs) {
                $("[id$=BCH_WT_A]").val('0.000');
                $("[id$=BCH_WT_B]").val('0.000');
                $("[id$=BCH_WT_C]").val('0.000');
                $("[id$=BCH_AVG_WT_A]").val('0.000');
                $("[id$=BCH_AVG_WT_B]").val('0.000');
                $("[id$=BCH_AVG_WT_C]").val('0.000');
                $("[id$=BCH_AVG_WT_C]").val('0.000');
                $("[id$=AGradePcs]").text('0.000');
                $("[id$=BGradePcs]").text('0.000');
                $("[id$=CGradePcs]").text('0.000');
            }
            

        }
        }
        

    }

    function AfterAutoCompleteSelect(targetControlID) {
        ////<summary>function used call after a data is picked from auto complete </summary>
        /// <param name="targetControlID"  type="Object">
        ///Determins the control wich picks a value
        /// </param>
        if (targetControlID == "BCHTXT_PRODUCT")
        {
            FillProductDetails($("[id$=BCH_PRODUCT]").val());
        }


    }

    function FillProductDetails(productID) {
        ////<summary>function used Fill ProductDetails </summary>
        /// <param name="productID"  type="string">
        ///Determins the Product id
        /// </param>
        $.get(BinCreation.GetProductDetails + productID, function (data) {

            $("#divtype").show();
            $("#divsize").show();
            $("[id$=ProductType]").text(data[0].PRT_NAME);
            $("[id$=ProductSize]").text(data[0].PRO_SIZE);
        });
    }

    function ModalOk(command) {
        //<summary>Function invoke after Model popup ok Click</summary>

        switch (command) {
            case BinCreation.UpdationFail:
                window.location = BinCreation.BinCardListUrl;
                break;
            case "EXCESS":
                $("[id$=" + ctrlAvgWeight + "]").val('0.00');
                $("[id$=" + ctrlhdnField + "]").val('0.00');
                $("[id$=" + ctrlResult + "]").text('0');
                $("[id$=" + ctrlWeight + "]").val('0.00');
                break;
            case BinCreation.SaveCommand:
                window.location = BinCreation.BinCardListUrl;
                break;


        }
    }

    function ResetPage() {
        ////<summary>function used Reset the page </summary>
        //Here we are redirecting to listing page
        window.location = BinCreation.BinCardListUrl;
        return false;

    }

//#endregion


//#endregion

//#region------ Validations ----------------
function AddValidations(mode) {
    ////<summary>function used validate each sections </summary>
    /// <param name="mode"  type="Object">
    /// Determins which session to validate if 1 po details 3 material details 
    /// </param>
    $("input[id$=BCH_PLAN]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Select plan" }

    });
    $("input[id$=BCH_COMP_BATCH]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Select compound batch no" }

    });
    $("input[id$=BCH_PRODUCT]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Select product" }

    });
    $("input[id$=BCH_SHIFT]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Select shift" }

    });
    $("input[id$=BCH_PROD_BATCH]").rules("add", {
        required: true,
        messages: { required: "Enter production batch #" }

    });
    $("input[id$=BCH_LINE]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Select Line" }

    });
    $("input[id$=BCH_TUMB_ST_TM]").rules("add", {
        required: true,
        messages: { required: "Select tumbling start time" }

    });

    $("input[id$=BCH_TOT_WT]").rules("add", {
        required: true,
        messages: { required: "Enter total bin weight" }

    });

    $("input[id$=BCH_TOT_AVG_WT]").rules("add", {
        required: true,
        messages: { required: "Enter average glove weight" }

    });
    $("input[id$=BCH_INSP_DT]").rules("add", {
        required: true,
        date:true,
        messages: { required: "Select inspection date" }

    });
    $("input[id$=BCH_INSP_BY]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Select inspected by" }

    });
    $("input[id$=BCH_WT_A]").rules("add", {
        required: true,
        messages: { required: "Enter total A grade weight" }

    });

    $("input[id$=BCH_WT_B]").rules("add", {
        required: true,
        messages: { required: "Enter total B grade weight" }

    });
    $("input[id$=BCH_WT_C]").rules("add", {
        required: true,
        messages: { required: "Enter total Rejection weight'  " }

    });

    $("input[id$=BCH_AVG_WT_A]").rules("add", {
        required: true,
        messages: { required: "Enter average A grade weight" }

    });

    $("input[id$=BCH_AVG_WT_B]").rules("add", {
        required: true,
        messages: { required: "Enter average B grade weight" }

    });

    $("input[id$=BCH_AVG_WT_C]").rules("add", {
        required: true,
        messages: { required: "Enter average C grade weight" }

    });
    
    
    
    
}
//#endregion