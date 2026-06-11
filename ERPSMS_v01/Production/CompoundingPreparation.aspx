<%@ Page Title="<%$ Resources:Captions,Title_CompoundingPreparation %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="CompoundingPreparation.aspx.cs"
    Theme="Classic" Inherits="ERPSMS_v01.Production.CompoundingPreparation" EnableEventValidation="false" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/CompoundPreparation.js.axd" type="text/javascript"></script>
    <script src="../Scripts/jquery/UI/timepicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery/UI/jquery.ui.datetimepicker.js" type="text/javascript"></script>
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                <asp:Button runat="server" ID="btnSaveSubmit" TabIndex="9" Text="<%$resources:ErpRes,SaveSubmit %>"
                                    OnClientClick="javascript:return ShowWkfSubmitPopUp();" ToolTip="<%$resources:ErpRes,SaveSubmit %>"
                                    SkinID="btnInner-submit" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="9" Text="<%$resources:ErpRes,Submit %>"
                                    OnClientClick="javascript:return ShowWkfSubmitPopUp();" ToolTip="<%$resources:ErpRes,Submit %>"
                                    CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                            </li>
                            <li>
                                <asp:Button ID="btnSave" runat="server" TabIndex="9" ToolTip="<%$resources:Controls,Save %>"
                                    Text="<%$Resources:Controls,Save %>" SkinID="btnInner-Save" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" TabIndex="10" OnClientClick="javascript:return ResetPage();"
                                    ToolTip="<%$resources:Controls,Cancel %>" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel %>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div id="grdTable-wrap" class="content-wrapper">
        <div id="divData">
            <div id="tab1Content">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=Resources.Controls.CompoundBatchno%>
                                </label>
                                <asp:Label ID="Batch_No" runat="server"></asp:Label>
                                <asp:HiddenField runat="server" ID="CTH_BATCH_NO" Value="0" />
                                <div style="display: none">
                                    <label for="CTH_PLAN">
                                        <%=Resources.Controls.Plan%>
                                        *</label>
                                    <asp:DropDownList runat="server" ID="CTH_PLAN">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=Resources.Controls.CompDate%>
                                    *</label>
                                <asp:TextBox ID="CTH_COMP_DT" runat="server" CssClass="date-picker"  TabIndex="1"></asp:TextBox>
                                <asp:HiddenField ID="CTH_TOTAL_TM_UNIT" Value="8" runat="server" />
                                <asp:HiddenField ID="CTD_PK" Value="0" runat="server" />
                                <div style="display: none">
                                    <label for="lblPreparedBy">
                                        <%=Resources.Controls.PreparedBy%></label>
                                    <asp:Label ID="lblPreparedBy" runat="server"></asp:Label>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <h1 class="search-colapse-normal">
                    <%=Resources.Captions.CompoundDetails%></h1>
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=Resources.Controls.CompoundName%>
                                    *</label>
                                <asp:DropDownList runat="server" ID="CTH_COMPOUND"  TabIndex="1" onchange="javascript:return GetCompoundDetails();">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="CTH_TANK_NO">
                                    <%=Resources.Controls.TankUsed%>
                                    *</label>
                                <asp:DropDownList runat="server" ID="CTH_TANK_NO"  TabIndex="3" onchange="javascript:return FillTanKcapacity();" >
                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S" style="float: right; margin-right: 0">
                                <label for="CTH_QUANTITY">
                                    <%=Resources.Controls.Quantity%>
                                    *</label>
                                <asp:TextBox runat="server" ID="CTH_QUANTITY" CssClass="numeric"   TabIndex="2" onchange="FillQuantityChange()"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="14" Width="100px"></asp:TextBox>
                                <asp:DropDownList runat="server" ID="CTH_QUANTITY_UOM" Width="100px"  TabIndex="2">
                                </asp:DropDownList>

                          <%--      Tank Capacity--%>                          
                                        <label for="TankCapacity">
                                            <%=Resources.Controls.TankCapacity%>
                                            *
                                        </label>
                                        <asp:TextBox ID="TankCapacity" runat="server" Width="100px" TabIndex="4" ClientIDMode="Static"
                                            ReadOnly="true" CssClass="numeric input-disabled">
                                        </asp:TextBox>
                                        <asp:DropDownList ID="UOMPk" runat="server" Width="100px" TabIndex="4">
                                        </asp:DropDownList>                                  

                           <%--     End tank Capacity--%>

                                <label>
                                </label>
                                <asp:Button ID="btnCalculate" runat="server" Text="<%$Resources:Controls,CalculateDistribution%>"
                                    OnClientClick="javascript:return FillCompoundDetails();" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div style="float: right">
                    <a href="#" id="lnkViewInspection" onclick="javascript:ViewInspctionDetails();">
                    </a>
                </div>
                <div class="clear">
                </div>
                <div class="grdTable" id="divMaterialInsert">
                    <table rules="all" id="MaterialInsert"  class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th width="20%">
                                    <%=Resources.Controls.MaterialType%>
                                    *
                                </th>
                                <th width="25%">
                                    <%=Resources.Controls.Material%>
                                    *
                                </th>
                                <th width="15%">
                                    <%=Resources.Controls.BatchNo%>
                                    *
                                </th>
                                <th width="10%">
                                    <%=Resources.Controls.CurrentStock%>
                                </th>
                                <th width="15%">
                                    <%=Resources.Controls.Quantity%>
                                    *
                                </th>
                                <th width="13%">
                                </th>
                                <th width="7%">
                                    <%=Resources.Controls.Add%>
                                    *
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="MaterialType" Width="90%" onChange="javascript:FillMaterialNames($(this).val())">
                                        <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:BindValues, RawMaterial%>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:BindValues, Dispersion%>" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:BindValues, Compound%>" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="Material" Width="90%" onchange="javascript:FillMaterialDetails();">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="BatchNo" Width="90%" onchange="javascript:FillBatchQuantity($(this).parent().parent());">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right" id="tdCurrentStock">
                                </td>
                                <td>
                                    <div style="width: 110px;">
                                        <asp:TextBox runat="server" Width="80%" ID="MaterialQuantity" Style="float: left;" CssClass="numeric input-w75"
                                            MaxLength="14" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="MaterialQuantityUOM" Value="0" />
                                        <span id="spnUOM" style="float: right; width: 11px !important;"></span>
                                    </div>
                                </td>
                                <th width="10%" id="tdReqQty" style="background-color: transparent;">
                                </th>
                                <td width="7%">
                                    <asp:ImageButton runat="server" ID="imbMaterialAdd" SkinID="imbaddnew" OnClientClick="javascript:return AddCompoundMaterials();" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="grdTable" id="divgrdCompoundingGrid">
                    <table rules="all" id="grdCompounding" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                        editable="true" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th fieldmap="CTD_PK" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="CPD_ITEM_CATEGORY" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="CTD_ITEM" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="CTD_ITEM_TYPE" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="CPD_COMP_PERC" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="CPD_QUANTITY" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="BALANCE_REQ" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="SLNO" isvisible="false">
                                </th>
                                <th fieldmap="MATERIALTYPENAME" width="15%">
                                    <%=Resources.Controls.MaterialType%>
                                    *
                                </th>
                                <th fieldmap="MaterialName" align="left" width="22%">
                                    <%=Resources.Controls.Material%>*
                                </th>
                                <th fieldmap="UOM_CODE" align="left" width="18%">
                                    <%=Resources.Controls.BatchNo%>
                                </th>
                                <th fieldmap="STOCK" align="right" width="12%">
                                    <%=Resources.Controls.CurrentStock%>
                                </th>
                                <th fieldmap="CPD_DRY_PERC" align="left" width="15%">
                                    <%=Resources.Controls.ActualQty%>*
                                </th>
                                <th fieldmap="CTD_REQ_QTY" align="right" width="10%" id="threqQty">
                                    <%=Resources.Controls.QtyReq%>
                                </th>
                                <th type="Template" width="8%" align="center" id="thAction">
                                    <div style="text-align: left;">
                                        <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="clear">
                </div>
                <%-- //CheckList Region Start--------------------------------------%>
                <div class="grdTable" id="divgrdChecklist">
                    <h1 class="search-colapse-normal">
                        <%=Resources.Captions.CheckList%>
                    </h1>
                    <table rules="all" id="grdChecklistDetails" grandtype="GrandGrid" paging="false"
                        width="100%" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th fieldmap="CDL_PK" isvisible="false">
                                </th>
                                <th fieldmap="CDL_CHECK_LIST_DTL" isvisible="false">
                                </th>
                                <th fieldmap="CDL_SL_NO" isvisible="false">
                                </th>
                                <%--<th fieldmap="CDL_ACTIVE" isvisible="false">
                                </th>--%>
                                <th fieldmap="CDL_NAME" align="left">
                                    <%=Resources.Controls.ChecklistItem%>
                                </th>
                                <th fieldmap="CDL_VALUE" align="left">
                                    <%=Resources.Controls.CheckListValue%>
                                </th>
                                <th fieldmap="CDL_DESC" align="left">
                                    <%=Resources.Controls.CheckListRemarks%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <%----------------------------  //End CheckList----------------------%>
                <div class="clear">
                </div>
                <div class="div2col-S">
                    <label for="CTH_START_TM">
                        <%=Resources.Controls.StartTime%>
                        *</label>
                    <asp:TextBox runat="server" ID="CTH_START_TM" Width="16%"></asp:TextBox>
                    <asp:TextBox runat="server" ID="CTH_ST_TM" Width="10%"></asp:TextBox>
                    <asp:HiddenField ID="hdnDate" runat="server" />
                    <asp:HiddenField runat="server" ID="hdnFromDate" />
                    <div class="clear">
                    </div>
                    <label for="CTH_END_TM">
                        <%=Resources.Controls.EndTime%>
                        *</label>
                    <asp:TextBox runat="server" ID="CTH_END_TM" Width="16%"></asp:TextBox>
                    <asp:TextBox runat="server" ID="CTH_ED_TM" Width="10%"></asp:TextBox>
                    <div class="clear">
                    </div>
                    <asp:HiddenField runat="server" ID="hdnTodate" />
                </div>
                <div class="div2col-S" style="float: right; margin-right: 0">
                    <label for="CTH_TOTAL_TM">
                        <%=Resources.Controls.TotalHrs%>
                        *</label>
                    <asp:TextBox runat="server" ID="CTH_TOTAL_TM" MaxLength="8" CssClass="small-a"></asp:TextBox>
                </div>
                <div class="clear">
                </div>
                <%-- <div id="Wofkflowdiv">--%>
                <div id="divWkfSubmit" style="display: none;">
                    <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
                    <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
                    <asp:HiddenField runat="server" ID="ActionID" Value="0" />
                    <asp:HiddenField ID="ApplicationID" runat="server" Value="0" />
                    <asp:HiddenField ID="AppNo" runat="server" Value="0" />
                    <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
                </div>
                <div class="clear">
                </div>
                <div id="divInspectionDetails" title="<%=Resources.Controls.InspectionList%>">
                    <div class="grdTable">
                        <table rules="all" id="grdInspectionDetails" grandtype="GrandGrid" paging="false"
                            width="100%" class="gridwraptable gridwrap filter-arrow">
                            <thead>
                                <tr>
                                    <th fieldmap="TIH_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="TIH_NO" width="40%">
                                        <%=Resources.Controls.TestReport%>
                                    </th>
                                    <th fieldmap="TIH_DATE" width="25%" align="center">
                                        <%=Resources.Controls.Date%>
                                    </th>
                                    <th fieldmap="TIH_TEST_TEXT" align="right" width="20%">
                                        <%=Resources.Controls.Test%>
                                    </th>
                                    <th fieldmap="TIH_RESULT_TEXT" align="left" width="15%">
                                        <%=Resources.Controls.Result%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
                <div id="divRawMaterialInspection" title="<%=Resources.Controls.InspectionDetails%>">
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.Date%>
                        </label>
                        <asp:Label ID="lblRawDate" runat="server"></asp:Label>
                        <label>
                            <%=Resources.Controls.TestReport%>
                        </label>
                        <asp:Label ID="lblTestReport" runat="server"></asp:Label>
                    </div>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.TestConductedAt%>
                        </label>
                        <asp:Label ID="lblTestConductedAt" runat="server"></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                    <h1>
                        <%=Resources.Controls.MaterialDetails%></h1>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.BatchType%></label>
                        <asp:Label ID="lblBatchType" runat="server"></asp:Label>
                        <label>
                            <%=Resources.Controls.LotBatch%></label>
                        <asp:Label ID="lblLotBatch" runat="server"></asp:Label>
                    </div>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.Material%></label>
                        <asp:Label ID="lblMaterial" runat="server"></asp:Label>
                        <label>
                            <%=Resources.Controls.LotSizeQty%></label>
                        <asp:Label ID="lblLotSize" runat="server"></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                    <h1>
                        <%=Resources.Controls.SamplingPlanDetails%></h1>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.Test%>
                        </label>
                        <asp:Label ID="lblTest" runat="server"></asp:Label>
                        <label>
                            <%=Resources.Controls.SampleTaken%></label>
                        <asp:Label ID="lblSampleTaken" runat="server"></asp:Label>
                    </div>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.SampleSize%></label>
                        <asp:Label ID="lblSampleSize" runat="server"></asp:Label>
                    </div>
                    <div class="grdTable">
                        <table rules="all" id="grdRawMaterialInspection" grandtype="GrandGrid" paging="false"
                            width="100%" class="gridwraptable gridwrap filter-arrow">
                            <thead>
                                <tr>
                                    <th fieldmap="TID_MIN_VALUE" isvisible="false">
                                    </th>
                                    <th fieldmap="TID_MAX_VALUE" isvisible="false">
                                    </th>
                                    <th fieldmap="TID_PARAMETER" align="left" width="40%">
                                        <%=Resources.Controls.Parameter%>
                                    </th>
                                    <th fieldmap="TID_UOM_TEXT" align="center" width="15%">
                                        <%=Resources.Controls.UOM%>
                                    </th>
                                    <th fieldmap="TID_STD_VALUE" align="right" width="15%">
                                        <%=Resources.Controls.StandardValue%>
                                    </th>
                                    <th fieldmap="TID_VALUE" align="right" width="15%">
                                        <%=Resources.Controls.ObservedValue%>
                                    </th>
                                    <th fieldmap="TID_VARIANCE" align="right" width="15%">
                                        <%=Resources.Controls.Varience%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="CompoundDetailsList" />
    <asp:HiddenField runat="server" ID="EditProduct" Value="0" />
    <asp:HiddenField runat="server" ID="CTH_PK" Value="0" />
    <asp:HiddenField runat="server" ID="CTH_ACTIVE" Value="1" />
    <asp:HiddenField runat="server" ID="CompoundDetail" />
    <asp:HiddenField runat="server" ID="MaterialStock" />
    <asp:HiddenField runat="server" ID="isPlanRequired" />
    <asp:HiddenField runat="server" ID="UitemID" />
    <asp:HiddenField runat="server" ID="UcategoryID" />
    <asp:HiddenField runat="server" ID="currentQty" />
    <asp:HiddenField runat="server" ID="hdfDeptID" Value="0" />
    <asp:HiddenField runat="server" ID="CTH_DEPT" Value="0" />
    <asp:HiddenField ID="CheckListDtl" runat="server" />
    <asp:HiddenField ID="hdfDSPCHECKLISTHDR" runat="server" />
    <div id="divArray" class="clear">
    </div>
</asp:Content>
