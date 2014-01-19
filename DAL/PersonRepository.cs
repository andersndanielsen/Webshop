using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.Entity;

namespace DAL {

    public class PersonRepository : PersonInterface{
        private string errorFile = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";

        public User GetUser(int id) {
            UserDb userDb;
            using (var db = new UserContext()) {
                try {
                    userDb = db.users.Find(id);
                    if (userDb != null) {
                        User foundUser = new User() {
                            id = userDb.id,
                            userName = userDb.userName,
                            isAdmin = userDb.isAdmin,
                            firstName = userDb.firstName,
                            surName = userDb.surName,
                            address = userDb.address,
                            telephoneNumber = userDb.telephoneNumber,
                            postcode = userDb.poscodeArea.postcode,
                            postcodeArea = userDb.poscodeArea.postcodeArea
                        };
                        return foundUser;
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

        public List<User> GetAllUsers() {
            using (var db = new UserContext()) {
                List<User> userList = new List<User>();
                try {
                    List<UserDb> userDbList = db.users.ToList();
                    foreach (UserDb userDb in userDbList) {
                        if (userDb.userName != null) {
                            User foundUser = new User() {
                                id = userDb.id,
                                userName = userDb.userName,
                                isAdmin = userDb.isAdmin,
                                firstName = userDb.firstName,
                                surName = userDb.surName,
                                address = userDb.address,
                                telephoneNumber = userDb.telephoneNumber,
                                postcode = userDb.poscodeArea.postcode,
                                postcodeArea = userDb.poscodeArea.postcodeArea
                            };
                            userList.Add(foundUser);
                        }
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return userList;
            }
        }

        public Person GetOnePerson(int id){
            PersonDb personDb;
            using (var db = new UserContext()) {
                try {
                    personDb = db.persons.Find(id);
                    if (personDb != null) {
                        Person foundPerson = new Person() {
                            id = personDb.id,
                            firstName = personDb.firstName,
                            surName = personDb.surName,
                            address = personDb.address,
                            telephoneNumber = personDb.telephoneNumber,
                            postcode = personDb.poscodeArea.postcode,
                            postcodeArea = personDb.poscodeArea.postcodeArea
                        };
                        return foundPerson;
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

        public List<Person> GetAllPersons() {
            using (var db = new UserContext()) {
                List<Person> personList = new List<Person>();
                try {
                    List<PersonDb> personDbList = db.persons.ToList();
                    foreach (PersonDb personDb in personDbList) {
                        try {
                            UserDb userDb = db.users.Find(personDb.id);
                        }
                        catch {
                            Person foundPerson = new Person() {
                                id = personDb.id,
                                firstName = personDb.firstName,
                                surName = personDb.surName,
                                address = personDb.address,
                                telephoneNumber = personDb.telephoneNumber,
                                postcode = personDb.poscodeArea.postcode,
                                postcodeArea = personDb.poscodeArea.postcodeArea
                            };
                            personList.Add(foundPerson);
                        }
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return personList;
            }
        }

        public List<UserHistory> GetHistory(int userId) {
            using (var db = new UserContext()) {
                List<UserHistory> userHistoryList = new List<UserHistory>();
                try {
                    List<UserHistoryDb> userHistoryDbList = db.usersHistory.OrderByDescending(o => o.id).Where(h => h.id == userId).ToList();
                    for (int i = 0; i < userHistoryDbList.Count(); i++) {
                        UserHistory userHist = new UserHistory() {
                            id = userHistoryDbList[i].id,
                            changedByPersonId = userHistoryDbList[i].changedById,
                            changeDateTime = userHistoryDbList[i].changeDateTime,
                            comment = userHistoryDbList[i].comment,
                            firstName = userHistoryDbList[i].firstName,
                            surName = userHistoryDbList[i].surName,
                            address = userHistoryDbList[i].address,
                            postcode = userHistoryDbList[i].poscodeArea.postcode,
                            postcodeArea = userHistoryDbList[i].poscodeArea.postcodeArea,
                            telephoneNumber = userHistoryDbList[i].telephoneNumber,
                            isAdmin = userHistoryDbList[i].isAdmin
                        };
                        userHistoryList.Add(userHist);
                    }
                }
                catch (ArgumentNullException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                catch (OverflowException e) {
                    var sw = new System.IO.StreamWriter(errorFile, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                    sw.Close();
                }
                return userHistoryList;
            }
        }

        public User LogIn(User user) {
            OrderRepository orderDal = new OrderRepository();
            using (var db = new UserContext()) {
                string userName = user.userName;
                string password = encrypt(user.password);
                try {
                    bool userOK = db.users.Any(u => u.userName == userName && u.password == password);

                    if (userOK) {
                        UserDb userDb = db.users.First(u => u.userName == userName && u.password == password);
                        if (userDb != null) {
                            User foundUser = new User() {
                                id = userDb.id,
                                userName = userDb.userName,
                                firstName = userDb.firstName,
                                surName = userDb.surName,
                                address = userDb.address,
                                telephoneNumber = userDb.telephoneNumber,
                                postcode = userDb.poscodeArea.postcode,
                                postcodeArea = userDb.poscodeArea.postcodeArea,
                                isAdmin = userDb.isAdmin
                            };
                            foundUser.orders = orderDal.GetPersonsOrders(foundUser.id);
                            return foundUser;
                        }
                    }
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
                return null;
            }
        }

        public int Create(User user) {
            using (var db = new UserContext()) {
                try {
                    bool userExists = db.users.Any(u => u.userName == user.userName);
                    if (userExists) return 0;

                    UserDb newUser = new UserDb() {
                        firstName = user.firstName,
                        surName = user.surName,
                        telephoneNumber = user.telephoneNumber,
                        address = user.address,
                        userName = user.userName,
                        password = encrypt(user.password)
                    };

                    bool postCodeExists = db.postCodeAreas.Any(p => p.postcode == user.postcode);
                    if (postCodeExists) {
                        PostcodeAreaDb foundPCA = db.postCodeAreas.First(p => p.postcode == user.postcode);
                        newUser.poscodeArea = foundPCA;
                    }
                    else {
                        PostcodeAreaDb newPostCodeArea = new PostcodeAreaDb();
                        newPostCodeArea.postcode = user.postcode;
                        newPostCodeArea.postcodeArea = user.postcodeArea;
                        newUser.poscodeArea = newPostCodeArea;
                    }
                    db.users.Add(newUser);
                    db.SaveChanges();
                    return db.users.OrderByDescending(u => u.id).FirstOrDefault().id;
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

        public bool Create(Person person) {
            using (var db = new UserContext()) {
                PersonDb newPerson = new PersonDb() {
                    id = person.id,
                    firstName = person.firstName,
                    surName = person.surName,
                    telephoneNumber = person.telephoneNumber,
                    address = person.address
                };
                try {
                    bool postCodeExists = db.postCodeAreas.Any(p => p.postcode == person.postcode);
                    if (postCodeExists) {
                        PostcodeAreaDb foundPCA = db.postCodeAreas.First(p => p.postcode == person.postcode);
                        newPerson.poscodeArea = foundPCA;
                    }
                    else {
                        PostcodeAreaDb newPostCodeArea = new PostcodeAreaDb();
                        newPostCodeArea.postcode = person.postcode;
                        newPostCodeArea.postcodeArea = person.postcodeArea;
                        newPerson.poscodeArea = newPostCodeArea;
                    }
                    db.persons.Add(newPerson);
                    db.SaveChanges();
                    return true;
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
                return false;
            }
        }

        public void Edit(int id, User newUser) {
            using (var db = new UserContext()) {
                try {
                    UserDb oldUser = db.users.First(u => u.id == id);
                    oldUser.firstName = newUser.firstName;
                    oldUser.surName = newUser.surName;
                    oldUser.telephoneNumber = newUser.telephoneNumber;
                    oldUser.address = newUser.address;
                    oldUser.password = encrypt(newUser.password);

                    bool postCodeExists = db.postCodeAreas.Any(p => p.postcode == newUser.postcode);
                    if (postCodeExists) {
                        PostcodeAreaDb foundPCA = db.postCodeAreas.First(p => p.postcode == newUser.postcode);
                        oldUser.poscodeArea = foundPCA;
                    }
                    else {
                        PostcodeAreaDb newPostCodeArea = new PostcodeAreaDb();
                        newPostCodeArea.postcode = newUser.postcode;
                        newPostCodeArea.postcodeArea = newUser.postcodeArea;
                        oldUser.poscodeArea = newPostCodeArea;
                    }

                    db.Entry(oldUser).State = EntityState.Modified;
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

        public void AdminEdit(int id, User newUser) {
            using (var db = new UserContext()) {
                try {
                    UserDb oldUser = db.users.First(u => u.id == id);
                    oldUser.firstName = newUser.firstName;
                    oldUser.surName = newUser.surName;
                    oldUser.telephoneNumber = newUser.telephoneNumber;
                    oldUser.address = newUser.address;
                    oldUser.isAdmin = newUser.isAdmin;

                    bool postCodeExists = db.postCodeAreas.Any(p => p.postcode == newUser.postcode);
                    if (postCodeExists) {
                        PostcodeAreaDb foundPCA = db.postCodeAreas.First(p => p.postcode == newUser.postcode);
                        oldUser.poscodeArea = foundPCA;
                    }
                    else {
                        PostcodeAreaDb newPostCodeArea = new PostcodeAreaDb();
                        newPostCodeArea.postcode = newUser.postcode;
                        newPostCodeArea.postcodeArea = newUser.postcodeArea;
                        oldUser.poscodeArea = newPostCodeArea;
                    }

                    db.Entry(oldUser).State = EntityState.Modified;
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

        public void DeactivateUser(int id) {
            using (var db = new UserContext()) {
                try {
                    UserDb user = db.users.First(u => u.id == id);
                    user.userName = null;
                    user.password = null;

                    db.Entry(user).State = EntityState.Modified;
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

        public void SaveUserHistory(User oldUser, User changedBy, string commentIn) {
            using (var db = new UserContext()) {
                UserHistoryDb userHistoryDb = new UserHistoryDb() {
                    firstName = oldUser.firstName,
                    surName = oldUser.surName,
                    address = oldUser.address,
                    telephoneNumber = oldUser.telephoneNumber,
                    changeDateTime = DateTime.Now,
                    changedByPersonDb = db.users.Find(changedBy.id),
                    personDb = db.users.Find(oldUser.id),
                    isAdmin = oldUser.isAdmin,
                    comment = commentIn
                };
                try {
                    bool postCodeExists = db.postCodeAreas.Any(p => p.postcode == oldUser.postcode);
                    if (postCodeExists) {
                        PostcodeAreaDb foundPCA = db.postCodeAreas.First(p => p.postcode == oldUser.postcode);
                        userHistoryDb.poscodeArea = foundPCA;
                    }
                    db.usersHistory.Add(userHistoryDb);
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

        public string encrypt(string text) {
            try {
                var algorithm = System.Security.Cryptography.SHA256.Create();
                byte[] inData, outData;
                inData = System.Text.Encoding.ASCII.GetBytes(text);
                outData = algorithm.ComputeHash(inData);

                return Convert.ToBase64String(outData);
            }
            catch (ArgumentNullException e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " " + e.InnerException);
                sw.Close();
            }
            catch (Exception e) {
                var sw = new System.IO.StreamWriter(errorFile, true);
                sw.WriteLine(DateTime.Now.ToString() + " " + e.Message + " EncryptionError");
                sw.Close();
            }
            return "EncryptionError";
        }
    }
}
