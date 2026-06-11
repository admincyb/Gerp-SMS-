<%@ Page Title="<%$ Resources:Captions,Title_POS %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="PurchaseOrderNonStock.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseOrderManagement.PurchaseOrderNonStock" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtResponseDate");
            GrandScriptUtils.DatePickerCommon("txtRFQDate");
            GrandScriptUtils.DatePickerCommon("txtReqByDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "SERVICEVENDOR");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrencyTxt", url + "?Type=" + $("[id$=hdfVendor]").val() + "&ExcDate=" + $("[id$=txtResponseDate]").val(), "hdfCurrencyTxt", true, true, "VENDORCURRENCY");
            if (url.indexOf("?") != -1)
                GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "&Type=&ProcessPK=4", "hdfBrand", true, true, "ITEMTYPE");
            else
                GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "?Type=&ProcessPK=4", "hdfBrand", true, true, "ITEMTYPE");

            BindPorts();
            //SetUOMAutoCompleteGrid();
            //            $("[id$=txtHdrDiscount],[id$=txtRate],[id$=txtDiscount]").focusout(function () {
            //                $(this).change();
            //                return false;
            //            });
            $("[id$=btnSetTax]").hide();
            $("[id$=txtResponseDate]").hide();
            if ($('[id$=btnSubmit]').is(":visible"))
                $('[id$=pnlSaveSubmit]').hide();


            if ($("[id$=hdfPOStatus]").val() == 2) {
                $("[id$=btnAmend]").show();
                $("[id$=btnCancelSubmit]").show();
            }
            else if ($("[id$=hdfPOStatus]").val() == 1) {
                $("[id$=btnAmend]").hide();
                $("[id$=btnCancelSubmit]").show();
            }
            else {
                $("[id$=btnAmend]").hide();
                $("[id$=btnCancelSubmit]").hide();
            }

            //Set a stamp for cancelled record
            if ($("[id$=hdfIsCancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

            //End

            if ($("[id$=hdfShowTransactionPort]").val() == "1") {
                $('[id$=divFromPort]').show();
                $('[id$=divToPort]').show();
            }
            else {
                $('[id$=divFromPort]').hide();
                $('[id$=divToPort]').hide();
            }
        }

        function BindPorts() {
            var potype = $("[id$=ddlType]").val();
            var porttype = 1;
            if (potype == 1)
                porttype = 2;
            GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", url + "?SIType=" + porttype + "&PurFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS", false, false, false, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtToPort", url + "?SIType=" + porttype + "&PurToPort=1", "hdfToPortID", true, true, "FILLPORTDETAILS");

        }

        //        function SetUOMAutoCompleteGrid() {
        //            $('[id$=grdRFQResponse] tr').each(function () {
        //                GrandScriptUtils.MakeAutoCompleteDDL($(this).find("input[id*=txtCurrency]").attr("id"), url, $(this).find("input[id*=hdfCurrency]").attr("id"), true, true, "CURRENCYCODE");
        //            });
        //        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();

            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                $("[id$=hdfIsAmend]").val("0");
            }
        }


        function ShowHideItemDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divItemDetails]").show();
                $("[id$=imbShowItemDetails]").hide();
                $("[id$=imbHideItemDetails]").show();
            }
            else {
                $("[id$=divItemDetails]").hide();
                $("[id$=imbShowItemDetails]").show();
                $("[id$=imbHideItemDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode            
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=btnAddItem]").hide();
                $('[id$=pnlSaveSubmit]').hide();
                $("[id$=btnApply]").hide();
                $("[id$=imgPopupAdd]").hide();
                $("#[id*=grdRFQResponse] [id*=btnEdit]").each(function () {
                    $(this).hide();
                });
                $("#[id*=grdRFQResponse] [id*=btnDelete]").each(function () {
                    $(this).hide();
                });
            }
            else if (mode == 2) {
                //$("[id$=pnlDelete]").hide();                
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

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {

            if (targetControlID == "txtVendor") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrencyTxt", url + "?Type=" + $("[id$=hdfVendor]").val() + "&ExcDate=" + $("[id$=txtResponseDate]").val(), "hdfCurrencyTxt", true, true, "VENDORCURRENCY");
                $("[id$=btnVendor]").click();
                //$("[id$=txtCurrency]").val("Select/Type");
                //$("[id$=hdfCurrency]").val("0");

            }
            else if (targetControlID == "txtBrand") {
                $("[id$=btnSelectProduct]").click();

            }
            //            else if (targetControlID == "txtCurrency") {

            //            }
            //            else {
            //                var CurrencyCode = ($("#[id*=grdRFQResponse]").find('input[type=Text][id$=' + targetControlID + ']')).val();
            //                var currencyPK = 0;
            //                $("#[id*=grdRFQResponse] input[type=text][id*=txtCurrency]").each(function (index) {
            //                    if (($(this).parent("td").find('input[type=Text][id$=' + targetControlID + ']')).val() == CurrencyCode) {
            //                        currencyPK = parseFloat($(this).parent("td").find('input[type=Text][id$=hdfCurrency]').val());
            //                    }
            //                });
            //                $("#[id*=grdRFQResponse] input[type=text][id*=txtCurrency]").each(function (index) {
            //                    $(this).parent("td").find('input[type=Text][id$=txtCurrency]').val(CurrencyCode);
            //                    $(this).parent("td").find('input[type=hidden][id$=hdfCurrency]').val(currencyPK);
            //                });
            //            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrencyTxt", url + "?Type=" + $("[id$=hdfVendor]").val() + "&ExcDate=" + $("[id$=txtResponseDate]").val(), "hdfCurrencyTxt", true, true, "VENDORCURRENCY");
                $("[id$=btnVendor]").click();

                //$("[id$=txtCurrency]").val("Select/Type");
                //$("[id$=hdfCurrency]").val("0");

            }
            else if (targetControlID == "txtBrand") {
                $("[id$=btnSelectProduct]").click();

            }
            //            else if (targetControlID == "txtCurrency") {

            //            }
            //            else {
            //                var CurrencyCode = ($("#[id*=grdRFQResponse]").find('input[type=Text][id$=' + targetControlID + ']')).val();
            //                var currencyPK = 0;
            //                $("#[id*=grdRFQResponse] input[type=text][id*=txtCurrency]").each(function (index) {
            //                    if (($(this).parent("td").find('input[type=Text][id$=' + targetControlID + ']')).val() == CurrencyCode) {
            //                        currencyPK = parseFloat($(this).parent("td").find('input[type=Text][id$=hdfCurrency]').val());
            //                    }
            //                });
            //                $("#[id*=grdRFQResponse] input[type=text][id*=txtCurrency]").each(function (index) {
            //                    $(this).parent("td").find('input[type=Text][id$=txtCurrency]').val(CurrencyCode);
            //                    $(this).parent("td").find('input[type=hidden][id$=hdfCurrency]').val(currencyPK);
            //                });
            //            }

        }

        //        function CalculateTotal(sender) {
        ////            if (isNaN(parseFloat($(sender).val()))) {
        ////                $(sender).val("0");
        ////            }
        //            var val1 = parseFloat($(sender).val());
        //            //var Amount = 0;
        //            var quantity = 0;
        //            var qty = 0;
        //            var Rate = 0;
        //            var Amount = 0;
        //            var Discount = 0;
        //            var Tax = 0;
        //            var total = 0;
        //            var subTotal = 0;
        //            var totalDiscount = 0;
        //            var totalTax = 0;
        //            var totalShipping = 0;
        //            var totalPriceAdj = 0;
        //            var NetTotal = 0;
        //            $("#[id*=grdRFQResponse] input[type=text]").each(function (index) {

        //                if (!isNaN(parseFloat($(this).closest('tr').find('.ItemQuantity').text()))) {
        //                    qty = Number($(this).closest('tr').find('.ItemQuantity').text());
        //                    quantity = parseFloat(qty);
        //                }
        //                if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtRate]').val()))) {
        //                    Rate = parseFloat($(this).parent("td").find('input[type=Text][id$=txtRate]').val());
        //                    Amount = (quantity * Rate);
        //                    $(this).closest('tr').find('input[type=Text][id$=txtAmount]').val(Amount.toFixed(2));
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val()))) {
        //                        Discount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val());
        //                    }
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val()))) {
        //                        Tax = parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val());
        //                    }
        //                    total = (Amount + Tax) - Discount;
        //                    $(this).closest('tr').find('input[type=Text][id$=txtTotal]').val(total.toFixed(2));
        //                }
        //                if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtAmount]').val()))) {
        //                    Amount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtAmount]').val());
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val()))) {
        //                        Discount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val());
        //                    }
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val()))) {
        //                        Tax = parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val());
        //                    }
        //                    total = (Amount + Tax) - Discount;
        //                    $(this).closest('tr').find('input[type=Text][id$=txtTotal]').val(total.toFixed(2));
        //                }
        //                if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val()))) {
        //                    Discount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val());
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtAmount]').val()))) {
        //                        Amount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtAmount]').val());
        //                    }
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val()))) {
        //                        Tax = parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val());
        //                    }
        //                    total = (Amount + Tax) - Discount;
        //                    $(this).closest('tr').find('input[type=Text][id$=txtTotal]').val(total.toFixed(2));
        //                }
        //                if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val()))) {
        //                    Tax = parseFloat($(this).parent("td").find('input[type=Text][id$=txtTax]').val());
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtAmount]').val()))) {
        //                        Amount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtAmount]').val());
        //                    }
        //                    if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val()))) {
        //                        Discount = parseFloat($(this).parent("td").find('input[type=Text][id$=txtDiscount]').val());
        //                    }
        //                    total = (Amount + Tax) - Discount;
        //                    $(this).closest('tr').find('input[type=Text][id$=txtTotal]').val(total.toFixed(2));

        //                }
        //                if (!isNaN(parseFloat($(this).parent("td").find('input[type=Text][id$=txtTotal]').val()))) {
        //                    total = parseFloat($(this).parent("td").find('input[type=Text][id$=txtTotal]').val());
        //                    subTotal = subTotal + total;
        //                }
        //            });
        //            $("#[id*=grdRFQResponse]").find('input[type=Text][id$=txtSubTotalFooter]').val(subTotal.toFixed(2));
        //            if (!isNaN(parseFloat($("#[id*=txtHdrDiscount]").val()))) {
        //                totalDiscount = parseFloat($("#[id*=txtHdrDiscount]").val());
        //            }
        //            if (!isNaN(parseFloat($("#[id*=txtHdrTax]").val()))) {
        //                totalTax = parseFloat($("#[id*=txtHdrTax]").val());
        //            }
        //            if (!isNaN(parseFloat($("#[id*=txtShipping]").val()))) {
        //                totalShipping = parseFloat($("#[id*=txtShipping]").val());
        //            }
        //            if (!isNaN(parseFloat($("#[id*=txtPriceAdj]").val()))) {
        //                totalPriceAdj = parseFloat($("#[id*=txtPriceAdj]").val());
        //            }
        //            NetTotal = (subTotal + totalTax + totalShipping + totalPriceAdj) - totalDiscount;
        //            $("#[id*=txtHdrTotal]").val((NetTotal).toFixed(2));
        //        }

        function CalculateTotal(sender) {
            var subTotal = parseFloat($("#[id*=grdRFQResponse]").find('input[type=Text][id$=txtSubTotalFooter]').val());
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var totalDiscount = parseFloat($("#[id*=txtHdrDiscount]").val());
            totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
            var totalTax = parseFloat($("#[id*=txtHdrTax]").val());
            totalTax = isNaN(totalTax) ? 0 : totalTax;
            var totalShipping = parseFloat($("#[id*=txtShipping]").val());
            totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
            var totalPriceAdj = parseFloat($("#[id*=txtPriceAdj]").val());
            totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;

            var netTotal = (subTotal + totalTax + totalShipping + totalPriceAdj) - totalDiscount;
            $("#[id*=txtHdrTotal]").val((netTotal).toFixed(NumberDigits));
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtResponseDate" || controlID == null) {
                //$("[id$=txtCurrency]").val("");
                //$("[id$=hdfCurrency]").val("");
                GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "VENDOR");
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url + "?Type=" + $("[id$=hdfVendor]").val() + "&ExcDate=" + $("[id$=txtResponseDate]").val(), "hdfCurrency", true, true, "VENDORCURRENCY");
                if (controlID == "txtResponseDate" && $("[id$=hdfHasTax]").val() != "0") {
                    ShowErrorMessage('<%=Resources.Messages.TaxDateChanged %>', '<%=Resources.Messages.Information %>');
                }
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
                //CheckValidationDuplicate(valGroup);
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
        function CalculateAmount() {
            var qty = 0;
            var rate = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());
            if (!isNaN(qty) && !isNaN(rate)) {
                var amount = qty * rate;
                $("[id$=txtItemAmount]").val(amount.toFixed(CurrencyDigits));
            }
            else
                $("[id$=txtItemAmount]").val(parseFloat(0).toFixed(CurrencyDigits));

            $("[id$=btnSetTax]").click();
        }

        function ResetFromPortId() {
            $("#[id*=hdfFromPortID]").val();
        }      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlRFQResponse">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:HiddenField ID="TaskID" runat="server" />
                    <asp:HiddenField ID="TaskName" runat="server" />
                    <asp:HiddenField ID="ReferenceID" runat="server" Value="0" />
                    <asp:HiddenField ID="ProcessID" runat="server" Value="3" />
                    <asp:HiddenField ID="ApplicationID" runat="server" />
                    <asp:HiddenField runat="server" ID="ActionID" />
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium margnbotm0"  runat="server" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li id="pnlAmend">
                                        <asp:HiddenField ID="hdfIsAmend" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnAmend" CommandName="AMEND" TabIndex="51" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Amend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,Amend %>" OnClientClick="javascript:ValidatePageNow('response')"
                                            ValidationGroup="response" />
                                    </li>
                                    <li id="pnlCancelSubmit">
                                        <asp:Button runat="server" TabIndex="52" ID="btnCancelSubmit" CommandName="DELETESUBMIT"
                                            OnClick="ActionHandler" Text="<%$resources:ErpRes,CancelSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" ToolTip="<%$resources:ErpRes,CancelSubmit %>" ValidationGroup="response" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="53" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('response')"
                                            ValidationGroup="response" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="54"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('response')"
                                            ValidationGroup="response" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="55" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('response')"
                                            ValidationGroup="response" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="56" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <%-- <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnRFQSearch" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnRFQSearch" Text="<%$resources:PageNameRes,RFQSearch %>"
                                TabIndex="16" CssClass="tab-inactive" OnClick="ActionHandler" CommandName="RFQSEARCH"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnRFQRequest" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnRFQRequest" Text="<%$resources:PageNameRes,RFQRequest %>"
                                TabIndex="17" CommandName="RFQREQUEST" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnRFQResponse" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnRFQResponse" Text="<%$resources:PageNameRes,RFQResponse %>"
                                TabIndex="18" OnClientClick="javascript:return false;" CommandName="RFQRESPONSE"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>--%>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfEdit" runat="server" />
                                            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfRateDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfResponsePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:HiddenField ID="hdfPOStatus" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithSeperator" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
                                            <%--<asp:HiddenField ID="hdfHdrCurrency" runat="server" />--%>
                                                <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor%>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="true" TabIndex="1"
                                                Visible="false" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfVendor" runat="server" Value="0" />
                                            <asp:TextBox ID="txtVendor" runat="server" TabIndex="2" MaxLength="100" CssClass="select-half-a"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfVendor" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtVendor" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Vendor %>"></asp:RequiredFieldValidator>
                                            <asp:Button ID="btnVendor" runat="server" OnClick="ActionHandler" CommandName="SELECTEDINDEXCHANGED"
                                                EnableTheming="false" Style="display: none" />                                    
                                            <div class="clear">
                                            </div>

                                       <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrencyTxt"></asp:Label>
                                            <asp:TextBox ID="txtCurrencyTxt" runat="server" CssClass="input-small" TabIndex="3"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrencyTxt" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtCurrencyTxt"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>

                                                 <asp:Label runat="server" ID="lblVendorRef" Text="<%$ resources:VendorRef%>" class="middle-lbl-b"  AssociatedControlID="txtVendorRef"></asp:Label>
                                            <asp:TextBox ID="txtVendorRef" runat="server" TabIndex="4" MaxLength="100" CssClass="input-small"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfVendorRef" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtVendorRef"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VendorRef%>"></asp:RequiredFieldValidator>
                                                 <div class="clear">
                                            </div>
                                            <div id="divFromPort">
                                                <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="txtFromPort" Text='<%$ Resources:Controls,FromPort%>'></asp:Label>
                                                <asp:TextBox runat="server" ID="txtFromPort" CssClass="select-half-a"
                                                    onkeydown="ResetFromPortId();" TabIndex="6"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfFromPortID" />
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                          
                                            <asp:Label runat="server" ID="lblResponseDate" Text="<%$ resources:ResponseDate%>"
                                                Visible="false" AssociatedControlID="txtResponseDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtResponseDate" CssClass="date-picker" 
                                                onkeydown="return false;" Style="display: none;" MaxLength="11" onpaste="return false;"
                                                onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHasTax" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="vrfResponseDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtResponseDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ResponseDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtResponseDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ResponseDate %>">
                                            </asp:RequiredFieldValidator>

                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:RFQNo%>" AssociatedControlID="lblRFQNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblRFQNo"  CssClass="input-small"></asp:Label>
                                           <div style="width:18px; display:inline-block;">
                                            <asp:ImageButton ID="btnRevision" runat="server" SkinID="history" ToolTip="<%$resources:RevisionHistory %>"
                                                OnClick="ActionHandler" CommandName="REVISIONHISTORY" Visible="false" />
                                            <asp:HiddenField ID="hdfRFQNo" runat="server" />
                                            </div>
                                             <asp:Label ID="Label1" runat="server" class="middle-lbl-xsmall" Text="<%$ resources:RFQDate%>" AssociatedControlID="txtRFQDate"></asp:Label>
                                            <asp:Label runat="server" ID="lblRFQDate" CssClass="date-picker" Visible="false"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRFQDate" CssClass="input-small Uidate-picker" TabIndex="2" onkeydown="return CheckKey(event)" onpaste="return false;"
                                                MaxLength="11" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfRFQDate" runat="server" />     
                                           

                                            <asp:Label runat="server" ID="lblType" Text="<%$ resources:Type%>" AssociatedControlID="txtCurrencyTxt"  ></asp:Label>
                                            <asp:DropDownList ID="ddlType" runat="server" TabIndex="5"  CssClass="select-small-a" onchange="BindPorts();">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfType" CssClass="star" SetFocusOnError="true" ValidationGroup="response"
                                                EnableClientScript="true" runat="server" ControlToValidate="ddlType" Display="Dynamic"
                                                InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_Type%>"></asp:RequiredFieldValidator> 
                                                
                                                <div class="clear">
                                            </div>
                                            <div id="divToPort">
                                                <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text='<%$ Resources:Controls,ToPort%>'></asp:Label>
                                                <asp:TextBox runat="server" ID="txtToPort" CssClass="input-half" TabIndex="7"></asp:TextBox>
                                                <asp:HiddenField ID="hdfToPortID" runat="server" />
                                                
                                            </div>                                         
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ItemDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowItemDetails" OnClientClick="javascript:return ShowHideItemDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowItemDetails %>"  />
                                <asp:ImageButton runat="server" ID="imbHideItemDetails" OnClientClick="javascript:return ShowHideItemDetails();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideItemDetails %>"
                                     />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divItemDetails" style="display: none">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdfDSlno" runat="server" Value="0" />
                                                <asp:Label ID="lblBrand" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:Brand_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtBrand" runat="server" TabIndex="8" MaxLength="200"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBrand" runat="server" />
                                                <asp:HiddenField ID="hdfItemInvoicedQty" runat="server" Value="0" />
                                                <asp:Button ID="btnSelectProduct" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                    EnableTheming="false" Style="display: none" />
                                                <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="AddItem" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                               
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                </asp:Label>
                                                <asp:TextBox ID="txtQty" runat="server" CssClass="input-small numeric" MaxLength="10"
                                                    TabIndex="9" onchange="CalculateAmount();"></asp:TextBox>
                                                <span style="width: 10px; border: 0 none; background: none;" class="nomargin">
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="AddItem" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:QuantityValidationP2P ID="vreQuantity" runat="server" ControlToValidate="txtQty"
                                                            NumberDigits="10" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="AddItem"
                                                            NonZero="true"></cc1:QuantityValidationP2P>
                                                    </div>
                                                </span>

                                                 <asp:Label ID="Label4" runat="server" AssociatedControlID="txtUOM" Text="<%$ resources:UoM %>"  CssClass="middle-lbl-a">
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtUOM" CssClass="input-small input-disabled" onkeydown="return false;" 
                                                    onpaste="return false;"></asp:TextBox>
                                                <asp:HiddenField ID="hdfUOM" runat="server" Value="1" />
                                                <asp:RequiredFieldValidator ID="vrfUOM" CssClass="star" SetFocusOnError="true" ValidationGroup="AddItem"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtUOM" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_UOM %>"></asp:RequiredFieldValidator>

                                                  <asp:Label runat="server" ID="lblReqByDate" Text="<%$ resources:ReqdDate_Mand %>"  AssociatedControlID="txtReqByDate"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtReqByDate" TabIndex="12" MaxLength="12" CssClass="input-small Uidate-picker"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfReqByDate" />
                                                <div class="clear">
                                                </div>
                                              
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblItemDiscount" runat="server" AssociatedControlID="txtItemDiscount"
                                                    Text="<%$ resources:Discount %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtItemDiscount" runat="server" CssClass="input-w80 input-disabled numeric"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    TabIndex="12" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="RFQDISCDETAILS"
                                                    ValidationGroup="AddItem" OnClientClick="javascript:ValidatePageNow('AddItem')" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                              
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfReqByDate" CssClass="star" SetFocusOnError="false"
                                                        ValidationGroup="AddItem" EnableClientScript="true" runat="server" ControlToValidate="txtReqByDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqByDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreReqByDate" CssClass="star" ValidationGroup="AddItem"
                                                        runat="server" ControlToValidate="txtReqByDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_ReqByDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                  <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="input-small numeric margnrgt1-6per" MaxLength="10"
                                                    TabIndex="11" onchange="CalculateAmount();"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="AddItem"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                        NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                        ValidationGroup="AddItem" NonZero="true"></cc1:RateValidation>
                                                </div>
                                                <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtItemAmount" Text="<%$ resources:Amount_Mand %>" CssClass="middle-lbl-a">
                                                </asp:Label>
                                                <asp:TextBox ID="txtItemAmount" runat="server" CssClass="input-small input-disabled numeric"
                                                    MaxLength="25" onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="AddItem" EnableClientScript="true" runat="server" ControlToValidate="txtItemAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <%--  <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtItemAmount"
                                                        ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="AddItem"></cc1:AmountValidation>--%>
                                                </div>
                                                <asp:Button ID="btnSetTax" runat="server" CommandName="SHOW" OnClick="ActionHandler" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblItemTax" runat="server" AssociatedControlID="txtItemTax" Text="<%$ resources:Tax %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtItemTax" runat="server" CssClass="input-w80 input-disabled numeric"
                                                    MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    TabIndex="13" ToolTip="<%$ resources:Tax %>" CommandName="RFQTAXDETAILS" ValidationGroup="AddItem"
                                                    OnClientClick="javascript:ValidatePageNow('AddItem')" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:TextBox ID="txtTotal" runat="server" EnableTheming="false" Text="0" Style="display: none" />
                                                <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="14" MaxLength="480"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="15" CssClass="margntop2"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('AddItem')"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="AddItem"
                                                    SkinID="plus" />
                                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="16" CssClass="margntop2"
                                                    OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="PageAction_Entry"
                                                    SkinID="cancel" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                           <%-- style="overflow-x: auto; width: 1190px;"--%>
                           <%-- <div style="overflow-x: auto;>--%>
                                <div class="gridwrap scroll-container">
                                    <asp:GridView ID="grdRFQResponse" runat="server" AutoGenerateColumns="False" Width="1250px"
                                        OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>" AllowPaging="false"
                                        EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" ShowFooter="true"
                                        TabIndex="17">                                      
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfDtlPK" runat="server" Value='<%#Eval("POD_PK") %>' />
                                                    <asp:HiddenField ID="hdfslno" runat="server" Value='<%#Eval("SL_NO") %>' />
                                                    <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("POD_ITEM") %>' />
                                                    <asp:HiddenField ID="hdfInvoicedQty" runat="server" Value='<%#Eval("POD_QTY_INVOICED") %>' />
                                                    <asp:Label ID="lblItem" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ITM_TEXT"),50) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("ITM_TEXT").ToString()) %>'></asp:Label>
                                                    <%--<asp:Label ID="lblItem" runat="server" Text='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'></asp:Label>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="460px" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POD_REMARKS"),18) %>'
                                                        ToolTip='<%# Eval("POD_REMARKS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="220px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRDate" runat="server" Text='<%# Eval("POD_REQD_DATE") %>' ToolTip='<%# Eval("POD_REQD_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUoM" runat="server" Text='<%#Eval("UOM_CODE") %>' ToolTip='<%# Eval("UOM_CODE") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("POD_UOM") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Rate%>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrate" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedRate(Eval("POD_RATE")) %>'
                                                        ToolTip='<%#GetFormattedRate(Eval("POD_RATE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Quantity%>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumberWithSeperator(Eval("POD_QTY_REQUESTED")) %>'
                                                        ToolTip='<%#GetFormattedNumberWithSeperator(Eval("POD_QTY_REQUESTED")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Amount%>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedCurrencyWithSeperator(Eval("POD_AMT_VALUE")) %>'
                                                        ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("POD_AMT_VALUE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Discount%>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscount" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedCurrencyWithSeperator(Eval("POD_DISC_AMT")) %>'
                                                        ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("POD_DISC_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Tax%>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTax" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedCurrencyWithSeperator(Eval("POD_TAX")) %>'
                                                        ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("POD_TAX")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="90px" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:SubTotal %>"></asp:Label></FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Total%>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" CssClass="ItemQuantity"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <FooterStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:TextBox runat="server" ID="txtSubTotalFooter" CssClass="input-w70 numeric input-disabled"
                                                        Enabled="false" AutoPostBack="true"></asp:TextBox></FooterTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEdit" runat="server" OnClick="ActionHandler" CommandName="EDITGRID"
                                                        SkinID="imbeditgrid" ToolTip="Edit or View" TabIndex="18" CommandArgument="PageAction_Entry" />
                                                    <asp:ImageButton ID="btnDelete" runat="server" OnClick="ActionHandler" CommandName="DELETEGRID"
                                                        SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="19" CommandArgument="PageAction_Entry" />
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                           <%-- </div>--%>
                            <div id="divCalc" runat="server">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right; width: 85%;">
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Controls,Discounts %>" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="20" CommandName="RFQDISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" TabIndex="21" CssClass="input-w70 numeric input-disabled"
                                                    MaxLength="11" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%--   OtherCharge start--%>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgOtherCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    TabIndex="22" ToolTip="<%$ resources:Shipping %>" CommandName="OTHERCHARGEHEADER" />
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w70 numeric input-disabled"
                                                    TabIndex="23" MaxLength="16" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                    ErrorMessage="<%$ resources:Err_Invalid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="response"></cc1:AmountValidation>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- End OtherCharge--%>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Tax %>" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    TabIndex="24" CommandName="RFQTAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w70 numeric input-disabled"
                                                    TabIndex="25" Enabled="false" MaxLength="11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="response"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtHdrTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_TotalTax%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:OtherCharge%>" AssociatedControlID="txtShipping"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w70 numeric" TabIndex="24"
                                                    Text="0" onkeyup="CalculateTotal(this);" MaxLength="11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtVendorRef"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w70 numeric" TabIndex="26"
                                                    onkeyup="CalculateTotal(this);" MaxLength="11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfPriceAdj" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtPriceAdj"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PriceAdj%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w70 numeric input-disabled"
                                                    TabIndex="27" Enabled="false" MaxLength="13"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup"  style="margin-right:0.8%;">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandName="TAXAPPLY" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:ItemAmount %>"
                                                        AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Amount %>" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="20" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="<%$ resources:TaxType %>" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="19" runat="server" CssClass="medium"
                                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="<%$ resources:TaxName %>" AssociatedControlID="txtPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="21" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        ValidationGroup="tax" ToolTip="Add" CommandName="TAXADD" OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                                    <ItemTemplate>
                                                        <%--<asp:HiddenField ID="hdfResponsePK" runat="server" Value='<%#Eval("RRD_PK") %>' />--%>
                                                        <%--<asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQReponseTaxSplitPK) %>' />--%>
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("POT_PK") %>' />
                                                        <%--<asp:HiddenField ID="hdfTaxSlNo" runat="server" Value='<%#Eval("RRD_PK") %>' />--%>
                                                        <%-- POT_SL_NO
                                                POT_PK
                                                POT_TAX--%>
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("POT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("POT_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("POT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("POT_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("POT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("POT_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("POT_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithSeperator(Eval("POT_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithSeperator(Eval("POT_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%--////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblPaymentTerms" Text="<%$ resources:PaymentTerms %>"
                                                AssociatedControlID="txtPaymentTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaymentTerms" TabIndex="28" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfPaymentTerms" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtPaymentTerms"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentTerms%>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="Label2" Text="<%$ resources:DeliveryTerms %>" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" TabIndex="29" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfDeliveryTerms" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtDeliveryTerms"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DeliveryTerms%>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="Label3" Text="<%$ resources:OtherDetails %>" AssociatedControlID="txtOtherDetails"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtOtherDetails" TabIndex="30" TextMode="MultiLine"
                                                Height="250px" CssClass="multiline-2col"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfOtherDetails" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtOtherDetails"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_OtherDetails%>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="response" runat="server" />
                    <asp:ValidationSummary ID="vsAddItem" ValidationGroup="AddItem" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="response" />
            </div>
            <div id="divRevisionHistory" style="display: none" class="content-wrapper">
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdRevisionHistory" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:Controls,RevDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRevisionDate" runat="server" Text='<%# Eval("POH_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                        ToolTip='<%# Eval("POH_DATE", Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Controls,PoNumber %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRevisionPrint" runat="server" ToolTip="View" CssClass="text-underline"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="40%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Controls,Currency %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRCUR" runat="server" Text='<%# Eval("POH_CURRENCY_TEXT") %>' ToolTip='<%# Eval("POH_CURRENCY_TEXT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Controls,TotalAmount %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRTotalAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("POH_TOTAL_VALUE")) %>'
                                        ToolTip='<%# GetFormattedCurrency(Eval("POH_TOTAL_VALUE")) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
              <asp:HiddenField ID="hdfShowTransactionPort" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
