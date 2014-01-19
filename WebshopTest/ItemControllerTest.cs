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
    public class ItemControllerTest {

        [TestMethod]
        public void Index() {
            // Arrange
            var controller = new ItemController("Test");

            List<Item> expectedResult = new List<Item>();
            Item item = new Item() {
                itemId = 4,
                name = "Sjokoladekake",
                description = "Helt konge",
                amount = 13,
                price = 100,
                rabatt = 20,
                subCategory = "Kaker"
            };

            expectedResult.Add(item);
            expectedResult.Add(item);
            expectedResult.Add(item);

            // Act
            var actionResult = (ViewResult)controller.Index();
            var resultat = (List<Item>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for(int i = 0; i < resultat.Count(); i++){
                Assert.AreEqual(expectedResult[i].itemId, resultat[i].itemId);
                Assert.AreEqual(expectedResult[i].name, resultat[i].name);
                Assert.AreEqual(expectedResult[i].description, resultat[i].description);
                Assert.AreEqual(expectedResult[i].amount, resultat[i].amount);
                Assert.AreEqual(expectedResult[i].price, resultat[i].price);
                Assert.AreEqual(expectedResult[i].rabatt, resultat[i].rabatt);
                Assert.AreEqual(expectedResult[i].subCategory, resultat[i].subCategory);
            }
        }

        [TestMethod]
        public void ProductDetails_Id0() {
            // Arrange
            var controller = new ItemController("Test");

            // Act
            var actionResult = (ViewResult)controller.ProductDetails(0);
            var resultat = (Item)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.IsNull(resultat);
        }

        [TestMethod]
        public void ProductDetails_Id5() {
            // Arrange
            var controller = new ItemController("Test");

            Item expectedResult = new Item() {
                itemId = 5,
                name = "SjokoladeBolle",
                description = "Helt bollekonge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker",
            };

            // Act
            var actionResult = (ViewResult)controller.ProductDetails(5);
            var resultat = (Item)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            Assert.AreEqual(expectedResult.itemId, resultat.itemId);
            Assert.AreEqual(expectedResult.name, resultat.name);
            Assert.AreEqual(expectedResult.description, resultat.description);
            Assert.AreEqual(expectedResult.amount, resultat.amount);
            Assert.AreEqual(expectedResult.price, resultat.price);
            Assert.AreEqual(expectedResult.rabatt, resultat.rabatt);
            Assert.AreEqual(expectedResult.subCategory, resultat.subCategory);
        }

        [TestMethod]
        public void Products() {
            // Arrange
            var controller = new ItemController("Test");

            List<Item> expectedResult = new List<Item>();
            Item item = new Item() {
                itemId = 5,
                name = "SjokoladeBolle",
                description = "Helt bollekonge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker",
            };
            expectedResult.Add(item);
            expectedResult.Add(item);
            expectedResult.Add(item);

            // Act
            var actionResult = (ViewResult)controller.Products(5);
            var resultat = (List<Item>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for (int i = 0; i < resultat.Count(); i++) {
                Assert.AreEqual(expectedResult[i].itemId, resultat[i].itemId);
                Assert.AreEqual(expectedResult[i].name, resultat[i].name);
                Assert.AreEqual(expectedResult[i].description, resultat[i].description);
                Assert.AreEqual(expectedResult[i].amount, resultat[i].amount);
                Assert.AreEqual(expectedResult[i].price, resultat[i].price);
                Assert.AreEqual(expectedResult[i].rabatt, resultat[i].rabatt);
                Assert.AreEqual(expectedResult[i].subCategory, resultat[i].subCategory);
            }
        }

        [TestMethod]
        public void AdminItems_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminItems();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void AdminItems_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminItems();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminItems_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            List<Item> expectedResult = new List<Item>();
            Item item = new Item() {
                itemId = 4,
                name = "Sjokoladekake",
                description = "Helt konge",
                amount = 13,
                price = 100,
                rabatt = 20,
                subCategory = "Kaker"
            };

            Item item2 = new Item() {
                itemId = 5,
                name = "SjokoladeBolle",
                description = "Helt bollekonge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker"
            };

            expectedResult.Add(item);
            expectedResult.Add(item);
            expectedResult.Add(item);
            expectedResult.Add(item2);

            // Act
            var actionResult = (ViewResult)controller.AdminItems();
            var resultat = (List<Item>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for (int i = 0; i < resultat.Count() - 1; i++) {
                Assert.AreEqual(expectedResult[i].itemId, resultat[i].itemId);
                Assert.AreEqual(expectedResult[i].name, resultat[i].name);
                Assert.AreEqual(expectedResult[i].description, resultat[i].description);
                Assert.AreEqual(expectedResult[i].amount, resultat[i].amount);
                Assert.AreEqual(expectedResult[i].price, resultat[i].price);
                Assert.AreEqual(expectedResult[i].rabatt, resultat[i].rabatt);
                Assert.AreEqual(expectedResult[i].subCategory, resultat[i].subCategory);
            }

            Assert.AreEqual(expectedResult[3].itemId, resultat[3].itemId);
            Assert.AreEqual(expectedResult[3].name, resultat[3].name);
            Assert.AreEqual(expectedResult[3].description, resultat[3].description);
            Assert.AreEqual(expectedResult[3].amount, resultat[3].amount);
            Assert.AreEqual(expectedResult[3].price, resultat[3].price);
            Assert.AreEqual(expectedResult[3].rabatt, resultat[3].rabatt);
            Assert.AreEqual(expectedResult[3].subCategory, resultat[3].subCategory);
        }

        [TestMethod]
        public void History_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminItems();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void History_Item_NotAdmin() {

            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            
            User user = new User {
                isAdmin = false
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.History();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void History_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            List<ItemHistory> expectedResult = new List<ItemHistory>();
            ItemHistory itemHist = new ItemHistory() {
                itemId = 5,
                changedByPersonId = 1,
                changeDateTime = DateTime.MaxValue,
                comment = "Edited",
                name = "Sjokokoladekake",
                description = "Helt konge",
                price = 100,
                rabatt = 20,
                amount = 15,
                subCategory = "Kaker"
            };
            expectedResult.Add(itemHist);
            expectedResult.Add(itemHist);
            expectedResult.Add(itemHist);

            // Act
            var actionResult = (ViewResult)controller.History(5);
            var result = (List<ItemHistory>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for (int i = 0; i < result.Count() - 1; i++) {
                Assert.AreEqual(expectedResult[i].itemId, result[i].itemId);
                Assert.AreEqual(expectedResult[i].changedByPersonId, result[i].changedByPersonId);
                Assert.AreEqual(expectedResult[i].changeDateTime, result[i].changeDateTime);
                Assert.AreEqual(expectedResult[i].comment, result[i].comment);
                Assert.AreEqual(expectedResult[i].name, result[i].name);
                Assert.AreEqual(expectedResult[i].description, result[i].description);
                Assert.AreEqual(expectedResult[i].amount, result[i].amount);
                Assert.AreEqual(expectedResult[i].price, result[i].price);
                Assert.AreEqual(expectedResult[i].rabatt, result[i].rabatt);
                Assert.AreEqual(expectedResult[i].subCategory, result[i].subCategory);
            }
        }

        [TestMethod]
        public void History_IsAdminId4() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            List<ItemHistory> expectedResult = new List<ItemHistory>();
            ItemHistory itemHist = new ItemHistory() {
                itemId = 5,
                changedByPersonId = 1,
                changeDateTime = DateTime.MaxValue,
                comment = "Edited",
                name = "Sjokokoladekake",
                description = "Helt konge",
                price = 100,
                rabatt = 20,
                amount = 15,
                subCategory = "Kaker"
            };
            expectedResult.Add(itemHist);
            expectedResult.Add(itemHist);
            expectedResult.Add(itemHist);
            
            // Act
            var actionResult = (ViewResult)controller.History(4);
            var result = (List<ItemHistory>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void DeletedItems_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeletedItems();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void DeletedItems_NotAdmin() {

            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeletedItems();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void DeletedItems_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            List<ItemHistory> expectedResult = new List<ItemHistory>();
            ItemHistory itemHist = new ItemHistory() {
                itemId = 5,
                changedByPersonId = 1,
                changeDateTime = DateTime.MaxValue,
                comment = "Deleted",
                name = "Sjokokoladekake",
                description = "Helt konge",
                price = 100,
                rabatt = 20,
                amount = 15,
                subCategory = "Kaker"
            };
            ItemHistory itemHist2 = new ItemHistory() {
                itemId = 4,
                changedByPersonId = 1,
                changeDateTime = DateTime.MaxValue,
                comment = "Deleted",
                name = "Sjokokoladebolle",
                description = "Helt bollekonge",
                price = 30,
                rabatt = 0,
                amount = 11,
                subCategory = "Kaker"
            };
            expectedResult.Add(itemHist);
            expectedResult.Add(itemHist2);

            // Act
            var actionResult = (ViewResult)controller.DeletedItems();
            var result = (List<ItemHistory>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            Assert.AreEqual(expectedResult[0].itemId, result[0].itemId);
            Assert.AreEqual(expectedResult[0].changedByPersonId, result[0].changedByPersonId);
            Assert.AreEqual(expectedResult[0].changeDateTime, result[0].changeDateTime);
            Assert.AreEqual(expectedResult[0].comment, result[0].comment);
            Assert.AreEqual(expectedResult[0].name, result[0].name);
            Assert.AreEqual(expectedResult[0].description, result[0].description);
            Assert.AreEqual(expectedResult[0].amount, result[0].amount);
            Assert.AreEqual(expectedResult[0].price, result[0].price);
            Assert.AreEqual(expectedResult[0].rabatt, result[0].rabatt);
            Assert.AreEqual(expectedResult[0].subCategory, result[0].subCategory);

            Assert.AreEqual(expectedResult[1].itemId, result[1].itemId);
            Assert.AreEqual(expectedResult[1].changedByPersonId, result[1].changedByPersonId);
            Assert.AreEqual(expectedResult[1].changeDateTime, result[1].changeDateTime);
            Assert.AreEqual(expectedResult[1].comment, result[1].comment);
            Assert.AreEqual(expectedResult[1].name, result[1].name);
            Assert.AreEqual(expectedResult[1].description, result[1].description);
            Assert.AreEqual(expectedResult[1].amount, result[1].amount);
            Assert.AreEqual(expectedResult[1].price, result[1].price);
            Assert.AreEqual(expectedResult[1].rabatt, result[1].rabatt);
            Assert.AreEqual(expectedResult[1].subCategory, result[1].subCategory);
        }

        [TestMethod]
        public void Create_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Create();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Create_NotAdmin() {

            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Create();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void Create_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

           // Act
            var actionResult = (ViewResult)controller.Create();

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void Create_Post_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            Item item = new Item();

            // Act
            var result = (RedirectToRouteResult)controller.Create(item, null);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Create_Post_AllreadyExists() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User() {
                id = 5
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            Item item = new Item() {
                itemId = 5,
                name = "Sjokoladekake"
            };

            // Act
            var actionResult = (ViewResult)controller.Create(item, null);
            var result = (Item)actionResult.Model;

            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void Create_Post_Created() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User() {
                id = 5
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            Item item = new Item() {
                itemId = 5,
                name = "Sjokoladebolle"
            };

            // Act
            var result = (RedirectToRouteResult)controller.Create(item, null);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("AdminItems", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Create_Post_NotValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User() {
                id = 5
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;
            controller.ViewData.ModelState.AddModelError("Id", "Id can not be 0"); 

            Item item = new Item() {
                itemId = 0,
                name = "Sjokoladebolle"
            };

            // Act
            var result = (ViewResult)controller.Create(item, null);

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void Edit_Item_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Edit(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Edit_NotFound() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            User user = new User() {
                id = 5,
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (HttpNotFoundResult)controller.Edit(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.StatusDescription);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Edit_Edited() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            User user = new User() {
                id = 5,
                isAdmin = true
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            Item expectedResult = new Item() {
                itemId = 5,
                name = "SjokoladeBolle",
                description = "Helt bollekonge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker",
            };

            // Act
            var actionResult = (ViewResult)controller.Edit(5);
            var result = (Item)actionResult.Model;

            // Assert
            Assert.AreEqual(expectedResult.itemId, result.itemId);
            Assert.AreEqual(expectedResult.name, result.name);
            Assert.AreEqual(expectedResult.description, result.description);
            Assert.AreEqual(expectedResult.amount, result.amount);
            Assert.AreEqual(expectedResult.price, result.price);
            Assert.AreEqual(expectedResult.rabatt, result.rabatt);
            Assert.AreEqual(expectedResult.subCategory, result.subCategory);
        }

        [TestMethod]
        public void Edit_Post_Item_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Edit(new Item());

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Edit_Post_Item_Edited() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User() {
                id = 5
            };

            Item oldItem = new Item() {
                itemId = 5,
                name = "Sjokolade",
                description = "Helt konge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker",
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            context.Setup(m => m.HttpContext.Session["ChangingItem"]).Returns(oldItem);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Edit(new Item());

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("AdminItems", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Edit_Post_Item_NotValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User() {
                id = 5
            };

            Item oldItem = new Item() {
                itemId = 0,
                name = "Sjokolade",
                description = "Helt konge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker",
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            context.Setup(m => m.HttpContext.Session["ChangingItem"]).Returns(oldItem);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;
            controller.ViewData.ModelState.AddModelError("Id", "ItemId can not be 0"); 

            // Act
            var result = (ViewResult)controller.Edit(new Item());

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void Delete_Item_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Delete(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Delete_Item_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Delete(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void Delete_Item_NotFound() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            User user = new User() {
                id = 5,
                isAdmin = true
            };
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (HttpNotFoundResult)controller.Delete(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.StatusDescription);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Delete_Item_Edited() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            User user = new User() {
                id = 5,
                isAdmin = true
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            Item expectedResult = new Item() {
                itemId = 5,
                name = "SjokoladeBolle",
                description = "Helt bollekonge",
                amount = 19,
                price = 32,
                rabatt = 0,
                subCategory = "Kaker",
            };

            // Act
            var actionResult = (ViewResult)controller.Edit(5);
            var result = (Item)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            Assert.AreEqual(expectedResult.itemId, result.itemId);
            Assert.AreEqual(expectedResult.name, result.name);
            Assert.AreEqual(expectedResult.description, result.description);
            Assert.AreEqual(expectedResult.amount, result.amount);
            Assert.AreEqual(expectedResult.price, result.price);
            Assert.AreEqual(expectedResult.rabatt, result.rabatt);
            Assert.AreEqual(expectedResult.subCategory, result.subCategory);
        }

        [TestMethod]
        public void Delete_Post_Item_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Delete(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Delete_Post_Item_LoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new ItemController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Delete(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }
    }
}