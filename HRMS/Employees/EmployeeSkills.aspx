<%@ Page Title="<%$ Resources:Title_EmployeeSkills %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmployeeSkills.aspx.cs" Inherits="HRMS.Employees.EmployeeSkills"
    Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%--Tab User control--%>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%--Ext Gridview control--%>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
        }
        function AfterGridExpand(row) {
            if ($("[id$=grdSkillCategoryList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsSkillCategory]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnSkillCategory]").click();
                }
            }
        }
        function ExpandSelected() {
            $("[id$=grdSkillCategoryList]").find("[id*=hdfSkillCategoryId]").each(function () {
                var hasChild = Number($(this).closest('tr').find("#[id*=hdfHasChild]").val());
                var colpsBtn = $(this).closest("tr").find("a.GridExpandCollapseButton");
                if (hasChild == 1) {
                    $(colpsBtn).closest("tr").next("tr").show();
                    $(colpsBtn).text("-");
                }
                //            else {
                //                $(colpsBtn).closest("tr").next("tr").hide();
                //                $(colpsBtn).text("+");
                //            }
            });
        }

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
            $("[id*=grdSkillCategoryList] tr td[cellIndex=0]").hide();
            $("[id*=grdSkillCategoryList] tr th[cellIndex=0]").hide();

        }

        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            currencyDecimal = 2;
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > currencyDecimal - 1)) {
                return false;
            }
            return true;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveAndContinue]").hide();              
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="5" />
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSaveAndContinue">
                                        <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                            TabIndex="63" Text="Save & Continue" ToolTip="Save & Continue" ValidationGroup="Employee"
                                            OnClientClick="javascript:return ValidatePageNow('Employee')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="63" Text="Save" ToolTip="Save" ValidationGroup="Employee" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" OnClientClick="javascript:return ValidatePageNow('Employee')" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="Cancel" ToolTip="Cancel"
                                            CommandName="CANCEL" TabIndex="64" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <ucBasicHdr:EmpBasicInfoControl ID="UCempBasicHdr" runat="server" />
                <%--<div class="detail-co3" runat="server" id="divEmployeeHeader">
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrEmployeeNo" runat="server" AssociatedControlID="lblhdrEmployeeNoTxt"
                            Text="Employee No:"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNoTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrEmployeeName" runat="server" AssociatedControlID="lblhdrEmployeeNameTxt"
                            Text="Employee Name:"></asp:Label>
                        <asp:Label ID="lblhdrEmployeeNameTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrDOJ" runat="server" AssociatedControlID="lblhdrDOJText" Text="DOJ:"></asp:Label>
                        <asp:Label ID="lblhdrDOJText" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrDOB" runat="server" AssociatedControlID="lblhdrDOBTxt" Text="DOB:"></asp:Label>
                        <asp:Label ID="lblhdrDOBTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="div3col-S">
                        <asp:Label ID="lblhdrDesignation" runat="server" AssociatedControlID="lblhdrDesignationTxt"
                            Text="Designation:"></asp:Label>
                        <asp:Label ID="lblhdrDesignationTxt" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblhdrDepartment" runat="server" AssociatedControlID="lblhdrDepartmentTxt"
                            Text="Department:"></asp:Label>
                        <asp:Label ID="lblhdrDepartmentTxt" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                </div>--%>
                <div class="tab-container-floating txtAlign-right">
                    <ul>
                        <li>
                            <asp:CheckBox runat="server" ID="chkShowAllSkill" TabIndex="1" AutoPostBack="true"
                                OnCheckedChanged="ActionHandler" />
                            <asp:Label ID="lblShowAllSkill" runat="server" Text="<%$resources:ShowAllSkills %>"></asp:Label>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks tablelayout">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap hierarchical-wrap max-425">
                                <%--<div class="gridwrap hierarchical-wrap max-380">--%>
                                <cc1:ExtGridView runat="server" ID="grdSkillCategoryList" AutoGenerateColumns="False"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" Width="100%" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnSkillCategory" OnClick="ActionHandler" CommandName="CATEGORYLIST"
                                                    CommandArgument='<%# Eval("CON_PK")%>' EnableTheming="false" Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsSkillCategory" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfSkillCategoryId" Value='<%# Eval("CON_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfHasChild" Value='<%# Eval("ESD_HAS_ENTRY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:GrdHrSkillCategory%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CON_NAME"))) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CON_NAME"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="99%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--'<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("CID_UOM_TEXT"),10) %>'
                                                '<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("CID_UOM_TEXT"))) %>'--%>
                                                <div class="hierarchical-gridwrap">
                                                    <asp:GridView runat="server" ID="grdSkillDetails" AutoGenerateColumns="False" GridLines="None"
                                                        EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" OnRowDataBound="ActionHandler"
                                                        Width="100%">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblInnerEmptyGrid" runat="server" Text="<%$ resources:Msg_EmptySkillGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkSkill" TabIndex="1" Checked='<%#Convert.ToInt32(Eval("ESD_SKILL_FLAG")) == 0 ? false:true %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfPk" Value='<%# Eval("ESD_PK") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfSkillCategoryPk" Value='<%# Eval("ESD_SKILL_CATEGORY") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfSkillPk" Value='<%# Eval("ESD_SKILL") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$resources:GrdHrArea%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSkillArea" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESD_SKILL_TEXT"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30%" VerticalAlign="Top" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$resources:GrdHrYears%>">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSkillYear" TabIndex="1" runat="server" CssClass="small" onkeypress="return validateFloatKeyPress(this,event);"
                                                                        Text='<%# Eval("ESD_EXP_YEAR")%>'>
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="14%" VerticalAlign="Top" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$resources:GrdHrExpertLevel%>">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="hdfSkillLevel" Value='<%# Eval("ESD_EXPERT_LEVEL") %>' />
                                                                    <asp:DropDownList runat="server" TabIndex="1" ID="ddlSkillLevel" CssClass="medium">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" VerticalAlign="Top" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$resources:GrdHrRating%>">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSkilRating" TabIndex="1" runat="server" CssClass="medium" MaxLength="15"
                                                                        Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESD_RATING"))) %>'
                                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESD_RATING"))) %>'>
                                                                    </asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" VerticalAlign="Top" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$resources:GrdHrRemarks%>">
                                                                <ItemTemplate>
                                                                    <div class="div2col-S">
                                                                        <asp:TextBox ID="txtSkilRemarks" TabIndex="1" runat="server" CssClass="select-full-a" MaxLength="200"
                                                                            Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESD_REMARKS"))) %>'
                                                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ESD_REMARKS"))) %>'>
                                                                        </asp:TextBox>
                                                                        <div class="clear">
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30%" VerticalAlign="Top" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                            <span style="float: right !important;" id="lblLastModifiedDate" runat="server"></span>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                </div>
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfNumberDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                <asp:HiddenField ID="hdfExchangeDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
                <asp:HiddenField ID="hdfLastModDate" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
