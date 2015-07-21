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
	public class PositionInfoAccess
	{
		private static MSSQLHelper helper;
		static PositionInfoAccess ()
		{
			helper = new MSSQLHelper(Global.ConnectString);
		}

		public static PositionInfo DataRowToModel(DataRow dr)
		{ 
			return new PositionInfo(){
				distance=((double)dr["distance"])*1000,
				longitude=(double)dr["longitude"],
				latitude=(double)dr["latitude"],
				strangerName=(string)dr["username"],
			};
		}
		/// <summary>
		/// 分页返回位置信息，包含startindex跟endindex
		/// </summary>
		public static List<PositionInfo> GetNearStranger(string oneperson,double atLatitude,double atLongitude,int startindex,int endindex)
		{ 
			List<PositionInfo> ans=new List<PositionInfo>();
			string cmd =String.Format( @"select * from(
							select ROW_NUMBER() over(order by TT.Distance) as RowNum,* from (
								select *,(ACOS(SIN((@Latitude * 3.1415) / 180 )*SIN((Latitude * 3.1415) / 180 )+COS((@Latitude * 3.1415) / 180 )*COS((Latitude * 3.1415) / 180 )*COS((@Longitude * 3.1415) / 180 - (Longitude * 3.1415) / 180 ))* 6380) as Distance from [PositionInfo]
								where Username<>@username
							)as TT
						)as TTT where (TTT.RowNum between {0} and {1})",startindex,endindex);
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50),
				new SqlParameter("latitude",SqlDbType.Float),
				new SqlParameter("longitude",SqlDbType.Float),
			};
			p[0].Value=oneperson;
			p[1].Value=atLatitude;
			p[2].Value=atLongitude;
			DataTable dt = helper.Query(cmd,p);
			if(dt.Rows.Count>0)
				foreach (DataRow dr in dt.Rows)
					ans.Add(DataRowToModel(dr));
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
			if(helper.Execute(cmd,p)>0)
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
			return ((int)helper.QueryScalar(cmd,p)>0);
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
			return helper.Execute(cmd,p)>0;
		}

		public static bool Delete(string username)
		{ 
			string cmd="delete from [positionInfo] where username=@username";
			SqlParameter[] p=new SqlParameter[]{
				new SqlParameter("username",SqlDbType.NVarChar,50)
			};
			p[0].Value=username;
			return helper.Execute(cmd,p)>0;
		}
	}
}