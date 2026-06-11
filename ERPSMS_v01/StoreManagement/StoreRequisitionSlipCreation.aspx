<%@ Page Title="<%$ Resources:Captions,Title_StoreRequisitionSlip %>" Language="C#"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="StoreRequisitionSlipCreation.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.StoreRequisitionSlipCreation" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StoreRequisitionSlipCreation.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.StoreRequisitionSlip%></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage('Draft');"
                EnableViewState="false" TabIndex="11" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="false" TabIndex="12" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                EnableViewState="false" TabIndex="13" />
            <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPagePrint" OnClientClick="javascript:return PrintPage();"
                EnableViewState="false" TabIndex="14" />
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
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="36" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnClose" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelPage();" TabIndex="37" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnPrint" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    TabIndex="38" EnableViewState="False" OnClientClick="javascript:return PrintPage();" /></li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
    </div>
    <div class="content-wrapper">
        <div id="DefaultMaterialMaster">
            <asp:HiddenField ID="MaterialDetailId" Value="0" runat="server" />
            <asp:HiddenField ID="ITM_CODE" runat="server" />
            <asp:HiddenField ID="ITM_MIN_STK" runat="server" Value="0" />
            <asp:HiddenField ID="ITM_ROL_STK" runat="server" Value="0" />
            <asp:HiddenField ID="ITM_MAX_STK" runat="server" Value="0" />
            <asp:HiddenField ID="STATUS" runat="server" Value="2" />
            <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDeptID" runat="server" EnableViewState="false" Value="0" />
            <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrentDate" runat="server" />
            <asp:HiddenField ID="MRH_IS_EDIT" runat="server" Value="0" />
        </div>
        <div id="divData">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label>
                                <%=Resources.Controls.SR_NO%></label>
                            <asp:Label ID="lblSRS" runat="server" Text="" Width="220px" CssClass="input-small"></asp:Label>
                            <asp:HiddenField ID="APT_CODE" runat="server" />
                            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                            <label class="middle-lbl-small-c">
                                <%=Resources.Controls.MRDate%>*</label>
                            <asp:TextBox ID="MRH_SUBMITTED_DATE" runat="server" TabIndex="2" Width="220px" CssClass="input-small"></asp:TextBox>
                            <asp:HiddenField ID="MRH_NO" runat="server" />
                            <div class="clear">
                            </div>
                            <div id="DivSbu">
                            <label for="Store" class="store-label">
                                <%=Resources.Controls.SBU%>*</label>
                            <asp:DropDownList ID="MRH_TO_BIZUNIT" runat="server" ClientIDMode="Static" TabIndex="3" Width="200px" onchange="javascript:FillStore();"
                                CssClass="select-half-a" EnableViewState="False">
                            </asp:DropDownList>
                                  <asp:HiddenField ID="MRH_TO_BIZUNIT_VAL" runat="server" />
                          </div>
                            <label for="Store">
                                <%=Resources.Controls.IssueStore%>*</label>
                            <asp:DropDownList ID="MRH_DEPT_STR" runat="server" TabIndex="3" Width="200px" onchange="javascript:FillDepartement();"
                                CssClass="select-half-a" EnableViewState="False">
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <label for="Departement">
                                <%=Resources.Controls.RequestingStore%>*</label>
                            <asp:DropDownList ID="DeptPk" runat="server" TabIndex="3" Width="200px" EnableViewState="False"
                                CssClass="select-half">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>

                            <div id="divSBUCompany">
                                <label for="MRH_COMPANY" class="margnrgt3">
                                    <%=GetGlobalResourceObject("Controls","CompanyPlant")%>*</label>
                                <asp:DropDownList ID="MRH_COMPANY" runat="server" TabIndex="3" ClientIDMode="Static"
                                    Width="227px">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdfSelCompany" runat="server" />
                            </div>
                            <div id="divItemType"  style="display: none;">
                            <label for="ITM_TYPE_TEXT">
                                <%=Resources.Controls.Type%>*</label>
                            <asp:DropDownList ID="ITM_TYPE_TEXT" runat="server" TabIndex="4" Width="200px" EnableViewState="False"
                                CssClass="select-half" onchange="javascript:FillNewCategories();">
                            </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="MRH_PK" />
            <asp:HiddenField runat="server" ID="RequisitionDetailsList" />
            <div id="Order" class="grdTable">
                <div id="ProductInsert">
                    <table id="tblProductInsert" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th width="16%" align="left">
                                    <%=Resources.Controls.MaterialType%>*
                                </th>
                                <th width="33%" align="left">
                                    <b>
                                        <%=Resources.Controls.MaterialCode%>* </b>
                                </th>
                                <th width="10%" align="left">
                                    <b>
                                        <%=Resources.Controls.CurrentStock%>* </b>
                                </th>
                                <th width="10%">
                                    <b>
                                        <%=Resources.Controls.QtyRequest%></b>*
                                </th>
                                <th width="5%" align="left">
                                    <b>
                                        <%=Resources.Controls.UOM%></b>*
                                </th>
                                <th width="21%" align="left">
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
                                <td width="16%">
                                    <%--  <asp:DropDownList ID="MaterialType" runat="server" Width="85%" TabIndex="3"
                                        onchange="javascript:FillCategoryDetails($(this).val());">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="MaterialCategory" runat="server" Width="90%" TabIndex="5">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                                    <%--            <asp:ImageButton ID="imbViewCag" runat="server" CssClass="imgbutton-wrap" ToolTip="Open  Material Category"
                                        SkinID="btnview" Width="22px" Height="22px" TabIndex="4" OnClientClick="javascript:return ShowCategory();"
                                        EnableViewState="false" />--%>
                                </td>
                                <td width="33%">
                                    <%--  <asp:DropDownList ID="ITV_ITEM" runat="server" TabIndex="5" Width="98%" onchange="javascript:GetCurrentStock($(this).val());">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="Material" runat="server" Width="96%" TabIndex="6">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                </td>
                                <td style="text-align: right" width="10%">
                                    <asp:Label ID="CurrentStock" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right" width="10%">
                                    <asp:TextBox ID="MRD_QTY_REQUESTED" runat="server" TabIndex="7" EnableViewState="false"
                                        Text="" CssClass="numeric" Width="80px">
                                    </asp:TextBox>
                                </td>
                                <td width="5%">
                                    <asp:DropDownList ID="MRD_UOM" runat="server" TabIndex="8" Width="80px" EnableViewState="false">
                                    </asp:DropDownList>
                                </td>
                                <td width="21%">
                                    <asp:TextBox ID="MRD_REMARKS" runat="server" TabIndex="9" EnableViewState="false"
                                        Width="98%">
                                    </asp:TextBox>
                                </td>
                                <td align="left" width="5%">
                                    <asp:ImageButton ID="imbAddNew" TabIndex="10" runat="server" SkinID="imbaddnew" OnClientClick="javascript:return AddRequisitionDetails();"
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
        <div class="grdTable">
            <table rules="all" id="grdRequisitionSlip" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                ajaxurl="" editable="true" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="MRD_PK" isvisible="false">
                        </th>
                        <%-- <th fieldmap="MRD_SL_NO" isvisible="false">
                        </th>--%>
                        <th fieldmap="MRD_ITEM" isvisible="false">
                        </th>
                        <th fieldmap="MRD_UOM" isvisible="false">
                        </th>
                        <th fieldmap="MaterialTypePk" isvisible="false">
                        </th>
                        <th fieldmap="MaterialType" align="left" width="16%">
                            <%=Resources.Controls.MaterialType%>*
                        </th>
                        <th fieldmap="MaterialCode" align="left" width="33%">
                            <%=Resources.Controls.MaterialCode%>*
                        </th>
                        <th fieldmap="CurrentStock" align="right" width="10%">
                            <%=Resources.Controls.CurrentStock%>*
                        </th>
                        <th fieldmap="MRD_QTY_REQUESTED" align="right" width="10%">
                            <%=Resources.Controls.QtyRequest%>*
                        </th>
                        <th fieldmap="UOM" align="left" width="5%">
                            <%=Resources.Controls.UOM%>*
                        </th>
                        <th fieldmap="MRD_REMARKS" align="left" width="21%">
                            <%=Resources.Controls.Comments%>
                        </th>
                        <th type="Template" width="5%" align="left">
                            <div>
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$Resources:Controls,Edit %>"
                                    Width="15px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" Width="15px"
                                    ToolTip="<%$Resources:Controls,Delete %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
        <div id="Wofkflowdiv" style="display: none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSlNo" runat="server" Value="0" />
            <asp:HiddenField ID="AppNo" runat="server" />
            <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
             <asp:HiddenField ID="hdfType" runat="server" Value="0" />
        </div>
        <%--#####END Add user Controls and process Id, app ID#####--%>
        <asp:HiddenField runat="server" ID="EditRequisition" Value="0" />
        <asp:HiddenField runat="server" ID="MRD_PK" Value="0" />
        <asp:HiddenField runat="server" ID="hdnMaterialPk" Value="0" />
        <asp:HiddenField runat="server" ID="ActionID" />
        <asp:HiddenField runat="server" ID="hdfItemTypes" Value="0" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="hdfCompoundStore" runat="server" Value="0" />
        <asp:HiddenField ID="hdfInvStore" runat="server" Value="0" />
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
        <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
        <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
         <asp:HiddenField ID="MRH_IS_RETURNABLE" runat="server" Value="0" />
         <asp:HiddenField ID="MRD_IS_RETURNABLE" runat="server" Value="0" />
         <asp:HiddenField ID="MRH_IS_BZU_TRN" runat="server" Value="0" />
        <asp:HiddenField ID="hdfMaterialWithStock" runat="server" />
        <!-- Stores the Order Json Object Jquery Data Store -->
        <div id="divRequisitionData">
        </div>
    </div>
    <div id="divCategory" title="<%=Resources.Captions.SelectCategory%>">
        <div id="treewrap" class="edittree">
            <div id="trvCategory" class="treeview-adj">
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnModifyMR" runat="server" Value="0" />
</asp:Content>
