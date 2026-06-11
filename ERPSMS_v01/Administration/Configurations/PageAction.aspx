<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master" AutoEventWireup="true" CodeBehind="PageAction.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.PageAction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     function ShowListing(flag) {
         if (flag) {
             $("[id$=PageAction_Entry]").hide();
             $("[id$=PageAction_List]").show();
             $("[id$=imbAdd]").show();
             $("[id$=imbSave]").hide();
         }
         else {
             $("[id$=PageAction_Entry]").show();
             $("[id$=PageAction_List]").hide();
             $("[id$=imbAdd]").hide();
             $("[id$=imbSave]").show();
         }
         return false;
     }

     function ValidateNow() {
         if (typeof (Page_ClientValidate) == 'function') {
             Page_ClientValidate();
         }
         if (!Page_IsValid) {
             $("#litErrorMsg").hide();

             ShowErrorMessage($("#diverror").html());
             return false;  //Page is invalid -- stop right here
         }
         else {
             //everythings ok --- Call your function & do your stuff
             return true;
         }
     }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%-- <div id="innerPage-wrap">
    <asp:UpdatePanel runat="server" ID="aupdpnlPageAction">
        <ContentTemplate>
        <asp:Table runat="server" ID="tblTemplate" Width="100%">
            <asp:TableRow Width="100%">
                <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION">
                    <div id="webwizard-wrap"><h1>
                       <asp:Literal ID="ltHead" runat="server" Text="<%$ resources:PageActions%>" /></h1>
                        <div class="button-wrap">
                            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" ClientIDMode="Static"  
                                TabIndex="11" OnClick="ActionHandler" CommandName="ADD" CommandArgument="SEC_ActionPanel" />
                            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return ValidateNow();" OnClick="ActionHandler" ValidationGroup="save"
                                CommandName="SAVE" CommandArgument="SEC_ActionPanel" TabIndex="9" />
                            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btnreset" OnClick="ActionHandler"
                                CommandName="CANCEL" CommandArgument="SEC_ActionPanel" TabIndex="10" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="PageAction_Entry" runat="server">
                <asp:TableCell CssClass="hrzcentr">
                    <div class="div2col-M">
                        <asp:HiddenField ID="hdfcurrPK" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfLastModDt" runat="server" />
                        <asp:Label ID="lblPage" runat="server" Text="<%$ resources:Page %>" AssociatedControlID="ddlPage"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlPage" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="vrefPage" ControlToValidate="ddlPage" Display="Dynamic" CssClass="star" 
                        ValidationGroup="save" ErrorMessage="<%$ resources:Msg_Page %>" Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDesc" Text="<%$ resources:PageDesc %>"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDesc" TabIndex="3"></asp:TextBox>
                   
                        <asp:Label ID="lblSection" runat="server" AssociatedControlID="txtSection" Text="<%$ resources:Section %>"></asp:Label>
                        <asp:TextBox runat="server" ID="txtSection" TabIndex="2"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="vrefSection" ControlToValidate="txtSection" Display="Dynamic" CssClass="star" 
                        ValidationGroup="save" ErrorMessage="<%$ resources:Msg_Section %>" Text="*" ></asp:RequiredFieldValidator>
                        <asp:Label ID="lblAction" runat="server" AssociatedControlID="txtAction" Text="<%$ resources:Action %>"></asp:Label>
                        <asp:TextBox runat="server" ID="txtAction" TabIndex="4"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="vrefAction" ControlToValidate="txtAction" Display="Dynamic" CssClass="star" 
                        ValidationGroup="save" ErrorMessage="<%$ resources:Msg_Action %>" Text="*" ></asp:RequiredFieldValidator>
                   </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="PageAction_List" runat="server">
                <asp:TableCell>
                    <div id="grdTable-wrap">
                        <div class="grdTable">
                            <asp:GridView runat="server" ID="grdActions" AllowPaging="True" Width="100%" PageSize="10" CssClass="grdTable"
                            OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler" 
                            DataKeyNames="ACT_PK,LAST_MOD_DT" AutoGenerateColumns="false">                                
                                <Columns>
                                   
                                    <asp:BoundField HeaderText="<%$ resources:Page %>" DataField="ACT_PAGE_TEXT" SortExpression="ACT_PAGE_TEXT" ItemStyle-Width="30%" />
                                    <asp:BoundField HeaderText="<%$ resources:Section %>" DataField="ACT_SECTION" SortExpression="ACT_SECTION" ItemStyle-Width="30%" />
                                    <asp:BoundField HeaderText="<%$ resources:Action %>" DataField="ACT_ACTION" SortExpression="ACT_ACTION" ItemStyle-Width="30%" />
                                    <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td><asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler" 
                                                CssClass="_edit" CommandName="GRIDEDIT" CommandArgument='<%#Eval("ACT_PK")%>' /></td>
                                                    <td><asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbDelete"  SkinID="imbdeletegrid" OnClick="ActionHandler" 
                                                CssClass="_delete" CommandName="GRIDDELETE" CommandArgument='<%#Eval("ACT_PK")%>' /></td>
                                                </tr>
                                            </table>

                                            
                                            
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="diverror" style="display: none">

        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static"></asp:Label>
        <asp:ValidationSummary ID="vvsPageAction" ValidationGroup="save" runat="server" />
    </div>
--%>
</asp:Content>
