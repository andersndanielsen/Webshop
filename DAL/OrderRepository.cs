using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;

namespace DAL {
    public class OrderRepository : OrderInterface{
        private string errorFile = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";
        private ItemRepository itemDal = new ItemRepository();

        public Order GetOrder(int orderId) {
            using (var db = new UserContext()) {
                try {
                    OrderDb orderDb = db.orders.Find(orderId);
                    List<OrderItemDb> orderItemDbList = GetOrderItems(orderId);
                    List<OrderItem> orderItemList = new List<OrderItem>();

                    foreach (OrderItemDb oid in orderItemDbList) {
                        OrderItem orderItem = new OrderItem() {
                            orderId = oid.orderId,
                            item = itemDal.GetFromHistory(oid.itemId, orderDb.date),
                            amount = oid.amount
                        };
                        orderItemList.Add(orderItem);
                    }

                    if (orderDb != null) {
                        Order order = new Order() {
                            orderId = orderDb.orderId,
                            personId = orderDb.person.id,
                            date = orderDb.date,
                            orderSent = orderDb.orderSent
                        };
                        order.orderItems = orderItemList;
                        return order;
                    }
                }
                catch (InvalidOperationException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return null;
            }
        }

        public List<Order> GetAllOrders() {
            using (var db = new UserContext()) {
                List<Order> orderList = new List<Order>();
                try {
                    List<OrderDb> orderDbList = db.orders.ToList();

                    foreach (OrderDb o in orderDbList) {
                        Order order = new Order() {
                            orderId = o.orderId,
                            personId = o.person.id,
                            date = o.date,
                            orderSent = o.orderSent
                        };

                        List<OrderItemDb> orderItemDbList = GetOrderItems(order.orderId);
                        List<OrderItem> orderItemList = new List<OrderItem>();

                        foreach (OrderItemDb oid in orderItemDbList) {
                            OrderItem orderItem = new OrderItem() {
                                orderId = oid.orderId,
                                item = itemDal.GetItem(oid.itemId),
                                amount = oid.amount
                            };
                            orderItemList.Add(orderItem);
                        }
                        order.orderItems = orderItemList;
                        orderList.Add(order);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return orderList;
            }
        }

        public List<Order> GetUnhandledOrders() {
            using (var db = new UserContext()) {
                List<Order> orderList = new List<Order>();
                try {
                    List<OrderDb> orderDbList = db.orders.Where(o => o.orderSent == false).ToList();

                    foreach (OrderDb o in orderDbList) {
                        Order order = new Order() {
                            orderId = o.orderId,
                            personId = o.person.id,
                            date = o.date,
                            orderSent = o.orderSent
                        };

                        List<OrderItemDb> orderItemDbList = GetOrderItems(order.orderId);
                        List<OrderItem> orderItemList = new List<OrderItem>();

                        foreach (OrderItemDb oid in orderItemDbList) {
                            OrderItem orderItem = new OrderItem() {
                                orderId = oid.orderId,
                                item = itemDal.GetItem(oid.itemId),
                                amount = oid.amount
                            };
                            orderItemList.Add(orderItem);
                        }
                        order.orderItems = orderItemList;
                        orderList.Add(order);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return orderList;
            }
        }

        //Returns every order for customer with person_id=personId 
        public List<Order> GetPersonsOrders(int personId) {
            using (var db = new UserContext()) {
                List<Order> orderList = new List<Order>();
                try {
                    List<OrderDb> orderDbList = db.orders.OrderByDescending(o => o.orderId).Where(i => i.person.id == personId).ToList();

                    foreach (OrderDb o in orderDbList) {
                        Order order = new Order() {
                            orderId = o.orderId,
                            personId = o.person.id,
                            orderSent = o.orderSent,
                            date = o.date
                        };

                        List<OrderItemDb> orderItemDbList = GetOrderItems(order.orderId);
                        List<OrderItem> orderItemList = new List<OrderItem>();

                        foreach (OrderItemDb oid in orderItemDbList) {
                            OrderItem orderItem = new OrderItem() {
                                orderId = oid.orderId,
                                item = itemDal.GetItem(oid.itemId),
                                amount = oid.amount
                            };
                            orderItemList.Add(orderItem);
                        }
                        order.orderItems = orderItemList;
                        orderList.Add(order);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return orderList;
            }
        }

        public int AddOrder(OrderDb order) {
            using(var db = new UserContext()) {
                PostcodeAreaDb postcode = order.person.poscodeArea;
                PersonDb personDb = order.person;

                /*We'll check if customer is PersonDb (not logged in) or UserDb (logged in).
                If user is logged in he's allready in db, and so is postcode, so we set EntityState to Unchanged.
                If user is not logged in we'll check PostCodeAreaDb-table if postcode is allready saved. If so
                we'll set this EntityState to Unchanged.*/
                try {
                    if (personDb is UserDb) {
                        db.Entry(postcode).State = EntityState.Unchanged;
                        db.Entry(personDb).State = EntityState.Unchanged;
                    }
                    else if (db.postCodeAreas.Any(p => p.postcode == postcode.postcode)) {
                        db.Entry(postcode).State = EntityState.Unchanged;
                    }

                    db.orders.Add(order);
                    db.SaveChanges();
                    return db.orders.OrderByDescending(o => o.orderId).FirstOrDefault().orderId;
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
                return 0;
            }
        }

        public void HandleOrder(int id) {
            using (var db = new UserContext()) {
                try {
                    OrderDb orderDb = db.orders.Find(id);
                    orderDb.orderSent = !orderDb.orderSent;
                    db.Entry(orderDb).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (InvalidOperationException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
            }
        }

        public void AddOrderItem(OrderItemDb orderItem) {
            using (var db = new UserContext()) {
                Item item = itemDal.GetItem(orderItem.itemId);
                ItemDb itemDb = new ItemDb() {
                    itemId = item.itemId,
                    name = item.name,
                    description = item.description,
                    subCategory = db.subCategories.First(s => s.name == item.subCategory),
                    amount = item.amount - orderItem.amount,
                    image = item.image,
                    price = item.price,
                    rabatt = item.rabatt
                };

                if (orderItem.amount > itemDb.amount) {

                }

                if (itemDb.amount < 0) {
                    itemDb.amount = 0;
                }

                db.Entry(itemDb).State = EntityState.Modified;

                db.orderItems.Add(orderItem);
                try {
                    db.SaveChanges();
                }
                catch (InvalidOperationException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
            }
        }

        public List<OrderItemDb> GetOrderItems(int orderId) {
            using (var db = new UserContext()) {
                try {
                    return db.orderItems.Where(o => o.orderId == orderId).ToList();
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return new List<OrderItemDb>();
            }
        }
    }
}
