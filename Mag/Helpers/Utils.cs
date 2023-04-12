using Mag.Models;
using System.Reflection;

namespace Mag.Helpers
{
    public static class Utils
    {
        public static string GetCategoryName(this Categories category)
        {
            var type = category.GetType().GetMember(category.ToString()).First();
            var atr = type.GetCustomAttribute<CategoryAttribute>();
            return atr.Name;
        }
        public static string GetCategoryDescription(this Categories category)
        {
            var type = category.GetType().GetMember(category.ToString()).First();
            var atr = type.GetCustomAttribute<CategoryAttribute>();
            return atr.Description;
        }
        public static string GetCategoryIcon(this Categories category)
        {
            var type = category.GetType().GetMember(category.ToString()).First();
            var atr = type.GetCustomAttribute<CategoryAttribute>();
            return atr.Icon;
        }
    }
}
