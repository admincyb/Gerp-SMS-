<%@ Page Title="<%$ Resources:Captions,Title_SemiFinishedGoods %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="SemiFinishedGoods.aspx.cs"
    EnableEventValidation="false" Theme="ClassicExt" Inherits="ERPSMS_v01.Production.SemiFinishedGoods" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/DispersionPreparation.js.axd" type="text/javascript"></script>
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <%--<asp:Label runat="server" ID="lblBreadCrum"></asp:Label>--%>
                            <asp:Label runat="server" ID="lblbreadCrumNew"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button runat="server" ID="btnCancelSubmit" TabIndex="23" Text="<%$resources:Controls,CancelSubmit %>"
                                    ToolTip="<%$resources:Controls,CancelSubmit %>" SkinID="btnInner-submit" OnClientClick="javascript:return CancelDispersion();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSaveSubmit" TabIndex="24" Text="<%$resources:Controls,SaveSubmit %>"
                                    OnClientClick="javascript:return ShowWkfSubmitPopUp();" ToolTip="<%$resources:Controls,SaveSubmit %>"
                                    SkinID="btnInner-submit" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="21" Text="<%$resources:Controls,Submit %>"
                                    OnClientClick="javascript:return ShowWkfSubmitPopUp();" ToolTip="<%$resources:Controls,Submit %>"
                                    CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                            </li>
                            <li>
                                <asp:Button ID="btnSave" runat="server" TabIndex="25" ToolTip="<%$resources:Controls,Save %>"
                                    Text="<%$Resources:Controls,Save %>" SkinID="btnInner-Save" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" TabIndex="26" OnClientClick="javascript:return ResetPage();"
                                    ToolTip="<%$resources:Controls,Cancel %>" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel %>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="tab1Content">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label>
                                <%=Resources.Controls.SFGBatchNo%>
                            </label>
                            <asp:Label ID="DTH_BATCH_NO" runat="server" Text="" CssClass="input-medium"></asp:Label>
                            <div style="display: none">
                                <label>
                                    <%=Resources.Controls.Plan%>
                                    *</label>
                                <asp:TextBox ID="PLANNAME" runat="server" TabIndex="1"></asp:TextBox>
                                <asp:HiddenField ID="DTH_PLAN" runat="server" Value=""></asp:HiddenField>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <label>
                                <%=Resources.Controls.PrepareDate%>
                                *</label>
                            <asp:TextBox ID="DTH_DATE" CssClass="input-small" runat="server" EnableViewState="false"
                                TabIndex="1"></asp:TextBox>
                            <asp:HiddenField runat="server" Value="0" ID="DTH_PK" />
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
            <h1 class="search-colapse-normal">
                <%=Resources.Captions.SFGDetails%></h1>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label>
                                <%=Resources.Controls.BOMName%>*</label>
                            <asp:TextBox ID="DISPESION" runat="server" TabIndex="2" CssClass="input-half"></asp:TextBox>
                            <asp:HiddenField ID="DTH_DISPERSION" runat="server" Value="0"></asp:HiddenField>
                             <div class="clear">
                            </div>
                           
                           <label>
                                <%=Resources.Controls.Quantity%>
                                *</label>
                            <asp:TextBox ID="DTH_QUANTITY" runat="server" CssClass="input-small numeric" TabIndex="2" MaxLength="20"
                                onchange="javascript:BindMaterailGrid(true);" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                            <%-- onchange="javascript:CalcPercQuantity();--%>
                            <asp:DropDownList ID="DTH_QTY_UOM" runat="server" Width="20%" Enabled="false" TabIndex="2"
                                CssClass="select-small">
                            </asp:DropDownList>
                            <asp:Button ID="btnCalculate" runat="server" Text="<%$Resources:Controls,Calculate%>"
                                ToolTip="<%$Resources:Controls,CalculateDistribution%>" OnClientClick="javascript:BindMaterailGrid(true);return false;"
                                TabIndex="2" />    
                           
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                          <label>
                                <%=Resources.Controls.MaterialCategory%>
                            </label>
                            <asp:Label ID="lblBomCategory" runat="server" CssClass="input-half"></asp:Label>                           
                            <div class="clear">
                            </div>

                              <label>
                                <%=Resources.Controls.PreparationCost%>
                            </label>
                            <asp:TextBox ID="DTH_PREP_COST" runat="server" TabIndex="3" CssClass="input-small numeroc" MaxLength="20"
                                onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);">
                            </asp:TextBox>
                            <label class="lbl-20-5perc">
                                <%=Resources.Controls.PreparedBy%></label>
                            <asp:TextBox ID="DTH_PREPARED_BY" runat="server" TabIndex="4" CssClass="input-small" MaxLength="50"></asp:TextBox>                       
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div class="divcol-S">
                            <label class="margn-rgt0">
                                Remarks
                            </label>
                            <asp:TextBox ID="DTH_REMARKS" runat="server" TextMode="MultiLine" CssClass="multiline-2col margn-lft3"
                                TabIndex="5"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="divSaveData">
            </div>
            <div id="divData">
                <div class="gridwrap" id="divMaterialInsert">
                    <table id="MaterialInsert" rules="all" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th width="25%" align="left">
                                    <%=Resources.Controls.MaterialCategory%>*
                                </th>
                                <th width="40%" align="left">
                                    <%=Resources.Controls.Item%>*
                                </th>
                                <%--  <th width="24%" align="left">
                                    <%=Resources.Controls.BatchNo%>*
                                </th>--%>
                                <th width="10%" align="right">
                                    <%=Resources.Controls.CurrStock%>*
                                </th>
                                <th width="10%" style="text-align: right !important">
                                    <%=Resources.Controls.Quantity%>*
                                </th>
                                <th width="10%" align="left">
                                    <%=Resources.Controls.Unit%>*
                                </th>
                                <%-- <th width="10%" align="left">
                                    %
                                </th>
                                   New field Adding--%>
                               <%-- <th width="10%" style="text-align: right !important">
                                    <%=Resources.Controls.ActualTSC%>
                                </th>--%>
                                <%--    End New Field--%>
                                <th width="5%" align="left">
                                    <%=Resources.Controls.Action%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="20%" align="left">
                                    <%--<asp:DropDownList ID="MaterialCatagory" runat="server" TabIndex="11" CssClass="right-M"
                                        Width="90%" onChange="javascript:FillCategoryDetails($(this).val());">
                                    </asp:DropDownList>--%>
                                    <%--<asp:DropDownList runat="server" ID="MaterialCatagory" Width="95%" TabIndex="11"
                                        CssClass="right-M" onChange="javascript:FillMaterialNames($(this).val())">
                                        <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>                                        
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="MaterialCatagory"   Width="350px"  runat="server" TabIndex="11">
                                               </asp:TextBox>
                                             <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField> 
                                </td>
                                <td width="35%" align="left">
                                    <%--<asp:DropDownList ID="DSD_ITEM" TabIndex="12" runat="server" CssClass="right-M" Width="99%"
                                        onChange="return MaterialChangeEvent();">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox runat="server" ID="DSD_ITEM" Width="350px" TabIndex="12">
                                    </asp:TextBox> 
                                    <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>  
                                    
                                </td>
                                <td style="display: none;">
                                    <asp:Label ID="MaterialName" runat="server" Text=""></asp:Label>
                                </td>
                                <%--   <td width="25%">
                                    <%-- <asp:Label ID="MaterialName" runat="server" Text=""></asp:Label>
                                    <asp:DropDownList ID="BatchNo" TabIndex="12" runat="server" CssClass="right-M" Width="110%"
                                        onchange="javascript:FillBatchQuantity($(this).parent().parent());">
                                    </asp:DropDownList>
                                </td>--%>
                                <td width="10%" align="right">
                                    <asp:Label ID="ITM_CUR_STK" runat="server" Width="80%">  </asp:Label> 
                                </td>
                                <td width="10%" align="right">
                                    <asp:TextBox ID="DSD_QUANTITY" runat="server" CssClass="numeric input-w75" MaxLength="10"
                                        onkeyup="ValidateQuantity()" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                        TabIndex="12">
                                    </asp:TextBox>
                                </td>
                                <td width="10%" align="left">
                                    <%-- <asp:DropDownList ID="DSD_QTY_UOM" runat="server" Width="80px" CssClass="right-M"
                                        onChange="GetConversionFactor(null,null,ValidateQuantity)" TabIndex="12">
                                    </asp:DropDownList>--%>
                                    <asp:Label runat="server" ID="DSD_QTY_UOM_TEXT"></asp:Label>
                                    <asp:HiddenField ID="DSD_QTY_UOM" runat="server" Value="0" />
                                </td>
                                <%--align="left"--%>
                               <%-- <td width="10%" style="text-align: right" id="tdPercentage">
                                    <%--<span ></span>0
                                </td>--%>
                                <%--   New field Adding--%>
                                <%--<td width="10%" style="text-align: right !important;">
                                    <asp:TextBox ID="DTD_ACTUAL_TSC" runat="server" TabIndex="12" CssClass="input-w75">
                                    </asp:TextBox>
                                </td>--%>
                                <%--    End New Field--%>
                                <td width="5%" align="left">
                                    <div style="text-align: left;">
                                        <asp:ImageButton runat="server" ID="imbAddNew" SkinID="imbaddnew" OnClientClick="javascript:return AddDispersionMaterials();"
                                            TabIndex="12" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="divgrdDispersionDetailsGrid">
                    <%--class="dispertabl"--%>
                    <table rules="all" id="grdDispersionDetails" grandtype="GrandGrid" paging="false"
                        editfunction="GridAction" editable="true" width="100%" class="dispertabl gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th fieldmap="CONVERT_FACTOR" isvisible="false">
                                </th>
                                <th fieldmap="DSD_PK" isvisible="false">
                                </th>
                                <th fieldmap="DSD_DISP" isvisible="false">
                                </th>
                                <th fieldmap="DSD_ITEM" isvisible="false">
                                </th>
                                <th fieldmap="DSD_ITEM_TYPE" isvisible="false">
                                </th>
                                <th fieldmap="ITM_PHR" isvisible="false">
                                </th>
                                <th fieldmap="UOM_NAME" isvisible="false">
                                </th>
                                <th fieldmap="QTY_IN_STOCK" isvisible="false">
                                </th>
                                <th fieldmap="DSD_QTY_UOM" isvisible="false">
                                </th>
                                <%--  <th fieldmap="DSD_ITM_CARTEGORY" isvisible="false">
                                </th>--%>
                                <th fieldmap="DTD_ITEM_TYPE" isvisible="false" width="0%">
                                </th>
                                <%-- <th fieldmap="DSD_STK_DISP_BATCH" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="DSD_STK_BATCH" isvisible="false">
                                </th>--%>
                                <th fieldmap="SL_NO" isvisible="false">
                                </th>
                                <th fieldmap="DSD_ITEM_TYPE_TEXT" align="left" width="25%">
                                    <%=Resources.Controls.MaterialCategory%>
                                </th>
                                <th fieldmap="ITM_TEXT" align="left" width="40%">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <%--   <th fieldmap="DSD_BATCH_NO" align="left" width="24%">
                                    <%=Resources.Controls.BatchNo%>
                                </th>--%>
                                <%--<th fieldmap="IMG" align="left" width="6%">
                                </th>--%>
                                <%-- <th fieldmap="DTD_MULT_BTCH_GRP" isvisible="false">
                                </th>
                                <th fieldmap="DTD_IS_MULT_BATCH" isvisible="false">
                                </th>--%>
                                <th fieldmap="ITM_CUR_STK" align="right" width="10%">
                                    <%=Resources.Controls.CurrStock%>
                                </th>
                                <th fieldmap="DSD_QUANTITY" style="text-align: right !important;" width="10%">
                                    <%=Resources.Controls.Quantity%>
                                </th>
                                <th fieldmap="DSD_QUANTITY_TEMP" isvisible="false">
                                </th>
                                <th fieldmap="UOM_CODE" align="left" width="10%">
                                    <%=Resources.Controls.Unit%>
                                </th>
                                <th fieldmap="DSD_QTY_PERC" align="right" width="10%" isvisible="false">
                                    <%-- <%=Resources.Controls.Percentage%>--%>
                                    
                                </th>
                                <%--   New field Adding--%>
                                <%--<th fieldmap="DTD_ACTUAL_TSC" style="text-align: right !important" width="10%">
                                    <%=Resources.Controls.ActualTSC%>
                                </th>--%>
                                <%--    End New Field--%>
                                <th type="Template" width="5%" align="center" id="thAction">
                                    <div style="text-align: left;">
                                        <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$resources:Controls,Delete %>"
                                            TabIndex="12" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                        <asp:ImageButton runat="server" ID="imbCalculate" SkinID="formula" ToolTip="<%$resources:Calculate %>"
                                            TabIndex="12" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Calculate')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="clear">
                </div>
                <%-- //CheckList Region Start--------------------------------------%>
                <%--<div class="grdTable" id="divgrdChecklist">
                    <h1 class="search-colapse-normal">
                        <%=Resources.Captions.CheckList%>
                        <img id="imgCheckListShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                            alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                            cursor: pointer" onclick="javascript:ShowCheckList();" />
                        <img id="imgChecklistHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                            alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                            onclick="javascript:HideCheckList();" />
                    </h1>
                    <div id="divgrdCheckListInner">
                        <div class="fields-grpwrap color-grey pad-t10 grp-after">
                            <table>
                                <tr>
                                    <td>
                                        <h1>
                                            <%= GetLocalResourceObject("AddChecklistHeader").ToString()%></h1>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                            Style="margin-right: 0px;" TabIndex="13" />
                                        <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                            ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                            Style="margin-right: 0px;" TabIndex="13" />
                                        <asp:HiddenField ID="hdfShowHideFilter" runat="server" Value="0" ClientIDMode="Static" />
                                    </td>
                                </tr>
                            </table>
                            <table id="tblChklstAdd" class="margn-bot-20 table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <label for="LtCheckList">
                                                <asp:Literal ID="LtCheckList" runat="server" Text="<%$ resources:lblCheckList %>" /></label>
                                            <asp:TextBox ID="txtCheckList" runat="server" EnableTheming="False" TabIndex="14"
                                                CssClass="input-half"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrftxtCheckList" SetFocusOnError="true" ValidationGroup="Add"
                                                CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="txtCheckList"
                                                Text="*" ErrorMessage="<%$ resources:Msg_EnterCheckList %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <label for="ddlValueType">
                                                <asp:Literal ID="Literal4" runat="server" Text="<%$ resources:ChecklistType %>"></asp:Literal></label>
                                            <asp:DropDownList ID="ddlValueType" runat="server" TabIndex="15" CssClass="select-medium">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfUom" ControlToValidate="ddlValueType"
                                                Text="*" ToolTip="<%$ resources:Msg_EnterCheckListType %>" ErrorMessage="<%$ resources:Msg_EnterCheckListType %>"
                                                CssClass="star" Display="Dynamic" InitialValue="-1" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="rowValueTemplate" style="display: none;">
                                    <td>
                                        <div class="div2col-S">
                                            <label for="LtValue">
                                                <asp:Literal ID="lblValue" runat="server" Text="<%$ resources:lblValue %>" /></label>
                                            <span id="divValueInputContainer" style="background: none !important; border: none !important;">
                                            </span>
                                            <%--<asp:TextBox ID="txtValue" runat="server" EnableTheming="False" TabIndex="5"> </asp:TextBox> 
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <label for="lblRemarks">
                                                <asp:Literal ID="lblRemarks" runat="server" Text="<%$ resources:lblRemarks %>" /></label>
                                            <asp:TextBox ID="txtRemarks" runat="server" EnableTheming="False" TabIndex="17"> </asp:TextBox>
                                            <asp:ImageButton ID="imbAddTemplate" TabIndex="18" SkinID="imbaddnew" CommandName="ADD_ACTION"
                                                ToolTip="Add" runat="server" Width="16px" Height="16px" ValidationGroup="Add"
                                                OnClientClick="return addNewCheckListItem();" Style="padding-left: 10px !important" />
                                            <asp:HiddenField ID="hdfPK" runat="server" Value="0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                        </div>
                        <table rules="all" id="grdChecklistDetails" grandtype="GrandGrid" paging="false"
                            width="100%" class="dispertabl gridwrap filter-arrow">
                            <thead>
                                <tr>
                                    <th fieldmap="CDL_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="CDL_CHECK_LIST_DTL" isvisible="false">
                                    </th>
                                    <th fieldmap="CDL_SL_NO" isvisible="false">
                                    </th>
                                    <th fieldmap="CDL_ACTIVE" isvisible="false">
                                    </th>
                                    <th fieldmap="CDL_NAME" align="left">
                                        <%=Resources.Controls.ChecklistItem%>
                                    </th>
                                    <th fieldmap="CDT_VALUE_TYPE" align="left" isvisible="false">
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
                </div>--%>
                <%----------------------------  //End CheckList----------------------%>
                <div class="clear">
                </div>
            </div>
            <%--<table id="Table1" class="table-devide margntop-30">
                                            <tr>
                                                <td>
                                                    <div class="div2col-S">
                                                        <label>
                                                            <%=Resources.Controls.TimeLoaded%>
                                                            *</label>
                                                        <asp:TextBox ID="DTH_LOAD_DT" runat="server" TabIndex="20" Width="16%" CssClass="input-w14-5per"
                                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                        <asp:TextBox ID="DTH_LOAD_TM" runat="server" TabIndex="21" Width="10%" CssClass="input-w9-2per"
                                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnDate" runat="server" />
                                                        <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />
                                                        <asp:HiddenField ID="hdnFromDate" runat="server" />
                                                        <label class="middle-lbl">
                                                            <%=Resources.Controls.TimeUnloaded %>*</label>
                                                        <asp:TextBox ID="DTH_UNLOAD_DT" runat="server" TabIndex="22" Width="16%" CssClass="input-w14-5per"
                                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                        <asp:TextBox ID="DTH_UNLOAD_TM" runat="server" TabIndex="23" Width="10%" CssClass="input-w9-2per"
                                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnToDate" runat="server" />
                                                        <div class="clear">
                                                        </div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="div2col-S">
                                                        <label>
                                                            <%=Resources.Controls.HrsOfMilling %>
                                                            *</label>
                                                        <asp:TextBox ID="DTH_MILL_HRS" CssClass="input-small" runat="server" Enabled="false"></asp:TextBox>
                                                        <div class="clear">
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>--%>
            <div class="clear">
            </div>
            <%-- <div id="Wofkflowdiv">--%>
            <div id="divWkfSubmit" style="display: none;">
                <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
                <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
                <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
                <asp:HiddenField ID="AppNo" runat="server" />
                <asp:HiddenField runat="server" ID="ActionID" />
                <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
            </div>
            <%--   <asp:HiddenField ID="CheckListDtl" runat="server" />
                                        <asp:HiddenField ID="hdfDSPCHECKLISTHDR" runat="server" />--%>
            <div class="clear">
            </div>
            <%--<div id="divInspectionDetails" title="<%=Resources.Controls.InspectionList%>">
                                            <div class="content-wrapper">
                                                <table rules="all" id="grdInspectionDetails" grandtype="GrandGrid" paging="false"
                                                    class="gridwraptable gridwrap" width="100%">
                                                    <thead>
                                                        <tr>
                                                            <th fieldmap="TIH_PK" isvisible="false">
                                                            </th>
                                                            <th fieldmap="TIH_NO" width="40%">
                                                                <%=Resources.Controls.TestReport%>
                                                            </th>
                                                            <th fieldmap="TIH_DATE" width="25%">
                                                                <%=Resources.Controls.Date%>
                                                            </th>
                                                            <th fieldmap="TIH_TEST_TEXT" align="left" width="20%">
                                                                <%=Resources.Controls.Test%>
                                                            </th>
                                                            <th fieldmap="TIH_RESULT_TEXT" align="left" width="15%">
                                                                <%=Resources.Controls.Result%>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                        </div>--%>
            <%-- <div class="clear">
                                        </div>--%>
            <%--<div id="divRawMaterialInspection" title="<%=Resources.Controls.InspectionDetails%>">
                                            <div class="content-wrapper">
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <label>
                                                                    <%=Resources.Controls.Date%>
                                                                </label>
                                                                <asp:Label ID="lblRawDate" runat="server" CssClass="input-small"></asp:Label>
                                                                <%-- <label class="middle-lbl">
                                        <%=Resources.Controls.TestNo%>
                                    </label>
                                    <asp:Label ID="lblTestNo" runat="server" CssClass="input-medium"></asp:Label>
                                                               <label>
                                        <%=Resources.Controls.TestConductedAt%>
                                    </label>
                                    <asp:Label ID="lblTestConductedAt" runat="server" CssClass="input-medium"></asp:Label> --
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <%--    <label>
                                        <%=Resources.Controls.ReportRefNo%>
                                    </label>
                                    <asp:Label ID="lblTestReport" runat="server" CssClass="input-small-c"></asp:Label>
                                    <label class="middle-lbl">
                                        <%=Resources.Controls.DoneBy%>
                                    </label>
                                    <asp:Label ID="lblDoneBy" runat="server" CssClass="input-small-c"></asp:Label>--
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear">
                                                </div>
                                                <h3>
                                                    <%=Resources.Controls.MaterialDetails%></h3>
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <label>
                                                                    <%=Resources.Controls.BatchType%></label>
                                                                <asp:Label ID="lblBatchType" runat="server" CssClass="input-half"></asp:Label>
                                                                <div class="clear">
                                                                </div>
                                                                <label>
                                                                    <%=Resources.Controls.Material%></label>
                                                                <asp:Label ID="lblMaterial" runat="server" CssClass="input-half"></asp:Label>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <label>
                                                                    <%=Resources.Controls.LotBatch%></label>
                                                                <asp:Label ID="lblLotBatch" runat="server" CssClass="input-w71per"></asp:Label>
                                                                <div class="clear">
                                                                </div>
                                                                <label>
                                                                    <%=Resources.Controls.LotSizeQty%></label>
                                                                <asp:Label ID="lblLotSize" runat="server" CssClass="input-small"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear">
                                                </div>
                                                <h3>
                                                    <%=Resources.Controls.SamplingPlanDetails%></h3>
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <label>
                                                                    <%=Resources.Controls.Test%>
                                                                </label>
                                                                <asp:Label ID="lblTest" runat="server" CssClass="input-half"></asp:Label>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <label>
                                                                    <%=Resources.Controls.SampleTaken%></label>
                                                                <asp:Label ID="lblSampleTaken" runat="server" CssClass="input-small"></asp:Label>
                                                                <label class="middle-lbl-d">
                                                                    <%=Resources.Controls.SampleSize%></label>
                                                                <asp:Label ID="lblSampleSize" runat="server" CssClass="input-small-c"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="grdTable">
                                                    <table rules="all" id="grdRawMaterialInspection" grandtype="GrandGrid" paging="false"
                                                        width="100%" class="gridwraptable gridwrap">
                                                        <thead>
                                                            <tr>
                                                                <th fieldmap="TID_MIN_VALUE" isvisible="false">
                                                                </th>
                                                                <th fieldmap="TID_MAX_VALUE" isvisible="false">
                                                                </th>
                                                                <th fieldmap="TID_PARAMETER" align="left" width="45%">
                                                                    <%=Resources.Controls.Parameter%>
                                                                </th>
                                                                <th fieldmap="TID_UOM_TEXT" align="left" width="20%">
                                                                    <%=Resources.Controls.UOM%>
                                                                </th>
                                                                <th fieldmap="TID_MIN_MAX_VALUE" align="center" width="15%">
                                                                    <%=Resources.Controls.StandardValue%>
                                                                </th>
                                                                <th fieldmap="TID_VALUE" align="right" width="13%">
                                                                    <%=Resources.Controls.ObservedValue%>
                                                                </th>
                                                                <th fieldmap="TID_VARIANCE" align="right" width="7%">
                                                                    <%=Resources.Controls.Varience%>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </div>
                        </div>--%>
            <%--    <div class="clear">
                        </div>--%>
            <%--<div id="divPopupLatexBatches" title="Add Batch Details">
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
                                                <th class="grd-head-rgt" width="25%">
                                                    <%=Resources.Controls.Quantity%>
                                                </th>
                                                <th align="center" width="10%">
                                                    <%=Resources.Controls.Action%>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td align="left" width="40%">
                                                    <asp:DropDownList runat="server" ID="ddlLatexBatchesPopUp" Width="210px" TabIndex="201"
                                                        CssClass="right-M" onchange="javascript:FillPopUpBatchQuantity($(this).parent().parent());">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField runat="server" ID="hdfItmType" Value="" />
                                                    <asp:HiddenField runat="server" ID="hdfItmgroup" Value="" />
                                                    <asp:HiddenField runat="server" ID="hdfTempQty" Value="0" />
                                                </td>
                                                <td align="right" width="25%">
                                                    <asp:Label runat="server" ID="lblLatexStockPopUp" Text=""></asp:Label>
                                                </td>
                                                <td align="right" width="25%">
                                                    <asp:TextBox runat="server" ID="txtLatexQtyPopUp" CssClass="numeric input-w110" TabIndex="202"
                                                        autocomplete="off" MaxLength="14" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
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
                                                    <th fieldmap="SL_NO" isvisible="false">
                                                    </th>
                                                    <th fieldmap="SLNO" isvisible="false">
                                                    </th>
                                                    <th fieldmap="DSD_ITEM" isvisible="false">
                                                    </th>
                                                    <th fieldmap="DSD_ITEM_TYPE" isvisible="false">
                                                    </th>
                                                    <th fieldmap="DTD_ITEM_TYPE" isvisible="false">
                                                    </th>
                                                    <th fieldmap="ITM_TEXT" isvisible="false">
                                                    </th>
                                                    <th fieldmap="DSD_BATCH_NO" isvisible="false">
                                                    </th>
                                                    <th fieldmap="BATCH_NO_TEXT" align="left" width="40%">
                                                    </th>
                                                    <th fieldmap="DTD_MULT_BTCH_GRP" isvisible="false">
                                                    </th>
                                                    <th fieldmap="DTD_IS_MULT_BATCH" isvisible="false">
                                                    </th>
                                                    <th fieldmap="ITM_CUR_STK" align="right" width="25%">
                                                    </th>
                                                    <th fieldmap="DSD_QUANTITY" align="right" width="25%">
                                                    </th>
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
                                            <td style="width: 40%;">
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                <asp:Label ID="lblTotalBatchQtyCaption" runat="server" Text="<%$ resources:TotalBatchQty %>"></asp:Label>
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                <asp:Label runat="server" ID="lblTotalBatchQty" Text=""></asp:Label>
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                        </tr>
                                        <tr style="height: 25px;">
                                            <td style="width: 40%;">
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                <asp:Label runat="server" ID="lblQtyRequiredCaption" Text="<%$ resources:QtyRequired %>"
                                                    CssClass="numeric"></asp:Label>
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                <asp:Label runat="server" ID="lblQtyRequired" Text=""></asp:Label>
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                        </tr>
                                        <tr style="height: 25px;">
                                            <td style="width: 40%;">
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                <asp:Label runat="server" ID="lblQtyBalCaption" Text="<%$ resources:BalRequired %>"
                                                    CssClass="numeric"></asp:Label>
                                            </td>
                                            <td style="width: 25%; text-align: right;">
                                                <asp:Label runat="server" ID="lblQtyBal" Text=""></asp:Label>
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>--%>
        </div>
        <asp:HiddenField runat="server" ID="EditProduct" Value="0" />
        <asp:HiddenField ID="hdfSlNo" runat="server" Value="-1" />
        <asp:HiddenField runat="server" ID="hdfDeptID" Value="0" />
        <asp:HiddenField runat="server" ID="DTH_DEPT" Value="0" />
        <asp:HiddenField ID="hdfDispPK" runat="server" Value="0" />
        <asp:HiddenField ID="MaterialList" runat="server" />
        <asp:HiddenField ID="BizUnitPk" runat="server" Value="0" />
</asp:Content>
