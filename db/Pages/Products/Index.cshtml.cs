using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace db.Pages.Products
{
    public class IndexModel : PageModel
    {

        public List<ProductInfo> listProducts = new List<ProductInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-50JSG76;Initial Catalog=HEALINNOVA_DB;Integrated Security=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        Console.WriteLine("Successfully connected to the database.");
                    }
                    String sql = "SELECT * FROM Product";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductInfo productInfo = new ProductInfo();
                                productInfo.ProductID = reader.GetInt32(0);
                                productInfo.ProductName = reader.GetString(1);
                                productInfo.Price = reader.GetDecimal(2);
                                productInfo.ProducerID = reader.GetInt32(3);
                                productInfo.Type = reader.GetString(4);
                                listProducts.Add(productInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, not a number! ," + ex.ToString());
            }
        }
    }

    public class ProductInfo
    {
        public int ProductID;
        public String ProductName;
        public decimal Price;
        public int ProducerID;
        public String Type;
    }
}
