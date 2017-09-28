using Dish.WebAPI.Log;
using Dish.WebAPI.SQLDao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace Dish.WebAPI.Controllers
{
    public class EmailController : ApiController
    {
        [HttpGet]
        public string SendEmail(string alias)
        {
            LoggerHelper.Instance.WriteLine("EnterMethod: SendEmail");
            LoggerHelper.Instance.WriteLine(string.Format("Alias:{0}",alias));
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 25)
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("Daily_Dish@outlook.com", "1qazZAQ!"),
                EnableSsl = true,
                Timeout = 60000
            };
            string from = "Daily_Dish@outlook.com";
            string to = alias.Trim() + "@microsoft.com";
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
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLine(ex.Message);
                return "Error";
            }
            finally
            {
                message.Dispose();
                client.Dispose();
            }
        }
        public string GetRandomNum()
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
    }
}
