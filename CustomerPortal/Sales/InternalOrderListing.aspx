<%@ Page Title="<%$ Resources:Captions,Title_IOListing %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="InternalOrderListing.aspx.cs" Inherits="CustomerPortal.Sales.InternalOrderListing"
    Theme="ClassicExt" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            //         GrandScriptUtils.AddDateRange("txtActFromDate", "hdfActFromDate", "txtActToDate", "hdfActToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", true, true, "CUSTOMERLIST");

            if ($("[id$=hdfCustomerPK]").val() != "") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url + "?CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfSoPK", true, true, "IONUMBER");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url, "hdfSoPK", true, true, "IONUMBER");
            }
            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            if ($("[id$=hdfCustomerPK]").val() != "") {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerID]"));
            }
            if ($("[id$=hdfIsSelected]").val() == "0") {
                $("[id$=grdSoList] tr td input[id*=rbtSelect]").attr("checked", false);
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

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function AfterGridExpand(row) {


            if ($("[id$=grdDOHdr]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedDOItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetDoDetails]").click();
                }
            }
            if ($("[id$=grdSoList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function DisableAuto(extender) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
        function EnableAuto(extender) {
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
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
                                <ul runat="server" id="pnlListing">
                                    <li id="pnlEdit">
                                        <asp:Button runat="server" TabIndex="9" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="10" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="list-active">
                            <asp:LinkButton runat="server" ID="lbnSOListing" ToolTip="Listing" TabIndex="11"
                                CssClass="list-active" OnClick="ActionHandler" CommandName="SALEORDERLISTING"
                                CommandArgument="SEC_ActionPanel"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbSODetails" Text="<%$resources:PageNameRes,InternalOrder %>"
                                TabIndex="12" OnClick="ActionHandler" CommandName="SALEORDER" CssClass="tab-inactive"
                                CommandArgument="SEC_ActionPanel"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <%--  <div class="clear">
                            </div>--%>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="100" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="101" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b" TabIndex="102">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblCompanyFilter" runat="server" Text="<%$resources:Controls,CompanyPlant%>"
                                                CssClass="middle-lbl-xsmall" AssociatedControlID="ddlCompanyFilter"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyFilter" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlVoucherCompany');"
                                                CssClass="select-small-a1">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half margnbotm0" MaxLength="100"
                                                TabIndex="103"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:HiddenField ID="hdfCustomerPK" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$resources:SONo %>" AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" CssClass="input-small-a margnbotm0"
                                                MaxLength="100" TabIndex="104"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSoPK" runat="server" Value="" />
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="105" CommandName="SEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="106" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <asp:HiddenField ID="hdfIsSelected" runat="server" Value="0" />
                                <cc1:ExtGridView runat="server" ID="grdSoList" AutoGenerateColumns="False" Width="100%"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    PageSize="<%$ resources:PageSize %>" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="8" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>--%>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PONumber %>">
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>--%>
                                                <asp:Label ID="lblPoNo"  runat="server" Text='<%# Eval(Resources.DataFieldRes.SOH_Reference) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SOH_Reference) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlantCode"  CssClass="<%# Eval(Resources.DataFieldRes.CMP_LINE_COLOUR) %>" Text="<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>"
                                                    ToolTip="<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_NAME) %>" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.CustomerMst + "." + Resources.DataFieldRes.CustomerName),53) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.CustomerMst+"."+Resources.DataFieldRes.CustomerName),300) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval(Resources.DataFieldRes.SOType) %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval( Resources.DataFieldRes.SohToPort),32) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SohToPort),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipmentDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipmentDateLst" runat="server" Text='<%# Eval( Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataTableRes.CurrencyMst +"." + Resources.DataFieldRes.CurrencyCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.CurrencyMst +"." + Resources.DataFieldRes.CurrencyCode) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval(Resources.DataFieldRes.SOCurrency) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("SOH_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.SaleOrderTotal, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SaleOrderTotal) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--<asp:Button ID="imgApproved" runat="server" />--%>
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SaleOrderStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdOrderDetails" AutoGenerateColumns="False"
                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        AllowPaging="false" OnRowDataBound="ActionHandler">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProduct" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemCode),15) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemCode) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>" SortExpression="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="48%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DesiredDeliveryDate %>" SortExpression="<%$ resources:DataFieldRes,SodDesiredDeliveryDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesiredDeliveryDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.ConfigMst+"."+ Resources.DataFieldRes.cfgData) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst+"."+ Resources.DataFieldRes.cfgData) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>" SortExpression="<%$ resources:DataFieldRes,SodSaleQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvoicedQty %>" SortExpression="<%$ resources:DataFieldRes,SOInvoicedQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoicedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SOInvoicedQty, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SOInvoicedQty, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--                                                            <asp:TemplateField HeaderText="<%$ resources:UnitCost %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitCost" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodUnitCost, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodUnitCost, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,SodAmount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--First--%>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                            <%--Secnd Division--%>
                            <h4>
                                <%= GetGlobalResourceObject("Captions", "GoodsOutward").ToString()%>
                            </h4>
                            <%--Secnd Division--%>
                            <div class="gridwrap hierarchical-wrap">
                                <cc1:ExtGridView runat="server" ID="grdDOHdr" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                    PageSize="<%$ resources:PageSize %>">
                                    <%--OnRowDataBound="ActionHandler"--%>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:GonNumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDoNumber" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedDOItem" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfDoPk" Value="<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>" />
                                                <asp:Button runat="server" ID="btnGetDoDetails" OnClick="ActionHandler" CommandName="DODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <%-- <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DoDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderDate, Resources.Constants.DateFormatGrid) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ShipTo),70) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ShipTo),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="45%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName),20) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdDODetails" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                        OnRowDataBound="ActionHandler">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("INV_ITEM_MST.ITM_CODE"),15) %>'
                                                                        ToolTip='<%# Eval("INV_ITEM_MST.ITM_CODE") %>'>
                                                                    </asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),78) %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Size">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSize" runat="server" Text='<%# GetConstName(Eval("INV_ITEM_MST")) %>'
                                                                        ToolTip='<%# Eval("INV_UOM_MST.UOM_NAME") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:c}") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DelQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDel" runat="server" Text='<%# Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED", "{0:c}") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BalQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBal" runat="server" Text='<%#  Convert.ToString( Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED"))) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
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
                                </cc1:ExtGridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <%-- <asp:ValidationSummary ID="vsPage" ValidationGroup="contract" runat="server" />--%>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
