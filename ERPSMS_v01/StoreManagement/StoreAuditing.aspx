<%@ Page Title="<%$ Resources:Captions,Title_StoreAudit %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="StoreAuditing.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.StoreAuditing" EnableEventValidation="false" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StoreAudit.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.StoreAudit%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="7" EnableViewState="False"
                OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton ID="imdReset" runat="server" TabIndex="34" EnableViewState="false"
                SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
        </div>
    </div>--%>
    <asp:HiddenField ID="hdfEnableBatch" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
    <asp:HiddenField ID="hdfCurrentDate" runat="server" />
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
                                    TabIndex="33" ToolTip="<%$resources:ErpRes,Submit %>" EnableViewState="False"
                                    OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="34" EnableViewState="False" ToolTip="<%$resources:ErpRes,Save %>" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    TabIndex="35" ToolTip="<%$resources:Controls,Close %>" EnableViewState="False"
                                    OnClientClick="javascript:return CancelFun();" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div id="divData">
            <asp:HiddenField ID="ItemList" runat="server" />
            <asp:HiddenField ID="SAH_PK" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="ViewStatus" Value="0" />
            <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label for="SANo">
                                <%=Resources.Controls.StoreAuditNo%></label>
                            <asp:Label ID="SAHTXT_NO" runat="server" CssClass="input-small-b"></asp:Label>
                            <asp:HiddenField ID="SAH_NO" runat="server" />
                            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                            <label for="SAH_DATE" class="middle-lbl">
                                <%=Resources.Controls.Date%></label>
                            <%--<asp:Label ID="SAHTXT_DATE" runat="server"></asp:Label>--%>
                            <asp:TextBox ID="SAHTXT_DATE" runat="server" TabIndex="1" onkeydown="return CheckKey(event)"
                                onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                            <asp:HiddenField ID="SAH_DATE" runat="server" />
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S" style="float: right; margin-right: 0">
                            <div class="content">
                                <label for="Store">
                                    <%=Resources.Controls.Store%>
                                    *
                                </label>
                                <asp:DropDownList ID="SAH_DEPT_STORE" runat="server" onChange="javascript:ClearMaterialDetails();"
                                    CssClass="select-half" EnableViewState="false" TabIndex="2">
                                </asp:DropDownList>
                                <div id="divSBUCompany">
                                    <label for="SAH_COMPANY">
                                        <%=Resources.Controls.Company%>*</label>
                                    <asp:DropDownList ID="SAH_COMPANY" runat="server" TabIndex="2" ClientIDMode="Static"
                                        CssClass="select-half">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfSelCompany" runat="server" />
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
        <div id="searchwrap" class="search-wrap-custom1">
            <label><%=Resources.Controls.MaterialCategory%>*</label>
            <%--<asp:DropDownList runat="server" ID="MaterialType" TabIndex="2" onChange="javascript:ChangeBatchMode();" CssClass="margnrgt1per"
                Width="100px">
                <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:BindValues, General%>" Value="1"></asp:ListItem>                
            </asp:DropDownList>--%>
            <div class="margnlft-minus12 margnrgt5">
            <asp:TextBox ID="MaterialCategory" runat="server" TabIndex="2"></asp:TextBox>
            <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
            </div>
            <label> <%=Resources.Controls.Item%>*</label>
            <div class="margnlft-minus12 margnrgt5">
                <asp:TextBox ID="ItemTXTCode" runat="server" TabIndex="3" Width="290px"></asp:TextBox>
                <asp:HiddenField ID="ItemCode" runat="server" Value=""></asp:HiddenField>
            </div>
            <div id="divBatch">
                <label>
                    <%=Resources.Controls.BatchNo%>*</label>
                <asp:DropDownList runat="server" ID="BatchNo" TabIndex="4" Width="200px">
                </asp:DropDownList>
            </div>
            <%------------- Stk Batch Adding--------------%>
            <div id="divStkBatch" class="margnrgt5">
                <label>
                    <%=Resources.Controls.BatchNo%>*</label>
                <asp:TextBox ID="StkBatchNo" runat="server" Width="120px" TabIndex="4">
                </asp:TextBox>
                <asp:HiddenField ID="StkBatchNoPK" runat="server" Value="0"></asp:HiddenField>
            </div>
            <%------------ End Stk Batch--------------------%>
            <%-- Show Batches with Zero Qty--%>
            <div id="divZeroQtyBatches">
                <asp:CheckBox ID="chkbxZeroQtyBatches" runat="server" TabIndex="5" Text="<%$ Resources:BindValues, FullQtyIssued%>"
                    ToolTip="<%$ Resources:BindValues, FullQtyIssdToolTip%>" onchange="FillStkBatchNoAutoComplete();" />
            </div>
            <%-- End Full Qty Checkbox ------------------------------------%>
            <asp:ImageButton ID="imbAddItem" runat="server" SkinID="imbaddnew" TabIndex="5" Width="16px"
                CssClass="srchbtn" Height="16px" ToolTip="<%$Resources:Controls,Add%>" OnClientClick="javascript:return AddMaterial();" />
            <div class="clear">
            </div>
        </div>
        <div class="scroll-h150">
            <div class="grdTable" id="divGrdItemList">
                <table rules="all" id="grdItemList" grandtype="GrandGrid" pagesize="20" paging="true"
                    width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="SAD_ID" isvisible="false">
                            </th>
                            <th fieldmap="SAD_PK" isvisible="false">
                            </th>
                            <th fieldmap="SAD_ITEM_CATEGORY" isvisible="false">
                            </th>
                            <th fieldmap="SAD_ITEM" isvisible="false">
                            </th>
                            <th fieldmap="SAD_ITEM_BATCH" isvisible="false">
                            </th>
                            <th fieldmap="SAD_STK_BATCH" isvisible="false">
                                <%--Stock Batch No PK--%>
                            </th>
                            <th fieldmap="SAD_ITEM_CATEGORY_TEXT" sortable="true" width="8%" align="left">
                                <%=Resources.Controls.Category%>
                            </th>
                            <th fieldmap="ITM_TEXT" sortable="true" width="20%" align="left">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="SAD_STK_BATCH_NO" sortable="true" width="10%" align="left">
                                <%--Stock Batch No--%>
                                <%=Resources.Controls.BatchNo%>
                            </th>
                            <th fieldmap="ITM_UOM_TEXT" sortable="true" width="5%" align="left">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="SAD_CUR_STK" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.LedgerStk%>
                            </th>
                            <th fieldmap="SAD_CUR_STK_VAL" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.LedgerVal%>
                            </th>
                            <th fieldmap="SAD_ACT_STK" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.ActualStk%>
                                *
                            </th>
                            <th fieldmap="SAD_ACT_STK_VAL" sortable="true" width="8%" align="right">
                                <%=Resources.Controls.ActualVal%>
                            </th>
                            <th fieldmap="SAD_Damage" sortable="true" width="14%" align="left">
                                <%=Resources.Controls.DamageDetails%>
                            </th>
                            <th fieldmap="SAD_REMARKS" sortable="true" width="10%" align="left">
                                <%=Resources.Controls.Remarks%>
                            </th>
                            <th type="Template" width="5%" align="left">
                                <div>
                                    <asp:ImageButton runat="server" ID="imbAddDamage" ToolTip="<%$Resources:Controls,AddDamageDetails%>"
                                        TabIndex="5" Visible="false" SkinID="imgaddnew" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'ADDDAMAGE') " />
                                    <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                        TabIndex="5" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div id="Wofkflowdiv" style="display: none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField ID="AppNo" runat="server" />
            <asp:HiddenField ID="SAH_STATUS" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="ActionID" />
            <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
        </div>
        <div id="divDatas">
        </div>
        <div id="divDamage" title="Damage Details">
            <div class="Button-container-popup" id="divDamageButtons">
                <asp:Button runat="server" ID="btnSaveDamage" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                    OnClientClick="javascript:return AddDamageDetails()" />
                <asp:Button runat="server" ID="btnCancelDamage" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Clear%>"
                    OnClientClick="javascript:return ClearDamageDetails()" />
            </div>
            <div class="content-wrapper">
                <div class="divcolmiddle-S">
                    <label for="SDD_DMG_TYPE">
                        <%=Resources.Controls.DamageType%>*
                    </label>
                    <asp:DropDownList runat="server" ID="SDD_DMG_TYPE">
                    </asp:DropDownList>
                    <label for="SDD_DMG_QTY">
                        <%=Resources.Controls.Quantity%>
                        *</label>
                    <asp:TextBox ID="SDD_DMG_QTY" onkeydown="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                        runat="server" EnableViewState="false"></asp:TextBox>
                    <asp:HiddenField ID="hdfVariations" runat="server" />
                    <asp:HiddenField ID="PrvQuantity" runat="server" Value="0" />
                </div>
                <table rules="all" id="grdDamageDtls" grandtype="GrandGrid" editfunction="DamageGridHandler"
                    editable="true" style="width: 100%" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="SDD_PK" isvisible="false">
                            </th>
                            <th fieldmap="SDD_DMG_TYPE" isvisible="false">
                            </th>
                            <th fieldmap="SDD_DMG_TYPE_TEXT" width="65%">
                                <%=Resources.Controls.DamageType%>
                            </th>
                            <th fieldmap="SDD_DMG_QTY" width="15%" align="right">
                                <%=Resources.Controls.Quantity%>
                            </th>
                            <th type="Template" align="center" width="15%">
                                <div style="text-align: center">
                                    <asp:ImageButton runat="server" ID="imbdamEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                        SkinID="imbeditgrid" OnClientClick="javascript:return DamageGridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                    <asp:ImageButton runat="server" ID="imbdamDel" ToolTip="<%$Resources:Controls,Delete %>"
                                        SkinID="imbdeletegrid" OnClientClick="javascript:return DamageGridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
