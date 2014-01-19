using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model {

    public class Person {
        public int id { get; set; }
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Fornavnet må bestå av minst 2 tegn")]
        [Required(ErrorMessage = "Fornavn må oppgis")]
        [Display(Name = "Fornavn")]
        public string firstName { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = "Etternavnet må bestå av minst 2 tegn")]
        [Required(ErrorMessage = "Etternavn må oppgis")]
        [Display(Name = "Etternavn")]
        public string surName { get; set; }

        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Telefonnummeret må bestå av 8 siffer")]
        [Required(ErrorMessage = "Telefonnummer må oppgis")]
        [Display(Name = "Telefonnummer")]
        public string telephoneNumber { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = "Adressen kan ikke være lengre enn 200 tegn")]
        [Required(ErrorMessage = "Adresse må oppgis")]
        [Display(Name = "Adresse")]
        public string address { get; set; }

        [RegularExpression(@"[0-9]{4}", ErrorMessage = "Postnummeret må bestå av 4 siffer")]
        [Required(ErrorMessage = "Postnummer må oppgis")]
        [Display(Name = "Postnummer")]
        public int postcode { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = "Poststedet må bestå av minst 2 tegn")]
        [Required(ErrorMessage = "Poststed må oppgis")]
        [Display(Name = "Poststed")]
        public string postcodeArea { get; set; }

        public virtual List<Order> orders { get; set; }

        public override string ToString() {
            return firstName + surName + telephoneNumber + address + postcode + postcodeArea;
        }
    }

    public class PersonHistory {
        public int id { get; set; }

        [Display(Name = "Tidspunkt")]
        public DateTime changeDateTime { get; set; }
        public int changedByPersonId { get; set; }

        [Display(Name = "Handling")]
        public string comment { get; set; }

        [Display(Name = "Fornavn")]
        public string firstName { get; set; }

        [Display(Name = "Etternavn")]
        public string surName { get; set; }

        [Display(Name = "Telefonnummer")]
        public string telephoneNumber { get; set; }

        [Display(Name = "Adresse")]
        public string address { get; set; }

        [Display(Name = "Postnummer")]
        public int postcode { get; set; }

        [Display(Name = "Poststed")]
        public string postcodeArea { get; set; }
    }

    public class User : Person {
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "E-postadressen har ikke gyldig format")]
        [Required(ErrorMessage = "E-postadressen blir ditt brukernavn og må oppgis")]
        [Display(Name = "E-post (brukernavn)")]
        public string userName { get; set; }

        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "Passordet må bestå av 8 tegn, hvorav minst en stor bokstav og minst ett tall")]
        [Required(ErrorMessage = "Passord må oppgis")]
        [Display(Name = "Passord")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "De oppgitte passordene må være like")]
        [Required(ErrorMessage = "Passord må gjentas")]
        [Display(Name = "Gjenta passord")]
        [DataType(DataType.Password)]
        public string secondPassword { get; set; }

        [Display(Name = "Er admin")]
        public bool isAdmin { get; set; }
    }

    public class UserHistory : PersonHistory {
        [Display(Name = "Er admin")]
        public bool isAdmin { get; set; }
    }

    public class Order {
        public int orderId { get; set; }
        public int personId { get; set; }

        [Display(Name = "Dato")]
        public DateTime date { get; set; }
        public bool orderSent { get; set; }

        public virtual List<OrderItem> orderItems { get; set; }
    }

    public class OrderItem {
        public int orderId { get; set; }
        public int amount { get; set; }

        public virtual Item item { get; set; }
    }

    public class Item {
        public int itemId { get; set; }

        public byte[] image { get; set; }

        [Display(Name = "Navn")]
        [Required(ErrorMessage = "Varen må ha et navn")]
        public string name { get; set; }

        [Display(Name = "Beskrivelse")]
        [Required(ErrorMessage = "Varen må ha en beskrivelse")]
        public string description { get; set; }

        [Display(Name = "Pris")]
        [Required(ErrorMessage = "Varen må ha en pris")]
        public int price { get; set; }

        [Display(Name = "Rabatt")]
        [Required(ErrorMessage = "Rabatt i kr må oppgis")]
        public int rabatt { get; set; }   //has value if the item is on sale

        [Display(Name = "Antall")]
        [Required(ErrorMessage = "Antall i beholdning må oppgis")]
        public int amount { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Du må velge en kategori")]
        public string subCategory { get; set; }

        public override string ToString() {
            return name + description + price + rabatt + amount + subCategory;
        }
    }

    public class ItemHistory {
        public int itemId { get; set; }

        [Display(Name = "Tidspunkt")]
        public DateTime changeDateTime { get; set; }
        public int changedByPersonId { get; set; }

        [Display(Name = "Handling")]
        public string comment { get; set; }

        [Display(Name = "Varenavn")]
        public string name { get; set; }

        [Display(Name = "Beskrivelse")]
        public string description { get; set; }

        [Display(Name = "Pris")]
        public int price { get; set; }

        [Display(Name = "Rabatt")]
        public int rabatt { get; set; }

        [Display(Name = "Antall")]
        public int amount { get; set; }

        [Display(Name = "Kategori")]
        public string subCategory { get; set; }
    }

    public class SubCategory {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Category {
        public int id { get; set; }
        public string name { get; set; }
    }
}