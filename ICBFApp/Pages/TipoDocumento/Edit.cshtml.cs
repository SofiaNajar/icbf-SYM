using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static ICBFApp.Pages.TipoDocumento.IndexModel;

namespace ICBFApp.Pages.TipoDocumento
{
    public class EditModel : PageModel
    {

        public TipoDocInfo tipoDocInfo = new TipoDocInfo();
        public string errorMessage = "";
        public string successMessage = "";

        String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
       

        public void OnGet()
        {
            String idTipoDoc = Request.Query["idTipoDoc"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM TipoDocumento WHERE idTipoDoc = @idTipoDoc";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@idTipoDoc", idTipoDoc);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tipoDocInfo.idTipoDoc = "" + reader.GetInt32(0);
                                tipoDocInfo.tipo = reader.GetString(1);
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
            tipoDocInfo.idTipoDoc = Request.Form["idTipoDoc"];
            tipoDocInfo.tipo = Request.Form["tipo"];

            if (tipoDocInfo.tipo.Length == 0)
            {
                errorMessage = "Debe completar todos los campos";
                return Page();
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    
                    String sqlExistsNom = "SELECT COUNT(*) FROM TipoDocumento WHERE tipo = @tipo";
                    using (SqlCommand commandCheck = new SqlCommand(sqlExistsNom, connection))
                    {
                        commandCheck.Parameters.AddWithValue("@tipo", tipoDocInfo.tipo);

                        int count = (int)commandCheck.ExecuteScalar();

                        if (count > 0)
                        {
                            errorMessage = "El tipo de documento '" + tipoDocInfo.tipo + "' ya existe. Verifique la información e intente de nuevo.";
                            return Page();
                        }
                    }

                    String sqlUpdate = "UPDATE TipoDocumento SET tipo = @tipo WHERE idTipoDoc = @idTipoDoc";
                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@idTipoDoc", tipoDocInfo.idTipoDoc);
                        command.Parameters.AddWithValue("@tipo", tipoDocInfo.tipo);

                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Tipo de Documento Editado exitosamente";
                return RedirectToPage("/TipoDocumento/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }
        }
    }
}