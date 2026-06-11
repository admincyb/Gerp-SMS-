<%@ Page Title="gERP -Record to Report -GAF File" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="GAFFile.aspx.cs" Inherits="ERPSMS_v01.Finance.GAFFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            GrandScriptUtils.DatePickerCommon("txtStartDate");
            GrandScriptUtils.DatePickerCommon("txtToDate");
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
                            <asp:Label ID="lblBreadCrum" runat="server" Text="Finance<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>GAF File" />
                        </ul>
                        <ul runat="server" id="pnlEntry">
                         <li>
                                <asp:Button runat="server" ID="btnViewText" Text="<%$resources:Controls,GeneratePipe %>"
                                    SkinID="btnInner-View" ToolTip="View" OnClick="ActionHandler" CommandName="GENERATE" 
                                    ValidationGroup="gaf"/>
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnView" Text="<%$resources:Controls,Generate %>"
                                    SkinID="btnInner-View" ToolTip="View" OnClick="ActionHandler" CommandName="GENERATE" 
                                    ValidationGroup="gaf"/>
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" 
                                    ToolTip="<%$resources:Controls,Cancel %>" />
                            </li>
                        </ul>
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="Label2" Text="Select Company" runat="server" AssociatedControlID="ddlCompany" />
                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="select-half">
                            </asp:DropDownList>
                        </div>
                    </td>
                     <td>
                        <div class="div2col-S">
                           <asp:Label ID="Label16" Text="From Date" runat="server" AssociatedControlID="txtStartDate" />
                            <asp:TextBox runat="server" ID="txtStartDate" Text="" CssClass="input-small" />
                            <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true" ValidationGroup="gaf"
                                EnableClientScript="true" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDt %>">
                            </asp:RequiredFieldValidator>

                             <asp:Label ID="Label1" Text="To Date" CssClass="middle-lbl" runat="server" AssociatedControlID="txtToDate" />
                            <asp:TextBox runat="server" ID="txtToDate" Text="" CssClass="input-small" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true" ValidationGroup="gaf"
                                EnableClientScript="true" runat="server" ControlToValidate="txtToDate"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDt %>">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>                
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 150px">
                        <asp:RadioButton ID="rdoCompanyInfo" Text="Company Info" runat="server" GroupName="GSTGrp" />
                    </td>
                    <td style="padding-left: 150px">
                        <asp:RadioButton ID="rdoGeneralLedger" Text="General Ledger" runat="server" GroupName="GSTGrp" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 150px">
                        <asp:RadioButton ID="rdoPurchase" Text="Purchase" runat="server" GroupName="GSTGrp" />
                    </td>
                    <td style="padding-left: 150px">
                        <asp:RadioButton ID="rdoSupply" Text="Supply" runat="server" GroupName="GSTGrp" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 150px">
                        <asp:RadioButton ID="rdoAll" Text="All" runat="server" GroupName="GSTGrp" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
