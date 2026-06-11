<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NumericControl.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.NumericControl" %>
    <script type="text/javascript">
        function InitAmountControl() {
            $("[id*=txtFormattedAmount]").ForceToNumeric();
        }
</script>
<asp:TextBox ID="txtFormattedAmount" runat="server" onblur="FormatAmount(this);"  ></asp:TextBox>
<asp:HiddenField ID="hdfAmountDecimals" runat="server" />
<asp:HiddenField ID="hdfIsCommaSep" runat="server" />
<asp:HiddenField ID="hdfIsNumGrp" runat="server" />
