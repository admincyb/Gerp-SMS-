<%@ Page Title="<%$ Resources:Captions,Title_OrderTracker %>" Language="C#" MasterPageFile="~/ERPSMS_Dashboard.Master"
    AutoEventWireup="true" CodeBehind="OrderTracker.aspx.cs" EnableEventValidation="true"
    Inherits="CustomerPortal.Dashboard.OrderTracker" Theme="Classic" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");


        function InitComponents() {
            //GrandScriptUtils.AddDateRangeCommon("txtFromDateOrder", "hdfFromDateOrder", "txtToDateOrder", "hdfToDateOrder", false, false); 
            showProductName();
            ShowHideExpand();
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


            if ($("[id$=grdCustomersOrders]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
        }



        function showProductName() {
            if ($("[id$=lblProductName]").html() == "") {
                $("#divProductName").hide();
            }
            else {
                $("#divProductName").show();
            }
        }

        function Disableautocomplete() {
            if ($("[id$=txtCustomerSearch]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomerSearch]"), $("[id$=hdfCustomerSearch]"));
            }
        }
        function DisableAuto(extender, hfield) {
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function ShowHideCustomersOrders(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divCustomersOrdersGid").show();
                $("[id$=imgShowCustomersOrders]").hide();
                $("[id$=imgHideCustomersOrders]").show();
            }
            else {

                $("#divCustomersOrdersGid").hide();
                $("[id$=imgShowCustomersOrders]").show();
                $("[id$=imgHideCustomersOrders]").hide();
            }
            return false;
        }

        function ShowHideAllocated(flag) {
            //If flag then Show Labours

            if (flag) {

                $("#divAllocatedGid").show();
                //$("[id$=imbAllocatedShow]").hide();
                //$("[id$=imbAllocatedHide]").show();
            }
            else {

                $("#divAllocatedGid").hide();
                //$("[id$=imbAllocatedShow]").show();
                //$("[id$=imbAllocatedHide]").hide();
            }
            return false;
        }

        function ShowHideInvoice(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divInvoiceGrid").show();
                $("[id$=imbInvoiceShow]").hide();
                $("[id$=imbInvoiceHide]").show();
            }
            else {

                $("#divInvoiceGrid").hide();
                $("[id$=imbInvoiceShow]").show();
                $("[id$=imbInvoiceHide]").hide();
            }
            return false;
        }

        function ShowHideStatement(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divStatementGrid").show();
                $("[id$=imbStatementShow]").hide();
                $("[id$=imbStatementHide]").show();
            }
            else {

                $("#divStatementGrid").hide();
                $("[id$=imbStatementShow]").show();
                $("[id$=imbStatementHide]").hide();
            }
            return false;
        }
        function ShowHidePTerm(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divPTermsGrid").show();
                $("[id$=imbPTermsShow]").hide();
                $("[id$=imbPTermsHide]").show();
            }
            else {

                $("#divPTermsGrid").hide();
                $("[id$=imbPTermsShow]").show();
                $("[id$=imbPTermsHide]").hide();
            }
            return false;
        }

        function ShowHideQUESTIONNAIRE(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divQUESTIONNAIRE").show();
                $("[id$=imbQUESTIONNAIREShow]").hide();
                $("[id$=imbQUESTIONNAIREHide]").show();
            }
            else {

                $("#divQUESTIONNAIRE").hide();
                $("[id$=imbQUESTIONNAIREShow]").show();
                $("[id$=imbQUESTIONNAIREHide]").hide();
            }
            return false;
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
        // for GO with search key
        function Search(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 13) {
                //                $('[id$=btnbinCardGet]').click();
                //                return false;
            }
        }

        function ShowError() {
            var msg = '<ul><li><%= GetLocalResourceObject("Err_Customer") %></li></ul>';
            $("[id$=litErrorMsg]").show();
            $("[id$=litErrorMsg]").html(msg);
            ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
        }

    </script>
    <script type="text/javascript">

        var defaultText = "Enter your text here";

        function WaterMark(txt, evt) {

            if (txt.value.length == 0 && evt.type == "blur") {

                txt.style.color = "gray";

                txt.value = defaultText;

            }

            if (txt.value == defaultText && evt.type == "focus") {

                txt.style.color = "black";

                txt.value = "";

            }

        }

    </script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlDashboard" runat="server">
        <ContentTemplate>
            <div class="content-wrapper">
                <div class="dashboard-wrapper">
                    <div class="dash100">
                        <div class="dashinner100">
                            <div class="dash-subhead">
                                <h1>
                                    <asp:Literal ID="ltl" runat="server" Text="<%$ resources:QUESTIONNAIRE %>"></asp:Literal>
                                </h1>
                                <div class="controls">
                                    <asp:ImageButton runat="server" ID="imbQUESTIONNAIREShow" OnClientClick="javascript:return ShowHideQUESTIONNAIRE(1);"
                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:QueriesShow %>" />
                                    <asp:ImageButton runat="server" ID="imbQUESTIONNAIREHide" OnClientClick="javascript:return ShowHideQUESTIONNAIRE();"
                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                        ToolTip="<%$ resources:QueriesHide %>" />
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents1" id="divQUESTIONNAIRE">
                                <asp:DropDownList ID="ddlQuestionnaire" runat="server" OnSelectedIndexChanged="ActionHandler"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Label runat="server" ID="lblTransDetails" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="trv-wrapper">
                        <table>
                            <tr>
                                <td class="trv-td">
                                    <div class="treeview" style="min-height: 325PX">
                                        <asp:TreeView ID="trvCustomers" runat="server" ShowLines="true" OnTreeNodePopulate="PopulateParentsChild"
                                            TabIndex="6" OnSelectedNodeChanged="ActionHandler_onSelect" EnableClientScript="true">
                                            <SelectedNodeStyle BackColor="#C6DEFF" ForeColor="Black" BorderStyle="Dotted" BorderWidth="1px"
                                                BorderColor="#000000" />
                                        </asp:TreeView>
                                    </div>
                                </td>
                                <td>
                                    <div class="content-wrapper">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div class="search-colapse-b">
                                                        <table>
                                                            <tr>
                                                                <td style="width: auto">
                                                                    <h1>
                                                                        <asp:Label ID="lblCustomerOrderHdr" AssociatedControlID="imgShowCustomersOrders"
                                                                            runat="server" Text="<%$ resources:CustomerOrderHdr %>"></asp:Label>
                                                                    </h1>
                                                                </td>
                                                                <td style="width: auto">
                                                                    <asp:ImageButton runat="server" ID="imgShowCustomersOrders" OnClientClick="javascript:return ShowHideCustomersOrders(1);"
                                                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:CustomerOrderHdrShow %>" />
                                                                    <asp:ImageButton runat="server" ID="imgHideCustomersOrders" OnClientClick="javascript:return ShowHideCustomersOrders();"
                                                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                                        ToolTip="<%$ resources:CustomerOrderHdrHide %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="divCustomersOrdersGid">
                                            <div class="gridwrap" id="PackingGrid">
                                                <asp:GridView runat="server" ID="grdPacking" Width="100%" ShowFooter="true" AllowPaging="true"
                                                    OnPageIndexChanging="grdPacking_OnPaging" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                                    OnRowDataBound="grdPacking_RowDataBound">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Product %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProduct" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProductCode) %>'
                                                                    EnableTheming="false" Style="display: none;"></asp:Label>
                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ProductText)),42) %>'
                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ProductText)) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="52%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Size %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSize" runat="server" Text='<%# Eval(Resources.DataFieldRes.Size)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.Size)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Reqd %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReqd" runat="server" Text='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>'
                                                                    ToolTip='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="13%" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>" ToolTip="<%$ resources:Total %>">
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrQty" runat="server" Text="Qty." ToolTip="<%$ resources:OrderQty %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQty,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.OrderQty,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQtyTotal,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.OrderQtyTotal,"{0:n0}")%>'>
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrBalProduce" runat="server" Text="Bal.Produce" ToolTip="<%$ resources:BalanceToProduce %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalanceToProduce" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToProduce,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToProduce,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalBalanceToProduce" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToProduceTotal)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToProduceTotal)%>'>
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrBalPlan" runat="server" Text="Bal.Plan" ToolTip="<%$ resources:BalanceToPlan %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalanceToPlan" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToPlan,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToPlan,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalBalanceToPlan" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToPlanTotal)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToPlanTotal)%>'>
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrBalDispatch" runat="server" Text="Bal.Dispatch" ToolTip="<%$ resources:BalanceToDispatch %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalanceToDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToDispatch,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToDispatch,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalBalanceToDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToDispatchTotal)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToDispatchTotal)%>'>
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrPacked" runat="server" Text="Packed" ToolTip="Packed"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPacked" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyPAcked,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SodQtyPAcked,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalPacked" runat="server">
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrDispatch" runat="server" Text="Dispatched" ToolTip="Dispatch"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyDispatched,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SodQtyDispatched,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalToDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyDispatchedTotal)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SodQtyDispatchedTotal)%>'>
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblhdrAllocated" runat="server" Text="Alloc/Prd" ToolTip="Allocated"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyAllocated,"{0:n0}")%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SodQtyAllocated,"{0:n0}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblAllocatedTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyAllocatedTotal)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SodQtyAllocatedTotal)%>'>
                                                                </asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <%--                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblhdrPlanned" runat="server" Text="Planned" ToolTip="Planned"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlanned" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyPlanned,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblPlannedTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyPlannedTotal)%>'>
                                                </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                             <HeaderStyle CssClass ="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblhdrBlaToplan" runat="server" Text="Bal.Plan" ToolTip="Bal.Plan"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBlaToplan" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodBalToPlan,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblBlaToplanTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodBalToPlanTotal)%>'>
                                                </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                             <HeaderStyle CssClass ="amount-numeric" />
                                        </asp:TemplateField>--%>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div id="divProductName">
                                                <h5>
                                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ resources:ShippingPlan %>"></asp:Label></h5>
                                            </div>
                                            <%--Shipping Plan--%>
                                            <div class="gridwrap hierarchical-wrap">
                                                <cc1:ExtGridView runat="server" ID="grdCustomersOrders" AutoGenerateColumns="False"
                                                    Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                    ShowFooter="true" OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="17" GroupName="SelectOne"
                                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                                    AutoPostBack="true" />
                                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SPPk) %>' EnableTheming="false"
                                                                    Style="display: none" />
                                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hdfShippingPlanID" Value='<%# Eval(Resources.DataFieldRes.SPPk) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="3%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:PlanNo %>" SortExpression="SNH_NO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContainer" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPNO)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPNO)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GeneratedOn %>" SortExpression="SNH_DATE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransportCo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPDate, Resources.Constants.DateFormatGrid) %>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Cartons %>" SortExpression="SNH_CTN_QTY">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCarrier" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPCartonsQty)%>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPCartonsQty)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ETD %>" SortExpression="SNH_ETD">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPETD, Resources.Constants.DateFormatGrid) %>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPETD, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="SNH_STATUS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatusText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPStatusText),30) %>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPStatusText) %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SPStatus) %>' />
                                                                <asp:HiddenField runat="server" ID="hdfTrxStatus" Value='<%# Eval(Resources.DataFieldRes.SPTrxStatus) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="30%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imbSelectedOrder" OnClick="ActionHandler" CommandArgument='<%# Eval(Resources.DataFieldRes.SPPk) %>'
                                                                    ImageUrl="../images/Classic/Icons/view-top-menu.png" CommandName="VIEW" ToolTip="<%$ resources:StatusDetails %>" />
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
                                                                            <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                                                <ItemTemplate>
                                                                                    <%--  <asp:Label ID="lblDetailsSoNO" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONumber) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SONumber) %>'></asp:Label>--%>
                                                                                    <asp:LinkButton ID="lnkSCNo" runat="server" Text='<%#Eval(Resources.DataFieldRes.SONumber) %>' CssClass="text-underline"
                                                                                        OnClick="ActionHandler" CommandName="SCPRINT" CommandArgument='<%# Eval(Resources.DataFieldRes.SCPK) %>'></asp:LinkButton>
                                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18%" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDetailsSoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SaleOrderDate, Resources.Constants.DateFormatGrid) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SaleOrderDate, Resources.Constants.DateFormatGrid) %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="16%" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDeatilsProductCode" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemCode) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.ItemCode) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18%" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDeatilsBrandName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.BrandName),33) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.BrandName) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="38%" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:Cartons %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDeatilscartons" runat="server" Text='<%# Eval(Resources.DataFieldRes.CartonsQty) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.CartonsQty) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="Label2" Text="" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="20%" />
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
                                                    <FooterStyle CssClass="table-firstlevela-total" />
                                                </cc1:ExtGridView>
                                                <uc1:PagerControl ID="uclPagingCustomersOrders" runat="server" />
                                            </div>
                                            <%--Status Details --%>
                                            <div class="gridwrap grid" id="divAllocatedGid">
                                                <asp:GridView runat="server" ID="grdAllocated" Width="100%" AllowSorting="True" ShowFooter="true"
                                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:PaymentDone %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPaymentDone" runat="server" Text='<%# Eval("SNH_PAY_DATE") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%-- <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ContainerConditionChecked %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContainerConditionChecked" runat="server" Text='<%# Eval("SNH_CHK_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%-- <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ContainerInspected %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContainerInspected" runat="server" Text='<%# Eval("SNH_INS_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%--  <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:QADocsUploaded %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQADocsUploaded" runat="server" Text='<%# Eval("SNH_QAD_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%-- <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ExportDocsUploaded %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExportDocsUploaded" runat="server" Text='<%# Eval("SNH_EXD_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%--   <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:LoadingPlanCompleted %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLoadingPlanCompleted" runat="server" Text='<%# Eval("SNH_LDP_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%--   <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Photographsuploaded %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhotographsuploaded" runat="server" Text='<%# Eval("SNH_PHT_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%--   <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:DOGenerated %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSDgenerated" runat="server" Text='<%# Eval("SNH_GON_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <%-- <ItemStyle Width="11%" />--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ContainerReleased %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContainerReleased" runat="server" Text='<%# Eval("SNH_CRL_DATE")%>'>

                                                                </asp:Label>
                                                                <asp:HiddenField ID="hdfTrxStatus" runat="server" Value='<%# Eval("SNH_TRX_STATUS")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:BillOfLoading %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBL" runat="server" Text='<%# Eval("SNH_BLL_DATE")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <table>
                                            <tr>
                                                <td colspan="9">
                                                    <div class="qty-colapse">
                                                        <table>
                                                            <tr>
                                                                <td style="width: auto">
                                                                    <h1>
                                                                        <asp:Label ID="lblPTerms" AssociatedControlID="imbPTermsShow" runat="server" Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                                    </h1>
                                                                </td>
                                                                <td style="width: auto">
                                                                    <asp:ImageButton runat="server" ID="imbPTermsShow" OnClientClick="javascript:return ShowHidePTerm(1);"
                                                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:PTermsShow %>" />
                                                                    <asp:ImageButton runat="server" ID="imbPTermsHide" OnClientClick="javascript:return ShowHidePTerm();"
                                                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                                        ToolTip="<%$ resources:PTermsHide %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--Payment terms --%>
                                        <div class="gridwrap" id="divPTermsGrid">
                                            <asp:GridView runat="server" ID="grdPaymentTerms" OnPageIndexChanging="grdPaymentTerms_OnPaging"
                                                Width="100%" PageSize="<%$ resources:PageSize%>" AllowSorting="false" AllowPaging="true"
                                                OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                                Style="margin: 0px !important;">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:PaymentTerm%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatementDate" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TCH_NAME")),32)  %>'
                                                                ToolTip='<%# HttpUtility.HtmlDecode(Eval("TCH_NAME").ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="40%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Description%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatementDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TCH_DESC")),55)  %>'
                                                                ToolTip='<%# HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(Eval("TCH_DESC").ToString()).ToString()) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="70%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div class="button-container-bottom">
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <table>
                                            <tr>
                                                <td colspan="9">
                                                    <div class="qty-colapse">
                                                        <table>
                                                            <tr>
                                                                <td style="width: auto">
                                                                    <h1>
                                                                        <asp:Label ID="lblinv" AssociatedControlID="imbInvoiceShow" runat="server" Text="<%$ resources:Invoice %>"></asp:Label>
                                                                    </h1>
                                                                </td>
                                                                <td style="width: auto">
                                                                    <asp:ImageButton runat="server" ID="imbInvoiceShow" OnClientClick="javascript:return ShowHideInvoice(1);"
                                                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:InvoiceShow %>" />
                                                                    <asp:ImageButton runat="server" ID="imbInvoiceHide" OnClientClick="javascript:return ShowHideInvoice();"
                                                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                                        ToolTip="<%$ resources:InvoiceHide %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--Invoice --%>
                                        <div class="gridwrap" id="divInvoiceGrid">
                                            <asp:GridView runat="server" ID="grdInvoice" OnPageIndexChanging="grdInvoice_OnPaging"
                                                Width="100%" PageSize="<%$ resources:PageSize%>" AllowSorting="false" AllowPaging="true"
                                                OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                                Style="margin: 0px !important;">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:InvNo%>">
                                                        <ItemTemplate>
                                                            <%-- <asp:Label ID="lblStatementDate" runat="server" Text='<%# Eval("ICH_NO")  %>'
                                                            ToolTip='<%# Eval("ICH_NO") %>'></asp:Label>--%>
                                                            <asp:LinkButton ID="lnkInvoiceNo" CssClass="text-underline" runat="server" Text='<%# Eval("ICH_NO") %>'
                                                                OnClick="ActionHandler" CommandName="PRINT" ToolTip='<%# Eval("ICH_NO") %>'></asp:LinkButton>
                                                            <asp:HiddenField ID="hdfInvType" runat="server" Value="<%# Eval(Resources.DataFieldRes.InvoiceType) %>" />
                                                            <asp:HiddenField ID="hdfAptCode" runat="server" Value="<%# Eval(Resources.DataFieldRes.AptCode) %>" />
                                                            <asp:HiddenField ID="hdfInvPK" runat="server" Value="<%# Eval(Resources.DataFieldRes.SalesInvoicePK) %>" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatementDate" runat="server" Text='<%# Eval("ICH_DATE",Resources.Constants.DateFormatGrid)  %>'
                                                                ToolTip='<%# Eval("ICH_DATE",Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Amount%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatementAmount" runat="server" Text='<%# Eval("ICH_AMOUNT", "{0:c}")  %>'
                                                                ToolTip='<%#Eval("ICH_AMOUNT", "{0:c}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:Received%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatementAmtRecd" runat="server" Text='<%# Eval("ICH_AMOUNT_RCVD", "{0:c}")  %>'
                                                                ToolTip='<%# Eval("ICH_AMOUNT_RCVD") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="amount-numeric" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div class="button-container-bottom">
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <%--  <table>
                                                <tr>
                                                    <td colspan="9">
                                                        <div class="qty-colapse">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: auto">
                                                                        <h1>
                                                                            <asp:Label ID="lblState" AssociatedControlID="imbStatementShow" runat="server" Text="<%$ resources:Statement %>"></asp:Label>
                                                                        </h1>
                                                                    </td>
                                                                    <td style="width: auto">
                                                                        <asp:ImageButton runat="server" ID="imbStatementShow" OnClientClick="javascript:return ShowHideStatement(1);"
                                                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:StatementShow %>" />
                                                                        <asp:ImageButton runat="server" ID="imbStatementHide" OnClientClick="javascript:return ShowHideStatement();"
                                                                            Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                                            ToolTip="<%$ resources:StatementHide %>" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>--%>
                                        <%--Statement --%>
                                        <div class="gridwrap" id="divStatementGrid">
                                            <div class="button-container-bottom">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="diverror" style="display: none">
                            <%--Use this label to bind the server errors--%>
                            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                            <asp:ValidationSummary ID="vsPlan" ValidationGroup="Allocation" runat="server" />
                        </div>
                        <div id="DiverrorMessages" style="display: none">
                            <asp:GridView runat="server" ID="grdError" Width="100%" AutoGenerateColumns="false"
                                EmptyDataRowStyle-CssClass="emptytable">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:Messages%>">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblErrorMsg" runat="server" Text='<%# Eval(Resources.DataFieldRes.dbRetValTxt) %>'></asp:Label>--%>
                                            <%# ((int)Eval(Resources.DataFieldRes.dbRetVal) > 0 ? "<span >" : "<span style='color:Red' >") + Eval(Resources.DataFieldRes.dbRetValTxt).ToString() + "</span>"%>
                                        </ItemTemplate>
                                        <ItemStyle Width="100%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="button-container-bottom">
                                <asp:Button ID="btnCancelpopup" runat="server" CausesValidation="true" Text="<%$ resources:Close %>"
                                    OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" />
                            </div>
                        </div>
                        <div id="divAllocatioDetails" style="display: none;">
                            <div class="contentwrapper present">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <%--Product --%>
                                                <asp:Label ID="litProduct" Text="<%$ resources:Product %>" AssociatedControlID="lblProduct"
                                                    runat="server"></asp:Label>
                                                <asp:Label ID="lblProduct" runat="server"> </asp:Label>
                                                <%--Description --%>
                                                <asp:Label ID="litDescription" Text="<%$ resources:Description %>" AssociatedControlID="lblDescription"
                                                    runat="server"></asp:Label>
                                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <%--Size --%>
                                                <asp:Label ID="litSize" Text="<%$ resources:Size %>" AssociatedControlID="lblSize"
                                                    runat="server"></asp:Label>
                                                <asp:Label ID="lblSize" runat="server"> </asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="grdhead">
                                    <asp:Label ID="lblAllocationHead" runat="server" Text="<%$ resources:AllocationDetails %>"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <%--100 Statement Grid row Begins--%>
                    <div class="dash100">
                        <div class="dashinner100">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Statement").ToString()%><asp:Literal ID="ltStatementCurrency"
                                        runat="server"></asp:Literal>
                                        <%--<asp:Label ID="lblCurrency" runat="server"></asp:Label>--%>
                                </h1>
                                <div class="controls">
                                    <asp:Label ID="lblOutstanding" runat="server" Text="Current Outstanding: USD 25,300.000"></asp:Label>
                                    <%-- <asp:ImageButton ID="lnkStatementRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="Statement"></asp:ImageButton>--%>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="grdStatement" OnPageIndexChanging="grdStatement_OnPaging"
                                            Width="100%" PageSize="<%$ resources:PageSizeGrdStmt%>" AllowSorting="false"
                                            AllowPaging="true" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                            Style="margin: 0px !important;">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementDate" runat="server" Text='<%# Eval("Date", Resources.Constants.DateFormatGrid)  %>'
                                                            ToolTip='<%# Eval("Date", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementType" runat="server" Text='<%# Eval("TYPE_TEXT")  %>' ToolTip='<%# Eval("TYPE_TEXT") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Statement_Reference%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementReference" runat="server" Text='<%# Eval("Ref_No") %>'
                                                            ToolTip='<%# Eval("Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TRXAmt%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTRXAmt" runat="server" Text='<%# Eval("TRXAmt")  %>' ToolTip='<%# Eval("TRXAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Debit%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementDebit" runat="server" Text='<%# (decimal)Eval("Debit") > 0 ? Eval("Debit", "{0:c}") : ""%>'
                                                            ToolTip='<%# (decimal)Eval("Debit") > 0 ? Eval("Debit", "{0:c}") : ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Credit%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementCredit" runat="server" Text='<%# (decimal)Eval("Credit") > 0 ?  Eval("Credit", "{0:c}") : ""%>'
                                                            ToolTip='<%# (decimal)Eval("Credit") > 0 ?  Eval("Credit", "{0:c}") : ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Balance%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementBalance" runat="server" Text='<%# Math.Round((decimal)Eval("Balance"),3) %>'
                                                            ToolTip='<%# Math.Round((decimal)Eval("Balance"),3) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfStatementBasecurrency" runat="server" Value='<%# Eval("base_cur_text") %>' />
                                                        <asp:HiddenField ID="hdfStatementBalance" runat="server" Value='<%# Eval("Balance") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
