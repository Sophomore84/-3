using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL {
	 	//tb_DingDian
		public partial class tb_DingDian
	{
   		     
		private readonly Maticsoft.DAL.tb_DingDian dal=new Maticsoft.DAL.tb_DingDian();
		public tb_DingDian()
		{}
		
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string DingDianName,string X,string Y,string Z)
		{
			return dal.Exists(DingDianName,X,Y,Z);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void  Add(Maticsoft.Model.tb_DingDian model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.tb_DingDian model)
		{
			return dal.Update(model);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool UpdateRead(Maticsoft.Model.tb_DingDian model)
		{
			return dal.UpdateRead(model);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool UpdateRead(Maticsoft.Model.tb_KuWei model)
		{
			return dal.UpdateRead(model);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string DingDianName,string X,string Y,string Z)
		{
			
			return dal.Delete(DingDianName,X,Y,Z);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		//public Maticsoft.Model.tb_DingDian GetModel(string DingDianName,string X,string Y,string Z)
		public Maticsoft.Model.tb_DingDian GetModel(string DingDianName)
		{
			
			return dal.GetModel(DingDianName);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Maticsoft.Model.tb_DingDian GetModelByCache(string DingDianName,string X,string Y,string Z)
		{
			
			string CacheKey = "tb_DingDianModel-" + DingDianName+X+Y+Z;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					//objModel = dal.GetModel(DingDianName,X,Y,Z);
					objModel = dal.GetModel(DingDianName);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Maticsoft.Model.tb_DingDian)objModel;
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
		public List<string> GetLocationList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToLoca(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<string> DataTableToLoca(DataTable dt)
		{
			List<string> modelList = new List<string>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.tb_DingDian model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.tb_DingDian();
					model.DingDianName = dt.Rows[n]["DingDianName"].ToString();
					model.X = dt.Rows[n]["X"].ToString();
					model.Y = dt.Rows[n]["Y"].ToString();
					model.Z = dt.Rows[n]["Z"].ToString();
					model.Precision = dt.Rows[n]["Precision"].ToString();

					modelList.Add(dt.Rows[n]["DingDianName"].ToString());
				}
			}
			return modelList;
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.tb_DingDian> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.tb_DingDian> DataTableToList(DataTable dt)
		{
			List<Maticsoft.Model.tb_DingDian> modelList = new List<Maticsoft.Model.tb_DingDian>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.tb_DingDian model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.tb_DingDian();					
																	model.DingDianName= dt.Rows[n]["DingDianName"].ToString();
																																model.X= dt.Rows[n]["X"].ToString();
																																model.Y= dt.Rows[n]["Y"].ToString();
																																model.Z= dt.Rows[n]["Z"].ToString();
					model.Precision = dt.Rows[n]["Precision"].ToString();

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