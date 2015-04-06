using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MyWebService.GlobelConfig
{
	public class Global
	{
		public static string ConnectString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
		public const string OPT_SUCCEED = "操作成功";
		public const string ERROR_EXISTED_USER = "已存在的用户";
		public const string ERROR_STILL_ONLINE = "还未下线";
		public const string ERROR_UNEXCEPT = "未知的错误";
		public class Protocol
		{
			public const string A_NEW_MSG = "Msg";
			public const string FRIEND_LIST = "YourFriends";
		}
		public class State
		{ 
			public const int ONLINE=1;
			public const int OFFLINE=0;
		}
	}
}
