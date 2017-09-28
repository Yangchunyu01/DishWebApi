using Dish.DB.Entity;
using Dish.WebAPI.Models;
using Dish.WebAPI.SQLDao;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Dish.WebAPI.Controllers
{
    public class LunchController : ApiController
    {
        private static DishRepository repository = new DishRepository();
        // GET api/values
        [HttpGet]
        public string GetLunchDishes()
        {
            List<DishModel> modelList = new List<DishModel>();
            List<Lunch> list = repository.QueryLunchDish_Classify1();
            var query = list.GroupBy(l => l.Claasify).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                DishModel model = new DishModel();
                model.Id = i;
                model.Tag = 'a' + i.ToString();
                model.Name = list[i].Claasify;
                model.Dishes = query[i].Select(x => new LunchModel()
                {
                    DishGuid = x.Id.ToString(),
                    Id = new ArrayList(query[i].ToArray()).IndexOf(x),
                    Name = x.DishName,
                    Pic = "",
                    GoodFB = x.GoodValue,
                    BadFB = x.BadValue,
                    CommentCount = x.CommentCount,
                    Price = x.Price,
                }).ToList();
                modelList.Add(model);
            }
            var json = JsonConvert.SerializeObject(modelList);
            return json;
        }
    }
}
