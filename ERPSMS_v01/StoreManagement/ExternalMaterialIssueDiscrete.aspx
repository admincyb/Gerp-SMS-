<%@ Page Title="<%$ Resources:Captions,Title_ExternalMaterialIssue %>"
 Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" EnableEventValidation="false" Theme="Classic"
 CodeBehind="ExternalMaterialIssueDiscrete.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.ExternalMaterialIssueDiscrete" %>
<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/ExternalMaterialIssue.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.ExternalMaterialIssue%></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage('Draft');"
                EnableViewState="false" TabIndex="15" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="false" TabIndex="16" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                EnableViewState="false" TabIndex="17" />
            <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPagePrint" OnClientClick="javascript:return PrintPage();"
                EnableViewState="false" TabIndex="18" />
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
                            <%-- <li>
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>--%>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="34" EnableViewState="False" ToolTip="<%$Resources:Controls,Save %>"
                                    OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%--  <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" ToolTip="<%$Resources:Controls,Reset %>" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" ToolTip="<%$Resources:Controls,Cancel %>" OnClientClick="javascript:return CancelFun();"
                                    TabIndex="36" />
                            </li>
                            <%-- <li>
                                <asp:Button ID="btnPrint" runat="server" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    EnableViewState="False" OnClientClick="javascript:return PrintPage();" TabIndex="36" />
                            </li>--%>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="DefaultMaterialMaster">
                <asp:HiddenField ID="MaterialDetailId" Value="0" runat="server" />
                <asp:HiddenField ID="ICH_TRX_TYPE" Value="1" runat="server" />
                <asp:HiddenField ID="ITM_CODE" runat="server" />
                <asp:HiddenField ID="ITM_MIN_STK" runat="server" Value="0" />
                <asp:HiddenField ID="ITM_ROL_STK" runat="server" Value="0" />
                <asp:HiddenField ID="ITM_MAX_STK" runat="server" Value="0" />
                <asp:HiddenField ID="ICH_STATUS" runat="server" Value="1" />
                <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
                <asp:HiddenField ID="hdfDeptID" runat="server" EnableViewState="false" Value="0" />
                <%--<asp:HiddenField ID="ICH_DEPT" runat="server" Value="0" />--%>
                <%-- <asp:HiddenField ID="ICH_PK"  runat="server" Value="0" />--%>
            </div>
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=Resources.Controls.MaterialIssueNo%></label>
                                <asp:Label ID="lblMaterilaConsumptionNo" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="APT_CODE" runat="server" />
                                <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                                <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                                <label for="IssuingType">
                                    <%=Resources.Controls.IssueAgainst%>*</label>
                                <asp:DropDownList ID="ICH_ISS_RCV_TYPE" runat="server" TabIndex="2" EnableViewState="False"
                                    onchange="javascript:FillIssueToCategories();">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdn_ICH_ISS_RCV_TYPE" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                                <label for="Store">
                                    <%=Resources.Controls.IssuingStore%>*</label>
                                <asp:DropDownList ID="ICH_DEPT" runat="server" TabIndex="4" onchange="javascript:FillNewCategories();"
                                    EnableViewState="False">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="ICH_REF_NO">
                                    <%=Resources.Controls.ReferenceNo%></label>
                                <asp:TextBox ID="ICH_REF_NO" runat="server" onkeypress="return this.value.length<25"
                                    onpaste="return this.value.length<25" MaxLength="25" TabIndex="6"></asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <%-- <label for="lblBlank">
                    </label>
                    <asp:Label ID="lblBlank" runat="server"></asp:Label>
                    <div class="clear">
                    </div>--%>
                                <label>
                                    <%=Resources.Controls.Date%>*</label>
                                <asp:TextBox ID="ICH_DATE" runat="server" onkeydown="return CheckKey(event)" onpaste="return false;"
                                    TabIndex="1"></asp:TextBox>
                                <asp:HiddenField ID="ICH_NO" runat="server" />
                                <div class="clear">
                                </div>
                                <label for="IssueTo">
                                    <%=Resources.Controls.IssueTo%>*</label>
                                <%--  <asp:DropDownList ID="ICH_ISS_RCV_PK" runat="server" TabIndex="3" EnableViewState="False">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtIssueTo" runat="server" TabIndex="3">
                                </asp:TextBox>
                                <asp:HiddenField ID="ICH_ISS_RCV_PK" runat="server" Value="0"></asp:HiddenField>
                                <%-- <div class="clear">
                                </div>--%>
                                <label for="ICH_ITEM_TYPE">
                                    <%=Resources.Controls.IssuingMaterialType%>*</label>
                                <asp:DropDownList ID="ICH_ITEM_TYPE" runat="server" TabIndex="5" EnableViewState="False"
                                    onchange="javascript:FillNewCategories();">
                                </asp:DropDownList>
                                <asp:HiddenField ID="ICH_ISS_RCV_NAME" runat="server" Value="" />
                                <label for="ICH_COMPANY">
                                    <%=Resources.Controls.Company%>*</label>
                                <asp:DropDownList ID="ICH_COMPANY" runat="server" TabIndex="7" ClientIDMode="Static">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdfSelCompany" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <%-- <label for="Departement">
                        <%=Resources.Controls.RequestingStore%>*</label>
                    <asp:DropDownList ID="DeptPk" runat="server" TabIndex="2" Width="200px" EnableViewState="False">
                    </asp:DropDownList>
                    <div class="clear"></div>--%>
                </table>
                <asp:HiddenField runat="server" ID="ICH_PK" />
                <asp:HiddenField runat="server" ID="ConsumptionDtl" />
            </div>
            <div id="Order" class="gridwrap">
                <div id="ProductInsert">
                    <table id="tblProductInsert" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th width="17%" align="left">
                                    <%=Resources.Controls.MaterialType%>*
                                </th>
                                <th width="17%" align="left">
                                    <b>
                                        <%=Resources.Controls.MaterialCode%>* </b>
                                </th>
                                <th width="17%" align="left">
                                    <b>
                                        <%=Resources.Controls.BatchNo%> </b>
                                </th>
                                <th width="7%" align="left">
                                    <b>
                                        <%=Resources.Controls.Stock%>* </b>
                                </th>
                                <th width="10%">
                                    <b>
                                        <%=Resources.Controls.QtyIssued%>*</b>
                                </th>
                                <th width="6%" align="left">
                                    <b>
                                        <%=Resources.Controls.UOM%>*</b>
                                </th>
                                <th width="6%" align="center">
                                    <b>
                                        <%=Resources.Controls.Return%>
                                    </b>
                                </th>
                                <th width="15%" align="left">
                                    <b>
                                        <%=Resources.Controls.Comments%>
                                    </b>
                                </th>
                                <th width="5%" align="left">
                                    <b>
                                        <%=Resources.Controls.Action%></b>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="grd-rowhead">
                                <td width="20%">
                                    <%--<asp:DropDownList ID="MaterialType" Width="85%" runat="server" TabIndex="7" onchange="javascript:FillCategoryDetails($(this).val());">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imbViewCag" runat="server" ToolTip="Open Item Category"
                                        SkinID="btnview"  TabIndex="0" OnClientClick="javascript:return ShowCategory();"
                                        EnableViewState="false" />--%>
                                    <asp:TextBox ID="MaterialCategory" runat="server" Width="85%" TabIndex="8">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td width="25%">
                                    <%-- <asp:DropDownList ID="ITV_ITEM" runat="server" TabIndex="8" Width="98%" onchange="javascript:GetCurrentStock($(this).val());">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="Material" runat="server" Width="90%" TabIndex="9" AutoPostBack="false">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                </td>
                                <td width="25%">
                                    <asp:TextBox ID="BatchNo" runat="server" Width="90%" TabIndex="9">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="BatchNoPK" runat="server" Value="0"></asp:HiddenField>
                                    <%--<asp:HiddenField ID="IsEdit" runat="server" Value="false" />--%>
                                </td>
                                <td style="text-align: right" width="8%">
                                    <asp:Label ID="CurrentStock" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right" width="10%">
                                    <asp:TextBox ID="ICD_QTY_CONSUMED" runat="server" TabIndex="9" EnableViewState="false"
                                        Text="" CssClass="numeric" MaxLength="12" Width="90px">
                                    </asp:TextBox>
                                </td>
                                <td width="6%">
                                    <asp:DropDownList ID="ICD_UOM" runat="server" TabIndex="10" Width="80px" EnableViewState="false">
                                    </asp:DropDownList>
                                </td>
                                <td align="center" width="6%">
                                    <asp:CheckBox ID="ICD_ISRETURN" runat="server" MaxLength="250" TabIndex="11" EnableViewState="false">
                                    </asp:CheckBox>
                                </td>
                                <td width="15%">
                                    <asp:TextBox ID="ICD_REMARKS" runat="server" MaxLength="250" TabIndex="12" EnableViewState="false"
                                        Width="125px">
                                    </asp:TextBox>
                                </td>
                                <td align="left" width="10%">
                                    <asp:ImageButton ID="imbAddNew" TabIndex="13" runat="server" SkinID="imbaddnew" OnClientClick="javascript:return AddRequisitionDetails();"
                                        Width="16px" />
                                    <asp:HiddenField ID="hdpk" runat="server" />
                                    <asp:HiddenField ID="hdnIsNeededStockValidation" runat="server" Value="0" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="gridwrap">
            <table rules="all" id="grdRequisitionSlip" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                ajaxurl="" editable="true" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="ICD_PK" isvisible="false">
                        </th>
                        <th fieldmap="ICD_ITEM" isvisible="false">
                        </th>
                        <th fieldmap="ICD_SL_NO" isvisible="false">
                        </th>
                        <th fieldmap="ICD_STK_BATCH" isvisible="false">
                        </th>
                        <th fieldmap="ICD_UOM" isvisible="false">
                        </th>
                        <th fieldmap="ICD_ITEM_CATEGORY" isvisible="false">
                            <%--MaterialTypePk--%>
                        </th>
                        <th fieldmap="ICD_ITEM_TYPE" isvisible="false">
                        </th>
                        <th fieldmap="ICD_ITEM_CATEGORY_TEXT" align="left" width="20%">
                            <%--MaterialType --%>
                            <%=Resources.Controls.MaterialType%>*
                        </th>
                        <th fieldmap="ICD_ITEM_TEXT" align="left" width="10%">
                            <%--MaterialCode--%>
                            <%=Resources.Controls.MaterialCode%>*
                        </th>
                        <th fieldmap="ICD_STK_BATCH_NO" align="left" width="10%">
                            <%--Batch No--%>
                            <%=Resources.Controls.BatchNo%>*
                        </th>
                        <th fieldmap="ICD_CURRENT_STK" align="right" width="15%">
                            <%--CurrentStock--%>
                            <%=Resources.Controls.Stock%>*
                        </th>
                        <th fieldmap="ICD_QTY_CONSUMED" align="right" width="10%">
                            <%=Resources.Controls.QtyIssued%>*
                        </th>
                        <th fieldmap="ICD_UOM_TEXT" align="left" width="5%">
                            <%=Resources.Controls.UOM%>*
                        </th>
                        <th fieldmap="ICD_ISRETURNTEXT" align="center" width="15%">
                            <%=Resources.Controls.IsReturn%>
                        </th>
                        <th fieldmap="ICD_REMARKS" align="left" width="15%">
                            <%=Resources.Controls.Comments%>
                        </th>
                        <th type="Template" width="10%" align="left">
                            <div>
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                    SkinID="imbeditgrid" Width="15px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                    SkinID="imbdeletegrid" Width="15px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <%--#####END Add user Controls and process Id, app ID#####--%>
        <asp:HiddenField runat="server" ID="EditRequisition" Value="0" />
        <asp:HiddenField runat="server" ID="hdfSlNo" Value="0" />
        <asp:HiddenField runat="server" ID="hdnMaterialPk" Value="0" />
        <asp:HiddenField runat="server" ID="ICD_PK" Value="0" />
        <asp:HiddenField runat="server" ID="ActionID" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="LAST_MOD_DT" runat="server" />
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="0" />
        <!-- Stores the Order Json Object Jquery Data Store -->
        <div id="divRequisitionData">
        </div>
        <div id="divCategory" title="<%=Resources.Captions.SelectCategory%>">
            <div id="treewrap" class="edittree">
                <div id="trvCategory" class="treeview-adj">
                </div>
            </div>
        </div>
        <div class="divcol-actiowrap" id="divAction" runat="server">
            <asp:Button ID="btnSaveandSubmit" TabIndex="14" runat="server" CssClass="inputbtn"
                Text="<%$Resources:Controls,Saveandsubmit%>" ToolTip="<%$Resources:Controls,Saveandsubmit%>"
                Style="width: 140px" OnClientClick="javascript:return SavePage();" />
        </div>
    </div>
</asp:Content>
