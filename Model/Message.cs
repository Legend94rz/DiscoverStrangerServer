using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebService.Model
{
	public class Message
	{
		public Guid Id { get; set; }
		public string FromId { get; set; }
		public string ToId { get; set; }
		public string Text { get; set; }
		public DateTime SendTime { get; set; }
		/// <summary>
		/// 消息状态：0 - 未读;1 - 已读
		/// </summary>
		public byte flag { get; set; }
	}
}