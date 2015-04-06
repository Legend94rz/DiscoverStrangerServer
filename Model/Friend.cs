using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
	public class Friend
	{
		public string username { get;set;}
		/// <summary>
		/// 这一项是一个json格式的字符串。
		/// </summary>
		public string friendList { get;set;}
	}
}
