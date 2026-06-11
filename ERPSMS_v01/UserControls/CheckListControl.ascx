<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckListControl.ascx.cs" Inherits="ERPSMS_v01.UserControls.CheckListControl" %>

<script type="text/javascript" language="javascript">
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
</script>
<div class="fields-grpwrap color-grey grp-before color-white" runat="server" id="divInspectionCheckList">
   <div class="header">
     <h1>
        <asp:Literal ID="ltlTitle" runat ="server" ></asp:Literal>
     </h1>
     <div class="fields-right">
            <asp:HiddenField ID="hdnControlCount" runat ="server" value="0"/>
            <label for="ddlGroup"><%=Resources.Controls.Group%></label>
            <asp:DropDownList ID="ddlGroup" runat ="server" AutoPostBack ="true" CssClass ="medium-a"  OnSelectedIndexChanged="ActionHandler" TabIndex ="5"></asp:DropDownList>
     </div> 
     <div class="clear"></div>
   </div>
   <div class="fields-group">
          <div id="divDetails" runat="server">&nbsp;</div>
                <asp:Panel ID="pnlControls" runat="server">
                      <%--The UI controls will bind here--%>
                </asp:Panel>
   </div>
</div> 
