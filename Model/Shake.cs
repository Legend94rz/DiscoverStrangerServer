using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebService.Model
{
	public class Shake
	{
		public Guid id { get; set; }
		public string username { get; set; }
		public DateTime shakeTime { get; set; }
	}
}