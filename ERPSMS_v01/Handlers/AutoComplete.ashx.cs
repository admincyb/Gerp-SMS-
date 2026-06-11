using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.CommonManagement;
using ERPService.Administration;
using System.Data;
using ERPService.Inventory;
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using BusinessLogic.MaterialManagement;
using BusinessObject;

namespace ERPSMS_v01.Handlers
{
    /// <summary>
    /// Summary description for AutoComplete
    /// </summary>
    public class AutoComplete : IHttpHandler, IRequiresSessionState
    {
        private int ProfessionPk = 0;
        private int ExitReasonPk = 0;
        private int ReligionPk = 0;
        private int SubReligionPk = 0;
        private int DepartmentPk = 0;

        private int branchLocationPk = 0;
        private int countryPk = 0;
        private int materialID = 0;
        private int batchPK = 0;
        private bool IsSBUVendor = false;
        private bool IsSBUCustomer = false;
        private bool IsSBUBank = false;
        private bool IsSBUPO = false;


        /// <summary>
        /// Resusable Flag
        /// don't delete
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        BusinessObject.User currentUser;
        HttpRequest request;
        HttpResponse response;

        /// <summary>
        /// Request Processing Event
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                GetFieldValues(context);
            }
            catch
            {
            }
        }

        #region GetFieldValues

        /// <summary>
        /// get field values
        /// </summary>
        /// <param name="context"></param>
        private void GetFieldValues(HttpContext context)
        {
            string searchKey;
            string SearchBy;
            string searchType;
            string filterType;
            string basisType;
            string autoFilterType;
            string serviceType;
            string voucherType;
            string customerid;
            string VendorPk;
            int accType;
            int CompanyPK;
            int CompoundPK;
            string WtType;
            int Status;
            string excDate;
            string empLocation;
            string relCtrlID = string.Empty;
            string relCtrlValue = string.Empty;
            string queryText = string.Empty;
            string payElmntValue = string.Empty;
            string OpParam = null;
            int? EmpDept = null;
            int? EmpDesignation = null;
            int? PaymentMode = null;
            int? EmpCurrency = null;
            int? ModulePK = null;
            int? UserLogin = null;
            AutoEnum currAutoEnum;
            string queryID = string.Empty;
            byte? ItemGrade = null;
            int? EmpCategory = null;
            int? EmpBranch = null;
            int? EmpType = null;
            int? IssueAgainst = null;
            int? EmploymentType = null;
            int? EmpCompany = null;
            int? BranchByUser = null;
            string BinSubType;
            byte? LinkedProduct = null;
            int? itemPK = null;
            int? ProcessMode = null;
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            int? EmpPayrollType = null;
            int? JobCategory = null;
            int? JobLevel = null;
            int? InvoicePk = null;
            string pageURL = string.Empty;
            int? InvGroup = null;
            int? InvCategory = null;
            int? InvType = null;
            string OperationPK = string.Empty;
            string BrandPK = "0";
            int? EnableWO = null;
            int statusval;
            int sbu = 0;
            int Group = 0;
            int Accpk = 0;
            int BudgetPk = 0;
            try
            {
                searchKey = string.Empty;
                SearchBy = string.Empty;
                searchType = string.Empty;
                filterType = string.Empty;
                basisType = string.Empty;
                autoFilterType = string.Empty;
                serviceType = string.Empty;
                voucherType = string.Empty;
                customerid = string.Empty;
                excDate = string.Empty;
                VendorPk = string.Empty;
                empLocation = string.Empty;
                BinSubType = string.Empty;
                CompanyPK = 0;
                CompoundPK = 0;

                accType = 0;
                WtType = "";
                Status = 0;
                statusval = 0;
                request = context.Request;
                response = context.Response;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                //fetch Request Parameters to variables
                if (request.Params[RequestParameters.StatusVal] != null)
                {
                    statusval = Convert.ToInt32(request.Params[RequestParameters.StatusVal].Trim().ToString());
                }
                if (request.Params[RequestParameters.CompanyPK] != null)
                {
                    CompanyPK = Convert.ToInt32(request.Params[RequestParameters.CompanyPK].Trim().ToString());
                }

                if (request.Params[RequestParameters.SBU] != null)
                {
                    sbu = Convert.ToInt32(request.Params[RequestParameters.SBU].Trim().ToString());
                }

                if (request.Params[RequestParameters.SearchValue] != null)
                {
                    searchKey = HttpUtility.HtmlEncode(request.Params[RequestParameters.SearchValue].Trim().ToString());
                }
                if (request.Params[RequestParameters.SearchBy] != null)
                {
                    SearchBy = HttpUtility.HtmlEncode(request.Params[RequestParameters.SearchBy].Trim().ToString());
                }
                if (request.Params[RequestParameters.SearchType] != null)
                {
                    searchType = request.Params[RequestParameters.SearchType].Trim().ToString();
                }
                if (request.Params[RequestParameters.Type] != null)
                {
                    filterType = request.Params[RequestParameters.Type].Trim().ToString();
                }
                if (request.Params[RequestParameters.BasisType] != null)
                {
                    basisType = request.Params[RequestParameters.BasisType].Trim().ToString();
                }
                if (request.Params[RequestParameters.FilterType] != null)
                {
                    autoFilterType = request.Params[RequestParameters.FilterType].Trim().ToString();
                }
                if (request.Params[RequestParameters.ServiceType] != null)
                {
                    serviceType = request.Params[RequestParameters.ServiceType].Trim().ToString();
                }
                if (request.Params[RequestParameters.VoucherType] != null)
                {
                    voucherType = request.Params[RequestParameters.VoucherType].Trim().ToString();
                }
                if (request.Params[RequestParameters.AccType] != null)
                {
                    if (request.Params[RequestParameters.AccType].Trim().ToString() != string.Empty)
                        accType = Convert.ToInt32(request.Params[RequestParameters.AccType].Trim().ToString());
                }
                if (request.Params[RequestParameters.Type] != null)
                {
                    if (request.Params[RequestParameters.Type].Trim().ToString() != string.Empty)
                        WtType = request.Params[RequestParameters.Type].Trim().ToString();
                }
                if (request.Params[RequestParameters.CustomerID] != null)
                {
                    customerid = request.Params[RequestParameters.CustomerID].Trim().ToString();
                }
                if (request.Params[RequestParameters.Status] != null)
                {
                    if (request.Params[RequestParameters.Status].Trim().ToString() != string.Empty)
                        Status = Convert.ToInt32(request.Params[RequestParameters.Status].Trim().ToString());
                }
                if (request.Params[RequestParameters.ExcDate] != null)
                {
                    excDate = request.Params[RequestParameters.ExcDate].ToString();
                }
                if (request.Params[RequestParameters.VendorPk] != null)
                {
                    VendorPk = request.Params[RequestParameters.VendorPk].ToString();
                }
                if (request.Params[RequestParameters.Location] != null)
                {
                    empLocation = request.Params[RequestParameters.Location].ToString();
                }
                if (request.Params[RequestParameters.Query] != null)
                {
                    queryID = request.Params[RequestParameters.Query].ToString();
                }

                if (request.Params[RequestParameters.RelCtrl] != null)
                {
                    relCtrlID = request.Params[RequestParameters.RelCtrl].ToString();
                }

                if (request.Params[RequestParameters.RelCtrlValue] != null)
                {
                    relCtrlValue = request.Params[RequestParameters.RelCtrlValue].ToString();
                }
                if (request.Params[RequestParameters.PayElmntValue] != null)
                {
                    payElmntValue = request.Params[RequestParameters.PayElmntValue].ToString();
                }
                if (request.Params[RequestParameters.ItemGrade] != null && !string.IsNullOrEmpty(request.Params[RequestParameters.ItemGrade].ToString()))
                {
                    ItemGrade = Convert.ToByte(request.Params[RequestParameters.ItemGrade].ToString());
                }
                if (request.Params[RequestParameters.EmpCategory] != null)
                {
                    if (request.Params[RequestParameters.EmpCategory].Trim().ToString() != string.Empty)
                        EmpCategory = Convert.ToInt32(request.Params[RequestParameters.EmpCategory].Trim().ToString());
                }
                if (request.Params[RequestParameters.EmpBranch] != null)
                {
                    if (request.Params[RequestParameters.EmpBranch].Trim().ToString() != string.Empty)
                        EmpBranch = Convert.ToInt32(request.Params[RequestParameters.EmpBranch].Trim().ToString());
                }
                if (request.Params[RequestParameters.EmpType] != null)
                {
                    if (request.Params[RequestParameters.EmpType].Trim().ToString() != string.Empty)
                        EmpType = Convert.ToInt32(request.Params[RequestParameters.EmpType].Trim().ToString());
                }
                if (request.Params[RequestParameters.EmploymentType] != null)
                {
                    if (request.Params[RequestParameters.EmploymentType].Trim().ToString() != string.Empty)
                        EmploymentType = Convert.ToInt32(request.Params[RequestParameters.EmploymentType].Trim().ToString());
                }
                if (request.Params[RequestParameters.PaymentMode] != null)
                {
                    if (request.Params[RequestParameters.PaymentMode].Trim().ToString() != string.Empty)
                        PaymentMode = Convert.ToInt32(request.Params[RequestParameters.PaymentMode].Trim().ToString());
                }
                if (request.Params[RequestParameters.EmpCompany] != null)
                {
                    if (request.Params[RequestParameters.EmpCompany].Trim().ToString() != string.Empty)
                        EmpCompany = Convert.ToInt32(request.Params[RequestParameters.EmpCompany].Trim().ToString());
                }
                if (request.Params[RequestParameters.OpParam] != null)
                {
                    OpParam = HttpUtility.HtmlEncode(request.Params[RequestParameters.OpParam].Trim().ToString());
                }
                if (request.Params[RequestParameters.EmpDept] != null)
                {
                    if (request.Params[RequestParameters.EmpDept].Trim().ToString() != string.Empty && Convert.ToInt32(request.Params[RequestParameters.EmpDept].Trim().ToString()) > 0)
                        EmpDept = Convert.ToInt32(request.Params[RequestParameters.EmpDept].Trim().ToString());
                }
                if (request.Params[RequestParameters.BinSubType] != null)
                {
                    BinSubType = request.Params[RequestParameters.BinSubType].Trim().ToString();
                }
                if (request.Params[RequestParameters.LinkedProduct] != null && !string.IsNullOrEmpty(request.Params[RequestParameters.LinkedProduct].ToString()))
                {
                    LinkedProduct = Convert.ToByte(request.Params[RequestParameters.LinkedProduct].ToString());
                }
                if (request.Params[RequestParameters.EmpDesignation] != null)
                {
                    if (!string.IsNullOrEmpty(request.Params[RequestParameters.EmpDesignation].Trim().ToString()) && Convert.ToInt32(request.Params[RequestParameters.EmpDesignation].Trim().ToString()) > 0)
                        EmpDesignation = Convert.ToInt32(request.Params[RequestParameters.EmpDesignation].Trim().ToString());
                }
                if (request.Params[RequestParameters.BranchByUser] != null)
                {
                    if (!string.IsNullOrEmpty(request.Params[RequestParameters.BranchByUser].Trim().ToString()) && Convert.ToInt32(request.Params[RequestParameters.BranchByUser].Trim().ToString()) > 0)
                        BranchByUser = Convert.ToInt32(request.Params[RequestParameters.BranchByUser].Trim().ToString());
                }
                if (request.Params[RequestParameters.itemPK] != null)
                {
                    if (request.Params[RequestParameters.itemPK].Trim().ToString() != string.Empty)
                        itemPK = Convert.ToInt32(request.Params[RequestParameters.itemPK].Trim().ToString());
                }
                if (request.Params[RequestParameters.ProcessMode] != null)
                {
                    if (request.Params[RequestParameters.ProcessMode].Trim().ToString() != string.Empty && Convert.ToInt32(request.Params[RequestParameters.ProcessMode].Trim().ToString()) > 0)
                        ProcessMode = Convert.ToInt32(request.Params[RequestParameters.ProcessMode].Trim().ToString());
                }
                if (request.Params[RequestParameters.EmpCurrency] != null)
                {
                    if (request.Params[RequestParameters.EmpCurrency].Trim().ToString() != string.Empty && Convert.ToInt32(request.Params[RequestParameters.EmpCurrency].Trim().ToString()) > 0)
                        EmpCurrency = Convert.ToInt32(request.Params[RequestParameters.EmpCurrency].Trim().ToString());
                }

                if (request.Params[RequestParameters.Country] != null)
                {
                    countryPk = Convert.ToInt32(request.Params[RequestParameters.Country].Trim().ToString());
                }
                if (request.Params[RequestParameters.materialID] != null)
                {
                    materialID = Convert.ToInt32(request.Params[RequestParameters.materialID].Trim().ToString());
                }
                if (request.Params[RequestParameters.IssueAgainst] != null)
                {
                    if (request.Params[RequestParameters.IssueAgainst].Trim().ToString() != string.Empty)
                        IssueAgainst = Convert.ToInt32(request.Params[RequestParameters.IssueAgainst].Trim().ToString());
                }
                if (request.Params[RequestParameters.batchPK] != null)
                {
                    batchPK = Convert.ToInt32(request.Params[RequestParameters.batchPK].Trim().ToString());
                }
                if (request.Params[RequestParameters.FromDate] != null)
                {
                    FromDate = request.Params[RequestParameters.FromDate].ToString();
                }
                if (request.Params[RequestParameters.ToDate] != null)
                {
                    ToDate = request.Params[RequestParameters.ToDate].ToString();
                }
                if (request.Params[RequestParameters.EmpPayrollType] != null)
                {
                    if (request.Params[RequestParameters.EmpPayrollType].Trim().ToString() != string.Empty && Convert.ToInt32(request.Params[RequestParameters.EmpPayrollType].Trim().ToString()) > 0)
                        EmpPayrollType = Convert.ToInt32(request.Params[RequestParameters.EmpPayrollType].Trim().ToString());
                }
                if (request.Params[RequestParameters.JobCategory] != null)
                {
                    JobCategory = Convert.ToInt32(request.Params[RequestParameters.JobCategory].Trim().ToString());
                }
                if (request.Params[RequestParameters.JobLevel] != null)
                {
                    JobLevel = Convert.ToInt32(request.Params[RequestParameters.JobLevel].Trim().ToString());
                }
                if (request.Params[RequestParameters.PAGE_URL] != null)
                {
                    pageURL = request.Params[RequestParameters.PAGE_URL].Trim().ToString();
                }
                if (request.Params[RequestParameters.InvoicePk] != null)
                {
                    if (request.Params[RequestParameters.InvoicePk].Trim().ToString() != string.Empty)
                        InvoicePk = Convert.ToInt32(request.Params[RequestParameters.InvoicePk].Trim().ToString());
                }
                if (request.Params[RequestParameters.ModulePK] != null)
                {
                    if (request.Params[RequestParameters.ModulePK].Trim().ToString() != string.Empty)
                        ModulePK = Convert.ToInt32(request.Params[RequestParameters.ModulePK].Trim().ToString());
                }
                if (request.Params[RequestParameters.UserLogin] != null)
                {
                    if (request.Params[RequestParameters.UserLogin].Trim().ToString() != string.Empty)
                        UserLogin = Convert.ToInt32(request.Params[RequestParameters.UserLogin].Trim().ToString());
                }
                if (request.Params[RequestParameters.InvGroup] != null)
                {
                    if (request.Params[RequestParameters.InvGroup].Trim().ToString() != string.Empty)
                        InvGroup = Convert.ToInt32(request.Params[RequestParameters.InvGroup].Trim().ToString());
                }
                if (request.Params[RequestParameters.InvCategory] != null)
                {
                    if (request.Params[RequestParameters.InvCategory].Trim().ToString() != string.Empty)
                        InvCategory = Convert.ToInt32(request.Params[RequestParameters.InvCategory].Trim().ToString());
                }

                if (request.Params[RequestParameters.EnableWO] != null)
                {
                    if (request.Params[RequestParameters.EnableWO].Trim().ToString() != string.Empty)
                        EnableWO = Convert.ToInt32(request.Params[RequestParameters.EnableWO].Trim().ToString());
                }

                if (request.Params[RequestParameters.InvType] != null)
                {
                    if (request.Params[RequestParameters.InvType].Trim().ToString() != string.Empty)
                        InvType = Convert.ToInt32(request.Params[RequestParameters.InvType].Trim().ToString());
                }
                if (request.Params[RequestParameters.IsSBUVendor] != null)
                {
                    if (request.Params[RequestParameters.IsSBUVendor].Trim().ToString() != string.Empty)
                        IsSBUVendor = Convert.ToBoolean(request.Params[RequestParameters.IsSBUVendor].Trim().ToString());
                }
                if (request.Params[RequestParameters.IsSBUCustomer] != null)
                {
                    if (request.Params[RequestParameters.IsSBUCustomer].Trim().ToString() != string.Empty)
                        IsSBUCustomer = Convert.ToBoolean(request.Params[RequestParameters.IsSBUCustomer].Trim().ToString());
                }
                if (request.Params[RequestParameters.IsSBUBank] != null)
                {
                    if (request.Params[RequestParameters.IsSBUBank].Trim().ToString() != string.Empty)
                        IsSBUBank = Convert.ToBoolean(request.Params[RequestParameters.IsSBUBank].Trim().ToString());
                }
                if (request.Params[RequestParameters.OperationPK] != null)
                {
                    if (request.Params[RequestParameters.OperationPK].Trim().ToString() != string.Empty)
                        OperationPK = request.Params[RequestParameters.OperationPK].Trim().ToString();
                }
                if (request.Params[RequestParameters.BrandID] != null)
                {
                    if (request.Params[RequestParameters.BrandID].Trim().ToString() != string.Empty)
                        BrandPK = request.Params[RequestParameters.BrandID].Trim().ToString();
                }
                if (request.Params[RequestParameters.Group] != null)
                {
                    if (request.Params[RequestParameters.Group].Trim().ToString() != string.Empty)
                        Group = Convert.ToInt32(request.Params[RequestParameters.Group].Trim().ToString());
                }
                if (request.Params[RequestParameters.AccountPk] != null)
                {
                    if (request.Params[RequestParameters.AccountPk].Trim().ToString() != string.Empty)
                        Accpk = Convert.ToInt32(request.Params[RequestParameters.AccountPk].Trim().ToString());
                }
                if (request.Params[RequestParameters.BudgetPk] != null)
                {
                    if (request.Params[RequestParameters.BudgetPk].Trim().ToString() != string.Empty)
                        BudgetPk = Convert.ToInt32(request.Params[RequestParameters.BudgetPk].Trim().ToString());
                }
                //switch the selected autocomplete method
                currAutoEnum = (AutoEnum)Enum.Parse(typeof(AutoEnum), searchType.ToUpper());
                switch (currAutoEnum)
                {
                    case AutoEnum.VENDOR:
                        GetVendor(searchKey);
                        break;
                    case AutoEnum.VENDORNAMECODE:
                        GetVendorNameCode(searchKey);
                        break;
                    case AutoEnum.COSTCENTER:
                        GetCostCenter(searchKey, Group);
                        break;
                    case AutoEnum.BUDGETCOSTCENTER:
                        GetBudgetCostCenter(searchKey, Group,BudgetPk);
                        break;
                    case AutoEnum.COSTCENTERWITHOUTGRP:
                        GetCostCenterWithoutGrp(searchKey);
                        break;
                    case AutoEnum.TRANSPORTER:
                        GetVendor(searchKey, new List<short>() {
                            (short)VendorRoles.Transporter
                        });
                        break;
                    case AutoEnum.PARTY:
                        GetVendor(searchKey, new List<short>() {
                            (short)VendorRoles.Transporter,
                            (short)VendorRoles.ServiceProvider,
                            (short)VendorRoles.ShippinngCompany,
                            (short)VendorRoles.BoxManufacture
                        });
                        break;
                    case AutoEnum.VENDOR_ROLE:
                        GetVendorRole(searchKey);
                        break;
                    case AutoEnum.CURRENCY:
                        GetCurrency(searchKey, null);
                        break;
                    case AutoEnum.CURRENCYCODE:
                        GetCurrency(searchKey, "Code");
                        break;
                    case AutoEnum.VENDORCURRENCY:
                        GetVendorCurrency(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), excDate);
                        break;
                    case AutoEnum.CUSTOMER:
                        GetCustomer(searchKey);
                        break;
                    case AutoEnum.CUSTOMERLIST:
                        GetCustomerList(searchKey);
                        break;
                    case AutoEnum.CUSTOMERVENDORLIST:
                        GetCustomerVendorList(searchKey);
                        break;
                    case AutoEnum.VENDORACCOUNT:
                        GetVendorAccount(searchKey);
                        break;
                    case AutoEnum.PONUMBER:
                        GetPoNumbers(searchKey);
                        break;

                    case AutoEnum.POWONUMBER:
                        GetPoWoNumbers(searchKey, EnableWO);
                        break;

                    case AutoEnum.CUSPONUMBER:
                        GetCUSPONumbers(searchKey);
                        break;
                    case AutoEnum.CUSPONUMBERCUSTOMERWISE:
                        GetCUSPONumbersCustomerWise(searchKey, customerid, 2);
                        break;

                    case AutoEnum.SONUMBER:
                        GetSONumbers(searchKey, customerid);
                        break;
                    case AutoEnum.SONUMBERWKF:
                        GetSONumbers(searchKey, customerid, user: currentUser.PKUser);
                        break;
                    case AutoEnum.IONUMBER:
                        GetSONumbers(searchKey, customerid, isIO: true);
                        break;
                    case AutoEnum.SONUMBERAPPROVED:
                        GetSONumbers(searchKey, customerid, 2);
                        break;
                    case AutoEnum.PURCHASEPAYMENTNUMBER:
                        GetPurchasePaymentNumbers(searchKey);
                        break;
                    case AutoEnum.SALESRECEIPTNUMBER:
                        GetSalesReceiptNumbers(searchKey);
                        break;
                    case AutoEnum.INVOICENUMBER:
                        GetInvoiceNumbers(searchKey);
                        break;
                    case AutoEnum.ADVINVNUMBER:
                        GetInvoiceNumbers(searchKey, 0, (byte)POInvoiceCategory.Advanced);
                        break;
                    case AutoEnum.INVNUMBER:
                        GetInvoiceNumbers(searchKey, 0, (byte)POInvoiceCategory.Invoice);
                        break;
                    case AutoEnum.EXPENSEINVNUMBER:
                        GetInvNumbers(searchKey, (byte)POInvoiceGroup.Expense, (byte)POInvoiceCategory.Invoice);
                        break;
                    case AutoEnum.EXPENSESETTILEMENTINVNUMBER:
                        GetInvNumbers(searchKey, (byte)POInvoiceGroup.ExpenseSettilement, (byte)POInvoiceCategory.Invoice);
                        break;
                    case AutoEnum.BANK:
                        GetBank(searchKey, filterType);
                        break;
                    case AutoEnum.ADVSALINVOICENUMBER:
                        GetAdvanceSalesInvoiceNumbers(searchKey);
                        break;
                    case AutoEnum.SALINVOICENUMBER:
                        GetSalesInvoiceNumbers(searchKey, filterType);
                        break;
                    case AutoEnum.FCVOUCHERNUMBER:
                        GetFCVoucherNumbers(searchKey);
                        break;
                    case AutoEnum.VATSALEEXPORTCUSTOMER:
                        GetVatSaleExportField(GTIService.Constants.Finance.Parameters.VSECUSTOMERNAME, searchKey);
                        break;
                    case AutoEnum.VATSALEEXPORTINVOICE:
                        GetVatSaleExportField(GTIService.Constants.Finance.Parameters.VSEINVOICENO, searchKey);
                        break;
                    case AutoEnum.DELIVERYORDERNUMBER:
                        GetDeliveryOrderNumbers(searchKey, Status);
                        break;
                    case AutoEnum.DONUMBER:
                        GetDONumbers(searchKey, customerid, InvoicePk);
                        break;
                    case AutoEnum.ACCOUNT:
                        GetAccounts(searchKey, voucherType, accType, CompanyPK);
                        break;
                    case AutoEnum.ACCOUNTMST:
                        GetAccountsMst(searchKey, accType);
                        break;
                    case AutoEnum.ITEMCATEGORY:
                        GetItemCategory(searchKey, filterType);
                        break;
                    case AutoEnum.MATERIALISSUENO:
                        GetMaterialIssueNo(searchKey);
                        break;
                    case AutoEnum.ITEMCODE:
                        GetItemCode(searchKey, filterType);
                        break;
                    case AutoEnum.BATCHNO:
                        GetBatchNo(searchKey, materialID, excDate, batchPK);
                        break;
                    case AutoEnum.ITEMNAME:
                        GetItemName(searchKey, filterType, IssueAgainst);
                        break;
                    case AutoEnum.CRDRNUMBER:
                        GetCrDrNumbers(searchKey);
                        break;
                    case AutoEnum.CRDRNUMBERSAL:
                        GetSalCrDrNumbers(searchKey);
                        break;
                    case AutoEnum.CRDRNUMBERPUR:
                        GetPurCrDrNumbers(searchKey);
                        break;
                    case AutoEnum.SUBACCOUNTS:
                        GetSubAccounts(searchKey, accType);
                        break;
                    case AutoEnum.REPORTGROUP:
                        GetReportGroup(searchKey);
                        break;
                    case AutoEnum.REPORT:
                        //GetReport(searchKey, filterType);
                        GetReportNew(searchKey, filterType);
                        break;
                    case AutoEnum.PRODUCTNATURE:
                        GetProductNature(searchKey, filterType);
                        break;
                    case AutoEnum.PRODUCTMASTER:
                        GetProductMaster(searchKey, filterType);
                        break;
                    case AutoEnum.COUNTRY:
                        GetCountry(searchKey);
                        break;
                    case AutoEnum.MESSAGECOUNT:
                        GetMessageCount();
                        break;
                    case AutoEnum.JOURNALACCOUNT:
                        GetJournalAccounts(searchKey, accType);
                        break;
                    case AutoEnum.UOM:
                        GetUOM(searchKey, WtType);
                        break;
                    case AutoEnum.WHTACCOUNTS:
                        GetWHTAccounts(searchKey);
                        break;
                    case AutoEnum.VATBUYACCOUNTS:
                        GetVatBuyAccounts(searchKey);
                        break;
                    case AutoEnum.VENDORCONTACTTYPES:
                        GetAddressTypeList(searchKey);
                        break;
                    case AutoEnum.VENDORCONTACTS:
                        GetVendorContacts(searchKey, string.IsNullOrEmpty(VendorPk) ? -1 : Convert.ToInt32(VendorPk));
                        break;
                    case AutoEnum.NATIONALITY:
                        GetAutoNationality(searchKey, OpParam);
                        break;
                    case AutoEnum.COUNTRYBIRTH:
                        GetAutoCountry(searchKey);
                        break;
                    case AutoEnum.COUNTRYFIRST:
                        GetAutoCountry(searchKey);
                        break;
                    case AutoEnum.COUNTRYSECOND:
                        GetAutoCountry(searchKey);
                        break;
                    case AutoEnum.STATEAUTO:
                        GetAutoState(searchKey, countryPk);
                        break;
                    case AutoEnum.PROFESSION:
                        GetAutoProfession(ProfessionPk, searchKey);
                        break;
                    case AutoEnum.EXITREASON:
                        GetExitReason(ExitReasonPk, searchKey);
                        break;
                    case AutoEnum.RELIGION:
                        GetAutoReligion(ReligionPk, searchKey);
                        break;
                    case AutoEnum.TEAM:
                        GetAutoTeam(searchKey);
                        break;
                    case AutoEnum.SUBRELIGION:
                        GetAutoSubReligion(SubReligionPk, searchKey);
                        break;
                    case AutoEnum.DEPARTMENT:
                        GetAutoDepartment(DepartmentPk, searchKey);
                        break;
                    case AutoEnum.DEPARTMENTAUTOCOMPLETE:
                        GetDepartmentAutocomplete(DepartmentPk, searchKey);
                        break;
                    case AutoEnum.BRANCHLOCATION:
                        GetAutoBranchLocation(branchLocationPk, searchKey, BranchByUser);
                        break;
                    case AutoEnum.TRANSACTIONNO:
                        GetTransactionNo(searchKey);
                        break;
                    case AutoEnum.EMPLOYEEREPORTTO:
                        GetEmployeeReportTo(searchKey);
                        break;
                    case AutoEnum.ASSIGNTASKUSER:
                        GetAutoTaskUsers(searchKey);
                        break;
                    case AutoEnum.EMPLOYEEAUTOCOMPLETE:
                        GetEmployeeAutoComplete(searchKey, EmpCategory, EmpBranch, EmpType, EmploymentType, EmpCompany, EmpDept, EmpDesignation, PaymentMode, EmpCurrency, ToDate);
                        break;
                    case AutoEnum.EMPLOYEEAUTOCOMPLETEBYFILTER:
                        GetEmployeeAutoComplete(searchKey, filterType, EmpCategory, EmpDept, EmpType, ProcessMode, ToDate);
                        break;
                    case AutoEnum.EMPLOYEEPAYROLLAUTOCOMPLETE:
                        GetEmployeePayrollAutoComplete(FromDate, ToDate, searchKey, EmpCategory, EmpBranch, EmpType, EmploymentType, EmpCompany, EmpDept, EmpDesignation, PaymentMode, EmpCurrency, EmpPayrollType);
                        break;
                    case AutoEnum.GETAUTOAGT:
                        GetAutoCustomerForAgentComm(searchKey, SearchBy);
                        break;
                    case AutoEnum.GETAUTOCSI:
                        GetAutoCusScInvForAgentComm(searchKey, SearchBy);
                        break;
                    case AutoEnum.GETAUTOAGTCOMM:
                        GetAutoForAgentComm(searchKey, SearchBy);
                        break;
                    case AutoEnum.DESIGNATION:
                        GetDesignation(0, searchKey);
                        break;
                    case AutoEnum.DESIGNATIONBYJOB:
                        GetDesignationByJob(searchKey, JobCategory, JobLevel);
                        break;
                    case AutoEnum.GETALLITEMS:
                        GetAllItem(searchKey, filterType);
                        break;
                    case AutoEnum.GETALLITEMSWITHCODENAME:
                        GetAllItemWithCodeName(searchKey, filterType);
                        break;
                    case AutoEnum.DEPRECIATIONAUTOGET:
                        GetDepreciationNo(searchKey);
                        break;
                    case AutoEnum.ASSETDISPOSALAUTO:
                        GetAssetDisposalNoAuto(searchKey);
                        break;
                    case AutoEnum.EMPLOYEEAUTO:
                        GetEmployeeAuto(searchKey, empLocation, filterType);
                        break;
                    case AutoEnum.EXECUTEQUERY:
                        GetDynamicAutoCompleteData(queryID, searchKey, relCtrlID, relCtrlValue);
                        break;
                    case AutoEnum.PAYELEMENT:
                        GetAutoPayElement(searchKey, payElmntValue);
                        break;
                    case AutoEnum.GETPRODUCTS:
                        GetProducts(searchKey, ItemGrade, BinSubType, itemPK, filterType);
                        break;
                    case AutoEnum.GETRELATEDPRODUCTS:
                        GetRelatedProducts(searchKey, ItemGrade, BinSubType, itemPK);
                        break;
                    case AutoEnum.GETBRANDPRODUCTS:
                        GetAutoBrandProducts(SearchBy, searchKey, Status.ToString());
                        break;
                    case AutoEnum.GETGLOVEITEMS:
                        GetAllGloveItems();
                        break;
                    case AutoEnum.ADDDEDNUMBER:
                        GetAdditionDeductionNumbers(searchKey);
                        break;
                    case AutoEnum.SALPYMTNUMBER:
                        GetSalaryPaymenetNumbers(searchKey);
                        break;
                    case AutoEnum.EOTNUMBER:
                        GetOvertimeDetailsNumbers(searchKey);
                        break;
                    case AutoEnum.EOLNUMBER:
                        GetOtherLeaveEntryNumbers(searchKey);
                        break;
                    case AutoEnum.EDOCEMPLOYEES:
                        GetAutoEdocEmployee(searchKey);
                        break;
                    case AutoEnum.GETASSETTYPEAUTO:
                        GetAssetTypesAuto(searchKey);
                        break;
                    case AutoEnum.GETPRODUCTGROUPS:
                        GetProductGroups(searchKey);
                        break;
                    case AutoEnum.GETASSETSAUTO:
                        GetAssetAuto(searchKey, filterType);
                        break;
                    case AutoEnum.GETASSETITEMSAUTO:
                        GETASSETITEMSAUTO(searchKey, filterType);
                        break;
                    case AutoEnum.GETASSETSERVICEREQUESTNOAUTO:
                        GETASSETSERVICEREQUESTNOAUTO(searchKey, filterType);
                        break;
                    case AutoEnum.GETASSETSERVICEORDERNOAUTO:
                        GETASSETSERVICEORDERNOAUTO(searchKey, filterType);
                        break;
                    case AutoEnum.GETASSETSERVICERECEIPTNOAUTO:
                        GETASSETSERVICERECEIPTNOAUTO(searchKey, filterType);
                        break;
                    case AutoEnum.EMPTRANSFERNUMBER:
                        GetEmpTransferNumbers(searchKey);
                        break;
                    case AutoEnum.GETDIRECTDONOAUTO:
                        GETDIRECTDONOAUTO(searchKey, filterType, pageURL);
                        break;
                    case AutoEnum.GETDIRECTSOPENDINGAUTO:
                        GETDIRECTSOPENDINGAUTO(searchKey, filterType, customerid);
                        break;
                    case AutoEnum.GETUSERSAUTO:
                        GETUSERSAUTO(searchKey, filterType, EmpType);
                        break;
                    case AutoEnum.PLANNINGGROUP:
                        GetPlanningGroupAutocomplete(searchKey, SearchBy);
                        break;
                    case AutoEnum.SCNUMBER:
                        GetSONumbers(searchKey, customerid, InvoicePk);
                        break;
                    case AutoEnum.FILLPROCESS:
                        GetFillProcess(WtType, searchKey, sbu);
                        break;
                    case AutoEnum.FILLDEPT:
                        GetFillDept(WtType, searchKey);
                        break;
                    #region ESS
                    case AutoEnum.ESSEMPLOYEEAUTOCOMPLETE:
                        GetAutoCompleteEmployeeESS(searchKey, ToDate);
                        break;
                    #endregion
                    #region GETDIRECTINVOICENOAUTO
                    case AutoEnum.GETDIRECTINVOICENOAUTO:
                        GETDIRECTINVOICENOAUTO(searchKey, InvCategory, InvGroup);
                        break;
                    #endregion
                    case AutoEnum.PURCHASEORDERNO:
                        GetPurchaseOrderNumbers(searchKey, customerid, InvoicePk);
                        break;
                    case AutoEnum.GETUSERSINBOXMAPPING:
                    case AutoEnum.GETROLESINBOXMAPPING:
                        GetAutoUserRoleInboxMapping(searchKey, filterType, ModulePK, UserLogin, EmpDept);
                        break;
                    case AutoEnum.GETCOAPARENT:
                        GetCoaParent(searchKey, SearchBy, Status,Accpk);
                        break;
                    case AutoEnum.GETBUDGETCOAPARENT:
                        GetBudgetCoaParent(searchKey, SearchBy, Status, Accpk,BudgetPk);
                        break;
                    case AutoEnum.GETMAPPEDITEMVENDOR:
                        GetMappedItemvendor(searchKey, SearchBy, filterType, VendorPk);
                        break;
                    case AutoEnum.SALINVNUMBERTRADING:
                        GetTradingInvoiceNumbers(searchKey, InvCategory, InvGroup, customerid, InvType);
                        break;
                    case AutoEnum.PENDINGPURCHASEINVNO:
                        GetPendingPurchaseInvNumbers(searchKey, customerid, InvCategory, InvType);
                        break;
                    case AutoEnum.GETDRCRNOTRADINGAUTO:
                        GETDRCRNOTRADINGAUTO(searchKey, filterType);
                        break;
                    case AutoEnum.GETPAYMENTNOTRADINGAUTO:
                        GETPAYMENTNOTRADINGAUTO(searchKey, filterType);
                        break;
                    case AutoEnum.PENDINGPURCHASEINVNOTRADING:
                        GetPendingInvoiceNoTrading(searchKey, InvCategory, InvGroup, InvType, customerid);
                        break;
                    case AutoEnum.GETFUNDREQUISITIONNOAUTO:
                        GETFUNDREQUISITIONNOAUTO(searchKey, filterType);
                        break;

                    case AutoEnum.GETAGENTLIST:
                        GETAGENTLIST(searchKey, filterType);
                        break;
                    case AutoEnum.INVCONVERTED:
                        GetConvertedInvNumbers(searchKey);
                        break;
                    case AutoEnum.WONUMBER:
                        GetWorkOrderNumbers(searchKey);
                        break;
                    case AutoEnum.WOITEM:
                        customerid = customerid == "undefined" ? "0" : customerid;
                        BrandPK = BrandPK == "undefined" ? "0" : BrandPK;
                        searchKey = "%" + searchKey + "%";
                        GetWorkOrderItemsAuto(Convert.ToInt32(OperationPK), Convert.ToInt32(filterType), Convert.ToInt32(customerid), searchKey, Convert.ToInt32(BrandPK));
                        break;
                    case AutoEnum.GETORDERPLANTEXT:
                        GetOrderPlanText(SearchBy, searchKey, statusval);
                        break;
                    case AutoEnum.WORKORDERITEM:
                        GetWorkOrderItemsForFilterAuto(searchKey);
                        break;
                    case AutoEnum.FINYEAROPENINGNO:
                        GetFinYearOpeningNumbers(searchKey);
                        break;
                    case AutoEnum.MIRATEADJUSTNO:
                        GetMIRateAdjustNumbers(searchKey);
                        break;
                    case AutoEnum.CWIPNO:
                        GetCWIPNumbers(searchKey);
                        break;
                    case AutoEnum.CWIPACCOUNT:
                        GetCWIPAccount(Group, Convert.ToInt32(itemPK), searchKey);
                        break;
                    case AutoEnum.QUOTATION:
                        GetQuotation(searchKey, customerid, sbu);
                        break;
                    case AutoEnum.COMPMSTFILTER:
                        GetCompound(searchKey, statusval, CompanyPK);
                        break;
                    case AutoEnum.DISPMSTFILTER:
                        GetDispersion(searchKey, statusval, CompanyPK,filterType);
                        break;
                    case AutoEnum.GETBUDGETNO:
                        GetBudgetNo(searchKey);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to get Plan name or code
        /// </summary>
        /// <returns>Item Key-Value rows</returns> 
        private void GetOrderPlanText(string searchBy, string searchValue, int active)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                User curUser = (User)HttpContext.Current.User.Identity;
                serializer = new JavaScriptSerializer();
                // get auto complete list and return to javascript
                List<AutoCompleteBO> lstValue = BusinessLogic.OrderPlanning.OrderPlanningBL.GetPlanNameAuto(searchBy, searchValue, curUser.CurrentSBUPK, active);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(lstValue, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Method to get Trading Invoice Numbers
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="InvCategory"></param>
        /// <param name="InvGroup"></param>
        private void GetTradingInvoiceNumbers(string searchKey, int? InvCategory, int? InvGroup, string CustomerPk, int? IvnType = null)
        {
            JavaScriptSerializer serializer;
            DataTable dtResult;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                int custPk = 0;
                int.TryParse(CustomerPk, out custPk);
                dtResult = BusinessLogic.Sales.SalesInvoiceBL.GetTradingInvoiceNumbers(currentUser.SBUID, searchValue, InvCategory, InvGroup, custPk, IvnType);
                result = dtResult.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get DO numbers for Autocomplete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="customerid"></param>
        private void GetSONumbers(string searchKey, string customerId, int? InvoicePk)
        {
            JavaScriptSerializer serializer;
            DataTable dtResult;
            List<AutoCompleteBO> result;
            try
            {
                int custPK = 0;
                int.TryParse(customerId, out custPK);
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                //dtAddDed = BusinessLogic.Sales.SalesInvoiceBL.GetSONumbers(currentUser.SBUID, searchValue, customerId, InvoicePk);               
                dtResult = BusinessLogic.Sales.SalesInvoiceBL.GetPendingSalesOrderList(custPK, (int?)null, InvoicePk, searchValue, currentUser.SBUID, 0, 0);
                result = dtResult.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("SOH_PK"),
                    Name = row.Field<string>("SOH_NO")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }


        /// <summary>
        /// Get DO numbers for Autocomplete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="customerid"></param>
        private void GetDONumbers(string searchKey, string customerId, int? InvoicePk)
        {
            JavaScriptSerializer serializer;
            DataTable dtAddDed;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAddDed = BusinessLogic.Sales.SalesInvoiceBL.GetDONumbers(currentUser.SBUID, searchValue, customerId, InvoicePk);
                result = dtAddDed.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        private void GetAdditionDeductionNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtAddDed;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAddDed = BusinessLogic.HRMS.Payroll.OtherAdditionDeductionBL.GetAdditionDeductionNumbers((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtAddDed.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("OAH_PK"),

                    Name = row.Field<string>("OAH_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetDynamicAutoCompleteData(string queryID, string searchKey, string relId, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) value = "NULL";
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                CommonService commonService = null;
                commonService = new CommonService();
                commonService = CommonFunctions.InitiateClient(commonService);
                string query = string.Empty;
                #region Generate qry from qryID
                query = string.Format("SELECT QRY_QUERY FROM ADM_QUERIES_CFG WHERE QRY_PK = {0}", queryID);
                query = commonService.ExecuteTextQuery(query)[0];
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "'%" + searchKey + "%'" : "'%%'";

                query = query.Replace("@P_VALUE", searchValue);
                //Replace query with the values if any condition is there
                List<string> conditionList = new List<string>();
                string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                if (splitWithAt.Count() > 1)
                {
                    for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                    {
                        conditionList.Add(splitWithAt[arrayCount].Trim());
                    }

                    foreach (string condition in conditionList)
                    {
                        if (HttpContext.Current.Session[condition] != null)//parameter name is same as any session name
                        {
                            if (!string.IsNullOrEmpty(HttpContext.Current.Session[condition].ToString()))
                                query = query.Replace("@" + condition + "@", HttpContext.Current.Session[condition].ToString());
                            else
                            {
                                query = string.Empty;
                                break;
                            }
                        }
                        else if (condition == relId)// parameter is value of any other control
                        {
                            if (!string.IsNullOrEmpty(value))
                                query = query.Replace("@" + condition + "@", value);
                            else
                            {
                                query = string.Empty;
                                break;
                            }
                        }
                        else if (condition == "BIZUNITPK")
                        {
                            query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                        }
                    }
                }
                #endregion
                #region Auto Complete Code
                serializer = new JavaScriptSerializer();
                List<DDLMaster> dataList = commonService.ExecuteQuery(query); //BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoNationality(searchValue);
                result = dataList.Select(row => new AutoCompleteBO()
                {
                    Key = row.PK ?? 0,
                    Name = row.Value
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }


        /// <summary>
        /// Get PO numbers for Autocomplete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="customerid"></param>
        private void GetPurchaseOrderNumbers(string searchKey, string vendorPk, int? InvoicePk)
        {
            JavaScriptSerializer serializer;
            DataTable dtAddDed;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                int venPk = 0;
                venPk = !string.IsNullOrEmpty(vendorPk) ? Convert.ToInt32(vendorPk) : 0;
                dtAddDed = BusinessLogic.POInvoicing.POInvoiceBL.GetPurchaseOrderNumbers(currentUser.SBUID, searchValue, venPk, InvoicePk);
                result = dtAddDed.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get PO numbers for Autocomplete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="customerid"></param>
        private void GetPendingPurchaseInvNumbers(string searchKey, string vendorPk, int? invCategory, int? invType)
        {
            JavaScriptSerializer serializer;
            DataTable dtAddDed;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAddDed = BusinessLogic.POInvoicing.DebitCreditTradingBL.GetPendingPurchaseInvNumbers(currentUser.SBUID, searchValue, vendorPk, invCategory, invType);
                result = dtAddDed.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<Int64>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Get User autocomplete for Inbox Mapping
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="customerid"></param>
        private void GetAutoUserRoleInboxMapping(string searchKey, string type, int? modulePK, int? userLogin, int? EmpDept)
        {
            JavaScriptSerializer serializer;
            DataTable dtAddDed;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAddDed = CommonBL.GetAutoUserRoleInboxMapping(searchValue, type, modulePK, userLogin, EmpDept, currentUser.SBUID);
                result = dtAddDed.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get COA Parent in AutoComplate
        /// </summary>
        /// <param name="searchKey"></param> 
        private void GetCoaParent(string searchKey, string searchField, int isGroup,int AccountPk)
        {
            JavaScriptSerializer serializer;
            DataTable dtData;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtData = BusinessLogic.CommonManagement.CommonBL.GetCoaParent(searchValue, searchField, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID, isGroup, AccountPk);
                result = dtData.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetBudgetCoaParent(string searchKey, string searchField, int isGroup, int AccountPk,int Budget)
        {
            JavaScriptSerializer serializer;
            DataTable dtData;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtData = BusinessLogic.CommonManagement.CommonBL.GetBudgetCoaParent(searchValue, searchField, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID, isGroup, AccountPk,Budget);
                result = dtData.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get Direct(Trading) invoice number auto
        /// </summary>
        /// <param name="searchKey"></param>
        private void GETDRCRNOTRADINGAUTO(string searchKey, string type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> autoList;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int purSales = 0;
                purSales = !string.IsNullOrEmpty(type) ? Convert.ToInt16(type) : 0;
                autoList = BusinessLogic.POInvoicing.DebitCreditTradingBL.GetDrCrNoteNoTradingAutoComplete(searchValue, objUser, purSales);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(autoList, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Get Direct(Trading) invoice number auto
        /// </summary>
        /// <param name="searchKey"></param>
        private void GETPAYMENTNOTRADINGAUTO(string searchKey, string type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> autoList;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                autoList = BusinessLogic.POInvoicing.POPaymentTradingBL.GetPaymentNoTradingAutoComplete(searchValue, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(autoList, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Method to get Trading Invoice Numbers
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="InvCategory"></param>
        /// <param name="InvGroup"></param>
        private void GetPendingInvoiceNoTrading(string searchKey, int? invCategory, int? invGroup, int? invType, string vendorPK)
        {
            JavaScriptSerializer serializer;
            DataTable dtResult;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                int venPk = 0;
                int.TryParse(vendorPK, out venPk);
                dtResult = BusinessLogic.POInvoicing.POInvoiceBL.GetPendingInvoiceNoTrading(currentUser.SBUID, searchValue, invCategory, invGroup, invType, venPk);
                result = dtResult.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// For auto Search Country
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetCountry(string searchKey)
        {

            CountryService countryServiceClient;
            countryServiceClient = null;
            AdmCountryMst admCountryMstObj;
            List<AdmCountryMst> admCountryMstList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            countryServiceClient = null;
            try
            {

                countryServiceClient = new CountryService();
                countryServiceClient = CommonFunctions.InitiateClient(countryServiceClient);
                admCountryMstObj = countryServiceClient.GetInitilizedAdmCountryMst();
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.ErpRes.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CntName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                admCountryMstList = countryServiceClient.GetAdmCountryMstAutoCompleteList(admCountryMstObj, serviceUtilityObj);
                //countryServiceClient.Close();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(admCountryMstList, Resources.DataFieldRes.CntName, Resources.DataFieldRes.CntPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                //countryServiceClient.Abort();
                throw ex;
            }
            finally
            {
                serializer = null;
                countryServiceClient = null;
                admCountryMstObj = null;
                serviceUtilityObj = null;
                admCountryMstList = null;
            }
        }

        /// <summary>
        /// Ge Po numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetPoNumbers(string searchKey)
        {
            POListService poListServiceClient;
            poListServiceClient = null;
            PUR_ORDER_HDR objPoHeader;
            List<PUR_ORDER_HDR> poHeaderList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                poListServiceClient = new POListService();
                poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                objPoHeader = CommonFunctions.Initilize<PUR_ORDER_HDR>();
                objPoHeader.POH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                objPoHeader.POH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.IsSBUSpecific = IsSBUPO;
                poHeaderList = poListServiceClient.GetPoNumberAutoCompleteList(objPoHeader, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(poHeaderList, Resources.DataFieldRes.PONumber, Resources.DataFieldRes.PurchaseOrderPk);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                poListServiceClient = null;
                objPoHeader = null;
                serviceUtilityObj = null;
                poHeaderList = null;
            }
        }


        /// <summary>
        /// Ge Po numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetSONumbers(string searchKey, string customerid, byte? wkfStatus = null, bool isIO = false, int user = 0)
        {
            SaleOrderService SaleOrderServiceClient;
            SaleOrderServiceClient = null;
            SAL_ORDER_HDR objSoHeader;
            List<SAL_ORDER_HDR> soHeaderList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                SaleOrderServiceClient = new SaleOrderService();
                SaleOrderServiceClient = CommonFunctions.InitiateClient(SaleOrderServiceClient);
                objSoHeader = CommonFunctions.Initilize<SAL_ORDER_HDR>();
                serviceUtilityObj = new ServiceUtility();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                if (customerid != string.Empty)
                {
                    objSoHeader.SOH_CUSTOMER = Convert.ToInt16(customerid);
                }
                objSoHeader.SOH_ACTIVE = 1;
                objSoHeader.SOH_BIZUNIT = currentUser.SBUID;
                if (wkfStatus.HasValue)
                {
                    objSoHeader.SOH_STATUS = wkfStatus.Value;
                    serviceUtilityObj.NeedAdvanceFilter = true;
                }
                if (isIO)
                    objSoHeader.SOH_TRX_STATUS = (byte)SaleOrderStatus.InternalOrder;
                if (user > 0)
                    objSoHeader.SOH_CRTD_BY = user;
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CustomerName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                soHeaderList = SaleOrderServiceClient.GetSoNumberAutoCompleteList(objSoHeader, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(soHeaderList, Resources.DataFieldRes.SONo, Resources.DataFieldRes.SalesOrderPk);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                SaleOrderServiceClient = null;
                objSoHeader = null;
                serviceUtilityObj = null;
                soHeaderList = null;
            }
        }

        private void GetCUSPONumbersCustomerWise(string searchKey, string customerid, byte? wkfStatus = null, bool isIO = false, int user = 0)
        {
            SaleOrderService SaleOrderServiceClient;
            SaleOrderServiceClient = null;
            SAL_ORDER_HDR objSoHeader;
            List<SAL_ORDER_HDR> soHeaderList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                SaleOrderServiceClient = new SaleOrderService();
                SaleOrderServiceClient = CommonFunctions.InitiateClient(SaleOrderServiceClient);
                objSoHeader = CommonFunctions.Initilize<SAL_ORDER_HDR>();
                serviceUtilityObj = new ServiceUtility();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                if (customerid != string.Empty)
                {
                    objSoHeader.SOH_CUSTOMER = Convert.ToInt16(customerid);
                }
                objSoHeader.SOH_ACTIVE = 1;
                objSoHeader.SOH_BIZUNIT = currentUser.SBUID;
                if (wkfStatus.HasValue)
                {
                    objSoHeader.SOH_STATUS = wkfStatus.Value;
                    serviceUtilityObj.NeedAdvanceFilter = true;
                }
                if (isIO)
                    objSoHeader.SOH_TRX_STATUS = (byte)SaleOrderStatus.InternalOrder;
                if (user > 0)
                    objSoHeader.SOH_CRTD_BY = user;
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CustomerName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                soHeaderList = SaleOrderServiceClient.GetCusPoNumberAutoCompleteList(objSoHeader, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(soHeaderList, Resources.DataFieldRes.SOH_Reference, Resources.DataFieldRes.SalesOrderPk);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                SaleOrderServiceClient = null;
                objSoHeader = null;
                serviceUtilityObj = null;
                soHeaderList = null;
            }
        }


        /// <summary>
        /// Ge Customer Po numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetCUSPONumbers(string searchKey, byte? wkfStatus = null, bool isIO = false, int user = 0)
        {
            SaleOrderService SaleOrderServiceClient;
            SaleOrderServiceClient = null;
            SAL_ORDER_HDR objSoHeader;
            List<SAL_ORDER_HDR> soHeaderList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                SaleOrderServiceClient = new SaleOrderService();
                SaleOrderServiceClient = CommonFunctions.InitiateClient(SaleOrderServiceClient);
                objSoHeader = CommonFunctions.Initilize<SAL_ORDER_HDR>();
                serviceUtilityObj = new ServiceUtility();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);

                objSoHeader.SOH_ACTIVE = 1;
                objSoHeader.SOH_BIZUNIT = currentUser.SBUID;
                if (wkfStatus.HasValue)
                {
                    objSoHeader.SOH_STATUS = wkfStatus.Value;
                    serviceUtilityObj.NeedAdvanceFilter = true;
                }
                if (isIO)
                    objSoHeader.SOH_TRX_STATUS = (byte)SaleOrderStatus.InternalOrder;
                if (user > 0)
                    objSoHeader.SOH_CRTD_BY = user;
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CustomerName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                soHeaderList = SaleOrderServiceClient.GetCusPoNumberAutoCompleteList(objSoHeader, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(soHeaderList, Resources.DataFieldRes.SOH_Reference, Resources.DataFieldRes.SOH_Reference);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                SaleOrderServiceClient = null;
                objSoHeader = null;
                serviceUtilityObj = null;
                soHeaderList = null;
            }
        }









        /// <summary>
        /// Get Purchase payment numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetPurchasePaymentNumbers(string searchKey)
        {
            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;
            //POPayment poPaymentObj;
            //List<POPayment> poPaymentList;
            FIN_PAYMENT_VND_HDR finPaymentVndHdrObj;
            List<FIN_PAYMENT_VND_HDR> finPaymentVndHdrList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                poPaymentServiceClient = new POPaymentService();
                poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                finPaymentVndHdrObj = CommonFunctions.Initilize<ERPData.FIN_PAYMENT_VND_HDR>();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                finPaymentVndHdrObj.PVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.POPaymentNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                finPaymentVndHdrObj.PVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                finPaymentVndHdrList = poPaymentServiceClient.GetPaymentNumberAutoCompleteList(finPaymentVndHdrObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(finPaymentVndHdrList, Resources.DataFieldRes.POPaymentNo, Resources.DataFieldRes.POPaymentPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                poPaymentServiceClient = null;
                finPaymentVndHdrObj = null;
                serviceUtilityObj = null;
                finPaymentVndHdrList = null;
            }
        }

        /// <summary>
        /// Get sales receipt numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetSalesReceiptNumbers(string searchKey)
        {
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            //SalesReceipt salesReceiptObj;
            //List<SalesReceipt> salesReceiptList;
            FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj;
            List<FIN_RECEIPT_CUS_HDR> finReceiptCusHdrList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                salesReceiptServiceClient = new SalesReceiptService();
                salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                finReceiptCusHdrObj = CommonFunctions.Initilize<ERPData.FIN_RECEIPT_CUS_HDR>();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                finReceiptCusHdrObj.RCH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.SalesReceiptNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.BizUnit = Convert.ToInt32(currentUser.SBUID);
                finReceiptCusHdrList = salesReceiptServiceClient.GetReceiptNumberAutoCompleteList(finReceiptCusHdrObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(finReceiptCusHdrList, Resources.DataFieldRes.SalesReceiptNo, Resources.DataFieldRes.SalesReceiptPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                salesReceiptServiceClient = null;
                finReceiptCusHdrObj = null;
                serviceUtilityObj = null;
                finReceiptCusHdrList = null;
            }
        }

        /// <summary>
        /// GetDeliveryOrderNumbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetDeliveryOrderNumbers(string searchKey, int Status)
        {
            DeliveryOrderService deliveryOrderServiceClient;
            deliveryOrderServiceClient = null;
            SAL_DESPATCH_HDR objSalesDespatch;
            List<SAL_DESPATCH_HDR> despatchHdrList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                deliveryOrderServiceClient = new DeliveryOrderService();
                deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                objSalesDespatch = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                //objPoInvoice.POH_ACTIVE = 1;
                objSalesDespatch.DPH_STATUS = (byte)WkfStatusEnum.APPROVED;//(byte)0;//For DO Invoicing Advanced search DDL
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.DeliveryOrderNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                despatchHdrList = deliveryOrderServiceClient.GetSaleDespatchNumberAutoCompleteList(objSalesDespatch, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(despatchHdrList, Resources.DataFieldRes.DeliveryOrderNo, Resources.DataFieldRes.DeliveryOrderPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                deliveryOrderServiceClient = null;
                objSalesDespatch = null;
                serviceUtilityObj = null;
                despatchHdrList = null;
            }
        }
        private void GetCostCenter(string searchKey, int Group)
        {
            JavaScriptSerializer serializer;
            DataTable dtCostCenter;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                //currentUser.SBUID
                dtCostCenter = BusinessLogic.Administration.Masters.CostCenterMasterBL.GetCostCenterAccountListByGroup(Group, searchKey, currentUser.SBUID);
                result = dtCostCenter.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        private void GetBudgetCostCenter(string searchKey, int Group,int BudgetPk)
        {
            JavaScriptSerializer serializer;
            DataTable dtCostCenter;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                //currentUser.SBUID
                dtCostCenter = BusinessLogic.Administration.Masters.CostCenterMasterBL.GetBudgetCostCenterAccountListByGroup(Group, searchKey, currentUser.SBUID, BudgetPk);
                result = dtCostCenter.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }

        /// For auto Cost Center With Out Group
        private void GetCostCenterWithoutGrp(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtCostCenter;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                //currentUser.SBUID
                dtCostCenter = BusinessLogic.Administration.Masters.CostCenterMasterBL.GetCostCenterAccountListWithOutGroup(searchKey, currentUser.SBUID);
                result = dtCostCenter.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        /// <summary>
        /// For auto Search Vendor
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetVendor(string searchKey, List<short> role = null)
        {
            VendorMstService VendorMstServiceClient;

            PUR_VENDOR_MST purVendorObj;
            List<PUR_VENDOR_MST> purVendorList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            VendorMstServiceClient = null;
            try
            {
                VendorMstServiceClient = new VendorMstService();
                VendorMstServiceClient = CommonFunctions.InitiateClient(VendorMstServiceClient);
                purVendorObj = CommonFunctions.Initilize<ERPData.PUR_VENDOR_MST>();
                purVendorObj.VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                purVendorObj.VEN_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = HttpUtility.HtmlDecode(searchKey.Trim());
                serviceUtilityObj.IsSBUSpecific = IsSBUVendor;
                if (role != null)
                {
                    purVendorObj.PUR_VENDOR_ROLE_MAP = new System.Data.Objects.DataClasses.EntityCollection<PUR_VENDOR_ROLE_MAP>();
                    role.ForEach(rol =>
                    purVendorObj.PUR_VENDOR_ROLE_MAP.Add(
                        new PUR_VENDOR_ROLE_MAP()
                        {
                            VRM_ROLE = rol
                        }));
                }
                //PUR_VENDOR_MST.PUR_VENDOR_ROLE_MAP.XacRolVendor.xrvPK==3,8,10

                purVendorList = VendorMstServiceClient.GetVendorListAutoCompleteList(purVendorObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(purVendorList, Resources.DataFieldRes.VendorName, Resources.DataFieldRes.VendorPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                VendorMstServiceClient = null;
                purVendorObj = null;
                serviceUtilityObj = null;
                purVendorList = null;
            }
        }

        /// <summary>
        /// For auto Search Vendor with vendor code in display text.
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetVendorNameCode(string searchKey)
        {
            VendorMstService VendorMstServiceClient;

            PUR_VENDOR_MST purVendorObj;
            List<PUR_VENDOR_MST> purVendorList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> vendorNameCodeList;
            VendorMstServiceClient = null;
            try
            {
                VendorMstServiceClient = new VendorMstService();
                VendorMstServiceClient = CommonFunctions.InitiateClient(VendorMstServiceClient);
                purVendorObj = CommonFunctions.Initilize<ERPData.PUR_VENDOR_MST>();
                purVendorObj.VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                purVendorObj.VEN_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = HttpUtility.HtmlDecode(searchKey.Trim());
                serviceUtilityObj.IsSBUSpecific = IsSBUVendor;

                purVendorList = VendorMstServiceClient.GetVendorNameCodeAutoCompleteList(purVendorObj, serviceUtilityObj);
                vendorNameCodeList = purVendorList.Select(vendor => new AutoCompleteBO()
                {
                    Key = vendor.VEN_PK,
                    Name = string.IsNullOrEmpty(vendor.VEN_CODE) ? vendor.VEN_NAME : vendor.VEN_NAME + " - " + vendor.VEN_CODE
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(vendorNameCodeList, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                VendorMstServiceClient = null;
                purVendorObj = null;
                serviceUtilityObj = null;
                purVendorList = null;
                vendorNameCodeList = null;
            }
        }



        /// <summary>
        /// For auto Search Vendor Role
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetVendorRole(string searchKey)
        {
            VendorMstService VendorMstServiceClient;

            PUR_VENDOR_MST purVendorObj;
            List<PUR_VENDOR_MST> purVendorList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            VendorMstServiceClient = null;
            try
            {
                VendorMstServiceClient = new VendorMstService();
                VendorMstServiceClient = CommonFunctions.InitiateClient(VendorMstServiceClient);
                purVendorObj = CommonFunctions.Initilize<ERPData.PUR_VENDOR_MST>();
                purVendorObj.VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                purVendorObj.VEN_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                purVendorList = VendorMstServiceClient.GetVendorRoleListAutoCompleteList(purVendorObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(purVendorList, Resources.DataFieldRes.VendorName, Resources.DataFieldRes.VendorPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                VendorMstServiceClient = null;
                purVendorObj = null;
                serviceUtilityObj = null;
                purVendorList = null;
            }
        }


        /// <summary>
        /// For auto Search Customer with account Type
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetCustomer(string searchKey)
        {
            CustomerMstService CustomerMstServiceClient;

            CRM_CUSTOMER_MST salCustomerObj;
            List<CRM_CUSTOMER_MST> salCustomerList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            CustomerMstServiceClient = null;
            try
            {
                CustomerMstServiceClient = new CustomerMstService();
                CustomerMstServiceClient = CommonFunctions.InitiateClient(CustomerMstServiceClient);
                salCustomerObj = CommonFunctions.Initilize<ERPData.CRM_CUSTOMER_MST>();
                salCustomerObj.CUS_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                salCustomerObj.CUS_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CustomerName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                salCustomerList = CustomerMstServiceClient.GetCustomerListAutoCompleteList(salCustomerObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(salCustomerList, Resources.DataFieldRes.CustomerName, Resources.DataFieldRes.CustomerAccount);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CustomerMstServiceClient = null;
                salCustomerObj = null;
                serviceUtilityObj = null;
                salCustomerList = null;
            }
        }

        /// <summary>
        /// For auto Search Customer with customer ID
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetCustomerList(string searchKey)
        {
            CustomerMstService CustomerMstServiceClient;

            CRM_CUSTOMER_MST crmCustomerObj;
            List<CRM_CUSTOMER_MST> crmCustomerList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            CustomerMstServiceClient = null;
            try
            {
                CustomerMstServiceClient = new CustomerMstService();
                CustomerMstServiceClient = CommonFunctions.InitiateClient(CustomerMstServiceClient);
                crmCustomerObj = CommonFunctions.Initilize<ERPData.CRM_CUSTOMER_MST>();
                crmCustomerObj.CUS_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                crmCustomerObj.CUS_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CustomerName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.IsSBUSpecific = IsSBUCustomer;
                //serviceUtilityObj.FilterValue = HttpUtility.HtmlEncode(searchKey.Trim());
                crmCustomerList = CustomerMstServiceClient.GetCustomerListAutoCompleteList(crmCustomerObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(crmCustomerList, Resources.DataFieldRes.CustomerName, Resources.DataFieldRes.CustomerPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CustomerMstServiceClient = null;
                crmCustomerObj = null;
                serviceUtilityObj = null;
                crmCustomerList = null;
            }
        }

        /// <summary>
        /// For auto Search Customer and Vendor list
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetCustomerVendorList(string searchKey)
        {
            CustomerMstService CustomerMstServiceClient;

            CRM_CUSTOMER_MST crmCustomerObj;
            List<BusinessObject.CommonManagement.AutoCompleteBO> crmCustomerList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            CustomerMstServiceClient = null;
            try
            {
                CustomerMstServiceClient = new CustomerMstService();
                CustomerMstServiceClient = CommonFunctions.InitiateClient(CustomerMstServiceClient);
                crmCustomerObj = CommonFunctions.Initilize<ERPData.CRM_CUSTOMER_MST>();
                crmCustomerObj.CUS_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                crmCustomerObj.CUS_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CustomerName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.IsSBUSpecific = IsSBUVendor;
                crmCustomerList = CustomerMstServiceClient.GetCustomerVendorAutoCompleteList(crmCustomerObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(crmCustomerList, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CustomerMstServiceClient = null;
                crmCustomerObj = null;
                serviceUtilityObj = null;
                crmCustomerList = null;
            }
        }

        /// <summary>
        /// For auto Search Vendor
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetVendorAccount(string searchKey)
        {
            VendorMstService VendorMstServiceClient;

            PUR_VENDOR_MST purVendorObj;
            List<PUR_VENDOR_MST> purVendorList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            VendorMstServiceClient = null;
            try
            {
                VendorMstServiceClient = new VendorMstService();
                VendorMstServiceClient = CommonFunctions.InitiateClient(VendorMstServiceClient);
                purVendorObj = CommonFunctions.Initilize<ERPData.PUR_VENDOR_MST>();
                purVendorObj.VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                purVendorObj.VEN_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                purVendorList = VendorMstServiceClient.GetVendorListAutoCompleteList(purVendorObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(purVendorList, Resources.DataFieldRes.VendorName, Resources.DataFieldRes.VendorAccount);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                VendorMstServiceClient = null;
                purVendorObj = null;
                serviceUtilityObj = null;
                purVendorList = null;
            }
        }

        /// <summary>
        /// For auto Search Currency
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetCurrency(string searchKey, string filterType)
        {
            CurrencyMstService currencyMstServiceClient;

            ADM_CURRENCY_MST admCurrencyMstObj;
            List<ADM_CURRENCY_MST> admCurrencyMstList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            currencyMstServiceClient = null;
            try
            {
                currencyMstServiceClient = new CurrencyMstService();
                currencyMstServiceClient = CommonFunctions.InitiateClient(currencyMstServiceClient);
                admCurrencyMstObj = CommonFunctions.Initilize<ERPData.ADM_CURRENCY_MST>();
                admCurrencyMstObj.CUR_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                admCurrencyMstObj.CUR_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CurrencyCode;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                admCurrencyMstList = currencyMstServiceClient.GetCurrencyListAutoCompleteList(admCurrencyMstObj, serviceUtilityObj);
                var searchResult = (Object)null;
                if (string.IsNullOrEmpty(filterType))
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(admCurrencyMstList, Resources.DataFieldRes.CurrencyCode, Resources.DataFieldRes.CurrencyName, Resources.DataFieldRes.CurrencyPK, true);
                else
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(admCurrencyMstList, Resources.DataFieldRes.CurrencyCode, Resources.DataFieldRes.CurrencyPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                currencyMstServiceClient = null;
                admCurrencyMstObj = null;
                serviceUtilityObj = null;
                admCurrencyMstList = null;
            }
        }

        /// <summary>
        /// For auto Search Vendor Currency
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetVendorCurrency(string searchKey, int vendor, string excDate)
        {
            List<AutoCompleteBO> currencies;
            JavaScriptSerializer serializer;
            DateTime date;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (DateTime.TryParse(excDate, out date))
                {
                    currencies = BusinessLogic.Administration.Masters.CurrencyMaster.GetVendorExchangeCurrencyAuto(searchValue, vendor, date);
                }
                else
                {
                    currencies = BusinessLogic.Administration.Masters.CurrencyMaster.GetVendorExchangeCurrencyAuto(searchValue, vendor, DateTime.Now);
                }

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(currencies, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Bank
        /// </summary>
        /// <param name="searchKey">
        /// <param name="filterType"></param>
        private void GetBank(string searchKey, string filterType)
        {
            BankMstService BankMstServiceClient;
            FIN_CASH_BANK_MST finCashBankMstObj;
            List<FIN_CASH_BANK_MST> finCashBankMstList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            BankMstServiceClient = null;
            try
            {
                BankMstServiceClient = new BankMstService();
                BankMstServiceClient = CommonFunctions.InitiateClient(BankMstServiceClient);
                finCashBankMstObj = CommonFunctions.Initilize<ERPData.FIN_CASH_BANK_MST>();
                finCashBankMstObj.CBM_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                finCashBankMstObj.CBM_ACTIVE = 1;
                finCashBankMstObj.CBM_TYPE = String.IsNullOrEmpty(filterType) ? Convert.ToByte(0) :
                    Convert.ToByte(filterType) == (byte)CashBankType.Cash ? (byte)CashBankType.Cash : (byte)CashBankType.Bank;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.BankName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.IsSBUSpecific = IsSBUBank;
                finCashBankMstList = BankMstServiceClient.GetFinCashBankMstAutoCompleteList(finCashBankMstObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(finCashBankMstList, Resources.DataFieldRes.BankCode, Resources.DataFieldRes.BankName, Resources.DataFieldRes.BankPK, true);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                BankMstServiceClient = null;
                finCashBankMstObj = null;
                serviceUtilityObj = null;
                finCashBankMstList = null;
            }
        }

        /// <summary>
        /// Get Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetInvoiceNumbers(string searchKey, byte group = 0, byte category = 0)
        {
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            FIN_INVOICE_VND_HDR objPoInvoice;
            List<FIN_INVOICE_VND_HDR> poInvoiceList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                poInvoiceServiceClient = new POInvoiceService();
                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                objPoInvoice = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                if (category > 0)
                {
                    objPoInvoice.IVH_CATEGORY = category;
                    objPoInvoice.IVH_GROUP = group;
                }
                objPoInvoice.IVH_BIZUNIT = currentUser.SBUID;
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                //objPoInvoice.POH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.InvoiceNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                poInvoiceList = poInvoiceServiceClient.GetInvoiceNumberAutoCompleteList(objPoInvoice, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(poInvoiceList, Resources.DataFieldRes.InvoiceNo, Resources.DataFieldRes.POInvoicePK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                poInvoiceServiceClient = null;
                objPoInvoice = null;
                serviceUtilityObj = null;
                poInvoiceList = null;
            }
        }


        /// <summary>
        /// Get Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetInvNumbers(string searchKey, byte group = 0, byte category = 0)
        {
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            FIN_INVOICE_VND_HDR objPoInvoice;
            List<FIN_INVOICE_VND_HDR> poInvoiceList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                poInvoiceServiceClient = new POInvoiceService();
                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                objPoInvoice = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                if (category > 0)
                {
                    objPoInvoice.IVH_CATEGORY = category;
                    objPoInvoice.IVH_GROUP = group;
                }
                objPoInvoice.IVH_BIZUNIT = currentUser.SBUID;
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                //objPoInvoice.POH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.InvoiceNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                poInvoiceList = poInvoiceServiceClient.GetInvNumberAutoCompleteList(objPoInvoice, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(poInvoiceList, Resources.DataFieldRes.InvoiceNo, Resources.DataFieldRes.POInvoicePK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                poInvoiceServiceClient = null;
                objPoInvoice = null;
                serviceUtilityObj = null;
                poInvoiceList = null;
            }
        }
        /// <summary>
        /// Get Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetSalesInvoiceNumbers(string searchKey, string group)
        {
            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            FIN_INVOICE_CUS_HDR objSalesInvoice;
            List<FIN_INVOICE_CUS_HDR> salesInvoiceList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                salesInvoiceServiceClient = new SalesInvoiceService();
                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                objSalesInvoice = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                //objSalesInvoice.ICH_CATEGORY = category;
                objSalesInvoice.ICH_GROUP = string.IsNullOrEmpty(group) ? Convert.ToByte(0) : Convert.ToByte(group);
                objSalesInvoice.ICH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);

                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.SalesInvoiceNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                salesInvoiceList = salesInvoiceServiceClient.GetInvoiceNumberAutoCompleteList(objSalesInvoice, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(salesInvoiceList, Resources.DataFieldRes.SalesInvoiceNo, Resources.DataFieldRes.SalesInvoicePK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                salesInvoiceServiceClient = null;
                objSalesInvoice = null;
                serviceUtilityObj = null;
                salesInvoiceList = null;
            }
        }
        /// <summary>
        /// Get Converted Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetConvertedInvNumbers(string searchKey, byte group = 0, byte category = 0)
        {
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            FIN_INVOICE_VND_HDR objPoInvoice;
            List<FIN_INVOICE_VND_HDR> poInvoiceList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                poInvoiceServiceClient = new POInvoiceService();
                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                objPoInvoice = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                if (category > 0)
                {
                    objPoInvoice.IVH_CATEGORY = category;
                    objPoInvoice.IVH_GROUP = group;
                }
                objPoInvoice.IVH_BIZUNIT = currentUser.SBUID;
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                //objPoInvoice.POH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.InvoiceNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                poInvoiceList = poInvoiceServiceClient.GetConvertedInvNumbersAutoCompleteList(objPoInvoice, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(poInvoiceList, Resources.DataFieldRes.InvoiceNo, Resources.DataFieldRes.POInvoicePK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                poInvoiceServiceClient = null;
                objPoInvoice = null;
                serviceUtilityObj = null;
                poInvoiceList = null;
            }
        }

        private void GetWorkOrderNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtWONumbers = BusinessLogic.WorkOrder.WorkOrderBL.GetAutoCompleteWONumber((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchKey);

                result = dtWONumbers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("WIH_PK"),
                    Name = row.Field<string>("WIH_NO")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetFinYearOpeningNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtOpeningNumbers = BusinessLogic.Finance.CommSetupBL.GetFinYearOpeningNumbers(currentUser.SBUID, searchKey);

                result = dtOpeningNumbers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetMIRateAdjustNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtOpeningNumbers = BusinessLogic.StoreManagement.MaterialIssue.GetMIRateAdjustNumbers(currentUser.SBUID, searchKey);

                result = dtOpeningNumbers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetCWIPNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtCWIPNumbers = BusinessLogic.Finance.CommSetupBL.GetCWIPFieldValues("CWH_NO", searchKey, currentUser.SBUID, currentUser.CurrentDeptPK);

                result = dtCWIPNumbers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetCWIPAccount(int GroupPK, int CWIP_PK, string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtCWIPAcounts = BusinessLogic.Finance.CommSetupBL.GetCOAByParent(GroupPK, CWIP_PK, searchKey);

                result = dtCWIPAcounts.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("COA_PK"),
                    Name = row.Field<string>("COA_TEXT")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetQuotation(string searchKey, string CustomerId, int sbu)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                if (string.IsNullOrEmpty(searchKey))
                    searchKey = "%";
                int CustomerPK = 0;
                if (string.IsNullOrEmpty(CustomerId))
                    CustomerPK = 0;
                else
                    CustomerPK = Convert.ToInt32(CustomerId);
                DataTable dtQuotation = BusinessLogic.Sales.QuotationBL.GetCrmQuoations(searchKey, CustomerPK, currentUser.SBUID);

                result = dtQuotation.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetCompound(string searchKey, int Status,int CompanyPK)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                searchKey = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                DataTable dtCompound = BusinessLogic.CommonManagement.CommonBL.GetCompound(searchKey, Status, currentUser.SBUID, CompanyPK);
                result = dtCompound.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetDispersion(string searchKey, int Status, int CompanyPK,string Type)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                searchKey = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                DataTable dtCompound = BusinessLogic.CommonManagement.CommonBL.GetDispersion(searchKey, Status, currentUser.SBUID, CompanyPK,Type);
                result = dtCompound.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetWorkOrderItemsAuto(int OperationPK, int Type, int CustomerPK, string SearchKey, int BrandPK = 0)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtWOItems = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderItemsAuto(OperationPK, Type, CustomerPK, currentUser.SBUID, SearchKey, BrandPK);

                result = dtWOItems.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetWorkOrderItemsForFilterAuto(string SearchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                DataTable dtWOItems = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderItemsForFilterAuto(SearchKey, currentUser.SBUID);

                result = dtWOItems.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetFCVoucherNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtFCVoucher;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtFCVoucher = BusinessLogic.Finance.FCReverseBL.GetFCVoucherNumberAuto((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtFCVoucher.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("HRH_PK"),

                    Name = row.Field<string>("HRH_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get VSE Field auto complete
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetVatSaleExportField(string itemField, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtVSEField;
            List<AutoCompleteBO> result;
            try
            {
                int x = 0;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtVSEField = BusinessLogic.Finance.VatSaleExportBL.GetVatSaleFieldAuto((byte)DbActiveStatus.ACTIVE, itemField, currentUser.SBUID, searchValue);
                result = dtVSEField.AsEnumerable().Select(row => new AutoCompleteBO()
                {

                    //Key=Convert.ToInt64(row.Field<string>("PK")),
                    //Key=Convert.ToInt32(row.Field<String>("PK")),
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }


        //private void GetSalesInvoiceNumbers(string searchKey)
        //{
        //    SalesInvoiceService salesInvoiceServiceClient;
        //    salesInvoiceServiceClient = null;
        //    FIN_INVOICE_CUS_HDR objSalesInvoice;
        //    List<FIN_INVOICE_CUS_HDR> salesInvoiceList;
        //    ServiceUtility serviceUtilityObj;
        //    JavaScriptSerializer serializer;

        //    try
        //    {
        //        salesInvoiceServiceClient = new SalesInvoiceService();
        //        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
        //        objSalesInvoice = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
        //        serviceUtilityObj = new ServiceUtility();
        //        serializer = new JavaScriptSerializer();
        //        serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
        //        serviceUtilityObj.FilterBy = Resources.DataFieldRes.SalesInvoiceNo;
        //        serviceUtilityObj.FilterValue = searchKey.Trim();
        //        salesInvoiceList = salesInvoiceServiceClient.GetInvoiceNumberAutoCompleteList(objSalesInvoice, serviceUtilityObj);
        //        var searchResult = CommonFunctions.GetFormatedAutoCompleteList(salesInvoiceList, Resources.DataFieldRes.SalesInvoiceNo, Resources.DataFieldRes.SalesInvoicePK);
        //        response.ClearContent();
        //        response.Write(serializer.Serialize(searchResult));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        serializer = null;
        //        salesInvoiceServiceClient = null;
        //        objSalesInvoice = null;
        //        serviceUtilityObj = null;
        //        salesInvoiceList = null;
        //    }
        //}

        /// <summary>
        /// Get Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAdvanceSalesInvoiceNumbers(string searchKey)
        {
            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            FIN_INVOICE_CUS_HDR objSalesInvoice;
            List<FIN_INVOICE_CUS_HDR> salesInvoiceList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                salesInvoiceServiceClient = new SalesInvoiceService();
                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                objSalesInvoice = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.SalesInvoiceNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                salesInvoiceList = salesInvoiceServiceClient.GetAdvanceSalesInvoiceNumberAutoCompleteList(objSalesInvoice, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(salesInvoiceList, Resources.DataFieldRes.SalesInvoiceNo, Resources.DataFieldRes.SalesInvoicePK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                salesInvoiceServiceClient = null;
                objSalesInvoice = null;
                serviceUtilityObj = null;
                salesInvoiceList = null;
            }
        }


        /// <summary>
        /// Get Invoice numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetCrDrNumbers(string searchKey)
        {
            FinCrDrHdrNoteService FinCrDrHdrNoteServiceClient;
            FinCrDrHdrNoteServiceClient = null;
            FIN_CRDR_NOTE_HDR objFinCrDrNoteHdr;
            List<FIN_CRDR_NOTE_HDR> FinCrDrNoteHdrList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                FinCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                FinCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(FinCrDrHdrNoteServiceClient);
                objFinCrDrNoteHdr = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                objFinCrDrNoteHdr.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                objFinCrDrNoteHdr.CDH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CrDrNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                FinCrDrNoteHdrList = FinCrDrHdrNoteServiceClient.GetCrDrNoteAutoCompleteList(objFinCrDrNoteHdr, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(FinCrDrNoteHdrList, Resources.DataFieldRes.CrDrNo, Resources.DataFieldRes.CrDrPk);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                FinCrDrHdrNoteServiceClient = null;
                objFinCrDrNoteHdr = null;
                serviceUtilityObj = null;
                FinCrDrNoteHdrList = null;
            }
        }

        /// <summary>
        /// Get Sales Cr/Dr numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetSalCrDrNumbers(string searchKey)
        {
            FinCrDrHdrNoteService FinCrDrHdrNoteServiceClient;
            FinCrDrHdrNoteServiceClient = null;
            FIN_CRDR_NOTE_HDR objFinCrDrNoteHdr;
            List<FIN_CRDR_NOTE_HDR> FinCrDrNoteHdrList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                FinCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                FinCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(FinCrDrHdrNoteServiceClient);
                objFinCrDrNoteHdr = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                objFinCrDrNoteHdr.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                objFinCrDrNoteHdr.CDH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CrDrNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                FinCrDrNoteHdrList = FinCrDrHdrNoteServiceClient.GetSalCrDrNoteAutoCompleteList(objFinCrDrNoteHdr, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(FinCrDrNoteHdrList, Resources.DataFieldRes.CrDrNo, Resources.DataFieldRes.CrDrPk);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                FinCrDrHdrNoteServiceClient = null;
                objFinCrDrNoteHdr = null;
                serviceUtilityObj = null;
                FinCrDrNoteHdrList = null;
            }
        }

        /// <summary>
        /// Get Purchase Cr/Dr numbers
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetPurCrDrNumbers(string searchKey)
        {
            FinCrDrHdrNoteService FinCrDrHdrNoteServiceClient;
            FinCrDrHdrNoteServiceClient = null;
            FIN_CRDR_NOTE_HDR objFinCrDrNoteHdr;
            List<FIN_CRDR_NOTE_HDR> FinCrDrNoteHdrList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                FinCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                FinCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(FinCrDrHdrNoteServiceClient);
                objFinCrDrNoteHdr = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                objFinCrDrNoteHdr.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                objFinCrDrNoteHdr.CDH_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CrDrNo;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                FinCrDrNoteHdrList = FinCrDrHdrNoteServiceClient.GetPurCrDrNoteAutoCompleteList(objFinCrDrNoteHdr, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(FinCrDrNoteHdrList, Resources.DataFieldRes.CrDrNo, Resources.DataFieldRes.CrDrPk);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                FinCrDrHdrNoteServiceClient = null;
                objFinCrDrNoteHdr = null;
                serviceUtilityObj = null;
                FinCrDrNoteHdrList = null;
            }
        }

        /// <summary>
        /// For auto Search Account
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAccounts(string searchKey, string voucherType, int accType, int companyPK = 0)
        {
            AccountMstService AccountMstServiceClient;
            AccountMstServiceClient = null;

            FIN_COA_MST accountMstObj;
            List<FIN_COA_MST> accountMstList;

            ServiceUtility serviceUtilityObj;

            JavaScriptSerializer serializer;
            DataTable dt;
            AccountMstServiceClient = null;
            try
            {
                AccountMstServiceClient = new AccountMstService();
                AccountMstServiceClient = CommonFunctions.InitiateClient(AccountMstServiceClient);

                accountMstObj = CommonFunctions.Initilize<FIN_COA_MST>();
                //accountMstObj.COA_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "COA");
                if (dt != null && dt.Rows.Count > 0)
                    accountMstObj.COA_BIZUNIT = dt.Rows[0]["ACF_VALUE"].ToString() == "1" ? -1 : Convert.ToInt16(currentUser.SBUID);
                else
                    accountMstObj.COA_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                accountMstObj.COA_IS_GROUP = false;

                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();

                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterValue = searchKey.Trim();

                accountMstList = AccountMstServiceClient.GetCoaMstAutoCompleteList(accountMstObj, voucherType, accType, serviceUtilityObj, companyPK);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(accountMstList, Resources.DataFieldRes.AccountCode, Resources.DataFieldRes.AccountName, Resources.DataFieldRes.AccountPK, true);

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch
            {
                throw;
            }
            finally
            {
                serializer = null;
                AccountMstServiceClient = null;
                accountMstObj = null;
                serviceUtilityObj = null;
                accountMstList = null;
            }
        }

        private void GetAccountsMst(string searchKey, int accType)
        {
            AccountMstService AccountMstServiceClient;
            AccountMstServiceClient = null;

            FIN_COA_MST accountMstObj;
            List<FIN_COA_MST> accountMstList;

            ServiceUtility serviceUtilityObj;

            JavaScriptSerializer serializer;
            DataTable dt;

            AccountMstServiceClient = null;
            try
            {
                AccountMstServiceClient = new AccountMstService();
                AccountMstServiceClient = CommonFunctions.InitiateClient(AccountMstServiceClient);

                accountMstObj = CommonFunctions.Initilize<FIN_COA_MST>();
                dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "COA");
                if (dt != null && dt.Rows.Count > 0)
                    accountMstObj.COA_BIZUNIT = dt.Rows[0]["ACF_VALUE"].ToString() == "1" ? -1 : Convert.ToInt16(currentUser.SBUID);
                else
                    accountMstObj.COA_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                //accountMstObj.COA_IS_GROUP = false;

                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();

                //serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.AccountName;

                accountMstList = AccountMstServiceClient.GetAccountCoaMstAutoCompleteList(accountMstObj, accType, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(accountMstList, Resources.DataFieldRes.AccountCode, Resources.DataFieldRes.AccountName, Resources.DataFieldRes.AccountPK, true);

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch
            {
                throw;
            }
            finally
            {
                serializer = null;
                AccountMstServiceClient = null;
                accountMstObj = null;
                serviceUtilityObj = null;
                accountMstList = null;
            }
        }

        private void GetSubAccounts(string searchKey, int accType)
        {
            ERPEntities currentEntity;
            CommonService commonService;
            FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
            List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
            JavaScriptSerializer serializer;
            string relquery;

            try
            {
                currentEntity = new ERPEntities();
                commonService = new CommonService();
                finCoaSubTypeCfgObj = new FIN_COA_SUB_TYPE_CFG();
                serializer = new JavaScriptSerializer();

                commonService = CommonFunctions.InitiateClient(commonService);
                finCoaSubTypeCfgObj = CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
                finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                finCoaSubTypeCfgObj.CST_PK = accType;
                finCoaSubTypeCfgList = commonService.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
                if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                {
                    relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                    var ddlValues = ERP.Utilities.CommonFunctions.GetResults(new DDLMaster(), currentEntity, relquery);
                    ddlValues = ddlValues.Where(c => c.Value.StartsWith(searchKey)).ToList();
                    var searchResult = ERP.Utilities.CommonFunctions.GetFormatedAutoCompleteList(ddlValues, "Value", "PK");
                    response.ClearContent();
                    response.Write(serializer.Serialize(searchResult));
                }
            }
            catch
            { }
        }

        private void GetWHTAccounts(string searchKey)
        {

            JavaScriptSerializer serializer;
            DataTable dtVendorAccount;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtVendorAccount = CommonBL.GetTaxMstListAuto(0, (int)TaxType.Tax, (int)TaxSubCategory.WHT, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtVendorAccount.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("TAX_PK"),

                    Name = row.Field<string>("TAX_HEAD")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }


        }
        private void GetVatBuyAccounts(string searchKey)
        {

            JavaScriptSerializer serializer;
            DataTable dtVendorAccount;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtVendorAccount = CommonBL.GetTaxMstListAuto(0, (int)TaxType.Tax, (int)TaxSubCategory.VATBuy, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtVendorAccount.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("TAX_PK"),

                    Name = row.Field<string>("TAX_HEAD")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }


        }
        private void GetAddressTypeList(string searchKey)
        {

            JavaScriptSerializer serializer;
            DataTable dtVendorContacts;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                //string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";

                dtVendorContacts = BusinessLogic.VendorManagement.VendorMaster.GetVendorAddressTypeList((int)currentUser.SBUID);
                result = dtVendorContacts.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = Convert.ToInt32(row.Field<Byte>("CFG_VALUE")),

                    Name = row.Field<string>("CFG_DATA")

                }).ToList();
                //var searchResult = ERP.Utilities.CommonFunctions.GetFormatedAutoCompleteList(ddlValues, "Value", "PK");//
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }


        }
        private void GetVendorContacts(string searchKey, int VendorPk)
        {
            JavaScriptSerializer serializer;
            DataTable dtVendorContacts;
            DataSet dsVendorContacts = new DataSet();
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                //string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dsVendorContacts = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.ACTIVE), 0, VendorPk, 0);
                if (dsVendorContacts != null && dsVendorContacts.Tables.Count > 0)
                {
                    dtVendorContacts = dsVendorContacts.Tables[0];

                    //dtVendorContacts = BusinessLogic.VendorManagement.VendorMaster.GetVendorAddressTypeList((int)currentUser.SBUID);
                    result = dtVendorContacts.AsEnumerable().Select(row => new AutoCompleteBO()
                    {
                        Key = Convert.ToInt32(row.Field<int>(Resources.DataFieldRes.VncPk)),

                        Name = row.Field<string>(Resources.DataFieldRes.VncName)

                    }).ToList();
                    //var searchResult = ERP.Utilities.CommonFunctions.GetFormatedAutoCompleteList(ddlValues, "Value", "PK");//
                    var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                    response.ClearContent();
                    response.Write(serializer.Serialize(searchResult));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetAutoPayElement(string searchKey, string payElmntValue)
        {
            JavaScriptSerializer serializer;
            DataTable dtPayElmnt;
            // DataSet dsPayElmnt = new DataSet();
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                //string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtPayElmnt = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetPayElementSearchList(currentUser.SBUID, payElmntValue, searchKey);
                if (dtPayElmnt != null && dtPayElmnt.Rows.Count > 0)
                {
                    result = dtPayElmnt.AsEnumerable().Select(row => new AutoCompleteBO()
                    {
                        Key = Convert.ToInt32(row.Field<int>(Resources.DataFieldRes.PK)),
                        Name = row.Field<string>(Resources.DataFieldRes.Value)
                    }).ToList();

                    var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                    response.ClearContent();
                    response.Write(serializer.Serialize(searchResult));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetReportGroup(string searchKey)
        {
            ReportService reportServiceClient;
            reportServiceClient = null;
            ADM_REPORT_GROUP_CFG admReportGroupCfgObj;
            List<ADM_REPORT_GROUP_CFG> admReportGroupCfgList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                reportServiceClient = new ReportService();
                reportServiceClient = CommonFunctions.InitiateClient(reportServiceClient);
                admReportGroupCfgObj = CommonFunctions.Initilize<ADM_REPORT_GROUP_CFG>();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                admReportGroupCfgObj.RGC_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.ReportGroupName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                admReportGroupCfgList = reportServiceClient.GetReportGroupAutoCompleteList(admReportGroupCfgObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(admReportGroupCfgList, Resources.DataFieldRes.ReportGroupName, Resources.DataFieldRes.ReportGroupPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                reportServiceClient = null;
                admReportGroupCfgObj = null;
                serviceUtilityObj = null;
                admReportGroupCfgList = null;
            }
        }

        private void GetReport(string searchKey, string filterType)
        {
            ReportService reportServiceClient;
            reportServiceClient = null;
            ADM_REPORT_CFG admReportCfgObj;
            List<ADM_REPORT_CFG> admReportCfgList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;

            try
            {
                reportServiceClient = new ReportService();
                reportServiceClient = CommonFunctions.InitiateClient(reportServiceClient);
                admReportCfgObj = CommonFunctions.Initilize<ADM_REPORT_CFG>();
                // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                admReportCfgObj.RPT_ACTIVE = 1;
                if (!string.IsNullOrEmpty(filterType) && Convert.ToInt32(filterType) > 0)
                {
                    admReportCfgObj.RPT_GROUP = Convert.ToInt32(filterType);
                }
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.ReportName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                if (HttpContext.GetGlobalResourceObject("ConfigurationsRes", "ShowReprotsMappingTab").ToString() == "1")
                    serviceUtilityObj.User = currentUser.PKUser;
                admReportCfgList = reportServiceClient.GetReportCfgAutoCompleteList(admReportCfgObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(admReportCfgList, Resources.DataFieldRes.ReportName, Resources.DataFieldRes.ReportPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                reportServiceClient = null;
                admReportCfgObj = null;
                serviceUtilityObj = null;
                admReportCfgList = null;
            }
        }

        private void GetReportNew(string searchKey, string filterType)
        {
            DataTable dtReports;
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            int UserID = 0;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                if (HttpContext.GetGlobalResourceObject("ConfigurationsRes", "ShowReprotsMappingTab").ToString() == "1")
                    UserID = currentUser.PKUser;
                dtReports = BusinessLogic.CommonManagement.CommonBL.GetReport(UserID, Convert.ToInt32(filterType), currentUser.SBUID, 1, searchValue);
                result = dtReports.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("RPT_PK"),
                    Name = row.Field<string>("RPT_NAME")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// GetProductNature
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetProductNature(string searchKey, string filterType)
        {
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<ADM_CONST_MST> admConstMstList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                int cngValue = 0;
                switch ((ProductNatureEnum)Enum.Parse(typeof(ProductNatureEnum), filterType))
                {
                    case ProductNatureEnum.Category:
                        cngValue = CommonConstants.Category;
                        break;
                    case ProductNatureEnum.Classification:
                        cngValue = CommonConstants.Classification;
                        break;
                    case ProductNatureEnum.Length:
                        cngValue = CommonConstants.Length;
                        break;
                    case ProductNatureEnum.Shade:
                        cngValue = CommonConstants.Shade;
                        break;
                    case ProductNatureEnum.Size:
                        cngValue = CommonConstants.Size;
                        break;
                    case ProductNatureEnum.Surface:
                        cngValue = CommonConstants.Surface;
                        break;
                    case ProductNatureEnum.Thickness:
                        cngValue = CommonConstants.Thickness;
                        break;
                    case ProductNatureEnum.Type:
                        cngValue = CommonConstants.Type;
                        break;
                }
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.ConstName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                admConstMstList = CommonServiceClient.GetConstMstAutoCompleteList(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, (int)ConstGroupType.Product, cngValue, currentUser.SBUID, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(admConstMstList, Resources.DataFieldRes.ConstName, Resources.DataFieldRes.ConstPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                serviceUtilityObj = null;
                admConstMstList = null;
            }
        }

        private void GetAllItem(string searchKey, string filterType)
        {
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<INV_ITEM_MST> INV_ITEM_MSTList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.FilterBy = filterType;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                INV_ITEM_MSTList = CommonServiceClient.GetAllInvItemMstAutoCompleteList(Convert.ToByte(DbActiveStatus.ACTIVE), serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                if (filterType == Resources.DataFieldRes.ItemName)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemName, Resources.DataFieldRes.ItemPK);
                }
                else if (filterType == Resources.DataFieldRes.ItemCode)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                }

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                serviceUtilityObj = null;
                INV_ITEM_MSTList = null;
            }

        }

        private void GetAllItemWithCodeName(string searchKey, string filterType)
        {
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<INV_ITEM_MST> INV_ITEM_MSTList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.FilterBy = filterType;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                INV_ITEM_MSTList = CommonServiceClient.GetAllInvItemMstAutoCompleteList(Convert.ToByte(DbActiveStatus.ACTIVE), serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemName, Resources.DataFieldRes.ItemPK, true);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                serviceUtilityObj = null;
                INV_ITEM_MSTList = null;
            }

        }

        /// <summary>
        /// Method to get Products.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetProducts(string searchKey, byte? ItemGrade = null, string BinSubType = null, int? itemPK = null, string Active = null)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                int buzUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                var lstOrders = CommonBL.GetProducts(searchKey.Trim(), buzUnit, ItemGrade, BinSubType, itemPK, string.IsNullOrEmpty(Active) ? 1 : Convert.ToInt32(Active));
                result = lstOrders.Select(row => new AutoCompleteBO()
                {
                    Key = row.ProductPK,
                    Name = row.ProductName
                }).OrderBy(c => c.Name).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }

        /// <summary>
        /// Method to get Rel Products.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetRelatedProducts(string searchKey, byte? ItemGrade = null, string BinSubType = null, int? itemPK = null)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                int buzUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                var lstOrders = CommonBL.GetProducts(searchKey.Trim(), buzUnit, ItemGrade, BinSubType, itemPK);
                result = lstOrders.Select(row => new AutoCompleteBO()
                {
                    Key = row.ProductPK,
                    Name = row.ProductName
                }).OrderBy(c => c.Name).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }

        /// <summary>
        /// Get Brand Products Auto
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchKey"></param>
        private void GetAutoBrandProducts(string searchBy, string searchKey, string active)
        {
            JavaScriptSerializer serializer;
            DataTable dtBrand;
            List<AutoCompleteBO> result;
            int activeStatus = string.IsNullOrEmpty(active) ? Convert.ToInt32(DbActiveStatus.ACTIVE) : Convert.ToInt32(active);
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtBrand = CommonBL.GetBrandProducts(searchBy, searchKey, currentUser.SBUID, activeStatus);
                result = dtBrand.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// GetProductNature
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        /// 

        private void GetProductMaster(string searchKey, string filterType)
        {
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<INV_ITEM_MST> INV_ITEM_MSTList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.FilterBy = filterType;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                INV_ITEM_MSTList = CommonServiceClient.GetInvItemMstAutoCompleteList(Convert.ToByte(DbActiveStatus.ACTIVE), serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                if (filterType == Resources.DataFieldRes.ItemName)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemName, Resources.DataFieldRes.ItemPK);
                }
                else if (filterType == Resources.DataFieldRes.ItemCode)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                }

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                serviceUtilityObj = null;
                INV_ITEM_MSTList = null;
            }
        }

        /// <summary>
        /// GetMessageCount
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetMessageCount()
        {
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<SpWkfTransactionNewCountGet_Result> MessageCountList;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                serializer = new JavaScriptSerializer();
                MessageCountList = CommonServiceClient.GetMessageCount(currentUser.PKUser, null);
                response.ClearContent();
                if (MessageCountList != null && MessageCountList.Count > 0)
                {
                    response.Write(serializer.Serialize(MessageCountList[0].usrInboxCount));
                }
                else
                {
                    response.Write(serializer.Serialize(0));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                MessageCountList = null;
            }
        }

        private void GetJournalAccounts(string searchKey, int accType)
        {
            AccountMstService AccountMstServiceClient;
            AccountMstServiceClient = null;

            List<DDLMaster> ddlValues;

            ServiceUtility serviceUtilityObj;

            JavaScriptSerializer serializer;

            AccountMstServiceClient = null;
            try
            {
                AccountMstServiceClient = new AccountMstService();
                AccountMstServiceClient = CommonFunctions.InitiateClient(AccountMstServiceClient);

                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();

                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.BizUnit = currentUser.SBUID;

                ddlValues = AccountMstServiceClient.GetJournalAccountsAutoCompleteList(accType, serviceUtilityObj);
                ddlValues = CommonFunctions.HtmlDecode(ddlValues, Resources.DataFieldRes.Value);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(ddlValues, Resources.DataFieldRes.Value, Resources.DataFieldRes.PK);

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch
            {
                throw;
            }
            finally
            {
                serializer = null;
                AccountMstServiceClient = null;
                serviceUtilityObj = null;
            }
        }
        /// <summary>
        /// Get UOM Autocomplete List
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetUOM(string searchKey, string type)
        {
            InvItemMstService invItemMstServiceClient;
            ServiceUtility serviceUtilityObj;
            INV_UOM_MST invUomMstObj;
            List<INV_UOM_MST> invUomMstList;
            JavaScriptSerializer serializer;
            try
            {
                invItemMstServiceClient = new InvItemMstService();
                invItemMstServiceClient = CommonFunctions.InitiateClient(invItemMstServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.CurrentPage = -1;
                serviceUtilityObj.PageSize = -1;
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.UomCode;
                serviceUtilityObj.FilterValue = searchKey;
                invUomMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_UOM_MST>();
                invUomMstObj.UOM_TYPE = string.IsNullOrEmpty(type) ? Convert.ToByte(0) : Convert.ToByte(type);
                invUomMstObj.UOM_PK = 0;
                invUomMstObj.UOM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                invUomMstList = invItemMstServiceClient.GetUomMstAutoCompleteList(invUomMstObj, serviceUtilityObj).OrderBy(f => f.UOM_CODE).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(invUomMstList, Resources.DataFieldRes.UomCode, Resources.DataFieldRes.UomPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                invItemMstServiceClient = null;
                serviceUtilityObj = null;
                invUomMstObj = null;
                invUomMstList = null;
            }
        }

        /// <summary>
        /// GetAutoNationality
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoTaskUsers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtTaskUsers;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTaskUsers = BusinessLogic.HRMS.TaskTracker.TaskHomeBL.GetUsers(0, searchValue, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
                result = dtTaskUsers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("USER_PK"),

                    Name = row.Field<string>("EMP_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// GetAutoCustomerForAgentComm
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoCustomerForAgentComm(string searchKey, string SearchBy)
        {
            JavaScriptSerializer serializer;
            DataTable dtTaskUsers;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTaskUsers = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetUsers(searchValue, currentUser.SBUID, Convert.ToInt16(SearchBy));
                result = dtTaskUsers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("PK"),

                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetAutoCusScInvForAgentComm(string searchKey, string SearchBy)
        {
            JavaScriptSerializer serializer;
            DataTable dtTaskUsers;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTaskUsers = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAutoCusScInvForAgentComm(searchValue, currentUser.SBUID, Convert.ToInt16(SearchBy));
                result = dtTaskUsers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("PK"),

                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// GetAutoCustomerForAgentComm Transaction
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoForAgentComm(string searchKey, string SearchBy)
        {
            JavaScriptSerializer serializer;
            DataTable dtTaskUsers;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTaskUsers = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAgtCommAuto(searchValue, currentUser.SBUID, Convert.ToInt16(SearchBy));
                result = dtTaskUsers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("PK"),

                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// GetAutoEmployeeList
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetEmployeeAutoComplete(string searchKey, int? EmpCategory = null, int? EmpBranch = null, int? EmpType = null, int? EmploymentType = null, int? EmpCompany = null, int?
            EmpDept = null, int? empDesignation = null, int? PaymentMode = null, int? EmpCurrency = null, string toDate = null)
        {
            JavaScriptSerializer serializer;
            DataTable dtEmployees;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtEmployees = BusinessLogic.HRMS.Employee.EmployeeDocBL.GetAutoCompleteEmployeeList((int?)null, searchValue, (int)DbActiveStatus.ACTIVE, EmpCategory, EmpBranch, EmpType, EmploymentType, EmpCompany, EmpDept, empDesignation, PaymentMode, EmpCurrency, toDate);

                result = dtEmployees.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("empPK"),
                    Name = row.Field<string>("emptext")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// GetAutoEmployeeList Payroll
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetEmployeePayrollAutoComplete(string FromDate, string ToDate, string searchKey, int? EmpCategory = null, int? EmpBranch = null, int? EmpType = null, int? EmploymentType = null, int? EmpCompany = null,
            int? EmpDept = null, int? empDesignation = null, int? PaymentMode = null, int? EmpCurrency = null, int? EmpPayrollType = null)
        {
            JavaScriptSerializer serializer;
            DataTable dtEmployees;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtEmployees = BusinessLogic.HRMS.Employee.EmployeeDocBL.GetAutoCompleteEmployeePayrollList((int?)null, FromDate, ToDate, searchValue, (int)DbActiveStatus.ACTIVE, EmpCategory, EmpBranch,
                    EmpType, EmploymentType, EmpCompany, EmpDept, empDesignation, PaymentMode, EmpCurrency, EmpPayrollType);

                result = dtEmployees.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("empPK"),
                    Name = row.Field<string>("emptext")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        /// <summary>
        /// Employee AutoComplete By Location
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetEmployeeAutoComplete(string searchKey, string filterType, int? EmpCategory = null, int? EmpDept = null, int? EmpType = null, int? ProcessMode = null, string toDate = null)
        {
            JavaScriptSerializer serializer;
            DataTable dtEmployees;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtEmployees = BusinessLogic.HRMS.Employee.EmployeeDocBL.GetAutoCompleteEmployeeListByFilter((int?)null, searchValue, filterType, (int)DbActiveStatus.ACTIVE, EmpCategory, EmpDept, EmpType, ProcessMode, toDate);

                result = dtEmployees.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("empPK"),
                    Name = row.Field<string>("emptext")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetSalaryPaymenetNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtSalPay;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtSalPay = BusinessLogic.HRMS.Payroll.SalaryPaymentBL.GetSalaryPaymentNumbers((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtSalPay.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PSH_PK"),

                    Name = row.Field<string>("PSH_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        private void GetAutoEdocEmployee(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtTaskUsers;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTaskUsers = BusinessLogic.HRMS.eDocs.EDocManagementBL.GetAutoEdocEmployee(searchValue);
                result = dtTaskUsers.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<Int32>("usrPK"),
                    Name = row.Field<string>("usrEmployeeText")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        private void GetOvertimeDetailsNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtSalPay;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtSalPay = BusinessLogic.HRMS.Payroll.OvertimeCalculatorBL.GetOvertimeDetailsNumbers((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtSalPay.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("EOE_PK"),

                    Name = row.Field<string>("EOE_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetOtherLeaveEntryNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtSalPay;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtSalPay = BusinessLogic.HRMS.Payroll.OvertimeCalculatorBL.GetOtherLeaveEntryNumbers((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtSalPay.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("EOL_PK"),

                    Name = row.Field<string>("EOL_DOC_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetEmpTransferNumbers(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtSalPay;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtSalPay = BusinessLogic.HRMS.Employee.EmployeeTransferBL.GetEmpTransferNumbers((byte)DbActiveStatus.ACTIVE, currentUser.SBUID, searchValue);
                result = dtSalPay.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("EFH_PK"),

                    Name = row.Field<string>("EFH_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        #endregion

        /// <summary>
        /// GetAutoNationality
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoNationality(string searchKey, string OpParam)
        {
            JavaScriptSerializer serializer;
            DataTable dtNationality;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtNationality = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoNationality(searchValue, OpParam);
                result = dtNationality.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CNT_PK"),

                    Name = row.Field<string>("CNT_NATIONALITY")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Auto Fill  ExitReason
        /// </summary>
        /// <param name="conPK"></param>
        /// <param name="searchKey"></param>
        /// 


        /// <summary>
        /// For auto Search Country
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetDesignation(int designationPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtDesignation;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                dtDesignation = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDesignation(designationPK, searchKey);
                result = dtDesignation.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int16>("dsgPK"),
                    Name = row.Field<string>("dsgName")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Designation By Job Category and Job Level
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetDesignationByJob(string searchKey, int? JobCategory = null, int? JobLevel = null)
        {
            JavaScriptSerializer serializer;
            DataTable dtDesignation;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                dtDesignation = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDesignationByJob(searchKey, JobCategory, JobLevel);
                result = dtDesignation.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int16>("dsgPK"),
                    Name = row.Field<string>("dsgName")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetExitReason(int conPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtExitReason;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtExitReason = BusinessLogic.HRMS.Employee.EmployeeExperienceBL.GetExitReason(ExitReasonPk, searchValue);
                result = dtExitReason.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("CON_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetAutoProfession(int conPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtProfession;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtProfession = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoFillProfession(ProfessionPk, searchValue);
                result = dtProfession.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("CON_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        // Auto Fill Religion
        private void GetAutoReligion(int conPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtReligion;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtReligion = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoFillReligion(ReligionPk, searchValue);
                result = dtReligion.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("CON_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        // Auto Fill Team
        private void GetAutoTeam(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtTeam;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTeam = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoFillTeam(searchValue);
                result = dtTeam.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("CON_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }


        // Auto Fill Sub Religion
        private void GetAutoSubReligion(int conPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtSubReligion;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtSubReligion = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoSubReligion(SubReligionPk, searchValue);
                result = dtSubReligion.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("CON_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Auto Fill Department
        /// </summary>
        /// <param name="conPK"></param>
        /// <param name="searchKey"></param>
        private void GetAutoDepartment(int conPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtDepartment;
            List<AutoCompleteComboBoxBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtDepartment = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoFillDepartment(DepartmentPk, searchValue);
                result = dtDepartment.AsEnumerable().Select(row => new AutoCompleteComboBoxBO()
                {
                    Key = row.Field<Int32>("CON_PK"),
                    Name = row.Field<string>("CON_NAME"),
                    Code = row.Field<string>("CON_CODE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteComboBoxList(result, "Name", "Key", "Code");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Auto Fill DepartmentAuto
        /// </summary>
        /// <param name="conPK"></param>
        /// <param name="searchKey"></param>
        private void GetDepartmentAutocomplete(int conPK, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtDepartment;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtDepartment = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetFillDepartmentAutocomplete(DepartmentPk, searchValue);
                result = dtDepartment.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("DPT_TEXT")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Auto Fill DepartmentAuto
        /// </summary>
        /// <param name="conPK"></param>
        /// <param name="searchKey"></param>
        private void GetPlanningGroupAutocomplete(string searchKey, string SearchBy)
        {
            JavaScriptSerializer serializer;
            DataTable dtDepartment;
            List<AutoCompleteBO> result;

            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtDepartment = BusinessLogic.Inventory.ProductsBL.GetPlanningGroupAutocomplete(searchKey, SearchBy);
                if (SearchBy == "NAME")
                {
                    result = dtDepartment.AsEnumerable().Select(row => new AutoCompleteBO()
                    {
                        Key = row.Field<Int32>("PIG_PK"),

                        Name = row.Field<string>("PIG_NAME")

                    }).ToList();
                    var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                    response.ClearContent();
                    response.Write(serializer.Serialize(searchResult));
                }
                else if (SearchBy == "CODE")
                {
                    result = dtDepartment.AsEnumerable().Select(row => new AutoCompleteBO()
                    {
                        Key = row.Field<Int32>("PIG_PK"),

                        Name = row.Field<string>("PIG_CODE")

                    }).ToList();
                    var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                    response.ClearContent();
                    response.Write(serializer.Serialize(searchResult));
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetAutoBranchLocation(int conPK, string searchKey, int? IsBranchByUser)
        {
            JavaScriptSerializer serializer;
            DataTable dtBranchLocation;
            List<AutoCompleteComboBoxBO> result;
            int? UserPk = null;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                if (IsBranchByUser.HasValue && IsBranchByUser == 1)
                    UserPk = currentUser.PKUser;
                dtBranchLocation = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoFillBranchLocation(branchLocationPk, searchValue, UserPk);
                result = dtBranchLocation.AsEnumerable().Select(row => new AutoCompleteComboBoxBO()
                {
                    Key = row.Field<Int32>("CON_PK"),
                    Name = row.Field<string>("CON_NAME"),
                    Code = row.Field<string>("CON_CODE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteComboBoxList(result, "Name", "Key", "Code");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }








        /// <summary>
        /// Auto Fill Country
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetTransactionNo(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtTransactionNo;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                //string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtTransactionNo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetTransactionNo(searchKey);
                result = dtTransactionNo.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("EAR_PK"),

                    Name = row.Field<string>("EAR_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        private void GetPoWoNumbers(string searchKey, int? EnableWO)
        {
            JavaScriptSerializer serializer;
            // HttpRequest Request = context.Request;
            DataTable dtTransactionNo;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                int BizUnit = Convert.ToInt16(currentUser.SBUID);
                //  int enblwo = ;
                dtTransactionNo = BusinessLogic.POInvoicing.POInvoiceBL.GetPoWoNumbers(searchValue, EnableWO, BizUnit);
                //dtTransactionNo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetTransactionNo(searchKey);
                result = dtTransactionNo.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>(Resources.DataFieldRes.PurchaseOrderPk),

                    Name = row.Field<string>(Resources.DataFieldRes.PONumber)


                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }




        /// <summary>
        /// Auto Fill Country
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoCountry(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtCountry;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtCountry = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoCountry(searchValue);
                result = dtCountry.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CNT_PK"),

                    Name = row.Field<string>("CNT_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Auto Fill State
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoState(string searchKey, int countryPk)
        {
            JavaScriptSerializer serializer;
            DataTable dtState;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtState = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoState(searchValue, countryPk);
                result = dtState.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),

                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetEmployeeReportTo(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtEmployeeReportTo;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtEmployeeReportTo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetAutoFillEmployee(searchValue);
                result = dtEmployeeReportTo.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("empPK"),

                    Name = row.Field<string>("empName")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// for get employee auto with filter employment type and branch
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetEmployeeAuto(string searchKey, string empBranchLoc, string empType)
        {
            JavaScriptSerializer serializer;
            DataTable dtEmployeeReportTo;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtEmployeeReportTo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeAuto(searchValue, empBranchLoc, empType);
                result = dtEmployeeReportTo.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("empPK"),

                    Name = row.Field<string>("empName")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// For auto Search Depreciation No
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetDepreciationNo(string searchKey)
        {
            List<AutoCompleteBO> DeprNo;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DeprNo = BusinessLogic.Finance.DepreciationBL.GetDepreciationNoAuto(searchValue);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(DeprNo, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetAssetDisposalNoAuto(string searchKey)
        {
            List<AutoCompleteBO> DisposalNo;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DisposalNo = BusinessLogic.Finance.DepreciationBL.GetAssetDisposalNoAuto(searchValue);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(DisposalNo, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetAllGloveItems()
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                int buzUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                var lstOrders = CommonBL.GetAllGloveItems(buzUnit);
                result = lstOrders.Select(row => new AutoCompleteBO()
                {
                    Key = row.ProductPK,
                    Name = row.ProductName
                }).OrderBy(c => c.Name).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        /// <summary>
        /// Method to get AssetTypes.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetAssetTypesAuto(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtAsset;
            try
            {
                int buzUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAsset = CommonBL.GetAssetTypesAuto(searchValue.Trim(), 0, 1, buzUnit);
                result = dtAsset.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int16>("atpPK"),
                    Name = row.Field<string>("atpName")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        private void GetProductGroups(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtPrdtGrp;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtPrdtGrp = CommonBL.GetProductGroups(searchValue.Trim());
                result = dtPrdtGrp.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        /// <summary>
        /// Method to get AssetTypes.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetAssetAuto(string searchKey, string asrType)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtAsset;
            try
            {
                int buzUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                int assetType = string.IsNullOrEmpty(asrType) ? 0 : Convert.ToInt32(asrType);
                dtAsset = CommonBL.GetAssetAuto(searchValue.Trim(), assetType, 0, 1, buzUnit);
                result = dtAsset.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("asrPK"),
                    Name = row.Field<string>("asrName")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }

        /// <summary>
        /// Method to get AssetTypes.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GETASSETITEMSAUTO(string searchKey, string asrType)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtAsset;
            try
            {
                int buzUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAsset = CommonBL.GetAssetItemsAuto(searchValue.Trim(), 0, 0, 1, buzUnit);
                result = dtAsset.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ATM_PK"),
                    Name = row.Field<string>("ATM_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }

        /// <summary>
        /// For auto Search Asset Service Request
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETASSETSERVICEREQUESTNOAUTO(string searchKey, string fieldName)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> stockAdmissionRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                stockAdmissionRequest = BusinessLogic.AssetService.ServiceRequestBL.GetAssetServiceRequestNoAutocomplete(fieldName, searchValue, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(stockAdmissionRequest, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// For auto Search Asset Service Order
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETASSETSERVICEORDERNOAUTO(string searchKey, string fieldName)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> stockAdmissionRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                stockAdmissionRequest = BusinessLogic.AssetService.ServiceOrderBL.GetAssetServiceOrderNoAutocomplete(fieldName, searchValue, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(stockAdmissionRequest, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// For auto Search Asset Service Order Receipt
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETASSETSERVICERECEIPTNOAUTO(string searchKey, string fieldName)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> stockAdmissionRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                stockAdmissionRequest = BusinessLogic.AssetService.ServiceOrderReceiptBL.GetAssetServiceOrderReceiptNoAutocomplete(fieldName, searchValue, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(stockAdmissionRequest, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// For auto Search Direct Delivery Order Number
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETDIRECTDONOAUTO(string searchKey, string fieldName, string pagURL)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> directDO;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                directDO = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetDirectDONoAutocomplete(fieldName, searchValue, objUser, pagURL);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(directDO, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// For auto Search Pending Direct Sale Order Item/SO Auto
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETDIRECTSOPENDINGAUTO(string searchKey, string fieldName, string cusPK)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> directDO;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int dphPk = 0;
                directDO = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetDirectSOPendingAutoComplete(fieldName, searchValue, objUser, cusPK, dphPk);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(directDO, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetMappedItemvendor(string searchKey, string searchField, string CategoryPk, string VendorPk)
        {
            JavaScriptSerializer serializer;
            DataTable dtData;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtData = BusinessLogic.CommonManagement.CommonBL.GetMappedItemvendor(searchKey, searchField, objUser.CurrentSBUPK, CategoryPk, VendorPk);
                result = dtData.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Users Name
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETUSERSAUTO(string searchKey, string fieldName, int? isSysUser = null)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            DataTable dtUserNames;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtUserNames = CommonBL.GetUsersAuto(searchValue.Trim(), fieldName, isSysUser);
                result = dtUserNames.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        #region Material Issue
        /// <summary>
        /// Method to get Item category.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetItemCategory(string searchKey, string filterType)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtItemCategory;
            try
            {
                int bizUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtItemCategory = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetItemCategory(bizUnit, Convert.ToInt32(filterType), currentUser.CurrentDeptPK, searchValue);
                result = dtItemCategory.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ITC_PK"),
                    Name = row.Field<string>("ITC_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        /// <summary>
        /// Method to get Item category.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetMaterialIssueNo(string searchKey)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtItemCategory;
            try
            {
                int bizUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtItemCategory = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetSearchValues("ICH_NO", searchValue, currentUser.SBUID, objUser, 1, 5);
                result = dtItemCategory.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        /// <summary>
        /// Method to get Item category code.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="categoryPK"></param>
        private void GetItemCode(string searchKey, string filterType)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtItemCode;
            try
            {
                int bizUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtItemCode = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetItemCode(searchValue, Convert.ToInt32(filterType), currentUser.CurrentDeptPK);
                result = dtItemCode.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }

        /// <summary>
        /// Method to get Batch No Autocomplete
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="categoryPK"></param>
        private void GetBatchNo(string searchKey, int materialID, string excDate, int batchPK)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtItemCode;
            int IsShowZeroQtyBatches = 0;
            int CDHPk = 0;
            try
            {
                int bizUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtItemCode = BusinessLogic.MaterialManagement.MaterialMaster.GetBatchNoAuto(materialID, currentUser.CurrentDeptPK, batchPK, Convert.ToDateTime(excDate), 0, Convert.ToDateTime(excDate), 0);
                result = dtItemCode.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("SBD_PK"),
                    Name = row.Field<string>("SBD_BATCH_NO")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "LongKey");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        /// <summary>
        /// Method to get Item Name.
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="categoryPK"></param>
        private void GetItemName(string searchKey, string filterType, int? IssueAgainst = null)
        {
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtItemName;
            try
            {
                int bizUnit = currentUser.SBUID;
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtItemName = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetItemName(searchValue, currentUser.SBUID, string.IsNullOrEmpty(filterType) || filterType == "-1" ? 0 : Convert.ToInt32(filterType), Convert.ToInt32(IssueAgainst));
                result = dtItemName.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ICH_ISS_RCV_PK"),
                    Name = row.Field<string>("ICH_ISS_RCV_TEXT")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }
        #endregion

        #region ESS
        /// <summary>
        /// Get Auto ESS EmployeeList 
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetAutoCompleteEmployeeESS(string searchKey, string toDate = null)
        {
            JavaScriptSerializer serializer;
            DataTable dtEmployees;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtEmployees = BusinessLogic.HRMS.ESS.EmpLeaveRequestBL.GetAutoCompleteEmployeeESS(searchValue, (int)DbActiveStatus.ACTIVE, currentUser.PKEmployee, toDate);

                result = dtEmployees.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("empPK"),
                    Name = row.Field<string>("emptext")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        #endregion

        /// <summary>
        /// Get Direct(Trading) invoice number auto
        /// </summary>
        /// <param name="searchKey"></param>
        private void GETDIRECTINVOICENOAUTO(string searchKey, int? InvCategory, int? InvGroup)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> autoList;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                autoList = BusinessLogic.POInvoicing.POInvoiceBL.GetDirectInvoiceNoAutoComplete(searchValue, objUser, InvCategory, InvGroup);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(autoList, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get Process Fill
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetFillProcess(string WtType, string searchKey, int sbu = 0)
        {
            JavaScriptSerializer serializer;
            DataTable dtfillProcess;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtfillProcess = BusinessLogic.CommonManagement.CommonBL.GetProcessList(currentUser.PKUser, Convert.ToInt32(WtType), searchKey, sbu);
                result = dtfillProcess.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),

                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get Process Fill
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetFillDept(string WtType, string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtfillProcess;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtfillProcess = BusinessLogic.CommonManagement.CommonBL.GetMenuDepartment(currentUser.PKUser, Convert.ToInt32(WtType), searchKey);
                result = dtfillProcess.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("DPT_PK"),

                    Name = row.Field<string>("DPT_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// For auto Search Fund Requisition No
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETFUNDREQUISITIONNOAUTO(string searchKey, string fieldName)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> stockAdmissionRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                stockAdmissionRequest = BusinessLogic.Administration.Masters.FundRequisitionDeptBL.GetFundRequisitionNoAutocomplete(fieldName, searchValue, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(stockAdmissionRequest, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        /// <summary>
        /// For auto Search Fund Requisition No
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GETAGENTLIST(string searchKey, string fieldName)
        {
            JavaScriptSerializer serializer;
            DataTable dtAgentList;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAgentList = BusinessLogic.CommonManagement.CommonBL.GETAGENTLIST(currentUser.SBUID);
                result = dtAgentList.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("VEN_PK"),

                    Name = row.Field<string>("VEN_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetBudgetNo(string searchKey)
        {
            JavaScriptSerializer serializer;
            DataTable dtBudget;
            List<AutoCompleteBO> result;
            try
            {
                serializer = new JavaScriptSerializer();
                searchKey = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtBudget = BusinessLogic.Finance.BudgetBL.GetBudgetNo(searchKey);
                result = dtBudget.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }

        }


        #region AutoEnum
        /// <summary>
        /// Auto Complete Enum
        /// </summary>
        private enum AutoEnum
        {
            VENDOR,
            VENDORNAMECODE,
            COSTCENTER,
            BUDGETCOSTCENTER,
            COSTCENTERWITHOUTGRP,
            TRANSPORTER,
            PARTY,
            PONUMBER,
            POWONUMBER,
            PURCHASEPAYMENTNUMBER,
            INVOICENUMBER,
            ADVINVNUMBER,
            INVNUMBER,
            EXPENSEINVNUMBER,
            EXPENSESETTILEMENTINVNUMBER,
            SONUMBER,
            SONUMBERWKF,
            CUSPONUMBER,
            CUSPONUMBERCUSTOMERWISE,
            IONUMBER,
            SONUMBERAPPROVED,
            BANK,
            BANKBYTYPE,
            VENDORACCOUNT,
            SALESRECEIPTNUMBER,
            CUSTOMER,
            ADVSALINVOICENUMBER,
            SALINVOICENUMBER,
            FCVOUCHERNUMBER,
            CUSTOMERLIST,
            CURRENCY,
            CURRENCYCODE,
            VENDORCURRENCY,
            ACCOUNT,
            CRDRNUMBER,
            CRDRNUMBERSAL,
            CRDRNUMBERPUR,
            SUBACCOUNTS,
            CRMCUSTOMER,
            DELIVERYORDERNUMBER,
            REPORTGROUP,
            REPORT,
            PRODUCTNATURE,
            PRODUCTMASTER,
            COUNTRY,
            MESSAGECOUNT,
            VENDOR_ROLE,
            JOURNALACCOUNT,
            ACCOUNTMST,
            ITEMCATEGORY,
            ITEMCODE,
            BATCHNO,
            ITEMNAME,
            MAILTYPE,
            UOM,
            WHTACCOUNTS,
            VATBUYACCOUNTS,
            VENDORCONTACTTYPES,
            VENDORCONTACTS,
            VATSALEEXPORTCUSTOMER,
            VATSALEEXPORTINVOICE,
            NATIONALITY,
            PROFESSION,
            RELIGION,
            SUBRELIGION,
            COUNTRYBIRTH,
            COUNTRYFIRST,
            TEAM,
            STATEAUTO,
            COUNTRYSECOND,
            BRANCHLOCATION,
            TRANSACTIONNO,
            DEPARTMENT,
            DEPARTMENTAUTOCOMPLETE,
            EMPLOYEEREPORTTO,
            ASSIGNTASKUSER,
            EMPLOYEEAUTOCOMPLETE,
            EMPLOYEEPAYROLLAUTOCOMPLETE,
            EXITREASON,
            GETAUTOAGT,
            GETAUTOAGTCOMM,
            DESIGNATION,
            DESIGNATIONBYJOB,
            GETALLITEMS,
            GETALLITEMSWITHCODENAME,
            DEPRECIATIONAUTOGET,
            EMPLOYEEAUTO,
            EXECUTEQUERY,
            PAYELEMENT,
            GETPRODUCTS,
            GETBRANDPRODUCTS,
            EMPLOYEEAUTOCOMPLETEBYFILTER,
            GETGLOVEITEMS,
            ADDDEDNUMBER,
            SALPYMTNUMBER,
            EOTNUMBER,
            CUSTOMERVENDORLIST,
            EDOCEMPLOYEES,
            GETASSETTYPEAUTO,
            GETASSETSAUTO,
            GETASSETITEMSAUTO,
            GETASSETSERVICEREQUESTNOAUTO,
            GETASSETSERVICEORDERNOAUTO,
            GETASSETSERVICERECEIPTNOAUTO,
            EMPTRANSFERNUMBER,
            GETDIRECTDONOAUTO,
            DONUMBER,
            GETDIRECTSOPENDINGAUTO,
            GETUSERSAUTO,
            PLANNINGGROUP,
            SCNUMBER,
            ESSEMPLOYEEAUTOCOMPLETE,
            GETDIRECTINVOICENOAUTO,
            PURCHASEORDERNO,
            GETUSERSINBOXMAPPING,
            GETROLESINBOXMAPPING,
            FILLPROCESS,
            FILLDEPT,
            GETCOAPARENT,
            GETBUDGETCOAPARENT,
            GETMAPPEDITEMVENDOR,
            SALINVNUMBERTRADING,
            FILLPORTDETAILS,
            PENDINGPURCHASEINVNO,
            MATERIALISSUENO,
            GETDRCRNOTRADINGAUTO,
            GETPAYMENTNOTRADINGAUTO,
            PENDINGPURCHASEINVNOTRADING,
            GETFUNDREQUISITIONNOAUTO,
            GETRELATEDPRODUCTS,
            GETAGENTLIST,
            INVCONVERTED,
            WONUMBER,
            GETAUTOCSI,
            WOITEM,
            GETORDERPLANTEXT,
            WORKORDERITEM,
            FINYEAROPENINGNO,
            EOLNUMBER,
            MIRATEADJUSTNO,
            CWIPNO,
            ASSETDISPOSALAUTO,
            CWIPACCOUNT,
            GETPRODUCTGROUPS,
            QUOTATION,
            COMPMSTFILTER,
            DISPMSTFILTER,
            GETBUDGETNO
        }
        #endregion
        /// <summary>
        /// ProductNature Enum
        /// </summary>
        #region ProductNatureEnum
        private enum ProductNatureEnum
        {
            Type,
            Thickness,
            Category,
            Surface,
            Shade,
            Classification,
            Size,
            Length
        }

        #endregion
        #region WkfStatusEnum
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }
        #endregion
    }
}
