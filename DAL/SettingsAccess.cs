using MyWebService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyWebService.DAL
{
	public class SettingsAccess
	{
		private static MSSQLHelper helper;
		static SettingsAccess()
		{ 
			helper=new MSSQLHelper(GlobelConfig.Global.ExtendConnectString);
		}
		public static bool Exist(string username)
		{ 
			string cmd="select count(1) from [settings] where username=@username";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			p[0].Value=username;
			return (int)helper.QueryScalar(cmd,p)>0;
		}
		public static Settings Get(string username)
		{
			string cmd="select * from [settings] where username=@username";
			SqlParameter[] p = new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			p[0].Value = username;
			DataTable dt=helper.Query(cmd,p);
			if(dt.Rows.Count>0)
				return DataRowToModel(dt.Rows[0]);
			else
				return null;
		}

		private static Settings DataRowToModel(DataRow dataRow)
		{
			return new Settings(){
				username=(string)dataRow["username"],
				settings=(string)dataRow["settings"]
			};
		}
		public static bool SaveOrUpdate(Settings model)
		{ 
			string cmd;
			if(Exist(model.username))
				cmd="update [settings] set username=@username,settings=@settings";
			else
				cmd="insert into [settings] (username,settings) VALUES (@username,@settings)";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("settings",SqlDbType.NVarChar,-1)
			};
			p[0].Value=model.username;
			p[1].Value=model.settings;
			return helper.Execute(cmd,p)>0;
		}
	}
}