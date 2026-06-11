using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;


namespace DataAccess.ProductionDL
{
    public class AllocationDL
    {
        /// <summary>
        /// method to get saleorder details
        /// </summary>
        /// <param name="pk"></param>
        public static DataSet GetAllocation(int wid, string[] XML)
        {
              DataSet ds = null;
              //if (DBService.PROVIDER == ProviderName.OracleClient)
              //{
              //    DBServiceOracle dbService = new DBServiceOracle();
              //    DBServiceOracle.ParametersOracle[] colParameters = null;
              //    colParameters = new DBServiceOracle.ParametersOracle[] 
              //    {
              //        new DBServiceOracle.ParametersOracle(AllocationDA.P_WID, wid,ParameterDirection.Input,DBServiceOracle.ParameterType.Int32),
              //        new DBServiceOracle.ParametersOracle(AllocationDA.P_XML_PRO_LST,  XML[1]==null? EmptyXML.val:XML[1],ParameterDirection.Input,DBServiceOracle.ParameterType.XmlType),
              //        new DBServiceOracle.ParametersOracle(AllocationDA.P_XML_ORD_LST,  XML[0]==null? EmptyXML.val:XML[0],ParameterDirection.Input,DBServiceOracle.ParameterType.XmlType),
              //        new DBServiceOracle.ParametersOracle(AllocationDA.P_XML_ALC_LST,  XML[2]==null? EmptyXML.val:XML[2],ParameterDirection.Input,DBServiceOracle.ParameterType.XmlType),
              //        new DBServiceOracle.ParametersOracle(RetVal.Cursor1,null,ParameterDirection.Output,DBServiceOracle.ParameterType.RefCursor),
              //        new DBServiceOracle.ParametersOracle(RetVal.Cursor2,null,ParameterDirection.Output,DBServiceOracle.ParameterType.RefCursor)
                     
              //    };
              //    ds = dbService.DataAdapterOracle(CommandType.StoredProcedure, AllocationDA.SP_GetAllocation, colParameters);
              //}
              //else
              {
                  DBService dbService = new DBService();
                  DBService.Parameters[] colParameters = null;
                  colParameters = new DBService.Parameters[] 
                  { 
                      new DBService.Parameters(AllocationDA.P_WID, wid),
                      new DBService.Parameters(AllocationDA.P_XML_ORD_LST, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                      new DBService.Parameters(AllocationDA.P_XML_PRO_LST, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),
                      new DBService.Parameters(AllocationDA.P_XML_ALC_LST, XML[2]==string.Empty? DBNull.Value.ToString():XML[2])
                  };
                  ds=dbService.DataAdapter(CommandType.StoredProcedure, AllocationDA.SP_GetAllocation, colParameters);
              }
              return ds;

        }

        /// <summary>
        /// Save Sale Order
        /// </summary>
        /// <param name="strxml"></param>
        public static DataSet SaveSaleorder(int wid, string strxml)
        {
             DataSet ds = null;
             //if (DBService.PROVIDER == ProviderName.OracleClient)
             //{ 
             //     DBServiceOracle dbService = new DBServiceOracle();
             //     DBServiceOracle.ParametersOracle[] colParameters = null;
             //     colParameters = new DBServiceOracle.ParametersOracle[] 
             //     {
             //         new DBServiceOracle.ParametersOracle(AllocationDA.P_WID, wid,ParameterDirection.Input,DBServiceOracle.ParameterType.Int32),
             //         new DBServiceOracle.ParametersOracle(AllocationDA.P_XML_ALC_DTL,  strxml==string.Empty? DBNull.Value.ToString():strxml,ParameterDirection.Input,DBServiceOracle.ParameterType.XmlType)
             //     };
             //     ds = dbService.DataAdapterOracle(CommandType.StoredProcedure, AllocationDA.SP_SaveAllocation, colParameters);
             //}
             //else
             {
                 DBService dbService = new DBService();
                 DBService.Parameters[] colParameters = null;
                 colParameters = new DBService.Parameters[] 
                 {                
                     new DBService.Parameters(AllocationDA.P_WID, wid),
                     new DBService.Parameters(AllocationDA.P_XML_ALC_DTL, strxml),
                 };
                 ds=dbService.DataAdapter(CommandType.StoredProcedure, AllocationDA.SP_SaveAllocation, colParameters);
             }
             return ds;
           
        }

    }
}