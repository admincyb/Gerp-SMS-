<%@ Page Title="gERP -Record to Report -GST Return" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic" CodeBehind="IndustryCodes.aspx.cs" Inherits="ERPSMS_v01.Finance.IndustryCodes" %>

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
    <div class="content-wrapper" style="margin-left: 150px">
        <table style="height: 100px; margin-top: 0px;">
            <tr>
                <td style="width:100px">
                    <img src="../Images/Demo/malysia-customs.png" alt="Logo" width="100px" height="100px"/>
                </td>
                <td align="center" style="letter-spacing:2px;">
                    <div>
                       <b>JABATAN KASTAM DIRAJA MALAYSIA</b> </div>
                    <div style="margin-bottom:15px">
                       <b> ROYAL MALAYSIAN CUSTOMS DEPARTMENT</b> </div>
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
        <div id="grdTable-wrap" style="width: 600px">
            <h1 class="search-colapse-normal">
                19) BREAK DOWN VALUE OF OUTPUT TAX IN ACCORDANCE WITH MAJOR INDUSTRY CODES
            </h1>
            <table class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th align="left" style="width: 230px">
                            Code
                        </th>
                        <th colspan="2" align="left">
                            Value Of Output Tax
                        </th>
                        <th align="left" style="width: 100px">
                            Percentage
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox8" Text="00014" Width="60px" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label10" Text="RM" runat="server" AssociatedControlID="TextBox1" />
                            <asp:TextBox runat="server" ID="TextBox1" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox16" Text="0" CssClass="numeric" Width="60px" />
                            <asp:Label ID="Label18" Text="%" runat="server" AssociatedControlID="TextBox16" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox9" Text="00016" Width="60px" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label1" Text="RM" runat="server" AssociatedControlID="TextBox2" />
                            <asp:TextBox runat="server" ID="TextBox2" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox15" Text="0" CssClass="numeric" Width="60px" />
                            <asp:Label ID="Label17" Text="%" runat="server" AssociatedControlID="TextBox15" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox10" Text="00018" Width="60px" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label2" Text="RM" runat="server" AssociatedControlID="TextBox3" />
                            <asp:TextBox runat="server" ID="TextBox3" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox14" Text="0" CssClass="numeric" Width="60px" />
                            <asp:Label ID="Label16" Text="%" runat="server" AssociatedControlID="TextBox14" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox17" Text="00020" Width="60px" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label3" Text="RM" runat="server" AssociatedControlID="TextBox4" />
                            <asp:TextBox runat="server" ID="TextBox4" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox13" Text="0" CssClass="numeric" Width="60px" />
                            <asp:Label ID="Label14" Text="%" runat="server" AssociatedControlID="TextBox13" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox18" Text="00022" Width="60px" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label4" Text="RM" runat="server" AssociatedControlID="TextBox5" />
                            <asp:TextBox runat="server" ID="TextBox5" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox12" Text="0" CssClass="numeric" Width="60px" />
                            <asp:Label ID="Label12" Text="%" runat="server" AssociatedControlID="TextBox12" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" Text="Others" runat="server" Width="60px" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label5" Text="RM" runat="server" AssociatedControlID="TextBox6" />
                            <asp:TextBox runat="server" ID="TextBox6" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="TextBox11" Text="0" CssClass="numeric" Width="60px" />
                            <asp:Label ID="Label11" Text="%" runat="server" AssociatedControlID="TextBox11" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" Text="Total" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label6" Text="RM" runat="server" AssociatedControlID="TextBox7" />
                            <asp:TextBox runat="server" ID="TextBox7" Text="0.00" CssClass="numeric" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="TextBox19" Text="100" CssClass="numeric input-disabled"
                                ReadOnly="true" Width="60px" />
                            <asp:Label ID="Label13" Text="%" runat="server" AssociatedControlID="TextBox19" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="divcol-actiowrap">
        <asp:Button ID="btnNext" Text="Next" runat="server" CssClass="inputbtn" OnClick="btnNext_Click" />
        <asp:Button ID="btnPrevious" Text="Previous" runat="server" CssClass="inputbtn" OnClick="btnPrevious_Click" />
    </div>
</asp:Content>
