<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic"
    AutoEventWireup="true" CodeBehind="ProductionReports.aspx.cs" Inherits="ERPSMS_v01.Reports.ProductionReports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StockDetailsReport.js" type="text/javascript"></script>
    <script type="text/javascript">
        function InitComponents() {

            GrandScriptUtils.AddDateRange("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, true, false);
        }
        $(document).ready(function () { InitComponents(); });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <%--<asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="PurchaseOrderListing.aspx" />--%>
                                        <asp:Button ID="btnCancel" TabIndex="6" runat="server" OnClientClick="javascript:return CancelFun();"
                                            SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                </div>
            </div>
           <div class="content-wrapper">
                <div id="searchwrap" class="search-wrap-custom1">
                <label for="ddlStore">
                        Store</label>
                    <asp:DropDownList ID="ddlStore" CssClass="medium" runat="server" TabIndex="2">
                    </asp:DropDownList>

                    <label for="StockDate">
                        Date From</label>
                    <asp:TextBox ID="StockDate" runat="server" CssClass="medium" EnableViewState="false"
                        TabIndex="1">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfDateStock" runat="server" />
                    <label for="StockToDate">
                        Date To</label>
                    <asp:TextBox ID="StockToDate" runat="server" CssClass="medium" EnableViewState="false"
                        TabIndex="1">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfStockToDate" runat="server" />
                    
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" EnableViewState="false"
                        OnClick="imbSearch_Click" />
                    <div class="clear">
                    </div>
                </div>
                <div class="reportviewer max-500">
                    <rsweb:ReportViewer ID="RptViewerReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                        Width="98%">
                    </rsweb:ReportViewer>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
