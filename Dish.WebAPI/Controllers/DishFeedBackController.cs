using Dish.DB.Entity;
using Dish.DB.Enum;
using Dish.WebAPI.Log;
using Dish.WebAPI.SQLDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dish.WebAPI.Controllers
{
    public class DishFeedBackController : ApiController
    {
        private static DishRepository repository = new DishRepository();

        [HttpGet]
        public List<DishComments> GetAllDishCommentById(string dishId)
        {
            List<DishComments> list = repository.GetBydishId(dishId);
            return list;
        }


        [HttpGet]
        public bool UserFeedBack(Enums.DishType dishType, string dishId, Enums.FeedBackType feedBackType)
        {
            if (dishType.Equals(Enums.DishType.Breakfast))
            {
                try
                {
                    switch (feedBackType)
                    {
                        case Enums.FeedBackType.Good:
                            repository.UpdateDishFeedBack("Table_Breakfast", "GoodValue", dishId);
                            LoggerHelper.Instance.WriteLine("Update Table:Table_Breakfast \t Column: GoodValue successfully!");
                            break;
                        case Enums.FeedBackType.Bad:
                            repository.UpdateDishFeedBack("Table_Breakfast", "BadValue", dishId);
                            LoggerHelper.Instance.WriteLine("Update Table:Table_Breakfast \t Column: BadValue successfully!");
                            break;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Instance.WriteLine(string.Format("Exception:{0}", ex.Message));
                    return false;
                }
            }
            else if (dishType.Equals(Enums.DishType.Lunch))
            {
                try
                {
                    switch (feedBackType)
                    {
                        case Enums.FeedBackType.Good:
                            repository.UpdateDishFeedBack("Table_Lunch", "GoodValue", dishId);
                            LoggerHelper.Instance.WriteLine("Update Table:Table_Lunch \t Column: GoodValue successfully!");
                            break;
                        case Enums.FeedBackType.Bad:
                            repository.UpdateDishFeedBack("Table_Lunch", "BadValue", dishId);
                            LoggerHelper.Instance.WriteLine("Update Table:Table_Lunch \t Column: BadValue successfully!");
                            break;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Instance.WriteLine(string.Format("Exception:{0}", ex.Message));
                    return false;
                }

            }
            else if (dishType.Equals(Enums.DishType.Dinner))
            {
                try
                {
                    switch (feedBackType)
                    {
                        case Enums.FeedBackType.Good:
                            repository.UpdateDishFeedBack("Table_Dinner", "GoodValue", dishId);
                            LoggerHelper.Instance.WriteLine("Update Table:Table_Dinner \t Column: GoodValue successfully!");
                            break;
                        case Enums.FeedBackType.Bad:
                            repository.UpdateDishFeedBack("Table_Dinner", "BadValue", dishId);
                            LoggerHelper.Instance.WriteLine("Update Table:Table_Dinner \t Column: BadValue successfully!");
                            break;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Instance.WriteLine(string.Format("Exception:{0}", ex.Message));
                    return false;
                }
            }
            else return false;
        }

        [HttpPost]
        public void Post([FromBody]DishComments inputData)
        {
            repository.SaveDishComments(inputData.UserOpenId, inputData.DishId, inputData.UserNickName, inputData.UserAvatarUrl, inputData.UserComments, inputData.DishType);
        }
    }
}
