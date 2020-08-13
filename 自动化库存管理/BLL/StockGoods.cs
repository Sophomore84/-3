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
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			return dal.Exists(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void  Add(Maticsoft.Model.StockGoods model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(Maticsoft.Model.StockGoods model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
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
		/// ��������б���ڿ�λ���Ƿ��л�
		/// </summary>
		public int GetShelvesWeight(string strWhere)
		{

			return dal.GetShelvesWeight(strWhere);
		}
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Maticsoft.Model.StockGoods GetModel(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			
			return dal.GetModel(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
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
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<Maticsoft.Model.StockGoods> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
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
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion
   
	}
}