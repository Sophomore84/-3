using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Maticsoft.DAL
{
	/// <summary>
	/// 数据访问类:tb_UserLoginRecords
	/// </summary>
	public partial class tb_UserLoginRecords
	{
		public tb_UserLoginRecords()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Maticsoft.Model.tb_UserLoginRecords model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_UserLoginRecords(");
			strSql.Append("UserID,Action,DateTime,WorkType)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@Action,@DateTime,@WorkType)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,50),
					new SqlParameter("@Action", SqlDbType.VarChar,10),
					new SqlParameter("@DateTime", SqlDbType.VarChar,50),
					new SqlParameter("@WorkType", SqlDbType.VarChar,50)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Action;
			parameters[2].Value = model.DateTime;
			parameters[3].Value = model.WorkType;

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
		public bool Update(Maticsoft.Model.tb_UserLoginRecords model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_UserLoginRecords set ");
			strSql.Append("UserID=@UserID,");
			strSql.Append("Action=@Action,");
			strSql.Append("DateTime=@DateTime,");
			strSql.Append("WorkType=@WorkType");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,50),
					new SqlParameter("@Action", SqlDbType.VarChar,10),
					new SqlParameter("@DateTime", SqlDbType.VarChar,50),
					new SqlParameter("@WorkType", SqlDbType.VarChar,50)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Action;
			parameters[2].Value = model.DateTime;
			parameters[3].Value = model.WorkType;

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
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_UserLoginRecords ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

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
		public Maticsoft.Model.tb_UserLoginRecords GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserID,Action,DateTime,WorkType from tb_UserLoginRecords ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Maticsoft.Model.tb_UserLoginRecords model=new Maticsoft.Model.tb_UserLoginRecords();
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
		public Maticsoft.Model.tb_UserLoginRecords DataRowToModel(DataRow row)
		{
			Maticsoft.Model.tb_UserLoginRecords model=new Maticsoft.Model.tb_UserLoginRecords();
			if (row != null)
			{
				if(row["UserID"]!=null)
				{
					model.UserID=row["UserID"].ToString();
				}
				if(row["Action"]!=null)
				{
					model.Action=row["Action"].ToString();
				}
				if(row["DateTime"]!=null)
				{
					model.DateTime=row["DateTime"].ToString();
				}
				if(row["WorkType"]!=null)
				{
					model.WorkType=row["WorkType"].ToString();
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
			strSql.Append("select UserID,Action,DateTime,WorkType ");
			strSql.Append(" FROM tb_UserLoginRecords ");
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
			strSql.Append(" UserID,Action,DateTime,WorkType ");
			strSql.Append(" FROM tb_UserLoginRecords ");
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
			strSql.Append("select count(1) FROM tb_UserLoginRecords ");
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
				strSql.Append("order by T.XuHao desc");
			}
			strSql.Append(")AS Row, T.*  from tb_UserLoginRecords T ");
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
			parameters[0].Value = "tb_UserLoginRecords";
			parameters[1].Value = "XuHao";
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

