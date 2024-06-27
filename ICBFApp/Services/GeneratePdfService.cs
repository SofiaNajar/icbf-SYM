using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Data.SqlClient;
using static ICBFApp.Pages.Jardin.IndexModel;
using static ICBFApp.Pages.Ninio.IndexModel;
using static ICBFApp.Pages.TipoDocumento.IndexModel;
using static ICBFApp.Pages.Usuario.IndexModel;

namespace ICBFApp.Services
{
    public class GeneratePdfService : IGeneratePdfService
    {
        private readonly IWebHostEnvironment _host;

        public GeneratePdfService(IWebHostEnvironment host)
        {
            _host = host;
        }

        public List<NinioInfo> listNinio = new List<NinioInfo>();

        public void GetData()
        {
            try
            {
                String connectionString = "Data Source=PC-MIGUEL-C\\SQLEXPRESS;Initial Catalog=db_ICBF;Integrated Security=True;";
                //String connectionString = "RUTA ANGEL";
                //String connectionString = "Data Source=BOGAPRCSFFSD108\\SQLEXPRESS;Initial Catalog=db_ICBF;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlSelect = "SELECT t.tipo, identificacion, nombres, fechaNacimiento, j.nombre, " +
                        "(SELECT nombres FROM Usuarios as u " +
                        "INNER JOIN DatosBasicos as d ON u.idDatosBasicos = d.idDatosBasicos " +
                        "WHERE idUsuario = n.idUsuario) as acudiente, " +
                        "ciudadNacimiento " +
                        "FROM Ninos as n " +
                        "INNER JOIN Jardines as j ON n.idJardin = j.idJardin " +
                        "INNER JOIN DatosBasicos as d ON n.idDatosBasicos = d.idDatosBasicos " +
                        "INNER JOIN TipoDocumento as t ON d.idTipoDocumento = t.idTipoDoc " +
                        "INNER JOIN Usuarios as u ON n.idUsuario = u.idUsuario; ";

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Validar si hay datos
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TipoDocInfo tipoDocInfo = new TipoDocInfo();
                                    tipoDocInfo.tipo = reader.GetString(0).ToString();

                                    DatosBasicosInfo datosBasicos = new DatosBasicosInfo();
                                    datosBasicos.tipoDoc = tipoDocInfo;
                                    datosBasicos.identificacion = reader.GetString(1);
                                    datosBasicos.nombres = reader.GetString(2);
                                    datosBasicos.fechaNacimiento = reader.GetDateTime(3).Date.ToShortDateString();

                                    JardinInfo jardin = new JardinInfo();
                                    jardin.nombre = reader.GetString(4);

                                    DatosBasicosInfo datosAcudiente = new DatosBasicosInfo();
                                    datosAcudiente.nombres = reader.GetString(5);

                                    UsuarioInfo acudiente = new UsuarioInfo();
                                    acudiente.datosBasicos = datosAcudiente;

                                    NinioInfo ninio = new NinioInfo();
                                    ninio.ciudadNacimiento = reader.GetString(6);
                                    ninio.edad = calcularEdad(reader.GetDateTime(3).Date.ToShortDateString());
                                    ninio.jardin = jardin;
                                    ninio.acudiente = acudiente;
                                    ninio.datosBasicos = datosBasicos;

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

        public Document GeneratePdfQuest()
        {
            GetData();
            DateTime today = DateTime.Today;
            var report = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        var rutaImgSena = Path.Combine(_host.WebRootPath, "images/logoSena.png");
                        byte[] imageDataSena = System.IO.File.ReadAllBytes(rutaImgSena);

                        var rutaImgICBF = Path.Combine(_host.WebRootPath, "images/logoICBF.png");
                        byte[] imageDataICBF = System.IO.File.ReadAllBytes(rutaImgICBF);

                        //row.ConstantItem(150).Height(60).Placeholder();
                        row.ConstantItem(75).AlignMiddle().Height(50).Image(imageDataSena);
                        row.ConstantItem(75).AlignMiddle().Height(65).Image(imageDataICBF);

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Height(20).Text("Asociación del ICBF").Bold().FontSize(14).AlignRight();
                            col.Item().Height(20).Text("Reporte Diario").Bold().AlignRight();
                            col.Item().Height(20).Text("Fecha de emisión: " + today.Date.ToShortDateString()).AlignRight();
                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().PaddingVertical(10).AlignCenter()
                        .Text("Listado de Niños Inscritos")
                        .Bold().FontSize(24).FontColor("#39a900");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.RelativeColumn(2);
                                columns.ConstantColumn(50);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Identificación").FontColor("#fff").AlignCenter();
                                header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Nombres").FontColor("#fff").AlignCenter();
                                header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Edad").FontColor("#fff").AlignCenter();
                                header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Acudiente").FontColor("#fff").AlignCenter();
                                header.Cell().Background("#212529").Border(0.5f).BorderColor(Colors.Black).AlignMiddle().Text("Ciudad Nacimiento").FontColor("#fff").AlignCenter();
                            });

                            foreach (var nino in listNinio)
                            {
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(nino.datosBasicos.identificacion).AlignCenter();
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(nino.datosBasicos.nombres).AlignCenter();
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(nino.edad.ToString()).AlignCenter();
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(nino.acudiente.datosBasicos.nombres).AlignCenter();
                                table.Cell().Border(0.5f).BorderColor(Colors.Black).Text(nino.ciudadNacimiento).AlignCenter();
                            }
                        });
                    });

                    page.Footer()
                        .AlignRight()
                        .Text(txt =>
                        {
                            txt.Span("Pagina ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" de ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });
                });
            });

            return report;
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

            // Comprueba si el cumpleaños aún no ha ocurrido en el año actual
            if (fechaNacimiento.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
