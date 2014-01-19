using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace DAL {
    
    public interface OrderInterface {
        Order GetOrder(int orderId);
        List<Order> GetAllOrders();
        List<Order> GetUnhandledOrders();
        List<Order> GetPersonsOrders(int personId);
        int AddOrder(OrderDb order);
        void HandleOrder(int id);
        void AddOrderItem(OrderItemDb orderItem);
        List<OrderItemDb> GetOrderItems(int orderId);
    }
}
