<%@ Page Title="<%$ Resources:Captions,Title_ProductMaster %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="ProductMaster.aspx.cs" Inherits="ERPSMS_v01.Inventory.Masters.ProductMaster"
    ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/Inventory/Masters/UserControls/SearchProducts.ascx" TagName="SearchProducts"
    TagPrefix="uc2" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
          
            var statusval = ($("[id$='chkProdAuto']").is(':checked') == true ? "1" : "0");
            if ($("[id$='hdfIsBrandProduct']").val() == "1") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtAdvProduct", url + "&SearchBy=ITM_TEXT&Status=" + statusval, "hdfProductPK", true, true, "GETBRANDPRODUCTS");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtAdvProduct", url + "&Type=" + statusval, "hdfProductPK", true, true, "GETPRODUCTS");
            }

            $("[id$='txtProductCode']").attr("disabled", true);
            $("[id$='txtProductCode']").addClass("input-disabled");
            $("[id$='chbAutoGenerate']").attr("checked", true);
            $("[id$='txtWeight']").ForceNumericOnly();
            $("[id$='txtMinWeight']").ForceNumericOnly();
            $("[id$='txtMaxWeight']").ForceNumericOnly();
            $("[id*=txtCount]").ForceNumericOnly();
            $("[id*=txtPackMatCount]").ForceNumericOnly();
            ShowHideCombination(1);
            GrandScriptUtils.MakeAutoCompleteDDL("txtSearchBy", url + "&Type=" + $("[id$=ddlFilterBy]").val(), "hdfSearch", false, false, "PRODUCTMASTER");
            // GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url + "?Type=ITM_NAME", "hdfItemID", true, true, "GETALLITEMS", "", false, false, false, 3, '<%= GetLocalResourceObject("TypeMin3") %>');
            GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url, "hdfItemID", true, true, "GETPRODUCTS", "", false, false, false, 3, '<%= GetLocalResourceObject("TypeMin3") %>');
            if ($("[id$=hdfProdSpecCompare]").val() == "1")
                GrandScriptUtils.MakeAutoCompleteDDL("txtRelatedItem", url + "&BinSubType=" + $("[id$=ddlSubType]").val() + "&itemPK=" + $("[id$=hdfRelItemPk]").val(), "hdfRelatedItem", true, true, "GETRELATEDPRODUCTS", "", false, false, false, 0, '<%= GetLocalResourceObject("TypeMin3") %>');
            else
                GrandScriptUtils.MakeAutoCompleteDDL("txtRelatedItem", url + "&BinSubType=" + $("[id$=ddlSubType]").val() + "&itemPK=0", "hdfRelatedItem", true, true, "GETRELATEDPRODUCTS", "", false, false, false, 0, '<%= GetLocalResourceObject("TypeMin3") %>');
            GrandScriptUtils.MakeAutoCompleteDDL("txtInventoryProd", url, "hdfInventoryProdPK", true, true, "GETGLOVEITEMS");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtProductGroup", url, "hdfProductGroupPk", true, true, "GETPRODUCTGROUPS");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProductGroup", url, "hdfProductGroupPk", true, true, "GETPRODUCTGROUPS", "", false, false, false, 3, "Type min. 3 characters");
            FillMaterialCategoryAutoComplete();
            FillMaterialAutoComplete();
            CheckProductCode();
            if ($("[id$=txtItem]").val() == '<%= GetLocalResourceObject("TypeMin3").ToString() %>') {
                $("[id$=txtItem]").addClass("grayText");
            }
            else {
                $("[id$=txtItem]").removeClass("grayText");
            }
            if ($("[id$=txtRelatedItem]").val() == '<%= GetLocalResourceObject("TypeMin3").ToString() %>') {
                $("[id$=txtRelatedItem]").addClass("grayText");
            }
            else {
                $("[id$=txtRelatedItem]").removeClass("grayText");
            }
        }

        $(document).ready(function () {
            $("[id$=ddlFilterBy]").live("change", function () {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchBy", url + "&Type=" + $(this).val(), "hdfSearch", false, false, "PRODUCTMASTER");
                $("[id$=txtSearchBy]").val("");
            });
            $("[id$=ddlSubType]").live("change", function () {
                if ($("[id$=hdfProdSpecCompare]").val() == "1")
                    GrandScriptUtils.MakeAutoCompleteDDL("txtRelatedItem", url + "&BinSubType=" + $("[id$=ddlSubType]").val() + "&itemPK=" + $("[id$=hdfRelItemPk]").val(), "hdfRelatedItem", true, true, "GETPRODUCTS", "", false, false, false, 0, '<%= GetLocalResourceObject("TypeMin3") %>');
                else
                    GrandScriptUtils.MakeAutoCompleteDDL("txtRelatedItem", url + "&BinSubType=" + $("[id$=ddlSubType]").val() + "&itemPK=0", "hdfRelatedItem", true, true, "GETPRODUCTS", "", false, false, false, 0, '<%= GetLocalResourceObject("TypeMin3") %>');
                $("[id$=txtRelatedItem]").val('<%= GetLocalResourceObject("TypeMin3").ToString() %>');
                $("[id$=txtRelatedItem]").addClass("grayText");
            });
        });
        function ProductAuto() {
            var statusval = ($("[id$='chkProdAuto']").is(':checked') == true ? "1" : "0");
            // GrandScriptUtils.MakeAutoCompleteDDL("txtAdvProduct", url + "&Type=" + statusval, "hdfProductPK", true, true, "GETPRODUCTS");

            if ($("[id$='hdfIsBrandProduct']").val() == "1") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtAdvProduct", url + "&SearchBy=ITM_TEXT&Status=" + statusval, "hdfProductPK", true, true, "GETBRANDPRODUCTS");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtAdvProduct", url + "&Type=" + statusval, "hdfProductPK", true, true, "GETPRODUCTS");
            }

        }
        function ShowListing(flag) {
            /// flag = 0 List tab
            /// flag = 1 Details tab
            /// flag = 2 Related products tab
            /// flag = 3 Sub grade products tab
            /// flag = 5 Line tab
            if (flag == 1) {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='PageAction_SubPrdts']").hide();
                $("[id$='PageAction_RelatedPrdts']").hide();
                $("[id$='PageAction_PackingMaterials']").hide();
                $("[id$='PageAction_Lines']").hide();
                $("[id$='PageAction_StoreMapping']").hide();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            else if (flag == 2) {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_SubPrdts']").hide();
                $("[id$='PageAction_RelatedPrdts']").show();
                $("[id$='PageAction_PackingMaterials']").hide();
                $("[id$='PageAction_Lines']").hide();
                $("[id$='PageAction_StoreMapping']").hide();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();

            }
            else if (flag == 3) {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_SubPrdts']").show();
                $("[id$='PageAction_RelatedPrdts']").hide();
                $("[id$='PageAction_PackingMaterials']").hide();
                $("[id$='PageAction_Lines']").hide();
                $("[id$='PageAction_StoreMapping']").hide();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();

            }
            //            Packing Material
            else if (flag == 4) {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_SubPrdts']").hide();
                $("[id$='PageAction_RelatedPrdts']").hide();
                $("[id$='PageAction_Lines']").hide();
                $("[id$='PageAction_StoreMapping']").hide();
                $("[id$='PageAction_PackingMaterials']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();

            }
            //            Line
            else if (flag == 5) {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_SubPrdts']").hide();
                $("[id$='PageAction_RelatedPrdts']").hide();
                $("[id$='PageAction_PackingMaterials']").hide();
                $("[id$='PageAction_Lines']").show();
                $("[id$='PageAction_StoreMapping']").hide();
                $("[id$='pnlListing']").hide();
                $("[id$='ModifiedDatePnl']").hide();
                $("[id$='pnlEntry']").show();

            }
            //            Store Mapping
            else if (flag == 6) {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_SubPrdts']").hide();
                $("[id$='PageAction_RelatedPrdts']").hide();
                $("[id$='PageAction_PackingMaterials']").hide();
                $("[id$='PageAction_Lines']").hide();
                $("[id$='PageAction_StoreMapping']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='ModifiedDatePnl']").hide();
                $("[id$='pnlEntry']").show();

            }
            else {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_SubPrdts']").hide();
                $("[id$='PageAction_RelatedPrdts']").hide();
                $("[id$='PageAction_PackingMaterials']").hide();
                $("[id$='PageAction_Lines']").hide();
                $("[id$='PageAction_StoreMapping']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            return false;
        }

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// Mode = 3 Indicates its on Related Tab
            /// Mode = 5 Indicates its on PACKINGMATERIALS Tab
            /// flag = 6 Line tab
            /// </param>
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlStoreSave']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                // $("[id$='chbAutoGenerate']").attr("checked", false);
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlStoreSave']").hide();
            }
            else if (mode == 3) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='pnlRelPrdtSave']").show();
                $("[id$='pnlRelPrdtDelete']").show();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlStoreSave']").hide();
            }
            else if (mode == 4) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").show();
                $("[id$='pnlSubPrdtDelete']").show();
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlStoreSave']").hide();
            }
            else if (mode == 5) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlPackMatSave']").show();
                $("[id$='pnlPackMatDelete']").show();
                $("[id$='pnlStoreSave']").hide();
            }
            else if (mode == 6) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").show();
                $("[id$='pnlStoreSave']").hide();
            }
                 else if (mode == 7) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlStoreSave']").show();
            }
            else {
                $("[id$='pnlRelPrdtSave']").hide();
                $("[id$='pnlRelPrdtDelete']").hide();
                $("[id$='pnlSubPrdtSave']").hide();
                $("[id$='pnlSubPrdtDelete']").hide();
                $("[id$='pnlPackMatSave']").hide();
                $("[id$='pnlPackMatDelete']").hide();
                $("[id$='pnlLineSave']").hide();
                $("[id$='pnlStoreSave']").hide();
            }
        }

        function SetTabs(tab) {
            /// flag = 0 List tab
            /// flag = 1 Details tab
            /// flag = 2 Related products tab
            /// flag = 3 Sub grade products tab
            /// flag = 4 Packing materials tab
            /// flag = 5 Line tab
            /// flag = 5 Store mapping tab
            if (tab == 1) {
                $("[id$='spnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnProductDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnRelatedProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnRelatedProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnLines']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbLine']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");

                $("[id$='spnPackingMaterials']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnPackingMaterials']").removeClass("tab-active").addClass("tab-inactive");
            }
            else if (tab == 2) {
                $("[id$='spnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductDetails']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnProductDetails']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnRelatedProducts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnRelatedProducts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnLines']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbLine']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");

                $("[id$='spnPackingMaterials']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnPackingMaterials']").removeClass("tab-active").addClass("tab-inactive");
            }
            else if (tab == 3) {
                $("[id$='spnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductDetails']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnProductDetails']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnRelatedProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnRelatedProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnLines']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbLine']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");

                $("[id$='spnPackingMaterials']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnPackingMaterials']").removeClass("tab-active").addClass("tab-inactive");
            }
            else if (tab == 4) {
                $("[id$='spnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnSubGradeProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnRelatedProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnRelatedProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnLines']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbLine']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");

                $("[id$='spnPackingMaterials']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnPackingMaterials']").removeClass("tab-inactive").addClass("tab-active");
            }
            else if (tab == 5) {
                $("[id$='spnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnSubGradeProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnRelatedProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnRelatedProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnPackingMaterials']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnPackingMaterials']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");

                $("[id$='spnLines']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbLine']").removeClass("tab-inactive").addClass("tab-active");
            }
            else if (tab == 6) {
                $("[id$='spnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnSubGradeProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnRelatedProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnRelatedProducts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnPackingMaterials']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnPackingMaterials']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-active");

                $("[id$='spnLines']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbLine']").removeClass("tab-active").addClass("tab-inactive");
            }

            else {
                $("[id$='spnProductListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnProductListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnProductDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnSubGradeProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnRelatedProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnRelatedProducts']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnLines']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbLine']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='spnStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbStoreMapping']").removeClass("tab-inactive").addClass("tab-inactive");

                $("[id$='spnPackingMaterials']").removeClass("tab-inactive").addClass("tab-inactive");
                $("[id$='lbnPackingMaterials']").removeClass("tab-inactive").addClass("tab-inactive");
            }
        }

        function AutoGenerateChecked() {
            if ($("[id$='chbAutoGenerate']").is(':checked') == true) {
                $("[id$='txtProductCode']").attr("disabled", true);
                $("[id$='txtProductCode']").addClass("input-disabled");
                GenerateProductCode();
            }
            else {
                $("[id$='txtProductCode']").attr("disabled", false);
                $("[id$='txtProductCode']").removeClass("input-disabled");
            }
        }

        function GenerateProductCode() {
            var pdtCode = "";
            var selectedText = "";
            var ddlCount = $("[id$='hdfDdlCount']").val();

            if ($("[id$='chbAutoGenerate']").is(':checked') == true) {
                for (var i = 1; i <= ddlCount; i++) {
                    if ($("[id$='ddl" + i + "']").val() != '-1') {
                        if ($("[id$='ddl" + i + "']").attr('canChangeProductCode') == '1') {
                            selectedText = $("[id$='ddl" + i + "'] :selected").text();
                            pdtCode = pdtCode + selectedText.substring(0, selectedText.indexOf("-", 1)); //Show First '-' Symbol
                        }
                    }
                }
                $("[id$='txtProductCode']").val(pdtCode);
            }

        }

        function CheckProductCode() {
            var pdtCode = "";
            var selectedText = "";
            var ddlCount = $("[id$='hdfDdlCount']").val();

            if ($("[id$='chbAutoGenerate']").is(':checked') == true) {
                for (var i = 1; i <= ddlCount; i++) {
                    if ($("[id$='ddl" + i + "']").val() != '-1') {
                        if ($("[id$='ddl" + i + "']").attr('canChangeProductCode') == '1') {
                            selectedText = $("[id$='ddl" + i + "'] :selected").text();
                            pdtCode = pdtCode + selectedText.substring(0, selectedText.indexOf("-"));
                        }
                    }
                }
            }
            if (pdtCode == $("[id$='txtProductCode']").val()) {
                $("[id$='chbAutoGenerate']").attr("checked", true);
            }
            else {
                $("[id$='chbAutoGenerate']").attr("checked", false);
            }

        }

        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false; //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsgRelPrdt]").hide();
                ShowErrorMessage($("#RelPrdtdiverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }

            return true;
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtItem") {
                $("[id$=txtItem]").removeClass("grayText");
            }
            else if (targetControlID == "txtRelatedItem") {
                $("[id$=txtRelatedItem]").removeClass("grayText");
            }
            else if (targetControlID == "txtMaterialCategory") {
                $("[id$=txtPackingMaterial]").val("Select/Type");
                $("[id$=hdnPackingMatID]").val(0);
                FillMaterialAutoComplete();
            }


            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtItem") {
                $("[id$=txtItem]").addClass("grayText");
            }
            else if (targetControlID == "txtRelatedItem") {
                $("[id$=txtRelatedItem]").addClass("grayText");
            }

            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }

        function ShowHideCombination(flag) {
            ///<summary>
            /// Used to Show/Hide HideShippingDetails Div
            ///</summary>

            //If flag then Show HideShippingDetails
            if (flag == 1) {
                $("[id$=divPackingCombination]").show();
                $("[id$=imbShowCombination]").hide();
                $("[id$=imbHideCombination]").show();
            }
            else {
                $("[id$=divPackingCombination]").hide();
                $("[id$=imbShowCombination]").show();
                $("[id$=imbHideCombination]").hide();
            }
            $("[id$=hdfIsCombinationVisible]").val(flag);
            return false;
        }

        function CalculateTotalCount(ctrl) {
            var TotalAmount = 0;
            $("#[id*=gdPackingCombination] input[type=text][id*=txtCount]").each(function (index) {
                var amount = 0;
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        amount = parseFloat($(this).val());
                        TotalAmount = TotalAmount + amount;
                    }
                }
            });
            $("#[id*=gdPackingCombination] [id*=lblTotalCount]").html(TotalAmount);
            //ctrl.Focus();
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }
        function FillMaterialCategoryAutoComplete() {
            //<summary> Function Used to make material category field as auto complete </summary>
            var MaterialCategroyURL = "MaterialCategory.do?Action=GetMaterialCategoryListAuto";
            GrandScriptUtils.MakeAutoComplete("txtMaterialCategory", MaterialCategroyURL + "&Type=3", "hdnMaterialCategoryPK", true, false, "BizUnitPk", false);
        }
        function FillMaterialAutoComplete() {
            //<summary> Function Used to make Item field as auto complete </summary>
            var MaterialURL = "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=";
            GrandScriptUtils.MakeAutoComplete("txtPackingMaterial", MaterialURL + $("[id$=BizUnitPk]").val() + "&Type=3&FLDNAME=ITM_TEXT", "hdnPackingMatID", true, false, "hdnMaterialCategoryPK", false);
        }

        function AfterClose(containerID) {
            if (containerID == "#divSearchProducts") {
                $("[id$=btnClearSearch]").click();
            }
        }


        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlProduct" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" TabIndex="34" OnClientClick="return ValidateNow('Product');"
                                            ValidationGroup="Product" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" ToolTip="<%$ resources:Controls,Delete %>"
                                            OnClientClick="return ShowDeleteConfirm(this);" OnClick="ActionHandler" TabIndex="35" />
                                    </li>
                                    <li id="pnlRelPrdtSave" runat="server">
                                        <asp:Button ID="btnRelPrdtSave" runat="server" CommandName="SAVESUBTYPEPRODUCTS"
                                            Text="<%$ resources:Controls,Save %>" SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$ resources:Controls,Save %>" OnClick="ActionHandler" TabIndex="34" />
                                    </li>
                                    <li id="pnlRelPrdtDelete" runat="server">
                                        <asp:Button ID="btnRelPrdtDelete" runat="server" CommandName="DELETESUBTYPEPRODUCTS"
                                            Text="<%$resources:Controls,Delete %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            OnClick="ActionHandler" TabIndex="35" />
                                    </li>
                                    <li id="pnlSubPrdtSave" runat="server">
                                        <asp:Button ID="btnSubPrdtSave" runat="server" CommandName="SAVEGRADEPRODUCTS" Text="<%$ resources:Controls,Save %>"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" TabIndex="34" />
                                    </li>
                                    <li id="pnlSubPrdtDelete" runat="server">
                                        <asp:Button ID="btnSubPrdtDelete" runat="server" CommandName="DELETEGRADEPRODUCTS"
                                            Text="<%$resources:Controls,Delete %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            OnClick="ActionHandler" TabIndex="35" />
                                    </li>
                                    <%------Packing Material Start--%>
                                    <li id="pnlPackMatSave" runat="server">
                                        <asp:Button ID="btnPackMatSave" runat="server" CommandName="SAVEPACKINGMATERIALS"
                                            Text="<%$ resources:Controls,Save %>" SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$ resources:Controls,Save %>" OnClick="ActionHandler" TabIndex="36" />
                                    </li>
                                    <li id="pnlPackMatDelete" runat="server">
                                        <asp:Button ID="btnPackMatDelete" runat="server" CommandName="DELETEPACKINGMATERIALS"
                                            Text="<%$resources:Controls,Delete %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            OnClick="ActionHandler" TabIndex="36" />
                                    </li>
                                    <%-- End Packing Material--%>
                                    <%------Line Section --%>
                                    <li id="pnlLineSave" runat="server">
                                        <asp:Button ID="btnLineSave" runat="server" CommandName="SAVEPRODLINEMAP" Text="<%$ resources:Controls,Save %>"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" TabIndex="36" />
                                    </li>
                                    <li id="pnlStoreSave" runat="server">
                                        <asp:Button ID="btnStoreSave" runat="server" CommandName="SAVEPRODSTOREMAP" Text="<%$ resources:Controls,Save %>"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" TabIndex="36" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" ToolTip="<%$ resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" TabIndex="36" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <%-- <asp:ImageButton runat="server" ID="imbPrint"SkinID="btnInner-Print" Text="<%$resources:Controls,Print %>"
                                            ToolTip="<%$resources:Controls,Print %>" />--%>
                                        <a href="<%= FileUrl %>" class="download-link nomargin" title="Download">Download</a>
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnProductListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnProductListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="8"
                                ToolTip="<%$ resources:Controls,List %>" />
                        </span></li>
                        <li><span id="spnProductDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnProductDetails" runat="server" Text="<%$ resources:Controls,ProductDetails %>"
                                CommandName="ACTIVATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9"
                                ToolTip="<%$ resources:Controls,ProductDetails %>" />
                        </span></li>
                        <li id="liRate" runat="server"><span id="spnRelatedProducts" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnRelatedProducts" runat="server" Text="<%$ resources:Controls,RelatedProducts %>"
                                CommandName="RELATEDPRODUCTS" CssClass="tab-inactive" OnClick="ActionHandler"
                                TabIndex="10" ToolTip="<%$ resources:Controls,RelatedProducts %>" />
                        </span></li>
                        <li id="liGrade" runat="server"><span id="spnSubGradeProducts" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnSubGradeProducts" runat="server" Text="<%$ resources:Controls,SubGradeProducts %>"
                                CommandName="SUBGRADEPRODUCTS" CssClass="tab-inactive" OnClick="ActionHandler"
                                TabIndex="11" ToolTip="<%$ resources:Controls,SubGradeProducts %>" />
                        </span></li>
                        <%--PackingMaterialTab--%>
                        <li id="liPacking" runat="server"><span id="spnPackingMaterials" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnPackingMaterials" runat="server" Text="<%$ resources:Controls,PackingMaterialTab %>"
                                CommandName="PACKINGMATERIALS" CssClass="tab-inactive" OnClick="ActionHandler"
                                TabIndex="11" ToolTip="<%$ resources:Controls,PackingMaterialTab %>" />
                        </span></li>
                        <%--LineTab--%>
                        <li id="liLines" runat="server"><span id="spnLines" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,IsVisibleLineTab %>">
                            <asp:LinkButton ID="lbLine" runat="server" Text="<%$ resources:Controls,LineTab %>"
                                CommandName="LINES" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="11"
                                ToolTip="<%$ resources:Controls,LineTab %>" />
                        </span></li>
                        <%--StoreMapping Tab--%>
                        <li id="li1" runat="server"><span id="spnStoreMapping" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,IsVisibleStoreMappingTab %>">
                            <asp:LinkButton ID="lbStoreMapping" runat="server" Text="<%$ resources:Controls,StoreMapping %>"
                                CommandName="STOREMAPPING" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="11"
                                ToolTip="<%$ resources:Controls,StoreMappingTab %>" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom" style="display: none">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1">
                                    <asp:ListItem Text="<%$ resources:Controls,ProductCode %>" Value="<%$ resources:DataFieldRes,ItemCode %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,ProductName %>" Value="<%$ resources:DataFieldRes,ItemName %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,GroupName %>" Value="<%$ resources:DataFieldRes,ItemGroup %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,ProductGrade %>" Value="<%$ resources:DataFieldRes,ItemGrade %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" TabIndex="2" />
                                <asp:HiddenField ID="hdnSearch" runat="server" />
                                <asp:ImageButton ID="btnSearch" SkinID="search-ext" CssClass="margntop2 margnbotm0"
                                    runat="server" Text="<%$ resources:Controls,Go %>" ToolTip="<%$ resources:Controls,Go %>"
                                    CommandName="SEARCH" TabIndex="3" OnClick="ActionHandler" />
                            </div>
                            <%--  ---Advance Search Region Start--------------------%>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td colspan="2">
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblAdvGroupName" Text="<%$ resources:Controls,GroupName %>"
                                                AssociatedControlID="ddlAdvProductGroup" CssClass="lbl-12-4perc"></asp:Label>
                                            <asp:DropDownList ID="ddlAdvProductGroup" runat="server" TabIndex="1" CssClass="input-w79-6per" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblAdvProduct" runat="server" Text="<%$ resources:Controls,Product %>"
                                                AssociatedControlID="txtAdvProduct" CssClass="lbl-10-1perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAdvProduct" TabIndex="2" placeholder="Select/Type"
                                                CssClass="input-small-e margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfProductPK" Value="0" runat="server" />
                                            <asp:ImageButton ID="imgbtnPropSearch" runat="server" Text="" ToolTip="<%$ resources:Controls,SEARCHPRDTPROPERTIES %>"
                                                OnClick="ActionHandler" TabIndex="2" CommandName="SEARCHWITHPROPERTIES" SkinID="show-popup"
                                                CssClass="margntop2 margnbotm0" />
                                            <asp:CheckBox ID="chkProdAuto" runat="server" TabIndex="2" Checked="true" CssClass="style-none margnbotm0"
                                                Style="margin-bottom: -6px !important; margin-left: 2px !important;" onClick="javascript:ProductAuto();"
                                                ToolTip="<%$ Resources:Captions,ActiveProducts %>" />

                                            <asp:CheckBox ID="chkMapped" runat="server" TabIndex="11" Checked="false" CssClass="style-none margnbotm0"
                                                Style="margin-bottom: -6px !important; margin-left: 0px !important;" />
                                            <asp:Label ID="lblMapped" runat="server" Text="<%$ resources:RelatedProdMappedShort %>"
                                                CssClass="margnlft-minus12 margntop2 style-none margnbotm0" />
                                        </div>
                                    </td>

                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblAdvProductGrade" Text="<%$ resources:Controls,ProductGrade %>"
                                                AssociatedControlID="ddlAdvPrdtGrade" CssClass="middle-lbl-xsmall-a1 margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlAdvPrdtGrade" runat="server" CssClass="select-small-b margnbotm0"
                                                TabIndex="2" />
                                            <div id="divAdvProdSubCategory" runat="server" class="display-inline">
                                                <asp:Label ID="lblAdvProdSubCategory" runat="server" Text="<%$ resources:Controls,SubCategory %>"
                                                    AssociatedControlID="ddlAdvProdSubCategory" CssClass="middle-lbl-xsmall-a2 margnbotm0" />
                                                <asp:DropDownList ID="ddlAdvProdSubCategory" runat="server" CssClass="lbl-18perc margnbotm0"
                                                    TabIndex="2" />
                                            </div>
                                            <asp:CheckBox ID="chkActiveFilter" runat="server" TabIndex="11" Checked="true" CssClass="style-none margnbotm0"
                                                Style="margin-bottom: -6px !important; margin-left: 16px !important;" />
                                            <asp:Label ID="lblActiveFilter" runat="server" Text="<%$ resources:Controls,Active %>"
                                                CssClass="lbl-9perc margnlft-minus12 margntop2 style-none margnbotm0" />
                                            <asp:ImageButton ID="btnSearchHdr" runat="server" Text="" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="2" CommandName="SEARCH" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="2" OnClick="ActionHandler" CommandName="CLEARSEARCH" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%------- End Advance search region--------------------%>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdProductList" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>

                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:HiddenField ID="hdfprohasbrandprd" runat="server" Value='<%# Eval("ITM_HAS_BRAND_PRD") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductCode %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPdtCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemCode)) %>' />
                                                <%--Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemCode)),13) %>'--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductName %>" SortExpression="<%$ resources:DataFieldRes,ItemName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPdtName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemName).ToString().Replace(" ","&nbsp;")) %>' />
                                                <%--Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemName)),80) %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="52%" CssClass="wordbreak" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,SubCategory %>" Visible="<%$ resources:ConfigurationsRes,ShowProductSubCategory %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfProdSubCategory" runat="server" Value='<%# Eval("ITM_SUB_TYPE") %>' />
                                                <asp:Label ID="lblProdSubCategory" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProGrade %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfProductGrade" runat="server" Value='<%# Eval("ITM_GRADE") %>' />
                                                <asp:Label ID="lblProductGrade" runat="server" Text='<%# Eval("ITM_GRADE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Left" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Size %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemSize" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Length %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemLength" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,UOM %>" SortExpression="<%$ resources:DataFieldRes,ItemUOM %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.InvUomMst2 + "." + Resources.DataFieldRes.UomCode) %>'
                                                    Text='<%# Eval(Resources.DataTableRes.InvUomMst2 + "." + Resources.DataFieldRes.UomCode) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,UnitWeightInGrams %>" SortExpression="<%$ resources:DataFieldRes,ItemWeight %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWeight" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.ItemWeight, "{0:n3}") %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemWeight, "{0:n3}"),10) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbGenerateBrand" runat="server" SkinID="add_small-icon"
                                                    ToolTip="Generate Brand" CssClass="Active" Enabled="true" OnClick="ActionHandler" CommandName="GENERATEBRAND" />
                                                <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("ITM_ACTIVE").ToString() == "0") ?
                                               true  : false %>'
                                                    ToolTip="Inactive" CssClass="Active" Enabled="false" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("ITM_ACTIVE").ToString() == "1") ?
                                               true  : false %>'
                                                    ToolTip="Active" CssClass="Active" Enabled="false" />

                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="imgMapped" OnClientClick="javascript:return false;" TabIndex="14" />
                                                <asp:HiddenField runat="server" ID="hdfMappedCount" Value='<%# Eval("INV_ITEM_SUB_TYPE_MAP.Count") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                            <%--Option to search by Product Properties--%>
                            <div id="divSearchProducts" style="display: none;">
                                <uc2:SearchProducts ID="ucrSearchProducts" runat="server" />
                            </div>
                            <asp:Button ID="btnClearSearch" runat="server" OnClick="ActionHandler" CommandName="CLEARSEARCHPROPERTIES"
                                EnableTheming="false" Style="display: none" />
                            <%--END:Option to search by Product Properties--%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <asp:HiddenField ID="hdfDdlCount" runat="server" />
                            <table class="table-devide">
                                <tr>
                                    <%--<td colspan="2">--%>
                                    <td>
                                        <%----%>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblProductCode" runat="server" Text="<%$ resources:Controls,ProductCode %>"
                                                AssociatedControlID="txtProductCode" />
                                            <%-- <asp:TextBox ID="txtProductCode" runat="server" MaxLength="60" TabIndex="10" CssClass="input-w30per" />--%>
                                            <asp:TextBox ID="txtProductCode" runat="server" MaxLength="60" TabIndex="10" CssClass="input-w51-7per" />
                                            <asp:RequiredFieldValidator ID="vrfPdtCode" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtProductCode" ErrorMessage="<%$ resources:Err_ProductCode %>" />
                                            <%--<div class="check-inline">--%>
                                            <asp:CheckBox ID="chbAutoGenerate" runat="server" TabIndex="11" onclick="AutoGenerateChecked();" />
                                            <asp:Label ID="lblAutoGenerate" runat="server" CssClass="lbl-14-2perc style-none"
                                                Text="<%$ resources:Controls,AutoGenerate %>" />
                                            <%--    <asp:CheckBox ID="chkActive" runat="server" TabIndex="11" Checked="true" />
                                            <asp:Label ID="lblActive" runat="server" CssClass="lbl-14-2perc style-none" Text="<%$ resources:Controls,Active %>" />--%>
                                            <%-- </div>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPrdCategory" runat="server" Text="<%$ resources:Controls,ProductCategory %>"
                                                AssociatedControlID="ddlPrdCategory" />
                                            <asp:DropDownList ID="ddlPrdCategory" runat="server" CssClass="lbl-49-4perc" TabIndex="13" />
                                            <asp:RequiredFieldValidator ID="vrfPrdCategory" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlPrdCategory" ErrorMessage="<%$ resources:Err_PrdCategory %>" />
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="11" Checked="true" />
                                            <asp:Label ID="lblActive" runat="server" CssClass="lbl-14-2perc style-none" Text="<%$ resources:Controls,Active %>" />
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblProductName" runat="server" Text="<%$ resources:Controls,ProductName %>"
                                                AssociatedControlID="txtProductName" />
                                            <asp:TextBox ID="txtProductName" runat="server" MaxLength="400" TabIndex="12" onkeydown="limitText(this,400);"
                                                onkeyup="limitText(this,400);" CssClass="input-full" />
                                            <asp:RequiredFieldValidator ID="vrfProductName" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtProductName" ErrorMessage="<%$ resources:Err_ProductName %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblUOM" runat="server" Text="<%$ resources:Controls,UOM %>" AssociatedControlID="ddlUOM" />
                                            <asp:DropDownList ID="ddlUOM" runat="server" CssClass="select-small-a1" TabIndex="13" />
                                            <asp:RequiredFieldValidator ID="vrfUOM" CssClass="star" SetFocusOnError="true" InitialValue="-1"
                                                ValidationGroup="Product" EnableClientScript="true" runat="server" Display="Dynamic"
                                                Text="*" ControlToValidate="ddlUOM" ErrorMessage="<%$ resources:Err_UOM %>" />
                                            <asp:Label ID="lblWeight" runat="server" Text="<%$ resources:Controls,UnitWeightInGrams %>"
                                                CssClass="middle-lbl-c" AssociatedControlID="txtWeight" />
                                            <asp:TextBox ID="txtWeight" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                onkeypress="return isFloatNumberKey(event);" TabIndex="14" />
                                            <asp:RequiredFieldValidator ID="vrfWeight" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtWeight" ErrorMessage="<%$ resources:Err_Weight %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblminweight" runat="server" Text="<%$ resources:MinWeight %>" AssociatedControlID="txtMinWeight" />
                                            <asp:TextBox ID="txtMinWeight" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                onkeypress="return isFloatNumberKey(event);" TabIndex="15" />
                                            <asp:RequiredFieldValidator ID="vrfMinWeight" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtMinWeight" ErrorMessage="<%$ resources:Err_MinWeight %>" />
                                            <asp:Label ID="lblMaxweight" runat="server" Text="<%$ resources:MaxWeight %>" AssociatedControlID="txtMaxWeight"
                                                CssClass="middle-lbl-c" />
                                            <asp:TextBox ID="txtMaxWeight" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                onkeypress="return isFloatNumberKey(event);" TabIndex="16" />
                                            <asp:RequiredFieldValidator ID="vrfMaxWeight" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="Product"
                                                EnableClientScript="true" runat="server" Display="Dynamic" Text="*" ControlToValidate="txtMaxWeight"
                                                ErrorMessage="<%$ resources:Err_MaxWeight %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <%-- <asp:Label ID="lblProductGroup" runat="server" Text="<%$ resources:ProductGroup %>"
                                                AssociatedControlID="ddlProductGroup" />--%>
                                            <div runat="server" id="divProductGroup" class="margnlft-minus2">
                                                <asp:Label ID="lblProductGroup" runat="server" Text="<%$ resources:ProductGroup %>"
                                                    AssociatedControlID="txtProductGroup" />
                                                <asp:TextBox runat="server" ID="txtProductGroup" TabIndex="18"
                                                    CssClass="select-58-9per">
                                                </asp:TextBox>
                                                <asp:HiddenField ID="hdfProductGroupPk" Value="0" runat="server" />
                                            </div>
                                            <%--                                            <asp:DropDownList ID="ddlProductGroup" runat="server" TabIndex="18" CssClass="select-w80-5per" />--%>
                                            <%-- <asp:RequiredFieldValidator ID="vrfProductGroup" CssClass="star" SetFocusOnError="true" InitialValue="-1"
                                                ValidationGroup="Product" EnableClientScript="true" runat="server" Display="Dynamic"
                                                Text="*" ControlToValidate="ddlProductGroup" ErrorMessage="<%$ resources:Err_ProductGroup %>" />--%>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lbFirstRef" runat="server" Text="<%$ resources:FirstRef %>" AssociatedControlID="txtProductDesc" />
                                            <asp:TextBox ID="txtFirstRef" runat="server" MaxLength="25" TabIndex="19" onkeydown="limitText(this,25);"
                                                CssClass="input-small" onkeyup="limitText(this,25);" />
                                            <asp:Label ID="lbSecRef" runat="server" Text="<%$ resources:SecRef %>" AssociatedControlID="txtProductDesc"
                                                CssClass="middle-lbl-c" />
                                            <asp:TextBox ID="txtSecRef" runat="server" MaxLength="25" TabIndex="19" onkeydown="limitText(this,25);"
                                                CssClass="input-small" onkeyup="limitText(this,25);" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGradeRealisation" runat="server" Text="<%$ resources:GradeRealisation %>"
                                                AssociatedControlID="txtGradeRealisation" />
                                            <asp:TextBox ID="txtGradeRealisation" runat="server" MaxLength="5" CssClass="input-small numeric"
                                                TabIndex="19" onkeydown="limitText(this,5);" onkeyup="limitText(this,5);" ToolTip="<%$ resources:GradeRealisation %>"
                                                Text="100" />
                                            <asp:RangeValidator ID="vre1GradeRealisation" runat="server" ErrorMessage="<%$ resources:Err_GradeRealisation %>"
                                                CssClass="star" SetFocusOnError="true" ValidationGroup="Product" Display="Dynamic"
                                                Text="*" ControlToValidate="txtGradeRealisation" Type="Double" MaximumValue="100.00"
                                                MinimumValue="0.00">
                                            </asp:RangeValidator>
                                            <%--<div class="check-inline">
                                            <asp:CheckBox ID="chkIsScrap" Text='<%$ resources:IsScrap%>' runat="server" TabIndex="19" />
                                            </div>--%>
                                            <asp:Label ID="lbThirdRef" runat="server" Text="<%$ resources:ThirdRef %>" AssociatedControlID="txtThirdRef"
                                                CssClass="middle-lbl-c" />
                                            <asp:TextBox ID="txtThirdRef" runat="server" MaxLength="25" TabIndex="19" onkeydown="limitText(this,25);"
                                                CssClass="input-small" onkeyup="limitText(this,25);" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lbFourthRef" runat="server" Text="<%$ resources:FourthRef%>" AssociatedControlID="txtFourthRef" />
                                            <asp:TextBox ID="txtFourthRef" runat="server" MaxLength="25" TabIndex="19" onkeydown="limitText(this,25);"
                                                CssClass="input-small" onkeyup="limitText(this,25);" />
                                            <asp:Label ID="lblNoOfCompounds" runat="server" Text="<%$ resources:NoCompounds%>"
                                                CssClass="middle-lbl-c" AssociatedControlID="ddlNoCompounds" />
                                            <asp:DropDownList ID="ddlNoCompounds" runat="server" TabIndex="19" CssClass="select-small-a1">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPrdtGrd" runat="server" Text="<%$ resources:Controls,ProductGrade %>"
                                                AssociatedControlID="ddlPrdtGrade" />
                                            <asp:DropDownList ID="ddlPrdtGrade" runat="server" CssClass="select-small-a1" TabIndex="19" />
                                            <asp:RequiredFieldValidator ID="vrfPrdtGrade" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlPrdtGrade" ErrorMessage="<%$ resources:Err_ProductGrade %>" />
                                            <div id="divProdSubCategory" runat="server" class="display-inline">
                                                <asp:Label ID="lblProdSubCategory" runat="server" Text="<%$ resources:Controls,SubCategory %>"
                                                    AssociatedControlID="ddlProdSubCategory" CssClass="lbl-20-5perc margn-rgt0" />
                                                <asp:DropDownList ID="ddlProdSubCategory" runat="server" CssClass="select-small-a1"
                                                    TabIndex="19" />
                                                <asp:RequiredFieldValidator ID="vrfProductSubCategory" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                    Display="Dynamic" Text="*" ControlToValidate="ddlProdSubCategory" ErrorMessage="<%$ resources:Err_SubCategory %>" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblProductDesc" runat="server" Text="<%$ resources:Controls,Description %>"
                                                AssociatedControlID="txtProductDesc" />
                                            <asp:TextBox ID="txtProductDesc" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="19" CssClass="prd-textarea" onkeydown="limitText(this,499);" onkeyup="limitText(this,499);" />
                                            <asp:Label ID="lblGSTClass" runat="server" Text="<%$ resources:Controls,HSNCode %>"
                                                AssociatedControlID="ddlGSTClass" CssClass="lbl-10-5perc" />
                                            <asp:DropDownList ID="ddlGSTClass" runat="server" CssClass="select-9-4per" TabIndex="19" />
                                            <div class="clear">
                                            </div>
                                            <div runat="server" id="divInvProduct" class="margnlft-minus2">
                                                <asp:Label ID="lblInventoryProd" runat="server" Text="<%$ resources:InvProduct %>"
                                                    AssociatedControlID="txtInventoryProd" />
                                                <asp:TextBox runat="server" ID="txtInventoryProd" TabIndex="19" placeholder="Select/Type"
                                                    CssClass="select-58-9per">
                                                </asp:TextBox>
                                                <%--<asp:HiddenField ID="hdfProCode" Value="-1" runat="server" />--%>
                                                <asp:HiddenField ID="hdfInventoryProdPK" Value="0" runat="server" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPackigSpec" runat="server" visible="false">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPackingSpec" runat="server" Text="<%$ resources:PackingSpec %>"
                                                AssociatedControlID="ddlPackingSpec" />
                                            <asp:DropDownList ID="ddlPackingSpec" runat="server" CssClass="lbl-49-4perc">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblPlanGroup" runat="server" Text="<%$ resources:Controls,PlanningGroup %>"
                                                AssociatedControlID="ddlPlanGroup" />
                                            <asp:DropDownList ID="ddlPlanGroup" runat="server" CssClass="select-w80-5per" TabIndex="19" />
                                            <asp:RequiredFieldValidator ID="vrfddlPlanGroup" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Product" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlPlanGroup" ErrorMessage="<%$ resources:Err_PlanningGroup %>" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="grid-group">
                                <h3 id="hHead1" runat="server">
                                    <%= GetLocalResourceObject("ProductAttributes").ToString() %>
                                </h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table padglft0">
                                    <table class="table-devide">
                                        <tr>
                                            <td id="tdCol1" runat="server"></td>
                                            <td id="tdCol2" runat="server"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="tblRate" runat="server" visible="false">
                                <h5 id="h5" runat="server">
                                    <%= GetLocalResourceObject("Rates").ToString() %>
                                </h5>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">

                                                <asp:Label ID="lblInterState" runat="server" Text="<%$ resources:InterState %>"
                                                    CssClass="middle-lbl-c" AssociatedControlID="txtInterState" />
                                                <asp:TextBox ID="txtInterState" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                    onkeypress="return isFloatNumberKey(event);" TabIndex="19" />
                                                <asp:Label ID="lblIntraState" runat="server" Text="<%$ resources:IntraState %>"
                                                    CssClass="middle-lbl-c" AssociatedControlID="txtIntraState" />
                                                <asp:TextBox ID="txtIntraState" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                    onkeypress="return isFloatNumberKey(event);" TabIndex="19" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblExport" runat="server" Text="<%$ resources:Export %>" AssociatedControlID="txtExport" />
                                                <asp:TextBox ID="txtExport" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                    onkeypress="return isFloatNumberKey(event);" TabIndex="19" />
                                                <asp:Label ID="lblOthers" runat="server" Text="<%$ resources:Others %>" AssociatedControlID="txtOthers"
                                                    CssClass="middle-lbl-c" />
                                                <asp:TextBox ID="txtOthers" runat="server" MaxLength="12" CssClass="input-small numeric"
                                                    onkeypress="return isFloatNumberKey(event);" TabIndex="19" />

                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                            </div>

                            <div id="divCombination" runat="server">
                                <div class="search-colapse-b margntop5">
                                    <h1>
                                        <%= GetLocalResourceObject("PackCombination").ToString()%></h1>
                                    <asp:ImageButton runat="server" ID="imbShowCombination" OnClientClick="javascript:return ShowHideCombination(1);"
                                        SkinID="imbArrowShow" ToolTip="<%$ resources:ShowCombination %>" />
                                    <asp:ImageButton runat="server" ID="imbHideCombination" OnClientClick="javascript:return ShowHideCombination();"
                                        Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideCombination %>" />
                                    <asp:HiddenField ID="hdfIsCombinationVisible" runat="server" Value="0" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <div id="divPackingCombination" style="display: none">
                                    <table class="table-devide tablelayout">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblCombinationBasedOn" runat="server" AssociatedControlID="ddlCombinationBasedOn"
                                                        Text="<%$ resources:BasedOn %>">
                                                    </asp:Label>
                                                    <asp:DropDownList ID="ddlCombinationBasedOn" runat="server" TabIndex="36" CssClass="select-small-c"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                    </asp:DropDownList>
                                                    <%-- <asp:RequiredFieldValidator ID="rfBasedOn" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlCombinationBasedOn"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Inspection %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"></td>
                                        </tr>
                                    </table>
                                    <table class="table-devide tablelayout">
                                        <tr>
                                            <td>
                                                <div class="div2col-S-packdetails">
                                                    <asp:Label ID="lblDummyTxt" runat="server" Text=" " AssociatedControlID="gdPackingCombination"
                                                        CssClass="margn-rgt0" />
                                                    <div class="gridwrap" style="width: 60.5%; display: inline-block">
                                                        <asp:GridView runat="server" ID="gdPackingCombination" AllowPaging="false" ShowFooter="true"
                                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" Style="float: left;"
                                                            OnRowDataBound="ActionHandler">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:Name %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAttributeName" runat="server" Text='<%#Eval("IPC_PROPERTY_TEXT") %>' />
                                                                        <asp:HiddenField ID="hdfPackCombPK" runat="server" Value='<%# Eval("IPC_PK") %>' />
                                                                        <asp:HiddenField ID="hdfAttributePK" runat="server" Value='<%# Eval("IPC_PROPERTY") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="style-none" />
                                                                    <HeaderStyle Width="90%" HorizontalAlign="Left" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Count %>" HeaderStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-CssClass="txt-rgt" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCount" runat="server" Text='<%#Eval("IPC_PROPERTY_COUNT") %>'
                                                                            CssClass="input-medium numeric" MaxLength="1" onkeyup="CalculateTotalCount(this);"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <%-- onkeypress="return isNumberKey(event);"--%>
                                                                    <HeaderStyle Width="10%" HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="datalistTbl inputTable" style="display: none;">
                                <asp:DataList ID="dlGradeProperties" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" DataKeyField="CON_PK" OnItemDataBound="ActionHandlerGrade">
                                    <HeaderTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <h3 style="font-size: 10px;">
                                                        <asp:Literal ID="ltCheckapplicableitems" runat="server" Text="<%$ resources:Checkapplicableitems%>" />
                                                    </h3>
                                                </td>
                                                <td align="right">
                                                    <%-- <h3 class="fontWGT-Nrml">
                                                            <asp:Literal ID="ltGrades" runat="server" Text="<%$ resources:Grades%>" />
                                                        </h3>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkGrade" Text='<%#Eval("CON_NAME")%>' runat="server" TabIndex="11" />
                                        <asp:Label ID="lblGradeChecked" runat="server" Text='<%# Eval("GradeSelected")%>'
                                            Visible="false"></asp:Label>
                                        <asp:HiddenField ID="hdnGradeValue" runat="server" Value='<%#Eval("CON_VALUE")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:DataList>
                                <table id="tableNoGradeProperties" runat="server" visible="false">
                                    <tr>
                                        <td align="right">
                                            <h4>
                                                <asp:Literal ID="ltGrades" runat="server" Text="<%$ resources:Grades%>" />
                                            </h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:ErrorMessages,Msg_Sorry_No_Data_Available %>"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_SubPrdts" runat="server">
                        <asp:TableCell>
                            <div class="detail-co3">
                                <div class="div3col-S w50perc">
                                    <asp:Label runat="server" ID="lblPrdt" Text="<%$resources:Controls,ProductName%>"
                                        CssClass="w23perc valign-base" AssociatedControlID="lblRelProductName">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblRelProductName" CssClass="disp-table w70perc linezero valign-base"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                    <asp:Label ID="lblPrdtCode" runat="server" Text="<%$resources:Controls,ProductCode%>"
                                        AssociatedControlID="lblRelProductCode"></asp:Label>
                                    <asp:Label runat="server" ID="lblRelProductCode" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                    <asp:Label ID="lbllblSubGradePrdtGradeH" runat="server" Text="<%$resources:Controls,Grade%>"
                                        AssociatedControlID="lblSubGradePrdtGrade"></asp:Label>
                                    <asp:Label runat="server" ID="lblSubGradePrdtGrade" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblItem" runat="server" AssociatedControlID="txtItem" Text="<%$ resources:Controls,Product %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtItem" runat="server" TabIndex="30" CssClass="input-half" MaxLength="400"
                                                ValidationGroup="invoice"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfItem" CssClass="star" SetFocusOnError="true" InitialValue="<%$ resources:TypeMin3 %>"
                                                ValidationGroup="SubProduct" EnableClientScript="true" runat="server" Display="Dynamic"
                                                Text="*" ControlToValidate="txtItem" ErrorMessage="<%$ resources:Err_Product %>" />
                                            <asp:HiddenField ID="hdfItemID" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblProductGrade" runat="server" Text="<%$ resources:Controls,ProductGrade %>"
                                                AssociatedControlID="txtProductName" />
                                            <asp:DropDownList ID="ddlProductGrade" runat="server" CssClass="large" TabIndex="31" />
                                            <asp:RequiredFieldValidator ID="vrfddlProductGrade" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="SubProduct" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlProductGrade" ErrorMessage="<%$ resources:Err_ProductGrade %>" />
                                            <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="32"
                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('SubProduct')"
                                                ToolTip="<%$resources:ErpRes,Add %>" ValidationGroup="SubProduct" SkinID="plus" />
                                            <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="33"
                                                OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" SkinID="cancel" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>

                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdRelatedPrdts" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductCode %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfRelPrdtPk" runat="server" Value='<%# Eval("IMR_REL_ITEM") %>' />
                                                <asp:Label ID="lblPdtCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_CODE")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_CODE")) %>' />
                                                <%--Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemCode)),13) %>'--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductName %>" SortExpression="<%$ resources:DataFieldRes,ItemName %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblPrdtName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME")), 80)%>' />--%>
                                                <asp:Label ID="lblPrdtName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME").ToString().Replace(" ","&nbsp;")), 100)%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="61%" CssClass="wordbreak" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductGrade %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfPrdtGrade" runat="server" Value='<%# Eval("IMR_ITEM_GRADE") %>' />
                                                <asp:Label ID="lblPrdtGrade" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                    SkinID="imbeditgrid" ToolTip="Edit" TabIndex="33" />
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    OnClientClick="return ShowDeleteConfirm(this);" SkinID="imbdeletegrid" ToolTip="Delete"
                                                    TabIndex="33" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_RelatedPrdts" runat="server">
                        <asp:TableCell>
                            <div class="detail-co3">
                                <div class="div3col-S w46perc">
                                    <asp:Label runat="server" ID="lblRelatedProdNameH" Text="<%$resources:Controls,ProductName%>"
                                        CssClass="w23perc valign-base" AssociatedControlID="lblRelatedProdName">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblRelatedProdName" CssClass="disp-table w70perc linezero valign-base"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                    <asp:Label ID="lblRelatedProdCodeH" runat="server" Text="<%$resources:Controls,ProductCode%>"
                                        AssociatedControlID="lblRelatedProdCode"></asp:Label>
                                    <asp:Label runat="server" ID="lblRelatedProdCode" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S w19perc">
                                    <asp:Label ID="lblRelatedProdSubCategoryH" runat="server" Text="<%$resources:Controls,SubCategory%>"
                                        AssociatedControlID="lblRelatedProdSubCategory"></asp:Label>
                                    <asp:Label runat="server" ID="lblRelatedProdSubCategory" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S w10perc">
                                    <asp:Label ID="lblRelatedProdGradeH" runat="server" Text="<%$resources:Controls,Grade%>"
                                        AssociatedControlID="lblRelatedProdGrade"></asp:Label>
                                    <asp:Label runat="server" ID="lblRelatedProdGrade" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRelatedItem" runat="server" AssociatedControlID="txtRelatedItem"
                                                Text="<%$ resources:Controls,Product %>">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlSubType" runat="server" CssClass="select-9-4per" TabIndex="30" />
                                            <asp:TextBox ID="txtRelatedItem" runat="server" TabIndex="31" CssClass="select-w70per"
                                                MaxLength="400" ValidationGroup="invoice"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRelatedItem" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:TypeMin3 %>" ValidationGroup="RelatedProduct" EnableClientScript="true"
                                                runat="server" Display="Dynamic" Text="*" ControlToValidate="txtRelatedItem"
                                                ErrorMessage="<%$ resources:Err_Product %>" />
                                            <asp:HiddenField ID="hdfRelatedItem" runat="server" />
                                            <asp:ImageButton runat="server" ID="imgSubTypeAdd" CommandName="ADDSUBTYPEITEM" TabIndex="32"
                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('RelatedProduct')"
                                                ToolTip="<%$resources:ErpRes,Add %>" ValidationGroup="RelatedProduct" SkinID="plus" />
                                            <asp:ImageButton runat="server" ID="imgSubTypeClear" CommandName="CLEARSUBTYPEITEM"
                                                TabIndex="33" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>"
                                                SkinID="cancel" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ resources:Controls,Category %>" AssociatedControlID="txtProductName" />
                                            <asp:DropDownList ID="ddlSubType" runat="server" CssClass="large" TabIndex="31" />
                                            <asp:RequiredFieldValidator ID="rfvSubType" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="RelatedProduct" EnableClientScript="true"
                                                runat="server" Display="Dynamic" Text="*" ControlToValidate="ddlSubType" ErrorMessage="<%$ resources:Err_Category %>" />
                                            <asp:ImageButton runat="server" ID="imgSubTypeAdd" CommandName="ADDSUBTYPEITEM" TabIndex="32"
                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('RelatedProduct')"
                                                ToolTip="<%$resources:ErpRes,Add %>" ValidationGroup="RelatedProduct" SkinID="plus" />
                                            <asp:ImageButton runat="server" ID="imgSubTypeClear" CommandName="CLEARSUBTYPEITEM"
                                                TabIndex="33" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>"
                                                SkinID="cancel" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>--%>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="gvSubTypeProducts" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductCode %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfRelPrdtPk" runat="server" Value='<%# Eval("ISM_REL_ITEM") %>' />
                                                <asp:Label ID="lblPdtCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_CODE")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_CODE")) %>' />
                                                <%--Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemCode)),13) %>'--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductName %>" SortExpression="<%$ resources:DataFieldRes,ItemName %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblPrdtName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME")), 80)%>' />--%>
                                                <asp:Label ID="lblPrdtName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME").ToString().Replace(" ","&nbsp;")), 100)%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="61%" CssClass="wordbreak" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,SubCategory %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSubCategory" runat="server" Value='<%# Eval("INV_ITEM_MST1.ITM_SUB_TYPE") %>' />
                                                <asp:Label ID="lblSubCategory" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITSUBTYPEITEM"
                                                    SkinID="imbeditgrid" ToolTip="Edit" TabIndex="33" />
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVESUBTYPEITEM"
                                                    OnClientClick="return ShowDeleteConfirm(this);" SkinID="imbdeletegrid" ToolTip="Delete"
                                                    TabIndex="33" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%-- ----------------------------Packing Materials Start--------------------------------------------------%><asp:TableRow ID="PageAction_PackingMaterials" runat="server">
                        <asp:TableCell>
                            <div class="detail-co3">
                                <div class="div3col-S w50perc">
                                    <asp:Label runat="server" ID="lblPackMatProdNameH" Text="<%$resources:Controls,ProductName%>"
                                        CssClass="w23perc valign-base" AssociatedControlID="lblPackMatProdName">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblPackMatProdName" CssClass="disp-table w70perc linezero valign-base"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                    <asp:Label ID="lblPackMatProdCodeH" runat="server" Text="<%$resources:Controls,ProductCode%>"
                                        AssociatedControlID="lblPackMatProdCode"></asp:Label>
                                    <asp:Label runat="server" ID="lblPackMatProdCode" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%--<asp:Label runat="server" ID="lblCategory" Text="<%$ Resources:BindValues, ItemCategory%>" AssociatedControlID="txtMaterialCategory"></asp:Label>--%>
                                            <asp:Label ID="lblPackMatH" runat="server" AssociatedControlID="txtPackingMaterial"
                                                CssClass="lbl-9perc" Text="<%$ resources:Controls,Item %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtMaterialCategory" CssClass="input-w24-5per" runat="server" TabIndex="31"
                                                Text='<%$resources:ErpRes,AutoDefaultValue %>'>
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdnMaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                                            <asp:TextBox ID="txtPackingMaterial" runat="server" TabIndex="32" CssClass="input-w59-4per"
                                                Text='<%$resources:ErpRes,AutoDefaultValue %>' MaxLength="200" ValidationGroup="PackingMaterials"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPackMat" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="PackingMaterials"
                                                EnableClientScript="true" runat="server" Display="Dynamic" Text="*" ControlToValidate="txtPackingMaterial"
                                                ErrorMessage="<%$ resources:Err_PackMat %>" />
                                            <asp:HiddenField ID="hdnPackingMatID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPackMatCount" runat="server" AssociatedControlID="txtPackMatCount"
                                                Text="Count">
                                            </asp:Label>
                                            <asp:TextBox ID="txtPackMatCount" runat="server" TabIndex="33" CssClass="input-small-sw2"
                                                MaxLength="1" ValidationGroup="PackingMaterials"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPackMatCount" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="PackingMaterials" EnableClientScript="true"
                                                runat="server" Display="Dynamic" Text="*" ControlToValidate="txtPackMatCount"
                                                ErrorMessage="<%$ resources:Err_PackMatCount %>" />
                                            <asp:ImageButton runat="server" ID="imgPackMatAdd" CommandName="ADDPACKMATITEM" TabIndex="34"
                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('PackingMaterials')"
                                                ToolTip="<%$resources:ErpRes,Add %>" ValidationGroup="PackingMaterials" SkinID="plus" />
                                            <asp:ImageButton runat="server" ID="imgPackMatClear" CommandName="CLEARPACKMATITEM"
                                                TabIndex="34" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>"
                                                SkinID="cancel" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>

                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="gvPackingMaterials" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ItemCode %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfPackMatPrdtPk" runat="server" Value='<%# Eval("IMP_PACK_ITEM") %>' />
                                                <asp:Label ID="lblPdtCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_CODE")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_CODE")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ItemName %>" SortExpression="<%$ resources:DataFieldRes,ItemName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdtName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("INV_ITEM_MST1.ITM_NAME").ToString().Replace(" ","&nbsp;")), 100)%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="70%" CssClass="wordbreak" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Count">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackMatCount" runat="server" Text='<%# Eval("IMP_QUANITY")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITPACKMATITEM"
                                                    SkinID="imbeditgrid" ToolTip="Edit" TabIndex="35" />
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEPACKMATITEM"
                                                    OnClientClick="return ShowDeleteConfirm(this);" SkinID="imbdeletegrid" ToolTip="Delete"
                                                    TabIndex="35" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Right" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%-- ----------------------------Packing Materials Start--------------------------------------------------%>
                    <asp:TableRow ID="PageAction_Lines" runat="server">
                        <asp:TableCell>
                            <div class="detail-co3">
                                <div class="div3col-S w50perc">
                                    <asp:Label runat="server" ID="lbllnePrdName" Text="<%$resources:Controls,ProductName%>"
                                        CssClass="w23perc valign-base" AssociatedControlID="lbllnePrdNameDisplay">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lbllnePrdNameDisplay" CssClass="disp-table w70perc linezero valign-base"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                    <asp:Label ID="lbllneProductCode" runat="server" Text="<%$resources:Controls,ProductCode%>"
                                        AssociatedControlID="lbllneProductCodeDisplay"></asp:Label>
                                    <asp:Label runat="server" ID="lbllneProductCodeDisplay" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S w25perc">
                                    <asp:Label ID="lbllnePrdtGrade" runat="server" Text="<%$resources:Controls,Grade%>"
                                        AssociatedControlID="lbllnePrdtGradeDisplay"></asp:Label>
                                    <asp:Label runat="server" ID="lbllnePrdtGradeDisplay" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="treeview max-500 w30perc">
                                <h4>
                                    <%= Resources.Captions.Lines %>
                                </h4>
                                <div class="clear">
                                </div>
                                <asp:TreeView ID="trvLine" runat="server" ShowLines="true" ExpandDepth="0" onclick="OnCheckBoxCheckChanged(event)">
                                    <RootNodeStyle Font-Bold="True" />
                                    <ParentNodeStyle Font-Bold="True" />
                                </asp:TreeView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%-------------------------------End Packing Materials-------------------------------------%>
                    <%-- ----------------------------Store Mapping Start--------------------------------------------------%>

                    <asp:TableRow ID="PageAction_StoreMapping" runat="server">
                        <asp:TableCell>
                            <div class="treeview max-500 w30perc">
                                <h4>
                                    <%= Resources.Captions.Stores %>
                                </h4>
                                <div class="clear">
                                </div>
                                <asp:TreeView ID="trvStore" runat="server" ShowLines="true" ExpandDepth="0">
                                    <RootNodeStyle Font-Bold="True" />
                                    <ParentNodeStyle Font-Bold="True" />
                                </asp:TreeView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%-- ----------------------------End Store Mapping --------------------------------------------------%>

                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="Product" runat="server" />
                </div>
                <div id="RelPrdtdiverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsgRelPrdt" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsRelatedPrdtSave" ValidationGroup="SubProduct" runat="server" />
                    <asp:ValidationSummary ID="vsRelatedProduct" ValidationGroup="RelatedProduct" runat="server" />
                    <asp:ValidationSummary ID="vsPackingMaterial" ValidationGroup="PackingMaterials"
                        runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hdfIsBrandProduct" Value="0" runat="server" />
            <asp:HiddenField ID="hdfRelItemPk" runat="server" />
            <asp:HiddenField ID="hdfSubTypeItemPk" runat="server" />
            <asp:HiddenField ID="hdfPackMatItemPk" runat="server" />
            <asp:HiddenField ID="hdfProdSpecCompare" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsProductInSBU" runat="server" Value="0" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
