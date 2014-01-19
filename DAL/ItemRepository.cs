using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.Entity;

namespace DAL {
    public class ItemRepository : ItemInterface{
        private string errorFile = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";

        public List<Item> GetItemsOnSale() {
            using (var db = new UserContext()) {
                List<Item> itemList = new List<Item>();
                try {
                    List<ItemDb> itemDbList = db.items.Where(i => i.rabatt > 0).ToList();

                    for (int i = 0; i < itemDbList.Count(); i++) {
                        Item item = new Item() {
                            itemId = itemDbList[i].itemId,
                            image = itemDbList[i].image,
                            name = itemDbList[i].name,
                            description = itemDbList[i].description,
                            amount = itemDbList[i].amount,
                            price = itemDbList[i].price,
                            rabatt = itemDbList[i].rabatt,
                            subCategory = itemDbList[i].subCategory.name
                        };
                        itemList.Add(item);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return itemList;
            }
        }

        public List<Item> GetAllItems() {
            using (var db = new UserContext()) {
                List<Item> itemList = new List<Item>();
                try {
                    List<ItemDb> itemDbList = db.items.ToList();
                    for (int i = 0; i < itemDbList.Count(); i++) {
                        Item item = new Item() {
                            itemId = itemDbList[i].itemId,
                            name = itemDbList[i].name,
                            description = itemDbList[i].description,
                            amount = itemDbList[i].amount,
                            price = itemDbList[i].price,
                            rabatt = itemDbList[i].rabatt,
                            subCategory = itemDbList[i].subCategory.name
                        };
                        itemList.Add(item);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return itemList;
            }
        }

        public Item GetItem(int id) {
            using (var db = new UserContext()) {
                Item item = new Item();
                try {
                    ItemDb itemDb = db.items.Find(id);

                    item = new Item {
                        itemId = itemDb.itemId,
                        image = itemDb.image,
                        name = itemDb.name,
                        description = itemDb.description,
                        amount = itemDb.amount,
                        price = itemDb.price,
                        rabatt = itemDb.rabatt,
                        subCategory = itemDb.subCategory.name
                    };
                }
                catch (InvalidOperationException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return item;
            }
        }

        public Item GetFromHistory(int id, DateTime dateTime) {
            using (var db = new UserContext()) {
                Item item = new Item();
                try {
                    List<ItemHistoryDb> itemHistoryDbList = db.itemsHistory.Where(i => i.itemId == id).OrderByDescending(i => i.changeDateTime).ToList();
                    if (itemHistoryDbList[0].changeDateTime.CompareTo(dateTime) < 0)        //If item has not changed since order
                        return GetItem(id);

                    ItemHistoryDb correctItemHistoryDb = new ItemHistoryDb();
                    foreach (ItemHistoryDb itemHistoryDb in itemHistoryDbList) {
                        if (itemHistoryDb.changeDateTime.CompareTo(dateTime) > 0)           //If edited after order, use first version of the item saved to history after order
                            correctItemHistoryDb = itemHistoryDb;
                    }

                    item = new Item {
                        itemId = correctItemHistoryDb.itemId,
                        name = correctItemHistoryDb.name,
                        description = correctItemHistoryDb.description,
                        price = correctItemHistoryDb.price,
                        rabatt = correctItemHistoryDb.rabatt
                    };
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return item;
            }
        }

        public List<Item> GetItemsInSubCategory(int id) {
            using (var db = new UserContext()) {
                List<Item> itemList = new List<Item>();
                try {
                    List<ItemDb> itemDbList = (from i in db.items
                                               where i.subCategory.id == id
                                               select i).ToList();
                    for (int i = 0; i < itemDbList.Count(); i++) {
                        Item item = new Item() {
                            itemId = itemDbList[i].itemId,
                            image = itemDbList[i].image,
                            name = itemDbList[i].name,
                            description = itemDbList[i].description,
                            amount = itemDbList[i].amount,
                            price = itemDbList[i].price,
                            rabatt = itemDbList[i].rabatt,
                            subCategory = itemDbList[i].subCategory.name
                        };
                        itemList.Add(item);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return itemList;
            }
        }

        public List<SubCategory> GetAllSubcategories() {
            using (var db = new UserContext()) {
                List<SubCategory> subCatList = new List<SubCategory>();
                try {
                    List<SubCategoryDb> subCatDbList = db.subCategories.ToList();
                    for (int i = 0; i < subCatDbList.Count(); i++) {
                        SubCategory subCat = new SubCategory() {
                            id = subCatDbList[i].id,
                            name = subCatDbList[i].name
                        };
                        subCatList.Add(subCat);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return subCatList;
            }
        }

        public List<ItemHistory> GetHistory(int itemId) {
            using (var db = new UserContext()) {
                List<ItemHistory> itemHistoryList = new List<ItemHistory>();
                try {
                    List<ItemHistoryDb> itemHistoryDbList = db.itemsHistory.OrderByDescending(o => o.changeDateTime).
                        Where(h => h.itemId == itemId).ToList();
                    for (int i = 0; i < itemHistoryDbList.Count(); i++) {
                        ItemHistory itemHist = new ItemHistory() {
                            itemId = itemHistoryDbList[i].itemId,
                            changedByPersonId = itemHistoryDbList[i].changedByPersonId,
                            changeDateTime = itemHistoryDbList[i].changeDateTime,
                            comment = itemHistoryDbList[i].comment,
                            name = itemHistoryDbList[i].name,
                            description = itemHistoryDbList[i].description,
                            price = itemHistoryDbList[i].price,
                            rabatt = itemHistoryDbList[i].rabatt,
                            amount = itemHistoryDbList[i].amount,
                            subCategory = itemHistoryDbList[i].subCategory.name
                        };
                        itemHistoryList.Add(itemHist);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return itemHistoryList;
            }
        }

        public List<ItemHistory> GetDeletedItems() {
            using (var db = new UserContext()) {
                List<ItemHistory> itemHistoryList = new List<ItemHistory>();
                try {
                    List<ItemHistoryDb> itemHistoryDbList = db.itemsHistory.Where(h => h.comment == "Deleted").ToList();
                    for (int i = 0; i < itemHistoryDbList.Count(); i++) {
                        ItemHistory itemHist = new ItemHistory() {
                            itemId = itemHistoryDbList[i].itemId,
                            changedByPersonId = itemHistoryDbList[i].changedByPersonId,
                            changeDateTime = itemHistoryDbList[i].changeDateTime,
                            comment = itemHistoryDbList[i].comment,
                            name = itemHistoryDbList[i].name,
                            description = itemHistoryDbList[i].description,
                            price = itemHistoryDbList[i].price,
                            rabatt = itemHistoryDbList[i].rabatt,
                            amount = itemHistoryDbList[i].amount,
                            subCategory = itemHistoryDbList[i].subCategory.name
                        };
                        itemHistoryList.Add(itemHist);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return itemHistoryList;
            }
        }
        
        public int Create(Item item) {
            using (var db = new UserContext()) {
                try {
                    bool itemExists = db.items.Any(i => i.name == item.name);
                    if (itemExists) return 0;

                    ItemDb itemDb = new ItemDb() {
                        name = item.name,
                        image = item.image,
                        description = item.description,
                        subCategory = db.subCategories.First(s => s.name == item.subCategory),
                        price = item.price,
                        rabatt = item.rabatt,
                        amount = item.amount
                    };

                    db.items.Add(itemDb);
                    db.SaveChanges();
                    return db.items.OrderByDescending(i => i.itemId).FirstOrDefault().itemId;
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                    return 0;
                }
            }
        }

        public void Edit(int id, Item newItem) {
            using (var db = new UserContext()) {
                try {
                    ItemDb oldItem = db.items.First(i => i.itemId == id);
                    oldItem.name = newItem.name;
                    oldItem.description = newItem.description;
                    oldItem.price = newItem.price;
                    oldItem.rabatt = newItem.rabatt;
                    oldItem.amount = newItem.amount;
                    oldItem.subCategory = db.subCategories.First(s => s.name == newItem.subCategory);

                    db.Entry(oldItem).State = EntityState.Modified;
                    db.SaveChanges();
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
            }
        }

        public void Delete(int id) {
            using (var db = new UserContext()) {
                try {
                    ItemDb itemDb = db.items.Find(id);
                    db.items.Remove(itemDb);
                    db.SaveChanges();
                }
                catch (InvalidOperationException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
            }
        }

        public void SaveItemHistory(Item oldItem, int changedByPersonIdIn, string changeComment) {
            using (var db = new UserContext()) {
                try {
                    ItemHistoryDb itemHistoryDb = new ItemHistoryDb() {
                        itemId = oldItem.itemId,
                        name = oldItem.name,
                        description = oldItem.description,
                        price = oldItem.price,
                        rabatt = oldItem.rabatt,
                        amount = oldItem.amount,
                        subCategory = db.subCategories.First(s => s.name == oldItem.subCategory),
                        changeDateTime = DateTime.Now,
                        changedByPersonId = changedByPersonIdIn,
                        comment = changeComment
                    };

                    db.itemsHistory.Add(itemHistoryDb);
                    db.SaveChanges();
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
            }
        }
    }
}