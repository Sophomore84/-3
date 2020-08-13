using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL {
	 	//LocationRecord
		public partial class LocationRecord
	{
   		     
		private readonly Maticsoft.DAL.LocationRecord dal=new Maticsoft.DAL.LocationRecord();
		public LocationRecord()
		{}
		
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName, string Weight)
		{
			return dal.Exists(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Weight);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void  Add(Maticsoft.Model.LocationRecord model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.LocationRecord model)
		{
			return dal.Update(model);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool UpdateByLocName(Maticsoft.Model.LocationRecord model, bool flag)
		{
			return dal.UpdateByLocName(model,flag);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			return dal.Delete(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName);
		}
		/// <summary>
		/// 删除一条数据根据库位
		/// </summary>
		public bool DeleteByLocationName(string LocationName,string time,bool flag)
		{

			return dal.DeleteByLocationName(LocationName,time,flag);
		}
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModel(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			return dal.GetModel(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName);
		}
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModel(string LocationName)
		{

			return dal.GetModel(LocationName);
		}
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByCache(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			string CacheKey = "LocationRecordModel-" + LocationName+GoodsName+GoodsCode+GoodsContract+GoodsNum+InTime+OutTime+UserName;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Maticsoft.Model.LocationRecord)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.LocationRecord> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 得到一个对象实体根据PictureBox
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByPicBox(string LocationName,bool flag)
		{
			return dal.GetModelByPicBox(LocationName,flag);
		}
		/// <summary>
		/// 得到一个对象实体根据PictureBox
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByPicBox(string LocationName)
		{
			return dal.GetModelByPicBox(LocationName);
		}
		/// <summary>
		/// 获得数据列表关于库位名
		/// </summary>
		public List<string> GetListLocName()
		{
			DataSet ds = dal.GetListLocName();
			return DataTableToListLocName(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表关于库位名是否有货
		/// </summary>
		public int GetLocNameNum(string strWhere)
		{
			
			return dal.GetLocNameNum(strWhere);
		}
		/// <summary>
		/// 获得数据列表关于库位名是否有货
		/// </summary>
		public int GetShelvesWeight(string strWhere)
		{

			return dal.GetShelvesWeight(strWhere);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.LocationRecord> DataTableToList(DataTable dt)
		{
			List<Maticsoft.Model.LocationRecord> modelList = new List<Maticsoft.Model.LocationRecord>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.LocationRecord model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.LocationRecord();					
																	model.LocationName= dt.Rows[n]["LocationName"].ToString();
																																model.GoodsName= dt.Rows[n]["GoodsName"].ToString();
																																model.GoodsCode= dt.Rows[n]["GoodsCode"].ToString();
																																model.GoodsContract= dt.Rows[n]["GoodsContract"].ToString();
																																model.GoodsNum= dt.Rows[n]["GoodsNum"].ToString();
																																model.InTime= dt.Rows[n]["InTime"].ToString();
																																model.OutTime= dt.Rows[n]["OutTime"].ToString();
																																model.UserName= dt.Rows[n]["UserName"].ToString();
					model.Action= dt.Rows[n]["Action"].ToString();


					modelList.Add(model);
				}
			}
			return modelList;
		}
		/// <summary>
		/// 获得数据列表关于库位名
		/// </summary>
		public List<string> DataTableToListLocName(DataTable dt)
		{
			List<string> modelList = new List<string>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				string model;
				for (int n = 0; n < rowsCount; n++)
				{
					//model = new Maticsoft.Model.LocationRecord();
					model = dt.Rows[n]["LocationName"].ToString();
					//model.GoodsName = dt.Rows[n]["GoodsName"].ToString();
					//model.GoodsCode = dt.Rows[n]["GoodsCode"].ToString();
					//model.GoodsContract = dt.Rows[n]["GoodsContract"].ToString();
					//model.GoodsNum = dt.Rows[n]["GoodsNum"].ToString();
					//model.InTime = dt.Rows[n]["InTime"].ToString();
					//model.OutTime = dt.Rows[n]["OutTime"].ToString();
					//model.UserName = dt.Rows[n]["UserName"].ToString();


					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion
   
	}
}