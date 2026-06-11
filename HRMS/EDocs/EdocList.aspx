<%@ Page Title="<%$ Resources:Captions,Title_EDocList %>" Language="C#" Theme="ClassicEdoc"
    MasterPageFile="~/DOC.Master" AutoEventWireup="true" CodeBehind="EdocList.aspx.cs"
    Inherits="HRMS.EDocs.EdocList" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/GtiFolderExplorer.ascx" TagName="FileExplorer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {

            GrandScriptUtils.MakeAutoCompleteDDL("txtSender", url, "hdfSender", true, true, "EDOCEMPLOYEES");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSendTo", url, "hdfSendTo", true, true, "EDOCEMPLOYEES");

            GrandScriptUtils.AddDateRangeCommon("txtSearchDateFrom", "hdfSearchDateFrom", "txtSearchDateTo", "hdfSearchDateTo", false, false);
            if ($("[id$=hdfIsExpandSearch]").val() == "1")
                ShowHideAdvancedSearch(1);
            else
                ShowHideAdvancedSearch();
        }
        //        function afterFolderClose(containerID) {
        //            if (containerID == "[id$=gtiFolderExplorerBody]") {
        //                $("[id$=imbSelect]").click();
        //            }
        //        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=divAdvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
                $("[id$=hdfIsExpandSearch]").val("1");
            }
            else {
                $("[id$=divAdvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
                $("[id$=hdfIsExpandSearch]").val("0");
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" CssClass=" padgtop2"></asp:Label>
                                </ul>
                                <div class="top-search-main ">
                                    <asp:Label runat="server"><img alt="" src="../Images/ClassicEdoc/Icons/search-separtn.png" class="margntop2" /></asp:Label>
                                    <asp:TextBox runat="server" ID="txtSearchText" CssClass="input-half h18 margnrgt5"
                                        PlaceHolder=" Type keywords for search" TabIndex="1" />
                                    <asp:ImageButton ID="btnSearch" Text="Search" runat="server" OnClick="ActionHandler"
                                        CommandName="SEARCH" SkinID="search-ext" Style="margin-top: 3px; margin-left: 5px;"
                                        TabIndex="2" />
                                    <asp:Label ID="Label1" runat="server"><img alt="" src="../Images/ClassicEdoc/Icons/search-separtn.png" class="margntop4" /></asp:Label>
                                    <span class="padgtop2">
                                        <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%>
                                    </span>
                                    <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                        ImageUrl="~/Images/ClassicEdoc/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        CssClass="margntop3" TabIndex="2" />
                                    <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                        ImageUrl="~/images/ClassicEdoc/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        CssClass="margntop3" TabIndex="2" />
                                </div>
                                <ul runat="server" id="pnlList">
                                    <li runat="server" id="pnlNew">
                                        <asp:ImageButton runat="server" ID="btnNew" CommandName="NEW" TabIndex="13" OnClick="ActionHandler"
                                            ToolTip="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            Style="margin: 3px 10px 0px 0px;" />
                                    </li>
                                    <li runat="server" id="pnlView" style="display: none">
                                        <asp:ImageButton runat="server" ID="btnView" CommandName="VIEW" TabIndex="13" OnClick="ActionHandler"
                                            ToolTip="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            Style="margin: 3px 0px 0px 0px;" />
                                    </li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                                <asp:HiddenField runat="server" ID="hdfIsExpandSearch" Value="0" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div>
                    <div id="divAdvancedSearch" class="adv-search" style="display: none;">
                        <table class="table-devide tablelayout">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblSearchDateFrom" runat="server" Text="<%$ resources:DateFrom%>"
                                            AssociatedControlID="txtSearchDateFrom"></asp:Label>
                                        <asp:TextBox ID="txtSearchDateFrom" runat="server" TabIndex="3" MaxLength="15" onkeydown="javascript:return CheckKey(event)"
                                            CssClass="input-small" onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfSearchDateFrom" runat="server" />
                                        <asp:Label ID="lblSearchDateTo" runat="server" Text="<%$ resources:DateTo%>" AssociatedControlID="txtSearchDateTo"
                                            CssClass="middle-lbl-small-d"></asp:Label>
                                        <asp:TextBox ID="txtSearchDateTo" runat="server" TabIndex="3" CssClass="input-small"
                                            MaxLength="15" onkeydown="javascript:return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfSearchDateTo" runat="server" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblTags" runat="server" Text="<%$ resources:Tags%>" AssociatedControlID="txtSearchTags"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtSearchTags" MaxLength="500" TabIndex="4" CssClass="input-half"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblSearchLetterNo" runat="server" Text="<%$ resources:LetterNo%>"
                                            AssociatedControlID="txtSearchLetterNo"></asp:Label>
                                        <asp:TextBox ID="txtSearchLetterNo" runat="server" MaxLength="100" TabIndex="5" CssClass="input-half"> </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblFrom" runat="server" Text="<%$ resources:From %>" AssociatedControlID="ddlFrom"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlFrom" TabIndex="6" CssClass="select-half-a">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department%>" AssociatedControlID="ddlDepartment"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlDepartment" TabIndex="7" CssClass="select-half-a">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
<%--                                         <asp:Label ID="lblSend" runat="server" Text="<%$ resources:SenderGrid%>" AssociatedControlID="ddlSend"></asp:Label>
                                         <asp:DropDownList runat="server" ID="ddlSend" CssClass="select-half-a">
                                         </asp:DropDownList>--%>

                                         <asp:Label ID="lblSend" runat="server" Text="<%$ resources:SenderGrid%>" AssociatedControlID="txtSender"></asp:Label>
                                         <asp:TextBox ID="txtSender" runat="server" CssClass="select-half" ></asp:TextBox>
                                         <asp:HiddenField ID="hdfSender" runat="server" Value="0" />


                                        <div id="divFolderBrowse" runat="server" visible="false">
                                            <div class="folder-lft-div2">
                                                Folder</div>
                                            <div class="folder-rgt-div2">
                                                <uc1:FileExplorer ID="FileExplorer1" runat="server" width="300" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblSearchDocNo" runat="server" Text="<%$ resources:EDocNo%>" AssociatedControlID="txtSearchEDocNo"></asp:Label>
                                        <asp:TextBox ID="txtSearchEDocNo" runat="server" MaxLength="100" TabIndex="3" CssClass="input-small"> </asp:TextBox>
                                        <asp:Label ID="lblSearchSubject" runat="server" Text="<%$ resources:Subject%>" AssociatedControlID="txtSearchSubject"
                                            CssClass="middle-lbl-a"></asp:Label>
                                        <asp:TextBox ID="txtSearchSubject" runat="server" MaxLength="100" TabIndex="3" CssClass="input-small"> </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblSearchComment" runat="server" Text="<%$ resources:Comments%>" AssociatedControlID="txtSearchComment"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtSearchComment" MaxLength="500" TabIndex="4" CssClass="input-halfsmall"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblSearchProjectSite" runat="server" Text="<%$ resources:ProjectSite%>"
                                            AssociatedControlID="ddlProjectSite"></asp:Label>
                                        <asp:DropDownList ID="ddlProjectSite" runat="server" TabIndex="5" CssClass="select-half">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblTo" runat="server" Text="<%$ resources:To %>" AssociatedControlID="ddlTo"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlTo" TabIndex="6" CssClass="select-half">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblSearchStatus" runat="server" Text="<%$ resources:Status%>" AssociatedControlID="ddlSearchStatus"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlSearchStatus" TabIndex="7" CssClass="select-half">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                         <%--<asp:Label ID="lblSendTo" runat="server" Text="<%$ resources:SendToGrid%>" AssociatedControlID="ddlSendTo"></asp:Label>
                                         <asp:DropDownList runat="server" ID="ddlSendTo" CssClass="select-half">
                                         </asp:DropDownList>--%>

                                         <asp:Label ID="lblSendTo" runat="server" Text="<%$ resources:SendToGrid%>" AssociatedControlID="txtSendTo"></asp:Label>
                                         <asp:TextBox ID="txtSendTo" runat="server" CssClass="select-halfsmall"></asp:TextBox>
                                         <asp:HiddenField ID="hdfSendTo" runat="server" Value="0" />

                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblSearchFileTitle" runat="server" Text="<%$ resources:FileTitle%>"
                                            AssociatedControlID="txtSearchFileTitle"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtSearchFileTitle" MaxLength="500" TabIndex="7"
                                            CssClass="input-halfsmall"></asp:TextBox>
                                        <label runat="server" class="margnrgt3">
                                            &nbsp;</label>
                                        <asp:Button Text="Search" runat="server" ID="btnadvancedSearch" TabIndex="8" SkinID="btnInner-search"
                                            CommandName="SEARCH" OnClick="ActionHandler" ToolTip="<%$ resources:Search%>" />
                                        <asp:Button Text="Reset" runat="server" ID="btnClear" TabIndex="8" SkinID="btnInner-reset"
                                            CommandName="CLEAR" OnClick="ActionHandler" ToolTip="<%$ resources:Reset%>"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnSummary" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnSummary" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CommandName="SUMMARYLIST" Text="<%$resources:Summary %>" ToolTip="<%$resources:Summary %>"
                                TabIndex="11" CssClass="tab-active summary"> </asp:LinkButton>
                        </span></li>
                        <li><span id="spnPending" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnPending" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CommandName="PENDINGLIST" Text="<%$resources:Pending %>" ToolTip="<%$resources:Pending %>"
                                TabIndex="11" CssClass="tab-inactive pending"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnInProgress" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnInProgress" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CommandName="INPROGRESSLIST" Text="<%$resources:InProgress %>" ToolTip="<%$resources:InProgress %>"
                                TabIndex="11" CssClass="tab-inactive in-progress"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnFinalized" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkFinalized" Text="<%$resources:Finalized %>"
                                ToolTip="<%$resources:Finalized %>" CommandArgument="SEC_ActionPanel" CommandName="FINALIZELIST"
                                TabIndex="11" OnClick="ActionHandler" CssClass="tab-inactive finalize"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAll" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnAll" Text="<%$resources:All %>" ToolTip="<%$resources:All %>"
                                CommandArgument="SEC_ActionPanel" CommandName="SHOWALL" TabIndex="11" OnClick="ActionHandler"
                                CssClass="tab-inactive all"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnFiles" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnFiles" Text="<%$resources:Files %>" ToolTip="<%$resources:Files %>"
                                CommandArgument="SEC_ActionPanel" CommandName="SHOWFILES" TabIndex="11" OnClick="ActionHandler"
                                CssClass="tab-inactive files">
                            </asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Visible="true">
                        <asp:TableCell ID="trSummary" runat="server">
                            <div id="divSummary" runat="server">
                                <asp:Repeater runat="server" ID="rptDocSummary" OnItemDataBound="ActionHandler">
                                    <HeaderTemplate>
                                        <div class="project-heading">
                                            <h1>
                                                <img src="../Images/ClassicEdoc/Icons/projects-icon.png" alt="" />
                                                <%# GetLocalResourceObject("Projects").ToString() %></h1>
                                            <a href="EdocCreate.aspx">
                                                <%# GetLocalResourceObject("AddDocument").ToString() %></a> <span class="project-count">
                                                    Count</span>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="edoc-sumry-list">
                                            <div class="list-heading">
                                                <h3>
                                                    <%# Eval(Resources.DataFieldRes.EDocProjectSiteText)%>
                                                    <span><a href="EdocCreate.aspx?ProjID=<%# Eval(Resources.DataFieldRes.EDocProjectSite)%>">
                                                        <%# GetLocalResourceObject("AddDocument").ToString()%></a>&nbsp; | &nbsp;<a href="EdocList.aspx?TabID=3&ProjID=<%# Eval(Resources.DataFieldRes.EDocProjectSite)%>">
                                                            <%# GetLocalResourceObject("ViewAll").ToString()%></a> </span>
                                                </h3>
                                            </div>
                                            <div class="count" >
                                                <span title='<%# GetLocalResourceObject("PendingCount").ToString()%>'><%# Eval(Resources.DataFieldRes.EDocTrxPendingCount)%></span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div id="divEmptyTemplate" runat="server">
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <%--<uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trPending" runat="server" Visible="false">
                        <asp:TableCell>
                            <div id="divPending">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdPendingDocList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                        AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                         AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AutoPostBack="true" OnCheckedChanged="ActionHandler">
                                       <%-- Style="table-layout: fixed;"--%>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:DateGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DocNoGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocNo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" Text='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString()) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString())%>'
                                                        runat="server" CssClass="text-underline" OnClick="ActionHandler" CommandName="EDIT" />
                                                    <asp:HiddenField runat="server" ID="hdfEDocPk" Value='<%# Eval(Resources.DataFieldRes.EDocPk) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:LetterNo %>" SortExpression="<%$ resources:DataFieldRes,EDocLetterNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingLetterNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SenderGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSender %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSender" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SendToGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSendTo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSendTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ProjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocProjectText %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocProject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSubject %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSubject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString()),50) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,EDocStatus %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.EDocStatus) %>' />
                                                    <div id="imbStatusIndicator" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPagingPending" runat="server" />
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trInProgress" runat="server" Visible="false">
                        <asp:TableCell>
                            <div id="divInProgress">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdInProgressDocList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                        AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                         AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AutoPostBack="true" OnCheckedChanged="ActionHandler">
                                        <%--Style="table-layout: fixed;"--%>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:DateGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DocNoGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocNo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" Text='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString()) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString())%>'
                                                        runat="server" CssClass="text-underline" OnClick="ActionHandler" CommandName="EDIT" />
                                                    <asp:HiddenField runat="server" ID="hdfEDocPk" Value='<%# Eval(Resources.DataFieldRes.EDocPk) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:LetterNo %>" SortExpression="<%$ resources:DataFieldRes,EDocLetterNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingLetterNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SenderGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSender %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSender" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SendToGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSendTo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSendTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ProjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocProjectText %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocProject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSubject %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSubject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString()),50) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,EDocStatus %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.EDocStatus) %>' />
                                                    <div id="imbStatusIndicator" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPagingInProgress" runat="server" />
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trFinalized" runat="server" Visible="false">
                        <asp:TableCell>
                            <div id="divFinalized">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdFinalizedDocList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                        AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" AutoPostBack="true"
                                        OnCheckedChanged="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:DateGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DocNoGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocNo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" Text='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString()) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString())%>'
                                                        runat="server" CssClass="text-underline" OnClick="ActionHandler" CommandName="EDIT" />
                                                    <asp:HiddenField runat="server" ID="hdfEDocPk" Value='<%# Eval(Resources.DataFieldRes.EDocPk) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:LetterNo %>" SortExpression="<%$ resources:DataFieldRes,EDocLetterNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingLetterNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SenderGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSender %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSender" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SendToGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSendTo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSendTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ProjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocProjectText %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocProject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="16%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSubject %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSubject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString()),50) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="17%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,EDocStatus %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.EDocStatus) %>' />
                                                    <div id="imbStatusIndicator" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPagingFinalized" runat="server" />
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trAll" runat="server" Visible="false">
                        <asp:TableCell>
                            <div id="divAll">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdAllDocList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                        AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                         AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AutoPostBack="true" OnCheckedChanged="ActionHandler">
                                        <%--Style="table-layout: fixed;"--%>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:DateGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DocNoGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocNo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" Text='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString()) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString())%>'
                                                        runat="server" CssClass="text-underline" OnClick="ActionHandler" CommandName="EDIT" />
                                                    <asp:HiddenField runat="server" ID="hdfEDocPk" Value='<%# Eval(Resources.DataFieldRes.EDocPk) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:LetterNo %>" SortExpression="<%$ resources:DataFieldRes,EDocLetterNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingLetterNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocLetterNo).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SenderGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSender %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSender" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSender).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SendToGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSendTo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSendTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSendTo).ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ProjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocProjectText %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocProject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocProjectText).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SubjectGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocSubject %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingDocSubject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString()),50) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocSubject).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,EDocStatus %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.EDocStatus) %>' />
                                                    <div id="imbStatusIndicator" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPagingAllDocs" runat="server" />
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trFiles" runat="server" Visible="false">
                        <asp:TableCell>
                            <div id="divFiles">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdFilesList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                        AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" AutoPostBack="true"
                                        OnCheckedChanged="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:SlNoGrid %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.EDocRowNo) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.EDocRowNo)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DateGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.EDocDate, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DocNoGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocNo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnbDocNo" Text='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString()) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocNo).ToString())%>'
                                                        runat="server" CssClass="text-underline" OnClick="ActionHandler" CommandName="EDIT" />
                                                    <asp:HiddenField runat="server" ID="hdfEDocPk" Value='<%# Eval(Resources.DataFieldRes.EDocPk) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:TitleGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocFileTitle %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocFileTitle).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocFileTitle).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:FileDescriptionGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocFileDescription %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocFileDescription).ToString()),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocFileDescription).ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:AttachedDocsGrid %>" SortExpression="<%$ resources:DataFieldRes,EDocAttachedDocs %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttachedDocs" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocAttachedDocs).ToString()),40) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EDocAttachedDocs).ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,EDocStatus %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.EDocStatus) %>' />
                                                    <div id="imbStatusIndicator" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <a class="download-icon nomargin" href='<%# Page.ResolveUrl(GetPhysicalPath(Eval(Resources.DataFieldRes.EDocFilePath),Eval(Resources.DataFieldRes.EDocProject))) %>'
                                                        target="_blank" title="Download"></a>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPagingFiles" runat="server" />
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
