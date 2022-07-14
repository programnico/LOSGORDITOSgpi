using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//referencia a la carpeta modelo
using Exam1gpi.Models;

namespace Exam1gpi.Controllers
{
    public class LoginController : Controller
    {
        test1gpiEntities contexto = new test1gpiEntities();
        // GET: Login
        /*---------------------------------------------Login------------------------------------------------------*/
        public ActionResult Login(string txtUsuario, string txtContrasennia)
        {
            string usuario = txtUsuario;
            string contrasennia = txtContrasennia;
            string nivel = "";
            if (usuario != null && contrasennia != null)
            {
                var user = contexto.person.Where(x => x.email.ToString() == txtUsuario && x.password.ToString() == contrasennia).ToList();
                if (user.Count == 0)
                {
                    nivel = "";
                }
                else
                {
                    nivel = user[0].roles.nameRole;
                    System.Web.HttpContext.Current.Session["nombre"] = user[0].namePerson;
                    System.Web.HttpContext.Current.Session["codigo"] = user[0].codPerson;
                }
            }


            if (!nivel.Equals(""))
            {
                System.Web.HttpContext.Current.Session["nivel"] = nivel;
                System.Web.HttpContext.Current.Session["usuario"] = usuario;

                return RedirectToAction("../Home/Index");//para el metodo se va
            }
            else
            {
                return View();
            }
        }
    }
}