using Dish.DB.Entity;
using Dish.WebAPI.Models;
using Dish.WebAPI.SQLDao;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dish.WebAPI.Controllers
{
    public class BreakfastController : ApiController
    {
        private static DishRepository repository = new DishRepository();
        // GET: api/Breakfast
        [HttpGet]
        public string GetBreakfastDish()
        {
            List<BreakfastModel> modelList = new List<BreakfastModel>();
            List<Breakfast> list = repository.QueryBreakfastDish();
            var query = list.GroupBy(l => l).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                BreakfastModel model = new BreakfastModel();
                model.Id = i;
                model.Tag = 'a' + i.ToString();
                if (i == 0)
                {
                    model.Name = "早餐";
                }
                model.Dishes = query[i].Select(x => new Breakfast_Model()
                {
                    DishGuid = x.Id.ToString(),
                    Id = new ArrayList(query[i].ToArray()).IndexOf(x),
                    Name = x.DishName,
                    Price = x.Price,
                    GoodFB = x.GoodValue,
                    BadFB = x.BadValue,
                    CommentCount = x.CommentCount,
                    Pic = ""
                }).ToList();
                modelList.Add(model);
            }
            var json = JsonConvert.SerializeObject(modelList);
            return json;
        }
    }
}
