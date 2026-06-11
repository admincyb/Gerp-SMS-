<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedItemWidget.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.RelatedItemWidget" %>
<%-- <div class="clear"></div>--%>
<div class="treeviewRjy" runat="server" id="divReferenceRecords" visible="false">
    <h1>
       <%=Resources.Captions.ReferenceRecords%> 
    </h1>
    <asp:TreeView ID="trvRelatedWidget" runat="server" ExpandDepth="0" SkipLinkText=""
        OnSelectedNodeChanged="ActionHandler">
        <NodeStyle Font-Bold="True" />
        <RootNodeStyle Font-Bold="True" />
        <ParentNodeStyle Font-Bold="True" />
    </asp:TreeView>
</div>
