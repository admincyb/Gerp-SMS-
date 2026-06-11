<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="Test.aspx.cs" Inherits="ERPSMS_v01.Test" Theme="ClassicExt" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link href="styles.css" rel="stylesheet" />
	<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
		<script type="text/javascript">
			var radWindow1 = null;
			var radGrid1 = null;
			var panelStep1 = null;
			var panelStep2 = null;

			<%--function pageLoad() {
				radGrid1 = $find("<%= RadGrid1.ClientID %>");
				radWindow1 = $find("<%= RadWindow1.ClientID %>");
				panelStep1 = $get("<%= FirstStepPanel.ClientID %>");
				panelStep2 = $get("<%= SecondStepPanel.ClientID %>");
			}--%>
		</script>
	</telerik:RadCodeBlock>
	<script type="text/javascript" src="scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <iframe width="1140" height="541.25" src="https://app.powerbi.com/reportEmbed?reportId=e330c2a6-f656-4627-a66a-60006a1d9d2c&autoAuth=true&ctid=604892ff-9bb0-499e-a6bd-38d7df6ae50a&config=eyJjbHVzdGVyVXJsIjoiaHR0cHM6Ly93YWJpLWluZGlhLWNlbnRyYWwtYS1wcmltYXJ5LXJlZGlyZWN0LmFuYWx5c2lzLndpbmRvd3MubmV0LyJ9" frameborder="0" allowFullScreen="true"></iframe>

      <%-- <rsweb:ReportViewer ID="rvViewReport" runat="server" ProcessingMode="Remote"
                            BorderWidth="0" SizeToReportContent="true"  Width="100%">
           <ServerReport ReportServerUrl="https://app.powerbi.com/reportEmbed?reportId=e330c2a6-f656-4627-a66a-60006a1d9d2c&autoAuth=true&ctid=604892ff-9bb0-499e-a6bd-38d7df6ae50a&config=eyJjbHVzdGVyVXJsIjoiaHR0cHM6Ly93YWJpLWluZGlhLWNlbnRyYWwtYS1wcmltYXJ5LXJlZGlyZWN0LmFuYWx5c2lzLndpbmRvd3MubmV0LyJ9"
               ReportPath="/local" />
                        </rsweb:ReportViewer>--%>

    <%--<telerik:RadScriptManager runat="server" ID="RadScriptManager1" />--%>
    <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />--%>
	<%--<div class="demo-container" id="demo-container">
		<div class="gridPositioning">--%>
			<%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" Visible="false">
				<AjaxSettings>
					<telerik:AjaxSetting AjaxControlID="BookNowCloseButton">
						<UpdatedControls>
							<telerik:AjaxUpdatedControl ControlID="RadGrid1" />
						</UpdatedControls>
					</telerik:AjaxSetting>
					<telerik:AjaxSetting AjaxControlID="SaveSettingsButton">
						<UpdatedControls>
							<telerik:AjaxUpdatedControl ControlID="RadGrid1" />
						</UpdatedControls>
					</telerik:AjaxSetting>
					<telerik:AjaxSetting AjaxControlID="RadGrid1">
						<UpdatedControls>
							<telerik:AjaxUpdatedControl ControlID="RadGrid1" />
						</UpdatedControls>
					</telerik:AjaxSetting>
				</AjaxSettings>
			</telerik:RadAjaxManager>
			<telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server" StorageProviderKey="OverviewDemoStorage">
				<PersistenceSettings>
					<telerik:PersistenceSetting ControlID="RadGrid1" />
				</PersistenceSettings>
			</telerik:RadPersistenceManager>
			<div class="gridHeader">
				<div></div>
				<telerik:RadButton RenderMode="Lightweight" ID="SaveSettingsButton" runat="server" Skin="" Text="Save"
					CssClass="saveButton" OnClick="SaveSettingsButton_Click" />
				<telerik:RadButton RenderMode="Lightweight" ID="LoadSettingsButton" runat="server" Skin="" Text="Load"
					CssClass="loadButton" OnClick="LoadSettingsButton_Click" />
			</div>
			<div>
				<telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True"
					OnNeedDataSource="RadGrid1_NeedDataSource" AllowFilteringByColumn="True"
					OnItemCommand="RadGrid1_ItemCommand" CellSpacing="0" GridLines="None">
					<GroupingSettings CaseSensitive="false" />
					<MasterTableView AutoGenerateColumns="false" TableLayout="Fixed">
						<%--<ColumnGroups >
							<telerik:GridColumnGroup Name="GeneralInformation" HeaderText="General Information"
								HeaderStyle-HorizontalAlign="Center" />
							<telerik:GridColumnGroup Name="SpecificInformation" HeaderText="Specific Information"
								HeaderStyle-HorizontalAlign="Center" />
							<telerik:GridColumnGroup Name="BookingInformation" HeaderText="Booking Information"
								HeaderStyle-HorizontalAlign="Center" />
						</ColumnGroups>--%>
					<%--	<HeaderStyle Width="102px" />
						<Columns>
							<telerik:GridBoundColumn DataField="BrandName" HeaderText="Brand Name" UniqueName="BrandName"
								ColumnGroupName="GeneralInformation">
								<HeaderStyle Width="170px" />
								<FilterTemplate>
									<telerik:RadComboBox RenderMode="Lightweight" ID="BrandNameCombo"  DataTextField="BrandName"
										OnDataBound="BrandNameCombo_DataBound" DataValueField="BrandName"
										Height="200px" AppendDataBoundItems="true" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("BrandName").CurrentFilterValue %>'
										runat="server" OnClientSelectedIndexChanged="BrandNameComboIndexChanged">
										<Items>
											<telerik:RadComboBoxItem Text="Select a Brand" />
										</Items>
									</telerik:RadComboBox>
									<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
										<script type="text/javascript">
											function BrandNameComboIndexChanged(sender, args) {
												var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
											tableView.filter("BrandName", args.get_item().get_value(), "EqualTo");
										}
										</script>
									</telerik:RadScriptBlock>
								</FilterTemplate>
							</telerik:GridBoundColumn>
							<telerik:GridBoundColumn DataField="Model" HeaderText="Model" UniqueName="Model"
								ColumnGroupName="GeneralInformation" FilterControlWidth="60px">
								<HeaderStyle Width="115px" />
							</telerik:GridBoundColumn>
							<telerik:GridBoundColumn DataField="Classification" HeaderText="Classification" UniqueName="Classification"
								ColumnGroupName="GeneralInformation">
								<FilterTemplate>
									<telerik:RadComboBox RenderMode="Lightweight" ID="ClassificationCombo" Width="90px" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("Classification").CurrentFilterValue %>'
										runat="server" OnClientSelectedIndexChanged="ClassificationComboIndexChanged">
										<Items>
											<telerik:RadComboBoxItem Text="All" Value="" />
											<telerik:RadComboBoxItem Text="Hatchback" Value="Hatchback" />
											<telerik:RadComboBoxItem Text="Sedan" Value="Sedan" />
											<telerik:RadComboBoxItem Text="SUV" Value="SUV" />
											<telerik:RadComboBoxItem Text="MPV" Value="MPV" />
										</Items>
									</telerik:RadComboBox>
									<telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
										<script type="text/javascript">
											function ClassificationComboIndexChanged(sender, args) {
												var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
											tableView.filter("Classification", args.get_item().get_value(), "EqualTo");
										}
										</script>
									</telerik:RadScriptBlock>
								</FilterTemplate>
							</telerik:GridBoundColumn>
							<telerik:GridBoundColumn DataField="Transmission" HeaderText="Transmission" UniqueName="Transmission"
								ColumnGroupName="GeneralInformation">
								<FilterTemplate>
									<telerik:RadComboBox RenderMode="Lightweight" ID="TransmissionCombo" Width="90px" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("Transmission").CurrentFilterValue %>'
										runat="server" OnClientSelectedIndexChanged="TransmissionComboIndexChanged">
										<Items>
											<telerik:RadComboBoxItem Text="All" Value="" />
											<telerik:RadComboBoxItem Text="Manual" Value="Manual" />
											<telerik:RadComboBoxItem Text="Automatic" Value="Automatic" />
										</Items>
									</telerik:RadComboBox>
									<telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
										<script type="text/javascript">
											function TransmissionComboIndexChanged(sender, args) {
												var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
											tableView.filter("Transmission", args.get_item().get_value(), "EqualTo");
										}
										</script>
									</telerik:RadScriptBlock>
								</FilterTemplate>
							</telerik:GridBoundColumn>
							<telerik:GridBoundColumn DataField="Year" HeaderText="Year" UniqueName="Year" ColumnGroupName="SpecificInformation" FilterControlWidth="55px">
								<HeaderStyle Width="110px" />
							</telerik:GridBoundColumn>
							<telerik:GridBoundColumn DataField="Fuel" HeaderText="Fuel" UniqueName="Fuel" ColumnGroupName="SpecificInformation">
								<FilterTemplate>
									<telerik:RadComboBox RenderMode="Lightweight" ID="FuelCombo" Width="90px" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("Fuel").CurrentFilterValue %>'
										runat="server" OnClientSelectedIndexChanged="FuelComboIndexChanged">
										<Items>
											<telerik:RadComboBoxItem Text="All" Value="" />
											<telerik:RadComboBoxItem Text="Diesel" Value="Diesel" />
											<telerik:RadComboBoxItem Text="Gasoline" Value="Gasoline" />
											<telerik:RadComboBoxItem Text="Hybrid" Value="Hybrid" />
										</Items>
									</telerik:RadComboBox>
									<telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
										<script type="text/javascript">
											function FuelComboIndexChanged(sender, args) {
												var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
											tableView.filter("Fuel", args.get_item().get_value(), "EqualTo");
										}
										</script>
									</telerik:RadScriptBlock>
								</FilterTemplate>
							</telerik:GridBoundColumn>
							<telerik:GridNumericColumn DataField="Price" HeaderText="Price" UniqueName="Price"
								ColumnGroupName="BookingInformation" DataFormatString="<strong>&#8364; {0}</strong>"
								AllowFiltering="false" HeaderStyle-Width="80px" />
							<telerik:GridTemplateColumn HeaderText="Book" ColumnGroupName="BookingInformation"
								AllowFiltering="false">
								<HeaderStyle Width="102px" />
								<ItemTemplate>
									<asp:LinkButton ID="BookButton" runat="server" Text="Book Now" OnClientClick='<%# String.Format("openConfirmationWindow({0}); return false;", Eval("CarID")) %>'
										CssClass="bookNowLink" />
								</ItemTemplate>
							</telerik:GridTemplateColumn>
						</Columns>
						<NestedViewTemplate>
							<div class="carBackground" style='<%# NormalizeValue(String.Format("background-image: url(Images/LargeLogos/{0}.png);", Eval("BrandName"))) %>'>
								<div style="float: left;">
									<asp:Image ID="CarImage" runat="server" AlternateText="Car Image" ImageUrl='<%# GetCarImageUrl(Container)%>' />
								</div>
								<div style="float: right; width: 50%">
									<div class="carTitle">
										<%# Eval("BrandName") %>
										<%# Eval("Model") %>
										<span style="color: #555555; float: right; font-size: 14px; font-weight: normal;">Rented
										<%# Eval("RentedCount") %>
										times</span>
									</div>
									<hr class="lineSeparator" />
									<table width="100%" class="carInfo">
										<tr>
											<td>
												<strong>Year:</strong>
												<%# Eval("Year") %>
											</td>
										</tr>
										<tr>
											<td>
												<strong>Classification:</strong>
												<%# Eval("Classification") %>
											</td>
										</tr>
										<tr>
											<td>
												<strong>Transmission:</strong>
												<%# Eval("Transmission") %>
											</td>
										</tr>
										<tr>
											<td>
												<strong>Fuel Type:</strong>
												<%# Eval("Fuel") %>
											</td>
										</tr>
										<tr>
											<td>
												<strong>Price:</strong> &#8364;<%# Eval("Price") %>
											</td>
										</tr>
									</table>
								</div>
								<div style="clear: both">
								</div>
							</div>
						</NestedViewTemplate>
						<PagerStyle PageSizes="5,10" PagerTextFormat="{4}<strong>{5}</strong> cars matching your search criteria"
							PageSizeLabelText="Cars per page:" />
					</MasterTableView>
					<ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true">
						<Selecting AllowRowSelect="true" EnableDragToSelectRows="false" />
						<Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
					</ClientSettings>
				</telerik:RadGrid>
			</div>
			<telerik:RadWindow RenderMode="Lightweight" ID="RadWindow1" runat="server" VisibleTitlebar="false" Modal="true" Width="550px" Height="380px"
				Behaviors="None" VisibleStatusbar="false">
				<ContentTemplate>
					<asp:Panel ID="FirstStepPanel" runat="server">
						<div class="bookNowFrame">
							<div class="bookNowTitle">
								Fill in the form to make your reservation
							</div>
							<hr class="lineSeparator" style="margin: 12px 0 12px 0" />
							<table cellspacing="8">
								<colgroup>
									<col width="90px" />
									<col width="150px" />
									<col />
									<col />
								</colgroup>
								<tr>
									<td>DATE OF RENT
									</td>
									<td>
										<telerik:RadDatePicker RenderMode="Lightweight" ID="DateOfRentPicker" Width="130px" runat="server" />
									</td>
									<td>RETURN DATE
									</td>
									<td>
										<telerik:RadDatePicker RenderMode="Lightweight" ID="ReturnDatePicker" Width="130px" runat="server" />
									</td>
								</tr>
							</table>
							<hr class="lineSeparator" style="margin: 12px 0 12px 0" />
							<table cellspacing="8">
								<colgroup>
									<col width="90px" />
									<col />
								</colgroup>
								<tr>
									<td>FIRST NAME
									</td>
									<td>
										<telerik:RadTextBox RenderMode="Lightweight" ID="FirstNameTextBox" runat="server" Width="190px" /><br />
									</td>
								</tr>
								<tr>
									<td>LAST NAME
									</td>
									<td>
										<telerik:RadTextBox RenderMode="Lightweight" ID="LastNameTextBox" runat="server" Width="190px" /><br />
									</td>
								</tr>
								<tr>
									<td>EMAIL
									</td>
									<td>
										<telerik:RadTextBox RenderMode="Lightweight" ID="EmailTextBox" runat="server" Width="190px" /><br />
									</td>
								</tr>
							</table>
							<hr class="lineSeparator" style="margin: 12px 0 15px 0" />
							<telerik:RadButton RenderMode="Lightweight" ID="BookNowButton" runat="server" Text="Book Now"
								Width="100px" OnClientClicking="bookNowClicking" UseSubmitBehavior="false" />
							<telerik:RadButton RenderMode="Lightweight" ID="CancelButton" runat="server" Text="Cancel"
								Width="100px" OnClientClicking="cancelClicking" UseSubmitBehavior="false" />
						</div>
					</asp:Panel>
					<asp:Panel ID="SecondStepPanel" runat="server" Style="display: none; padding: 120px 20px 0 30px; width: 480px;">
						<div style="float: left;">
							<img src="Images/Confirmation.png" alt="Confirmation sign" />
						</div>
						<div style="float: left; padding: 10px 0 0 20px;">
							<div class="bookNowComplete">
								You have successfully made your reservation!
							</div>
							<hr class="lineSeparator" style="margin: 10px 10px 20px 0" />
							<telerik:RadButton RenderMode="Lightweight" ID="BookNowCloseButton" runat="server" Text="Close"
								Width="100px" OnClientClicking="bookNowCloseClicking" UseSubmitBehavior="false" />
						</div>
						<div style="clear: both">
						</div>
					</asp:Panel>
				</ContentTemplate>
			</telerik:RadWindow>--%>
		</div>
	</div>--%>
      
   
</asp:Content>

