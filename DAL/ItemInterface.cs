using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace DAL {

    public interface ItemInterface {
        List<Item> GetItemsOnSale();
        List<Item> GetAllItems();
        Item GetItem(int id);
        List<Item> GetItemsInSubCategory(int id);
        List<SubCategory> GetAllSubcategories();
        List<ItemHistory> GetHistory(int itemId);
        List<ItemHistory> GetDeletedItems();
        int Create(Item item);
        void Edit(int id, Item newItem);
        void Delete(int id);
        void SaveItemHistory(Item oldItem, int changedByPersonIdIn, string changeComment);
    }
}
