using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace DAL {
    
    public interface PersonInterface {
        User GetUser(int id);
        List<User> GetAllUsers();
        Person GetOnePerson(int id);
        List<Person> GetAllPersons();
        List<UserHistory> GetHistory(int userId);
        User LogIn(User user);
        int Create(User user);
        bool Create(Person person);
        void Edit(int id, User newUser);
        void AdminEdit(int id, User newUser);
        void DeactivateUser(int id);
        void SaveUserHistory(User oldUser, User changedBy, string comment);
        string encrypt(string text);
    }
}
