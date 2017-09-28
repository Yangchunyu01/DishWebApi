using Dish.DB;
using Dish.DB.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DishApp
{
    public class DishUtil
    {
        public int AddBreDish(List<Breakfast> breDish)
        {
            int ret = 0;
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Table_Breakfast(");
            sb.Append("Id,DishName,Price,Date,GoodValue,BadValue,CommentCount,CreateDate)");
            sb.Append(" values ");
            foreach (Breakfast item in breDish)
            {
                sb.Append(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'),", item.Id, item.DishName, item.Price, item.Date, item.GoodValue, item.BadValue, item.CommentCount, item.CreatDate.ToString("s")));
            }
            if (sh.ExecuteSql(sb.ToString().Trim(',')) >= 1)
            {
                ret = 1;
            }
            return ret;
        }

        public int AddLunDish(List<Lunch> lunDish)
        {
            int ret = 0;
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Table_Lunch(");
            sb.Append("Id,DishName,Classify,Price,Date,GoodValue,BadValue,CommentCount,CreateDate)");
            sb.Append(" values ");
            foreach (Lunch item in lunDish)
            {
                sb.Append(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'),", item.Id, item.DishName, item.Claasify, item.Price, item.Date, item.GoodValue, item.BadValue, item.CommentCount, item.CreatDate.ToString("s")));
            }
            if (sh.ExecuteSql(sb.ToString().Trim(',')) >= 1)
            {
                ret = 1;
            }
            return ret;
        }

        public int AddDinDish(List<Dinner> dinDish)
        {
            int ret = 0;
            SQLiteHelper sh = new SQLiteHelper();
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Table_Dinner(");
            sb.Append("Id,DishName,Price,Date,GoodValue,BadValue,CommentCount,CreateDate)");
            sb.Append(" values ");
            foreach (Dinner item in dinDish)
            {
                sb.Append(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'),", item.Id, item.DishName, item.Price, item.Date, item.GoodValue, item.BadValue, item.CommentCount, item.CreatDate.ToString("s")));
            }
            if (sh.ExecuteSql(sb.ToString().Trim(',')) >= 1)
            {
                ret = 1;
            }
            return ret;
        }
    }
}
