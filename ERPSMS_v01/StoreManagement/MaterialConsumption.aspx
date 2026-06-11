<%@ Page Title="<%$ Resources:Captions,Title_MaterialConsumption %>" Language="C#"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic"
    AutoEventWireup="true" CodeBehind="MaterialConsumption.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.MaterialConsumption" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/MaterialConsumption.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MaterialConsumption %></h1>
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
                            <%-- <li>
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>--%>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                          <%--  <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" TabIndex="36" />
                            </li>
                            <%--  <li>
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
        <div id="DefaultMaterialMaster">
            <asp:HiddenField ID="MaterialDetailId" Value="0" runat="server" />
            <asp:HiddenField ID="ITM_CODE" runat="server" />
            <asp:HiddenField ID="ITM_MIN_STK" runat="server" Value="0" />
            <asp:HiddenField ID="ITM_ROL_STK" runat="server" Value="0" />
            <asp:HiddenField ID="ITM_MAX_STK" runat="server" Value="0" />
            <asp:HiddenField ID="ICH_STATUS" runat="server" Value="1" />
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
                                <%=Resources.Controls.MCN%></label>
                            <asp:Label ID="lblMaterilaConsumptionNo" runat="server" Text=""></asp:Label>
                            <div class="clear">
                            </div>
                            <label>
                                <%=Resources.Controls.Date%>*</label>
                            <asp:TextBox ID="ICH_DATE" runat="server" TabIndex="1"></asp:TextBox>
                            <asp:HiddenField ID="ICH_NO" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <label for="Store">
                                <%=Resources.Controls.ConsumptionStore%>*</label>
                            <asp:DropDownList ID="ICH_DEPT" runat="server" TabIndex="1" onchange="javascript:FillDepartement();"
                                EnableViewState="False">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <%-- <label for="Departement">
                        <%=Resources.Controls.RequestingStore%>*</label>
                    <asp:DropDownList ID="DeptPk" runat="server" TabIndex="2" Width="200px" EnableViewState="False">
                    </asp:DropDownList>
                    <div class="clear"></div>--%>
                            <label for="ICH_ITEM_TYPE">
                                <%=Resources.Controls.Type%>*</label>
                            <asp:DropDownList ID="ICH_ITEM_TYPE" runat="server" TabIndex="2" EnableViewState="False"
                                onchange="javascript:FillNewCategories();">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="ICH_PK" />
            <asp:HiddenField runat="server" ID="ConsumptionDtl" />
            <div id="Order" class="gridwrap">
                <div id="ProductInsert">
                    <table id="tblProductInsert" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th width="25%" align="left">
                                    <%=Resources.Controls.MaterialType%>*
                                </th>
                                <th width="20%" align="left">
                                    <b>
                                        <%=Resources.Controls.MaterialCode%>* </b>
                                </th>
                                <th width="8%" align="left">
                                    <b>
                                        <%=Resources.Controls.Stock%>* </b>
                                </th>
                                <th width="13%">
                                    <b>
                                        <%=Resources.Controls.QtyConsumed%></b>*
                                </th>
                                <th width="6%" align="left">
                                    <b>
                                        <%=Resources.Controls.UOM%></b>*
                                </th>
                                <th width="18%" align="left">
                                    <b>
                                        <%=Resources.Controls.Comments%>
                                    </b>
                                </th>
                                <th width="10%" align="left">
                                    <b>
                                        <%=Resources.Controls.Action%></b>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="grd-rowhead">
                                <td width="25%">
                                    <asp:DropDownList ID="MaterialType" runat="server" Width="85%" TabIndex="3"
                                        onchange="javascript:FillCategoryDetails($(this).val());">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imbViewCag" runat="server"  ToolTip="Open Item Category"
                                        SkinID="btnview" TabIndex="4" OnClientClick="javascript:return ShowCategory();"
                                        EnableViewState="false" />
                                </td>
                                <td width="20%">
                                    <asp:DropDownList ID="ITV_ITEM" runat="server" Width="98%" TabIndex="5"
                                        onchange="javascript:GetCurrentStock($(this).val());">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                </td>
                                <td style="text-align: right" width="8%">
                                    <asp:Label ID="CurrentStock" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right" width="13%">
                                    <asp:TextBox ID="ICD_QTY_CONSUMED" runat="server" TabIndex="7" EnableViewState="false"
                                        Text="" CssClass="numeric" MaxLength="12" Width="90px">
                                    </asp:TextBox>
                                </td>
                                <td width="6%">
                                    <asp:DropDownList ID="ICD_UOM" runat="server" TabIndex="8" Width="80px" EnableViewState="false">
                                    </asp:DropDownList>
                                </td>
                                <td width="18%">
                                    <asp:TextBox ID="ICD_REMARKS" runat="server" MaxLength="250" TabIndex="9" EnableViewState="false"
                                        Width="125px">
                                    </asp:TextBox>
                                </td>
                                <td align="left" width="10%">
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
                        <th fieldmap="ICD_UOM" isvisible="false">
                        </th>
                        <th fieldmap="ICD_ITEM_CATEGORY" isvisible="false">
                            <%--MaterialTypePk--%>
                        </th>
                        <th fieldmap="ICD_ITEM_CATEGORY_TEXT" align="left" width="30%">
                            <%--MaterialType --%>
                            <%=Resources.Controls.MaterialType%>*
                        </th>
                        <th fieldmap="ICD_ITEM_TEXT" align="left" width="10%">
                            <%--MaterialCode--%>
                            <%=Resources.Controls.MaterialCode%>*
                        </th>
                        <th fieldmap="ICD_CURRENT_STK" align="right" width="8%">
                            <%--CurrentStock--%>
                            <%=Resources.Controls.Stock%>*
                        </th>
                        <th fieldmap="ICD_QTY_CONSUMED" align="right" width="17%">
                            <%=Resources.Controls.QtyConsumed%>*
                        </th>
                        <th fieldmap="ICD_UOM_TEXT" align="left" width="5%">
                            <%=Resources.Controls.UOM%>*
                        </th>
                        <th fieldmap="ICD_REMARKS" align="left" width="15%">
                            <%=Resources.Controls.Comments%>
                        </th>
                        <th type="Template" width="10%" align="left">
                            <div>
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" Width="15px" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" Width="15px"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <%--#####END Add user Controls and process Id, app ID#####--%>
        <asp:HiddenField runat="server" ID="EditRequisition" Value="0" />
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
            <asp:Button ID="btnSaveandSubmit" runat="server" CssClass="inputbtn" Text="<%$Resources:Controls,Saveandsubmit%>"
                Style="width: 140px" OnClientClick="javascript:return SavePage();" />
        </div>
    </div>
</asp:Content>
