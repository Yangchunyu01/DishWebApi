using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dish.DB
{
    public class SQLiteHelper
    {
        private static string DbFile = @"E:\Dish.WebApi\Dish.WebAPI\DishDB.v1db";//HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["SQLString"]);
        private static string connectionString = "Data Source=" + DbFile;

        public SQLiteHelper()
        {
            CreateDataBaseIfNotExists();
        }

        public bool Exists(string strSql)
        {
            object obj = ExecuteSql(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Exists(string strSql, params SQLiteParameter[] cmdParms)
        {
            object obj = ExecuteSql(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public int ExecuteSql(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        public int ExecuteSql(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {

                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        connection.Open();
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        public static SQLiteDataReader ExecuteReader(string SQLString, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SQLiteDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }

        }

        public object GetSingle(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        public DataSet Query(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SQLite.SQLiteException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        public static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }


        public SQLiteParameter MakeSQLiteParameter(string name, DbType type, object value)
        {
            SQLiteParameter parm = new SQLiteParameter(name, type);
            parm.Value = value;
            return parm;
        }

        public void CreateDataBaseIfNotExists()
        {
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }
        }
        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile, true);
        }
        private void CreateDatabase()
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                ExecuteSql("CREATE TABLE IF NOT EXISTS Table_Breakfast('Id' varchar(100) not null,'DishName' varchar(100) not null, 'Price' varchar(100) not null, 'Date' varchar(100) not null ,'GoodValue' int,'BadValue' int,'CommentCount' int,'CreateDate' DateTime)");
                ExecuteSql("CREATE TABLE IF NOT EXISTS Table_Lunch('Id' varchar(100) not null,'DishName' varchar(100) not null,'Classify' varchar(100), 'Price' varchar(100) not null, 'Date' varchar(100) not null ,'GoodValue' int,'BadValue' int,'CommentCount' int,'CreateDate' DateTime)");
                ExecuteSql("CREATE TABLE IF NOT EXISTS Table_Dinner('Id' varchar(100) not null,'DishName' varchar(100) not null, 'Price' varchar(100) not null, 'Date' varchar(100) not null ,'GoodValue' int,'BadValue' int,'CommentCount' int,'CreateDate' DateTime)");
                ExecuteSql("CREATE TABLE IF NOT EXISTS Table_UserInfo('UserId' varchar(100) not null,'OpenId' varchar(100) not null,'UnionId' varchar(100),'NickName' varchar(100),'AvatarUrl' varchar(100),'Gender' varchar(4),'Province' varchar(100),'City' varchar(100),'Country' varchar(100),'IsMsEmp' varchar(4),'CreateTime' DateTime)");
                ExecuteSql("CREATE TABLE IF NOT EXISTS Table_Comments('CommentId' varchar(100) not null,'UserOpenId' varchar(100) not null,'UserNickName' varchar(100),'UserAvatarUrl' varchar(100),'UserComments' varchar(100),'CreateTime' DateTime)");
                ExecuteSql("CREATE TABLE IF NOT EXISTS Table_DishComments('CommentId' varchar(100) not null,'DishId' varchar(100),'UserOpenId' varchar(100) not null,'UserNickName' varchar(100),'UserAvatarUrl' varchar(100),'UserComments' varchar(100),'CreateTime' DateTime)");
            }

        }
    }
}
