using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class SettingProductSupplyController : Controller
    {
        test1gpiEntities context = new test1gpiEntities();
        public ActionResult SettingProductSupply()
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
                    List<settingProductSupply> data = context.settingProductSupply.ToList();
                    ViewBag.dataSPS = data;

                    ViewBag.supplies = context.supply.Select(x => new SelectListItem
                    {
                        Text = x.nameSupply,
                        Value = x.codSupply.ToString()
                    });

                    ViewBag.productUnitMeasurements = context.productUnitMeasurement.Select(x => new SelectListItem
                    {
                        Text = x.product.nameProduct + " - " + x.unitMeasurement.nameUnitMeasurement,
                        Value = x.codProductUnitMeasurement.ToString()
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
        public ActionResult SettingProductSupply(settingProductSupply sps)
        {
            if (ModelState.IsValid)
            {
                string accion = Request.Form["boton"].ToString();

                switch (accion)
                {
                    case "Guardar":

                        //checks if the limit based on the unit of measure is correct
                        //example
                        //1 quintal = 100 lb
                        //1 libra = 16ozs
                        //then
                        //first get the selected unit of measure

                        settingProductSupply temp0 = context.settingProductSupply.FirstOrDefault(x => x.codProductUnitMeasurement == sps.codProductUnitMeasurement);

                        if (temp0 != null)
                        {
                            if (temp0.productUnitMeasurement.unitMeasurement.nameUnitMeasurement.Equals("QUINTAL"))
                            {
                                //validate that the sum items equals 100 lb
                                var valor = context.settingProductSupply.Where(t => t.codProductUnitMeasurement == sps.codProductUnitMeasurement).Sum(i => i.quantity);

                                if (valor >= 100)
                                {
                                    TempData["msj"] = "mayora100";
                                    return RedirectToAction("SettingProductSupply");
                                }
                                else
                                {
                                    var nuevoVal = valor + sps.quantity;
                                    if (nuevoVal >= 101)
                                    {
                                        TempData["msj"] = "mayora100";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * save
                                         */
                                        insertar(sps);
                                    }
                                }
                            }
                            else
                            {
                                //validate that the sum item equals 16 0zs
                                var valor = context.settingProductSupply.Where(t => t.codProductUnitMeasurement == sps.codProductUnitMeasurement).Sum(i => i.quantity);


                                if (valor >= 16)
                                {
                                    TempData["msj"] = "mayora16";
                                    return RedirectToAction("SettingProductSupply");
                                }
                                else
                                {
                                    var nuevoVal = valor + sps.quantity;
                                    if (nuevoVal >= 17)
                                    {
                                        TempData["msj"] = "mayora16";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * save
                                         */
                                        insertar(sps);
                                    }
                                }

                            }
                        }
                        else
                        {
                            if(sps.codProductUnitMeasurement > 0)
                            {
                                productUnitMeasurement tempPUM = context.productUnitMeasurement.FirstOrDefault(x => x.codProductUnitMeasurement == sps.codProductUnitMeasurement);

                                if (tempPUM.unitMeasurement.nameUnitMeasurement.Equals("QUINTAL"))
                                {
                                    if (sps.quantity >= 101)
                                    {
                                        TempData["msj"] = "mayora100";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * save
                                         */
                                        insertar(sps);
                                    }
                                }
                                else if (tempPUM.unitMeasurement.nameUnitMeasurement.Equals("LIBRA"))
                                {
                                    if (sps.quantity >= 17)
                                    {
                                        TempData["msj"] = "mayora16";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * save
                                         */
                                        insertar(sps);
                                    }
                                }

                            }
                        }

                        break;
                    case "Eliminar":
                        context.settingProductSupply.Remove(context.settingProductSupply.FirstOrDefault(x => x.codSettingProductSupply == sps.codSettingProductSupply));
                        context.SaveChanges();
                        TempData["msj"] = "Eliminado";
                        break;
                    case "Modificar":

                        //checks if the limit based on the unit of measure is correct
                        //example
                        //1 quintal = 100 lb
                        //1 libra = 16ozs
                        //then
                        //first get the selected unit of measure

                        settingProductSupply tempps = context.settingProductSupply.FirstOrDefault(x => x.codProductUnitMeasurement == sps.codProductUnitMeasurement);

                        if (tempps != null)
                        {
                            if (tempps.productUnitMeasurement.unitMeasurement.nameUnitMeasurement.Equals("QUINTAL"))
                            {
                                //validate that the sum items equals 100 lb
                                
                                //pending optimization  (: 
                                List<settingProductSupply> listadoSPS = context.settingProductSupply.Where(t => t.codProductUnitMeasurement == sps.codProductUnitMeasurement).ToList();
                                List<settingProductSupply> temp3 = context.settingProductSupply.Where(x => x.codSupply == sps.codSupply && x.codProductUnitMeasurement == sps.codProductUnitMeasurement && x.codSettingProductSupply == sps.codSettingProductSupply).ToList();

                                if (listadoSPS.Count > temp3.Count )
                                {
                                    var listUpdate = listadoSPS.ToList().Except(temp3).ToList();

                                    var sumItem = listUpdate.Select(x=>x.quantity).Sum();

                                    var newValor = sumItem + sps.quantity;

                                    if (newValor >= 101)
                                    {
                                        TempData["msj"] = "mayora100";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * update here
                                         */
                                        update(sps);
                                    }
                                }
                                else
                                {
                                    if(sps.quantity >= 101)
                                    {
                                        TempData["msj"] = "mayora100";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * update here
                                         */
                                        update(sps);
                                    }
                                }
                            }else if (tempps.productUnitMeasurement.unitMeasurement.nameUnitMeasurement.Equals("LIBRA"))
                            {
                                List<settingProductSupply> listadoSPS = context.settingProductSupply.Where(t => t.codProductUnitMeasurement == sps.codProductUnitMeasurement).ToList();
                                List<settingProductSupply> temp3 = context.settingProductSupply.Where(x => x.codSupply == sps.codSupply && x.codProductUnitMeasurement == sps.codProductUnitMeasurement && x.codSettingProductSupply == sps.codSettingProductSupply).ToList();

                                if (listadoSPS.Count > temp3.Count)
                                {
                                    var listUpdate = listadoSPS.ToList().Except(temp3).ToList();

                                    var sumItem = listUpdate.Select(x => x.quantity).Sum();

                                    var newValor = sumItem + sps.quantity;

                                    if (newValor >= 17)
                                    {
                                        TempData["msj"] = "mayora16";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * update here
                                         */
                                        update(sps);
                                    }
                                }
                                else
                                {
                                    if (sps.quantity >= 16)
                                    {
                                        TempData["msj"] = "mayora16";
                                        return RedirectToAction("SettingProductSupply");
                                    }
                                    else
                                    {
                                        /*
                                         * update here
                                         */
                                        update(sps);
                                    }
                                }
                            }
                        }

                        
                        break;
                }
            }
            return RedirectToAction("SettingProductSupply");

        }

        public void insertar(settingProductSupply sps)
        {
            settingProductSupply temp1 = context.settingProductSupply.FirstOrDefault(x => x.codSupply == sps.codSupply && x.codProductUnitMeasurement == sps.codProductUnitMeasurement);
            if (temp1 != null)
            {
                TempData["msj"] = "Existe";
            }
            else
            {
                context.settingProductSupply.Add(sps);
                context.SaveChanges();
                TempData["msj"] = "Guardado";
            }
        }

        public void update(settingProductSupply sps)
        {
            settingProductSupply temp2 = context.settingProductSupply.FirstOrDefault(x => x.codSupply == sps.codSupply && x.codProductUnitMeasurement == sps.codProductUnitMeasurement && x.codSettingProductSupply != sps.codSettingProductSupply);
            if (temp2 != null)
            {
                TempData["msj"] = "Existe";
            }
            else
            {
                settingProductSupply temp = context.settingProductSupply.FirstOrDefault(x => x.codSettingProductSupply == sps.codSettingProductSupply);
                temp.codSupply = sps.codSupply;
                temp.quantity = sps.quantity;
                temp.codProductUnitMeasurement = sps.codProductUnitMeasurement;
                context.SaveChanges();
                TempData["msj"] = "Modificado";
            }
        }

    }
}