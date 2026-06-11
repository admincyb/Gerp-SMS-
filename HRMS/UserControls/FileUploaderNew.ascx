<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploaderNew.ascx.cs"
    Inherits="HRMS.UserControls.FileUploaderNew" %>
<script type="text/javascript">
    function AlertMessage(msg, title) {
        GrandScriptUtils.ShowModal(msg, title);
    }    
</script>
<div class="divcol-FileuplWrap min-height16">
    <label for="aupDocument" class="margntop3 margnbotm0">
        <%= Resources.Controls.Attachments %></label>
    <div id="FileUploader" runat="server" visible="false">
        <asp:FileUpload ID="flpUploader" runat="server" size="29" Height="22px" Style="margin-top: 3px"
            TabIndex="4" />
        <asp:RequiredFieldValidator runat="server" ID="rfvflpUploader" ValidationGroup="Upload"
            ControlToValidate="flpUploader" CssClass="star" Text="*" ErrorMessage="<%$ Resources:ErrorMessages, Msg_File_Required %>" />
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S">
                        <asp:Label ID="lblFlpFileTitle" runat="server" Text="Title" AssociatedControlID="txtFlpFileTite"
                            CssClass="lbl" />
                        <asp:TextBox ID="txtFlpFileTite" runat="server" TabIndex="4" MaxLength="200" CssClass="input-half"
                            ValidationGroup="Upload" CausesValidation="true" />
                        <div class="starwrap">
                            <asp:RequiredFieldValidator runat="server" ID="rfvFlpFileTitle" ValidationGroup="Upload"
                                ControlToValidate="txtFlpFileTite" CssClass="star" Text="*" ErrorMessage="<%$ Resources:ErrorMessages, Msg_FileTitle_Required %>" />
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <asp:Label ID="lblFlpFileDescription" runat="server" Text="File Description" AssociatedControlID="txtFlpFileDescription"
                            CssClass="lbl" />
                        <asp:TextBox ID="txtFlpFileDescription" runat="server" TabIndex="4" CssClass="input-medium-a"
                            MaxLength="500" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="ActionHandler" SkinID="btnInner-upload"
                            CssClass="padd-left25" CommandName="ADD" TabIndex="4" ValidationGroup="Upload" ToolTip="<%$ resources:Controls,Upload%>"
                            OnClientClick="javascript:ValidatePageNow('Upload')" />
                        <asp:HiddenField ID="FileUploadList" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="grdTable">
    <asp:GridView ID="grdUpload" runat="server" AllowPaging="false" Width="100%" AllowSorting="false"
        AutoGenerateColumns="false" CssClass="grdTable" OnRowDataBound="ActionHandler">
        <Columns>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, No %>" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%#(Container.DataItemIndex + 1)%>
                    <asp:HiddenField ID="hdfActName" runat="server" Value='<%#Eval("FilePath")%>' />
                </ItemTemplate>
                <ItemStyle Width="5%" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, FileTitle %>" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtFileTitle" Text='<%# HttpUtility.HtmlDecode(Eval("Title").ToString())%>'
                        CssClass="w-250" MaxLength="200" ToolTip='<%# HttpUtility.HtmlDecode(Eval("Title").ToString())%>'/>
                </ItemTemplate>
                <ItemStyle Width="25%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, FileName %>" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblFilePath" Text='<%#Eval("FileName")%>' runat="server" ToolTip='<%#Eval("FileName")%>' />
                </ItemTemplate>
                <ItemStyle Width="20%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, Description %>" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblFileDescription" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval("FileDescription").ToString())%>'
                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("FileDescription").ToString()),50) %>' />
                </ItemTemplate>
                <ItemStyle Width="40%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, Action %>" ItemStyle-Width="10%"
                HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a class="download-icon nomargin" href='<%# Page.ResolveUrl(BindFileLinkNew(Eval("FilePath"))) %>'
                        target="_blank" title="View" tabindex="49"></a>
                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" CssClass="_delete"
                        OnClick="ActionHandler" CommandName="DELETE_ACTION" ToolTip="<%$ resources:Controls,Delete%>"
                        TabIndex="50" OnClientClick="return ShowDeleteConfirm(this);" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
