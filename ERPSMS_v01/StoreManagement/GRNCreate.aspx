<%@ Page Title="<%$ Resources:Captions,Title_GRN %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="GRNCreate.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.GRNCreate" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/GRNCreate.js.axd" type="text/javascript"></script>
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
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" TabIndex="36" />
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
        <table class="table-devide" id="tblDetailHdr">
            <tr>
                <td>
                    <div class="div2col-S">
                        <label for="GRH_NO">
                            <%=Resources.Controls.GRNNo %></label>
                        <asp:Label ID="GRH_NO" runat="server" Text="" EnableViewState="false" CssClass="input-small"></asp:Label>
                        <asp:HiddenField ID="APT_CODE" runat="server" />
                        <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                        <asp:HiddenField ID="WKF_PROCESS" runat="server" Value="" />
                        <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                        <label for="GRH_DATE" class="middle-lbl-a0">
                            <%=Resources.Controls.GRNDate %>*</label>
                        <asp:TextBox ID="GRH_DATE" runat="server" EnableViewState="false" onkeydown="return CheckKey(event)"
                            onpaste="return false;" TabIndex="1" CssClass="input-small"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="GRH_VENDOR">
                            <%=Resources.Controls.Vendor%>*</label>
                        <asp:DropDownList ID="GRH_VENDOR" runat="server" TabIndex="3" CssClass="select-half"
                            onchange="javascript:AddNew();">
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                        <div id="divPlantCompany">
                            <label for="GRH_COMPANY">
                                <%=GetGlobalResourceObject("Controls","CompanyPlant")%>*</label>
                            <asp:DropDownList ID="GRH_COMPANY" runat="server" TabIndex="6" CssClass="select-half margnlft-minus4"
                                ClientIDMode="Static">
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <label for="GRH_DEPT">
                            <%=Resources.Controls.QuarantineArea%>
                            <%--Store--%>*</label>
                        <asp:DropDownList ID="GRH_DEPT" runat="server" TabIndex="2" onchange="javascript:BindGrid();"
                            CssClass="select-half">
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                        <label for="SPL_REFNO">
                            <%=Resources.Controls.SPL_REFNO%></label>
                        <asp:TextBox ID="GRH_VND_REF_NO" runat="server" MaxLength="50" onkeypress="return this.value.length<50"
                            onpaste="return this.value.length<50" EnableViewState="false" TabIndex="4" CssClass="input-small" ></asp:TextBox>
                        <label for="SPL_DATE" class="middle-lbl-a0">
                            <%=Resources.Controls.REF_DATE%></label>
                        <asp:TextBox ID="GRH_VND_REF_DATE" runat="server" EnableViewState="false" onkeydown="return CheckKey(event)"
                            onpaste="return false;" TabIndex="5" CssClass="input-small"></asp:TextBox>
                        <asp:HiddenField ID="GRH_PK" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="GRH_STATUS" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="hdfDeptID" runat="server" EnableViewState="false" Value="0" />
                        <asp:HiddenField ID="GRH_IS_EDIT" runat="server" Value="0" />
                    </div>
                </td>
            </tr>
        </table>
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.PendingPurchaseOrderItems%>
            <img id="imgPendingOrderShow" src="../Images/Classic/Icons/arrow-colapse-active.png"
                alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                cursor: pointer" onclick="javascript:ShowPendingOrder();" />
            <img id="imgPendingOrderHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                onclick="javascript:HidePendingOrder();" />
        </h1>
        <div id="divPendingOrders">
            <div id="searchwrap" class="search-wrap-custom1">
                <label>
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" TabIndex="6" runat="server" CssClass="medium"
                    onchange="javascript:SetSearchType(true);" EnableViewState="false">
                    <asp:ListItem Value="POH_NO" Text="<%$ Resources:BindValues, PONumber%>">
                    </asp:ListItem>
                    <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, Item%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="Date Range">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="7">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="8" EnableViewState="false" CssClass="input-small">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="9" EnableViewState="false" CssClass="input-small">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="10" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
                <div class="clear">
                </div>
            </div>
            <div class="scroll-h150">
                <div id="divPendingPOList" class="gridwrap nomargin">
                    <table rules="all" id="grdPOList" grandtype="GrandGrid" paging="false" enablecheckbox="true"
                        width="100%" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="POH_PK" isvisible="false">
                                </th>
                                <th fieldmap="POD_PK" isvisible="false">
                                </th>
                                <th fieldmap="POD_ITEM" isvisible="false">
                                </th>
                                <th fieldmap="POD_UOM" isvisible="false">
                                </th>
                                <th fieldmap="POD_RATE" isvisible="false">
                                </th>
                                <th fieldmap="POD_GRN_FLAG" align="left" width="3%">
                                    <%=Resources.Controls.Added%>
                                </th>
                                <th fieldmap="POH_NO" align="left" width="10%">
                                    <%=Resources.Controls.PoNumber%>
                                </th>
                                <th fieldmap="ITM_NAME" align="left" width="52%">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <th fieldmap="UOM_CODE" align="left" width="5%">
                                    <%=Resources.Controls.UOM%>
                                </th>
                                <th fieldmap="POD_QTY_APPROVED" align="right" width="10%">
                                    <%=Resources.Controls.QtyOrdered%>
                                </th>
                                <th fieldmap="POD_QTY_RECEIVED" align="right" width="10%">
                                    <%=Resources.Controls.PrevRcvdQty%>
                                </th>
                                <th fieldmap="BALANCE_QTY" align="right" width="10%">
                                    <%=Resources.Controls.BalToReceive%>
                                </th>
                                <th fieldmap="POH_COMPANY" isvisible="false">
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="button-wrap-right">
                <asp:Button ID="AddToList" runat="server" TabIndex="11" Text="<%$ Resources:Controls, AddToList%>"
                    OnClientClick="javascript:return AddToList();" />
            </div>
            <div class="clear">
            </div>
        </div>
        <h1 class="search-colapse-normal">
            <%=Resources.Captions.GRNList%>
            <img id="imgGrnListShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Show%>"
                title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                onclick="javascript:ShowGrnList();" />
            <img id="imgGrnListHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                onclick="javascript:HideGrnList();" />
        </h1>
        <div id="divGrnList">
            <div class="gridwrap">
                <div id="divPendingPOInsert">
                    <table rules="all" id="grdPendingPOList" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                        editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="POH_PK" isvisible="false">
                                </th>
                                <th fieldmap="GRD_PK" isvisible="false">
                                </th>
                                <th fieldmap="GRD_PO" isvisible="false">
                                </th>
                                <th fieldmap="GRD_ITEM" isvisible="false">
                                </th>
                                <th fieldmap="GRD_UOM" isvisible="false">
                                </th>
                                <th fieldmap="ORG_BALANCE_QTY" isvisible="false">
                                </th>
                                <th fieldmap="POH_NO" align="left" width="10%">
                                    <%=Resources.Controls.PoNumber%>
                                </th>
                                <th fieldmap="ITM_NAME" align="left" width="25%">
                                    <%=Resources.Controls.Item%>
                                </th>
                                <th fieldmap="UOM_CODE" align="left" width="2%">
                                    <%=Resources.Controls.UOM%>
                                </th>
                                <th fieldmap="POD_QTY_APPROVED" align="right" width="8%">
                                    <%=Resources.Controls.OrdQty%>
                                </th>
                                <th fieldmap="POD_QTY_RECEIVED" align="right" width="8%">
                                    <%=Resources.Controls.RcvdQty%>
                                </th>
                                <th fieldmap="BALANCE_QTY" align="right" width="8%">
                                    <%=Resources.Controls.BalQty%>
                                </th>
                                <th fieldmap="GRD_QTY_RECEIVED" align="right" width="7%">
                                    <%=Resources.Controls.RcvdNow%>
                                </th>
                                <%--------  //Adding Lot no/Batchno -----------------------------%>
                                <th fieldmap="GRD_VND_REF_NO" align="left" width="8%">
                                    <%=Resources.Controls.LotBatch%>
                                </th>
                                <%-- ---------End----------------------%>
                                <th fieldmap="GRD_DOM" align="left" width="7%">
                                    <%=Resources.Controls.DOM%>
                                </th>
                                <th fieldmap="GRD_DOE" align="left" width="7%">
                                    <%=Resources.Controls.DOE%>
                                </th>
                                <th fieldmap="GRD_REMARKS" align="left" width="8%">
                                    <%=Resources.Controls.Remarks%>
                                </th>
                                <th type="Template" width="2%">
                                    <div>
                                        <%-- <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />--%>
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$ resources:Controls,Delete %>" TabIndex="11"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <br class="clear" />
            </div>
        </div>
        <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
        <div id="Wofkflowdiv" style="display: none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField ID="AppNo" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="ActionID" />
            <asp:HiddenField runat="server" ID="orderPercentage" Value="5" />
            <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="addlGrnQty" runat="server" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="hfPOPrint" runat="server" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSBUCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrentDate" runat="server" />
        </div>
        <%--#####END Add user Controls and process Id, app ID#####--%>
        <%------------ FileUploader Region Start-------------------------%>
        <h1 class="search-colapse-normal">
            <%=Resources.Controls.Attachments%>
            <img id="imgShowAttachment" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none;
                cursor: pointer" onclick="javascript:ShowAttachment();" />
            <img id="imgHideAttachment" src="../Images/Classic/Icons/arrow-colapse-active.png"
                alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                onclick="javascript:HideAttachment();" />
        </h1>
        <div class="divcol-FileuplWrap" id="divAttachment">
            <label for="aupDocument">
                <%=Resources.Controls.Attachments%></label>
            <div id="FileUploader" class="input-file">
                <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="29" Height="22px"
                    Style="margin-top: 3px" TabIndex="12" />
                <asp:HiddenField ID="FILELIST" runat="server" />
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="divFileData">
        </div>
        <%--End FileUploader Region---------------------%>
        <div class="clear">
        </div>
        <div id="divPreviousGRN" class="grdTable" title="<%=Resources.Captions.PreviousGRNDetails%>">
            <table rules="all" id="grdPreviousGRN" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="GRD_NO" align="left" width="11%">
                            <%=Resources.Controls.GRNNo%>
                        </th>
                        <th fieldmap="GRD_DATE" align="left" width="15%">
                            <%=Resources.Controls.GRNDate%>
                        </th>
                        <th fieldmap="POH_NO" align="left" width="15%">
                            <%=Resources.Controls.PoNumber%>
                        </th>
                        <th fieldmap="ITM_NAME" align="left" width="20%">
                            <%=Resources.Controls.Item%>
                        </th>
                        <th fieldmap="GRD_QTY_RECEIVED" align="right" width="12%">
                            <%=Resources.Controls.QtyRecieved%>
                        </th>
                        <th fieldmap="GRD_QTY_REJECTED" align="right" width="12%">
                            <%=Resources.Controls.QunatityRejected%>
                        </th>
                        <th fieldmap="UOM_CODE" align="left" width="10%">
                            <%=Resources.Controls.UOM%>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divPODetails" class=" contentwrapper" title="<%=Resources.Captions.PODetails%>">
            <div class="content-wrapper">
                <table rules="all" id="grdPODetails" grandtype="GrandGrid" paging="false" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="POD_NO" align="left" width="15%">
                                <%=Resources.Controls.PoNumber%>
                            </th>
                            <th fieldmap="POD_DATE" align="left" width="15%">
                                <%=Resources.Controls.Date%>
                            </th>
                            <th fieldmap="VEN_NAME" align="left" width="25%">
                                <%=Resources.Controls.Vendor%>
                            </th>
                            <th fieldmap="ITM_NAME" align="left" width="25%">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="POD_QTY_APPROVED" align="right" width="10%">
                                <%=Resources.Controls.QtyOrd%>
                            </th>
                            <th fieldmap="UOM_CODE" align="left" width="10%">
                                <%=Resources.Controls.UOM%>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="GRNList" runat="server" />
        <asp:HiddenField ID="IsPrefID" runat="server" Value="0" />
        <asp:HiddenField ID="PO_PK" runat="server" Value="0" />
        <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
        <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
        <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <div id="divData">
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
