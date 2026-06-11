<%@ Page Title="gERP -Record to Report -GST Return" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic"  CodeBehind="AdditionalDetails.aspx.cs"
    Inherits="ERPSMS_v01.Finance.AdditionalDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell ID="tblCell" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label ID="lblBreadCrum" runat="server" Text="Finance <label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label> GST Return" />
                        </ul>
                        <ul runat="server" id="pnlEntry">
                            <li>
                                <asp:Button ID="btnNew" runat="server" Text="New" SkinID="btnInner-New" ToolTip="New" />
                            </li>
                            <li runat="server" id="pnlSave">
                                <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="14" Text="<%$resources:Controls,Save %>"
                                    ToolTip="<%$resources:Controls,Save %>" SkinID="btnInner-Save" />
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
        <table style="height: 100px; margin-top: 0px;">
            <tr>
                <td style="width: 100px">
                    <img src="../Images/Demo/malysia-customs.png" alt="Logo" width="100px" height="100px" />
                </td>
                <td align="center" style="letter-spacing: 2px;">
                    <div>
                        <b>JABATAN KASTAM DIRAJA MALAYSIA</b>
                    </div>
                    <div style="margin-bottom: 15px">
                        <b>ROYAL MALAYSIAN CUSTOMS DEPARTMENT</b>
                    </div>
                    <div>
                        PENYATA CUKAI BARANG DAN PERKHIDMATAN</div>
                    <div>
                        GOODS AND SEVRVICES TAX RETURN</div>
                </td>
                <td style="width: 150px;" align="center" valign="middle">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="CheckBox1" Text="Amendment" runat="server" />
                </td>
            </tr>
        </table>
        <div id="grdTable-wrap">
            <h1 class="search-colapse-normal">
                PART C: ADDITIONAL INFORMATION
            </h1>
            <table class="gridwraptable gridwrap">
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="10) Total Value of Local Zero Rated Supplies" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label10" Text="RM" runat="server" AssociatedControlID="amt1" />
                        <asp:TextBox runat="server" ID="amt1" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" Text="11) Total Value of Export Supplies" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label11" Text="RM" runat="server" AssociatedControlID="TextBox1" />
                        <asp:TextBox runat="server" ID="TextBox1" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" Text="12) Total Value of Exempt Supplies" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label12" Text="RM" runat="server" AssociatedControlID="TextBox2" />
                        <asp:TextBox runat="server" ID="TextBox2" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" Text="13) Total Value of Supplies Granted GST Relief" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label14" Text="RM" runat="server" AssociatedControlID="TextBox3" />
                        <asp:TextBox runat="server" ID="TextBox3" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" Text="14) Total Value of goods Imported Under Aproved Trader Scheme"
                            runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label16" Text="RM" runat="server" AssociatedControlID="TextBox4" />
                        <asp:TextBox runat="server" ID="TextBox4" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" Text="15) Total Value of GST Suspended Under Aproved Trader Scheme"
                            runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label17" Text="RM" runat="server" AssociatedControlID="TextBox5" />
                        <asp:TextBox runat="server" ID="TextBox5" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" Text="16) Total Value of capital goods Acquired" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label18" Text="RM" runat="server" AssociatedControlID="TextBox6" />
                        <asp:TextBox runat="server" ID="TextBox6" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" Text="17) Bad Debt Relief" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label19" Text="RM" runat="server" AssociatedControlID="TextBox7" />
                        <asp:TextBox runat="server" ID="TextBox7" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" Text="18) Bad Debt Recoverd" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label20" Text="RM" runat="server" AssociatedControlID="TextBox8" />
                        <asp:TextBox runat="server" ID="TextBox8" Text="0.00" CssClass="numeric" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divcol-actiowrap">
            <asp:Button ID="Button2" Text="Next" runat="server" CssClass="inputbtn" OnClick="Button2_Click" />
            <asp:Button ID="Button1" Text="Previous" runat="server" CssClass="inputbtn" OnClick="Button1_Click" />
        </div>
    </div>
</asp:Content>
