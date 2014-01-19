using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using BLL;
using Model;
using Webshop.Controllers;
using System.IO;

namespace WebshopTest {
    
    [TestClass]
    public class ErrorControllerTest {

        [TestMethod]
        public void Error() {
            // Arrange
            var mockHttpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            mockHttpContext.SetupGet(x => x.Response).Returns(response.Object);

            //creates an instance of an asp.net mvc controller
            var controller = new ErrorController() {
                ControllerContext = new ControllerContext() {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var actionResult = (ViewResult)controller.Error(403);
            var result = (int)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(403, result);
        }
    }
}