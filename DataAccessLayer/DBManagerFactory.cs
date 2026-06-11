using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;

using System.Data.OracleClient;
//using MySql.Data.MySqlClient;
using System.Configuration;


namespace DataAccessLayer
{
    /// <remarks>
    /// Holds the  methods to manage DB operations
    /// </remarks>
    /// <author>Pems Team</author>
    /// <createddate>07/04/2009</createddate>
public sealed class DBManagerFactory
  {
    //Connection string
     static string strConnection=string.Empty;
    /// <summary>
    /// Default constructor
    /// </summary>
    private DBManagerFactory()
    {
       strConnection= ConfigurationSettings.AppSettings["ConnectionString"];
    }

    /// <summary>
    /// Get connection 
    /// </summary>
    /// <param name="providerType"></param>
    /// <returns></returns>
    public static IDbConnection GetConnection(DataProvider providerType)
    {
      IDbConnection iDbConnection = null;
      switch (providerType)
      {
        case DataProvider.SqlServer:
          iDbConnection = new SqlConnection();
          break;
        case DataProvider.OleDb:
          iDbConnection = new OleDbConnection();
          break;
        case DataProvider.Odbc:
          iDbConnection = new OdbcConnection();
          break;
        case DataProvider.Oracle:
          iDbConnection = new OracleConnection();
          break;
        ////Mysql
        //case DataProvider.MySql:
        //  iDbConnection = new MySqlConnection();
          break;
        default:
          return null;
      }
      return iDbConnection;
    }  

    /// <summary>
    /// Get Coppmmand
    /// </summary>
    /// <param name="providerType"></param>
    /// <returns></returns>
    public static IDbCommand GetCommand(DataProvider providerType)
    {
      switch (providerType)
      {
        case DataProvider.SqlServer:
          return new SqlCommand();
        case DataProvider.OleDb:
          return new OleDbCommand();
        case DataProvider.Odbc:
          return new OdbcCommand();
        case DataProvider.Oracle:
          return new OracleCommand();
        //Mysql
        //case DataProvider.MySql:
        //  return new MySqlCommand();
        default:
          return null;
      }
    } 

    /// <summary>
    /// Get data adapter
    /// </summary>
    /// <param name="providerType"></param>
    /// <returns></returns>
    public static IDbDataAdapter GetDataAdapter(DataProvider providerType)
    {
      switch (providerType)
      {
        case DataProvider.SqlServer:
          return new SqlDataAdapter();
        case DataProvider.OleDb:
          return new OleDbDataAdapter();
        case DataProvider.Odbc:
          return new OdbcDataAdapter();
        case DataProvider.Oracle:
          return new OracleDataAdapter();
        //Mysql
        //case DataProvider.MySql:
        //  return new MySqlDataAdapter();
        default:
          return null;
      }
    } 

   /* public static IDbTransaction GetTransaction(DataProvider providerType)
    {
      IDbConnection iDbConnection =GetConnection(providerType);
    //////
    ////  idbConnection = DBManagerFactory.GetConnection(this.providerType);
    //  iDbConnection.ConnectionString = strConnection;
    //    if (iDbConnection.State != ConnectionState.Open)
    //      iDbConnection.Open();

    //////
     
      IDbTransaction iDbTransaction = iDbConnection.BeginTransaction();
      return iDbTransaction;
    }*/
    /// <summary>
    /// Get transaction 
    /// </summary>
    /// <param name="iDbConnection"></param>
    /// <returns></returns>
    public static IDbTransaction GetTransaction(IDbConnection iDbConnection)
    {
        IDbTransaction iDbTransaction = iDbConnection.BeginTransaction();
        return iDbTransaction;
    
    }

    /// <summary>
    /// Get Parameters
    /// </summary>
    /// <param name="providerType"></param>
    /// <returns></returns>
    public static IDataParameter GetParameter(DataProvider providerType)
    {
      IDataParameter iDataParameter = null;
      switch (providerType)
      {
        case DataProvider.SqlServer:
          iDataParameter = new SqlParameter();
          break;
        case DataProvider.OleDb:
          iDataParameter = new OleDbParameter();
          break;
        case DataProvider.Odbc:
          iDataParameter = new OdbcParameter();
          break;
        case DataProvider.Oracle:
          iDataParameter = new OracleParameter();
          break;
        //Mysql
        //case DataProvider.MySql:
        //iDataParameter = new MySqlParameter();
        break;
      }
      return iDataParameter;
    } 

    /// <summary>
    /// Get Paramter Array
    /// </summary>
    /// <param name="providerType"></param>
    /// <param name="paramsCount"></param>
    /// <returns></returns>
    public static IDbDataParameter[]GetParameters(DataProvider providerType,int paramsCount)
    {
      IDbDataParameter[]idbParams = new IDbDataParameter[paramsCount];
 
      switch (providerType)
      {
        case DataProvider.SqlServer:
          for (int i = 0; i < paramsCount;++i)
          {
            idbParams[i] = new SqlParameter();
          }
          break;
        case DataProvider.OleDb:
          for (int i = 0; i < paramsCount;++i)
          {
            idbParams[i] = new OleDbParameter();
          }
          break;
        case DataProvider.Odbc:
          for (int i = 0; i < paramsCount;++i)
          {
            idbParams[i] = new OdbcParameter();
          }
          break;
        case DataProvider.Oracle:
          for (int i = 0; i < paramsCount; ++i) //intParamsLength
          {
              idbParams[i] = new OracleParameter();
          }
          break;
        //Mysql
        //case DataProvider.MySql:
        //  for (int i = 0; i < paramsCount; ++i) //intParamsLength
        //  {
        //      idbParams[i] = new MySqlParameter();
        //  }
        //  break;
        default:
          idbParams = null;
          break;
      }
      return idbParams;
    }
  }

}
