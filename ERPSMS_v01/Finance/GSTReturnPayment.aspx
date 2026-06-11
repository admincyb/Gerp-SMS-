<%@ Page Title="gERP -Record to Report -GST Return Report" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic" CodeBehind="GSTReturnPayment.aspx.cs"
    Inherits="ERPSMS_v01.Finance.GSTReturnPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            GrandScriptUtils.DatePickerCommon("txtStartDate");
            GrandScriptUtils.DatePickerCommon("txtEndDate");
        });

        function viewReport() {
            $('#imgReport').fadeIn();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell ID="tblCell" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label ID="lblBreadCrum" runat="server" Text="Finance<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>GST Return Report" />
                        </ul>
                        <ul runat="server" id="pnlEntry">
                            <li>
                                <asp:Button runat="server" ID="btnView" Text="View" SkinID="btnInner-View" ToolTip="View"
                                    OnClientClick="return viewReport();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                            </li>
                            <li>
                                <asp:Button runat="server" TabIndex="21" ID="btnPrint" CommandName="PRINT" Text="<%$resources:Controls,Print %>"
                                    SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="Label16" Text="Start Date" runat="server" AssociatedControlID="txtStartDate" />
                            <asp:TextBox runat="server" ID="txtStartDate" Text="01-08-2014" Width="150px"/>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="Label17" Text="End Date" runat="server" AssociatedControlID="txtEndDate" />
                            <asp:TextBox runat="server" ID="txtEndDate" Text="31-08-2014" Width="150px"/>
                        </div>
                    </td>
                </tr>
            </table>
            <img id="imgReport" src="GST/Report.png" alt="" style="display: none; margin-top: 25px;
                width: 100%;" />
        </div>
    </div>
</asp:Content>
