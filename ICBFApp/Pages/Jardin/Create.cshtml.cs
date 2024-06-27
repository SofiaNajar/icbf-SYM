using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ICBFApp.Pages.Jardin.IndexModel;
using System.Data.SqlClient;

namespace ICBFApp.Pages.Jardin
{
    public class CreateModel : PageModel
    {
        public JardinInfo jardinInfo = new JardinInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            jardinInfo.nombre = Request.Form["nombreJardin"];
            jardinInfo.direccion = Request.Form["direccionJardin"];

            if (jardinInfo.nombre.Length == 0 || jardinInfo.direccion.Length == 0)
            {
                errorMessage = "Debe completar todos los campos";
                return Page();
            }

            try
            {
                String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
                

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

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

                    // Espacio para validar que el jadin no exista
                    String sqlInsert = "INSERT INTO jardines (nombre, direccion, estado)" +
                        "VALUES (@nombreJardin, @direccionJardin, @estado);";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@nombreJardin", jardinInfo.nombre);
                        command.Parameters.AddWithValue("@direccionJardin", jardinInfo.direccion);
                        command.Parameters.AddWithValue("@estado", "En trámite");

                        command.ExecuteNonQuery();
                    }
                    TempData["SuccessMessage"] = "Jardín agregado con éxito";
                    return RedirectToPage("/Jardin/Index");
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }
        }
    }
}
