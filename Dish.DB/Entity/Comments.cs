using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dish.DB.Entity
{
    public class Comments
    {
        public string CommentId { get; set; }
        public string UserOpenId { get; set; }
        public string UserNickName { get; set; }
        public string UserAvatarUrl { get; set; }
        public string UserComments { get; set; }
        public string CreateTime { get; set; }
    }

    public class DishComments : Comments
    {
        public string DishId { get; set; }

        public string DishType { get; set; }
    }
}
