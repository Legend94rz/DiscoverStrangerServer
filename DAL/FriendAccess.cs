using MyWebService.DAL;
using MyWebService.GlobelConfig;
using Server.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL
{
	public class FriendAccess
	{
		private static MSSQLHelper helper;
		static FriendAccess ()
		{
			helper = new MSSQLHelper(Global.ConnectString);
		}
		public static Friend GetFriend(string username)
		{
			string cmd = "SELECT * FROM [Friend] WHERE username=@username";
			SqlParameter[] Params = new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			Params[0].Value = username;
			DataTable dt = helper.Query(cmd, Params);
			if (dt.Rows.Count > 0)
			{
				return dataRowToModel(dt.Rows[0]);
			}
			return new Friend();
		}
		public static bool isExists(String username)
		{ 
			string cmd="SELECT COUNT(1) FROM [Friend] WHERE username=@username";
			SqlParameter[] Params = new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			Params[0].Value = username;
			return (int)helper.QueryScalar(cmd,Params)>0;
		}
		private static Friend dataRowToModel(DataRow dr)
		{
			return new Friend()
			{
				username=(string)dr["username"],
				friendList=(string)dr["friendList"],
			};
		}
		public static bool Update(Friend model)
		{
			string cmd = "UPDATE [Friend] SET friendList=@friendList WHERE username=@username";
			SqlParameter[] Params = new SqlParameter[]
			{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("friendList",SqlDbType.NVarChar),
			};
			Params[0].Value = model.username;
			Params[1].Value = model.friendList;
			return helper.Execute(cmd, Params)>0;
		}
		public static bool Add(Friend model)
		{ 
			string cmd="INSERT INTO [Friend] (username,friendList) VALUES (@username,@friendList)";
			SqlParameter[] Params = new SqlParameter[]
			{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("friendList",SqlDbType.NVarChar),
			};
			Params[0].Value = model.username;
			Params[1].Value = model.friendList;
			return helper.Execute(cmd,Params)>0;
		}
	}
}
