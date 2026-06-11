<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="ClassicExt" CodeBehind="PlanningGroupsMaster.aspx.cs" Inherits="ERPSMS_v01.Inventory.Masters.PlanningGroupsMaster" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "/Handlers/AutoComplete.ashx");
        function InitComponents() {
            $("[id$='lblValidGroupCode']").hide();
            $("[id$='lblValidGroupName']").hide();
            $("[id$='lblValidStatus']").hide();
            SetSearchBy();
        }


        function SetSearchBy() {
            $("[id$=hdfFilterPlanPK]").val(0);
            if ($("select[id$=ddlFilterBy]").val() == 1) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterPlan", url + "&SearchBy=CODE", "hdfFilterPlanPK", true, true, "PLANNINGGROUP");}
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFilterPlan", url + "&SearchBy=NAME", "hdfFilterPlanPK", true, true, "PLANNINGGROUP");
                }
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnProductGroupListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnProductGroupListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnProductGroupDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductGroupDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnProductGroupListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductGroupListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductGroupDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnProductGroupDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
        }


        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidGroupCode']").hide();
            $("[id$='lblValidGroupName']").hide();
            $("[id$='lblValidStatus']").hide();

            if ($("[id$='txtGroupCode']").val() == '') {
                $("[id$='lblValidGroupCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_GroupCode") %></li></ul>';
            }

            if ($("[id$='txtGroupName']").val() == '') {
                $("[id$='lblValidGroupName']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_GroupName") %></li></ul>';
            }

            if ($("[id$='ddlStatus']").val() == '-1') {
                $("[id$='lblValidStatus']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Status") %>' + '</li></ul>';
            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="ProductGroup" SkinID="btnInner-Save"
                                            TabIndex="97" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="98" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="99" ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnProductGroupListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnProductGroupListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="8" />
                        </span></li>
                        <li><span id="spnProductGroupDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnProductGroupDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="ACTIVATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="searchwrap" class="search-wrap-custom1">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1" onkeypress="javascript:return SetFocus(event);"
                                                onchange="SetSearchBy();">
                                    <asp:ListItem Text="<%$ resources:Controls,PlanningGroupCode %>" Value="<%$resources:SearchPlanCodeValue %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,PlanningGroupName %>" Value="<%$resources:SearchPlanNameValue %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" onkeydown="return Search(event);" OnClick="ActionHandler"
                                    TabIndex="2" Visible="false"/>
                                    <%--<asp:TextBox ID="txtFilterPlanName" runat="server" TabIndex="1" MaxLength="500" CssClass="display-none">
                                            </asp:TextBox>--%>
                                            <asp:TextBox ID="txtFilterPlan" runat="server" TabIndex="1" autocomplete="off" CssClass="input-medium-b">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterPlanPK" runat="server" Value="0" />


                                   <%--  <asp:TextBox ID="txtPlanningGrpSearchBy" runat="server"/>--%>
                                     <asp:HiddenField ID="hdfPlanningGrpSearchBy" runat="server"/>
                                      <asp:HiddenField ID="hdfSearchBy" runat="server" Value="2"/>
                                <asp:Button ID="btnSearch1" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler" style="float:none!important;"
                                    TabIndex="3" />
                                <div class="clear">
                                </div>
                            </div>
                             <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdPlanningGroupMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler">
                                      <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4"   OnCheckedChanged="ActionHandler"  />
                                                     <asp:HiddenField runat="server" ID="hdfPlanningPk" Value='<%# Eval("PIG_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                       <%-- <asp:TemplateField HeaderText="<%$ resources:Controls,PlanningGroupCode%>" SortExpression="<%$ resources:DataFieldRes,INVPlanningGroupCode %>">--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,PlanningGroupCode%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPGCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.INVPlanningGroupCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.INVPlanningGroupCode)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                      <%--  <asp:TemplateField HeaderText="<%$ resources:Controls,PlanningGroupName%>" SortExpression="<%$ resources:DataFieldRes,INVPlanningGroupName %>">--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,PlanningGroupName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPGName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.INVPlanningGroupName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.INVPlanningGroupName)),100) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="78%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Description%>" SortExpression="<%$ resources:DataFieldRes, INVPlanningGroupDesc %>"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.INVPlanningGroupDesc)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.INVPlanningGroupDesc)),200) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="<%$ resources:Controls,Status%>" SortExpression="<%$ resources:DataFieldRes,INVPlanningGroupStatus %>" >--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Status%>">
                                            <ItemTemplate>
                                                <image id="imgStatus" title='<%# (Eval("PIG_ACTIVE")).ToString()=="1"? Resources.ErpRes.Active :Resources.ErpRes.InActive %>'
                                                    class='<%# (Eval("PIG_ACTIVE")).ToString()=="1"?"active" :"inactive"%>' alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                         </Columns>
                                 </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                             </div>






                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroupCode" runat="server" Text="<%$ resources:Controls,PlanningGroupCode%>"
                                                AssociatedControlID="txtGroupCode" />
                                            <asp:TextBox ID="txtGroupCode" runat="server" CssClass="input-half" MaxLength="100"
                                                TabIndex="10" onpaste="limitText(this,100);" />
                                            <asp:Label ID="lblValidGroupCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Controls,Status%>" AssociatedControlID="ddlStatus" />
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="11" CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblValidStatus" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblGroupName" runat="server" Text="<%$ resources:Controls,PlanningGroupName%>"
                                                AssociatedControlID="txtGroupName" />
                                            <asp:TextBox ID="txtGroupName" runat="server" MaxLength="200" TabIndex="12" onpaste="limitText(this,200);"
                                                CssClass="input-half" />
                                            <asp:Label ID="lblValidGroupName" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblGroupDesc" runat="server" Text="<%$ resources:Controls,Description%>"
                                                AssociatedControlID="txtGroupDesc" />
                                            <asp:TextBox ID="txtGroupDesc" runat="server" TextMode="MultiLine" MaxLength="500" 
                                                TabIndex="13" CssClass="multiline-3line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="tree-label">
                                <label>
                                    &nbsp;</label>
                                <span><b id="treeLabel" runat="server">Product Items</b></span>
                                <div class="clear">
                                </div>
                                <label>
                                    &nbsp;</label>
                                <%--<div class="treeview max-380">--%>
                                <div class="treeview">
                                    <div class="clear">
                                    </div>
                                  <%--  <asp:TreeView ID="trvItem" runat="server" ShowLines="false" ShowCheckBoxes="All" ExpandDepth="0" TabIndex="14">
                                    </asp:TreeView>--%>
                                       <asp:TreeView ID="trvItem" runat="server" ShowLines="false" ExpandDepth="0" TabIndex="14">
                                    </asp:TreeView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="ProductGroup" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
