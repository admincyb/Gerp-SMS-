<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploader.ascx.cs" Inherits="ERPSMS_v01.UserControls.FileUploader" %>
<div class="divcol-FileuplWrap">
    <label for="aupDocument" style="width: 100px">
        <%= Resources.Controls.Attachments %></label>
    <div id="FileUploader">
        <asp:FileUpload ID="flpUploader" runat="server" size="29" Height="22px" Style="margin-top: 3px" TabIndex="47"/>
        <asp:Label ID="Label1" runat="server" Text="Title" />
        <asp:TextBox ID="txtFlpFileTite" runat="server" CssClass="w-250" TabIndex="48" MaxLength="200"/>
        <asp:Button ID="btnUploadFile" runat="server" Text="Upload" Width="60px" OnClick="ActionHandler"
            CommandName="ADDFILE" TabIndex="49"/>
        <asp:HiddenField ID="FileUploadList" runat="server" />
    </div>
</div>
<div class="grdTable">
    <asp:GridView ID="grdUploadedFiles" runat="server" AllowPaging="false" Width="100%" AllowSorting="false"
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
                    <asp:TextBox runat="server" id="txtFileTitle" Text='<%#Eval("Title")%>' CssClass="w-250" MaxLength="200" />  
                </ItemTemplate>
                <ItemStyle Width="25%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, FileName %>" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%#Eval("FileName")%>
                </ItemTemplate>
                <ItemStyle Width="60%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$ Resources:Controls, Action %>" ItemStyle-Width="10%"
                HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>                  
                    <a  class="download-icon nomargin" href='<%# Page.ResolveUrl(BindFileLinkNew(Eval("FilePath"))) %>' target="_blank" title="View" tabindex="49"></a>
                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" CssClass="_delete"
                        EnableViewState="false" OnClick="ActionHandler" CommandName="DELETE_ACTION" ToolTip="Delete"  TabIndex="50" OnClientClick="return ShowDeleteConfirm(this);"/>  
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>    
</div>
