<%@ Page Title="<%$ Resources:Captions,Title_ProductionBatchMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ProductionBatchMaster.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.ProductionBatchMaster"
    Theme="ClassicExt" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .lbl-35perc {
            min-width: 35% !important;
            max-width: 35% !important;
        }
    </style>
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtRevision]").ForceNumersOnly();
            $("[id*=txtRevisionFlter]").ForceNumersOnly();
            //ShowHideAdvancedSearch(1);
            GrandScriptUtils.DatePickerCommon("txtDate", "M-yy");
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
                                <ul runat="server" id="pnlEntry">
                                    <%--style="display: none"--%>

                                    <li>
                                        <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            ToolTip="<%$Resources:Controls,Save%>" EnableViewState="False" CommandName="SAVE"
                                            OnClick="ActionHandler" ValidationGroup="Save" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="2" />
                                    </li>
                                    <%-- <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandName="DELETE" SkinID="btnInner-Delete" ToolTip="<%$resources:Controls,Delete %>"
                                            TabIndex="35" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                            TabIndex="2" />
                                    </li>
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
                                            <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date%>" AssociatedControlID="txtDate"
                                                CssClass="input-w24-9per"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="medium" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" Text="*" ErrorMessage="<%$ resources:Err_SelectDate%>"
                                                CssClass="star" ControlToValidate="txtDate" ValidationGroup="Save">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblBatch" Text="<%$ resources:BatchNo%>" AssociatedControlID="txtBatchNo"
                                                CssClass="middle-lbl-xsmall-c2"></asp:Label>
                                            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="lbl-35perc" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBatchNo" runat="server" Text="*" ErrorMessage="<%$ resources:Err_EnterBatchNo%>"
                                                CssClass="star" ControlToValidate="txtBatchNo" ValidationGroup="Save">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>" AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="1" MaxLength="450" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="chkActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="1" CssClass="lbl-3-7perc style-none" />
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
                                                TabIndex="3" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="3" />
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
                                            <asp:Label runat="server" ID="lblBatchNoFilter" Text="<%$ resources:BatchNo%>" AssociatedControlID="txtBatchNoFilter"
                                                CssClass="bl-14-7perc"></asp:Label>
                                            <asp:TextBox ID="txtBatchNoFilter" runat="server" CssClass="medium" TabIndex="3"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <%--<tr>
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
                                           
                                        </div>
                                    </td>
                                </tr>--%>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdProductionBatchList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" CssClass="grdTable"
                                    OnSorting="ActionHandler" OnRowDeleting="grdProductionBatchList_RowDeleting">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdPBN_PK" runat="server" Value='<%# Eval("PBN_PK") %>' />
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("PBN_DATE",Resources.Constants.DateFormatGridMonthYear) %>' ToolTip='<%# Eval("PBN_DATE",Resources.Constants.DateFormatGridMonthYear)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BatchNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBatchNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PBN_BATCH_NO")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PBN_BATCH_NO")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PBN_BATCH_DESC")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PBN_BATCH_DESC")))%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" OnClick="ActionHandler" CommandName="EDIT_ACTION" TabIndex="6"/>
                                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>" OnClick="ActionHandler" CommandName="DELETE" TabIndex="6"/>
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbActive" SkinID="btnactive"
                                                    CommandName="INACTIVATE" Visible='<%# (Eval("PBN_IS_ACTIVE").ToString() == "1") ? Convert.ToBoolean("true") : Convert.ToBoolean("false") %>'
                                                    Enabled="true" OnClick="ActionHandler" Style="cursor: default;" ToolTip="Active" TabIndex="6"/>
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbInActive" SkinID="btninactive"
                                                    CommandName="ACTIVATE" Visible='<%# (Eval("PBN_IS_ACTIVE").ToString() == "0") ? Convert.ToBoolean("true") : Convert.ToBoolean("false") %>'
                                                    Enabled="true" OnClick="ActionHandler" Style="cursor: default;" ToolTip="Inactive" TabIndex="6"/>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                     <%--   <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>" OnClick="ActionHandler" CommandName="DELETE" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>--%>
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
