using MyWebService.GlobelConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace MyWebService.DAL
{
	public class MSSQLHelper
	{
		private string connStr;
		public MSSQLHelper(string connStr)
		{
			this.connStr=connStr;
		}
		private void _AddParams(SqlCommand cmd, SqlParameter[] Params)
		{
			if (Params != null)
				foreach (var param in Params)
					cmd.Parameters.Add(param);
		}
		public DataTable Query(string selCmd, SqlParameter[] Params = null)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				SqlCommand _selCmd = new SqlCommand(selCmd, conn);
				_AddParams(_selCmd, Params);
				conn.Open();
				SqlDataAdapter adp = new SqlDataAdapter(_selCmd);
				DataTable dt = new DataTable();
				adp.Fill(dt);
				return dt;
			}
		}
		public int Execute(string cmd, SqlParameter[] Params = null)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				SqlCommand _cmd = new SqlCommand(cmd, conn);
				_AddParams(_cmd, Params);
				conn.Open();
				return _cmd.ExecuteNonQuery();
			}
		}
		public object QueryScalar(string cmd,SqlParameter[] Params=null)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				SqlCommand _cmd = new SqlCommand(cmd, conn);
				_AddParams(_cmd, Params);
				conn.Open();
				return _cmd.ExecuteScalar();
			}
		}
	}
}
