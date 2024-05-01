using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFitnessProy1.Models;

namespace WebFitnessProy1.Controllers
{
    public class MetaSaludController : Controller
    {
        // GET: MetaSalud
        public ActionResult RegistrarMetaSalud()
        {
            // Obtener el nombre de usuario de la sesión
            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };

            // Pasar el objeto Usuario a la vista

            if (usuario.Email == null)
            {

                return RedirectToAction("Acceder", "Login");
            }

            return View(usuario);
        }


        public ActionResult MostrarMetaSalud()
        {

            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };


            if (usuario.Email != null)
            {
                DataTable dietaTable = new DataTable();

                using (SqlConnection con = DBConexion.ObtenerConeccion())
                {
                    con.Open();
                    string query = "SELECT * FROM MetaSalud WHERE Correo = @IdUsuario";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@IdUsuario", usuario.Email);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(dietaTable);
                    return View(dietaTable);
                }
            }
            else
            {
                return RedirectToAction("Acceder", "Login");

            }

        }


        public ActionResult Editar(int id)
        {

            // Cargar los datos en el objeto dieta
            MetaSalud metaSalud = ObtenerMetaSaludDesdeBD(id);

            ViewBag.Id = metaSalud.IdMeta;
            ViewBag.Correo = metaSalud.Email;
            ViewBag.TipoMeta = metaSalud.TipoMeta;
            ViewBag.PesoObjetivo = metaSalud.PesoObjetivo;
            ViewBag.FechaObjetivo = metaSalud.FechaObjetivo;
            ViewBag.NivelActividadDes = metaSalud.NivelActividadDes;
            ViewBag.ObjEspecificos = metaSalud.ObjEspecificos;

            return View();

        }

        [HttpPost]
        public ActionResult Actualizar(MetaSalud metaSalud)
        {
            string query = "UPDATE MetaSalud SET Correo = @Correo, TipoMeta = @TipoMeta, PesoObjetivo = @PesoObjetivo," +
                " FechaObjetivo = @FechaObjetivo, NivelActividadDes = @NivelActividadDes, ObjEspecificos = @ObjEspecificos" +
                " WHERE IdMeta = @Id";

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {   
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@IdMeta", metaSalud.IdMeta);
                command.Parameters.AddWithValue("@Correo", metaSalud.Email);
                command.Parameters.AddWithValue("@TipoMeta", metaSalud.TipoMeta);
                command.Parameters.AddWithValue("@PesoObjetivo", metaSalud.PesoObjetivo);
                command.Parameters.AddWithValue("@FechaObjetivo", metaSalud.FechaObjetivo);
                command.Parameters.AddWithValue("@NivelActividadDes", metaSalud.NivelActividadDes);
                command.Parameters.AddWithValue("@ObjEspecificos", metaSalud.ObjEspecificos);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("MostrarMetaSalud", "MetaSalud");
        }


        public ActionResult Eliminar(int id)
        {
            string query = "DELETE FROM MetaSalud WHERE IdMeta = @Id";

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@IdMeta", id);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("MostrarMetaSalud");
        }


        [HttpPost]
        public ActionResult RegistrarMetaSalud(FormCollection form)
        {
            // Obtener los datos del formulario
            var metaSalud = new MetaSalud
            {
                Email = form["Email"],
                FechaObjetivo = DateTime.Parse(form["FechaObjetivo"]),
                PesoObjetivo = Convert.ToDecimal(form["PesoObjetivo"]),
                TipoMeta = form["TipoMeta"],
                NivelActividadDes = form["NivelActividadDes"],
                ObjEspecificos = form["ObjEspecificos"]
            };
            // Guardado de los datos en la base de datos
            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand("GuardarMetaSalud", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Correo", metaSalud.Email);
                command.Parameters.AddWithValue("@FechaObjetivo", metaSalud.FechaObjetivo);
                command.Parameters.AddWithValue("@PesoObjetivo", metaSalud.PesoObjetivo);
                command.Parameters.AddWithValue("@TipoMeta", metaSalud.TipoMeta);
                command.Parameters.AddWithValue("@NivelActividadDes", metaSalud.NivelActividadDes);
                command.Parameters.AddWithValue("@ObjEspecificos", metaSalud.ObjEspecificos);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("RegistrarMetaSalud", "MetaSalud");

        }


        private MetaSalud ObtenerMetaSaludDesdeBD(int id)
        {
            string query = "SELECT IdMeta, Correo, FechaObjetivo, PesoObjetivo, TipoMeta, " +
                "NivelActividadDes, ObjEspecificos FROM MetaSalud WHERE IdMeta = @Id";

            MetaSalud metaSalud = new MetaSalud();

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@IdMeta", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    metaSalud.IdMeta = (int)reader["IdMeta"];
                    metaSalud.Email = reader["Correo"].ToString();
                    metaSalud.FechaObjetivo = (DateTime)reader["FechaObjetivo"];
                    metaSalud.PesoObjetivo = (decimal)reader["PesoObjetivo"];
                    metaSalud.TipoMeta = reader["TipoMeta"].ToString();
                    metaSalud.NivelActividadDes = reader["NivelActividadDes"].ToString();
                    metaSalud.ObjEspecificos = reader["ObjEspecificos"].ToString();
                }
                reader.Close();

            }
            return metaSalud;
        }

    }
}