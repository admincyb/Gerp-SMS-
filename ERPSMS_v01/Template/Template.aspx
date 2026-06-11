<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="Template.aspx.cs" Inherits="gERPDiscrete.UI.Template" Theme="Classic" %>

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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
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
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="54" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="55" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="57" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                        <li><span id="spnSOListing" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="LIST"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnShippingPlan" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkShippingPlan" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" CommandName="DEFAULT" OnClick="ActionHandler"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                     <%--   Listing Grid--%>
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
                                                    <%-- <asp:Label runat="server" ID="lblPlanNo" Text="<%$ resources:PlanNo %>" AssociatedControlID="lblShippingPlanNo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblShippingPlanNo" Text=""></asp:Label>
                                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                    <asp:HiddenField ID="AST_CODE" runat="server" />
                                                    <asp:HiddenField ID="hdfShippingPlanNo" runat="server" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblShiptoPort" Text="<%$ resources:Shiptoport %>" AssociatedControlID="txtShipPort"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShipPort" Text="" TabIndex="36"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType %>"
                                                        AssociatedControlID="ddlContainerType"></asp:Label>
                                                    <asp:DropDownList ID="ddlContainerType" runat="server" TabIndex="38">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfContainerType" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="ddlContainerType"
                                                        Display="Dynamic" InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_ContainerType %>"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblDelivery" Text="<%$ resources:Customer %>" AssociatedControlID="lblDeliveryTo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDeliveryTo" Text=""></asp:Label>
                                                    <asp:HiddenField ID="hdnCustomerID" runat="server" Value="0" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblShippingAgent" Text="<%$ resources:ShippingAgent %>"
                                                        AssociatedControlID="ddlAgent"></asp:Label>
                                                    <asp:DropDownList ID="ddlAgent" runat="server" TabIndex="40">
                                                    </asp:DropDownList>--%>
                                                    <div id="divPopUpTask" style="display: none">
                                                     
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <%--<div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label2" Text="<%$ resources:GeneratedOn %>" AssociatedControlID="txtGeneratedOn"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtGeneratedOn" Text="" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="35"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfGeneratedOn" runat="server" Value="" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtGeneratedOn" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="delivery"
                                                            runat="server" ControlToValidate="txtGeneratedOn" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label3" Text="<%$ resources:ETD %>" AssociatedControlID="txtETD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETD" Text="" CssClass="Uidate-picker" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="37"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETD" runat="server" Value="" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblETA" runat="server" Text="<%$ resources:ETA %>" AssociatedControlID="txtETA"></asp:Label>
                                                    <asp:TextBox ID="txtETA" runat="server" CssClass="date-picker" MaxLength="11" onkeydown="return CheckKey(event)"
                                                        onpaste="return false;" TabIndex="39"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETA" runat="server" Value="" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfETD" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtETD" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_ETDDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreETDDate" CssClass="star" ValidationGroup="delivery"
                                                            runat="server" ControlToValidate="txtETD" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_ETDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:Status %>" AssociatedControlID="lblSPStatus"></asp:Label>
                                                    <asp:Label runat="server" ID="lblSPStatus" Text=""></asp:Label>
                                                    <asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department %>"
                                                        AssociatedControlID="lblPageDeptText" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblPageDeptText" runat="server" Visible="false"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblLoadingDate" Text="<%$ resources:LoadingDate %>"
                                                        AssociatedControlID="txtLoadingDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtLoadingDate" Text="" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="41"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfLoadingDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtLoadingDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_LoadingDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreLoadingDate" CssClass="star" ValidationGroup="delivery"
                                                        runat="server" ControlToValidate="txtLoadingDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_LoadingDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>--%>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="gridwrap">
                                <%-- <asp:GridView ID="grdShippingList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="25" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="42">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='' OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" ToolTip=''></asp:LinkButton>
                                                <asp:HiddenField ID="hdfSONumber" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderHdrPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderDtlPK" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProductCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIGPLCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfIGPLCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfBrandCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="16%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfUOM" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:TotalCTNs %>"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanNow %>" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalPlanNow" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CBM %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCBM" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdnPackingPcs" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCBM" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CTNQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCTNQty" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdnTotalPcs" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCTN" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>--%>
                            </div>
                            <%-- Additional Information --%>
                            <div class="clear">
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%--  <%= GetGlobalResourceObject("Controls", "ManageResource").ToString()%></h1>--%>
                                    <div class="button-wrap-right ">
                                        <asp:ImageButton runat="server" ID="imgManageResShow" OnClientClick="javascript:return ShowHideManageResource(1);"
                                            ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                            TabIndex="65" />
                                        <asp:ImageButton runat="server" ID="imgManageResHide" OnClientClick="javascript:return ShowHideManageResource();"
                                            ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                            TabIndex="66" /></div>
                                    <div class="clear">
                                    </div>
                                    <div class="fields-group">
                                        <table class="table-devide" id="tblManageRes">
                                            <tr>
                                                <td>
                                                    <div class="div2col-S">
                                                        <%--    <asp:Label ID="lblBookingRefNo" runat="server" Text="<%$ resources:BookingRefNo %>"
                                                        AssociatedControlID="txtBookingRefNo"></asp:Label>
                                                    <asp:TextBox ID="txtBookingRefNo" runat="server" CssClass="large" MaxLength="100"
                                                        TabIndex="42"></asp:TextBox>--%>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="div2col-S">
                                                        <%--      <asp:Label ID="lblFeederVessel" runat="server" Text="<%$ resources:FeederVessel %>"
                                                        AssociatedControlID="txtFeederVessel"></asp:Label>
                                                    <asp:TextBox ID="txtFeederVessel" runat="server" CssClass="large" MaxLength="200"
                                                        TabIndex="43"></asp:TextBox>--%>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="div2col-S">
                                                        <%--  <asp:Label ID="lblClosingDate" runat="server" Text="<%$ resources:ClosingDate %>"
                                                        AssociatedControlID="txtClosingDate"></asp:Label>
                                                    <asp:TextBox ID="txtClosingDate" runat="server" CssClass="date-picker" MaxLength="11"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="44"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblClosingTime" Text="<%$ resources:ClosingTime %>"
                                                        AssociatedControlID="txtClosingTime"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtClosingTime" MaxLength="12" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="46"></asp:TextBox>--%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="div2col-S">
                                                        <%--  <asp:Label ID="lblMotherVessel" runat="server" Text="<%$ resources:MotherVessel %>"
                                                        AssociatedControlID="txtMotherVessel"></asp:Label>
                                                    <asp:TextBox ID="txtMotherVessel" runat="server" CssClass="large" MaxLength="200"
                                                        TabIndex="45"></asp:TextBox>--%>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
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
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="delivery" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--  <asp:PostBackTrigger ControlID="btnAddItem" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
