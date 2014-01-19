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

namespace Webshop.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(int status) {
            Response.StatusCode = status;
            return View(status);
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
        }
    }
}
