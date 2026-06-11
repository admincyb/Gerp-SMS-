<%@ Page Title="<%$ Resources:Captions,Title_IODetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="InternalOrderDetails.aspx.cs" Inherits="CustomerPortal.Sales.InternalOrderDetails"
    Theme="Classic" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        function InitComponents() {
            SetInitGrid();
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
        function SetInitGrid() {
            $('[id$=grdItemDetails] tr').each(function () {
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
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode 
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaleOrder]").hide();
                //$("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                //$("[id$=pnlDelete]").hide();
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
        function ShowContract() {
            ShowContainerDiv('#divTerms', '<%=GetLocalResourceObject("ContractTerms").ToString() %>', '700', '500');
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
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="27" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li id="pnlPrintIO">
                                        <asp:Button runat="server" TabIndex="30" ID="btnPrintIO" CommandName="PRINTIO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="31" CommandArgument="SEC_ActionPanel"
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
                                            <asp:HiddenField ID="hdfSaleOrderPK" runat="server" />
                                            <asp:HiddenField ID="hdfSaleOrderRate" runat="server" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:SaleOrderNo%>" AssociatedControlID="lblSaleOrderNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblSaleOrderNo" CssClass="medium" TabIndex="1"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblRef" runat="server" Text="<%$ resources:Reference%>" AssociatedControlID="lblReferenceNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblReferenceNo" CssClass="medium"></asp:Label>
                                        </div>
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
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="medium input-disabled" MaxLength="100"></asp:TextBox>
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
                                            <asp:TextBox ID="txtCustomer" runat="server" Enabled="false" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtBuyerAddress"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBuyerAddress" MaxLength="500" TextMode="MultiLine"
                                                CssClass="multiline-1col input-disabled" onkeydown="return EnableArrowKey(event)"
                                                onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                Text="<%$ resources:SpecialCause %>"></asp:Label>
                                            <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                Enabled="false" TabIndex="2">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtSpecialCause_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                Enabled="false"></asp:TextBox>--%>
                                            <%--<asp:HiddenField ID="hdfSpecialCause" runat="server" />--%>
                                            <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="3" TextMode="MultiLine"
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
                                                TabIndex="4">
                                            </asp:DropDownList>
                                            <%-- <asp:TextBox ID="txtDeliveryTerms_Txt" runat="server" MaxLength="100" Enabled="false"
                                                CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfDeliveryTerms" runat="server" />--%>
                                            <asp:Label ID="Label1" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="5" TextMode="MultiLine"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-L">
                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                            <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                            <%-- <asp:TextBox ID="txtPaymentTerms_Txt" runat="server" MaxLength="100" CssClass="input-disabled"
                                                Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPaymentTerms" runat="server" />--%>
                                            <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="7" TextMode="MultiLine"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="8">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtShipBy" runat="server" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfShipBy" runat="server" />--%>
                                            <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                Text="<%$ resources:Transhipment %>"></asp:Label>
                                            <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="10">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtTranshipment" runat="server" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTranshipment" runat="server" />--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtToPort" runat="server" TabIndex="9" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfEnquiryNo" runat="server" />
                                            <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="ddlFromPort" Text="<%$ resources:FromPort %>"></asp:Label>
                                            <asp:DropDownList ID="ddlFromPort" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="11">
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
                                                TabIndex="12">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblOriginofGoods" runat="server" AssociatedControlID="ddlOriginofGoods"
                                                Text="<%$ resources:OriginofGoods %>"></asp:Label>
                                            <asp:DropDownList ID="ddlOriginofGoods" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                TabIndex="14">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSaleOrderType" Text="<%$ resources:Type%>" AssociatedControlID="ddlSaleOrderType"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" runat="server" TabIndex="13">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSaleOrderType"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderType %>"
                                                InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department %>" AssociatedControlID="lblPageDeptText"></asp:Label>
                                            <asp:Label ID="lblPageDeptText" runat="server" ></asp:Label>

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
                                            Width="1300px" ShowFooter="true" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                        <asp:HiddenField ID="hdfSaleOrderDtlPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                        <asp:HiddenField ID="hdfCusItemPK" runat="server" Value='<%#Eval("SOD_CUST_ITEM") %>' />
                                                        <asp:Label ID="lblBrandName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_CUST_ITEM_TEXT"),15) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("SOD_CUST_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<HeaderStyle Width="120px" />--%>
                                                    <ItemStyle Width="140px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_TEXT"),24) %>'
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
                                                        <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# ERP.Utilities.CommonFunctions.GetQty(Eval("SOD_QTY")) %>'
                                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetQty(Eval("SOD_QTY")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="60px" CssClass="amount-numeric" />
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
                                                            ToolTip='<%# GetCeiledInteger(Eval("SOD_QTY_CARTONS")) %>'></asp:Label>
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
                                                <asp:TemplateField HeaderText="<%$ resources:LotNo %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_NO"))) %>'
                                                            ID="txtLotNo" MaxLength="100" TabIndex="15"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:LotSize %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Text='<%#Eval("SOD_LOT_SIZE") %>' ID="txtLotSize" MaxLength="100"
                                                            TabIndex="16"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ArtWork %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtArtWork" Text='<%#Eval("SOD_ART_WORK_TEXT") %>'
                                                            CssClass="medium" TabIndex="16" MaxLength="100"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfArtWork" runat="server" Value='<%#Eval("SOD_ART_WORK") %>' />
                                                        <asp:HiddenField ID="hdfArtWorkUrl" runat="server" />
                                                        <asp:RequiredFieldValidator ID="vrfArtWork" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtArtWork"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ArtWork %>" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>">
                                                        </asp:RequiredFieldValidator>
                                                        <a runat="server" id="lnkArtWork" class="view-BTN nomargin" href='#'
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
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInspection" runat="server" AssociatedControlID="ddlInspection"
                                                Text="<%$ resources:Inspection %>"></asp:Label>
                                            <asp:DropDownList ID="ddlInspection" runat="server" TabIndex="17">
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
                                            <asp:DropDownList ID="ddlExportDoc" runat="server" TabIndex="17">
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
                                                TabIndex="17">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblNotifyPrty" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNotifyParty" MaxLength="500" TabIndex="18" TextMode="MultiLine"
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
                                                TabIndex="19">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblConsigneeDtl" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtConsigneeDetails" MaxLength="500" TabIndex="20"
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
                                            <asp:TextBox ID="txtPackingInstruction" runat="server" TabIndex="21" MaxLength="100"></asp:TextBox>
                                            <asp:Label ID="lblShppingIntimationto" runat="server" AssociatedControlID="txtShppingIntimationto"
                                                Text="<%$ resources:ShppingIntimationto %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtShppingIntimationto" runat="server" TabIndex="23" MaxLength="100"></asp:TextBox>
                                            <asp:Label ID="lblShppingIntimationtoFax" runat="server" AssociatedControlID="txtShppingIntimationtoFax"
                                                Text="<%$ resources:ShppingIntimationtoFax %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtShppingIntimationtoFax" runat="server" TabIndex="25" MaxLength="100"></asp:TextBox>
                                            <a id="lnkTerms" runat="server" onclick="ShowContract();">
                                                <%=GetLocalResourceObject("ContractTerms").ToString() %>
                                            </a>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPortofDischarge" runat="server" AssociatedControlID="txtPortofDischarge"
                                                Text="<%$ resources:PortofDischarge %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtPortofDischarge" runat="server" TabIndex="22" MaxLength="100"></asp:TextBox>
                                            <asp:Label ID="lblAgent" runat="server" AssociatedControlID="ddlAgent" Text="<%$ resources:Agent %>">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlAgent" runat="server" TabIndex="24">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSupplimentarydetails" runat="server" AssociatedControlID="txtSupplimentarydetails"
                                                Text="<%$ resources:Supplimentarydetails %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtSupplimentarydetails" runat="server" TabIndex="26" MaxLength="100"></asp:TextBox>
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
            <div id="divTerms" class="max-425">
                <asp:Literal ID="ltrTerms" runat="server"></asp:Literal>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
