using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;

namespace BusinessLogic.Sales
{
    public class Enquiry
    {
        public static int SaveEnquiryDetails(string saveXml)
        {
            return DataAccess.SaleOrder.EnquiryDL.SaveEnquiryDetails(saveXml);
        }
        public static DataSet GetEnquiryList(GridPrams grid, User objUser, int procID, int userPK, short status, int enqPK, int cusPK, string pageURL)
        {
            return DataAccess.SaleOrder.EnquiryDL.GetEnquiryList(grid, objUser, procID, userPK, status, enqPK, cusPK, pageURL);
        }
        public static DataSet GetEnquiryNoAuto(string field, string name, int bizUnit, int userPK)
        {
            return DataAccess.SaleOrder.EnquiryDL.GetEnquiryNoAuto(field, name, bizUnit, userPK);
        }
        public static DataSet GetEnquiryDetails(int enqPK, int custPK)
        {
            return DataAccess.SaleOrder.EnquiryDL.GetEnquiryDetails(enqPK, custPK);
        }
        public static int DeleteRFQDetails(int enqPK, DateTime lastModDate)
        {
            return DataAccess.SaleOrder.EnquiryDL.DeleteEnquiryDetails(enqPK, lastModDate);
        }
        public static DataTable GetBankDetails(int bankPK, int active, int bizunit, int bankType = 0,int fcHold=0)
        {
            return DataAccess.SaleOrder.EnquiryDL.GetBankDetails(bankPK, active, bizunit, bankType,fcHold);
        }
    }
}
