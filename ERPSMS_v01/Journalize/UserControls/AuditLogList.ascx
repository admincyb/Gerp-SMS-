<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuditLogList.ascx.cs" Inherits="ERPSMS_v01.Journalize.UserControls.AuditLogList" %>
<script type="text/javascript" language="javascript">
    function ChangeRowColor(row, version, pk, rowIndex, Type) {
        var rows = row.parentNode.getElementsByTagName('TR');
        //loop over all rows and set there colors to default
        for (var i = 0; i < rows.length; i++) { 
            rows[i].style.backgroundColor = 'White'; //if its your default color 
        }
        //if ($("[id$=hdfSelRow]").val() != "0") {
        //    row = parseInt($("[id$=hdfSelRow]").val());
        //}
        //set the current row to be with the needed color
        row.style.backgroundColor = "YELLOW";

        $("[id$=hdfSelRowVer]").val(version);
        $("[id$=hdfSelRowTranPk]").val(pk);
        $("[id$=hdfSelRow]").val(rowIndex);
        if (Type == "DPVJ") { $("[id$='btnVoucherComparision']").click(); }
        else if (Type == "DRVJ") { $("[id$='btnComparision']").click(); }
        else {
            $("[id$='btnJournalComparision']").click();
        }
    }

</script>
<div class="gridwrap" style="overflow-x: hidden !important;">
    <asp:GridView runat="server" ID="grdAuditLog" Width="100%" AutoGenerateColumns="false"
        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdAuditLog_RowDataBound">
        <EmptyDataTemplate>
            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Version">
                <ItemTemplate>
                    <asp:Label ID="lblAuditVersion" runat="server" Text='<%# Eval("FTH_AUDIT_VERSION") %>'
                        ToolTip='<%# Eval("FTH_AUDIT_VERSION") %>'></asp:Label><asp:HiddenField ID="hdfFTH_PK" runat="server" Value='<%# Eval("FTH_PK") %>' />
                    <asp:HiddenField ID="hdfRowIndex" runat="server" Value='<%#Container.DataItemIndex %>' />
                    <asp:HiddenField ID="hdfRefType" runat="server" Value='<%# Eval("FTH_REF_TYPE") %>' />
                </ItemTemplate>
                <ItemStyle Width="1%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Activity">
                <ItemTemplate>
                    <asp:Label ID="lblActivity" runat="server" Text='<%# Eval("FTH_ACTIVITY") %>'
                        ToolTip='<%# Eval("FTH_ACTIVITY") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="45%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Username">
                <ItemTemplate>
                    <asp:Label ID="lbUsername" runat="server" Text='<%# Eval("FTH_MOD_BY_TEXT") %>'
                        ToolTip='<%# Eval("FTH_MOD_BY_TEXT") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="8%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date & Time">
                <ItemTemplate>
                    <asp:Label ID="lblDateTime" runat="server" Text='<%# Eval("FTH_MOD_DT",Resources.ErpRes.GridFormatDatetime) %>'
                        ToolTip='<%# Eval("FTH_MOD_DT",Resources.ErpRes.GridFormatDatetime) %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="28%" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <%-- <asp:Button ID="btnAssign" runat="server" OnClick="ActionHandler" CommandName="ASSIGN"
            EnableTheming="false" Style="display: none" />--%>
    <asp:HiddenField ID="hdfSelRowVer" runat="server" Value="0" />
    <asp:HiddenField ID="hdfSelRowTranPk" runat="server" Value="0" />
    <asp:HiddenField ID="hdfSelRow" runat="server" Value="0" />
</div>
