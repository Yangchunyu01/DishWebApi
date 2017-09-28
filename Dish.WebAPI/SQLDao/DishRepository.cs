using Dish.DB;
using Dish.DB.Entity;
using Dish.WebAPI.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Web;

namespace Dish.WebAPI.SQLDao
{
    public class DishRepository
    {
        private static SQLiteHelper sqlHelper = new SQLiteHelper();

        public List<Lunch> QueryLunchDish_Classify1()
        {
            string day = DateTime.Now.Date.ToShortDateString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Lunch");
            strSql.Append(" where Date=@day");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@day",DbType.String,day)};
            List<Lunch> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new Lunch
            {
                Id = d[0].ToString(),
                DishName = d[1].ToString(),
                Claasify = d[2].ToString(),
                Price = d[3].ToString(),
                Date = d[4].ToString(),
                GoodValue = Convert.ToInt32(d[5]),
                BadValue = Convert.ToInt32(d[6]),
                CommentCount = Convert.ToInt32(d[7]),
                CreatDate = (DateTime)d[8]
            }).ToList<Lunch>();
            return list;
        }

        public List<Breakfast> QueryBreakfastDish()
        {
            string day = DateTime.Now.Date.ToShortDateString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Breakfast");
            strSql.Append(" where Date=@day");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@day",DbType.String,day)};
            List<Breakfast> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new Breakfast
            {
                Id = d[0].ToString(),
                DishName = d[1].ToString(),
                Price = d[2].ToString(),
                Date = d[3].ToString(),
                GoodValue = Convert.ToInt32(d[4]),
                BadValue = Convert.ToInt32(d[5]),
                CommentCount = Convert.ToInt32(d[6]),
                CreatDate = (DateTime)d[7]
            }).ToList<Breakfast>();
            return list;
        }

        public List<Dinner> QueryDinnerDish()
        {
            string day = DateTime.Now.Date.ToShortDateString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Dinner");
            strSql.Append(" where Date=@day");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@day",DbType.String,day)};
            List<Dinner> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new Dinner
            {
                Id = d[0].ToString(),
                DishName = d[1].ToString(),
                Price = d[2].ToString(),
                Date = d[3].ToString(),
                GoodValue = Convert.ToInt32(d[4]),
                BadValue = Convert.ToInt32(d[5]),
                CommentCount = Convert.ToInt32(d[6]),
                CreatDate = (DateTime)d[7]
            }).ToList<Dinner>();
            return list;
        }

        public bool UpdateDishFeedBack(string tableName, string column, string dishId)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format("update {0} set {1} = {1} + 1 where id = @dishId", tableName, column));
                SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@dishId",DbType.String,dishId)};
                if (sqlHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(string.Format("{0}", ex.Message));
                return false;
            }

        }

        public int SaveDishComments(string openId, string dishId, string nickName, string avatarUrl, string comments, string dishType)
        {
            int ret = 0;
            try
            {
                if (comments.Length > 0)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into Table_DishComments(");
                    strSql.Append("CommentId,DishId,UserOpenId,UserNickName,UserAvatarUrl,UserComments,CreateTime)");
                    strSql.Append(" values (");
                    strSql.Append("@CommentId,@DishId,@UserOpenId,@UserNickName,@UserAvatarUrl,@UserComments,@CreateTime)");
                    SQLiteParameter[] parameters = {
                        sqlHelper.MakeSQLiteParameter("@CommentId",DbType.String,Guid.NewGuid().ToString()),
                        sqlHelper.MakeSQLiteParameter("@DishId",DbType.String,dishId),
                        sqlHelper.MakeSQLiteParameter("@UserOpenId",DbType.String,openId),
                        sqlHelper.MakeSQLiteParameter("@UserNickName",DbType.String,nickName),
                        sqlHelper.MakeSQLiteParameter("@UserAvatarUrl",DbType.String,avatarUrl),
                        sqlHelper.MakeSQLiteParameter("@UserComments",DbType.String,comments),
                        sqlHelper.MakeSQLiteParameter("@CreateTime",DbType.String,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    };
                    if (sqlHelper.ExecuteSql(strSql.ToString(), parameters) >= 0)
                    {
                        ret = 1;
                    }
                    if (ret == 1)
                    {
                        string tableName = "Table_" + dishType;
                        UpdateDishFeedBack(tableName, "CommentCount", dishId);
                    }
                }
                return ret;
            }

            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(string.Format("{0}", ex.Message));
                return ret;
            }
        }

        public List<DishComments> GetBydishId(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_DishComments");
            strSql.Append(" where DishId=@id");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@id",DbType.String,id)};
            DataSet data = sqlHelper.Query(strSql.ToString(), parameters);
            if (data.Tables[0].Rows.Count > 0)
            {
                List<DishComments> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new DishComments
                {
                    CommentId = d[0].ToString(),
                    DishId = d[1].ToString(),
                    UserOpenId = d[2].ToString(),
                    UserNickName = d[3].ToString(),
                    UserAvatarUrl = d[4].ToString(),
                    UserComments = d[5].ToString(),
                    CreateTime = d[6].ToString()
                }).ToList<DishComments>();
                return list;
            }
            else
            {
                return new List<DishComments>();
            }
        }

        public List<Breakfast> GetBreakfastByRanking()
        {
            string day = DateTime.Now.Date.ToShortDateString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Breakfast");
            strSql.Append(" where Date=@day");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@day",DbType.String,day)};
            List<Breakfast> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new Breakfast()
            {
                Id = d[0].ToString(),
                DishName = d[1].ToString(),
                Price = d[2].ToString(),
                Date = d[3].ToString(),
                GoodValue = Convert.ToInt32(d[4]),
                BadValue = Convert.ToInt32(d[5]),
                CommentCount = Convert.ToInt32(d[6]),
                CreatDate = (DateTime)d[7]
            }).OrderByDescending(x=>x.GoodValue).Take(5).ToList<Breakfast>();
            return list;
        }

        public List<Dinner> GetDinnerByRanking()
        {
            string day = DateTime.Now.Date.ToShortDateString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Dinner");
            strSql.Append(" where Date=@day");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@day",DbType.String,day)};
            List<Dinner> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new Dinner
            {
                Id = d[0].ToString(),
                DishName = d[1].ToString(),
                Price = d[2].ToString(),
                Date = d[3].ToString(),
                GoodValue = Convert.ToInt32(d[4]),
                BadValue = Convert.ToInt32(d[5]),
                CommentCount = Convert.ToInt32(d[6]),
                CreatDate = (DateTime)d[7]
            }).OrderByDescending(x => x.GoodValue).Take(5).ToList<Dinner>();
            return list;
        }

        public List<Lunch> GetLunchByRanking()
        {
            string day = DateTime.Now.Date.ToShortDateString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Lunch");
            strSql.Append(" where Date=@day");
            SQLiteParameter[] parameters = {
                sqlHelper.MakeSQLiteParameter("@day",DbType.String,day)};
            List<Lunch> list = sqlHelper.Query(strSql.ToString(), parameters).Tables[0].AsEnumerable().Select(d => new Lunch
            {
                Id = d[0].ToString(),
                DishName = d[1].ToString(),
                Claasify = d[2].ToString(),
                Price = d[3].ToString(),
                Date = d[4].ToString(),
                GoodValue = Convert.ToInt32(d[5]),
                BadValue = Convert.ToInt32(d[6]),
                CommentCount = Convert.ToInt32(d[7]),
                CreatDate = (DateTime)d[8]
            }).OrderByDescending(x => x.GoodValue).Take(10).ToList<Lunch>();
            return list;
        }
    }
}