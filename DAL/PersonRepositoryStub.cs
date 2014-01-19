using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL {
    public class PersonRepositoryStub : PersonInterface {
        public User GetUser(int id) {
            if (id == 0) {
                return null;
            }
            User newUser = null;

            if (id == 1) {
                newUser = new User {
                    id = 1,
                    firstName = "Arne",
                    surName = "Arnesen",
                    telephoneNumber = "12345678",
                    address = "Arnevei 32",
                    postcode = 1182,
                    postcodeArea = "Oslo",
                    isAdmin = false,
                    userName = "arnesen",
                    password = "1234"
                };
            }

            return newUser;
        }

        public List<User> GetAllUsers() {
            List<UserDb> userDbList = new List<UserDb>();
            List<User> userList = new List<User>();

            User foundUser = new User() {
                id = 1,
                firstName = "Arne",
                surName = "Arnesen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo",
                isAdmin = false,
                userName = "arnesen",
                password = "1234"
            };
            userList.Add(foundUser);
            userList.Add(foundUser);
            userList.Add(foundUser);

            return userList;
        }

        public Person GetOnePerson(int id) {
            if (id == 0) {
                return null;
            }
            Person newPerson = null;

            if (id == 2) {
                newPerson = new Person {
                    id = 2,
                    firstName = "Geir",
                    surName = "Børresen",
                    telephoneNumber = "12345678",
                    address = "Arnevei 32",
                    postcode = 1182,
                    postcodeArea = "Oslo"
                };
            }
            return newPerson;
        }

        public List<Person> GetAllPersons() {
            List<Person> personList = new List<Person>();

            Person foundPerson = new Person() {
                id = 2,
                firstName = "Geir",
                surName = "Børresen",
                telephoneNumber = "12345678",
                address = "Arnevei 32",
                postcode = 1182,
                postcodeArea = "Oslo"
            };
            personList.Add(foundPerson);
            personList.Add(foundPerson);
            personList.Add(foundPerson);

            return personList;
        }

        public List<UserHistory> GetHistory(int itemId) {
            List<UserHistory> userHistoryList = new List<UserHistory>();
            if (itemId == 6) {
                UserHistory userHist = new UserHistory() {
                    id = 6,
                    changedByPersonId = 2,
                    changeDateTime = DateTime.MaxValue,
                    comment = "Deleted",
                    firstName = "Arne",
                    surName = "Arnesen",
                    telephoneNumber = "12345678",
                    address = "Arnevei 32",
                    postcode = 1182,
                    postcodeArea = "Oslo",
                    isAdmin = false
                };
                userHistoryList.Add(userHist);
                userHistoryList.Add(userHist);
                userHistoryList.Add(userHist);
            }
            return userHistoryList;
        }

        public User LogIn(User user) {
            if (user == null) {
                return null;
            }

            bool userOk = (!user.userName.Equals("")) ? true : false;

            if (userOk) {
                User foundUser = new User() {
                    id = user.id,
                    userName = user.userName,
                    firstName = user.firstName,
                    surName = user.surName,
                    address = user.address,
                    telephoneNumber = user.telephoneNumber,
                    postcode = user.postcode,
                    postcodeArea = user.postcodeArea,
                    isAdmin = false
                };
                return foundUser;
            }
            return null;
        }

        public int Create(User user) {
            int userExists = (user.userName == "svein@svein.no") ? 0 : 1;
            return userExists;
        }

        public bool Create(Person person) {
            return true;
        }

        public void Edit(int id, User newUser) {
        }

        public void AdminEdit(int id, User newUser) {
        }

        public void DeactivateUser(int id) { 
        }

        public void SaveUserHistory(User oldUser, User changedBy, string comment) {
        }

        public string encrypt(string text) {
            var algorithm = System.Security.Cryptography.SHA256.Create();
            byte[] inData, outData;
            inData = System.Text.Encoding.ASCII.GetBytes(text);
            outData = algorithm.ComputeHash(inData);

            string convertedText = Convert.ToBase64String(outData);

            return text.Equals(convertedText) ? "Uforandret" : "Forandret";
        }
    }
}
