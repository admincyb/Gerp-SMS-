using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using System.Data;
using GTIService.Constants.Common;
using GTIService.Constants.Administration.Configurations;
using BusinessObject;

namespace DataAccess.Administration.Configurations
{
    public class CompanyDA
    {
        #region Methods
        /// <summary>
        /// To save company details
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveCompany(CompanyBO objCompany, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPK,objCompany.PK),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPCODE,objCompany.CompanyCode),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPNAME,objCompany.CompanyName),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPDESC,objCompany.Description),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPADDR1,objCompany.Address1),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPADDR2,objCompany.Address2),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPNAME2,objCompany.CompanyNameLocal),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPADDR3,objCompany.Address3),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPCITY,objCompany.City),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPPHONE,objCompany.Phone),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPMOBIL,objCompany.Mobile),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPFAX,objCompany.Fax),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPEMAIL,objCompany.Email),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPSTATE,objCompany.State),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPCNTRY,objCompany.Country),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPCURRENCY,objCompany.Currency),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPTAXNO,objCompany.TaxNo),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPLOGO,objCompany.Logo),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPLOGOURL,objCompany.LogoURL),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PUSERPK,objUser.PKUser),
                //new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PBIZUNIT,objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPWEBSITE,objCompany.Website),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPZIP,objCompany.ZipCode),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPREGNO,objCompany.BisRegNo),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPGSTNO,objCompany.GSTNo),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPFINSTARTDT,objCompany.FinYearStartDate),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPFINENDDT,objCompany.FinYearEndDate),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPBIZUNIT,objCompany.BizUnit > 0 ? objCompany.BizUnit : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPOPLOGO,objCompany.OutputLogo),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPOPLOGOURL,objCompany.OutputLogoURL),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_DISPLAY_CODE,objCompany.DisplayCode == string.Empty ? (Object)DBNull.Value:objCompany.DisplayCode),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_DISPLAY_NAME,objCompany.DisplayName == string.Empty ? (Object)DBNull.Value:objCompany.DisplayName),
                //Config settings
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_PCS_OR_WGT,objCompany.ProductionIn > 0 ? objCompany.ProductionIn : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_NO_OF_PCS_PER_BSKT,objCompany.NoOfPcsPerBskt > 0 ? objCompany.NoOfPcsPerBskt : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_WGT_PER_BSKT,objCompany.WeightPerBskt > 0 ? objCompany.WeightPerBskt : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_NO_OF_BSKT,objCompany.NoOfBasket > 0 ? objCompany.NoOfBasket : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_PRDCTION_REQD_TM,objCompany.TimeReqToPrd > 0 ? objCompany.TimeReqToPrd : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_PRINT_LABEL,objCompany.PrintLabel >= 0 ? objCompany.PrintLabel : (object)DBNull.Value),
                //QA Sample
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_MEDICAL,objCompany.Medical >= 0 ? objCompany.Medical : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.P_CMP_NON_MEDICAL,objCompany.NonMedical >= 0 ? objCompany.NonMedical : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Company.Procedures.SPADM_COMPANY_MST_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Company.Parameters.PRETVAL]).Value);
        }

        /// <summary>
        /// To retrieve company details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetCompanyList(int CompanyPK, int Active, int BizUnit)
        {
            DataTable dtCompany;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PCMPPK,CompanyPK),
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PACTIVE,Active),
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PBIZUNIT, BizUnit),
            };
            dtCompany = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.CompanyList.Procedure.SPADM_COMPANY_MST_SAVE, colParameters).Tables[0];
            return dtCompany;
        }

        public static DataTable GetCompanyDetails(int CompanyPK, int Active,string cmpName,int bizUnit,string specialCondition)
        {
            DataTable dtCompany;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PCMPPK,CompanyPK>0 ? CompanyPK :(Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PACTIVE,Active > 0 ? Active : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PCMPNAME,string.IsNullOrEmpty(cmpName) ? (Object)DBNull.Value :cmpName),
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PBIZUNIT,bizUnit > 0 ? bizUnit : (Object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.SPLCONDITION,string.IsNullOrEmpty(specialCondition) ? (Object)DBNull.Value :specialCondition),
            };
            dtCompany = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.CompanyList.Procedure.SPADM_COMPANY_MST_SAVE, colParameters).Tables[0];
            return dtCompany;
        }


        /// <summary>
        /// To Delete company details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static int DeleteCompany(int CompanyPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PCMPK,CompanyPK),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Company.Procedures.SPADM_COMPANY_MST_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.Company.Parameters.PRETVAL]).Value);
        }



        /// <summary>
        /// To Show Product Name, Version, GAF at the footer
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetVersionDetails()
        {
            DataTable dtVersion;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Configurations.CompanyList.Parameters.PSYSPK,DBNull.Value)               
            };
            dtVersion = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.CompanyList.Procedure.SPADM_SYSTEM_CFG_GET, colParameters).Tables[0];
            return dtVersion;
        }


        #endregion
    }
}
