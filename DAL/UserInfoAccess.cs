﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWebService.GlobelConfig;
using MyWebService.Model;
using System.Data.SqlClient;
using System.Data;
namespace MyWebService.DAL
{
	public class UserInfoAccess
	{
		private static UserInfo dataRowToModel(DataRow dr)
		{
			return new UserInfo()
			{
				username = (string)dr["username"],
				password = (string)dr["password"],
				state = (byte)dr["state"],
				sex = (bool)dr["sex"],
				nickName = (string)dr["nickName"],
			};
		}
		public static UserInfo GetDataBy(string username)
		{
			string cmd = "SELECT * FROM [UserInfo] WHERE username=@username";
			SqlParameter[] Params = new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			Params[0].Value = username;
			DataTable dt = MSSQLHelper.Query(cmd, Params);
			if (dt.Rows.Count > 0)
			{
				return dataRowToModel(dt.Rows[0]);
			}
			return null;
		}
		public static bool AddData(UserInfo model)
		{
			string cmd = "INSERT INTO [UserInfo] VALUES (@username,@password,@state,@sex,@nickName)";
			SqlParameter[] Params = new SqlParameter[]
			{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("password",SqlDbType.NVarChar),
				new SqlParameter("state",SqlDbType.Int),
				new SqlParameter("sex",SqlDbType.Bit),
				new SqlParameter("nickName",SqlDbType.NVarChar,50)
			};
			Params[0].Value = model.username;
			Params[1].Value = model.password;
			Params[2].Value = model.state;
			Params[3].Value = model.sex;
			Params[4].Value = model.nickName;
			int r = MSSQLHelper.Execute(cmd, Params);
			return r > 0;
		}
		public static bool Update(UserInfo model)
		{
			string cmd = "UPDATE [UserInfo] SET password=@password,state=@state,nickName=@nickName,sex=@sex WHERE username=@username";
			SqlParameter[] Params = new SqlParameter[]
			{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("password",SqlDbType.NVarChar),
				new SqlParameter("state",SqlDbType.Int),
				new SqlParameter("nickName",SqlDbType.NVarChar,50),
				new SqlParameter("sex",SqlDbType.Bit),
			};
			Params[0].Value = model.username;
			Params[1].Value = model.password;
			Params[2].Value = model.state;
			Params[3].Value = model.nickName;
			Params[4].Value = model.sex;
			int r = MSSQLHelper.Execute(cmd, Params);
			return r > 0;
		}
		public static int DeleteAll()
		{
			string cmd = "Delete from [UserInfo]";
			return MSSQLHelper.Execute(cmd);
		}
	}
}
