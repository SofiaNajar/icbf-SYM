using ICBFApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using System.Data.SqlClient;
using static ICBFApp.Pages.EPS.IndexModel;
using static ICBFApp.Pages.Jardin.IndexModel;
using static ICBFApp.Pages.TipoDocumento.IndexModel;
using static ICBFApp.Pages.Usuario.IndexModel;

namespace ICBFApp.Pages.Ninio
{
    public class IndexModel : PageModel
    {
        private readonly IGeneratePdfService _generatePdfService;
        private readonly IWebHostEnvironment _host;

        public IndexModel(IGeneratePdfService generatePdfService, IWebHostEnvironment host)
        {
            _generatePdfService = generatePdfService;
            _host = host;
        }

        public List<NinioInfo> listNinio = new List<NinioInfo>();
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            string id= Request.Query["idNino"];

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
                    String sqlSelect = "SELECT * FROM Ninos";

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
                                    tipoDocInfo.tipo = reader.GetString(1).ToString();

                                    DatosBasicosInfo datosBasicos = new DatosBasicosInfo();
                                    datosBasicos.idDatosBasicos = reader.GetInt32(2).ToString();
                                    datosBasicos.tipoDoc = tipoDocInfo;
                                    datosBasicos.identificacion = reader.GetString(3);
                                    datosBasicos.nombres = reader.GetString(4);
                                    datosBasicos.fechaNacimiento = reader.GetDateTime(5).Date.ToShortDateString();

                                    EPSInfo eps = new EPSInfo();
                                    eps.idEps = reader.GetInt32(6).ToString();
                                    eps.nombre = reader.GetString(7);

                                    JardinInfo jardin = new JardinInfo();
                                    jardin.idJardin = reader.GetInt32(8).ToString();
                                    jardin.nombre = reader.GetString(9);

                                    DatosBasicosInfo datosAcudiente = new DatosBasicosInfo();
                                    datosAcudiente.idDatosBasicos = reader.GetInt32(11).ToString();
                                    datosAcudiente.nombres = reader.GetString(12);

                                    UsuarioInfo acudiente = new UsuarioInfo();
                                    acudiente.idUsuario = reader.GetInt32(10).ToString();
                                    acudiente.datosBasicos = datosAcudiente;

                                    NinioInfo ninio = new NinioInfo();
                                    ninio.idNinio = reader.GetInt32(13).ToString();
                                    ninio.ciudadNacimiento = reader.GetString(14);
                                    ninio.tipoSangre = reader.GetString(15);
                                    ninio.edad = calcularEdad(reader.GetDateTime(5).Date.ToShortDateString());
                                    ninio.jardin = jardin;
                                    ninio.acudiente = acudiente;
                                    ninio.datosBasicos = datosBasicos;
                                    ninio.eps = eps;

                                    listNinio.Add(ninio);
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
            var report = _generatePdfService.GeneratePdfQuest();
            byte[] pdfBytes = report.GeneratePdf();
            var mimeType = "application/pdf";
            
            return File(pdfBytes, mimeType); 
        }

        public int calcularEdad(string fechaNacimientoStr)
        {
            DateTime fechaNacimiento;
            bool isValidDate = DateTime.TryParse(fechaNacimientoStr, out fechaNacimiento);

            if (!isValidDate)
            {
                throw new ArgumentException("La fecha de nacimiento no está en un formato válido.");
            }

            DateTime today = DateTime.Today;
            int age = today.Year - fechaNacimiento.Year;

           
            if (fechaNacimiento.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public class NinioInfo
        {
            public string idNinio { get; set; }
            public string tipoSangre { get; set; }
            public string ciudadNacimiento { get; set; }
            public string peso { get; set; } 
            public string estatura { get; set; } 
            public int edad { get; set; }
            public JardinInfo jardin { get; set; }
            public UsuarioInfo acudiente { get; set; }
            public DatosBasicosInfo datosBasicos {  get; set; }
            public EPSInfo eps { get; set; }
        }
    }
}
