using MyWebService.DAL;
using MyWebService.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MyWebService.GlobelConfig;
using Server.DAL;
namespace MyWebService
{
	/// <summary>
	/// _default 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://legendsService/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
	// [System.Web.Script.Services.ScriptService]
	public class _default : System.Web.Services.WebService
	{
		[WebMethod]
		public string HelloWorld()
		{
			return "Hello World";
		}
		[WebMethod(Description="[登录]")]
		public bool SignIn(string name,string pass)
		{
			var model=UserInfoAccess.GetDataBy(name);
			if(model!=null && model.password==pass)
			{
				model.state=Global.State.ONLINE;
				UserInfoAccess.Update(model);
				return true;
			}
			return false;
		}
		[WebMethod]
		public UserInfo GetUser(string name)
		{ 
			return UserInfoAccess.GetDataBy(name);
		}
		[WebMethod(Description="[注册]必须先检验用户名是否符合规定,惟一性")]
		public string SignUp(string name,string pass,string headImgPath)
		{
			if(UserInfoAccess.GetDataBy(name)!=null) return Global.ERROR_EXISTED_USER;
			UserInfo model=new UserInfo()
			{
				username=name,
				password=pass,
				headImgPath=headImgPath,
			};
			if( UserInfoAccess.AddData(model))
				return Global.OPT_SUCCEED;
			else
				return Global.ERROR_UNEXCEPT;
		}
		[WebMethod(Description="目前只删除userInfo表，与参数无关")]
		public int DeleteAll(string tabelName)
		{
			return UserInfoAccess.DeleteAll();
		}
		[WebMethod]
		public string GetIp()
		{
			var str = HttpContext.Current.Request.ServerVariables.GetValues("REMOTE_ADDR");
			return str[0];
		}
		[WebMethod]
		public bool pushMsg(string from,string to,string msg)
		{
			Message model=new Message()
			{
				FromId=from,
				ToId=to,
				Text=msg,
			};
			return MessageAccess.AddMessage(model);
		}
		[WebMethod]
		public List<Message> pullMsg(string name)
		{ 
			return MessageAccess.GetUnreadMsg(name);
		}
		[WebMethod]
		public string getFriends(string name)
		{ 
			var friend=FriendAccess.GetFriend(name);
			return friend.friendList;
		}
	}
	/*Todo 修改密码、修改头像、上传文件、下载文件*/
}
