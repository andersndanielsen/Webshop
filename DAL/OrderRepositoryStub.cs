using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL {
    public class OrderRepositoryStub : OrderInterface{

        public Order GetOrder(int orderId) {
            if (orderId != 5) {
                return null;
            }
            DateTime dateTime = DateTime.MaxValue;
            Order order = new Order() {
                orderId = 5,
                date = dateTime
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
            
            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem() {
                orderId = 5,
                item = item,
                amount = 3
            };
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem);

            order.orderItems = orderItemList;
            return order;
        }

        public List<Order> GetAllOrders() {
            List<Order> orderList = new List<Order>();
            DateTime dateTime = DateTime.MaxValue;
            Order order = new Order() {
                orderId = 5,
                date = dateTime,
                orderSent = false,
                personId = 5
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

            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem() {
                orderId = 5,
                item = item,
                amount = 3
            };
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem);

            order.orderItems = orderItemList;

            orderList.Add(order);
            orderList.Add(order);
            orderList.Add(order);
            return orderList;
        }

        public List<Order> GetUnhandledOrders() {
            return null;
        }

        public List<Order> GetPersonsOrders(int personId) {
            if (personId != 5) {
                return null;
            }
            
            List<Order> orderList = new List<Order>();
            Order order = new Order() {
                orderId = 3,
                date = DateTime.MaxValue,
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

            List<OrderItem> orderItemList = new List<OrderItem>();
            OrderItem orderItem = new OrderItem() {
                orderId = 3,
                item = item,
                amount = 43
            };
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem);
            orderItemList.Add(orderItem);

            order.orderItems = orderItemList;
            orderList.Add(order);
            orderList.Add(order);
            orderList.Add(order);

            return orderList;
        }

        public int AddOrder(OrderDb order) {
            return 2;
        }

        public void HandleOrder(int id) { }

        public void AddOrderItem(OrderItemDb orderItem) {
        }

        public List<OrderItemDb> GetOrderItems(int orderId) {
            if (orderId != 5) {
                return null;
            }

            List<OrderItemDb> createdList = new List<OrderItemDb>();
            OrderItemDb createdOrderItem = new OrderItemDb { 
                orderId = 5,
                itemId = 4,
                amount = 51
            };
            createdList.Add(createdOrderItem);
            createdList.Add(createdOrderItem);
            createdList.Add(createdOrderItem);

            return createdList;
        }
    }
}
