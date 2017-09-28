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
    public class UserRepository
    {
        public int SaveUserInfo(UserInfo user)
        {
            int ret = 0;
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Table_UserInfo(");
            strSql.Append("UserId,OpenId,UnionId,NickName,AvatarUrl,Gender,Province,City,Country,IsMsEmp,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@OpenId,@UnionId,@NickName,@AvatarUrl,@Gender,@Province,@City,@Country,@IsMsEmp,@CreateTime)");
            SQLiteParameter[] parameters = {
                sh.MakeSQLiteParameter("@UserId",DbType.String,user.UserId),
                sh.MakeSQLiteParameter("@OpenId",DbType.String,user.OpenId),
                sh.MakeSQLiteParameter("@UnionId",DbType.String,user.UnionId),
                sh.MakeSQLiteParameter("@NickName",DbType.String,user.NickName),
                sh.MakeSQLiteParameter("@AvatarUrl",DbType.String,user.AvatarUrl),
                sh.MakeSQLiteParameter("@Gender",DbType.String,user.Gender),
                sh.MakeSQLiteParameter("@Province",DbType.String,user.Province),
                sh.MakeSQLiteParameter("@City",DbType.String,user.City),
                sh.MakeSQLiteParameter("@Country",DbType.String,user.Country),
                sh.MakeSQLiteParameter("@IsMsEmp",DbType.String,user.IsMsEmp),
                sh.MakeSQLiteParameter("@CreateTime",DbType.DateTime,user.CreateTime),
                };
            if (sh.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                ret = 1;
            }
            return ret;
        }

        public bool QueryUserIsMsEmp(string openId)
        {
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select IsMsEmp from Table_UserInfo");
            strSql.Append(" where OpenId = @openId");
            SQLiteParameter[] parameters = {
                sh.MakeSQLiteParameter("OpenId",DbType.String,openId)};
            DataSet data = sh.Query(strSql.ToString(), parameters);
            string res = data.Tables[0].Rows[0].ItemArray[0].ToString();
            if (res.Equals("1"))
            {
                return true;
            }
            return false;
        }

        public bool UpdateUserInfoByOpenId(string openId)
        {
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update Table_UserInfo set IsMsEmp = 1 where OpenId = @OpenId");
            SQLiteParameter[] parameters = {
                sh.MakeSQLiteParameter("@OpenId",DbType.String,openId)};
            if (sh.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool QueryUserByOpenId(string openId)
        {
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_UserInfo");
            strSql.Append(" where OpenId = @openId");
            SQLiteParameter[] parameters = {
                sh.MakeSQLiteParameter("OpenId",DbType.String,openId)};
            DataSet data = sh.Query(strSql.ToString(), parameters);
            if (data.Tables[0].Rows.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Comments> GetAll()
        {
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Table_Comments");
            List<Comments> list = sh.Query(strSql.ToString()).Tables[0].AsEnumerable().Select(s => new Comments
            {
                CommentId = s[0].ToString(),
                UserOpenId = s[1].ToString(),
                UserNickName = s[2].ToString(),
                UserAvatarUrl = s[3].ToString(),
                UserComments = s[4].ToString(),
                CreateTime = DateTime.Parse(s[5].ToString()).ToString("yyyy-MM-dd")
            }).ToList<Comments>();
            return list;
        }

        public int SaveUserComments(string openId, string nickName, string avatarUrl, string comments)
        {
            int ret = 0;
            try
            {
                SQLiteHelper sh = new SQLiteHelper();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Table_Comments(");
                strSql.Append("CommentId,UserOpenId,UserNickName,UserAvatarUrl,UserComments,CreateTime)");
                strSql.Append(" values (");
                strSql.Append("@CommentId,@UserOpenId,@UserNickName,@UserAvatarUrl,@UserComments,@CreateTime)");
                SQLiteParameter[] parameters = {
                sh.MakeSQLiteParameter("@CommentId",DbType.String,Guid.NewGuid().ToString()),
                sh.MakeSQLiteParameter("@UserOpenId",DbType.String,openId),
                sh.MakeSQLiteParameter("@UserNickName",DbType.String,nickName),
                sh.MakeSQLiteParameter("@UserAvatarUrl",DbType.String,avatarUrl),
                sh.MakeSQLiteParameter("@UserComments",DbType.String,comments),
                sh.MakeSQLiteParameter("@CreateTime",DbType.String,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    };
                if (sh.ExecuteSql(strSql.ToString(), parameters) >= 0)
                {
                    return ret = 1;
                }
                return 1;
            }

            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(string.Format("{0}",ex.Message));
                return ret;
            }
        }
    }
}