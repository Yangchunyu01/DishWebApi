using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dish.WebAPI.Models
{
    public class BreakfastModel
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public List<Breakfast_Model> Dishes { get; set; }
    }
    public class Breakfast_Model
    {
        public string DishGuid { get; set; }
        public int Id { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
        public int GoodFB { get; set; }
        public int BadFB { get; set; }
        public int CommentCount { get; set; }
        public string Pic { get; set; }
    }
}