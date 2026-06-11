<%@ Page Title="<%$ Resources:Captions,Title_MaterialCategoryMaster %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" EnableEventValidation="false"
    Theme="ClassicExt" CodeBehind="MaterialCategoryMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.MaterialCategoryMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/MaterialManagement/MaterialCategoryMaster.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MaterialCategoryMaster%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="7" EnableViewState="False"
                OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton runat="server" ID="imbDelete" SkinID="btndelete" TabIndex="8" EnableViewState="False"
                OnClientClick="javascript:return DeletePage();" />
            <asp:ImageButton runat="server" ID="imbResetall" SkinID="btnreset" TabIndex="9" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                EnableViewState="False" TabIndex="10" />
        </div>
    </div>--%>
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button ID="btnDeletePage" runat="server" SkinID="btnInner-Delete" Text="<%$Resources:Controls,Delete%>"
                                    EnableViewState="False" OnClientClick="javascript:return DeletePage();" />
                            </li>
                            <%--<li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" PostBackUrl="~/AccountManagement/WorkflowInbox.aspx" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <table class="table-devide">
            <tr>
                <td>
                    <div id="divMaterialCategory" title="<%=Resources.Captions.MaterialCategoryList%>">
                        <div id="treewrap" class="edittree">
                            <div id="trvCategory" class="treeview-adj">
                            </div>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="divcolmiddle-S">
                        <label for="CategoryCode" class="lbl-30perc">
                            <%=Resources.Controls.MaterialCategoryCode %>*</label>
                        <asp:TextBox ID="CategoryCode" CssClass="input-half-03-12" TabIndex="2" runat="server"
                            EnableViewState="False">
                        </asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="CategoryName" class="lbl-30perc">
                            <%=Resources.Controls.MaterialCategoryName %>*</label>
                        <asp:TextBox ID="CategoryName" CssClass="input-half-03-12" TabIndex="3" runat="server"
                            EnableViewState="False">
                        </asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="CategoryDesc" class="lbl-30perc">
                            <%=Resources.Controls.MaterialCategoryDesc %></label>
                        <asp:TextBox ID="CategoryDesc" CssClass="input-half-03-12" TabIndex="4" runat="server"
                            EnableViewState="False">
                        </asp:TextBox>
                        <div class="clear">
                        </div>
                        <div class="w60perc disp-inline" style="display: none">
                            <label for="UOMType" class="lbl-30perc">
                                <%=Resources.Controls.UOMType%>*</label>
                            <asp:DropDownList ID="UOMType" CssClass="select-small-a1" TabIndex="5" runat="server"
                                EnableViewState="False" Enabled="false">
                            </asp:DropDownList>
                        </div>
                        <%--Inactive Period--%>
                        <label for="InactivePeriod" class="lbl-30perc">
                            <%=Resources.Controls.InactivePeriod%></label>
                        <asp:TextBox ID="InactivePeriod" CssClass="w10perc numeric" TabIndex="5" runat="server"
                            MaxLength="5" EnableViewState="False">
                        </asp:TextBox>
                        <div class="clear">
                        </div>
                        <%--Inactive Period End--%>
                        <label for="CategoryType" class="lbl-30perc">
                            <%=Resources.Controls.CategoryType%>*</label>
                        <asp:DropDownList ID="CategoryType" TabIndex="5" runat="server" EnableViewState="False"
                            CssClass="select-half-a">
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                        <div id="divSinglePlantAccounts" style="margin-left: -4px; margin-right: 5px;">
                            <label for="PurAccount" class="lbl-30perc">
                                <%=Resources.Controls.PurAccount%></label>
                            <asp:DropDownList ID="PurAccount" TabIndex="6" runat="server" EnableViewState="False"
                                CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <label for="InvAccount" class="lbl-30perc">
                                <%=Resources.Controls.InvAccount%></label>
                            <asp:DropDownList ID="InvAccount" TabIndex="7" runat="server" EnableViewState="False"
                                CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <label for="SaleAccount" class="lbl-30perc">
                                <%=Resources.Controls.SaleAccount%></label>
                            <asp:DropDownList ID="SaleAccount" TabIndex="7" runat="server" EnableViewState="False"
                                CssClass="select-half-a">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <label for="ConsumptionAccount" class="lbl-30perc">
                                <%=Resources.Controls.ConsumptionAccount%></label>
                            <asp:DropDownList ID="ConsumptionAccount" TabIndex="7" runat="server" EnableViewState="False"
                                CssClass="select-half-a">
                            </asp:DropDownList>
                        </div>
                        <div class="clear">
                        </div>
                        <label for="Category" class="lbl-30perc">
                            <%=Resources.Controls.POCategory_Req%></label>
                        <asp:DropDownList runat="server" ID="PoCategory" TabIndex="15" CssClass="select-half-a">
                        </asp:DropDownList>
                        <label for="GrnType" class="lbl-30perc">
                            <%=Resources.Controls.GrnType%></label>
                        <asp:DropDownList runat="server" ID="GrnType" TabIndex="15" CssClass="select-half-a">
                            <asp:ListItem Value="10" Text="text1" />
                            <asp:ListItem Value="30" Text="text2" />
                        </asp:DropDownList>
                        <label for="ReportCategory" class="lbl-30perc">
                            <%=Resources.Controls.ReportCategory%></label>
                        <asp:DropDownList runat="server" ID="ReportCategory" TabIndex="15" CssClass="select-half-a">
                        </asp:DropDownList>
                        <label for="Parent" class="lbl-30perc">
                            <%=Resources.Controls.Parent%></label>
                        <asp:Label ID="Parent" runat="server" EnableViewState="False" CssClass="input-half-03-12"></asp:Label>
                        <%--  <label for="RequireInspection">
                            <%=Resources.Controls.RequireInspection%></label>
                        <asp:CheckBox ID="RequireInspection" runat="server" EnableViewState="False" ></asp:CheckBox>--%>
                        <div class="clear">
                        </div>
                        <label for="Stock" class="lbl-30perc">
                            <%=Resources.Controls.Stock%></label>
                        <asp:CheckBox ID="Stock" runat="server" EnableViewState="False"></asp:CheckBox>
                        <div id="divSaleItem" class="w60perc disp-inline">
                            <label for="SaleItem" class="lbl-18perc margn-rgt4">
                                <%=Resources.Controls.SaleItem%></label>
                            <asp:CheckBox ID="ITC_IS_SALE" runat="server" EnableViewState="False"></asp:CheckBox>
                        </div>
                        <div id="divMIJ" class="w60perc disp-inline">
                            <label for="IsforMIJ" class="lbl-25-1perc  margn-rgt4">
                                <%=Resources.Controls.isforMIJ%></label>
                            <asp:CheckBox ID="ITC_IS_VCH_POST" runat="server" EnableViewState="False"></asp:CheckBox>
                        </div>
                        <div class="clear">
                        </div>
                        <label class="lbl-18perc margn-rgt4">
                        </label>
                        <asp:LinkButton runat="server" ID="lnkMapAccounts" Text="Account Mapping" OnClientClick="javascript:return ShowMultiplePlantAccountPopUp();return false;"
                            Style="text-decoration: underline; font-weight: bold; padding-left: 12%;" EnableViewState="False">
                        </asp:LinkButton>
                        <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                        <asp:HiddenField ID="MaterialCategoryParentPK" runat="server" Value="0"></asp:HiddenField>
                        <asp:HiddenField ID="SBU" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfShowSaleItem" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfCategoryACBySBU" runat="server" Value="0" />
                    </div>
                </td>
            </tr>
        </table>
        <div id="divMultiplePlantAccounts" style="display: none" title="Account Mapping">
            <div class="content-wrapper">
                <div class="button-wrap-right">
                    <asp:Button ID="btnApplyBatches" runat="server" SkinID="btnInner-add-dsd" TabIndex="66"
                        Text="Apply" EnableViewState="False" OnClientClick="javascript:return ApplyAccountMapping();" />
                </div>
                <table class="table-devide">
                    <tr>
                        <td class="txt-lft">
                          <div id="divSBUPopup">
                                <label for="ddlSBUPopup" class="middle-lbl-xsmall-a4">
                                    <%=Resources.Controls.SBU%></label>
                                <asp:DropDownList ID="ddlSBUPopup" TabIndex="60" runat="server" Width="100px" onchange="javascript:return SBUChange();">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-lft">
                            <div id="divPlantPopup">
                                <label for="ddlPlantPopup" class="middle-lbl-xsmall-a4">
                                    <%=Resources.Controls.Plant%></label>
                                <asp:DropDownList ID="ddlPlantPopup" TabIndex="60" runat="server" Width="100px">
                                </asp:DropDownList>
                            </div>
                          
                            <label for="AccountType" class="middle-lbl-xsmall-a4">
                                <%=Resources.Controls.AccountType%></label>
                            <asp:DropDownList ID="AccountType" TabIndex="61" runat="server" EnableViewState="False"
                                Width="150px" onchange="javascript:return AccountTypeChange();">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt-lft">
                            <label for="AccountPopUp" class="middle-lbl-xsmall-a4">
                                <%=Resources.Controls.Account%></label>
                            <%--<asp:DropDownList ID="AccountPopUp" TabIndex="62" runat="server" EnableViewState="False"
                                Width="365px">
                            </asp:DropDownList>--%>
                            
                            <asp:TextBox ID="txtAccount" runat="server" CssClass="h14" Width="365px" 
                                MaxLength="100" TabIndex="4"> </asp:TextBox>
                            <asp:HiddenField ID="hdfAccPK" runat="server" Value="0" />
                            <asp:ImageButton ID="imbAddAccounts" runat="server" SkinID="imbaddnew" CssClass="margntop3"
                                TabIndex="64" ToolTip="<%$resources:ErpRes,AddNewMaterials %>" OnClientClick="javascript:return AddMappedAccountsPopup();"
                                EnableViewState="false" />
                        </td>
                    </tr>
                </table>
                <div id="divAccountMap" class="scroll-container">
                    <table rules="all" id="grdMappedAcclist" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                        editable="true" class="gridwraptable gridwrap tablefixwidth-td">
                        <thead>
                            <tr>
                                <th fieldmap="ICC_PK" isvisible="false">
                                </th>
                                <th fieldmap="ICC_COMPANY" isvisible="false">
                                </th>
                                 <th fieldmap="ICC_SBU" isvisible="false">
                                </th>
                                <th fieldmap="ICC_ACCOUNT" isvisible="false">
                                </th>
                                <th fieldmap="ICC_ACCOUNT_TYPE" isvisible="false">
                                </th>
                                <th runat="server" id="thCompany" fieldmap="ICC_COMPANY_TEXT" align="left" width="20%">
                                    <%=Resources.Controls.Plant%>
                                </th>
                                 <th runat="server" id="thSBU" fieldmap="ICC_SBU_TEXT" align="left" width="20%">
                                    <%=Resources.Controls.SBU%>
                                </th>
                                <th fieldmap="ICC_ACCOUNT_TYPE_TEXT" align="left" width="25%">
                                    <%=Resources.Controls.AccountType%>
                                </th>
                                <th fieldmap="ICC_ACCOUNT_TEXT" align="left" width="50%">
                                    <%=Resources.Controls.Account%>
                                </th>
                                <th type="Template" width="5%">
                                    <div>
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETEACC')"
                                            TabIndex="65" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="MaterialAccountlst" runat="server" Value="" />
        <div id="divMappingData">
        </div>
        <div id="divSaveData">
        </div>
        <div id="grdTable-wrap">
            <%--  <div class="div2col-S">
                <div id="edittree" class="edittree">
                    <h3>
                        <%=Resources.Captions.MaterialCategoryList%>
                    </h3>
                    <div id="trvCategory" class="treeview-adj">
                    </div>
                </div>
            </div>--%>
            <%-- 
  <div class="treeview-center">
                <div class="treeview max-380">
                    <h3>
                        <%=Resources.Captions.MaterialCategoryList%>
                    </h3>
                    <div class="clear">
                    </div>
                    <div id="trvCategory" >
                    </div>
                </div>
            </div>
            --%>
        </div>
    </div>
</asp:Content>
