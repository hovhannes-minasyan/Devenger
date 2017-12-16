using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
   
    public class Contact
    {
        private static DatabaseConnection MyDatabase = new DatabaseConnection();


        public string Username;
        public string Name;
        public int Id;
        List<string> items = new List<string>();
        public Contact(int id)
        {
            Id = id;
            MyDatabase.RetrieveContact("People", "ID", id, ref Username, ref Name);
        }

        public Contact(string username)
        {
            Username = username;
            MyDatabase.RetrieveContact("People", "Username", username, ref Id, ref Name);
        }


        public static void Initialize(DatabaseConnection conntection)
        {
            MyDatabase=conntection;
            //MyDatabase.RetrieveInfo("People", "Username", ref Usernames);
        }
    }
    }

