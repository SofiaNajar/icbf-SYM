using Microsoft.AspNetCore.Mvc.RazorPages;
using static ICBFApp.Pages.Jardin.IndexModel;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace ICBFApp.Pages.Jardin
{
    public class EditModel : PageModel
    {
        public JardinInfo jardinInfo = new JardinInfo();
        public string errorMessage = "";
        public string successMessage = "";
        String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
        

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM jardines WHERE idJardin = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                jardinInfo.idJardin = "" + reader.GetInt32(0);
                                jardinInfo.nombre = reader.GetString(1).Trim();
                                jardinInfo.direccion = reader.GetString(2).Trim();
                                jardinInfo.estado = reader.GetString(3);
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
            jardinInfo.idJardin = Request.Form["id"];
            jardinInfo.nombre = Request.Form["nombreJardin"];
            jardinInfo.direccion = Request.Form["direccionJardin"];
            jardinInfo.estado = Request.Form["estado"];

            if (jardinInfo.idJardin.Length == 0 || jardinInfo.nombre.Length == 0 || jardinInfo.direccion.Length == 0 || jardinInfo.estado.Length == 0)
            {
                errorMessage = "Debe completar todos los campos";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    /* REVISAR PORQUE SI AL EDITAR NO QUIERO MODIFICAR EL NOMBRE, ME LO VA DAR COMO QUE YA EXISTE
                    // Espacio para validar que el jadin no exista
                    String sqlExists = "SELECT COUNT(*) FROM jardines WHERE nombre = @nombreJardin";
                    using (SqlCommand commandCheck = new SqlCommand(sqlExists, connection))
                    {
                        commandCheck.Parameters.AddWithValue("@nombreJardin", jardinInfo.nombre);

                        int count = (int)commandCheck.ExecuteScalar();

                        if (count > 0)
                        {
                            errorMessage = "El Jardín '" + jardinInfo.nombre + "' ya existe. Verifique la información e intente de nuevo.";
                            return Page();
                        }
                    }
                    */

                    String sqlUpdate = "UPDATE jardines SET nombre = @nombreJardin, direccion = @direccionJardin, estado = @estado WHERE idJardin = @id";
                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@id", jardinInfo.idJardin);
                        command.Parameters.AddWithValue("@nombreJardin", jardinInfo.nombre);
                        command.Parameters.AddWithValue("@direccionJardin", jardinInfo.direccion);
                        command.Parameters.AddWithValue("@estado", jardinInfo.estado);

                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Jardín Editado exitosamente";
                return RedirectToPage("/Jardin/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }
        }
    }
}
