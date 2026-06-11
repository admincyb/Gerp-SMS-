<%@ Page Title="<%$ Resources:Captions,Title_POCrDrNote %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DebitCreditNote.aspx.cs" Inherits="ERPSMS_v01.CrDrNote.DebitCreditNote"
    ValidateRequest="false" Theme="ClassicExt" %>

<%--Theme="Classic"--%>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitVendorAutoComplete() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url + "?IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val(), "hdfVendorID", true, true, "VENDOR");
        }
        function InitComponents() {
            InitVendorAutoComplete();
            $("[id$='chkAffectStkHdr']").attr("disabled", true);
            $("[id$='chkAffectStkSplit']").attr("disabled", true); 
            if ($("[id$=ddlMode]").val() > 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "?Type=" + $("[id$=ddlMode]").val(), "hdfBank", true, true, "BANK");
            }
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtInstrumentDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCrDrNumber", url, "hdfCrDrNumber", true, true, "CRDRNUMBERPUR");
            GrandScriptUtils.AddDateRangeCommon("txtSearchDateFrom", "hdfSearchDateFrom", "txtSearchDateTo", "hdfSearchDateTo", false, false);

            GrandScriptUtils.DatePickerCommon("txtPVDate");
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            $("[id*=txtExchangeRate]").ForceNumericOnly();
            $("[id*=txtNoteFor]").ForceNumericOnly();
            $("[id*=txtTAXSplitTotal]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();


            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            
            //End
        }
        function pageLoad(sender, args) {
            InitVendorAutoComplete();
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            }
            else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
            }
            else if (containerID == "[id$=divTaxSplitupLIneItem]") {
                //  Showing Splitup Popup
                CalculateAmount();
                CalculateTotalSplit();
                //ShowContainerDiv('[id$=divCrDrSplitUp]', '<%= GetLocalResourceObject("ReceiptSplit").ToString()%>', '900', '300');
                ShowDrCrSplit();
            }
        }

        function ShowDrCrSplit() {
            ShowContainerDiv('[id$=divCrDrSplitUp]', '<%= GetLocalResourceObject("ReceiptSplit").ToString()%>', '1200', '300');
        }
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=ddlCompany]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ddlCompany]").show();
            }
            return false;
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=btnListPrint]").hide();
            }
        }

        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function EnableAuto(extender) {
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }

        function CalcOtherChargeTax() {
            $("[id$=btnCalcOtherTax]").click();
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtBank") {
                $("[id$=btnAccountNo]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }

        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtBank") {
                $("[id$=btnAccountNo]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
            if (targetControlID == "txtCrDrNumber") {
                $("[id$=hdfCrDrNumber]").val("0");
                $("[id$=txtCrDrNumber]").val("<%= Resources.Messages.AutoDefaultValue %>");
            }
        }
        function CalculateAmount(sender) {
            var netamount = 0;
            var amount = 0;
            var DecimalDigits = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var ExchangeRate = 0;
            var grndTotalTax = 0;
            var discount = 0;
            var discountAmount = 0;
            var discountPer = 0;



            var CurrencyFormat = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyFormat]").val()))) {
                CurrencyFormat = parseFloat($("#[id*=hdfCurrencyFormat]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfExchangeCurr]").val()))) {
                ExchangeRate = parseFloat($("#[id*=hdfExchangeCurr]").val());
            }
            //$("#[id*=grdDCSplit] input[type=text][id*=txtQtySplit]").each(function (index) {
            var qty = 0;
            var rate = 0;
            //                if ($.trim($(sender).val()) != "") {
            //                    //Check if number is a valid integer
            //                    if (!isNaN(parseFloat($(sender).val()))) {

            //                        qty = parseFloat($(sender).val());
            //                    }
            //                }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtQtySplit]").val()))) {
                qty = Number($(sender).closest('tr').find("#[id*=txtQtySplit]").val().replace(/[^0-9\.]+/g, ""));
                qty = parseFloat(qty);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtRateSplit]").val()))) {
                rate = Number($(sender).closest('tr').find("#[id*=txtRateSplit]").val().replace(/[^0-9\.]+/g, ""));
                rate = parseFloat(rate);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val().replace(/[^0-9\.]+/g, ""));
                hdftax = parseFloat(number);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfNETSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfNETSplit]").val().replace(/[^0-9\.]+/g, ""));
                netamount = parseFloat(number);
            }

            if (!isNaN(qty) && !isNaN(rate)) {
                amount = qty * rate;
            }
            else {
                amount = 0;
            }
            if ($("[id$=hdfisDiscountAdd]").val() == "1" && (qty <= 0 || rate<=0) ) {
                $(sender).closest('tr').find("#[id*=txtTotalSplit]").val(toFixed("0.0", DecimalDigits));
                return;
            }
            if (((amount.toFixed(DecimalDigits)).length) <= 15) {
                if (amount > 0) {
                    $(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed(amount, DecimalDigits));
                }
                else {
                    if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtSumSplit]").val()))) {
                        var number = Number($(sender).closest('tr').find("#[id*=txtSumSplit]").val().replace(/[^0-9\.]+/g, ""));
                        amount = parseFloat(number);
                    }
                }

                taxpercentage = hdftax / (netamount == 0 ? 1 : netamount);
                //tamt = parseFloat($(sender).val());
                //basevalue = (amount) / (1 + taxpercentage);
                //ttaxamt = (amount - basevalue);
                ttaxamt = (amount * taxpercentage);
                grndTotalTax = grndTotalTax + ttaxamt;
                //                    $(sender).closest('tr').find("#[id*=lblTAXSplitTotal]").text(ttaxamt.toFixed(NumberDigits));
                //                    $(sender).closest('tr').find("#[id*=lblTAXSplitTotal]").attr("title", ttaxamt.toFixed(NumberDigits));
                //                    $(sender).closest('tr').find("#[id*=lbnTAXSplitTotal]").text(ttaxamt.toFixed(DecimalDigits));
                //                    $(sender).closest('tr').find("#[id*=lbnTAXSplitTotal]").attr("title", ttaxamt.toFixed(DecimalDigits));
                $(sender).closest('tr').find("#[id*=txtTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));
                $(sender).closest('tr').find("#[id*=hdfTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));

                //DiscountCalculation
                if ($("[id$=hdfisDiscountAdd]").val() == "1") {
                    if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfActualDiscountSplit]").val()))) {
                        discount = Number($(sender).closest('tr').find("#[id*=hdfActualDiscountSplit]").val().replace(/[^0-9\.]+/g, ""));
                        discount = parseFloat(discount);
                    }
                    discountPer = discount / (netamount == 0 ? 1 : netamount);
                    discountAmount = (amount * discountPer);
                    // discountAmount = (amount - discountAmount + ttaxamt);
                    $(sender).closest('tr').find("#[id*=txtDiscountSplit]").val(toFixed(discountAmount, DecimalDigits));

                    $(sender).closest('tr').find("#[id*=txtTotalSplit]").val(toFixed(amount, DecimalDigits));
                    $(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed((amount - discountAmount + ttaxamt), DecimalDigits));
                    $(sender).closest('tr').find("#[id*=hdfSumSplit]").val(toFixed((amount - discountAmount + ttaxamt), DecimalDigits));

                }
                //  
                CalculateTotalSplit($(sender).closest('tr').find("#[id*=txtSumSplit]").val());


            }
            //If Qty is zero, uncheck and disable stock affect checkbox
            if (qty == 0) {
                $(sender).closest('tr').find("#[id*=chkAffectStkSplit]").removeAttr("checked");
                var grid = $(sender).closest("table");
                var chkHeader = $("[id*=chkAffectStkHdr]", grid);
                $("td", $(sender).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            }
            else {  
                var invGroup = parseInt($("#[id*=hdfInvGroup]").val())
                if (invGroup != 3 && invGroup !=2) { // 3:Expense invoice ,2:Service Invoice
                    $(sender).closest('tr').find("#[id*=chkAffectStkSplit]").removeAttr("disabled");
                }
            }
            //});
            CalculateTotalTaxSplit();
            CalculateTotalDiscountSplit();
            //$("#[id*=grdDCSplit] [id*=lblTotalTaxFooterSplit]").html(grndTotalTax.toFixed(DecimalDigits));
            // $("#[id$=hdfTotalTaxFooterSplit]").val(grndTotalTax.toFixed(DecimalDigits));
        }

        function CalculateTaxAmount(sender) {
            var netamount = 0;
            var amount = 0;
            var DecimalDigits = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var ExchangeRate = 0;
            var grndTotalTax = 0;
            var discount = 0;
            var discountPer = 0;
            var discountAmount = 0;
            var sumSplitOld = 0;

            var CurrencyFormat = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyFormat]").val()))) {
                CurrencyFormat = parseFloat($("#[id*=hdfCurrencyFormat]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfExchangeCurr]").val()))) {
                ExchangeRate = parseFloat($("#[id*=hdfExchangeCurr]").val());
            }


            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfTAXSplit]").val().replace(/[^0-9\.]+/g, ""));
                hdftax = parseFloat(number);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfNETSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfNETSplit]").val().replace(/[^0-9\.]+/g, ""));
                netamount = parseFloat(number);
            }

            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtSumSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=txtSumSplit]").val().replace(/[^0-9\.]+/g, ""));
                amount = parseFloat(number);
            }
            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfSumSplit]").val()))) {
                var number = Number($(sender).closest('tr').find("#[id*=hdfSumSplit]").val().replace(/[^0-9\.]+/g, ""));
                sumSplitOld = parseFloat(number);
            }
            if (sumSplitOld == amount)
                return;

            //            if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=txtDiscountSplit]").val()))) {
            //                var number = Number($(sender).closest('tr').find("#[id*=txtDiscountSplit]").val().replace(/[^0-9\.]+/g, ""));
            //                discount = parseFloat(number);
            //            }



            if (((amount.toFixed(DecimalDigits)).length) <= 15) {
                $(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed(amount, DecimalDigits));
                taxpercentage = hdftax / (netamount == 0 ? 1 : netamount);
                //tamt = parseFloat($(sender).val());
                //basevalue = (amount) / (1 + taxpercentage);
                //ttaxamt = (amount - basevalue);
                ttaxamt = (amount * taxpercentage);
                //                    $(sender).closest('tr').find("#[id*=lblTAXSplitTotal]").text(ttaxamt.toFixed(NumberDigits));
                //                    $(sender).closest('tr').find("#[id*=lblTAXSplitTotal]").attr("title", ttaxamt.toFixed(NumberDigits));
                //                    $(sender).closest('tr').find("#[id*=lbnTAXSplitTotal]").text(ttaxamt.toFixed(DecimalDigits));
                //                    $(sender).closest('tr').find("#[id*=lbnTAXSplitTotal]").attr("title", ttaxamt.toFixed(DecimalDigits));
                $(sender).closest('tr').find("#[id*=txtTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));
                $(sender).closest('tr').find("#[id*=hdfTAXSplitTotal]").val(toFixed(ttaxamt, DecimalDigits));

                //DiscountCalculation
                if ($("[id$=hdfisDiscountAdd]").val() == "1") {
                    $(sender).closest('tr').find("#[id*=txtQtySplit]").val(toFixed("0.00", DecimalDigits));
                    if (!isNaN(parseFloat($(sender).closest('tr').find("#[id*=hdfActualDiscountSplit]").val()))) {
                        discount = Number($(sender).closest('tr').find("#[id*=hdfActualDiscountSplit]").val().replace(/[^0-9\.]+/g, ""));
                        discount = parseFloat(discount);
                    }
                    discountPer = discount / (netamount == 0 ? 1 : netamount);
                    discountAmount = (amount * discountPer);

                    // discountAmount = (amount - discountAmount + ttaxamt);
                    $(sender).closest('tr').find("#[id*=txtDiscountSplit]").val(toFixed(discountAmount, DecimalDigits));
                    //$(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed((amount - discountAmount + ttaxamt), DecimalDigits));
                    $(sender).closest('tr').find("#[id*=txtSumSplit]").val(toFixed((amount), DecimalDigits));
                    $(sender).closest('tr').find("#[id*=hdfSumSplit]").val(toFixed((amount), DecimalDigits));

                }

                CalculateTotalSplit($(sender).closest('tr').find("#[id*=txtSumSplit]").val());

            }
            CalculateTotalTaxSplit();
            CalculateTotalDiscountSplit();
        }


        function CalculateTotalSplit(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0.00;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var CurrencyFormat = 0;

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyFormat]").val()))) {
                CurrencyFormat = parseFloat($("#[id*=hdfCurrencyFormat]").val());
            }

            $("#[id*=grdDCSplit] input[type=text][id*=txtSumSplit]").each(function (index) {

                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //   Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {

                        $(this).parent("td").find('input[type=hidden][id$=hdfPayNowSplit]').val($(this).val());
                        Amount = Amount + parseFloat($(this).val());
                    }
                }
            });

            $("#[id*=grdDCSplit] [id*=lblTotalPayNowFooterSplit]").html(toFixed(Amount, DecimalDigits));
            $("#[id*=grdDCSplit] [id*=hdfTotalPayNowFooterSplit]").val(toFixed(Amount, DecimalDigits));
        }
        function RaiseNoteChange(sender) {
            var itemindex = 0;
            itemindex = $(sender).closest('tr').find('[id*=hdfInvRowIndex]').val();
            $("#[id*=hdfRowIndex]").val(itemindex);
            $("[id$=btnRaiseNote]").click();
        }
        function CalculateTotal(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var taxpercentage = 0;
            var basevalue = 0;
            var ttaxamt = 0;
            var hdftax = 0;
            var hdftotalamt = 0;
            var TotalTaxFooter = 0;
            var TotalPayNowFooter = 0;
            var ItemIncluded = 0;
            var IsDetailTaxExist = 0;

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }

            $("#[id*=grdInvoiceList] input[type=text][id*=txtNoteFor]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find('.BalancetoPay').text()))) {
                    var number = Number($(this).closest('tr').find('.BalancetoPay').text().replace(/[^0-9\.]+/g, ""));
                    BalancetoPay = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTaxAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTaxAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftax = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalAmt]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfTotalAmt]").val().replace(/[^0-9\.]+/g, ""));
                    hdftotalamt = parseFloat(number);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfItemIncluded]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfItemIncluded]").val().replace(/[^0-9\.]+/g, ""));
                    ItemIncluded = parseInt(number);
                }

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfIsDetailTaxExist]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfIsDetailTaxExist]").val().replace(/[^0-9\.]+/g, ""));
                    IsDetailTaxExist = parseInt(number);
                }

                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        //                        if (BalancetoPay < parseFloat($(this).val())) {
                        //                            $("[id$=litErrorMsg]").show();
                        //                            $("[id$=litErrorMsg]").html("Paid amount should be less than or equal to Balance to Pay");
                        //                            ShowErrorMessage($("#diverror").html());
                        //                            $(this).val('0');
                        //                        }
                        //                        else {

                        taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                        tamt = parseFloat($(this).val());

                        if (ItemIncluded == 1) {
                            ttaxamt = tamt * taxpercentage.toFixed(4);
                        }
                        else {
                            basevalue = (tamt) / (1 + taxpercentage);
                            ttaxamt = (tamt - basevalue);
                        }
                        //                        $(this).closest('tr').find("#[id*=lblTotalTax]").text(ttaxamt.toFixed(DecimalDigits));
                        //                        $(this).closest('tr').find("#[id*=lblTotalTax]").attr("title", ttaxamt.toFixed(DecimalDigits));
                        //                        $(this).closest('tr').find("#[id*=hdfTotalTax]").val(ttaxamt.toFixed(NumberDigits));

                        if (IsDetailTaxExist > 0) { // if detail tax exist then no need to calculate tax.Just apply from popup
                            if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfTotalTax]").val()))) {
                                var number = Number($(this).closest('tr').find("#[id*=hdfTotalTax]").val().replace(/[^0-9\.]+/g, ""));
                                ttaxamt = parseFloat(number);
                            }


                        }
                        else {
                            //Rounding issue 552.50 tax getting as 38.67 insted of 38.68
                            //                            $(this).closest('tr').find("#[id*=lbnTotalTax]").text(addCommas(ttaxamt.toFixed(DecimalDigits)));
                            //                            $(this).closest('tr').find("#[id*=lbnTotalTax]").attr("title", addCommas(ttaxamt.toFixed(DecimalDigits)));
                            //                            $(this).closest('tr').find("#[id*=hdfTotalTax]").val(ttaxamt.toFixed(DecimalDigits));
                            $(this).closest('tr').find("#[id*=lbnTotalTax]").text(addCommas(toFixed(ttaxamt, DecimalDigits)));
                            $(this).closest('tr').find("#[id*=lbnTotalTax]").attr("title", addCommas(toFixed(ttaxamt, DecimalDigits)));
                            $(this).closest('tr').find("#[id*=hdfTotalTax]").val(toFixed(ttaxamt, DecimalDigits));
                        }
                        Amount = Amount + parseFloat($(this).val());
                        TotalTaxFooter = TotalTaxFooter + ttaxamt;
                        //}
                    }
                }
            });

            var TotalTax = 0;
            var SubTotal = 0;
            var SplitCount = 0;
            if (!isNaN(parseInt($("#[id*=hdfSplitCount]").val()))) {
                SplitCount = parseInt($("#[id*=hdfSplitCount]").val());
            }

            if (!isNaN(TotalTaxFooter || !isNaN(Amount))) {
                if (SplitCount == 0) {
                    SubTotal = Amount - TotalTaxFooter;
                }
                else {
                    SubTotal = Amount;
                }
                $("[id$=txtSubTotal]").val(toFixed((SubTotal ), DecimalDigits)); // Calculation as per Thailand settings, dont change
                $("[id$=txtTax]").val(toFixed(TotalTaxFooter, DecimalDigits)); // Calculation as per Thailand settings, dont change
                var Totalamount = (SubTotal) + TotalTaxFooter;
                if (((Totalamount.toFixed(DecimalDigits)).length) <= 15) {
                    if (!isNaN(Totalamount)) {
                        $("[id$=txtHdrTotal]").val(toFixed(Totalamount, DecimalDigits));
                    }
                }
            }

            $("#[id*=grdInvoiceList] [id*=lblTotalPayNowFooter]").html(addCommas(toFixed(Amount, DecimalDigits)));
            $("#[id*=grdInvoiceList] [id*=lblTotalTaxFooter]").html(addCommas(toFixed(TotalTaxFooter, DecimalDigits)));
            //$("#[id*=grdInvoiceList] input[type=text][id*=lblTotalPayNowFooter]").val(Amount);
            $("#[id*=txtPaidAmount]").val(toFixed(Amount, DecimalDigits));
            $("#[id$=hdfTotalPayNowFooter]").val(toFixed(Amount, DecimalDigits));
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").html("");
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;

            }
        }
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate();

            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        //Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        //TAx Payable in MYR Div hide show
        function ShowHideTaxPayableOuter() {
            if ($("[id$=hdfIsTaxPayable]").val() == 1) {
                $("#divOuterTaxPayable").show();
            }
            else {
                $("#divOuterTaxPayable").hide();
            }
        }

        function HideTaxPayable() {
            //<summary>Function Used to Hide TaxPayable </summary>
            $("#imgHideTaxPayable").hide();
            $("#imgShowTaxPayable").show();
            $("#divTaxPayable").hide();
        }

        function ShowTaxPayable() {
            //<summary>Function Used to Show TaxPayable Panel </summary>
            $("#imgHideTaxPayable").show();
            $("#imgShowTaxPayable").hide();
            $("#divTaxPayable").show();
        }

        function RaiseNoteChanged() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("RaiseNoteNotTallied") %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnRaiseNote]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $("[id$=hdfIscontNo]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnRaiseNote]").click();
                        return false;
                    }
                }
            });
            return false;
        }

        function ValidateRaiseNote(sender, args) {
            var payNow = $(sender).closest('tr').find('[id*=txtNoteFor]').val();
            var gross = $(sender).closest('tr').find('[id*=lblGrossAmount]').text().replace(/[^0-9\.]+/g, "");
            var tax = $(sender).closest('tr').find('[id*=lblTax]').text().replace(/[^0-9\.]+/g, "");

            var pattern = new RegExp($(sender).closest('tr').find('[id*=vamNoteFor]')[0].validationexpression);
            var payNowAmt = parseFloat(payNow);
            var grossamnt = parseFloat(gross);
            var taxAmnt = parseFloat(tax);
            if (!pattern.test(payNow) || isNaN(payNowAmt) || (payNowAmt > (grossamnt + taxAmnt))) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }

        function CalculateTotalAmountSplit(sender) {
            var TotalAmountSplit = 0;
            $("#[id*=grdBalAmntSplitup] input[type=hidden][id*=hdfAmountSplit]").each(function (index) {
                TotalAmountSplit = TotalAmountSplit + parseFloat($(this).val());
            });
            $("#[id*=grdBalAmntSplitup] [id*=lblTotalAmountSplit]").html(addCommas(toFixed(TotalAmountSplit, CurrencyDigits)));

        }


        function addCommas(number) {
            var FormattedNumber = number;
            var curGroup1 = 3;
            var curGroup2 = 3;
            var NumericPart = "", LastNumericPart = "", DecimalPart = "";
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
                curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
                curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
            }

            DecimalPart = number.split('.')[1];
            (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
            NumericPart = number.split('.')[0];
            if (NumericPart.length > curGroup1) {
                LastNumericPart = NumericPart.substr(NumericPart.length - curGroup1, curGroup1);
                (LastNumericPart) ? LastNumericPart = "," + LastNumericPart : LastNumericPart = "";
            }
            if ((NumericPart.length - curGroup1) > 0) {
                NumericPart = NumericPart.substr(0, NumericPart.length - curGroup1);
                var pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
                var expression = new RegExp(pattern, "g");
                NumericPart = NumericPart.toString().replace(expression, ",");
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
            return FormattedNumber;
        }

        function ShowHideUploadDocDetails(flag) {
            //If flag then Show Upload Doc Details
            if (flag) {
                $("[id$=tblUploadDocDetails]").show();
                $("[id$=imbShowDetails]").hide();
                $("[id$=imbHideDetails]").show();
            }
            else {
                $("[id$=tblUploadDocDetails]").hide();
                $("[id$=imbShowDetails]").show();
                $("[id$=imbHideDetails]").hide();
            }
            return false;
        }
    </script>
    <script type="text/javascript">
        $("[id*=chkAffectStkHdr]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    if (!$(this).is(':disabled')) {//do not checked if it's disabled
                        $(this).attr("checked", "checked");
                        $("td", $(this).closest("tr")).addClass("selected");
                    }
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        $("[id*=chkAffectStkSplit]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkAffectStkHdr]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkAffectStkSplit]", grid).length == $("[id*=chkAffectStkSplit]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });


        function CalculateTotalTaxSplit() {
            var TotalTaxSplit = 0;
            $("#[id*=grdDCSplit] input[type=text][id*=txtTAXSplitTotal]").each(function (index) {
                if (!isNaN(parseFloat($(this).val()))) {
                    TotalTaxSplit = TotalTaxSplit + parseFloat($(this).val());
                }
            });
            $("#[id*=grdDCSplit] [id*=lblTotalTaxFooterSplit]").html(addCommas(toFixed(TotalTaxSplit, CurrencyDigits)));
            $("#[id*=grdDCSplit] [id*=hdfTotalTaxFooterSplit]").val(toFixed(TotalTaxSplit, CurrencyDigits));

        }

        function CalculateTotalDiscountSplit() {
            var TotalDiscountSplit = 0;
            $("#[id*=grdDCSplit] input[type=text][id*=txtDiscountSplit]").each(function (index) {
                if (!isNaN(parseFloat($(this).val()))) {
                    TotalDiscountSplit = TotalDiscountSplit + parseFloat($(this).val());
                }
            });
            $("#[id*=grdDCSplit] [id*=lblTotalDiscountFooterSplit]").html(addCommas(toFixed(TotalDiscountSplit, CurrencyDigits)));
            $("#[id*=grdDCSplit] [id*=hdfTotalDiscountFooterSplit]").val(toFixed(TotalDiscountSplit, CurrencyDigits));

        }

      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlManpowerAttendance">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="select-full-a margnbotm0" runat="server"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="50"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="13" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')" ValidationGroup="drcr"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="13"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')"
                                            ValidationGroup="drcr" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSaveDrcr" CommandName="SAVE" TabIndex="14" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('drcr')" ValidationGroup="drcr"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteDrcr" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="15" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="16" ID="btnListPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="17" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="14"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" />
                                    </li>
                                    <%--<li>
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="10" Text="<%$resources:Controls,Alert %>"
                                            Visible="false" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>--%>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li style="display: none">
                                        <asp:Button runat="server" TabIndex="18" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="61" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelDrCr %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelDrCr %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="19" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="20" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="21" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPO %>">
                            <asp:LinkButton runat="server" ID="lbnPOListing" Text="<%$resources:PageNameRes,PurchaseOrder %>"
                                TabIndex="1" CssClass="tab-inactive" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <%-- <li><span id="spnDirectPurchase" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnDirectPurchase" Text="<%$resources:PageNameRes,DirectPurchase %>"
                                TabIndex="22" CommandName="DIRECTPURCHASE" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                        <li><span id="spnInvoicing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnkInvoicing" Text="<%$resources:PageNameRes,Invoice %>"
                                TabIndex="2" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="INVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPOInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseInvoice %>">
                            <asp:LinkButton runat="server" ID="lbnPOInvoice" Text="<%$resources:PageNameRes,POInvoice %>"
                                TabIndex="3" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="POINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnExpenses" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowExpense %>">
                            <asp:LinkButton runat="server" ID="lbnExpenses" Text="<%$resources:PageNameRes,Expenses %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="4" OnClick="ActionHandler" CommandName="EXPENSES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPayment" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPayment %>">
                            <asp:LinkButton runat="server" ID="lnkPayment" Text="<%$resources:PageNameRes,Payment %>"
                                TabIndex="5" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="PAYMENT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="6" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CRDRNOTE"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAP %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountPayables %>"
                                TabIndex="7" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="ACPAYABLES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
                <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                TabIndex="8" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CREDITDEBITLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="9" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CREDITDEBITDETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblSearchDateFrom" Text="<%$ resources:FromDate %>"
                                                AssociatedControlID="txtSearchDateFrom"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateFrom" CssClass="input-small margnrgt0-7per"
                                                TabIndex="100" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateFrom" runat="server" />
                                            <asp:Label runat="server" ID="lblSearchDateTo" Text="<%$ resources:ToDate %>" AssociatedControlID="txtSearchDateTo"
                                                class="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSearchDateTo" CssClass="input-small" TabIndex="101"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSearchDateTo" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanySrch"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanySrch" runat="server" CssClass="select-small-a1" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblInvoiceNo" Text="<%$ resources:InvNo %>" AssociatedControlID="txtInvoiceNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceNo" CssClass="input-small-20-11-5 margnrgt0-8per"
                                                TabIndex="102"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-a-22-12"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a" TabIndex="103">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,NotPosted %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" MaxLength="100" TabIndex="104" CssClass="select-half margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorID" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPaymentNumber" runat="server" Text="<%$resources:CrDrNumber %>"
                                                AssociatedControlID="txtCrDrNumber"></asp:Label>
                                            <asp:TextBox ID="txtCrDrNumber" runat="server" CssClass="input-small-b margnbotm0"
                                                MaxLength="100" TabIndex="105"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCrDrNumber" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblCnDn" Text="<%$ resources:CreditDebitType%>" AssociatedControlID="ddlCreditDebitType"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:DropDownList ID="ddlCreditDebitType" runat="server" CssClass="select-small-a margnbotm0"
                                                TabIndex="106">
                                                <asp:ListItem Text="<%$ Resources:Captions,Select %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Debit %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Credit %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearchHdr" class="middle-lbl"></asp:Label>--%>
                                            <asp:ImageButton ID="btnSearchHdr" runat="server" Text="" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="107" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="108" CssClass="margntop2" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="clear-ext" Style="margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCrDbHdr" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfCrDrPk" Value='<%# Eval(Resources.DataFieldRes.CrDrPk) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("CDH_DEPT") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("CDH_IS_DELETED") %>' />
                                                <asp:HiddenField ID="hdfCrDrType" runat="server" Value='' />
                                                <asp:HiddenField ID="hdfInvoiceType" runat="server" Value='' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="<%$ resources:DataFieldRes,CrDrDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.CrDrDate, Resources.Constants.DateFormatGrid).ToString()  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CrDrDate, Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:No %>" SortExpression="<%$ resources:DataFieldRes,CrDrNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionId" runat="server" Text='<%# Eval(Resources.DataFieldRes.CrDrNo)==""?"[NEW]": Eval(Resources.DataFieldRes.CrDrNo)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CrDrNo)==""?"[NEW]": Eval(Resources.DataFieldRes.CrDrNo) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpName" CssClass="<%# Eval(Resources.DataFieldRes.CMP_LINE_COLOUR) %>"
                                                    runat="server" Text='<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo1 %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInvnos" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    Style="text-align: left!important;" CommandName="PRINTCRDRNOTE"></asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="hdfListinvPK" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfListInvType" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfListinvCategory" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfListGroup" Value="0" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Invdate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssuedTo %>" SortExpression="<%$ Resources:DataFieldRes,CrDrVendorPk%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName),50) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval(Resources.DataFieldRes.CrDrVendorPk) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CreditDebitType %>" SortExpression="<%$ resources:DataFieldRes,CrDrType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblModeofPayment" runat="server" Text='<%# Eval(Resources.DataFieldRes.CrDrType)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CrDrType)%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCreditDebitType" Value='<%# Eval(Resources.DataFieldRes.CrDrType).ToString()%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval(Resources.DataTableRes.CurrencyMst1+"."+Resources.DataFieldRes.CurrencyCode)  %>'
                                                    ToolTip='<%#Eval(Resources.DataTableRes.CurrencyMst1+"."+Resources.DataFieldRes.CurrencyCode)  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,CrDrAmt %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAmount" runat="server" Text=' <%# Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString() %>'
                                                    ToolTip=' <%# Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString() %>'></asp:Label>--%>
                                                <asp:Label ID="lblAmount" runat="server" Text=' <%# GetFormattedCurrencyWithSeperator(Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)) %>'
                                                    ToolTip=' <%# GetFormattedCurrencyWithSeperator(Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceAmount %>" SortExpression=""
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)) %>'></asp:Label>
                                                <asp:LinkButton ID="lnkBalanceAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperator(Math.Round((Convert.ToDecimal(Eval(Resources.DataFieldRes.CrDrAmt)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhShipCharge)) + Convert.ToDecimal(Eval(Resources.DataFieldRes.cdhOtherCharge))),System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)) %>'
                                                    OnClick="ActionHandler" CommandName="BALANCEAMOUNTSPLIT" CssClass="text-underline"
                                                    Visible="false"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.CrDrApproved) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.CrDrPosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%-- Visible='<%# Convert.ToInt16(Eval(Resources.DataFieldRes.AffectStock)) == 1 ? (Convert.ToInt32(Eval("CDH_STATUS")) > 0 ? true : false) : false%>' --%>
                                                <asp:Button ID="imgAffectStock" runat="server" OnClick="ActionHandler" CommandName="STOCKUPDATION"
                                                    ToolTip="<%$ resources:StockUpdate %>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblMode" Text="<%$ resources:Mode%>" AssociatedControlID="ddlMode"></asp:Label>
                                            <asp:DropDownList ID="ddlMode" runat="server" CssClass="input-small" TabIndex="10">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfMode" CssClass="star" SetFocusOnError="true" ValidationGroup="drcr"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlMode"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfMode2" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="splitMain" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Mode %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPaymentNo" runat="server" Text="<%$ resources:No%>" AssociatedControlID="lblPaymentNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDrCrNo" CssClass="input-small"></asp:Label>
                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date%>" AssociatedControlID="txtDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="input-small Uidate-picker" TabIndex="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrftDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:Vendor%>" AssociatedControlID="lblCustomerTxt"></asp:Label>
                                            <asp:Label runat="server" ID="lblCustomerTxt" CssClass="input-half" Style="width: 300px!important;"></asp:Label>
                                            <asp:HiddenField ID="hdfCusPK" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblInstrumentNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtInstrumentNo"></asp:Label>
                                            <asp:TextBox ID="txtInstrumentNo" runat="server" TabIndex="12" CssClass="input-small margnrgt1-5per"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="vrfInstrumentNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentNo"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InstrumentNo%>"></asp:RequiredFieldValidator>--%>
                                            <asp:Label runat="server" ID="lblInstrumentDate" Text="<%$ resources:RefDate%>" AssociatedControlID="txtInstrumentDate"
                                                CssClass="lbl-20-5perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInstrumentDate" CssClass="input-small Uidate-picker"
                                                TabIndex="13" onpaste="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="vrfInstrumentDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="receipt" EnableClientScript="true" runat="server" ControlToValidate="txtInstrumentDate"
                                                Display="Dynamic" Text="*" Enabled="false" ErrorMessage="<%$ resources:Err_InstrumentDate %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompanyView" Visible="false" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompanyView"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyView" Visible="false" Enabled="false" runat="server"
                                                CssClass="select-small-e-20-11-5">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <div style="display: none;">
                                                <asp:Label runat="server" ID="lblPaidAmount" Text="<%$ resources:Amount%>" AssociatedControlID="txtPaidAmount"></asp:Label>
                                                <asp:TextBox ID="txtPaidAmount" runat="server" TabIndex="11" MaxLength="17" CssClass="Uiinput-amount numeric"
                                                    Enabled="false"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfPaidAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtPaidAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaidAmount%>"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <asp:Label ID="lblPaymentCurrency" runat="server" Text="<%$resources:Currency %>"
                                                AssociatedControlID="txtPaymentCurrency"></asp:Label>
                                            <asp:TextBox ID="txtPaymentCurrency" Enabled="false" runat="server" MaxLength="3"
                                                TabIndex="16" CssClass="input-small  input-disabled"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentCurrency" runat="server" Value="" />
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:Controls,ExchangeRate %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" Enabled="true" CssClass="input-small numeric"
                                                AutoPostBack="true" OnTextChanged="ActionHandler" onkeypress="return validateRateFloatKeyPress(this,event);" />
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblDeclarationNo" Text="<%$ resources:DeclarationNo%>"
                                                AssociatedControlID="txtDeclarationNo"></asp:Label>
                                            <asp:TextBox ID="txtDeclarationNo" runat="server" TabIndex="14" CssClass="input-small"></asp:TextBox>
                                            <asp:Label ID="lblAffectStk" runat="server" Text="<%$ resources:Controls,AffectStock %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl" />
                                            <asp:CheckBox ID="chkAffectStock" runat="server" Enabled="false" CssClass="disable" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:HiddenField ID="hdfInvoiceCategory" runat="server" />
                                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
                                <asp:HiddenField ID="hdfVendorPK" runat="server" />
                                <asp:HiddenField ID="hdfVendorAccountNo" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceCurr" runat="server" />
                                <asp:HiddenField ID="hdfExchangeCurr" runat="server" />
                                <asp:HiddenField ID="hdfPOType" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceGroup" runat="server" />
                                <asp:HiddenField ID="hdfInvoiceType" runat="server" />
                                <asp:Button runat="server" ID="btnRaiseNote" OnClick="ActionHandler" CommandName="RAISENOTECHANGE"
                                    EnableTheming="false" Style="display: none;" />
                                <asp:GridView ID="grdInvoiceList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo1 %>" SortExpression="<%$ resources:DataFieldRes,InvoiceNo %>">
                                            <ItemTemplate>
                                               <asp:HiddenField ID="hdfTaxApplayValue" runat="server" Value="0" />
                                               <asp:HiddenField ID="hdfLineItemTax" runat="server" />
                                               <asp:HiddenField ID="hdfHdrTax" runat="server" />
                                               <asp:HiddenField ID="hdfTotalAmt" runat="server" />
                                               <asp:HiddenField ID="hdfDiscoutntTotalSplitApplay" runat="server" />
                                                <asp:HiddenField ID="hdfTaxAmt" runat="server" />
                                                <asp:HiddenField ID="hdfIsDetailTaxExist" runat="server" />
                                                <asp:HiddenField ID="hdfInvoicePK" runat="server" />
                                                <asp:HiddenField ID="hdfCrDbMpgPK" runat="server" />
                                                <asp:HiddenField ID="hdfhasjournalized" runat="server" />
                                                <asp:HiddenField ID="hdfInvoiceGrp" runat="server" />
                                               
                                                <asp:LinkButton ID="lnkInvoiceNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP"></asp:LinkButton>
                                                <%--<asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate1 %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false" HeaderText="<%$ resources:Vendor %>" SortExpression="<%$ Resources:DataFieldRes,POInvoiceVendorText%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorInv" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceGrossAmountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossAmount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceDiscountBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                      
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceTaxtBC %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                          <%-- Other Charges-------------------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" SortExpression="" ItemStyle-HorizontalAlign="Right" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtherAmount" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--End Other Charges---------------------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:InvAmt %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceNetAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paid %>" SortExpression="<%$ resources:DataFieldRes,POInvoicePaidAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaid" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AdjAmount %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdjAmount" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Baltopay %>" SortExpression="<%$ resources:DataFieldRes,POInvoiceBalAmount %>"
                                            ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBaltopay" CssClass="BalancetoPay" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:Total %>"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RaiseNoteFor %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfInvRowIndex" runat="server" Value='<%# Container.DataItemIndex %>' />
                                                <asp:TextBox ID="txtNoteFor" onkeyup="CalculateTotal(this);" onblur="RaiseNoteChange(this)"
                                                    Enabled="false" runat="server" CssClass="input-w70 numeric disabled" MaxLength="15"
                                                    TabIndex="8"></asp:TextBox>
                                                <asp:HiddenField ID="hdfItemIncluded" runat="server" Value="0" />
                                                <%--   <asp:RequiredFieldValidator ID="vrfNot" CssClass="star" SetFocusOnError="true" ValidationGroup="drcr"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtNoteFor" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_NotFor %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <cc1:AmountValidation ID="vamNoteFor" runat="server" ControlToValidate="txtNoteFor"
                                                    ErrorMessage="<%$ resources:Err_NotFor1 %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>
                                                <%--<cc1:AmountValidation ID="vamNoteFor1" runat="server" ControlToValidate="txtNoteFor"
                                                    ErrorMessage="<%$ resources:Err_NotFor1 %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="splitMain"></cc1:AmountValidation>--%>
                                                <asp:CustomValidator ID="vcmNoteFor" CssClass="star" SetFocusOnError="true" ClientValidationFunction="ValidateRaiseNote"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtNoteFor"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExcessRaiseNote %>"></asp:CustomValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooter"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkAllocation" runat="server" OnClick="ActionHandler" CommandName="DCDETAIL"
                                                    OnClientClick="javascript:ValidatePageNow('splitMain')" SkinID="allocation-icon"
                                                    ToolTip="Allocation" CommandArgument="PageAction_Entry" ValidationGroup="splitMain" /><%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblTotalTax" runat="server"></asp:Label>  CssClass="text-underline nomargin" --%>
                                                <asp:LinkButton ID="lbnTotalTax" runat="server" OnClick="ActionHandler" CommandName="TAXHEADERSPLITUP"></asp:LinkButton>
                                                <asp:HiddenField ID="hdfTotalTax" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalTaxFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OtherCharges %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOtherCharges" Enabled="true" runat="server" onblur="CalcOtherChargeTax();" CssClass="input-w70 numeric "
                                                    MaxLength="15" TabIndex="8"></asp:TextBox>
                                                <%--onkeyup="CalculateTotal(this);" onblur="RaiseNoteChange(this)"--%>
                                                <cc1:AmountValidation ID="vamForOther" runat="server" ControlToValidate="txtOtherCharges"
                                                    ErrorMessage="<%$ resources:Err_ForOther %>" NumberDigits="11" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalOtherFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    CommandName="REMOVE" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right; width: 90%;">
                                                <asp:Label runat="server" ID="lblSubTotal" Text="<%$ resources:SubTotal%>" AssociatedControlID="txtSubTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right;" class="btn-margin">
                                                <asp:TextBox ID="txtSubTotal" Text="0" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                    Enabled="false" TabIndex="17" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtShipCharge"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                <%--<cc1:AmountValidation ID="AmountValidation1" runat="server" ControlToValidate="txtSubTotal"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Subtotal %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>--%>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:TotalTax%>" AssociatedControlID="txtTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:TextBox ID="txtTax" Text="0" runat="server" CssClass="input-w80 numeric  input-disabled"
                                                    Enabled="false" TabIndex="17" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtShipCharge"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                <%--<cc1:AmountValidation ID="AmountValidation2" runat="server" ControlToValidate="txtTax"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Tax %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="drcr"></cc1:AmountValidation>--%>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" Text="0" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Description %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,400);" onkeyup="limitText(this,400);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRemarks" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="drcr" EnableClientScript="true" runat="server" ControlToValidate="txtRemarks"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Remarks%>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%---------------------------------Start TAX Payable in MYR-------------------------------------%>
                            <div id="divOuterTaxPayable">
                                <h1 class="search-colapse-normal">
                                    <%=Resources.Controls.TaxDetails %>
                                    <img id="imgShowTaxPayable" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                                        cursor: pointer" onclick="javascript:ShowTaxPayable();" />
                                    <img id="imgHideTaxPayable" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                                        onclick="javascript:HideTaxPayable();" />
                                </h1>
                                <div id="divTaxPayable" class="gridwrap">
                                    <asp:GridView runat="server" ID="grdTaxPayable" Width="100%" AllowSorting="false"
                                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxHdrCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxHdrName" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxRate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxRate" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_RATE")) == "0" ? string.Empty : Convert.ToString(Eval("CIT_TAX_RATE")+"%") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubTotal %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubTotal" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("CIT_TAX_CID_AMOUNT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TaxAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxAmountBC" runat="server" Text='<%# GetFormattedCurrencyWithSeperator(Convert.ToDouble(Eval("CIT_TAX_AMT")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" Wrap="false" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <%------------------------------- End TAX Payable in MYR--------------------------------------%>
                            <%---------------------------------Start TAX Splitup Popup-----------------------------------%>
                            <div id="divTaxSplitup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxSplitup" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? string.Empty : HttpUtility.HtmlDecode(Eval("CIT_TAX_CODE").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxCode" runat="server" Value='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                                    <ItemTemplate>                                                       
                                                                                                  
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End TAX Splitup Popup-------------------------------------%>
                            <%---------------------------------Start TAX SplitupLineItemwise Popup-----------------------------------%>
                            <div id="divTaxSplitupLIneItem" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxSplitupLineItem" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            PageSize="<%$ resources:PageSize%>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxCode" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_TAX_CODE")) == string.Empty ? Resources.Report.Custom :HttpUtility.HtmlDecode(Eval("CIT_TAX_CODE").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxCode" runat="server" Value='<%# Convert.ToString(Eval("CIT_TAX_CODE")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="<%$ resources:TaxType %>" Visible="false" >
                                                    <ItemTemplate>                                                       
                                                                                                  
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CIT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CIT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>'
                                                            ToolTip='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : HttpUtility.HtmlDecode(Eval("CIT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%# Convert.ToString(Eval("CIT_NAME")) == string.Empty ? Resources.Report.Custom : Eval("CIT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("CIT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End TAX SplitupLineItemwise Popup-------------------------------------%>
                            <%---------------------------------Start Balance Amount details Popup-----------------------------------%>
                            <div id="divBalAmntSplitup" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdBalAmntSplitup" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                                            OnRowDataBound="ActionHandler" ShowFooter="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrxNo" runat="server" Text='<%# Convert.ToString(Eval("TRX_NO")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("TRX_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("TRX_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                            ToolTip='<%# Eval("TRX_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxAmount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("TRX_AMOUNT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("TRX_AMOUNT")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAmountSplit" runat="server" Value='<%#Eval("TRX_AMOUNT") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalAmountSplit" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------------------End Balance Amount details Popup-------------------------------------%>
                            <%-- file upload start here--%>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Fileupload_Details").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideUploadDocDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideUploadDocDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" /></div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblUploadDocDetails">
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                    <div class="fileupload-main">
                                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="26" CssClass="margn-rgt0 upload-area" />
                                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">                                                        
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                                    <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEM" TabIndex="12" OnClick="ActionHandler"
                                                        OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                                        CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$resources:ErpRes,Add %>"
                                                        SkinID="btnInner-add" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="gridwrap">
                                                    <asp:GridView runat="server" ID="grdUploads" Width="95%" PageSize="<%$ resources:PageSize%>"
                                                        AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                        OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="8" EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                    <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>' ToolTip='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="92%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                                        target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'>
                                                                    </a>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                                        SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                        SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);" /><%--OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender"--%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- file upload end here--%>
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow></asp:Table><asp:HiddenField ID="hdfCrDrTrxNo" runat="server" Value="" />
                <%--------------Cr/Dr Split Start--------------------------------%>
                <div id="divCrDrSplitUp" style="display: none">
                    <div class="content-wrapper">
                        <asp:HiddenField ID="HiddenField1" runat="server" Value="3" />
                        <asp:HiddenField ID="HiddenField2" runat="server" Value="3" />
                        <asp:Panel runat="server" ID="Allocation_Section" CssClass="Button-container-popup">
                            <asp:Button ID="btnSaveDCSplit" runat="server" Text="<%$ resources:Controls,Apply %>"
                                ToolTip="<%$ resources:Controls,Apply %>" OnClick="ActionHandler" TabIndex="72"
                                CommandName="DCSPLITSAVE" SkinID="btnInner-add-dsd" OnClientClick="javascript:ValidatePageNow('split')"
                                ValidationGroup="split" CommandArgument="Allocation_Section" />
                        </asp:Panel>
                        <div class="detail-co3">
                            <div class="div3col-S">
                                <asp:Label ID="LabelDC" runat="server" Text="<%$resources:InvoiceNoDC %>" AssociatedControlID="lblDCSplitNo"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitNo" runat="server" CssClass="medium"></asp:Label><br /><asp:Label ID="Label6" runat="server" Text="<%$resources:AmountDC %>" AssociatedControlID="lblDCSplitAmount"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitAmount" runat="server" CssClass="medium"></asp:Label></div><div class="detail-co3-2">
                                <asp:Label ID="Label2" runat="server" Text="<%$resources:DateDC %>" AssociatedControlID="lblDCSplitDate"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitDate" runat="server" CssClass="medium"></asp:Label><div class=" clear">
                                </div>
                                <asp:Label ID="lblCustomerDC" runat="server" Text="<%$resources:VendorDC %>" AssociatedControlID="lblDCSplitSupplier"
                                    Font-Bold="true"></asp:Label><asp:Label ID="lblDCSplitSupplier" runat="server"></asp:Label><asp:Label ID="Label8" runat="server" Visible="false" Text="<%$resources:ReceivedDC %>"
                                    AssociatedControlID="lblDCSplitReceived"></asp:Label><asp:Label ID="lblDCSplitReceived" Visible="false" runat="server"></asp:Label><asp:Label ID="Label10" runat="server" Visible="false" Text="<%$resources:ReceiveNowDC %>"
                                    AssociatedControlID="lblDCSplitReceiveNow"></asp:Label><asp:Label ID="lblDCSplitReceiveNow" runat="server" Visible="false"></asp:Label><asp:Label ID="lbltaxSplitpopup" runat="server" Visible="false"></asp:Label></div><div class=" clear">
                            </div>
                        </div>
                        <div class="error" id="divErrorLabel" runat="server" visible="false">
                            <ul>
                                <li>
                                    <asp:Literal runat="server" ID="lblSplitErrorMessage" Text="<%$resources:error_allocation %>"></asp:Literal></li></ul></div><div class="gridwrap">
                            <asp:TableCell> <div class="gridwrap">
                                    <asp:GridView ID="grdDCSplit" runat="server" AutoGenerateColumns="False" Width="100%"
                                        PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate><Columns>
                                            <asp:TemplateField HeaderText="<%$resources:PRODUCT %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRODUCTSplit" runat="server"></asp:Label><asp:HiddenField ID="hdfDCSplitPK"
                                                        runat="server" />
                                                    <asp:HiddenField ID="hdfSOPK" runat="server" />
                                                    <asp:HiddenField ID="hdfInvCusDtlPK" runat="server" />
                                                    <asp:HiddenField ID="hdfDCTRXPK" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:PoInvNo %>" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPONo" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="16%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:DCQTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQTYSplit" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:RATE %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRATESplit" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Discount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualDiscountSplit" runat="server" CssClass="input-w80 numeric"></asp:Label><asp:HiddenField ID="hdfActualDiscountSplit" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <%--<FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDiscountFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalDiscountFooterSplit"  Value="0" />
                                                </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmountSplit" runat="server"></asp:Label><asp:HiddenField ID="hdfNETSplit"
                                                        runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:TAX %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfTAXSplit" runat="server" />
                                                    <asp:Label ID="lblTAXSplit" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:NET %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNETSplit" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:QTY %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQtySplit" runat="server" Width="13px" CssClass="small numeric"
                                                        onblur="CalculateAmount(this);" MaxLength="13" TabIndex="70"></asp:TextBox><asp:HiddenField
                                                            ID="hdfQtySplit" runat="server" />
                                                    <cc1:QuantityValidationP2P ID="vreQtySplit" runat="server" ControlToValidate="txtQtySplit"
                                                        NumberDigits="9" ErrorMessage="<%$ resources:Err_Invalid_Qty %>" Display="Dynamic"
                                                        Text="*" NonZero="false" EnableClientScript="true" CssClass="star" ValidationGroup="split"></cc1:QuantityValidationP2P>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:RATE %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRateSplit" TabIndex="71" runat="server" CssClass="input-w80 numeric"
                                                        onblur="CalculateAmount(this);" MaxLength="15"> </asp:TextBox><asp:HiddenField ID="hdfRateSplit"
                                                            runat="server" />
                                                    <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRateSplit"
                                                        DecimalDigits="8" ErrorMessage="<%$ resources:Err_Invalid_Rate %>" NumberDigits="10"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="split"
                                                        NonZero="false"></cc1:RateValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SUM %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTotalSplit" runat="server" CssClass="input-w80 numeric" Enabled="false"
                                                        MaxLength="15"> </asp:TextBox></ItemTemplate><ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <%-- <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPayNowFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalPayNowFooterSplit" />
                                                </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:NetTotal %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSumSplit" onblur="CalculateTotalSplit(this);CalculateTaxAmount(this);"
                                                        runat="server" CssClass="input-w80 numeric" MaxLength="15"> </asp:TextBox><asp:HiddenField
                                                            ID="hdfSumSplit" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPayNowFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalPayNowFooterSplit" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Discount %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDiscountSplit" onblur="CalculateTotalSplit(this);CalculateTotalDiscountSplit(this);"
                                                        runat="server" CssClass="input-w80 numeric" MaxLength="15"> </asp:TextBox><asp:HiddenField
                                                            ID="hdfDiscountSplit" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalDiscountFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalDiscountFooterSplit" Value="0" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:TAX %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblTAXSplitTotal" runat="server"></asp:Label>--%>
                                                    <%--<asp:LinkButton ID="lbnTAXSplitTotal" runat="server" Text='' CssClass="text-underline nomargin"
                                                        OnClick="ActionHandler" CommandName="TAXLINEITEMSPLITUP" ToolTip=''></asp:LinkButton>--%>
                                                    <asp:TextBox ID="txtTAXSplitTotal" TabIndex="71" runat="server" CssClass="input-w80 numeric"
                                                        MaxLength="15" onblur="CalculateTotalTaxSplit();"> </asp:TextBox><asp:HiddenField ID="hdfTAXSplitTotal" runat="server" />
                                                    <cc1:AmountValidation ID="vamTAXSplitTotal" runat="server" ControlToValidate="txtTAXSplitTotal"
                                                        ErrorMessage="<%$ resources:Err_Invalid_Tax %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="split"></cc1:AmountValidation>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" Wrap="false" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalTaxFooterSplit"></asp:Label><asp:HiddenField
                                                        runat="server" ID="hdfTotalTaxFooterSplit" Value="0" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAffectStkSplit" runat="server" ToolTip='<%$resources:Controls,AffectStock %>'>
                                                    </asp:CheckBox></ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAffectStkHdr" runat="server" ToolTip='<%$resources:Controls,AffectStock %>'>
                                                    </asp:CheckBox>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:TableCell></div></div></div><%--------------Cr/Dr Split End----------------------------------%><div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="drcr" runat="server" />
                    <asp:ValidationSummary ID="vsSplit" ValidationGroup="split" runat="server" />
                    <asp:ValidationSummary ID="vsSplitMain" ValidationGroup="splitMain" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                </div>
            </div>
            <div id="divScriptButtons">
                <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                    EnableTheming="false" Style="display: none" />
                <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                    EnableTheming="false" Style="display: none" />
            </div>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="drcr">
                </uc1:WorkflowUserComments>
            </div>
            <div style="display: none">
            <asp:Button ID="btnCalcOtherTax" runat="server" OnClick="ActionHandler" CommandName="OTHERCHARGETAX" />
            </div>
            <asp:HiddenField ID="hdfShipCharge" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShipchargeNew" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfJournalHeader" runat="server" />
            <asp:HiddenField runat="server" ID="hdfTotalPayNowFooter" />
            <asp:HiddenField ID="hdfIsTaxPayable" runat="server" Value="0" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSplitCount" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRowIndex" runat="server" />
            <asp:HiddenField runat="server" ID="IsTaxForOtherCharge" Value="0" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfInvGroup" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="hdfisTaxAdd" Value="0" />
            <asp:HiddenField runat="server" ID="hdfisDiscountAdd" Value="0" />
            <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
