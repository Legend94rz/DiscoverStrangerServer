using MyWebService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyWebService.DAL
{
	public class ShakeAccess
	{
		private static Shake DataRowToModel(DataRow dr)
		{
			return new Shake()
			{
				id = (Guid)dr["id"],
				shakeTime = DateTime.Parse(dr["shakeTime"].ToString()),
				username = (string)dr["username"],
			};
		}
		public static bool Exist(string name)
		{ 
			string cmd="select count(1) from [shake] where username=@username";
			SqlParameter[] p=new  SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			p[0].Value=name;
			return (int)MSSQLHelper.QueryScalar(cmd,p)>0;
		}
		public static bool update(Shake model)
		{ 
			string cmd="update [shake] set shaketime=@shaketime where username=@username";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("shaketime",SqlDbType.DateTime)
			};
			p[0].Value=model.username;
			p[1].Value=model.shakeTime;
			return MSSQLHelper.Execute(cmd,p)>0;
		}
		public static bool Add(Shake model)
		{
			string cmd = "INSERT INTO [Shake] (id,username,shakeTime) VALUES (@id,@username,@shaketime)";
			SqlParameter[] p = new SqlParameter[]{
				new SqlParameter("id",SqlDbType.UniqueIdentifier,16),
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("shakeTime",SqlDbType.DateTime),
			};
			p[0].Value = model.id;
			p[1].Value = model.username;
			p[2].Value = model.shakeTime;
			return MSSQLHelper.Execute(cmd, p) > 0;
		}
		public static void DelOverdueData(DateTime n)
		{
			TimeSpan t = new TimeSpan(0, 1, 0);
			n = n - t;
			string cmd = "delete from [Shake] where shakeTime<@now";
			SqlParameter[] p = new SqlParameter[]{
				new SqlParameter("now",SqlDbType.DateTime)
			};
			p[0].Value = n;
			MSSQLHelper.Execute(cmd, p);
		}
		public static List<Shake> GetShakes(string name, DateTime shakeTime)
		{
			TimeSpan t = new TimeSpan(0, 0, 30);
			List<Shake> ans = new List<Shake>();
			string cmd = "select top 10 * from [shake] where shakeTime>=@old and shakeTime<=@now and username<>@name";
			SqlParameter[] p = new SqlParameter[]{
				new SqlParameter("old",SqlDbType.DateTime),
				new SqlParameter("now",SqlDbType.DateTime),
				new SqlParameter("name",SqlDbType.NVarChar,50)
			};
			p[0].Value = shakeTime - t;
			p[1].Value = shakeTime + t;
			p[2].Value = name;
			DataTable dt = MSSQLHelper.Query(cmd, p);
			if (dt.Rows.Count > 0)
				foreach (DataRow dr in dt.Rows)
				{
					ans.Add(DataRowToModel(dr));
				}
			return ans;
		}
	}
}