using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static ICBFApp.Pages.EPS.IndexModel;
using static ICBFApp.Pages.Jardin.IndexModel;
using static ICBFApp.Pages.Ninio.IndexModel;
using static ICBFApp.Pages.Usuario.IndexModel;

namespace ICBFApp.Pages.Ninio
{
    public class EditModel : PageModel
    {
        public List<JardinInfo> listaJardines { get; set; } = new List<JardinInfo>();
        public List<UsuarioInfo> listaAcudientes { get; set; } = new List<UsuarioInfo>();
        public List<EPSInfo> listaEps { get; set; } = new List<EPSInfo>();
        public string[] listaTiposSangre { get; set; } = new string[] { "O+", "O-", "A+", "A-", "AB+", "AB-" };
        public NinioInfo ninio = new NinioInfo();
        public DatosBasicosInfo datosBasicos = new DatosBasicosInfo();
        public EPSInfo epsSelected = new EPSInfo();
        public UsuarioInfo acudienteSelected = new UsuarioInfo();
        public DatosBasicosInfo datosBasicosAcudiente = new DatosBasicosInfo();
        public JardinInfo jardinSelected = new JardinInfo();
        public string errorMessage = "";
        public string successMessage = "";

        String connectionString = "Data Source=BOGAPRCSFFSD119\\SQLEXPRESS;Initial Catalog=bdMAFIA;Integrated Security=True;";
       

        public void OnGet()
        {
            String idNino = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlSelect = "SELECT n.idDatosBasicos, identificacion, nombres, fechaNacimiento, " +
                        "celular, d.direccion, e.idEps, e.nombre, j.idJardin, j.nombre, n.idUsuario, " +
                        "(SELECT idDatosBasicos FROM Usuarios WHERE idUsuario = n.idUsuario) as idDatosBasicosAcudiente, " +
                        "(SELECT identificacion FROM Usuarios as u " +
                        "INNER JOIN DatosBasicos as d ON u.idDatosBasicos = d.idDatosBasicos " +
                        "WHERE idUsuario = n.idUsuario) as acudiente, " +
                        "idNino, ciudadNacimiento, tipoSangre " +
                        "FROM Ninos as n " +
                        "INNER JOIN Jardines as j ON n.idJardin = j.idJardin " +
                        "INNER JOIN DatosBasicos as d ON n.idDatosBasicos = d.idDatosBasicos " +
                        "INNER JOIN TipoDocumento as t ON d.idTipoDocumento = t.idTipoDoc " +
                        "INNER JOIN EPS as e ON n.idEps = e.idEps " +
                        "INNER JOIN Usuarios as u ON n.idUsuario = u.idUsuario " +
                        "WHERE idNino = @idNino;";
                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    {
                        command.Parameters.AddWithValue("@idNino", idNino);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                datosBasicos.idDatosBasicos = "" + reader.GetInt32(0);
                                datosBasicos.identificacion = reader.GetString(1);
                                datosBasicos.nombres = reader.GetString(2);
                                datosBasicos.fechaNacimiento = reader.GetDateTime(3).Date.ToString("yyyy-MM-dd");//
                                datosBasicos.celular = reader.GetString(4);
                                datosBasicos.direccion = reader.GetString(5);

                                epsSelected.idEps = reader.GetInt32(6).ToString();
                                epsSelected.nombre = reader.GetString(7);

                                jardinSelected.idJardin = reader.GetInt32(8).ToString();
                                jardinSelected.nombre = reader.GetString(9);

                                acudienteSelected.idUsuario = reader.GetInt32(10).ToString();
                                datosBasicosAcudiente.idDatosBasicos = reader.GetInt32(11).ToString();
                                datosBasicosAcudiente.identificacion = reader.GetString(12);
                                acudienteSelected.datosBasicos = datosBasicosAcudiente;

                                ninio.idNinio = "" + reader.GetInt32(13);
                                ninio.ciudadNacimiento = reader.GetString(14);
                                ninio.tipoSangre = reader.GetString(15);
                                ninio.datosBasicos = datosBasicos;
                                ninio.jardin = jardinSelected;
                                ninio.acudiente = acudienteSelected;
                                ninio.eps = epsSelected;
                            } 
                        }
                    }

                    String sqlEps = "SELECT idEps, nombre FROM eps;";
                    using (SqlCommand command = new SqlCommand(sqlEps, connection))
                    {
                        command.Parameters.AddWithValue("@idEps", epsSelected.idEps);
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

                    String sqlJardines = "SELECT idJardin, nombre FROM jardines;";
                    using (SqlCommand command = new SqlCommand(sqlJardines, connection))
                    {
                        command.Parameters.AddWithValue("@idJardin", jardinSelected.idJardin);
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
                    string idDatosBasicos = Request.Form["idDatosBasicos"];
                    string idNino = Request.Form["idNino"];

                    String sqlUpdate = "UPDATE DatosBasicos SET " +
                        "identificacion = @identificacion, nombres = @nombres, fechaNacimiento = @fechaNacimiento, " +
                        "celular = @celular, direccion = @direccion, idTipoDocumento = @tipoDocumento " +
                        "WHERE idDatosBasicos = @idDatosBasicos";

                    using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@identificacion", identificacion);
                        command.Parameters.AddWithValue("@nombres", nombres);
                        command.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        command.Parameters.AddWithValue("@celular", celular);
                        command.Parameters.AddWithValue("@direccion", direccion);
                        command.Parameters.AddWithValue("@tipoDocumento", tipoDocId);
                        command.Parameters.AddWithValue("@idDatosBasicos", idDatosBasicos);

                        command.ExecuteNonQuery();
                    }

                    String sqlUpdateNino = "UPDATE ninos SET tipoSangre = @tipoSangre, ciudadNacimiento = @ciudadNacimiento, " +
                        "idJardin = @idJardin, idUsuario = @idAcudiente, idDatosBasicos = @idDatosBasicos, idEps = @idEps " +
                            "WHERE idNino = @idNino;"; 
                    using (SqlCommand command2 = new SqlCommand(sqlUpdateNino, connection))
                    {
                        command2.Parameters.AddWithValue("@tipoSangre", tipoSangre);
                        command2.Parameters.AddWithValue("@ciudadNacimiento", ciudadNacimiento);
                        command2.Parameters.AddWithValue("@idJardin", jardinId);
                        command2.Parameters.AddWithValue("@idAcudiente", acudienteId);
                        command2.Parameters.AddWithValue("@idDatosBasicos", idDatosBasicos);
                        command2.Parameters.AddWithValue("@idEps", epsId);
                        command2.Parameters.AddWithValue("@idNino", idNino);

                        command2.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Niño Editado exitosamente";
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
