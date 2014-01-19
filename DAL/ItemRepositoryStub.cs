using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL {
    public class ItemRepositoryStub : ItemInterface{
        public List<Item> GetItemsOnSale() {
            
            List<Item> itemList = new List<Item>();
            Item item = new Item() {
                itemId = 4,
                name = "Sjokoladekake",
                description = "Helt konge",
                amount = 13,
                price = 100,
                rabatt = 20,
                subCategory = "Kaker"
            };

            itemList.Add(item);
            itemList.Add(item);
            itemList.Add(item);

            return itemList;
        }

        public List<Item> GetAllItems() {
            List<Item> itemList = new List<Item>();
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

            itemList.Add(item);
            itemList.Add(item);
            itemList.Add(item);
            itemList.Add(item2);

            return itemList;
        }

        public Item GetItem(int id) {

            Item item = null;
            if (id == 5) {
                item = new Item() {
                    itemId = 5,
                    name = "SjokoladeBolle",
                    description = "Helt bollekonge",
                    amount = 19,
                    price = 32,
                    rabatt = 0,
                    subCategory = "Kaker",
                };
            }
            return item;
        }

        public List<Item> GetItemsInSubCategory(int id) {

            List<Item> itemList = new List<Item>();
            if (id == 5) {
                Item item = new Item() {
                    itemId = 5,
                    name = "SjokoladeBolle",
                    description = "Helt bollekonge",
                    amount = 19,
                    price = 32,
                    rabatt = 0,
                    subCategory = "Kaker",
                };
                itemList.Add(item);
                itemList.Add(item);
                itemList.Add(item);
            }

            return itemList;
        }
        public List<SubCategory> GetAllSubcategories() {

            List<SubCategory> subCatList = new List<SubCategory>();
            SubCategory subCat = new SubCategory() {
                id = 2,
                name = "Kaker"
            };
            subCatList.Add(subCat);
            subCatList.Add(subCat);
            subCatList.Add(subCat);
            
            return subCatList;
        }

        public List<ItemHistory> GetHistory(int itemId) {

            List<ItemHistory> itemHistoryList = new List<ItemHistory>();
            ItemHistory itemHist = null;
            
            if (itemId == 5) {
                itemHist = new ItemHistory() {
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
                itemHistoryList.Add(itemHist);
                itemHistoryList.Add(itemHist);
                itemHistoryList.Add(itemHist);
            }
            return itemHistoryList;
        }

        public List<ItemHistory> GetDeletedItems() {

            List<ItemHistory> itemHistoryList = new List<ItemHistory>();
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
            itemHistoryList.Add(itemHist);
            itemHistoryList.Add(itemHist2);

            return itemHistoryList;
        }

        public int Create(Item item) {

            if (item.name.Equals("Sjokoladekake")) {
                return 0;
            }

            return 24;
        }

        public void Edit(int id, Item newItem) {
        }

        public void Delete(int id) {
        }

        public void SaveItemHistory(Item oldItem, int changedByPersonIdIn, string changeComment) {
        }
    }
}
