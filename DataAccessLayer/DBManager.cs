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
using GTIService;



namespace DataAccessLayer
{
    /// <remarks>
    /// Holds the  methods to manage DB operations
    /// </remarks>
    /// <author>Pems Team</author>
    /// <createddate>07/04/2009</createddate>
    
   public sealed class DBManager: IDBManager,IDisposable
  {    
    //Connection 
    private IDbConnection idbConnection;
    //Data reader
    private IDataReader idataReader;
    //Command
    private IDbCommand idbCommand;
    //Provider Type
    private DataProvider providerType;
    //Transaction
    private IDbTransaction idbTransaction =null;
    //Parameters
    private IDbDataParameter[]idbParameters =null;
    //Connection String
    private string strConnection;
       //Mode Of Operation
    private string OperationMode;

    /// <summary>
    /// Default Constructor
    /// </summary>
    public DBManager()
    {
        this.providerType  =(DataProvider)Enum.Parse(typeof(DataProvider),ConfigurationSettings.AppSettings["Provider"]);
        this.strConnection = ConfigurationSettings.AppSettings["ConnectionString"];
        this.OperationMode = ConfigurationSettings.AppSettings["OperationMode"].ToUpper();    
    } 

    /// <summary>
    /// Connection provider
    /// </summary>
    /// <param name="providerType"></param>
    public DBManager(DataProvider providerType)
    {
      this.providerType = providerType;
    } 

    /// <summary>
    /// OverLoaded Constructoor to hold provider & connection string
    /// </summary>
    /// <param name="providerType"></param>
    /// <param name="connectionString"></param>
    public DBManager(DataProvider providerType,string connectionString)
    {
      this.providerType = providerType;
      this.strConnection = connectionString;
    } 

    /// <summary>
    /// Connection
    /// </summary>
    public IDbConnection Connection
    {
      get
      {
        return idbConnection;
      }
    } 

    /// <summary>
    /// Data reder
    /// </summary>
    public IDataReader DataReader
    {
      get
      {
        return idataReader;
      }
      set
      {
        idataReader = value;
      }
    } 

    /// <summary>
    /// Provider Type
    /// </summary>
    public DataProvider ProviderType
    {
      get
      {
        return providerType;
      }
      set
      {
        providerType = value;
      }
    } 

    /// <summary>
    /// Connection string
    /// </summary>
    public string ConnectionString
    {
      get
      {
        return strConnection;
      }
      set
      {
        strConnection = value;
      }
    } 
    /// <summary>
    /// Command
    /// </summary>
    public IDbCommand Command
    {
      get
      {
        return idbCommand;
      }
    } 

    /// <summary>
    /// Trnasction 
    /// </summary>
    public IDbTransaction Transaction
    {
      get
      {
        return idbTransaction;
      }
    } 
    
    /// <summary>
    /// Get Parameter
    /// </summary>
    public IDbDataParameter[]Parameters
    {
      get
      {
        return idbParameters;
      }
    } 

    /// <summary>
    /// Open Connection 
    /// </summary>
    public void Open()
    {
      idbConnection =DBManagerFactory.GetConnection(this.providerType);
      idbConnection.ConnectionString =this.ConnectionString;
      if (idbConnection.State !=ConnectionState.Open)
        idbConnection.Open();
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      this.idbTransaction =DBManagerFactory.GetTransaction(idbConnection); 
    } 

    /// <summary>
    /// Close Connection 
    /// </summary>
    public void Close()
    {
      if (idbConnection.State !=ConnectionState.Closed)
        idbConnection.Close();
    } 

    /// <summary>
    /// Dispose Obects
    /// </summary>
    public void Dispose()
    {
      GC.SuppressFinalize(this);
      this.Close();
      this.idbCommand = null;
      this.idbTransaction = null;
      this.idbConnection = null;
    } 

    /// <summary>
    /// Set Parameters
    /// </summary>
    /// <param name="paramsCount"></param>
    public void CreateParameters(int paramsCount)
    {
      idbParameters = new IDbDataParameter[paramsCount];
      idbParameters =DBManagerFactory.GetParameters(this.ProviderType,paramsCount);
    } 

   /// <summary>
   /// Create Parameters
   /// </summary>
   /// <param name="index"></param>
   /// <param name="paramName"></param>
   /// <param name="objValue"></param>
    public void AddParameters(int index, string paramName, object objValue)
    {
      if (index < idbParameters.Length)
      {
          if (OperationMode == "RELEASE")
              idbParameters[index].ParameterName =  CommonFunctions.EncriptDB(paramName,5);
          else
              idbParameters[index].ParameterName =paramName;


        idbParameters[index].Value = objValue;
      }
    } 

   /// <summary>
   /// Start Transaction 
   /// </summary>
    public void BeginTransaction()
    {
        if (this.idbTransaction == null)
            Open();
        //this.idbTransaction =//DBManagerFactory.GetTransaction(this.ProviderType);
        //this.idbCommand.Transaction =idbTransaction;
    } 

    /// <summary>
    /// Commit transaction 
    /// </summary>
    public void CommitTransaction()
    {
      if (this.idbTransaction != null)
        this.idbTransaction.Commit();       
      idbTransaction = null;
    }

    /// <summary>
    /// Rollback transaction 
    /// </summary>
    public void RollbackTransaction()
    {
        if (this.idbTransaction != null)
            this.idbTransaction.Rollback();
        idbTransaction = null;
    } 

    /// <summary>
    /// Execute data reader
    /// </summary>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <returns></returns>
    public IDataReader ExecuteReader(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      idbCommand.Connection = this.Connection;
      PrepareCommand(idbCommand,this.Connection, this.Transaction,commandType,commandText, this.Parameters);
      this.DataReader =idbCommand.ExecuteReader();
      idbCommand.Parameters.Clear();
      return this.DataReader;
    } 

   /// <summary>
   /// Close data reader
   /// </summary>
    public void CloseReader()
    {
      if (this.DataReader != null)
        this.DataReader.Close();
    } 

    /// <summary>
    /// Attach paramert to Command
    /// </summary>
    /// <param name="command"></param>
    /// <param name="commandParameters"></param>
    private void AttachParameters(IDbCommand command,IDbDataParameter[]commandParameters)
    {
      foreach (IDbDataParameter idbParameter in commandParameters)
      {
        if ((idbParameter.Direction == ParameterDirection.InputOutput)&&(idbParameter.Value == null))
        {
          idbParameter.Value = DBNull.Value;
        }
        command.Parameters.Add(idbParameter);
      }
    } 

    /// <summary>
    /// Preapare Command
    /// </summary>
    /// <param name="command"></param>
    /// <param name="connection"></param>
    /// <param name="transaction"></param>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <param name="commandParameters"></param>
    private void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[]commandParameters)
    {
      command.Connection = connection;
      if (OperationMode == "RELEASE")
          command.CommandText = CommonFunctions.EncriptDB(commandText,5);
       else
          command.CommandText = commandText;     
      command.CommandType = commandType;
 
      if (transaction != null)
      {
        command.Transaction = transaction;
      }
 
      if (commandParameters != null)
      {
        AttachParameters(command, commandParameters);
      }
    } 

    /// <summary>
    /// Execute NonQuery
    /// </summary>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <returns></returns>
    public int ExecuteNonQuery(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      PrepareCommand(idbCommand,this.Connection, this.Transaction, commandType, commandText,this.Parameters);
      int returnValue =idbCommand.ExecuteNonQuery();
      idbCommand.Parameters.Clear();
      return returnValue;
    } 

    /// <summary>
    /// Execute Scalar
    /// </summary>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <returns></returns>
    public object ExecuteScalar(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      PrepareCommand(idbCommand,this.Connection, this.Transaction, commandType, commandText, this.Parameters);
      object returnValue = idbCommand.ExecuteScalar();
      idbCommand.Parameters.Clear();
      return returnValue;
    } 

    /// <summary>
    /// Execute dataset
    /// </summary>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <returns></returns>
    public DataSet ExecuteDataSet(CommandType commandType, string commandText)
    {
      this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
      PrepareCommand(idbCommand,this.Connection, this.Transaction,commandType,commandText, this.Parameters);
      IDbDataAdapter dataAdapter =DBManagerFactory.GetDataAdapter(this.ProviderType);
      dataAdapter.SelectCommand = idbCommand;
      DataSet dataSet = new DataSet();
      dataAdapter.Fill(dataSet);
      idbCommand.Parameters.Clear();
      return dataSet;
    }    
 
  }
}
