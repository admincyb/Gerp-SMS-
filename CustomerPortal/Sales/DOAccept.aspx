<%@ Page Title="<%$ Resources:Captions,Title_OrderAccep %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DOAccept.aspx.cs" Inherits="CustomerPortal.Sales.DOAccept"
    Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            if ($("[id$=txtQuotationDate]").attr("disabled") != true)
                GrandScriptUtils.DatePickerCommon("txtQuotationDate");
            if ($("[id$=txtCurrency]").attr("disabled") != true)
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            SetDatepickerGrid();
            SetFooter();
        }
        function SetDatepickerGrid() {
            $('[id$=grdQuotation] tr').each(function () {
                GrandScriptUtils.AddDateRangeCommon($(this).find("input[id*=txtValidFrom]").attr("id"), $(this).find("input[id*=hdfValidFrom]").attr("id"), $(this).find("input[id*=txtValidTo]").attr("id"), $(this).find("input[id*=hdfValidTo]").attr("id"));
            });
        }
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
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode 
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveSubmit]").hide();
                //$("[id$=pnlDelete]").hide();
                $("[id$=btnApply]").hide();
                $("[id$=imgPopupAdd]").hide();
                $("[id*=imbTaxRemove]").hide();
            }
            else if (mode == 2) {
                //$("[id$=pnlDelete]").hide();
            }
        }
        function ShowSaleOrder() {
            //            if ($("[id$=hdfQuotationFlag]").val() != "1")
            //                $("[id$=pnlSaleOrder]").show();
        }

        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
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

        function CalculateTotal(sender) {
            var subTotal = parseFloat($("#[id*=grdQuotation]").find('input[type=Text][id$=txtSubTotalFooter]').val());
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
            $("[id$=txtHdrTotal]").val((netTotal).toFixed(CurrencyDigits));
            if (totalShipping == 0)
                $("[id$=txtShipping]").val((totalShipping).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
        }
        function ShowSOConfirm(sender) {
            var soMsg = '<%=GetLocalResourceObject("continuepermission").ToString() %>';
            return ShowDeleteConfirm(sender, soMsg);
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtQuotationDate" && $("[id$=hdfHasTax]").val() != "0") {
                ShowErrorMessage('<%=Resources.Messages.TaxDateChanged %>', '<%=Resources.Messages.Information %>');
            }
        }
        function SetFooter() {
            var totalCartonsOrBags = parseInt(0);
            var totalQty = parseFloat(0);
            var totalAmount = parseFloat(0);
            var totalDiscount = parseFloat(0);
            var totalTax = parseFloat(0);
            var isFooter = true;

            $("[id$=grdQuotation] tr:has(td)").each(function () {
                isFooter = true;
                var lblItemCartonsOrBags = $(this).find("[id*=lblItemCartonsOrBags]");
                var txtQuantity = $(this).find("[id*=txtQuantity]");
                var hdfTotalPieces = $(this).find("[id*=hdfTotalPieces]");
                var vreQuantity = $(this).find("[id*=vreQuantity]");
                var isValidQty = txtQuantity.length > 0 && $(txtQuantity).val().match(new RegExp(vreQuantity[0].validationexpression));
                if (isValidQty) {
                    totalQty += parseFloat($(txtQuantity).val());
                    if (lblItemCartonsOrBags.length > 0 && hdfTotalPieces.length > 0) {
                        var cartonsBags = Math.ceil(parseFloat($(txtQuantity).val()) / parseFloat($(hdfTotalPieces).val()));
                        $(lblItemCartonsOrBags).html(cartonsBags);
                        totalCartonsOrBags += cartonsBags;
                    }
                    isFooter = false;
                }
                var txtAmount = $(this).find("[id*=txtAmount]");
                if (txtAmount.length > 0) {
                    totalAmount += parseFloat($(txtAmount).val());
                    isFooter = false;
                }
                var txtDiscount = $(this).find("[id*=txtDiscount]");
                if (txtDiscount.length > 0) {
                    totalDiscount += parseFloat($(txtDiscount).val());
                    isFooter = false;
                }
                var txtTax = $(this).find("[id*=txtTax]");
                if (txtTax.length > 0) {
                    totalTax += parseFloat($(txtTax).val());
                    isFooter = false;
                }
                if (isFooter) {
                    var lblItemTotalCarton = $(this).find("[id*=lblItemTotalCarton]");
                    if (lblItemTotalCarton.length > 0) {
                        $(lblItemTotalCarton).html(totalCartonsOrBags);
                    }
                    var lblItemTotalQty = $(this).find("[id*=lblItemTotalQty]");
                    if (lblItemTotalQty.length > 0) {
                        $(lblItemTotalQty).html(totalQty.toFixed(NumberDigits));
                    }
                    var lblItemTotalAmount = $(this).find("[id*=lblItemTotalAmount]");
                    if (lblItemTotalAmount.length > 0) {
                        $(lblItemTotalAmount).html(totalAmount.toFixed(CurrencyDigits));
                    }
                    var lblDiscountTotal = $(this).find("[id*=lblDiscountTotal]");
                    if (lblDiscountTotal.length > 0) {
                        $(lblDiscountTotal).html(totalDiscount.toFixed(CurrencyDigits));
                    }
                    var lblTaxTotal = $(this).find("[id*=lblTaxTotal]");
                    if (lblTaxTotal.length > 0) {
                        $(lblTaxTotal).html(totalTax.toFixed(CurrencyDigits));
                    }
                }
            });
        }

        function QtyConversion(ctrl) {
            var DecimalDigits = 0;
            var hdfConvFactor = 1;
            var ItemQtyPcs = 0;

            if (!isNaN(parseFloat($("#[id*=hdfNumberDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfNumberDigits]").val());
            }
            $("#[id*=grdQuotation] input[type=text][id*=txtBrandQuantity]").each(function (index) {
                var ItemQty = 0;
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        ItemQty = parseFloat($(this).val());
                    }
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=hdfBrandUOMConvFactor]").val()))) {
                    var number = Number($(this).closest('tr').find("#[id*=hdfBrandUOMConvFactor]").val().replace(/[^0-9\.]+/g, ""));
                    hdfConvFactor = parseFloat(number);
                }
                ItemQtyPcs = ItemQty * hdfConvFactor;
                $(this).closest('tr').find("#[id*=txtQuantity]").val(ItemQtyPcs.toFixed(DecimalDigits));
            });
        }
    </script>
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
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('quotation')"
                                            ValidationGroup="quotation" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('quotation')"
                                            ValidationGroup="quotation" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                        <%--<asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />--%>
                                    </li>
                                    <li runat="server" id="pnlSaleOrder">
                                        <asp:Button runat="server" ID="btnSaleOrder" CommandName="SALEORDER" TabIndex="14"
                                            Text="<%$resources:SaleOrder %>" OnClick="ActionHandler" OnClientClick="return ShowSOConfirm(this);"
                                            ToolTip="<%$resources:SaleOrder %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-sale" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="14" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('quotation')"
                                            ValidationGroup="quotation" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-save" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="12" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="15" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnEnqList" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="13" CommandName="ENQUIRYLIST"
                                Text="<%$resources:PageNameRes,DirectOrderListing %>" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQtnList" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQtnList" TabIndex="13" CommandName="QUOTATIONLIST"
                                Text="<%$resources:PageNameRes,OrderAcceptListing %>" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,DirectOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="26" CommandName="ENQUIRY" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,DirectOrderAccept %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="27" CommandName="QUOTE" OnClick="ActionHandler"
                                CssClass="tab-active" OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Customer %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" Enabled="false" MaxLength="100" CssClass="input-disabled select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <%-- <div class="starwrap">
                                               <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="enquiry" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                                    </div>--%>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblEnq" runat="server" Text="<%$ resources:RefNo%>" AssociatedControlID="lblEnquiryNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblEnquiryNo" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfEnquiryNo" runat="server" />
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:RefDate%>" AssociatedControlID="lblEnquiryDate" CssClass="middle-lbl-c"></asp:Label>
                                            <asp:Label runat="server" ID="lblEnquiryDate" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfEnquiryDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>" >
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="3" CssClass="select-small-ax">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtShipBy" runat="server" MaxLength="100" CssClass="input-disabled" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfShipBy" runat="server" />--%>
                                            <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>" CssClass="middle-lbl-xsmall-a3">
                                            </asp:Label>
                                            <asp:TextBox ID="txtToPort" runat="server" TabIndex="4" MaxLength="100" CssClass="input-medium"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfQuotationFlag" runat="server" />
                                            <asp:HiddenField ID="hdfQuotationPK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:EnquiryNo%>" AssociatedControlID="lblQuotationNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblQuotationNo" CssClass="input-small"></asp:Label>
                                            <%-- <asp:ImageButton ID="btnRevision" runat="server" OnClick="ActionHandler" CommandName="REVISIONHISTORY"
                                                CommandArgument="PageAction_Entry" SkinID="history" ToolTip="<%$resources:RevisionHistory %>" />--%>
                                            <asp:Label runat="server" ID="lblQuotationDate" Text="<%$ resources:EnquiryDate%>"
                                                AssociatedControlID="txtQuotationDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtQuotationDate" CssClass="input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:HiddenField ID="hdfHasTax" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="vrfQuotationDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtQuotationDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_QuotationDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfTaxDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtQuotationDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_QuotationDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:HiddenField ID="hdfQuotationNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" />
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="select-medium" TabIndex="4" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblBkDate" runat="server" Text="<%$ resources:BookingDate%>" AssociatedControlID="lblBookingDate" CssClass="middle-lbl-xsmall-a1"></asp:Label>
                                            <asp:Label runat="server" ID="lblBookingDate" CssClass="input-small"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                Text="<%$ resources:Transhipment %>"></asp:Label>
                                            <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="5" CssClass="select-small-a1">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtTranshipment" runat="server" MaxLength="100" CssClass="input-disabled" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTranshipment" runat="server" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="fields-grpwrap color-grey grp-before color-white">
                                <div class="header">
                                    <h1>
                                        <%= GetLocalResourceObject("ItemDetails").ToString()%></h1>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <div class="gridwrap scroll-container">
                                        <asp:GridView ID="grdQuotation" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            Width="1690px" ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="5">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfCEDPK" runat="server" Value='<%#Eval("CED_PK") %>' />
                                                        <asp:HiddenField ID="hdfQuotationDtlPK" runat="server" Value='<%#Eval("CED_PK") %>' />
                                                        <asp:HiddenField ID="hdfCusItemPK" runat="server" Value='<%#Eval("CED_CUST_ITEM") %>' />
                                                        <asp:Label ID="lblBrandName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CED_CUST_ITEM_TEXT"),55)  %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_CUST_ITEM_CODE"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="300px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CED_ITEM_TEXT"),28) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_ITEM_TEXT"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("CED_ITEM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="180px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CED_SALE_UOM_TEXT"),10) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_SALE_UOM_TEXT"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("CED_UOM") %>' />
                                                        <asp:HiddenField ID="hdfBrandUOMPK" runat="server" Value='<%#Eval("CED_SALE_UOM") %>' />
                                                        <asp:HiddenField ID="hdfBrandUOMConvFactor" runat="server" Value='<%#Eval("CED_SALE_UOM_CONV") %>' />
                                                    </ItemTemplate>
                                                    <%--CED_UOM_TEXT--%>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Quantity%>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBrandQuantity" runat="server" CssClass="input-w80 numeric" Text='<%# GetFormattedNumber(Eval("CED_SALE_QTY")) %>'
                                                            ToolTip='<%#GetFormattedNumber(Eval("CED_SALE_QTY")) %>' AutoPostBack="true"
                                                            onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onblur="QtyConversion(this);"
                                                            OnTextChanged="ActionHandler" MaxLength="13"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtBrandQuantity"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtBrandQuantity"
                                                            NumberDigits="7" ErrorMessage="<%$ resources:Err_Invalid_Quantity %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="quotation"
                                                            NonZero="true"></cc1:QuantityValidation>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ItemQty%>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="input-w80 input-disabled numeric"
                                                            Text='<%# GetFormattedNumber(Eval("CED_ENQ_QTY")) %>' ToolTip='<%#GetFormattedNumber(Eval("CED_ENQ_QTY")) %>'
                                                            Enabled="false" MaxLength="11"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="90px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:CartonsOrBags %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCartonsOrBags" runat="server" ToolTip='<%# Eval("PACKING_TEXT") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTotalPieces" runat="server" Value='<%# Eval("APS_TOTAL_PCS") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px" CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblItemTotalCarton" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReqdShipDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReqdShipDate" runat="server" Text='<%#Eval("CED_REQUIRED_DATE") %>'
                                                            ToolTip='<%# Eval("CED_REQUIRED_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("CED_REMARKS")), 10) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_REMARKS"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRate" runat="server" Text='<%#GetFormattedRate(Eval("CED_RATE")) %>'
                                                            CssClass="input-w58 numeric" MaxLength="16" ToolTip='<%#GetFormattedRate(Eval("CED_RATE")) %>'
                                                            OnTextChanged="ActionHandler" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfRate" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="quotation"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Invalid_Rate %>"
                                                            NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                            ValidationGroup="quotation"></cc1:RateValidation>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_AMOUNT")) %>'
                                                            CssClass="input-w80 numeric" MaxLength="15" Enabled="false" ToolTip='<%#GetFormattedCurrency(Eval("CED_AMOUNT")) %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfAmount" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                            ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="quotation"></cc1:AmountValidation>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblItemTotalAmount" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="90px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                            ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                            ToolTip="<%$ resources:Controls,Discounts %>" CommandName="RFQDISCDETAILS" />
                                                        <asp:TextBox ID="txtDiscount" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_DISCOUNT")) %>'
                                                            CssClass="input-w80 numeric" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("CED_DISCOUNT")) %>'
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDiscount" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount %>">
                                                        </asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblDiscountTotal"></asp:Label></FooterTemplate>
                                                    <ItemStyle Width="110px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                            ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                            ToolTip="<%$ resources:Tax %>" CommandName="RFQTAXDETAILS" />
                                                        <asp:TextBox ID="txtTax" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_TAX")) %>'
                                                            CssClass="input-w80 numeric" MaxLength="15" ToolTip='<%#GetFormattedCurrency(Eval("CED_TAX")) %>'
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfTax" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="quotation"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtTax" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                        </asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblTaxTotal"></asp:Label></FooterTemplate>
                                                    <ItemStyle Width="110px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="input-w80 numeric" MaxLength="15"
                                                            Text='<%#GetFormattedCurrency(Eval("CED_AMT_NET_TOTAL")) %>' Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfTotal" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:TextBox runat="server" ID="txtSubTotalFooter" CssClass="input-w80 numeric" Enabled="false"
                                                            AutoPostBack="true"></asp:TextBox></FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ValidFrom %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtValidFrom" Text='<%#Eval("CED_VALID_FROM") %>'
                                                            CssClass="date-picker" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
                                                            onchange="AfterDateSelect(null)"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdfValidFrom" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ValidTo %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtValidTo" Text='<%#Eval("CED_VALID_TO") %>' CssClass="date-picker"
                                                            onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdfValidTo" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Comments %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Text='<%#Eval("CED_COMMENTS") %>' ID="txtComments" MaxLength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="110px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnItemRates" runat="server" OnClick="ActionHandler" CommandName="ITEMRATES"
                                                            SkinID="history" ToolTip="Show Rates" Visible="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div id="divCalc">
                                        <div class="gridwrap">
                                            <table id="tblCalc" class="gridwraptable gridwrap btn-margin">
                                                <tr>
                                                    <td style="text-align: right; width: 85%;">
                                                        <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                            ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                            ToolTip="<%$ resources:Controls,Discounts %>" CommandName="RFQDISCHEADER" />
                                                        <asp:TextBox ID="txtHdrDiscount" runat="server" TabIndex="6" CssClass="input-w80 numeric"
                                                            MaxLength="16" Enabled="false"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                            ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                            ToolTip="<%$ resources:Tax %>" CommandName="RFQTAXHEADER" />
                                                        <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric" TabIndex="7"
                                                            Enabled="false" MaxLength="16"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="quotation"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtHdrTax" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_TotalTax%>"></asp:RequiredFieldValidator>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric" TabIndex="8"
                                                            onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>
                                                        <div class="starwrap">
                                                            <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                                ErrorMessage="<%$ resources:Err_Valid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="quotation"></cc1:AmountValidation>
                                                            <asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric" TabIndex="9"
                                                            onchange="CalculateTotal(this);" MaxLength="16"></asp:TextBox>
                                                        <div class="starwrap">
                                                            <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                                ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="quotation"></cc1:AmountValidation>
                                                            <asp:RequiredFieldValidator ID="vrfPriceAdj" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtPriceAdj"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PriceAdj%>"></asp:RequiredFieldValidator>
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
                                                        <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric" TabIndex="10"
                                                            Enabled="false" MaxLength="16"></asp:TextBox>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandName="TAXAPPLY" CommandArgument="PageAction_Entry" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="Item Amount" AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="Amount" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="20" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="15"></asp:TextBox>
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
                                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="19" runat="server" CssClass="medium"
                                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="Name" AssociatedControlID="txtPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="21" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        ValidationGroup="tax" CommandArgument="PageAction_Entry" CommandName="TAXADD"
                                                        OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("ETD_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("ETD_TAX") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("ETD_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("ETD_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("ETD_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("ETD_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("ETD_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("ETD_NAME"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("ETD_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("ETD_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("ETD_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                            SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div id="ItemRateDialog" style="display: none" class="content-wrapper">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblItemCode" Text="ItemCode" AssociatedControlID="lblItemCodeTxt"></asp:Label>
                                                <asp:Label runat="server" ID="lblItemCodeTxt"></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblItemName" Text="ItemName" AssociatedControlID="lblItemNameTxt"></asp:Label>
                                                <asp:Label runat="server" ID="lblItemNameTxt"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdItemRates" Width="100%" AllowSorting="True" OnSorting="ActionHandler"
                                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Customer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVendorLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CUS_CODE"), 14) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CUS_CODE"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="14%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LastQuotedDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastQuotedDateLst" runat="server" Text='<%# Eval("CIH_LAST_QUOT_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("CIH_LAST_QUOT_DATE", Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="14%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LastQuotedRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastQuotedRateLst" runat="server" Text='<%# Eval("CIH_LAST_QUOT_RATE") %>'
                                                        ToolTip='<%# Eval("CIH_LAST_QUOT_RATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LastOrderDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastOrderDateLst" runat="server" Text='<%# Eval("CIH_LAST_ORDR_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("CIH_LAST_ORDR_DATE", Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LastOrderRate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastOrderRateLst" runat="server" Text='<%# Eval("CIH_LAST_ORDR_RATE") %>'
                                                        ToolTip='<%# Eval("CIH_LAST_ORDR_RATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LastOrderQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLastOrderQtyLst" runat="server" Text='<%# Eval("CIH_LAST_ORDR_QTY") %>'
                                                        ToolTip='<%# Eval("CIH_LAST_ORDR_RATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LeadTime">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeadTimeLst" runat="server" Text='<%# Eval("CIH_LEAD_TIME") %>'
                                                        ToolTip='<%# Eval("CIH_LEAD_TIME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="fields-grpwrap color-grey grp-before pad-t10 color-white">
                                <div class="header">
                                    <h1>
                                        <%= GetLocalResourceObject("TermsnCond").ToString() %></h1>
                                    <div class="clear">
                                    </div>
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms" CssClass="w25perc"
                                                        Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="select-half"
                                                        TabIndex="14">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label3" runat="server" Text="" AssociatedControlID="txtDeliveryTerms" CssClass="w25perc"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="15" TextMode="MultiLine"
                                                        CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms" CssClass="w25perc"
                                                        Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="select-half"
                                                        TabIndex="16">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms" CssClass="w25perc"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="17" TextMode="MultiLine"
                                                        CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause" CssClass="w25perc"
                                                        Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="select-half"
                                                        TabIndex="18">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause" CssClass="w25perc"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="19" TextMode="MultiLine"
                                                        CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>" CssClass="w25perc"
                                                        AssociatedControlID="ddlCustAddress"></asp:Label>
                                                    <asp:DropDownList ID="ddlCustAddress" runat="server" TabIndex="20" AutoPostBack="true" CssClass="select-half"
                                                        OnSelectedIndexChanged="ActionHandler">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="Label5" runat="server" AssociatedControlID="txtShippingAddress" CssClass="w25perc"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TabIndex="21"
                                                        TextMode="MultiLine" EnableTheming="false" CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                        onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="22" TextMode="MultiLine"
                                                        CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                        ErrorMessage="<%$ Resources:Err_Remarks %>" ValidationExpression="^[\s\S]{0,500}$"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
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
                                                    <asp:Label ID="lblRevisionDate" runat="server" Text='<%# Eval("CEH_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("CEH_DATE", Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:QuotNo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkRevisionPrint" runat="server" ToolTip="View"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="40%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:CUR %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRCUR" runat="server" Text='<%# Eval("CEH_CURRENCY_TEXT") %>' ToolTip='<%# Eval("CEH_CURRENCY_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRTotalAmount" runat="server" Text='<%# Eval("CEH_NET_AMOUNT") %>'
                                                        ToolTip='<%# Eval("CEH_NET_AMOUNT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
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
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="quotation" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="quotation" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
