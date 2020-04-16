using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class MealPlanSqlDAL : IMealPlanDAL
    {
        private readonly string connectionString;

        private string mealPlanListSql = "SELECT id, plan_name FROM meal_plans WHERE user_id = @userId";

        private string singleMealPlanSql = "SELECT recipes.id, recipe_name FROM recipes " +
            "RIGHT JOIN meals ON recipes.id = meals.recipe_id " +
            "INNER JOIN meal_plans ON meal_plans.id = meals.plan_id " +
            "WHERE meal_plans.id = @planId ORDER BY meals.meal_order";

        private string createPlanSql = "INSERT INTO meal_plans (plan_name, user_id) VALUES(@planName, @userId); SELECT SCOPE_IDENTITY();";

        private string addMealsToPlanSql = "INSERT INTO meals (plan_id, meal_order) VALUES (@planId, @mealOrder)";

        private string addRecipeToPlanSql = "UPDATE meals SET recipe_id = @recipeId WHERE plan_id = @planId AND meal_order = @mealOrder;";

        private string deleteRecipeFromMealSql = "UPDATE meals SET recipe_id = null WHERE plan_id = @planId AND meal_order = @mealOrder;";

        private string getGroceriesSql = "SELECT ingredient_name FROM Ingredients " +
                "INNER JOIN Recipe_Ingredients ON Ingredients.id = Recipe_Ingredients.ingredient_id " +
                "INNER JOIN Recipes ON Recipes.id = Recipe_Ingredients.recipe_id " +
                "INNER JOIN meals ON meals.recipe_id = Recipes.id " +
                "INNER JOIN meal_plans on meal_plans.id = meals.plan_id " +
                "WHERE meal_plans.id = @planId GROUP BY ingredient_name " +
                "ORDER BY ingredient_name";

        public MealPlanSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDictionary<int, string> GetAllMealPlans(int userId)
        {
            
            IDictionary<int, string> plans = new Dictionary<int, string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(mealPlanListSql, conn);
                    comm.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        int planId = Convert.ToInt32(reader["id"]);
                        string planName =Convert.ToString(reader["plan_name"]);

                        plans[planId] = planName;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return plans;
        }

        public IList<Meal> GetMealPlan(int id)
        {
            IList<Meal> singlePlan = new List<Meal>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(singleMealPlanSql, conn);
                    comm.Parameters.AddWithValue("@planId", id);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Meal meal = new Meal();

                        if(reader["id"] == DBNull.Value)
                        {
                            meal.RecipeId = 0;
                            meal.RecipeName = "";
                        } else
                        {
                            meal.RecipeId = Convert.ToInt32(reader["id"]);
                            meal.RecipeName = Convert.ToString(reader["recipe_name"]);
                        }
                        singlePlan.Add(meal);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return singlePlan;
        }

        public int CreateMealPlan(int userId, string planName)
        {
            int planId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(createPlanSql, conn);
                    comm.Parameters.AddWithValue("@planName", planName);
                    comm.Parameters.AddWithValue("@userId", userId);
                    planId = Convert.ToInt32(comm.ExecuteScalar());

                    comm = new SqlCommand(addMealsToPlanSql, conn);

                    for(int i= 1; i <22; i++)
                    {
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@planId", planId);
                        comm.Parameters.AddWithValue("@mealOrder", i);
                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return planId;
        }

        public void AddToPlan(int id, string recipeId, int mealOrder)
        {
            int r_id = Convert.ToInt32(recipeId);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(addRecipeToPlanSql, conn);
                    comm.Parameters.AddWithValue("@planId", id);
                    comm.Parameters.AddWithValue("@recipeId", r_id);
                    comm.Parameters.AddWithValue("@mealOrder", mealOrder);

                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DeleteFromPlan(int planId, int recipeId, int mealOrder)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(deleteRecipeFromMealSql, conn);
                    comm.Parameters.AddWithValue("@planId", planId);
                    comm.Parameters.AddWithValue("@recipeId", recipeId);
                    comm.Parameters.AddWithValue("@mealOrder", mealOrder);

                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public IList<string> GetGroceries(int planId)
        {
            IList<string> groceryList = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(getGroceriesSql, conn);
                    comm.Parameters.AddWithValue("@planId", planId);

                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        groceryList.Add(Convert.ToString(reader["ingredient_name"]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return groceryList;

        }
    }

 
}
