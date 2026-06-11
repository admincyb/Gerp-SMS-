<%@ Page Title="gERP -Record to Report -GST Return" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic" CodeBehind="TaxReturnDetails.aspx.cs"
    Inherits="ERPSMS_v01.Finance.TaxReturnDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table-6divide
        {
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            GrandScriptUtils.DatePickerCommon("txtStartDate");
            GrandScriptUtils.DatePickerCommon("txtEndDate");
            GrandScriptUtils.DatePickerCommon("txtReturnableDate");
            initializeField();
        });

        function initializeField() {
            $('#TextBox').val('0.00');
            $('#TextBox1').val('0.00');
            $('#TextBox2').val('0.00');
            $('#TextBox3').val('0.00');
            $('#TextBox4').val('0.00');
            $('#TextBox5').val('0.00');
        }

        function btnGoClick() {
            $('#TextBox').val('10000.00');
            $('#TextBox1').val('600.00');
            $('#TextBox2').val('218654.00');
            $('#TextBox3').val('13119.24');
            $('#TextBox4').val('0.00');
            $('#TextBox5').val('12519.24');
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
                            <asp:Label ID="lblBreadCrum" runat="server" Text="Finance<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>GST Return" />
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
                                <asp:Button runat="server" TabIndex="21" ID="btnPrint" CommandName="PRINT" Text="<%$resources:Controls,Print %>" OnClick="ActionHandler"
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
                <td style="width: 150px;padding-top:15px" align="center" valign="middle">
                    <div style="background-color: #DEE3ED; color: #506C92; font-weight: bold;height: 25px;margin-bottom:5px ">
                        GST - 03
                    </div>
                    <div>
                        <input type="text" style="height: 25px; width: 150px; font-size: large; text-align: center;"
                            value="GST00002" />
                    </div>
                </td>
            </tr>
            <tr>
            <td>
                <asp:CheckBox ID="CheckBox1" Text="Amendment" runat="server" />
            </td>
            </tr>
        </table>
        <div>
            <h1 class="search-colapse-normal">
                PART A: REGISTERED PERSON DETAILS</h1>
        </div>
        <div id="grdTable-wrap">
            <table class="gridwraptable gridwrap">
                <tr>
                    <td>
                        <asp:Label ID="Label13" Text="1) GST No" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="TextBox6" Text="7474747gst" Width="300px" CssClass="input-disabled" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label15" Text="2) Name of Business" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="TextBox7" Text="GTI Glove & Latex" Width="590px"
                            CssClass="input-disabled" />
                    </td>
                </tr>
            </table>
            <div>
                <h1 class="search-colapse-normal">
                    PART B: RETURN DETAILS
                </h1>
            </div>
            <table class="gridwraptable gridwrap">
                <tr>
                    <td>
                        <asp:Label ID="Label14" Text="3) Taxable Period" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label16" Text="Start Date" runat="server" AssociatedControlID="txtStartDate" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtStartDate" Text="01/08/2014" />
                    </td>
                    <td>
                        <asp:Label ID="Label17" Text="End Date" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEndDate" Text="29/08/2014" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnGo" Text="Go" runat="server" ClientIDMode="Static" OnClientClick="return btnGoClick();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label18" Text="4) Returnable and Payable Due Date" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtReturnableDate" Text="31/08/2014" />
                    </td>
                    <td colspan="3">
                    </td>
                </tr>
            </table>
            <%-- <h3>
                GOODS AND SERVICES TAX RETURN</h3>--%>
            <table class="gridwraptable gridwrap">
                <tr>
                    <th align="left">
                        5) OUTPUT TAX
                    </th>
                    <th class="amount-numeric" style="width: 165px">
                        Amount
                    </th>
                    <th style="width: 145px">
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="Total Value of Standared Rated Supply" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label2" Text="RM" runat="server" AssociatedControlID="TextBox" />
                        <asp:TextBox runat="server" ID="TextBox" Text="0.00" CssClass="numeric" ClientIDMode="Static" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" Text="Total Output Tax (Supply x GST Rate)" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label4" Text="RM" runat="server" AssociatedControlID="TextBox1" />
                        <asp:TextBox runat="server" ID="TextBox1" Text="0.00" CssClass="numeric" ClientIDMode="Static" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <th align="left">
                        6) INPUT TAX
                    </th>
                    <th class="amount-numeric">
                        Amount
                    </th>
                    <th>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label Text="Total Value of Standared Rated Acquision" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label5" Text="RM" runat="server" AssociatedControlID="TextBox2" />
                        <asp:TextBox runat="server" ID="TextBox2" Text="0.00" CssClass="numeric" ClientIDMode="Static" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" Text="Total Input Tax (Acquision x GST Rate)" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label10" Text="RM" runat="server" AssociatedControlID="TextBox3" />
                        <asp:TextBox runat="server" ID="TextBox3" Text="0.00" CssClass="numeric" ClientIDMode="Static" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" Text="7) GST Amount Payable" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label11" Text="RM" runat="server" AssociatedControlID="TextBox4" />
                        <asp:TextBox runat="server" ID="TextBox4" Text="0.00" CssClass="numeric" ClientIDMode="Static" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" Text="8) GST Amount Claimable" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label12" Text="RM" runat="server" AssociatedControlID="TextBox5" />
                        <asp:TextBox runat="server" ID="TextBox5" Text="0.00" CssClass="numeric" ClientIDMode="Static" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" Text="9) Do you choose carry forward refund?" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Checked="false" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <div class="divcol-actiowrap">
            <asp:Button ID="btnNext" Text="Next" runat="server" CssClass="inputbtn" OnClick="btnNext_Click" />
        </div>
    </div>
</asp:Content>
