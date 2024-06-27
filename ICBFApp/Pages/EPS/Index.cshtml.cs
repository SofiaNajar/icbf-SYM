using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ICBFApp.Pages.EPS
{
    public class IndexModel : PageModel
    {

        public List<EPSInfo> listEPS = new List<EPSInfo>();
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
           

            try
            {
                String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlSelect = "SELECT * FROM EPS";

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                           
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    EPSInfo epsInfo  = new EPSInfo();
                                    epsInfo.idEps = reader.GetInt32(0).ToString();
                                    epsInfo.NIT = reader.GetString(1);
                                    epsInfo.nombre = reader.GetString(2);
                                    epsInfo.direccion = reader.GetString(3);
                                    epsInfo.telefono = reader.GetString(4);

                                    listEPS.Add(epsInfo);
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

        public class EPSInfo
        {
            public string idEps { get; set; }
            public string nombre { get; set; }
            public string NIT { get; set; }
            public string direccion { get; set; }
            public string telefono { get; set; }

        }
    }
}
