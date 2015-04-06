using MyWebService.DAL;
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
		public static Friend GetFriend(string username)
		{
			string cmd = "SELECT * FROM [Friend] WHERE username=@username";
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
		private static Friend dataRowToModel(DataRow dr)
		{
			return new Friend()
			{
				username=(string)dr["username"],
				friendList=(string)dr["friendList"],
			};
		}
		public static void Update(Friend model)
		{
			string cmd = "UPDATE [UserInfo] SET password=@password,friendList=@friendList WHERE username=@username";
			SqlParameter[] Params = new SqlParameter[]
			{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("friendList",SqlDbType.NVarChar),
			};
			Params[0].Value = model.username;
			Params[1].Value = model.friendList;
			int r = MSSQLHelper.Execute(cmd, Params);
		}
	}
}
