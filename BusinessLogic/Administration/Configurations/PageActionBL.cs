using System;
using System.Data;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using DataAccess.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class PageActionBL
    {
        public static DataSet GetPageActions(int bizUnit)
        {
            return PageActionDA.GetPageActions(bizUnit);
        }

        public static DataSet GetPageActions(int pageActionPK, DbActiveStatus status, int bizUnit)
        {
            return PageActionDA.GetPageActions(pageActionPK, status, bizUnit);
        }

        public static DataTable GetPages(int pagePK, DbActiveStatus status)
        {
            return PageActionDA.GetPages(pagePK, status);
        }
        public static DataTable GetPages(int? PagePK, int? Status, string PageUrl)
        {
            return PageActionDA.GetPages(PagePK, Status, PageUrl);
        }
        public static int SavePageAction(PageActionBO pageAct)
        {
            return PageActionDA.SavePageAction(pageAct);
        }

        public static int DeletePageAction(int currPK, DateTime lastModBY)
        {
            return PageActionDA.DeletePageAction(currPK, lastModBY);
        }

        public static DataSet GetPageAction(int currPK, DbActiveStatus status, int bizunit)
        {
            return PageActionDA.GetPageAction(currPK, status, bizunit);
        }
        
    }
}
