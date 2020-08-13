using System; 
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Data;
using Maticsoft.DBUtility;
namespace Maticsoft.DAL  
{
	 	//tb_DingDian
		public partial class tb_DingDian
	{
   		     
		public bool Exists(string DingDianName,string X,string Y,string Z)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DingDian");
			strSql.Append(" where ");
			                                       strSql.Append(" DingDianName = @DingDianName and  ");
                                                                   strSql.Append(" X = @X and  ");
                                                                   strSql.Append(" Y = @Y and  ");
                                                                   strSql.Append(" Z = @Z  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@DingDianName", SqlDbType.VarChar,50),
					new SqlParameter("@X", SqlDbType.VarChar,50),
					new SqlParameter("@Y", SqlDbType.VarChar,50),
					new SqlParameter("@Z", SqlDbType.VarChar,50)			};
			parameters[0].Value = DingDianName;
			parameters[1].Value = X;
			parameters[2].Value = Y;
			parameters[3].Value = Z;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
		
				
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Maticsoft.Model.tb_DingDian model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DingDian(");			
            strSql.Append("DingDianName,X,Y,Z");
			strSql.Append(") values (");
            strSql.Append("@DingDianName,@X,@Y,@Z");            
            strSql.Append(") ");            
            		
			SqlParameter[] parameters = {
			            new SqlParameter("@DingDianName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@X", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Y", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Z", SqlDbType.VarChar,50)             
              
            };
			            
            parameters[0].Value = model.DingDianName;                        
            parameters[1].Value = model.X;                        
            parameters[2].Value = model.Y;                        
            parameters[3].Value = model.Z;                        
			            DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
            			
		}
		
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Maticsoft.Model.tb_DingDian model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DingDian set ");
			                        
            //strSql.Append(" DingDianName = @DingDianName , ");                                    
            strSql.Append(" X = @X , ");                                    
            strSql.Append(" Y = @Y , ");                                    
            strSql.Append(" Z = @Z , ");
			strSql.Append(" precision = @Precision ");
			//strSql.Append(" where DingDianName=@DingDianName and X=@X and Y=@Y and Z=@Z  ");
			strSql.Append(" where DingDianName=@DingDianName  ");
			SqlParameter[] parameters = {
			            new SqlParameter("@DingDianName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@X", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Y", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Z", SqlDbType.VarChar,50),
						new SqlParameter("@Precision", SqlDbType.VarChar,50)

			};
						            
            parameters[0].Value = model.DingDianName;                        
            parameters[1].Value = model.X;                        
            parameters[2].Value = model.Y;                        
            parameters[3].Value = model.Z;
			parameters[4].Value = model.Precision;
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
		/// 更新一条数据
		/// </summary>
		public bool UpdateRead(Maticsoft.Model.tb_DingDian model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update tb_DingDian set ");

			//strSql.Append(" DingDianName = @DingDianName , ");                                    
			strSql.Append(" X = @X , ");
			strSql.Append(" Y = @Y , ");
			strSql.Append(" Z = @Z  ");
			//strSql.Append(" precision = @Precision ");
			//strSql.Append(" where DingDianName=@DingDianName and X=@X and Y=@Y and Z=@Z  ");
			strSql.Append(" where DingDianName=@DingDianName  ");
			SqlParameter[] parameters = {
						new SqlParameter("@DingDianName", SqlDbType.VarChar,50) ,
						new SqlParameter("@X", SqlDbType.VarChar,50) ,
						new SqlParameter("@Y", SqlDbType.VarChar,50) ,
						new SqlParameter("@Z", SqlDbType.VarChar,50),
						//new SqlParameter("@Precision", SqlDbType.VarChar,50)

			};

			parameters[0].Value = model.DingDianName;
			parameters[1].Value = model.X;
			parameters[2].Value = model.Y;
			parameters[3].Value = model.Z;
			//parameters[4].Value = model.Precision;
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
		/// 更新一条数据
		/// </summary>
		public bool UpdateRead(Maticsoft.Model.tb_KuWei model)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update tb_DingDian set ");

			//strSql.Append(" KuWeiName = @KuWeiName , ");                                    
			strSql.Append(" X = @X , ");
			strSql.Append(" Y = @Y , ");
			strSql.Append(" Z = @Z  ");
			//strSql.Append(" precision = @Precision ");
			//strSql.Append(" where KuWeiName=@KuWeiName and X=@X and Y=@Y and Z=@Z  ");
			strSql.Append(" where DingDianName=@DingDianName ");
			SqlParameter[] parameters = {
						new SqlParameter("@DingDianName", SqlDbType.VarChar,50) ,
						new SqlParameter("@X", SqlDbType.VarChar,50) ,
						new SqlParameter("@Y", SqlDbType.VarChar,50) ,
						new SqlParameter("@Z", SqlDbType.VarChar,50)  ,
						//new SqlParameter("@Precision", SqlDbType.VarChar,50)

			};

			parameters[0].Value = model.KuWeiName;
			parameters[1].Value = model.X;
			parameters[2].Value = model.Y;
			parameters[3].Value = model.Z;
			//parameters[4].Value = model.Precision;
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
		public bool Delete(string DingDianName,string X,string Y,string Z)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DingDian ");
			strSql.Append(" where DingDianName=@DingDianName and X=@X and Y=@Y and Z=@Z ");
						SqlParameter[] parameters = {
					new SqlParameter("@DingDianName", SqlDbType.VarChar,50),
					new SqlParameter("@X", SqlDbType.VarChar,50),
					new SqlParameter("@Y", SqlDbType.VarChar,50),
					new SqlParameter("@Z", SqlDbType.VarChar,50)			};
			parameters[0].Value = DingDianName;
			parameters[1].Value = X;
			parameters[2].Value = Y;
			parameters[3].Value = Z;


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
		/// 得到一个对象实体
		/// </summary>
		//public Maticsoft.Model.tb_DingDian GetModel(string DingDianName,string X,string Y,string Z)
		public Maticsoft.Model.tb_DingDian GetModel(string DingDianName)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  * ");
			//strSql.Append("select DingDianName, X, Y, Z  ");			
			strSql.Append("  from tb_DingDian ");
			//strSql.Append(" where DingDianName=@DingDianName and X=@X and Y=@Y and Z=@Z ");
			strSql.Append(" where DingDianName=@DingDianName ");
			SqlParameter[] parameters = {
					new SqlParameter("@DingDianName", SqlDbType.VarChar,50)
					//,
					//new SqlParameter("@X", SqlDbType.VarChar,50),
					//new SqlParameter("@Y", SqlDbType.VarChar,50),
					//new SqlParameter("@Z", SqlDbType.VarChar,50)			
			};
			parameters[0].Value = DingDianName;
			//parameters[1].Value = X;
			//parameters[2].Value = Y;
			//parameters[3].Value = Z;

			
			Maticsoft.Model.tb_DingDian model=new Maticsoft.Model.tb_DingDian();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			
			if(ds.Tables[0].Rows.Count>0)
			{
																model.DingDianName= ds.Tables[0].Rows[0]["DingDianName"].ToString();
																																model.X= ds.Tables[0].Rows[0]["X"].ToString();
																																model.Y= ds.Tables[0].Rows[0]["Y"].ToString();
																																model.Z= ds.Tables[0].Rows[0]["Z"].ToString();
				model.Precision = ds.Tables[0].Rows[0]["Precision"].ToString();

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
			strSql.Append(" FROM tb_DingDian ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by DingDianName asc ");
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
			strSql.Append(" FROM tb_DingDian ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

   
	}
}

