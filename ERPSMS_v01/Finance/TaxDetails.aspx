<%@ Page Title="gERP -Record to Report -Tax Settings"  Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="TaxDetails.aspx.cs" Inherits="ERPSMS_v01.Finance.TaxDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .both
        {
            display: table-row;
        }
        .purchase
        {
            display: table-row;
        }
        .Supply
        {
            display: table-row;
        }
        img
        {
            width: 24px;
            height: 24px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#bothIconToggler').click(bothClick);
            $('#purchaseIconToggler').click(purchaseClick);
            $('#supplyIconToggler').click(supplyClick);
        });

        function bothClick() {
            $('.both').toggle();
            $(this).find('img').toggle();
        }
        function purchaseClick() {
            $('.purchase').toggle();
            $(this).find('img').toggle();
        }
        function supplyClick() {
            $('.Supply').toggle();
            $(this).find('img').toggle();
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
                            <asp:Label ID="lblBreadCrum" runat="server" Text="Finance <label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label> Tax Settings " />
                        </ul>
                        <ul runat="server" id="pnlEntry">
                            <li>
                                <asp:Button runat="server" Text="New" SkinID="btnInner-New" ToolTip="New" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnEdit" Text="<%$resources:Controls,Edit %>" 
                                    SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>" />
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
        <div id="grdTable-wrap">
            <h1 class="search-colapse-normal">
                TAX SETTINGS
            </h1>
            <table class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                        </th>
                        <th>
                            Tax Code
                        </th>
                        <th>
                            Tax Rate
                        </th>
                        <th>
                            Tax Description
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="5">
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <div id="supplyIconToggler">
                                <img id="img3" src="../Images/Demo/plus-small.png" alt="Supply" style="display: none;
                                    width: 22px; height: 22px;" />
                                <img id="img4" src="../Images/Demo/minus-small.png" alt="Supply" />
                            </div>
                        </th>
                        <th colspan="4">
                            Supply Tax Code
                        </th>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            SR
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Standared-rated supplies with GST charged
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton1" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            ZRL
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Local supply of goods and services which are subject to Zero rated supplies.
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton2" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            ZRE
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Exporation of goods and services which are subject to Zero rated supplies.
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton3" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            ES43
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Incidental Exempt supplies.
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton4" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            DS
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Deemed supplies
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton5" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            OS
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Out-of-scope supplies
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton6" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            ES
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Exempt supplies under GST
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton7" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            RS
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Relief supply under GST
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton8" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            GS
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Disregarded supplyies.
                        </td>
                    </tr>
                    <tr class="Supply">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton9" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            AGS
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Any adjustment made to Output Tax
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <div id="purchaseIconToggler">
                                <img id="img1" src="../Images/Demo/plus-small.png" alt="Purchase" style="display: none;
                                    width: 22px; height: 22px;" />
                                <img id="img2" src="../Images/Demo/minus-small.png" alt="Purchase" />
                            </div>
                        </th>
                        <th colspan="4">
                            Purchase Tax Code
                        </th>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton10" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TX
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Purchase with GST incurred at 6% and directly attribuitable to taxable supplies
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton11" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            BL
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Purchase with GST incurred but no claimable (Disallowance of Input Tax)
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton12" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            NR
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Purchase from non GST registered supplier with no GST incurred
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton13" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            ZP
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Purchase from GST registered supplier with no GST incurred
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton14" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            EP
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Purchase exempted from GST
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton15" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            OP
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Purchase transactions which is out of the scope of GST legislation
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton16" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TX-E43
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Purchase with GST incurred directly attribuitable to incidental exempt supplies
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton17" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TAX-N43
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Purchase with GST incurred directly attribuitable to incidental exempt supplies
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton18" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TAX-RE
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Purchase with GST incurred that is not directly attribuitable to taxable or exempt
                            supplies
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton19" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            GP
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Purchase transactions wich disregarded under GST legislation
                        </td>
                    </tr>
                    <tr class="purchase">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton20" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            AGP
                        </td>
                        <td>
                            6%
                        </td>
                        <td>
                            Any adjustment made to Input Tax
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <div id="bothIconToggler">
                                <img id="imgBothPlus" src="../Images/Demo/plus-small.png" alt="Both" style="display: none;
                                    width: 22px; height: 22px;" />
                                <img id="imgBothMinus" src="../Images/Demo/minus-small.png" alt="Both" />
                            </div>
                        </th>
                        <th colspan="4">
                            Both
                        </th>
                    </tr>
                    <tr class="both">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton21" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TS
                        </td>
                        <td>
                            10%
                        </td>
                        <td>
                            Standared Rated
                        </td>
                    </tr>
                    <tr class="both">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton22" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TZ
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Zero Rated
                        </td>
                    </tr>
                    <tr class="both">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton23" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TE
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Exempt
                        </td>
                    </tr>
                    <tr class="both">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton24" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TI
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Imports
                        </td>
                    </tr>
                    <tr class="both">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton25" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TD
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Disallowed
                        </td>
                    </tr>
                    <tr class="both">
                        <td>
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton26" runat="server" GroupName="Edit" />
                        </td>
                        <td>
                            TP
                        </td>
                        <td>
                            0%
                        </td>
                        <td>
                            Partial Rate
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
