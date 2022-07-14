using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class ProductController : Controller
    {

        //context first
        test1gpiEntities context = new test1gpiEntities();

        public ActionResult Product()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["nivel"].ToString() == "SECRETARIA")
                {
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    List<product> data = context.product.ToList();
                    ViewBag.dataP = data;

                    if (TempData["msj"] != null)
                    {
                        ViewBag.msj = TempData["msj"];
                    }

                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult Product(product p)
        {
            if (ModelState.IsValid) {    
                string accion = Request.Form["boton"].ToString();
                
                switch (accion)
                {
                    case "Guardar":
                        try
                        {
                            context.product.Add(p);
                            context.SaveChanges();
                            TempData["msj"] = "Guardado";

                        }
                        catch (Exception e)
                        {
                            TempData["msj"] = "VerificarDatos";
                        }
                        break;
                    case "Eliminar":
                        try
                        {
                            context.product.Remove(context.product.FirstOrDefault(x => x.codProduct == p.codProduct));
                            context.SaveChanges();
                            TempData["msj"] = "Eliminado";
                        }
                        catch (Exception e)
                        {
                            TempData["msj"] = "NoEliminado";
                        }

                        
                        break;
                    case "Modificar":
                        
                        product temp = context.product.FirstOrDefault(x => x.codProduct == p.codProduct);
                        temp.nameProduct = p.nameProduct;
                        context.SaveChanges();
                        TempData["msj"] = "Modificado";
                        
                        break;
                }
            }
            return RedirectToAction("Product");
        }
    }
}