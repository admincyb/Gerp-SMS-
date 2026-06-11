<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="TaskCategory.aspx.cs" Inherits="HRMS.TaskTracker.TaskCategory" Theme="Classic" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            ShowHideTaskDetails($("[id$=hdfTaskDtl]").val());
            ToggleBtnSave();
            $("[id$=txtDuration]").ForceNumericOnly();
        }

        function ShowHideTaskDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divTaskDetails]").show();
                $("[id$=imbShowTaskDetails]").hide();
                $("[id$=imbHideTaskDetails]").show();
            }
            else {
                $("[id$=divTaskDetails]").hide();
                $("[id$=imbShowTaskDetails]").show();
                $("[id$=imbHideTaskDetails]").hide();
            }
            $("[id$=hdfTaskDtl]").val(flag);
            return false;
        }

        function ShowListing(flag) {
            if (flag == 1) {
                $("[id$=CategoryListing]").show();
                $("[id$=CategoryDetail]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=CategoryListing]").hide();
                $("[id$=CategoryDetail]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }

        function ToggleBtnSave() {
            if ($("[id$=hdfStatus]").val() > 0) {
                $('[id$=btnSave]').hide();
            }
            else {
                $('[id$=btnSave]').show();
            }
        }
        //For Setting/Resetting Colour of a selected Row
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdCategory.ClientID %> input[type=hidden][id*=hdfCategoryPK]").each(function (index) {
                        if ($.trim($(this).val()) == selectedIdsArray[i]) {
                            var selectedRowColor;
                            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';
                            $(this).closest('tr').css('background-color', selectedRowColor);

                        }

                    });
                }

            }
        }
        //End

        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper">
        <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
            <ContentTemplate>
                <div class="fixed-buttons" style="padding-bottom: 30px !important;">
                    <div class="Button-container" style="padding: 0px !important;">
                        <asp:Table ID="Table1" runat="server">
                            <asp:TableRow>
                                <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                                <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                    <ul class="bredcrum">
                                        <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                    </ul>
                                    <ul runat="server" id="pnlListing" style="display: none">
                                        <li runat="server" id="pnlNew">
                                            <asp:Button runat="server" ID="btnNew" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                                OnClick="ActionHandler" ToolTip="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel"
                                                SkinID="btnInner-New" />
                                        </li>
                                        <li runat="server" id="pnlEdit">
                                            <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                                ToolTip="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                                ValidationGroup="SaveTask" OnClick="ActionHandler" />
                                        </li>
                                    </ul>
                                    <ul runat="server" id="pnlEntry" style="display: none">
                                        <li runat="server" id="pnlSubmit">
                                            <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" Text="<%$resources:Controls,Submit %>"
                                                ToolTip="<%$resources:Controls,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit"
                                                ValidationGroup="Category" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Category')" />
                                        </li>
                                        <li runat="server" id="pnlSave">
                                            <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                                ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                                ValidationGroup="Category" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Category')" />
                                        </li>
                                        <li runat="server" id="pnlCancel">
                                            <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                                CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                                SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler" />
                                        </li>
                                    </ul>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                </div>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="14" CommandName="CATEGORYLIST" CssClass="tab-active"
                                OnClick="ActionHandler"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="15" CommandName="CATEGORYDETAIL"
                                CssClass="tab-inactive" OnClick="ActionHandler"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <div id="CategoryListing">
                    <div class="gridwrap">
                        <asp:GridView ID="grdCategory" runat="server" AutoGenerateColumns="False" PageSize="10"
                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                            Width="100%" ShowFooter="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" AutoPostBack="true"
                                            OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                        <asp:HiddenField runat="server" ID="hdfCategoryPK" Value='<%#Eval("TCT_PK") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:CategoryName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbTaskName" runat="server" Text='<%# Eval("TCT_NAME") %>' ToolTip='<%# Eval("TCT_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:CategoryDescription %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TCT_DESC"),32) %>'
                                            ToolTip='<%# Eval("TCT_DESC") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Active %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbActive" runat="server" Text='<%# ((byte)Eval("TCT_ACTIVE"))==1 ?"Active":"Inactive" %>'
                                            ToolTip='<%# ((byte)Eval("TCT_ACTIVE"))==1 ?"Active":"Inactive" %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div id="CategoryDetail">
                    <table>
                        <tr>
                            <td>
                                <div class="divcol-S">
                                    <asp:Label runat="server" ID="lbCategory" Text="<%$ Resources:CategoryName %>" AssociatedControlID="txtCategory"></asp:Label>
                                    <asp:TextBox ID="txtCategory" runat="server"> </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vrfCategory" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="Category" EnableClientScript="true" runat="server" ControlToValidate="txtCategory"
                                        Display="Dynamic" Text="*" ErrorMessage="Enter Category">
                                    </asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="divcol-S">
                                    <asp:Label runat="server" ID="lbCategoryDescription" Text="<%$ Resources:CategoryDescription %>"
                                        AssociatedControlID="txtCategoryDescription"></asp:Label>
                                    <asp:TextBox ID="txtCategoryDescription" runat="server" TextMode="MultiLine" Height="30px"> </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="divcol-S">
                                    <asp:Label runat="server" ID="lblGroup" Text="<%$ Resources:Group %>"
                                        AssociatedControlID="ddlGroup"></asp:Label>
                                    <asp:DropDownList ID="ddlGroup" runat="server" Width="200px">
                                    </asp:DropDownList>
                                    <%--<asp:HiddenField ID="TCT_GROUP" runat="server" />--%>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="divcol-S">
                                    <asp:Label runat="server" ID="lbCategoryStatus" Text="<%$ Resources:Active %>" AssociatedControlID="chkCategoryActive"></asp:Label>
                                    <asp:CheckBox ID="chkCategoryActive" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="search-colapse-b">
                        <h1>
                            Task Details
                        </h1>
                        <asp:ImageButton runat="server" ID="imbShowTaskDetails" OnClientClick="javascript:return ShowHideTaskDetails(1);"
                            SkinID="imbArrowInactive" ToolTip="ShowItemDetails" />
                        <asp:ImageButton runat="server" ID="imbHideTaskDetails" OnClientClick="javascript:return ShowHideTaskDetails();"
                            Style="display: none" SkinID="imbArrowActive" ToolTip="HideItemDetails" />
                        <asp:HiddenField ID="hdfTaskDtl" runat="server" Value="0" />
                        <div class="clear">
                        </div>
                    </div>
                    <div id="divTaskDetails" style="display: none">
                        <table class="table-3devide">
                            <tr>
                                <td colspan="3">
                                    <div class="divcol-S">
                                        <asp:HiddenField ID="hdfTaskPK" runat="server" Value="0" />
                                        <asp:Label runat="server" ID="lbTaskName" Text="<%$ Resources:TaskName %>" AssociatedControlID="txtTaskName"></asp:Label>
                                        <asp:TextBox ID="txtTaskName" runat="server"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfTask" CssClass="star" SetFocusOnError="true" ValidationGroup="Task"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtTaskName" Display="Dynamic"
                                            Text="*" ErrorMessage="Enter Task Name">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="divcol-S">
                                        <asp:Label runat="server" ID="lbTaskDesc" Text="<%$ Resources:TaskDescription %>"
                                            AssociatedControlID="txtTaskDesc"></asp:Label>
                                        <asp:TextBox ID="txtTaskDesc" runat="server" TextMode="MultiLine" Height="30px"> </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                    </div>
                    </td> </tr>
                    <tr>
                        <td>
                            <div style="padding-left:50px;">
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lbDuration" Text="<%$ Resources:TaskDuration %>" AssociatedControlID="txtDuration"></asp:Label>
                                    <asp:TextBox ID="txtDuration" runat="server" Width="100px"> </asp:TextBox>
                                    <asp:Literal ID="Literal1" Text="Days" runat="server"></asp:Literal>
                                </div>
                                <div class="clear">
                                </div>
                        </td>
                        <td>
                            <div class="div3col-S" style="text-align:right !important;padding-right:55px;">
                                <asp:Label runat="server" ID="lbTaskActive" Text="<%$ Resources:Active %>" AssociatedControlID="chkTaskActive"></asp:Label>
                                <asp:CheckBox ID="chkTaskActive" runat="server" />
                            </div>
                            <div class="clear">
                            </div>
                        </td>
                        <td>
                            <div class="div3col-S" style="text-align:right !important; padding-right:55px;">
                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="27"
                                    OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry"
                                    ValidationGroup="Task" SkinID="plus" OnClientClick="javascript:ValidateNow('Task')" />
                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="28"
                                    OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="PageAction_Entry"
                                    SkinID="cancel" />
                            </div>
                            <div class="clear">
                            </div>
                </div>
                </td> </tr> </table> </div>
                <div class="gridwrap">
                    <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" PageSize="10"
                        AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                        Width="100%" ShowFooter="true">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:TaskName %>">
                                <ItemTemplate>
                                    <asp:Label ID="lbTaskName" runat="server" Text='<%# Eval("TCI_ITEM") %>' ToolTip='<%# Eval("TCI_ITEM") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:TaskDescription %>">
                                <ItemTemplate>
                                    <asp:Label ID="lbDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TCI_DESC"),45) %>'
                                        ToolTip='<%# Eval("TCI_DESC") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:TaskDuration %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("TCI_DURATION") %>' ToolTip='<%# Eval("TCI_DURATION") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:TaskSequence %>">
                                <ItemTemplate>
                                    <asp:Label ID="lbSequence" runat="server" Text='<%# Eval("TCI_SEQUENCE") %>' ToolTip='<%#Eval("TCI_SEQUENCE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Active %>">
                                <ItemTemplate>
                                    <asp:Label ID="lbActive" runat="server" Text='<%# ((byte)Eval("TCI_ACTIVE"))==1 ?"Active":"Inactive" %>'
                                        ToolTip='<%# ((byte)Eval("TCI_ACTIVE"))==1 ?"Active":"Inactive" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditItem" runat="server" CommandName="EDITITEM" CommandArgument="PageAction_Entry"
                                        SkinID="imbeditgrid" ToolTip="Edit" TabIndex="28" OnClick="ActionHandler" /><%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                    <asp:ImageButton ID="btnRemoveItem" runat="server" CommandName="REMOVEITEM" CommandArgument="PageAction_Entry"
                                        OnClientClick="return ShowDeleteConfirm(this);" SkinID="imbdeletegrid" ToolTip="Delete"
                                        TabIndex="29" OnClick="ActionHandler" />
                                    <%--OnPreRender="btnAction_PreRender"
                                                    OnLoad="btnAction_Load"--%>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
                </div>
                <div id="diverrorAlert" style="display: none">
                    <asp:ValidationSummary ID="vsCategory" ValidationGroup="Category" runat="server" />
                    <asp:ValidationSummary ID="vsTask" ValidationGroup="Task" runat="server" />
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
