<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_Dashboard.Master" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="Dashboard.aspx.cs" Inherits="ERP.Dashboard.Dashboard"
    EnableEventValidation="false" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms"
    TagPrefix="rsweb" %>
<%@ Register Src="~/UserControls/DashboardPagerControl.ascx" TagName="PagerControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <link href="JQueryPrint/css/print-preview.css" rel="stylesheet" type="text/css" />
    <script src="JQueryPrint/jquery.print-preview.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function InitComponents() {
        }

        function ValidateNow() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
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
    <style>
        .dash-menu-bar {
  background: #ffffff url(../../Images/BlueExt/layout/dash-menu-bg.png) repeat-x left bottom; 
  border-bottom: 1px solid #b7b7b7;
  padding: 2px 10px;
  height: 50px;
  background: #2891ae  !important;
  box-shadow: inset 0 -27px 8px 0 rgba(22,104,126,0.3)  !important;
}
.dashboard-search > label {
  color: #fff  !important;
}
.dash-subhead {
  border-bottom: 1px solid #a8a8a8;
  padding: 2px 5px;
  border-radius: 8px 6px 0 0;
  background: #2891ae !important;
  box-shadow: inset 0 -8px 1px 0 rgba(22,104,126,0.3) !important;
}

.dash-subhead h1 {
  font-size: 11px;
  margin: 0;
  float: left;
  border-bottom: 0 none;
  padding: 0 0 0 0;
  color: #fff !important;
}
.dash-subhead .controls > span {
  color: #fff !important;
  font-weight: 600 !important;
}
.transaction-wrapper h2 {
  font-size: 11px;
  margin: 2px 0;
  border-bottom: 0 none;
  padding: 13px 0 5px !important;
  color: #000 !important;
  font-weight: 600 !important;
  margin-bottom: 0 !important;
}
.transaction-sub50 {
  background: #f7f7f7 !important;
  border: 1px solid #a8a8a8;
  padding: 3px;
  width: 49%;
  border-radius: 6px 8px 8px 6px;
}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlDashboard">
        <ContentTemplate>
            <div class="content-wrapper">
                <div class="dashboard-wrapper">
                    <%-- 30 70 Pie and Bar chart Begins--%>
                    <div class="dash30-70">
                        <div class="dashinner30">
                            <div class="dash-subhead">
                                <h1>
                                    <asp:Literal ID="ltTopBrands" runat="server" Text="<%$ resources:Top5Brands_Ordered%>"></asp:Literal></h1>
                                <div class="controls">
                                    <asp:ImageButton ID="lnkPieRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="PIECHART"></asp:ImageButton>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Chart ID="chPie" runat="server" Width="270" Height="120">
                                            <Series>
                                                <asp:Series Name="Series1" ChartType="Pie" ToolTip="#VALX">
                                                </asp:Series>
                                            </Series>
                                            <ChartAreas>
                                                <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="false">
                                                </asp:ChartArea>
                                            </ChartAreas>
                                        </asp:Chart>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="dashinner70">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Monthly_Orders").ToString()%></h1>
                                <div class="controls">
                                    <asp:ImageButton ID="lnkBarRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="BARCHART"></asp:ImageButton>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" EnableViewState="true">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlChart">
                                            <asp:Chart ID="chBar" runat="server" Width="622" Height="120">
                                                <BorderSkin BackColor="Transparent" PageColor="Transparent" />
                                                <Series>
                                                    <asp:Series Name="Ordered" ChartType="Area" BackImageTransparentColor="Transparent"
                                                        LegendText="<%$ resources:Orders%>" BackSecondaryColor="White" BorderColor="White"
                                                        Color="#c0e5f0">
                                                    </asp:Series>
                                                    <asp:Series Name="Dispatched" ChartType="StackedArea" BackImageTransparentColor="Transparent"
                                                        BackSecondaryColor="White" BorderColor="White" Color="#bee4b2">
                                                    </asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1" BackColor="Transparent" ShadowColor="Transparent"
                                                        BackGradientStyle="TopBottom">
                                                        <AxisY LineColor="64, 64, 64, 64">
                                                            <MajorGrid Enabled="false" />
                                                        </AxisY>
                                                        <AxisX LineColor="64, 64, 64, 64">
                                                            <MajorGrid Enabled="false" />
                                                        </AxisX>
                                                    </asp:ChartArea>
                                                </ChartAreas>
                                                <BorderSkin BackColor="Transparent" />
                                            </asp:Chart>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <%-- 30 70 Pie and Bar chart Ends--%>
                    <%-- 60 40 Invoice, Dispatch Grid row Begins--%>
                    <div class="dash60-40">
                        <div class="dashinner60">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Invoices").ToString()%></h1>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlInvoiceStatus" runat="server" OnSelectedIndexChanged="ddlInvoiceStatus_IndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="lnkInvoicesRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="Invoice"></asp:ImageButton>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="grdInvoices" OnPageIndexChanging="grdInvoices_OnPaging"
                                            Width="100%" PageSize="<%$ resources:PageSize%>" AllowSorting="false" AllowPaging="true"
                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" Style="margin: 0px !important;">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("Date", Resources.Constants.DateFormatGrid)  %>'
                                                            ToolTip='<%# Eval("Date",  Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:InvoiceNo%>">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("No") %>' ToolTip='<%# Eval("No") %>'></asp:Label>--%>
                                                        <%-- <asp:HyperLink ID="hylkInvoiceNo" runat="server" Text='<%# Eval("No") %>' ToolTip='<%# Eval("No") %>'></asp:HyperLink>--%>
                                                        <asp:LinkButton ID="lnkInvoiceNo" CssClass="text-underline" runat="server" Text='<%# Eval("No") %>'
                                                            OnClick="ActionHandler" CommandName="PRINT" ToolTip='<%# Eval("No") %>'></asp:LinkButton>
                                                        <asp:HiddenField ID="hdfInvType" runat="server" Value="<%# Eval(Resources.DataFieldRes.InvoiceType) %>" />
                                                        <asp:HiddenField ID="hdfAptCode" runat="server" Value="<%# Eval(Resources.DataFieldRes.AptCode) %>" />
                                                        <asp:HiddenField ID="hdfInvPK" runat="server" Value="<%# Eval(Resources.DataFieldRes.PK) %>" />
                                                        <asp:HiddenField ID="hdfCategory" runat="server" Value="<%# Eval(Resources.DataFieldRes.InvCategory) %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="18%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Amount%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("Currency") + " " + (String.Format("{0:c}",(decimal)Eval("Amount"))).ToString(),17) %>'
                                                            ToolTip='<%#  Eval("Currency") + " " + (String.Format("{0:c}",(decimal)Eval("Amount") )).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="24%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Paid%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoicePaid" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("Currency") + " " + (String.Format("{0:c}",(decimal)Eval("Paid"))).ToString(),17) %>'
                                                            ToolTip='<%# Eval("Currency") + " " + (String.Format("{0:c}",(decimal)Eval("Paid") )).ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="22%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Balance%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceBalance" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("Currency") + " " + (String.Format("{0:c}",(decimal)Eval("Balance"))).ToString(),17)  %>'
                                                            ToolTip='<%# Eval("Currency") + " " + (String.Format("{0:c}",(decimal)Eval("Balance") )).ToString()  %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="24%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="dashinner40">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Dispatch").ToString()%></h1>
                                <div class="controls">
                                    <asp:ImageButton ID="lnkDispatchRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="Dispatch"></asp:ImageButton>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="grdDispatch" OnPageIndexChanging="grdDispatch_OnPaging"
                                            Width="100%" PageSize="<%$ resources:PageSize%>" AllowSorting="false" AllowPaging="true"
                                            OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                            Style="margin: 0px !important;">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDispatchDate" runat="server" Text='<%# Eval("Date", Resources.Constants.DateFormatGrid)  %>'
                                                            ToolTip='<%# Eval("Date", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DispatchNo%>">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblDispatchNo" runat="server" Text='<%# Eval("No") %>' ToolTip='<%# Eval("No") %>'></asp:Label>--%>
                                                         <asp:HiddenField ID="hdfDOPK" runat="server" Value='<%# Eval("PK") %>' />
                                                        <asp:LinkButton ID="lnkDispatchNo" runat="server" Text='<%#Eval("No") %>' CssClass="text-underline"
                                                            OnClick="ActionHandler" CommandName="DOPRINT" CommandArgument='<%# Eval("PK") %>' ></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDispatchAmount" runat="server" Text='<%# String.Format("{0:n}",Eval("Total_Qty")) %>'
                                                            ToolTip='<%# String.Format("{0:n}",Eval("Total_Qty")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="25%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Port%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDispatchPort" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("Port"),5)%>'
                                                            ToolTip='<%# Eval("Port")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
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
                    <%-- 60 40 Invoice, Dispatch Grid row Ends--%>
                    <%-- 60 40 Orders, Inquires Grid row Begins--%>
                    <div class="dash60-40">
                        <div class="dashinner60">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Orders").ToString()%></h1>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlOrdersStatus" runat="server" OnSelectedIndexChanged="ddlOrdersStatus_IndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="lnkOrdersRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="Orders"></asp:ImageButton>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="grdOrders" OnPageIndexChanging="grdOrders_OnPaging"
                                            Width="100%" PageSize="<%$ resources:PageSize%>" AllowSorting="false" AllowPaging="true"
                                            OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                            Style="margin: 0px !important;">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("Date", Resources.Constants.DateFormatGrid)  %>'
                                                            ToolTip='<%# Eval("Date", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="14%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:OrderNo%>">
                                                    <ItemTemplate>
                                                             <asp:HiddenField ID="hdfPK" runat="server" Value='<%# Eval("PK") %>' />
                                                        <asp:LinkButton ID="lnkSCNo" runat="server" Text='<%#Eval("No") %>' CssClass="text-underline"
                                                            OnClick="ActionHandler" CommandName="SCPRINT" CommandArgument='<%# Eval("PK") %>' ></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Reference%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderReference" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("Brand_Text"),24) %>'
                                                            ToolTip='<%# Eval("Brand_Text") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="46%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderAmount" runat="server" Text='<%# String.Format("{0:n}",Eval("Ordered_Qty"))%>'
                                                            ToolTip='<%# String.Format("{0:n}",Eval("Ordered_Qty"))%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Status%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" ID="iOrderStatus" ImageUrl="~/Images/Classic/layout/grd-green.png" />
                                                        <asp:HiddenField ID="hdfOrderStatus" runat="server" Value='<%# Eval("STATUS")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" HorizontalAlign="center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="dashinner40">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Enquiries_Quotations").ToString()%></h1>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlEnquiryStatus" runat="server" OnSelectedIndexChanged="ddlEnquiryStatus_IndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="lnkEnquiresRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="Enquiries"></asp:ImageButton>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" ID="grdEnquiries" OnPageIndexChanging="grdEnquiries_OnPaging"
                                            Width="100%" PageSize="<%$ resources:PageSize%>" AllowSorting="false" AllowPaging="true"
                                            OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                            Style="margin: 0px !important;">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", Resources.Constants.DateFormatGrid)  %>'
                                                            ToolTip='<%# Eval("Date", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Enquiry_Quotation_No%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEnquiryNo" runat="server" Text='<%# Eval("No") %>' ToolTip='<%# Eval("No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="72%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEnquiryAmount" runat="server" Text='<%# String.Format("{0:n}",Eval("TOTAL_QTY")) %>'
                                                            ToolTip='<%# String.Format("{0:n}",Eval("TOTAL_QTY")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="14%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" ID="iEnquiryStatus" Visible="false" ImageUrl="~/Images/Classic/layout/grd-green.png" />
                                                        <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                            CssClass='<%# Eval("STATUS_CLASS") %>' ToolTip='<%# Eval("STATUS_TEXT") %>' />
                                                        <asp:HiddenField ID="hdfEnquiryStatus" runat="server" Value='<%# Eval("STATUS")%>' />
                                                        <asp:HiddenField ID="hdfEnquiryStatusText" runat="server" Value='<%# Eval("STATUS_TEXT")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" HorizontalAlign="Center" />
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
                    <%-- 60 40 Orders, Inquires Grid row Ends--%>
                    <%--100 Statement Grid row Begins--%>
                    <div class="dash100">
                        <div class="dashinner100">
                            <div class="dash-subhead">
                                <h1>
                                    <%= GetLocalResourceObject("Statement").ToString()%><asp:Literal ID="ltStatementCurrency"
                                        runat="server"></asp:Literal>
                                </h1>
                                <div class="controls">
                                    <asp:Label ID="lblOutstanding" runat="server" Text="Current Outstanding: USD 25,300.000"></asp:Label>
                                    <asp:ImageButton ID="lnkStatementRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                        OnClick="Refresh_Area" CommandName="Statement"></asp:ImageButton>
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
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementType" runat="server" Text='<%# Eval("Type_Text")  %>'
                                                            ToolTip='<%# Eval("Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Statement_Reference%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementReference" runat="server" Text='<%# Eval("Ref_No") %>'
                                                            ToolTip='<%# Eval("Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" />
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
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Credit%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementCredit" runat="server" Text='<%# (decimal)Eval("Credit") > 0 ?  Eval("Credit", "{0:c}") : ""%>'
                                                            ToolTip='<%# (decimal)Eval("Credit") > 0 ?  Eval("Credit", "{0:c}") : ""%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Balance%>" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatementBalance" runat="server" Text='<%# String.Format("{0:c}",(decimal)Eval("Balance") ) %>'
                                                            ToolTip='<%# String.Format("{0:c}",(decimal)Eval("Balance") ) %>'></asp:Label>
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
                    <%-- 100 Statement Grid row Ends--%>
                    <%--100 Recent Transaction row Begins--%>
                    <div class="transaction-wrapper">
                        <h2>
                            <%= GetLocalResourceObject("Recent_Transactions").ToString()%>
                            <asp:ImageButton ID="lnkRecentTranRefresh" runat="server" SkinID="reback" ToolTip="Refresh"
                                OnClick="Refresh_Area" CommandName="RECENTDETAILS"></asp:ImageButton>
                            <div class="clear">
                            </div>
                        </h2>
                        <div class="transaction-sub50 floatLeft">
                            <div class="section">
                                <h4>
                                    <%= GetLocalResourceObject("Dispatch1").ToString()%>
                                </h4>
                                <asp:Literal ID="ltRecent_Dispatch" runat="server"></asp:Literal>
                            </div>
                            <div class="section">
                                <h4>
                                    <%= GetLocalResourceObject("Order1").ToString()%>
                                </h4>
                                <asp:Literal ID="ltRecent_Order" runat="server"></asp:Literal>
                            </div>
                            <div class="section no-border">
                                <h4>
                                    <%= GetLocalResourceObject("Enquiry1").ToString()%>
                                </h4>
                                <asp:Literal ID="ltRecent_Enquiry" runat="server"></asp:Literal>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="transaction-sub50 floatRight">
                            <div class="section">
                                <h4>
                                    <%= GetLocalResourceObject("Invoice1").ToString()%>
                                </h4>
                                <asp:Literal ID="ltRecent_Invoice" runat="server"></asp:Literal>
                            </div>
                            <div class="section">
                                <h4>
                                    <%= GetLocalResourceObject("Payment1").ToString()%>
                                </h4>
                                <asp:Literal ID="ltRecent_Payment" runat="server"></asp:Literal>
                            </div>
                            <div class="section no-border">
                                <h4>
                                    <%= GetLocalResourceObject("Quote1").ToString()%>
                                </h4>
                                <asp:Literal ID="ltRecent_Quote" runat="server"></asp:Literal>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <%--100 Recent Transaction row Ends--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
