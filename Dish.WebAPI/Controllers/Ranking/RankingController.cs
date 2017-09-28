using Dish.WebAPI.Log;
using Dish.WebAPI.SQLDao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dish.WebAPI.Controllers.Ranking
{
    public class RankingController : ApiController
    {
        private static DishRepository repository = new DishRepository();
        private static TimeSpan _startBreakfast = DateTime.Parse("06:00").TimeOfDay;
        private static TimeSpan _endBreakfast = DateTime.Parse("10:30").TimeOfDay;
        private static TimeSpan _startLunch = DateTime.Parse("10:30").TimeOfDay;
        private static TimeSpan _endLunch = DateTime.Parse("13:00").TimeOfDay;
        private static TimeSpan _startDinner = DateTime.Parse("18:00").TimeOfDay;
        private static TimeSpan _endDinner = DateTime.Parse("20:00").TimeOfDay;
        // GET: api/Ranking
        public string GetRankingData()
        {
            try
            {
                LoggerHelper.Instance.WriteLine("Enter Method:  GetRankingData");
                DateTime currentTime = DateTime.Now;
                TimeSpan currentTimeSanp = currentTime.TimeOfDay;
                if (currentTimeSanp > _startBreakfast && currentTimeSanp < _endBreakfast)
                {
                    var list = repository.GetBreakfastByRanking();
                    var json = JsonConvert.SerializeObject(list);
                    return json;
                }
                else if (currentTimeSanp > _startLunch && currentTimeSanp < _endLunch)
                {
                    var list = repository.GetLunchByRanking();
                    var json = JsonConvert.SerializeObject(list);
                    return json;
                }
                else if (currentTimeSanp > _startDinner && currentTimeSanp < _endDinner)
                {
                    var list = repository.GetDinnerByRanking();
                    var json = JsonConvert.SerializeObject(list);
                    return json;
                }
                else
                {
                    var list = repository.GetDinnerByRanking();
                    var json = JsonConvert.SerializeObject(list);
                    return json;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(string.Format("Catch Exception:{0}", ex.Message));
                return "Error";
            }
        }
    }
}
