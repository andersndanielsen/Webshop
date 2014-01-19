using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BLL
{
    public class PersonLogic{

        private PersonInterface personDal;
        private string _stub;

        public PersonLogic(string stub)
        {
            _stub = stub;
            if (stub=="Test")
            {
                personDal = new PersonRepositoryStub();
            }
            else
            {
                personDal = new PersonRepository();
            }
        }

        public List<User> GetAllUsers() {
            return personDal.GetAllUsers();
        }
        
        public List<Person> GetAllPersons() {
            return personDal.GetAllPersons();
        }

        public Person GetPerson(int id) {
            Person person = personDal.GetUser(id);
            
            if(person == null){
                person = personDal.GetOnePerson(id);
            }
            OrderLogic orderBll = new OrderLogic(_stub);
            if (person != null) {
                person.orders = orderBll.GetPersonsOrders(person.id);
            }
            return person;
        }

        public List<UserHistory> GetHistory(int userId) {
            return personDal.GetHistory(userId);
        }

        public User LogIn(User user) {
            return personDal.LogIn(user);
        }

        public int Create(User user) {
            return personDal.Create(user);
        }

        public bool Create(Person person) {
            return personDal.Create(person);
        }

        public void Edit(int id, User newUser) {
            personDal.Edit(id, newUser);
        }
        
        public void AdminEdit(int id, User newUser) {
            personDal.AdminEdit(id, newUser);
        }

        public void DeactivateUser(int id) {
            personDal.DeactivateUser(id);
        }

        public void SaveUserHistory(User oldUser, User changedBy, string comment) {
            personDal.SaveUserHistory(oldUser, changedBy, comment);
        }
    }
}
