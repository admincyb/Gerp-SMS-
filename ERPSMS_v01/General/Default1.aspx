<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="Default1.aspx.cs" Inherits="ERPSMS_v01.General.Default1" Theme="ClassicExt" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function InitComponents() {
            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            SetGridScroll();
        }
        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function ShowMappingDetails() {
            ShowContainerDiv('[id$=divMappingDetails]', '<%= GetLocalResourceObject("NextLvlAction") %>', '600', '400');
        }
        function AfterGridExpand(row) {

            if ($("[id$=grdTask]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedTask]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnTaskDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
        }
        function SetGridScroll(rowId) {
            if (rowId)
                rowArray = $("[id$=" + rowId + "]");
            else
                rowArray = $("[id$=_ExpandPosition]");
            rowArray.each(function () {
                if ($.trim($(this).val()) != "") {
                    var containerDiv = $(this).parent("[id$=_ScrollContainer]");
                    if (containerDiv != null) {
                        $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
                    }
                }
                $(this).val("")
            });
        }
        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
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

        function EnableRadioButtonGrouping(sender) {
            var IsChecked = sender.checked;
            var CurrentRdbID = sender.id;
            $("#[id*=grdSeqActionDetails] input[type=radio][id*=rbtIsDefault]").each(function (index) {
                var id = $(this).closest('tr').find("#[id*=rbtIsDefault]").attr("id");
                if (id != CurrentRdbID) {
                    $(this).closest('tr').find("#[id*=rbtIsDefault]").attr("checked", false);
                }
            });
        }

        function ShowWorkflowTransactions() {
            ShowContainerDiv('[id$=divWkfTrx]', '<%= GetLocalResourceObject("WorkflowRelData") %>', '900', '450');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupCategoryDefectMapping" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell  OnClick="ActionHandler"  --%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            CommandName="SAVE" OnClick="ActionHandler" ToolTip="Save" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            CommandName="CANCEL" OnClick="ActionHandler" ToolTip="Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div id="divUserLogin" runat="server">
                <table class="table-devide" id="Table2">
                    <tr>
                        <td>
                            <div class="div2col-S padgtop7 margn-btm0">
                                <asp:Label ID="lblPassword" runat="server" Text="Enter Password" AssociatedControlID="txtPassword"></asp:Label>
                                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CommandName="SUBMIT" OnClick="ActionHandler" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="content-wrapper">
                <div id="divWorkflowDetails" runat="server">
                    <div id="grdTable-wrap">
                        <table class="table-devide" id="Group" runat="server">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblDepartment" runat="server" AssociatedControlID="ddlDepartment"
                                            Text="<%$ resources:Department %>" />
                                        <asp:DropDownList runat="server" ID="ddlDepartment" CssClass="select-half-a" OnSelectedIndexChanged="ActionHandler"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblProcess" runat="server" AssociatedControlID="ddlProcess" Text="<%$ resources:Process %>" />
                                        <asp:DropDownList runat="server" ID="ddlProcess" OnSelectedIndexChanged="ActionHandler"
                                            CssClass="select-half-a" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--Hierarchical grid--%>
                    <div class="gridwrap hierarchical-wrap2" id="divTask_ScrollContainer" grid="grdTask">
                        <asp:HiddenField ID="hdfTask_ExpandPosition" runat="server" />
                        <cc1:ExtGridView runat="server" ID="grdTask" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                            ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                            ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Level%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLevel" runat="server" Text='<%# Eval("wsqLevel") %>'></asp:Label>
                                        <asp:Button runat="server" ID="btnTaskDetails" OnClick="ActionHandler" CommandName="TASKACTIONS"
                                            CommandArgument='<%# Eval("wsqTask") %>' EnableTheming="false" Style="display: none" />
                                        <asp:HiddenField runat="server" ID="hdfIsExpandedTask" Value="0" />
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Task%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTaskName" Width="100%" runat="server" Text='<%# Eval("wsqTaskText") %>'></asp:TextBox>
                                        <asp:HiddenField ID="hdfTskPK" runat="server" Value='<%# Eval("wsqTask") %>' />
                                        <asp:HiddenField ID="hdfNextTask" runat="server" Value='<%# Eval("wsqNextTask") %>' />
                                        <asp:HiddenField ID="hdfTskProcess" runat="server" Value='<%# Eval("wsqProcess") %>' />
                                        <asp:HiddenField ID="hdfLevel" runat="server" Value='<%# Eval("wsqLevel") %>' />
                                        <asp:HiddenField ID="hdfIsInitial" runat="server" Value='<%# Eval("wsqIsInitial") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="43%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Description%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("wsqTaskDesc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="48%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>                                  
                                        <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,VIEW %>"
                                            SkinID="btnview" TabIndex="14" CommandName="VIEW" OnClick="ActionHandler"
                                            Visible='<%# ((Convert.ToInt32(Eval("wsqHasEntry"))) > 0? true:false) %>' />
                                        <asp:ImageButton runat="server" TabIndex="14" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                            SkinID="imbdeletegrid" CommandName="DELETETASK" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                            Visible='<%# ((Convert.ToInt32(Eval("wsqHasEntry"))) > 0? false:true) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="gridwrap hierarchical-wrap3">
                                            <%--class="hierarchical-gridwrap"--%>
                                            <asp:GridView runat="server" ID="grdAction" AutoGenerateColumns="False" GridLines="None"
                                                EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" OnRowDataBound="ActionHandler">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:Sequence%>">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSequence" runat="server" Width="50px" MaxLength="2" Text='<%# Eval("wsqTaskActionSeq") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="5%" BackColor="#bfedff" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:CurrentAction%>">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTaskAction" Width="100%" runat="server" Text='<%# Eval("wsqTaskActionText") %>'></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hdfTaskAction" Value='<%# Eval("wsqTaskAction") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfWsqTask" Value='<%# Eval("wsqTask") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                            <asp:HiddenField runat="server" ID="hdfWsqPK" Value='<%# Eval("wsqPK") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfNextTask" Value='<%# Eval("wsqNextTask") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfType" Value='<%# Eval("wsqType") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="37%" BackColor="#bfedff" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:NextTask%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextTask" runat="server" Text='<%# Eval("wsqNextTaskText") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="33%" HorizontalAlign="Left" BackColor="#bfedff" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:NextTaskLevel%>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaskLevel" runat="server" Text='<%# Eval("wsqNextTaskLevel") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" HorizontalAlign="Left" BackColor="#bfedff" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="imbMap" ToolTip="Map Nest Action Task" SkinID="btnattach"
                                                                TabIndex="14" CommandName="ACTIONMAPPING" OnClick="ActionHandler" CommandArgument='<%# Eval("wsqTaskAction") %>' />
                                                            <%-- <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                                SkinID="imbeditgrid" TabIndex="14" CommandName="EDITTASK" />
                                                            <asp:ImageButton runat="server" TabIndex="14" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                                SkinID="imbdeletegrid" CommandName="DELETETASK" OnClientClick="return ShowDeleteConfirm(this);" />--%>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" BackColor="#bfedff" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <%--Second--%>
                                                <RowStyle CssClass="table-secondlevel" />
                                                <HeaderStyle CssClass="table-secondlevela" />
                                            </asp:GridView>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="nopadding" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="table-firstlevel" />
                            <HeaderStyle CssClass="table-firstlevela" />
                        </cc1:ExtGridView>
                    </div>
                </div>
                <div id="divMappingDetails" style="display: none;">
                    <div class="Button-container-popup">
                        <asp:Button runat="server" ID="btnApply" ValidationGroup="RateApply" CommandName="APPLYMAPPING"
                            TabIndex="23" Text="<%$resources:ErpRes,Apply %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>"
                            SkinID="btnInner-ok" />
                        <asp:Button runat="server" ID="btnCancelPopup" CommandName="CANCELPOPUP" TabIndex="24"
                            Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>"
                            SkinID="btnInner-Cancel" />
                    </div>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdSeqActionDetails" Width="100%" AllowSorting="True"
                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" ShowHeader="true"
                            OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:IsMapped%>">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsMapped" runat="server" Checked='<%#  Eval("sqaIsMapped").ToString() == "1" ? true : false %>' />
                                        <asp:HiddenField ID="hdfSqaTaskAction" runat="server" Value='<%# Eval("sqaTaskAction")%>' />
                                        <asp:HiddenField ID="hdfsqaType" runat="server" Value='<%# Eval("sqaType")%>' />
                                        <asp:HiddenField ID="hdfsqaSequence" runat="server" Value='<%# Eval("sqaSequence")%>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:ActionText%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAction" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("sqaTaskActionText")) %>'
                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("sqaTaskActionText")) %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="70%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:IsDefault%>">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbtIsDefault" CssClass="rdoSelection" runat="server" GroupName="SelectOne"
                                            Checked='<%# Eval("sqaIsDefault").ToString() == "1" ? true : false %>' onclick="EnableRadioButtonGrouping(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div id="divWkfTrx" style="display: none;">
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdWkfTrx" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" ShowHeader="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrxDate" runat="server" Text='<%# Convert.ToDateTime(Eval("wtdModOn")).ToString(Resources.Constants.ReportDateTimeFormat) %>'
                                            ToolTip='<%# Convert.ToDateTime(Eval("wtdModOn")).ToString(Resources.Constants.ReportDateTimeFormat) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:TaskName%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaskText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("wtdTaskText")) %>' ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("wtdTaskText")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Message%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("wtdMessage")) %>' ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("wtdMessage")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="55%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />
                    <asp:ValidationSummary ID="vsSearch" ValidationGroup="Search" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
