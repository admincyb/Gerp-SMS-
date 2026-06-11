<%@ Page Title="<%$ Resources:Captions,Title_ExternalMaterialReceive %>" Language="C#" ValidateRequest="false"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="ExternalMaterialReceive.aspx.cs"
    Theme="ClassicExt" EnableEventValidation="false" Inherits="ERPSMS_v01.StoreManagement.ExternalMaterialReceive" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/ExternalMaterialRecive.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.ExternalMaterialReceive%></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage('Draft');"
                EnableViewState="false" TabIndex="14" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="false" TabIndex="15" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                EnableViewState="false" TabIndex="16" />
            <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPagePrint" OnClientClick="javascript:return PrintPage();"
                EnableViewState="false" TabIndex="17" />
        </div>
    </div>--%>
    <asp:HiddenField ID="hdfWeightedAverage" runat="server" Value="0" />
    <asp:HiddenField ID="hdfSbuCurrency" runat="server" Value="0" />
    <asp:HiddenField ID="ConfirmStockValueChange" runat="server" Value="0" />
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
                            <%-- <li>
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnSaveandSubmit" runat="server" TabIndex="34" SkinID="btnInner-submit"
                                    Text="<%$Resources:Controls,Saveandsubmit%>" ToolTip="<%$Resources:Controls,Saveandsubmit %>"
                                    OnClientClick="javascript:return SavePage();" />
                            </li>
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
                                    TabIndex="34" />
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
        <div id="grdTable-wrap">
            <div id="DefaultMaterialMaster">
                <asp:HiddenField ID="MaterialDetailId" Value="0" runat="server" />
                <asp:HiddenField ID="ICH_TRX_TYPE" Value="2" runat="server" />
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
                <table class="table-devide" id="tblDetailHdr">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=MaterialReceiptNo%></label>
                                <asp:Label ID="lblMaterilaConsumptionNo" runat="server" CssClass="input-small" Text=""></asp:Label>
                                <asp:HiddenField ID="APT_CODE" runat="server" />
                                <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                                <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                <asp:HiddenField ID="transactionType" runat="server" Value="2" />
                                <label class="middle-lbl-small-b">
                                    <%=Resources.Controls.Date%>*</label>
                                <asp:TextBox ID="ICH_DATE" runat="server" onkeydown="return CheckKey(event)" onpaste="return false;"
                                    TabIndex="1" CssClass="input-small-a"></asp:TextBox>
                                <asp:HiddenField ID="ICH_NO" runat="server" />
                                <div class="clear">
                                </div>
                                <div id="divType">
                                    <label for="IssuingType" class="margnrgt3">
                                        <%=Resources.Controls.Type%>*</label>
                                    <asp:DropDownList ID="ICH_ISS_RCV_TYPE" runat="server" TabIndex="3" CssClass="select-w60-2per"
                                        EnableViewState="False" onchange="javascript:FillIssueToCategories();">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdn_ICH_ISS_RCV_TYPE" runat="server" Value="0" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <label for="ICH_REF_NO">
                                    <%=Resources.Controls.ReferenceNo%></label>
                                <asp:TextBox ID="ICH_REF_NO" runat="server" MaxLength="50" CssClass="input-w59-2per"
                                    onkeypress="return this.value.length<50" onpaste="return this.value.length<50"
                                    TabIndex="5"></asp:TextBox>
                                <%-- onkeypress="return this.value.length<500"
                                --%>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="Store">
                                    <%=ReceivingStore%>*</label>
                                <asp:DropDownList ID="ICH_DEPT" runat="server" TabIndex="2" CssClass="select-half"
                                    onchange="javascript:FillDepartement();" EnableViewState="False">
                                </asp:DropDownList>
                                <%--                                <div class="clear">
                                </div>--%>
                                <div class="clear">
                                </div>
                                <div id="divFrom">
                                    <label for="IssueTo" class="margnrgt3">
                                        <%=Resources.Controls.From%>*</label>
                                    <asp:DropDownList ID="ICH_ISS_RCV_PK" runat="server" TabIndex="4" CssClass="select-half"
                                        EnableViewState="False">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                </div>
                                <div style="display: none">
                                    <label for="ICH_ITEM_TYPE">
                                        <%=Resources.Controls.IssuingMaterialType%>*</label>
                                    <asp:DropDownList ID="ICH_ITEM_TYPE" runat="server" TabIndex="6" CssClass="select-half"
                                        EnableViewState="False" onchange="javascript:FillNewCategories();">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="ICH_ISS_RCV_NAME" runat="server" Value="" />
                                </div>
                                <div id="divSBUCompany">
                                    <label for="ICH_COMPANY">
                                        <%=GetGlobalResourceObject("Controls","CompanyPlant")%>*</label>
                                    <asp:DropDownList ID="ICH_COMPANY" runat="server" TabIndex="7" CssClass="select-half"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfSelCompany" runat="server" />
                                </div>
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
                <asp:HiddenField runat="server" ID="ICH_CRDR_NOTE_HDR" />
                <asp:HiddenField runat="server" ID="ICH_CRDR_FLAG" Value="0" />
                <%--For Identifying View mode or not.If 1=>ViewMode--%>
                <asp:HiddenField runat="server" ID="ConsumptionDtl" />
            </div>
            <div id="Order" class="gridwrap">
                <div id="ProductInsert">
                    <table id="tblProductInsert" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th width="18%" align="left">
                                    <%=Resources.Controls.Category%>*
                                </th>
                                <th width="21%" align="left">
                                    <b>
                                        <%=Resources.Controls.Code%>* </b>
                                </th>
                                <th width="10%" align="left">
                                    <b>
                                        <%=Resources.Controls.LotNo%>
                                    </b>
                                </th>
                                <th width="5%" style="text-align: right !important; display: none">
                                    <b>
                                        <%=Resources.Controls.Stock%>* </b>
                                </th>
                                <th width="8%" style="text-align: right !important">
                                    <b>
                                        <%=Resources.Controls.RcvdQty%>* </b>
                                </th>
                                <th width="5%" align="left">
                                    <b>
                                        <%=Resources.Controls.UOM%>* </b>
                                </th>
                                <th width="8%" style="text-align: right !important">
                                    <b>
                                        <%=GetGlobalResourceObject("Controls","RateCurrency")%>
                                        * </b>&nbsp;
                                </th>
                                <th width="5%" align="left">
                                    <b>
                                        <%=Resources.Controls.ExpiryDate%>
                                    </b>
                                </th>
                                <th width="15%" align="left">
                                    <b>
                                        <%=Resources.Controls.Comments%>
                                    </b>
                                </th>
                                <th width="3%" align="left">
                                    <%=Resources.Controls.Action%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="grd-rowhead">
                                <td width="18%">
                                    <%-- <asp:DropDownList ID="MaterialType" runat="server" Width="70%" TabIndex="7" onchange="javascript:FillCategoryDetails($(this).val());">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imbViewCag" runat="server" ToolTip="Open Item Category" SkinID="btnview"
                                        TabIndex="7" OnClientClick="javascript:return ShowCategory();" EnableViewState="false"
                                        CssClass="margntop2" />--%>
                                    <asp:TextBox ID="MaterialCategory" runat="server" Width="85%" TabIndex="7">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td width="21%">
                                    <%--  <asp:DropDownList ID="ITV_ITEM" runat="server" TabIndex="8" Width="98%" onchange="javascript:GetCurrentStock($(this).val());">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="Material" runat="server" Width="90%" TabIndex="8" AutoPostBack="false">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                    <asp:HiddenField ID="hdnICD_CRDR_NOTE_DTL" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td width="10%">
                                    <asp:TextBox ID="ICD_LOT_NO" runat="server" TabIndex="9" EnableViewState="false"
                                        Text="" CssClass="input-w95" MaxLength="12"></asp:TextBox>
                                </td>
                                <td style="text-align: right; display: none" width="5%">
                                    <asp:Label ID="CurrentStock" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right" width="8%">
                                    <asp:TextBox ID="ICD_QTY_CONSUMED" runat="server" TabIndex="10" EnableViewState="false"
                                        Text="" CssClass="numeric small-a" MaxLength="12">
                                    </asp:TextBox>
                                </td>
                                <td width="4%">
                                    <asp:DropDownList ID="ICD_UOM" runat="server" TabIndex="11" Width="80px" EnableViewState="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right" width="8%">
                                    <asp:TextBox ID="ICD_VALUE_CONSUMED" runat="server" TabIndex="12" EnableViewState="false"
                                        Text="" CssClass="numeric small-a" MaxLength="14">
                                    </asp:TextBox>
                                </td>
                                <%--<td width="6%">
                                    <asp:CheckBox ID="ICD_ISRETURN" runat="server" MaxLength="250" TabIndex="9" EnableViewState="false">
                                    </asp:CheckBox>
                                </td>--%>
                                <td width="5%">
                                    <asp:TextBox ID="ICD_EXPIRY_DATE" runat="server" Width="80px" onkeydown="return CheckKey(event)"
                                        onpaste="return false;" TabIndex="13" MaxLength="12" CssClass="date-picker floatLeft">
                                    </asp:TextBox>
                                </td>
                                <td width="15%">
                                    <asp:TextBox ID="ICD_REMARKS" runat="server" MaxLength="250" TabIndex="14" EnableViewState="false"
                                        CssClass="input-halfsmall-b">
                                    </asp:TextBox>
                                </td>
                                <td align="left" width="3%">
                                    <asp:ImageButton ID="imbAddNew" TabIndex="14" runat="server" SkinID="imbaddnew" OnClientClick="javascript:return AddRequisitionDetails();"
                                        Width="16px" />
                                    <asp:ImageButton ID="imbReset" TabIndex="14" runat="server" SkinID="cancel" OnClientClick="javascript:return ResetGridControlDetails();"
                                        Width="16px" ToolTip="<%$ resources:Reset %>" />
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
                ajaxurl="" editable="true" class="gridwraptable gridwrap" width="100%">
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
                        <th fieldmap="ICD_CRDR_NOTE_DTL" isvisible="false">
                        </th>
                        <th fieldmap="ICD_ITEM_CATEGORY" isvisible="false">
                            <%--MaterialTypePk--%>
                        </th>
                        <%-- <th fieldmap="ICD_ITEM_TYPE" isvisible="false">
                    </th>--%>
                        <th fieldmap="ICD_ITEM_CATEGORY_TEXT" align="left" width="18%">
                            <%--MaterialType --%>
                            <%=Resources.Controls.Category%>*
                        </th>
                        <th fieldmap="ICD_ITEM_TEXT" align="left" width="21%">
                            <%--MaterialCode--%>
                            <%=Resources.Controls.Code%>*
                        </th>
                        <th fieldmap="ICD_LOT_NO" align="left" width="10%">
                            <%--MaterialCode--%>
                            <%=Resources.Controls.LotNo%>*
                        </th>
                        <%--  <th fieldmap="ICD_CURRENT_STK" style="display:none;" width="5%">
                           
                            <%=Resources.Controls.Stock%>*
                        </th>--%>
                        <th fieldmap="ICD_QTY_CONSUMED" style="text-align: right !important" width="8%">
                            <%=Resources.Controls.RcvdQty%>*
                        </th>
                        <th fieldmap="ICD_UOM_TEXT" align="left" width="4%">
                            <%=Resources.Controls.UOM%>*
                        </th>
                        <th fieldmap="ICD_VALUE_CONSUMED" style="text-align: right !important" width="8%">
                            <%=GetGlobalResourceObject("Controls", "RateCurrency")%>&nbsp;
                        </th>
                        <%-- <th fieldmap="ICD_ISRETURNTEXT" align="left" width="15%">
                        <%=Resources.Controls.IsReturn%>
                    </th>--%>
                        <th fieldmap="ICD_EXPIRY_DATE" align="left" width="5%">
                            <%=Resources.Controls.ExpiryDate%>
                        </th>
                        <th fieldmap="ICD_REMARKS" align="left" width="13%">
                            <%=Resources.Controls.Comments%>
                        </th>
                        <th type="Template" width="5%" align="left">
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
        <div class="clear">
        </div>
        <%--  <div class="divcol-actiowrap" id="divAction" runat="server">--%>
        <%--</div>--%>
        <%--#####END Add user Controls and process Id, app ID#####--%>
        <asp:HiddenField runat="server" ID="EditRequisition" Value="0" />
        <asp:HiddenField runat="server" ID="hdnMaterialPk" Value="0" />
        <asp:HiddenField runat="server" ID="ICD_PK" Value="0" />
        <asp:HiddenField runat="server" ID="ActionID" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="LAST_MOD_DT" runat="server" />
        <asp:HiddenField ID="hdnSlNo" runat="server" Value="0" />
        <asp:HiddenField ID="ICH_IS_EDIT" runat="server" Value="0" />
        <asp:HiddenField ID="hdnShowSFGCategory" runat="server" Value="0" />
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
        <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
        <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />        
        <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
        <!-- Stores the Order Json Object Jquery Data Store -->
        <div id="divRequisitionData">
        </div>
        <div id="divCategory" title="<%=Resources.Captions.SelectCategory%>">
            <div id="treewrap" class="edittree">
                <div id="trvCategory" class="treeview-adj">
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
