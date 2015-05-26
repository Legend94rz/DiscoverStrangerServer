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
using System.IO;
using Server.Model;
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
		[WebMethod(Description = "[登录]")]
		public bool SignIn(string name, string pass)
		{
			var model = UserInfoAccess.GetDataBy(name);
			if (model != null && model.password == pass)
			{
				model.state = Global.State.ONLINE;
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
		[WebMethod(Description = "[注册]必须先检验用户名是否符合规定,惟一性")]
		public string SignUp(string name, string pass, bool sex, string nickName)
		{
			if (UserInfoAccess.GetDataBy(name) != null) return Global.ERROR_EXISTED_USER;
			UserInfo model = new UserInfo()
			{
				username = name,
				password = pass,
				sex = sex,
				nickName = nickName
			};
			if (UserInfoAccess.AddData(model))
				return Global.OPT_SUCCEED;
			else
				return Global.ERROR_UNEXCEPT;
		}
		[WebMethod(Description = "[勿用]目前只删除userInfo表，与参数无关")]
		public int DeleteAll(string tabelName)
		{
			return UserInfoAccess.DeleteAll();
		}
		[WebMethod(Description = "废弃")]
		public string GetIp()
		{
			var str = HttpContext.Current.Request.ServerVariables.GetValues("REMOTE_ADDR");
			return str[0];
		}
		[WebMethod]
		public bool pushMsg(string from, string to, string msg, string time, byte msgType)
		{
			Message model = new Message()
			{
				FromId = from,
				ToId = to,
				Text = msg,
				SendTime = DateTime.Parse(time),
				msgType = msgType
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
			var friend = FriendAccess.GetFriend(name);

			return friend.friendList;
		}
		[WebMethod]
		public bool updatePosition(string name, float latitude, float longitude)
		{
			if (PositionInfoAccess.Exist(name))
			{
				return PositionInfoAccess.updatePositionInfo(new PositionInfo()
				{
					strangerName = name,
					latitude = latitude,
					longitude = longitude,
				});
			}
			else
			{
				PositionInfo model = new PositionInfo()
				{
					strangerName = name,
					latitude = latitude,
					longitude = longitude,
				};
				return PositionInfoAccess.Add(model);
			}
		}
		[WebMethod]
		public List<PositionInfo> getNearStranger(string name, double latitude, double longitude)
		{
			return PositionInfoAccess.GetNearStranger(name, latitude, longitude);
		}
		[WebMethod]
		public bool uploadFile(string base64, string path, string fileName,int blockSerial=0)
		{
			bool flag = false;
			byte[] file = Convert.FromBase64String(base64);
			try
			{
				path = Server.MapPath(path);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				MemoryStream m = new MemoryStream(file);
				FileStream fs;
				if(blockSerial==0)
					fs = new FileStream(path + @"\" + fileName, FileMode.Create);
				else
					fs=new FileStream(path + @"\" + fileName, FileMode.Append);
				m.WriteTo(fs);
				m.Close();
				fs.Close();
				m = null;
				fs = null;
				flag = true;
			}
			catch (Exception e)
			{
				flag = false;
			}
			return flag;
		}
		[WebMethod]
		public byte[] downloadFile(string path, string fileName)
		{
			FileStream fs;
			string CurrentUploadFolderPath = Server.MapPath(path);
			string CurrentUploadFilePath = CurrentUploadFolderPath + @"\" + fileName;
			try
			{
				if (File.Exists(CurrentUploadFilePath))
				{
					fs = File.OpenRead(CurrentUploadFilePath);
					int b1;
					MemoryStream ms = new MemoryStream();
					while ((b1 = fs.ReadByte()) != -1)
					{
						ms.WriteByte((byte)b1);
					}
					return ms.ToArray();
				}
				else
					return new byte[0];
			}
			catch (Exception e)
			{
				return new byte[0];
			}
		}
		[WebMethod]
		public byte[] downloadFileByBlock(string path,string fileName,long blockSize,int blockSerial)
		{
			FileStream fs;
			string CurrentUploadFolderPath = Server.MapPath(path);
			string CurrentUploadFilePath = CurrentUploadFolderPath + @"\" + fileName;
			try
			{
				if (File.Exists(CurrentUploadFilePath))
				{
					fs = File.OpenRead(CurrentUploadFilePath);
					fs.Seek(blockSize*blockSerial,SeekOrigin.Begin);
					MemoryStream ms = new MemoryStream();
					for(int i=0;i<blockSize;i++)
					{ 
						int b1=fs.ReadByte();
						if(b1==-1)break;
						ms.WriteByte((byte)b1);
					}
					return ms.ToArray();
				}
				else
					return new byte[0];
			}
			catch (Exception)
			{
				return new byte[0];
			}
		}
		[WebMethod]
		public long getFileSize(string path,string fileName)
		{
			FileStream fs;
			string FolderPath = Server.MapPath(path);
			string FilePath = FolderPath + @"\" + fileName;
			try { 
				if(File.Exists(FilePath))
				{ 
					fs=File.OpenRead(FilePath);
					return fs.Length;
				}
				else
				{
					return 0;
				}
			}
			catch(Exception)
			{ 
				return 0;
			}
		}
		[WebMethod]
		public bool updateFriendList(string name, string friendList)
		{
			try
			{
				if (!FriendAccess.isExists(name))
				{
					return FriendAccess.Add(new Friend()
					{
						friendList = friendList,
						username = name,
					});
				}
				return FriendAccess.Update(new Friend()
				{
					friendList = friendList,
					username = name,
				});
			}
			catch (System.Data.SqlClient.SqlException e)
			{
				return false;
			}
		}
		[WebMethod]
		public List<Shake> getShakes(string name,string time)
		{
			ShakeAccess.DelOverdueData(DateTime.Parse(time));
			return ShakeAccess.GetShakes(name,DateTime.Parse(time));
		}
		[WebMethod]
		public bool addShake(string name,string time)
		{
			var model=new Shake(){
				id=Guid.NewGuid(),
				username=name,
				shakeTime=DateTime.Parse(time),
			};
			if(ShakeAccess.Exist(name))
				return ShakeAccess.update(model);
			return ShakeAccess.Add(model);
		}

	}

	/*Todo 修改密码*/
}
