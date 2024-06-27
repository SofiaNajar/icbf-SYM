using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static ICBFApp.Pages.EPS.IndexModel;
using static ICBFApp.Pages.Jardin.IndexModel;
using static ICBFApp.Pages.Ninio.IndexModel;
using static ICBFApp.Pages.Usuario.IndexModel;

namespace ICBFApp.Pages.Ninio
{
    public class CreateModel : PageModel
    {
        public List<JardinInfo> listaJardines { get; set; } = new List<JardinInfo>();
        public List<UsuarioInfo> listaAcudientes { get; set; } = new List<UsuarioInfo>();
        public List<EPSInfo> listaEps { get; set; } = new List<EPSInfo>();
        public string[] listaTiposSangre { get; set; } = new string[] { "O+", "O-", "A+", "A-", "AB+", "AB-" };
        public NinioInfo ninio = new NinioInfo();
        public DatosBasicosInfo datosBasicos = new DatosBasicosInfo();
        public string errorMessage = "";
        public string successMessage = "";

        String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
        

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlJardines = "SELECT idJardin, nombre from jardines";
                    using (SqlCommand command = new SqlCommand(sqlJardines, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verificar si hay filas en el resultado antes de intentar leer
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var id = reader.GetInt32(0).ToString();
                                    var nombreJardin = reader.GetString(1);

                                    listaJardines.Add(new JardinInfo
                                    {
                                        idJardin = reader.GetInt32(0).ToString(),
                                        nombre = reader.GetString(1)
                                    });

                                    foreach (var jardin in listaJardines)
                                    {
                                        Console.WriteLine("List item - id: {0}, nombreJardin: {1}", jardin.idJardin, jardin.nombre);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No hay filas en el resultado.");
                                Console.WriteLine("No se encontraron datos en la tabla jardines.");
                            }
                        }
                    }

                    String sqlEps = "SELECT idEps, nombre FROM eps";
                    using (SqlCommand command = new SqlCommand(sqlEps, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verificar si hay filas en el resultado antes de intentar leer
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var idEps = reader.GetInt32(0).ToString();
                                    var nombre = reader.GetString(1);

                                    listaEps.Add(new EPSInfo
                                    {
                                        idEps = reader.GetInt32(0).ToString(),
                                        nombre = reader.GetString(1)
                                    });

                                    foreach (var eps in listaEps)
                                    {
                                        Console.WriteLine("List item - id: {0}, eps: {1}", eps.idEps, eps.nombre);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No hay filas en el resultado.");
                                Console.WriteLine("No se encontraron datos en la tabla eps.");
                            }
                        }
                    }

                    String sqlAcudiente = "SELECT idUsuario, identificacion FROM Usuarios as u " +
                        "INNER JOIN DatosBasicos as d ON u.idDatosBasicos = d.idDatosBasicos " +
                        "INNER JOIN Roles as r ON u.idRol = r.idRol " +
                        "WHERE r.nombre = 'Acudiente';";
                    using (SqlCommand command = new SqlCommand(sqlAcudiente, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verificar si hay filas en el resultado antes de intentar leer
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var idUsuario = reader.GetInt32(0).ToString();
                                    var identificacion = reader.GetString(1);
                                    DatosBasicosInfo datosAcudiente = new DatosBasicosInfo();
                                    datosAcudiente.identificacion = reader.GetString(1);

                                    listaAcudientes.Add(new UsuarioInfo
                                    {
                                        idUsuario = reader.GetInt32(0).ToString(),
                                        datosBasicos = datosAcudiente
                                    });

                                    foreach (var acudiente in listaAcudientes)
                                    {
                                        Console.WriteLine("List item - id: {0}, identificacion: {1}", acudiente.idUsuario, acudiente.datosBasicos.identificacion);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("No hay filas en el resultado.");
                                Console.WriteLine("No se encontraron datos en la tabla usuarios.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                errorMessage = ex.Message;
            }
        }
        public IActionResult OnPost()
        {
            string identificacion = Request.Form["identificacion"];
            string nombres = Request.Form["nombres"];
            string fechaNacimiento = Request.Form["fechaNacimiento"];
            string ciudadNacimiento = Request.Form["ciudadNacimiento"];
            string celular = Request.Form["celular"];
            string direccion = Request.Form["direccion"];
            string tipoSangre = Request.Form["tipoSangre"];
            string acudienteIdString = Request.Form["acudiente"];
            string jardinIdString = Request.Form["jardin"];
            string epsIdString = Request.Form["eps"];
            int epsId;
            int acudienteId;
            int jardinId;
            int tipoDocId = 3;
            int edad = calcularEdad(fechaNacimiento);

            if (string.IsNullOrEmpty(identificacion) || string.IsNullOrEmpty(nombres) || string.IsNullOrEmpty(fechaNacimiento) 
                || string.IsNullOrEmpty(ciudadNacimiento) || string.IsNullOrEmpty(celular) || string.IsNullOrEmpty(direccion)
                || string.IsNullOrEmpty(tipoSangre))
            {
                errorMessage = "Todos los campos son obligatorios";
                return Page();
            }

            if (!int.TryParse(acudienteIdString, out acudienteId))
            {
                errorMessage = "Acudiente inválido seleccionado";
                return Page();
            }

            if (!int.TryParse(jardinIdString, out jardinId))
            {
                errorMessage = "Jardín inválido seleccionado";
                return Page();
            }

            if (!int.TryParse(epsIdString, out epsId))
            {
                errorMessage = "EPS inválido seleccionado";
                return Page();
            }

            if (edad > 5)
            {
                errorMessage = "La edad máxima permitida que es de 5 años";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlExists = "SELECT COUNT(*) FROM Ninos as n " +
                        "INNER JOIN DatosBasicos as d ON n.idDatosBasicos = d.idDatosBasicos " +
                        "WHERE d.identificacion = @identificacion;";
                    using (SqlCommand commandCheck = new SqlCommand(sqlExists, connection))
                    {
                        commandCheck.Parameters.AddWithValue("@identificacion", identificacion);

                        int count = (int)commandCheck.ExecuteScalar();
                        if (count > 0)
                        {
                            errorMessage = "El niño " + nombres + " con identificación " + identificacion + " ya existe. " +
                                           "Verifique la información e intente de nuevo";
                            return Page();
                        }
                    }

                    String sqlTipoId = "SELECT idTipoDoc FROM TipoDocumento WHERE tipo = 'NIUP';";
                    using (SqlCommand commandTypeDoc = new SqlCommand(sqlTipoId, connection))
                    {
                        using (SqlDataReader reader = commandTypeDoc.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tipoDocId = reader.GetInt32(0);
                            }
                        }
                    }

                    String sqlInsert = "INSERT INTO DatosBasicos" +
                        "(identificacion, nombres, fechaNacimiento, celular, direccion, idTipoDocumento)" +
                        "VALUES" +
                        "(@identificacion, @nombres, @fechaNacimiento, @celular, @direccion, @tipoDocumento)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@identificacion", identificacion);
                        command.Parameters.AddWithValue("@nombres", nombres);
                        command.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        command.Parameters.AddWithValue("@celular", celular);
                        command.Parameters.AddWithValue("@direccion", direccion);
                        command.Parameters.AddWithValue("@tipoDocumento", tipoDocId);

                        command.ExecuteNonQuery();
                    }

                    String sqlSelectDatosBasicos = "SELECT TOP 1 idDatosBasicos FROM DatosBasicos ORDER BY idDatosBasicos DESC";

                    using (SqlCommand command2 = new SqlCommand(sqlSelectDatosBasicos, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            // Validar si hay datos
                            if (reader.Read())
                            {
                                datosBasicos.idDatosBasicos = reader.GetInt32(0).ToString();
                            }
                        }
                    }

                    String sqlInsertNinio = "INSERT INTO Ninos (tipoSangre, ciudadNacimiento, idJardin, idUsuario, idDatosBasicos, idEps)" +
                            "VALUES (@tipoSangre, @ciudadNacimiento, @idJardin, @idUsuario, @idDatosBasicos, @idEps);";

                    using (SqlCommand command2 = new SqlCommand(sqlInsertNinio, connection))
                    {
                        command2.Parameters.AddWithValue("@tipoSangre", tipoSangre);
                        command2.Parameters.AddWithValue("@ciudadNacimiento", ciudadNacimiento);
                        command2.Parameters.AddWithValue("@idJardin", jardinId);
                        command2.Parameters.AddWithValue("@idUsuario", acudienteId);
                        command2.Parameters.AddWithValue("@idDatosBasicos", datosBasicos.idDatosBasicos);
                        command2.Parameters.AddWithValue("@idEps", epsId);

                        command2.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Niño creado exitosamente";
                return RedirectToPage("/Ninio/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }
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