<%@ Page Title="<%$ Resources:Captions,Title_AgentManagement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="AgentMaster.aspx.cs"
    Inherits="ERPSMS_v01.VendorManagement.AgentMaster" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
    <script src="../Scripts/PageScript/VendorManagement/AgentRegistration.js.axd" type="text/javascript"></script>
    <script src="../Scripts/GrandTreeMulti.js" type="text/javascript"></script>
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
                                    TabIndex="100" EnableViewState="False" ToolTip="<%$resources:Controls,Submit %>"
                                    OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="101" EnableViewState="False" ToolTip="<%$resources:Controls,Save %>"
                                    OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" ToolTip="<%$resources:Controls,Cancel %>" OnClientClick="javascript:return CancelFun();"
                                    TabIndex="103" />
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
        <asp:HiddenField runat="server" ID="VendorDetails" />
        <div id="FormData">
            <asp:HiddenField runat="server" ID="BizUnitPk" />
            <asp:HiddenField runat="server" ID="UserID" />
            <asp:HiddenField runat="server" ID="AddressBookDetails" />
            <asp:HiddenField runat="server" ID="MaterialDetails" />
            <asp:HiddenField runat="server" ID="TermsDetails" />
            <%-----------------------------------------/New Start -----------------%>
            <asp:HiddenField runat="server" ID="MaterialSamples" />
            <asp:HiddenField runat="server" ID="RateHistoryDetails" />
            <%-----------------------------------------/New End -----------------%>
            <asp:HiddenField runat="server" ID="VEN_PK" Value="0" />
            <asp:HiddenField runat="server" ID="ViewStatus" Value="0" />
            <asp:HiddenField ID="ActionID" runat="server" />
            <asp:HiddenField ID="AutoStartValue" runat="server" />
            <asp:HiddenField runat="server" ID="TaxHdr" />
            <asp:HiddenField runat="server" ID="hdfViewMode" />
            <asp:HiddenField runat="server" ID="VRM_ROLE" />
            <asp:HiddenField runat="server" ID="LAST_MOD_DATE" />
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S vendormaster">
                            <label for="VEN_CODE">
                                <%=Resources.Controls.AgentCode%>
                                *</label>
                            <asp:TextBox runat="server" ID="VEN_CODE" onkeypress="return this.value.length<100"
                                onpaste="return this.value.length<100" MaxLength="100" TabIndex="1" CssClass="input-half"></asp:TextBox>
                            <div style="display: none">
                                <label for="VEN_REG_NO">
                                    <%=Resources.Controls.BRNo%></label>
                                <asp:TextBox runat="server" ID="VEN_REG_NO" TabIndex="3" onkeypress="return this.value.length<200"
                                    MaxLength="200" onpaste="return this.value.length<200"></asp:TextBox>
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_CONT_NAME">
                                <%=Resources.Controls.ContactName%></label>
                            <asp:TextBox runat="server" ID="VEN_CONT_NAME" onkeypress="return this.value.length<200"
                                MaxLength="200" onpaste="return this.value.length<200" TabIndex="3" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_PHONE">
                                <%=Resources.Controls.Phone%></label>
                            <asp:TextBox runat="server" ID="VEN_PHONE" onkeypress="return this.value.length<30"
                                onpaste="return this.value.length<30" MaxLength="30" TabIndex="5" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_EMAIL">
                                <%=Resources.Controls.Email%></label>
                            <asp:TextBox runat="server" ID="VEN_EMAIL" onkeypress="return this.value.length<100"
                                onpaste="return this.value.length<100" MaxLength="100" TabIndex="7" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_WEBSITE">
                                <%=Resources.Controls.Website%></label>
                            <asp:TextBox runat="server" ID="VEN_WEBSITE" onkeypress="return this.value.length<100"
                                onpaste="return this.value.length<100" MaxLength="100" TabIndex="9" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_MOBIL">
                                <%=Resources.Controls.Mobile%></label>
                            <asp:TextBox runat="server" ID="VEN_MOBIL" onkeypress="return this.value.length<50"
                                onpaste="return this.value.length<50" MaxLength="50" TabIndex="11" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_FAX">
                                <%=Resources.Controls.Fax%></label>
                            <asp:TextBox runat="server" ID="VEN_FAX" onkeypress="return this.value.length<50"
                                onpaste="return this.value.length<50" MaxLength="50" TabIndex="13" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <div id="venCurr" runat="server" class="display-inline">
                                <label for="VEN_CURRENCY">
                                    <%=Resources.Controls.Currency%>*</label>
                                    <asp:DropDownList runat="server" ID="VEN_CURRENCY" TabIndex="15" onchange="ChangeCurrency();"
                                       CssClass="select-half-a margnlft-minus4">
                                    </asp:DropDownList>
                            </div>
                            <div id="venCurrText" style="display: none">
                                <label for="VEN_CURRENCY_TEXT">
                                    <%=Resources.Controls.Currency%>*</label>
                                <asp:Label ID="VEN_CURRENCY_TEXT" runat="server" CssClass="margnlft-minus4" ></asp:Label>
                                <%--  <span id="VEN_CURRENCY_TEXT"></span>--%>
                            </div>
                            <div style="display: none">
                                <label for="VEN_TYPE">
                                    <%=Resources.Controls.ApprovalStatus%>
                                    *</label>
                                <asp:DropDownList runat="server" ID="VEN_TYPE" TabIndex="15" CssClass="input-half">
                                    <asp:ListItem Value="1" Text="Approved"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Non Approved" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <%-- Shamnad --%>
                            <div class="clear">
                            </div>
                            <label for="VEN_CREDIT_DAYS">
                                <%=Resources.Controls.CreditDays%></label>
                            <asp:TextBox runat="server" ID="VEN_CREDIT_DAYS" MaxLength="3" TabIndex="17" CssClass="input-half numeric"
                                onkeypress="return isNumber(event)" onpaste="return false"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="TaxByVendor">
                                <%=Resources.Controls.Taxes%></label>
                            <asp:Label runat="server" ID="TaxByVendor" CssClass="input-half"></asp:Label>
                            <asp:ImageButton runat="server" ID="imgVendorTax" TabIndex="19" src="../Images/Classic/Icons/tax.png"
                                purpose="VendorTax" OnClientClick="javascript:AddVendorItemTax('1','21416'); return false;"
                                alt="Taxes" title="Taxes" Style="cursor: pointer" />
                            <%--<asp:ImageButton runat="server" ID="imgVendorTax" TabIndex="18" onclick="javascript:AddVendorItemTax('1','21416');" 
                            src="../Images/Classic/Icons/tax.png" alt="Taxes" title="Taxes" style="cursor: pointer" />--%>
                            <%--<img id="imgVendorTax" TabIndex="18" onclick="javascript:AddVendorItemTax('1','21416');" src="../Images/Classic/Icons/tax.png"
                                alt="Taxes" title="Taxes" style="cursor: pointer" purpose="VendorTax" />--%>
                            <%--<img id="imgVendorTax" TabIndex="18" onclick="javascript:AddVendorItemTax('1','21416');" src="../Images/Classic/Icons/tax.png"
                                alt="Taxes" title="Taxes" style="cursor: pointer" purpose="VendorTax" />--%>
                            <div class="clear">
                            </div>
                            <div id="divTaxddl" runat="server" class="display-inline">
                                <label for="VEN_WHT_TAX">
                                    <%=Resources.Controls.WHTAccounts%></label>
                                <asp:DropDownList runat="server" ID="VEN_WHT_TAX" TabIndex="21" CssClass="select-half-a margnlft-minus4">
                                </asp:DropDownList>
                                <label for="VEN_PAY_FOR_VENDOR" class="margn-rgt2">
                                    <%=Resources.Controls.Pay_Form_Agent%>
                                </label>
                                <asp:HiddenField runat="server" ID="VEN_PAY_FOR_VENDOR" Value="false" />
                                <asp:CheckBox runat="server" ID="VEN_PAY_FOR_VENDOR1" Checked="false" TabIndex="22" />
                            </div>                          
                            <div id="divTaxText" runat="server" style="display: none" class="display-inline">
                                <label for="VEN_WHT_TAX_TEXT">
                                    <%=Resources.Controls.WHTAccounts%></label>
                                <asp:Label ID="VEN_WHT_TAX_TEXT" Width="150px" runat="server" CssClass="margnlft-minus4"></asp:Label>
                                <label for="VEN_PAY_FOR_VENDOR_TEXT" class="width-auto">
                                    <%=Resources.Controls.Pay_Form_Agent%>
                                </label>
                                <asp:CheckBox runat="server" ID="VEN_PAY_FOR_VENDOR_TEXT" Checked="false" TabIndex="20" />
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_NAME2">
                                <%=Resources.Controls.LocalName%></label>
                            <asp:TextBox runat="server" ID="VEN_NAME2" onkeypress="return this.value.length<50"
                                onpaste="return this.value.length<50" MaxLength="50" TabIndex="24" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VNC_TYPE">
                                <%=Resources.Controls.Type%></label>
                            <asp:DropDownList ID="VNC_TYPE" runat="server" TabIndex="25" CssClass="lbl-61-2perc">
                            </asp:DropDownList>
                            <span id="VNC_TYPE_TEXT"></span>
                            <div class="clear">
                            </div>
                            <%-- Shamnad --%>
                            <%--    18_08_2014          --%>
                            <label for="VEN_ACTIVE">
                                <%=Resources.Controls.VendorActive%>
                            </label>
                            <asp:HiddenField runat="server" ID="VEN_ACTIVE" Value="true" />
                            <asp:CheckBox runat="server" ID="VEN_ACTIVE1" Checked="true" TabIndex="27" />
                            <%--     end                       --%>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S vendormaster" style="float: right; margin-right: 0">
                            <label for="VEN_NAME">
                                <%=Resources.Controls.AgentName%>*</label>
                            <asp:TextBox runat="server" ID="VEN_NAME" onkeypress="return this.value.length<200"
                                MaxLength="200" onpaste="return this.value.length<200" TabIndex="2" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_TIN">
                                <%=Resources.Controls.TaxID%></label>
                            <asp:TextBox runat="server" ID="VEN_TIN" onkeypress="return this.value.length<100"
                                onpaste="return this.value.length<100" MaxLength="100" TabIndex="4" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_GST_NO_MOD_DT">
                                <%=Resources.Controls.GSTLastChangeDate%></label>
                            <asp:TextBox ID="VEN_GST_NO_MOD_DT" runat="server" Text="" onkeydown="return CheckKey(event)"
                                onpaste="return false;" TabIndex="6" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_ADDR1">
                                <%=Resources.Controls.Address1%></label>
                                <asp:TextBox runat="server" ID="VEN_ADDR1" onkeypress="return this.value.length<200"
                                    MaxLength="200" onpaste="return this.value.length<200" TabIndex="8" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_ADDR2">
                                <%=Resources.Controls.Address2%></label>
                            <asp:TextBox runat="server" ID="VEN_ADDR2" TabIndex="10" onkeypress="return this.value.length<200"
                                MaxLength="200" onpaste="return this.value.length<200" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_CITY">
                                <%=Resources.Controls.City%></label>
                            <asp:TextBox runat="server" ID="VEN_CITY" TabIndex="12" MaxLength="40" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_CNTRY">
                                <%=Resources.Controls.Country%></label>
                            <asp:DropDownList runat="server" ID="VEN_CNTRY" onchange="FillState('VENDOR',false,$(this).val())"
                                TabIndex="14" CssClass="lbl-61-2perc">
                            </asp:DropDownList>
                            <span id="VEN_CNTRY_TEXT"></span>
                            <div class="clear">
                            </div>
                            <label for="VEN_STATE">
                                <%=Resources.Controls.State%></label>
                            <asp:DropDownList runat="server" ID="VEN_STATE" TabIndex="16" CssClass="lbl-61-2perc">
                            </asp:DropDownList>
                            <span id="VEN_STATE_TEXT"></span>
                            <div class="clear">
                            </div>
                            <label for="VEN_PIN">
                                <%=Resources.Controls.Zip%></label>
                            <asp:TextBox runat="server" ID="VEN_PIN" onkeypress="return this.value.length<30"
                                onpaste="return this.value.length<30" MaxLength="30" TabIndex="18" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                           <div id="divAccountddl">
                           <label for="VEN_ACCOUNT">
                                    <%=Resources.Controls.Account%></label><asp:DropDownList runat="server" ID="VEN_ACCOUNT" CssClass="select-half-a"
                                        TabIndex="18">
                            </asp:DropDownList>
                            </div>
                             <div id="divAccountText" style="display: none">
                                <label for="VEN_ACCOUNT_TEXT">
                                    <%=Resources.Controls.Account%></label><span id="VEN_ACCOUNT_TEXT"></span>
                            </div>                           
                             <div class="clear">
                                </div>                
                            <div id="divTypeddl" style="display: none">
                                <label for="Type">
                                    <%=Resources.Controls.Type%>*</label><asp:DropDownList runat="server" ID="VEN_PO_TYPE" CssClass="select-half-a"
                                        TabIndex="20">
                                        <asp:ListItem Text="Local" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divTypeText" style="display: none">
                                <label for="VEN_PO_TYPE_TEXT">
                                    <%=Resources.Controls.Type%>*</label><asp:Label ID="VEN_PO_TYPE_TEXT" runat="server"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_ADDR3">
                                <%=Resources.Controls.LocalAddress%></label>
                            <asp:TextBox runat="server" ID="VEN_ADDR3" onkeypress="return this.value.length<200"
                                onpaste="return this.value.length<200" MaxLength="200" TabIndex="23" CssClass="input-half"  Height="45px"
                                TextMode="MultiLine"></asp:TextBox>
                            <span id="VEN_ADDR3"></span>
                            <div class="clear">
                            </div>
                            <label for="VNC_TYPE_NAME">
                                <%=Resources.Controls.TypeID%></label>
                            <asp:TextBox runat="server" ID="VNC_TYPE_NAME" onpaste="return this.value.length<5"
                                MaxLength="5" TabIndex="26" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <%--New Vendor Tax PopUp Begins--%>
            <div id="divVendorTax" title="<%=Resources.Messages.TaxDetails%>" style="display: none">
                <div class="Button-container-popup">
                    <asp:Button ID="btnApplyVendorTax" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                        OnClientClick="javascript:return SaveApplyVendorTax();" />
                </div>
                <div class="content-wrapper">
                    <div class="divcolmiddle-S">
                        <label for="ChooseTaxVendor">
                            <%=Resources.Controls.ChooseType%>
                        </label>
                        <asp:DropDownList ID="ChooseTaxVendor" runat="server" EnableViewState="false">
                        </asp:DropDownList>
                        <asp:ImageButton ID="imbTaxDiscountSaveVendor" SkinID="imbaddnew" runat="server"
                            TabIndex="15" EnableViewState="False" OnClientClick="javascript:return AddTaxDiscountVendorToGrid();" />
                    </div>
                    <div>
                        <table rules="all" id="grdTaxDetailsVendor" grandtype="GrandGrid" paging="false"
                            width="100%" editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="IVT_SL_NO" isvisible="false">
                                    </th>
                                    <th fieldmap="IVT_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="IVT_TAX" isvisible="false">
                                    </th>
                                    <th fieldmap="IVT_TAX_CATEGORY" isvisible="false">
                                    </th>
                                    <th fieldmap="IVT_TAX_TEXT" width="96%">
                                        <%=Resources.Controls.Type%>
                                    </th>
                                    <th type="Template" width="4%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="imbTaxDeleteVendor" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXDELETEVENDOR')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
            <%--New Vendor Tax PopUp Ends--%>
            <h1 class="search-colapse-normal" id="VendorHeading" style="display: none;">
                <%=Resources.Captions.VendorDetails%>
                <img id="imgVendorShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Show"
                    title="Show" style="cursor: pointer" onclick="javascript:ShowVendor();" />
                <img id="imgVendorHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="Hide" title="Hide" style="display: none; cursor: pointer" onclick="javascript:HideVendor();" />
            </h1>
            <div id="divTaxDataVendor">
            </div>
            <asp:HiddenField ID="hdfTaxPopUpByVendor" runat="server" Value="hdfTaxPopUpByVendor Value" />
            <div id="VendorSelection" style="display: none;">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="VEN_ANNUAL_SALES">
                                    <%=Resources.Controls.AnnualSales%></label>
                                <asp:TextBox runat="server" ID="VEN_ANNUAL_SALES" TabIndex="20" CssClass="numeric"
                                    MaxLength="20" onkeypress="javascript:MakeNumeric(event);"></asp:TextBox>
                                <label for="VEN_MANAGER">
                                    <%=Resources.Controls.ManagerName%></label>
                                <asp:TextBox runat="server" ID="VEN_MANAGER" onkeypress="return this.value.length<100"
                                    onpaste="return this.value.length<100" MaxLength="100" TabIndex="22"></asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S" style="float: right; margin-right: 0">
                                <label for="VEN_WAREHOUSE_DTL">
                                    <%=Resources.Controls.WarehouseDetails%></label>
                                <asp:TextBox runat="server" ID="VEN_WAREHOUSE_DTL" onkeypress="return this.value.length<200"
                                    onpaste="return this.value.length<200" MaxLength="200" TabIndex="21"></asp:TextBox>
                                <label for="VEN_HAS_ISO">
                                    <%=Resources.Controls.ISOCertificate%></label>
                                <asp:HiddenField runat="server" ID="VEN_HAS_ISO" Value="false" />
                                <asp:HiddenField runat="server" ID="Roles" Value="" />
                                <asp:CheckBox runat="server" ID="VEN_HAS_ISO1" Checked="false" TabIndex="23" />
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="divcol-FileuplWrap">
                    <label for="aupDocument">
                        <%=Resources.Controls.Attachments%></label>
                    <div id="FileUploader">
                        <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="24" Height="22px"
                            TabIndex="24" />
                        <asp:HiddenField ID="FILELIST" runat="server" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <div id="divResult">
        </div>
        <div id="divTaxData">
        </div>
        <div id="divData">
            <div id="divRoleData">
            </div>
        </div>
        <div id="tabs" runat="server">
            <div class="jquery-tabs">
                <ul id="tab-menu">
                    <%-- <li><a href="#AddressBook" tabindex="39">
                        <%=Resources.Controls.AddressBook%></a> </li>
                    <li><a href="#MaterialMapping" tabindex="40" onclick="TabChange('material')">
                        <%=Resources.Controls.MaterialMapping%></a> </li>
                    <li><a href="#Terms" tabindex="41" onclick="TabChange('terms')">
                        <%=Resources.Controls.Terms%></a> </li>                
                    <li><a href="#Samples" tabindex="42" onclick="TabChange('samples')">
                        <%=Resources.Controls.Samples%></a> </li>
                    <li><a href="#VendorRoles" tabindex="43" onclick="TabChange('VendorRoles')">
                        <%=Resources.Controls.VendorRoles%></a> </li>--%>
                    <li><a href="#Banks" tabindex="44" onclick="TabChange('Banks')">
                        <%=Resources.Controls.Bank%></a> </li>
                </ul>
            </div>
            <div class="clear">
            </div>
            <div style="display: none">
                <div id="AddressBook" class="jquery-tabs-contents">
                    <div id="AddressBookData">
                        <input type="hidden" runat="server" id="EditAddress" value="0" />
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="VNC_NAME">
                                            <%=Resources.Controls.Title%>
                                            *</label>
                                        <asp:TextBox runat="server" ID="VNC_NAME" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="27"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <label for="VNC_CONT_NAME">
                                            <%=Resources.Controls.ContactPerson%>
                                            *</label>
                                        <asp:TextBox runat="server" ID="VNC_CONT_NAME" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="29"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <label for="VNC_PHONE">
                                            <%=Resources.Controls.Phone%>
                                            *</label>
                                        <asp:TextBox runat="server" ID="VNC_PHONE" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="31"></asp:TextBox>
                                        <label for="VNC_ADDR1">
                                            <%=Resources.Controls.Address1%></label>
                                        <asp:TextBox runat="server" ID="VNC_ADDR1" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="33"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <label for="VNC_ADDR2">
                                            <%=Resources.Controls.Address2%></label>
                                        <asp:TextBox runat="server" ID="VNC_ADDR2" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="35"></asp:TextBox>
                                        <label for="VNC_EMAIL">
                                            <%=Resources.Controls.Email%></label>
                                        <asp:TextBox runat="server" ID="VNC_EMAIL" onkeypress="return this.value.length<100"
                                            onpaste="return this.value.length<100" MaxLength="100" TabIndex="37"></asp:TextBox>
                                        <label for="VNC_NAME2">
                                            <%=Resources.Controls.LocalName%></label>
                                        <asp:TextBox runat="server" ID="VNC_NAME2" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="39"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="VNC_DEFAULT">
                                            <%=Resources.Controls.Primary%>
                                        </label>
                                        <asp:CheckBox runat="server" ID="VNC_DEFAULT" Checked="true" TabIndex="28" />
                                        <div class="clear">
                                        </div>
                                        <label for="VNC_CITY">
                                            <%=Resources.Controls.City%></label>
                                        <asp:TextBox runat="server" ID="VNC_CITY" onkeypress="return this.value.length<200"
                                            onpaste="return this.value.length<200" MaxLength="200" TabIndex="30"></asp:TextBox>
                                        <label for="VNC_CNTRY">
                                            <%=Resources.Controls.Country%></label>
                                        <asp:DropDownList ID="VNC_CNTRY" runat="server" onchange="FillState('ADDRESS',false,$(this).val())"
                                            TabIndex="32">
                                        </asp:DropDownList>
                                        <label for="VNC_STATE">
                                            <%=Resources.Controls.State%></label>
                                        <asp:DropDownList ID="VNC_STATE" runat="server" TabIndex="34">
                                        </asp:DropDownList>
                                        <label for="VNC_MOBIL">
                                            <%=Resources.Controls.Mobile%></label>
                                        <asp:TextBox runat="server" ID="VNC_MOBIL" onkeypress="return this.value.length<100"
                                            onpaste="return this.value.length<100" MaxLength="100" TabIndex="36"></asp:TextBox>
                                        <label for="VNC_FAX">
                                            <%=Resources.Controls.Fax%></label>
                                        <asp:TextBox runat="server" ID="VNC_FAX" onkeypress="return this.value.length<100"
                                            onpaste="return this.value.length<100" MaxLength="100" TabIndex="38"></asp:TextBox>
                                        <label for="VNC_TAX_NO">
                                            <%=Resources.Controls.TaxID%></label>
                                        <asp:TextBox runat="server" ID="VNC_TAX_NO" onkeypress="return this.value.length<100"
                                            onpaste="return this.value.length<100" MaxLength="100" TabIndex="40"></asp:TextBox>
                                        <label for="VNC_ADDR3">
                                            <%=Resources.Controls.LocalAddress%></label>
                                        <asp:TextBox runat="server" ID="VNC_ADDR3" onkeypress="return this.value.length<500"
                                            onpaste="return this.value.length<200" MaxLength="500" CssClass="multiline-3line"
                                            TextMode="MultiLine" TabIndex="42"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <div class="button-wrap-right">
                                            <%--  <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack" style="width: 130px"
                                            class="inputbtn" onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                            <asp:Button ID="AddressSave" runat="server" Text="<%$Resources:Controls,saveandaddnew%>"
                                                SkinID="btnInner-Save" ToolTip="<%$resources:Controls,saveandaddnew %>" EnableViewState="False"
                                                OnClientClick="javascript:return SaveAddress();" TabIndex="44" />
                                            <asp:Button ID="btnBack" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Clear%>"
                                                EnableViewState="False" ToolTip="<%$resources:Controls,Clear %>" OnClientClick="javascript:return ClearAddress();"
                                                TabIndex="43" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                        <div class="gridwrap max-200">
                            <table rules="all" id="grdVendorAddressDetails" grandtype="GrandGrid" paging="false"
                                editfunction="GridAddressAction" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="AddressID" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_ADDR1" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_ADDR3" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_NAME2" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_ADDR2" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_EMAIL" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_DEFAULT" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_CITY" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_CNTRY" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_STATE" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_MOBIL" isvisible="false">
                                        </th>
                                        <th fieldmap="VNC_TAX_NO" isvisible="false">
                                        </th>
                                        <%--   //Edit 18_08--%>
                                        <th fieldmap="VNC_TYPE_NAME" isvisible="false">
                                        </th>
                                        <%--  //end--%>
                                        <th fieldmap="VNC_FAX" isvisible="false">
                                        </th>
                                        <%------------------------------------------------//New start--------------------------------%>
                                        <th fieldmap="VNC_TYPE" isvisible="false">
                                        </th>
                                        <%------------------------------------------------//New end--------------------------------%>
                                        <th fieldmap="VNC_NAME" align="left" width="25%">
                                            <%=Resources.Controls.Title%>
                                        </th>
                                        <th fieldmap="VNC_CONT_NAME" align="left" width="25%">
                                            <%=Resources.Controls.ContactPerson%>
                                        </th>
                                        <th fieldmap="VNC_PHONE" align="left" width="20%">
                                            <%=Resources.Controls.Phone%>
                                        </th>
                                        <th fieldmap="VNC_TYPE_TEXT" align="left" width="20%">
                                            <%=Resources.Controls.Type%>
                                        </th>
                                        <th fieldmap="VNC_ADDRESS_TYPE" align="left" width="20%">
                                            <%=Resources.Controls.Primary%>
                                        </th>
                                        <th type="Template" width="10%">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridAddressHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridAddressHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                                    SkinID="btnview" OnClientClick="javascript:return GridAddressHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Terms" class="jquery-tabs-contents">
                    <div id="TermsData">
                        <div class="gridwrap">
                            <table rules="all" id="grdTermsDetails" grandtype="GrandGrid" paging="false" editfunction="GridTermsAction"
                                enablecheckbox="true" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="VET_TYPE" isvisible="false">
                                        </th>
                                        <th fieldmap="VET_PK" isvisible="false">
                                        </th>
                                        <th fieldmap="VET_TITLE" width="60%" align="left">
                                            <%=Resources.Controls.Title%>
                                        </th>
                                        <th type="Template" align="left" width="38%">
                                            <div>
                                                <input id="Text1" style="width: 90%" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="MaterialMapping" class="jquery-tabs-contents">
                    <div id="MaterialMappingData">
                        <div class="grdTable">
                            <div id="divdummyMaterial">
                                <table id="materialInsert" class="gridwraptable gridwrap tablefixwidth-td">
                                    <thead>
                                        <tr class="dummytr">
                                            <th style="width: 110px">
                                                <%=Resources.Controls.MaterialCategory%>*
                                            </th>
                                            <th style="width: 100px">
                                                <%=Resources.Controls.MaterialCode%>*
                                            </th>
                                            <th style="width: 120px">
                                                <%=Resources.Controls.Material%>*
                                            </th>
                                            <th align="right" style="width: 60px">
                                                <%=Resources.Controls.Price%>
                                            </th>
                                            <th style="width: 80px">
                                                <%=Resources.Controls.Currency%>*
                                            </th>
                                            <th style="width: 50px" align="right">
                                                <%=Resources.Controls.MOQ%>
                                            </th>
                                            <th style="width: 50px">
                                                <%=Resources.Controls.UOM%>*
                                            </th>
                                            <th style="width: 75px" align="right">
                                                <%=Resources.Controls.LeadDays%>*
                                            </th>
                                            <th style="width: 45px" align="right">
                                                <%=Resources.Controls.Active%>
                                            </th>
                                            <th>
                                            </th>
                                            <%---------------------------------//NewMaterial End------------------%>
                                            <th style="width: 80px">
                                                <%=Resources.Controls.Action%>
                                            </th>
                                            <th>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr id="dummyTr" class="grd-rowhead">
                                            <td>
                                                <asp:DropDownList ID="MaterialType" runat="server" Width="80px" onchange="javascript:FillCategoryDetails($(this).val());"
                                                    TabIndex="50">
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imbViewCag" runat="server" SkinID="btnview" Width="22px" Height="22px"
                                                    ToolTip="<%$ Resources:Controls, Add%>" TabIndex="51" OnClientClick="javascript:return ShowCategory();"
                                                    EnableViewState="false" CssClass="floatRight" />
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ITV_ITEM" runat="server" Width="90px" onchange="javascript:FillMaterialDetails($(this).val());"
                                                TabIndex="52">
                                            </asp:DropDownList>--%>
                                                <asp:TextBox ID="MaterialItem" runat="server" Width="90%" TabIndex="52">
                                                </asp:TextBox>
                                                <asp:HiddenField ID="ITV_ITEM" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ITV_NAME" runat="server" Text="" onkeypress="return this.value.length<100"
                                                    onpaste="return this.value.length<100" Width="110px" TabIndex="53"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="ITV_PRICE" CssClass="numeric input-w58" Text="" TabIndex="54"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ITV_CURRENCY" runat="server" CssClass="input-uom" TabIndex="55">
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="ITV_MOQ" Text="" CssClass="numeric input-w50" TabIndex="56"></asp:TextBox>
                                                <%--  onchange="GrandScriptUtils.SetZeroDefault(this, 2)"--%>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ITV_MOQ_UOM" runat="server" TabIndex="57" CssClass="input-uom">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox ID="ITV_LEAD_TIME" runat="server" Text="0" MaxLength="5" CssClass="numeric input-w50"
                                                    onchange="GrandScriptUtils.SetZeroDefault(this)" TabIndex="59" onkeypress="javascript:MakeNumeric(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlActive" runat="server" Width="60px" TabIndex="8">
                                                    <asp:ListItem Value="1" Text="<%$ Resources:Captions,Yes %>"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="<%$ Resources:Captions,No %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbaddnew" Text="0.00" OnClientClick="javascript:return AddMaterialDetails()"
                                                    TabIndex="59" />
                                                <input type="hidden" runat="server" id="EditMaterial" value="0" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divMaterials" class="max-250">
                                <table rules="all" id="grdVendorMaterialDetails" grandtype="GrandGrid" pagesize="25"
                                    paging="true" editfunction="GridMaterialAction" editable="true" class="gridwraptable gridwrap tablefixwidth-td"
                                    width="100%">
                                    <thead>
                                        <tr>
                                            <th fieldmap="ITV_SL_NO" isvisible="false">
                                            </th>
                                            <%--  <th fieldmap="VendorMaterialID" isvisible="false">
                                        </th>--%>
                                            <th fieldmap="ITV_PK" isvisible="false">
                                            </th>
                                            <th fieldmap="MaterialType" isvisible="false">
                                            </th>
                                            <th fieldmap="ITV_ITEM" isvisible="false">
                                            </th>
                                            <th fieldmap="ITV_MOQ_UOM" isvisible="false">
                                            </th>
                                            <th fieldmap="ITV_CURRENCY" isvisible="false">
                                            </th>
                                            <th fieldmap="MaterialTypeText" align="left" width="110px">
                                                <%=Resources.Controls.MaterialCategory%>*
                                            </th>
                                            <th fieldmap="MaterialCode" width="100px">
                                                <%=Resources.Controls.MaterialCode%>*
                                            </th>
                                            <th fieldmap="ITV_NAME" width="140px">
                                                <%=Resources.Controls.Material%>*
                                            </th>
                                            <th fieldmap="ITV_PRICE" width="60px" align="right">
                                                <%=Resources.Controls.Price%>
                                            </th>
                                            <th fieldmap="MaterialCurrencyText" width="80px">
                                                <%=Resources.Controls.Currency%>*
                                            </th>
                                            <th fieldmap="ITV_MOQ" width="50px" align="right">
                                                <%=Resources.Controls.MOQ%>
                                            </th>
                                            <th fieldmap="UOMText" width="50px">
                                                <%=Resources.Controls.Unit%>*
                                            </th>
                                            <th fieldmap="ITV_LEAD_TIME" width="75px" align="right">
                                                <%=Resources.Controls.LeadDays%>*
                                            </th>
                                            <th fieldmap="ITV_ACTIVE" width="45px" align="right">
                                                <%=Resources.Controls.Active%>*
                                            </th>
                                            <th fieldmap="ITV_TAX_PERC" width="15px">
                                            </th>
                                            <th fieldmap="ITV_DISC_PERC" width="15px">
                                            </th>
                                            <%---------------------------------//NewMaterial End----------------------------------------------%>
                                            <th type="Template" width="80px">
                                                <div>
                                                    <asp:ImageButton runat="server" ID="imbEditMaterial" ToolTip="<%$Resources:Controls,Edit %>"
                                                        SkinID="imbeditgrid" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'EDIT')" />
                                                    <asp:ImageButton runat="server" ID="imbDeleteMaterial" ToolTip="<%$Resources:Controls,Delete %>"
                                                        SkinID="imbdeletegrid" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'DELETE')" />
                                                    <asp:ImageButton runat="server" ID="imbrateHistory" ToolTip="<%$Resources:ErpRes,RateHistory %>"
                                                        SkinID="history" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'HISTORY')" />
                                                    <%-- <asp:ImageButton ID="imbrateHistory" runat="server" SkinID="btnview" Width="22px" Height="22px"
                                                ToolTip="<%$ Resources:Controls, Add%>" TabIndex="51" OnClientClick="javascript:return ShowRateHistory();"
                                                EnableViewState="false" />--%>
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divItemTax" title="<%=Resources.Messages.TaxDetails%>" style="display: none">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClientClick="javascript:return SaveApply();" />
                    </div>
                    <div class="content-wrapper">
                        <div class="divcolmiddle-S">
                            <label for="ChooseTax">
                                <%=Resources.Controls.ChooseType%>
                            </label>
                            <asp:DropDownList ID="ChooseTax" runat="server" EnableViewState="false">
                            </asp:DropDownList>
                            <asp:ImageButton ID="imbTaxDiscountSave" SkinID="imbaddnew" runat="server" TabIndex="15"
                                EnableViewState="False" OnClientClick="javascript:return SaveTaxDiscount();" />
                        </div>
                        <asp:HiddenField runat="server" ID="hdfTaxCategory" Value="1" />
                        <asp:HiddenField runat="server" ID="hdnSlNo" Value="0" />
                        <div>
                            <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="false" width="100%"
                                editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="IVT_SL_NO" isvisible="false">
                                        </th>
                                        <th fieldmap="IVT_PK" isvisible="false">
                                        </th>
                                        <th fieldmap="IVT_TAX" isvisible="false">
                                        </th>
                                        <th fieldmap="IVT_TAX_CATEGORY" isvisible="false">
                                        </th>
                                        <th fieldmap="IVT_TAX_TEXT" width="96%">
                                            <%=Resources.Controls.Type%>
                                        </th>
                                        <th type="Template" width="4%">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbTaxDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXDELETE')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
                <%--------------------------------------/NewMaterial Rate History Start----------------------------------------------------------------%>
                <div id="divRateHistory" title="<%=Resources.Captions.RateHistory%>" style="display: none">
                    <div class="content-wrapper">
                        <div class="divcolmiddle-S">
                            <label for="ItemCode">
                                <%=Resources.Controls.ItemCode%>
                            </label>
                            <asp:Label ID="ItemCode" runat="server"></asp:Label>
                            <label for="ItemName">
                                <%=Resources.Controls.ItemName%>
                            </label>
                            <asp:Label ID="ItemName" runat="server"></asp:Label>
                        </div>
                        <div>
                            <table id="grdRateDetails" grandtype="GrandGrid" rules="all" paging="false" width="100%"
                                class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="VIH_EFCT_FROM" width="25%">
                                            <%=Resources.Controls.FromDate%>
                                        </th>
                                        <th fieldmap="VIH_EFCT_TO" width="25%">
                                            <%=Resources.Controls.ToDate%>
                                        </th>
                                        <th fieldmap="VIH_RATE" width="20%">
                                            <%=Resources.Controls.Rate%>
                                        </th>
                                        <th fieldmap="VIH_TAX_PERC" width="15%">
                                            <%=Resources.Controls.Tax%>
                                        </th>
                                        <th fieldmap="VIH_DISC_PERC" width="15%">
                                            <%=Resources.Controls.Discount%>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
                <div id="VendorRoles" class="jquery-tabs-contents">
                    <div id="divTree" style="overflow: auto">
                        <div id="treewrap">
                            <div id="trvRoleMap" class="treeview-adj">
                            </div>
                        </div>
                    </div>
                </div>
                <%--------------------------------------/NewMaterial Rate History Update End----------------------------------------------------------------%>
                <%--------------------------------------/New Update Start Tab----------------------------------------------------------------%>
                <%--------------------------------------/New Update Start Tab----------------------------------------------------------------%>
                <div id="Samples" class="jquery-tabs-contents">
                    <div id="SamplesData">
                        <div class="grdTable">
                            <div id="divdummySamples">
                                <table id="sampleInsert" class="gridwraptable gridwrap">
                                    <thead>
                                        <tr class="dummytr">
                                            <th style="width: 17%">
                                                <%=Resources.Controls.MaterialCategory%>
                                                *
                                            </th>
                                            <th style="width: 15%">
                                                <%=Resources.Controls.MaterialCode%>
                                                *
                                            </th>
                                            <th style="width: 10%">
                                                <%=Resources.Controls.ReceivedDate%>
                                                *
                                            </th>
                                            <th style="width: 10%">
                                                <%=Resources.Controls.QCTest%>
                                                *
                                            </th>
                                            <th style="width: 10%">
                                                <%=Resources.Controls.Value%>
                                                *
                                            </th>
                                            <th style="width: 9%">
                                                <%=Resources.Controls.Remarks%>
                                                *
                                            </th>
                                            <th style="width: 9%">
                                                <%=Resources.Controls.Qty%>
                                                *
                                            </th>
                                            <th style="width: 9%">
                                                <%=Resources.Controls.Reference%>
                                            </th>
                                            <th style="width: 12%">
                                                <%=Resources.Controls.Acpt%>
                                                *
                                            </th>
                                            <th style="width: 10%">
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr id="dummyTrSamples" class="grd-rowhead">
                                            <td>
                                                <asp:DropDownList ID="MaterialTypeSamples" runat="server" Width="70%" onchange="javascript:FillCategorySamplesDetails($(this).val());"
                                                    TabIndex="50">
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imbViewCagSamples" runat="server" SkinID="btnview" Width="22px"
                                                    Height="22px" ToolTip="<%$ Resources:Controls, Add%>" TabIndex="51" OnClientClick="javascript:return ShowCategory();"
                                                    EnableViewState="false" />
                                            </td>
                                            <td>
                                                <%--   <asp:DropDownList ID="ISV_ITEM" runat="server" Width="90%" TabIndex="52">
                                            </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtSampleItem" runat="server" Width="90%" TabIndex="52">
                                                </asp:TextBox>
                                                <asp:HiddenField ID="ISV_ITEM" runat="server" Value="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ISV_RECEIVED_DATE" runat="server" Text="" onkeydown="return CheckKey(event)"
                                                    onpaste="return false;" CssClass="date-picker" TabIndex="53"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="ISV_QC_TEST" onkeypress="return this.value.length<100"
                                                    onpaste="return this.value.length<100" Width="60px" Text="" TabIndex="54"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox runat="server" ID="ISV_QC_VALUE" Width="90%" Text="" TabIndex="55"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ISV_REMARKS" runat="server" onkeypress="return this.value.length<400"
                                                    onpaste="return this.value.length<400" Width="60px" TabIndex="56"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="ISV_QUANTITY" Width="90%" Text="" TabIndex="57"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ISV_REFERENCE" runat="server" onkeypress="return this.value.length<400"
                                                    onpaste="return this.value.length<400" Width="60px" TabIndex="58"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="ISV_STATUS" runat="server" Checked="false" TabIndex="59"></asp:CheckBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbSamplesAdd" OnClientClick="javascript:return AddSampleDetails()"
                                                    TabIndex="60" />
                                                <input type="hidden" runat="server" id="EditSamples" value="0" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divSamples" class="max-250">
                                <table rules="all" id="grdMaterialSampleDetails" grandtype="GrandGrid" paging="false"
                                    editfunction="GridSamplesAction" editable="true" class="gridwraptable gridwrap"
                                    width="100%">
                                    <thead>
                                        <tr>
                                            <th fieldmap="VendorMaterialID" isvisible="false">
                                            </th>
                                            <th fieldmap="MaterialType" isvisible="false">
                                            </th>
                                            <th fieldmap="ISV_ITEM" isvisible="false">
                                            </th>
                                            <th fieldmap="MaterialTypeText" align="left" width="17%">
                                                <%=Resources.Controls.MaterialCategory%>
                                            </th>
                                            <th fieldmap="SampleMaterialCode" width="15%">
                                                <%=Resources.Controls.MaterialCode%>
                                            </th>
                                            <th fieldmap="ISV_RECEIVED_DATE" width="12%">
                                                <%=Resources.Controls.ReceivedDate%>
                                            </th>
                                            <th fieldmap="ISV_QC_TEST" width="12%">
                                                <%=Resources.Controls.QCTest%>
                                            </th>
                                            <th fieldmap="ISV_QC_VALUE" width="10%">
                                                <%=Resources.Controls.Value%>
                                            </th>
                                            <th fieldmap="ISV_REMARKS" width="9%">
                                                <%=Resources.Controls.Remarks%>
                                            </th>
                                            <th fieldmap="ISV_QUANTITY" width="10%">
                                                <%=Resources.Controls.Qty%>
                                            </th>
                                            <th fieldmap="ISV_REFERENCE" width="9%">
                                                <%=Resources.Controls.Reference%>
                                            </th>
                                            <th fieldmap="ISV_STATUS" isvisible="false">
                                                <%=Resources.Controls.Accept%>
                                            </th>
                                            <th fieldmap="ISV_STATUS_TEXT" width="15%">
                                                <%=Resources.Controls.AcceptOrReject%>
                                            </th>
                                            <th type="Template" width="10%">
                                                <div>
                                                    <%-- <asp:ImageButton runat="server" ID="imbSamplesEdit" ToolTip="<%$Resources:Controls,Edit %>" SkinID="imbeditgrid" OnClientClick="javascript:return GridSamplesAction($(this).parents('tr:eq(0)'),'EDIT')" />--%>
                                                    <asp:ImageButton runat="server" ID="imbSamplesDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                        SkinID="imbdeletegrid" OnClientClick="javascript:return GridSamplesAction($(this).parents('tr:eq(0)'),'DELETE')" />
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <%--------------------------------------/New Update End----------------------------------------------------------------%>
                <div class="clear">
                </div>
            </div>
            <div id="Banks" class="jquery-tabs-contents">
                <div id="BankData">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_NAME" class="lbl-22-6perc">
                                        <%=Resources.Controls.BankName%>*</label>
                                    <asp:TextBox runat="server" ID="VBD_NAME" onkeypress="return this.value.length<200"
                                        onpaste="return this.value.length<200" MaxLength="200" TabIndex="61" CssClass="lbl-62perc"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_BRANCH">
                                        <%=Resources.Controls.Branch%></label>
                                    <asp:TextBox runat="server" ID="VBD_BRANCH" onpaste="return this.value.length<200"
                                        MaxLength="200" onkeypress="return this.value.length<200" TabIndex="62" CssClass="lbl-62perc"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="div2col-S">
                                    <label for="VBD_ADDRESS" class="lbl-11-3perc">
                                        <%=Resources.Controls.Address%></label>
                                    <asp:TextBox runat="server" ID="VBD_ADDRESS" onkeypress="return this.value.length<500"
                                        onpaste="return this.value.length<500" MaxLength="500" class="multiline-2line"
                                        TextMode="MultiLine" Style="width: 695px!important;" TabIndex="63" CssClass="lbl-31-1perc" Height="45px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_CITY" class="lbl-22-6perc">
                                        <%=Resources.Controls.City%></label>
                                    <asp:TextBox runat="server" ID="VBD_CITY" onkeypress="return this.value.length<200"
                                        onpaste="return this.value.length<200" MaxLength="200" TabIndex="64" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_COUNTRY" class="lbl-22-6perc">
                                        <%=Resources.Controls.Country%></label>
                                    <asp:DropDownList ID="VBD_COUNTRY" runat="server" TabIndex="66" CssClass="lbl-63-2perc">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_PHONE" class="lbl-22-6perc">
                                        <%=Resources.Controls.Phone%></label>
                                    <asp:TextBox runat="server" ID="VBD_PHONE" onkeypress="return this.value.length<50"
                                        onpaste="return this.value.length<50" MaxLength="50" TabIndex="68" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_FAX" class="lbl-22-6perc">
                                        <%=Resources.Controls.Fax%></label>
                                    <asp:TextBox runat="server" ID="VBD_FAX" onkeypress="return this.value.length<50"
                                        onpaste="return this.value.length<50" MaxLength="50" TabIndex="70" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_IFSC_CODE" class="lbl-22-6perc">
                                        <%=Resources.Controls.IFSCCode%></label>
                                    <asp:TextBox runat="server" ID="VBD_IFSC_CODE" onkeypress="return this.value.length<100"
                                        onpaste="return this.value.length<100" MaxLength="100" TabIndex="72" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ACCOUNT_NO" class="lbl-22-6perc">
                                        <%=Resources.Controls.AccountNumber%></label>
                                    <asp:TextBox runat="server" ID="VBD_ACCOUNT_NO" onkeypress="return this.value.length<100"
                                        onpaste="return this.value.length<100" MaxLength="100" TabIndex="74" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ACCOUNT_TYPE" class="lbl-22-6perc">
                                        <%=Resources.Controls.AccountType%></label>
                                    <asp:DropDownList ID="VBD_ACCOUNT_TYPE" runat="server" TabIndex="76" CssClass="lbl-30-7perc">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="VBD_ACCOUNT_TYPE_OTHER" onkeypress="return this.value.length<180"
                                        onpaste="return this.value.length<180" MaxLength="180" CssClass="lbl-30-7perc"
                                        TabIndex="77"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_STATE_OTHER">
                                        <%=Resources.Controls.State%></label>
                                    <asp:TextBox runat="server" ID="VBD_STATE_OTHER" onkeypress="return this.value.length<200"
                                        onpaste="return this.value.length<200" MaxLength="200" TabIndex="65" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ZIP">
                                        <%=Resources.Controls.Zip%></label>
                                    <asp:TextBox runat="server" ID="VBD_ZIP" onkeypress="return this.value.length<50"
                                        onpaste="return this.value.length<50" MaxLength="50" TabIndex="67" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_MOBILE">
                                        <%=Resources.Controls.Mobile%></label>
                                    <asp:TextBox runat="server" ID="VBD_MOBILE" onkeypress="return this.value.length<50"
                                        onpaste="return this.value.length<50" MaxLength="50" TabIndex="69" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_EMAIL">
                                        <%=Resources.Controls.Email%></label>
                                    <asp:TextBox runat="server" ID="VBD_EMAIL" onkeypress="return this.value.length<200"
                                        onpaste="return this.value.length<200" MaxLength="200" TabIndex="71" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_SWIFT_CODE">
                                        <%=Resources.Controls.SWIFTCode%></label>
                                    <asp:TextBox runat="server" ID="VBD_SWIFT_CODE" onkeypress="return this.value.length<100"
                                        onpaste="return this.value.length<100" MaxLength="100" TabIndex="73" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ACCOUNT_NO_CNFM">
                                        <%=Resources.Controls.ReEnterAccountNo%></label>
                                    <asp:TextBox runat="server" ID="VBD_ACCOUNT_NO_CNFM" onkeypress="return this.value.length<100"
                                        onpaste="return this.value.length<100" MaxLength="100" TabIndex="75" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_CONTACT">
                                        <%=Resources.Controls.ContactName%></label>
                                    <asp:TextBox runat="server" ID="VBD_CONTACT" onkeypress="return this.value.length<200"
                                        onpaste="return this.value.length<200" MaxLength="200" TabIndex="78" CssClass="lbl-62perc"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div class="button-wrap-right">
                                        <%--  <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack" style="width: 130px"
                                            class="inputbtn" onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                        <asp:Button ID="BankClear" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Clear%>"
                                            EnableViewState="False" ToolTip="<%$resources:Controls,Clear %>" OnClientClick="javascript:return clearBank();"
                                            TabIndex="80" />
                                        <asp:Button ID="BankSave" runat="server" Text="<%$Resources:Controls,saveandaddnew%>"
                                            SkinID="btnInner-Save" ToolTip="<%$resources:Controls,saveandaddnew %>" EnableViewState="False"
                                            OnClientClick="javascript:return AddBankDetails();" TabIndex="79" />
                                        <%--<input type="button" value="<%=Resources.Controls.saveandaddnew%>" id="AddressSave"
                                            style="width: 130px" class="inputbtn" onclick="javascript:return SaveAddress();"
                                            tabindex="29" />--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                    <div class="gridwrap max-200">
                        <div class="grdTable">
                            <%--class="max-250"--%>
                            <table rules="all" id="grdVendorBankDetails" grandtype="GrandGrid" pagesize="25"
                                paging="true" editfunction="GridBankAction" editable="true" class="gridwraptable gridwrap tablefixwidth-td"
                                width="100%">
                                <thead>
                                    <tr>
                                        <%--<th fieldmap="ITV_SL_NO" isvisible="true">
                                        </th>
                                          <th fieldmap="VendorMaterialID" isvisible="true">
                                        </th>--%>
                                        <th fieldmap="VBD_PK" isvisible="false">
                                        </th>
                                        <%--<th fieldmap="MaterialType" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_ITEM" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_MOQ_UOM" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_CURRENCY" isvisible="false">
                                        </th>--%>
                                        <th fieldmap="VBD_NAME" align="left" width="11%">
                                            <%=Resources.Controls.BankName%>
                                        </th>
                                        <th fieldmap="VBD_BRANCH" width="10%">
                                            <%=Resources.Controls.Branch%>
                                        </th>
                                        <th fieldmap="VBD_COUNTRY_NAME" width="11%">
                                            <%=Resources.Controls.Country%>
                                        </th>
                                        <th fieldmap="VBD_IFSC_CODE" width="11%">
                                            <%=Resources.Controls.IFSCCode%>
                                        </th>
                                        <th fieldmap="VBD_SWIFT_CODE" width="11%">
                                            <%=Resources.Controls.SWIFTCode%>
                                        </th>
                                        <th fieldmap="VBD_ACCOUNT_NO" width="11%">
                                            <%=Resources.Controls.AccountNumber%>
                                        </th>
                                        <th fieldmap="VBD_CONTACT" width="11%">
                                            <%=Resources.Controls.ContactName%>
                                        </th>
                                        <th type="Template" width="5%">
                                            <div align="right">
                                                <asp:ImageButton runat="server" ID="imbViewBankGrid" ToolTip="<%$Resources:Controls,View %>"
                                                    SkinID="btnview" OnClientClick="javascript:return GridBankAction($(this).parents('tr:eq(0)'),'VIEW')" />
                                                <asp:ImageButton runat="server" ID="ImageButton4" ToolTip="<%$Resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridBankAction($(this).parents('tr:eq(0)'),'EDIT')" />
                                                <asp:ImageButton runat="server" ID="ImageButton5" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridBankAction($(this).parents('tr:eq(0)'),'DELETE')" />
                                            </div>
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
    </div>
    <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
    <div id="Wofkflowdiv" style="display: none">
        <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
        <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
        <asp:HiddenField ID="TaskID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfVendorPK" runat="server" Value="0" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
    </div>
    <%--#####END Add user Controls and process Id, app ID#####--%>
    <div class="clear">
    </div>
    <asp:HiddenField runat="server" ID="MaterialDetailId" Value="0" EnableViewState="false" />
    <div id="divCategory" title="<%=Resources.Captions.SelectCategory%>">
        <div id="treewrap" class="treeviewrap-fxd">
            <div id="trvCategory" class="treeview-adj">
            </div>
        </div>
    </div>
    <input type="hidden" id="hdfVirtualPath" value='<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())
    %>' />
    <asp:HiddenField ID="hdfIsVendorTax" runat="server" />
    <asp:HiddenField ID="hdfIsMaterialTax" runat="server" />
</asp:Content>
