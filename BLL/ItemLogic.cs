using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace BLL {
    
    public class ItemLogic {
        private ItemInterface itemDal;

        public ItemLogic(string stub)
        {
            if (stub=="Test")
            {
                itemDal = new ItemRepositoryStub();
            }
            else
            {
                itemDal = new ItemRepository();
            }
        }

        public List<Item> GetItemsOnSale() {
            return itemDal.GetItemsOnSale();
        }

        public List<Item> GetAllItems() {
            return itemDal.GetAllItems();
        }

        public Item GetItem(int id) {
            return itemDal.GetItem(id);            
        }

        public List<Item> GetItemsInSubCategory(int id) {
            return itemDal.GetItemsInSubCategory(id);
        }

        public List<SubCategory> GetAllSubCategories() {
            return itemDal.GetAllSubcategories();
        }

        public List<ItemHistory> GetHistory(int itemId) {
            return itemDal.GetHistory(itemId);
        }

        public List<ItemHistory> GetDeletedItems() {
            return itemDal.GetDeletedItems();
        }

        public int Create(Item newItem) {
            return itemDal.Create(newItem);
        }

        public void Edit(int id, Item newItem) {
            itemDal.Edit(id, newItem);
        }

        public void Delete(int id) {
            itemDal.Delete(id);
        }

        public void SaveItemHistory(Item oldItem, int changedByPersonId, string changeComment) {
            itemDal.SaveItemHistory(oldItem, changedByPersonId, changeComment);
        }
    }
}
