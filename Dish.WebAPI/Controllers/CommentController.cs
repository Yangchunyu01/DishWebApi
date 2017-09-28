using Dish.DB.Entity;
using Dish.WebAPI.Log;
using Dish.WebAPI.SQLDao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dish.WebAPI.Controllers
{
    public class CommentController : ApiController
    {
        private UserRepository repositiry = new UserRepository();
        // GET: api/Comment
        [HttpGet]
        public List<Comments> GetAllComments()
        {
            List<Comments> list = repositiry.GetAll();
            return list;
        }

        [HttpPost]
        public void PutComments([FromBody]Comments inputData)
        {
            try
            {
                LoggerHelper.Instance.WriteLine(string.Format("Enter Method: PutComments,OpenId:{0},NickName:{1}", inputData.UserOpenId,inputData.UserNickName));
                int ret = repositiry.SaveUserComments(inputData.UserOpenId, inputData.UserNickName, inputData.UserAvatarUrl, inputData.UserComments);
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(string.Format("{0}",ex.Message));
            }
        }
    }
}
