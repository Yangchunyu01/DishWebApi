using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dish.DB.Entity
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string OpenId { get; set; }
        public string UnionId { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }
        public string Gender { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IsMsEmp { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class WeChatLoginInfo
    {
        public string code { get; set; }
        public string encryptedData { get; set; }
        public string iv { get; set; }
        public string rawData { get; set; }
        public string signature { get; set; }
    }

    public class OpenIdAndSessionKey
    {
        public string OpenId { get; set; }
        public string Session_Key { get; set; }
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
    }
}
