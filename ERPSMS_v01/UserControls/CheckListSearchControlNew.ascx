<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckListSearchControlNew.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.CheckListSearchControlNew" %>
<script language="javascript" type="text/javascript">
    function SearchCheckListItem(txtSearchItem, cblCtrl) {
        if ($(txtSearchItem).val() != "" && $(txtSearchItem).val().length > 3) {
            var count = 0;
            $(cblCtrl).children('tbody').children('tr').each(function () {
                var match = false;
                $(this).children('td').children('label').each(function () {
                    if ($(this).text().toUpperCase().indexOf($(txtSearchItem).val().toUpperCase()) > -1)
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
    function SelectAllItem(evt) {
        if ($(evt).is(":checked")) {
            $("[id$=cblListItem] input[type=checkbox]").each(function (index) {
                var isVisible = $(this).closest("td").find("label").is(':visible');
                if (isVisible == true) {
                    $(this).attr("checked", "checked");
                }
            });
        }
        else {
            $("[id$=cblListItem] input[type=checkbox]").each(function (index) {
                $(this).removeAttr("checked");
            });
        }
    }
</script>
<div style="overflow: auto;">
    <asp:TextBox ID="txtSearchItem" runat="server" onkeyup="SearchCheckListItem(this,'#cblListItem');"
        CssClass="input-w81per margn-lft3" placeholder="Search Text">
    </asp:TextBox>
    <asp:CheckBox ID="chkAllItem" ToolTip="Select All" onclick="SelectAllItem(this);" runat="server"
        CssClass="margntop4" />
    <span id="spnCount"></span>
    <%--<asp:Label runat="server" ID="test" Text="" AssociatedControlID="" CssClass="float-left lbl-29perc margn-rgt0"></asp:Label>--%>
    <div class="treelist-scroll margn-lft3" runat="server" id="divEdocPopUp">
        <%--Id Using for  Edoc PopUp--%>
        <asp:CheckBoxList ID="cblListItem" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
            CssClass="treelist" ClientIDMode="Static">
        </asp:CheckBoxList>
    </div>
</div>
