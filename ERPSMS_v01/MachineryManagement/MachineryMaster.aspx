<%@ Page Title="Machinery Master" Theme="ERP-Blue" Language="C#" MasterPageFile="~/ERPSMS.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="MachineryMaster.aspx.cs"
    EnableViewState="false" Inherits="ERPSMS_v01.MachineryManagement.MachineryMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/MachineryManagement/MachineryMaster.js.axd" type="text/javascript"></script>
    <script src="../Scripts/jquery/FileUpload/jquery.MultiFile.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MachineryMaster%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" TabIndex="33" EnableViewState="false"
                SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" TabIndex="29" EnableViewState="false"
                SkinID="btnsave" OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton ID="imdReset" runat="server" TabIndex="34" EnableViewState="false"
                SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return history.go(-1)" />
        </div>
    </div>
    <div id="grdTable-wrap">
        <div id="divData">
            <div class="div2col-S">
                <div class="content">
                    <div>
                        <label for="Code">
                            <%=Resources.Controls.MachineCode%>
                        </label>
                        <asp:TextBox ID="Code" runat="server" EnableViewState="false" TabIndex="1">
                        </asp:TextBox>
                    </div>
                    <div>
                        <label for="MachineName">
                            <%=Resources.Controls.Name%>
                        </label>
                        <asp:TextBox ID="MachineName" runat="server" EnableViewState="false" TabIndex="4">
                        </asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="div2col-S" style="float: right; margin-right: 0">
                <div class="content">
                    <label for="MachineType">
                        <%=Resources.Controls.Type%>
                    </label>
                    <asp:DropDownList ID="MachineType" runat="server" EnableViewState="false" Width="150px"
                        TabIndex="2">
                    </asp:DropDownList>
                    <asp:ImageButton ID="imbAddMachineType" runat="server" EnableViewState="false" SkinID="imbaddnew"
                        Width="16px" CssClass="srchbtn" Height="16px" Text="Add" ToolTip="Add" TabIndex="3"
                        OnClientClick="javascript:return AddMachineTypeDetails();" />
                    <div class="clear">
                    </div>
                    <label for="Location">
                        <%=Resources.Controls.LocationWorkCenter%>
                    </label>
                    <asp:DropDownList ID="Location" runat="server" Width="150px" TabIndex="5" EnableViewState="false">
                    </asp:DropDownList>
                    <asp:ImageButton ID="imbAddLocation" runat="server" EnableViewState="false" SkinID="imbaddnew"
                        Width="16px" CssClass="srchbtn" Height="16px" Text="Add" ToolTip="Add" TabIndex="6"
                        OnClientClick="javascript:return AddLocationDetails();" />
                </div>
            </div>
            <div class="clear">
            </div>
            <div id="tabs" runat="server">
                <ul>
                    <li><a href="#Purchase">
                        <%=Resources.Controls.PurchaseInfo%>
                    </a></li>
                    <li><a href="#Maintanace">
                        <%=Resources.Controls.MaintenanceInfo%></a></li>
                    <li><a href="#Performance">
                        <%=Resources.Controls.PerformanceInfo%></a></li>
                </ul>
                <div id="Purchase" style="width: 99%">
                    <div class="div2col-S">
                        <label for="PurchaseFrom">
                            <%=Resources.Controls.PurchasedFrom%>
                        </label>
                        <asp:DropDownList ID="PurchaseFrom" runat="server" Width="150px" TabIndex="7" EnableViewState="false">
                        </asp:DropDownList>
                        <asp:ImageButton ID="imbAddVendor" runat="server" EnableViewState="false" SkinID="imbaddnew"
                            Width="16px" CssClass="srchbtn" Height="16px" Text="Add" ToolTip="Add" TabIndex="8"
                            OnClientClick="javascript:return AddVendorDetails();" />
                        <div class="clear">
                        </div>
                        <label for="Condition">
                            <%=Resources.Controls.Condition%>
                        </label>
                        <asp:DropDownList runat="server" ID="Condition" EnableViewState="false" TabIndex="10">
                        </asp:DropDownList>
                        <label for="PurchasePrice">
                            <%=Resources.Controls.PurchasePrice%></label>
                        <asp:TextBox runat="server" ID="PurchasePrice" EnableViewState="false" Width="80px"
                            TabIndex="12">
                        </asp:TextBox>
                        <asp:DropDownList runat="server" ID="PurchasePriceUOM" EnableViewState="false" Width="70px"
                            TabIndex="13">
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                        <label for="ExpiryDate">
                            <%=Resources.Controls.Expiry%>
                        </label>
                        <asp:TextBox runat="server" ID="ExpiryDate" EnableViewState="false" TabIndex="15">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdnExpiryDate" runat="server" />
                    </div>
                    <div class="div2col-S" style="float: right; margin-right: 0">
                        <label for="DateofPurchase">
                            <%=Resources.Controls.DateofPurchase%>
                        </label>
                        <asp:TextBox runat="server" ID="DateofPurchase" EnableViewState="false" TabIndex="9">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdnDateofPurchase" runat="server" />
                        <label for="PurchaseMode">
                            <%=Resources.Controls.ModeofPurchase%>
                        </label>
                        <asp:DropDownList ID="PurchaseMode" runat="server" EnableViewState="false" TabIndex="11">
                        </asp:DropDownList>
                        <label for="FileUpload1">
                            <%=Resources.Controls.Attachments%>
                        </label>
                        <asp:FileUpload ID="FileUpload1" runat="server" class="multi" accept="gif|jpg" abcd="2"
                            EnableViewState="false" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload All" Width="100px" Height="20px"
                            OnClick="btnUpload_Click" />
                    </div>
                    <div class="clear">
                    </div>
                    <div class="divcol-S">
                        <label for="Comments">
                            <%=Resources.Controls.Remarks%>
                        </label>
                        <asp:TextBox runat="server" ID="Comments" TextMode="MultiLine" EnableViewState="false"
                            EnableTheming="false" TabIndex="16">
                        </asp:TextBox>
                    </div>
                    <asp:HiddenField ID="FileList" runat="server" />
                </div>
                <div id="Maintanace">
                    <div class="div2col-S">
                        <label for="MaintenanceType">
                            <%=Resources.Controls.TypeOfMaintenance%>
                        </label>
                        <asp:DropDownList ID="MaintenanceType" runat="server" TabIndex="17" EnableViewState="false">
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                        <label for="Freequency">
                            <%=Resources.Controls.Freequency%>
                        </label>
                        <asp:DropDownList ID="Freequency" runat="server" TabIndex="18" onChange="FillMaintanceDuration();"
                            EnableViewState="false">
                        </asp:DropDownList>
                    </div>
                    <div class="div2col-S" style="float: right; margin-right: 0">
                        <div id="maintenceFrom">
                            <label for="MaintanceFromDate">
                                <%=Resources.Controls.FromDate%>
                            </label>
                            <asp:TextBox ID="MaintanceFromDate" runat="server" TabIndex="19" EnableViewState="false">
                            </asp:TextBox>
                            <asp:HiddenField ID="hdnMaintanceFromDate" runat="server" />
                        </div>
                        <div id="maintenceTo">
                            <label for="MaintanceToDate">
                                <%=Resources.Controls.ToDate%>
                            </label>
                            <asp:TextBox ID="MaintanceToDate" runat="server" TabIndex="20" EnableViewState="false">
                            </asp:TextBox>
                            <asp:HiddenField ID="hdnMaintanceToDate" runat="server" />
                        </div>
                        <div id="maintenceDay">
                            <label for="FromTime">
                                <%=Resources.Controls.FromTime%>
                            </label>
                            <asp:TextBox ID="FromTime" runat="server" TabIndex="21" EnableViewState="false">
                            </asp:TextBox>
                            <label for="ToTime">
                                <%=Resources.Controls.ToTime%>
                            </label>
                            <asp:TextBox ID="ToTime" runat="server" TabIndex="22" EnableViewState="false">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <asp:Button runat="server" ID="btnAddMaintance" Text="<%$ Resources:Controls, Add%>"
                        Width="50px" Height="20px" OnClientClick="javascript:return AddMaintanceDtls();"
                        EnableViewState="false" />
                    <div class="grdTable">
                        <table rules="all" id="grdMaintanaceDetails" grandtype="GrandGrid" paging="false"  
                            editfunction="GridAction" ajaxurl="" editable="true">
                            <thead>
                                <tr>
                                    <th fieldmap="MaintenaceInfoID" isvisible="false">
                                    </th>
                                    <th fieldmap="MaintenanceType" isvisible="false">
                                    </th>
                                    <th fieldmap="Freequency" isvisible="false">
                                    </th>
                                    <th fieldmap="MaintenanceTypeName" align="left" width="25%">
                                        <%=Resources.Controls.TypeOfMaintenance%>
                                    </th>
                                    <th fieldmap="FreequencyName" align="left" width="25%">
                                        <%=Resources.Controls.Freequency%>
                                    </th>
                                    <th fieldmap="MaintanceFromDate" align="left" width="20%">
                                        <%=Resources.Controls.FromDateTime%>
                                    </th>
                                    <th fieldmap="MaintanceToDate" align="left" width="20%">
                                        <%=Resources.Controls.ToDateTime%>
                                    </th>
                                    <th type="Template" width="10%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="imbMaintnceEdit" SkinID="imbeditgrid" Width="16px"
                                                EnableViewState="false" Height="16px" OnClientClick="javascript:return MainteanceGridHandler($(this).parents('tr:eq(0)'),'EditMaintance')" />
                                            <asp:ImageButton runat="server" ID="imbMaintnceDelete" SkinID="imbdeletegrid" Width="16px"
                                                EnableViewState="false" Height="16px" OnClientClick="javascript:return MainteanceGridHandler($(this).parents('tr:eq(0)'),'DeleteMaintance')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <div id="Performance">
                    <div class="div2col-S">
                        <label for="Throughput">
                            <%=Resources.Controls.Throughput%></label>
                        <asp:TextBox ID="Throughput" runat="server" TabIndex="23" EnableViewState="false">
                        </asp:TextBox>
                        <label for="AvgConsumption">
                            <%=Resources.Controls.AvgConsumption%></label>
                        <asp:TextBox ID="AvgConsumption" runat="server" Width="60px" TabIndex="25" EnableViewState="false">
                        </asp:TextBox>
                        <asp:DropDownList ID="AvgConsumptionUOM" runat="server" Width="70px" TabIndex="26"
                            EnableViewState="false">
                            <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="div2col-S" style="float: right; margin-right: 0">
                        <label for="RunBy">
                            <%=Resources.Controls.RunBy%></label>
                        <asp:DropDownList ID="RunBy" runat="server" TabIndex="24" onChange="FillConsumptionUOM();"
                            EnableViewState="false">
                        </asp:DropDownList>
                        <label for="MaxContinousUsage">
                            <%=Resources.Controls.MaxContinuousUsage%></label>
                        <asp:TextBox ID="MaxContinousUsage" runat="server" Width="60px" TabIndex="27" EnableViewState="false">
                        </asp:TextBox>
                        <asp:DropDownList ID="MaxContinuousUOM" runat="server" Width="70px" TabIndex="28"
                            EnableViewState="false">
                            <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>
                            <asp:ListItem Value="1">Hrs</asp:ListItem>
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="divListing" class="grdTable">
            <div id="searchwrap">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="30"
                    EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="TmmEsec" Text="<%$ Resources:Controls, MachineType%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MctNem" Text="<%$ Resources:Controls, MachineName%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Mctcd" Text="<%$ Resources:Controls, MachineCode%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="SearchValue" runat="server" TabIndex="31" EnableViewState="false">
                </asp:TextBox>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="btnsearchgo" Width="30px"
                    EnableViewState="false" TabIndex="32" Height="20px" />
            </div>
            <div class="clear">
            </div>
            <table rules="all" id="grdMachineDetails" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineList"
                pagesize="10" paging="true" editfunction="GridAction" editable="true">
                <thead>
                    <tr>
                        <th fieldmap="Mctp" isvisible="false">
                        </th>
                        <th fieldmap="Mctcd" sortable="true" align="left" width="15" >
                            <%=Resources.Controls. MachineCode%>
                        </th>
                        <th fieldmap="MctNem" sortable="true" align="left" width="15">
                            <%=Resources.Controls.MachineName%>
                        </th>
                        <th fieldmap="TmmEsec" sortable="true" align="left" width="15">
                            <%=Resources.Controls.MachineType%>
                        </th>
                        <th fieldmap="ClpNem" sortable="true" align="left" width="15">
                            <%=Resources.Controls.Location%>
                        </th>
                        <th type="Template">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbEditMachineDtls" SkinID="imbeditgrid" Width="16px"
                                    Height="16px" OnClientClick="javascript:return MachineGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                <asp:ImageButton runat="server" ID="imbDeleteMachineDtls" SkinID="imbdeletegrid"
                                    Width="16px" Height="16px" OnClientClick="javascript:return MachineGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div id="divMachineType" style="margin-top: 25px" title="Machine Type">
        <div style="width: 120px; height: 20px; float: left">
            <label for="">
                <%=Resources.Controls.MachineType%>
            </label>
          <%--  <asp:Label ID="lblstarmachine" runat="server" Text="*" Style="color: Red; display: none;"
                EnableViewState="false"></asp:Label>--%>
        </div>
        <div style="float: left;">
            <asp:TextBox runat="server" ID="MachineTypeName" MaxLength="100" EnableViewState="false">
            </asp:TextBox><asp:HiddenField ID="MachineTypePK" runat="server" Value="0" />
        </div>
        <div style="float: left; margin-left: 2px">
            <asp:Button runat="server" ID="btnAdd" Text="<%$ Resources:Controls, Save%>" Width="50px"
                EnableViewState="false" Height="20px" OnClientClick="SaveMachineType()" />
        </div>
        <div class="clear">
        </div>
        <%--Machine Type Details--%>
        <div class="grdTable">
            <table rules="all" id="grdMachineTypeDtls" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineTypeDetails"
                pagesize="5" paging="true" editfunction="GridAction" editable="true">
                <thead>
                    <tr>
                        <th fieldmap="Tmmp" isvisible="false">
                        </th>
                        <th fieldmap="TmmEsec" sortable="true" align="left" align="left" width="75">
                            <%=Resources.Controls.MachineType%>
                        </th>
                        <th type="Template" width="25">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbMachineEdit" SkinID="imbeditgrid" OnClientClick="javascript:return MachineTypeGridHandler($(this).parents('tr:eq(0)'),'editmachinetype')" />
                                <asp:ImageButton runat="server" ID="imbMachineDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return MachineTypeGridHandler($(this).parents('tr:eq(0)'),'deletemachinetype')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div id="LocationDiv" style="width: 300px; margin-top: 25px" title="Location">
        <div style="width: 70px; height: 20px; float: left">
            <label for="LocationName">
                <%=Resources.Controls.Location%>
            </label>
           <%-- <asp:Label ID="lblstarloc" runat="server" Text="*" Style="color: Red; display: none;"
                EnableViewState="false"></asp:Label>--%>
        </div>
        <div style="float: left;">
            <asp:TextBox runat="server" ID="LocationName" MaxLength="100" EnableViewState="false">
            </asp:TextBox>
            <asp:HiddenField ID="LocationPK" runat="server" Value="0" />
        </div>
        <div style="float: left; margin-left: 2px">
            <asp:Button runat="server" ID="AddLocation" Text="<%$ Resources:Controls, Save%>"
                EnableViewState="false" EnableTheming="false" CssClass="inputbtn" Width="50px"
                Height="20px" OnClientClick="SaveLocation();" />
        </div>
        <div class="clear">
        </div>
        <%--Location Details--%>
        <div class="grdTable">
            <table rules="all" id="grdLocationDtls" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetLocationDetails"
                pagesize="5" paging="true" editfunction="GridAction" editable="true">
                <thead>
                    <tr>
                        <th fieldmap="ClpPK" isvisible="false">
                        </th>
                        <th fieldmap="ClpNem" sortable="true" align="left" width="75">
                            <%=Resources.Controls.Location%>
                        </th>
                        <th type="Template" align="center" width="25">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbLocEdit" SkinID="imbeditgrid" OnClientClick="javascript:return LocationGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                <asp:ImageButton runat="server" ID="imbLocDel" SkinID="imbdeletegrid" OnClientClick="javascript:return LocationGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div id="divVendor" title="Create Vendor">
        <div class="divcol-M" style="width: 95%">
            <label for="VendorName">
                <%=Resources.Controls.VendorName%></label>
            <asp:TextBox ID="VendorName" runat="server" MaxLength="200" EnableViewState="false">
            </asp:TextBox>
            <asp:Label ID="lblstarVendorName" runat="server" Text="*" Style="color: Red; display: none;"
                EnableViewState="false"></asp:Label>
            <label for="ContactName">
                <%=Resources.Controls.ContactName%></label>
            <asp:TextBox ID="ContactName" runat="server" MaxLength="200" EnableViewState="false">
            </asp:TextBox>
            <div class="clear">
            </div>
            <label for="Address">
                <%=Resources.Controls.Address%>
            </label>
            <asp:TextBox ID="Address" runat="server" TextMode="MultiLine" EnableTheming="false"
                EnableViewState="false" MaxLength="500">
            </asp:TextBox>
            <div class="clear">
            </div>
            <label for="VendorPhone">
                <%=Resources.Controls.Phone%></label>
            <asp:TextBox ID="VendorPhone" runat="server" MaxLength="23" EnableViewState="false">
            </asp:TextBox>
            <div class="clear">
            </div>
            <label for="Email">
                <%=Resources.Controls.Email%>
            </label>
            <asp:TextBox ID="Email" runat="server" MaxLength="90" EnableViewState="false">
            </asp:TextBox>
            <asp:HiddenField ID="vendorPK" runat="server" Value="0" />
            <div style="float: right">
                <asp:Button runat="server" ID="btnVendorSave" Text="<%$ Resources:Controls, Save%>"
                    EnableViewState="false" EnableTheming="false" CssClass="inputbtn" Width="50px"
                    Height="20px" OnClientClick="SaveVendor()" />
                <div class="clear">
                </div>
            </div>
        </div>
        <%--Vendor Details--%>
        <div class="grdTable">
            <table rules="all" id="grdVendorDetails" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetVendorDetails"
                pagesize="5" paging="true" editfunction="GridAction" editable="true">
                <thead>
                    <tr>
                        <th fieldmap="DndPK" isvisible="false">
                        </th>
                        <th fieldmap="DndNem" sortable="true" align="left" width="15">
                            <%=Resources.Controls.VendorName%>
                        </th>
                        <th fieldmap="DndNemC" sortable="true" align="left" width="15">
                            <%=Resources.Controls.ContactName%>
                        </th>
                        <th fieldmap="DndSrda1" sortable="true" align="left" width="15">
                            <%=Resources.Controls.Address%>
                        </th>
                        <th fieldmap="DndNph" sortable="true" align="left" width="15">
                            <%=Resources.Controls.Phone%>
                        </th>
                        <th fieldmap="DndELim" isvisible="false" align="left" width="5">
                            <%=Resources.Controls.Email%>
                        </th>
                        <th type="Template">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" EnableViewState="false"
                                    OnClientClick="javascript:return VendorGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" EnableViewState="false"
                                    OnClientClick="javascript:return VendorGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <!-- Stores the Maintenace Json Object (BusinessObject.MachineManagement.MachineMaster) -->
    <asp:HiddenField runat="server" ID="MaintenaceList" />
    <asp:HiddenField runat="server" ID="EditMaintance" Value="0" />
    <asp:HiddenField runat="server" ID="MachineID" Value="0" />
    <div id="divDatas">
    </div>
</asp:Content>
