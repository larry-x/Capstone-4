using System;
using System.Transactions;
using System.Data.SqlClient;
using System.IO;
using WebApplication.Web.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace WebApplication.Tests.UnitTests 
{
    [TestClass]
    public class ParentDALTest
    {
        public TransactionScope trans;
        protected string ConnectionString { get; } = "Data Source=.\\SQLEXPRESS;Initial Catalog=MealPlanner;Integrated Security=True";
        public int recipe_id, ingredient_id1, ingredient_id2;

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope();

            string sql = File.ReadAllText("test-script.sql");
            string sql2 = "INSERT INTO Ingredients (ingredient_name) VALUES('Custard'); SELECT SCOPE_IDENTITY();";
            string sql3 = "INSERT INTO Ingredients (ingredient_name) VALUES('Flan'); SELECT SCOPE_IDENTITY();";
            string sql4 = "INSERT INTO Recipe_Ingredients (recipe_id, ingredient_id, " +
                "ingredient_quantity, measurement_form) VALUES(@rid, @iid, '2', 'tbsp');";
            string sql5 = 
                "INSERT INTO Recipe_Ingredients(recipe_id, ingredient_id, ingredient_quantity, measurement_form) " +
                "VALUES(@rid, @iid, '1', 'tbsp');";
            string sql6 = "UPDATE Recipes SET recipe_name = 'Dummy', recipe_description = 'Yummy', " +
            "recipe_category = 'desert', instructions = '1 2 3' WHERE id = @id;";
            

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                recipe_id = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand(sql2, conn);
                ingredient_id1 = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand(sql3, conn);
                ingredient_id2 = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand(sql4, conn);
                cmd.Parameters.AddWithValue("@rid", recipe_id);
                cmd.Parameters.AddWithValue("@iid", ingredient_id1);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(sql5, conn);
                cmd.Parameters.AddWithValue("@rid", recipe_id);
                cmd.Parameters.AddWithValue("@iid", ingredient_id2);
                cmd.ExecuteNonQuery();

                //cmd = new SqlCommand(sql6, conn);
                //cmd.Parameters.AddWithValue("@id", recipe_id);
                //cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Reset()
        {
            trans.Dispose();
        }

        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }
    }
}
