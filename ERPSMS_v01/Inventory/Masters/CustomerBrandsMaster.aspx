<%@ Page Title="<%$ Resources:Captions,Title_CustomerBrandsMaster %>" Language="C#" Theme="Classic"
         EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CustomerBrandsMaster.aspx.cs"
         Inherits="ERPSMS_v01.Inventory.Masters.CustomerBrandsMaster" MasterPageFile="~/ERPSMS_2.Master" 
         ValidateRequest="false"  %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id$='lblvalidCustomer']").hide();
            $("[id$='lblValidBrandCode']").hide();
            $("[id$='lblValidBrandName']").hide();
            $("[id$='lblValidStatus']").hide();
            $("[id$='lblvalidIGPLProductCode']").hide();
            var pageURL = window.document.URL;
            var virtualPath = $("[id$='hdfAbsolutePath']").val();
            var urlauto = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProducttest", urlauto + "?Type=ITM_NAME", "hdfProducttest", true, true, "PRODUCTMASTER");
            
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnCustomerBrandsListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnCustomerBrandsListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnCustomerBrandsDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnCustomerBrandsDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnCustomerBrandsListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnCustomerBrandsListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnCustomerBrandsDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnCustomerBrandsDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
        }

        function ValidateSearch() {
            var isValid = true;
            var msg = "";

            if ($("[id$='ddlSearchCustomer']").val() == '-1' && $("[id$='ddlSearchIGPLProductCode']").val() == '-1') {
                isValid = false;
                msg += '<%= GetLocalResourceObject("Err_Search") %>' + '<br/>';
            }
            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblvalidCustomer']").hide();
            $("[id$='lblValidBrandCode']").hide();
            $("[id$='lblValidBrandName']").hide();
            $("[id$='lblValidStatus']").hide();
            $("[id$='lblvalidIGPLProductCode']").hide();

            if ($("[id$='ddlCustomer']").val() == '-1') {
                $("[id$='lblvalidCustomer']").show();
                isValid = false;
                msg += '<%= GetLocalResourceObject("Err_Customer") %>' + '<br/>';
            }

            if ($("[id$='txtBrandCode']").val() == '') {
                $("[id$='lblValidBrandCode']").show();
                isValid = false;
                msg += '<%= GetLocalResourceObject("Err_BrandCode") %>' + '<br/>';
            }

            if ($("[id$='txtBrandName']").val() == '') {
                $("[id$='lblValidBrandName']").show();
                isValid = false;
                msg += '<%= GetLocalResourceObject("Err_BrandName") %>' + '<br/>';
            }

            if ($("[id$='ddlStatus']").val() == '-1') {
                $("[id$='lblValidStatus']").show();
                isValid = false;
                msg += '<%= GetLocalResourceObject("Err_Status") %>' + '<br/>';
            }

            if ($("[id$='ddlIGPLProductCode']").val() == '-1') {
                    $("[id$='lblvalidIGPLProductCode']").show();
                    isValid = false;
                    msg += '<%= GetLocalResourceObject("Err_IGPLProductCode") %>' + '<br/>';
            }

           
            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function EnableDisableProductAttributes() {
            if ($("[id$='ddlIGPLProductCode']").val() == '-1') {
                $("[id$=ProductAttributes]").hide(); 
            }
            else {
               $("[id$=ProductAttributes]").show(); 
            }
        }

        function EnableDisableSearch() {
            if ($("[id$='ddlSearchCustomer']").val() == '-1' && $("[id$='ddlSearchIGPLProductCode']").val() == '-1') {
                $("[id$='gridCustomerBrands']").hide();
            }
            else {
                $("[id$='gridCustomerBrands']").show();
            }
        }


    </script>
</asp:Content>

<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server"  />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="CustomerBrand" SkinID="btnInner-Save" TabIndex="17"
                                            CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>" OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler" TabIndex="18"
                                            ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler" TabIndex="19"
                                            ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnCustomerBrandsListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnCustomerBrandsListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="8" />
                        </span></li>
                        <li><span id="spnCustomerBrandsDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnCustomerBrandsDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="ACTIVATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom" id="searchCustomerBrands" runat="server">
                                <asp:Label ID="lblSearchCustomer" runat="server" Text="<%$ resources:Controls,Customer %>"
                                    AssociatedControlID="ddlSearchCustomer" />
                                <asp:DropDownList ID="ddlSearchCustomer" runat="server" TabIndex="1" onchange="EnableDisableSearch()" OnSelectedIndexChanged="ActionHandler" AutoPostBack ="true"    >
                                    
                                </asp:DropDownList>
                                 <asp:TextBox ID="txtProducttest" runat="server" MaxLength="100" TabIndex ="102" ></asp:TextBox>
                                <asp:HiddenField ID="hdfProducttest" runat="server" />

                                 <asp:Label ID="lblSearchIGPLProductCode" runat="server" Text="<%$ resources:Controls,IGPLProductCode %>"
                                    AssociatedControlID="ddlSearchIGPLProductCode"  />
                                <asp:DropDownList ID="ddlSearchIGPLProductCode" runat="server" TabIndex="2" onchange="EnableDisableSearch()" OnSelectedIndexChanged="ActionHandler" AutoPostBack ="true" >
                                    
                                </asp:DropDownList>
                                <!--<asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>" OnClientClick="return ValidateSearch();"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler" TabIndex="3" />-->
                            </div>
                            <div class="gridwrap" id="gridCustomerBrands" runat="server">
                                <asp:GridView runat="server" ID="grdCustomerBrandsMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                         <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Customer%>" SortExpression="<%$ resources:DataFieldRes,CustomerName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CustomerName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CustomerName)),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,BrandName%>" SortExpression="<%$ resources:DataFieldRes,BrandName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemBrandName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.BrandName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.BrandName)),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,IGPLProductCode%>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                            <ItemTemplate>

                                             <asp:Label ID="lblItemCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemCode)),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,IGPLProductName%>" SortExpression="<%$ resources:DataFieldRes,ItemName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemName)),100) %>' />

                                            </ItemTemplate>
                                            <ItemStyle Width="35%"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Status%>" SortExpression="<%$ resources:DataFieldRes,CIMActive %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemActive" runat="server"  />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td colspan ="2">
                                     <div class="div2col-S">
                                        <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Controls,Customer%>" AssociatedControlID="ddlCustomer" />
                                            <asp:DropDownList ID="ddlCustomer" runat="server"  TabIndex="10" />
                                            <asp:Label ID="lblvalidCustomer" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                            </div> 
                                    </td>
                                    </tr>
                                    <tr>
                                     <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrandCode" runat="server" Text="<%$ resources:Controls,BrandCode%>"
                                                AssociatedControlID="txtBrandCode" />
                                            <asp:TextBox ID="txtBrandCode" runat="server" CssClass="max500" MaxLength="200"
                                                 TabIndex="11" />
                                            <asp:Label ID="lblValidBrandCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrandName" runat="server" Text="<%$ resources:Controls,BrandName%>"
                                                AssociatedControlID="txtBrandName" />
                                            <asp:TextBox ID="txtBrandName" runat="server" CssClass="max500" MaxLength="200"
                                                 TabIndex="12" />
                                            <asp:Label ID="lblValidBrandName" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblIGPLProductCode" runat="server" Text="<%$ resources:Controls,IGPLProductCode%>" AssociatedControlID="ddlIGPLProductCode" />
                                            <asp:DropDownList ID="ddlIGPLProductCode" runat="server" onchange="EnableDisableProductAttributes()" OnSelectedIndexChanged="ActionHandler" TabIndex="13" AutoPostBack="true"  />
                                            <asp:Label ID="lblvalidIGPLProductCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                            
                                        </div>
                                    </td>
                                    <td>
                                       <div class="div2col-S">
                                            <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Controls,Status%>"
                                                AssociatedControlID="ddlStatus" />
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="14" CssClass="medium">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblValidStatus" runat="server" Text="*" CssClass="star" />
                                           <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblIGPLProductName" runat="server" Text="<%$ resources:Controls,IGPLProductName%>"
                                                AssociatedControlID="txtIGPLProductName" />
                                                <asp:TextBox ID="txtIGPLProductName" runat="server" CssClass="max500" MaxLength="200" Width="575"
                                                 TabIndex="15" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="ProductAttributes" runat ="server" >
                              <asp:Literal ID="ltlProductAttributes" runat ="server" ></asp:Literal>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="CustomerBrand" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>