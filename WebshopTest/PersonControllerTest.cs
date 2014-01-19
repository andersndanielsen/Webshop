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

namespace WebshopTest {
    [TestClass]
    public class PersonControllerTest {

        [TestMethod]
        public void ControlPanel_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.ControlPanel();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void ControlPanel_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.ControlPanel();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void ControlPanel_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (ViewResult)controller.ControlPanel();

            // Assert
            Assert.AreEqual("", result.ViewName);
        }


        [TestMethod]
        public void AdminUsers_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminUsers();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void AdminUsers_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminPersons();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminUsers_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User admin = new User() {
                isAdmin = true
            };

            List<User> expectedResult = new List<User>();
            User user = new User() {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };
            expectedResult.Add(user);
            expectedResult.Add(user);
            expectedResult.Add(user);

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(admin);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.AdminUsers();
            var result = (List<User>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for(int i = 0; i < result.Count(); i++){
                Assert.AreEqual(expectedResult[i].id, result[i].id);
                Assert.AreEqual(expectedResult[i].firstName, result[i].firstName);
                Assert.AreEqual(expectedResult[i].surName, result[i].surName);
                Assert.AreEqual(expectedResult[i].telephoneNumber, result[i].telephoneNumber);
                Assert.AreEqual(expectedResult[i].address, result[i].address);
                Assert.AreEqual(expectedResult[i].postcode, result[i].postcode);
                Assert.AreEqual(expectedResult[i].postcodeArea, result[i].postcodeArea);
                Assert.AreEqual(expectedResult[i].isAdmin, result[i].isAdmin);
                Assert.AreEqual(expectedResult[i].userName, result[i].userName);
                Assert.AreEqual(expectedResult[i].password, result[i].password);
            }            
        }

        [TestMethod]
        public void AdminPersons_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminPersons();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void AdminPersons_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminPersons();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminPersons_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User admin = new User() {
                isAdmin = true
            };

            List<Person> expectedResult = new List<Person>();
            Person person = new Person() {
                id = 2,
                firstName = "Geir",
                surName = "Børresen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo"
            };
            expectedResult.Add(person);
            expectedResult.Add(person);
            expectedResult.Add(person);

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(admin);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.AdminPersons();
            var result = (List<Person>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for (int i = 0; i < result.Count(); i++) {
                Assert.AreEqual(expectedResult[i].id, result[i].id);
                Assert.AreEqual(expectedResult[i].firstName, result[i].firstName);
                Assert.AreEqual(expectedResult[i].surName, result[i].surName);
                Assert.AreEqual(expectedResult[i].telephoneNumber, result[i].telephoneNumber);
                Assert.AreEqual(expectedResult[i].address, result[i].address);
                Assert.AreEqual(expectedResult[i].postcode, result[i].postcode);
                Assert.AreEqual(expectedResult[i].postcodeArea, result[i].postcodeArea);
            }
        }

        [TestMethod]
        public void History_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.History();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void History_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new UserController("Test");
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

            User admin = new User() {
                isAdmin = true
            };

            List<UserHistory> expectedResult = new List<UserHistory>();
            UserHistory userHist = new UserHistory() {
                id = 6,
                changedByPersonId = 2,
                changeDateTime = DateTime.MaxValue,
                comment = "Deleted",
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false
            };
            expectedResult.Add(userHist);
            expectedResult.Add(userHist);
            expectedResult.Add(userHist);

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(admin);
            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.History();
            var result = (List<UserHistory>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for (int i = 0; i < result.Count(); i++) {
                Assert.AreEqual(expectedResult[i].id, result[i].id);
                Assert.AreEqual(expectedResult[i].changeDateTime, result[i].changeDateTime);
                Assert.AreEqual(expectedResult[i].changedByPersonId, result[i].changedByPersonId);
                Assert.AreEqual(expectedResult[i].comment, result[i].comment);
                Assert.AreEqual(expectedResult[i].firstName, result[i].firstName);
                Assert.AreEqual(expectedResult[i].surName, result[i].surName);
                Assert.AreEqual(expectedResult[i].telephoneNumber, result[i].telephoneNumber);
                Assert.AreEqual(expectedResult[i].address, result[i].address);
                Assert.AreEqual(expectedResult[i].postcode, result[i].postcode);
                Assert.AreEqual(expectedResult[i].postcodeArea, result[i].postcodeArea);
            }
        }

        [TestMethod]
        public void LogIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            var request = new Mock<HttpRequestBase>();
            context.SetupGet(x => x.HttpContext.Request.UrlReferrer).Returns(new Uri("http://www.vg.no/"));
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.LogIn("");

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("http://www.vg.no/", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void LogIn_LoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            var request = new Mock<HttpRequestBase>();
            //context.Setup(x => x.HttpContext.Request).Returns(request.Object);
            //context.SetupGet(x => x.HttpContext.Request.Url).Returns(new Uri("http://www.vg.no"));
            context.SetupGet(x => x.HttpContext.Request.UrlReferrer).Returns(new Uri("http://www.vg.no/"));
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectResult)controller.LogIn("http://www.vg.no/");

            // Assert
            Assert.AreEqual("http://www.vg.no/", result.Url);
        }

        [TestMethod]
        public void LogIn_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            var request = new Mock<HttpRequestBase>();
            context.SetupGet(x => x.HttpContext.Request.UrlReferrer).Returns(new Uri("http://www.vg.no/"));
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

             // Act
            var result = (ViewResult)controller.LogIn("http://www.vg.no/");

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void LogIn_Post_UserFoundWithURL() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            var request = new Mock<HttpRequestBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            context.SetupGet(x => x.HttpContext.Request.UrlReferrer).Returns(new Uri("http://www.vg.no/"));

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User user = new User() {
                id = 5,
                userName = "Arne@arne.no",
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false
            };

            // Act
            var result = (RedirectResult)controller.LogIn(user, "http://www.vg.no/");

            // Assert
            Assert.AreEqual("http://www.vg.no/", result.Url);
        }

        [TestMethod]
        public void LogIn_Post_UserFoundWithoutURL() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            var request = new Mock<HttpRequestBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            context.SetupGet(x => x.HttpContext.Request.UrlReferrer).Returns(new Uri("http://www.vg.no/"));

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User user = new User() {
                id = 5,
                userName = "Arne@arne.no",
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false
            };

            // Act
            var result = (RedirectToRouteResult)controller.LogIn(user, "");

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void LogIn_Post_UserIsNull() {
            // Arrange
            var controller = new UserController("Test");

            User user = new User() {
                id = 5,
                userName = "",
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false
            };

            // Act
            var result = (ViewResult)controller.LogIn(user, "");

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void CheckoutNli() {
            // Arrange
            var controller = new UserController("Test");

            // Act
            var actionResult = (ViewResult)controller.CheckoutNli();

            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
        }

        [TestMethod]
        public void CheckoutNli_Post_Valid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User user = new User() {
                id = 5,
                userName = "Arne@arne.no",
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false
            };

            // Act
            var result = (RedirectToRouteResult)controller.CheckoutNli(user);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("Checkout", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void CheckoutNli_Post_NotValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;
            controller.ViewData.ModelState.AddModelError("Firstname", "No name given"); 

            User user = new User() {
                id = 5,
                userName = "Arne@arne.no",
                firstName = "",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false
            };

            // Act
            var result = (ViewResult)controller.CheckoutNli(user);

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
        }

        [TestMethod]
        public void MyPage() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.MyPage(1);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void MyPage_UserIsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.MyPage();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(0));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void MyPage_User() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            // Act
            var actionResult = (ViewResult)controller.MyPage(1);
            var result = (User) actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(expected.firstName, result.firstName);
            Assert.AreEqual(expected.surName, result.surName);
            Assert.AreEqual(expected.telephoneNumber, result.telephoneNumber);
            Assert.AreEqual(expected.address, result.address);
            Assert.AreEqual(expected.postcode, result.postcode);
            Assert.AreEqual(expected.postcodeArea, result.postcodeArea);
            Assert.AreEqual(expected.isAdmin, result.isAdmin);
            Assert.AreEqual(expected.userName, result.userName);
            Assert.AreEqual(expected.password, result.password);
        }

        [TestMethod]
        public void MyPage_Person() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            Person expected = new Person {
                id = 2,
                firstName = "Geir",
                surName = "Børresen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo"
            };

            // Act
            var actionResult = (ViewResult)controller.MyPage(2);
            var result = (Person)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(expected.firstName, result.firstName);
            Assert.AreEqual(expected.surName, result.surName);
            Assert.AreEqual(expected.telephoneNumber, result.telephoneNumber);
            Assert.AreEqual(expected.address, result.address);
            Assert.AreEqual(expected.postcode, result.postcode);
            Assert.AreEqual(expected.postcodeArea, result.postcodeArea);
        }

        [TestMethod]
        public void Create_UserLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Create();

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("Index", result.RouteValues.Values.First());
            Assert.AreEqual("Item", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Create_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (ViewResult)controller.Create();

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_UserLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Create();

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("Index", result.RouteValues.Values.First());
            Assert.AreEqual("Item", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Create_Post_UserNameTaken() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User expected = new User {
                id = 1,
                firstName = "Svein",
                surName = "Svinesti",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "svein@svein.no",
                password = "1234"
            };

            // Act
            var result = (ViewResult)controller.Create(expected);

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_Created() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User expected = new User {
                id = 1,
                firstName = "Svein",
                surName = "Svinesti",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arne@arne.no",
                password = "1234"
            };

            // Act
            var result = (RedirectToRouteResult)controller.Create(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Create_Post_NotValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            controller.ViewData.ModelState.AddModelError("Firstname", "Firstname not given"); 

            User expected = new User {
                id = 1,
                firstName = "",
                surName = "Svinesti",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "svein@svein.no",
                password = "1234"
            };

            // Act
            var result = (ViewResult)controller.Create(expected);

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
        }

        [TestMethod]
        public void Edit_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User expected = new User {
                id = 1,
                firstName = "Svein",
                surName = "Svinesti",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arne@arne.no",
                password = "1234"
            };

            // Act
            var result = (RedirectToRouteResult)controller.Edit(0);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Edit_Id1() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(expected);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.Edit(1);
            var result = (User)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(expected.firstName, result.firstName);
            Assert.AreEqual(expected.surName, result.surName);
            Assert.AreEqual(expected.telephoneNumber, result.telephoneNumber);
            Assert.AreEqual(expected.address, result.address);
            Assert.AreEqual(expected.postcode, result.postcode);
            Assert.AreEqual(expected.postcodeArea, result.postcodeArea);
            Assert.AreEqual(expected.isAdmin, result.isAdmin);
            Assert.AreEqual(expected.userName, result.userName);
            Assert.AreEqual(expected.password, result.password);
        }

        [TestMethod]
        public void Edit_Id0() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = true,
                userName = "arnesen",
                password = "1234"
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(expected);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Edit(0);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(0));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Edit_Post_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            // Act
            var result = (RedirectToRouteResult)controller.Edit(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Edit_Post_Valid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User expected = new User {
                id = 1,
                firstName = "Bjarne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(expected);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Edit(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("Index", result.RouteValues.Values.First());
            Assert.AreEqual("Item", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Edit_Post_NotValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            var controller = new UserController("Test");
            controller.ViewData.ModelState.AddModelError("Firstname", "Firstname not given");

            User expected = new User {
                id = 1,
                firstName = "",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            controller.ControllerContext = context.Object;

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(expected); 

            // Act
            var result = (ViewResult)controller.Edit(expected);

            // Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState.Count == 1);
        }

        [TestMethod]
        public void AdminEdit_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(0);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void AdminEdit_IsNotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(0);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminEdit_IsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(0);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual(1000, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminEdit_Edited() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            var controller = new UserController("Test");

            User user = new User {
                isAdmin = true
            };
            
            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            controller.ControllerContext = context.Object;

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            // Act
            var actionResult = (ViewResult)controller.AdminEdit(1);
            var result = (User)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(expected.id, result.id);
            Assert.AreEqual(expected.firstName, result.firstName);
            Assert.AreEqual(expected.surName, result.surName);
            Assert.AreEqual(expected.telephoneNumber, result.telephoneNumber);
            Assert.AreEqual(expected.address, result.address);
            Assert.AreEqual(expected.postcode, result.postcode);
            Assert.AreEqual(expected.postcodeArea, result.postcodeArea);
            Assert.AreEqual(expected.isAdmin, result.isAdmin);
            Assert.AreEqual(expected.userName, result.userName);
            Assert.AreEqual(expected.password, result.password);
        }

        [TestMethod]
        public void AdminEdit_Post_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void AdminEdit_Post_IsNotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminEdit_IsValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            context.Setup(m => m.HttpContext.Session["UserChanged"]).Returns(expected);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("AdminUsers", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void AdminEdit_NotValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            User expected = new User {
                id = 1,
                firstName = "",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            context.Setup(m => m.HttpContext.Session["UserChanged"]).Returns(expected);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            controller.ViewData.ModelState.AddModelError("Firstname", "Must write firstname"); 

            // Act
            var actionResult = (ViewResult)controller.AdminEdit(expected);
            var result = (User)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(expected.id, result.id);
            Assert.AreEqual(expected.firstName, result.firstName);
            Assert.AreEqual(expected.surName, result.surName);
            Assert.AreEqual(expected.telephoneNumber, result.telephoneNumber);
            Assert.AreEqual(expected.address, result.address);
            Assert.AreEqual(expected.postcode, result.postcode);
            Assert.AreEqual(expected.postcodeArea, result.postcodeArea);
            Assert.AreEqual(expected.isAdmin, result.isAdmin);
            Assert.AreEqual(expected.userName, result.userName);
            Assert.AreEqual(expected.password, result.password);
        }

        [TestMethod]
        public void DeactivateUser_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeactivateUser(0);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void DeactivateUser_IsNotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeactivateUser(1);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void DeactivateUser_IsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeactivateUser(1);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void DeactivateUser_IsValid() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = true
            };
            User expected = new User {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            context.Setup(m => m.HttpContext.Session["UserChanged"]).Returns(expected);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminEdit(expected);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("AdminUsers", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void DeactivateUser_Post_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            context.Setup(m => m.HttpContext.Session).Returns(session.Object);

            var controller = new UserController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeactivateUser(0);

            // Assert
            Assert.AreEqual("", result.RouteName);
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }
    }
}
