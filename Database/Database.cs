using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Text;


public partial class Database
    {
        // ─── Configuration (set once at startup via Initialize()) ───────────────
        private static string m_DefaultConnectionString = string.Empty;
        private static int m_CommandTimeOut = 120;
        private static bool m_DebugSQL = false;

        /// <summary>
        /// Call this once in Program.cs to configure the Database helper.
        /// </summary>
        public static void Initialize(string connectionString, int commandTimeout = 120, bool debugSQL = false)
        {
            m_DefaultConnectionString = connectionString;
            m_CommandTimeOut = commandTimeout;
            m_DebugSQL = debugSQL;
        }

        public static string DefaultConnectionString => m_DefaultConnectionString;
        public static int CommandTimeout => m_CommandTimeOut;
        public static bool DebugSQL => m_DebugSQL;

        // DatabaseOwner kept for compatibility
        private static string m_DatabaseOwner = "dbo";
        public static string DatabaseOwner
        {
            get => m_DatabaseOwner;
            set => m_DatabaseOwner = value;
        }

        // ─── Debug logging (replaces HttpContext.Current.Response.Write) ─────────
        private static void LogSQL(string sql)
        {
            if (m_DebugSQL) Console.WriteLine("[SQL] " + sql);
        }

        // ─── DataReader ──────────────────────────────────────────────────────────
        public static SqlDataReader GetDataReader(string strSQL)
        {
            return GetDataReader(strSQL, m_DefaultConnectionString);
        }

        public static SqlDataReader GetDataReader(string strSQL, string strConnectionString)
        {
            SqlConnection objConn = null;
            SqlCommand objComm = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                objComm = new SqlCommand(strSQL, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                LogSQL(strSQL);
                return objComm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
                objConn = null;
            }
        }

        public static SqlDataReader GetDataReader(string strSQL, SqlConnection objConn)
        {
            return GetDataReader(strSQL, objConn, null);
        }

        public static SqlDataReader GetDataReader(string strSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;

            try
            {
                objComm = new SqlCommand(strSQL, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                if (objTran != null) objComm.Transaction = objTran;
                LogSQL(strSQL);
                return objComm.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── SP DataReader ───────────────────────────────────────────────────────
        public static SqlDataReader GetSPDataReader(string strProcName, Hashtable dicParam)
        {
            return GetSPDataReader(strProcName, dicParam, m_DefaultConnectionString);
        }

        public static SqlDataReader GetSPDataReader(string strProcName, Hashtable dicParam, string strConnectionString)
        {
            SqlConnection objConn = null;
            SqlCommand objComm = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                objComm = new SqlCommand(strProcName, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                objComm.CommandType = CommandType.StoredProcedure;
                if (dicParam != null)
                {
                    foreach (string strKey in dicParam.Keys)
                    {
                        if (dicParam[strKey] != null)
                            objComm.Parameters.Add(new SqlParameter(strKey, dicParam[strKey].ToString()));
                    }
                }
                return objComm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
                objConn = null;
            }
        }

        public static SqlDataReader GetSPDataReader(string strProcName, Hashtable dicParam, SqlConnection objConn)
        {
            return GetSPDataReader(strProcName, dicParam, objConn, null);
        }

        public static SqlDataReader GetSPDataReader(string strProcName, Hashtable dicParam, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;

            try
            {
                objComm = new SqlCommand(strProcName, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                objComm.CommandType = CommandType.StoredProcedure;
                if (dicParam != null)
                {
                    foreach (string strKey in dicParam.Keys)
                    {
                        if (dicParam[strKey] != null)
                            objComm.Parameters.Add(new SqlParameter(strKey, dicParam[strKey].ToString()));
                    }
                }
                if (objTran != null) objComm.Transaction = objTran;
                return objComm.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── DataSet ─────────────────────────────────────────────────────────────
        public static DataSet GetDataSet(string strSQL)
        {
            return GetDataSet(strSQL, m_DefaultConnectionString);
        }

        public static DataSet GetDataSet(string strSQL, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return GetDataSet(strSQL, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static DataSet GetDataSet(string strSQL, SqlConnection objConn)
        {
            return GetDataSet(strSQL, objConn, null);
        }

        public static DataSet GetDataSet(string strSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;
            SqlDataAdapter objAdapter = null;
            DataSet objData = null;

            try
            {
                objComm = new SqlCommand(strSQL, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                if (objTran != null) objComm.Transaction = objTran;
                objAdapter = new SqlDataAdapter(objComm);
                objAdapter.TableMappings.Add("Table", "Query");
                objData = new DataSet();
                objAdapter.Fill(objData);
                LogSQL(strSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
                if (objAdapter != null) objAdapter.Dispose();
                objAdapter = null;
            }
            return objData;
        }

        // ─── SP DataSet ──────────────────────────────────────────────────────────
        public static DataSet GetSPDataSet(string strProcName, Hashtable dicParam)
        {
            return GetSPDataSet(strProcName, dicParam, m_DefaultConnectionString);
        }

        public static DataSet GetSPDataSet(string strProcName, Hashtable dicParam, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return GetSPDataSet(strProcName, dicParam, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static DataSet GetSPDataSet(string strProcName, Hashtable dicParam, SqlConnection objConn)
        {
            return GetSPDataSet(strProcName, dicParam, objConn, null);
        }

        public static DataSet GetSPDataSet(string strProcName, Hashtable dicParam, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;
            SqlDataAdapter objAdapter = null;
            DataSet objData = null;

            try
            {
                objComm = new SqlCommand(strProcName, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                objComm.CommandType = CommandType.StoredProcedure;
                if (dicParam != null)
                {
                    foreach (string strKey in dicParam.Keys)
                    {
                        objComm.Parameters.Add(new SqlParameter(strKey, dicParam[strKey].ToString()));
                    }
                }
                if (objTran != null) objComm.Transaction = objTran;
                objAdapter = new SqlDataAdapter(objComm);
                objAdapter.TableMappings.Add("Table", "Query");
                objData = new DataSet();
                objAdapter.Fill(objData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
                if (objAdapter != null) objAdapter.Dispose();
                objAdapter = null;
            }
            return objData;
        }

        // ─── ExecuteScalar ───────────────────────────────────────────────────────
        public static object ExecuteScalar(string strSQL)
        {
            return ExecuteScalar(strSQL, m_DefaultConnectionString);
        }

        public static object ExecuteScalar(string strSQL, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return ExecuteScalar(strSQL, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static object ExecuteScalar(string strSQL, SqlConnection objConn)
        {
            return ExecuteScalar(strSQL, objConn, null);
        }

        public static object ExecuteScalar(string strSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;

            try
            {
                objComm = new SqlCommand(strSQL, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                if (objTran != null) objComm.Transaction = objTran;
                LogSQL(strSQL);
                return objComm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── ExecuteSQL ──────────────────────────────────────────────────────────
        public static int ExecuteSQL(string strSQL)
        {
            return ExecuteSQL(strSQL, m_DefaultConnectionString);
        }

        public static int ExecuteSQL(string strSQL, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return ExecuteSQL(strSQL, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static int ExecuteSQL(string strSQL, SqlConnection objConn)
        {
            return ExecuteSQL(strSQL, objConn, null);
        }

        public static int ExecuteSQL(string strSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;

            try
            {
                objComm = new SqlCommand(strSQL, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                if (objTran != null) objComm.Transaction = objTran;
                LogSQL(strSQL);
                return objComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── ExecuteSP ───────────────────────────────────────────────────────────
        public static int ExecuteSP(string strProcName, Hashtable dicParam)
        {
            return ExecuteSP(strProcName, dicParam, m_DefaultConnectionString);
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, string OutputParameterName, out object OutputParameterValue)
        {
            return ExecuteSP(strProcName, dicParam, m_DefaultConnectionString, OutputParameterName, out OutputParameterValue);
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, string strConnectionString)
        {
            object OutputParameterValue;
            return ExecuteSP(strProcName, dicParam, strConnectionString, string.Empty, out OutputParameterValue);
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, string strConnectionString, string OutputParameterName, out object OutputParameterValue)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return ExecuteSP(strProcName, dicParam, objConn, OutputParameterName, out OutputParameterValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, SqlConnection objConn)
        {
            return ExecuteSP(strProcName, dicParam, objConn, null);
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, SqlConnection objConn, string OutputParameterName, out object OutputParameterValue)
        {
            return ExecuteSP(strProcName, dicParam, objConn, null, OutputParameterName, out OutputParameterValue);
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, SqlConnection objConn, SqlTransaction objTran)
        {
            object OutputParameterValue;
            return ExecuteSP(strProcName, dicParam, objConn, objTran, string.Empty, out OutputParameterValue);
        }

        public static int ExecuteSP(string strProcName, Hashtable dicParam, SqlConnection objConn, SqlTransaction objTran, string OutputParameterName, out object OutputParameterValue)
        {
            SqlCommand objComm = null;
            SqlParameter objParam = null;
            int intReturn = 0;

            try
            {
                OutputParameterValue = null;
                objComm = new SqlCommand(strProcName, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                objComm.CommandType = CommandType.StoredProcedure;
                if (dicParam != null)
                {
                    foreach (string strKey in dicParam.Keys)
                    {
                        objComm.Parameters.Add(new SqlParameter(strKey, dicParam[strKey]));
                    }
                }
                if (!string.IsNullOrEmpty(OutputParameterName))
                {
                    objParam = new SqlParameter();
                    objParam.ParameterName = OutputParameterName;
                    objParam.Size = -1;
                    objParam.Direction = ParameterDirection.Output;
                    objComm.Parameters.Add(objParam);
                }
                if (objTran != null) objComm.Transaction = objTran;
                intReturn = objComm.ExecuteNonQuery();
                if (objParam != null) OutputParameterValue = objParam.Value;
                return intReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── ExecuteList ─────────────────────────────────────────────────────────
        public static bool ExecuteList(ArrayList arylSQL)
        {
            return ExecuteList(arylSQL, m_DefaultConnectionString);
        }

        public static bool ExecuteList(ArrayList arylSQL, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return ExecuteList(arylSQL, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static bool ExecuteList(ArrayList arylSQL, SqlConnection objConn)
        {
            SqlTransaction objTran = null;

            try
            {
                objTran = objConn.BeginTransaction();
                ExecuteList(arylSQL, objConn, objTran);
                objTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (objTran != null) objTran.Rollback();
                throw ex;
            }
            finally
            {
                if (objTran != null) objTran.Dispose();
                objTran = null;
            }
        }

        public static bool ExecuteList(ArrayList arylSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;

            try
            {
                objComm = new SqlCommand();
                objComm.Connection = objConn;
                objComm.CommandTimeout = m_CommandTimeOut;
                objComm.Transaction = objTran;
                for (int i = 0; i < arylSQL.Count; i++)
                {
                    objComm.CommandText = arylSQL[i].ToString();
                    LogSQL(arylSQL[i].ToString());
                    objComm.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── ExecuteSQLWithIdentity ──────────────────────────────────────────────
        public static long ExecuteSQLWithIdentity(string strSQL)
        {
            return ExecuteSQLWithIdentity(strSQL, m_DefaultConnectionString);
        }

        public static long ExecuteSQLWithIdentity(string strSQL, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return ExecuteSQLWithIdentity(strSQL, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static long ExecuteSQLWithIdentity(string strSQL, SqlConnection objConn)
        {
            return ExecuteSQLWithIdentity(strSQL, objConn, null);
        }

        public static long ExecuteSQLWithIdentity(string strSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlCommand objComm = null;

            try
            {
                objComm = new SqlCommand(strSQL, objConn);
                objComm.CommandTimeout = m_CommandTimeOut;
                if (objTran != null) objComm.Transaction = objTran;
                LogSQL(strSQL);
                if (objComm.ExecuteNonQuery() > 0)
                    return Convert.ToInt64(ExecuteScalar("SELECT SCOPE_IDENTITY() AS ID", objConn, objTran));
                else
                    throw new Exception("Error getting scope_identity()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objComm != null) objComm.Dispose();
                objComm = null;
            }
        }

        // ─── HasRows ─────────────────────────────────────────────────────────────
        public static bool HasRows(string strSQL)
        {
            return HasRows(strSQL, m_DefaultConnectionString);
        }

        public static bool HasRows(string strSQL, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return HasRows(strSQL, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static bool HasRows(string strSQL, SqlConnection objConn)
        {
            return HasRows(strSQL, objConn, null);
        }

        public static bool HasRows(string strSQL, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlDataReader objRead = null;

            try
            {
                objRead = GetDataReader(strSQL, objConn, objTran);
                return objRead.HasRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objRead != null) objRead.Dispose();
                objRead = null;
            }
        }

        public static bool HasSPRows(string strProcName, Hashtable dicParam)
        {
            return HasSPRows(strProcName, dicParam, m_DefaultConnectionString);
        }

        public static bool HasSPRows(string strProcName, Hashtable dicParam, string strConnectionString)
        {
            SqlConnection objConn = null;

            try
            {
                objConn = new SqlConnection(strConnectionString);
                objConn.Open();
                return HasSPRows(strProcName, dicParam, objConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn != null) objConn.Dispose();
                objConn = null;
            }
        }

        public static bool HasSPRows(string strProcName, Hashtable dicParam, SqlConnection objConn)
        {
            return HasSPRows(strProcName, dicParam, objConn, null);
        }

        public static bool HasSPRows(string strProcName, Hashtable dicParam, SqlConnection objConn, SqlTransaction objTran)
        {
            SqlDataReader objRead = null;

            try
            {
                objRead = GetSPDataReader(strProcName, dicParam, objConn, objTran);
                return objRead.HasRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objRead != null) objRead.Dispose();
                objRead = null;
            }
        }

        // ─── SQL Builders ────────────────────────────────────────────────────────
        public static string GetInsertSQL(Hashtable dicParam, string strTableName)
        {
            return GetInsertSQL(dicParam, strTableName, true);
        }

        public static string GetInsertSQL(Hashtable dicParam, string strTableName, bool NationalCharacterSet)
        {
            StringBuilder objKeyString = new StringBuilder("INSERT INTO " + strTableName + " (");
            StringBuilder objValueString = new StringBuilder("VALUES (");
            StringBuilder objReturnString = new StringBuilder();

            try
            {
                if (dicParam == null || dicParam.Keys.Count == 0)
                    throw new Exception("Missing VALUES parameters in insert statement");

                foreach (string strKey in dicParam.Keys)
                {
                    objKeyString.Append(strKey + ",");
                    objValueString.Append(HandleQuote(
                        (dicParam[strKey] == null || dicParam[strKey] == DBNull.Value)
                            ? null
                            : Convert.ToString(dicParam[strKey].GetType() == typeof(Boolean)
                                ? Convert.ToInt32(dicParam[strKey])
                                : dicParam[strKey]),
                        NationalCharacterSet) + ",");
                }

                objReturnString.Append(objKeyString.ToString().Substring(0, objKeyString.Length - 1) + ") ");
                objReturnString.Append(objValueString.ToString().Substring(0, objValueString.Length - 1) + ")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objReturnString.ToString();
        }

        public static string GetUpdateSQL(Hashtable dicParam, Hashtable dicWParam, string strTableName)
        {
            return GetUpdateSQL(dicParam, dicWParam, strTableName, true);
        }

        public static string GetUpdateSQL(Hashtable dicParam, Hashtable dicWParam, string strTableName, bool NationalCharacterSet)
        {
            StringBuilder objParamString = new StringBuilder("UPDATE " + strTableName + " SET ");
            StringBuilder objWhereString = new StringBuilder("WHERE ");
            StringBuilder objReturnString = new StringBuilder();

            try
            {
                if (dicParam == null || dicParam.Keys.Count == 0)
                    throw new Exception("Missing SET parameters in update statement");
                if (dicWParam == null || dicWParam.Keys.Count == 0)
                    throw new Exception("Missing WHERE parameters in update statement");

                foreach (string strKey in dicParam.Keys)
                {
                    if (dicParam[strKey] == DBNull.Value || dicParam[strKey] == null)
                    {
                        objParamString.Append(strKey + "=NULL, ");
                    }
                    else
                    {
                        string strAssignment = (Convert.ToString(dicParam[strKey]).Length >= 3 &&
                            Convert.ToString(dicParam[strKey]).Substring(0, 3) == "_##") ? " " : "=";
                        objParamString.Append(strKey + strAssignment + HandleQuote(
                            Convert.ToString(dicParam[strKey].GetType() == typeof(Boolean)
                                ? Convert.ToInt32(dicParam[strKey])
                                : dicParam[strKey]), NationalCharacterSet) + ", ");
                    }
                }

                foreach (string strKey in dicWParam.Keys)
                {
                    if (dicWParam[strKey] == DBNull.Value || dicWParam[strKey] == null)
                    {
                        objWhereString.Append(strKey + " IS NULL AND ");
                    }
                    else
                    {
                        string strAssignment = (Convert.ToString(dicWParam[strKey]).Length >= 3 &&
                            Convert.ToString(dicWParam[strKey]).Substring(0, 3) == "_##") ? " " : "=";
                        objWhereString.Append(strKey + strAssignment + HandleQuote(
                            Convert.ToString(dicWParam[strKey].GetType() == typeof(Boolean)
                                ? Convert.ToInt32(dicWParam[strKey])
                                : dicWParam[strKey]), NationalCharacterSet) + " AND ");
                    }
                }

                objReturnString.Append(objParamString.ToString().Substring(0, objParamString.Length - 2) + " ");
                objReturnString.Append(objWhereString.ToString().Substring(0, objWhereString.Length - 5));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objReturnString.ToString();
        }

        public static string GetDeleteSQL(Hashtable dicWParam, string strTableName)
        {
            return GetDeleteSQL(dicWParam, strTableName, true);
        }

        public static string GetDeleteSQL(Hashtable dicWParam, string strTableName, bool NationalCharacterSet)
        {
            StringBuilder objParamString = new StringBuilder("DELETE FROM " + strTableName + " ");
            StringBuilder objWhereString = new StringBuilder("WHERE ");
            StringBuilder objReturnString = new StringBuilder();

            try
            {
                if (dicWParam == null || dicWParam.Keys.Count == 0)
                    throw new Exception("Missing WHERE parameters in delete statement");

                foreach (string strKey in dicWParam.Keys)
                {
                    if (dicWParam[strKey] == DBNull.Value || dicWParam[strKey] == null)
                    {
                        objWhereString.Append(strKey + " IS NULL");
                    }
                    else
                    {
                        string strAssignment = (Convert.ToString(dicWParam[strKey]).Length >= 3 &&
                            Convert.ToString(dicWParam[strKey]).Substring(0, 3) == "_##") ? " " : "=";
                        objWhereString.Append(strKey + strAssignment + HandleQuote(
                            Convert.ToString(dicWParam[strKey].GetType() == typeof(Boolean)
                                ? Convert.ToInt32(dicWParam[strKey])
                                : dicWParam[strKey]), NationalCharacterSet) + " AND ");
                    }
                }

                objReturnString.Append(objParamString.ToString() +
                    objWhereString.ToString().Substring(0, objWhereString.Length - 5));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objReturnString.ToString();
        }

        // ─── Paging ──────────────────────────────────────────────────────────────
        public static string GetPagingSQL(string strSQL, string OrderByColumnNames, int PageSize, int PageNumber)
        {
            if (OrderByColumnNames.Contains("'")) throw new Exception("Order by column cannot contain single quotes");

            return string.Format(@"
            SELECT * FROM
            (
                SELECT *, COUNT(*) OVER () AS TotalRecord FROM
                (
                    SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) AS RowNumber FROM
                    (
                        {1}
                    ) t1
                ) t2
            ) t3
            WHERE RowNumber >= {2} AND RowNumber <= {3}
            ORDER BY RowNumber ASC",
                OrderByColumnNames, strSQL,
                (PageNumber - 1) * PageSize + 1, PageNumber * PageSize);
        }

        public static string GetPagingSQL(string strSQL, string OrderByColumnNames, bool SortAscending, int PageSize, int PageNumber)
        {
            if (OrderByColumnNames.Contains("'")) throw new Exception("Order by column cannot contain single quotes");

            return string.Format(@"
            SELECT * FROM
            (
                SELECT *, COUNT(*) OVER () AS TotalRecord FROM
                (
                    SELECT *, ROW_NUMBER() OVER (ORDER BY {0} {1}) AS RowNumber FROM
                    (
                        {2}
                    ) t1
                ) t2
            ) t3
            WHERE RowNumber >= {3} AND RowNumber <= {4}
            ORDER BY RowNumber ASC",
                OrderByColumnNames, SortAscending ? "ASC" : "DESC", strSQL,
                (PageNumber - 1) * PageSize + 1, PageNumber * PageSize);
        }

        // ─── HandleQuote ─────────────────────────────────────────────────────────
        public static string HandleQuote(string strParam)
        {
            return HandleQuote(strParam, true);
        }

        public static string HandleQuote(string strParam, bool NationalCharacterSet)
        {
            string strReturn = "NULL";
            if (strParam != null) strParam = strParam.Trim();
            if (!string.IsNullOrEmpty(strParam))
            {
                if (strParam.Length >= 3 && strParam.Substring(0, 3) == "_##")
                    strReturn = strParam.Substring(3);
                else if (strParam.Length >= 2 && strParam.Substring(0, 2) == "_#")
                    strReturn = strParam.Substring(2);
                else
                    strReturn = (NationalCharacterSet ? "N'" : "'") + strParam.Replace("'", "''") + "'";
            }
            return strReturn + " ";
        }

        public static string HandleQuoteIn(string strParam)
        {
            return HandleQuoteIn(strParam, true);
        }

        public static string HandleQuoteIn(string strParam, bool NationalCharacterSet)
        {
            string strReturn = "NULL";
            if (strParam != null) strParam = strParam.Trim();
            if (!string.IsNullOrEmpty(strParam))
            {
                if (strParam.Length >= 3 && strParam.Substring(0, 3) == "_##")
                    strReturn = strParam.Substring(3);
                else if (strParam.Length >= 2 && strParam.Substring(0, 2) == "_#")
                    strReturn = strParam.Substring(2);
                else
                    strReturn = strParam;
            }
            return strReturn + " ";
        }

        // ─── DateTime helpers ────────────────────────────────────────────────────
        public static bool IsValidSqlDateTime(string InputDateTime)
        {
            bool blnReturn = false;
            DateTime dtNetDateTime = DateTime.MinValue;
            if (DateTime.TryParse(InputDateTime, out dtNetDateTime))
            {
                try
                {
                    var objSqlDateTime = new System.Data.SqlTypes.SqlDateTime(dtNetDateTime);
                    blnReturn = true;
                }
                catch { }
            }
            return blnReturn;
        }

        public static DateTime GetSqlDateTime(DateTime InputDateTime)
        {
            IsValidSqlDateTime(ref InputDateTime);
            return InputDateTime;
        }

        public static bool IsValidSqlDateTime(ref DateTime InputDateTime)
        {
            DateTime minDateTime = new DateTime(1753, 1, 1);
            DateTime maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (InputDateTime < minDateTime) { InputDateTime = minDateTime; return false; }
            else if (InputDateTime > maxDateTime) { InputDateTime = maxDateTime; return false; }
            return true;
        }
    }

