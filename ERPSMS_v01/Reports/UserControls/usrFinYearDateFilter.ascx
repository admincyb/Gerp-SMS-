<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrFinYearDateFilter.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.usrFinYearDateFilter" %>

<script type="text/javascript">
    function usrInitComponents() {
        GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
        RestrictDate();
    }
    function RestrictDate() {
        GrandScriptUtils.RestrictedDatePicker("txtFromDate", false, true, true, $("[id$=txtFrom]").val(), $("[id$=txtTo]").val());
        GrandScriptUtils.RestrictedDatePicker("txtToDate", false, true, true, $("[id$=txtFrom]").val(), $("[id$=txtTo]").val());
    }
</script>

<div id="divUsrTrialBal" runat="server">
    <table class="table-devide tablelayout">
        <tr>
            <td width="50%">
                <div  class="padgtop7">
                    <div id="divDate" runat="server">
                        <asp:Label ID="Label2" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" CssClass="middle-lbl-xsmall-c4"
                            AssociatedControlID="ddlFinYear"></asp:Label>
                        <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="medium"
                            OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                        </asp:DropDownList>

                        <asp:Label runat="server" ID="lblDateFrom" Text="From" AssociatedControlID="txtFromDate" CssClass="middle-lbl-xsmall-c3"></asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" MaxLength="100" CssClass="Uidate-picker" onkeydown="return false" onpaste="return false" />
                        <asp:HiddenField ID="hdfFromDate" runat="server" />

                        <asp:Label runat="server" ID="lblDateTo" Text="To" AssociatedControlID="txtToDate" CssClass="middle-lbl-xsmall-c3"></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" MaxLength="100" CssClass="Uidate-picker" onkeydown="return false" onpaste="return false" />
                        <asp:HiddenField ID="hdfToDate" runat="server" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display:none">
    <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
</div>
