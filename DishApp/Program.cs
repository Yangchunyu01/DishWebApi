using Dish.DB.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DishApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            DishUtil util = new DishUtil();
            List<Breakfast> breList = new List<Breakfast>();
            List<Lunch> lunList = new List<Lunch>();
            List<Dinner> dinList = new List<Dinner>();
            List<string> dishList = System.IO.File.ReadAllLines(@"D:\FormatedDishData\dish25-29.txt").ToList();
            for (int i = 0; i < dishList.Count; i++)
            {
                string[] dishInfo = dishList[i].Split('\t');
                if (!string.IsNullOrEmpty(dishInfo[0]))
                {
                    Breakfast bra_dish = new Breakfast()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DishName = dishInfo[0],
                        Price = dishInfo[1],
                        Date = dishInfo[2],
                        BadValue = 0,
                        GoodValue = 0,
                        CommentCount = 0,
                        CreatDate = DateTime.Now
                    };
                    breList.Add(bra_dish);
                }

                if (!string.IsNullOrEmpty(dishInfo[3]))
                {
                    Lunch lun_dish = new Lunch()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DishName = dishInfo[3],
                        Price = dishInfo[4],
                        Claasify = "西式简餐",
                        Date = dishInfo[5],
                        BadValue = 0,
                        GoodValue = 0,
                        CommentCount = 0,
                        CreatDate = DateTime.Now
                    };
                    lunList.Add(lun_dish);
                }
                if (!string.IsNullOrEmpty(dishInfo[6]))
                {
                    Lunch lun_dish = new Lunch()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DishName = dishInfo[6],
                        Price = dishInfo[7],
                        Claasify = "风味美食",
                        Date = dishInfo[8],
                        BadValue = 0,
                        GoodValue = 0,
                        CommentCount = 0,
                        CreatDate = DateTime.Now
                    };
                    lunList.Add(lun_dish);
                }
                if (!string.IsNullOrEmpty(dishInfo[9]))
                {
                    Lunch lun_dish = new Lunch()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DishName = dishInfo[9],
                        Price = dishInfo[10],
                        Claasify = "麦香面粉馆",
                        Date = dishInfo[11],
                        BadValue = 0,
                        GoodValue = 0,
                        CommentCount = 0,
                        CreatDate = DateTime.Now
                    };
                    lunList.Add(lun_dish);
                }
                if (!string.IsNullOrEmpty(dishInfo[12]))
                {
                    Lunch lun_dish = new Lunch()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DishName = dishInfo[12],
                        Price = dishInfo[13],
                        Claasify = "星级自选",
                        Date = dishInfo[14],
                        BadValue = 0,
                        GoodValue = 0,
                        CommentCount = 0,
                        CreatDate = DateTime.Now
                    };
                    lunList.Add(lun_dish);
                }
                //if (!string.IsNullOrEmpty(dishInfo[15]))
                //{
                //    Lunch lun_dish = new Lunch()
                //    {
                //        Id = Guid.NewGuid().ToString(),
                //        DishName = dishInfo[15],
                //        Price = dishInfo[16],
                //        Claasify = "晶动乐果吧",
                //        Date = dishInfo[17],
                //        BadValue = 0,
                //        GoodValue = 0,
                //        CommentCount = 0,
                //        CreatDate = DateTime.Now
                //    };
                //    lunList.Add(lun_dish);
                //}
                if (!string.IsNullOrEmpty(dishInfo[15]))
                {
                    Dinner din_dish = new Dinner()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DishName = dishInfo[15],
                        Price = dishInfo[16],
                        Date = dishInfo[17],
                        BadValue = 0,
                        GoodValue = 0,
                        CommentCount = 0,
                        CreatDate = DateTime.Now
                    };
                    dinList.Add(din_dish);
                }
            }
            util.AddBreDish(breList);
            util.AddLunDish(lunList);
            util.AddDinDish(dinList);
        }
    }
}
