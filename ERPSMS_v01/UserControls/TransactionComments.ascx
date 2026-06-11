<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionComments.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.TransactionComments" %>
<script type="text/javascript">
    function InitComponentsCmnts() {
        GrandScriptUtils.DatePickerCommon("txtTrxCmntDate");
    }

    function ValidateTrxCmntsPage(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#divTrxCmntError").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }
</script>
<div class="content-wrapper">
    <div class="detail-co2">
        <div class="div2col-S">
            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:Controls,TrxNo%>"
                AssociatedControlID="lblTrxNo" Font-Bold="true"></asp:Label>
            <asp:Label ID="lblTrxNo" runat="server"></asp:Label>
        </div>
        <div class="div2col-S">
            <asp:Label ID="lblTrxHdr" runat="server" Text="<%$ resources:Controls,Trx %>" AssociatedControlID="lblTrx"
                Font-Bold="true"></asp:Label>
            <asp:Label ID="lblTrx" runat="server"></asp:Label>
        </div>
        <div class="clear">
        </div>
    </div>
    <div id="SEC_UcrTransCommentsPanel" runat="server" class="comments-container bg-none">
        <div class="submitwrap commentswrap">
            <label for="lblTrxComments">
                <%=Resources.Controls.Comments %></label>
            <asp:TextBox ID="txtTrxComments" runat="server" TextMode="MultiLine" Height="35px"
                MaxLength="500" TabIndex="500" EnableTheming="false" onkeydown="limitText(this,500);"
                onchange="limitText(this,500);"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vrfTrxComments" CssClass="star" SetFocusOnError="true"
                ValidationGroup="TrnxSave" EnableClientScript="true" runat="server" ControlToValidate="txtTrxComments"
                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_Comments %>">
            </asp:RequiredFieldValidator>
            <div class="clear">
            </div>
            <label for="lblTrxCmntDate">
                <%=Resources.Controls.Date %></label>
            <asp:TextBox runat="server" ID="txtTrxCmntDate" CssClass="input-w13-8per input-disabled" TabIndex="501" Enabled="false"
                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
            <asp:RequiredFieldValidator ID="vrfTrxCmntDate" CssClass="star" SetFocusOnError="true"
                ValidationGroup="TrnxSave" EnableClientScript="true" runat="server" ControlToValidate="txtTrxCmntDate"
                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:ErrorMessages,Err_Date %>">
            </asp:RequiredFieldValidator>
            <asp:Button runat="server" ID="btnCmntSave" ToolTip="<%$Resources:Controls,Save %>"
                TabIndex="502" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save %>" OnClick="ActionHandler"
                CommandName="SAVE" ValidationGroup="TrnxSave" OnClientClick="javascript:ValidateTrxCmntsPage('TrnxSave')" />
        </div>
    </div>
    <h4>
        <%=Resources.Controls.Log%>
    </h4>
    <div class="clear">
    </div>
    <div class="gridwrap scroll-h180">
        <asp:GridView runat="server" ID="grdTrxCommentsList" AutoGenerateColumns="false" OnRowDataBound="ActionHandler" 
            EmptyDataRowStyle-CssClass="emptytable">
            <RowStyle CssClass="rowcolor1" />
            <AlternatingRowStyle CssClass="rowcolor2" />
            <EmptyDataTemplate>
                <asp:Label ID="lblCmntEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="<%$ resources:Controls,Date %>">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdfCurrCmntPk" runat="server" Value='<%#Eval("ACM_PK") %>' />
                        <asp:HiddenField ID="hdfLastModDate" runat="server" Value='<%#Eval("ACM_MOD_DT") %>' />
                        <asp:Label ID="lblgrdTrxCmntDate" runat="server" Text='<%# Eval("ACM_CRTD_DT", Resources.Constants.DateTimeFormatGrid)%>'
                            ToolTip='<%# Eval("ACM_CRTD_DT", Resources.Constants.DateTimeFormatGrid)%>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="false" />
                    <ItemStyle Width="8%" Wrap="false" CssClass="colcolor1" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Controls,CommentedBy %>">
                    <ItemTemplate>
                        <asp:Label ID="lblgrdTrxCmntBy" runat="server" Text='<%# Eval("ACM_CRTD_BY_TEXT") %>'
                            ToolTip='<%# Eval("ACM_CRTD_BY_TEXT") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Controls,Comments %>">
                    <ItemTemplate>
                        <asp:Label ID="lblgrdTrxCmnt" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ACM_COMMENT"), 500) %>'
                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ACM_COMMENT"), 500) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="72%" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btngrdTrxEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                            CommandArgument="PageAction_Entry" SkinID="imbeditgrid" ToolTip="Edit" TabIndex="503"
                            Visible='<%# (Convert.ToInt32(Eval("ACM_IS_CREATED_USR")) == 1)? true : false %>' />
                        <asp:ImageButton ID="btngrdTrxRemoveItem" runat="server" OnClick="ActionHandler"
                            CommandName="DELETEITEM" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                            SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="503" Visible='<%# (Convert.ToInt32(Eval("ACM_IS_CREATED_USR")) == 1)? true : false %>' />
                    </ItemTemplate>
                    <ItemStyle Width="5%" Wrap="false" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div id="divTrxCmntError" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vsTrnxSave" ValidationGroup="TrnxSave" runat="server" />
    </div>
    <div class="clear">
    </div>
</div>
