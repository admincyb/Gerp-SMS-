<%@ Page Title="<%$ Resources:Captions,Title_AccountsMaster %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="AccountsMaster.aspx.cs" Inherits="ERPSMS_v01.Finance.Administration.Masters.AccountsMaster" %>

<%--<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>--%>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        var NumberDigits = 0;
        $(document).ready(function () {

            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
        });
        function InitComponents() {

            //Select All check box
            $('[id$=ChkSelectAll]').click(function () {
                $("[id$='chkCCselect']").attr('checked', this.checked);
            });

            $('[id$=chkCCselect]').click(function () {
                if ($("[id$='ChkSelectAll']").attr("checked", true))
                $("[id$='ChkSelectAll']").attr('checked', false);
            });

            $("[id$='txtSequence']").ForceNumericOnly();
            $("[id$='txtPercentage']").ForceNumericOnly();
            if ($("[id$='hdfHasChild']").val() == '1')
                $("[id$='chkIsGroup']").attr("disabled", true);
            else
                $("[id$='chkIsGroup']").attr("disabled", false);
            GetCheckedRows();

            GrandScriptUtils.MakeAutoCompleteDDL("txtCoaParent", url + "&SearchBy=COA_NAME&Status=1", "hdfCoaParent", true, true, "GETCOAPARENT");
            if ($("[id$=txtCoaParent]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCoaParent]"), $("[id$=hdfCoaParent]"));
            }
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCoaParent") {
                $("[id$='btnCoaParent']").click();
            }
        }
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCoaParent") {
                $("[id$='btnCoaParent']").click();
            }
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
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
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnAccountTree']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnAccountTree']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAllocation']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAllocation']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='PageAction_Tree']").show();
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='ModifiedDatePnl']").hide();
                $("[id$='PageAction_Allocate']").hide();
            }
            else if (tab == 2) {
                $("[id$='spnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnAccountListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAllocation']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAllocation']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='PageAction_Tree']").hide();
                $("[id$='PageAction_Allocate']").hide();
            }
            else if (tab == 3) {
                $("[id$='spnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnMngAccounts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnMngAccounts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnAllocation']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnAllocation']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='PageAction_Tree']").hide();
                $("[id$='PageAction_Allocate']").hide();
            }
            else if (tab == 4) {
                $("[id$='spnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnMngAccounts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnMngAccounts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnAllocation']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnAllocation']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='PageAction_Tree']").hide();
                $("[id$='PageAction_Allocate']").show();
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlEntry']").show();
                $("[id$='btnDelete']").hide();
                CalculateTotalCC();
            }
        }

        function ValidateNow() {

            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false; //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function CalculateTotalCC() {
            var TotalgrnQty = 0;
            $("#[id*=grdCostCenter] input[type=text][id*=txtPercentage]").each(function (index) {
                if ($.trim($(this).val()) != "") {
                    if (!isNaN(parseFloat($(this).val()))) {
                        TotalgrnQty = TotalgrnQty + parseFloat($(this).val().replace(/[^0-9\.]+/g, ""));
                    }
                }
            });
            //$("#[id*=grdCostCenter] [id*=lblTotalPercentageFooter]").html(TotalgrnQty.toFixed(NumberDigits));
            $("#[id*=grdCostCenter] [id*=lblTotalPercentageFooter]").html(TotalgrnQty);
        }

        function GetCheckedRows() {
            $("[id$=grdCostCenter]").find("tr:has(td)").each(function () {
                var checked = $(this).closest('tr').find('[id*=chkCCselect]').attr("checked");
                if (checked == true) {
                    $(this).closest('tr').find('[id*=txtPercentage]').attr("disabled", false);
                }
                else {
                    $(this).closest('tr').find('[id*=txtPercentage]').attr("disabled", true);
                    $(this).closest('tr').find('[id*=txtPercentage]').val("");
                }
            });
        }
        function IsNeedCoaParent(source, arguments) {
            if ($("[id$=hdfCoaParent]").val() == '0' || $("[id$=hdfCoaParent]").val() == '') {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }
        function DisableAuto(extender, hfield) {
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlAccounts" runat="server">
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
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidateNow()" ValidationGroup="Account" SkinID="btnInner-Save"
                                            TabIndex="19" CommandArgument="SEC_ActionPanel" ToolTip="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="Li1" runat="server">
                                        <asp:Button ID="btnSaveAllocate" runat="server" CommandName="SAVEALLOCATE" Text="<%$resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidateNow()" ValidationGroup="Account" SkinID="btnInner-Save"
                                            TabIndex="19" CommandArgument="SEC_ActionPanel" ToolTip="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" Visible="false" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="20" ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="21" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="8" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnAccountTree" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnAccountTree" runat="server" Text="<%$resources:Controls,Tree %>"
                                CssClass="tab-inactive" CommandName="TREE_SELECT" OnClick="ActionHandler" TabIndex="22" />
                        </span></li>
                        <li><span id="spnAccountListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnAccountListing" runat="server" Text="<%$resources:Controls,List %>"
                                CommandName="SELECT" CssClass="tab-active" OnClick="ActionHandler" TabIndex="23" />
                        </span></li>
                        <li><span id="spnMngAccounts" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnMngAccounts" runat="server" Text="<%$resources:Controls,ManageAccounts %>"
                                CommandName="EDIT" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="24" />
                        </span></li>
                        <li><span id="spnAllocation" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnAllocation" runat="server" Text="<%$resources:Controls,Allocation %>"
                                CommandName="ALLOCATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="25" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfHasChild" runat="server" />
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Tree" runat="server">
                        <asp:TableCell>
                            <div style="padding-left: 70px;">
                                <div class="treeview scroll-h350">
                                    <%--<asp:Label ID="lblAccounts" runat="server" Text="Accounts" />--%>
                                    <%--<asp:TreeView ID="trvAccounts" runat="server" ShowLines="True" TabIndex="25">
                                        <SelectedNodeStyle Font-Bold="true" />
                                    </asp:TreeView>--%>
                                      <asp:TreeView ID="trvAccounts" runat="server" ShowLines="True" ExpandDepth="0"
                                        SelectedNodeStyle-BackColor="#e6f3ff" SelectedNodeStyle-Font-Bold="true" SelectedNodeStyle-ForeColor="#236cb5"
                                        OnTreeNodePopulate="ActionHandler">
                                        <NodeStyle Font-Bold="True" />
                                        <RootNodeStyle Font-Bold="True" />
                                        <ParentNodeStyle Font-Bold="True" />
                                        <Nodes>
                                            <asp:TreeNode PopulateOnDemand="true" Text="Node 0" />
                                        </Nodes>
                                    </asp:TreeView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1">
                                    <asp:ListItem Text="<%$ resources:Controls,AccountCode %>" Value="<%$ resources:DataFieldRes,AccountCode %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,Name %>" Value="<%$ resources:DataFieldRes,AccountName %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,Type %>" Value="<%$ resources:DataFieldRes,ConfigName %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,Category %>" Value="<%$ resources:DataFieldRes,CoaSubTypeName %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" CssClass="input-medium" onkeydown="return Search(event);"
                                    OnClick="ActionHandler" TabIndex="2" />
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                    TabIndex="3" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdAccounts" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,AccountCode%>" SortExpression="<%$ resources:DataFieldRes,AccountCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.AccountCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.AccountCode)),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Name%>" SortExpression="<%$ resources:DataFieldRes,AccountName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.AccountDescription)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.AccountName)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Parent%>" SortExpression="<%$ resources:DataFieldRes,AccountName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParent" runat="server" ToolTip='<%# String.IsNullOrEmpty(Convert.ToString(Eval("COA_PARENT"))) ? "" : ERP.Utilities.CommonFunctions.GetDecodedString(Eval("FIN_COA_MST2" + "." +"COA_DESC"))%>'
                                                    Text='<%# String.IsNullOrEmpty(Convert.ToString(Eval("COA_PARENT"))) ? "" : string.Format("{0} {1}", Eval("FIN_COA_MST2" + "." +"COA_NAME") , "(" + Eval("FIN_COA_MST2" + "." +"COA_CODE") +")") %>' />
                                                <asp:HiddenField ID="hdfParent" runat="server" Value='<%# Eval(Resources.DataFieldRes.CoaParent)%> ' />
                                            </ItemTemplate>
                                            <ItemStyle Width="27%" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$resources:Controls,ShortName%>" SortExpression="<%$ resources:DataFieldRes,AccountShortName %>">
                                            <ItemTemplate>
                                                 <asp:Label ID="lblShortName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.AccountShortName)) %>'
                                                    Text='<%#Eval(Resources.DataFieldRes.AccountShortName) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Type%>" SortExpression="<%$ resources:DataFieldRes, AccountType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccType" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>'
                                                    Text='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,IsGroup%>" SortExpression="<%$ resources:DataFieldRes,AccountIsGroup %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccIsGroup" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.AccountIsGroup).ToString() == "True" ? "Yes" : "No" %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.AccountIsGroup).ToString() == "True" ? "Yes" : "No" %>' />
                                                <asp:HiddenField ID="hdfAccIsGroup" runat="server" Value='<%# Eval(Resources.DataFieldRes.AccountIsGroup).ToString() == "True" ? "1" : "0" %> ' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Category%>" SortExpression="<%$ resources:DataFieldRes, AccountCategory %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccCategory" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.FinCoaSubTypeCfg + "." + Resources.DataFieldRes.CoaSubTypeName) %>'
                                                    Text='<%# Eval(Resources.DataTableRes.FinCoaSubTypeCfg + "." + Resources.DataFieldRes.CoaSubTypeName) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide tablelayout">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaCode" runat="server" Text="<%$resources:Controls,AccountCode%>"
                                                AssociatedControlID="txtCoaCode" />
                                            <asp:TextBox ID="txtCoaCode" runat="server" MaxLength="50" CssClass="input-half"
                                                TabIndex="9" />
                                            <asp:RequiredFieldValidator ID="vrfCoaCode" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtCoaCode" ErrorMessage="<%$ resources:Err_AccountCode %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCoaName" runat="server" Text="<%$resources:Controls,Name%>" AssociatedControlID="txtCoaName" />
                                            <asp:TextBox ID="txtCoaName" runat="server" MaxLength="50" TabIndex="11" CssClass="input-half" />
                                            <asp:RequiredFieldValidator ID="vrfCoaName" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtCoaName" ErrorMessage="<%$ resources:Err_AccountName %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaParent" runat="server" Text="<%$resources:Controls,Parent%>"
                                                AssociatedControlID="txtCoaParent" />
                                            <asp:TextBox ID="txtCoaParent" runat="server" CssClass="input-half" TabIndex="10"
                                                onfocus="this.select();" onMouseUp="return false;" />
                                            <asp:Button ID="btnCoaParent" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="CHANGE" />
                                            <asp:HiddenField ID="hdfCoaParent" runat="server" />
                                            <asp:CustomValidator ID="vcfCoaParent" runat="server" ErrorMessage="<%$ resources:Err_AccountParent %>"
                                                SetFocusOnError="true" CssClass="star" Text="*" Display="Dynamic" ValidationGroup="Account"
                                                ClientValidationFunction="IsNeedCoaParent" EnableClientScript="true"></asp:CustomValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCoaShortName" runat="server" Text="<%$resources:Controls,ShortName%>"
                                                AssociatedControlID="txtCoaShortName" />
                                            <asp:TextBox ID="txtCoaShortName" runat="server" MaxLength="100" TabIndex="12" CssClass="input-half" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblCoaDesc" runat="server" Text="<%$resources:Controls,Description%>"
                                                AssociatedControlID="txtCoaDesc" />
                                            <asp:TextBox ID="txtCoaDesc" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="13" CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaType" runat="server" Text="<%$resources:Controls,Type%>" AssociatedControlID="ddlCoaType" />
                                            <asp:DropDownList ID="ddlCoaType" runat="server" CssClass="w31-5perc" TabIndex="14" />
                                            <asp:RequiredFieldValidator ID="vrfCoaType" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlCoaType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountType %>" />
                                            <%--<asp:Label ID="lblIsGroup" runat="server" Text="<%$resources:Controls,IsGroup%>"
                                                AssociatedControlID="chkIsGroup" />--%>
                                            <asp:CheckBox ID="chkIsGroup" runat="server" TabIndex="15" Text="<%$resources:Controls,IsGroup%>"
                                                TextAlign="Left" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFinGroup" runat="server" Text="<%$resources:FinGroup%>" AssociatedControlID="ddlFinGroup" />
                                            <asp:DropDownList ID="ddlFinGroup" runat="server" TabIndex="17" CssClass="select-half-a" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaSubType" runat="server" Text="<%$resources:Controls,Category%>"
                                                AssociatedControlID="ddlSubType" />
                                            <asp:DropDownList ID="ddlSubType" runat="server" TabIndex="16" CssClass="select-w33per" />
                                            <asp:RequiredFieldValidator ID="vrfCoaSubType" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlSubType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountCategory %>" />
                                            <asp:Label ID="lblSequence" runat="server" Text="<%$resources:Controls,DisplayOrder%>"
                                                CssClass="middle-lbl" AssociatedControlID="txtSequence" />
                                            <asp:TextBox ID="txtSequence" runat="server" MaxLength="4" CssClass="small numeric"
                                                onkeypress="return isNumberKey(event)" onpaste="return false;" TabIndex="17" />
                                            <asp:RequiredFieldValidator ID="vrfCoaSequence" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtSequence" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DisplayOrder %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompany" runat="server" Text="Company" AssociatedControlID="ddlCompany" />
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="17" CssClass="select-half-a" />
                                             <asp:RequiredFieldValidator ID="vrfCompany" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlCompany" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Company %>" />
                                            <div class="clear">
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$resources:Controls,Remarks%>"
                                                AssociatedControlID="txtRemarks" />
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="18" CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="DivAdditionalDetails" runat="server" visible="true">
                                <div class="search-colapse-b">
                                    <h1 class="fontWGT-Nrml">
                                        <asp:Literal ID="ltrAdditionalDetails" runat="server" Text="<%$Resources:AdditionalDetails %>" />
                                    </h1>
                                </div>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblReportColumn" runat="server" Text="<%$resources:Controls,ReportColumn%>"
                                                    AssociatedControlID="ddlReportColumn" CssClass="lbl-12-5perc" />
                                                <asp:DropDownList ID="ddlReportColumn" runat="server" TabIndex="16" CssClass="select-w16per" />
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlSubType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountCategory %>" />--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="PageAction_Allocate" runat="server">
                    <asp:TableRow ID="TableRow1" runat="server">
                        <asp:TableCell>
                            <div id="divAccountHeader" runat="server" border="0" cellpadding="0" cellspacing="0"
                                class="detail-poi-co3">
                                <%--class="detail-co2"--%>
                                <table style="width: 100%;">
                                    <%--class="table-4devide" border="0" cellpadding="0" cellspacing="0"--%>
                                    <tr>
                                        <td style="width: 1%;">
                                        </td>
                                        <td style="width: 40%;">
                                            <asp:Label ID="lblhdrAccountCode" runat="server" AssociatedControlID="lblhdrAccountCodeText"
                                                Text="<%$resources:AccountCode%>" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblhdrAccountCodeText" runat="server" Text="" class="margnbotm0" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 59%;">
                                            <asp:Label ID="lblhdrAccountName" runat="server" AssociatedControlID="lblhdrAccountNameText"
                                                Text="<%$resources:AccountName%>" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblhdrAccountNameText" runat="server" Text="" class="margnbotm0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCostCenter" Width="50%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler" ShowFooter="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAll" runat="server" TabIndex="4" Checked="false" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%-- <asp:RadioButton ID="rbtCCSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />--%>
                                                <asp:CheckBox runat="server" ID="chkCCselect" Checked='<%# Eval("FCM_PK") ==  DBNull.Value ? false : true %>'
                                                    OnClick="GetCheckedRows();" />
                                                <asp:HiddenField runat="server" ID="hdfCostCenterID" Value='<%# Eval("CNM_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfCCPK" Value='<%# Eval("FCM_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:CostCenter%>">
                                            <%--SortExpression="<%$ resources:DataFieldRes,AccountCode %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCostCenter" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CostCenterName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CostCenterName)),40) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="90%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$resources:Percentage%>"  Visible= '<%= GetGlobalResourceObject("ConfigurationsRes","CCPercentageRequired").ToString() == "0" ? false : true %>' >--%>
                                        <asp:TemplateField HeaderText="<%$resources:Percentage%>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPercentage" runat="server" CssClass="Uiinput-amount numeric"
                                                    onkeyup="CalculateTotalCC();" onmousedown="CalculateTotalCC();" MaxLength="16"
                                                    Text='<%#Eval("FCM_VALUE")%>' onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);">
                                                </asp:TextBox>
                                                <%--   <cc1:AmountValidation ID="vamPercentage" runat="server" ControlToValidate="txtPercentage"
                                ErrorMessage="Invalid Percentage" NumberDigits="11"
                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Account"></cc1:AmountValidation>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="70%" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="input-w24 amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPercentageFooter" AutoPostBack="true" CssClass="Uiinput-amount numeric"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="Account" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfIsAllocate" runat="server" />
                     <asp:HiddenField ID="hdfAddParentChildValidation" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
