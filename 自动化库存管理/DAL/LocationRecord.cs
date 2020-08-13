using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.DBUtility;
namespace Maticsoft.DAL  
{
	 	//LocationRecord
		public partial class LocationRecord
	{
		/// <summary>
		/// 调库数据库操作
		/// </summary>
		/// <param name="model1">出</param>
		/// <param name="model2">入</param>
		/// <param name="model3">中间库位出入</param>
		public void ExKuWei(Maticsoft.Model.LocationRecord model1, Maticsoft.Model.LocationRecord model2, Maticsoft.Model.LocationRecord model3)
		{
			Add(model1);//添加出库，action为调库
			Add(model2);//添加入库，action为调库
		}
		/// <summary>
		/// 获得库位上是否有货物
		/// </summary>
		/// <param name="strWhere"></param>
		/// <returns></returns>
   		public int GetLocNameNum(string strWhere)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("select SUM(Convert(int,GoodsNum))");
			strSql.Append(" FROM LocationRecord ");
			if (strWhere.Trim() != "")
			{
				strSql.Append(" where LocationName ='" + strWhere+"'");
			}
			Object obj= DbHelperSQL.GetSingle(strSql.ToString());
			//DbHelperSQL.Query(strSql.ToString());
			if(obj==null)
			{
				return 0;
			}
			else
			{
				return (int)obj;
			}
			//return (int)DbHelperSQL.GetSingle(strSql.ToString());
		}
		/// <summary>
		/// 获得货架上货物重量
		/// </summary>
		/// <param name="strWhere"></param>
		/// <returns></returns>
		public int GetShelvesWeight(string strWhere)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("select SUM(Convert(int,Weight))");
			strSql.Append(" FROM StockGoods ");
			if (strWhere.Trim() != "")
			{
				strSql.Append(" where left(LocationName,1) ='" + strWhere + "'");
			}
			Object obj = DbHelperSQL.GetSingle(strSql.ToString());
			//DbHelperSQL.Query(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return (int)obj;
			}
			//return (int)DbHelperSQL.GetSingle(strSql.ToString());
		}
		public bool Exists(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Weight)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from LocationRecord");
			strSql.Append(" where ");
			                                       strSql.Append(" LocationName = @LocationName and  ");
                                                                   strSql.Append(" GoodsName = @GoodsName and  ");
                                                                   strSql.Append(" GoodsCode = @GoodsCode and  ");
                                                                   strSql.Append(" GoodsContract = @GoodsContract and  ");
                                                                   strSql.Append(" GoodsNum = @GoodsNum and  ");
                                                                   strSql.Append(" InTime = @InTime and  ");
                                                                   strSql.Append(" OutTime = @OutTime and  ");
                                                                   strSql.Append(" UserName = @UserName  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
			new SqlParameter("@Weight", SqlDbType.VarChar,50)
			};
			parameters[0].Value = LocationName;
			parameters[1].Value = GoodsName;
			parameters[2].Value = GoodsCode;
			parameters[3].Value = GoodsContract;
			parameters[4].Value = GoodsNum;
			parameters[5].Value = InTime;
			parameters[6].Value = OutTime;
			parameters[7].Value = UserName;
			parameters[8].Value = Weight;
			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Maticsoft.Model.LocationRecord model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into LocationRecord(");			
            strSql.Append("LocationName,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action,Weight");
			strSql.Append(") values (");
            strSql.Append("@LocationName,@GoodsName,@GoodsCode,@GoodsContract,@GoodsNum,@InTime,@OutTime,@UserName,@Action,@Weight");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@LocationName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsContract", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsNum", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@InTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OutTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@UserName", SqlDbType.VarChar,50),
						new SqlParameter("@Action", SqlDbType.VarChar,50),
						new SqlParameter("@Weight", SqlDbType.VarChar,50)

			};
			            
            parameters[0].Value = model.LocationName;                        
            parameters[1].Value = model.GoodsName;                        
            parameters[2].Value = model.GoodsCode;                        
            parameters[3].Value = model.GoodsContract;                        
            parameters[4].Value = model.GoodsNum;                        
            parameters[5].Value = model.InTime;                        
            parameters[6].Value = model.OutTime;                        
            parameters[7].Value = model.UserName;
			parameters[8].Value = model.Action;
			parameters[9].Value = model.Weight;
			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.LocationRecord model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update LocationRecord set ");
			                        
            strSql.Append(" LocationName = @LocationName , ");                                    
            strSql.Append(" GoodsName = @GoodsName , ");                                    
            strSql.Append(" GoodsCode = @GoodsCode , ");                                    
            strSql.Append(" GoodsContract = @GoodsContract , ");                                    
            strSql.Append(" GoodsNum = @GoodsNum , ");                                    
            strSql.Append(" InTime = @InTime , ");                                    
            strSql.Append(" OutTime = @OutTime , ");                                    
            strSql.Append(" UserName = @UserName , ");
			strSql.Append(" Action = @Action,  ");
			strSql.Append(" Weight = @Weight  ");
			strSql.Append(" where LocationName=@LocationName and GoodsName=@GoodsName and GoodsCode=@GoodsCode and GoodsContract=@GoodsContract and GoodsNum=@GoodsNum and InTime=@InTime and OutTime=@OutTime and UserName=@UserName and Action=@Action and Weight=@Weight ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@LocationName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsContract", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsNum", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@InTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OutTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@UserName", SqlDbType.VarChar,50) ,
						new SqlParameter("@Action", SqlDbType.VarChar,50),
						new SqlParameter("@Weight", SqlDbType.VarChar,50),
			};
						            
            parameters[0].Value = model.LocationName;                        
            parameters[1].Value = model.GoodsName;                        
            parameters[2].Value = model.GoodsCode;                        
            parameters[3].Value = model.GoodsContract;                        
            parameters[4].Value = model.GoodsNum;                        
            parameters[5].Value = model.InTime;                        
            parameters[6].Value = model.OutTime;                        
            parameters[7].Value = model.UserName;
			parameters[8].Value = model.Action;
			parameters[8].Value = model.Weight;
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 更新一条数据根据库位名
		/// </summary>
		public bool UpdateByLocName(Maticsoft.Model.LocationRecord model,bool flag)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update LocationRecord set ");

			//strSql.Append(" LocationName = @LocationName , ");
			strSql.Append(" GoodsName = @GoodsName , ");
			strSql.Append(" GoodsCode = @GoodsCode , ");
			strSql.Append(" GoodsContract = @GoodsContract , ");
			strSql.Append(" GoodsNum = @GoodsNum , ");
			//strSql.Append(" InTime = @InTime , ");
			//strSql.Append(" OutTime = @OutTime , ");
			strSql.Append(" UserName = @UserName , ");
			strSql.Append(" Action = @Action  ");
			strSql.Append(" Weight = @Weight  ");
			strSql.Append(" where LocationName=@LocationName ");
			if (flag)
			{
				strSql.Append(" and OutTime=@OutTime ");
			}
			else
			{
				strSql.Append(" and InTime=@InTime ");
			}

			SqlParameter[] parameters = {
						new SqlParameter("@LocationName", SqlDbType.VarChar,50) ,
						new SqlParameter("@GoodsName", SqlDbType.VarChar,50) ,
						new SqlParameter("@GoodsCode", SqlDbType.VarChar,50) ,
						new SqlParameter("@GoodsContract", SqlDbType.VarChar,50) ,
						new SqlParameter("@GoodsNum", SqlDbType.VarChar,50) ,
						new SqlParameter("@InTime", SqlDbType.VarChar,50) ,
						new SqlParameter("@OutTime", SqlDbType.VarChar,50) ,
						new SqlParameter("@UserName", SqlDbType.VarChar,50),
						new SqlParameter("@Action", SqlDbType.VarChar,50),
						new SqlParameter("@Weight", SqlDbType.VarChar,50)

			};

			parameters[0].Value = model.LocationName;
			parameters[1].Value = model.GoodsName;
			parameters[2].Value = model.GoodsCode;
			parameters[3].Value = model.GoodsContract;
			parameters[4].Value = model.GoodsNum;
			parameters[5].Value = model.InTime;
			parameters[6].Value = model.OutTime;
			parameters[7].Value = model.UserName;
			parameters[8].Value = model.Action;
			parameters[9].Value = model.Weight;
			int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LocationRecord ");
			strSql.Append(" where LocationName=@LocationName and GoodsName=@GoodsName and GoodsCode=@GoodsCode and GoodsContract=@GoodsContract and GoodsNum=@GoodsNum and InTime=@InTime and OutTime=@OutTime and UserName=@UserName ");
						SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50)			};
			parameters[0].Value = LocationName;
			parameters[1].Value = GoodsName;
			parameters[2].Value = GoodsCode;
			parameters[3].Value = GoodsContract;
			parameters[4].Value = GoodsNum;
			parameters[5].Value = InTime;
			parameters[6].Value = OutTime;
			parameters[7].Value = UserName;


			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 删除一条数据根据库位
		/// </summary>
		public bool DeleteByLocationName(string LocationName,string time,bool flag)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("delete from LocationRecord ");

			strSql.Append(" where LocationName=@LocationName ");
			if (flag)
			{
				strSql.Append(" and OutTime=@OutTime ");
			}
			else
			{
				strSql.Append(" and InTime=@InTime ");
			}
			
			SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),

							};
			parameters[0].Value = LocationName;
			parameters[1].Value = time;
			parameters[2].Value = time;

			int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModel(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select LocationName, GoodsName, GoodsCode, GoodsContract, GoodsNum, InTime, OutTime, UserName  ");			
			strSql.Append("  from LocationRecord ");
			strSql.Append(" where LocationName=@LocationName and GoodsName=@GoodsName and GoodsCode=@GoodsCode and GoodsContract=@GoodsContract and GoodsNum=@GoodsNum and InTime=@InTime and OutTime=@OutTime and UserName=@UserName ");
						SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50)			};
			parameters[0].Value = LocationName;
			parameters[1].Value = GoodsName;
			parameters[2].Value = GoodsCode;
			parameters[3].Value = GoodsContract;
			parameters[4].Value = GoodsNum;
			parameters[5].Value = InTime;
			parameters[6].Value = OutTime;
			parameters[7].Value = UserName;

			
			Maticsoft.Model.LocationRecord model=new Maticsoft.Model.LocationRecord();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.LocationName= ds.Tables[0].Rows[0]["LocationName"].ToString();
																																model.GoodsName= ds.Tables[0].Rows[0]["GoodsName"].ToString();
																																model.GoodsCode= ds.Tables[0].Rows[0]["GoodsCode"].ToString();
																																model.GoodsContract= ds.Tables[0].Rows[0]["GoodsContract"].ToString();
																																model.GoodsNum= ds.Tables[0].Rows[0]["GoodsNum"].ToString();
																																model.InTime= ds.Tables[0].Rows[0]["InTime"].ToString();
																																model.OutTime= ds.Tables[0].Rows[0]["OutTime"].ToString();
																																model.UserName= ds.Tables[0].Rows[0]["UserName"].ToString();
																										
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 得到一个对象实体根据PictureBox
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByPicBox(string LocationName,bool flag)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("select Top 1 * ");
			strSql.Append("  from LocationRecord ");
			strSql.Append(" where LocationName=@LocationName order by ");
			if (flag)
			{
				strSql.Append(" cast(InTime as datetime) desc ");
			}
			else
			{
				strSql.Append("  cast(OutTime as datetime) desc ");
			}
			SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					//new SqlParameter("@InTime", SqlDbType.VarChar,50),
					//new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					//new SqlParameter("@UserName", SqlDbType.VarChar,50)         
			};
			parameters[0].Value = LocationName;
			//parameters[1].Value = GoodsName;
			//parameters[2].Value = GoodsCode;
			//parameters[3].Value = GoodsContract;
			//parameters[4].Value = GoodsNum;
			//parameters[5].Value = InTime;
			//parameters[6].Value = OutTime;
			//parameters[7].Value = UserName;


			Maticsoft.Model.LocationRecord model = new Maticsoft.Model.LocationRecord();
			DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

			if (ds.Tables[0].Rows.Count > 0)
			{
				model.LocationName = ds.Tables[0].Rows[0]["LocationName"].ToString();
				model.GoodsName = ds.Tables[0].Rows[0]["GoodsName"].ToString();
				model.GoodsCode = ds.Tables[0].Rows[0]["GoodsCode"].ToString();
				model.GoodsContract = ds.Tables[0].Rows[0]["GoodsContract"].ToString();
				model.GoodsNum = ds.Tables[0].Rows[0]["GoodsNum"].ToString();
				model.InTime = ds.Tables[0].Rows[0]["InTime"].ToString();
				model.OutTime = ds.Tables[0].Rows[0]["OutTime"].ToString();
				model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
				model.Action= ds.Tables[0].Rows[0]["Action"].ToString();
				model.Weight= ds.Tables[0].Rows[0]["Weight"].ToString();

				return model;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 得到一个对象实体根据PictureBox
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModelByPicBox(string LocationName)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("select LocationName, GoodsName, GoodsCode, GoodsContract, GoodsNum, InTime, OutTime, UserName  ");
			strSql.Append("  from LocationRecord ");
			strSql.Append(" where LocationName=@LocationName ");
		
			SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					//new SqlParameter("@InTime", SqlDbType.VarChar,50),
					//new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					//new SqlParameter("@UserName", SqlDbType.VarChar,50)         
			};
			parameters[0].Value = LocationName;
			//parameters[1].Value = GoodsName;
			//parameters[2].Value = GoodsCode;
			//parameters[3].Value = GoodsContract;
			//parameters[4].Value = GoodsNum;
			//parameters[5].Value = InTime;
			//parameters[6].Value = OutTime;
			//parameters[7].Value = UserName;


			Maticsoft.Model.LocationRecord model = new Maticsoft.Model.LocationRecord();
			DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

			if (ds.Tables[0].Rows.Count > 0)
			{
				model.LocationName = ds.Tables[0].Rows[0]["LocationName"].ToString();
				model.GoodsName = ds.Tables[0].Rows[0]["GoodsName"].ToString();
				model.GoodsCode = ds.Tables[0].Rows[0]["GoodsCode"].ToString();
				model.GoodsContract = ds.Tables[0].Rows[0]["GoodsContract"].ToString();
				model.GoodsNum = ds.Tables[0].Rows[0]["GoodsNum"].ToString();
				model.InTime = ds.Tables[0].Rows[0]["InTime"].ToString();
				model.OutTime = ds.Tables[0].Rows[0]["OutTime"].ToString();
				model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();

				return model;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 得到一个对象实体根据库位名
		/// </summary>
		public Maticsoft.Model.LocationRecord GetModel(string LocationName)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("select TOP 1 LocationName, GoodsName, GoodsCode, GoodsContract, GoodsNum, InTime, OutTime, UserName  ");
			strSql.Append("  from LocationRecord ");
			strSql.Append(" where LocationName=@LocationName ");
			strSql.Append(" order by InTime desc ");
			SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					//new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					//new SqlParameter("@InTime", SqlDbType.VarChar,50),
					//new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					//new SqlParameter("@UserName", SqlDbType.VarChar,50)         
			};
			parameters[0].Value = LocationName;
			//parameters[1].Value = GoodsName;
			//parameters[2].Value = GoodsCode;
			//parameters[3].Value = GoodsContract;
			//parameters[4].Value = GoodsNum;
			//parameters[5].Value = InTime;
			//parameters[6].Value = OutTime;
			//parameters[7].Value = UserName;


			Maticsoft.Model.LocationRecord model = new Maticsoft.Model.LocationRecord();
			DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

			if (ds.Tables[0].Rows.Count > 0)
			{
				model.LocationName = ds.Tables[0].Rows[0]["LocationName"].ToString();
				model.GoodsName = ds.Tables[0].Rows[0]["GoodsName"].ToString();
				model.GoodsCode = ds.Tables[0].Rows[0]["GoodsCode"].ToString();
				model.GoodsContract = ds.Tables[0].Rows[0]["GoodsContract"].ToString();
				model.GoodsNum = ds.Tables[0].Rows[0]["GoodsNum"].ToString();
				model.InTime = ds.Tables[0].Rows[0]["InTime"].ToString();
				model.OutTime = ds.Tables[0].Rows[0]["OutTime"].ToString();
				model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();

				return model;
			}
			else
			{
				return null;
			}
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * ");
			strSql.Append(" FROM LocationRecord ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}
        /// <summary>
        /// 获得数据列表关于库位名
        /// </summary>
        //public DataSet GetListLocName(string strWhere)
        public DataSet GetListLocName()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT LocationName ");
            strSql.Append(" FROM LocationRecord ");
            //if (strWhere.Trim() != "")
            //{
            //	strSql.Append(" where " + strWhere);
            //}
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" * ");
			strSql.Append(" FROM LocationRecord ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

   
	}
}

