<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" 
Inherits="HRMS.TaskTracker.Test" Theme="Classic"%>
<%@ Register Src="UserControls/TaskCreatePopUp.ascx" TagName="PopUp" TagPrefix="uc2" %>
<%@ Register Src="UserControls/TaskStatusPopUp.ascx" TagName="PopUp2" TagPrefix="uc3" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
     <script type="text/javascript">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:UpdatePanel runat="server" ID="aupdpnl">
 <ContentTemplate>
 <div class="content-wrapper">
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S">
                        <asp:LinkButton ID="lnkPopoUp" runat="server" CommandName="SHOWPOPUP" OnClick="ActionHandler">Show Pop Up</asp:LinkButton>
                        <div class="clear">
                        <asp:LinkButton ID="lnkPopUp2" runat="server" CommandName="SHOWPOPUP2" OnClick="ActionHandler">Show Pop Up</asp:LinkButton>
                        <div class="clear">
                        </div>
                    </div>
                </td>  
                <td>
                <div id="divPopUp" style="display:none">
                    <uc2:PopUp ID="ucPopUpTask" runat="server" />
                </div>
                <div id="divPopUp2" style="display:none">
                    <uc3:PopUp2 ID="ucPopUpStatus" runat="server" />
                </div>
                </td>            
            </tr>          
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
