<%@ Page Title="<%$ Resources:Captions,Title_ShippingPlan %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="ShippingPlan.aspx.cs" Inherits="ERPSMS_v01.Shipping.ShippingPlan"
    ValidateRequest="false" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        a.downloadClass {
            cursor: pointer;
            text-decoration: underline;
        }

        a.removedownloadClass {
            cursor: auto;
            text-decoration: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", false, false);
            GrandScriptUtils.DatePickerCommon("txtLoadingDate");

            GrandScriptUtils.AddDateRangeCommon("txtGeneratedOn", "hdfGeneratedOn", "txtETD", "hdfETD", false, false);
            GrandScriptUtils.DatePickerCommon("txtClosingDate");
            GrandScriptUtils.DatePickerCommon("txtETA");
            $('[id$=txtClosingTime]').timepicker();

            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "&IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, "CUSTOMERLIST");

            $("[id*=txtDespNow]").ForceNumericOnly();
            $("[id$=btnSetTotal]").hide();
            ShowHideExpand();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfDelstatus]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End
        }

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function AfterGridExpand(row) {


            if ($("[id$=grdShippingPlanList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
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
                $("[id$=ModifiedDatePnl]").hide();
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
                $("[id$=btnAddnewSCItem]").hide();
                //                $("[id$=btnAddItem]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=btnAddnewSCItem]").hide();
            }
            if (mode == 3) {
                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }
        }

        function setTotal(sender) {
            $("[id$=btnSetTotal]").click();
        }

        function ShowError() {
            var msg = '<%=Resources.Messages.ReportError %>';
            var information = '<%=Resources.Messages.Information %>';
            GrandScriptUtils.ShowModal(msg, information);
        }

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
                        Page_Validators.splice(i, 1);
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
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
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }

        //for working the dropdown postback properly after firing the Add button validation.
        //After the validation Dummy button click firing for postbacking the dropdwn 
        function ClearBrandValidation() {
            $("[id$=btnBrandQty]").click();
        }

        function CalculateTotal() {
            var TotalPlanNow = 0;
            var PlanNow = 0;
            var DecimalDigits = 0;
            var hdnTotalPcs = 1;
            var hdnPackingPcs = 0;
            var CTN_Qty = 0;
            var TotalCTN_Qty = 0;
            var CBM = 0;
            var TotalCBM = 0;
            var cbmDecimals = 4;

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            $("#[id*=grdShippingList] input[type=text][id*=txtPlanNow]").each(function (index) {

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdnTotalPcs]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdnTotalPcs]").val().replace(/[^0-9\.]+/g, ""));
                    hdnTotalPcs = parseFloat(number);
                    hdnTotalPcs = hdnTotalPcs <= 0 ? 1 : hdnTotalPcs;
                }

                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdnPackingPcs]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdnPackingPcs]").val().replace(/[^0-9\.]+/g, ""));
                    hdnPackingPcs = parseFloat(number);
                }


                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        PlanNow = parseFloat($(this).val());
                        TotalPlanNow = TotalPlanNow + PlanNow;
                        //}
                    }
                }
                CTN_Qty = Math.ceil(PlanNow / hdnTotalPcs);
                TotalCTN_Qty = TotalCTN_Qty + CTN_Qty;
                $(this).closest('tr').find("#[id*=lblCTNQty]").text(addCommas(CTN_Qty.toFixed(0)));

                CBM = CTN_Qty * hdnPackingPcs;
                TotalCBM = TotalCBM + CBM;
                $(this).closest('tr').find("#[id*=lblCBM]").text(CBM.toFixed(DecimalDigits));

                var orderqty = $(this).closest('tr').find("#[id*=lblOrderQty]").text();
                if (orderqty == "") {
                    var qty = 0;
                    $(this).closest('tr').find("#[id*=lblOrderQty]").text(qty.toFixed(DecimalDigits))
                }

            });

            $("#[id*=grdShippingList] [id*=lblTotalPlanNow]").html(TotalPlanNow.toFixed(DecimalDigits));
            $("#[id*=grdShippingList] [id*=lblTotalCTN]").html(addCommas(TotalCTN_Qty.toFixed(0)));
            $("#[id*=grdShippingList] [id*=lblTotalCBM]").html(TotalCBM.toFixed(DecimalDigits));

        }


        function ShowHideUploadDocDetails(flag) {
            //If flag then Show file upload details
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

        function ShowHideAdditionalInfo(flag) {
            //If flag then Show additional Info
            if (flag) {
                $("[id$=tblAdditionalInfo]").show();
                $("[id$=imgAdditionalInfoShow]").hide();
                $("[id$=imgAdditionalInfoHide]").show();
            }
            else {
                $("[id$=tblAdditionalInfo]").hide();
                $("[id$=imgAdditionalInfoShow]").show();
                $("[id$=imgAdditionalInfoHide]").hide();
            }
            return false;
        }

        function CalculatePlanNowQty(ctrl) {
            var DecimalDigits = 0;
            var TotalPlanNow = 0;
            var hdfProductConvFactor = 1;
            var PlanNowQty = 0;
            var TotalPlanNowQty = 0;

            $("[id$=hdfIsPlanQtyChanged]").val(1);

            if (!isNaN(parseFloat($("#[id*=hdfQtyDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfQtyDecimalDigits]").val());
            }
            $("#[id*=grdShippingList] input[type=text][id*=txtProductPlanNow]").each(function (index) {
                var PlanNow = 0;
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        PlanNow = parseFloat($(this).val());
                        TotalPlanNow = TotalPlanNow + PlanNow;
                    }
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfProductConvFactor]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfProductConvFactor]").val().replace(/[^0-9\.]+/g, ""));
                    hdfProductConvFactor = parseFloat(number);
                }
                PlanNowQty = PlanNow * hdfProductConvFactor;
                TotalPlanNowQty = TotalPlanNowQty + PlanNowQty;
                $(this).closest('tr').find("#[id*=txtPlanNow]").val(PlanNowQty.toFixed(DecimalDigits));
            });
            //            $("#[id*=grdShippingList] [id*=lblProductTotalPlanNow]").html(TotalPlanNow.toFixed(DecimalDigits));
            $("#[id*=grdShippingList] [id*=lblTotalPlanNow]").html(TotalPlanNowQty.toFixed(DecimalDigits));
            ctrl.Focus();
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


        //For carten check
        function ShowCartCheckConfirming(type) {
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
                        if (type == 1) {
                            $("[id$=btnSave]").click();
                        }
                        else if (type == 2) {
                            $("[id$=btnSaveSubmit]").click();
                        }
                        else if (type == 3) {
                            $("[id$=btnSubmit]").click();
                        }
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

        function ValidationCheckRate(sender, args) {
            if ($("[id$=hdfShipmentTermValue]").val() == "4") { // 4 - DAP - Date of ETA
                if ($("[id$=txtETA]").val()) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                }
            }
            else
                args.IsValid = true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" runat="server" class="medium margnbotm0" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="51"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="52"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="inv" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="53" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="54" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="51" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <%--<li runat="server" id="pnlPrint">
                                       <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="53" Text="Print"
                                            OnClick="ActionHandler" ToolTip="Print" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="55" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="56" ID="btnListPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:PrintBtnText %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:PrintBtnToolTip %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="pnlListPrint" runat="server" visible="false">
                                        <asp:Button runat="server" TabIndex="60" ID="btnPrintLst" CommandName="PRINTCILISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,PrintCI %>" CssClass="btnInner-Print-green-btn" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:Controls,PrintCI %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li id="pnlPrintPM" runat="server">
                                        <asp:Button runat="server" TabIndex="60" ID="btnPrintPM" CommandName="PRINTPM"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,PrintPM %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,PrintPM %>" OnClientClick="javascript:return SelectedCheckBoxCount(1);" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelEI %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelEI %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="57" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="58" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="59" ID="btnPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:PrintBtnText %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-receipt" ToolTip="<%$resources:PrintBtnToolTip %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnSOListing" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                CommandArgument="SEC_ActionPanel" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnShippingPlan" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkShippingPlan" Text="<%$resources:PageNameRes,ShippingPlan %>"
                                CommandArgument="SEC_ActionPanel" CommandName="SHIPPINGPLAN" OnClick="ActionHandler"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <%--  <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="6" CommandName="CONTAINEREVALUATION" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="7" CommandName="CONTAINERINSPECTION" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkEnquiry" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="8" CommandName="UPLOADQA" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkQuotation" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="9" CommandName="UPLOADEXPORT" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="10" CommandName="LOADINGPLAN" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="11" CommandName="UPLOADPHOTOGRAPHS" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Span1" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="12" CommandName="GOODOUTWARD" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="13" CommandName="CONTAINERRELEASE" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                         <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="14" CommandName="PRINT" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="14" OnClick="ActionHandler" CommandName="SHIPPINGPLANLIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="15" OnClick="ActionHandler" CommandName="SHIPPINGPLANDETAIL"
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
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7" id="divCompany" runat="server">
                                            <asp:Label ID="lblCompanyFilter" CssClass="lblCompanyFilter-label-19-11" runat="server" Text="<%$resources:Controls,CompanyPlant%>"
                                                AssociatedControlID="ddlCompanyFilter"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyFilter" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlVoucherCompany');"
                                                CssClass="select-small-a1">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" CssClass="lblFrmDate-label-19-11" runat="server" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="3" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" CssClass="middle-lbl-small-d-19-11" Text="<%$ resources:ToDate %>"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="4" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblStat" runat="server" Text="<%$ resources:Status %>" CssClass="middle-lbl-small-19-11"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="5" CssClass="select-half">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomerSearch" runat="server" Text="<%$ resources:Customer %>" CssClass="margnbotm0 middle-lbl-20-11-7"
                                                AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-w47-7per margnbotm0" MaxLength="100"
                                                TabIndex="6"> </asp:TextBox>

                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <%--<div class="clear">
                                            </div>--%>

                                            <asp:Label ID="lblCustomerPo" runat="server" Text="<%$resources:CustPo %>" CssClass="lbl-14-2perc"
                                                AssociatedControlID="txtPONumber"></asp:Label>
                                            <asp:TextBox ID="txtPONumber" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="7"> </asp:TextBox>

                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">

                                            <asp:Label ID="lblSCno" runat="server" Text="<%$resources:SONo %>" CssClass="middle-lbl-xsmall-13 margnbotm0"
                                                AssociatedControlID="txtSCno"></asp:Label>
                                            <asp:TextBox ID="txtSCno" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="7"> </asp:TextBox>
                                            <asp:Label ID="lblShipPlanNo" runat="server" Text="<%$resources:PlanNo %>" CssClass="middle-lbl-xsmall-e-19-11 margnbotm0"
                                                AssociatedControlID="txtPlanNo"></asp:Label>
                                            <asp:TextBox ID="txtPlanNo" runat="server" CssClass="input-small margnbotm0" MaxLength="100"
                                                TabIndex="7"> </asp:TextBox>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" ValidationGroup="Search" OnClick="ActionHandler"
                                                TabIndex="7" CommandName="SEARCH" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                TabIndex="7" ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler"
                                                CommandName="CLEAR" Style="margin-bottom: 0px!important; margin-top: 2px;" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap" style="max-height: 400px; padding: 7px;">
                                <cc1:ExtGridView runat="server" ID="grdShippingPlanList" AutoGenerateColumns="False"
                                    TabIndex="22" Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="7" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("SNH_DEPT") %>' />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SPPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfShippingPlanID" Value='<%# Eval(Resources.DataFieldRes.SPPk) %>' />
                                                <asp:HiddenField runat="server" ID="hdfDeleteStatus" Value='<%# Eval(Resources.DataFieldRes.SPDeleteStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfShippingType" Value='<%# Eval("SOH_TYPE_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ETD %>" SortExpression="SNH_ETD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPETD, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPETD, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlantCode" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>" Text="<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>"
                                                    ToolTip="<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="SNH_CUSTOMER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPCustomerText),33)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPCustomerText),300)%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SPCustomer) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustPo %>" SortExpression="SOH_REFERENCE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustPo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.CustPo),15)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.CustPo),300)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Shiptoport %>" SortExpression="SNH_SHIP_TO_PORT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPShipToPort),15)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPShipToPort),300)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="SNH_UOM_TEXT"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUomText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SNH_UOM_TEXT"),15)%>'
                                                    ToolTip='<%# Eval("SNH_UOM_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>" SortExpression="SNH_PLAN_QTY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlandQuantity" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPPlandQty, "{0:n}")%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPPlandQty, "{0:n}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Cartons %>" SortExpression="SNH_CTN_QTY">
                                            <ItemTemplate>
                                                <%-- <asp:Label ID="lblCarrier" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPCartonsQty)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPCartonsQty)%>'></asp:Label>--%>
                                                <asp:Label ID="lblCarrier" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPCartonsQty,"{0:n0}")%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPCartonsQty, "{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanNo %>" SortExpression="SNH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContainer" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPNO)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPNO)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LoadingDate %>" SortExpression="SNH_LOADING_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransportCo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPLoadingDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPLoadingDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                          <%--SC No--%>
                                      <%--  <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="SOH_NO" Visible="<%$ resources:ConfigurationsRes,ShowSCNoInShppingPlan %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSCNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOH_NO"),12)%>'
                                                    ToolTip='<%# Eval("SOH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <%--BOIStatus--%>
                                       <%-- <asp:TemplateField HeaderText="<%$ resources:BOIStatus %>" Visible="<%$ resources:ConfigurationsRes,ShowBOIStatusInShppingPlan %>">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlStatus" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField SortExpression="SNH_STATUS">
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval(Resources.DataFieldRes.SPStatusText) %>' />
                                                <%--<asp:Label ID="lblStatusText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPStatusText),10)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPStatusText)%>'></asp:Label>--%>
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SPStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxStatus" Value='<%# Eval(Resources.DataFieldRes.SPTrxStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdOrderDetails" AutoGenerateColumns="False"
                                                        Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        AllowPaging="false" OnRowDataBound="ActionHandler">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONumber) %>'
                                                                        OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SCPK) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SONumber) %>'></asp:LinkButton>
                                                                    <%--<asp:Label ID="lblDetailsSoNO" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONumber) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SONumber) %>'></asp:Label>--%>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDetailsSoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SaleOrderDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SaleOrderDate, Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ShipDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDetailsShipDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeatilsProductCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemCode),15) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.ItemName) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeatilsBrandName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.BrandName),75) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.BrandName) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="47%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblplndQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PlanQty, "{0:n}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PlanQty, "{0:n}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Cartons %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeatilscartons" runat="server" Text='<%# Eval(Resources.DataFieldRes.CartonsQty, "{0:n}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.CartonsQty, "{0:n}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblDetailHdr">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPlanNo" Text="<%$ resources:PlanNo %>" AssociatedControlID="lblShippingPlanNo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblShippingPlanNo" Text="" CssClass="input-small"></asp:Label>
                                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                    <asp:HiddenField ID="AST_CODE" runat="server" />
                                                    <asp:HiddenField ID="hdfShippingPlanNo" runat="server" />
                                                    <asp:Label runat="server" ID="Label2" Text="<%$ resources:GeneratedOn %>" class="middle-lbl-c-20-11-6"
                                                        AssociatedControlID="txtGeneratedOn"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtGeneratedOn" Text="" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="35"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfGeneratedOn" runat="server" Value="" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtGeneratedOn" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="delivery"
                                                            runat="server" ControlToValidate="txtGeneratedOn" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblDelivery" Text="<%$ resources:Customer %>" AssociatedControlID="lblDeliveryTo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDeliveryTo" Text="" CssClass="select-half"></asp:Label>
                                                    <asp:HiddenField ID="hfCustomerName" runat="server" />
                                                    <asp:HiddenField ID="hdnCustomerID" runat="server" Value="0" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblShiptoPort" Text="<%$ resources:Shiptoport %>" AssociatedControlID="txtShipPort"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShipPort" Text="" TabIndex="36" CssClass="select-half"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblShippingAgent" Text="<%$ resources:ShippingAgent %>"
                                                        AssociatedControlID="ddlAgent"></asp:Label>
                                                    <asp:DropDownList ID="ddlAgent" runat="server" TabIndex="40" CssClass="select-half-a">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType %>"
                                                        AssociatedControlID="ddlContainerType"></asp:Label>
                                                    <asp:DropDownList ID="ddlContainerType" runat="server" TabIndex="38" CssClass="select-small-b-20-11-6">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfContainerType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="ddlContainerType"
                                                        Display="Dynamic" InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_ContainerType %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label3" Text="<%$ resources:ETD %>" AssociatedControlID="txtETD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETD" Text="" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="37"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETD" runat="server" Value="" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfETD" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtETD" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_ETDDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreETDDate" CssClass="star" ValidationGroup="delivery"
                                                            runat="server" ControlToValidate="txtETD" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_ETDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <asp:Label ID="lblETA" runat="server" Text="<%$ resources:ETA %>" AssociatedControlID="txtETA"
                                                        CssClass="middle-lbl-c-20-11-6-sp"></asp:Label>
                                                    <asp:TextBox ID="txtETA" runat="server" CssClass="input-small" MaxLength="11" onkeydown="return CheckKey(event)"
                                                        onpaste="return false;" TabIndex="39"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETA" runat="server" Value="" />
                                                    <asp:CustomValidator ID="cvETA" runat="server" ErrorMessage="<%$ resources:Err_ETADate%>"
                                                        ValidateEmptyText="true" ClientValidationFunction="ValidationCheckRate"
                                                        Text="*" EnableClientScript="true" ControlToValidate="txtETA" CssClass="star"
                                                        Display="Dynamic" ValidationGroup="delivery"></asp:CustomValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:Status %>" AssociatedControlID="lblSPStatus"></asp:Label>
                                                    <asp:Label runat="server" ID="lblSPStatus" Text="" CssClass="select-half"></asp:Label>
                                                    <asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department %>"
                                                        AssociatedControlID="lblPageDeptText" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblPageDeptText" runat="server" Visible="false"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblLoadingDate" Text="<%$ resources:LoadingDate %>"
                                                        AssociatedControlID="txtLoadingDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtLoadingDate" Text="" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="41"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfLoadingDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtLoadingDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_LoadingDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreLoadingDate" CssClass="star" ValidationGroup="delivery"
                                                        runat="server" ControlToValidate="txtLoadingDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_LoadingDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    <asp:Label ID="lblContNo" runat="server" Text="<%$ resources:ContainerNumber %>" AssociatedControlID="txtContNo"
                                                        CssClass="middle-lbl-c-20-11-6-sp"></asp:Label>
                                                    <asp:TextBox ID="txtContNo" runat="server" CssClass="input-small" MaxLength="11"
                                                        TabIndex="41"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:Button ID="btnAddnewSCItem" SkinID="btnInner-add" runat="server" Text="<%$ resources:AddNewSCItem %>"
                                ToolTip="<%$ resources:AddNewSCItem %>" OnClick="ActionHandler" CommandName="ADDSCITEM"
                                CommandArgument="PageAction_Entry" Style="float: right; margin-right: 0px;" />
                            <div class="clear">
                            </div>
                            <div class="gridwrap scroll-container scroll-container-19-11-2">
                                <asp:GridView ID="grdShippingList" runat="server" AutoGenerateColumns="False" Width="1415px"
                                    PageSize="25" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="42">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='' OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" ToolTip=''></asp:LinkButton>
                                                <%--<asp:Label ID="lblSONo" runat="server" Text='' ToolTip=''></asp:Label>--%>
                                                <asp:HiddenField ID="hdfSONumber" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderHdrPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderDtlPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSOH_PK" Value='<%# Eval("SOD_SO") %>' runat="server" />
                                                <asp:HiddenField ID="hdfIsPackMat" Value='<%# Eval("SOD_IS_PACK_MAT") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <%--                                        <asp:TemplateField HeaderText="<%$ resources:ProductCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIGPLCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfIGPLCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfBrandCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="19%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfUOM" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfDespatchQty" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:TotalCTNs %>"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanNow %>" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtProductPlanNow" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                                    runat="server" CssClass="small-a numeric" MaxLength="10" TabIndex="42" onkeyup="CalculatePlanNowQty(this);"
                                                    AutoPostBack="true" OnTextChanged="ActionHandler"></asp:TextBox>
                                                <cc2:QuantityValidation ID="vrePlanNow" runat="server" ControlToValidate="txtProductPlanNow"
                                                    NumberDigits="11" ErrorMessage="<%$ resources:Err_Plan_Valid %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="delivery"
                                                    NonZero="false">
                                                </cc2:QuantityValidation>
                                                <asp:HiddenField ID="hdfProductConvFactor" runat="server" Value="1" />
                                                <asp:HiddenField ID="hdfProductUOMPK" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblProductTotalPlanNow" runat="server"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanNow %>" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPlanNow" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                                    runat="server" CssClass="input-disabled small-a numeric" MaxLength="10" Enabled="false"></asp:TextBox><%--onChange="setTotal();"--%>
                                                <%--<asp:RequiredFieldValidator ID="vrfPlanNow" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtPlanNow"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Plan %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <%--<cc2:QuantityValidation ID="vrePlanNow" runat="server" ControlToValidate="txtPlanNow"
                                                    NumberDigits="11" ErrorMessage="<%$ resources:Err_Plan_Valid %>" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="delivery"
                                                    NonZero="false">
                                                </cc2:QuantityValidation>--%>
                                                <%--<asp:CompareValidator ID="cmpPlanNow" runat="server" ControlToValidate="txtPlanNow" ValidationGroup="delivery"
                                                    ErrorMessage="<%$ resources:Err_Plan_Quantity %>"  Type="Integer" ValueToCompare="0" Operator="GreaterThan" Text="*">
                                                </asp:CompareValidator>--%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalPlanNow" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--   Gross weight & Net Weight--%>
                                        <asp:TemplateField HeaderText="<%$ resources:NetWeight %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetWeight" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalNetWeight" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossWeight %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrossWeight" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalGrossWeight" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--    End  Gross weight--%>
                                        <asp:TemplateField HeaderText="<%$ resources:CBM %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCBM" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdnPackingPcs" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCBM" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CTNQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCTNQty" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdnTotalPcs" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCTN" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProductCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIGPLCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfIGPLCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    CommandName="REMOVE" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="btnSetTotal" runat="server" CommandName="SHOW" OnClick="ActionHandler" />
                            </div>
                            <%-- Additional Information --%>
                            <div class="clear">
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetGlobalResourceObject("Controls", "Additional_Info").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imgAdditionalInfoShow" OnClientClick="javascript:return ShowHideAdditionalInfo(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imgAdditionalInfoHide" OnClientClick="javascript:return ShowHideAdditionalInfo();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblAdditionalInfo">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblBookingRefNo" runat="server" Text="<%$ resources:BookingRefNo %>"
                                                        AssociatedControlID="txtBookingRefNo"></asp:Label>
                                                    <asp:TextBox ID="txtBookingRefNo" runat="server" MaxLength="100" CssClass="input-half"
                                                        TabIndex="42"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblClosingDate" runat="server" Text="<%$ resources:ClosingDate %>"
                                                        AssociatedControlID="txtClosingDate"></asp:Label>
                                                    <asp:TextBox ID="txtClosingDate" runat="server" CssClass="input-small" MaxLength="11"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="44"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblClosingTime" Text="<%$ resources:ClosingTime %>"
                                                        CssClass="middle-lbl-small-d-20-11-5" AssociatedControlID="txtClosingTime"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtClosingTime" MaxLength="12" CssClass="input-small"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="44"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblFeederVessel" runat="server" Text="<%$ resources:FeederVessel %>"
                                                        AssociatedControlID="txtFeederVessel"></asp:Label>
                                                    <asp:TextBox ID="txtFeederVessel" runat="server" CssClass="input-half" MaxLength="200"
                                                        TabIndex="43"></asp:TextBox>
                                                    <asp:Label ID="lblMotherVessel" runat="server" Text="<%$ resources:MotherVessel %>"
                                                        AssociatedControlID="txtMotherVessel"></asp:Label>
                                                    <asp:TextBox ID="txtMotherVessel" runat="server" CssClass="input-half" MaxLength="200"
                                                        TabIndex="45"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                                    <asp:TextBox ID="txtRemarks" runat="server" TabIndex="45" MaxLength="450" TextMode="MultiLine"
                                                        Height="40" CssClass="multiline lbl-hgt-20"></asp:TextBox>
                                                </div>
                                            </td>
                                            <%-- <td>
                                                <div class="div2col-S"></div>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- Upload Documents --%>
                            <div class="clear">
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetGlobalResourceObject("Controls", "UploadDoc_Info").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideUploadDocDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideUploadDocDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblUploadDocDetails">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblTitle" Text="<%$ resources:Title %>" AssociatedControlID="txtTitle"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtTitle" MaxLength="200" onkeydown="limitText(this,200);"
                                                        CssClass="input-half" onkeyup="limitText(this,200);" TabIndex="47"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"></asp:Label>
                                                    <div class="fileupload-main">
                                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="48" CssClass="margn-rgt0 upload-area2" />
                                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star input-medium" SetFocusOnError="true"
                                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="btnwrap-divcol" style="padding-right: 0%!important;">
                                                        <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="49" OnClick="ActionHandler"
                                                            OnClientClick="javascript:ValidatePageNow('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                                            CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$resources:ErpRes,Add %>"
                                                            SkinID="btnInner-add" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <a id="anchorFile" runat="server" target="_blank" tabindex="50"></a>
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td colspan="2">
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description %>"
                                                        AssociatedControlID="txtDescription"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDescription" MaxLength="500" TextMode="MultiLine"
                                                      CssClass="multiline-2col"  onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td colspan="2">
                                                <div class="gridwrap">
                                                    <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                                        AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                        OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="8" EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                    <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("SCD_PK") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="4%" HorizontalAlign="Center" Wrap="false" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Title %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTttle" runat="server" Text='<%# Eval("SCD_TITLE") %>' ToolTip='<%# Eval("SCD_TITLE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="31%" />
                                                                <HeaderStyle CssClass="padglft0" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("SCD_FILE") %>' ToolTip='<%# Eval("SCD_FILE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="56%" />
                                                                <HeaderStyle CssClass="padglft0" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                                        target="_blank" href='<%# Page.ResolveClientUrl(Eval("SCD_FILE_PATH").ToString()) %>'></a>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                                        SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                                                        OnLoad="btnAction_Load" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                        SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                                        OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--Print popup window --%>
                            <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                            <div id="divSCItemDetails" style="display: none">
                                <div class="padgrgt8 Button-container-popup">
                                    <asp:Button ID="btnApplyItems" SkinID="btnInner-add-dsd" runat="server" Text="<%$ resources:Apply %>"
                                        ToolTip="<%$ resources:Apply %>" OnClick="ActionHandler" CommandName="SCITEMSAPPLY"
                                        CommandArgument="PageAction_Entry" OnClientClick="return ValidatePageNow('ValscItemApply')" />
                                </div>
                                <div class="content-wrapper">
                                    <div class="detail-co2" style="padding: 3px 1px;">
                                        <div class="div2col-S">
                                            <asp:Label ID="lblScCustomerH" runat="server" Text="<%$ resources:Customer1 %>" AssociatedControlID="lblScCustomer"></asp:Label>
                                            <asp:Label ID="lblScCustomer" runat="server"></asp:Label>
                                        </div>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblScShiptoportH" Text="<%$ resources:Shiptoport1 %>"
                                                AssociatedControlID="lblScShiptoport"></asp:Label>
                                            <asp:Label ID="lblScShiptoport" runat="server"></asp:Label>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblSalesContract" runat="server" Text="<%$ resources:SONo %>" AssociatedControlID="ddlSalesContract"></asp:Label>
                                                    <asp:DropDownList ID="ddlSalesContract" runat="server" CssClass="select-small-a0"
                                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true"
                                                        onchange="javascript:ClearBrandValidation();">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvSalesContract" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="ValscItem" EnableClientScript="true" runat="server" ControlToValidate="ddlSalesContract"
                                                        Display="Dynamic" InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_SalesContract %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="divcol-S">
                                                    <asp:Label ID="lblBrandName" runat="server" Text="<%$ resources:BrandName %>" AssociatedControlID="ddlBrandName"></asp:Label>
                                                    <asp:DropDownList ID="ddlBrandName" runat="server" Width="80%" EnableViewState="true"
                                                        OnSelectedIndexChanged="ActionHandler" onchange="javascript:ClearBrandValidation();"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvBrandName" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="ValscItem" EnableClientScript="true" runat="server" ControlToValidate="ddlBrandName"
                                                        Display="Dynamic" InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                                <div style="display: none;">
                                                    <asp:Button ID="btnBrandQty" runat="server" OnClick="ActionHandler" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="divcol-S">
                                                    <asp:Label ID="Label4" runat="server" Text="<%$ resources:OrderQty %>" AssociatedControlID="lblCusOrderQty"></asp:Label>
                                                    <asp:Label runat="server" ID="lblCusOrderQty" Text="" CssClass="input-small"></asp:Label>
                                                    <%-- <div class="clear">
                                    </div>--%>
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ resources:DespQty %>" AssociatedControlID="lblCusDespatchQty"></asp:Label>
                                                    <asp:Label runat="server" ID="lblCusDespatchQty" Text="" CssClass="input-small"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S input-margin2 nomargin">
                                                    <div class="floatRight">
                                                        <asp:Button runat="server" ID="btnAddToList" CommandName="ADDBRANDTOLIST" OnClick="ActionHandler"
                                                            Style="margin-right: 3px;" ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry"
                                                            ValidationGroup="ValscItem" Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add"
                                                            OnClientClick="return ValidatePageNow('ValscItem')" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView ID="grdCustomerShipping" runat="server" AutoGenerateColumns="False"
                                            Width="100%" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            ShowFooter="true">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustSONo" runat="server" Text='<%#Eval("SOH_NO")%>' ToolTip='<%#Eval("SOH_NO")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdfSCPK" Value='<%#Eval("SOH_PK") != null ?Eval("SOH_PK") :""%>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <%--   <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustSODate" runat="server" Text='' ToolTip=''></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustProductCode" runat="server" Text='' ToolTip=''></asp:Label>
                                        <asp:HiddenField ID="hdfCustProductCode" Value='' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustBrandName" runat="server" Text='<%#Eval("SOD_BRAND_NAME")%>'
                                                            ToolTip='<%#Eval("SOD_BRAND_NAME")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdfSodPK" Value='<%#Eval("SOD_PK")%>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="47%" />
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                        <asp:HiddenField ID="hdfCustUOM" Value='' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustPackedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:OrderQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustOrderQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("SOD_QTY"))%>'
                                                            ToolTip=''></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DespQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustDespatchedQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("SOD_QTY_DISPATCHED"))%>'
                                                            ToolTip=''></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkSCRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVESCITEM"
                                                            SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);" /><%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div style="display: none">
                    <asp:Button ID="btnResubmit" runat="server" CommandName="SAVESUBMIT" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="delivery" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="vsScItem" ValidationGroup="ValscItem" runat="server" />
                    <asp:ValidationSummary ID="VsItemsApply" ValidationGroup="ValscItemApply" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="delivery">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfIscartYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfType" runat="server" Value="" />
            <asp:HiddenField ID="hdfCurPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDOType" runat="server" Value="" />
            <asp:HiddenField ID="hdfDOPK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfQtyDecimalDigits" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
            <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
            <asp:HiddenField ID="hdfIsPlanQtyChanged" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRequiredSCValidation" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShipmentTermValue" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShipmentTermPK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShipmentTerm" runat="server" Value="" />
            <asp:HiddenField runat="server" ID="hdfInvTerm" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddItem" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
