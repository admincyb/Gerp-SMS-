using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.OrderToCash;

namespace BusinessLogic.OrderToCash
{
    public class BrandCopyBL
    {
        public static int? SaveBrandCopyDetails(string strxml, out string TrxNo)
        {
            try
            {
                return BrandCopyDL.SaveBrandCopyDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }
    }
}
