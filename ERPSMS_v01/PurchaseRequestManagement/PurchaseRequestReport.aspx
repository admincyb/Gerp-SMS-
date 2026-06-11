<%@ Page Title="<%$ Resources:Captions,Title_PurchaseRequestReport %>" Language="C#"
   MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="Classic" CodeBehind="PurchaseRequestReport.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseRequestManagement.PurchaseRequestReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.PurchaseRequestReport%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="8" />
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
                               <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" OnClientClick="javascript:return CancelFun();"  />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div class="reportviewer">
            <rsweb:ReportViewer ID="rptPurchaseRequest" runat="server" BorderWidth="0" SizeToReportContent="true" 
            width="98%">
        </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
