using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace db.Pages
{
    public class MoneyModel : PageModel
    {
        public List<FinanceInfo> listFinance = new List<FinanceInfo>();
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
                    String sql = "SELECT * FROM Finance";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FinanceInfo financeInfo = new FinanceInfo();
                                financeInfo.Assets = reader.GetDecimal(0);
                                financeInfo.Taxes = reader.GetDecimal(1);
                                financeInfo.Banks = reader.GetString(2);
                                financeInfo.CompanyCreditStatus = reader.GetString(3);
                                listFinance.Add(financeInfo);
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


    public class FinanceInfo
    {
        public decimal Assets;
        public decimal Taxes;
        public String Banks;
        public String CompanyCreditStatus;
    }

}
