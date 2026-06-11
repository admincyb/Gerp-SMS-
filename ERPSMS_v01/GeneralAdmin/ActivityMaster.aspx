<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" AutoEventWireup="true" CodeBehind="ActivityMaster.aspx.cs"
    ValidateRequest="false"
    Inherits="ERPSMS_v01.GeneralAdmin.ActivityMaster" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponent() {


            GrandScriptUtils.MakeAutoCompleteDDL("txtTeam", url, "hdfTeam", true, true, "TEAM");
            GrandScriptUtils.MakeAutoCompleteDDL("txtTeamSrch", url, "hdfTeamSrch", true, true, "TEAM");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCostCenter", url + "?Group=" + $("[id$='ddlGroup']").val(), "hdfCostCenterPK", true, true, "COSTCENTER");
        }
        $(document).ready(function () {
            InitComponent();
        });
        function ShowListing(flag) {
            if (flag == 1) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Mapping']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
                $("[id$='lnkMapping']").hide();

            }
            else if (flag == 2) {
                $("[id$='PageAction_Mapping']").show();
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
                $("[id$='lnkMapping']").show();
            }
            else {

                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Mapping']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
                $("[id$='lnkMapping']").show();
            }
            return false;
        }

        function ShowACDetails() {

            $("[id$='divACList']").show();
        }

        function HideACDetails() {

            $("[id$='divACList']").hide();
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
                $("[id$='pnlPrint']").hide();
            }
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
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
                    //                    else {
                    //                        Page_Validators.splice(i, 1);
                    //                    }
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
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
                        //Page_Validators.splice(i, 1);
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
        function ShowConfirmation() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Mapping_Exsists").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnSave]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $(this).dialog("close");
                        ClosePopup();
                        return false;
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="auplDetailList" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons" style="padding-bottom: 30px !important;">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" OnClick="ActionHandler"
                                            Text="<%$ resources:Controls,Save%>" ToolTip="<%$ resources:Controls,Save%>"
                                            OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="21" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="22" OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            CommandName="CANCEL" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            TabIndex="23" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="9" ID="btnNew" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="10" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                </ul>

                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAILS"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkMapping" Text="<%$ resources:MappingToolTip%>" ToolTip="<%$ resources:MappingToolTip%>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="MAPPING"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:Controls,ShowFilter%>"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:Controls,HideFilter%>"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblActivityCode" runat="server" AssociatedControlID="txtActivityCode"
                                                Text="<%$ resources:Code%>" CssClass="lbl-9perc"></asp:Label>
                                            <asp:TextBox ID="txtActivityCode" runat="server" onkeydown="return Search(event);"
                                                CssClass="select-small-e2 margnbotm0" MaxLength="50" TabIndex="2" />
                                            <asp:HiddenField ID="hdftxtActivityCode" runat="server" />

                                            <asp:Label ID="lblActivityName" runat="server" AssociatedControlID="txtActivityName"
                                                Text="<%$ resources:Name%>" CssClass="lbl-9perc"></asp:Label>
                                            <asp:TextBox ID="txtActivityName" runat="server" TabIndex="3" onkeydown="return Search(event);"
                                                CssClass="select-small-e2 margnbotm0" MaxLength="200" />
                                            <asp:HiddenField ID="hdfActivityName" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                              
                                            <asp:Label ID="lblTeamSrch" runat="server" AssociatedControlID="txtTeamSrch"
                                                Text="<%$ resources:Team%>" CssClass="lbl-9perc"></asp:Label>
                                            <asp:TextBox ID="txtTeamSrch" runat="server" onkeydown="return Search(event);"
                                                CssClass="margin-bot-0" MaxLength="50" TabIndex="4" />
                                            <asp:HiddenField ID="hdfTeamSrch" runat="server" Value="0"/>
                                             <asp:Label ID="lblMainActivitySrch" runat="server" Text="<%$ resources:MainActivity%>" AssociatedControlID="lblMainActivitySrch" CssClass="middle-lbl-xsmall-a2"></asp:Label>
                                            <asp:DropDownList ID="ddlMainActivitySrch" runat="server" CssClass="input-w170 margin-bot-0" TabIndex="4">
                                            </asp:DropDownList>

                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="5"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="6" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>

                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdActivityList" Width="100%" AllowPaging="false" PageSize="25"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="7" />
                                                <asp:HiddenField runat="server" ID="hdfActivityPk" Value='<%# Eval("EAM_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActivityCode %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EAM_CODE")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAM_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ActivityName %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EAM_NAME")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAM_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("EAM_ACTIVITY_TEXT") %>'
                                                    ToolTip='<%#Eval("EAM_ACTIVITY_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MainActivity %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCSubActicity" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EAM_MAIN_ACTIVITY_TEXT")),16) %>'
                                                    ToolTip='<%#Eval("EAM_MAIN_ACTIVITY_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Team %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCTeam" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ACM_TEAM_TEXT")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ACM_TEAM_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$ resources:ActivityDesc %> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EAM_DESC")),60) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EAM_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" CommandName="ACTIVATE"
                                                    Visible='<%# (Eval("EAM_ACTIVE").ToString() == "0") ?
                                               true  : false %>'
                                                    ToolTip="<%$ resources:Inactive %>" OnClick="ActionHandler" CssClass="Active" TabIndex="8" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" CommandName="INACTIVATE"
                                                    Visible='<%# (Eval("EAM_ACTIVE").ToString() == "1") ?
                                               true  : false %>'
                                                    ToolTip="<%$ resources:Active %>" OnClick="ActionHandler" CssClass="Active" TabIndex="8" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                            <HeaderStyle Width="1%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblcode" runat="server" Text="<%$ resources:ActivityCodeStar%>"
                                                AssociatedControlID="txtCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCode" TabIndex="11" CssClass="input-half" onkeydown="limitText(this,40);"
                                                onkeyup="limitText(this,40);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                                                CssClass="star" ValidationGroup="Save" SetFocusOnError="true" Display="Dynamic"
                                                ValidationExpression="^(.|\n){1,15}$" Text="*" ErrorMessage="<%$ resources:Err_EnterCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblName" runat="server" Text="<%$ resources:ActivityNameStar%>"
                                                AssociatedControlID="txtName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtName" TabIndex="12" CssClass="input-half" onkeydown="limitText(this,100);"
                                                onkeyup="limitText(this,100);">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                CssClass="star" ValidationGroup="Save" Text="*" SetFocusOnError="true" Display="Dynamic"
                                                ValidationExpression="^(.|\n){1,40}$" ErrorMessage="<%$ resources:Err_EnterName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:ActivityDesc%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="13" TextMode="MultiLine"
                                                Height="40" CssClass="input-full" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator runat="server" ID="revDescription" CssClass="star"
                                                ValidationGroup="CostMaster" SetFocusOnError="true" ControlToValidate="txtDescription"
                                                Display="Dynamic" ValidationExpression="^(.|\n){1,500}$" Text="*" ErrorMessage="<%$ resources:ErpRes, Msg_Exceed_MaxLen %>"
                                                EnableClientScript="true" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">

                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="14" Checked="true" />

                                            <asp:Label ID="lblMainActivity" runat="server" Text="<%$ resources:MainActivity%>" AssociatedControlID="lblMainActivity"></asp:Label>
                                            <asp:CheckBox ID="chkMainActivity" runat="server" TabIndex="14" OnCheckedChanged="ActionHandler" AutoPostBack="true" /><%--Checked="true"--%>
                                            <asp:DropDownList ID="ddlMainActivity" runat="server" CssClass="input-w170" TabIndex="15">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfMainActivity" runat="server" ControlToValidate="ddlMainActivity"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_MainActivity%>" InitialValue="-1"></asp:RequiredFieldValidator>

                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" Visible="false" ForeColor="Blue" Text="<%$ resources:Show_Maped_Accounts%>" runat="server" OnClientClick="javascript:return ShowACDetails();">
                                        </asp:LinkButton>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkShowActivityAccounts" Visible="false" ForeColor="Blue" Text="<%$ resources:Show_Maped_Accounts%>" runat="server" OnClientClick="javascript:return ShowACDetails();">
                                        </asp:LinkButton>
                                    </td>
                                </tr>--%>
                            </table>
                            <div id="divACList" class="grdTable w100perc">
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Mapping" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" style="padding-bottom: 20px;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:DepartmentStar%>" AssociatedControlID="ddlDepartment"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlDepartment" TabIndex="14" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="false" CssClass="input-half">
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment"
                                                CssClass="star" ValidationGroup="Maping" Text="*" ErrorMessage="<%$ resources:Err_AddDepartment%>"></asp:RequiredFieldValidator>--%>
                                            <asp:RequiredFieldValidator ID="rfvDepartment" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Maping" EnableClientScript="true" runat="server" ControlToValidate="ddlDepartment"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AddDepartment %>" InitialValue="-1"></asp:RequiredFieldValidator>

                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:ComanyPlantStar %>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="input-half" TabIndex="15">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddlCompany"
                                                CssClass="star" ValidationGroup="Maping" Text="*" ErrorMessage="<%$ resources:Err_AddLocation%>" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroup" runat="server" Text="<%$ resources:Group%>" AssociatedControlID="ddlGroup"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlGroup" onchange="javascript:InitComponent();" TabIndex="16" CssClass="input-half">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvGroup" runat="server" ControlToValidate="ddlGroup"
                                                CssClass="star" ValidationGroup="Maping" Text="*" ErrorMessage="<%$ resources:Err_SelectGroup%>" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>--%>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTeam" runat="server" Text="<%$ resources:Team%>" AssociatedControlID="txtTeam"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTeam" Text="" TabIndex="15" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTeam" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfTeam" InitialValue="Select/Type" CssClass="star"
                                                SetFocusOnError="true" ValidationGroup="Maping" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtTeam" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectTeam%>"></asp:RequiredFieldValidator>
                                            <asp:Button runat="server" TabIndex="18" ID="btnAddCostCenter" OnClientClick="javascript:ValidatePageNow('Maping')" ValidationGroup="Maping" CommandName="ADD" Text="<%$resources:AddCostCenter %>"
                                                CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:AddCostCenterTooltip %>"
                                                OnClick="ActionHandler" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="DivCostCenter" class="grdTable w100perc">
                                <asp:GridView runat="server" ID="GrdCostCenter" Width="100%" AllowPaging="false"
                                    PageSize="25" AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Gr_SlNo%>">
                                            <ItemTemplate>
                                                <asp:Label ID="grd_lblSlNo" runat="server" Text='<% #Eval("SL_NO") %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Gr_Department%>">
                                            <ItemTemplate>
                                                <asp:Label ID="grd_lblDepartment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ACM_DEPT_TEXT")),20) %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Gr_Group%>">
                                            <ItemTemplate>
                                                <asp:Label ID="grd_lblGroup" runat="server" Text='<%# Eval("ACM_GROUP_TEXT") %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Gr_Company%>">
                                            <ItemTemplate>
                                                <asp:Label ID="grd_lblCompany" runat="server" Text='<%# Eval("ACM_COMPANY_TEXT") %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Gr_Team%>">
                                            <ItemTemplate>
                                                <asp:Label ID="grd_lblCostCenter" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ACM_TEAM_TEXT")),20) %>'
                                                    ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfSLNo" runat="server" Value='<%#Eval("EAM_PK") %>' />
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("ACM_COMPANY_PK") %>' />
                                                <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("ACM_DEPT_PK") %>' />
                                                <asp:HiddenField ID="HiddenField3" runat="server" Value='<%#Eval("ACM_TEAM_PK") %>' />
                                                <%--<asp:HiddenField ID="HiddenField4" runat="server" Value='<%#Eval("ACM_GROUP_PK") %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Gr_Edit_Delete%>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditCostCenterMap" runat="server" OnClick="ActionHandler" CommandName="EDITCOSTCENTER"
                                                    CommandArgument='<% #Eval("SL_NO") %>' SkinID="imbeditgrid" ToolTip="Edit" TabIndex="19" />
                                                <asp:ImageButton ID="btnDelete" runat="server" OnClick="ActionHandler" CommandName="REMOVECOSTCENTER"
                                                    CommandArgument='<% #Eval("SL_NO") %>' OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="20" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsMapping" ValidationGroup="Maping" runat="server" />
            </div>
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
