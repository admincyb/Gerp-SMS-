<%@ Page Title="<%$ Resources:Captions,Title_SaleOrderDetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="SaleOrderDetails.aspx.cs" Inherits="ERPSMS_v01.Sales.SaleOrderDetails"
    Theme="Classic" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
    <%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtSaleOrderDate");
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");            
            SetInitGrid();
        }
        function GoBack() {
            var backURL = document.referrer;
            backURL = backURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            window.location = backURL;
        }
        function SetInitGrid() {
            $('[id$=grdItemDetails] tr').each(function () {
                GrandScriptUtils.AddDateRangeCommon($(this).find("input[id*=txtValidFrom]").attr("id"), $(this).find("input[id*=hdfValidFrom]").attr("id"), $(this).find("input[id*=txtValidTo]").attr("id"), $(this).find("input[id*=hdfValidTo]").attr("id"));
                GrandScriptUtils.MakeAutoCompletePaired($(this).find("[id*=txtArtWork]").attr("id"), $(this).find("[id*=hdfArtWork]").attr("id")
                , uiUrl + "?Type=" + $(this).find("[id*=hdfCusItemPK]").val(), $(this).find("[id*=hdfArtWorkUrl]").attr("id"), true, true, "ARTWORK");
                var lnkArtWork = $(this).find("[id$=lnkArtWork]");
                if ($(this).find("[id*=hdfArtWork]").val() != "" && $(this).find("[id*=hdfArtWork]").val() != "0") {
                    var artUrl = $(this).find("[id$=hdfArtWorkUrl]").val();
                    if (artUrl != null && jQuery.trim(artUrl) != "") {
                        //artUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? artUrl.replace("~", "") : "/" + virtualPath + artUrl.replace("~", ""));
                        $(lnkArtWork).show();
                        $(lnkArtWork).attr("target", "_blank");
                        $(lnkArtWork).attr('href', artUrl);
                    }
                    else {
                        $(lnkArtWork).removeAttr("target");
                        $(lnkArtWork).attr('href', '#');
                        $(lnkArtWork).hide();
                    }
                }
                else {
                    $(lnkArtWork).removeAttr("target");
                    $(lnkArtWork).attr('href', '#');
                    $(lnkArtWork).hide();
                }
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
            }
            else if (mode == 2) {
                $("[id$=pnlAlert]").hide();
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function AfterDateSelect(controlID) {
            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }
        }

        function CalculateTotal(sender) {
            var subTotal = parseFloat($("#[id*=grdItemDetails]").find('input[type=Text][id$=lblSubTotalFooter]').html());
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
            $("#[id*=txtHdrTotal]").val((netTotal).toFixed(3));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(3));
        }
        function AfterAutoCompleteSelect(targetControlID) {
            var lnkArtWork = $("[id$=" + targetControlID + "]").closest('tr').find("[id$=lnkArtWork]");
            if (targetControlID.indexOf("txtArtWork") >= 0) {
                if ($("[id$=" + targetControlID + "]").closest('tr').find("[id*=hdfArtWork]").val() != ""
                && $("[id$=" + targetControlID + "]").closest('tr').find("[id*=hdfArtWork]").val() != "0") {
                    var artUrl = $("[id$=" + targetControlID + "]").closest('tr').find("[id$=hdfArtWorkUrl]").val();
                    if (artUrl != null && jQuery.trim(artUrl) != "") {
                        //artUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == ""
                        //? artUrl.replace("~", "") : "/" + virtualPath + artUrl.replace("~", ""));
                        $(lnkArtWork).attr("target", "_blank");
                        $(lnkArtWork).show();
                        $(lnkArtWork).attr('href', artUrl);
                    }
                    else {
                        $(lnkArtWork).removeAttr("target");
                        $(lnkArtWork).attr('href', '#');
                        $(lnkArtWork).hide();
                    }
                }
                else {
                    $(lnkArtWork).removeAttr("target");
                    $(lnkArtWork).attr('href', '#');
                    $(lnkArtWork).hide();
                }
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID.indexOf("txtArtWork") >= 0) {
                var lnkArtWork = $("[id$=" + targetControlID + "]").closest('tr').find("[id$=lnkArtWork]");
                $(lnkArtWork).removeAttr("target");
                $(lnkArtWork).attr('href', '#');
                $(lnkArtWork).hide();
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
                                <li runat="server" id="pnlSaveSubmit">
                                <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="37" Text="<%$resources:ErpRes,SaveSubmit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="38" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="39" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-save" />
                                    </li>
                                    <li id="pnlPrintSO" runat="server">
                                        <asp:Button runat="server" TabIndex="40" ID="btnPrintSO" CommandName="PRINTSO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                     <li id="pnlAlert" runat="server" >
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="41" Text="<%$resources:Controls,Alert %>"
                                           OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-alert" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="42" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
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
                                            <asp:HiddenField ID="hdfSaleOrderPK" runat="server" />
                                            <asp:HiddenField ID="hdfSaleOrderRate" runat="server" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:SaleOrderNo%>" AssociatedControlID="lblSaleOrderNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblSaleOrderNo" CssClass="medium" TabIndex="1"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblRef" runat="server" Text="<%$ resources:Reference%>" AssociatedControlID="lblReferenceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblReferenceNo" CssClass="medium"></asp:Label>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSaleOrderDate" Text="<%$ resources:SaleOrderDate%>"
                                                AssociatedControlID="txtSaleOrderDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSaleOrderDate" CssClass="date-picker" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtSaleOrderDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfSaleOrderNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="medium input-disabled" MaxLength="100"
                                                TabIndex="3"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblBuyer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Buyer %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" Enabled="false" MaxLength="100" CssClass="input-disabled"
                                                TabIndex="4"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtBuyerAddress"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBuyerAddress" MaxLength="500" TextMode="MultiLine"
                                                TabIndex="6" CssClass="multiline-1col input-disabled" onkeydown="return EnableArrowKey(event)"
                                                onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                Text="<%$ resources:SpecialCause %>"></asp:Label>
                                            <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtSpecialCause_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                Enabled="false"></asp:TextBox>--%>
                                            <%--<asp:HiddenField ID="hdfSpecialCause" runat="server" />--%>
                                            <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="7" TextMode="MultiLine"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms"
                                                Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                            <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="8">
                                            </asp:DropDownList>
                                            <%-- <asp:TextBox ID="txtDeliveryTerms_Txt" runat="server" MaxLength="100" Enabled="false"
                                                CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfDeliveryTerms" runat="server" />--%>
                                            <asp:Label ID="Label1" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="10" TextMode="MultiLine"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                            <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="9">
                                            </asp:DropDownList>
                                            <%-- <asp:TextBox ID="txtPaymentTerms_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentTerms" runat="server" />--%>
                                            <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="11" TextMode="MultiLine"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="12">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtShipBy" runat="server" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfShipBy" runat="server" />--%>
                                            <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                Text="<%$ resources:Transhipment %>"></asp:Label>
                                            <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="14">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtTranshipment" runat="server" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTranshipment" runat="server" />--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtToPort" runat="server" TabIndex="13" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfEnquiryNo" runat="server" />
                                            <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="ddlFromPort" Text="<%$ resources:FromPort %>"></asp:Label>
                                            <asp:DropDownList ID="ddlFromPort" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="15">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBankDetails" runat="server" AssociatedControlID="ddlBankDetails"
                                                Text="<%$ resources:BankDetails %>"></asp:Label>
                                            <asp:DropDownList ID="ddlBankDetails" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="16">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblOriginofGoods" runat="server" AssociatedControlID="ddlOriginofGoods"
                                                Text="<%$ resources:OriginofGoods %>"></asp:Label>
                                            <asp:DropDownList ID="ddlOriginofGoods" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="18">
                                            </asp:DropDownList>
                                            <%--<asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department %>" AssociatedControlID="lblPageDeptText"></asp:Label>
                                            <asp:Label ID="lblPageDeptText" runat="server" ></asp:Label>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSaleOrderType" Text="<%$ resources:Type%>" AssociatedControlID="ddlSaleOrderType"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" runat="server" TabIndex="17">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSaleOrderType"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderType %>"
                                                InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:CheckBox ID="chkNeedAdvPay" runat="server" TabIndex="19" Text="<%$resources:NeedAdvPay%>"
                                                TextAlign="Left" />
                                            <div class="clear">
                                            </div>
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
                                        <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            Width="1650px" ShowFooter="true" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                        <asp:HiddenField ID="hdfSaleOrderDtlPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                        <asp:HiddenField ID="hdfCusItemPK" runat="server" Value='<%#Eval("SOD_CUST_ITEM") %>' />
                                                        <asp:Label ID="lblBrandName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_CUST_ITEM_TEXT"),13) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("SOD_CUST_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<HeaderStyle Width="120px" />--%>
                                                    <ItemStyle Width="140px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_TEXT"),16) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("SOD_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("SOD_ITEM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_UOM_TEXT"),13) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("SOD_UOM_TEXT").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("SOD_UOM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40px" />
                                                    <%-- <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                                    </FooterTemplate>--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumber(Eval("SOD_QTY")) %>'
                                                            ToolTip='<%# GetFormattedNumber(Eval("SOD_QTY")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px" CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalFooter" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PcsBox %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPcsBox" runat="server" Text='<%# Eval("SOD_CIM_PCS_PER_IP") %>'
                                                            ToolTip='<%#  Eval("SOD_CIM_PCS_PER_IP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:BoxCorton %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBoxCorton" runat="server" Text='<%# Eval("SOD_CIM_PCS_PER_OP") %>'
                                                            ToolTip='<%# Eval("SOD_CIM_PCS_PER_OP") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:QtyCarton %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQtyCarton" runat="server" Text='<%# GetCeiledInteger(Eval("SOD_QTY_CARTONS")) %>'
                                                            ToolTip='<%# GetCeiledInteger(Eval("SOD_QTY_CARTONS").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReqdShipDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReqdShipDate" runat="server" Text='<%#Eval("SOD_REQUIRED_BY") %>'
                                                            ToolTip='<%# Eval("SOD_REQUIRED_BY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRate" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_RATE")) %>'
                                                            ToolTip='<%# GetFormattedCurrency(Eval("SOD_RATE")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_AMOUNT")) %>'
                                                            ToolTip='<%# GetFormattedCurrency(Eval("SOD_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                            ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCDETAILS" />
                                                            <asp:TextBox ID="txtDiscount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_DISCOUNT")) %>'
                                                            CssClass="input-w70 numeric input-disabled" MaxLength="15" ToolTip='<%# GetFormattedCurrency(Eval("SOD_DISCOUNT")) %>'
                                                            onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="124px" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                            ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS" />
                                                            <asp:TextBox ID="txtTax" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_TAX")) %>'
                                                            CssClass="input-w70 numeric input-disabled" MaxLength="15" ToolTip='<%# GetFormattedCurrency(Eval("SOD_TAX")) %>'
                                                            onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="124px" CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label runat="server" ID="lblfooter" Text="<%$ resources:SubTotal %>"></asp:Label></FooterTemplate>
                                                </asp:TemplateField>
                                                <%--Pay Now--%>
                                                <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_NET_AMOUNT")) %>'
                                                            ToolTip='<%# GetFormattedCurrency(Eval("SOD_NET_AMOUNT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" CssClass="amount-numeric" />
                                                    <FooterStyle CssClass="amount-numeric" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblSubTotalFooter" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:LotNo %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_NO"))) %>' ID="txtLotNo" MaxLength="100"
                                                            TabIndex="20"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:LotSize %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_SIZE"))) %>' ID="txtLotSize" MaxLength="100"
                                                            TabIndex="21"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ArtWork %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtArtWork" Text='<%#Eval("SOD_ART_WORK_TEXT") %>'
                                                            CssClass="medium" TabIndex="22" MaxLength="100"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfArtWork" runat="server" Value='<%#Eval("SOD_ART_WORK") %>' />
                                                        <asp:HiddenField ID="hdfArtWorkUrl" runat="server" />
                                                        <%-- <asp:RequiredFieldValidator ID="vrfArtWork" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtArtWork"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ArtWork %>" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>">
                                                        </asp:RequiredFieldValidator>--%>
                                                        <a runat="server" id="lnkArtWork" class="viewlist-BTN nomargin" href='#'
                                                            title="<%$ resources:ErpRes,View %>"></a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="165px" />
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                            SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="13" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
                                                    <td style="text-align: right"  class="btn-margin">
                                                        <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                            ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                        <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w80 numeric input-disabled"
                                                            MaxLength="11" Enabled="false"></asp:TextBox>
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
                                                            ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                        <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                            Enabled="false" MaxLength="11"></asp:TextBox>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric input-disabled"
                                                            Enabled="false" MaxLength="11" TabIndex="23"></asp:TextBox>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric input-disabled"
                                                            Enabled="false" MaxLength="11" TabIndex="24"></asp:TextBox>
                                                        <div class="clear">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                                    </td>
                                                    <td style="text-align: right">
                                                        <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled"
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
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInspection" runat="server" AssociatedControlID="ddlInspection"
                                                Text="<%$ resources:Inspection %>"></asp:Label>
                                            <asp:DropDownList ID="ddlInspection" runat="server" TabIndex="25">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfInspection" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlInspection"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Inspection %>" InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblExportDoc" runat="server" AssociatedControlID="ddlExportDoc" Text="<%$ resources:ExportDoc %>"></asp:Label>
                                            <asp:DropDownList ID="ddlExportDoc" runat="server" TabIndex="26">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfExportDoc" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlExportDoc"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExportDoc %>" InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblNotifyParty" runat="server" AssociatedControlID="ddlNotifyParty"
                                                Text="<%$ resources:NotifyParty %>"></asp:Label>
                                            <asp:DropDownList ID="ddlNotifyParty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="27">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblNotifyPrty" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNotifyParty" MaxLength="500" TabIndex="29" TextMode="MultiLine"
                                                CssClass="multiline-1col input-disabled" onkeydown="return EnableArrowKey(event)"
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
                                        <div class="div2col-L">
                                            <asp:Label ID="lblConsigneeDetails" runat="server" AssociatedControlID="ddlConsigneeDetails"
                                                Text="<%$ resources:ConsigneeDetails %>"></asp:Label>
                                            <asp:DropDownList ID="ddlConsigneeDetails" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="28">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblConsigneeDtl" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtConsigneeDetails" MaxLength="500" TabIndex="30"
                                                onkeydown="return EnableArrowKey(event)" onpaste="return false;" TextMode="MultiLine"
                                                CssClass="multiline-1col input-disabled" onkeyup="limitText(this,500);"></asp:TextBox>
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
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPackingInstruction" runat="server" AssociatedControlID="txtPackingInstruction"
                                                Text="<%$ resources:PackingInstruction %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtPackingInstruction" runat="server" TabIndex="31" MaxLength="100"></asp:TextBox>
                                            <asp:Label ID="lblShppingIntimationto" runat="server" AssociatedControlID="txtShppingIntimationto"
                                                Text="<%$ resources:ShppingIntimationto %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtShppingIntimationto" runat="server" TabIndex="33" MaxLength="100"></asp:TextBox>
                                            <asp:Label ID="lblShppingIntimationtoFax" runat="server" AssociatedControlID="txtShppingIntimationtoFax"
                                                Text="<%$ resources:ShppingIntimationtoFax %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtShppingIntimationtoFax" runat="server" TabIndex="35" MaxLength="100"></asp:TextBox>
                                            <a id="lnkTerms" runat="server" onclick="ShowContract();" href="#">
                                                <%=GetLocalResourceObject("ContractTerms").ToString() %>
                                            </a>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPortofDischarge" runat="server" AssociatedControlID="txtPortofDischarge"
                                                Text="<%$ resources:PortofDischarge %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtPortofDischarge" runat="server" TabIndex="32" MaxLength="100"></asp:TextBox>
                                            <asp:Label ID="lblAgent" runat="server" AssociatedControlID="ddlAgent" Text="<%$ resources:Agent %>">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlAgent" runat="server" TabIndex="34" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfAgentName" runat="server" />
                                            <asp:HiddenField ID="hdfAgentAddress" runat="server" />
                                            <asp:HiddenField ID="hdfAgentCountry" runat="server" />
                                            <asp:HiddenField ID="hdfAgentCountryText" runat="server" />
                                            <asp:HiddenField ID="hdfAgentZip" runat="server" />
                                            <asp:HiddenField ID="hdfAgentPhone" runat="server" />
                                            <asp:HiddenField ID="hdfAgentMobile" runat="server" />
                                            <asp:HiddenField ID="hdfAgentFax" runat="server" />
                                            <asp:HiddenField ID="hdfAgentEmail" runat="server" />
                                            <asp:Label ID="lblSupplimentarydetails" runat="server" AssociatedControlID="txtSupplimentarydetails"
                                                Text="<%$ resources:Supplimentarydetails %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtSupplimentarydetails" runat="server" TabIndex="36" MaxLength="100"></asp:TextBox>
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
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="so" runat="server" />
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
                <div class="content-wrapper">
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT")) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="38%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("SLT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("SLT_NAME").ToString()) %>'></asp:Label>
                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("SLT_NAME") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="38%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("SLT_TAX_AMT")) %>'
                                            ToolTip='<%#GetFormattedCurrency(Eval("SLT_TAX_AMT")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="24%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
