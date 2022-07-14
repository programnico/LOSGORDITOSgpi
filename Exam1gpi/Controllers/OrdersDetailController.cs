using Exam1gpi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1gpi.Controllers
{
    public class OrdersDetailController : Controller
    {
        test1gpiEntities context = new test1gpiEntities();
        public ActionResult OrdersDetail()
        {
            //validamos que este logueado
            if (System.Web.HttpContext.Current.Session["nivel"] == null)
            {
                return RedirectToAction("../Login/Login");
            }
            else
            {
                ViewBag.products = context.product.Select(x => new SelectListItem
                {
                    Text = x.nameProduct,
                    Value = x.codProduct.ToString()
                });
                ViewBag.unitMeasurements = context.unitMeasurement.Select(x => new SelectListItem
                {
                    Text = x.nameUnitMeasurement,
                    Value = x.codUnitMeasurement.ToString()
                });

                if (Session["PRODUCTOS"] != null)
                {
                    ViewBag.itemPedido = Session["PRODUCTOS"];
                }

                if (TempData["msj"] != null)
                {
                    ViewBag.msj = TempData["msj"];
                }
                return View();
            }
        }

        [HttpPost]
        public ActionResult OrdersAddCart(ordersDetail od, string cmbUnidadMedida)
        {

            if(od.quantity <= 0)
            {
                TempData["msj"] = "noAplica";
                return RedirectToAction("OrdersDetail");
            }

            string accion = Request.Form["boton"].ToString();
            switch (accion)
            {
                case "Agregar al pedido":
                    List<Cart> listadoProductos;

                    if (Session["PRODUCTOS"] == null)
                    {
                        listadoProductos = new List<Cart>();
                    }
                    else
                    {
                        listadoProductos = Session["PRODUCTOS"] as List<Cart>;
                    }

                    var codUnitm = int.Parse(cmbUnidadMedida);
                    //get product
                    product tempProducto = context.product.FirstOrDefault(x => x.codProduct == od.codProduct);
                    //get productUnitMeasurement
                    productUnitMeasurement tempUM = context.productUnitMeasurement.FirstOrDefault(
                        x => x.codProduct == od.codProduct && x.codUnitMeasurement == codUnitm);

                    if (tempUM != null)
                    {
                        //check if setting is valid

                        settingProductSupply tempSPS = context.settingProductSupply.FirstOrDefault(
                            t => t.codProductUnitMeasurement == tempUM.codProductUnitMeasurement);

                        if (tempSPS != null)
                        {
                            var valor = context.settingProductSupply.Where(
                                t => t.codProductUnitMeasurement == tempUM.codProductUnitMeasurement
                                ).Sum(i => i.quantity);

                            if (tempUM.unitMeasurement.nameUnitMeasurement.Equals("QUINTAL"))
                            {
                                if (valor == 100)
                                {
                                    // check stock supply (create concentrated)
                                    List<settingProductSupply> listadoSPS = context.settingProductSupply.Where(
                                        t => t.codProductUnitMeasurement == tempUM.codProductUnitMeasurement
                                        ).ToList();
                                    var flag = false;
                                    foreach (settingProductSupply listado in listadoSPS)
                                    {
                                        var value = listado.quantity * od.quantity;

                                        if (value <= listado.supply.stock)
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            flag = false;
                                            TempData["msj"] = "ingreseSuministros";
                                            /*
                                             * do
                                             * return view
                                             */
                                            break;
                                        }
                                    }

                                    if (flag)
                                    {
                                        /**
                                         * add cart
                                         */
                                        var flagExists = false;
                                        if (listadoProductos != null)
                                        {

                                            foreach (Cart itemLs in listadoProductos)
                                            {
                                                if (itemLs.codProduct == od.codProduct)
                                                {
                                                    flagExists = true;
                                                    TempData["msj"] = "existeEnElListado";
                                                    break;
                                                }
                                            }

                                            if (flagExists)
                                            {
                                                return RedirectToAction("OrdersDetail");
                                            }
                                            else
                                            {
                                                Cart c = new Cart();
                                                c.codProduct = od.codProduct;
                                                c.nameProduct = tempProducto.nameProduct;
                                                c.quantity = od.quantity;
                                                c.codUnitMeasurement = codUnitm;
                                                c.nameUnitMeasurement = tempUM.unitMeasurement.nameUnitMeasurement;
                                                listadoProductos.Add(c);
                                                Session["PRODUCTOS"] = listadoProductos;
                                                TempData["msj"] = "agregadoListado";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("OrdersDetail");
                                    }
                                }
                                else
                                {
                                    TempData["msj"] = "configIncorrectaQ";
                                }
                            }
                            else if (tempUM.unitMeasurement.nameUnitMeasurement.Equals("LIBRA"))
                            {
                                if (valor == 16)
                                {
                                    // check stock supply (create concentrated)
                                    List<settingProductSupply> listadoSPS = context.settingProductSupply.Where(
                                        t => t.codProductUnitMeasurement == tempUM.codProductUnitMeasurement
                                        ).ToList();
                                    var flag = false;
                                    foreach (settingProductSupply listado in listadoSPS)
                                    {

                                        //convert ozs to pounds 
                                        // and operate correctly
                                        decimal value = (listado.quantity * od.quantity) / 16m;

                                        if (value <= listado.supply.stock)
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            flag = false;
                                            TempData["msj"] = "ingreseSuministros";
                                            /*
                                             * do
                                             * return view
                                             */

                                            break;
                                        }
                                    }

                                    if (flag)
                                    {
                                        /**
                                         * add cart
                                         */
                                        var flagExists = false;
                                        if (listadoProductos != null)
                                        {

                                            foreach (Cart itemLs in listadoProductos)
                                            {
                                                if (itemLs.codProduct == od.codProduct)
                                                {
                                                    flagExists = true;
                                                    TempData["msj"] = "existeEnElListado";
                                                    break;
                                                }
                                            }

                                            if (flagExists)
                                            {
                                                return RedirectToAction("OrdersDetail");
                                            }
                                            else
                                            {
                                                Cart c = new Cart();
                                                c.codProduct = od.codProduct;
                                                c.nameProduct = tempProducto.nameProduct;
                                                c.quantity = od.quantity;
                                                c.codUnitMeasurement = codUnitm;
                                                c.nameUnitMeasurement = tempUM.unitMeasurement.nameUnitMeasurement;
                                                listadoProductos.Add(c);
                                                Session["PRODUCTOS"] = listadoProductos;
                                                TempData["msj"] = "agregadoListado";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("OrdersDetail");
                                    }
                                }
                                else
                                {
                                    TempData["msj"] = "configIncorrectaL";
                                }
                            }
                        }
                        else
                        {
                            TempData["msj"] = "NoExisteConfig";
                            return RedirectToAction("OrdersDetail");
                        }
                    }
                    else
                    {
                        TempData["msj"] = "NoUnidadMedida";
                    }

                    //check if quantity is valid (db)
                    //supply temps = 

                    break;
            }
            return RedirectToAction("OrdersDetail");
        }

        public ActionResult OrdersDetailRemove(int? id)
        {
            if (id != null)
            {
                List<Cart> list = (List<Cart>)Session["PRODUCTOS"];
                Cart cartDelete = new Cart();

                foreach (Cart itList in list)
                {
                    if (itList.codProduct == id)
                    {
                        cartDelete = itList;
                    }
                }

                if (cartDelete != null)
                {
                    list.Remove(cartDelete);
                }

                if (list.Count() == 0)
                {
                    Session["PRODUCTOS"] = null;
                }
                else
                {
                    Session["PRODUCTOS"] = list;
                }
            }
            return RedirectToAction("OrdersDetail");
        }

        public void subtractStock(Cart c)
        {

            productUnitMeasurement tempPUM = context.productUnitMeasurement.
                FirstOrDefault(x => x.codProduct == c.codProduct && x.codUnitMeasurement == c.codUnitMeasurement);

            if (tempPUM.unitMeasurement.nameUnitMeasurement.Equals("QUINTAL")) 
            {
                List<settingProductSupply> listadoSPS = context.settingProductSupply.
                    Where(t => t.codProductUnitMeasurement == tempPUM.codProductUnitMeasurement).ToList();

                //update supplies stock
                foreach (settingProductSupply settingps in listadoSPS)
                {
                    var subtractQuantity = settingps.quantity * c.quantity;
                    supply temp = context.supply.FirstOrDefault(x => x.codSupply == settingps.codSupply);
                    var updateQuantity = temp.stock - subtractQuantity;
                    temp.stock = updateQuantity;
                    context.SaveChanges();

                    createMovementSupply(subtractQuantity, settingps.codSupply);
                }
                //supply tempSu = context.supply.FirstOrDefault(x => x.codSupply == s.codSupply)
            }
            else if (tempPUM.unitMeasurement.nameUnitMeasurement.Equals("LIBRA"))
            {
                List<settingProductSupply> listadoSPS = context.settingProductSupply
                    .Where(t => t.codProductUnitMeasurement == tempPUM.codProductUnitMeasurement).ToList();

                //update supplies stock
                foreach (settingProductSupply settingps in listadoSPS)
                {
                    var subtractQuantity = (settingps.quantity * c.quantity)/16m;
                    supply temp = context.supply.FirstOrDefault(x => x.codSupply == settingps.codSupply);
                    var updateQuantity = temp.stock - subtractQuantity;
                    temp.stock = updateQuantity;
                    context.SaveChanges();

                    createMovementSupply(subtractQuantity, settingps.codSupply);
                }
            }

            //supply tempSu = context.supply.FirstOrDefault(x => x.codSupply == s.codSupply);
        }

        public void createMovementSupply(decimal cantidad, int codigoSupply)
        {
            supplyMovement sm = new supplyMovement();
            sm.codSupply = codigoSupply;
            sm.quantity = cantidad;
            sm.tipo = 2;
            context.supplyMovement.Add(sm);
            context.SaveChanges();
        }

        public ActionResult OrdersDetailCreateOrder()
        {

            List<Cart> list = (List<Cart>)Session["PRODUCTOS"];

            DateTime today = DateTime.Now;

            try
            {
                if (list.Count() > 0)
                {
                    orders o = new orders();
                    o.codPerson = int.Parse((string) Session["codigo"]); //session login code save //user who created the order
                    o.dateOrder = today.Date;
                    o.status = 1;
                    context.orders.Add(o);
                    context.SaveChanges();

                    foreach (Cart c in list)
                    {
                        //firts
                        subtractStock(c);
                        //save
                        ordersDetail od = new ordersDetail();
                        od.codOrder = o.codOrder;
                        od.codProduct = c.codProduct;
                        od.quantity = c.quantity;

                        context.ordersDetail.Add(od);
                        context.SaveChanges();
                    }
                }
                Session["PRODUCTOS"] = null;
                TempData["msj"] = "Guardado";
            }
            catch (Exception e)
            {
                Console.WriteLine("error" + e);
            }

            return RedirectToAction("OrdersDetail");
        }

    }

    public class Cart
    {
        public int codProduct { get; set; }
        public string nameProduct { get; set; }
        public int quantity { get; set; }
        public int codUnitMeasurement { get; set; }
        public string nameUnitMeasurement { get; set; }
    }
}