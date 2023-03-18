using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace db.Pages.Customers
{
    public class CreateModel : PageModel
    {
        public CustomerInfo customerInfo = new CustomerInfo();
        public String errorMesage = "";
        public String succesMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() {
            
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

            if(customerInfo.Gender == null || customerInfo.CustomerFirstName.Length == 0 || customerInfo.CustomerLastName.Length == 0 || customerInfo.Age == 0) {
                errorMesage = "All the fields must be filled.";
                return;
            }


            try
            {
                String connectionString = "Data Source=DESKTOP-50JSG76;Initial Catalog=HEALINNOVA_DB;Integrated Security=True";
                using (SqlConnection connection= new SqlConnection(connectionString))
                {
                    connection.Open();

                        String sqlforindex = "SELECT max(CustomerID) FROM Customer";
                        using (SqlCommand commandforindex = new SqlCommand(sqlforindex, connection))
                        {
                            using (SqlDataReader reader = commandforindex.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    customerInfo.CustomerID = reader.GetInt32(0);

                                }
                            }
                        }
                        int lastindex = customerInfo.CustomerID + 1;
                        String sql = "INSERT INTO Customer" +  "(CustomerID,CustomerFirstName,CustomerLastName,BirthDate,Gender,Age) VALUES" + "(@CustomerID,@CustomerFirstName,@CustomerLastName,@BirthDate,@Gender,@Age);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", lastindex);
                        command.Parameters.AddWithValue("@CustomerFirstName", customerInfo.CustomerFirstName);
                        command.Parameters.AddWithValue("@CustomerLastName", customerInfo.CustomerLastName);
                        command.Parameters.AddWithValue("@BirthDate", customerInfo.BirthDate);
                        command.Parameters.AddWithValue("@Gender", customerInfo.Gender);
                        command.Parameters.AddWithValue("@Age", customerInfo.Age);

                        command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception ex) {
                errorMesage = ex.Message;
                return;
            }

            succesMessage = "New Customer added";
            //Response.Redirect("/Customer/Index");

        }
    }
}
