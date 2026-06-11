<%@ Page Title="<%$ Resources:Captions,Title_StockTransfer %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" EnableEventValidation="false" CodeBehind="MaterialIssue.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.StoreAccept" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/MaterialIssue.js.axd" type="text/javascript"></script>
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>

<style>
    .search-wrap-custom1 {
        padding: 8px 10px 30px !important;
    }
    #ctl00_MainContent_DivChkPendingWO {
  margin-left: 0 !important;
}

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MaterialIssueSlip%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="7" EnableViewState="False"
                OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="8" EnableViewState="False"
                OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False"
                OnClientClick="javascript:return CancelFun();" TabIndex="9" />
        </div>
    </div>--%>
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <div class="buttoncontainer-fields floatLeft" style="display: none">
                            <asp:DropDownList ID="MIH_COMPANY" TabIndex="1" class="margnbotm0" runat="server" Width="155px">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdfSelCompany" runat="server" />
                        </div>
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
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelPage();" TabIndex="36" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnPrint" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    TabIndex="37" EnableViewState="False" OnClientClick="javascript:return PrintPage();" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
        </div>
        <asp:HiddenField ID="ConfirmStockValueChange" runat="server" Value="0" />
        <asp:HiddenField ID="hdfEnableBatch" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
        <asp:HiddenField ID="hdfCurrentDate" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <asp:HiddenField ID="MIH_IS_EDIT" runat="server" Value="0" />
        <asp:HiddenField ID="MIH_IS_WORK_ORDER" runat="server" Value="0" />
        <asp:HiddenField ID="hdfMihDate" runat="server" Value="0" />
    </div>
    <div class="content-wrapper">
        <asp:HiddenField runat="server" ID="ActionID" />
        <table class="table-devide" id="tblDetailHdr">
            <tr>
                <td>
                    <div class="div2col-S">
                        <label for="lblMINo">
                            <%=MenuType == 1 ? Resources.Controls.WOTNo : Resources.Controls.STNo%>
                        </label>
                        <asp:Label ID="lblMINo" runat="server" Text="" EnableViewState="false" CssClass="input-small"></asp:Label>
                        <asp:HiddenField ID="APT_CODE" runat="server" />
                        <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                        <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfSRSStore" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfApplicationID" runat="server" Value="0" />
                        <%--<asp:HiddenField ID="hdfAppId" runat="server" Value="0" />--%>
                        <label for="lblMIDate" class="middle-lbl">
                            <%=MenuType == 1 ? Resources.Controls.WOTDate : Resources.Controls.STDate%>
                        </label>
                        <asp:TextBox ID="MIHDATE" runat="server" TabIndex="1" onkeydown="return CheckKey(event)"
                            onpaste="return false;" CssClass="input-small"></asp:TextBox>
                        <asp:HiddenField ID="MIH_PK" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="MIH_STATUS" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="MIHNO" runat="server" />

                        <div class="clear">
                        </div>
                        <div id="DivSbu">
                            <label for="Store" class="store-label">
                                <%=Resources.Controls.SBU%>*</label>
                            <asp:DropDownList ID="MRH_TO_BIZUNIT" runat="server" ClientIDMode="Static" TabIndex="3" Width="200px"
                                CssClass="select-half-a" EnableViewState="False">
                            </asp:DropDownList>
                            <asp:HiddenField ID="MRH_TO_BIZUNIT_VAL" runat="server" />
                        </div>

                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <label for="MIH_DEPT" class="middle-lbl-small">
                            <%=Resources.Controls.IssuedBy%>
                            *</label>
                        <asp:DropDownList ID="MIH_DEPT" runat="server" onchange="javascript:FillDepartement();" CssClass="select-medium">
                        </asp:DropDownList>

                        <label for="MIH_DEPT_TO" class="middle-lbl-small">
                            <%=Resources.Controls.IssuedTo%>
                            *</label>
                        <asp:DropDownList ID="MIH_DEPT_TO" runat="server" TabIndex="2" CssClass="select-medium" onchange="javascript:AddNew();">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <h1 class="search-colapse-normal">
            <%--  <%=Resources.Captions.SrsDetails%>--%>
            <%=MenuType == 1 ? Resources.Captions.WODetails : Resources.Captions.SrsDetails%>
        </h1>
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label>
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                    TabIndex="4" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <%-- <asp:ListItem Value="MRH_NO" Text="<%$ Resources:BindValues, SRNo%>">
                    </asp:ListItem>
                     <asp:ListItem Value="WIH_NO" Text="<%$ Resources:BindValues, WihNo%>">
                    </asp:ListItem>--%>
                    <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, Item%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="5">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="6" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="7" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="8" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
                <div id="DivChkPendingWO" runat="server" visible="false">
                    <asp:CheckBox ID="ChkPendingWO" runat="server" ToolTip="Pending WO" Checked="true" onchange="javascript:return BindGrid();" />
                    <asp:Label ID="lblchkpendingWO" runat="server" Text="Pending WO"></asp:Label>
                </div>
            </div>

        </div>
        <div class="gridwrap" style="width: 100% !important">
            <div class="scroll-h150" style="height: 250px">
                <div id="divSRSList" class="gridwrap nomargin">
                    <table rules="all" id="grdSRSList" grandtype="GrandGrid" paging="false" enablecheckbox="true"
                        width="100%" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="MRH_PK" isvisible="false"></th>
                                <th fieldmap="MRD_PK" isvisible="false"></th>
                                <th fieldmap="MRD_ITEM" isvisible="false"></th>
                                <th fieldmap="MRD_UOM" isvisible="false"></th>
                                <th fieldmap="MRH_NO" align="left" width="15%">
                                    <%=MenuType == 1 ? Resources.Controls.WORNo : Resources.Controls.MRNo%>
                                </th>
                                <th fieldmap="MRH_DATE" align="left" width="12%">
                                    <%=MenuType == 1 ? Resources.Controls.WORDate :Resources.Controls.MRDate%>
                                </th>
                                <th fieldmap="ITM_NAME" align="left" width="30%">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <th fieldmap="MRD_QTY_APPROVED" align="right" width="13%">
                                    <%=Resources.Controls.ReqQty%>
                                </th>
                                <th fieldmap="MRD_QTY_ISSUED" align="right" width="11%">
                                    <%=Resources.Controls.QtyIssued%>
                                </th>
                                <th fieldmap="BALANCE_QTY" align="right" width="11%">
                                    <%=Resources.Controls.BalanceQty%>
                                </th>
                                <th fieldmap="UOM_CODE" align="left" width="8%">
                                    <%=Resources.Controls.UOM%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div class="button-wrap-right">
                        <asp:Button ID="AddToList" runat="server" Text="Add To List" TabIndex="8" OnClientClick="javascript:return AddToList();" />
                    </div>
                </div>
            </div>
        </div>


        <div id="divMaterial">
            <div style="display: none">
                <table id="tblEditMaterial" runat="server" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th width="15%" align="left">
                                <%=Resources.Controls.MRNo%>
                            </th>
                            <th width="25%" align="left">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th width="160px" align="left">
                                <%=Resources.Controls.BatchNo%>
                            </th>
                            <th width="8%" align="left">
                                <%=Resources.Controls.Qty%>
                            </th>
                            <th width="15%" align="left">
                                <%=Resources.Controls.IssueQty%>*
                            </th>
                            <th width="10%" align="left">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th width="15%" align="left">
                                <%=Resources.Controls.Action%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td width="15%" align="left">
                                <%--<asp:TextBox ID="SRSEdit" runat="server" Width="99%" Enabled="false" ></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlMRNo" runat="server" TabIndex="8" CssClass="right-M" Width="99%" onChange="return FillMRItems();">
                                </asp:DropDownList>
                                <%--<asp:HiddenField ID="SRSEditPK" runat="server" />--%>
                            </td>
                            <td width="25%" align="left">
                                <%--<asp:TextBox ID="SRSItemEdit" runat="server" Width="99%" Enabled="false" ></asp:TextBox>--%>
                                <asp:DropDownList ID="ddlMRItem" runat="server" TabIndex="8" CssClass="right-M" Width="99%" onChange="return MRItemChange();">
                                </asp:DropDownList>
                                <%--<asp:HiddenField ID="SRSItemEditPK" runat="server" />--%>
                            </td>
                            <td width="160px">
                                <asp:DropDownList ID="MID_BATCH" runat="server" TabIndex="8" CssClass="right-M" Width="99%" onChange="return BatchChangeEvent($(this).parent().parent());">
                                </asp:DropDownList>
                            </td>
                            <td width="8%" id="QTY_IN_STOCK">
                                <%-- <asp:Label ID="lblQtyInStock" runat="server"></asp:Label>--%>
                            </td>
                            <td width="15%" align="left">
                                <asp:TextBox ID="QtyIssued" Width="99%" runat="server" TabIndex="8" CssClass="Uiinput-amount numeric"></asp:TextBox>
                                <asp:HiddenField ID="QtyIssuedOrg" runat="server" />
                            </td>
                            <td width="10%" align="left">
                                <asp:DropDownList ID="UOM" runat="server" Width="99%" TabIndex="8" onchange="javascript:GetUomConversion($(this).val())"
                                    Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td width="15%" align="left">
                                <asp:HiddenField runat="server" ID="POD_CONV_FACT" Value="1" />
                                <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbSelect" Width="16px" Height="16px"
                                    OnClientClick="javascript:return AddMIDetails()" TabIndex="8" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <%--  <div class="div3col-VMappingListing">
                <label for="SRSEdit" style="width: 10%">
                    <%=Resources.Controls.SRSNo%>
                </label>
                <asp:TextBox ID="SRSEdit" runat="server" Enabled="false" Width="10%"></asp:TextBox>
                <asp:HiddenField ID="SRSEditPK" runat="server" />
                <label for="SRSItemEdit">
                    <%=Resources.Controls.Item%></label>
                <asp:TextBox ID="SRSItemEdit" runat="server" Enabled="false" Width="10%"></asp:TextBox>
                <asp:HiddenField ID="SRSItemEditPK" runat="server" />
                <label for="SRSItemEdit">
                    <%=Resources.Controls.BatchNo%></label>
                <asp:DropDownList ID="SRS_BATCH" runat="server" CssClass="right-M" Width="10%" onChange="return MaterialChangeEvent();">
                </asp:DropDownList>
                <label for="SRSItemEdit">
                    <%=Resources.Controls.IssueQty%>*</label>
                <asp:TextBox ID="QtyIssued" runat="server" Width="10%"></asp:TextBox>
                <asp:HiddenField ID="QtyIssuedOrg" runat="server" />
                <label for="UOM">
                    <%=Resources.Controls.UOM%></label>
                <asp:DropDownList ID="UOM" runat="server" onchange="javascript:GetUomConversion($(this).val())"
                    Width="90px" Enabled="false">
                </asp:DropDownList>
                <asp:HiddenField runat="server" ID="POD_CONV_FACT" Value="1" />
                <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbSelect" Width="16px" Height="16px"
                    OnClientClick="javascript:return AddMIDetails()" TabIndex="13" />
            </div>--%>
            <h1 id="pendingHdr" class="search-colapse-normal" runat="server">
                <%=MenuType == 1 ? Resources.Captions.PendingWODetails : Resources.Captions.PendingSRDetails%>
                <%--  <%=Resources.Captions.PendingSRDetails%>--%>
            </h1>
            <div id="divMIListinsert" class="scroll-container">
                <%-- class="gridwrap grid-w930"--%>
                <table rules="all" id="grdPendingSRSList" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                    editable="true" class="gridwraptable gridwrap tablefixwidth-td">
                    <thead>
                        <tr>
                            <th fieldmap="MID_PK" isvisible="false"></th>
                            <th fieldmap="IS_LESS_QTY" isvisible="false"></th>
                            <th fieldmap="MID_PO" isvisible="false"></th>
                            <th fieldmap="MRH_PK" isvisible="false"></th>
                            <th fieldmap="MID_SL_NO" isvisible="false"></th>
                            <th fieldmap="MID_MR_DTL" isvisible="false">
                                <%--MRD_PK--%>
                            </th>
                            <th fieldmap="MID_STK_BATCH" isvisible="false"></th>
                            <th fieldmap="MID_IS_MULTIPLE_BATCH" isvisible="false"></th>
                            <th fieldmap="MID_MULT_BTCH_GRP" isvisible="false"></th>
                            <th fieldmap="MID_ITEM" isvisible="false"></th>
                            <th fieldmap="MID_UOM" isvisible="false"></th>
                            <th fieldmap="ORG_BALANCE_QTY" isvisible="false"></th>
                            <th fieldmap="MRH_NO" align="left" width="90px">
                                <%=MenuType == 1 ? Resources.Controls.WORNo : Resources.Controls.MRNo%>
                            </th>
                            <th fieldmap="ITM_NAME" align="left" width="220px">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="MID_STK_BATCH_NO" width="140px">
                                <%=Resources.Controls.BatchNo%>
                            </th>
                            <th fieldmap="IMG" class="grd-head-left" width="20px"></th>
                            <th fieldmap="QTY_IN_STOCK" width="100px" align="right">
                                <%=Resources.Controls.Qty%>
                            </th>
                            <th fieldmap="MRD_QTY_APPROVED" align="right" width="70px">
                                <%=Resources.Controls.QtyRequestShort%>
                            </th>
                            <th fieldmap="MRD_QTY_ISSUED" align="right" width="75px">
                                <%=Resources.Controls.QtyIssued%>
                            </th>
                            <th fieldmap="MID_QTY_ISSUED" align="right" width="75px">
                                <%=Resources.Controls.IssueQty%>
                            </th>
                            <th fieldmap="UOM_CODE" align="left" width="40px">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="MID_REMARKS" align="left" width="130px">
                                <%=Resources.Controls.Remarks%>
                            </th>
                            <th type="Template" width="50px">
                                <div>
                                    <%-- <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />--%>
                                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" TabIndex="8" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <br class="clear" />
        </div>
        <%--  <div class="divcol-actiowrap" id="divAction" runat="server">
            <asp:Button ID="btnSaveandSubmit" runat="server" CssClass="inputbtn" Text="<%$Resources:Controls,Saveandsubmit%>"
                Style="width: 140px" OnClientClick="javascript:return SavePage();" />
        </div>--%>
        <div class="clear">
        </div>
        <div id="divPreviousSRS" class="grdTable" title="<%=Resources.Captions.PreviousSRSDetails%>">
            <table rules="all" id="grdPreviousSRS" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="MRH_NO" align="left" width="15%">
                            <%=MenuType == 1 ? Resources.Controls.WORNo : Resources.Controls.MRNo%>
                        </th>
                        <th fieldmap="ITM_NAME" align="left" width="15%">
                            <%=Resources.Controls.MaterialName%>
                        </th>
                        <th fieldmap="MID_NO" align="left" width="15%">
                            <%=Resources.Controls.MINo%>
                        </th>
                        <th fieldmap="MID_DATE" align="left" width="15%">
                            <%=Resources.Controls.MIDate%>
                        </th>
                        <th fieldmap="MID_QTY_ISSUED" align="right" width="8%">
                            <%=Resources.Controls.QtyIssued%>
                        </th>
                        <th fieldmap="UOM_CODE" align="left" width="8%">
                            <%=Resources.Controls.UOMCode%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <asp:HiddenField ID="MIList" runat="server" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
        <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
        <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
        <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
        <div id="divData">
        </div>
        <div id="divGridData">
        </div>
        <div id="divSaveData">
        </div>
        <div class="clear">
        </div>
        <div id="Wofkflowdiv" style="display: none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSlNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField ID="AppNo" runat="server" />
            <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
        </div>
        <%--  For New Workflow SP--%>
        <asp:HiddenField runat="server" ID="WKF_REFERENCE" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_APPLICATION" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_PROCESS" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_TASK" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_TASK_ACTION" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_COMMENTS" Value="" />
        <asp:HiddenField runat="server" ID="WKF_TRX_FLAG" Value="0" />
        <asp:HiddenField runat="server" ID="USER_PK" Value="" />
        <asp:HiddenField ID="hdfMenuType" runat="server" Value="0" />
        <asp:HiddenField ID="hdfAllowExcessIssue" runat="server" Value="0" />
        <asp:HiddenField ID="hdfExcessIssueValidated" runat="server" Value="0" />

        <div id="divPopupMaterialBatches" style="display: none" title="Add Batch Details">
            <div class="content-wrapper">
                <div class="button-wrap-right">
                    <asp:Button ID="btnApplyBatches" runat="server" SkinID="btnInner-add-dsd" TabIndex="205"
                        Text="Apply" OnClientClick="javascript:return ApplyPopUpBatches();" />
                </div>
                <div class="div2col-S">
                    <asp:Label runat="server" ID="lblItemNamePopup" Text="" CssClass="hgt-auto"></asp:Label>
                </div>
                <div class="gridwrap" id="divMultipleBatch">
                    <table id="BatchInsert" rules="all" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th align="left" width="40%">
                                    <%=Resources.Controls.BatchNo%>
                                </th>
                                <th class="grd-head-rgt" width="25%">
                                    <%=Resources.Controls.Stock%>
                                </th>
                                <th class="grd-head-rgt" width="17%">
                                    <%=Resources.Controls.Quantity%>
                                </th>
                                <th align="left" width="8%">
                                    <%=Resources.Controls.UOM%>
                                </th>
                                <th align="center" width="10%">
                                    <%=Resources.Controls.Action%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td align="left" width="40%">
                                    <asp:DropDownList runat="server" ID="ddlPopUpBatch" Width="210px" TabIndex="201"
                                        CssClass="right-M" onchange="javascript:FillPopUpBatchQuantity(0);">
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hdfItmType" Value="" />
                                    <asp:HiddenField runat="server" ID="hdfItmgroup" Value="" />
                                    <asp:HiddenField runat="server" ID="hdfTempQty" Value="0" />
                                </td>
                                <td align="right" width="25%">
                                    <asp:Label runat="server" ID="lblStockPopUp" Text=""></asp:Label>
                                </td>
                                <td align="right" width="17%">
                                    <asp:TextBox runat="server" ID="txtQtyPopUp" CssClass="numeric input-w100" TabIndex="202"
                                        autocomplete="off" MaxLength="14" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                </td>
                                <td align="left" width="8%">
                                    <asp:Label ID="lblPopupUOM" runat="server"></asp:Label>
                                </td>
                                <td align="center" width="10%">
                                    <asp:ImageButton runat="server" ID="imgAddBatchPopup" SkinID="imbaddnew" TabIndex="203"
                                        OnClientClick="javascript:return AddPopUpBatchDtls();" ValidationGroup="PopUpBatchAdd" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="grdTable">
                        <table rules="all" id="grdBatchDetails" grandtype="GrandGrid" paging="false" editable="true"
                            width="100%" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="SLNO" isvisible="false"></th>
                                    <th fieldmap="MID_STK_BATCH" isvisible="false"></th>
                                    <th fieldmap="MID_STK_BATCH_NO" align="left" width="40%"></th>
                                    <th fieldmap="QTY_IN_STOCK" class="grd-head-rgt" width="25%"></th>
                                    <th fieldmap="MID_QTY_ISSUED" class="grd-head-rgt" width="17%"></th>
                                    <th fieldmap="UOM_CODE" align="right" width="8%"></th>
                                    <th type="Template" width="10%" align="center" id="th1">
                                        <div style="text-align: center;">
                                            <asp:ImageButton runat="server" ID="imgDltBatch" SkinID="imbdeletegrid" ToolTip="<%$resources:Controls,Delete %>"
                                                TabIndex="204" OnClientClick="javascript:return PopUpGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <table style="font-weight: bold; background: #f1f1f1;">
                        <tr style="height: 25px;">
                            <td style="width: 40%;"></td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label runat="server" ID="lblQtyRequiredCaption" Text="<%$ resources:QtyRequired %>"
                                    CssClass="numeric"></asp:Label>
                            </td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label runat="server" ID="lblQtyRequired" Text=""></asp:Label>
                            </td>
                            <td style="width: 10%;"></td>
                        </tr>
                        <tr style="height: 25px;">
                            <td style="width: 40%;"></td>
                            <td style="width: 28%; text-align: right;">
                                <asp:Label runat="server" ID="lblQtyAlradyIssuedCaption" Text="<%$ resources:AlreadyissuedQty %>"
                                    CssClass="numeric"></asp:Label>
                            </td>
                            <td style="width: 22%; text-align: right;">
                                <asp:Label runat="server" ID="lblQtyAlradyIssued" Text=""></asp:Label>
                            </td>
                            <td style="width: 10%;"></td>
                        </tr>
                        <tr style="height: 25px;">
                            <td style="width: 40%;"></td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label ID="lblTotalBatchQtyCaption" runat="server" Text="<%$ resources:TotalBatchQty %>"></asp:Label>
                            </td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label runat="server" ID="lblTotalIssuingQty" Text=""></asp:Label>
                            </td>
                            <td style="width: 10%;"></td>
                        </tr>
                        <tr style="height: 25px;">
                            <td style="width: 40%;"></td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label runat="server" ID="lblQtyBalCaption" Text="<%$ resources:BalRequired %>"
                                    CssClass="numeric"></asp:Label>
                            </td>
                            <td style="width: 25%; text-align: right;">
                                <asp:Label runat="server" ID="lblQtyBalance" Text=""></asp:Label>
                            </td>
                            <td style="width: 10%;"></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        function fnConfirmStockValueChange(command) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= Resources.Messages.MayAffectStockValueConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('1');
                        SavePage(command);
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('0');
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
