using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace Webshop.Controllers
{
    public class OrderController : Controller
    {
        private string errorFile = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";
        private string _stub;

        public OrderController(){
            _stub = "";
        }

        public OrderController(string rep){
            _stub = rep;
        }

        public ActionResult AdminOrders() {
            OrderLogic orderBll = new OrderLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(orderBll.GetAllOrders());
        }

        public ActionResult UnhandledOrders() {
            OrderLogic orderBll = new OrderLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(orderBll.GetUnhandledOrders());
        }

        public ActionResult Handle(int id=0) {
            OrderLogic orderBll = new OrderLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });

            orderBll.HandleOrder(id);
            return RedirectToAction("AdminOrders");
        }

        public ActionResult AdminDetails(int id = 0) {
            OrderLogic orderBll = new OrderLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });

            Order order = orderBll.GetOrder(id);
            if (order == null) {
                return RedirectToAction("Error", "Error");
            }
            return View(order);
        }

        public ActionResult ViewCart() {
            Order cart;
            if (Session["cart"] != null) {
                cart = (Order)Session["cart"];
                return View(cart);
            }
            return RedirectToAction("Index", "Item");
        }

        [HttpPost]
        public ActionResult ViewCart(FormCollection form) {
            Order order;
            User user;
            DateTime date = DateTime.Now;
            int nrOfItems;

            if (Session["cart"] == null) {
                ViewBag.error = "Handlekurven er tom";
                return View();
            }
            else {
                order = (Order)Session["cart"];
                nrOfItems = order.orderItems.Count;
                order.date = date;
                for (int i = 0; i < nrOfItems; i++) {   //To save amount of each item from form, and save it
                    try {
                        int amountIfItem = Convert.ToInt32(form[i]);
                        if (amountIfItem <= 0) {
                            ViewBag.error = "Vi er desverre tomme på lager for en eller flere av valgte varer. Vennligst slett vare fra handlekurv!";
                            return View(order);
                        }
                        order.orderItems[i].amount = amountIfItem;
                        
                    }
                    catch (FormatException e) {
                        var sw = new System.IO.StreamWriter(errorFile, true);
                        sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                        sw.Close();
                    }
                    catch (OverflowException e) {
                        var sw = new System.IO.StreamWriter(errorFile, true);
                        sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                        sw.Close();
                    }
                }
                if (Session["UserLoggedIn"] != null) {  //User is logged in
                    user = (User)Session["UserLoggedIn"];
                    user.orders.Add(order);
                    Session["UserLoggedIn"] = user;
                    return RedirectToAction("Checkout", "Order");
                }
                else {
                    return RedirectToAction("CheckoutNLI", "User");
                }
            }
        }

        public ActionResult DeleteFromCart(int id) {
            Order order;
            OrderItem orderItem;
            if (Session["cart"] != null) {
                order = (Order)Session["cart"];
                try {
                    orderItem = order.orderItems.First(i => i.item.itemId == id);
                    order.orderItems.Remove(orderItem);
                    if (order.orderItems.Count == 0) {
                        Session["cart"] = null;
                        return RedirectToAction("Index", "Item");
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                catch (InvalidOperationException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
            }
            return RedirectToAction("ViewCart");
        }

        // When clickin "Kjøp" on an item we'll add this item to the cart with AJAX

        public string ToCart(int id = 0) {
            Order order;
            string outString;
            if (id == 0) {   //If going back to mainpage or refreshing page.
                if (Session["cart"] == null)
                    return "Handlevogn";
                else {
                    order = (Order)Session["cart"];
                    int length = order.orderItems.Count;
                    if (length == 1)
                        outString = "Handlevogn(" + length + " vare)";
                    else
                        outString = "Handlevogn(" + length + " varer)";
                    return outString;
                }
            }
            else {
                ItemLogic itemBll = new ItemLogic(_stub);
                OrderItem orderItem = new OrderItem();
                Item item = itemBll.GetItem(id);

                if (Session["cart"] != null) {  //An order is allready created
                    order = (Order)Session["cart"];
                    try {
                        if (!order.orderItems.Exists(i => i.item.itemId == id)) {
                            orderItem.item = item;
                            order.orderItems.Add(orderItem);
                            Session["cart"] = order;
                        }
                    }
                    catch (ArgumentNullException e) {
                        var sw = new System.IO.StreamWriter(errorFile, true);
                        sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                        sw.Close();
                    }
                }
                else {
                    order = new Order();
                    orderItem = new OrderItem();
                    order.orderItems = new List<OrderItem>();
                    orderItem.item = item;
                    order.orderItems.Add(orderItem);
                    Session["cart"] = order;
                }
                int length = order.orderItems.Count;
                if (length == 1)
                    outString = "Handlevogn(" + length + " vare)";
                else
                    outString = "Handlevogn(" + length + " varer)";
                return outString;
            }
        }

        public ActionResult Checkout() {
            Order order;
            if (Session["cart"] != null)
                order = (Order)Session["cart"];
            else
                return RedirectToAction("Index", "Item");
            return View(order);
        }

        public ActionResult Payment() {
            OrderLogic orderBll = new OrderLogic(_stub);
            Order order;
            Person person;
            
            if (Session["cart"] != null) {
                order = (Order)Session["cart"];
            }
            else
                return RedirectToAction("Index", "Item");

            if (Session["UserLoggedIn"] != null) {
                person = (User)Session["UserLoggedIn"];
            }
            else {
                if (Session["AnonymousUser"] != null){
                    person = (Person)Session["AnonymousUser"];
                }      
                else
                    return RedirectToAction("Index", "Item");
            }
            if(orderBll.addOrder(order, person))
                return RedirectToAction("PaymentOK");
            else
                return RedirectToAction("Checkout");
        }

        public ActionResult paymentOK() {
            Session["cart"] = null;
            return View();
        }

        public ActionResult Details(int id = 0){
            OrderLogic orderBll = new OrderLogic(_stub);
            Order order = orderBll.GetOrder(id);

            if (order == null)
            {
                return RedirectToAction("Error", "Error");
            }
            return View(order);
        }
    }
}