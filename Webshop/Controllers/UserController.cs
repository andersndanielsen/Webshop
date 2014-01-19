using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace Webshop.Controllers {

    public class UserController : Controller {
        private string errorFile = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";
        private string _stub;

        public UserController(){
            _stub = "";
        }

        public UserController(string rep){
            _stub = rep;
        }

        public ActionResult ControlPanel() {
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn");       //Can only view if logged in
            
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin) {
                return RedirectToAction("Error", "Error", new { status = 403 });
            }
            return View();
        }

        public ActionResult AdminUsers() {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(personBll.GetAllUsers());
        }

        public ActionResult AdminPersons() {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(personBll.GetAllPersons());
        }

        public ActionResult History(int id = 0) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(personBll.GetHistory(id));
        }

        public ActionResult LogIn(string returnUrl) {
            PersonLogic personBll = new PersonLogic(_stub);
            //returnUrl contains url to previus page, so we can return to this when logged in
            try {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0) {
                    return RedirectToAction("LogIn", new { returnUrl = Request.UrlReferrer.ToString() });
                }
            }
            catch (NotImplementedException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            if ((User)Session["UserLoggedIn"] != null) {
                Session["UserLoggedIn"] = null; //Logging out if logged in
                return Redirect(returnUrl);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(User user, string returnUrl) {
            PersonLogic personBll = new PersonLogic(_stub);
            User userFound = personBll.LogIn(user);

            if (userFound != null) {
                Session["UserLoggedIn"] = userFound;
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Item");
            }
            else {
                ViewBag.error = "Brukernavn eller passord er feil";
                return View();
            }
        }

        public ActionResult CheckoutNli() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckoutNli(User user) {    //Checkout when not logged in
            try {
                ModelState.Remove("userName");              //To pass validation, since this field is not sent with "user"
                ModelState.Remove("password");
                ModelState.Remove("secondPassword");
                Person person = new Person();
                List<Order> orderList = new List<Order>();
                if (ModelState.IsValid) {
                    person.firstName = user.firstName;
                    person.surName = user.surName;
                    person.address = user.address;
                    person.postcode = user.postcode;
                    person.postcodeArea = user.postcodeArea;
                    person.telephoneNumber = user.telephoneNumber;
                    person.orders = orderList;
                    Session["AnonymousUser"] = person;
                    return RedirectToAction("Checkout", "Order");
                }
            }
            catch (NotSupportedException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            catch (ArgumentNullException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            return View();
        }

        public ActionResult MyPage(int id = 0) {
            PersonLogic personBll = new PersonLogic(_stub);
            if(Session["UserLoggedIn"]==null)
                return RedirectToAction("LogIn");       //Can only view if logged in
            Person user = personBll.GetPerson(id);
            if (user == null) {
                return RedirectToAction("Error", "Error");
            }
            return View(user);
        }

        public ActionResult Create() {
            if (Session["UserLoggedIn"] != null)
                return RedirectToAction("Index", "Item");         //Can only create new user if logged out
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] != null)
                return RedirectToAction("Index", "Item");         //Can only create new user if logged out
            if (ModelState.IsValid) {
                int id = personBll.Create(user);
                if (id==0) {
                    ViewBag.userNameError = "Brukernavnet eksisterer fra før av";
                    return View(user);
                }
                User newUser = (User)personBll.GetPerson(id);
                personBll.SaveUserHistory(newUser, newUser, "Created");
                return RedirectToAction("LogIn");
            }
            return View(user);
        }

        public ActionResult Edit(int id = 0) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn");       //Can only view if logged in
            User user = (User)personBll.GetPerson(id);
            if (user == null) {
                return RedirectToAction("Error", "Error");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn");       //Can only view if logged in
            try {
                ModelState.Remove("userName");              //To pass validation, since this field is not sent with "user"
                if (ModelState.IsValid) {
                    User oldUser = (User)Session["UserLoggedIn"];
                    int id = oldUser.id;
                    personBll.Edit(id, user);

                    User newUser = (User)personBll.GetPerson(id);
                    if (!newUser.ToString().Equals(oldUser.ToString()))
                        personBll.SaveUserHistory(oldUser, oldUser, "Edited");    //Saves userhistory to database if changed
                    Session["UserLoggedIn"] = newUser;
                    return RedirectToAction("Index", "Item");
                }
            }
            catch (ArgumentNullException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            catch (NotSupportedException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            return View(user);
        }

        public ActionResult AdminEdit(int id = 0) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn");
            User user = (User)Session["UserLoggedIn"];
            if (!user.isAdmin) {
                return RedirectToAction("Error", "Error", new { status = 403 });
            }
            User editUser = (User)personBll.GetPerson(id);
            if (editUser == null) {
                return RedirectToAction("Error", "Error", new { status = 1000 });
            }
            Session["UserChanged"] = editUser;
            return View(editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit(User user) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn");       //Can only view if logged in
            User userLoggedIn = (User)Session["UserLoggedIn"];
            if (!userLoggedIn.isAdmin) {
                return RedirectToAction("Error", "Error", new { status = 403 });
            }
            try {
                ModelState.Remove("userName");              //To pass validation, since these fields are not sent with "user"
                ModelState.Remove("password");
                ModelState.Remove("secondPassword");
                if (ModelState.IsValid) {
                    User oldUser = (User)Session["UserChanged"];
                    int id = oldUser.id;
                    personBll.AdminEdit(id, user);

                    User newUser = (User)personBll.GetPerson(id);
                    if (!newUser.ToString().Equals(oldUser.ToString()))
                        personBll.SaveUserHistory(oldUser, userLoggedIn, "Edited");    //Saves userhistory to database if changed
                    if (userLoggedIn.ToString().Equals(oldUser.ToString()))
                        Session["UserLoggedIn"] = newUser;                              //Update user logged in if it has been edited
                    return RedirectToAction("AdminUsers");
                }
            }
            catch (ArgumentNullException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            catch (NotSupportedException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            return View(user);
        }

        public ActionResult DeactivateUser(int id = 0) {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            User user = (User)personBll.GetPerson(id);
            if (!user.isAdmin) {
                return RedirectToAction("Error", "Error", new { status = 403 });
            }
            if (user == null) {
                return RedirectToAction("Error", "Error", new { status = 1001 });
            }
            Session["ChangingPerson"] = user;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateUser() {
            PersonLogic personBll = new PersonLogic(_stub);
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in

            User oldUser = (User)Session["ChangingPerson"];
            User changedBy = (User)Session["UserLoggedIn"];
            int id = oldUser.id;
            personBll.DeactivateUser(id);
            personBll.SaveUserHistory(oldUser, changedBy, "Deactivated");
            if (oldUser.ToString().Equals(changedBy.ToString())) {
                Session["UserLoggedIn"] = null;
                return RedirectToAction("Index", "Item");
            }

            return RedirectToAction("AdminUsers");
        }
    }
}