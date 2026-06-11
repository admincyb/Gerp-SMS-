<%@ Page Title="<%$ Resources:Captions,Title_OrderAcceptance %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="Classic"
    CodeBehind="DirectOrderAccept.aspx.cs" Inherits="CustomerPortal.Sales.DirectOrderAccept" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            if ($("[id$=txtQuotationDate]").attr("disabled") != true)
                GrandScriptUtils.DatePickerCommon("txtQuotationDate");
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCurrency]").attr("disabled") != true)
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            SetDatepickerGrid();
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
            if ($("[id$=hdfQuotationFlag]").val() != "1")
                $("[id$=pnlSaleOrder]").show();
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
            $("[id$=txtHdrTotal]").val((netTotal).toFixed(3));
            if (totalShipping == 0)
                $("[id$=txtShipping]").val((totalShipping).toFixed(3));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(3));
        }
        function ShowSOConfirm(sender) {
            var soMsg = '<%=GetLocalResourceObject("continuepermission").ToString() %>';
            return ShowDeleteConfirm(sender, soMsg);
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
                                    <li runat="server" id="pnlSaleOrder" style="display: none">
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
                        <li><span id="Span1" runat="server" class="list-inactive">
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="26" CommandName="ENQUIRYLIST"
                                OnClick="ActionHandler" CssClass="list-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,Order %>"
                                TabIndex="26" CommandName="ENQUIRY" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,OrderAccept %>"
                                TabIndex="27" CommandName="QUOTE" OnClick="ActionHandler" CssClass="tab-active"
                                OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Customer %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" Enabled="false" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <div class="starwrap">
                                                <%--<asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="enquiry" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                                    </div>--%>
                                            </div>
                                            <asp:Label runat="server" ID="lblQuotationDate" Text="<%$ resources:QuotationDate%>"
                                                AssociatedControlID="txtQuotationDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtQuotationDate" CssClass="date-picker" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfQuotationDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtQuotationDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_QuotationDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" Text="<%$ resources:EnquiryDate%>" AssociatedControlID="lblEnquiryDate"></asp:Label>
                                            <asp:Label runat="server" ID="lblEnquiryDate" CssClass="date-picker"></asp:Label>
                                            <asp:HiddenField ID="hdfEnquiryDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="3">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtShipBy" runat="server" MaxLength="100" CssClass="input-disabled" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfShipBy" runat="server" />--%>
                                            <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                Text="<%$ resources:Transhipment %>"></asp:Label>
                                            <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="5">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtTranshipment" runat="server" MaxLength="100" CssClass="input-disabled" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTranshipment" runat="server" />--%>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfQuotationFlag" runat="server" />
                                            <asp:HiddenField ID="hdfQuotationPK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:QuotationNo%>" AssociatedControlID="lblQuotationNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblQuotationNo" CssClass="medium"></asp:Label>
                                            <asp:ImageButton ID="btnRevision" runat="server" OnClick="ActionHandler" CommandName="REVISIONHISTORY"
                                                SkinID="history" ToolTip="<%$resources:RevisionHistory %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:HiddenField ID="hdfQuotationNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" />
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="medium" TabIndex="4" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblEnq" runat="server" Text="<%$ resources:EnquiryNo%>" AssociatedControlID="lblEnquiryNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblEnquiryNo" CssClass="medium"></asp:Label>
                                            <asp:HiddenField ID="hdfEnquiryNo" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtToPort" runat="server" TabIndex="4" MaxLength="100"></asp:TextBox>
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
                                    <div class="gridwrap  grid-w930">
                                        <asp:GridView ID="grdQuotation" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            Width="1600px" ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="5">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfCEDPK" runat="server" Value='<%#Eval("CED_PK") %>' />
                                                        <asp:HiddenField ID="hdfQuotationDtlPK" runat="server" Value='<%#Eval("CED_PK") %>' />
                                                        <asp:HiddenField ID="hdfCusItemPK" runat="server" Value='<%#Eval("CED_CUST_ITEM") %>' />
                                                        <asp:Label ID="lblBrandName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CED_CUST_ITEM_TEXT"),15) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("CED_CUST_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                        <%--<asp:Label ID="lblItem" runat="server" Text='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <%--<HeaderStyle Width="140px" />--%>
                                                    <ItemStyle Width="140px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CED_ITEM_TEXT"),18) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("CED_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("CED_ITEM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CED_UOM_TEXT"),13) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("CED_UOM_TEXT").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("CED_UOM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="input-w58 numeric" Text='<%# GetFormattedNumber(Eval("CED_ENQ_QTY")) %>'
                                                            ToolTip='<%#GetFormattedNumber(Eval("CED_ENQ_QTY")) %>' AutoPostBack="true" OnTextChanged="ActionHandler"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true" ValidationGroup="quotation"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rngQuantity" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtQuantity" Type="Double" ErrorMessage="<%$ resources:Err_Invalid_Quantity %>"
                                                            MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="68px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PcsBox %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPcsBox" runat="server" Text='<%#Eval("CED_CIM_PCS_PER_IP") %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("CED_CIM_PCS_PER_IP").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:BoxCorton %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBoxCorton" runat="server" Text='<%#Eval("CED_CIM_PCS_PER_OP") %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("CED_CIM_PCS_PER_OP").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:BookingDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBookingDate" runat="server" Text='<%#Eval("CED_BOOKING_DATE") %>'
                                                            ToolTip='<%# Eval("CED_BOOKING_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
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
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRate" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_RATE")) %>'
                                                            CssClass="input-w58 numeric" MaxLength="11" ToolTip='<%#GetFormattedCurrency(Eval("CED_RATE")) %>'
                                                            OnTextChanged="ActionHandler" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfRate" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="quotation"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rngRate" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtRate" Type="Double" ErrorMessage="<%$ resources:Err_Invalid_Rate %>"
                                                            MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="68px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_AMOUNT")) %>'
                                                            CssClass="input-w70 numeric" MaxLength="11" Enabled="false" ToolTip='<%#GetFormattedCurrency(Eval("CED_AMOUNT")) %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfAmount" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rngAmount" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtAmount" Type="Double" ErrorMessage="<%$ resources:Err_Invalid_Amount %>"
                                                            MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                            ToolTip="<%$ resources:Controls,Discounts %>" CommandName="RFQDISCDETAILS" />
                                                        <asp:TextBox ID="txtDiscount" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_DISCOUNT")) %>'
                                                            CssClass="input-w70 numeric" MaxLength="11" ToolTip='<%#GetFormattedCurrency(Eval("CED_DISCOUNT")) %>'
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDiscount" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rngDiscount" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtDiscount" Type="Double" ErrorMessage="<%$ resources:Err_Invliad_Discount %>"
                                                            MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="110px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                            ToolTip="<%$ resources:Tax %>" CommandName="RFQTAXDETAILS" />
                                                        <asp:TextBox ID="txtTax" runat="server" Text='<%#GetFormattedCurrency(Eval("CED_TAX")) %>'
                                                            CssClass="input-w70 numeric" MaxLength="11" ToolTip='<%#GetFormattedCurrency(Eval("CED_TAX")) %>'
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfTax" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="quotation"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtTax" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rngTax" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="quotation" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtTax" Type="Double" ErrorMessage="<%$ resources:Err_Invliad_Tax %>"
                                                            MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="110px" CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:SubTotal %>"></asp:Label></FooterTemplate>
                                                </asp:TemplateField>
                                                <%--Pay Now--%>
                                                <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotal" runat="server" CssClass="input-w70 numeric" MaxLength="11"
                                                            Text='<%#GetFormattedCurrency(Eval("CED_AMT_NET_TOTAL")) %>' Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfTotal" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:TextBox runat="server" ID="txtSubTotalFooter" CssClass="input-w70 numeric" Enabled="false"
                                                            AutoPostBack="true"></asp:TextBox></FooterTemplate>
                                                </asp:TemplateField>
                                                <%--Remove--%>
                                                <%--Pay Now--%>
                                                <asp:TemplateField HeaderText="<%$ resources:ValidFrom %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtValidFrom" Text='<%#Eval("CED_VALID_FROM") %>'
                                                            CssClass="date-picker" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
                                                            onchange="AfterDateSelect(null)"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdfValidFrom" />
                                                        <%--<asp:RequiredFieldValidator ID="vrfValidFrom" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtValidFrom"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Valid_From %>">
                                                </asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                    <%--<ItemStyle Width="10%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ValidTo %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtValidTo" Text='<%#Eval("CED_VALID_TO") %>' CssClass="date-picker"
                                                            onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdfValidTo" />
                                                        <%-- <asp:RequiredFieldValidator ID="vrfValidTo" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="quotation" EnableClientScript="true" runat="server" ControlToValidate="txtValidTo"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Valid_To %>">
                                                </asp:RequiredFieldValidator>--%>
                                                    </ItemTemplate>
                                                    <%--<ItemStyle Width="10%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Comments %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Text='<%#Eval("CED_COMMENTS") %>' ID="txtComments" MaxLength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <%--<ItemStyle Width="10%" />--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnItemRates" runat="server" OnClick="ActionHandler" CommandName="ITEMRATES"
                                                            SkinID="history" ToolTip="Show Rates" Visible="false" />
                                                    </ItemTemplate>
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
                                                            ToolTip="<%$ resources:Controls,Discounts %>" CommandName="RFQDISCHEADER" />
                                                        <asp:TextBox ID="txtHdrDiscount" runat="server" TabIndex="6" CssClass="input-w80 numeric"
                                                            MaxLength="11" Enabled="false"></asp:TextBox>
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
                                                            ToolTip="<%$ resources:Tax %>" CommandName="RFQTAXHEADER" />
                                                        <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric" TabIndex="7"
                                                            Enabled="false" MaxLength="11"></asp:TextBox>
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
                                                            onchange="CalculateTotal(this);" MaxLength="11"></asp:TextBox>
                                                        <div class="starwrap">
                                                            <asp:RegularExpressionValidator ID="vreShipping" runat="server" ControlToValidate="txtShipping"
                                                                ErrorMessage="<%$ resources:Err_Valid_Shipping %>" ValidationExpression="^\$?([0-9]{0,12})?(\.[0-9]{0,3})?$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="quotation">
                                                            </asp:RegularExpressionValidator>
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
                                                            onchange="CalculateTotal(this);" MaxLength="11"></asp:TextBox>
                                                        <div class="starwrap">
                                                            <asp:RegularExpressionValidator ID="vrePriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                                ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" ValidationExpression="^\$?(-{0,1})?([0-9]{0,12})?(\.[0-9]{0,3})?$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="quotation">
                                                            </asp:RegularExpressionValidator>
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
                                                            Enabled="false" MaxLength="13"></asp:TextBox>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandName="TAXAPPLY" />
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
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" ValidationExpression="^\$?([0-9]{0,12})?(\.[0-9]{0,3})?$"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax">
                                                        </asp:RegularExpressionValidator>
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
                                                        ValidationGroup="tax" CommandName="TAXADD" OnClientClick="javascript:ValidatePageNow('tax')" />
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
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("ETD_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("ETD_NAME").ToString()) %>'></asp:Label>
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
                            <%--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
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
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("CUS_CODE").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="14%" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Rating">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRatingLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemRateRating), 10) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateRating) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>--%>
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
                            <%--////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
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
                                                    <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms"
                                                        Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                        TabIndex="14">
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtDeliveryTerms_Txt" runat="server" MaxLength="100" Enabled="false"
                                                        CssClass="input-disabled"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfDeliveryTerms" runat="server" />--%>
                                                    <asp:Label ID="Label3" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="15" TextMode="MultiLine"
                                                        CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                        Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                        TabIndex="16">
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtPaymentTerms_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                        Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfPaymentTerms" runat="server" />--%>
                                                    <asp:Label runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="17" TextMode="MultiLine"
                                                        CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                        Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                        TabIndex="18">
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtSpecialCause_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                        Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfSpecialCause" runat="server" />--%>
                                                    <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="19" TextMode="MultiLine"
                                                        CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>"
                                                        AssociatedControlID="ddlCustAddress"></asp:Label>
                                                    <asp:DropDownList ID="ddlCustAddress" runat="server" TabIndex="20" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ActionHandler">
                                                    </asp:DropDownList>
                                                    <%--<asp:TextBox ID="txtCustAddress_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                        Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCustAddress" runat="server" />--%>
                                                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtShippingAddress"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TabIndex="21"
                                                        TextMode="MultiLine" EnableTheming="false" CssClass="multiline-1col" onkeydown="limitText(this,500);"
                                                        onkeyup="limitText(this,500);"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="22" TextMode="MultiLine"
                                                        CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                        ErrorMessage="<%$ Resources:Err_Remarks %>" ValidationExpression="^[\s\S]{0,500}$"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
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
