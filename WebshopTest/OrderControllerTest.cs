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
    public class OrderControllerTest {

        [TestMethod]
        public void AdminOrders_NotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminOrders();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("LogIn", result.RouteValues.Values.First());
            Assert.AreEqual("User", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void AdminOrders_NotAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User user = new User {
                isAdmin = false
            };

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.AdminOrders();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(403, result.RouteValues.Values.First());
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(2));
        }

        [TestMethod]
        public void AdminOrders_IsAdmin() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            User admin = new User() {
                isAdmin = true
            };

            Item item = new Item() {
                itemId = 4,
                name = "Sjokoladekake",
                description = "Helt konge",
                amount = 13,
                price = 100,
                rabatt = 20,
                subCategory = "Kaker"
            };

            List<OrderItem> expectedOI = new List<OrderItem>();
            OrderItem orderItem = new OrderItem() {
                orderId = 5,
                item = item,
                amount = 3
            };
            expectedOI.Add(orderItem);
            expectedOI.Add(orderItem);
            expectedOI.Add(orderItem);

            List<Order> expectedOrder = new List<Order>();
            Order order = new Order() {
                orderId = 5,
                date = DateTime.MaxValue,
                orderSent = false,
                personId = 5
            };
            expectedOrder.Add(order);
            expectedOrder.Add(order);
            expectedOrder.Add(order);

            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(admin);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.AdminOrders();
            var result = (List<Order>)actionResult.Model;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);

            for (int i = 0; i < result.Count(); i++) {
                Assert.AreEqual(expectedOrder[i].orderId, result[i].orderId);
                Assert.AreEqual(expectedOrder[i].date, result[i].date);
                Assert.AreEqual(expectedOrder[i].orderSent, result[i].orderSent);
                Assert.AreEqual(expectedOrder[i].personId, result[i].personId);

                for (int j = 0; j < result[i].orderItems.Count; j++) {
                    Assert.AreEqual(expectedOI[i].orderId, result[i].orderItems[j].orderId);
                    Assert.AreEqual(expectedOI[i].amount, result[i].orderItems[j].amount);
                    Assert.AreEqual(expectedOI[i].item.itemId, result[i].orderItems[j].item.itemId);
                    Assert.AreEqual(expectedOI[i].item.itemId, result[i].orderItems[j].item.itemId);
                }
            }
        }

        [TestMethod]
        public void ViewCart_WithCart() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session["cart"]).Returns(new Order());
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.ViewCart();

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void ViewCart_CartIsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.ViewCart();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void ViewCart_Post_CartIsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            FormCollection form = new FormCollection();
            form.Add("nrOfItems1", "4");
            form.Add("nrOfItems2", "3");

            // Act
            var actionResult = (ViewResult)controller.ViewCart(form);

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void ViewCart_Post_WithCartAndLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            FormCollection form = new FormCollection();
            form.Add("nrOfItems1", "4");
            form.Add("nrOfItems2", "3");

            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem {
                orderId = 1
            };
            OrderItem orderItem2 = new OrderItem {
                orderId = 1
            };
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem2);

            Order order = new Order();
            order.orderItems = orderItemList;

            List<Order> orderList = new List<Order>();
            User user = new User() {
                orders = orderList
            };

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(user);

            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.ViewCart(form);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Checkout", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void ViewCart_Post_OutOfStock() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            FormCollection form = new FormCollection();
            form.Add("nrOfItems1", "0");
            form.Add("nrOfItems2", "3");

            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem {
                orderId = 1
            };
            OrderItem orderItem2 = new OrderItem {
                orderId = 1
            };
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem2);

            Order order = new Order();
            order.orderItems = orderItemList;

            List<Order> orderList = new List<Order>();
            User user = new User() {
                orders = orderList
            };

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);

            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.ViewCart(form);
            var result = (Order)actionResult.Model;

            // Assert
            Assert.AreEqual(actionResult.ViewName, "");
            Assert.IsNotNull(result.date);
        }

        [TestMethod]
        public void ViewCart_Post_WithCartButNotLoggedIn() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            FormCollection form = new FormCollection();
            form.Add("nrOfItems1", "4");
            form.Add("nrOfItems2", "3");

            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem {
                orderId = 1
            };
            OrderItem orderItem2 = new OrderItem {
                orderId = 1
            };
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem2);

            Order order = new Order();
            order.orderItems = orderItemList;

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.ViewCart(form);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("CheckoutNLI", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void DeleteFromCart_WithCart2() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            Item item = new Item {
                itemId = 5
            };
            Item item2 = new Item {
                itemId = 6
            };
            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem {
                orderId = 1,
                item = item,
            };
            OrderItem orderItem2 = new OrderItem {
                orderId = 1,
                item = item2,
            };

            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem2);

            Order order = new Order();
            order.orderItems = orderItemList;

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeleteFromCart(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("ViewCart", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void DeleteFromCart_WithCart1() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            Item item = new Item {
                itemId = 5
            };
            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem {
                orderId = 1,
                item = item,
            };

            orderItemList.Add(orderItem);

            Order order = new Order();
            order.orderItems = orderItemList;

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.DeleteFromCart(5);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void ToCart_IdIs0_CartIsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = controller.ToCart(0);

            // Arrange
            Assert.AreEqual("Handlevogn", result);
        }

        [TestMethod]
        public void ToCart_IdIs0_CartSize1() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            Order order = new Order{
                orderItems = new List<OrderItem>(){
                    new OrderItem()
                }
            };

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = controller.ToCart(0);

            // Arrange
            Assert.AreEqual("Handlevogn(1 vare)", result);
        }

        [TestMethod]
        public void ToCart_IdIs5_CartSize1() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            Order order = new Order {
                orderItems = new List<OrderItem>(){
                    new OrderItem(){
                        item = new Item(){
                            itemId = 5
                        }
                    }
                }
            };

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = controller.ToCart(5);

            // Arrange
            Assert.AreEqual(result, "Handlevogn(1 vare)");
        }

        [TestMethod]
        public void Checkout_CartIsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Checkout();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }
        
        [TestMethod]
        public void Checkout_WithCart() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session["cart"]).Returns(new Order());
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (ViewResult)controller.Checkout();

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Payment_CartIsNull() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Payment();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Payment_WithCartAndNoUser() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Payment();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Payment_CartAndUser() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            Order order = new Order {
                orderItems = new List<OrderItem>(){
                    new OrderItem(){
                        item = new Item{
                            itemId = 4
                        }
                    }
                }
            };

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            context.Setup(m => m.HttpContext.Session["UserLoggedIn"]).Returns(new User());
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Payment();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("PaymentOK", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void Payment_CartAndPerson() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();

            Order order = new Order {
                orderItems = new List<OrderItem>(){
                    new OrderItem(){
                        item = new Item{
                            itemId = 4
                        }
                    }
                }
            };

            context.Setup(m => m.HttpContext.Session["cart"]).Returns(order);
            context.Setup(m => m.HttpContext.Session["AnonymousUser"]).Returns(new Person());
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var result = (RedirectToRouteResult)controller.Payment();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("PaymentOK", result.RouteValues.Values.First());
        }

        [TestMethod]
        public void PaymentOK() {
            // Arrange
            var context = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(m => m.HttpContext.Session).Returns(session.Object);
            var controller = new OrderController("Test");
            controller.ControllerContext = context.Object;

            // Act
            var actionResult = (ViewResult)controller.paymentOK();

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
        }

        [TestMethod]
        public void Details_Id0() {
            // Arrange
            var controller = new OrderController("Test");

            // Act
            var result = (RedirectToRouteResult)controller.Details(0);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(0));
            Assert.AreEqual("Error", result.RouteValues.Values.ElementAt(1));
        }

        [TestMethod]
        public void Details_Id5() {
            // Arrange
            var controller = new OrderController("Test");

            Item expectedItem = new Item() {
                itemId = 4,
                name = "Sjokoladekake",
                description = "Helt konge",
                amount = 13,
                price = 100,
                rabatt = 20,
                subCategory = "Kaker"
            };

            List<OrderItem> expectedOrderItemList = new List<OrderItem>();
            OrderItem expectedOrderItem = new OrderItem() {
                orderId = 5,
                item = expectedItem,
                amount = 3
            };
            expectedOrderItemList.Add(expectedOrderItem);
            expectedOrderItemList.Add(expectedOrderItem);
            expectedOrderItemList.Add(expectedOrderItem);

            // Act
            var actionResult = (ViewResult)controller.Details(5);
            var result = (Order)actionResult.Model;
            var nrOfItems = result.orderItems.Count;

            // Assert
            Assert.AreEqual("", actionResult.ViewName);
            Assert.AreEqual(5, result.orderId);
            Assert.AreEqual(result.date, DateTime.MaxValue);
            for(int i = 0; i < nrOfItems; i++){
                Assert.AreEqual(expectedOrderItemList[i].amount, result.orderItems[i].amount);
                Assert.AreEqual(expectedOrderItemList[i].orderId, result.orderItems[i].orderId);
                Assert.AreEqual(expectedOrderItemList[i].item.itemId, result.orderItems[i].item.itemId);
                Assert.AreEqual(expectedOrderItemList[i].item.name, result.orderItems[i].item.name);
                Assert.AreEqual(expectedOrderItemList[i].item.description, result.orderItems[i].item.description);
                Assert.AreEqual(expectedOrderItemList[i].item.amount, result.orderItems[i].item.amount);
                Assert.AreEqual(expectedOrderItemList[i].item.price, result.orderItems[i].item.price);
                Assert.AreEqual(expectedOrderItemList[i].item.rabatt, result.orderItems[i].item.rabatt);
                Assert.AreEqual(expectedOrderItemList[i].item.subCategory, result.orderItems[i].item.subCategory);
            }
        }
    }
}
