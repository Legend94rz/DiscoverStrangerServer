using MyWebService.GlobelConfig;
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
		private static MSSQLHelper helper;
		static MessageAccess ()
		{
			helper = new MSSQLHelper(Global.ConnectString);
		}

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
			DataTable dt = helper.Query(cmd, p);
			if (dt.Rows.Count > 0)
			{
				string updateStr = "";
				foreach (DataRow dr in dt.Rows)
				{
					var m = DataRowToModel(dr);
					ans.Add(m);
					updateStr += generateSetReadCMD(m);
				}
				helper.Execute(updateStr);
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
			return helper.Execute(cmd, p) > 0;
		}
		public static int GetCount(string where)
		{ 
			string cmd="select count(1) from [Message] where " + where;
			return (int)helper.QueryScalar(cmd);
		}
		//不包括 startIndex 跟 endIndex 这两条记录
		public static List<Message> GetDataByPage(string where,int startIndex,int endIndex)
		{ 
			string cmd=String.Format( "select * from ( Select ROW_NUMBER() OVER (order by T.SendTime) AS Row,T.* from [Message] T where {0} ) TT where TT.Row between {1} and {2}",where,startIndex+1,endIndex-1);
			DataTable dt=helper.Query(cmd);
			List<Message> ans=new List<Message>();
			if(dt.Rows.Count>0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					ans.Add(DataRowToModel(dr));
				}
			}
			return ans;
		}
	}
}