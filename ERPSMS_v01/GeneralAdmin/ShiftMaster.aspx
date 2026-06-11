<%@ Page Title="<%$ Resources:Captions,Title_ShiftMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ShiftMaster.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.ShiftMaster"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtSequence]").ForceNumersOnly();
            ShowHideAdvancedSearch(1);
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", false, false);
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


        function ShowHideAdvancedSearch(flag) {
            if (flag == 1) {
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


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <%--Top Buttons "Save", ...--%>
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <%-- <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSEND" CommandName="SEND" TabIndex="150" Text="<%$resources:Controls,Send %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('SEND')" ValidationGroup="SEND"
                                            ToolTip="<%$resources:Controls,SEND %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            ToolTip="<%$Resources:Controls,Save%>" EnableViewState="False" CommandName="SAVE"
                                            OnClick="ActionHandler" ValidationGroup="ShiftSave" OnClientClick="javascript:ValidateNow('ShiftSave')"
                                            TabIndex="35" />
                                    </li>
                                      <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandName="DELETE" SkinID="btnInner-Delete" ToolTip="<%$resources:Controls,Delete %>"
                                            TabIndex="35" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                            TabIndex="35" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <%-- <li>
                                        <asp:Button runat="server" TabIndex="153" ID="btnResend" CommandName="RESEND" OnClick="ActionHandler"
                                            Text="<%$resources:ReSend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:ReSend %>" />
                                    </li>--%>
                                    <%--  <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="154" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>--%>
                                    <li runat="server" id="pnlEdit">
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="152" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                   <%-- <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandName="DELETE" SkinID="btnInner-Delete" ToolTip="<%$resources:Controls,Delete %>"
                                            TabIndex="152" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="155" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="156" OnClick="ActionHandler" CommandName="EDIT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="Table2" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="2" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:Code%>" AssociatedControlID="txtCodeFilterList"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCodeFilterList" TabIndex="2" CssClass="input-small margnbotm0"></asp:TextBox>
                                            <asp:Label ID="lblNameFilterList" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtNameFilterList"
                                                CssClass="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNameFilterList" TabIndex="2" CssClass="input-w34per margnbotm0"></asp:TextBox>
                                             </div>
                                       </td>
                                       <td>
                                        <div class="div2col-S padgtop7">
                                              <asp:Label ID="lblShiftType" runat="server"  Text="<%$ resources:ShiftType %>"
                                               AssociatedControlID="ddlShiftType"></asp:Label>
                                        <asp:DropDownList ID="ddlShiftType" runat="server" CssClass="select-medium-a" TabIndex="20">
                                        <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1" ></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>    </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="LIST" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCompanyList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfShiftPk" Value='<%# Eval("SHF_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShiftCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShftCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("SHF_CODE")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SHF_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShiftName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShftName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("SHF_NAME")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SHF_NAME")))%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:From %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShftFrom" runat="server" Text='<%# Eval("SHF_TIME_FROM") %>' ToolTip='<%# Eval("SHF_TIME_FROM")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:To %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShftTo" runat="server" Text='<%# Eval("SHF_TIME_TO") %>' ToolTip='<%# Eval("SHF_TIME_TO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProductionShift %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShftPrdn" runat="server" Text='<%# (Eval("SHF_IS_PRODUCTION")).ToString()=="1"?"Yes" :"No"%>'
                                                    ToolTip='<%# (Eval("SHF_IS_PRODUCTION")).ToString()=="1"?"Yes" :"No"%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Sequence %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShftSequence" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("SHF_SEQUENCE")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SHF_SEQUENCE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>">
                                            <ItemTemplate>
                                                <image id="imgStatus" title='<%# (Eval("SHF_ACTIVE")).ToString()=="1"? Resources.ErpRes.Active :Resources.ErpRes.InActive %>'
                                                    class='<%# (Eval("SHF_ACTIVE")).ToString()=="1"?"active" :"inactive"%>' alt=""></image>
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
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCode" Text="<%$ resources:ShftCode%>" AssociatedControlID="txtCode"
                                                CssClass="input-w24-9per"></asp:Label>
                                            <asp:TextBox ID="txtCode" runat="server" CssClass="medium" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" Text="*" ErrorMessage="<%$ resources:Err_EnterCode%>"
                                                CssClass="star" ControlToValidate="txtCode" ValidationGroup="ShiftSave">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblName" Text="<%$ resources:ShftName%>" AssociatedControlID="txtName"
                                                CssClass="lbl-39-7perc"></asp:Label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="input-w45per" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_EnterName%>" CssClass="star" ControlToValidate="txtName"
                                                ValidationGroup="ShiftSave">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFrom" Text="<%$ resources:ShftFrom%>" AssociatedControlID="txtFrom"
                                                CssClass="input-w24-9per"></asp:Label>
                                            <asp:TextBox ID="txtFrom" runat="server" CssClass="medium" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_EnterFrom%>" CssClass="star" ControlToValidate="txtFrom"
                                                ValidationGroup="ShiftSave" Display="Static">
                                            </asp:RequiredFieldValidator>
                                            <cc1:MaskedEditExtender ID="meeBInTime2" runat="server" AutoComplete="false" Mask="99:99:99"
                                                MaskType="Time" TargetControlID="txtFrom">
                                            </cc1:MaskedEditExtender>
                                            <asp:RegularExpressionValidator runat="server" ID="regBInTime2" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ShiftSave" ControlToValidate="txtFrom" Display="Static" Text="*"
                                                ErrorMessage="<%$ resources:ValidFrom%>" ValidationExpression="([0-1]?\d|2[0-3]):([0-5]?\d):([0-5]?\d)">
                                            </asp:RegularExpressionValidator>
                                            <asp:Label runat="server" ID="lblTo" Text="<%$ resources:ShftTo%>" AssociatedControlID="txtTo"
                                                CssClass="lbl-3-7perc"></asp:Label>
                                            <asp:TextBox ID="txtTo" runat="server" CssClass="medium" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_EnterTo%>" CssClass="star" ControlToValidate="txtTo"
                                                ValidationGroup="ShiftSave" Display="Static">
                                            </asp:RequiredFieldValidator>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="false"
                                                Mask="99:99:99" MaskType="Time" TargetControlID="txtTo">
                                            </cc1:MaskedEditExtender>
                                            <asp:RegularExpressionValidator runat="server" ID="revTo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ShiftSave" ControlToValidate="txtTo" Display="Static" Text="*"
                                                ErrorMessage="<%$ resources:ValidTo%>" ValidationExpression="([0-1]?\d|2[0-3]):([0-5]?\d):([0-5]?\d)">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSequence" Text="<%$ resources:ShftSequence%>" AssociatedControlID="txtSequence"
                                                CssClass="lbl-39-7perc"></asp:Label>
                                            <asp:TextBox ID="txtSequence" runat="server" CssClass="medium" TabIndex="1" MaxLength="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_EnterSequence%>" CssClass="star" ControlToValidate="txtSequence"
                                                ValidationGroup="ShiftSave">
                                            </asp:RequiredFieldValidator>
                                           <%-- <asp:RegularExpressionValidator runat="server" ID="revSequence" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="ShiftSave" ControlToValidate="txtSequence" Display="Static"
                                                Text="*" ErrorMessage="<%$ resources:ValidSequence%>" ValidationExpression="^\d+$">
                                            </asp:RegularExpressionValidator>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:ShftDesc%>" AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="13" MaxLength="450" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                            <asp:Label ID="lblShift" runat="server" Text="<%$ resources:ShftPrdnShift%>" AssociatedControlID="chkShift"></asp:Label>
                                            <asp:CheckBox ID="chkShift" runat="server" Checked="true" TabIndex="14" CssClass="lbl-3-7perc style-none" />
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:ShftActive%>" AssociatedControlID="chkActive"
                                                CssClass="lbl-9-1perc"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="14" CssClass="lbl-3-7perc style-none" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="ShiftSave" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <%--<asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />--%>
                <asp:HiddenField ID="hdfAppType" runat="server" />
                <asp:HiddenField ID="hdfAppSubType" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
