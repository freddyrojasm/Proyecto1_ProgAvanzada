using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFitnessProy1.Models;
using System.Configuration;
using System.Web.DynamicData;

namespace WebFitnessProy1.Controllers
{
    public class DietaController : Controller
    {
        // GET: Dieta
        public ActionResult RegistrarDieta()
        {
            // Obtener el nombre de usuario de la sesión
            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };

            // Pasar el objeto Usuario a la vista

            if (usuario.Email == null) {

                return RedirectToAction("Acceder", "Login");
            }

            return View(usuario);
        }


        public ActionResult MostrarDieta()
        {

            string nombreUsuario = (string)Session["Usuario"];
            var usuario = new Usuario { Email = nombreUsuario };


            if (usuario.Email != null)
            {
             DataTable dietaTable = new DataTable();

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();   
                string query = "SELECT * FROM Dieta WHERE Correo = @IdUsuario"; 
                
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@IdUsuario",usuario.Email);
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
            Dieta dieta = ObtenerDietaDesdeBD(id);

            ViewBag.Id = dieta.Id;
            ViewBag.Correo = dieta.Email;
            ViewBag.FechaComida = dieta.FechaComida;
            ViewBag.TipoComida = dieta.TipoComida;
            ViewBag.AlimentosConsumidos = dieta.AlimentosConsumidos;
            ViewBag.CaloriasTotales = dieta.CaloriasTotales;
            ViewBag.Comentarios = dieta.Comentarios;

            return View();

        }

        [HttpPost]
        public ActionResult Actualizar(Dieta dieta)
        {
            string query = "UPDATE Dieta SET Correo = @Correo, FechaComida = @FechaComida, TipoComida = @TipoComida," +
                " AlimentosConsumidos = @AlimentosConsumidos, CaloriasTotales = @CaloriasTotales, Comentarios = @Comentarios" +
                " WHERE Id = @Id";

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", dieta.Id);
                command.Parameters.AddWithValue("@Correo", dieta.Email);
                command.Parameters.AddWithValue("@FechaComida", dieta.FechaComida);
                command.Parameters.AddWithValue("@TipoComida", dieta.TipoComida);
                command.Parameters.AddWithValue("@AlimentosConsumidos", dieta.AlimentosConsumidos);
                command.Parameters.AddWithValue("@CaloriasTotales", dieta.CaloriasTotales);
                command.Parameters.AddWithValue("@Comentarios", dieta.Comentarios);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("MostrarDieta", "Dieta");
        }


        public ActionResult Eliminar(int id)
        {
            string query = "DELETE FROM Dieta WHERE Id = @Id";

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }

            return RedirectToAction("MostrarDieta");
        }


        [HttpPost]
        public ActionResult RegistrarDieta(FormCollection form)
        {
            // Obtener los datos del formulario
            var dieta = new Dieta
            {
                Email = form["IdUsuario"],
                FechaComida = DateTime.Parse(form["FechaComida"]),
                TipoComida = form["TipoComida"],
                AlimentosConsumidos = form["AlimentosConsumidos"],
                CaloriasTotales = Convert.ToInt32(form["CaloriasTotales"]),
                Comentarios = form["Comentarios"]
            };
            // Guardado de los datos en la base de datos
            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand("GuardarDieta", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Correo", dieta.Email);
                command.Parameters.AddWithValue("@FechaComida", dieta.FechaComida);
                command.Parameters.AddWithValue("@TipoComida", dieta.TipoComida);
                command.Parameters.AddWithValue("@AlimentosConsumidos", dieta.AlimentosConsumidos);
                command.Parameters.AddWithValue("@CaloriasTotales", dieta.CaloriasTotales);
                command.Parameters.AddWithValue("@Comentarios", dieta.Comentarios);

                command.ExecuteNonQuery();
            }

            TempData["SuccessMessage"] = "¡La dieta se ha registrado correctamente!";


            return RedirectToAction("RegistrarDieta", "Dieta");

        }


        private Dieta ObtenerDietaDesdeBD(int id)
        {
            string query = "SELECT Id, Correo, FechaComida, TipoComida, AlimentosConsumidos, " +
                "CaloriasTotales, Comentarios FROM Dieta WHERE Id = @Id";

            Dieta dieta = new Dieta();

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    dieta.Id = (int)reader["Id"];
                    dieta.Email = reader["Correo"].ToString();
                    dieta.FechaComida = (DateTime)reader["FechaComida"];
                    dieta.TipoComida = reader["TipoComida"].ToString();
                    dieta.AlimentosConsumidos = reader["AlimentosConsumidos"].ToString();
                    dieta.CaloriasTotales = (int)reader["CaloriasTotales"];
                    dieta.Comentarios = reader["Comentarios"].ToString();
                }
                reader.Close();
            }
            return dieta;
        }
    }
}