<%@ Page Title="<%$ Resources:Captions,Title_SaleOrderListDO %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic"
    AutoEventWireup="true" CodeBehind="SaleOrderListDO.aspx.cs" Inherits="ERPSMS_v01.Sales.SaleOrderListDO" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false, false);
            //         GrandScriptUtils.AddDateRange("txtActFromDate", "hdfActFromDate", "txtActToDate", "hdfActToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url, "hdfSoPK", true, true, "SONUMBER");
            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();

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
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnPickForInvoice" CommandName="PICKFORINVOICING"
                                            TabIndex="14" Text="<%$resources:PickSoForDO %>" OnClick="ActionHandler" ToolTip="Pick PO For Invoiving"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                TabIndex="1" CssClass="tab-active" OnClick="ActionHandler" CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="8" OnClick="ActionHandler" CommandName="DELIVERYORDER" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
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
                            <%--  <div class="clear">
                            </div>--%>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="medium" MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="10" CssClass="date-picker"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="Label1" runat="server" Text="Status" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="medium">
                                                <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$resources:SONo %>" AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" CssClass="medium" MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSoPK" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="10" CssClass="date-picker" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="14" CommandName="SEARCH"
                                                SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="15"
                                                OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--  <div class="clear">
                            </div>--%>
                            <div class="gridwrap hierarchical-wrap">
                                <cc1:ExtGridView runat="server" ID="grdSoList" AutoGenerateColumns="False" Width="100%"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="24" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval(Resources.DataTableRes.CustomerMst + "." + Resources.DataFieldRes.CustomerName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.CustomerMst+"."+Resources.DataFieldRes.CustomerName) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ship To Port">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipToPort" runat="server" Text='<%# Eval("SOH_TO_PORT") %>' ToolTip='<%# Eval("SOH_TO_PORT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ship By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# Eval("ADM_CONST_MST"+"."+"CON_NAME") %>' ToolTip='<%# Eval("ADM_CONST_MST"+"."+"CON_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CreatedBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval(Resources.DataTableRes.WkfUserMst1 +"." + Resources.DataTableRes.EmployeeMst +"." + Resources.DataFieldRes.EmployeeName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.WkfUserMst1 +"." + Resources.DataTableRes.EmployeeMst +"." + Resources.DataFieldRes.EmployeeName) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ApprovedBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApprovedBy" runat="server" Text='<%# Eval(Resources.DataTableRes.WkfUserMst +"." + Resources.DataTableRes.EmployeeMst +"." + Resources.DataFieldRes.EmployeeName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.WkfUserMst +"." + Resources.DataTableRes.EmployeeMst +"." + Resources.DataFieldRes.EmployeeName) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="100%">
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
                                                            <asp:TemplateField HeaderText="IGPL Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIGPLCode" runat="server" Text='<%# Eval("INV_ITEM_MST"+"."+"ITM_CODE") %>'
                                                                        ToolTip='<%# Eval("INV_ITEM_MST"+"."+"ITM_CODE") %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Brand Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"CIM_BRAND_NAME") %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"CIM_BRAND_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="24%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Size">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSize" runat="server" Text='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"ADM_CONST_MST19"+"."+"CON_NAME") %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"ADM_CONST_MST19"+"."+"CON_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Colour">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblColour" runat="server" Text='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"ADM_CONST_MST17"+"."+"CON_NAME") %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"ADM_CONST_MST17"+"."+"CON_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Texture">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTexture" runat="server" Text='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"ADM_CONST_MST"+"."+"CON_NAME") %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP"+"."+"ADM_CONST_MST"+"."+"CON_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>" SortExpression="<%$ resources:DataFieldRes,SodQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQty, "{0:N}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodQty, "{0:N}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Del. Qty" SortExpression="SOD_QTY_DISPATCHED">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDQty" runat="server" Text='<%# Eval("SOD_QTY_DISPATCHED", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("SOD_QTY_DISPATCHED", "{0:N}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Bal. Qty" SortExpression="SOD_BAL_TO_DISPATCH">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBQty" runat="server" Text='<%# Eval("SOD_BAL_TO_DISPATCH", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("SOD_BAL_TO_DISPATCH", "{0:N}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dely. Date" SortExpression="<%$ resources:DataFieldRes,SodDesiredDeliveryDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesiredDeliveryDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" />
                                                                <HeaderStyle  HorizontalAlign="Left"/>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ resources:Comments %>" SortExpression="<%$ resources:DataFieldRes,SodComments %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComments" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodComments) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodComments) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField></asp:TemplateField>
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
