using PersonalAccountOfStudent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.TablesGateway
{
    public static class UserGateway
    {
        public static User FindtUser(SchoolContext context, string login, string password)
        {
            return context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public static void InsertUser(SchoolContext context, User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public static void UpdateUser(SchoolContext context, User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }

        public static void DeleteUser(SchoolContext context, User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }
}
