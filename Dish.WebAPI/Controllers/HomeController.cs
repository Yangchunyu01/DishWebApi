using Dish.DB;
using Dish.DB.Entity;
using Dish.WebAPI.Models;
using Dish.WebAPI.SQLDao;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Dish.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private static DishRepository repository = new DishRepository();
        private string appId = "wxa7042fbd0a4b46be";
        private string appSecret = "8157ca8db0299ca222400ddcbf4dffb7";
        [HttpGet]
        public ActionResult Index()
        {
            //UserRepository userRe = new UserRepository();
            //UserInfo user = new UserInfo() {
            //    OpenId = "oaHUN0YbabJFgfigx0IpB0h87vtE",
            //    NickName = "Chunyu",
            //    AvatarUrl = "",
            //    City = "",
            //    Country = "",
            //    CreatDate = DateTime.Now,
            //    Gender = "1",
            //    Province = "",
            //    UnionId = "",
            //    UserId = Guid.NewGuid().ToString()
            //};

            //userRe.SaveUserInfo(user);
            //DishRepository re = new DishRepository();
            //var a = GetBreakfastDish();
            //string result = SendEmail(alias);
            //var b = GetLunchDishes();
            ViewBag.Title = "Home Page";

            return View();
        }

        #region Test Local
        public string Get(string code)
        {
            string temp = "https://api.weixin.qq.com/sns/jscode2session?" +
                "appid=" + appId
                + "&secret=" + appSecret
                + "&js_code=" + code
                + "&grant_type=authorization_code";
            string type = "utf-8";
            UserHelper.UtilHelper Userhelper = new UserHelper.UtilHelper();
            string j = Userhelper.GetUrltoHtml(temp, type);
            JObject jo = (JObject)JsonConvert.DeserializeObject(j);
            return GetResponse(temp);
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetResponse(string url)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        public static string SendEmail(string alias)
        {
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 25)
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("Daily_Dish@outlook.com","1qazZAQ!"),
                EnableSsl = true,
            };
            string from = "Daily_Dish@outlook.com";
            string to = alias + "@microsoft.com";
            string subject = "小程序验证码";
            string content = GetRandomNum();
            MailAddress From = new MailAddress(from);
            MailAddress To = new MailAddress(to);
            MailMessage message = new MailMessage(From, To);
            message.Subject = subject;
            message.Body = "欢迎使用我们的小程序,验证码：" + content;
            message.IsBodyHtml = true;
            try
            {
                client.Send(message);
                return content;
            }
            catch (Exception e)
            {
                return "Error";
            }
            finally
            {
                message.Dispose();
                client.Dispose();
            }
        }
        public static string GetRandomNum()
        {
            List<string> arr = new List<string>();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            int a1 = rd.Next(0, 10);
            int a2 = rd.Next(0, 10);
            int a3 = rd.Next(0, 10);
            int a4 = rd.Next(0, 10);
            string s = string.Format("{0}{1}{2}{3}", a1, a2, a3, a4);
            return s;
        }

        public ActionResult GetDinnerDish()
        {
            List<DinnerModel> modeList = new List<DinnerModel>();
            List<Dinner> list = repository.QueryDinnerDish();
            var query = list.GroupBy(l => l).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                DinnerModel model = new DinnerModel();
                model.Id = i;
                model.Tag = 'a' + i.ToString();
                if (i == 0)
                {
                    model.Name = "晚餐";
                }
                model.Dishes = query[i].Select(x => new Dinner_Model()
                {
                    Id = new ArrayList(query[i].ToArray()).IndexOf(x),
                    Name = x.DishName,
                    Pic = "",
                    Price = x.Price
                }).ToList();
                modeList.Add(model);
            }

            return Json(modeList);
        }

        public IEnumerable<BreakfastModel> GetBreakfastDish()
        {
            List<BreakfastModel> modeList = new List<BreakfastModel>();
            List<Breakfast> list = repository.QueryBreakfastDish();
            var query = list.GroupBy(l => l).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                BreakfastModel model = new BreakfastModel();
                model.Id = i;
                model.Tag = 'a' + i.ToString();
                model.Dishes = query[i].Select(x => new Breakfast_Model()
                {
                    Id = new ArrayList(query[i].ToArray()).IndexOf(x),
                    Name = x.DishName,
                    Pic = "",
                    Price = x.Price
                }).ToList();
                modeList.Add(model);
            }
            return modeList;
        }

        public IEnumerable<DishModel> GetLunchDishes()
        {
            DishRepository repository = new DishRepository();
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
                    Id = new ArrayList(query[i].ToArray()).IndexOf(x),
                    Name = x.DishName,
                    Pic = "",
                    Price = x.Price,
                }).ToList();
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion
    }
}
