<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkflowUserComments.ascx.cs"
    Inherits="gComs.WorkFlow.WorkflowUserComments" %>
<script type="text/javascript">


    function ValidateNow(validationGroup) {

        if (typeof (Page_ClientValidate) == 'function') {
            Page_ClientValidate(validationGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#divWrkferror").html(), '<%= Resources.Messages.Information %>');
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

    function ShowComments() {

        var currentHeight = $("[id$=wrkDiv]").height();
        GrandScriptUtils.ShowWorkFlowCommandList();
        var newHeight = $("[id$=wrkDiv]").height();
        var frameHeight = $("[id$=frmDetails]", parent.document).height();
        if (frameHeight != null) {
            $("[id$=frmDetails]", parent.document).height((frameHeight - currentHeight) + newHeight);
            $("[id$=innerPage-wrap]").height((frameHeight - currentHeight) + newHeight);
        }
    }

    function HideComments() {

        var currentHeight = $("[id$=wrkDiv]").height();
        GrandScriptUtils.HideWorkFlowCommandList();
        var newHeight = $("[id$=wrkDiv]").height();
        var frameHeight = $("[id$=frmDetails]", parent.document).height();
        if (frameHeight != null) {
            $("[id$=frmDetails]", parent.document).height((frameHeight - currentHeight) + newHeight);
            $("[id$=innerPage-wrap]").height((frameHeight - currentHeight) + newHeight);
        }
    }

    function CheckIsWkfMultipleClick() {
        if ($("[id$=hdfIsWkfMultipleClick]").val() == "0") {
            $("[id$=hdfIsWkfMultipleClick]").val("1");
            return true;
        }
        else
            return false;
    }

</script>
<div class="content-wrapper">
    <div id="divActionComments" runat="server" class="comments-container">
        <div class="submitwrap commentswrap" id="div1" runat="server">
            <label for="WRKFACT_ID">
                <%= Resources.Controls.Action%></label><asp:DropDownList runat="server" ID="WRKFACT_ID"
                    Width="320px" onmouseover="javascript:ShowTooltip('WRKFACT_ID');">
                </asp:DropDownList>
            <asp:Button runat="server" ID="btnSubmit" ToolTip="<%$Resources:Controls,Submit %>"
                Text="<%$ Resources:Controls, Submit %>" OnClick="wrkfSubmit_Click" CommandName="WRKFSUBMIT" OnClientClick="return CheckIsWkfMultipleClick();"
                SkinID="btnInner-submit" />
            <div class="clear">
            </div>
            <asp:Label ID="lblComments" runat="server" AssociatedControlID="WrkfComments" Text="<%$ resources:Controls,Comments %>"></asp:Label>
            <%--<label for="WrkfComments"><%= Resources.Controls.Comments%></label>--%>
            <asp:TextBox ID="WrkfComments" runat="server" CssClass="multiline-2line" TextMode="MultiLine"></asp:TextBox>
        </div>
        <%--Page Comments --%>
        <div id="divPageComments" runat="server" class="div-notify" >
            <asp:Label ID="lblPageComment" Text="" runat="server" ></asp:Label>
        </div>
    </div>

    <h4>
        <%=Resources.Controls.Log%>
    </h4>
    <div id="divWrkfComment" class="gridwrap max-250">
        <asp:GridView runat="server" ID="grdComments" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
            <EmptyDataTemplate>
                <asp:Label ID="lblEmptyComment" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="cmtDate" HeaderText="<%$ Resources:Controls, Date %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="24%" />
                <asp:BoundField DataField="acnName" HeaderText="<%$ Resources:Controls, Action %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="23%" />
                <asp:BoundField DataField="cmtModByText" HeaderText="<%$ Resources:Controls, User %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="18%" />
                <asp:BoundField DataField="cmtDesc" HeaderText="<%$ Resources:Controls, Comments %>"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="35%" />
            </Columns>
        </asp:GridView>
    </div>
    <div id="divWrkferror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vvswrkfSummary" ValidationGroup="Save" runat="server" />
        <asp:HiddenField ID="hdfIsWkfMultipleClick" Value="0" runat="server" />
    </div>
</div>
