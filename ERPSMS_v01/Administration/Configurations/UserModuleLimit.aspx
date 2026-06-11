<%@ Page Title="<%$ Resources:Captions,Title_UserModuleLimit %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="UserModuleLimit.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.UserModuleLimit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    var pageURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    function InitComponents() {
        GetUserRoleAuto();
    }

    function GetUserRoleAuto() {
        GrandScriptUtils.MakeAutoCompleteDDL("txtUser", url + "?Type=USER&ModulePK=" + $("[id$='hdfModulePK']").val(), "hdfUserPK", true, true, "GETUSERSINBOXMAPPING");
        GrandScriptUtils.MakeAutoCompleteDDL("txtRole", url + "?Type=ROLE&ModulePK=" + $("[id$='hdfModulePK']").val(), "hdfRolePK", true, true, "GETROLESINBOXMAPPING");
    }

</script> 
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" />
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="8" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--  <div id="grdTable-wrap">
          
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
        </div>--%>
               
                <div class="gridwrap">                   
                    
                    <asp:label id="lblrolebased" runat="server" Text="<%$ resources:Rolebased %>" Font-Bold="true" CssClass="label-align-cen"></asp:label>
                  
                    <asp:GridView ID="grdUserModule" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                        AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                        Width="60%" OnRowDataBound="ActionHandler">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Module %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblModule" runat="server" Text='<%# Eval("SYM_CODE") %>' ToolTip='<%# Eval("SYM_CODE")%>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfCurPk" Value='<%# Eval("SYM_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:ModuleName %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblModuleName" runat="server" Text='<%# Eval("SYM_NAME") %>' ToolTip='<%# Eval("SYM_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="45%" />
                                <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>" ToolTip="<%$ resources:Total %>">
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:License %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblLicence" runat="server" Text='<%# Eval("SYM_CFG_COUNT") %>' ToolTip='<%# Eval("SYM_CFG_COUNT") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <FooterTemplate>
                                    <asp:Label ID="lblLicenseTotal" runat="server">
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Used %>">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkCurrent" Text='<%# Eval("SYM_USER_COUNT") %>'
                                        ToolTip='<%# Eval("SYM_USER_COUNT") %>' OnClick="ActionHandler" CommandArgument='<%# Eval("SYM_PK") %>'
                                        CssClass="text-underline" CommandName="VIEW">
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <FooterTemplate>
                                    <asp:Label ID="lblUsedTotal" runat="server">
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>

                      <asp:Label ID="lblTranscation" runat="server"  Text="<%$ resources:Transcationbased %>" Font-Bold="true"  CssClass="label-align-cen"> </asp:Label>
                    <asp:GridView ID="grdLicenceview" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                        AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                        Width="60%">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle Width="5%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Module %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblModule" runat="server" Text='<%# Eval("Module") %>' ToolTip='<%# Eval("Module")%>'></asp:Label>
                                    
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:TranscationLicense %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblLicence" runat="server" Text='<%# Eval("License") %>' ToolTip='<%# Eval("License")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="45%" />
                               
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:LicenseUsed %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblused" runat="server" Text='<%# Eval("Used") %>' ToolTip='<%# Eval("Used") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>
  
                        </Columns>
                    </asp:GridView>


                       <table  class="div2col-S gridwrap">
                    <tr>
                        <td style="width:160px">
                            <asp:Label runat="server" ID="lblApplicationUser" Text="<%$ resources:ApplicationUserCount %> "></asp:Label>
                        </td>
                        <td style="width:40px">
                            <asp:Label runat="server" ID="lblApplicationUserCount" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:160px">
                            <asp:Label runat="server" ID="lblSystemnUser" Text="<%$ resources:SystemUserCount %> "></asp:Label>
                        </td>
                        <td style="width:40px">
                            <asp:Label runat="server" ID="lblSystemnUserCount" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:160px">
                            <asp:Label runat="server" ID="lblServiceUser" Text="<%$ resources:ServiceUserCount %> "></asp:Label>
                        </td>
                        <td style="width:40px">
                            <asp:Label runat="server" ID="lblServiceUserCount" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:160px">
                            <asp:Label runat="server" ID="lblPortalUser" Text="<%$ resources:PortalUserCount %> "></asp:Label>
                        </td>
                        <td style="width:40px">
                            <asp:Label runat="server" ID="lblPortalUserCount" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                  
                </div>


             
                <%--<div class="div2col-S">
                                
                                
                                <div class="clear">
                                </div>
                                
                               
                                 <div class="clear">
                                </div>
                               
                                
                                 <div class="clear">
                                </div>
                                
                                
                                
                            </div>--%>
                <div id="divRoleDetails" style="display: none;">
                    <div id="searchwrap" class="padgtop5">
                        <asp:Label runat="server" ID="lblSearchUser" Text="<%$ resources:User %>" AssociatedControlID="ddlUsers"
                            CssClass="margn-lft5"></asp:Label>
                        <asp:DropDownList ID="ddlUsers" CssClass="select-medium" runat="server" Visible="false">
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtUser" CssClass="select-medium" Visible="true"></asp:TextBox> 
                        <asp:HiddenField runat="server" ID="hdfUserPK" Value="0" />
                        <asp:Label runat="server" ID="lblRole" Text="<%$ resources:Role %>" AssociatedControlID="ddlRole"
                            CssClass="margn-lft5"></asp:Label>
                        <asp:DropDownList ID="ddlRole" CssClass="select-medium-a" runat="server" Visible="false">
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtRole" CssClass="input-w45per" Visible="true"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdfRolePK"  Value="0"/>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="11" SkinID="search-ext" OnClick="ActionHandler" CommandName="SEARCH"
                            CssClass="margn-rgt0 margntop2" />
                        <asp:ImageButton ID="btnClear" runat="server" OnClick="ActionHandler" CommandName="CLEAR"
                            TabIndex="12" ToolTip="<%$ resources:Controls,Clear %>" CssClass="margntop2"
                            SkinID="clear-ext" />
                        <div class="clear">
                        </div>
                    </div>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdModuleUser" Width="100%" AllowSorting="True"
                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" ShowHeader="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:User %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("SYM_USER_TEXT") %>' ToolTip='<%# Eval("SYM_USER_TEXT")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfUserPk" Value='<%# Eval("SYM_USER") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Role %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("SYM_USER_GROUP_TEXT") %>'
                                            ToolTip='<%# Eval("SYM_USER_GROUP_TEXT")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfUserGroupPk" Value='<%# Eval("SYM_USER_GROUP") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="70%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
                <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgUser" runat="server" />
            </div>
            <asp:HiddenField runat="server"  ID="hdfModulePK" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
