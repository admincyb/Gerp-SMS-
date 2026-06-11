<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoicePrintDocs.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.InvoicePrintDocs" %>
<%--Print popup window --%>
<div id="divPrint" style="display: none" class="content-wrapper">
    <table class="table-devide">
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 60%;">
                <asp:Label ID="lblSDPrint" runat="server" Text="<%$resources:Controls,PrintShippingDocuments %>"
                    AssociatedControlID="ddlSDPrint"></asp:Label>
            </td>
            <td style="width: 30%;">
                <asp:DropDownList ID="ddlSDPrint" runat="server" TabIndex="101" Width="165px" onmouseover="javascript:ShowTooltip('ddlSDPrint');">
                </asp:DropDownList>
            </td>
            <td style="width: 20%;">
                <asp:Button runat="server" TabIndex="102" ID="btnPrintlistSD" Text="<%$resources:Controls,Print %>"
                    SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClick="ActionHandler"
                    CommandName="PRINTGON" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 60%;">
                <asp:Label ID="lblSICopyPrint" runat="server" Text="<%$resources:Controls,PrintCommericalInvoice %>"
                    AssociatedControlID="ddlSICopy"></asp:Label>
            </td>
            <td style="width: 30%;">
                <asp:DropDownList ID="ddlSICopy" runat="server" TabIndex="101" Width="165px" onmouseover="javascript:ShowTooltip('ddlSICopy');">
                </asp:DropDownList>
            </td>
            <td style="width: 20%;">
                <asp:Button runat="server" TabIndex="102" ID="btnPrintCopySI" Text="<%$resources:Controls,Print %>"
                    SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClick="ActionHandler"
                    CommandName="PRINTSI" />
            </td>
        </tr>
    </table>
    <asp:Button ID="btnPrintDoc" runat="server" Text="Button" OnClick="PrintDoc_Click" Visible="false" />
</div>
