using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.DBUtility;
namespace Maticsoft.DAL  
{
	 	//StockGoods
		public partial class StockGoods
	{
   		     
		public bool Exists(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from StockGoods");
			strSql.Append(" where ");
			                                       strSql.Append(" LocationName = @LocationName and  ");
                                                                   strSql.Append(" GoodsName = @GoodsName and  ");
                                                                   strSql.Append(" GoodsCode = @GoodsCode and  ");
                                                                   strSql.Append(" GoodsContract = @GoodsContract and  ");
                                                                   strSql.Append(" GoodsNum = @GoodsNum and  ");
                                                                   strSql.Append(" InTime = @InTime and  ");
                                                                   strSql.Append(" OutTime = @OutTime and  ");
                                                                   strSql.Append(" UserName = @UserName and  ");
                                                                   strSql.Append(" Action = @Action and  ");
                                                                   strSql.Append(" Weight = @Weight  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Action", SqlDbType.VarChar,50),
					new SqlParameter("@Weight", SqlDbType.VarChar,50)			};
			parameters[0].Value = LocationName;
			parameters[1].Value = GoodsName;
			parameters[2].Value = GoodsCode;
			parameters[3].Value = GoodsContract;
			parameters[4].Value = GoodsNum;
			parameters[5].Value = InTime;
			parameters[6].Value = OutTime;
			parameters[7].Value = UserName;
			parameters[8].Value = Action;
			parameters[9].Value = Weight;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Maticsoft.Model.StockGoods model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into StockGoods(");			
            strSql.Append("LocationName,Weight,GoodsName,GoodsCode,GoodsContract,GoodsNum,InTime,OutTime,UserName,Action");
			strSql.Append(") values (");
            strSql.Append("@LocationName,@Weight,@GoodsName,@GoodsCode,@GoodsContract,@GoodsNum,@InTime,@OutTime,@UserName,@Action");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@LocationName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Weight", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsContract", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsNum", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@InTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OutTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@UserName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Action", SqlDbType.VarChar,50)             
              
            };
			            
            parameters[0].Value = model.LocationName;                        
            parameters[1].Value = model.Weight;                        
            parameters[2].Value = model.GoodsName;                        
            parameters[3].Value = model.GoodsCode;                        
            parameters[4].Value = model.GoodsContract;                        
            parameters[5].Value = model.GoodsNum;                        
            parameters[6].Value = model.InTime;                        
            parameters[7].Value = model.OutTime;                        
            parameters[8].Value = model.UserName;                        
            parameters[9].Value = model.Action;                        
			            DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.StockGoods model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update StockGoods set ");
			                        
            strSql.Append(" LocationName = @LocationName , ");                                    
            strSql.Append(" Weight = @Weight , ");                                    
            strSql.Append(" GoodsName = @GoodsName , ");                                    
            strSql.Append(" GoodsCode = @GoodsCode , ");                                    
            strSql.Append(" GoodsContract = @GoodsContract , ");                                    
            strSql.Append(" GoodsNum = @GoodsNum , ");                                    
            strSql.Append(" InTime = @InTime , ");                                    
            strSql.Append(" OutTime = @OutTime , ");                                    
            strSql.Append(" UserName = @UserName , ");                                    
            strSql.Append(" Action = @Action  ");            			
			strSql.Append(" where LocationName=@LocationName and GoodsName=@GoodsName and GoodsCode=@GoodsCode and GoodsContract=@GoodsContract and GoodsNum=@GoodsNum and InTime=@InTime and OutTime=@OutTime and UserName=@UserName and Action=@Action and Weight=@Weight  ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@LocationName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Weight", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsContract", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@GoodsNum", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@InTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OutTime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@UserName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Action", SqlDbType.VarChar,50)             
              
            };
						            
            parameters[0].Value = model.LocationName;                        
            parameters[1].Value = model.Weight;                        
            parameters[2].Value = model.GoodsName;                        
            parameters[3].Value = model.GoodsCode;                        
            parameters[4].Value = model.GoodsContract;                        
            parameters[5].Value = model.GoodsNum;                        
            parameters[6].Value = model.InTime;                        
            parameters[7].Value = model.OutTime;                        
            parameters[8].Value = model.UserName;                        
            parameters[9].Value = model.Action;                        
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
		/// 删除一条数据
		/// </summary>
		public bool Delete(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from StockGoods ");
			strSql.Append(" where LocationName=@LocationName and GoodsName=@GoodsName and GoodsCode=@GoodsCode and GoodsContract=@GoodsContract and GoodsNum=@GoodsNum and InTime=@InTime and OutTime=@OutTime and UserName=@UserName and Action=@Action and Weight=@Weight ");
						SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Action", SqlDbType.VarChar,50),
					new SqlParameter("@Weight", SqlDbType.VarChar,50)			};
			parameters[0].Value = LocationName;
			parameters[1].Value = GoodsName;
			parameters[2].Value = GoodsCode;
			parameters[3].Value = GoodsContract;
			parameters[4].Value = GoodsNum;
			parameters[5].Value = InTime;
			parameters[6].Value = OutTime;
			parameters[7].Value = UserName;
			parameters[8].Value = Action;
			parameters[9].Value = Weight;


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
		/// 删除全部数据
		/// </summary>
		public bool DeleteAll()
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("TRUNCATE table StockGoods ");
			


			int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
				strSql.Append(" where LEFT(LocationName,1) ='" + strWhere + "'");
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

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.StockGoods GetModel(string LocationName,string GoodsName,string GoodsCode,string GoodsContract,string GoodsNum,string InTime,string OutTime,string UserName,string Action,string Weight)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select LocationName, Weight, GoodsName, GoodsCode, GoodsContract, GoodsNum, InTime, OutTime, UserName, Action  ");			
			strSql.Append("  from StockGoods ");
			strSql.Append(" where LocationName=@LocationName and GoodsName=@GoodsName and GoodsCode=@GoodsCode and GoodsContract=@GoodsContract and GoodsNum=@GoodsNum and InTime=@InTime and OutTime=@OutTime and UserName=@UserName and Action=@Action and Weight=@Weight ");
						SqlParameter[] parameters = {
					new SqlParameter("@LocationName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsContract", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.VarChar,50),
					new SqlParameter("@InTime", SqlDbType.VarChar,50),
					new SqlParameter("@OutTime", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@Action", SqlDbType.VarChar,50),
					new SqlParameter("@Weight", SqlDbType.VarChar,50)			};
			parameters[0].Value = LocationName;
			parameters[1].Value = GoodsName;
			parameters[2].Value = GoodsCode;
			parameters[3].Value = GoodsContract;
			parameters[4].Value = GoodsNum;
			parameters[5].Value = InTime;
			parameters[6].Value = OutTime;
			parameters[7].Value = UserName;
			parameters[8].Value = Action;
			parameters[9].Value = Weight;

			
			Maticsoft.Model.StockGoods model=new Maticsoft.Model.StockGoods();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.LocationName= ds.Tables[0].Rows[0]["LocationName"].ToString();
																																model.Weight= ds.Tables[0].Rows[0]["Weight"].ToString();
																																model.GoodsName= ds.Tables[0].Rows[0]["GoodsName"].ToString();
																																model.GoodsCode= ds.Tables[0].Rows[0]["GoodsCode"].ToString();
																																model.GoodsContract= ds.Tables[0].Rows[0]["GoodsContract"].ToString();
																																model.GoodsNum= ds.Tables[0].Rows[0]["GoodsNum"].ToString();
																																model.InTime= ds.Tables[0].Rows[0]["InTime"].ToString();
																																model.OutTime= ds.Tables[0].Rows[0]["OutTime"].ToString();
																																model.UserName= ds.Tables[0].Rows[0]["UserName"].ToString();
																																model.Action= ds.Tables[0].Rows[0]["Action"].ToString();
																										
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
			strSql.Append(" FROM StockGoods ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
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
			strSql.Append(" FROM StockGoods ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

   
	}
}

