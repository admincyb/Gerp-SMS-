<%@ Page Title="<%$ Resources:Title_WorkOrderItem %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false"
    CodeBehind="OrderItem.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.OrderItem" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        function InitComponents() {
            ShowHideAdvancedSearch(1);
            FillMaterialCategoryAutoComplete(1);
            FillMaterialAutoComplete(1);
            InitCustomerBrands();
            InitProducts();
            InitCustSearch();
            // InitPMCategory();
            if ($("[id$=hdfSelectedPMMCATPK]").val() == "0") {
                InitPMMCategory();
                InitCust();
            }
            if ($("[id$=hdfSelectPMMCatPk]").val() != "0") {
                InitPMMCategory();
            }

            if ($("[id$=hdfCustomerPK]").val() == "" || $("[id$=hdfCustomerPK]").val() == "0") {
                DisableAuto($("[id$=txtBrandID]"), $("[id$=hdfBrandID]"));
            }
        }
        $(document).ready(function () {

        });
        function InitCustSearch() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerSearch", uiUrl, "hdfCustomerSearchPK", true, true, "CUSTOMER");
        }
        function InitCust(selval) {
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerID", uiUrl, "hdfCustomerPK", true, true, "CUSTOMER");
            if (selval == 1) {
                $("[id$=txtCustomerID]").val($("[id$=hdfSelectedCustomer]").val());
                $("[id$=hdfCustomerPK]").val($("[id$=hdfSelectedCustomerPK]").val());
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrandID", uiUrl + "?Type=" + $("[id$=hdfCustomerPK]").val(), "hdfBrandID", true, true, "CUTOMERBRAND");
            if (selval == 1) {
                $("[id$=txtBrandID]").val($("[id$=hdfSelectedBrand]").val());
                $("[id$=hdfBrandID]").val($("[id$=hdfSelectedBrandPK]").val());
            }
        }
        function InitWOIPMCategory() {

            GrandScriptUtils.MakeAutoComplete("txtPackingCat", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=3", "hdfPackingCat", true, false, "BizUnitPk", true);
            GrandScriptUtils.MakeAutoComplete("txtPackingItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT", "hdfPackingItem", true, false, "hdfPackingCat", true);
        }
        function InitWOIPMItem() {

            GrandScriptUtils.MakeAutoComplete("txtPackingItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT", "hdfPackingItem", true, false, "hdfPackingCat", true);
        }
        function InitPMCategory(selval) {

            GrandScriptUtils.MakeAutoComplete("txtPMCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=3", "hdfPMCatgoryPK", true, false, "BizUnitPk", true);
            if (selval == 1) {
                $("[id$=txtPMCategory]").val($("[id$=hdfSelectedCat]").val());
                $("[id$=hdfPMCatgoryPK]").val($("[id$=hdfSelectedCatpak]").val());
            }
            GrandScriptUtils.MakeAutoComplete("txtPMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT", "hdfPMItem", true, false, "hdfPMCatgoryPK", true);
            if (selval == 1) {
                $("[id$=txtPMItem]").val($("[id$=hdfSelectedMaterial]").val());
                $("[id$=hdfPMItem]").val($("[id$=hdfSelectedMaterialPK]").val());
            }
        }
        function InitPMMItem() {
            GrandScriptUtils.MakeAutoComplete("txtPMMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT", "hdfPMMItemPK", true, false, "hdfPMMCatPK", true);

        }
        function InitPMMCategory(selval) {
            GrandScriptUtils.MakeAutoComplete("txtPMMCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=3", "hdfPMMCatPK", true, false, "BizUnitPk", true);
            if (selval == 1) {
                $("[id$=txtPMMCategory]").val($("[id$=hdfSelectedPMMCATTEXT]").val());
                $("[id$=hdfPMMCatPK]").val($("[id$=hdfSelectedPMMCATPK]").val());
            }
            GrandScriptUtils.MakeAutoComplete("txtPMMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT", "hdfPMMItemPK", true, false, "hdfPMMCatPK", true);
            if (selval == 1) {
                //alert($("[id$=hdfSelectedMaterialPMM]").val());
                //alert($("[id$=hdfSelectedMaterialPKPMM]").val());

                $("[id$=txtPMMItem]").val($("[id$=hdfSelectedPMMTEXT]").val());
                $("[id$=hdfPMMItemPK]").val($("[id$=hdfSelectedPMMPK]").val());
                InitCust(1);
            }
            if ($("[id$=hdfSelectPMMCatPk]").val() != "0") {
                $("[id$=txtPMMCategory]").val($("[id$=hdfSelectPMMCat]").val());
                $("[id$=hdfPMMCatPK]").val($("[id$=hdfSelectPMMCatPk]").val());
                $("[id$=txtPMMItem]").val($("[id$=hdfSelectPMMMat]").val());
                $("[id$=hdfPMMItemPK]").val($("[id$=hdfSelectPMMMatPk]").val());
            }
        }
        function InitPMItem() {

            GrandScriptUtils.MakeAutoComplete("txtPMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT", "hdfPMItem", true, false, "hdfPMCatgoryPK", true);
        }
        function InitCustomerBrands() {

            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "?IsSBUCustomer=" + $("[id$=hdfIsSBUCustomer]").val(), "hdfCustomerID", true, true, 4, "CUSTOMERLIST");

            GrandScriptUtils.MakeAutoCompleteDDL("txtBrandProduct", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
        }
        function InitCustomerBrandsBOM(selval) {

            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomerBOM", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerIDBOM", true, true, 4, "CUSTOMERLIST");
            if (selval == 1) {
                $("[id$=txtCustomerBOM]").val($("[id$=hdfSelectedCat]").val());
                $("[id$=hdfCustomerIDBOM]").val($("[id$=hdfSelectedCatpak]").val());
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrandProductBOM", uiUrl + "?Type=" + $("[id$=hdfCustomerIDBOM]").val(), "hdfBrandBOM", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            if (selval == 1) {
                $("[id$=txtBrandProductBOM]").val($("[id$=hdfSelectedMaterial]").val());
                $("[id$=hdfBrandBOM]").val($("[id$=hdfSelectedMaterialPK]").val());
            }
        }

        function InitProducts() {

            GrandScriptUtils.MakeAutoCompleteDDL("txtItemProduct", url + "?Type=1&BinSubType=" + $("[id$=ddl_matcat]").val() + "&SBUPk=" + $("[id$=BizUnitPk]").val(), "hdfProductPK", true, true, "GETPRODUCTS");
        }
        function InitProductsBOM(selectval) {

            GrandScriptUtils.MakeAutoCompleteDDL("txtItemProductBOM", url + "?Type=1&BinSubType=" + $("[id$=ddlProductcatBOM]").val(), "hdfProductPKBOM", true, true, "GETPRODUCTS");
            if (selectval == 1) {

                $("[id$=txtItemProductBOM]").val($("[id$=hdfSelectedMaterial]").val());
                $("[id$=hdfProductPKBOM]").val($("[id$=hdfSelectedMaterialPK]").val());
            }
        }
        function InitBrands() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrandProduct", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            //alert( uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val());
        }
        function InitBrandsBOM() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrandProductBOM", uiUrl + "?Type=" + $("[id$=hdfCustomerIDBOM]").val(), "hdfBrandBOM", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            //alert( uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val());
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

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>          
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }

        }

        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag == 1) {//Listing
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=spnPackingSpecsMapping]").hide();

                $("[id$=PackMatMapping]").hide();

            }
            else if (flag == 2) {//Packing material Map
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=spnPackingSpecsMapping]").show();

                $("[id$=PackMatMapping]").show();

            }
            else {//BOM Tab
                $("[id$=PageAction_List]").hide();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=PageAction_Entry]").show();

                $("[id$=PackMatMapping]").hide();
                if ($("[id$=hdfItemType]").val() == 2) {

                    if ($("[id$=hdf_catpk]").val() == 2 || $("[id$=hdf_catpk]").val() == 3) {
                        $("[id$=spnPackingSpecsMapping]").show();
                    }
                    else {

                        $("[id$=spnPackingSpecsMapping]").hide();

                    }

                }
                else {
                    $("[id$=spnPackingSpecsMapping]").hide();
                }
            }
            return false;
        }
        function InitWOItem() {
            var selectItem = $("[id$=ddl_ItemType").val();
            switch (selectItem) {
                case "1":
                    FillMaterialAutoComplete(0);
                    break;

            }
        }
        // function InitBOMItem() {
        //     var selectItem = $("[id$=ddlWOItemType]").val();
        //    switch (selectItem) {
        //        case "1":
        //            FillBOMMaterialCategory(0);
        //            FillBOMMaterial(0);
        //            break;

        //    }
        //}
        function FillMaterialCategoryAutoComplete(SelectVal) {
            //<summary> Function Used to make material category field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("MaterialCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=1" + "&PM_WorkOrder=1", "MaterialCategoryPK", true, false, "BizUnitPk", true, false, false, false, true)
            if ($("[id$=hdfSelectCatpak]").val() != '0' && SelectVal == '1') {
                $("[id$=MaterialCategoryPK]").val($("[id$=hdfSelectCatpak]").val());
                $("[id$=MaterialCategory]").val($("[id$=hdfSelectCat]").val());

            }


            if ($("[id$=hdfSelectBOMCatPk]").val() != '0' && SelectVal == '1') {
                $("[id$=txtBOMCategory").val($("[id$=hdfSelectBOMCat]").val());
                $("[id$=txtBOMMaterial]").val($("[id$=hdfSelectBOMMat]").val());

            }

        }
        function FillMaterialAutoComplete(SelectVal) {
            //<summary> Function Used to make Item field as auto complete </summary>
            //if ($("[id$=hdfselectmaterialpk]").val() == "12")
            //    GrandScriptUtils.MakeAutoComplete("ItemCodeMaterial", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=0&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT", "MaterialPK", true, false, "MaterialCategoryPK", true);
            //else
            GrandScriptUtils.MakeAutoComplete("ItemCodeMaterial", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=0&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT", "MaterialPK", true, false, "MaterialCategoryPK", true);
            if ($("[id$=hdfSelectMaterialPK]").val() != '0' && SelectVal == '1') {
                $("[id$=MaterialPK]").val($("[id$=hdfSelectMaterialPK]").val());
                $("[id$=ItemCodeMaterial]").val($("[id$=hdfSelectMaterial]").val());

            }
        }
        function AfterAutoCompleteSelect(targetControlID) {
            //<summary> Function Used to an event fire after select category then fill material and uom </summary>
            if (targetControlID == "MaterialCategory") {
                FillMaterialAutoComplete(0);
            }
            if (targetControlID == "txtBOMCategory") {

                FillBOMMaterial();
            }
            if (targetControlID == "txtBOMMaterial") {
                FillMaterialDetails($("[id$=hdfBOMMaterialPK]").val());
            }
            if (targetControlID == "txtBrandProductBOM") {

                FillBrandDetails($("[id$=hdfBrandBOM]").val());
            }
            if (targetControlID == "txtPMItem") {
                FillMaterialDetails($("[id$=hdfPMItem]").val());
            }

            if (targetControlID == "txtPMMItem") {
                FillMaterialDetails($("[id$=hdfPMMItemPK]").val());
            }
            if (targetControlID == "txtItemProductBOM") {
                FillMaterialDetails($("[id$=hdfProductPKBOM]").val());
            }

            if (targetControlID == "txtCustomer") {
                InitBrands();
            }
            if (targetControlID == "txtPMCategory") {
                InitPMItem();

            }
            if (targetControlID == "txtCustomerBOM") {
                InitBrandsBOM();
            }
            if (targetControlID == "txtPackingCat") {
                InitWOIPMItem();
            }
            if (targetControlID == "txtBrandID") {
                FillBrandDetails($("[id$=hdfBrandID]").val());
            }
            if (targetControlID == "txtCustomerID") {
                $("[id$=hdfBrandID]").val("0");
                $("[id$=txtBrandID]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                if ($("[id$=hdfCustomerPK]").val() != "" && $("[id$=hdfCustomerPK]").val() != "0") {
                    EnableAuto($("[id$=txtBrandID]"));
                    GrandScriptUtils.MakeAutoCompleteDDL("txtBrandID", uiUrl + "?Type=" + $("[id$=hdfCustomerPK]").val(), "hdfBrandID", true, true, "CUTOMERBRAND");
                    //                    MakeExtAutoCompleteDDL("txtBrandID", urlauto + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrandID", true, true, "CUTOMERBRAND");
                    if ($("[id$=hdfCustomerPK]").val() == "" || $("[id$=hdfCustomerPK]").val() == "0") {
                        DisableAuto($("[id$=txtBrandID]"), $("[id$=hdfBrandID]"));
                    }
                }
                else {

                    DisableAuto($("[id$=txtBrandID]"), $("[id$=hdfBrandID]"));
                }

            }
            if (targetControlID == "txtPMMCategory") {
                InitPMMItem();
            }
        }

        function FillMaterialDetails(materialID) {

            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            $.get("MaterialManagement.do?Action=GetMaterialDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
                if (data) {
                    if (materialID != 0) {
                        if ($("[id$=hdfBOMMaterialPK]").val() > 0 || $("[id$=hdfProductPKBOM]").val() > 0 || $("[id$=hdfPMItem]").val() > 0) {
                            $("[id$=txtUOM]").val(data[0].UOM_CODE);
                            $("[id$=hdfItemUOM]").val(data[0].ITM_UOM);
                            $("[id$=dddlIssueUom]").val(data[0].ITM_UOM);
                        }

                        if ($("[id$=hdfBOMMaterialPK]").val() > 0) {
                            $("[id$=txtBOMCategory]").val(data[0].ITC_NAME);
                            $("[id$=hdfBOMCategoryPK]").val(data[0].ITM_CATEGORY);
                        }
                        if ($("[id$=hdfProductPKBOM]").val() > 0) {
                            $("[id$=dlProductcatBOM]").val(data[0].ITM_SUB_TYPE);
                        }
                        if ($("[id$=hdfPMItem]").val() > 0) {
                            $("[id$=txtPMCategory]").val(data[0].ITC_NAME);
                            $("[id$=hdfPMCatgoryPK]").val(data[0].ITM_CATEGORY);
                        }
                        //Autofill item category in pm mapping
                        if ($("[id$=hdfPMMItemPK]").val() > 0) {
                            $("[id$=txtPMMCategory]").val(data[0].ITC_NAME);
                            $("[id$=hdfPMMCatPK]").val(data[0].ITM_CATEGORY);
                            $("[id$=txtPMMUOM]").val(data[0].UOM_CODE);
                            $("[id$=hdfPMMUOMPK]").val(data[0].ITM_UOM);
                            $("[id$=ddlPMMIssueUom]").val(data[0].ITM_UOM);
                        }
                    }
                }
            });
        }


        function FillBrandDetails(brandPK) {
            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            $.get("MaterialManagement.do?Action=GetBrandDetails&BrandPK=" + brandPK, function (data) {
                if (data) {
                    if (brandPK != 0) {
                        $("[id$=txtUOM]").val(data[0].SOD_SALE_UOM_TEXT);
                        $("[id$=hdfItemUOM]").val(data[0].CIM_SALE_UOM);
                        $("[id$=dddlIssueUom]").val(data[0].SOD_SALE_UOM_TEXT);
                        $("[id$=txtCustomerBOM").val(data[0].CUS_NAME);
                        $("[id$=hdfCustomerIDBOM").val(data[0].CUS_PK);
                        if ($("[id$=hdfBrandID").val() != 0 || $("[id$=hdfBrandID").val() != "") {
                            $("[id$=hdfbrandcode").val(data[0].CIM_BRAND_CODE);
                        }
                    }

                }
            });
        }



        function InitBOMMAuto(selectVal) {


            if ($("[id$=hdfItemTypeBOM]").val() == 1) {

                FillBOMMaterialCategory(1);
                FillBOMMaterial(1);
            }
            else {

                FillBOMMaterialCategory();
                FillBOMMaterial();
            }
            if ($("[id$=hdfItemTypeBOM]").val() == 2) {
                InitProductsBOM(selectVal);
            }
            if ($("[id$=hdfItemTypeBOM]").val() == 3) {
                InitCustomerBrandsBOM(1);
            }
            if ($("[id$=hdfItemTypeBOM]").val() == 4) {

                InitPMCategory(selectVal);
            }
            $("[id*=txtQty]").ForceNumericOnly();
            $("[id*=txtQuantity]").ForceNumericOnly();
            $("[id*=txtIssue_Qty]").ForceNumericOnly();

        }


        function FillBOMMaterialCategory(selectVal) {

            //<summary> Function Used to make material category field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("txtBOMCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=1" + "&PM_WorkOrder=1", "hdfBOMCategoryPK", true, false, "BizUnitPk", true);
            if (selectVal == 1) {

                $("[id$=txtBOMCategory]").val($("[id$=hdfSelectedCat]").val());
                $("[id$=hdfBOMCategoryPK]").val($("[id$=hdfSelectedCatpak]").val());

            }

        }
        function FillBOMMaterial(selectVal) {

            //<summary> Function Used to make Item field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("txtBOMMaterial", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=0" + "&FLDNAME=ITM_TEXT", "hdfBOMMaterialPK", true, false, "hdfBOMCategoryPK", true);
            if (selectVal == 1) {
                $("[id$=txtBOMMaterial]").val($("[id$=hdfSelectedMaterial]").val());
                $("[id$=hdfBOMMaterialPK]").val($("[id$=hdfSelectedMaterialPK]").val());

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
                    //                    else {
                    //                        Page_Validators.splice(i, 1);
                    //                    }
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ValidatePage(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsgPmMap]").hide();
                ShowErrorMessage($("#PmMap").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlWOItem">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="26" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>" ValidationGroup="WorkOrderItemSave" />
                                        <%-- OnClientClick="javascript:ValidatePageNow('Employee')" --%>
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" CommandName="CANCEL" TabIndex="27" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                        <%-- OnClientClick="javascript:ValidatePageNow('Employee')" --%>
                                    </li>
                                </ul>
                                <%--  <ul runat="server" id="pnlMapEntry" style="display: none">
                                    <li runat="server" id="pnlPackMapEntry">
                                        <asp:Button runat="server" ID="btnPackMapSave" CommandName="SAVE" TabIndex="27" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>" ValidationGroup="WorkOrderItemSave" />
                                        
                                    </li>
                                    <li runat="server" id="pnlPackMapCancel">
                                        <asp:Button runat="server" ID="btnPackMapCancel" CommandName="CANCEL" TabIndex="28" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Cancel %>" />
                                     
                                    </li>
                                </ul>--%>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div id="divTabContainer" class="tab-container" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnWOItemList" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnWorkOrderItems" Text="<%$resources:WorkOrderItems %>"
                                CommandName="LIST" OnClick="ActionHandler"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBOM" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnBOM" Text="<%$resources:BOM %>"
                                CommandName="ADDITION" OnClick="ActionHandler"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPackingSpecsMapping" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lblPackingSpecsMapping" runat="server" Text="<%$ resources:PMMapping %>"
                                CommandName="PACKINGMATERIALS" CssClass="tab-active" OnClick="ActionHandler" TabIndex="3"
                                ToolTip="<%$ resources:PMMapping %>" />
                        </span></li>
                    </ul>
                </div>
            </div>

            <div class="content-wrapper">
                <div id="PackMatMapping" runat="server">

                    <table class="pack-material" id="PackingHeaderDiv" style="height: 57px !important;">
                        <tr style="height: 24%;">
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width: 5%"></td>
                            <td style="width: 45%">
                                <%--<div class="div2col-S">--%>
                                <asp:Label ID="lblcode" runat="server" AssociatedControlID="lblItmcode" Text="<%$ resources:ItemCode %>"></asp:Label>:
                            <asp:Label ID="lblItmcode" runat="server" Text=""></asp:Label>
                                <%--  </div>--%>

                            </td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblname" runat="server" AssociatedControlID="lblItmname"
                                    Text="<%$ resources:ItemName %>"></asp:Label>:
                            <asp:Label ID="lblItmname" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>

                    </table>
                    <asp:Table ID="tbladvancedSrch" runat="server" CssClass="asptbllinks">
                        <asp:TableRow ID="PageActionList" runat="server">
                            <asp:TableCell>

                                <table class="table-devide" id="tbladvanceSrch" style="margin-top: 8px;">
                                    <tr id="Tr1" runat="server">
                                        <td style="width: 5%"></td>
                                        <td style="width: 45%">
                                            <div class="">
                                                <asp:Label ID="lblCustomerSearch" runat="server" Text="<%$ resources:Controls,Customer %>"
                                                    AssociatedControlID="txtCustomerID"></asp:Label>
                                                <asp:TextBox ID="txtCustomerID" runat="server" CssClass="input-half" MaxLength="100" TabIndex="6"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfCustomerPK" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvCust" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="PMM"
                                                    EnableClientScript="true" runat="server" Display="Dynamic" Text="*" ControlToValidate="txtCustomerID"
                                                    ErrorMessage="<%$ resources:Err_Customer %>" />
                                                <div class="clear">
                                                </div>

                                            </div>
                                        </td>
                                        <td>
                                            <div class="">
                                                <asp:Label ID="lblBrandSearch" runat="server" Text="<%$ resources:Controls,Brand%>"
                                                    AssociatedControlID="txtBrandID" />
                                                <asp:TextBox ID="txtBrandID" runat="server" CssClass="input-half-20-11-9 BrandWidth" TabIndex="7" />
                                                <asp:HiddenField ID="hdfBrandID" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="PMM"
                                                    EnableClientScript="true" runat="server" Display="Dynamic" Text="*" ControlToValidate="txtBrandID"
                                                    ErrorMessage="<%$ resources:Err_Brand %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch" CssClass="middle-lbl-xsmall-d style-none"></asp:Label>
                                                <asp:ImageButton ID="imgsrch" runat="server" ToolTip="<%$ resources:Controls,Search %>" Visible="false"
                                                    ValidationGroup="Search" OnClick="ActionHandler" TabIndex="10" CommandName="SEARCH" SkinID="search-ext" />
                                                <asp:ImageButton ID="imgclear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="11"
                                                    ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEAR"
                                                    SkinID="clear-ext" Visible="false" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear">
                                </div>

                                <h1 class="search-colapse-normal">
                                    <%=Resources.Captions.PMMapping%>
                                </h1>
                                <table class="gridwraptable" style="width: 100% !important">
                                    <tr>
                                        <th align="left" style="width: 1%"><%=GetLocalResourceObject("Operation_Work").ToString()%>
                                            <asp:RequiredFieldValidator ID="vrfPmmOperationWork" CssClass="star" SetFocusOnError="true" ValidationGroup="PMM"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlPMMOperationWork"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Operation_Work %>">
                                            </asp:RequiredFieldValidator>
                                        </th>

                                        <th align="left" style="width: 1%" id="th1" runat="server"><%=GetLocalResourceObject("Category").ToString()%>
                                            <asp:RequiredFieldValidator ID="vrfPmmCategory" CssClass="star" SetFocusOnError="true" Visible="true"
                                                ValidationGroup="PMM" EnableClientScript="true" runat="server" ControlToValidate="txtPMMCategory"
                                                Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_BomCategory%>">
                                            </asp:RequiredFieldValidator>

                                        </th>

                                        <th align="left" style="width: 75%"><%=GetLocalResourceObject("Item").ToString()%>
                                            <asp:RequiredFieldValidator ID="vrfPmmItem" CssClass="star" SetFocusOnError="true" Visible="true"
                                                ValidationGroup="PMM" EnableClientScript="true" runat="server" ControlToValidate="txtPMMItem"
                                                Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                ErrorMessage="<%$ resources:Err_BomMaterial%>">
                                            </asp:RequiredFieldValidator>

                                        </th>
                                        <th align="left" style="width: 4%"><%=Resources.Controls.UOM %>
                                      
                                        </th>
                                        <th align="left" style="width: 5%"><%=GetLocalResourceObject("Qty").ToString()%>
                                            <asp:RequiredFieldValidator ID="vrfPmmQty" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="PMM" EnableClientScript="true" runat="server" ControlToValidate="txtPMMQty"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                            </asp:RequiredFieldValidator>
                                        </th>
                                        <th align="left" style="width: 2%"><%=GetLocalResourceObject("IssuingUOM") %>
                                            <asp:RequiredFieldValidator ID="vrfPmmIssueUom" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="PMM" EnableClientScript="true" runat="server" ControlToValidate="ddlPMMIssueUom"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IssueUom %>" InitialValue="<%$ resources:ddlDefaultValue %>">
                                            </asp:RequiredFieldValidator>
                                        </th>
                                        <th align="left" style="width: 1%"><%=GetLocalResourceObject("IssuingQty") %>
                                            <asp:RequiredFieldValidator ID="vrfPmmIssueQty" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="PMM" EnableClientScript="true" runat="server" ControlToValidate="txtPMMIssueQty"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Issue_Qty %>">
                                            </asp:RequiredFieldValidator>
                                        </th>

                                        <th align="left" style="width: 1%"><%=GetLocalResourceObject("Tolerance").ToString()%>
                                            <asp:RequiredFieldValidator ID="vrfPmmTolerance" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="PMM" EnableClientScript="true" runat="server" ControlToValidate="txtPMMTolerance"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Tolerance %>">
                                            </asp:RequiredFieldValidator>
                                        </th>
                                        <th align="left" style="width: 10%"><%=GetLocalResourceObject("In").ToString()%></th>
                                        <th style="width: 2%"></th>
                                        <th style="width: 3%"></th>
                                    </tr>
                                    <tr class="grd-rowhead">
                                        <td>
                                            <asp:DropDownList ID="ddlPMMOperationWork" runat="server" TabIndex="14" Width="93%">
                                            </asp:DropDownList>


                                        </td>

                                        <td runat="server" id="td4">

                                            <%--      <asp:Label ID="Label5" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomerBOM"></asp:Label>--%>
                                            <asp:TextBox ID="txtPMMCategory" runat="server" CssClass="input-half-05-01-2021 margnbotm0" MaxLength="100"
                                                TabIndex="6"> </asp:TextBox>

                                            <asp:HiddenField ID="hdfPMMCatPK" runat="server" Value="0" />

                                        </td>

                                        <td runat="server" id="td5">
                                            <asp:TextBox ID="txtPMMItem" runat="server" TabIndex="17" Width="96%"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPMMItemPK" runat="server" Value="0" />

                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtPMMUOM" runat="server" Enabled="false" CssClass="" Width="100%"></asp:TextBox>
                                            <asp:HiddenField ID="hdfPMMUOMPK" runat="server" Value="0" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPMMQty" runat="server" CssClass="numeric" Width="91%" TabIndex="18"></asp:TextBox>

                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPMMIssueUom" runat="server" TabIndex="19" Width="100%">
                                            </asp:DropDownList>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPMMIssueQty" runat="server" CssClass="numeric" Width="91%" TabIndex="20"></asp:TextBox>

                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtPMMTolerance" runat="server" CssClass="numeric" Width="91%" TabIndex="21"></asp:TextBox>

                                        </td>

                                        <td>
                                            <asp:DropDownList ID="ddlPMMType" runat="server" TabIndex="22" Width="100%">
                                            </asp:DropDownList>

                                        </td>

                                        <td>
                                            <asp:ImageButton runat="server" ID="btnAddPmMap" CommandName="ADDNEW" TabIndex="23"
                                                OnClick="ActionHandler"
                                                ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="PMM"
                                                SkinID="plus" OnClientClick="javascript:ValidatePage('PMM');" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdPackMaterialDetail" Width="100%" PageSize="<%$ resources:PageSize%>"
                                        AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">

                                        <EmptyDataTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>

                                            <asp:TemplateField HeaderText="<%$ resources:Customer %>"><%--SortExpression="OperationText"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMCustomer" runat="server" ToolTip='<%# Eval("CustomerName") %>'
                                                        Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>"><%--SortExpression="BomItemTypeText"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMBrand" runat="server" ToolTip='<%# Eval("BrandName") %>'
                                                        Text='<%# Eval("BrandCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5.5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Operation_Work %>"><%--SortExpression="BomItemTypeText"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMOperationWork" runat="server" ToolTip='<%# Eval("OperationText") %>'
                                                        Text='<%# Eval("OperationText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <%----%>

                                            <asp:TemplateField HeaderText="<%$ resources:Category %>"><%--SortExpression="BomMaterialCat"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMCategory" runat="server" ToolTip='<%# Eval("BomMaterialCat") %>'
                                                        Text='<%# Eval("BomMaterialCatText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Item %>"><%--SortExpression="BOMaterial"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMItem" runat="server" ToolTip='<%# Eval("BOMaterial") %>'
                                                        Text='<%# Eval("BOMaterial") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ resources:UOM%>"><%--SortExpression="UomText"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMUomText" runat="server" ToolTip='<%# Eval("UomText")%>'
                                                        Text='<%# Eval("UomText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>"><%--SortExpression="QTY"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMQty" runat="server" ToolTip='<%# Eval("QTY")%>'
                                                        Text='<%# Eval("QTY") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                            </asp:TemplateField>



                                            <asp:TemplateField HeaderText="<%$ resources:IssuingUOM %>"><%--SortExpression="IssueUomText"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMIssue_UOM" runat="server" ToolTip='<%# Eval("IssueUomText")%>'
                                                        Text='<%# Eval("IssueUomText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:IssuingQty %>"><%--SortExpression="IssueQty"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMIssue_Qty" runat="server" ToolTip='<%# Eval("IssueQty")%>'
                                                        Text='<%# Eval("IssueQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ resources:Tolerance %>"><%--SortExpression="Tolerance"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMTolerance" runat="server" ToolTip='<%# Eval("Tolerance")%>'
                                                        Text='<%# Eval("Tolerance") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$ resources:In %>"><%--SortExpression="BomTypeText"--%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPMMIn" runat="server" ToolTip='<%# Eval("BomTypeText")%>'
                                                        Text='<%# Eval("BomTypeText") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkEdit" TabIndex="24" runat="server" OnClick="ActionHandler" SkinID="edit-icon"
                                                        CommandName="EDITPACKMATITEM" ToolTip="Edit" CommandArgument='<%# Eval("SlNo")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkRemove" TabIndex="25" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                        OnClientClick="return ShowDeleteConfirm(this);"
                                                        CommandName="REMOVEPACKMATITEM" ToolTip="Remove" CommandArgument='<%# Eval("SlNo")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <%-- <uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>

                            <%--=========Advance Search Region Begin=========================--%>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr>
                                    <td style="width: 10% !important;">

                                        <div class="divcol-Total padgtop7">
                                            <asp:Label runat="server" ID="lblItemType" Text="<%$ resources:ItemType%>"
                                                AssociatedControlID="ddl_ItemType"></asp:Label>
                                            <asp:DropDownList ID="ddl_ItemType" runat="server" onchange="javascript:InitWOItem();" TabIndex="1" Width="46%" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                        </div>
                                    </td>

                                    <td style="width: 5% !important;" runat="server" id="tdItemCategory">
                                        <div class="div2col-S padgtop7">

                                            <asp:Label runat="server" ID="lblCategory" Text="<%$ resources:Category%>"
                                                AssociatedControlID="MaterialCategory"></asp:Label>
                                            <asp:TextBox ID="MaterialCategory" CssClass="input-half-05-01-2021" runat="server" TabIndex="2">
                                            </asp:TextBox>

                                            <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                                        </div>
                                    </td>
                                    <td style="width: 7% !important;" runat="server" id="tdPrdCategory" visible="false">
                                        <div class="div2col-S padgtop7">

                                            <asp:Label runat="server" ID="Label2" Text="<%$ resources:Category%>"
                                                AssociatedControlID="ddl_matcat" CssClass="lbl_cat_width"></asp:Label>
                                            <asp:DropDownList ID="ddl_matcat" onchange="javascript:InitProducts();" CssClass="input-half-05-01-2021 maxwidth" runat="server" TabIndex="4" Width="68%" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="0"></asp:HiddenField>
                                        </div>
                                    </td>
                                    <td style="width: 6% !important;" runat="server" id="tdBrandCategory" visible="false">
                                        <div class="div2col-S padgtop7">

                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half-05-01-2021 margnbotm0" MaxLength="100"
                                                TabIndex="6"> </asp:TextBox>

                                            <asp:HiddenField ID="hdfCustomerID" runat="server" Value="0" />
                                        </div>
                                    </td>

                                    <td runat="server" id="tdPackingCat" visible="false" style="width: 6% !important;">
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="Label3" Text="<%$ resources:Category%>"
                                                AssociatedControlID="txtPackingCat"></asp:Label>
                                            <asp:TextBox ID="txtPackingCat" runat="server" CssClass="input-half-05-01-2021 margnbotm0" MaxLength="100"
                                                TabIndex="6"> </asp:TextBox>

                                            <asp:HiddenField ID="hdfPackingCat" runat="server" Value="0" />
                                        </div>
                                    </td>





                                    <td style="width: 20% !important;" runat="server" id="tdMaterial">
                                        <div class="div2col-S padgtop7">

                                            <asp:Label runat="server" ID="lblItemCodeName" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="ItemCodeMaterial"></asp:Label>
                                            <asp:TextBox runat="server" ID="ItemCodeMaterial" CssClass="input-small-e-05-01-2021 ddlItemWidth" TabIndex="3">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>
                                        </div>
                                    </td>

                                    <td style="width: 20% !important;" runat="server" id="tdProduct" visible="false">
                                        <div class="div2col-S padgtop7">

                                            <asp:Label runat="server" ID="lblProduct" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="txtItemProduct"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtItemProduct" CssClass="input-small-e-05-01-2021" TabIndex="5" Width="91%">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfProductPK" runat="server" Value="0"></asp:HiddenField>


                                        </div>
                                    </td>
                                    <td style="width: 22% !important;" runat="server" id="tdBrand" visible="false">
                                        <div class="div2col-S padgtop7">

                                            <asp:Label runat="server" ID="Label4" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="txtBrandProduct"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBrandProduct" CssClass="input-small-e-05-01-2021" TabIndex="7" Width="92%">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfBrand" runat="server" Value="0"></asp:HiddenField>


                                        </div>
                                    </td>
                                    <td runat="server" id="tdPackingItem" visible="false" style="width: 22% !important;">

                                        <div class="div2col-S padgtop7">

                                            <asp:Label runat="server" ID="Label5" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="txtBrandProduct"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPackingItem" CssClass="input-small-e-05-01-2021" TabIndex="7" Width="92%">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfPackingItem" runat="server" Value="0"></asp:HiddenField>
                                        </div>
                                    </td>
                                    <td style="width: 9%">
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblStatus" runat="server" Text="<%$Resources:Controls,Status%>" CssClass="lbl-10-7perc-04-01-2021"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="lbl-10-7perc-04-01-2021" TabIndex="8">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Active %>" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Inactive %>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="ChkMappedItem" runat="server" Checked="true" ToolTip="<%$ resources:MappedItems %>" CssClass="style-none margnbotm0"
                                                Style="margin-bottom: -6px !important; margin-left: 2px !important;" TabIndex="9" />
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                TabIndex="10" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                OnClick="ActionHandler" CommandName="SEARCH" OnClientClick="javascript:return BindGrid();" />
                                            <asp:ImageButton ID="btnClear" runat="server" TabIndex="11" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" />
                                        </div>

                                    </td>
                                </tr>
                                <tr runat="server" id="trRow2" visible="false">
                                    <td colspan="5">
                                        <div class="divcol-Total padgtop7">
                                            <asp:Label CssClass="width11" runat="server" ID="lblCustomer_Search" Text="<%$ resources:Controls,Customer %>"
                                                AssociatedControlID="txtBrandProduct"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCustomerSearch" CssClass="input-small-c-25-3" TabIndex="7">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerSearchPK" runat="server" Value="0" />
                                        </div>
                                    </td>
                                </tr>
                               
                            </table>
                            <%--=============End Advance Search Region=====================--%>


                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdWOItems" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">

                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblmsg" runat="server" TabIndex="23" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="12" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Category %>" SortExpression="ITC_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" ToolTip='<%# Eval("ITC_NAME")%>'
                                                    Text='<%# Eval("ITC_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hdf_catpk" runat="server" Value='<%# Eval("ITC_PK")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:ItemCode %>" SortExpression="ITM_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" ToolTip='<%# Eval("ITM_CODE") %>'
                                                    Text='<%# Eval("ITM_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ItemName %>" SortExpression="ITM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" ToolTip='<%# Eval("ITM_NAME")%>'
                                                    Text='<%# Eval("ITM_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="53%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="UOM_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" ToolTip='<%# Eval("UOM_NAME")%>'
                                                    Text='<%# Eval("UOM_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hdf_uompk" runat="server" Value='<%# Eval("UOM_PK")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="<%$ resources:MinStock %>" SortExpression="ITM_MIN_STK">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMinStock" runat="server" ToolTip='<%# Eval("ITM_MIN_STK")%>'
                                                    Text='<%# Eval("ITM_MIN_STK") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MaxStock %>" SortExpression="ITM_MAX_STK">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaxStock" runat="server" ToolTip='<%# Eval("ITM_MAX_STK")%>'
                                                    Text='<%# Eval("ITM_MAX_STK") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:IsMapped %>" SortExpression="ITM_ACTIVE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIsMapped" runat="server" ToolTip='<%# (Eval("WO_COUNT")).ToString()=="0"? Resources.ErpRes.NotMapped :Resources.ErpRes.Mapped%>'
                                                    Text='<%# (Eval("WO_COUNT")).ToString()=="0" ? Resources.ErpRes.NotMapped :Resources.ErpRes.Mapped %>'></asp:Label>

                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ErpRes,Status  %>" SortExpression="ITM_ACTIVE">
                                            <ItemTemplate>
                                                <image id="imgStatus" title='<%# (Eval("ITM_ACTIVE")).ToString()=="1"? Resources.ErpRes.Active :Resources.ErpRes.InActive %>'
                                                    class='<%# (Eval("ITM_ACTIVE")).ToString()=="1"?"active" :"inactive"%>'
                                                    alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">

                        <asp:TableCell>
                            <div class="contentwrapper">


                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblICode" runat="server" AssociatedControlID="lblItemCode" Text="<%$ resources:ItemCode %>"></asp:Label>
                                                <asp:Label ID="lblItemCode" runat="server" CssClass="input-half"></asp:Label>


                                            </div>

                                        </td>

                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblIName" runat="server" AssociatedControlID="lblItemName" Text="<%$ resources:ItemName %>"></asp:Label>
                                                <asp:Label ID="lblItemName" runat="server" CssClass="input-half lblItemNameheight"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">

                                                <asp:Label ID="lbl_IType" runat="server" AssociatedControlID="lbl_ItemType" Text="<%$ resources:ItemType %>"
                                                    CssClass="Typewidth"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lbl_ItemType" runat="server" CssClass="input-half Typewidth-23-2"></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lbl_Uom" runat="server" AssociatedControlID="lblUOM" Text="<%$ resources:UOM %>"></asp:Label>
                                                <asp:Label ID="lblUOM" runat="server" CssClass="lblUom-23-2" Width="5%"></asp:Label>
                                                <asp:Label ID="lblQuantity" runat="server" AssociatedControlID="txtQuantity" Text="<%$ resources:Qty %>" CssClass="lblQuantity"></asp:Label>
                                                <asp:TextBox ID="txtQuantity" runat="server" TabIndex="13" Width="14%" Text="1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrf_qty" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="WorkOrderItemSave" EnableClientScript="true" runat="server" ControlToValidate="txtQuantity"
                                                    Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_Qty%>">
                                                </asp:RequiredFieldValidator>




                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <h1 class="search-colapse-normal">
                                <%=Resources.Captions.WODetails%>
                            </h1>
                            <table class="gridwraptable" style="width: 100% !important">
                                <tr>
                                    <th align="left" style="width: 1%"><%=GetLocalResourceObject("Operation_Work").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfOperation" CssClass="star" SetFocusOnError="true" ValidationGroup="WorkOrderItem"
                                            EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlOperation"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Operation_Work %>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 1%"><%=GetLocalResourceObject("ItemType").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfItemType" CssClass="star" SetFocusOnError="true" ValidationGroup="WorkOrderItem"
                                            EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlWOItemType"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ItemType %>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 1%" id="thcathead" runat="server"><%=GetLocalResourceObject("Category").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfBOMCategory" CssClass="star" SetFocusOnError="true" Visible="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtBOMCategory"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                            ErrorMessage="<%$ resources:Err_BomCategory%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="vrfprdcat" CssClass="star" SetFocusOnError="true" Visible="false"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="ddlProductcatBOM"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:ddlDefaultValue%>"
                                            ErrorMessage="<%$ resources:Err_BomCategory%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="vrfpmcat" CssClass="star" SetFocusOnError="true" Visible="false"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtPMCategory"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue%>"
                                            ErrorMessage="<%$ resources:Err_BomCategory%>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <%--id="thcathead" runat="server" visible="false"--%>
                                    <th align="left" style="width: 1%" visible="false" id="thcusthead" runat="server"><%=GetLocalResourceObject("Customer").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfcust" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtCustomerBOM"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                            ErrorMessage="<%$ resources:Err_Customer%>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 75%"><%=GetLocalResourceObject("Item").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfBomMaterial" CssClass="star" SetFocusOnError="true" Visible="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtBOMMaterial"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                            ErrorMessage="<%$ resources:Err_BomMaterial%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="vrfprdItem" CssClass="star" SetFocusOnError="true" Visible="false"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtItemProductBOM"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                            ErrorMessage="<%$ resources:Err_BomMaterial%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="vrfcustItem" CssClass="star" SetFocusOnError="true" Visible="false"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtBrandProductBOM"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                            ErrorMessage="<%$ resources:Err_BomMaterial%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="vrfpmItem" CssClass="star" SetFocusOnError="true" Visible="false"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtPMItem"
                                            Display="Dynamic" Text="*" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                            ErrorMessage="<%$ resources:Err_BomMaterial%>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 4%"><%=Resources.Controls.UOM %>
                                      
                                    </th>
                                    <th align="left" style="width: 5%"><%=GetLocalResourceObject("Qty").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfQty" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 2%"><%=GetLocalResourceObject("IssuingUOM") %>
                                        <asp:RequiredFieldValidator ID="vrfIssueUom" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="dddlIssueUom"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IssueUom %>" InitialValue="<%$ resources:ddlDefaultValue %>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 1%"><%=GetLocalResourceObject("IssuingQty") %>
                                        <asp:RequiredFieldValidator ID="vrf_IssueQty" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtIssue_Qty"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Issue_Qty %>">
                                        </asp:RequiredFieldValidator>
                                    </th>

                                    <th align="left" style="width: 1%"><%=GetLocalResourceObject("Tolerance").ToString()%>
                                        <asp:RequiredFieldValidator ID="vrfTolerance" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="WorkOrderItem" EnableClientScript="true" runat="server" ControlToValidate="txtTolerance"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Tolerance %>">
                                        </asp:RequiredFieldValidator>
                                    </th>
                                    <th align="left" style="width: 10%"><%=GetLocalResourceObject("In").ToString()%></th>
                                    <th style="width: 2%"></th>
                                    <th style="width: 3%"></th>
                                </tr>
                                <tr class="grd-rowhead">
                                    <td>
                                        <asp:DropDownList ID="ddlOperation" runat="server" TabIndex="14" Width="93%">
                                        </asp:DropDownList>


                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlWOItemType" runat="server" TabIndex="15" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList><%--onchange="javascript:InitWOItem();"--%>
                                        <asp:HiddenField ID="hdfBOMItemTypePK" runat="server" Value="0" />
                                    </td>
                                    <td id="tdItemCategoryBOM" runat="server">
                                        <asp:TextBox ID="txtBOMCategory" runat="server" TabIndex="16"></asp:TextBox>
                                        <asp:HiddenField ID="hdfBOMCategoryPK" runat="server" Value="0" />

                                    </td>
                                    <td runat="server" id="tdPrdCategoryBOM" visible="false">
                                        <%--<asp:Label runat="server" ID="Label3" Text="<%$ resources:Category%>"
                                                AssociatedControlID="ddl_matcatBOM" CssClass="lbl_cat_width"></asp:Label>--%>
                                        <asp:DropDownList ID="ddlProductcatBOM" onchange="javascript:InitProductsBOM();" CssClass="input-half-05-01-2021" runat="server" TabIndex="4">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="0"></asp:HiddenField>

                                    </td>
                                    <td runat="server" id="tdBrandCategoryBOM" visible="false">

                                        <%--      <asp:Label ID="Label5" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomerBOM"></asp:Label>--%>
                                        <asp:TextBox ID="txtCustomerBOM" runat="server" CssClass="input-half-05-01-2021 margnbotm0" MaxLength="100"
                                            TabIndex="6"> </asp:TextBox>

                                        <asp:HiddenField ID="hdfCustomerIDBOM" runat="server" Value="0" />

                                    </td>
                                    <td runat="server" id="tdPMCatBOM" visible="false">

                                        <%--      <asp:Label ID="Label5" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomerBOM"></asp:Label>--%>
                                        <asp:TextBox ID="txtPMCategory" runat="server" CssClass="input-half-05-01-2021 margnbotm0" MaxLength="100"
                                            TabIndex="6"> </asp:TextBox>

                                        <asp:HiddenField ID="hdfPMCatgoryPK" runat="server" Value="0" />

                                    </td>

                                    <td runat="server" id="tdMaterialBOM">
                                        <asp:TextBox ID="txtBOMMaterial" runat="server" TabIndex="17" Width="96%"></asp:TextBox>
                                        <asp:HiddenField ID="hdfBOMMaterialPK" runat="server" Value="0" />

                                    </td>
                                    <td runat="server" id="tdProductBOM" visible="false">

                                        <%--<asp:Label runat="server" ID="Label6" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="txtItemProductBOM"></asp:Label>--%>
                                        <asp:TextBox runat="server" ID="txtItemProductBOM" CssClass="input-small-e-05-01-2021" TabIndex="5" Width="96%">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdfProductPKBOM" runat="server" Value="0"></asp:HiddenField>



                                    </td>
                                    <td runat="server" id="tdBrandBOM" visible="false">

                                        <%-- <asp:Label runat="server" ID="Label7" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="txtBrandProductBOM"></asp:Label>--%>
                                        <asp:TextBox runat="server" ID="txtBrandProductBOM" CssClass="input-small-e-05-01-2021" TabIndex="7" Width="96%">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdfBrandBOM" runat="server" Value="0"></asp:HiddenField>

                                    </td>
                                    <td runat="server" id="tdPMItem" visible="false">

                                        <%-- <asp:Label runat="server" ID="Label7" Text="<%$ Resources:BindValues, Item%>"
                                                CssClass="lbl-10-7perc-04-01-2021" AssociatedControlID="txtBrandProductBOM"></asp:Label>--%>
                                        <asp:TextBox runat="server" ID="txtPMItem" CssClass="input-small-e-05-01-2021" TabIndex="7" Width="96%">
                                        </asp:TextBox>


                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUOM" runat="server" Enabled="false" CssClass="" Width="100%"></asp:TextBox>
                                        <asp:HiddenField ID="hdfItemUOM" runat="server" Value="0" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="numeric" Width="91%" TabIndex="18"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dddlIssueUom" runat="server" TabIndex="19" Width="100%">
                                        </asp:DropDownList>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIssue_Qty" runat="server" CssClass="numeric" Width="91%" TabIndex="20"></asp:TextBox>

                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtTolerance" runat="server" CssClass="numeric" Width="91%" TabIndex="21"></asp:TextBox>

                                    </td>

                                    <td>
                                        <asp:DropDownList ID="ddlBOMType" runat="server" TabIndex="22" Width="100%">
                                        </asp:DropDownList>

                                    </td>

                                    <td>
                                        <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADD" TabIndex="23"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePage('WorkOrderItem');"
                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="WorkOrderItem"
                                            SkinID="plus" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>

                            <asp:HiddenField ID="hdfPMItem" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectCat" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectCatpak" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectMaterial" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectMaterialPK" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectItem" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectItemPK" runat="server" Value="0" />
                            <%--<asp:HiddenField ID="hdfSelectBomItemType" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectBomItemTypePK" runat="server" Value="-1" />
                             <asp:HiddenField ID="hdfSelectIssueUom" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectIssueUomPK" runat="server" Value="-1" />--%>

                            <asp:HiddenField ID="hdfSelectBOMCatPk" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectBOMCat" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectBOMMatPk" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectBOMMat" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfWOIUOM" runat="server" Value="-1" />

                            <asp:HiddenField ID="hdfSelectPMMCatPk" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectPMMCat" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectPMMMatPk" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectPMMMat" runat="server" Value="-1" />

                            <%-- For filling category&material -edit--%>
                            <asp:HiddenField ID="hdfSelectedCat" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectedCatpak" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectedMaterial" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectedMaterialPK" runat="server" Value="0" />

                            <%-- For filling category&packingmaterial -edit--%>
                            <asp:HiddenField ID="hdfSelectedPMMCATPK" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectedPMMCATTEXT" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectedPMMTEXT" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectedPMMPK" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectedCustomerPK" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectedCustomer" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectedBrandPK" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfSelectedBrand" runat="server" Value="0" />
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdBOM" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">

                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>

                                        <asp:TemplateField HeaderText="<%$ resources:Operation_Work %>"><%--SortExpression="OperationText"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOperationText" runat="server" ToolTip='<%# Eval("OperationText") %>'
                                                    Text='<%# Eval("OperationText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:ItemType %>"><%--SortExpression="BomItemTypeText"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBomItemType" runat="server" ToolTip='<%# Eval("BomItemTypeText") %>'
                                                    Text='<%# Eval("BomItemTypeText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10.5%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:Category %>"><%--SortExpression="BomMaterialCat"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBOMaterialCat" runat="server" ToolTip='<%# Eval("BomMaterialCat") %>'
                                                    Text='<%# Eval("BomMaterialCatText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>"><%--SortExpression="BOMaterial"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBOMaterial" runat="server" ToolTip='<%# Eval("BOMaterial") %>'
                                                    Text='<%# Eval("BOMaterial") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:UOM%>"><%--SortExpression="UomText"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUomText" runat="server" ToolTip='<%# Eval("UomText")%>'
                                                    Text='<%# Eval("UomText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>"><%--SortExpression="QTY"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" ToolTip='<%# Eval("QTY")%>'
                                                    Text='<%#String.Format("{0:#.0000}", Eval("QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>



                                        <asp:TemplateField HeaderText="<%$ resources:IssuingUOM %>"><%--SortExpression="IssueUomText"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssue_UOM" runat="server" ToolTip='<%# Eval("IssueUomText")%>'
                                                    Text='<%# Eval("IssueUomText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssuingQty %>"><%--SortExpression="IssueQty"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssue_Qty" runat="server" ToolTip='<%# Eval("IssueQty")%>'
                                                    Text='<%#String.Format("{0:#.0000}", Eval("IssueQty")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:Tolerance %>"><%--SortExpression="Tolerance"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTolerance" runat="server" ToolTip='<%# Eval("Tolerance")%>'
                                                    Text='<%# Convert.ToInt32(Eval("Tolerance"))<=0?"0.0000":String.Format("{0:#.0000}", Eval("Tolerance")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:In %>"><%--SortExpression="BomTypeText"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblIn" runat="server" ToolTip='<%# Eval("BomTypeText")%>'
                                                    Text='<%# Eval("BomTypeText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkEdit" TabIndex="24" runat="server" OnClick="ActionHandler" SkinID="edit-icon"
                                                    CommandName="EDITITEM" ToolTip="Edit" CommandArgument='<%# Eval("SlNo")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" TabIndex="25" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    OnClientClick="return ShowDeleteConfirm(this);"
                                                    CommandName="REMOVE" ToolTip="Delete" CommandArgument='<%# Eval("SlNo")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:HiddenField ID="hdfWOICat" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
                <asp:HiddenField ID="hdfItemType" runat="server" Value="1" />
                <asp:HiddenField ID="hdfItemTypeBOM" runat="server" Value="0" />
                <asp:HiddenField ID="hdfItemTypeText" runat="server" Value="Material" />
                <asp:HiddenField ID="hdfWOUomPk" runat="server" Value="0" />
                <asp:HiddenField ID="hdfbrandcode" runat="server" Value="0" />
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star">
                        <asp:ValidationSummary ID="vsPage" ValidationGroup="WorkOrderItemSave" runat="server" />
                    </asp:Label>
                </div>
                <div id="PmMap" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsgPmMap" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPmMap" ValidationGroup="PMM" runat="server" />
                    <asp:ValidationSummary ID="vsBom" ValidationGroup="WorkOrderItem" runat="server" />

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
