using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class RecipeSqlDAL: IRecipeDAL
    {
        private readonly string connectionString;

        public RecipeSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string recipeIngredients = "SELECT ingredient_name, ingredient_quantity, measurement_form FROM Ingredients " +
            "INNER JOIN Recipe_Ingredients ON Ingredients.id = Recipe_Ingredients.ingredient_id " +
            "INNER JOIN Recipes ON Recipes.id = Recipe_Ingredients.recipe_id WHERE recipe_id = @id;" ;

        private string newRecipeCmd = "INSERT INTO Recipes (recipe_name, recipe_description, recipe_category, user_id, instructions) " +
            "VALUES (@name, @description, @category, @id, @instructions); SELECT SCOPE_IDENTITY();" ;

        private string newRecipeIngredientCmd = "INSERT INTO Recipe_Ingredients (ingredient_id, recipe_id, ingredient_quantity, measurement_form) " +
            "VALUES (@i_id, @r_id, @quantity, @measurement);";

        private string updateRecipe = "UPDATE Recipes SET user_id = @user, recipe_name = @name, recipe_description = @description, " +
            "recipe_category = @category, instructions = @instructions WHERE id = @id; " +
            "DELETE FROM Recipe_Ingredients WHERE recipe_id = @id";

        public IList<string> GetIngredients()
        {
            IList<string> allIngredients = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("SELECT * FROM Ingredients ORDER BY ingredient_name ", conn);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        allIngredients.Add(Convert.ToString(reader["ingredient_name"]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allIngredients;
        }

        public void AddNewIngredient(string ingredient)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("INSERT INTO Ingredients (ingredient_name) VALUES (@name)", conn);
                    comm.Parameters.AddWithValue("@name", ingredient);
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddNewRecipe(Recipe recipe)
        {
            int recipeId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(newRecipeCmd, conn);
                    comm.Parameters.AddWithValue("@name", recipe.Name);
                    comm.Parameters.AddWithValue("@description", recipe.Description);
                    comm.Parameters.AddWithValue("@category", recipe.Category);
                    comm.Parameters.AddWithValue("@instructions", recipe.Instructions);
                    comm.Parameters.AddWithValue("@id", recipe.UserId);

                    recipeId = Convert.ToInt32(comm.ExecuteScalar());

                    comm = new SqlCommand(newRecipeIngredientCmd, conn);

                    foreach(string[] item in recipe.Ingredients)
                    {
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@i_id", GetIngredientId(item[2]));
                        comm.Parameters.AddWithValue("@r_id", recipeId);
                        comm.Parameters.AddWithValue("@quantity", Convert.ToDouble(item[0]));
                        comm.Parameters.AddWithValue("@measurement", item[1]);

                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int GetIngredientId(string name)
        {
            int id = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("SELECT id FROM Ingredients WHERE ingredient_name = @name", conn);
                    comm.Parameters.AddWithValue("@name", name);

                    id = Convert.ToInt32(comm.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return id;
        }

        public IList<Recipe> GetAllRecipes(int userId)
        {
            IList<Recipe> allRecipes = new List<Recipe>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("SELECT * FROM Recipes WHERE user_id = @id", conn);
                    comm.Parameters.AddWithValue("@id", userId);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.Id = Convert.ToInt32(reader["id"]);
                        recipe.Name = Convert.ToString(reader["recipe_name"]);
                        recipe.Category = Convert.ToString(reader["recipe_category"]);
                        recipe.Description = Convert.ToString(reader["recipe_description"]);
                        recipe.Instructions = Convert.ToString(reader["instructions"]);
                        allRecipes.Add(recipe);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return allRecipes;
        }


        public Recipe GetRecipe( int id)
        {
            Recipe recipe = new Recipe();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("SELECT * FROM Recipes WHERE id = @id", conn);
                    comm.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comm.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        recipe.Id = Convert.ToInt32(reader["id"]);
                        recipe.Name = Convert.ToString(reader["recipe_name"]);
                        recipe.Category = Convert.ToString(reader["recipe_category"]);
                        recipe.Description = Convert.ToString(reader["recipe_description"]);
                        recipe.Instructions = Convert.ToString(reader["instructions"]);
                        recipe.UserId = Convert.ToInt32(reader["user_id"]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            recipe.Ingredients = GetRecipeIngredients(recipe.Id);
            return recipe;
        }


        public List<string[]> GetRecipeIngredients(int id)
        {
            List<string[]> ingredientsList = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(recipeIngredients, conn);
                    comm.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] item = new string[3];

                        item[0] = Convert.ToString(reader["ingredient_quantity"]);
                        item[1] = Convert.ToString(reader["measurement_form"]);
                        item[2] = Convert.ToString(reader["ingredient_name"]);

                        ingredientsList.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ingredientsList;
        }

        public void UpdateRecipe(Recipe recipe)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(updateRecipe, conn);
                    comm.Parameters.AddWithValue("@id", recipe.Id);
                    comm.Parameters.AddWithValue("@name", recipe.Name);
                    comm.Parameters.AddWithValue("@description", recipe.Description);
                    comm.Parameters.AddWithValue("@category", recipe.Category);
                    comm.Parameters.AddWithValue("@instructions", recipe.Instructions);
                    comm.Parameters.AddWithValue("@user", recipe.UserId);
                    comm.ExecuteNonQuery();

                    comm = new SqlCommand(newRecipeIngredientCmd, conn);

                    foreach (string[] item in recipe.Ingredients)
                    {
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@i_id", GetIngredientId(item[2]));
                        comm.Parameters.AddWithValue("@r_id", recipe.Id);
                        comm.Parameters.AddWithValue("@quantity", Convert.ToDouble(item[0]));
                        comm.Parameters.AddWithValue("@measurement", item[1]);
                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

       

    }
}
