using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IMealPlanDAL
    {
        IDictionary<int, string> GetAllMealPlans(int id);

        IList<Meal> GetMealPlan(int id);

        int CreateMealPlan(int userId, string planName);

        void AddToPlan(int id, string recipeId, int mealOrder);

        void DeleteFromPlan(int planId, int recipeId, int mealOrder);

        IList<string> GetGroceries(int planId);
    }
}
