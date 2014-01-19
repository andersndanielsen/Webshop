using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL {

    public class PersonDb {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string surName { get; set; }
        public string telephoneNumber { get; set; }
        public string address { get; set; }

        public virtual PostcodeAreaDb poscodeArea { get; set; }
    }

    public class PersonHistoryDb {
        [Key, Column(Order = 0)]
        [ForeignKey("personDb")]
        public int id { get; set; }

        [Key, Column(Order = 1)]
        [DataType(DataType.Date)]
        public DateTime changeDateTime { get; set; }

        [ForeignKey("changedByPersonDb")]
        public int changedById { get; set; }
        public string comment { get; set; }
        public string firstName { get; set; }
        public string surName { get; set; }
        public string telephoneNumber { get; set; }
        public string address { get; set; }
        
        public virtual PersonDb personDb { get; set; }
        public virtual PersonDb changedByPersonDb { get; set; }
        public virtual PostcodeAreaDb poscodeArea { get; set; }
    }

    public class UserDb : PersonDb {
        public string userName { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
    }

    public class UserHistoryDb : PersonHistoryDb {
        public bool isAdmin { get; set; }
    }

    public class PostcodeAreaDb { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int postcode { get; set; }
        public string postcodeArea { get; set; }
    }

    public class OrderDb {
        [Key]
        public int orderId { get; set; }

        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public bool orderSent { get; set; }

        public virtual PersonDb person { get; set; }
    }

    public class OrderItemDb {
        [Key, Column(Order = 0)]
        [ForeignKey("orderDb")]
        public int orderId { get; set; }
        public virtual OrderDb orderDb { get; set; }
        
        [Key, Column(Order = 1)]
        [ForeignKey("itemDb")]
        public int itemId { get; set; }
        public virtual ItemDb itemDb { get; set; }
        public int amount { get; set; }
    }

    public class ItemDb {
        [Key]
        public int itemId { get; set; }
        public byte[] image { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int rabatt { get; set; }   //has value if the item is on sale
        public int amount { get; set; }

        public virtual SubCategoryDb subCategory { get; set; }
    }

    public class ItemHistoryDb {
        [Key, Column(Order = 0)]
        public int itemId { get; set; }

        [Key, Column(Order = 1)]
        [DataType(DataType.Date)]
        public DateTime changeDateTime { get; set; }
        public int changedByPersonId { get; set; }
        public string comment { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int rabatt { get; set; }   //has value if the item is on sale
        public int amount { get; set; }

        public virtual SubCategoryDb subCategory { get; set; }
    }

    public class SubCategoryDb {
        [Key]
        public int id { get; set; }
        public string name { get; set; }

        public virtual CategoryDb category { get; set; }
    }

    public class CategoryDb {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }

    public class UserContext : DbContext {
        public UserContext()
            : base("name=UserContext19") {
            Database.CreateIfNotExists();
        }

        public DbSet<PersonDb> persons { get; set; }
        public DbSet<PersonHistoryDb> personsHistory { get; set; }
        public DbSet<UserDb> users { get; set; }
        public DbSet<UserHistoryDb> usersHistory { get; set; }
        public DbSet<PostcodeAreaDb> postCodeAreas { get; set; }
        public DbSet<OrderDb> orders { get; set; }
        public DbSet<OrderItemDb> orderItems { get; set; }
        public DbSet<ItemDb> items { get; set; }
        public DbSet<ItemHistoryDb> itemsHistory { get; set; }
        public DbSet<SubCategoryDb> subCategories { get; set; }
        public DbSet<CategoryDb> categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}