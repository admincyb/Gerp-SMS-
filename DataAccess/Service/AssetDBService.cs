//===============================================================================
// GrandTrust InfoTech  Data Access Class
//
// AssetDBService.cs
//
// This file contains the implementations of the AssetDBService class.
//
// Define the Data Wrapper Class which provides the functionalities of DBProvider Factory. 
// 
//===============================================================================
//
// Copyright (C) GrandTrust InfoTech
//
//==============================================================================

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;
//using System.Data.Common;
//using System.Collections;
//using System.Configuration;
//// For oracle 
//using System.Data.SqlClient;
//using GTIService;
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using GTIService;


namespace DataAccess
{
    /// <remarks>
    /// Define the Data Wrapper Class which provides the functionalities of DBProvider Factory.
    /// </remarks>
    /// <author>GTI</author>
    /// <createdate>18/Nov/2008</createdate>
    /// <modifieddate>18/Nov/2008</modifieddate>
   public class AssetDBService
    {
        #region Declarations

        private DbProviderFactory   oFactory;
        private DbConnection        oConnection;
        private ConnectionState     oConnectionState;
        public  DbCommand           oCommand;
        public  SqlCommand oCommandOracle;         // For oracle
        private DbParameter         oParameter;
        private SqlParameter     oParameterOracle;       // For oracle
        private DbTransaction       oTransaction;
        private bool                mblTransaction;

        private string S_CONNECTION = ConfigurationManager.ConnectionStrings["ASSETCONNECTIONSTRING"].ConnectionString;
        private readonly string S_PROVIDER = ConfigurationManager.ConnectionStrings["ASSETCONNECTIONSTRING"].ProviderName;
        //Mode Of Operation
        //private string OperationMode = ConfigurationSettings.AppSettings["OperationMode"].ToUpper();
        
        #endregion
     
        #region Enumerators

        public enum TransactionType : uint
        {
            Open        = 1,
            Commit      = 2,
            Rollback    = 3
        }
        
        #endregion

        #region Structures
       
        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	
        ///OutPut			:	
        ///Comments			:	
        /// </summary>
        public struct ParametersOLD
        {
            public string ParamName;
            public object ParamValue;
            public ParameterDirection ParamDirection;

            public ParametersOLD(string Name, object Value)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamDirection = ParameterDirection.Input;
            }

            public ParametersOLD(string Name, object Value, ParameterDirection Direction)
            {
                ParamName       = Name;
                ParamValue      = Value;
                ParamDirection  = Direction;
            }

        }
        
        public struct Parameters
        {
            public string ParamName;
            public object ParamValue;
            public ParameterDirection ParamDirection;
            public ParameterType ParamType;
            public int ParamSize;

            public Parameters(string Name, object Value)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamSize = 0;
                ParamDirection = ParameterDirection.Input;
                ParamType = 0;
            }

            public Parameters(string Name, object Value, ParameterDirection Direction)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamSize = 0;
                ParamDirection = Direction;
                ParamType = 0;
            }

            public Parameters(string Name, object Value, ParameterType Type)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamSize = 0;
                ParamDirection = ParameterDirection.Input;
                ParamType = Type;
                
            }


            public Parameters(string Name, object Value, ParameterDirection Direction, ParameterType pType)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamSize = 0;
                ParamDirection = Direction;
                ParamType = pType;
            }

            
            public Parameters(string Name, object Value, int Size, ParameterDirection Direction, ParameterType pType)
            {
                ParamName = Name;
                ParamValue = Value;
                ParamSize = Size;
                ParamDirection = Direction;
                ParamType = pType;
            }


        }



        


        #endregion

        #region Constructor

        public AssetDBService()
        {
          
           S_CONNECTION = ConfigurationManager.ConnectionStrings["ASSETCONNECTIONSTRING"].ConnectionString;
           oFactory = DbProviderFactories.GetFactory(S_PROVIDER);
            //OperationMode = ConfigurationSettings.AppSettings["OperationMode"].ToUpper();  
        }

        public AssetDBService(string ConnectionString)
        {
            S_CONNECTION = ConnectionString;
            oFactory = DbProviderFactories.GetFactory(S_PROVIDER);
            //OperationMode = ConfigurationSettings.AppSettings["OperationMode"].ToUpper();  
        }

        #endregion

        #region Destructor 

        ~AssetDBService()
        {
            oFactory = null;
        }

        #endregion

        #region Connections

        /// <summary>
        ///Description	    :	This function is used to Open Database Connection
        ///Date				:	
        ///Input			:	NA
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        public void EstablishFactoryConnection()
        {
            /*
            // This check is not required as it will throw "Invalid Provider Exception" on the contructor itself.
            if (0 == DbProviderFactories.GetFactoryClasses().Select("InvariantName='" + S_PROVIDER + "'").Length)
                throw new Exception("Invalid Provider");
            */
            oConnection = oFactory.CreateConnection();

            if (oConnection.State == ConnectionState.Closed)
            {
                oConnection.ConnectionString    = S_CONNECTION;
                oConnection.Open();
                oConnectionState                = ConnectionState.Open;
            }
        }

        /// <summary>
        ///Description	    :	This function is used to Close Database Connection
        ///Date				:	
        ///Input			:	NA
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        public void CloseFactoryConnection()
        {
            //check for an open connection            
            try
            {
                if (oConnection.State == ConnectionState.Open)
                {
                    oConnection.Close();
                    oConnectionState = ConnectionState.Closed;
                }
            }
            catch (DbException oDbErr)
            {
                //catch any SQL server data provider generated error messag
                throw new Exception(oDbErr.Message);
            }
            catch (System.NullReferenceException oNullErr)
            {
                throw new Exception(oNullErr.Message);
            }
            finally
            {
               if (null != oConnection)
                   oConnection.Dispose();
            }
        }

        #endregion

        #region Transaction

        /// <summary>
        ///Description	    :	This function is used to Handle Transaction Events
        ///Date				:	
        ///Input			:	Transaction Event Type
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        public void TransactionHandler(TransactionType veTransactionType)
        {
            switch (veTransactionType)
            {
                case TransactionType.Open:  //open a transaction
                    try
                    {
                        oTransaction    = oConnection.BeginTransaction();
                        mblTransaction  = true;
                    }
                    catch (InvalidOperationException oErr)
                    {
                        throw new Exception("@TransactionHandler - " + oErr.Message);
                    }
                    break;

                case TransactionType.Commit:  //commit the transaction
                    if (null != oTransaction.Connection)
                    {
                        try
                        {
                            oTransaction.Commit();
                            mblTransaction = false;
                        }
                        catch (InvalidOperationException oErr)
                        {
                            throw new Exception("@TransactionHandler - " + oErr.Message);
                        }
                    }
                    break;

                case TransactionType.Rollback:  //rollback the transaction
                    try
                    {
                        if (mblTransaction)
                        {
                            oTransaction.Rollback();
                        }
                        mblTransaction = false;
                    }
                    catch (InvalidOperationException oErr)
                    {
                        throw new Exception("@TransactionHandler - " + oErr.Message);
                    }
                    break;
            }

        }

        #endregion

        #region Commands

        #region Parameterless Methods

        /// <summary>
        ///Description	    :	This function is used to Prepare Command For Execution
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	Has to be changed/removed if object based array concept is removed.
        /// </summary>
        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString    = S_CONNECTION;
                oConnection.Open();
                oConnectionState                = ConnectionState.Open;
            }

            if (null == oCommand)
                oCommand = oFactory.CreateCommand();

            oCommand.Connection     = oConnection;
            //if (OperationMode == "RELEASE")
            //    oCommand.CommandText    =  CommonFunctions.EncriptDB(cmdText, 5);
            //else
                oCommand.CommandText    = cmdText;
                


            //oCommand.CommandText    = cmdText;
            oCommand.CommandType    = cmdType;

            if (blTransaction)
                oCommand.Transaction = oTransaction;
        }

        #endregion

        #region Object Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to Prepare Command For Execution
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString    = S_CONNECTION;
                oConnection.Open();
                oConnectionState                = ConnectionState.Open;
            }

            if (null == oCommand)
                oCommand            = oFactory.CreateCommand();

            oCommand.Connection     = oConnection;
            //if (OperationMode == "RELEASE")
            //    oCommand.CommandText = CommonFunctions.EncriptDB(cmdText, 5);
            //else
                oCommand.CommandText = cmdText;
            //oCommand.CommandText    = cmdText;
            oCommand.CommandType    = cmdType;

            if (blTransaction)
                oCommand.Transaction = oTransaction;

            if (null != cmdParms)
                CreateDBParameters(cmdParms);
        }

        #endregion

        #region Structure Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to Prepare Command For Execution
        ///Date				:	28 June 2007
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        private void PrepareCommand(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString = S_CONNECTION;
                oConnection.Open();
                oConnectionState = ConnectionState.Open;
            }

            oCommand = oFactory.CreateCommand();
            oCommand.Connection = oConnection;
            //if (OperationMode == "RELEASE")
            //    oCommand.CommandText = CommonFunctions.EncriptDB(cmdText, 5);
            //else
            oCommand.CommandText = cmdText;

            oCommand.CommandType = cmdType;

            if (blTransaction)
                oCommand.Transaction = oTransaction;

            if (null != cmdParms)
                CreateDBParameters(cmdParms);
        }

        #endregion
        
        #endregion

        #region Parameter Methods

        #region Object Based

        /// <summary>
        ///Description	    :	This function is used to Create Parameters for the Command For Execution
        ///Date				:	
        ///Input			:	2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        private void CreateDBParameters(object[,] colParameters)
        {
            for (int i = 0; i < colParameters.Length / 2; i++)
            {
                oParameter = oCommand.CreateParameter();
                //if (OperationMode == "RELEASE")
                //    oParameter.ParameterName    = CommonFunctions.EncriptDB(colParameters[i, 0].ToString(),5);
                //else
                oParameter.ParameterName = colParameters[i, 0].ToString();

                //oParameter.ParameterName    = colParameters[i, 0].ToString();
                oParameter.Value = colParameters[i, 1];
                oCommand.Parameters.Add(oParameter);
            }
        }

        #endregion

        #region Structure Based

        /// <summary>
        ///Description	    :	This function is used to Create Parameters for the Command For Execution
        ///Date				:	
        ///Input			:	2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        private void CreateDBParameters(Parameters[] colParameters)
        {
            for (int i = 0; i < colParameters.Length; i++)
            {
                Parameters oParam = (Parameters)colParameters[i];

                oParameter = oCommand.CreateParameter();
                //if (OperationMode == "RELEASE")
                //    oParameter.ParameterName = CommonFunctions.EncriptDB(oParam.ParamName, 5);
                //else
                oParameter.ParameterName = oParam.ParamName;

                //oParameter.ParameterName    = oParam.ParamName;
                oParameter.Value = oParam.ParamValue;
                oParameter.Size = oParam.ParamSize;
                oParameter.Direction = oParam.ParamDirection;
                oCommand.Parameters.Add(oParameter);
            }
        }
        
        #endregion

        #endregion

        #region Execute Methods

        #region Parameterless Methods

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        ///                     Has to be changed/removed if object based array concept is removed.
        /// </summary>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText);
                return oCommand.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose(); 
                CloseFactoryConnection();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array, Clear Paramaeters
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        ///                     Has to be changed/removed if object based array concept is removed.
        /// </summary>
        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText)
        {
            try
            {
                PrepareCommand(blTransaction, cmdType, cmdText);
                int val = oCommand.ExecuteNonQuery();

                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }

        #endregion

        #region Object Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array, Clear Parameters
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        /// </summary>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();                
            }           
        }

         /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Count of Records Affected
        ///Comments			:	Overloaded method. 
        /// </summary>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteNonQuery(cmdType, cmdText, cmdParms, true);
        }

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array, Clear Paramaeters
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        /// </summary>
        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Count of Records Affected
        ///Comments			:	Overloaded function. 
        /// </summary>
        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteNonQuery(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #region Structure Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Date				:	
        ///Input			:	Command Type, Command Text, Parameter Structure Array, Clear Parameters
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        /// </summary>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false , cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, Parameter Structure Array
        ///OutPut			:	Count of Records Affected
        ///Comments			:	Overloaded method. 
        /// </summary>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteNonQuery(cmdType, cmdText, cmdParms, true);
        }
        
        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Author			:	
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, Parameter Structure Array, Clear Parameters
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        /// </summary>
        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to Execute the Command
        ///Author			:	
        ///Date				:	
        ///Input			:	Transaction, Command Type, Command Text, Parameter Structure Array
        ///OutPut			:	Count of Records Affected
        ///Comments			:	
        /// </summary>
        public int ExecuteNonQuery(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteNonQuery(blTransaction, cmdType, cmdText, cmdParms, true);
        }
        
        #endregion
                
        #endregion

        #region Reader Methods

        #region Parameterless Methods

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Reader	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Reader
        ///Comments			:	
        ///                     Has to be changed/removed if object based array concept is removed.
        /// </summary>
        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText);
                DbDataReader dr = oCommand.ExecuteReader(CommandBehavior.CloseConnection);
                oCommand.Parameters.Clear();
                return dr;

            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }

        #endregion

        #region Object Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Reader	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Reader
        ///Comments			:	
        /// </summary>
        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, object[,] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work

            try
            {

                EstablishFactoryConnection();                
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                DbDataReader dr = oCommand.ExecuteReader(CommandBehavior.CloseConnection);
                oCommand.Parameters.Clear();
                return dr;

            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }

        #endregion

        #region Structure Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Reader	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, Parameter AStructure Array
        ///OutPut			:	Data Reader
        ///Comments			:	
        /// </summary>
        public DbDataReader ExecuteReader(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
            }
        }
               
        #endregion

        #endregion

        #region Adatper Methods

        #region Parameterless Methods

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Adapter	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Set
        ///Comments			:	
        ///                     Has to be changed/removed if object based array concept is removed.
        /// </summary>
        public DataSet DataAdapter(CommandType cmdType, string cmdText)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda   = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(false, cmdType, cmdText);

                dda.SelectCommand   = oCommand;
                DataSet ds          = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #region Object Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Adapter	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Set
        ///Comments			:	
        /// </summary>
        public DataSet DataAdapter(CommandType cmdType, string cmdText, object[,] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(false, cmdType, cmdText, cmdParms);

                dda.SelectCommand   = oCommand;
                DataSet ds          = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #region Structure Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Adapter	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Set
        ///Comments			:	
        /// </summary>
        public DataSet DataAdapter(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda   = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(false, cmdType, cmdText, cmdParms);

                dda.SelectCommand   = oCommand;
                DataSet ds          = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #endregion

        #region Scalar Methods

        #region Parameterless Methods

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            try
            {
                EstablishFactoryConnection();

                PrepareCommand(false, cmdType, cmdText);

                object val = oCommand.ExecuteScalar();

                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        #endregion

        #region Object Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

         /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	Overloaded Method. 
        /// </summary>
        public object ExecuteScalar(CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteScalar(cmdType, cmdText, cmdParms, true);
        }

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, object[,] cmdParms)
        {
            return ExecuteScalar(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #region Structure Based Parameter Array

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {
                EstablishFactoryConnection();
                PrepareCommand(false, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	Overloaded Method. 
        /// </summary>
        public object ExecuteScalar(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteScalar(cmdType, cmdText, cmdParms, true);
        }

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms, bool blDisposeCommand)
        {
            try
            {

                PrepareCommand(blTransaction, cmdType, cmdText, cmdParms);
                return oCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (blDisposeCommand && null != oCommand)
                    oCommand.Dispose();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to invoke Execute Scalar Method	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Object
        ///Comments			:	
        /// </summary>
        public object ExecuteScalar(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            return ExecuteScalar(blTransaction, cmdType, cmdText, cmdParms, true);
        }

        #endregion

        #endregion
        

        #region Oracle Dependent Methods

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Adapter	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Set
        ///Comments			:	
        /// </summary>
        /// <author>GTI</author>
        public DataSet DataAdapterOracle(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommandOracle(false, cmdType, cmdText, cmdParms);

                dda.SelectCommand = oCommandOracle;
                DataSet ds = new DataSet();
                dda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommandOracle)
                    oCommandOracle.Dispose();
                CloseFactoryConnection();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Adapter	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Set
        ///Comments			:	
        /// </summary>
        /// <author>GTI</author>
        public DataTable DataAdapterTableOracle(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dbDataAdapter = null;
            try
            {
                EstablishFactoryConnection();
                dbDataAdapter = oFactory.CreateDataAdapter();
                PrepareCommandOracle(false, cmdType, cmdText, cmdParms);

                dbDataAdapter.SelectCommand = oCommandOracle;
                DataTable dataTable = new DataTable();
                dbDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommandOracle)
                    oCommandOracle.Dispose();
                CloseFactoryConnection();
            }
        }
        
        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Reader	
        ///Date				:	
        ///Input			:	Command Type, Command Text, Parameter AStructure Array
        ///OutPut			:	Data Reader
        ///Comments			:	
        /// </summary>
        /// <author>GTI</author>
        public DbDataReader ExecuteReaderOracle(CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {

                EstablishFactoryConnection();
                PrepareCommandOracle(false, cmdType, cmdText, cmdParms);
                return oCommandOracle.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {
                CloseFactoryConnection();
                throw ex;
            }
            finally
            {
                if (null != oCommandOracle)
                    oCommandOracle.Dispose();
            }
        }

        /// <summary>
        ///Description	    :	This function is used to Prepare Command For Execution
        ///Date				:	28 June 2007
        ///Input			:	Transaction, Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        /// <author>GTI</author>
        private void PrepareCommandOracle(bool blTransaction, CommandType cmdType, string cmdText, Parameters[] cmdParms)
        {

            if (oConnection.State != ConnectionState.Open)
            {
                oConnection.ConnectionString = S_CONNECTION;
                oConnection.Open();
                oConnectionState = ConnectionState.Open;
            }

            oCommandOracle = (SqlCommand) oFactory.CreateCommand();
            oCommandOracle.Connection = (SqlConnection) oConnection;
            oCommandOracle.CommandText = cmdText;
            oCommandOracle.CommandType = cmdType;

            if (blTransaction)
                oCommandOracle.Transaction = (SqlTransaction) oTransaction;

            if (null != cmdParms)
                CreateDBParametersOracle(cmdParms);
        }


        /// <summary>
        ///Description	    :	This function is used to Create Parameters for the Command For Execution
        ///Date				:	
        ///Input			:	2-Dimensional Parameter Array
        ///OutPut			:	NA
        ///Comments			:	
        /// </summary>
        /// <author>GTI</author>
        private void CreateDBParametersOracle(Parameters[] colParameters)
        {
            for (int i = 0; i < colParameters.Length; i++)
            {
                Parameters oParam = (Parameters)colParameters[i];

                oParameterOracle = oCommandOracle.CreateParameter();
                oParameterOracle.ParameterName = oParam.ParamName;
                oParameterOracle.Value = oParam.ParamValue;
                oParameterOracle.Direction = oParam.ParamDirection;
                if(oParam.ParamType != ParameterType.Default)
                    oParameterOracle.SqlDbType = (SqlDbType)oParam.ParamType;

                oCommandOracle.Parameters.Add(oParameterOracle);

            }
        }

        /// <summary>
        /// Oracle Data type
        /// </summary>
        /// <author>GTI</author> 
        public enum ParameterType
        {
            Default = 0,
            //     An Oracle BFILE data type that contains a reference to binary data with a
            //     maximum size of 4 gigabytes that is stored in an external file. Use the OracleClient
            //     System.Data.OracleClient.OracleBFile data type with the System.Data.OracleClient.OracleParameter.Value
            //     property.
            BFile = 1,
            //
            // Summary:
            //     An Oracle BLOB data type that contains binary data with a maximum size of
            //     4 gigabytes. Use the OracleClient System.Data.OracleClient.OracleLob data
            //     type in System.Data.OracleClient.OracleParameter.Value.
            Blob = 2,
            //
            // Summary:
            //     An Oracle CHAR data type that contains a fixed-length character string with
            //     a maximum size of 2,000 bytes. Use the .NET Framework System.String or OracleClient
            //     System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            Char = 3,
            //
            // Summary:
            //     An Oracle CLOB data type that contains character data, based on the default
            //     character set on the server, with a maximum size of 4 gigabytes. Use the
            //     OracleClient System.Data.OracleClient.OracleLob data type in System.Data.OracleClient.OracleParameter.Value.
            Clob = 4,
            //
            // Summary:
            //     An Oracle REF CURSOR. The System.Data.OracleClient.OracleDataReader object
            //     is not available.
            Cursor = 5,
            //
            // Summary:
            //     An Oracle DATE data type that contains a fixed-length representation of a
            //     date and time, ranging from January 1, 4712 B.C. to December 31, A.D. 4712,
            //     with the default format dd-mmm-yy. For A.D. dates, DATE maps to System.DateTime.
            //     To bind B.C. dates, use a String parameter and the Oracle TO_DATE or TO_CHAR
            //     conversion functions for input and output parameters respectively. Use the
            //     .NET Framework System.DateTime or OracleClient System.Data.OracleClient.OracleDateTime
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            DateTime = 6,
            //
            // Summary:
            //     An Oracle INTERVAL DAY TO SECOND data type (Oracle 9i or later) that contains
            //     an interval of time in days, hours, minutes, and seconds, and has a fixed
            //     size of 11 bytes. Use the .NET Framework System.TimeSpan or OracleClient
            //     System.Data.OracleClient.OracleTimeSpan data type in System.Data.OracleClient.OracleParameter.Value.
            IntervalDayToSecond = 7,
            //
            // Summary:
            //     An Oracle INTERVAL YEAR TO MONTH data type (Oracle 9i or later) that contains
            //     an interval of time in years and months, and has a fixed size of 5 bytes.
            //     Use the .NET Framework System.Int32 or OracleClient System.Data.OracleClient.OracleMonthSpan
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            IntervalYearToMonth = 8,
            //
            // Summary:
            //     An Oracle LONGRAW data type that contains variable-length binary data with
            //     a maximum size of 2 gigabytes. Use the .NET Framework Byte[] or OracleClient
            //     System.Data.OracleClient.OracleBinary data type in System.Data.OracleClient.OracleParameter.Value.
            LongRaw = 9,
            //
            // Summary:
            //     An Oracle LONG data type that contains a variable-length character string
            //     with a maximum size of 2 gigabytes. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            LongVarChar = 10,
            //
            // Summary:
            //     An Oracle NCHAR data type that contains fixed-length character string to
            //     be stored in the national character set of the database, with a maximum size
            //     of 2,000 bytes (not characters) when stored in the database. The size of
            //     the value depends on the national character set of the database. See your
            //     Oracle documentation for more information. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            NChar = 11,
            //
            // Summary:
            //     An Oracle NCLOB data type that contains character data to be stored in the
            //     national character set of the database, with a maximum size of 4 gigabytes
            //     (not characters) when stored in the database. The size of the value depends
            //     on the national character set of the database. See your Oracle documentation
            //     for more information. Use the .NET Framework System.String or OracleClient
            //     System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            NClob = 12,
            //
            // Summary:
            //     An Oracle NUMBER data type that contains variable-length numeric data with
            //     a maximum precision and scale of 38. This maps to System.Decimal. To bind
            //     an Oracle NUMBER that exceeds what System.Decimal.MaxValue can contain, either
            //     use an System.Data.OracleClient.OracleNumber data type, or use a String parameter
            //     and the Oracle TO_NUMBER or TO_CHAR conversion functions for input and output
            //     parameters respectively. Use the .NET Framework System.Decimal or OracleClient
            //     System.Data.OracleClient.OracleNumber data type in System.Data.OracleClient.OracleParameter.Value.
            Number = 13,
            //
            // Summary:
            //     An Oracle NVARCHAR2 data type that contains a variable-length character string
            //     stored in the national character set of the database, with a maximum size
            //     of 4,000 bytes (not characters) when stored in the database. The size of
            //     the value depends on the national character set of the database. See your
            //     Oracle documentation for more information. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            NVarChar = 14,
            //
            // Summary:
            //     An Oracle RAW data type that contains variable-length binary data with a
            //     maximum size of 2,000 bytes. Use the .NET Framework Byte[] or OracleClient
            //     System.Data.OracleClient.OracleBinary data type in System.Data.OracleClient.OracleParameter.Value.
            Raw = 15,
            //
            // Summary:
            //     The base64 string representation of an Oracle ROWID data type. Use the .NET
            //     Framework System.String or OracleClient System.Data.OracleClient.OracleString
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            RowId = 16,
            //
            // Summary:
            //     An Oracle TIMESTAMP (Oracle 9i or later) that contains date and time (including
            //     seconds), and ranges in size from 7 to 11 bytes. Use the .NET Framework System.DateTime
            //     or OracleClient System.Data.OracleClient.OracleDateTime data type in System.Data.OracleClient.OracleParameter.Value.
            Timestamp = 18,
            //
            // Summary:
            //     An Oracle TIMESTAMP WITH LOCAL TIMEZONE (Oracle 9i or later) that contains
            //     date, time, and a reference to the original time zone, and ranges in size
            //     from 7 to 11 bytes. Use the .NET Framework System.DateTime or OracleClient
            //     System.Data.OracleClient.OracleDateTime data type in System.Data.OracleClient.OracleParameter.Value.
            TimestampLocal = 19,
            //
            // Summary:
            //     An Oracle TIMESTAMP WITH TIMEZONE (Oracle 9i or later) that contains date,
            //     time, and a specified time zone, and has a fixed size of 13 bytes. Use the
            //     .NET Framework System.DateTime or OracleClient System.Data.OracleClient.OracleDateTime
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            TimestampWithTZ = 20,
            //
            // Summary:
            //     An Oracle VARCHAR2 data type that contains a variable-length character string
            //     with a maximum size of 4,000 bytes. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            VarChar = 22,
            //
            // Summary:
            //     An integral type representing unsigned 8-bit integers with values between
            //     0 and 255. This is not a native Oracle data type, but is provided to improve
            //     performance when binding input parameters. Use the .NET Framework System.Byte
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            Byte = 23,
            //
            // Summary:
            //     An integral type representing unsigned 16-bit integers with values between
            //     0 and 65535. This is not a native Oracle data type, but is provided to improve
            //     performance when binding input parameters. For information about conversion
            //     of Oracle numeric values to common language runtime (CLR) data types, see
            //     System.Data.OracleClient.OracleNumber. Use the .NET Framework System.UInt16
            //     or OracleClient System.Data.OracleClient.OracleNumber data type in System.Data.OracleClient.OracleParameter.Value.
            UInt16 = 24,
            //
            // Summary:
            //     An integral type representing unsigned 32-bit integers with values between
            //     0 and 4294967295. This is not a native Oracle data type, but is provided
            //     to improve performance when binding input parameters. For information about
            //     conversion of Oracle numeric values to common language runtime (CLR) data
            //     types, see System.Data.OracleClient.OracleNumber. Use the .NET Framework
            //     System.UInt32 or OracleClient System.Data.OracleClient.OracleNumber data
            //     type in System.Data.OracleClient.OracleParameter.Value.
            UInt32 = 25,
            //
            // Summary:
            //     An integral type representing signed 8 bit integers with values between -128
            //     and 127. This is not a native Oracle data type, but is provided to improve
            //     performance when binding input parameters. Use the .NET Framework System.SByte
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            SByte = 26,
            //
            // Summary:
            //     An integral type representing signed 16-bit integers with values between
            //     -32768 and 32767. This is not a native Oracle data type, but is provided
            //     to improve performance when binding input parameters. For information about
            //     conversion of Oracle numeric values to common language runtime (CLR) data
            //     types, see System.Data.OracleClient.OracleNumber. Use the .NET Framework
            //     System.Int16 or OracleClient System.Data.OracleClient.OracleNumber data type
            //     in System.Data.OracleClient.OracleParameter.Value.
            Int16 = 27,
            //
            // Summary:
            //     An integral type representing signed 32-bit integers with values between
            //     -2147483648 and 2147483647. This is not a native Oracle data type, but is
            //     provided for performance when binding input parameters. For information about
            //     conversion of Oracle numeric values to common language runtime data types,
            //     see System.Data.OracleClient.OracleNumber. Use the .NET Framework System.Int32
            //     or OracleClient System.Data.OracleClient.OracleNumber data type in System.Data.OracleClient.OracleParameter.Value.
            Int32 = 28,
            //
            // Summary:
            //     A single-precision floating-point value. This is not a native Oracle data
            //     type, but is provided to improve performance when binding input parameters.
            //     For information about conversion of Oracle numeric values to common language
            //     runtime data types, see System.Data.OracleClient.OracleNumber. Use the .NET
            //     Framework System.Single or OracleClient System.Data.OracleClient.OracleNumber
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            Float = 29,
            //
            // Summary:
            //     A double-precision floating-point value. This is not a native Oracle data
            //     type, but is provided to improve performance when binding input parameters.
            //     For information about conversion of Oracle numeric values to common language
            //     runtime (CLR) data types, see System.Data.OracleClient.OracleNumber. Use
            //     the .NET Framework System.Double or OracleClient System.Data.OracleClient.OracleNumber
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            Double = 30,

            // Summary:
            //     A XML Datatype for sql
            XML =31,
        }


        #endregion
        /// <summary>
        ///Description	    :	This function is used to fetch data using Data Adapter	
        ///Author			:	
        ///Date				:	
        ///Input			:	Command Type, Command Text, 2-Dimensional Parameter Array
        ///OutPut			:	Data Set
        ///Comments			:	
        ///                     Has to be changed/removed if object based array concept is removed.
        /// </summary>
        public DataTable DataAdapterTable(CommandType cmdType, string cmdText)
        {
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            DbDataAdapter dda = null;
            try
            {
                EstablishFactoryConnection();
                dda = oFactory.CreateDataAdapter();
                PrepareCommand(false, cmdType, cmdText);

                dda.SelectCommand = oCommand;
                DataTable dataTable = new DataTable();
                dda.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != oCommand)
                    oCommand.Dispose();
                CloseFactoryConnection();
            }
        }
    }
}
