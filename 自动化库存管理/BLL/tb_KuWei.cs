using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL {
	 	//tb_KuWei
		public partial class tb_KuWei
	{
   		     
		private readonly Maticsoft.DAL.tb_KuWei dal=new Maticsoft.DAL.tb_KuWei();
		public tb_KuWei()
		{}
		
		#region  Method
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(string KuWeiName,string X,string Y,string Z)
		{
			return dal.Exists(KuWeiName,X,Y,Z);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void  Add(Maticsoft.Model.tb_KuWei model)
		{
						dal.Add(model);
						
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(Maticsoft.Model.tb_KuWei model)
		{
			return dal.Update(model);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public bool UpdateRead(Maticsoft.Model.tb_KuWei model)
		{
			return dal.UpdateRead(model);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(string KuWeiName,string X,string Y,string Z)
		{
			
			return dal.Delete(KuWeiName,X,Y,Z);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		//public Maticsoft.Model.tb_KuWei GetModel(string KuWeiName,string X,string Y,string Z)
		public Maticsoft.Model.tb_KuWei GetModel(string KuWeiName)
		{

			//return dal.GetModel(KuWeiName,X,Y,Z);
			return dal.GetModel(KuWeiName);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public Maticsoft.Model.tb_KuWei GetModelByCache(string KuWeiName,string X,string Y,string Z)
		{
			
			string CacheKey = "tb_KuWeiModel-" + KuWeiName+X+Y+Z;
			object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					//objModel = dal.GetModel(KuWeiName,X,Y,Z);
					objModel = dal.GetModel(KuWeiName);
					if (objModel != null)
					{
						int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Maticsoft.Model.tb_KuWei)objModel;
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
		public List<Maticsoft.Model.tb_KuWei> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<string> GetLocationList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToLoca(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<string> DataTableToLoca(DataTable dt)
		{
			List<string> modelList = new List<string>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.tb_KuWei model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.tb_KuWei();
					model.KuWeiName = dt.Rows[n]["KuWeiName"].ToString();
					model.X = dt.Rows[n]["X"].ToString();
					model.Y = dt.Rows[n]["Y"].ToString();
					model.Z = dt.Rows[n]["Z"].ToString();
					model.Precision = dt.Rows[n]["Precision"].ToString();

					modelList.Add(dt.Rows[n]["KuWeiName"].ToString());
				}
			}
			return modelList;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public List<Maticsoft.Model.tb_KuWei> DataTableToList(DataTable dt)
		{
			List<Maticsoft.Model.tb_KuWei> modelList = new List<Maticsoft.Model.tb_KuWei>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.tb_KuWei model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.tb_KuWei();					
																	model.KuWeiName= dt.Rows[n]["KuWeiName"].ToString();
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
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion
   
	}
}