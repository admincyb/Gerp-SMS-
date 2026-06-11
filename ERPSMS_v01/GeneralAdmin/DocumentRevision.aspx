<%@ Page Title="<%$ Resources:Captions,Title_DocumentRevision %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DocumentRevision.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.DocumentRevision"
    Theme="ClassicExt" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*#ctl00_MainContent_rdoType, #ctl00_MainContent_rdoTypeFlter {
            background: none;
            border: none;
            margin-top: -3px;
        }*/
        .inlineCheckBox label {
            white-space: nowrap;
            /*padding-right: 150px;*/
        }

        .inlineCheckBox {
            white-space: nowrap !important;
            /*padding-right: 150px !important;*/
        }

        #ctl00_MainContent_lblType, #ctl00_MainContent_lblTypeFlter {
            background: none !important;
            border: none !important;
            text-align: right;
            margin-right: 2% !important;
        }

        lbl-17-5perc-06-10-2020-01 {
            text-align: right;
        }

        .lbl-17-5perc-06-10-2020-01 {
            min-width: 40% !important;
            max-width: 40% !important;
        }

        #ctl00_MainContent_txtRevisionFlter {
            max-width: 13.4% !important;
        }
    </style>
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtRevision]").ForceNumersOnly();
            $("[id*=txtRevisionFlter]").ForceNumersOnly();
            //ShowHideAdvancedSearch(1);
            GrandScriptUtils.DatePickerCommon("txtRevisionDate");
            GrandScriptUtils.DatePickerCommon("txtRevisionDateFlter");
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
            $("[id$=hdfShow]").val(flag);
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

                                    <%--                                    <li>
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
                                    </li>--%>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <%-- <li>
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                  
                                    <li runat="server" id="pnlEdit">
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="152" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    --%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--Container for List and Detail tabs--%>
                <%--  <div class="tab-container-floating">
              
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
                </div>--%>
                <asp:Table runat="server" ID="Table2" CssClass="tablelayout asptbllinks">

                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <div class="content">

                                                <div style="display: flex; align-items: center;">
                                                    <asp:Label Style="text-align: right;" runat="server" ID="lblType" align="Right"
                                                        Text="<%$resources:Type %>" CssClass="input-w24-9per"></asp:Label>

                                                    <asp:RadioButtonList ID="rdoType" OnSelectedIndexChanged="ActionHandler" CssClass="inlineCheckBox" AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="<%$Resources:BindValues,MISReport%>" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$Resources:BindValues,OutputPrint%>" Value="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblReport" Text="<%$ resources:Report%>" AssociatedControlID="ddlReport"
                                                CssClass="lbl-16-7perc"></asp:Label>
                                            <asp:DropDownList ID="ddlReport" runat="server" CssClass="select-medium-a" TabIndex="2" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfReport" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" InitialValue="-1"
                                                runat="server" ControlToValidate="ddlReport" Display="Dynamic" Text="*"
                                                ErrorMessage="<%$ resources:Err_Report %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDocNo" Text="<%$ resources:DocumentNo%>" AssociatedControlID="txtDocumentNo"
                                                CssClass="input-w24-9per"></asp:Label>
                                            <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="medium" TabIndex="3"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfDocNo" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_EnterDocNo%>" CssClass="star" ControlToValidate="txtDocumentNo"
                                                ValidationGroup="Save" Display="Static">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblRevision" Text="<%$ resources:Revision%>" AssociatedControlID="txtRevision" CssClass="lbl-16-7perc"></asp:Label>
                                            <asp:TextBox ID="txtRevision" runat="server" CssClass="medium" TabIndex="4" MaxLength="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRevision" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_EnterRevision%>" CssClass="star" ControlToValidate="txtRevision"
                                                ValidationGroup="Save" Display="Static">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDocName" Text="<%$ resources:DocumentName%>" AssociatedControlID="txtDocumentName"
                                                CssClass="input-w24-9per"></asp:Label>
                                            <asp:TextBox ID="txtDocumentName" runat="server" CssClass="select-medium-a" TabIndex="5" Enabled="false"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">

                                            <asp:Label runat="server" ID="lblRevDate" Text="<%$ resources:RevisionDate%>" AssociatedControlID="txtRevision"
                                                CssClass="lbl-16-7perc"></asp:Label>
                                            <asp:TextBox ID="txtRevisionDate" runat="server" TabIndex="6" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="medium"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfRevDate" runat="server" Text="*"
                                                ErrorMessage="<%$ resources:Err_SelectDate%>" CssClass="star" ControlToValidate="txtRevisionDate"
                                                ValidationGroup="Save" Display="Static">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblDARNo" Text="<%$ resources:DARNumber%>" AssociatedControlID="txtDARNumber"
                                                CssClass="lbl-16-7perc"></asp:Label>
                                            <asp:TextBox ID="txtDARNumber" runat="server" CssClass="medium" TabIndex="7"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="btnSave" CommandName="SAVE" TabIndex="8"
                                                OnClick="ActionHandler"
                                                ToolTip="<%$resources:ErpRes,Add %>" ValidationGroup="Save"
                                                SkinID="plus" OnClientClick="javascript:ValidateNow('Save');" />
                                            <asp:ImageButton ID="btnClear" runat="server" TabIndex="8" Style="margin-bottom: 0px!important;"
                                                ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="9" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="9" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <%--<asp:Label ID="lblTypeFlter" runat="server" Text="<%$ resources:Type %>"
                                                AssociatedControlID="rdoTypeFlter" CssClass="input-w24-9per"></asp:Label>
                                            <asp:RadioButtonList ID="rdoTypeFlter" runat="server" TabIndex="10" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true"
                                                RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="checkbx-inline select-medium-a">
                                                <asp:ListItem Text="<%$Resources:BindValues,MISReport%>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$Resources:BindValues,OutputPrint%>" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>--%>
                                            <div class="content">

                                                <div style="display: flex; align-items: center;">
                                                    <asp:Label Style="text-align: right;" runat="server" ID="lblTypeFlter" align="Right"
                                                        Text="<%$resources:Type %>" CssClass="input-w24-9per"></asp:Label>

                                                    <asp:RadioButtonList ID="rdoTypeFlter" OnSelectedIndexChanged="ActionHandler" CssClass="inlineCheckBox" AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="<%$Resources:BindValues,MISReport%>" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="<%$Resources:BindValues,OutputPrint%>" Value="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblReportFlter" runat="server" Text="<%$ resources:Report %>"
                                                AssociatedControlID="ddlReportFlter"></asp:Label>
                                            <asp:DropDownList ID="ddlReportFlter" runat="server" CssClass="select-medium-a" TabIndex="11">
                                            </asp:DropDownList>

                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblDocNoFlter" runat="server" Text="<%$ resources:DocumentNo%>" AssociatedControlID="txtDocumentNoFlter"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDocumentNoFlter" TabIndex="12" CssClass="select-medium-a"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblRevisionFlter" runat="server" Text="<%$ resources:Revision%>" AssociatedControlID="txtRevisionFlter"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRevisionFlter" TabIndex="13" Text=""></asp:TextBox>
                                            <asp:Label runat="server" ID="lblRevDateFlter" Text="<%$ resources:RevisionDate%>" AssociatedControlID="txtRevisionDateFlter"
                                                CssClass="lbl-16-7perc"></asp:Label>
                                            <asp:TextBox ID="txtRevisionDateFlter" runat="server" TabIndex="14" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="15"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="16" OnClick="ActionHandler"
                                                CommandName="CANCEL" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdRevisionList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler"   >
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DRM_DOC_TYPE")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DRM_DOC_TYPE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Report %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReport" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("DRM_DOC_NAME")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("DRM_DOC_NAME")))%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DocumentNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocumentNo" runat="server" Text='<%# Eval("DRM_DOC_NO") %>' ToolTip='<%# Eval("DRM_DOC_NO")%>'></asp:Label>
                                                 <asp:HiddenField ID="hdfDRM_PK" runat="server" Value='<%# Eval("DRM_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Revision %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRevision" runat="server" Text='<%# Eval("DRM_REVISION") %>' ToolTip='<%# Eval("DRM_REVISION")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RevisionDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRevisionDate" runat="server" Text='<%# Eval("DRM_REV_DT",Resources.Constants.DateFormatGridExpanded) %>' ToolTip='<%# Eval("DRM_REV_DT",Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DARNumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDARNumber" runat="server" Text='<%# Eval("DRM_DAR_NO") %>' ToolTip='<%# Eval("DRM_DAR_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" OnClick="ActionHandler" CommandName="EDIT" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>" OnClick="ActionHandler" CommandName="REVISIONDELETE" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:HiddenField ID="hdfShow" runat="server" />
                <asp:HiddenField ID="hdfRowPk" runat="server" Value="0" />
                <%--  <asp:HiddenField ID="hdfAppSubType" runat="server" />--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
