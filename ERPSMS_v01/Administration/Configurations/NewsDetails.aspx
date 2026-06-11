<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic" AutoEventWireup="true" CodeBehind="NewsDetails.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.NewsDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    //Pename the Page_Entry ID with the corresponding pages Entry section ID        
    function ShowHide(flag) {
        if (flag == "ADD_DETAILS") {
            $("[id$=SEC_ActionPanel]").hide();
            $("[id$=Page_Entry]").hide();
        }
        else {
            $("[id$=Page_Entry]").show();
            $("[id$=SEC_ActionPanel]").show();
        }
        return false;
    }
    function ValidateNow() {
        if (typeof (Page_ClientValidate) == 'function') {
            Page_ClientValidate();
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverror").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }
    function CheckOtherIsCheckedByGVID(spanChk) {
        var IsChecked = spanChk.checked;
        var CurrentRdbID = spanChk.id;
        $("[id$=grdPage]").find("tr:has(td)").each(function () {
            //type = $(this).find("td:first input").attr("type")
            var id = $(this).find("td:first input").attr("id");
            if (id != CurrentRdbID) {
                $(this).find("td:first input").attr("checked", false);
            }
        });

    }
    </script>

    <asp:UpdatePanel ID="auplDetailList" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
     <div class="datawrap">
         <%--use the width property of the below table corresponding to the contents in the page--%>
        <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks" Width="70%">
            <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
            <asp:TableRow ID="Page_Entry" runat="server">
                <%--Align table cell according to design--%>
                <asp:TableCell>
                    <div class="contentwrapper">
                        <asp:Label runat="server" ID="lblTitle" Text="title"></asp:Label>
                                <asp:TextBox runat="server" ID="txtTitle" TabIndex="1" CssClass="small" MaxLength="50"></asp:TextBox>
                                

                                <asp:Label runat="server" ID="lblShortDesc" Text="Short Description"></asp:Label>
                                <asp:TextBox runat="server" ID="txtShortDesc" TabIndex="1" CssClass="small" MaxLength="50"></asp:TextBox>
                               

                                <asp:Label runat="server" ID="lblPublishedDt" Text="Published Date"></asp:Label>
                                <asp:TextBox runat="server" ID="txtPublishedDt" TabIndex="10" CssClass="medium" onkeydown="return false;" onpaste="return false;"></asp:TextBox>
                                <asp:HiddenField ID="hdfPublishedDt" runat="server" />
                                

                                <asp:Label runat="server" ID="lblDetails" Text="Details"></asp:Label>
                                <textarea id="txtDetails" runat="server" cols="20" rows="2"></textarea>
                    </div>        
                </asp:TableCell>
                                
            </asp:TableRow>
            <%--Rename this ID Page_List with the corresponding section Id in the documet--%>
            <asp:TableRow ID="Page_List">
                <asp:TableCell>
                    <%-- Leave this table as such --%>
                    <asp:Table ID="Table1" runat="server" >
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" align="center">
                               <ul>
                                    <li runat="server" ID="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$ Resources:gBudgetRes, Save %>" ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow()" CommandArgument="<%$ resources:Section2 %>" />
                                    </li>
                                    <li runat="server" ID="pnlEdit">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$ Resources:gBudgetRes, Delete %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                                        <asp:Button runat="server" ID="btnActivate" CommandName="ACTIVATE" Text="<%$ Resources:gBudgetRes, Activate %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                                        <asp:Button runat="server" ID="btnInActivate" CommandName="DEACTIVATE" Text="<%$ Resources:gBudgetRes, Deactivate %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"/>
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" CommandName="CANCEL" Text="<%$ Resources:gBudgetRes, Cancel %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                                    </li>       
                                </ul>  
                                <div class="blockr"></div>                     
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                           <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                           <asp:TableCell CssClass="SEC_ACTION">
                                <asp:Button runat="server" ID="btnNew" CommandName="NEW" Text= "<%$ Resources:gBudgetRes, New %>"  OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                                <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$ Resources:gBudgetRes, Edit %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                                <asp:Button runat="server" ID="btnView" CommandName="VIEW" Text="<%$ Resources:gBudgetRes, View %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>"  />
                                <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" Text="<%$ Resources:gBudgetRes, Print %>" CommandArgument="<%$ resources:Section2 %>" />
                           </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                               <%-- use the grid to list the records in the page--%>
                                <asp:GridView ID="grdPage" runat="server" AutoGenerateColumns="False" Width="100%"
                                        AllowPaging="True" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:RadioButton runat="server" GroupName="SelectOne" ID="rbtSelect" onclick="javascript:CheckOtherIsCheckedByGVID(this);" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                </asp:GridView>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
     </div>
         <div id="diverror" style="display: none">
            <%--Use this label to bind the server errors--%>
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass ="star"></asp:Label>
            <asp:ValidationSummary ID="vvsPage" ValidationGroup="Save" runat="server" />
         </div>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
