<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerControl.ascx.cs" Inherits="ERP.UserControl.PagerControl" %>

<div class="gridpagingWrapper">
<table>
<tr>
    <td width="85%">
    <div class="page-navigation">
         
        <asp:label id="lblCurrentPage" Runat="server" Visible="false"></asp:label>
        <span style="width:auto;">Page</span> 
        <asp:DropDownList Width="50px" id="ddPage" runat="server" AutoPostBack="true"></asp:DropDownList>
        <span style="width:auto;">of</span> 
        <asp:label id="lblTotalPages" Runat="server"></asp:label>
        <span style="width:auto;">-</span> 
          <asp:label id="lblTotalCount" Runat="server"></asp:label>     
        </div>
    </td>
    <td width="10%">
    <div class="user-control">
    <table><tr>
    <td><asp:imagebutton id="btnFirst" Runat="server" Enabled="false" SkinID="imbGridFirst" CommandName="FIRST" /></td>
     <td><asp:imagebutton id="btnPrevious" Runat="server" Enabled="false" SkinID="imbGridPrev" CommandName="PREVIOUS"/></td>
      <td><asp:imagebutton id="btnNext" Runat="server" Enabled="false" SkinID="imbGridNext" CommandName="NEXT"/></td>
       <td><asp:imagebutton id="btnLast" Runat="server" Enabled="false" SkinID="imbGridLast" CommandName="LAST" /></td>
            </tr>
     </table>
     </div>
    </td>
</tr>
</table>
</div>