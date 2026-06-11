<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PgerControlNew.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.PgerControlNew" %>
<div class="gridpagingWrapper">
    <table>
        <tr>
            <td width="85%">
                <div class="page-navigation">
                    <asp:Label ID="lblCurrentPage" runat="server" Visible="false"></asp:Label>
                    <span style="width: auto;">Page</span>
                    <asp:DropDownList Width="50px" ID="ddPage" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <span style="width: auto;">of</span>
                    <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
                </div>
            </td>
            <td width="10%">
                <div class="user-control">
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnFirst" runat="server" Enabled="false" SkinID="imbGridFirst"
                                    CommandName="FIRST" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnPrevious" runat="server" Enabled="false" SkinID="imbGridPrev"
                                    CommandName="PREVIOUS" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnNext" runat="server" Enabled="false" SkinID="imbGridNext"
                                    CommandName="NEXT" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btnLast" runat="server" Enabled="false" SkinID="imbGridLast"
                                    CommandName="LAST" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
