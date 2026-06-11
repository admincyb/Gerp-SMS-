<%@ Page Title="<%$ Resources:Captions,Title_RFQ %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="RFQResponse.aspx.cs" Inherits="ERPSMS_v01.PurchaseOrderManagement.RFQResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ register assembly="ERP.Utilities" namespace="ERP.Utilities.Validations" tagprefix="cc1" %>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtResponseDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url + "?Type=" + $("[id$=ddlVendor]").val() + "&ExcDate=" + $("[id$=txtResponseDate]").val(), "hdfCurrency", true, true, "VENDORCURRENCY");
            //SetUOMAutoCompleteGrid();
            //            $("[id$=txtHdrDiscount],[id$=txtRate],[id$=txtDiscount]").focusout(function () {
            //                $(this).change();
            //                return false;
            //            });
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
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                //$("[id$=pnlDelete]").hide();
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
                $("[id$=btnVendor]").click();
            }
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
                $("[id$=btnVendor]").click();
            }
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
            var DecimalDigits = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
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
            $("#[id*=txtHdrTotal]").val((netTotal).toFixed(DecimalDigits));
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtResponseDate" || controlID == null) {
                $("[id$=txtCurrency]").val("");
                $("[id$=hdfCurrency]").val("");
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url + "?Type=" + $("[id$=ddlVendor]").val() + "&ExcDate=" + $("[id$=txtResponseDate]").val(), "hdfCurrency", true, true, "VENDORCURRENCY");
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlRFQResponse">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div class="buttoncontainer-fields floatLeft" id="divSBUCompany" style="display:none">
                                    <%-- <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:DDLCompany %>" AssociatedControlID="ddlCompany"></asp:Label>--%>
                                    <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" AutoPostBack="true"
                                        class="w145 margnbotm0" OnSelectedIndexChanged="ActionHandler" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfCompanyPk" runat="server" Value="0" />
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="14" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('response')"
                                            ValidationGroup="response" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
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
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateDigits" runat="server" Value="3" />
                                            <asp:HiddenField ID="hdfResponsePK" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                                            <%--<asp:HiddenField ID="hdfHdrCurrency" runat="server" />--%>
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:RFQNo%>" AssociatedControlID="lblRFQNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblRFQNo" CssClass="lbl-18perc"></asp:Label>
                                            <asp:HiddenField ID="hdfRFQNo" runat="server" />
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:RFQDate%>" AssociatedControlID="lblRFQDate"></asp:Label>
                                            <asp:Label runat="server" ID="lblRFQDate" CssClass="date-picker input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfRFQDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor%>" AssociatedControlID="ddlVendor"></asp:Label>
                                            <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="true" TabIndex="1"
                                                OnSelectedIndexChanged="ActionHandler" CssClass="input-half">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfVendor" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlVendor" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Vendor %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblResponseDate" Text="<%$ resources:ResponseDate%>"
                                                AssociatedControlID="txtResponseDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtResponseDate" CssClass="date-picker input-small"
                                                TabIndex="2" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
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
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="4"
                                                MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblVendorRef" Text="<%$ resources:VendorRef%>" AssociatedControlID="txtVendorRef"></asp:Label>
                                            <asp:TextBox ID="txtVendorRef" runat="server" TabIndex="3" MaxLength="100" CssClass="input-halfsmall"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfVendorRef" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtVendorRef"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_VendorRef%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdRFQResponse" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="<%$ resources:PageSize %>" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="false" ShowFooter="true" TabIndex="5">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfRRDPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQResponseDtlPK) %>' />
                                                <asp:HiddenField ID="hdfRFQDtlPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQDtlPK) %>' />
                                                <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQItemPK) %>' />
                                                <asp:Label ID="lblItem" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.RFQDtlItemText),12) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.RFQDtlItemText).ToString()) %>'></asp:Label>
                                                <%--<asp:Label ID="lblItem" runat="server" Text='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.VendorMst+"."+Resources.DataFieldRes.VendorName) %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.RFQItemDisc),14) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.RFQItemDisc).ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Specifications%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpecifications" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.RFQItemSpec),11) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.RFQItemSpec).ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Quantity%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedNumber(Eval(Resources.DataFieldRes.RFQQtyRequested)) %>'
                                                    ToolTip='<%#GetFormattedNumber(Eval(Resources.DataFieldRes.RFQQtyRequested)) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUoM" runat="server" Text='<%#Eval(Resources.DataFieldRes.RFQUoMText) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.RFQUoMText).ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQUomPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCurrency" runat="server" CssClass="small" Text='<%#Eval("RRD_CURRENCY_TEXT") %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfCurrency" runat="server" Value='<%#Eval("RRD_CURRENCY") %>' />
                                                <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>" Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#GetFormattedRate(Eval(Resources.DataFieldRes.RFQResponseRate)) %>'
                                                    CssClass="input-w70 numeric" MaxLength="11" ToolTip='<%#GetFormattedRate(Eval(Resources.DataFieldRes.RFQResponseRate)) %>'
                                                    OnTextChanged="ActionHandler" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="hdfRate" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="response"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rngRate" runat="server" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" Display="Dynamic" Text="*"
                                                    ControlToValidate="txtRate" Type="Double" ErrorMessage="<%$ resources:Err_Invalid_Rate %>"
                                                    MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQReponseAmt)) %>'
                                                    CssClass="input-w70 numeric" MaxLength="11" Enabled="false" ToolTip='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQReponseAmt)) %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdfAmount" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rngAmount" runat="server" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" Display="Dynamic" Text="*"
                                                    ControlToValidate="txtAmount" Type="Double" ErrorMessage="<%$ resources:Err_Invalid_Amount %>"
                                                    MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Controls,Discounts %>" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    CommandName="RFQDISCDETAILS" Visible='<%#GetDiscountConfiguration()%>' />
                                                <asp:TextBox ID="txtDiscount" runat="server" Text='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQReponseDtlDisc)) %>'
                                                    CssClass="input-w70 numeric" MaxLength="11" ToolTip='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQReponseDtlDisc)) %>'
                                                    Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfDiscount" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Discount %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rngDiscount" runat="server" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" Display="Dynamic" Text="*"
                                                    ControlToValidate="txtDiscount" Type="Double" ErrorMessage="<%$ resources:Err_Invliad_Discount %>"
                                                    MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Tax %>" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    CommandName="RFQTAXDETAILS" Visible='<%#GetTaxConfiguration()%>' />
                                                <asp:TextBox ID="txtTax" runat="server" Text='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQResponseDtlTax)) %>'
                                                    CssClass="input-w70 numeric" MaxLength="11" ToolTip='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQResponseDtlTax)) %>'
                                                    Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfTax" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="response"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Tax %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rngTax" runat="server" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" Display="Dynamic" Text="*"
                                                    ControlToValidate="txtTax" Type="Double" ErrorMessage="<%$ resources:Err_Invliad_Tax %>"
                                                    MinimumValue="0" MaximumValue="99999999"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:SubTotal %>"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Pay Now--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Total %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="input-w70 numeric" MaxLength="11"
                                                    Text='<%#GetFormattedCurrency(Eval("RRD_AMT_NET_TOTAL")) %>' ToolTip='<%#GetFormattedCurrency(Eval("RRD_AMT_NET_TOTAL")) %>'
                                                    Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfTotal" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:TextBox runat="server" ID="txtSubTotalFooter" CssClass="input-w70 numeric" Enabled="false"
                                                    AutoPostBack="true"></asp:TextBox></FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right; width: 85%;">
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Controls,Discounts %>" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    CommandName="RFQDISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" TabIndex="6" CssClass="input-w70 numeric"
                                                    MaxLength="11" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>
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
                                                    ToolTip="<%$ resources:Tax %>" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')"
                                                    CommandName="RFQTAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w70 numeric" TabIndex="7"
                                                    Enabled="false" MaxLength="11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="response"
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
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w70 numeric" TabIndex="8"
                                                    onkeyup="CalculateTotal(this);" MaxLength="11"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtVendorRef"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w70 numeric" TabIndex="9"
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
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w70 numeric" TabIndex="10"
                                                    Enabled="false" MaxLength="13"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
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
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:ItemAmount %>"
                                                        AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Amount %>" AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" TabIndex="20" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="20"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <%--  <asp:RegularExpressionValidator ID="vreTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,3})?$"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax">
                                                        </asp:RegularExpressionValidator>--%>
                                                        <cc1:AmountValidation ID="vreTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="14" Display="Dynamic"
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
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQReponseTaxSplitPK) %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQReponseTaxPK) %>' />
                                                        <%--<asp:HiddenField ID="hdfTaxSlNo" runat="server" Value='<%#Eval("RRD_PK") %>' />--%>
                                                        <%-- POT_SL_NO
                                                POT_PK
                                                POT_TAX--%>
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval(Resources.DataFieldRes.RFQResponseTaxText)) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval(Resources.DataFieldRes.RFQResponseTaxText)) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval(Resources.DataFieldRes.RFQResponseTaxText)) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval(Resources.DataFieldRes.RFQResponseTaxText))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TaxName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval(Resources.DataFieldRes.RFQReponseTaxName) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.RFQReponseTaxName).ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval(Resources.DataFieldRes.RFQReponseTaxName) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQResponseTaxAmt)) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.RFQResponseTaxAmt)) %>'></asp:Label>
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
                                            <asp:TextBox runat="server" ID="txtPaymentTerms" TabIndex="11" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator ID="vrfPaymentTerms" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="response" EnableClientScript="true" runat="server" ControlToValidate="txtPaymentTerms"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentTerms%>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="Label2" Text="<%$ resources:DeliveryTerms %>" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" TabIndex="12" TextMode="MultiLine"
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
                                            <asp:TextBox runat="server" ID="txtOtherDetails" TabIndex="13" TextMode="MultiLine"
                                                CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
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
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsDiscount" runat="server" />
            <asp:HiddenField ID="hdfIsTax" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
