using db.Pages.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace db.Pages.Products
{
    public class CreateModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public String errorMesage = "";
        public String succesMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {

            productInfo.ProductName = Request.Form["productname"];
            string priceString = Request.Form["price"];
            int price;
            if (int.TryParse(priceString, out price))
            {
                productInfo.Price = price;
            }
            else
            {
                Console.WriteLine("The Age field could not be parsed to an int.");
            }
            string producerIDString = Request.Form["producerid"];
            int producerID;
            if (int.TryParse(producerIDString, out producerID))
            {
                productInfo.ProducerID = producerID;
            }
            else
            {
                Console.WriteLine("The Age field could not be parsed to an int.");
            }
            productInfo.Type = Request.Form["type"];

            if (productInfo.ProductName == null || productInfo.Price <= 0 || productInfo.ProducerID <= 0 || productInfo.Type.Length == 0)
            {
                errorMesage = "All the fields must be filled.";
                return;
            }


            try
            {
                String connectionString = "Data Source=DESKTOP-50JSG76;Initial Catalog=HEALINNOVA_DB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    String sqlforindex = "SELECT max(ProductID) FROM Product";
                    connection.Open();
                    using (SqlCommand commandforindex = new SqlCommand(sqlforindex, connection))
                    {
                        using (SqlDataReader reader = commandforindex.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productInfo.ProductID =  reader.GetInt32(0);



                            }
                        }
                    }
                    int lastindex = productInfo.ProductID+1;
                    String sql = "INSERT INTO Product" + "(ProductID,ProductName,Price,ProducerID,Type) VALUES" + "(@ProductID,@ProductName,@Price,@ProducerID,@Type);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", lastindex);
                        command.Parameters.AddWithValue("@ProductName", productInfo.ProductName);
                        command.Parameters.AddWithValue("@Price", productInfo.Price);
                        command.Parameters.AddWithValue("@ProducerID", productInfo.ProducerID);
                        command.Parameters.AddWithValue("@Type", productInfo.Type);

                        command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception ex)
            {
                errorMesage = ex.Message;
                return;
            }

            succesMessage = "New Product added";
            Response.Redirect("/Products/Index");

        }
    }
}
