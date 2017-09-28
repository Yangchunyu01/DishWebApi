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
    public class CommentRouteController : ApiController
    {
        UserRepository repositiry = new UserRepository();
        [HttpGet]
        public string IsMsEmp(string openId)
        {
            bool result = repositiry.QueryUserIsMsEmp(openId);
            if(result)
                return "Success";
            return "Failed";
        }

        [HttpGet]
        public void UpdateUserInfo(string userOpenId)
        {
            try
            {
                LoggerHelper.Instance.WriteLine(string.Format("EnterMethod: UpdateUserInfo, OpenId:{0}", userOpenId));
                repositiry.UpdateUserInfoByOpenId(userOpenId);
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(string.Format("{0}", ex.Message));
            }
        }
    }
}
