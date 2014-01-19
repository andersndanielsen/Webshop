using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using BLL;
using Model;
using System.IO;

namespace Webshop.Controllers {
    public class ItemController : Controller {
        private string _stub;

         public ItemController(){
            _stub = "";
        }

        public ItemController(string rep){
            _stub = rep;
        }
        
        //
        // GET: /Item/

        public ActionResult Index() {
            ItemLogic itemBll = new ItemLogic(_stub);
            return View(itemBll.GetItemsOnSale());
        }

        //
        // GET: /Item/ProductDetails/5

        public ActionResult ProductDetails(int id) {
            ItemLogic itemBll = new ItemLogic(_stub);
            return View(itemBll.GetItem(id));
        }

        //
        // GET: /Item/Products/5
        public ActionResult Products(int id) {
            ItemLogic itemBll = new ItemLogic(_stub);
            return View(itemBll.GetItemsInSubCategory(id));
        }

        public ActionResult AdminItems() {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            Model.User user = (Model.User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 } );
            return View(itemBll.GetAllItems());
        }

        public ActionResult History(int id = 0) {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            Model.User user = (Model.User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(itemBll.GetHistory(id));
        }

        public ActionResult DeletedItems() {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            Model.User user = (Model.User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View(itemBll.GetDeletedItems());
        }

        public ActionResult Create() {
            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            Model.User user = (Model.User)Session["UserLoggedIn"];
            if (!user.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item, HttpPostedFileBase file) {

            if (file != null) {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/Images"), pic);
                // file is uploaded
                file.SaveAs(path);

                byte[] imageAsByte;
                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream()) {
                    file.InputStream.CopyTo(ms);
                    imageAsByte = ms.GetBuffer();
                }
                item.image = imageAsByte;
            }

            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
           
            Model.User changedBy = (Model.User)Session["UserLoggedIn"];
            if (ModelState.IsValid) {
                int itemId = itemBll.Create(item);
                if (itemId==0) {
                    ViewBag.itemNameError = "Varenavnet eksisterer fra før av";
                    return View(item);
                }
                Item newItem = itemBll.GetItem(itemId);
                itemBll.SaveItemHistory(newItem, changedBy.id, "Created");
                return RedirectToAction("AdminItems");
            }
            return View(item);
        }

        public ActionResult Edit(int id = 0) {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            Model.User changedBy = (Model.User)Session["UserLoggedIn"];
            if (!changedBy.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });      
            Item item = (Item)itemBll.GetItem(id);
            if (item == null) {
                return HttpNotFound();
            }
            Session["ChangingItem"] = item;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item) {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in

            Model.User changedBy = (Model.User)Session["UserLoggedIn"];         
            if (ModelState.IsValid) {
                Item oldItem = (Item)Session["ChangingItem"];
                int id = oldItem.itemId;
                itemBll.Edit(id, item);
                Item newItem = itemBll.GetItem(id);
                if (newItem.ToString() != oldItem.ToString())
                    itemBll.SaveItemHistory(oldItem, changedBy.id, "Edited");
                return RedirectToAction("AdminItems");
            }
            return View(item);
        }

        public ActionResult Delete(int id = 0) {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in
            Model.User changedBy = (Model.User)Session["UserLoggedIn"];
            if (!changedBy.isAdmin)
                return RedirectToAction("Error", "Error", new { status = 403 });
            Item item = (Item)itemBll.GetItem(id);
            if (item == null) {
                return HttpNotFound();
            }
            Session["ChangingItem"] = item;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete() {
            ItemLogic itemBll = new ItemLogic(_stub);

            if (Session["UserLoggedIn"] == null)
                return RedirectToAction("LogIn", "User");       //Can only view if logged in

            Model.User changedBy = (Model.User)Session["UserLoggedIn"];
            Item oldItem = (Item)Session["ChangingItem"];
            int id = oldItem.itemId;
            itemBll.Delete(id);
            itemBll.SaveItemHistory(oldItem, changedBy.id, "Deleted");
            return RedirectToAction("AdminItems");
        }
    }
}