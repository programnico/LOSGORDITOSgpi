using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class PersonController : Controller
    {
        test1gpiEntities context = new test1gpiEntities();

        public ActionResult Person()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["nivel"].ToString() != "ADMINISTRADOR")
                {
                    return RedirectToAction("../Home/Index");
                }
                else
                {

                    List<person> data = context.person.ToList();
                    ViewBag.dataP = data;

                    ViewBag.roles = context.roles.Select(x => new SelectListItem
                    {
                        Text = x.nameRole,
                        Value = x.codRole.ToString()
                    });

                    if (TempData["msj"] != null)
                    {
                        ViewBag.msj = TempData["msj"];
                    }
                    return View();
                }
            }

        }

        [HttpPost]
        public ActionResult Person(person p)
        {
            if (ModelState.IsValid)
            {
                string accion = Request.Form["boton"].ToString();

                switch (accion)
                {
                    case "Guardar":
                        if (ModelState.IsValid)
                        {
                            context.person.Add(p);
                            context.SaveChanges();
                            TempData["msj"] = "Guardado";

                        }

                        break;
                    case "Eliminar":
                        try
                        {
                            context.person.Remove(context.person.FirstOrDefault(x => x.codPerson == p.codPerson));
                            context.SaveChanges();
                            TempData["msj"] = "Eliminado";
                        }
                        catch (Exception e)
                        {
                            TempData["msj"] = "NoEliminado";
                        }
                        
                        break;
                    case "Modificar":

                        person temp = context.person.FirstOrDefault(x => x.codPerson == p.codPerson);
                        temp.namePerson = p.namePerson;
                        temp.password = p.password;
                        context.SaveChanges();
                        TempData["msj"] = "Modificado";

                        break;
                }
            }
            return RedirectToAction("Person");
        }

    }
}