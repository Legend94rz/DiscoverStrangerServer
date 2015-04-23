using MyWebService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyWebService.DAL
{
	public class PositionInfoAccess
	{
		public static PositionInfo DataRowToModel(DataRow dr)
		{ 
			return new PositionInfo(){
				distance=(double)dr["distance"],
				longitude=(double)dr["longitude"],
				latitude=(double)dr["latitude"],
				strangerName=(string)dr["username"],
			};
		}
		public static List<PositionInfo> GetNearStranger(string oneperson,double atLatitude,double atLongitude)
		{ 
			List<PositionInfo> ans=new List<PositionInfo>();
			string cmd = @"select top 10 *,(ACOS(SIN((@Latitude * 3.1415) / 180 )*SIN((Latitude * 3.1415) / 180 )+COS((@Latitude * 3.1415) / 180 )*COS((Latitude * 3.1415) / 180 )*COS((@Longitude * 3.1415) / 180 - (Longitude * 3.1415) / 180 ))* 6380) as Distance from [PositionInfo] where	[Username]<>@username order by Distance";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("latitude",SqlDbType.Float),
				new SqlParameter("longitude",SqlDbType.Float),
			};
			p[0].Value=oneperson;
			p[1].Value=atLatitude;
			p[2].Value=atLatitude;
			DataTable dt = MSSQLHelper.Query(cmd,p);
			if(dt.Rows.Count>0)
				foreach (DataRow dr in dt.Rows)
				{
					ans.Add(DataRowToModel(dr));
				}
			return ans;
		}
		public static bool updatePositionInfo(PositionInfo model)
		{
			string cmd = "update [PositionInfo] set Latitude=@latitude,Longitude=@longitude where username=@username";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("latitude",SqlDbType.Float),
				new SqlParameter("longitude",SqlDbType.Float),
				new SqlParameter("username",SqlDbType.NVarChar,50),
			};
			p[0].Value=model.latitude;
			p[1].Value=model.longitude;
			p[2].Value=model.strangerName;
			if(MSSQLHelper.Execute(cmd,p)>0)
				return true;
			return false;
		}
		public static bool Exist(string name)
		{
			string cmd = "select count(1) from [PositionInfo] where username=@name";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("name",SqlDbType.NVarChar,50),
			};
			p[0].Value=name;
			return ((int)MSSQLHelper.QueryScalar(cmd,p)>0);
		}

		public static bool Add(PositionInfo model)
		{
			string cmd = "insert into [PositionInfo] VALUES (@username,@latitude,@longitude)";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("latitude",SqlDbType.Float),
				new SqlParameter("longitude",SqlDbType.Float),
			};
			p[0].Value=model.strangerName;
			p[1].Value=model.latitude;
			p[2].Value=model.longitude;
			return MSSQLHelper.Execute(cmd,p)>0;
		}
	}
}