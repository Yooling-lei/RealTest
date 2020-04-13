using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApp1
{
    public class DbHelper
    {
        //连接字符串
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static int ExecuteNonQuery(string sql, int cmdType, params SqlParameter[] paras)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmdType == 2)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();
                }
                return count;
            }

        }
        public static object ExecuteScalar(String sql, int cmdType, params SqlParameter[] paras)
        {
            object o = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmdType == 2)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                    conn.Open();
                    o = cmd.ExecuteScalar();
                    conn.Close();
                }
                return o;
            }
        }

        public static SqlDataReader ExecuteReader(String sql, int cmdType, params SqlParameter[] paras)
        {
            SqlDataReader dr = null;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (cmdType == 2)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            if (paras != null && paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
                try
                {
                    conn.Open();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    throw new Exception("查询出现异常", ex);
                }

            }
            return dr;
        }
        public static DataSet GetDataSet(String sql, int cmdType, params SqlParameter[] paras)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmdType == 2)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Open();
                da.Fill(ds);
                conn.Close();
            }
            return ds;
        }

        public static DataTable GetDataTable(String sql, int cmdType, params SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (cmdType == 2)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                if (paras != null && paras.Length > 0)
                {
                    cmd.Parameters.AddRange(paras);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                conn.Open();
                da.Fill(dt);
                conn.Close();
            }
            return dt;
        }

        public static bool ExecuteTrans(List<string> listSQL)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = trans;

                try
                {
                    for(int i = 0; i < listSQL.Count; i++)
                    {
                        cmd.CommandText = listSQL[i];
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch(SqlException ex)
                {
                    trans.Rollback();
                    throw new Exception("执行事务出现异常", ex);
                }
            }
        }
    }
}
