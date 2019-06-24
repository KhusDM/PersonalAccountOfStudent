using PersonalAccountOfStudent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAccountOfStudent.TablesGateway
{
    public static class UserGateway
    {
        public static User FindUser(SchoolContext context, string login, string password)
        {
            return context.Users.FirstOrDefault(user => user.Login == login && user.Password == password);
        }

        public static void InsertUser(SchoolContext context, string userGuid, string login, string password, string userType = null, string avatar = null)
        {
            var user = new User() { GUID = userGuid, Login = login, Password = password, UserType = userType, Avatar = avatar };
            context.Users.Add(user);
            context.SaveChanges();
        }

        public static void UpdateUser(SchoolContext context, string userGuid, string login, string password, string userType = null, string avatar = null)
        {
            var user = new User();

            if (userType != null)
            {
                user.UserType = userType;
            }

            if (avatar != null)
            {
                user.Avatar = avatar;
            }

            context.Users.Update(user);
            context.SaveChanges();
        }

        public static void DeleteUser(SchoolContext context, string userGuid, string login, string password, string userType = null, string avatar = null)
        {
            var user = new User() { GUID = userGuid, Login = login, Password = password, UserType = userType, Avatar = avatar };
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }
}
