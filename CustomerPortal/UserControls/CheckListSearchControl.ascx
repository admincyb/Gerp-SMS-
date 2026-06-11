<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckListSearchControl.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.CheckListSearchControl" %>
<script language="javascript" type="text/javascript">
    function SearchCheckList(txtSearch, cblCtrl) {
        if ($(txtSearch).val() != "" && $(txtSearch).val().length > 3) {
            var count = 0;
            $(cblCtrl).children('tbody').children('tr').each(function () {
                var match = false;
                $(this).children('td').children('label').each(function () {
                    if ($(this).text().toUpperCase().indexOf($(txtSearch).val().toUpperCase()) > -1)
                        match = true;
                });
                if (match) {
                    $(this).show();
                    count++;
                }
                else { $(this).hide(); }
            });
            //                $('#spnCount').html((count) + ' match');
        }
        else {
            $(cblCtrl).children('tbody').children('tr').each(function () {
                $(this).show();
            });
            $('#spnCount').html('');
        }
    }
</script>
<script type="text/javascript">
    function SelectAll(evt) {
        if ($(evt).is(":checked")) {
            $("[id$=cblList] input[type=checkbox]").each(function (index) {
                var isVisible = $(this).closest("td").find("label").is(':visible');
                if (isVisible == true) {
                    $(this).attr("checked", "checked");
                }
            });
        }
        else {
            $("[id$=cblList] input[type=checkbox]").each(function (index) {
                $(this).removeAttr("checked");
            });
        }
    }
</script>
<div style="overflow: auto;">
    <asp:TextBox ID="txtSearch" runat="server" onkeyup="SearchCheckList(this,'#cblList');"
        CssClass="input-w81per margn-lft3" placeholder="Search Text">
    </asp:TextBox>
    <asp:CheckBox ID="chkAll" ToolTip="Select All" onclick="SelectAll(this);" runat="server"
        CssClass="margntop4" />
    <span id="spnCount"></span>
    <%--<asp:Label runat="server" ID="test" Text="" AssociatedControlID="" CssClass="float-left lbl-29perc margn-rgt0"></asp:Label>--%>
    <div class="treelist-scroll margn-lft3" runat="server" id="divEdocPopUp">
        <%--Id Using for  Edoc PopUp--%>
        <asp:CheckBoxList ID="cblList" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
            CssClass="treelist" ClientIDMode="Static">
        </asp:CheckBoxList>
    </div>
</div>
