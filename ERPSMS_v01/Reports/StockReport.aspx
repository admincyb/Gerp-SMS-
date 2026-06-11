<%@ Page Title="<%$ Resources:Captions,Title_StockReport %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="StockReport.aspx.cs" Theme="Classic" Inherits="ERPSMS_v01.StoreManagement.StockReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <div class="button-wrap">
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
        </div>
    </div>--%>
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
                                <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
    </div>
    <div id="searchwrap" class="search-wrap-custom1">
        <div id="divSearch">
            <label for="Store">
                <%=Resources.Controls.Store%></label>
            <asp:DropDownList ID="Store" runat="server" CssClass="srchboxtextbx" TabIndex="1">
            </asp:DropDownList>
            <label for="SearchType">
                <%=Resources.Controls.Item%></label>
            <asp:DropDownList ID="Item" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                Width="200px">
            </asp:DropDownList>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" EnableViewState="false"
                OnClick="imbSearch_Click" />
        </div>
        <div class="clear">
        </div>
    </div>
  <%--  <div class="reportviewer" style="width: 98%; overflow-y: scroll;">
        <rsweb:ReportViewer ID="RptStockReport" runat="server" Width="100%" BorderWidth="0"
            BorderStyle="None">
        </rsweb:ReportViewer>
    </div>--%>
     <div class="content-wrapper">
                <div class="reportviewer">
                    <rsweb:ReportViewer ID="RptStockReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                        Width="98%">
                    </rsweb:ReportViewer>
                </div>
            </div>

    <div class="clear">
    </div>
</asp:Content>
