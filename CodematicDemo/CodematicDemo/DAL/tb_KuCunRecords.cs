using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Maticsoft.DAL
{
	/// <summary>
	/// 数据访问类:tb_KuCunRecords
	/// </summary>
	public partial class tb_KuCunRecords
	{
		public tb_KuCunRecords()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string HuoJiaHao,string CengHao,string LieHao)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_KuCunRecords");
			strSql.Append(" where HuoJiaHao=@HuoJiaHao and CengHao=@CengHao and LieHao=@LieHao ");
			SqlParameter[] parameters = {
					new SqlParameter("@HuoJiaHao", SqlDbType.NChar,10),
					new SqlParameter("@CengHao", SqlDbType.NChar,10),
					new SqlParameter("@LieHao", SqlDbType.NChar,10)			};
			parameters[0].Value = HuoJiaHao;
			parameters[1].Value = CengHao;
			parameters[2].Value = LieHao;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Maticsoft.Model.tb_KuCunRecords model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_KuCunRecords(");
			strSql.Append("HuoJiaHao,CengHao,LieHao,BianMa,MingCheng,GuiGe,XingHao,HeTongHao,X,Y,Z,RuKuTime,ChuKuTime,CaoZuoYuan)");
			strSql.Append(" values (");
			strSql.Append("@HuoJiaHao,@CengHao,@LieHao,@BianMa,@MingCheng,@GuiGe,@XingHao,@HeTongHao,@X,@Y,@Z,@RuKuTime,@ChuKuTime,@CaoZuoYuan)");
			SqlParameter[] parameters = {
					new SqlParameter("@HuoJiaHao", SqlDbType.NChar,10),
					new SqlParameter("@CengHao", SqlDbType.NChar,10),
					new SqlParameter("@LieHao", SqlDbType.NChar,10),
					new SqlParameter("@BianMa", SqlDbType.NChar,50),
					new SqlParameter("@MingCheng", SqlDbType.NChar,50),
					new SqlParameter("@GuiGe", SqlDbType.NChar,50),
					new SqlParameter("@XingHao", SqlDbType.NChar,50),
					new SqlParameter("@HeTongHao", SqlDbType.NChar,50),
					new SqlParameter("@X", SqlDbType.NChar,50),
					new SqlParameter("@Y", SqlDbType.NChar,50),
					new SqlParameter("@Z", SqlDbType.NChar,50),
					new SqlParameter("@RuKuTime", SqlDbType.NChar,50),
					new SqlParameter("@ChuKuTime", SqlDbType.NChar,50),
					new SqlParameter("@CaoZuoYuan", SqlDbType.NChar,50)};
			parameters[0].Value = model.HuoJiaHao;
			parameters[1].Value = model.CengHao;
			parameters[2].Value = model.LieHao;
			parameters[3].Value = model.BianMa;
			parameters[4].Value = model.MingCheng;
			parameters[5].Value = model.GuiGe;
			parameters[6].Value = model.XingHao;
			parameters[7].Value = model.HeTongHao;
			parameters[8].Value = model.X;
			parameters[9].Value = model.Y;
			parameters[10].Value = model.Z;
			parameters[11].Value = model.RuKuTime;
			parameters[12].Value = model.ChuKuTime;
			parameters[13].Value = model.CaoZuoYuan;

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
		public bool Update(Maticsoft.Model.tb_KuCunRecords model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_KuCunRecords set ");
			strSql.Append("BianMa=@BianMa,");
			strSql.Append("MingCheng=@MingCheng,");
			strSql.Append("GuiGe=@GuiGe,");
			strSql.Append("XingHao=@XingHao,");
			strSql.Append("HeTongHao=@HeTongHao,");
			strSql.Append("X=@X,");
			strSql.Append("Y=@Y,");
			strSql.Append("Z=@Z,");
			strSql.Append("RuKuTime=@RuKuTime,");
			strSql.Append("ChuKuTime=@ChuKuTime,");
			strSql.Append("CaoZuoYuan=@CaoZuoYuan");
			strSql.Append(" where HuoJiaHao=@HuoJiaHao and CengHao=@CengHao and LieHao=@LieHao ");
			SqlParameter[] parameters = {
					new SqlParameter("@BianMa", SqlDbType.NChar,50),
					new SqlParameter("@MingCheng", SqlDbType.NChar,50),
					new SqlParameter("@GuiGe", SqlDbType.NChar,50),
					new SqlParameter("@XingHao", SqlDbType.NChar,50),
					new SqlParameter("@HeTongHao", SqlDbType.NChar,50),
					new SqlParameter("@X", SqlDbType.NChar,50),
					new SqlParameter("@Y", SqlDbType.NChar,50),
					new SqlParameter("@Z", SqlDbType.NChar,50),
					new SqlParameter("@RuKuTime", SqlDbType.NChar,50),
					new SqlParameter("@ChuKuTime", SqlDbType.NChar,50),
					new SqlParameter("@CaoZuoYuan", SqlDbType.NChar,50),
					new SqlParameter("@HuoJiaHao", SqlDbType.NChar,10),
					new SqlParameter("@CengHao", SqlDbType.NChar,10),
					new SqlParameter("@LieHao", SqlDbType.NChar,10)};
			parameters[0].Value = model.BianMa;
			parameters[1].Value = model.MingCheng;
			parameters[2].Value = model.GuiGe;
			parameters[3].Value = model.XingHao;
			parameters[4].Value = model.HeTongHao;
			parameters[5].Value = model.X;
			parameters[6].Value = model.Y;
			parameters[7].Value = model.Z;
			parameters[8].Value = model.RuKuTime;
			parameters[9].Value = model.ChuKuTime;
			parameters[10].Value = model.CaoZuoYuan;
			parameters[11].Value = model.HuoJiaHao;
			parameters[12].Value = model.CengHao;
			parameters[13].Value = model.LieHao;

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
		public bool Delete(string HuoJiaHao,string CengHao,string LieHao)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_KuCunRecords ");
			strSql.Append(" where HuoJiaHao=@HuoJiaHao and CengHao=@CengHao and LieHao=@LieHao ");
			SqlParameter[] parameters = {
					new SqlParameter("@HuoJiaHao", SqlDbType.NChar,10),
					new SqlParameter("@CengHao", SqlDbType.NChar,10),
					new SqlParameter("@LieHao", SqlDbType.NChar,10)			};
			parameters[0].Value = HuoJiaHao;
			parameters[1].Value = CengHao;
			parameters[2].Value = LieHao;

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
		public Maticsoft.Model.tb_KuCunRecords GetModel(string HuoJiaHao,string CengHao,string LieHao)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 HuoJiaHao,CengHao,LieHao,BianMa,MingCheng,GuiGe,XingHao,HeTongHao,X,Y,Z,RuKuTime,ChuKuTime,CaoZuoYuan from tb_KuCunRecords ");
			strSql.Append(" where HuoJiaHao=@HuoJiaHao and CengHao=@CengHao and LieHao=@LieHao ");
			SqlParameter[] parameters = {
					new SqlParameter("@HuoJiaHao", SqlDbType.NChar,10),
					new SqlParameter("@CengHao", SqlDbType.NChar,10),
					new SqlParameter("@LieHao", SqlDbType.NChar,10)			};
			parameters[0].Value = HuoJiaHao;
			parameters[1].Value = CengHao;
			parameters[2].Value = LieHao;

			Maticsoft.Model.tb_KuCunRecords model=new Maticsoft.Model.tb_KuCunRecords();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.tb_KuCunRecords DataRowToModel(DataRow row)
		{
			Maticsoft.Model.tb_KuCunRecords model=new Maticsoft.Model.tb_KuCunRecords();
			if (row != null)
			{
				if(row["HuoJiaHao"]!=null)
				{
					model.HuoJiaHao=row["HuoJiaHao"].ToString();
				}
				if(row["CengHao"]!=null)
				{
					model.CengHao=row["CengHao"].ToString();
				}
				if(row["LieHao"]!=null)
				{
					model.LieHao=row["LieHao"].ToString();
				}
				if(row["BianMa"]!=null)
				{
					model.BianMa=row["BianMa"].ToString();
				}
				if(row["MingCheng"]!=null)
				{
					model.MingCheng=row["MingCheng"].ToString();
				}
				if(row["GuiGe"]!=null)
				{
					model.GuiGe=row["GuiGe"].ToString();
				}
				if(row["XingHao"]!=null)
				{
					model.XingHao=row["XingHao"].ToString();
				}
				if(row["HeTongHao"]!=null)
				{
					model.HeTongHao=row["HeTongHao"].ToString();
				}
				if(row["X"]!=null)
				{
					model.X=row["X"].ToString();
				}
				if(row["Y"]!=null)
				{
					model.Y=row["Y"].ToString();
				}
				if(row["Z"]!=null)
				{
					model.Z=row["Z"].ToString();
				}
				if(row["RuKuTime"]!=null)
				{
					model.RuKuTime=row["RuKuTime"].ToString();
				}
				if(row["ChuKuTime"]!=null)
				{
					model.ChuKuTime=row["ChuKuTime"].ToString();
				}
				if(row["CaoZuoYuan"]!=null)
				{
					model.CaoZuoYuan=row["CaoZuoYuan"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select HuoJiaHao,CengHao,LieHao,BianMa,MingCheng,GuiGe,XingHao,HeTongHao,X,Y,Z,RuKuTime,ChuKuTime,CaoZuoYuan ");
			strSql.Append(" FROM tb_KuCunRecords ");
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
			strSql.Append(" HuoJiaHao,CengHao,LieHao,BianMa,MingCheng,GuiGe,XingHao,HeTongHao,X,Y,Z,RuKuTime,ChuKuTime,CaoZuoYuan ");
			strSql.Append(" FROM tb_KuCunRecords ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_KuCunRecords ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.LieHao desc");
			}
			strSql.Append(")AS Row, T.*  from tb_KuCunRecords T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "tb_KuCunRecords";
			parameters[1].Value = "LieHao";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

