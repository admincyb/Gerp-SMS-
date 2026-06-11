using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
    public class RelatedLinkBL
    {
        /// <summary>
        /// method for Get Pages
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPages(int pageID)
        {
            return RelatedLinkDA.GetPages(pageID);
        }

        /// <summary>
        /// method for Get Page List
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetRelatedPageList(int pageID)
        {
            return RelatedLinkDA.GetRelatedPageList(pageID);
        }

        /// <summary>
        /// method used to Save Related Links and return PK
        /// </summary>
        /// <param name="Xml" Type=object></param>
        /// <returns>int</returns>
        public static int SaveRelatedLinks(string Xml)
        {
            return RelatedLinkDA.SaveRelatedLinks(Xml);
        }

        /// <summary>
        /// method for Get User Related Pages according User Rights.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetUserRelatedPages(int userID, int pageID, string absolutePath)
        {
            return RelatedLinkDA.GetUserRelatedPages(userID, pageID, absolutePath);
        }
    }
}
