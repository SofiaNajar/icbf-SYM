using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static ICBFApp.Pages.EPS.IndexModel;

namespace ICBFApp.Pages.EPS
{
    public class EditModel : PageModel
    {
        public EPSInfo epsInfo = new EPSInfo();
        public string errorMessage = "";
        public string successMessage = "";
        
        String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
        
        public void OnGet()
        {
            String idEps = Request.Query["idEps"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM EPS WHERE idEps = @idEps";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idEps", idEps);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                epsInfo.idEps = reader.GetInt32(0).ToString();
                                epsInfo.NIT = reader.GetString(1);
                                epsInfo.nombre = reader.GetString(2);
                                epsInfo.direccion = reader.GetString(3);
                                epsInfo.telefono = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }

        public IActionResult OnPost() 
        {
            epsInfo.idEps = Request.Form["idEps"];
            epsInfo.NIT = Request.Form["NIT"];
            epsInfo.nombre = Request.Form["nombre"];
            epsInfo.direccion = Request.Form["direccion"];
            epsInfo.telefono = Request.Form["telefono"];

            if (epsInfo.nombre.Length == 0 || epsInfo.NIT.Length == 0 || epsInfo.direccion.Length == 0 || epsInfo.telefono.Length == 0)
            {
                errorMessage = "Debe completar todos los campos";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    String sqlUpdate = "UPDATE EPS SET NIT = @NIT, nombre = @nombre, direccion = @direccion, telefono = @telefono WHERE idEps = @idEps";
                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@idEps", epsInfo.idEps);
                        command.Parameters.AddWithValue("@NIT", epsInfo.NIT);
                        command.Parameters.AddWithValue("@nombre", epsInfo.nombre);
                        command.Parameters.AddWithValue("@direccion", epsInfo.direccion);
                        command.Parameters.AddWithValue("@telefono", epsInfo.telefono);

                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "EPS Editada exitosamente";
                return RedirectToPage("/EPS/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }
        }
    }
}
