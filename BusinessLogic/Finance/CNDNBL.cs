using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;

namespace BusinessLogic.Finance
{
    public class CNDNBL
    {
        public static DataSet GetCNDNDetails(int RecPK)
        {
            return CNDNDL.GetCNDNDetails(RecPK);
        }

        /// <summary>
        /// Delete Attachment Documents
        /// </summary>
        /// <param name="docPk"></param>
        /// <returns>int</returns>
        public static int DeleteAttachmentDocuments(int docPk, int? docTask, int? docTaskId)
        {
            return CNDNDL.DeleteAttachmentDocuments(docPk, docTask, docTaskId);
        }
    }
}
