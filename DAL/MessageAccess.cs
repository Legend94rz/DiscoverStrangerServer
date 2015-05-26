using MyWebService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyWebService.DAL
{
	public class MessageAccess
	{
		/// <summary>
		/// 获取用户未读消息并设为已读
		/// </summary>
		public static List<Message> GetUnreadMsg(string username)
		{
			string cmd = "select * from [Message] where ToId=@username and flag=0 order by sendTime";
			List<Message> ans = new List<Message>();
			SqlParameter[] p = new SqlParameter[]{
				new	SqlParameter("username",System.Data.SqlDbType.NVarChar,50),
			};
			p[0].Value = username;
			DataTable dt = MSSQLHelper.Query(cmd, p);
			if (dt.Rows.Count > 0)
			{
				string updateStr = "";
				foreach (DataRow dr in dt.Rows)
				{
					var m = DataRowToModel(dr);
					ans.Add(m);
					updateStr += generateSetReadCMD(m);
				}
				MSSQLHelper.Execute(updateStr);
			}
			return ans;
		}
		private static string generateSetReadCMD(Message m)
		{
			return "update [Message] set flag=1 where id='" + m.Id.ToString() + "'\n";
		}
		private static Message DataRowToModel(DataRow dr)
		{
			return new Message()
			{
				Id = Guid.Parse(dr["Id"].ToString()),
				flag = (byte)dr["flag"],
				FromId = (string)dr["FromId"],
				ToId = (string)dr["ToId"],
				SendTime = DateTime.Parse(dr["SendTime"].ToString()),
				Text = (string)dr["Text"],
				msgType = (byte)dr["msgType"],
			};
		}
		public static bool AddMessage(Message model)
		{
			string cmd = "insert into [Message] (fromid,toid,text,SendTime,msgType) VALUES (@fromid,@toid,@text,@Datetime,@msgType)";
			SqlParameter[] p = new SqlParameter[]{
				new SqlParameter("fromid",SqlDbType.NVarChar,50),
				new SqlParameter("toid",SqlDbType.NVarChar,50),
				new SqlParameter("text",SqlDbType.NVarChar),
				new SqlParameter("Datetime",SqlDbType.DateTime),
				new SqlParameter("msgType",SqlDbType.TinyInt),
			};
			p[0].Value = model.FromId;
			p[1].Value = model.ToId;
			p[2].Value = model.Text;
			p[3].Value = model.SendTime;
			p[4].Value = model.msgType;
			return MSSQLHelper.Execute(cmd, p) > 0;
		}
	}
}