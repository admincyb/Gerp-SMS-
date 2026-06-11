using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;
//using MySql.Data.MySqlClient;
using System.Data.OracleClient;

namespace DataAccessLayer
{
    /// <remarks>
    /// Holds the  methods to manage DB operations
    /// </remarks>
    /// <author>Pems Team</author>
    /// <createddate>07/04/2009</createddate>
   public  interface IDBManager
    {
       /// <summary>
        ///  ProviderType
       /// </summary>
        DataProvider ProviderType
        {
          get;
          set;
        }     

       /// <summary>
        /// ConnectionString
       /// </summary>
        string ConnectionString
        {
          get;
          set;
        }     

       /// <summary>
        /// Connection
       /// </summary>
        IDbConnection Connection
        {
          get;
        }

       /// <summary>
        /// Transaction
       /// </summary>
        IDbTransaction Transaction
        {
          get;
        }     

       /// <summary>
        /// DataReader
       /// </summary>
        IDataReader DataReader
        {
          get;
        }

       /// <summary>
        /// Command
       /// </summary>
        IDbCommand Command
        {
          get;
        }     

       /// <summary>
        /// Parameters
       /// </summary>
        IDbDataParameter[]Parameters
        {
          get;
        }     

       /// <summary>
       /// Open Connection 
       /// </summary>
        void Open();

       /// <summary>
        /// BeginTransaction
       /// </summary>
        void BeginTransaction();

       /// <summary>
        /// CommitTransaction
       /// </summary>
        void CommitTransaction();

       /// <summary>
        /// RollbackTransaction
       /// </summary>
        void RollbackTransaction();

       /// <summary>
        /// CreateParameters
       /// </summary>
       /// <param name="paramsCount"></param>
        void CreateParameters(int paramsCount);

       /// <summary>
        /// AddParameters
       /// </summary>
       /// <param name="index"></param>
       /// <param name="paramName"></param>
       /// <param name="objValue"></param>
        void AddParameters(int index, string paramName, object objValue);

       /// <summary>
        /// ExecuteReader
       /// </summary>
       /// <param name="commandType"></param>
       /// <param name="commandText"></param>
       /// <returns></returns>
        IDataReader ExecuteReader(CommandType commandType, string commandText);

       /// <summary>
        /// ExecuteDataSet
       /// </summary>
       /// <param name="commandType"></param>
       /// <param name="commandText"></param>
       /// <returns></returns>
        DataSet ExecuteDataSet(CommandType commandType, string commandText);

       /// <summary>
        /// ExecuteScalar
       /// </summary>
       /// <param name="commandType"></param>
       /// <param name="commandText"></param>
       /// <returns></returns>
        object ExecuteScalar(CommandType commandType, string commandText);

       /// <summary>
        /// ExecuteNonQuery
       /// </summary>
       /// <param name="commandType"></param>
       /// <param name="commandText"></param>
       /// <returns></returns>
        int ExecuteNonQuery(CommandType commandType,string commandText);

       /// <summary>
        /// CloseReader
       /// </summary>
        void CloseReader();

       /// <summary>
       /// Close connection 
       /// </summary>
        void Close();

       /// <summary>
       /// Dispose Objects
       /// </summary>
        void Dispose();

  }

    
}
