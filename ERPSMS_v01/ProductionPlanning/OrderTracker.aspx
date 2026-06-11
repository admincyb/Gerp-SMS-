<%@ Page Title="<%$ Resources:Captions,Title_OrderTracker %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" 
    AutoEventWireup="true" EnableEventValidation="true" CodeBehind="OrderTracker.aspx.cs"
    Theme="Classic" Inherits="ERPSMS_v01.ProductionPlanning.OrderTracker" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");


        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDateOrder", "hdfFromDateOrder", "txtToDateOrder", "hdfToDateOrder", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", false, false, "CUSTOMERLIST");
            showProductName();
        }
        function showProductName() {
            if ($("[id$=lblProductName]").html() == "") {
                $("#divProductName").hide();
            }
            else {
                $("#divProductName").show();
            }
        }

        $(document).ready(function () {
            ShowHideCustomersOrders(0);
            ShowHideAllocated(0);
            ShowHidePlanned(0);
            ShowHideDespatched(0);
            ShowHidePacked(0);
           
        });


        function ShowHideCustomersOrders(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divCustomersOrdersGid").show();
                $("[id$=imbShowCustomersOrders]").hide();
                $("[id$=imbHideCustomersOrders]").show();
            }
            else {

                $("#divCustomersOrdersGid").hide();
                $("[id$=imbShowCustomersOrders]").show();
                $("[id$=imbHideCustomersOrders]").hide();
            }
            return false;
        }

        function ShowHideAllocated(flag) {
            //If flag then Show Labours

            if (flag) {

                $("#divAllocatedGid").show();
                $("[id$=imbAllocatedShow]").hide();
                $("[id$=imbAllocatedHide]").show();
            }
            else {

                $("#divAllocatedGid").hide();
                $("[id$=imbAllocatedShow]").show();
                $("[id$=imbAllocatedHide]").hide();
            }
            return false;
        }

        function ShowHidePlanned(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divPlannedGid").show();
                $("[id$=imbPlannedShow]").hide();
                $("[id$=imbPlannedHide]").show();
            }
            else {

                $("#divPlannedGid").hide();
                $("[id$=imbPlannedShow]").show();
                $("[id$=imbPlannedHide]").hide();
            }
            return false;
        }

        function ShowHideDespatched(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divDespatchedGid").show();
                $("[id$=imbDespatchedShow]").hide();
                $("[id$=imbDespatchedHide]").show();
            }
            else {

                $("#divDespatchedGid").hide();
                $("[id$=imbDespatchedShow]").show();
                $("[id$=imbDespatchedHide]").hide();
            }
            return false;
        }
        function ShowHidePacked(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divPackedGrid").show();
                $("[id$=imbPackedShow]").hide();
                $("[id$=imbPackedHide]").show();
            }
            else {

                $("#divPackedGrid").hide();
                $("[id$=imbPackedShow]").show();
                $("[id$=imbPackedHide]").hide();
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

    </script>
    

<script type = "text/javascript">

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
    <asp:UpdatePanel ID="udpMainContent" runat="server">
        <ContentTemplate>
            <div class="search-wrap-fixed">
                <asp:TextBox ID="txtFromDateOrder" runat="server" TabIndex="2"></asp:TextBox>
                <asp:HiddenField ID="hdfFromDateOrder" runat="server" />
                <asp:RequiredFieldValidator ID="vrfFromDateOrder" CssClass="star" SetFocusOnError="true"
                    ValidationGroup="rpt" runat="server" ControlToValidate="txtFromDateOrder" Display="Dynamic"
                    Text="*" ErrorMessage="<%$ resources:Msg_DateFrom%>" EnableClientScript="true">
                </asp:RequiredFieldValidator>
                <asp:TextBox ID="txtToDateOrder" runat="server" TabIndex="3" CssClass="medium"></asp:TextBox>
                <asp:HiddenField ID="hdfToDateOrder" runat="server" />
                <asp:RequiredFieldValidator ID="vrfToDateOrder" CssClass="star" SetFocusOnError="true"
                    ValidationGroup="rpt" runat="server" ControlToValidate="txtToDateOrder" Display="Dynamic"
                    Text="*" ErrorMessage="<%$ resources:Msg_DateTo%>" EnableClientScript="true">
                </asp:RequiredFieldValidator>
                <asp:Button runat="server" TabIndex="1" ID="btnGenerate" CommandName="SEARCH" OnClick="ActionHandler"
                    ToolTip="<%$ resources:Controls,Go %>" Text="<%$ resources:Controls,Go %>"
                    CommandArgument="SEC_ActionPanel" SkinID="btnInner-Go" />
            </div>
            <div class="search-wrap-fixed-space">
                <%--only for space--%></div>
            <table>
                <tr>
                    <td class="trv-td">
                        <div class="tree-search">
                            <asp:TextBox ID="txtCustomer"  ClientIDMode="Static"  runat="server" Text="<%$ resources:EnterCustomer%>" TabIndex="4"
                              onblur="if (this.value == '') {this.value = 'Enter Customer Name';}" onfocus="if (this.value == 'Enter Customer Name') {this.value = '';}">
                            </asp:TextBox>
                          <%--  <asp:TextBox ID="txtCustomer"  ClientIDMode="Static"  runat="server" Text="<%$ resources:Customer%>"
                              ForeColor = "Gray" onblur = "WaterMark(this, event);"onfocus = "WaterMark(this, event);">
                            </asp:TextBox>--%>

                            <asp:HiddenField ID="hdfCustomerID" runat="server" ClientIDMode="Static" Value="0" />
                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="ActionHandler" CommandName="AUTOSEARCH" SkinID="imgbtnsearch" TabIndex="5" />
                        </div>
                        <div class="tree-search-space">
                            <%--for space--%></div>
                        <div class="treeview" style="min-height: 450PX">
                            <asp:TreeView ID="trvCustomers" runat="server" ShowLines="true" OnTreeNodePopulate="PopulateParentsChild" TabIndex="6"
                                OnSelectedNodeChanged="ActionHandler_onSelect" EnableClientScript="true">
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
                                                            <asp:Label ID="lblCustomerOrderHdr" AssociatedControlID="imbShowCustomersOrders"
                                                                runat="server" Text="<%$ resources:CustomerOrderHdr %>"></asp:Label>
                                                        </h1>
                                                    </td>
                                                    <td style="width: auto">
                                                        <asp:ImageButton runat="server" ID="imbShowCustomersOrders" OnClientClick="javascript:return ShowHideCustomersOrders(1);"
                                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:CustomerOrderHdrShow %>" />
                                                        <asp:ImageButton runat="server" ID="imbHideCustomersOrders" OnClientClick="javascript:return ShowHideCustomersOrders();"
                                                            Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                            ToolTip="<%$ resources:CustomerOrderHdrHide %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--Customers Orders--%>
                            <div class="gridwrap" id="divCustomersOrdersGid">
                                <asp:GridView runat="server" ID="grdCustomersOrders" Width="100%" ShowFooter="true"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdCustomersOrders_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Product %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblProduct" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProductCode) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>">
                                    </asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# gErpProductionPlanning.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Product),25) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.Product) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Product %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProduct" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProductCode) %>'
                                                    EnableTheming="false" Style="display: none;"></asp:Label>
                                                <asp:Label ID="lblSKU" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ProductText)),17) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ProductText)) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>">
                                                </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="20%" />

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Size %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" runat="server" Text='<%# Eval(Resources.DataFieldRes.Size)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Reqd %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReqd" runat="server" Text='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblhdrQty" runat="server" Text="Qty." ToolTip="<%$ resources:OrderQty %>"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQty,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQtyTotal,"{0:n0}")%>'>
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
                                                <asp:Label ID="lblBalanceToProduce" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToProduce,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalBalanceToProduce" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToProduceTotal)%>'>
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
                                                <asp:Label ID="lblBalanceToPlan" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToPlan,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalBalanceToPlan" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToPlanTotal)%>'>
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
                                                <asp:Label ID="lblBalanceToDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToDispatch,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalBalanceToDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToDispatchTotal)%>'>
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
                                                <asp:Label ID="lblPacked" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyPAcked,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalPacked" runat="server" >
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
                                                <asp:Label ID="lblDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyDispatched,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalToDispatch" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyDispatchedTotal)%>'>
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
                                                <asp:Label ID="lblAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyAllocated,"{0:n0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAllocatedTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyAllocatedTotal)%>'>
                                                </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
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
                                            <HeaderStyle CssClass="amount-numeric" />
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
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbSelectedOrder" OnClick="ActionHandler" CommandArgument='<%# Eval(Resources.DataFieldRes.SaleODPK) %>'
                                                    ImageUrl="../images/Classic/Icons/view-top-menu.png" CommandName="VIEW" ToolTip="Show Product Details" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPagingCustomersOrders" runat="server" />
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divProductName">
                            <h5>
                                <asp:Label ID="lblProductName" runat="server"></asp:Label></h5>
                                </div>
                            <table>
                                <tr>
                                    <td colspan="9">
                                        <div class="qty-colapse">
                                            <table>
                                                <tr>
                                                    <td style="width: auto">
                                                        <h1>
                                                            <asp:Label ID="lblAllocatedHdr" AssociatedControlID="imbAllocatedShow" runat="server"
                                                                Text="<%$ resources:AllocatedHdr %>"></asp:Label>
                                                            <asp:Label ID="lblAllocatedDetails" runat="server"></asp:Label>
                                                        </h1>
                                                    </td>
                                                    <td style="width: auto">
                                                        <asp:ImageButton runat="server" ID="imbAllocatedShow" OnClientClick="javascript:return ShowHideAllocated(1);"
                                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:AllocatedHdrShow %>" />
                                                        <asp:ImageButton runat="server" ID="imbAllocatedHide" OnClientClick="javascript:return ShowHideAllocated(0);"
                                                            Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                            ToolTip="<%$ resources:AllocatedHdrHide %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--Allocated Items --%>
                            <div class="gridwrap" id="divAllocatedGid">
                                <asp:GridView runat="server" ID="grdAllocated" Width="100%" AllowSorting="True" ShowFooter="true"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdAllocated_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:AllocatedDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAllocatedDate" runat="server" Text='<%#Eval(Resources.DataFieldRes.AllocatedDate,Resources.Constants.DateFormatGrid) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AllocatedQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAllocatedQty" runat="server" Text='<%#Eval(Resources.DataFieldRes.AllocatedQty,"{0:n0}") %>'>
                                                </asp:Label>
                                                <asp:HiddenField ID="hdfAllocatedQty" runat="server" Value='<%#Eval(Resources.DataFieldRes.AllocatedQty) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="clear">
                            </div>
                          <%--   <table>
                                <tr>
                                    <td colspan="9">
                                        <div class="qty-colapse">
                                            <table>
                                                <tr>
                                                    <td style="width: auto">
                                                        <h1>
                                                            <asp:Label ID="lblPlannedHdr" AssociatedControlID="imbPlannedShow" runat="server"
                                                                Text="<%$ resources:PlannedHdr %>"></asp:Label>
                                                            <asp:Label ID="lblPlannedDetails" runat="server"></asp:Label>
                                                        </h1>
                                                    </td>
                                                    <td style="width: auto">
                                                        <asp:ImageButton runat="server" ID="imbPlannedShow" OnClientClick="javascript:return ShowHidePlanned(1);"
                                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:PlannedHdrShow %>" />
                                                        <asp:ImageButton runat="server" ID="imbPlannedHide" OnClientClick="javascript:return ShowHidePlanned(0);"
                                                            Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                            ToolTip="<%$ resources:PlannedHdrHide %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>--%>
                            <%--Planned Items--%>
                           <%-- <div class="gridwrap" id="divPlannedGid">
                                <asp:GridView runat="server" ID="grdPlanned" Width="100%" ShowFooter="true" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdPlanned_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:gErpProductionRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:PlannedDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlannedDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.PlannedDate,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlannedQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlannedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PlannedQty,"{0:n0}") %>'></asp:Label>
                                                <asp:HiddenField ID="hfPlannedQty" runat="server" Value='<%# Eval(Resources.DataFieldRes.PlannedQty) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>--%>
                                <%-- <uc1:PagerControl ID="uclPagingPlanned" runat="server" />--%>
                           <%-- </div>
                            <div class="clear">
                            </div>--%>
                            <table>
                                <tr>
                                    <td colspan="9">
                                        <div class="qty-colapse">
                                            <table>
                                                <tr>
                                                    <td style="width: auto">
                                                        <h1>
                                                            <asp:Label ID="Label1" AssociatedControlID="imbPackedShow" runat="server" Text="<%$ resources:PackedHdr %>"></asp:Label>
                                                            <asp:Label ID="lblPackedDetail" runat="server"></asp:Label>
                                                        </h1>
                                                    </td>
                                                    <td style="width: auto">
                                                        <asp:ImageButton runat="server" ID="imbPackedShow" OnClientClick="javascript:return ShowHidePacked(1);"
                                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:PackedHdrShow %>" />
                                                        <asp:ImageButton runat="server" ID="imbPackedHide" OnClientClick="javascript:return ShowHidePacked();"
                                                            Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                            ToolTip="<%$ resources:PackedHdrHide %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--Packed Items --%>
                            <div class="gridwrap" id="divPackedGrid">
                                <asp:GridView runat="server" ID="grdPackedDetails" Width="100%" AllowSorting="True"
                                    ShowFooter="true" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowDataBound="grdPackedDetails_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:PackedDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPAckedDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackedDate,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>">
                                                </asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PackedQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackedQty,"{0:n0}") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfPackedQty" runat="server" Value='<%# Eval(Resources.DataFieldRes.PackedQty) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalPackedQty" runat="server"> </asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--                    <uc1:PagerControl ID="uclPagingDespatched" runat="server" />--%>
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
                                                            <asp:Label ID="lblDespatchedHdr" AssociatedControlID="imbDespatchedShow" runat="server"
                                                                Text="<%$ resources:DespatchedHdr %>"></asp:Label>
                                                            <asp:Label ID="lblDespatchedDetails" runat="server"></asp:Label>
                                                        </h1>
                                                    </td>
                                                    <td style="width: auto">
                                                        <asp:ImageButton runat="server" ID="imbDespatchedShow" OnClientClick="javascript:return ShowHideDespatched(1);"
                                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:DespatchedHdrShow %>" />
                                                        <asp:ImageButton runat="server" ID="imbDespatchedHide" OnClientClick="javascript:return ShowHideDespatched();"
                                                            Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                            ToolTip="<%$ resources:DespatchedHdrHide %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--Despatched Items --%>
                            <div class="gridwrap" id="divDespatchedGid">
                                <asp:GridView runat="server" ID="grdDespatched" Width="100%" AllowSorting="True"
                                    ShowFooter="true" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnRowDataBound="grdDespatched_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:DespatchDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.DespatchDate,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespatchedQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.DespatchedQty,"{0:n0}") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfDespatchedQty" runat="server" Value='<%# Eval(Resources.DataFieldRes.DespatchedQty) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--                    <uc1:PagerControl ID="uclPagingDespatched" runat="server" />--%>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>