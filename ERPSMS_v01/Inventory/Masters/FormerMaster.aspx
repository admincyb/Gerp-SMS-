<%@ Page Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="FormerMaster.aspx.cs" Inherits="ERPSMS_v01.Inventory.Masters.FormerMaster"
    Title="<%$ resources:Title_FormerMaster %>" Theme="ClassicExt" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() { //--------Date Pickers
            //            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            //          GrandScriptUtils.DatePickerCommon("txtLoadingDate");
            //        GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", true, true, "CUSTOMERLIST");
            //          ShowHideExpand();

            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendorID", true, true, "VENDOR");
            $("[id*=txtStdPrice]").ForceNumericOnly();
            $("[id*=txtMOQ]").ForceNumericOnly();
            $("[id*=txtLeadDays]").ForceNumericOnly();
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
        //---------Show and hide the Input fiels table
        function ShowHideManageResource(flag) {
            //If flag then Show additional Info
            if (flag) {
                $("[id$=tblManageRes]").show();
                $("[id$=imgManageResShow]").hide();
                $("[id$=imgManageResHide]").show();
            }
            else {
                $("[id$=tblManageRes]").hide();
                $("[id$=imgManageResShow]").show();
                $("[id$=imgManageResHide]").hide();
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
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }


        function ShowError() { //----------Error Maessages
            var msg = '<%=Resources.Messages.ReportError %>';
            var information = '<%=Resources.Messages.Information %>';
            GrandScriptUtils.ShowModal(msg, information);
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
        //        function AfterClose(containerID) {
        //            if (containerID == "#divWkfSubmit") {
        //                $("[id$=hdfIsSaveSubmit]").val("0");
        //            }
        //        }

        function PageViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlAlert]").hide();
            }
        }
        function SetSearchType() {
            $("[id$=SearchValue]").val('');
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                $("[id$=btnVendor]").click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <asp:HiddenField ID="hdfItemCategory" runat="server" />
            <asp:HiddenField ID="hdfType" runat="server" Value="1" />
            <asp:HiddenField ID="hdfMaterialPk" runat="server" />
            <asp:HiddenField ID="hdfLastModifiedDate" runat="server" Value="" />
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                    <%--Shows the navigation on top left side--%>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="11" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" ToolTip="<%$ resources:Controls,Delete %>"
                                            OnClientClick="return ShowDeleteConfirm(this);" OnClick="ActionHandler" TabIndex="11" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="11" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="Li1" runat="server">
                                        <asp:Button runat="server" ID="btnNew" CommandName="NEW" TabIndex="11" Text="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-New" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="11" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <%--  <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="58" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>--%>
                                    <%-- <li>
                                        <asp:Button runat="server" TabIndex="59" ID="btnPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:PrintBtnText %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-receipt" ToolTip="<%$resources:PrintBtnToolTip %>" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnListing" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnListing" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="LIST"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDetail" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" CommandName="DEFAULT" OnClick="ActionHandler"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnProductDetail" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnProductDetail" Text="<%$resources:ProductMapping %>"
                                CommandArgument="SEC_ActionPanel" CommandName="PRODUCTDETAIL" OnClick="ActionHandler"
                                CssClass="tab-active" OnClientClick="return ValidatePageNow('delivery');"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnStoreMapping" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnStoreMapping" Text="<%$resources:StoreMapping %>"
                                CommandArgument="SEC_ActionPanel" CommandName="STOREMAPPING" OnClick="ActionHandler"
                                CssClass="tab-active" OnClientClick="return ValidatePageNow('delivery');"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnVendorMapping" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnVendorMapping" Text="<%$resources:VendorMapping %>"
                                CommandArgument="SEC_ActionPanel" CommandName="VENDORMAPPING" OnClick="ActionHandler"
                                CssClass="tab-active" OnClientClick="return ValidatePageNow('delivery');"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="searchwrap" class="search-wrap-custom1">
                                <div id="divSearch">
                                    <span>
                                        <%=Resources.Controls.SearchBy%></span>
                                    <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                                        EnableViewState="false">
                                        <%--<asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                                        </asp:ListItem>--%>
                                        <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:Name%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="ITM_CODE" Text="<%$ Resources:Code%>">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                                    </asp:TextBox>
                                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" EnableViewState="false"
                                        CommandName="SEARCH" OnClick="ActionHandler" />
                                    <div style="float: right; display: none">
                                        <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="false" PageSize="<%$ resources:PageSize %>"
                                EmptyDataRowStyle-CssClass="emptytable">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            <asp:HiddenField ID="hdfItemPk" runat="server" Value='<%#Eval("ITM_PK") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:ItemCode %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemCodeGv" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ITM_CODE") ,30) %>'
                                                runat="server" ToolTip='<%# Eval("ITM_CODE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Name %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemNameGv" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ITM_NAME") ,70) %>'
                                                runat="server" ToolTip='<%# Eval("ITM_NAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUomNameGv" Text='<%# Eval("UOM_NAME") %>' runat="server" ToolTip='<%# Eval("UOM_NAME") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblItemCode" Text="<%$ resources:ItemCode %>" AssociatedControlID="txtItemCode"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtItemCode" Text="" TabIndex="1" MaxLength="30"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfItemCode" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtItemCode"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ItemCode %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblName" Text="<%$ resources:Name %>" AssociatedControlID="txtName"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtName" Text="" TabIndex="2" MaxLength="180" CssClass="input-full"></asp:TextBox>
                                                    <%--MaxLength="30"--%>
                                                    <asp:RequiredFieldValidator ID="vrfName" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtName" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Name %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblSize" Text="<%$ resources:Size %>" AssociatedControlID="ddlSize"></asp:Label>
                                                    <asp:DropDownList ID="ddlSize" runat="server" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfSize" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                        EnableClientScript="true" runat="server" ControlToValidate="ddlSize" Display="Dynamic"
                                                        Text="*" InitialValue="-1" ErrorMessage="<%$ resources:Err_Size %>"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblUOM" Text="<%$ resources:UOM %>" AssociatedControlID="ddlUOM"></asp:Label>
                                                    <asp:DropDownList ID="ddlUOM" runat="server" TabIndex="4">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfUOM" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                        EnableClientScript="true" runat="server" ControlToValidate="ddlUOM" Display="Dynamic"
                                                        Text="*" InitialValue="-1" ErrorMessage="<%$ resources:Err_UOM %>"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblOpeningStock" Text="<%$ resources:OpeningStock %>"
                                                        AssociatedControlID="txtOpeningStock"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtOpeningStock" Text="0" TabIndex="5"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfOpeningStock" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtOpeningStock"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_OpeningStock %>"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreOpeningStock" ValidationExpression="[0-9]+(\.[0-9][0-9]?)?"
                                                        CssClass="star" SetFocusOnError="true" ValidationGroup="delivery" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtOpeningStock" Display="Dynamic" Text="*"
                                                        ErrorMessage="<%$ resources:Err_ValidOpeningStock %>"></asp:RegularExpressionValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label1" Text="<%$resources:Controls,RequireInspection %>"
                                                        AssociatedControlID="chkRequireInspection"></asp:Label>
                                                    <asp:CheckBox ID="chkRequireInspection" runat="server" TabIndex="6" EnableViewState="False">
                                                    </asp:CheckBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblProductGroup" Text="<%$ resources:ProductGroup %>"
                                                        AssociatedControlID="ddlProductGroup"></asp:Label>
                                                    <asp:DropDownList ID="ddlProductGroup" runat="server" TabIndex="7" CssClass="input-w80-6per">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfProductGroup" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="ddlProductGroup"
                                                        Display="Dynamic" Text="*" InitialValue="-1" ErrorMessage="<%$ resources:Err_ProductGroup %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description %>"
                                                        AssociatedControlID="txtDescription"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDescription" Text="" TabIndex="8" TextMode="MultiLine"
                                                        onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" CssClass="input-full"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="gridwrap">
                            </div>
                            <%-- Additional Information --%>
                            <div class="clear">
                            </div>
                            <%-- Upload Documents --%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="divProductDetail" runat="server" class="content-wrapper">
                    <div>
                        <table class="inputTable">
                            <tr>
                                <td align="left" colspan="3">
                                    <h3 class="fontWGT-Nrml">
                                        <asp:Literal ID="Literal5" runat="server" Text="<%$ resources:Select_Item%>" /></h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div3col-S">
                                        <label>
                                            <asp:Literal ID="Literal1" runat="server" Text="<%$ resources:ItemCategory %>"></asp:Literal></label>
                                        <asp:DropDownList CssClass="medium" ID="ddlCategory" runat="server" TabIndex="1"
                                            CausesValidation="false" ValidationGroup="none" onchange="Page_BlockSubmit = false;"
                                            OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfCategory" SetFocusOnError="true" ValidationGroup="vlgProductPlus"
                                            EnableClientScript="true" runat="server" ControlToValidate="ddlCategory" Text="*"
                                            CssClass="star star-rel" Display="Dynamic" ErrorMessage="<%$ resources:Err_Category %>"
                                            InitialValue="-1"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div class="div3col-S">
                                        <label>
                                            <asp:Literal ID="ltrType" runat="server" Text="<%$ resources:Type %>"></asp:Literal></label>
                                        <asp:DropDownList CssClass="medium" ID="ddlType" runat="server" TabIndex="2" CausesValidation="false"
                                            ValidationGroup="none" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div3col-S">
                                        <label>
                                            <asp:Literal ID="ltrCategory" runat="server" Text="<%$ resources:Category %>"></asp:Literal></label>
                                        <asp:DropDownList CssClass="medium" ID="ddlCategories" runat="server" TabIndex="3"
                                            CausesValidation="false" ValidationGroup="none" OnSelectedIndexChanged="ActionHandler"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div3col-S">
                                        <label>
                                            <asp:Literal ID="ltrSurface" runat="server" Text="<%$ resources:Surface %>"></asp:Literal></label>
                                        <asp:DropDownList CssClass="medium" ID="ddlSurface" runat="server" TabIndex="4" CausesValidation="false"
                                            ValidationGroup="none" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div3col-S">
                                        <label>
                                            <asp:Literal ID="ltrClassification" runat="server" Text="<%$ resources:Classification %>"></asp:Literal></label>
                                        <asp:DropDownList CssClass="medium" ID="ddlClassification" runat="server" TabIndex="5"
                                            CausesValidation="false" ValidationGroup="none" OnSelectedIndexChanged="ActionHandler"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td>
                                    <div class="div3col-S">
                                        <label>
                                            <asp:Literal ID="ltrSize" runat="server" Text="<%$ resources:Size %>"></asp:Literal></label>
                                        <asp:DropDownList CssClass="medium" ID="ddlSizeTab3" runat="server" TabIndex="6"
                                            CausesValidation="false" ValidationGroup="none" OnSelectedIndexChanged="ActionHandler"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                            </tr>
                            <td>
                                <div class="div3col-S">
                                    <label>
                                        <asp:Literal ID="LtrShade" runat="server" Text="<%$ resources:Shade %>"></asp:Literal></label>
                                    <asp:DropDownList CssClass="medium" ID="Ddlshade" runat="server" TabIndex="6" CausesValidation="false"
                                        ValidationGroup="none" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <tr>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="divcol-S">
                                        <label style="width: 114px!important;">
                                            <asp:Literal ID="Literal2" runat="server" Text="<%$ resources:Item %>"></asp:Literal></label>
                                        <asp:HiddenField ID="hfdPlmPk" runat="server" />
                                        <asp:DropDownList ID="ddlItem" runat="server" TabIndex="7" ValidationGroup="vlgProductPlus"
                                            CssClass="input-w77-7per">
                                        </asp:DropDownList>
                                        <%--Width="81.5%"--%>
                                        <asp:RequiredFieldValidator ID="vrfItem" SetFocusOnError="true" ValidationGroup="vlgProductPlus"
                                            EnableClientScript="true" runat="server" ControlToValidate="ddlItem" Text="*"
                                            CssClass="star star-rel" Display="Dynamic" ErrorMessage="<%$ resources:Err_Item %>"
                                            InitialValue="-1"></asp:RequiredFieldValidator>
                                        <asp:ImageButton ID="imbProductPlus" runat="server" ValidationGroup="vlgProductPlus"
                                            SkinID="imbaddnew" Width="18px" OnClick="ActionHandler" CommandName="ADD_ACTION"
                                            TabIndex="9" />
                                        <%--  <asp:ImageButton ID="imbProductCancel" runat="server" SkinID="btnclose" Width="18px"
                                        OnClick="ActionHandler" CommandName="CANCEL_ACTION" TabIndex="8" />--%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <%--<asp:ImageButton ID="imbProductPlus" runat="server" ValidationGroup="vlgProductPlus"
                                        SkinID="imbaddnew" Width="18px" OnClick="ActionHandler" CommandName="ADD_ACTION"
                                        TabIndex="7" 
                                         />--%>
                                    <%-- OnClientClick="javascript:return ValidateNow('vlgProductPlus')"--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="grdTable">
                        <asp:GridView ID="grdLineProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                            AllowPaging="false" CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable"
                            EmptyDataRowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Item %>" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl2" Text='<%# Eval("ITM_TEXT")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfProductPK" Value='<%# Eval("ITM_PK")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Action %>" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:ImageButton Height="16px" Width="16px" ID="imbDeletetLineProduct" runat="server"
                                            SkinID="imbdeletegrid" OnClick="ActionHandler" CommandName="DELETE_ACTION" CssClass="_delete"
                                            ToolTip="<%$ resources:Controls,Delete %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hdfFormerItemMapping" runat="server" />
                    </div>
                    <div class="clear">
                    </div>
                    <div style="text-align: right;">
                        <asp:Button runat="server" ID="btnClearMapping" Text="<%$resources:Controls,ClearAll %>"
                            ToolTip="<%$resources:Controls,ClearAll %>" OnClick="ActionHandler" CommandName="CLEARMAPPING"
                            TabIndex="10" />
                    </div>
                </div>
                <div id="divStoreMapping" runat="server" class="treeview max-200">
                    <%--class="content-wrapper"--%>
                    <asp:Label runat="server"></asp:Label>
                    <asp:TreeView ID="trvStores" runat="server" ShowCheckBoxes="All" ShowLines="true"
                        ExpandDepth="0" InitialExpandDepth="2" CssClass="margn-lft30">
                    </asp:TreeView>
                </div>
                <div id="divVendorMapping" runat="server" class="content-wrapper">
                    <div class="detail-poi-co2">
                        <div class="divseccol-S">
                            <asp:Label ID="lblMaterialCodeH" runat="server" Text="<%$ resources:Controls,MaterialCode %>"
                                AssociatedControlID="lblMaterialCode"></asp:Label>
                            <asp:Label ID="lblMaterialCode" runat="server" Text="" CssClass="height-auto break-word"></asp:Label>
                        </div>
                        <div class="divfirstcol-S">
                            <asp:Label ID="lblMaterialNameH" runat="server" Text="<%$ resources:Controls,MaterialName %>"
                                AssociatedControlID="lblMaterialName"></asp:Label>
                            <asp:Label ID="lblMaterialName" runat="server" Text="" CssClass="height-auto break-word"></asp:Label>
                        </div>
                        <%-- <div class="">
                            <asp:Label ID="lblMaterialCategoryNameH" runat="server" Text="<%$ resources:Controls,MaterialCategoryName %>"
                                AssociatedControlID="lblMaterialCategoryName"></asp:Label>
                            <asp:Label ID="lblMaterialCategoryName" runat="server" Text="" CssClass="height-auto break-word"></asp:Label>
                        </div>--%>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblVendorName" runat="server" Text="<%$resources:Controls,VendorName %>"
                                        AssociatedControlID="txtVendor"></asp:Label>
                                    <asp:TextBox ID="txtVendor" runat="server" MaxLength="100" Width="63%"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvVendorName" runat="server" ControlToValidate="txtVendor"
                                        EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                        CssClass="star" ValidationGroup="ValVendor" Text="*" ErrorMessage="<%$ resources:Err_VendorName%>"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdfVendorID" runat="server" />
                                    <asp:HiddenField ID="hdfMappingSlNo" runat="server" Value="0" />
                                    <asp:Button ID="btnVendor" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTED"
                                        Style="display: none" EnableTheming="false" />
                                    <%-- <asp:DropDownList ID="ddlVendorName" runat="server" Width="65%" onchange="javascript:FillCurrencyByVendor();">
                                    </asp:DropDownList>--%>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblStdPrice" runat="server" Text="<%$resources:Controls,StdPrice %>"
                                        AssociatedControlID="txtStdPrice"></asp:Label>
                                    <asp:TextBox ID="txtStdPrice" runat="server" CssClass="numeric input-small" Text=""
                                        MaxLength="7"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvStdPrice" runat="server" ControlToValidate="txtStdPrice"
                                        CssClass="star" ValidationGroup="ValVendor" Text="*" ErrorMessage="<%$ resources:Err_StdPrice%>"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblCurrency" runat="server" Text="<%$resources:Controls,Currency %>"
                                        AssociatedControlID="ddlCurrency" CssClass="lbl-23-5perc"></asp:Label>
                                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="input-small" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" ControlToValidate="ddlCurrency"
                                        CssClass="star" ValidationGroup="ValVendor" Text="*" ErrorMessage="<%$ resources:Err_Currency%>"
                                        InitialValue=""></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblLeadDays" runat="server" Text="<%$resources:Controls,LeadDays %>"
                                        AssociatedControlID="txtLeadDays"></asp:Label>
                                    <asp:TextBox ID="txtLeadDays" runat="server" Text="0" CssClass="numeric input-small"
                                        MaxLength="5" onkeypress="javascript:MakeNumeric(event);" onchange="GrandScriptUtils.SetZeroDefault(this)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLeadDays" runat="server" ControlToValidate="txtLeadDays"
                                        CssClass="star" ValidationGroup="ValVendor" Text="*" ErrorMessage="<%$ resources:Err_LeadDays%>"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblActive" runat="server" Text="<%$resources:Controls,Active %>" AssociatedControlID="chkActive"></asp:Label>
                                    <asp:CheckBox ID="chkActive" runat="server" />
                                    <asp:ImageButton ID="imbAddNew" runat="server" SkinID="imbaddnew" OnClick="ActionHandler"
                                        ValidationGroup="ValVendor" CommandName="ADDVENDORDETAILS" OnClientClick="javascript:ValidatePageNow('ValVendor')" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="Label2" runat="server" Text="<%$resources:Controls,msName %>" AssociatedControlID="txtMaterialName"></asp:Label>
                                    <asp:TextBox ID="txtMaterialName" runat="server" Width="61%" Text=""></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblMOQ" runat="server" Text="<%$resources:Controls,MOQ %>" AssociatedControlID="txtMOQ"></asp:Label>
                                    <asp:TextBox ID="txtMOQ" runat="server" CssClass="numeric input-small" Text="0.00"
                                        MaxLength="7" onchange="GrandScriptUtils.SetZeroDefault(this, 2)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMOQ" runat="server" ControlToValidate="txtMOQ"
                                        CssClass="star" ValidationGroup="ValVendor" Text="*" ErrorMessage="<%$ resources:Err_MOQ%>"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblUnit" runat="server" Text="<%$resources:Controls,Unit %>" AssociatedControlID="ddlUnit"
                                        CssClass="lbl-19-4perc"></asp:Label>
                                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="lbl-20-5perc">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvUnit" runat="server" ControlToValidate="ddlUnit"
                                        CssClass="star" ValidationGroup="ValVendor" Text="*" ErrorMessage="<%$ resources:Err_Unit%>"
                                        InitialValue="-1"></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="grdTable">
                        <asp:GridView ID="grdVendorMaterialDetails" runat="server" AutoGenerateColumns="False"
                            Width="100%" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                            ShowFooter="false" EmptyDataRowStyle-HorizontalAlign="Center" CssClass="grdTable">
                            <%--OnRowDataBound="ActionHandler"--%>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,VendorName%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("ITV_VENDORNAME") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfSlNo" runat="server" Value='<%# Eval("ITV_SL_NO") %>' />
                                        <asp:HiddenField ID="hdfPk" runat="server" Value='<%# Eval("ITV_PK") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,msName%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaterialServName" runat="server" Text='<%#Eval("ITV_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,StdPrice%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStdPrice" runat="server" Text='<%#Eval("ITV_PRICE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Currency%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("MaterialCurrencyText") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfCurrency" runat="server" Value='<%# Eval("ITV_CURRENCY") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,MOQ%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMOQ" runat="server" Text='<%#Eval("ITV_MOQ") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Unit%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnit" runat="server" Text='<%#Eval("UOMText") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfUnit" runat="server" Value='<%# Eval("ITV_MOQ_UOM") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,LeadDays%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeadDays" runat="server" Text='<%#Eval("ITV_LEAD_TIME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Active%>" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActive" runat="server" Text='<%# (Eval("ITV_ACTIVE").ToString() == "1") ?
                                               "Active"  : "In active" %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="imbEditDetails" SkinID="imbeditgrid" EnableViewState="false"
                                            CommandName="EDITVENDORDETAILS" OnClick="ActionHandler" />
                                        <asp:ImageButton runat="server" ID="imbDeleteDetails" SkinID="imbdeletegrid" EnableViewState="false"
                                            CommandName="REMOVEVENDORDETAILS" OnClientClick="return ShowDeleteConfirm(this);"
                                            OnClick="ActionHandler" />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <%--<div class="grdTable" style="overflow-x: auto">
                            <table rules="all" id="grdVendorMaterialDetails" grandtype="GrandGrid" paging="false"
                                editfunction="GridMaterialAction" editable="true" width="100%" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="ITV_ACTIVE" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_SL_NO" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_VENDOR" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_VENDORNAME" width="22%" align="left">
                                            <%=Resources.Controls.VendorName%>*
                                        </th>
                                        <th fieldmap="ITV_NAME" width="18%" align="left">
                                            <%=Resources.Controls.msName%>*
                                        </th>
                                        <th fieldmap="ITV_PRICE" width="9%" align="right">
                                            <%=Resources.Controls.StdPrice%>
                                        </th>
                                        <th fieldmap="ITV_CURRENCY" isvisible="false" align="left">
                                        </th>
                                        <th fieldmap="MaterialCurrencyText" width="8%" align="left">
                                            <%=Resources.Controls.Currency%>*
                                        </th>
                                        <th fieldmap="ITV_MOQ" width="8%" align="right">
                                            <%=Resources.Controls.MOQ%>
                                        </th>
                                        <th fieldmap="ITV_MOQ_UOM" isvisible="false">
                                        </th>
                                        <th fieldmap="UOMText" width="8%" align="left">
                                            <%=Resources.Controls.Unit%>*
                                        </th>                                       
                                        <th fieldmap="ITV_LEAD_TIME" width="11%" align="right">
                                            <%=Resources.Controls.LeadDays%>
                                        </th>
                                        <th fieldmap="ITV_ACTIVE_TEXT" width="7%" align="right">
                                            <%=Resources.Controls.Active%>*
                                        </th>
                                        <th fieldmap="ITV_DISC_PERC" width="3%">
                                        </th>
                                        <th fieldmap="ITV_TAX_PERC" width="3%">
                                        </th>
                                        <th type="Template" width="8%" align="left">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbEditVendor" ToolTip="<%$ resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridVendorHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                                <asp:ImageButton runat="server" ID="imbDeleteVendor" ToolTip="<%$ resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridVendorHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>--%>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="delivery" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="vsVendor" ValidationGroup="ValVendor" runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hdfVendorMapping" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <%--  <asp:PostBackTrigger ControlID="btnAddItem" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
