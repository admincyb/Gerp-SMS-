<%@ Page Title="gERP -Record to Report -GST Return"  Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="Classic" CodeBehind="TaxDeclaration.aspx.cs" Inherits="ERPSMS_v01.Finance.TaxDeclaration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .inputSmall
        {
            width: 25px;
        }
        .inputMedium
        {
            width: 250px;
        }
        .inputFull
        {
            width: 400px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            GrandScriptUtils.DatePickerCommon("txtDate");
        });
    </script>
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
                            <asp:Button ID="btnNew" runat="server" Text="New" SkinID="btnInner-New" ToolTip="New" />
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
        <table style="margin-top: 0px;">
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
        <div id="grdTable-wrap">
            <h1 class="search-colapse-normal">
                PART D: DECLARATION</h1>
            <p>
                I, hereby declare that the information stated in this form are true, correct and
                complete</p>
            <table class="gridwraptable gridwrap">                
                <tr>
                    <td>
                        <asp:Label ID="Label10" Text="20) Name of Authorized Person" runat="server" AssociatedControlID="txtContent" />
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" EnableTheming="false" ID="txtContent" TabIndex="4" Width="455px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        New
                    </td>
                    <td align="left">
                        Old
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="21) Identity Card" runat="server" AssociatedControlID="TextBox2" />
                    </td>
                    <td style="width:90px">
                        <asp:TextBox runat="server" ID="TextBox2" Text="" Width="80px"/>
                    </td>
                    <td style="width:50px">
                        <asp:Label ID="Label2" Text=" - " runat="server" AssociatedControlID="TextBox3" />
                        <asp:TextBox runat="server" Width="25px" ID="TextBox3" Text="" />
                    </td>
                    <td  style="width:120px">
                        <asp:Label ID="Label3" Text=" - " runat="server" AssociatedControlID="TextBox4" />
                        <asp:TextBox runat="server" ID="TextBox4" Text="" Width="80px"/>
                    </td>
                    <td style="width:25px"><asp:Label ID="Label4" Text="OR" runat="server" AssociatedControlID="TextBox5" /></td>
                    <td>
                        <asp:TextBox runat="server" ID="TextBox5" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" Text="22) Passport No" runat="server" AssociatedControlID="TextBox6" />
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="TextBox6" Text="" Width="255px" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="5">
                        <asp:Label ID="Label6" Text="Mandatory for foreign citizen" runat="server" CssClass="inputFull" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" Text="23) Nationality" runat="server" AssociatedControlID="TextBox7" />
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="TextBox7" Text="" Width="255px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" Text="24) Date" runat="server" AssociatedControlID="txtDate" />
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="txtDate" Text="29-08-2014" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="5">
                        <asp:Label ID="Label8" Text="DD - MM - YYYY" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="divcol-actiowrap">
            <asp:Button ID="btnPrevious" Text="Previous" runat="server" CssClass="inputbtn" OnClick="btnPrevious_Click" />
        </div>
    </div>
</asp:Content>
