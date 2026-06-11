<%@ Page Title="<%$ Resources:Captions,Title_Print %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="GenRecordToReport.aspx.cs" Inherits="ERPSMS_v01.Reports.GenRecordToReport" %>

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
                                <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="View" SkinID="btnInner-View" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>"
                                    OnClientClick="javascript:return CancelFun();" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="divPeriod" runat="server">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblDateFrom" Text="FromDt" AssociatedControlID="txtFromDate"></asp:Label>
                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="100" CssClass="Uidate-picker" />
                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblDateTo" Text="ToDt" AssociatedControlID="txtToDate"></asp:Label>
                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="100" CssClass="Uidate-picker" />
                            <asp:HiddenField ID="hdfToDate" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divAccount" runat="server">
            <table class="table-devide">
                <tr>
                    <td width="50%">
                        <div class="div2col-S">
                        </div>
                    </td>
                    <td>
                        <div id="divParty" class="div2col-S">
                            <asp:Label runat="server" ID="lblParty" Text="Party" AssociatedControlID="ddlParty"></asp:Label>
                            <asp:DropDownList ID="ddlParty" runat="server"></asp:DropDownList>
                            </div>
                            <div class="div2col-S">
                            <asp:Label runat="server" ID="lblAccGroup" Text="Account Group" AssociatedControlID="ddlAccGroup"></asp:Label>
                            <asp:DropDownList ID="ddlAccGroup" runat="server"></asp:DropDownList>
                        </div>
                       
                    </td>
                    <td>
                     <div class="div2col-S">
                            <asp:Label runat="server" ID="lblType" Text="Type" AssociatedControlID="ddlType"></asp:Label>
                            <asp:DropDownList ID="ddlType" runat="server">
                            </asp:DropDownList>
                    </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="reportviewer">
            <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="visiblefalse">
        <asp:HiddenField ID="hdfAppType" runat="server" />
    </div>
    <div id="diverror" class="visiblefalse">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
    </div>
</asp:Content>
