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
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName, string Weight)
		{
			return dal.Exists(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Weight);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void  Add(Maticsoft.Model.LocationRecord model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(Maticsoft.Model.LocationRecord model)
		{
			return dal.Update(model);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public bool UpdateByLocName(Maticsoft.Model.LocationRecord model, bool flag)
		{
			return dal.UpdateByLocName(model,flag);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			return dal.Delete(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName);
		}
		/// <summary>
		/// ɾ��һ�����ݸ��ݿ�λ
		/// </summary>
		public bool DeleteByLocationName(string LocationName,string time,bool flag)
		{

			return dal.DeleteByLocationName(LocationName,time,flag);
		}
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModel(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			return dal.GetModel(LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName);
		}
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModel(string LocationName)
		{

			return dal.GetModel(LocationName);
		}
		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
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
		public List<Maticsoft.Model.LocationRecord> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// �õ�һ������ʵ�����PictureBox
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByPicBox(string LocationName,bool flag)
		{
			return dal.GetModelByPicBox(LocationName,flag);
		}
		/// <summary>
		/// �õ�һ������ʵ�����PictureBox
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByPicBox(string LocationName)
		{
			return dal.GetModelByPicBox(LocationName);
		}
		/// <summary>
		/// ��������б���ڿ�λ��
		/// </summary>
		public List<string> GetListLocName()
		{
			DataSet ds = dal.GetListLocName();
			return DataTableToListLocName(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б���ڿ�λ���Ƿ��л�
		/// </summary>
		public int GetLocNameNum(string strWhere)
		{
			
			return dal.GetLocNameNum(strWhere);
		}
		/// <summary>
		/// ��������б���ڿ�λ���Ƿ��л�
		/// </summary>
		public int GetShelvesWeight(string strWhere)
		{

			return dal.GetShelvesWeight(strWhere);
		}
		/// <summary>
		/// ��������б�
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
		/// ��������б���ڿ�λ��
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
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion
   
	}
}