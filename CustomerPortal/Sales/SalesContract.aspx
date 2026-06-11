<%@ Page Title="<%$ Resources:Title_SaleOrderDetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="SalesContract.aspx.cs" Inherits="CustomerPortal.Sales.SalesContract"
    ValidateRequest="false" Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        [class="ui-widget-overlay"] {
            position: fixed !important;
        }

        [aria-labelledby^="ui-dialog"] {
            position: fixed !important;
        }

        /* Sales Contract - Discount Percentage */
        .div2col-P .divpercentage {
            width: 132px;
            float: left;
        }

            .div2col-P .divpercentage .dvperc-lbl {
                width: 96px;
                margin-right: 0px !important;
            }

            .div2col-P .divpercentage .dvperc-inpt {
                width: 25px !important;
                margin-right: 0px !important;
            }
    </style>
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        var rateDigits = 0;
        var ExchangeRateDigits = 0;
        var CBPriceDecimalDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
            rateDigits = parseInt($("[id$=hdfRateDigits]").val());
            ExchangeRateDigits = parseInt($("[id$=hdfExchangeRateDigits]").val());
            CBPriceDecimalDigits = parseInt($("[id$=hdfCBPriceDecimals]").val());
            $("[id$=hdfPreviousUrl]").val(document.referrer);
            $("[id$=txtSaleOrderDate]").focus();
        });
        function InitDates() {
            GrandScriptUtils.DatePickerCommon("txtMfgDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtExpiryDate", false, false);
        }
        function InitComponents() {
            InitQuotation();
            GrandScriptUtils.DatePickerCommon("txtSaleOrderDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtRefDate");

            GrandScriptUtils.AddDateRangeCommon("txtBookingDate", "hdfBookingDate", "txtReqByDate", "hdfReqByDate", false, false, true);
            GrandScriptUtils.AddDateRangeCommon("txtBookingDate", "hdfBookingDate", "txtSpecReqByDate", "hdfSpecReqByDate", false, false, true);
            GrandScriptUtils.DatePickerCommon("txtPODate", false, false, false, false, false, "txtBookingDate", "txtReqByDate");

            if ($("[id$=hdfInitMfgDate]").val() == "1") {
                InitDates();
                $("[id$=txtMfgDate]").attr("onkeydown", "return CheckKey(event)");
                $("[id$=txtExpiryDate]").attr("onkeydown", "return CheckKey(event)");
            }
            else {
                $("[id$=txtMfgDate]").removeAttr("onkeydown");
                $("[id$=txtExpiryDate]").removeAttr("onkeydown");
                $("[id$=txtMfgDate]").removeAttr("onpaste");
                $("[id$=txtExpiryDate]").removeAttr("onpaste");
            }

            GrandScriptUtils.DatePickerCommon("txtShipmentDate");
            GrandScriptUtils.DatePickerCommon("txtAmendDate");

            $("[id*=txtRate]").ForceNumericOnly();
            $("[id*=txtBrandQuantity]").ForceNumericOnly();
            $("[id*=txtNoofLayers]").ForceNumericOnly();
            $("[id*=txtPcsLayers]").ForceNumericOnly();
            //            $("[id*=txtInner]").ForceNumericOnly();
            //            $("[id*=txtCarton]").ForceNumericOnly();



            GrandScriptUtils.MakeAutoCompleteDDL("txtPrdCategory", uiUrl + "?IsProductRequired=" + $("[id$=hdfIsProductRequired]").val(), "hdfPrdCategory", true, true, "PACKINGSPECCATEGORY");
            // GrandScriptUtils.MakeAutoCompleteDDL("txtPrdCategory", uiUrl , "hdfPrdCategory", true, true, "PACKINGSPECCATEGORY");

            GrandScriptUtils.MakeAutoCompleteDDL("txtPrdType", uiUrl + "?IsProductRequired=" + $("[id$=hdfIsProductRequired]").val(), "hdfPrdType", true, true, "PACKINGSPECTYPE");



            if ($("[id$=hdfSaleOrderType]").val() == 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=0" + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=" + $("[id$=ddlSaleOrderType]").val() + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
            }
            GrandScriptUtils.MakeAutoCompleteText("txtToPort", uiUrl + "?SIType=" + $("[id$=ddlSaleOrderType]").val() + "&SaleToPort=1", "hdfToPortID", true, true, "FILLPORTDETAILS");

            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", uiUrl, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            if ($("[id$=txtBrand]").attr("disabled") == true) {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
             GrandScriptUtils.MakeAutoCompleteDDL("txtHSNNo", uiUrl, "hdfHSNNo", true, true, "HSNNO");
            if ($("[id$=txtHSNNo]").attr("disabled") == true) {
                DisableAuto($("[id$=txtHSNNo]"), $("[id$=hdfHSNNo]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", uiUrl, "hdfCurrency", true, true, "CURRENCY");
            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
            ShowHideItemDetails($("[id$=hdfIsItemDetailsVisible]").val());
            ShowHideShippingDetails($("[id$=hdfIsShippingDetailsVisible]").val());
            ShowHideTerms($("[id$=hdfIsTermsVisible]").val());
            ShowHideRelatedInformation($("[id$=hdfIsRelatedInformationVisible]").val());
            ShowHideAttachDocs($("[id$=hdfIsAttachDocsVisible]").val());
            ShowHideAdditionalPackDtls($("[id$=hdfIsAdditionalPackDtlsVisible]").val());
            ShowHideAdditionalDetails($("[id$=hdfIsAdditionalDetailsVisible]").val());
            $('a[disabled=disabled]').click(function () { return false; });
            SetLinkDisabled();
            InitPackingMaterial();
            //ShowHidePackingMaterialDetails();
            //ShowSpec();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //            var IsPostBack = parseInt($('[id$=hdfIsPostback]').val());
            //            if (IsPostBack == 0) {
            //                var txt = $('[id$=txtSaleOrderDate]');
            //                txt.focus();
            //            }
            //For Hide Move Up/Down arrow 
            var totalRows = $('[id*=grdItemDetails] tr').length - 1;   //Total Row Count
            $('[id*=grdItemDetails] tr:nth-child(2)').find("[id*=imbRuleUp]").hide(); //Hide first rows Up arrow
            $('[id*=grdItemDetails] tr:nth-child(' + totalRows + ')').find("[id*=imbRuleDown]").hide(); //Hide last rows Down Arrow
            //  $('[id*=grdItemDetails] tr:last').find("[id*=imbRuleDown]").hide();// If no footer exist use this code

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
        }

        function InitQuotation() {
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtQuotation", url + "?CustomerID=" + $("[id$=hdfCustomer]").val(), "hdfQuotationPK", true, true, false, "QUOTATION");
            if ($("[id$=txtQuotation]").attr("disabled") == true) {
                DisableAuto($("[id$=txtQuotation]"), $("[id$=hdfQuotationPK]"));
            }
        }

        //function DisableQuote() {
        //    DisableAuto($("[id$=txtQuotation]"), $("[id$=hdfQuotationPK]"));
        //}

        //function EnableQuote() {
        //    EnableAuto($("[id$=txtQuotation]"), $("[id$=hdfQuotationPK]"));
        //}

        function InitPackingMaterial() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtPrdPackSpec", uiUrl + "?Category=" + $("[id$=hdfPrdCategory]").val() + "&Type=" + $("[id$=hdfPrdType]").val() + "&CustomerID=" + $("[id$=hdfCustomer]").val() + "&IsProductRequired=" + $("[id$=hdfIsProductRequired]").val(), "hdfPrdPackSpec", true, true, "GETPACKINGSPEC");
            //        if ($("[id$=txtPrdPackSpec]").attr("disabled") == true) {
            //                DisableAuto($("[id$=txtPrdPackSpec]"), $("[id$=hdfPrdPackSpec]"));

        }
        function ExpandAdditionalDetails() {
            $("[id$=imbShowAdditionalDetails]").click();
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtSaleOrderDate") {
                GrandScriptUtils.DatePickerCommon("txtPODate");
                if ($("[id$=hdfContractDateChange]").val() != 1)
                {
                $("[id$=txtPODate]").val($("[id$=txtSaleOrderDate]").val());
                $("[id$=txtBookingDate]").val($("[id$=txtSaleOrderDate]").val());
                }
                GrandScriptUtils.AddDateRangeCommon("txtPODate", "hdfPODate", "txtBookingDate", "hdfBookingDate", false, false);
                // GrandScriptUtils.AddDateRangeCommon("txtBookingDate", "txtSaleOrderDate", "txtReqByDate", "hdfReqByDate");
                AfterDateSelect("txtBookingDate");
            }
            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }
            if (controlID == "txtReqByDate" && $("[id$=txtShipmentDate]").val() == "") {
                $("[id$=txtShipmentDate]").val($("[id$=txtReqByDate]").val());
            }
            else if (controlID == "txtBookingDate") {
                var bookingDate = $.datepicker.parseDate("dd-M-yy", $('input[id$=txtBookingDate]').val());
                bookingDate.setMonth(bookingDate.getMonth() + 1);
                bookingDate.setDate(0);
                var expectedDateText = $.datepicker.formatDate("dd-M-yy", bookingDate);
                var expectedDate = $.datepicker.formatDate("dd-M-yy", bookingDate);
                $("[id$=txtReqByDate]").val(expectedDateText);
                $("[id$=hdfReqByDate]").val(expectedDate);
                $("[id$=txtShipmentDate]").val(expectedDateText);
                $("[id$=btnBookingDate]").click();
            }
            if (controlID == "txtReqByDate") {
                var txt = $('[id$=txtRate]');
                txt.focus();
            }
        }
        function ItemAdd() {
            $("[id$=hdfIscartYes]").val(1);
            $("[id$=btnAddItem]").click();
            return false;
        }

        function ShowSpec(mode) {

            if (mode == 1) {
                alert(mode);
                document.getElementById("rbtShowPackingMaterail").checked = true;
                $('#divPackingMaterialDetails').show();
            }
            else {
                alert(mode);
                $('#divPackingMaterialDetails').hide();
            }
        }

        //Customer Po No already exists with same customer and different date
        function ShowCustPONoExistConfirm(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("Msg_CustPO.NoExist").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfCusPoNumber]").val("1");
                        $(this).dialog("close");
                        $("[id$=" + btn + "]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function CreditCheckContinueConfirm(msg) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
           // msg = '<%=GetLocalResourceObject("Msg_CustPO.NoExist").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 170,
                width: 390,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfCreditCheckContinue]").val("1");
                        $(this).dialog("close");
                        $("[id$=btnDummySaveSubmit]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }






        //For carten check
        function ShowCartCheckConfirming() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Cont_Confirm").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfIscartYes]").val(1);
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $(this).dialog("close");
                        $("[id$=btnAddItem]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscartYes]").val(0);
                        $(this).dialog("close");
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        return false;
                    }
                },
                open: function (event, ui) {
                    if ($('#popupHolder div.ui-dialog').is(':visible')) {
                        var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                        $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                        $('html').scrollTop(0);
                        $('html,body').animate({ scrollTop: 0 });
                        $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
                    }
                },
                close: function (event) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
                }

            });
            return false;
        }

        //For CBM Check
        function ShowCBMCheckConfirming(AlertMsg, flag) {

            var msgTitle;
            var msg;
            var IsFromOk = 0;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = AlertMsg;
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $("[id$=hdfIscartYes]").val(1);
                        if (flag == 1 || flag == 2) { $("[id$=hdfIsWrkflwClose]").val(-1); }
                        IsFromOk = 1;
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (flag == 0) {//AddItem
                            $("[id$=btnAddItem]").click();
                        }
                        else if (flag == 1) {//Save                            
                            if ($("[id$=btnSave]").length > 0) {
                                $("[id$=btnSave]").click();
                            }
                            else {
                                $("[id$=btnIOReviewSave]").click();
                            }
                        }
                        else if (flag == 2) {//Save & Submit ButtonClick
                            $("[id$=btnDummySaveSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $("[id$=hdfIscartYes]").val(0);
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        return false;
                    }
                },
                open: function (event, ui) {
                    if ($('#popupHolder div.ui-dialog').is(':visible')) {
                        var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                        $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                        $('html').scrollTop(0);
                        $('html,body').animate({ scrollTop: 0 });
                        $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
                    }
                },
                close: function (event) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
                    if ($("[id$=hdfIsWrkflwClose]").val() != -1) {
                        $("[id$=hdfIsWrkflwClose]").val(1);
                    }
                    if (IsFromOk != 1) {
                        $("[id$=hdfOldCBMLimitPK]").val(0);
                        $("[id$=hdfNewCBMLimitPK]").val(0);
                    }
                }
            });
            return false;
        }


        //For Brand check
        function ShowBrandCheckConfirming() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_Same_Brand_Cont").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $("[id$=hdfIsBrandYes]").val(1);
                        $("[id$=hdfIscartYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnAddItem]").click();
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $("[id$=hdfIsBrandYes]").val(0);
                        $("[id$=hdfIscartYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                },
                open: function (event, ui) {
                    if ($('#popupHolder div.ui-dialog').is(':visible')) {
                        var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                        $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                        $('html').scrollTop(0);
                        $('html,body').animate({ scrollTop: 0 });
                        $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
                    }
                },
                close: function (event) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
                }
            });
            return false;
        }




        //For Packing Material check
        function ShowPackinMaterailCheckConfirming() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_Same_Packin_Material_Cont").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        // $("[id$=hdfIsBrandYes]").val(1);
                        // $("[id$=hdfIscartYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnSpecAdd]").click();
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        // $("[id$=hdfIsBrandYes]").val(0);
                        // $("[id$=hdfIscartYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                },
                open: function (event, ui) {
                    if ($('#popupHolder div.ui-dialog').is(':visible')) {
                        var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                        $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                        $('html').scrollTop(0);
                        $('html,body').animate({ scrollTop: 0 });
                        $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
                    }
                },
                close: function (event) {
                    $('html').css('overflow', 'auto');
                    $('body').css('overflow', 'visible');
                    $('#divmodel').hide();
                }
            });
            return false;
        }






        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=btnSelectProduct]").click();
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtPrdPackSpec") {
                //InitPackingMaterial();
                $("[id$=btnSelectPackSpec]").click();

            }
            else if (targetControlID == "txtPrdCategory") {
                InitPackingMaterial();
            }
            else if (targetControlID == "txtPrdType") {
                InitPackingMaterial();
            }
            else if (targetControlID == "txtQuotation") {
                $("[id$=btnQuotation]").click();
            }
            else if (targetControlID == "txtHSNNo") {
                $("[id$=BtnSelectHSNNo]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            var pageURL = window.document.URL;
            var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
            var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=txtBrandCode]").val("");
                $("[id$=txtPacking]").val("");
                $("[id$=hdfPackingSpec]").val("");
                $("[id$=hdfPackingText]").val("");
                $("[id$=txtQty]").val("");
                $("[id$=txtBrandQuantity]").val("");
                $("[id$=hdfQtyTemp]").val("");
                $("[id$=hdfUOM]").val("0");
                $("[id$=HdfIsPcs]").val("0");
                $("[id$=txtUOM]").val("");
                $("[id$=txtRate]").val("");
                $("[id$=txtDiscount]").val("");
                $("[id$=txtLotNo]").val("");
                  $("[id$=txtHSNNo]").val("");
                $("[id$=hdfProduct]").val("0");
                $("[id$=hdfProductName]").val("");
                $("[id$=txtProduct]").val("");
                $("[id$=txtTotalPiecesCtn]").val("");
                $("[id$=hdfTotalPcsInBox]").val("");
                $("[id$=txtAmount]").val("");
                $("[id$=txtctnRate]").val("");
                $("[id$=hdfCtnRate]").val("");
                $("[id$=txtBoxRate]").val("");
                $("[id$=txtTax]").val("");
                $("[id$=txtLotSize]").val("");
                $("[id$=txtDtlRemark]").val("");
                $("[id$=txtDtlRemark2]").val("");
                $("[id$=btnCustSelected]").click();

                if ($("[id$=hdfSaleOrderType]").val() == 0) {
                    GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=0" + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
                }
                else {
                    GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=" + $("[id$=ddlSaleOrderType]").val() + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
                }
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=txtBrandCode]").val("");
                $("[id$=txtPacking]").val("");
                $("[id$=hdfPackingSpec]").val("");
                $("[id$=hdfPackingText]").val("");
                $("[id$=txtQty]").val("");
                $("[id$=txtBrandQuantity]").val("");
                $("[id$=hdfQtyTemp]").val("");
                $("[id$=hdfUOM]").val("0");
                $("[id$=HdfIsPcs]").val("0");
                $("[id$=txtUOM]").val("");
                $("[id$=txtRate]").val("");
                $("[id$=txtDiscount]").val("");
                $("[id$=txtLotNo]").val("");

                $("[id$=hdfProduct]").val("0");
                $("[id$=hdfProductName]").val("");
                $("[id$=txtProduct]").val("");
                $("[id$=txtTotalPiecesCtn]").val("");
                $("[id$=hdfTotalPcsInBox]").val("");
                $("[id$=txtAmount]").val("");
                $("[id$=txtctnRate]").val("");
                $("[id$=hdfCtnRate]").val("");
                $("[id$=txtBoxRate]").val("");
                $("[id$=txtTax]").val("");
                $("[id$=txtLotSize]").val("");
                $("[id$=txtDtlRemark]").val("");
                $("[id$=txtDtlRemark2]").val("");
                 $("[id$=txtHSNNo]").val("");
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=txtCurrency]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfCurrency]").val("0");
                $("[id$=btnCurrency]").click();
            }

        }
        function GoBack() {
            var backURL = document.referrer;
            backURL = backURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            window.location = backURL;
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
                InitComponents();
            }
            return false;
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode 
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveSubmit]").hide();
                $("[id$=btnAddItem]").hide();
                $("[id$=btnClearItem]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlInActive]").hide();
                $("[id$=divDetailActions]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlInActive]").hide();
                $("[id$=pnlAlert]").hide();
                $("[id$=pnlPrintSO]").hide();
                $("[id$=divAmendDate]").hide();
                $("[id$=txtAmendDate]").val("");
                $("[id$=txtAmendDate]").hide();
            }
        }


        function IOReview() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_IOReview").ToString() %>';
            // msg = $("[id$=hdfAmntMissmatch]").val();
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        // $("[id$=hdfIscontYes]").val(1);
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        $("[id$=btnSaveIOReview]").click();
                    },
                    Cancel: function (e) {
                        // $("[id$=hdfIscontYes]").val(0);
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        //        function ValidatePageNow(valGroup) {
        //            if (typeof (Page_ClientValidate) == 'function') {
        //                //For finding and removing duplicate and other group validation controls
        //                //CheckValidationDuplicate(valGroup);
        //                //For Script validating the Page
        //                Page_ClientValidate(valGroup);
        //            }
        //            if (!Page_IsValid) {
        //                $("[id$=litErrorMsg]").hide();
        //                ShowErrorMessage($("#diverror").html());
        //                return false;  //Page is invalid -- stop right here
        //            }
        //            else {
        //                //everythings ok --- Call your function & do your stuff
        //                return true;
        //            }
        //        }
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
                    //                    else {
                    //                        Page_Validators.splice(i, 1);
                    //                    }
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ShowContract() {
            ShowContainerDiv('#divTerms', '<%=GetLocalResourceObject("ContractTerms").ToString() %>', '700', '500');
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }
        function CalculateSpecAmount(sender) {
            var SpecQty = 0;
            var SpecRate = 0;
            var SpecAmount = 0;
            SpecQty = parseFloat($("[id$=txtspecqty]").val());
            SpecRate = parseFloat($("[id$=txtspecprice]").val());
            if (!isNaN(SpecQty) && !isNaN(SpecRate)) {
                SpecAmount = SpecQty * SpecRate;
                $("[id$=txtspecamount]").val(SpecAmount.toFixed(CurrencyDigits));
            }
        }


        function CalculateAmount(sender) {
            //            alert($(sender).attr('id'));
            $("[id$=hdfErrorMsgType]").val("0"); //For Resetting (Bug ID:  16120)
            var IsCopy = parseInt($("[id$=hdfIsCopy]").val());
            if (IsCopy == 0) {
                if ($(sender).attr("id").indexOf("txtQty") >= 0) {
                    var qtyDespatched = parseFloat($("[id$=hdfQtyDespatched]").val());
                    var qtyInvoiced = parseFloat($("[id$=hdfQtyInvoiced]").val());
                    var currentQty = parseFloat($(sender).val());
                    if (!isNaN(currentQty)) {
                        if (qtyDespatched > currentQty) {
                            $(sender).val($("[id$=hdfQtyTemp]").val());
                            $("[id$=hdfErrorMsgType]").val("1"); //For Identifying which error message is to show
                        }
                        else if (qtyInvoiced > currentQty) {

                            $(sender).val($("[id$=hdfQtyTemp]").val());
                            $("[id$=hdfErrorMsgType]").val("2");
                        }
                    }
                    //                else {
                    //                    $(sender).val($("[id$=hdfQtyTemp]").val());
                    //                }
                }
            }
            var qty = 0;
            var rate = 0;
            var ctn = 0;
            var box = 0;
            var brandQty = 0;
            var UOMConv = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());
            ctn = parseFloat($("[id$=txtTotalPiecesCtn]").val());
            box = parseFloat($("[id$=hdfTotalPcsInBox]").val());
            brandQty = parseFloat($("[id$=txtBrandQuantity]").val());
            UOMConv = parseFloat($("[id$=hdfBrandUOMConversion]").val());
            if (!isNaN(brandQty)) {
                qty = UOMConv * brandQty;
                $("[id$=txtQty]").val(qty.toFixed(NumberDigits));
            }
            else {
                qty = 0;
                $("[id$=txtQty]").val(qty.toFixed(NumberDigits));
            }
            if (!isNaN(brandQty) && !isNaN(rate)) {
                var amount = brandQty * rate;
                $("[id$=txtAmount]").val(amount.toFixed(CurrencyDigits));
                var amountCtn = (isNaN(ctn) ? 0 : ctn) * rate;
                var amountBox = (isNaN(box) ? 0 : box) * rate;
                if ($("[id$=HdfIsPcs]").val() == 1) {
                    if (amountBox > 0) {
                        $("[id$=txtctnRate]").val(((amountBox.toFixed(CBPriceDecimalDigits)).toString()) + (amountBox > 0 ? ' /Box' : '') + ' \n' + ((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                        $("[id$=hdfCtnRate]").val(((amountBox.toFixed(CBPriceDecimalDigits)).toString()) + (amountBox > 0 ? ' /Box' : '') + ' \n' + ((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                    }
                    else {
                        $("[id$=txtctnRate]").val(((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                        $("[id$=hdfCtnRate]").val(((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                    }
                }
            }
            else
                $("[id$=txtAmount]").val(parseFloat(0).toFixed(CurrencyDigits));
            $("[id$=hdfQtyTemp]").val($("[id$=txtBrandQuantity]").val());

            if ($(sender).attr('id') == $('[id$=txtBrandQuantity]').attr('id')) {

                $("[id$=hdfFocusPdctAdd]").val("1");
                //                var txt = $('[id$=txtReqByDate]');
                //                txt.focus();
            } else if ($(sender).attr('id') == $('[id$=txtRate]').attr('id')) {
                $("[id$=hdfFocusPdctAdd]").val("2");
                //                var txt = $('[id$=txtctnRate]');
                //                txt.focus();
            }

            $("[id$=btnTooltip]").click();

        }

        function CalculateAmountPopUpApply() {
            var qty = 0;
            var rate = 0;
            var ctn = 0;
            var box = 0;
            var brandQty = 0;
            var UOMConv = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());
            ctn = parseFloat($("[id$=txtTotalPiecesCtn]").val());
            box = parseFloat($("[id$=hdfTotalPcsInBox]").val());
            brandQty = parseFloat($("[id$=txtBrandQuantity]").val());
            UOMConv = parseFloat($("[id$=hdfBrandUOMConversion]").val());
            if (!isNaN(brandQty)) {
                qty = UOMConv * brandQty;
                $("[id$=txtQty]").val(qty.toFixed(NumberDigits));
            }
            else {
                qty = 0;
                $("[id$=txtQty]").val(qty.toFixed(NumberDigits));
            }
            if (!isNaN(brandQty) && !isNaN(rate)) {
                var amount = brandQty * rate;
                $("[id$=txtAmount]").val(amount.toFixed(CurrencyDigits));
                var amountCtn = (isNaN(ctn) ? 0 : ctn) * rate;
                var amountBox = (isNaN(box) ? 0 : box) * rate;
                if ($("[id$=HdfIsPcs]").val() == 1) {
                    if (amountBox > 0) {
                        $("[id$=txtctnRate]").val(((amountBox.toFixed(CBPriceDecimalDigits)).toString()) + (amountBox > 0 ? ' /Box' : '') + ' \n' + ((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                        $("[id$=hdfCtnRate]").val(((amountBox.toFixed(CBPriceDecimalDigits)).toString()) + (amountBox > 0 ? ' /Box' : '') + ' \n' + ((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                    }
                    else {
                        $("[id$=txtctnRate]").val(((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                        $("[id$=hdfCtnRate]").val(((amountCtn.toFixed(CBPriceDecimalDigits)).toString()) + (amountCtn > 0 ? ' /Ctn' : ''));
                    }
                }
            }
            else
                $("[id$=txtAmount]").val(parseFloat(0).toFixed(CurrencyDigits));
            $("[id$=hdfQtyTemp]").val($("[id$=txtBrandQuantity]").val());

            $("[id$=hdfFocusPdctAdd]").val("1");
            $("[id$=btnTooltip]").click();

        }

        function CalculateTotal(sender) {
            var subTotal = $("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').length > 0 ? parseFloat($("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').html().replace(new RegExp(',', 'g'), '')) : parseFloat(0);
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
            totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
            var totalTax = parseFloat($("[id$=txtHdrTax]").val());
            totalTax = isNaN(totalTax) ? 0 : totalTax;
            var totalShipping = parseFloat($("[id$=txtShipping]").val());
            totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
            var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
            totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;

            var netTotal = (subTotal + totalTax + totalShipping + totalPriceAdj) - totalDiscount;
            $("[id$=txtHdrTotal]").val(netTotal.toFixed(parseInt($("[id$=hdfCurrencyDigits]").val())));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(parseInt($("[id$=hdfCurrencyDigits]").val())));
            if (totalShipping == 0)
                $("[id$=txtShipping]").val((totalShipping).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
        }
        function ShowHideItemDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                if ($('input[id*=rdbPackingSpec]').is(":checked") || $('input[id*=rdbProduct]').is(":checked")) {
                    $("[id$=divItemDetails]").hide();
                    $("[id$=imbShowItemDetails]").hide();
                    $("[id$=imbHideItemDetails]").show();
                    $("[id$=divPackingMaterialDetails]").show();
                }
                else {
                    $("[id$=divItemDetails]").show();
                    $("[id$=imbShowItemDetails]").hide();
                    $("[id$=imbHideItemDetails]").show();
                }
            }
            else {
                $("[id$=divItemDetails]").hide();
                $("[id$=imbShowItemDetails]").show();
                $("[id$=imbHideItemDetails]").hide();
                $("[id$=divPackingMaterialDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }

        function ShowHidePackingMaterialDetails() {
            //if ($("#rbtShowPackingMaterail").is(":checked")) {
            $("[id$=divItemDetails]").hide();
            $("[id$=divPackingMaterialDetails]").show();

            if ($('input[id*=rdbProduct]').is(":checked")) {
                $("[id$=hdfIsProductRequired]").val(1);
                //$("[id$=lbl_Prd_PackSpeck]").hide();
                //$("[id$=lbl_product]").show();
            }
            else {
                $("[id$=hdfIsProductRequired]").val(0);
                //$("[id$=lbl_product]").hide();
                //$("[id$=lbl_Prd_PackSpeck]").show();
            }
        }

        function ShowBrandDetails() {
            //if ($("#rbtShowPackingMaterail").is(":checked")) {
            $("[id$=divItemDetails]").show();
            $("[id$=divPackingMaterialDetails]").hide();
        }

        function ShowHideShippingDetails(flag) {
            ///<summary>
            /// Used to Show/Hide HideShippingDetails Div
            ///</summary>

            //If flag then Show HideShippingDetails
            if (flag == 1) {
                $("[id$=divShippingDetails]").show();
                $("[id$=imbShowShippingDetails]").hide();
                $("[id$=imbHideShippingDetails]").show();
            }
            else {
                $("[id$=divShippingDetails]").hide();
                $("[id$=imbShowShippingDetails]").show();
                $("[id$=imbHideShippingDetails]").hide();
            }
            $("[id$=hdfIsShippingDetailsVisible]").val(flag);
            return false;
        }

        function ShowHideAdditionalPackDtls(flag) {
            ///<summary>
            /// Used to Show/Hide HideShippingDetails Div
            ///</summary>

            //If flag then Show HideShippingDetails
            if (flag == 1) {
                $("[id$=divAdditionalPackDtlsHdr]").show();
                $("[id$=imbShowAdditionalPackDtls]").hide();
                $("[id$=imbHideAdditionalPackDtls]").show();
            }
            else {
                $("[id$=divAdditionalPackDtlsHdr]").hide();
                $("[id$=imbShowAdditionalPackDtls]").show();
                $("[id$=imbHideAdditionalPackDtls]").hide();
            }
            $("[id$=hdfIsAdditionalPackDtlsVisible]").val(flag);
            return false;
        }
        function ShowHideAdditionalDetails(flag) {
            ///<summary>
            /// Used to Show/Hide HideShippingDetails Div
            ///</summary>

            //If flag then Show HideShippingDetails
            if (flag == 1) {
                $("[id$=divAdditionalPackDtls1]").show();
                $("[id$=divAdditionalPackDtls2]").show();
                $("[id$=imbShowAdditionalDetails]").hide();
                $("[id$=imbHideAdditionalDetails]").show();
            }
            else {
                $("[id$=divAdditionalPackDtls1]").hide();
                $("[id$=divAdditionalPackDtls2]").hide();
                $("[id$=imbShowAdditionalDetails]").show();
                $("[id$=imbHideAdditionalDetails]").hide();
            }
            $("[id$=hdfIsAdditionalDetailsVisible]").val(flag);
            return false;
        }

        function ShowHideTerms(flag) {
            ///<summary>
            /// Used to Show/Hide Terms Div
            ///</summary>

            //If flag then Show Terms
            if (flag == 1) {
                $("[id$=divTermDetails]").show();
                $("[id$=imbShowTerms]").hide();
                $("[id$=imbHideTerms]").show();
            }
            else {
                $("[id$=divTermDetails]").hide();
                $("[id$=imbShowTerms]").show();
                $("[id$=imbHideTerms]").hide();
            }
            $("[id$=hdfIsTermsVisible]").val(flag);
            return false;
        }
        function ShowHideRelatedInformation(flag) {
            ///<summary>
            /// Used to Show/Hide Related Information Div
            ///</summary>

            //If flag then Show Related Information
            if (flag == 1) {
                $("[id$=divRelatedInformation]").show();
                $("[id$=imbShowRelatedInformation]").hide();
                $("[id$=imbHideRelatedInformation]").show();
            }
            else {
                $("[id$=divRelatedInformation]").hide();
                $("[id$=imbShowRelatedInformation]").show();
                $("[id$=imbHideRelatedInformation]").hide();
            }
            $("[id$=hdfIsRelatedInformationVisible]").val(flag);
            return false;
        }
        function ShowHideAttachDocs(flag) {
            ///<summary>
            /// Used to Show/Hide Attach Document Div
            ///</summary>

            //If flag then Show Attach Document
            if (flag == 1) {
                $("[id$=divAttachDocs]").show();
                $("[id$=imbShowAttachDocs]").hide();
                $("[id$=imbHideAttachDocs]").show();
            }
            else {
                $("[id$=divAttachDocs]").hide();
                $("[id$=imbShowAttachDocs]").show();
                $("[id$=imbHideAttachDocs]").hide();
            }
            $("[id$=hdfIsAttachDocsVisible]").val(flag);
            return false;
        }
        function SetNewArtWork(sender) {
            if ($(sender).attr("checked")) {
                $("[id$=lnkArtWorkPC]").hide();
                $("[id$=lnkArtWorkIB]").hide();
                $("[id$=lnkArtWorkIC]").hide();
                $("[id$=lnkArtWorkZB]").hide();
                $("[id$=lnkArtWorkMC]").hide();
                $("[id$=lnkArtWorkSC]").hide();
            }
            else {
                $("[id$=lnkArtWorkPC]").show();
                $("[id$=lnkArtWorkIB]").show();
                $("[id$=lnkArtWorkIC]").show();
                $("[id$=lnkArtWorkZB]").show();
                $("[id$=lnkArtWorkMC]").show();
                $("[id$=lnkArtWorkSC]").show();
            }
        }
        function SetLinkDisabled() {
            if ($("[id$=lnkArtWorkPC]").attr("href") == "" || $("[id$=lnkArtWorkPC]").attr("href") == "#") {
                $("[id$=lnkArtWorkPC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkPC]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkIB]").attr("href") == "" || $("[id$=lnkArtWorkIB]").attr("href") == "#") {
                $("[id$=lnkArtWorkIB]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkIB]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkIC]").attr("href") == "" || $("[id$=lnkArtWorkIC]").attr("href") == "#") {
                $("[id$=lnkArtWorkIC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkIC]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkZB]").attr("href") == "" || $("[id$=lnkArtWorkZB]").attr("href") == "#") {
                $("[id$=lnkArtWorkZB]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkZB]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkMC]").attr("href") == "" || $("[id$=lnkArtWorkMC]").attr("href") == "#") {
                $("[id$=lnkArtWorkMC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkMC]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkSC]").attr("href") == "" || $("[id$=lnkArtWorkSC]").attr("href") == "#") {
                $("[id$=lnkArtWorkSC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkSC]").removeClass("link-disabled");
            }


            $("#[id*=grdItemDetails] [id*=lnkLstArtWorkPC]").each(function (index) {
                if ($(this).attr("href") == "" || $(this).attr("href") == "#" || typeof $(this).attr("href") == 'undefined') {
                    $(this).addClass("link-disabled");
                }
                else {
                    $(this).removeClass("link-disabled");
                }
            });
            $("#[id*=grdItemDetails] [id*=lnkLstArtWorkIB]").each(function (index) {
                if ($(this).attr("href") == "" || $(this).attr("href") == "#" || typeof $(this).attr("href") == 'undefined') {
                    $(this).addClass("link-disabled");
                }
                else {
                    $(this).removeClass("link-disabled");
                }
            });
            $("#[id*=grdItemDetails] [id*=lnkLstArtWorkIC]").each(function (index) {
                if ($(this).attr("href") == "" || $(this).attr("href") == "#" || typeof $(this).attr("href") == 'undefined') {
                    $(this).addClass("link-disabled");
                }
                else {
                    $(this).removeClass("link-disabled");
                }
            });

            $("#[id*=grdItemDetails] [id*=lnkLstArtWorkZB]").each(function (index) {
                if ($(this).attr("href") == "" || $(this).attr("href") == "#" || typeof $(this).attr("href") == 'undefined') {
                    $(this).addClass("link-disabled");
                }
                else {
                    $(this).removeClass("link-disabled");
                }
            });



            $("#[id*=grdItemDetails] [id*=lnkLstArtWorkMC]").each(function (index) {
                if ($(this).attr("href") == "" || $(this).attr("href") == "#" || typeof $(this).attr("href") == 'undefined') {
                    $(this).addClass("link-disabled");
                }
                else {
                    $(this).removeClass("link-disabled");
                }
            });

            $("#[id*=grdItemDetails] [id*=lnkLstArtWorkSC]").each(function (index) {
                if ($(this).attr("href") == "" || $(this).attr("href") == "#" || typeof $(this).attr("href") == 'undefined') {
                    $(this).addClass("link-disabled");
                }
                else {
                    $(this).removeClass("link-disabled");
                }
            });

        }
        function ShowDeleteConfirmWithReason() {
            ShowContainerDiv('[id$=divConfirmationWithReason]', 'Information', '550', '220');
            return false;
        }
        function closeDeletePopup() {
            $("[id$=txtReason]").val("");
            $('#divConfirmationWithReason').dialog('close');
            ClosePopup();
            return false;
        }

        function ShowDeleteConfirmationMsg(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.MsgDeleteConfirm %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        //Show/Hide ddlSaleOrderSubtype
        function ShowHideSOSubtype() {
            if ($("[id$=hdfEnableSubType]").val() == 1) {
                $("[id$=ddlSaleOrderSubtype]").show();
            }
            else {
                $("[id$=ddlSaleOrderSubtype]").hide();
            }
        }

        //For Checking Brand have CBM
        function ShowCBMConfirm() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Cont_CBM").ToString() %>';

            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');

            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfIsContinueCBM]").val(1);
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        $("[id$=btnAddItem]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsContinueCBM]").val(0);
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function SetValueForAddItem() {
            $("[id$=hdfAddItem]").val("1");
        }


        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }
            return true;
        }

        //        function ScrollDown() {
        //            window.scroll(400, 400);
        //            return false;
        //        }

    </script>
    <script type="text/javascript">

        $(window).load(function EndRequest() {
            FormatCalendar('4');

            // HideFilter();
        });


        var ControlID = 'calendar1|calendar2';
        function EndRequest() { FormatCalendar('4'); }
        function FormatCalendar(type) {
            var ctrlBehaviourarray = ControlID.split('|');
            for (var i = 0; i < ctrlBehaviourarray.length; i++) {
                var calenderCtrl = $find(ctrlBehaviourarray[i]);
                if (calenderCtrl) {
                    switch (type) {
                        case "6":
                            return;
                            break;
                        case "4":
                            $(calenderCtrl).attr('CalenderType', '2');
                            modifyMontDelegates(calenderCtrl);
                            break;
                        case "1":
                            $(calenderCtrl).attr('CalenderType', '3');
                            modifyYearDelegates(calenderCtrl);
                            break;
                    }
                }
            }
        }
        function modifyMontDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "month":
                            //if the mode is month, then stop switching to day mode.
                            if (target.month == visibleDate.getMonth()) {
                                //this._switchMode("days");
                            } else {
                                cal._visibleDate = target.date;
                                //this._switchMode("days");
                            }
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                cal._switchMode("months");
                            }
                            break;
                        // case "day":                                                                                                                
                        // this.set_selectedDate(target.date);                                                                                                                
                        // this._switchMonth(target.date);                                                                                                                
                        // this._blur.post(true);                                                                                                                
                        // this.raiseDateSelectionChanged();                                                                                                                
                        // break;                                                                                                                
                        case "today":
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }
        function modifyYearDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        // case "month":                                                                                                             
                        // //if the mode is month, then stop switching to day mode.                                                                                                             
                        // if (target.month == visibleDate.getMonth()) {                                                                                                             
                        // //this._switchMode("days");                                                                                                             
                        // } else {                                                                                                             
                        // cal._visibleDate = target.date;                                                                                                             
                        // //this._switchMode("days");                                                                                                             
                        // }                                                                                                             
                        // cal.set_selectedDate(target.date);                                                                                                             
                        // cal._switchMonth(target.date);                                                                                                             
                        // cal._blur.post(true);                                                                                                             
                        // cal.raiseDateSelectionChanged();                                                                                                             
                        // break;                                                                                                             
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                // cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                //cal._switchMode("months");
                            }
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        // case "day":                                                                                                             
                        // this.set_selectedDate(target.date);                                                                                                             
                        // this._switchMonth(target.date);                                                                                                             
                        // this._blur.post(true);                                                                                                             
                        // this.raiseDateSelectionChanged();                                                                                                             
                        // break;                                                                                                             
                        case "today":
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }

        function changeMonthCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function changeYearCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }

        function onCalendarShown(cal, args) {
            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;
        }

        function onCalendarHidden(sender, args) {
        }

        function CalcQuantity() {
            var Quantity = 0;
            var DecimalCount = $("[id$=hdfDecimalVal]").val();
            var TotalPcsCtn = $("[id$=txtPopUpTotalPcsCtn]").val();
            var SalePcs = $("[id$=txtPopUpPcsperUnit]").val();
            var OrderQty = $("[id$=txtPopUpOrderQty]").val();
            if (OrderQty != '')
                Quantity = (parseFloat(TotalPcsCtn) / parseFloat(SalePcs)) * parseFloat(OrderQty);
            $("[id$=txtPopUpQty]").val(parseFloat(Quantity).toFixed(DecimalCount));
        }

        function CalcTax() {
            var Amount = $("[id$=txtPopupItemAmount]").val();
            var TaxPer = $("[id$=txtTaxPerc]").val();
            var DecimalCount = $("[id$=hdfDecimalVal]").val();
            var Total = 0;
            if (TaxPer != '' && TaxPer > 0)
                Total = (parseFloat(Amount) * parseFloat(TaxPer)) / 100;
            $("[id$=txtPopupAmount]").val(Total.toFixed(DecimalCount));
        }

    </script>
    <style type="text/css">
        /* Sales Order listg */
        .file-details {
            text-align: right;
            padding: 3px 0px !important;
            margin: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlQuotation">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETEANDSUMBIT" TabIndex="63"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="64"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')"
                                            ValidationGroup="so" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="65" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="66" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="67" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <li runat="server" id="pnlInActive">
                                        <asp:Button runat="server" ID="btnInActive" CommandName="INACTIVE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="68" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirmWithReason();" />
                                    </li>
                                    <li id="pnlPrintSO" runat="server">
                                        <asp:Button runat="server" TabIndex="69" ID="btnPrintSO" CommandName="PRINTSO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li id="pnlPrintIO" runat="server">
                                        <asp:Button runat="server" TabIndex="69" ID="btnPrintIO" CommandName="PRINTIO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,IOReview %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-IOPrint"
                                            ToolTip="<%$resources:Controls,IOReview %>" />
                                    </li>
                                    <li id="pnlAlert" runat="server">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="70" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="71" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Button runat="server" ID="btnSaveIOReview" CommandName="SAVE" Text="<%$resources:ErpRes,Save %>"
                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                    ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" Style="display: none"
                    EnableTheming="false" />
                <asp:HiddenField ID="hdfPreviousUrl" runat="server" />
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfRateDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfExchangeRateDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfisItemHaveTax" runat="server" Value="0" />
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfIscartYes" runat="server" />
                                            <asp:HiddenField ID="hdfTotalCBM" runat="server" />
                                            <asp:HiddenField ID="hdfIsBrandYes" runat="server" />
                                            <asp:HiddenField ID="hdfIsPackingMaterailYes" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfWeightFormat" runat="server" />
                                            <asp:HiddenField ID="hdfSaleOrderPK" runat="server" />
                                            <asp:HiddenField ID="hdfIsCopy" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfSaleOrderRate" runat="server" />
                                            <asp:HiddenField ID="hdfIsContinueCBM" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfAddItem" runat="server" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:SaleOrderNo%>" AssociatedControlID="lblSaleOrderNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblSaleOrderNo" CssClass="input-small-a margnrgt0-8per"
                                                TabIndex="1"></asp:Label>
                                            <%-- <asp:Label runat="server" ID="lblCustPOSCNo" CssClass="medium"></asp:Label>--%>
                                            <div style="width: 18px; display: inline-block;">
                                                <asp:ImageButton ID="btnRevision" runat="server" OnClick="ActionHandler" CommandName="REVISIONHISTORY"
                                                    SkinID="history" ToolTip="<%$resources:RevisionHistory %>" />
                                            </div>
                                            <asp:Label runat="server" ID="lblSaleOrderDate" Text="<%$ resources:SaleOrderDate%>"
                                                AssociatedControlID="txtSaleOrderDate" CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSaleOrderDate" CssClass="input-small" TabIndex="1"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtSaleOrderDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfSaleOrderNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" />
                                            <div class="clear">
                                            </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRef" runat="server" Text="<%$ resources:CustPONo%>" AssociatedControlID="txtPONo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPONo" MaxLength="100" TabIndex="2" CssClass="input-small"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfPONo" CssClass="star" SetFocusOnError="true" ValidationGroup="so"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtPONo" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_PONo %>"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label ID="lblReferenceDate" runat="server" Text="<%$ resources:PODate%>" AssociatedControlID="txtPODate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPODate" CssClass="input-small" TabIndex="3" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPODate" runat="server" />
                                            <%--<asp:RequiredFieldValidator ID="vrfPODate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtPODate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PODate %>"></asp:RequiredFieldValidator>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBuyer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Buyer %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" MaxLength="100" TabIndex="4" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Buyer %>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtBuyerAddress"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBuyerAddress" MaxLength="500" TextMode="MultiLine"
                                                TabIndex="8" CssClass="multiline-m1col select-half" onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCusAddress" runat="server" />
                                            <asp:HiddenField ID="hdfCusCountry" runat="server" />
                                            <asp:HiddenField ID="hdfCusCountryText" runat="server" />
                                            <asp:HiddenField ID="hdfCusZip" runat="server" />
                                            <asp:HiddenField ID="hdfCusPhone" runat="server" />
                                            <asp:HiddenField ID="hdfCusMobile" runat="server" />
                                            <asp:HiddenField ID="hdfCusFax" runat="server" />
                                            <asp:HiddenField ID="hdfCusEmail" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSaleOrderType" Text="<%$ resources:Type%>" AssociatedControlID="ddlSaleOrderType"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" CssClass="select-small-a1" runat="server"
                                                TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:HiddenField runat="server" ID="hdfSaleOrderType" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfSaleOrderType" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSaleOrderType"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderType %>"
                                                    InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <%--  <span style="width: 7px; visibility: hidden;"></span>--%>
                                            <asp:DropDownList ID="ddlSaleOrderSubtype" CssClass="medium" runat="server" TabIndex="6">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderSubType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSaleOrderSubtype"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderSubType %>"
                                                InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblBookingDate" Text="<%$ resources:BookingDate%>"
                                                AssociatedControlID="txtBookingDate" CssClass="middle-lbl-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBookingDate" CssClass="input-small" TabIndex="7"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:Button ID="btnBookingDate" runat="server" EnableTheming="false" OnClick="ActionHandler"
                                                CommandName="BOOKINGDATECHANGE" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfBookingDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtBookingDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BookingDate %>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="9"
                                                MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                    Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="EXCHANGERATE"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:ExchangeRate %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl-small-d" /><%-- TabIndex="13"--%>
                                            <asp:TextBox ID="txtExchangeRate" runat="server" MaxLength="8" TabIndex="10" CssClass=" numeric input-small"
                                                onkeypress="return isFloatNumberKey(event);" />
                                            <div class="clear">
                                            </div>
                                            <div id="trQuotationReference" runat="server" visible="false">
                                                <asp:Label ID="lblRefNo" runat="server" Text="<%$ resources:Reference%>" AssociatedControlID="txtRefNo"
                                                    CssClass="margn-rgt0"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRefNo" Enabled="false" CssClass="input-small input-disabled"></asp:TextBox>
                                                <asp:Label ID="lblRefDate" runat="server" Text="<%$ resources:ReferenceDate%>" AssociatedControlID="txtRefDate"
                                                    CssClass="middle-lbl-a margn-rgt0"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRefDate" Enabled="false" CssClass="input-small"
                                                    onpaste="return false;" onkeydown="return CheckKey(event)" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div id="divAmendDate" runat="server">
                                                <asp:Label runat="server" ID="lblAmendDate" Text="<%$ resources:AmendmentDate%>"
                                                    AssociatedControlID="txtAmendDate" CssClass="margn-rgt0"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtAmendDate" CssClass="input-small input-disabled"
                                                    Enabled="false" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
                                                    onchange="AfterDateSelect(null)"></asp:TextBox>
                                            </div>
                                            <div class="clear"></div>
                                            <div id="divCrmQuote" runat="server">
                                                <asp:Label ID="lblQuotation" runat="server" Text="<%$ resources:QuotationNo%>" AssociatedControlID="txtQuotation"
                                                    CssClass="margn-rgt0"></asp:Label>
                                                <asp:TextBox ID="txtQuotation" runat="server" CssClass="input-small"></asp:TextBox>
                                                <asp:HiddenField ID="hdfQuotationPK" runat="server" Value="0" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="tr1" runat="server" visible="true">
                                    <td>
                                        <div class="div2col-S" id="divPlantCompany">
                                            <asp:Label ID="lblCompany" runat="server" Text='<%# GetGlobalResourceObject("Controls", "CompanyPlant").ToString() %>'
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlVoucherCompany');"
                                                CssClass="select-small-a">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ItemDetails").ToString() + " :"%></h1>
                                <div style="width: 69.5%; float: left;">
                                    <asp:ImageButton runat="server" ID="imbHdrRef" SkinID="reference" OnClick="ActionHandler"
                                        CommandName="ITEMREFDTL" Visible="false" ToolTip="<%$ resources:ItemRefTooltip %>" />
                                </div>
                                <asp:ImageButton runat="server" ID="imbShowItemDetails" OnClientClick="javascript:return ShowHideItemDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowItemDetails %>" />
                                <asp:ImageButton runat="server" ID="imbHideItemDetails" OnClientClick="javascript:return ShowHideItemDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideItemDetails %>" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divrbt" runat="server" visible='<%# GetGlobalResourceObject("ConfigurationsRes","SCPackingMaterialOrder").ToString() == "0" ? false : true %>'>
                                <%--  <asp:RadioButton runat="server" ID="rbtShowPackingMaterail" Text="Packing Material" />--%>
                                <%--  <input type="radio" id="rbtShowPackingMaterail" onclick="javascript:return ShowHidePackingMaterialDetails();" />--%>
                                <asp:RadioButton ID="rdbBrand" TabIndex="7" GroupName="rdbType" runat="server" Checked="false"
                                    onclick="javascript:return ShowBrandDetails();" />
                                <asp:Label runat="server" ID="lbl_brand" Text="Brand" AssociatedControlID="rdbBrand"
                                    CssClass="margn-lft0 bold"></asp:Label>
                                <asp:RadioButton ID="rdbPackingSpec" TabIndex="7" GroupName="rdbType" runat="server"
                                    OnCheckedChanged="ActionHandler" Checked="false" AutoPostBack="true" onclick="ShowHidePackingMaterialDetails();" />
                                <asp:Label runat="server" ID="lblPackingMaterial" Text="Packing Material" AssociatedControlID="rdbPackingSpec"
                                    CssClass="margn-lft0 margnrgt12 bold"></asp:Label>



                                <asp:RadioButton ID="rdbProduct" TabIndex="7" GroupName="rdbType" runat="server"
                                    OnCheckedChanged="ActionHandler" Checked="false" AutoPostBack="true" onclick="ShowHidePackingMaterialDetails();" />
                                <asp:Label runat="server" ID="lblrbtProduct" Text="Product" AssociatedControlID="rdbProduct"
                                    CssClass="margn-lft0 margnrgt12 bold"></asp:Label>



                            </div>
                            <div id="divItemDetails" style="display: none">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                                <asp:Label ID="lblBrand" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:Brand_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtBrand" runat="server" TabIndex="10" MaxLength="200"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBrand" runat="server" />
                                                <asp:Button ID="btnSelectProduct" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                    EnableTheming="false" Style="display: none" />
                                                <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBrandCode" runat="server" AssociatedControlID="txtBrandCode" Text="<%$ resources:BrandCode %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtBrandCode" runat="server" MaxLength="200" Enabled="false" CssClass="input-disabled select-half"
                                                    TabIndex="11"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblPacking" Text="<%$ resources:Packing %>" AssociatedControlID="txtPacking"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPacking" CssClass="input-disabled select-half"
                                                    onkeydown="return EnableArrowKey(event)" onpaste="return false;" TabIndex="13"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPackingSpec" runat="server" />
                                                <asp:HiddenField ID="hdfPackingText" runat="server" />
                                                <%--<asp:HiddenField ID="hdfArtWork" runat="server" />--%>
                                                <asp:HiddenField runat="server" ID="hdfCBM" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblProduct" runat="server" AssociatedControlID="txtProduct" Text="<%$ resources:Product %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtProduct" runat="server" MaxLength="100" Enabled="false" CssClass="input-disabled select-half"
                                                    TabIndex="12"></asp:TextBox>
                                                <asp:HiddenField ID="hdfProduct" runat="server" />
                                                <asp:HiddenField ID="hdfProductName" runat="server" />
                                                <asp:Label runat="server" ID="lblTotalPiecesCtn" Text="<%$ resources:TotalPiecesCtn %>"
                                                    AssociatedControlID="txtTotalPiecesCtn"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtTotalPiecesCtn" CssClass="input-small input-disabled numeric"
                                                    Enabled="false" TabIndex="14"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblReqByDate" Text="<%$ resources:ShpDate_Mand %>"
                                                    AssociatedControlID="txtReqByDate" CssClass="middle-lbl-c"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtReqByDate" MaxLength="12" CssClass="input-small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="15"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfReqByDate" />
                                                <%-- <span style="visibility: hidden; width: 1px;"></span>--%>
                                                <asp:CheckBox ID="chkShipdateUpdation" runat="server" TextAlign="Left" ToolTip="<%$ resources:ShipdateApplyforAll%>"
                                                    TabIndex="16" CssClass="span-normal null-graph" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShipdateUpdation").ToString() == "0" ? false : true %>' />
                                                <div class="starwrap" style="margin-left: 14px!important;">
                                                    <asp:RequiredFieldValidator ID="vrfReqByDate" CssClass="star" SetFocusOnError="false"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtReqByDate"
                                                        Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ReqByDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreReqByDate" CssClass="star" ValidationGroup="scDetails"
                                                        runat="server" ControlToValidate="txtReqByDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_ReqByDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Static" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfQtyDespatched" Value='0' />
                                                <asp:HiddenField runat="server" ID="hdfQtyInvoiced" Value='0' />
                                                <asp:HiddenField runat="server" ID="hdfQtyTemp" Value='0' />
                                                <asp:HiddenField runat="server" ID="hdfBrandUOM" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfBrandUOMConversion" Value="1" />
                                                <asp:TextBox ID="txtBrandQuantity" runat="server" CssClass="input-small numeric"
                                                    TabIndex="17" onblur="CalculateAmount(this);"></asp:TextBox>
                                                <div class="starwrap">
                                                    <%-- <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>--%>
                                                    <asp:RequiredFieldValidator ID="vrfBrandQuantity" CssClass="star" SetFocusOnError="true"
                                                        Style="padding: 0px !important;" ValidationGroup="scDetails" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtBrandQuantity" Display="Dynamic" Text="*"
                                                        ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtBrandQuantity"
                                                        Style="padding: 0px !important;" NumberDigits="8" ErrorMessage="<%$ resources:Err_Quantity_Valid %>"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails"
                                                        NonZero="true">
                                                    </cc1:QuantityValidation>
                                                    <asp:RequiredFieldValidator ID="vrfSaleCostQuantity" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scCost" EnableClientScript="true" runat="server" ControlToValidate="txtBrandQuantity"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <%-- <span style="width: 1px; visibility: hidden;"></span>--%>
                                                <asp:ImageButton runat="server" ID="imbCalcQty" SkinID="formula" ToolTip="<%$ resources:QunatityPopUpCaption %>"
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument="PageAction_Entry" />
                                                <asp:TextBox runat="server" ID="txtBrandUOM" CssClass="input-normal lbl-16-7perc"
                                                    Enabled="false" TabIndex="22"></asp:TextBox>
                                                <asp:TextBox ID="txtQty" runat="server" CssClass="input-small input-disabled numeric"
                                                    Enabled="false" onblur="CalculateAmount(this);" TabIndex="23"></asp:TextBox>
                                                <asp:HiddenField ID="hdfUOM" runat="server" />
                                                <asp:HiddenField ID="HdfIsPcs" runat="server" Value="0" />
                                                <asp:TextBox runat="server" ID="txtUOM" CssClass="input-normal input-uom-small" Enabled="false"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblLotNo" runat="server" AssociatedControlID="txtLotNo" Text="<%$ resources:LotNo %>"
                                                    Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowLotNo").ToString() == "0" ? false : true %>'>
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtLotNo" CssClass="input-small" MaxLength="100"
                                                    TabIndex="19" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowLotNo").ToString() == "0" ? false : true %>'>
                                                </asp:TextBox>
                                                <%-- <span style="visibility: hidden; width: 1px;"></span>--%>
                                                <asp:CheckBox ID="chkLotNoApplyAllItem" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                    TabIndex="20" CssClass="span-normal" Width="8px" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowLotNo").ToString() == "0" ? false : true %>' />
                                                <asp:Label ID="lblLotSize" runat="server" AssociatedControlID="txtLotSize" Text="<%$ resources:LotSize %>"
                                                    Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowLotSize").ToString() == "0" ? false : true %>'
                                                    CssClass="lbl-18-3perc">
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtLotSize" CssClass="input-small" MaxLength="100"
                                                    TabIndex="21" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowLotSize").ToString() == "0" ? false : true %>'></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <%--<asp:CheckBox ID="chkNewArtWork" runat="server" TabIndex="19" Text="<%$resources:NewArtWork%>"
                                                    TextAlign="Left" Checked="false" onclick="SetNewArtWork(this);" />--%>
                                                <asp:Label ID="lblArtWork" runat="server" AssociatedControlID="ddlArtWork" Text="<%$ resources:ArtWork %>">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlArtWork" runat="server" AutoPostBack="true" CssClass="select-small-b"
                                                    TabIndex="23" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>




                                                <a id="lnkArtWorkPC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkIB" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkIC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkZB" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkMC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkSC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <asp:HiddenField ID="hdfArtWorkPC" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkIB" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkIC" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkZB" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkMC" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkSC" runat="server" />
                                                <%-- <div class="clear">
                                                </div>
                                                <asp:Label ID="lblBoxRate" runat="server" AssociatedControlID="txtBoxRate" Text="<%$ resources:BoxRate %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtBoxRate" runat="server" width="80px"  onpaste="return false;"></asp:TextBox>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="input-small  numeric" MaxLength="18"
                                                    TabIndex="18" onblur="CalculateAmount(this);"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="scDetails"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                        NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                        ValidationGroup="scDetails" NonZero="false">
                                                    </cc1:RateValidation>
                                                    <asp:RequiredFieldValidator ID="vrfSaleCostRate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scCost" EnableClientScript="true" runat="server" ControlToValidate="txtRate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <asp:TextBox runat="server" ID="txtBrandRateUOM" CssClass="input-normal small" Enabled="false"></asp:TextBox>
                                                <span style="width: 4px; visibility: hidden;"></span>
                                                <%--<div class="clear">
                                                </div>
                                                <asp:Label ID="lblctnRate" runat="server" AssociatedControlID="txtctnRate" Text="<%$ resources:Ctn_Rate %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtctnRate" runat="server" CssClass="medium input-disabled" TabIndex="13"></asp:TextBox>--%>
                                                <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ resources:Amount_Mand %>"
                                                    CssClass="middle-lbl-xsmall-e">
                                                </asp:Label>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="input-small input-disabled numeric"
                                                    onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="imbShowSCdtlcost" SkinID="estimate-icon" ToolTip="<%$ resources:ShowSaleCost %>"
                                                    OnClick="ActionHandler" CommandName="SHOWSALESCOST" CommandArgument="PageAction_Entry"
                                                    OnClientClick="javascript:ValidatePageNow('scCost')" ValidationGroup="scCost" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                        ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails">
                                                    </cc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblItemTax" runat="server" class="right-lbl" AssociatedControlID="txtTax"
                                                    Text="<%$ resources:Tax %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtTax" runat="server" CssClass="input-small input-disabled numeric"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    TabIndex="17" ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS" ValidationGroup="scDetails"
                                                    OnClientClick="javascript:ValidatePageNow('scDetails')" />
                                                <%-- <asp:Label ID="lblCaseMark" runat="server" AssociatedControlID="txtCaseMark" Text="<%$ resources:CaseMark %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtCaseMark" MaxLength="30" runat="server" CssClass="input-small"></asp:TextBox>
                                                --%>
                                                <asp:Label ID="lblItemDiscount" runat="server" AssociatedControlID="txtDiscount"
                                                    class="middle-lbl-xsmall-a" Text="<%$ resources:Discount %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtDiscount" runat="server" CssClass="input-small input-disabled numeric"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    TabIndex="15" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCDETAILS"
                                                    ValidationGroup="scDetails" OnClientClick="javascript:ValidatePageNow('scDetails')" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblCaseMark" runat="server" AssociatedControlID="txtCaseMark" Text="<%$ resources:CaseMark %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtCaseMark" MaxLength="100" runat="server" CssClass="select-half"></asp:TextBox>
                                                <asp:CheckBox ID="ChkCaseMarkApplyAllItem" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                    TabIndex="20" CssClass="span-normal" Width="8px" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowCaseMarkApplyAllItem").ToString() == "0" ? false : true %>' />

                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblctnRate" runat="server" AssociatedControlID="txtctnRate" Text="<%$ resources:ApproxCtnBoxPrice %>"
                                                    Visible='<%# GetGlobalResourceObject("ConfigurationsRes","SCApproxCtnBoxPriceVisible").ToString() == "0" ? false : true %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="txtctnRate" runat="server" CssClass="medium select-half" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","SCApproxCtnBoxPriceVisible").ToString() == "0" ? false : true %>'
                                                    TabIndex="22" TextMode="MultiLine" Width="150px" Height="47px"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCtnRate" runat="server" />
                                                <asp:HiddenField ID="hdfTotalPcsInBox" runat="server" />

                                                
                                                
                                                <asp:HiddenField ID="HdHSNDetailPK" runat="server" Value="0" />
                                                <asp:Label ID="Label6" runat="server" AssociatedControlID="txtHSNNo" Text="<%$ resources:HSNLabel %>" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowHSNNo").ToString() == "0" ? false : true %>'>
                                                </asp:Label>
                                                <asp:TextBox ID="txtHSNNo" runat="server" TabIndex="10" MaxLength="500" Width="60%" Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowHSNNo").ToString() == "0" ? false : true %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfHSNNo" runat="server" />
                                                <asp:Button ID="BtnSelectHSNNo" runat="server"   Visible='<%# GetGlobalResourceObject("ConfigurationsRes","ShowHSNNo").ToString() == "0" ? false : true %>'
                                                    EnableTheming="false" Style="display: none" />
                                  <%--              <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtHSNNo" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_HSN %>"></asp:RequiredFieldValidator>--%>
                                           
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="search-colapse-b" runat="server" id="divAdditionalDetails" visible="false">
                                                <h1>
                                                    <%= GetLocalResourceObject("AdditionalDetails").ToString() + " :"%></h1>
                                                <asp:ImageButton runat="server" ID="imbShowAdditionalDetails" OnClientClick="javascript:return ShowHideAdditionalDetails(1);"
                                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowAdditionalDetails %>" />
                                                <asp:ImageButton runat="server" ID="imbHideAdditionalDetails" OnClientClick="javascript:return ShowHideAdditionalDetails();"
                                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideAdditionalDetails %>" />
                                                <asp:HiddenField ID="hdfIsAdditionalDetailsVisible" runat="server" Value="0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S" id="divAdditionalPackDtls1" runat="server">
                                                <div class="grupitem-bg1">
                                                    <asp:Label ID="lblStrapping" runat="server" AssociatedControlID="chkStrapping" Text="<%$ resources:Strapping %>"
                                                        CssClass="margnbotm0">
                                                    </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkStrapping" TabIndex="26" />
                                                    <asp:Label ID="lblStrappingColor" runat="server" AssociatedControlID="chkStrapping"
                                                        Text="<%$ resources:StrappingColorSku %>" CssClass="lbl-30-7perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlstrappingColor" CssClass="select-w25-8per margnbotm0"
                                                        TabIndex="26">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="grupitem-bg3">
                                                    <asp:Label ID="lblMfgDate" runat="server" AssociatedControlID="txtMfgDate" Text="<%$ resources:MfgDate %>"
                                                        CssClass="margnbotm0">
                                                    </asp:Label>
                                                    <asp:TextBox runat="server" ID="txtMfgDate" CssClass="input-small margnbotm0" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="27"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdftxtMfgDate" />
                                                    <asp:CheckBox ID="ChkMfgDateApplyAllItem" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                        TabIndex="20" CssClass="span-normal" Width="8px" />

                                                    <%--<cc2:CalendarExtender ID="txtCalender_CalendarExtenderMfgDate" runat="server" BehaviorID="calendar1"
                                                        TargetControlID="txtMfgDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                        ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                    </cc2:CalendarExtender>--%>
                                                    <asp:Label ID="lblExpiryDate" runat="server" AssociatedControlID="txtExpiryDate"
                                                        CssClass="lbl-13perc" Text="<%$ resources:ExpiryDate %>"> </asp:Label>
                                                    <asp:TextBox runat="server" ID="txtExpiryDate" CssClass="input-small margnbotm0"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="27"></asp:TextBox>
                                                    <asp:CheckBox ID="ChkExpDateApplyAllItem" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                        TabIndex="20" CssClass="span-normal" Width="8px" />

                                                    <%--<cc2:CalendarExtender ID="txtCalender_CalendarExtenderExpiryDate" runat="server"
                                                        BehaviorID="calendar2" TargetControlID="txtExpiryDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                        ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                    </cc2:CalendarExtender>--%>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S" id="divAdditionalPackDtls2" runat="server">
                                                <div class="grupitem-bg2">
                                                    <asp:Label ID="lblLayering" runat="server" AssociatedControlID="chkLayering" Text="<%$ resources:Layering %>"
                                                        CssClass="margnbotm0">
                                                    </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkLayering" TabIndex="26" />
                                                    <asp:Label ID="lblNoofLayers" runat="server" AssociatedControlID="txtNoofLayers"
                                                        Text="<%$ resources:NoofLayers %>" CssClass="lbl-15-4perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:TextBox runat="server" ID="txtNoofLayers" CssClass="input-w10per margnbotm0"
                                                        TabIndex="26"></asp:TextBox>
                                                    <asp:Label ID="lblPcsLayers" runat="server" AssociatedControlID="txtPcsLayers" Text="<%$ resources:PcsLayers %>"
                                                        CssClass="lbl-16perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPcsLayers" CssClass="input-w10per margnbotm0"
                                                        TabIndex="26"></asp:TextBox>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="27" MaxLength="480" TextMode="MultiLine"
                                                    CssClass="multiline lbl-hgt-20" ></asp:TextBox><%--onkeypress="return this.value.length<1000" onpaste="return this.value.length<1000"--%>
                                                <asp:CheckBox ID="chkRemarkToAll" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                    TabIndex="27" CssClass="span-normal null-graph minw-17" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:TextBox ID="txtTotal" runat="server" EnableTheming="false" Text="0" Style="display: none" />
                                                <asp:Label runat="server" ID="lblDtlRemark2" Text="<%$ resources:AddlRemarks %>"
                                                    AssociatedControlID="txtDtlRemark2" CssClass="lbl-hgt-80"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark2" TabIndex="27" MaxLength="480" TextMode="MultiLine"
                                                    CssClass="multiline" ></asp:TextBox><%--onkeypress="return this.value.length<1000" onpaste="return this.value.length<1000--%>
                                                <asp:CheckBox ID="chkAddtlRemarkToAll" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                    TabIndex="27" CssClass="span-normal null-graph minw-17" />
                                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="28"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('scDetails');"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="scDetails"
                                                    SkinID="plus" />
                                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="29"
                                                    OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="PageAction_Entry"
                                                    SkinID="cancel" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divPackingMaterialDetails" style="display: none">
                                <table class="table-devide" id="Table2">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lbl_Prd_Category" runat="server" AssociatedControlID="txtPrdCategory"
                                                    Text="Category">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPrdCategory" runat="server" TabIndex="10" MaxLength="200" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPrdCategory" runat="server" />
                                                <%--    <asp:Button ID="Button1" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                    EnableTheming="false" Style="display: none" />--%>
                                                <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtPrdCategory" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lbl_Prd_Type" runat="server" AssociatedControlID="txtPrdType" Text="Type">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPrdType" runat="server" TabIndex="10" MaxLength="200" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPrdType" runat="server" />
                                           <%--     <asp:Button ID="btnPrdType" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                    EnableTheming="false" Style="display: none" />--%>
                                                <asp:RequiredFieldValidator ID="vrfType" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scPackMaterialDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtPrdType" Display="Dynamic" Text="*"
                                                    ErrorMessage="Select Type"></asp:RequiredFieldValidator>
                                                <%--   <asp:Button ID="Button2" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                    EnableTheming="false" Style="display: none" />--%>
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtPrdType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lbl_Prd_PackSpeck" runat="server" AssociatedControlID="txtPrdPackSpec"
                                                    Text="Packing Material">                                                     
                                                </asp:Label>

                                                <asp:Label ID="lbl_product" runat="server" AssociatedControlID="txtPrdPackSpec"
                                                    Text="Product">
                                                </asp:Label>

                                                <asp:TextBox ID="txtPrdPackSpec" runat="server" TabIndex="10" MaxLength="200" CssClass="select-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPrdPackSpec" runat="server" />
                                                <asp:HiddenField ID="hdfPrdPackSpecPK" runat="server" />
                                                <asp:Button ID="btnSelectPackSpec" runat="server" OnClick="ActionHandler" CommandName="PACKSPECSELECTED"
                                                    EnableTheming="false" Style="display: none" />
                                                <asp:RequiredFieldValidator ID="vrfPackingMaterial" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scPackMaterialDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtPrdPackSpec" Display="Dynamic" Text="*"
                                                    ErrorMessage="Select Packing Materail"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblSpecCode" runat="server" AssociatedControlID="txtSpecCode" Text="Code">
                                                </asp:Label>
                                                <asp:TextBox ID="txtSpecCode" runat="server" TabIndex="10" MaxLength="200" CssClass="select-medium input-disabled"></asp:TextBox>
                                                <asp:HiddenField ID="hdfspeccode" runat="server" />
                                                <asp:Label runat="server" ID="lblSpecReqByDate" Text="Date" AssociatedControlID="txtSpecReqByDate"
                                                    CssClass="middle-lbl-xsmall-f"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtSpecReqByDate" MaxLength="12" CssClass="input-small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="15"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfSpecReqByDate" />
                                                <div class="starwrap" style="margin-left: 14px!important;">
                                                    <asp:RequiredFieldValidator ID="vrfSpecReqByDate" CssClass="star" SetFocusOnError="false"
                                                        ValidationGroup="scPackMaterialDetails" EnableClientScript="true" runat="server"
                                                        ControlToValidate="txtSpecReqByDate" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ReqByDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreSpecReqByDate" CssClass="star" ValidationGroup="scPackMaterialDetails"
                                                        runat="server" ControlToValidate="txtSpecReqByDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_ReqByDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Static" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblspecqty" runat="server" AssociatedControlID="txtspecqty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%></asp:Label>
                                                <asp:TextBox ID="txtspecqty" runat="server" onblur="CalculateSpecAmount(this);" CssClass="input-small numeric"
                                                    TabIndex="17"></asp:TextBox>
                                                <asp:HiddenField ID="hdfspecqty" runat="server" />
                                                <asp:HiddenField ID="hdfspecuom" runat="server" />
                                                <asp:HiddenField ID="hdfspecsaleuom" runat="server" />
                                                <asp:HiddenField ID="hdfspecuomcode" runat="server" />
                                                <asp:HiddenField ID="hdfapstotalpcs" runat="server" />
                                                <div class="starwrap">
                                                    <%-- <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>--%>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="star" SetFocusOnError="true"
                                                        Style="padding: 0px !important;" ValidationGroup="scPackMaterialDetails" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtspecqty" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:QuantityValidation ID="QuantityValidation1" runat="server" ControlToValidate="txtspecqty"
                                                        Style="padding: 0px !important;" NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scPackMaterialDetails"
                                                        NonZero="true">
                                                    </cc1:QuantityValidation>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scCost" EnableClientScript="true" runat="server" ControlToValidate="txtspecqty"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Label ID="Label7" runat="server" AssociatedControlID="SPECQTY" Text="Uom" CssClass="middle-lbl-small-b2"><%--<%$ resources:Quantity %>--%></asp:Label>
                                                <asp:TextBox ID="SPECQTY" runat="server" CssClass="input-small numeric input-disabled"
                                                    Enabled="true" TabIndex="23"></asp:TextBox>
                                                <asp:HiddenField ID="hdf_SPECQTY" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblspecprice" runat="server" AssociatedControlID="txtspecprice" Text="Unit Price"><%--<%$ resources:Quantity %>--%></asp:Label>
                                                <asp:TextBox ID="txtspecprice" runat="server" onblur="CalculateSpecAmount(this);"
                                                    CssClass="input-small numeric" Enabled="true" TabIndex="17"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfspecprice" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scPackMaterialDetails" EnableClientScript="true" runat="server"
                                                        ControlToValidate="txtspecprice" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:RateValidation ID="vrfratespecprice" runat="server" ControlToValidate="txtspecprice"
                                                        ErrorMessage="<%$ resources:Err_Rate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scPackMaterialDetails"
                                                        NonZero="false">
                                                    </cc1:RateValidation>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scCost" EnableClientScript="true" runat="server" ControlToValidate="txtspecprice"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <asp:HiddenField ID="hdfspecprice" runat="server" />
                                                <asp:Label ID="lbl_spec_amount" runat="server" AssociatedControlID="txtspecamount"
                                                    CssClass="middle-lbl-small-b2" Text="Amount"><%--<%$ resources:Quantity %>--%></asp:Label>
                                                <asp:TextBox ID="txtspecamount" runat="server" CssClass="input-small numeric input-disabled"
                                                    TabIndex="17"></asp:TextBox>
                                                <asp:HiddenField ID="hdfspecamount" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblspecremark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtspecremark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtspecremark" TabIndex="27" MaxLength="480" TextMode="MultiLine"
                                                    CssClass="multiline"></asp:TextBox>
                                                <%--  <asp:CheckBox ID="CheckBox1" runat="server" TextAlign="Left" ToolTip="<%$ resources:LotNoApplyforAll%>"
                                                    TabIndex="27" CssClass="span-normal null-graph minw-17" />--%>
                                                <asp:ImageButton runat="server" ID="btnSpecAdd" CommandName="ADDPACKSPECITEM" TabIndex="28"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('scPackMaterialDetails');"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="scPackMaterialDetails"
                                                    SkinID="plus" />
                                                <asp:ImageButton runat="server" ID="btnSpecClear" CommandName="CLEARPACKMATERIALITEM"
                                                    TabIndex="29" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>"
                                                    CommandArgument="PageAction_Entry" SkinID="cancel" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap scroll-container">
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    Width="2123px" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField> <%--0--%>
                                            <ItemTemplate>
                                                <asp:ImageButton CssClass="nomargin" ID="imbRuleUp" SkinID="move-up" runat="server"
                                                    OnClick="ActionHandler" CommandName="MOVEUP" ToolTip="Move Up" CommandArgument='<%# Eval("SOD_SL_NO") %>'
                                                    TabIndex="13"></asp:ImageButton>
                                                <asp:ImageButton CssClass="nomargin" ID="imbRuleDown" SkinID="move-down" runat="server"
                                                    OnClick="ActionHandler" CommandName="MOVEDOWN" ToolTip="Move Down" CommandArgument='<%# Eval("SOD_SL_NO") %>'
                                                    TabIndex="14"></asp:ImageButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="15px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>"><%--1--%>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                <asp:HiddenField ID="hdfCusItemPK" runat="server" Value='<%#Eval("SOD_CUST_ITEM") %>' />
                                                <asp:Label ID="lblBrandName" runat="server" Text='<%#Eval("SOD_CUST_ITEM_TEXT")%>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CUST_ITEM_TEXT"))) %>'></asp:Label>
                                                <%--<asp:Label ID="lblBrandName" runat="server" Text='<%#Eval("SOD_CUST_ITEM_TEXT")%>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CUST_ITEM_TEXT"))) + (string.IsNullOrEmpty(Convert.ToString(Eval("SOD_CUST_ITEM_CODE"))) ? "" : (" - " + HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CUST_ITEM_CODE"))))) %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="590px" Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM%>"> <%--2--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ?
                                                   ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_UOM_TEXT"),10)
                                                  : ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_SALE_UOM_TEXT"),10) %>'
                                                    ToolTip='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ?
                                                     ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_UOM_TEXT"),10) :
                                                    HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_SALE_UOM_TEXT"))) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("SOD_UOM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-CssClass="amount-numeric"> <%--3--%>
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="lblBrandQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumber(Eval("SOD_SALE_QTY")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("SOD_SALE_QTY")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblBrandQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumberWithComma(Eval("SOD_SALE_QTY")) %>'
                                                    ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_SALE_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="75px" CssClass="amount-numeric" />
                                            <%--<FooterStyle CssClass="amount-numeric" />--%>
                                            <%--<FooterTemplate>
                                                <asp:Label ID="lblBrandItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QuantityPcs%>" HeaderStyle-CssClass="amount-numeric"> <%--4--%>
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumber(Eval("SOD_QTY")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("SOD_QTY")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'
                                                    ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="75px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CartonsOrBags %>" HeaderStyle-CssClass="amount-numeric"> <%--5--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCartonsOrBags" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="55px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalCarton" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%------------------ CBM-----------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:CBM1 %>" HeaderStyle-CssClass="amount-numeric"
                                            ItemStyle-HorizontalAlign="Right"> <%--6--%>
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="Label6" runat="server" Text='<%# Convert.ToDouble(Eval("SOD_QTY")) %>'></asp:Label>
                                          <asp:Label ID="Label7" runat="server" Text='<%# Convert.ToDouble(Eval("APS_TOTAL_PCS")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblCBM" runat="server" Text='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ||Eval("SOD_IS_PACK_MAT").ToString() == "2" ? "-" :   Math.Round(Convert.ToDouble(Eval("SOD_QTY"))/ Convert.ToDouble(Eval("APS_TOTAL_PCS"))*  Convert.ToDouble(Eval("CBM")), 4).ToString("N4")%>'
                                                    ToolTip='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ? "-" : Math.Round(Convert.ToDouble(Eval("SOD_QTY"))/ Convert.ToDouble(Eval("APS_TOTAL_PCS"))*  Convert.ToDouble(Eval("CBM")), 4).ToString("N4")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric"> <%--7--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemRate" runat="server" Text='<%# GetFormattedRate(Eval("SOD_RATE")) %>'
                                                    ToolTip='<%# GetFormattedRate(Eval("SOD_RATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <%--<asp:Label ID="lblTotalCBM" runat="server"></asp:Label>--%>
                                                <asp:ImageButton runat="server" ID="imbShowAlldtlCost" SkinID="estimate-icon" ToolTip="<%$ resources:ShowAllSaleCost %>"
                                                    OnClick="ActionHandler" CommandName="SHOWSALESCOST" CommandArgument="PageAction_Entry"
                                                    OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric"> <%--8--%>
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblItemAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_AMOUNT")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblItemAmount" runat="server" Text='<%# Eval("SOD_AMOUNT", "{0:c}")%>'
                                                    ToolTip='<%# Eval("SOD_AMOUNT", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalAmount" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric"> <%--9--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDiscount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_DISCOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_DISCOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblDiscountTotal"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric"> <%--10--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTax" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_TAX")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_TAX")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTaxTotal"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Pay Now--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric"> <%--11--%>
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblItemTotal" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_NET_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_NET_AMOUNT")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblItemTotal" runat="server" Text='<%# Eval("SOD_NET_AMOUNT", "{0:c}")%>'
                                                    ToolTip='<%# Eval("SOD_NET_AMOUNT", "{0:c}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblSubTotalFooter" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField> <%--12--%>
                                            <HeaderTemplate>
                                                <%= GetLocalResourceObject("LotNo").ToString() %>
                                                <%--                                                <asp:ImageButton ID="imbAutogenerateLotNo" SkinID="gene-number" runat="server" OnClick="ActionHandler"
                                                    TabIndex="21" CommandArgument="PageAction_Entry" ToolTip="<%$ resources:AutogenerateLotNo %>"
                                                    CommandName="AUTOGENERATELOTNO" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                    CssClass="floatRight" />--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_LOT_NO"), 17) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_NO"))) %>'
                                                    ID="lblItemLotNo"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LotSize %>"> <%--13--%>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_LOT_SIZE"), 10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_SIZE"))) %>'
                                                    ID="lblItemLotSize"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ArtWork %>"> <%--14--%>
                                            <ItemTemplate>
                                                <div class="link-inline">
                                                    <asp:Label ID="lblNewArtWork" runat="server" Text="[NEW]"></asp:Label>


                                                    <a id="lnkLstArtWorkPC" runat="server" href="#" visible="false" target="_blank"></a><a id="lnkLstArtWorkIB" runat="server" href="#" visible="false" target="_blank"></a><a id="lnkLstArtWorkIC" runat="server" href="#" visible="false" target="_blank"></a><a id="lnkLstArtWorkZB" runat="server" href="#" visible="false" target="_blank"></a><a id="lnkLstArtWorkMC" runat="server" href="#" visible="false" target="_blank"></a><a id="lnkLstArtWorkSC" runat="server" href="#" visible="false" target="_blank"></a>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="175px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReqdShipDate1 %>"> <%--15--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemReqdShipDate" runat="server" Text='<%#Eval("SOD_REQUIRED_BY") %>'
                                                    ToolTip='<%# Eval("SOD_REQUIRED_BY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProductCode %>"> <%--16--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ? HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CUST_ITEM_CODE")))
                                                : ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_CODE"),18) %>'
                                                    ToolTip='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ? HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CUST_ITEM_CODE"))) 
                                                   :  HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_ITEM_CODE")))%>'></asp:Label>


                                                <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("SOD_ITEM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:remarks %>"> <%--17--%>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_REMARKS"), 14) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_REMARKS"))) %>'
                                                    ID="lblItemRemarks"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CaseMark %>"> <%--18--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCaseMark" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CASE_MARK"))) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CASE_MARK"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="<%$ resources:HSNLabel %>"> <%--19--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblHSNNo" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_HSN_CODE_TEXT"))) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_HSN_CODE_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                    CommandArgument="PageAction_Entry" SkinID="imbeditgrid" ToolTip="Edit" TabIndex="30"
                                                    OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="31" OnPreRender="btnAction_PreRender"
                                                    OnLoad="btnAction_Load" />
                                                <asp:ImageButton ID="btnViewItem" runat="server" OnClick="ActionHandler" CommandName="VIEW"
                                                    SkinID="btnview" ToolTip="<%$ resources:ViewAddPackDtls %>" TabIndex="31" />
                                                <asp:ImageButton runat="server" ID="imbDtlRef" SkinID="reference-detail" OnClick="ActionHandler"
                                                    TabIndex="31" CommandName="ITEMREFDTL" Visible="false" ToolTip="<%$ resources:ItemRefTooltip %>"
                                                    CommandArgument='<%#Eval("SOD_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="55px" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="width: 84%; text-align: right">
                                                <div class="floatLeft">
                                                    <asp:Label ID="lblTotalCBMLbl" Font-Bold="true" runat="server" Text="<%$ resources:CBM%>"
                                                        AssociatedControlID="lblTotalCBMLbl"></asp:Label>
                                                    <asp:Label ID="lblTotalCBM" AssociatedControlID="lblTotalCBM" runat="server"></asp:Label>
                                                </div>
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    TabIndex="32" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>--%>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <div class="floatLeft" style="display: none;">
                                                    <asp:Label ID="lblHGrossWt" Font-Bold="true" runat="server" Text="<%$ resources:GrossWeight%>"
                                                        AssociatedControlID="lblHGrossWt"></asp:Label>
                                                    <asp:Label ID="lblGrossWeight" AssociatedControlID="lblGrossWeight" runat="server"></asp:Label>
                                                </div>
                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgShippingCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    TabIndex="33" ToolTip="<%$ resources:ShippingCharge %>" CommandName="SHIPPINGHEADER" />
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w97 numeric  input-disabled"
                                                    MaxLength="16" Enabled="false" TabIndex="25"></asp:TextBox>
                                                <%--  onchange="CalculateTotal(this);"--%>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                        ErrorMessage="<%$ resources:Err_Valid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="so">
                                                    </cc1:AmountValidation>
                                                    <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    TabIndex="34" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="so"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtHdrTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_TotalTax%>"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w97 numeric" onchange="CalculateTotal(this);"
                                                    MaxLength="16" TabIndex="35"></asp:TextBox>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                        ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="so">
                                                    </cc1:AmountValidation>
                                                    <%--<asp:RequiredFieldValidator ID="vrfPriceAdj" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtPriceAdj"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PriceAdj%>"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ShippingDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowShippingDetails" OnClientClick="javascript:return ShowHideShippingDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowShippingDetails %>" />
                                <asp:ImageButton runat="server" ID="imbHideShippingDetails" OnClientClick="javascript:return ShowHideShippingDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideShippingDetails %>" />
                                <asp:HiddenField ID="hdfIsShippingDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divShippingDetails" style="display: none">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="36" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="txtFromPort" Text="<%$ resources:FromPort %>"></asp:Label>
                                                <%-- <asp:DropDownList ID="ddlFromPort" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="39" Visible="false">
                                                </asp:DropDownList>--%>
                                                <asp:TextBox runat="server" ID="txtFromPort" CssClass="input-halfsmall-a"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfFromPortID" />
                                                <%--  <asp:RequiredFieldValidator ID="vrfFromPort" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtFromPort"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromPort %>" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                    Text="<%$ resources:Transhipment %>"></asp:Label>
                                                <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="41" CssClass="select-half">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblShipmentDate" Text="<%$ resources:ShipmentDate%>"
                                                    AssociatedControlID="txtShipmentDate"></asp:Label>
                                                <asp:TextBox ID="txtShipmentDateText" runat="server" TabIndex="37" CssClass="input-small"
                                                    Text="<%$ resources:ShipmentText %>" MaxLength="100"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtShipmentDate" CssClass="input-small" TabIndex="38"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtToPort" runat="server" TabIndex="40" CssClass="input-halfsmall-a"
                                                    Visible="false"></asp:TextBox>
                                                <asp:DropDownList runat="server" ID="ddlToPort" CssClass="select-half" Visible="false">
                                                </asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdfToPortID" />
                                                <asp:Label ID="lblPortofDischarge" runat="server" AssociatedControlID="txtPortofDischarge"
                                                    Text="<%$ resources:FinalDestination %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPortofDischarge" runat="server" TabIndex="42" MaxLength="100"
                                                    CssClass="input-halfsmall-a"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblConsigneeDetails" runat="server" AssociatedControlID="ddlConsigneeDetails"
                                                    Text="<%$ resources:ConsigneeDetails %>"></asp:Label>
                                                <asp:DropDownList ID="ddlConsigneeDetails" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="43">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblConsigneeDtl" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtConsigneeDetails" MaxLength="500" onkeydown="return EnableArrowKey(event)"
                                                    onpaste="return false;" TextMode="MultiLine" CssClass="multiline-m1col input-disabled input-halfsmall-a"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCNEName" runat="server" />
                                                <asp:HiddenField ID="hdfCNEAddress" runat="server" />
                                                <asp:HiddenField ID="hdfCNECountry" runat="server" />
                                                <asp:HiddenField ID="hdfCNECountryText" runat="server" />
                                                <asp:HiddenField ID="hdfCNEZip" runat="server" />
                                                <asp:HiddenField ID="hdfCNEPhone" runat="server" />
                                                <asp:HiddenField ID="hdfCNEMobile" runat="server" />
                                                <asp:HiddenField ID="hdfCNEFax" runat="server" />
                                                <asp:HiddenField ID="hdfCNEEmail" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>"
                                                    AssociatedControlID="ddlCustAddress"></asp:Label>
                                                <asp:DropDownList ID="ddlCustAddress" runat="server" TabIndex="44" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ActionHandler" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label5" runat="server" AssociatedControlID="txtShippingAddress"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TextMode="MultiLine"
                                                    EnableTheming="false" CssClass="multiline-m1col input-disabled input-halfsmall-a"
                                                    onkeydown="return EnableArrowKey(event)" onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreShippingAddress" runat="server" ControlToValidate="txtShippingAddress"
                                                    ErrorMessage="<%$ Resources:Err_ShippingAddress %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfShpName" runat="server" />
                                                <asp:HiddenField ID="hdfShpAddress" runat="server" />
                                                <asp:HiddenField ID="hdfShpCountry" runat="server" />
                                                <asp:HiddenField ID="hdfShpCountryText" runat="server" />
                                                <asp:HiddenField ID="hdfShpZip" runat="server" />
                                                <asp:HiddenField ID="hdfShpPhone" runat="server" />
                                                <asp:HiddenField ID="hdfShpMobile" runat="server" />
                                                <asp:HiddenField ID="hdfShpFax" runat="server" />
                                                <asp:HiddenField ID="hdfShpEmail" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblNotifyParty" runat="server" AssociatedControlID="ddlNotifyParty"
                                                    Text="<%$ resources:NotifyParty %>"></asp:Label>
                                                <asp:DropDownList ID="ddlNotifyParty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    TabIndex="45" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblNotifyPrty" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNotifyParty" MaxLength="500" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-disabled input-halfsmall-a" onkeydown="return EnableArrowKey(event)"
                                                    onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:HiddenField ID="hdfNPName" runat="server" />
                                                <asp:HiddenField ID="hdfNPAddress" runat="server" />
                                                <asp:HiddenField ID="hdfNPCountry" runat="server" />
                                                <asp:HiddenField ID="hdfNPCountryText" runat="server" />
                                                <asp:HiddenField ID="hdfNPZip" runat="server" />
                                                <asp:HiddenField ID="hdfNPPhone" runat="server" />
                                                <asp:HiddenField ID="hdfNPMobile" runat="server" />
                                                <asp:HiddenField ID="hdfNPFax" runat="server" />
                                                <asp:HiddenField ID="hdfNPEmail" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblAgent" runat="server" AssociatedControlID="ddlAgent" Text="<%$ resources:Agent %>">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlAgent" runat="server" TabIndex="46" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ActionHandler" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnTooltip" runat="server" OnClick="ActionHandler" CommandName="TOOLTIP"
                                                    Style="display: none" EnableTheming="false" />
                                                <asp:HiddenField ID="hdfAgentName" runat="server" />
                                                <asp:HiddenField ID="hdfAgentAddress" runat="server" />
                                                <asp:HiddenField ID="hdfAgentCountry" runat="server" />
                                                <asp:HiddenField ID="hdfAgentCountryText" runat="server" />
                                                <asp:HiddenField ID="hdfAgentZip" runat="server" />
                                                <asp:HiddenField ID="hdfAgentPhone" runat="server" />
                                                <asp:HiddenField ID="hdfAgentMobile" runat="server" />
                                                <asp:HiddenField ID="hdfAgentFax" runat="server" />
                                                <asp:HiddenField ID="hdfAgentEmail" runat="server" />
                                                <asp:Label ID="lblShppingIntimationto" runat="server" AssociatedControlID="txtShppingIntimationto"
                                                    Text="<%$ resources:ShppingIntimationto %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtShppingIntimationto" runat="server" TabIndex="47" MaxLength="100"
                                                    CssClass="input-halfsmall-a"></asp:TextBox>
                                                <asp:Label ID="lblShppingIntimationtoFax" runat="server" AssociatedControlID="txtShppingIntimationtoFax"
                                                    Text="<%$ resources:ShppingIntimationtoFax %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtShppingIntimationtoFax" runat="server" TabIndex="48" MaxLength="100"
                                                    CssClass="input-halfsmall-a"></asp:TextBox>
                                                <asp:Label ID="lblContainerSize" runat="server" AssociatedControlID="txtContainerSize"
                                                    Text="<%$ resources:ContainerSize %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtContainerSize" runat="server" TabIndex="49" MaxLength="100" CssClass="input-halfsmall-a"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("Terms").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowTerms" OnClientClick="javascript:return ShowHideTerms(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowTerms %>" />
                                <asp:ImageButton runat="server" ID="imbHideTerms" OnClientClick="javascript:return ShowHideTerms();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideTerms %>" />
                                <asp:HiddenField ID="hdfIsTermsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divTermDetails" style="display: none">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlShipmentTerms"
                                                    Text="<%$ resources:ShipmentTerms %>"></asp:Label>
                                                <%--<asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    TabIndex="49" CssClass="select-half">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownList ID="ddlShipmentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    TabIndex="49" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlShipmentTerms"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ShipmentTermsVal %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <%--<dv class="Del">
                                                 <asp:Label ID="lblDelMan" runat="server" Text="*" CssClass="star" Visible="false" ></asp:Label>
                                                </dv>--%>
                                                <asp:Label ID="Label1" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="51" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                    Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    TabIndex="50" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="52" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                    Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    TabIndex="53" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="56" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBankDetails" runat="server" AssociatedControlID="ddlBankDetails"
                                                    Text="<%$ resources:BankDetails %>"></asp:Label>
                                                <asp:DropDownList ID="ddlBankDetails" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-small-e2" TabIndex="54">
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkNeedAdvPay" runat="server" TabIndex="55" Text="<%$resources:NeedAdvPay%>"
                                                    TextAlign="Left" Checked="true" />
                                                <span style="width: 100px; visibility: hidden;"></span><a id="lnkTerms" runat="server"
                                                    onclick="ShowContract();" href="#">
                                                    <%=GetLocalResourceObject("ContractTerms").ToString() %>
                                                </a>
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSaleOrderSubtype"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderSubType %>"
                                                    InitialValue="-1">
                                                </asp:RequiredFieldValidator>--%>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblAgentN" runat="server" Visible="true" AssociatedControlID="ddlBankDetails"
                                                    Text="Sales Agent"></asp:Label>
                                                <asp:DropDownList ID="ddlAgentN" CssClass="select-half" Visible="true" runat="server"
                                                    TabIndex="57">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfagent" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlAgentN"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Agent %>" Enabled="false" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("RelatedInformation").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowRelatedInformation" OnClientClick="javascript:return ShowHideRelatedInformation(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowRelatedInformation %>" />
                                <asp:ImageButton runat="server" ID="imbHideRelatedInformation" OnClientClick="javascript:return ShowHideRelatedInformation();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideRelatedInformation %>" />
                                <asp:HiddenField ID="hdfIsRelatedInformationVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divRelatedInformation" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblInspection" runat="server" AssociatedControlID="ddlInspection"
                                                    Text="<%$ resources:Inspection %>"></asp:Label>
                                                <asp:DropDownList ID="ddlInspection" runat="server" TabIndex="58" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfInspection" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlInspection"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Inspection %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblPackingInstruction" runat="server" AssociatedControlID="txtPackingInstruction"
                                                    Text="<%$ resources:PackingInstruction %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPackingInstruction" runat="server" TabIndex="60" MaxLength="100"
                                                    CssClass="input-halfsmall-a"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblExportDoc" runat="server" AssociatedControlID="ddlExportDoc" Text="<%$ resources:ExportDoc %>"></asp:Label>
                                                <asp:DropDownList ID="ddlExportDoc" runat="server" TabIndex="59" CssClass="select-half">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfExportDoc" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlExportDoc"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExportDoc %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblOriginofGoods" runat="server" AssociatedControlID="ddlOriginofGoods"
                                                    Text="<%$ resources:OriginofGoods %>"></asp:Label>
                                                <asp:DropDownList ID="ddlOriginofGoods" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    TabIndex="61" CssClass="select-half">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-devide">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" Height="55px" TabIndex="62"
                                                    TextMode="MultiLine" CssClass="multiline-m1col input-halfsmall-a"></asp:TextBox>
                                                <%--  onkeydown="limitText(this,500);"   onkeyup="limitText(this,500);"--%>
                                                <%--<asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                    ErrorMessage="<%$ Resources:Err_Remarks %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b" runat="server" id="divAdditionalPackDtlsSearch" visible="false">
                                <h1>
                                    <%= GetLocalResourceObject("AdditionalPackDtls").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowAdditionalPackDtls" OnClientClick="javascript:return ShowHideAdditionalPackDtls(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowAdditionalPackDtls %>" />
                                <asp:ImageButton runat="server" ID="imbHideAdditionalPackDtls" OnClientClick="javascript:return ShowHideAdditionalPackDtls();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideAdditionalPackDtls %>" />
                                <asp:HiddenField ID="hdfIsAdditionalPackDtlsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divAdditionalPackDtlsHdr" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <div class="grupitem-bg2 float-left minw-50per">
                                                    <asp:Label ID="lblNetting" runat="server" AssociatedControlID="chkNetting" Text="<%$ resources:Netting %>"
                                                        CssClass="lbl-49perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkNetting" TabIndex="62" />
                                                </div>
                                                <div class="grupitem-bg3">
                                                    <asp:Label ID="lblInsulation" runat="server" AssociatedControlID="chkInsulation"
                                                        Text="<%$ resources:Insulation %>" CssClass="lbl-32-2perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkInsulation" TabIndex="62" />
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <div class="grupitem-bg5">
                                                    <asp:Label ID="lblPreShipment" runat="server" AssociatedControlID="chkPreShipment"
                                                        Text="<%$ resources:PreShipment %>" CssClass="margnbotm0">
                                                    </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkPreShipment" TabIndex="63" />
                                                    <asp:Label ID="lblStandard" runat="server" AssociatedControlID="ddlStandard" Text="<%$ resources:Standard %>"
                                                        CssClass="lbl-27perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlStandard" CssClass="select-small-e2 margnbotm0"
                                                        TabIndex="63">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <div class="grupitem-bg2">
                                                    <asp:Label ID="lblAddlPackaging" runat="server" AssociatedControlID="chkAddlPackaging"
                                                        Text="<%$ resources:AddlPackaging %>" CssClass="margnbotm0">
                                                    </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkAddlPackaging" TabIndex="62" />
                                                    <asp:Label ID="lblInner" runat="server" AssociatedControlID="txtInner" Text="<%$ resources:Inner %>"
                                                        CssClass="lbl-12perc margnbotm0">
                                                    </asp:Label>
                                                    <asp:TextBox runat="server" ID="txtInner" CssClass="input-xsmall-b margnbotm0" TabIndex="62"
                                                        MaxLength="20"></asp:TextBox>
                                                    <asp:Label ID="lblCarton" runat="server" AssociatedControlID="txtCarton" Text="<%$ resources:Carton %>"
                                                        CssClass="lbl-12perc margnbotm0"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtCarton" CssClass="input-xsmall-b margnbotm0" TabIndex="62"
                                                        MaxLength="20"></asp:TextBox>
                                                </div>
                                                <div class="grupitem-bg5 w30perc">
                                                    <asp:Label ID="lblProteinTest" runat="server" AssociatedControlID="chkProteinTest"
                                                        Text="<%$ resources:ProteinTest %>" CssClass="lbl-82-2perc margnbotm0"> </asp:Label>
                                                    <asp:CheckBox runat="server" ID="chkProteinTest" TabIndex="63" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("AttachDocuments").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowAttachDocs" OnClientClick="javascript:return ShowHideAttachDocs(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowAttachDocs %>" />
                                <asp:ImageButton runat="server" ID="imbHideAttachDocs" OnClientClick="javascript:return ShowHideAttachDocs();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideAttachDocs %>" />
                                <asp:HiddenField ID="hdfIsAttachDocsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divAttachDocs" style="display: none">
                                <div class="divcol-S">
                                    <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"></asp:Label>
                                    <div class="fileupload-main">
                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="63" CssClass="margn-rgt0 upload-area" />
                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_File_Upload%>">                                                        
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <a id="anchorFile" runat="server" target="_blank" tabindex="55"></a>
                                    <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="64"
                                        OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                        CommandArgument="PageAction_Entry" ToolTip="<%$resources:ErpRes,Add%>" ValidationGroup="upload"
                                        Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$resources:UploadPageSize%>"
                                        AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                        OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$resources:SlNo%>">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                    <%-- <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("DOC_SEQ_NO") %>' ToolTip='<%# Eval("DOC_SEQ_NO") %>'></asp:Label>--%>
                                                    <asp:HiddenField runat="server" ID="hdfPK" Value='<%#Eval("DOC_PK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:File %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFile" runat="server" Text='<%#Eval("DOC_NAME") %>' ToolTip='<%#Eval("DOC_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="92%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="file-details">
                                                <ItemTemplate>
                                                    <a runat="server" id="fileView" class="download-icon nomargin" style="margin-right: -1px!important;"
                                                        title="<%$resources:View %>" target="_blank" href='<%#Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="file-details">
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                        SkinID="edit-icon" ToolTip="Edit" Style="margin-right: 3px!important;" /><%--CommandArgument="PageAction_Entry" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" --%>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="file-details">
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD"
                                                        SkinID="delete-icon" ToolTip="Delete" OnClientClick="return ShowDeleteConfirmationMsg(this);" /><%--CommandArgument="PageAction_Entry" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender"--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <%---------------------- Reference Details Popup Start ------------------%>
                            <div id="divItemRefDtl" style="display: none">
                                <div class="content-wrapper">
                                    <div class="gridwrap">
                                        <div class="detail-co3" id="divPopupBrand" runat="server">
                                            <div class="w100perc">
                                                <asp:Label ID="lblPopupBrnd" runat="server" AssociatedControlID="lblPopupBrndName"
                                                    Text="<%$ resources:BrandNameHdr %>" CssClass="margnbotm0"> </asp:Label>
                                                <asp:Label ID="lblPopupBrndName" runat="server" Text="<%$ resources:ProteinTest %>"
                                                    CssClass="bold margnbotm0"> </asp:Label>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <asp:GridView runat="server" ID="grdItemRefDtl" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRefBrandName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("CIM_BRAND_NAME")) %>'
                                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Convert.ToString(Eval("CIM_BRAND_NAME"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxType %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrxType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("TRX_TYPE_TEXT")) %>'
                                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Convert.ToString(Eval("TRX_TYPE_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="55%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrxNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("TRX_NO")) %>'
                                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Convert.ToString(Eval("TRX_NO"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TrxDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrxDate" runat="server" Text='<%# Eval("TRX_DATE") %>' ToolTip='<%# String.IsNullOrEmpty(Convert.ToString(Eval("TRX_DATE"))) ? "" : Eval("TRX_DATE", Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%---------------------- Reference Details Popup End ------------------%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="so" runat="server" />
                    <asp:ValidationSummary ID="vsDetails" ValidationGroup="scDetails" runat="server" />
                    <asp:ValidationSummary ID="vsPackMaterail" ValidationGroup="scPackMaterialDetails"
                        runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="vsSalesCost" ValidationGroup="scCost" runat="server" />
                    <asp:ValidationSummary ID="vsOrderQty" ValidationGroup="OrderQty" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
            <div id="divAlert" style="display: none">
                <uc2:Alert ID="ucrAlert" runat="server" />
            </div>
            <div id="divTerms" class="max-425" style="display: none">
                <asp:Literal ID="ltrTerms" runat="server"></asp:Literal>
            </div>
            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
            <div id="divItemTax" style="display: none">
                <div class="Button-container-popup">
                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" TabIndex="59" />
                </div>
                <div class="content-wrapper">
                    <%-- *********************Checkbox Region Start*******************************************************************--%>
                    <div id="divTaxApplicableAmount" style="display: none" runat="server">
                        <label for="chkSubTotal" style="width: 148px">
                            <%=Resources.Controls.SubTotal%>
                        </label>
                        <asp:CheckBox ID="chkSubTotal" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                            AutoPostBack="true" />
                        <label for="chkDiscount" style="width: 120px">
                            <%=Resources.Controls.Discounts%>
                        </label>
                        <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                            AutoPostBack="true" />
                        <label for="chkOtherCharges" style="width: 125px">
                            <%=Resources.Controls.OtherCharges%>
                        </label>
                        <asp:CheckBox ID="chkOtherCharges" runat="server" Checked="true" OnCheckedChanged="ActionHandler"
                            AutoPostBack="true" />
                    </div>
                    <%--   *******************End Check Box Region ****************************************************************--%>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-P">
                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:Amount %>"
                                        AssociatedControlID="txtPopupItemAmount"></asp:Label><asp:TextBox ID="txtPopupItemAmount"
                                            CssClass="input-w70 numeric" runat="server" EnableViewState="false" Enabled="false"
                                            MaxLength="11"></asp:TextBox><div class="clear">
                                            </div>
                                    <div runat="server" id="dvPerc">
                                        <asp:Label ID="lblPerc" runat="server" Text="<%$ resources:DiscPerc %>" AssociatedControlID="txtTaxPerc"></asp:Label><asp:TextBox
                                            ID="txtTaxPerc" TabIndex="55" runat="server" CssClass="input-w70 numeric" EnableViewState="false"
                                            MaxLength="15" onChange="CalcTax();"></asp:TextBox>
                                    </div>
                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Charge %>" AssociatedControlID="txtPopupAmount"></asp:Label><asp:TextBox
                                        ID="txtPopupAmount" TabIndex="55" runat="server" CssClass="input-w70 numeric"
                                        EnableViewState="false" Enabled="false" MaxLength="15"></asp:TextBox><div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                            </asp:RequiredFieldValidator><cc1:AmountValidation ID="vamTaxAmt" runat="server"
                                                ControlToValidate="txtPopupAmount" ErrorMessage="<%$ resources:Err_Amount_Valid %>"
                                                NumberDigits="11" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                ValidationGroup="tax"></cc1:AmountValidation>
                                        </div>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-P">
                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label><asp:DropDownList
                                        ID="ddlPopupTaxType" TabIndex="54" runat="server" CssClass="medium" EnableViewState="true"
                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblPopupOther" runat="server" Text="Name" AssociatedControlID="txtPopupOther"></asp:Label><asp:TextBox
                                        ID="txtPopupOther" runat="server" TabIndex="56" CssClass="medium" EnableViewState="false"
                                        MaxLength="100" Enabled="false"></asp:TextBox><asp:RequiredFieldValidator ID="vrfPopupOther"
                                            CssClass="star" SetFocusOnError="true" ValidationGroup="tax" EnableClientScript="true"
                                            runat="server" ControlToValidate="txtPopupOther" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Name %>">
                                        </asp:RequiredFieldValidator><asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew"
                                            runat="server" OnClick="ActionHandler" CommandArgument="PageAction_Entry" ValidationGroup="tax"
                                            CommandName="TAXADD" TabIndex="57" OnClientClick="javascript:ValidatePageNow('tax')" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("SLT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_NAME"))) %>'></asp:Label><asp:HiddenField
                                            ID="hdfTaxName" runat="server" Value='<%#Eval("SLT_NAME") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("SLT_PK") %>' />
                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("SLT_TAX") %>' />
                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT"))) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode( HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT")))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscPer" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("SLT_DISC_PERC")) %>'
                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("SLT_DISC_PERC")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="22%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("SLT_TAX_AMT")) %>'
                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("SLT_TAX_AMT")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="22%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                            TabIndex="58" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                            OnLoad="btnAction_Load" SkinID="btnclose" ToolTip="Remove" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="divConfirmationWithReason" style="display: none">
                <div class="content-wrapper">
                    <div class="divcol-P">
                        <h5>
                            <%= Resources.Messages.DeleteConfirmation %>
                        </h5>
                        <div class="clear">
                        </div>
                        <asp:Label ID="lblReason" Text="<%$ Resources:Reason %>" runat="server" AssociatedControlID="txtReason"></asp:Label><asp:TextBox
                            runat="server" ID="txtReason" TextMode="MultiLine" CssClass="multiline-3col"
                            TabIndex="60" onkeydown="limitText(this,450);" onchange="limitText(this,450);"></asp:TextBox><asp:Label
                                ID="lblDeleteOK" runat="server" AssociatedControlID="lblDeleteOK"></asp:Label><asp:Button
                                    ID="btnDeleteOK" runat="server" Text="<%$resources:ErpRes,Ok %>" ToolTip="<%$resources:ErpRes,Ok %>"
                                    TabIndex="61" SkinID="btnInner-ok" OnClick="ActionHandler" CommandName="INACTIVE"
                                    CommandArgument="SEC_ActionPanel" />
                        <asp:Button ID="btnDeleteCancel" runat="server" Text="<%$resources:Controls,Cancel %>"
                            TabIndex="62" ToolTip="<%$resources:Controls,Cancel %>" SkinID="btnInner-Cancel"
                            OnClientClick="return closeDeletePopup();" />
                    </div>
                </div>
            </div>
            <div id="divRevisionHistory" style="display: none" class="content-wrapper">
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdRevisionHistory" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:RevDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRevisionDate" runat="server" Text='<%# Eval("SOH_DATE", Resources.ErpRes.GridFormatDatetime) %>'
                                        ToolTip='<%# Eval("SOH_DATE", Resources.ErpRes.GridFormatDatetime) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="43%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:SaleOrderNo %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRevisionPrint" runat="server" ToolTip="View" CssClass="text-underline"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="27%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:CUR %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRCUR" runat="server" Text='<%# Eval("SOH_CURRENCY_TEXT") %>' ToolTip='<%# Eval("SOH_CURRENCY_TEXT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRTotalAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOH_NET_AMOUNT")) %>'
                                        ToolTip='<%# GetFormattedCurrency(Eval("SOH_NET_AMOUNT")) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divViewAdditionalPackDtls" style="display: none;">
                <div class="content-wrapper">
                    <table>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblStrap" runat="server" AssociatedControlID="chkstrapView" Text="<%$ resources:Strapping %>">
                                    </asp:Label><asp:CheckBox runat="server" ID="chkstrapView" />
                                    <asp:Label ID="lblDummyLayer" runat="server" AssociatedControlID="chkLayerView" CssClass="lbl-9perc">
                                    </asp:Label><asp:Label ID="lblLayer" runat="server" AssociatedControlID="chkLayerView"
                                        Text="<%$ resources:Layering %>" CssClass="lbl-49perc">
                                    </asp:Label><asp:CheckBox runat="server" ID="chkLayerView" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblNoofLrs" runat="server" AssociatedControlID="txtNoofLrsView" Text="<%$ resources:NoofLayers %>">
                                    </asp:Label><asp:TextBox runat="server" ID="txtNoofLrsView" CssClass="input-small"
                                        Enabled="false"></asp:TextBox><asp:Label ID="lblPcs" runat="server" AssociatedControlID="txtPcsView"
                                            Text="<%$ resources:PcsLayers %>">
                                        </asp:Label><asp:TextBox runat="server" ID="txtPcsView" CssClass="input-small" Enabled="false"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblStrapColor" runat="server" AssociatedControlID="ddlStrappingColorView"
                                        Text="<%$ resources:StrappingColorSku %>" CssClass="middle-lbl-small-j">
                                    </asp:Label><asp:DropDownList runat="server" ID="ddlStrappingColorView" CssClass="select-half"
                                        Enabled="false">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblMfgDt" runat="server" AssociatedControlID="txtMfgDtView" Text="<%$ resources:MfgDate %>"
                                        CssClass="middle-lbl-small-j">
                                    </asp:Label><asp:TextBox runat="server" ID="txtMfgDtView" CssClass="input-small"
                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" Enabled="false"></asp:TextBox><asp:Label
                                            ID="lblExpiry" runat="server" AssociatedControlID="txtExpiryView" Text="<%$ resources:ExpiryDate %>"
                                            CssClass="lbl-19-4perc">
                                        </asp:Label><asp:TextBox runat="server" ID="txtExpiryView" CssClass="input-small"
                                            Enabled="false"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divSaleCostDtls" style="display: none;">
                <div class="content-wrapper">
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdSalesCost" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBrandText" runat="server" Text='<%# Eval("SOD_CUST_ITEM_TEXT") %>'
                                            ToolTip='<%# Eval("SOD_CUST_ITEM_TEXT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:SalesCostQuantity %>" HeaderStyle-CssClass="txt-rgt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSodQty" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOD_BRAND_QTY")) %>'
                                            ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_BRAND_QTY")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="bg1" />
                                    <ItemStyle Width="7%" HorizontalAlign="Right" CssClass="bg2" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBrandUOM" runat="server" Text='<%# Eval("CIM_UOM_TEXT") %>' ToolTip='<%# Eval("CIM_UOM_TEXT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="bg1" />
                                    <ItemStyle Width="1%" HorizontalAlign="Left" CssClass="bg2" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmtCurrency" runat="server" Text='<%# Eval("CIM_CURRENCY_TEXT") %>'
                                            ToolTip='<%# Eval("CIM_CURRENCY_TEXT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="bg1" />
                                    <ItemStyle Width="1%" HorizontalAlign="Left" CssClass="bg2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:SaleRate %>" HeaderStyle-CssClass="txt-rgt bg1">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSaleRate" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOD_SALE_RATE")) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Eval("SOD_SALE_RATE")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Right" CssClass="bg2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="txt-rgt bg1">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOD_AMOUNT")) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Eval("SOD_AMOUNT")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" CssClass="bg2" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:ProdRate %>" HeaderStyle-CssClass="txt-rgt bg3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProdCoste" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOD_PROD_COST")) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Eval("SOD_PROD_COST")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" HorizontalAlign="Right" CssClass="bg4" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:PackRate %>" HeaderStyle-CssClass="txt-rgt bg3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPackCost" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOD_PACK_COST")) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Eval("SOD_PACK_COST")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" HorizontalAlign="Right" CssClass="bg4" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:TotalCost %>" HeaderStyle-CssClass="txt-rgt bg3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalCost" runat="server" Text='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("SOD_PROD_COST")) + Convert.ToDouble(Eval("SOD_PACK_COST"))) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("SOD_PROD_COST")) + Convert.ToDouble(Eval("SOD_PACK_COST"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" CssClass="bg4" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:UnitRate %>" HeaderStyle-CssClass="txt-rgt bg3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("SOD_PROD_RATE")) + Convert.ToDouble(Eval("SOD_PACK_RATE"))) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDouble(Eval("SOD_PROD_RATE")) + Convert.ToDouble(Eval("SOD_PACK_RATE"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" CssClass="bg4" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:RateDiff %>" HeaderStyle-CssClass="txt-rgt bg3">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRateDiff" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("SOD_RATE_DIFF")) %>'
                                            ToolTip='<%# GetFormattedCurrencyWithComma(Eval("SOD_RATE_DIFF")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Right" CssClass="bg4" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div id="divQuantityCalculation" style="display: none;">
                    <div class="content-wrapper">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnApplyQty" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="QUANTITYAPPLY"
                                TabIndex="75" OnClientClick="javascript:ValidatePageNow('OrderQty')" ValidationGroup="OrderQty" />
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblPopUpTotalPcsCtn" runat="server" AssociatedControlID="txtPopUpTotalPcsCtn"
                                            Text="<%$ resources:TotalPiecesCtn %>">
                                        </asp:Label><asp:TextBox runat="server" ID="txtPopUpTotalPcsCtn" CssClass="input-small numeric input-disabled"
                                            Enabled="false"></asp:TextBox><asp:Label ID="lblPopUpPcsperUnit" runat="server" AssociatedControlID="txtPopUpPcsperUnit"
                                                Text="<%$ resources:PcsperUnit %>" CssClass="middle-lbl">
                                            </asp:Label><asp:TextBox runat="server" ID="txtPopUpPcsperUnit" CssClass="input-small numeric input-disabled"
                                                Enabled="false"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblPopUpOrderQty" runat="server" AssociatedControlID="txtPopUpOrderQty"
                                            Text="<%$ resources:OrderQty %>">
                                        </asp:Label><asp:TextBox runat="server" ID="txtPopUpOrderQty" CssClass="input-small numeric"
                                            MaxLength="11" onchange="CalcQuantity();"></asp:TextBox><div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvOrdrQty" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="OrderQty" EnableClientScript="true" runat="server" ControlToValidate="txtPopUpOrderQty"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PopUpOrderQty %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        <asp:Label ID="lblPopUpQty" runat="server" AssociatedControlID="txtPopUpQty" Text="<%$ resources:Quantity %>"
                                            CssClass="middle-lbl">
                                        </asp:Label><asp:TextBox runat="server" ID="txtPopUpQty" CssClass="input-small numeric input-disabled"
                                            Enabled="false" onkeydown="return CheckKey(event)" MaxLength="11"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div style="display: none;">
                    <asp:Button ID="btnIOReviewSave" runat="server" CommandName="SAVE" OnClick="ActionHandler" />
                </div>
                <asp:HiddenField runat="server" ID="hdfEnableSubType" Value="0" />
                <asp:HiddenField runat="server" ID="hdfCreditCheckContinue" Value="0" />

                <asp:HiddenField runat="server" ID="hdfAgtSaves" Value="-1" />
                <asp:HiddenField runat="server" ID="hdfErrorMsgType" Value="0" />
                <asp:HiddenField runat="server" ID="hdfIsPostback" Value="0" />
                <asp:HiddenField runat="server" ID="hdfFocusPdctAdd" Value="0" />
                <asp:HiddenField runat="server" ID="hdfOldCBMLimitPK" Value="0" />
                <asp:HiddenField runat="server" ID="hdfNewCBMLimitPK" Value="0" />
                <asp:HiddenField runat="server" ID="hdfIsWrkflwClose" Value="0" />
                <asp:HiddenField runat="server" ID="hdfIsLotNoIOReview" Value="0" />
                <asp:HiddenField runat="server" ID="hdfIsMsgIOReview" Value="0" />
                <asp:HiddenField runat="server" ID="hdfIsCancelled" Value="0" />
                <asp:HiddenField runat="server" ID="hdfVersion" Value="0" />
                <asp:HiddenField runat="server" ID="hdfMailAttachmentName" Value="" />
                <asp:HiddenField ID="hdfCBPriceDecimals" runat="server" Value="2" />
                <asp:HiddenField ID="hdfDecimalVal" runat="server" Value="0" />
                <asp:HiddenField ID="hdfPcsperCtn" runat="server" Value="0" />
                <asp:HiddenField ID="hdfSaleUOMPcs" runat="server" Value="0" />
                <asp:HiddenField ID="hdfSubTotal" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCusPoNumber" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsProductRequired" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsShowProductRequired" Value="0" runat="server" />
                <asp:HiddenField ID="hdfInitMfgDate" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsCrmCustomer" Value="0" runat="server" />
                <asp:HiddenField ID="hdfContractDateChange" Value="0" runat="server" />
                <div style="display: none">
                    <asp:Button runat="server" ID="btnDummySaveSubmit" CommandName="WRKFSUBMIT" TabIndex="64"
                        Text="" OnClick="ActionHandler" SkinID="btnInner-submit" />
                </div>
                <div style="display:none">
                    <asp:Button runat="server" ID="btnQuotation" CommandName="QUOTATIONCHANGE" OnClick="ActionHandler" />
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
