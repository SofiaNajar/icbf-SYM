using Microsoft.AspNetCore.Mvc.RazorPages;
using static ICBFApp.Pages.Ninio.IndexModel;
using static ICBFApp.Pages.Usuario.IndexModel;
using System.Data.SqlClient;
using ICBFApp.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace ICBFApp.Pages.Asistencia
{
    public class IndexModel : PageModel
    {

        public List<AsistenciaInfo> listAsistenciaInfo = new List<AsistenciaInfo>();

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        private readonly IGeneratePdfService _generatePdfServiceAsistencia;
        private readonly string _connectionString;

        public IndexModel(IConfiguration configuration, IGeneratePdfService generatePdfServiceAsistencia)
        {
            _generatePdfServiceAsistencia = generatePdfServiceAsistencia;
            _connectionString = configuration.GetConnectionString("ConexionSQLServer");
        }

        public void OnGet()
        {
            if (TempData.ContainsKey("SuccessMessage") || TempData.ContainsKey("ErrorMessage"))
            {
                SuccessMessage = TempData["SuccessMessage"] as string;
                ErrorMessage = TempData["ErrorMessage"] as string;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    String sqlSelect = "SELECT DatosBasicos.nombres, DatosBasicos.identificacion, Asistencias.fecha, Asistencias.estadoNino, Asistencias.idAsistencia " +
                        "FROM Asistencias " +
                        "INNER JOIN Ninos ON Asistencias.idNino = Ninos.idNino " +
                        "INNER JOIN DatosBasicos ON Ninos.idDatosBasicos = DatosBasicos.idDatosBasicos;";

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                   
                                    DatosBasicosInfo datosBasicosInfo = new DatosBasicosInfo();
                                    datosBasicosInfo.nombres = reader.GetString(0).ToString();  
                                    datosBasicosInfo.identificacion = reader.GetString(1).ToString(); ;  

                                    
                                    AsistenciaInfo asistenciaInfo = new AsistenciaInfo();
                                    asistenciaInfo.fecha = reader.GetDateTime(2).Date.ToShortDateString();  
                                    asistenciaInfo.estadoNino = reader.GetString(3).ToString(); 
                                    asistenciaInfo.idAsistencia = reader.GetInt32(4).ToString(); 

                                   
                                    asistenciaInfo.datosBasicosInfo = datosBasicosInfo;

                                 
                                    listAsistenciaInfo.Add(asistenciaInfo);
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

        public IActionResult OnPostDownloadPdf()
        {
            var report = _generatePdfServiceAsistencia.GeneratePdfQuest();
            byte[] pdfBytes = report.GeneratePdf();
            var mimeType = "application/pdf";
            
            return File(pdfBytes, mimeType); 
        }

        public class AsistenciaInfo
        {
            public string idAsistencia { get; set; }
            public string fecha { get; set; }
            public string estadoNino { get; set; }
            public DatosBasicosInfo datosBasicosInfo { get; set; }
            public NinioInfo ninioInfo { get; set; }
            public UsuarioInfo usuarioInfo { get; set; }
        }
    }
}
