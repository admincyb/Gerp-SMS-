<%@ Page Title="<%$ Resources:Captions,Title_SupplierEvaluationForm %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" EnableEventValidation="false"
    Theme="ClassicExt" CodeBehind="VendorEvaluation.aspx.cs" Inherits="ERPSMS_v01.VendorManagement.VendorEvaluation" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/VendorManagement/VendorEvaluation.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">--%>
    <%-- <h1>
            <%=Resources.Captions.SupplierReEvaluationForm%>
        </h1>--%>
    <%--/  CRITERIA  FOR  ACCEPTANCE--%>
    <%--<div class="button-wrap" style="width: auto; padding: 5px; margin: 0px;float: right;text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                TabIndex="12" />
            <asp:ImageButton ID="imbDraft" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage(1);"
                TabIndex="9" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                TabIndex="10" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="11" />
        </div>--%>
    <%--  </div>--%>
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
                                    TabIndex="34" EnableViewState="False" ToolTip="<%$resources:Controls,Submit %>"
                                    OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    ToolTip="<%$resources:Controls,Save %>" TabIndex="35" EnableViewState="False"
                                    OnClientClick="javascript:return SavePage(1);" />
                            </li>
                            <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="36"  ToolTip="<%$resources:Controls,Reset %>" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    ToolTip="<%$resources:Controls,Cancel %>" EnableViewState="False" OnClientClick="javascript:return CancelFun();"
                                    TabIndex="37" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div id="divVendorData">
            </div>
            <div id="divFileData">
            </div>
        </div>
    </div>
    <div class="content-wrapper">
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S">
                        <label for="VEH_VENDOR">
                            <%=Resources.Controls.SupplierName%>*</label>
                        <asp:DropDownList runat="server" ID="VEH_VENDOR" TabIndex="1" onchange="javascript:ClearEvaluationDtls();"
                            CssClass="select-w61per">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.SupplierCode%></label>
                        <span id="CodeSupplier" class="input-half"></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="div2col-S">
                        <label>
                            <%=Resources.Controls.Address%></label>
                        <span id="SupplierAddress" class="input-half" style="height:45px;"></span>
                    </div>
                </td>
                <td>
                    <div class="div2col-S">
                        <label for="VEH_ITEM">
                            <%=Resources.Controls.Product%>*</label>
                        <asp:DropDownList runat="server" ID="VEH_ITEM" TabIndex="2" onchange="javascript:ClearEvaluationDtls();"
                            CssClass="select-w61per">
                            <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                        </asp:DropDownList>
                        <div class="clear"></div>
                        <label for="VEH_COMPANY">
                            <%=Resources.Controls.Company%>*</label>
                        <asp:DropDownList ID="VEH_COMPANY" runat="server" TabIndex="3" ClientIDMode="Static"
                            CssClass="select-w61per">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdfSelCompany" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <div class="content-wrapper">
            <div id="tabs-1">
                <div id="ProductInsert" class="gridwrap">
                    <table id="ProductInsertTable" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <%-------------------------------------------//NewEvaluation Start-------------------------------------------------%>
                                <th width="30%" align="left">
                                    <b>
                                        <%=Resources.Controls.Group%>* </b>
                                </th>
                                <%-------------------------------------------//NewEvaluation End-------------------------------------------------%>
                                <th width="20%" align="left">
                                    <b>
                                        <%=Resources.Controls.Parameter%>* </b>
                                </th>
                                <th width="20%" align="left">
                                    <b>
                                        <%=Resources.Controls.Points%>* </b>
                                </th>
                                <th width="20%" align="left">
                                    <b>
                                        <%=Resources.Controls.Remarks%>
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
                                <%-------------------------------------------//NewEvaluation Start-------------------------------------------------%>
                                <td width="35%">
                                    <asp:DropDownList ID="VED_TERM_HDR" runat="server" Width="90%" CssClass="right-M"
                                        onchange="javascript:FillEvaluationParameters($(this).val());" TabIndex="3">
                                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <%-------------------------------------------//NewEvaluation End-------------------------------------------------%>
                                <td width="25%">
                                    <asp:DropDownList ID="VED_PARAM" runat="server" Width="88%" CssClass="right-M" TabIndex="3"
                                        onchange="javascript:FillEvaluationPoint($(this).val());">
                                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="IsEdit" runat="server" Value="false" />
                                </td>
                                <td width="10%">
                                    <asp:DropDownList ID="VED_POINT" runat="server" Width="88%" TabIndex="4">
                                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                                        </asp:ListItem>
                                        <%-- <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="VED_REMARKS" runat="server" Width="100%" CssClass="multiline-1col nomargin"
                                        TabIndex="5" MaxLength="200" TextMode="MultiLine" onkeypress="return (this.value.length<200)"
                                        onpaste="return this.value.length<200">
                                    </asp:TextBox>
                                </td>
                                <td width="5%">
                                    <div align="right">
                                        <asp:ImageButton ID="imbAddNew" runat="server" SkinID="imbaddnew" OnClientClick="javascript:return AddEvaluationDetails();"
                                            TabIndex="6" Width="16px" />
                                        <asp:HiddenField ID="hdpk" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="gridwrap" id="divgrdEvalDetails">
                    <table rules="all" id="grdEvalDetails" grandtype="GrandGrid" paging="false" editfunction="GridAction"
                        ajaxurl="" editable="true" width="100%" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="VED_TERM_HDR" isvisible="false">
                                </th>
                                <th fieldmap="VED_PK" isvisible="false">
                                </th>
                                <th fieldmap="EvaluationID" isvisible="false">
                                </th>
                                <th fieldmap="VED_PARAM" isvisible="false">
                                </th>
                                <th fieldmap="VED_TERM_HDR_TEXT" align="left" width="28%">
                                    <%=Resources.Controls.Group%>*
                                </th>
                                <th fieldmap="VED_PARAM_NAME" align="left" width="20%">
                                    <%=Resources.Controls.Parameter%>*
                                </th>
                                <th fieldmap="VED_POINT" align="left" width="15%">
                                    <%=Resources.Controls.Points%>*
                                </th>
                                <th fieldmap="VED_REMARKS" align="left" width="15%">
                                    <%=Resources.Controls.Remarks%>*
                                </th>
                                <th type="Template" width="5%">
                                    <div align="right">
                                        <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                            SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')"
                                            TabIndex="7" />
                                        <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')"
                                            TabIndex="8" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <%-- </div>--%>
                <div class="grd-2rows" style="display: none">
                    <%-- <div class="gridwraptable">
                        <%=Resources.Constants.VendorEvaluaionTable%>
                    </div>--%>
                    <div class="gridwraptable" style="display: none">
                        <%-- <table>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.MaxPoints%>
                                </td>
                                <td style="text-align: left">
                                    <span id="MaxPoint">- </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.PointsAwarded%>
                                </td>
                                <td style="text-align: left">
                                    <span id="PointAwarded">- </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.Percentage%>
                                </td>
                                <td style="text-align: left">
                                    <span id="Percentage">- </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.Overallratingbasis%>
                                </td>
                                <td style="text-align: left">
                                    <span id="Rating">- </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.Grade%>
                                </td>
                                <td style="text-align: left">
                                    <span id="SpnGrade">-</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.Name%>
                                </td>
                                <td style="text-align: left">
                                    <span id="SpnName">-</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <%=Resources.Captions.Date%>
                                </td>
                                <td style="text-align: left">
                                    <span id="SpnDate">-</span>
                                </td>
                            </tr>
                        </table>--%>
                    </div>
                    <%--  <div class="grdnotes">
                        <%=Resources.Captions.Notes%>
                        <div class="clear">
                        </div>
                        <b>
                            <%=Resources.Captions.Note1%>
                            <div class="clear">
                            </div>
                            <%=Resources.Captions.Note2%>
                            <div class="clear">
                            </div>
                            <%=Resources.Captions.Note3%>
                            <div class="clear">
                            </div>
                        </b>
                        <div class="clear">
                        </div>
                    </div>--%>
                </div>
                <div class="clear">
                </div>
                <div class="div2col-S" style="display: none">
                    <div id="divissuedby">
                        <label for="lblissuedby" style="margin-bottom: 5px">
                            <%=Resources.Controls.IssuedBy%></label>
                        <asp:Label ID="lblissuedby" runat="server"></asp:Label>
                        <asp:DropDownList ID="ddlIssuedBy" runat="server" Width="90%" Visible="false">
                        </asp:DropDownList>
                    </div>
                    <div id="divApprovedBy" style="display: none">
                        <label for="lblApprovedBy" style="margin-bottom: 5px">
                            <%=Resources.Controls.ApprovedBy%></label>
                        <asp:Label ID="lblApprovedBy" runat="server"></asp:Label>
                        <asp:DropDownList ID="ddlApprovedBy" runat="server" Width="90%" Visible="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="clear">
                </div>
                <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
                <div id="Wofkflowdiv" style="display: none">
                    <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
                    <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
                    <asp:HiddenField runat="server" ID="VendorPk" Value="-1" />
                    <asp:HiddenField runat="server" ID="hdfEvalPk" Value="0" />
                    <asp:HiddenField ID="ActionID" runat="server" />
                    <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
                </div>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="EvalDetailsList" />
        <asp:HiddenField runat="server" ID="EditEvaluation" Value="0" />
        <asp:HiddenField runat="server" ID="SupplierMaterialID" Value="0" />
        <asp:HiddenField runat="server" ID="DeptPk" Value="1" />
        <asp:HiddenField runat="server" ID="VED_PK" Value="0" />
        <asp:HiddenField runat="server" ID="VEH_PK" Value="0" />
        <asp:HiddenField runat="server" ID="VEH_POINT" Value="22" />
        <asp:HiddenField runat="server" ID="VEH_MAX_POINT" Value="30" />
        <asp:HiddenField runat="server" ID="VEH_PERC" Value="70" />
        <asp:HiddenField runat="server" ID="VEH_RAT_DESC" Value="Good" />
        <asp:HiddenField runat="server" ID="VND_PK" Value="0" />
        <asp:HiddenField runat="server" ID="VEH_ISSUED_BY" Value="0" />
        <asp:HiddenField runat="server" ID="VEH_APPROVED_BY" />
        <asp:HiddenField runat="server" ID="VEH_STATUS" Value="0" />
        <asp:HiddenField runat="server" ID="Readonly" Value="0" />
        <asp:HiddenField ID="TaskIDReg" runat="server" />
        <asp:HiddenField ID="TaskNameReg" runat="server" />
        <asp:HiddenField ID="ReferenceIDReg" runat="server" />
        <asp:HiddenField ID="ProcessIDReg" runat="server" />
        <asp:HiddenField ID="ApplicationIDReg" runat="server" />
        <asp:HiddenField ID="ActionIDReg" runat="server" />
        <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
        <!-- Stores the Order Json Object Jquery Data Store -->
        <div id="divEvalData">
        </div>
    </div>
</asp:Content>
