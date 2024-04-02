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
    public class ActividadFisicaController : Controller
    {
        // GET: ActividadFisica
        public ActionResult Registrar()
        {
            // Obtener el nombre de usuario de la sesión
            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };

            //validar si el usuario está conectado
            if (usuario.Email == null)
            {

                return RedirectToAction("Acceder", "Login");
            }


            return View(usuario);
        }


        public ActionResult MostrarActividad()
        {

            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };


            if (usuario.Email != null)
            {
                DataTable actividadTable = new DataTable();

                using (SqlConnection con = DBConexion.ObtenerConeccion())
                {
                    con.Open();
                    string query = "SELECT * FROM ActividadFisica WHERE Correo = @Correo";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@Correo", usuario.Email);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(actividadTable);
                    return View(actividadTable);
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
            ActividadFisica actFisica = ObtenerActividadDesdeBD(id);

            ViewBag.Id = actFisica.Id;
            ViewBag.IdUsuario = actFisica.Email;
            ViewBag.FechaHora = actFisica.FechaHora;
            ViewBag.Duracion = actFisica.Duracion;
            ViewBag.DistanciaRecorrida = actFisica.DistanciaRecorrida;
            ViewBag.TipoActividad = actFisica.TipoActividad;
            ViewBag.CaloriasQuemadas = actFisica.CaloriasQuemadas;
            ViewBag.Comentarios = actFisica.Comentarios;

            return View();

        }


        [HttpPost]
        public ActionResult Registrar(FormCollection form)
        {
            // Utilizar los datos del formulario
            string idUsuario = form["IdUsuario"];
            string tipoActividad = form["tipoActividad"];
            DateTime fechaHora = DateTime.Parse(form["fechaHora"]);
            int duracion = Convert.ToInt32(form["duracion"]);
            int distanciaRecorrida = Convert.ToInt32(form["distanciaRecorrida"]);
            int caloriasQuemadas = Convert.ToInt32(form["caloriasQuemadas"]);
            string comentarios = form["comentarios"];

            // Guardado de los datos en la base de datos
            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand("GuardarActividadFisica", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Correo", idUsuario);
                command.Parameters.AddWithValue("@TipoActividad", tipoActividad);
                command.Parameters.AddWithValue("@FechaHora", fechaHora);
                command.Parameters.AddWithValue("@Duracion", duracion);
                command.Parameters.AddWithValue("@DistanciaRecorrida", distanciaRecorrida);
                command.Parameters.AddWithValue("@CaloriasQuemadas", caloriasQuemadas);
                command.Parameters.AddWithValue("@Comentarios", comentarios);
                command.ExecuteNonQuery();
            }

            //return View();
            return RedirectToAction("Registrar", "ActividadFisica");

        }



        [HttpPost]
        public ActionResult Actualizar(ActividadFisica actFisica)
        {
            string query = "UPDATE ActividadFisica SET Correo = @Correo, TipoActividad = @TipoActividad, FechaHora = @FechaHora, " +
                "Duración = @Duracion, DistanciaRecorrida = @DistanciaRecorrida, CaloriasQuemadas = @CaloriasQuemadas, Comentarios = @Comentarios" +
                " WHERE IdActividad = @Id";

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", actFisica.Id);
                command.Parameters.AddWithValue("@Correo", actFisica.Email);
                command.Parameters.AddWithValue("@TipoActividad", actFisica.TipoActividad);
                command.Parameters.AddWithValue("@FechaHora", actFisica.FechaHora);
                command.Parameters.AddWithValue("@Duracion", actFisica.Duracion);
                command.Parameters.AddWithValue("@DistanciaRecorrida", actFisica.DistanciaRecorrida);
                command.Parameters.AddWithValue("@CaloriasQuemadas", actFisica.CaloriasQuemadas);
                command.Parameters.AddWithValue("@Comentarios", actFisica.Comentarios);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("MostrarActividad", "ActividadFisica");
        }



        public ActionResult Eliminar(int id)
        {
            string query = "DELETE FROM ActividadFisica WHERE IdActividad = @Id";

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("MostrarActividad", "ActividadFisica");
        }



        private ActividadFisica ObtenerActividadDesdeBD(int id)
        {
            string query = "SELECT IdActividad, Correo, TipoActividad, FechaHora, Duracion, DistanciaRecorrida, CaloriasQuemadas, " +
                "Comentarios FROM ActividadFisica WHERE IdActividad = @Id";

            ActividadFisica ActFisica = new ActividadFisica();

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ActFisica.Id = (int)reader["IdActividad"];
                    ActFisica.Email = reader["Correo"].ToString();
                    ActFisica.FechaHora = (DateTime)reader["FechaHora"];
                    ActFisica.Duracion = (int)reader["Duracion"];
                    ActFisica.DistanciaRecorrida = (int)reader["DistanciaRecorrida"];
                    ActFisica.TipoActividad = reader["TipoActividad"].ToString();
                    ActFisica.CaloriasQuemadas = (int)reader["CaloriasQuemadas"];
                    ActFisica.Comentarios = reader["Comentarios"].ToString();
                }
                reader.Close();
            }
            return ActFisica;
        }

    }
}