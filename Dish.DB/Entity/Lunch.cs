﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dish.DB.Entity
{
    public class Lunch
    {
        public string Id { get; set; }
        public string DishName { get; set; }
        public string Claasify { get; set; }
        public string Price { get; set; }
        public string Date { get; set; }
        public int GoodValue { get; set; }
        public int BadValue { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreatDate { get; set; }
    }
}
