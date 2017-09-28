using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System;
using System.Net.Http;
using System.Net;
using Dish.DB.Entity;
using System.Security.Cryptography;
using System.Text;
using Dish.WebAPI.UserHelper;
using Dish.WebAPI.SQLDao;
using Dish.WebAPI.Log;

namespace Dish.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        private string appId = "wxa7042fbd0a4b46be";
        private string appSecret = "8157ca8db0299ca222400ddcbf4dffb7";

        [HttpGet]
        public string Get(string encryptedData, string iv, string code)
        {
            UserRepository repositiry = new UserRepository();
            LoggerHelper.Instance.WriteLine("Entering Method:GetUserInformation");
            string grantType = "authorization_code";
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret=" + appSecret + "&js_code=" + code + "&grant_type=" + grantType;
            string type = "utf-8";
            UtilHelper userHelper = new UtilHelper();
            string j = userHelper.GetUrltoHtml(url, type);
            JObject jo = (JObject)JsonConvert.DeserializeObject(j);
            
            OpenIdAndSessionKey oisk = new OpenIdAndSessionKey();
            try
            {
                oisk.OpenId = jo["openid"].ToString();
                oisk.Session_Key = jo["session_key"].ToString();
                LoggerHelper.Instance.WriteLine(string.Format("OpenId:{0},Session_Key:{1}", jo["openid"].ToString(), jo["session_key"].ToString()));
            }
            catch (Exception)
            {
                oisk.ErrCode = jo["errcode"].ToString();
                oisk.ErrMsg = jo["errmsg"].ToString();
                LoggerHelper.Instance.WriteLine(string.Format("ErrCode:{0},ErrMsg:{1}", jo["errcode"].ToString(), jo["errmsg"].ToString()));
            }
            try
            {
                if (!string.IsNullOrEmpty(oisk.OpenId))
                {
                    if (!repositiry.QueryUserByOpenId(oisk.OpenId))
                    {
                        userHelper.AesIV = iv;
                        userHelper.AesKey = oisk.Session_Key;

                        string result = userHelper.AESDecrypt(encryptedData);

                        JObject _userInfo = (JObject)JsonConvert.DeserializeObject(result);

                        UserInfo userInfo = new UserInfo();
                        userInfo.OpenId = _userInfo["openId"].ToString();
                        try //部分验证返回值中没有unionId  
                        {
                            userInfo.UnionId = _userInfo["unionId"].ToString();
                        }
                        catch (Exception)
                        {
                            userInfo.UnionId = "unionId";
                        }
                        userInfo.NickName = _userInfo["nickName"].ToString();
                        userInfo.Gender = _userInfo["gender"].ToString();
                        userInfo.City = _userInfo["city"].ToString();
                        userInfo.Province = _userInfo["province"].ToString();
                        userInfo.Country = _userInfo["country"].ToString();
                        userInfo.AvatarUrl = _userInfo["avatarUrl"].ToString();
                        userInfo.CreateTime = DateTime.Now;
                        userInfo.UserId = Guid.NewGuid().ToString();
                        userInfo.IsMsEmp = "0";
                        repositiry.SaveUserInfo(userInfo);
                        LoggerHelper.Instance.WriteLine("Save user information successfully!");
                        return _userInfo["openId"].ToString();
                    }
                    else
                    {
                        return oisk.OpenId;
                    }

                }
                else
                {
                    LoggerHelper.Instance.WriteLine("OpenId is null or user exists!");
                    return "Session has expired";
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(ex.Message);
                return "Error";
            }
        }
       
    }
}
