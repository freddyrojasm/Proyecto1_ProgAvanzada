using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebFitnessProy1.Models;

namespace WebFitnessProy1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Acceder()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registrar(string email, string clave, string confirmarClave, string nombre,
            decimal pesoActual, int altura, DateTime fechaNacimiento, string genero, HttpPostedFileBase imagen)
        {

            Usuario pUsuario = new Usuario();

            bool registrado;
            string mensaje;

            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (imagen != null && imagen.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(imagen.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(imagen.ContentLength);
                    }
                }

                //Asignación de valores en la entidad usuario
                pUsuario.Imagen = imageData;
                pUsuario.Altura = altura;
                pUsuario.Email = email;
                pUsuario.Genero = genero;
                pUsuario.ConfirmarClave = confirmarClave;
                pUsuario.Nombre = nombre;
                pUsuario.FechaNacimiento = fechaNacimiento;
                pUsuario.Clave = clave;
                pUsuario.PesoActual = pesoActual;


                if (pUsuario.Clave == confirmarClave)
                {
                    pUsuario.Clave = EncriptarSha256(clave);
                }
                else
                {
                    ViewData["Mensaje"] = "No coinciden las contraseñas ingresadas. ¡Por favor revise!";
                    return View();
                }
                using (SqlConnection con = DBConexion.ObtenerConeccion())
                {

                    SqlCommand comando = new SqlCommand("pr_CrearUsuario", con);
                    comando.Parameters.AddWithValue("Correo", pUsuario.Email);
                    comando.Parameters.AddWithValue("Clave", pUsuario.Clave);
                    comando.Parameters.AddWithValue("Nombre", pUsuario.Nombre);
                    comando.Parameters.AddWithValue("FechaNacimiento", pUsuario.FechaNacimiento);
                    comando.Parameters.AddWithValue("Altura", pUsuario.Altura);
                    comando.Parameters.AddWithValue("PesoActual", pUsuario.PesoActual);
                    comando.Parameters.AddWithValue("Genero", pUsuario.Genero);
                    comando.Parameters.AddWithValue("Imagen", (object)pUsuario.Imagen ?? DBNull.Value);
                    comando.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    comando.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(comando.Parameters["Registrado"].Value);
                    mensaje = Convert.ToString(comando.Parameters["Mensaje"].Value);
                }
                ViewData["Mensaje"] = mensaje;

                if (registrado)
                {
                    return RedirectToAction("Acceder", "Login");
                }

            }
            return View();
        }


        [HttpPost]
        public ActionResult Acceder(Usuario pUsuario)
        {

            pUsuario.Clave = EncriptarSha256(pUsuario.Clave);

            using (SqlConnection con = DBConexion.ObtenerConeccion())
            {

                SqlCommand comando = new SqlCommand("pr_ValidarUsuario", con);
                comando.Parameters.AddWithValue("Correo", pUsuario.Email);
                comando.Parameters.AddWithValue("Clave", pUsuario.Clave);
                con.Open();
                comando.CommandType = CommandType.StoredProcedure;

                pUsuario.Id = Convert.ToInt32(comando.ExecuteScalar().ToString());

            }

            if (pUsuario.Id != 0)
            {
                Session["usuario"] = pUsuario.Email;
                Session["UserId"] = pUsuario;
                return RedirectToAction("Registrar", "ActividadFisica");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no existe en la base de datos";
                return View();
            }
        }



        //Seguridad con encriptado SHA256
        public static string EncriptarSha256(string txt)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] resultado = hash.ComputeHash(enc.GetBytes(txt));

                foreach (byte b in resultado)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}