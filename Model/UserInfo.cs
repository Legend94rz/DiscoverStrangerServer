using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebService.Model
{
	public class UserInfo
	{
		public string username { get;set;}
		public string password { get;set;}
		/// <summary>
		/// 0-离线;1-已登录
		/// </summary>
		public byte state { get;set;}
	}
}
