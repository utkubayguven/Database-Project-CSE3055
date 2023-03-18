using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace db.Pages.Customers
{
    public class IndexModel : PageModel
    {

        public List<CustomerInfo> listCustomers = new List<CustomerInfo>();
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
                    String sql = "SELECT * FROM Customer";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerInfo customerInfo = new CustomerInfo();
                                customerInfo.CustomerID = reader.GetInt32(0);
                                customerInfo.CustomerFirstName = reader.GetString(1);
                                customerInfo.CustomerLastName = reader.GetString(2);
                                customerInfo.BirthDate = reader.GetDateTime(3);
                                customerInfo.Gender = reader.GetString(4);
                                customerInfo.Age = reader.GetInt32(5);
                                listCustomers.Add(customerInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Error, not a number! ,"+ ex.ToString());
            }
        }
    }

    public class CustomerInfo
    {
        public int CustomerID;
        public String CustomerFirstName;
        public String CustomerLastName;
        public DateTime BirthDate;
        public String Gender;
        public int Age;
    }
}
