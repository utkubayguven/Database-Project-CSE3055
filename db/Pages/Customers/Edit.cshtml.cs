using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;


namespace db.Pages.Customers
{
    public class EditModel : PageModel
    {
        public CustomerInfo customerInfo = new CustomerInfo();
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
                    String sql = "SELECT * FROM Customer WHERE CustomerID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read()) { 
                            customerInfo.CustomerID = reader.GetInt32(0);
                            customerInfo.CustomerFirstName = reader.GetString(1);
                            customerInfo.CustomerLastName= reader.GetString(2);
                            customerInfo.BirthDate = reader.GetDateTime(3);
                            customerInfo.Gender = reader.GetString(4);
                            customerInfo.Age = reader.GetInt32(5);

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message+ "exex" + id ;
            }
        }

        public void OnPost()
        {
            string customerIDString = Request.Form["id"];
            int customerIDint;
            if (Int32.TryParse(customerIDString, out customerIDint))
            {
                customerInfo.CustomerID = customerIDint;
            }
            else
            {
                Console.WriteLine("The input could not be parsed to a Integer.");
                Console.WriteLine(customerIDString);
            }
            customerInfo.CustomerFirstName = Request.Form["firstname"];
            customerInfo.CustomerLastName = Request.Form["lastname"];
            string input = Request.Form["birth_date"];
            DateTime result;
            if (DateTime.TryParse(input, out result))
            {
                customerInfo.BirthDate = result;
            }
            else
            {
                Console.WriteLine("The input could not be parsed to a DateTime.");
                Console.WriteLine(input);
            }
            customerInfo.Gender = Request.Form["gender"];
            string ageString = Request.Form["Age"];
            int age;
            if (int.TryParse(ageString, out age))
            {
                customerInfo.Age = age;
            }
            else
            {
                Console.WriteLine("The Age field could not be parsed to an int.");
            }

            if (customerInfo.Gender == null || customerInfo.CustomerFirstName.Length == 0 || customerInfo.CustomerLastName.Length == 0 || customerInfo.Age == 0)
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
                    String sql = "UPDATE Customer " + "SET CustomerFirstName=@CustomerFirstName, CustomerLastName=@CustomerLastName , BirthDate=@BirthDate, Gender=@Gender, Age=@Age " + "WHERE CustomerID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerFirstName", customerInfo.CustomerFirstName);
                        command.Parameters.AddWithValue("@CustomerLastName", customerInfo.CustomerLastName);
                        command.Parameters.AddWithValue("@BirthDate", customerInfo.BirthDate);
                        command.Parameters.AddWithValue("@Gender", customerInfo.Gender);
                        command.Parameters.AddWithValue("@Age", customerInfo.Age);
                        command.Parameters.AddWithValue("@id", customerInfo.CustomerID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Customers/Index");

        }

    }
}
