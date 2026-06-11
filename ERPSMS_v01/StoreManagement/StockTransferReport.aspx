<%@ Page Title="<%$ Resources:Captions,Title_StockTransfer %>" Language="C#"  MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="StockTransferReport.aspx.cs"
 Inherits="ERPSMS_v01.StoreManagement.StockTransferReport" Theme="Classic"  %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
     <div class="content-wrapper">
        <div class="reportviewer">
            <rsweb:ReportViewer ID="rvStockTransfer" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>
        </div>
    </div>
    <div class="no-data">
        <asp:Literal ID="ltNodata" runat="server" Text="<%$ resources:Msg_Sorry_No_Data_Available %>"
            Visible="false" />
    </div>
    <div class="clear">
    </div>
</asp:Content>