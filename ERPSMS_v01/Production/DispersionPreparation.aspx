<%@ Page Title="<%$ Resources:Captions,Title_DispersionPreparation %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="DispersionPreparation.aspx.cs"
    EnableEventValidation="false" Inherits="ERPSMS_v01.Production.DispersionPreparation"
    Theme="Classic" %> 

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/DispersionPreparation.js.axd" type="text/javascript"></script>
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
                           <asp:Button runat="server" ID="btnCancelSubmit"  TabIndex="23"
                                Text="<%$resources:ErpRes,CancelSubmit %>" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                 SkinID="btnInner-submit" OnClientClick="javascript:return CancelDispersion();"/>
                        </li>
                            <li>
                                <asp:Button runat="server" ID="btnSaveSubmit" TabIndex="24" Text="<%$resources:ErpRes,SaveSubmit %>"
                                    OnClientClick="javascript:return ShowWkfSubmitPopUp();" ToolTip="<%$resources:ErpRes,SaveSubmit %>"
                                    SkinID="btnInner-submit" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="21" Text="<%$resources:ErpRes,Submit %>"
                                    OnClientClick="javascript:return ShowWkfSubmitPopUp();" ToolTip="<%$resources:ErpRes,Submit %>"
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
                                <%=Resources.Controls.DispBatchNo%>
                            </label>
                            <asp:Label ID="DTH_BATCH_NO" runat="server" Text=""></asp:Label>
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
                            <asp:TextBox ID="DTH_DATE" CssClass="date-picker" runat="server" EnableViewState="false"
                                TabIndex="1"></asp:TextBox>
                            <asp:HiddenField runat="server" Value="0" ID="DTH_PK" />
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
            <h1 class="search-colapse-normal">
                <%=Resources.Captions.DispersionDetails%></h1>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label>
                                <%=Resources.Controls.Dispersion %>*</label>
                            <asp:TextBox ID="DISPESION" runat="server" TabIndex="2"></asp:TextBox>
                            <asp:HiddenField ID="DTH_DISPERSION" runat="server" Value="0"></asp:HiddenField>
                            <label>
                                <%=Resources.Controls.MachineUsed%>
                            </label>
                            <asp:DropDownList ID="DTH_MACHINE" runat="server" TabIndex="3">
                            </asp:DropDownList>
                             <label>
                                <%=Resources.Controls.PreparedBy%></label>
                            <asp:TextBox ID="DTH_PREPARED_BY" runat="server" TabIndex="2"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <label>
                                <%=Resources.Controls.DispersionType %>
                            </label>
                            <asp:Label ID="lblDispType" runat="server" Width="175px"></asp:Label>
                            <div class="clear">
                            </div>
                            <label>
                                <%=Resources.Controls.Quantity%>
                                *</label>
                            <asp:TextBox ID="DTH_QUANTITY" runat="server" CssClass="small-a numeric" TabIndex="4" onchange="javascript:BindMaterailGrid(true);"></asp:TextBox>
                           <%-- onchange="javascript:CalcPercQuantity();--%>
                            <asp:DropDownList ID="DTH_QTY_UOM" runat="server" Width="20%" Enabled="false" TabIndex="5">
                            </asp:DropDownList>
                            <label>
                            </label>
                            <asp:Button ID="btnCalculate" runat="server" Text="<%$Resources:Controls,CalculateDistribution%>"
                                OnClientClick="javascript:BindMaterailGrid(true);return false;" TabIndex="6" />
                                <%-- OnClientClick="javascript:CalcPercQuantity();return false;" TabIndex="6" />--%>
                            <div style="float: right">
                                <a href="#" id="lnkViewInspection" onclick="javascript:ViewInspctionDetails();">
                                </a>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="divData">
                <div class="gridwrap" id="divMaterialInsert">
                    <table id="MaterialInsert" rules="all" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th width="13%" align="left">
                                    <%=Resources.Controls.MaterialCategory%>*
                                </th>
                                <th width="21%" align="left">
                                    <%=Resources.Controls.Item%>*
                                </th>
                                <th width="20%" align="left">
                                    <%=Resources.Controls.BatchNo%>*
                                </th>
                                <th width="9%" align="right">
                                    <%=Resources.Controls.CurrStock%>*
                                </th>
                                <th width="8%" style="text-align: right !important">
                                    <%=Resources.Controls.Quantity%>*
                                </th>
                                <th width="10%" align="left">
                                    <%=Resources.Controls.Unit%>*
                                </th>
                                <th width="5%" align="left">
                                    %
                                </th>
                                <%--   New field Adding--%>
                                <th width="9%" style="text-align: right !important">
                                    <%=Resources.Controls.ActualTSC%>
                                </th>
                                <%--    End New Field--%>
                                <th width="5%" align="left">
                                    <%=Resources.Controls.Action%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="13%" align="left">
                                    <%--<asp:DropDownList ID="MaterialCatagory" runat="server" TabIndex="11" CssClass="right-M"
                                        Width="90%" onChange="javascript:FillCategoryDetails($(this).val());">
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList runat="server" ID="MaterialCatagory" Width="98%" TabIndex="11"
                                        CssClass="right-M" onChange="javascript:FillMaterialNames($(this).val())">
                                        <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:BindValues, RM%>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:BindValues, DSP%>" Value="2"></asp:ListItem>
                                        <%--<asp:ListItem Text="<%$ Resources:BindValues, Compound%>" Value="3"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td width="21%" align="left">                                    
                                    <asp:TextBox ID="DSD_ITEM_TEXT" TabIndex="12" runat="server" CssClass="right-M" Width="175px"></asp:TextBox>
                                    <asp:HiddenField ID="DSD_ITEM" runat="server" Value="0"/>
                                </td>
                                <td width="20%">
                                    <%-- <asp:Label ID="MaterialName" runat="server" Text=""></asp:Label>--%>
                                    <asp:DropDownList ID="BatchNo" TabIndex="12" runat="server" CssClass="right-M" Width="107%"
                                        onchange="javascript:FillBatchQuantity($(this).parent().parent());" ></asp:DropDownList>                                        
                                </td>
                                <td width="9%" align="right" id="ITM_CUR_STK">
                                    <%--<asp:Label ID="ITM_CUR_STK" runat="server" Width="80%">  </asp:Label> --%>
                                </td>
                                <td width="8%" style="text-align: right">
                                    <asp:TextBox ID="DSD_QUANTITY"  runat="server" CssClass="numeric input-w75" 
                                        MaxLength="10" onkeyup="ValidateQuantity()" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                        TabIndex="12" >
                                    </asp:TextBox>
                                </td>
                                <td width="10%" align="left">
                                    <asp:DropDownList ID="DSD_QTY_UOM" runat="server" Width="93%" CssClass="right-M"
                                        onChange="GetConversionFactor(null,null,ValidateQuantity)" TabIndex="12">
                                    </asp:DropDownList>
                                </td>
                                <%--align="left"--%>
                                <td width="5%" style="text-align: right" id="tdPercentage">
                                    <%--<span ></span>--%>0
                                </td>
                                <%--   New field Adding--%>
                                <td width="9%" style="text-align: right">
                                    <asp:TextBox ID="DTD_ACTUAL_TSC" runat="server" Width="98%" MaxLength="100" CssClass="numeric input-w75"
                                        TabIndex="12">
                                    </asp:TextBox>
                                </td>
                                <%--    End New Field--%>
                                <td width="5%" align="left">
                                    <div style="text-align: center;">
                                        <asp:ImageButton runat="server" ID="imbAddNew" SkinID="imbaddnew" OnClientClick="javascript:return AddDispersionMaterials();"
                                            TabIndex="12" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div  id="divgrdDispersionDetailsGrid"><%--class="dispertabl"--%>
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
                                <th fieldmap="DSD_STK_DISP_BATCH" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="DSD_STK_BATCH" isvisible="false">
                                </th>
                                <th fieldmap="SL_NO" isvisible="false">
                                </th>
                                <th fieldmap="DSD_ITEM_TYPE_TEXT" align="left" width="13%">
                                    <%=Resources.Controls.MaterialCategory%>
                                </th>
                                <th fieldmap="ITM_TEXT" align="left" width="24%">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <th fieldmap="DSD_BATCH_NO" align="left" width="15%">
                                    <%=Resources.Controls.BatchNo%>
                                </th>
                                <th fieldmap="ITM_CUR_STK" align="right" width="10%">
                                    <%=Resources.Controls.CurrStock%>
                                </th>
                                <th fieldmap="DSD_QUANTITY" style="text-align: right !important" width="9%">
                                    <%=Resources.Controls.Quantity%>
                                </th>
                                <th fieldmap="UOM_CODE" align="left" width="10%">
                                    <%=Resources.Controls.Unit%>
                                </th>
                                <th fieldmap="DSD_QTY_PERC" align="right" width="5%">
                                    <%-- <%=Resources.Controls.Percentage%>--%>
                                    %
                                </th>
                                <%--   New field Adding--%>
                                <th fieldmap="DTD_ACTUAL_TSC" style="text-align: right !important" width="9%">
                                    <%=Resources.Controls.ActualTSC%>
                                </th>
                                <%--    End New Field--%>
                                <th type="Template" width="5%" align="center" id="thAction">
                                    <div style="text-align: left;">
                                        <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$resources:ErpRes,Delete %>"
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
                <div class="grdTable" id="divgrdChecklist">
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
                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" style="margin-right:0px;"
                                        TabIndex="13" />
                                    <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                        ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" style="margin-right:0px;"
                                        TabIndex="13" />
                                    <asp:HiddenField ID="hdfShowHideFilter" runat="server" Value="0" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </table>
                        <table id="tblChklstAdd" class="margn-bot-20">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="LtCheckList">
                                            <asp:Literal ID="LtCheckList" runat="server" Text="<%$ resources:lblCheckList %>" /></label>
                                        <asp:TextBox ID="txtCheckList" runat="server" EnableTheming="False" TabIndex="14"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrftxtCheckList" SetFocusOnError="true" ValidationGroup="Add"
                                            CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="txtCheckList"
                                            Text="*" ErrorMessage="<%$ resources:Msg_EnterCheckList %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="ddlValueType">
                                            <asp:Literal ID="Literal4" runat="server" Text="<%$ resources:ChecklistType %>"></asp:Literal></label>
                                        <asp:DropDownList ID="ddlValueType" runat="server" TabIndex="15">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="vrfUom" ControlToValidate="ddlValueType"
                                            Text="*" ToolTip="<%$ resources:Msg_EnterCheckListType %>" ErrorMessage="<%$ resources:Msg_EnterCheckListType %>"
                                            CssClass="star" Display="Dynamic" InitialValue="-1" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr id="rowValueTemplate" style="display:none;">
                                <td>
                                    <div class="div2col-S">
                                        <label for="LtValue">
                                            <asp:Literal ID="lblValue" runat="server" Text="<%$ resources:lblValue %>" /></label>
                                            <span id="divValueInputContainer" style="background:none !important;border:none !important;">
                                            </span>
                                        <%--<asp:TextBox ID="txtValue" runat="server" EnableTheming="False" TabIndex="5"> </asp:TextBox>  --%>                                     
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
                </div>
                <%----------------------------  //End CheckList----------------------%>
                <div class="clear">
                </div>
            </div>
            <div class="div2col-S">
                <label>
                    <%=Resources.Controls.TimeLoaded%>
                    *</label>
                <asp:TextBox ID="DTH_LOAD_DT" runat="server" TabIndex="20" Width="16%" CssClass="date-picker"
                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                <asp:TextBox ID="DTH_LOAD_TM" runat="server" TabIndex="21" Width="10%" CssClass="date-picker"
                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                <asp:HiddenField ID="hdnDate" runat="server" />
                <asp:HiddenField ID="hdnSlNo" runat="server" Value="-1" />
                <asp:HiddenField ID="hdnFromDate" runat="server" />
                <div class="clear">
                </div>
                <label>
                    <%=Resources.Controls.TimeUnloaded %>*</label>
                <asp:TextBox ID="DTH_UNLOAD_DT" runat="server" TabIndex="22" Width="16%" CssClass="date-picker"
                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                <asp:TextBox ID="DTH_UNLOAD_TM" runat="server" TabIndex="23" Width="10%" CssClass="date-picker"
                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                <asp:HiddenField ID="hdnToDate" runat="server" />
            </div>
            <div class="div2col-S">
                <label>
                    <%=Resources.Controls.HrsOfMilling %>
                    *</label>
                <asp:TextBox ID="DTH_MILL_HRS" CssClass="small-a" runat="server" Enabled="false"></asp:TextBox>
            </div>
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
            <asp:HiddenField ID="MaterialList" runat="server" />
            <asp:HiddenField ID="CheckListDtl" runat="server" />
            <asp:HiddenField ID="hdfDSPCHECKLISTHDR" runat="server" />
            <div class="clear">
            </div>
            <div id="divInspectionDetails" title="<%=Resources.Controls.InspectionList%>">
                <div class="content-wrapper">
                    <table rules="all" id="grdInspectionDetails" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap"
                        width="100%">
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
            </div>
            <div class="clear">
            </div>
            <div id="divRawMaterialInspection" title="<%=Resources.Controls.InspectionDetails%>">
            <div class="content-wrapper">
                <div class="div2col-S">
                    <label>
                        <%=Resources.Controls.Date%>
                    </label>
                    <asp:Label ID="lblRawDate" runat="server"></asp:Label>
                    <div class="clear">
                    </div>
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
                        width="100%" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="TID_MIN_VALUE" isvisible="false">
                                </th>
                                <th fieldmap="TID_MAX_VALUE" isvisible="false">
                                </th>
                                <th fieldmap="TID_PARAMETER" align="left" width="45%" >
                                    <%=Resources.Controls.Parameter%>
                                </th>
                                <th fieldmap="TID_UOM_TEXT" align="left" width="20%" >
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
        </div>
    </div>
    <asp:HiddenField runat="server" ID="EditProduct" Value="0" />
    <asp:HiddenField ID="hdfSlNo" runat="server" Value="-1" />
    <asp:HiddenField runat="server" ID="hdfDeptID" Value="0" />
    <asp:HiddenField runat="server" ID="DTH_DEPT" Value="0" />
    <asp:HiddenField ID="hdfDispPK" runat="server" Value="0"/>
</asp:Content>
