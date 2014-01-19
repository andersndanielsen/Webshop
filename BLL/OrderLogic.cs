using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BLL {
    public class OrderLogic {

        private OrderInterface orderDal;

        public OrderLogic(string stub)
        {
            if (stub=="Test")
            {
                orderDal = new OrderRepositoryStub();
            }
            else
            {
                orderDal = new OrderRepository();
            }
        }

        // Returns the order with orderId=orderId
        public Order GetOrder(int orderId) {
            return orderDal.GetOrder(orderId);
        }

        public List<Order> GetAllOrders() {
            return orderDal.GetAllOrders();
        }

        public List<Order> GetUnhandledOrders() {
            return orderDal.GetUnhandledOrders();
        }

        // Returns all the orders that a user with userId has bought
        public List<Order> GetPersonsOrders(int personId) {
            return orderDal.GetPersonsOrders(personId);
        }

        public bool addOrder(Order order, Person person) {
            UserDb userDb;
            PersonDb personDb;

            OrderDb orderDb = new OrderDb() {
                date = order.date
            };

            if (person is User) {
                User user = person as User;
                PostcodeAreaDb postCodeDb = new PostcodeAreaDb() {
                    postcode = user.postcode,
                    postcodeArea = user.postcodeArea
                };
                    userDb = new UserDb {
                    id = user.id,
                    firstName = user.firstName,
                    surName = user.surName,
                    address = user.address,
                    poscodeArea = postCodeDb,
                    telephoneNumber = user.telephoneNumber,
                    userName = user.userName,
                    password = user.password
                };
                    orderDb.person = userDb;
            }
            else {
                PostcodeAreaDb postCodeDb = new PostcodeAreaDb(){
                        postcode = person.postcode,
                        postcodeArea = person.postcodeArea
                    };
                    personDb = new PersonDb {
                    id = person.id,
                    firstName = person.firstName,
                    surName = person.surName,
                    address = person.address,
                    poscodeArea = postCodeDb,
                    telephoneNumber = person.telephoneNumber
                };
                orderDb.person = personDb;
            }
            
            int orderId = orderDal.AddOrder(orderDb);
            if (orderId == 0)
                return false;
            foreach (OrderItem oi in order.orderItems) {
                OrderItemDb orderItemDb = new OrderItemDb() {
                    orderId = orderId,
                    itemId = oi.item.itemId,
                    amount = oi.amount
                };

                orderDal.AddOrderItem(orderItemDb);
            }
            return true;
        }

        public void HandleOrder(int id) {
            orderDal.HandleOrder(id);
        }
    }
}
