<%@ Page Title="<%$ Resources:Captions,Title_InboxUserMapping %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" AutoEventWireup="true" CodeBehind="InboxUserMapping.aspx.cs"
    Inherits="ERPSMS_v01.GeneralAdmin.InboxUserMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GetUserRoleAuto();
            if ($("[id$=hdfUserAutoEnable]").val() == 1) {
                DisableAuto($("[id$=txtUser]"), $("[id$=hdfUserPK]"));
            }

        }

        function GetUserRoleAuto() {
            //GrandScriptUtils.MakeAutoCompleteDDL("txtUser", url + "?Type=USER&ModulePK=" + $("[id$='ddlModule']").val(), "&hdfUserPK" + "UserLogin=" + $("[id$='hdfUserPK']").val(), true, true, "GETUSERSINBOXMAPPING");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtRole", url + "?Type=ROLE&ModulePK=" + $("[id$='ddlModule']").val(), "&hdfRolePK" + "UserLogin=" + $("[id$='hdfUserPK']").val(), true, true, "GETROLESINBOXMAPPING");

            GrandScriptUtils.MakeAutoCompleteDDL("txtUser", url + "?Type=USER&ModulePK=" + $("[id$='ddlModule']").val() + "&UserLogin=" + $("[id$='hdfuserLogin']").val(), "hdfUserPK", true, true, "GETUSERSINBOXMAPPING");
            GrandScriptUtils.MakeAutoCompleteDDL("txtRole", url + "?Type=ROLE&ModulePK=" + $("[id$='ddlModule']").val() + "&UserLogin=" + $("[id$='hdfuserLogin']").val() + "&EmpDept=" + $("[id$='ddlDepartment']").val(), "hdfRolePK", true, true, "GETROLESINBOXMAPPING");
        }

        function AfterAutoCompleteSelect(ControlID) {
            if (ControlID == 'txtUser' || ControlID == 'txtRole') {
                $("[id$=btnUserRoleChange]").click();
            }
        }

        $("[id*=chkAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                } else {
                    $(this).removeAttr("checked");
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div id="divBtnContainer" runat="server" class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul id="ulBrudCrum" runat="server"  class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" />
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            OnClick="ActionHandler" CommandName="SAVE" TabIndex="8" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" ToolTip="<%$Resources:Controls,Cancel%>"
                                            Text="<%$Resources:Controls,Cancel%>" OnClientClick="javascript:return CancelFun();"
                                            TabIndex="8" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div id="divRoleDetails">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div id="divTblCol1" runat="server"  class="div2col-S">
                                    <asp:Label runat="server" ID="lblUserModule" Text="<%$ resources:Module %>" AssociatedControlID="ddlModule"></asp:Label>
                                    <asp:DropDownList ID="ddlModule" CssClass="select-medium" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ActionHandler">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblSearchUser" Text="<%$ resources:User %>" AssociatedControlID="ddlUsers"
                                        CssClass="middle-lbl-small"></asp:Label>
                                    <asp:DropDownList ID="ddlUsers" CssClass="select-medium" runat="server" Visible="false">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtUser" CssClass="select-medium"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdfUserPK" Value="0" />
                                      <asp:HiddenField runat="server" ID="hdfuserLogin" Value="0" />
                                </div>
                            </td>
                            <td>
                                <div id="divTblCol2" runat="server" class="div2col-S">
                                 <asp:Label runat="server" ID="lblDepartment" Text="<%$ resources:Controls,Department %>" AssociatedControlID="ddlDepartment"></asp:Label>
                                    <asp:DropDownList ID="ddlDepartment" CssClass="select-small-a0" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" >
                                    </asp:DropDownList>

                                    <asp:Label runat="server" ID="lblRole" Text="<%$ resources:Role %>" AssociatedControlID="ddlRole" class="lbl-12perc"></asp:Label>
                                    <asp:DropDownList ID="ddlRole" CssClass="select-medium-a" runat="server" Visible="false">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="txtRole" CssClass="select-medium"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdfRolePK" Value="0" />
                                    <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                        TabIndex="11" SkinID="search-ext" OnClick="ActionHandler" CommandName="SEARCH"
                                        Style="margin-bottom: 0px!important; margin-top: 2px; margin-right: 5px!important;" />
                                    <asp:ImageButton ID="btnClear" runat="server" OnClick="ActionHandler" CommandName="CLEAR"
                                        TabIndex="12" Style="margin-bottom: 0px!important; margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>"
                                        SkinID="clear-ext" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <%--<div id="searchwrap" class="search-wrap-custom1">
                         
                        
                    </div>--%>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdModuleUser" Width="100%" AllowSorting="True"
                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" ShowHeader="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="No." ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:User %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("gumUserText") %>' ToolTip='<%# Eval("gumUserText")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfUserPk" Value='<%# Eval("gumUser") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Role %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("gumGroupText") %>' ToolTip='<%# Eval("gumGroupText")%>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfUserGroupPk" Value='<%# Eval("gumGroup") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="50%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HasInboxMessage %>">
                                    <ItemStyle Width="15%" />
                                     <HeaderStyle CssClass="txt-rgt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:HasInboxMessage %>">
                                    <HeaderTemplate >
                                        <asp:CheckBox runat="server" ID="chkAll" />
                                    </HeaderTemplate>
                                   
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkHasInInboxMessage" runat="server" Checked='<%# Convert.ToBoolean(Eval("gumHasInbox")) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdfUserAutoEnable" Value="0"/>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
                <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgUser" runat="server" />
            </div>
            <div style="display: none;">
                <asp:Button runat="server" ID="btnUserRoleChange" OnClick="ActionHandler" CommandName="SEARCH" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
