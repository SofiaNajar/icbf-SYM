using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ICBFApp.Pages.TipoDocumento
{
    public class IndexModel : PageModel
    {
        public List<TipoDocInfo> listTipoDocumento = new List<TipoDocInfo>();
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                SuccessMessage = TempData["SuccessMessage"] as string;
            }

            try
            {
                String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
               

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlSelect = "SELECT * FROM TipoDocumento";

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TipoDocInfo tipoDocInfo = new TipoDocInfo();
                                    tipoDocInfo.idTipoDoc = reader.GetInt32(0).ToString();
                                    tipoDocInfo.tipo = reader.GetString(1);

                                    listTipoDocumento.Add(tipoDocInfo);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No hay filas en el resultado");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public class TipoDocInfo
        {
            public string idTipoDoc { get; set; }
            public string tipo { get; set; }
        }
    }
}
