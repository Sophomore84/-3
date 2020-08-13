using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL {
	 	//StockGoods
		public partial class StockGoods
	{
   		     
		private readonly Maticsoft.DAL.StockGoods dal=new Maticsoft.DAL.StockGoods();
		public StockGoods()
		{}
		
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			return dal.Exists(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void  Add(Maticsoft.Model.StockGoods model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.StockGoods model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			
			return dal.Delete(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight);
		}
		public bool DeleteAll()
		{

			return dal.DeleteAll();
		}
		/// <summary>
		/// 获得数据列表关于库位名是否有货
		/// </summary>
		public int GetShelvesWeight(string strWhere)
		{

			return dal.GetShelvesWeight(strWhere);
		}
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.StockGoods GetModel(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			
			return dal.GetModel(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Maticsoft.Model.StockGoods GetModelByCache(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			
			string CacheKey = "StockGoodsModel-" + LocationName+GoodsName+GoodsCode+GoodsContract+GoodsNum+InTime+OutTime+UserName+Action+Weight;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Maticsoft.Model.StockGoods)objModel;
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
		public List<Maticsoft.Model.StockGoods> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.StockGoods> DataTableToList(DataTable dt)
		{
			List<Maticsoft.Model.StockGoods> modelList = new List<Maticsoft.Model.StockGoods>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.StockGoods model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.StockGoods();					
																	model.LocationName= dt.Rows[n]["LocationName"].ToString();
																																model.Weight= dt.Rows[n]["Weight"].ToString();
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion
   
	}
}