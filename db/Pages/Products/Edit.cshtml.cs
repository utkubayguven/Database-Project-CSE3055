using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace db.Pages.Products
{
    public class EditModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

            String id = Request.Query["id"];
            Console.WriteLine("id: " + id);

            try
            {
                String connectionString = "Data Source=DESKTOP-50JSG76;Initial Catalog=HEALINNOVA_DB;Integrated Security=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Product WHERE ProductID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productInfo.ProductID = reader.GetInt32(0);
                                productInfo.ProductName = reader.GetString(1);
                                productInfo.Price = reader.GetDecimal(2);
                                productInfo.ProducerID = reader.GetInt32(3);
                                productInfo.Type = reader.GetString(4);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "exex" + id;
            }
        }

        public void OnPost()
        {
            string ProductIDString = Request.Form["id"];
            int productIDint;
            if (Int32.TryParse(ProductIDString, out productIDint))
            {
                productInfo.ProductID = productIDint;
            }
            else
            {
                Console.WriteLine("The input could not be parsed to a Integer.");
                Console.WriteLine(ProductIDString);
            }
            productInfo.ProductName = Request.Form["productname"];
            string priceString = Request.Form["price"];
            decimal price;
            if (decimal.TryParse(priceString, out price))
            {
                productInfo.Price = price;
            }
            else
            {
                Console.WriteLine("The input could not be parsed to a DateTime.");
                Console.WriteLine(priceString);
            }
            string producerIDString = Request.Form["producerid"];
            int produceridint;
            if (int.TryParse(producerIDString, out produceridint))
            {
                productInfo.ProducerID = produceridint;
            }
            else
            {
                Console.WriteLine("The input could not be parsed to a Integer.");
                Console.WriteLine(producerIDString);
            }
            productInfo.Type = Request.Form["type"];
           

            if (productInfo.ProductName == "" || productInfo.Type.Length == 0 || productInfo.Price == 0)
            {
                errorMessage = "All the fields must be filled.";
                return;
            }
            try
            {
                String connectionString = "Data Source=DESKTOP-50JSG76;Initial Catalog=HEALINNOVA_DB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Product " +
             "SET ProductName=@ProductName, Price=@Price , ProducerID=@ProducerID, Type=@Type " +
             "WHERE ProductID=@ProductID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productInfo.ProductName);
                        command.Parameters.AddWithValue("@Price", productInfo.Price);
                        command.Parameters.AddWithValue("@ProducerID", productInfo.ProducerID);
                        command.Parameters.AddWithValue("@Type", productInfo.Type);
                        command.Parameters.AddWithValue("@ProductID", productInfo.ProductID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Products/Index");

        }

    }
}
