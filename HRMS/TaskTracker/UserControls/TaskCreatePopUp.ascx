<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskCreatePopUp.ascx.cs"
    Inherits="HRMS.TaskTracker.UserControls.TaskCreatePopUp" %>
<script src="../../../ERPSMS_v01/Scripts/jquery/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    /// AutoComplete Users
    var pageURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    function TaskCreatePopUpInitComponents() {
        //GrandScriptUtils.DatePickerCommon("txtTaskDate");
        GrandScriptUtils.DatePickerCommon("txtSubTaskDate");
        //GrandScriptUtils.DatePickerCommon("txtMainTaskExpDate");
        GrandScriptUtils.DatePickerCommon("txtSubTaskExpDate");

        GrandScriptUtils.MakeAutoCompleteDDL("txtUser", url, "hdfUserPK", true, true, "ASSIGNTASKUSER");
        GrandScriptUtils.AddDateRangeCommon("txtSubTaskDate", "hdfTaskDate", "txtSubTaskExpDate", "hdfExpCmpDate", false, false);
        ToggleBtnSave();
    }
    function ToggleBtnSave() {
        if ($("[id$=hdfTaskMode]").val() > 0) {
            $('[id$=btnSave]').hide();
        }
        else {
            $('[id$=btnSave]').show();
        }
    }

//    function AfterClose() {
//        $('[id$=btnClosePopUp]').click();
//    }

    function AfterClose(containerID) {
        if (containerID == "[id$=divCategoryDetails]") {  
            $("[id$=btnShowTaskPopup]").click();
        }
        else {
            $('[id$=btnClosePopUp]').click();
        }
    }

    function ValidateTask(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverrorTask").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

</script>
<style type="text/css">
    .task-detail
    {
        display: inline-block;
        width: 100%;
    }
    .task-detail label
    {
        width: 30%;
    }
    .task-detail span, .task-detail input[type=text], .task-detail textarea, .task-detail select, .task-detail input[type=password]
    {
        width: 62%;
        margin-bottom: 0px !important;
    }
    .task-detail span
    {
        padding: 3px 2px;
        min-height: 14px;
        line-height: normal;
        font-weight: bold;
        color: #506c92;
    }
    .task-detail input[type=checkbox]
    {
        margin: 3px 0 0 0;
    }
    .task-detail select
    {
        width: 63.4%;
    }
    .task-detail input[type="file"]
    {
        width: auto !important;
    }
    .task-detail .lbDt
    {
        width: 40%;
    }
    .task-detail .lbShwDt
    {
        width: 50%;
    }
    /*width half*/
    .task-detail input[type=text].half
    {
        width: 30%;
    }
    .task-detail span.half
    {
        width: 30%;
    }
    
    /* single column*/
    .task-detail-col1 label
    {
        width: 15% !important;
    }
    .task-detail-col1 span, .task-detail-col1 input[type=text], .task-detail-col1 textarea, .task-detail-col1 select
    {
        width: 81%;
        margin-bottom: 0px !important;
    }
    .task-detail-col1 span
    {
        padding: 2px;
        min-height: 14px;
        line-height: normal;
        font-weight: bold;
        color: #506c92;
    }
    .task-detail-col1 select
    {
        width: 82%;
    }
    .task-detail-col1 textarea
    {
        height: 60px;
        padding-top: 1px;
    }
    
    /*Notify*/
    .notify
    {
        background: #eff0f1;
        border: 1px solid #dcdcdc;
        padding: 3px;
        margin: 10px;
    }
    
    /*blank td*/
    .blank
    {
        height: 10px;
    }
    .blank2
    {
        height: 4px;
    }
</style>
<asp:UpdatePanel runat="server" ID="aupdpopup"> 
    <ContentTemplate>
    <div class="content-wrapper">
        <asp:HiddenField ID="hdfShowContainerDiv" EnableViewState="true" runat="server" />
        <div style="display: none" >
        <asp:Button runat="server" ID="btnShowTaskPopup" CommandName="SHOW" OnClick="ActionHandler"
                Text="<%$resources:Controls,Submit %>" ToolTip="<%$resources:Controls,Submit %>"
                CommandArgument="SEC_ActionPanel"  />
                </div>
        <div class="Button-container-popup">
            <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" OnClick="ActionHandler"
                Text="<%$resources:Controls,Submit %>" ToolTip="<%$resources:Controls,Submit %>"
                CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit2" ValidationGroup="SaveTask"
                OnClientClick="javascript:ValidateTask('SaveTask')" />
            <asp:Button runat="server" ID="btnSave" CommandName="SAVE" OnClick="ActionHandler"
                Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save1" ValidationGroup="SaveTask"
                OnClientClick="javascript:ValidateTask('SaveTask')" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                CssClass="popupclose" CommandName="CANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                SkinID="btnInner-Cancel2" ToolTip="<%$resources:Controls,Cancel %>" />
            <asp:Button runat="server" ID="btnClosePopUp" Style="display: none;" CommandName="CANCELPOPUP"
                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
        </div>
        <div class="content-wrapper">
            <asp:Table runat="server" ID="tblTemplate">
                <asp:TableRow ID="MainTask" runat="server" CssClass="notify">
                    <asp:TableCell Style="padding-left: 57px;">
                        <table class="table-3devide" style="margin: 5px 0px;">
                            <tr>
                                <td colspan="2">
                                    <div class="task-detail-col1">
                                        <asp:Label runat="server" ID="lblTaskName" Text="<%$resources:MainTskName %>" AssociatedControlID="lbShowTaskName"></asp:Label>
                                        <asp:Label ID="lbShowTaskName" runat="server"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lblTaskNo" Text="<%$resources:MainTskNo %>" AssociatedControlID="lbShowTaskNo"></asp:Label>
                                        <asp:Label ID="lbShowTaskNo" runat="server"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lblTaskDate" Text="<%$resources:MainTskDt %>" AssociatedControlID="lbShowTaskDate"></asp:Label>
                                        <asp:Label ID="lbShowTaskDate" runat="server"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lbMainTskExpdt" Text="<%$resources:MainTskExpDt %>"
                                            AssociatedControlID="lbShowMainTaskExpDate" CssClass="lbDt"></asp:Label>
                                        <asp:Label ID="lbShowMainTaskExpDate" runat="server" CssClass="lbShwDt"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lbStatus" Text="<%$resources:TskStatus %>" AssociatedControlID="lbShowStatus"></asp:Label>
                                        <asp:Label ID="lbShowStatus" runat="server"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass=" blank"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="SubTask" runat="server" Style="margin-top: 20px;">
                    <asp:TableCell>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbSubtaskDate" Text="<%$resources:SubTskDt %>" AssociatedControlID="txtSubTaskDate"></asp:Label>
                                        <asp:TextBox ID="txtSubTaskDate" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                            onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfTaskDate" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="SaveTask"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtSubTaskDate" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$ resources:Err_Sel_Date %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbSubTask" Text="<%$resources:SubTskNo %>" AssociatedControlID="txtSubTaskNo"></asp:Label>
                                        <asp:TextBox ID="txtSubTaskNo" runat="server" CssClass="input-disabled" MaxLength="13"> </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbGroup" Text="<%$resources:TskGroup %>" AssociatedControlID="ddlGroup"></asp:Label>
                                        <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ActionHandler"  CommandName="CHANGE">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                    </div>
                            </td>
                            <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbCategory" Text="<%$resources:TskCategory %>" AssociatedControlID="ddlCategory"
                                        ></asp:Label>
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select-halfsmall-b margn-rgt0">                                      
                                        </asp:DropDownList>
                                        <asp:ImageButton ImageUrl="~/Images/Classic/Icons/view-top-menu.png" runat="server" ID="imbCategoryDetailsPopup" CommandName="DETAILS" OnClick="ActionHandler"
                                        SkinID="btnInner-View" style="margin-top:1px;"
                                         />
                                       <%-- <asp:ImageButton ImageUrl="imageurl" runat="server"
                                        ID="btnCategoryDetailsPopup" CommandName="DETAILS" OnClick="ActionHandler"
                                          />--%>
                                        <%--<asp:ImageButton runat="server" ID="btnCategoryDetailsPopup" SkinID="btnInner-View"
                                         CommandName="DETAILS" OnClick="ActionHandler"   />--%>
                                        <div class="clear">
                                        </div>
                                    </div>
                            </td>
                            </tr>
                            <tr>
                                <td>
                                     <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbSubTaskExpCmpDate" Text="<%$resources:SubTskExpDt %>"
                                            AssociatedControlID="txtSubTaskExpDate"></asp:Label>
                                        <asp:TextBox ID="txtSubTaskExpDate" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                            onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfExpCmpDate" runat="server" Value="" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-P">
                                        <asp:Label runat="server" ID="lbName" Text="<%$resources:SubTskName %>" AssociatedControlID="txtName"></asp:Label>
                                        <asp:TextBox ID="txtName" runat="server"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfTaskName" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="SaveTask" EnableClientScript="true" runat="server" ControlToValidate="txtName"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TskName %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-P">
                                        <asp:Label runat="server" ID="lbDescription" Text="<%$resources:TskDesc %>" AssociatedControlID="txtDescription"></asp:Label>
                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"> </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbAssign" Text="<%$resources:TskAssign %>" AssociatedControlID="txtUser"></asp:Label>
                                        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hdfUserPK" runat="server" Value="0" />
                                        <asp:RequiredFieldValidator ID="vrfAssignto" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="SaveTask" EnableClientScript="true" runat="server" ControlToValidate="txtUser"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_user %>" InitialValue="Select/Type">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div id="diverrorTask" style="display: none">
                <asp:ValidationSummary runat="server" ID="vsSaveTask" ValidationGroup="SaveTask" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <div>
                <asp:HiddenField ID="hdfParentPK" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdfSubTask" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdfTaskMode" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdfCategoryPK" runat="server" Value="0"></asp:HiddenField>
            </div>
        </div>
        <div id="divCategoryDetails" style="display: none" >
        <div class="gridwrap">
                    <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" 
                        AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                        Width="100%" >
                        <EmptyDataTemplate>
                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Task %>">
                                <ItemTemplate>
                                    <asp:Label ID="lbTaskName" runat="server" Text='<%# Eval("TCI_ITEM") %>' ToolTip='<%# Eval("TCI_ITEM") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:TskDesc %>">
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
                        </Columns>
                    </asp:GridView>
                </div>
        </div>
    </div>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="imbCategoryDetailsPopup" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
