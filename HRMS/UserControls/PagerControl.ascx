<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerControl.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.PagerControl" %>
<%--<br clear="all" />--%>
<div class="gridpagingWrapper">

       <div class="prevnext-wrap">
                <asp:ImageButton ID="btnNext" runat="server" Enabled="false" SkinID="imbGridNext" />
           </div>
            <asp:Label ID="lblCurrentPage" runat="server" Visible="false"></asp:Label>
            <div class="select-wrap">
                <asp:DropDownList ID="ddPage" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="prevnext-wrap">
                <asp:ImageButton ID="btnPrevious" runat="server" Enabled="false" SkinID="imbGridPrev" />
                <asp:Label ID="lblTotalPages" runat="server" Visible="false"></asp:Label>
            </div>
       
 
    <br clear="all" />
</div>
